using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetAnswersStaredByUserId;

public class GetAnswersStaredByUserIdHandler : IRequestHandler<GetAnswersStaredByUserIdRequest, Result<List<AnswerCardDto>>>
{
    public async Task<Result<List<AnswerCardDto>>> Handle(GetAnswersStaredByUserIdRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}