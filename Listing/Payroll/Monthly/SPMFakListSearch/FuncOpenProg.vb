
Option Strict Off

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports System.Xml
Imports System.Xml.Linq

Imports System.Xml.XmlTextWriter
Imports System.Xml.XmlTextReader
Imports System.Xml.XPath
Imports SPMFakListSearch.ClsDataDetail


Module FuncOpenProg

	'Dim _ClsFunc As New ClsDivFunc
	'Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)
    Dim i As Integer = 0
    Dim strSqlQuery As String = "Select RecNr, Bezeichnung, ToolTip, MnuName From ExportDb Where ModulName = @GuidNr "
    strSqlQuery += "Order By RecNr"

    Dim strQuery As String = String.Empty
    Dim xmlDoc As New Xml.XmlDocument()
    Dim xpNav As XPathNavigator
    Dim xni As XPathNodeIterator
    Dim strUSLang As String = _ClsProgSetting.GetUSLanguage()
    Dim strBez As String = String.Empty

    If strUSLang <> String.Empty Then strUSLang = "_" & strUSLang
    xmlDoc.Load(_ClsProgSetting.GetFormDataFile())
    xpNav = xmlDoc.CreateNavigator()


		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@GuidNr", clsDatadetail.GetAppGuidValue())

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

					If Not IsDBNull(rMnurec("MnuName").ToString) Then
						mnu.Name = rMnurec("MnuName").ToString
					End If
					strQuery = "//Control[@Name=" & Chr(34) & clsDatadetail.GetAppGuidValue() & Chr(34) & "]/_" & mnu.Text & strUSLang
					xni = xpNav.Select(strQuery)
					Do While xni.MoveNext()
						strBez = xni.Current.Value
					Loop
					If strBez = String.Empty Then strBez = rMnurec("Bezeichnung").ToString
					mnu.Text = strBez

					If Not IsDBNull(rMnurec("ToolTip")) Then
						strQuery = "//Control[@Name=" & Chr(34) & clsDatadetail.GetAppGuidValue() & Chr(34) & "]/_" & rMnurec("ToolTip").ToString() & strUSLang
						xni = xpNav.Select(strQuery)
						Do While xni.MoveNext()
							strBez = xni.Current.Value
						Loop
						If strBez = String.Empty Then strBez = rMnurec("ToolTip").ToString

						mnu.ToolTipText = strBez
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

	'Sub GetMenuItems4Show(ByVal tsbMenu As ToolStripDropDownButton, _
	'											ByVal dTotalbetrag As Decimal)
	'	Dim i As Integer = 0
	'	Dim strBez As String = String.Empty


	'	Try
	'		tsbMenu.DropDownItems.Clear()
	'		tsbMenu.DropDown.SuspendLayout()

	'		Dim mnu As ToolStripMenuItem

	'		If dTotalbetrag <> 0 Then
	'			mnu = New ToolStripMenuItem()
	'			mnu.Name = "Fak_Total"
	'			strBez = m_Translate.GetSafeTranslationValue("Totalbetrag") & ": "
	'			mnu.Text = strBez & Format(dTotalbetrag, "n")
	'			tsbMenu.DropDownItems.Add(mnu)
	'		End If

	'		tsbMenu.DropDown.ResumeLayout()
	'		tsbMenu.ShowDropDown()

	'	Catch e As Exception
	'		MsgBox(Err.GetException.ToString)

	'	Finally

	'	End Try

	'End Sub

	'#Region "Funktionen für Exportieren..."

	'	Sub RunOpenRPForm(ByVal iRPNr As Integer)
	'		Dim oMyProg As Object
	'		Dim strTranslationProgName As String = String.Empty

	'		strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "RPUtility.ClsMain")
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iRPNr.ToString)

	'		Try
	'			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "RPNr", iRPNr.ToString)

	'			oMyProg = CreateObject("SPSModulsView.ClsMain")
	'			oMyProg.TranslateProg4Net("RPUtility.ClsMain", iRPNr.ToString)

	'		Catch e As Exception
	'			MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenRPForm")

	'		End Try

	'	End Sub

	'	Sub RunOpenMAForm(ByVal iMANr As Integer)
	'		Dim oMyProg As Object
	'		Dim strTranslationProgName As String = String.Empty

	'		strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "KandidatUtility.ClsMain")
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iMANr.ToString)

	'		Try
	'			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "MANr", iMANr.ToString)

	'			oMyProg = CreateObject("SPSModulsView.ClsMain")
	'			oMyProg.TranslateProg4Net("KandidatUtility.ClsMain", iMANr.ToString)

	'		Catch e As Exception
	'			MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenMAForm")

	'		End Try

	'	End Sub

	'	Sub RunOpenESForm(ByVal iESNr As Integer)
	'		Dim oMyProg As Object
	'		Dim strTranslationProgName As String = String.Empty

	'		strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "ESUtility.ClsMain")
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iESNr.ToString)

	'		Try
	'			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "ESNr", iESNr.ToString)

	'			oMyProg = CreateObject("SPSModulsView.ClsMain")
	'			oMyProg.TranslateProg4Net("ESUtility.ClsMain", iESNr.ToString, "2")

	'		Catch e As Exception
	'			MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenESForm")

	'		End Try

	'	End Sub

	'	Sub RunOpenKDForm(ByVal iKDNr As Integer)
	'		Dim oMyProg As Object
	'		Dim strTranslationProgName As String = String.Empty

	'		strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "KundenUtility.ClsMain")
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iKDNr.ToString)

	'		Try
	'			_ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "KDNr", iKDNr.ToString)

	'			oMyProg = CreateObject("SPSModulsView.ClsMain")
	'			oMyProg.TranslateProg4Net("KundenUtility.ClsMain", iKDNr.ToString)

	'		Catch e As Exception
	'			MsgBox(e.Message, MsgBoxStyle.Critical, "RunOpenKDForm")

	'		End Try

	'	End Sub



	'	Sub RunTapi_KDZhd(ByVal strNumber As String, ByVal iKDNr As Integer, ByVal iKDZhdNr As Integer)
	'		Dim strTranslationProgName As String = String.Empty
	'		Dim iTest As Integer = 0

	'		strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSKDZHD.ClsMain")
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iKDZhdNr.ToString)

	'		Try


	'		Catch e As Exception
	'			MsgBox(e.Message, MsgBoxStyle.Critical, "RunTapi_KDZhd")

	'		End Try

	'	End Sub

	'	Private Function ShowMyFileDlg(ByVal strFile2Search As String) As String
	'		Dim strFullFileName As String = String.Empty
	'		Dim strFilePath As String = String.Empty
	'		Dim myStream As Stream = Nothing
	'		Dim openFileDialog1 As New OpenFileDialog()

	'		openFileDialog1.Title = strFile2Search
	'		openFileDialog1.InitialDirectory = strFile2Search
	'		openFileDialog1.Filter = "EXE-Dateien (*.exe)|*.exe|Alle Dateien (*.*)|*.*"
	'		openFileDialog1.FilterIndex = 1
	'		openFileDialog1.RestoreDirectory = True

	'		If openFileDialog1.ShowDialog() = DialogResult.OK Then
	'			Try

	'				myStream = openFileDialog1.OpenFile()
	'				If (myStream IsNot Nothing) Then
	'					strFullFileName = openFileDialog1.FileName()

	'					' Insert code to read the stream here.
	'				End If

	'			Catch Ex As Exception
	'				MessageBox.Show("Kann keine Daten lesen: " & Ex.Message)
	'			Finally
	'				' Check this again, since we need to make sure we didn't throw an exception on open.
	'				If (myStream IsNot Nothing) Then
	'					myStream.Close()
	'				End If
	'			End Try
	'		End If

	'		Return strFullFileName
	'	End Function

	'	Sub RunSMSProg()
	'		'Dim strProgPath As String
	'		'Dim strSMSProgName As String = "Sputnik Suite SMS.EXE"

	'		'Dim strSMSFile As String = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SMSProg")
	'		'If strSMSFile = String.Empty Then
	'		'  strProgPath = _ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "ProgUpperPath")
	'		'  strProgPath = _ClsReg.AddDirSep(strProgPath) & "Binn\"

	'		'  If strSMSFile = String.Empty Then strSMSFile = strProgPath & strSMSProgName
	'		'End If

	'		'If Not File.Exists(strSMSFile) Then
	'		'  MsgBox(_ClsProgSetting.GetMessageFromXML("MSGID1006") & vbLf & _
	'		'          (strSMSFile), MsgBoxStyle.Critical, _ClsProgSetting.GetMessageFromXML("MSGID1007"))

	'		'  strSMSFile = ShowMyFileDlg(strSMSFile)
	'		'  If strSMSFile <> String.Empty Then
	'		'    _ClsReg.SetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SMSProg", strSMSFile)
	'		'    Process.Start(strSMSFile)
	'		'  End If

	'		'Else
	'		'  Process.Start(strSMSFile)

	'		'End If

	'	End Sub

	'	Sub RunMailModul(ByVal strTempSQL As String)
	'		Dim oMyProg As Object
	'		Dim strTranslationProgName As String = String.Empty

	'		strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSMailUtility.ClsMain")
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

	'		Try
	'			oMyProg = CreateObject("SPSModulsView.ClsMain")
	'			oMyProg.TranslateProg4Net("SPSMailUtility.ClsMain", strTempSQL)

	'		Catch e As Exception

	'		End Try

	'	End Sub

	'	'Sub ExportDataToOutlook(ByVal strTempSQL As String)
	'	'  Dim oMyProg As Object
	'	'  Dim strTranslationProgName As String = String.Empty

	'	'  strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'	'  _ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSCommUtil.ClsMain")
	'	'  _ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

	'	'  Try
	'	'    If MsgBox(_ClsProgSetting.GetMessageFromXML("MSGID1008"), _
	'	'              MsgBoxStyle.Information + MsgBoxStyle.YesNo, _ClsProgSetting.GetMessageFromXML("MSGID1009")) = MsgBoxResult.Yes Then
	'	'      oMyProg = CreateObject("SPSModulsView.ClsMain")
	'	'      oMyProg.ExportDataToOutlook(strTempSQL, "KD")
	'	'    End If

	'	'  Catch e As Exception

	'	'  End Try

	'	'End Sub

	'	Sub RunKommaModul(ByVal strTempSQL As String)
	'		Dim oMyProg As Object
	'		Dim strTranslationProgName As String = String.Empty

	'		strTranslationProgName = _ClsProgSetting.GetPersonalFolder() & "SPTranslationProg" & _ClsProgSetting.GetLogedUSNr()
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTxtUtility.ClsMain")
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", strTempSQL)

	'		Try
	'			oMyProg = CreateObject("SPSModulsView.ClsMain")
	'			oMyProg.TranslateProg4Net("SPSTxtUtility.ClsMain", strTempSQL, clsDatadetail.GetAppGuidValue())

	'		Catch e As Exception

	'		End Try

	'	End Sub

	'	Sub RunXMLModul(ByVal strTempSQL As String)
	'		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'		Dim strTranslationProgName As String = String.Empty

	'		Dim cmd As System.Data.SqlClient.SqlCommand
	'		cmd = New System.Data.SqlClient.SqlCommand(strTempSQL & " FOR XML AUTO, ROOT('MeineListe')", Conn)

	'		Try
	'			Conn.Open()

	'			Dim Xml_Reader As System.Xml.XmlReader

	'			Xml_Reader = cmd.ExecuteXmlReader()
	'			Dim sb As New System.Text.StringBuilder
	'			sb.Append("<xml>")
	'			Xml_Reader.Read()
	'			Do
	'				Dim node As String = Xml_Reader.ReadOuterXml()
	'				If node.Length = 0 Then Exit Do
	'				sb.Append(node)
	'			Loop
	'			sb.Append("</xml>")

	'			Xml_Reader.Close()

	'			Dim objDateiMacher As StreamWriter
	'			objDateiMacher = New StreamWriter(_ClsProgSetting.GetPersonalFolder() & "MyList.XML")
	'			objDateiMacher.Write(sb.ToString)
	'			objDateiMacher.Close()
	'			objDateiMacher.Dispose()
	'			'MsgBox(_ClsProgSetting.GetMessageFromXML("MSGID1010") & (_ClsProgSetting.GetPersonalFolder() & "MyList.XML"), MsgBoxStyle.Information, _ClsProgSetting.GetMessageFromXML("MSGID1011"))

	'		Catch e As Exception
	'			MsgBox(e.Message, MsgBoxStyle.Critical, "RunXMLModul")

	'		End Try

	'	End Sub


	'#End Region



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

	'Function ExtraRights(ByVal lModulNr As Integer) As Boolean
	'  Dim bAllowed As Boolean
	'  Dim strModulCode As String

	'  ' 10200        ' Fremdrechnung
	'  ' 10201        ' Rapportinhalt
	'  ' 10202        ' Export nach Abacus
	'  ' 10206        ' Export nach Sesam
	'  strModulCode = _ClsReg.GetINIString(_ClsProgSetting.GetInitIniFile, "ExtraModuls", CStr(lModulNr))
	'  If InStr(1, strModulCode, "+" & lModulNr & "+") > 0 Then bAllowed = True

	'  ExtraRights = bAllowed

	'End Function



End Module
