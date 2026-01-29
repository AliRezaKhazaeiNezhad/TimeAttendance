using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;
using wskh.Data;

namespace wskh.LogAndEnrlol.analyzer.SQL
{
    #region SQLRepository
    public class SQLRepository
    {
        #region Propertices
        private string _connectionString;
        private SqlConnection sqlConnection;
        private wskhContext _context;
        #endregion
        #region Ctor
        public SQLRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion
        #region Action Methods
        private void Connect()
        {
            sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
        }
        private void DisConnect()
        {
            sqlConnection.Dispose();
        }
        #endregion

        #region General Methods
        /// <summary>
        /// در این بخش آنالیز اولیه ورود و خروج ها را انجام میدهد
        /// </summary>
        public void LogPreAnalyze()
        {
            RawLogRemoveFractions();
            RawLogRemoveDuplicate();
            RawLogAddDateTime();
            LogsInsert();
            UpdateEnrollId();
            LogRemoveDuplicate();
            //DeleteLogsWithNoEnrollId();
            DeleteRawLogsWithNoEnrollId();
        }

        /// <summary>
        /// آنالیز اولیه انرول را انجام میدهد
        /// </summary>
        public void EnrollAnalyze()
        {
            RawEnrollRemoveFractions();
            RawEnrollRemoveDuplicate();
            RawEnrollRemoveDuplicateFromEnroll();
            UpdateEnrolls();
            EnrollsInsert();
            RawEnrollRemoveDuplicates();
            UpdateEnrollId();
        }

        /// <summary>
        /// آنالیز گزارش روزانه و تهیه قرارداد برای هر کاربر
        /// </summary>
        public void ReportDayAnalyze()
        {
            ReportDayCreate();
            UpdateReprotDaysEnrollId();
            ReportDayRemoveDuplicate();
            ReportDayAddIdToLogs();
            RemoveEnollIdNull();
            RemoveFrachtonLogs2();
        }

        /// <summary>
        /// مجموع سه آنالیز ورود و خروج، انرول و گزارش بترتیب
        /// </summary>
        public void FullAnanlyze()
        {
            LogPreAnalyze();
            EnrollAnalyze();
            ReportDayAnalyze();
        }
        #endregion

