
Option Strict Off

Imports System.Data.SqlClient
Imports SP.Infrastructure.Logging

Module FuncOpenProg

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsReg As New SPProgUtility.ClsDivReg


	Function GetMenuItems4Export() As List(Of String)
		Dim sql As String = String.Format("Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From ExportDb Where ModulName = '{0}' Order By RecNr",
																							ClsDataDetail.GetAppGuidValue)
		Dim liResult As New List(Of String)

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMnurec As SqlDataReader = cmd.ExecuteReader

			While rMnurec.Read

				liResult.Add(String.Format("{0}#{1}#{2}", (rMnurec("Bezeichnung").ToString),
																		 (rMnurec("MnuName").ToString),
																		 (rMnurec("Docname").ToString)))

			End While


		Catch e As Exception
			MsgBox(e.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return liResult

	End Function

#Region "Funktionen für Exportieren..."

	'Sub RunOpenKDForm(ByVal iKDNr As Integer)

	'	Try
	'		Dim hub = MessageService.Instance.Hub
	'		Dim openMng As New OpenCustomerMngRequest(Me, ClsDataDetail.UserData.UserNr, ClsDataDetail.MDData.MDNr, iKDNr)
	'		hub.Publish(openMng)




	'		'Dim frm_Kunden As New frmCustomers(CreateInitialData(ClsDataDetail.m_InitialData.MDData.MDNr, ClsDataDetail.m_InitialData.UserData.UserNr))
	'		'If iKDNr > 0 Then
	'		'	frm_Kunden.LoadCustomerData(iKDNr)
	'		'Else
	'		'	frm_Kunden.LoadCustomerData(Nothing)

	'		'End If
	'		'frm_Kunden.Show()

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)

	'	End Try


	'End Sub

	Sub RunOpenOPForm(ByVal iOPNr As Integer)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iOPNr.ToString)

		Try
			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "RENr", iOPNr.ToString)

			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("OPUtility.ClsMain", iOPNr.ToString, 2)

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenOPForm")

		End Try

	End Sub

	Sub RunKommaModul(ByVal strTempSQL As String)
		Dim oMyProg As Object
		Dim strTranslationProgName As String = String.Empty

		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

		Try
			oMyProg = CreateObject("SPSModulsView.ClsMain")
			oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "OP")

		Catch e As Exception

		End Try

	End Sub


#End Region

	Public Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
		Dim bResult As Boolean = False

		' alle geöffneten Forms durchlauden
		For Each oForm As Form In Application.OpenForms
			If oForm.Name.ToLower = sName.ToLower Then
				If bDisposeForm Then oForm.Dispose() : Exit For
				bResult = True : Exit For
			End If
		Next

		Return (bResult)
	End Function


End Module
