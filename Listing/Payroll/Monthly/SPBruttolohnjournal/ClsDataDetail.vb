
Imports System.Windows.Forms
Imports System.Data.SqlClient



Public Class ClsDataDetail

  Public Shared strBruttolohnjournalData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty
	'Public Shared frmBruttolohnjournal As Form

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

	Public Shared Function GetAppGuidValue() As String
    Return "ADACC15D-A63E-4dce-948C-1BC7876D792C"
  End Function

	''// Query für Datensuche
	'Shared _strConnString As String
	'Public Shared Property GetDbConnString() As String
	'  Get
	'    Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath ' ClsMain_Net
	'    Dim _strConnString As String = _ClsProgSetting.GetConnString()

	'    Return (_strConnString)
	'  End Get
	'  Set(ByVal value As String)
	'    _strConnString = value
	'  End Set
	'End Property

	''// Query für Datensuche
	'Shared _strRootConnString As String
	'Public Shared Property GetDbRootConnString() As String
	'  Get
	'    Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath ' ClsMain_Net
	'    Dim _strRootConnString As String = _ClsProgSetting.GetDbSelectConnString()

	'    Return _strRootConnString
	'  End Get
	'  Set(ByVal value As String)
	'    _strRootConnString = value
	'  End Set
	'End Property

  '// Tabellennamen der Quelle (Bruttolohnjournal)
  Shared _tablenameSource As String
  Public Shared Property TablenameSource() As String
    Get
      Return _tablenameSource
    End Get
    Set(ByVal value As String)
      _tablenameSource = value
    End Set
  End Property

  '// Tabellennamen der Bruttolohnjournal-Liste
  Shared _tablenameBLJListe As String
  Public Shared Property TablenameBLJListe() As String
    Get
      Return _tablenameBLJListe
    End Get
    Set(ByVal value As String)
      _tablenameBLJListe = value
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

  '// Sorted from frmBruttolohnjournal_LV
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

  '// SQLCommand für ListView
  Shared _sqlCommand_LV As SqlCommand
  Public Shared Property SQLCommand_LV() As SqlCommand
    Get
      Return _sqlCommand_LV
    End Get
    Set(ByVal value As SqlCommand)
      _sqlCommand_LV = value
    End Set
  End Property

  '// Parameter für ListView
  Shared _sqlBLJParameter_LV As ClsDivFunc.BLJParameter
  Public Shared Property BLJParameter_LV() As ClsDivFunc.BLJParameter
    Get
      Return _sqlBLJParameter_LV
    End Get
    Set(ByVal value As ClsDivFunc.BLJParameter)
      _sqlBLJParameter_LV = value
    End Set
  End Property

End Class
