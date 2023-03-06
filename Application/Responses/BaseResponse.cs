using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public abstract class BaseResponse
    {

        public BaseResponse()
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
        }

        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }

        public List<string> ValidationErrors { get; set; } = new List<string>();
    }
}
