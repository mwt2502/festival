using Blazored.LocalStorage;
using festival.Client;
using festival.Client.Services;
using festival.Server.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IVolunteerService, VolunteerServiceClient>();
builder.Services.AddScoped<IShiftService, ShiftServiceClient>();
builder.Services.AddScoped<ICoordinatorService, CoordinatorServiceClient>();
builder.Services.AddBlazoredLocalStorage();



await builder.Build().RunAsync();
