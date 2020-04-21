using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HnCompanyTasks.Models.Data;
using Quartz;

namespace HnCompanyTasks.Models
{
    public class TasksServer:ITasksServer
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler scheduler;

        public TasksServer(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory??throw new ArgumentNullException(nameof(schedulerFactory));
        }

        /// <summary>
        /// 新增任务
        /// </summary>
        /// <param name="responseData">新增的数据</param>
        /// <returns></returns>
        public Task<TaskRequestData> AddTask(ResponseData responseData)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">数据的编号</param>
        /// <returns></returns>
        public Task<TaskRequestData> DeleteTask(int id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取全部的任务
        /// </summary>
        /// <returns></returns>
        public Task<TaskRequestData> GetTask()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取相关的任务
        /// </summary>
        /// <param name="responseData">查询的数据</param>
        /// <returns></returns>
        public Task<TaskRequestData> GetTask(ResponseData responseData)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="id">任务编号</param>
        /// <param name="responseData">更新后的数据</param>
        /// <returns></returns>
        public Task<TaskRequestData> UpdateTask(int id, ResponseData responseData)
        {
            throw new NotImplementedException();
        }
    }
}
