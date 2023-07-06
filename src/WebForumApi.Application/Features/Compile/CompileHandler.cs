using Ardalis.Result;
using MediatR;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WebForumApi.Application.Extensions.Serializer;

namespace WebForumApi.Application.Features.Compile;

public class CompileHandler : IRequestHandler<CompileRequest, Result<CompileDto>>
{
    private readonly HttpClient _httpClient;
    private readonly ISerializerService _serializerService;
    public CompileHandler(HttpClient factory, ISerializerService serializerService)
    {
        _httpClient = factory;
        _serializerService = serializerService;
    }
    public async Task<Result<CompileDto>> Handle(CompileRequest request, CancellationToken cancellationToken)
    {
        StringContent json = new(
            JsonSerializer.Serialize(new
                {
                    language = request.Language,
                    memoryLimit = 100,
                    sourcecode = request.SourceCode,
                    testCases = new
                    {
                        test1 = new
                        {
                            expectedOutput = "", input = ""
                        }
                    },
                    timeLimit = 5
                },
                new JsonSerializerOptions(JsonSerializerDefaults.Web)),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
        // Console.WriteLine(await json.ReadAsStringAsync(cancellationToken));
        HttpResponseMessage message = await _httpClient.PostAsync(requestUri: "http://121.37.158.48:8080/api/compile/json", json, cancellationToken);
        Console.WriteLine(await message.Content.ReadAsStringAsync(cancellationToken));
        CompileResponse? response = _serializerService.Deserialize<CompileResponse>(await message.Content.ReadAsStringAsync(cancellationToken));
        // CompileResponse? response = JsonSerializer.Deserialize<CompileResponse>(await message.Content.ReadAsStringAsync(cancellationToken));
        if (response == null)
        {
            return Result.Error();
        }

        return new CompileDto
        {
            Output = response.testCasesResult?.First().Value.Deserialize<TestCaseResult>()?.output ?? "", Status = response.verdict is "Wrong Answer" or "Accepted" ? "OK" : response.verdict, Error = response.error
        };
    }
}