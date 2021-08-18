
Imports TrxmlUtility.Xsd
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Common.DataObjects
Imports SP.DatabaseAccess.Applicant
Imports SP.DatabaseAccess.CVLizer.DataObjects


Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors

Imports DevExpress.XtraBars
Imports System.ComponentModel
Imports System.Reflection

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI

Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports DevExpress.Pdf
Imports SPProgUtility.CommonXmlUtility

Imports System.Text
Imports DevExpress.Skins
Imports DevExpress.UserSkins
Imports SPProgUtility
Imports System.Security.Cryptography
Imports System.IO
Imports SP.Infrastructure
Imports System.Collections.Specialized
Imports System.Net
Imports TrxmlUtility
Imports SP.DatabaseAccess.CVLizer
Imports SP.Infrastructure.Initialization
Imports SP.ApplicationMng.GraphicsEditor
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Layout
Imports DevExpress.XtraLayout.Utils
Imports SP.ApplicationMng.SPApplicationWebService
Imports SP.Main.Notify.SPApplicationWebService

Namespace CVLizer.UI

	Public Class ucCVLDocument

#Region "Private Consts"

		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "http://asmx.domain.com/wsSPS_services/SPApplication.asmx"

#End Region

#Region "private fields"


		Private m_applicantDb As String
		Private m_customerID As String

		Private m_CVLizerXMLData As CVLizerXMLData
		Private m_CurrentFileExtension As String


		Private m_DocumentData As IEnumerable(Of DocumentLocalViewData)
		Private m_CVLProfileID As Integer

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		Private m_mandant As Mandant
		Private m_path As SPProgUtility.ProgPath.ClsProgPath
		Private m_SettingFile As ProgramSettings


#End Region


#Region "constructor"

		Public Sub New() ' ByVal _setting As InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			'm_InitializationData = _setting
			m_mandant = New Mandant
			m_UtilityUI = New UtilityUI
			m_Utility = New SP.Infrastructure.Utility
			m_path = New SPProgUtility.ProgPath.ClsProgPath
			'm_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)


			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			'm_SettingFile = ProgSettingData
			'Dim domainName = m_SettingFile.WebserviceDomain
			'm_ApplicationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)

			Reset()

#If DEBUG Then
			'm_customerID = "EFFE4D79-2AC6-4f9f-839F-1E4E9D6D9E2A"
#End If


		End Sub


#End Region


