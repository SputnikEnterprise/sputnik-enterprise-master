
Imports System.Data.SqlClient
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.XtraEditors

Imports SPS.ES.Utility.SPRPSUtility.ClsRPFunktionality

Imports System.IO
Imports SP.Infrastructure.Logging
Imports SP.Internal.Automations.WOSUtility.DataObjects

Module Utilities

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()


	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


#Region "Löschen vom Einsatz..."


	Function DeleteSelectedESFromDb(ByVal _setting As ClsESDataSetting) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
		Dim strGeschlecht As String = String.Empty
		Dim strMAAnrede As String = String.Empty
		Dim strNachname As String = String.Empty
		Dim strVorname As String = String.Empty

		Dim _iESNr As Integer = _setting.SelectedESNr
		Dim _iMANr As Integer = _setting.SelectedMANr
		Dim _iKDNr As Integer = _setting.SelectedKDNr

		If _iKDNr = 0 Or _iMANr = 0 Or _iESNr = 0 Then Throw New Exception("Keine Einsätze wurde gefunden.")
		Dim sSql As String = "Select MA.Geschlecht, MA.Nachname, MA.Vorname From Mitarbeiter MA Where MA.MANr = @MANr"
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@MANr", _iMANr)
		cmd.CommandType = Data.CommandType.Text

		Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
		rFoundedrec.Read()
		If rFoundedrec.HasRows Then
			strGeschlecht = rFoundedrec("Geschlecht")
			strNachname = rFoundedrec("Nachname")
			strVorname = rFoundedrec("Vorname")

		Else
			Throw New Exception(String.Format("Error: {0}", _ClsProgSetting.TranslateText("Der Kandidat wurde nicht gefunden.")))

		End If
		strMAAnrede = String.Format(_ClsProgSetting.TranslateText(If(UCase(strGeschlecht = "M"), "Herr", "Frau")) & " {0} {1}", strVorname, strNachname)

		Dim strMsg As String = "Mit diesem Vorgang löschen Sie den ausgewählten Einsatz.{0}Einsatznummer: {1}{0}KandidatIn: {2}{0}{0}"
		strMsg &= "Möchten Sie wirklich mit dem Vorgang fortfahren?"
		strMsg = String.Format(_ClsProgSetting.TranslateText(strMsg), vbNewLine, _iESNr, strMAAnrede)
		If _setting.ShowMsgBox Then
			If XtraMessageBox.Show(strMsg, _ClsProgSetting.TranslateText("Einsatz löschen"),
																										 System.Windows.Forms.MessageBoxButtons.YesNo,
																										 System.Windows.Forms.MessageBoxIcon.Question,
																										 System.Windows.Forms.MessageBoxDefaultButton.Button1) = DialogResult.No Then
				Throw New Exception(String.Format("Error: {0}", _ClsProgSetting.TranslateText("Der Vorgang wurde abgebrochen.")))
			End If
		End If

		Try
			Dim ESUtility As New ClsESStart(ModulConstants.m_InitialData)
			Dim rFrec As SqlClient.SqlDataReader = GetSelectedESData4DeletingRec(_setting)
			rFrec.Read()
			If rFrec.HasRows Then
				Dim strLONr As String = rFrec("LONr")
				Dim strRENr As String = rFrec("RENr")
				Dim strLANr As String = rFrec("LANr")
				Dim strESVertragDocGuid As String = rFrec("ESDoc_Guid")
				Dim strVerleihDocGuid As String = rFrec("VerleihDoc_Guid")
				Dim strRPDocGuid As String = rFrec("RPDocGuid")
				strMsg = String.Empty

				If strLONr <> String.Empty Then
					strMsg = String.Format("Lohnabrechnung: {0}", strLONr)
				End If
				If strRENr <> String.Empty Then
					strMsg &= String.Format("{0}Rechnungen: {1}", If(strMsg = "", "", vbNewLine), strRENr)
				End If
				If strLANr <> String.Empty Then
					strMsg &= String.Format("{0}Monatliche Lohnangaben: {1}", If(strMsg = "", "", vbNewLine), strLANr)
				End If
				If strMsg <> String.Empty Then
					strMsg = String.Format("Mit Ihrem Einsatz sind folgende Daten verknüpft:{0}{1}", vbNewLine, strMsg)
				End If


				If strMsg = String.Empty Then
					Try
						If IsUserActionAllowed(ModulConstants.m_InitialData.UserData.UserNr, 302, ModulConstants.m_InitialData.MDData.MDNr) Then
							strResult = DeleteSelectedRPWithESNr(New List(Of Integer)(New Integer() {_setting.SelectedESNr}), bShowMsg:=False)
						Else
							strMsg = "Bitte stellen Sie sicher, dass Sie ausreichende Benutzerrechte besitzen, um Rapporte zu löschen."
							XtraMessageBox.Show(_ClsProgSetting.TranslateText(strMsg), _ClsProgSetting.TranslateText("Einsatzdaten mit Rapporte löschen"),
										System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information,
										System.Windows.Forms.MessageBoxDefaultButton.Button1)
							Throw New Exception("error: " & _ClsProgSetting.TranslateText(strMsg))
						End If

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.Einsatzrapporte löschen. {1}", strMethodeName, ex.Message))
						Return ex.ToString

					End Try
					Dim guidArray = New String() {strESVertragDocGuid, strVerleihDocGuid, strRPDocGuid}.ToArray()
					Dim strDocGuid As String = String.Join(",", guidArray.Where(Function(s) Not String.IsNullOrWhiteSpace(s)))
					If Not String.IsNullOrWhiteSpace(strDocGuid) AndAlso (_ClsProgSetting.bAllowedMADocTransferTo_WS() OrElse _ClsProgSetting.bAllowedKDDocTransferTo_WS()) Then
						Try
							ESUtility.ESSetting = _setting

							ESUtility.ESTemplateGuid = strESVertragDocGuid
							ESUtility.ESVerleihTemplateGuid = strVerleihDocGuid
							ESUtility.ReportScanGuids = strRPDocGuid.Split(","c).ToList

							ESUtility.DeleteDocFrom_WS(_iMANr, _iKDNr, _iESNr)

						Catch ex As Exception
							m_Logger.LogError(String.Format("{0}.Datensätze im WOS löschen. {1}", strMethodeName, ex.Message))

						End Try
					End If


					Try
						strResult = DeleteSelectedESRec(_setting)
						If _setting.ShowMsgBox Then
							strMsg = String.Format("Ihre Daten wurden erfolgreich gelöscht.")
							XtraMessageBox.Show(_ClsProgSetting.TranslateText(strMsg), _ClsProgSetting.TranslateText("Einsatzdaten löschen"),
																	System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information,
																	System.Windows.Forms.MessageBoxDefaultButton.Button1)
						End If

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.Datensätze löschen. {1}", strMethodeName, ex.Message))

					End Try

				Else
					strMsg &= String.Format("{0}{0}Der Vorgang wird abgebrochen.", vbNewLine)
					XtraMessageBox.Show(_ClsProgSetting.TranslateText(strMsg), _ClsProgSetting.TranslateText("Einsatzdaten löschen"),
															System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information,
															System.Windows.Forms.MessageBoxDefaultButton.Button1)
					Throw New Exception("error: " & _ClsProgSetting.TranslateText(strMsg))

				End If

			Else
				strMsg = "Die allgemeinen Daten wurden nicht gefunden. Der Vorgang wird abgebrochen."
				Throw New Exception("error: " & _ClsProgSetting.TranslateText(strMsg))

			End If

		Catch ex As Exception
			strMsg = String.Format("{0}", ex.Message)
			m_Logger.LogError(String.Format("{0}.Einsatzdetails auflisten. {1}", strMethodeName, ex.Message))
			strResult = strMsg
		End Try

		Return strResult
	End Function

	Function GetSelectedESData4DeletingRec(ByVal _setting As ClsESDataSetting) As SqlClient.SqlDataReader
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim rFrec As SqlClient.SqlDataReader
		Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)

		Try
			Conn.Open()

			Dim sSql As String = "[Get ESData 4 Delete Selected ES]"
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			param = New System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _setting.SelectedESNr)

			cmd.CommandType = Data.CommandType.StoredProcedure
			rFrec = cmd.ExecuteReader


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			rFrec = Nothing

		End Try

		Return rFrec
	End Function

	Function DeleteSelectedESRec(ByVal _setting As ClsESDataSetting) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."
		Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)

		Try
			Conn.Open()

			Dim sSql As String = "[Delete Selected ES Data In All Table]"
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			param = New System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _setting.SelectedESNr)
			param = cmd.Parameters.AddWithValue("@MANr", _setting.SelectedMANr)
			param = cmd.Parameters.AddWithValue("@KDNr", _setting.SelectedKDNr)
			param = cmd.Parameters.AddWithValue("@UserName", String.Format("{0} {1}", _ClsProgSetting.GetUserFName, _ClsProgSetting.GetUserLName))

			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.ExecuteNonQuery()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			strResult = String.Format("Error: {0}", ex.Message)

		End Try

		Return strResult
	End Function

	Function SaveFileIntoDb(ByVal strFile2Save As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strUSName As String = _ClsProgSetting.GetUserName()
		Dim Conn As New SqlConnection(_ClsProgSetting.GetConnString)
		Dim strLogFileName As String = _ClsProgSetting.GetProzessLOGFile()
		Dim sSql As String = String.Empty
		Dim strResult As String = "Success..."

		sSql = "Update DeleteInfo Set DeletedDoc = @BinaryFile Where ID = (Select Top 1 [ID] From DeleteInfo Order By ID DESC)"
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		Dim param As System.Data.SqlClient.SqlParameter

		Try
			Conn.Open()
			cmd.Connection = Conn

			If strFile2Save <> String.Empty Then
				Dim myFile() As Byte = GetFileToByte(strFile2Save)
				Dim fi As New System.IO.FileInfo(strFile2Save)
				Dim strFileExtension As String = fi.Extension

				Try
					cmd.CommandType = CommandType.Text
					cmd.CommandText = sSql
					param = cmd.Parameters.AddWithValue("@BinaryFile", myFile)

					cmd.Connection = Conn
					cmd.ExecuteNonQuery()
					cmd.Parameters.Clear()

				Catch ex As Exception
					strResult = String.Format("Error: {0}", ex.Message)
					m_Logger.LogError(String.Format("{0}.Datei in die Datenbank schreiben. {1}", strMethodeName, ex.Message))

				End Try
			End If

		Catch ex As Exception
			strResult = String.Format("Error: {0}", ex.Message)
			m_Logger.LogError(String.Format("{0}.Datei in die Datenbank schreiben. {1}", strMethodeName, ex.Message))

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Return strResult
	End Function

	Function GetFileToByte(ByVal filePath As String) As Byte()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim stream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
		Dim reader As BinaryReader = New BinaryReader(stream)

		Dim photo() As Byte = Nothing
		Try

			photo = reader.ReadBytes(CInt(stream.Length))
			reader.Close()
			stream.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

		Return photo
	End Function

#End Region



End Module
