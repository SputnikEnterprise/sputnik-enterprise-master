
'Imports System.Reflection.Assembly
'Imports System.ComponentModel
'Imports DevExpress.XtraGrid.Columns

'Imports DevComponents.DotNetBar

'Imports SPProgUtility.ColorUtility.ClsColorUtility
'Imports SPProgUtility.SPTranslation.ClsTranslation
'Imports SPS.ES.Utility.CalculateESMarge.ClsESMarge
'Imports SPS.ES.Utility.SPSESUtility.ClsESFunktionality

'Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsLOFunktionality

'Imports DevExpress.XtraGrid.Views.Base
'Imports System.Globalization
'Imports DevExpress.XtraEditors.Repository
'Imports DevExpress.Utils.Paint
'Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
'Imports System.Text.RegularExpressions
'Imports SP.Infrastructure.UI
'Imports SPProgUtility.Mandanten
'Imports DevExpress.LookAndFeel
'Imports SP.Infrastructure.Logging

'Public Class frmESMarge

'	''' <summary>
'	''' The logger.
'	''' </summary>
'	Private Shared m_Logger As ILogger = New Logger()

'	''' <summary>
'	''' The Initialization data.
'	''' </summary>
'	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

'	''' <summary>
'	''' The translation value helper.
'	''' </summary>
'	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper


'	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

'	Private _ClsESUtility As CalculateESMarge.ClsESMarge
'	Private Property SelectedMANr As Integer

'	''' <summary>
'	''' UI Utility functions.
'	''' </summary>
'	Private m_UtilityUI As UtilityUI

'	''' <summary>
'	''' The mandant.
'	''' </summary>
'	Private m_MandantData As Mandant


'#Region "public properties"

'	Public Property ESSettingData As ClsESDataSetting

'#End Region


'#Region "Construct"

'	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

'		DevExpress.UserSkins.BonusSkins.Register()
'		DevExpress.Skins.SkinManager.EnableFormSkins()

'		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
'		m_InitializationData = _setting
'		m_MandantData = New Mandant
'		m_UtilityUI = New UtilityUI

'		' Dieser Aufruf ist für den Designer erforderlich.
'		InitializeComponent()

'		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
'		'Me.ESSettingData = _setting

'	End Sub

'#End Region

'	Private Sub frm_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

'		If Not Me.WindowState = FormWindowState.Minimized Then
'			My.Settings.iHeight = Me.Height
'			My.Settings.iWidth = Me.Width
'			My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)

'			My.Settings.Save()
'		End If

'	End Sub

'	Private Sub frm_Load(sender As Object, e As System.EventArgs) Handles Me.Load
'		Dim tbl As New DataTable()
'		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'		Try
'			Me.btnClose.Text = TranslateText(Me.btnClose.Text)
'			Me.XtraTabPage1.Text = TranslateText(Me.XtraTabPage1.Text)
'			Me.XtraTabPage2.Text = TranslateText(Me.XtraTabPage2.Text)

'			Try
'				Me.KeyPreview = True
'				Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
'				If strStyleName <> String.Empty Then
'					UserLookAndFeel.Default.SetSkinStyle(strStyleName)
'				End If

'			Catch ex As Exception
'				m_Logger.LogError(String.Format("{0}.Setting FormColor:{1}", strMethodeName, ex.Message))

'			End Try

'			Try
'				If My.Settings.iHeight > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.iHeight)
'				If My.Settings.iWidth > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.iWidth)
'				If My.Settings.frmLocation <> String.Empty Then
'					Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))
'					If Screen.AllScreens.Length = 1 Then
'						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
'					End If
'					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
'				End If

'			Catch ex As Exception
'				m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

'			End Try
'			Me.sccMain_0.SplitterPosition = Math.Max(Me.sccMain_0.Height - 100, 100)
'			Me.sccMain_1.SplitterPosition = Math.Min(Me.sccMain_1.Height - 200, 100)

