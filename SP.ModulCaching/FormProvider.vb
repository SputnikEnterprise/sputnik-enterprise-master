Imports SP.Infrastructure.Caching
Imports SPProgUtility.Mandanten
Imports System.Windows.Forms

''' <summary>
''' Creates forms for modul cache.
''' </summary>
Public Class FormProvider
    Implements IFormProvider

#Region "Private Fields"

    Private m_MDNr As Integer
    Private m_UsNr As Integer

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="mdNr">The mandant number.</param>
    ''' <param name="usNr">The user number.</param>
    Public Sub New(ByVal mdNr As Integer, ByVal usNr As Integer)
        m_MDNr = mdNr
        m_UsNr = usNr
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Profides a new form of a given type..
    ''' </summary>
    ''' <param name="type">The type.</param>
    ''' <returns>The new form.</returns>
    Public Function ProvideNewFormOfType(type As System.Type) As System.Windows.Forms.Form Implements Infrastructure.Caching.IFormProvider.ProvideNewFormOfType

        Dim m_md As New Mandant
        Dim clsMandant = m_md.GetSelectedMDData(m_MDNr)
        Dim logedUserData = m_md.GetSelectedUserData(m_MDNr, m_UsNr)
        Dim personalizedData = m_md.GetPersonalizedCaptionInObject(m_MDNr)

        Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
        Dim translate = clsTransalation.GetTranslationInObject

        Dim initClass = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

        Dim frm As Form = Nothing

		If type Is GetType(SP.MA.EmployeeMng.UI.frmEmployees) Then
			frm = New SP.MA.EmployeeMng.UI.frmEmployees(initClass)
		ElseIf type Is GetType(SP.KD.CustomerMng.UI.frmCustomers) Then
			frm = New SP.KD.CustomerMng.UI.frmCustomers(initClass)
		ElseIf type Is GetType(SP.MA.EinsatzMng.UI.frmES) Then
			frm = New SP.MA.EinsatzMng.UI.frmES(initClass)
		ElseIf type Is GetType(SP.KD.InvoiceMng.UI.frmInvoices) Then
			frm = New SP.KD.InvoiceMng.UI.frmInvoices(initClass)
		ElseIf type Is GetType(SP.MA.ReportMng.UI.frmReportMng) Then
			frm = New SP.MA.ReportMng.UI.frmReportMng(initClass)
		ElseIf type Is GetType(SP.MA.AdvancePaymentMng.UI.frmAdvancePayments) Then
			frm = New SP.MA.AdvancePaymentMng.UI.frmAdvancePayments(initClass)
		ElseIf type Is GetType(SP.MA.PayrollMng.UI.frmPayroll) Then
			frm = New SP.MA.PayrollMng.UI.frmPayroll(initClass)
		ElseIf type Is GetType(SP.MA.ApplicantMng.UI.frmApplicant) Then
			frm = New SP.MA.ApplicantMng.UI.frmApplicant(initClass)
		ElseIf type Is GetType(SPkd.Vakanz.frmVakanzen) Then
			frm = New SPKD.Vakanz.frmVakanzen(initClass)
		ElseIf type Is GetType(SPProposeUtility.frmPropose) Then
			frm = New SPProposeUtility.frmPropose(initClass, Nothing)

		ElseIf type Is GetType(SP.KD.CPersonMng.UI.frmResponsiblePerson) Then
			frm = New SP.KD.CPersonMng.UI.frmResponsiblePerson(initClass)


		Else
			frm = Nothing
        End If

        Return frm

    End Function

#End Region
End Class
