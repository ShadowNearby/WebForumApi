using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebForumApi.Api.Controllers.BaseController;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Questions.Dto;
using WebForumApi.Application.Features.Users.DeleteUser;
using WebForumApi.Application.Features.Users.Dto;
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
    ///     Returns all users in the database
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [ProducesResponseType(typeof(PaginatedList<UserDetailDto>), StatusCodes.Status200OK)]
    [Authorize(Roles = Roles.Admin)]
    [HttpGet]
    public async Task<ActionResult<PaginatedList<UserDetailDto>>> GetUsers(
        [FromQuery] GetUsersRequest request
    )
    {
        return Ok(await Mediator.Send(request));
    }

    /// <summary>
    ///     Get one user by id from the database
    /// </summary>
    /// <param name="id">The user's ID</param>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<UserDetailDto>> GetUserById(UserId id)
    {
        Result<UserDetailDto> result = await Mediator.Send(new GetUserByIdRequest(id));
        return result;
    }
    [HttpGet]
    [Route("{id:guid}/questions")]
    public async Task<Result<List<QuestionCardDto>>> GetQuestionsByUserId([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }
    [HttpGet]
    [Route("{id:guid}/answers")]
    public async Task<Result<List<AnswerDto>>> GetAnswersByUserId([FromRoute] Guid id)
    {
        throw new NotImplementedException();
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