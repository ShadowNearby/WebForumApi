using Ardalis.Result;
using MediatR;
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
        var answer = new Answer
        {
            Content = request.Content,
            CreateUserId = _session.UserId,
            QuestionId = new Guid(request.QuestionId)
        };
        await _context.Answers.AddAsync(answer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}