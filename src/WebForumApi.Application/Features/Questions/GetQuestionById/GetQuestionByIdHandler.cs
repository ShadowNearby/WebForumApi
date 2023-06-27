using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
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
        QuestionDto? cachedDto = await _cache.GetAsync<QuestionDto?>($"{request.UserId}-{request.QuestionId}", cancellationToken);
        if (cachedDto != null)
        {
            Console.WriteLine("cache hit");
            return cachedDto;
        }

        QuestionDto? question = await _context.Questions.Select(q =>
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
                    Id = q.CreateUser.Id.ToString(), UserName = q.CreateUser.Username, Avatar = q.CreateUser.Avatar
                },
                Tags = _context.QuestionTags.Where(t => t.QuestionId == q.Id).Select(t => new TagDto
                {
                    Id = t.TagId, Content = t.Tag.Content, Description = t.Tag.Description
                }).ToList(),
                Answers = q.Answers.Where(a => a.QuestionId == q.Id).Select(a => new AnswerDto
                {
                    Id = a.Id.ToString(),
                    Content = a.Content,
                    UserCard = new UserCardDto
                    {
                        Id = a.CreateUser.Id.ToString(), UserName = a.CreateUser.Username, Avatar = a.CreateUser.Avatar
                    },
                    LikeCount = a.LikeCount,
                    DislikeCount = a.DislikeCount,
                    StarCount = a.StarCount
                }).ToList()
            }).FirstOrDefaultAsync(q => q.Id == request.QuestionId.ToString(), cancellationToken);
        if (question == null)
        {
            return Result.NotFound();
        }

        var action = await _context.UserQuestionActions.Select(a => new
        {
            a.QuestionId,
            a.UserId,
            a.IsLike,
            a.IsDislike,
            a.IsStar
        }).FirstOrDefaultAsync(a => a.QuestionId.ToString() == question.Id && a.UserId == userId, cancellationToken);
        question.UserLike = action?.IsLike ?? false;
        question.UserDislike = action?.IsDislike ?? false;
        question.UserStar = action?.IsStar ?? false;
        question.Answers.ForEach(at =>
        {
            var aq = _context.UserAnswerActions.Select(a => new
            {
                a.AnswerId,
                a.UserId,
                a.IsLike,
                a.IsDislike,
                a.IsStar
            }).FirstOrDefault(a => a.AnswerId.ToString() == question.Id && a.UserId == userId);
            at.UserLike = aq?.IsLike ?? false;
            at.UserDislike = aq?.IsDislike ?? false;
            at.UserStar = aq?.IsStar ?? false;
        });
        await _cache.SetAsync($"{request.UserId}-{request.QuestionId}", question, TimeSpan.FromMinutes(10), cancellationToken);
        return question;
    }
}