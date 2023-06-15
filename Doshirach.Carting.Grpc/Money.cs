namespace Google.Type;

public sealed partial class Money
{
	private const decimal NanoFactor = 1_000_000_000;

	public static implicit operator Money(decimal value)
	{
		var units = decimal.ToInt64(value);
		var nanos = decimal.ToInt32((value - units) * NanoFactor);

		return new() { Units = units, Nanos = nanos };
	}

	public static implicit operator decimal(Money value) => value.Units + value.Nanos / NanoFactor;
}