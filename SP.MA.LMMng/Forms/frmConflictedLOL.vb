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
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng
Imports SP.DatabaseAccess.Employee.DataObjects.MonthlySalary

''' <summary>
''' Document management.
''' </summary>
Public Class frmConflictedLOL

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

		TranslateControls()

    gvLOL.Columns.Clear()

    Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
    columnLONr.Caption = m_Translate.GetSafeTranslationValue("LONr")
    columnLONr.Name = "LONr"
    columnLONr.FieldName = "LONr"
    columnLONr.Visible = True
    columnLONr.Width = 35
    gvLOL.Columns.Add(columnLONr)

    Dim columnLP As New DevExpress.XtraGrid.Columns.GridColumn()
    columnLP.Caption = m_Translate.GetSafeTranslationValue("Monat")
    columnLP.Name = "LP"
    columnLP.FieldName = "LP"
    columnLP.Visible = True
    columnLP.Width = 35
    columnLP.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    columnLP.AppearanceHeader.Options.UseTextOptions = True
    columnLP.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
    columnLP.AppearanceCell.Options.UseTextOptions = True
    gvLOL.Columns.Add(columnLP)

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
    gvLOL.Columns.Add(columnYear)

  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' shows the conflicted LOL data.
  ''' </summary>
  Public Sub ShowConflicts(ByVal conflicts As IEnumerable(Of ConflictedLOLData))

    ' Reset the grid



    grdLOL.DataSource = conflicts

    ShowDialog()

  End Sub


#End Region

#Region "Private Methods"

	''' <summary>
	''' Translate the controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.lblInfo.Text = m_Translate.GetSafeTranslationValue(Me.lblInfo.Text)
		Me.btnOK.Text = m_Translate.GetSafeTranslationValue(Me.btnOK.Text)

	End Sub

  ''' <summary>
  ''' Handles click on ok button.
  ''' </summary>
  Private Sub OnBtnOK_Click(sender As System.Object, e As System.EventArgs) Handles btnOK.Click
    Close()
  End Sub

#End Region


End Class