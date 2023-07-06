namespace WebForumApi.Application.Features.Compile;

public record CompileDto
{
    public string Output { get; init; } = null!;
    public string Status { get; init; } = null!;

    public string Error { get; init; } = null!;
}