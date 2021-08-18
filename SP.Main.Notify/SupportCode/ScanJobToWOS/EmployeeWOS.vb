
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Internal.Automations.WOSUtility.DataObjects
Imports SP.DatabaseAccess.Report
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.DatabaseAccess.PayrollMng.DataObjects


Namespace WOSDataTransfer



	Partial Class SendScanJobTOWOS


#Region "public methods"

		Public Function TransferEmployeeEmploymentDataToWOS(ByVal esNumber As Integer, ByVal file2Transfer As Byte()) As Boolean
			Dim result As Boolean = True
			If String.IsNullOrWhiteSpace(m_EmployeeWOSID) Then Return False

			Dim esSalaryDataList As List(Of DatabaseAccess.ES.DataObjects.ESMng.ESSalaryData) = m_ESDatabaseAccess.LoadESSalaryData(esNumber)
			If esSalaryDataList Is Nothing Then
				m_Logger.LogDebug(String.Format("Einsatz mit der Nummer {0} wurde nicht gefunden.", esNumber))

				Return False
			End If

			Dim salaryDta As DatabaseAccess.ES.DataObjects.ESMng.ESSalaryData = esSalaryDataList.Where(Function(x) x.AktivLODaten = True).FirstOrDefault
			If salaryDta Is Nothing Then
				m_Logger.LogError("keine Einsatzlohn Daten wurden gefunden.")

				Return False
			End If

			m_CurrentEmployeeNumber = salaryDta.EmployeeNumber
			m_CurrentESNumber = esNumber
			m_DocumentGuid = salaryDta.ESDoc_Guid
			Dim employeeData As EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_CurrentEmployeeNumber, False)

			If Not employeeData.Send2WOS OrElse String.IsNullOrWhiteSpace(employeeData.Email) Then Return False

			m_EmployeeGuid = employeeData.Transfered_Guid

			If String.IsNullOrWhiteSpace(m_EmployeeGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateEmployeeGuidData(m_CurrentEmployeeNumber, newGuid)

				If result Then m_EmployeeGuid = newGuid
			End If

			If String.IsNullOrWhiteSpace(m_DocumentGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateEmploymentGuidData(esNumber, newGuid)

				If result Then m_DocumentGuid = newGuid
			End If

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.DocumentArtEnum = WOSSendSetting.DocumentArt.Einsatzvertrag
			_setting.DocumentInfo = String.Format("Einsatzvertrag: {0}", esNumber)
			_setting.EmployeeGuid = m_EmployeeGuid
			_setting.EmployeeNumber = m_CurrentEmployeeNumber
			_setting.EmploymentNumber = esNumber

			_setting.AssignedDocumentGuid = m_DocumentGuid
			_setting.SignTransferedDocument = True
			_setting.ScanDoc = file2Transfer
			_setting.ScanDocName = String.Empty


			Dim obj As New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitializationData)
			obj.WOSSetting = _setting
			Dim sendResult = obj.TransferEmployeeDocumentDataToWOS(True)

			result = sendResult.Value

			Return result
		End Function

		Public Function TransferEmployeeReportDataToWOS(ByVal reportNumber As Integer, ByVal reportLineNumber As Integer, ByVal file2Transfer As Byte()) As Boolean
			Dim result As Boolean = True
			If String.IsNullOrWhiteSpace(m_EmployeeWOSID) Then Return False

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
			m_CurrentReportNumber = reportNumber
			Dim reportGuid As String = rpData.RPDoc_Guid
			Dim employeeData As EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_CurrentEmployeeNumber, False)

			If Not employeeData.Send2WOS OrElse String.IsNullOrWhiteSpace(employeeData.Email) Then Return False

			m_EmployeeGuid = employeeData.Transfered_Guid

			If String.IsNullOrWhiteSpace(m_EmployeeGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateEmployeeGuidData(m_CurrentEmployeeNumber, newGuid)

				If result Then m_EmployeeGuid = newGuid
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
			_setting.EmployeeGuid = m_EmployeeGuid
			_setting.EmployeeNumber = m_CurrentEmployeeNumber
			_setting.CustomerNumber = m_CurrentCustomerNumber
			_setting.EmploymentNumber = m_CurrentESNumber
			_setting.ReportNumber = reportNumber
			_setting.ReportLineNumber = reportLineNumber
			_setting.ReportDocumentNumber = 0

			_setting.AssignedDocumentGuid = rpScanDocGuid
			_setting.SignTransferedDocument = True
			_setting.ScanDoc = file2Transfer


			Dim obj As New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitializationData)
			obj.WOSSetting = _setting
			Dim sendResult = obj.TransferEmployeeDocumentDataToWOS(True)

			result = sendResult.Value

			Return result
		End Function

		Public Function TransferEmployeePayrollDataToWOS(ByVal loNumber As Integer, ByVal file2Transfer As Byte()) As Boolean
			Dim result As Boolean = True
			Dim currentPayrollNumber As Integer = 0
			If String.IsNullOrWhiteSpace(m_EmployeeWOSID) Then Return False

			Dim payrollData As LOMasterData = m_PayrollDatabaseAccess.LoadPayrollMasterData(loNumber)
			If payrollData Is Nothing Then
				m_Logger.LogDebug(String.Format("Lohnabrechnung mit der Nummer {0} wurde nicht gefunden.", loNumber))

				Return False
			End If

			m_CurrentEmployeeNumber = payrollData.MANR
			currentPayrollNumber = loNumber
			m_DocumentGuid = payrollData.LODoc_Guid
			Dim employeeData As EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(m_CurrentEmployeeNumber, False)

			If Not employeeData.Send2WOS OrElse String.IsNullOrWhiteSpace(employeeData.Email) Then Return False

			m_EmployeeGuid = employeeData.Transfered_Guid

			If String.IsNullOrWhiteSpace(m_EmployeeGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdateEmployeeGuidData(m_CurrentEmployeeNumber, newGuid)

				If result Then m_EmployeeGuid = newGuid
			End If

			If String.IsNullOrWhiteSpace(m_DocumentGuid) Then
				Dim newGuid As String = Guid.NewGuid.ToString
				result = result AndAlso m_WOSDatabaseAccess.UpdatePayrollGuidData(loNumber, newGuid)

				If result Then m_DocumentGuid = newGuid
			End If

			Dim _setting As New SP.Internal.Automations.WOSUtility.DataObjects.WOSSendSetting
			_setting.DocumentArtEnum = WOSSendSetting.DocumentArt.Lohnabrechnung
			_setting.DocumentInfo = String.Format("Lohnabrechnung: {0} / {1}", payrollData.LP, payrollData.Jahr)
			_setting.EmployeeGuid = m_EmployeeGuid
			_setting.EmployeeNumber = m_CurrentEmployeeNumber
			_setting.PayrollNumber = loNumber

			_setting.AssignedDocumentGuid = m_DocumentGuid
			_setting.SignTransferedDocument = True
			_setting.ScanDoc = file2Transfer
			_setting.ScanDocName = String.Empty


			Dim obj As New SP.Internal.Automations.WOSUtility.EmployeeExport(m_InitializationData)
			obj.WOSSetting = _setting
			Dim sendResult = obj.TransferEmployeeDocumentDataToWOS(True)

			result = sendResult.Value

			Return result
		End Function


#End Region


	End Class


End Namespace
