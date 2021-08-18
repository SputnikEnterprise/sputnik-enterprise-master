
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Imports DevExpress.LookAndFeel

Imports SP.DatabaseAccess

Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.MandantData


Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages


Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraSplashScreen
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.Initialization
Imports System.ComponentModel
Imports DevExpress.XtraGrid.Views.Base



Public Class frmLATranslation


#Region "Private Fields"


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess
	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_MandantDatabaseAccess As MandantData

	Private m_SuppressUIEvents As Boolean

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private connectionString As String

	Private m_MandantXMLFile As String
	Private m_MandantFormXMLFileName As String
	Private m_MandantSetting As String
	Private m_FibuSetting As String

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_ProgPath As ClsProgPath

	Private m_Year As Integer

#End Region


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_FIBU_SETTING As String = "MD_{0}/BuchungsKonten"

#End Region


#Region "public property"

	Public Property IsDataValid As Boolean


#End Region


#Region "Constructor"


	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_SuppressUIEvents = True
		InitializeComponent()
		m_SuppressUIEvents = False

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = ClsDataDetail.m_InitialData

		IsDataValid = True
		m_mandant = New Mandant
		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_ProgPath = New ClsProgPath

		If m_InitializationData Is Nothing Then Exit Sub


		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)

		Try

			connectionString = m_InitializationData.MDData.MDDbConn
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			IsDataValid = False

		End Try


		Reset()

		TranslateControls()

	End Sub



