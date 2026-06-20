using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using MSVenta.Venta.Models;
using System.Threading.Tasks;
using System.Linq;

namespace MSVenta.Venta.Services
{
    public class LibelulaResult
    {
        public bool Ok { get; set; }
        public string Mensaje { get; set; }
        public string IdTransaccion { get; set; }
        public string UrlPasarela { get; set; }
        public string QrUrl { get; set; }
    }

    public class LibelulaConsultaResult
    {
        public bool Ok { get; set; }
        public bool Pagado { get; set; }
        public string Mensaje { get; set; }
    }

    public class LibelulaService
    {
        private const string AppKey = "11bb10ce-68ba-4af1-8eb7-4e6624fed729";
        private const string BaseUrl = "https://api.libelula.bo/rest";
        private const string CallbackUrl = "https://triage-acronym-elixir.ngrok-free.dev/api/pago/callback";
        private const string RetornoUrl = "https://electronicapnp.cfd/ventas/notas";

        private readonly HttpClient _http;

        public LibelulaService(HttpClient http)
        {
            _http = http;
        }

        public async Task<LibelulaResult> RegistrarDeudaVenta(Models.Venta venta)
        {
            var lineas = new List<object>();

            if (venta.Detalles != null && venta.Detalles.Any())
            {
                foreach (var d in venta.Detalles)
                {
                    lineas.Add(new
                    {
                        concepto = d.NombreProducto ?? "Producto",
                        cantidad = (double)d.Cantidad,
                        costo_unitario = (double)d.PrecioUni,
                        descuento_unitario = 0
                    });
                }
            }
            else
            {
                lineas.Add(new
                {
                    concepto = $"Venta #{venta.Id}",
                    cantidad = 1,
                    costo_unitario = (double)venta.MontoTotal,
                    descuento_unitario = 0
                });
            }

            var fecha = venta.Fecha.ToString("yyyyMMdd");
            var hora = $"{venta.Hora.Hours:D2}{venta.Hora.Minutes:D2}{venta.Hora.Seconds:D2}";
            var identificador = $"VENTA-{venta.Id}-{fecha}-{hora}-{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";

            var emailCliente = venta.Cliente?.Correo;
            if (string.IsNullOrWhiteSpace(emailCliente))
                emailCliente = "sin-correo@electronica.bo";

            var payload = new
            {
                appkey = AppKey,
                descripcion = $"Venta #{venta.Id} - Electronica PNP",
                email_cliente = emailCliente,
                identificador = identificador,
                callback_url = CallbackUrl,
                url_retorno = RetornoUrl,
                lineas_detalle_deuda = lineas
            };

            try
            {
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await _http.PostAsync($"{BaseUrl}/deuda/registrar", content);
                var body = await resp.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(body);

                if (data.GetProperty("error").GetInt32() == 0)
                {
                    return new LibelulaResult
                    {
                        Ok = true,
                        IdTransaccion = identificador,
                        UrlPasarela = data.TryGetProperty("url_pasarela_pagos", out var up) ? up.GetString() : null,
                        QrUrl = data.TryGetProperty("qr_simple_url", out var qr) ? qr.GetString() : null,
                        Mensaje = "QR generado correctamente"
                    };
                }
                else
                {
                    var msg = data.TryGetProperty("mensaje", out var m) ? m.GetString() : "Error desconocido";
                    return new LibelulaResult { Ok = false, Mensaje = $"Error Libelula: {msg}" };
                }
            }
            catch (Exception ex)
            {
                return new LibelulaResult { Ok = false, Mensaje = $"No se pudo conectar: {ex.Message}" };
            }
        }

        public async Task<LibelulaConsultaResult> ConsultarPago(string identificador)
        {
            var payload = new { appkey = AppKey, identificador };

            try
            {
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var resp = await _http.PostAsync($"{BaseUrl}/deuda/consultar_deudas/por_identificador", content);
                var body = await resp.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<JsonElement>(body);

                if (data.GetProperty("error").GetInt32() == 0)
                {
                    bool pagado = false;
                    if (data.TryGetProperty("datos", out var datos) && datos.ValueKind != JsonValueKind.Null)
                    {
                        if (datos.TryGetProperty("estado_transaccion", out var est))
                        {
                            var estStr = est.ValueKind == JsonValueKind.String
                                ? est.GetString()
                                : est.GetRawText();
                            pagado = estStr == "PAGADO" || estStr == "COMPLETADO" || estStr == "2";
                        }
                        if (datos.TryGetProperty("pagado", out var pag) && pag.ValueKind == JsonValueKind.True)
                            pagado = true;
                    }
                    return new LibelulaConsultaResult { Ok = true, Pagado = pagado };
                }
                else
                {
                    var msg = data.TryGetProperty("mensaje", out var m) ? m.GetString() : "Error desconocido";
                    return new LibelulaConsultaResult { Ok = false, Pagado = false, Mensaje = $"Error al consultar: {msg}" };
                }
            }
            catch (Exception ex)
            {
                return new LibelulaConsultaResult { Ok = false, Pagado = false, Mensaje = $"No se pudo conectar: {ex.Message}" };
            }
        }
    }
}