
Imports SP.DatabaseAccess.Report.DataObjects
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.Customer
Imports System.ComponentModel
Imports SP.KD.ReAdresse.UI
Imports System.Globalization
Imports SP.DatabaseAccess.Report
Imports SPS.SalaryValueCalculation.RPLValueCalculation
Imports SPS.SalaryValueCalculation.RPLValueCalculation.RPLSalaryTypeValuesCalcParams
Imports SPS.SalaryValueCalculation.RPLValueCalculation.RPLAdditionalFeesValuesCalcParams
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.Infrastructure.DateAndTimeCalculation
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.ES
Imports System.IO
Imports SPProgUtility.CommonXmlUtility
Imports DevExpress.XtraGrid.Views.Grid
Imports SPProgUtility.SPUserSec.ClsUserSec

Namespace UI

	Public Class ucMainContent

#Region "Private Consts"

		Public Const Anzahl_Default_Value As Decimal = 0.0
		Public Const Basis_Default_Value As Decimal = 0.0
		Public Const Ansatz_Default_Value As Decimal = 0.0
		Public Const Betrag_Default_Value As Decimal = 0.0
		Public Const MwSt_Default_Value As Decimal = 0.0

		Public Const LANr_Feiertag As Decimal = 500D
		Public Const LANR_Ferien As Decimal = 600D
		Public Const LANr_Lohn13 As Decimal = 700D

		Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

		Private MODUL_NAME_SETTING As String = "Report"
		Private Const USER_XML_SETTING_SPUTNIK_RP_RPLEMPLOYEE_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/RP/rplemployee/restorelayoutfromxml"
		Private Const USER_XML_SETTING_SPUTNIK_RP_RPLEMPLOYEE_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/RP/rplemployee/keepfilter"

		Private Const USER_XML_SETTING_SPUTNIK_RP_RPLCUSTOMER_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/RP/rplcustomer/restorelayoutfromxml"
		Private Const USER_XML_SETTING_SPUTNIK_RP_RPLCUSTOMER_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/RP/rplcustomer/keepfilter"

#End Region


#Region "Private Fields"

		''' <summary>
		''' The customer database access.
		''' </summary>
		Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

		''' <summary>
		''' The data access object.
		''' </summary>
		Private m_EmployeeDataAccess As IEmployeeDatabaseAccess

		''' <summary>
		''' The data access object.
		''' </summary>
		Protected m_ESDataAccess As IESDatabaseAccess

		''' <summary>
		''' LA List.
		''' </summary>
		Private m_LAList As List(Of LAData)

		''' <summary>
		''' Additional employee ES salary type data.
		''' </summary>
		Private m_AdditionalEmployeeESSalaryTypeData As List(Of DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData)

		''' <summary>
		''' Additional customer ES salary type data.
		''' </summary>
		Private m_AdditionalCustomerESSalaryTypeData As List(Of DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData)

		''' <summary>
		''' The customer KST list.
		''' </summary>
		Private m_CustomerKSTList As BindingList(Of KSTViewData)

		''' <summary>
		''' The active rpl type.
		''' </summary>
		''' <remarks>Determines the type of the active RPL, mainly used for new RPL.</remarks>
		Private m_ActiveRPLType As RPLType

		''' <summary>
		''' The selected RPL list data.
		''' </summary>
		''' <remarks>Could be employee or customer data.</remarks>
		Private m_SelectedRPLListData As RPLListData

		''' <summary>
		''' The RPL value calculator.
		''' </summary>
		Private m_RPLValueCalculator As RPLValueCalculator

		''' <summary>
		''' The create RPL service.
		''' </summary>
		Private m_RPLCreateService As CreateRPLService

		''' <summary>
		''' Time table data to save.
		''' </summary>
		Private m_TimeTableDataToSave As TimeTable

		''' <summary>
		''' The date and time utility.
		''' </summary>
		Private m_DateUtility As DateAndTimeUtily

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As Utility

		''' <summary>
		''' Check edit for existing RE.
		''' </summary>
		Private m_CheckEditReExisting As RepositoryItemCheckEdit

		''' <summary>
		''' The pdf icon.
		''' </summary>
		Private m_PDFIcon As Image

		''' <summary>
		''' The empty icon.
		''' </summary>
		Private m_EmptyIcon As Image

		''' <summary>
		'''  Current LA values upper bounds.
		''' </summary>
		Private m_CurrentLAValueUpperBounds As LAValueUpperBounds

		Private m_InvoiceAddressesForm As frmInvoiceAddress

		''' <summary>
		''' The cls prog path.
		''' </summary>
		Private m_path As ClsProgPath

		''' <summary>
		''' The mandant.
		''' </summary>
		Private m_Mandant As Mandant

		Private m_GVRPLEmployeeSettingfilename As String
		Private m_GVRPLCustomerSettingfilename As String
		Private Property GridSettingPath As String

		Private UserGridSettingsXml As SettingsXml

		Private m_xmlSettingRPLEmployeeFilter As String
		Private m_xmlSettingRestoreRPLEmployeeSetting As String

		Private m_xmlSettingRPLCustomerFilter As String
		Private m_xmlSettingRestoreRPLCustomerSetting As String

		' print label by zero hour?
		Private m_PrintNoLabelbyzeroHour As Boolean?
		Private m_EmployeeWithWeeklyPayment As Boolean
		Private m_printDatamatrixLabel As Boolean
		Private m_DatamatrixprinterName As String
		Private m_UserAllowedToChanged As Boolean


#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			' Dieser Aufruf ist für den Designer erforderlich.

			Try
				m_Mandant = New Mandant
				m_path = New ClsProgPath
			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
			End Try

			Dim previousState = SetSuppressUIEventsState(True)
			InitializeComponent()
			SetSuppressUIEventsState(previousState)

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			'm_RPLValueCalculator = New RPLValueCalculator()
			m_DateUtility = New DateAndTimeUtily
			m_Utility = New Utility


			' Important symbol.
			m_CheckEditReExisting = CType(grdCustomerRPL.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
			m_CheckEditReExisting.PictureChecked = My.Resources.Checked
			m_CheckEditReExisting.PictureUnchecked = Nothing
			m_CheckEditReExisting.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

			m_PDFIcon = My.Resources.pdf
			m_EmptyIcon = My.Resources.Empty
			m_CurrentLAValueUpperBounds = New LAValueUpperBounds
			m_printDatamatrixLabel = False

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			AddHandler gvEmployeeRPL.ColumnPositionChanged, AddressOf OngvRPLEmployeeColumnPositionChanged
			AddHandler gvEmployeeRPL.ColumnWidthChanged, AddressOf OngvRPLEmployeeColumnPositionChanged

			AddHandler gvCustomerRPL.ColumnPositionChanged, AddressOf OngvRPLCustomerColumnPositionChanged
			AddHandler gvCustomerRPL.ColumnWidthChanged, AddressOf OngvRPLCustomerColumnPositionChanged

			AddHandler lueCustomerKST.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueWeek.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditFromDate.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditToDate.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueLAData.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueAdditionalInfo.ButtonClick, AddressOf OnDropDown_ButtonClick

			AddHandler lueCustomerKST.KeyPress, AddressOf OnControl_KeyPress
			AddHandler lueWeek.KeyPress, AddressOf OnControl_KeyPress
			AddHandler dateEditFromDate.KeyPress, AddressOf OnControl_KeyPress
			AddHandler dateEditToDate.KeyPress, AddressOf OnControl_KeyPress
			AddHandler lueLAData.KeyPress, AddressOf OnControl_KeyPress

			AddHandler lueAdditionalInfo.KeyPress, AddressOf OnControl_KeyPress
			AddHandler txtAnzahl.KeyPress, AddressOf OnControl_KeyPress
			AddHandler txtBasis.KeyPress, AddressOf OnControl_KeyPress
			AddHandler txtAnsatz.KeyPress, AddressOf OnControl_KeyPress

			AddHandler txtAnzahl.KeyUp, AddressOf OnControl_KeyUp2Save
			AddHandler txtBasis.KeyUp, AddressOf OnControl_KeyUp2Save
			AddHandler txtAnsatz.KeyUp, AddressOf OnControl_KeyUp2Save

		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the selected employee RPL data.
		''' </summary>
		''' <returns>The selected employee RPL or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedEmployeeRPLData As RPLListData
			Get
				Dim grdView = TryCast(grdEmployeeRPL.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim rplDData = CType(grdView.GetRow(selectedRows(0)), RPLListData)
						Return rplDData
					End If

				End If

				Return Nothing
			End Get

		End Property

		''' <summary>
		''' Gets the selected customer RPL data.
		''' </summary>
		''' <returns>The selected customer RPL or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedCustomerRPLData As RPLListData
			Get
				Dim grdView = TryCast(grdCustomerRPL.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim rplDData = CType(grdView.GetRow(selectedRows(0)), RPLListData)
						Return rplDData
					End If

				End If

				Return Nothing
			End Get

		End Property




		''' <summary>
		''' Gets the selected ESMALA data.
		''' </summary>
		''' <returns>The selected ESMALA data or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedESMALAData As DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData
			Get
				Dim grdView = TryCast(grdEmployeeSalaryTypeData.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim esLAData = CType(grdView.GetRow(selectedRows(0)), DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData)
						Return esLAData
					End If

				End If

				Return Nothing
			End Get

		End Property

		''' <summary>
		''' Gets the selected ESKDLA data.
		''' </summary>
		''' <returns>The selected ESKDLA data or nothing if none is selected.</returns>
		Public ReadOnly Property SelectedESKDLAData As DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData
			Get
				Dim grdView = TryCast(grdCustomerSalaryTypeData.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim esLAData = CType(grdView.GetRow(selectedRows(0)), DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData)
						Return esLAData
					End If

				End If

				Return Nothing
			End Get

		End Property




		''' <summary>
		''' Gets the first employee RPL data in list.
		''' </summary>
		''' <returns>First employee RPL in list or nothing.</returns>
		Public ReadOnly Property FirstEmployeeRPLDataInList As RPLListData
			Get
				If gvEmployeeRPL.RowCount > 0 Then

					Dim rowHandle = gvEmployeeRPL.GetVisibleRowHandle(0)
					Return CType(gvEmployeeRPL.GetRow(rowHandle), RPLListData)
				Else
					Return Nothing
				End If

			End Get
		End Property

		''' <summary>
		''' Gets the first customer RPL data in list.
		''' </summary>
		''' <returns>First customer RPL data in list or nothing.</returns>
		Public ReadOnly Property FirstCustomerRPLDataInList As RPLListData
			Get
				If gvCustomerRPL.RowCount > 0 Then

					Dim rowHandle = gvCustomerRPL.GetVisibleRowHandle(0)
					Return CType(gvCustomerRPL.GetRow(rowHandle), RPLListData)
				Else
					Return Nothing
				End If

			End Get
		End Property

		''' <summary>
		''' Gets the selected customer KST.
		''' </summary>
		''' <returns>The selected customer KST or nothing.</returns>
		Public ReadOnly Property SelectedCustomerKST As KSTViewData
			Get

				If lueCustomerKST.EditValue Is Nothing Or m_CustomerKSTList Is Nothing Then
					Return Nothing
				End If

				Dim kstData = m_CustomerKSTList.Where(Function(data) data.RecordNumber = lueCustomerKST.EditValue).FirstOrDefault()
				Return kstData
			End Get
		End Property


		''' <summary>
		''' Gets the selected LA.
		''' </summary>
		''' <returns>The selected LA or nothing.</returns>
		Public ReadOnly Property SelectedLA As LAData
			Get

				If lueLAData.EditValue Is Nothing Or m_LAList Is Nothing Then
					Return Nothing
				End If

				Dim laData = m_LAList.Where(Function(data) data.LANr = lueLAData.EditValue).FirstOrDefault()
				Return laData
			End Get
		End Property


		''' <summary>
		''' Gets the BasisFactor.
		''' </summary>
		Public ReadOnly Property BasisFactor As Decimal
			Get

				Dim laData = SelectedLA
				Dim factor As Decimal = 1.0

				If Not laData Is Nothing Then
					factor = If(laData.Sign.Equals("-"), -1.0, 1.0)
				End If

				Return factor

			End Get
		End Property

		''' <summary>
		''' Gets or set sets the Anzahl.
		''' </summary>
		Public Property AnzahlValueInUI As Decimal
			Get
				Return txtAnzahl.EditValue
			End Get

			Set(value As Decimal)
				txtAnzahl.EditValue = value
			End Set
		End Property

		''' <summary>
		''' Gets or set sets the Basis.
		''' </summary>
		Public Property BasisValueInUI As Decimal
			Get
				Return txtBasis.EditValue
			End Get

			Set(value As Decimal)
				txtBasis.EditValue = value
			End Set
		End Property

		''' <summary>
		''' Gets the signed basis.
		''' </summary>
		Public ReadOnly Property SignedBasisValueInUI As Decimal
			Get
				Return BasisValueInUI * BasisFactor
			End Get
		End Property

		''' <summary>
		''' Gets or set sets the Ansatz.
		''' </summary>
		Public Property AnsatzValueInUI As Decimal
			Get
				Return txtAnsatz.EditValue
			End Get

			Set(value As Decimal)
				txtAnsatz.Text = value
			End Set
		End Property

		''' <summary>
		''' Gets or set sets the Betrag.
		''' </summary>
		Public Property BetragValueInUI As Decimal
			Get
				Return txtBetrag.EditValue
			End Get

			Set(value As Decimal)

				Dim la = SelectedLA
				Dim rounding As Short = 2

				If Not la Is Nothing AndAlso la.Rounding.HasValue Then
					rounding = la.Rounding
					txtBetrag.Properties.Mask.EditMask = "n" + rounding.ToString()
				Else
					txtBetrag.Properties.Mask.EditMask = "n2"

				End If
				'txtBetrag.EditValue = Math.Round(value * 2, 1) / 2
				txtBetrag.EditValue = value

			End Set
		End Property


		''' <summary>
		''' Get Betrag rounded in CH 5 Rappen
		''' </summary>
		Public ReadOnly Property BetragValueInUIRoundedTo5Rappen As Decimal
			Get

				Dim la = SelectedLA
				Dim rounding As Short = 2

				If Not la Is Nothing AndAlso la.Rounding.HasValue Then
					rounding = la.Rounding
				End If

				Dim roundedValue = 0D
				If rounding <= 2 Then
					roundedValue = Math.Round(txtBetrag.EditValue * 2, 1) / 2
				Else
					roundedValue = (txtBetrag.EditValue)
				End If

				Return roundedValue
			End Get

		End Property

		''' <summary>
		''' Gets or set sets the MwST.
		''' </summary>
		Public Property MwStInUI As Decimal
			Get
				Return txtMwSt.EditValue
			End Get

			Set(value As Decimal)
				txtMwSt.Text = value
			End Set
		End Property

#Region "AdditionalFees and FlexibleTime Values"

		''' <summary>
		''' Gets the Feiertag Basis in UI
		''' </summary>
		Public ReadOnly Property FeiertagBasisInUI As Decimal
			Get
				Dim row = GetAdditonalFeeRowByLANr(LANr_Feiertag)
				Return If(Not row Is Nothing AndAlso row.M_Basis.HasValue, row.M_Basis, 0D)
			End Get
		End Property

		''' <summary>
		''' Gets the Ferien Basis in UI
		''' </summary>
		Public ReadOnly Property FerienBasisInUI As Decimal
			Get
				Dim row = GetAdditonalFeeRowByLANr(LANR_Ferien)
				Return If(Not row Is Nothing AndAlso row.M_Basis.HasValue, row.M_Basis, 0D)
			End Get
		End Property

		''' <summary>
		''' Gets the Lohn13 Basis in UI
		''' </summary>
		Public ReadOnly Property Lohn13BasisInUI As Decimal
			Get
				Dim row = GetAdditonalFeeRowByLANr(LANr_Lohn13)
				Return If(Not row Is Nothing AndAlso row.M_Basis.HasValue, row.M_Basis, 0D)
			End Get
		End Property

		''' <summary>
		''' Gets the Feiertag Ansatz in UI
		''' </summary>
		Public ReadOnly Property FeiertagAnsatzInUI As Decimal
			Get
				Dim row = GetAdditonalFeeRowByLANr(LANr_Feiertag)
				Return If(Not row Is Nothing AndAlso row.M_Ansatz.HasValue, row.M_Ansatz, 0D)
			End Get
		End Property

		''' <summary>
		''' Gets the Ferien Ansatz in UI
		''' </summary>
		Public ReadOnly Property FerienAnsatzInUI As Decimal
			Get
				Dim row = GetAdditonalFeeRowByLANr(LANR_Ferien)
				Return If(Not row Is Nothing AndAlso row.M_Ansatz.HasValue, row.M_Ansatz, 0D)
			End Get
		End Property

		''' <summary>
		''' Gets the Lohn13 Ansatz in UI
		''' </summary>
		Public ReadOnly Property Lohn13AnsatzInUI As Decimal
			Get
				Dim row = GetAdditonalFeeRowByLANr(LANr_Lohn13)
				Return If(Not row Is Nothing AndAlso row.M_Ansatz.HasValue, row.M_Ansatz, 0D)
			End Get
		End Property

		''' <summary>
		''' Gets the Feiertag Betrag in UI
		''' </summary>
		Public ReadOnly Property FeiertagBetragInUI As Decimal
			Get
				Dim row = GetAdditonalFeeRowByLANr(LANr_Feiertag)
				Return If(Not row Is Nothing AndAlso row.M_Betrag.HasValue, row.M_Betrag, 0D)
			End Get
		End Property

		''' <summary>
		''' Gets the Ferien Betrag in UI
		''' </summary>
		Public ReadOnly Property FerienBetragInUI As Decimal
			Get
				Dim row = GetAdditonalFeeRowByLANr(LANR_Ferien)
				Return If(Not row Is Nothing AndAlso row.M_Betrag.HasValue, row.M_Betrag, 0D)
			End Get
		End Property

		''' <summary>
		''' Gets the Lohn13 Betrag in UI
		''' </summary>
		Public ReadOnly Property Lohn13BetragInUI As Decimal
			Get
				Dim row = GetAdditonalFeeRowByLANr(LANr_Lohn13)
				Return If(Not row Is Nothing AndAlso row.M_Betrag.HasValue, row.M_Betrag, 0D)
			End Get
		End Property

		''' <summary>
		''' Gets the FlexibleTime Hours in UI
		''' </summary>
		Public ReadOnly Property FlexibleTimeHoursInUI As Decimal
			Get
				Dim row = GetFlexibleTimeRow()
				Return If(Not row Is Nothing AndAlso row.KompStd.HasValue, row.KompStd, 0D)
			End Get
		End Property

		''' <summary>
		''' Gets the FlexibleTime Betrag in UI
		''' </summary>
		Public ReadOnly Property FlexibleTimeBetragInUI As Decimal
			Get
				Dim row = GetFlexibleTimeRow()
				Return If(Not row Is Nothing AndAlso row.KompBetrag.HasValue, row.KompBetrag, 0D)
			End Get
		End Property

#End Region

#End Region

