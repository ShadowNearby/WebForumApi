using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebForumApi.Api.Common;
using WebForumApi.Api.Configurations;
using WebForumApi.Application.Features.Auth;
using WebForumApi.Application.Features.Auth.TokenService;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseWebRoot(builder.Configuration.GetSection("WebRoot").Value!);
// Controllers
builder.Services.UseControllerSetup();

// Route
builder.Services.UseRouteSetup();

builder.Services.AddTransient<ITokenService, TokenService>();

// Authn / Authrz
builder.Services.AddAuthSetup(builder.Configuration);

// Swagger
builder.Services.AddSwaggerSetup();

// Cache
builder.Services.AddCaching(builder.Configuration);

// MongoDB
// builder.Services.Configure<MongoSetup>(builder.Configuration.GetSection("MongoDatabase"));

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

builder.Services.AddSerializer();

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
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "_myAllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost").AllowAnyHeader().AllowAnyMethod();
        });
});
WebApplication app = builder.Build();
app.UseHttpLogging();
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
app.UseStaticFiles();
app.UseCors();
// app.UseDirectoryBrowser();
app.MapControllers().RequireAuthorization();

await app.Migrate();

await app.RunAsync();