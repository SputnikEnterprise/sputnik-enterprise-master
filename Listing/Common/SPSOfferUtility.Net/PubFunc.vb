
Imports System.Data.SqlClient
Imports SP.Infrastructure.Logging

Module PubFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	'Friend _ClsLLFunc As New ClsLLFunc

	Dim _ClsReg As New SPProgUtility.ClsDivReg
	Dim _ClsLog As New SPProgUtility.ClsEventLog
	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath ' ClsMain_Net

	Sub main()


	End Sub

	Function GetOfferDbData() As DataTable
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
		Dim strQuery As String = "[List OfferData For Search OfferSendig]"
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "OfferData")

		Return ds.Tables(0)
	End Function


	Function GetFaxDomain() As String
		Dim strMyValue As String = String.Empty

		Try
			strMyValue = _ClsProgSetting.GetMDProfilValue("Mailing", "faxusername", "").ToString      ' Daten für eCall


		Catch ex As Exception
			MsgBox(ex.Message.ToString & vbCrLf & ex.GetBaseException.ToString, MsgBoxStyle.Critical, "GetOffMailingValue")

		End Try

		Return strMyValue
	End Function

	Function GetRecipientFaxField(ByVal strFieldNr As String) As String
		Dim strFaxField As String = "KDTelefax"

		Try

			If Val(_ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile(), "Export", "FaxRecipient_" & strFieldNr, "0").ToString) = 1 Then
				strFaxField = "ZHDTelefax"
			Else
				strFaxField = "KDTelefax"
			End If

			Dim strSavedValue As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms",
																													 "frmOFFMailing / KDOffFields_1")
			If strSavedValue.ToUpper.Contains("Kunden".ToUpper) Or strSavedValue.ToUpper.Contains("Telefax".ToUpper) Then
				strFaxField = strSavedValue
			End If

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetRecipientFaxField_0")

		End Try

		Return strFaxField
	End Function

	Function GetRecipientField(ByVal str4What As String) As String
		Dim strFaxField As String = String.Empty
		Try

			Dim strSavedValue As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms",
																													 "frmOFFMailing / KDOffFields_1")
			If strSavedValue.ToUpper.Contains("Kunden".ToUpper) Then
				strFaxField = strSavedValue
			Else
				If str4What.ToUpper.Contains("Mail".ToUpper) Then
					If strSavedValue.ToUpper.Contains("Mail".ToUpper) Then strFaxField = strSavedValue
				ElseIf str4What.ToUpper.Contains("Fax".ToUpper) Then
					If strSavedValue.ToUpper.Contains("Telefax".ToUpper) Then strFaxField = strSavedValue
				Else

				End If

			End If


		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetRecipientField_0")

		End Try

		Return strFaxField
	End Function

End Module
