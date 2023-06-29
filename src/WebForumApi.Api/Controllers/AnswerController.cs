using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebForumApi.Api.Controllers.BaseController;
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
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> CreateAnswer([FromBody] CreateAnswerRequest request)
    {
        Result result = await Mediator.Send(request, cancellationToken: default);
        return result;
    }

    [HttpPost]
    [Route("{id:guid}/like")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> LikeAnswer([FromRoute] Guid id)
    {
        Result result = await Mediator.Send(new AnswerLikeRequest(id.ToString()), cancellationToken: default);
        return result;
    }

    [HttpPost]
    [Route("{id:guid}/dislike")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> DislikeAnswer([FromRoute] Guid id)
    {
        Result result = await Mediator.Send(new AnswerDislikeRequest(id.ToString()), cancellationToken: default);
        return result;
    }

    [HttpPost]
    [Route("{id:guid}/star")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> StarAnswer([FromRoute] Guid id)
    {
        Result result = await Mediator.Send(new AnswerStarRequest(id.ToString()), cancellationToken: default);
        return result;
    }
}