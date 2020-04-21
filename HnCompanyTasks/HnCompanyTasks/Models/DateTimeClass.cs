using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HnCompanyTasks.Models
{
    public class DateTimeClass
    {
        public DateTimeOffset? PrevDateTime { get; set; }
        public DateTimeOffset ExDateTime { get; set; }
        public DateTimeOffset NextDateTime { get; set; }
    }
}
