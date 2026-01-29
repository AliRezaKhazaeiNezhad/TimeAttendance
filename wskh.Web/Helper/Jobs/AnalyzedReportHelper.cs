using OrdinaryWorkProgram.anly;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TimeAttendance.Core;
using wskh.Core.Enumerator;
using wskh.Data;
using wskh.Service;
using wskh.WebEssentials.DateAndTime;

namespace wskh.Web.Helper.Jobs
{
    public static class AnalyzedReportHelper
    {
        public static async void Initial()
        {

            try
            {
                bool addReportDay = await AddAnalyzedReportDay();
            }
            catch (Exception e)
            {
            }

        }

        #region 
        private static async Task<bool> AddAnalyzedReportDay()
        {
            bool result = false;

            try
            {
                Analyzer analyzer = new Analyzer();

                analyzer.Initial();

                result = true;
            }
            catch (Exception e)
            {

            }

            return result;
        }
        #endregion

    }
}

