using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Questions.QuestionAction;

public class QuestionDislikeHandler : IRequestHandler<QuestionDislikeRequest, Result>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public QuestionDislikeHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<Result> Handle(QuestionDislikeRequest request, CancellationToken cancellationToken)
    {
        // get question
        Question? question =
            await _context.Questions.FirstOrDefaultAsync(q => q.Id == new Guid(request.Id), cancellationToken);
        if (question is null)
        {
            return Result.NotFound();
        }

        // get user question action
        UserQuestionAction? action =
            await _context.UserQuestionActions.FirstOrDefaultAsync(u => u.QuestionId == question.Id, cancellationToken);
        if (action == null)
        {
            UserQuestionAction? ac = new()
            {
                QuestionId = question.Id,
                UserId = _session.UserId,
                IsLike = false,
                IsDislike = true,
                IsStar = false
            };
            _context.UserQuestionActions.Add(ac);
            // update question
            question.DislikeCount += 1;
        }
        else
        {
            // update question
            question.DislikeCount += action.IsDislike ? -1 : 1;
            question.LikeCount += action.IsLike ? -1 : 0;
            // update action
            action.IsDislike = !action.IsDislike;
            action.IsLike = false;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}