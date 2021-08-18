
'Option Strict Off

'Imports System.Data.SqlClient
'Imports System.IO
'Imports System.Text.RegularExpressions
'Imports System.Reflection
'Imports SP.KD.CustomerMng.UI
'Imports SP.MA.EmployeeMng.UI
'Imports SP.Infrastructure.UI
'Imports SP.MA.EinsatzMng.UI
'Imports SPRPListSearch.ClsDataDetail
'Imports SP.Infrastructure.Logging

'Module FuncOpenProg

'	''' <summary>
'	''' The logger.
'	''' </summary>
'	Private m_Logger As ILogger = New Logger()

'	Private m_UtilityUI As New UtilityUI

'	Private _ClsFunc As New ClsDivFunc
'	Private _ClsReg As New SPProgUtility.ClsDivReg


'	'Function GetMenuItems4Export() As List(Of String)
'	'	Dim sql As String = String.Format("Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From ExportDb Where ModulName = '{0}' Order By RecNr",
'	'																						ClsDataDetail.GetAppGuidValue)
'	'	Dim liResult As New List(Of String)

'	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

'	'	Try
'	'		Conn.Open()

'	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
'	'		cmd.CommandType = Data.CommandType.Text

'	'		Dim rMnurec As SqlDataReader = cmd.ExecuteReader

'	'		While rMnurec.Read

'	'			liResult.Add(String.Format("{0}#{1}#{2}", m_Translate.GetSafeTranslationValue(rMnurec("Bezeichnung").ToString),
'	'																	 m_Translate.GetSafeTranslationValue(rMnurec("MnuName").ToString),
'	'																	 m_Translate.GetSafeTranslationValue(rMnurec("Docname").ToString)))

'	'		End While


'	'	Catch e As Exception
'	'		MsgBox(e.ToString)

'	'	Finally
'	'		Conn.Close()
'	'		Conn.Dispose()

'	'	End Try

'	'	Return liResult

'	'End Function


'	'Sub GetMenuItems4Show(ByVal tsbMenu As ToolStripDropDownButton, ByVal dBetrag_1 As Double)
'	'  Dim i As Integer = 0

'	'  Try
'	'    tsbMenu.DropDownItems.Clear()
'	'    tsbMenu.DropDown.SuspendLayout()

'	'    Dim mnu As ToolStripMenuItem

'	'    mnu = New ToolStripMenuItem()
'	'    mnu.Text = m_Translate.GetSafeTranslationValue("Totalbetrag:") & " " & Format(dBetrag_1, "n")
'	'    tsbMenu.DropDownItems.Add(mnu)

'	'    tsbMenu.DropDown.ResumeLayout()
'	'    tsbMenu.ShowDropDown()

'	'  Catch e As Exception
'	'    MsgBox(Err.GetException.ToString)

'	'  Finally

'	'  End Try

'	'End Sub

'#Region "Funktionen für Exportieren..."

'	'Sub RunOpenRPForm(ByVal iRPNr As Integer)
'	'  Dim oMyProg As Object
'	'  Dim strTranslationProgName As String = String.Empty

'	'  _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "RPUtility.ClsMain")
'	'  _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iRPNr.ToString)

'	'  Try
'	'    _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "RPNr", iRPNr.ToString)

'	'    oMyProg = CreateObject("SPSModulsView.ClsMain")
'	'    oMyProg.TranslateProg4Net("RPUtility.ClsMain", iRPNr.ToString)

'	'  Catch e As Exception
'	'    MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenRPForm")

'	'  End Try

'	'End Sub

'	'Sub OpenSelectedEmployee(ByVal Employeenumber As Integer)

'	'  Try
'	'    Dim frm As frmEmployees = CType(ClsDataDetail.GetModuleCach.GetModuleForm(m_InitialData.MDData.MDNr, SP.ModuleCaching.ModuleName.EmployeeMng), frmEmployees)
'	'    frm.LoadEmployeeData(Employeenumber)

'	'    If frm.IsEmployeeDataLoaded Then
'	'      frm.Show()
'	'      frm.BringToFront()
'	'    End If

'	'  Catch ex As Exception
'	'    m_Logger.LogError(ex.ToString())
'	'  End Try

'	'End Sub

'	'Sub OpenSelectedES(ByVal ESNumber As Integer)
'	'  Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'	'  Try
'	'    Dim frmEinsatz As SP.MA.EinsatzMng.UI.frmES = CType(ClsDataDetail.GetModuleCach.GetModuleForm(m_InitialData.MDData.MDNr, SP.ModuleCaching.ModuleName.ESMng), frmES)

'	'    frmEinsatz.LoadESData(ESNumber)
'	'    If frmEinsatz.IsESDataLoaded Then
'	'      frmEinsatz.Show()
'	'      frmEinsatz.BringToFront()
'	'    End If


'	'  Catch ex As Exception
'	'    m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))
'	'    m_UtilityUI.ShowErrorDialog(ex.Message)

'	'  End Try

'	'End Sub


