using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace HnCompanyTasks.Controllers
{
    [EnableCors("Domain")]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new Exception();
        }
        [HttpPost]
        public async Task<IActionResult> Add()
        {
            throw new Exception();
        }
        [HttpPut]
        public async Task<IActionResult> update()
        {
            throw new Exception();
        }
        [HttpDelete]
        public async Task<IActionResult> delete()
        {
            throw new Exception();
        }
    }
}