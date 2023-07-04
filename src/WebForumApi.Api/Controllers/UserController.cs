using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebForumApi.Api.Controllers.BaseController;
using WebForumApi.Application.Common.Requests;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Users.DeleteUser;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Application.Features.Users.GetAnswersByUserId;
using WebForumApi.Application.Features.Users.GetAnswersLikedByUserId;
using WebForumApi.Application.Features.Users.GetAnswersStaredByUserId;
using WebForumApi.Application.Features.Users.GetQuestionsByUserId;
using WebForumApi.Application.Features.Users.GetQuestionsLikedByUserId;
using WebForumApi.Application.Features.Users.GetQuestionsStaredByUserId;
using WebForumApi.Application.Features.Users.GetUserById;
using WebForumApi.Application.Features.Users.GetUsers;
using WebForumApi.Application.Features.Users.UpdateUser;
using WebForumApi.Domain.Auth;
using WebForumApi.Domain.Entities.Common;
using ISession=WebForumApi.Domain.Auth.Interfaces.ISession;

namespace WebForumApi.Api.Controllers;

public class UserController : BaseApiController
{
    public UserController(IMediator mediator, ISession session) : base(mediator, session)
    {
    }

    /// <summary>
    /// return all user
    /// </summary>
    /// <returns></returns>
    [ProducesResponseType(typeof(PaginatedList<UserDetailDto>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PaginatedList<UserDetailDto>>> GetUsers(
        [FromQuery] GetUsersRequest request
    )
    {
        return Ok(await Mediator.Send(request, cancellationToken: default));
    }

    [Authorize]
    [HttpGet]
    [Route("me")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<UserDetailDto>> GetMe()
    {
        Result<UserDetailDto> result = await Mediator.Send(new GetUserByIdRequest
            {
                Id = Session.UserId
            },
            cancellationToken: default);
        return result;
    }

    /// <summary>
    ///     Get one user by id from the database
    /// </summary>
    /// <param name="id">The user's ID</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<UserDetailDto>> GetUserById(UserId id)
    {
        Result<UserDetailDto> result = await Mediator.Send(new GetUserByIdRequest
            {
                Id = id
            },
            cancellationToken: default);
        return result;
    }
    [HttpGet]
    [Route("me/questions")]
    [Authorize]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<QuestionCardDto>>> GetQuestionsByOfMe([FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetQuestionsByUserIdRequest>() with
            {
                Id = Session.UserId.Value
            },
            cancellationToken: default);
    }
    [HttpGet]
    [Route("me/answers")]
    [Authorize]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<AnswerCardDto>>> GetAnswersByOfMe([FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetAnswersByUserIdRequest>() with
            {
                Id = Session.UserId.Value
            },
            cancellationToken: default);
    }
    [HttpGet]
    [Route("me/questions/like")]
    [Authorize]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<QuestionCardDto>>> GetQuestionsLikedByOfMe([FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetQuestionsLikedByUserIdRequest>() with
            {
                Id = Session.UserId.Value
            },
            cancellationToken: default);
    }
    [HttpGet]
    [Route("me/answers/like")]
    [Authorize]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<AnswerCardDto>>> GetAnswersLikedByOfMe([FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetAnswersLikedByUserIdRequest>() with
            {
                Id = Session.UserId.Value
            },
            cancellationToken: default);
    }
    [HttpGet]
    [Route("me/questions/star")]
    [Authorize]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<QuestionCardDto>>> GetQuestionsStaredOfMe([FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetQuestionsStaredByUserIdRequest>() with
            {
                Id = Session.UserId.Value
            },
            cancellationToken: default);
    }
    [HttpGet]
    [Route("me/answers/star")]
    [Authorize]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<AnswerCardDto>>> GetAnswersStaredOfMe([FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetAnswersStaredByUserIdRequest>() with
            {
                Id = Session.UserId.Value
            },
            cancellationToken: default);
    }
    [HttpGet]
    [Route("{id:guid}/questions")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<QuestionCardDto>>> GetQuestionsByUserId([FromRoute] Guid id, [FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetQuestionsByUserIdRequest>() with
            {
                Id = id
            },
            cancellationToken: default);
    }
    [HttpGet]
    [Route("{id:guid}/answers")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<AnswerCardDto>>> GetAnswersByUserId([FromRoute] Guid id, [FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetAnswersByUserIdRequest>() with
            {
                Id = id
            },
            cancellationToken: default);
    }
    [HttpGet]
    [Route("{id:guid}/questions/like")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<QuestionCardDto>>> GetQuestionsLikedByUserId([FromRoute] Guid id, [FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetQuestionsLikedByUserIdRequest>() with
            {
                Id = id
            },
            cancellationToken: default);
    }
    [HttpGet]
    [Route("{id:guid}/answers/like")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<AnswerCardDto>>> GetAnswersLikedByUserId([FromRoute] Guid id, [FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetAnswersLikedByUserIdRequest>() with
            {
                Id = id
            },
            cancellationToken: default);
    }
    [HttpGet]
    [Route("{id:guid}/questions/star")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<QuestionCardDto>>> GetQuestionsStaredByUserId([FromRoute] Guid id, [FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetQuestionsStaredByUserIdRequest>() with
            {
                Id = id
            },
            cancellationToken: default);
    }
    [HttpGet]
    [Route("{id:guid}/answers/star")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    public async Task<Result<PaginatedList<AnswerCardDto>>> GetAnswersStaredByUserId([FromRoute] Guid id, [FromQuery] PaginatedRequest request)
    {
        return await Mediator.Send(request.Adapt<GetAnswersStaredByUserIdRequest>() with
            {
                Id = id
            },
            cancellationToken: default);
    }
    /// <summary>
    ///     update user's profile
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("me/update")]
    [TranslateResultToActionResult]
    [Authorize]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
    public async Task<Result> UpdateUser([FromBody] UpdateUserRequest request)
    {
        Result result = await Mediator.Send(request with
        {
            Id = Session.UserId
        });
        return result;
    }
    /// <summary>
    ///     Delete the user identified by a id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
    public async Task<Result> DeleteUser(UserId id)
    {
        Result result = await Mediator.Send(new DeleteUserRequest(id));
        return result;
    }
}