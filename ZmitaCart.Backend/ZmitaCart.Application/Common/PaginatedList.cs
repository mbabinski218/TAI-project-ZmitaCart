using Microsoft.EntityFrameworkCore;

namespace ZmitaCart.Application.Common;

public class PaginatedList<T>
{
    public List<T> Items { get; init; } = null!;
    public int? PageNumber { get; init; }
    public int? TotalPages { get; init; }
    public int TotalCount { get; init; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int? pageNumber, int? pageSize)
    {
        var count = await source.CountAsync();

        if (pageSize.HasValue && pageNumber.HasValue)
        {
            source = source.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await source.ToListAsync();

        return new PaginatedList<T>
        {
            Items = items,
            PageNumber = pageNumber,
            TotalPages = pageSize == null ? null : (int)Math.Ceiling(count / (double)pageSize.Value),
            TotalCount = count
        };
    }
    
    public static PaginatedList<T> Create(IEnumerable<T> source, int? pageNumber, int? pageSize)
    {
        var temp = source.ToList();
        var count = temp.Count;
        
        if (pageSize.HasValue && pageNumber.HasValue)
        {
            temp = temp.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
        }
        
        return new PaginatedList<T>
        {
            Items = temp,
            PageNumber = pageNumber,
            TotalPages = pageSize == null ? null : (int)Math.Ceiling(count / (double)pageSize.Value),
            TotalCount = count
        };
    }
}