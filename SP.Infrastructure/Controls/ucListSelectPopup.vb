Imports DevExpress.XtraGrid.Columns
Imports System.Windows.Forms
Imports System.Drawing

Public Delegate Sub RowClickedHandler(ByVal sender As Object, ByVal clickedObject As Object)
Public Delegate Sub SizeChangedHandler(ByVal sender As Object, ByVal newHeight As Integer, ByVal newHeight As Integer)

''' <summary>
''' List select popup.
''' </summary>
Public Class ucListSelectPopup

#Region "Private Fields"

  ''' <summary>
  ''' The popup control container.
  ''' </summary>
  Private m_PopupContainer As DevExpress.XtraBars.PopupControlContainer

  ''' <summary>
  ''' The grid control.
  ''' </summary>
  Private m_Grid As DevExpress.XtraGrid.GridControl

  ''' <summary>
  ''' The grid view.
  ''' </summary>
  Private m_GridView As DevExpress.XtraGrid.Views.Grid.GridView

#End Region

#Region "Events"

  Public Event RowClicked As RowClickedHandler
  Public Event PopupSizeChanged As SizeChangedHandler

#End Region

#Region "Public Properties"

  ''' <summary>
  ''' Gets a boolean flag indicating if the popup has been initialized.
  ''' </summary>
  ''' <value></value>
  ''' <returns>Boolean flag indicating if the popup has been initialzed.</returns>
  Public ReadOnly Property IsIntialized As Boolean
    Get
      Return Not m_PopupContainer Is Nothing
    End Get
  End Property

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Inits the popup.
  ''' </summary>
  ''' <param name="dataSource">The data source.</param>
  ''' <param name="columnDefintions">The column definitions.</param>
  Public Sub InitPopup(ByVal dataSource As Object, ByVal columnDefintions As IEnumerable(Of PopupColumDefintion))

    If Not IsIntialized Then

      ' --- Create the Popup controls ----
      m_PopupContainer = New DevExpress.XtraBars.PopupControlContainer
      m_PopupContainer.Size = New Size(Width, Height)
      m_PopupContainer.SuspendLayout()
      m_PopupContainer.Manager = New DevExpress.XtraBars.BarManager
      m_PopupContainer.ShowCloseButton = True
      m_PopupContainer.ShowSizeGrip = True
      Me.Controls.Add(m_PopupContainer)

      m_Grid = New DevExpress.XtraGrid.GridControl
      m_Grid.Dock = DockStyle.Fill
      m_Grid.MainView = m_Grid.CreateView("MainView")
      m_Grid.ForceInitialize()
      m_Grid.Visible = False
      m_PopupContainer.Controls.Add(m_Grid)

      AddHandler m_PopupContainer.SizeChanged, AddressOf OnPopup_SizeChanged
      AddHandler m_Grid.DoubleClick, AddressOf OnPopup_RowClick

      m_GridView = TryCast(m_Grid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

      m_GridView.OptionsBehavior.Editable = False
      m_GridView.OptionsSelection.EnableAppearanceFocusedCell = False
      m_GridView.OptionsSelection.InvertSelection = False
      m_GridView.OptionsSelection.EnableAppearanceFocusedRow = True
      m_GridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
      m_GridView.OptionsView.ShowGroupPanel = False

      m_Grid.Visible = True

      m_PopupContainer.ResumeLayout()

    End If

    m_GridView.Columns.Clear()

    ' Create columns
    For Each columnDefintion In columnDefintions

      Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
      If Not (columnDefintion.AutoFilterCondition Is Nothing) Then columnName.OptionsFilter.AutoFilterCondition = columnDefintion.AutoFilterCondition



      columnName.Caption = columnDefintion.Translation
      columnName.Name = columnDefintion.Name
      columnName.FieldName = columnDefintion.Name
      columnName.Visible = True

      m_GridView.Columns.Add(columnName)

    Next

    m_Grid.DataSource = dataSource
  End Sub

  ''' <summary>
  ''' Inits the popup.
  ''' </summary>
  ''' <param name="dataSource">The data source.</param>
  ''' <param name="columnDefintions">The column definitions.</param>
  Public Sub InitPopup(ByVal dataSource As Object, ByVal columnDefintions As IEnumerable(Of PopupColumDefintion), ByVal ShowIndicator As Boolean?,
                       ByVal ShowAutoFilterCondition As Boolean?,
                       ByVal ShowFilterPanelMode As DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode?)

    If Not IsIntialized Then

      ' --- Create the Popup controls ----
      m_PopupContainer = New DevExpress.XtraBars.PopupControlContainer
      m_PopupContainer.Size = New Size(Width, Height)
      m_PopupContainer.SuspendLayout()
      m_PopupContainer.Manager = New DevExpress.XtraBars.BarManager
      m_PopupContainer.ShowCloseButton = True
      m_PopupContainer.ShowSizeGrip = True
      Me.Controls.Add(m_PopupContainer)

      m_Grid = New DevExpress.XtraGrid.GridControl
      m_Grid.Dock = DockStyle.Fill
      m_Grid.MainView = m_Grid.CreateView("MainView")
      m_Grid.ForceInitialize()
      m_Grid.Visible = False
      m_PopupContainer.Controls.Add(m_Grid)

      AddHandler m_PopupContainer.SizeChanged, AddressOf OnPopup_SizeChanged
      AddHandler m_Grid.DoubleClick, AddressOf OnPopup_RowClick

      m_GridView = TryCast(m_Grid.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

      m_GridView.OptionsBehavior.Editable = False
      m_GridView.OptionsSelection.EnableAppearanceFocusedCell = False
      m_GridView.OptionsSelection.InvertSelection = False
      m_GridView.OptionsSelection.EnableAppearanceFocusedRow = True
      m_GridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus
      m_GridView.OptionsView.ShowGroupPanel = False

      If Not (showindicator Is Nothing) Then m_GridView.OptionsView.ShowIndicator = showindicator
      If Not (ShowIndicator Is Nothing) Then m_GridView.OptionsView.ShowAutoFilterRow = ShowAutoFilterCondition
      If Not (ShowIndicator Is Nothing) Then m_GridView.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode

      m_Grid.Visible = True

      m_PopupContainer.ResumeLayout()

    End If

    m_GridView.Columns.Clear()

    ' Create columns
    For Each columnDefintion In columnDefintions

      Dim columnName As New DevExpress.XtraGrid.Columns.GridColumn()
      If Not (columnDefintion.AutoFilterCondition Is Nothing) Then columnName.OptionsFilter.AutoFilterCondition = columnDefintion.AutoFilterCondition



      columnName.Caption = columnDefintion.Translation
      columnName.Name = columnDefintion.Name
      columnName.FieldName = columnDefintion.Name
      columnName.Visible = True

      m_GridView.Columns.Add(columnName)

    Next

    m_Grid.DataSource = dataSource
  End Sub

  ''' <summary>
  ''' Show the popup if it has been initialized.
  ''' </summary>
  Public Sub ShowPopup(ByVal location As Point, ByVal size As Size)
    If IsIntialized Then
      m_PopupContainer.Size = size
      m_PopupContainer.ShowPopup(location)
    End If
  End Sub

  ''' <summary>
  ''' Hides the popup if it has been initialized.
  ''' </summary>
  Public Sub HidePopup()
    If IsIntialized Then
      m_PopupContainer.HidePopup()
    End If
  End Sub

  ''' <summary>
  ''' Handles poupup size change.
  ''' </summary>
  Private Sub OnPopup_SizeChanged(sender As Object, _
                                  e As System.EventArgs)
    RaiseEvent PopupSizeChanged(Me, sender.Size.Width, sender.Size.Height)

  End Sub

  ''' <summary>
  ''' Handles popup row click.
  ''' </summary>
  Private Sub OnPopup_RowClick(sender As Object, e As System.EventArgs)

    Dim success = True
    Dim clickedObject As Object = Nothing

    Dim grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

    If Not (grdView Is Nothing) Then
      Dim selectedRows = grdView.GetSelectedRows()

      If (selectedRows.Count > 0) Then
        clickedObject = grdView.GetRow(selectedRows(0))
      End If
    End If

    RaiseEvent RowClicked(Me, clickedObject)

  End Sub

#End Region

#Region "Helper classes"

  ''' <summary>
  ''' Popup column definition.
  ''' </summary>
  Public Class PopupColumDefintion

    Public Property Name As String
    Public Property Translation As String
    Public Property AutoFilterCondition As DevExpress.XtraGrid.Columns.AutoFilterCondition?

  End Class

#End Region

End Class
