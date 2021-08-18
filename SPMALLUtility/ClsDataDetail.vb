
Imports System.Windows.Forms

Public Class ClsDataDetail


  Public Shared bContentChanged As Boolean = False
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty

  Public Shared strFilename4Print_Save As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty

  Public Shared IsLVLarg As Boolean

  Public Shared Function GetAppGuidValue() As String
    Return "363028C8-74D4-4dbd-97A5-3A8BB207CC28"
  End Function



  '// Vorlage für Lebenslauf
  Shared _strTemplateName As String
  Public Shared Property GetSelectedTemplateName() As String
    Get
      Return _strTemplateName
    End Get
    Set(ByVal value As String)
      _strTemplateName = value
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

  '// Sorted from frmLL_LV
  Shared _strSortBez As String
  Public Shared Property GetLVSortBez() As String
    Get
      Return _strSortBez
    End Get
    Set(ByVal value As String)
      _strSortBez = value
    End Set
  End Property

  '// Selected MANumber
  Shared _MANumber As Integer
  Public Shared Property GetMANumber() As Integer
    Get
      Return _MANumber
    End Get
    Set(ByVal value As Integer)
      _MANumber = value
    End Set
  End Property


  '// SelectedNumbers
  Shared _SelectedNumbers As String = ""
  Public Shared Property GetSelectedNumbers() As String
    Get
      Return _SelectedNumbers
    End Get
    Set(ByVal value As String)
      _SelectedNumbers = value
    End Set
  End Property

  '// SelectedBez
  Shared _SelectedBez As String = ""
  Public Shared Property GetSelectedBez() As String
    Get
      Return _SelectedBez
    End Get
    Set(ByVal value As String)
      _SelectedBez = value
    End Set
  End Property

  '// Darf KST geändert werden?
  Shared _bAllowedChangeKST As Boolean
  Public Shared Property bAllowedTochangeKST() As Boolean
    Get
      Return _bAllowedChangeKST
    End Get
    Set(ByVal value As Boolean)
      _bAllowedChangeKST = value
    End Set
  End Property

End Class
