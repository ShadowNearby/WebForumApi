using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace WebForumApi.Api.Configurations;

public static class ControllerSetup
{
    public static IServiceCollection UseControllerSetup(
        this IServiceCollection services)
    {
        services.AddControllers(options =>
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
        return services;
    }
}