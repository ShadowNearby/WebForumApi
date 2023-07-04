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

    public GetQuestionsHandler(IContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<Result<PaginatedList<QuestionCardDto>>> Handle(GetQuestionsRequest request,
        CancellationToken cancellationToken)
    {
        // tab in ["newest", "heat", "unanswered"]
        Console.WriteLine(request.Tab);
        switch (request.Tab)
        {
            case null:
            case "heat":
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
                    List<QuestionCardDto>? cacheResult = await _cache.GetAsync<List<QuestionCardDto>>(key: "question_newest", cancellationToken);
                    if (cacheResult != null)
                    {
                        return result with
                        {
                            Result = cacheResult
                        };
                    }

                    await _cache.SetAsync(key: "question_newest", result.Result, TimeSpan.FromDays(1), cancellationToken);
                }

                LoggerMessage.Define(LogLevel.Information, new EventId(0), request.PageSize.ToString());
                return result;

            default:
                return Result.Invalid(
                    new List<ValidationError>
                    {
                        new()
                        {
                            Identifier = $"{nameof(request.Tab)}", ErrorMessage = "tab does not match!"
                        }
                    });
            // select unanswered questions
            // IQueryable<Question> unansweredQuestions = _context.Questions
            //         .Where(x => x.Answers.Count == 0)
            //         .OrderByDescending(x => x.CreateTime)
            //         .WhereIf(
            //             !string.IsNullOrEmpty(request.KeyWord),
            //             x => EF.Functions.Like(x.Title, $"%{request.KeyWord}%")
            //         )
            //     ;
            // return await unansweredQuestions.ProjectToType<QuestionCardDto>()
            //     .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        }
    }
}