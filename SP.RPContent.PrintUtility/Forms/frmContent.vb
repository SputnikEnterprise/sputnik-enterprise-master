
Option Strict Off

Imports System.IO
Imports DevComponents.DotNetBar.Metro.ColorTables
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors

Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports System.Threading
Imports DevExpress.XtraBars
Imports DevComponents.DotNetBar

Imports SP.RPContent.PrintUtility.FillRPContent.ClsFillRPContent
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel


Public Class frmContent
  Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private _RPCSetting As New ClsRPCSetting
  Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Private _ClsFunc As New ClsDivFunc
  Private m_xml As New ClsXML
  Private m_md As Mandant

  Private Property SelectedYear2Print As Integer

  Private Property PrintJobNr As String
  Private Property SQL4Print As String

  Private Property SelectedESNr As New List(Of Integer)
  Private Property SelectedMANr As New List(Of Integer)
  Private Property SelectedKDNr As New List(Of Integer)
  Private Property SelectedKDZHDNr As New List(Of Integer)
  Private Property SelectedData2WOS As New List(Of Boolean)
  Private Property SelectedMALang As New List(Of String)



#Region "Contructor"

  Public Sub New(ByVal _setting As ClsRPCSetting)

    ' Dieser Aufruf ist für den Designer erforderlich.
    DevExpress.UserSkins.BonusSkins.Register()
    DevExpress.Skins.SkinManager.EnableFormSkins()

    InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Me._RPCSetting = _setting

		bbiSetting.Visibility = BarItemVisibility.Never
		ResetMandantenDropDown()
    LoadMandantenDropDown()

  End Sub


#End Region


