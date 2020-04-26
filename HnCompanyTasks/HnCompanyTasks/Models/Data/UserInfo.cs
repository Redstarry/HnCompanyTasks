using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HnCompanyTasks.Models.Data
{
    [PetaPoco.TableName("UserInfo")]
    public class UserInfo
    {
        public string UserName { get; set; }
        public string Pwd { get; set; }
    }
}
