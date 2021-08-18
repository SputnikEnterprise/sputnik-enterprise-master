
Option Strict Off

Imports System.Reflection.Assembly

Imports System.ComponentModel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Repository

Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Common.DataObjects

Imports SP.Infrastructure.Settings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Imports System.IO

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.Mandanten
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions

Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing
Imports DevExpress.Pdf
Imports System.Drawing.Printing
Imports SPProgUtility.ProgPath
Imports SPProgUtility.SPTranslation
Imports SP.Internal.Automations
Imports SP.DatabaseAccess.Listing.DataObjects

Public Class frmLoNLASearch
	Inherits DevExpress.XtraEditors.XtraForm


#Region "Private Fields"

	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	''' <summary>
	''' The settings manager.
	''' </summary>
	Private m_SettingsManager As ISettingsManager

	Private m_connectionString As String

	Private m_mandant As Mandant

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	'	Private _ClsFunc As New ClsDivFunc
	'	Private _ClsDb As ClsDbFunc

	'Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private strLastSortBez As String
	Private Stopwatch As Stopwatch = New Stopwatch()

	' Während des Abbrechen-Dialogfensters kann der Prozess weitergehen und ein Abbruch-Stop signalisieren.
	' So lange darf der Abbrechen-Button auch nicht re-aktiviert werden bzw. ein Abbruch auslösen.
	Private _abortDeniedPending As Boolean

	'Public Shared frmMyLV As frmLoNLASearch_LV
	Public Const frmMyLVName As String = "frmLoNLASearch_LV"

	Private ShowListViewFaild As Boolean = False

	Private _pnlLohnausweisTop As Integer ' Die Position muss vorgemerkt werden. Fixe Ausgangslage fürs Expandieren.
	Private _pnlLohnausweisHeight As Integer ' Die Höhe muss vorgemerkt werden. Fixe Ausgangslage fürs Expandieren.

	Private Property m_ISAllRowsSelected As Boolean
	Private Property SelectedYear2Print As Integer
	Private PrintListingThread As Thread
	Private Property SQL4Print As String
	Private Property Conn2Open As New SqlClient.SqlConnection
	Private Property SendPrintJob2WOS As Short
	Private Property CreatedJobForExport As Boolean?

	Private pcc As New DevExpress.XtraBars.PopupControlContainer

	Private m_GridSettingPath As String
	Private m_GVESSettingfilename As String

	Private m_xmlSettingRestoreSearchSetting As String
	Private m_xmlSettingSearchFilter As String

	Private m_SearchCriteria As New SearchCriteria
	Private m_NLASetting As String

	Private m_BaseTableData As BaseTable.SPSBaseTables
	Private m_PermissionData As BindingList(Of SP.Internal.Automations.PermissionData)
	Private m_CountryData As BindingList(Of SP.Internal.Automations.CVLBaseTableViewData)

#End Region


#Region "private consts"

	Private Const MODUL_NAME_SETTING = "nlasearch"
	Private Const USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_RESTORE As String = "gridsetting/User_{0}/{1}/restorelayoutfromxml"
	Private Const USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_FILTER As String = "gridsetting/User_{0}/{1}/keepfilter"
	Private Const FORM_XML_NLAFIEKDS_KEY As String = "Forms_Normaly/Lohnausweis_NLA"

#End Region


#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()
		m_ISAllRowsSelected = False

		m_InitializationData = _setting

		m_mandant = New Mandant
		m_Utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_BaseTableData = New BaseTable.SPSBaseTables(m_InitializationData)

		Try
			InitializeComponent()

		Catch ex As Exception

		End Try


		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_connectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_connectionString, m_InitializationData.UserData.UserLanguage)

		m_PermissionData = m_BaseTableData.PerformPermissionDataOverWebService(m_InitializationData.UserData.UserLanguage)
		m_BaseTableData.BaseTableName = "Country"
		m_CountryData = m_BaseTableData.PerformCVLBaseTablelistWebserviceCall()

		Try
			m_GridSettingPath = String.Format("{0}{1}\", m_mandant.GetGridSettingPath(m_InitializationData.MDData.MDNr), MODUL_NAME_SETTING)
			If Not Directory.Exists(m_GridSettingPath) Then Directory.CreateDirectory(m_GridSettingPath)

			m_GVESSettingfilename = String.Format("{0}{1}{2}.xml", m_GridSettingPath, Me.grdRP.Name, m_InitializationData.UserData.UserNr)

			m_xmlSettingRestoreSearchSetting = String.Format(USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_RESTORE, m_InitializationData.UserData.UserNr, MODUL_NAME_SETTING)
			m_xmlSettingSearchFilter = String.Format(USER_XML_SETTING_SPUTNIK_DELETED_SEARCH_LIST_GRIDSETTING_FILTER, m_InitializationData.UserData.UserNr, MODUL_NAME_SETTING)

		Catch ex As Exception

		End Try

		TranslateControls()

		Reset()
		LoadMandantenDropDown()
		LoadPermissionCodeDropDown()

		LoadSortDropDown()


		AddHandler lueCountry.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler luePermissionCode.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler luePermissionCode.QueryPopUp, AddressOf checkedComboBoxEdit1_QueryPopUp

		AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler gvRP.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

		AddHandler gvRP.FocusedRowChanged, AddressOf OngvRP_FocusedRowChanged

	End Sub


#End Region


#Region "Private Properties"

	Private ReadOnly Property SelectedRowViewData As FoundedData
		Get
			Dim grdView = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (grdView Is Nothing) Then

				Dim selectedRows = grdView.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim contact = CType(grdView.GetRow(selectedRows(0)), FoundedData)
					Return contact
				End If

			End If

			Return Nothing
		End Get

	End Property

	''' <summary>
	''' Gets the open nla pdf file.
	''' </summary>
	Private ReadOnly Property OpenNLAPDFFileForPrint As Boolean?
		Get
			Dim m_path As New ClsProgPath

			Dim value As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr),
																									String.Format("{0}/opennlapdffile", FORM_XML_NLAFIEKDS_KEY)), False)


			Return value
		End Get
	End Property

#End Region

