﻿using FastExpressionCompiler;
using Mapster;
using MassTransit;
using MassTransit.NewIdProviders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebForumApi.Application.Common;
using WebForumApi.Application.MappingConfig;
using WebForumApi.Infrastructure.Context;

namespace WebForumApi.Api.Configurations;

public static class ApplicationSetup
{
    public static IServiceCollection AddApplicationSetup(this IServiceCollection services)
    {
        services.AddScoped<IContext, ApplicationDbContext>();
        NewId.SetProcessIdProvider(new CurrentProcessIdProvider());
        ApplyAllMappingConfigFromAssembly();
        TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileFast();

        return services;
    }

    private static IEnumerable<Type> GetTypesWithInterface<TInterface>(Assembly asm)
    {
        Type? it = typeof(TInterface);
        return asm.GetTypes()
            .Where(x => it.IsAssignableFrom(x) && x is { IsInterface: false, IsAbstract: false });
    }

    private static void ApplyAllMappingConfigFromAssembly()
    {
        IEnumerable<Type>? mappers = GetTypesWithInterface<IMappingConfig>(typeof(IMappingConfig).Assembly);
        foreach (Type? mapperType in mappers)
        {
            IMappingConfig? instance = (IMappingConfig)Activator.CreateInstance(mapperType)!;
            instance.ApplyConfig();
        }
    }
}