using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    //https://codewithmukesh.com/blog/pagination-in-aspnet-core-webapi/
    public class Response<T> : BaseResponse
    {
        public Response()
        {
        }
        public Response(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
        public T Data { get; protected set; }
    }
}
