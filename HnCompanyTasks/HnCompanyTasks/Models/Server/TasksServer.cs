using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HnCompanyTasks.Business;
using HnCompanyTasks.Models.Data;
using Quartz;
using AutoMapper;

namespace HnCompanyTasks.Models
{
    public class TasksServer:ITasksServer
    {
        //private readonly ISchedulerFactory _schedulerFactory;
        private readonly IMapper mapper;
        private IScheduler scheduler;
        private HelperFunction helperFunction;
        private FormalBusiness formalBusiness;
        private PetaPoco.Database Db;

        public TasksServer(ISchedulerFactory schedulerFactory,IMapper mapper)
        {
            //_schedulerFactory = schedulerFactory??throw new ArgumentNullException(nameof(schedulerFactory));
            this.mapper = mapper;
            helperFunction = new HelperFunction();
            formalBusiness = new FormalBusiness(schedulerFactory);
            Db = new PetaPoco.Database("server = .;database = TaskInfo;uid = sa; pwd = 123", "System.Data.SqlClient", null);
        }

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="taskRequestData">新增的数据</param>
        /// <returns></returns>
        public async Task<ResponseData> AddTask(TaskRequestData taskRequestData)
        {
            if (!helperFunction.CheckForNull(taskRequestData))
            {
                string Message = "任务名字 和 任务类型 和 业务类型 和 预定时间 不能为空";
                return new ResponseData(Message, "", StatusCode.Fail);
            }
            var taskData = mapper.Map<TaskData>(taskRequestData);
            switch (taskData.Task_TaskType)
            {
                // 添加一次性任务
                case "OneOff":
                    
                    taskData.Task_CreateTime = DateTime.Now.ToString("F");
                    taskData.Task_PresetTime = DateTime.Now.ToString("F");
                    taskData.Task_ExecuteReuslt = 0;
                    taskData.Task_Isexists = 1;
                    await  Db.InsertAsync(taskData);
                    await formalBusiness.AddOneOffTask(taskData);
                    //await Db.PageAsync<TaskData>(1, 5, "where Task_Isexists = 1");
                    return new ResponseData("增加一次性任务成功", await Db.PageAsync<TaskData>(1, 5, "where Task_Isexists = 1"), StatusCode.Success);
                // 添加定时任务
                case "TimedTask":
                    var dateTimeMap = await formalBusiness.AddTimedTask(taskData);
                    taskData.Task_CreateTime = DateTime.Now.ToString("F");
                    var TimeDifference = (dateTimeMap.ExDateTime - DateTime.Now).TotalSeconds;
                    taskData.Task_IntervalTime = helperFunction.CheckDateTime(TimeDifference);
                    taskData.Task_ExecuteReuslt = 0;
                    taskData.Task_Isexists = 1;
                    await Db.InsertAsync(taskData);
                    return new ResponseData("增加定时任务成功", await Db.PageAsync<TaskData>(1, 5, "where Task_Isexists = 1"), StatusCode.Success);
                default:
                    return new ResponseData("增加任务失败，请明确任务类型", "", StatusCode.Success);
            }
            
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">数据的编号</param>
        /// <returns></returns>
        public async Task<ResponseData> DeleteTask(int id)
        {
            var selectData = Db.SingleOrDefault<TaskData>("where id = @0", id);
            if (selectData == null)
            {
                return new ResponseData("删除的数据不存在","",StatusCode.Fail);
            }
           await Db.UpdateAsync<TaskData>("set Task_Isexists = 0 where Id = @0", id);
            if (!await scheduler.DeleteJob(new JobKey(selectData.Task_Name)))
            {
                return new ResponseData("删除任务失败", "", StatusCode.Fail);
            }

            return new ResponseData("删除任务成功", "", StatusCode.Success);
        }
        /// <summary>
        /// 获取全部的任务
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData> GetTask()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取相关的任务
        /// </summary>
        /// <param name="responseData">查询的数据</param>
        /// <returns></returns>
        public Task<ResponseData> GetTask(ResponseData responseData)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="id">任务编号</param>
        /// <param name="responseData">更新后的数据</param>
        /// <returns></returns>
        public Task<ResponseData> UpdateTask(int id, ResponseData responseData)
        {
            throw new NotImplementedException();
        }
    }
}
