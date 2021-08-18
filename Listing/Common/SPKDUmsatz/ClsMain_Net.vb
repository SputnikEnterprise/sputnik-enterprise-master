

Imports SPProgUtility.Mandanten
Imports SPProgUtility

Public Class ClsMain_Net



	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		ClsDataDetail.MDData = _setting.MDData
		ClsDataDetail.UserData = _setting.UserData

		ClsDataDetail.GetSelectedMDConnstring = _setting.MDData.MDDbConn

		'ClsDataDetail.ProgSettingData.LogedUSNr = _setting.UserData.UserNr

		'ClsDataDetail.ProsonalizedData = _setting.ProsonalizedData
		'ClsDataDetail.TranslationData = _setting.TranslationData



		Application.EnableVisualStyles()

	End Sub

	Public Sub New()
		Dim m_md As New SPProgUtility.Mandanten.Mandant

		Dim _setting = CreateInitialData(m_md.GetDefaultMDNr, m_md.GetDefaultUSNr)
		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		ClsDataDetail.MDData = _setting.MDData
		ClsDataDetail.UserData = _setting.UserData

		ClsDataDetail.GetSelectedMDConnstring = _setting.MDData.MDDbConn

		ClsDataDetail.ProgSettingData.LogedUSNr = _setting.UserData.UserNr

		ClsDataDetail.ProsonalizedData = _setting.ProsonalizedData
		ClsDataDetail.TranslationData = _setting.TranslationData


		Application.EnableVisualStyles()

	End Sub

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


  Sub ShowListForm()
		Dim frmTest As frmKDUmsatz

		frmTest = New frmKDUmsatz(ClsDataDetail.m_InitialData)
    frmTest.Show()

  End Sub


  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

	'Public Sub New(ByVal _setting As ClsSetting)

	'  ClsDataDetail.ProgSettingData = _setting

	'  ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
	'  ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, 0)
	'  If _setting.SelectedMDNr = 0 Then ClsDataDetail.ProgSettingData.SelectedMDNr = ClsDataDetail.MDData.MDNr
	'  ClsDataDetail.GetSelectedMDConnstring = ClsDataDetail.MDData.MDDbConn

	'  If _setting.LogedUSNr = 0 Then ClsDataDetail.ProgSettingData.LogedUSNr = ClsDataDetail.UserData.UserNr

	'  If ClsDataDetail.ProgSettingData.TranslationItems Is Nothing Then
	'    ClsDataDetail.ProsonalizedData = ClsDataDetail.ProsonalizedName
	'    ClsDataDetail.TranslationData = ClsDataDetail.Translation
	'  Else
	'    ClsDataDetail.TranslationData = ClsDataDetail.ProgSettingData.TranslationItems
	'  End If


	'  Application.EnableVisualStyles()

	'End Sub

End Class
