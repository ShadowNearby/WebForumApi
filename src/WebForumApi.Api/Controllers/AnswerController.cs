using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebForumApi.Application.Features.Answers;
using WebForumApi.Application.Features.Answers.AnswerAction;
using WebForumApi.Application.Features.Answers.CreateAnswer;

namespace WebForumApi.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AnswerController
{
    private readonly IMediator _mediator;

    public AnswerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    // [Route("")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> CreateAnswer([FromBody] CreateAnswerRequest request)
    {
        Result result = await _mediator.Send(request, cancellationToken: default);
        return result;
    }

    [HttpPost]
    [Route("like")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> LikeAnswer([FromBody] AnswerLikeRequest request)
    {
        Result result = await _mediator.Send(request, cancellationToken: default);
        return result;
    }

    [HttpPost]
    [Route("dislike")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> DislikeAnswer([FromBody] AnswerDislikeRequest request)
    {
        Result result = await _mediator.Send(request, cancellationToken: default);
        return result;
    }

    [HttpPost]
    [Route("star")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> StarAnswer([FromBody] AnswerStarRequest request)
    {
        Result result = await _mediator.Send(request, cancellationToken: default);
        return result;
    }
}