'		Catch ex As Exception
'			m_Logger.LogError(String.Format("{0}.Formtranslation.{1}", strMethodeName, ex.Message))

'		End Try

'		tbl = CreateTable()
'		Try
'			Me.grdFoundedrec.BeginUpdate()
'			Me.grdFoundedrec.DataSource = tbl
'			Me.grdFoundedrec.ForceInitialize()
'			Me.grdFoundedrec.EndUpdate()
'			Me.GridView1.BestFitColumns()

'			Try
'				Me.GridView1.Columns(2).DisplayFormat.FormatString = "n"
'				Me.GridView1.OptionsFind.AlwaysVisible = False

'				For Each col As GridColumn In GridView1.Columns
'					Trace.WriteLine(String.Format("{0}", col.FieldName))
'					col.MinWidth = 0
'					Try
'						col.Visible = True
'						col.Caption = TranslateText(col.GetCaption)

'					Catch ex As Exception
'						col.Visible = False

'					End Try
'				Next col

'			Catch ex As Exception

'			End Try

'		Catch ex As Exception

'		End Try

'	End Sub

'	Private Function CreateTable() As DataTable
'		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'		Dim tbl As New DataTable()
'		Dim bAllowedKDVerInTarif As Boolean = GetKDTarifMinerKDVer

'		tbl.Columns.Add(TranslateText("Bezeichnung"), GetType(String))
'		tbl.Columns.Add(TranslateText("Ohne BVG"), GetType(String))
'		tbl.Columns.Add(TranslateText("Mit BVG"), GetType(String)) 'GetType(Decimal))

'		_ClsESUtility = New CalculateESMarge.ClsESMarge(Me.ESSettingData)
'		Dim rFrec As DataTable = _ClsESUtility.GetSelectedData4CalcESMargeInDt()

'		If rFrec Is Nothing Or rFrec.Rows.Count = 0 Then Return rFrec

'		Dim dbetrag As Single = 0
'		Dim strValue As String = String.Empty

'		dbetrag = rFrec.Rows(0)("Stundenlohn")
'		If dbetrag <> 0 Then tbl.Rows.Add(New Object() {TranslateText("Stundenlohn"), "", Format(dbetrag, "n")})

'		dbetrag = rFrec.Rows(0)("MAStdSpesen")
'		If dbetrag <> 0 Then tbl.Rows.Add(New Object() {TranslateText("Stundenspesen"), "", Format(dbetrag, "n")})
'		dbetrag = rFrec.Rows(0)("MATSpesen")
'		If dbetrag <> 0 Then tbl.Rows.Add(New Object() {TranslateText("Tagesspesen"), "", Format(dbetrag, "n")})

'		dbetrag = GetMAAlter(Now.Year, rFrec.Rows(0)("GebDat"))
'		If dbetrag <> 0 Then tbl.Rows.Add(New Object() {TranslateText("Alter"), "", Format(dbetrag, "00")})
'		strValue = rFrec.Rows(0)("Suva")
'		If strValue <> String.Empty Then tbl.Rows.Add(New Object() {TranslateText("Suva-Code"), "", strValue})

'		strValue = rFrec.Rows(0)("BVGBegin")
'		If strValue <> String.Empty Then tbl.Rows.Add(New Object() {TranslateText("BVG-pflichtig ab"), "", strValue})

'		strValue = IsMARentner(rFrec.Rows(0)("MANr"), Me.ESSettingData.SelectedYear)
'		If strValue <> String.Empty Then tbl.Rows.Add(New Object() {TranslateText("Ist Rentner?"), "", TranslateText(If(CBool(strValue), "Ja", "Nein"))})

'		dbetrag = rFrec.Rows(0)("Tarif")
'		Dim _sKDMinderung As Single = dbetrag * If(bAllowedKDVerInTarif, rFrec.Rows(0)("KD_UmsMin"), 0) / 100