#Region "private property"

		''' <summary>
		''' Gets the selected work phase data.
		''' </summary>
		Private ReadOnly Property SelectedDocumentViewData As DocumentLocalViewData
			Get
				Dim grdView = TryCast(grdDocument.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (grdView Is Nothing) Then

					Dim selectedRows = grdView.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim viewData = CType(grdView.GetRow(selectedRows(0)), DocumentLocalViewData)
						Return viewData
					End If

				End If

				Return Nothing
			End Get

		End Property


#End Region


#Region "public Methodes"

		Public Function LoadAssignedDocumentData(ByVal cvlProfileID As Integer?) As Boolean
			Dim result As Boolean = True

			m_CVLProfileID = cvlProfileID

			result = result AndAlso PerformDocumentWebservice(m_CVLProfileID)

			Return True

		End Function


#End Region


		Private Sub Reset()

			ResetDocumentFields()

		End Sub

		Private Sub ResetDocumentFields()

			gvDocument.OptionsView.ShowIndicator = False
			gvDocument.OptionsView.ShowAutoFilterRow = True
			gvDocument.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
			gvDocument.OptionsView.ShowFooter = False
			gvDocument.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

			gvDocument.Columns.Clear()


			Dim columnID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnID.OptionsColumn.AllowEdit = False
			columnID.Caption = ("ID")
			columnID.Name = "ID"
			columnID.FieldName = "ID"
			columnID.Visible = False
			columnID.Width = 50
			gvDocument.Columns.Add(columnID)

			Dim columnCustomer_ID As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCustomer_ID.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCustomer_ID.OptionsColumn.AllowEdit = False
			columnCustomer_ID.Caption = ("DocClass")
			columnCustomer_ID.Name = "DocClass"
			columnCustomer_ID.FieldName = "DocClass"
			columnCustomer_ID.Width = 10
			columnCustomer_ID.Visible = True
			gvDocument.Columns.Add(columnCustomer_ID)

			Dim columnIsCedLable As New DevExpress.XtraGrid.Columns.GridColumn()
			columnIsCedLable.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnIsCedLable.OptionsColumn.AllowEdit = False
			columnIsCedLable.Caption = ("Pages")
			columnIsCedLable.Name = "Pages"
			columnIsCedLable.FieldName = "Pages"
			columnIsCedLable.Width = 10
			columnIsCedLable.Visible = True
			gvDocument.Columns.Add(columnIsCedLable)

			Dim columnNationalityLable As New DevExpress.XtraGrid.Columns.GridColumn()
			columnNationalityLable.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnNationalityLable.OptionsColumn.AllowEdit = False
			columnNationalityLable.Caption = ("Plaintext")
			columnNationalityLable.Name = "Plaintext"
			columnNationalityLable.FieldName = "Plaintext"
			columnNationalityLable.Width = 10
			columnNationalityLable.Visible = True
			gvDocument.Columns.Add(columnNationalityLable)

			Dim columnCivilStatusLable As New DevExpress.XtraGrid.Columns.GridColumn()
			columnCivilStatusLable.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnCivilStatusLable.OptionsColumn.AllowEdit = False
			columnCivilStatusLable.Caption = ("FileType")
			columnCivilStatusLable.Name = "FileType"
			columnCivilStatusLable.FieldName = "FileType"
			columnCivilStatusLable.Width = 10
			columnCivilStatusLable.Visible = True
			gvDocument.Columns.Add(columnCivilStatusLable)

			Dim columnAddressLable As New DevExpress.XtraGrid.Columns.GridColumn()
			columnAddressLable.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnAddressLable.OptionsColumn.AllowEdit = False
			columnAddressLable.Caption = ("DocID")
			columnAddressLable.Name = "DocID"
			columnAddressLable.FieldName = "DocID"
			columnAddressLable.Width = 10
			columnAddressLable.Visible = True
			gvDocument.Columns.Add(columnAddressLable)

			Dim columnDocSize As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDocSize.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDocSize.OptionsColumn.AllowEdit = False
			columnDocSize.Caption = ("DocSize")
			columnDocSize.Name = "DocSize"
			columnDocSize.FieldName = "DocSize"
			columnDocSize.Width = 10
			columnDocSize.Visible = True
			gvDocument.Columns.Add(columnDocSize)

			Dim columnDocLanguage As New DevExpress.XtraGrid.Columns.GridColumn()
			columnDocLanguage.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnDocLanguage.OptionsColumn.AllowEdit = False
			columnDocLanguage.Caption = ("DocLanguage")
			columnDocLanguage.Name = "DocLanguage"
			columnDocLanguage.FieldName = "DocLanguage"
			columnDocLanguage.Width = 10
			columnDocLanguage.Visible = True
			gvDocument.Columns.Add(columnDocLanguage)


			grdDocument.DataSource = Nothing

		End Sub


		''' <summary>
		'''  Performs loading cvl document data.
		''' </summary>
		Private Function PerformDocumentWebservice(ByVal profileID As Integer?) As Boolean

#If DEBUG Then
			'm_ApplicationUtilWebServiceUri = "http://localhost:44721/SPApplication.asmx"
