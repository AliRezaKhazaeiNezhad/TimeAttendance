CREATE procedure fullLogReport
AS
SELECT  
	   logs.Id
      ,logs.VerifyMode
      ,logs.InOutMode
      ,logs.WorkCode
      ,logs.Orgin
      ,logs.LogDate
      ,logs.LogTime
      ,logs.State
      ,logs.TransportType
      ,logs.ReportDayId
      ,logs.EnrollId
      ,logs.DeviceId
      ,logs.EnrollNo

	  , enrolls.Name
	  , enrolls.Password
	  , enrolls.Privileg
	  , enrolls.Enabled
	  , enrolls.FingerDeviceId
	  , enrolls.UserId

	  ,fingerdevices.Title
	  ,fingerdevices.ModelName

	  ,users.FirstName
	  ,users.Lastname
	  ,users.NationalCode
	  ,users.Active
	  ,users.Sex

  FROM Logs logs

  INNER JOIN Enrolls as enrolls
  ON enrolls.EnrollNo = logs.EnrollNo

  INNER JOIN FingerDevices as fingerdevices
  ON fingerdevices.Id = enrolls.FingerDeviceId


  INNER JOIN AspNetUsers as users
  ON users.Id = enrolls.UserId
  Where logs.LogDate >  '1990-01-01'