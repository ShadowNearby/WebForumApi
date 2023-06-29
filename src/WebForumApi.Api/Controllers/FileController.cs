using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Api.Controllers.BaseController;
using WebForumApi.Application.Features.Files.Dto;
using WebForumApi.Application.Features.Files.FileUpload;
using ISession=WebForumApi.Domain.Auth.Interfaces.ISession;

namespace WebForumApi.Api.Controllers;

public class FileController : BaseApiController
{
    public FileController(IMediator mediator, ISession session) : base(mediator, session)
    {
    }
    [HttpPost]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<FileDto>> FileUpload(IFormFile file, CancellationToken cancellationToken)
    {
        return await Mediator.Send(new FileUploadRequest
            {
                File = file
            },
            cancellationToken);
    }
}