
Imports System.Data.SqlClient

Public Class ClsDataDetail

  Public Shared strValueData As String
  Public Shared strButtonValue As String

  Public Shared Get4What As String

  Public Shared GetSortBez As String
  Public Shared GetFilterBez As String
  Public Shared GetFilterBez2 As String
  Public Shared GetFilterBez3 As String
  Public Shared GetFilterBez4 As String



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





  Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "335c767b-2873-4c64-b53f-e338c054f836"
    End Get
  End Property

	' Helps extracting a column value form a data reader.
  Public Shared Function GetColumnTextStr(ByVal dr As SqlDataReader, _
                                          ByVal columnName As String, ByVal replacementOnNull As String) As String

    If Not dr.IsDBNull(dr.GetOrdinal(columnName)) Then
      Return CStr(dr(columnName))
    End If

    Return replacementOnNull
  End Function

  ' Helps extracting a column value form a data reader.
  Public Shared Function GetColumnTextStr(ByVal dataRow As DataRow, _
                                          ByVal columnName As String, ByVal replacementOnNull As String) As String

    If Not dataRow.IsNull(columnName) Then Return CStr(dataRow(columnName))

    Return replacementOnNull
  End Function



  '// Query für Datensuche
  Shared _strSQLString As String
  Public Shared Property GetSQLQuery() As String
    Get
      Return _strSQLString
    End Get
    Set(ByVal value As String)
      _strSQLString = value
    End Set
  End Property

  '// Query für NUR Sortierung der Daten
  Shared _strSortString As String
  Public Shared Property GetSQLSortString() As String
    Get
      Return _strSortString
    End Get
    Set(ByVal value As String)
      _strSortString = value
    End Set
  End Property

  '// Query für NUR Sortierung der Ausgabe
  Shared _strSortString_2 As String
  Public Shared Property GetSQLSortString_2() As String
    Get
      Return _strSortString_2
    End Get
    Set(ByVal value As String)
      _strSortString_2 = value
    End Set
  End Property




End Class
