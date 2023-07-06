using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Extensions;
using WebForumApi.Application.Extensions.Cache;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Questions.GetQuestions;

public class GetQuestionsHandler : IRequestHandler<GetQuestionsRequest, Result<PaginatedList<QuestionCardDto>>>
{
    private readonly ICacheService _cache;
    private readonly IContext _context;
    private readonly ILogger<GetQuestionsHandler> _logger;

    public GetQuestionsHandler(IContext context, ICacheService cache, ILogger<GetQuestionsHandler> logger)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<QuestionCardDto>>> Handle(GetQuestionsRequest request,
        CancellationToken cancellationToken)
    {
        // tab in ["newest", "heat", "unanswered"]
        switch (request.Tab)
        {
            case null:
            case "heat":
                // order by answer number
                if (string.IsNullOrEmpty(request.KeyWord) && request.CurrentPage == 1)
                {
                    PaginatedList<QuestionCardDto>? cacheResult =
                        await _cache.GetAsync<PaginatedList<QuestionCardDto>>(key: "question_heat", cancellationToken);
                    if (cacheResult != null)
                    {
                        // _logger.LogCritical("hit");
                        return cacheResult;
                    }
                }

                const double answerCountWeight = 0.6;
                const double createTimeWeight = 0.4;
                IQueryable<Question> heatQuestions = _context.Questions
                    .OrderByDescending(
                        x => x.AnswerCount * answerCountWeight +
                             (DateTime.Now - x.CreateTime).Seconds * createTimeWeight)
                    .WhereIf(
                        !string.IsNullOrEmpty(request.KeyWord),
                        x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%")
                    );
                PaginatedList<QuestionCardDto> heatResult = await heatQuestions.Select(q => new QuestionCardDto
                    {
                        Id = q.Id.ToString(),
                        Title = q.Title,
                        VoteNumber =
                            _context.UserQuestionActions.Count(action => action.QuestionId == q.Id && action.IsLike),
                        CreateTime = q.CreateTime,
                        AnswerNumber = q.AnswerCount,
                        Tags = _context.QuestionTags.Where(x => x.QuestionId == q.Id).Select(x => new TagDto
                        {
                            Id = x.TagId, Content = x.Tag.Content, Description = x.Tag.Description
                        }).ToList()
                    })
                    .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
                if (string.IsNullOrEmpty(request.KeyWord) && request.CurrentPage is 1 or 0)
                {
                    await _cache.SetAsync(key: "question_heat", heatResult, TimeSpan.FromMinutes(3), cancellationToken);
                }

                return heatResult;

            case "unanswered":
                if (string.IsNullOrEmpty(request.KeyWord) && request.CurrentPage == 1)
                {
                    PaginatedList<QuestionCardDto>? cacheResult =
                        await _cache.GetAsync<PaginatedList<QuestionCardDto>>(key: "question_answered",
                            cancellationToken);
                    if (cacheResult != null)
                    {
                        _logger.LogCritical("hit");
                        return cacheResult;
                    }
                }

                IQueryable<Question> unansweredQuestions = _context.Questions
                        .Where(x => x.Answers.Count == 0)
                        .OrderByDescending(x => x.CreateTime)
                        .WhereIf(
                            !string.IsNullOrEmpty(request.KeyWord),
                            x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%")
                        )
                    ;
                PaginatedList<QuestionCardDto> unansweredResult = await unansweredQuestions
                    .Select(q => new QuestionCardDto
                    {
                        Id = q.Id.ToString(),
                        Title = q.Title,
                        VoteNumber =
                            _context.UserQuestionActions.Count(action => action.QuestionId == q.Id && action.IsLike),
                        CreateTime = q.CreateTime,
                        AnswerNumber = q.AnswerCount,
                        Tags = _context.QuestionTags.Where(x => x.QuestionId == q.Id).Select(x => new TagDto
                        {
                            Id = x.TagId, Content = x.Tag.Content, Description = x.Tag.Description
                        }).ToList()
                    })
                    .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
                if (string.IsNullOrEmpty(request.KeyWord) && request.CurrentPage == 1)
                {
                    await _cache.SetAsync(key: "question_unanswered", unansweredResult, TimeSpan.FromMinutes(5),
                        cancellationToken);
                }

                return unansweredResult;

            case "newest":
                // order by create time
                if (string.IsNullOrEmpty(request.KeyWord) && request.CurrentPage == 1)
                {
                    PaginatedList<QuestionCardDto>? cacheResult =
                        await _cache.GetAsync<PaginatedList<QuestionCardDto>>(key: "question_newest",
                            cancellationToken);
                    if (cacheResult != null)
                    {
                        // _logger.LogCritical("hit");
                        return cacheResult;
                    }
                }

                IQueryable<Question> newestQuestions = _context.Questions
                    .OrderByDescending(x => x.CreateTime)
                    .WhereIf(
                        !string.IsNullOrEmpty(request.KeyWord),
                        x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%")
                    );
                PaginatedList<QuestionCardDto> newestResult = await newestQuestions
                    .Select(q => new QuestionCardDto
                    {
                        Id = q.Id.ToString(),
                        Title = q.Title,
                        VoteNumber =
                            _context.UserQuestionActions.Count(action => action.QuestionId == q.Id && action.IsLike),
                        CreateTime = q.CreateTime,
                        AnswerNumber = q.AnswerCount,
                        Tags = _context.QuestionTags.Where(x => x.QuestionId == q.Id).Select(x => new TagDto
                        {
                            Id = x.TagId, Content = x.Tag.Content, Description = x.Tag.Description
                        }).ToList()
                    })
                    .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
                if (string.IsNullOrEmpty(request.KeyWord) && request.CurrentPage == 1)
                {
                    await _cache.SetAsync(key: "question_newest", newestResult, TimeSpan.FromDays(1),
                        cancellationToken);
                }

                return newestResult;

            default:
                return Result.Invalid(
                    new List<ValidationError>
                    {
                        new() { Identifier = $"{nameof(request.Tab)}", ErrorMessage = "tab does not match!" }
                    });
            // select unanswered questions
        }
    }
}