'		If dbetrag <> 0 Then tbl.Rows.Add(New Object() {String.Format("{0} {1}", TranslateText("Kundentarif"),
'					If(_sKDMinderung <> 0, String.Format("(Vergütung {0} %)", Format(rFrec.Rows(0)("KD_UmsMin"), "n")), "")),
'					"",
'				String.Format("{0}{1}", Format(dbetrag, "n"), If(_sKDMinderung <> 0, Format(_sKDMinderung * -1, "n"), ""))})

'		dbetrag = rFrec.Rows(0)("KDTSpesen")
'		If dbetrag <> 0 Then tbl.Rows.Add(New Object() {TranslateText("Kunden Tagesspesen"), "", Format(dbetrag, "n")})


'		'dbetrag = _ClsESUtility.GetAGBeitraginES()
'		'Me._ESSetting.ShowMargeWithBVG = True
'		'Dim dbetragWithBVG = _ClsESUtility.GetAGBeitraginES()
'		'If dbetrag + dbetragWithBVG <> 0 Then tbl.Rows.Add(New Object() {TranslateText("Arbeitgeber-Beitrag"), Format(dbetrag, "n"), Format(dbetragWithBVG, "n")})

'		strValue = _ClsESUtility.GetSelectedESMarge()
'		If strValue <> String.Empty Then tbl.Rows.Add(New Object() {TranslateText("AG-Beiträge in CHF"), Format(Val(strValue.Split(CChar("¦"))(4)), "n"),
'				Format(Val(strValue.Split(CChar("¦"))(5)), "n")})
'		If strValue <> String.Empty Then tbl.Rows.Add(New Object() {TranslateText("Marge pro Stunde in CHF"),
'				String.Format("{0}", Format(Val(strValue.Split(CChar("¦"))(0)), "n")),
'				String.Format("{0}", Format(Val(strValue.Split(CChar("¦"))(1)), "n"))})
'		If strValue <> String.Empty Then tbl.Rows.Add(New Object() {TranslateText("Marge pro Stunde in Prozent"),
'				String.Format("{0} %", Format(Val(strValue.Split(CChar("¦"))(6)), "n")),
'				String.Format("{0} %", Format(Val(strValue.Split(CChar("¦"))(7)), "n"))})

'		' Einsatzdetail
'		Try
'			Dim strColumnBez As String = "; "
'			Dim strColumnWidth As String = String.Format("{0}-0;150-1", Me.lvESDetail.Width - 300)
'			Try
'				strColumnBez = TranslateText(strColumnBez)
'			Catch ex As Exception
'				m_Logger.LogError(String.Format("{0}.Übersetzungsfehler. {1}", strMethodeName, ex.Message))
'			End Try

'			FillDataHeaderLv(lvESDetail, strColumnBez, strColumnWidth)
'			Me.lvESDetail.BeginUpdate()
'			With Me.lvESDetail
'				.Items.Add(TranslateText("Einsatznummer"))
'				.Items(.Items.Count - 1).SubItems.Add(rFrec.Rows(0)("ESNr"))
'				.Items.Add(TranslateText("Kandidatennummer"))
'				.Items(.Items.Count - 1).SubItems.Add(rFrec.Rows(0)("MANr"))

'				.Items.Add(TranslateText("Zeitraum"))
'				.Items(.Items.Count - 1).SubItems.Add(String.Format("{0} - {1}", Format(rFrec.Rows(0)("ES_Ab"), "d"),
'																						 If(String.IsNullOrWhiteSpace(rFrec.Rows(0)("ES_Ende").ToString),
'																								GetESEndeByNull,
'																								Format(rFrec.Rows(0)("ES_Ende"), "d"))))
'				.Items.Add(TranslateText("GAV-Beruf"))
'				.Items(.Items.Count - 1).SubItems.Add(rFrec.Rows(0)("GAVGruppe0"))