#Region "Lookup Edit Reset und Load..."

	Private Sub Reset()

		ResetMandantenDropDown()

		ResetCountryDropDown()
		ResetPermissionCodeDropDown()

		ResetGridNLAData()
		ResetSortDropDown()

	End Sub

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.DisplayMember = "MandantName1"
		lueMandant.Properties.ValueMember = "MandantNumber"

		lueMandant.Properties.Columns.Clear()
		lueMandant.Properties.Columns.Add(New LookUpColumnInfo With {.FieldName = "MandantName1",
																					 .Width = 100,
																					 .Caption = m_Translate.GetSafeTranslationValue("Mandant")})

		lueMandant.Properties.ShowHeader = False
		lueMandant.Properties.ShowFooter = False

		lueMandant.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueMandant.Properties.SearchMode = SearchMode.AutoComplete
		lueMandant.Properties.AutoSearchColumnIndex = 0

		lueMandant.Properties.NullText = String.Empty
		lueMandant.EditValue = Nothing

	End Sub

	Private Sub ResetCountryDropDown()

		lueCountry.Properties.Items.Clear()

		lueCountry.Properties.DisplayMember = "CountryDataViewData"
		lueCountry.Properties.ValueMember = "Code"

		lueCountry.Properties.DropDownRows = 10
		lueCountry.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		lueCountry.Properties.NullText = String.Empty
		lueCountry.EditValue = Nothing

	End Sub


	''' <summary>
	''' Resets the permission state drop down.
	''' </summary>
	Private Sub ResetPermissionCodeDropDown()

		luePermissionCode.Properties.Items.Clear()

		luePermissionCode.Properties.DisplayMember = "PermissionCodeViewData"
		luePermissionCode.Properties.ValueMember = "PermissionCode"

		luePermissionCode.Properties.DropDownRows = 20
		luePermissionCode.Properties.ForceUpdateEditValue = DevExpress.Utils.DefaultBoolean.True

		luePermissionCode.Properties.NullText = String.Empty
		luePermissionCode.EditValue = Nothing

	End Sub


	' Mandantendaten...
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim SelectedData As SP.DatabaseAccess.Common.DataObjects.MandantData = TryCast(Me.lueMandant.GetSelectedDataRow(), SP.DatabaseAccess.Common.DataObjects.MandantData)
		ResetGridNLAData()

		If Not SelectedData Is Nothing Then
			Dim ChangeMandantData = ClsDataDetail.ChangeMandantData(CInt(lueMandant.EditValue), ClsDataDetail.m_InitialData.UserData.UserNr)

			m_InitializationData = ClsDataDetail.m_InitialData

		Else
			' do nothing
		End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)
		Me.bbiSetting.Enabled = Not (ClsDataDetail.m_InitialData.MDData Is Nothing)

	End Sub

	Private Sub ResetSortDropDown()

		lueSort.Properties.DisplayMember = "DisplayText"
		lueSort.Properties.ValueMember = "Value"

		Dim columns = lueSort.Properties.Columns
		columns.Clear()

		columns.Add(New LookUpColumnInfo("DisplayText", 0))

		lueSort.Properties.ShowHeader = False
		lueSort.Properties.ShowFooter = False
		lueSort.Properties.DropDownRows = 10
		lueSort.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueSort.Properties.SearchMode = SearchMode.AutoComplete
		lueSort.Properties.AutoSearchColumnIndex = 0
		lueSort.Properties.NullText = String.Empty

		lueSort.EditValue = Nothing

	End Sub

	Private Function LoadSortDropDown() As Boolean

		Dim dayMonthStdData = New List(Of SortViewData) From {
			New SortViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Kandidatenname"), .Value = 0},
			New SortViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Kandidatenname, Land"), .Value = 1},
			New SortViewData With {.DisplayText = m_Translate.GetSafeTranslationValue("Land, Kandidatenname"), .Value = 2}
		}

		lueSort.Properties.DataSource = dayMonthStdData

		Return True

	End Function


#End Region


	Private ReadOnly Property GetJobID() As Short
		Get
			Dim xmlFile = m_mandant.GetSelectedMDFormDataXMLFilename(m_InitializationData.MDData.MDNr)
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim sValue As String = String.Empty
			Dim strMainKey As String = "//Lohnausweis_NLA"
			Dim strKeyName As String = "NLA_2_3".ToLower
			Dim strQuery As String = String.Format("{0}/{1}", strMainKey, strKeyName)

			strKeyName = "Orientation".ToLower
			strQuery = String.Format("{0}/{1}", strMainKey, strKeyName)
			sValue = _ClsProgSetting.GetXMLValueByQuery(xmlFile, strQuery, "2")

			Return CShort(Val(sValue))
		End Get
	End Property


#Region "Delegate Variabeln-Deklaration"
	' Der Thread im Hintergrund
	Private _ThreadMain As Threading.Thread

	' Das Object (z.B. diese Maske) muss benachrigt werden
	Private _SynchronizingObject As System.ComponentModel.ISynchronizeInvoke

	' Das andere Prozess muss benachrichtigt werden, wenn die Suche abgebrochen werden soll.
	Private _SynchronizingLV As System.ComponentModel.ISynchronizeInvoke

	' Darin wird die Methode gespeichert, die vom Thread via Delegate aufgerufen werden soll.
	Private _NotifyMainProgressDelegate As NotifyMainProgressDel

	' Diese Maske stellt ein Delegate für Benarichtigung des Fortschritts zur Verfügung
	Public Delegate Sub NotifyMainProgressDel(ByVal Message As String, ByVal PercentComplete As Integer)

	' Diese Maske muss wissen, wann der nächste Prozess gestartet werden soll
	Private _NotifyMainStartLVDelegate As NotifyMainStartLVDel
	Public Delegate Sub NotifyMainStartLVDel(ByVal Message As String)

	' Entsprechend muss das Hauptfenster wissen, wenn der nächste Prozess abgeschlossen ist
	Private _NotifyMainLVCompletedDelegate As NotifyMainLVCompletedDel
	Public Delegate Sub NotifyMainLVCompletedDel(ByVal Message As String)

	' Das Resultat-Fenster muss benachrichtigt werden, wenn das Hauptfenster sich bewegt hat.
	'Private _NotifyLVMoveDelegate As frmLoNLASearch_LV.NotifyLVMoveDel
#End Region

#Region "Delegate Methoden-Aufrufe"
	''' <summary>
	''' Diese Methode darf vom Thread aufgerufen werden, wenn die Statusbar verändert werden soll.
	''' Sie gibt der Maske sozusagen bescheid. Der Thread hat auf der Maske keine Rechnte mehr.
	''' </summary>
	''' <param name="Message"></param>
	''' <param name="Value"></param>
	''' <remarks></remarks>
	Public Sub NotifyMainProgressBar(ByVal Message As String, ByVal Value As Integer)
		Try
			If Not _NotifyMainProgressDelegate Is Nothing Then
				Dim args(1) As Object
				args(0) = Message
				args(1) = Value
				_SynchronizingObject.Invoke(_NotifyMainProgressDelegate, args)
			End If
		Catch ex As Threading.ThreadAbortException
			' Der Thread wurde abgebrochen --> keine Fehlermeldung
		Catch ex As ObjectDisposedException
			' Das Objekt wurde zerstört --> keine Fehlermeldung
		Catch ex As InvalidOperationException
			' Das Delegate wurde nicht erstellt --> keine Fehlermeldung
		Catch ex As Exception
			MessageBoxShowError("NotifyMainProgressBar", ex)
		End Try

	End Sub

	''' <summary>
	''' Diese Methode wird ausgeführt, wenn der Thread darum bittet.
	''' Das wurde dem Delegate mitgeteilt, welche Methode der Thread ankicken darf.
	''' </summary>
	''' <param name="Message"></param>
	''' <param name="PercentComplete"></param>
	''' <remarks></remarks>
	Private Sub DelegateProgress(ByVal Message As String, ByVal PercentComplete As Integer)
		Try
			bsiInfo.Caption = Message
		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)
		End Try

	End Sub

	''' <summary>
	''' Diese Methode darf vom Thread aufgerufen werden, wenn die ListView gefüllt werden soll.
	''' </summary>
	''' <param name="Message"></param>
	''' <remarks></remarks>
	Public Sub NotifyStartLV(ByVal Message As String)
		Try
			If Not _NotifyMainStartLVDelegate Is Nothing Then
				Dim args(0) As Object
				args(0) = Message
				_SynchronizingObject.Invoke(_NotifyMainStartLVDelegate, args)
			End If
		Catch ex As ObjectDisposedException
			' Das Objekt wurde zerstört --> keine Fehlermeldung
		Catch ex As InvalidOperationException
			' Das Delegate wurde nicht erstellt --> keine Fehlermeldung
		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)
		End Try
	End Sub

	''' <summary>
	''' Wenn die LV alle Daten übernommen hat, muss das Hauptfenster informiert werden.
	''' </summary>
	''' <param name="Message"></param>
	''' <remarks></remarks>
	Private Sub NotifyMainLVCompleted(ByVal Message As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			If Not _NotifyMainLVCompletedDelegate Is Nothing Then
				Dim args(0) As Object
				args(0) = Message
				_SynchronizingObject.Invoke(_NotifyMainLVCompletedDelegate, args)
			End If
		Catch ex As ObjectDisposedException
			' Das Objekt wurde zerstört --> keine Fehlermeldung
		Catch ex As InvalidOperationException
			' Das Delegate wurde nicht erstellt --> keine Fehlermeldung
		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)
		End Try

	End Sub


	''' <summary>
	''' Wenn der separate Thread im LV die Daten übertragen hat, so muss
	''' es das Hauptfenster wissen, damit die Suche freigegeben wird und
	''' die Anzahl gefundene Datensätze angezeigt wird, usw.
	''' </summary>
	''' <param name="Message"></param>
	''' <remarks></remarks>
	Public Sub DelegateLVCompleted(ByVal Message As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze gefunden"), Me.gvRP.RowCount)
			bbiSearch.Caption = m_Translate.GetSafeTranslationValue("Suchen")
			' Die Buttons Drucken und Export aktivieren

			If Me.gvRP.RowCount > 0 Then
				Me.bbiPrint.Enabled = True
				Me.bbiSetting.Enabled = True

				CreatePrintPopupMenu()
				CreateExportPopupMenu()
			End If

			Me.bbiSearch.Enabled = True
			Me.bbiClear.Enabled = True

			SuchStatus = SuchStatusEnum.Abgebrochen ' Zustand zurücksetzen
			'Me.pnl_Infozeile.Visible = True

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)
		End Try
	End Sub

	''' <summary>
	''' Während der Datenübertragung zwischen Station und SQL-Server darf nicht unterbrochen werden,
	''' da andernfalls die Connection offen bleibt.
	''' Grund: Der Thread wird eben nicht unmittelbar beendet und somit kann auch kein Connection.Close()
	''' durchgeführt werden bzw. es bleibt so lange offen.
	''' </summary>
	''' <param name="Abort"></param>
	''' <remarks></remarks>
	Private Sub DelegateAllowAbort(ByVal Abort As Boolean)
		_abortDeniedPending = Not Abort
		bbiSearch.Enabled = Abort

		'Close MessageBox programmatically gives you a lot of work. Never mind...A.Ragusa 01.11.2010
		' z.B. --> http://p2p.wrox.com/vb-how/13948-programmatically-close-message-box.html

	End Sub
#End Region


