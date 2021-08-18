
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Invoice
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.TableSetting

Namespace UI

	''' <summary>
	''' Mediator to communicate between user controls (only for new invoice wizard).
	''' </summary>
	Public Class NewPaymentUserControlFormMediator
		Implements INewPaymentUserControlFormMediator


#Region "Private Fields"

		Private m_NewPaymentFrm As frmNewZahlungsEingang

		Private m_ucPageSelectMandantPayment As ucPageSelectMandantPayment

		Private m_ucPageCreatePaymentData As ucPageCreatePayment

		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		Private m_UIUtility As SP.Infrastructure.UI.UtilityUI

#End Region

#Region "Constructor"

		Public Sub New(ByVal frmNewPayment As frmNewZahlungsEingang,
									ByVal pageSelectMandant As ucPageSelectMandantPayment,
									ByVal pageCreatePayment As ucPageCreatePayment,
									ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper)

			m_UIUtility = New SP.Infrastructure.UI.UtilityUI

			Me.m_NewPaymentFrm = frmNewPayment
			Me.m_ucPageSelectMandantPayment = pageSelectMandant
			Me.m_ucPageCreatePaymentData = pageCreatePayment
			Me.m_Translate = translate

			m_ucPageSelectMandantPayment.UCPaymentMediator = Me
			m_ucPageCreatePaymentData.UCPaymentMediator = Me

		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Handles change of mandant.
		''' </summary>
		''' <param name="mdNumber">The mandant number.</param>
		Public Sub HandleChageOfMandant(ByVal mdNumber As Integer) Implements INewPaymentUserControlFormMediator.HandleChangeOfMandant
			m_NewPaymentFrm.ChangeMandant(mdNumber)
			m_ucPageCreatePaymentData.Reset()
		End Sub

		''' <summary>
		''' Handle finish click
		''' </summary>
		Public Function HandleFinishClick() As Boolean Implements INewPaymentUserControlFormMediator.HandleFinishClick

			If ValidateData() Then
				m_ucPageCreatePaymentData.CreatePayment()

				Return True
			Else
				m_UIUtility.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Bitte prüfen Sie die Eingaben."))
				Return False
			End If
		End Function


		''' <summary>
		''' Validates all data.
		''' </summary>
		Public Function ValidateData() As Boolean Implements INewPaymentUserControlFormMediator.ValidateData
			Return m_NewPaymentFrm.ValidateData()
		End Function


#End Region


#Region "Public Properties"

		ReadOnly Property InitPaymentDataPage1 As InitPaymentDataPage1 Implements INewPaymentUserControlFormMediator.InitPaymentDataPage1
			Get
				Return m_ucPageSelectMandantPayment.SelecteData

			End Get
		End Property

		''' <summary>
		''' Gets the common db access object.
		''' </summary>
		Public ReadOnly Property CommonDbAccess As ICommonDatabaseAccess Implements INewPaymentUserControlFormMediator.CommonDbAccess
			Get
				Return m_NewPaymentFrm.CommonDbAccess
			End Get
		End Property

		''' <summary>
		''' Gets the invoice db access object.
		''' </summary>
		Public ReadOnly Property InvoiceDbAccess As IInvoiceDatabaseAccess Implements INewPaymentUserControlFormMediator.InvoiceDbAccess
			Get
				Return m_NewPaymentFrm.InvoiceDbAccess
			End Get
		End Property

		''' <summary>
		''' Gets the invoice db access object.
		''' </summary>
		Public ReadOnly Property TableDbAccess As ITablesDatabaseAccess Implements INewPaymentUserControlFormMediator.TableDbAccess
			Get
				Return m_NewPaymentFrm.TableDbAccess
			End Get
		End Property

#End Region

	End Class

End Namespace