#Region "Private Properties"

		''' <summary>
		''' Gets boolean flag indicating if the selected RPL can be changed.
		''' </summary>
		''' <returns>Boolean flag indicating if employee RPL can be deleted.</returns>
		Private ReadOnly Property CanSelectedRPLBeChanged As Boolean
			Get

				If m_SelectedRPLListData Is Nothing Then
					Return False
				End If

				If m_UCMediator.ActiveReportData Is Nothing OrElse
							m_UCMediator.ActiveReportData.ReportData.IsMonthClosed Then
					Return False
				End If

				Select Case m_SelectedRPLListData.Type
					Case RPLType.Employee

						If m_UCMediator.ActiveReportData.ReportData.LONr > 0 Then
							Return False
						End If
					Case RPLType.Customer

						If m_SelectedRPLListData.RENr > 0 Then
							Return False
						End If

					Case Else
						Return False
				End Select

				Return True

			End Get
		End Property

		''' <summary>
		''' Get boolean flag indicating if flexible time is active.
		''' </summary>
		''' <returns>Boolean flag indicating if flexible time is active.</returns>
		Private ReadOnly Property IsFlexibleTimeActive As Boolean

			Get
				Dim la = SelectedLA
				If Not la.GleitTime.HasValue OrElse Not la.GleitTime Then Return False

				Dim isFlexibleTimeAllowedXmLSetting As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/allowedflextimeinreports", FORM_XML_MAIN_KEY)), False)

				If isFlexibleTimeAllowedXmLSetting.HasValue AndAlso isFlexibleTimeAllowedXmLSetting.Value Then
					Return True
				End If

				' In the xml the option is false. 
				Dim rpData = m_UCMediator.ActiveReportData

				Dim employeeLOSetting = m_EmployeeDataAccess.LoadEmployeeLOSettings(rpData.EmployeeOfActiveReport.EmployeeNumber)

				If Not employeeLOSetting Is Nothing Then
					Return (employeeLOSetting.MAGleitzeit.HasValue AndAlso employeeLOSetting.MAGleitzeit = True)
				End If

				Return False

			End Get
		End Property

		Private ReadOnly Property EmployeeWithWeeklyPayment As Boolean

			Get
				'Dim la = SelectedLA
				'If Not la.LANr <> 100 Then Return False

				Dim rpData = m_UCMediator.ActiveReportData

				Dim employeeLOSetting = m_EmployeeDataAccess.LoadEmployeeLOSettings(rpData.EmployeeOfActiveReport.EmployeeNumber)

				If Not employeeLOSetting Is Nothing Then
					Return (employeeLOSetting.WeeklyPayment.GetValueOrDefault(False))
				End If

				Return False

			End Get
		End Property

		''' <summary>
		''' Gets the default MwStValue.
		''' </summary>
		''' <returns>The default MwSt value.</returns>
		Private ReadOnly Property DefaultMwStValue(ByVal mdYear As Integer) As Decimal
			Get

				Dim mdNumber = m_InitializationData.MDData.MDNr

				If mdYear = 0 Then mdYear = Now.Year
				Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
				Dim mwstsatz As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDDataXMLFilename(mdNumber, mdYear), String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY))

				If String.IsNullOrWhiteSpace(mwstsatz) Then
					Return 8
				End If

				Return Convert.ToDecimal(mwstsatz)

				'Dim strQuery As String = "//Debitoren/mwstsatz"
				'Dim r = m_ClsProgSetting.GetUserProfileFile
				'Dim defaultMwSt As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "8")

				'Return Convert.ToDecimal(defaultMwSt)
			End Get
		End Property

		Private ReadOnly Property SetAdditionalInfoAsCustomerKST As Boolean
			Get

				Dim copyksttorreportline As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/copykstintoreportline", FORM_XML_MAIN_KEY)), False)
				Return copyksttorreportline

			End Get
		End Property

		''' <summary>
		''' get datamatrix printername from userprofile
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property GetUserDataMaxtrixPrintername() As String
			Get
				Dim sp_utility As New SPProgUtility.MainUtilities.Utilities

				Dim strQuery As String = "//Report/matrixprintername"
				Dim strStyleName As String = sp_utility.GetXMLValueByQueryWithFilename(m_Mandant.GetSelectedMDUserProfileXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr), strQuery, Nothing)

				Return strStyleName

			End Get
		End Property

		''' <summary>
		''' get datamatrix code from mandant
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property GetDataMaxtrixCodeString() As String
			Get
				Dim strQuery As String = String.Format("{0}/datamatrixcodestringforlabel", FORM_XML_MAIN_KEY)
				Dim dataMatrixCode As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), strQuery)
				If String.IsNullOrWhiteSpace(dataMatrixCode) Then dataMatrixCode = "{0}_{1}_{2}_{3}_{4:dd}_{5:dd}"

				Return dataMatrixCode

			End Get
		End Property

		Private ReadOnly Property PrintNoLabelByZeroHour As Boolean?
			Get

				Dim FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"
				Dim value As String = m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr), String.Format("{0}/byzerohourdonotprintlabel", FORM_XML_DEFAULTVALUES_KEY))

				Return If(String.IsNullOrWhiteSpace(value), Nothing, CBool(value))

			End Get
		End Property


		''' <summary>
		''' Boolean flag indicating if hourly data check should be ignored throuhg validation.
		''' </summary>
		Private Property IgnoreHourlyDataCheckInValidation As Boolean

#End Region

#Region "Public Methods"

		''' <summary>
		''' Inits the control with configuration information.
		''' </summary>
		'''<param name="initializationClass">The initialization class.</param>
		'''<param name="translationHelper">The translation helper.</param>
		Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
			MyBase.InitWithConfigurationData(initializationClass, translationHelper)

			m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_EmployeeDataAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
			m_ESDataAccess = New DatabaseAccess.ES.ESDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

			m_RPLCreateService = New CreateRPLService(m_InitializationData.MDData.MDNr, m_InitializationData)

			UserGridSettingsXml = New SettingsXml(m_Mandant.GetAllUserGridSettingXMLFilename(m_InitializationData.MDData.MDNr))
			m_DatamatrixprinterName = GetUserDataMaxtrixPrintername()

			m_UserAllowedToChanged = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 301, m_InitializationData.MDData.MDNr) AndAlso IsUserActionAllowed(m_InitializationData.UserData.UserNr, 302, m_InitializationData.MDData.MDNr)
			m_RPLValueCalculator = New RPLValueCalculator(m_InitializationData)

		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			Dim previousState = SetSuppressUIEventsState(True)

			Try
				Dim mSettingpath = String.Format("{0}Report\", m_Mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr))
				If Not Directory.Exists(mSettingpath) Then Directory.CreateDirectory(mSettingpath)

				m_GVRPLEmployeeSettingfilename = String.Format("{0}{1}{2}.xml", mSettingpath, gvEmployeeRPL.Name, m_InitializationData.UserData.UserNr)
				m_GVRPLCustomerSettingfilename = String.Format("{0}{1}{2}.xml", mSettingpath, gvCustomerRPL.Name, m_InitializationData.UserData.UserNr)

				m_xmlSettingRestoreRPLEmployeeSetting = String.Format(USER_XML_SETTING_SPUTNIK_RP_RPLEMPLOYEE_GRIDSETTING_RESTORE, m_InitializationData.UserData.UserNr)
				m_xmlSettingRPLEmployeeFilter = String.Format(USER_XML_SETTING_SPUTNIK_RP_RPLEMPLOYEE_GRIDSETTING_FILTER, m_InitializationData.UserData.UserNr)

				m_xmlSettingRestoreRPLCustomerSetting = String.Format(USER_XML_SETTING_SPUTNIK_RP_RPLCUSTOMER_GRIDSETTING_RESTORE, m_InitializationData.UserData.UserNr)
				m_xmlSettingRPLCustomerFilter = String.Format(USER_XML_SETTING_SPUTNIK_RP_RPLCUSTOMER_GRIDSETTING_FILTER, m_InitializationData.UserData.UserNr)

			Catch ex As Exception

			End Try


			m_LAList = Nothing
			m_AdditionalEmployeeESSalaryTypeData = Nothing
			m_AdditionalCustomerESSalaryTypeData = Nothing

			m_CustomerKSTList = Nothing

			m_SelectedRPLListData = Nothing
			m_ActiveRPLType = RPLType.Employee
			m_TimeTableDataToSave = Nothing

			lueCustomerKST.EditValue = Nothing
			lueWeek.EditValue = Nothing
			dateEditFromDate.EditValue = Nothing
			dateEditFromDate.Properties.MinValue = DateTime.MinValue
			dateEditFromDate.Properties.MaxValue = DateTime.MaxValue
			dateEditFromDate.Properties.ReadOnly = False
			dateEditToDate.EditValue = Nothing
			dateEditToDate.Properties.MinValue = DateTime.MinValue
			dateEditToDate.Properties.MaxValue = DateTime.MaxValue
			dateEditToDate.Properties.ReadOnly = False

			lblNegSign.Visible = False

			AnzahlValueInUI = Anzahl_Default_Value
			txtAnzahl.Properties.MaxLength = 16
			txtAnzahl.Properties.ReadOnly = False

			BasisValueInUI = Basis_Default_Value
			txtBasis.Properties.MaxLength = 16
			txtBasis.Properties.ReadOnly = False

			AnsatzValueInUI = Ansatz_Default_Value
			txtAnsatz.Properties.MaxLength = 16
			txtAnsatz.Properties.ReadOnly = False

			BetragValueInUI = Betrag_Default_Value
			txtBetrag.Properties.MaxLength = 16
			txtBetrag.Properties.ReadOnly = True

			m_CurrentLAValueUpperBounds.Reset()

			MwStInUI = MwSt_Default_Value
			txtMwSt.Properties.MaxLength = 16
			txtMwSt.Properties.ReadOnly = False

			btnOpenHourInput.Visible = False

			IgnoreHourlyDataCheckInValidation = False

			' ---Reset drop downs, grids and lists---

			ResetCustomerKSTDropDown()
			ResetWeekDropDown()
			ResetLADropDown()
			ResetAdditionalInfoDropDown()

			ResetEmployeeRPLGrid()
			ResetEmployeeSalaryTypesGrid()

			ResetCustomerRPLGrid()
			ResetCustomerSalaryTypesGrid()

			ResetRPLAdditionalFeeGrid()
			ResetRPLFlexibleTimeGrid()

			SetFocusedAppearanceOnEmployeeRPLGrid(True)
			SetFocusedAppearanceOnCustomerRPLGrid(False)

			errorProvider.Clear()

			SetSuppressUIEventsState(previousState)
		End Sub

		''' <summary>
		''' Loads data of the active report.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Public Overrides Function LoadDataOfActiveReport() As Boolean

			Dim success As Boolean = True

			Dim previousState = SetSuppressUIEventsState(True)

			Dim activeReportData = m_UCMediator.ActiveReportData

			SetMinAndMaxDateToReportRange()

			success = success AndAlso LoadCustomerKSTDropdowndata(activeReportData.CustomerOfActiveReport.CustomerNumber)
			success = success AndAlso LoadAdditionalInfoDropdownData(activeReportData.CustomerOfActiveReport.CustomerNumber)
			success = success AndAlso LoadWeekData()

			success = success AndAlso LoadEmployeeAndCustomerEmployeeRPLList()
			success = success AndAlso LoadFirstEmployeeRPLIntoUI()
			m_EmployeeWithWeeklyPayment = EmployeeWithWeeklyPayment


			SetSuppressUIEventsState(False)

			Return True
		End Function

		''' <summary>
		''' Loads the RPL lists.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadEmployeeAndCustomerEmployeeRPLList()

			Dim success As Boolean = True

			success = success AndAlso LoadEmployeeRPList()
			success = success AndAlso LoadEmployeeAdditionalESSalaryList()

			success = success AndAlso LoadCustomerRPList()
			success = success AndAlso LoadCustomerAdditionalESSalaryList()

			LoadEmployeeAndCustomerESSalaryList()

			Return success
		End Function

		''' <summary>
		''' Loads the ESAdditionalSalaryTypes lists.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function LoadEmployeeAndCustomerESSalaryList() As Boolean

			Dim success As Boolean = True
			Dim value As Boolean = False

			success = success AndAlso LoadEmployeeAdditionalESSalaryList()
			value = gvEmployeeSalaryTypeData.RowCount > 0
			grdEmployeeSalaryTypeData.Visible = value
			If Not value Then
				grdEmployeeRPL.Dock = DockStyle.Fill
			Else
				grdEmployeeRPL.Dock = DockStyle.Top
			End If

			success = success AndAlso LoadCustomerAdditionalESSalaryList()
			value = gvCustomerSalaryTypeData.RowCount > 0
			grdCustomerSalaryTypeData.Visible = value
			If Not value Then
				grdCustomerRPL.Dock = DockStyle.Fill
			Else
				grdCustomerRPL.Dock = DockStyle.Top
			End If

			Return success
		End Function

		''' <summary>
		''' Load the first employee RPL into UI.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Public Function LoadFirstEmployeeRPLIntoUI() As Boolean

			Dim success As Boolean = True

			Dim firstEmployeeRPL = FirstEmployeeRPLDataInList

			If Not firstEmployeeRPL Is Nothing Then
				FocusEmployeeRPLData(firstEmployeeRPL.RPLNr)
				success = success AndAlso LoadRPLToUI(firstEmployeeRPL)
				m_UCMediator.LoadDayTotalDataOfRPLType(DatabaseAccess.Report.RPLType.Employee)
			Else
				lueCustomerKST.EditValue = Nothing
				success = success AndAlso PrepareForNewRPL(RPLType.Employee)
			End If

			Return success
		End Function

		''' <summary>
		''' Load the first customer RPL into UI.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadFirstCustomerRPLIntoUI() As Boolean

			Dim success As Boolean = True

			Dim firstCustomerRPL = FirstCustomerRPLDataInList

			If Not firstCustomerRPL Is Nothing Then
				FocusCustomerRPLData(firstCustomerRPL.RPLNr)
				success = success AndAlso LoadRPLToUI(firstCustomerRPL)
				m_UCMediator.LoadDayTotalDataOfRPLType(DatabaseAccess.Report.RPLType.Customer)
			Else
				success = success AndAlso PrepareForNewRPL(RPLType.Customer)
			End If

			Return success
		End Function

		''' <summary>
		''' Handles change of ES salary data selection.
		''' </summary>
		Public Sub HandleESSalaryDataSelectionChange()

			LoadWeekData()
			LoadEmployeeAndCustomerEmployeeRPLList()

			LoadFirstEmployeeRPLIntoUI()
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			Me.grpEmplyoeeRPL.Text = m_Translate.GetSafeTranslationValue("Kandidat") + ":"
			Me.grpCustomerRPL.Text = m_Translate.GetSafeTranslationValue("Kunde") + ":"

			Me.grpRPLData.Text = m_Translate.GetSafeTranslationValue(Me.grpRPLData.Text)
			Me.lblKostenstelle.Text = m_Translate.GetSafeTranslationValue(Me.lblKostenstelle.Text)

			Me.lblWoche.Text = m_Translate.GetSafeTranslationValue(Me.lblWoche.Text)
			Me.lblVon.Text = m_Translate.GetSafeTranslationValue(Me.lblVon.Text)
			Me.lblBis.Text = m_Translate.GetSafeTranslationValue(Me.lblBis.Text)

			Me.lblLohnart.Text = m_Translate.GetSafeTranslationValue(Me.lblLohnart.Text)
			Me.lblZusatz.Text = m_Translate.GetSafeTranslationValue(Me.lblZusatz.Text)
			Me.lblAnzahl.Text = m_Translate.GetSafeTranslationValue(Me.lblAnzahl.Text)
			Me.lblAnsatz.Text = m_Translate.GetSafeTranslationValue(Me.lblAnsatz.Text)
			Me.lblBasis.Text = m_Translate.GetSafeTranslationValue(Me.lblBasis.Text)
			Me.lblResult.Text = m_Translate.GetSafeTranslationValue(Me.lblResult.Text)
			Me.lblMwSt.Text = m_Translate.GetSafeTranslationValue(Me.lblMwSt.Text)

			Me.btnSave.Text = m_Translate.GetSafeTranslationValue(Me.btnSave.Text)
			Me.btnOpenPDF.Text = m_Translate.GetSafeTranslationValue(Me.btnOpenPDF.Text)

			Me.grpZuschlaege.Text = m_Translate.GetSafeTranslationValue(Me.grpZuschlaege.Text)
			Me.grpGleitzeiten.Text = m_Translate.GetSafeTranslationValue(Me.grpGleitzeiten.Text)

		End Sub

#Region "Reset"

		''' <summary>
		''' Resets the customer KST drop down.
		''' </summary>
		Private Sub ResetCustomerKSTDropDown()

			lueCustomerKST.Properties.DisplayMember = "Description"
			lueCustomerKST.Properties.ValueMember = "RecordNumber"

			lueCustomerKST.Properties.ReadOnly = False
			lueCustomerKST.Properties.BestFitMode = BestFitMode.BestFitResizePopup


			gvCustomerKST.OptionsView.ShowIndicator = False
			gvCustomerKST.OptionsView.ShowColumnHeaders = False
			gvCustomerKST.OptionsView.ShowFooter = False

			gvCustomerKST.OptionsView.ShowAutoFilterRow = True
			gvCustomerKST.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvCustomerKST.Columns.Clear()

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Beschreibung")
			columnDescription.Name = "Description"
			columnDescription.FieldName = "Description"
			columnDescription.Visible = True
			columnDescription.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvCustomerKST.Columns.Add(columnDescription)


			lueCustomerKST.Properties.NullText = String.Empty
			lueCustomerKST.EditValue = Nothing
			lueCustomerKST.Properties.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the week drop down.
		''' </summary>
		Private Sub ResetWeekDropDown()

			lueWeek.Properties.ReadOnly = False
			lueWeek.Properties.DropDownRows = 20

			lueWeek.Properties.DisplayMember = "CalendarWeek"
			lueWeek.Properties.ValueMember = "CalendarWeek"

			Dim columns = lueWeek.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("CalendarWeek", 0, String.Empty))

			lueWeek.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueWeek.Properties.SearchMode = SearchMode.AutoComplete
			lueWeek.Properties.AutoSearchColumnIndex = 1

			lueWeek.Properties.NullText = String.Empty
			lueWeek.EditValue = Nothing
			lueWeek.Properties.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the LA drop down data.
		''' </summary>
		Private Sub ResetLADropDown()

			lueLAData.Properties.ReadOnly = False

			lueLAData.Properties.DisplayMember = "DisplayText"
			lueLAData.Properties.ValueMember = "LANr"

			' Reset the grid view
			gvLueLAData.OptionsView.ShowIndicator = False
			gvLueLAData.OptionsView.ShowColumnHeaders = False
			gvLueLAData.OptionsView.ShowFooter = False

			gvLueLAData.Columns.Clear()

			Dim laNrColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			laNrColumn.Caption = m_Translate.GetSafeTranslationValue("LANr")
			laNrColumn.Name = "LANr"
			laNrColumn.FieldName = "LANr"
			laNrColumn.Visible = True

			laNrColumn.DisplayFormat.FormatString = "0.###"
			laNrColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom
			gvLueLAData.Columns.Add(laNrColumn)

			Dim translatedLATextColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			translatedLATextColumn.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			translatedLATextColumn.Name = "TranslatedLaText"
			translatedLATextColumn.FieldName = "TranslatedLaText"
			translatedLATextColumn.Visible = True
			gvLueLAData.Columns.Add(translatedLATextColumn)

			lueLAData.Properties.BestFitMode = BestFitMode.BestFitResizePopup

			lueLAData.Properties.NullText = String.Empty
			lueLAData.EditValue = Nothing
			lueLAData.Properties.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the additional info drop down data.
		''' </summary>
		Private Sub ResetAdditionalInfoDropDown()

			lueAdditionalInfo.Properties.DisplayMember = "AdditionalText"
			lueAdditionalInfo.Properties.ValueMember = "AdditionalText"
			lueAdditionalInfo.Properties.TextEditStyle = TextEditStyles.Standard
			lueAdditionalInfo.Properties.ReadOnly = False

			gvAdditionalInfo.OptionsView.ShowIndicator = False
			gvAdditionalInfo.OptionsView.ShowColumnHeaders = False
			gvAdditionalInfo.OptionsView.ShowFooter = False
			gvAdditionalInfo.OptionsView.ShowAutoFilterRow = True
			gvAdditionalInfo.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

			gvAdditionalInfo.Columns.Clear()

			Dim columnAdditionalText As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAdditionalText.Caption = m_Translate.GetSafeTranslationValue("AdditionalText")
			columnAdditionalText.Name = "AdditionalText"
			columnAdditionalText.FieldName = "AdditionalText"
			columnAdditionalText.Visible = True
			columnAdditionalText.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			gvAdditionalInfo.Columns.Add(columnAdditionalText)

			lueAdditionalInfo.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueAdditionalInfo.Properties.NullText = String.Empty
			lueAdditionalInfo.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the employee RPL grid.
		''' </summary>
		Private Sub ResetEmployeeRPLGrid()

			' Reset the grid
			gvEmployeeRPL.OptionsView.ShowIndicator = False
			gvEmployeeRPL.OptionsView.ColumnAutoWidth = True

			gvEmployeeRPL.Columns.Clear()

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("LA")
			columnLANr.Name = "LANr"
			columnLANr.FieldName = "LANr"
			columnLANr.Visible = True
			columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLANr.DisplayFormat.FormatString = "0.###"
			gvEmployeeRPL.Columns.Add(columnLANr)

			Dim columnLAText As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLAText.Caption = m_Translate.GetSafeTranslationValue("Lohnart")
			columnLAText.Name = "TranslatedLAText"
			columnLAText.FieldName = "TranslatedLAText"
			columnLAText.Visible = True
			gvEmployeeRPL.Columns.Add(columnLAText)

			Dim columnRPLTime As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPLTime.Caption = m_Translate.GetSafeTranslationValue("Zeit")
			columnRPLTime.Name = "rpltime"
			columnRPLTime.FieldName = "rpltime"
			columnRPLTime.Visible = True
			gvEmployeeRPL.Columns.Add(columnRPLTime)


			Dim columnAnzahl As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAnzahl.Caption = m_Translate.GetSafeTranslationValue("Anzahl")
			columnAnzahl.Name = "Anzahl"
			columnAnzahl.FieldName = "Anzahl"
			columnAnzahl.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAnzahl.AppearanceHeader.Options.UseTextOptions = True
			columnAnzahl.Visible = True
			columnAnzahl.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAnzahl.DisplayFormat.FormatString = "N2"
			gvEmployeeRPL.Columns.Add(columnAnzahl)

			Dim columnBasis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBasis.Caption = m_Translate.GetSafeTranslationValue("Basis")
			columnBasis.Name = "Basis"
			columnBasis.FieldName = "Basis"
			columnBasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBasis.AppearanceHeader.Options.UseTextOptions = True
			columnBasis.Visible = True
			columnBasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBasis.DisplayFormat.FormatString = "N2"
			gvEmployeeRPL.Columns.Add(columnBasis)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "Betrag"
			columnBetrag.FieldName = "Betrag"
			columnBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnBetrag.Visible = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			gvEmployeeRPL.Columns.Add(columnBetrag)

			Dim columnRPLKWVon As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPLKWVon.Caption = m_Translate.GetSafeTranslationValue("KW-Von")
			columnRPLKWVon.Name = "rplkwvon"
			columnRPLKWVon.FieldName = "rplkwvon"
			columnRPLKWVon.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnRPLKWVon.AppearanceHeader.Options.UseTextOptions = True
			columnRPLKWVon.Visible = False
			columnRPLKWVon.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnRPLKWVon.DisplayFormat.FormatString = "N0"
			gvEmployeeRPL.Columns.Add(columnRPLKWVon)

			Dim columnRPLKWBis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPLKWBis.Caption = m_Translate.GetSafeTranslationValue("KW-Bis")
			columnRPLKWBis.Name = "rplkwbis"
			columnRPLKWBis.FieldName = "rplkwbis"
			columnRPLKWBis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnRPLKWBis.AppearanceHeader.Options.UseTextOptions = True
			columnRPLKWBis.Visible = False
			columnRPLKWBis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnRPLKWBis.DisplayFormat.FormatString = "N0"
			gvEmployeeRPL.Columns.Add(columnRPLKWBis)

			Dim columnRPLKW As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPLKW.Caption = m_Translate.GetSafeTranslationValue("W")
			columnRPLKW.Name = "rplkw"
			columnRPLKW.FieldName = "rplkw"
			columnRPLKW.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnRPLKW.AppearanceHeader.Options.UseTextOptions = True
			columnRPLKW.Visible = True
			gvEmployeeRPL.Columns.Add(columnRPLKW)

			Dim columnKSTNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKSTNr.Caption = m_Translate.GetSafeTranslationValue("KST.")
			columnKSTNr.Name = "KSTNr"
			columnKSTNr.FieldName = "KSTNr"
			columnKSTNr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnKSTNr.AppearanceHeader.Options.UseTextOptions = True
			columnKSTNr.Visible = False
			gvEmployeeRPL.Columns.Add(columnKSTNr)

			Dim columnKSTName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKSTName.Caption = m_Translate.GetSafeTranslationValue("Kostenstelle")
			columnKSTName.Name = "kstname"
			columnKSTName.FieldName = "kstname"
			columnKSTName.Visible = False
			gvEmployeeRPL.Columns.Add(columnKSTName)

			Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
			docType.Caption = m_Translate.GetSafeTranslationValue("Scan")
			docType.Name = "RPLDocScan"
			docType.FieldName = "RPLDocScan"
			docType.Visible = True
			docType.ColumnEdit = New RepositoryItemPictureEdit()
			docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
			gvEmployeeRPL.Columns.Add(docType)

			RestoreGridLayoutFromXml(gvEmployeeRPL.Name.ToLower)

			grdEmployeeRPL.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the employee salary types grid
		''' </summary>
		Private Sub ResetEmployeeSalaryTypesGrid()

			' Reset the grid
			gvEmployeeSalaryTypeData.OptionsView.ShowIndicator = False
			gvEmployeeSalaryTypeData.OptionsView.ShowColumnHeaders = False
			gvEmployeeSalaryTypeData.OptionsSelection.EnableAppearanceFocusedRow = False

			gvEmployeeSalaryTypeData.Columns.Clear()

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("LANr")
			columnLANr.Name = "LANr"
			columnLANr.FieldName = "LANr"
			columnLANr.Visible = True
			columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLANr.DisplayFormat.FormatString = "0.###"
			columnLANr.Width = 30
			gvEmployeeSalaryTypeData.Columns.Add(columnLANr)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "LABez"
			columnDescription.FieldName = "LABez"
			columnDescription.Visible = True
			columnDescription.Width = 100
			gvEmployeeSalaryTypeData.Columns.Add(columnDescription)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "Betrag"
			columnBetrag.FieldName = "Betrag"
			columnBetrag.Visible = True
			columnBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnBetrag.Visible = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			columnBetrag.Width = 30
			gvEmployeeSalaryTypeData.Columns.Add(columnBetrag)

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			grdEmployeeSalaryTypeData.DataSource = Nothing
			m_SuppressUIEvents = suppressUIEventsState

		End Sub

		''' <summary>
		''' Resets the customer RPL grid.
		''' </summary>
		Private Sub ResetCustomerRPLGrid()

			' Reset the grid
			gvCustomerRPL.OptionsView.ShowIndicator = False
			gvCustomerRPL.OptionsView.ColumnAutoWidth = True

			gvCustomerRPL.Columns.Clear()

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("LA")
			columnLANr.Name = "LANr"
			columnLANr.FieldName = "LANr"
			columnLANr.Visible = True
			columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLANr.DisplayFormat.FormatString = "0.###"
			gvCustomerRPL.Columns.Add(columnLANr)

			Dim columnLAText As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLAText.Caption = m_Translate.GetSafeTranslationValue("Lohnart")
			columnLAText.Name = "TranslatedLAText"
			columnLAText.FieldName = "TranslatedLAText"
			columnLAText.Visible = True
			gvCustomerRPL.Columns.Add(columnLAText)

			Dim columnRPLTime As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPLTime.Caption = m_Translate.GetSafeTranslationValue("Zeit")
			columnRPLTime.Name = "rpltime"
			columnRPLTime.FieldName = "rpltime"
			columnRPLTime.Visible = True
			gvCustomerRPL.Columns.Add(columnRPLTime)

			Dim columnAnzahl As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAnzahl.Caption = m_Translate.GetSafeTranslationValue("Anzahl")
			columnAnzahl.Name = "Anzahl"
			columnAnzahl.FieldName = "Anzahl"
			columnAnzahl.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAnzahl.AppearanceHeader.Options.UseTextOptions = True
			columnAnzahl.Visible = True
			columnAnzahl.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAnzahl.DisplayFormat.FormatString = "N2"
			gvCustomerRPL.Columns.Add(columnAnzahl)

			Dim columnBasis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBasis.Caption = m_Translate.GetSafeTranslationValue("Basis")
			columnBasis.Name = "Basis"
			columnBasis.FieldName = "Basis"
			columnBasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBasis.AppearanceHeader.Options.UseTextOptions = True
			columnBasis.Visible = True
			columnBasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBasis.DisplayFormat.FormatString = "N2"
			gvCustomerRPL.Columns.Add(columnBasis)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "Betrag"
			columnBetrag.FieldName = "Betrag"
			columnBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnBetrag.Visible = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			gvCustomerRPL.Columns.Add(columnBetrag)

			Dim columnRPLKWVon As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPLKWVon.Caption = m_Translate.GetSafeTranslationValue("KW-Von")
			columnRPLKWVon.Name = "rplkwvon"
			columnRPLKWVon.FieldName = "rplkwvon"
			columnRPLKWVon.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnRPLKWVon.AppearanceHeader.Options.UseTextOptions = True
			columnRPLKWVon.Visible = False
			columnRPLKWVon.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnRPLKWVon.DisplayFormat.FormatString = "N0"
			gvCustomerRPL.Columns.Add(columnRPLKWVon)

			Dim columnRPLKWBis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPLKWBis.Caption = m_Translate.GetSafeTranslationValue("KW-Bis")
			columnRPLKWBis.Name = "rplkwbis"
			columnRPLKWBis.FieldName = "rplkwbis"
			columnRPLKWBis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnRPLKWBis.AppearanceHeader.Options.UseTextOptions = True
			columnRPLKWBis.Visible = False
			columnRPLKWBis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnRPLKWBis.DisplayFormat.FormatString = "N0"
			gvCustomerRPL.Columns.Add(columnRPLKWBis)

			Dim columnRPLKW As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRPLKW.Caption = m_Translate.GetSafeTranslationValue("W")
			columnRPLKW.Name = "rplkw"
			columnRPLKW.FieldName = "rplkw"
			columnRPLKW.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnRPLKW.AppearanceHeader.Options.UseTextOptions = True
			columnRPLKW.Visible = True
			gvCustomerRPL.Columns.Add(columnRPLKW)

			Dim columnKSTNr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKSTNr.Caption = m_Translate.GetSafeTranslationValue("KST.")
			columnKSTNr.Name = "KSTNr"
			columnKSTNr.FieldName = "KSTNr"
			columnKSTNr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnKSTNr.AppearanceHeader.Options.UseTextOptions = True
			columnKSTNr.Visible = False
			gvCustomerRPL.Columns.Add(columnKSTNr)

			Dim columnKSTName As New DevExpress.XtraGrid.Columns.GridColumn()
			columnKSTName.Caption = m_Translate.GetSafeTranslationValue("Kostenstelle")
			columnKSTName.Name = "kstname"
			columnKSTName.FieldName = "kstname"
			columnKSTName.Visible = False
			gvCustomerRPL.Columns.Add(columnKSTName)

			Dim existingReColumn As New DevExpress.XtraGrid.Columns.GridColumn()
			existingReColumn.Caption = m_Translate.GetSafeTranslationValue("F")
			existingReColumn.Name = "ReNr"
			existingReColumn.FieldName = "ReNr"
			existingReColumn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			existingReColumn.AppearanceHeader.Options.UseTextOptions = True
			existingReColumn.Visible = True
			existingReColumn.ColumnEdit = m_CheckEditReExisting
			existingReColumn.UnboundType = DevExpress.Data.UnboundColumnType.Boolean
			gvCustomerRPL.Columns.Add(existingReColumn)

			Dim columnRENumber As New DevExpress.XtraGrid.Columns.GridColumn()
			columnRENumber.Caption = m_Translate.GetSafeTranslationValue("RE-Nr.")
			columnRENumber.Name = "RENr"
			columnRENumber.FieldName = "RENr"
			columnRENumber.Visible = False
			gvCustomerRPL.Columns.Add(columnRENumber)

			Dim docType As New DevExpress.XtraGrid.Columns.GridColumn()
			docType.Caption = m_Translate.GetSafeTranslationValue("Scan")
			docType.Name = "RPLDocScan"
			docType.FieldName = "RPLDocScan"
			docType.Visible = True
			docType.ColumnEdit = New RepositoryItemPictureEdit()
			docType.UnboundType = DevExpress.Data.UnboundColumnType.Object
			gvCustomerRPL.Columns.Add(docType)

			RestoreGridLayoutFromXml(gvCustomerRPL.Name.ToLower)

			grdCustomerRPL.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the customer salary types grid
		''' </summary>
		Private Sub ResetCustomerSalaryTypesGrid()

			' Reset the grid
			gvCustomerSalaryTypeData.OptionsView.ShowIndicator = False
			gvCustomerSalaryTypeData.OptionsView.ShowColumnHeaders = False
			gvCustomerSalaryTypeData.OptionsSelection.EnableAppearanceFocusedRow = False

			gvCustomerSalaryTypeData.Columns.Clear()

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("LANr")
			columnLANr.Name = "LANr"
			columnLANr.FieldName = "LANr"
			columnLANr.Visible = True
			columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLANr.DisplayFormat.FormatString = "0.###"
			columnLANr.Width = 30
			gvCustomerSalaryTypeData.Columns.Add(columnLANr)

			Dim columnDescription As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDescription.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
			columnDescription.Name = "LABez"
			columnDescription.FieldName = "LABez"
			columnDescription.Visible = True
			columnDescription.Width = 100
			gvCustomerSalaryTypeData.Columns.Add(columnDescription)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "Betrag"
			columnBetrag.FieldName = "Betrag"
			columnBetrag.Visible = True
			columnBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnBetrag.Visible = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			columnBetrag.Width = 30
			gvCustomerSalaryTypeData.Columns.Add(columnBetrag)

			Dim suppressUIEventsState = m_SuppressUIEvents
			m_SuppressUIEvents = True
			grdCustomerSalaryTypeData.DataSource = Nothing
			m_SuppressUIEvents = suppressUIEventsState

		End Sub

		''' <summary>
		''' Resets the RPL additional fee grid.
		''' </summary>
		Private Sub ResetRPLAdditionalFeeGrid()

			' Reset the grid
			gvRPLAdditionalFees.OptionsView.ShowIndicator = False
			gvRPLAdditionalFees.OptionsView.ColumnAutoWidth = True

			gvRPLAdditionalFees.Columns.Clear()

			Dim columnLANr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANr.Caption = m_Translate.GetSafeTranslationValue("LA")
			columnLANr.Name = "LANr"
			columnLANr.FieldName = "LANr"
			columnLANr.Visible = False
			columnLANr.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnLANr.DisplayFormat.FormatString = "0.###"
			gvRPLAdditionalFees.Columns.Add(columnLANr)

			Dim columnLANrTranslation As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLANrTranslation.Caption = m_Translate.GetSafeTranslationValue("Typ")
			columnLANrTranslation.Name = "LANrText"
			columnLANrTranslation.FieldName = "LANrText"
			columnLANrTranslation.Visible = True
			gvRPLAdditionalFees.Columns.Add(columnLANrTranslation)

			Dim columnAnsatz As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAnsatz.Caption = m_Translate.GetSafeTranslationValue("Ansatz")
			columnAnsatz.Name = "M_Ansatz"
			columnAnsatz.FieldName = "M_Ansatz"
			columnAnsatz.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAnsatz.AppearanceHeader.Options.UseTextOptions = True
			columnAnsatz.Visible = True
			columnAnsatz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAnsatz.DisplayFormat.FormatString = "N2"
			gvRPLAdditionalFees.Columns.Add(columnAnsatz)

			Dim columnBasis As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBasis.Caption = m_Translate.GetSafeTranslationValue("Basis")
			columnBasis.Name = "M_Basis"
			columnBasis.FieldName = "M_Basis"
			columnBasis.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBasis.AppearanceHeader.Options.UseTextOptions = True
			columnBasis.Visible = True
			columnBasis.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBasis.DisplayFormat.FormatString = "N2"
			gvRPLAdditionalFees.Columns.Add(columnBasis)

			Dim columnBetrag As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnBetrag.Name = "M_Betrag"
			columnBetrag.FieldName = "M_Betrag"
			columnBetrag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnBetrag.AppearanceHeader.Options.UseTextOptions = True
			columnBetrag.Visible = True
			columnBetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnBetrag.DisplayFormat.FormatString = "N2"
			gvRPLAdditionalFees.Columns.Add(columnBetrag)

			grdRPLAdditionalFees.DataSource = Nothing

		End Sub

		''' <summary>
		''' Resets the RPL flexible time grid.
		''' </summary>
		Private Sub ResetRPLFlexibleTimeGrid()

			' Reset the grid
			gvFlexibleTime.OptionsView.ShowIndicator = False
			gvFlexibleTime.OptionsView.ColumnAutoWidth = True

			gvFlexibleTime.Columns.Clear()

			Dim columnHours As New DevExpress.XtraGrid.Columns.GridColumn()
			columnHours.Caption = m_Translate.GetSafeTranslationValue("Stunden")
			columnHours.Name = "KompStd"
			columnHours.FieldName = "KompStd"
			columnHours.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnHours.AppearanceHeader.Options.UseTextOptions = True
			columnHours.Visible = True
			columnHours.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnHours.DisplayFormat.FormatString = "N2"
			gvFlexibleTime.Columns.Add(columnHours)

			Dim columnAmount As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAmount.Caption = m_Translate.GetSafeTranslationValue("Betrag")
			columnAmount.Name = "KompBetrag"
			columnAmount.FieldName = "KompBetrag"
			columnAmount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			columnAmount.AppearanceHeader.Options.UseTextOptions = True
			columnAmount.Visible = True
			columnAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
			columnAmount.DisplayFormat.FormatString = "N2"
			gvFlexibleTime.Columns.Add(columnAmount)

			grdFlexibletime.DataSource = Nothing

		End Sub

#End Region

#Region "Load data"

		''' <summary>
		''' Loads employee report data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeRPList() As Boolean

			Dim activeReport = m_UCMediator.ActiveReportData
			Dim esLohnNr As Integer? = Nothing
			Dim selectedESSalaryData = m_UCMediator.SelectedESSalaryData

			' Filter ESLohnNr 
			If Not selectedESSalaryData Is Nothing Then
				esLohnNr = selectedESSalaryData.ESLohnNr
			End If

			If Not activeReport Is Nothing Then

				Dim employeeRPLData = Nothing
				If Not activeReport Is Nothing Then

					Dim langEmployee As String = activeReport.EmployeeOfActiveReport.Language

					employeeRPLData = m_ReportDataAccess.LoadRPLListData(activeReport.ReportData.RPNR, langEmployee, DatabaseAccess.Report.RPLType.Employee, esLohnNr)

					If employeeRPLData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen (Mitarbeiter) konnten nicht geladen werden."))
					End If

				End If

				Dim employee = activeReport.EmployeeOfActiveReport
				Dim salutation = String.Empty
				Select Case employee.Gender
					Case "M"
						salutation = "Herr"
					Case "W"
						salutation = "Frau"
					Case Else
						salutation = "Herr"
				End Select

				Dim previousState = SetSuppressUIEventsState(True)
				grdEmployeeRPL.DataSource = employeeRPLData
				gvEmployeeRPL.BestFitColumns()

				SetSuppressUIEventsState(previousState)

				grpEmplyoeeRPL.Text = String.Format("{0}: <b>{1} {2} {3}</b>",
																						m_Translate.GetSafeTranslationValue("Kandidat"),
																						m_Translate.GetSafeTranslationValue(salutation),
																						employee.Lastname, employee.Firstname)
				Return Not employeeRPLData Is Nothing
			Else
				grdEmployeeRPL.DataSource = Nothing

				grpEmplyoeeRPL.Text = m_Translate.GetSafeTranslationValue("Kandidat") + ":"

				Return True

			End If

		End Function

		''' <summary>
		''' Loads employee additional ES salary list.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadEmployeeAdditionalESSalaryList() As Boolean

			Dim activeReport = m_UCMediator.ActiveReportData
			Dim esLohnNr As Integer? = Nothing
			Dim selectedESSalaryData = m_UCMediator.SelectedESSalaryData

			If Not activeReport Is Nothing Then

				Dim nonAssignedData = m_ESDataAccess.LoadESAdditionalSalaryTypeData(activeReport.ReportData.ESNR, DatabaseAccess.ES.ESAdditionalSalaryType.Employee, Nothing, 0)
				Dim dataAssignedToSelectedESLohnNr = m_ESDataAccess.LoadESAdditionalSalaryTypeData(activeReport.ReportData.ESNR, DatabaseAccess.ES.ESAdditionalSalaryType.Employee, Nothing, selectedESSalaryData.ESLohnNr)

				Dim completeList = New List(Of SP.DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData)

				completeList.AddRange(nonAssignedData)
				completeList.AddRange(dataAssignedToSelectedESLohnNr)

				m_AdditionalEmployeeESSalaryTypeData = completeList

				grdEmployeeSalaryTypeData.DataSource = m_AdditionalEmployeeESSalaryTypeData

				If m_AdditionalEmployeeESSalaryTypeData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnarten (Kandidat) konnten nicht geladen werden."))
				End If

				Return Not m_AdditionalEmployeeESSalaryTypeData Is Nothing
			Else
				grdEmployeeSalaryTypeData.DataSource = Nothing
				Return False
			End If

		End Function

		''' <summary>
		''' Loads customer report data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadCustomerRPList() As Boolean

			Dim activeReport = m_UCMediator.ActiveReportData

			Dim esLohnNr As Integer? = Nothing
			Dim selectedESSalaryData = m_UCMediator.SelectedESSalaryData

			' Filter ESLohnNr 
			If Not selectedESSalaryData Is Nothing Then
				esLohnNr = selectedESSalaryData.ESLohnNr
			End If

			If Not activeReport Is Nothing Then

				Dim customerRPLData = Nothing
				If Not activeReport Is Nothing Then

					Dim langCustomer As String = activeReport.CustomerOfActiveReport.Language

					customerRPLData = m_ReportDataAccess.LoadRPLListData(activeReport.ReportData.RPNR, langCustomer, DatabaseAccess.Report.RPLType.Customer, esLohnNr)

					If customerRPLData Is Nothing Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rapportzeilen (Kunde) konnten nicht geladen werden."))
					End If

				End If

				Dim customer = activeReport.CustomerOfActiveReport

				Dim previousState = SetSuppressUIEventsState(True)
				grdCustomerRPL.DataSource = customerRPLData
				gvCustomerRPL.BestFitColumns()
				SetSuppressUIEventsState(previousState)

				grpCustomerRPL.Text = String.Format("{0}: <b>{1}</b>", m_Translate.GetSafeTranslationValue("Kunde"), customer.Company1)

				grpRPLData.AllowHtmlText = True
				Dim WeeklyBillType As Boolean = customer.BillTypeCode.Contains("W")
				Dim WeeklyMessage As String = If(WeeklyBillType, String.Format("<color=255, 0, 0>({0})", m_Translate.GetSafeTranslationValue("Bitte Stunden wöchentlich eingeben!")), "")

				Dim scanSignedReports As Boolean = customer.OPShipment.Contains("99")
				Dim scanReportMessage As String = If(scanSignedReports, String.Format(m_Translate.GetSafeTranslationValue("<color=255, 0, 0>({0})"), "Rapporte scannen und hinterlegen!"), "")
				Dim attentionMessage As String = If(WeeklyMessage <> String.Empty OrElse scanReportMessage <> String.Empty, String.Format(m_Translate.GetSafeTranslationValue("{0}"), "Achtung:"), "")

				grpRPLData.Text = String.Format("{0} <b>{1}</b> {2} {3} {4}", m_Translate.GetSafeTranslationValue("Rapportdaten"), attentionMessage, WeeklyMessage,
																						If(WeeklyMessage <> String.Empty AndAlso scanReportMessage <> String.Empty, "|", ""), scanReportMessage)

				Return Not customerRPLData Is Nothing

			Else

				grdCustomerRPL.DataSource = Nothing
				grpCustomerRPL.Text = m_Translate.GetSafeTranslationValue("Kunde") + ":"
				Return True

			End If

		End Function

		''' <summary>
		''' Loads customer additional ES salary list.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadCustomerAdditionalESSalaryList() As Boolean

			Dim activeReport = m_UCMediator.ActiveReportData
			Dim esLohnNr As Integer? = Nothing
			Dim selectedESSalaryData = m_UCMediator.SelectedESSalaryData

			If Not activeReport Is Nothing Then

				Dim nonAssignedData = m_ESDataAccess.LoadESAdditionalSalaryTypeData(activeReport.ReportData.ESNR, DatabaseAccess.ES.ESAdditionalSalaryType.Customer, Nothing, 0)
				Dim dataAssignedToSelectedESLohnNr = m_ESDataAccess.LoadESAdditionalSalaryTypeData(activeReport.ReportData.ESNR, DatabaseAccess.ES.ESAdditionalSalaryType.Customer, Nothing, selectedESSalaryData.ESLohnNr)

				Dim completeList = New List(Of SP.DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData)

				completeList.AddRange(nonAssignedData)
				completeList.AddRange(dataAssignedToSelectedESLohnNr)

				m_AdditionalCustomerESSalaryTypeData = completeList

				grdCustomerSalaryTypeData.DataSource = m_AdditionalCustomerESSalaryTypeData

				If m_AdditionalCustomerESSalaryTypeData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnarten (Kunde) konnten nicht geladen werden."))
				End If

				Return Not m_AdditionalCustomerESSalaryTypeData Is Nothing
			Else
				grdCustomerSalaryTypeData.DataSource = Nothing
				Return False
			End If

		End Function

		''' <summary>
		''' Loads the selected employee RPL.
		''' </summary>
		Private Sub LoadSelectedEmployeeRPL()

			Dim selectedEmployeeRPL = SelectedEmployeeRPLData
			btnPrintDatamatrix.Visible = False

			If Not selectedEmployeeRPL Is Nothing Then
				LoadRPLToUI(selectedEmployeeRPL)
			End If

		End Sub

		''' <summary>
		''' Loads the selected customer RPL.
		''' </summary>
		Private Sub LoadSelectedCustomerRPL()

			Dim selectedCustomerRPL = SelectedCustomerRPLData
			btnPrintDatamatrix.Visible = Not String.IsNullOrWhiteSpace(m_DatamatrixprinterName) AndAlso Not selectedCustomerRPL Is Nothing

			If Not selectedCustomerRPL Is Nothing Then
				LoadRPLToUI(selectedCustomerRPL)
			End If

		End Sub

		''' <summary>
		''' Loads customer KST drop down data.
		''' </summary>
		''' <param name="customerNumber">The customer number.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadCustomerKSTDropdowndata(ByVal customerNumber As Integer) As Boolean

			Dim assignedKSTs = m_CustomerDatabaseAccess.LoadAssignedKSTsOfCustomer(customerNumber)

			If assignedKSTs Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kostenstellen konnten nicht geladen werden."))

				m_CustomerKSTList = Nothing

				lueCustomerKST.Properties.DataSource = Nothing

				Return False
			Else

				Dim listDataSource As BindingList(Of KSTViewData) = New BindingList(Of KSTViewData)

				' Convert the data to view data.
				For Each kst In assignedKSTs

					Dim invoiceAddressViewData = New KSTViewData() With {
									.Id = kst.ID,
									.RecordNumber = kst.RecordNumber,
									.Description = kst.Description
									}

					listDataSource.Add(invoiceAddressViewData)
				Next

				m_CustomerKSTList = listDataSource

				lueCustomerKST.Properties.DataSource = listDataSource

				Return True
			End If

		End Function

		''' <summary>
		''' Loads the additional text drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadAdditionalInfoDropdownData(ByVal customerNumber As Integer) As Boolean

			Dim additionalInfoTexts = m_ReportDataAccess.LoadRPLAdditionalTexts(customerNumber)

			If (additionalInfoTexts Is Nothing) Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zusatztexte konnten nicht geladen werden."))
			End If

			lueAdditionalInfo.Properties.DataSource = additionalInfoTexts

			Return Not additionalInfoTexts Is Nothing

		End Function

		''' <summary>
		''' Loads weeks beetween.
		''' </summary>
		Private Function LoadWeekData() As Boolean

			Dim activeReportData = m_UCMediator.ActiveReportData
			Dim selectedESSalaryData = m_UCMediator.SelectedESSalaryData


			Dim loBisDate = If(selectedESSalaryData.LOBis Is Nothing, DateTime.MaxValue, selectedESSalaryData.LOBis) ' LOBis could be NULL. In this case replace NULL with Date.MaxValue

			' Determine start and end date of work weeks list.
			Dim startDate = m_DateUtility.LimitToRange(selectedESSalaryData.LOVon, activeReportData.ReportData.Von, activeReportData.ReportData.Bis)
			Dim endDate = m_DateUtility.LimitToRange(loBisDate, activeReportData.ReportData.Von, activeReportData.ReportData.Bis)

			Dim weeks = m_DateUtility.GetCalendarWeeksBetweenDates(startDate, endDate)

			If weeks Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kalenderwochen konnten nicht ermittelt werden."))

				lueWeek.Properties.DataSource = Nothing
				lueWeek.Properties.ForceInitialize()

				Return False

			Else

				Dim listDataSource As BindingList(Of CalendarWeekViewData) = New BindingList(Of CalendarWeekViewData)

				' Convert the data to view data.
				For Each week In weeks

					Dim weekData = New CalendarWeekViewData() With {
							.CalendarWeek = week
						}

					listDataSource.Add(weekData)
				Next

				lueWeek.Properties.DataSource = listDataSource
				lueWeek.Properties.ForceInitialize()

				Return True

			End If
		End Function

		''' <summary>
		''' Loads LA data.
		''' </summary>
		''' <param name="year">The year.</param>
		''' <param name="rplType">The RPL type.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadLAData(ByVal year As Integer, ByVal rplType As RPLType) As Boolean

			Dim listOfLAData As List(Of LAData) = m_ReportDataAccess.LoadLAListData(year, m_InitializationData.UserData.UserLanguage, rplType)

			If listOfLAData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnarten konnten nicht geladen werden."))
				m_LAList = Nothing
			Else

				' Load additional LA data from ES
				Dim esNumber As Integer = m_UCMediator.ActiveReportData.ReportData.ESNR
				Dim additionESSalaryTypeData As List(Of DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData) = Nothing

				Select Case rplType
					Case DatabaseAccess.Report.RPLType.Employee
						additionESSalaryTypeData = m_ESDataAccess.LoadESAdditionalSalaryTypeData(esNumber, ESAdditionalSalaryType.Employee)
					Case DatabaseAccess.Report.RPLType.Customer
						additionESSalaryTypeData = m_ESDataAccess.LoadESAdditionalSalaryTypeData(esNumber, ESAdditionalSalaryType.Customer)
					Case Else
						' Do nothing
				End Select

				If additionESSalaryTypeData Is Nothing Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zusätzliche Einsatzlohnarten konnten nicht geladen werden."))
					m_LAList = listOfLAData
				Else

					Dim distinctLANrFromES = additionESSalaryTypeData.Select(Function(data) data.LANr).Distinct().ToArray()

					Dim newLAList As New List(Of LAData)

					' Add LANr numbers form ES salary types first to new list
					For Each laNr In distinctLANrFromES

						Dim theLaNrToFind = laNr
						Dim laData = listOfLAData.Where(Function(data) data.LANr = theLaNrToFind).FirstOrDefault

						If Not laData Is Nothing Then
							laData.Highlight = True
							newLAList.Add(laData)
							listOfLAData.Remove(laData)
						End If

					Next

					newLAList = newLAList.OrderBy(Function(data) data.LANr).ToList()

					' Add the rest of the LA data.
					For Each laData In listOfLAData
						laData.Highlight = False
						newLAList.Add(laData)
					Next

					m_LAList = newLAList
				End If
			End If

			lueLAData.EditValue = Nothing
			lueLAData.Properties.DataSource = m_LAList

			Return Not m_LAList Is Nothing

		End Function

		''' <summary>
		''' Sets the LA.
		''' </summary>
		''' <param name="laNr">The LANr.</param>
		''' <param name="year">The year.</param>
		''' <param name="rplType">Thre RPL type.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function SelectLA(ByVal laNr As Decimal, ByVal year As Integer, ByVal rplType As RPLType) As Boolean

			lueLAData.EditValue = Nothing

			' Add LA data that is not in the list
			If Not m_LAList.Any(Function(data) data.LANr = laNr) Then
				Dim resultList = m_ReportDataAccess.LoadLAListData(year, m_InitializationData.UserData.UserLanguage, rplType, laNr)

				If (resultList Is Nothing OrElse
					Not resultList.Count = 1) Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnart konnte nicht gesetzt werden."))
					lueLAData.EditValue = Nothing
					Return False
				Else
					Dim laDataToAdd = resultList(0)
					m_LAList.Add(laDataToAdd)
				End If

			End If

			lueLAData.EditValue = laNr

			Return True
		End Function

		''' <summary>
		''' Loads additional fee data of RPL.
		''' </summary>
		''' <param name="rpNr">The RPNr.</param>
		''' <param name="rpLNr">The RPLNr.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadAdditionalFeeDataOfRPL(ByVal rpNr As Integer, ByVal rpLNr As Integer) As Boolean

			Dim additionalFeeData = m_ReportDataAccess.LoadAdditionalFeeListOfEmployeeRPL(rpNr, rpLNr)

			If additionalFeeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Zuschläge konnten nicht geladen werden."))
			Else
				For Each feedata In additionalFeeData
					Select Case feedata.LANr
						Case LANr_Feiertag
							feedata.LANrText = m_Translate.GetSafeTranslationValue("Feiertag")
						Case LANR_Ferien
							feedata.LANrText = m_Translate.GetSafeTranslationValue("Ferien")
						Case LANr_Lohn13
							feedata.LANrText = m_Translate.GetSafeTranslationValue("13.Lohn")
						Case Else
							feedata.LANrText = String.Empty
					End Select
				Next
			End If

			grdRPLAdditionalFees.DataSource = additionalFeeData
			gvRPLAdditionalFees.BestFitColumns()

			Return Not additionalFeeData Is Nothing

		End Function

		''' <summary>
		''' Loads flexible time data of RPL.
		''' </summary>
		''' <param name="rpNr">The RPNr.</param>
		''' <param name="rpLNr">The RPLNr.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function LoadFlexibleTimeDataOfRPL(ByVal rpNr As Integer, ByVal rpLNr As Integer) As Boolean

			Dim flexibleTimeData = m_ReportDataAccess.LoadFlexibleTimeListOfRPL(rpNr, rpLNr)

			If flexibleTimeData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Gleitzeit konnte nicht geladen werden."))
			End If

			grdFlexibletime.DataSource = flexibleTimeData
			gvFlexibleTime.BestFitColumns()

			Return Not flexibleTimeData Is Nothing

		End Function


		''' <summary>
		''' Loads a RPL to the UI.
		''' </summary>
		''' <param name="rplData">The rpl data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function LoadRPLToUI(ByVal rplData As RPLListData) As Boolean

			Dim success As Boolean = True

			m_SelectedRPLListData = rplData
			m_ActiveRPLType = rplData.Type
			m_TimeTableDataToSave = Nothing

			Dim previousState = SetSuppressUIEventsState(True)

			lueCustomerKST.EditValue = If(rplData.KSTNr.HasValue, Convert.ToInt32(rplData.KSTNr.Value), Nothing)

			FillTimePeriodGroup(rplData)
			success = success AndAlso FillLohnartGroup(rplData)

			success = success AndAlso LoadAdditionalFeeDataOfRPL(rplData.RPNr, rplData.RPLNr)
			success = success AndAlso LoadFlexibleTimeDataOfRPL(rplData.RPNr, rplData.RPLNr)

			ConfigAdditionalFeesAndFlexibleTimeVisiblity()
			SetAccessStateOfControls()

			m_UCMediator.HighlightRPLTimeTableData(rplData)
			m_UCMediator.UpdateRPLChangeTimeInStatusBar(rplData.ChangedOn, rplData.ChangedFrom)

			If rplData.Type = RPLType.Employee Then
				SetFocusedAppearanceOnCustomerRPLGrid(False)
				SetFocusedAppearanceOnEmployeeRPLGrid(True)
			Else
				SetFocusedAppearanceOnEmployeeRPLGrid(False)
				SetFocusedAppearanceOnCustomerRPLGrid(True)
			End If

			SetSuppressUIEventsState(previousState)

			Return success
		End Function

		''' <summary>
		''' Sets the access state of controls.
		''' </summary>
		Private Sub SetAccessStateOfControls()

			If m_UCMediator.ActiveReportData Is Nothing Then
				Return
			End If

			Dim rpData = m_UCMediator.ActiveReportData.ReportData

			Dim isCustomerKSTReadonly = False
			Dim isWeekReadonly = False
			Dim isDateFromReadonly = False
			Dim isDateToReadonly = False
			Dim isLAReadonly = True
			Dim isAdditonalTextReadonly = False
			Dim isAnzahlReadonly = False
			Dim isBasisReadonly = False
			Dim isAnsatzReadonly = False
			Dim isMwstReadonly = False
			Dim isMwstVisible = False
			Dim isSaveEnabled = True
			Dim isNewEmployeeRPLEnabled = True
			Dim isNewCustomerRPLEnabled = True
			Dim isBtnOpenHourInputVisible = False

			If m_SelectedRPLListData Is Nothing Then

				Dim isMonthClosed = rpData.IsMonthClosed

				isCustomerKSTReadonly = isMonthClosed
				isWeekReadonly = isMonthClosed
				isDateFromReadonly = isMonthClosed
				isDateToReadonly = isMonthClosed
				isLAReadonly = isMonthClosed
				isAdditonalTextReadonly = isMonthClosed
				isAnzahlReadonly = isMonthClosed
				isBasisReadonly = isMonthClosed
				isAnsatzReadonly = isMonthClosed
				isMwstReadonly = isMonthClosed
				isSaveEnabled = Not isMonthClosed
				isNewEmployeeRPLEnabled = Not isMonthClosed
				isNewCustomerRPLEnabled = Not isMonthClosed
				isMwstVisible = (m_ActiveRPLType = RPLType.Customer)

			Else

				If Not CanSelectedRPLBeChanged Then
					isCustomerKSTReadonly = True
					isWeekReadonly = True
					isDateFromReadonly = True
					isDateToReadonly = True
					isLAReadonly = True
					isAdditonalTextReadonly = True
					isAnzahlReadonly = True
					isBasisReadonly = True
					isAnsatzReadonly = True
					isMwstReadonly = True
					isSaveEnabled = False
				End If

				If (m_SelectedRPLListData.Type = RPLType.Customer) Then
					isMwstVisible = True
				End If

				Dim la = SelectedLA

				If Not la Is Nothing Then

					If la.ProTag Then
						isAnzahlReadonly = True
						isWeekReadonly = True
						isDateFromReadonly = True
						isDateToReadonly = True
						isBtnOpenHourInputVisible = True
					End If

					If la.TypeAnzahl = 2 Then
						isAnzahlReadonly = True
					End If

					If la.TypeBasis = 2 Then
						isBasisReadonly = True
					End If

					If la.TypeAnsatz = 2 Then
						isAnsatzReadonly = True
					End If

				End If

			End If
			' do it anyway!
			If rpData.IsMonthClosed Then
				isNewEmployeeRPLEnabled = False
				isNewCustomerRPLEnabled = False
			End If
			If (rpData.LONr > 0) Then isNewEmployeeRPLEnabled = False


			lueCustomerKST.Properties.ReadOnly = isCustomerKSTReadonly
			lueWeek.Properties.ReadOnly = isWeekReadonly
			dateEditFromDate.Properties.ReadOnly = isDateFromReadonly
			dateEditToDate.Properties.ReadOnly = isDateToReadonly
			lueLAData.Properties.ReadOnly = isLAReadonly
			lueAdditionalInfo.Properties.ReadOnly = isAdditonalTextReadonly
			txtAnzahl.Properties.ReadOnly = isAnzahlReadonly
			txtBasis.Properties.ReadOnly = isBasisReadonly
			txtAnsatz.Properties.ReadOnly = isAnsatzReadonly
			txtMwSt.Properties.ReadOnly = isMwstReadonly
			txtMwSt.Visible = isMwstVisible
			lblMwSt.Visible = isMwstVisible
			btnSave.Enabled = isSaveEnabled AndAlso m_UserAllowedToChanged
			btnAddNewEmployeeRPL.Enabled = isNewEmployeeRPLEnabled
			btnAddNewCustomerRPL.Enabled = isNewCustomerRPLEnabled
			btnOpenHourInput.Visible = isBtnOpenHourInputVisible

		End Sub

		''' <summary>
		''' Configures the additional fees and flexible time visibility.
		''' </summary>
		Private Sub ConfigAdditionalFeesAndFlexibleTimeVisiblity()

			If m_ActiveRPLType = RPLType.Customer Then
				grpZuschlaege.Visible = False
				grpGleitzeiten.Visible = False
			Else
				grpZuschlaege.Visible = True
				grpGleitzeiten.Visible = True
			End If

		End Sub

		''' <summary>
		''' Fills the time period group.
		''' </summary>
		''' <param name="rpldata">The RPL data.</param>
		Private Sub FillTimePeriodGroup(ByVal rpldata As RPLListData)

			Dim rpData = m_UCMediator.ActiveReportData.ReportData

			Dim weekOfVonDate As Integer = DatePart(DateInterval.WeekOfYear, rpldata.VonDate.Value, FirstDayOfWeek.System, FirstWeekOfYear.System)
			Dim weekOfBisDate As Integer = DatePart(DateInterval.WeekOfYear, rpldata.BisDate.Value, FirstDayOfWeek.System, FirstWeekOfYear.System)

			' Set week of lookup only if the two dates are in the same week.
			lueWeek.EditValue = If(weekOfVonDate = weekOfBisDate, weekOfVonDate, Nothing)

			' Make sure min and max date or in report range.
			SetMinAndMaxDateToReportRange()

			dateEditFromDate.EditValue = rpldata.VonDate
			dateEditToDate.EditValue = rpldata.BisDate

		End Sub

		''' <summary>
		''' Fills the Lohnart group.
		''' </summary>
		''' <param name="rplData">The RPL data.</param>
		Private Function FillLohnartGroup(ByVal rplData As RPLListData) As Boolean

			Dim success As Boolean = True

			success = success AndAlso LoadLAData(rplData.VonDate.Value.Year, rplData.Type)
			success = success AndAlso SelectLA(rplData.LANr, rplData.VonDate.Value.Year, rplData.Type)

			SetAdditionalInfo(rplData.RPZusatzText)

			lblNegSign.Visible = rplData.Sign.Equals("-")

			AnzahlValueInUI = If(rplData.Anzahl.HasValue, rplData.Anzahl, Anzahl_Default_Value)

			If rplData.Basis.HasValue Then
				BasisValueInUI = If(rplData.Sign.Equals("-"), rplData.Basis.Value * -1.0, rplData.Basis.Value)
			Else
				BasisValueInUI = Basis_Default_Value
			End If

			AnsatzValueInUI = If(rplData.Ansatz.HasValue, rplData.Ansatz, Ansatz_Default_Value)
			BetragValueInUI = If(rplData.Betrag.HasValue, rplData.Betrag, Betrag_Default_Value)

			MwStInUI = If(rplData.MWST.HasValue, rplData.MWST, MwSt_Default_Value)

			RememberLAUpperBoundValuesBasedOnCurrentLAValues()

			Return success
		End Function

#End Region

#Region "New data"

		''' <summary>
		''' Prepares the form to enter an new RPL.
		''' </summary>
		''' <param name="type">The type of the RPL.</param>
		''' <returns>Boolean value indicating success.</returns>
		Private Function PrepareForNewRPL(ByVal type As RPLType) As Boolean

			Dim success As Boolean = True

			If m_UCMediator.ActiveReportData Is Nothing Then
				Return False
			End If

			Dim previousState = SetSuppressUIEventsState(True)

			errorProvider.Clear()

			m_SelectedRPLListData = Nothing
			m_ActiveRPLType = type
			m_TimeTableDataToSave = Nothing

			' Unfocus all rows
			SetFocusedAppearanceOnEmployeeRPLGrid(False)
			SetFocusedAppearanceOnCustomerRPLGrid(False)

			Dim rpData = m_UCMediator.ActiveReportData.ReportData

			Dim esSalaryData = m_UCMediator.SelectedESSalaryData
			' KST ist not touched -> it keeps its value
			If lueCustomerKST.EditValue Is Nothing Then
				lueCustomerKST.EditValue = esSalaryData.KSTNr
			End If

			Dim loBis = If(esSalaryData.LOBis Is Nothing, DateTime.MaxValue, esSalaryData.LOBis)

			' During entering of new date, the valid period to enter should be in range of ESSalary and not outsite the report.
			dateEditFromDate.Properties.MinValue = m_DateUtility.LimitToRange(esSalaryData.LOVon, rpData.Von.Value, rpData.Bis.Value)
			dateEditFromDate.Properties.MaxValue = m_DateUtility.LimitToRange(loBis, rpData.Von.Value, rpData.Bis.Value)
			dateEditToDate.Properties.MinValue = dateEditFromDate.Properties.MinValue
			dateEditToDate.Properties.MaxValue = dateEditFromDate.Properties.MaxValue

			' Group time period
			dateEditFromDate.EditValue = dateEditFromDate.Properties.MinValue
			dateEditToDate.EditValue = dateEditToDate.Properties.MaxValue

			Dim weekOfVonDate As Integer = DatePart(DateInterval.WeekOfYear, rpData.Von.Value, FirstDayOfWeek.System, FirstWeekOfYear.System)
			Dim weekOfBisDate As Integer = DatePart(DateInterval.WeekOfYear, rpData.Bis.Value, FirstDayOfWeek.System, FirstWeekOfYear.System)
			lueWeek.EditValue = If(weekOfVonDate = weekOfBisDate, weekOfVonDate, Nothing)

			' Group LA
			success = success AndAlso LoadLAData(Convert.ToInt32(rpData.Jahr), type)

			lueLAData.EditValue = Nothing
			If SetAdditionalInfoAsCustomerKST AndAlso lueCustomerKST.EditValue <> 1 Then
				SetAdditionalInfo(lueCustomerKST.Text)
			Else
				lueAdditionalInfo.EditValue = Nothing
			End If

			lblNegSign.Visible = False
			AnzahlValueInUI = Anzahl_Default_Value
			BasisValueInUI = Basis_Default_Value
			AnsatzValueInUI = Ansatz_Default_Value
			BetragValueInUI = Betrag_Default_Value
			MwStInUI = MwSt_Default_Value

			btnOpenHourInput.Visible = False

			grdRPLAdditionalFees.DataSource = Nothing
			grdFlexibletime.DataSource = Nothing

			If m_ActiveRPLType = RPLType.Employee Then
				m_UCMediator.LoadDayTotalDataOfRPLType(DatabaseAccess.Report.RPLType.Employee)
			Else
				m_UCMediator.LoadDayTotalDataOfRPLType(DatabaseAccess.Report.RPLType.Customer)
			End If
			m_UCMediator.UpdateRPLChangeTimeInStatusBar(Nothing, String.Empty)

			SetAccessStateOfControls()
			ConfigAdditionalFeesAndFlexibleTimeVisiblity()

			SetSuppressUIEventsState(previousState)

			Return success
		End Function

#End Region

#Region "Save data"

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			errorProvider.Clear()

			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")
			Dim errorTextToDateLiesBeforeFromDate As String = m_Translate.GetSafeTranslationValue("Bis Datum liegt vor Von Datum.")
			Dim errorTextDayHourDataMustBeEnterd As String = m_Translate.GetSafeTranslationValue("Sie müssen im Stundenplan die tage bzw. Fehlcodes eintragen.")
			Dim errorTextDateDoesNoFallInESSalaryPeriod = m_Translate.GetSafeTranslationValue("Der {0:dd.MM.yyyy} liegt ausserhalb der Lohnperiode {1:dd.MM.yyyy} bis {2}.")

			Dim isValid As Boolean = True

			isValid = isValid And SetErrorIfInvalid(lueCustomerKST, errorProvider, lueCustomerKST.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(dateEditFromDate, errorProvider, dateEditFromDate.EditValue Is Nothing, errorText)
			isValid = isValid And SetErrorIfInvalid(dateEditToDate, errorProvider, dateEditToDate.EditValue Is Nothing, errorTextToDateLiesBeforeFromDate)
			isValid = isValid And SetErrorIfInvalid(lueLAData, errorProvider, lueLAData.EditValue Is Nothing, errorText)

			' Check date to lies after date from
			If isValid Then

				Dim rplFromDate As DateTime = dateEditFromDate.EditValue
				Dim rplToDate As DateTime = dateEditToDate.EditValue

				isValid = isValid And SetErrorIfInvalid(dateEditToDate, errorProvider, rplToDate < rplFromDate, errorText)

			End If

			If isValid Then

				Dim laData = SelectedLA

				' Checks for new RPL Data that has to be enterd per day.
				If m_SelectedRPLListData Is Nothing AndAlso
					laData.ProTag.HasValue AndAlso
					laData.ProTag Then

					' Check that day hour data has been entered if the LA is per day and its a new RPL.
					isValid = isValid And SetErrorIfInvalid(btnOpenHourInput, errorProvider, m_TimeTableDataToSave Is Nothing AndAlso Not IgnoreHourlyDataCheckInValidation, errorTextDayHourDataMustBeEnterd)


					' Check if entered date period falls in ESLohn period.
					Dim selectedESSalaryData = m_UCMediator.SelectedESSalaryData

					Dim esLohnVon As DateTime = selectedESSalaryData.LOVon
					Dim esLohnBis As DateTime = If(selectedESSalaryData.LOBis Is Nothing, DateTime.MaxValue, selectedESSalaryData.LOBis)
					Dim esLohnBisTextual As String = If(esLohnBis = DateTime.MaxValue, "?", String.Format("{0:dd.MM.yyyy}", esLohnBis))

					Dim dateFrom As DateTime = dateEditFromDate.EditValue
					isValid = isValid And SetErrorIfInvalid(dateEditFromDate, errorProvider, (dateFrom < esLohnVon) Or (dateFrom > esLohnBis), String.Format(errorTextDateDoesNoFallInESSalaryPeriod, dateFrom, esLohnVon, esLohnBisTextual))

					Dim dateTo As DateTime = dateEditToDate.EditValue
					isValid = isValid And SetErrorIfInvalid(dateEditToDate, errorProvider, (dateTo < esLohnVon) Or (dateTo > esLohnBis), String.Format(errorTextDateDoesNoFallInESSalaryPeriod, dateTo, esLohnVon, esLohnBisTextual))


				End If

			End If

			Return isValid

		End Function

		''' <summary>
		''' Cleanup control.
		''' </summary>
		Public Overrides Sub CleanUp()

			If Not m_InvoiceAddressesForm Is Nothing AndAlso
					Not m_InvoiceAddressesForm.IsDisposed Then

				Try
					m_InvoiceAddressesForm.Close()
					m_InvoiceAddressesForm.Dispose()
				Catch
					' Do nothing
				End Try

			End If
		End Sub


		''' <summary>
		''' Saves RPL data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function SaveRPLData() As Boolean
			If m_InitializationData.MDData.ClosedMD = 1 Then Return False

			Dim success As Boolean = True

			If m_UCMediator.ActiveReportData Is Nothing OrElse
				m_UCMediator.ActiveReportData.ReportData.IsMonthClosed Then
				Return False
			End If

			If Not btnSave.Enabled Then
				Return False
			End If

			If ValidateData() Then

				If m_SelectedRPLListData Is Nothing Then
					If m_ActiveRPLType = RPLType.Employee Then
						success = CreateEmployeeRPLData()
						If success AndAlso m_EmployeeWithWeeklyPayment Then
							Dim la = SelectedLA
							If la.LANr <> 100 Then Return False

							If Not UCMediator.IsAktiveReportDone Then ShowAdvancePaymentDetailFrom()
						End If

					Else
						success = CreateCustomerRPLData()
					End If
				Else
					If m_ActiveRPLType = RPLType.Employee Then
						success = UpdateEmployeeRPLData()
					Else
						success = UpdateCustomerRPLData()
					End If
				End If

			Else
				success = False
			End If

			If Not success Then
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Report (RPL) Daten konnten nicht gespeichert werden."))
			End If

			Return success
		End Function

		''' <summary>
		''' Creates employee RPL data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function CreateEmployeeRPLData() As Boolean

			Dim rpData = m_UCMediator.ActiveReportData.ReportData
			Dim customerKST = SelectedCustomerKST
			Dim la = SelectedLA
			Dim esSalaryData = m_UCMediator.SelectedESSalaryData

			If la Is Nothing Or customerKST Is Nothing Or esSalaryData Is Nothing Then
				Return False
			End If

			Dim inputParams As New CreateEmployeeRPLParams

			inputParams.RPNr = rpData.RPNR
			inputParams.RPMonat = rpData.Monat
			inputParams.RPJahr = rpData.Jahr
			inputParams.RPGAVNr = rpData.RPGAV_Nr
			inputParams.RPGAV_StdWeek = rpData.RPGAV_StdWeek
			inputParams.MDNr = rpData.MDNr
			inputParams.KDNr = rpData.CustomerNumber
			inputParams.MANr = rpData.EmployeeNumber
			inputParams.ESNr = rpData.ESNR
			inputParams.KSTNR = customerKST.RecordNumber
			inputParams.KstBez = customerKST.Description
			inputParams.GAVText = esSalaryData.GAVText
			inputParams.Currency = rpData.Currency
			inputParams.LANr = la.LANr
			inputParams.M_Anzahl = AnzahlValueInUI
			inputParams.M_Basis = SignedBasisValueInUI
			inputParams.M_Ansatz = AnsatzValueInUI
			inputParams.SUVA = rpData.SUVA
			inputParams.M_Ferien = FerienAnsatzInUI
			inputParams.M_Feier = FeiertagAnsatzInUI
			inputParams.M_13 = Lohn13AnsatzInUI
			inputParams.VonDate = dateEditFromDate.EditValue
			inputParams.BisDate = dateEditToDate.EditValue
			inputParams.FerBas = FerienBasisInUI
			inputParams.Basis13 = Lohn13BasisInUI
			inputParams.ESLohnNr = esSalaryData.ESLohnNr
			inputParams.LOSpesenBas = esSalaryData.MAStdSpesen
			inputParams.LOSpesen = esSalaryData.MAStdSpesen > 0
			inputParams.MATSpesenBas = esSalaryData.MATSpesen
			inputParams.MATSpesen = esSalaryData.MATSpesen > 0
			inputParams.StdTotal = BetragValueInUIRoundedTo5Rappen
			inputParams.FeierTotal = FeiertagBetragInUI
			inputParams.FerTotal = FerienBetragInUI
			inputParams.Total13 = Lohn13BetragInUI
			inputParams.UserName = m_InitializationData.UserData.UserFullName
			inputParams.IsPVL = esSalaryData.IsPVL
			inputParams.RPZusatzText = lueAdditionalInfo.EditValue
			inputParams.Stundenlohn = esSalaryData.StundenLohn

			inputParams.TimeTable = m_TimeTableDataToSave
			inputParams.IsFlexibleTimeActive = IsFlexibleTimeActive

			' Check if data should be duplicated for the customer.
			If (la.DuppinKD.HasValue AndAlso la.DuppinKD) OrElse DoesAdditionalCustomerESSalaryTypeDataExistsForLANr(la.LANr) Then

				Dim esParameters As New RPLSalaryTypeValuesCalcESParams(rpData.ESNR, esSalaryData.Tarif, esSalaryData.GrundLohn, esSalaryData.MWStBetrag)
				esParameters.Stundenlohn = esSalaryData.StundenLohn
				Dim laParameters As New RPLSalaryTypeValuesCalcLAParams(la.LANr, la.TypeAnsatz, la.TypeBasis, la.FixAnsatz, la.MABasVar, la.KDBasis, la.MWSTPflichtig)
				Dim calculationParameters As New RPLSalaryTypeValuesCalcParams(rpData.EmployeeNumber, DefaultMwStValue(Val(rpData.Jahr)), esParameters, laParameters)
				esParameters.ESLohnIsMwStPflichtig = esSalaryData.ESLohnIsMwStPflichtig

				' Calculate salary type values for customer.
				Dim result As RPLSalaryTypeValuesCalcuResult = m_RPLValueCalculator.CalculateSalaryTypeValuesForCustomer(calculationParameters)

				Dim customerRPLDuplicationParams = Nothing

				If DoesAdditionalCustomerESSalaryTypeDataExistsForLANr(la.LANr) Then

					Dim mwst As Decimal = If(result.MwSt.HasValue, result.MwSt, MwSt_Default_Value)

					Dim customerAdditionalESSalaryTypeData = m_AdditionalCustomerESSalaryTypeData.Where(Function(data) data.LANr = la.LANr).FirstOrDefault()
					Dim basisValue = customerAdditionalESSalaryTypeData.Basis
					Dim ansatzValue = customerAdditionalESSalaryTypeData.Ansatz

					customerRPLDuplicationParams = New AdditionalParamsForCustomerRPLDuplication With {
							.BasisValue = basisValue,
							.AnsatzValue = ansatzValue,
							.LABasisFactor = BasisFactor,
							.KDTSpesen = esSalaryData.KDTSpesen,
							.MwSt = mwst}
				ElseIf Not DoesAdditionalEmployeeESSalaryTypeDataExistsForLANr(la.LANr) Then

					Dim basisValue = 0.0

					' Speziallfall: Wenn kein zusätzliche Lohnart im Einsatz vorhanden ist und KDBasis nicht 8000 ist, dann muss als Basis Wert der vom Kandidat genommen werden.
					If la.KDBasis = "8000" Then
						basisValue = If(result.Basis.HasValue, result.Basis, Basis_Default_Value)

					Else
						basisValue = BasisValueInUI
					End If

					Dim mwst As Decimal = If(result.MwSt.HasValue, result.MwSt, MwSt_Default_Value)

					customerRPLDuplicationParams = New AdditionalParamsForCustomerRPLDuplication With {
							.BasisValue = basisValue,
							.AnsatzValue = inputParams.M_Ansatz,
							.LABasisFactor = BasisFactor,
							.KDTSpesen = esSalaryData.KDTSpesen,
							.MwSt = mwst}

				End If

				inputParams.AdditionalParamsForCustomerRPLDuplication = customerRPLDuplicationParams

			End If


			' Search if LANr already exists for selected time
			Dim success = m_RPLCreateService.ExistsLANrForSameTime(inputParams.RPNr, inputParams.LANr, inputParams.VonDate, inputParams.BisDate, False)
			If success Then Return False

			' Create the emplyoee RPL
			success = m_RPLCreateService.CreateEmployeeRPL(inputParams)

			LoadEmployeeAndCustomerEmployeeRPLList()

			If success Then
				FocusEmployeeRPLData(inputParams.NewRPLNr)
				m_UCMediator.LoadDayTotalDataOfRPLType(DatabaseAccess.Report.RPLType.Employee)

				Dim rplData = SelectedEmployeeRPLData

				If Not inputParams.AdditionalParamsForCustomerRPLDuplication Is Nothing Then
					FocusCustomerRPLData(inputParams.NewCustomerDuplicatedRPLNr)
					Dim cutomerRPLData = SelectedCustomerRPLData
					cutomerRPLData.RPLNr = inputParams.NewCustomerDuplicatedRPLNr
					PrintDataMatrixCode(cutomerRPLData)
				End If

				LoadRPLToUI(rplData)
				Else
					LoadFirstEmployeeRPLIntoUI()
			End If

			Return success
		End Function

		''' <summary>
		''' Creates customer RPL data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function CreateCustomerRPLData() As Boolean

			Dim rpData = m_UCMediator.ActiveReportData.ReportData
			Dim customerKST = SelectedCustomerKST
			Dim la = SelectedLA
			Dim esSalaryData = m_UCMediator.SelectedESSalaryData

			If la Is Nothing Or customerKST Is Nothing Or esSalaryData Is Nothing Then
				Return False
			End If

			Dim inputParams As New CreateCustomerRPLParams

			inputParams.RPNr = rpData.RPNR
			inputParams.RPMonat = rpData.Monat
			inputParams.RPJahr = rpData.Jahr
			inputParams.RPGAVNr = rpData.RPGAV_Nr
			inputParams.RPGAV_StdWeek = rpData.RPGAV_StdWeek
			inputParams.KDNr = rpData.CustomerNumber
			inputParams.MANr = rpData.EmployeeNumber
			inputParams.ESNr = rpData.ESNR
			inputParams.KSTNR = customerKST.RecordNumber
			inputParams.KstBez = customerKST.Description
			inputParams.GAVText = esSalaryData.GAVText
			inputParams.Currency = rpData.Currency
			inputParams.LANr = la.LANr
			inputParams.K_Anzahl = AnzahlValueInUI
			inputParams.K_Basis = SignedBasisValueInUI
			inputParams.K_Ansatz = AnsatzValueInUI
			inputParams.MWST = MwStInUI
			inputParams.SUVA = rpData.SUVA
			inputParams.VonDate = dateEditFromDate.EditValue
			inputParams.BisDate = dateEditToDate.EditValue
			inputParams.ESLohnNr = esSalaryData.ESLohnNr
			inputParams.KDTSpesenBas = esSalaryData.KDTSpesen
			inputParams.KDTSpesen = esSalaryData.KDTSpesen > 0D
			inputParams.KDBetrag = BetragValueInUIRoundedTo5Rappen
			inputParams.UserName = m_InitializationData.UserData.UserFullName
			inputParams.RPZusatzText = lueAdditionalInfo.EditValue
			inputParams.IsCreatedWithEmployee = False

			inputParams.TimeTable = m_TimeTableDataToSave

			' Search if LANr already exists for selected time
			Dim success = m_RPLCreateService.ExistsLANrForSameTime(inputParams.RPNr, inputParams.LANr, inputParams.VonDate, inputParams.BisDate, True)
			If success Then Return False

			' Create customer RPL
			success = m_RPLCreateService.CreateCustomerRPL(inputParams)

			LoadEmployeeAndCustomerEmployeeRPLList()

			If success Then
				FocusCustomerRPLData(inputParams.NewRPLNr)
				m_UCMediator.LoadDayTotalDataOfRPLType(DatabaseAccess.Report.RPLType.Customer)

				Dim rplData = SelectedCustomerRPLData
				PrintDataMatrixCode(rplData)

				LoadRPLToUI(rplData)
			Else
				LoadFirstCustomerRPLIntoUI()
			End If


			Return success
		End Function

		''' <summary>
		''' Updates employee RPL.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Private Function UpdateEmployeeRPLData() As Boolean

			Dim rpData = m_UCMediator.ActiveReportData.ReportData
			Dim customerKST = SelectedCustomerKST
			Dim la = SelectedLA
			Dim esSalaryData = m_UCMediator.SelectedESSalaryData
			Dim selectedRPLData = m_SelectedRPLListData

			If Not CanSelectedRPLBeChanged Then
				Return False
			End If

			If la Is Nothing Or
				customerKST Is Nothing Or
				esSalaryData Is Nothing Or
				selectedRPLData Is Nothing Then
				Return False
			End If

			Dim inputParams As New UpdateEmployeeRPLParams
			inputParams.RPNr = rpData.RPNR
			inputParams.RPMonat = rpData.Monat
			inputParams.RPJahr = rpData.Jahr
			inputParams.RPGAVNr = rpData.RPGAV_Nr
			inputParams.RPGAV_StdWeek = rpData.RPGAV_StdWeek
			inputParams.RPLNr = selectedRPLData.RPLNr
			inputParams.LANr = la.LANr
			inputParams.M_Anzahl = AnzahlValueInUI
			inputParams.M_Basis = SignedBasisValueInUI
			inputParams.M_Ansatz = AnsatzValueInUI
			inputParams.M_Ferien = FerienAnsatzInUI
			inputParams.M_Feier = FeiertagAnsatzInUI
			inputParams.M_13 = Lohn13AnsatzInUI
			inputParams.VonDate = dateEditFromDate.EditValue
			inputParams.BisDate = dateEditToDate.EditValue
			inputParams.ESLohnNr = esSalaryData.ESLohnNr
			inputParams.LOSpesenBas = esSalaryData.MAStdSpesen
			inputParams.LOSpesen = esSalaryData.MAStdSpesen > 0
			inputParams.StdTotal = BetragValueInUIRoundedTo5Rappen
			inputParams.FeierTotal = FeiertagBetragInUI
			inputParams.FerTotal = FerienBetragInUI
			inputParams.Total13 = Lohn13BetragInUI
			inputParams.FerBas = FerienBasisInUI
			inputParams.Basis13 = Lohn13BasisInUI
			inputParams.MATSpesenBas = esSalaryData.MATSpesen
			inputParams.MATSpesen = esSalaryData.MATSpesen > 0
			inputParams.KstNr = customerKST.RecordNumber
			inputParams.KstBez = customerKST.Description
			inputParams.KompStd = FlexibleTimeHoursInUI
			inputParams.KompBetrag = FlexibleTimeBetragInUI
			inputParams.UserName = m_InitializationData.UserData.UserFullName
			inputParams.IsPVL = esSalaryData.IsPVL
			inputParams.RPZusatzText = lueAdditionalInfo.EditValue

			inputParams.TimeTable = m_TimeTableDataToSave

			' Update employee RPL.
			Dim success = m_RPLCreateService.UpdateEmployeeRPLData(inputParams)

			LoadEmployeeAndCustomerEmployeeRPLList()
			FocusEmployeeRPLData(inputParams.RPLNr)
			m_UCMediator.LoadDayTotalDataOfRPLType(DatabaseAccess.Report.RPLType.Employee)

			Dim rplData = SelectedEmployeeRPLData
			LoadRPLToUI(rplData)

			Return success
		End Function

		''' <summary>
		''' Upadates customer RPL data.
		''' </summary>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function UpdateCustomerRPLData() As Boolean
			Dim rpData = m_UCMediator.ActiveReportData.ReportData
			Dim customerKST = SelectedCustomerKST
			Dim la = SelectedLA
			Dim esSalaryData = m_UCMediator.SelectedESSalaryData
			Dim selectedRPLData = m_SelectedRPLListData

			If Not CanSelectedRPLBeChanged Then
				Return False
			End If

			If la Is Nothing Or
				customerKST Is Nothing Or
				esSalaryData Is Nothing Or
				selectedRPLData Is Nothing Then
				Return False
			End If

			Dim inputParams As New UpdateCustomerRPLParams

			inputParams.RPNr = rpData.RPNR
			inputParams.RPMonat = rpData.Monat
			inputParams.RPJahr = rpData.Jahr
			inputParams.RPGAVNr = rpData.RPGAV_Nr
			inputParams.RPGAV_StdWeek = rpData.RPGAV_StdWeek
			inputParams.RPLNr = selectedRPLData.RPLNr
			inputParams.LANr = la.LANr
			inputParams.K_Anzahl = AnzahlValueInUI
			inputParams.K_Basis = SignedBasisValueInUI
			inputParams.K_Ansatz = AnsatzValueInUI
			inputParams.MWST = MwStInUI
			inputParams.VonDate = dateEditFromDate.EditValue
			inputParams.BisDate = dateEditToDate.EditValue
			inputParams.ESLohnNr = esSalaryData.ESLohnNr
			inputParams.KDBetrag = BetragValueInUIRoundedTo5Rappen
			inputParams.MATSpesenBas = esSalaryData.MATSpesen
			inputParams.MATSpesen = esSalaryData.MATSpesen > 0
			inputParams.KstNr = customerKST.RecordNumber
			inputParams.KstBez = customerKST.Description
			inputParams.UserName = m_InitializationData.UserData.UserFullName
			inputParams.RPZusatzText = lueAdditionalInfo.EditValue

			inputParams.TimeTable = m_TimeTableDataToSave

			' Update customer RPL
			Dim success = m_RPLCreateService.UpdateCustomerRPLData(inputParams)

			LoadEmployeeAndCustomerEmployeeRPLList()
			FocusCustomerRPLData(inputParams.RPLNr)
			m_UCMediator.LoadDayTotalDataOfRPLType(DatabaseAccess.Report.RPLType.Customer)

			Dim rplData = SelectedCustomerRPLData
			LoadRPLToUI(rplData)

			Return success
		End Function

		Private Sub PrintDataMatrixCode(ByVal inputParams As RPLListData)

			If Not m_printDatamatrixLabel Then Return

			If String.IsNullOrWhiteSpace(m_DatamatrixprinterName) Then
				Dim msg = "Achtung: Sie haben keinen Drucker definiert. Der Ausdruck wird auf Standarddrucker gesendet.{0}"
				msg &= "Sie können in der Mandantenverwaltung -> Einstellungen -> Standard-Werte -> Rapportverwaltung einen Standarddrucker definieren."
				msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine)

				m_UtilityUI.ShowInfoDialog(msg)

				Return
			End If

			' (fstr$(rpnr, "########", 1) + "_" + fstr$(woche, "##", 1) + "_" + Jahr + "_" + fstr$(Monat, "##", 1) + "_" + day$(date(@ufirstday))+ "_" + day$(date(@ulastday))
			' DATAMATRIX_VALUE_PATTERN_REPORT As String = "^RP_(?<RecordNo>\d+)_(?<Year>\d+)_(?<Month>\d+)_(?<Week>\d+)_(?<FirstRPDay>\d+)_(?<LastRPDay>\d+)$"
			Dim dataMatrixCode As String = GetDataMaxtrixCodeString()
			If String.IsNullOrWhiteSpace(dataMatrixCode) OrElse dataMatrixCode.Length < 10 Then dataMatrixCode = "RP_{0}_{2}_{3}_{1}_{4:dd}_{5:dd}_{6}"
			Dim barcodeText = String.Format(dataMatrixCode, inputParams.RPNr, lueWeek.EditValue, Year(inputParams.VonDate), Month(inputParams.VonDate), inputParams.VonDate, inputParams.BisDate, inputParams.ID)
			Dim printResult = SP.Infrastructure.BarcodeUtility.PrintBarcode(barcodeText, m_DatamatrixprinterName)

			m_Logger.LogDebug(String.Format("barcodeText: {0} | printerName: {1} | printResult: {2}", barcodeText, m_DatamatrixprinterName, printResult))
			If m_InitializationData.UserData.UserNr = 1 AndAlso (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) Then SP.Infrastructure.BarcodeUtility.ShowBarcodePreviewDialog(barcodeText)

		End Sub

		Private Sub ShowAdvancePaymentDetailFrom()

			Dim rpData = m_UCMediator.ActiveReportData.ReportData

			Dim yearToLoad As Integer = m_UCMediator.ActiveReportData.ReportData.Jahr
			Dim monthToLoad As Integer = m_UCMediator.ActiveReportData.ReportData.Monat
			Dim lonr As Integer? = m_UCMediator.ActiveReportData.ReportData.LONr
			If lonr.HasValue AndAlso lonr > 0 Then
				Dim msg As String = m_Translate.GetSafeTranslationValue("Für diesen Rapport können Sie keinen Vorschuss erfassen, da Sie bereits die Lohnabrechnung erstellt haben!")
				m_UtilityUI.ShowInfoDialog(msg)

				Return
			End If
			Dim preselection = New SP.MA.AdvancePaymentMng.PreselectionData With {.MDNr = m_InitializationData.MDData.MDNr,
																																	.EmployeeNumber = rpData.EmployeeNumber,
																																	.Advisor = m_InitializationData.UserData.UserKST,
																																	.RPNr = rpData.RPNR,
																																							.reportyear = yearToLoad,
																																							.reportmonth = monthToLoad
																																 }

			Dim frmAdvancePayment = New SP.MA.AdvancePaymentMng.UI.frmNewAdvancePayment(m_InitializationData, preselection)
			'AddHandler m_AdvancePaymentDetailForm.FormClosed, AddressOf OnAdvancePaymentFormClosed
			frmAdvancePayment.Show()
			frmAdvancePayment.BringToFront()

		End Sub

		''' <summary>
		''' Handles close of advance payment form.
		''' </summary>
		Private Sub OnAdvancePaymentFormClosed(sender As System.Object, e As System.EventArgs)
			'LoadAdvancePaymentList(chkThisYear.Checked)

			'' Andreas fragen!!!
			'If Not m_AdvancePaymentDetailForm Is Nothing AndAlso Not m_AdvancePaymentDetailForm.IsDisposed Then
			'	m_AdvancePaymentDetailForm.Dispose()
			'End If

			'Dim advancePaymentForm = CType(sender, frmNewAdvancePayment)

			'If advancePaymentForm.NewAdvancePaymentNumber.HasValue AndAlso advancePaymentForm.NewAdvancePaymentNumber > 0 Then
			'	FocusAdvancePayment(m_EmployeeNumber, advancePaymentForm.NewAdvancePaymentNumber)
			'End If

		End Sub


#End Region

#Region "Delete data"

		''' <summary>
		''' Deletes the selected employee RPL.
		''' </summary>
		Private Sub DeleteSelectedEmployeeRPLData()
			If m_InitializationData.MDData.ClosedMD = 1 OrElse Not m_UserAllowedToChanged Then Return

			If Not CanSelectedRPLBeChanged Then
				Return
			End If

			Dim selectedEmployeeRPL = SelectedEmployeeRPLData

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																				m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
				Return
			End If

			Dim rpData = m_UCMediator.ActiveReportData.ReportData

			Dim result = m_RPLCreateService.DeleteEmployeeRPLData(selectedEmployeeRPL.RPNr, rpData.Jahr, rpData.Monat,
																														selectedEmployeeRPL.RPLNr, selectedEmployeeRPL.ESLohnNr, rpData.RPGAV_Nr, rpData.RPGAV_StdWeek)

			Select Case result
				Case DeleteMARPLDataResult.ResultCanNotDeleteBecauseMonthIsClosed
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Daten können nicht gelöscht werden, da der Rapportmonat bereits abgeschlossen ist."),
																								 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
				Case DeleteMARPLDataResult.ResultCanNotDeleteBecauseLoNrIsPresent
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Daten können nicht gelöscht werden, da bereits eine Lohnnummer existiert."),
																			 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
				Case DeleteMARPLDataResult.ResultDeleteError
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gelöscht werden."))
				Case DeleteMARPLDataResult.ResultDeleteOk
					LoadEmployeeAndCustomerEmployeeRPLList()
					LoadFirstEmployeeRPLIntoUI()
			End Select

		End Sub

		''' <summary>
		''' Deletes the selected customer RPL.
		''' </summary>
		Private Sub DeleteSelectedCustomerRPLData()
			If m_InitializationData.MDData.ClosedMD = 1 OrElse Not m_UserAllowedToChanged Then Return

			If Not CanSelectedRPLBeChanged Then
				Return
			End If

			Dim selectedCustomerRPL = SelectedCustomerRPLData

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
																				m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
				Return
			End If

			Dim rpData = m_UCMediator.ActiveReportData.ReportData

			Dim result = m_RPLCreateService.DeleteCustomerRPLData(selectedCustomerRPL.RPNr, rpData.Jahr, rpData.Monat, selectedCustomerRPL.RPLNr)

			Select Case result
				Case DeleteKDRPLDataResult.ResultCanNotDeleteBecauseMonthIsClosed
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Daten können nicht gelöscht werden, da der Rapportmonat bereits abgeschlossen ist."),
																								 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
				Case DeleteKDRPLDataResult.ResultCanNotDeleteBecauseOfExistingRe
					m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Daten können nicht gelöscht werden, da bereits eine Rechnung existiert."),
																			 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)
				Case DeleteKDRPLDataResult.ResultDeleteError
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gelöscht werden."))
				Case DeleteKDRPLDataResult.ResultDeleteOk
					LoadEmployeeAndCustomerEmployeeRPLList()
					LoadFirstCustomerRPLIntoUI()
			End Select

		End Sub

		''' <summary>
		''' Clears the flexible time of the selected RPL
		''' </summary>
		Private Sub ClearFlexibleTimeOfRPLSelectedRPL()

			If Not CanSelectedRPLBeChanged Then
				Return
			End If

			If FlexibleTimeHoursInUI = 0D And FlexibleTimeBetragInUI = 0D Then
				Return
			End If

			If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Hiermit löschen Sie die Gleitzeit für die ausgewählte Rapportzeile!") & vbCrLf &
																			m_Translate.GetSafeTranslationValue("Sind Sie sicher?"),
																			m_Translate.GetSafeTranslationValue("Gleitzeit löschen")) = False) Then
				Return
			End If

			Dim success = m_ReportDataAccess.ClearFlexibleTimeOfRPL(m_SelectedRPLListData.RPNr, m_SelectedRPLListData.RPLNr)

			If Not success Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Gleitzeit konnte nicht zurückgesetzt werden."))
			Else
				LoadFlexibleTimeDataOfRPL(m_SelectedRPLListData.RPNr, m_SelectedRPLListData.RPLNr)
			End If

		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Handles click on grdCustomerRPL.
		''' </summary>
		Private Sub OnGrdCustomerRPL_Click(sender As System.Object, e As System.EventArgs) Handles grdCustomerRPL.Click
			LoadSelectedCustomerRPL()
		End Sub

		''' <summary>
		''' Handles key up on grdCustomerRPL.
		''' </summary>
		Private Sub OnGrdCustomerRPL_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles grdCustomerRPL.KeyUp

			If e.KeyCode = Keys.Delete Then
				' Do not reload on delete key
				Return

			ElseIf e.KeyCode = Keys.Insert Then
				OnBtnAddNewCustomerRPL_Click(sender, New System.EventArgs)
				Return

			End If

			LoadSelectedCustomerRPL()
		End Sub

		''' <summary>
		''' Handles click on grdEmployeeRPL.
		''' </summary>
		Private Sub OnGrdEmployeeRPL_Click(sender As System.Object, e As System.EventArgs) Handles grdEmployeeRPL.Click
			LoadSelectedEmployeeRPL()
		End Sub

		''' <summary>
		''' Handles key up on grdEmployeeRPL.
		''' </summary>
		Private Sub OnGrdEmployeeRPL_KeyUp(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles grdEmployeeRPL.KeyUp

			If e.KeyCode = Keys.Delete Then
				' Do not reload on delete key
				Return

			ElseIf e.KeyCode = Keys.Insert Then
				OnBtnAddNewEmployeeRPL_Click(sender, New System.EventArgs)
				Return

			End If

			LoadSelectedEmployeeRPL()
		End Sub

		''' <summary>
		''' Handles enter event of grdEmployeeRPL
		''' </summary>
		Private Sub OnGrdEmployeeRPL_Enter(sender As System.Object, e As System.EventArgs) Handles grdEmployeeRPL.Enter
			SetFocusedAppearanceOnCustomerRPLGrid(False)
			SetFocusedAppearanceOnEmployeeRPLGrid(True)

			m_UCMediator.LoadDayTotalDataOfRPLType(DatabaseAccess.Report.RPLType.Employee)

		End Sub

		''' <summary>
		''' Handles enter event of grdCustomerRPL
		''' </summary>
		Private Sub OnGrdCustomerRPL_Enter(sender As System.Object, e As System.EventArgs) Handles grdCustomerRPL.Enter
			SetFocusedAppearanceOnEmployeeRPLGrid(False)
			SetFocusedAppearanceOnCustomerRPLGrid(True)

			m_UCMediator.LoadDayTotalDataOfRPLType(DatabaseAccess.Report.RPLType.Customer)
		End Sub

		''' <summary>
		''' Handles click on address lookup edit button.
		''' </summary>
		Private Sub OnLueAdresseButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles lueCustomerKST.ButtonClick
			If e.Button.Index = 2 Then

				Dim activeReportData = m_UCMediator.ActiveReportData

				If Not lueCustomerKST.EditValue Is Nothing AndAlso Not activeReportData Is Nothing Then

					If m_InvoiceAddressesForm Is Nothing OrElse m_InvoiceAddressesForm.IsDisposed Then

						If Not m_InvoiceAddressesForm Is Nothing Then
							' First cleanup handlers of old form before new form is created.
							RemoveHandler m_InvoiceAddressesForm.FormClosed, AddressOf OnInvoiceAddressFormClosed
							RemoveHandler m_InvoiceAddressesForm.InvoiceKSTDataSaved, AddressOf OnInvoiceKSTFormKSTDataSaved
							RemoveHandler m_InvoiceAddressesForm.InvoiceKSTDataDeleted, AddressOf OnInvoiceKSTFormKSTDataDeleted
						End If

						m_InvoiceAddressesForm = New frmInvoiceAddress(m_InitializationData)
						AddHandler m_InvoiceAddressesForm.FormClosed, AddressOf OnInvoiceAddressFormClosed
						AddHandler m_InvoiceAddressesForm.InvoiceKSTDataSaved, AddressOf OnInvoiceKSTFormKSTDataSaved
						AddHandler m_InvoiceAddressesForm.InvoiceKSTDataDeleted, AddressOf OnInvoiceKSTFormKSTDataDeleted
					End If

					m_InvoiceAddressesForm.Show()
					m_InvoiceAddressesForm.LoadCustomerInvoiceAddresses(activeReportData.CustomerOfActiveReport.CustomerNumber, Nothing)
					m_InvoiceAddressesForm.BringToFront()

				End If

			End If
		End Sub

		''' <summary>
		''' Handles close of invoice address form.
		''' </summary>
		Private Sub OnInvoiceAddressFormClosed(sender As System.Object, e As System.EventArgs)

			LoadCustomerKSTDropdowndata(m_UCMediator.ActiveReportData.CustomerOfActiveReport.CustomerNumber)

		End Sub

		''' <summary>
		''' Handles invoice save event of invoice form.
		''' </summary>
		Private Sub OnInvoiceKSTFormKSTDataSaved(ByVal sender As Object, ByVal customerNumber As Integer)

			LoadCustomerKSTDropdowndata(customerNumber)

		End Sub

		''' <summary>
		''' Handles invoice delete event of invoice form.
		''' </summary>
		Private Sub OnInvoiceKSTFormKSTDataDeleted(ByVal sender As Object, ByVal customerNumber As Integer)

			LoadCustomerKSTDropdowndata(customerNumber)

		End Sub


		''' <summary>
		''' Handles change of lueLAData (Lohnart).
		''' </summary>
		Private Sub OnLueLAData_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueLAData.EditValueChanged

			If AreUIEventsSuppressed Then
				Return
			End If

			lblNegSign.Visible = (BasisFactor < 0.0)

			SetLAValuesBasesOnSelectedLA()

			If m_ActiveRPLType = RPLType.Employee Then
				SetAdditionalFeeValuesBasesOnInputData()
			End If

		End Sub

		''' <summary>
		''' Sets the additional info.
		''' </summary>
		''' <param name="additionalInfo">The additional info.</param>
		Private Sub SetAdditionalInfo(ByVal additionalInfo As String)

			If Not String.IsNullOrWhiteSpace(additionalInfo) And Not lueAdditionalInfo.Properties.DataSource Is Nothing Then
				Dim additionalInfoTexts = CType(lueAdditionalInfo.Properties.DataSource, List(Of RPLAdditionalTextData))

				If Not additionalInfoTexts.Any(Function(data) data.AdditionalText = additionalInfo) Then
					Dim newAdditionalInfoText As New RPLAdditionalTextData With {.AdditionalText = additionalInfo}
					additionalInfoTexts.Add(newAdditionalInfoText)
				End If

			End If

			lueAdditionalInfo.EditValue = additionalInfo
		End Sub

		''' <summary>
		''' Handles new value event on additionalinfo (Zusatztext) lookup edit.
		''' </summary>
		Private Sub OnLueAdditionalInfo_ProcessNewValue(sender As System.Object, e As DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs) Handles lueAdditionalInfo.ProcessNewValue

			If Not lueAdditionalInfo.Properties.DataSource Is Nothing Then

				Dim additionalInfoTexts = CType(lueAdditionalInfo.Properties.DataSource, List(Of RPLAdditionalTextData))

				Dim newAdditionalInfoText As New RPLAdditionalTextData With {.AdditionalText = e.DisplayValue.ToString()}
				additionalInfoTexts.Add(newAdditionalInfoText)

				e.Handled = True
			End If
		End Sub


		''' <summary>
		''' Handles edit value changing event of txtAnzahl, txtBasis and txtAnsatz.
		''' </summary>
		Private Sub OnLAValue_EditValueChanging(sender As System.Object, e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles txtAnzahl.EditValueChanging, txtBasis.EditValueChanging, txtAnsatz.EditValueChanging

			If AreUIEventsSuppressed Then
				Return
			End If
			If sender Is txtAnzahl Then
				e.Cancel = Not m_CurrentLAValueUpperBounds.IsAnzahlValueInBoundary(Convert.ToDecimal(e.NewValue))
			ElseIf sender Is txtBasis Then
				e.Cancel = Not m_CurrentLAValueUpperBounds.IsBasisValueInBoundary(Convert.ToDecimal(e.NewValue) * BasisFactor)
			ElseIf sender Is txtAnsatz Then
				e.Cancel = Not m_CurrentLAValueUpperBounds.IsAnsatzValueInBoundary(Convert.ToDecimal(e.NewValue))
			End If

		End Sub

		''' <summary>
		''' Handles change of txtAnzahl, txtBasis and txtAnsatz EditValue change.
		''' </summary>
		Private Sub OnLAValue_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles txtAnzahl.EditValueChanged, txtBasis.EditValueChanged, txtAnsatz.EditValueChanged

			If AreUIEventsSuppressed = True Then
				Return
			End If

			HandleChangeOfAnzahlBasisOrAnsatzValue()
		End Sub

		''' <summary>
		''' Handles click on hour input button.
		''' </summary>
		Private Sub OnBtnOpenHourInput_Click(sender As System.Object, e As System.EventArgs) Handles btnOpenHourInput.Click
			If m_InitializationData.MDData.ClosedMD = 1 Then Return

			Dim isDataValidForHourDataInput As Boolean = True
			Dim maxWorkingHoursPerWorkingDay As Decimal = DetermineMaximalWorkingHoursPerWorkingDay()

			Try
				IgnoreHourlyDataCheckInValidation = True
				isDataValidForHourDataInput = ValidateData()
			Finally
				IgnoreHourlyDataCheckInValidation = False
			End Try

			If Not isDataValidForHourDataInput Then
				Return
			End If
			m_PrintNoLabelbyzeroHour = PrintNoLabelByZeroHour
			Dim frmHourInput As New frmHourInput(m_InitializationData)

			Dim fromDate = dateEditFromDate.EditValue
			Dim toDate = dateEditToDate.EditValue
			Dim rpNr As Integer

			If Not m_SelectedRPLListData Is Nothing Then

				rpNr = m_SelectedRPLListData.RPNr
				Dim rplNr As Integer = m_SelectedRPLListData.RPLNr
				Dim type = m_SelectedRPLListData.Type

				Dim exists As Boolean? = m_ReportDataAccess.ExistsRPLDayDataForRPL(rpNr, rplNr, type)

				If exists Then
					frmHourInput.LoadData(rpNr, rplNr, type, fromDate, toDate)
					frmHourInput.IsReadonly = (Not btnSave.Enabled) AndAlso Not m_UserAllowedToChanged
					frmHourInput.IsReadonlyAbsenceCode = (Not m_UCMediator.ActiveReportData.ReportData.LONr = 0)

					frmHourInput.ShowDialog()
				End If

			Else
				rpNr = m_UCMediator.ActiveReportData.ReportData.RPNR
				frmHourInput.NewInput(rpNr, fromDate, toDate)
				frmHourInput.IsReadonly = (Not btnSave.Enabled) AndAlso Not m_UserAllowedToChanged

				Dim showHoursInNormalOption = m_UCMediator.ActiveReportData.CustomerOfActiveReport.ShowHoursInNormal
				If (showHoursInNormalOption.HasValue AndAlso showHoursInNormalOption.Value) Then
					frmHourInput.IsShowAsNormalHoursChecked = True
				End If

				If m_ActiveRPLType = RPLType.Employee Then ' AndAlso IsFlexibleTimeActive Then
					' Show maximal working hours per day.
					frmHourInput.MaximalWorkingHoursPerDayInfo = maxWorkingHoursPerWorkingDay
				End If

				frmHourInput.IsReadonlyAbsenceCode = (Not m_UCMediator.ActiveReportData.ReportData.LONr = 0)
				frmHourInput.ShowDialog()
			End If

			If frmHourInput.IsReadonly OrElse Not m_UserAllowedToChanged Then
				' Do nothing if form is read only
				Return
			End If

			If Not frmHourInput.DialogResult = DialogResult.OK OrElse Not m_UserAllowedToChanged Then
				' Do nothing if save was not pressed.
				Return
			End If

			Dim printLabel As Boolean = frmHourInput.chkAutoPrinBarcode.Checked
			If m_PrintNoLabelbyzeroHour.GetValueOrDefault(False) Then
				If frmHourInput.TimeTable.SumHourData = 0 Then printLabel = False
			End If
			m_printDatamatrixLabel = printLabel

			' Remember time table data to save.
			m_TimeTableDataToSave = frmHourInput.TimeTable

			If m_SelectedRPLListData Is Nothing AndAlso
				m_ActiveRPLType = RPLType.Employee AndAlso
				IsFlexibleTimeActive Then

				Dim workingHoursAndFlexibleTime = m_TimeTableDataToSave.GetRegularWorkingHoursAndFlexibleTime(maxWorkingHoursPerWorkingDay)

				' Working hours withouth flexible time
				AnzahlValueInUI = workingHoursAndFlexibleTime.SumRegularWorkingHours
			Else
				AnzahlValueInUI = m_TimeTableDataToSave.SumHourData
			End If

			SaveRPLData()

			m_printDatamatrixLabel = False

		End Sub

		''' <summary>
		''' Determines the maximal working hours per working day.
		''' </summary>
		''' <returns>Maximal working hours for working day.</returns>
		Private Function DetermineMaximalWorkingHoursPerWorkingDay() As Decimal

			Dim value As Decimal = Decimal.MaxValue ' No flexible time

			' Flexible time only for new entries.
			If m_SelectedRPLListData Is Nothing Then

				Dim rp = m_UCMediator.ActiveReportData.ReportData
				Dim flexibleTimeHelper As New FlexibleTimeHelper(rp.MDNr, m_ReportDataAccess)
				value = flexibleTimeHelper.DetermineMaximalWorkingHoursPerWorkingDay(rp.RPGAV_Nr, rp.RPGAV_StdWeek, Convert.ToInt16(rp.Jahr))

			Else
				' no flexible time
			End If

			Return value

		End Function

		''' <summary>
		''' Handles click on new  employee RPL button.
		''' </summary>
		Private Sub OnBtnAddNewEmployeeRPL_Click(sender As System.Object, e As System.EventArgs) Handles btnAddNewEmployeeRPL.Click
			If Not btnAddNewEmployeeRPL.Enabled Then Exit Sub
			PrepareForNewRPL(RPLType.Employee)
			lueWeek.Focus()
		End Sub

		''' <summary>
		''' Handles click on new customer RPL button.
		''' </summary>
		Private Sub OnBtnAddNewCustomerRPL_Click(sender As System.Object, e As System.EventArgs) Handles btnAddNewCustomerRPL.Click
			If Not btnAddNewCustomerRPL.Enabled Then Exit Sub
			PrepareForNewRPL(RPLType.Customer)
			lueWeek.Focus()
		End Sub

		''' <summary>
		''' Handles click on save button.
		''' </summary>
		Private Sub OnBtnSave_Click(sender As System.Object, e As System.EventArgs) Handles btnSave.Click
			SaveRPLData()
		End Sub

		''' <summary>
		''' Handles change of lueWeek.
		''' </summary>
		Private Sub OnLueWeek_EditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueWeek.EditValueChanged
			If AreUIEventsSuppressed Then
				Return
			End If

			SetFromAndToDateBasedOnActiveCalendarWeek()
		End Sub

		''' <summary>
		''' Handles key down on grdEmployeeRPL.
		''' </summary>
		Private Sub OnGrdEmployeeRPL_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles grdEmployeeRPL.KeyDown
			If (e.KeyCode = Keys.Delete) Then
				DeleteSelectedEmployeeRPLData()
			End If
		End Sub

		''' <summary>
		''' Handles key down on grdEmployeeRPL.
		''' </summary>
		Private Sub OnGrdCustomerRPL_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles grdCustomerRPL.KeyDown
			If (e.KeyCode = Keys.Delete) Then
				DeleteSelectedCustomerRPLData()
			End If
		End Sub

		''' <summary>
		''' Handles key down on grdFlexibletime.
		''' </summary>
		Private Sub OnGrdFlexibletime_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles grdFlexibletime.KeyDown
			If (e.KeyCode = Keys.Delete) Then
				ClearFlexibleTimeOfRPLSelectedRPL()
			End If
		End Sub

		''' <summary>
		''' Handles unbound column data event on gvCustomerRPL.
		''' </summary>
		Private Sub OnGvCustomerRPL_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvCustomerRPL.CustomUnboundColumnData

			If e.Column.Name = "ReNr" Then
				If (e.IsGetData()) Then
					e.Value = (CType(e.Row, RPLListData).RENr > 0)
				End If
			ElseIf e.Column.Name = "RPLDocScan" Then
				If (e.IsGetData()) Then

					Dim rplData = CType(e.Row, RPLListData)

					e.Value = If(rplData.HasDocument, m_PDFIcon, m_EmptyIcon)

				End If
			End If

		End Sub

		''' <summary>
		''' Handles unbound column data event on gvEmployeeRPL.
		''' </summary>
		Private Sub OnGvEmployeeRPL_CustomUnboundColumnData(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs) Handles gvEmployeeRPL.CustomUnboundColumnData

			If e.Column.Name = "RPLDocScan" Then
				If (e.IsGetData()) Then

					Dim rplData = CType(e.Row, RPLListData)

					e.Value = If(rplData.HasDocument, m_PDFIcon, m_EmptyIcon)

				End If
			End If
		End Sub

		''' <summary>
		''' Handles cell click on gvEmployeeRPL.
		''' </summary>
		Private Sub OnGvEmployeeRPL_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvEmployeeRPL.RowCellClick

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvCustomerRPL.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then
					Dim rplData = CType(dataRow, RPLListData)

					Select Case column.Name
						Case "RPLDocScan"
							If rplData.HasDocument Then
								OpenPDFOfSelectedRPL()
							End If
					End Select

				End If

			End If

		End Sub

		''' <summary>
		''' Handles cell click on gvCustomerRPL.
		''' </summary>
		Private Sub OnGvCustomerRPL_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvCustomerRPL.RowCellClick

			If (e.Clicks = 2) Then

				Dim column = e.Column
				Dim dataRow = gvCustomerRPL.GetRow(e.RowHandle)
				If Not dataRow Is Nothing Then
					Dim rplData = CType(dataRow, RPLListData)

					Select Case column.Name
						Case "ReNr", "RENr"
							Dim reNr = rplData.RENr

							' Send a request to open a invoiceMng form.
							Dim hub = MessageService.Instance.Hub
							Dim openInvoiceMng As New OpenInvoiceMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, reNr)
							hub.Publish(openInvoiceMng)

						Case "RPLDocScan"
							If rplData.HasDocument Then
								OpenPDFOfSelectedRPL()
							End If
					End Select

				End If

			End If

		End Sub

		''' <summary>
		''' Handles row click of employee salary type data row.
		''' </summary>
		Private Sub OnGvEmployeeSalaryTypeData_RowClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvEmployeeSalaryTypeData.RowCellClick

			If m_SuppressUIEvents Then
				Return
			End If

			If Not btnAddNewEmployeeRPL.Enabled Then
				Return
			End If

			If (e.Clicks = 2) Then

				Dim selectedESMALA = SelectedESMALAData

				If Not selectedESMALA Is Nothing Then
					PrepareForNewRPL(RPLType.Employee)

					Dim year As Integer = DateTime.Now.Year

					If Not m_UCMediator.ActiveReportData Is Nothing Then
						year = Convert.ToInt32(m_UCMediator.ActiveReportData.ReportData.Jahr)
					End If

					SelectLA(selectedESMALA.LANr, year, RPLType.Employee)

					BasisValueInUI = selectedESMALA.Basis
					AnsatzValueInUI = selectedESMALA.Ansatz

					lueAdditionalInfo.Focus()

				End If
			End If

		End Sub

		''' <summary>
		''' Handles row style event of employee salary type data row.
		''' </summary>
		Private Sub OnGvEmployeeSalaryTaypeData_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvEmployeeSalaryTypeData.RowStyle
			If (e.RowHandle >= 0) Then
				Dim view As GridView = CType(sender, GridView)
				Dim rowData = CType(view.GetRow(e.RowHandle), DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData)

				If (rowData.ESLohnNr = 0) Then
					e.Appearance.BackColor = Color.Yellow
				End If

			End If

		End Sub

		''' <summary>
		''' Handles row click of customer salary type data row.
		''' </summary>
		Private Sub OnGvCustomerSalaryTypeData_RowClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvCustomerSalaryTypeData.RowCellClick

			If m_SuppressUIEvents Then
				Return
			End If

			If Not btnAddNewCustomerRPL.Enabled Then
				Return
			End If

			If (e.Clicks = 2) Then

				Dim selectedESKDLA = SelectedESKDLAData

				If Not selectedESKDLA Is Nothing Then
					PrepareForNewRPL(RPLType.Customer)

					Dim year As Integer = DateTime.Now.Year

					If Not m_UCMediator.ActiveReportData Is Nothing Then
						year = Convert.ToInt32(m_UCMediator.ActiveReportData.ReportData.Jahr)
					End If

					SelectLA(selectedESKDLA.LANr, year, RPLType.Customer)

					BasisValueInUI = selectedESKDLA.Basis
					AnsatzValueInUI = selectedESKDLA.Ansatz

				End If

			End If

		End Sub

		''' <summary>
		''' Handles row style event of customer salary type data row.
		''' </summary>
		Private Sub OnGvCustomerSalaryTaypeData_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvCustomerSalaryTypeData.RowStyle
			If (e.RowHandle >= 0) Then
				Dim view As GridView = CType(sender, GridView)
				Dim rowData = CType(view.GetRow(e.RowHandle), DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData)

				If (rowData.ESLohnNr = 0) Then
					e.Appearance.BackColor = Color.Yellow
				End If

			End If

		End Sub

		''' <summary>
		''' Handles click on open PDF button.
		''' </summary>
		Private Sub OnBtnOpenPDF_Click(sender As System.Object, e As System.EventArgs) Handles btnOpenPDF.Click
			OpenPDFOfSelectedRPL()
		End Sub

		Private Sub OnbtnPrintDatamatrix_Click(sender As System.Object, e As System.EventArgs) Handles btnPrintDatamatrix.Click

			If m_ActiveRPLType = RPLType.Customer Then
				m_printDatamatrixLabel = True
				Dim rplData = SelectedCustomerRPLData
				PrintDataMatrixCode(rplData)
				m_printDatamatrixLabel = False
			End If

		End Sub

		''' <summary>
		'''  Handles RowStyle event of gvLueLAData grid view.
		''' </summary>
		Private Sub OnGvLueLAData_RowStyle(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs) Handles gvLueLAData.RowStyle

			If e.RowHandle >= 0 Then

				Dim rowData = CType(gvLueLAData.GetRow(e.RowHandle), LAData)

				If rowData.Highlight Then
					e.Appearance.BackColor = Color.Yellow
					e.Appearance.BackColor2 = Color.Yellow
				End If

			End If

		End Sub