#Region "Enum und Property"


	'Enum Arrow As Integer
	'	Left
	'	Right
	'End Enum

	'Enum LibShowListViewText As Integer
	'	ShowListView
	'	ShowListViewFailed
	'End Enum

	'Private ReadOnly Property GetLibShowListViewText(ByVal LibText As LibShowListViewText) As String
	'	Get
	'		If LibText = LibShowListViewText.ShowListView Then
	'			Return m_Translate.GetSafeTranslationValue("Korrekte Liste anzeigen")
	'		End If
	'		Return m_Translate.GetSafeTranslationValue("Fehlerhafte Liste anzeigen")
	'	End Get
	'End Property

	'Dim _lvCheckStatus As CheckState = CheckState.Checked
	'Dim _lvFailedCheckStatus As CheckState = CheckState.Checked

	'Enum SelectText As Integer
	'	AlleAusgewählt
	'	AlleAbgewählt
	'End Enum


	''' <summary>
	''' Wenn alle ausgewählt sind, so wird man nur alle abwählen können und umgekehrt.
	''' </summary>
	''' <param name="sel"></param>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Private ReadOnly Property GetSelectAllText(ByVal sel As Boolean) As String
		Get
			If sel Then
				Return m_Translate.GetSafeTranslationValue("Alle Zeilen deaktivieren")
			End If
			Return m_Translate.GetSafeTranslationValue("Alle Zeilen aktivieren")
		End Get
	End Property

	'Dim _selectText As SelectText = SelectText.AlleAusgewählt
	'Dim _selectFailedText As SelectText = SelectText.AlleAusgewählt

	Enum SuchStatusEnum As Integer
		Suchend
		Abgebrochen
	End Enum

	Dim _suchStatus As SuchStatusEnum = SuchStatusEnum.Abgebrochen
	Public Property SuchStatus() As SuchStatusEnum
		Get
			Return _suchStatus
		End Get
		Set(ByVal value As SuchStatusEnum)
			_suchStatus = value
		End Set
	End Property

	''' <summary>
	''' Bezeichnung im LinkButton
	''' </summary>
	''' <remarks></remarks>
	Enum ListViewTitelEnum As Integer
		Korrekt
		Fehlgeschlagen
	End Enum

#End Region

#Region "Dropdown Funktionen Allgemein"

	Private Sub LoadMandantenDropDown()
		Dim m_CommonDatabaseAccess = New SP.DatabaseAccess.Common.CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

		Dim Data = m_CommonDatabaseAccess.LoadCompaniesListData

		lueMandant.Properties.DataSource = Data
		lueMandant.Properties.ForceInitialize()
	End Sub

	Private Function LoadCountryDropDown() As Boolean
		Dim result As Boolean = True

		If lueMandant.EditValue Is Nothing OrElse Cbo_Jahr.EditValue Is Nothing Then Return True
		Dim data = m_ListingDatabaseAccess.LoadTaxCountryCodeData(lueMandant.EditValue, Cbo_Jahr.EditValue, 1, 12, String.Empty)
		If data Is Nothing Then
			Dim msg As String = "Länder Daten konnten nicht geladen werden."
			msg = m_Translate.GetSafeTranslationValue(msg)

			m_UtilityUI.ShowErrorDialog(msg)

			Return False
		End If
		Dim listData = New BindingList(Of CountryData)

		For Each itm In data
			Dim code = New CountryData


			If Not String.IsNullOrWhiteSpace(itm.Code) Then

				If Not m_CountryData Is Nothing AndAlso m_CountryData.Count > 0 Then
					Dim lndData = New SP.Internal.Automations.CVLBaseTableViewData
					lndData = m_CountryData.Where(Function(x) x.Code = itm.Code).FirstOrDefault()
					If Not lndData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(lndData.Translated_Value) Then
						code.Code = itm.Code
						code.Name = lndData.Translated_Value
					End If

				Else
					code.Code = itm.Code
					code.Name = itm.Name

				End If

				listData.Add(code)
			End If

		Next
		lueCountry.Properties.DataSource = listData
		lueCountry.Properties.DropDownRows = Math.Min(20, listData.Count + 1)
		lueCountry.Enabled = listData.Count > 0


		Return Not listData Is Nothing

	End Function

	Private Function LoadPermissionCodeDropDown() As Boolean
		Dim result As Boolean = True
		Dim listData = New BindingList(Of QSTPermissionData)

		luePermissionCode.EditValue = Nothing
		If lueMandant.EditValue Is Nothing OrElse Cbo_Jahr.EditValue Is Nothing Then Return True
		Try

			Dim data = m_ListingDatabaseAccess.LoadQSTPermissionData(lueMandant.EditValue, Cbo_Jahr.EditValue, 1, 12, String.Empty, String.Empty)
			If data Is Nothing Then
				Dim msg As String = "Bewilligung Daten konnten nicht geladen werden."
				msg = m_Translate.GetSafeTranslationValue(msg)

				m_UtilityUI.ShowErrorDialog(msg)

				Return False
			End If

			Dim code = New QSTPermissionData
			For Each itm In data
				code = New QSTPermissionData

				If Not String.IsNullOrWhiteSpace(itm.PermissionCode) Then
					code.PermissionCode = itm.PermissionCode
					code.PermissionCodeLabel = itm.PermissionCodeLabel

					If Not m_PermissionData Is Nothing AndAlso m_PermissionData.Count > 0 Then
						Dim bewdata = New SP.Internal.Automations.PermissionData
						bewdata = m_PermissionData.Where(Function(x) x.Code = itm.PermissionCode).FirstOrDefault()
						If Not bewdata Is Nothing AndAlso Not String.IsNullOrWhiteSpace(bewdata.Translated_Value) Then
							code.PermissionCode = itm.PermissionCode
							code.PermissionCodeLabel = bewdata.Translated_Value
						End If
					End If

					listData.Add(code)
				End If

			Next

			code = New QSTPermissionData
			code.PermissionCode = "S"
			code.PermissionCodeLabel = "CH als Nationalität!"
			listData.Add(code)


			luePermissionCode.Properties.DataSource = listData
			luePermissionCode.Properties.DropDownRows = Math.Min(20, listData.Count + 1)
			luePermissionCode.Enabled = listData.Count > 0

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return True
		End Try


		Return Not listData Is Nothing

	End Function


	'Private Function LoadPermissionDropDownData() As Boolean
	'	Dim permissionData = m_CommonDatabaseAccess.LoadPermissionData()

	'	If (permissionData Is Nothing) Then
	'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Verfügbare Bewilligungsdaten konnten nicht geladen werden."))
	'	End If

	'	luePermission.Properties.DataSource = permissionData
	'	luePermission.Properties.ForceInitialize()


	'	Return Not permissionData Is Nothing
	'End Function

	Private Sub OnCbo_MAKanton_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MAKanton.QueryPopUp
		If Cbo_Jahr.EditValue Is Nothing Then Cbo_Jahr.EditValue = Now.Year
		ListMAKanton(Me.Cbo_MAKanton, Cbo_Jahr.EditValue)
	End Sub

	'Private Sub OnCbo_MALand_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MALand.QueryPopUp
	'	If Cbo_Jahr.EditValue Is Nothing Then Cbo_Jahr.EditValue = Now.Year
	'	ListMANationality(Me.Cbo_MALand, Cbo_Jahr.EditValue)
	'End Sub

#End Region

#Region "Allgemeine Funktionen"

	'' Monate 1 bis 12
	'Private Sub Monate1bis12(ByVal sender As System.Object, ByVal e As System.EventArgs)
	'  If TypeOf (sender) Is myCbo Then
	'    Dim cbo As myCbo = DirectCast(sender, myCbo)
	'    If cbo.Items.Count = 0 Then ListCboMonate1Bis12(cbo)
	'  End If
	'End Sub

#End Region

#Region "Private Methods"

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderFett.Text)
		Me.lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(Me.lblHeaderNormal.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblMDName.Text = m_Translate.GetSafeTranslationValue(Me.lblMDName.Text)
		Me.lblSortieren.Text = m_Translate.GetSafeTranslationValue(Me.lblSortieren.Text)
		Me.lbl_JahrVon.Text = m_Translate.GetSafeTranslationValue(Me.lbl_JahrVon.Text)

		Me.grpKandidatendaten.Text = m_Translate.GetSafeTranslationValue(Me.grpKandidatendaten.Text)
		Me.LibMANr.Text = m_Translate.GetSafeTranslationValue(Me.LibMANr.Text)
		Me.lblBewilligung.Text = m_Translate.GetSafeTranslationValue(Me.lblBewilligung.Text)

		Me.tgsSelection.Properties.OnText = m_Translate.GetSafeTranslationValue(Me.tgsSelection.Properties.OnText)
		Me.tgsSelection.Properties.OffText = m_Translate.GetSafeTranslationValue(Me.tgsSelection.Properties.OffText)

		Me.xtabAllgemein.Text = m_Translate.GetSafeTranslationValue(Me.xtabAllgemein.Text)
		Me.xtabSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.xtabSQLAbfrage.Text)
		Me.lblSQLAbfrage.Text = m_Translate.GetSafeTranslationValue(Me.lblSQLAbfrage.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiSetting.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSetting.Caption)

	End Sub


#End Region


