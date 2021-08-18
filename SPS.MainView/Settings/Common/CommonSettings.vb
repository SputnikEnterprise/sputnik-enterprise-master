
Public Class GridData

  Public Property SQLQuery As String
  Public Property DisplayMember As String

  Public Property GridColFieldName As String
  Public Property GridColCaption As String
  Public Property GridColWidth As String

  Public Property BackColor As String
  Public Property ForeColor As String

  Public Property PopupFields As String
  Public Property PopupCaptions As String

  Public Property CountOfFieldsInHeader As Short?
  Public Property FieldsInHeaderToShow As String
  Public Property CaptionsInHeaderToShow As String

  Public Property IsUserProperty As Boolean

	Public Property MainGridColumnData As List(Of CridDataColumnProperties)
	Public Property PopupColumnData As List(Of PopupCridColumnData)
	Public Property TopPropertyColumnData As List(Of PopupCridColumnData)


End Class

Public Class CridDataColumnProperties

	Public Property GridColFieldName As String
	Public Property GridColCaption As String
	Public Property GridColWidth As String
	Public Property ShowColumn As Boolean

	Public Property ColumnDataTye As EnunColumnDataType

	'Public Property FieldsInHeaderToShow As String
	'Public Property CaptionsInHeaderToShow As String

End Class

Public Class PopupCridColumnData

	Public Property ColumnName As String
	Public Property ColumnCaption As String

End Class

Public Enum EnunColumnDataType

	COMMONTYPE
	DATE_COLUMN_NAME
	DATE_COLUMN_NAME_LONG
	INTEGER_COLUMN_NAME
	DECIMAL_COLUMN_NAME
	CHECKBOX_COLUMN_NAME

	CHECK_EDIT_NOT_ALLOWED
	CHECK_EDIT_WARNING
	CHECK_EDIT_COMPLETED
	CHECK_EDIT_EXPIRE

End Enum

Public Class PropertyGridData

  Public Property SQLQuery As String
  Public Property DisplayMember As String

  Public Property GridColFieldName As String
  Public Property GridColCaption As String
  Public Property GridColWidth As String

  Public Property BackColor As String
  Public Property ForeColor As String

  Public Property PopupFields As String
  Public Property PopupCaptions As String

  Public Property CountOfFieldsInHeader As Short?
  Public Property FieldsInHeaderToShow As String
  Public Property CaptionsInHeaderToShow As String

  Public Property IsUserProperty As Boolean

End Class


Public Class MenuData

  Public Property mnuitem As FoundedEmployeeData
  Public Property mnuvalue As String

End Class


Class PropertyData
  Public Property ValueName As String
  Public Property Value As String
End Class

