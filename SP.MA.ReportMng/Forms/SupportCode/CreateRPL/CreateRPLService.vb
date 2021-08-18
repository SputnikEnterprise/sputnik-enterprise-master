Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.MA.ReportMng.TimeTable
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Report.DataObjects.RPAbsenceDaysData
Imports SP.Infrastructure
Imports SPProgUtility.CommonXmlUtility
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer

Public Class CreateRPLService

#Region "Private Constants"

	Private Const MANDANT_XML_SETTING_SPUTNIK_TAGESPESENSTDAB_KEY As String = "MD_{0}/Lohnbuchhaltung/report/tagesspesenstdab"

#End Region


#Region "Private Fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private m_MandantSettingsXml As SettingsXml
	Private m_TagesspesenStdAb As Decimal

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Protected m_UtilityUI As UtilityUI

	Protected m_Utility As Utility


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
	Protected m_ReportDataAccess As IReportDatabaseAccess

	''' <summary>
	''' The data access object.
	''' </summary>
	Protected m_EmployeeDataAccess As IEmployeeDatabaseAccess

	''' <summary>
	''' The data access object.
	''' </summary>
	Protected m_CustomerDataAccess As ICustomerDatabaseAccess

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

	Private m_CustomerRPLNumber As Integer?

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
		m_Utility = New Utility

		m_MandantSettingsXml = New SettingsXml(m_md.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year))
		m_TagesspesenStdAb = m_Utility.ParseToDec(m_MandantSettingsXml.GetSettingByKey(String.Format(MANDANT_XML_SETTING_SPUTNIK_TAGESPESENSTDAB_KEY, m_InitializationData.MDData.MDNr)), 8.25)

		Dim conStr = m_md.GetSelectedMDData(mdNr).MDDbConn
		m_ReportDataAccess = New ReportDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDataAccess = New EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_CustomerDataAccess = New CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

	End Sub

#End Region

