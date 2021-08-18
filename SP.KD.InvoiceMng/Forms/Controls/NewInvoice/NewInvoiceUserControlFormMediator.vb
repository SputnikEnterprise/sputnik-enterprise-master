
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.ES
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Invoice
Imports SP.DatabaseAccess.Common.DataObjects

Namespace UI

  ''' <summary>
  ''' Mediator to communicate between user controls (only for new invoice wizard).
  ''' </summary>
  Public Class NewInvoiceUserControlFormMediator
    Implements INewInvoiceUserControlFormMediator

#Region "Private Fields"

    Private m_NewInvoiceFrm As frmNewInvoice

    Private m_ucPageSelectMandant As ucPageSelectMandant

    Private m_ucPageSelectInvoiceData As ucPageSelectInvoiceData

    Private m_ucPageCreateInvoiceData As ucPageCreateInvoice

    Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

    Private m_UIUtility As SP.Infrastructure.UI.UtilityUI

#End Region

#Region "Constructor"

    Public Sub New(ByVal frmNewInvoice As frmNewInvoice,
                  ByVal pageSelectMandant As ucPageSelectMandant,
                  ByVal pageSelectInvoiceData As ucPageSelectInvoiceData,
                  ByVal pageCreateInvoice As ucPageCreateInvoice,
                  ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper)

      m_UIUtility = New SP.Infrastructure.UI.UtilityUI

      Me.m_NewInvoiceFrm = frmNewInvoice
      Me.m_ucPageSelectMandant = pageSelectMandant
      Me.m_ucPageSelectInvoiceData = pageSelectInvoiceData
      Me.m_ucPageCreateInvoiceData = pageCreateInvoice
      Me.m_Translate = translate

      m_ucPageSelectMandant.UCMediator = Me
      m_ucPageSelectInvoiceData.UCMediator = Me
      m_ucPageCreateInvoiceData.UCMediator = Me

    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Handles change of mandant.
    ''' </summary>
    ''' <param name="mdNumber">The mandant number.</param>
    Public Sub HandleChageOfMandant(ByVal mdNumber As Integer) Implements INewInvoiceUserControlFormMediator.HandleChangeOfMandant
      m_NewInvoiceFrm.ChangeMandant(mdNumber)
      m_ucPageSelectInvoiceData.Reset()
      m_ucPageCreateInvoiceData.Reset()
    End Sub

    ''' <summary>
    ''' Handle finish click
    ''' </summary>
    Public Function HandleFinishClick() As Boolean Implements INewInvoiceUserControlFormMediator.HandleFinishClick

      If ValidateData() Then
        m_ucPageCreateInvoiceData.CreateInvoice()

        Return True
      Else
        m_UIUtility.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Bitte prüfen Sie die Eingaben."))
        Return False
      End If
    End Function


    ''' <summary>
    ''' Validates all data.
    ''' </summary>
    Public Function ValidateData() As Boolean Implements INewInvoiceUserControlFormMediator.ValidateData
      Return m_NewInvoiceFrm.ValidateData()
    End Function

#End Region

#Region "Public Properties"


    ReadOnly Property InitDataPage1 As InitDataPage1 Implements INewInvoiceUserControlFormMediator.InitDataPage1
      Get
        Return m_ucPageSelectMandant.SelecteData

      End Get
    End Property
    ReadOnly Property InitDataPage2 As InitDataPage2 Implements INewInvoiceUserControlFormMediator.InitDataPage2
      Get
        Return m_ucPageSelectInvoiceData.SelecteData
      End Get
    End Property

    ''' <summary>
    ''' Gets the common db access object.
    ''' </summary>
    Public ReadOnly Property CommonDbAccess As ICommonDatabaseAccess Implements INewInvoiceUserControlFormMediator.CommonDbAccess
      Get
        Return m_NewInvoiceFrm.CommonDbAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the invoice db access object.
    ''' </summary>
    Public ReadOnly Property InvoiceDbAccess As IInvoiceDatabaseAccess Implements INewInvoiceUserControlFormMediator.InvoiceDbAccess
      Get
        Return m_NewInvoiceFrm.InvoiceDbAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the customer db access object.
    ''' </summary> 
    Public ReadOnly Property CustomerDbAccess As ICustomerDatabaseAccess Implements INewInvoiceUserControlFormMediator.CustomerDbAccess
      Get
        Return m_NewInvoiceFrm.CustomerDbAccess
      End Get
    End Property

#End Region

  End Class

End Namespace