#End Region

#Region "Calculation & Logic Helpers"

		''' <summary>
		''' Recalculates the amount.
		''' </summary>
		Private Sub RecalculateAmount()

			Dim count As Decimal = AnzahlValueInUI
			Dim bas As Decimal = SignedBasisValueInUI
			Dim ans As Decimal = AnsatzValueInUI / 100.0

			Dim la = SelectedLA
			Dim rounding As Short = 2

			If Not la Is Nothing AndAlso la.Rounding.HasValue Then
				rounding = la.Rounding
			End If


			If rounding > 2 Then
				BetragValueInUI = (count * bas * ans)
			Else
				BetragValueInUI = m_Utility.SwissCommercialRound(count * Math.Round(bas, 2) * ans)
			End If


		End Sub

		''' <summary>
		''' Sets the LA  valeus bases on the selected la.
		''' </summary>
		Private Sub SetLAValuesBasesOnSelectedLA()

			Dim rpData = m_UCMediator.ActiveReportData.ReportData
			Dim la = SelectedLA
			Dim esSalaryData = m_UCMediator.SelectedESSalaryData

			If la Is Nothing Or esSalaryData Is Nothing Then
				Return
			End If

			Dim esParameters As New RPLSalaryTypeValuesCalcESParams(rpData.ESNR, esSalaryData.Tarif, esSalaryData.GrundLohn, esSalaryData.MWStBetrag)
			esParameters.LohnFeierProz = esSalaryData.FeierProz
			esParameters.LohnFerienProz = esSalaryData.FerienProz
			esParameters.Lohn13Proz = esSalaryData.Lohn13Proz
			esParameters.Stundenlohn = esSalaryData.StundenLohn
			esParameters.ESLohnIsMwStPflichtig = esSalaryData.ESLohnIsMwStPflichtig
			Dim laParameters As New RPLSalaryTypeValuesCalcLAParams(la.LANr, la.TypeAnsatz, la.TypeBasis, la.FixAnsatz, la.MABasVar, la.KDBasis, la.MWSTPflichtig)

			Dim calculationParameters As New RPLSalaryTypeValuesCalcParams(rpData.EmployeeNumber, DefaultMwStValue(rpData.Jahr), esParameters, laParameters)

			Dim result As RPLSalaryTypeValuesCalcuResult = Nothing
			Select Case m_ActiveRPLType
				Case DatabaseAccess.Report.RPLType.Employee
					result = m_RPLValueCalculator.CalculateSalaryTypeValuesForEmployee(calculationParameters)

				Case (DatabaseAccess.Report.RPLType.Customer)
					result = m_RPLValueCalculator.CalculateSalaryTypeValuesForCustomer(calculationParameters)
				Case Else
					' This case should never happen
					result = New RPLSalaryTypeValuesCalcuResult
			End Select

			Dim presviousSuppressState = SetSuppressUIEventsState(True)
			AnzahlValueInUI = If(result.Anzahl.HasValue, result.Anzahl, Anzahl_Default_Value)

			If Not TakeBasisAndAnsatzFromESAdditionalLADataIfExists(la.LANr) Then
				' no additional data in ES -> take normal data form Lohnartenstamm.
				BasisValueInUI = If(result.Basis.HasValue, result.Basis, Basis_Default_Value)
				AnsatzValueInUI = If(result.Ansatz.HasValue, result.Ansatz, Ansatz_Default_Value)
			End If

			'MwStInUI = If(result.MwSt.HasValue, result.MwSt, MwSt_Default_Value)
			MwStInUI = If(esSalaryData.ESLohnIsMwStPflichtig, result.MwSt.GetValueOrDefault(0), MwSt_Default_Value)
			SetSuppressUIEventsState(presviousSuppressState)

			HandleChangeOfAnzahlBasisOrAnsatzValue()

			RememberLAUpperBoundValuesBasedOnCurrentLAValues()

			If Not la Is Nothing Then
				btnOpenHourInput.Visible = la.ProTag.Value
				txtAnzahl.Properties.ReadOnly = la.ProTag.Value
			Else
				btnOpenHourInput.Visible = False
				txtAnzahl.Properties.ReadOnly = False
			End If

		End Sub

		''' <summary>
		''' Taskes Basis and Ansatz value from ES additional LA data if exists.
		''' </summary>
		''' <param name="laNr">The LA number to search for.</param>
		Private Function TakeBasisAndAnsatzFromESAdditionalLADataIfExists(ByVal laNr As Decimal) As Boolean

			Dim additionalLADataFromES As SP.DatabaseAccess.ES.DataObjects.ESMng.ESEmployeeAndCustomerLAData = Nothing

			Select Case m_ActiveRPLType
				Case RPLType.Customer

					If (DoesAdditionalCustomerESSalaryTypeDataExistsForLANr(laNr)) Then
						additionalLADataFromES = m_AdditionalCustomerESSalaryTypeData.Where(Function(data) data.LANr = laNr).FirstOrDefault()
					End If

				Case RPLType.Employee

					If (DoesAdditionalEmployeeESSalaryTypeDataExistsForLANr(laNr)) Then
						additionalLADataFromES = m_AdditionalEmployeeESSalaryTypeData.Where(Function(data) data.LANr = laNr).FirstOrDefault()
					End If

				Case Else
					' should not happen.
			End Select

			If Not additionalLADataFromES Is Nothing Then

				BasisValueInUI = additionalLADataFromES.Basis
				AnsatzValueInUI = additionalLADataFromES.Ansatz
				Return True
			End If

			Return False

		End Function

		''' <summary>
		''' Handles change of Anzahl, Basis or Ansatz value.
		''' </summary>
		Private Sub HandleChangeOfAnzahlBasisOrAnsatzValue()

			RecalculateAmount()

			If m_ActiveRPLType = RPLType.Employee Then
				SetAdditionalFeeValuesBasesOnInputData()
			End If

		End Sub

		''' <summary>
		''' Sets the LA upper bound values based on current LA values.
		''' </summary>
		Private Sub RememberLAUpperBoundValuesBasedOnCurrentLAValues()

			Dim la = SelectedLA

			m_CurrentLAValueUpperBounds.Reset()
			txtAnzahl.ToolTip = String.Empty
			txtBasis.ToolTip = String.Empty
			txtAnsatz.ToolTip = String.Empty

			If la Is Nothing Then
				Return
			End If

			' Remember upper bounds
			Dim MIN_VALUE_STR As String = m_Translate.GetSafeTranslationValue("Minimaler")
			Dim MAX_VALUE_STR As String = m_Translate.GetSafeTranslationValue("Maximaler")
			Dim Wert_STR As String = m_Translate.GetSafeTranslationValue("Wert")

			If Not la.AllowMoreAnzahl AndAlso la.TypeAnzahl.HasValue AndAlso la.TypeAnzahl = 2 Then
				m_CurrentLAValueUpperBounds.AnzahlBoundaryValue = AnzahlValueInUI
				txtAnzahl.ToolTip = String.Format("{0} {1}: {2}", If(AnzahlValueInUI < 0, MIN_VALUE_STR, MAX_VALUE_STR), Wert_STR, AnzahlValueInUI)
			End If

			If Not la.AllowMoreBasis AndAlso la.TypeBasis.HasValue AndAlso la.TypeBasis = 2 Then
				m_CurrentLAValueUpperBounds.BasisBoundaryValue = SignedBasisValueInUI
				txtBasis.ToolTip = String.Format("{0} {1}: {2}", If(SignedBasisValueInUI < 0, MIN_VALUE_STR, MAX_VALUE_STR), Wert_STR, SignedBasisValueInUI)
			End If

			If Not la.AllowMoreAnsatz AndAlso la.TypeAnsatz.HasValue AndAlso la.TypeAnsatz = 2 Then
				m_CurrentLAValueUpperBounds.AnsatzBoundaryValue = AnsatzValueInUI
				txtAnsatz.ToolTip = String.Format("{0} {1}: {2}", If(AnsatzValueInUI < 0, MIN_VALUE_STR, MAX_VALUE_STR), Wert_STR, AnsatzValueInUI)
			End If

		End Sub

		''' <summary>
		''' Sets additional fee values base on input data.
		''' </summary>
		Private Sub SetAdditionalFeeValuesBasesOnInputData()

			Dim rpData = m_UCMediator.ActiveReportData.ReportData
			Dim la = SelectedLA
			Dim esSalaryData = m_UCMediator.SelectedESSalaryData

			If la Is Nothing Or esSalaryData Is Nothing Then
				m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Lohnartenwerte konnten nicht gesetz werden."))
				Return
			End If

			' ES parameters.
			'Dim esParameters As New RPLAdditionalFeesValuesCalcESParams(If(la.MoreProz4Feier.Value, ((AnsatzValueInUI / 100D) * esSalaryData.FeierProz), esSalaryData.FeierProz),
			'																														If(la.MoreProz4Fer.Value, ((AnsatzValueInUI / 100D) * esSalaryData.FerienProz), esSalaryData.FerienProz),
			'																														If(la.MoreProz413.Value, ((AnsatzValueInUI / 100D) * esSalaryData.Lohn13Proz), esSalaryData.Lohn13Proz),
			'																														esSalaryData.LOFeiertagWay,
			'																														esSalaryData.FerienWay,
			'																														esSalaryData.LO13Way,
			'																														esSalaryData.HasGavData)
			Dim proz4Feier As Decimal = 0D
			Dim proz4Fer As Decimal = 0D
			Dim proz413 As Decimal = 0D

			' Grundlohn value parameter for base value calculation
			Dim grundlohnValueForBaseValueCalculation As Decimal = (SignedBasisValueInUI)

			If la.MoreProz4Feier.Value OrElse la.MoreProz4FeierAmount.GetValueOrDefault(0) > 0D Then
				If la.MoreProz4FeierAmount.GetValueOrDefault(0) > 0D Then
					proz4Feier = ((la.MoreProz4FeierAmount.GetValueOrDefault(0) / 100D) * esSalaryData.FeierProz)
				Else
					proz4Feier = ((AnsatzValueInUI / 100D) * esSalaryData.FeierProz)
					proz4Feier = esSalaryData.FeierProz
				End If
				grundlohnValueForBaseValueCalculation = ((AnsatzValueInUI / 100D) * SignedBasisValueInUI)
			Else
				proz4Feier = esSalaryData.FeierProz
			End If

			If la.MoreProz4Fer.Value OrElse la.MoreProz4FerAmount.GetValueOrDefault(0) > 0D Then
				If la.MoreProz4FerAmount.GetValueOrDefault(0) > 0D Then
					proz4Fer = ((la.MoreProz4FerAmount.GetValueOrDefault(0) / 100D) * esSalaryData.FerienProz)
				Else
					proz4Fer = ((AnsatzValueInUI / 100D) * esSalaryData.FerienProz)
					proz4Fer = esSalaryData.FerienProz
				End If
				grundlohnValueForBaseValueCalculation = ((AnsatzValueInUI / 100D) * SignedBasisValueInUI)
			Else
				proz4Fer = esSalaryData.FerienProz
			End If

			If la.MoreProz413.Value OrElse la.MoreProz413Amount.GetValueOrDefault(0) > 0D Then
				If la.MoreProz413Amount.GetValueOrDefault(0) > 0D Then
					proz413 = ((la.MoreProz413Amount.GetValueOrDefault(0) / 100D) * esSalaryData.Lohn13Proz)
				Else
					proz413 = ((AnsatzValueInUI / 100D) * esSalaryData.Lohn13Proz)
					proz413 = esSalaryData.Lohn13Proz
				End If
				grundlohnValueForBaseValueCalculation = ((AnsatzValueInUI / 100D) * SignedBasisValueInUI)
			Else
				proz413 = esSalaryData.Lohn13Proz
			End If

			Dim esParameters As New RPLAdditionalFeesValuesCalcESParams(proz4Feier, proz4Fer, proz413, esSalaryData.LOFeiertagWay, esSalaryData.FerienWay, esSalaryData.LO13Way, esSalaryData.HasGavData)

			' LA parameters.
			Dim laParameters As New RPLAdditionalFeesValuesCalcLAParams(la.FeierInklusiv, la.FerienInklusiv, la.Inklusiv13)


			Dim calculationParameters As New RPLAdditionalFeesValuesCalcParams(grundlohnValueForBaseValueCalculation, esParameters, laParameters)

			Dim result As RPLAdditionalFeesValuesCalcResult = m_RPLValueCalculator.CalculateRPLAdditionalFeeValues(calculationParameters)

			' Create empty rows first
			CreateEmptyAdditonalFeeRows(la.FeierInklusiv, la.FerienInklusiv, la.Inklusiv13)

			Dim anzahlInUI = AnzahlValueInUI
			If la.FeierInklusiv Then
				Dim row = GetAdditonalFeeRowByLANr(LANr_Feiertag)
				row.M_Ansatz = result.FeiertagAnsatz
				row.M_Basis = result.CalcFerFeier13BasisResult.FeiertagBasis
				row.M_Betrag = result.CalcFerFeier13BasisResult.FeiertagBetrag * anzahlInUI
			End If

			If la.FerienInklusiv Then
				Dim row = GetAdditonalFeeRowByLANr(LANR_Ferien)
				row.M_Ansatz = result.FerienAnsatz
				row.M_Basis = result.CalcFerFeier13BasisResult.FerienBasis
				row.M_Betrag = result.CalcFerFeier13BasisResult.FerienBetrag * anzahlInUI
			End If

			If la.Inklusiv13 Then
				Dim row = GetAdditonalFeeRowByLANr(LANr_Lohn13)
				row.M_Ansatz = result.Lohn13Ansatz
				row.M_Basis = result.CalcFerFeier13BasisResult.Lohn13Basis
				row.M_Betrag = result.CalcFerFeier13BasisResult.Lohn13Betrag * anzahlInUI
			End If

			RefreshAdditonalFeeGridDataSource()

		End Sub

		''' <summary>
		''' Creates empty additonal fee rows.
		''' </summary>
		''' <param name="inclueFeier">Boolean flag indicating if include Feier.</param>
		''' <param name="includeFerien">Boolean flag indicating if include Ferien.</param>
		''' <param name="includeLohn13">Boolean flag indicating if include Lohn13.</param>
		Private Sub CreateEmptyAdditonalFeeRows(ByVal inclueFeier As Boolean, ByVal includeFerien As Boolean, ByVal includeLohn13 As Boolean)

			Dim additionalFeeData As New List(Of RPLAdditionalFee)

			If inclueFeier Then
				additionalFeeData.Add(New RPLAdditionalFee With {.LANr = LANr_Feiertag, .LANrText = m_Translate.GetSafeTranslationValue("Feiertag")})
			End If

			If includeFerien Then
				additionalFeeData.Add(New RPLAdditionalFee With {.LANr = LANR_Ferien, .LANrText = m_Translate.GetSafeTranslationValue("Ferien")})
			End If

			If includeLohn13 Then
				additionalFeeData.Add(New RPLAdditionalFee With {.LANr = LANr_Lohn13, .LANrText = m_Translate.GetSafeTranslationValue("13.Lohn")})
			End If

			grdRPLAdditionalFees.DataSource = additionalFeeData

		End Sub

		''' <summary>
		''' Gets an additonal fee row by LANr.
		''' </summary>
		''' <param name="laNr">The LAnr.</param>
		''' <returns>The RPL additonal fee row or nothing if it could not be found.</returns>
		Private Function GetAdditonalFeeRowByLANr(ByVal laNr As Decimal) As RPLAdditionalFee

			If grdRPLAdditionalFees.DataSource Is Nothing Then
				Return Nothing
			End If

			Dim additonalFeeRows = CType(grdRPLAdditionalFees.DataSource, List(Of RPLAdditionalFee))

			Dim result = additonalFeeRows.Where(Function(data) data.LANr = laNr).FirstOrDefault

			Return result

		End Function

		''' <summary>
		''' Gets the flexible time row.
		''' </summary>
		''' <returns>The RPL flexible time row or nothing if it could not be found.</returns>
		Private Function GetFlexibleTimeRow() As RPLFlexibleTimeData

			If grdFlexibletime.DataSource Is Nothing Then
				Return Nothing
			End If

			Dim flexibleTimeRows = CType(grdFlexibletime.DataSource, List(Of RPLFlexibleTimeData))

			Dim result = flexibleTimeRows.FirstOrDefault

			Return result

		End Function

		''' <summary>
		''' Refreshes the additonal fee grid data source.
		''' </summary>
		Private Sub RefreshAdditonalFeeGridDataSource()
			grdRPLAdditionalFees.RefreshDataSource()
			gvRPLAdditionalFees.BestFitColumns()
		End Sub

		''' <summary>
		''' Sets the from and to date based on the active calendar week.
		''' </summary>
		Private Sub SetFromAndToDateBasedOnActiveCalendarWeek()
			Dim rpData = m_UCMediator.ActiveReportData.ReportData

			Dim reportMonth = rpData.Monat
			Dim reportYear = Convert.ToInt32(rpData.Jahr)
			Dim MondaySundayDate As List(Of Date)

			If reportMonth = 1 AndAlso lueWeek.EditValue >= 50 Then
				MondaySundayDate = m_DateUtility.GetMondayAndSundayOfWeek(reportYear - 1, reportMonth, lueWeek.EditValue)
			Else
				MondaySundayDate = m_DateUtility.GetMondayAndSundayOfWeek(reportYear, reportMonth, lueWeek.EditValue)
			End If
			'MondaySundayDate = m_DateUtility.GetMondayAndSundayOfWeek(reportYear, reportMonth, lueWeek.EditValue)

			Dim firstDayOfReportRange = rpData.Von.Value.Date
			Dim lastDayOfReportRange = rpData.Bis.Value.Date

			Dim firstValidDay = MondaySundayDate(0) ' m_DateUtility.GetFirstDayOfWeek(reportYear, lueWeek.EditValue)
			firstValidDay = m_DateUtility.MaxDate(firstDayOfReportRange, firstValidDay)
			firstValidDay = m_DateUtility.MinDate(lastDayOfReportRange, firstValidDay)

			' rechnet falsch!
			'Dim lastValidDay = firstValidDay.AddDays(7 - firstValidDay.DayOfWeek)

			Dim lastValidDay = MondaySundayDate(1) 'm_DateUtility.GetLastDayOfWeek(reportYear, reportMonth, lueWeek.EditValue)


			lastValidDay = m_DateUtility.MaxDate(firstDayOfReportRange, lastValidDay)
			lastValidDay = m_DateUtility.MinDate(lastDayOfReportRange, lastValidDay)

			' Makre sure dates are in min/max range of control
			dateEditFromDate.EditValue = m_DateUtility.LimitToRange(firstValidDay.Date, dateEditFromDate.Properties.MinValue, dateEditFromDate.Properties.MaxValue)
			dateEditToDate.EditValue = m_DateUtility.LimitToRange(lastValidDay.Date, dateEditToDate.Properties.MinValue, dateEditToDate.Properties.MaxValue)

		End Sub

		''' <summary>
		''' Sets min and max date to the report range.
		''' </summary>
		Private Sub SetMinAndMaxDateToReportRange()
			Dim rpData = m_UCMediator.ActiveReportData.ReportData
			dateEditFromDate.Properties.MinValue = rpData.Von
			dateEditFromDate.Properties.MaxValue = rpData.Bis

			dateEditToDate.Properties.MinValue = rpData.Von
			dateEditToDate.Properties.MaxValue = rpData.Bis
		End Sub

#End Region

#Region "Helper methods"


		''' <summary>
		''' Checks if additonal customer ES salary type data exists for an LANr.
		''' </summary>
		''' <param name="laNr">The LANr.</param>
		Private Function DoesAdditionalCustomerESSalaryTypeDataExistsForLANr(ByVal laNr As Decimal) As Boolean
			Dim doesExists = (Not m_AdditionalCustomerESSalaryTypeData Is Nothing AndAlso
													 m_AdditionalCustomerESSalaryTypeData.Any(Function(data) data.LANr = laNr))

			Return doesExists

		End Function

		''' <summary>
		''' Checks if additonal employee ES salary type data exists for an LANr.
		''' </summary>
		''' <param name="laNr">The LANr.</param>
		Private Function DoesAdditionalEmployeeESSalaryTypeDataExistsForLANr(ByVal laNr As Decimal) As Boolean
			Dim doesExists = (Not m_AdditionalEmployeeESSalaryTypeData Is Nothing AndAlso
													 m_AdditionalEmployeeESSalaryTypeData.Any(Function(data) data.LANr = laNr))

			Return doesExists

		End Function

		''' <summary>
		''' Focuses employee RPL data.
		''' </summary>
		''' <param name="rplNr">The RPLnr.</param>
		Private Sub FocusEmployeeRPLData(ByVal rplNr As Integer)

			If Not grdEmployeeRPL.DataSource Is Nothing Then

				Dim employeeRPLData = CType(grdEmployeeRPL.DataSource, List(Of RPLListData))

				Dim index = employeeRPLData.ToList().FindIndex(Function(data) data.RPLNr = rplNr)

				Dim previousState = SetSuppressUIEventsState(True)
				Dim rowHandle = gvEmployeeRPL.GetRowHandle(index)
				gvEmployeeRPL.FocusedRowHandle = rowHandle
				previousState = SetSuppressUIEventsState(previousState)
			End If

		End Sub

		''' <summary>
		''' Focuses customer RPL data.
		''' </summary>
		''' <param name="rplNr">The RPLnr.</param>
		Private Sub FocusCustomerRPLData(ByVal rplNr As Integer)

			If Not grdCustomerRPL.DataSource Is Nothing Then

				Dim customerRPLData = CType(grdCustomerRPL.DataSource, List(Of RPLListData))

				Dim index = customerRPLData.ToList().FindIndex(Function(data) data.RPLNr = rplNr)

				Dim previousState = SetSuppressUIEventsState(True)
				Dim rowHandle = gvCustomerRPL.GetRowHandle(index)
				gvCustomerRPL.FocusedRowHandle = rowHandle
				previousState = SetSuppressUIEventsState(previousState)
			End If

		End Sub

		''' <summary>
		''' Sets the focus appearance on the employee RPL grid.
		''' </summary>
		''' <param name="isFocused">The is focused flag.</param>
		Private Sub SetFocusedAppearanceOnEmployeeRPLGrid(ByVal isFocused As Boolean)

			gvEmployeeRPL.OptionsSelection.EnableAppearanceFocusedRow = isFocused
			gvEmployeeRPL.OptionsSelection.EnableAppearanceFocusedCell = isFocused

		End Sub

		''' <summary>
		''' Sets the focus appearance on the customer RPL grid.
		''' </summary>
		''' <param name="isFocused">The is focused flag.</param>
		Private Sub SetFocusedAppearanceOnCustomerRPLGrid(ByVal isFocused As Boolean)

			gvCustomerRPL.OptionsSelection.EnableAppearanceFocusedRow = isFocused
			gvCustomerRPL.OptionsSelection.EnableAppearanceFocusedCell = isFocused

		End Sub

		''' <summary>
		''' Opens pf of selected RPL.
		''' </summary>
		Private Sub OpenPDFOfSelectedRPL()

			If m_UCMediator.ActiveReportData Is Nothing Then
				Return
			End If

			If m_SelectedRPLListData Is Nothing Then
				Return
			End If

			'Dim o2Open As New SP.RP.ShowScannedDoc.ClsMain_Net

			Dim rpData = m_UCMediator.ActiveReportData.ReportData

			Try
				Dim kw As Integer? = lueWeek.EditValue
				Dim kwVonBis = m_DateUtility.GetCalendarWeeksBetweenDates(m_SelectedRPLListData.VonDate, m_SelectedRPLListData.BisDate)
				If kw Is Nothing Then kw = kwVonBis(0)



				Dim frm As New SP.RP.ShowScannedDoc.frmRPDocScan(m_InitializationData)
				frm.PreselectionData = New SP.RP.ShowScannedDoc.PreselectionData With {.EmployeeNumber = rpData.EmployeeNumber,
																																							 .CustomerNumber = rpData.CustomerNumber,
																																							 .ESNumber = rpData.ESNR,
																																							 .ReportNumber = rpData.RPNR,
																																							 .ReportLineNumber = m_SelectedRPLListData.RPLNr,
																																							 .CalendarWeek = CShort(kw)}

				frm.LoadIndividualReport()
				frm.Show()
				frm.BringToFront()



				'				o2Open.ShowfrmScanOneRP(rpData.EmployeeNumber, rpData.CustomerNumber, rpData.ESNR, rpData.RPNR, m_SelectedRPLListData.RPLNr, CShort(kw))

			Catch ex As Exception
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try

		End Sub

		''' <summary>
		''' Handles drop down button clicks.
		''' </summary>
		Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

			Const ID_OF_DELETE_BUTTON As Int32 = 1

			' If delete button has been clicked reset the drop down.
			If e.Button.Index = ID_OF_DELETE_BUTTON Then

				If TypeOf sender Is BaseEdit Then
					If CType(sender, BaseEdit).Properties.ReadOnly Then
						' nothing
					Else
						CType(sender, BaseEdit).EditValue = Nothing
					End If
				End If
			End If
		End Sub

#End Region

#End Region

#Region "View helper classes"

		''' <summary>
		'''  KST view data.
		''' </summary>
		Class KSTViewData
			Public Property Id As Integer
			Public Property RecordNumber As Integer?
			Public Property Description As String

		End Class

		''' <summary>
		''' Calendar week view data.
		''' </summary>
		Class CalendarWeekViewData
			Public Property CalendarWeek As Integer
		End Class

		''' <summary>
		''' LA value bounds
		''' </summary>
		Class LAValueUpperBounds