        #region Log Methods
        /// <summary>
        /// حذف ورود و خروج های خام  غیرمعتبر
        /// </summary>
        /// <returns></returns>
        private bool RawLogRemoveFractions()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"Delete From RawLogs WHERE Year > 2100 OR Year < 2000 OR (12 < Month OR Month < 1)  OR (31 < Day OR Day < 1) OR (24 < Hour OR Hour < 0)  OR (60 < Minute OR Minute < 0) OR (60 < Second OR Second < 0) OR DeviceId = 0; ", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// حذف ورود و خروج های تکراری 
        /// </summary>
        /// <returns></returns>
        private bool RawLogRemoveDuplicate()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE n1 FROM RawLogs n1, RawLogs n2 WHERE n1.id > n2.id  AND n1.EnrollNo = n2.EnrollNo AND n1.InOutMode = n2.InOutMode AND n1.VerifyMode = n2.VerifyMode AND n1.WorkCode = n2.WorkCode AND n1.DeviceId = n2.DeviceId AND n1.Year = n2.Year AND n1.Month = n2.Month AND n1.Day = n2.Day AND n1.Hour = n2.Hour AND n1.Minute = n2.Minute AND n1.Second = n2.Second", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// افزودن تاریخ به ردیف های بدون تاریخ در جدول RawLogs
        /// </summary>
        /// <returns></returns>
        private bool RawLogAddDateTime()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"UPDATE RawLogs Set RawLogs.LogTime = RawLogs.Hour + ':' + RawLogs.Minute + ':' + RawLogs.Second, RawLogs.LogDate = TRY_CONVERT(Date, (RawLogs.Month + '-' + RawLogs.Day + '-' + RawLogs.Year)) WHERE RawLogs.LogDate IS NULL OR RawLogs.LogTime IS NULL", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// حذف ورود و خروج ها خام در صورت وجود در ورود و خروج های پردازش شده
        /// </summary>
        /// <returns></returns>
        private bool LogRemoveDuplicate()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE n1 FROM Logs n1, Logs n2 WHERE n1.id > n2.id  AND n1.EnrollNo = n2.EnrollNo AND n1.InOutMode = n2.InOutMode AND n1.VerifyMode = n2.VerifyMode AND n1.WorkCode = n2.WorkCode AND n1.DeviceId = n2.DeviceId AND n1.LogDate = n2.LogDate AND n1.LogTime = n2.LogTime", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// بروزرسانی ای دی انرول
        /// </summary>
        /// <returns></returns>
        private bool UpdateEnrollId()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"UPDATE Logs SET Logs.EnrollId = Enrolls.Id  FROM Logs INNER JOIN Enrolls ON Enrolls.EnrollNo = Logs.EnrollNo AND Enrolls.FingerDeviceId = Logs.DeviceId WHERE Enrolls.EnrollNo = Logs.EnrollNo AND Enrolls.FingerDeviceId = Logs.DeviceId", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// افزودن ورود و خروج ها از جدول ورود و خروج خام
        /// </summary>
        /// <returns></returns>
        private bool LogsInsert()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"INSERT INTO Logs (VerifyMode, InOutMode, WorkCode, Orgin, State, TransportType, ReportDayId, LogDate, EnrollId, LogTime, DeviceId, EnrollNo, Remove, CreatorUserId) SELECT rawlog.VerifyMode, rawlog.InOutMode, rawlog.WorkCode, 1, 0, 0, null, rawlog.LogDate, rawlog.EnrollId, rawlog.LogTime, rawlog.DeviceId, rawlog.EnrollNo, 0, null FROM RawLogs rawlog", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// حذف ورود و خروج هایی که انرول پردازش شده ندارند
        /// </summary>
        /// <returns></returns>
        private bool DeleteLogsWithNoEnrollId()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE FROM Logs WHERE LogDate != '' AND LogTime != '' AND EnrollId IS NULL", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// حذف ورود و خروج هایی که انرول پردازش شده ندارند
        /// </summary>
        /// <returns></returns>
        private bool DeleteRawLogsWithNoEnrollId()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE FROM RawLogs WHERE LogDate != '' AND LogTime != '' AND EnrollId IS NULL", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        #endregion
        #region Enroll Methods
        /// <summary>
        /// حذف انرول های خام  غیرمعتبر
        /// </summary>
        /// <returns></returns>
        private bool RawEnrollRemoveFractions()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"Delete From RawEnrolls WHERE EnrollNo = 0;", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// حذف انرول های تکراری 
        /// </summary>
        /// <returns></returns>
        private bool RawEnrollRemoveDuplicate()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE n1 FROM RawEnrolls n1, RawEnrolls n2 WHERE n1.Id > n2.Id AND n1.DeviceId = n2.DeviceId AND n1.EnrollNo = n2.EnrollNo AND n1.Name = n2.Name AND n1.Password = n2.Password AND n1.Privileg = n2.Privileg AND n1.Enabled = n2.Enabled", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// حذف انرول های تکراری براساس جدول انرول 
        /// </summary>
        /// <returns></returns>
        private bool RawEnrollRemoveDuplicateFromEnroll()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE RawEnrolls FROM RawEnrolls INNER JOIN Enrolls ON RawEnrolls.EnrollNo = Enrolls.EnrollNo AND RawEnrolls.DeviceId = Enrolls.FingerDeviceId WHERE RawEnrolls.EnrollNo = Enrolls.EnrollNo AND RawEnrolls.DeviceId = Enrolls.FingerDeviceId AND RawEnrolls.EnrollNo = Enrolls.EnrollNo AND RawEnrolls.Name = Enrolls.Name AND RawEnrolls.Password = Enrolls.Password AND RawEnrolls.Privileg = Enrolls.Privileg AND RawEnrolls.Enabled = Enrolls.Enabled", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// حذف انرول ها خام در صورت وجود در انرول های پردازش شده
        /// </summary>
        /// <returns></returns>
        private bool RawEnrollRemoveDuplicates()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE rawenrolls FROM RawEnrolls rawenrolls, Enrolls enrolls  WHERE rawenrolls.DeviceId = enrolls.FingerDeviceId AND rawenrolls.EnrollNo = enrolls.EnrollNo AND rawenrolls.Name = enrolls.Name AND rawenrolls.Password = enrolls.Password  AND rawenrolls.Privileg = enrolls.Privileg AND rawenrolls.Enabled = enrolls.Enabled ", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// بروزرسانی انرول ها
        /// </summary>
        /// <returns></returns>
        private bool UpdateEnrolls()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"UPDATE enroll SET enroll.Name = rawenrolls.Name, enroll.Password = rawenrolls.Password, enroll.Privileg = rawenrolls.Privileg, enroll.Enabled = rawenrolls.Enabled FROM Enrolls AS enroll INNER JOIN RawEnrolls rawenrolls ON enroll.EnrollNo = rawenrolls.EnrollNo AND enroll.FingerDeviceId = rawenrolls.DeviceId WHERE enroll.EnrollNo = rawenrolls.EnrollNo AND enroll.FingerDeviceId = rawenrolls.DeviceId; ", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        /// <summary>
        /// افزودن انرول ها از جدول انرول خام
        /// </summary>
        /// <returns></returns>
        private bool EnrollsInsert()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"INSERT INTO Enrolls (EnrollNo, Name, Password, Privileg, Enabled, CreateDate, UpdateDate, FingerDeviceId)  SELECT rawEnrolls.EnrollNo, rawEnrolls.Name, rawEnrolls.Password, rawEnrolls.Privileg, rawEnrolls.Enabled, GETDATE(), null, rawEnrolls.DeviceId FROM RawEnrolls rawEnrolls WHERE NOT EXISTS(SELECT * FROM Enrolls en2 WHERE rawEnrolls.EnRollNo = en2.EnrollNo AND rawEnrolls.DeviceId = en2.FingerDeviceId)", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        #endregion
        #region Report Maker
        /// <summary>
        /// ایجاد روز به ازای ورود و خروج ها
        /// </summary>
        /// <returns></returns>
        private bool ReportDayCreate()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"INSERT INTO ReportDays (Remove, ReportDate, EnrollId, DeviceId, EnrollNo) SELECT 0, logs.LogDate, logs.EnrollId, logs.DeviceId, logs.EnrollNo FROM Logs AS logs WHERE NOT EXISTS (SELECT * FROM ReportDays as rds WHERE rds.EnrollId = logs.EnrollId AND rds.EnrollNo = logs.EnrollNo AND rds.DeviceId = logs.DeviceId  AND rds.ReportDate = logs.LogDate); ", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }

