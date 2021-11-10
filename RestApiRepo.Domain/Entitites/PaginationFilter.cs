using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Domain.Entities
{
    public class PaginationFilter
    {
        public PaginationFilter()
        {

        }
        public PaginationFilter(PaginationFilter toCopy)
        {
            PageNumber = toCopy.PageNumber;
            PageSize = toCopy.PageSize;
        }
        public PaginationFilter AddAPage()
        {
            PageNumber++;
            return this;
        }
        public PaginationFilter RemoveAPage()
        {
            PageNumber--;
            return this;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
