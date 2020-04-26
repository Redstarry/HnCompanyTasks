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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            return Ok(await tasksServer.DeleteTask(id));
        }
        /// <summary>
        /// 登录验证接口
        /// </summary>
        /// <param name="userInfo">用户名和密码</param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> PostLogin(UserInfo userInfo)
        {
            return Ok(await tasksServer.UserInfo(userInfo));
        }
    }
}