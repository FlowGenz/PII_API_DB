using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;

namespace api.DTO
{
    public class PaginationCustomerDTO : PaginationDTO
    {
        public PaginationCustomerDTO(IEnumerable<CustomerDTO> customersDTO, int pageSize, int pageIndex, int lastingNumberPages) 
            : base (pageSize, pageIndex, lastingNumberPages)
        {
            CustomersDTO = customersDTO;
        }

        public IEnumerable<CustomerDTO> CustomersDTO { get; set; }
    }
}
