using HnCompanyTasks.JobsListen;
using HnCompanyTasks.Models;
using HnCompanyTasks.Models.Data;
using HnCompanyTasks.Models.Quartz;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HnCompanyTasks.Business
{
    public class FormalBusiness
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler scheduler;
        private DateTimeClass timeClass;
        public FormalBusiness(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
            timeClass = new DateTimeClass();
        }

        public async Task<DateTimeClass> AddOneOffTask(TaskData taskData)
        {
            scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.Start();
            var trigger = TriggerBuilder.Create()
                .StartNow()
                .Build();
            var jobDetail = JobBuilder.Create<Job>()
                .WithIdentity(taskData.Task_Name, "group1")
                .Build();

            await scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.ListenerManager.AddJobListener(new mJobListen());
            timeClass.ExDateTime = TimeZoneInfo.ConvertTime((DateTimeOffset)trigger.GetNextFireTimeUtc(), TimeZoneInfo.Local);

            return timeClass;
        }
        public async Task<DateTimeClass> AddTimedTask(TaskData taskData)
        {
            scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.Start();
            var trigger = TriggerBuilder.Create()
                //.WithCronSchedule(taskData.Task_PresetTime, p => p.InTimeZone(TimeZoneInfo.Local))
                .StartAt( Convert.ToDateTime( taskData.Task_PresetTime))
                .Build();
            var jobDetail = JobBuilder.Create<Job>()
                .WithIdentity(taskData.Task_Name, "group2")
                .Build();
            await scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.ListenerManager.AddJobListener(new mJobListen());
            //var prevDate = trigger.GetPreviousFireTimeUtc();
            timeClass.ExDateTime = TimeZoneInfo.ConvertTime((DateTimeOffset)trigger.GetNextFireTimeUtc(), TimeZoneInfo.Local);
            return timeClass;
            
        }
    }
  }
