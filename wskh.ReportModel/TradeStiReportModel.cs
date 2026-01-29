using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wskh.ReportModel
{
    public class TradeStiReportModel : BaseStiReportModel
    {
        public TradeStiReportModel()
        {

        }

        public string Index { get; set; }
        public string GroupName { get; set; }
        public string NameAndFamily { get; set; }
        public string Date { get; set; }
        public string Trades { get; set; }
        public string State { get; set; }


        public string PersonalCode{ get; set; }
        public string DayName { get; set; }

        public string TradeOne { get; set; }
        public string TradeTwo { get; set; }
        public string Duration { get; set; }

        public string EnteranceDevice { get; set; }
        public string ExitDevice { get; set; }







        public string PersianDayName { get; set; }
        public string LiveTime { get; set; }
        public string LowWorkTime { get; set; }
        public string DelayEnterance { get; set; }
        public string EarlyEnterance { get; set; }
        public string DelayExit { get; set; }
        public string EarlyExit { get; set; }
        public string LegalTime { get; set; }
        

        public string TotalLiveTime { get; set; }
        public string TotalLowWorkTime { get; set; }
        public string TotalEarlyEnterance { get; set; }
        public string TotalEarlyExit { get; set; }
        public string TotalDelayEnterance { get; set; }
        public string TotalDelayExit { get; set; }
        public string TotalLegalTime { get; set; }
    }
}
