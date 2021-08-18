Imports System.Text
Imports System.Linq

Public Class ClsExtendedResult

  ''' <summary>
  ''' Constructor
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub New()
    m_resultList = New List(Of ClsExtendedResultInfo)
  End Sub

  Public Sub AddMessage(message As String)
    m_resultList.Add(New ClsExtendedResultInfo(message, EnumInfoType.Message))
  End Sub

  Public Sub AddMessage(format As String, ParamArray args() As Object)
    ResultList.Add(New ClsExtendedResultInfo(String.Format(format, args), EnumInfoType.Message))
  End Sub

  Public Sub AddWarning(message As String)
    m_resultList.Add(New ClsExtendedResultInfo(message, EnumInfoType.Warning))
  End Sub

  Public Sub AddWarning(format As String, ParamArray args() As Object)
    ResultList.Add(New ClsExtendedResultInfo(String.Format(format, args), EnumInfoType.Warning))
  End Sub

  Public Sub AddError(message As String)
    m_resultList.Add(New ClsExtendedResultInfo(message, EnumInfoType.Error))
  End Sub

  Public Sub AddError(format As String, ParamArray args() As Object)
    ResultList.Add(New ClsExtendedResultInfo(String.Format(format, args), EnumInfoType.Error))
  End Sub

  Public Sub AddError(ex As Exception)
    Dim message As New StringBuilder()
    message.Append("Exception: ")
    message.Append(ex.Message)
    Dim innerException As Exception = ex.InnerException
    While innerException IsNot Nothing
      message.AppendLine("Inner Exception: ")
      message.Append(ex.Message)
      innerException = innerException.InnerException
    End While
    message.AppendLine("StackTrace: ")
    message.Append(Environment.StackTrace)
    m_resultList.Add(New ClsExtendedResultInfo(message.ToString(), EnumInfoType.Error))
  End Sub

  Public ReadOnly Property ResultList() As List(Of ClsExtendedResultInfo)
    Get
      Return m_resultList
    End Get
  End Property

  Public ReadOnly Property HasErrorOrWarning() As Boolean
    Get
      Return (
        From r In m_resultList
        Where r.InfoType = EnumInfoType.Warning OrElse r.InfoType = EnumInfoType.Error
        ).Count() > 0
    End Get
  End Property


  Private m_resultList As List(Of ClsExtendedResultInfo)


#Region "Nested Classes"

  Public Enum EnumInfoType

    ''' <summary>
    ''' Information.
    ''' </summary>
    ''' <remarks></remarks>
    Message = 1

    ''' <summary>
    ''' Warnung
    ''' </summary>
    ''' <remarks></remarks>
    Warning = 2

    ''' <summary>
    ''' Fehler
    ''' </summary>
    ''' <remarks></remarks>
    [Error] = 3

  End Enum

  Public Class ClsExtendedResultInfo

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(message As String, infoType As EnumInfoType)
      m_timestamp = Date.Now
      m_message = message
      m_infoType = infoType
    End Sub

    Public Overrides Function ToString() As String
      Return m_timestamp.ToString("dd.MM.yyyy HH:mm:ss") + " " + Me.InfoTypeString + Environment.NewLine _
        + m_message
    End Function

    Public Property Timestamp() As Date
      Get
        Return m_timestamp
      End Get
      Set(value As Date)
        m_timestamp = value
      End Set
    End Property

    Public Property Message() As String
      Get
        Return m_message
      End Get
      Set(value As String)
        m_message = value
      End Set
    End Property

    Public Property InfoType() As EnumInfoType
      Get
        Return m_infoType
      End Get
      Set(value As EnumInfoType)
        m_infoType = value
      End Set
    End Property

    Public ReadOnly Property InfoTypeString() As String
      Get
        Select Case m_infoType
          Case EnumInfoType.Message : Return "Information"
          Case EnumInfoType.Warning : Return "Warnung"
          Case EnumInfoType.Error : Return "Fehler"
          Case Else : Return "Unbekannt"
        End Select
      End Get
    End Property

    Private m_timestamp As Date
    Private m_message As String
    Private m_infoType As EnumInfoType

  End Class

#End Region

End Class
