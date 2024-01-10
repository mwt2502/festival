using festival.Server.Interfaces;
using festival.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
        public async Task AssignShiftToVolunteer(string shiftId, string volunteerId)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { volunteerId }), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/shift/{shiftId}/assign", content);
            if (!response.IsSuccessStatusCode)
            {
                // Fejlhåndtering: Udlæs fejlbesked og kast en exception eller log fejlen
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Kunne ikke tildele vagten: {errorResponse}");
            }
        }

        public async Task<List<Volunteer>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Volunteer>>("api/volunteer");
        }

        public async Task<Volunteer> GetByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<Volunteer>($"api/volunteer/{id}");
        }

        public async Task<Volunteer> CreateAsync(Volunteer volunteer)
        {
            // Send anmodningen til serveren og få respons
            var response = await _httpClient.PostAsJsonAsync("api/volunteer", volunteer);

            // Tjek at anmodningen var vellykket og returner den frivillige
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Volunteer>();
            }
            else
            {
                // Håndter fejl eller kast en undtagelse
                throw new HttpRequestException($"Failed to create volunteer: {response.ReasonPhrase}");
            }
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