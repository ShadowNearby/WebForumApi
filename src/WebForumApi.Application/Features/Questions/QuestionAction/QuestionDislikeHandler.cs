using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Extensions.Cache;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Questions.GetQuestionById;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Questions.QuestionAction;

public class QuestionDislikeHandler : IRequestHandler<QuestionDislikeRequest, Result>
{
    private readonly IContext _context;
    private readonly ISession _session;
    private readonly ICacheService _cache;

    public QuestionDislikeHandler(IContext context, ISession session, ICacheService cache)
    {
        _context = context;
        _session = session;
        _cache = cache;
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

        QuestionDto? questionDto = await _cache.GetAsync<QuestionDto?>(request.Id, cancellationToken);
        if (questionDto is not null)
        {
            questionDto.DislikeCount = question.DislikeCount;
            questionDto.LikeCount = question.LikeCount;
            await _cache.SetAsync(request.Id, questionDto, TimeSpan.FromMinutes(5), cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}