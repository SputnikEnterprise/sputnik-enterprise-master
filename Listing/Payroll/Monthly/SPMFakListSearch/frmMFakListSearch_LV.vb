
'Imports System.Data.SqlClient
'Imports System.Text.RegularExpressions

'Imports System.Xml
'Imports System.Xml.Linq

'Imports System.Xml.XmlTextWriter
'Imports System.Xml.XmlTextReader
'Imports System.Xml.XPath


'Public Class frmMFakListSearch_LV

'  Dim _ClsFunc As New ClsDivFunc
'  Dim _ClsReg As New SPProgUtility.ClsDivReg
'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
'  Dim strLastSortBez As String

'#Region "Diverse Speichermethoden..."

'  Sub New()

'    ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
'    InitializeComponent()
'    _TotalBetrag = 0

'  End Sub
'  '// 3600
'  Dim _TotalBetrag As Decimal
'  Public Property GetTotal_Betrag() As Decimal
'    Get
'      Return _TotalBetrag
'    End Get
'    Set(ByVal value As Decimal)
'      _TotalBetrag = value
'    End Set
'  End Property

'#End Region


'#Region "Funktionen zur Virtualisierung..."

'  Private Structure LvData
'    Public _Test As String
'    Public _MANr As String
'    Public _MAName As String
'    Public _Kanton As String
'    Public _Monat As String
'    Public _Basis As String
'    Public _Betrag As String

'    Public Sub New(ByVal rLOrec As SqlClient.SqlDataReader)

'      Try
'        With rLOrec
'          Me._Test = String.Empty
'          Me._MANr = rLOrec("MANr").ToString
'          Me._MAName = rLOrec("MANachname").ToString & ", " & rLOrec("MAVorname").ToString
'          Me._Kanton = rLOrec("S_Kanton").ToString
'          Me._Monat = Format(rLOrec("LP"), "00") & " / " & rLOrec("Jahr").ToString

'          Me._Basis = Format(rLOrec("m_Bas"), "n")
'          Me._Betrag = Format(rLOrec("m_btr"), "n")

'        End With

'      Catch ex As SqlException
'        MsgBox("Db: " & ex.Message, MsgBoxStyle.Critical, "frmMFakListSearch_LV -> New")

'      Catch ex As Exception
'        MsgBox(ex.Message, MsgBoxStyle.Critical, "frmMFakListSearch_LV -> New")

'      End Try

'    End Sub

'  End Structure

'  Private TotalLVData As New List(Of LvData)

'  Function GetLvDataCount(ByVal rLOrec As SqlClient.SqlDataReader) As Integer
'    Dim i As Integer = 0
'    Dim TotalBetrag As Double = 0

'    Try
'      While rLOrec.Read
'        TotalLVData.Add(New LvData(rLOrec))

'        _TotalBetrag += CDec(rLOrec("m_btr").ToString)

'        i += 1
'      End While

'    Catch ex As SqlException
'      MsgBox("Db: " & ex.Message, MsgBoxStyle.Critical, "frmMFakListSearch_LV -> GetLvDataCount")


'    Catch ex As Exception
'      MsgBox(ex.Message, MsgBoxStyle.Critical, "frmMFakListSearch_LV -> GetLvDataCount")

'    End Try

'    Return i
'  End Function

'  Sub FillrecData2Array(ByVal Lv As ListView, ByVal strQuery As String)
'    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
'    Dim i As Integer = 0

'    Try
'      Conn.Open()
'      Dim cmd As System.Data.SqlClient.SqlCommand
'      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)

'      Dim rLOrec As SqlDataReader = cmd.ExecuteReader
'      Dim Time_1 As Double = System.Environment.TickCount

'      Lv.VirtualMode = True
'      Lv.VirtualListSize = GetLvDataCount(rLOrec)

'      Dim Time_2 As Double = System.Environment.TickCount
'      Console.WriteLine("Zeit für Listenaufbau: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


'    Catch e As Exception
'      Lv.Items.Clear()
'      MsgBox(e.Message, MsgBoxStyle.Critical, "frmMFakListSearch_LV -> FillrecData2Array")

'    Finally
'      Conn.Close()
'      Conn.Dispose()

'    End Try

'  End Sub

