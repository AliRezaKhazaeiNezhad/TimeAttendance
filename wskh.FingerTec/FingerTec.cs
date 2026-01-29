using BioBridgeSDKDLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wskh.FingerTec.Models;

namespace wskh.FingerTec
{
    public class FingerTec
    {
        #region Propertices
        private ConnectionType _connectionType { get; set; }
        private string _deviceModel { get; set; }
        private string _ip { get; set; }
        private int _deviceNo { get; set; }
        private int _commKey { get; set; }
        private int _portNumber { get; set; }
        private int _bundleRate { get; set; }
        private BioBridgeSDKClass _sdk { get; set; }
        public int LogCount { get; set; }
        #endregion
        #region Ctor
        public FingerTec(ConnectionType connectionType, string deviceModel, string ip = null, int deviceInnerId = 0, int deviceNo = 0, int commKey = 0, int portNumber = 0, int bundleRate = 0)
        {
            _connectionType = connectionType;
            _ip = ip;
            _deviceNo = deviceNo <= 0 ? 1 : deviceNo;
            _commKey = commKey <= 0 ? 0 : commKey;
            _deviceModel = deviceModel;
            _portNumber = portNumber <= 0 ? 4370 : portNumber;
            _bundleRate = bundleRate;
            //_sdk.OnFinger += OnFinger;
            LogCount = 0;

        }
        public FingerTec()
        {

        }
        #endregion
        #region Connection

        public bool Connect()
        {
            _sdk = new BioBridgeSDKClass();
            bool result = false;
            switch (_connectionType)
            {
                case ConnectionType.TCP:
                    result = _sdk.Connect_TCPIP(_deviceModel, _deviceNo, _ip, _portNumber, _commKey) == 0 ? true : false;
                    return result;
                case ConnectionType.USB:
                    result = _sdk.Connect_USB(_deviceModel, _deviceNo, _commKey) == 0 ? true : false;
                    return result;
                case ConnectionType.COM:
                    result = _sdk.Connect_COMM(_deviceModel, _deviceNo, _portNumber, _bundleRate, _commKey) == 0 ? true : false;
                    return result;
                default:
                    result = _sdk.Connect_TCPIP(_deviceModel, _deviceNo, _ip, _portNumber, _commKey) == 0 ? true : false;
                    return result;
            }
        }
        #endregion
        #region DisConnect
        public void DisConnect()
        {
            _sdk.Disconnect();
        }
        #endregion
        #region RestartDevice
        public void RestartDevice()
        {
            _sdk.RestartDevice();
        }
        #endregion
        #region SetDateTime
        public void SetDateTime(DateTime dateTime)
        {

            _sdk.SetDeviceTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }
        #endregion
        #region ClearAdministrator
        public void ClearAdministrator()
        {
            _sdk.ClearAdministrator();
        }
        #endregion
        #region DeleteGeneralLog
        public void DeleteGeneralLog()
        {
            _sdk.DeleteGeneralLog();
        }
        #endregion
        #region ClearAllData
        public void ClearAllData()
        {
            _sdk.ClearAllData();
        }
        #endregion
        #region GetSDKVersion
        public string GetSDKVersion()
        {
            string result = "";
            _sdk.GetSDKVersion(ref result, 0);
            return result;
        }
        #endregion
        #region GetManufacturer
        public string GetManufacturer()
        {
            string result = "";
            result = _sdk.VC;
            return result;
        }
        #endregion
        #region GetSerialNumber
        public string GetSerialNumber()
        {
            string result = "";
            result = _sdk.SN;
            return result;
        }
        #endregion
        #region GetMac
        public string GetMacAddress()
        {
            string result = "";
            result = _sdk.MAC;
            return result;
        }
        #endregion
        #region GetModel
        public string GetModel()
        {
            string result = "";
            result = _sdk.DC;
            return result;
        }
        #endregion
        #region GetFTPDescription
        public string GetFTPDescription()
        {
            string result = "";
            result = _sdk.Finger10 == 1 ? "FTP10" : "Not FTP10";
            return result;
        }
        #endregion
        #region IsColorScreen
        public bool IsColorScreen()
        {
            bool result = false;
            result = _sdk.IsTFT() == 0 ? true : false;
            return result;
        }
        #endregion



