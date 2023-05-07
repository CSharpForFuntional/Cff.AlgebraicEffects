using Cff.AlgebraicEffect.Abstraction;
using FluentValidation;

namespace Cff.AlgebraicEffect.Http;

public interface IHasValidator<RT> : IHas<RT, IValidator> where RT : struct, IHasValidator<RT>
{
    public static Aff<RT, Unit> ValidateAff<T>(T req) => 
        from validator in Eff
        from _ in Aff(() => validator.ValidateAsync(
            ValidationContext<T>.CreateWithOptions(req, o => o.ThrowOnFailures())).ToUnit().ToValue()
        )
        select unit;
}