        /// <summary>
        /// بروزرسانی Enroll ID
        /// </summary>
        /// <returns></returns>
        private bool UpdateReprotDaysEnrollId()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"Update rds Set EnrollId = enroll.Id FROM ReportDays rds INNER JOIN Enrolls as enroll on rds.EnrollId = enroll.Id where enroll.EnrollNo = rds.EnrollNo AND enroll.FingerDeviceId = rds.DeviceId", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }

        /// <summary>
        /// حذف روزهای تکراری
        /// </summary>
        /// <returns></returns>
        private bool ReportDayRemoveDuplicate()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE n1 FROM ReportDays n1, ReportDays n2 WHERE n1.id > n2.id AND n1.ReportDate = n2.ReportDate AND n1.DeviceId = n2.DeviceId AND n1.EnrollId = n2.EnrollId AND n1.ContractId IS NULL", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }

        /// <summary>
        /// افزودن ای دی گزارش روزانه به ورود و خروج ها
        /// </summary>
        /// <returns></returns>
        private bool ReportDayAddIdToLogs()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"Update logs Set logs.ReportDayId = reportdays.Id From Logs logs  INNER JOIN ReportDays reportdays  ON logs.LogDate = reportdays.ReportDate AND logs.DeviceId = reportdays.DeviceId AND logs.EnrollId = reportdays.EnrollId", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }

        /// <summary>
        /// حذف انرول های Null
        /// </summary>
        /// <returns></returns>
        private bool RemoveEnollIdNull()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE FROM ReportDays WHERE EnrollId IS NULL", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }

        /// <summary>
        /// حذف ورود و خروج هایی که انرول انها براساس دستگاه آنها وجود ندارد یا بطور کلی انرول آنها قبلا از روی دستگاه حذف شذه و ورود و خروج آنها مانده است
        /// </summary>
        /// <returns></returns>
        private bool RemoveFrachtonLogs2()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE FROM Logs FROM Logs LEFT OUTER JOIN Enrolls ON Logs.DeviceId = Enrolls.FingerDeviceId AND Logs.EnrollNo = Enrolls.EnrollNo WHERE Logs.EnrollId IS NULL", sqlConnection);
                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return result;
        }
        #endregion
    }
    #endregion
}
