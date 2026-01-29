using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeAttendance.Model
{
    public class EnrollModels
    {
        public EnrollModels()
        {

        }

        public int Id { get; set; }
        public int Index { get; set; }
        public int EnrollNo { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Privileg { get; set; }
        public string Enabled { get; set; }
    }
}
