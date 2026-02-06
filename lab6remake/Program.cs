using Microsoft.EntityFrameworkCore;
using lab6remake.Data;
using lab6remake.Repositories;
using lab6remake.Repositories.Interfaces;
using lab6remake.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection - Repository Pattern (Bài 2)
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Dependency Injection - HttpClient và Service cho Web Client (Bài 3)
builder.Services.AddHttpClient<IProductApiService, ProductApiService>(client =>
{
    // HttpClient sẽ gọi chính API của project này
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7000";
    client.BaseAddress = new Uri(baseUrl);
});

// Add MVC Controllers + Views (cho Bài 3)
builder.Services.AddControllersWithViews();

// Add API Controllers (cho Bài 1 và 2)
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Lab 6 Remake API",
        Version = "v1",
        Description = "API cho Lab 6 - Lập trình Web Nâng Cao"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab 6 API V1");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map Controllers
app.MapControllers(); // Cho API Controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}"); // Cho MVC Controllers

app.Run();