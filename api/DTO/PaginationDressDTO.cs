using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;

namespace api.DTO
{
    public class PaginationDressDTO : PaginationDTO
    {
        public PaginationDressDTO(IEnumerable<DressDTO> dressesDTO, int pageSize, int pageIndex, int lastingNumberPages)
            : base(pageSize, pageIndex, lastingNumberPages)
        {
            DressesDTO = dressesDTO;
        }

        public IEnumerable<DressDTO> DressesDTO { get; set; }
    }
}
