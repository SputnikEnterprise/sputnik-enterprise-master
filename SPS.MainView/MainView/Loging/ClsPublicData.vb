
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility
Imports SPProgUtility.Mandanten
Imports System.Collections.Generic

Public Class ClsPublicData

  Public Shared MDData As New SPProgUtility.ClsMDData

#Region "Properties"


  ''' <summary>
  ''' Datenbankeinstellungen aus der \\Server\spenterprise$\bin\Programm.XML. gemäss Mandantennummer
  ''' </summary>
  ''' <param name="iMDNr"></param>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared ReadOnly Property SelectedMDData(ByVal iMDNr As Integer) As SPProgUtility.ClsMDData
    Get
      Dim m_md As New Mandant
      MDData = m_md.GetSelectedMDData(iMDNr)

      Return MDData
    End Get
  End Property


  Public Shared TranslationData As Dictionary(Of String, ClsTranslationData)

  Public Shared ReadOnly Property Translation() As Dictionary(Of String, ClsTranslationData)
    Get
      Try
        Dim m_Translation As New SPProgUtility.SPTranslation.ClsTranslation
        Return m_Translation.GetTranslationInObject

      Catch ex As Exception
        Return Nothing
      End Try
    End Get
  End Property

  '// DbSelectConn 
  Dim _DbSelectConn As String
  Public Property DbSelectConn() As String
    Get
      Return _DbSelectConn
    End Get
    Set(ByVal value As String)
      _DbSelectConn = value
    End Set
  End Property

  '// MDName (= MDName)
  Dim _MDName1 As String
  Public Property MDName1() As String
    Get
      Return _MDName1
    End Get
    Set(ByVal value As String)
      _MDName1 = value
    End Set
  End Property

  '// MDPath 
  Dim _MDPw4 As String
  Public Property MDPw4() As String
    Get
      Return _MDPw4
    End Get
    Set(ByVal value As String)
      _MDPw4 = value
    End Set
  End Property

  '// MDverzeichnis (= MDverzeichnis)
  Dim _MDPath As String
  Public Property MDPath() As String
    Get
      Return _MDPath
    End Get
    Set(ByVal value As String)
      _MDPath = value
    End Set
  End Property

  '// MDGuid = Customer_ID
  Dim _MDGuid As String
  Public Property MDGuid() As String
    Get
      Return _MDGuid
    End Get
    Set(ByVal value As String)
      _MDGuid = value
    End Set
  End Property

  '// MDNr (= MDNr)
  Dim _MDID As Integer
  Public Property MDID() As Integer
    Get
      Return _MDID
    End Get
    Set(ByVal value As Integer)
      _MDID = value
    End Set
  End Property

  '// MDNr (= MDNr)
  Dim _MDNr As Integer
  Public Property MDNr() As Integer
    Get
      Return _MDNr
    End Get
    Set(ByVal value As Integer)
      _MDNr = value
    End Set
  End Property

  Public Property SelectMDGroupNr() As String
  Public Property SelectedFileServerPath As String

  '// USNr (= USNr)
  Dim _USNr As Integer
  Public Property USNr() As Integer
    Get
      Return _USNr
    End Get
    Set(ByVal value As Integer)
      _USNr = value
    End Set
  End Property

  '// USVorname (= USVorname)
  Dim _USVorname As String
  Public Property USVorname() As String
    Get
      Return _USVorname
    End Get
    Set(ByVal value As String)
      _USVorname = value
    End Set
  End Property

  '// USNachname (= USNachname)
  Dim _USNachname As String
  Public Property USNachname() As String
    Get
      Return _USNachname
    End Get
    Set(ByVal value As String)
      _USNachname = value
    End Set
  End Property

  '// GetIP (= GetIP)
  Dim _GetIP As String
  Public Property GetMyIP() As String
    Get
      Return _GetIP
    End Get
    Set(ByVal value As String)
      _GetIP = value
    End Set
  End Property

#End Region

End Class
