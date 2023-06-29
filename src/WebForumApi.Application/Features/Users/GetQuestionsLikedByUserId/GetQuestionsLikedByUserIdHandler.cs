using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Users.GetQuestionsLikedByUserId;

public class GetQuestionsLikedByUserIdHandler : IRequestHandler<GetQuestionsLikedByUserIdRequest, Result<List<QuestionCardDto>>>
{
    public async Task<Result<List<QuestionCardDto>>> Handle(GetQuestionsLikedByUserIdRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}