#Region "Public Properties"

			Public Property AnzahlBoundaryValue As Decimal?
			Public Property BasisBoundaryValue As Decimal?
			Public Property AnsatzBoundaryValue As Decimal?

#End Region

#Region "Public Methods"

			''' <summary>
			''' Resets the values.
			''' </summary>
			Public Sub Reset()
				AnzahlBoundaryValue = Nothing
				BasisBoundaryValue = Nothing
				AnsatzBoundaryValue = Nothing
			End Sub

			''' <summary>
			''' Checks if Anzahl value is in boundary.
			''' </summary>
			''' <param name="anzahl">The Anzahl value.</param>
			''' <returns>Boolean value indicating if value is in boundary.</returns>
			Public Function IsAnzahlValueInBoundary(ByVal anzahl As Decimal) As Boolean
				Return IsValueInBoundary(anzahl, AnzahlBoundaryValue)
			End Function

			''' <summary>
			''' Checks if Basis value is in boundary.
			''' </summary>
			''' <param name="Basis">The Basis value.</param>
			''' <returns>Boolean value indicating if value is in boundary.</returns>
			Public Function IsBasisValueInBoundary(ByVal basis As Decimal) As Boolean
				Return IsValueInBoundary(basis, BasisBoundaryValue)
			End Function

			''' <summary>
			''' Checks if Ansatz value is in boundary.
			''' </summary>
			''' <param name="Ansatz">The Ansatz value.</param>
			''' <returns>Boolean value indicating if value is in boundary.</returns>
			Public Function IsAnsatzValueInBoundary(ByVal ansatz As Decimal) As Boolean
				Return IsValueInBoundary(ansatz, AnsatzBoundaryValue)
			End Function

