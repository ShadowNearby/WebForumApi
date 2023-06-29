using FluentValidation;
using System.IO;

namespace WebForumApi.Application.Features.Files.FileUpload;

public class FileUploadValidator : AbstractValidator<FileUploadRequest>
{
    public FileUploadValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.File).Must(x =>
        {
            string ext = Path.GetExtension(x.FileName).ToLowerInvariant();
            return ext.Equals(".jpg") || ext.Equals(".png") || ext.Equals(".bmp") || ext.Equals(".gif");
        }).WithMessage("please upload .jpg|.png|.bmp|.gif file");
    }
}