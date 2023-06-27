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
        RuleFor(x => x.AnswerId).Must(
            (answerId, ct) => context.Answers.Any(a => a.Id == new Guid(answerId.AnswerId))
        );
    }
}

public class AnswerDislikeValidator : AbstractValidator<AnswerLikeRequest>
{
    public AnswerDislikeValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // answer must exist
        RuleFor(x => x.AnswerId).Must(
            (answerId, ct) => context.Answers.Any(a => a.Id == new Guid(answerId.AnswerId))
        );
    }
}

public class AnswerStarValidator : AbstractValidator<AnswerLikeRequest>
{
    public AnswerStarValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        // answer must exist
        RuleFor(x => x.AnswerId).Must(
            (answerId, ct) => context.Answers.Any(a => a.Id == new Guid(answerId.AnswerId))
        );
    }
}