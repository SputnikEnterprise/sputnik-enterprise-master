
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Utility

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data

Module FuncLv
  Dim _ClsFunc As New ClsDivFunc
	'Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private m_UtilityUI As New UtilityUI


#Region "Dropdown-Funktionen für 1. Seite..."


	Sub ListBVGListeJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = "Select LO.Jahr From LO "
		strSqlQuery += "Where LO.MDNr = @MDnr Group By LO.Jahr Order By LO.Jahr DESC"


		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)

			Dim rFOPrec As SqlDataReader = cmd.ExecuteReader									'

			cbo.Properties.Items.Clear()
			While rFOPrec.Read
				cbo.Properties.Items.Add(rFOPrec("Jahr"))
			End While


		Catch e As Exception
			m_UtilityUI.ShowErrorDialog(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListBVGListeMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal year As Integer)
		Dim strSqlQuery As String = "Select LO.LP From LO "
		strSqlQuery += "Where LO.MDNr = @MDnr And LO.Jahr = @Year Group By LO.LP Order By LO.LP ASC"


		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()
			If year = 0 Then year = Now.Year

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)
			cmd.Parameters.AddWithValue("@Year", year)

			Dim rFOPrec As SqlDataReader = cmd.ExecuteReader									'

			cbo.Properties.Items.Clear()
			While rFOPrec.Read
				cbo.Properties.Items.Add(rFOPrec("LP"))

			End While


		Catch e As Exception
			m_UtilityUI.ShowErrorDialog(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub


	Sub ListBVGListLohnarten(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit,
													 ByVal JahrVon As Integer?,
													 ByVal JahrBis As Integer?,
													 ByVal MonatVon As Integer?,
													 ByVal MonatBis As Integer?)
		Dim strText As String
		Dim strValue As String
		Dim strSqlQuery As String = "[Show LAData For Search In BVGList]"

		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)
			cmd.Parameters.AddWithValue("@JahrVon", ReplaceMissing(JahrVon, Now.Year))
			cmd.Parameters.AddWithValue("@JahrBis", ReplaceMissing(JahrVon, Now.Year))

			cmd.Parameters.AddWithValue("@MonatVon", ReplaceMissing(MonatVon, Now.Month))
			cmd.Parameters.AddWithValue("@MonatBis", ReplaceMissing(MonatBis, Now.Month))


			Dim rFOPrec As SqlDataReader = cmd.ExecuteReader

			Dim allValues As String = ""
			cbo.Properties.Items.Clear()
			While rFOPrec.Read
				strText = String.Format("{0} {1}", rFOPrec("LANR"), rFOPrec("LALOText"))
				strValue = String.Format("{0}", rFOPrec("LANR"))
				allValues += String.Format("{0},", strValue)
				cbo.Properties.Items.Add(New ComboValue(strText, strValue))

				i += 1
			End While

			' Wenn eine Lohnart gefunden wurde, so auch Item "Alle" hinzufügen.
			If allValues.Length > 0 Then
				allValues = allValues.Substring(0, allValues.Length - 1)
				cbo.Properties.Items.Insert(0, New ComboValue("Alle", allValues))
			End If


			' Automatisch erstes Item selektieren, falls vorhanden
			If cbo.Properties.Items.Count > 0 Then
				cbo.SelectedIndex = 0
			End If

		Catch e As Exception
			m_UtilityUI.ShowErrorDialog(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

#End Region

#Region "Allgemeine Funktionen"

	Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function

	' Monate 1 bis 12
	Sub ListCboMonate1Bis12(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		For i As Integer = 1 To 12
			cbo.Properties.Items.Add(New ComboValue(i.ToString, i.ToString))
		Next
		cbo.SelectedIndex = Date.Now.Month - 1
	End Sub

	Function CheckIfRunning(ByVal proccessname As String) As Boolean

		For Each clsProcess As Process In Process.GetProcesses()
			If clsProcess.ProcessName.ToLower.Contains(proccessname.ToLower) Then
				Return True
			End If
		Next

		Return False

	End Function



#End Region


End Module
