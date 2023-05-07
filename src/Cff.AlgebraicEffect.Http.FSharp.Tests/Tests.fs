module Tests

open Xunit
open Cff.AlgebraicEffect.Http
open System.Runtime.CompilerServices
open Microsoft.AspNetCore.Http
open System.Threading

[<IsReadOnly; Struct>]
type Runtime(HttpContext: HttpContext, CancellationTokenSource : CancellationTokenSource) =
    interface IHasHttp<Runtime> with
        member __.It = HttpContext
        member __.CancellationTokenSource = CancellationTokenSource

[<Fact>]
let ``My test`` () =
    let eff = IHasHttp<Runtime>.Eff
    let _2 = eff.Bind(fun x -> ReadRequestAff<String>())

    Assert.True(true)
