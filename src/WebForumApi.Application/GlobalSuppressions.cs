﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    category: "Globalization",
    checkId: "CA1307:Especificar StringComparison",
    Justification = "<Pendente>",
    Scope = "member",
    Target = "~M:WebForumApi.Application.Services.HeroAppService.GetAllHeroes(WebForumApi.Application.Filters.GetHeroesFilter)~System.Threading.Tasks.Task{System.Collections.Generic.List{WebForumApi.Application.DTOs.Hero.GetHeroDTO}}"
)]