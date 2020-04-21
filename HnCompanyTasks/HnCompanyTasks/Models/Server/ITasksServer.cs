using HnCompanyTasks.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HnCompanyTasks.Models
{
    public interface ITasksServer
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="responseData">添加的内容</param>
        /// <returns></returns>
        Task<TaskRequestData> AddTask(ResponseData responseData);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="id">数据的编号</param>
        /// <param name="responseData">更新后的内容</param>
        /// <returns></returns>
        Task<TaskRequestData> UpdateTask(int id, ResponseData responseData);
        /// <summary>
        /// 获取全部任务
        /// </summary>
        /// <returns></returns>
        Task<TaskRequestData> GetTask();
        /// <summary>
        /// 根据特定的内容查询任务
        /// </summary>
        /// <param name="responseData">查询的任务数据</param>
        /// <returns></returns>
        Task<TaskRequestData> GetTask(ResponseData responseData);
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id">任务的编号</param>
        /// <returns></returns>
        Task<TaskRequestData> DeleteTask(int id);
    }
}
