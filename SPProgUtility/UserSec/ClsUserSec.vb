
Imports System.IO
Imports System.Data.SqlClient
Imports SPProgUtility.Mandanten
Imports nlog


Namespace SPUserSec

	Public Class ClsUserSec

		Private Shared logger As Logger = LogManager.GetCurrentClassLogger()


		''' <summary>
		''' Ob der angemeldete User für Exportieren berechtigt ist. 
		''' </summary>
		''' <returns>Boolean (True | False)</returns>
		''' <example>IsUserAllowed4DocExport("255")</example>
		''' <remarks></remarks>
		Public Shared Function IsUserAllowed4DocExport(ByVal strJobNr As String) As Boolean
			Dim _ClsProgSetting As New ClsProgSettingPath
			Dim m_Utility As New MainUtilities.Utilities
			Dim bResult As Boolean = False
			Dim _ClsReg As New ClsDivReg
			Dim strUserProfileName As String = _ClsProgSetting.GetUserProfileFile()


			Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString)
			Conn.Open()

			Dim sSql As String = "[Get DocumentrightData For Selected User]"
			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@USNr", _ClsProgSetting.GetLogedUSNr)
			param = cmd.Parameters.AddWithValue("@JobNr", strJobNr)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then
					bResult = m_Utility.SafeGetBoolean(reader, "AllowedToExport", False)

				Else
					Dim strQuery As String = "//User_" & _ClsProgSetting.GetLogedUSNr & "/Documents/DocName[@ID=" & Chr(34) & strJobNr & Chr(34) & "]/Export"
					Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)
					If strBez <> String.Empty Then
						If strBez = CStr(1) Then bResult = True
					End If

				End If
				reader.Close()


			Catch ex As Exception
				logger.Error(String.Format("{0}", ex.ToString))

			End Try


			Return bResult
		End Function

		''' <summary>
		''' Ob der angemeldete User für LOG berechtigt ist. 
		''' </summary>
		''' <returns>Boolean (True | False)</returns>
		''' <example>IsUserAllowed4DocLOG("1.3")</example>
		''' <remarks></remarks>
		Public Shared Function IsUserAllowed4DocLOG(ByVal strJobNr As String) As Boolean
			Dim _ClsProgSetting As New ClsProgSettingPath
			Dim m_Utility As New MainUtilities.Utilities
			Dim bResult As Boolean = False


			Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString)
			Conn.Open()

			Dim sSql As String = "[Get DocumentrightData For Selected User]"
			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@USNr", _ClsProgSetting.GetLogedUSNr)
			param = cmd.Parameters.AddWithValue("@JobNr", strJobNr)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then
					bResult = m_Utility.SafeGetBoolean(reader, "LogActivity", False)
				End If
				reader.Close()


			Catch ex As Exception
				logger.Error(String.Format("{0}", ex.ToString))

			End Try


			Return bResult
		End Function

		Public Shared Function GetUserDataWithName() As Dictionary(Of String, String)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim _ClsProgSetting As New ClsProgSettingPath
			Dim dValue As New Dictionary(Of String, String)
			Dim strUSKst As String = String.Empty
			Dim strUSNachname As String = String.Empty
			Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString)
			Conn.Open()

			Dim sSql As String = "[Get USData 4 Templates With USName]"
			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@USNachname", _ClsProgSetting.GetUserLName)
			param = cmd.Parameters.AddWithValue("@USVorname", _ClsProgSetting.GetUserFName)

			Dim rTemprec As SqlDataReader = cmd.ExecuteReader					 ' Benutzerdatenbank

			Try
				While rTemprec.Read
					For i As Integer = 0 To rTemprec.FieldCount - 1
						dValue.Add((rTemprec.GetName(i)).ToLower, rTemprec(rTemprec.GetName(i)))
					Next
				End While
				rTemprec.Close()


			Catch ex As Exception
				logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

			Finally
				rTemprec.Close()
				Conn.Close()

			End Try

			Return dValue
		End Function

		Public Shared Function GetUSTitle(ByVal iUSNr As Integer) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim _ClsProgSetting As New ClsProgSettingPath
			Dim iLogedUsnr As Integer = _ClsProgSetting.GetLogedUSNr()
			Dim strTitel_1 As String = String.Empty
			Dim strTitel_2 As String = String.Empty
			Dim strResult As String = "|"
			Dim sSql As String = "Select IsNull(USTitel_1, '') As USTitel_1, IsNull(USTitel_2, '') As USTitel_2 From Benutzer Where USNr = @USNr"
			Dim ConnDbSelect As New SqlConnection(_ClsProgSetting.GetConnString)

			Try
				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)
				Dim param As System.Data.SqlClient.SqlParameter
				Dim rUSrec As SqlClient.SqlDataReader

				Try
					ConnDbSelect.Open()
					param = cmd.Parameters.AddWithValue("@USNr", iUSNr)

					rUSrec = cmd.ExecuteReader					' User-Datenbank
					rUSrec.Read()
					If rUSrec.HasRows Then
						strTitel_1 = rUSrec("USTitel_1").ToString
						strTitel_2 = rUSrec("USTitel_2").ToString
					End If
					strResult = String.Format("{0}|{1}", strTitel_1, strTitel_2)
					rUSrec.Close()

				Catch ex As Exception
					logger.Error(String.Format("{0}.USTitel-Datenbank lesen:{1}", strMethodeName, ex.Message))

				Finally

				End Try


			Catch ex As Exception
				logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

			Finally
				ConnDbSelect.Close()

			End Try
			Return strResult

		End Function

		Public Shared Function IsUserActionAllowed(ByVal iUSNr As Integer, ByVal iFuncName As Integer) As Boolean
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim iLogedUsnr As Integer = iUSNr
			Dim bResult As Boolean
			Dim sSql As String = "[Get User SecLevel For Selected Moduls]"
			Dim iMyFuncNr As Integer = iFuncName
			If iMyFuncNr = 0 Then
				logger.Warn(String.Format("{0}.Kein Modul wurde ausgewählt...", strMethodeName))
				Return False
			End If
			Dim _ClsProgSetting As New ClsProgSettingPath
			If iLogedUsnr = 0 Then iLogedUsnr = _ClsProgSetting.GetLogedUSNr()
			Dim ConnDbSelect As New SqlConnection(_ClsProgSetting.GetConnString)

			Try
				'sSql = "[Get User SecLevel For Selected Moduls]" '"Select Autorized From USSecLevel Where ModulName = @strModulname And USNr = @LogedUSNr "

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				Dim rSecrec As SqlClient.SqlDataReader

				Try
					ConnDbSelect.Open()
					param = cmd.Parameters.AddWithValue("@LogedUSNr", iLogedUsnr)
					param = cmd.Parameters.AddWithValue("@FuncSecNr", iMyFuncNr)

					rSecrec = cmd.ExecuteReader					 ' UserSecDatenbank
					rSecrec.Read()
					If rSecrec.HasRows Then
						If Not IsDBNull(rSecrec("IsAllowed")) Then
							bResult = CBool(rSecrec("IsAllowed"))
						Else
							bResult = False
						End If
					End If
					rSecrec.Close()

				Catch ex As Exception
					logger.Error(String.Format("{0}.SecDb lesen:{1}", strMethodeName, ex.Message))

				Finally

				End Try

			Catch ex As Exception
				logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

			Finally
				ConnDbSelect.Close()

			End Try
			Return bResult

		End Function

		Public Shared Function IsUserActionAllowed(ByVal iUSNr As Integer, ByVal iFuncName As Integer, ByVal iMDNr As Integer) As Boolean
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim iLogedUsnr As Integer = iUSNr
			Dim bResult As Boolean
			Dim sSql As String = "[Get User SecLevel For Selected Moduls]"
			Dim iMyFuncNr As Integer = iFuncName
			If iMyFuncNr = 0 Then
				logger.Warn(String.Format("{0}.Kein Modul wurde ausgewählt...", strMethodeName))
				Return False
			End If
			Dim _ClsProgSetting As New ClsProgSettingPath
			If iLogedUsnr = 0 Then iLogedUsnr = _ClsProgSetting.GetLogedUSNr()
			Dim ConnDbSelect As New SqlConnection(_ClsProgSetting.GetConnString)
			If String.IsNullOrWhiteSpace(ConnDbSelect.ConnectionString) Then Return False

			Try
				'sSql = "[Get User SecLevel For Selected Moduls]" '"Select Autorized From USSecLevel Where ModulName = @strModulname And USNr = @LogedUSNr "

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				Dim rSecrec As SqlClient.SqlDataReader

				Try
					ConnDbSelect.Open()
					param = cmd.Parameters.AddWithValue("@LogedUSNr", iLogedUsnr)
					param = cmd.Parameters.AddWithValue("@FuncSecNr", iMyFuncNr)
					param = cmd.Parameters.AddWithValue("@MDNr", iMDNr)

					rSecrec = cmd.ExecuteReader					 ' UserSecDatenbank
					rSecrec.Read()
					If rSecrec.HasRows Then
						If Not IsDBNull(rSecrec("IsAllowed")) Then
							bResult = CBool(rSecrec("IsAllowed"))
						Else
							bResult = False
						End If
					End If
					rSecrec.Close()

				Catch ex As Exception
					logger.Error(String.Format("{0}.SecDb lesen:{1}", strMethodeName, ex.Message))

				Finally

				End Try

			Catch ex As Exception
				logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

			Finally
				ConnDbSelect.Close()

			End Try
			Return bResult

		End Function

		Public Shared Function GetUserSecurityValues(ByVal iMDNr As Integer?, ByVal iUSNr As Integer?) As IEnumerable(Of ClsUserSecData)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Dim m_Utility As New MainUtilities.Utilities
			Dim result As List(Of ClsUserSecData) = Nothing

			Dim Sql As String = "[Get User SecLevel As Object]"

			If Not iUSNr.HasValue Or Not iMDNr.HasValue Then
				logger.Warn(String.Format("{0}.Kein Modul wurde ausgewählt...", strMethodeName))
				Return result
			End If
			Dim _ClsProgSetting As New ClsProgSettingPath

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("LogedUSNr", iUSNr))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", iMDNr))

			Dim reader = m_Utility.OpenReader(_ClsProgSetting.GetConnString, Sql, listOfParams, CommandType.StoredProcedure)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of ClsUserSecData)

					While reader.Read
						Dim overviewData As New ClsUserSecData

						overviewData.SecUSNr = m_Utility.SafeGetInteger(reader, "USNr", 0)
						overviewData.SecNr = m_Utility.SafeGetInteger(reader, "SecNr", 0)
						overviewData.ModulName = m_Utility.SafeGetString(reader, "Modulname")
						overviewData.Autorized = m_Utility.SafeGetBoolean(reader, "Autorized", False)

						result.Add(overviewData)

					End While

				End If

			Catch ex As Exception
				logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

			Finally
				reader.Close()

			End Try

			Return result

		End Function

		Public Shared Function GetUserSecLevelInObject(ByVal mandantNumber As Integer?, ByVal userNumber As Integer?) As Dictionary(Of Integer, UserSecurityData)
			Dim secLookup As New Dictionary(Of Integer, UserSecurityData)

			Dim SecData = GetUserSecurityValues(mandantNumber, userNumber)
			Try

				For Each Data As ClsUserSecData In SecData
					Dim ctrolObject As New UserSecurityData

					ctrolObject.SecNr = Data.SecNr
					ctrolObject.Autorized = Data.Autorized

					secLookup.Add(ctrolObject.SecNr, ctrolObject)
				Next

				Return secLookup

			Catch ex As Exception
				logger.Error(ex.ToString)
				Return Nothing

			Finally

			End Try

			Return secLookup

		End Function





		''' <summary>
		''' Rechte für Moduls werden registriert. 
		''' </summary>
		''' <returns>Boolean (True | False)</returns>
		''' <example>IsUserAllowed4DocExport("sesam")</example>
		''' <remarks></remarks>
		Public Shared Function IsModulLicenceOK(ByVal strFieldname As String) As Boolean
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			If strFieldname = String.Empty Then Return False
			Dim m_md As New Mandant

			Dim bResult As Boolean = m_md.IsModulLicenseOK(0, Now.Year, strFieldname)

			Return bResult

		End Function

		''' <summary>
		''' Rechte für Moduls werden registriert. 
		''' </summary>
		''' <param name="strFieldname"></param>
		''' <param name="iMDNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function IsModulLicenceOK(ByVal strFieldname As String, ByVal iMDNr As Integer) As Boolean
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim _ClsProgSetting As New ClsProgSettingPath
			If strFieldname = String.Empty Then Return False
			Dim m_md As New Mandant

			Dim bResult As Boolean = m_md.IsModulLicenseOK(iMDNr, Now.Year, strFieldname)

			Return bResult

		End Function

		''' <summary>
		''' Ob der gewünschte Monat und Jahr abgeschlossen ist.
		''' </summary>
		''' <param name="_sMonth"></param>
		''' <param name="_iYear"></param>
		''' <returns>Boolean (True | False)</returns>
		''' <remarks></remarks>
		Public Shared Function IsMonthClosed(ByVal _sMonth As Short, ByVal _iYear As Integer) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty
			Dim sSql As String = "[Get Data For ClosedMonth]"
			Dim _ClsProgSetting As New ClsProgSettingPath
			Dim ConnDbSelect As New SqlConnection(_ClsProgSetting.GetConnString)

			Try
				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				Dim rFrec As SqlClient.SqlDataReader

				Try
					ConnDbSelect.Open()
					param = cmd.Parameters.AddWithValue("@sMonth", _sMonth)
					param = cmd.Parameters.AddWithValue("@Year", _iYear)

					rFrec = cmd.ExecuteReader
					rFrec.Read()
					If rFrec.HasRows Then
						strResult = String.Format("{0}|{1}", rFrec("UserName"), rFrec("CreatedOn"))
					End If
					rFrec.Close()

				Catch ex As Exception
					logger.Error(String.Format("{0}.Datenbank lesen:{1}", strMethodeName, ex.Message))

				Finally

				End Try

			Catch ex As Exception
				logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

			Finally
				ConnDbSelect.Close()

			End Try
			Return strResult

		End Function

		''' <summary>
		''' Ob der gewünschte Monat und Jahr abgeschlossen ist.
		''' </summary>
		''' <param name="_sMonth"></param>
		''' <param name="_iYear"></param>
		''' <param name="_iMDNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function IsMonthClosed(ByVal _sMonth As Short, ByVal _iYear As Integer, ByVal _iMDNr As Integer) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty
			Dim sSql As String = "[Get Data For ClosedMonth With Mandant]"
			Dim _ClsProgSetting As New ClsProgSettingPath
			Dim ConnDbSelect As New SqlConnection(_ClsProgSetting.GetConnString)

			Try
				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				Dim rFrec As SqlClient.SqlDataReader

				Try
					ConnDbSelect.Open()
					param = cmd.Parameters.AddWithValue("@sMonth", _sMonth)
					param = cmd.Parameters.AddWithValue("@Year", _iYear)
					param = cmd.Parameters.AddWithValue("@MDNr", _iMDNr)

					rFrec = cmd.ExecuteReader
					rFrec.Read()
					If rFrec.HasRows Then
						strResult = String.Format("{0}|{1}", rFrec("UserName"), rFrec("CreatedOn"))
					End If
					rFrec.Close()

				Catch ex As Exception
					logger.Error(String.Format("{0}.Datenbank lesen:{1}", strMethodeName, ex.Message))

				Finally

				End Try

			Catch ex As Exception
				logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

			Finally
				ConnDbSelect.Close()

			End Try
			Return strResult

		End Function



	End Class

End Namespace
