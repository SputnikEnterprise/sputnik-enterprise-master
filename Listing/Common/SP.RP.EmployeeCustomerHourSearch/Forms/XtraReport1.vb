

Imports System.ComponentModel
Imports System.Drawing.Printing
Imports DevExpress.XtraBars
Imports DevExpress.XtraPrinting.Preview
Imports DevExpress.XtraReports.UI
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.Infrastructure.Logging

Public Class XtraReport1


	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper



#Region "public properties"

	Public Property InitialData As SP.Infrastructure.Initialization.InitializeClass
	Public Property CustomerLinesToPrintData As List(Of CustomerReportLinesPrintData)
	Public Property m_ListingDatabaseAccess As IListingDatabaseAccess


#End Region


#Region "Constructor"

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		'BindingSource1.DataSource = CustomerLinesToPrintData

	End Sub

#End Region


#Region "Public Methods"

	Public Function LoadData() As Boolean
		Dim result As Boolean = True

		InitialNewData()
		BindingSource1.DataSource = CustomerLinesToPrintData

		LoadMDUserData()

		Dim Tool As ReportPrintTool = New ReportPrintTool(Me)
		'Dim myItem As BarItem = New BarButtonItem() With {.Caption = "Mit Details"}
		'Dim manager As BarManager = TryCast((TryCast(Tool.PreviewForm, PrintPreviewFormEx)).PrintBarManager, BarManager)
		'manager.Bars("Toolbar").AddItem(myItem)

		'AddHandler myItem.ItemClick, AddressOf myItem_ItemClick
		Tool.ShowPreview()


		Return result

	End Function

	Private Sub InitialNewData()

		m_InitializationData = InitialData
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		Me.PaperKind = Printing.PaperKind.A4
		Me.Margins.Left = 20
		Me.Margins.Right = 20

	End Sub

	Private Sub XtraReport1_BeforePrint(sender As Object, e As PrintEventArgs) Handles Me.BeforePrint

		'lblCostcenterName.DataBindings.Add(New XRBinding("Text", CustomerLinesToPrintData, ""))

	End Sub

#End Region

	Private Sub LoadMDUserData()
		Dim mdUserData = m_ListingDatabaseAccess.LoadUserAndMandantData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserFName, m_InitializationData.UserData.UserLName)
		If mdUserData Is Nothing Then
			m_Logger.LogError(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

			Return
		End If

		'pgiUsername.DataBindings.Add(New XRBinding("Text", m_InitializationData.UserData, "UserFullName"))
		lblUsername.Text = m_InitializationData.UserData.UserFullName

		Dim addressData = String.Format("{0}", mdUserData.USMDname)
		If Not String.IsNullOrWhiteSpace(mdUserData.USMDname2) Then addressData = String.Format("{1}{0}{2}", vbNewLine, addressData, mdUserData.USMDname2)
		If Not String.IsNullOrWhiteSpace(mdUserData.USMDname3) Then addressData = String.Format("{1}{0}{2}", vbNewLine, addressData, mdUserData.USMDname3)
		If Not String.IsNullOrWhiteSpace(mdUserData.USMDStrasse) Then addressData = String.Format("{1}{0}{2}", vbNewLine, addressData, mdUserData.USMDStrasse)
		addressData = String.Format("{1}{0}{2} {3}", vbNewLine, addressData, mdUserData.USMDPlz, mdUserData.USMDOrt)

		lblHeaderMDData.Text = addressData

		'DetailReport.Visible = False

	End Sub

	'Private Sub myItem_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
	'	'ps.ExecCommand(PrintingSystemCommand.ClosePreview)
	'	Dim manager As BarManager = TryCast(sender, BarManager)
	'	DetailReport.Visible = Not DetailReport.Visible

	'End Sub

End Class
