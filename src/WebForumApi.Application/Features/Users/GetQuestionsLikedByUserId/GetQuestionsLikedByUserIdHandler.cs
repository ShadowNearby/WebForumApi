using Ardalis.Result;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Tags.Dto;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Application.Features.Users.GetQuestionsLikedByUserId;

public class GetQuestionsLikedByUserIdHandler : IRequestHandler<GetQuestionsLikedByUserIdRequest, Result<PaginatedList<QuestionCardDto>>>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public GetQuestionsLikedByUserIdHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }
    public async Task<Result<PaginatedList<QuestionCardDto>>> Handle(GetQuestionsLikedByUserIdRequest request, CancellationToken cancellationToken)
    {
        return await _context.UserQuestionActions.Where(a => a.UserId == request.Id && a.IsLike).Select(a => new QuestionCardDto
        {
            Id = a.QuestionId.ToString(),
            Title = a.Question.Title,
            VoteNumber = a.Question.LikeCount,
            AnswerNumber = a.Question.AnswerCount,
            CreateTime = a.Question.CreateTime,
            Tags = _context.QuestionTags.Where(x => x.QuestionId == a.QuestionId).Select(t => new TagDto
            {
                Id = t.TagId, Content = t.Tag.Content
            }).ToList()
        }).OrderByDescending(x => x.VoteNumber).ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}