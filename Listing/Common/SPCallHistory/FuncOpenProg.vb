
Option Strict Off

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Module FuncOpenProg

  Private _ClsFunc As New ClsDivFunc
  Private _ClsReg As New SPProgUtility.ClsDivReg


  Function GetMenuItems4Export() As List(Of String)
		Dim sql As String = String.Format("Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From ExportDb Where ModulName = '{0}' Order By RecNr", _
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

  Sub RunOpenMAForm(ByVal iMANr As Integer)
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    If iMANr = 0 Then Exit Sub
    Try
      _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "MANr", iMANr.ToString)

      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("KandidatUtility.ClsMain", iMANr.ToString)

    Catch e As Exception
      MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenMAForm")

    End Try

  End Sub

  Sub RunOpenKDform(ByVal iKDNr As Integer)
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    If iKDNr = 0 Then Exit Sub

    Try
      _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "KDNr", iKDNr.ToString)

      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("KundenUtility.ClsMain", iKDNr.ToString)

    Catch e As Exception
      MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenKDForm")

    End Try

  End Sub

  Sub RunOpenKDZForm(ByVal iKDNr As Integer, ByVal iKDZhdNr As Integer)
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    If iKDNr = 0 Or iKDZhdNr = 0 Then Exit Sub
    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSKDZHD.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iKDZhdNr.ToString)

    Try
      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("SPSKDZHD.ClsMain", iKDNr.ToString, iKDZhdNr.ToString)

    Catch e As Exception
      MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenKDZForm")

    End Try

  End Sub

  Sub RunKommaModul(ByVal strTempSQL As String)
    Dim oMyProg As Object
    Dim strTranslationProgName As String = String.Empty

    _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
    _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

    Try
      oMyProg = CreateObject("SPSModulsView.ClsMain")
      oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "ma")

    Catch e As Exception

    End Try

  End Sub


#End Region

  Public Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
    Dim bResult As Boolean = False

    ' alle geöffneten Forms durchlaufen
    For Each oForm As Form In Application.OpenForms
      If oForm.Name.ToLower = sName.ToLower Then
        If bDisposeForm Then oForm.Dispose() : Exit For
        bResult = True : Exit For
      End If
    Next

    Return (bResult)
  End Function

End Module
