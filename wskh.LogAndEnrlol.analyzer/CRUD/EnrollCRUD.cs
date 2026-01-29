using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.Core;

namespace wskh.LogAndEnrlol.analyzer.CRUD
{
    public class EnrollCRUD
    {
        #region Propertices
        private string _connectionString;
        public SqlConnection sqlConnection;
        #endregion
        #region Ctor
        public EnrollCRUD(string connectionString)
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
        public void Create(Enroll entity)
        {
            try
            {
                Connect();
                SqlCommand command = new SqlCommand($"INSERT INTO Enrolls (FingerDeviceId, wskhUserId, EnrollNo, Name, Password, Privileg, Enabled, CreateDate) VALUES (@FingerDeviceId, @wskhUserId, @EnrollNo, @Name, @Password, @Privileg, @Enabled, @DeviceId);SELECT CAST(scope_identity() AS int)", sqlConnection);

                command.Parameters.Add("@FingerDeviceId", SqlDbType.Int).Value = entity.FingerDeviceId;
                //command.Parameters.Add("@wskhUserId", SqlDbType.Int).Value = entity.wskhUserId;
                command.Parameters.Add("@EnrollNo", SqlDbType.Int).Value = entity.EnrollNo;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = entity.Name;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = entity.Password;
                command.Parameters.Add("@Privileg", SqlDbType.Int).Value = entity.Privileg;
                command.Parameters.Add("@Enabled", SqlDbType.Bit).Value = entity.Enabled;

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
        public List<Enroll> List()
        {
            List<Enroll> list = new List<Enroll>();
            Connect();
            SqlCommand command = new SqlCommand($"SELECT * FROM Enrolls", sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    Enroll Enroll = new Enroll();

                    Enroll.Id = (int)reader[0];
                    Enroll.FingerDeviceId = (int)reader[1];
                    Enroll.EnrollNo = (int)reader[3];
                    Enroll.Name = (string)reader[4];
                    Enroll.Password = (string)reader[5];
                    Enroll.Privileg = (int)reader[6];
                    Enroll.Enabled = (bool)reader[7];
                    Enroll.CreateDate = DateTime.Now;
                    Enroll.UpdateDate = null;

                    list.Add(Enroll);
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
        public bool Edit(Enroll entity)
        {
            bool result = false;
            try
            {
                Connect();
                int enable = entity.Enabled == true ? 1 : 0;
                SqlCommand command = new SqlCommand($"Update Enrolls SET EnrollNo = {entity.EnrollNo}, Name = '{entity.Name}', Password = {entity.Password}, Privileg = {entity.Privileg}, Enabled = {enable}, DeviceId = {entity.FingerDeviceId}, wskhUserId = 0, UpdateDate = {DateTime.Now} WHERE Id = {entity.Id};", sqlConnection);

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
                SqlCommand command = new SqlCommand($"DELETE FROM Enrolls  WHERE Id = {id};", sqlConnection);

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

                bulkCopy.DestinationTableName = "Enrolls";
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
        #endregion
    }
}
