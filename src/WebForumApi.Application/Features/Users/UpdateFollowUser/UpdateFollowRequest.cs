using Ardalis.Result;
using FluentValidation;
using MediatR;
using System;

namespace WebForumApi.Application.Features.Users.UpdateFollowUser;

public record UpdateFollowRequest : IRequest<Result>
{
    public Guid Id { get; init; }
    public bool IsFollow { get; init; }
}

public class UpdateFollowValidator : AbstractValidator<UpdateFollowRequest>
{
    public UpdateFollowValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Id).NotNull();
    }
}