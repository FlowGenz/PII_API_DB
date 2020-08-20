using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTO
{
    public abstract class PaginationDTO
    {

        public PaginationDTO(int pageIndex, int pageSize, int totalPages)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalPages = totalPages;

        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
    }
}
