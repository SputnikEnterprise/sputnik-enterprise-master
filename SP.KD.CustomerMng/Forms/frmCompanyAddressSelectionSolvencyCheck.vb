
Imports SPS.ExternalServices.DeltavistaWebService
Imports SPS.ExternalServices
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

''' <summary>
''' Company adddress selection for solvency check.
''' </summary>
Public Class frmCompanyAddressSelectionSolvencyCheck

#Region "Private Fields"

  ''' <summary>
  ''' The translation value helper.
  ''' </summary>
  Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

  ''' <summary>
  ''' The candidates.
  ''' </summary>
  Private m_Candidates As Candidate()

  ''' <summary>
  ''' The choosed company address identifier.
  ''' </summary>
  Private m_ChossedCompanyAddressID As Identifier

  ''' <summary>
  ''' UI Utility functions.
  ''' </summary>
  Protected m_UtilityUI As UtilityUI

  ''' <summary>
  ''' The logger.
  ''' </summary>
  Private Shared m_Logger As ILogger

#End Region

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  Public Sub New(ByVal candiates As Candidate(),
                ByVal translate As SP.Infrastructure.Initialization.TranslateValuesHelper)

    ' Dieser Aufruf ist für den Designer erforderlich.
    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Me.gvAddresses.OptionsView.ShowIndicator = False

    m_UtilityUI = New UtilityUI
    m_Logger = New Logger()

    m_Candidates = candiates
    m_Translate = translate

    TranslateControls()

  End Sub

#End Region

#Region "Private Properties"


  ''' <summary>
  '''  Translate controls.
  ''' </summary>
  Private Sub TranslateControls()

    Me.Text = m_translate.GetSafeTranslationValue(Me.Text)

    Me.btnOk.Text = m_translate.GetSafeTranslationValue(Me.btnOk.Text)
    Me.btnCancel.Text = m_translate.GetSafeTranslationValue(Me.btnCancel.Text)

    Me.lblwaehlen.Text = m_translate.GetSafeTranslationValue(Me.lblwaehlen.Text)

  End Sub

  ''' <summary>
  ''' Gets the selected company address data.
  ''' </summary>
  ''' <returns>The selected company address or nothing if none is selected.</returns>
  Private ReadOnly Property SelectedCompanyAddressData As CompanyAddressViewData
    Get
      Dim grdView = TryCast(grdAddresses.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

      If Not (grdView Is Nothing) Then

        Dim selectedRows = grdView.GetSelectedRows()

        If (selectedRows.Count > 0) Then
          Dim document = CType(grdView.GetRow(selectedRows(0)), CompanyAddressViewData)
          Return document
        End If

      End If

      Return Nothing
    End Get

  End Property

#End Region

#Region "Public Properties"

  ''' <summary>
  ''' Gets the choosed company address identifier.
  ''' </summary>
  ''' <returns>The choosed company address.</returns>
  Public ReadOnly Property ChoosedCompanyAddressIdentifier As Identifier
    Get
      Return m_ChossedCompanyAddressID

    End Get
  End Property

#End Region

#Region "Private Methods"

  ''' <summary>
  ''' Resets the document grid.
  ''' </summary>
  Private Sub ResetCompanyAddressGrid()

    ' Reset the grid
    gvAddresses.Columns.Clear()

    Dim columnCompany As New DevExpress.XtraGrid.Columns.GridColumn()
    columnCompany.Caption = m_translate.GetSafeTranslationValue("Firma")
    columnCompany.Name = "Company"
    columnCompany.FieldName = "Company"
    columnCompany.Visible = True
    gvAddresses.Columns.Add(columnCompany)

    Dim columnAddress As New DevExpress.XtraGrid.Columns.GridColumn()
    columnAddress.Caption = m_translate.GetSafeTranslationValue("Adresse")
    columnAddress.Name = "Address"
    columnAddress.FieldName = "Address"
    columnAddress.Visible = True
    gvAddresses.Columns.Add(columnAddress)

    grdAddresses.DataSource = Nothing

  End Sub

  ''' <summary>
  ''' Handles form load event.
  ''' </summary>
  Private Sub OnAddressSelectionSolvencyCheck_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    ' Reset the company address grid.
    ResetCompanyAddressGrid()

    If Not m_Candidates Is Nothing Then

      ' Create view data fro grid
      Dim listOfCompanyAddressViewData As New List(Of CompanyAddressViewData)

      For Each candiate In m_Candidates
        Dim companyAddressViewData = New CompanyAddressViewData

        Try

          Dim companyAddressDesc As CompanyAddressDescription = CType(candiate.address, CompanyAddressDescription)

          ' The company name
          companyAddressViewData.Company = companyAddressDesc.companyName

          ' The address
          If (Not candiate.address.location Is Nothing) Then
            Dim location = candiate.address.location
            companyAddressViewData.Address = String.Format("{0} {1}-{2}, {3}", location.street, location.country, location.zip, location.city)

          End If

          ' the address identifier
          Dim addressIdentifer = (From identifier In candiate.identifiers Where identifier.identifierType = IdentifierType.ADDRESS_ID).FirstOrDefault()

          If Not addressIdentifer Is Nothing Then
            companyAddressViewData.AddressIdentifer = New Identifier With {.identifierText = addressIdentifer.identifierText,
                                                                     .identifierType = addressIdentifer.identifierType}
            listOfCompanyAddressViewData.Add(companyAddressViewData)
          Else
            m_Logger.LogWarning(String.Format(m_translate.GetSafeTranslationValue("Address identifier was not present in identifier collection. Company = {0}"),
                                              companyAddressViewData.Company))
          End If

        Catch ex As Exception
          m_Logger.LogError(ex.ToString())
          m_ChossedCompanyAddressID = Nothing
          Close()
        End Try
      Next

      grdAddresses.DataSource = listOfCompanyAddressViewData
    Else
      Close()
    End If

  End Sub

  ''' <summary>
  ''' Handles click on ok button.
  ''' </summary>
  Private Sub OnBtnOk_Click(sender As System.Object, e As System.EventArgs) Handles btnOk.Click
    m_ChossedCompanyAddressID = SelectedCompanyAddressData.AddressIdentifer
    Close()
  End Sub

  ''' <summary>
  ''' Handles click on cancel button.
  ''' </summary>
  Private Sub OnBtnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
    m_ChossedCompanyAddressID = Nothing
    Close()
  End Sub

#End Region

#Region "View helper classes"

  ''' <summary>
  ''' View data for company address.
  ''' </summary>
  Private Class CompanyAddressViewData
    Public Property AddressIdentifer As Identifier
    Public Property Company As String
    Public Property Address As String

  End Class

#End Region

End Class