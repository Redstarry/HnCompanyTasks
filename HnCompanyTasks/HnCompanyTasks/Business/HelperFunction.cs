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

        public Sql SqlAssembly( TaskData taskDataMap )
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
                    selectSql.Append("and Task_TaskType like @0", "%" + taskDataMap.Task_TaskType + "%");
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
                //间隔时间
                if (!string.IsNullOrEmpty(taskDataMap.Task_IntervalTime))
                {
                    selectSql.Append("and Task_Interval like @0", "%" + taskDataMap.Task_IntervalTime + "%");
                }
                
            }

            //完成状态不为空
            if (taskDataMap.Task_ExecuteReuslt == 0 || taskDataMap.Task_ExecuteReuslt == 1)
            {
                selectSql.Append("and Task_ExecuteReuslt like @0", taskDataMap.Task_ExecuteReuslt);
            }
            return selectSql;
        }

        public TaskData ChangeData(TaskData RequestData, TaskData selectData)
        {
            if (string.IsNullOrEmpty(RequestData.Task_Name))
            {
                RequestData.Task_Name = selectData.Task_Name;
            }
            if (string.IsNullOrEmpty(RequestData.Task_TaskType))
            {
                RequestData.Task_TaskType = selectData.Task_TaskType;
            }
            if (string.IsNullOrEmpty(RequestData.Task_BusinessType))
            {
                RequestData.Task_BusinessType = selectData.Task_BusinessType;
            }
            if (string.IsNullOrEmpty(RequestData.Task_Describe))
            {
                RequestData.Task_Describe = selectData.Task_Describe;
            }
            RequestData.Task_CreateTime = selectData.Task_CreateTime;
            return RequestData;
        }
    }
}
