using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebForumApi.Application.Auth;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Infrastructure.Context;

namespace WebForumApi.Api.Configurations;

public static class PersistanceSetup
{
    public static IServiceCollection AddPersistenceSetup(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISession, Session>();
        services.AddDbContext<ApplicationDbContext>(o =>
        {
            o.UseMySql(configuration["ConnectionStrings:DefaultConnection"],
                new MySqlServerVersion(new Version(8, 0, 28)));
        });

        return services;
    }
}