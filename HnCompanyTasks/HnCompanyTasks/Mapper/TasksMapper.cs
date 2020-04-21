using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HnCompanyTasks.Models;
using HnCompanyTasks.Models.Data;

namespace HnCompanyTasks.Mapper
{
    public class TasksMapper:Profile
    {
        public TasksMapper()
        {
            CreateMap<TaskData, TaskRequestData>();
            CreateMap<TaskRequestData, TaskData>();
        }
    }
}
