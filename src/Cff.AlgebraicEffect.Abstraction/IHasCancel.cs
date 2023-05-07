namespace Cff.AlgebraicEffect.Abstraction;

using LanguageExt.Effects.Traits;

public interface IHasCancel<RT> : HasCancel<RT> where RT : struct, IHasCancel<RT>
{
    RT HasCancel<RT>.LocalCancel => default;
    CancellationToken HasCancel<RT>.CancellationToken => CancellationTokenSource.Token;
}


