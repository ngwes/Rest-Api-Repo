using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Domain.Configurations
{
    public class AuthenticationSettings
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifeTime { get; internal set; }
    }
}
