
Imports System.Windows.Forms


Public Class ClsDataDetail



  Public Shared Function GetAppGuidValue() As String
    Return "1FB9E334-BBFE-4667-BACE-8C23C9AB5863"
  End Function



  '// Wert welche zurückgegeben wird...
  Shared _strReturnValue As String
  Public Shared Property GetReturnValue() As String
    Get
      Return _strReturnValue
    End Get
    Set(ByVal value As String)
      _strReturnValue = value
    End Set
  End Property

  '// Tapi_Called
  Shared _bAllowedMultiSelect As Boolean
  Public Shared Property AllowedMultiSelect() As Boolean
    Get
      Return _bAllowedMultiSelect
    End Get
    Set(ByVal value As Boolean)
      _bAllowedMultiSelect = value
    End Set
  End Property

  '// Sorted from frmkdsearch_LV
  Shared _strSortBez As String
  Public Shared Property GetLVSortBez() As String
    Get
      Return _strSortBez
    End Get
    Set(ByVal value As String)
      _strSortBez = value
    End Set
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

  '// Die Sortierung der gespeicherte Tabelle für die Kontaktliste
  Shared _llOrderByStringKontakliste As String
  Public Shared Property LLOrderByStringKontaktliste() As String
    Get
      If _llOrderByStringKontakliste Is Nothing Then
        _llOrderByStringKontakliste = ""
      End If
      Return _llOrderByStringKontakliste
    End Get
    Set(ByVal value As String)
      _llOrderByStringKontakliste = value
    End Set
  End Property
End Class
