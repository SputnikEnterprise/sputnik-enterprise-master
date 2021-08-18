
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.Logging


Public Class ClsDivFunc

#Region "Diverses"

  Public Property GetSelektion() As String

  '// Get4What._strModul4What
  Dim _strModul4What As String
  Public Property Get4What() As String
    Get
      Return _strModul4What
    End Get
    Set(ByVal value As String)
      _strModul4What = value
    End Set
  End Property

  '// Query.GetSearchQuery
  Dim _strQuery As String
  Public Property GetSearchQuery() As String
    Get
      Return _strQuery
    End Get
    Set(ByVal value As String)
      _strQuery = value
    End Set
  End Property

  '// LargerLV
  Dim _bLargerLV As Boolean
  Public Property GetLargerLV() As Boolean
    Get
      Return _bLargerLV
    End Get
    Set(ByVal value As Boolean)
      _bLargerLV = value
    End Set
  End Property


#End Region


#Region "US Setting"

  '// USeMail (= eMail des Personalvermittlers)
  Dim _USeMail As String
  Public Property USeMail() As String
    Get
      Return _USeMail
    End Get
    Set(ByVal value As String)
      _USeMail = value
    End Set
  End Property

  '// USTelefon (= USTelefon des Personalvermittlers)
  Dim _USTelefon As String
  Public Property USTelefon() As String
    Get
      Return _USTelefon
    End Get
    Set(ByVal value As String)
      _USTelefon = value
    End Set
  End Property

  '// USTelefax (= USTelefax des Personalvermittlers)
  Dim _USTelefax As String
  Public Property USTelefax() As String
    Get
      Return _USTelefax
    End Get
    Set(ByVal value As String)
      _USTelefax = value
    End Set
  End Property

  '// USVorname (= USVorname des Personalvermittlers)
  Dim _USVorname As String
  Public Property USVorname() As String
    Get
      Return _USVorname
    End Get
    Set(ByVal value As String)
      _USVorname = value
    End Set
  End Property

  '// USAnrede (= USAnrede des Personalvermittlers)
  Dim _USAnrede As String
  Public Property USAnrede() As String
    Get
      Return _USAnrede
    End Get
    Set(ByVal value As String)
      _USAnrede = value
    End Set
  End Property

  '// USNachname (= USNachname des Personalvermittlers)
  Dim _USNachname As String
  Public Property USNachname() As String
    Get
      Return _USNachname
    End Get
    Set(ByVal value As String)
      _USNachname = value
    End Set
  End Property

  '// USMDName (= MDName des Personalvermittlers)
  Dim _USMDname As String
  Public Property USMDname() As String
    Get
      Return _USMDname
    End Get
    Set(ByVal value As String)
      _USMDname = value
    End Set
  End Property

  '// MDName2 (= MDName2 des Personalvermittlers)
  Dim _USMDname2 As String
  Public Property USMDname2() As String
    Get
      Return _USMDname2
    End Get
    Set(ByVal value As String)
      _USMDname2 = value
    End Set
  End Property

  '// MDName3 (= MDName3 des Personalvermittlers)
  Dim _USMDname3 As String
  Public Property USMDname3() As String
    Get
      Return _USMDname3
    End Get
    Set(ByVal value As String)
      _USMDname3 = value
    End Set
  End Property

  '// USMDPostfach (= MDPostfach des Personalvermittlers)
  Dim _USMDPostfach As String
  Public Property USMDPostfach() As String
    Get
      Return _USMDPostfach
    End Get
    Set(ByVal value As String)
      _USMDPostfach = value
    End Set
  End Property

  '// USMDStrasse (= MDstrasse des Personalvermittlers)
  Dim _USMDStrasse As String
  Public Property USMDStrasse() As String
    Get
      Return _USMDStrasse
    End Get
    Set(ByVal value As String)
      _USMDStrasse = value
    End Set
  End Property

  '// USMDOrt (= MDOrt des Personalvermittlers)
  Dim _USMDOrt As String
  Public Property USMDOrt() As String
    Get
      Return _USMDOrt
    End Get
    Set(ByVal value As String)
      _USMDOrt = value
    End Set
  End Property

  '// USMDPLZ (= MDPLZ des Personalvermittlers)
  Dim _USMDPlz As String
  Public Property USMDPlz() As String
    Get
      Return _USMDPlz
    End Get
    Set(ByVal value As String)
      _USMDPlz = value
    End Set
  End Property

  '// USMDLand (= MDLand des Personalvermittlers)
  Dim _USMDLand As String
  Public Property USMDLand() As String
    Get
      Return _USMDLand
    End Get
    Set(ByVal value As String)
      _USMDLand = value
    End Set
  End Property

  '// USMDTelefon (= MDTelefon des Personalvermittlers)
  Dim _USMDTelefon As String
  Public Property USMDTelefon() As String
    Get
      Return _USMDTelefon
    End Get
    Set(ByVal value As String)
      _USMDTelefon = value
    End Set
  End Property

  '// USMDTelefax (= MDTelefax des Personalvermittlers)
  Dim _USMDTelefax As String
  Public Property USMDTelefax() As String
    Get
      Return _USMDTelefax
    End Get
    Set(ByVal value As String)
      _USMDTelefax = value
    End Set
  End Property

  '// USMDeMail (= MDeMail des Personalvermittlers)
  Dim _USMDeMail As String
  Public Property USMDeMail() As String
    Get
      Return _USMDeMail
    End Get
    Set(ByVal value As String)
      _USMDeMail = value
    End Set
  End Property

  '// USMDHomepage (= MDHomepage des Personalvermittlers)
  Dim _USMDHomepage As String
  Public Property USMDHomepage() As String
    Get
      Return _USMDHomepage
    End Get
    Set(ByVal value As String)
      _USMDHomepage = value
    End Set
  End Property

