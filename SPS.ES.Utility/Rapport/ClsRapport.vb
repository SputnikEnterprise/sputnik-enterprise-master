
Imports System.Data.SqlClient

Imports System.Windows.Forms
Imports SP.Infrastructure.Logging


Namespace SPRPSUtility

	Public Class ClsRPFunktionality

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Public Shared Function DeleteSelectedRP(ByVal liRPNr As List(Of Integer), _
																						ByVal bShowMsg As Boolean) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "Success..."
			Dim sSql As String = String.Empty
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
			Dim sMonth As Short = Now.Month
			Dim iYear As Integer = Now.Year
			Dim iRPNr As Integer = 0
			Dim iESNr As Integer = 0
			Dim iMANr As Integer = 0
			Dim iKDNr As Integer = 0
			Dim strLOGuid As String = String.Empty
			Dim strMAGuid As String = String.Empty
			Dim strRPNr As String = String.Empty

			For i As Integer = 0 To liRPNr.Count - 1
				strRPNr &= If(strRPNr.Length > 0, ",", "") & CStr(liRPNr(i))
			Next
			sSql = "Select RP.RPNr, RP.MANr, RP.KDNr, RP.ESNr, RP.Monat, RP.Jahr From RP "
			sSql &= "Where RP.RPNr In ({0}) Order By RPNr"
			sSql = String.Format(sSql, strRPNr)

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				cmd.CommandType = Data.CommandType.Text

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				Dim _clsRPDeleteFunc As New ClsDeleteRPData
				While rFoundedrec.Read
					iRPNr = CInt(rFoundedrec("RPNr"))
					iMANr = CInt(rFoundedrec("MANr"))
					iKDNr = CInt(rFoundedrec("KDNr"))
					iESNr = CInt(rFoundedrec("ESNr"))

					Dim _setting As New ClsRPDataSetting With {.SelectedRPNr = iRPNr, _
																										 .SelectedMANr = iMANr, _
																										 .SelectedKDNr = iKDNr, _
																										 .SelectedESNr = iESNr, _
																										 .ShowMsgBox = bShowMsg}

					strResult = _clsRPDeleteFunc.DeleteSelectedRPFromDb(_setting)
					If strResult.ToLower.Contains("error") Then Exit While
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				If bShowMsg Then
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("{0}", ex.Message), _
																										 _ClsProgSetting.TranslateText("Einsatz löschen"))
				End If
				Return String.Format("{0}", ex.Message)

			Finally
				Conn.Close()
				Conn.Dispose()

			End Try

			Return strResult
		End Function

		Public Shared Function DeleteSelectedRPWithESNr(ByVal liESNr As List(Of Integer), _
																						ByVal bShowMsg As Boolean) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "Success..."
			Dim sSql As String = String.Empty
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
			Dim sMonth As Short = Now.Month
			Dim iYear As Integer = Now.Year
			Dim iRPNr As Integer = 0
			Dim iESNr As Integer = 0
			Dim iMANr As Integer = 0
			Dim iKDNr As Integer = 0
			Dim strLOGuid As String = String.Empty
			Dim strMAGuid As String = String.Empty
			Dim strESNr As String = String.Empty

			For i As Integer = 0 To liESNr.Count - 1
				strESNr &= If(strESNr.Length > 0, ",", "") & CStr(liESNr(i))
			Next
			sSql = "Select RP.RPNr, RP.MANr, RP.KDNr, RP.ESNr, RP.Monat, RP.Jahr From RP "
			sSql &= "Where RP.ESNr In ({0}) Order By RPNr"
			sSql = String.Format(sSql, strESNr)

			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				cmd.CommandType = Data.CommandType.Text

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
				Dim _clsRPDeleteFunc As New ClsDeleteRPData
				While rFoundedrec.Read
					iRPNr = CInt(rFoundedrec("RPNr"))
					iMANr = CInt(rFoundedrec("MANr"))
					iKDNr = CInt(rFoundedrec("KDNr"))
					iESNr = CInt(rFoundedrec("ESNr"))

					Dim _setting As New ClsRPDataSetting With {.SelectedRPNr = iRPNr, _
																										 .SelectedMANr = iMANr, _
																										 .SelectedKDNr = iKDNr, _
																										 .SelectedESNr = iESNr, _
																										 .ShowMsgBox = bShowMsg}

					strResult = _clsRPDeleteFunc.DeleteSelectedRPFromDb(_setting)
					If strResult.ToLower.Contains("error") Then Exit While
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				If bShowMsg Then
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("{0}", ex.Message), _
																										 _ClsProgSetting.TranslateText("Einsatz löschen"))
				End If
				Return String.Format("{0}", ex.Message)

			Finally
				Conn.Close()
				Conn.Dispose()

			End Try

			Return strResult
		End Function

	End Class

End Namespace

