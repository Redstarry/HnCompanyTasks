using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using HnCompanyTasks.Models.Data;
using HnCompanyTasks.Models;
using ContactsAPI.Models.PageModel;

namespace HnCompanyTasks.Controllers
{
    [EnableCors("Domain")]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksServer tasksServer;

        public TasksController(ITasksServer tasksServer)
        {
            this.tasksServer = tasksServer;
        }
        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <param name="page">分页</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTaskAll([FromQuery] Page page)
        {
            return Ok(await tasksServer.GetTasks(page));
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="taskRequestData">任务信息</param>
        /// <param name="page">分页</param>
        /// <returns></returns>
        [HttpPost("task")]
        public async Task<IActionResult> Add(TaskRequestData taskRequestData, [FromQuery]Page page)
        {
            return Ok(await tasksServer.AddTask(taskRequestData, page));
        }
        /// <summary>
        /// 根据数据，自定义查询
        /// </summary>
        /// <param name="page">分页</param>
        /// <param name="taskRequestData">查询的数据</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetTask([FromQuery] Page page,[FromBody]SelectRequestData taskRequestData)
        {
            return Ok(await tasksServer.GetTask(page, taskRequestData));
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="id">任务编号</param>
        /// <param name="updateRequestData">更新后的信息</param>
        /// <param name="page">分页</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> update( int id , UpdateRequestData updateRequestData, [FromQuery]Page page)
        {
            return Ok(await tasksServer.UpdateTask(id, updateRequestData, page));
        }
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="id">任务编号</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            return Ok(await tasksServer.DeleteTask(id));
        }
    }
}