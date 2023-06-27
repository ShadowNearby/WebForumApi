using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebForumApi.Application.Features.Auth;
using WebForumApi.Application.Features.Auth.Authenticate;
using WebForumApi.Application.Features.Auth.Forget;
using WebForumApi.Application.Features.Auth.Refresh;
using WebForumApi.Application.Features.Auth.Register;
using WebForumApi.Application.Features.Users;
using WebForumApi.Application.Features.Users.CreateUser;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Api.Controllers;

public class AuthController : BaseApiController
{
    public AuthController(IMediator mediator, ISession session) : base(mediator, session)
    {
    }
    /// <summary>
    ///     Authenticates the user and returns the token information.
    /// </summary>
    /// <param name="request">Email and password information</param>
    /// <returns>Token information</returns>
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<JwtDto>> Authenticate([FromBody] AuthenticateRequest request)
    {
        Result<JwtDto> jwt = await Mediator.Send(request);
        return jwt;
    }

    [HttpPost]
    [Route("token/refresh")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<JwtDto>> Refresh([FromBody] RefreshRequest request)
    {
        Result<JwtDto> jwt = await Mediator.Send(request);
        return jwt;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> CreateUser([FromBody] RegisterRequest request)
    {
        Result result = await Mediator.Send(request);
        return result;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("forget")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result> Forget([FromBody] ForgetRequest request)
    {
        Result result = await Mediator.Send(request);
        return result;
    }
}