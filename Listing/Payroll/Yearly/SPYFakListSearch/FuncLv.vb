
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

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility

Imports SPYFakListSearch.ClsDataDetail


Module FuncLv

	Private m_Logger As ILogger = New Logger()
	Private m_UtilityUi As New SP.Infrastructure.UI.UtilityUI


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


  Function GetLAName(ByVal dLANr As Decimal) As String
    Dim strResult As String = String.Empty

    Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
      Dim strQuery As String = "Select Top 1 LALOText From LA Where LANr = @LANr And LAJahr = @Year"

      Dim Time_1 As Double = System.Environment.TickCount
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      cmd.CommandType = CommandType.Text

      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@LANr", dLANr)
      param = cmd.Parameters.AddWithValue("@Year", Now.Year)

      Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

      While rFoundedrec.Read
        strResult = CStr(rFoundedrec("LALOText"))

      End While


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)

    End Try

    Return strResult
  End Function

#End Region


#Region "Sonstige Funktions..."

  Sub DeleteRecFromDb(ByVal strYear As String)
    Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
      Dim i As Integer = 0

      Dim strResult As String = "[Dbo].[Create Table 4 Fakabrechnung]"

      Try
        Dim Time_1 As Double = System.Environment.TickCount
        Conn.Open()

        Dim cmd As System.Data.SqlClient.SqlCommand
        cmd = New System.Data.SqlClient.SqlCommand(strResult, Conn)
        cmd.CommandType = CommandType.StoredProcedure

        Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@USNr", m_InitialData.UserData.UserNr)

        cmd.ExecuteNonQuery()     ' Datensatz hinzufügen...


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUi.ShowErrorDialog(ex.ToString)


      Finally
        Conn.Close()
        Conn.Dispose()

      End Try


    Catch e As Exception
      MsgBox(e.Message)

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

      Dim rPLZrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

      While rPLZrec.Read
        strPLZResult += rPLZrec(strFieldName).ToString & ","

      End While

      If strPLZResult.Length > 1 Then
        strPLZResult = Mid(strPLZResult, 2, Len(strPLZResult) - 2)
        strPLZResult = Replace(strPLZResult, ",", "','")
      Else
        strPLZResult = String.Empty
      End If

		Catch ex As Exception
			strPLZResult = String.Empty
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)


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

      Dim rPLZrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

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

		Catch ex As Exception
			strKSTResult = String.Empty
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)


    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

    Return strKSTResult
  End Function

  Sub ListForActivate(ByVal cbo As ComboBox)
    cbo.Items.Clear()
    Try
      cbo.Items.Add("")

      cbo.Items.Add("Aktiviert")
      cbo.Items.Add("Nicht Aktiviert")

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)


    Finally

    End Try

  End Sub

  Sub ListForActivate_1(ByVal cbo As ComboBox)
    cbo.Items.Clear()
    Try
      cbo.Items.Add("")

      cbo.Items.Add("Aktiviert")
      cbo.Items.Add("Nicht Aktiviert")

      cbo.Items.Add("Leere Felder")

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)


    Finally

    End Try

  End Sub

#End Region


End Module
