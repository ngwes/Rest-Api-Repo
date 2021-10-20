using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Domain
{
    public class PostTag
    {
        public Tag Tag { get; set; }
        public Guid TagId { get; set; }
        public Post Post { get; set; }
        public Guid PostId { get; set; }
    }
}
