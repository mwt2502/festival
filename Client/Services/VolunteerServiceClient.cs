using festival.Server.Interfaces;
using festival.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace festival.Client.Services
{
    public class VolunteerServiceClient : IVolunteerService
    {
        private readonly HttpClient _httpClient;

        public VolunteerServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Volunteer>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Volunteer>>("api/volunteer");
        }

        public async Task<Volunteer> GetByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<Volunteer>($"api/volunteer/{id}");
        }

        public async Task CreateAsync(Volunteer volunteer)
        {
            var response = await _httpClient.PostAsJsonAsync("api/volunteer", volunteer);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(string id, Volunteer volunteer)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/volunteer/{id}", volunteer);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/volunteer/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}