using HnCompanyTasks.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PetaPoco;
using HnCompanyTasks.Models;

namespace HnCompanyTasks.Business
{
    public class HelperFunction
    {
        public bool CheckForNull(TaskRequestData taskRequestData)
        {
            var IsName = string.IsNullOrEmpty(taskRequestData.Task_Name);
            var IsTaskType = string.IsNullOrEmpty(taskRequestData.Task_TaskType);
            var IsBusinessType = string.IsNullOrEmpty(taskRequestData.Task_BusinessType);
            //var IsPresetTime = string.IsNullOrEmpty(taskRequestData.Task_PresetTime);

            if (IsName || IsTaskType || IsBusinessType )
            {
                return false;
            }
            return true;
            
        }
        /// <summary>
        /// 把秒数转换成时,分，秒
        /// </summary>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        public string CheckDateTime(double Seconds)
        {
            int ss = Convert.ToInt32(Seconds);
            TimeSpan ts = new TimeSpan(0,0, ss);
            var day = ts.TotalDays;
            var IntDay = (int)day;
            return $" {IntDay}天,  {ts.Hours}时, {ts.Minutes}分, {ts.Seconds}秒，后执行";
        }

        public Sql SqlAssembly(SelectRequestData taskDataMap )
        {
            Sql selectSql = new Sql();
            selectSql.Append("select * from Tasks where Task_IsExists=1 ");
            if (taskDataMap != null)
            {
                //任务名字不为空
                if (!string.IsNullOrEmpty(taskDataMap.Task_Name))
                {
                    selectSql.Append("and Task_Name like @0", "%" + taskDataMap.Task_Name + "%");
                }
                //任务类型不为空
                if (!string.IsNullOrEmpty(taskDataMap.Task_TaskType))
                {
                    if (taskDataMap.Task_TaskType == "all")
                    {
                        selectSql.Append("and (Task_TaskType = @0 or  Task_TaskType = @1)", "OneOff", "TimedTask");
                    }
                    else
                    {
                        selectSql.Append("and Task_TaskType like @0", "%" + taskDataMap.Task_TaskType + "%");
                    }
                   
                }
                //业务类型不为空
                if (!string.IsNullOrEmpty(taskDataMap.Task_BusinessType))
                {
                    selectSql.Append("and Task_BusinessType like @0", "%" + taskDataMap.Task_BusinessType + "%");
                }
                //创建时间不为空
                if (!string.IsNullOrEmpty(taskDataMap.Task_CreateTime))
                {
                    selectSql.Append("and Task_CreateTime like @0", "%" + taskDataMap.Task_CreateTime + "%");
                }
                //预定时间不为空
                if (!string.IsNullOrEmpty(taskDataMap.Task_PresetTime))
                {
                    selectSql.Append("and Task_PresetTime like @0", "%" + taskDataMap.Task_PresetTime + "%");
                }
                //时间间隔不为空
                if (!string.IsNullOrEmpty(taskDataMap.Task_Interval))
                {
                    selectSql.Append("and Task_Interval like @0", "%" + taskDataMap.Task_Interval + "%");
                }
                ////间隔时间
                //if (!string.IsNullOrEmpty(taskDataMap.Task_IntervalTime))
                //{
                //    selectSql.Append("and Task_Interval like @0", "%" + taskDataMap.Task_IntervalTime + "%");
                //}
                
            }

            //完成状态不为空
            if (!string.IsNullOrEmpty(taskDataMap.Task_ExecuteReuslt))
            {
                if (Convert.ToInt32(taskDataMap.Task_ExecuteReuslt) == 0 || Convert.ToInt32(taskDataMap.Task_ExecuteReuslt) == 1)
                {
                    selectSql.Append("and Task_ExecuteReuslt like @0", taskDataMap.Task_ExecuteReuslt);
                }
                else
                {
                    selectSql.Append("and (Task_ExecuteReuslt like 0 or Task_ExecuteReuslt like 1)");
                }
                
            }
            //创建时间
            selectSql.Append(PeriodOfTimeQuery("Task_CreateTime", taskDataMap.CreatTimeStart, taskDataMap.CreatTimeEnd));
            //预定时间
            selectSql.Append(PeriodOfTimeQuery("Task_PresetTime", taskDataMap.TaskPresetTimeStart, taskDataMap.TaskPresetTimeEnd));
            //执行时间
            selectSql.Append(PeriodOfTimeQuery("Task_LastExecuteTime", taskDataMap.TaskLastExecuteTimeStart, taskDataMap.TaskLastExecuteTimeEnd));
            return selectSql;
        }
        /// <summary>
        /// 更新赋值
        /// </summary>
        /// <param name="selectData">要更新的数据</param>
        /// <param name="updateRequestData">更新的内容</param>
        /// <returns></returns>
        public TaskData ChangeData(TaskData selectData, UpdateRequestData updateRequestData)
        {
            if (!string.IsNullOrEmpty(updateRequestData.Task_Name)) selectData.Task_Name = updateRequestData.Task_Name;
            if (!string.IsNullOrEmpty(updateRequestData.Task_BusinessType)) selectData.Task_BusinessType = updateRequestData.Task_BusinessType;
            if (!string.IsNullOrEmpty(updateRequestData.Task_PresetTime)) selectData.Task_PresetTime = updateRequestData.Task_PresetTime;
            if (!string.IsNullOrEmpty(updateRequestData.Task_Interval)) selectData.Task_Interval = updateRequestData.Task_Interval;
            if (!string.IsNullOrEmpty(updateRequestData.Task_Describe)) selectData.Task_Describe = updateRequestData.Task_Describe;
            return selectData;
        }
        public bool CheckDataIsUpdate(TaskData selectData)
        {
            if (selectData.Task_Isexists == 0) return false;
            if (selectData.Task_TaskType == "OneOff" && selectData.Task_ExecuteReuslt == 1) return false;
            return true;
        }
        public List<int> ConversionTime(string Date)
        {
            List<int> TimeSpanDate = new List<int>();
            foreach (var item in Date.Split(":"))
            {
                TimeSpanDate.Add(Convert.ToInt32(item));
            }
            return TimeSpanDate;
        }

        public  string PeriodOfTimeQuery(string queryField, string startTime, string endTIme)
        {
            string message = "";
            if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTIme))
            {
                message = $"and {queryField} between '{startTime}' and '{endTIme}'";
            }
            else if (!string.IsNullOrEmpty(startTime))
            {
                message = $"and {queryField} > '{startTime}'";
            }
            else if (!string.IsNullOrEmpty(endTIme))
            {
                message = $"and {queryField} < '{endTIme}'";
            }

            return message;
        }
    }
}
