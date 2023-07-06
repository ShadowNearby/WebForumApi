using System.Text.Json.Nodes;

namespace WebForumApi.Application.Features.Compile;

public record TestCaseResult
{
    public string output { get; init; } = "";
}

public record CompileResponse
{
    public JsonObject? testCasesResult { get; init; }
    public string error { get; init; } = "";
    public string verdict { get; init; } = "";
}