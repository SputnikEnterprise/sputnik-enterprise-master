Imports SP.DatabaseAccess
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports DevExpress.XtraEditors.Controls
Imports System.ComponentModel
Imports SP.Infrastructure
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository
Imports SP.DatabaseAccess.ES.DataObjects.ESMng

''' <summary>
''' Document management.
''' </summary>
Public Class frmConflictedMonthClose

#Region "Private Fields"

  ''' <summary>
  ''' The translation value helper.
  ''' </summary>
  Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

#End Region

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  Public Sub New(ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper)

    ' Dieser Aufruf ist für den Designer erforderlich.
    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    m_Translate = translate

    gvMonthCloseRecords.Columns.Clear()

    Dim columnLP As New DevExpress.XtraGrid.Columns.GridColumn()
    columnLP.Caption = m_Translate.GetSafeTranslationValue("Monat")
    columnLP.Name = "Month"
    columnLP.FieldName = "Month"
    columnLP.Visible = True
    columnLP.Width = 35
    columnLP.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    columnLP.AppearanceHeader.Options.UseTextOptions = True
    columnLP.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    columnLP.AppearanceCell.Options.UseTextOptions = True
    gvMonthCloseRecords.Columns.Add(columnLP)

    Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
    columnYear.Caption = m_Translate.GetSafeTranslationValue("Jahr")
    columnYear.Name = "Year"
    columnYear.FieldName = "Year"
    columnYear.Visible = True
    columnYear.Width = 35
    columnYear.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    columnYear.AppearanceHeader.Options.UseTextOptions = True
    columnYear.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    columnYear.AppearanceCell.Options.UseTextOptions = True
    gvMonthCloseRecords.Columns.Add(columnYear)

  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' shows the conflicted MonthClsoe data.
  ''' </summary>
  Public Sub ShowConflicts(ByVal conflicts As IEnumerable(Of ConflictedMonthCloseData))

    ' Reset the grid

    grdMonthCloseRecords.DataSource = conflicts

    ShowDialog()

  End Sub


#End Region

#Region "Private Methods"

  ''' <summary>
  ''' Handles click on ok button.
  ''' </summary>
  Private Sub OnBtnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
    Close()
  End Sub

#End Region


End Class