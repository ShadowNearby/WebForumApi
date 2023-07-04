using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Api.Controllers.BaseController;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.CreateQuestion;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Questions.GetQuestionAnswers;
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
        Result<QuestionDto> result = await Mediator.Send(new GetQuestionByIdRequest(id), cancellationToken: default);
        return result;
    }

    /// <summary>
    ///     Get the answers of the question identified by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id:guid}/answers")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<PaginatedList<AnswerDto>>> GetAnswersByQuestionId([FromRoute] Guid id,
        [FromQuery] PaginatedRequest request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"session:{Session.UserId}");
        GetQuestionAnswersRequest r =
            new(id)
            {
                CurrentPage = request.CurrentPage, PageSize = request.PageSize
            };
        Result<PaginatedList<AnswerDto>>
            result = await Mediator.Send(r, cancellationToken);
        return result;
    }

    /// <summary>
    /// search questions by tab and keyword
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
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

    /// <summary>
    /// search questions by tab and tag
    /// </summary>
    /// <param name="request"></param>
    /// <param name="tagName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("tagged/{tagName}")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<PaginatedList<QuestionCardDto>>> SearchQuestionByTag(
        [FromQuery] GetQuestionsByTagRequestPublic request,
        [FromRoute] string tagName,
        CancellationToken cancellationToken
    )
    {
        Result<PaginatedList<QuestionCardDto>> result =
            await Mediator.Send(new GetQuestionsByTagRequest(tagName), cancellationToken);
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