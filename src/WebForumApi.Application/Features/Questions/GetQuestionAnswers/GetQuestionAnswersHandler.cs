using Ardalis.Result;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Extensions.Cache;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Questions.GetQuestionAnswers;

public class GetQuestionAnswersHandler : IRequestHandler<GetQuestionAnswersRequest, Result<PaginatedList<AnswerDto>>>
{
    private readonly ICacheService _cache;
    private readonly IContext _context;
    private readonly ISession _session;

    public GetQuestionAnswersHandler(IContext context, ICacheService cache, ISession session)
    {
        _context = context;
        _cache = cache;
        _session = session;
    }

    public async Task<Result<PaginatedList<AnswerDto>>> Handle(GetQuestionAnswersRequest request,
        CancellationToken cancellationToken)
    {
        // Console.WriteLine($"request:{request.Authorization}");
        UserId userId = _session.UserId;

        const double answerVoteWeight = 0.6;
        const double createTimeWeight = 0.4;

        // if (request.CurrentPage is 1 or 0)
        // {
        //     PaginatedList<AnswerDto>? cacheRes =
        //         await _cache.GetAsync<PaginatedList<AnswerDto>?>($"{request.QuestionId}_answers", cancellationToken);
        //
        //     cacheRes?.Result.ForEach(dto =>
        //         {
        //             dto.UserAction = _context.UserAnswerActions
        //                 .Where(action => action.AnswerId.Equals(dto.Id) && action.UserId.Equals(userId))
        //                 .Select(a => new UserActionDto
        //                 {
        //                     UserLike = a.IsLike, UserDislike = a.IsDislike, UserStar = a.IsStar
        //                 }).FirstOrDefault() ?? new UserActionDto();
        //         }
        //     );
        //
        //     if (cacheRes is not null)
        //     {
        //         Console.WriteLine("cache hit");
        //         return Result<PaginatedList<AnswerDto>>.Success(cacheRes);
        //     }
        // }

        IQueryable<AnswerDto> answers = _context.Answers.Where(answer => answer.QuestionId.Equals(request.QuestionId))
            .Select(a =>
                new AnswerDto
                {
                    Id = a.Id.ToString(),
                    Content = a.Content,
                    StarCount = a.StarCount,
                    LikeCount = a.LikeCount,
                    DislikeCount = a.DislikeCount,
                    CreateTime = a.CreateTime,
                    UserCard = new UserCardDto
                    {
                        Id = a.CreateUserId.ToString(), UserName = a.CreateUserUsername, Avatar = a.CreateUserAvatar
                    },
                    UserAction = _context.UserAnswerActions.Where(answerAction =>
                            answerAction.AnswerId.Equals(a.Id) && answerAction.UserId.Equals(userId))
                        .Select(action =>
                            new UserActionDto
                            {
                                UserLike = action.IsLike, UserDislike = action.IsDislike, UserStar = action.IsStar
                            })
                        .FirstOrDefault() ?? new UserActionDto()
                }).OrderByDescending(x =>
                ((DateTime.Now - x.CreateTime).Seconds * createTimeWeight + x.LikeCount * answerVoteWeight));
        PaginatedList<AnswerDto> result = await answers.OrderByDescending(a => a.LikeCount)
            .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
        // await _cache.SetAsync($"{request.QuestionId}_answers", result, TimeSpan.FromSeconds(30), cancellationToken);

        return Result<PaginatedList<AnswerDto>>.Success(result);
    }
}