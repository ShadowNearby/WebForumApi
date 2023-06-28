using Ardalis.Result;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Questions.QuestionAction;

public class QuestionLikeHandler : IRequestHandler<QuestionLikeRequest, Result>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public QuestionLikeHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<Result> Handle(QuestionLikeRequest request, CancellationToken cancellationToken)
    {
        // get question
        Question? question = _context.Questions.First(q => q.Id == new Guid(request.Id));
        // get user
        User? user = _context.Users.First(u => u.Id == _session.UserId);
        // get user question action
        UserQuestionAction? action = user.UserQuestionActions.Find(u => u.QuestionId == question.Id);
        if (action == null)
        {
            UserQuestionAction? ac = new()
            {
                QuestionId = question.Id,
                UserId = user.Id,
                IsLike = true,
                IsDislike = false,
                IsStar = false
            };
            user.UserQuestionActions.Add(ac);
            // update question
            question.LikeCount += 1;
        }
        else
        {
            // update question
            question.LikeCount += action.IsLike ? -1 : 1;
            question.DislikeCount += action.IsDislike ? -1 : 0;
            // update action
            action.IsLike = !action.IsLike;
            action.IsDislike = false;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}