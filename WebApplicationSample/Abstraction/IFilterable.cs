using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace WebApplicationSample.Abstraction;

public static class QueryableExtensions
{
    public static IQueryable<TSource> ApplyFilter<TSource>(this IQueryable<TSource> source, IFilterable filterable)
    {
        var expression = filterable.FilterBy?.Trim();
        if (string.IsNullOrEmpty(expression) || string.IsNullOrWhiteSpace(expression)) return source;

        var variables = new Dictionary<string, object?>();
        foreach (var variable in typeof(TSource).GetProperties().Select(t => t.Name))
        {
            var value = filterable.GetType().GetProperty(variable)?.GetValue(filterable);
            variables.Add($"${variable}", value);
        }

        return source.Where(expression, variables);
    }

    public static IQueryable<TSource> ApplySort<TSource>(this IQueryable<TSource> source, ISortable sortable)
    {
        var sortByItems = sortable.SortBy?
            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (sortByItems is null || !sortByItems.Any()) return source;

        IOrderedQueryable<TSource>? order = null;
        foreach (var item in sortByItems)
        {
            var orderBy = item.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Take(2)
                .ToArray();

            if (!orderBy.Any()) continue;
            var col = orderBy.FirstOrDefault();
            var dir = orderBy.LastOrDefault() switch
            {
                "desc" => "desc",
                _ => "asc"
            };
            if (col is null) continue;
            var lambda = DynamicExpressionParser.ParseLambda<TSource, object>(null, false, col);
            if (dir == "asc")
            {
                order = order is null ? source.OrderBy(lambda) : order.ThenBy(lambda);
            }
            else
            {
                order = order is null ? source.OrderByDescending(lambda) : order.ThenByDescending(lambda);
            }
        }

        return order ?? source;
    }


    public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, IPagination page)
    {
        page.PageIndex = page.PageIndex <= 0 ? 1 : page.PageIndex;
        page.PageSize = page.PageSize <= 0 ? 10 : page.PageSize;
        var pageItems = source.Count();
        var items = source.Skip(page.PageIndex * page.PageSize).Take(page.PageSize).ToList();
        return new PagedList<T>(items, page.PageIndex, page.PageSize, items.Count, pageItems);
    }

    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, IPagination page,
        CancellationToken cancellationToken = default)
    {
        page.PageIndex = page.PageIndex <= 0 ? 1 : page.PageIndex;
        page.PageSize = page.PageSize <= 0 ? 10 : page.PageSize;
        var totalItems = await source.CountAsync(cancellationToken);
        var items = await source.Skip(page.PageIndex * page.PageSize).Take(page.PageSize)
            .ToListAsync(cancellationToken);
        return new PagedList<T>(items, page.PageIndex, page.PageSize, items.Count, totalItems);
    }
}

public interface IPagination
{
    int PageIndex { get; set; }
    int PageSize { get; set; }
}

public interface IPagedList<out T>
{
    IEnumerable<T> Items { get; }
    int PageIndex { get; }
    int PageSize { get; }
    int PageItems { get; }
    int TotalItems { get; }
}

public class PagedList<T> : IPagedList<T>
{
    public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int pageItems, int totalItems)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        PageItems = pageItems;
        TotalItems = totalItems;
    }

    public int PageIndex { get; }
    public int PageSize { get; }
    public int PageItems { get; }
    public int TotalItems { get; }

    public IEnumerable<T> Items { get; }
}

public interface IFilterable
{
    string? FilterBy { get; set; }
}

public interface ISortable
{
    string? SortBy { get; set; }
}