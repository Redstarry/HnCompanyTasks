using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Quartz;
using PetaPoco;
using HnCompanyTasks.Models;

namespace HnCompanyTasks.JobsListen
{
    public class mJobListen : IJobListener
    {
        private Database Db;
        public mJobListen()
        {
            Db = new Database("server = .;database = TaskInfo;uid = sa; pwd = 123", "System.Data.SqlClient", null);
        }
        public string Name => GetType().Name;

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.Run(()=> {
                Console.WriteLine("任务不会执行");
            });
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.Run(()=> {
                Console.WriteLine($"任务：{context.JobDetail.Key.Name} 即将执行");
            });
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            var ExDate = TimeZoneInfo.ConvertTime(context.FireTimeUtc, TimeZoneInfo.Local).ToString("F");
            //if (context.NextFireTimeUtc == null)
            //{
            //    Db.Update<TaskData>("set Task_ExecuteReuslt = 1, Task_LastExecuteTime = @0 where Task_Name = @1", ExDate, context.JobDetail.Key.Name);
            //}
            var UTCNextDate = context.NextFireTimeUtc;
            var UTCProDate = context.PreviousFireTimeUtc;
            //var NextDate = UTCProDate != null ? TimeZoneInfo.ConvertTime((DateTimeOffset)context.PreviousFireTimeUtc, TimeZoneInfo.Local).ToString("F") : ExDate;
            var prevDate = UTCProDate != null ? TimeZoneInfo.ConvertTime((DateTimeOffset)context.PreviousFireTimeUtc, TimeZoneInfo.Local).ToString("F") : ExDate;
            Sql sql = Sql.Builder
                .Set("Task_PresetTime = @0, Task_LastExecuteTime = @1, Task_ExecuteReuslt = 1", ExDate, prevDate)
                .Where("Task_Name = @0", context.JobDetail.Key.Name);
            Db.Update<TaskData>(sql);
            return Task.Run(()=> {
                
                Console.WriteLine($"任务：{context.JobDetail.Key.Name} 已执行， 数据已更新");
            });
            
            
        }
    }
}
