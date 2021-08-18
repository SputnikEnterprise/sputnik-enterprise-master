
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Invoice
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.TableSetting

Namespace UI

	Public Interface INewInvoiceUserControlFormMediator

		Sub HandleChangeOfMandant(ByVal mdNumber As Integer)
		Function HandleFinishClick() As Boolean
		Function ValidateData() As Boolean

		ReadOnly Property InitDataPage1 As InitDataPage1
		ReadOnly Property InitDataPage2 As InitDataPage2

		ReadOnly Property CommonDbAccess As ICommonDatabaseAccess
		ReadOnly Property InvoiceDbAccess As IInvoiceDatabaseAccess
		ReadOnly Property CustomerDbAccess As ICustomerDatabaseAccess


	End Interface

	Public Interface INewPaymentUserControlFormMediator

		Sub HandleChangeOfMandant(ByVal mdNumber As Integer)
		Function HandleFinishClick() As Boolean
		Function ValidateData() As Boolean

		ReadOnly Property CommonDbAccess As ICommonDatabaseAccess
		ReadOnly Property InvoiceDbAccess As IInvoiceDatabaseAccess
		ReadOnly Property TableDbAccess As ITablesDatabaseAccess
		ReadOnly Property InitPaymentDataPage1 As InitPaymentDataPage1

	End Interface

End Namespace
