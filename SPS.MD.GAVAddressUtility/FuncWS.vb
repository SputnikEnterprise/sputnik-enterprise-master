
Imports SP.Infrastructure.Logging
Imports SPS.MD.GAVAddressUtility.ClsDataDetail

Module FuncWS

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private ReadOnly Property GetUserID() As String
		Get
			Return "7BDD25A4FBB1A9AA5FA5DD9BC8BB6546EFF712FAE3B8C01E20241B03EC9697F04E2DB80838B3F33C"
		End Get
	End Property

	Function GetGAVAdressData(ByVal strGAVBeruf As String,
														ByVal strGAVOrgan As String,
														ByVal strGAVKanton As String) As List(Of String)
		Dim strMessage As String = String.Empty
		Dim strStationID As String = Environment.UserDomainName & "; " &
																Environment.UserName & "; " & Environment.MachineName
		Dim liGAVValue As New List(Of String)

		Try
			Dim strIDString As String = GetUserID
			Dim wsMyService As New GAVServices_1.ClsGetGAVData
			liGAVValue = wsMyService.GetGAVAdressen(strIDString, strGAVKanton, strGAVBeruf, strGAVOrgan).ToList


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Return liGAVValue
	End Function

End Module
