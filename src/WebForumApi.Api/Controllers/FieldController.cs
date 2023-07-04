using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Api.Controllers.BaseController;
using WebForumApi.Application.Features.Tag.CreateTag;
using WebForumApi.Domain.Auth;
using WebForumApi.Domain.Auth.Interfaces;

namespace WebForumApi.Api.Controllers;

public class FieldController : BaseApiController
{
    public FieldController(IMediator mediator, ISession session) : base(mediator, session)
    {
    }
    /// <summary>
    ///     add a new field, only admin can do this
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
}