Namespace Common.DataObjects

  Public Class PermissionData

    Public Property RecValue As String

    ''' <summary>
    ''' Gets or set the permission text.
    ''' </summary>
    ''' <returns>Returns the translated permssion text.</returns>
    Public Property TranslatedPermission As String

		Public ReadOnly Property PermissionCodeViewData As String
			Get
				Return String.Format("{0} - {1}", RecValue, TranslatedPermission)
			End Get
		End Property

	End Class

End Namespace
