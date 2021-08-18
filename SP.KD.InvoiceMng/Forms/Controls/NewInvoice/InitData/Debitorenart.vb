
Namespace UI

  ''' <summary>
  ''' Debitoren Art
  ''' </summary>
  Public Class Debitorenart
    Public Property Display As String
    Public Property Value As String
    Public ReadOnly Property Label As String
      Get
				Return String.Format("{0} - {1}", Value, Display)
			End Get
    End Property
  End Class

	Public Class DebitorenAutomatedArt
		Public Property Value As Integer
		Public Property Display As String
		Public ReadOnly Property Label As String
			Get
				Return String.Format("{0}", Display)
			End Get
		End Property
	End Class

	Public Class WhatTODO
		Public Property Value As Integer
		Public Property Display As String
		Public ReadOnly Property Label As String
			Get
				Return String.Format("{0}", Display)
			End Get
		End Property
	End Class

	Public Class DunningLevel
		Public Property Value As Integer
		Public Property Display As String
		Public ReadOnly Property Label As String
			Get
				Return String.Format("{0}", Display)
			End Get
		End Property
	End Class

End Namespace