        #region GetFirmwareVersion
        public string GetFirmwareVersion()
        {
            string result = "";
            _sdk.GetFirmwareVersion(ref result);
            return result;
        }
        #endregion
        #region ReadGeneralLog
        public int ReadGeneralLog()
        {
            int result = 0;
            _sdk.ReadGeneralLog(ref result);
            return result;
        }
        #endregion
        #region GetLogs
        public DataTable GetLogs(DataTable dataTable, int deviceId)
        {
            #region در این بخش ستون های جدول تعریف میشوند
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("EnrollNo", typeof(string));
            dataTable.Columns.Add("Year", typeof(string));
            dataTable.Columns.Add("Month", typeof(string));
            dataTable.Columns.Add("Day", typeof(string));
            dataTable.Columns.Add("Hour", typeof(string));
            dataTable.Columns.Add("Minute", typeof(string));
            dataTable.Columns.Add("Second", typeof(string));
            dataTable.Columns.Add("VerifyMode", typeof(string));
            dataTable.Columns.Add("InOutMode", typeof(string));
            dataTable.Columns.Add("WorkCode", typeof(string));
            dataTable.Columns.Add("DeviceId", typeof(int));
  

            #endregion

            List<RawLogModel> modelList = new List<RawLogModel>();
            try
            {
                string enrollNo = "";
                int enrollNoInt = 0;
                int yr = 0;
                int mth = 0;
                int day_Renamed = 0;
                int hr = 0;
                int min = 0;
                int sec = 0;
                int ver = 0;
                int io = 0;
                int work = 0;
                int logSize = 0;


                if (_sdk.ReadGeneralLog(ref logSize) == 0)
                {
                    if (_sdk.IsTFT() == 0)
                    {
                        do
                        {
                            DataRow dataRow = dataTable.NewRow();

                            dataRow["Id"] = 0;
                            dataRow["EnrollNo"] = enrollNo;
                            dataRow["Year"] = yr.ToString();
                            dataRow["Month"] = mth.ToString();
                            dataRow["Day"] = day_Renamed.ToString();
                            dataRow["Hour"] = hr.ToString();
                            dataRow["Minute"] = min.ToString();
                            dataRow["Second"] = sec.ToString();
                            dataRow["VerifyMode"] = ver.ToString();
                            dataRow["InOutMode"] = io.ToString();
                            dataRow["WorkCode"] = work.ToString();
                            dataRow["DeviceId"] = deviceId;

                            
                          


                            dataTable.Rows.Add(dataRow);

                        } while (_sdk.SSR_GetGeneralLog(ref enrollNo, ref yr, ref mth, ref day_Renamed, ref hr, ref min, ref sec, ref ver, ref io, ref work) == 0);
                    }
                    else
                    {
                        do
                        {
                            DataRow dataRow = dataTable.NewRow();

                            dataRow["Id"] = 0;
                            dataRow["EnrollNo"] = enrollNoInt.ToString();
                            dataRow["Year"] = yr.ToString();
                            dataRow["Month"] = mth.ToString();
                            dataRow["Day"] = day_Renamed.ToString();
                            dataRow["Hour"] = hr.ToString();
                            dataRow["Minute"] = min.ToString();
                            dataRow["Second"] = sec.ToString();
                            dataRow["VerifyMode"] = ver.ToString();
                            dataRow["InOutMode"] = io.ToString();
                            dataRow["WorkCode"] = work.ToString();
                            dataRow["DeviceId"] = deviceId;


                            dataTable.Rows.Add(dataRow);

                        } while (_sdk.GetGeneralLog(ref enrollNoInt, ref yr, ref mth, ref day_Renamed, ref hr, ref min, ref sec, ref ver, ref io, ref work) == 0);
                    }

                    LogCount = dataTable.Rows.Count;
                }
            }
            catch (Exception e)
            {
            }
            return dataTable;
        }
        #endregion
        #region GetEnrolls
        public List<EnrollModel> GetEnrolls()
        {
            int result = 0;
            List<EnrollModel> modelList = new List<EnrollModel>();
            try
            {
                int enrollNo = 0;
                string enrollNo2 = "";
                string name_Renamed = "";
                string pwd = "";
                int priv = 0;
                int size = 0;
                Boolean enable = false;
                int index = 0;

                if (_sdk.ReadAllUserInfo(ref size) == 0)
                {
                    if (IsColorScreen())
                    {
                        do
                        {
                            try
                            {
                                modelList.Add(new EnrollModel()
                                {
                                    EnrollNo = !string.IsNullOrEmpty(enrollNo2) ? int.Parse(enrollNo2) : 0,
                                    Enable = enable,
                                    Name = name_Renamed,
                                    PassWord = pwd,
                                    Privilage = priv
                                });
                            }
                            catch (Exception e)
                            {
                            }
                        } while (_sdk.SSR_GetAllUserInfo(ref enrollNo2, ref name_Renamed, ref pwd, ref priv, ref enable) == 0);
                    }else
                    {
                        do
                        {
                            try
                            {
                                modelList.Add(new EnrollModel()
                                {
                                    EnrollNo = enrollNo,
                                    Enable = enable,
                                    Name = name_Renamed,
                                    PassWord = pwd,
                                    Privilage = priv
                                });
                            }
                            catch (Exception e)
                            {
                            }
                        } while (_sdk.GetAllUserInfo(ref enrollNo, ref name_Renamed, ref pwd, ref priv, ref enable) == 0);
                    }
                }

            }
            catch (Exception e)
            {
            }
            return modelList;
        }
        #endregion

        #region SetLastLog
        public void SetLastLog(int ct)
        {
            _sdk.SetLastCount(ct);
        }
        #endregion


        #region InfiniteConnection
        public void InfiniteConnection()
        {
            _sdk = new BioBridgeSDKClass();
            if (_sdk.Connect_TCPIP("", 1, "192.168.1.221", 4370, 0) == 0)
            {
                RegisterEvent();
            }
        }

        public void RegisterEvent()
        {
            try
            {
                _sdk.OnFinger += _sdk_OnFinger;
            }
            catch (Exception e)
            {
            }
        }

        private void _sdk_OnFinger()
        {
            int x= 1;
            x = 2;
        }
        #endregion
    }
}
