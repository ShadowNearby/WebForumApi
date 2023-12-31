using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Features.Files.Dto;

namespace WebForumApi.Application.Features.Files.FileUpload;

public class FileUploadHandler : IRequestHandler<FileUploadRequest, Result<FileDto>>
{
    private static readonly string DataPath = "";
    public async Task<Result<FileDto>> Handle(FileUploadRequest request, CancellationToken cancellationToken)
    {
        IFormFile file = request.File;
        string filename = $"{Guid.NewGuid().ToString()}{file.FileName}";
        string filepath = $"{DataPath}/wwwroot/images/{filename}";
        await using FileStream stream = File.Create(filepath);
        await file.CopyToAsync(stream, cancellationToken);
        const string host = "121.37.158.48";
        return new Result<FileDto>(new FileDto
        {
            Url = $"http://{host}:8000/images/{filename}"
        });
    }
}