
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility
Imports SPProgUtility.Mandanten


Public Class ClsDataDetail

  Public Shared strESData As String = String.Empty

  Public Shared strKDData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty

  Public Shared GetSortBez As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty
  Public Shared GetFilterBezArray As ArrayList = New ArrayList()
  ' Die Faktura-Art-Filterbezeichnung wird nach dem Suchen festgelegt. Kann nicht in der Array direkt hinzugefügt werden,
  ' da mehrere unterschiedliche Listen mit unterschiedliche Einschränkungen gedruckt werden.
  Public Shared FakturaArtFilterBez As String = String.Empty


	'Public Shared TranslationData As Dictionary(Of String, ClsTranslationData)
	'Public Shared ProsonalizedData As Dictionary(Of String, ClsProsonalizedData)
	'Public Shared ProgSettingData As ClsSetting

	'Public Shared MDData As New ClsMDData
	'Public Shared UserData As New ClsUserData

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


	'''' <summary>
	'''' alle übersetzte Elemente in 3 Sprachen
	'''' </summary>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Public Shared ReadOnly Property Translation() As Dictionary(Of String, ClsTranslationData)
	'   Get
	'     Try
	'       Dim m_Translation As New SPProgUtility.SPTranslation.ClsTranslation
	'       Return m_Translation.GetTranslationInObject

	'     Catch ex As Exception
	'       Return Nothing
	'     End Try
	'   End Get
	' End Property

	' ''' <summary>
	' ''' Individuelle Bezeichnungen für Labels
	' ''' </summary>
	' ''' <value></value>
	' ''' <returns></returns>
	' ''' <remarks></remarks>
	' Public Shared ReadOnly Property ProsonalizedName() As Dictionary(Of String, ClsProsonalizedData)
	'   Get
	'     Try
	'       Dim m As New Mandant
	'       Return m.GetPersonalizedCaptionInObject(ProgSettingData.SelectedMDNr)

	'     Catch ex As Exception
	'       Return Nothing
	'     End Try
	'   End Get
	' End Property

	' ''' <summary>
	' ''' Datenbankeinstellungen für [Sputnik DBSelect]
	' ''' </summary>
	' ''' <value></value>
	' ''' <returns></returns>
	' ''' <remarks></remarks>
	' Public Shared ReadOnly Property SelectedDbSelectData() As ClsDbSelectData
	'   Get
	'     Dim m_Progpath As New ClsProgPath

	'     Return m_Progpath.GetDbSelectData()
	'   End Get
	' End Property

	' ''' <summary>
	' ''' Datenbankeinstellungen aus der \\Server\spenterprise$\bin\Programm.XML. gemäss Mandantennummer
	' ''' </summary>
	' ''' <param name="iMDNr"></param>
	' ''' <value></value>
	' ''' <returns></returns>
	' ''' <remarks></remarks>
	' Public Shared ReadOnly Property SelectedMDData(ByVal iMDNr As Integer) As ClsMDData
	'   Get
	'     Dim m_md As New Mandant
	'     MDData = m_md.GetSelectedMDData(iMDNr) 'ProgSettingData.SelectedMDNr)
	'     ProgSettingData.SelectedMDNr = MDData.MDNr
	'     GetSelectedMDConnstring = MDData.MDDbConn

	'     Return MDData
	'   End Get
	' End Property

	' ''' <summary>
	' ''' Benutzerdaten aus der Datenbank
	' ''' </summary>
	' ''' <param name="iMDNr"></param>
	' ''' <param name="iUserNr"></param>
	' ''' <value></value>
	' ''' <returns></returns>
	' ''' <remarks></remarks>
	' Public Shared ReadOnly Property LogededUSData(ByVal iMDNr As Integer, ByVal iUserNr As Integer) As ClsUserData
	'   Get
	'     Dim m_md As New Mandant
	'     UserData = m_md.GetSelectedUserData(iMDNr, iUserNr) 'ProgSettingData.SelectedMDNr, ProgSettingData.LogedUSNr)
	'     ProgSettingData.LogedUSNr = UserData.UserNr

	'     Return UserData
	'   End Get
	' End Property

	' ''' <summary>
	' ''' Benutzerdaten aus der Datenbank
	' ''' </summary>
	' ''' <param name="iMDNr"></param>
	' ''' <param name="strLastname"></param>
	' ''' <param name="strFirstname"></param>
	' ''' <value></value>
	' ''' <returns></returns>
	' ''' <remarks></remarks>
	' Public Shared ReadOnly Property LogededUSData(ByVal iMDNr As Integer, ByVal strLastname As String, ByVal strFirstname As String) As ClsUserData
	'   Get
	'     Dim m_md As New Mandant
	'     UserData = m_md.GetSelectedUserDataWithUserName(iMDNr, strLastname, strFirstname)
	'     ProgSettingData.LogedUSNr = UserData.UserNr

	'     Return UserData
	'   End Get
	' End Property





	Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return ("BAFCF91A-CE90-4b8f-896C-E3731A90D253")
    End Get
  End Property



  '// Listenbezeichnung zum Drucken
  Shared _strListBez As String
  Public Shared Property ListBez() As String
    Get
      Return _strListBez
    End Get
    Set(ByVal value As String)
      _strListBez = value
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


  '// Totalbetrag
  Shared _dTotalBetrag As Double
  Public Shared Property GetTotalBetrag() As Double
    Get
      Return _dTotalBetrag
    End Get
    Set(ByVal value As Double)
      _dTotalBetrag = value

    End Set
  End Property

  '// Total offener Betrag per Datum
  Shared _dTotalOpenBetrag As Double
  Public Shared Property GetTotalOpenBetrag4Date() As Double
    Get
      Return _dTotalOpenBetrag
    End Get
    Set(ByVal value As Double)
      _dTotalOpenBetrag = value

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

  '// Print.LLDocName
  Shared _LLDocName As String
  Public Shared Property LLDocName() As String
    Get
      Return _LLDocName
    End Get
    Set(ByVal value As String)
      _LLDocName = value
    End Set
  End Property

  '// Tabellennamen der Zahlungeingangsliste auf der DB
  Shared _SPTabNamenZEListe As String
  Public Shared Property SPTabNamenZEListe() As String
    Get
      Return _SPTabNamenZEListe
    End Get
    Set(ByVal value As String)
      _SPTabNamenZEListe = value
    End Set
  End Property

  '// Die Datensätze für die Anzeige auf dem LV und speichern auf der DB
  Shared _dtZE As DataTable
  Public Shared Property DtZE() As DataTable
    Get
      If _dtZE Is Nothing Then
        _dtZE = New DataTable("Zahlungeingangsliste")
      End If
      Return _dtZE
    End Get
    Set(ByVal value As DataTable)
      _dtZE = value
    End Set
  End Property

  '// Wurden Datensätze für die Listview gefunden?
  'Shared _bRecsFound As Boolean
  'Public Shared Property RecsFound() As Boolean
  '  Get
  '    Return _bRecsFound
  '  End Get
  '  Set(ByVal value As Boolean)
  '    _bRecsFound = value
  '  End Set
  'End Property

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