'			End With
'			Me.lvESDetail.EndUpdate()

'		Catch ex As Exception

'		End Try

'		Try
'			Dim strColumnBez As String = ";Durchschnitt; "
'			Dim strColumnWidth As String = String.Format("{0}-0;150-1;80-1", Me.lvFinalData.Width - 250)
'			Try
'				strColumnBez = TranslateText(strColumnBez)
'			Catch ex As Exception
'				m_Logger.LogError(String.Format("{0}.Übersetzungsfehler. {1}", strMethodeName, ex.Message))
'			End Try

'			FillDataHeaderLv(lvFinalData, strColumnBez, strColumnWidth)
'			Me.lvFinalData.BeginUpdate()
'			With Me.lvFinalData
'				For i As Integer = 0 To 1
'					.Items.Add(String.Format("{0}", TranslateText("Margendurchschnitt"))) ', If(i = 0, "in CHF/Std.", "in %")))
'					.Items(i).SubItems.Add(Format(Val(strValue.Split(CChar("¦"))(If(i = 0, 2, 3))), "n"))
'					.Items(i).SubItems.Add(If(i = 0, " CHF/Std.", " %"))
'				Next
'			End With
'			Me.lvFinalData.EndUpdate()

'		Catch ex As Exception

'		End Try


'		Return tbl
'	End Function

'	Function CreateDetailTable() As DataTable
'		Dim tbl As New DataTable()

'		tbl.Columns.Add(TranslateText("Bezeichnung"), GetType(String))
'		'tbl.Columns.Add(" ", GetType(String))
'		tbl.Columns.Add(TranslateText("Beträge"), GetType(String))

'		For i As Integer = 0 To Me.ESSettingData.aDebugMargenCalculation.Count - 1
'			Dim strLine As String = Me.ESSettingData.aDebugMargenCalculation.Keys(i)

'			If strLine <> String.Empty Then tbl.Rows.Add(New Object() {TranslateText(Me.ESSettingData.aDebugMargenCalculation.Keys(i)),
'					Me.ESSettingData.aDebugMargenCalculation.Values(i)})
'			'If(Me._ESSetting.aDebugMargenCalculation.Keys(i).ToLower.Contains("betrag für"), "", _
'			'  Me._ESSetting.aDebugMargenCalculation.Values(i)), _
'			'If(Me._ESSetting.aDebugMargenCalculation.Keys(i).ToLower.Contains("betrag für"), Me._ESSetting.aDebugMargenCalculation.Values(i), "")})

'		Next

'		Return tbl
'	End Function

'	Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
'		Dim lstStuff As ListViewItem = New ListViewItem()
'		Dim lvwColumn As ColumnHeader

'		With Lv
'			.Clear()

'			' Nr;Nummer;Name;Strasse;PLZ Ort
'			If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
'			If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

'			Dim strCaption As String() = Regex.Split(strColumnList, ";")
'			' 0-1;0-1;2000-0;2000-0;2500-0
'			Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
'			Dim strFieldWidth As String
'			Dim strFieldAlign As String = "0"
'			Dim strFieldData As String()

'			For i = 0 To strCaption.Length - 1
'				lvwColumn = New ColumnHeader()
'				lvwColumn.Text = strCaption(i).ToString
'				strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

'				If strFieldInfo(i).ToString.StartsWith("-") Then
'					strFieldWidth = strFieldData(1)
'					lvwColumn.Width = CInt(strFieldWidth) * -1
'					If strFieldData.Count > 1 Then
'						strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
'					End If
'				Else
'					strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
'					lvwColumn.Width = CInt(strFieldWidth) '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
'					If strFieldData.Count > 1 Then
'						strFieldAlign = strFieldData(1)
'					End If
'				End If
'				If strFieldAlign = "1" Then
'					lvwColumn.TextAlign = HorizontalAlignment.Right
'				ElseIf strFieldAlign = "2" Then
'					lvwColumn.TextAlign = HorizontalAlignment.Center
'				Else
'					lvwColumn.TextAlign = HorizontalAlignment.Left

