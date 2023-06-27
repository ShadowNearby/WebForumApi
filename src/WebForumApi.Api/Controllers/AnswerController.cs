using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebForumApi.Api.Controllers.BaseController;
using WebForumApi.Application.Features.Answers;
using WebForumApi.Application.Features.Answers.AnswerAction;
using WebForumApi.Application.Features.Answers.CreateAnswer;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Api.Controllers;

public class AnswerController : BaseApiController
{
    public AnswerController(IMediator mediator, ISession session) : base(mediator, session)
    {
    }
    [HttpPost]
    // [Route("")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> CreateAnswer([FromBody] CreateAnswerRequest request)
    {
        Result result = await Mediator.Send(request, cancellationToken: default);
        return result;
    }

    [HttpPost]
    [Route("like")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> LikeAnswer([FromBody] AnswerLikeRequest request)
    {
        Result result = await Mediator.Send(request, cancellationToken: default);
        return result;
    }

    [HttpPost]
    [Route("dislike")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> DislikeAnswer([FromBody] AnswerDislikeRequest request)
    {
        Result result = await Mediator.Send(request, cancellationToken: default);
        return result;
    }

    [HttpPost]
    [Route("star")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> StarAnswer([FromBody] AnswerStarRequest request)
    {
        Result result = await Mediator.Send(request, cancellationToken: default);
        return result;
    }
}