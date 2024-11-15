using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcreteIndustry.BLL.DTOs.Responses.Api
{
    public class PaginatedApiResponse<T> : ApiResponse<T>
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }

        public PaginatedApiResponse(
         bool success,
         string message,
         T data,
         short statusCode,
         int totalItems,
         int totalPages,
         int itemsPerPage,
         int currentPage,
         bool hasNext,
         bool hasPrevious)
         : base(success, message, data, statusCode)
        {
            TotalItems = totalItems;
            TotalPages = totalPages;
            ItemsPerPage = itemsPerPage;
            CurrentPage = currentPage;
            HasNext = hasNext;
            HasPrevious = hasPrevious;
        }
    }
}
