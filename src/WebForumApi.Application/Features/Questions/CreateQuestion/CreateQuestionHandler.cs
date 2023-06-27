using Ardalis.Result;
using Mapster;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Questions.CreateQuestion;

public class CreateQuestionHandler : IRequestHandler<CreateQuestionRequest, Result>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public CreateQuestionHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<Result> Handle(CreateQuestionRequest request, CancellationToken cancellationToken)
    {
        var tags = request.Tags.Adapt<List<Tag>>();
        Question question = new Question
        {
            Title = request.Title, Content = request.Content, CreateUserId = _session.UserId
        };
        await _context.Questions.AddAsync(question, cancellationToken);
        await _context.QuestionTags.AddRangeAsync(
            tags.Select(t => new QuestionTag { QuestionId = question.Id, TagId = t.Id }).ToList(), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}