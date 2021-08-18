
Option Strict Off

Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging


Public Class frmQSTAddress
  Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_common As New CommonSetting
	Private m_md As New Mandant
	Private _ClsSetting As New ClsSetting


#Region "Constructor"

	Public Sub New(ByVal _setting As ClsSetting)

    ' Dieser Aufruf ist für den Designer erforderlich.
    DevExpress.UserSkins.BonusSkins.Register()
    DevExpress.Skins.SkinManager.EnableFormSkins()

    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Me._ClsSetting = _setting

  End Sub

#End Region



#Region "Lb clicks 1. Seite..."

  'Private Sub LibCountry_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LibCountry.LinkClicked
  '  Dim frmTest As New frmSearchRec
  '  Dim i As Integer = 0

  '  _ClsFunc.Get4What = "Land"
  '  ClsDataDetail.strButtonValue = "Land"
  '  ClsDataDetail.Get4What = "Land"

  '  frmTest.ShowDialog()
  '  frmTest.MdiParent = Me.MdiParent

  '  If ClsDataDetail.strButtonValue <> String.Empty Then
  '    Dim m As String
  '    m = frmTest.iKDValue(_ClsFunc.Get4What)
  '    If m.ToString <> String.Empty Then
  '      Me.txt_Land.Text = m.ToString
  '    End If
  '  End If
  '  frmTest.Dispose()

  'End Sub

  'Private Sub LibPLZ_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LibPLZ.LinkClicked
  '  Dim frmTest As New frmSearchRec
  '  Dim i As Integer = 0

  '  _ClsFunc.Get4What = "PLZ"
  '  ClsDataDetail.strButtonValue = "PLZ"
  '  ClsDataDetail.Get4What = "PLZ"

  '  frmTest.ShowDialog()
  '  frmTest.MdiParent = Me.MdiParent

  '  If ClsDataDetail.strButtonValue <> String.Empty Then
  '    Dim m As String
  '    m = frmTest.iKDValue(_ClsFunc.Get4What)
  '    If m.ToString <> String.Empty Then
  '      Me.txt_PLZ.Text = m.ToString
  '    End If
  '  End If
  '  frmTest.Dispose()

  'End Sub

