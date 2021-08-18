
Imports SPProgUtility.Mandanten

Imports System.IO.File

Public Class ClsMain_Net

  Private m_md As Mandant


#Region "Startfunktionen..."

	Sub ShowfrmOPSearch()
		Dim frmTest As frmOPSearch

		frmTest = New frmOPSearch
		frmTest.Show()

	End Sub


	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub

	Public Sub New(ByVal _setting As ClsSetting)
		Dim init = CreateInitialData(_setting.SelectedMDNr, _setting.LogedUSNr)


		'ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
		'ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, 0)
		'If _setting.SelectedMDNr = 0 Then m_InitialData.MDData.MDNr = m_InitialData.MDData.MDNr
		''m_InitialData.MDData.MDDbConn = ClsDataDetail.MDData.MDDbConn

		'If _setting.LogedUSNr = 0 Then m_InitialData.UserData.UserNr = m_InitialData.UserData.UserNr

		'If ClsDataDetail.ProgSettingData.TranslationItems Is Nothing Then
		'	ClsDataDetail.ProsonalizedData = ClsDataDetail.ProsonalizedName
		'	ClsDataDetail.TranslationData = ClsDataDetail.Translation
		'Else
		'	ClsDataDetail.TranslationData = ClsDataDetail.ProgSettingData.TranslationItems
		'End If

		ClsDataDetail.m_InitialData = init ' New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(init.TranslationData, init.ProsonalizedData)


		Application.EnableVisualStyles()

	End Sub


	Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

#End Region

End Class
