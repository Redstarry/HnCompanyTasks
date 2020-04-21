using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HnCompanyTasks.Models.Data
{
    public class ResponseData
    {
        public StatusCode Status { get; set; }
        public string Message { get; set; }
        public object Response { get; set; }
        public ResponseData(string Message, object Response, StatusCode status = StatusCode.Success)
        {
            this.Message = Message;
            this.Response = Response;
        }
    }
}

namespace HnCompanyTasks
{
    public enum StatusCode
    {
        Success = 1,
        Fail = 0
    }
}