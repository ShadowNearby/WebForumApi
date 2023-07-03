using Ardalis.Result;
using FluentValidation;
using MediatR;
using System.Linq;
using WebForumApi.Application.Common;

namespace WebForumApi.Application.Features.Tag.ModifyTag;

public record ModifyTagRequest : IRequest<Result>
{
    public long TagId { get; init; }
    public string Description { get; init; } = "";
}

public class ModifyTagValidator : AbstractValidator<ModifyTagRequest>
{
    public ModifyTagValidator(IContext context)
    {
        RuleFor(x => x.TagId).NotEmpty();
        RuleFor(x => x.TagId).Must(x => context.Tags.Any(t => t.Id == x));
    }
}