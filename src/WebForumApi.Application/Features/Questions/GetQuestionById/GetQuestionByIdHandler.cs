using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Extensions;
using WebForumApi.Application.Extensions.Cache;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Questions.GetQuestionById;

public class GetQuestionByIdHandler : IRequestHandler<GetQuestionByIdRequest, Result<QuestionDto>>
{
    private readonly IContext _context;
    private readonly ICacheService _cache;
    public GetQuestionByIdHandler(IContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }
    public async Task<Result<QuestionDto>> Handle(GetQuestionByIdRequest request, CancellationToken cancellationToken)
    {
        UserId userId = request.UserId;
        Guid questionId = request.QuestionId;
        // QuestionDto? cachedDto = await _cache.GetAsync<QuestionDto?>($"{request.UserId}-{request.QuestionId}", cancellationToken);
        // if (cachedDto != null)
        // {
        //     // Console.WriteLine("cache hit");
        //     return cachedDto;
        // }

        QuestionDto? question = await _context.Questions.Where(q => q.Id == questionId).Select(q =>
            new QuestionDto
            {
                Id = q.Id.ToString(),
                Content = q.Content,
                Title = q.Title,
                CreateTime = q.CreateTime,
                LikeCount = q.LikeCount,
                DislikeCount = q.DislikeCount,
                StarCount = q.StarCount,
                UserCard = new UserCardDto
                {
                    Id = q.CreateUserId.ToString(), UserName = q.CreateUserUsername, Avatar = q.CreateUserAvatar
                },
                UserAction = q.UserQuestionActions.Where(a => a.UserId == userId).Select(a => new UserActionDto
                {
                    UserLike = a.IsLike, UserDislike = a.IsDislike, UserStar = a.IsStar
                }).FirstOrDefault() ?? new UserActionDto
                {
                    UserLike = false, UserDislike = false, UserStar = false
                },
                Tags = q.QuestionTags.Select(t => new TagDto
                {
                    Id = t.TagId, Content = t.Tag.Content, Description = t.Tag.Description
                }).ToList(),
                Answers = q.Answers.Select(a => new AnswerDto
                {
                    Id = a.Id.ToString(),
                    Content = a.Content,
                    UserCard = new UserCardDto
                    {
                        Id = a.CreateUserId.ToString(), UserName = a.CreateUserUsername, Avatar = a.CreateUserAvatar
                    },
                    LikeCount = a.LikeCount,
                    DislikeCount = a.DislikeCount,
                    StarCount = a.StarCount,
                    UserAction = a.UserAnswerActions.Where(aa => aa.UserId == userId).Select(aa => new UserActionDto
                    {
                        UserLike = aa.IsLike, UserDislike = aa.IsDislike, UserStar = aa.IsStar
                    }).FirstOrDefault() ?? new UserActionDto
                    {
                        UserLike = false, UserDislike = false, UserStar = false
                    }
                }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);
        if (question == null)
        {
            return Result.NotFound();
        }

        // await _cache.SetAsync($"{request.UserId}-{request.QuestionId}", question, TimeSpan.FromMinutes(10), cancellationToken);
        return question;
    }
}