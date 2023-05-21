global using static Cff.SampleApp.Controllers.Prelude;
using Cff.AlgebraicEffect.Abstraction;
using Cff.AlgebraicEffect.Http;
using Cff.SampleApp.Dto;
using FluentValidation;

namespace Cff.SampleApp.Controllers;

using static IHasHttp<Runtime>;

public static partial class Prelude
{
    public static async ValueTask EchoAsync(HttpContext httpContext, CancellationToken ct)
    {
        using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        await IO().Run(new(httpContext, cts));

        static Aff<Runtime, Unit> IO() => Response
        (
            from dto in Request<EchoDto>()
            select dto
        );
    }
}

file readonly record struct Runtime
(
    HttpContext HttpContext,
    CancellationTokenSource CancellationTokenSource
) : IHasHttp<Runtime>,
    IHasValid<Runtime>
{
   

    HttpContext IHas<Runtime, HttpContext>.It => HttpContext;
    IValidator IHas<Runtime, IValidator>.It { get; }
}