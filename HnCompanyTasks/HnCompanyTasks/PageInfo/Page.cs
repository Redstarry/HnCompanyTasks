using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsAPI.Models.PageModel
{
    public class Page
    {
        private const int MaxPageSize = 20;
        public int PageNumber { get; set; } = 1;
        private int _PageSize = 5;

        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize:value;
        }

    }
}
