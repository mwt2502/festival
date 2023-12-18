using festival.Server.Interfaces;
using festival.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace festival.Client.Services
{
    public class ShiftServiceClient : IShiftService
    {
        private readonly HttpClient _httpClient;

        public ShiftServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Shift>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Shift>>("api/shift");
        }

        public async Task<Shift> GetByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<Shift>($"api/shift/{id}");
        }

        public async Task CreateAsync(Shift shift)
        {
            var response = await _httpClient.PostAsJsonAsync("api/shift", shift);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(string id, Shift shiftIn)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/shift/{id}", shiftIn);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/shift/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
