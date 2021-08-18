
Namespace UI

  Public Class ucBaseControlBottomTab

#Region "Protected Fields"

    ''' <summary>
    ''' Contains the advance payment number.
    ''' </summary>
    Protected m_ZGNumber As Integer?

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Boolean flag indicating if advance payment data is loaded.
    ''' </summary>
    Public ReadOnly Property IsAdvancePaymentDataLoaded As Boolean
      Get
        Return m_ZGNumber.HasValue
      End Get

    End Property

#End Region

  End Class

End Namespace
