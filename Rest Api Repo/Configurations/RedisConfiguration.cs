using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Configurations
{
    public class RedisConfiguration
    {
        public bool Enabled { get; set; }
        public string ConnectionString { get; set; }
    }
}
