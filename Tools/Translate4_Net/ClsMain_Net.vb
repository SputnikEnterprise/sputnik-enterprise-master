
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.ComponentModel
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Drawing

'Imports SP.MA.BankMng

'Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsLOFunktionality
'Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthabenIndividuell
'Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthabenProLohnabrechnung
'Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsGuthaben
'Imports SPS.MA.Guthaben.ShowMAGuthabenForm.ClsShowForm
'Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsLohn

Imports NLog
Imports SP.KD.CustomerMng.UI
Imports SP.KD.CPersonMng.UI
'Imports SP.KD.ReAdresse.UI

Imports SP.MA.EmployeeMng.UI
'Imports SP.Infrastructure.Messaging
'Imports SP.Infrastructure.Messaging.Messages

'Imports SP.MA.EinsatzMng.UI
'Imports SP.MA.ReportMng.UI
Imports SP.KD.InvoiceMng.UI


Public Interface IDotNet4Class


	Function OpenResponsibleCustomerForm(iKDNr As Integer, iZHDNr As Integer) As String

	Function OpenCustomerForm(ByVal iMDNr As Integer, ByVal iYear As Integer, ByVal iLogedUSNr As Integer, ByVal strCustomer_ID As String, ByVal iKDNr As Integer) As String



	Function OpenQualificationForm(bSelectMulti As Boolean, strsex As String) As String


	Function SendOfferWithMail(iOfferNr As Integer, iKDTempNr As Integer, iKDZTempNr As Integer, bSendAsTest As Boolean, _
														 streMailField As String, strGuid As String, strJobNr As String, _
														 Optional strFileToSend As String = "") As Boolean


	Function PrintOfferBlattToFile(iOffNr As Integer, iMANr As Integer, iKDNr As Integer, iZHDNr As Integer, _
																 strModulToPrint As String) As String

	Sub PrintOfferBlatt(iOffNr As Integer, iMANr As Integer, iKDNr As Integer,
											iZHDNr As Integer, bShowForm As Boolean,
											bForExport As Boolean, strJobNr As String, strSQLQuery As String)


End Interface

