using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PatientManagementServices.Domain.Dtos;

namespace PatientManagementServices.Application.Common.Extensions
{
    public static class MappingExtensions
    {
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
            => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
    }
}
