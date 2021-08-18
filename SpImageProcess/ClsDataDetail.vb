
Imports System.Windows.Forms

Public Class ClsDataDetail

  Public Shared strKDData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty

  Public Shared Get4What As String = String.Empty

  Public Shared GetSortBez As String = String.Empty
  Public Shared GetFilterBez As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty

  Public Shared IsLVLarg As Boolean
  Public Shared strNewFileName As String = String.Empty


  '// SQL-Query
  Shared _SQLString As String
  Public Shared Property SQLQuery() As String
    Get
      Return _SQLString
    End Get
    Set(ByVal value As String)
      _SQLString = value
    End Set
  End Property

  '// Ist als DTA?
  Shared _bAsDTA As Boolean
  Public Shared Property bAsDTA() As Boolean
    Get
      Return _bAsDTA
    End Get
    Set(ByVal value As Boolean)
      _bAsDTA = value
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

End Class