'	'Sub RunOpenMAForm(ByVal _iMANr As Integer)
'	'  If ClsDataDetail.IsModernUIAllowed Then
'	'    OpenSelectedEmployee(_iMANr)
'	'    Exit Sub
'	'  End If

'	'  Dim oMyProg As Object
'	'  Dim strTranslationProgName As String = String.Empty

'	'  _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "KandidatUtility.ClsMain")
'	'  _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", _iMANr.ToString)

'	'  Try
'	'    _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "MANr", _iMANr.ToString)

'	'    oMyProg = CreateObject("SPSModulsView.ClsMain")
'	'    oMyProg.TranslateProg4Net("KandidatUtility.ClsMain", _iMANr.ToString)

'	'  Catch e As Exception
'	'    MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenMAForm")

'	'  End Try

'	'End Sub

'	'Sub RunOpenESForm(ByVal _iESNr As Integer)
'	'  If ClsDataDetail.IsModernUIAllowed Then
'	'    OpenSelectedES(_iESNr)
'	'    Exit Sub
'	'  End If

'	'  Dim oMyProg As Object
'	'  Dim strTranslationProgName As String = String.Empty

'	'  _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "ESUtility.ClsMain")
'	'  _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", _iESNr.ToString)

'	'  Try
'	'    _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "ESNr", _iESNr.ToString)

'	'    oMyProg = CreateObject("SPSModulsView.ClsMain")
'	'    oMyProg.TranslateProg4Net("ESUtility.ClsMain", _iESNr.ToString, "2")

'	'  Catch e As Exception
'	'    MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenESForm")

'	'  End Try

'	'End Sub

'	'Sub RunOpenKDForm(ByVal iKDNr As Integer)

'	'  Try
'	'    Dim frm_Kunden As New frmCustomers(CreateInitialData(m_InitialData.MDData.MDNr, m_InitialData.UserData.UserNr))
'	'    If iKDNr > 0 Then
'	'      frm_Kunden.LoadCustomerData(iKDNr)
'	'    Else
'	'      frm_Kunden.LoadCustomerData(Nothing)

'	'    End If
'	'    frm_Kunden.Show()

'	'  Catch ex As Exception
'	'    m_Logger.LogError(ex.ToString)

'	'  End Try

'	'End Sub



'	'Sub RunTapi_KDZhd(ByVal strNumber As String, ByVal iKDNr As Integer, ByVal iKDZhdNr As Integer)
'	'  Dim strTranslationProgName As String = String.Empty
'	'  Dim iTest As Integer = 0

'	'  _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSKDZHD.ClsMain")
'	'  _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iKDZhdNr.ToString)

'	'  Try


'	'  Catch e As Exception
'	'    MsgBox(e.Message, MsgBoxStyle.Critical, "RunTapi_KDZhd")

'	'  End Try

'	'End Sub

'	'Private Function ShowMyFileDlg(ByVal strFile2Search As String) As String
'	'	Dim strFullFileName As String = String.Empty
'	'	Dim strFilePath As String = String.Empty
'	'	Dim myStream As Stream = Nothing
'	'	Dim openFileDialog1 As New OpenFileDialog()

'	'	openFileDialog1.Title = strFile2Search
'	'	openFileDialog1.InitialDirectory = strFile2Search
'	'	openFileDialog1.Filter = "EXE-Dateien (*.exe)|*.exe|Alle Dateien (*.*)|*.*"
'	'	openFileDialog1.FilterIndex = 1
'	'	openFileDialog1.RestoreDirectory = True

'	'	If openFileDialog1.ShowDialog() = DialogResult.OK Then
'	'		Try

'	'			myStream = openFileDialog1.OpenFile()
'	'			If (myStream IsNot Nothing) Then
'	'				strFullFileName = openFileDialog1.FileName()

'	'				' Insert code to read the stream here.
'	'			End If

'	'		Catch Ex As Exception
'	'			MessageBox.Show("Kann keine Daten lesen: " & Ex.Message)
'	'		Finally
'	'			' Check this again, since we need to make sure we didn't throw an exception on open.
'	'			If (myStream IsNot Nothing) Then
'	'				myStream.Close()
'	'			End If
'	'		End Try
'	'	End If

'	'	Return strFullFileName
'	'End Function

'	'Sub RunKommaModul(ByVal strTempSQL As String)
'	'	Dim oMyProg As Object
'	'	Dim strTranslationProgName As String = String.Empty

'	'	_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
'	'	_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

'	'	Try
'	'		oMyProg = CreateObject("SPSModulsView.ClsMain")
'	'		oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, "ZG")

'	'	Catch e As Exception

'	'	End Try

'	'End Sub

'#End Region


'	Public Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
'		Dim bResult As Boolean = False

'		' alle geöffneten Forms durchlauden
'		For Each oForm As Form In Application.OpenForms
'			If oForm.Name.ToLower = sName.ToLower Then
'				If bDisposeForm Then oForm.Dispose() : Exit For
'				bResult = True : Exit For
'			End If
'		Next

'		Return (bResult)
'	End Function



'End Module
