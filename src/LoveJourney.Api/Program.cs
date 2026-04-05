using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using LoveJourney.Api.Middleware;
using LoveJourney.Application.Services;
using LoveJourney.Application.Validators;
using LoveJourney.Infrastructure;
using LoveJourney.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure (DbContext, TokenService, FileStorage)
builder.Services.AddInfrastructure(builder.Configuration);

// Application services
builder.Services.AddScoped<AuthService>(sp =>
    new AuthService(sp.GetRequiredService<AppDbContext>(), sp.GetRequiredService<LoveJourney.Application.Common.Interfaces.ITokenService>()));
builder.Services.AddScoped<ProfileService>(sp =>
    new ProfileService(sp.GetRequiredService<AppDbContext>()));
builder.Services.AddScoped<JourneyService>(sp =>
    new JourneyService(sp.GetRequiredService<AppDbContext>()));
builder.Services.AddScoped<PlaceService>(sp =>
    new PlaceService(sp.GetRequiredService<AppDbContext>()));
builder.Services.AddScoped<ReviewService>(sp =>
    new ReviewService(sp.GetRequiredService<AppDbContext>()));
builder.Services.AddScoped<JourneyReviewService>(sp =>
    new JourneyReviewService(sp.GetRequiredService<AppDbContext>()));
builder.Services.AddScoped<PhotoService>(sp =>
    new PhotoService(sp.GetRequiredService<AppDbContext>(), sp.GetRequiredService<LoveJourney.Application.Common.Interfaces.IFileStorageService>()));
builder.Services.AddScoped<AnniversaryService>(sp =>
    new AnniversaryService(sp.GetRequiredService<AppDbContext>()));
builder.Services.AddScoped<BlogPostService>(sp =>
    new BlogPostService(sp.GetRequiredService<AppDbContext>()));
builder.Services.AddScoped<DashboardService>(sp =>
    new DashboardService(sp.GetRequiredService<AppDbContext>(), sp.GetRequiredService<AnniversaryService>()));

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:5173" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .WithHeaders("Content-Type", "Authorization")
              .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
              .AllowCredentials();
    });
});

var app = builder.Build();

// Middleware
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // for uploaded files in wwwroot
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
