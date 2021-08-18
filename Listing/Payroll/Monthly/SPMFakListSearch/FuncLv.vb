
Imports System.Data.SqlClient
Imports System
Imports System.IO

Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data

Imports SPMFakListSearch.ClsDataDetail


Module FuncLv

	'Private _ClsFunc As New ClsDivFunc
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath



#Region "Dropdown-Funktionen für 1. Seite..."

	Sub ListMAFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal year As Integer)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "Select MF.Bezeichnung From MA_Filiale MF Left Join LOL On MF.MANr = LOL.MANr "
			strSqlQuery &= "Where (MF.Bezeichnung Is Not Null Or MF.Bezeichnung <> '') And LOL.MDNr = @MDNr "
			strSqlQuery &= "And LOL.Jahr = @jahr "
			strSqlQuery &= "AND LOL.Lanr In (3600, 3602, 3650, 3700, 3750, 3800, 3850, 3900, 3901) "
			strSqlQuery &= "Group By MF.Bezeichnung order by MF.Bezeichnung "

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As SqlParameter = New System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@jahr", year)


			Dim rRPrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rRPrec.Read
				cbo.Properties.Items.Add(rRPrec("Bezeichnung").ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListMAKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal year As Integer)
		Dim strFieldName As String = "Kanton"
		Dim strSqlQuery As String = String.Empty

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()
			strSqlQuery &= "Select LOL.S_Kanton As Kanton From LOL "
			strSqlQuery &= "Where LOL.MDNr = @MDNr "
			strSqlQuery &= "AND LOL.Jahr = @jahr "
			strSqlQuery &= "AND LOL.Lanr In (3600, 3602, 3650, 3700, 3750, 3800, 3850, 3900, 3901) "
			strSqlQuery &= "AND LOL.S_Kanton IS NOT Null "
			strSqlQuery &= "Group By LOL.S_Kanton "
			strSqlQuery &= "ORDER By LOL.S_Kanton "

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As SqlParameter = New System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@jahr", year)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(reader(strFieldName).ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListLOMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetLOMonth]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rRPrec As SqlDataReader = cmd.ExecuteReader					 ' Datenbank

			cbo.Properties.Items.Clear()
			While rRPrec.Read
				cbo.Properties.Items.Add(CType(rRPrec("LP"), Integer))
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListLOYear(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetLOYear]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rRPrec As SqlDataReader = cmd.ExecuteReader					 ' Kundendatenbank

			cbo.Properties.Items.Clear()
			While rRPrec.Read
				cbo.Properties.Items.Add(CType(rRPrec("Jahr"), Integer))
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListMANationality(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal Year As Integer)
		Dim strFieldName As String = "Nationality"
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim strSqlQuery As String = String.Empty
			strSqlQuery &= "Select ma.nationality "
			strSqlQuery &= "from LoL  Join mitarbeiter ma on LOL.MANr = ma.manr "
			strSqlQuery &= "Where LOL.MDNr = @MDNr "
			strSqlQuery &= "AND LOL.Jahr = @jahr "
			strSqlQuery &= "AND LOL.Lanr In (3600, 3602, 3650, 3700, 3750, 3800, 3850, 3900, 3901) "

			strSqlQuery &= "group by ma.nationality "
			strSqlQuery &= "ORDER by ma.nationality "

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As SqlParameter = New System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@jahr", Year)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(reader(strFieldName).ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

#End Region


#Region "Sonstige Funktions..."

	Sub DeleteRecFromDb(ByVal year As Integer)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim sSql As String = "Delete [KiAuZulage_Year] Where USNr In (@LogedUSNr, 0) And Jahr = @Year"
		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As SqlParameter = New System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@LogedUSNr", m_InitialData.UserData.UserNr)
			param = cmd.Parameters.AddWithValue("@Year", Year)

			cmd.ExecuteNonQuery()


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function GetKantonPLZ(ByVal strKanton As String) As String
		Dim strPLZResult As String = ","
		Dim strFieldName As String = "PLZ"

		Dim strSqlQuery As String = "Select PLZ.PLZ, PLZ.Kanton From PLZ "
		strSqlQuery += "Where PLZ.Kanton In ('" & strKanton & "') "
		strSqlQuery += "Group By PLZ.PLZ, PLZ.Kanton Order By PLZ.PLZ, PLZ.Kanton"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rPLZrec As SqlDataReader = cmd.ExecuteReader					' PLZ-Datenbank

			While rPLZrec.Read
				strPLZResult += rPLZrec(strFieldName).ToString & ","

			End While

			If strPLZResult.Length > 1 Then
				strPLZResult = Mid(strPLZResult, 2, Len(strPLZResult) - 2)
				strPLZResult = Replace(strPLZResult, ",", "','")
			Else
				strPLZResult = String.Empty
			End If

		Catch e As Exception
			strPLZResult = String.Empty
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strPLZResult
	End Function

	Function GetFilialKstData(ByVal strFiliale As String) As String
		Dim strKSTResult As String = ","
		Dim strFieldName As String = "KST"

		Dim strSqlQuery As String = "Select Benutzer.KST From Benutzer Left Join US_Filiale on Benutzer.USNr = US_Filiale.USNr "
		strSqlQuery += "Where US_Filiale.Bezeichnung = '" & strFiliale & "' And "
		strSqlQuery += "US_Filiale.Bezeichnung <> '' "
		strSqlQuery += "And US_Filiale.Bezeichnung Is Not Null Group By Benutzer.KST Order By Benutzer.KST"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rPLZrec As SqlDataReader = cmd.ExecuteReader					' PLZ-Datenbank

			While rPLZrec.Read
				strKSTResult += rPLZrec(strFieldName).ToString & ","

			End While
			Console.WriteLine("strKSTResult: " & strKSTResult)
			If strKSTResult.Length > 1 Then
				strKSTResult = Mid(strKSTResult, 2, Len(strKSTResult) - 2)
				strKSTResult = Replace(strKSTResult, ",", "','")
			Else
				strKSTResult = String.Empty
			End If

		Catch e As Exception
			strKSTResult = String.Empty
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strKSTResult
	End Function

	'Sub ListForActivate(ByVal cbo As ComboBox)
	'	cbo.Properties.Items.Clear()
	'	Try
	'		cbo.Properties.Items.Add("")

	'		cbo.Properties.Items.Add("Aktiviert")
	'		cbo.Properties.Items.Add("Nicht Aktiviert")

	'	Catch e As Exception
	'		MsgBox(e.Message)

	'	Finally

	'	End Try

	'End Sub

	'Sub ListForActivate_1(ByVal cbo As ComboBox)
	'	cbo.Properties.Items.Clear()
	'	Try
	'		cbo.Properties.Items.Add("")

	'		cbo.Properties.Items.Add("Aktiviert")
	'		cbo.Properties.Items.Add("Nicht Aktiviert")

	'		cbo.Properties.Items.Add("Leere Felder")

	'	Catch e As Exception
	'		MsgBox(e.Message)

	'	Finally

	'	End Try

	'End Sub

	'Function TranslateMyText(ByVal strBez As String) As String
	'	Dim strOrgText As String = strBez
	'	Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez)
	'	Dim _clsLog As New SPProgUtility.ClsEventLog

	'	Return strTranslatedText
	'End Function

	'Function TranslateMyText(ByVal strFuncName As String, _
	'												 ByVal strOrgControlBez As String, _
	'												 ByVal strBez As String) As String
	'	Dim strOrgText As String = strBez
	'	Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez)
	'	Dim _clsLog As New SPProgUtility.ClsEventLog

	'	Return strTranslatedText
	'End Function

#End Region




End Module
