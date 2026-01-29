using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.WebEssentials.DataTablePart
{
    public class DataTableResponse<T> where T : class
    {
        public DataTableResponse()
        {
            data = new List<T>();
        }
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public List<T> data { get; set; }
    }
}
