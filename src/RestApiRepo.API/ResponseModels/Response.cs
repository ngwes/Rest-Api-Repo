using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.ResponseModels
{
    public class Response<T>
    {
        public Response()
        {

        }

        public Response(T response)
        {
            Data = response;
        }

        public T Data { get; set; }
    }
}
