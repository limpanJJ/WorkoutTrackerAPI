using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using WorkoutTrackerAPI.Data;
using WorkoutTrackerAPI.Models;
using WorkoutTrackerAPI.Services;
using WorkoutTrackerAPI.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;

// load .env into environment variables BEFORE CreateBuilder
if (File.Exists(".env"))
{
    Env.Load(".env");
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});

if (builder.Environment.IsDevelopment())
{
    // Dev: bypass JWT – every request is auto-authenticated as demo user
    builder.Services.AddAuthentication("Dev")
        .AddScheme<AuthenticationSchemeOptions, DevAuthHandler>("Dev", null);
}
else
{
    // Production: real JWT validation
    var jwtSecret = builder.Configuration["Jwt:SecretKey"];
    if (string.IsNullOrWhiteSpace(jwtSecret))
        throw new InvalidOperationException("Jwt:SecretKey is not configured.");

    byte[] secretKeyBytes;
    try { secretKeyBytes = Convert.FromBase64String(jwtSecret); }
    catch { secretKeyBytes = Encoding.UTF8.GetBytes(jwtSecret); }

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ValidateLifetime = true,
                RoleClaimType = System.Security.Claims.ClaimTypes.Role
            };
        });
}

builder.Services
    .AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// register token service
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(services);

    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddHttpAuthentication("Bearer", auth => { });
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
