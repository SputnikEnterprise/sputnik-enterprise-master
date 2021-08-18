
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Common.DataObjects.MandantData


Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraNavBar
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.EmployeeContactData

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages


Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen


Public Class frmMandant




#Region "Private Fields"

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' List of user controls.
	''' </summary>
	Private m_ListOfUserControls As New List(Of DevExpress.XtraEditors.XtraUserControl)
	Private m_ISAlreadySaved As Boolean
	Private connectionString As String

	Private ucFieldLabel1 As ucFieldlables
	Private ucJobPlattforms1 As ucJobPlattforms
	Private ucMDFibuKonten1 As ucMDFibuKonten
	Private UcMDMarginFees1 As ucMDMarginFees
	Private ucMDName1 As ucMDName
	Private UcMDProgramm1 As ucMDProgramm
	Private UcMDPublicAuthorities1 As ucMDPublicAuthorities
	Private ucMDStartNumbers1 As ucMDStartNumbers
	Private ucRequiredFields1 As ucRequiredFields
	Private ucStandardvalues1 As ucStandardvalues
	Private ucWebServices1 As ucWebServices


	Enum ArtOfModul

		MandantenName

		PublicAuthorities

		Firmenkonstant

		MarginAndFees

		FIBUKonten

		StartNumbers

		ProgSetting

		StandardValues

		FieldLable

		RequiredFields

		WebServices

		Jobplattforms

		UNKNOWN

	End Enum

	Private m_SelectedModul As ArtOfModul


#End Region


#Region "private consts"



#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		ClsDataDetail.m_InitialData = m_InitializationData

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		InitializeComponent()


		' Translate controls.
		TranslateControls()

		bbiSave.Enabled = False
		bbiPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
		bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		' Creates the navigation bar.
		CreateMyNavBar()
		m_SelectedModul = ArtOfModul.MandantenName

		connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)

		Reset()
		loadInitData()
		'sccMain.Dock = DockStyle.Fill


		ucFieldLabel1 = New ucFieldlables
		ucJobPlattforms1 = New ucJobPlattforms
		ucMDFibuKonten1 = New ucMDFibuKonten
		UcMDMarginFees1 = New ucMDMarginFees
		ucMDName1 = New ucMDName
		UcMDProgramm1 = New ucMDProgramm
		UcMDPublicAuthorities1 = New ucMDPublicAuthorities
		ucMDStartNumbers1 = New ucMDStartNumbers
		ucRequiredFields1 = New ucRequiredFields
		ucStandardvalues1 = New ucStandardvalues
		ucWebServices1 = New ucWebServices

		m_ListOfUserControls.Add(UcMDName1)
		m_ListOfUserControls.Add(UcMDPublicAuthorities1)
		m_ListOfUserControls.Add(UcMDProgramm1)
		m_ListOfUserControls.Add(UcMDMarginFees1)
		m_ListOfUserControls.Add(ucMDFibuKonten1)

		sccMain.Panel2.Controls.Add(UcMDName1)
		sccMain.Panel2.Controls.Add(UcMDPublicAuthorities1)
		sccMain.Panel2.Controls.Add(UcMDProgramm1)
		sccMain.Panel2.Controls.Add(UcMDMarginFees1)
		sccMain.Panel2.Controls.Add(ucMDFibuKonten1)


		sccMain.Panel2.Controls.Add(ucMDStartNumbers1)
		sccMain.Panel2.Controls.Add(ucStandardvalues1)
		sccMain.Panel2.Controls.Add(ucFieldLabel1)
		sccMain.Panel2.Controls.Add(ucRequiredFields1)
		sccMain.Panel2.Controls.Add(ucWebServices1)




		' Init sub controls with configuration information
		UcMDName1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
		UcMDName1.Dock = DockStyle.Fill
		UcMDName1.Visible = False

		UcMDPublicAuthorities1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
		UcMDPublicAuthorities1.Dock = DockStyle.Fill
		UcMDPublicAuthorities1.Visible = False

		UcMDProgramm1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
		UcMDProgramm1.Dock = DockStyle.Fill
		UcMDProgramm1.Visible = False

		UcMDMarginFees1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
		UcMDMarginFees1.Dock = DockStyle.Fill
		UcMDMarginFees1.Visible = False

		ucMDFibuKonten1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
		ucMDFibuKonten1.Dock = DockStyle.Fill
		ucMDFibuKonten1.Visible = False


		ucMDStartNumbers1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
		ucMDStartNumbers1.Dock = DockStyle.Fill
		ucMDStartNumbers1.Visible = False

		ucStandardvalues1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
		ucStandardvalues1.Dock = DockStyle.Fill
		ucStandardvalues1.Visible = False

		ucFieldLabel1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
		ucFieldLabel1.Dock = DockStyle.Fill
		ucFieldLabel1.Visible = False

		ucRequiredFields1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
		ucRequiredFields1.Dock = DockStyle.Fill
		ucRequiredFields1.Visible = False

		ucWebServices1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
		ucWebServices1.Dock = DockStyle.Fill
		ucWebServices1.Visible = False


		AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged
		AddHandler Me.lueYear.EditValueChanged, AddressOf OnlueYear_EditValueChanged

		Dim success = LoadSelectedPage()
		If success Then bbiSave.Enabled = True
		'CreateExportPopupMenu()


	End Sub


#End Region


	Private Sub Reset()

		ResetMandantenDropDown()
		ResetYearDataDropDown()

	End Sub


	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	Private Sub ResetYearDataDropDown()
		lueYear.Properties.DisplayMember = "Value"
		lueYear.Properties.ValueMember = "Value"
		lueYear.Properties.ShowHeader = False

		lueYear.Properties.Columns.Clear()
		lueYear.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "Value",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Value")})

		lueYear.Properties.ShowFooter = False
		lueYear.Properties.DropDownRows = 10
		lueYear.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueYear.Properties.SearchMode = SearchMode.AutoComplete
		lueYear.Properties.AutoSearchColumnIndex = 0

		lueYear.Properties.NullText = String.Empty
		lueYear.EditValue = Nothing
	End Sub

	''' <summary>
	''' Load Mandanten drop down
	''' </summary>
	''' <remarks></remarks>
	Private Sub LoadMandantenDropDown()
		Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()

	End Sub


	Private Function LoadYearDropDownData() As Boolean
		Dim success As Boolean = True

		Dim yearData = m_CommonDatabaseAccess.LoadMandantYears(m_InitializationData.MDData.MDNr)

		If (yearData Is Nothing) Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Jahre (Mandanten) konnten nicht geladen werden."))
		End If

		Dim yearValues As List(Of YearValueView) = Nothing

		If Not yearData Is Nothing Then
			yearValues = New List(Of YearValueView)

			For Each yearValue In yearData
				yearValues.Add(New YearValueView With {.Value = yearValue})
			Next

		End If

		lueYear.Properties.DataSource = yearValues
		If lueYear.EditValue Is Nothing OrElse Not yearValues.Any(Function(data) data.Value = lueYear.EditValue) Then
			lueYear.EditValue = Now.Year
		Else
			lueYear.EditValue = lueYear.EditValue
		End If

		lueYear.Properties.ForceInitialize()

		Return yearData IsNot Nothing

	End Function

	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim SelectedData As SP.DatabaseAccess.Common.DataObjects.MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), SP.DatabaseAccess.Common.DataObjects.MandantData)

		If Not SelectedData Is Nothing Then

			If m_InitializationData.MDData.MDNr <> SelectedData.MandantNumber Then
				Dim msg = "Achtung: Ihre Daten wurden nicht gespeichert! Möchten Sie die Daten speichern?"
				If Not m_ISAlreadySaved AndAlso (m_SelectedModul <> ArtOfModul.UNKNOWN) Then
					If m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Daten speichern")) = True Then
						SaveSelectedPage(False)
					End If
				End If

				Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation

				Dim clsMandant = m_mandant.GetSelectedMDData(SelectedData.MandantNumber)
				Dim logedUserData = m_mandant.GetSelectedUserData(clsMandant.MDNr, m_InitializationData.UserData.UserNr)
				Dim personalizedData = m_InitializationData.ProsonalizedData
				Dim translate = m_InitializationData.TranslationData

				m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

				LoadYearDropDownData()

				ReloadSelectedPage()

			End If

		End If

	End Sub

	Private Sub OnlueYear_EditValueChanged(sender As Object, e As System.EventArgs)

		If lueYear.EditValue Is Nothing Then Return
		If m_InitializationData.MDData.MDYear <> lueYear.EditValue Then

			Dim msg = "Achtung: Ihre Daten wurden nicht gespeichert! Möchten Sie die Daten speichern?"
			If Not m_ISAlreadySaved AndAlso (m_SelectedModul <> ArtOfModul.UNKNOWN) Then
				If m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Daten speichern")) = True Then
					SaveSelectedPage(False)
				End If
			End If

			m_InitializationData.MDData.MDYear = lueYear.EditValue
			ReloadSelectedPage()
		End If

	End Sub




#Region "Private Methods"


	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.btnClose.Text = m_Translate.GetSafeTranslationValue(Me.btnClose.Text)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)

		Me.bbiSave.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSave.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

	End Sub


	''' <summary>
	''' Creates Navigationbar
	''' </summary>
	Private Sub CreateMyNavBar()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.navMain.Items.Clear()
		Try
			navMain.PaintStyleName = "SkinExplorerBarView"

			' Create a Local group.
			Dim groupMandant As NavBarGroup = New NavBarGroup(("Mandantenverwaltung"))
			groupMandant.Name = "gNavMandant"


			Dim nbiMandant_Name As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Allgemein"))
			nbiMandant_Name.Name = "Show_Mandant_Name"

			Dim nbiMandant_Authorities As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Behörden/Versicherungen"))
			nbiMandant_Authorities.Name = "Show_Mandant_Authorities"

			Dim nbiMandant_Firmenkonstanten As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Lohnkonstanten"))
			nbiMandant_Firmenkonstanten.Name = "Show_Mandant_FirmenKonstanten"

			Dim nbiMandant_MarginFee As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Margen und Gebühren"))
			nbiMandant_MarginFee.Name = "Show_Mandant_MarginFee"

			Dim nbiMandant_FIBU As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("FIBU-Konten"))
			nbiMandant_FIBU.Name = "Show_Mandant_Fibu"


			Dim groupSetting As NavBarGroup = New NavBarGroup(("Einstellungen"))
			groupSetting.Name = "gNavSetting"

			Dim nbiSetting_StartNumbers As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Startnummern"))
			nbiSetting_StartNumbers.Name = "Show_Setting_Startnumbers"

			Dim nbiSetting_Standardvalues As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Standard-Werte"))
			nbiSetting_Standardvalues.Name = "Show_Setting_Standardvalues"

			Dim nbiSetting_FieldLables As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Feldbezeichnungen"))
			nbiSetting_FieldLables.Name = "Show_Setting_FieldLables"

			Dim nbiSetting_RequiredValues As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Pflichtfelder"))
			nbiSetting_RequiredValues.Name = "Show_Setting_RequiredValues"

			Dim nbiSetting_WebServices As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Webservices"))
			nbiSetting_WebServices.Name = "Show_Setting_WebServices"

			Dim nbiSetting_Jobplattforms As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Jobplattformen"))
			nbiSetting_Jobplattforms.Name = "Show_Setting_Jobplattforms"


			' Create a Extra Modules group.
			Dim groupExternModuls As NavBarGroup = New NavBarGroup(("Sonstige Module"))
			groupExternModuls.Name = "gNavMandant"

			Dim nbiExtern_CreateNewMDYear As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Neues Mandantenjahr"))
			nbiExtern_CreateNewMDYear.Name = "CreateNewMDYear"
			Dim nbiExtern_BankData As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ESR-/DTA Angaben"))
			nbiExtern_BankData.Name = "BankData"
			Dim nbiExtern_TaxAddresses As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Steuerverwaltungen"))
			nbiExtern_TaxAddresses.Name = "TaxAddresses"
			Dim nbiExtern_KiAu As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kinder-/Ausbildungszulagen"))
			nbiExtern_KiAu.Name = "KiAu"

			Dim nbiExtern_GAVAuthority As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("GAV: Organisationen"))
			nbiExtern_GAVAuthority.Name = "GAVAuthority"
			Dim nbiExtern_KTGLMV As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("GAV: KTG-Daten"))
			nbiExtern_KTGLMV.Name = "KTGLMV"
			Dim nbiExtern_TSpesenLMV As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("GAV: Spesen/Arbeitszeiten"))
			nbiExtern_TSpesenLMV.Name = "TSpesenLMV"


			navMain.BeginUpdate()

			navMain.Groups.Add(groupMandant)
			groupMandant.ItemLinks.Add(nbiMandant_Name)
			groupMandant.ItemLinks.Add(nbiMandant_Authorities)
			groupMandant.ItemLinks.Add(nbiMandant_Firmenkonstanten)
			groupMandant.ItemLinks.Add(nbiMandant_MarginFee)
			groupMandant.ItemLinks.Add(nbiMandant_FIBU)

			groupMandant.Expanded = True


			navMain.Groups.Add(groupSetting)
			groupSetting.ItemLinks.Add(nbiSetting_StartNumbers)
			groupSetting.ItemLinks.Add(nbiSetting_Standardvalues)
			groupSetting.ItemLinks.Add(nbiSetting_FieldLables)
			groupSetting.ItemLinks.Add(nbiSetting_RequiredValues)
			groupSetting.ItemLinks.Add(nbiSetting_WebServices)

			groupSetting.Expanded = True


			navMain.Groups.Add(groupExternModuls)
			groupExternModuls.ItemLinks.Add(nbiExtern_CreateNewMDYear)
			groupExternModuls.ItemLinks.Add(nbiExtern_BankData)
			groupExternModuls.ItemLinks.Add(nbiExtern_TaxAddresses)
			groupExternModuls.ItemLinks.Add(nbiExtern_KiAu)
			groupExternModuls.ItemLinks.Add(nbiExtern_GAVAuthority)
			groupExternModuls.ItemLinks.Add(nbiExtern_KTGLMV)
			groupExternModuls.ItemLinks.Add(nbiExtern_TSpesenLMV)

			groupExternModuls.Expanded = True


			navMain.EndUpdate()


			'' Create a Local group.
			'Dim groupExternModuls As NavBarGroup = New NavBarGroup(("Sonstige Module"))
			'groupExternModuls.Name = "gNavMandant"

			'navFilter.Groups.Add(groupExternModuls)

			'Dim nbiExtern_CreateNewMDYear As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Neues Mandatenjahr erstellen"))
			'nbiExtern_CreateNewMDYear.Name = "CreateNewMDYear"

			'Dim nbiExtern_GAVAuthority As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("GAV-Organisationen"))
			'nbiExtern_GAVAuthority.Name = "GAVAuthority"
			'Dim nbiExtern_TaxAddresses As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Adresse für Steuerverwaltung"))
			'nbiExtern_TaxAddresses.Name = "TaxAddresses"
			'Dim nbiExtern_BankData As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("ESR- /DTA-Angaben"))
			'nbiExtern_BankData.Name = "BankData"
			'Dim nbiExtern_KiAu As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Kinder- und Ausbildungszulagen"))
			'nbiExtern_KiAu.Name = "KiAu"
			'Dim nbiExtern_KTGLMV As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("KTG-Daten für GAV"))
			'nbiExtern_KTGLMV.Name = "KTGLMV"
			'Dim nbiExtern_TSpesenLMV As NavBarItem = New NavBarItem(m_Translate.GetSafeTranslationValue("Tagesspesen und Wochen-Stunden für GAV"))
			'nbiExtern_TSpesenLMV.Name = "TSpesenLMV"

			'navFilter.BeginUpdate()





		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Navbar Controls binden. {1}", strMethodeName, ex.Message))
			DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Fehler (navBarMain): {0}", ex.Message), "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)

		End Try

	End Sub

	''' <summary>
	''' Clickevent for Navbar.
	''' </summary>
	Private Sub OnnbMain_LinkClicked(ByVal sender As Object, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs) Handles navMain.LinkClicked

		Dim strLinkName As String = e.Link.ItemName
		Dim strLinkCaption As String = e.Link.Caption

		Dim msg = "Achtung: Ihre Daten wurden nicht gespeichert! Möchten Sie die Daten speichern?"
		If Not m_ISAlreadySaved AndAlso (m_SelectedModul <> ArtOfModul.UNKNOWN) Then
			If m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Daten speichern")) = True Then
				SaveSelectedPage(False)
			End If
		End If

		m_SelectedModul = ArtOfModul.UNKNOWN
		bbiSave.Enabled = False
		bbiExport.Enabled = False

		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bForDesign As Boolean = False

		Try
			UcMDName1.Visible = False
			UcMDPublicAuthorities1.Visible = False
			UcMDProgramm1.Visible = False
			UcMDMarginFees1.Visible = False
			ucMDFibuKonten1.Visible = False

			ucMDStartNumbers1.Visible = False
			ucStandardvalues1.Visible = False
			ucFieldLabel1.Visible = False
			ucRequiredFields1.Visible = False
			ucWebServices1.Visible = False

			For i As Integer = 0 To Me.navMain.Groups(0).NavBar.Items.Count - 1
				e.Link.NavBar.Items(i).Appearance.ForeColor = Color.Black
			Next
			e.Link.Item.Appearance.ForeColor = Color.Orange

			Select Case strLinkName.ToLower
				Case "Show_Mandant_Name".ToLower
					m_SelectedModul = ArtOfModul.MandantenName

				Case "Show_Mandant_Authorities".ToLower
					m_SelectedModul = ArtOfModul.PublicAuthorities

				Case "Show_Mandant_Firmenkonstanten".ToLower
					m_SelectedModul = ArtOfModul.Firmenkonstant

				Case "Show_Mandant_MarginFee".ToLower
					m_SelectedModul = ArtOfModul.MarginAndFees

				Case "Show_Mandant_Fibu".ToLower
					m_SelectedModul = ArtOfModul.FIBUKonten


				Case "Show_Setting_Startnumbers".ToLower
					m_SelectedModul = ArtOfModul.StartNumbers

				Case "Show_Setting_Standardvalues".ToLower
					m_SelectedModul = ArtOfModul.StandardValues

				Case "Show_Setting_FieldLables".ToLower
					m_SelectedModul = ArtOfModul.FieldLable

				Case "Show_Setting_RequiredValues".ToLower
					m_SelectedModul = ArtOfModul.RequiredFields

				Case "Show_Setting_WebServices".ToLower
					m_SelectedModul = ArtOfModul.WebServices

				Case "Show_Setting_Jobplattforms".ToLower
					m_SelectedModul = ArtOfModul.Jobplattforms




				Case "CreateNewMDYear".ToLower
					Dim frm As New SPS.MD.CreateNewUtility.ClsMain_Net(New SPS.MD.CreateNewUtility.ClsSetting With {.SelectedMDNr = m_InitializationData.MDData.MDNr,
																																																					.SelectedMDYear = m_InitializationData.MDData.MDYear,
																																																					.SelectedMDGuid = String.Empty,
																																																					.LogedUSNr = m_InitializationData.UserData.UserNr})
					frm.ShowfrmCreateNewYear()
					Return

				Case "GAVAuthority".ToLower
					Dim frm As New SPS.MD.GAVAddressUtility.ClsMain_Net(m_InitializationData)
					frm.ShowfrmGAVAddress()

				Case "TaxAddresses".ToLower
					Dim frm As New SPS.MD.QstAddressUtility.ClsMain_Net(New SPS.MD.QstAddressUtility.ClsSetting With {.SelectedMDNr = m_InitializationData.MDData.MDNr,
																																																.SelectedMDYear = m_InitializationData.MDData.MDYear,
																																																.SelectedMDGuid = String.Empty,
																																																.LogedUSNr = m_InitializationData.UserData.UserNr})
					frm.ShowfrmQstAddress()

				Case "BankData".ToLower
					Dim frm As New SPS.MD.ESRDTAUtility.UI.frmESRDTA(m_InitializationData)
					frm.LoadData()
					frm.Show()
					frm.BringToFront()

				Case "KiAu".ToLower
					Dim frm As New frmMDFak(m_InitializationData)
					frm.LoadChildeducationData()

					frm.Show()
					frm.BringToFront()

				Case "KTGLMV".ToLower
					Dim frm As New frmMDLmvKtg(m_InitializationData)
					frm.LoadKTGForLMVData()

					frm.Show()
					frm.BringToFront()

				Case "TSpesenLMV".ToLower
					Dim frm As New frmMDLmvTSpesen(m_InitializationData)
					frm.LoadTSpesenForLMVData()

					frm.Show()
					frm.BringToFront()


				Case Else
					m_SelectedModul = ArtOfModul.UNKNOWN


			End Select
			If m_SelectedModul = ArtOfModul.UNKNOWN Then Return

			Dim success = LoadSelectedPage()
			bbiSave.Enabled = success


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			bbiSave.Enabled = False

		End Try
		m_ISAlreadySaved = False


	End Sub

	Public Sub loadInitData()

		LoadMandantenDropDown()
		lueMandant.EditValue = m_InitializationData.MDData.MDNr

		LoadYearDropDownData()

	End Sub

	Private Function LoadSelectedPage() As Boolean
		Dim success As Boolean = True

		bbiExport.Enabled = False
		If lueYear.EditValue Is Nothing Then Return False

		Select Case m_SelectedModul
			Case ArtOfModul.MandantenName
				showMandantNameFields()
				bbiExport.Enabled = True

			Case ArtOfModul.PublicAuthorities
				showMandantPublicAuthoritiesFields()
				bbiExport.Enabled = True

			Case ArtOfModul.Firmenkonstant
				showMandantFirmenkonstanten()
				bbiExport.Enabled = True

			Case ArtOfModul.MarginAndFees
				showMandantMarginFeesFields()
				bbiExport.Enabled = True

			Case ArtOfModul.FIBUKonten
				showMandantFibuKontenFields()
				bbiExport.Enabled = True

			Case ArtOfModul.StartNumbers
				showSettingStartNumbers()

			Case ArtOfModul.StandardValues
				showSettingStandardValues()

			Case ArtOfModul.FieldLable
				showSettingFieldLabels()

			Case ArtOfModul.RequiredFields
				showSettingRequiredValues()

			Case ArtOfModul.WebServices
				showSettingWebservices()

			Case ArtOfModul.Jobplattforms
				showSettingJobplattforms()


			Case Else
				Return False

		End Select
		m_ISAlreadySaved = False

		Return success

	End Function

	Private Function ReloadSelectedPage() As Boolean
		Dim success As Boolean = True

		If lueYear.EditValue Is Nothing Then Return False

		Dim _Year As Integer = lueYear.EditValue
		UcMDName1.Visible = False
		UcMDPublicAuthorities1.Visible = False
		UcMDProgramm1.Visible = False
		UcMDMarginFees1.Visible = False
		ucMDFibuKonten1.Visible = False

		ucMDStartNumbers1.Visible = False
		ucStandardvalues1.Visible = False
		ucFieldLabel1.Visible = False
		ucRequiredFields1.Visible = False
		ucWebServices1.Visible = False


		UcMDName1.InitWithConfigurationData(m_InitializationData, m_Translate, _Year)
		UcMDName1.Dock = DockStyle.Fill
		success = success AndAlso UcMDName1.IsDataValid

		If success Then
			UcMDPublicAuthorities1.InitWithConfigurationData(m_InitializationData, m_Translate, _Year)
			UcMDPublicAuthorities1.Dock = DockStyle.Fill
			success = success AndAlso UcMDPublicAuthorities1.IsDataValid
		End If

		If success Then
			UcMDProgramm1.InitWithConfigurationData(m_InitializationData, m_Translate, _Year)
			UcMDProgramm1.Dock = DockStyle.Fill
			success = success AndAlso UcMDProgramm1.IsDataValid
		End If

		If success Then
			UcMDMarginFees1.InitWithConfigurationData(m_InitializationData, m_Translate, _Year)
			UcMDMarginFees1.Dock = DockStyle.Fill
			success = success AndAlso UcMDMarginFees1.IsDataValid
		End If

		If success Then
			ucMDFibuKonten1.InitWithConfigurationData(m_InitializationData, m_Translate, _Year)
			ucMDFibuKonten1.Dock = DockStyle.Fill
			success = success AndAlso ucMDFibuKonten1.IsDataValid
		End If

		If success Then
			ucMDStartNumbers1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
			ucMDStartNumbers1.Dock = DockStyle.Fill
			ucMDStartNumbers1.Visible = False
		End If

		If success Then
			ucStandardvalues1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
			ucStandardvalues1.Dock = DockStyle.Fill
			ucStandardvalues1.Visible = False
		End If

		If success Then
			ucFieldLabel1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
			ucFieldLabel1.Dock = DockStyle.Fill
			ucFieldLabel1.Visible = False
		End If

		If success Then
			ucRequiredFields1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
			ucRequiredFields1.Dock = DockStyle.Fill
			ucRequiredFields1.Visible = False
		End If

		If success Then
			ucWebServices1.InitWithConfigurationData(m_InitializationData, m_Translate, m_InitializationData.MDData.MDYear)
			ucWebServices1.Dock = DockStyle.Fill
			ucWebServices1.Visible = False
		End If

		success = success AndAlso LoadSelectedPage()

		bbiSave.Enabled = success
		If Not success Then m_SelectedModul = ArtOfModul.UNKNOWN

		Return success

	End Function

	Private Sub SaveSelectedPage(ByVal showMessage As Boolean)

		Dim success As Boolean = True
		If lueYear.EditValue Is Nothing Then Return

		Select Case m_SelectedModul
			Case ArtOfModul.MandantenName
				success = UcMDName1.SaveMandantenData()

			Case ArtOfModul.PublicAuthorities
				success = UcMDPublicAuthorities1.SaveMandantenData()

			Case ArtOfModul.Firmenkonstant
				success = UcMDProgramm1.SaveMandantenData()

			Case ArtOfModul.MarginAndFees
				success = UcMDMarginFees1.SaveMandantenData()

			Case ArtOfModul.FIBUKonten
				success = ucMDFibuKonten1.SaveMandantenData()


			Case ArtOfModul.StartNumbers
				success = ucMDStartNumbers1.SaveMandantenData()

			Case ArtOfModul.StandardValues
				success = ucStandardvalues1.SaveSettingData()

			Case ArtOfModul.FieldLable
				success = ucFieldLabel1.SaveSettingData()

			Case ArtOfModul.RequiredFields
				success = ucRequiredFields1.SaveSettingData()

			Case ArtOfModul.WebServices
				success = ucWebServices1.SaveSettingData()

			Case ArtOfModul.Jobplattforms
				success = ucWebServices1.SaveSettingData()



			Case Else
				Return

		End Select
		m_ISAlreadySaved = True

		If Not showMessage Then Return
		If success Then
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))

		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
			Return

		End If

	End Sub



	Private Sub Onfrm_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
		SaveFromSettings()
	End Sub

	''' <summary>
	''' Loads form settings if form gets visible.
	''' </summary>
	Private Sub OnFrm_VisibleChanged(sender As System.Object, e As System.EventArgs) Handles MyBase.VisibleChanged

		If Visible Then
			LoadFormSettings()
		End If

	End Sub

	''' <summary>
	''' Loads form settings.
	''' </summary>
	Private Sub LoadFormSettings()

		Try
			Dim setting_form_height = My.Settings.ifrmHeight
			Dim setting_form_width = My.Settings.ifrmWidth
			Dim setting_form_location = My.Settings.frmLocation

			If setting_form_height > 0 Then Me.Height = Math.Max(Me.Height, setting_form_height)
			If setting_form_width > 0 Then Me.Width = Math.Max(Me.Width, setting_form_width)

			If Not String.IsNullOrEmpty(setting_form_location) Then
				Dim aLoc As String() = setting_form_location.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub

	''' <summary>
	''' Saves the form settings.
	''' </summary>
	Private Sub SaveFromSettings()

		' Save form location, width and height in setttings
		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmWidth = Me.Width
				My.Settings.ifrmHeight = Me.Height

				My.Settings.Save()

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())

		End Try

	End Sub



