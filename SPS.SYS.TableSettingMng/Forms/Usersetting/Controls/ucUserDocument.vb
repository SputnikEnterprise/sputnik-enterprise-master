
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Imports DevExpress.LookAndFeel

Imports SP.DatabaseAccess

Imports SP.DatabaseAccess.TableSetting
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SP.DatabaseAccess.TableSetting.DataObjects.MandantData


Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages


Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraSplashScreen
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.Initialization
Imports System.ComponentModel
Imports DevExpress.XtraGrid.Views.Base
Imports System.Xml
Imports DevExpress.XtraGrid.Views.Grid



Public Class ucUserDocument


#Region "Private Fields"


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	Private m_TablesettingDatabaseAccess As ITablesDatabaseAccess
	''' <summary>
	''' The common data access object.
	''' </summary>
	Private m_MandantDatabaseAccess As MandantData

	Private m_SuppressUIEvents As Boolean

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private connectionString As String

	Private m_MandantXMLFile As String
	Private m_UserXMLFile As String
	Private m_MandantSetting As String
	Private m_UserDocSetting As String

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_ProgPath As ClsProgPath

	Private m_Year As Integer
	Private m_UserNumber As Integer
	Private m_UserMandantNumber As Integer


#End Region


#Region "private consts"

	Private Const MANDANT_XML_MAIN_KEY As String = "MD_{0}/"
	Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"
	Private Const MANDANT_XML_SETTING_SPUTNIK_DOCUMENT_SETTING As String = "UserProfile/User_{0}/Documents"

#End Region




#Region "Constructor"


	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_SuppressUIEvents = True
		InitializeComponent()
		m_SuppressUIEvents = False

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = ClsDataDetail.m_InitialData ' _setting

		m_mandant = New Mandant

		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_ProgPath = New ClsProgPath

		If m_InitializationData Is Nothing Then Exit Sub


		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, m_InitializationData.MDData.MDNr)
		m_UserDocSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_DOCUMENT_SETTING, m_UserNumber)
		IsDataValid = True

		Try
			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_Year)
			m_UserXMLFile = m_mandant.GetSelectedMDUserProfileXMLFilename(m_InitializationData.MDData.MDNr, m_UserNumber)

			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
				IsDataValid = False
			Else
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If

			connectionString = m_InitializationData.MDData.MDDbConn
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_MandantDatabaseAccess = m_TablesettingDatabaseAccess.LoadMandantData(m_InitializationData.MDData.MDNr, m_Year)

			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			IsDataValid = False

		End Try


		Reset()

		TranslateControls()

	End Sub



#End Region


	''' <summary>
	''' Inits the control with configuration information.
	''' </summary>
	'''<param name="initializationClass">The initialization class.</param>
	'''<param name="translationHelper">The translation helper.</param>
	Public Overridable Sub InitWithConfigurationData(ByVal initializationClass As InitializeClass, ByVal translationHelper As TranslateValuesHelper, _Year As Integer)

		m_InitializationData = initializationClass
		m_Translate = translationHelper
		m_Year = _Year
		IsDataValid = True

		Try
			connectionString = m_InitializationData.MDData.MDDbConn
			m_TablesettingDatabaseAccess = New TablesDatabaseAccess(connectionString, m_InitializationData.UserData.UserLanguage)
			m_MandantDatabaseAccess = m_TablesettingDatabaseAccess.LoadMandantData(MandantenNumber, m_Year)

			m_MandantSetting = String.Format(MANDANT_XML_MAIN_KEY, MandantenNumber)
			m_UserDocSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_DOCUMENT_SETTING, UserNumber)

			m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, m_Year)
			m_UserXMLFile = m_mandant.GetSelectedMDUserProfileXMLFilename(MandantenNumber, UserNumber)
			If Not System.IO.File.Exists(m_MandantXMLFile) Then
				m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))
				IsDataValid = False
			Else
				m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
			End If

		Catch ex As Exception
			IsDataValid = False

		End Try

		TranslateControls()

	End Sub