#End Region



End Class

Public Class ClsDbFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private m_md As Mandant


	''' <summary>
	''' listet eine Auflistung der Mandantendaten
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function LoadMandantenData() As IEnumerable(Of MandantenData)
		Dim m_utility As New Utilities
		Dim result As List(Of MandantenData) = Nothing
		m_md = New Mandant

		Dim sql As String = "[Mandanten. Get All Allowed MDData]"

		Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.GetSelectedMDConnstring, sql, Nothing, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of MandantenData)

				While reader.Read()
					Dim recData As New MandantenData

					recData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
					recData.MDName = m_utility.SafeGetString(reader, "MDName")
					recData.MDGuid = m_utility.SafeGetString(reader, "MDGuid")
					recData.MDConnStr = m_md.GetSelectedMDData(recData.MDNr).MDDbConn

					result.Add(recData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function


#Region "Funktionen für Speichern der Daten..."

	'// RecNr
	Dim _AddressRecNr As Integer
	Public Property AddressRecNr() As Integer
		Get
			Return _AddressRecNr
		End Get
		Set(ByVal value As Integer)
			_AddressRecNr = value
		End Set
	End Property

	'// Modulnr 
	Dim _ModulNr As Short
	Public Property Add_ModulNr() As Short
		Get
			Return _ModulNr
		End Get
		Set(ByVal value As Short)
			_ModulNr = value
		End Set
	End Property

	'// MANr
	Dim _MANr As Integer
	Public Property Add_MANr() As Integer
		Get
			Return _MANr
		End Get
		Set(ByVal value As Integer)
			_MANr = value
		End Set
	End Property

	'// Geschlecht
	Dim _Geschlecht As String
	Public Property Add_Geschlecht() As String
		Get
			Return _Geschlecht
		End Get
		Set(ByVal value As String)
			_Geschlecht = value
		End Set
	End Property
	'// Nachname
	Dim _Nachname As String
	Public Property Add_Nachname() As String
		Get
			Return _Nachname
		End Get
		Set(ByVal value As String)
			_Nachname = value
		End Set
	End Property
	'// Vorname 
	Dim _Vorname As String
	Public Property Add_Vorname() As String
		Get
			Return _Vorname
		End Get
		Set(ByVal value As String)
			_Vorname = value
		End Set
	End Property
	'// Zusatz
	Dim _Zusatz As String
	Public Property Add_Zusatz() As String
		Get
			Return _Zusatz
		End Get
		Set(ByVal value As String)
			_Zusatz = value
		End Set
	End Property

	'// Postfach (= Postfach)
	Dim _Postfach As String
	Public Property Add_Postfach() As String
		Get
			Return _Postfach
		End Get
		Set(ByVal value As String)
			_Postfach = value
		End Set
	End Property
	'// Strasse (= Strasse)
	Dim _Strasse As String
	Public Property Add_Strasse() As String
		Get
			Return _Strasse
		End Get
		Set(ByVal value As String)
			_Strasse = value
		End Set
	End Property
	'// Land (= Land)
	Dim _Land As String
	Public Property Add_Land() As String
		Get
			Return _Land
		End Get
		Set(ByVal value As String)
			_Land = value
		End Set
	End Property
	'// PLZ (= PLZ)
	Dim _Plz As String
	Public Property Add_Plz() As String
		Get
			Return _Plz
		End Get
		Set(ByVal value As String)
			_Plz = value
		End Set
	End Property
	'// Ort (= Ort)
	Dim _Ort As String
	Public Property Add_Ort() As String
		Get
			Return _Ort
		End Get
		Set(ByVal value As String)
			_Ort = value
		End Set
	End Property

	'// Bemerkung 
	Dim _Bemerkung As String
	Public Property Add_Bemerkung() As String
		Get
			Return _Bemerkung
		End Get
		Set(ByVal value As String)
			_Bemerkung = value
		End Set
	End Property
	'// Add_Res1 
	Dim _Add_Res1 As String
	Public Property Add_Res1() As String
		Get
			Return _Add_Res1
		End Get
		Set(ByVal value As String)
			_Add_Res1 = value
		End Set
	End Property
	'// Add_Res2 
	Dim _Add_Res2 As String
	Public Property Add_Res2() As String
		Get
			Return _Add_Res2
		End Get
		Set(ByVal value As String)
			_Add_Res2 = value
		End Set
	End Property
	'// Add_Res3 
	Dim _Add_Res3 As String
	Public Property Add_Res3() As String
		Get
			Return _Add_Res3
		End Get
		Set(ByVal value As String)
			_Add_Res3 = value
		End Set
	End Property

	'// ActivRec
	Dim _ActivRecNr As Boolean
	Public Property Add_ActivRec() As Boolean
		Get
			Return _ActivRecNr
		End Get
		Set(ByVal value As Boolean)
			_ActivRecNr = value
		End Set
	End Property

#End Region


#Region "Funktionen zur Suche nach Daten..."


	Function GetLstItems(ByVal lst As ListBox) As String
		Dim strBerufItems As String = String.Empty

		For i = 0 To lst.Items.Count - 1
			strBerufItems += lst.Items(i).ToString & "#@"
		Next

		Return Left(strBerufItems, Len(strBerufItems) - 2)
	End Function

#End Region


End Class