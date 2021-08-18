
Option Explicit On
Public Class myCbo
  Inherits System.Windows.Forms.ComboBox
  'APIs
  <System.Runtime.InteropServices.DllImport("user32.dll")> Private Shared Function SendMessage(ByVal hWnd As System.IntPtr, ByVal Msg As Integer, ByVal wParam As System.Int32, ByVal lParam As System.IntPtr) As Integer
  End Function
  'properties
  Private mDividerFormat As String = ""
  Private mGroupColor As Color = System.Drawing.SystemColors.WindowText
  Private mItemsChecks() As System.Windows.Forms.CheckState
  Private mItemsChecks_Temp() As System.Windows.Forms.CheckState
  Private mCheckBoxes As Boolean
  Private mChecked As System.Windows.Forms.CheckState = CheckState.Unchecked
  Private mGridColor As Color = Color.FromArgb(240, 248, 255)
  'vars - last selected item
  Private mItemSeparator1 As Char = ","c
  Private mItemSeparator2 As Char = "&"c
  Private mHoverIndex As Int32
  Private mHoverIndex_Dec As Double
  'events
  Event ItemHover(ByVal eIndex As Int32)
  Event ItemChecked(ByVal eIndex As Int32)
  'vars
  Private mLastSelectedIndex As Int32 = -1
  Private mTimer As Timer
  Private mFirerTimer As Int32
  Private mKillEvents1 As Int32 ' kills _SelectectedIndexChange   ON ForceRedraw
  Private mKillEvents2 As Int32 ' kills _OnDrawItem               ON FroceRedraw
  Private mKillEvents3 As Int32 ' kills .ItemsChecks              ON Click Selecting (not programatic selecting of .ItemsChecks)
  'constructor
  Public Sub New()
		Me.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
	End Sub
  'properties
  <System.ComponentModel.Description("Use this property to set divider flag.  Recommend you use three hyphens ---.")> _
  <System.ComponentModel.Category("Power Properties")> Public Property DividerFormat() As String
    Get
      Return mDividerFormat
    End Get
    Set(ByVal value As String)
      mDividerFormat = value
    End Set
  End Property
  <System.ComponentModel.Description("Use this property to set the ForeColor of the grouping text.")> _
  <System.ComponentModel.Category("Power Properties")> Public Property GroupColor() As Color

    Get
      Return mGroupColor
    End Get
    Set(ByVal value As Color)
      mGroupColor = value
    End Set
  End Property
  <System.ComponentModel.Description("Use this property to set the BackColor of the grid.")> _
  <System.ComponentModel.Category("Power Properties")> Public Property GridColor() As Color
    Get
      Return mGridColor
    End Get
    Set(ByVal value As Color)
      mGridColor = value
    End Set
  End Property
  <System.ComponentModel.Description("Use this property to get/set corresponding checkboc values.")> _
  <System.ComponentModel.Category("Power Properties")> Public Property ItemsChecks(ByVal xIndex As Int32) As System.Windows.Forms.CheckState
    Get
      Try
        Return mItemsChecks(xIndex)
      Catch ex As Exception
        Return CheckState.Unchecked
      End Try

    End Get

    Set(ByVal value As System.Windows.Forms.CheckState)

      If xIndex < 0 Or xIndex > Me.Items.Count Then Exit Property

      If mKillEvents3 <> 0 Then
        mItemsChecks(xIndex) = value
      Else
        If Me.CheckBoxes Then
          If Me.DroppedDown Then
            If Me.SelectedIndex = xIndex Then Me.SelectedIndex = -1
            Me.SelectedIndex = xIndex
            PrepareTimer()
          Else

            If mItemsChecks Is Nothing Then
              ReDim Preserve mItemsChecks_Temp(Me.Items.Count - 1)
              mItemsChecks_Temp(xIndex) = value
            Else
              mItemsChecks(xIndex) = value
            End If
            CommitCheckList()
          End If
        End If
      End If

    End Set
  End Property
  <System.ComponentModel.Description("Use this property to enable checkboxes.")> _
  <System.ComponentModel.Category("Power Properties")> Public Property CheckBoxes() As Boolean
    Get
      Return mCheckBoxes
    End Get
    Set(ByVal value As Boolean)
      mCheckBoxes = value
    End Set
  End Property
  <System.ComponentModel.Description("Use this property to set CheckBox's default value.")> _
  <System.ComponentModel.Category("Power Properties")> Public Property Checked() As System.Windows.Forms.CheckState
    Get
      Return mChecked
    End Get
    Set(ByVal value As System.Windows.Forms.CheckState)
      mChecked = value
    End Set
  End Property
  <System.ComponentModel.Description("Use this property to set item separator1 character.")> _
  <System.ComponentModel.Category("Power Properties")> Public Property ItemSeparator1() As Char
    Get
      Return mItemSeparator1
    End Get
    Set(ByVal value As Char)
      mItemSeparator1 = value
    End Set
  End Property
  <System.ComponentModel.Description("Use this property to set item separator2 character.")> _
  <System.ComponentModel.Category("Power Properties")> Public Property ItemSeparator2() As Char
    Get
      Return mItemSeparator2
    End Get
    Set(ByVal value As Char)
      mItemSeparator2 = value
    End Set
  End Property
  'overrides
  Protected Overrides Sub OnSelectedIndexChanged(ByVal e As System.EventArgs)

    Dim i As Int32

    If mKillEvents1 <> 0 Or SelectedIndex = -1 Then Exit Sub

    If Me.DividerFormat.Length > 0 AndAlso IsItemAGroup(SelectedIndex) Then
      If Not Me.CheckBoxes Then
        Me.SelectedIndex = mLastSelectedIndex
        Exit Sub
      Else
        mKillEvents3 += 1
        i = SelectedIndex
        Do
          i += 1
          If i > (Me.Items.Count - 1) Then Exit Do
          If IsItemAGroup(i) Then Exit Do
          Me.SelectedIndex = i
        Loop
        mKillEvents3 -= 1
        MyBase.OnSelectedIndexChanged(e)
        Exit Sub
      End If
    End If

    '2 - standard event stuff
    mLastSelectedIndex = Me.SelectedIndex

    MyBase.OnSelectedIndexChanged(e)

    '3 - toggle checkbox/force redraw
    If Me.CheckBoxes AndAlso SelectedIndex > -1 Then
      mKillEvents3 += 1
      If Me.ItemsChecks(Me.SelectedIndex) = CheckState.Checked Then
        Me.ItemsChecks(Me.SelectedIndex) = CheckState.Unchecked
      Else
        Me.ItemsChecks(Me.SelectedIndex) = CheckState.Checked
      End If
      mKillEvents3 -= 1
      PrepareTimer()
    End If

  End Sub
  Protected Overrides Sub OnDrawItem(ByVal e As DrawItemEventArgs)

    Dim zX1 As Int32
    Dim zPen As Pen
    Dim zWidth As Single
    Dim zText As String
    Dim zFont As Font
    Dim zFore As Color
    Dim zState As System.Windows.Forms.VisualStyles.CheckBoxState

    '1 - Exit
    If e.Index < 0 Then
      MyBase.OnDrawItem(e)
      Exit Sub
    End If

    '2 - Grouping
    If Me.Items(e.Index).ToString.Contains(Me.mDividerFormat) And mDividerFormat.Length > 0 Then
      e.DrawBackground()
      zText = Me.Items(e.Index).ToString
      'zText = "" + zText.Replace(Me.mDividerFormat, "") + ""
      zText = " " + zText.Replace(Me.mDividerFormat, "") + " "
      zFont = New Font(Font, FontStyle.Bold)
      If e.BackColor = System.Drawing.SystemColors.Highlight Then
        zFore = Color.Gainsboro
      Else
        zFore = Me.GroupColor
      End If
      zPen = New Pen(zFore)
      zWidth = e.Graphics.MeasureString(zText, zFont).Width
      zX1 = Convert.ToInt32(e.Bounds.Width - zWidth) \ 2
      e.Graphics.DrawRectangle(zPen, New Rectangle(e.Bounds.X, e.Bounds.Y + e.Bounds.Height \ 2, zX1, 1))
      e.Graphics.DrawRectangle(zPen, New Rectangle(e.Bounds.Width - zX1, e.Bounds.Y + e.Bounds.Height \ 2, e.Bounds.Width, 1))
      e.Graphics.DrawString(zText, zFont, New SolidBrush(zFore), zX1, e.Bounds.Top)
    Else

      '3 - ItemBackColor
      If mKillEvents2 = 0 Then
        Select Case True
          Case System.Convert.ToBoolean(e.State And DrawItemState.Selected)
            e.DrawBackground()
          Case e.Index Mod 2 = 0
            e.Graphics.FillRectangle(New SolidBrush(Color.White), e.Bounds)
          Case Else
            e.Graphics.FillRectangle(New SolidBrush(mGridColor), e.Bounds)
        End Select
      End If
      '4 - ItemText ( _SearchPoint1)
      e.Graphics.DrawString(Me.Items(e.Index).ToString, Font, New SolidBrush(e.ForeColor), e.Bounds.Left, e.Bounds.Top)
      '5 - CheckBox
      If mCheckBoxes Then
        If System.Convert.ToBoolean(e.State And DrawItemState.Selected) Then
          If Me.ItemsChecks(e.Index) = CheckState.Checked Then
            zState = VisualStyles.CheckBoxState.CheckedHot
          Else
            zState = VisualStyles.CheckBoxState.UncheckedHot
          End If
        Else
          If Me.ItemsChecks(e.Index) = CheckState.Checked Then
            zState = VisualStyles.CheckBoxState.CheckedNormal
          Else
            zState = VisualStyles.CheckBoxState.UncheckedNormal
          End If
        End If
        zX1 = Me.FontHeight
        zPen = New Pen(Color.Black, 1)
        zPen.DashStyle = Drawing2D.DashStyle.Dot
				System.Windows.Forms.CheckBoxRenderer.DrawCheckBox(e.Graphics, New System.Drawing.Point(e.Bounds.X + e.Bounds.Width - 15, e.Bounds.Y + 1 + ((e.Bounds.Height - 13) \ 2)), e.Bounds, "", Me.Font, False, zState)
			End If
    End If

    '6 - Base event
    MyBase.OnDrawItem(e)

  End Sub
  Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)

    Const WM_SETCURSOR As Int32 = 32
    Const WM_COMMAND As Int32 = 273
    Const WM_CTLCOLORLISTBOX As Integer = 308
    Const CB_ADDSTRING As Int32 = 323
    Const CB_GETCURSEL As Int32 = 327
    Const WM_LBUTTONUP As Int32 = 514
    Const OCM_COMMAND As Int32 = 8465

    Select Case True
      Case m.Msg = WM_CTLCOLORLISTBOX Or m.Msg = WM_SETCURSOR
        GetHoverIndex()
        If mHoverIndex_Dec >= 0 Then
          If mHoverIndex > -1 And mHoverIndex < Me.Items.Count And Me.DroppedDown Then
            RaiseEvent ItemHover(mHoverIndex)
          End If
        End If

      Case m.Msg = CB_ADDSTRING
        MyBase.WndProc(m)
        StretchCheckList()
        Exit Sub

      Case m.Msg = WM_COMMAND And m.WParam = New System.IntPtr(66536)

        '1 - normal behaviour when no checkboxes
        If Not Me.mCheckBoxes Then
          MyBase.WndProc(m)
          Exit Sub
        End If

        '2 - (NEW) nulls MouseWheel on not .DropDown
        If Not Me.DroppedDown Then
          Exit Sub
        End If

        '3 - ClickEvent reconstruction (cancelled) child events from WM_COMMAND MSG
        myCbo.SendMessage(Me.Handle, OCM_COMMAND, 591814, New IntPtr(1705748))  '1 
        myCbo.SendMessage(Me.Handle, OCM_COMMAND, 67526, New IntPtr(1705748))   '2 SelectedIndexChange
        myCbo.SendMessage(Me.Handle, CB_GETCURSEL, 0, New IntPtr(0))            '3 
        myCbo.SendMessage(Me.Handle, WM_LBUTTONUP, 0, New IntPtr(721012))       '4 

        '4 - cancels event
        Exit Sub
    End Select

    MyBase.WndProc(m)

  End Sub
  'subs
  Private Sub PrepareTimer()

    Try

      mTimer = Nothing
      mTimer = New Timer
      mTimer.Interval = 64
      mTimer.Enabled = True
      AddHandler mTimer.Tick, AddressOf mTimer_Tick
      mFirerTimer = 1

    Catch ex As Exception

    End Try

  End Sub
  Private Sub mTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs)
    If mFirerTimer > 0 Then
      mTimer.Enabled = False
      mTimer.Dispose()
      ForceRedraw()
      Me.Text = GetCommaText()
      mFirerTimer = 0
      RaiseEvent ItemChecked(SelectedIndex)
    End If
  End Sub
  Public Sub StretchCheckList()
    Dim i As Int32
    ReDim Preserve mItemsChecks(Me.Items.Count - 1)
    '1. suck in temp
    If Not mItemsChecks_Temp Is Nothing Then
      mItemsChecks = mItemsChecks_Temp
      mItemsChecks_Temp = Nothing
      '2. formats list with pre-defined format
    Else
      If mChecked <> CheckState.Checked Then Exit Sub
      For i = 0 To mItemsChecks.GetUpperBound(0)
        If Not IsItemAGroup(i) Then mItemsChecks(i) = CheckState.Checked
      Next
    End If
    Me.Text = GetCommaText()
  End Sub
  Public Sub CommitCheckList()
    mItemsChecks = mItemsChecks_Temp
    Me.Text = GetCommaText()
  End Sub
  Private Sub GetHoverIndex()
    Dim yPos As Int32
    yPos = Me.PointToClient(Cursor.Position).Y
    If Me.DropDownStyle = ComboBoxStyle.Simple Then
      yPos -= (Me.ItemHeight + 10)
    Else
      yPos -= (Me.Size.Height + 1)
    End If
    mHoverIndex_Dec = yPos / Me.ItemHeight
    mHoverIndex = System.Convert.ToInt32(Math.Floor(mHoverIndex_Dec))
  End Sub
  Private Sub ForceRedraw()
    If Me.Items.Count > 0 Then
      mKillEvents1 += 1
      mKillEvents2 += 1
      Me.SelectedIndex -= 1
      mKillEvents2 -= 1
      Me.SelectedIndex += 1
      mKillEvents1 -= 1
    End If
  End Sub
  'functions
  Public Function IDsToCSV(Optional ByVal xZeroBound As Boolean = True) As String
    Dim i As Int32
    Dim c As Int32
    Dim a As Int32
    Dim sb As New System.Text.StringBuilder("")
    Dim s As String

    If xZeroBound Then a = 0 Else a = 1

    For i = 0 To Me.Items.Count - 1
      If IsItemAGroup(i) Then Continue For
      If Me.ItemsChecks(i) = CheckState.Checked Then
        If Me.mDividerFormat.Length > 0 Then
          sb.Append((c + a).ToString + ",")
        Else
          sb.Append((i + a).ToString + ",")
        End If
      End If

      c += 1
    Next

    s = sb.ToString
    If s.Length > 0 Then s = s.Substring(0, s.Length - 1)
    Return s
  End Function
  Public Function IDsToBITSHIFT() As String
    Dim i As Int32
    Dim c As Int32 = 1
    Dim a As Int32

    For i = 0 To Me.Items.Count - 1
      If IsItemAGroup(i) Then Continue For
      If Me.ItemsChecks(i) <> CheckState.Checked Then
        a += c
      End If
      c = (c * 2)
    Next
    Return a.ToString

  End Function
  Public Function TextToCSV() As String
    Dim i As Int32
    Dim sb As New System.Text.StringBuilder("")
    Dim s As String
    For i = 0 To Me.Items.Count - 1
      If IsItemAGroup(i) Then Continue For
      If Me.ItemsChecks(i) = CheckState.Checked Then
        sb.Append(Me.Items(i).ToString + ",")
      End If
    Next
    s = sb.ToString
    If s.Length > 0 Then s = s.Substring(0, s.Length - 1)
    Return s
  End Function
  Private Function GetCommaText() As String
    Dim i As Int32
    Dim sb As New System.Text.StringBuilder("")
    Dim s As String
    Dim zFirst As String
    Dim zLast As String
    Dim zLastComa As Int32
    Dim zSpace As String = " "
    If Not Me.CheckBoxes Then Return Me.Text
    For i = 0 To Me.Items.Count - 1
      If IsItemAGroup(i) Then Continue For
      If Me.ItemsChecks(i) = CheckState.Checked Then
        sb.Append(Me.Items(i).ToString)
        sb.Append(mItemSeparator1)
      End If
    Next
    s = sb.ToString
    If s.Length = 0 Then Return ""
    s = s.Substring(0, s.Length - 1)
    zLastComa = s.LastIndexOf(mItemSeparator1)

    If zLastComa <> -1 Then
      zLast = s.Substring(zLastComa)
      zFirst = s.Substring(0, zLastComa)
      s = zFirst + zSpace + zLast.Replace(Convert.ToString(mItemSeparator1), Convert.ToString(mItemSeparator2) + zSpace)
      Return s
    Else
      Return s + " "
    End If
  End Function
  Private Function IsItemAGroup(ByVal xIndex As Int32) As Boolean
    If Me.mDividerFormat.Length > 0 AndAlso Me.Items(xIndex).ToString.Contains(Me.mDividerFormat) Then Return True
  End Function
  Public Sub CheckAll(ByVal xChecked As CheckState)
    Dim i As Int32
    For i = 0 To Me.Items.Count - 1
      Me.ItemsChecks(i) = xChecked
      RaiseEvent ItemChecked(SelectedIndex)
    Next
    Me.Text = GetCommaText()
  End Sub
End Class