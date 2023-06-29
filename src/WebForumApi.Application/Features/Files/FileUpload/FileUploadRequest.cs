using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using WebForumApi.Application.Features.Files.Dto;

namespace WebForumApi.Application.Features.Files.FileUpload;

public record FileUploadRequest : IRequest<Result<FileDto>>
{
    public IFormFile File { get; init; } = null!;
}