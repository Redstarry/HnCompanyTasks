using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using HnCompanyTasks.Models.Data;

namespace HnCompanyTasks.Controllers
{
    [EnableCors("Domain")]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetTaskAll()
        {
            throw new Exception();
        }
        [HttpPost]
        public async Task<IActionResult> Add(TaskRequestData taskRequestData)
        {
            throw new Exception();
        }
        [HttpPost]
        public async Task<IActionResult> GetTask(TaskRequestData taskRequestData)
        {
            throw new Exception();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> update( int id)
        {
            throw new Exception();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> delete(int id)
        {
            throw new Exception();
        }
    }
}