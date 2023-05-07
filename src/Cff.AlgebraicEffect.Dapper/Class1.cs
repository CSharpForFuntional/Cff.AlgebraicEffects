namespace Cff.AlgebraicEffect.Dapper;

public interface IMammal
{
    public record Human: IMammal
    {
        public required string Name { get; init; }
        public int Age { get; init; }
        public int Amount { get; init; }
    }
    public record Dog: IMammal
    {
        public required string Name { get; init; }
        public int Age { get; init; }

    }
}