#End Region

#Region "Private Methods"

			''' <summary>
			''' Checks if a value in in boundary.
			''' </summary>
			''' <param name="value">The value.</param>
			''' <param name="boundaryValue">The boundary value.</param>
			''' <returns>Boolean value indicating if value is in boundary.</returns>
			Private Function IsValueInBoundary(ByVal value As Decimal, ByVal boundaryValue As Decimal?) As Boolean

				Dim inBoundary As Boolean = True

				If boundaryValue.HasValue Then
					If boundaryValue > 0 Then
						inBoundary = value <= boundaryValue
					Else
						inBoundary = value >= boundaryValue
					End If

				End If

				Return inBoundary
			End Function

#End Region

		End Class

#End Region


#Region "GridSettings"

		Private Sub RestoreGridLayoutFromXml(ByVal GridName As String)
			Dim keepFilter = False
			Dim restoreLayout = True

			Select Case GridName.ToLower
				Case gvEmployeeRPL.Name.ToLower
					Try
						keepFilter = m_Utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRPLEmployeeFilter), False)
						restoreLayout = m_Utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreRPLEmployeeSetting), True)

					Catch ex As Exception

					End Try

					If restoreLayout AndAlso File.Exists(m_GVRPLEmployeeSettingfilename) Then gvEmployeeRPL.RestoreLayoutFromXml(m_GVRPLEmployeeSettingfilename)
					If restoreLayout AndAlso Not keepFilter Then gvEmployeeRPL.ActiveFilterCriteria = Nothing

				Case gvCustomerRPL.Name.ToLower
					Try
						keepFilter = m_Utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRPLCustomerFilter), False)
						restoreLayout = m_Utility.ParseToBoolean(UserGridSettingsXml.GetSettingByKey(m_xmlSettingRestoreRPLCustomerSetting), True)
					Catch ex As Exception

					End Try

					If restoreLayout AndAlso File.Exists(m_GVRPLCustomerSettingfilename) Then gvCustomerRPL.RestoreLayoutFromXml(m_GVRPLCustomerSettingfilename)
					If restoreLayout AndAlso Not keepFilter Then gvCustomerRPL.ActiveFilterCriteria = Nothing


				Case Else

					Exit Sub


			End Select


		End Sub



		Private Sub OngvRPLEmployeeColumnPositionChanged(sender As Object, e As System.EventArgs)

			gvEmployeeRPL.SaveLayoutToXml(m_GVRPLEmployeeSettingfilename)

		End Sub

		Private Sub OngvRPLCustomerColumnPositionChanged(sender As Object, e As System.EventArgs)

			gvCustomerRPL.SaveLayoutToXml(m_GVRPLCustomerSettingfilename)

		End Sub



