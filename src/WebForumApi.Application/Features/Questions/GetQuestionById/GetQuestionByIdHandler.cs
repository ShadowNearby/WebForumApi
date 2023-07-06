using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Extensions.Cache;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Questions.GetQuestionById;

public class GetQuestionByIdHandler : IRequestHandler<GetQuestionByIdRequest, Result<QuestionDto>>
{
    private readonly ICacheService _cache;
    private readonly IContext _context;
    private readonly ILogger<GetQuestionByIdHandler> _logger;
    private readonly ISession _session;

    public GetQuestionByIdHandler(IContext context,
        ICacheService cache,
        ILogger<GetQuestionByIdHandler> logger,
        ISession session)
    {
        _context = context;
        _cache = cache;
        _logger = logger;
        _session = session;
    }

    public async Task<Result<QuestionDto>> Handle(GetQuestionByIdRequest request, CancellationToken cancellationToken)
    {
        UserId userId = _session.UserId;
        Guid questionId = request.QuestionId;
        QuestionDto? cachedDto =
            await _cache.GetAsync<QuestionDto?>($"{request.QuestionId}", cancellationToken);
        UserActionDto action = _context.UserQuestionActions
                .Where(x => x.UserId == _session.UserId && x.QuestionId == questionId).Select(
                    a =>
                        new UserActionDto
                        {
                            UserLike = a.IsLike, UserDislike = a.IsDislike, UserStar = a.IsStar
                        })
                .FirstOrDefault() ??
            new UserActionDto
            {
                UserDislike = false, UserStar = false, UserLike = false
            };

        if (cachedDto is not null)
        {
            cachedDto.UserAction = action;
            return cachedDto;
        }

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
                UserCard =
                    new UserCardDto
                    {
                        Id = q.CreateUserId.ToString(), UserName = q.CreateUserUsername, Avatar = q.CreateUserAvatar
                    },
                UserAction = new UserActionDto(),
                Tags = q.QuestionTags.Select(t => new TagDto
                {
                    Id = t.TagId, Content = t.Tag.Content, Description = t.Tag.Description
                }).ToList(),
                Answers = new List<AnswerDto>()
            }).FirstOrDefaultAsync(cancellationToken);
        await _cache.SetAsync($"{request.QuestionId}", question, TimeSpan.FromMinutes(10), cancellationToken);
        if (question is null)
        {
            return Result.NotFound();
        }

        question.UserAction = action;
        return question;
    }
}