#End Region


	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		bsiLblRecCount.Caption = m_Translate.GetSafeTranslationValue(bsiLblRecCount.Caption)


	End Sub

	Private Sub Reset()

		Dim suppressState = m_SuppressUIEvents
		m_SuppressUIEvents = True

		' Automatische Debitoren
		ResetLohnartGrid()

		m_SuppressUIEvents = False

	End Sub


	Private Sub ResetLohnartGrid()

		gvLATranslated.OptionsView.ShowIndicator = False
		gvLATranslated.OptionsView.ShowAutoFilterRow = True
		gvLATranslated.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvLATranslated.OptionsView.ShowFooter = True

		gvLATranslated.Columns.Clear()


		Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnrecid.Name = "recid"
		columnrecid.FieldName = "recid"
		columnrecid.Visible = False
		gvLATranslated.Columns.Add(columnrecid)

		Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLANr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLANr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnLANr.Name = "LANumber"
		columnLANr.FieldName = "LANumber"
		columnLANr.Width = 50
		columnLANr.Visible = True
		gvLATranslated.Columns.Add(columnLANr)


		Dim columnLAText As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLAText.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLAText.Caption = m_Translate.GetSafeTranslationValue("Name")
		columnLAText.Name = "LAName"
		columnLAText.FieldName = "LAName"
		columnLAText.Visible = True
		columnLAText.BestFit()
		gvLATranslated.Columns.Add(columnLAText)


		Dim columnName_I As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName_I.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnName_I.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (IT)")
		columnName_I.Name = "Name_I"
		columnName_I.FieldName = "Name_I"
		columnName_I.Visible = True
		columnName_I.BestFit()
		gvLATranslated.Columns.Add(columnName_I)

		Dim columnName_F As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName_F.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnName_F.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (FR)")
		columnName_F.Name = "Name_F"
		columnName_F.FieldName = "Name_F"
		columnName_F.Visible = True
		columnName_F.BestFit()
		gvLATranslated.Columns.Add(columnName_F)

		Dim columnName_E As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName_E.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnName_E.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung (EN)")
		columnName_E.Name = "Name_E"
		columnName_E.FieldName = "Name_E"
		columnName_E.Visible = True
		columnName_E.BestFit()
		gvLATranslated.Columns.Add(columnName_E)

		Dim columnName_OP_I As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName_OP_I.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnName_OP_I.Caption = m_Translate.GetSafeTranslationValue("Rechnung: Bezeichnung (IT)")
		columnName_OP_I.Name = "Name_OP_I"
		columnName_OP_I.FieldName = "Name_OP_I"
		columnName_OP_I.Visible = True
		columnName_OP_I.BestFit()
		gvLATranslated.Columns.Add(columnName_OP_I)

		Dim columnName_OP_F As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName_OP_F.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnName_OP_F.Caption = m_Translate.GetSafeTranslationValue("Rechnung: Bezeichnung (FR)")
		columnName_OP_F.Name = "Name_OP_F"
		columnName_OP_F.FieldName = "Name_OP_F"
		columnName_OP_F.Visible = True
		columnName_OP_F.BestFit()
		gvLATranslated.Columns.Add(columnName_OP_F)

		Dim columnName_OP_E As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName_OP_E.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnName_OP_E.Caption = m_Translate.GetSafeTranslationValue("Rechnung: Bezeichnung (EN)")
		columnName_OP_E.Name = "Name_OP_E"
		columnName_OP_E.FieldName = "Name_OP_E"
		columnName_OP_E.Visible = True
		columnName_OP_E.BestFit()
		gvLATranslated.Columns.Add(columnName_OP_E)

		Dim columnName_LO_I As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName_LO_I.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnName_LO_I.Caption = m_Translate.GetSafeTranslationValue("Lohnabrechnung: Bezeichnung (IT)")
		columnName_LO_I.Name = "Name_LO_I"
		columnName_LO_I.FieldName = "Name_LO_I"
		columnName_LO_I.Visible = True
		columnName_LO_I.BestFit()
		gvLATranslated.Columns.Add(columnName_LO_I)

		Dim columnName_LO_F As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName_LO_F.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnName_LO_F.Caption = m_Translate.GetSafeTranslationValue("Lohnabrechnung: Bezeichnung (FR)")
		columnName_LO_F.Name = "Name_LO_F"
		columnName_LO_F.FieldName = "Name_LO_F"
		columnName_LO_F.Visible = True
		columnName_LO_F.BestFit()
		gvLATranslated.Columns.Add(columnName_LO_F)

		Dim columnName_LO_E As New DevExpress.XtraGrid.Columns.GridColumn()
		columnName_LO_E.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnName_LO_E.Caption = m_Translate.GetSafeTranslationValue("Lohnabrechnung: Bezeichnung (EN)")
		columnName_LO_E.Name = "Name_LO_E"
		columnName_LO_E.FieldName = "Name_LO_E"
		columnName_LO_E.Visible = True
		columnName_LO_E.BestFit()
		gvLATranslated.Columns.Add(columnName_LO_E)

		grdLATranslated.DataSource = Nothing

	End Sub

	Public Function LoadLAStammTranslationData() As Boolean
		Dim success As Boolean = True

		Try
			success = success AndAlso LoadLohnartList()


		Catch ex As Exception
			success = False

		End Try

		Return success

	End Function


	Private Function LoadLohnartList() As Boolean

		Dim listOfData = m_TablesettingDatabaseAccess.LoadLATranslationData()

		Dim gridData = (From person In listOfData
										Select New LATranslationData With
											{.recid = person.recid,
											.LANr = person.LANr,
											.LAText = person.LAText,
											.Name_I = person.Name_I,
											.Name_F = person.Name_F,
											.Name_E = person.Name_E,
											.Name_OP_I = person.Name_OP_I,
											.Name_OP_F = person.Name_OP_F,
											.Name_OP_E = person.Name_OP_E,
											.Name_LO_I = person.Name_LO_I,
											.Name_LO_F = person.Name_LO_F,
											.Name_LO_E = person.Name_LO_E
											}).ToList()

		Dim listDataSource As BindingList(Of LATranslationData) = New BindingList(Of LATranslationData)

		For Each p In gridData
			listDataSource.Add(p)
		Next

		grdLATranslated.DataSource = listDataSource
		Me.bsiRecCount.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvLATranslated.RowCount)


		Return Not listDataSource Is Nothing

	End Function

	Private Sub OngvTableContent_RowUpdated(sender As Object, e As RowObjectEventArgs) Handles gvLATranslated.RowUpdated

		grdLATranslated.FocusedView.CloseEditor()
		Dim success = UpdateRecord(e.Row)

		If success Then
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))
		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
		End If

	End Sub

	Function UpdateRecord(ByVal rowobject As Object) As Boolean
		Dim success As Boolean = True

		Dim SelectedData = CType(rowobject, LATranslationData)
		If SelectedData.recid = 0 Then
			success = m_TablesettingDatabaseAccess.AddLATranslationData(SelectedData)
		Else
			success = m_TablesettingDatabaseAccess.UpdateAssignedLATranslationData(SelectedData)
		End If
		success = success AndAlso LoadLohnartList()

		Return success

	End Function



	Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		Me.Close()
	End Sub

End Class