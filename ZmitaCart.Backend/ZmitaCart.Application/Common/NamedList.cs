namespace ZmitaCart.Application.Common;

public class NamedList<TKey, TElement>
	where TKey : notnull
{
	public TKey Name { get; set; } = default!;
	public List<TElement> Data { get; set; } = null!;
}