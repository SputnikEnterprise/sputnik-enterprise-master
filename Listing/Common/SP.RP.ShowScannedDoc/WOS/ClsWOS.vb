
Imports System.Data.SqlClient
Imports SP.Infrastructure.Logging

Public Class ClsWOS
	Implements IClsWOS


	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsProgSetting As SPProgUtility.ClsProgSettingPath

	Private wosInfo As New ClsWOSInfomation


	Sub New(ByVal objWosInfo As ClsWOSInfomation)
		' TODO: Complete member initialization 
		Me.wosInfo = objWosInfo
		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath

	End Sub

	Function SendKDDoc2WOS() As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Erfolgreich"

		Dim strSqlQuery As String = "[Get RPData For Transfering To WS]"
		Dim bAllowedKDData2Send As Boolean = Me.wosInfo.SendRPToKDWOS
		Dim bAllowedMAData2Send As Boolean = Me.wosInfo.SendRPToMAWOS
		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@RPNr", Me.wosInfo.SelectedRPNr)
			param = cmd.Parameters.AddWithValue("@RPLNr", Me.wosInfo.SelectedRPLNr)
			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

			While rFoundedrec.Read
				Me.wosInfo.SelectedZHDNr = rFoundedrec("ZHDNr")
				Me.wosInfo.SelectedKDGuid = rFoundedrec("KD_Guid")
				Me.wosInfo.SelectedZHDGuid = rFoundedrec("ZHD_Guid")
				Me.wosInfo.SelectedMAGuid = rFoundedrec("MA_Guid")
				bAllowedKDData2Send = Me.wosInfo.SendRPToKDWOS AndAlso CBool(rFoundedrec("KDSend2Wos"))
				bAllowedMAData2Send = Me.wosInfo.SendRPToMAWOS AndAlso CBool(rFoundedrec("MASend2Wos"))
				Me.wosInfo.SelectedFilename = String.Empty
			End While

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
		Trace.WriteLine(String.Format("DocGuid: {0} / RPLNr: {1}", Me.wosInfo.SelectedDocGuid, Me.wosInfo.SelectedRPLNr))


		Try
			Dim obj As New spFileUploadUtil.ClsMain_Net(ClsDataDetail.m_InitialData)

			If bAllowedKDData2Send Then
				strResult = obj.SaveDataToKDRPScanDocDb(Me.wosInfo.SelectedKDNr,
																			 Me.wosInfo.SelectedZHDNr,
																			 Me.wosInfo.SelectedESNr,
																			 Me.wosInfo.SelectedESLohnNr,
																			 Me.wosInfo.SelectedRPNr,
																			 Me.wosInfo.SelectedRPLNr,
																			 Me.wosInfo.SelectedKDGuid,
																			 Me.wosInfo.SelectedZHDGuid,
																			 Me.wosInfo.SelectedDocGuid,
																			 "Rapport",
																			 String.Format("Rapport: {0}", Me.wosInfo.SelectedRPNr),
																			 False)
				strResult = obj.SendKDDocRecFromLocalTo_WS
			End If
			If bAllowedMAData2Send Then
				strResult = obj.SaveDataToMARPScanDocDb(Me.wosInfo.SelectedMANr,
																			 Me.wosInfo.SelectedESNr,
																			 Me.wosInfo.SelectedESLohnNr,
																			 Me.wosInfo.SelectedRPNr,
																			 Me.wosInfo.SelectedRPLNr,
																			 Me.wosInfo.SelectedMAGuid,
																			 Me.wosInfo.SelectedDocGuid,
																			 "Rapport",
																			 String.Format("Rapport: {0}", Me.wosInfo.SelectedRPNr),
																			 False)
				strResult = obj.SendDocRecFromLocalTo_WS
			End If

		Catch ex As Exception

		End Try

		Return strResult
	End Function

	Function DeleteDocFromWOS() As String Implements IClsWOS.DeleteDocFromWOS
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Erfolgreich"

		Try
			Dim obj As New spFileUploadUtil.ClsMain_Net(ClsDataDetail.m_InitialData)
			strResult = obj.DeleteDocWithGuidFrom_WS(_ClsProgSetting.GetKDWOSGuid, wosInfo.SelectedDocGuid)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

		Return strResult
	End Function

End Class
