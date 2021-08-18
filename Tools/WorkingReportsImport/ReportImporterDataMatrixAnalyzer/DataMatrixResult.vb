'------------------------------------
' File: DataMatrixResult.vb
' Date: 22.08.2012
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text.RegularExpressions

''' <summary>
''' DataMatrix analysis result.
''' </summary>
Public Class DataMatrixResult

#Region "Private Fields"

    ''' <summary>
    ''' The full path of the PDF file that has been analyzed.
    ''' </summary>
    Private pdfFilePathValue As String

    ''' <summary>
    ''' The data matrix infos found in the PDF file.
    ''' </summary>
    Private dataMatrixInfosList As New List(Of DataMatrixInfo)

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="pdfFilePathValue">The pdf file full path.</param>
    Public Sub New(ByVal pdfFilePathValue As String)
        Me.pdfFilePathValue = pdfFilePathValue
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Adds a DataMatrix info object.
    ''' </summary>
    ''' <param name="dataMatrixInfo">The DataMatrix info object.</param>
    Public Sub AddDataMatrixInfo(ByVal dataMatrixInfo As DataMatrixInfo)

        If Not dataMatrixInfo Is Nothing Then
            Me.dataMatrixInfosList.Add(dataMatrixInfo)
        End If

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Boolean truth value indicating if PDF file could be analyzed.
    ''' </summary>
    Public Property CouldPDFFileBeAnalyzed As Boolean

#End Region

#Region "Public Readonly Properties"

    ''' <summary>
    ''' Gets the full path of the PDF file that has been analyzed.
    ''' </summary>
    Public ReadOnly Property PDFFullPath As String
        Get
            Return Me.pdfFilePathValue
        End Get
    End Property

    ''' <summary>
    ''' Gets the DataMatrix infos.
    ''' </summary>
    Public ReadOnly Property DataMatrixInfos As DataMatrixInfo()
        Get
            Return Me.dataMatrixInfosList.ToArray()
        End Get
    End Property

#End Region

End Class
