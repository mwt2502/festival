using festival.Server.DataService;
using festival.Server.Interfaces;
using festival.Server.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IVolunteerService, VolunteerService>();
// builder.Services.AddScoped<ICoordinatorService, CoordinatorService>(); Ikke brugt i denne version
builder.Services.AddScoped<IShiftService, ShiftService>();

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder =>
    {
        builder.WithOrigins("http://example.com") // Erstat med den faktiske oprindelse, hvis nødvendigt
               .AllowAnyHeader()
               .AllowAnyMethod(); // Tillader alle metoder, inklusive POST
    });
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
    app.UseCors("CORSPolicy");

    app.UseSwagger(); // Tilføj Swagger middleware
    app.UseSwaggerUI(); // Tilføj Swagger UI middleware
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
