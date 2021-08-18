
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
Imports SPFARListeSearch.ClsDataDetail

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

	Public Function LoadLAData(ByVal Jahr As Integer, ByVal MonatVon As Integer, ByVal MonatBis As Integer?, ByVal resor As Boolean) As IEnumerable(Of LAData)
		Dim result As List(Of LAData) = Nothing
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities

		Dim sql As String

		If resor Then
			sql = "Select DISTINCT ESL.GAVNr, LOL.GAV_Beruf From LOL "
			sql &= "LEFT JOIN ESLohn ESL ON ESL.MANr = LOL.MANr AND ESL.GAVKanton = lol.GAV_Kanton AND ESL.GAVGruppe0 = lol.GAV_Beruf "
			sql &= "Where LOL.MDNr = @MDNr And (LOL.Jahr = @Jahr And LOL.LP Between @LPVon And @LPBis) And "
			sql &= "LOL.LANR IN (7395.10, 7895.10) And "
			sql &= "LOL.M_BTR <> 0 AND ("
			sql &= "(ESL.GAVNr = 230001 AND ESL.GAVKanton IN ('GE') ) OR "
			sql &= "(ESL.GAVNr = 305001 AND ESL.GAVKanton IN ('TI') ) OR "
			sql &= "(ESL.GAVNr = 350001 AND ESL.GAVKanton IN ('BS') ) OR "
			sql &= "(ESL.GAVNr = 355003 AND ESL.GAVKanton IN ('BL') ) OR "
			sql &= "(ESL.GAVNr = 355004 AND ESL.GAVKanton IN ('BL') ) OR "
			sql &= "(ESL.GAVNr = 355005 AND ESL.GAVKanton IN ('BS') ) OR "
			sql &= "(ESL.GAVNr = 355006 AND ESL.GAVKanton IN ('TI') ) OR "
			sql &= "(ESL.GAVNr = 365001 AND ESL.GAVKanton In ('FR', 'VD', 'GE', 'BL', 'TI') ) OR "
			sql &= "(ESL.GAVNr = 365003 AND ESL.GAVKanton IN ('BS') ) OR "
			sql &= "(ESL.GAVNr = 365005 AND ESL.GAVKanton IN ('TI') ) OR "
			sql &= "(ESL.GAVNr = 365006) OR "
			sql &= "(ESL.GAVNr = 385001 AND ESL.GAVKanton IN ('JU', 'NE', 'VS', 'GE') ) OR "
			sql &= "(ESL.GAVNr = 445001 AND ESL.GAVKanton IN ('FR', 'JU', 'VS', 'GE', 'BL') ) OR "
			sql &= "(ESL.GAVNr = 455001) OR "
			sql &= "(ESL.GAVNr = 455002 AND ESL.GAVKanton IN ('FR', 'JU', 'NE', 'VS', 'VD', 'GE', 'BL', 'BS', 'TI') ) "
			sql &= ") "
			sql &= "ORDER By LOL.GAV_Beruf"

		Else
			sql = "Select LOL.GAV_Beruf From LOL "
			sql &= "Where LOL.MDNr = @MDNr And (LOL.Jahr = @Jahr And LOL.LP Between @LPVon And @LPBis) And "
			sql &= "LOL.LANR IN (7395.10, 7895.10) And "
			sql &= "LOL.M_BTR <> 0 "
			sql &= "Group By LOL.GAV_Beruf Order By LOL.GAV_Beruf"

		End If


		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ReplaceMissing(m_InitialData.MDData.MDNr, 0)))
		listOfParams.Add(New SqlClient.SqlParameter("@Jahr", ReplaceMissing(Jahr, Now.Year)))
		listOfParams.Add(New SqlClient.SqlParameter("@LPVon", ReplaceMissing(MonatVon, Now.Month)))
		listOfParams.Add(New SqlClient.SqlParameter("@LPBis", ReplaceMissing(MonatBis, Now.Month)))

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of LAData)

				While reader.Read

					Dim CustomerContactData = New LAData()
					CustomerContactData.gavberuf = m_utility.SafeGetString(reader, "GAV_Beruf")


					result.Add(CustomerContactData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally

		End Try

		Return result

	End Function

	Sub ListFARListeGAVBeruf(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal Jahr As Integer, ByVal MonatVon As Integer, ByVal MonatBis As Integer?, ByVal resor As Boolean)
		'Dim strEntry As String
		'Dim m_utility As New SPProgUtility.MainUtilities.Utilities
		'Dim sql As String

		'If resor Then
		'	sql = "Select DISTINCT ESL.GAVNr, LOL.GAV_Beruf From LOL "
		'	sql &= "LEFT JOIN ESLohn ESL ON ESL.MANr = LOL.MANr AND ESL.GAVKanton = lol.GAV_Kanton AND ESL.GAVGruppe0 = lol.GAV_Beruf "
		'	sql &= "Where LOL.MDNr = @MDNr And (LOL.Jahr = @Jahr And LOL.LP Between @LPVon And @LPBis) And "
		'	sql &= "LOL.LANR IN (7395.10, 7895.10) And "
		'	sql &= "LOL.M_BTR <> 0 AND ("
		'	sql &= "(ESL.GAVNr = 230001 AND ESL.GAVKanton IN ('GE') ) OR "
		'	sql &= "(ESL.GAVNr = 305001 AND ESL.GAVKanton IN ('TI') ) OR "
		'	sql &= "(ESL.GAVNr = 350001 AND ESL.GAVKanton IN ('BS') ) OR "
		'	sql &= "(ESL.GAVNr = 355003 AND ESL.GAVKanton IN ('BL') ) OR "
		'	sql &= "(ESL.GAVNr = 355004 AND ESL.GAVKanton IN ('BL') ) OR "
		'	sql &= "(ESL.GAVNr = 355005 AND ESL.GAVKanton IN ('BS') ) OR "
		'	sql &= "(ESL.GAVNr = 365001 AND ESL.GAVKanton In ('FR', 'VD', 'GE', 'BL', 'TI') ) OR "
		'	sql &= "(ESL.GAVNr = 365003 AND ESL.GAVKanton IN ('BS') ) OR "
		'	sql &= "(ESL.GAVNr = 365005 AND ESL.GAVKanton IN ('TI') ) OR "
		'	sql &= "(ESL.GAVNr = 380001 AND ESL.GAVKanton IN ('BL', 'BS') ) OR "
		'	sql &= "(ESL.GAVNr = 385001 AND ESL.GAVKanton IN ('JU', 'NE', 'VS', 'GE') ) OR "
		' Sql &= "(ESL.GAVNr = 445001 AND ESL.GAVKanton IN ('FR', 'JU', 'VS', 'GE', 'BL') ) OR "
		' Sql &= "(ESL.GAVNr = 455002 AND ESL.GAVKanton IN ('FR', 'JU', 'NE', 'VS', 'VD', 'GE', 'BL', 'BS', 'TI') ) "
		'	sql &= ") "
		'	sql &= "ORDER By LOL.GAV_Beruf"

		'Else
		'	sql = "Select LOL.GAV_Beruf From LOL "
		'	sql &= "Where LOL.MDNr = @MDNr And (LOL.Jahr = @Jahr And LOL.LP Between @LPVon And @LPBis) And "
		'	sql &= "LOL.LANR IN (7395.10, 7895.10) And "
		'	sql &= "LOL.M_BTR <> 0 "
		'	sql &= "Group By LOL.GAV_Beruf Order By LOL.GAV_Beruf"

		'End If

		'Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			'Conn.Open()

			'Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			'cmd.CommandType = Data.CommandType.Text
			'Dim param As System.Data.SqlClient.SqlParameter

			'param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			'param = cmd.Parameters.AddWithValue("@Jahr", Jahr)
			'param = cmd.Parameters.AddWithValue("@LPVon", MonatVon)
			'param = cmd.Parameters.AddWithValue("@LPBis", ReplaceMissing(MonatBis, MonatVon))

			'Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			Dim data = LoadLAData(Jahr, MonatVon, MonatBis, resor)
			If Not data Is Nothing Then
				For Each itm In data
					cbo.Properties.Items.Add(itm.gavberuf)
				Next
			End If


			'While reader.Read
			'	strEntry = m_utility.SafeGetString(reader, "GAV_Beruf", "")

			'	cbo.Properties.Items.Add(strEntry)
			'End While


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			'Conn.Close()
			'Conn.Dispose()

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
