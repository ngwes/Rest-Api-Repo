
using System;
using System.Collections.Generic;
using System.Text;

namespace Rest_Api_Repo.Domain.Queries.V1
{
    public class PaginationQuery
    {
        public PaginationQuery()
        {
            PageNumber = 1;
            PageSize = 5;
        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
