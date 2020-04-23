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
        private HelperFunction helperFunction;
        public FormalBusiness(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
            timeClass = new DateTimeClass();
            helperFunction = new HelperFunction();
        }

        public async Task<DateTimeClass> AddOneOffTask(TaskData taskData)
        {
            scheduler = await _schedulerFactory.GetScheduler();
            await scheduler.Start();
            var trigger = TriggerBuilder.Create()
                .StartNow()
                .Build();
            var jobDetail = JobBuilder.Create<Job>()
                .WithIdentity(taskData.Task_Name)
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
            ITrigger trigger;
            //判断间隔时间是否为空
            if (string.IsNullOrEmpty(taskData.Task_Interval))
            {
                trigger = TriggerBuilder.Create()
                   .StartAt(Convert.ToDateTime(taskData.Task_PresetTime))
                   .Build();
            }
            else
            {
                var arryDate = helperFunction.ConversionTime(taskData.Task_Interval);
                TimeSpan timeSpan = new TimeSpan(arryDate[0], arryDate[1], arryDate[2], arryDate[3]);
                trigger = TriggerBuilder.Create()
                    //.WithCronSchedule(taskData.Task_PresetTime, p => p.InTimeZone(TimeZoneInfo.Local))
                    //.WithSimpleSchedule(x=>x.WithIntervalInSeconds(int.Parse(taskData.Task_Interval)).RepeatForever())
                    .WithSimpleSchedule(x => x.WithInterval(timeSpan).RepeatForever())
                    .StartAt(Convert.ToDateTime(taskData.Task_PresetTime))
                    .Build();
            }
            
            var jobDetail = JobBuilder.Create<Job>()
                .WithIdentity(taskData.Task_Name)
                .UsingJobData("Business", taskData.Task_BusinessType)
                .Build();
            await scheduler.ScheduleJob(jobDetail, trigger);
            scheduler.ListenerManager.AddJobListener(new mJobListen());
            timeClass.ExDateTime = TimeZoneInfo.ConvertTime((DateTimeOffset)trigger.GetNextFireTimeUtc(), TimeZoneInfo.Local);
            return timeClass;
            
        }
    }
  }
