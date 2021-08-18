
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25

Imports SPS.Listing.Print.Utility.MainUtilities
Imports SPS.Listing.Print.Utility.ESDbDatabases.ClsESDb4Print

Imports SP.Infrastructure.Logging
Imports System.Reflection
Imports SP.Infrastructure.UI


Public Class ClsLLESTemplatePrint

	Implements IDisposable
	Protected disposed As Boolean = False

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	Private m_ProgPath As New ClsProgPath
	Private m_mandant As New Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities

	Private _ESVertragSetting As New ClsLLESTemplateSetting

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsLog As New SPProgUtility.ClsEventLog
	Friend _ClsLLFunc As New ClsLLFunc

	Private strConnString As String = String.Empty
	Private strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml
	Private m_MandantXMLFile As String
	Private m_InvoiceSetting As String
	Private m_MandantFormXMLFile As String
	Private m_ESUnterzeichner As Boolean

	Private Property USSignFileName As String
	Private Property ExistsDocFile As Boolean

	Private LL As ListLabel = New ListLabel


#Region "private consts"

	Private Const MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING As String = "MD_{0}/Debitoren"
	Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


#Region "Private Properties"

	Private ReadOnly Property GetYearMwSt(ByVal iYear As Integer) As Single
		Get
			Dim mdNumber = _ESVertragSetting.SelectedMDNr
			If iYear = 0 Then iYear = Now.Year
			Dim FORM_XML_MAIN_KEY As String = String.Format("MD_{0}/Debitoren", mdNumber)
			Dim mwstsatz As String = m_ProgPath.GetXMLNodeValue(m_mandant.GetSelectedMDDataXMLFilename(mdNumber, iYear), String.Format("{0}/mwstsatz", FORM_XML_MAIN_KEY))

			If String.IsNullOrWhiteSpace(mwstsatz) Then
				Return 8
			End If

			Return mwstsatz

		End Get
	End Property

	Private ReadOnly Property GetESEndeByNull() As String
		Get
			Dim esendebynull As String = m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/esendebynull", FORM_XML_DEFAULTVALUES_KEY))
			Return If(String.IsNullOrWhiteSpace(esendebynull), "unbestimmt", esendebynull)
		End Get
	End Property

	Private ReadOnly Property GetESUnterzeichner() As Boolean
		Get
			Dim sValue As Boolean = True

			Dim strQuery As String = String.Format("//ExportSetting[@Name={0}SP.ES.PrintUtility{0}]/esunterzeichner_esvertrag", Chr(34))
			Dim strBez As String = _ClsProgSetting.GetXMLNodeValue(m_mandant.GetSelectedMDUserProfileXMLFilename(_ESVertragSetting.SelectedMDNr, _ESVertragSetting.LogedUSNr), strQuery)
			sValue = StrToBool(strBez)

			Return sValue
		End Get

	End Property

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As ClsLLESTemplateSetting)
		Dim strOrgText As New List(Of String)
		Dim strNewText As New List(Of String)

		m_UtilityUI = New UtilityUI
		m_ProgPath = New ClsProgPath
		m_mandant = New Mandant

		m_InvoiceSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_INVOICE_SETTING, _setting.SelectedMDNr)
		m_MandantFormXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(_setting.SelectedMDNr)
		m_MandantXMLFile = m_mandant.GetSelectedMDDataXMLFilename(_setting.SelectedMDNr, _setting.SelectedYear)
		If Not System.IO.File.Exists(m_MandantXMLFile) Then
			m_UtilityUI.ShowErrorDialog(String.Format("Die Einstellung-Datei wurde nicht gefunden.{0}{1}", vbNewLine, m_MandantXMLFile))

		Else
			m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
		End If
		m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)

		Try
			Me._ESVertragSetting = _setting
			Me.strConnString = _setting.DbConnString2Open
			Me.ExistsDocFile = Utilities.BuildPrintJob(_ESVertragSetting.SelectedMDNr, _ESVertragSetting.DbConnString2Open, _ESVertragSetting.JobNr2Print, Me._ClsLLFunc)

			m_Utility = New SPProgUtility.MainUtilities.Utilities
			m_ESUnterzeichner = GetESUnterzeichner


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Öffnen der Daten von Einsatzlohndaten:{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

#End Region

	Sub ShowInDesign()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim bAllowedtoDebug As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "EnableLLDebug", "0"))
			If bAllowedtoDebug Then
				LL.Debug = LlDebug.LogToFile
				LL.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

		End Try

		If Not Me.ExistsDocFile Then Exit Sub

		Try
			Dim rFoundedrec As SqlDataReader = MainUtilities.OpenDb4PrintESTemplate(_ESVertragSetting.DbConnString2Open, _ESVertragSetting.SelectedESNr2Print)
			If (rFoundedrec Is Nothing) OrElse Not rFoundedrec.HasRows Then Exit Sub

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()
			Me._ESVertragSetting.SelectedMonth = CDate(rFoundedrec("ES_Ab")).Month
			Me._ESVertragSetting.SelectedYear = CDate(rFoundedrec("ES_Ab")).Year
			SetLLVariable(rFoundedrec)

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If

			LL.Core.LlDefineLayout(CType(0, IntPtr), _ClsLLFunc.LLDocLabel, _
														 If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card), _
														 _ClsLLFunc.LLDocName)

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException. ESNr: {0} | MANr: {1}: {2}:{3}", _
																 Me._ESVertragSetting.SelectedESNr2Print, _
																 Me._ESVertragSetting.SelectedMANr2Print, _
																 strMethodeName, LlException.Message))

		Finally

		End Try

	End Sub

	Function ShowInPrint(ByRef bShowBox As Boolean) As String
		Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
		Dim strResult As String = "Success"
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim strJobNr As String = Me._ESVertragSetting.JobNr2Print

		Dim strQuery As String = _ESVertragSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return strResult

		Try
			Dim bAllowedtoDebug As Boolean = CBool(_ClsProgSetting.GetMDProfilValue("Sonstiges", "EnableLLDebug", "0"))
			If bAllowedtoDebug Then
				LL.Debug = LlDebug.LogToFile
				LL.Debug = LlDebug.Enabled
			End If
		Catch ex As Exception
			m_Logger.LogError(String.Format("EnablingLLDebuging.{0}: {1}", strMethodeName, ex))

		End Try

		Try
			Dim rFoundedrec As SqlDataReader = MainUtilities.OpenDb4PrintESTemplate(_ESVertragSetting.DbConnString2Open, _ESVertragSetting.SelectedESNr2Print)
			If (rFoundedrec Is Nothing) OrElse Not rFoundedrec.HasRows Then Return TranslateMyText("Keine Daten wurden gefunden") & "..."

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()
			Me._ESVertragSetting.SelectedMonth = CDate(rFoundedrec("ES_Ab")).Month
			Me._ESVertragSetting.SelectedYear = CDate(rFoundedrec("ES_Ab")).Year

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(rFoundedrec)

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), _
																		 LlProject.List, LlProject.Card), _
																		 _ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, _
																		 CType(0, IntPtr), _ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOption(LlPrintOption.Copies, _ClsLLFunc.LLCopyCount)
			LL.Core.LlPrintSetOption(LL_PRNOPT_COPIES_SUPPORTED, LL.Core.LlPrintGetOption(LlPrintOption.Copies))

			If bShowBox Then
				LL.Core.LlPrintOptionsDialog(CType(0, IntPtr), String.Format("{1}: {2}{0}{3}", _
																																	 vbNewLine, _
																																	 _ClsLLFunc.LLDocLabel, _
																																	 strJobNr, _
																																	 _ClsLLFunc.LLDocName))
			End If
			Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

			While Not LL.Core.LlPrint()
			End While

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				Do
					' pass data for current record
					DefineData(LL, True, rFoundedrec)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData
						LL.Core.LlPrint()
					End While

				Loop While rFoundedrec.Read()

				While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData
				End While
			End If
			Try
				LL.Core.LlPrintEnd(0)
			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.LlPrintEnd...:{1}", strMethodeName, ex.ToString))
			End Try


			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder, CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_ProgPath.GetSpSTempFolder)

				Return "Error"
			End If

			Try
				CType(LL, IDisposable).Dispose()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.LL Disposing...:{1}", strMethodeName, ex.ToString))

			End Try


		Catch LlException As LL_User_Aborted_Exception
			Return "Error"

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException. ESNr: {0} | MANr: {1}: {2}:{3}", _
																 Me._ESVertragSetting.SelectedESNr2Print, _
																 Me._ESVertragSetting.SelectedMANr2Print, _
																 strMethodeName, LlException.Message))
			strResult = String.Format("Error. ESNr: {0} | MANr: {1}: {2}:{3}", _
																 Me._ESVertragSetting.SelectedESNr2Print, _
																 Me._ESVertragSetting.SelectedMANr2Print, _
																 strMethodeName, LlException.Message)

		Finally
			CType(LL, IDisposable).Dispose()

		End Try

		Return strResult
	End Function

	Sub DefineData(ByVal LL As ListLabel, ByVal AsFields As Boolean, ByVal MyDataReader As SqlDataReader)
		Dim i As Integer

		Dim iType As String
		Dim strParam As LlFieldType
		Dim strContent As String

		For i = 0 To MyDataReader.FieldCount - 1
			iType = MyDataReader.GetFieldType(i).ToString.ToUpper

			Select Case iType.ToUpper

				Case "System.Int16".ToUpper, "System.Int32".ToUpper, "System.Int64".ToUpper, "System.Decimal".ToUpper, "System.Double".ToUpper
					strParam = LlFieldType.Numeric
					strContent = MyDataReader.Item(i).ToString & ""

				Case "System.DateTime".ToUpper
					strParam = LlFieldType.Date_OLE

					If IsDBNull(MyDataReader.Item(i)) Then
						strContent = CDate("01.01.1900").ToOADate().ToString() ' , "G")
					Else
						strContent = MyDataReader.GetDateTime(i).ToOADate().ToString()
					End If

				Case "System.Boolean".ToUpper
					strParam = LlFieldType.Boolean
					If IsDBNull(MyDataReader.Item(i)) Then
						strContent = "0"
					Else
						strContent = CStr(IIf(CBool(MyDataReader.Item(i)), 1, 0))
					End If

				Case Else
					strParam = LlFieldType.Text
					If IsDBNull(MyDataReader.Item(i)) Then
						strContent = String.Empty
					Else
						strContent = MyDataReader.Item(i).ToString
					End If

			End Select

			LL.Core.LlDefineFieldExt(MyDataReader.GetName(i), strContent, strParam)
			LL.Core.LlDefineVariableExt(MyDataReader.GetName(i), strContent, strParam)
		Next

	End Sub

	Sub GetMDUSData4Print(ByVal LL As ListLabel)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		GetUSData(_ESVertragSetting.DbConnString2Open, _ClsLLFunc, _ESVertragSetting.SelectedMDNr)
		Try
			With _ClsLLFunc
				LL.Variables.Add("MDName", .USMDname)
				LL.Variables.Add("MDName2", .USMDname2)
				LL.Variables.Add("MDName3", .USMDname3)
				LL.Variables.Add("MDPostfach", .USMDPostfach)
				LL.Variables.Add("MDStrasse", .USMDStrasse)
				LL.Variables.Add("MDPLZ", .USMDPlz)
				LL.Variables.Add("MDOrt", .USMDOrt)
				LL.Variables.Add("MDLand", .USMDLand)

				LL.Variables.Add("MDTelefax", .USMDTelefax)
				LL.Variables.Add("MDTelefon", .USMDTelefon)
				LL.Variables.Add("MDDTelefon", .USMDDTelefon)
				LL.Variables.Add("MDHomepage", .USMDHomepage)
				LL.Variables.Add("MDeMail", .USMDeMail)

				LL.Variables.Add("USNachName", .USNachname)
				LL.Variables.Add("USVorname", .USVorname)

				LL.Variables.Add("USTitle1", .USTitel_1)
				LL.Variables.Add("USTitle2", .USTitel_2)
				LL.Variables.Add("USAbteilung", .USAbteilung)

				' Bewilligungsbehörden
				LL.Variables.Add("BewName", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewName"))
				LL.Variables.Add("BewStr", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewStrasse"))
				LL.Variables.Add("BewOrt", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewPLZOrt"))
				LL.Variables.Add("BewNameAus", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Sonstiges", "BewSeco"))
				LL.Variables.Add("MwStProzent", GetYearMwSt(_ESVertragSetting.SelectedMDYear))

			End With

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub InitLL(ByVal LL As ListLabel)
		Const LL_DLGBOXMODE_3DBUTTONS As Integer = 32768											 ' 0x8000
		Const LL_DLGBOXMODE_ALT10 As Integer = 11															 ' 0x000B
		Const LL_OPTION_INCLUDEFONTDESCENT As Integer = 79										 ' 79
		Const LL_OPTION_INCREMENTAL_PREVIEW As Integer = 135									 ' 135

		Const LL_OPTION_VARSCASESENSITIVE As Integer = 46											 ' 46
		Const LL_OPTION_IMMEDIATELASTPAGE As Integer = 64											 ' 64
		Const LL_OPTION_CONVERTCRLF As Integer = 21														 ' 21

		Const LL_OPTION_NOPARAMETERCHECK As Integer = 32											 ' 32
		Const LL_OPTION_XLATVARNAMES As Integer = 51													 ' 51

		Const LL_OPTION_ESC_CLOSES_PREVIEW As Integer = 102										 ' 102
		Const LL_OPTION_SUPERVISOR As Integer = 3															 ' 3
		Const LL_OPTION_UISTYLE As Integer = 99																 ' 99
		Const LL_OPTION_UISTYLE_OFFICE2003 As Integer = 2											 ' 2
		Const LL_OPTION_AUTOMULTIPAGE As Integer = 42													 ' 42
		Const LL_OPTION_SUPPORTPAGEBREAK As Integer = 10											 ' 10
		Const LL_OPTION_PRVZOOM_PERC As Integer = 25													 ' 25

		LL.LicensingInfo = ClsMainSetting.GetLL25LicenceInfo()
		LL.Language = LlLanguage.German

		LlCore.LlSetDlgboxMode(LL_DLGBOXMODE_3DBUTTONS + LL_DLGBOXMODE_ALT10)

		' beim LL13 muss ich es so machen...
		LL.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, 0)
		LL.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, 0)

		' beim LL13 muss ich es so machen...
		LL.Core.LlSetOption(LL_OPTION_INCLUDEFONTDESCENT, _ClsLLFunc.LLFontDesent)
		LL.Core.LlSetOption(LL_OPTION_INCREMENTAL_PREVIEW, _ClsLLFunc.LLIncPrv)

		LL.Core.LlSetOption(LL_OPTION_VARSCASESENSITIVE, 0)

		LL.Core.LlSetOption(LL_OPTION_IMMEDIATELASTPAGE, 1)				' Lastpage
		LL.Core.LlSetOption(LL_OPTION_CONVERTCRLF, 1)							' Doppelte Zeilenumbruch

		LL.Core.LlSetOption(LL_OPTION_NOPARAMETERCHECK, _ClsLLFunc.LLParamCheck)
		LL.Core.LlSetOption(LL_OPTION_XLATVARNAMES, _ClsLLFunc.LLKonvertName)

		LL.Core.LlSetOption(LL_OPTION_ESC_CLOSES_PREVIEW, 1)
		LL.Core.LlSetOption(LL_OPTION_SUPERVISOR, 1)
		LL.Core.LlSetOption(LL_OPTION_UISTYLE, LL_OPTION_UISTYLE_OFFICE2003)
		LL.Core.LlSetOption(LL_OPTION_AUTOMULTIPAGE, 1)

		LL.Core.LlSetOption(LL_OPTION_SUPPORTPAGEBREAK, 1)
		LL.Core.LlSetOption(LL_OPTION_PRVZOOM_PERC, _ClsLLFunc.LLZoomProz)

		LL.Core.LlPreviewSetTempPath(m_ProgPath.GetSpSTempFolder)
		LL.Core.LlSetPrinterDefaultsDir(m_ProgPath.GetPrinterHomeFolder)

	End Sub

	Sub SetLLVariable(ByVal rFrec As SqlDataReader)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim aValue As New List(Of String)

		Try
			' Mandantendaten drucken...
			GetMDUSData4Print(LL)
			' Zusätzliche Variable einfügen
			LL.Variables.Add("AutorFName", _ClsProgSetting.GetUserFName)
			LL.Variables.Add("AutorLName", _ClsProgSetting.GetUserLName)
			LL.Variables.Add("USSignFilename", Me._ESVertragSetting.USSignFileName)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}. US-Variable auflisten. {1}", strMethodeName, ex.ToString))

		End Try

		With rFrec
			Try
				Dim iKDZustaendigNr As Integer = m_Utility.SafeGetInteger(rFrec, "KDZHDNr", 0)
				Dim KDPostfach As String = rFrec("KDPostfach")
				Dim KDStrasse As String = rFrec("KDStrasse")
				Dim KDPLZOrt As String = rFrec("KDPLZOrt")

				Dim aKst As String() = rFrec("ESKST").ToString.Split(CChar("/"))
				Dim strKst_1 As String = aKst(0)
				Dim strKst_2 As String = aKst(0)
				If aKst.Length > 1 Then strKst_2 = aKst(1).Trim

				Dim KDzAnredeForm As String = m_Utility.SafeGetString(rFrec, "KDzAnredeForm")
				Dim KDzAnrede As String = m_Utility.SafeGetString(rFrec, "KDzAnrede")
				Dim KDzAnrede_Prog As String = m_Utility.SafeGetString(rFrec, "KDzAnrede")
				Dim KDZFName As String = m_Utility.SafeGetString(rFrec, "KDzVorname")
				Dim KDZLName As String = m_Utility.SafeGetString(rFrec, "KDzNachname")

				Dim KDzPostfach = m_Utility.SafeGetString(rFrec, "KDzPostfach")
				Dim KDzStrasse As String = m_Utility.SafeGetString(rFrec, "KDzStrasse")
				Dim KDzPLZOrt As String = String.Format("{0} {1}", m_Utility.SafeGetString(rFrec, "KDzPLZ"), m_Utility.SafeGetString(rFrec, "KDzOrt"))
				Dim strES_EndeDate As String
				Dim esEndeDateWithGoeslonger As String = String.Empty
				Dim isESEndeNull As Boolean

				If Utilities.GetColumnTextStr(rFrec, "ES_Ende", "") = String.Empty Then
					strES_EndeDate = GetESEndeByNull
					esEndeDateWithGoeslonger = m_Utility.SafeGetString(rFrec, "GoesLonger")
					strES_EndeDate = GetSafeTranslationValue(strES_EndeDate, m_Utility.SafeGetString(rFrec, "MASprache").Substring(0, 1).ToUpper)
					isESEndeNull = True

				Else
					strES_EndeDate = String.Format(rFrec("ES_Ende"), "d")
					isESEndeNull = False

				End If
				If String.IsNullOrWhiteSpace(esEndeDateWithGoeslonger) Then esEndeDateWithGoeslonger = strES_EndeDate

				LL.Variables.Add("KDPostfach", KDPostfach)
				LL.Variables.Add("KDStrasse", KDStrasse)
				LL.Variables.Add("KDPLZOrt", KDPLZOrt)

				LL.Variables.Add("KDZNr", iKDZustaendigNr)
				If KDzAnrede_Prog = String.Empty Then
					KDzAnrede_Prog = MainUtilities.TranslateMyText("Sehr geehrter Kunde")
				Else
					KDzAnrede_Prog = String.Format(MainUtilities.TranslateMyText(String.Format("Sehr geehrte{0}", If(KDzAnrede = "Herr", "r", ""))) & " {0}", KDZLName)

				End If
				LL.Variables.Add("KDzAnrede", KDzAnrede)
				LL.Variables.Add("KDzAnrede_Prog", KDzAnrede_Prog)
				LL.Variables.Add("KDzAnredeForm", KDzAnredeForm)
				LL.Variables.Add("KDzFName", KDZFName)
				LL.Variables.Add("KDzLName", KDZLName)
				LL.Variables.Add("KDzPostfach", KDzPostfach)
				LL.Variables.Add("KDzStrasse", KDzStrasse)
				LL.Variables.Add("KDzPLZOrt", KDzPLZOrt)

				LL.Variables.Add("ES_EndeDate", strES_EndeDate)		' "unbestimmt"
				LL.Variables.Add("esEndeDateWithGoeslonger", esEndeDateWithGoeslonger)
				LL.Variables.Add("isESEndeNull", isESEndeNull, LlFieldType.Boolean)

				Dim dESBegin As Date = m_Utility.SafeGetDateTime(rFrec, "ES_Ab", Nothing)
				Dim dESLoVon As Date = m_Utility.SafeGetDateTime(rFrec, "LOVon", Nothing)

				LL.Variables.Add("MwSt_Satz", GetYearMwSt(Year(dESLoVon)))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}. Normale Variable auflisten. {1}", strMethodeName, ex.ToString))

			End Try

			' Zusätzliche Lohnarten in Einsatzverwaltung
			GetESLohnData4Print()

			' Zusätzliche Lohnarten in ES_ZLohn
			GetES_ZLohnDataArt0_4Print()

			' Zusätzliche Lohnarten in ES_ZLohn
			GetES_ZLohnDataArt1_4Print()

			' Zusätzliche Lohnarten in ES_ZLohn
			GetES_ZLohnDataArt2_4Print()

			' Benutzerdaten wegen angemeldeten Benutzer und Unterzeichner
			GetUSData4ESVertrag()

			' Benutzerunterschrift wegen angemeldeten Benutzer oder Unterzeichner
			Test_GetUSSign4ESVertrag()

		End With

		Try

			Dim rFoundedrec As SqlDataReader = MainUtilities.OpenDb4PrintESTemplate(_ESVertragSetting.DbConnString2Open, _ESVertragSetting.SelectedESNr2Print)
			If Not rFoundedrec.HasRows Then
				Throw New Exception("Keine Mandanteninformationen wurden gefunden.")

			End If
			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}. Mandanteninformationen auflisten. {1}", strMethodeName, ex.ToString))

		End Try

	End Sub


	Sub GetES_ZLohnDataArt0_4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try

			For i As Short = 1 To 4
				LL.Variables.Add(String.Format("S_ESZLANr{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABez{0}", i), "")

				LL.Variables.Add(String.Format("S_Art_{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABetrag_G_{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABetrag_StdLohn_{0}", i), 0)

				LL.Variables.Add(String.Format("S_ESZLABetrag_Feier_{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABetrag_FeierProz_{0}", i), 0)

				LL.Variables.Add(String.Format("S_ESZLABetrag_Fer_{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABetrag_FerProz_{0}", i), 0)

				LL.Variables.Add(String.Format("S_ESZLABetrag_13_{0}", i), 0)
				LL.Variables.Add(String.Format("S_ESZLABetrag_13Proz_{0}", i), 0)

			Next i

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim Conn As New SqlConnection(strConnString)
			Dim sSql As String = "[Get ESZLohnData For Print Vertrag Art]"
			Conn.Open()

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@Art", 0)
			param = cmd.Parameters.AddWithValue("@Year", Now.Year)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Dim i As Integer = 1
			Try
				While rTemprec.Read
					LL.Variables.Add(String.Format("S_ESZLANr{0}", i), rTemprec("LANr"))
					LL.Variables.Add(String.Format("S_ESZLABez{0}", i), rTemprec("LABez"))

					LL.Variables.Add(String.Format("S_Art_{0}", i), rTemprec("Art"))
					LL.Variables.Add(String.Format("S_ESZLABetrag_G_{0}", i), rTemprec("Grundlohn"))
					LL.Variables.Add(String.Format("S_ESZLABetrag_StdLohn_{0}", i), rTemprec("Stundenlohn"))

					LL.Variables.Add(String.Format("S_ESZLABetrag_Feier_{0}", i), rTemprec("Feier"))
					LL.Variables.Add(String.Format("S_ESZLABetrag_FeierProz_{0}", i), rTemprec("FeierProz"))

					LL.Variables.Add(String.Format("S_ESZLABetrag_Fer_{0}", i), rTemprec("Ferien"))
					LL.Variables.Add(String.Format("S_ESZLABetrag_FerProz_{0}", i), rTemprec("FerienProz"))

					LL.Variables.Add(String.Format("S_ESZLABetrag_13_{0}", i), rTemprec("Lohn13"))
					LL.Variables.Add(String.Format("S_ESZLABetrag_13Proz_{0}", i), rTemprec("Lohn13Proz"))

					i += 1
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten lesen.{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetES_ZLohnDataArt1_4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			For i As Short = 5 To 8
				LL.Variables.Add(String.Format("W_ESZLANr{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABez{0}", i), "")

				LL.Variables.Add(String.Format("W_Art_{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABetrag_G_{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABetrag_StdLohn_{0}", i), 0)

				LL.Variables.Add(String.Format("W_ESZLABetrag_Feier_{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABetrag_FeierProz_{0}", i), 0)

				LL.Variables.Add(String.Format("W_ESZLABetrag_Fer_{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABetrag_FerProz_{0}", i), 0)

				LL.Variables.Add(String.Format("W_ESZLABetrag_13_{0}", i), 0)
				LL.Variables.Add(String.Format("W_ESZLABetrag_13Proz_{0}", i), 0)

			Next i

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim Conn As New SqlConnection(strConnString)
			Dim sSql As String = "[Get ESZLohnData For Print Vertrag Art]"
			Conn.Open()

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@Art", 1)
			param = cmd.Parameters.AddWithValue("@Year", Now.Year)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Dim i As Integer = 5
			Try
				While rTemprec.Read
					LL.Variables.Add(String.Format("W_ESZLANr{0}", i), rTemprec("LANr"))
					LL.Variables.Add(String.Format("W_ESZLABez{0}", i), rTemprec("LABez"))

					LL.Variables.Add(String.Format("W_Art_{0}", i), rTemprec("Art"))
					LL.Variables.Add(String.Format("W_ESZLABetrag_G_{0}", i), rTemprec("Grundlohn"))
					LL.Variables.Add(String.Format("W_ESZLABetrag_StdLohn_{0}", i), rTemprec("Stundenlohn"))

					LL.Variables.Add(String.Format("W_ESZLABetrag_Feier_{0}", i), rTemprec("Feier"))
					LL.Variables.Add(String.Format("W_ESZLABetrag_FeierProz_{0}", i), rTemprec("FeierProz"))

					LL.Variables.Add(String.Format("W_ESZLABetrag_Fer_{0}", i), rTemprec("Ferien"))
					LL.Variables.Add(String.Format("W_ESZLABetrag_FerProz_{0}", i), rTemprec("FerienProz"))

					LL.Variables.Add(String.Format("W_ESZLABetrag_13_{0}", i), rTemprec("Lohn13"))
					LL.Variables.Add(String.Format("W_ESZLABetrag_13Proz_{0}", i), rTemprec("Lohn13Proz"))

					i += 1
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten lesen.{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetES_ZLohnDataArt2_4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			For i As Short = 9 To 12
				LL.Variables.Add(String.Format("M_ESZLANr{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABez{0}", i), "")

				LL.Variables.Add(String.Format("M_Art_{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABetrag_G_{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABetrag_StdLohn_{0}", i), 0)

				LL.Variables.Add(String.Format("M_ESZLABetrag_Feier_{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABetrag_FeierProz_{0}", i), 0)

				LL.Variables.Add(String.Format("M_ESZLABetrag_Fer_{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABetrag_FerProz_{0}", i), 0)

				LL.Variables.Add(String.Format("M_ESZLABetrag_13_{0}", i), 0)
				LL.Variables.Add(String.Format("M_ESZLABetrag_13Proz_{0}", i), 0)

			Next i

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))

		End Try

		Try
			Dim Conn As New SqlConnection(strConnString)
			Dim sSql As String = "[Get ESZLohnData For Print Vertrag Art]"
			Conn.Open()

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@Art", 2)
			param = cmd.Parameters.AddWithValue("@Year", Now.Year)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Dim i As Integer = 9
			Try
				While rTemprec.Read
					LL.Variables.Add(String.Format("M_ESZLANr{0}", i), rTemprec("LANr"))
					LL.Variables.Add(String.Format("M_ESZLABez{0}", i), rTemprec("LABez"))

					LL.Variables.Add(String.Format("M_Art_{0}", i), rTemprec("Art"))
					LL.Variables.Add(String.Format("M_ESZLABetrag_G_{0}", i), rTemprec("Grundlohn"))
					LL.Variables.Add(String.Format("M_ESZLABetrag_StdLohn_{0}", i), rTemprec("Stundenlohn"))

					LL.Variables.Add(String.Format("M_ESZLABetrag_Feier_{0}", i), rTemprec("Feier"))
					LL.Variables.Add(String.Format("M_ESZLABetrag_FeierProz_{0}", i), rTemprec("FeierProz"))

					LL.Variables.Add(String.Format("M_ESZLABetrag_Fer_{0}", i), rTemprec("Ferien"))
					LL.Variables.Add(String.Format("M_ESZLABetrag_FerProz_{0}", i), rTemprec("FerienProz"))

					LL.Variables.Add(String.Format("M_ESZLABetrag_13_{0}", i), rTemprec("Lohn13"))
					LL.Variables.Add(String.Format("M_ESZLABetrag_13Proz_{0}", i), rTemprec("Lohn13Proz"))

					i += 1
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten lesen.{1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Sub GetESLohnData4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim Conn As New SqlConnection(strConnString)
			Conn.Open()
			Dim sSql As String = String.Empty

			sSql = "[Get ESLAData For Print In Einsatzvertrag]"

			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ESNr", _ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@ESLohnNr", _ESVertragSetting.SelectedESLohnNr2Print)
			Dim rTemprec As SqlDataReader = cmd.ExecuteReader
			Dim i As Integer = 1

			Try
				For i = 1 To 20
					LL.Variables.Add(String.Format("ESLANr{0}", i), 0)
					LL.Variables.Add(String.Format("ESLABez{0}", i), String.Empty)
					LL.Variables.Add(String.Format("ESLABetrag{0}", i), 0)
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten vorbelegen.{1}", strMethodeName, ex.ToString))
			End Try

			i = 1
			Try
				While rTemprec.Read
					LL.Variables.Add(String.Format("ESLANr{0}", i), rTemprec("LANr"))
					LL.Variables.Add(String.Format("ESLABez{0}", i), rTemprec("LABez"))
					LL.Variables.Add(String.Format("ESLABetrag{0}", i), rTemprec("Betrag"))

					i += i
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:Daten von Einsatzlohndaten lesen:{1}", strMethodeName, ex.ToString))

			End Try
			rTemprec.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Öffnen der Daten von Einsatzlohndaten:{1}", strMethodeName, ex.ToString))

		End Try

	End Sub


	Sub GetUSData4ESVertrag()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As New SqlConnection(strConnString)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte() = Nothing
		Dim bSignFromUser As Boolean = m_ESUnterzeichner 'If(Me._ESVertragSetting.GetESUnterzeichner = 1, True, False)
		Dim strAllESKst As String = String.Empty

		LL.Variables.Add("US_Anrede", String.Empty)
		LL.Variables.Add("ESUS_Anrede", String.Empty)
		LL.Variables.Add("KstFullName_Anrede", String.Empty)
		LL.Variables.Add("KstFullName", String.Empty)
		LL.Variables.Add("USTitel_1", String.Empty)
		LL.Variables.Add("USTitel_2", String.Empty)
		LL.Variables.Add("USAbteilung", String.Empty)

		Dim sUSSql As String = String.Empty

		If bSignFromUser Then
			sUSSql = "[Get USData 4 ESPrint With ESUnterzeichner]"
		Else
			sUSSql = "[Get USData 4 ESPrint With ESKSTData]"
		End If

		Conn.Open()
		Dim cmd As SqlCommand = New SqlCommand(sUSSql, Conn)
		cmd.CommandType = CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		If bSignFromUser Then
			param = cmd.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)

		Else
			param = cmd.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)
			param = cmd.Parameters.AddWithValue("@AsVerleih", 0)

		End If
		Dim rTemprec As SqlDataReader = cmd.ExecuteReader


		Try
			rTemprec.Read()
			If rTemprec.HasRows Then
				Dim strESUSAnrede As String = rTemprec("Anrede") '"ESUS_Anrede")
				Dim strUSrede As String = rTemprec("Anrede")

				LL.Variables.Add("ESUS_Anrede", strESUSAnrede)
				LL.Variables.Add("US_Anrede", strUSrede)

				LL.Variables.Add("KstFullName_Anrede", rTemprec("Anrede"))
				LL.Variables.Add("KstFullName", String.Format("{0} {1}", rTemprec("Vorname"), rTemprec("Nachname")))
				LL.Variables.Add("USTitel_1", rTemprec("USTitel_1"))
				LL.Variables.Add("USTitel_2", rTemprec("USTitel_2"))
				LL.Variables.Add("USAbteilung", rTemprec("USAbteilung"))

				'strAllESKst = rTemprec("AllESKst")
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Datenbanken lesen: {1}", strMethodeName, ex.ToString))
			Return

		End Try

	End Sub

	Sub Test_GetUSSign4ESVertrag()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As New SqlConnection(strConnString)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte() = Nothing
		Dim bSignFromUser As Boolean = m_ESUnterzeichner ' If(Me._ESVertragSetting.GetESUnterzeichner = 1, True, False)
		LL.Variables.Add("USSignFilename", String.Empty)

		Dim sUSSql As String = String.Empty
		If bSignFromUser Then
			sUSSql = "[Get USSign 4 ESPrint With ESUnterzeichner]"
		Else
			sUSSql = "[Get USSign 4 ESPrint With ESKSTData]"
		End If

		Conn.Open()
		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)
		SQLCmd_1.CommandType = CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		If bSignFromUser Then
			param = SQLCmd_1.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)

		Else
			param = SQLCmd_1.Parameters.AddWithValue("@ESNr", Me._ESVertragSetting.SelectedESNr2Print)
			param = SQLCmd_1.Parameters.AddWithValue("@AsVerleih", 0)

		End If

		Try
			strFullFilename = String.Format("{0}Bild_{1}.JPG", m_ProgPath.GetSpS2DeleteHomeFolder, System.Guid.NewGuid.ToString())

			Try
				Try
					BA = CType(SQLCmd_1.ExecuteScalar, Byte())
					If BA Is Nothing Then Return

				Catch ex As Exception
					Return

				End Try
				Dim ArraySize As New Integer
				ArraySize = BA.GetUpperBound(0)

				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
				Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
				fs.Write(BA, 0, ArraySize + 1)
				fs.Close()
				fs.Dispose()
				LL.Variables.Add("USSignFilename", strFullFilename)

			Catch ex As Exception
				m_Logger.LogError(String.Format("***Fehler ({0}): {1}", strMethodeName, ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("***Fehler ({0}): {1}", strMethodeName, ex.ToString))

		End Try

	End Sub



	Protected Overridable Overloads Sub Dispose( _
		 ByVal disposing As Boolean)
		If Not Me.disposed Then
			If disposing Then

			End If
			' Add code here to release the unmanaged resource.
			LL.Dispose()
			LL.Core.Dispose()
			' Note that this is not thread safe.
		End If
		Me.disposed = True
	End Sub

#Region " IDisposable Support "
	' Do not change or add Overridable to these methods.
	' Put cleanup code in Dispose(ByVal disposing As Boolean).
	Public Overloads Sub Dispose() Implements IDisposable.Dispose
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub
	Protected Overrides Sub Finalize()
		Dispose(False)
		MyBase.Finalize()
	End Sub
#End Region

End Class

