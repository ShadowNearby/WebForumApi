using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace WebForumApi.Application.Features.Questions.CreateQuestion;

public class CreateQuestionHandler : IRequestHandler<CreateQuestionRequest, Result>
{
    public Task<Result> Handle(CreateQuestionRequest request, CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}