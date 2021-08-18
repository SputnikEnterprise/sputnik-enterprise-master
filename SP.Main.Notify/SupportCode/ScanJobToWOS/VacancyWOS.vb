
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

		Public Function VerifyVacanciesWithWOS() As Boolean
			Dim result As Boolean = True

			If String.IsNullOrWhiteSpace(m_VacancyWOSID) Then Return False


			Dim obj As New SP.Internal.Automations.WOSUtility.CustomerExport(m_InitializationData)
			result = result AndAlso obj.VerifyVacanciesWithWOS(0, 0)


			Return result
		End Function


#End Region


	End Class


End Namespace
