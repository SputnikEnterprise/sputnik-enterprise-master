
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
Imports TrxmlUtility
Imports SP.Infrastructure.Logging


Public Class frmSourceBox


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

	Private ucSourceBox As ucSourceBox


#End Region


#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		ucSourceBox = New ucSourceBox

		Me.Controls.Add(ucSourceBox)
		ucSourceBox.Dock = DockStyle.Fill
		Dim success As Boolean = ucSourceBox.Login("username", "username", "vcge6123")

	End Sub

#End Region


#Region "public properties"

	Public Property TrXMLID As Integer

#End Region


#Region "public methods"

	Public Sub LoadCV(ByVal trxmlID As Integer)
		Dim success As Boolean = True

		success = success AndAlso ucSourceBox.LoadCv(trxmlID)	 ' 53854028
		success = success AndAlso ucSourceBox.LoadCvOneFrame

	End Sub

#End Region


	Private Sub frmSourceBox_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
		ucSourceBox.CleanUp()

	End Sub

	Private Sub frmSourceBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		ucSourceBox.Visible = True
	End Sub


End Class