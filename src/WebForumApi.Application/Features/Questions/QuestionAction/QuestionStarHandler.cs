using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Extensions.Cache;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Questions.QuestionAction;

public class QuestionStarHandler : IRequestHandler<QuestionStarRequest, Result>
{
    private readonly IContext _context;
    private readonly ISession _session;
    private readonly ICacheService _cache;


    public QuestionStarHandler(IContext context, ISession session, ICacheService cache)
    {
        _context = context;
        _session = session;
        _cache = cache;
    }

    public async Task<Result> Handle(QuestionStarRequest request, CancellationToken cancellationToken)
    {
        // get question
        Question? question =
            await _context.Questions.FirstOrDefaultAsync(q => q.Id.Equals(new Guid(request.Id)), cancellationToken);
        if (question is null)
        {
            return Result.NotFound();
        }

        // get user question action
        UserQuestionAction? action =
            _context.UserQuestionActions.FirstOrDefault(questionAction =>
                questionAction.QuestionId.Equals(question.Id) && questionAction.UserId.Equals(_session.UserId));
        if (action is null)
        {
            UserQuestionAction ac = new UserQuestionAction
            {
                QuestionId = question.Id,
                UserId = _session.UserId,
                IsLike = false,
                IsDislike = false,
                IsStar = true
            };
            _context.UserQuestionActions.Add(ac);
            // update question
            question.StarCount += 1;
            await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            // update question
            question.StarCount += action.IsStar ? -1 : 1;
            // update action
            action.IsStar = !action.IsStar;
        }

        QuestionDto? questionDto = await _cache.GetAsync<QuestionDto?>(request.Id, cancellationToken);
        if (questionDto is not null)
        {
            questionDto.StarCount = question.StarCount;
            await _cache.SetAsync(request.Id, questionDto, TimeSpan.FromMinutes(5), cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}