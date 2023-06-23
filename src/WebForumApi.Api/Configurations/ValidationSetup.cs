using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace WebForumApi.Api.Configurations;

public static class ValidationSetup
{
    public static void AddValidationSetup(this IMvcBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<WebForumApi.Application.IAssemblyMarker>();
    }
}
