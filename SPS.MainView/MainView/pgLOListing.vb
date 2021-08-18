
Imports SPS.MainView.DataBaseAccess
Imports SPS.MainView.ModulConstants

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.DXperience.Demos.TutorialControlBase
Imports System.Data.SqlClient
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Xml
Imports DevExpress.XtraEditors.Repository
Imports System.IO

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.LookAndFeel
Imports System.ComponentModel

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports System.Runtime.Serialization
Imports SPProgUtility.CommonXmlUtility

Public Class pgLOListing


#Region "private consts"

	Private Const MODUL_NAME = "Lohnlisten"
	Private Const ProgHeaderName As String = "Sputnik Enterprise Suite - [{0}] - [{1}]"
	Private Const UsercontrolCaption As String = "Lohn-Listen"
	Private Const TileLayoutFile As String = "{0}{1}_{2}.xml"
	Private Const MANDANT_XML_SETTING_SPUTNIK_SHOW_GEFAK_SCHREINER As String = "MD_{0}/Sonstiges/showgefakschreiner"

#End Region


#Region "private fields"

	Private Property LoadedMDNr As Integer

	Private LiGridProperty As New Dictionary(Of String, String)
	Private aColFieldName As String()
	Private aColCaption As String()
	Private aColWidth As String()

	Private aPropertyColFieldName As String()
	Private aPropertyColCaption As String()

	Private m_MandantData As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues
	Private Shared m_Logger As ILogger = New Logger()
	Private m_MandantSettingsXml As SettingsXml

	Private m_UtilityUI As UtilityUI
	Private m_MainUtility As SP.Infrastructure.Utility

	Private m_OpenModul As ClsOpenModul

	Private m_PayrollTileLayoutFile As String

#End Region


#Region "Contructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try


			m_MandantData = New Mandant
			m_utility = New Utilities
			m_common = New CommonSetting
			m_path = New ClsProgPath

			m_MainUtility = New SP.Infrastructure.Utility
			m_UtilityUI = New UtilityUI
			m_translate = New TranslateValues
			m_MandantSettingsXml = New SettingsXml(m_MandantData.GetSelectedMDDataXMLFilename(ModulConstants.MDData.MDNr, Now.Year))

			LoadedMDNr = ModulConstants.MDData.MDNr

			'tiGAVSonstige.Image = Nothing
			tiGAVInkasso.Image = Nothing
			tiGAVGEFAK.Image = My.Resources.Gefak_BL
			tiGAVSchreiner.Image = Nothing
			tiGAVPVL.Image = My.Resources.tempdata
			tiGAVFAR.Image = My.Resources.Far_Stiftung

			tiMFak.Image = My.Resources.Sputnik_Icon_FAK_Abrechnung_GzD
			tiMLohnbelege.Image = My.Resources.Sputnik_Icon_Lohnbelege_GzD
			tiMBuchungsbelege.Image = My.Resources.Sputnik_Icon_Buchungsbelege_GzD
			tiMArbeitsstunden.Image = My.Resources.Sputnik_Icon_Arbeitsstunden_GzD
			tiMFremdleistung.Image = My.Resources.Sputnik_Icon_Fremdleistungen_GzD
			tiMBruttolohnjournal.Image = My.Resources.Sputnik_Icon_Bruttolohnjournal_GzD
			tiMANRekap.Image = My.Resources.Sputnik_Icon_AN_Lohnrekap_GzD
			tiMAGRekap.Image = My.Resources.Sputnik_Icon_AG_Lohnrekap_GzD
			tiMQST.Image = My.Resources.Sputnik_Icon_Quellensteuer_GzD
			tiMBVG.Image = My.Resources.Sputnik_Icon_BVG_Liste_GzD

			tiYLohnkonti.Image = My.Resources.Sputnik_Icon_Lohnkonti_GzD
			tiYFranz.Image = My.Resources.Sputnik_Icon_Franz__Grenzg_nger_GzD
			tiYAHV.Image = My.Resources.Sputnik_Icon_AHV_Lohnbescheinigung_GzD
			tiYUVG.Image = My.Resources.Sputnik_Icon_UVG_Jahresliste_GzD
			tiYLohnrekap.Image = My.Resources.Sputnik_Icon_Lohnarten_Rekap_GzD
			tiYGuthaben.Image = My.Resources.Sputnik_Icon_Guthaben_GzD
			tiYLohnausweis.Image = My.Resources.Sputnik_Icon_Lohnausweis_GzD
			tiYKiga.Image = My.Resources.Sputnik_Icon_KIGA_Statistik_GzD
			tiYKinder.Image = My.Resources.Sputnik_Icon_Kinder_Ausbildungszulage_GzD

			Dim showtiGEFAK_Schreiner = m_utility.ParseToBoolean(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_SHOW_GEFAK_SCHREINER, ModulConstants.MDData.MDNr)), False)
			tiGAVGEFAK.Visible = showtiGEFAK_Schreiner
			tiGAVSchreiner.Visible = showtiGEFAK_Schreiner

			m_OpenModul = New ClsOpenModul(New ClsSetting With {.SelectedMANr = Nothing})

			m_PayrollTileLayoutFile = String.Format(TileLayoutFile, ModulConstants.GridSettingPath, ticPayrollModules.Name, ModulConstants.UserData.UserNr)

			SaveLayoutofTiles()

			TranslateControls()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

