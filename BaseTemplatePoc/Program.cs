using BaseTemplatePoc.DbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


//Add DbContext

var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
        
        sqlServerOptions.CommandTimeout(120);
    });
});

var app = builder.Build();

app.MapOpenApi();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        context.Database.EnsureCreated();
        context.Database.Migrate();
        
        app.Logger.LogInformation("Database connection successful!");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error connecting to database");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}



app.UseHttpsRedirection();
app.Run();