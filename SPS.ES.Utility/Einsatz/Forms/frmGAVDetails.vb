
'Imports System.Reflection.Assembly
'Imports System.ComponentModel
'Imports DevExpress.XtraGrid.Columns
'Imports NLog
'Imports DevComponents.DotNetBar

'Imports SPProgUtility.ColorUtility.ClsColorUtility
'Imports SPProgUtility.SPTranslation.ClsTranslation
'Imports SPS.ES.Utility.CalculateESMarge.ClsESMarge
'Imports SPS.ES.Utility.SPSESUtility.ClsESFunktionality

'Imports DevExpress.XtraGrid.Views.Base
'Imports System.Globalization
'Imports DevExpress.XtraEditors.Repository
'Imports DevExpress.Utils.Paint
'Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
'Imports System.Text.RegularExpressions

'Public Class frmGAVDetails
'  Private Shared logger As Logger = LogManager.GetCurrentClassLogger()
'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

'  Private Property _ESSetting As New ClsESDataSetting
'	Private _ClsESUtility As CalculateESMarge.ClsESMarge

'	Private loFunctionality As SPS.MA.Lohn.Utility.ClsLOFunktionality

'  Private Property SelectedMANr As Integer
'  Public Property MetroForeColor As System.Drawing.Color
'  Public Property MetroBorderColor As System.Drawing.Color

'  Private Sub frmGAVDetails_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

'    If Not Me.WindowState = FormWindowState.Minimized Then
'      My.Settings.iHeightGAVDetail = Me.Height
'      My.Settings.iWidthGAVDetail = Me.Width
'      My.Settings.frmLocationGAVDetail = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

'      My.Settings.Save()
'    End If

'  End Sub

'  Private Sub frmGAVDetails_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
'    If e.KeyCode = Keys.F12 And _ClsProgSetting.GetLogedUSNr = 1 Then
'      Dim strRAssembly As String = ""
'      Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"

'      For Each a In AppDomain.CurrentDomain.GetAssemblies()
'        strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
'      Next

'      strMsg = String.Format(strMsg, vbNewLine, _
'                             GetExecutingAssembly().FullName, _
'                             GetExecutingAssembly().Location, _
'                             strRAssembly)
'      DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
'    End If

'  End Sub

'  Private Sub frmGAVDetails_Load(sender As Object, e As System.EventArgs) Handles Me.Load
'    Dim tbl As New DataTable()
'    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'    Try
'      Me.Text = TranslateText(Me.Text)
'      Me.btnClose.Text = TranslateText(Me.btnClose.Text)

'      Try
'        Me.KeyPreview = True
'        Dim liColor As List(Of Color) = GetMetroColor("INFO")  ' Color.White |  Color.Orange

'        If liColor.Count < 1 Then liColor = New List(Of Color)(New Color() {Color.White, Color.Orange})
'        Me.MetroForeColor = liColor(0)
'        Me.MetroBorderColor = liColor(1)

'        Me.KeyPreview = True
'        StyleManager.Style = eStyle.Metro
'        StyleManager.MetroColorGeneratorParameters = New DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(Me.MetroForeColor, _
'                                                                                                                                 Me.MetroBorderColor)

'      Catch ex As Exception
'        m_Logger.LogError(String.Format("{0}.Setting FormColor:{1}", strMethodeName, ex.Message))

'      End Try

'      Try
'        If My.Settings.iHeightGAVDetail > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.iHeightGAVDetail)
'        If My.Settings.iWidthGAVDetail > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.iWidthGAVDetail)
'        If My.Settings.frmLocationGAVDetail <> String.Empty Then
'          Dim aLoc As String() = My.Settings.frmLocationGAVDetail.Split(CChar(";"))
'          If Screen.AllScreens.Length = 1 Then
'            If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
'          End If
'          Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
'        End If

'      Catch ex As Exception
'        m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

'      End Try

'    Catch ex As Exception
'      m_Logger.LogError(String.Format("{0}.Formtranslation.{1}", strMethodeName, ex.Message))

'    End Try
'    ListGAVDetails()

'  End Sub

