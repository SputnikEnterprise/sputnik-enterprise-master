
Imports SP.DatabaseAccess.Employee
Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports DevExpress.XtraEditors.Controls



Public Class frmNLAData

#Region "private fields"


	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The employee data access object.
	''' </summary>
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

	Private m_employeeNumber As Integer


#End Region



#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_InitializationData = _setting

		m_MandantData = New Mandant
		m_UtilityUI = New UtilityUI
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(_setting.MDData.MDDbConn, _setting.UserData.UserLanguage)

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

		m_MandantData = New Mandant

		m_UtilityUI = New UtilityUI

		m_InitializationData = _setting

		Reset()



	End Sub


	Private Sub Reset()

		PrepareForNew()

		bsiCreated.Caption = String.Empty
		bsiChanged.Caption = String.Empty

		TranslateControls()

	End Sub

	Private Sub PrepareForNew()

		rgNLA.SelectedIndex = 0
		chk_UB.Checked = False
		chk_Kantine.Checked = False

		txt_2_3.Text = String.Empty
		txt_3.Text = String.Empty
		txt_4.Text = String.Empty
		txt_7.Text = String.Empty
		txt_13_1_2.Text = String.Empty
		txt_13_2_3.Text = String.Empty
		txt_14_1.Text = String.Empty
		txt_14_2.Text = String.Empty
		txt_15_1.Text = String.Empty
		txt_15_2.Text = String.Empty

	End Sub

	Private Sub PresentDetailData(ByVal data As EmployeeNLAData)

		If data Is Nothing Then Return

		If data.recID.GetValueOrDefault(0) = 0 Then Return

		rgNLA.Properties.AllowFocused = False
		rgNLA.SelectedIndex = If(data.NLA_LoAusweis, 0, 1)

		chk_UB.Checked = CBool(data.NLA_Befoerderung)
		chk_Kantine.Checked = CBool(data.NLA_Kantine)

		txt_2_3.Text = data.NLA_2_3
		txt_3.Text = data.NLA_3_0
		txt_4.Text = data.NLA_4_0
		txt_7.Text = data.NLA_7_0
		txt_13_1_2.Text = data.NLA_13_1_2
		txt_13_2_3.Text = data.NLA_13_2_3

		txt_14_1.Text = data.NLA_Nebenleistung_1
		txt_14_2.Text = data.NLA_Nebenleistung_2

		txt_15_1.Text = data.NLA_Bemerkung_1
		txt_15_2.Text = data.NLA_Bemerkung_2

		bsiCreated.Caption = String.Format("{0:f}, {1}", data.createdon, data.createdfrom)
		bsiChanged.Caption = String.Format("{0:f}, {1}", data.ChangedOn, data.ChangedFrom)

		bsiCreated.Caption = String.Format("{0:f}, {1}", data.createdon, data.createdfrom)
		bsiChanged.Caption = String.Format("{0:f}{1} {2}", data.ChangedOn, If(data.ChangedOn Is Nothing, "", ","), data.ChangedFrom)

	End Sub

	Sub TranslateControls()

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
			Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

			rgNLA.Properties.Items(0).Description = m_Translate.GetSafeTranslationValue(rgNLA.Properties.Items(0).Description)
			rgNLA.Properties.Items(1).Description = m_Translate.GetSafeTranslationValue(rgNLA.Properties.Items(1).Description)

			grpDiverseLeistungen.Text = m_Translate.GetSafeTranslationValue(grpDiverseLeistungen.Text)
			grpSpesen.Text = m_Translate.GetSafeTranslationValue(grpSpesen.Text)
			grpWeitereLeistungen.Text = m_Translate.GetSafeTranslationValue(grpWeitereLeistungen.Text)
			grpBemerkungen.Text = m_Translate.GetSafeTranslationValue(grpBemerkungen.Text)

			Me.lbl_2_3.Text = m_Translate.GetSafeTranslationValue(Me.lbl_2_3.Text)
			Me.lbl_3.Text = m_Translate.GetSafeTranslationValue(Me.lbl_3.Text)
			Me.lbl_4.Text = m_Translate.GetSafeTranslationValue(Me.lbl_4.Text)
			Me.lbl_7.Text = m_Translate.GetSafeTranslationValue(Me.lbl_7.Text)

			Me.lbl_13_1_2.Text = m_Translate.GetSafeTranslationValue(Me.lbl_13_1_2.Text)
			Me.lbl_13_2_3.Text = m_Translate.GetSafeTranslationValue(Me.lbl_13_2_3.Text)
			Me.lbl_14.Text = m_Translate.GetSafeTranslationValue(Me.lbl_14.Text)
			Me.lbl_15.Text = m_Translate.GetSafeTranslationValue(Me.lbl_15.Text)

			Me.chk_UB.Text = m_Translate.GetSafeTranslationValue(Me.chk_UB.Text)
			Me.chk_Kantine.Text = m_Translate.GetSafeTranslationValue(Me.chk_Kantine.Text)

			bsiLblErstellt.Caption = m_Translate.GetSafeTranslationValue(bsiLblErstellt.Caption)
			bsilblGeaendert.Caption = m_Translate.GetSafeTranslationValue(bsilblGeaendert.Caption)
			bbiSave.Caption = m_Translate.GetSafeTranslationValue(bbiSave.Caption)
			bbiDelete.Caption = m_Translate.GetSafeTranslationValue(bbiDelete.Caption)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub


	Private Sub CmdClose_Click(sender As Object, e As EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub


#End Region


	Public Sub LoadData(ByVal employeeNumber As Integer)

		m_employeeNumber = employeeNumber

		Dim data = m_EmployeeDatabaseAccess.LoadEmployeeNLAData(employeeNumber)
		If data Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Ihre Daten konnten nicht geladen werden. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return
		End If
		If data.recID.GetValueOrDefault(0) = 0 Then
			PrepareForNew()
		Else
			PresentDetailData(data)
		End If

	End Sub

	Private Sub OnbbiSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick

		Dim data = m_EmployeeDatabaseAccess.LoadEmployeeNLAData(m_employeeNumber)
		If data Is Nothing Then
			data = New EmployeeNLAData
		End If

		data.NLA_LoAusweis = (rgNLA.SelectedIndex = 0)

		data.NLA_Befoerderung = chk_UB.Checked
		data.NLA_Kantine = chk_Kantine.Checked

		data.NLA_2_3 = txt_2_3.Text
		data.NLA_3_0 = txt_3.Text
		data.NLA_4_0 = txt_4.Text
		data.NLA_7_0 = txt_7.Text
		data.NLA_13_1_2 = txt_13_1_2.Text
		data.NLA_13_2_3 = txt_13_2_3.Text

		data.NLA_Nebenleistung_1 = txt_14_1.Text
		data.NLA_Nebenleistung_2 = txt_14_2.Text

		data.NLA_Bemerkung_1 = txt_15_1.Text
		data.NLA_Bemerkung_2 = txt_15_2.Text

		data.ChangedFrom = m_InitializationData.UserData.UserFullName


		Dim success = m_EmployeeDatabaseAccess.SaveEmployeeNLAData(data, m_employeeNumber)
		Dim msg As String = String.Empty
		If Not success Then
			msg = "Daten konnten nicht gespeichert werden. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt."
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

		Else
			msg = "Die Daten wurden gespeichert."
			m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Daten speichern"), MessageBoxIcon.Exclamation)

		End If

	End Sub

	Private Sub OnbbiDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick

		If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																							m_Translate.GetSafeTranslationValue("Datensatz löschen")) = True) Then

			Dim success = m_EmployeeDatabaseAccess.DeleteEmployeeNLAData(m_employeeNumber)
			If Not success Then
				m_UtilityUI.ShowErrorDialog("Die Daten konnten nicht gelöscht werden.")
			Else
				m_UtilityUI.ShowInfoDialog("Die Daten wurden erfolgreich gelöscht.")

				Me.Close()
				Me.Dispose()

			End If

		End If


	End Sub




End Class