#Region "Form Utility"

	Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmLoNLASearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded(frmMyLVName, True)

		Try
			My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.ifrmHeight = Me.Height
			My.Settings.ifrmWidth = Me.Width

			My.Settings.SortNumber = Me.lueSort.EditValue

			My.Settings.Save()

			If Directory.GetFiles(String.Format("{0}", _ClsProgSetting.GetSpSNLATempPath), "*.pdf").Count > 0 Then
				Dim strMsg As String = "In Ihrem Verzeichnis existieren Temporäre Dateien, welche bei Bedarf gelöscht werden können.{0}Möchten Sie diese löschen?"
				strMsg = String.Format(m_Translate.GetSafeTranslationValue(strMsg), vbNewLine)

				If DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_Translate.GetSafeTranslationValue("Temporäre Daten löschen"),
															 MessageBoxButtons.YesNo,
															 MessageBoxIcon.Question,
															 MessageBoxDefaultButton.Button1) = DialogResult.Yes Then
					Directory.Delete(String.Format("{0}", _ClsProgSetting.GetSpSNLATempPath), True)
				End If
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub


	''' <summary>
	''' Beim Starten der Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub OnFormLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		TranslateControls()
		SetInitialFields()

	End Sub


#End Region


	Private Sub SetInitialFields()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Formstyle. {0}", ex.Message))

		End Try
		Try
			Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
			If My.Settings.frm_Location <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = "0"
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Setting FormSize:{0}", ex.Message))

		End Try

		Try
			Me.lueMandant.EditValue = m_InitializationData.MDData.MDNr
			Me.lueMandant.Visible = IsUserActionAllowed(m_InitializationData.UserData.UserNr, 675, m_InitializationData.MDData.MDNr)
			Me.lblMDName.Visible = Me.lueMandant.Visible

			AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

			Me.xtabSQLAbfrage.PageVisible = m_InitializationData.UserData.UserNr = 1
			FillDefaultValues()
			LoadCountryDropDown()
			LoadPermissionCodeDropDown()

			lueSort.EditValue = My.Settings.SortNumber
			If lueSort.EditValue Is Nothing Then lueSort.EditValue = 0


		Catch ex As Exception
			m_Logger.LogError(String.Format("Mandantenauswahl anzeigen: {0}", ex.Message))
			Me.lueMandant.Visible = False
			Me.lblMDName.Visible = False
		End Try

	End Sub

	''' <summary>
	''' Default-Werte in den Felder Monat und Jahr einfügen.
	''' </summary>
	''' <remarks></remarks>
	Sub FillDefaultValues()

		FillDefaultDates()

	End Sub

	Private Sub FillDefaultDates()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			' DATUM ---------------------------------
			' Dropdown Jahr muss vorbelegt sein.
			If Me.Cbo_Jahr.Properties.Items.Count = 0 Then
				ListLOJahr(Me.Cbo_Jahr)
			End If
			Me.Cbo_Jahr.Text = If(Month(Now) < 6, Year(Now) - 1, Year(Now))

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	''' <summary>
	''' Daten fürs Drucken bereit stellen.
	''' </summary>
	Sub GetSQLData4Print(ByVal strJobInfo As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim iESNr As Integer = 0
		Dim bResult As Boolean = True
		Dim storedProc As String = ""
		Dim anzMax As Integer = 0
		Dim lohnTextManr As ArrayList = New ArrayList()

		ClsDataDetail.PrintSelection = ClsDataDetail.PrintSelectionEnum.Alles
		ClsDataDetail.GetFilename4USSign = String.Empty
		ClsDataDetail.IsLastPrintjobAsExport = False
		ClsDataDetail.bSentIntoToWOS = False

		Try

			Directory.Delete(String.Format("{0}", _ClsProgSetting.GetSpSNLATempPath), True)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Dateien löschen.", strMethodeName))

		End Try

		Dim sSql As String = Me.txt_SQLQuery.Text
		If sSql = String.Empty Then
			m_Logger.LogError(String.Format("{0}.Keine Suche wurde gestartet.", strMethodeName))
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet!"))
			Exit Sub
		End If

		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim cmdText As String = String.Format("SELECT * FROM {0} ", ClsDataDetail.LLTablename)
		cmdText += "WHERE MANR IN ("

		grdRP.RefreshDataSource()
		Dim employeeList As BindingList(Of FoundedData) = grdRP.DataSource
		Dim employeesToProcess = employeeList.Where(Function(data) data.IsSelected = True)

		For Each foundeddata In employeesToProcess
			If foundeddata.IsSelected Then
				cmdText += String.Format("{0},", foundeddata.MANr)
				anzMax += 1
				lohnTextManr.Add(foundeddata.MANr)
			End If
		Next

		Dim SortString As String = GetSortString(m_SearchCriteria.sortvalue)
		If String.IsNullOrWhiteSpace(SortString) Then SortString = " Order By Nachname, Vorname"
		cmdText = cmdText.Substring(0, cmdText.Length - 1) + ") " & SortString

		Dim cmd As New SqlCommand(cmdText, Conn)

		ClsDataDetail.AnzMax = anzMax

		Try
			Conn.Open()
			Dim rNLAListerec As SqlDataReader = cmd.ExecuteReader
			Try
				If Not rNLAListerec.HasRows Then
					cmd.Dispose()
					rNLAListerec.Close()
					m_Logger.LogWarning(String.Format("{0}.Keine Daten wurden gefunden.", strMethodeName))
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Ich konnte leider Keine Daten finden."))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try
			cmd.Dispose()
			rNLAListerec.Close()
			Me.Conn2Open = Conn
			Me.SQL4Print = cmd.CommandText
			Me.SelectedYear2Print = CInt(Me.Cbo_Jahr.Text)
			StartPrinting()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		Finally
			cmd.Dispose()
			Conn.Close()
		End Try


	End Sub

	Sub StartPrinting()
		If CreatedJobForExport.GetValueOrDefault(False) Then SendPrintJob2WOS = Nothing

		Dim _ClsSetting As New SPS.Fill.NLAFields.ClsNLASetting With {.DbConn2Open = Me.Conn2Open,
																																	.SelectedYear = Me.SelectedYear2Print,
																																	.SQL2Open = Me.SQL4Print,
																																	.CreateJobsForExport = CreatedJobForExport.GetValueOrDefault(False),
																																	.Send2WOS = Me.SendPrintJob2WOS}
		Dim _ClsPDF As New SPS.Fill.NLAFields.FillNLA.ClsFillStart(0, _ClsSetting, m_InitializationData)

		'Dim fieldsData = _ClsPDF.LoadPDFFields(String.Empty)

		Dim result = _ClsPDF.StartFillingNLA(Me.SQL4Print)
		If result Then
			Dim fileToPrint As String = _ClsPDF.GetDoneNLAFiles

			Try

				If _ClsPDF.GetSentNLAWOSFiles.Count > 0 Then
					Dim msg = m_Translate.GetSafeTranslationValue("Ihre Dokumente wurden erfolgreich übermitteilt.")

					m_UtilityUI.ShowInfoDialog(msg)

					If String.IsNullOrWhiteSpace(fileToPrint) Then Return
				End If

				Dim pdfFilesToSend As New List(Of String)
				If File.Exists(fileToPrint) Then
					Dim newFilename As String = Path.Combine(Path.GetDirectoryName(fileToPrint), String.Format("{0}", "Lohnausweise.PDF"))
					If File.Exists(newFilename) Then File.Delete(newFilename)
					IO.File.Move(fileToPrint, newFilename)
					pdfFilesToSend.Add(newFilename)

				End If

				If pdfFilesToSend.Count > 0 Then
					Dim tmpfileName As String = pdfFilesToSend(0)
					Dim pdfViewer As New frmViewPDF(tmpfileName)
					pdfViewer.OpenPDFDocument()

					If OpenNLAPDFFileForPrint.GetValueOrDefault(False) Then
						pdfViewer.Show()
						pdfViewer.BringToFront()
					Else
						pdfViewer.PdfViewer.Print()
						pdfViewer.PdfViewer.CloseDocument()
					End If
				End If


				'Dim docServer As New PdfDocumentProcessor()
				'docServer.LoadDocument(fileToPrint)

				'Dim printerSetting As PrinterSettings
				'Dim dlgPrinter As New PrintDialog

				'dlgPrinter.ShowNetwork = True
				'If dlgPrinter.ShowDialog(Me) = DialogResult.OK Then
				'	printerSetting = dlgPrinter.PrinterSettings
				'Else
				'	Return
				'End If
				'Dim pdfPrinterSettings As New PdfPrinterSettings(printerSetting)

				'docServer.Print(pdfPrinterSettings)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				m_UtilityUI.ShowErrorDialog(ex.ToString)

			End Try

		End If

	End Sub

	Function GetSearchKrieteria() As SearchCriteria
		Dim result As New SearchCriteria

		result.sortvalue = Val(lueSort.EditValue)

		Dim bisjahr As String = Me.Cbo_Jahr.EditValue

		If Me.Cbo_Jahr.EditValue Is Nothing Then Me.Cbo_Jahr.EditValue = CStr(Year(Now))
		bisjahr = Cbo_Jahr.EditValue

		result.jahr = bisjahr

		result.listname = m_Translate.GetSafeTranslationValue("Aufstellung über Lohnausweise")
		result.mandantenname = lueMandant.Text

		result.MANr = txt_MANr.EditValue
		result.FromEmployee = txt_NameVon.EditValue
		result.ToEmployee = txt_NameBis.EditValue

		result.employeeCanton = Cbo_MAKanton.EditValue
		result.employeeCountry = lueCountry.EditValue
		result.employeePermission = luePermissionCode.EditValue


		Return result

	End Function

#Region "Funktionen zur Menüaufbau..."

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Stopwatch As Stopwatch = New Stopwatch()
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		Try

			If Not (Kontrolle()) Then Exit Sub
			m_SearchCriteria = GetSearchKrieteria()

			Me.txt_SQLQuery.Text = String.Empty

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")
			'Me.StatusStrip1.Update()

			If SuchStatus = SuchStatusEnum.Abgebrochen Then
				FormIsLoaded(frmMyLVName, True)
			End If

			' PROGRESSBAR
			If _SynchronizingObject Is Nothing Then
				_SynchronizingObject = Me
				_NotifyMainProgressDelegate = New NotifyMainProgressDel(AddressOf DelegateProgress)
				_NotifyMainStartLVDelegate = New NotifyMainStartLVDel(AddressOf ResultatFensterAnzeigen)
			End If

			If SuchStatus = SuchStatusEnum.Abgebrochen Then
				' Die ListView zurücksetzen
				ResetGridNLAData()

				'ResetListView()
				Me.bbiClear.Enabled = False

				' Status nun suchend...
				SuchStatus = SuchStatusEnum.Suchend
				bbiSearch.Caption = m_Translate.GetSafeTranslationValue("Abbrechen")

				' Thread wird gestartet...
				_ThreadMain = New Threading.Thread(AddressOf SucheStarten)
				_ThreadMain.Name = "NLASuche"
				_ThreadMain.IsBackground = True ' So wird nicht verhindert, dass die Maske so lange offen bleibt, wie dieser Thread läuft.
				_ThreadMain.Start()
			Else
				If Not _abortDeniedPending Then
					bbiSearch.Enabled = False
				End If
				If m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie die Suche wirklich abbrechen?"),
													 m_Translate.GetSafeTranslationValue("Suche abbrechen"), MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
					If SuchStatus = SuchStatusEnum.Suchend Then
						_ThreadMain.Abort()
						SuchStatus = SuchStatusEnum.Abgebrochen
						bbiSearch.Caption = m_Translate.GetSafeTranslationValue("Suchen")
						NotifyMainProgressBar(m_Translate.GetSafeTranslationValue("Suche abgebrochen"), 0)
						Me.bbiClear.Enabled = True
					End If
				End If
				If Not _abortDeniedPending Then
					bbiSearch.Enabled = True
				End If
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError("btnSearch_Click", ex)
		Finally
		End Try

	End Sub

	''' <summary>
	''' Diese Methode wird vom Thread aufgerufen und hat auf der Maske keine Rechte mehr.
	''' Jegliche von dieser Methode folgende Aufrufe laufen im diesem Thread und werden
	''' geschlossen, so bald der Thread beendet wird. Rechte haben diese übrigens ebenso keine.
	''' </summary>
	''' <remarks></remarks>
	Private Sub SucheStarten()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Stopwatch As Stopwatch = New Stopwatch()
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty

		Try
			NotifyMainProgressBar(m_Translate.GetSafeTranslationValue("Die Query wird zusammengestellt"), 1)
			' Die Query-String aufbauen...

			GetMyQueryString(Me)
			NotifyMainProgressBar(m_Translate.GetSafeTranslationValue("Die Query wird zusammengestellt"), 90)

			NotifyMainProgressBar(m_Translate.GetSafeTranslationValue("Suche abgeschlossen"), 100)

			' Die ListView befindet sich neu auf dem Hauptfenster
			NotifyStartLV("ListView")


		Catch ex As Threading.ThreadAbortException
			' Der Thread wurde abgebrochen --> Kein Fehler
		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError("SucheStarten", ex)
		End Try
	End Sub

	Private Sub ResultatFensterAnzeigen()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			NotifyMainProgressBar(m_Translate.GetSafeTranslationValue("Daten werden aufbereitet"), 1)

			_NotifyMainLVCompletedDelegate = New NotifyMainLVCompletedDel(AddressOf DelegateLVCompleted)

			' Die Header von normalen und fehlgeschlagenen ListView setzen
			ResetGridNLAData()
			LoadFoundedNLAData(True)

			'LibSelectAll.Text = GetSelectAllText(m_ISAllRowsSelected)

			NotifyMainProgressBar(m_Translate.GetSafeTranslationValue("Daten Aufbereitung abgeschlossen"), 100)
			NotifyMainLVCompleted(m_Translate.GetSafeTranslationValue("ListView füllen abgeschlossen"))

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError("ResultatFensterAnzeigen", ex)
		End Try
	End Sub


#Region "Reset und Load data..."


	Private Sub ResetGridNLAData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never
		Dim RepositoryCheckCol As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit = New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
		grdRP.RepositoryItems.Add(RepositoryCheckCol)

		gvRP.Columns.Clear()

		'		gvRP.Columns.Add(New Columns.GridColumn() With {.Name = "IsSelected", .FieldName = "IsSelected", .Visible = True, .Width = 10})

		Dim columnIsSelected As New DevExpress.XtraGrid.Columns.GridColumn()
		columnIsSelected.Caption = m_Translate.GetSafeTranslationValue("Ausgewählt")
		columnIsSelected.Name = "IsSelected"
		columnIsSelected.FieldName = "IsSelected"
		columnIsSelected.Visible = True
		columnIsSelected.ColumnEdit = RepositoryCheckCol
		columnIsSelected.OptionsColumn.AllowEdit = True
		gvRP.Columns.Add(columnIsSelected)


		Dim columnMANr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnMANr.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Nr.")
		columnMANr.Name = "MANr"
		columnMANr.FieldName = "MANr"
		columnMANr.Visible = False
		columnMANr.OptionsColumn.AllowEdit = False
		gvRP.Columns.Add(columnMANr)

		Dim columnLastnameFirstname As New DevExpress.XtraGrid.Columns.GridColumn()
		columnLastnameFirstname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnLastnameFirstname.Caption = m_Translate.GetSafeTranslationValue("Kandidat")
		columnLastnameFirstname.Name = "LastnameFirstname"
		columnLastnameFirstname.FieldName = "LastnameFirstname"
		columnLastnameFirstname.Visible = True
		columnLastnameFirstname.BestFit()
		columnLastnameFirstname.OptionsColumn.AllowEdit = False
		gvRP.Columns.Add(columnLastnameFirstname)

		Dim columnemployeemastrasse As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeemastrasse.Caption = m_Translate.GetSafeTranslationValue("Strasse")
		columnemployeemastrasse.Name = "employeemastrasse"
		columnemployeemastrasse.FieldName = "employeemastrasse"
		columnemployeemastrasse.Visible = True
		columnemployeemastrasse.OptionsColumn.AllowEdit = False
		gvRP.Columns.Add(columnemployeemastrasse)

		Dim columnPostcodeCity As New DevExpress.XtraGrid.Columns.GridColumn()
		columnPostcodeCity.Caption = m_Translate.GetSafeTranslationValue("PLZ Ort")
		columnPostcodeCity.Name = "PostcodeCity"
		columnPostcodeCity.FieldName = "PostcodeCity"
		columnPostcodeCity.Visible = True
		columnPostcodeCity.OptionsColumn.AllowEdit = False
		gvRP.Columns.Add(columnPostcodeCity)


		Dim columnemployeemaland As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeemaland.Caption = m_Translate.GetSafeTranslationValue("Land")
		columnemployeemaland.Name = "employeemaland"
		columnemployeemaland.FieldName = "employeemaland"
		columnemployeemaland.Visible = True
		columnemployeemaland.OptionsColumn.AllowEdit = False
		gvRP.Columns.Add(columnemployeemaland)

		Dim columnemployeeahv_nr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeahv_nr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeahv_nr.Caption = m_Translate.GetSafeTranslationValue("AHV-Nr.")
		columnemployeeahv_nr.Name = "employeeahv_nr"
		columnemployeeahv_nr.FieldName = "employeeahv_nr"
		columnemployeeahv_nr.Visible = False
		columnemployeeahv_nr.BestFit()
		columnemployeeahv_nr.OptionsColumn.AllowEdit = False
		gvRP.Columns.Add(columnemployeeahv_nr)

		Dim columnemployeeahv_nr_new As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeahv_nr_new.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeahv_nr_new.Caption = m_Translate.GetSafeTranslationValue("AHV-Nr. neu")
		columnemployeeahv_nr_new.Name = "employeeahv_nr_new"
		columnemployeeahv_nr_new.FieldName = "employeeahv_nr_new"
		columnemployeeahv_nr_new.Visible = True
		columnemployeeahv_nr_new.BestFit()
		columnemployeeahv_nr_new.OptionsColumn.AllowEdit = False
		gvRP.Columns.Add(columnemployeeahv_nr_new)


		AddHandler RepositoryCheckCol.EditValueChanged, AddressOf RepositoryCheckCol_EditValueChanged

		grdRP.DataSource = Nothing

		ClsDataDetail.SelectedDataTable.Clear()
		ClsDataDetail.SelectedDataTableFailed.Clear()

		bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

	End Sub

	Function GetDbNLAData4Show(ByVal failedData As Boolean?) As IEnumerable(Of FoundedData)
		Dim result As List(Of FoundedData) = Nothing
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities

		Dim sql As String = String.Format("Select * From {0} {1}", ClsDataDetail.LLTablename, GetSortString(lueSort.EditValue))
		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(m_InitializationData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData

					overviewData.MANr = m_utility.SafeGetInteger(reader, "MANr", Nothing)

					'MANR, Nachname, Vorname, MAStrasse, MAPLZ, MAOrt, MALand, MACo, AHV_Nr, AHV_Nr_New, Send2WOS, Geschlecht, MAPostfach,LAJahr
					'Z_1_0, Z_2_1, Z_2_2, Z_2_3, Z_3_0, Z_4_0, Z_5_0, Z_6_0, Z_7_0, Z_8_0, Z_9_0, Z_10_1, Z_10_2, Z_11_0, Z_12_0, 
					'Z_13_1_1, Z_13_1_2, Z_13_2_1, Z_13_2_2, Z_13_2_3, Z_13_3_0

					overviewData.employeefirstname = String.Format("{0}", m_utility.SafeGetString(reader, "Vorname"))
					overviewData.employeelastname = String.Format("{0}", m_utility.SafeGetString(reader, "Nachname"))
					overviewData.employeemastrasse = m_utility.SafeGetString(reader, "MAStrasse")
					overviewData.employeemaplz = m_utility.SafeGetString(reader, "MAPLZ")
					overviewData.employeemaort = m_utility.SafeGetString(reader, "MAOrt")

					overviewData.employeemaland = m_utility.SafeGetString(reader, "MALand")
					overviewData.employeemaco = m_utility.SafeGetString(reader, "MACo")

					overviewData.employeeahv_nr = m_utility.SafeGetString(reader, "AHV_Nr")
					overviewData.employeeahv_nr_new = m_utility.SafeGetString(reader, "AHV_Nr_New")

					overviewData.employeesend2wos = m_utility.SafeGetBoolean(reader, "Send2WOS", Nothing)
					overviewData.employeegeschlecht = m_utility.SafeGetString(reader, "Geschlecht")
					overviewData.employeemapostfach = m_utility.SafeGetString(reader, "MAPostfach")
					overviewData.employeelajahr = m_utility.SafeGetInteger(reader, "LAJahr", Nothing)

					overviewData.Z_1_0 = m_utility.SafeGetDecimal(reader, "Z_1_0", Nothing)
					overviewData.Z_2_1 = m_utility.SafeGetDecimal(reader, "Z_2_1", Nothing)
					overviewData.Z_2_2 = m_utility.SafeGetDecimal(reader, "Z_2_2", Nothing)
					overviewData.Z_2_3 = m_utility.SafeGetDecimal(reader, "Z_2_3", Nothing)
					overviewData.Z_3_0 = m_utility.SafeGetDecimal(reader, "Z_3_0", Nothing)
					overviewData.Z_4_0 = m_utility.SafeGetDecimal(reader, "Z_4_0", Nothing)
					overviewData.Z_5_0 = m_utility.SafeGetDecimal(reader, "Z_5_0", Nothing)
					overviewData.Z_6_0 = m_utility.SafeGetDecimal(reader, "Z_6_0", Nothing)
					overviewData.Z_7_0 = m_utility.SafeGetDecimal(reader, "Z_7_0", Nothing)
					overviewData.Z_8_0 = m_utility.SafeGetDecimal(reader, "Z_8_0", Nothing)
					overviewData.Z_9_0 = m_utility.SafeGetDecimal(reader, "Z_9_0", Nothing)
					overviewData.Z_10_1 = m_utility.SafeGetDecimal(reader, "Z_10_1", Nothing)
					overviewData.Z_10_2 = m_utility.SafeGetDecimal(reader, "Z_10_2", Nothing)
					overviewData.Z_11_0 = m_utility.SafeGetDecimal(reader, "Z_11_0", Nothing)
					overviewData.Z_12_0 = m_utility.SafeGetDecimal(reader, "Z_12_0", Nothing)
					overviewData.Z_13_1_1 = m_utility.SafeGetDecimal(reader, "Z_13_1_1", Nothing)
					overviewData.Z_13_1_2 = m_utility.SafeGetDecimal(reader, "Z_13_1_2", Nothing)
					overviewData.Z_13_2_1 = m_utility.SafeGetDecimal(reader, "Z_13_2_1", Nothing)
					overviewData.Z_13_2_2 = m_utility.SafeGetDecimal(reader, "Z_13_2_2", Nothing)
					overviewData.Z_13_2_3 = m_utility.SafeGetDecimal(reader, "Z_13_2_3", Nothing)
					overviewData.Z_13_3_0 = m_utility.SafeGetDecimal(reader, "Z_13_3_0", Nothing)

					'NLA_LoAusweis, NLA_Befoerderung, NLA_Kantine
					'NLA_2_3, NLA_3_0, NLA_4_0, NLA_7_0
					'NLA_Spesen_NotShow, NLA_13_1_2, NLA_13_2_3
					'NLA_Nebenleistung_1, NLA_Nebenleistung_2, NLA_Bemerkung_1, NLA_Bemerkung_2, Grund
					'ES_Ab1, ES_Bis1

					overviewData.NLA_LoAusweis = m_utility.SafeGetBoolean(reader, "NLA_LoAusweis", Nothing)
					overviewData.NLA_Befoerderung = m_utility.SafeGetBoolean(reader, "NLA_Befoerderung", Nothing)
					overviewData.NLA_Kantine = m_utility.SafeGetBoolean(reader, "NLA_Kantine", Nothing)

					overviewData.NLA_2_3 = m_utility.SafeGetString(reader, "NLA_2_3")
					overviewData.NLA_3_0 = m_utility.SafeGetString(reader, "NLA_3_0")
					overviewData.NLA_4_0 = m_utility.SafeGetString(reader, "NLA_4_0")
					overviewData.NLA_7_0 = m_utility.SafeGetString(reader, "NLA_7_0")

					overviewData.NLA_Spesen_NotShow = m_utility.SafeGetBoolean(reader, "NLA_LoAusweis", Nothing)
					overviewData.NLA_13_1_2 = m_utility.SafeGetString(reader, "NLA_13_1_2")
					overviewData.NLA_13_2_3 = m_utility.SafeGetString(reader, "NLA_13_2_3")

					overviewData.NLA_Nebenleistung_1 = m_utility.SafeGetString(reader, "NLA_Nebenleistung_1")
					overviewData.NLA_Nebenleistung_2 = m_utility.SafeGetString(reader, "NLA_Nebenleistung_2")
					overviewData.NLA_Bemerkung_1 = m_utility.SafeGetString(reader, "NLA_Bemerkung_1")
					overviewData.NLA_Bemerkung_2 = m_utility.SafeGetString(reader, "NLA_Bemerkung_2")
					overviewData.Grund = m_utility.SafeGetString(reader, "Grund")

					overviewData.ES_Ab1 = m_utility.SafeGetString(reader, "ES_Ab1")
					overviewData.ES_Bis1 = m_utility.SafeGetString(reader, "ES_Bis1")


					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result
	End Function

	Private Function LoadFoundedNLAData(ByVal failedData As Boolean?) As Boolean

		Dim listOfEmployees = GetDbNLAData4Show(failedData)

		If listOfEmployees Is Nothing Then
			m_UtilityUI.ShowErrorDialog("Die Daten konnten nicht geladen werden.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
										  Select New FoundedData With
													   {.MANr = person.MANr,
														  .employeefirstname = person.employeefirstname,
														  .employeelastname = person.employeelastname,
														  .employeeahv_nr = person.employeeahv_nr,
														  .employeelajahr = person.employeelajahr,
														  .employeeahv_nr_new = person.employeeahv_nr_new,
														  .employeemastrasse = person.employeemastrasse,
														  .employeemaort = person.employeemaort,
														  .employeemaland = person.employeemaland,
														  .IsSelected = tgsSelection.EditValue
													   }).ToList()

		Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		m_ISAllRowsSelected = tgsSelection.EditValue
		'tgsSelection.EditValue = True

		Return Not listOfEmployees Is Nothing
	End Function


#End Region


	Function Kontrolle() As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			Dim msg As String = ""
			Me.txt_NameVon.Text = Me.txt_NameVon.Text.Replace("*", "%") '.Replace("%", "")
			If Me.txt_NameVon.Text.Length < 3 AndAlso Me.txt_NameVon.Text.Length > 0 AndAlso Not Me.txt_NameVon.Text.Contains("%") Then Me.txt_NameVon.Text &= "%"
			Me.txt_NameBis.Text = If(Me.txt_NameVon.Text.Contains("%"), "", Me.txt_NameBis.Text)

			ClsDataDetail.Param.Jahr = Me.Cbo_Jahr.Text
			ClsDataDetail.Param.MANR = Me.txt_MANr.Text.Replace("*", "").Replace("%", "")
			ClsDataDetail.Param.NameVon = Me.txt_NameVon.Text '.Replace("*", "").Replace("%", "")
			ClsDataDetail.Param.NameBis = Me.txt_NameBis.Text

			If ClsDataDetail.Param.MANR.Trim.Length > 0 Then
				For Each manr As String In ClsDataDetail.Param.MANR.Split(CChar(","))
					If Not manr.Contains("-") Then
						If Not IsNumeric(manr.Trim()) Then
							msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist nicht numerisch.{1}"), manr, vbLf)
						ElseIf CInt(manr.Trim()).ToString <> manr.Trim() Then
							msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist ungültig.{1}"), manr, vbLf)
						End If
					Else
						Dim vonZahl As String = manr.Split(CChar("-"))(0)
						Dim bisZahl As String = manr.Split(CChar("-"))(1)
						If vonZahl.Length > 0 Then
							If Not IsNumeric(vonZahl) Then
								msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist nicht numerisch.{1}"), vonZahl, vbLf)
							End If
						End If
						If bisZahl.Length > 0 Then
							If Not IsNumeric(bisZahl) Then
								msg += String.Format(m_Translate.GetSafeTranslationValue("Die Kandidatennummer '{0}' ist nicht numerisch.{1}"), bisZahl, vbLf)
							End If
						End If

					End If
				Next
			End If
			ClsDataDetail.ConvertMANREntry()
			If ClsDataDetail.Param.MANR.Length > 3950 Then
				msg += m_Translate.GetSafeTranslationValue("Leider ist die Suchspanne zu gross. Bitte geben Sie entweder keine Von-Nummer " &
															 "oder Bis-Nummer an, oder wählen Sie eine kleinere Spanne.")
			End If

			If msg.Length > 0 Then
				MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Suchkriterien sind fehlerhaft:{0}{1}"), vbLf, msg),
												"Keine Suche möglich", MessageBoxButtons.OK, MessageBoxIcon.Information)
				Return False
			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUI.ShowErrorDialog(ex.ToString)
		End Try

		Return True
	End Function

	Function GetMyQueryString(ByVal frm As frmLoNLASearch) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			Dim sSqlQuerySelect As String = String.Empty
			Dim _ClsDb As New ClsDbFunc(m_SearchCriteria)

			_ClsDb._SynchronizingMain = Me
			_ClsDb._SynchronizingThread = _ThreadMain
			_ClsDb._NotifyMainProgressDelegate = New ClsDbFunc.NotifyMainProgressDel(AddressOf DelegateProgress)
			_ClsDb._NotifyMainAllowAbortDelegate = New ClsDbFunc.NotifyMainAllowAbortDel(AddressOf DelegateAllowAbort)

			_ClsDb.GetQuerySQLString()
			Dim UpdateDelegate As New MethodInvoker(AddressOf UpdateSQLQuery)
			Me.txt_SQLQuery.Invoke(UpdateDelegate)
			'Me.txt_SQLQuery.Text = ClsDataDetail.txt_SQLQuery

			NotifyMainProgressBar(m_Translate.GetSafeTranslationValue("Abfrage wurde beendet"), 80)

		Catch ex As Threading.ThreadAbortException
			' Der Thread wurde abgebrochen --> Kein Fehler
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUI.ShowErrorDialog(ex.ToString)
		End Try

		Return True
	End Function

	Sub UpdateSQLQuery()
		Me.txt_SQLQuery.Text = ClsDataDetail.txt_SQLQuery
	End Sub

	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			FormIsLoaded(frmMyLVName, True)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

			ResetAllTabEntries()

			FillDefaultValues()

			Me.txt_SQLQuery.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")
			ResetGridNLAData()

			'ResetListView()

			Me.bbiPrint.Enabled = False

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUI.ShowErrorDialog(ex.ToString)
		End Try

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item("XtraTabControl1").Controls
				For Each con As Control In tabPg.Controls
					ResetControl(con)
				Next
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Bei GroupBox wird die Funktion rekursiv aufgerufen.</remarks>
	Private Sub ResetControl(ByVal con As Control)
		Try
			If con.Name.ToLower = lueMandant.Name.ToLower Or con.Name.ToLower = lueSort.Name.ToLower Then Exit Sub

			If TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
				Dim tb As DevExpress.XtraEditors.TextEdit = CType(con, DevExpress.XtraEditors.TextEdit)
				tb.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = CType(con, DevExpress.XtraEditors.ComboBoxEdit)
				cbo.EditValue = Nothing

			ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckedComboBoxEdit Then
				Dim cbo As DevExpress.XtraEditors.CheckedComboBoxEdit = CType(con, DevExpress.XtraEditors.CheckedComboBoxEdit)
				cbo.EditValue = Nothing

			ElseIf con.HasChildren Then
				For Each childCon As Control In con.Controls
					ResetControl(childCon)
				Next
			Else

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub


#End Region


#Region "bbi Buttons"

	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = CType(Me.bbiPrint.DropDownControl, DevExpress.XtraBars.PopupMenu)

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim allowedWOS As Boolean = m_mandant.AllowedExportEmployee2WOS(m_InitializationData.MDData.MDNr, Now.Year)
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {If(allowedWOS, "Nur drucken ohne Übermitteln", "Drucken") & "#PrintNoWOS",
																					 If(allowedWOS, "WOS übermitteln restliche drucken#SendWOSRestPrint", ""),
																					 If(allowedWOS, "Drucken und übermitteln#sendandprint", ""),
																					 "Alle in eine Datei exportieren#createonefile"}
		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))

				bshowMnu = myValue(0).ToString <> String.Empty
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)

					If myValue(1).ToString.ToLower.Contains("PrintDesign".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub GetMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.SQL4Print = Me.txt_SQLQuery.Text
		CreatedJobForExport = False
		If Not CheckTemplate() Then Return

		Try
			Select Case e.Item.Name.ToUpper
				Case "PrintNoWOS".ToUpper
					SendPrintJob2WOS = WOSSENDValue.PrintWithoutSending
					GetSQLData4Print("12.4.1")

				Case "sendandprint".ToUpper
					SendPrintJob2WOS = WOSSENDValue.PrintAndSend
					GetSQLData4Print("12.4.1")

				Case "SendWOSRestPrint".ToUpper
					SendPrintJob2WOS = WOSSENDValue.PrintOtherSendWOS
					GetSQLData4Print("12.4.1")

				Case "createonefile".ToUpper
					CreatedJobForExport = True
					GetSQLData4Print("12.4.1")

				Case Else
					Exit Sub

			End Select

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSetting.ItemClick
		Dim _Setting As New SPS.Fill.NLAFields.ClsNLASetting
		Dim obj As New SPS.Fill.NLAFields.FillNLA.ClsFillStart(_Setting, m_InitializationData)


		Dim frmSetting = New SPS.Fill.NLAFields.frmNLASetting(m_InitializationData)

		Dim result As DialogResult = frmSetting.ShowDialog()

		If result = DialogResult.OK AndAlso gvRP.RowCount > 0 Then
			Dim strMsg As String = "Wenn Sie die Einstellungen geändern haben, müssen Sie aber die Suche neu starten!"
			strMsg = m_Translate.GetSafeTranslationValue(strMsg)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_Translate.GetSafeTranslationValue("Einstellungen"),
																								 MessageBoxButtons.OK, MessageBoxIcon.Information,
																								 MessageBoxDefaultButton.Button1)
		End If

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As New List(Of String) From {"Daten in CSV- / TXT exportieren...#CSV"}
		Try
			bbiSetting.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiSetting.ActAsDropDown = False
			Me.bbiSetting.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiSetting.DropDownEnabled = True
			Me.bbiSetting.DropDownControl = popupMenu
			Me.bbiSetting.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = myValue(1).ToString

					If myValue(0).ToString.ToLower.Contains("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetExportMenuItem
				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strSQL As String = Me.txt_SQLQuery.Text

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("TXT"), UCase("CSV")
				StartExportModul()

		End Select

	End Sub

	Sub StartExportModul()
		'Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.m_InitialData.MDData.MDDbConn, _
		'																																	 .SQL2Open = Me.txt_SQLQuery.Text, _
		'																																	 .ModulName = "YFakListToCSV"}
		'Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)

		'obj.exportcsvfrom(Me.txt_SQLQuery.Text)

	End Sub

#End Region


	'Private Sub frmQSTListeSearch_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
	'	Try
	'		Me.Cbo_Jahr.Focus()
	'	Catch ex As Exception	
	'		m_Logger.LogError(String.Format("{0}", ex.ToString))
	'	End Try
	'End Sub

	''' <summary>
	''' Selektionsfenster für die erste Mitarbeiter-Nummer öffnen.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub txt_MANr_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_MANr.ButtonClick
		Try
			Dim frmTest As New frmSearchRec("MANr", Val(Me.Cbo_Jahr.Text))

			ClsDataDetail.StrButtonValue = ClsDataDetail.ButtonValue.KandidatenNummer
			ClsDataDetail.Get4What = ClsDataDetail.What.MANR

			frmTest.ShowDialog()
			frmTest.MdiParent = Me.MdiParent

			Dim m As String
			m = frmTest.iLoNLAValue(ClsDataDetail.Get4What.ToString)
			Me.txt_MANr.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace("#@", ",")))

			frmTest.Dispose()
		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	''' <summary>
	''' Selektionsfenster für Mitarbeiter-Name öffnen.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub txt_NameVon_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_NameVon.ButtonClick
		Try
			Dim frmTest As New frmSearchRec("Nachname", Val(Me.Cbo_Jahr.Text))

			ClsDataDetail.StrButtonValue = ClsDataDetail.ButtonValue.KandidatenNamen
			ClsDataDetail.Get4What = ClsDataDetail.What.Nachname

			frmTest.ShowDialog()
			frmTest.MdiParent = Me.MdiParent

			Dim m As String
			m = frmTest.iLoNLAValue(ClsDataDetail.Get4What.ToString)
			Me.txt_NameVon.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace("#@", ","))) ' CStr(m.ToString.Replace("#@", ","))

			frmTest.Dispose()
		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	''' <summary>
	''' Selektionsfenster für Mitarbeiter-Name öffnen.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub txt_NameBis_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_NameBis.ButtonClick
		Try
			Dim frmTest As New frmSearchRec("Nachname", Val(Me.Cbo_Jahr.Text))

			ClsDataDetail.StrButtonValue = ClsDataDetail.ButtonValue.KandidatenNamen
			ClsDataDetail.Get4What = ClsDataDetail.What.Nachname

			frmTest.ShowDialog()
			frmTest.MdiParent = Me.MdiParent

			Dim m As String
			m = frmTest.iLoNLAValue(ClsDataDetail.Get4What.ToString)
			Me.txt_NameBis.Text = If(m = Nothing, String.Empty, CStr(m.ToString.Replace("#@", ","))) ' CStr(m.ToString.Replace("#@", ","))

			frmTest.Dispose()
		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Sub

	Private Sub ChangeIsSelectedForAllRecords(ByVal selectAll As Boolean)

		Dim employeeList As BindingList(Of FoundedData) = grdRP.DataSource
		If employeeList Is Nothing Then Return
		Dim employeesToProcess = employeeList.Where(Function(data) data.IsSelected = Not selectAll)

		For Each foundeddata In employeeList
			foundeddata.IsSelected = selectAll
		Next
		grdRP.RefreshDataSource()

	End Sub

	Private Sub RepositoryCheckCol_EditValueChanged(ByVal sender As Object, ByVal e As EventArgs)

		gvRP.PostEditor()

	End Sub

	Private Sub OngvRP_FocusedRowChanged(sender As Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim selectedrow = SelectedRowViewData

			If Not selectedrow Is Nothing Then
				grdRP.RefreshDataSource()

				Dim employeeList As BindingList(Of FoundedData) = grdRP.DataSource
				Dim employeesToProcess = employeeList.Where(Function(data) data.IsSelected = True)

				Me.bsiInfo.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True
				Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl selektierte Datensätze: {0}"), employeesToProcess.Count)

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedData)

				Select Case column.Name.ToLower

					Case Else
						If viewData.MANr > 0 Then OpenSelectedEmployee(viewData.MANr)

				End Select

			End If

		End If

	End Sub

	Sub OpenSelectedEmployee(ByVal Employeenumber As Integer)

		Try
			Dim hub = MessageService.Instance.Hub
			Dim openMng As New OpenEmployeeMngRequest(Me, m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, Employeenumber)
			hub.Publish(openMng)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
		End Try

	End Sub

	Private Function CheckTemplate() As Boolean

		Try
			Dim sValue As Short = Me.GetJobID
			If sValue = 2 Then
				Dim strMsg As String = "Sie haben noch keine Vorlage (Adresse: Links- oder Rechtsbündig) ausgewählt!"
				strMsg = m_Translate.GetSafeTranslationValue(strMsg)
				DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_Translate.GetSafeTranslationValue("Einstellungen"),
																									 MessageBoxButtons.OK, MessageBoxIcon.Exclamation,
																									 MessageBoxDefaultButton.Button1)
				'Me.btnSetting_Click(Me.btnSetting, New System.EventArgs) ' LblSetting_Click(Me.LblSetting, e)
				Return False
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

		Try
			' Für die Ermittlung der ES-Ab und ES-Ende im LL nur für selektierte Datensätze
			'ClsDataDetail.SelectedListView = LvFoundedrecs
			'ClsDataDetail.SelectedListViewFailed = LvFoundedrecsFailed
			Return True

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

	End Function


	Private Sub frmLONLASearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And _ClsProgSetting.GetLogedUSNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub



#Region "Helpers"

	'Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

	'	Dim m_md As New SPProgUtility.Mandanten.Mandant
	'	Dim clsMandant = m_InitializationData.MDData
	'	Dim logedUserData = m_InitializationData.UserData
	'	Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

	'	Dim clsTransalation As New ClsTranslation
	'	Dim translate = clsTransalation.GetTranslationInObject

	'	Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	'End Function

	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.txt_NameBis.Visible = Me.SwitchButton1.Value
		Me.txt_NameBis.Text = String.Empty
	End Sub

	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_Translate.GetSafeTranslationValue("Keine Daten sind vorhanden")

		Try
			s = m_Translate.GetSafeTranslationValue(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub

	Private Function GetSortString(ByVal SortNumber As Integer?) As String
		Dim result As String = "Order By Nachname, Vorname"
		If Not SortNumber.HasValue Then Return result

		Select Case SortNumber

			Case 1
				result = "Order By Nachname, Vorname, MALand"

			Case 2
				result = "Order By MALand, Nachname, Vorname"


		End Select

		Return result

	End Function



	''' <summary>
	''' Sort view data.
	''' </summary>
	Class SortViewData

