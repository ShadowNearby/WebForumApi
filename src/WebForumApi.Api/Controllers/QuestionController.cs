using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.CreateQuestion;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Questions.GetQuestionById;
using WebForumApi.Application.Features.Questions.GetQuestionByTag;
using WebForumApi.Application.Features.Questions.GetQuestions;
using WebForumApi.Application.Features.Questions.QuestionAction;
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
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<QuestionDto>> GetQuestionById(Guid id)
    {
        Result<QuestionDto> result = await _mediator.Send(new GetQuestionByIdRequest(id, _session.UserId));
        return result;
    }

    // get questions by tab and keyword(?), and no tag
    [HttpGet]
    [Route("search")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<PaginatedList<QuestionCardDto>>> SearchQuestion(
        [FromQuery] GetQuestionsRequest request
    )
    {
        Result<PaginatedList<QuestionCardDto>> result = await _mediator.Send(request);
        return result;
    }

    // get questions by tab and keyword(?), and tag
    [HttpGet]
    [Route("tagged")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<PaginatedList<QuestionCardDto>>> SearchQuestionByTag(
        [FromQuery] GetQuestionsByTagRequest request
    )
    {
        Result<PaginatedList<QuestionCardDto>> result = await _mediator.Send(request);
        return result;
    }


    // create a new question
    [HttpPost]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> CreateQuestion([FromBody] CreateQuestionRequest request)
    {
        Result result = await _mediator.Send(request, cancellationToken: default);
        return result;
    }

    [HttpPost("like")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> LikeQuestion([FromBody] QuestionLikeRequest request)
    {
        Result result = await _mediator.Send(request, cancellationToken: default);
        return result;
    }

    [HttpPost("dislike")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> DislikeQuestion([FromBody] QuestionDislikeRequest request)
    {
        Result result = await _mediator.Send(request, cancellationToken: default);
        return result;
    }

    [HttpPost("star")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> StarQuestion([FromBody] QuestionStarRequest request)
    {
        Result result = await _mediator.Send(request, cancellationToken: default);
        return result;
    }
}