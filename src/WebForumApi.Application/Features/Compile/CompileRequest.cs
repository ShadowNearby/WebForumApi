using Ardalis.Result;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;

namespace WebForumApi.Application.Features.Compile;

public record CompileRequest : IRequest<Result<CompileDto>>
{
    public string Language { get; init; } = null!;
    public string SourceCode { get; init; } = null!;
}

public class CompileValidator : AbstractValidator<CompileRequest>
{
    private readonly List<string> _expectedLanguages = new()
    {
        "CPP",
        "RUBY",
        "HASKELL",
        "GO",
        "SCALA",
        "C",
        "CS",
        "PYTHON",
        "JAVA",
        "RUST",
        "KOTLIN"
    };
    public CompileValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Language).NotEmpty().Must(x => _expectedLanguages.Any(a => a.Equals(x)));

        RuleFor(x => x.SourceCode).NotEmpty();
    }
}