Public Class ClsMain_Net
	Implements IDotNet4Class


	Private _ClsSettingPath As New ClsProgSettingPath


	Sub StartUpdate()
		StartPathWatching()
	End Sub


	Sub PrintOfferBlatt(ByVal iOffNr As Integer, ByVal iMANr As Integer, _
											ByVal iKDNr As Integer, ByVal iZHDNr As Integer, _
											ByVal bShowForm As Boolean, ByVal bForExport As Boolean, _
											ByVal strJobNr As String, ByVal strSQLQuery As String) Implements IDotNet4Class.PrintOfferBlatt
		Dim o2Open As New SPSOfferUtility_Net.ClsMain_Net

		Try
			o2Open.PrintLLDoc(iOffNr, iMANr, iKDNr, iZHDNr, bShowForm, bForExport, strJobNr, strSQLQuery)

		Catch ex As Exception
			MsgBox(ex.StackTrace & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical, "PrintOfferBlatt")

		End Try

	End Sub

	Function PrintOfferBlattToFile(ByVal iOffNr As Integer, _
																 ByVal iMANr As Integer, _
																 ByVal iKDNr As Integer, _
																 ByVal iZHDNr As Integer, _
																 ByVal strModulToPrint As String) As String Implements IDotNet4Class.PrintOfferBlattToFile
		Dim o2Open As New SPSOfferUtility_Net.ClsMain_Net

		Return o2Open.PrintLLDocToFile(iOffNr, iMANr, iKDNr, iZHDNr, False, True, strModulToPrint)
	End Function

	Function SendOfferWithMail(ByVal iOfferNr As Integer,
														ByVal iKDTempNr As Integer,
														ByVal iKDZTempNr As Integer,
														ByVal bSendAsTest As Boolean,
														ByVal streMailField As String,
														ByVal strGuid As String,
														ByVal strJobNr As String,
														Optional ByVal strFileToSend As String = "") As Boolean Implements IDotNet4Class.SendOfferWithMail
		Try

			Dim o2Open_1 As New SPSSendMail.ClsMain_Net
			o2Open_1.SendOfferWithMail(iOfferNr, 0, iKDTempNr, iKDZTempNr, False, True, True, True, streMailField,
																 System.Guid.NewGuid.ToString, "", "", "") 'strOfferblattFileName)

		Catch ex As Exception

		End Try

		Return True
	End Function

	'Function SendMADocFile2EMail(ByVal iMANr As Integer, _
	'														 ByVal strFileName As String, _
	'														 ByVal strDocArt As String) As String Implements IDotNet4Class.SendMADocFile2EMail
	'	Dim obj As New SPSSendMail.ClsMain_Net
	'	Dim strValue As String

	'	strValue = obj.SendMADoc2eMailWithTemplate(iMANr, strFileName, strDocArt)

	'	Return strValue
	'End Function

	'	Function SendDocEMail2MA(ByVal iMANr As Integer) As String Implements IDotNet4Class.SendDocEMail2MA
	'		Dim obj As New SPSSendMail.ClsMain_Net
	'		Dim strValue As String

	'		strValue = obj.SendMADoceMailWithTemplate(iMANr)

	'		Return strValue
	'	End Function

	'	Function SendDocEMail2KD(ByVal iKDNr As Integer) As String Implements IDotNet4Class.SendDocEMail2KD
	'		Dim obj As New SPSSendMail.ClsMain_Net
	'		Dim strValue As String

	'		strValue = obj.SendKDDoceMailWithTemplate(iKDNr)

	'		Return strValue
	'	End Function

	'	Function SendDocEMail2ZHD(ByVal iKDNr As Integer, ByVal iZHDNr As Integer) As String Implements IDotNet4Class.SendDocEMail2ZHD
	'		Dim obj As New SPSSendMail.ClsMain_Net
	'		Dim strValue As String

	'		strValue = obj.SendZHDDoceMailWithTemplate(iKDNr, iZHDNr)

	'		Return strValue
	'	End Function

	'	Function OpenMailingList(ByVal iCommArt As Short, _
	'													 ByVal strGuid As String) As Boolean Implements IDotNet4Class.OpenMailingList

	'		Dim o2Open_1 As New SPSSendMail.ClsMain_Net

	'		o2Open_1.OpenMailingList(iCommArt, strGuid)

	'		Return True
	'	End Function


	'	'' Uploadfunktion für Übermittlung der Verleihverträge...
	'	'Sub runSPFileUpload(ByVal iESNr As Integer, _
	'	'                    ByVal strFullFileName As String) Implements IDotNet4Class.runSPFileUpload
	'	'  Dim o2Open As New spFileUploadUtil.ClsMain_Net

	'	'  Try
	'	'    o2Open.StartWithOutput(iESNr, strFullFileName)

	'	'  Catch ex As Exception
	'	'    MsgBox(ex.StackTrace & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical, "runSPFileUpload")

	'	'  End Try

	'	'End Sub

	'	' Telefonieren...
	'	Sub RunTapi(ByVal strNumber As String, _
	'							ByVal iMANr As Integer, _
	'							ByVal iKDNr As Integer, ByVal iKDZhdNr As Integer, _
	'							Optional ByVal iModulNr As Short = 0, _
	'							Optional ByVal iRecID As Integer = 0) Implements IDotNet4Class.RunTapi
	'		Dim strTranslationProgName As String = String.Empty
	'		Dim iTest As Integer = 0

	'		strTranslationProgName = _ClsSettingPath.GetPersonalFolder() & "SPTranslationProg" & _ClsSettingPath.GetLogedUSNr()
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgName", "Now", "SPSTapi.ClsMain_Net")
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_1", iMANr.ToString)
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_2", iKDNr.ToString)
	'		_ClsReg.SetINIString(strTranslationProgName, "ProgParam", "Param_3", iKDZhdNr.ToString)

	'		Try
	'			Dim oMyProg As New SPSTapi.ClsMain_Net
	'			iTest = oMyProg.ShowfrmTapi(strNumber, iMANr, iKDNr, iKDZhdNr, iTest, iModulNr, iRecID)


	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.StackTrace & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical, "RunTapi")

	'		End Try

	'	End Sub

	'	Function OpenfrmMAPhoto(ByVal iMANr As Integer, _
	'													ByVal strFileName As String) As String Implements IDotNet4Class.OpenfrmMAPhoto
	'		Dim strResult As String = String.Empty
	'		Dim o2Open As New SpImageProcess.ClsMain_Net

	'		Try
	'			strResult = o2Open.ShowfrmMAPhoto(iMANr, strFileName)


	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			strResult = String.Format("***Fehler: {0}", ex.Message)

	'		End Try

	'		Return strResult
	'	End Function

	'	' Addresse der Quellensteuerämter...
	'	Sub OpenQSTAddressForm(ByVal iMDNr As Integer, iYear As Integer, ByVal iLogedUSNr As Integer, ByVal Customer_ID As String) Implements IDotNet4Class.OpenQSTAddressForm
	'		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'		Dim obj As New SPS.MD.QstAddressUtility.ClsMain_Net(New SPS.MD.QstAddressUtility.ClsSetting With {.SelectedMDNr = iMDNr,
	'																																															.SelectedMDYear = iYear,
	'																																															.SelectedMDGuid = String.Empty,
	'																																															.LogedUSNr = iLogedUSNr})
	'		Try
	'			obj.ShowfrmQstAddress()

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.StackTrace & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical, "OpenQSTAddressForm")

	'		End Try

	'	End Sub

	'	' Daten für Banken in Mandantenverwaltung...
	'	Sub OpenMDBankDataForm(ByVal iMDNr As Integer, ByVal iYear As Integer, ByVal iLogedUSNr As Integer, ByVal Customer_ID As String) Implements IDotNet4Class.OpenMDBankDataForm
	'		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	'		Dim obj As New SPS.MD.ESRDTAUtility.ClsMain_Net(New SPS.MD.ESRDTAUtility.ClsSetting With {.SelectedMDNr = iMDNr,
	'																																															.SelectedMDYear = iYear,
	'																																															.SelectedMDGuid = String.Empty,
	'																																															.LogedUSNr = iLogedUSNr})
	'		Try
	'			obj.ShowMDfrmBankData()

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.StackTrace & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical, strMethodeName)

	'		End Try

	'	End Sub

	'	Sub OpenCallHistoryForm(ByVal iMDNr As Integer, ByVal iYear As Integer, ByVal iLogedUSNr As Integer, ByVal strCustomer_ID As String) Implements IDotNet4Class.OpenCallHistoryForm
	'		Dim o2Open As New SPCallHistory.ClsMain_Net(New SPCallHistory.ClsSetting With {.SelectedMDNr = iMDNr,
	'																																															.SelectedMDYear = iYear,
	'																																															.SelectedMDGuid = String.Empty,
	'																																															.LogedUSNr = iLogedUSNr})

	'		Try
	'			o2Open.ShowfrmCallHistory()

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.Message, MsgBoxStyle.Critical, "OpenCallHistoryForm")

	'		End Try

	'	End Sub

	'	Sub OpenTarifCalculator() Implements IDotNet4Class.OpenTarifCalculator
	'		Dim o2Open As New SPTarifCalculator.ClsMain_Net

	'		Try
	'			o2Open.ShowfrmTarifCalculator()

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.Message, MsgBoxStyle.Critical, "OpenTarifCalculator")

	'		End Try

	'	End Sub

	'	Sub OpenMAAddressForm(ByVal iMANr As Integer) Implements IDotNet4Class.OpenMAAddressForm
	'		Dim o2Open As New SPMAAddress.ClsMain_Net

	'		Try
	'			o2Open.ShowfrmMAAddress(iMANr)

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.Message, MsgBoxStyle.Critical, "OpenMAAddressForm")

	'		End Try

	'	End Sub

	Function OpenQualificationForm(ByVal bSelectMulti As Boolean, _
																 ByVal strsex As String) As String Implements IDotNet4Class.OpenQualificationForm
		Dim o2Open As New SPQualicationUtility.ClsMain_Net
		Dim strResult As String = String.Empty

		Try
			strResult = o2Open.ShowfrmQualifications(bSelectMulti, strsex)

		Catch ex As Exception
			MsgBox(ex.StackTrace & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical, "OpenQualificationForm")

		End Try

		Return strResult
	End Function

	'	Function OpenBranchesForm(ByVal bSelectMulti As Boolean) As String Implements IDotNet4Class.OpenBranchesForm
	'		Dim o2Open As New SPQualicationUtility.ClsMain_Net
	'		Dim strResult As String = String.Empty

	'		Try
	'			strResult = o2Open.ShowfrmBranches(bSelectMulti)

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.StackTrace & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical, "OpenBranchesForm")

	'		End Try

	'		Return strResult
	'	End Function

	'	Sub OpenMyCockPit() Implements IDotNet4Class.OpenMyCockPit
	'		Dim strCockpitProg As String = _ClsSettingPath.GetLocalBinnPath & "SPMy.Cockpit.exe"

	'		Try
	'			If _ClsSettingPath.IsProcessRunning("SPMy.Cockpit") Then _ClsSettingPath.TerminateSelectedProcess("SPMy.Cockpit")

	'			Dim startInfo As New ProcessStartInfo(strCockpitProg)
	'			Process.Start(startInfo)

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.Message, MsgBoxStyle.Critical, "OpenMyCockPit")

	'		End Try

	'	End Sub

	'	Sub OpenProposal(ByVal iMDNr As Integer, ByVal iYear As Integer, ByVal iLogedUSNr As Integer, ByVal strCustomer_ID As String, ByVal iProposeNr As Integer, _
	'								 ByVal iMANr As Integer, _
	'								 ByVal iKDNr As Integer, _
	'								 ByVal iVakNr As Integer) Implements IDotNet4Class.OpenProposal
	'		'Dim o2Open As New SPProposeUtility.ClsMain_Net(New SPProposeUtility.ClsSetting With {.SelectedMDNr = iMDNr,
	'		'                                                                                     .SelectedMDYear = iYear,
	'		'                                                                                     .SelectedMDGuid = strCustomer_ID,
	'		'                                                                                     .LogedUSNr = iLogedUSNr})

	'		'logger.Debug(String.Format("iProposeNr: {0} | imanr: {1} | iKDNr: {2} | ivaknr: {3}", iProposeNr, iMANr, iKDNr, iVakNr))
	'		'Try
	'		'  If iProposeNr > 0 Then
	'		'    o2Open.ShowfrmProposal(iProposeNr)

	'		'  Else
	'		'    Dim employeeNumber As Integer?
	'		'    Dim customerNumber As Integer?
	'		'    Dim vacancyNumber As Integer?

	'		'    If iMANr = 0 Then
	'		'      employeeNumber = Nothing
	'		'    Else
	'		'      employeeNumber = iMANr
	'		'    End If

	'		'    If iKDNr = 0 Then
	'		'      customerNumber = Nothing
	'		'    Else
	'		'      customerNumber = iKDNr
	'		'    End If

	'		'    If iVakNr = 0 Then
	'		'      vacancyNumber = Nothing
	'		'    Else
	'		'      vacancyNumber = iVakNr
	'		'    End If

	'		'    logger.Debug(String.Format("iProposeNr: {0} | employeeNumber: {1} | customerNumber: {2} | vacancyNumber: {3}", iProposeNr, employeeNumber, customerNumber, vacancyNumber))

	'		'    o2Open.ShowfrmProposal(employeeNumber, customerNumber, vacancyNumber)
	'		'  End If


	'		'Catch ex As Exception
	'		'  logger.Error(ex.ToString)
	'		'  MsgBox(ex.Message, MsgBoxStyle.Critical, "Translate.OpenProposal")

	'		'End Try

	'	End Sub

	'	' Jahresübergang eines Mandanten...
	'	Sub OpenCreateNewMDYearForm(ByVal iMDNr As Integer, iYear As Integer, ByVal iLogedUSNr As Integer, ByVal Customer_ID As String) Implements IDotNet4Class.OpenCreateNewMDYearForm
	'		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'		Dim obj As New SPS.MD.CreateNewUtility.ClsMain_Net(New SPS.MD.CreateNewUtility.ClsSetting With {.SelectedMDNr = iMDNr,
	'																																							 .SelectedMDYear = iYear,
	'																																							 .SelectedMDGuid = String.Empty,
	'																																							.LogedUSNr = iLogedUSNr})
	'		Try
	'			obj.ShowfrmCreateNewYear()

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.StackTrace & vbNewLine & vbNewLine & ex.Message, MsgBoxStyle.Critical, strMethodeName)

	'		End Try

	'	End Sub

	'	''' <summary>
	'	''' Startet das FileWatcher auf und überwacht die Änderungen an geöffneten Dokumenten in Kandidatenverwaltung
	'	''' </summary>
	'	''' <param name="iRecNr"></param>
	'	''' <param name="iMANr"></param>
	'	''' <param name="strPath2Watch"></param>
	'	''' <param name="strFileToWatch"></param>
	'	''' <remarks></remarks>
	'	Sub OpenFileWatcher(ByVal iRecNr As Integer, ByVal iMANr As Integer, _
	'										ByVal strPath2Watch As String, _
	'										ByVal strFileToWatch As String) Implements IDotNet4Class.OpenFileWatcher
	'		Dim o2Open As New SPProgWatcher.ClsMain_Net

	'		Try
	'			o2Open.StartFileWatching(iRecNr, iMANr, strPath2Watch, strFileToWatch)

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.Message, MsgBoxStyle.Critical, "OpenFileWatcher")

	'		End Try

	'	End Sub

	'	Function StoreSelectedOfferDocsToFs(ByVal strRecNr As String) As String() Implements IDotNet4Class.StoreSelectedOfferDocsToFs
	'		Dim _ClsLog As New ClsEventLog
	'		Dim Conn As New SqlConnection(_ClsSettingPath.GetConnString)
	'		Dim strFullFilename As String() = New String() {""}
	'		Dim strFiles As String = String.Empty

	'		Dim bCreateFile As Boolean = False
	'		Dim sOffDocSql As String = "Select DocScan, MANr, Bezeichnung, ScanExtension From OFF_Doc Where "
	'		sOffDocSql &= String.Format("RecNr In ({0})", strRecNr)

	'		Dim i As Integer = 0

	'		Conn.Open()
	'		Dim SQLCmd As SqlCommand = New SqlCommand(sOffDocSql, Conn)
	'		Dim rOfDoc As SqlDataReader = SQLCmd.ExecuteReader

	'		Try
	'			While rOfDoc.Read
	'				Dim strSelectedFile As String = _ClsSettingPath.GetPersonalFolder & _
	'																							System.IO.Path.GetFileName(rOfDoc("Bezeichnung").ToString) & _
	'																							If(rOfDoc("Bezeichnung").ToString.Contains("\"), "", "." & _
	'																								 rOfDoc("ScanExtension").ToString)
	'				strFiles &= If(strFiles <> String.Empty, ";", "") & _
	'										_ClsSettingPath.GetPersonalFolder & System.IO.Path.GetFileName(rOfDoc("Bezeichnung").ToString) & _
	'										If(rOfDoc("Bezeichnung").ToString.Contains("\"), "", "." & rOfDoc("ScanExtension").ToString)

	'				Try
	'					Dim BA As Byte() = Nothing
	'					BA = CType(rOfDoc("DocScan"), Byte())

	'					Dim ArraySize As New Integer
	'					ArraySize = BA.GetUpperBound(0)

	'					If File.Exists(strSelectedFile) Then File.Delete(strSelectedFile)
	'					Dim fs As New FileStream(strSelectedFile, FileMode.CreateNew)
	'					fs.Write(BA, 0, ArraySize + 1)
	'					fs.Close()
	'					fs.Dispose()

	'					i += 1

	'				Catch ex As Exception
	'					logger.Error(ex.ToString)
	'					_ClsLog.WriteTempLogFile(String.Format("***StoreDataToFs_1: {0}", ex.Message))

	'				End Try


	'			End While

	'			ReDim strFullFilename(i - 1)
	'			strFullFilename = strFiles.Split(CChar(";"))

	'			rOfDoc.Close()

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			_ClsLog.WriteTempLogFile(String.Format("***StoreDataToFs_2: {0}", ex.Message))

	'		End Try

	'		Return strFullFilename
	'	End Function

	'	Function StoreSelectedMAPhoto2FS(ByVal iMANr As Integer) As String Implements IDotNet4Class.StoreSelectedMAPhoto2FS
	'		If iMANr = 0 Then Return String.Empty

	'		Dim _ClsLog As New ClsEventLog
	'		Dim Conn As New SqlConnection(_ClsSettingPath.GetConnString)
	'		Dim strFullFilename As String = String.Empty
	'		Dim strFiles As String = String.Empty
	'		Dim BA As Byte() = Nothing
	'		Dim sMASql As String = "Select MABild, MANr From Mitarbeiter Where "
	'		sMASql &= String.Format("MANr = {0} And MABild Is Not Null", iMANr)

	'		Dim i As Integer = 0

	'		Conn.Open()
	'		Dim SQLCmd As SqlCommand = New SqlCommand(sMASql, Conn)
	'		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sMASql, Conn)

	'		Try

	'			strFullFilename = String.Format("{0}Bild_{1}_{2}.JPG", _ClsSettingPath.GetSpSBildFiles2DeletePath, _
	'																			 iMANr, System.Guid.NewGuid.ToString())

	'			Try
	'				Try
	'					BA = CType(SQLCmd_1.ExecuteScalar, Byte())
	'				Catch ex As Exception

	'				End Try
	'				If BA Is Nothing Then Return String.Empty

	'				Dim ArraySize As New Integer
	'				ArraySize = BA.GetUpperBound(0)

	'				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
	'				Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
	'				fs.Write(BA, 0, ArraySize + 1)
	'				fs.Close()
	'				fs.Dispose()

	'				i += 1

	'			Catch ex As Exception
	'				logger.Error(ex.ToString)
	'				MsgBox(String.Format("Fehler: {0}", ex.Message), MsgBoxStyle.Critical, "GetMAPicture")
	'				strFullFilename = String.Empty

	'			End Try


	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			strFullFilename = String.Empty

	'		End Try

	'		Return strFullFilename
	'	End Function

	'	Function SaveFileIntoRPDb(ByVal iRecNr As Integer, ByVal iRPNr As Integer, _
	'									 ByVal strRecBez As String, ByVal strRecBeschreibung As String, _
	'									 ByVal strDocGuid As String, ByVal strFileToSave As String, _
	'									 ByVal strTblName As String) As String Implements IDotNet4Class.SaveFileIntoRPDb
	'		Dim _ClsLog As New ClsEventLog
	'		Dim Time_1 As Double = System.Environment.TickCount
	'		Dim strUSName As String = _ClsSettingPath.GetUserName()
	'		Dim Conn As New SqlConnection(_ClsSettingPath.GetConnString)
	'		Dim strLogFileName As String = _ClsSettingPath.GetProzessLOGFile()
	'		Dim sSql As String = String.Empty
	'		Dim strResult As String = String.Empty

	'		sSql = String.Format("Update {0} Set DocScan = @BinaryFile, ScanExtension = @ScanExtension, ", strTblName)
	'		sSql &= "RPDoc_Guid = @DocGuid Where RecNr = @RecNr And RPNr = @RPNr"

	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
	'		Dim param As System.Data.SqlClient.SqlParameter

	'		Try
	'			Conn.Open()
	'			cmd.Connection = Conn

	'			If strFileToSave <> String.Empty Then
	'				Dim myFile() As Byte = GetFileToByte(strFileToSave)
	'				Dim fi As New System.IO.FileInfo(strFileToSave)
	'				Dim strFileExtension As String = fi.Extension

	'				Try
	'					cmd.CommandType = CommandType.Text
	'					cmd.CommandText = sSql

	'					strFileExtension = strFileExtension.Replace(".", String.Empty)
	'					param = cmd.Parameters.AddWithValue("@BinaryFile", myFile)
	'					param = cmd.Parameters.AddWithValue("@ScanExtension", strFileExtension)
	'					param = cmd.Parameters.AddWithValue("@DocGuid", strDocGuid)
	'					param = cmd.Parameters.AddWithValue("@RecNr", iRecNr)
	'					param = cmd.Parameters.AddWithValue("@RPNr", iRPNr)

	'					cmd.Connection = Conn
	'					cmd.ExecuteNonQuery()

	'					cmd.Parameters.Clear()
	'					strResult = "Erfolgreich..."

	'				Catch ex As Exception
	'					strResult = String.Format("***Fehler (SaveFileIntoDb_1): {0}", ex.Message)
	'					logger.Error(ex.ToString)

	'				End Try
	'			End If
	'			_ClsLog.WriteTempLogFile(String.Format("Erfolgreich: SaveFileIntoDb: " & _
	'																						 "RecNr: {0} / MANr: {1} / strFilename: {2}", _
	'																						 iRecNr, iRPNr, strFileToSave), strLogFileName)

	'		Catch ex As Exception
	'			strResult = String.Format("***Fehler (SaveFileIntoDb_2): {0}", ex.Message)
	'			logger.Error(ex.ToString)

	'		Finally
	'			cmd.Dispose()
	'			Conn.Close()

	'		End Try

	'		Dim Time_2 As Double = System.Environment.TickCount
	'		Console.WriteLine("Zeit für SaveFileIntoDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'		Return strResult
	'	End Function

	'	Function SaveFileIntoDb(ByVal iRecNr As Integer, ByVal iMANr As Integer, _
	'										 ByVal strRecBez As String, ByVal strRecBeschreibung As String, _
	'										 ByVal strDocPath As String, ByVal strFileToSave As String, _
	'										 ByVal strTblName As String) As String Implements IDotNet4Class.SaveFileIntoDb
	'		Dim _ClsLog As New ClsEventLog
	'		Dim Time_1 As Double = System.Environment.TickCount
	'		Dim strUSName As String = _ClsSettingPath.GetUserName()
	'		Dim Conn As New SqlConnection(_ClsSettingPath.GetConnString)
	'		Dim strLogFileName As String = _ClsSettingPath.GetProzessLOGFile()
	'		Dim sSql As String = String.Empty
	'		Dim strResult As String = String.Empty

	'		sSql = String.Format("Update {0} Set DocScan = @BinaryFile, ScanExtension = @ScanExtension ", strTblName)
	'		sSql &= "Where RecNr = @RecNr And MANr = @MANr"

	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
	'		Dim param As System.Data.SqlClient.SqlParameter

	'		Try
	'			Conn.Open()
	'			cmd.Connection = Conn

	'			If strFileToSave <> String.Empty Then
	'				Dim myFile() As Byte = GetFileToByte(strFileToSave)
	'				Dim fi As New System.IO.FileInfo(strFileToSave)
	'				Dim strFileExtension As String = fi.Extension

	'				Try
	'					cmd.CommandType = CommandType.Text
	'					cmd.CommandText = sSql

	'					strFileExtension = strFileExtension.Replace(".", String.Empty)
	'					param = cmd.Parameters.AddWithValue("@BinaryFile", myFile)
	'					param = cmd.Parameters.AddWithValue("@ScanExtension", strFileExtension)
	'					param = cmd.Parameters.AddWithValue("@RecNr", iRecNr)
	'					param = cmd.Parameters.AddWithValue("@MANr", iMANr)

	'					cmd.Connection = Conn
	'					cmd.ExecuteNonQuery()

	'					cmd.Parameters.Clear()
	'					strResult = "Erfolgreich..."

	'				Catch ex As Exception
	'					strResult = String.Format("***Fehler (SaveFileIntoDb_1): {0}", ex.Message)
	'					logger.Error(ex.ToString)

	'				End Try
	'			End If
	'			_ClsLog.WriteTempLogFile(String.Format("Erfolgreich: SaveFileIntoDb: " & _
	'																						 "RecNr: {0} / MANr: {1} / strFilename: {2}", _
	'																						 iRecNr, iMANr, strFileToSave), strLogFileName)

	'		Catch ex As Exception
	'			strResult = String.Format("***Fehler (SaveFileIntoDb_2): {0}", ex.Message)
	'			logger.Error(ex.ToString)

	'		Finally
	'			cmd.Dispose()
	'			Conn.Close()

	'		End Try

	'		Dim Time_2 As Double = System.Environment.TickCount
	'		Console.WriteLine("Zeit für SaveFileIntoDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'		Return strResult
	'	End Function

	'	Function SaveFileIntoKDDb(ByVal iRecNr As Integer, ByVal iKDNr As Integer, _
	'									 ByVal strRecBez As String, ByVal strRecBeschreibung As String, _
	'									 ByVal strDocPath As String, ByVal strFileToSave As String, _
	'									 ByVal strTblName As String) As String Implements IDotNet4Class.SaveFileIntoKDDb
	'		Dim _ClsLog As New ClsEventLog
	'		Dim Time_1 As Double = System.Environment.TickCount
	'		Dim strUSName As String = _ClsSettingPath.GetUserName()
	'		Dim Conn As New SqlConnection(_ClsSettingPath.GetConnString)
	'		Dim strLogFileName As String = _ClsSettingPath.GetProzessLOGFile()
	'		Dim sSql As String = String.Empty
	'		Dim strResult As String = String.Empty

	'		sSql = String.Format("Update {0} Set DocScan = @BinaryFile, ScanExtension = @ScanExtension ", strTblName)
	'		sSql &= "Where RecNr = @RecNr And KDNr = @KDNr"

	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
	'		Dim param As System.Data.SqlClient.SqlParameter

	'		Try
	'			Conn.Open()
	'			cmd.Connection = Conn

	'			If strFileToSave <> String.Empty Then
	'				Dim myFile() As Byte = GetFileToByte(strFileToSave)
	'				Dim fi As New System.IO.FileInfo(strFileToSave)
	'				Dim strFileExtension As String = fi.Extension

	'				Try
	'					cmd.CommandType = CommandType.Text
	'					cmd.CommandText = sSql

	'					param = cmd.Parameters.AddWithValue("@BinaryFile", myFile)
	'					param = cmd.Parameters.AddWithValue("@ScanExtension", strFileExtension)
	'					param = cmd.Parameters.AddWithValue("@RecNr", iRecNr)
	'					param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)

	'					cmd.Connection = Conn
	'					cmd.ExecuteNonQuery()

	'					cmd.Parameters.Clear()
	'					strResult = "Erfolgreich..."


	'				Catch ex As Exception
	'					strResult = String.Format("***Fehler (SaveFileIntoKDDb_1): {0}", ex.Message)
	'					logger.Error(ex.ToString)

	'				End Try
	'			End If

	'			_ClsLog.WriteTempLogFile(String.Format("Erfolgreich: SaveFileIntoKDDb: " & _
	'																						 "RecNr: {0} / MANr: {1} / strFilename: {2}", _
	'																						 iRecNr, iKDNr, strFileToSave), strLogFileName)

	'		Catch ex As Exception
	'			strResult = String.Format("***Fehler (SaveFileIntoKDDb_2): {0}", ex.Message)
	'			logger.Error(ex.ToString)

	'		Finally
	'			cmd.Dispose()
	'			Conn.Close()

	'		End Try

	'		Dim Time_2 As Double = System.Environment.TickCount
	'		Console.WriteLine("Zeit für SaveFileIntoKDDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'		Return strResult
	'	End Function

	'	Function SavePrintedDocToDb(ByVal iMANr As Integer, _
	'												 ByVal strDocArt As String, _
	'												 ByVal strDocBeschreibung As String, _
	'												 ByVal strFullFileToSave As String, _
	'												 ByVal strTblName As String) As String Implements IDotNet4Class.SavePrintedDocToDb
	'		Dim strValue As String = "Erfolgreich"
	'		Dim _ClsLog As New ClsEventLog
	'		Dim Time_1 As Double = System.Environment.TickCount
	'		Dim strUSName As String = _ClsSettingPath.GetUserName()
	'		Dim Conn As New SqlConnection(_ClsSettingPath.GetConnString)
	'		Dim strLogFileName As String = _ClsSettingPath.GetProzessLOGFile()
	'		Dim sSql As String = String.Empty

	'		sSql = String.Format("Insert Into {0} (MANr, DocName, UserName, CreatedOn, ScanDoc) Values ", strTblName)
	'		'DocScan = @BinaryFile, ScanExtension = @ScanExtension ", strTblName)
	'		sSql &= "(@MANr, @DocName, @UserName, @CreatedOn, @ScanDoc)"

	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
	'		Dim param As System.Data.SqlClient.SqlParameter

	'		Try
	'			Conn.Open()
	'			cmd.Connection = Conn

	'			If strFullFileToSave <> String.Empty Then
	'				Dim myFile() As Byte = GetFileToByte(strFullFileToSave)
	'				Dim fi As New System.IO.FileInfo(strFullFileToSave)
	'				Dim strFileExtension As String = fi.Extension

	'				Try
	'					cmd.CommandType = CommandType.Text
	'					cmd.CommandText = sSql

	'					param = cmd.Parameters.AddWithValue("@MANr", iMANr)
	'					param = cmd.Parameters.AddWithValue("@DocName", String.Format("{0} {1}", strDocArt, strDocBeschreibung).Trim)
	'					param = cmd.Parameters.AddWithValue("@UserName", String.Format("{0} {1}", _
	'																																				 _ClsSettingPath.GetUserFName, _
	'																																				 _ClsSettingPath.GetUserLName).Trim)
	'					param = cmd.Parameters.AddWithValue("@CreatedOn", Now.ToString("G"))

	'					param = cmd.Parameters.AddWithValue("@ScanDoc", myFile)

	'					cmd.Connection = Conn
	'					cmd.ExecuteNonQuery()

	'					cmd.Parameters.Clear()

	'				Catch ex As Exception
	'					logger.Error(ex.ToString)
	'					strValue = String.Format("***SavePrintedDocToDb_1: {0}", ex.Message)

	'				End Try
	'			End If

	'			_ClsLog.WriteTempLogFile(String.Format("Erfolgreich: SavePrintedDocToDb: " & _
	'																						 "MANr: {0} / DocName: {1} / strFilename: {2}", _
	'																						 iMANr, strDocArt, strFullFileToSave), strLogFileName)

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			strValue = String.Format("***SaveFileIntoDb_2: {0}", ex.Message)

	'		Finally
	'			cmd.Dispose()
	'			Conn.Close()

	'		End Try

	'		Dim Time_2 As Double = System.Environment.TickCount
	'		Console.WriteLine("Zeit für SavePrintedDocToDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'		Return strValue
	'	End Function

	'	Function Image2ByteArray(ByVal Bild As Image, _
	'													 ByVal Bildformat As System.Drawing.Imaging.ImageFormat) As Byte() _
	'												 Implements IDotNet4Class.Image2ByteArray
	'		Dim MS As New IO.MemoryStream

	'		Bild.Save(MS, Bildformat)
	'		MS.Flush()

	'		Return MS.ToArray
	'	End Function

	'	Function GetFileToByte(ByVal filePath As String) As Byte() Implements IDotNet4Class.GetFileToByte
	'		Dim stream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
	'		Dim reader As BinaryReader = New BinaryReader(stream)
	'		Dim _ClsLog As New ClsEventLog

	'		Dim photo() As Byte = Nothing
	'		Try

	'			photo = reader.ReadBytes(CInt(stream.Length))
	'			reader.Close()
	'			stream.Close()

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)

	'		End Try

	'		Return photo
	'	End Function

	'	Function GetMDGuid() As String Implements IDotNet4Class.GetMDGuid
	'		Dim _ClsLog As New ClsEventLog
	'		Dim Conn As New SqlConnection(_ClsSettingPath.GetConnString)
	'		Dim strResult As String = String.Empty

	'		Dim bCreateFile As Boolean = False
	'		Dim sOffDocSql As String = "Select Customer_ID From Mandanten Where "
	'		sOffDocSql &= "Jahr = @MDJahr"

	'		Dim i As Integer = 0

	'		Conn.Open()
	'		Dim SQLCmd As SqlCommand = New SqlCommand(sOffDocSql, Conn)
	'		Dim rMDrec As SqlDataReader = SQLCmd.ExecuteReader
	'		'Dim param As System.Data.SqlClient.SqlParameter

	'		Try
	'			While rMDrec.Read
	'				Try
	'					strResult = CType(rMDrec("Customer_ID"), String)

	'				Catch ex As Exception
	'					logger.Error(ex.ToString)

	'				End Try

	'			End While
	'			rMDrec.Close()

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			'_ClsLog.WriteTempLogFile(String.Format("***GetMDGuid_2: {0}", ex.Message))

	'		End Try

	'		Return strResult
	'	End Function

	'	' GAV-Adressen
	'	Sub OpenGAVAddressForm(ByVal iMDNr As Integer, iYear As Integer, ByVal iLogedUSNr As Integer, ByVal Customer_ID As String) Implements IDotNet4Class.OpenGAVAddressForm
	'		Dim obj As New SPS.MD.GAVAddressUtility.ClsMain_Net(New SPS.MD.GAVAddressUtility.ClsSetting With {.SelectedMDNr = iMDNr,
	'																																																			.SelectedMDYear = iYear,
	'																																																			.SelectedMDGuid = Customer_ID,
	'																																																			.LogedUSNr = iLogedUSNr})
	'		Try
	'			obj.ShowfrmGAVAddress()

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.Message, MsgBoxStyle.Critical, "T_OpenGAVAddressForm")

	'		End Try

	'	End Sub

	'	' GAV-Maske
	'	Function OpenGAVDataForm(ByVal strModulName As String, _
	'													 ByVal iMANr As Integer, _
	'													 ByVal iKDNr As Integer, _
	'													 ByVal iESNr As Integer, _
	'													 ByVal strKanton As String, _
	'													 ByVal strOldGAVInfo As String) As String Implements IDotNet4Class.OpenGAVDataForm
	'		Dim o2Open As New SPGAV.ClsMain_Net
	'		Dim strResult As String = String.Empty

	'		Try
	'			strResult = o2Open.ShowfrmPVLDialog(strModulName, iMANr, iKDNr, iESNr, strKanton, strOldGAVInfo)

	'		Catch ex As Exception
	'			logger.Error(ex.ToString)
	'			MsgBox(ex.Message, MsgBoxStyle.Critical, "T_OpenGAVDataForm")

	'		End Try

	'		Return strResult
	'	End Function

	''' <summary>
	''' Open Customer form for new and existing record
	''' </summary>
	''' <param name="iKDNr"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function OpenCustomerForm(ByVal iMDNr As Integer, ByVal iYear As Integer, ByVal iLogedUSNr As Integer, ByVal strCustomer_ID As String, ByVal iKDNr As Integer) As String Implements IDotNet4Class.OpenCustomerForm

	End Function

	''' <summary>
	''' opens responsible person of given customer
	''' </summary>
	''' <param name="iKDNr"></param>
	''' <param name="iZHDNr"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function OpenResponsibleCustomerForm(ByVal iKDNr As Integer, ByVal iZHDNr As Integer) As String Implements IDotNet4Class.OpenResponsibleCustomerForm

	End Function



#Region "Rechnungsverwaltung"

	' ''' <summary>
	' ''' open invoice form for given customer
	' ''' </summary>
	' ''' <param name="iCustomerNr"></param>
	' ''' <returns></returns>
	' ''' <remarks></remarks>
	'Function OpenInvoiceForm(ByVal iCustomerNr As Integer) As String Implements IDotNet4Class.OpenInvoiceFormOld
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strResult As String = "success"

	'	Try
	'		Dim adr = New frmInvoiceAddress(CreateInitialData(0, 0))
	'		If iCustomerNr > 0 Then
	'			If (adr.LoadCustomerInvoiceAddresses(iCustomerNr, Nothing)) Then
	'				adr.Show()
	'			End If
	'		End If

	'	Catch ex As Exception
	'		logger.Error(String.Format("{0}:{1}", strMethodeName, ex.Message))
	'		strResult = String.Format("Error: {1}", ex.Message)

	'	End Try

	'	Return strResult
	'End Function

	'Function OpenInvoiceForm(ByVal iMDNr As Integer, ByVal iYear As Integer, ByVal iLogedUSNr As Integer, ByVal strCustomer_ID As String, ByVal iInvoiceNr As Integer) As String Implements IDotNet4Class.OpenInvoiceForm
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strResult As String = "success"

	'	Try

	'		Dim frm As frmInvoices = CType(GetModuleCach(iLogedUSNr).GetModuleForm(iMDNr, iLogedUSNr, SP.ModuleCaching.ModuleName.InvoiceMng), frmInvoices)

	'		If iInvoiceNr > 0 Then
	'			frm.LoadInvoiceData(iInvoiceNr)

	'			If frm.IsInvoiceDataLoaded Then
	'				frm.Show()
	'				frm.BringToFront()
	'			End If

	'		End If

	'	Catch ex As Exception
	'		logger.Error(String.Format("{0}:{1}", strMethodeName, ex.Message))
	'	End Try


	'	Return strResult
	'End Function

#End Region



	Public Sub New()
		Try


		Catch ex As Exception
			If _ClsSettingPath.GetLogedUSNr = 1 Then MsgBox(ex.Message)

		End Try

	End Sub



End Class
