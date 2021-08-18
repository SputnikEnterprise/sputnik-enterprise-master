
Option Strict Off

Imports DevExpress.LookAndFeel
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraEditors.Controls
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.Infrastructure.UI
Imports System.ComponentModel
Imports DevExpress.XtraTab
Imports SP.DatabaseAccess.Common.DataObjects
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.DXErrorProvider
Imports SPS.MD.ESRDTAUtility.SPBankUtilWebService
Imports DevExpress.XtraEditors.Repository

Namespace UI

	Public Class frmESRDTA
		Inherits DevExpress.XtraEditors.XtraForm

#Region "Constants"

		'Private Const MANDANT_XML_SETTING_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webserviceibanutil"
		'Public Const DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPIBANUtil.asmx"

		'Private Const MANDANT_XML_SETTING_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicebankdatabase"
		Private Const DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "wssps_services/spbankutil.asmx"

#End Region


		''' <summary>
		''' The logger.
		''' </summary>
		Private m_Logger As ILogger = New Logger()

		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		Private m_SuppressUIEvents As Boolean = False

		Private m_TableSettingDatabaseAccess As SP.DatabaseAccess.TableSetting.ITablesDatabaseAccess

		Private m_BankUtilWebServiceUri As String
		Private m_StandardImage As Image

		Private m_common As New CommonSetting
		Private m_utility As New Utilities
		Private m_UtilityUI As UtilityUI
		Private m_Mandant As Mandant

		Private m_MandantBankData As IEnumerable(Of MDBankData)
		Private m_CurrentBankData As MDBankData
		Private m_ConnectionString As String


#Region "Contructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()
			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_Mandant = New Mandant
			m_utility = New Utilities
			m_UtilityUI = New UtilityUI
			m_SuppressUIEvents = True

			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Me.KeyPreview = True
			Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			m_ConnectionString = m_InitializationData.MDData.MDDbConn
			m_TableSettingDatabaseAccess = New SP.DatabaseAccess.TableSetting.TablesDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)

			Dim domainName = m_InitializationData.MDData.WebserviceDomain
			m_BankUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			TranslateControls()
			Reset()

			mainTab.SelectedTabPage = xtabDTA
			m_StandardImage = My.Resources.Checked

			m_SuppressUIEvents = True

			AddHandler gvDTA.RowCellClick, AddressOf OngvDTA_RowCellClick
			AddHandler gvESR.RowCellClick, AddressOf OngvESR_RowCellClick

		End Sub

#End Region


#Region "Pulbic Methodes"

		Public Sub LoadData()

			m_SuppressUIEvents = True
			LoadMandantenDropDown()

			lueMandant.EditValue = m_InitializationData.MDData.MDNr

			LoadMandantBankData()
			If m_MandantBankData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Mandanten Bankdaten konnten nicht geladen werden."))

				Return
			End If
			'If mainTab.SelectedTabPage Is xtabDTA Then
			'	grdDTA.DataSource = m_MandantBankData
			'Else
			'	grdESR.DataSource = m_MandantBankData
			'End If


			m_SuppressUIEvents = False

		End Sub

#End Region

#Region "private properties"

		''' <summary>
		''' Gets the selected document.
		''' </summary>
		''' <returns>The selected employee or nothing if none is selected.</returns>
		Private ReadOnly Property SelectedDTARecord As MDBankData
			Get
				Dim gv = TryCast(grdDTA.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gv Is Nothing) Then

					Dim selectedRows = gv.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim data = CType(gv.GetRow(selectedRows(0)), MDBankData)
						Return data
					End If

				End If

				Return Nothing
			End Get

		End Property

		Private ReadOnly Property SelectedESRRecord As MDBankData
			Get
				Dim gv = TryCast(grdESR.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gv Is Nothing) Then

					Dim selectedRows = gv.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim data = CType(gv.GetRow(selectedRows(0)), MDBankData)
						Return data
					End If

				End If

				Return Nothing
			End Get

		End Property

#End Region

		Private Sub Reset()

			ResetDTAFields()
			ResetESRFields()

			ResetMandantenDropDown()

			ResetDTAGrid()
			ResetESRGridData()


			ErrorProvider.ClearErrors()

		End Sub

		Private Sub ResetDTAFields()

			' dta fields
			txtDTA_RecBez.EditValue = String.Empty
			txtDTA_MD_ID.EditValue = String.Empty
			txtDTA_DTAClnr.EditValue = String.Empty
			txtDTA_KontoDTA.EditValue = String.Empty
			txtDTA_KontoVG.EditValue = String.Empty
			beDTA_Swift.EditValue = String.Empty
			txtDTA_DTAIBAN.EditValue = String.Empty
			txtDTA_VGIBAN.EditValue = String.Empty
			txtDTA_DTAAdr1.EditValue = String.Empty
			txtDTA_DTAAdr2.EditValue = String.Empty
			txtDTA_DTAAdr3.EditValue = String.Empty
			txtDTA_DTAAdr4.EditValue = String.Empty
			chkDTA_AsStandard.Checked = False

		End Sub

		Private Sub ResetESRFields()

			' esr fields
			txtESR_RecBez.EditValue = String.Empty
			txtESR_MD_ID.EditValue = String.Empty
			txtESR_KontoESR1.EditValue = String.Empty
			txtESR_KontoESR2.EditValue = String.Empty
			txtESR_Bankname.EditValue = String.Empty
			txtESR_BankClnr.EditValue = String.Empty
			txtESR_BankAdresse.EditValue = String.Empty
			beESR_Swift.EditValue = String.Empty
			txtESR_ESRIBAN1.EditValue = String.Empty
			txtESR_ESRIBAN2.EditValue = String.Empty
			txtESR_ESRIBAN3.EditValue = String.Empty
			chkESR_AsStandard.Checked = False

		End Sub

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

		Private Sub ResetDTAGrid()

			gvDTA.OptionsView.ShowIndicator = False
			gvDTA.OptionsView.ShowAutoFilterRow = True
			gvDTA.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

			gvDTA.Columns.Clear()

			' Important symbol.
			'm_CheckEditDedfaultBank = CType(grdDTA.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			'm_CheckEditDedfaultBank.PictureChecked = My.Resources.Checked
			'm_CheckEditDedfaultBank.PictureUnchecked = Nothing
			'm_CheckEditDedfaultBank.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

			Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			columnID.Visible = False
			gvDTA.Columns.Add(columnID)

			Dim columnRecBez As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRecBez.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnRecBez.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnRecBez.Name = "RecBez"
			columnRecBez.FieldName = "RecBez"
			columnRecBez.BestFit()
			columnRecBez.Visible = True
			gvDTA.Columns.Add(columnRecBez)

			Dim columnSwift As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSwift.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnSwift.Caption = m_Translate.GetSafeTranslationValue("Swift")
			columnSwift.Name = "Swift"
			columnSwift.FieldName = "Swift"
			columnSwift.BestFit()
			columnSwift.Visible = True
			gvDTA.Columns.Add(columnSwift)

			Dim columnDTAClnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDTAClnr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDTAClnr.Caption = m_Translate.GetSafeTranslationValue("Clearing-Nummer")
			columnDTAClnr.Name = "DTAClnr"
			columnDTAClnr.FieldName = "DTAClnr"
			columnDTAClnr.BestFit()
			columnDTAClnr.Visible = True
			gvDTA.Columns.Add(columnDTAClnr)

			Dim columnDTAIBAN As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDTAIBAN.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDTAIBAN.Caption = m_Translate.GetSafeTranslationValue("IBAN-Nummer")
			columnDTAIBAN.Name = "DTAIBAN"
			columnDTAIBAN.FieldName = "DTAIBAN"
			columnDTAIBAN.BestFit()
			columnDTAIBAN.Visible = True
			gvDTA.Columns.Add(columnDTAIBAN)

			Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			activeColumn.Caption = " "
			activeColumn.Name = "AsStandard"
			activeColumn.FieldName = "AsStandard"
			activeColumn.Visible = True
			activeColumn.Width = 20
			gvDTA.Columns.Add(activeColumn)


			grdDTA.DataSource = Nothing


		End Sub

		Private Sub ResetESRGridData()

			gvESR.OptionsView.ShowIndicator = False
			gvESR.OptionsView.ShowAutoFilterRow = True
			gvESR.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

			gvESR.Columns.Clear()

			Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnID.Caption = m_Translate.GetSafeTranslationValue("ID")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			columnID.Visible = False
			gvESR.Columns.Add(columnID)

			Dim columnRecBez As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRecBez.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnRecBez.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnRecBez.Name = "RecBez"
			columnRecBez.FieldName = "RecBez"
			columnRecBez.BestFit()
			columnRecBez.Visible = True
			gvESR.Columns.Add(columnRecBez)

			Dim columnDisplayName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDisplayName.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDisplayName.Caption = m_Translate.GetSafeTranslationValue("Bank")
			columnDisplayName.Name = "DisplayName"
			columnDisplayName.FieldName = "DisplayName"
			columnDisplayName.BestFit()
			columnDisplayName.Visible = True
			gvESR.Columns.Add(columnDisplayName)

			Dim columnSwift As New DevExpress.XtraGrid.Columns.GridColumn()
			columnSwift.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnSwift.Caption = m_Translate.GetSafeTranslationValue("Swift")
			columnSwift.Name = "Swift"
			columnSwift.FieldName = "Swift"
			columnSwift.BestFit()
			columnSwift.Visible = True
			gvESR.Columns.Add(columnSwift)

			Dim columnBankClnr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBankClnr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBankClnr.Caption = m_Translate.GetSafeTranslationValue("Clearing-Nummer")
			columnBankClnr.Name = "BankClnr"
			columnBankClnr.FieldName = "BankClnr"
			columnBankClnr.BestFit()
			columnBankClnr.Visible = False
			gvESR.Columns.Add(columnBankClnr)

			Dim columnKontoESR2 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKontoESR2.Caption = m_Translate.GetSafeTranslationValue("Konto-Nummer")
			columnKontoESR2.Name = "KontoESR2"
			columnKontoESR2.FieldName = "KontoESR2"
			columnKontoESR2.Visible = True
			gvESR.Columns.Add(columnKontoESR2)

			Dim columnESRIBAN1 As New DevExpress.XtraGrid.Columns.GridColumn()
			columnESRIBAN1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnESRIBAN1.Caption = m_Translate.GetSafeTranslationValue("IBAN-Nummer")
			columnESRIBAN1.Name = "ESRIBAN1"
			columnESRIBAN1.FieldName = "ESRIBAN1"
			columnESRIBAN1.BestFit()
			columnESRIBAN1.Visible = True
			gvESR.Columns.Add(columnESRIBAN1)

			Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			activeColumn.Caption = " "
			activeColumn.Name = "AsStandard"
			activeColumn.FieldName = "AsStandard"
			activeColumn.Visible = True
			activeColumn.Width = 20
			gvESR.Columns.Add(activeColumn)


			grdESR.DataSource = Nothing

		End Sub

		Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Lbl_Header1.Text = m_Translate.GetSafeTranslationValue(Lbl_Header1.Text)
			Lbl_Header2.Text = m_Translate.GetSafeTranslationValue(Lbl_Header2.Text)

			Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

			lblMDName.Text = m_Translate.GetSafeTranslationValue(lblMDName.Text)

			xtabDTA.Text = m_Translate.GetSafeTranslationValue(xtabDTA.Text)
			xtabESR.Text = m_Translate.GetSafeTranslationValue(xtabESR.Text)

			lbl_ESRBezeichnung.Text = m_Translate.GetSafeTranslationValue(lbl_ESRBezeichnung.Text)
			lbl_ESRIdentifikation.Text = m_Translate.GetSafeTranslationValue(lbl_ESRIdentifikation.Text)
			lbl_ESRKonto.Text = m_Translate.GetSafeTranslationValue(lbl_ESRKonto.Text)
			lbl_ESRKonto2.Text = m_Translate.GetSafeTranslationValue(lbl_ESRKonto2.Text)
			lbl_ESRBankname.Text = m_Translate.GetSafeTranslationValue(lbl_ESRBankname.Text)
			lbl_Clearingnummer.Text = m_Translate.GetSafeTranslationValue(lbl_Clearingnummer.Text)
			lbl_BankAdresse.Text = m_Translate.GetSafeTranslationValue(lbl_BankAdresse.Text)

			lbl_IBAN1.Text = m_Translate.GetSafeTranslationValue(lbl_IBAN1.Text)
			lbl_IBAN2.Text = m_Translate.GetSafeTranslationValue(lbl_IBAN2.Text)
			lbl_IBAN3.Text = m_Translate.GetSafeTranslationValue(lbl_IBAN3.Text)
			lbl_Swift.Text = m_Translate.GetSafeTranslationValue(lbl_Swift.Text)
			lblIID.Text = m_Translate.GetSafeTranslationValue(lblIID.Text)

			Lbl_DTABez.Text = m_Translate.GetSafeTranslationValue(Lbl_DTABez.Text)
			Lbl_DTAZeile1.Text = m_Translate.GetSafeTranslationValue(Lbl_DTAZeile1.Text)
			Lbl_DTAZeile2.Text = m_Translate.GetSafeTranslationValue(Lbl_DTAZeile2.Text)
			Lbl_DTAZeile3.Text = m_Translate.GetSafeTranslationValue(Lbl_DTAZeile3.Text)
			Lbl_DTAZeile4.Text = m_Translate.GetSafeTranslationValue(Lbl_DTAZeile4.Text)

			Lbl_DTAKontoVG.Text = m_Translate.GetSafeTranslationValue(Lbl_DTAKontoVG.Text)
			Lbl_DTAIBANVG.Text = m_Translate.GetSafeTranslationValue(Lbl_DTAIBANVG.Text)
			Lbl_DTAClearing.Text = m_Translate.GetSafeTranslationValue(Lbl_DTAClearing.Text)
			Lbl_DTAKontoDTA.Text = m_Translate.GetSafeTranslationValue(Lbl_DTAKontoDTA.Text)

			Lbl_DTAIBANDTA.Text = m_Translate.GetSafeTranslationValue(Lbl_DTAIBANDTA.Text)

			chkDTA_AsStandard.Text = m_Translate.GetSafeTranslationValue(chkDTA_AsStandard.Text)
			chkESR_AsStandard.Text = m_Translate.GetSafeTranslationValue(chkESR_AsStandard.Text)

			bbiNew.Caption = m_Translate.GetSafeTranslationValue(bbiNew.Caption)
			bbiSave.Caption = m_Translate.GetSafeTranslationValue(bbiSave.Caption)
			bbiDelete.Caption = m_Translate.GetSafeTranslationValue(bbiDelete.Caption)

		End Sub


#Region "Lookup Edit Reset und Load..."


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

		' Mandantendaten...
		Private Sub lueMandant_EditValueChanged(sender As Object, e As System.EventArgs) Handles lueMandant.EditValueChanged

			If m_SuppressUIEvents Then Return

			If lueMandant.EditValue IsNot Nothing Then
				If m_InitializationData.MDData.MDNr <> lueMandant.EditValue Then
					Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation

					Dim clsMandant = m_Mandant.GetSelectedMDData(lueMandant.EditValue)
					Dim logedUserData = m_Mandant.GetSelectedUserData(clsMandant.MDNr, m_InitializationData.UserData.UserNr)
					Dim personalizedData = m_InitializationData.ProsonalizedData
					Dim translate = m_InitializationData.TranslationData

					m_InitializationData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

				End If
				LoadMandantBankData()
				If mainTab.SelectedTabPage Is xtabDTA Then FocusDTArecord(m_MandantBankData(0).ID) Else FocusESRrecord(m_MandantBankData(0).ID)

			End If
			Me.bbiSave.Enabled = Not (m_InitializationData.MDData Is Nothing)
			Me.bbiDelete.Enabled = Not (m_InitializationData.MDData Is Nothing)

		End Sub


#End Region


		Private Sub frmQstAddress_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

			Try
				If My.Settings.frmLocation <> String.Empty Then
					Me.Width = Math.Max(My.Settings.iWidth, Me.Width)
					Me.Height = Math.Max(My.Settings.iHeight, Me.Height)
					Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))

					If Screen.AllScreens.Length = 1 Then
						If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
					End If
					Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("FormStyle: {0}", ex.ToString))

			End Try

		End Sub

		Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
			Me.Dispose()
		End Sub

		Private Sub frmQstAddress_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

			Try
				If Not Me.WindowState = FormWindowState.Minimized Then
					My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
					My.Settings.iWidth = Me.Width
					My.Settings.iHeight = Me.Height

					My.Settings.Save()
				End If

			Catch ex As Exception
				' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
			End Try

		End Sub


#Region "Funktionen zur Menüaufbau..."

		Private Sub ResetAllTabEntries()

			For Each ctrls In Me.Controls
				If Not ctrls.name.tolower.contains("luemandant") Then
					ResetControl(ctrls)
				End If
			Next

		End Sub

		Private Sub ResetControl(ByVal con As Control)
			If TypeOf (con) Is TextBox Then
				Dim tb As TextBox = con
				tb.Text = String.Empty

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
				Dim tb As DevExpress.XtraEditors.TextEdit = con
				tb.EditValue = String.Empty

			ElseIf TypeOf (con) Is GroupBox Then
				Dim grp As Control = con
				For Each con2 In grp.Controls
					ResetControl(con2)
				Next

			End If

		End Sub

#End Region

		Private Sub bbiSave_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
			Dim success As Boolean = True
			Dim msg As String = String.Empty

			success = success AndAlso SaveMandantBankData()

			If success Then
				msg = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
				m_UtilityUI.ShowInfoDialog(msg)

			Else
				msg = m_Translate.GetSafeTranslationValue("Die Daten konnten nicht gespeichert werden.")
				m_UtilityUI.ShowErrorDialog(msg)

			End If

		End Sub

		Private Function SaveMandantBankData() As Boolean
			Dim success As Boolean = True

			success = success AndAlso ValidateInputData()
			If Not success Then Return success

			Try
				If m_CurrentBankData Is Nothing Then m_CurrentBankData = New MDBankData

				If mainTab.SelectedTabPage Is xtabDTA Then
					m_CurrentBankData.ModulArt = BankModulEnum.DTADATA

					m_CurrentBankData.RecBez = txtDTA_RecBez.EditValue
					m_CurrentBankData.MD_ID = txtDTA_MD_ID.EditValue
					m_CurrentBankData.DTAClnr = txtDTA_DTAClnr.EditValue
					m_CurrentBankData.KontoDTA = txtDTA_KontoDTA.EditValue
					m_CurrentBankData.KontoVG = txtDTA_KontoVG.EditValue
					m_CurrentBankData.Swift = beDTA_Swift.EditValue
					m_CurrentBankData.DTAIBAN = txtDTA_DTAIBAN.EditValue
					m_CurrentBankData.VGIBAN = txtDTA_VGIBAN.EditValue
					m_CurrentBankData.DTAAdr1 = txtDTA_DTAAdr1.EditValue
					m_CurrentBankData.DTAAdr2 = txtDTA_DTAAdr2.EditValue
					m_CurrentBankData.DTAAdr3 = txtDTA_DTAAdr3.EditValue
					m_CurrentBankData.DTAAdr4 = txtDTA_DTAAdr4.EditValue
					m_CurrentBankData.AsStandard = chkDTA_AsStandard.Checked

				Else
					m_CurrentBankData.ModulArt = BankModulEnum.ESRDATA

					m_CurrentBankData.RecBez = txtESR_RecBez.EditValue
					m_CurrentBankData.MD_ID = txtESR_MD_ID.EditValue
					m_CurrentBankData.KontoESR1 = txtESR_KontoESR1.EditValue
					m_CurrentBankData.KontoESR2 = txtESR_KontoESR2.EditValue
					m_CurrentBankData.BankName = txtESR_Bankname.EditValue
					m_CurrentBankData.BankClnr = txtESR_BankClnr.EditValue
					m_CurrentBankData.BankAdresse = txtESR_BankAdresse.EditValue
					m_CurrentBankData.Swift = beESR_Swift.EditValue
					m_CurrentBankData.ESRIBAN1 = txtESR_ESRIBAN1.EditValue
					m_CurrentBankData.ESRIBAN2 = txtESR_ESRIBAN2.EditValue
					m_CurrentBankData.ESRIBAN3 = txtESR_ESRIBAN3.EditValue
					m_CurrentBankData.AsStandard = chkESR_AsStandard.Checked

				End If

				m_CurrentBankData.MDNr = lueMandant.EditValue
				m_CurrentBankData.USNr = m_InitializationData.UserData.UserNr

				If m_CurrentBankData.ID.GetValueOrDefault(0) = 0 Then
					m_CurrentBankData.CreatedFrom = m_InitializationData.UserData.UserFullName

					success = m_TableSettingDatabaseAccess.AddNewMandantBankData(m_CurrentBankData)

				Else
					m_CurrentBankData.ChangedFrom = m_InitializationData.UserData.UserFullName
					success = m_TableSettingDatabaseAccess.UpdateAssignedMandantBankData(m_CurrentBankData)

				End If

				If success Then
					LoadMandantBankData()
					m_SuppressUIEvents = True

					If mainTab.SelectedTabPage Is xtabDTA Then
						FocusDTArecord(m_CurrentBankData.ID)
					Else
						FocusESRrecord(m_CurrentBankData.ID)
					End If

					m_SuppressUIEvents = False

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				success = False
			End Try


			Return success

		End Function

		Private Function ValidateInputData() As Boolean
			Dim success As Boolean = True
			Dim meldung As String = String.Empty

			ErrorProvider.ClearErrors()

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen korrekten Wert ein.")

			Dim isValid As Boolean = True

			If mainTab.SelectedTabPage Is xtabDTA Then
				isValid = isValid And SetDXErrorIfInvalid(beDTA_Swift, ErrorProvider, String.IsNullOrWhiteSpace(beDTA_Swift.EditValue), errorText)
				isValid = isValid And SetDXErrorIfInvalid(txtDTA_DTAIBAN, ErrorProvider, String.IsNullOrWhiteSpace(txtDTA_DTAIBAN.EditValue) OrElse (txtDTA_DTAIBAN.EditValue.ToString.Replace(" ", "").Trim.Length < 20), errorText)
				isValid = isValid And SetDXErrorIfInvalid(txtDTA_DTAAdr1, ErrorProvider, String.IsNullOrWhiteSpace(txtDTA_DTAAdr1.EditValue), errorText)
				isValid = isValid And SetDXErrorIfInvalid(txtDTA_DTAAdr2, ErrorProvider, String.IsNullOrWhiteSpace(txtDTA_DTAAdr2.EditValue), errorText)

			Else
				isValid = isValid And SetDXErrorIfInvalid(txtESR_BankAdresse, ErrorProvider, String.IsNullOrWhiteSpace(txtESR_BankAdresse.EditValue), errorText)
				isValid = isValid And SetDXErrorIfInvalid(beESR_Swift, ErrorProvider, String.IsNullOrWhiteSpace(beESR_Swift.EditValue), errorText)
				isValid = isValid And SetDXErrorIfInvalid(txtESR_ESRIBAN1, ErrorProvider, String.IsNullOrWhiteSpace(txtESR_ESRIBAN1.EditValue) OrElse (txtESR_ESRIBAN1.EditValue.ToString.Replace(" ", "").Trim.Length < 20), errorText)

			End If


			Return isValid

		End Function

		Private Sub mainTab_SelectedPageChanged(sender As Object, e As TabPageChangedEventArgs) Handles mainTab.SelectedPageChanged

			If m_SuppressUIEvents Then Return

			LoadMandantBankData()
			If (m_MandantBankData Is Nothing OrElse m_MandantBankData.Count = 0) Then
				Dim msg = "Keine Datensätze wurden gefunden."
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Daten anzeigen"), MessageBoxIcon.Asterisk)

				Return
			End If

			If mainTab.SelectedTabPage Is xtabDTA Then FocusDTArecord(m_MandantBankData(0).ID) Else FocusESRrecord(m_MandantBankData(0).ID)

		End Sub

		Sub OngvDTA_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			m_CurrentBankData = SelectedDTARecord

			If m_MandantBankData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

				Return
			End If

			DisplayDTADetails()

		End Sub

		Sub OngvESR_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

			m_CurrentBankData = SelectedESRRecord

			If m_MandantBankData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

				Return
			End If

			DisplayESRDetails()

		End Sub

		Private Sub OngvDTA_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvDTA.FocusedRowChanged

			If m_SuppressUIEvents Then
				Return
			End If

			m_CurrentBankData = SelectedDTARecord

			If Not m_CurrentBankData Is Nothing Then
				DisplayDTADetails()
			End If

		End Sub

		Private Sub FocusDTArecord(ByVal recID As Integer)

			If Not m_MandantBankData Is Nothing AndAlso m_MandantBankData.Count > 0 Then

				Dim index = m_MandantBankData.ToList().FindIndex(Function(data) data.ID = recID)

				Dim rowHandle = gvDTA.GetRowHandle(index)
				gvDTA.FocusedRowHandle = rowHandle

				gvDTA.MakeRowVisible(rowHandle)
				DisplayDTADetails()

			End If

		End Sub

		Private Sub OngvESR_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvESR.FocusedRowChanged

			If m_SuppressUIEvents Then
				Return
			End If

			m_CurrentBankData = SelectedESRRecord

			If Not m_CurrentBankData Is Nothing Then
				DisplayESRDetails()
			End If

		End Sub

		'Private Sub OngvDTA_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvDTA.CustomUnboundColumnData

		'	If e.Column.Name = "dependentSymbol" Then
		'		If (e.IsGetData()) Then
		'			e.Value = m_StandardImage
		'		End If
		'	End If
		'End Sub

		'Private Sub OngvESR_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvESR.CustomUnboundColumnData

		'	If e.Column.Name = "dependentSymbol" Then
		'		If (e.IsGetData()) Then
		'			If e.Value Then e.Value = m_StandardImage
		'			'e.Value = m_StandardImage
		'		End If
		'		End If
		'End Sub

		Private Sub FocusESRrecord(ByVal recID As Integer)

			If Not m_MandantBankData Is Nothing AndAlso m_MandantBankData.Count > 0 Then

				Dim index = m_MandantBankData.ToList().FindIndex(Function(data) data.ID = recID)

				Dim rowHandle = gvESR.GetRowHandle(index)
				gvESR.FocusedRowHandle = rowHandle

				gvESR.MakeRowVisible(rowHandle)
				DisplayESRDetails()

			End If

		End Sub

		Private Sub DisplayDTADetails()

			m_CurrentBankData = SelectedDTARecord
			If m_CurrentBankData Is Nothing Then Return

			txtDTA_RecBez.EditValue = m_CurrentBankData.RecBez
			txtDTA_MD_ID.EditValue = m_CurrentBankData.MD_ID
			txtDTA_DTAClnr.EditValue = m_CurrentBankData.DTAClnr
			txtDTA_KontoDTA.EditValue = m_CurrentBankData.KontoDTA
			txtDTA_KontoVG.EditValue = m_CurrentBankData.KontoVG
			beDTA_Swift.EditValue = m_CurrentBankData.Swift
			txtDTA_DTAIBAN.EditValue = m_CurrentBankData.DTAIBAN
			txtDTA_VGIBAN.EditValue = m_CurrentBankData.VGIBAN
			txtDTA_DTAAdr1.EditValue = m_CurrentBankData.DTAAdr1
			txtDTA_DTAAdr2.EditValue = m_CurrentBankData.DTAAdr2
			txtDTA_DTAAdr3.EditValue = m_CurrentBankData.DTAAdr3
			txtDTA_DTAAdr4.EditValue = m_CurrentBankData.DTAAdr4

			chkDTA_AsStandard.Checked = m_CurrentBankData.AsStandard

			bsiCreated.Caption = String.Format(" {0:f}, {1}", m_CurrentBankData.CreatedOn, m_CurrentBankData.CreatedFrom)
			If m_CurrentBankData.ChangedOn Is Nothing Then
				bsiChanged.Caption = String.Empty
			Else
				bsiChanged.Caption = String.Format(" {0:f}, {1}", m_CurrentBankData.ChangedOn, m_CurrentBankData.ChangedFrom)
			End If

		End Sub

		Private Sub DisplayESRDetails()

			m_CurrentBankData = SelectedESRRecord
			If m_CurrentBankData Is Nothing Then Return

			txtESR_RecBez.EditValue = m_CurrentBankData.RecBez
			txtESR_MD_ID.EditValue = m_CurrentBankData.MD_ID
			txtESR_KontoESR1.EditValue = m_CurrentBankData.KontoESR1
			txtESR_KontoESR2.EditValue = m_CurrentBankData.KontoESR2
			txtESR_Bankname.EditValue = m_CurrentBankData.BankName
			txtESR_BankClnr.EditValue = m_CurrentBankData.BankClnr
			txtESR_BankAdresse.EditValue = m_CurrentBankData.BankAdresse
			beESR_Swift.EditValue = m_CurrentBankData.Swift
			txtESR_ESRIBAN1.EditValue = m_CurrentBankData.ESRIBAN1
			txtESR_ESRIBAN2.EditValue = m_CurrentBankData.ESRIBAN2
			txtESR_ESRIBAN3.EditValue = m_CurrentBankData.ESRIBAN3

			chkESR_AsStandard.Checked = m_CurrentBankData.AsStandard

			bsiCreated.Caption = String.Format(" {0:f}, {1}", m_CurrentBankData.CreatedOn, m_CurrentBankData.CreatedFrom)
			If m_CurrentBankData.ChangedOn Is Nothing Then
				bsiChanged.Caption = String.Empty
			Else
				bsiChanged.Caption = String.Format(" {0:f}, {1}", m_CurrentBankData.ChangedOn, m_CurrentBankData.ChangedFrom)
			End If

		End Sub

		Private Sub OnbeDTA_Swift_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles beDTA_Swift.ButtonClick
			If String.IsNullOrWhiteSpace(txtDTA_DTAClnr.EditValue) Then Return

			Dim bnkData = ReadBankDataByClearingNumberOverWebService(txtDTA_DTAClnr.EditValue, String.Empty)
			beDTA_Swift.EditValue = bnkData.Swift

		End Sub

		Private Sub OnbeESR_Swift_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles beESR_Swift.ButtonClick
			If String.IsNullOrWhiteSpace(txtESR_BankClnr.EditValue) Then Return

			Dim bnkData = ReadBankDataByClearingNumberOverWebService(txtESR_BankClnr.EditValue, String.Empty)
			beESR_Swift.EditValue = bnkData.Swift
			txtESR_Bankname.EditValue = bnkData.BanName
			txtESR_BankAdresse.EditValue = String.Format("{0} {1}", bnkData.Postcode, bnkData.Location)

		End Sub

		Private Function ReadBankDataByClearingNumberOverWebService(ByVal clearingNumber As String, ByVal location As String) As BankSearchResultDTO
			Dim result As New BankSearchResultDTO

			If String.IsNullOrWhiteSpace(clearingNumber) Then Return result

			Dim webservice As New SPBankUtilWebService.SPBankUtilSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_BankUtilWebServiceUri)

			' Read data over webservice
			Try
				Dim searchResult As BankSearchResultDTO = webservice.GetBankDataByClearingNumber(RemoveLeadingZeros(clearingNumber), location)

				If Not searchResult Is Nothing Then

					result = searchResult

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Bankdaten konnten nicht über Webservice abgefragt werden."))
			End Try


			Return result

		End Function

		Private Sub bbiNew_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiNew.ItemClick
			ResetAllFields()
		End Sub

		Private Sub bbiDelete_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																										m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
				Return
			End If

			Dim result = DeleteMandantBankData()
			If result Then ResetAllFields()

		End Sub

		Private Sub ResetAllFields()

			If mainTab.SelectedTabPage Is xtabDTA Then
				ResetDTAFields()
				Me.txtDTA_RecBez.Focus()

			Else
				ResetESRFields()
				Me.txtESR_RecBez.Focus()

			End If
			m_CurrentBankData = Nothing

			'Dim cControl As Control = Nothing
			'ResetAllTabEntries()

			'Me.chkESR_AsStandard.Checked = False

		End Sub


		Private Function LoadMandantBankData() As Boolean
			Dim result As Boolean = True

			m_SuppressUIEvents = True
			Dim modulArt As New BankModulEnum

			m_MandantBankData = Nothing
			If mainTab.SelectedTabPage Is xtabDTA Then
				modulArt = BankModulEnum.DTADATA
			Else
				modulArt = BankModulEnum.ESRDATA
			End If

			m_MandantBankData = m_TableSettingDatabaseAccess.LoadMandantBankData(lueMandant.EditValue, modulArt)
			Dim listDataSource As BindingList(Of MDBankData) = New BindingList(Of MDBankData)

			' bank data to view data.
			For Each p In m_MandantBankData

					Dim cViewData = New MDBankData() With {
					.AsStandard = p.AsStandard,
					.BankAdresse = p.BankAdresse,
					.BankClnr = p.BankClnr,
					.BankName = p.BankName,
					.ChangedFrom = p.ChangedFrom,
					.ChangedOn = p.ChangedOn,
					.CreatedFrom = p.CreatedFrom,
					.CreatedOn = p.CreatedOn,
					.DTAAdr1 = p.DTAAdr1,
					.DTAAdr2 = p.DTAAdr2,
					.DTAAdr3 = p.DTAAdr3,
					.DTAAdr4 = p.DTAAdr4,
					.DTAClnr = p.DTAClnr,
					.DTAIBAN = p.DTAIBAN,
					.ESRIBAN1 = p.ESRIBAN1,
					.ESRIBAN2 = p.ESRIBAN2,
					.ESRIBAN3 = p.ESRIBAN3,
					.ID = p.ID,
					.Jahr = p.Jahr,
					.KontoDTA = p.KontoDTA,
					.KontoESR1 = p.KontoESR1,
					.KontoESR2 = p.KontoESR2,
					.KontoVG = p.KontoVG,
					.MDNr = p.MDNr,
					.MD_ID = p.MD_ID,
					.ModulArt = p.ModulArt,
					.RecBez = p.RecBez,
					.RecNr = p.RecNr,
					.Swift = p.Swift,
					.USNr = p.USNr,
					.VGIBAN = p.VGIBAN
					}


					listDataSource.Add(cViewData)
				Next

			m_MandantBankData = listDataSource


			If mainTab.SelectedTabPage Is xtabDTA Then
				grdDTA.DataSource = m_MandantBankData

			Else
				grdESR.DataSource = m_MandantBankData

			End If
			m_SuppressUIEvents = False

			Return Not (m_MandantBankData Is Nothing)

		End Function

		Private Function DeleteMandantBankData() As Boolean
			Dim result = DeleteResult.Deleted

			If m_CurrentBankData Is Nothing Then Return False
			If mainTab.SelectedTabPage Is xtabDTA Then
				Dim dtadata = SelectedDTARecord
				result = m_TableSettingDatabaseAccess.DeleteAssignedMandantBankData(dtadata.ID, m_InitializationData.UserData.UserNr)
			Else
				Dim esrData = SelectedESRRecord
				result = m_TableSettingDatabaseAccess.DeleteAssignedMandantBankData(esrData.ID, m_InitializationData.UserData.UserNr)
			End If

			Dim msg As String = String.Empty
			Select Case result
				Case DeleteResult.Deleted
					LoadMandantBankData()
					If m_MandantBankData Is Nothing OrElse m_MandantBankData.Count = 0 Then Return True
					If mainTab.SelectedTabPage Is xtabDTA Then FocusDTArecord(m_MandantBankData(0).ID) Else FocusESRrecord(m_MandantBankData(0).ID)

					msg = "Der ausgewählte Datensatz wurde erfolgreich gelöscht."
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(msg),
																	 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Information)
					Return False

				Case Else
					msg = "Die Daten konnten nicht gelöscht werden."
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

					Return False
			End Select

		End Function


#Region "helpers"

		Protected Function SetDXErrorIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

			If (invalid) Then
				errorProvider.SetError(control, errorText, ErrorType.Critical)
			Else
				errorProvider.SetError(control, String.Empty)
			End If

			Return Not invalid

		End Function

		Protected Function SetDXWarningIfInvalid(ByVal control As Control, ByVal errorProvider As DXErrorProvider, ByVal invalid As Boolean, ByVal errorText As String) As Boolean

			If (invalid) Then
				errorProvider.SetError(control, errorText, ErrorType.Warning)
			End If

			Return Not invalid

		End Function

		Private Function RemoveLeadingZeros(ByVal str As String) As String

			If String.IsNullOrWhiteSpace(str) Then
				Return String.Empty
			Else
				Return str.Trim().TrimStart("0")
			End If

		End Function


#End Region


	End Class


End Namespace
