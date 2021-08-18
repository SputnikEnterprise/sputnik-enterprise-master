
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
Imports SP.KD.KSTSearch.ClsDataDetail

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

	Sub ListMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal year As Integer)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities
		Dim strFieldName As String = "LP"
		Dim strSqlQuery As String = String.Empty
		Dim strEntry As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "Select LP From LO Where MDNr = @MDNr And Jahr = @Jahr Group By LP Order By LP"
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@Jahr", Year)
			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				strEntry = CInt(m_utility.SafeGetInteger(reader, strFieldName, 0))

				cbo.Properties.Items.Add(strEntry)

			End While
			cbo.Properties.DropDownRows = 13

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListFARListeGAVBeruf(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal Jahr As Integer, ByVal MonatVon As Integer, ByVal MonatBis As Integer?)
		Dim strEntry As String
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities
		Dim strSqlQuery As String = "Select LOL.GAV_Beruf From LOL "
		strSqlQuery += "Where LOL.MDNr = @MDNr And (LOL.Jahr = @Jahr And LOL.LP Between @LPVon And @LPBis) And "
		strSqlQuery += "LOL.LANR IN (7395.10, 7895.10) And "
		strSqlQuery += "LOL.M_BTR <> 0 "
		strSqlQuery += "Group By LOL.GAV_Beruf Order By LOL.GAV_Beruf"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@Jahr", jahr)
			param = cmd.Parameters.AddWithValue("@LPVon", MonatVon)
			param = cmd.Parameters.AddWithValue("@LPBis", ReplaceMissing(MonatBis, MonatVon))

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				strEntry = m_utility.SafeGetString(reader, "GAV_Beruf", "")

				cbo.Properties.Items.Add(strEntry)
			End While


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

#End Region


#Region "Allgemeine Funktionen"

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
