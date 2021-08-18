

Namespace JobPlatform.AVAM


	''' <summary>
	''' Interface for vacancy database access.
	''' </summary>
	Public Interface IAVAMDatabaseAccess

		Function AddAVAMNotifyResultData(ByVal customerID As String, ByVal userid As String, ByVal jobroomID As String, ByVal resultContent As String, ByVal syncFrom As String) As Boolean



	End Interface


End Namespace
