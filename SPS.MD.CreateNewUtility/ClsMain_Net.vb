
Public Class ClsMain_Net

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

  Public Sub New(ByVal _setting As ClsSetting)

		'ClsDataDetail.ProgSettingData = _setting

		m_InitializationData = CreateInitialData(_setting.SelectedMDNr, _setting.LogedUSNr)

		'If ClsDataDetail.ProgSettingData.TranslationItems Is Nothing Then
		'    ClsDataDetail.ProsonalizedData = ClsDataDetail.ProsonalizedName
		'    ClsDataDetail.TranslationData = ClsDataDetail.Translation
		'  Else
		'    ClsDataDetail.TranslationData = ClsDataDetail.ProgSettingData.TranslationItems
		'  End If

		Application.EnableVisualStyles()

  End Sub

  Sub ShowfrmCreateNewYear()

		Try

			Dim frmTest = New frmNewMDYear(m_InitializationData)
			frmTest.Show()

			frmTest.BringToFront()


		Catch ex As Exception
      MsgBox(ex.Message, MsgBoxStyle.Critical, "ShowfrmCreateNewYear")

    End Try

  End Sub


#Region "Helpers"

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

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
