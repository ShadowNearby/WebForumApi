using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Application.Features.Users.GetQuestionsByUserId;

public class GetQuestionsByUserIdHandler : IRequestHandler<GetQuestionsByUserIdRequest, PaginatedList<QuestionCardDto>>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public GetQuestionsByUserIdHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }
    public async Task<PaginatedList<QuestionCardDto>> Handle(GetQuestionsByUserIdRequest request, CancellationToken cancellationToken)
    {
        return await _context.Questions.Where(a => a.CreateUserId == request.Id).Select(a => new QuestionCardDto
        {
            Id = a.Id.ToString(),
            Title = a.Title,
            VoteNumber = a.LikeCount,
            AnswerNumber = a.AnswerCount,
            Tags = _context.QuestionTags.Where(x => x.QuestionId == a.Id).Select(t => new TagDto
            {
                Id = t.TagId, Content = t.Tag.Content
            }).ToList()
        }).OrderByDescending(x => x.VoteNumber).ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}