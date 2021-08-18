
Imports SPProgUtility

Public Class ClsESSetting

  Public Property DbConnString2Open As String

  Public Property SelectedYear As List(Of Integer)
  Public Property SelectedMonth As List(Of Short)

  'Public Property SelectedKDNr As List(Of Integer)
  'Public Property SelectedMANr As List(Of Integer)
  Public Property SelectedESNr As List(Of Integer)

  Public Property SelectedKanton As String
  Public Property SelectedPVLBez As String

  ''' <summary>
  ''' 0 = Ohne WOS Kandidaten
  ''' 1 = Nur WOS Kandidaten
  ''' 2 = Alle suchen
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SelectedWOSProperty As Short

  Public Property MetroForeColor As System.Drawing.Color
  Public Property MetroBorderColor As System.Drawing.Color
  Public Property SearchAutomatic As Boolean
  Public Property FormOpenArt As OpenFormArt

  Public Property PrintESVertrag As Boolean
  Public Property PrintVerleihvertrag As Boolean
	Public Property ShowDesign As Boolean?

	Public Property JobNr2Print As String
	Public MDData As New SPProgUtility.ClsMDData
	Public UserData As New SPProgUtility.ClsUserData

  Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public TranslationData As Dictionary(Of String, ClsTranslationData)
  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)


  Enum OpenFormArt
    PrintingES
    DeleteingES
  End Enum

End Class
