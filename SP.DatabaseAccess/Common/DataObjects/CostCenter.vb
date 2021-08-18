Namespace Common.DataObjects
  ''' <summary>
  ''' The CostCenters 1 and 2.
  ''' </summary>
  Public Class CostCenters

#Region "Private Fields"

    Private m_CostCenter1 As List(Of CostCenter)
    Private m_CostCenter2 As List(Of CostCenter2)

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The consturctor.
    ''' </summary>
    Public Sub New()
      m_CostCenter1 = New List(Of CostCenter)
      m_CostCenter2 = New List(Of CostCenter2)
    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the const centers1.
    ''' </summary>
    Public ReadOnly Property CostCenter1 As List(Of CostCenter)
      Get
        Return m_CostCenter1
      End Get
    End Property

    ''' <summary>
    ''' Gets the cost centers2.
    ''' </summary>
    Public ReadOnly Property CostCenter2 As List(Of CostCenter2)
      Get
        Return m_CostCenter2
      End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Gets a list of dependent cost center2 for a cost center1.
    ''' </summary>
    ''' <param name="costCenter1">The cost center1.</param>
    ''' <returns>The list of cost center 2</returns>
    Public Function GetCostCenter2ForCostCenter1(ByVal costCenter1 As String) As List(Of CostCenter2)

      Dim costCenters2 = New List(Of CostCenter2)

      If (String.IsNullOrEmpty(costCenter1)) Then
        Return costCenters2
      End If

      costCenters2 = m_CostCenter2.Where(Function(data) data.KSTName1 = costCenter1).ToList()

      Return costCenters2
    End Function

    ''' <summary>
    ''' Adds a cost center1.
    ''' </summary>
    ''' <param name="costCenter">The cost center.</param>
    Public Sub AddCostCenter1(ByVal costCenter As CostCenter)
      m_CostCenter1.Add(costCenter)
    End Sub

    ''' <summary>
    ''' Adds a cost center2.
    ''' </summary>
    ''' <param name="costCenter">The cost center.</param>
    Public Sub AddCostCenter2(ByVal costCenter As CostCenter2)
      m_CostCenter2.Add(costCenter)
    End Sub

#End Region

  End Class

  Public Class CostCenter
    Public Property KSTName As String
    Public Property KSTBezeichnung As String

  End Class

  Public Class CostCenter2
    Inherits CostCenter
    Public Property KSTName1 As String

  End Class


  Public Class DefaultCostCenters
    Public Property CostCenter1 As String
    Public Property CostCenter2 As String
  End Class

End Namespace
