namespace Cff.AlgebraicEffect.Http;

using FluentValidation;
using System.Diagnostics;
using System.Text.Json;
using Cff.AlgebraicEffect.Abstraction;
using Microsoft.AspNetCore.Http;

public interface IHasHttp<RT> : IHas<RT, HttpContext> where RT : struct, IHas<RT, HttpContext>
{

    public static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static Aff<RT, T> RequestAff<T>() =>
        from http in Eff
        from res in Aff(() => http.Request.ReadFromJsonAsync<T>
        (
            JsonSerializerOptions,
            http.RequestAborted
        ))
        select res;

    public static Aff<RT, Unit> ResponseAff<T>(Aff<RT, T> aff) =>
        from http in Eff
        from res in aff.Map(x =>
        {
            var expando = JsonSerializer.SerializeToNode(x, JsonSerializerOptions)!;
            expando.Root["traceId"] = Activity.Current?.Id ?? http.TraceIdentifier;

            return Results.Text(expando.ToJsonString(JsonSerializerOptions), "application/json", System.Text.Encoding.UTF8);
        }) | @catch(e => true, ExceptionToResults)
        from _1 in Aff(res.ExecuteAsync(http).ToUnit().ToValue)
        select unit;

    public static Eff<IResult> ExceptionToResults(Exception ex) => ex switch
    {
        ValidationException e => Eff(() => Results.ValidationProblem(
            e.Errors.GroupBy(x => x.PropertyName)
             .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray()))),
        _ => Eff(() => Results.Problem(ex.ToString()))
    };
}
