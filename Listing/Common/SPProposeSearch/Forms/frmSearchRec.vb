
Imports System.IO
Imports System.Data.SqlClient
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Logging

Public Class frmSearchRec
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()
	Private m_xml As New ClsXML
	Private m_md As Mandant



#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_md = New Mandant

	End Sub

#End Region


	Public ReadOnly Property iValue(ByVal strValue As String) As String

		Get
			Dim strBez As String = String.Empty

			If Me.LvData.SelectedItems.Count = 0 Then
				Return strBez
			End If

			Return ClsDataDetail.GetSelectedBez
		End Get

	End Property

	Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		m_xml.GetChildChildBez(Me)

		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formstyle. {1}", strMethodeName, ex.Message))

		End Try

		' Die DropDownListBox für die Sortierung füllen
		Select Case UCase(ClsDataDetail.strButtonValue)
			Case ("Propose".ToUpper)
				Me.cboDbField.Items.Add("Nummer")
				Me.cboDbField.Items.Add("Bezeichnung")
				Me.cboDbField.SelectedIndex = 1

			Case ("MA")
				Me.cboDbField.Items.Add("Nummer")
				Me.cboDbField.Items.Add("Kandidatenname")
				Me.cboDbField.Items.Add("Firmenname")
				Me.cboDbField.SelectedIndex = 1

			Case ("KD")
				Me.cboDbField.Items.Add("Nummer")
				Me.cboDbField.Items.Add("Firmenname")
				Me.cboDbField.Items.Add("Kandidatenname")
				Me.cboDbField.SelectedIndex = 1

			Case ("VAK")
				Me.cboDbField.Items.Add("Nummer")
				Me.cboDbField.Items.Add("Firmenname")
				Me.cboDbField.Items.Add("Bezeichnung")
				Me.cboDbField.SelectedIndex = 2

		End Select

		SetLvwHeader()
		FillLvData(Me.LvData.Text, Me.txtSearchValue.Text)

		' Set Focus to Textbox
		Me.txtSearchValue.Focus()


	End Sub

	Sub SetLvwHeader()
		Dim strColumnString As String = String.Empty
		Dim strColumnWidthInfo As String = String.Empty
		Dim strUSLang As String = ""

		Select Case UCase(ClsDataDetail.strButtonValue)
			Case ("Propose".ToUpper)
				strColumnString = "Res;Nummer;Bezeichnung"

			Case "MA"
				strColumnString = "Res;Nummer;Kandidat"

			Case "KD"
				strColumnString = "Res;Nummer;Firmenname"

			Case "VAK"
				strColumnString = "Res;Nummer;Firmenname;Bezeichnung"

		End Select

		Select Case UCase(ClsDataDetail.strButtonValue)
			Case ("Propose".ToUpper)
				strColumnWidthInfo = "0-0;100-1;500-0"

			Case "MA".ToUpper
				strColumnWidthInfo = "0-0;100-1;500-0"

			Case "KD".ToUpper
				strColumnWidthInfo = "0-0;100-1;500-0"

			Case "VAK".ToUpper
				strColumnWidthInfo = "0-0;50-1;100-0;100-0"

		End Select
		strColumnString = m_xml.GetSafeTranslationValue(strColumnString)
		FillDataHeaderLv(Me.LvData, strColumnString, strColumnWidthInfo)

	End Sub

	Sub FillLvData(ByVal strField As String, ByVal strFieldValue As String)
		Dim strOperator As String = "="
		Dim strSqlQuery As String = ""
		Dim i As Integer = 0

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Dim cmd As New System.Data.SqlClient.SqlCommand("", Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim str1Param As String = String.Empty
		Dim str2Param As String = String.Empty
		Dim esnr As Integer = CType(Val(strFieldValue), Integer)
		Try

			' Auswahl-Button deaktivieren
			Me.cmdOK.Enabled = False

			Conn.Open()

			Dim sortKey As Integer = 0

			strFieldValue = strFieldValue.Replace("*", "%")
			' Nach was soll explizit sortiert werden.
			Select Case strField
				Case ("Propose".ToUpper)
					If strFieldValue.Replace("%", "").Length = 0 Then
						strFieldValue = "0"
					End If
				Case "Kandidatenname"
					sortKey = 1
					strFieldValue = String.Format("{0}%", strFieldValue)
				Case "Firmenname"
					sortKey = 2
					strFieldValue = String.Format("{0}%", strFieldValue)
				Case "Kandidatennummer"
					sortKey = 3
					If strFieldValue.Replace("%", "").Length = 0 Then
						strFieldValue = "0"
					End If
				Case "Kundennummer"
					sortKey = 4
					If strFieldValue.Replace("%", "").Length = 0 Then
						strFieldValue = "0"
					End If
				Case "Vankanzennummer"
					sortKey = 4
					If strFieldValue.Replace("%", "").Length = 0 Then
						strFieldValue = "0"
					End If
				Case "Vankanzbezeichnung"
					sortKey = 2
					strFieldValue = String.Format("{0}%", strFieldValue)

				Case Else
					If strFieldValue.Replace("%", "").Length = 0 Then
						strFieldValue = String.Format("{0}%", strFieldValue)
					End If

			End Select

			' Zu verwendende gespeicherte Prozedur bestimmen und Parameter hinzufügen.
			Select Case UCase(ClsDataDetail.strButtonValue)
				Case ("Propose".ToUpper)
					strSqlQuery = "[List All Propose For Propose_Search]"
					cmd.Parameters.AddWithValue("@Bez", strFieldValue & "%")
					cmd.Parameters.AddWithValue("@Filiale1", ClsDataDetail.UserData.UserFiliale)

				Case "MA".ToUpper
					strSqlQuery = "[List MAData For Propose_Search]"
					cmd.Parameters.AddWithValue("@Name", strFieldValue)
					cmd.Parameters.AddWithValue("@Filiale1", ClsDataDetail.UserData.UserFiliale)

				Case "KD".ToUpper
					strSqlQuery = "[List KDData For Propose_Search]"
					cmd.Parameters.AddWithValue("@Name", strFieldValue)
					cmd.Parameters.AddWithValue("@Filiale1", ClsDataDetail.UserData.UserFiliale)

				Case "VAK".ToUpper
					strSqlQuery = "[List VakData For Propose_Search]"
					cmd.Parameters.AddWithValue("@Bezeichnung", strFieldValue)
					cmd.Parameters.AddWithValue("@Filiale1", ClsDataDetail.Get4What)

			End Select

			cmd.CommandText = strSqlQuery

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader                  '
			Me.LvData.Items.Clear()
			Me.LvData.FullRowSelect = True
			Dim subItem As ListViewItem.ListViewSubItem

			While rDbrec.Read

				' Reserve
				Me.LvData.Items.Add("")
				Select Case UCase(ClsDataDetail.strButtonValue)
					Case ("Propose".ToUpper)
						If Not IsDBNull(rDbrec("ProposeNr")) Then
							subItem = New ListViewItem.ListViewSubItem
							subItem.Name = "Nummer"
							subItem.Text = rDbrec("ProposeNr").ToString
							Me.LvData.Items(i).SubItems.Add(subItem)
						End If
						' Bezeichnung
						If Not IsDBNull(rDbrec("Bezeichnung")) Then
							subItem = New ListViewItem.ListViewSubItem
							subItem.Name = "Name"
							subItem.Text = rDbrec("Bezeichnung").ToString
							Me.LvData.Items(i).SubItems.Add(subItem)
						End If

					Case UCase("MA")
						' MANr
						If Not IsDBNull(rDbrec("MANr")) Then
							subItem = New ListViewItem.ListViewSubItem
							subItem.Name = "Nummer"
							subItem.Text = rDbrec("MANr").ToString
							Me.LvData.Items(i).SubItems.Add(subItem)
						End If

						' Namen
						If Not IsDBNull(rDbrec("Nachname")) Then
							subItem = New ListViewItem.ListViewSubItem
							subItem.Name = "Name"
							subItem.Text = rDbrec("Nachname").ToString & ", " & rDbrec("Vorname").ToString
							Me.LvData.Items(i).SubItems.Add(subItem)
						End If

					Case UCase("KD")
						' KDNr
						If Not IsDBNull(rDbrec("KDNr")) Then
							subItem = New ListViewItem.ListViewSubItem
							subItem.Name = "Nummer"
							subItem.Text = rDbrec("KDNr").ToString
							Me.LvData.Items(i).SubItems.Add(subItem)
						End If

						' Firma1
						If Not IsDBNull(rDbrec("Firma1")) Then
							subItem = New ListViewItem.ListViewSubItem
							subItem.Name = "Name"
							subItem.Text = rDbrec("Firma1").ToString
							Me.LvData.Items(i).SubItems.Add(subItem)
						End If

					Case UCase("Vak")
						' KDNr
						If Not IsDBNull(rDbrec("VakNr")) Then
							subItem = New ListViewItem.ListViewSubItem
							subItem.Name = "Nummer"
							subItem.Text = rDbrec("VakNr").ToString
							Me.LvData.Items(i).SubItems.Add(subItem)
						End If

						' Bezeichnung
						If Not IsDBNull(rDbrec("Bezeichnung")) Then
							subItem = New ListViewItem.ListViewSubItem
							subItem.Name = "Name"
							subItem.Text = rDbrec("Bezeichnung").ToString
							Me.LvData.Items(i).SubItems.Add(subItem)
						End If

				End Select

				i += 1
			End While


		Catch e As Exception
			Me.LvData.Items.Clear()
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
		Me.Close()
	End Sub

	Private Sub LvData_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles LvData.DoubleClick
		cmdOK_Click(sender, e)
	End Sub

	Private Sub txtSearchValue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearchValue.KeyPress, cmdSearch.KeyPress, cboDbField.KeyPress, cmdOK.KeyPress, cmdClose.KeyPress

		If Asc(e.KeyChar) = Keys.Enter Then
			FillLvData(Me.cboDbField.Text, Me.txtSearchValue.Text)
		ElseIf Asc(e.KeyChar) = Keys.Escape Then
			cmdClose_Click(sender, New System.EventArgs)
		End If

	End Sub

	Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
		Me.Close()
		Me.Dispose()
	End Sub

	Private Sub LvData_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles LvData.KeyPress
		If e.KeyChar = Chr(13) Then ' Enter
			LvData_MouseClick(sender, New MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0))
			cmdOK_Click(sender, New System.EventArgs)
		ElseIf Asc(e.KeyChar) = Keys.Escape Then
			cmdClose_Click(sender, New System.EventArgs)
		End If
	End Sub

	Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
		FillLvData(Me.cboDbField.Text, Me.txtSearchValue.Text)
	End Sub

	Private Sub LvData_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles LvData.KeyUp
		LvData_MouseClick(sender, New MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, 0, 0, 0))
	End Sub

	Private Sub LvData_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LvData.MouseClick
		Dim strValue As String = String.Empty
		Dim strBez As String = String.Empty

		For Each index As Integer In LvData.SelectedIndices
			strValue += LvData.Items(index).SubItems("Nummer").Text & "#@"
			strBez += LvData.Items(index).SubItems("Name").Text & "#@"
		Next

		If strValue.EndsWith("#@") Then strValue = Mid(strValue, 1, Len(strValue) - 2)
		If strBez.EndsWith("#@") Then strBez = Mid(strBez, 1, Len(strBez) - 2)
		ClsDataDetail.GetSelectedNumbers = strValue
		ClsDataDetail.GetSelectedBez = strBez

		Me.cmdOK.Enabled = LvData.SelectedIndices.Count > 0

	End Sub

	Private Sub LvData_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LvData.SelectedIndexChanged

	End Sub


End Class