using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Entities;
using WebForumApi.Domain.Entities.Enums;
using BC = BCrypt.Net.BCrypt;

namespace WebForumApi.Api.IntegrationTests.Helpers;

public static class TestingDatabase
{
    public static async Task SeedDatabase(Func<IContext> contextFactory)
    {
        await using IContext? db = contextFactory();
        await db.Users.ExecuteDeleteAsync();
        db.Users.AddRange(GetSeedingUsers);
        await db.SaveChangesAsync();
    }

    public static readonly User[] GetSeedingUsers = new[]
    {
        new User()
        {
            Id = new Guid("2e3b7a21-f06e-4c47-b28a-89bdaa3d2a37"), Password = BC.HashPassword("testpassword123"), Email = "admin@boilerplate.com", Role = "Admin"
        },
        new User()
        {
            Id = new Guid("c68acd7b-9054-4dc3-b536-17a1b81fa7a3"), Password = BC.HashPassword("testpassword123"), Email = "user@boilerplate.com", Role = "User"
        }
    };
}