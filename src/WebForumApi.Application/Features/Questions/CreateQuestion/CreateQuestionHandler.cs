using Ardalis.Result;
using Mapster;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Extensions.Cache;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities;

namespace WebForumApi.Application.Features.Questions.CreateQuestion;

public class CreateQuestionHandler : IRequestHandler<CreateQuestionRequest, Result>
{
    private readonly ICacheService _cache;
    private readonly IContext _context;
    private readonly ISession _session;

    public CreateQuestionHandler(IContext context, ISession session, ICacheService cache)
    {
        _context = context;
        _session = session;
        _cache = cache;
    }

    public async Task<Result> Handle(CreateQuestionRequest request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Tag>? tags = request.Tags.Adapt<List<Domain.Entities.Tag>>();
        Question question = new()
        {
            Title = request.Title,
            Content = request.Content,
            CreateUserId = _session.UserId,
            CreateUserUsername = _session.Username ?? "",
            CreateUserAvatar = _session.Avatar ?? ""
        };
        await _context.Questions.AddAsync(question, cancellationToken);
        await _context.QuestionTags.AddRangeAsync(
            tags.Select(t => new QuestionTag
            {
                QuestionId = question.Id, TagId = t.Id
            }).ToList(),
            cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(key: "question_newest", cancellationToken);
        return Result.Success();
    }
}