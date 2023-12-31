﻿using StronglyTypedIds;
using System;

[assembly: StronglyTypedIdDefaults(
    StronglyTypedIdBackingType.Guid,
    StronglyTypedIdConverter.SystemTextJson
    | StronglyTypedIdConverter.EfCoreValueConverter
    | StronglyTypedIdConverter.Default
    | StronglyTypedIdConverter.TypeConverter,
    StronglyTypedIdImplementations.IEquatable
    | StronglyTypedIdImplementations.Default
)]

namespace WebForumApi.Domain.Entities.Common;

public interface IGuid
{
}

[StronglyTypedId]
public partial struct UserId : IGuid
{
    public static implicit operator UserId(Guid guid)
    {
        return new UserId(guid);
    }
}