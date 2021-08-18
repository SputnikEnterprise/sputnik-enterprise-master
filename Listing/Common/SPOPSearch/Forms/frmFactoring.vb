
Imports System.IO
Imports System.Data.SqlClient
Imports System.Threading

Imports DevExpress.LookAndFeel
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPOPSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class frmFactoring
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	'Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg

	Private m_utility As New Utilities
	Private m_common As New CommonSetting
	Private m_md As New Mandant

	Private _docpath As String
	Private _docname As String
	Private _docnameFull As String
	Private _date As String

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean


#Region "Constructor"

	Public Sub New(Optional ByVal docname As String = "KMUFactoringExport.csv")

		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		' Voreinstellungen aus dem Settings
		_docpath = m_utility.GetMyDocumentsPathWithBackSlash
		If My.Settings.Filepath4Factoring.Length > 0 Then
			_docpath = My.Settings.Filepath4Factoring
		End If
		If My.Settings.Filename4Factoring.Length > 0 Then
			docname = My.Settings.Filename4Factoring
		End If
		' Dokumentname vorbereiten
		Dim endung As String = ".csv"
		If docname.Contains(".") Then
			endung = docname.Substring(docname.LastIndexOf(CChar(".")), docname.Length - docname.LastIndexOf(CChar(".")))
		End If
		_date = String.Format("{0}{1:00}{2:00}", Date.Now.Year, Date.Now.Month, Date.Now.Day)
		_docname = docname
		_docnameFull = String.Format("{0}_{1}{2}", docname.Replace(endung, ""), _date, endung)

		Me.txtFilename.Text = _docpath & _docnameFull
	End Sub

