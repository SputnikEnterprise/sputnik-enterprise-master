Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SPProgUtility.CommonSettings
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.ES
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Namespace UI

  ''' <summary>
  ''' ES create service.
  ''' </summary>
  Public Class ESCreateService

#Region "Private Constants"

    Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

#Region "Private Fields"

    ''' <summary>
    ''' The logger.
    ''' </summary>
    Private Shared m_Logger As ILogger = New Logger()

    ''' <summary>
    ''' UI Utility functions.
    ''' </summary>
    Protected m_UtilityUI As UtilityUI

    ''' <summary>
    ''' The Initialization data.
    ''' </summary>
    Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

    ''' <summary>
    ''' The translation value helper.
    ''' </summary>
    Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

    ''' <summary>
    ''' The data access object.
    ''' </summary>
    Protected m_ESDataAccess As IESDatabaseAccess

    ''' <summary>
    ''' The employee data access object.
    ''' </summary>
    Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

    ''' <summary>
    ''' The mandant.
    ''' </summary>
    Private m_md As Mandant

    ''' <summary>
    ''' The cls prog path.
    ''' </summary>
    Private m_ProgPath As ClsProgPath

    ''' <summary>
    ''' The SPProgUtility object.
    ''' </summary>
    Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="mdNr">The mandant number.</param>
    ''' <param name="_setting">The settings object.</param>
    Public Sub New(ByVal mdNr As Integer, ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
      Try
        m_md = New Mandant
        m_ProgPath = New ClsProgPath
        m_InitializationData = _setting
        m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

      Catch ex As Exception
        m_Logger.LogError(ex.ToString)

      End Try

      m_UtilityUI = New SP.Infrastructure.UI.UtilityUI

      Dim conStr = m_md.GetSelectedMDData(mdNr).MDDbConn

      m_ESDataAccess = New DatabaseAccess.ES.ESDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
      m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)

    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Creats an ES.
    ''' </summary>
    ''' <param name="candidateAndCustomerData">The candidate and customer data.</param>
    ''' <param name="esData">The ES data.</param>
    ''' <param name="salaryData">The salary data.</param>
    ''' <returns>Number of new ES.</returns>
    Public Function CreateES(ByVal candidateAndCustomerData As InitCandidateAndCustomerData,
                            ByVal esData As InitESData,
                            ByVal salaryData As InitESSalaryData) As Integer?


      Dim sumStundenLohnAndTarif As Decimal = salaryData.StundenLohn + salaryData.Tarif

      ' Customer data.
      Dim customerMasterData = candidateAndCustomerData.CustomerData

      ' Employee data.
      Dim employeeMasterData = candidateAndCustomerData.EmployeeData
      Dim employeeLOSettingData As EmployeeLOSettingsData = m_EmployeeDatabaseAccess.LoadEmployeeLOSettings(employeeMasterData.EmployeeNumber)

      If employeeLOSettingData Is Nothing Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohneinstellungen des Mitarbeiters konnten nicht geladen werden."))
        Return Nothing
      End If

      ' ZHD data.
      Dim responsiblePersonMasterData As ResponsiblePersonMasterData = candidateAndCustomerData.ResponsiblePersondata


      Dim mdNr As Integer = candidateAndCustomerData.MandantData.MandantNumber
      Dim employeeNumber As Integer = candidateAndCustomerData.EmployeeData.EmployeeNumber
      Dim customerNumber As Integer = candidateAndCustomerData.CustomerData.CustomerNumber

      Dim printNoRPCodeFromXML As String = m_ProgPath.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(mdNr), String.Format("{0}/esreportsnotprint", FORM_XML_MAIN_KEY))
      Dim printNoRPInitialSetting As Boolean = False

      Select Case printNoRPCodeFromXML
        Case "0"
          printNoRPInitialSetting = False
        Case "1"
          printNoRPInitialSetting = True
        Case "2" ' Take setting from customer
          printNoRPInitialSetting = customerMasterData.NotPrintReports.HasValue AndAlso customerMasterData.NotPrintReports.Value
        Case "3" ' Take setting from employee
          printNoRPInitialSetting = employeeLOSettingData.NoRPPrint.HasValue AndAlso employeeLOSettingData.NoRPPrint.Value
        Case Else ' Take setting form customer as fallback
          printNoRPInitialSetting = customerMasterData.NotPrintReports.HasValue AndAlso customerMasterData.NotPrintReports.Value
      End Select

      Dim createDate As DateTime = DateTime.Now

      ' Create ES
      Dim esDataObject = CreatedESObject(mdNr,
                                         employeeNumber,
                                         customerNumber,
                                         candidateAndCustomerData.ResponsiblePersondata,
                                         employeeLOSettingData.Currency,
                                         salaryData.EffectiveGAVData,
                                         esData,
                                         salaryData,
                                         createDate,
                                         printNoRPInitialSetting)


      ' Create ESLohn
      Dim esLohnObject = CreateESLohnObject(employeeNumber,
                                            customerNumber,
                                            salaryData,
                                            salaryData.EffectiveGAVData,
                                            createDate)

      ' Create ESLohn_GAVData
      Dim esLohnGAVDataObject = CreateESGAVDataObject(employeeNumber,
                                                      customerNumber,
                                                      salaryData.EffectiveGAVData,
                                                      createDate)

      ' -- RP --
      Dim rp As RPData = Nothing

      If salaryData.Tarif + salaryData.StundenLohn > 0 Then

        rp = CreateRPDataObject(mdNr,
                                employeeNumber,
                                customerNumber,
                                employeeLOSettingData.Currency,
                                salaryData.FarPflichtig,
                                esData,
                                salaryData.EffectiveGAVData,
                                createDate)

      End If

      Dim esOffset As Integer = ReadESOffsetFromSettings()
      Dim rpOffset As Integer = ReadRPOffsetFromSettings()

      Dim success As Boolean = m_ESDataAccess.AddNewESWithESLohnAndRP(esDataObject,
                                                                      esLohnObject,
                                                                      esLohnGAVDataObject,
                                                                      rp,
                                                                      esOffset,
                                                                      rpOffset)

      If success Then
        Return esDataObject.ESNR
      End If

      Return Nothing

    End Function

    ''' <summary>
    ''' Creates an ES salary.
    ''' </summary>
    ''' <param name="esNr">The ES number.</param>
    ''' <param name="candidateAndCustomerData">The candidate and customer data.</param>
    ''' <param name="esData">The ES data.</param>
    ''' <param name="salaryData">The salary data.</param>
    ''' <returns>The new ESLohn number.</returns>
    Public Function CreateESLohn(ByVal esNr As Integer,
                                 ByVal candidateAndCustomerData As InitCandidateAndCustomerData,
                                 ByVal esData As InitESData,
                                 ByVal salaryData As InitESSalaryData) As Integer?

      Dim customerMasterData = candidateAndCustomerData.CustomerData
      Dim employeeMasterData = candidateAndCustomerData.EmployeeData
      Dim employeeLOSettingData As EmployeeLOSettingsData = m_EmployeeDatabaseAccess.LoadEmployeeLOSettings(employeeMasterData.EmployeeNumber)
      Dim existsRpForES = m_ESDataAccess.CheckIfRPExistsForES(esNr)

      If employeeLOSettingData Is Nothing Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohneinstellungen des Mitarbeiters konnten nicht geladen werden."))
        Return Nothing
      End If

      If Not existsRpForES.HasValue Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Prüfung auf existierende Rapport ist fehlgeschlagen."))
        Return Nothing
      End If

      Dim mdNr As Integer = candidateAndCustomerData.MandantData.MandantNumber
      Dim employeeNumber As Integer = candidateAndCustomerData.EmployeeData.EmployeeNumber
      Dim customerNumber As Integer = candidateAndCustomerData.CustomerData.CustomerNumber

      Dim createDate As DateTime = DateTime.Now

      ' Create ESLohn
      Dim esLohnObject = CreateESLohnObject(employeeNumber,
                                           customerNumber,
                                           salaryData,
                                           salaryData.EffectiveGAVData,
                                           createDate)
      ' Create ESLohn_GAVData
      Dim esLohnGAVDataObject = CreateESGAVDataObject(employeeNumber,
                                                      customerNumber,
                                                      salaryData.EffectiveGAVData,
                                                      createDate)

      ' -- RP --
      Dim rp As RPData = Nothing

			If (salaryData.Tarif + salaryData.StundenLohn > 0) AndAlso Not existsRpForES Then

				rp = CreateRPDataObject(mdNr,
																employeeNumber,
																customerNumber,
																employeeLOSettingData.Currency,
																salaryData.FarPflichtig,
																esData,
																salaryData.EffectiveGAVData,
																createDate)

			End If

			Dim rpOffset As Integer = ReadRPOffsetFromSettings()
      Dim success As Boolean = m_ESDataAccess.AddNewESLohnAndRP(esNr,
                                                                esLohnObject,
                                                                esLohnGAVDataObject,
                                                                rp,
                                                                rpOffset)

      If success Then
        Return esLohnObject.ESLohnNr
      End If

      Return Nothing

    End Function

    ''' <summary>
    ''' Determines effective GAV data based on GAV string data and Stundenlohn.
    ''' </summary>
    ''' <param name="gavStringData">The gav string data.</param>
		''' <param name="grundLohn">The grund lohn.</param>
    ''' <param name="farPflichtig">Far pflichtig value.</param>
    ''' <param name="mdNumber">The mandant number.</param>
    ''' <param name="bNoPVL">noPVL boolean flag.</param>
    ''' <returns>Final GAV data.</returns>
		Public Function DetermineEffectiveGAVData(ByVal gavStringData As GAVStringData,
																					ByVal grundLohn As Decimal,
																					ByVal farPflichtig As Boolean,
																					ByVal mdNumber As Integer,
																					ByVal bNoPVL As Boolean) As EffectiveGAVData

			Dim isGAVDataSelected As Boolean = (Not gavStringData Is Nothing)

			If isGAVDataSelected Then
				' Make a copy of the data so the data can be altered if bNoGav is true.
				gavStringData = gavStringData.ShallowCopy

			End If

			Dim gavText As String = String.Empty

			' -- NOGAV etc.

			Dim bNoGAV As Boolean = False
			If isGAVDataSelected AndAlso gavStringData.GAVNr = 815001 Then

				Dim loadSuvaDataSuccess As Boolean

				' Note: The current year is taken here not the Year or the ES start.
				Dim suva_HL = m_ESDataAccess.LoadMandantSuvaHLData(DateTime.Now.Year, mdNumber, loadSuvaDataSuccess)

				If Not loadSuvaDataSuccess Then
					m_UtilityUI.ShowErrorDialog(String.Format("Suva-HL Daten des Mandanten fürs Nummer, Jahr {0}, {1} konnten nicht geladen werden. Es wird der Standardwert von 126'000.- CHF angenommen!", mdNumber, DateTime.Now.Year))
					suva_HL = 126000D
				End If

				Dim cHLLohn As Decimal = suva_HL
				Dim minLohn4GAV = (cHLLohn / 13D) / 182.25D

				bNoGAV = (grundLohn >= minLohn4GAV)

				If bNoGAV Then
					Dim msgHLSuva As String = "Achtung: Der Lohn ist höher als Höchstlohn ({1:n2} CHF) für GAV-Berufe. Daher ist der Einsatz NICHT GAV-pflichtig. Berechnungsart: ({2:n2} / 13) / 182.25 = {1:n2}"
					msgHLSuva = String.Format(m_Translate.GetSafeTranslationValue(msgHLSuva), vbNewLine, minLohn4GAV, cHLLohn)
					m_UtilityUI.ShowOKDialog(msgHLSuva)
					'm_Translate.GetSafeTranslationValue("Achtung: Der Lohn ist höher als Höchstlohn ") &
					'																	 String.Format("({0:0.00} CHF) ", minLohn4GAV) &
					'																	 m_Translate.GetSafeTranslationValue("für GAV-Berufe. Daher ist der Einsatz NICHT GAV-pflichtig."), m_Translate.GetSafeTranslationValue("GAV-Höchstlohn"))
				End If



				If bNoGAV Then
					gavStringData.GAVNr = 0
					gavStringData.Gruppe0 = String.Empty
					gavStringData.Gruppe1 = String.Empty
					gavStringData.Gruppe2 = String.Empty
					gavStringData.Gruppe3 = String.Empty
					gavStringData.GAVText = String.Empty

					gavStringData._WAG = 0D	 ' GAV_WAG
					gavStringData._WAN = 0D	' GAV_WAN
					gavStringData._VAG = 0D	' GAV_VAG
					gavStringData._VAN = 0D	' GAV_VAN

					gavStringData._WAG_S = 0D	' GAV_WAG_S 
					gavStringData._WAN_S = 0D	' GAV_WAN_S 
					gavStringData._VAG_S = 0D	' GAV_VAG_S 
					gavStringData._VAN_S = 0D	' GAV_VAN_S  
					gavStringData._FAG_M = 0D	' GAV_FAG_M
					gavStringData._FAN_M = 0D	' GAV_FAN_M 
					gavStringData._WAG_M = 0D	' GAV_WAG_M 
					gavStringData._WAN_M = 0D	' GAV_WAN_M 
					gavStringData._VAG_M = 0D	' GAV_VAG_M 
					gavStringData._VAN_M = 0D	' GAV_VAN_M
					gavStringData._WAG_J = 0D	' GAV_WAG_J
					gavStringData._WAN_J = 0D	' GAV_WAN_J
					gavStringData._VAG_J = 0D	' GAV_VAG_J,
					gavStringData._VAN_J = 0D	' GAV_VAN_J

				End If

			End If


			If bNoGAV Then
				gavText = String.Format("1 (- 815001 NOGAV)")
			Else
				gavText = If(isGAVDataSelected,
									String.Format("{0}) {1}", gavStringData.GAVNr, gavStringData.Gruppe0),
									"1 (-)")
			End If


			Dim effectiveGAVData As New EffectiveGAVData

			effectiveGAVData.GavText = gavText
			effectiveGAVData.GAVNr = If(bNoGAV, 0, If(isGAVDataSelected, gavStringData.GAVNr, 0))

			effectiveGAVData.GAVKanton = If(isGAVDataSelected, gavStringData.Kanton, String.Empty)
			effectiveGAVData.GAVGruppe0 = If(bNoGAV, String.Empty, If(isGAVDataSelected, gavStringData.Gruppe0, String.Empty))
			effectiveGAVData.GAVGruppe1 = If(bNoGAV, String.Empty, If(isGAVDataSelected, gavStringData.Gruppe1, String.Empty))
			effectiveGAVData.GAVGruppe2 = If(bNoGAV, String.Empty, If(isGAVDataSelected, gavStringData.Gruppe2, String.Empty))
			effectiveGAVData.GAVGruppe3 = If(bNoGAV, String.Empty, If(isGAVDataSelected, gavStringData.Gruppe3, String.Empty))
			effectiveGAVData.GAVBezeichnung = If(bNoGAV, String.Empty, If(isGAVDataSelected, gavStringData.GAVText, String.Empty))

			If Not farPflichtig Or bNoGAV Then
				effectiveGAVData.GAV_FAG = 0
				effectiveGAVData.GAV_FAN = 0
			Else

				If bNoPVL Then
					effectiveGAVData.GAV_FAG = If(isGAVDataSelected, gavStringData._FAG, 0D)
					effectiveGAVData.GAV_FAN = If(isGAVDataSelected, gavStringData._FAN, 0D)

				Else
					effectiveGAVData.GAV_FAG = If(isGAVDataSelected, gavStringData.FARAG, 0D)
					effectiveGAVData.GAV_FAN = If(isGAVDataSelected, gavStringData.FARAN, 0D)

				End If

			End If


			If bNoGAV Then
				effectiveGAVData.GAV_WAG = 0
				effectiveGAVData.GAV_WAN = 0
				effectiveGAVData.GAV_VAG = 0
				effectiveGAVData.GAV_VAN = 0
			Else
				If bNoPVL Then

					effectiveGAVData.GAV_WAG = If(isGAVDataSelected, gavStringData._WAG, 0D)
					effectiveGAVData.GAV_WAN = If(isGAVDataSelected, gavStringData._WAN, 0D)

					effectiveGAVData.GAV_VAG = If(isGAVDataSelected, gavStringData._VAG, 0D)
					effectiveGAVData.GAV_VAN = If(isGAVDataSelected, gavStringData._VAN, 0D)

				Else
					effectiveGAVData.GAV_WAG = 0
					effectiveGAVData.GAV_WAN = 0
					effectiveGAVData.GAV_VAG = If(isGAVDataSelected, gavStringData.VAG_Value, 0D)
					effectiveGAVData.GAV_VAN = If(isGAVDataSelected, gavStringData.VAN_Value, 0D)

				End If

			End If

			effectiveGAVData.GAV_StdWeek = If(isGAVDataSelected, gavStringData.StdWeek, 50D) ' TODO für Zukunft: maximale Arbeitstunden pro Woche
			effectiveGAVData.GAV_StdMonth = If(isGAVDataSelected, gavStringData.StdMonth, 182.25D) ' TODO für Zukunft: maximale Monats Stunden
			effectiveGAVData.GAV_StdYear = If(isGAVDataSelected, gavStringData.StdYear, 2180D) ' TODO für Zukunft: maximale Jahres Stunden
			effectiveGAVData.GAVStdLohn = If(isGAVDataSelected, gavStringData.StdLohn, grundLohn)

			If bNoGAV Then
				effectiveGAVData.GAV_FAG_S = 0
				effectiveGAVData.GAV_FAN_S = 0
				effectiveGAVData.GAV_WAG_S = 0
				effectiveGAVData.GAV_WAN_S = 0
				effectiveGAVData.GAV_VAG_S = 0
				effectiveGAVData.GAV_VAN_S = 0
				effectiveGAVData.GAV_FAG_M = 0
				effectiveGAVData.GAV_FAN_M = 0
				effectiveGAVData.GAV_WAG_M = 0
				effectiveGAVData.GAV_WAN_M = 0
				effectiveGAVData.GAV_VAG_M = 0
				effectiveGAVData.GAV_VAN_M = 0
				effectiveGAVData.GAV_FAG_J = 0
				effectiveGAVData.GAV_FAN_J = 0
				effectiveGAVData.GAV_WAG_J = 0
				effectiveGAVData.GAV_WAN_J = 0
				effectiveGAVData.GAV_VAG_J = 0
				effectiveGAVData.GAV_VAN_J = 0
			Else
				effectiveGAVData.GAV_FAG_S = 0
				effectiveGAVData.GAV_FAN_S = 0
				'effectiveGAVData.GAV_WAG_S = 0
				'effectiveGAVData.GAV_WAN_S = 0
				'effectiveGAVData.GAV_VAG_S = 0
				'effectiveGAVData.GAV_VAN_S = 0
				effectiveGAVData.GAV_FAG_M = 0
				effectiveGAVData.GAV_FAN_M = 0
				'effectiveGAVData.GAV_WAG_M = 0
				'effectiveGAVData.GAV_WAN_M = 0
				'effectiveGAVData.GAV_VAG_M = 0
				'effectiveGAVData.GAV_VAN_M = 0
				effectiveGAVData.GAV_FAG_J = 0
				effectiveGAVData.GAV_FAN_J = 0

				If bNoPVL Then

					effectiveGAVData.GAV_WAG_S = If(isGAVDataSelected, gavStringData._WAG_S, 0D)
					effectiveGAVData.GAV_WAN_S = If(isGAVDataSelected, gavStringData._WAN_S, 0D)
					effectiveGAVData.GAV_VAG_S = If(isGAVDataSelected, gavStringData._VAG_S, 0D)
					effectiveGAVData.GAV_VAN_S = If(isGAVDataSelected, gavStringData._VAN_S, 0D)

					effectiveGAVData.GAV_WAG_M = If(isGAVDataSelected, gavStringData._WAG_M, 0D)
					effectiveGAVData.GAV_WAN_M = If(isGAVDataSelected, gavStringData._WAN_M, 0D)
					effectiveGAVData.GAV_VAG_M = If(isGAVDataSelected, gavStringData._VAG_M, 0D)
					effectiveGAVData.GAV_VAN_M = If(isGAVDataSelected, gavStringData._VAN_M, 0D)

					effectiveGAVData.GAV_WAG_J = If(isGAVDataSelected, gavStringData._WAG_J, 0D)
					effectiveGAVData.GAV_WAN_J = If(isGAVDataSelected, gavStringData._WAN_J, 0D)
					effectiveGAVData.GAV_VAG_J = If(isGAVDataSelected, gavStringData._VAG_J, 0D)
					effectiveGAVData.GAV_VAN_J = If(isGAVDataSelected, gavStringData._VAN_J, 0D)

				Else
					effectiveGAVData.GAV_WAG_S = 0
					effectiveGAVData.GAV_WAN_S = 0
					effectiveGAVData.GAV_VAG_S = 0
					effectiveGAVData.GAV_VAN_S = 0

					effectiveGAVData.GAV_WAG_M = 0
					effectiveGAVData.GAV_WAN_M = 0
					effectiveGAVData.GAV_VAG_M = 0
					effectiveGAVData.GAV_VAN_M = 0

					effectiveGAVData.GAV_WAG_J = 0
					effectiveGAVData.GAV_WAN_J = 0
					effectiveGAVData.GAV_VAG_J = 0
					effectiveGAVData.GAV_VAN_J = 0
				End If

			End If

			effectiveGAVData.GAVInfo_String = If(bNoGAV Or Not isGAVDataSelected, String.Empty, gavStringData.CompleteGAVString)

			effectiveGAVData.IsPVL = If(isGAVDataSelected, gavStringData.IsPVL, 0)

			effectiveGAVData.IsGavDataSelected = isGAVDataSelected

			Return effectiveGAVData

		End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Creates an ES master data object.
    ''' </summary>
    ''' <param name="mdNumber">The manant number.</param>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="responsiblePersonMasterData">The responsible person data.</param>
    ''' <param name="currency">The currency.</param>
    ''' <param name="effectiveGAVData">The effective GAV data.</param>
    ''' <param name="esData">The ES data.</param>
    ''' <param name="salaryData">The salary data.</param>
    ''' <param name="createDate">The create date.</param>
    ''' <returns>ES mater data object.</returns>
    Private Function CreatedESObject(ByVal mdNumber As Integer,
                                    ByVal employeeNumber As Integer,
                                    ByVal customerNumber As Integer,
                                    ByVal responsiblePersonMasterData As ResponsiblePersonMasterData,
                                    ByVal currency As String,
                                    ByVal effectiveGAVData As EffectiveGAVData,
                                    ByVal esData As InitESData,
                                    ByVal salaryData As InitESSalaryData,
                                    ByVal createDate As DateTime,
                                    ByVal printNoRPInitialSetting As Boolean) As ESMasterData


      Dim esZeitFromXML As String = m_ProgPath.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(mdNumber), String.Format("{0}/eszeit", FORM_XML_MAIN_KEY))
      Dim esOrtFromXML As String = m_ProgPath.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(mdNumber), String.Format("{0}/esort", FORM_XML_MAIN_KEY))
      Dim esVertragFromXML As String = m_ProgPath.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(mdNumber), String.Format("{0}/esvertrag", FORM_XML_MAIN_KEY))
      Dim esVerleihvertragFromXML As String = m_ProgPath.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(mdNumber), String.Format("{0}/esverleih", FORM_XML_MAIN_KEY))

      Dim sumStundenLohnAndTarif As Decimal = salaryData.StundenLohn + salaryData.Tarif

      ' ---ES ---
      Dim es As New ESMasterData

      es.EmployeeNumber = employeeNumber
      es.CustomerNumber = customerNumber
      es.KSTBez = String.Empty
      es.ESKst = esData.MA_KD_Berater
      es.Arbzeit = esZeitFromXML
      es.Arbort = esOrtFromXML
      es.Melden = String.Empty
      es.ES_Als = esData.ESAls
      es.ES_Ab = esData.ESStartDate
      es.ES_Uhr = esData.Uhrzeit
      es.ES_Ende = esData.ESEndDate
      es.Ende = String.Empty
      es.GAVText = effectiveGAVData.GavText

      es.Bemerk_MA = esVertragFromXML
      es.Bemerk_KD = esVerleihvertragFromXML
      es.Bemerk_RE = String.Empty
      es.Bemerk_Lo = String.Empty
      es.Bemerk_P = String.Empty

      ' W=Rapporte werden erstellt, K=Keine Rapporte werden erstellt.
      es.RP_Art = If(sumStundenLohnAndTarif > 0D, "W", "K")

      es.LeistungsDoc = String.Empty
      es.MWST = String.Empty
      es.SUVA = esData.SUVA
      es.Currency = currency
      es.CreatedOn = createDate
      es.CreatedFrom = m_InitializationData.UserData.UserFullName
      es.CreatedKST = m_InitializationData.UserData.UserKST
      es.ChangedOn = createDate
      es.ChangedFrom = m_InitializationData.UserData.UserFullName
      es.ChangedKST = m_InitializationData.UserData.UserKST
      es.Result = String.Empty

      es.KDZustaendig = If(Not responsiblePersonMasterData Is Nothing,
                           String.Format("{0:000000})   {1} {2}, {3}", responsiblePersonMasterData.RecordNumber,
                                                                       responsiblePersonMasterData.TranslatedSalutation,
                                                                       responsiblePersonMasterData.Lastname,
                                                                       responsiblePersonMasterData.Firstname),
                           String.Empty)

      es.ESKST1 = esData.Kst1
      es.ESKST2 = esData.Kst2
      es.ESUnterzeichner = esData.Unterzeichner
      es.VerleihBacked = False
      es.Bemerk_1 = String.Empty
      es.Bemerk_2 = String.Empty
      es.Bemerk_3 = String.Empty
      es.Print_KD = False
      es.Print_MA = False
      es.FarPflichtig = salaryData.FarPflichtig
      es.ESVerBacked = False
      es.NoListing = False
      es.BVGCode = 9 ' Automaticher Abzug
      es.Einstufung = If(esData.ESEinstufung Is Nothing, String.Empty, esData.ESEinstufung)
      es.ESBranche = If(esData.Branche Is Nothing, String.Empty, esData.Branche)
      es.GoesLonger = String.Empty
      es.ProposeNr = 0
      es.VakNr = esData.VakNr
      es.PNr = esData.PNr
      es.KDZHDNr = If(responsiblePersonMasterData Is Nothing, Nothing, responsiblePersonMasterData.RecordNumber)
      es.MDNr = mdNumber
      es.PrintNoRP = printNoRPInitialSetting

      Return es
    End Function

    ''' <summary>
    ''' Creats an ES Lohn object.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="salaryData">The salary data.</param>
    ''' <param name="effectiveGAVData">The effective GAV data.</param>
    ''' <param name="createDate">The crate date.</param>
    ''' <returns>ESSalary data.</returns>
    Private Function CreateESLohnObject(ByVal employeeNumber As Integer,
                                        ByVal customerNumber As Integer,
                                        ByVal salaryData As InitESSalaryData,
                                        ByVal effectiveGAVData As EffectiveGAVData,
                                        ByVal createDate As DateTime) As ESSalaryData


      ' --- ESLohn ---
      Dim esLohn As New ESSalaryData

      esLohn.EmployeeNumber = employeeNumber
      esLohn.CustomerNumber = customerNumber
      esLohn.KSTNr = salaryData.CustomerKST

      esLohn.KSTBez = salaryData.CustomerKSTBez
      esLohn.GavText = effectiveGAVData.GavText
      esLohn.GrundLohn = salaryData.GrundLohn
      esLohn.StundenLohn = salaryData.StundenLohn
      esLohn.FerBasis = salaryData.FerienBasis
      esLohn.Ferien = salaryData.FerienBetrag
      esLohn.FerienProz = salaryData.FerienAnsatz
      esLohn.Feier = salaryData.FeiertagBetrag
      esLohn.FeierProz = salaryData.FeiertagAnsatz
      esLohn.Basis13 = salaryData.Lohn13Basis
      esLohn.Lohn13 = salaryData.Lohn13Betrag
      esLohn.Lohn13Proz = salaryData.Lohn13Ansatz
      esLohn.Tarif = salaryData.Tarif
      esLohn.MAStdSpesen = salaryData.Lohnspesen
      esLohn.MATSpesen = salaryData.TagespesenMA
      esLohn.KDTSpesen = salaryData.TagesspesenKD
      esLohn.MATotal = salaryData.StundenLohn + salaryData.TagespesenMA
      esLohn.KDTotal = salaryData.Tarif + salaryData.TagesspesenKD + salaryData.MwStBetrag
      esLohn.MWSTBetrag = salaryData.MwStBetrag
      esLohn.BruttoMarge = salaryData.MargeData.MargeOhneBVG
      esLohn.LOVon = If(salaryData.LOVon.HasValue, salaryData.LOVon.Value.Date, DateTime.Now.Date)
      esLohn.Result = String.Empty
      esLohn.AktivLODaten = 1
      esLohn.MargeMitBVG = salaryData.MargeData.MargeMitBVG
      esLohn.GAVNr = effectiveGAVData.GAVNr
      esLohn.GAVKanton = effectiveGAVData.GAVKanton
      esLohn.GAVGruppe0 = effectiveGAVData.GAVGruppe0
      esLohn.GAVGruppe1 = effectiveGAVData.GAVGruppe1
      esLohn.GAVGruppe2 = effectiveGAVData.GAVGruppe2
      esLohn.GAVGruppe3 = effectiveGAVData.GAVGruppe3
      esLohn.GAVBezeichnung = effectiveGAVData.GAVBezeichnung
      esLohn.GAV_FAG = effectiveGAVData.GAV_FAG
      esLohn.GAV_FAN = effectiveGAVData.GAV_FAN
      esLohn.GAV_WAG = effectiveGAVData.GAV_WAG
      esLohn.GAV_WAN = effectiveGAVData.GAV_WAN
      esLohn.GAV_VAG = effectiveGAVData.GAV_VAG
      esLohn.GAV_VAN = effectiveGAVData.GAV_VAN

      esLohn.GAV_StdWeek = effectiveGAVData.GAV_StdWeek
      esLohn.GAV_StdMonth = effectiveGAVData.GAV_StdMonth
      esLohn.GAV_StdYear = effectiveGAVData.GAV_StdYear
      esLohn.GAVStdLohn = effectiveGAVData.GAVStdLohn

      esLohn.GAV_FAG_S = effectiveGAVData.GAV_FAG_S
      esLohn.GAV_FAN_S = effectiveGAVData.GAV_FAN_S
      esLohn.GAV_WAG_S = effectiveGAVData.GAV_WAG_S
      esLohn.GAV_WAN_S = effectiveGAVData.GAV_WAN_S
      esLohn.GAV_VAG_S = effectiveGAVData.GAV_VAG_S
      esLohn.GAV_VAN_S = effectiveGAVData.GAV_VAN_S
      esLohn.GAV_FAG_M = effectiveGAVData.GAV_FAG_M
      esLohn.GAV_FAN_M = effectiveGAVData.GAV_FAN_M
      esLohn.GAV_WAG_M = effectiveGAVData.GAV_WAG_M
      esLohn.GAV_WAN_M = effectiveGAVData.GAV_WAN_M
      esLohn.GAV_VAG_M = effectiveGAVData.GAV_VAG_M
      esLohn.GAV_VAN_M = effectiveGAVData.GAV_VAN_M
      esLohn.GAV_FAG_J = effectiveGAVData.GAV_FAG_J
      esLohn.GAV_FAN_J = effectiveGAVData.GAV_FAN_J
      esLohn.GAV_WAG_J = effectiveGAVData.GAV_WAG_J
      esLohn.GAV_WAN_J = effectiveGAVData.GAV_WAN_J
      esLohn.GAV_VAG_J = effectiveGAVData.GAV_VAG_J
      esLohn.GAV_VAN_J = effectiveGAVData.GAV_VAN_J

      esLohn.FerienWay = salaryData.CalcFerienWay
      esLohn.LO13Way = salaryData.CalcLohn13Way
      esLohn.CreatedOn = createDate
      esLohn.CreatedFrom = m_InitializationData.UserData.UserFullName
      esLohn.ChangedOn = createDate
      esLohn.ChangedFrom = m_InitializationData.UserData.UserFullName
      esLohn.VerleihDoc_Guid = String.Empty
      esLohn.ESDoc_Guid = String.Empty
      esLohn.Transfered_User = String.Empty
      esLohn.Transfered_On = Nothing
      esLohn.IsPVL = effectiveGAVData.IsPVL
      esLohn.FeierBasis = salaryData.FeiertagBasis
      esLohn.GAVInfo_String = effectiveGAVData.GAVInfo_String
      esLohn.LOFeiertagWay = salaryData.CalcFeierWay
			esLohn.GavDate = createDate
			If Not salaryData.SelectedGAVStringData Is Nothing Then
				esLohn.PVLDatabaseName = salaryData.SelectedGAVStringData.PVLDatabaseName
			End If
			esLohn.MargenInfo_String = salaryData.MargeData.CompleteMargeString

			Return esLohn
    End Function

    ''' <summary>
    ''' Creates an ES gav data object.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="effectiveGAVData">The effective gav data.</param>
    ''' <param name="createDate">The create date.</param>
    ''' <returns>ES gav data boject.</returns>
    Private Function CreateESGAVDataObject(ByVal employeeNumber As Integer,
                                           ByVal customerNumber As Integer,
                                           ByVal effectiveGAVData As EffectiveGAVData,
                                           ByVal createDate As DateTime)





      Dim esLohnGAV As New ESSalaryGAVData
      esLohnGAV.EmployeeNumber = employeeNumber
      esLohnGAV.CustomerNumber = customerNumber
      esLohnGAV.GAVNr = effectiveGAVData.GAVNr
      esLohnGAV.Kanton = effectiveGAVData.GAVKanton
      esLohnGAV.Zusatz1 = String.Empty
      esLohnGAV.Zusatz2 = String.Empty
      esLohnGAV.Zusatz3 = String.Empty
      esLohnGAV.Zusatz4 = String.Empty
      esLohnGAV.Zusatz5 = String.Empty
      esLohnGAV.Zusatz6 = String.Empty
      esLohnGAV.Zusatz7 = String.Empty
      esLohnGAV.Zusatz8 = String.Empty
      esLohnGAV.Zusatz9 = String.Empty
      esLohnGAV.Zusatz10 = String.Empty
      esLohnGAV.Zusatz11 = String.Empty
      esLohnGAV.Zusatz12 = String.Empty
      esLohnGAV.Zusatz13 = String.Empty
      esLohnGAV.Zusatz14 = String.Empty
      esLohnGAV.Zusatz15 = String.Empty
      esLohnGAV.Zusatz16 = String.Empty
      esLohnGAV.Zusatz17 = String.Empty
      esLohnGAV.Zusatz18 = String.Empty
      esLohnGAV.Zusatz19 = String.Empty
      esLohnGAV.Zusatz20 = String.Empty
      esLohnGAV.CreatedOn = createDate
      esLohnGAV.CreatedFrom = m_InitializationData.UserData.UserFullName
      esLohnGAV.IsPVL = Convert.ToByte(effectiveGAVData.IsPVL)

      Return esLohnGAV
    End Function

    ''' <summary>
    ''' Creates an RP data object.
    ''' </summary>
    ''' <param name="mdNubmer">The mandant number.</param>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <param name="currency">The currency.</param>
    ''' <param name="farPflichtig">Far pflichtig value.</param>
    ''' <param name="esData">ES data.</param>
    ''' <param name="effectiveGAVData">Effective gav data.</param>
    ''' <param name="createDate">The creation date.</param>
    ''' <returns>RP data object.</returns>
    Private Function CreateRPDataObject(ByVal mdNubmer As Integer,
                                        ByVal employeeNumber As Integer,
                                        ByVal customerNumber As Integer,
                                        ByVal currency As String,
                                        ByVal farPflichtig As Boolean,
                                        ByVal esData As InitESData,
                                        ByVal effectiveGAVData As EffectiveGAVData,
                                        ByVal createDate As DateTime) As RPData




      ' -- RP --
      Dim rp As New RPData()

      rp.MANR = employeeNumber
      rp.KDNR = customerNumber
      rp.LONr = 0
      rp.Currency = currency
      rp.SUVA = esData.SUVA
      rp.Monat = esData.ESStartDate.Value.Month
      rp.Jahr = esData.ESStartDate.Value.Year
      rp.Von = esData.ESStartDate.Value.Date

      ' Calculate Bis date. 
      ' Take minimum 'end of month of start date' and 'end date'
      Dim esStartDate = esData.ESStartDate
      Dim esEndDate = esData.ESEndDate
      Dim yearStart = esStartDate.Value.Year
      Dim monthStart = esStartDate.Value.Month
      Dim endOfMonthStartDate As DateTime = New DateTime(yearStart, monthStart, DateTime.DaysInMonth(yearStart, monthStart)).Date
      rp.Bis = If(esEndDate.HasValue AndAlso esEndDate.Value.Date < endOfMonthStartDate, esEndDate.Value.Date, endOfMonthStartDate)

      rp.Erfasst = False
      rp.Result = String.Empty
      rp.RPKST = esData.MA_KD_Berater
      rp.RPKST1 = esData.Kst1
      rp.RPKST2 = esData.Kst2
      rp.PrintedWeeks = String.Empty
      rp.PrintedDate = String.Empty
      rp.FarPflicht = farPflichtig
      rp.BVGStd = 0D
      rp.CreatedFrom = m_InitializationData.UserData.UserFullName
      rp.CreatedOn = createDate
      rp.BVGCode = 9 ' Automaticher Abzug
      rp.RPGAV_FAG = effectiveGAVData.GAV_FAG
      rp.RPGAV_FAN = effectiveGAVData.GAV_FAN
      rp.RPGAV_WAG = effectiveGAVData.GAV_WAG
      rp.RPGAV_WAN = effectiveGAVData.GAV_WAN
      rp.RPGAV_VAG = effectiveGAVData.GAV_VAG
      rp.RPGAV_VAN = effectiveGAVData.GAV_VAN
      rp.RPGAV_Nr = effectiveGAVData.GAVNr
      rp.RPGAV_Kanton = effectiveGAVData.GAVKanton
      rp.RPGAV_Beruf = effectiveGAVData.GAVGruppe0
      rp.RPGAV_Gruppe1 = effectiveGAVData.GAVGruppe1
      rp.RPGAV_Gruppe2 = effectiveGAVData.GAVGruppe2
      rp.RPGAV_Gruppe3 = effectiveGAVData.GAVGruppe3
      rp.RPGAV_Text = effectiveGAVData.GAVBezeichnung
      rp.RPGAV_StdWeek = effectiveGAVData.GAV_StdWeek
      rp.RPGAV_StdMonth = effectiveGAVData.GAV_StdMonth
      rp.RPGAV_StdYear = effectiveGAVData.GAV_StdYear
      rp.RPGAV_FAG_M = effectiveGAVData.GAV_FAG_M
      rp.RPGAV_FAN_M = effectiveGAVData.GAV_FAN_M
      rp.RPGAV_VAG_M = effectiveGAVData.GAV_VAG_M
      rp.RPGAV_VAN_M = effectiveGAVData.GAV_VAN_M
      rp.RPGAV_WAG_M = effectiveGAVData.GAV_WAG_M
      rp.RPGAV_WAN_M = effectiveGAVData.GAV_WAN_M
      rp.RPGAV_FAG_S = effectiveGAVData.GAV_FAG_S
      rp.RPGAV_FAN_S = effectiveGAVData.GAV_FAN_S
      rp.RPGAV_VAG_S = effectiveGAVData.GAV_VAG_S
      rp.RPGAV_VAN_S = effectiveGAVData.GAV_VAN_S
      rp.RPGAV_WAG_S = effectiveGAVData.GAV_WAG_S
      rp.RPGAV_WAN_S = effectiveGAVData.GAV_WAN_S
      rp.RPGAV_FAG_J = effectiveGAVData.GAV_FAG_J
      rp.RPGAV_FAN_J = effectiveGAVData.GAV_FAN_J
      rp.RPGAV_VAG_J = effectiveGAVData.GAV_VAG_J
      rp.RPGAV_VAN_J = effectiveGAVData.GAV_VAN_J
      rp.RPGAV_WAG_J = effectiveGAVData.GAV_WAG_J
      rp.RPGAV_WAN_J = effectiveGAVData.GAV_WAN_J
      rp.ES_Einstufung = If(esData.ESEinstufung Is Nothing, String.Empty, esData.ESEinstufung)
      rp.KDBranche = If(esData.Branche Is Nothing, String.Empty, esData.Branche)
      rp.ProposeNr = 0
      rp.RPDoc_Guid = String.Empty
      rp.MDNr = mdNubmer

      Return rp
    End Function

    ''' <summary>
    ''' Reads the ES offset from the settings.
    ''' </summary>
    ''' <returns>ES offset or zero if it could not be read.</returns>
    Private Function ReadESOffsetFromSettings() As Integer

      Dim strQuery As String = "//StartNr/Einsatzverwaltung"
      Dim r = m_ClsProgSetting.GetUserProfileFile
      Dim esNumberStartNumberSetting As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "0")
      Dim intVal As Integer

      If Integer.TryParse(esNumberStartNumberSetting, intVal) Then
        Return intVal
      Else
        Return 0
      End If

    End Function

    ''' <summary>
    ''' Reads the RP offset from the settings.
    ''' </summary>
    ''' <returns>RP offset or zero if it could not be read.</returns>
    Private Function ReadRPOffsetFromSettings() As Integer

      Dim strQuery As String = "//StartNr/Rapporte"
      Dim r = m_ClsProgSetting.GetUserProfileFile
      Dim rapporteStartNumberSetting As String = m_ClsProgSetting.GetXMLValueByQuery(m_ClsProgSetting.GetMDData_XMLFile, strQuery, "0")
      Dim intVal As Integer

      If Integer.TryParse(rapporteStartNumberSetting, intVal) Then
        Return intVal
      Else
        Return 0
      End If

    End Function

#End Region

  End Class

End Namespace
