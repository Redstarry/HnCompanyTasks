using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
