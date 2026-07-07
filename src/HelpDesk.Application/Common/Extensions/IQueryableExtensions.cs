using HelpDesk.Application.Common.Models;

namespace HelpDesk.Application.Common.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<Ticket> FilterByStatus(this IQueryable<Ticket> query, TicketStatus? status)
    {
        if (status is not null)
        {
            query = query.Where(t => t.Status == status);
        }
        return query;
    }

    public static IQueryable<Ticket> FilterByPriority(this IQueryable<Ticket> query, TicketPriority? priority)
    {
        if (priority is null)
            return query;

        return query.Where(x => x.Priority == priority);
    }

    public static IQueryable<Ticket> FilterByCategory(this IQueryable<Ticket> query, int? categoryId)
    {
        if (categoryId is null)
            return query;

        return query.Where(x => x.CategoryId == categoryId);
    }

    public static IQueryable<Ticket> Search(this IQueryable<Ticket> query, string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        search = search.Trim();

        return query.Where(x =>
            x.Title.Contains(search) ||
            x.Description.Contains(search));
    }

    public static IQueryable<Ticket> ApplySorting(
    this IQueryable<Ticket> query,
    TicketSortField sortBy,
    bool descending)
    {
        return sortBy switch
        {
            TicketSortField.Title =>
                descending
                    ? query.OrderByDescending(x => x.Title)
                    : query.OrderBy(x => x.Title),

            TicketSortField.Priority =>
                descending
                    ? query.OrderByDescending(x => x.Priority)
                    : query.OrderBy(x => x.Priority),

            TicketSortField.Status =>
                descending
                    ? query.OrderByDescending(x => x.Status)
                    : query.OrderBy(x => x.Status),

            _ =>
                descending
                    ? query.OrderByDescending(x => x.CreatedAtUtc)
                    : query.OrderBy(x => x.CreatedAtUtc)
        };
    }

    public static IQueryable<T> Paginate<T>(
    this IQueryable<T> query,
    int page,
    int pageSize)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        return query
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
    }

    public static IQueryable<Ticket> ApplyFilter(
    this IQueryable<Ticket> query,
    TicketFilter filter)
    {
        return query
            .FilterByStatus(filter.Status)
            .FilterByPriority(filter.Priority)
            .FilterByCategory(filter.CategoryId)
            .Search(filter.Search)
            .ApplySorting(filter.SortBy, filter.Descending);
    }

    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
    this IQueryable<T> query,
    int page,
    int pageSize,
    CancellationToken cancellationToken = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