#Region "Lookup Edit Reset und Load..."

  ''' <summary>
  ''' Resets the Mandanten drop down.
  ''' </summary>
  Private Sub ResetMandantenDropDown()

    lueMandant.Properties.DisplayMember = "MDName"
    lueMandant.Properties.ValueMember = "MDNr"

    Dim columns = lueMandant.Properties.Columns
    columns.Clear()
    columns.Add(New LookUpColumnInfo With {.FieldName = "MDName",
                                           .Width = 100,
                                           .Caption = "Mandant"})

    lueMandant.Properties.ShowHeader = False
    lueMandant.Properties.ShowFooter = False

    lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
    lueMandant.Properties.SearchMode = SearchMode.AutoComplete
    lueMandant.Properties.AutoSearchColumnIndex = 0

    lueMandant.Properties.NullText = String.Empty
    lueMandant.EditValue = Nothing

  End Sub

  ''' <summary>
  ''' Load Mandanten drop down
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub LoadMandantenDropDown()
    Dim _ClsFunc As New ClsDbFunc
    Dim Data = _ClsFunc.LoadMandantenData()

    lueMandant.Properties.DataSource = Data
    lueMandant.Properties.ForceInitialize()

    '    Return Not Data Is Nothing
  End Sub

  ' Mandantendaten...
  Private Sub lueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged
    Dim SelectedData As MandantenData = TryCast(Me.lueMandant.GetSelectedDataRow(), MandantenData)
    If Not SelectedData Is Nothing Then
      ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(Me.lueMandant.EditValue)
      ClsDataDetail.UserData = ClsDataDetail.LogededUSData(Me.lueMandant.EditValue, ClsDataDetail.UserData.UserLName, ClsDataDetail.UserData.UserFName)

    Else
      ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
      ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, ClsDataDetail.UserData.UserNr)

    End If

    Me.bbiSearch.Enabled = Not (ClsDataDetail.MDData Is Nothing)
    Me.bbiPrint.Enabled = Not (ClsDataDetail.MDData Is Nothing)
    Me.bbiExport.Enabled = Not (ClsDataDetail.MDData Is Nothing)

  End Sub


#End Region

  Private Sub sbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sbClose.Click
    Me.Dispose()
  End Sub

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount

		Me.Text = m_xml.GetSafeTranslationValue(Me.Text)

		Me.lblHeader1.Text = m_xml.GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblHeader2.Text = m_xml.GetSafeTranslationValue(Me.lblHeader2.Text)
		Me.sbClose.Text = m_xml.GetSafeTranslationValue(Me.sbClose.Text)

		Me.bbiSearch.Caption = m_xml.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiPrint.Caption = m_xml.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_xml.GetSafeTranslationValue(Me.bbiExport.Caption)
		Me.bbiSetting.Caption = m_xml.GetSafeTranslationValue(Me.bbiSetting.Caption)
		Me.bbiExport.Caption = m_xml.GetSafeTranslationValue(Me.bbiExport.Caption)

		Me.grpSuchkriterien.Text = m_xml.GetSafeTranslationValue(Me.grpSuchkriterien.Text)
		Me.lblMDName.Text = m_xml.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.lblDetails.Text = m_xml.GetSafeTranslationValue(Me.lblDetails.Text)
		Me.lblJahr.Text = m_xml.GetSafeTranslationValue(Me.lblJahr.Text)
		Me.lblMonat.Text = m_xml.GetSafeTranslationValue(Me.lblMonat.Text)
		Me.lblKanton.Text = m_xml.GetSafeTranslationValue(Me.lblKanton.Text)
		Me.lblPVLBeruf.Text = m_xml.GetSafeTranslationValue(Me.lblPVLBeruf.Text)

	End Sub

	Private Sub frmRPContent_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.iHeight = Me.Height
			My.Settings.iWidth = Me.Width
			My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

			My.Settings.Save()
		End If

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmRPContent_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		m_md = New Mandant

		Try
			' Rapporte drucken
			If Not IsUserActionAllowed(0, 303) Then
				Me.bbiPrint.Visibility = BarItemVisibility.Never
				Me.bbiExport.Visibility = BarItemVisibility.Never
			End If
			If _ClsProgSetting.GetLogedUSNr <> 1 And Not IsUserAllowed4DocExport("1.3.3") Then Me.bbiExport.Visibility = BarItemVisibility.Never

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Benutzerrechte:{1}", strMethodeName, ex.Message))
			' Alles sperren...
			Me.bbiPrint.Visibility = BarItemVisibility.Never
			Me.bbiExport.Visibility = BarItemVisibility.Never

		End Try

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If
			StyleManager.MetroColorGeneratorParameters = New MetroColorGeneratorParameters(_RPCSetting.MetroForeColor,
																																				 _RPCSetting.MetroBorderColor)

			Try
				If My.Settings.iHeight > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.iHeight)
				If My.Settings.iWidth > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.iWidth)
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
			Dim UpdateDelegate As New MethodInvoker(AddressOf TranslateControls)
			Me.Invoke(UpdateDelegate)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.FormStyle: {1}", strMethodeName, ex.Message))

		End Try
		Try
			Me.lueMandant.EditValue = ClsDataDetail.ProgSettingData.SelectedMDNr
			Me.lueMandant.Visible = IsUserActionAllowed(ClsDataDetail.ProgSettingData.LogedUSNr, 642, ClsDataDetail.ProgSettingData.SelectedMDNr)
			Me.lblMDName.Visible = Me.lueMandant.Visible

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.Message))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

		Dim strrpNr As String = String.Empty
		For i As Integer = 0 To _RPCSetting.SelectedRPNr.Count - 1
			strrpNr &= If(Not String.IsNullOrWhiteSpace(strrpNr), ",", "") & _RPCSetting.SelectedRPNr(i)
		Next

		Me.cbo_Month.Text = If(_RPCSetting.SelectedMonth.Count = 0, Now.Month, _RPCSetting.SelectedMonth(0))
		Me.cbo_Year.Text = If(_RPCSetting.SelectedYear.Count = 0, Now.Year, _RPCSetting.SelectedYear(0))

		Me.LblTimeValue.Visible = CBool(CInt(_ClsProgSetting.GetLogedUSNr().ToString) = 1)
		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Bereit")

	End Sub


#Region "Funktionen für Reset der Controls..."

	Sub BlankFields()
		ResetAllTabEntries()

		Me.bsiInfo.Caption = m_xml.GetSafeTranslationValue("Bereit")

	End Sub


	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetAllTabEntries()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			For Each ctrls As Control In Me.Controls
				ResetControl(ctrls)
			Next

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			'_ClsErrException.MessageBoxShowError(_ClsProgSetting.GetLogedUSNr, "ResetControl", ex)
		End Try

	End Sub

	Private Sub ResetControl(ByVal con As Control)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			' Rekursiver Aufruf
			If con.HasChildren Then
				For Each childCon As Control In con.Controls
					ResetControl(childCon)
				Next
			Else
				' Sonst Control zurücksetzen
				If TypeOf (con) Is TextBox Then
					Dim tb As TextBox = con
					tb.Text = String.Empty

				ElseIf TypeOf (con) Is System.Windows.Forms.ComboBox Or TypeOf (con) Is ComboBoxEdit Or TypeOf (con) Is CheckedComboBoxEdit Then
					Dim cbo As System.Windows.Forms.ComboBox = con
					cbo.Text = String.Empty
					cbo.SelectedIndex = -1

				ElseIf TypeOf (con) Is ListBox Then
					Dim lst As ListBox = con
					lst.Items.Clear()

				End If
			End If

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub


#End Region


	Sub FillFieldsWithDefaults()

		BlankFields()
		Me.cbo_Month.Text = If(Now.Day < 15, Now.Month - 1, Now.Month)
		Me.cbo_Year.Text = If(Now.Day < 15 And Now.Month = 1, Now.Year - 1, Now.Year)

	End Sub

	Private Sub cbo_RPNr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles cbo_RPNr.ButtonClick
		Dim liMonth As New List(Of Short)
		Dim liYear As New List(Of Integer)
		Dim aMonth As String() = Me.cbo_Month.Text.Split(CChar(","))
		Dim aYear As String() = Me.cbo_Year.Text.Split(CChar(","))

		For i As Integer = 0 To aMonth.Length - 1
			liMonth.Add(CShort(aMonth(i)))
		Next
		For i As Integer = 0 To aYear.Length - 1
			liYear.Add(CShort(aYear(i)))
		Next

		Dim _setting As New ClsPopupSetting With {.SearchMonth = liMonth, .SearchYear = liYear, .SearchPVLBez = Me.cbo_PVLBez.Text, .SearchedField = "Rapport-Nr."}

		Dim frmTest As New frmSearchRec(_setting)
		ClsDataDetail.strButtonValue = Me.cbo_RPNr.Text

		Dim m As String = String.Empty
		m = frmTest.ShowDialog()
		m = frmTest.GetSelectedValues

		frmTest.MdiParent = Me.MdiParent

		'm = frmTest.iMyValue(ClsDataDetail.strButtonValue)
		Me.cbo_RPNr.Text = If(m = Nothing, String.Empty, CStr(m.ToString))
		frmTest.Dispose()

	End Sub

	Private Sub cbo_PVLNr_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_PVLBez.QueryPopUp
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim liMonth As New List(Of Short)
		Dim liYear As New List(Of Integer)
		Dim liRPNr As New List(Of Integer)
		Dim aMonth As String() = Me.cbo_Month.Text.Split(CChar(","))
		Dim aYear As String() = Me.cbo_Year.Text.Split(CChar(","))
		Dim aRPNr As String() = Me.cbo_RPNr.Text.Split(CChar(","))

		For i As Integer = 0 To aMonth.Length - 1
			liMonth.Add(CShort(aMonth(i)))
		Next
		For i As Integer = 0 To aYear.Length - 1
			liYear.Add(CShort(aYear(i)))
		Next
		For i As Integer = 0 To aRPNr.Length - 1
			liRPNr.Add(CShort(Val(aRPNr(i))))
		Next

		Dim _setting As New ClsPopupSetting With {.SearchMonth = liMonth,
																							.SearchYear = liYear,
																							.SearchRPNr = liRPNr,
																							.SearchPVLKanton = Me.cbo_PVLKanton.Text,
																							.cbo2Fill = cbo_PVLBez}

		Try

			Try
				ListPVLBez(_setting)


			Catch ex As Exception ' Manager
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		Finally
			Me.Cursor = Cursors.Default

		End Try

	End Sub

	Private Sub cbo_PVLKanton_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_PVLKanton.QueryPopUp
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim liMonth As New List(Of Short)
		Dim liYear As New List(Of Integer)
		Dim liRPNr As New List(Of Integer)
		Dim aMonth As String() = Me.cbo_Month.Text.Split(CChar(","))
		Dim aYear As String() = Me.cbo_Year.Text.Split(CChar(","))
		Dim aRPNr As String() = Me.cbo_RPNr.Text.Split(CChar(","))

		For i As Integer = 0 To aMonth.Length - 1
			liMonth.Add(CShort(aMonth(i)))
		Next
		For i As Integer = 0 To aYear.Length - 1
			liYear.Add(CShort(aYear(i)))
		Next
		For i As Integer = 0 To aRPNr.Length - 1
			liRPNr.Add(CInt(Val(aRPNr(i))))
		Next

		Dim _setting As New ClsPopupSetting With {.SearchMonth = liMonth,
																							.SearchYear = liYear,
																							.SearchRPNr = liRPNr,
																							.SearchPVLKanton = Me.cbo_PVLKanton.Text,
																							.cbo2Fill = cbo_PVLKanton}

		Try

			Try
				ListPVLKanton(_setting)


			Catch ex As Exception ' Manager
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		Finally
      Me.Cursor = Cursors.Default

    End Try

  End Sub

  Private Sub cbo_Month_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles cbo_Month.KeyPress, cbo_Year.KeyPress, cbo_PVLKanton.KeyPress

    Select Case Asc(e.KeyChar)
      Case 48 To 57, 8, 32
        ' Zahlen, Backspace und Space zulassen

      Case Else
        ' alle anderen Eingaben unterdrücken
        e.Handled = True
    End Select

  End Sub



#Region "Listview Funktionen..."

  Private Sub ListViewEx1_ColumnWidthChanged(sender As Object, e As System.Windows.Forms.ColumnWidthChangedEventArgs) Handles lvDetails.ColumnWidthChanged
    Dim strColInfo As String = String.Empty
    Dim strColAlign As String = String.Empty

    For i As Integer = 0 To lvDetails.Columns.Count - 1
      If lvDetails.Columns.Item(i).TextAlign = HorizontalAlignment.Center Then
        strColAlign = "2"

      ElseIf lvDetails.Columns.Item(i).TextAlign = HorizontalAlignment.Right Then
        strColAlign = "1"
      Else
        strColAlign = "0"
      End If

      strColInfo &= CStr(IIf(strColInfo = String.Empty, "", ";")) & (lvDetails.Columns.Item(i).Width) & "-" & strColAlign
    Next
    My.Settings.LV_ColumnWidth = strColInfo
    My.Settings.Save()

  End Sub

  Private Sub ListViewEx1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles lvDetails.SelectedIndexChanged
    Dim bSelected As Boolean = False

    For i As Integer = 0 To lvDetails.Items.Count - 1
      If lvDetails.Items(i).Selected = True Then
        'Me.LblRecID.Text = Me.LvFoundedrecs.Items(i).SubItems(0).Text
        'Me.LblRecNr.Text = Me.LvFoundedrecs.Items(i).SubItems(1).Text

        'DisplayFoundedData(Me, Me.LblRecID.Text)
        bSelected = True
        Exit For
      End If

    Next

    Me.bbiPrint.Enabled = bSelected
    Me.bbiExport.Enabled = bSelected

  End Sub

#End Region


  Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
    Dim liRPnr As String() = Me.cbo_RPNr.Text.Split(CChar(","))
    Dim aYear As String() = Me.cbo_Year.Text.Split(CChar(","))
    Dim aMonth As String() = Me.cbo_Month.Text.Split(CChar(","))

    If Not _RPCSetting.SelectedYear Is Nothing Then _RPCSetting.SelectedYear.Clear()
    If Not _RPCSetting.SelectedMonth Is Nothing Then _RPCSetting.SelectedMonth.Clear()
    If Not _RPCSetting.SelectedRPNr Is Nothing Then _RPCSetting.SelectedRPNr.Clear()
    _RPCSetting.FoundedRPNr.Clear()

    For i As Integer = 0 To aYear.Length - 1
      If CInt(Val(aYear(i))) <> 0 Then
        _RPCSetting.SelectedYear.Add(CInt(Val(aYear(i))))
      End If
    Next
    For i As Integer = 0 To aMonth.Length - 1
      If CInt(Val(aMonth(i))) <> 0 Then
        _RPCSetting.SelectedMonth.Add(CInt(Val(aMonth(i))))
      End If
    Next
    For i As Integer = 0 To liRPnr.Count - 1
      _RPCSetting.SelectedRPNr.Add(CInt(Val(liRPnr(i))))
    Next

    _RPCSetting.SelectedPVLBez = Me.cbo_PVLBez.Text
    _RPCSetting.SelectedKanton = Me.cbo_PVLKanton.Text
    _RPCSetting.PrintFerFeiertage = Me.chkPrintFerFeier.Checked
    _RPCSetting.lv2Fill = Me.lvDetails

    ListFoundedrec(_RPCSetting)

    If Me.lvDetails.Items.Count > 1 Then
      Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensätze wurden gefunden."), Me.lvDetails.Items.Count)

    Else
      Me.bsiInfo.Caption = String.Format(m_xml.GetSafeTranslationValue("{0} Datensatz wurde gefunden."), Me.lvDetails.Items.Count)
    End If

    'For i As Integer = 0 To Me.lvDetails.Items.Count - 1
    '  lvDetails.Items(i).Selected = True
    'Next i
    Me.bbiPrint.Enabled = Me.lvDetails.SelectedItems.Count > 0
    Me.bbiExport.Enabled = Me.lvDetails.SelectedItems.Count > 0

  End Sub

  Private Sub bbiSetting_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSetting.ItemClick
    Dim frm As New frmRPCPrintSetting

    frm.Top = (Me.Top + Me.Height) - frm.Height - 50
    frm.Left = (Me.Left + Me.Width) - frm.Width - 50

    frm.Show()

  End Sub

#Region "Exportieren..."

  Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick

    _RPCSetting.FoundedRPNr.Clear()
    _RPCSetting.PrintCreatedPDFFile = False
    For Each item As ListViewItem In lvDetails.SelectedItems()
      _RPCSetting.FoundedRPNr.Add(Val(item.SubItems(1).Text))
    Next
    _RPCSetting.ShowMessage = True

    If _RPCSetting.FoundedRPNr.Count > 0 Then
      Dim _clsPDFFill As New FillRPContent.ClsFillRPContent(_frm:=Me, _
                                                            _setting:=_RPCSetting)
      _clsPDFFill.StartFillingRPCFile()

    Else
      Dim strMsg As String = String.Format(m_xml.GetSafeTranslationValue("Sie haben keine Daten ausgewählt."), vbNewLine)
      DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, _
                                                 m_xml.GetSafeTranslationValue("Rapportdaten drucken"), _
                                                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)

    End If

  End Sub


#End Region


  Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick

    _RPCSetting.FoundedRPNr.Clear()
    _RPCSetting.PrintCreatedPDFFile = True
    For Each item As ListViewItem In lvDetails.SelectedItems()
      _RPCSetting.FoundedRPNr.Add(Val(item.SubItems(1).Text))
    Next
    _RPCSetting.ShowMessage = True

    If _RPCSetting.FoundedRPNr.Count > 0 Then
      Dim _clsPDFFill As New FillRPContent.ClsFillRPContent(_frm:=Me, _
                                                            _setting:=_RPCSetting)
      _clsPDFFill.StartFillingRPCFile()

    Else
      Dim strMsg As String = String.Format(m_xml.GetSafeTranslationValue("Sie haben keine Daten ausgewählt."), vbNewLine)
      DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, _
                                                 m_xml.GetSafeTranslationValue("Rapportdaten drucken"), _
                                                MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)

    End If

  End Sub

  Private Sub cbo_Year_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_Year.QueryPopUp
    ListLOYear(sender)
  End Sub

  Private Sub cbo_Month_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_Month.QueryPopUp
    ListLOMonth(sender, Me.cbo_Year.Text)
  End Sub



  Private Sub cbo_Year_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbo_Year.SelectedIndexChanged

  End Sub

  Private Sub cbo_PVLKanton_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbo_PVLKanton.SelectedIndexChanged

  End Sub

  Private Sub cbo_PVLBez_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbo_PVLBez.SelectedIndexChanged

  End Sub

  Private Sub cbo_RPNr_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cbo_RPNr.SelectedIndexChanged

  End Sub
End Class

