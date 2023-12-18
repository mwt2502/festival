using festival.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using festival.Server.Interfaces;

namespace festival.Client.Services
{
    public class CoordinatorServiceClient : ICoordinatorService
    {
        private readonly HttpClient _httpClient;

        public CoordinatorServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Coordinator>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Coordinator>>("api/coordinator");
        }

        public async Task<Coordinator> GetByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<Coordinator>($"api/coordinator/{id}");
        }

        public async Task CreateAsync(Coordinator coordinator)
        {
            var response = await _httpClient.PostAsJsonAsync("api/coordinator", coordinator);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(string id, Coordinator coordinator)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/coordinator/{id}", coordinator);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/coordinator/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
