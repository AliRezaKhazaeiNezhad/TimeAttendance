using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.FingerTec.Models
{
    public class RawLogModel
    {
        public RawLogModel()
        {

        }
        public int Id { get; set; }
        public string EnrollNo { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Second { get; set; }
        public string VerifyMode { get; set; }
        public string InOutMode { get; set; }
        public string WorkCode { get; set; }
        public int DeviceId { get; set; }
    }
}
