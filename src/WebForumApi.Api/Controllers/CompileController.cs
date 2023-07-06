using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebForumApi.Api.Controllers.BaseController;
using WebForumApi.Application.Features.Compile;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Api.Controllers;

public class CompileController : BaseApiController
{
    public CompileController(IMediator mediator, ISession session) : base(mediator, session)
    {
    }
    [HttpPost]
    [Route("json")]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.Unauthorized)]
    public async Task<Result<CompileDto>> CompileJson([FromBody] CompileRequest request)

    {
        return await Mediator.Send(request, cancellationToken: default);
    }
}