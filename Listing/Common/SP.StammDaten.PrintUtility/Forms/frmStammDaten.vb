
Imports System.IO

'Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports System.Threading
Imports DevExpress.XtraBars
Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Metro.ColorTables
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Logging


Public Class frmStammDaten

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()
	Private Property _StammSetting As New ClsStammDatenSetting

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'Dim _ClsXML As New ClsXML

	Dim PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean
	Private Property bPrintAsExport As Boolean
	Private Property liMANr As New List(Of Integer)
	Private Property liKDNr As New List(Of Integer)
	Private Property liVakNr As New List(Of Integer)
	Private Property liProposeNr As New List(Of Integer)

	Private Property ResultOFExportingRec As String

	Private m_AllowedDesign As Boolean


	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount

		'_ClsXML.GetChildChildBez(Me)
		Me.bbiPrint.Caption = _ClsProgSetting.TranslateText(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = _ClsProgSetting.TranslateText(Me.bbiExport.Caption)
		Me.bbiDesign.Caption = _ClsProgSetting.TranslateText(Me.bbiDesign.Caption)

		Me.Text = _ClsProgSetting.TranslateText(Me.Text)

		If _ClsProgSetting.GetLogedUSNr = 1 Then
			m_Logger.LogInfo(String.Format("{0}. Ladezeit für Translation: {1} s.", strMethodeName, ((System.Environment.TickCount - Time_1) / 1000)))
		End If

	End Sub

	Private Sub frmStammDaten_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

			My.Settings.Save()
		End If

	End Sub

	Private Sub frmStammDaten_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.KeyPreview = True
			Dim strStyleQuery As String = "//Layouts/Form_DevEx/FormStyle"
			Dim strStyleName As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strStyleQuery, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			Try
				If My.Settings.frmLocation <> String.Empty Then
					Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try

		Try
			' UpdateMethod() enthält den Code, der ein Windows-Steuerelement modifiziert.
			Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
			' UpdateMethod() wird nun auf dem Benutzeroberflächen-Thread aufgerufen.
			Me.Invoke(UpdateDelegate)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try

		'Me.RadialMenu1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias
		Dim _e As New System.EventArgs
		Dim _sender As New DevComponents.DotNetBar.RadialMenuItem	'= DirectCast(_sender, DevComponents.DotNetBar.RadialMenuItem)

		Try
			Dim bIsDesignAllowed As Boolean
			Dim bIsPrintAllowed As Boolean

			If _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Kandidat Then
				_sender = itm_kandidat
			ElseIf _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Kunde Then
				_sender = itm_kunde

			ElseIf _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Vakanz Then
				_sender = itm_vakanz

			ElseIf _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Vorschlag Then
				_sender = itm_vorschlag

			Else
				Exit Sub

			End If
			RadialMenu2_ItemClick(_sender, _e)

			Try
				' Stammblatt drucken
				bIsDesignAllowed = IsUserActionAllowed(0, 105)
				bIsPrintAllowed = IsUserActionAllowed(0, 104)
				If Not (bIsDesignAllowed And bIsPrintAllowed) Then Me.itm_kandidat.Enabled = False

				bIsDesignAllowed = IsUserActionAllowed(0, 205)
				bIsPrintAllowed = IsUserActionAllowed(0, 204)
				If Not (bIsDesignAllowed And bIsPrintAllowed) Then Me.itm_kunde.Enabled = False

				bIsDesignAllowed = IsUserActionAllowed(0, 705)
				bIsPrintAllowed = IsUserActionAllowed(0, 704)
				If Not (bIsDesignAllowed And bIsPrintAllowed) Then Me.itm_vakanz.Enabled = False

				bIsDesignAllowed = IsUserActionAllowed(0, 805)
				bIsPrintAllowed = IsUserActionAllowed(0, 804)
				If Not (bIsDesignAllowed And bIsPrintAllowed) Then Me.itm_vorschlag.Enabled = False

				Me.itm_vakanz.Enabled = False
				Me.itm_vorschlag.Enabled = False

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Benutzerrechte:{1}", strMethodeName, ex.Message))
				' Alles sperren...
				Me.bbiPrint.Visibility = BarItemVisibility.Never
				Me.bbiExport.Visibility = BarItemVisibility.Never

			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setzen von Property: {1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Public Sub New(ByVal _setting As ClsStammDatenSetting)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me._StammSetting = _setting

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Kandidaten#PrintMA", _
																					 "Kunden#PrintKD", _
																					 "Vakanzen#PrintVak", _
																					 "Vorschlag#PrintPropose"}
		Try
			BarManager1.ForceInitialize()

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))

				If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then bshowMnu = IsUserActionAllowed(0, 254) Else bshowMnu = myValue(0).ToString <> String.Empty
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.AllowHtmlText = True

					itm.Caption = _ClsProgSetting.TranslateText(myValue(0).ToString)
					itm.Name = _ClsProgSetting.TranslateText(myValue(1).ToString)
					If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)

					'AddHandler itm.ItemClick, AddressOf GetMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Private Sub btnWhatPrint_Click(sender As System.Object, e As System.EventArgs) Handles btnWhatPrint.Click
		Me.RadialMenu2.IsOpen = True
	End Sub

	Private Sub RadialMenu2_ItemClick(sender As System.Object, e As System.EventArgs) Handles RadialMenu2.ItemClick

		Me.LabelControl2.Visible = False
		Me.txtNumber.Visible = False

		Select Case sender.name.ToString.ToLower
			Case "itm_kandidat".ToLower
				Me.LabelControl2.Text = _ClsProgSetting.TranslateText("Kandidatennummer")
				Me.LabelControl2.Visible = True
				Me.txtNumber.Visible = True
				_StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Kandidat
				Me.PrintJobNr = "1.0"

			Case "itm_kunde".ToLower
				Me.LabelControl2.Text = _ClsProgSetting.TranslateText("Kundennummer")
				Me.LabelControl2.Visible = True
				Me.txtNumber.Visible = True
				_StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Kunde
				Me.PrintJobNr = "2.0"

			Case "itm_vakanz".ToLower
				Me.LabelControl2.Text = _ClsProgSetting.TranslateText("Vakanzennummer")
				Me.LabelControl2.Visible = True
				Me.txtNumber.Visible = True
				_StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Vakanz
				Me.PrintJobNr = "19.0"

			Case "itm_vorschlag".ToLower
				Me.LabelControl2.Text = _ClsProgSetting.TranslateText("Vorschlagnummer")
				Me.LabelControl2.Visible = True
				Me.txtNumber.Visible = True
				_StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Vorschlag
				Me.PrintJobNr = "18.0"

		End Select
		ReadUserRights()

	End Sub

	Sub ReadUserRights()
		Dim bIsPrintAllowed As Boolean
		Dim bIsExportAllowed As Boolean

		If _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Kandidat Then
			m_AllowedDesign = IsUserActionAllowed(0, 105)
			bIsPrintAllowed = IsUserActionAllowed(0, 104)
			bIsExportAllowed = IsUserAllowed4DocExport("1.0")

		ElseIf _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Kunde Then
			m_AllowedDesign = IsUserActionAllowed(0, 205)
			bIsPrintAllowed = IsUserActionAllowed(0, 204)
			bIsExportAllowed = IsUserAllowed4DocExport("2.0")

		ElseIf _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Vakanz Then
			m_AllowedDesign = IsUserActionAllowed(0, 705)
			bIsPrintAllowed = IsUserActionAllowed(0, 704)
			bIsExportAllowed = IsUserAllowed4DocExport("19.0")

		ElseIf _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Vorschlag Then
			m_AllowedDesign = IsUserActionAllowed(0, 805)
			bIsPrintAllowed = IsUserActionAllowed(0, 804)
			bIsExportAllowed = IsUserAllowed4DocExport("15.0")

		End If
		Me.bbiPrint.Visibility = If(bIsPrintAllowed, BarItemVisibility.Always, BarItemVisibility.Never)
		Me.bbiDesign.Visibility = If(m_AllowedDesign, BarItemVisibility.Always, BarItemVisibility.Never)
		Me.bbiExport.Visibility = If(bIsExportAllowed, BarItemVisibility.Always, BarItemVisibility.Never)

	End Sub

	Sub PrintMATemplate()
		Dim strResult As String = "Success..."
		bPrintAsDesign = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		If bPrintAsDesign Then bPrintAsDesign = False

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMASearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetDbConnString, _
																																									 .SQL2Open = Me.SQL4Print, _
																																									 .JobNr2Print = Me.PrintJobNr, _
																																									.ShowAsDesign = Me.bPrintAsDesign, _
																																									.liMANr2Print = Me.liMANr,
																																									.ShowMessageIFNotFounded = If(Me.liMANr.Count > 1, False, True)}
		Dim obj As New SPS.Listing.Print.Utility.MAStammblatt.ClsPrintMAStammblatt(_Setting)
		If Me.bPrintAsExport Then
			strResult = obj.ExportMAStammBlatt()
			Me.ResultOFExportingRec = strResult
			StartExportingCompleted()

		Else
			strResult = obj.PrintMAStammBlatt()

		End If

	End Sub

	Sub PrintKDTemplate()
		Dim strResult As String = "Success..."
		bPrintAsDesign = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		If bPrintAsExport Then bPrintAsDesign = False

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLKDSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetDbConnString, _
																																									 .SQL2Open = Me.SQL4Print, _
																																									 .JobNr2Print = Me.PrintJobNr, _
																																									.ShowAsDesign = Me.bPrintAsDesign, _
																																									.liKDNr2Print = Me.liKDNr,
																																									.ShowMessageIFNotFounded = If(Me.liKDNr.Count > 1, False, True)}
		Dim obj As New SPS.Listing.Print.Utility.KDStammblatt.ClsPrintKDStammblatt(_Setting)
		If Me.bPrintAsExport Then
			strResult = obj.ExportKDStammBlatt()
			Me.ResultOFExportingRec = strResult
			StartExportingCompleted()

		Else
			strResult = obj.PrintKDStammBlatt()

		End If

	End Sub

	Sub PrintVakTemplate()
		Dim strResult As String = "Success..."
		bPrintAsDesign = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		If bPrintAsExport Then bPrintAsDesign = False

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLKDSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetDbConnString, _
																																									 .SQL2Open = Me.SQL4Print, _
																																									 .JobNr2Print = Me.PrintJobNr, _
																																									.ShowAsDesign = Me.bPrintAsDesign, _
																																									.liKDNr2Print = Me.liKDNr}
		Dim obj As New SPS.Listing.Print.Utility.KDStammblatt.ClsPrintKDStammblatt(_Setting)
		If Me.bPrintAsExport Then
			strResult = obj.ExportKDStammBlatt()
			Me.ResultOFExportingRec = strResult
			StartExportingCompleted()

		Else
			strResult = obj.PrintKDStammBlatt()

		End If

	End Sub

	Sub PrintProposeTemplate()
		Dim strResult As String = "Success..."
		bPrintAsDesign = m_AllowedDesign AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown)
		If bPrintAsExport Then bPrintAsDesign = False

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLKDSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetDbConnString, _
																																									 .SQL2Open = Me.SQL4Print, _
																																									 .JobNr2Print = Me.PrintJobNr, _
																																									.ShowAsDesign = Me.bPrintAsDesign, _
																																									.liKDNr2Print = Me.liKDNr}
		Dim obj As New SPS.Listing.Print.Utility.KDStammblatt.ClsPrintKDStammblatt(_Setting)
		If Me.bPrintAsExport Then
			strResult = obj.ExportKDStammBlatt()
			Me.ResultOFExportingRec = strResult
			StartExportingCompleted()

		Else
			strResult = obj.PrintKDStammBlatt()

		End If

	End Sub

	Sub StartWithPrinting()

		Me.liKDNr = Me.liMANr
		Me.liVakNr = Me.liMANr
		Me.liProposeNr = Me.liMANr

		If Me.liMANr.Count > 0 Then
			If _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Kandidat Then
				PrintMATemplate()
			ElseIf _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Kunde Then
				PrintKDTemplate()
			ElseIf _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Vakanz Then
				PrintVakTemplate()
			ElseIf _StammSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Vorschlag Then
				PrintProposeTemplate()
			End If
		End If

	End Sub

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim aMANr As String() = Me.txtNumber.Text.Split(CChar(","))
		Me.liMANr.Clear()

		If Me.txtNumber.Text.Contains(CChar(",")) Then
			aMANr = Me.txtNumber.Text.Split(CChar(","))
			For i As Integer = 0 To aMANr.Length - 1
				Me.liMANr.Add(CInt(Val(aMANr(i))))
			Next

		ElseIf Me.txtNumber.Text.Contains(CChar("-")) Then
			Dim aNr As String() = Me.txtNumber.Text.Split(CChar("-"))
			If aNr.Length > 1 Then
				Dim j As Integer = 0
				For i As Integer = aNr(0) To aNr(1)
					liMANr.Add(i)
					j += 1
				Next
			End If

		Else
			liMANr.Add(aMANr(0))

		End If

		Me.SQL4Print = ""
		Me.bPrintAsExport = False

		If Me.liMANr.Count > 0 Then
			StartWithPrinting()
		Else
			Dim strMsg As String = "Sie haben noch keine Daten zum Druck ausgewählt."
			DevExpress.XtraEditors.XtraMessageBox.Show(TranslateText(strMsg), TranslateText("Daten drucken"))

		End If

	End Sub

	Private Sub bbiDesign_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDesign.ItemClick
		Dim aMANr As String() = Me.txtNumber.Text.Split(CChar(","))

		Me.liMANr.Clear()
		Me.SQL4Print = ""
		Me.bPrintAsExport = False

		For i As Integer = 0 To aMANr.Length - 1
			Me.liMANr.Add(CInt(Val(aMANr(i))))
		Next

		If Me.liMANr.Count > 0 Then
			StartWithPrinting()
		Else
			Dim strMsg As String = "Sie haben noch keine Daten zum Druck ausgewählt."
			DevExpress.XtraEditors.XtraMessageBox.Show(TranslateText(strMsg), TranslateText("Daten drucken"))

		End If

	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim aMANr As String() = Me.txtNumber.Text.Split(CChar(","))

		Me.liMANr.Clear()
		Me.SQL4Print = ""
		Me.bPrintAsExport = True

		For i As Integer = 0 To aMANr.Length - 1
			Me.liMANr.Add(CInt(Val(aMANr(i))))
		Next

		If Me.liMANr.Count > 0 Then
			StartWithPrinting()
		Else
			Dim strMsg As String = "Sie haben noch keine Daten zum Druck ausgewählt."
			DevExpress.XtraEditors.XtraMessageBox.Show(TranslateText(strMsg), TranslateText("Daten exportieren"))

		End If

	End Sub

	Sub StartExportingCompleted()

		If File.Exists(ResultOFExportingRec) Then
			Dim strMsg As String = String.Format(_ClsProgSetting.TranslateText("Ihre Daten wurden erfolgreich in {0} gespeichert."), ResultOFExportingRec)

			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, _ClsProgSetting.TranslateText("Daten exportieren"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Information)
			Process.Start(ResultOFExportingRec)

		Else
			Dim strMsg As String = String.Format(_ClsProgSetting.TranslateText("Ihre Daten konnten nicht erfolgreich gespeichert werden.{0}{1}"), _
																					 vbNewLine, ResultOFExportingRec)

			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, _ClsProgSetting.TranslateText("Daten exportieren"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If

	End Sub

	Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub


End Class

