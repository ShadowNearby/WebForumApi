﻿using Ardalis.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace WebForumApi.Api.Common;

public class ExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            Exception? exception = ex.Demystify();
            _logger.LogError(exception, message: "An error ocurred: {Message}", exception.Message);
            HttpStatusCode code = exception switch
            {
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            Result? result = Result.Error(exception.ToStringDemystified());
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}