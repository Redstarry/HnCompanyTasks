using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HnCompanyTasks.Models.Data
{
    public class SelectRequestData
    {
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
        /// <summary>
        /// 执行结果 0 是False 1是True
        /// </summary>
        public int Task_ExecuteReuslt { get; set; }

        /// <summary>
        /// 查询类 创建时间开始 CreatTimeStart 格式： 2020-12-02 54:57
        /// </summary>
        public string CreatTimeStart { get; set; }
        /// <summary>
        /// 查询类 创建时间结束 CreatTimeStart 格式： 2020-12-02 54:57
        /// </summary>
        public string CreatTimeEnd { get; set; }

        /// <summary>
        /// 查询类 预定时间开始 CreatTimeStart 格式： 2020-12-02 54:57
        /// </summary>
        public string TaskPresetTimeStart { get; set; }

        /// <summary>
        /// 查询类 预定时间结束 CreatTimeStart 格式： 2020-12-02 54:57
        /// </summary>
        public string TaskPresetTimeEnd { get; set; }

        /// <summary>
        /// 查询类 执行时间开始 CreatTimeStart 格式： 2020-12-02 54:57
        /// </summary>
        public string TaskLastExecuteTimeStart { get; set; }

        /// <summary>
        /// 查询类 执行时间结束 CreatTimeStart 格式： 2020-12-02 54:57
        /// </summary>
        public string TaskLastExecuteTimeEnd { get; set; }
    }
}
