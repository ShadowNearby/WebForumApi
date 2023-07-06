using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Api.Controllers.BaseController;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Tag.CreateTag;
using WebForumApi.Application.Features.Tag.GetTags;
using WebForumApi.Application.Features.Tag.ModifyTag;
using WebForumApi.Application.Features.Tag.SearchTag;
using WebForumApi.Domain.Auth;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Api.Controllers;

public class TagController : BaseApiController
{
    public TagController(IMediator mediator, ISession session) : base(mediator, session)
    {
    }

    /// <summary>
    /// add a new tag, only admin can do this
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("add")]
    [Authorize(Roles = Roles.Admin)]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.Unauthorized)]
    public async Task<Result> CreateTag([FromBody] CreateTagRequest request, CancellationToken cancellationToken)
    {
        Result result = await Mediator.Send(request, cancellationToken);
        return result;
    }


    /// <summary>
    /// modify the description of a tag, only admin can do this
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("modify")]
    [Authorize(Roles = Roles.Admin)]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.Unauthorized)]
    public async Task<Result> ModifyTag([FromBody] ModifyTagRequest request, CancellationToken cancellationToken)
    {
        Result result = await Mediator.Send(request, cancellationToken);
        return result;
    }


    /// <summary>
    /// get all tags
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
    public async Task<Result<PaginatedList<TagDto>>> GetTags([FromQuery] GetTagsRequest request,
        CancellationToken cancellationToken)
    {
        PaginatedList<TagDto> result = await Mediator.Send(request, cancellationToken);
        return result;
    }

    /// <summary>
    /// search tags through keyword
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("search")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
    public async Task<PaginatedList<TagDto>> SearchTag([FromQuery] SearchTagRequest request,
        CancellationToken cancellationToken)
    {
        PaginatedList<TagDto> result = await Mediator.Send(request, cancellationToken);
        return result;
    }
}