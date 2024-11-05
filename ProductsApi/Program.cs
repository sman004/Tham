using Microsoft.EntityFrameworkCore;
using ProductsApi;

var builder = WebApplication.CreateBuilder(args);

// Configure database context
builder.Services.AddDbContext<ProductsContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "Products.db");
        options.UseSqlite($"Data Source={dbPath}");
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
    else
    {
        var cs = builder.Configuration.GetConnectionString("ProductsContext");
        options.UseSqlServer(cs); // Azure SQL for production
    }
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// API endpoints for product access
app.MapGet("/api/products", async (ProductsContext dbContext) =>
{
    try
    {
        var products = await dbContext.Products.ToListAsync();
        return Results.Ok(products);
    }
    catch (Exception ex)
    {
        
        return Results.Problem("An error occurred while retrieving products: " + ex.Message);
    }
});


app.MapGet("/api/products/{id}", async (int id, ProductsContext dbContext) =>
{
    var product = await dbContext.Products.FindAsync(id);
    return product != null ? Results.Ok(product) : Results.NotFound();
});

// Test endpoint
app.MapGet("/", () => "Hello, World!");

app.Run();

