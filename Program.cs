using DashboardData.Components;
using DashboardData.Services;
using Microsoft.EntityFrameworkCore;
using DashboardData;
using DashboardData.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<ISensorService, SensorService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));
var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    if(!context.Sensors.Any())
    {
        Console.WriteLine("--- Generating Test Data ----");


        var lab = new Location {Name = "Lab",Building="Bldg. A"};
        var factory = new Location {Name = "Factory", Building="Bldg. B"};
        context.Locations.AddRange(lab, factory);

        var tagCritical= new Tag {Label = "Critical"};
        var tagMaintenance = new Tag {Label = "Maintenance"};
        context.Tags.AddRange(tagCritical, tagMaintenance);
        context.SaveChanges();

        var sensor1 = new SensorData {Name = "Sensor_Alpha",  Value=25.4, LocationId=lab.Id,Tags=new List<Tag> {tagCritical}};
        var sensor2 = new SensorData {Name = "Sensor_Beta",  Value=40.2, LocationId=factory.Id,Tags=new List<Tag> {tagMaintenance}};
        context.Sensors.AddRange(sensor1, sensor2);
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
