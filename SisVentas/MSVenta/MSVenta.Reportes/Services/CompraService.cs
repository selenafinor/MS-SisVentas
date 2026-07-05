using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MSVenta.Reportes.DTOs;
namespace MSVenta.Reportes.Services
{
    public class CompraService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly JsonSerializerOptions _jsonOptions =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public CompraService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<List<NotaCompraDto>> GetNotasCompraAsync()
        {
            var client = _httpClientFactory.CreateClient("Compras");
            var response = await client.GetAsync("/api/notacompra");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<NotaCompraDto>>(json, _jsonOptions);
        }

        // NUEVO: para el dashboard — "órdenes pendientes" viene de OrdenCompra, no de NotaCompra.
        public async Task<List<OrdenCompraDto>> GetOrdenesCompraAsync()
        {
            var client = _httpClientFactory.CreateClient("Compras");
            var response = await client.GetAsync("/api/ordencompra");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<OrdenCompraDto>>(json, _jsonOptions);
        }
    }
}