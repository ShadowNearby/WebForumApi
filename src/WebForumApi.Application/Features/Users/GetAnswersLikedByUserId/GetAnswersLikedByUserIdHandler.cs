using Ardalis.Result;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Application.Features.Users.GetAnswersLikedByUserId;

public class
    GetAnswersLikedByUserIdHandler : IRequestHandler<GetAnswersLikedByUserIdRequest,
        Result<PaginatedList<AnswerCardDto>>>
{
    private readonly IContext _context;
    private readonly ISession _session;

    public GetAnswersLikedByUserIdHandler(IContext context, ISession session)
    {
        _context = context;
        _session = session;
    }

    public async Task<Result<PaginatedList<AnswerCardDto>>> Handle(GetAnswersLikedByUserIdRequest request,
        CancellationToken cancellationToken)
    {
        return await _context.UserAnswerActions.Where(a => a.AnswerId == request.Id && a.IsLike).Select(a =>
            new AnswerCardDto
            {
                QuestionId = _context.Answers.First(answer => answer.Id == a.AnswerId).QuestionId.ToString(),
                Id = a.AnswerId.ToString(),
                Content = a.Answer.Content,
                VoteNumber = a.Answer.LikeCount,
                CreateTime = _context.Answers.First(answer => answer.Id == a.AnswerId).CreateTime
            }).OrderByDescending(x => x.VoteNumber).ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}