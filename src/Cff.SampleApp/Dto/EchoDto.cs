using FluentValidation;

namespace Cff.SampleApp.Dto;

public record EchoDto
{
    public required string Message { get; init; }
}


public class EchoDtoValidation : AbstractValidator<EchoDto>
{
    public EchoDtoValidation()
    {
        RuleFor(x => x.Message).MinimumLength(0);
    }
}