'  Private Sub LvFoundedrecs_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LvFoundedrecs.MouseDoubleClick
'    Dim iMANr As Integer = 0

'    Dim iIndex As Integer = Me.LvFoundedrecs.SelectedIndices(0)

'    iMANr = CInt(_ClsFunc.GetMANr)              ' MANr
'    RunOpenMAForm(iMANr)

'  End Sub

'  Sub OpenMyMA(ByVal sender As Object, ByVal e As EventArgs)
'    Dim iMANr As Integer = 0

'    iMANr = CInt(_ClsFunc.GetMANr)
'    RunOpenMAForm(iMANr)

'  End Sub

'  Private Sub LvFoundedrecs_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LvFoundedrecs.MouseDown

'    Dim strQuery As String = String.Empty
'    Dim xmlDoc As New Xml.XmlDocument()
'    Dim xpNav As XPathNavigator
'    Dim strUSLang As String = _ClsProgSetting.GetUSLanguage()
'    Dim strBez As String = String.Empty

'    If strUSLang <> String.Empty Then strUSLang = "_" & strUSLang
'    xmlDoc.Load(_ClsProgSetting.GetFormDataFile())
'    xpNav = xmlDoc.CreateNavigator()

'    Me.ContextMenuStrip1.Items.Clear()
'    _ClsFunc.GetMANr = "0"

'    Try
'      If Not LvFoundedrecs.GetItemAt(e.X, e.Y) Is Nothing Then
'        Console.WriteLine(LvFoundedrecs.HitTest(e.X, e.Y).Item.SubItems(3).Text)
'        _ClsFunc.GetMANr = LvFoundedrecs.HitTest(e.X, e.Y).Item.SubItems(1).Text

'        Dim cont As New ToolStripMenuItem

'        If Trim(LvFoundedrecs.HitTest(e.X, e.Y).Item.SubItems(1).Text) <> String.Empty Then
'          '_ClsFunc.GetMANr = LvFoundedrecs.HitTest(e.X, e.Y).Item.SubItems(1).Text
'          cont = New ToolStripMenuItem()
'          cont.Tag = "cMnuOpenMA"
'          strBez = TranslateMyText("Kandidatenmaske öffnen")

'          cont.Text = strBez
'          Me.ContextMenuStrip1.Items.Add(New ToolStripButton(cont.Text, Nothing, New EventHandler(AddressOf Me.OpenMyMA)))
'        End If

'        ' Leeres Menü wegen abgeschnittene Items
'        If Trim(LvFoundedrecs.HitTest(e.X, e.Y).Item.SubItems(1).Text) <> String.Empty Then
'          cont = New ToolStripMenuItem()
'          cont.Tag = ""
'          cont.Text = ""

'          Me.ContextMenuStrip1.Items.Add(New ToolStripButton(cont.Text, Nothing, New EventHandler(AddressOf Me.OpenMyMA)))
'        End If

'        Me.ContextMenuStrip1.Show(Me, New Point(Me.LvFoundedrecs.Width, e.Y))
'      End If

'    Catch ex As Exception
'      MessageBox.Show(ex.Message, "LVFoundedrecs_MouseDown", MessageBoxButtons.OK)

'    End Try

'  End Sub

'  Private Sub LvFoundedrecs_RetrieveVirtualItem(ByVal sender As Object, ByVal e As System.Windows.Forms.RetrieveVirtualItemEventArgs) Handles LvFoundedrecs.RetrieveVirtualItem

'    Dim lvItem As LvData = TotalLVData(e.ItemIndex)
'    Dim dTotalBetrag As Double = 0

'    Try
'      'Me.LblState_1.Text = "Bitte warten Sie einen Augenblick..."
'      Dim lviZahl As New ListViewItem( _
'          New String() { _
'              lvItem._Test.ToString, _
'              lvItem._MANr.ToString, _
'              lvItem._MAName.ToString, _
'              lvItem._Kanton.ToString, _
'              lvItem._Monat.ToString, _
'              lvItem._Basis.ToString, _
'              lvItem._Betrag.ToString _
'  })

'      ' Das ListView-Control erwartet wenn das Ereignis abgearbeitet ist in 
'      '  e.Item das ListViewItem, welches es angefordert hat
'      e.Item = lviZahl

