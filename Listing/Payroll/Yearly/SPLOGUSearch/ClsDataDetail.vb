
Imports System.Windows.Forms
Imports System.Data.SqlClient

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility
Imports SPProgUtility.Mandanten
Imports System.IO


Public Class ClsDataDetail

	Public Shared strLOGUData As String = String.Empty
	Public Shared _strButtonValue As ButtonValue = ButtonValue.LOGU
	Public Shared _Get4What As What = What.MANR

	Public Shared GetSortBez As String = String.Empty
	Public Shared GetFilterBez As String = String.Empty
	Public Shared GetFilterBez2 As String = String.Empty
	Public Shared GetFilterBez3 As String = String.Empty
	Public Shared GetFilterBez4 As String = String.Empty


	''' <summary>
	''' searchcriteria
	''' </summary>
	''' <remarks></remarks>
	Public Shared SelectionCriteria As SearchCriteria

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Public Shared m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Public Shared m_InitialData As SP.Infrastructure.Initialization.InitializeClass

	Public Shared ReadOnly Property ChangeMandantData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass
		Get
			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Get
	End Property

	'Private Shared m_ModuleCache As SP.ModuleCaching.ModuleCache

	'Public Shared Function GetModuleCach() As SP.ModuleCaching.ModuleCache

	'	If m_ModuleCache Is Nothing Then
	'		m_ModuleCache = New SP.ModuleCaching.ModuleCache()
	'		m_ModuleCache.MaxCustomerFormsToCache = 2
	'		m_ModuleCache.MaxResponsiblePersonFormsToCache = 2
	'		m_ModuleCache.MaxEmployeeFormsToCache = 2
	'	End If

	'	Return m_ModuleCache

	'End Function






	Public Shared Function GetAppGuidValue() As String
		Return "7C3D9C79-8054-4292-98F3-24FCB5BEB4D1"
	End Function



	Structure ParamVar
		Dim MonatBis As String
		Dim JahrBis As String
		Dim MANR As String
	End Structure
	Public Shared Param As ParamVar


	Enum What As Integer
		MANR
	End Enum

	Enum ButtonValue As Integer
		LOGU
	End Enum

	Public Shared Property Get4What(Optional ByVal What As What = What.MANR) As What
		Get
			Return _Get4What
		End Get
		Set(ByVal value As What)
			_Get4What = What
		End Set
	End Property

	Public Shared Property StrButtonValue(Optional ByVal button As ButtonValue = ButtonValue.LOGU) As ButtonValue
		Get
			Return _strButtonValue
		End Get
		Set(ByVal value As ButtonValue)
			_strButtonValue = button
		End Set
	End Property

	' Checkboxes
	Structure Checkboxes
		Dim ChkFeierChecked As Boolean
		Dim ChkFerienChecked As Boolean
		Dim Chk13LohnChecked As Boolean
		Dim ChkDarelehenChecked As Boolean
		Dim ChkGleitstundenChecked As Boolean
		Dim ChkFeierVJChecked As Boolean
		Dim ChkFerienVJChecked As Boolean
		Dim Chk13LohnVJChecked As Boolean
	End Structure

	Public Shared CheckedCheckboxes As Checkboxes

	''' <summary>
	''' Gibt die maximale Höhe des Panels, wenn alle Controls sichtbar sind.
	''' </summary>
	''' <param name="pnl"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetPanelHeight(ByVal pnl As Panel) As Integer
		Dim height As Integer = 0
		Dim lowestPos As Integer = 0
		For Each con As Control In pnl.Controls
			If lowestPos < con.Location.Y + con.Height Then
				lowestPos = con.Location.Y + con.Height
			End If
		Next
		Return lowestPos + 0
	End Function

	''' <summary>
	''' Gibt an, ob ein Panel aufgeklappt ist oder nicht.
	''' </summary>
	''' <param name="pnl"></param>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared ReadOnly Property PanelExpanded(ByVal pnl As Panel) As Boolean
		Get
			If pnl.Height = GetPanelHeight(pnl) Then
				Return True
			End If
			Return False
		End Get
	End Property


	'// Tabellennamen der zu abspeichernde Liste
	Shared _lltablename As String
	Public Shared Property LLTablename() As String
		Get
			If _lltablename Is Nothing Then
				_lltablename = ""
			End If
			Return _lltablename
		End Get
		Set(ByVal value As String)
			_lltablename = value
		End Set
	End Property

	'// ModulToPrint für Drucken
	Shared _strModulToprint As String
	Public Shared Property GetModulToPrint() As String
		Get
			Return _strModulToprint
		End Get
		Set(ByVal value As String)
			_strModulToprint = value
		End Set
	End Property


	'// Sort for LV
	Shared _strSortBez As String
	Public Shared Property GetLVSortBez() As String
		Get
			Return _strSortBez
		End Get
		Set(ByVal value As String)
			_strSortBez = value
		End Set
	End Property

#Region "Parameter für LL-Variabeln"

	Enum SuchkriterienList As Integer
		MonatBis
		JahrBis
		MANR
	End Enum

	Structure SelectionItem
		Dim Bezeichnung As SuchkriterienList
		Dim Text As String
	End Structure

	'// Selektions-Container für die Anzeige der selektierten Kriterien
	Shared _SelectedContainer As ArrayList
	Public Shared ReadOnly Property SelectedContainer() As ArrayList
		Get
			If _SelectedContainer Is Nothing Then
				_SelectedContainer = New ArrayList
			End If
			Return _SelectedContainer
		End Get
	End Property

#End Region

	'// DataTable 
	Shared _SelectedDataTable As DataTable
	Public Shared Property SelectedDataTable() As DataTable
		Get
			If _SelectedDataTable Is Nothing Then
				_SelectedDataTable = New DataTable()
			End If
			Return _SelectedDataTable
		End Get
		Set(ByVal value As DataTable)
			_SelectedDataTable = value
		End Set
	End Property

	Public Shared AnzMax As Integer	' Für die Fortschrittsanzeige im LL

	''' <summary>
	''' Für die ProgressBar-Aufzählung ganzzahliger Prozent
	''' </summary>
	''' <param name="min"></param>
	''' <param name="max"></param>
	''' <param name="current"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetProzent(ByVal min As Integer, ByVal max As Integer, ByVal current As Integer) As Integer
		If current > max Then
			Return 100
		End If
		If max < 1 Then
			Return 0
		End If
		Return CInt((current / max) * 100)
	End Function
End Class