#End Region


  Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
    Me.Dispose()
  End Sub

  Private Sub frmQstAddress_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

    Me.Timer1.Enabled = False

    Try
      If Not Me.WindowState = FormWindowState.Minimized Then
        My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
        My.Settings.iWidth = Me.Width
        My.Settings.iHeight = Me.Height
        My.Settings.Save()
      End If

    Catch ex As Exception
      ' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
    End Try

  End Sub

  Private Sub frm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    Me.Timer1.Enabled = False
  End Sub

  Sub TranslateForm()

    Me.Text = GetSafeTranslationValue(Me.Text)

    Me.CmdClose.Text = GetSafeTranslationValue(Me.CmdClose.Text)

    Me.lblHeader1.Text = GetSafeTranslationValue(Me.lblHeader1.Text)
    Me.lblHeader2.Text = GetSafeTranslationValue(Me.lblHeader2.Text)

    Me.lblkanton.Text = GetSafeTranslationValue(Me.lblkanton.Text)
    Me.lblgemeinde.Text = GetSafeTranslationValue(Me.lblgemeinde.Text)
    Me.lbladresse.Text = GetSafeTranslationValue(Me.lbladresse.Text)
    Me.lblzusatz.Text = GetSafeTranslationValue(Me.lblzusatz.Text)
    Me.lblzhd.Text = GetSafeTranslationValue(Me.lblzhd.Text)
    Me.lblpostfach.Text = GetSafeTranslationValue(Me.lblpostfach.Text)
    Me.lblstrasse.Text = GetSafeTranslationValue(Me.lblstrasse.Text)

    Me.lblland.Text = GetSafeTranslationValue(Me.lblland.Text)
    Me.lblort.Text = GetSafeTranslationValue(Me.lblort.Text)
    Me.lblplz.Text = GetSafeTranslationValue(Me.lblplz.Text)
    Me.lblstartnummer.Text = GetSafeTranslationValue(Me.lblstartnummer.Text)

    Me.lbldetail.Text = GetSafeTranslationValue(Me.lbldetail.Text)

    Me.bbiDelete.Caption = GetSafeTranslationValue(Me.bbiDelete.Caption)
    Me.bbiNew.Caption = GetSafeTranslationValue(Me.bbiNew.Caption)
    Me.bbiSave.Caption = GetSafeTranslationValue(Me.bbiSave.Caption)
    Me.bsiInfo.Caption = GetSafeTranslationValue(Me.bsiInfo.Caption)

  End Sub

  ''' <summary>
  ''' Starten von Anwendung.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub frmQstAddress_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Try
      Dim time1 As New Stopwatch
      time1.Start()
      TranslateForm()
      time1.Stop()
      Trace.WriteLine(String.Format("1. Zeit: {0}", time1.Elapsed))

    Catch ex As Exception

    End Try

    Try
      Me.KeyPreview = True
      Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
      If strStyleName <> String.Empty Then
        UserLookAndFeel.Default.SetSkinStyle(strStyleName)
      End If

    Catch ex As Exception
      m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

    End Try

    Try
      If My.Settings.frmLocation <> String.Empty Then
        Me.Width = Math.Max(My.Settings.iWidth, Me.Width)
        Me.Height = Math.Max(My.Settings.iHeight, Me.Height)
        Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))

        If Screen.AllScreens.Length = 1 Then
          If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
        End If
        Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
      End If

    Catch ex As Exception
      m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

    End Try

    Me.LblTimeValue.Visible = CBool(CInt(ClsDataDetail.ProgSettingData.LogedUSNr) = 1)
    Me.SetLvwHeader()
    FillFoundedData(Me.LvFoundedrecs, "")


  End Sub

  Private Sub frmESSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    If e.KeyCode = Keys.F12 And m_common.GetLogedUserNr = 1 Then
      Dim strRAssembly As String = ""
      Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
      For Each a In AppDomain.CurrentDomain.GetAssemblies()
        strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
      Next
      strMsg = String.Format(strMsg, vbNewLine, _
                             GetExecutingAssembly().FullName, _
                             GetExecutingAssembly().Location, _
                             strRAssembly)
      DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End If
  End Sub


#Region "Funktionen zur Menüaufbau..."

  Private Sub bbiSave_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
    Dim _ClsFuncDb As New ClsDbFunc

    _ClsFuncDb.QSTKanton = Me.Cbo_Kanton.Text
    _ClsFuncDb.QSTGemeinde = Me.txt_Gemeinde.Text
    _ClsFuncDb.QSTAderesse1 = Me.txt_Adresse.Text
    _ClsFuncDb.QSTZusatz = Me.txt_Zusatz.Text
    _ClsFuncDb.QSTZHD = Me.txt_ZHD.Text
    _ClsFuncDb.QSTPostfach = Me.txt_Postfach.Text
    _ClsFuncDb.QSTStrasse = Me.txt_Strasse.Text
    _ClsFuncDb.QSTLand = Me.txt_Land.Text
    _ClsFuncDb.QSTPlz = Me.txt_PLZ.Text
    _ClsFuncDb.QSTOrt = Me.txt_Ort.Text
    _ClsFuncDb.QSTStammNr = Me.txt_StammNr.Text
    _ClsFuncDb.QSTProvision = CDbl(Val(Me.txt_Provision.Text))
    _ClsFuncDb.QSTBemerkung = Me.txt_Bemerkung.Text

    _ClsFuncDb.SaveDataToQSTDb(CInt(Me.LblRecNr.Text))

    If Me.LblRecNr.Text <> 0 Then
      For i As Integer = 0 To LvFoundedrecs.Items.Count - 1
        If LvFoundedrecs.Items(i).Selected Then
          Me.LvFoundedrecs.Items(i).SubItems(1).Text = Me.Cbo_Kanton.Text
          Me.LvFoundedrecs.Items(i).SubItems(2).Text = Me.txt_Gemeinde.Text
          Me.LvFoundedrecs.Items(i).SubItems(3).Text = Me.txt_Adresse.Text
          Me.LvFoundedrecs.Items(i).SubItems(4).Text = Me.txt_PLZ.Text & " " & Me.txt_Ort.Text
          Me.LvFoundedrecs.Items(i).SubItems(5).Text = Format(CDbl(Me.txt_Provision.Text), "n4")

          Exit For
        End If

      Next

    Else
      FillFoundedData(Me.LvFoundedrecs, "")

    End If
    Me.LblRecNr.Text = CInt(_ClsFuncDb.QSTRecNr)

  End Sub

  ''' <summary>
  ''' Funktion für das Leeren der Felder...
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub bbiNew_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiNew.ItemClick
    Dim cControl As Control
    Dim strText As String = Me.Cbo_Kanton.Text

    Me.bsiInfo.Caption = GetSafeTranslationValue("Bereit")

    ' Cbo leeren...
    For Each cControl In Me.Controls
      If TypeOf (cControl) Is DevExpress.XtraEditors.TextEdit Then
        cControl.Text = String.Empty
      End If

      If TypeOf (cControl) Is DevExpress.XtraEditors.ComboBoxEdit Then
        cControl.Text = String.Empty
      End If

      Console.WriteLine(cControl.Name)

    Next cControl
    Me.LblRecInfo.Text = GetSafeTranslationValue("Keine Datensätze ausgewählt...")
    Me.txt_Land.Text = "CH"
    Me.LblRecNr.Text = 0
    Me.Cbo_Kanton.Focus()

  End Sub


