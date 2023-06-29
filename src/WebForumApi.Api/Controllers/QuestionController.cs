using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebForumApi.Api.Controllers.BaseController;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.CreateQuestion;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Questions.GetQuestionById;
using WebForumApi.Application.Features.Questions.GetQuestionByTag;
using WebForumApi.Application.Features.Questions.GetQuestions;
using WebForumApi.Application.Features.Questions.QuestionAction;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Api.Controllers;

public class QuestionController : BaseApiController
{
    public QuestionController(IMediator mediator, ISession session) : base(mediator, session)
    {
    }
    /// <summary>
    ///     Get the questions identified by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id:guid}")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<QuestionDto>> GetQuestionById(Guid id)
    {
        Result<QuestionDto> result = await Mediator.Send(new GetQuestionByIdRequest(id, Session.UserId), cancellationToken: default);
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
        Result<PaginatedList<QuestionCardDto>> result = await Mediator.Send(request);
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
        Result<PaginatedList<QuestionCardDto>> result = await Mediator.Send(request);
        return result;
    }


    /// <summary>
    ///     Creates a new question.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("add")]
    [TranslateResultToActionResult]
    [Authorize]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> CreateQuestion([FromBody] CreateQuestionRequest request)
    {
        Result result = await Mediator.Send(request, cancellationToken: default);
        return result;
    }
    /// <summary>
    ///     Casts a like on the given question.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("{id:guid}/like")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> LikeQuestion([FromRoute] Guid id)
    {
        Result result = await Mediator.Send(new QuestionLikeRequest(id.ToString()), cancellationToken: default);
        return result;
    }
    /// <summary>
    ///     Casts a dislike on the given question.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("{id:guid}/dislike")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> DislikeQuestion([FromRoute] Guid id)
    {
        Result result = await Mediator.Send(new QuestionDislikeRequest(id.ToString()), cancellationToken: default);
        return result;
    }
    /// <summary>
    ///     Casts a star on the given question.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("{id:guid}/star")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> StarQuestion([FromRoute] Guid id)
    {
        Result result = await Mediator.Send(new QuestionStarRequest(id.ToString()), cancellationToken: default);
        return result;
    }
}