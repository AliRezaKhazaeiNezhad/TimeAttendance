using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeAttendance.Core;

namespace wskh.LogAndEnrlol.analyzer.CRUD
{
    public class RawEnrollCRUD
    {
        #region Propertices
        private string _connectionString;
        public SqlConnection sqlConnection;
        #endregion
        #region Ctor
        public RawEnrollCRUD(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion
        #region Private Methods
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
        #region Methods
        /// <summary>
        /// ثبت
        /// </summary>
        /// <param name="entity"></param>
        public void Create(RawEnroll entity)
        {
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"INSERT INTO RawEnrolls (EnrollNo, Name, Password, Privileg, Enabled, DeviceId) VALUES (@EnrollNo, @Name, @Password, @Privileg, @Enabled, @DeviceId);SELECT CAST(scope_identity() AS int)", sqlConnection);

                command.Parameters.Add("@EnrollNo", SqlDbType.Int).Value = entity.EnrollNo;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = entity.Password;
                command.Parameters.Add("@Privileg", SqlDbType.Int).Value = entity.Privileg;
                command.Parameters.Add("@Enabled", SqlDbType.Bit).Value = entity.Enabled;
                command.Parameters.Add("@DeviceId", SqlDbType.Int).Value = entity.DeviceId;

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
            }
            DisConnect();
        }

        /// <summary>
        /// گزارش
        /// </summary>
        /// <returns></returns>
        public List<RawEnroll> List()
        {
            List<RawEnroll> list = new List<RawEnroll>();
            Connect();
            SqlCommand command = new SqlCommand($"SELECT * FROM RawEnrolls", sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    RawEnroll RawEnroll = new RawEnroll();
                    RawEnroll.Id = (int)reader[0];
                    RawEnroll.EnrollNo = (int)reader[1];
                    RawEnroll.Name = (string)reader[2];
                    RawEnroll.Password = (string)reader[3];
                    RawEnroll.Privileg = (int)reader[4];
                    RawEnroll.Enabled = (bool)reader[5];
                    RawEnroll.DeviceId = (int)reader[6];

                    list.Add(RawEnroll);
                }
            }
            catch (Exception e)
            {
            }
            DisConnect();
            return list;
        }

        /// <summary>
        /// ویرایش
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Edit(RawEnroll entity)
        {
            bool result = false;
            try
            {
                Connect();
                int enable = entity.Enabled == true ? 1 : 0;
                SqlCommand command = new SqlCommand($"Update RawEnrolls SET EnrollNo = {entity.EnrollNo}, Name = '{entity.Name}', Password = {entity.Password}, Privileg = {entity.Privileg}, Enabled = {enable}, DeviceId = {entity.DeviceId} WHERE Id = {entity.Id};", sqlConnection);

                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            DisConnect();
            return result;
        }

        /// <summary>
        /// حذف
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE FROM RawEnrolls  WHERE Id = {id};", sqlConnection);

                command.ExecuteNonQuery();
                result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            DisConnect();
            return result;
        }

        /// <summary>
        /// درج عظیم داده ها با دیتاتیبل
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public bool BulkInsert(DataTable dataTable)
        {
            bool result = false;
            try
            {
                Connect();
                SqlBulkCopy bulkCopy = new SqlBulkCopy(
                         sqlConnection,
                         SqlBulkCopyOptions.TableLock |
                         SqlBulkCopyOptions.FireTriggers |
                         SqlBulkCopyOptions.UseInternalTransaction,
                         null
                         );

                bulkCopy.DestinationTableName = "RawEnrolls";
                bulkCopy.WriteToServer(dataTable);
                result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            DisConnect();
            return result;
        }



        /// <summary>
        /// حذف کاربران غیرمعتبر
        /// </summary>
        /// <returns></returns>
        public bool RemoveFractions()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"Delete From RawEnrolls Where EnrollNo = 0 AND Enabled = 0", sqlConnection);
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
        public bool RemoveDuplicate()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE n1 FROM RawEnrolls n1, RawEnrolls n2 WHERE n1.id > n2.id  AND n1.EnrollNo = n2.EnrollNo AND n1.Name = n2.Name AND n1.Password = n2.Password AND n1.Privileg = n2.Privileg AND n1.Enabled = n2.Enabled AND n1.DeviceId = n2.DeviceId", sqlConnection);
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
        public bool RemoveClones()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"DELETE rawenrolls FROM RawEnrolls rawenrolls, Enrolls enrolls WHERE rawenrolls.DeviceId = enrolls.FingerDeviceId AND rawenrolls.EnrollNo = enrolls.EnrollNo AND rawenrolls.Name = enrolls.Name AND rawenrolls.Password = enrolls.Password AND rawenrolls.Privileg = enrolls.Privileg AND rawenrolls.Enabled = enrolls.Enabled", sqlConnection);
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
        /// درج انرول های خام به انرول پرازش شده
        /// </summary>
        /// <returns></returns>
        public bool InsertToEnrolls()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"Insert Into Enrolls (FingerDeviceId, EnrollNo, Name, Password, Privileg, Enabled, CreateDate) Select rawenrolls.DeviceId, rawenrolls.EnrollNo, rawenrolls.Name, rawenrolls.Password, rawenrolls.Privileg, rawenrolls.Enabled, getDate()  From RawEnrolls rawenrolls WHERE NOT EXISTS(SELECT Id FROM Enrolls enrolls WHERE enrolls.EnrollNo = rawenrolls.EnrollNo AND enrolls.Name = rawenrolls.Name AND enrolls.Password = rawenrolls.Password AND enrolls.Privileg = rawenrolls.Privileg AND enrolls.Enabled = rawenrolls.Enabled AND enrolls.FingerDeviceId = rawenrolls.DeviceId)", sqlConnection);
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
        /// در صورتیکه انرولی در حالت خام ، متفاوت با پردازش شده باشد، انرول را ویرایش میکند
        /// </summary>
        /// <returns></returns>
        public bool UpdateToEnrolls()
        {
            bool result = false;
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"UPDATE Enrolls SET enrolls.Name = rawenrolls.Name, enrolls.Password = rawenrolls.Password, enrolls.Privileg = rawenrolls.Privileg, enrolls.Enabled = rawenrolls.Enabled FROM Enrolls AS enrolls INNER JOIN RawEnrolls AS rawenrolls ON enrolls.EnrollNo = rawenrolls.EnrollNo AND enrolls.FingerDeviceId = rawenrolls.DeviceId WHERE enrolls.EnrollNo = rawenrolls.EnrollNo AND enrolls.FingerDeviceId = rawenrolls.DeviceId", sqlConnection);
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
}
