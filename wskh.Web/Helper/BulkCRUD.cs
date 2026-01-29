using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TimeAttendance.WebEssentials.CommandPart;
using wskh.Core;
using wskh.Core.Enumerator;
using wskh.FingerTec;
using wskh.FingerTec.Models;

namespace TimeAttendance.Web.Helper
{
    public static class BulkCRUD
    {
        public static bool InsertLog(FingerTec fingerTec, FingerDevice entity)
        {
            bool result = false;
            try
            {
                DataTable dataTable = new DataTable("RawLogs");
                dataTable = fingerTec.GetLogs(dataTable, entity.Id);

                using (SqlConnection connection = new SqlConnection(ConnectionHelper.Get()))
                {
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(
                        connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                        );

                    bulkCopy.DestinationTableName = "RawLogs";
                    connection.Open();

                    bulkCopy.WriteToServer(dataTable);
                    connection.Close();

                    CommandHelper.Create(CommandCategory.LogCommand, false, $"دریافت ورود و خروج ها - در دستگاه {entity.Title}", dataTable.Rows.Count, UserHelper.CurrentUser(), entity.Id);
                }
                dataTable.Clear();
                result = true;
            }
            catch (Exception)
            {
            }
            return result;
        }
    }
}