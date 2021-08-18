
Imports System.IO
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.TableSetting


Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.DateAndTimeCalculation

Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.PayrollMng
Imports SP.DatabaseAccess.SalaryValueCalculation

Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects.DocumentMng

Imports System.Windows.Forms
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports SPS.MA.Lohn.Utility.ModulConstants
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.ES
Imports SPProgUtility.CommonXmlUtility
Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.DatabaseAccess.Deleted
Imports SP.DatabaseAccess.Deleted.DataObjects
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects

Public Class DeletePayroll


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
	''' The common data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_TablesDatabaseAccess As ITablesDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess
	Private m_DeleteDatabaseAccess As IDeletedDatabaseAccess

	Private m_md As New Mandant

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As SP.Infrastructure.Utility
	Private m_utilitySP As SPProgUtility.MainUtilities.Utilities

	Private m_CurrentYear As Integer
	Private m_CurrentMonth As Integer
	Private m_CurrentEmployeeNumber As Integer
	Private m_CurrentPayrollNumber As Integer
	Private m_CurrentAdvancedPayment As Integer
	Private m_CurrentLMID As Integer
	Private m_CurrentEmployeeGuid As String
	Private m_CurrentPayrollGuid As String
	Private m_CurrentReason As String
	Private m_CurrentExportedFile As String
	Private m_CurrentEmployeeFullName As String

	Private NotDeletedZGNumbers As List(Of Integer)
	Private m_UserSecData As IEnumerable(Of UserRightData)


#End Region


#Region "public properties"

	Public Property CurrentDelteData As List(Of DeleteData)

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_md = New Mandant
		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_utilitySP = New SPProgUtility.MainUtilities.Utilities

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Dim connectionString As String = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_TablesDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_DeleteDatabaseAccess = New DeletedDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
		m_UserSecData = m_TablesDatabaseAccess.LoadAssignedUserRightsData(m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, String.Empty)


	End Sub


#End Region


#Region "public methodes"

	Public Function DeleteSelectedLO(ByVal showMsg As Boolean) As String
		Dim success As Boolean = True
		Dim sMonth As Short = Now.Month
		Dim iYear As Integer = Now.Year
		Dim iMDNr As Integer = 0
		Dim iMANr As Integer = 0
		Dim iLMID As Integer = 0
		Dim iLONr As Integer = 0
		Dim iZGNr As Integer = 0
		Dim strLOGuid As String = String.Empty
		Dim strMAGuid As String = String.Empty
		Dim strLONr As String = String.Empty
		Dim _liNotDeletedZGNr As New List(Of Integer)

		Try

			Dim loNumbers As New List(Of Integer)
			For Each lo In CurrentDelteData
				loNumbers.Add(lo.PayrollNumber)
			Next
			Dim delelteData = m_ListingDatabaseAccess.LoadPayrollsPrintData(New PayrollSearchData With {.MDNr = m_InitializationData.MDData.MDNr, .LONr = loNumbers, .mawos = PayrollSearchData.WOSValue.All})

			NotDeletedZGNumbers = New List(Of Integer) '.Clear()
			For Each lo In delelteData
				m_CurrentMonth = lo.monat
				m_CurrentYear = lo.jahr

				m_CurrentPayrollNumber = lo.LONr
				m_CurrentEmployeeNumber = lo.MANr
				m_CurrentLMID = lo.LMID
				m_CurrentAdvancedPayment = lo.ZGNumber
				m_CurrentEmployeeFullName = lo.EmployeeFullnameWithoutComma

				m_CurrentPayrollGuid = lo.PayrollGuid
				m_CurrentEmployeeGuid = lo.EmployeeGuid
				Dim assignedData = CurrentDelteData.Where(Function(x) x.PayrollNumber = m_CurrentPayrollNumber).FirstOrDefault
				If Not assignedData Is Nothing Then
					m_CurrentExportedFile = assignedData.ExportedFilename
					success = success AndAlso DeleteSelectedLOFromDb(lo, showMsg)
				End If

				If Not success Then Exit For

			Next

			If success AndAlso NotDeletedZGNumbers.Count > 0 Then
				Dim frmTest As New frmNotDeletedZG(m_InitializationData, NotDeletedZGNumbers)

				frmTest.Show()
				frmTest.BringToFront()
			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			If showMsg Then
				m_UtilityUI.ShowErrorDialog(String.Format("{0}", ex.ToString))
			End If
			Return False

		Finally

		End Try

		Return success

	End Function

#End Region


#Region "private methodes"

	Private Function DeleteSelectedLOFromDb(ByVal printData As PayrollPrintData, ByVal showMsg As Boolean) As Boolean
		Dim success As Boolean = True
		Dim strGeschlecht As String = String.Empty
		Dim strMAAnrede As String = String.Empty
		Dim strNachname As String = String.Empty
		Dim strVorname As String = String.Empty

		If m_CurrentMonth = 0 OrElse m_CurrentEmployeeNumber = 0 OrElse m_CurrentPayrollNumber = 0 Then Throw New Exception("Keine Lohnabrechnung wurde gefunden.")
		If IsLOMonthClosed(m_InitializationData.MDData.MDNr, m_CurrentMonth, m_CurrentYear) <> String.Empty Then
			Throw New Exception(m_Translate.GetSafeTranslationValue("Achtung: Der ausgewählte Monat wurde bereits abgeschlossen."))
		End If
		Dim verifyUserRight As Boolean = GetUserSecLevelInObject(665)
		Dim existsLMID As Boolean = printData.LMID.GetValueOrDefault(0) > 0
		Dim existsPayedLP As Boolean = printData.lpVGNr.GetValueOrDefault(0) > 0

		If verifyUserRight Then
			If existsLMID OrElse existsPayedLP > 0 Then
				Dim lm8100Warn As Boolean = existsLMID AndAlso GetUserSecLevelInObject(570)
				Dim lm8100Delete = existsLMID AndAlso GetUserSecLevelInObject(571)
				If lm8100Delete Then
					m_Logger.LogInfo("payroll can not be deleted because 8100 la exists.")
					m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Achtung: Es gibt einen Minuslohnvortrag für nächsten den Monat. Bitte löschen Sie zuerst die Daten über die monatlichen Lohnangaben."),
																	 m_Translate.GetSafeTranslationValue("Lohnabrechnung löschen"), MessageBoxIcon.Stop)
					Return False
				End If

				If lm8100Warn Then
					m_Logger.LogInfo("deleteing payroll with warning because 8100 la exists.")
					If m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Achtung: Es gibt einen Minuslohnvortrag für den nächsten Monat. Möchten Sie trotzdem die Lohnabrechnung inklusive Minuslohnvortrag löschen?"),
																			 m_Translate.GetSafeTranslationValue("Lohnabrechnung löschen"), MessageBoxDefaultButton.Button2, MessageBoxIcon.Exclamation) = False Then
						Return False
					End If
				End If

				If existsPayedLP Then
					m_Logger.LogInfo("deleteing payroll with warning because 8730 la exists.")
					If m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Achtung: Es gibt eine Lohnpfändung welche bereits überwiesen wurde. Möchten Sie trotzdem die Lohnabrechnung löschen?"),
																			 m_Translate.GetSafeTranslationValue("Lohnabrechnung löschen"), MessageBoxDefaultButton.Button2, MessageBoxIcon.Exclamation) = False Then
						Return False
					End If
				End If

			End If

		Else
				m_Logger.LogWarning("user rights will be not verified!")

		End If

		strGeschlecht = printData.Gender
		strNachname = printData.employeelastname
		strVorname = printData.employeefirstname
		strMAAnrede = String.Format(m_Translate.GetSafeTranslationValue(If(UCase(strGeschlecht = "M"), "Herr", "Frau")) & " {0} {1}", strVorname, strNachname)

		Dim strMsg As String
		If showMsg Then
			strMsg = "Mit diesem Vorgang löschen Sie die ausgewählte Lohnabrechnung.{0}Abrechnungsnummer: {1}{0}KandidatIn: {2}{0}{0}"
			strMsg &= "Möchten Sie wirklich mit dem Vorgang fortfahren?"
			strMsg = String.Format(m_Translate.GetSafeTranslationValue(strMsg), vbNewLine, m_CurrentPayrollNumber, strMAAnrede)
			If Not m_UtilityUI.ShowYesNoDialog(strMsg, m_Translate.GetSafeTranslationValue("Lohnabrechnung löschen"), MessageBoxDefaultButton.Button1) Then
				Throw New Exception("Der Vorgang wurde abgebrochen.")
			End If
		End If

		Dim strExportedFileName As String = m_CurrentExportedFile

		' ZG Wird auf 0 gesetzt
		Dim bZGDeleted As Boolean
		If m_CurrentAdvancedPayment > 0 Then
			Dim bZGDelete As Boolean = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 569, m_InitializationData.MDData.MDNr)
			If bZGDelete Then
				If m_CurrentAdvancedPayment <= 0 Then       ' Ist der Betrag von LO negativ?
					bZGDeleted = True       ' Es existiert keinen Vorschussdatensatz von LO

				Else
					bZGDeleted = True

				End If
			End If
		End If

		' vom WOS zuerst löschen...
		Try
			Dim allowedWOS = Not String.IsNullOrWhiteSpace(m_CurrentPayrollGuid) AndAlso m_md.AllowedExportEmployee2WOS(m_InitializationData.MDData.MDNr, Now.Year)
			'If allowedWOS Then success = DeleteDocFrom_WS(m_CurrentPayrollNumber, m_CurrentPayrollGuid)
			If allowedWOS Then success = success AndAlso DeletePayrollDocumentFromWOS()
			If Not success Then Return success

		Catch ex As Exception
			m_Logger.LogError(String.Format("Datensätze im WOS löschen. {0}", ex.ToString))

		End Try

		Try
			success = success AndAlso DeleteSelectedPayroll()

		Catch ex As Exception
			m_Logger.LogError(String.Format("Datensätze löschen. {0}", ex.ToString))

		End Try

		Try
			If success Then
				strMsg = m_Translate.GetSafeTranslationValue("Die Lohnabrechnung wurde erfolgreich gelöscht:{0}Abrechnungsnummer: {1}{0}KandidatIn: {2}")
			Else
				strMsg = m_Translate.GetSafeTranslationValue("Die Lohnabrechnung konnte nicht erfolgreich gelöscht werden!{0}Abrechnungsnummer: {1}{0}KandidatIn: {2}")
			End If

			If m_CurrentLMID > 0 OrElse (Not bZGDeleted AndAlso m_CurrentAdvancedPayment > 0) Then
				strMsg &= m_Translate.GetSafeTranslationValue("{0}{0}Achtung:{0}")
				If m_CurrentLMID > 0 Then strMsg &= m_Translate.GetSafeTranslationValue("Ich habe die Lohnart: '(8100) Minuslohn Vortrag' aus der Monatlichen Lohnangaben automatisch gelöscht.{0}")
				If Not bZGDeleted AndAlso m_CurrentAdvancedPayment > 0 Then
					strMsg &= m_Translate.GetSafeTranslationValue("Aus Sicherheistgründen wurde aber der Auszahlungsbetrag in der Vorschussverwaltung: {3} nicht gelöscht.")
					NotDeletedZGNumbers.Add(m_CurrentAdvancedPayment)
				End If
			End If
			strMsg = String.Format(strMsg, vbNewLine, m_CurrentPayrollNumber, strMAAnrede, m_CurrentAdvancedPayment)

			' eine Doc-Kopie in die Datenbank speichern...
			If File.Exists(strExportedFileName) Then SaveFileIntoDb(strExportedFileName) : File.Delete(strExportedFileName)

			If showMsg Then
				If success Then
					m_UtilityUI.ShowInfoDialog(strMsg)
				Else
					m_UtilityUI.ShowErrorDialog(strMsg)
				End If

			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("MSGBox-Ausgabe. {0}", ex.ToString))

		End Try


		Return success

	End Function

	''' <summary>
	''' Deletes a selected payroll.
	''' </summary>
	Private Function DeleteSelectedPayroll() As Boolean
		Dim result = DeletePayrollResult.Deleted

		result = m_ListingDatabaseAccess.DeletePayroll(m_InitializationData.MDData.MDNr, m_CurrentPayrollNumber, "LO", m_InitializationData.UserData.UserFullName, m_InitializationData.UserData.UserNr)

		Dim msg As String = String.Empty

		Select Case result
			Case DeletePayrollResult.Deleted
				'msg = "Die ausgewählte Lohnabrechnung wurde erfolgreich gelöscht."
				''m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue(msg), m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Information)

				Return True

			Case Else
				msg = String.Format(m_Translate.GetSafeTranslationValue("Die ausgewählte Lohnabrechnung konnte nicht gelöscht werden: {0}"), m_CurrentPayrollNumber)

		End Select
		m_UtilityUI.ShowOKDialog(String.Format(msg), m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)


		Return result = DeletePayrollResult.Deleted
	End Function

	Private Function SaveFileIntoDb(ByVal strFile2Save As String) As Boolean
		Dim result As Boolean = True

		Try

			Dim deleteData As New DeletedData With {.deletedmodul = "LO", .createdfrom = m_InitializationData.UserData.UserFullName, .deletenumber = m_CurrentPayrollNumber}

			deleteData.deleteinfo = String.Format("{0} für ({1}/{2})", m_CurrentEmployeeFullName, m_CurrentMonth, m_CurrentYear)
			deleteData.scandoc = m_Utility.LoadFileBytes(m_CurrentExportedFile)

			result = result AndAlso m_DeleteDatabaseAccess.AddDeleteRecInfo(deleteData)

			If Not result Then
				Dim msg As String = "Die gelöschte Lohnabrechnungsvorlage konnte nicht gespeichert werden!"
				m_Logger.LogWarning(msg)

				Return True
			End If

		Catch ex As Exception
			Return False

		End Try


		Return result
	End Function

	Function DeletePayrollDocumentFromWOS() As Boolean
		Dim result As Boolean = True
		If String.IsNullOrWhiteSpace(m_CurrentPayrollGuid) Then Return True
		Dim wos = New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitializationData)
		Dim wosSetting = New WOSSendSetting

		wosSetting.EmployeeNumber = m_CurrentEmployeeNumber
		wosSetting.EmployeeGuid = m_CurrentEmployeeGuid
		wosSetting.DocumentArtEnum = WOSSendSetting.DocumentArt.Lohnabrechnung
		wosSetting.PayrollNumber = m_CurrentPayrollNumber
		wosSetting.AssignedDocumentGuid = m_CurrentPayrollGuid
		wosSetting.DocumentInfo = String.Format("Lohnabrechnung: {0} / {1}", m_CurrentMonth, m_CurrentYear)

		wos.WOSSetting = wosSetting

		result = result AndAlso wos.DeleteTransferedEmployeeDocument()


		Return result

	End Function


#End Region


#Region "helpers"

	Private Function GetUserSecLevelInObject(ByVal secNumber As Integer) As Boolean
		Dim result As Boolean = True

		If m_UserSecData Is Nothing Then
			m_Logger.LogWarning("usersec data was not loaded.")
			Return False
		End If

		Try
			Dim secData = m_UserSecData.Where(Function(x) x.SecNr = secNumber).FirstOrDefault()
			If secData Is Nothing Then Return False

			result = result AndAlso secData.Autorized

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return False

		Finally

		End Try

		Return result

	End Function

#End Region


End Class
