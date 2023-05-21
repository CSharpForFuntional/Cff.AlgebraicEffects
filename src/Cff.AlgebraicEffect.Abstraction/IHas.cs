namespace Cff.AlgebraicEffect.Abstraction;
public interface IHas<RT, T> : IHasCancel<RT> where RT : struct, IHas<RT, T>
{
    protected T It { get; }

    protected static Eff<RT, T> Eff => Eff<RT, T>(static rt => rt.It);
}
