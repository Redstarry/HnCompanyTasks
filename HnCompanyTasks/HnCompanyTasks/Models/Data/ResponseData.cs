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
    }
}

namespace HnCompanyTasks
{
    public enum StatusCode
    {
        Success = 0,
        Fail = 1
    }
}