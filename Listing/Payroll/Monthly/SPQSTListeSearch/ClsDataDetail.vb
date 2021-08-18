
Imports System.Windows.Forms
Imports SPProgUtility
Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten

Public Class ClsDataDetail

	Public Shared strQSTListeData As String = String.Empty
	Public Shared strButtonValue As String = String.Empty

	Public Shared Get4What As String = String.Empty
	Public Shared frmQSTListe As Form

	Public Shared GetSortBez As String = String.Empty
	Public Shared GetFilterBez As String = String.Empty
	Public Shared GetFilterBez2 As String = String.Empty
	Public Shared GetFilterBez3 As String = String.Empty
	Public Shared GetFilterBez4 As String = String.Empty


	Public Shared SelPeriodeVon As DateTime
	Public Shared SelPeriodeBis As DateTime



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


	'Public Shared ReadOnly Property IsModernUIAllowed() As Boolean
	'	Get
	'		Dim strfile2Validate As String = "C:\Path\ModernView.dat"

	'		Return True	' (File.Exists(strfile2Validate))

	'	End Get
	'End Property

	Public Shared ReadOnly Property GetAppGuidValue() As String
		Get
			Return "23EC6013-BBB0-45ff-B72B-5940FA738A6B"
		End Get
	End Property


	'// Tabellennamen
	Shared _LLTabellennamen As String
	Public Shared Property LLTabellennamen() As String
		Get
			Return _LLTabellennamen
		End Get
		Set(ByVal value As String)
			_LLTabellennamen = value
		End Set
	End Property


	'// Vorjahreslohnsumme für GetJahreslohnsumme()
	Shared _LLSelektionText As String
	Public Shared Property LLSelektionText() As String
		Get
			Return _LLSelektionText
		End Get
		Set(ByVal value As String)
			_LLSelektionText = value
		End Set
	End Property


	'// Kanton angeben, für die Quellensteuer-Adresse
	Shared _LLQSTAdresseKanton As String
	Public Shared Property LLQSTAdresseKanton() As String
		Get
			Return _LLQSTAdresseKanton
		End Get
		Set(ByVal value As String)
			_LLQSTAdresseKanton = value
		End Set
	End Property

	'// Gemeinde angeben, für die Quellensteuer-Adresse
	Shared _LLQSTAdresseGemeinde As String
	Public Shared Property LLQSTAdresseGemeinde() As String
		Get
			Return _LLQSTAdresseGemeinde
		End Get
		Set(ByVal value As String)
			_LLQSTAdresseGemeinde = value
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

	'// Tapi_Called
	Shared _bFirstCall As Boolean
	Public Shared Property IsFirstTapiCall() As Boolean
		Get
			Return _bFirstCall
		End Get
		Set(ByVal value As Boolean)
			_bFirstCall = value
		End Set
	End Property

	'// Sorted from frmQSTListeSearch_LV
	Shared _strSortBez As String
	Public Shared Property GetLVSortBez() As String
		Get
			Return _strSortBez
		End Get
		Set(ByVal value As String)
			_strSortBez = value
		End Set
	End Property

	Shared _arbeitgeberPauschale As Boolean
	Public Shared Property CheckArbeitgeberPauschale() As Boolean
		Get
			Return _arbeitgeberPauschale
		End Get
		Set(ByVal value As Boolean)
			_arbeitgeberPauschale = value
		End Set
	End Property

	'// DataTable QSTListe
	Shared _dtQSTListe As DataTable
	Public Shared Property QSTListeDataTable() As DataTable
		Get
			Return _dtQSTListe
		End Get
		Set(ByVal value As DataTable)
			_dtQSTListe = value
		End Set
	End Property

	Public Shared Function GetBasiskategorie(ByVal bewilligungsCode As String) As Integer
		Select Case bewilligungsCode
			Case "A" : Return 1	' Saisonarbeiter
			Case "B" : Return 2	' Aufenthalter
			Case "C" : Return 3	'Niedergelassene
			Case "Ci" : Return 4 ' Erwerbstätige Ehepartner / Angehörigen ausländischer Vertretungen oder staatlichen internationalen Organisationen
			Case "F" : Return 5	' Vorläufig Aufgenommener
			Case "G" : Return 6	' Grenzgänger
			Case "L" : Return 7	' Kurzaufenthalter
			Case "N" : Return 8	' Asylsuchender
			Case "S" : Return 9	' Schutzbedürftiger
			Case "M" : Return 10 ' Meldepflichtiger (M ist keine offizielle ausgestellte Ausländerkategorie)
				' 11 Diplomat und internationaler Funktionär mit diplomatische Immunität
				' 12 Internationaler Funktionär ohne diplomatische Immunität
			Case Else : Return 13	' Nicht zugeteilt
		End Select
	End Function

End Class

Public Class ComboValue
	Private _Bez As String
	Private _Value As String

	Public Sub New(ByVal _Text2Show As String, ByVal _Value2Save As String)
		_Bez = _Text2Show
		_Value = _Value2Save
	End Sub

	Public Function ComboValue() As String
		Return _Value
	End Function

	Public Function Text() As String
		Return _Bez
	End Function

	Public Overrides Function ToString() As String
		Return _Bez
	End Function

End Class