'  Private Sub ListGAVDetails()
'    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'    Dim bAllowedKDVerInTarif As Boolean = GetKDTarifMinerKDVer

'    _ClsESUtility = New CalculateESMarge.ClsESMarge(Me._ESSetting)
'    Dim dValue As Dictionary(Of String, String) = _ClsESUtility.AnalyseGAVString

'    ' GAV-detail
'    Try
'      Dim strColumnBez As String = "; "
'      Dim strColumnWidth As String = String.Format("{0}-0;250-0", 200)
'      Try
'        strColumnBez = TranslateText(strColumnBez)
'      Catch ex As Exception
'        m_Logger.LogError(String.Format("{0}.Übersetzungsfehler. {1}", strMethodeName, ex.Message))
'      End Try
'      FillDataHeaderLv(lvGAVDetail, strColumnBez, strColumnWidth)

'    Catch ex As Exception
'      m_Logger.LogError(String.Format("{0}.Aufbau der Spalten. {1}", strMethodeName, ex.Message))

'    End Try

'    Try
'      Me.lvGAVDetail.BeginUpdate()
'      Dim strKey As String = String.Empty
'      Dim strValue As String = String.Empty

'      For i As Integer = 0 To dValue.Count - 1
'        strKey = dValue.Keys(i)
'        strValue = dValue.Values(i)
'        If strValue.ToLower = "false" Then strValue = "Nein"
'        If strValue.ToLower = "true" Then strValue = "Ja"
'        If strValue <> String.Empty Then
'          Me.lvGAVDetail.Items.Add(strKey)
'          Me.lvGAVDetail.Items(Me.lvGAVDetail.Items.Count - 1).SubItems.Add(strValue)
'        End If
'      Next
'      Me.lvGAVDetail.EndUpdate()

'    Catch ex As Exception
'      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

'    End Try

'  End Sub

'  Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
'    Dim lstStuff As ListViewItem = New ListViewItem()
'    Dim lvwColumn As ColumnHeader

'    With Lv
'      .Clear()

'      ' Nr;Nummer;Name;Strasse;PLZ Ort
'      If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
'      If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

'      Dim strCaption As String() = Regex.Split(strColumnList, ";")
'      ' 0-1;0-1;2000-0;2000-0;2500-0
'      Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
'      Dim strFieldWidth As String
'      Dim strFieldAlign As String = "0"
'      Dim strFieldData As String()

'      For i = 0 To strCaption.Length - 1
'        lvwColumn = New ColumnHeader()
'        lvwColumn.Text = strCaption(i).ToString
'        strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

'        If strFieldInfo(i).ToString.StartsWith("-") Then
'          strFieldWidth = strFieldData(1)
'          lvwColumn.Width = CInt(strFieldWidth) * -1
'          If strFieldData.Count > 1 Then
'            strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
'          End If
'        Else
'          strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
'          lvwColumn.Width = CInt(strFieldWidth) '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
'          If strFieldData.Count > 1 Then
'            strFieldAlign = strFieldData(1)
'          End If
'        End If
'        If strFieldAlign = "1" Then
'          lvwColumn.TextAlign = HorizontalAlignment.Right
'        ElseIf strFieldAlign = "2" Then
'          lvwColumn.TextAlign = HorizontalAlignment.Center
'        Else
'          lvwColumn.TextAlign = HorizontalAlignment.Left

'        End If
'        lstStuff.BackColor = Color.Yellow
'        .Columns.Add(lvwColumn)
'      Next

'      lvwColumn = Nothing
'    End With

'  End Sub

'  Public Sub New(ByVal _setting As ClsESDataSetting)

'    ' Dieser Aufruf ist für den Designer erforderlich.
'    InitializeComponent()

'    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
'    Me._ESSetting = _setting

'    Dim liColor As List(Of Color) = GetMetroColor("INFO")  ' Color.White |  Color.Orange
'    If liColor.Count < 1 Then liColor = New List(Of Color)(New Color() {Color.White, Color.Orange})
'    Me.MetroForeColor = liColor(0)
'    Me.MetroBorderColor = liColor(1)

'  End Sub

'  Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
'    Me.Dispose()
'  End Sub

'End Class