'				End If
'				lstStuff.BackColor = Color.Yellow
'				.Columns.Add(lvwColumn)
'			Next

'			lvwColumn = Nothing
'		End With

'	End Sub


'	Private Sub btnClose_Click(sender As System.Object, e As System.EventArgs) Handles btnClose.Click
'		Me.Dispose()
'	End Sub


'	Private Sub gridView1_CustomColumnDisplayText(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles GridView1.CustomColumnDisplayText
'		Dim ciCH As CultureInfo = New CultureInfo("de-ch")

'		Dim View As ColumnView = sender
'		If e.Column.FieldName = "Betrag" Then
'			Dim currencyType As Integer = View.GetRowCellValue(View.GetRow(e.GroupRowHandle), View.Columns("decimalType"))

'			Dim dBetrag As Decimal = View.GetRowCellValue(View.GetRow(e.GroupRowHandle), View.Columns("Betrag"))
'			If dBetrag <> 0 Then
'				' Conditional formatting: 
'				Select Case currencyType
'					Case 0
'						e.DisplayText = String.Format(ciCH, "{0:c2}", dBetrag) ' "{0:c}", dBetrag) "#.00;[#.0];Zero"
'						e.Column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
'						'Case 1 : e.DisplayText = String.Format(ciEUR, "{0:c}", price)
'				End Select
'			Else
'				e.DisplayText = String.Empty

'			End If

'		End If

'	End Sub

'	Private Sub gridView2_CustomColumnDisplayText(sender As Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles GridView2.CustomColumnDisplayText
'		'Dim ciCH As CultureInfo = New CultureInfo("de-ch")

'		'Dim View As ColumnView = sender
'		'If e.Column.FieldName = "Betrag" Then
'		'  Dim currencyType As Integer = View.GetRowCellValue(e.RowHandle, View.Columns("decimalType"))
'		'  Dim dBetrag As Decimal = View.GetRowCellValue(e.RowHandle, View.Columns("Betrag"))
'		'  If dBetrag <> 0 Then
'		'    ' Conditional formatting: 
'		'    Select Case currencyType
'		'      Case 0
'		'        e.DisplayText = String.Format(ciCH, "{0:c2}", dBetrag)
'		'        e.Column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far

'		'    End Select
'		'  Else
'		'    e.DisplayText = String.Empty
'		'  End If
'		'ElseIf e.Column.FieldName = "Art" Then
'		'  Dim strValue As String = View.GetRowCellValue(e.RowHandle, View.Columns("Art"))
'		'  'e.DisplayText = String.Format(ciCH, "{0:c2}", strValue)
'		'  e.Column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center

'		'ElseIf e.Column.FieldName = "Periode" Then
'		'  Dim strValue As String = View.GetRowCellValue(e.RowHandle, View.Columns("Periode"))
'		'  e.Column.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far

'		'End If

'	End Sub

'	Private Sub frmMAGuthaben_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
'		If e.KeyCode = Keys.Escape Then
'			Me.Dispose()

'		ElseIf e.KeyCode = Keys.F12 And _ClsProgSetting.GetLogedUSNr = 1 Then
'			Dim strRAssembly As String = ""
'			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
'			For Each a In AppDomain.CurrentDomain.GetAssemblies()
'				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
'			Next

'			strMsg = String.Format(strMsg, vbNewLine,
'														 GetExecutingAssembly().FullName,
'														 GetExecutingAssembly().Location,
'														 strRAssembly)
'			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
'		End If

'	End Sub

'	Function RoundSPSNumber(ByVal value As Decimal, ByVal Digit As Short) As Decimal
'		Dim dRValue As Decimal = value
'		If Digit = Decimal.Zero Then Digit = 2

'		dRValue = Format(CLng(value / 0.05) * 0.05, "0." & (New String("0", Digit)))

