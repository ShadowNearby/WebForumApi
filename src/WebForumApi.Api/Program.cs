using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using WebForumApi.Api.Common;
using WebForumApi.Api.Configurations;
using WebForumApi.Application.Features.Auth;
using WebForumApi.Application.Features.Auth.TokenService;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services
    .AddControllers(options =>
    {
        options.AllowEmptyInputInBodyModelBinding = true;
        options.AddResultConvention(resultMap =>
        {
            resultMap
                .AddDefaultMap()
                .For(
                    ResultStatus.Ok,
                    HttpStatusCode.OK,
                    resultStatusOptions =>
                        resultStatusOptions
                            .For(method: "POST", HttpStatusCode.Created)
                            .For(method: "DELETE", HttpStatusCode.NoContent)
                );
        });
    })
    .AddValidationSetup();
builder.Services.AddTransient<ITokenService, TokenService>();

// Authn / Authrz
builder.Services.AddAuthSetup(builder.Configuration);

// Swagger
builder.Services.AddSwaggerSetup();

// Persistence
builder.Services.AddPersistenceSetup(builder.Configuration);

// Application layer setup
builder.Services.AddApplicationSetup();

// Request response compression
builder.Services.AddCompressionSetup();

// HttpContextAcessor
builder.Services.AddHttpContextAccessor();

// Mediator
builder.Services.AddMediatRSetup();

// Middleware
builder.Services.AddScoped<ExceptionHandlerMiddleware>();

builder.Logging.ClearProviders();

// Add serilog
if (builder.Environment.EnvironmentName != "Testing")
{
    builder.Host.UseLoggingSetup(builder.Configuration);
}

// Add opentelemetry
builder.AddOpenTemeletrySetup();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware(typeof(ExceptionHandlerMiddleware));

app.UseSwaggerSetup();

app.UseResponseCompression();
// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();

await app.Migrate();

await app.RunAsync();