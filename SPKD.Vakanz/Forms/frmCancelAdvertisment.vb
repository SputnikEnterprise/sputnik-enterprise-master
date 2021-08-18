
Imports System.Text.RegularExpressions
Imports System.IO

Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Vacancy
Imports SP.DatabaseAccess.Vacancy.DataObjects
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPKD.Vakanz.ClsDataDetail
Imports SP.Vacancies.Intern

Public Class frmCancelAdvertisment


#Region "private fields"

	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_InitializationChangedData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess
	Protected m_VacancyDatabaseAccess As IVacancyDatabaseAccess

	Protected m_UtilityUI As UtilityUI
	Private m_mandant As Mandant
	Private m_path As ClsProgPath
	Private m_common As CommonSetting

	Private m_connectionString As String
	Private m_currentVacancyNumber As Integer
	Private m_StmpData As VacancyStmpSettingData

#End Region


#Region "public properties"

	Public Property VacancySettingData As ClsVakSetting
	Public Property CurrentVacancyNumber As Integer

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal _changedSetting As SP.Infrastructure.Initialization.InitializeClass)

		m_InitializationData = _setting
		m_InitializationChangedData = _changedSetting

		m_mandant = New Mandant
		m_common = New CommonSetting
		m_path = New ClsProgPath
		m_UtilityUI = New UtilityUI

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_VacancyDatabaseAccess = New VacancyDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()

		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If


		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		TranslateControls()
		Reset()

	End Sub

#End Region


#Region "public methodes"

	Public Function LoadData() As Boolean
		Dim success As Boolean = True

		m_currentVacancyNumber = CurrentVacancyNumber
		m_StmpData = m_VacancyDatabaseAccess.LoadStmpSettingData(m_InitializationData.MDData.MDGuid, m_InitializationData.UserData.UserFullName, m_InitializationChangedData.UserData.UserNr, m_currentVacancyNumber)
		If Not (m_StmpData.AVAMStateEnum = SP.DatabaseAccess.Vacancy.DataObjects.AVAMState.PUBLISHED_PUBLIC OrElse m_StmpData.AVAMStateEnum = SP.DatabaseAccess.Vacancy.DataObjects.AVAMState.PUBLISHED_RESTRICTED) Then
			grpCancellationReason.Text = String.Format("{0}: <color=red>[{1}]</color=red> is nicht erlaubt!", m_Translate.GetSafeTranslationValue("Möglichen Gründe für Absage der übermittlten Daten"), m_StmpData.AVAMStateEnum)
			success = False
		End If
		If success AndAlso (m_StmpData Is Nothing AndAlso String.IsNullOrWhiteSpace(m_StmpData.JobroomID)) Then
			success = False
		End If
		cmdTransfer.Enabled = success
		opCancellationReason.Enabled = success

		Return success

	End Function

#End Region

	Private Sub Reset()


	End Sub

	Sub TranslateControls()

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			lblHeader1.Text = m_Translate.GetSafeTranslationValue(lblHeader1.Text)
			lblHeader2.Text = m_Translate.GetSafeTranslationValue(lblHeader2.Text)

			grpCancellationReason.Text = m_Translate.GetSafeTranslationValue(grpCancellationReason.Text)
			For Each itm In opCancellationReason.Properties.Items
				itm.description = m_Translate.GetSafeTranslationValue(itm.description)
			Next

			CmdClose.Text = m_Translate.GetSafeTranslationValue(CmdClose.Text)
			cmdTransfer.Text = m_Translate.GetSafeTranslationValue(cmdTransfer.Text)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Private Sub CmdClose_Click(sender As Object, e As EventArgs) Handles CmdClose.Click
		Me.Close()
	End Sub

	Private Sub OnCmdTransfer_Click(sender As Object, e As EventArgs) Handles cmdTransfer.Click

		Dim orgUserdata = m_mandant.GetSelectedUserData(m_InitializationData.MDData.MDNr, m_InitializationChangedData.UserData.UserNr)
		Dim InernUploader As New SP.Vacancies.Intern.InternVacancyUploader(m_InitializationChangedData)
		If m_StmpData Is Nothing Then Return

		InernUploader.CurrentVacancyNumber = m_currentVacancyNumber
		Dim jobRoomID As String = m_StmpData.JobroomID


		Dim cancelReason = New AVAMAdvertismentCancelReason
		Select Case opCancellationReason.SelectedIndex
			Case 0
				cancelReason = AVAMAdvertismentCancelReason.OCCUPIED_JOBCENTER
			Case 1
				cancelReason = AVAMAdvertismentCancelReason.OCCUPIED_AGENCY
			Case 2
				cancelReason = AVAMAdvertismentCancelReason.OCCUPIED_JOBROOM
			Case 3
				cancelReason = AVAMAdvertismentCancelReason.OCCUPIED_OTHER
			Case 4
				cancelReason = AVAMAdvertismentCancelReason.NOT_OCCUPIED
			Case 5
				cancelReason = AVAMAdvertismentCancelReason.CHANGE_OR_REPOSE

			Case Else
				Return

		End Select
		Dim cancelResult = InernUploader.CancelAssignedJobAdvertisementData(m_InitializationChangedData.MDData.MDGuid, m_InitializationChangedData.UserData.UserGuid, m_InitializationData.UserData.UserNr = 1, m_currentVacancyNumber, cancelReason, jobRoomID)

		If cancelResult Then
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Ihre Stelle wurde erfolgreich abgemeldet."))

			Me.Close()

		End If

	End Sub

End Class