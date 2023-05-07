namespace Cff.AlgebraicEffect.Http;

using FluentValidation;
using System.Diagnostics;
using System.Text.Json;
using Cff.AlgebraicEffect.Abstraction;
using Microsoft.AspNetCore.Http;

public interface IHasHttp<RT> : IHas<RT, HttpContext> where RT : struct, IHasHttp<RT>
{

    public static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static Aff<RT, T> ReadRequestAff<T>() =>
        from http in Eff
        from res in Aff(() => http.Request.ReadFromJsonAsync<T>
        (
            JsonSerializerOptions,
            http.RequestAborted
        ))
        select res;

    public static Aff<RT, Unit> WriteResponseAff<T>(Aff<RT, T> aff) =>
        from http in Eff
        from res in aff.Map(x =>
        {
            var expando = JsonSerializer.SerializeToNode(x, JsonSerializerOptions)!;

            expando.Root["traceId"] = Activity.Current?.Id ?? http.TraceIdentifier;

            return Results.Text(expando.ToJsonString(JsonSerializerOptions), "text/json", System.Text.Encoding.UTF8);
        })
        | @catch(e => true, ex => Eff(() => ex switch
        {
            ValidationException e => Results.ValidationProblem(
                e.Errors.GroupBy(x => x.PropertyName)
                 .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray())),
            _ => Results.Problem(ex.ToString())
        }))
        from _1 in Aff(async () =>
        {
            await res.ExecuteAsync(http);
            return unit;
        })
        select unit;
}
