using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetaPoco;

namespace HnCompanyTasks.Models
{
    [PetaPoco.TableName("Tasks")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class TaskData
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        
        public int Id { get; set; }
        /// <summary>
        /// 任务名字
        /// </summary>
        public string Task_Name { get; set; }
        /// <summary>
        /// 任务类型
        /// </summary>
        public string Task_TaskType { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string Task_BusinessType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string Task_CreateTime { get; set; }
        /// <summary>
        /// 预定时间
        /// </summary>
        public string Task_PresetTime { get; set; }
        /// <summary>
        /// 时间间隔
        /// </summary>
        public string Task_Interval { get; set; }
        ///// <summary>
        ///// 间隔时间
        ///// </summary>
        ////public string Task_IntervalTime { get; set; }
        /// <summary>
        /// 最后执行时间
        /// </summary>
        public string Task_LastExecuteTime { get; set; }
        /// <summary>
        /// 执行结果 0 是False 1是True
        /// </summary>
        public int Task_ExecuteReuslt { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Task_Describe { get; set; }
        /// <summary>
        /// 是否存在
        /// </summary>
        public int Task_Isexists { get; set; }
    }
}
