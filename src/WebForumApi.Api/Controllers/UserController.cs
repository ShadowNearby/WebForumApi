using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebForumApi.Application.Common.Responses;
using WebForumApi.Application.Features.Auth;
using WebForumApi.Application.Features.Auth.Authenticate;
using WebForumApi.Application.Features.Auth.Refresh;
using WebForumApi.Application.Features.Users;
using WebForumApi.Application.Features.Users.CreateUser;
using WebForumApi.Application.Features.Users.DeleteUser;
using WebForumApi.Application.Features.Users.Dto;
using WebForumApi.Application.Features.Users.GetUserById;
using WebForumApi.Application.Features.Users.GetUsers;
using WebForumApi.Application.Features.Users.UpdateUser;
using WebForumApi.Domain.Auth;
using WebForumApi.Domain.Entities.Common;
using ISession=WebForumApi.Domain.Auth.Interfaces.ISession;

namespace WebForumApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ISession _session;

    public UserController(ISession session, IMediator mediator)
    {
        _session = session;
        _mediator = mediator;
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
        return Ok(await _mediator.Send(request));
    }

    /// <summary>
    ///     Get one user by id from the database
    /// </summary>
    /// <param name="id">The user's ID</param>
    /// <returns></returns>
    [Authorize(Roles = Roles.Admin)]
    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<UserDetailDto>> GetUserById(UserId id)
    {
        Result<UserDetailDto> result = await _mediator.Send(new GetUserByIdRequest(id));
        return result;
    }

    [HttpPut("update")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
    public async Task<Result> UpdateUser([FromBody] UpdateUserRequest request)
    {
        Result result = await _mediator.Send(request with
        {
            Id = _session.UserId
        });
        return result;
    }

    [Authorize(Roles = Roles.Admin)]
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound, ResultStatus.Invalid)]
    public async Task<Result> DeleteUser(UserId id)
    {
        Result result = await _mediator.Send(new DeleteUserRequest(id));
        return result;
    }
}