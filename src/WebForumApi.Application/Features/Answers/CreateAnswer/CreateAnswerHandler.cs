using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Answers.CreateAnswer;

public class CreateAnswerHandler : IRequestHandler<CreateAnswerRequest, Result>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public CreateAnswerHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<Result> Handle(CreateAnswerRequest request, CancellationToken cancellationToken)
    {
        Answer? answer = new()
        {
            Content = request.Content,
            CreateUserId = _session.UserId,
            CreateUserUsername = _session.Username ?? "",
            CreateUserAvatar = _session.Avatar ?? "",
            QuestionId = new Guid(request.QuestionId)
        };
        Question question = await _context.Questions.FirstAsync(q => q.Id.ToString() == request.QuestionId, cancellationToken);
        question.AnswerCount += 1;
        await _context.Answers.AddAsync(answer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}