'		Return dRValue
'	End Function

'	Private Sub XtraTabControl1_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabControl1.SelectedPageChanged

'		If XtraTabControl1.SelectedTabPage Is Me.XtraTabPage2 And Me.GridView2.Columns.Count = 0 Then
'			Try
'				Dim tblDetail As DataTable = CreateDetailTable()

'				Me.grdFoundedDetailrec.BeginUpdate()
'				Me.grdFoundedDetailrec.DataSource = tblDetail

'				Me.grdFoundedDetailrec.ForceInitialize()
'				Me.grdFoundedDetailrec.EndUpdate()
'				Me.GridView2.BestFitColumns()

'				Try
'					'Me.GridView2.Columns(2).DisplayFormat.FormatString = "n"
'					Me.GridView2.OptionsFind.AlwaysVisible = False
'					For Each col As GridColumn In GridView2.Columns
'						Trace.WriteLine(String.Format("{0}", col.FieldName))
'						col.MinWidth = 0
'						Try
'							col.Visible = True
'							col.Caption = TranslateText(col.GetCaption)

'						Catch ex As Exception
'							col.Visible = False

'						End Try
'					Next col


'				Catch ex As Exception

'				End Try

'			Catch ex As Exception

'			End Try
'		End If

'	End Sub

'	Private Sub GridView2_CustomDrawCell(sender As Object, e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles GridView2.CustomDrawCell
'		Dim r As Rectangle = e.Bounds

'		'If e.Column.FieldName = "Bezeichnung" Then
'		'  e.Appearance.DrawString(e.Cache, DirectCast(e.CellValue, DateTime).Month.ToString(), r)
'		'  If (DirectCast(e.Cell, GridCellInfo).State And GridRowCellState.FocusedCell) > 0 Then
'		'    XPaint.Graphics.DrawFocusRectangle(e.Graphics, r, e.Appearance.GetForeColor(), e.Appearance.GetBackColor())
'		'  End If
'		'  e.Handled = True
'		'End If

'		If e.CellValue.ToString.ToLower.Contains("teil") Then
'			Trace.WriteLine("e.DisplayText: " & e.DisplayText & " | " & "e.CellValue: " & e.CellValue)
'			'e.Appearance.BackColor = Me.MetroBorderColor ' Color.
'			e.Appearance.Font = New Font(e.Appearance.Font.Name, e.Appearance.Font.Size, FontStyle.Bold)

'		Else
'			If e.Column.FieldName = "Beträge" Then
'				e.CellValue = If(Val(e.CellValue) = 0, e.CellValue, Format(Val(e.CellValue), "n"))
'				Trace.WriteLine("e.DisplayText: " & e.DisplayText & " | " & "e.CellValue: " & e.CellValue)
'				e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
'			End If
'		End If

'	End Sub

'	Private Sub GridView2_RowStyle(sender As Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles GridView2.RowStyle
'		Dim rowcolors() As Color = {Color.Gray, Color.LightGray, Color.DarkGray, Color.LightGray}



'		'If Not (CType(sender, DevExpress.XtraGrid.Views.Base.ColumnView)).IsDataRow(e.RowHandle) Then
'		'  Return
'		'End If
'		'Dim index As Integer = e.RowHandle Mod rowcolors.Length
'		'If Me.grdFoundedDetailrec.Views.Item(0).GetRow(index).item("Beträge") = String.Empty Then
'		'  e.Appearance.BackColor = Color.White
'		'  e.Appearance.BackColor2 = rowcolors(index)
'		'  e.Appearance.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical

'		'End If


'	End Sub


'End Class

'Public Class MyObject

'	Private Property Column_1 As String
'	Private Property Column_2 As String

'	Public Sub New(ByVal Col1 As String, ByVal Col2 As String)
'		Column_1 = Col1
'		Column_2 = Col2
'	End Sub


'End Class