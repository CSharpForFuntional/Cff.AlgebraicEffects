global using static Cff.SampleApp.Controllers.Prelude;
using Cff.AlgebraicEffect.Abstraction;
using Cff.AlgebraicEffect.Http;
using Cff.SampleApp.Dto;

namespace Cff.SampleApp.Controllers;

public static partial class Prelude
{
    public static async ValueTask EchoAsync
    (
        HttpContext httpContext,
        CancellationToken ct
    )
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        await Process<RunTime>().Run(new(httpContext, cts));
    }

    public static Aff<RT, Unit> Process<RT>() where RT: struct, IHasHttp<RT> =>
        IHasHttp<RT>.ResponseAff
        (
            from dto in IHasHttp<RT>.RequestAff<EchoDto>()
            select dto
        );
    
}

file readonly record struct RunTime
(
    HttpContext HttpContext,
    CancellationTokenSource CancellationTokenSource
) : IHasHttp<RunTime>
{
    HttpContext IHas<RunTime, HttpContext>.It => HttpContext;
}