#Region "Public Consts"

		Public Const VALUE_Employee As Integer = 0
		Public Const VALUE_EmployeeCountry As Integer = 1
		Public Const VALUE_CountryEmployee As Integer = 2

#End Region

#Region "Public Properties"

		Public Property DisplayText As String
		Public Property Value As Integer

		Public ReadOnly Property IsEmployee
			Get
				Return Value = VALUE_Employee
			End Get
		End Property

		Public ReadOnly Property IsEmployeeCountry
			Get
				Return Value = VALUE_EmployeeCountry
			End Get
		End Property

		Public ReadOnly Property IsCountryEmployee
			Get
				Return Value = VALUE_CountryEmployee
			End Get
		End Property


#End Region

	End Class



#End Region


	Private Sub tgsSelection_Toggled(sender As Object, e As EventArgs) Handles tgsSelection.Toggled

		ChangeIsSelectedForAllRecords(Not m_ISAllRowsSelected)

		m_ISAllRowsSelected = Not m_ISAllRowsSelected
		'LibSelectAll.Text = GetSelectAllText(isAllRowsSelected)

	End Sub

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_DELETE_BUTTON As Int32 = 1

		' If delete button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_DELETE_BUTTON Then

			If TypeOf sender Is LookUpEdit Then
				Dim lookupEdit As LookUpEdit = CType(sender, LookUpEdit)
				lookupEdit.EditValue = Nothing
			ElseIf TypeOf sender Is DateEdit Then
				Dim dateEdit As DateEdit = CType(sender, DateEdit)
				dateEdit.EditValue = Nothing
			End If
		End If
	End Sub

	Private Sub checkedComboBoxEdit1_QueryPopUp(ByVal sender As Object, ByVal e As CancelEventArgs)
		Dim max As Integer = 0
		Dim g = sender.CreateGraphics()
		For i As Integer = 0 To sender.Properties.Items.Count - 1
			Dim w As SizeF = g.MeasureString(sender.Properties.Items(i).ToString(), Font)
			If CInt(w.Width) + 40 > max Then
				max = CInt(w.Width) + 40
			End If
		Next i

		sender.Properties.PopupFormSize = New Size(max, sender.Properties.PopupFormSize.Height)
	End Sub


End Class