#End Region


	Sub showMandantNameFields()

		Dim success As Boolean = True

		success = success And UcMDName1.LoadMandantenData()

		UcMDName1.Visible = success
		If success Then UcMDName1.Dock = DockStyle.Fill

	End Sub

	Sub showMandantPublicAuthoritiesFields()

		Dim success As Boolean = True

		success = success And UcMDPublicAuthorities1.LoadMandantenData()

		UcMDPublicAuthorities1.Visible = success
		If success Then UcMDPublicAuthorities1.Dock = DockStyle.Fill

	End Sub

	Sub showMandantFirmenkonstanten()

		Dim success As Boolean = True

		success = success And UcMDProgramm1.LoadMandantenData()

		UcMDProgramm1.Visible = success
		If success Then UcMDProgramm1.Dock = DockStyle.Fill

	End Sub

	Sub showMandantMarginFeesFields()

		Dim success As Boolean = True

		success = success And UcMDMarginFees1.LoadMandantenData()

		UcMDMarginFees1.Visible = success
		If success Then UcMDMarginFees1.Dock = DockStyle.Fill

	End Sub

	Sub showMandantFibuKontenFields()

		Dim success As Boolean = True

		success = success And ucMDFibuKonten1.LoadMandantenData()

		ucMDFibuKonten1.Visible = success
		If success Then ucMDFibuKonten1.Dock = DockStyle.Fill

	End Sub