#Region "Public Methods"

	''' <summary>
	''' looks for duplicate LANr in RPL for same time
	''' </summary>
	''' <param name="rpNr"></param>
	''' <param name="lanr"></param>
	''' <param name="vonDate"></param>
	''' <param name="bisDate"></param>
	''' <param name="isKd"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function ExistsLANrForSameTime(ByVal rpNr As Integer, ByVal lanr As Decimal, ByVal vonDate As DateTime, ByVal bisDate As DateTime, ByVal isKd As Boolean) As Boolean

		Dim success As Boolean = True

		Dim reportDBBase As DatabaseAccessBase = CType(m_ReportDataAccess, DatabaseAccessBase)

		success = m_ReportDataAccess.ExistsRPLLADataForPeriode(rpNr, lanr, vonDate, bisDate, isKd)
		If success Then
			Dim msg As String = m_Translate.GetSafeTranslationValue("Die ausgewählte Lohnart wurde bereits erfasst: {0}{1:F3}: {2:d} - {3:d}{0}{0}Möchten Sie die Lohnart trotzdem erfassen?")
			msg = String.Format(msg, vbNewLine, lanr, vonDate, bisDate)
			success = m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Lohnarten erfassen"), MessageBoxDefaultButton.Button2) = False
		End If

		Return success

	End Function

	''' <summary>
	''' Creates an employee RPL.
	''' </summary>
	''' <param name="params">The parameters.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function CreateEmployeeRPL(ByVal params As CreateEmployeeRPLParams) As Boolean

		Dim success As Boolean = True

		Dim reportDBBase As DatabaseAccessBase = CType(m_ReportDataAccess, DatabaseAccessBase)

		Dim isExplicitConnectionAlreadyOpenOnMethodEntry As Boolean = reportDBBase.IsExplicitConnectionOpen
		Dim isExplicitTransactionAlreadyOpenOnMethodEntry As Boolean = reportDBBase.IsExplicitTransactionAvailable

		Try

			' Only open explicit connection if it is not already open
			If (Not isExplicitConnectionAlreadyOpenOnMethodEntry) AndAlso
				Not reportDBBase.OpenExplicitConnection Then
				success = False
				Return success
			End If

			' Only open an explicit transactin if it is not already open
			If (Not isExplicitTransactionAlreadyOpenOnMethodEntry) AndAlso
				 Not reportDBBase.OpenExplicitTransaction Then
				success = False
				Return success
			End If

			Dim iniData As New NewEmployeeRPLInitData

			Dim nextFreeRPLNr = LoadNextFreeRPLNumber(params.RPNr)

			If nextFreeRPLNr Is Nothing Then
				success = False
				Return success
			End If

			iniData.RPNr = params.RPNr
			iniData.RPLNr = nextFreeRPLNr
			iniData.KDNr = params.KDNr
			iniData.MANr = params.MANr
			iniData.ESNr = params.ESNr
			iniData.KSTNR = params.KSTNR
			iniData.KstBez = params.KstBez
			iniData.GAVText = params.GAVText
			iniData.Currency = params.Currency
			iniData.LANr = params.LANr
			iniData.M_Anzahl = params.M_Anzahl
			iniData.M_Basis = params.M_Basis
			iniData.M_Ansatz = params.M_Ansatz
			iniData.SUVA = params.SUVA
			iniData.M_Ferien = params.M_Ferien
			iniData.M_Feier = params.M_Feier
			iniData.M_13 = params.M_13
			iniData.VonDate = params.VonDate
			iniData.BisDate = params.BisDate
			iniData.FerBas = params.FerBas
			iniData.Basis13 = params.Basis13
			iniData.ESLohnNr = params.ESLohnNr
			iniData.LOSpesenBas = params.LOSpesenBas
			iniData.LOSpesen = params.LOSpesen
			iniData.MATSpesenBas = params.MATSpesenBas
			iniData.MATSpesen = params.MATSpesen
			iniData.StdTotal = params.StdTotal
			iniData.FeierTotal = params.FeierTotal
			iniData.FerTotal = params.FerTotal
			iniData.Total13 = params.Total13
			iniData.UserName = params.UserName
			iniData.IsPVL = params.IsPVL
			iniData.RPZusatzText = params.RPZusatzText
			iniData.KompStd = 0.0
			iniData.KompBetrag = 0.0

			Dim rplDayData As RPLDayData = Nothing
			Dim rplAbsenceDayData As RPAbsenceDaysData = Nothing

			If Not params.TimeTable Is Nothing Then

				If params.IsFlexibleTimeActive Then
					' Flexible time is active
					Dim workignHoursAndFlexibleTime = GetWorkingHoursAndFlexibleTime(params.TimeTable, params.RPGAVNr, params.RPGAV_StdWeek, params.RPJahr, params.MDNr)

					iniData.KompStd = workignHoursAndFlexibleTime.SumFlexibleTime
					iniData.KompBetrag = workignHoursAndFlexibleTime.SumFlexibleTime * params.Stundenlohn
				Else
					iniData.KompStd = 0D
					iniData.KompBetrag = 0D
				End If

				success = success AndAlso AddNewEmployeeRPLDayData(params, nextFreeRPLNr)
				success = success AndAlso AddOrUpdateAbsenceDayData(params.RPNr, Convert.ToInt32(params.RPJahr), params.RPMonat, params.TimeTable)

			End If

			If success Then
				Dim spesenDays As Integer? = GetNumberOfSpesenDays(params.RPNr, RPLType.Employee, params.RPGAVNr, params.RPGAV_StdWeek, Convert.ToInt32(params.RPJahr))
				success = success AndAlso spesenDays.HasValue
				iniData.TAnzahl = If(spesenDays.HasValue, spesenDays.Value, 0)
			End If

			success = success AndAlso m_ReportDataAccess.AddNewEmployeeRPLData(iniData)

			' Create a duplication for the customer if parameters are passed.
			If Not params.AdditionalParamsForCustomerRPLDuplication Is Nothing Then
				success = success AndAlso CreateCustomerRPLDuplicateOfEmployeeRPL(params)
			End If

			If success Then
				params.NewRPLNr = nextFreeRPLNr
				params.NewCustomerDuplicatedRPLNr = m_CustomerRPLNumber
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			success = False
		Finally

			If Not isExplicitTransactionAlreadyOpenOnMethodEntry Then
				If success Then
					' Commit  explicit transaction.
					If Not reportDBBase.CommitExplicitTransaction Then
						success = False
					End If
				Else
					' Rollback explicit transaction.
					reportDBBase.RollbackExplicitTransaction()
				End If
			End If

			If Not isExplicitConnectionAlreadyOpenOnMethodEntry Then
				' Close explicit connection.
				If Not reportDBBase.CloseExplicitConnection Then
					success = False
				End If
			End If

		End Try

		Return success
	End Function


	''' <summary>
	''' Creates a customer RPL.
	''' </summary>
	''' <param name="params">The parameters.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function CreateCustomerRPL(ByVal params As CreateCustomerRPLParams) As Boolean

		Dim success As Boolean = True

		Dim reportDBBase As DatabaseAccessBase = CType(m_ReportDataAccess, DatabaseAccessBase)

		Dim isExplicitConnectionAlreadyOpenOnMethodEntry As Boolean = reportDBBase.IsExplicitConnectionOpen
		Dim isExplicitTransactionAlreadyOpenOnMethodEntry As Boolean = reportDBBase.IsExplicitTransactionAvailable

		Try

			' Only open explicit connection if it is not already open
			If (Not isExplicitConnectionAlreadyOpenOnMethodEntry) AndAlso
				Not reportDBBase.OpenExplicitConnection Then
				success = False
				Return success
			End If

			' Only open an explicit transactin if it is not already open
			If (Not isExplicitTransactionAlreadyOpenOnMethodEntry) AndAlso
				 Not reportDBBase.OpenExplicitTransaction Then
				success = False
				Return success
			End If

			Dim iniData As New NewCustomerRPLInitData

			Dim nextFreeRPLNr = LoadNextFreeRPLNumber(params.RPNr)

			If nextFreeRPLNr Is Nothing Then
				success = False
				Return success
			End If

			iniData.RPNr = params.RPNr
			iniData.RPLNr = nextFreeRPLNr
			iniData.KDNr = params.KDNr
			iniData.MANr = params.MANr
			iniData.ESNr = params.ESNr
			iniData.KSTNR = params.KSTNR
			iniData.KstBez = params.KstBez
			iniData.GAVText = params.GAVText
			iniData.Currency = params.Currency
			iniData.LANr = params.LANr
			iniData.K_Anzahl = params.K_Anzahl
			iniData.K_Basis = params.K_Basis
			iniData.K_Ansatz = params.K_Ansatz
			iniData.MWST = params.MWST
			iniData.SUVA = params.SUVA
			iniData.VonDate = params.VonDate
			iniData.BisDate = params.BisDate
			iniData.ESLohnNr = params.ESLohnNr
			iniData.KDTSpesenBas = params.KDTSpesenBas
			iniData.KDTSpesen = params.KDTSpesen
			iniData.KDBetrag = params.KDBetrag
			iniData.UserName = params.UserName
			iniData.RPZusatzText = params.RPZusatzText
			iniData.IsCreatedWithEmployee = params.IsCreatedWithEmployee

			Dim rplDayData As RPLDayData = Nothing
			Dim rplAbsenceDayData As RPAbsenceDaysData = Nothing

			If Not params.TimeTable Is Nothing Then

				success = success AndAlso AddNewCustomerRPLDayData(params, nextFreeRPLNr)
				success = success AndAlso AddOrUpdateAbsenceDayData(params.RPNr, Convert.ToInt32(params.RPJahr), params.RPMonat, params.TimeTable)

			End If

			If success Then
				Dim spesenDays As Integer? = GetNumberOfSpesenDays(params.RPNr, RPLType.Customer, params.RPGAVNr, params.RPGAV_StdWeek, Convert.ToInt32(params.RPJahr))
				success = success AndAlso spesenDays.HasValue
				iniData.TAnzahl = If(spesenDays.HasValue, spesenDays.Value, 0)
			End If

			success = success AndAlso m_ReportDataAccess.AddNewCustomerRPLData(iniData)

			If success Then
				params.NewRPLNr = nextFreeRPLNr
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			success = False
		Finally

			If Not isExplicitTransactionAlreadyOpenOnMethodEntry Then
				If success Then
					' Commit  explicit transaction.
					If Not reportDBBase.CommitExplicitTransaction Then
						success = False
					End If
				Else
					' Rollback explicit transaction.
					reportDBBase.RollbackExplicitTransaction()
				End If
			End If

			If Not isExplicitConnectionAlreadyOpenOnMethodEntry Then
				' Close explicit connection.
				If Not reportDBBase.CloseExplicitConnection Then
					success = False
				End If
			End If

		End Try

		Return success

	End Function

	''' <summary>
	''' Updates a employee RPL.
	''' </summary>
	''' <param name="params">The parameters.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function UpdateEmployeeRPLData(ByVal params As UpdateEmployeeRPLParams) As Boolean

		Dim success As Boolean = True

		Dim reportDBBase As DatabaseAccessBase = CType(m_ReportDataAccess, DatabaseAccessBase)

		Dim isExplicitConnectionAlreadyOpenOnMethodEntry As Boolean = reportDBBase.IsExplicitConnectionOpen
		Dim isExplicitTransactionAlreadyOpenOnMethodEntry As Boolean = reportDBBase.IsExplicitTransactionAvailable

		Try

			' Only open explicit connection if it is not already open
			If (Not isExplicitConnectionAlreadyOpenOnMethodEntry) AndAlso
				Not reportDBBase.OpenExplicitConnection Then
				success = False
				Return success
			End If

			' Only open an explicit transactin if it is not already open
			If (Not isExplicitTransactionAlreadyOpenOnMethodEntry) AndAlso
				 Not reportDBBase.OpenExplicitTransaction Then
				success = False
				Return success
			End If

			Dim updateData As New UpdateEmployeeRPLData

			updateData.RPNr = params.RPNr
			updateData.RPLNr = params.RPLNr
			updateData.LANr = params.LANr
			updateData.M_Anzahl = params.M_Anzahl
			updateData.M_Basis = params.M_Basis
			updateData.M_Ansatz = params.M_Ansatz
			updateData.M_Ferien = params.M_Ferien
			updateData.M_Feier = params.M_Feier
			updateData.M_13 = params.M_13
			updateData.VonDate = params.VonDate
			updateData.BisDate = params.BisDate
			updateData.ESLohnNr = params.ESLohnNr
			updateData.LOSpesenBas = params.LOSpesenBas
			updateData.LOSpesen = params.LOSpesen
			updateData.StdTotal = params.StdTotal
			updateData.FeierTotal = params.FeierTotal
			updateData.FerTotal = params.FerTotal
			updateData.Total13 = params.Total13
			updateData.FerBas = params.FerBas
			updateData.Basis13 = params.Basis13
			updateData.MATSpesenBas = params.MATSpesenBas
			updateData.MATSpesen = params.MATSpesen
			updateData.KstNr = params.KstNr
			updateData.KstBez = params.KstBez
			updateData.KompStd = params.KompStd
			updateData.KompBetrag = params.KompBetrag
			updateData.UserName = params.UserName
			updateData.IsPVL = params.IsPVL
			updateData.RPZusatzText = params.RPZusatzText

			If Not params.TimeTable Is Nothing Then
				success = success AndAlso UpdateEmployeeRPLDayData(params)
				success = success AndAlso AddOrUpdateAbsenceDayData(params.RPNr, Convert.ToInt32(params.RPJahr), params.RPMonat, params.TimeTable)
			End If

			If success Then
				Dim spesenDays As Integer? = GetNumberOfSpesenDays(params.RPNr, RPLType.Employee, params.RPGAVNr, params.RPGAV_StdWeek, Convert.ToInt32(params.RPJahr))
				success = success AndAlso spesenDays.HasValue
				updateData.TAnzahl = If(spesenDays.HasValue, spesenDays.Value, 0)
			End If

			success = success AndAlso m_ReportDataAccess.UpdateEmployeeRPLData(updateData)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			success = False
		Finally
			If Not isExplicitTransactionAlreadyOpenOnMethodEntry Then
				If success Then
					' Commit  explicit transaction.
					If Not reportDBBase.CommitExplicitTransaction Then
						success = False
					End If
				Else
					' Rollback explicit transaction.
					reportDBBase.RollbackExplicitTransaction()
				End If
			End If

			If Not isExplicitConnectionAlreadyOpenOnMethodEntry Then
				' Close explicit connection.
				If Not reportDBBase.CloseExplicitConnection Then
					success = False
				End If
			End If
		End Try

		Return success
	End Function

	''' <summary>
	''' Updates a customer RPL.
	''' </summary>
	''' <param name="params">The parameters.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function UpdateCustomerRPLData(ByVal params As UpdateCustomerRPLParams) As Boolean

		Dim success As Boolean = True

		Dim reportDBBase As DatabaseAccessBase = CType(m_ReportDataAccess, DatabaseAccessBase)

		Dim isExplicitConnectionAlreadyOpenOnMethodEntry As Boolean = reportDBBase.IsExplicitConnectionOpen
		Dim isExplicitTransactionAlreadyOpenOnMethodEntry As Boolean = reportDBBase.IsExplicitTransactionAvailable

		Try

			' Only open explicit connection if it is not already open
			If (Not isExplicitConnectionAlreadyOpenOnMethodEntry) AndAlso
				Not reportDBBase.OpenExplicitConnection Then
				success = False
				Return success
			End If

			' Only open an explicit transactin if it is not already open
			If (Not isExplicitTransactionAlreadyOpenOnMethodEntry) AndAlso
				 Not reportDBBase.OpenExplicitTransaction Then
				success = False
				Return success
			End If

			Dim updateData As New UpdateCustomerRPLData

			updateData.RPNr = params.RPNr
			updateData.RPLNr = params.RPLNr
			updateData.LANr = params.LANr
			updateData.K_Anzahl = params.K_Anzahl
			updateData.K_Basis = params.K_Basis
			updateData.K_Ansatz = params.K_Ansatz
			updateData.MWST = params.MWST
			updateData.VonDate = params.VonDate
			updateData.BisDate = params.BisDate
			updateData.ESLohnNr = params.ESLohnNr
			updateData.KDBetrag = params.KDBetrag
			updateData.MATSpesenBas = params.MATSpesenBas
			updateData.MATSpesen = params.MATSpesen
			updateData.KstNr = params.KstNr
			updateData.KstBez = params.KstBez
			updateData.UserName = params.UserName
			updateData.RPZusatzText = params.RPZusatzText

			If Not params.TimeTable Is Nothing Then
				success = success AndAlso UpdateCustomerRPLDayData(params)
				success = success AndAlso AddOrUpdateAbsenceDayData(params.RPNr, Convert.ToInt32(params.RPJahr), params.RPMonat, params.TimeTable)
			End If

			If success Then
				Dim spesenDays As Integer? = GetNumberOfSpesenDays(params.RPNr, RPLType.Customer, params.RPGAVNr, params.RPGAV_StdWeek, Convert.ToInt32(params.RPJahr))
				success = success AndAlso spesenDays.HasValue
				updateData.TAnzahl = If(spesenDays.HasValue, spesenDays.Value, 0)
			End If

			success = success AndAlso m_ReportDataAccess.UpdateCustomerRPLData(updateData)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			success = False
		Finally
			If Not isExplicitTransactionAlreadyOpenOnMethodEntry Then
				If success Then
					' Commit  explicit transaction.
					If Not reportDBBase.CommitExplicitTransaction Then
						success = False
					End If
				Else
					' Rollback explicit transaction.
					reportDBBase.RollbackExplicitTransaction()
				End If
			End If

			If Not isExplicitConnectionAlreadyOpenOnMethodEntry Then
				' Close explicit connection.
				If Not reportDBBase.CloseExplicitConnection Then
					success = False
				End If
			End If
		End Try

		Return success
	End Function

	''' <summary>
	''' Deletes an employee RPL data.
	''' </summary>
	''' <param name="rpNr">The report number.</param>
	''' <param name="rpYear">The report year.</param>
	''' <param name="rpMonth">The report month.</param>
	''' <param name="rplNr">The RPL number.</param>
	''' <param name="esLohnNr">The es lohn number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function DeleteEmployeeRPLData(ByVal rpNr As Integer, ByVal rpYear As Integer, ByVal rpMonth As Integer,
																				ByVal rplNr As Integer, ByVal esLohnNr As Integer, ByVal rpGAVNr As Integer, ByVal rpGav_StdWeek As Integer) As DeleteMARPLDataResult

		Dim scanDocGuid = m_ReportDataAccess.GetRPLScanDocGuid(rpNr, rplNr)
		Dim result = m_ReportDataAccess.DeleteEmployeeRPLData(rpNr, rplNr)

		Select Case result
			Case DeleteMARPLDataResult.ResultDeleteOk

				Dim spesenDays As Integer? = GetNumberOfSpesenDays(rpNr, RPLType.Employee, rpGAVNr, rpGav_StdWeek, rpYear)

				Dim success As Boolean = m_ReportDataAccess.UpdateEmployeeRPLTSpesenData(rpNr, If(spesenDays.HasValue, spesenDays.Value, 0), esLohnNr)
				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Aktualisierung der Tagespesen ist fehlgeschlagen."))
				End If

				success = m_ReportDataAccess.CorrectRPAbsenceDaysDataAfterDeleteOfRPL(rpNr, rpYear, rpMonth)
				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Aktualisierung der Fehltage ist fehlgeschlagen."))
				End If

				If success AndAlso Not String.IsNullOrWhiteSpace(scanDocGuid) Then

					Dim resultWOS = DeleteEmployeeReportDocumentFromWOS(rpNr, scanDocGuid)
					resultWOS = resultWOS AndAlso DeleteCustomerReportDocumentFromWOS(rpNr, scanDocGuid)
					If Not resultWOS Then m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das Dokument in WOS konnte nicht entfernt werden!"))

				End If
		End Select

		Return result
	End Function

	''' <summary>
	''' Deletes an customer RPL data.
	''' </summary>
	''' <param name="rpNr">The report number.</param>
	''' <param name="rpYear">The report year.</param>
	''' <param name="rpMonth">The report month.</param>
	''' <param name="rplNr">The RPL number.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function DeleteCustomerRPLData(ByVal rpNr As Integer, ByVal rpYear As Integer, ByVal rpMonth As Integer, ByVal rplNr As Integer) As DeleteKDRPLDataResult

		Dim scanDocGuid = m_ReportDataAccess.GetRPLScanDocGuid(rpNr, rplNr)
		Dim result = m_ReportDataAccess.DeleteCustomerRPLData(rpNr, rplNr)

		Select Case result
			Case DeleteKDRPLDataResult.ResultDeleteOk

				Dim success As Boolean = m_ReportDataAccess.CorrectRPAbsenceDaysDataAfterDeleteOfRPL(rpNr, rpYear, rpMonth)
				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Aktualisierung der Fehltage ist fehlgeschlagen."))
				End If

				If success AndAlso Not String.IsNullOrWhiteSpace(scanDocGuid) Then

					Dim resultWOS = DeleteEmployeeReportDocumentFromWOS(rpNr, scanDocGuid)
					resultWOS = resultWOS AndAlso DeleteCustomerReportDocumentFromWOS(rpNr, scanDocGuid)
					If Not resultWOS Then m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das Dokument in WOS konnte nicht entfernt werden!"))

				End If

		End Select

		Return result
	End Function

	Private Function DeleteEmployeeReportDocumentFromWOS(ByVal rpNr As Integer, ByVal scanDocGuid As String) As Boolean
		Dim result As Boolean = True
		Dim wos = New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitializationData)
		Dim wosSetting = New WOSSendSetting

		Dim rpData = m_ReportDataAccess.LoadRPMasterData(rpNr)
		Dim employeeData = m_EmployeeDataAccess.LoadEmployeeMasterData(rpData.EmployeeNumber, False)
		wosSetting.EmployeeNumber = rpData.EmployeeNumber
		wosSetting.EmployeeGuid = employeeData.Transfered_Guid
		wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
		wosSetting.ReportDocumentNumber = rpNr
		wosSetting.AssignedDocumentGuid = scanDocGuid
		wosSetting.DocumentInfo = String.Format("Rapport: {0}", rpNr)

		wos.WOSSetting = wosSetting

		result = result AndAlso wos.DeleteTransferedEmployeeDocument()


		Return result

	End Function

	Private Function DeleteCustomerReportDocumentFromWOS(ByVal rpNr As Integer, ByVal scanDocGuid As String) As Boolean
		Dim result As Boolean = True
		Dim wos = New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)
		Dim wosSetting = New WOSSendSetting

		Dim rpData = m_ReportDataAccess.LoadRPMasterData(rpNr)
		Dim customerData = m_CustomerDataAccess.LoadCustomerMasterData(rpData.CustomerNumber, "%%")

		wosSetting.CustomerNumber = rpData.CustomerNumber
		wosSetting.CustomerGuid = customerData.Transfered_Guid
		wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
		wosSetting.ReportDocumentNumber = rpNr
		wosSetting.AssignedDocumentGuid = scanDocGuid
		wosSetting.DocumentInfo = String.Format("Rapport: {0}", rpNr)

		wos.WOSSetting = wosSetting

		result = result AndAlso wos.DeleteTransferedCustomerDocument()


		Return result

	End Function

#End Region


#Region "Private Methods"

	''' <summary>
	''' Loads the next free RPL number.
	''' </summary>
	''' <param name="rpNr">The report number.</param>
	''' <returns>The next free RPL number.</returns>
	Private Function LoadNextFreeRPLNumber(ByVal rpNr As Integer) As Integer?

		Dim nextRPLNr = m_ReportDataAccess.GetNextFeeRPLNumber(rpNr)

		Return nextRPLNr

	End Function

	''' <summary>
	''' Adds new employee RPL day data.
	''' </summary>
	''' <param name="params">The create employee RPL params.</param>
	''' <param name="rplNr">The rplNr.</param>
	''' <returns>Boolen flag indicating success.</returns>
	Private Function AddNewEmployeeRPLDayData(ByVal params As CreateEmployeeRPLParams, ByVal rplNr As Integer) As Boolean

		Dim kumulativStd As Double = 0
		Dim workingHoursList As IEnumerable(Of DateAndHourData) = Nothing

		If params.IsFlexibleTimeActive Then
			' Flexible time is active.
			Dim workingHoursAndFexibleTime = GetWorkingHoursAndFlexibleTime(params.TimeTable, params.RPGAVNr, params.RPGAV_StdWeek, params.RPJahr, params.MDNr)

			kumulativStd = workingHoursAndFexibleTime.SumRegularWorkingHours
			workingHoursList = workingHoursAndFexibleTime.WorkingHoursWithoutFlexTime
		Else
			kumulativStd = params.TimeTable.SumHourData
			workingHoursList = params.TimeTable.GetDateAndHourData
		End If

		Dim rplDayData = New RPLDayData() With {
			.RPNr = params.RPNr,
			.RPLNr = rplNr,
			.EmployeeNumber = params.MANr,
			.CustomerNumber = params.KDNr,
			.ESNr = params.ESNr,
			.Monat = params.RPMonat,
			.Jahr = params.RPJahr,
			.KumulativStd = kumulativStd,
			.KstNr = params.KSTNR,
			.KstBez = params.KstBez,
			.ESLohnNr = params.ESLohnNr
			}

		rplDayData.ApplyWorkingHours(workingHoursList)

		Dim success As Boolean = True

		success = m_ReportDataAccess.AddNewEmployeeRPLDayData(rplDayData)

		Return success
	End Function

	''' <summary>
	''' Adds new customer RPL day data.
	''' </summary>
	''' <param name="params">The create customer RPL params.</param>
	''' <param name="rplNr">The rplNr.</param>
	''' <returns>Boolen flag indicating success.</returns>
	Private Function AddNewCustomerRPLDayData(ByVal params As CreateCustomerRPLParams, ByVal rplNr As Integer) As Boolean

		Dim workingHours = params.TimeTable.GetDateAndHourData
		Dim sumWokringHours = params.TimeTable.SumHourData

		Dim rplDayData = New RPLDayData() With {
			.RPNr = params.RPNr,
			.RPLNr = rplNr,
			.EmployeeNumber = params.MANr,
			.CustomerNumber = params.KDNr,
			.ESNr = params.ESNr,
			.Monat = params.RPMonat,
			.Jahr = params.RPJahr,
			.KumulativStd = sumWokringHours,
			.KstNr = params.KSTNR,
			.KstBez = params.KstBez,
			.ESLohnNr = params.ESLohnNr
			}

		rplDayData.ApplyWorkingHours(workingHours)

		Dim success As Boolean = True

		success = m_ReportDataAccess.AddNewCustomerRPLDayData(rplDayData)

		Return success
	End Function

	''' <summary>
	''' Adds or updates absence day data.
	''' </summary>
	''' <param name="rpNr">The report number.</param>
	''' <param name="rpYear">The report year.</param>
	''' <param name="rpMonth">The report month.</param>
	''' <param name="timetable">The time table data.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function AddOrUpdateAbsenceDayData(ByVal rpNr As Integer, ByVal rpYear As Integer, ByVal rpMonth As Byte, ByVal timetable As TimeTable) As Boolean
		Dim doesAbsenceDayDataExists = m_ReportDataAccess.ExistsRPAbsenceDaysDataForRP(rpNr)

		If Not doesAbsenceDayDataExists.HasValue Then
			Return False
		End If

		If doesAbsenceDayDataExists Then
			Return UpdateAbsenceDayData(rpNr, timetable)
		Else
			Return AddNewAbsenceDayData(rpNr, rpYear, rpMonth, timetable)
		End If

	End Function

	''' <summary>
	''' Adds new absence day data.
	''' </summary>
	''' <param name="rpNr">The report number.</param>
	''' <param name="rpYear">The report year.</param>
	''' <param name="rpMonth">The report month.</param>
	''' <param name="timetable">The time table data.</param>
	''' <returns>Boolean flag indicating succes.</returns>
	Private Function AddNewAbsenceDayData(ByVal rpNr As Integer, ByVal rpYear As Integer, ByVal rpMonth As Byte, ByVal timetable As TimeTable) As Boolean
		Dim rplAbsenceDayData As RPAbsenceDaysData = New RPAbsenceDaysData With {
					.RPNr = rpNr,
					.RPNr2 = Nothing}

		rplAbsenceDayData.InitAbsenceDayData(rpYear, rpMonth)

		Dim absenceCodes = timetable.GetDateAndAbsenceCodeData()
		rplAbsenceDayData.ApplyAbsenceCodeData(absenceCodes)

		Dim success As Boolean = m_ReportDataAccess.AddNewAbsenceDayData(rplAbsenceDayData)
		Return success
	End Function

	''' <summary>
	''' Updates absence day data.
	''' </summary>
	''' <param name="rpNr">The report number.</param>
	''' <param name="timetable">The time table.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function UpdateAbsenceDayData(ByVal rpNr As Integer, ByVal timetable As TimeTable) As Boolean

		Dim rplAbsenceDayData = m_ReportDataAccess.LoadRPAbsenceDaysData(rpNr)

		If (rplAbsenceDayData Is Nothing) Then
			Return False
		End If

		Dim absenceCodes = timetable.GetDateAndAbsenceCodeData()
		rplAbsenceDayData.ApplyAbsenceCodeData(absenceCodes)

		Dim success As Boolean = m_ReportDataAccess.UpdateRPAbsenceDaysData(rplAbsenceDayData)

		Return success
	End Function

	''' <summary>
	''' Updates employee RPL day data.
	''' </summary>
	''' <param name="params">The update employee RPL params.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function UpdateEmployeeRPLDayData(ByVal params As UpdateEmployeeRPLParams) As Boolean

		Dim rplDayDataList = m_ReportDataAccess.LoadRPLDayData(params.RPNr, RPLType.Employee, params.RPLNr)

		If Not rplDayDataList.Count = 1 Then
			Return False
		End If

		Dim employeeRPLDayData = rplDayDataList(0)

		Dim workingHours = params.TimeTable.GetDateAndHourData()
		employeeRPLDayData.ApplyWorkingHours(workingHours)
		employeeRPLDayData.KumulativStd = params.TimeTable.SumHourData()

		Dim success As Boolean = True

		success = m_ReportDataAccess.UpdateEmployeeRPLDayData(employeeRPLDayData)

		Return success

	End Function

	''' <summary>
	''' Upates customer RPL day data.
	''' </summary>
	''' <param name="params">The update customer RPL params.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function UpdateCustomerRPLDayData(ByVal params As UpdateCustomerRPLParams) As Boolean

		Dim rplDayDataList = m_ReportDataAccess.LoadRPLDayData(params.RPNr, RPLType.Customer, params.RPLNr)

		If Not rplDayDataList.Count = 1 Then
			Return False
		End If

		Dim customerRPLDayData = rplDayDataList(0)

		Dim workingHours = params.TimeTable.GetDateAndHourData()
		customerRPLDayData.ApplyWorkingHours(workingHours)
		customerRPLDayData.KumulativStd = params.TimeTable.SumHourData()

		Dim success As Boolean = True

		success = m_ReportDataAccess.UpdateCustomerRPLDayData(customerRPLDayData)

		Return success

	End Function

	''' <summary>
	''' Gets working hours and flexible time.
	''' </summary>
	''' <param name="timeTable">The time table.</param>
	''' <param name="rpGAVNr">The report GAV number.</param>
	''' <param name="rpGAVStdWeek">The report GAV Std week hours.</param>
	''' <param name="rpYear">The report year.</param>
	''' <param name="mdNr">The mandant year.</param>
	''' <returns>Working hours and flexible time.</returns>
	Private Function GetWorkingHoursAndFlexibleTime(ByVal timeTable As TimeTable, ByVal rpGAVNr As Decimal, ByVal rpGAVStdWeek As Decimal, ByVal rpYear As Integer, ByVal mdNr As Integer) As WorkingHoursAndFlexibleTime

		Dim maximalWorkingHoursPerWorkingDay As Decimal = Decimal.MaxValue

		Dim flexibleTimeHelper As New FlexibleTimeHelper(mdNr, m_ReportDataAccess)
		maximalWorkingHoursPerWorkingDay = flexibleTimeHelper.DetermineMaximalWorkingHoursPerWorkingDay(rpGAVNr, rpGAVStdWeek, rpYear)

		Dim workingHoursAndFlexibleTime = timeTable.GetRegularWorkingHoursAndFlexibleTime(maximalWorkingHoursPerWorkingDay)

		Return workingHoursAndFlexibleTime
	End Function

	''' <summary>
	''' Gets the number of spesen days.
	''' </summary>
	''' <param name="rpNr">The report number.</param>
	''' <param name="rplType">The RPL type.</param>
	''' <param name="rpGAVNr">The GAV number.</param>
	''' <param name="rpGAVStdWeek">The GAV std week hour.s</param>
	''' <returns>Number of Spesen days.</returns>
	Private Function GetNumberOfSpesenDays(ByVal rpNr As Integer, ByVal rplType As RPLType, ByVal rpGAVNr As Decimal, ByVal rpGAVStdWeek As Decimal, ByVal rpYear As Integer) As Integer?

		Dim daysTotals = m_ReportDataAccess.GetRPLDayHoursTotal(rpNr, rplType)

		If daysTotals Is Nothing Then
			Return Nothing
		End If

		Dim numberOfHoursToReachForSpesen As Decimal = m_TagesspesenStdAb

		If rpGAVNr > 0 Then

			Dim numberOfHoursForSpesenInTSPLMVSepsen As Decimal? = m_ReportDataAccess.LoadMandantTSPLMVSpesenHourValue(rpGAVNr, rpYear)

			If numberOfHoursForSpesenInTSPLMVSepsen.HasValue AndAlso numberOfHoursForSpesenInTSPLMVSepsen.Value > 0 Then
				numberOfHoursToReachForSpesen = numberOfHoursForSpesenInTSPLMVSepsen
				' wurde deaktiviert da für allgemein gültig sein darf!
				'Else
				'  numberOfHoursToReachForSpesen = rpGAVStdWeek / 5
			End If

		End If

		Dim numberOfDays = daysTotals.GetNumberOfDaysThatReachASpecifiedWorktime(numberOfHoursToReachForSpesen)

		Return numberOfDays

	End Function

	''' <summary>
	''' Creates a customer RPL duplication of an employee RPL.
	''' </summary>
	''' <param name="createEmployeeRPLParams">The create employee RPL params.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function CreateCustomerRPLDuplicateOfEmployeeRPL(ByVal createEmployeeRPLParams As CreateEmployeeRPLParams) As Boolean

		If createEmployeeRPLParams.AdditionalParamsForCustomerRPLDuplication Is Nothing Then
			Return False
		End If

		Dim inputParamsDuplicate As New CreateCustomerRPLParams

		Dim anzahl As Decimal = 0D

		If Not createEmployeeRPLParams.TimeTable Is Nothing Then
			' Here the SumHourData from the time table is taken because M_Anzahl is maybe not correct due to flexible time (Gleitzeit)
			anzahl = createEmployeeRPLParams.TimeTable.SumHourData
		Else
			anzahl = createEmployeeRPLParams.M_Anzahl
		End If

		'Dim la = SelectedLA
		'Dim rounding As Short = 2
		'If Not la Is Nothing AndAlso la.Rounding.HasValue Then
		'	rounding = la.Rounding
		'End If


		Dim basisFactor As Decimal = createEmployeeRPLParams.AdditionalParamsForCustomerRPLDuplication.LABasisFactor
		Dim basis As Decimal = createEmployeeRPLParams.AdditionalParamsForCustomerRPLDuplication.BasisValue
		Dim ansatz As Decimal = createEmployeeRPLParams.AdditionalParamsForCustomerRPLDuplication.AnsatzValue
		Dim betrag As Decimal = 0D '= m_Utility.SwissCommercialRound(anzahl * (basisFactor * Math.Round(basis, 2)) * (ansatz / 100.0))
		If createEmployeeRPLParams.LANr = 101 OrElse createEmployeeRPLParams.LANr = 290 Then
			betrag = (anzahl * (basisFactor * basis) * (ansatz / 100.0))
		Else
			betrag = m_Utility.SwissCommercialRound(anzahl * (basisFactor * Math.Round(basis, 2)) * (ansatz / 100.0))
		End If

		inputParamsDuplicate.RPNr = createEmployeeRPLParams.RPNr
		inputParamsDuplicate.RPMonat = createEmployeeRPLParams.RPMonat
		inputParamsDuplicate.RPJahr = createEmployeeRPLParams.RPJahr
		inputParamsDuplicate.RPGAVNr = createEmployeeRPLParams.RPGAVNr
		inputParamsDuplicate.RPGAV_StdWeek = createEmployeeRPLParams.RPGAV_StdWeek
		inputParamsDuplicate.KDNr = createEmployeeRPLParams.KDNr
		inputParamsDuplicate.MANr = createEmployeeRPLParams.MANr
		inputParamsDuplicate.ESNr = createEmployeeRPLParams.ESNr
		inputParamsDuplicate.KSTNR = createEmployeeRPLParams.KSTNR
		inputParamsDuplicate.KstBez = createEmployeeRPLParams.KstBez
		inputParamsDuplicate.GAVText = createEmployeeRPLParams.GAVText
		inputParamsDuplicate.Currency = createEmployeeRPLParams.Currency
		inputParamsDuplicate.LANr = createEmployeeRPLParams.LANr
		inputParamsDuplicate.K_Anzahl = anzahl
		inputParamsDuplicate.K_Basis = basis * basisFactor
		inputParamsDuplicate.K_Ansatz = ansatz
		inputParamsDuplicate.MWST = createEmployeeRPLParams.AdditionalParamsForCustomerRPLDuplication.MwSt
		inputParamsDuplicate.SUVA = createEmployeeRPLParams.SUVA
		inputParamsDuplicate.VonDate = createEmployeeRPLParams.VonDate
		inputParamsDuplicate.BisDate = createEmployeeRPLParams.BisDate
		inputParamsDuplicate.ESLohnNr = createEmployeeRPLParams.ESLohnNr
		inputParamsDuplicate.KDTSpesenBas = createEmployeeRPLParams.AdditionalParamsForCustomerRPLDuplication.KDTSpesen
		inputParamsDuplicate.KDTSpesen = createEmployeeRPLParams.AdditionalParamsForCustomerRPLDuplication.KDTSpesen > 0D
		inputParamsDuplicate.KDBetrag = betrag
		inputParamsDuplicate.UserName = m_InitializationData.UserData.UserFullName
		inputParamsDuplicate.RPZusatzText = createEmployeeRPLParams.RPZusatzText
		inputParamsDuplicate.IsCreatedWithEmployee = True

		inputParamsDuplicate.TimeTable = createEmployeeRPLParams.TimeTable

		Dim result = CreateCustomerRPL(inputParamsDuplicate)

		If result Then m_CustomerRPLNumber = inputParamsDuplicate.NewRPLNr Else m_CustomerRPLNumber = 0


		Return result 'CreateCustomerRPL(inputParamsDuplicate)

	End Function

#End Region


End Class
