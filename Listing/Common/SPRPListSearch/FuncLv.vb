
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports SPRPListSearch.ClsDataDetail
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Logging

Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsFunc As New ClsDivFunc
	Private _ClsReg As New SPProgUtility.ClsDivReg


	Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


#Region "Dropdown-Funktionen für 1. Seite..."



	Sub ListRPFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal monat As Integer?, ByVal jahr As Integer?, ByVal berater As String)
		Dim m_utility As New Utilities

		Try
			Dim sql As String
			sql = "[List All UserBranches For Listing In Not Done Reports]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_utility.ReplaceMissing(m_InitialData.MDData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("kst", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", m_utility.ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", m_utility.ReplaceMissing(jahr, 0)))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_utility.SafeGetString(reader, "Filiale"))

			End While

		Catch e As Exception
			m_Logger.LogError(e.ToString())

		Finally


		End Try

	End Sub

	Sub ListREKst1(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal monat As Integer?, ByVal jahr As Integer?)
		Dim m_utility As New Utilities

		Try

			Dim sql As String
			sql = "SELECT RP.RPKst1 As Bezeichnung From RP "
			sql &= "WHERE RP.RPKst2 Is Not Null And RP.RPKst2 <> '' "
			sql &= "And RP.MDNr = @MDNr "
      'naas -- Entfernt 28.08.2018
      'sql &= "And RP.Erfasst <> 1 "
      sql &= "And (@Monat = 0 Or RP.Monat = @Monat) "
			sql &= "And (@Jahr = 0 Or RP.Jahr = @Jahr) "

			sql &= "Group By RP.RPKst1 "
			sql &= "Order By RP.RPKst1"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_utility.ReplaceMissing(m_InitialData.MDData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", m_utility.ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", m_utility.ReplaceMissing(jahr, 0)))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)


			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_utility.SafeGetString(reader, "Bezeichnung"))

			End While

		Catch e As Exception
			m_Logger.LogError(e.ToString())

		End Try

	End Sub

	Sub ListREKst2(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal monat As Integer?, ByVal jahr As Integer?)
		Dim m_utility As New Utilities

		Try

			Dim sql As String
			sql = "SELECT RP.RPKst2 As Bezeichnung From RP "
			sql &= "WHERE RP.RPKst2 Is Not Null And RP.RPKst2 <> '' "
      sql &= "And RP.MDNr = @MDNr "
      'naas -- Entfernt 28.08.2018
      'sql &= "And RP.Erfasst <> 1 "
      sql &= "And (@Monat = 0 Or RP.Monat = @Monat) "
			sql &= "And (@Jahr = 0 Or RP.Jahr = @Jahr) "

			sql &= "Group By RP.RPKst2 "
			sql &= "Order By RP.RPKst2"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_utility.ReplaceMissing(m_InitialData.MDData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", m_utility.ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", m_utility.ReplaceMissing(jahr, 0)))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)


			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_utility.SafeGetString(reader, "Bezeichnung"))

			End While

		Catch e As Exception
			m_Logger.LogError(e.ToString())

		End Try

	End Sub

	Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit, ByVal monat As Integer?, ByVal jahr As Integer?)
		Dim m_utility As New Utilities

		Try

			Dim sql As String
			sql = "SELECT MA.KST, (US.Nachname + ', ' + US.Vorname) As USName From RP "
			sql &= "Left Join Mitarbeiter MA On RP.MANr = MA.MANr "
			sql &= "Left Join Benutzer US On MA.KST = US.KST "
			sql &= "WHERE MA.KST <> '' and (US.Nachname Is Not Null Or US.Nachname <> '') "
      sql &= "And RP.MDNr = @MDNr "
      'naas -- Entfernt 28.08.2018
      'sql &= "And RP.Erfasst <> 1 "
      sql &= "And (@Monat = 0 Or RP.Monat = @Monat) "
			sql &= "And (@Jahr = 0 Or RP.Jahr = @Jahr) "
			sql &= "And (@Filiale = '' Or USFiliale = @Filiale) "

			sql &= "Group By MA.KST, "
			sql &= "US.Nachname + ', ' + US.Vorname "
			sql &= "Order By MA.KST, (US.Nachname + ', ' + US.Vorname)"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", m_utility.ReplaceMissing(m_InitialData.MDData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Monat", m_utility.ReplaceMissing(monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Jahr", m_utility.ReplaceMissing(jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Filiale", m_utility.ReplaceMissing(m_InitialData.UserData.UserFiliale, String.Empty)))

			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(New ComboValue(m_utility.SafeGetString(reader, "USName"), m_utility.SafeGetString(reader, "KST")))
			End While
			cbo.Properties.DropDownRows = 20


		Catch e As Exception
			m_Logger.LogError(e.ToString())

		End Try

	End Sub

	Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit, ByVal filiale As String)
		Dim strSqlQuery As String = "[List AllBerater For Search In RPList]"
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@filiale", filiale)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rESrec.Read
				cbo.Properties.Items.Add(New ComboValue(rESrec("USName").ToString, rESrec("KST").ToString))

			End While
			cbo.Properties.DropDownRows = 15

		Catch e As Exception
			m_Logger.LogError(e.ToString())
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListRPMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim m_utility As New Utilities
		Dim strSqlQuery As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetRPLP]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_utility.SafeGetInteger(reader, "Monat", 0))

				i += 1
			End While
			cbo.Properties.DropDownRows = 12

		Catch e As Exception
			m_Logger.LogError(e.ToString())
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListRPYear(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim m_utility As New Utilities
		Dim strSqlQuery As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetRPYear]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim reader As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While reader.Read
				cbo.Properties.Items.Add(m_utility.SafeGetInteger(reader, "Jahr", 0))

				i += 1
			End While
			cbo.Properties.DropDownRows = 10

		Catch e As Exception
			m_Logger.LogError(e.ToString())
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

#End Region




#Region "Sonstige Funktions..."

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

		Catch e As Exception
			strKSTResult = String.Empty
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strKSTResult
	End Function


#End Region




End Module
