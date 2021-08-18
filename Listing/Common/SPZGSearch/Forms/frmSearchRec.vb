
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

	Private m_md As Mandant
	Private _ClsData As New ClsDivFunc


#Region "constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_md = New Mandant

	End Sub

#End Region


	Public ReadOnly Property iKDValue(ByVal strValue As String) As String

		Get
			Dim strBez As String = String.Empty

			If Me.LvData.SelectedItems.Count > 0 Then

				If UCase(strValue) = UCase("ZGNr") Then
					strBez = _ClsData.GetZGNr

				ElseIf UCase(strValue) = UCase("MANr") Then
					strBez = _ClsData.GetMANr

				Else
					strBez = _ClsData.GetZGNr

				End If

				Select Case UCase(strValue)
					Case UCase("ZGNr")
						Me.LblChanged.Text = strBez                   ' KDNr

					Case UCase("MANr")
						Me.LblChanged.Text = strBez


				End Select
			End If
			ClsDataDetail.strValueData = CStr(Me.LblChanged.Text)

			Return ClsDataDetail.strValueData
		End Get

	End Property

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = ClsDataDetail.m_Translate.GetSafeTranslationValue(Me.Text)

			lblHeaderFett.Text = ClsDataDetail.m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
			lblHeaderNormal.Text = ClsDataDetail.m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)
			lblDetails.Text = ClsDataDetail.m_Translate.GetSafeTranslationValue(lblDetails.Text)
			lblSearchField.Text = ClsDataDetail.m_Translate.GetSafeTranslationValue(lblSearchField.Text)

			cmdClose.Text = ClsDataDetail.m_Translate.GetSafeTranslationValue(cmdClose.Text)
			cmdOK.Text = ClsDataDetail.m_Translate.GetSafeTranslationValue(cmdOK.Text)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frmDataSel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		TranslateControls()

		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formstyle. {1}", strMethodeName, ex.Message))

		End Try

		Me.cboDbField.Items.Add("ZG-Nummer")
		Me.cboDbField.Items.Add("MA-Nummer")
		Me.cboDbField.Items.Add("Kandidatenname")

		SetLvwHeader()
		Me.LblChanged.Text = String.Empty

		FillLvData(Me.LvData.Text, Me.txtSearchValue.Text)

	End Sub

	Sub SetLvwHeader()
		Dim strColumnString As String = String.Empty
		Dim strColumnWidthInfo As String = String.Empty
		Dim strUSLang As String = ""

		strColumnString = "ZGNr;MANr;MAName"
		If ClsDataDetail.strButtonValue.ToLower = "zg" Then
			strColumnWidthInfo = "200-1;200-0;500-0"
		Else
			strColumnWidthInfo = "0;200-0;500-0"

		End If
		strColumnString = ClsDataDetail.m_Translate.GetSafeTranslationValue(strColumnString)
		FillDataHeaderLv(Me.LvData, strColumnString, strColumnWidthInfo)

	End Sub

	Sub FillLvData(ByVal strField As String, ByVal strFieldValue As String)
		Dim strOperator As String = "="
		Dim strSqlQuery As String = ""
		Dim strSearchArt As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim str1Param As String = String.Empty
		Dim str2Param As String = String.Empty
		Dim str3Param As String = String.Empty

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand

			Select Case UCase(ClsDataDetail.strButtonValue)
				Case UCase("ZG")
					strSearchArt = UCase("ZGNummer")

				Case Else
					If Me.cboDbField.Text = String.Empty Then
						If IsNumeric(strFieldValue) Then strSearchArt = UCase("MANummer")

					Else
						strSearchArt = UCase("MAName")

					End If


			End Select
			Select Case strSearchArt.ToUpper
				Case UCase("ZGNummer")
					strSqlQuery = "[List ZGNrData For Search In ZGSearch]"
					str1Param = "@ZGNUMBER"

				Case UCase("MANummer")
					strSqlQuery = "[List MANrData For Search in ZGSearch]"
					str1Param = "@MANumber"

				Case Else
					strSqlQuery = "[List MANameData For Search in ZGSearch]"
					str1Param = "@MANAME"

			End Select
			str2Param = "@Filiale1"

			cmd = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim param As System.Data.SqlClient.SqlParameter
			If Me.txtSearchValue.Text <> String.Empty Then
				param = cmd.Parameters.AddWithValue(str1Param, Me.txtSearchValue.Text)
			End If
			If str2Param <> String.Empty Then
				param = cmd.Parameters.AddWithValue(str2Param, ClsDataDetail.m_InitialData.UserData.UserFiliale)
			End If
			If str3Param <> String.Empty Then
				param = cmd.Parameters.AddWithValue(str3Param, ClsDataDetail.m_InitialData.UserData.UserFiliale)
			End If

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader
			Me.LvData.Items.Clear()
			Me.LvData.FullRowSelect = True

			While rZGrec.Read

				If ClsDataDetail.strButtonValue.ToLower = "zg" Then Me.LvData.Items.Add(rZGrec("ZGNr").ToString) Else Me.LvData.Items.Add("0")

				If Not IsDBNull(rZGrec("MANr")) Then
					Me.LvData.Items(i).SubItems.Add(rZGrec("MANr").ToString)
				Else
					Me.LvData.Items(i).SubItems.Add("")
				End If
				If Not IsDBNull(rZGrec("MAName")) Then
					Me.LvData.Items(i).SubItems.Add(rZGrec("MAName").ToString)
				Else
					Me.LvData.Items(i).SubItems.Add("")
				End If

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

	Private Sub LvData_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LvData.MouseClick
		Dim strValue As String = String.Empty
		Dim strBez As String = String.Empty

		If UCase(ClsDataDetail.strButtonValue) = UCase("ZG") Then

			For i As Integer = 0 To LvData.Items.Count - 1
				If LvData.Items(i).Selected = True Then
					strBez += LvData.Items(i).SubItems(CInt(IIf(UCase(ClsDataDetail.Get4What) = UCase("ZGNr"), 0, 1).ToString)).Text & ","
				End If

			Next
			If strBez.Contains(",") Then strBez = Mid(strBez, 1, Len(strBez) - 1)
			If UCase(ClsDataDetail.Get4What) = UCase("ZGNr") Then
				_ClsData.GetZGNr = strBez
			Else
				_ClsData.GetMANr = strBez

			End If

		Else
			For i As Integer = 0 To LvData.Items.Count - 1
				If LvData.Items(i).Selected Then
					strBez += LvData.Items(i).SubItems(1).Text & ","
				End If

			Next

			If strBez.Contains(",") Then strBez = Mid(strBez, 1, Len(strBez) - 1)
			_ClsData.GetMANr = strBez

		End If
		Me.LblChanged.Text = strBez

	End Sub

	Private Sub txtSearchValue_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearchValue.KeyPress

		If Asc(e.KeyChar) = Keys.Enter Then
			FillLvData(Me.cboDbField.Text, Me.txtSearchValue.Text)
		End If

	End Sub

	Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
		Me.Close()
		Me.Dispose()
	End Sub




	Private Sub LvData_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LvData.SelectedIndexChanged

	End Sub
End Class