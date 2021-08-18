
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
Imports SPGAVPVLSearch.ClsDataDetail

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

	Sub ListGAVDivKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal GAVBeruf As String, ByVal Jahr As Integer, ByVal MonatVon As Integer, ByVal MonatBis As Integer)
		Dim strEntry As String
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities
		Dim strSqlQuery As String = "[List GAVKanton For PVLGAV With Mandant]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@GAVBeruf", GAVBeruf)
			param = cmd.Parameters.AddWithValue("@MDYear", Jahr)
			param = cmd.Parameters.AddWithValue("@LPVon", MonatVon)
			param = cmd.Parameters.AddWithValue("@LPBis", MonatBis)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				strEntry = m_utility.SafeGetString(reader, "GAVKanton", String.Empty)

				cbo.Properties.Items.Add(strEntry)

			End While

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListGAVDivBeruf(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal GAVKanton As String, ByVal Jahr As Integer, ByVal MonatVon As Integer, ByVal MonatBis As Integer?)
		Dim strEntry As String
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities
		Dim strSqlQuery As String = "[List GAVBerufe For PVLGAV With Mandant]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@GAVKanton", ReplaceMissing(GAVKanton, String.Empty))
			param = cmd.Parameters.AddWithValue("@MDYear", Jahr)
			param = cmd.Parameters.AddWithValue("@LPVon", MonatVon)
			param = cmd.Parameters.AddWithValue("@LPBis", ReplaceMissing(MonatBis, MonatVon))

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				strEntry = m_utility.SafeGetString(reader, "GAVBeruf", String.Empty)

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



	Function GetJahreslohnsumme(ByVal jahr As Integer, ByVal beruf As String) As Decimal
		Dim vorjaherslohnsumme As Decimal = 0

		If beruf Is Nothing OrElse String.IsNullOrWhiteSpace(beruf) Then Return vorjaherslohnsumme

		Dim m_utility As New SPProgUtility.MainUtilities.Utilities
		Dim conn As New SqlConnection(m_InitialData.MDData.MDDbConn)

		Dim Sql As String = "[Get Jahreslohnsumme Sonstige Berufe With Mandant]"

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(Sql, conn)
		cmd.CommandType = Data.CommandType.StoredProcedure

		'@MDNr int = 0,
		'@jahr INT=2014,
		'@gavBeruf nvarchar(40)

		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
		param = cmd.Parameters.AddWithValue("@jahr", Jahr)
		param = cmd.Parameters.AddWithValue("@gavBeruf", beruf)

		Try
			conn.Open()
			Dim reader As SqlDataReader = cmd.ExecuteReader
			reader.Read()
			If reader.HasRows Then vorjaherslohnsumme = CDec(m_utility.SafeGetDecimal(reader, "Lohnsumme", 0))

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return 0

		Finally
			conn.Close()
		End Try

		Return vorjaherslohnsumme

	End Function

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


End Module
