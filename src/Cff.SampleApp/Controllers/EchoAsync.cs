global using static Cff.SampleApp.Controllers.Prelude;
using Cff.AlgebraicEffect.Abstraction;
using Cff.AlgebraicEffect.Http;
using Cff.SampleApp.Dto;
using FluentValidation;
using LanguageExt;

namespace Cff.SampleApp.Controllers;

using static IHasHttp<Runtime>;
using static IHasValid<Runtime>;

public static partial class Prelude
{
    public static EchoDtoValidation Validation { get; } = new EchoDtoValidation();

    public static async ValueTask EchoAsync(HttpContext httpContext, CancellationToken ct)
    {
        using CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        await IO().Run(new Runtime(httpContext, Validation, cts));

        static Aff<Runtime, Unit> IO() => Response
        (
            from dto in Request<EchoDto>()
            from __1 in Validate(dto)
            select dto
        );
    }
}

file readonly record struct Runtime
(
    HttpContext HttpContext,
    IValidator Validator,
    CancellationTokenSource CancellationTokenSource
) : IHasHttp<Runtime>,
    IHasValid<Runtime>
{
    HttpContext IHas<Runtime, HttpContext>.It => HttpContext;
    IValidator IHas<Runtime, IValidator>.It => Validator;
}