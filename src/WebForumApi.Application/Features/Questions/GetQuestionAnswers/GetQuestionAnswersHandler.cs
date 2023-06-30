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

public class GetQuestionAnswersHandler : IRequestHandler<GetQuestionAnswersRequest, PaginatedList<AnswerDto>>
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

    public async Task<PaginatedList<AnswerDto>> Handle(GetQuestionAnswersRequest request,
        CancellationToken cancellationToken)
    {
        // Console.WriteLine($"request:{request.Authorization}");
        UserId userId = _session.UserId;
        // Guid userId = request.Authorization is null ? Guid.Empty : Guid.Parse(request.Authorization);
        // find in cache
        // PaginatedList<AnswerDto>? cachedDto = await _cache.GetAsync<PaginatedList<AnswerDto>>(
        //     $"{userId}-{request.QuestionId}-{request.CurrentPage}-{request.PageSize}", cancellationToken);
        // if (cachedDto is not null)
        // {
        //     return cachedDto;
        // }

        Console.WriteLine($"userId:{userId}");
        IQueryable<AnswerDto> answers = _context.Answers.Where(answer => answer.QuestionId.Equals(request.QuestionId))
            .Select(a =>
                new AnswerDto
                {
                    Id = a.Id.ToString(),
                    Content = a.Content,
                    StarCount = a.StarCount,
                    LikeCount = a.LikeCount,
                    DislikeCount = a.DislikeCount,
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
                });
        return await answers.OrderByDescending(a => a.LikeCount)
            .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}