using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.ReportModel
{
    public class Trade
    {
        public Trade()
        {
            TradeLogs = new List<TradeLog>();
        }

        public string PersianDayName { get; set; }
        public string PersianDate { get; set; }
        public string Index { get; set; }
        public string State { get; set; }


        public List<TradeLog> TradeLogs { get; set; }
    }
}
