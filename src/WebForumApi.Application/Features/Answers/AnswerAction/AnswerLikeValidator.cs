using FluentValidation;
using System;
using System.Linq;
using WebForumApi.Application.Common;

namespace WebForumApi.Application.Features.Answers.AnswerAction;

public class AnswerLikeValidator : AbstractValidator<AnswerLikeRequest>
{
    public AnswerLikeValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // answer must exist
        RuleFor(x => x.Id).Must(
            (answerId, ct) => context.Answers.Any(a => a.Id == new Guid(answerId.Id))
        );
    }
}

public class AnswerDislikeValidator : AbstractValidator<AnswerDislikeRequest>
{
    public AnswerDislikeValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // answer must exist
        RuleFor(x => x.Id).Must(
            (answerId, ct) => context.Answers.Any(a => a.Id == new Guid(answerId.Id))
        );
    }
}

public class AnswerStarValidator : AbstractValidator<AnswerStarRequest>
{
    public AnswerStarValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // answer must exist
        RuleFor(x => x.Id).Must(
            (answerId, ct) => context.Answers.Any(a => a.Id == new Guid(answerId.Id))
        );
    }
}