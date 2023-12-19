using Blazored.LocalStorage;
using festival.Client;
using festival.Client.Services;
using festival.Server.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Konfigurerer JSON serializeren til at bruge en string enum converter globalt
var jsonSerializerOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Converters = { new JsonStringEnumConverter() }
};

// Registrerer en HttpClient med baseadresse konfigureret til applikationens hostmilj�
// og anvender de tilpassede JsonSerializerOptions for alle anmodninger
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// Tilf�jer services for de forskellige interfaces i applikationen
builder.Services.AddScoped<IVolunteerService, VolunteerServiceClient>();
builder.Services.AddScoped<IShiftService, ShiftServiceClient>();
builder.Services.AddScoped<ICoordinatorService, CoordinatorServiceClient>();

// Tilf�jer local storage service til at gemme data lokalt i browseren
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddSingleton(jsonSerializerOptions);

// Bygger og k�rer applikationen
await builder.Build().RunAsync();