#Region "Settings"

	Sub showSettingStartNumbers()

		Dim success As Boolean = True

		success = success And ucMDStartNumbers1.LoadMandantenData()

		ucMDStartNumbers1.Visible = success
		If success Then ucMDStartNumbers1.Dock = DockStyle.Fill

	End Sub

	Sub showSettingStandardValues()

		Dim success As Boolean = True

		success = success And ucStandardvalues1.LoadSettingData()

		ucStandardvalues1.Visible = success
		If success Then ucStandardvalues1.Dock = DockStyle.Fill

	End Sub

	Sub showSettingFieldLabels()

		Dim success As Boolean = True

		success = success And ucFieldLabel1.LoadSettingData()

		ucFieldLabel1.Visible = success
		If success Then ucFieldLabel1.Dock = DockStyle.Fill

	End Sub

	Sub showSettingRequiredValues()

		Dim success As Boolean = True

		success = success And ucRequiredFields1.LoadSettingData()

		ucRequiredFields1.Visible = success
		If success Then ucRequiredFields1.Dock = DockStyle.Fill

	End Sub

	Sub showSettingWebservices()

		Dim success As Boolean = True

		success = success And ucWebServices1.LoadSettingData()

		ucWebServices1.Visible = success
		If success Then ucWebServices1.Dock = DockStyle.Fill

	End Sub

	Sub showSettingJobplattforms()

		Dim success As Boolean = True

		success = success And ucWebServices1.LoadSettingData()

		ucWebServices1.Visible = success
		If success Then ucWebServices1.Dock = DockStyle.Fill

	End Sub



