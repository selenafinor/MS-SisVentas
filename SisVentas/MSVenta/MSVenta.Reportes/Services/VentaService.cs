using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using MSVenta.Reportes.DTOs;

namespace MSVenta.Reportes.Services
{
    public class VentaService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VentaService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<NotaVentaDto>> GetNotasVentaAsync()
        {
            var client = _httpClientFactory.CreateClient("Ventas");
            var response = await client.GetAsync("/api/venta");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<NotaVentaDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}