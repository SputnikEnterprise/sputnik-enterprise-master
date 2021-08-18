
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
Imports SPLOKigaYSearch.ClsDataDetail

Imports SP.Infrastructure.Logging


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
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities

		Dim strSqlQuery As String = "[GetMAFiliale With Mandant]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_utility.SafeGetString(reader, strFieldName, ""))
			End While


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

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



End Module
