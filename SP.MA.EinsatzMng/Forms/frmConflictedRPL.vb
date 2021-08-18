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
''' Shows conflicted RPL records..
''' </summary>
Public Class frmConflictedRPL

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

    gvRPL.Columns.Clear()

    Dim columnRPLNr As New DevExpress.XtraGrid.Columns.GridColumn()
    columnRPLNr.Caption = m_Translate.GetSafeTranslationValue("RPNr")
    columnRPLNr.Name = "RPNr"
    columnRPLNr.FieldName = "RPNr"
    columnRPLNr.Visible = True
    columnRPLNr.Width = 35
    gvRPL.Columns.Add(columnRPLNr)


    Dim columnVon As New DevExpress.XtraGrid.Columns.GridColumn()
    columnVon.Caption = m_Translate.GetSafeTranslationValue("Von Datum")
    columnVon.Name = "VonDate"
    columnVon.FieldName = "VonDate"
    columnVon.Visible = True
    gvRPL.Columns.Add(columnVon)


    Dim columnBis As New DevExpress.XtraGrid.Columns.GridColumn()
    columnBis.Caption = m_Translate.GetSafeTranslationValue("Bis Datum")
    columnBis.Name = "BisDate"
    columnBis.FieldName = "BisDate"
    columnBis.Visible = True
    gvRPL.Columns.Add(columnBis)

  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' shows the conflicted RPL data.
  ''' </summary>
  Public Sub ShowConflicts(ByVal conflicts As IEnumerable(Of ConflictedRPLData))

    ' Reset the grid

    grdRPL.DataSource = conflicts

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