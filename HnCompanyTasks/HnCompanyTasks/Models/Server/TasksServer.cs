using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HnCompanyTasks.Business;
using HnCompanyTasks.Models.Data;
using Quartz;
using AutoMapper;
using ContactsAPI.Models.PageModel;

namespace HnCompanyTasks.Models
{
    public class TasksServer:ITasksServer
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IMapper mapper;
        private IScheduler scheduler;
        private HelperFunction helperFunction;
        private FormalBusiness formalBusiness;
        private PetaPoco.Database Db;

        public TasksServer(ISchedulerFactory schedulerFactory,IMapper mapper)
        {
            _schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
            this.mapper = mapper;
            helperFunction = new HelperFunction();
            formalBusiness = new FormalBusiness(schedulerFactory);
            scheduler = _schedulerFactory.GetScheduler().Result;
            Db = new PetaPoco.Database("server = .;database = TaskInfo;uid = sa; pwd = 123", "System.Data.SqlClient", null);
        }

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="taskRequestData">新增的数据</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ResponseData> AddTask(TaskRequestData taskRequestData, Page page)
        {
            if (!helperFunction.CheckForNull(taskRequestData))
            {
                string Message = "任务名字 和 任务类型 和 业务类型 和 预定时间 不能为空";
                return new ResponseData(Message, "", StatusCode.Fail);
            }
            var taskData = mapper.Map<TaskData>(taskRequestData);
            var NameIsExists = Db.SingleOrDefault<TaskData>("where Task_Name = @0", taskData.Task_Name);
            var TimeSpanDate = Convert.ToDateTime(taskRequestData.Task_PresetTime) - DateTime.Now;
            if (NameIsExists != null) return new ResponseData("任务名，已存在", "" , StatusCode.Fail);
            if (Convert.ToInt32(TimeSpanDate) < 0) return new ResponseData("执行时间 必须大于 当前时间", "" , StatusCode.Fail);
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
                    return new ResponseData("增加一次性任务成功", await Db.PageAsync<TaskData>(page.PageNumber, page.PageSize, "where Task_Isexists = 1"), StatusCode.Success);
                // 添加定时任务
                case "TimedTask":
                    if (string.IsNullOrEmpty(taskData.Task_PresetTime))
                    {
                        return new ResponseData("增加任务失败,必须指定执行时间", "", StatusCode.Success);
                    }
                    var dateTimeMap = await formalBusiness.AddTimedTask(taskData);
                    taskData.Task_CreateTime = DateTime.Now.ToString("F");
                    var TimeDifference = (dateTimeMap.ExDateTime - DateTime.Now).TotalSeconds;
                    //taskData.Task_IntervalTime = helperFunction.CheckDateTime(TimeDifference);
                    taskData.Task_ExecuteReuslt = 0;
                    taskData.Task_Isexists = 1;
                    await Db.InsertAsync(taskData);
                    return new ResponseData("增加定时任务成功", await Db.PageAsync<TaskData>(page.PageNumber, page.PageSize, "where Task_Isexists = 1"), StatusCode.Success);
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
            JobKey jobKey = new JobKey(selectData.Task_Name);
            if (!await scheduler.DeleteJob(jobKey))
            {
                return new ResponseData("删除任务失败，任务不存在", "", StatusCode.Fail);
            }

            return new ResponseData("删除任务成功", "", StatusCode.Success);
        }
        /// <summary>
        /// 获取全部的任务
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseData> GetTasks(Page page)
        {
            var Data = await Db.PageAsync<TaskData>(page.PageNumber, page.PageSize, "select * from Tasks where Task_IsExists=1");
            return new ResponseData("查询成功", Data, StatusCode.Success);
        }
        /// <summary>
        /// 获取相关的任务
        /// </summary>
        /// <param name="page">分页</param>
        /// <param name="taskRequestData">查询的数据</param>
        /// <returns></returns>
        public async Task<ResponseData> GetTask( Page page, TaskRequestData taskRequestData)
        {
            var TaskDataMap = mapper.Map<TaskData>(taskRequestData);
            var sql = helperFunction.SqlAssembly(TaskDataMap);
            var Data = await Db.PageAsync<TaskData>(page.PageNumber, page.PageSize, sql);
            return new ResponseData("查询成功", Data, StatusCode.Success);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="id">任务编号</param>
        /// <param name="updateRequestData">更新后的数据</param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<ResponseData> UpdateTask(int id, UpdateRequestData updateRequestData, Page page)
        {
            var NameIsExists = Db.SingleOrDefault<TaskData>("where Task_Name = @0", updateRequestData.Task_Name);
            if (NameIsExists != null) return new ResponseData("任务名，已存在", "", StatusCode.Fail);

            var selectData = Db.SingleOrDefault<TaskData>("where id = @0", id);

            if (!helperFunction.CheckDataIsUpdate(selectData)) return new ResponseData("更新失败，输入的内容有误", "",StatusCode.Fail);

            await scheduler.DeleteJob(new JobKey(selectData.Task_Name));


            var NewSelectData = helperFunction.ChangeData(selectData, updateRequestData);

            var dateTimeMap = await formalBusiness.AddTimedTask(NewSelectData);

            int resultCode = await Db.UpdateAsync(NewSelectData, id);
            var Data1 = await Db.PageAsync<TaskData>(page.PageNumber, page.PageSize, "where Task_Isexists = 1");
            if (resultCode > 0) return new ResponseData("修改成功", Data1, StatusCode.Success);
            else return new ResponseData("修改失败", "", StatusCode.Fail);
            //if (string.IsNullOrEmpty(taskRequestData.Task_PresetTime))
            //{
            //    //await formalBusiness.AddOneOffTask(RequestData);
            //    helperFunction.ChangeData(RequestData, selectData);
            //    RequestData.Task_PresetTime = DateTime.Now.ToString("F");
            //    RequestData.Task_Isexists = 1;
            //    RequestData.Task_ExecuteReuslt = 0;
            //    int a = await Db.UpdateAsync(RequestData, id);
            //    await formalBusiness.AddOneOffTask(RequestData);
            //    var Data = await Db.PageAsync<TaskData>(page.PageNumber, page.PageSize, "where Task_Isexists = 1");
            //    return new ResponseData("修改成功", Data, StatusCode.Success);
            //}
            //var dateTimeMap = await formalBusiness.AddTimedTask(RequestData);
            //helperFunction.ChangeData(RequestData, selectData);
            //var TimeDifference = (dateTimeMap.ExDateTime - DateTime.Now).TotalSeconds;
            ////RequestData.Task_IntervalTime = helperFunction.CheckDateTime(TimeDifference);
            //RequestData.Task_ExecuteReuslt = 0;
            //RequestData.Task_Isexists = 1;
            //await Db.UpdateAsync(RequestData);
            //var Data1 = await Db.PageAsync<TaskData>(page.PageNumber, page.PageSize, "where Task_Isexists = 1");
            //return new ResponseData("修改成功", Data1, StatusCode.Success);
        }
    }
}
