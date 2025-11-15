using Common.Extensions;
using Common.Handlers;
using Identity;
using Microsoft.AspNetCore.Antiforgery;
using Persistence;
using Serilog;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// DI Containers
builder.Services.AddOpenApi();
builder.Services.AddEndpoints();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

// Add exception handlers
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Configure logger
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

if (builder.Environment.IsDevelopment())
{
    // Enable CORS for development environment
    builder.Services.AddDevelopmentsCorsSettings();
}

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
});

builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddDatabaseServices(builder.Configuration);

// Build app
var app = builder.Build();

// Anti forgery middleware
var antiforgery = app.Services.GetRequiredService<IAntiforgery>();
app.Use(async (context, next) =>
{
    var tokens = antiforgery.GetAndStoreTokens(context);
    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!,
        new CookieOptions { HttpOnly = false, SameSite = SameSiteMode.None, Secure = true });
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyDatabaseMigrations();
}

app.MapEndpoints();
app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseIdentityServices();
app.Run();