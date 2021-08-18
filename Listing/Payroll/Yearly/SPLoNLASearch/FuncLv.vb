
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
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions
Imports SPLoNLASearch.ClsDataDetail

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility


Module FuncLv

	Private m_Logger As ILogger = New Logger()

	Private m_UtilityUi As New SP.Infrastructure.UI.UtilityUI

	Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


  Function GetMADbData4NLA(ByVal iYear As Integer) As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
    Dim strQuery As String = "[List MAData For Search In NLA]"
    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.StoredProcedure

    Dim objAdapter As New SqlDataAdapter
    Dim param As System.Data.SqlClient.SqlParameter

    param = cmd.Parameters.AddWithValue("@MDYear", iYear)

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "NLAMAData")

    Return ds.Tables(0)
  End Function

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
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub


#End Region

#Region "Allgemeine Funktionen"

	Sub ListMAKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal selYear As Integer)
		Dim strFieldName As String = "Kanton"

		Dim Sql As String = "Select MA.MA_Kanton as Kanton From Mitarbeiter MA Left Join LO On LO.MANr = MA.MANr "
		Sql += "Where LO.MDNr = @MDNr And LO.Jahr = @Jahr And Not (MA.MA_Kanton = '' Or MA.MA_Kanton is Null) "
		Sql += "Group By MA.MA_Kanton Order By MA.MA_Kanton"
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(Sql, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@Jahr", selYear)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(reader(strFieldName).ToString)
			End While
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListMANationality(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal selYear As Integer)

		Dim Sql As String = "SELECT LO.Land, LND.Land FROM LO LEFT JOIN LND ON LND.Code = LO.Land "
		Sql &= "WHERE LO.MDNr = @MDNr And LO.Jahr = @Jahr And LND.Land <> '' "
		Sql &= "GROUP BY LO.Land, LND.Land "
		Sql &= "ORDER BY LO.Land "
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(Sql, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@Jahr", selYear)
			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(reader("Land"))
			End While
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function GetLAData4LAWField(ByVal iYear As Integer, ByVal iMANr As Integer, ByVal strLAWField As String) As String
		Dim strResult As String = String.Empty
		Dim strSqlQuery As String = "[Get LAData With LAWField 4 LONLA]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@MDYear", iYear)
			cmd.Parameters.AddWithValue("@MANr", iMANr)
			cmd.Parameters.AddWithValue("@LAWField", strLAWField)

			Dim rLOrec As SqlDataReader = cmd.ExecuteReader									 '
			rLOrec.Read()
			If rLOrec.HasRows Then strResult = rLOrec("Bezeichnung").ToString
			If strResult.Length > 0 Then
				strResult = strResult.Remove(0, 1)
			End If

		Catch ex As Exception	' Manager
			MessageBoxShowError("GetLAData4LAWField", ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function


#End Region

#Region "Funktionen zur Übersetzung..."

	Function TranslateMyText(ByVal strBez As String) As String
    Dim strOrgText As String = strBez
    Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez)
    Dim _clsLog As New SPProgUtility.ClsEventLog

    If _ClsProgSetting.GetLogedUSNr = 1 Then
      _clsLog.WriteTempLogFile(String.Format("Progbez: {0}{1}{0} Translatedbez: {0}{2}{0}", _
                                  Chr(34), strBez, strTranslatedText), _
                                _ClsProgSetting.GetSpSTempPath & "DeinFile.txt")
    End If

    Return strTranslatedText
  End Function

  Function TranslateMyText(ByVal strFuncName As String, _
                           ByVal strOrgControlBez As String, _
                           ByVal strBez As String) As String
    Dim strOrgText As String = strBez
    Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez)
    Dim _clsLog As New SPProgUtility.ClsEventLog

    If _ClsProgSetting.GetLogedUSNr = 1 Then
      _clsLog.WriteTempLogFile(String.Format("{1}: Progbez: {0}{2}{0} Namedbez: {0}{3}{0}, Translatedbez: {0}{4}{0}", _
                                  Chr(34), strFuncName, strOrgControlBez, strBez, strTranslatedText), _
                                _ClsProgSetting.GetSpSTempPath & "DeinFile.txt")
    End If

    Return strTranslatedText
  End Function

#End Region


End Module
