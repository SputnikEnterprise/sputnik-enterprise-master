
Imports SPProgUtility.Mandanten

Imports System.IO.File
Imports System.Threading

Public Class ClsMain_Net

  Private Property PrintJobNr As String
  Private Property SQL4Print As String
  Private Property bPrintAsDesign As Boolean
  Private m_xml As New ClsXML


#Region "Constructor"

	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub


	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Application.EnableVisualStyles()

	End Sub


	'Public Sub New()
	'	Dim m_md As New SPProgUtility.Mandanten.Mandant

	'	Dim _setting = CreateInitialData(m_md.GetDefaultMDNr, m_md.GetDefaultUSNr)
	'	ClsDataDetail.m_InitialData = _setting
	'	ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

	'	Application.EnableVisualStyles()

	'End Sub

#End Region


#Region "Startfunktionen..."

	Sub ShowfrmProposal(ByVal iPNr As Integer?)

		ClsDataDetail.GetProposalNr = iPNr

		Dim proposeSetting = New ClsProposeSetting With {.SelectedProposeNr = iPNr}
		Dim frmTest = New frmPropose(ClsDataDetail.m_InitialData, proposeSetting)

		frmTest.Show()
		frmTest.BringToFront()

	End Sub

	Sub ShowfrmProposal(ByVal iMANr As Integer?, ByVal iKDNr As Integer?, ByVal iVakNr As Integer?)

		ClsDataDetail.GetProposalNr = 0
		Dim frmTest As frmPropose
		frmTest = New frmPropose(ClsDataDetail.m_InitialData, New ClsProposeSetting With {.SelectedProposeNr = 0,
																												 .SelectedMANr = iMANr,
																												 .SelectedKDNr = iKDNr,
																												 .SelectedVakNr = iVakNr})
		frmTest.Show()
		frmTest.BringToFront()

	End Sub

	Sub ShowfrmProposal(ByVal iMANr As Integer?, ByVal iKDNr As Integer?, ByVal iKDZHDNr As Integer?, ByVal iVakNr As Integer?)

		ClsDataDetail.GetProposalNr = 0
		Dim frmTest As frmPropose
		frmTest = New frmPropose(ClsDataDetail.m_InitialData, New ClsProposeSetting With {.SelectedProposeNr = 0,
																												 .SelectedMANr = iMANr,
																												 .SelectedKDNr = iKDNr,
																												 .SelectedZHDNr = iKDZHDNr,
																												 .SelectedVakNr = iVakNr})
		frmTest.Show()
		frmTest.BringToFront()

	End Sub

	Sub PrintProposeStammBlatt(ByVal _iPNr As Integer?, ByVal _bAsDesign As Boolean?, ByVal _strJobNr As String)
		Dim _ClsDb As New ClsDbFunc

		Me.SQL4Print = _ClsDb.GetSQLString4Print(0)
		Me.PrintJobNr = _strJobNr
		Me.bPrintAsDesign = _bAsDesign
		ClsDataDetail.GetProposalNr = _iPNr

		_StartPrinting()

	End Sub

	Private Sub _StartPrinting()
		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLProposeSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn,
																																											 .SQL2Open = Me.SQL4Print, .JobNr2Print = Me.PrintJobNr,
																																											 .SelectedMDNr = ClsDataDetail.m_InitialData.MDData.MDNr,
																																											 .LogedUSNr = ClsDataDetail.m_InitialData.UserData.UserNr}
		Dim obj As New SPS.Listing.Print.Utility.ProposeSearchListing.ClsPrintProposeSearchList(_Setting)

		obj.PrintProposeTpl_1(Me.bPrintAsDesign, String.Empty, ClsDataDetail.GetProposalNr, False, False)

	End Sub

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


#End Region

End Class
