
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Settings
Imports SP.KD.CustomerMng.Settings
Imports System.Threading.Tasks
Imports System.Threading
Imports SPS.ExternalServices.DeltavistaWebService
Imports SPS.ExternalServices
Imports System.ComponentModel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Repository
Imports SPProgUtility.CommonSettings

''' <summary>
''' The solvency check result.
''' </summary>
Public Class frmSolvencyResult

#Region "Private Fields"

  ''' <summary>
  ''' The Initialization data.
  ''' </summary>
  Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

  ''' <summary>
  ''' The translation value helper.
  ''' </summary>
  Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

  ''' <summary>
  ''' The credit info customer number.
  ''' </summary>
  Private m_CICustomerNumber As Integer

  ''' <summary>
  ''' The credit info record number.
  ''' </summary>
  Private m_CIRecordNumber As Integer

  ''' <summary>
  ''' The data access object.
  ''' </summary>
  Private m_DataAccess As ICustomerDatabaseAccess

  ''' <summary>
  ''' The settings manager.
  ''' </summary>
  Protected m_SettingsManager As ISettingsManager

  ''' <summary>
  ''' Utility functions.
  ''' </summary>
  Private m_UtilityUI As UtilityUI

  ''' <summary>
  ''' Utility functions.
  ''' </summary>
  Private m_Utility As Utility

  ''' <summary>
  ''' The logger.
  ''' </summary>
  Private Shared m_Logger As ILogger

  ''' <summary>
  ''' The customer credit info data.
  ''' </summary>
  Private m_CustomerCreditInfoData As CustomerAssignedCreditInfo

  ''' <summary>
  ''' The reference number for the deltavista web service.
  ''' </summary>
  Private m_ReferenceNumber As String

  ''' <summary>
  ''' The user name for the deltavista web service.
  ''' </summary>
  Private m_DeltavistaUsername As String

  ''' <summary>
  ''' The password for the deltavista web service.
  ''' </summary>
  Private m_DeltavistaPassword As String

  ''' <summary>
  ''' The service url for the deltavista web service.
  ''' </summary>
  Private m_DeltavistaServiceUrl As String

  ''' <summary>
  ''' Item memo edit repository used for word wrap (multiline) in debts grid.
  ''' </summary>
  Private m_ItemMemoEdit As RepositoryItemMemoEdit

  ''' <summary>
  ''' Boolean flag indicating if form has been closed.
  ''' </summary>
  Private m_IsFormClosed As Boolean

#End Region

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  ''' <param name="creditInfoCustomerNumber">The credit info customer number.</param>
  ''' <param name="creditInfoRecordNumber">The credit info record number</param>
  ''' <param name="deltavistaReferenceNumber">The deltavista webservice reference number.</param>
  ''' <param name="deltavistaWSUserName">The deltavista webservice user name.</param>
  ''' <param name="deltavistaWSPassword">The deltavista webservice password.</param>
  ''' <param name="deltavistaWSUrl">The deltavista webservice url.</param>
  ''' <param name="_setting">The settings data.</param>
  Public Sub New(ByVal creditInfoCustomerNumber As Integer, ByVal creditInfoRecordNumber As Integer,
                 ByVal deltavistaReferenceNumber As String,
                 ByVal deltavistaWSUserName As String,
                 ByVal deltavistaWSPassword As String,
                 ByVal deltavistaWSUrl As String,
                 ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

    ' Dieser Aufruf ist für den Designer erforderlich.
    InitializeComponent()

    m_InitializationData = _setting
    m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Me.gvDebts.OptionsView.ShowIndicator = False
    m_CICustomerNumber = creditInfoCustomerNumber
    m_CIRecordNumber = creditInfoRecordNumber

    m_DataAccess = New SP.DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
    m_SettingsManager = New SettingsManager
    m_UtilityUI = New UtilityUI
    m_Utility = New Utility
    m_Logger = New Logger

    ' Required config data for deltavista web service
    m_ReferenceNumber = deltavistaReferenceNumber
    m_DeltavistaUsername = deltavistaWSUserName
    m_DeltavistaPassword = deltavistaWSPassword
    m_DeltavistaServiceUrl = deltavistaWSUrl

    m_ItemMemoEdit = New RepositoryItemMemoEdit
    grdDebts.RepositoryItems.Add(m_ItemMemoEdit)

    translateControls()

  End Sub

#End Region

#Region "Private Methods"

  Private Sub translateControls()

    Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

    Me.lblgefundeneadresse.Text = m_Translate.GetSafeTranslationValue(Me.lblgefundeneadresse.Text)
    Me.lbldatum.Text = m_Translate.GetSafeTranslationValue(Me.lbldatum.Text)
    Me.lblartAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblartAbfrage.Text)
    Me.lblbenutzer.Text = m_Translate.GetSafeTranslationValue(Me.lblbenutzer.Text)
    Me.lblentscheidung.Text = m_Translate.GetSafeTranslationValue(Me.lblentscheidung.Text)
    Me.grpzahlungserfahrung.Text = m_Translate.GetSafeTranslationValue(Me.grpzahlungserfahrung.Text)
    Me.btnShowSolvencyPDFReport.Text = m_Translate.GetSafeTranslationValue(Me.btnShowSolvencyPDFReport.Text)

  End Sub

  ''' <summary>
  ''' Handles form load event.
  ''' </summary>
  Private Sub OnFrmSolvencyResult_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    ResetDebtsGrid()

    Dim success = LoadSolvencyCheckData()

    If (success) Then
      RequestDeptDetailsOverWebService(m_CustomerCreditInfoData.DV_FoundedAddressID)
    End If

    If Not success Then
      m_UtilityUI.ShowErrorDialog(m_translate.GetSafeTranslationValue("Prüfungsergebnisse konnten nicht geladen werden."))
      Close()
    End If

  End Sub

  ''' <summary>
  ''' Handles form closed event.
  ''' </summary>
  Private Sub OnFrmSolvencyResult_FormClosed(sender As System.Object, e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
    m_IsFormClosed = True
  End Sub

  ''' <summary>
  ''' Load the solvency check data.
  ''' </summary>
  Private Function LoadSolvencyCheckData() As Boolean

    Dim listOfCreditInfo = m_DataAccess.LoadAssignedCreditInfosOfCustomer(m_CICustomerNumber, m_CIRecordNumber, True)

    If listOfCreditInfo Is Nothing OrElse Not listOfCreditInfo.Count = 1 Then
      Return False
    End If

    Dim creditInfo = listOfCreditInfo(0)

    memoFoundedAddress.Text = creditInfo.DV_FoundedAddress
    txtDecisionText.Text = creditInfo.DV_DecisionText

    lblDateValue.Text = creditInfo.CreatedOn.Value
    lblUser.Text = creditInfo.CreatedFrom

    If creditInfo.DV_DecisionID.HasValue Then
      Try
        Dim decision = (CType(creditInfo.DV_DecisionID, DecisionResult))

        Select Case decision
          Case DecisionResult.LightGreen
            btnSolvencyDecision.Image = My.Resources.bullet_green_small
          Case DecisionResult.Green
            btnSolvencyDecision.Image = My.Resources.bullet_green_small
          Case DecisionResult.YellowGreen
            btnSolvencyDecision.Image = My.Resources.bullet_green_small
          Case DecisionResult.Yellow
            btnSolvencyDecision.Image = My.Resources.bullet_yellow_small
          Case DecisionResult.Orange
            btnSolvencyDecision.Image = My.Resources.bullet_yellow_small
          Case DecisionResult.Red
            btnSolvencyDecision.Image = My.Resources.bullet_red_small
          Case DecisionResult.DarkRed
            btnSolvencyDecision.Image = My.Resources.bullet_red_small
          Case Else
            btnSolvencyDecision.Visible = False
            btnSolvencyDecision.Image = Nothing
        End Select
      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
      End Try

    End If

    Select Case creditInfo.DV_QueryType
      Case BusinessSolvencyCheckType.QuickBusinessCheck
        lblTypeOfQuery.Text = m_translate.GetSafeTranslationValue("Quick Business Prüfung")
      Case BusinessSolvencyCheckType.BusinessCheck
        lblTypeOfQuery.Text = m_translate.GetSafeTranslationValue("Business Prüfung")
      Case Else
        lblTypeOfQuery.Text = "-"
    End Select

    m_CustomerCreditInfoData = creditInfo

    Return True
  End Function

  ''' <summary>
  ''' Resets the document grid.
  ''' </summary>
  Private Sub ResetDebtsGrid()

    ' Reset the grid
    gvDebts.Columns.Clear()

    Dim originColumn As New DevExpress.XtraGrid.Columns.GridColumn()
    originColumn.Caption = m_translate.GetSafeTranslationValue("Herkunft")
    originColumn.Name = "Origin"
    originColumn.FieldName = "Origin"
    originColumn.Visible = True
    gvDebts.Columns.Add(originColumn)

    Dim dateColumn As New DevExpress.XtraGrid.Columns.GridColumn()
    dateColumn.Caption = m_translate.GetSafeTranslationValue("Datum")
    dateColumn.Name = "DeptDate"
    dateColumn.FieldName = "DeptDate"
    dateColumn.Visible = True
    dateColumn.Width = 80
    dateColumn.OptionsColumn.FixedWidth = True
    gvDebts.Columns.Add(dateColumn)

    Dim amountColumn As New DevExpress.XtraGrid.Columns.GridColumn()
    amountColumn.Caption = m_translate.GetSafeTranslationValue("Betrag")
    amountColumn.Name = "Amount"
    amountColumn.FieldName = "Amount"
    amountColumn.Visible = True
    amountColumn.Width = 80
    amountColumn.OptionsColumn.FixedWidth = True
    amountColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    amountColumn.DisplayFormat.FormatString = "N"
    gvDebts.Columns.Add(amountColumn)

    Dim openAmount As New DevExpress.XtraGrid.Columns.GridColumn()
    openAmount.Caption = m_translate.GetSafeTranslationValue("Betrag offen")
    openAmount.Name = "OpenAmount"
    openAmount.FieldName = "OpenAmount"
    openAmount.Visible = True
    openAmount.Width = 80
    openAmount.OptionsColumn.FixedWidth = True
    openAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
    openAmount.DisplayFormat.FormatString = "N"
    gvDebts.Columns.Add(openAmount)

    Dim riskClassColumn As New DevExpress.XtraGrid.Columns.GridColumn()
    riskClassColumn.Caption = " "
    riskClassColumn.Name = "RiskClass"
    riskClassColumn.FieldName = "RiskClass"
    riskClassColumn.Visible = True
    riskClassColumn.Width = 100
    riskClassColumn.OptionsColumn.FixedWidth = True
    riskClassColumn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
    gvDebts.Columns.Add(riskClassColumn)

    Dim paymentStatusColumn As New DevExpress.XtraGrid.Columns.GridColumn()
    paymentStatusColumn.Caption = m_translate.GetSafeTranslationValue("Zahlungsstatus")
    paymentStatusColumn.Name = "PaymentStatus"
    paymentStatusColumn.FieldName = "PaymentStatus"
    paymentStatusColumn.Visible = True
    gvDebts.Columns.Add(paymentStatusColumn)

    Dim commentColumn As New DevExpress.XtraGrid.Columns.GridColumn()
    commentColumn.Caption = m_translate.GetSafeTranslationValue("Kommentar")
    commentColumn.Name = "Comment"
    commentColumn.FieldName = "Comment"
    commentColumn.Visible = True
    commentColumn.ColumnEdit = m_ItemMemoEdit
    gvDebts.Columns.Add(commentColumn)

    grdDebts.DataSource = Nothing

    gvDebts.OptionsView.RowAutoHeight = True

  End Sub

  ''' <summary>
  ''' Requests the dept details over a web service call.
  ''' </summary>
  ''' <param name="addressId">The address id.</param>
  Private Sub RequestDeptDetailsOverWebService(ByVal addressId As String)

    Dim uiSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext()

    gvDebts.ShowLoadingPanel()

    Task(Of TypeGetDebtDetailsResponse).Factory.StartNew(Function() GetDeptDetialsAsync(addressId),
                                            CancellationToken.None,
                                            TaskCreationOptions.None,
                                            TaskScheduler.Default).ContinueWith(Sub(t) FinishTask(t), CancellationToken.None, TaskContinuationOptions.None, uiSynchronizationContext)

  End Sub

  ''' <summary>
  ''' Gets the dept details asynchronous.
  ''' </summary>
  ''' <param name="addressId">The address id.</param>
  ''' <returns>The debt details response.</returns>
  Private Function GetDeptDetialsAsync(ByVal addressId As String) As TypeGetDebtDetailsResponse
    Dim solvencyChecker = New DeltavistaServices(m_DeltavistaUsername, m_DeltavistaPassword, m_DeltavistaServiceUrl)

    Dim addressIdentifer As New Identifier With {.identifierText = addressId, .identifierType = IdentifierType.ADDRESS_ID}
    Dim requestData As New DeptDetailsRequestData(addressIdentifer, m_ReferenceNumber)
    Return solvencyChecker.RequestDebtDetails(requestData)
  End Function


  ''' <summary>
  ''' Finishd the task.
  ''' </summary>
  Private Sub FinishTask(ByVal t As Task(Of TypeGetDebtDetailsResponse))

    Select Case t.Status
      Case TaskStatus.RanToCompletion
        FillDebtsGrid(t.Result.debts)
      Case TaskStatus.Faulted
        m_Logger.LogError(t.Exception.ToString())
      Case Else
        ' do nothing
    End Select

    gvDebts.HideLoadingPanel()

  End Sub

  ''' <summary>
  ''' Fill s the debt grid with data received form the web service.
  ''' </summary>
  ''' <param name="debts">The debts array.</param>
  Private Sub FillDebtsGrid(ByVal debts As DebtEntry())

    If m_IsFormClosed Then
      ' If the form was closed before the web service call returns then ignore the data.
      Return
    End If

    If (debts Is Nothing) Then
      grpzahlungserfahrung.Text = m_translate.GetSafeTranslationValue("Zahlungserfahrungen: (Es sind keine Zahlungserfahrungen zu für diese Adresse hinterlegt)")
      Return
    End If

    Dim listDataSource As BindingList(Of DebitGridViewData) = New BindingList(Of DebitGridViewData)
    For Each entry In debts


      Dim debtViewData As New DebitGridViewData With
          {.Origin = entry.origin,
           .DeptDate = entry.dateOpen,
           .Amount = If(entry.amount Is Nothing, CType(Nothing, Decimal?), CType(entry.amount.amount, Decimal?)),
           .OpenAmount = If(entry.amountOpen Is Nothing, CType(Nothing, Decimal?), CType(entry.amountOpen.amount, Decimal?)),
           .RiskClass = TranslateRiskClass(entry.riskClass),
           .PaymentStatus = entry.paymentStatusText,
           .Comment = entry.text
           }

      listDataSource.Add(debtViewData)
    Next

    grdDebts.DataSource = listDataSource

  End Sub

  ''' <summary>
  ''' Translate a risk class.
  ''' </summary>
  ''' <param name="riskClass">The risk class.</param>
  ''' <returns>The transalte text for the risk class.</returns>
  Private Function TranslateRiskClass(ByVal riskClass As RiskClass)

    Select Case riskClass
      Case DeltavistaWebService.RiskClass.NO_NEGATIVE
        Return "NO_NEGATIVE"
      Case DeltavistaWebService.RiskClass.PRE_LEGAL
        Return "PRE_LEGAL"
      Case DeltavistaWebService.RiskClass.LEGAL_INITIAL
        Return "LEGAL_INITIAL"
      Case DeltavistaWebService.RiskClass.LEGAL_ESCALATION
        Return "LEGAL_ESCALATION"
      Case DeltavistaWebService.RiskClass.LEGAL_DEFAULTED
        Return "LEGAL_DEFAULTED"
      Case DeltavistaWebService.RiskClass.UNKNOWN
        Return "UNKNOWN"
      Case Else
        Return String.Empty
    End Select

  End Function

  ''' <summary>
  ''' Handles click on show solvency report button.
  ''' </summary>
  Private Sub OnBtnShowSolvencyPDFReport_Click(sender As System.Object, e As System.EventArgs) Handles btnShowSolvencyPDFReport.Click

    If Not m_CustomerCreditInfoData Is Nothing AndAlso
        Not m_CustomerCreditInfoData.DV_PDFFile Is Nothing Then

      ' Copy bytes to temp file and show the report
      Dim bytes() = m_CustomerCreditInfoData.DV_PDFFile
      Dim tempFileName = System.IO.Path.GetTempFileName()
      Dim tempFileFinal = System.IO.Path.ChangeExtension(tempFileName, "pdf")

      If (Not bytes Is Nothing AndAlso m_Utility.WriteFileBytes(tempFileFinal, bytes)) Then
        m_Utility.OpenFileWithDefaultProgram(tempFileFinal)

      End If

    End If

  End Sub

#End Region

#Region "View helper classes"

  ''' <summary>
  ''' View data for existing customers.
  ''' </summary>
  Private Class DebitGridViewData

    Public Property Origin As String
    Public Property DeptDate As Date?
    Public Property Amount As Decimal?
    Public Property OpenAmount As Decimal?
    Public Property RiskClass As String
    Public Property PaymentStatus As String
    Public Property Comment As String

  End Class

#End Region

End Class