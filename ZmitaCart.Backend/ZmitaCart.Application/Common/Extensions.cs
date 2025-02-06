using Microsoft.EntityFrameworkCore;
using ZmitaCart.Domain.Common.Types;
using ZmitaCart.Domain.Entities;

namespace ZmitaCart.Application.Common;

public static class Extensions
{
	public static Task<PaginatedList<TDestination>> ToPaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable,
		int? pageNumber, int? pageSize)
		where TDestination : class
		=> PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

	public static PaginatedList<TDestination> ToPaginatedList<TDestination>(this IEnumerable<TDestination> enumerable,
		int? pageNumber, int? pageSize)
		where TDestination : class
		=> PaginatedList<TDestination>.Create(enumerable, pageNumber, pageSize);
	
	public static IQueryable<TSource> SortBy<TSource>(this IQueryable<TSource> queryable, string? sortBy) 
		where TSource : Offer 
	{
		return sortBy switch
		{
			Sort.createdAscending => queryable.OrderBy(x => x.CreatedAt),
			Sort.createdDescending => queryable.OrderByDescending(x => x.CreatedAt),
			Sort.priceAscending => queryable.OrderBy(x => x.Price),
			Sort.priceDescending => queryable.OrderByDescending(x => x.Price),
			_ => queryable
		};
	}

	public static async Task<List<NamedList<TKey, TElement>>> ToUniqueListAsync<TSource, TKey, TElement>
	(
		this IQueryable<TSource> queryable,
		Func<TSource, TKey> keySelector,
		Func<TSource, TElement> elementSelector
	)
		where TKey : IEquatable<TKey>
	{
		var list = new List<NamedList<TKey, TElement>>();
		
		await foreach (var element in queryable.AsAsyncEnumerable())
		{
			if (list.Any(x => x.Name.Equals(keySelector(element))))
			{
				list.First(x => x.Name.Equals(keySelector(element))).Data.Add(elementSelector(element));
			}
			else
			{
				list.Add(new NamedList<TKey, TElement>
				{
					Name = keySelector(element),
					Data = new List<TElement> { elementSelector(element) }
				});
			}
		}

		return list;
	}
}