#End Region


	Private Sub TranslateControls()

		Text = String.Format("{0}", m_translate.GetSafeTranslationValue(UsercontrolCaption))

		'Me.tiGAVSonstige.Text = m_translate.GetSafeTranslationValue(Me.tiGAVSonstige.Text)
		Me.tiGAVInkasso.Text = m_translate.GetSafeTranslationValue(Me.tiGAVInkasso.Text)
		Me.tiGAVGEFAK.Text = m_translate.GetSafeTranslationValue(Me.tiGAVGEFAK.Text)
		Me.tiGAVSchreiner.Text = m_translate.GetSafeTranslationValue(Me.tiGAVSchreiner.Text)
		Me.tiGAVPVL.Text = m_translate.GetSafeTranslationValue(Me.tiGAVPVL.Text)
		Me.tiGAVFAR.Text = m_translate.GetSafeTranslationValue(Me.tiGAVFAR.Text)

		Me.tiMFak.Text = m_translate.GetSafeTranslationValue(Me.tiMFak.Text)
		Me.tiMFak.Text = m_translate.GetSafeTranslationValue(Me.tiMFak.Text)
		Me.tiMLohnbelege.Text = m_translate.GetSafeTranslationValue(Me.tiMLohnbelege.Text)
		Me.tiMBuchungsbelege.Text = m_translate.GetSafeTranslationValue(Me.tiMBuchungsbelege.Text)
		Me.tiMArbeitsstunden.Text = m_translate.GetSafeTranslationValue(Me.tiMArbeitsstunden.Text)
		Me.tiMFremdleistung.Text = m_translate.GetSafeTranslationValue(Me.tiMFremdleistung.Text)
		Me.tiMBruttolohnjournal.Text = m_translate.GetSafeTranslationValue(Me.tiMBruttolohnjournal.Text)
		Me.tiMANRekap.Text = m_translate.GetSafeTranslationValue(Me.tiMANRekap.Text)
		Me.tiMAGRekap.Text = m_translate.GetSafeTranslationValue(Me.tiMAGRekap.Text)
		Me.tiMQST.Text = m_translate.GetSafeTranslationValue(Me.tiMQST.Text)
		Me.tiMBVG.Text = m_translate.GetSafeTranslationValue(Me.tiMBVG.Text)

		Me.tiYLohnkonti.Text = m_translate.GetSafeTranslationValue(Me.tiYLohnkonti.Text)
		Me.tiYFranz.Text = m_translate.GetSafeTranslationValue(Me.tiYFranz.Text)
		Me.tiYAHV.Text = m_translate.GetSafeTranslationValue(Me.tiYAHV.Text)
		Me.tiYUVG.Text = m_translate.GetSafeTranslationValue(Me.tiYUVG.Text)
		Me.tiYLohnrekap.Text = m_translate.GetSafeTranslationValue(Me.tiYLohnrekap.Text)
		Me.tiYGuthaben.Text = m_translate.GetSafeTranslationValue(Me.tiYGuthaben.Text)
		Me.tiYLohnausweis.Text = m_translate.GetSafeTranslationValue(Me.tiYLohnausweis.Text)
		Me.tiYKiga.Text = m_translate.GetSafeTranslationValue(Me.tiYKiga.Text)
		Me.tiYKinder.Text = m_translate.GetSafeTranslationValue(Me.tiYKinder.Text)

		If File.Exists(m_PayrollTileLayoutFile) Then ticPayrollModules.RestoreLayoutFromXml(m_PayrollTileLayoutFile)


	End Sub


