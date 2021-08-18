
Imports SP.Infrastructure.Logging
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
Imports SPYAHVListSearch.ClsDataDetail
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.UI


Module FuncLv

	Private m_Logger As ILogger = New Logger()


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
				strEntry = m_utility.SafeGetInteger(reader, "Jahr", 0)

				cbo.Properties.Items.Add(strEntry)

			End While

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListEmployeeFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal jahr As Integer)
		Dim strFieldName As String = "Bezeichnung"
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities

		Dim sql As String
		sql = "[List All UserBranches For Listing In Payroll Lists]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(Sql, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			cmd.Parameters.AddWithValue("@kst", String.Empty)
			cmd.Parameters.AddWithValue("@monatVon", 1)
			cmd.Parameters.AddWithValue("@monatBis", 12)
			cmd.Parameters.AddWithValue("@jahrVon", jahr)
			cmd.Parameters.AddWithValue("@jahrBis", jahr)

			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader									 '

			cbo.Properties.Items.Clear()
			While rFoundedrec.Read
				cbo.Properties.Items.Add(m_utility.SafeGetString(rFoundedrec, "Filiale"))
			End While
			cbo.Properties.DropDownRows = 20



			'Dim strSqlQuery As String = "[GetMAFiliale With Mandant]"
			'Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

			'Try
			'	Conn.Open()

			'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			'	cmd.CommandType = Data.CommandType.StoredProcedure
			'	Dim param As System.Data.SqlClient.SqlParameter

			'	param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)

			'	Dim reader As SqlDataReader = cmd.ExecuteReader

			'	cbo.Properties.Items.Clear()
			'	While reader.Read
			'		cbo.Properties.Items.Add(m_utility.SafeGetString(reader, strFieldName, ""))
			'	End While


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMAKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal year As Integer)
		Dim strFieldName As String = "Kanton"
		Dim sql As String = "[Get MAKanton From LO]"
		Dim m_utility = New Utilities

		Try
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(year, Now.Year)))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_utility.SafeGetString(reader, strFieldName, ""))
			End While

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally

		End Try

	End Sub

	Sub ListMANationality(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal year As Integer)
		Dim strFieldName As String = "Nationality"
		Dim sql As String = "[Get MANationality From LO]"
		Dim m_utility = New Utilities

		Try
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(year, Now.Year)))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_utility.SafeGetString(reader, strFieldName, ""))
			End While

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally

		End Try

	End Sub


	Public Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
		Try
			Dim bResult As Boolean = False

			' alle geöffneten Forms durchlaufen
			For Each oForm As Form In Application.OpenForms
				If oForm.Name.ToLower = sName.ToLower Then
					If bDisposeForm Then oForm.Dispose() : Exit For
					bResult = True : Exit For
				End If
			Next

			Return (bResult)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return False

		End Try

	End Function

	Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function


#End Region


#Region "Sonstige Funktions..."

	Function DeleteRecFromDb(ByVal year As Integer) As Boolean
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
		Dim sSql As String = "Delete [AHV_Year] Where USNr In (@LogedUSNr, 0) And Jahr = @Year"
		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As SqlParameter = New System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@LogedUSNr", m_InitialData.UserData.UserNr)
			param = cmd.Parameters.AddWithValue("@Year", year)

			cmd.ExecuteNonQuery()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return False

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return True

	End Function

	Function GetFilialKstData(ByVal strFiliale As String) As String
		Dim strKSTResult As String = ","
		Dim strFieldName As String = "KST"
		Dim m_UtilityUI As New UtilityUI


		Dim strSqlQuery As String
		strSqlQuery = "Select Benutzer.KST From dbo.Benutzer Left Join dbo.US_Filiale on Benutzer.USNr = US_Filiale.USNr "
		strSqlQuery += "Where US_Filiale.Bezeichnung = '" & strFiliale & "' And "
		strSqlQuery += "US_Filiale.Bezeichnung <> '' "
		strSqlQuery += "And US_Filiale.Bezeichnung Is Not Null Group By Benutzer.KST Order By Benutzer.KST"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rPLZrec As SqlDataReader = cmd.ExecuteReader

			While rPLZrec.Read
				strKSTResult += rPLZrec(strFieldName).ToString & ","

			End While

			If strKSTResult.Length > 1 Then
				strKSTResult = Mid(strKSTResult, 2, Len(strKSTResult) - 2)
				strKSTResult = Replace(strKSTResult, ",", "','")
			Else
				strKSTResult = String.Empty
			End If
			If strKSTResult = String.Empty Then
				m_UtilityUI.ShowErrorDialog("Keine Benutzer-KST wurden gefunden.")
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			strKSTResult = String.Empty
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strKSTResult
	End Function


#End Region


End Module
