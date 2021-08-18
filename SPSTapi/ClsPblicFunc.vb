

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility
Imports SPProgUtility.Mandanten
Imports System.Collections.Generic
Imports System.IO

Public Class ClsDataDetail
  Inherits ClsMain_Net

  Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  'Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

#Region "Kunden_Setting..."

  Public Shared Function GetInterLineLen() As Short
    Dim _ClsReg As New SPProgUtility.ClsDivReg
    Return CShort(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\TapiDevice", _
                                             "InternLineLen").ToString))
  End Function

  Public Shared Function GetTapiLineID() As String
    Dim _ClsReg As New SPProgUtility.ClsDivReg
    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\TapiDevice", "TapiDeviceName")
  End Function

  Public Shared Function GetAmtsZiffer() As String
    Dim _ClsReg As New SPProgUtility.ClsDivReg
    Return _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\TapiDevice", "AmtsZiffer")
  End Function

  Public Shared Property GetMDNr As Integer

  '// MANr
  Shared _iMANr As Integer
  Public Shared Property GetMANr() As Integer
    Get
      Return _iMANr
    End Get
    Set(ByVal value As Integer)
      _iMANr = value
    End Set
  End Property

  '// KDNr
  Shared _iKDNr As Integer
  Public Shared Property GetKDNr() As Integer
    Get
      Return _iKDNr
    End Get
    Set(ByVal value As Integer)
      _iKDNr = value
    End Set
  End Property

  '// KDZhd
  Shared _iKDZhdNr As Integer
  Public Shared Property GetKDZhdNr() As Integer
    Get
      Return _iKDZhdNr
    End Get
    Set(ByVal value As Integer)
      _iKDZhdNr = value
    End Set
  End Property

  '// _iKontaktNr
  Shared _iKontaktNr As Integer
  Public Shared Property GetNewKontaktNr() As Integer
    Get
      Return _iKontaktNr
    End Get
    Set(ByVal value As Integer)
      _iKontaktNr = value
    End Set
  End Property

  '// _iTaskNr
  Shared _iTaskNr As Integer
  Public Shared Property GetNewTaskNr() As Integer
    Get
      Return _iTaskNr
    End Get
    Set(ByVal value As Integer)
      _iTaskNr = value
    End Set
  End Property

  '// RecNr
  Shared _iRecNr As Integer
  Public Shared Property GetRecNr() As Integer
    Get
      Return _iRecNr
    End Get
    Set(ByVal value As Integer)
      _iRecNr = value
    End Set
  End Property

  '// Firmenname
  Shared _strKDName As String
  Public Shared Property GetKDName() As String
    Get
      Return _strKDName
    End Get
    Set(ByVal value As String)
      _strKDName = value
    End Set
  End Property

  '// ModeNr
  Shared _iModeNr As Short
  Public Shared Property GetModeNr() As Short
    Get
      Return _iModeNr
    End Get
    Set(ByVal value As Short)
      _iModeNr = value
    End Set
  End Property

#End Region

#Region "Tapi Angaben..."

  '// DeviceID
  Shared _iDeviceID As Integer
  Public Shared Property Tapi_DeviceID() As Integer
    Get
      Return _iDeviceID
    End Get
    Set(ByVal value As Integer)
      _iDeviceID = value
    End Set
  End Property

  '// InternCall
  Shared _bIsInternCall As Boolean
  Public Shared Property Tapi_IsInternCall() As Boolean
    Get
      Return _bIsInternCall
    End Get
    Set(ByVal value As Boolean)
      _bIsInternCall = value
    End Set
  End Property

  '// CallHandle
  Shared _lCallHandle As Long
  Public Shared Property Tapi_CallHanld() As Long
    Get
      Return _lCallHandle
    End Get
    Set(ByVal value As Long)
      _lCallHandle = value
    End Set
  End Property

  '// CallerID
  Shared _lCallID As String
  Public Shared Property Tapi_CallID() As String
    Get
      Return _lCallID
    End Get
    Set(ByVal value As String)
      _lCallID = value
    End Set
  End Property

  '// CallerName
  Shared _strCallerName As String
  Public Shared Property Tapi_CallerName() As String
    Get
      Return _strCallerName
    End Get
    Set(ByVal value As String)
      _strCallerName = value
    End Set
  End Property

  '// Incoming / Outgoing
  Shared _bCallIn As Boolean
  Public Shared Property Tapi_CallIncoming() As Boolean
    Get
      Return _bCallIn
    End Get
    Set(ByVal value As Boolean)
      _bCallIn = value
    End Set
  End Property

#End Region




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




  ''' <summary>
  ''' Individuelle Bezeichnungen für Labels
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared ReadOnly Property ProsonalizedName() As Dictionary(Of String, ClsProsonalizedData)
    Get
      Try
        Dim m As New Mandant
				Return m.GetPersonalizedCaptionInObject(m_InitialData.MDData.MDNr)

      Catch ex As Exception
        Return Nothing
      End Try
    End Get
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

  Public Shared Function GetColumnTextStr(ByVal dr As SqlClient.SqlDataReader, _
                                        ByVal columnName As String, ByVal replacementOnNull As String) As String

    If Not dr.IsDBNull(dr.GetOrdinal(columnName)) Then
      If String.IsNullOrEmpty(CStr(dr(columnName))) Then
        Return replacementOnNull
      End If
      Return CStr(dr(columnName))
    End If

    Return replacementOnNull
  End Function


End Class