'    Catch ex As Exception
'      MessageBox.Show(ex.Message, "LvFoundedrecs_RetrieveVirtualItem", MessageBoxButtons.OK)

'    End Try
'  End Sub

'#End Region


'#Region "Header erstellen..."

'  Sub SetLvwHeader()
'    Dim strColumnString As String = String.Empty
'    Dim strColumnWidthInfo As String = String.Empty
'    Dim strUSLang As String = String.Empty
'    If strUSLang <> String.Empty Then strUSLang = "_" & strUSLang

'    strColumnString = "Test;MANr;Name;Kanton;Monat/Jahr;Basis;Betrag"
'    strColumnWidthInfo = "0-1;200-1;500-0;"
'    strColumnWidthInfo += "200-0;200-0;200-1;200-1"

'    strColumnString = TranslateMyText(strColumnString)
'    FillDataHeaderLv(Me.LvFoundedrecs, strColumnString, strColumnWidthInfo)

'  End Sub

'  Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
'    Dim lstStuff As ListViewItem = New ListViewItem()
'    Dim lvwColumn As ColumnHeader

'    With Lv
'      .Clear()

'      ' Nr;Nummer;Name;Strasse;PLZ Ort
'      Dim strCaption As String() = Regex.Split(strColumnList, ";")
'      ' 0-1;0-1;2000-0;2000-0;2500-0
'      Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")

'      For i = 0 To strCaption.Length - 1
'        lvwColumn = New ColumnHeader()
'        lvwColumn.Text = strCaption(i).ToString

'        lvwColumn.Width = CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel
'        If CInt(Microsoft.VisualBasic.Right(strFieldInfo(i).ToString, 1)) = 1 Then
'          lvwColumn.TextAlign = HorizontalAlignment.Right
'        ElseIf CInt(Microsoft.VisualBasic.Right(strFieldInfo(i).ToString, 1)) = 2 Then
'          lvwColumn.TextAlign = HorizontalAlignment.Center
'        Else
'          lvwColumn.TextAlign = HorizontalAlignment.Left
'        End If

'        .Columns.Add(lvwColumn)
'      Next

'      lvwColumn = Nothing
'    End With

'  End Sub

'#End Region


'  Public Sub New(ByVal strQuery As String, ByVal LX As Integer, ByVal LY As Integer, ByVal lHeight As Integer)

'    _TotalBetrag = 0

'    ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
'    InitializeComponent()
'    Dim strForWidth As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\Coordination\" & _
'                                                       ClsDataDetail.GetAppGuidValue(), Me.Name & "_0")

'    Me.Width = CInt(Val(strForWidth))
'    Me.Height = lHeight
'    Me.Location = New Point(LX - Me.Width - 5, LY)
'    Trace.WriteLine("LX: " & LX.ToString & vbTab & "LY: " & LY.ToString & "Top: " & Me.Top.ToString & vbTab & "Left: " & Me.Left.ToString)

'    SetLvwHeader()
'    _ClsFunc.GetSearchQuery = strQuery
'    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
'    ' zur(Virtualisierung)
'    FillrecData2Array(Me.LvFoundedrecs, strQuery)

'  End Sub

'  Private Sub frmRPListSearch_LV_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
'    _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\Coordination\" & ClsDataDetail.GetAppGuidValue(), Me.Name & "_0", Me.Width.ToString)
'  End Sub

'  Private Sub btnTotalbetrag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTotalbetrag.Click
'    GetMenuItems4Show(Me.btnTotalbetrag, _TotalBetrag)
'  End Sub

'  Private Sub frmMFakListSearch_LV_Load(sender As Object, e As System.EventArgs) Handles Me.Load
'    Dim _ClsXML As New ClsXML
'    Dim Time_1 As Double = System.Environment.TickCount
'    _ClsXML.GetFormDataFromXML(Me, _ClsProgSetting.GetFormDataFile())
'  End Sub

'  Private Sub btnTotalbetrag_DropDownOpened(sender As Object, e As System.EventArgs) Handles btnTotalbetrag.DropDownOpened
'    Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
'    For Each itm As ToolStripItem In ts.DropDownItems
'      itm.Text = TranslateMyText(itm.Text)
'      Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
'    Next
'  End Sub

'End Class