#End If

			Dim webservice As New Main.Notify.SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			'm_Logger.LogDebug(String.Format("Customer_ID: {0} contacting...", m_customerID))
			Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

			' Read data over webservice
			' TODO: Dim searchResult = webservice.LoadAssignedCVLDocumenViewtData(m_customerID, profileID)
			Dim searchResult = webservice.LoadAssignedCVLDocumenViewtData(m_customerID, profileID) ' LoadAssignedCVLDocumentData(m_customerID, profileID)

			If searchResult Is Nothing Then
				m_Logger.LogError(String.Format("Documents could not be loaded from webservice! {0} | {1}", m_customerID, profileID))

				Return False
			End If

			Dim gridData = (From person In searchResult
											Select New DocumentLocalViewData With {.ID = person.ID,
														.DocClass = person.DocClass,
														.Pages = person.Pages,
														.Plaintext = person.Plaintext,
														.FileType = person.FileType,
														.DocID = person.DocID,
														.DocSize = person.DocSize,
														.DocLanguage = person.DocLanguage,
														.FileHashvalue = person.FileHashvalue,
														.DocXML = person.DocXML
														}).ToList()

			Dim listDataSource As BindingList(Of DocumentLocalViewData) = New BindingList(Of DocumentLocalViewData)
			For Each p In gridData
				listDataSource.Add(p)
			Next

			m_DocumentData = listDataSource
			grdDocument.DataSource = m_DocumentData


			Return Not m_DocumentData Is Nothing

		End Function

		''' <summary>
		'''  Performs loading cvl document data.
		''' </summary>
		Private Function PerformAssignedDocumentWebservice(ByVal recID As Integer?) As DocumentLocalViewData

#If DEBUG Then
			'm_ApplicationUtilWebServiceUri = "http://localhost:44721/SPApplication.asmx"
#End If

			Dim webservice As New Main.Notify.SPApplicationWebService.SPApplicationSoapClient
			webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_ApplicationUtilWebServiceUri)

			'm_Logger.LogDebug(String.Format("Customer_ID: {0} contacting...", m_customerID))
			Trace.WriteLine(String.Format("Customer_ID: {0} contacting...", m_customerID))

			' Read data over webservice
			' TODO: Dim searchResult = webservice.LoadAssignedDocumentViewData(m_customerID, recID)
			Dim searchResult = webservice.LoadAssignedDocumentData(m_customerID, recID)

			Dim data = New DocumentLocalViewData With {.ID = searchResult.ID,
														.DocClass = searchResult.DocClass,
														.Pages = searchResult.Pages,
														.Plaintext = searchResult.Plaintext,
														.FileType = searchResult.FileType,
														.DocID = searchResult.DocID,
														.DocSize = searchResult.DocSize,
														.DocLanguage = searchResult.DocLanguage,
														.FileHashvalue = searchResult.FileHashvalue,
														.DocXML = searchResult.DocXML,
														.DocBinary = searchResult.DocBinary,
														.ExtensionData = searchResult.ExtensionData
			}


			Return data
		End Function

		Sub OngvDocument_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvDocument.RowCellClick
			Dim success As Boolean = True

			Dim data = SelectedDocumentViewData
			If data Is Nothing Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(("Dokument Daten konnte nicht geladen werden."))
				Return
			End If

			If e.Clicks = 2 Then
				Dim docData = PerformAssignedDocumentWebservice(data.ID)
				If docData Is Nothing Then
					SplashScreenManager.CloseForm(False)
					m_UtilityUI.ShowErrorDialog(("Selektiertes Dokument konnte nicht geladen werden."))
					Return
				End If

				success = success AndAlso DisplayAssignedEducationPhaseData(docData)
			End If


		End Sub

		Private Function DisplayAssignedEducationPhaseData(ByVal data As DocumentLocalViewData) As Boolean
			Dim success As Boolean = True

			Dim tmpFilename = Path.GetTempFileName()
			tmpFilename = Path.ChangeExtension(tmpFilename, data.FileType)
			success = success AndAlso m_Utility.WriteFileBytes(tmpFilename, data.DocBinary)

			If success Then
				m_Utility.OpenFileWithDefaultProgram(tmpFilename)
			End If

			Return success

		End Function


#Region "Helpers Class"

		Private Class DocumentLocalViewData
			Inherits Main.Notify.SPApplicationWebService.DocumentViewDataDTO


		End Class

#End Region


	End Class

End Namespace
