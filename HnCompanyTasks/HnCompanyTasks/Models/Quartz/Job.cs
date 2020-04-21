using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HnCompanyTasks.Models.Quartz
{
    public class Job : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(()=> {

                Console.WriteLine("已开启");
            });
        }
    }
}