#End Region



#Region "Sonstige Funktionen..."

  Sub SetLvwHeader()
    Dim strColumnString As String = String.Empty
    Dim strColumnWidthInfo As String = String.Empty

    strColumnString = "RecNr;Kanton;Gemeinde;Adresse;PLZ, Ort;Provision"
    strColumnWidthInfo = "0-0;50-0;100-0;200-0;100-0;50-1"

    FillDataHeaderLv(Me.LvFoundedrecs, GetSafeTranslationValue(strColumnString), strColumnWidthInfo)

  End Sub

  Private Function LV_GetItemIndex(ByRef lv As ListView) As Integer

    Try
      If lv.Items.Count > 0 Then
        Dim lvi As ListViewItem = lv.SelectedItems(0)  '.Item(0)
        If lvi.Selected Then
          Return lvi.Index
        Else
          Return -1
        End If
      End If

    Catch ex As Exception

    End Try

  End Function

#End Region

  Private Sub txt_Ort_KeyPress(ByVal sender As Object, _
                                    ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Cbo_Kanton.KeyPress, _
                                    txt_Gemeinde.KeyPress, txt_Adresse.KeyPress, txt_Strasse.KeyPress, txt_Postfach.KeyPress, _
                                    txt_Zusatz.KeyPress, txt_ZHD.KeyPress, txt_Land.KeyPress, txt_PLZ.KeyPress, txt_Ort.KeyPress, _
                                    txt_StammNr.KeyPress, txt_Provision.KeyPress

    Try
      If e.KeyChar = Chr(13) Then
        SendKeys.Send("{tab}")
        e.Handled = True
      End If

    Catch ex As Exception
      MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try

  End Sub

  Private Sub LvFoundedrecs_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles LvFoundedrecs.DoubleClick

    LvFoundedrecs.LabelEdit = True
    LvFoundedrecs.SelectedItems(0).BeginEdit()

  End Sub

  Private Sub LvFoundedrecs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LvFoundedrecs.SelectedIndexChanged

    For i As Integer = 0 To LvFoundedrecs.Items.Count - 1
      If LvFoundedrecs.Items(i).Selected = True Then
        DisplayFoundedData(Me, Me.LvFoundedrecs.Items(i).SubItems(0).Text)
        Exit For
      End If

    Next

  End Sub

  Private Sub Cbo_Kanton_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Kanton.QueryPopUp
    ListAllKanton(Cbo_Kanton)
  End Sub

  Private Sub bbiDelete_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick
    Dim _ClsFuncDb As New ClsDbFunc

    _ClsFuncDb.DeleteSelectedRec(CInt(Me.LblRecNr.Text))

    For i As Integer = 0 To LvFoundedrecs.Items.Count - 1
      If LvFoundedrecs.Items(i).Selected Then
        Me.LvFoundedrecs.Items(i).Remove()
        Exit For
      End If

    Next
    Me.LblRecNr.Text = 0

  End Sub

  Private Sub txt_Provision_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Provision.Validated
    If ErrorProvider1.GetError(txt_Provision) = String.Empty Then ErrorProvider1.SetError(txt_Provision, String.Empty)
    Me.txt_Provision.Text = Format(Val(Me.txt_Provision.Text), "f")
  End Sub

  Private Sub txt_Provision_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txt_Provision.Validating
    If Not IsNumeric(Me.txt_Provision.Text) Then
      e.Cancel = False

      ' Select the offending text.
      txt_Provision.Select(0, txt_Provision.Text.Length)

      ' Give the ErrorProvider the error message to display.
      ErrorProvider1.SetError(txt_Provision, GetSafeTranslationValue("Falsche Format. Z. B. 4.0"))

    Else
      ErrorProvider1.SetError(txt_Provision, "")

    End If

  End Sub

  Private Sub txt_PLZ_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_PLZ.LostFocus
    Me.txt_Ort.Text = GetCitynameFromBox(Me.txt_PLZ.Text)
  End Sub

  Private Sub txt_Land_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Land.LostFocus
    If Me.txt_Land.Text = String.Empty Then Me.txt_Land.Text = "CH"
  End Sub


  'Private Sub btnRecInfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  '  'Me.HelpProvider1.SetHelpString(Me.Cbo_Kanton, Me.LblRecInfo.Text)

  '  Me.ToolTip1.ToolTipTitle = "Info über Datensatz"
  '  Me.ToolTip1.Show(Me.LblRecInfo.Text, Me.LvFoundedrecs, Me.LvFoundedrecs.Location.X + Me.LvFoundedrecs.Width - Me.LvFoundedrecs.Left, Me.LvFoundedrecs.Location.Y / Screen.PrimaryScreen.BitsPerPixel, 5000) ' Me.Cbo_Kanton.Left, 2000)

  'End Sub




