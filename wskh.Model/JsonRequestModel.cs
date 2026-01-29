using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.Model
{
    public class JsonRequestModel
    {
        public JsonRequestModel()
        {
            State = 0;
        }

        public int State { get; set; }
        public string Message { get; set; }
        public string HTML { get; set; }
    }
}
