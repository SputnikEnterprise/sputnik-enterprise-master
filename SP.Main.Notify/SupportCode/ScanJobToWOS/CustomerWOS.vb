
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Invoice.DataObjects
Imports SP.DatabaseAccess.Propose.DataObjects
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng

Namespace WOSDataTransfer


	Partial Class SendScanJobTOWOS


#Region "public methods"

		Public Function TransferCustomerEmploymentDataToWOS(ByVal esNumber As Integer, ByVal file2Transfer As Byte()) As Boolean
			Dim result As Boolean = True
			Dim cResponsibleNumber As Integer = 0
			Dim responsiblePersonData As ResponsiblePersonMasterData = Nothing
			Dim responblePersonGuid As String = String.Empty

			If String.IsNullOrWhiteSpace(m_CustomerWOSID) Then Return False

			Dim esData As DatabaseAccess.ES.DataObjects.ESMng.ESMasterData = m_ESDatabaseAccess.LoadESMasterData(esNumber)
			Dim esSalaryDataList As List(Of DatabaseAccess.ES.DataObjects.ESMng.ESSalaryData) = m_ESDatabaseAccess.LoadESSalaryData(esNumber)
			If esData Is Nothing OrElse esSalaryDataList Is Nothing Then
				m_Logger.LogDebug(String.Format("Einsatz mit der Nummer {0} wurde nicht gefunden.", esNumber))

				Return False
			End If

			Dim salaryDta As DatabaseAccess.ES.DataObjects.ESMng.ESSalaryData = esSalaryDataList.Where(Function(x) x.AktivLODaten = True).FirstOrDefault
			If salaryDta Is Nothing Then
				m_Logger.LogError("keine Einsatzlohn Daten wurden gefunden.")

				Return False
			End If

			m_CurrentEmployeeNumber = salaryDta.EmployeeNumber
			m_CurrentCustomerNumber = salaryDta.CustomerNumber
			m_CurrentESNumber = esNumber
			cResponsibleNumber = esData.KDZHDNr.GetValueOrDefault(0)
			m_DocumentGuid = salaryDta.VerleihDoc_Guid

			Dim customerData As CustomerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(m_CurrentCustomerNumber, m_InitializationData.UserData.UserFiliale)
			Dim customereMailData As List(Of CustomerAssignedEmailData) = m_CustomerDatabaseAccess.LoadAssignedEmailsOfCustomer(m_CurrentCustomerNumber)
			If cResponsibleNumber > 0 Then
				responsiblePersonData = m_CustomerDatabaseAccess.LoadResponsiblePersonMasterData(m_CurrentCustomerNumber, cResponsibleNumber)
			End If

			If customerData Is Nothing OrElse customereMailData Is Nothing Then
				m_Logger.LogDebug(String.Format("Kundendaten {0} mit Einsatznummer {1} wurde nicht gefunden.", m_CurrentCustomerNumber, m_CurrentESNumber))

				Return False
			End If
			If Not customerData.sendToWOS OrElse customereMailData.Count = 0 Then
				m_Logger.LogDebug(String.Format("Der Kunde {0} ist nicht WOS-pflichtig!", m_CurrentCustomerNumber))

				Return False
			End If

			m_CustomerGuid = customerData.Transfered_Guid
			If String.IsNullOrWhiteSpace(m_CustomerGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateCustomerGuidData(m_CurrentCustomerNumber, newGuid)

				If result Then m_CustomerGuid = newGuid
			End If

			If Not responsiblePersonData Is Nothing AndAlso cResponsibleNumber > 0 Then
				responblePersonGuid = responsiblePersonData.TransferedGuid
				If String.IsNullOrWhiteSpace(responblePersonGuid) Then
					Dim newGuid As String = Guid.NewGuid.ToString
					result = result AndAlso m_WOSDatabaseAccess.UpdateCustomerResponsibleGuidData(m_CurrentCustomerNumber, cResponsibleNumber, newGuid)

					If result Then responblePersonGuid = newGuid
				End If
			End If

			If String.IsNullOrWhiteSpace(m_DocumentGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateCustomerEmploymentGuidData(esNumber, newGuid)

				If result Then m_DocumentGuid = newGuid
			End If

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.DocumentArtEnum = WOSSendSetting.DocumentArt.Verleihvertrag
			_setting.DocumentInfo = String.Format("Verleihvertrag: {0}", esNumber)
			_setting.EmployeeGuid = String.Empty
			_setting.CustomerGuid = m_CustomerGuid
			_setting.CresponsibleGuid = responblePersonGuid

			_setting.EmployeeNumber = m_CurrentEmployeeNumber
			_setting.CustomerNumber = m_CurrentCustomerNumber
			_setting.CresponsibleNumber = cResponsibleNumber
			_setting.EmploymentNumber = m_CurrentESNumber

			_setting.AssignedDocumentGuid = m_DocumentGuid
			_setting.SignTransferedDocument = True
			_setting.ScanDoc = file2Transfer
			_setting.ScanDocName = String.Empty


			Dim obj As New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)
			obj.WOSSetting = _setting
			Dim sendResult = obj.TransferCustomerDocumentDataToWOS(True)

			result = sendResult.Value

			Return result
		End Function

		Public Function TransferCustomerReportDataToWOS(ByVal reportNumber As Integer, ByVal reportLineNumber As Integer, ByVal file2Transfer As Byte()) As Boolean
			Dim result As Boolean = True
			Dim cResponsibleNumber As Integer = 0
			Dim responsiblePersonData As ResponsiblePersonMasterData = Nothing
			Dim responblePersonGuid As String = String.Empty

			If String.IsNullOrWhiteSpace(m_CustomerWOSID) Then Return False

			Dim rpData As RPMasterData = m_RPDatabaseAccess.LoadRPMasterData(reportNumber)
			Dim rpScanDocGuid As String = m_RPDatabaseAccess.GetRPLScanDocGuid(reportNumber, reportLineNumber)
			Dim rplineData As RPLListData = m_WOSDatabaseAccess.LoadRPLineData(reportNumber, RPLType.Employee, reportLineNumber)

			If rpData Is Nothing OrElse rplineData Is Nothing Then
				m_Logger.LogDebug(String.Format("Rapportzeile mit der Nummer {0} -> {1} wurde nicht gefunden.", reportNumber, reportLineNumber))

				Return False
			End If

			m_CurrentEmployeeNumber = rpData.EmployeeNumber
			m_CurrentCustomerNumber = rpData.CustomerNumber
			m_CurrentESNumber = rpData.ESNR
			cResponsibleNumber = rpData.ResponsiblePersonNumber.GetValueOrDefault(0)
			m_CurrentReportNumber = reportNumber

			Dim reportGuid As String = rpData.RPDoc_Guid
			Dim customerData As CustomerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(m_CurrentCustomerNumber, m_InitializationData.UserData.UserFiliale)
			Dim customereMailData As List(Of CustomerAssignedEmailData) = m_CustomerDatabaseAccess.LoadAssignedEmailsOfCustomer(m_CurrentCustomerNumber)
			If cResponsibleNumber > 0 Then
				responsiblePersonData = m_CustomerDatabaseAccess.LoadResponsiblePersonMasterData(m_CurrentCustomerNumber, rpData.ResponsiblePersonNumber)
			End If

			' must be true!
			If customerData Is Nothing OrElse customereMailData Is Nothing Then
				m_Logger.LogDebug(String.Format("Kundendaten {0} mit Rapportnummer {1} wurde nicht gefunden.", m_CurrentCustomerNumber, reportNumber))

				Return False
			End If
			If Not customerData.sendToWOS OrElse customereMailData.Count = 0 Then
				m_Logger.LogDebug(String.Format("Der Kunde {0} ist nicht WOS-pflichtig!", m_CurrentCustomerNumber))

				Return False
			End If

			m_CustomerGuid = customerData.Transfered_Guid
			If String.IsNullOrWhiteSpace(m_CustomerGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateCustomerGuidData(m_CurrentCustomerNumber, newGuid)

				If result Then m_CustomerGuid = newGuid
			End If

			If Not responsiblePersonData Is Nothing AndAlso rpData.ResponsiblePersonNumber.GetValueOrDefault(0) > 0 Then
				responblePersonGuid = responsiblePersonData.TransferedGuid
				If String.IsNullOrWhiteSpace(responblePersonGuid) Then
					Dim newGuid As String = Guid.NewGuid.ToString
					result = result AndAlso m_WOSDatabaseAccess.UpdateCustomerResponsibleGuidData(m_CurrentCustomerNumber, rpData.ResponsiblePersonNumber, newGuid)

					If result Then responblePersonGuid = newGuid
				End If
			End If

			If String.IsNullOrWhiteSpace(reportGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateReportGuidData(reportNumber, newGuid)

				If result Then reportGuid = newGuid
			End If

			If String.IsNullOrWhiteSpace(rpScanDocGuid) Then
				Dim newReportLineGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateReportLineGuidData(reportNumber, reportLineNumber, newReportLineGuid)

				If result Then m_DocumentGuid = newReportLineGuid
			End If

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rapport
			_setting.DocumentInfo = String.Format("Rapport: {0}", reportNumber)
			_setting.EmployeeGuid = String.Empty
			_setting.CustomerGuid = m_CustomerGuid
			_setting.CresponsibleGuid = responblePersonGuid

			_setting.EmployeeNumber = m_CurrentEmployeeNumber
			_setting.CustomerNumber = m_CurrentCustomerNumber
			_setting.CresponsibleNumber = rpData.ResponsiblePersonNumber.GetValueOrDefault(0)
			_setting.EmploymentNumber = m_CurrentESNumber
			_setting.ReportNumber = reportNumber
			_setting.ReportLineNumber = reportLineNumber
			_setting.ReportDocumentNumber = 0

			_setting.AssignedDocumentGuid = rpScanDocGuid
			_setting.SignTransferedDocument = True
			_setting.ScanDoc = file2Transfer


			Dim obj As New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)
			obj.WOSSetting = _setting
			Dim sendResult = obj.TransferCustomerDocumentDataToWOS(True)

			result = sendResult.Value

			Return result
		End Function

		Public Function TransferCustomerInvoiceDataToWOS(ByVal invoiceNumber As Integer, ByVal file2Transfer As Byte()) As Boolean
			Dim result As Boolean = True

			If String.IsNullOrWhiteSpace(m_CustomerWOSID) Then Return False

			Dim reData As Invoice = m_REDatabaseAccess.LoadInvoice(invoiceNumber)

			If reData Is Nothing Then
				m_Logger.LogDebug(String.Format("Rechnung Daten mit der Nummer {0} wurde nicht gefunden.", invoiceNumber))

				Return False
			End If

			m_CurrentCustomerNumber = reData.KdNr

			Dim invoiceGuid As String = reData.REDoc_Guid
			Dim customerData As CustomerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(m_CurrentCustomerNumber, m_InitializationData.UserData.UserFiliale)
			Dim customereMailData As List(Of CustomerAssignedEmailData) = m_CustomerDatabaseAccess.LoadAssignedEmailsOfCustomer(m_CurrentCustomerNumber)

			If customerData Is Nothing OrElse customereMailData Is Nothing Then
				m_Logger.LogDebug(String.Format("Kundendaten {0} mit Rechnungsnummer {1} wurde nicht gefunden.", m_CurrentCustomerNumber, invoiceNumber))

				Return False
			End If
			If Not customerData.sendToWOS OrElse customereMailData.Count = 0 Then
				m_Logger.LogDebug(String.Format("Der Kunde {0} ist nicht WOS-pflichtig!", m_CurrentCustomerNumber))

				Return False
			End If

			m_CustomerGuid = customerData.Transfered_Guid
			If String.IsNullOrWhiteSpace(m_CustomerGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateCustomerGuidData(m_CurrentCustomerNumber, newGuid)

				If result Then m_CustomerGuid = newGuid Else Return False
			End If

			If String.IsNullOrWhiteSpace(invoiceGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateInvoiceGuidData(invoiceNumber, newGuid)

				If result Then invoiceGuid = newGuid Else Return False
			End If

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.DocumentArtEnum = WOSSendSetting.DocumentArt.Rechnung
			_setting.DocumentInfo = String.Format("Rechnung: {0} ({1:d})", invoiceNumber, reData.FakDat)
			_setting.CustomerGuid = m_CustomerGuid

			_setting.CustomerNumber = m_CurrentCustomerNumber
			_setting.EmploymentNumber = m_CurrentESNumber
			_setting.InvoiceNumber = invoiceNumber

			_setting.AssignedDocumentGuid = invoiceGuid
			_setting.SignTransferedDocument = True
			_setting.ScanDoc = file2Transfer


			Dim obj As New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)
			obj.WOSSetting = _setting
			Dim sendResult = obj.TransferCustomerDocumentDataToWOS(True)

			result = sendResult.Value

			Return result
		End Function

		Public Function TransferCustomerProposeDataToWOS(ByVal proposeNumber As Integer, ByVal file2Transfer As Byte()) As Boolean
			Dim result As Boolean = True

			result = result AndAlso TransfertCustomerDocumentToWOS(proposeNumber, file2Transfer)
			If Not result Then Return result


			Return result

		End Function

		Public Function NotifyCustomerProposeResultDataFromWOS() As Boolean
			Dim result As Boolean = True

			result = result AndAlso LoadProposeResultFromWOS()
			If Not result Then Return result


			Return result

		End Function

#End Region

		Private Function TransfertCustomerDocumentToWOS(ByVal proposeNumber As Integer, ByVal file2Transfer As Byte()) As Boolean
			Dim result As Boolean = True
			Dim cResponsibleNumber As Integer = 0
			Dim responsiblePersonData As ResponsiblePersonMasterData = Nothing
			Dim responblePersonGuid As String = String.Empty
			Dim currentProposeNumber As Integer = 0
			If String.IsNullOrWhiteSpace(m_CustomerWOSID) Then Return False

			Dim proposeData As ProposeMasterData = m_ProposeDatabaseAccess.LoadProposeMasterData(proposeNumber)
			If proposeData Is Nothing Then
				m_Logger.LogDebug(String.Format("Vorschlag mit der Nummer {0} wurde nicht gefunden.", proposeNumber))

				Return False
			End If

			currentProposeNumber = proposeNumber
			m_CurrentEmployeeNumber = proposeData.MANr
			m_CurrentCustomerNumber = proposeData.KDNr
			cResponsibleNumber = proposeData.KDZHDNr
			m_DocumentGuid = proposeData.Doc_Guid

			Dim employeeData As EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_CurrentEmployeeNumber, True)
			Dim employeeContactCommData As EmployeeContactComm = m_EmployeeDatabaseAccess.LoadEmployeeContactCommData(m_CurrentEmployeeNumber)
			If employeeData Is Nothing OrElse employeeContactCommData Is Nothing Then
				m_Logger.LogDebug(String.Format("Kandidat mit der Nummer {0} wurde nicht gefunden.", m_CurrentEmployeeNumber))

				Return False
			End If

			m_EmployeeGuid = employeeData.Transfered_Guid
			If String.IsNullOrWhiteSpace(m_EmployeeGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateEmployeeGuidData(m_CurrentEmployeeNumber, newGuid)

				If result Then m_EmployeeGuid = newGuid
			End If


			Dim customerData As CustomerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(m_CurrentCustomerNumber, m_InitializationData.UserData.UserFiliale)
			Dim customereMailData As List(Of CustomerAssignedEmailData) = m_CustomerDatabaseAccess.LoadAssignedEmailsOfCustomer(m_CurrentCustomerNumber)
			If cResponsibleNumber > 0 Then
				responsiblePersonData = m_CustomerDatabaseAccess.LoadResponsiblePersonMasterData(m_CurrentCustomerNumber, cResponsibleNumber)
			End If

			If customerData Is Nothing OrElse customereMailData Is Nothing Then
				m_Logger.LogDebug(String.Format("Kundendaten {0} mit Vorschlagnummer {1} wurde nicht gefunden.", m_CurrentCustomerNumber, currentProposeNumber))

				Return False
			End If
			If Not customerData.sendToWOS OrElse customereMailData.Count = 0 Then
				' for propose should it be ok so!
				m_Logger.LogDebug(String.Format("Der Kunde {0} ist nicht WOS-pflichtig!", m_CurrentCustomerNumber))
			End If

			m_CustomerGuid = customerData.Transfered_Guid
			If String.IsNullOrWhiteSpace(m_CustomerGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateCustomerGuidData(m_CurrentCustomerNumber, newGuid)

				If result Then m_CustomerGuid = newGuid
			End If

			If Not responsiblePersonData Is Nothing AndAlso cResponsibleNumber > 0 Then
				responblePersonGuid = responsiblePersonData.TransferedGuid
				If String.IsNullOrWhiteSpace(responblePersonGuid) Then
					Dim newGuid As String = Guid.NewGuid.ToString
					result = result AndAlso m_WOSDatabaseAccess.UpdateCustomerResponsibleGuidData(m_CurrentCustomerNumber, cResponsibleNumber, newGuid)

					If result Then responblePersonGuid = newGuid
				End If
			End If

			If String.IsNullOrWhiteSpace(m_DocumentGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateProposeGuidData(proposeNumber, newGuid)

				If result Then m_DocumentGuid = newGuid
			End If

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.DocumentArtEnum = WOSSendSetting.DocumentArt.Vorschlag
			_setting.DocumentInfo = String.Format("Vorschlag: {0}", currentProposeNumber)
			_setting.EmployeeGuid = m_EmployeeGuid
			_setting.CustomerGuid = m_CustomerGuid
			_setting.CresponsibleGuid = responblePersonGuid

			_setting.EmployeeNumber = m_CurrentEmployeeNumber
			_setting.CustomerNumber = m_CurrentCustomerNumber
			_setting.CresponsibleNumber = cResponsibleNumber
			_setting.EmploymentNumber = m_CurrentESNumber
			_setting.ProposeNumber = currentProposeNumber

			_setting.AssignedDocumentGuid = m_DocumentGuid
			_setting.SignTransferedDocument = True
			_setting.ScanDoc = file2Transfer
			_setting.ScanDocName = String.Empty


			Dim obj As New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)
			obj.WOSSetting = _setting
			Dim sendResult = obj.TransferCustomerDocumentDataToWOS(True)

			If m_SendEmployeeAfterPropose AndAlso sendResult.Value Then
				Dim objEmployee As New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitializationData)
				objEmployee.WOSSetting = _setting
				Dim sendEmployeeResult = objEmployee.TransferAvailableEmployeeDataToWOS()

				If sendEmployeeResult.Value AndAlso Not employeeContactCommData.WebExport.GetValueOrDefault(False) Then
					result = result AndAlso m_WOSDatabaseAccess.UpdateEmployeeWebExportData(m_CurrentEmployeeNumber, Not employeeContactCommData.WebExport.GetValueOrDefault(False))
					sendResult.Value = result
				End If
			End If



			result = sendResult.Value

			Return result
		End Function

		Private Function LoadProposeResultFromWOS() As Boolean
			Dim result As Boolean = True

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.DocumentArtEnum = WOSSendSetting.DocumentArt.Vorschlag

			_setting.ScanDocName = String.Empty


			Dim obj As New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)
			obj.WOSSetting = _setting
			result = result AndAlso obj.NotifyAdvisorForTransferedProposeData(m_InitializationData.MDData.MDGuid, String.Empty, 0, WOSSendSetting.DocumentArt.Vorschlag)

			Return result
		End Function

	End Class


End Namespace