#Region "public property"

	Public Property IsDataValid As Boolean

	Public Property MandantenNumber As Integer

	Public Property UserNumber As Integer

	''' <summary>
	''' Gets the selected record.
	''' </summary>
	''' <returns>The selected user or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedExitingRecord As DocumentData
		Get
			Dim gvData = TryCast(grdDoc.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvData Is Nothing) Then

				Dim selectedRows = gvData.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvData.GetRow(selectedRows(0)), DocumentData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property

	Public Sub UpdateGridData()
		gvDoc.FocusedColumn = gvDoc.VisibleColumns(1)
		grdDoc.RefreshDataSource()
	End Sub


#End Region


	Private Sub Reset()

		ResetDocumentGrid()

	End Sub


	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)
		bbiCopyForAllUsers.Caption = m_Translate.GetSafeTranslationValue(bbiCopyForAllUsers.Caption)

	End Sub

	Private Sub ResetDocumentGrid()

		gvDoc.OptionsView.ShowIndicator = False
		gvDoc.OptionsView.ShowAutoFilterRow = True
		gvDoc.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		gvDoc.OptionsView.ShowFooter = False
		gvDoc.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False

		gvDoc.Columns.Clear()


		Dim columnrecid As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrecid.Caption = m_Translate.GetSafeTranslationValue("ID")
		columnrecid.OptionsColumn.AllowEdit = False
		columnrecid.Name = "recid"
		columnrecid.FieldName = "recid"
		columnrecid.Visible = False
		gvDoc.Columns.Add(columnrecid)

		Dim columnJobNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnJobNr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnJobNr.OptionsColumn.AllowEdit = False
		columnJobNr.Caption = m_Translate.GetSafeTranslationValue("Nummer")
		columnJobNr.Name = "JobNr"
		columnJobNr.FieldName = "JobNr"
		columnJobNr.Width = 60
		columnJobNr.Visible = True
		gvDoc.Columns.Add(columnJobNr)

		Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
		columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnBezeichnung.OptionsColumn.AllowEdit = False
		columnBezeichnung.Caption = m_Translate.GetSafeTranslationValue("Bezeichnung")
		columnBezeichnung.Name = "Bezeichnung"
		columnBezeichnung.FieldName = "Bezeichnung"
		columnBezeichnung.Visible = True
		columnBezeichnung.Width = 150
		gvDoc.Columns.Add(columnBezeichnung)

		Dim columnallowedToExport As New DevExpress.XtraGrid.Columns.GridColumn()
		columnallowedToExport.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnallowedToExport.OptionsColumn.AllowEdit = True
		columnallowedToExport.Caption = m_Translate.GetSafeTranslationValue("Exportieren?")
		columnallowedToExport.Name = "AllowedToExport"
		columnallowedToExport.FieldName = "AllowedToExport"
		columnallowedToExport.Visible = True
		columnallowedToExport.Width = 50
		gvDoc.Columns.Add(columnallowedToExport)

		Dim columnLogActivity As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLogActivity.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLogActivity.OptionsColumn.AllowEdit = True
		columnLogActivity.Caption = m_Translate.GetSafeTranslationValue("LOG?")
		columnLogActivity.Name = "LogActivity"
		columnLogActivity.FieldName = "LogActivity"
		columnLogActivity.Visible = True
		columnLogActivity.Width = 30
		gvDoc.Columns.Add(columnLogActivity)

		Dim columnChangedFrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columnChangedFrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnChangedFrom.OptionsColumn.AllowEdit = False
		columnChangedFrom.Caption = m_Translate.GetSafeTranslationValue("Geändert durch")
		columnChangedFrom.Name = "ChangedFrom"
		columnChangedFrom.FieldName = "ChangedFrom"
		columnChangedFrom.Visible = True
		columnChangedFrom.Width = 80
		gvDoc.Columns.Add(columnChangedFrom)

		Dim columnChangedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnChangedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnChangedOn.OptionsColumn.AllowEdit = False
		columnChangedOn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
		columnChangedOn.DisplayFormat.FormatString = "G"
		columnChangedOn.Caption = m_Translate.GetSafeTranslationValue("Geändert am")
		columnChangedOn.Name = "ChangedOn"
		columnChangedOn.FieldName = "ChangedOn"
		columnChangedOn.Visible = True
		columnChangedOn.Width = 60
		gvDoc.Columns.Add(columnChangedOn)


		grdDoc.DataSource = Nothing

	End Sub

	Private Function LoadUserDocumentList() As Boolean

		Dim listOfData = m_TablesettingDatabaseAccess.LoadDocumentData(MandantenNumber, UserNumber)

		If (listOfData Is Nothing) Then
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Dokument-Daten konnten nicht geladen werden."))
			Return False
		End If

		Dim gridData = (From person In listOfData
										Select New DocumentData With
			 {.recid = person.recid,
			 .UDR_ID = person.UDR_ID,
				.USNr = person.USNr,
				.MDNr = person.MDNr,
				.Bezeichnung = person.Bezeichnung,
				.DocName = person.DocName,
				.JobNr = person.JobNr,
				.ChangedOn = person.ChangedOn,
				.ChangedFrom = person.ChangedFrom,
				.AllowedToExport = person.AllowedToExport,
				.LogActivity = person.LogActivity
			 }).ToList()

		Dim listDataSource As BindingList(Of DocumentData) = New BindingList(Of DocumentData)

		For Each p In gridData
			If Not p.UDR_ID.HasValue Then
				p.AllowedToExport = LoadDocumentSetting(p.JobNr)
				p.MDNr = MandantenNumber
				p.USNr = UserNumber

				Dim success As Boolean = True
				success = m_TablesettingDatabaseAccess.AddAssignedUserDocumentRightsData(p)

			End If

			listDataSource.Add(p)
		Next

		grdDoc.DataSource = listDataSource
		bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), listDataSource.Count)

		Return Not listOfData Is Nothing

	End Function


	Private Sub FocusUserData(ByVal recordID As Integer)

		If Not grdDoc.DataSource Is Nothing Then

			Dim esSalaryData = CType(gvDoc.DataSource, BindingList(Of DocumentData))

			Dim index = esSalaryData.ToList().FindIndex(Function(data) data.recid >= recordID)

			Dim rowHandle = gvDoc.GetRowHandle(index)
			gvDoc.FocusedRowHandle = rowHandle

		End If

	End Sub

	Private Sub OngvDoc_RowCellStyle(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs) Handles gvDoc.RowCellStyle

		If (e.RowHandle >= 0) Then
			Dim view As GridView = CType(sender, GridView)
			Dim data = CType(view.GetRow(e.RowHandle), DocumentData)
			If Not data Is Nothing Then
				If data.DocName.ToLower.EndsWith(".crd") Then e.Appearance.BackColor = Color.Yellow
			End If
		End If

	End Sub

	Private Sub OngvDoc_RowUpdated(sender As Object, e As RowObjectEventArgs) Handles gvDoc.RowUpdated

		grdDoc.FocusedView.CloseEditor()
		Dim success = UpdateRecord(e.Row)

		If success Then
			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
		Else
			bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden.")
		End If

	End Sub

	Private Sub OnbbiCopyForAllUsers_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiCopyForAllUsers.ItemClick
		Dim success = CopyUserDocRightsforAllUser()

		If success Then
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert."))
		Else
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Daten konnten nicht gespeichert werden."))
		End If

	End Sub

	Private Function CopyUserDocRightsforAllUser() As Boolean
		Dim success As Boolean = True

		Try

			Dim msg = m_Translate.GetSafeTranslationValue("Möchten Sie wirklich die Documenten-Rechte für alle anderen Benutzer ersetzen?")
			If Not (m_UtilityUI.ShowYesNoDialog(msg, m_Translate.GetSafeTranslationValue("Documentenrechte ersetzen?"))) Then
				Return False
			End If
			gvDoc.PostEditor()
			gvDoc.UpdateCurrentRow()

			grdDoc.RefreshDataSource()
			If grdDoc.DataSource Is Nothing Then Return False
			Dim docDataSource As BindingList(Of DocumentData) = New BindingList(Of DocumentData)
			docDataSource = grdDoc.DataSource

			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
			SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			For Each u In UserDataList

				For Each p In docDataSource
					p.USNr = u.USNr
					p.ChangedFrom = m_InitializationData.UserData.UserFullName

					SplashScreenManager.Default.SetWaitFormCaption(String.Format(m_Translate.GetSafeTranslationValue("Ausgeführt wird für: {0} {1}"), u.Vorname, u.Nachname))
					success = success AndAlso m_TablesettingDatabaseAccess.UpdateAssignedUserDocumentRightsForAllUsersData(p)
				Next

			Next


		Catch ex As Exception
			success = False
			m_Logger.LogError(ex.ToString)

		Finally
			SplashScreenManager.CloseForm(False)

		End Try

		success = success AndAlso LoadUserDocumentList()


		Return success

	End Function


	Private Function UpdateRecord(ByVal rowobject As Object) As Boolean
		Dim success As Boolean = True

		Dim SelectedData = CType(rowobject, DocumentData)
		If SelectedData.recid = 0 Then

		Else
			SelectedData.ChangedFrom = m_InitializationData.UserData.UserFullName
			success = UpdateAssignedUserDocumentData(SelectedData)
			success = success AndAlso m_TablesettingDatabaseAccess.UpdateAssignedUserDocumentRightsData(SelectedData)

		End If
		success = success AndAlso LoadUserDocumentList()


		Return success

	End Function


	Private Function LoadDocumentSetting(ByVal jobNr As String) As Boolean
		Dim success As Boolean = True

		Try
			Dim strQuery As String = String.Format("//DocName[@ID={0}{1}{0}]/Export", Chr(34), jobNr)
			Dim strBez As String = m_ProgPath.GetXMLNodeValue(m_UserXMLFile, strQuery)

			success = If(strBez = "1", True, False)

		Catch ex As Exception

		End Try

		Return success

	End Function

	Private Function UpdateAssignedUserDocumentData(ByVal data As DocumentData) As Boolean
		Dim success As Boolean = True

		Dim strQuery As String = String.Format("//DocName[@ID={0}{1}{0}]/Export", Chr(34), data.JobNr)
		Dim strBez As String = m_ProgPath.GetXMLNodeValue(m_UserXMLFile, strQuery)

		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing

		Try

			xDoc.Load(m_UserXMLFile)

			' Metro Design anpassen...
			xNode = xDoc.SelectSingleNode(String.Format("//DocName[@ID='{0}']", data.JobNr))
			If xNode Is Nothing Then
				Dim newNode As Xml.XmlElement = xDoc.CreateElement("DocName")

				newNode.SetAttribute("ID", data.JobNr)
				xDoc.DocumentElement.AppendChild(newNode)
				xNode = xDoc.SelectSingleNode(String.Format("//DocName[@ID='{0}']", data.JobNr))
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If
				' Kachelgrösse
				If xElmntFamily.SelectSingleNode("Export") IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("Export"))
				InsertTextNode(xDoc, xElmntFamily, "Export", If(data.AllowedToExport, "1", "0"))

			End If
			xDoc.Save(m_UserXMLFile)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			success = False

		End Try

		Return success

	End Function


#Region "public metheods"

	Public Property UserDataList As IEnumerable(Of UserData)

	Public Function LoadDocData() As Boolean
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Dokumentenliste wird zusammengestellt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Dim success As Boolean = True

		success = success AndAlso LoadUserDocumentList()
		SplashScreenManager.CloseForm(False)

		Return success

	End Function


#End Region



#Region "Helpers"

	Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode, _
																ByVal strTag As String, ByVal strText As String) As XmlElement
		Dim xNodeTemp As XmlNode

		xNodeTemp = xDoc.CreateElement(strTag)
		xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
		xNode.AppendChild(xNodeTemp)

		Return CType(xNodeTemp, XmlElement)
	End Function


#End Region

End Class
