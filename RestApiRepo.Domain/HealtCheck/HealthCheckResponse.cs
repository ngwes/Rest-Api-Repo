using System;
using System.Collections.Generic;
using System.Text;

namespace RestApi_Contracts.HealtCheck
{
    public class HealthCheckResponse
    {
        public string Status { get; set; }
        public IEnumerable<HealthCheck> Checks { get; set; }
        public TimeSpan Duration { get; set; }

        
    }

    
}
