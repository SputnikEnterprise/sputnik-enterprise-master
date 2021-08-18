Imports SP.Infrastructure.Caching

Public Class ModuleCache

#Region "Private Consts"
  Private Const DEFAULT_CACHE_SIZE As Integer = 2
  '  "<Mdnr>_<UsNr>"
  Private Const CACHE_KEY_PATTERN = "{0}_{1}"
#End Region

#Region "Private Fields"

  ' Key Format: "<Mdnr>_<UsNr>"
  Private m_ModuleCaches As Dictionary(Of String, FormCache)

#End Region

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  Public Sub New()

    m_ModuleCaches = New Dictionary(Of String, FormCache)

    MaxCustomerFormsToCache = DEFAULT_CACHE_SIZE
    MaxResponsiblePersonFormsToCache = DEFAULT_CACHE_SIZE
    MaxEmployeeFormsToCache = DEFAULT_CACHE_SIZE
    MaxESFormsToCache = DEFAULT_CACHE_SIZE
    MaxInvoiceFormsToCache = DEFAULT_CACHE_SIZE
    MaxReportFormsToCache = DEFAULT_CACHE_SIZE
    MaxAdvancePaymentFormsToCache = DEFAULT_CACHE_SIZE
    MaxPayrollFormsToCache = DEFAULT_CACHE_SIZE
		MaxApplicantFormsToCache = DEFAULT_CACHE_SIZE
	End Sub

#End Region

