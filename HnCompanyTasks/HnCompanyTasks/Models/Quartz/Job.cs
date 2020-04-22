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
            var container = context.JobDetail.JobDataMap.GetString("Business");
            var ExDate =TimeZoneInfo.ConvertTime( context.FireTimeUtc, TimeZoneInfo.Local);
            return Task.Run(()=> {
                
                Console.WriteLine($"{container}, 当前执行时间：{ExDate}");
            });
        }
    }
}
