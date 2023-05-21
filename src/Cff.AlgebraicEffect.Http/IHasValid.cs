using Cff.AlgebraicEffect.Abstraction;
using FluentValidation;

namespace Cff.AlgebraicEffect.Http;

public interface IHasValid<RT> : IHas<RT, IValidator> where RT : struct, IHas<RT, IValidator>
{
    public static Aff<RT, Unit> Validate<T>(T req) => 
        from validator in Eff
        from _ in Aff(validator.ValidateAsync(ValidationContext<T>.CreateWithOptions(req, o => o.ThrowOnFailures())).ToUnit().ToValue)
        select unit;
}
