
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices
Imports System.Xml

Public Class ClsDivFunc

#Region "Property für ein Auswahl-Fenster (wird nicht benötigt)"

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

#End Region

#Region "Funktionen für LvClick in der Suchmaske...(wird nicht benötigt)"

  ' // Allgemeiner Zwischenspeicher
  Dim _strSelektion As String
  Public Property GetSelektion() As String
    Get
      Return _strSelektion
    End Get
    Set(ByVal value As String)
      _strSelektion = value
    End Set
  End Property

  ' // ID
  Dim _strID As String
  Public Property GetID() As String
    Get
      Return _strID
    End Get
    Set(ByVal value As String)
      _strID = value
    End Set
  End Property

  '// MANr
  Dim _strMANr As String
  Public Property GetMANr() As String
    Get
      Return _strMANr
    End Get
    Set(ByVal value As String)
      _strMANr = value
    End Set
  End Property

  '// Kandidatenname
  Dim _strMAName As String
  Public Property GetMAName() As String
    Get
      Return _strMAName
    End Get
    Set(ByVal value As String)
      _strMAName = value
    End Set
  End Property

  '// Kandidatenvorname
  Dim _strMAVorname As String
  Public Property GetMAVorname() As String
    Get
      Return _strMAVorname
    End Get
    Set(ByVal value As String)
      _strMAVorname = value
    End Set
  End Property

  '// Kandidatenberuf
  Dim _strMABeruf As String
  Public Property GetMABeruf() As String
    Get
      Return _strMABeruf
    End Get
    Set(ByVal value As String)
      _strMABeruf = value
    End Set
  End Property

  '// QSTGemeinde
  Dim _strQSTGemeinde As String
  Public Property GetQSTGemeinde() As String
    Get
      Return _strQSTGemeinde
    End Get
    Set(ByVal value As String)
      _strQSTGemeinde = value
    End Set
  End Property

  '// Query.GetSearchQuery
  Dim _strTelNr As String
  Public Property GetTelNr() As String
    Get
      Return _strTelNr
    End Get
    Set(ByVal value As String)
      _strTelNr = value
    End Set
  End Property

  ' ''' <summary>
  ' ''' Allgemeine Funktion für Dropdownlistbox (auch myCbo), damit die Breite der Dropdownlist-Auswahl-Box
  ' ''' an den jeweiligen Text angepasst wird. Die Funktion gibt diese Breite zurück.
  ' ''' </summary>
  ' ''' <param name="comboBoxObject">Das ComboBox-Control übergeben</param>
  ' ''' <returns>Gibt die Breite (Width) für die DropDown-Box zurück.</returns>
  ' ''' <remarks></remarks>
  'Public Function GetComboBoxDropDownWidth(ByVal comboBoxObject As Object) As Integer

  '  Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = DirectCast(comboBoxObject, DevExpress.XtraEditors.ComboBoxEdit)
  '  Dim width As Integer = cbo.Properties.DropDownWidth
  '  Dim g As Graphics = cbo.CreateGraphics()
  '  Dim font As Font = cbo.Font
  '  Dim vertScrollBarWidth As Integer = CInt(IIf(cbo.Items.Count > cbo.MaxDropDownItems, SystemInformation.VerticalScrollBarWidth, 0))
  '  Dim newWidth As Integer
  '  If cbo.Items.Count > 0 Then
  '    If TypeOf (cbo.Items(0)) Is ComboBoxItem Then
  '      For Each item As ComboBoxItem In cbo.Items
  '        newWidth = CInt(g.MeasureString(item.Text, font).Width) + vertScrollBarWidth
  '        If width < newWidth Then
  '          width = newWidth
  '        End If
  '      Next
  '    Else
  '      For Each s As String In cbo.Items
  '        newWidth = CInt(g.MeasureString(s, font).Width) + vertScrollBarWidth
  '        If width < newWidth Then
  '          width = newWidth
  '        End If
  '      Next
  '    End If
  '  End If
  '  Return width
  'End Function

#End Region

