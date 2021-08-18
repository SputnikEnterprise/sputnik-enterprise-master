
Imports System.IO.File

Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions

Public Class ClsMain_Net
  Public Shared frmTest As frmLoNLASearch

  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Application.EnableVisualStyles()

	End Sub

	Public Sub New()
		Dim m_md As New SPProgUtility.Mandanten.Mandant

		Dim _setting = CreateInitialData(m_md.GetDefaultMDNr, m_md.GetDefaultUSNr)
		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

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

  Sub ShowfrmLoNLA()

    Try
			frmTest = New frmLoNLASearch(ClsDataDetail.m_InitialData)
			frmTest.Show()

		Catch ex As Exception
			MessageBoxShowError("ShowfrmLoNLA", ex)
    End Try


  End Sub

End Class
