using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;

namespace api.DTO
{
    public class PaginationDressOrderDTO : PaginationDTO
    {
        public PaginationDressOrderDTO(IEnumerable<DressOrderDTO> dressOrdersDTO, int pageSize, int pageIndex, int lastingNumberPages)
            : base(pageSize, pageIndex, lastingNumberPages)
        {
            DressOrdersDTO = dressOrdersDTO;
        }

        public IEnumerable<DressOrderDTO> DressOrdersDTO { get; set; }
    
    }
}