#Region "LL_Properties (wird nicht benötigt)"
  '// Print.LLDocName
  Dim _LLDocName As String
  Public Property LLDocName() As String
    Get
      Return _LLDocName
    End Get
    Set(ByVal value As String)
      _LLDocName = value
    End Set
  End Property

  '// Print.LLDocLabel
  Dim _LLDocLabel As String
  Public Property LLDocLabel() As String
    Get
      Return _LLDocLabel
    End Get
    Set(ByVal value As String)
      _LLDocLabel = value
    End Set
  End Property

  '// Print.LLFontDesent
  Dim _LLFontDesent As Integer
  Public Property LLFontDesent() As Integer
    Get
      Return _LLFontDesent
    End Get
    Set(ByVal value As Integer)
      _LLFontDesent = value
    End Set
  End Property

  '// Print.LLIncPrv
  Dim _LLIncPrv As Integer
  Public Property LLIncPrv() As Integer
    Get
      Return _LLIncPrv
    End Get
    Set(ByVal value As Integer)
      _LLIncPrv = value
    End Set
  End Property

  '// Print.LLParamCheck
  Dim _LLParamCheck As Integer
  Public Property LLParamCheck() As Integer
    Get
      Return _LLParamCheck
    End Get
    Set(ByVal value As Integer)
      _LLParamCheck = value
    End Set
  End Property

  '// Print.LLKonvertName
  Dim _LLKonvertName As Integer
  Public Property LLKonvertName() As Integer
    Get
      Return _LLKonvertName
    End Get
    Set(ByVal value As Integer)
      _LLKonvertName = value
    End Set
  End Property

  '// Print.LLZoomProz
  Dim _LLZoomProz As Integer
  Public Property LLZoomProz() As Integer
    Get
      Return _LLZoomProz
    End Get
    Set(ByVal value As Integer)
      _LLZoomProz = value
    End Set
  End Property

  '// Print.LLCopyCount
  Dim _LLCopyCount As Integer
  Public Property LLCopyCount() As Integer
    Get
      Return _LLCopyCount
    End Get
    Set(ByVal value As Integer)
      _LLCopyCount = value
    End Set
  End Property

  '// Print.LLExportedFilePath
  Dim _LLExportedFilePath As String
  Public Property LLExportedFilePath() As String
    Get
      Return _LLExportedFilePath
    End Get
    Set(ByVal value As String)
      _LLExportedFilePath = value
    End Set
  End Property

  '// Print.LLExportedFileName
  Dim _LLExportedFileName As String
  Public Property LLExportedFileName() As String
    Get
      Return _LLExportedFileName
    End Get
    Set(ByVal value As String)
      _LLExportedFileName = value
    End Set
  End Property

  '// Print.LLExportfilter
  Dim _LLExportfilter As String
  Public Property LLExportfilter() As String
    Get
      Return _LLExportfilter
    End Get
    Set(ByVal value As String)
      _LLExportfilter = value
    End Set
  End Property

  '// Print.LLExporterName
  Dim _LLExporterName As String
  Public Property LLExporterName() As String
    Get
      Return _LLExporterName
    End Get
    Set(ByVal value As String)
      _LLExporterName = value
    End Set
  End Property

  '// Print.LLExporterFileName
  Dim _LLExporterFileName As String
  Public Property LLExporterFileName() As String
    Get
      Return _LLExporterFileName
    End Get
    Set(ByVal value As String)
      _LLExporterFileName = value
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

  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


#Region "Funktionen zur Suche nach Daten..."

  Sub New()

  End Sub



  Function GetLstItems(ByVal lst As ListBox) As String
    Dim strBerufItems As String = String.Empty

    For i = 0 To lst.Items.Count - 1
      strBerufItems += lst.Items(i).ToString & "#@"
    Next

    Return Left(strBerufItems, Len(strBerufItems) - 2)
  End Function

#End Region

End Class

Public Class ESData
	Public Property EmployeeFirstName As String
	Public Property EmployeeLastName As String
	Public Property EmployeeGebDate As Date?
	Public Property EmployeeAlter As Integer?

	Public Property EmployeeBewilligung As String
	Public Property EmployeeBewilligungDate As Date?
	Public Property EmployeeBeruf As String
	Public Property EmployeeBerufLand As String

	Public Property CustomerName As String
	Public Property CustomerPLZ As String

	Public ReadOnly Property EmployeeFullName As String
		Get
			Return String.Format("{0} {1}", EmployeeFirstName, EmployeeLastName)
		End Get
	End Property

End Class



Module MyComboBoxExtensions
  <Extension()> _
 _
  Public Function ToItem(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit) As ComboBoxItem
    If TypeOf (cbo.SelectedItem) Is ComboBoxItem And cbo.SelectedIndex > -1 Then
      Return DirectCast(cbo.Properties.Items(cbo.SelectedIndex), ComboBoxItem)
    ElseIf cbo.SelectedIndex > -1 Then
      Dim item As New ComboBoxItem("", "")
      item.Text = cbo.Properties.Items(cbo.SelectedIndex).ToString
      item.Value_0 = item.Text
      Return item
    Else
      Dim item As New ComboBoxItem("", "")
      item.Text = cbo.Text
      item.Value_0 = cbo.Text
      Return item
    End If
  End Function

End Module

