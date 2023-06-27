using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Features.Questions.CreateQuestion;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Questions.GetQuestionById;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Application.Features.Users.GetUserById;
using WebForumApi.Domain.Auth.Interfaces;
using WebForumApi.Domain.Entities.Common;

namespace WebForumApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuestionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISession _session;

    public QuestionController(ISession session, IMediator mediator)
    {
        _session = session;
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id:guid}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<QuestionDto>> GetQuestionById(Guid id)
    {
        Result<QuestionDto> result = await _mediator.Send(new GetQuestionByIdRequest(id, _session.UserId));
        return result;
    }

    [HttpPost("new")]
    [TranslateResultToActionResult]
    // [AllowAnonymous]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> CreateQuestion([FromBody] CreateQuestionRequest request)
    {
        Result result = await _mediator.Send(request, cancellationToken: default);
        return result;
    }
}