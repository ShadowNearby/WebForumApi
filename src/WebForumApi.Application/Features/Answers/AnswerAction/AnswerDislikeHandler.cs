using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Answers.AnswerAction;

public class AnswerDislikeHandler : IRequestHandler<AnswerDislikeRequest, Result>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public AnswerDislikeHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<Result> Handle(AnswerDislikeRequest request, CancellationToken cancellationToken)
    {
        // select the target answer
        Answer? answer =
            await _context.Answers.FirstOrDefaultAsync(a => a.Id == new Guid(request.Id), cancellationToken);
        if (answer is null)
        {
            return Result.NotFound();
        }

        UserAnswerAction? action =
            await _context.UserAnswerActions.FirstOrDefaultAsync(u => u.AnswerId.Equals(answer.Id), cancellationToken);
        if (action is null)
        {
            action = new UserAnswerAction
            {
                AnswerId = answer.Id,
                UserId = _session.UserId,
                IsLike = false,
                IsDislike = true,
                IsStar = false
            };
            _context.UserAnswerActions.Add(action);
            // update the answer
            answer.DislikeCount += 1;
        }
        else
        {
            // update the answer
            answer.DislikeCount += action.IsDislike ? -1 : 1;
            answer.LikeCount += action.IsLike ? -1 : 0;

            // update the action
            action.IsDislike = !action.IsDislike;
            action.IsLike = false;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}