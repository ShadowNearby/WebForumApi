using Ardalis.Result;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common;
using WebForumApi.Application.Features.Questions.Dto;

namespace WebForumApi.Application.Features.Questions.GetQuestionById;

public class GetQuestionByIdHandler : IRequestHandler<GetQuestionByIdRequest, Result<QuestionDto>>
{
    private readonly IContext _context;

    public GetQuestionByIdHandler(IContext context)
    {
        _context = context;
    }
    public Task<Result<QuestionDto>> Handle(GetQuestionByIdRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();// _context.Questions.Select(q=>new QuestionDto{Id = q.Id.ToString(), Content = q.Content, })
    }
}