
Option Strict Off

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports SPLOStdListSearch.ClsDataDetail


Module FuncOpenProg

	'Private _ClsFunc As New ClsDivFunc
	'Dim _ClsReg As New SPProgUtility.ClsDivReg
	'Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)
    '    Dim strFieldName As String = "Bezeichnung"
    Dim i As Integer = 0
    Dim strSqlQuery As String = "Select RecNr, Bezeichnung, ToolTip, MnuName From ExportDb Where ModulName = @GuidNr "
    strSqlQuery += "Order By RecNr"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

    Dim strQuery As String = String.Empty
    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@GuidNr", ClsDataDetail.GetAppGuidValue())

      Dim rMnurec As SqlDataReader = cmd.ExecuteReader

      tsbMenu.DropDownItems.Clear()
      tsbMenu.DropDown.SuspendLayout()

      Dim mnu As ToolStripMenuItem
      While rMnurec.Read
        i += 1

        If rMnurec("Bezeichnung").ToString = "-" Then
          Dim sep As New ToolStripSeparator()
          tsbMenu.DropDownItems.Add(sep)

        Else
          mnu = New ToolStripMenuItem()

          mnu.Text = rMnurec("Bezeichnung").ToString
          If Not IsDBNull(rMnurec("ToolTip")) Then
            mnu.ToolTipText = rMnurec("ToolTip").ToString
          End If
          If Not IsDBNull(rMnurec("MnuName").ToString) Then
            mnu.Name = rMnurec("MnuName").ToString
          End If
          tsbMenu.DropDownItems.Add(mnu)

        End If

      End While
      tsbMenu.DropDown.ResumeLayout()
      tsbMenu.ShowDropDown()


    Catch e As Exception
      MsgBox(Err.GetException.ToString)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

#Region "Funktionen für Exportieren..."

	'Sub RunBewModul(ByVal strTempSQL As String)
	'  Dim oMyProg As Object
	'  Dim strTranslationProgName As String = String.Empty

	'  strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'  _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSBewUtility.ClsMain")
	'  _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

	'  Try
	'    oMyProg = CreateObject("SPSBewUtility.ClsMain")
	'    oMyProg.OpenKDFieldsform(strTempSQL)

	'  Catch e As Exception

	'  End Try

	'End Sub


	' ''' <summary>
	' ''' Lohnabrechnungsverwaltung öffnen
	' ''' </summary>
	' ''' <param name="iLONr"></param>
	' ''' <remarks></remarks>
	'Sub RunOpenLOForm(ByVal iLONr As Integer, ByVal iLP As Integer, ByVal iJahr As Integer)
	'  Dim oMyProg As Object
	'  Dim strTranslationProgName As String = String.Empty

	'  Try
	'    _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "LONr", iLONr.ToString)

	'    oMyProg = CreateObject("SPSModulsView.ClsMain")
	'    oMyProg.TranslateProg4Net("SPSLOUtility.ClsMain", iLONr.ToString, iLP.ToString, iJahr.ToString)
	'  Catch e As Exception
	'    MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenLOForm")
	'  End Try
	'End Sub


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
