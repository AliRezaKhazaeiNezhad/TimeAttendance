using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.WebEssentials.DataTablePart
{
    public class DataTableRequestFilter
    {
        public string Search { get; set; }
        public int SortColumn { get; set; }
        public string SortDirection { get; set; }
    }
}
