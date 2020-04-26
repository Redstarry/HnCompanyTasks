using ContactsAPI.Models.PageModel;
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
        /// <param name="taskRequestData">添加的内容</param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<ResponseData> AddTask(TaskRequestData taskRequestData, Page page);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="id">数据的编号</param>
        /// <param name="updateRequestData">更新后的内容</param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<ResponseData> UpdateTask(int id, UpdateRequestData updateRequestData, Page page);
        /// <summary>
        /// 获取全部任务
        /// </summary>
        /// <returns></returns>
        Task<ResponseData> GetTasks(Page page);
        /// <summary>
        /// 根据特定的内容查询任务
        /// </summary>
        /// <param name="page">分页</param>
        /// <param name="taskRequestData">查询的任务数据</param>
        /// <returns></returns>
        Task<ResponseData> GetTask(Page page , SelectRequestData taskRequestData);
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id">任务的编号</param>
        /// <returns></returns>
        Task<ResponseData> DeleteTask(int id);

        Task<ResponseData> UserInfo(UserInfo userInfo);
    }
}
