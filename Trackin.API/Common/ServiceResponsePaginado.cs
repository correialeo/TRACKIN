using Trackin.API.DTOs;

namespace Trackin.API.Common
{
    public class ServiceResponsePaginado<T> : ServiceResponse<ResultadoPaginadoDTO<T>>
    {
        public ServiceResponsePaginado(IEnumerable<T> items, int pageNumber, int pageSize, int totalCount)
        {
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            Data = new ResultadoPaginadoDTO<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < totalPages
            };
            Success = true;
        }
    }
}
