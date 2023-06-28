﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WebForumApi.Api.IntegrationTests.Helpers;

public static class UriExtensions
{
    public static string ToQueryString(this object request, string separator = ",")
    {
        // Get all properties on the object
        Dictionary<string, object>? properties = request
            .GetType()
            .GetProperties()
            .Where(x => x.CanRead)
            .Where(x => x.GetValue(request, index: null) != null)
            .ToDictionary(x => x.Name, x => x.GetValue(request, index: null));

        // Get names for all IEnumerable properties (excl. string)
        List<string>? propertyNames = properties
            .Where(x => !(x.Value is string) && x.Value is IEnumerable)
            .Select(x => x.Key)
            .ToList();

        // Concat all IEnumerable properties into a comma separated string
        foreach (string? key in propertyNames)
        {
            if (string.IsNullOrEmpty(key))
            {
                continue;
            }

            Type? valueType = properties[key]!.GetType();
            Type? valueElemType = valueType.IsGenericType
                ? valueType.GetGenericArguments()[0]
                : valueType.GetElementType();
            if (valueElemType!.IsPrimitive || valueElemType == typeof(string))
            {
                IEnumerable? enumerable = properties[key] as IEnumerable;
                properties[key] = string.Join(separator, enumerable!.Cast<object>());
            }
        }

        // Concat all key/value pairs into a string separated by ampersand
        return string.Join(
            separator: "&",
            properties.Select(
                x =>
                    string.Concat(
                        Uri.EscapeDataString(x.Key),
                        str1: "=",
                        Uri.EscapeDataString(x.Value?.ToString()!)
                    )
            )
        );
    }
}