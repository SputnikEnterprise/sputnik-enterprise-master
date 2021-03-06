
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure


Public Class frmViewPDF


#Region "Private fields"

	'Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Private Shared m_Logger As ILogger
	Private m_UtilityUI As UtilityUI

#End Region

#Region "Private Properties"

	Private Property m_Filename As String

#End Region


#Region "Public Properties"

	Public Property UpdateProtokollID As Integer?

#End Region


#Region "Constructor"

	Public Sub New(ByVal _filename As String)

		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		m_Logger = New Logger

		m_UtilityUI = New UtilityUI
		'm_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_Filename = _filename

	End Sub


#End Region


#Region "Private methodes"

	'Private Sub Onfrm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

	'	Try
	'		Dim UpdateData = m_DatabaseAccess.UpdateViewedUpdate(UpdateProtokollID)

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)
	'	End Try


	'End Sub

	Private Sub TranslateControls()

		'Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)


	End Sub

#End Region



#Region "Public methodes"

	Public Function OpenPDFDocument() As Boolean

		Me.PdfViewer.LoadDocument(m_Filename)

		'PdfViewer.ShowPrintStatusDialog = false

		Return True

	End Function


#End Region



End Class