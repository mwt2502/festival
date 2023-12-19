﻿using festival.Server.Interfaces;
using festival.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace festival.Client.Services
{
    public class ShiftServiceClient : IShiftService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ShiftServiceClient(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<List<Shift>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("api/shift");
            var responseContent = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<Shift>>(responseContent, _jsonSerializerOptions)
                ?? new List<Shift>(); // Return an empty list if null is returned.
        }

        public async Task<Shift> GetByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"api/shift/{id}");
            var responseContent = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<Shift>(responseContent, _jsonSerializerOptions)
                ?? new Shift(); // Return a new Shift if null is returned.
        }

        public async Task CreateAsync(Shift shift)
        {
            var content = new StringContent(JsonSerializer.Serialize(shift, _jsonSerializerOptions), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/shift", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(string id, Shift shiftIn)
        {
            var content = new StringContent(JsonSerializer.Serialize(shiftIn, _jsonSerializerOptions), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/shift/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"api/shift/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task AssignVolunteer(string shiftId, string volunteerId)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { volunteerId }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/shift/{shiftId}/assign", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Kunne ikke tildele vagten: {errorResponse}");
            }
        }
    }
}
