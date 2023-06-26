using MediatR;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Questions.GetQuestions;

public class GetQuestionsHandler : IRequestHandler<GetQuestionsRequest, PaginatedList<QuestionDto>>
{
    public Task<PaginatedList<QuestionDto>> Handle(GetQuestionsRequest request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}