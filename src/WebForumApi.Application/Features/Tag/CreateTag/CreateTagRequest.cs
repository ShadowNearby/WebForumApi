using Ardalis.Result;
using FluentValidation;
using MediatR;
using System.Linq;
using WebForumApi.Application.Common;

namespace WebForumApi.Application.Features.Tag.CreateTag;

public class CreateTagRequest : IRequest<Result>
{
    public string Content { get; set; } = null!;
    public string Description { get; set; } = "";
}

public class CreateTagValidator : AbstractValidator<CreateTagRequest>
{
    public CreateTagValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // cannot exist same tag
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.Content).Must(content => !context.Tags.Any(t => t.Content == content));
    }
}