#Region "Helpers..."

  Function GetSafeTranslationValue(ByVal dicKey As String) As String
    Try

      If ClsDataDetail.TranslationData.ContainsKey(dicKey) Then
        Return ClsDataDetail.TranslationData.Item(dicKey).LogedUserLanguage

      Else
        Return dicKey

      End If

    Catch ex As Exception
      Return dicKey
    End Try

  End Function

  Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
    Dim lstStuff As ListViewItem = New ListViewItem()
    Dim lvwColumn As ColumnHeader

    With Lv
      .Clear()

      ' Nr;Nummer;Name;Strasse;PLZ Ort
      If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
      If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

      Dim strCaption As String() = Regex.Split(strColumnList, ";")
      ' 0-1;0-1;2000-0;2000-0;2500-0
      Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
      Dim strFieldWidth As String
      Dim strFieldAlign As String = "0"
      Dim strFieldData As String()

      For i = 0 To strCaption.Length - 1
        lvwColumn = New ColumnHeader()
        lvwColumn.Text = strCaption(i).ToString
        strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

        If strFieldInfo(i).ToString.StartsWith("-") Then
          strFieldWidth = strFieldData(1)
          lvwColumn.Width = CInt(strFieldWidth) * -1
          If strFieldData.Count > 1 Then
            strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
          End If
        Else
          strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
          lvwColumn.Width = CInt(strFieldWidth) '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
          If strFieldData.Count > 1 Then
            strFieldAlign = strFieldData(1)
          End If
          'CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
        End If
        If strFieldAlign = "1" Then
          lvwColumn.TextAlign = HorizontalAlignment.Right
        ElseIf strFieldAlign = "2" Then
          lvwColumn.TextAlign = HorizontalAlignment.Center
        Else
          lvwColumn.TextAlign = HorizontalAlignment.Left

        End If

        lstStuff.BackColor = Color.Yellow

        .Columns.Add(lvwColumn)
      Next

      lvwColumn = Nothing
    End With

  End Sub


#End Region



End Class