#End Region

	Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

		Me.Close()

	End Sub

	Private Sub OnbbiSave_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
		SaveSelectedPage(True)
	End Sub

	Private Sub OnbbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiExport.DropDownControl

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub


	'Private Sub CreateExportPopupMenu()
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim bshowMnu As Boolean = True
	'	Dim popupMenu As New DevExpress.XtraBars.PopupMenu
	'	Dim liMnu As New List(Of String) From {"Neues Mandatenjahr erstellen...#CreateNewMDYear",
	'																				 "-GAV-Organisationen...#GAVAuthority",
	'																				 "Adresse für Steuerverwaltung...#TaxAddresses",
	'																				 "-ESR- /DTA-Angaben...#BankData",
	'																				 "Kinder- und Ausbildungszulagen...#KiAu",
	'																				 "-Angaben über KTG für LMV...#KTGLMV",
	'																				 "Angaben über Tagesspesen und Wochenstunde für LMV...#TSpesenLMV"}
	'	Try
	'		bbiExport.Manager = Me.BarManager1
	'		BarManager1.ForceInitialize()

	'		Me.bbiExport.ActAsDropDown = False
	'		Me.bbiExport.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
	'		Me.bbiExport.DropDownEnabled = True
	'		Me.bbiExport.DropDownControl = popupMenu
	'		Me.bbiExport.Enabled = True

	'		For i As Integer = 0 To liMnu.Count - 1
	'			Dim beginGroupping = False
	'			Dim myValue As String() = liMnu(i).Split(CChar("#"))
	'			If bshowMnu Then
	'				popupMenu.Manager = BarManager1

	'				Dim itm As New DevExpress.XtraBars.BarButtonItem
	'				Dim mnuCaption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
	'				If mnuCaption.StartsWith("-") Then
	'					beginGroupping = True
	'					mnuCaption = mnuCaption.Remove(0, 1)
	'				End If
	'				itm.Caption = m_Translate.GetSafeTranslationValue(mnuCaption)
	'				itm.Name = myValue(1).ToString

	'				If beginGroupping Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
	'				AddHandler itm.ItemClick, AddressOf GetExportMenuItem
	'			End If
	'		Next

	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	End Try

	'End Sub

	'Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)

	'	Select Case UCase(e.Item.Name.ToUpper)
	'		Case UCase("CreateNewMDYear")
	'			Dim frm As New SPS.MD.CreateNewUtility.ClsMain_Net(New SPS.MD.CreateNewUtility.ClsSetting With {.SelectedMDNr = m_InitializationData.MDData.MDNr,
	'																																																			.SelectedMDYear = m_InitializationData.MDData.MDYear,
	'																																																			.SelectedMDGuid = String.Empty,
	'																																																			.LogedUSNr = m_InitializationData.UserData.UserNr})
	'			frm.ShowfrmCreateNewYear()

	'		Case UCase("GAVAuthority")
	'			Dim frm As New SPS.MD.GAVAddressUtility.ClsMain_Net(m_InitializationData)
	'			frm.ShowfrmGAVAddress()

	'		Case UCase("TaxAddresses")
	'			Dim frm As New SPS.MD.QstAddressUtility.ClsMain_Net(New SPS.MD.QstAddressUtility.ClsSetting With {.SelectedMDNr = m_InitializationData.MDData.MDNr,
	'																																																			.SelectedMDYear = m_InitializationData.MDData.MDYear,
	'																																																			.SelectedMDGuid = String.Empty,
	'																																																			.LogedUSNr = m_InitializationData.UserData.UserNr})
	'			frm.ShowfrmQstAddress()

	'		Case UCase("BankData")
	'			Dim frm As New SPS.MD.ESRDTAUtility.UI.frmESRDTA(m_InitializationData)
	'			frm.LoadData()
	'			frm.Show()
	'			frm.BringToFront()

	'		Case UCase("KiAu")
	'			Dim frm As New frmMDFak(m_InitializationData)
	'			frm.LoadChildeducationData()

	'			frm.Show()
	'			frm.BringToFront()


	'		Case UCase("KTGLMV")
	'			Dim frm As New frmMDLmvKtg(m_InitializationData)
	'			frm.LoadKTGForLMVData()

	'			frm.Show()
	'			frm.BringToFront()

	'		Case UCase("TSpesenLMV")
	'			Dim frm As New frmMDLmvTSpesen(m_InitializationData)
	'			frm.LoadTSpesenForLMVData()

	'			frm.Show()
	'			frm.BringToFront()


	'		Case Else
	'			Return


	'	End Select

	'End Sub



	Private Class YearValueView
		Public Property Value As Integer

	End Class


End Class