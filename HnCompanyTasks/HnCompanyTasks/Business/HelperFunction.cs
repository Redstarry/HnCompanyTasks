using HnCompanyTasks.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public string CheckDateTime(double Seconds)
        {
            int ss = Convert.ToInt32(Seconds);
            TimeSpan ts = new TimeSpan(0,0, ss);
            var day = ts.TotalDays;
            return $" {day}天,  {ts.Hours}时, {ts.Minutes}分, {ts.Seconds}秒，后执行";
        }
    }
}
