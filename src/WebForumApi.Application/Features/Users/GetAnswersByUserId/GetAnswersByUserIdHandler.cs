using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetAnswersByUserId;

public class GetAnswersByUserIdHandler : IRequestHandler<GetAnswersByUserIdRequest, Result<List<AnswerCardDto>>>
{
    public async Task<Result<List<AnswerCardDto>>> Handle(GetAnswersByUserIdRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}