#End Region


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Close()
		Me.Dispose()
	End Sub

	Private Sub txtFilename_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtFilename.ButtonClick
		Dim fldlgList As New FolderBrowserDialog

		With fldlgList
			'set the RootFolder
			'      .RootFolder = Environment.SpecialFolder.Personal
			' optional Description to provide additional instructions. 
			.Description = m_Translate.GetSafeTranslationValue("Bitte wählen Sie ein Verzeichnis für Export der Datei.")
			.SelectedPath = Me.txtFilename.Text

			.ShowNewFolderButton = True
			If .ShowDialog() = DialogResult.OK Then
				_docpath = _ClsReg.AddDirSep(.SelectedPath)
				Me.txtFilename.Text = _docpath + _docnameFull
			End If

		End With
	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick

		If Me.txtFilename.Text = String.Empty Then
			Me.txtFilename.Text = _docpath & _docnameFull

		Else
			If Not Me.txtFilename.Text.ToUpper.EndsWith(".csv".ToUpper) Then Me.txtFilename.Text &= ".csv"
			Dim MyFile As FileInfo = New FileInfo(Me.txtFilename.Text)

		End If

		Try
			IO.File.WriteAllLines(Me.txtFilename.Text, GetDataForFactoring().ToArray, System.Text.Encoding.Default)

			MessageBox.Show(String.Format("Die Datei {0} wurde erfolgreich gespeichert.", Me.txtFilename.Text), "Daten gespeichert",
											MessageBoxButtons.OK, MessageBoxIcon.Information)
			Dim strDirectory As String = Path.GetDirectoryName(Me.txtFilename.Text)
			System.Diagnostics.Process.Start(strDirectory)

			SaveSettings()
			GetData4Print(False, False, "7.16")

		Catch ex As Exception
			MessageBox.Show(ex.Message, "Speicherung nicht durchgeführt", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	Function GetDataForFactoring() As List(Of String)
		Dim lst As New List(Of String)
		Dim zeile As String = ""
		Dim strPipe As String = ";"
		' In der Mandantenverwaltung die Kundennummer
		Dim MDkundennummer As String = String.Empty '_ClsReg.GetINIString(m_md.GetSelectedMDYearPath(m_InitialData.MDData.MDNr, Now.Year), "Debitoren", "FACTOSKDNr")
		MDkundennummer = m_md.GetSelectedMDProfilValue(m_InitialData.MDData.MDNr, Now.Year, "Debitoren", "FACTOSKDNr", String.Empty)
		Dim debitorenNr As String = "" ' KDNR
		Dim nameDebitor As String = "" ' R_Name1
		Dim buchungscode As String = "RA" ' Fix
		Dim währung As String = "" ' (Fix)
		Dim rgNr As String = "" ' RENR
		Dim rgDatum As String = "" ' FAK_DAT
		Dim betragBrutto As String = "" ' BetragInk
		Dim fälligkeit As String = "" ' Faellig
		Dim sk As String = "0" ' SK% Fix
		Dim skTage As String = "" ' Gemäss Mahncode FAK_Dat_Faellig

		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim cmdText As String = ""
		cmdText += String.Format("BEGIN TRY DROP TABLE [_KMUFACTORING_{0}] END TRY BEGIN CATCH END CATCH ",
														 m_common.GetLogedUserGuid)
		cmdText += "SELECT deb.*, @mdKundennummer as MDkundennummer, t.Mahn1 as SKTage, 'RA' as Buchungscode, '0' as SK "
		cmdText += String.Format("INTO [_KMUFACTORING_{0}] ", m_common.GetLogedUserGuid)
		cmdText += String.Format("FROM {0} deb ", ClsDataDetail.SPTabNamenDBL)
		cmdText += "LEFT JOIN Tab_Mahncode t ON "
		cmdText += "t.GetFeld = deb.Mahncode "
		cmdText += String.Format("SELECT * FROM [_KMUFACTORING_{0}]", m_common.GetLogedUserGuid)
		Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
		Dim pMDKundennummer As SqlParameter = New SqlParameter("@mdKundennummer", SqlDbType.NVarChar, 20)
		pMDKundennummer.Value = MDkundennummer
		cmd.Parameters.Add(pMDKundennummer)
		Dim dt As DataTable = New DataTable("Debitorenliste")
		Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
		da.Fill(dt)

		zeile = String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}",
																						strPipe _
																						, "Mandanten-Nr." _
																						, "Debitoren-Nr." _
																						, "Name Debitor" _
																						, "Buchungscode RA/RI/GS" _
																						, "Währung" _
																						, "Rg.-Nr." _
																						, "Rg.-Datum" _
																						, "Betrag brutto" _
																						, "Fälligkeit" _
																						, "SK%" _
																						, "SK Tage"
																						)
		lst.Add(zeile)

		If dt.Rows.Count > 0 Then
			For Each row As DataRow In dt.Rows
				debitorenNr = row("KDNR").ToString()
				nameDebitor = row("R_Name1").ToString()
				währung = row("Currency").ToString
				rgNr = row("RENR").ToString()
				rgDatum = DateTime.Parse(row("FAK_DAT").ToString).ToShortDateString
				betragBrutto = String.Format("{0:0.00}", Decimal.Parse(row("BetragInk").ToString))
				fälligkeit = DateTime.Parse(row("Faellig").ToString).ToShortDateString
				skTage = row("SKTage").ToString()

				zeile = String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}",
																						strPipe _
																						, MDkundennummer _
																						, debitorenNr _
																						, nameDebitor _
																						, buchungscode _
																						, währung _
																						, rgNr _
																						, rgDatum _
																						, betragBrutto _
																						, fälligkeit _
																						, sk _
																						, skTage
																						)
				lst.Add(zeile)
			Next
		End If

		Return lst
	End Function

	''' <summary>
	''' Daten fürs Drucken bereit stellen.
	''' </summary>
	''' <param name="bForDesign">ob Designer gestartet werden soll.</param>
	''' <param name="bForExport">ob die Liste für Export ist.</param>
	''' <param name="strJobInfo">Der JobNr von DocPrint Datenbank</param>
	''' <remarks></remarks>
	Sub GetData4Print(ByVal bForDesign As Boolean, ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim iKDNr As Integer = 0
		Dim iKDZNr As Integer = 0
		Dim bResult As Boolean = True
		Dim bWithKD As Boolean = True

		Dim sSql As String = ""

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim cmd As System.Data.SqlClient.SqlCommand

		Try
			Conn.Open()
			'Daten sind bereits auf der Datenbank
			GetDataForFactoring() ' Zuerst müssen die Daten aufbereitet werden
			sSql = String.Format("SELECT * FROM [_KMUFACTORING_{0}]", m_common.GetLogedUserGuid)

			If bForDesign Then sSql = Replace(sSql, "SELECT", "SELECT TOP 10 ", 1, 1)

			cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)

			Dim rFoundedRec2Print As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank
			Try
				If Not rFoundedRec2Print.HasRows Then
					cmd.Dispose()
					rFoundedRec2Print.Close()

					MessageBox.Show("Ich konnte leider Keine Daten finden.", "GetData4Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
					Exit Sub
				End If

			Catch ex As NoNullAllowedException

			Catch ex As Exception
				MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "GetData4Print")

			End Try

			rFoundedRec2Print.Read()
			If rFoundedRec2Print.HasRows Then
				Me.SQL4Print = sSql
				Me.bPrintAsDesign = bForDesign
				Me.PrintJobNr = strJobInfo

				PrintListingThread = New Thread(AddressOf StartPrinting)
				PrintListingThread.Name = "PrintingVakListing"
				PrintListingThread.SetApartmentState(ApartmentState.STA)
				PrintListingThread.Start()

			End If

			'While rZGrec.Read

			'  'If LL.Core.Handle <> 0 Then LL.Core.LlJobClose()
			'  'LL.Core.LlJobOpen(2)

			'  If bForExport Then
			'    '            ExportLLDoc(LL, Me.LblChanged.Text, CShort(Me.CboFormat.SelectedIndex),CInt(Me.txtOfNr.Text), iKDNr, iKDZNr)
			'    '          bResult = ExportLLDocWithStorage(LL, Me.LblChanged.Text, CShort(Me.CboFormat.SelectedIndex), CInt(Me.txtOfNr.Text), iKDNr, iKDZNr)
			'  Else
			'    If bForDesign Then
			'      'ShowInDesign(LL, strJobInfo, rZGrec)

			'    Else
			'      'bResult = ShowInPrint(LL, strJobInfo, rZGrec)

			'    End If
			'  End If

			'If Not bResult Or bForDesign Then Exit While
			'End While
			rFoundedRec2Print.Close()

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetData4Print")

		Finally
			'cmd.Dispose() 10.02.2010
			Conn.Close()

		End Try

	End Sub

	Sub StartPrinting()
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLOPSearchPrintSetting With {.DbConnString2Open = m_InitialData.MDData.MDDbConn,
																																									 .SQL2Open = Me.SQL4Print,
																																									 .JobNr2Print = Me.PrintJobNr,
																																									.SelectedMDNr = m_InitialData.MDData.MDNr}
		Dim obj As New SPS.Listing.Print.Utility.OPSearchListing.ClsPrintOPSearchList(_Setting)
		obj.PrintOPExportList_Factoring(Me.bPrintAsDesign, ClsDataDetail.GetSortBez,
														New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilterBez4}))
	End Sub

	Private Sub SaveSettings()
		Dim fn As String = Me.txtFilename.Text
		Dim path As String = fn.Substring(0, fn.LastIndexOf(CChar("\")) + 1)
		Dim name As String = fn.Substring(fn.LastIndexOf(CChar("\")) + 1, fn.Length - path.Length)

		If name.Contains("_") Then
			Dim n As String = name.Substring(0, name.IndexOf(CChar("_")))
			Dim endung As String = name.Substring(name.LastIndexOf(CChar(".")), name.Length - name.LastIndexOf(CChar(".")))
			name = n & endung
		End If

		My.Settings.Filename4Factoring = name
		My.Settings.Filepath4Factoring = path

		My.Settings.Save()
	End Sub

	Private Sub StartTranslation()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

		Me.lblHeader1.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblHeader2.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader2.Text)
		Me.lblDatei.Text = m_Translate.GetSafeTranslationValue(Me.lblDatei.Text)

	End Sub

	Private Sub frmFactoring_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		m_md = New Mandant
		StartTranslation()

		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitialData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formstyle. {1}", strMethodeName, ex.Message))

		End Try

		CreatePrintPopupMenu()

	End Sub


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiPrint.DropDownControl, DevExpress.XtraBars.PopupMenu)

		popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim bShowDesign As Boolean = True
		Dim liMnu As New List(Of String) From {"Liste drucken#mnuListPrint",
																					 If(bShowDesign, "Entwurfansicht#PrintDesign", "")}

		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			'Me.bbiPrint.Visibility = BarItemVisibility.Always
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				bshowMnu = myValue(0).ToString <> String.Empty

				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

					If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetPrintMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetPrintMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

		Me.SQL4Print = String.Empty
		Me.bPrintAsDesign = False

		Select Case e.Item.Name.ToUpper
			Case "mnuListPrint".ToUpper
				GetData4Print(False, False, "7.16") ' Liste drucken

			Case "PrintDesign".ToUpper
				GetData4Print(True, False, "7.16") ' Entwurfsansicht


			Case Else
				Exit Sub

		End Select

	End Sub



End Class