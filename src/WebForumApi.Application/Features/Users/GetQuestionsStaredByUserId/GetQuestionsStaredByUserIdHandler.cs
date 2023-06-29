using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetQuestionsStaredByUserId;

public class GetQuestionsStaredByUserIdHandler : IRequestHandler<GetQuestionsStaredByUserIdRequest, Result<List<QuestionCardDto>>>
{
    public async Task<Result<List<QuestionCardDto>>> Handle(GetQuestionsStaredByUserIdRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}