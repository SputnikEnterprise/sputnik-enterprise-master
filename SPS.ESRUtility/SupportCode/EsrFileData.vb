''' <summary>
''' Klasse mit dem gesamten Inhalt eier ESR Datei.
''' <author>egle</author>
''' </summary>
''' <remarks></remarks>
Public Class EsrFileData

#Region "Private Fields"

  Private m_list As IList(Of EsrRecord)
  Private m_listDetail As IList(Of EsrRecord)
  Private m_summaryRecord As EsrRecord
  Private m_amountTotal As Decimal?

#End Region ' Private Fields

#Region "Public Methods"

  ''' <summary>
  ''' Konstruktor.
  ''' </summary>
  ''' <remarks></remarks>
  Public Sub New()
    m_list = New List(Of EsrRecord)
  End Sub

  ''' <summary>
  ''' Fügt die Daten zur Liste. Teilt die Daten auf, wenn sie länger als <ref>EsrData.LINELENGTH_MAX</ref> sind.
  ''' </summary>
  ''' <param name="data"></param>
  ''' <param name="use7digits"></param>
  ''' <remarks></remarks>
  Public Sub Add(ByVal data As String, Optional ByVal use7digits As Boolean = False)
    Dim length As Integer
		Dim line As String

		For startIndex As Integer = 0 To data.Length - 1 Step EsrRecord.LINELENGTH_MAX
      length = Math.Min(data.Length - startIndex, EsrRecord.LINELENGTH_MAX)
      line = data.Substring(startIndex, length)

			' TODO: Fardin 10-stellig aus Konfiguration
			m_list.Add(New EsrRecord(line, use7digits))
		Next

	End Sub

#End Region ' Public Methods

#Region "Public Properties"

	''' <summary>
	''' Liste mit allen Records
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property List As IList(Of EsrRecord)
    Get
      Return m_list
    End Get
  End Property

  ''' <summary>
  ''' Liste mit Detail Records
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public ReadOnly Property ListDetail As IList(Of EsrRecord)
    Get
      If m_listDetail Is Nothing Then
        m_listDetail = (
          From item In m_list
          Where item.IsSummaryRecord = False
          Select item
          ).ToList()
      End If
      Return m_listDetail
    End Get
  End Property

  ''' <summary>
  ''' Gesamt Eintrag.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public ReadOnly Property SummaryRecord As EsrRecord
    Get
      If m_summaryRecord Is Nothing Then
        m_summaryRecord = (
          From item In m_list
          Where item.IsSummaryRecord
          Select item
          ).FirstOrDefault()
      End If
      Return m_summaryRecord
    End Get
  End Property

  Public ReadOnly Property AmountTotal As Decimal
    Get
      If Not m_amountTotal.HasValue Then
        Dim summaryRecord = Me.SummaryRecord
        If summaryRecord Is Nothing Then
          m_amountTotal = 0D
        Else
          m_amountTotal = summaryRecord.Amount
        End If
      End If
      Return m_amountTotal.Value
    End Get
  End Property

#End Region ' Public Properties

End Class
