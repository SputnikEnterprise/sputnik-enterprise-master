Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI
Imports SPProgUtility
Imports SP.Infrastructure

Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Applicant.DataObjects


Public Class frmApplicationDetail

#Region "private fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility

	Private m_mandant As Mandant
	Private m_path As SPProgUtility.ProgPath.ClsProgPath

	Private ucDetail As ucApplicationDetail
	Private m_CurrentApplicationData As MainViewApplicationData


#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal data As MainViewApplicationData)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()
		m_InitializationData = _setting

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		TranslateControls()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_CurrentApplicationData = data
		ucDetail = New ucApplicationDetail(m_InitializationData)
		ucDetail.CurrentApplicationID = m_CurrentApplicationData.ID
		ucDetail.LoadData()

		pnlDetail.Controls.Add(ucDetail)
		ucDetail.Dock = DockStyle.Fill

	End Sub

#End Region

	Private Sub TranslateControls()

		btnClose.Text = m_Translate.GetSafeTranslationValue(btnClose.Text)

	End Sub

	Private Sub OnbtnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		ucDetail.CleanUp()

		Me.Close()

	End Sub




#Region "Helpers"



#End Region


End Class
