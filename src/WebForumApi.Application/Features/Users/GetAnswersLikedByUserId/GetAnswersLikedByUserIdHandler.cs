using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetAnswersLikedByUserId;

public class GetAnswersLikedByUserIdHandler : IRequestHandler<GetAnswersLikedByUserIdRequest, Result<List<AnswerCardDto>>>
{
    public async Task<Result<List<AnswerCardDto>>> Handle(GetAnswersLikedByUserIdRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}