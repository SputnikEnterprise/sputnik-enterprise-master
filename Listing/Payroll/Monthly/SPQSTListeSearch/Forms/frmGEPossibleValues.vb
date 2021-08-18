
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI
Imports SPQSTListeSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class frmGEPossibleValues

	Inherits DevExpress.XtraEditors.XtraForm

#Region "Private Fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

#End Region


#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ClsDataDetail.m_InitialData.TranslationData, ClsDataDetail.m_InitialData.ProsonalizedData)


		Reset()


	End Sub

#End Region


	Private Sub Reset()

		ResetCommunityData()

	End Sub


	Function GetDbSalaryData4Show() As IEnumerable(Of XMLGECommunityData)
		Dim result As List(Of XMLGECommunityData) = Nothing


		Try
			result = New List(Of XMLGECommunityData)

			For i As Integer = 1 To 45

				Dim overviewData As New XMLGECommunityData

				overviewData.CommunityNumber = 6600 + i

				result.Add(overviewData)

			Next i

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		End Try

		Return result
	End Function

	Public Function LoadFoundedCommunityList() As Boolean

		Dim listOfEmployees = GetDbSalaryData4Show()
		If listOfEmployees Is Nothing Then
			m_UtilityUi.ShowErrorDialog("Keine Daten wurden gefunden.")

			Return False
		End If

		'Dim responsiblePersonsGridData = (From person In listOfEmployees
		'Select New XMLGECommunityData With
		'			 {.CommunityNumber = person.CommunityNumber
		'			 }).ToList()

		'Dim listDataSource As BindingList(Of XMLGECommunityData) = New BindingList(Of XMLGECommunityData)

		'For Each p In responsiblePersonsGridData
		'	listDataSource.Add(p)
		'Next

		grdCommunity.DataSource = listOfEmployees

		Return Not listOfEmployees Is Nothing
	End Function

	Private Sub ResetCommunityData()

		gvCommunity.OptionsView.ShowIndicator = False
		gvCommunity.OptionsView.ShowAutoFilterRow = True
		gvCommunity.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvCommunity.OptionsView.ShowFooter = False

		gvCommunity.Columns.Clear()


		Dim CommunityNr As New DevExpress.XtraGrid.Columns.GridColumn()
		CommunityNr.Caption = m_Translate.GetSafeTranslationValue("Gemeinde-Nr.")
		CommunityNr.Name = "CommunityNumber"
		CommunityNr.FieldName = "CommunityNumber"
		CommunityNr.Visible = True
		gvCommunity.Columns.Add(CommunityNr)


		grdCommunity.DataSource = Nothing

	End Sub


	Private Sub frmGEPossibleValues_Load(sender As Object, e As EventArgs) Handles MyBase.Load

		Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

	End Sub


End Class