#End Region

		Private Sub OnControl_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs)
			Try
				If e.KeyChar = Chr(13) Then
					If sender.name.ToString.ToLower.Contains("txtAdditionalInfo".ToLower) Then
						If btnOpenHourInput.Visible Then
							OnBtnOpenHourInput_Click(sender, New System.EventArgs)
						End If
					End If
					SendKeys.Send("{tab}")
					e.Handled = True
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("KeyPress: {0}", ex.Message))
			End Try

		End Sub

		Private Sub OnControl_KeyUp2Save(sender As Object, e As System.Windows.Forms.KeyEventArgs)
			Try
				If e.KeyCode = Keys.F5 OrElse (e.Control And e.KeyCode = Keys.S) Then
					If Not btnOpenHourInput.Visible Then
						OnBtnSave_Click(sender, New System.EventArgs)
					End If

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("KeyUp: {0}", ex.Message))
			End Try

		End Sub

		Private Sub OntxtAnzahl_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtAnzahl.KeyUp

			Try
				If e.KeyCode = Keys.F4 Then
					If btnOpenHourInput.Visible Then
						OnBtnOpenHourInput_Click(sender, New System.EventArgs)

					End If
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("KeyUp: {0}", ex.Message))
			End Try

		End Sub

		Protected Sub OntxtControl_GotFocus(sender As Object, e As System.EventArgs)
			Try
				Dim txtCtl As TextEdit = CType(sender, TextEdit)
				'txtCtl.SelectionStart = 0
				'txtCtl.SelectionLength = txtCtl.Text.Length
				txtCtl.SelectAll()
				'SendKeys.Send("+{HOME}")
				'txtCtl.Focus()

				'Trace.WriteLine(txtCtl.SelectedText)

			Catch ex As Exception
				m_Logger.LogError(String.Format("KeyUp: {0}", ex.Message))
			End Try

		End Sub



	End Class

End Namespace

