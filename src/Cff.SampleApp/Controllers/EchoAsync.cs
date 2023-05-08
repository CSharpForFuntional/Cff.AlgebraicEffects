global using static Cff.SampleApp.Controllers.Prelude;
using Cff.AlgebraicEffect.Abstraction;
using Cff.AlgebraicEffect.Http;
using Cff.SampleApp.Dto;

namespace Cff.SampleApp.Controllers;

using static IHasHttp<RunTime>;

public static partial class Prelude
{
    public static async ValueTask EchoAsync
    (
        HttpContext httpContext,
        CancellationToken ct
    ) 
    {
        var q = WriteResponseAff
        (
            from dto in ReadRequestAff<EchoDto>()
            select dto
        );

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        await q.Run(new(httpContext, cts));
    }
}

file readonly record struct RunTime
(
    HttpContext HttpContext,
    CancellationTokenSource CancellationTokenSource
) : IHasHttp<RunTime>
{
    HttpContext IHas<RunTime, HttpContext>.It => HttpContext;
}