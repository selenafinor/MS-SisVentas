using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MSVenta.Reportes.DTOs;

namespace MSVenta.Reportes.Services
{
    public class InventarioService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly JsonSerializerOptions _jsonOptions =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public InventarioService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ArticuloReporteDto>> GetArticulosAsync()
        {
            var client = _httpClientFactory.CreateClient("Inventario");
            var response = await client.GetAsync("/api/articulo");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ArticuloReporteDto>>(json, _jsonOptions);
        }

        public async Task<List<IngresoDto>> GetIngresosAsync(bool incluirDetalle = false)
        {
            var client = _httpClientFactory.CreateClient("Inventario");
            var response = await client.GetAsync("/api/ingreso");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var ingresos = JsonSerializer.Deserialize<List<IngresoDto>>(json, _jsonOptions);

            if (incluirDetalle)
            {
                foreach (var ingreso in ingresos)
                {
                    ingreso.Detalles = await GetDetalleIngresoAsync(ingreso.Id);
                }
            }

            return ingresos;
        }

        public async Task<List<EgresoDto>> GetEgresosAsync(bool incluirDetalle = false)
        {
            var client = _httpClientFactory.CreateClient("Inventario");
            var response = await client.GetAsync("/api/egreso");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var egresos = JsonSerializer.Deserialize<List<EgresoDto>>(json, _jsonOptions);

            if (incluirDetalle)
            {
                foreach (var egreso in egresos)
                {
                    egreso.Detalles = await GetDetalleEgresoAsync(egreso.Id);
                }
            }

            return egresos;
        }

        private async Task<List<DetalleMovimientoDto>> GetDetalleIngresoAsync(int ingresoId)
        {
            var client = _httpClientFactory.CreateClient("Inventario");
            var response = await client.GetAsync($"/api/DetalleIngreso/ingreso/{ingresoId}");
            if (!response.IsSuccessStatusCode)
                return new List<DetalleMovimientoDto>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<DetalleMovimientoDto>>(json, _jsonOptions)
                ?? new List<DetalleMovimientoDto>();
        }

        private async Task<List<DetalleMovimientoDto>> GetDetalleEgresoAsync(int egresoId)
        {
            var client = _httpClientFactory.CreateClient("Inventario");
            var response = await client.GetAsync($"/api/DetalleEgreso/egreso/{egresoId}");
            if (!response.IsSuccessStatusCode)
                return new List<DetalleMovimientoDto>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<DetalleMovimientoDto>>(json, _jsonOptions)
                ?? new List<DetalleMovimientoDto>();
        }
    }
}