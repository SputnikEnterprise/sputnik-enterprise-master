
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports SPLOStdListSearch.ClsDataDetail

Imports SP.Infrastructure.Logging


Module FuncLv

	Private m_Logger As ILogger = New Logger()


	'private _ClsReg As New SPProgUtility.ClsDivReg
	'Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

#Region "Dropdown-Funktionen für 1. Seite..."

	Sub ListLOJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As Integer?
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities
		Dim strSqlQuery As String = "Select LO.Jahr From LO Where LO.Jahr > 2006 "
		strSqlQuery += "And MDNr = @MDNr And Jahr Is Not Null Group By LO.Jahr Order By LO.Jahr DESC"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				strEntry = m_utility.SafeGetInteger(reader, "Jahr", Nothing)

				cbo.Properties.Items.Add(strEntry)

			End While

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListEmployeeFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Bezeichnung"

		Dim strSqlQuery As String = "[GetMAFiliale]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("Leere Felder")
			While rMArec.Read
				cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Function ListKanton(ByVal JahrVon As Integer, ByVal JahrBis As Integer?, ByVal MonatVon As Integer, ByVal MonatBis As Integer?,
											ByVal Beruf As String, ByVal MANr As String) As IEnumerable(Of ComboboxValue)

		Dim result As List(Of ComboboxValue) = Nothing
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities

		If Not MonatBis.HasValue Then MonatBis = MonatVon
		If Not JahrBis.HasValue Then JahrBis = JahrVon

		Dim sql As String = "SELECT LOL.GAV_Kanton "
		sql += "FROM LOL "
		sql += "WHERE "
		If Not MANr Is Nothing Then
			sql += String.Format("LOL.MANR IN ({0}) And ", MANr)
		End If
		sql += "LOL.MDNr = @MDNr And LOL.LANR IN (6989, 6990) And "
		sql += "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or "
		sql += "(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
		sql += "(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis)))) And "
		sql += "(@beruf = '' Or LOL.GAV_Beruf = @beruf) "
		sql += "GROUP BY LOL.GAV_Kanton ORDER BY LOL.GAV_Kanton ASC "

		Try
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitialData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@beruf", ReplaceMissing(Beruf, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("@jahrVon", ReplaceMissing(JahrVon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrBis", ReplaceMissing(JahrBis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@monatVon", ReplaceMissing(MonatVon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@monatBis", ReplaceMissing(MonatBis, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

			result = New List(Of ComboboxValue)
			While reader.Read
				Dim itm As New ComboboxValue

				itm.ComboValue = m_utility.SafeGetString(reader, "GAV_Kanton")

				result.Add(itm)

			End While


		Catch ex As Exception
			result = Nothing
			m_Logger.LogError(ex.ToString)

		Finally

		End Try

		Return result

	End Function

	Function ListBeruf(ByVal JahrVon As Integer, ByVal JahrBis As Integer?, ByVal MonatVon As Integer, ByVal MonatBis As Integer?,
										 ByVal Kanton As String, ByVal MANr As String) As IEnumerable(Of ComboboxValue)
		Dim result As List(Of ComboboxValue) = Nothing
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities

		If Not MonatBis.HasValue Then MonatBis = MonatVon
		If Not JahrBis.HasValue Then JahrBis = JahrVon

		Dim sql As String = "SELECT LOL.GAV_Beruf "
		sql += "FROM LOL "
		sql += "WHERE "
		If Not MANr Is Nothing Then
			sql += String.Format("LOL.MANR IN ({0}) And ", MANr)
		End If
		sql += "LOL.MDNr = @MDNr And LOL.LANR IN (6989, 6990) And "
		sql += "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or "
		sql += "(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
		sql += "(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis)))) And "
		sql += "(@kanton = '' Or LOL.GAV_Kanton = @kanton) "
		sql += "GROUP BY LOL.GAV_Beruf ORDER BY LOL.GAV_Beruf ASC "

		Try

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitialData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@kanton", ReplaceMissing(Kanton, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("@jahrVon", ReplaceMissing(JahrVon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrBis", ReplaceMissing(JahrBis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@monatVon", ReplaceMissing(MonatVon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@monatBis", ReplaceMissing(MonatBis, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

			result = New List(Of ComboboxValue)
			While reader.Read
				Dim itm As New ComboboxValue

				itm.ComboValue = m_utility.SafeGetString(reader, "GAV_Beruf")

				result.Add(itm)

			End While


		Catch ex As Exception
			result = Nothing
			m_Logger.LogError(ex.ToString)

		Finally

		End Try

		Return result

	End Function

	'Sub List1Kategorie(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal JahrVon As String, ByVal JahrBis As String, _
	'									 ByVal MonatVon As String, ByVal MonatBis As String, ByVal Kanton As String, _
	'									 ByVal Beruf As String, ByVal MANr As String)
	'	Dim strValue As String
	'	If Not IsNumeric(JahrBis) Then
	'		JahrBis = JahrVon
	'	End If
	'	If Not IsNumeric(MonatBis) Then
	'		MonatBis = MonatVon
	'	End If

	'	Dim strSqlQuery As String = "SELECT LOL.GAV_Gruppe1 "
	'	strSqlQuery += "FROM LOL "
	'	strSqlQuery += "WHERE "
	'	If MANr.Length > 0 Then
	'		strSqlQuery += String.Format("LOL.MANR IN ({0}) And ", MANr)
	'	End If
	'	strSqlQuery += "LOL.LANR IN (6989, 6990) And "
	'	strSqlQuery += "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or "
	'	strSqlQuery += "(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
	'	strSqlQuery += "(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis)))) And "
	'	If Kanton = "Leere Felder" Then
	'		strSqlQuery += "LOL.GAV_Kanton = '' And "
	'	Else
	'		strSqlQuery += "(@kanton = '' Or LOL.GAV_Kanton = @kanton) And "
	'	End If
	'	If Beruf = "Leere Felder" Then
	'		strSqlQuery += "LOL.GAV_Beruf = '' "
	'	Else
	'		strSqlQuery += "(@beruf = '' Or LOL.GAV_Beruf = @beruf) "
	'	End If
	'	strSqlQuery += "GROUP BY LOL.GAV_Gruppe1 ORDER BY LOL.GAV_Gruppe1 ASC "

	'	'Dim i As Integer = 0
	'	'Dim iWidth As Integer = 0
	'	'Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'	Try
	'		Conn.Open()

	'		Dim listOfParams As New List(Of SqlClient.SqlParameter)
	'		listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitialData.MDData.MDNr))
	'		listOfParams.Add(New SqlClient.SqlParameter("@kanton", Kanton))
	'		listOfParams.Add(New SqlClient.SqlParameter("@beruf", Gruppe1))

	'		listOfParams.Add(New SqlClient.SqlParameter("@jahrVon", JahrVon))
	'		listOfParams.Add(New SqlClient.SqlParameter("@jahrBis", JahrBis))
	'		listOfParams.Add(New SqlClient.SqlParameter("@monatVon", MonatVon))
	'		listOfParams.Add(New SqlClient.SqlParameter("@monatBis", MonatBis))

	'		Dim reader As SqlClient.SqlDataReader = m_Utility.OpenReader(m_InitialData.MDData.MDDbConn, Sql, listOfParams, CommandType.Text)



	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'		cmd.CommandType = Data.CommandType.Text
	'		Dim pKanton As SqlParameter = New SqlParameter("@kanton", SqlDbType.NVarChar, 100)
	'		Dim pBeruf As SqlParameter = New SqlParameter("@beruf", SqlDbType.NVarChar, 100)
	'		Dim pJahrVon As SqlParameter = New SqlParameter("@jahrVon", SqlDbType.Int, 4)
	'		Dim pJahrBis As SqlParameter = New SqlParameter("@jahrBis", SqlDbType.Int, 4)
	'		Dim pMonatVon As SqlParameter = New SqlParameter("@monatVon", SqlDbType.Int, 2)
	'		Dim pMonatBis As SqlParameter = New SqlParameter("@monatBis", SqlDbType.Int, 2)
	'		pKanton.Value = Kanton
	'		pBeruf.Value = Beruf
	'		pJahrVon.Value = JahrVon
	'		pJahrBis.Value = JahrBis
	'		pMonatVon.Value = MonatVon
	'		pMonatBis.Value = MonatBis
	'		cmd.Parameters.Add(pKanton)
	'		cmd.Parameters.Add(pBeruf)
	'		cmd.Parameters.Add(pJahrVon)
	'		cmd.Parameters.Add(pJahrBis)
	'		cmd.Parameters.Add(pMonatVon)
	'		cmd.Parameters.Add(pMonatBis)

	'		Dim rESrec As SqlDataReader = cmd.ExecuteReader									 '

	'		cbo.Items.Clear()
	'		cbo.Items.Add("")
	'		While rESrec.Read
	'			If (IsDBNull(rESrec("GAV_Gruppe1")) OrElse rESrec("GAV_Gruppe1").ToString = "") Then
	'				strValue = "Leere Felder"
	'			Else
	'				strValue = rESrec("GAV_Gruppe1").ToString
	'			End If
	'			If (strValue = "Leere Felder" Xor cbo.ContainsValue("Leere Felder")) Or strValue <> "Leere Felder" Then
	'				cbo.Items.Add(New ComboBoxItem(strValue, strValue))

	'			End If
	'		End While

	'	Catch e As Exception
	'		MsgBox(e.Message)

	'	Finally
	'		Conn.Close()
	'		Conn.Dispose()

	'	End Try
	'End Sub



#End Region

#Region "Helpers"

	Sub ListCboMonate1Bis12(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		For i As Integer = 1 To 12
			cbo.Properties.Items.Add(i.ToString)
		Next
	End Sub

	Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function



#End Region

End Module