#Region "GAV-Lohnlisten Listen..."

	'Private Sub tiGAVSonstige_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiGAVSonstige.ItemClick

	'  Try
	'    m_OpenModul.OpenGAVDivSearchForm()

	'  Catch ex As Exception

	'  End Try

	'End Sub

	Private Sub tiGAVInkasso_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiGAVInkasso.ItemClick

		Try
			m_OpenModul.OpenGAVIPoolSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiGAVGefaK_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiGAVGEFAK.ItemClick

		Try
			'm_OpenModul.OpenGAVGEFakSearchForm()

		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiGAVSchreiner_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiGAVSchreiner.ItemClick

		Try
			m_OpenModul.OpenGAVSchSearchForm()

		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiGAVPVL_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiGAVPVL.ItemClick

		Try
			m_OpenModul.OpenPVLLohnSearchForm()

		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiGAVFAR_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiGAVFAR.ItemClick

		Try
			m_OpenModul.OpenFARLohnSearchForm()

		Catch ex As Exception

		End Try

	End Sub

#End Region




#Region "Monatliche Lohnlisten..."


	Private Sub tiMFAK_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMFak.ItemClick

		Try
			m_OpenModul.OpenMFakSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiMLOBegleg_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMLohnbelege.ItemClick

		Try
			m_OpenModul.OpenMLohnbelegFibuSearchForm()

		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiMBuchungsbeleg_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMBuchungsbelege.ItemClick

		Try
			m_OpenModul.OpenMFibuSearchForm()
		Catch ex As Exception

		End Try

	End Sub


	Private Sub tiMBVG_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMBVG.ItemClick

		Try
			m_OpenModul.OpenMBVGSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiMQST_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMQST.ItemClick

		Try
			m_OpenModul.OpenMQSTSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiMBruttolohn_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMBruttolohnjournal.ItemClick

		Try
			m_OpenModul.OpenMBJournalSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiMKDRekap_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMAGRekap.ItemClick

		Try
			m_OpenModul.OpenMFRekapLOAGSearchForm(1)
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiMArbeitsstunden_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMArbeitsstunden.ItemClick

		Try
			m_OpenModul.OpenMLOStdSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiMFremdleistung_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMFremdleistung.ItemClick

		Try
			m_OpenModul.OpenMFremdleistungSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiMMARepak_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiMANRekap.ItemClick

		Try
			m_OpenModul.OpenMMARekapLOANSearchForm(1)
		Catch ex As Exception

		End Try

	End Sub


#End Region


#Region "Jährliche Lohnlisten..."

	Private Sub tiYLKonti_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiYLohnkonti.ItemClick

		Try
			m_OpenModul.OpenYLohnkontiSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiYRekap_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiYLohnrekap.ItemClick

		Try
			m_OpenModul.OpenYLohnRekapSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiYKiga_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiYKiga.ItemClick

		Try
			m_OpenModul.OpenYKigaSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiYFranc_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiYFranz.ItemClick

		Try
			m_OpenModul.OpenYFGrenzGSearchForm()
		Catch ex As Exception

		End Try
		DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm(False)

	End Sub

	Private Sub tiYGuthaben_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiYGuthaben.ItemClick

		Try
			m_OpenModul.OpenYGuthabenSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiYKinder_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiYKinder.ItemClick

		Try
			m_OpenModul.OpenYFAKSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiYNLA_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiYLohnausweis.ItemClick

		Try
			m_OpenModul.OpenYNLASearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiYUVG_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiYUVG.ItemClick

		Try
			m_OpenModul.OpenUVGSearchForm()
		Catch ex As Exception

		End Try

	End Sub

	Private Sub tiYAHV_ItemClick(sender As Object, e As DevExpress.XtraEditors.TileItemEventArgs) Handles tiYAHV.ItemClick

		Try
			m_OpenModul.OpenYAHVSearchForm()
		Catch ex As Exception

		End Try

	End Sub


#End Region


#Region "saving and restoring layout of tilecontrol"

	Sub SaveLayoutofTiles()

		Try
			If File.Exists(m_PayrollTileLayoutFile) Then ticPayrollModules.RestoreLayoutFromXml(m_PayrollTileLayoutFile)

		Catch ex As Exception
			m_Logger.LogError(String.Format("SaveLayoutofTiles: {0}", ex.Message))
		End Try

	End Sub

	Private Sub ticPayrollModules_EndItemDragging(sender As Object, e As TileItemDragEventArgs) Handles ticPayrollModules.EndItemDragging

		ticPayrollModules.SaveLayoutToXml(m_PayrollTileLayoutFile)

	End Sub

#End Region


End Class