#Region "Public Properties"

  Public Property MaxCustomerFormsToCache As Integer
  Public Property MaxResponsiblePersonFormsToCache As Integer
  Public Property MaxEmployeeFormsToCache As Integer
  Public Property MaxESFormsToCache As Integer
  Public Property MaxInvoiceFormsToCache As Integer
  Public Property MaxReportFormsToCache As Integer
  Public Property MaxAdvancePaymentFormsToCache As Integer
  Public Property MaxPayrollFormsToCache As Integer
	Public Property MaxApplicantFormsToCache As Integer

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Gets a module. 
  ''' </summary>
  ''' <param name="mdNr">The mandant number.</param>
  ''' <param name="usNr">The user number.</param>
  ''' <param name="moduleName">The module name.</param>
  ''' <returns>The module form.</returns>
  Public Function GetModuleForm(ByVal mdNr As Integer, ByVal usNr As Integer, ByVal moduleName As ModuleName) As System.Windows.Forms.Form

    Dim moduleCache As FormCache = GetModuleCache(mdNr, usNr)

    Dim frm As System.Windows.Forms.Form = Nothing
    Select Case moduleName
      Case ModuleCaching.ModuleName.CustomerMng
        frm = moduleCache.GetFormByType(GetType(SP.KD.CustomerMng.UI.frmCustomers))
      Case ModuleCaching.ModuleName.EmployeeMng
        frm = moduleCache.GetFormByType(GetType(SP.MA.EmployeeMng.UI.frmEmployees))
      Case ModuleCaching.ModuleName.ESMng
        frm = moduleCache.GetFormByType(GetType(SP.MA.EinsatzMng.UI.frmES))
      Case ModuleCaching.ModuleName.InvoiceMng
        frm = moduleCache.GetFormByType(GetType(SP.KD.InvoiceMng.UI.frmInvoices))
      Case ModuleCaching.ModuleName.ReportMng
        frm = moduleCache.GetFormByType(GetType(SP.MA.ReportMng.UI.frmReportMng))
      Case ModuleCaching.ModuleName.AdvancePaymentMng
        frm = moduleCache.GetFormByType(GetType(SP.MA.AdvancePaymentMng.UI.frmAdvancePayments))
      Case ModuleCaching.ModuleName.PayrollMng
        frm = moduleCache.GetFormByType(GetType(SP.MA.PayrollMng.UI.frmPayroll))
			Case ModuleCaching.ModuleName.ApplicantMng
				frm = moduleCache.GetFormByType(GetType(SP.MA.applicantMng.UI.frmapplicant))

			Case ModuleCaching.ModuleName.VacancyMng
				frm = moduleCache.GetFormByType(GetType(SPKD.Vakanz.frmVakanzen))
			Case ModuleCaching.ModuleName.ProposeMng
				frm = moduleCache.GetFormByType(GetType(SPProposeUtility.frmPropose))
			Case ModuleCaching.ModuleName.CResponsibleMng
				frm = moduleCache.GetFormByType(GetType(SP.KD.CPersonMng.UI.frmResponsiblePerson))


			Case Else
				frm = Nothing
		End Select

    Return frm

  End Function

  ''' <summary>
  ''' Clears all caches.
  ''' </summary>
  Public Sub ClearAllCaches()

    For Each cacheKey In m_ModuleCaches.Keys

      Dim mdNrAndUsNr = GetTupleOfMDNrAndUSNrFromCacheKey(cacheKey)

      Dim cache As FormCache = GetModuleCache(mdNrAndUsNr.Item1, mdNrAndUsNr.Item2)

      cache.ClearCache()

    Next

  End Sub

  ''' <summary>
  ''' Clears the cache of a specific mandant.
  ''' </summary>
  ''' <param name="mdnr">The mandant number.</param>
  ''' <param name="usnr">The user number.</param>
  Public Sub ClearCache(ByVal mdnr As Integer, ByVal usNr As Integer)

    Dim cacheKey As String = GetCacheKeyFromMDNrAndUsNr(mdnr, usNr)

    If m_ModuleCaches.Keys.Contains(cacheKey) Then
      Dim cache As FormCache = GetModuleCache(mdnr, usNr)
      cache.ClearCache()
    End If

  End Sub

#End Region

#Region "Private Methods"

  ''' <summary>
  ''' Gets a module cache.
  ''' </summary>
  ''' <param name="mdNr">The mandant number.</param>
  ''' <param name="usNr">The user number.</param>
  ''' <returns>The module form cache.</returns>
  Private Function GetModuleCache(ByVal mdNr As Integer, ByVal usNr As Integer) As FormCache

    Dim cacheKey As String = GetCacheKeyFromMDNrAndUsNr(mdNr, usNr)

    If Not m_ModuleCaches.Keys.Contains(cacheKey) Then
      Dim formProvider As New FormProvider(mdNr, usNr)
      Dim newFormCache As New FormCache(formProvider)
      RegisterForms(newFormCache)
      m_ModuleCaches.Add(cacheKey, newFormCache)
    End If

    Return m_ModuleCaches(cacheKey)

  End Function

  ''' <summary>
  ''' Register forms.
  ''' </summary>
  ''' <param name="formCache">The form cache.</param>
  Private Sub RegisterForms(ByVal formCache As FormCache)

    formCache.RegisterForm(GetType(SP.KD.CustomerMng.UI.frmCustomers), MaxCustomerFormsToCache)
    formCache.RegisterForm(GetType(SP.MA.EmployeeMng.UI.frmEmployees), MaxEmployeeFormsToCache)
    formCache.RegisterForm(GetType(SP.MA.EinsatzMng.UI.frmES), MaxESFormsToCache)
    formCache.RegisterForm(GetType(SP.KD.InvoiceMng.UI.frmInvoices), MaxInvoiceFormsToCache)
    formCache.RegisterForm(GetType(SP.MA.ReportMng.UI.frmReportMng), MaxReportFormsToCache)
    formCache.RegisterForm(GetType(SP.MA.AdvancePaymentMng.UI.frmAdvancePayments), MaxAdvancePaymentFormsToCache)
    formCache.RegisterForm(GetType(SP.MA.PayrollMng.UI.frmPayroll), MaxPayrollFormsToCache)
		formCache.RegisterForm(GetType(SP.MA.ApplicantMng.UI.frmApplicant), MaxApplicantFormsToCache)
	End Sub

  ''' <summary>
  ''' Gets a cache key from an MDNr and an USNr.
  ''' </summary>
  ''' <param name="mdNr">The MDNr.</param>
  ''' <param name="usNr">The USNr.</param>
  ''' <returns>Combined cache key string form MDNr and USNr.</returns>
  Private Function GetCacheKeyFromMDNrAndUsNr(ByVal mdNr As Integer, ByVal usNr As Integer) As String
    Dim cacheKey As String = String.Format(CACHE_KEY_PATTERN, mdNr, usNr)

    Return cacheKey
  End Function

  ''' <summary>
  ''' Gets a tuple of MDNr and USNr form cache key.
  ''' </summary>
  ''' <param name="cacheKey">The cache key (MDNr_USNr).</param>
  ''' <returns>Tuple of MDNr and USNr.</returns>
  Private Function GetTupleOfMDNrAndUSNrFromCacheKey(ByVal cacheKey As String) As Tuple(Of Integer, Integer)
    Dim mdNr As Integer = 0
    Dim usNr As Integer = 0

    Dim tokens = cacheKey.Split("_")
    mdNr = tokens(0)
    usNr = tokens(1)

    Dim tuple As New Tuple(Of Integer, Integer)(mdNr, usNr)

    Return tuple
  End Function

#End Region

End Class
