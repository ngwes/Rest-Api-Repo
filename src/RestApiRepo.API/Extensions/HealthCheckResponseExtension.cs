using Newtonsoft.Json;
using RestApiRepo.Contracts.HealtCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Extensions
{
    public static class HealthCheckResponseExtension
    {
        public static string ToJson(this HealthCheckResponse response)
        {
            return JsonConvert.SerializeObject(response);
        }

        public static byte[] ToByte(this HealthCheckResponse response) 
        {
            return Encoding.UTF8.GetBytes(response.ToJson());
        }
    }
}
