using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Application.Features.Questions.GetQuestionById;

public class GetQuestionByIdHandler : IRequestHandler<GetQuestionByIdRequest, Result<QuestionDto>>
{
    private readonly IContext _context;
    private readonly ISession _session;
    public GetQuestionByIdHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }
    public async Task<Result<QuestionDto>> Handle(GetQuestionByIdRequest request, CancellationToken cancellationToken)
    {
        UserId userId = _session.UserId;
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
                UserLike = q.UserQuestionActions.Select(a => a.UserId).Any(i => i == userId),
                UserDislike = q.UserQuestionActions.Select(a => a.UserId).Any(i => i == userId),
                UserStar = q.UserQuestionActions.Select(a => a.UserId).Any(i => i == userId),
                UserCard = new UserCardDto
                {
                    Id = q.CreateUser.Id.ToString(), UserName = q.CreateUser.Username, Avatar = q.CreateUser.Avatar
                },
                Tags = q.Tags.Select(t => new TagDto
                {
                    Id = t.Id.ToString(), Content = t.Content, Description = t.Description
                }).ToList(),
                Answers = q.Answers.Select(a => new AnswerDto
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
            }).FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);
        if (question == null)
        {
            return Result.NotFound();
        }

        return question.Adapt<QuestionDto>();
    }
}