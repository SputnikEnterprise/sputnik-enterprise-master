
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath


Imports System.Data.SqlClient
Imports System.IO
Imports combit.ListLabel25
Imports SPS.Listing.Print.Utility.MainUtilities

Imports SP.Infrastructure.Logging

Public Class ClsLLLOANSearchPrint


	Implements IDisposable
	Protected disposed As Boolean = False

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

  Private m_path As New ClsProgPath
  Private m_md As New Mandant
	Private LOANListPrintSetting As New ClsLLLOANSearchPrintSetting

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsLog As New SPProgUtility.ClsEventLog
  Friend _ClsLLFunc As New ClsLLFunc

	Private strConnString As String = String.Empty
	Private strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

  Private Property USSignFileName As String
  Private Property ExistsDocFile As Boolean

	Private LL As New ListLabel

  Function BuildPrintJob() As Boolean
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Dim sSql As String = "Select * From DokPrint Where [JobNr] = @JobNr"
    Dim Conn As SqlConnection = New SqlConnection(strConnString)
    Dim bResult As Boolean = True
    Dim JobNr As String = LOANListPrintSetting.JobNr2Print

    If JobNr = String.Empty Then
      Dim strMessage As String = "Sie haben keine Vorlage ausgewählt.{0}Bitte wählen Sie aus der Liste eine Vorlage aus."
      MsgBox(String.Format(TranslateMyText(strMessage), vbNewLine), _
            MsgBoxStyle.Critical, TranslateMyText("Leere Vorlage"))
      Return False
    End If
    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
    Dim param As System.Data.SqlClient.SqlParameter

    Try
      Conn.Open()

      param = cmd.Parameters.AddWithValue("@JobNr", LOANListPrintSetting.JobNr2Print)
      Dim rDocrec As SqlDataReader = cmd.ExecuteReader          ' Dokumentendatenbank
      rDocrec.Read()
      If Not String.IsNullOrEmpty(rDocrec("DocName").ToString) Then
        _ClsLLFunc.LLDocName = m_md.GetSelectedMDDocPath(Me.LOANListPrintSetting.SelectedMDNr) & rDocrec("DocName").ToString
        _ClsLLFunc.LLDocLabel = rDocrec("Bezeichnung").ToString

        If String.IsNullOrEmpty(rDocrec("ParamCheck").ToString) Then
          _ClsLLFunc.LLParamCheck = 0
        Else
          _ClsLLFunc.LLParamCheck = CInt(IIf(CBool(rDocrec("ParamCheck")), 1, 0))
        End If

        If String.IsNullOrEmpty(rDocrec("KonvertName").ToString) Then
          _ClsLLFunc.LLKonvertName = 0
        Else
          _ClsLLFunc.LLKonvertName = CInt(IIf(CBool(rDocrec("KonvertName")), 1, 0))
        End If

        If String.IsNullOrEmpty(rDocrec("ZoomProz").ToString) Then
          _ClsLLFunc.LLParamCheck = 100
        Else
          _ClsLLFunc.LLZoomProz = CInt(IIf(CInt(rDocrec("ZoomProz")) = 0, 150, CInt(rDocrec("ZoomProz"))))
        End If

        If String.IsNullOrEmpty(rDocrec("Anzahlkopien").ToString) Then
          _ClsLLFunc.LLCopyCount = 1
        Else
          _ClsLLFunc.LLCopyCount = CInt(IIf(CInt(rDocrec("Anzahlkopien")) = 0, 1, CInt(rDocrec("Anzahlkopien"))))
        End If

        If String.IsNullOrEmpty((rDocrec("TempDocPath").ToString)) Then
          _ClsLLFunc.LLExportedFilePath = m_path.GetSpS2DeleteHomeFolder
        Else
          _ClsLLFunc.LLExportedFilePath = _ClsReg.AddDirSep(rDocrec("TempDocPath").ToString)
        End If

        If String.IsNullOrEmpty(rDocrec("ExportedFileName").ToString) Then
          _ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("DocName").ToString)
        Else
          _ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("ExportedFileName").ToString)
        End If


      Else
        _ClsLLFunc.LLDocName = String.Empty
        _ClsLLFunc.LLDocLabel = String.Empty
        _ClsLLFunc.LLParamCheck = 0
        _ClsLLFunc.LLKonvertName = 0

        _ClsLLFunc.LLParamCheck = 100
        _ClsLLFunc.LLCopyCount = 1
        _ClsLLFunc.LLExportedFilePath = m_path.GetPrinterHomeFolder
        _ClsLLFunc.LLExportedFileName = String.Empty
        bResult = False

      End If
      rDocrec.Close()

    Catch ex As Exception
			m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.ToString))

			'MsgBox(String.Format(TranslateMyText("Fehler: {1}{0}JobNr: {2}"), vbNewLine, Err.Description, JobNr), _
			'       MsgBoxStyle.Critical, strMethodeName)

			bResult = False

		Finally
			cmd.Dispose()
			Conn.Close()

		End Try

		Return bResult
	End Function

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
			Dim rFoundedrec As SqlDataReader = MainUtilities.OpenDb4PrintListing(strConnString, Me.LOANListPrintSetting.SQL2Open)
			If Not rFoundedrec.HasRows Then
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, "Keine Daten wurden gefunden..."))
				Exit Sub
			End If

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(LL)

			LL.Core.LlDefineLayout(CType(0, IntPtr), _ClsLLFunc.LLDocLabel, _
														 If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card), _
														 _ClsLLFunc.LLDocName)

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))

		Finally

		End Try

	End Sub

	Function ShowInPrint(ByRef bShowBox As Boolean) As String
		Const LL_PRNOPT_COPIES_SUPPORTED As Integer = 3
		Dim strResult As String = "Success"
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim strJobNr As String = Me.LOANListPrintSetting.JobNr2Print

		Dim strQuery As String = LOANListPrintSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim i As Integer = 0

		If Not Me.ExistsDocFile Then Return strResult

		Try
			Dim rFoundedrec As SqlDataReader = Utilities.OpenDb4PrintListing(strConnString, LOANListPrintSetting.SQL2Open)
			If Not rFoundedrec.HasRows Then Return TranslateMyText("Keine Daten wurden gefunden") & "..."

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(LL)
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
			LL.Core.LlPrintEnd(0)

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, _
																 m_path.GetSpSTempFolder, CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, m_path.GetSpSTempFolder)
				Return "Error"
			End If


		Catch LlException As LL_User_Aborted_Exception
			Return "Error"

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.{0}: {1}", strMethodeName, LlException.Message))
			strResult = String.Format("Error: {0}", LlException.Message)

		Finally

		End Try

		Return strResult
	End Function

	Function ExportLLDoc(ByVal LL As ListLabel, ByVal strJobNr As String, ByVal iExportMode As Short, _
											 ByVal iProposeNr As Integer) As Boolean

		Dim strLLTemplate As String = _ClsLLFunc.LLDocName
		Dim bResult As Boolean = True
		If Not Me.ExistsDocFile Then Return bResult

		Dim strQuery As String = LOANListPrintSetting.SQL2Open
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim i As Integer = 0

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@iProposeNr", iProposeNr)

			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
			rFoundedrec.Read()
			If IsNothing(rFoundedrec) Then
				Dim strMessage As String = "Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben."
				MsgBox(TranslateMyText(strMessage), _
							 MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))
				Return bResult
			End If

			InitLL(LL)
			LL.Variables.Clear()
			LL.Fields.Clear()

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				DefineData(LL, True, rFoundedrec)
			Else
				DefineData(LL, False, rFoundedrec)
			End If
			SetLLVariable(LL)

			LL.ExportOptions.Clear()
			SetExportSetting(iExportMode)

			LL.Core.LlPrintWithBoxStart(If(_ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper), LlProject.List, LlProject.Card), _
																	_ClsLLFunc.LLDocName, LlPrintMode.Export, LlBoxType.StandardAbort, CType(0, IntPtr), _
																	_ClsLLFunc.LLDocLabel)

			LL.Core.LlPrintSetOptionString(LlPrintOptionString.Export, _ClsLLFunc.LLExporterName)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.File", _ClsLLFunc.LLExporterFileName)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Path", m_path.GetSpSOfferHomeFolder)
			LL.Core.LlXSetParameter(LlExtensionType.Export, "PDF", "Export.Quiet", "1")

			LL.Core.LlPrintSetOption(LlPrintOption.Dialog_DestinationMask, 2)
			Dim TargetFormat As String = LL.Core.LlPrintGetOptionString(LlPrintOptionString.Export)

			While Not LL.Core.LlPrint()
			End While

			If _ClsLLFunc.LLDocName.ToUpper.EndsWith(".LST".ToUpper) Then
				Do
					' pass data for current record
					DefineData(LL, True, rFoundedrec)

					While LL.Core.LlPrintFields() = LlConstants.WrnRepeatData	' LlConst.LL_WRN_REPEAT_DATA
						LL.Core.LlPrint()
					End While

				Loop While rFoundedrec.Read()

				While LL.Core.LlPrintFieldsEnd() = LlConstants.WrnRepeatData ' LlConst.LL_WRN_REPEAT_DATA
				End While
			End If
			LL.Core.LlPrintEnd(0)

			If TargetFormat = "PRV" Then
				LL.Core.LlPreviewDisplay(_ClsLLFunc.LLDocName, _
																 m_path.GetSpSTempFolder, _
																 CType(0, IntPtr))
				LL.Core.LlPreviewDeleteFiles(_ClsLLFunc.LLDocName, _
																		 m_path.GetSpSTempFolder)
				Return False
			End If
			'      _ClsLLFunc.LLExportFileName = String.Format("{0}{1}", m_path.GetSpSOfferHomeFolder, _ClsLLFunc.LLExporterFileName)

		Catch LlException As Exception
			m_Logger.LogError(String.Format("LlException.Message:{0}This information was generated by a List & Label custom exception.", vbNewLine))

			'If Err.Number <> (LlError.LL_ERR_USER_ABORTED) And Err.Number <> 5 Then
			'  MessageBox.Show(LlException.Message + vbCrLf, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
			'End If
			bResult = False

		Finally

		End Try

		Return bResult
	End Function

  Sub SetExportSetting(ByVal iIndex As Short)
    Select Case iIndex
      Case 0
        _ClsLLFunc.LLExportfilter = "PDF Files|*.PDF"
        _ClsLLFunc.LLExporterName = "PDF"
        _ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".pdf"

      Case 1
        _ClsLLFunc.LLExportfilter = "MHTML Files|*.mht"
        _ClsLLFunc.LLExporterName = "MHTML"
        _ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".mht"

      Case 2
        _ClsLLFunc.LLExportfilter = "HTML Files|*.HTM"
        _ClsLLFunc.LLExporterName = "HTML"
        _ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".htm"

      Case 3
        _ClsLLFunc.LLExportfilter = "RTF Files|*.RTF"
        _ClsLLFunc.LLExporterName = "RTF"
        _ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".rtf"

      Case 4
        _ClsLLFunc.LLExportfilter = "XML Files|*.XML"
        _ClsLLFunc.LLExporterName = "XML"
        _ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".xml"

      Case 5
        _ClsLLFunc.LLExportfilter = "Tiff Files|*.TIF"
        _ClsLLFunc.LLExporterName = "PICTURE_MULTITIFF"
        _ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".tif"

      Case 6
        _ClsLLFunc.LLExportfilter = "Text Files|*.TXT"
        _ClsLLFunc.LLExporterName = "TXT"
        _ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".txt"

      Case 7
        _ClsLLFunc.LLExportfilter = "Excel Files|*.XLS"
        _ClsLLFunc.LLExporterName = "XLS"
        _ClsLLFunc.LLExporterFileName = _ClsLLFunc.LLExportedFileName + ".xls"
    End Select

  End Sub

  Sub DefineData(ByVal LL As ListLabel, ByVal AsFields As Boolean, ByVal MyDataReader As SqlDataReader)
    Dim i As Integer

    'Define(Data)
    'For i = 0 To MyDataReader.FieldCount - 1
    '  If AsFields Then
    '    LL.Fields.Add(MyDataReader.GetName(i), MyDataReader.GetValue(i))
    '  Else
    '    LL.Variables.Add(MyDataReader.GetName(i), MyDataReader.GetValue(i))
    '  End If
    'Next


    Dim iType As String
    Dim strParam As LlFieldType
    Dim strContent As String

    ' Define data
    For i = 0 To MyDataReader.FieldCount - 1
      iType = MyDataReader.GetFieldType(i).ToString.ToUpper

      Select Case iType.ToUpper

        'GR: Numerisches Feld
        Case "System.Int16".ToUpper, "System.Int32".ToUpper, "System.Int64".ToUpper, "System.Decimal".ToUpper, "System.Double".ToUpper
          'DbType.Currency, DbType.Decimal, DbType.Double, DbType.Int16, DbType.Int32, DbType.Int64
          'TypeCode.Decimal, TypeCode.Double, TypeCode.Int16, TypeCode.Int32, TypeCode.Int64, TypeCode.UInt16, TypeCode.UInt32, TypeCode.Int64
          'SqlDbType.Money, SqlDbType.Decimal, SqlDbType.Float, SqlDbType.Int, SqlDbType.SmallInt, SqlDbType.SmallMoney, SqlDbType.TinyInt
          strParam = LlFieldType.Numeric
          strContent = MyDataReader.Item(i).ToString & ""

          'GR: Falls der Datentyp "Datum" ist, Umwandlun in einen numerischen Datumswert
        Case "System.DateTime".ToUpper
          ''SqlDbType.Date, SqlDbType.DateTime, SqlDbType.DateTime2, SqlDbType.SmallDateTime, SqlDbType.Time
          'strParam = LlFieldType.Date_Localized
          'If IsDBNull(MyDataReader.Item(i)) Then
          '  strContent = CDate("1.1.1900").ToShortDateString
          'Else
          '  strContent = CDate(MyDataReader.Item(i)).ToShortDateString
          'End If

          strParam = LlFieldType.Date_OLE

          If IsDBNull(MyDataReader.Item(i)) Then
            strContent = CDate("01.01.1900").ToOADate().ToString() ' , "G")
          Else
            strContent = MyDataReader.GetDateTime(i).ToOADate().ToString()
          End If

          'GR: Entscheidungsfeld (Ja/Nein), Boolean
        Case "System.Boolean".ToUpper
          strParam = LlFieldType.Boolean
          If IsDBNull(MyDataReader.Item(i)) Then
            strContent = "0"
          Else
            strContent = CStr(IIf(CBool(MyDataReader.Item(i)), 1, 0))
          End If

          'GR: Zeichenformat = Text
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
      '      LL.Core.LlDefineFieldExt(MyDataReader.GetName(i), MyDataReader.GetValue(i),LlFieldType.Numeric 
    Next

  End Sub

  Sub GetMDUSData4Print(ByVal LL As ListLabel)

		MainUtilities.GetUSData(Me.LOANListPrintSetting.DbConnString2Open, _ClsLLFunc, LOANListPrintSetting.SelectedMDNr)
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
      LL.Variables.Add("MwStProzent", _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Debitoren", "MWST-Satz"))

    End With

  End Sub

  Sub InitLL(ByVal LL As ListLabel)
    Const LL_DLGBOXMODE_3DBUTTONS As Integer = 32768                       ' 0x8000
    Const LL_DLGBOXMODE_ALT10 As Integer = 11                              ' 0x000B
    Const LL_OPTION_INCLUDEFONTDESCENT As Integer = 79                     ' 79
    Const LL_OPTION_INCREMENTAL_PREVIEW As Integer = 135                   ' 135

    Const LL_OPTION_VARSCASESENSITIVE As Integer = 46                      ' 46
    Const LL_OPTION_IMMEDIATELASTPAGE As Integer = 64                      ' 64
    Const LL_OPTION_CONVERTCRLF As Integer = 21                            ' 21

    Const LL_OPTION_NOPARAMETERCHECK As Integer = 32                       ' 32
    Const LL_OPTION_XLATVARNAMES As Integer = 51                           ' 51

    Const LL_OPTION_ESC_CLOSES_PREVIEW As Integer = 102                    ' 102
    Const LL_OPTION_SUPERVISOR As Integer = 3                              ' 3
    Const LL_OPTION_UISTYLE As Integer = 99                                ' 99
    Const LL_OPTION_UISTYLE_OFFICE2003 As Integer = 2                      ' 2
    Const LL_OPTION_AUTOMULTIPAGE As Integer = 42                          ' 42
    Const LL_OPTION_SUPPORTPAGEBREAK As Integer = 10                       ' 10
    Const LL_OPTION_PRVZOOM_PERC As Integer = 25                           ' 25

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

    LL.Core.LlSetOption(LL_OPTION_IMMEDIATELASTPAGE, 1)       ' Lastpage
    LL.Core.LlSetOption(LL_OPTION_CONVERTCRLF, 1)             ' Doppelte Zeilenumbruch

    LL.Core.LlSetOption(LL_OPTION_NOPARAMETERCHECK, _ClsLLFunc.LLParamCheck)
    LL.Core.LlSetOption(LL_OPTION_XLATVARNAMES, _ClsLLFunc.LLKonvertName)

    LL.Core.LlSetOption(LL_OPTION_ESC_CLOSES_PREVIEW, 1)
    LL.Core.LlSetOption(LL_OPTION_SUPERVISOR, 1)
    LL.Core.LlSetOption(LL_OPTION_UISTYLE, LL_OPTION_UISTYLE_OFFICE2003)
    LL.Core.LlSetOption(LL_OPTION_AUTOMULTIPAGE, 1)

    LL.Core.LlSetOption(LL_OPTION_SUPPORTPAGEBREAK, 1)
    LL.Core.LlSetOption(LL_OPTION_PRVZOOM_PERC, _ClsLLFunc.LLZoomProz)

    LL.Core.LlPreviewSetTempPath(m_path.GetSpSTempFolder)
    LL.Core.LlSetPrinterDefaultsDir(m_path.GetPrinterHomeFolder)

  End Sub

  Sub SetLLVariable(ByVal LL As ListLabel)
    Dim aValue As New List(Of String)

    ' Zusätzliche Variable einfügen
    LL.Variables.Add("AutorFName", _ClsProgSetting.GetUserFName)
    LL.Variables.Add("AutorLName", _ClsProgSetting.GetUserLName)
    LL.Variables.Add("USSignFilename", Me.LOANListPrintSetting.USSignFileName)

    LL.Variables.Add("SortBez", LOANListPrintSetting.ListSortBez)
    LL.Variables.Add("FilterBez", LOANListPrintSetting.ListFilterBez(0))
		LL.Variables.Add("FilterBez2", If(LOANListPrintSetting.ListFilterBez.Count > 1, LOANListPrintSetting.ListFilterBez(1), String.Empty))
		LL.Variables.Add("FilterBez3", If(LOANListPrintSetting.ListFilterBez.Count > 2, LOANListPrintSetting.ListFilterBez(2), String.Empty))
		LL.Variables.Add("FilterBez4", If(LOANListPrintSetting.ListFilterBez.Count > 3, LOANListPrintSetting.ListFilterBez(3), String.Empty))

    LL.Variables.Add("ListBez2Print", LOANListPrintSetting.ListBez2Print)

    ' Mandantendaten drucken...
    GetMDUSData4Print(LL)

  End Sub


  Public Sub New(ByVal _Setting As ClsLLLOANSearchPrintSetting)

    Me.LOANListPrintSetting = _Setting
    Me.strConnString = _Setting.DbConnString2Open
    Me.ExistsDocFile = BuildPrintJob()

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
