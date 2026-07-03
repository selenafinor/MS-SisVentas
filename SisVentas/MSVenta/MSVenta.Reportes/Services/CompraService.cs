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
            return JsonSerializer.Deserialize<List<NotaCompraDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}