
Imports System.IO
Imports System.Data.SqlClient
Imports System.Threading

Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.CommonSettings

Imports SPOPSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class frmCreditreform
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	'Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg

	Private m_md As New Mandant
	Private m_utility As New Utilities
	Private m_common As New CommonSetting

	Private _Trennzeichen As String
	Private _dMahnDate As Date
	Private _iMahnNr As Integer
	Private _docpath As String
	Private _docname As String
	Private _docnameFull As String
	Private _date As String

	Private PrintListingThread As Thread
	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean


#Region "Contructor"

	Public Sub New()

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

	End Sub

	Public Sub New(ByVal strDocname As String, ByVal iSelMahnNr As Integer, ByVal dSelMahnDate As Date)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		InitializeComponent()
		If strDocname = String.Empty Then strDocname = "CreditreformExport.csv"
		_iMahnNr = iSelMahnNr
		_dMahnDate = dSelMahnDate
		_Trennzeichen = ";"

		_docpath = m_utility.GetSpSREHomeFolder
		If My.Settings.Filepath4Creditreform.Length > 0 Then
			_docpath = My.Settings.Filepath4Creditreform
		End If
		If My.Settings.Filename4Creditreform.Length > 0 Then
			strDocname = My.Settings.Filename4Creditreform
		End If
		' Dokumentname vorbereiten
		Dim endung As String = ".csv"
		If strDocname.Contains(".") Then
			endung = strDocname.Substring(strDocname.LastIndexOf(CChar(".")), strDocname.Length - strDocname.LastIndexOf(CChar(".")))
		End If
		_date = String.Format("{0}{1:00}{2:00}", Date.Now.Year, Date.Now.Month, Date.Now.Day)
		_docname = strDocname
		_docnameFull = String.Format("{0}_{1}{2}", strDocname.Replace(endung, ""), _date, endung)

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
			IO.File.WriteAllLines(Me.txtFilename.Text, GetDataForCreditreform().ToArray, System.Text.Encoding.Default)
			MessageBox.Show(String.Format("Die Datei {0} wurde erfolgreich gespeichert.",
																		Me.txtFilename.Text), "Daten gespeichert",
																		MessageBoxButtons.OK, MessageBoxIcon.Information)
			Dim strDirectory As String = Path.GetDirectoryName(Me.txtFilename.Text)
			System.Diagnostics.Process.Start(strDirectory)

			SaveSettings()
			GetData4Print(False, False, "7.17")

		Catch ex As Exception
			MessageBox.Show(ex.Message, "Speicherung nicht durchgeführt", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	Private Function GetDataForCreditreform() As List(Of String)
		Dim lst As New List(Of String)
		Dim zeile As String = ""
		Dim belegDatum As String = ""
		Dim belegTyp As String = ""
		Dim belegNr As String = ""
		Dim debitorenNr As String = ""
		Dim währung As String = ""
		Dim betrag As String = ""
		Dim beschreibung As String = ""
		Dim fälligkeitsDatum As String = ""
		Dim skontoDatum As String = ""
		Dim skonto As String = ""
		Dim ausgleichMitBelegart As String = ""
		Dim ausgleichMitBelegNr As String = ""

		zeile = String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}" _
																		, _Trennzeichen _
																		, "Auszugsdatum" _
																		, "Debitornummer" _
																		, "Debitor Nachname" _
																		, "Debitor Vorname" _
																		, "Strasse" _
																		, "Ort" _
																		, "PLZ" _
																		, "Belegnummer" _
																		, "Zahlungskondition" _
																		, "Belegdatum" _
																		, "Nettofällig" _
																		, "Betrag" _
																		, "M-Stufe")
		lst.Add(zeile)
		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		' Anpassung SQL 21.03.2011
		Dim cmdText As String = ""
		cmdText += String.Format("BEGIN TRY DROP TABLE [_CRF_{0}] END TRY BEGIN CATCH END CATCH ",
														 m_common.GetLogedUserGuid) ' ClsDataDetail.SPTabNamenDBL)
		cmdText += "SELECT deb.*, IsNull(zk.Description,'') as SkontoProzCr "
		cmdText += String.Format("INTO [_CRF_{0}] ", m_common.GetLogedUserGuid)
		cmdText += String.Format("FROM {0} deb ", ClsDataDetail.SPTabNamenDBL)
		cmdText += "LEFT JOIN RE ON "
		cmdText += "RE.RENR = deb.RENR And "
		cmdText += "RE.Zahlkond <> '' "
		cmdText += "LEFT JOIN TAB_ZahlKond zk ON "
		cmdText += "zk.GetFeld = RE.Zahlkond "
		cmdText += String.Format("SELECT * FROM [_CRF_{0}]", m_common.GetLogedUserGuid)
		Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
		Dim dt As DataTable = New DataTable("Debitorenliste")
		Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
		Dim dMyMahnDate As Date = _dMahnDate
		Dim iChangedMahnNr As Integer = _iMahnNr

		da.Fill(dt)
		If dt.Rows.Count > 0 Then
			For Each row As DataRow In dt.Rows
				belegDatum = DateTime.Parse(row("FAK_DAT").ToString).ToShortDateString
				If CInt(row("RENr")) = 20726 Then
					Trace.WriteLine(row("RENr"))
				End If
				If _iMahnNr > 3 Then
					If ClsDataDetail.GetColumnTextStr(row, "ma3", "") = Format(_dMahnDate, "d") Then
						iChangedMahnNr = 3
					Else
						If ClsDataDetail.GetColumnTextStr(row, "ma2", "") = Format(_dMahnDate, "d") Then
							iChangedMahnNr = 2
						Else
							If ClsDataDetail.GetColumnTextStr(row, "ma1", "") = Format(_dMahnDate, "d") Then
								iChangedMahnNr = 2
							Else
								If ClsDataDetail.GetColumnTextStr(row, "ma0", "") = Format(_dMahnDate, "d") Then
									iChangedMahnNr = 0
								End If
							End If
						End If

					End If
				Else
					If Year(_dMahnDate) = 1900 Then _dMahnDate = CDate(ClsDataDetail.GetColumnTextStr(row,
																															String.Format("ma{0}", _iMahnNr), ""))
				End If

				zeile = String.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}{9}{0}{10}{0}{11}{0}{12}{0}{13}" _
																						, _Trennzeichen _
																						, Format(_dMahnDate, "d") _
																						, row("KDNr") _
																						, row("R_Name1") _
																						, row("R_Name2") _
																						, row("R_Strasse") _
																						, row("R_Ort") _
																						, row("R_PLZ") _
																						, row("RENr") _
																						, row("ZahlKond") _
																						, Format(row("FAK_DAT"), "d") _
																						, Format(row("Faellig"), "d") _
																						, row("Betragink") _
																						, iChangedMahnNr)
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
			GetDataForCreditreform() ' Zuerst müssen die Daten aufbereitet werden
			sSql = String.Format("SELECT * FROM [_CRF_{0}]", m_common.GetLogedUserGuid)

			If bForDesign Then sSql = Replace(sSql, "SELECT", "SELECT TOP 10 ", 1, 1)

			cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)

			Dim rFoundedRec2Print As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank
			Try
				If Not rFoundedRec2Print.HasRows Then
					cmd.Dispose()
					rFoundedRec2Print.Close()

					MessageBox.Show("Ich konnte leider Keine Daten finden.", "GetData4Data",
													MessageBoxButtons.OK, MessageBoxIcon.Information)
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
																																									.SelectedMDNr = m_InitialData.MDData.MDNr,
																																									.LogedUSNr = m_InitialData.UserData.UserNr}
		Dim obj As New SPS.Listing.Print.Utility.OPSearchListing.ClsPrintOPSearchList(_Setting)
		obj.PrintOPExportList_Creditreform(Me.bPrintAsDesign, ClsDataDetail.GetSortBez,
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

		My.Settings.Filename4Creditreform = name
		My.Settings.Filepath4Creditreform = path

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

	Private Sub frmCreditreform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

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
				GetData4Print(False, False, "7.17") ' Liste drucken

			Case "PrintDesign".ToUpper
				GetData4Print(True, False, "7.17") ' Liste drucken


			Case Else
				Exit Sub

		End Select

	End Sub


End Class