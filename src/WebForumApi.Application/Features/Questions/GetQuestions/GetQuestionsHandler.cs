using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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

public class GetQuestionsHandler : IRequestHandler<GetQuestionsRequest, PaginatedList<QuestionCardDto>>
{
    private readonly ICacheService _cache;
    private readonly IContext _context;

    public GetQuestionsHandler(IContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<PaginatedList<QuestionCardDto>> Handle(GetQuestionsRequest request,
        CancellationToken cancellationToken)
    {
        // tab in ["newest", "heat", "unanswered"]
        switch (request.Tab)
        {
            case "newest":
                // order by create time
                IQueryable<Question> newestQuestions = _context.Questions
                    .OrderByDescending(x => x.CreateTime)
                    .WhereIf(
                        !string.IsNullOrEmpty(request.KeyWord),
                        x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%")
                    );
                PaginatedList<QuestionCardDto> result = await newestQuestions
                    .ProjectToType<QuestionCardDto>()
                    .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
                if (string.IsNullOrEmpty(request.KeyWord) && request.CurrentPage == 1)
                {
                    PaginatedList<QuestionCardDto>? cacheResult = await _cache.GetAsync<PaginatedList<QuestionCardDto>>("question_newest", cancellationToken);
                    if (cacheResult != null)
                    {
                        return cacheResult;
                    }

                    await _cache.SetAsync("question_newest", result, TimeSpan.FromDays(1), cancellationToken);
                }

                LoggerMessage.Define(LogLevel.Information, new EventId(0), request.PageSize.ToString());
                return result;
            case "heat":
            case "":
                // order by answer number
                IQueryable<Question> heatQuestions = _context.Questions
                    .OrderByDescending(x => x.AnswerCount)
                    .WhereIf(
                        !string.IsNullOrEmpty(request.KeyWord),
                        x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%")
                    );
                return await heatQuestions
                    .ProjectToType<QuestionCardDto>()
                    .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
            default:
                // select unanswered questions
                IQueryable<Question> unansweredQuestions = _context.Questions
                        .Where(x => x.Answers.Count == 0)
                        .OrderByDescending(x => x.CreateTime)
                        .WhereIf(
                            !string.IsNullOrEmpty(request.KeyWord),
                            x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%")
                        )
                    ;
                return await unansweredQuestions.ProjectToType<QuestionCardDto>()
                    .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
    }
}