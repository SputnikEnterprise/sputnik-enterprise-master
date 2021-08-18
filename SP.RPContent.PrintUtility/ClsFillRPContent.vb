

Imports System.IO
Imports O2S.Components.PDF4NET
Imports O2S.Components.PDF4NET.Forms
Imports SPProgUtility.MainUtilities.Utilities
Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient

Imports DevExpress.XtraEditors

Imports System.Threading
Imports SP.Infrastructure
Imports SPProgUtility.Mandanten

Namespace FillRPContent

	Public Class ClsFillRPContent

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		Private m_utility_SP As SPProgUtility.MainUtilities.Utilities

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private _RPCSetting As ClsRPCSetting

		Private Property GetSelectedRPNr() As Integer
		Private m_Utility As Infrastructure.Utility
		Private m_mandant As Mandant

		Public Property RPCFile2Open() As String
		Public Property DbConn2Open As New SqlClient.SqlConnection()
		Private m_PDFUtility As PDFUtilities.Utilities

		Public Property frm As XtraForm
		Private strSonstigeLA As String
		Private FinalFilename2Print As String
		Private liRPCFile As New List(Of String)
		Private m_PrintWithCustomerdata As Boolean
		Private m_AppName As String = "SP.RPContent.PrintUtility"


#Region "constructor"

		Public Sub New(ByVal _frm As Form, ByVal _setting As ClsRPCSetting)
			Dim bDownladnewFile As Boolean = False

			m_utility_SP = New SPProgUtility.MainUtilities.Utilities
			m_Utility = New Infrastructure.Utility
			m_PDFUtility = New PDFUtilities.Utilities

			m_InitializationData = CreateInitialData(ClsDataDetail.MDData.MDNr, ClsDataDetail.UserData.UserNr)
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)
			m_mandant = New Mandant

			Me.frm = _frm
			Me.RPCFile2Open = String.Format("{0}Rapportformular.PDF", _ClsProgSetting.GetMDDocPath)
			Dim strAdresse As String = "http://downloads.domain.com/sps_downloads/prog/forms/"
			Dim strFilename As String = "Rapportformular.pdf"

			If Not File.Exists(String.Format("{0}{1}", _ClsProgSetting.GetMDDocPath, strFilename)) Then
				bDownladnewFile = True
			End If
			If bDownladnewFile Then
				Try
					My.Computer.Network.DownloadFile(String.Format("{0}{1}", strAdresse, strFilename),
																					 String.Format("{0}{1}", _ClsProgSetting.GetMDDocPath, strFilename))

				Catch ex As Exception
					Dim strMsg As String = "Die Datei konnte nicht heruntergeladen werden."
					strMsg = m_Translate.GetSafeTranslationValue(strMsg)
					DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_Translate.GetSafeTranslationValue("Datei herunterladen"),
																										 MessageBoxButtons.OK, MessageBoxIcon.Error,
																										 MessageBoxDefaultButton.Button1)

				End Try
			End If
			Me.RPCFile2Open = Path.Combine(_ClsProgSetting.GetMDDocPath, strFilename)
			Me._RPCSetting = _setting


			'Dim sValue As String = String.Empty
			'Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
			'Dim strKeyName As String = "ExportPfad".ToLower
			'Dim strQuery As String = String.Format(strMainKey, Chr(34), m_AppName, strKeyName)

			'strKeyName = "printcustomerdataonreporttemplate".ToLower
			'strQuery = String.Format(strMainKey, Chr(34), m_AppName, strKeyName)
			'sValue = "true"
			' m_Utility.ParseToBoolean(_ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetUserProfileFile, strQuery, sValue), True)

			m_PrintWithCustomerdata = PrintCusotmerDataOnReportsContentTemplate

		End Sub


#End Region


#Region "readonly properties"

		ReadOnly Property GetPDFVW_O2SSerial() As String
			Get
				Return _ClsProgSetting.GetPDFVW_O2SSerial
			End Get
		End Property

		ReadOnly Property GetPDF_O2SSerial() As String
			Get
				Return _ClsProgSetting.GetPDF_O2SSerial
			End Get
		End Property

		Sub New(ByVal _setting As ClsRPCSetting)
			Me._RPCSetting = _setting

			m_utility_SP = New SPProgUtility.MainUtilities.Utilities

		End Sub

		Private ReadOnly Property PrintCusotmerDataOnReportsContentTemplate() As Boolean
			Get
				Dim FORM_XML_MAIN_KEY As String = "UserProfile/programsetting"
				Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

				Dim UserXMLFileName = m_mandant.GetSelectedMDUserProfileXMLFilename(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)
				Dim value As String = m_path.GetXMLNodeValue(UserXMLFileName, String.Format("{0}/printcustomerdataonreporttemplate", FORM_XML_MAIN_KEY))
				If String.IsNullOrWhiteSpace(value) Then Return True Else Return StrToBool(value)

			End Get

		End Property

#End Region

		Function StartFillingRPCFile() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "Success..."
			Dim Conn As New SqlConnection
			Dim strMsg As String = String.Empty

			liRPCFile.Clear()
			If Not File.Exists(Me.RPCFile2Open) Then
				m_Logger.LogWarning(String.Format("{0}.Die Datei existiert nicht!", strMethodeName))
				Return "Error: Die Datei existiert nicht!"
			End If

			Try
				Me.FinalFilename2Print = String.Empty

				Try
					For i As Integer = 0 To _RPCSetting.FoundedRPNr.Count - 1
						strSonstigeLA = String.Empty

						Dim iCurrRPNr As Integer = _RPCSetting.FoundedRPNr.Item(i)
						Me.GetSelectedRPNr = iCurrRPNr
						strMsg = m_Translate.GetSafeTranslationValue("Rapportdetail für {0} {1} wird erstellt") & "..."
						'Dim time_1 As Double = System.Environment.TickCount

						strResult = CreatedPDFDocWithMA(iCurrRPNr)

					Next

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Fill PDF-Files: {1}", strMethodeName, ex.Message))
					Return String.Format("Error: {0}", ex.Message)

				End Try

				Dim strExportPfad As String = m_InitializationData.UserData.spTempRepportPath
				If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempRepportPath)

				Dim filename As String = String.Format("Rapport_{0}_{1}.pdf", m_InitializationData.MDData.MDName, Environment.TickCount)
				Dim strFinalFilename As String = Path.Combine(strExportPfad, filename)

				Try
					If liRPCFile.Count > 1 Then
						strMsg = m_Translate.GetSafeTranslationValue("Die Dokumente werden zusammengefügt") & "..."

						'Dim time_1 As Double = System.Environment.TickCount
						'Dim _clsPDF As New SP.PDFO2S.ClsPDF4Net
						'_clsPDF.Merg2PDFFiles(strFinalFilename, liRPCFile.ToArray)

						Dim mergePDF = m_PDFUtility.MergePdfFiles(liRPCFile.ToArray, strFinalFilename)
						If Not mergePDF Then
							m_Logger.LogError("merging pdf file was not successfull!")
							strFinalFilename = String.Empty
						End If

					ElseIf liRPCFile.Count = 1 Then
						File.Copy(liRPCFile(0), strFinalFilename, True)

					Else
						strMsg = "Error: Leider wurden keine Dateien erstellt."

					End If
					If Not Me._RPCSetting.ShowMessage Then Return strFinalFilename

					strMsg = "Ihre Daten wurden erfolgreich in {0}{1}{0}geschrieben"
					If _RPCSetting.PrintCreatedPDFFile Then
						strMsg &= " und das Dokument wird nun gedruckt."
					Else
						strMsg &= "."
					End If
					strMsg = String.Format(m_Translate.GetSafeTranslationValue(strMsg), vbNewLine, strFinalFilename)

					DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_Translate.GetSafeTranslationValue(If(_RPCSetting.PrintCreatedPDFFile, "Daten erstellt", "Daten exportieren")),
																										 MessageBoxButtons.OK, MessageBoxIcon.Information,
																										 MessageBoxDefaultButton.Button1)

					' wenn nicht drucken dann ist exportiert und muss geöffnet werden...
					If Not _RPCSetting.PrintCreatedPDFFile Then
						Process.Start("explorer.exe", "/select," & strFinalFilename)
						Process.Start(strFinalFilename)
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Merge PDF-Files: {1}", strMethodeName, ex.Message))
					DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString, m_Translate.GetSafeTranslationValue("Daten exportieren"),
																										 MessageBoxButtons.OK, MessageBoxIcon.Error,
																										 MessageBoxDefaultButton.Button1)

				End Try

				' soll gedruckt werden...
				If _RPCSetting.PrintCreatedPDFFile Then
					Try
						Me.FinalFilename2Print = strFinalFilename
						Dim PrintListingThread = New Thread(AddressOf StartPrinting)
						PrintListingThread.Name = "PrintingPDFFile"
						PrintListingThread.SetApartmentState(ApartmentState.STA)
						PrintListingThread.Start()

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.PrintSelectedPDFFile: {1}", strMethodeName, ex.ToString))
						Return String.Format("Error: {0}.PrintSelectedPDFFile: {1}", strMethodeName, ex.ToString)
					End Try
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Open&Fill PDF-Files: {1}", strMethodeName, ex.ToString))
				Return String.Format("Error: {0}.Open&Fill PDF-Files: {1}", strMethodeName, ex.ToString)

			End Try
			'LoadingPanel.Close()

			Return strResult
		End Function

		Sub StartPrinting()
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim _clsPDF As New SP.PDFO2S.ClsMain_Net
			Dim strResult As String = ""

			Try
				strResult = _clsPDF.PrintSelectedPDFFile(Me.FinalFilename2Print, True)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

			End Try

		End Sub

		Function FillMDDataIntoPDF(ByVal doc As PDFDocument) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "success"

			Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
			Conn.Open()

			Dim sSql As String = "[Get USData 4 Templates With USName]"
			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@USNachname", m_InitializationData.UserData.UserLName)
			param = cmd.Parameters.AddWithValue("@USVorname", m_InitializationData.UserData.UserFName)

			Dim rRecord As SqlDataReader = cmd.ExecuteReader          ' Benutzerdatenbank

			Try
				rRecord.Read()
				If rRecord.HasRows Then
					If Not doc.Form.Fields("MD_Name") Is Nothing Then CType(doc.Form.Fields("MD_Name"), PDFTextBoxField).Text = rRecord("MDName")
					If Not doc.Form.Fields("MD_Strasse") Is Nothing Then CType(doc.Form.Fields("MD_Strasse"), PDFTextBoxField).Text = rRecord("MDStrasse")
					If Not doc.Form.Fields("MD_PLZ_Ort") Is Nothing Then CType(doc.Form.Fields("MD_PLZ_Ort"), PDFTextBoxField).Text = String.Format("{0} {1}", rRecord("MDPLZ"), rRecord("MDOrt"))
					If Not doc.Form.Fields("MD_Telefon") Is Nothing Then CType(doc.Form.Fields("MD_Telefon"), PDFTextBoxField).Text = String.Format("Telefon: {0}", rRecord("MDTelefon"))
					If Not doc.Form.Fields("MD_Telefax") Is Nothing Then CType(doc.Form.Fields("MD_Telefax"), PDFTextBoxField).Text = String.Format("Telefax: {0}", rRecord("MDTelefax"))
					If Not doc.Form.Fields("MD_Email") Is Nothing Then CType(doc.Form.Fields("MD_Email"), PDFTextBoxField).Text = String.Format("E-Mail: {0}", rRecord("MDeMail"))
					If Not doc.Form.Fields("MD_Homepage") Is Nothing Then CType(doc.Form.Fields("MD_Homepage"), PDFTextBoxField).Text = String.Format("Homepage: {0}", rRecord("MDHomepage"))

					If Not doc.Form.Fields("MD_Ort_Datum") Is Nothing Then CType(doc.Form.Fields("MD_Ort_Datum"), PDFTextBoxField).Text = String.Format("{0}, {1}", rRecord("MDOrt"), Format(Now, "d"))

				Else
					Throw New Exception(m_Translate.GetSafeTranslationValue("Keine Mandantendaten wurden gefunden."))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.MD-Adresse füllen: {1}", strMethodeName, ex.ToString))
				Return String.Format("Error.{0}", ex.ToString)

			End Try

			Return strResult
		End Function

		Function FillRPDataIntoPDF(ByVal doc As PDFDocument) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "success"
			Dim SQL2Open As String = "[Get RPBaseCData 4 Print in RPContent]"
			Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

			m_Logger.LogDebug(String.Format("{0}. RPNr: {1} wird bearbeitet...", strMethodeName, Me.GetSelectedRPNr))
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(SQL2Open, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@RPNr", Me.GetSelectedRPNr)
			Dim rRecord As SqlDataReader = cmd.ExecuteReader

			rRecord.Read()
			If rRecord.HasRows Then
				Dim customerName As String = String.Empty
				If m_PrintWithCustomerdata Then
					customerName = rRecord("Firma1")
					Dim customerName2 As String = rRecord("Firma2")
					Dim customerpostfach As String = If(rRecord("KDPostfach") <> "", "Postfach " & rRecord("KDPostfach"), "")
					Dim customerStreet As String = rRecord("KDStrasse")
					Dim customerAddress As String = String.Format("{0}-{1} {2}", rRecord("KDLand"), rRecord("KDPLZ"), rRecord("KDOrt"))

					customerName = customerName & If(Not String.IsNullOrWhiteSpace(customerName2 & customerpostfach & customerStreet & customerAddress), vbNewLine, "")
					If Not String.IsNullOrWhiteSpace(customerName2) Then customerName &= customerName2 & If(Not String.IsNullOrWhiteSpace(customerpostfach & customerStreet & customerAddress), vbNewLine, "")
					If Not String.IsNullOrWhiteSpace(customerpostfach) Then customerName &= customerpostfach & If(Not String.IsNullOrWhiteSpace(customerStreet & customerAddress), vbNewLine, "")
					customerName &= customerStreet & If(Not String.IsNullOrWhiteSpace(customerAddress), vbNewLine, "")
					customerName &= customerAddress
					customerName = customerName.Replace(vbNewLine & vbNewLine, vbNewLine)

				Else

				End If

				Dim employeeName As String = String.Format("{0} {1}", rRecord("MAVorname"), rRecord("MANachname"))
				Dim employeeCo As String = rRecord("MACo")
				Dim employeepostfach As String = If(rRecord("MAPostfach") <> "", "Postfach " & rRecord("MAPostfach"), "")
				Dim employeeStreet As String = rRecord("MAStrasse")
				Dim employeeAddress As String = String.Format("{0}-{1} {2}", rRecord("MALand"), rRecord("MAPLZ"), rRecord("MAOrt"))

				employeeName = employeeName & If(Not String.IsNullOrWhiteSpace(employeeCo & employeepostfach & employeeStreet & employeeAddress), vbNewLine, "")
				If Not String.IsNullOrWhiteSpace(employeeCo) Then employeeName &= employeeCo & If(Not String.IsNullOrWhiteSpace(employeepostfach & employeeStreet & employeeAddress), vbNewLine, "")
				If Not String.IsNullOrWhiteSpace(employeepostfach) Then employeeName &= employeepostfach & If(Not String.IsNullOrWhiteSpace(employeeStreet & employeeAddress), vbNewLine, "")
				If Not String.IsNullOrWhiteSpace(employeeStreet) Then employeeName &= employeeStreet & If(Not String.IsNullOrWhiteSpace(employeeAddress), vbNewLine, "")
				employeeName &= employeeAddress


				Try
					If Not doc.Form.Fields("RP_RPNr") Is Nothing Then _
					CType(doc.Form.Fields("RP_RPNr"), PDFTextBoxField).Text = rRecord("RPNr")
					If Not doc.Form.Fields("RP_ESNr") Is Nothing Then _
					CType(doc.Form.Fields("RP_ESNr"), PDFTextBoxField).Text = rRecord("ESNr")

					If Not doc.Form.Fields("KD_Name") Is Nothing Then _
					CType(doc.Form.Fields("KD_Name"), PDFTextBoxField).Text = customerName ' rRecord("Firma1")

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.KD-Adresse füllen: {1}", strMethodeName, ex.Message))
					Return String.Format("Error.{0}", ex.Message)

				End Try

				Try
					If Not doc.Form.Fields("ES_GAV") Is Nothing Then CType(doc.Form.Fields("ES_GAV"), PDFTextBoxField).Text = String.Format("({0}) {1}", rRecord("RPGAV_Kanton"), rRecord("RPGAV_Beruf"))
					If Not doc.Form.Fields("RP_month_Year") Is Nothing Then CType(doc.Form.Fields("RP_month_Year"), PDFTextBoxField).Text = String.Format("{0:d} - {1:d}", rRecord("von"), rRecord("bis"))

					If Not doc.Form.Fields("MA_Name") Is Nothing Then CType(doc.Form.Fields("MA_Name"), PDFTextBoxField).Text = employeeName
					If Not doc.Form.Fields("ES_als") Is Nothing Then CType(doc.Form.Fields("ES_als"), PDFTextBoxField).Text = String.Format("{0}", rRecord("ES_Als"))


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.MA-Adresse füllen: {1}", strMethodeName, ex.ToString))
					Return String.Format("Error.{0}", ex.ToString)

				End Try

			Else
				Throw New Exception(m_Translate.GetSafeTranslationValue("Keine Rapportdaten wurden gefunden."))

			End If

			Return strResult
		End Function

		Function FillRPTagDataIntoPDF(ByVal doc As PDFDocument) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "success"
			Dim SQL2Open As String = "[Get RPCTagData 4 Print in RPContent]"
			Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
			m_Logger.LogDebug(String.Format("{0}. RPNr: {1} wird bearbeitet...", strMethodeName, Me.GetSelectedRPNr))

			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(SQL2Open, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@RPNr", Me.GetSelectedRPNr)
			Dim rRecord As SqlDataReader = cmd.ExecuteReader
			Dim strLName_1_Field As String = "laname_1"
			Dim strLName_2_Field As String = "laname_2"
			Dim strTotalField As String = "total_"
			strSonstigeLA = String.Empty

			While rRecord.Read
				Try
					For i As Integer = 1 To 31
						Dim strDbFieldName As String = String.Empty
						Dim strDocFieldName As String = String.Format("_Tag{0}", i)
						If i = 29 Then
							Trace.WriteLine(strDocFieldName)
						End If
						Select Case rRecord("LANr")
							Case 100
								strDocFieldName = String.Format("100_{0}", i)
								strTotalField = "total_100"

							Case 125
								strDocFieldName = String.Format("125_{0}", i)
								strTotalField = "total_125"

							Case 150
								strDocFieldName = String.Format("150_{0}", i)
								strTotalField = "total_150"

							Case 200
								strDocFieldName = String.Format("200_{0}", i)
								strTotalField = "total_200"

							Case 210
								strDocFieldName = String.Format("Schicht_{0}", i)
								strTotalField = "total_schicht"

							Case 110
								strDocFieldName = String.Format("Stollen_{0}", i)
								strTotalField = "total_stollen"

							Case Else
								strDocFieldName = String.Format("Spesen_{0}", i)
								strTotalField = "total_spesen"

						End Select

						If Not doc.Form.Fields(strDocFieldName) Is Nothing Then
							Dim strFieldValue As String = "{0}"
							Dim strFehlcode As String = ""
							Dim strStd As String = ""

							'If rRecord(String.Format("_Fehl{0}", i)) <> String.Empty Then  'And rRecord("LANr") = 100 Then
							'  strFehlcode = " (" & rRecord(String.Format("_Fehl{0}", i)) & ")"
							'End If
							If rRecord(String.Format("_Tag{0}", i)) <> 0 Then
								strStd = Format(rRecord(String.Format("_Tag{0}", i)), "f")
							End If
							' möglicherweise ist das Feld bereits beschrieben!
							If CType(doc.Form.Fields(strDocFieldName), PDFTextBoxField).Text <> String.Empty Then strFehlcode = String.Empty
							'If i = 30 And strDocFieldName.ToLower.Contains("stollen") Then
							'  Trace.WriteLine(i)
							'End If

							strStd = Val(CType(doc.Form.Fields(strDocFieldName), PDFTextBoxField).Text) + Val(strStd)
							If strStd = 0 Then strStd = "" Else strStd = String.Format("{0:f2}", Val(strStd))

							strFieldValue = String.Format(strFieldValue, strStd)
							CType(doc.Form.Fields(strDocFieldName), PDFTextBoxField).Text = strFieldValue

						End If

					Next
					If Not (doc.Form.Fields(strTotalField) Is Nothing) Then
						CType(doc.Form.Fields(strTotalField), PDFTextBoxField).Text = String.Format("{0:f2}", (Val(CType(doc.Form.Fields(strTotalField), PDFTextBoxField).Text) + rRecord("totalstd")))
					End If
					If rRecord("LANr") = 110 AndAlso Not (doc.Form.Fields(strLName_2_Field) Is Nothing) Then
						CType(doc.Form.Fields(strLName_2_Field), PDFTextBoxField).Text = String.Format("{0:f}", rRecord("LANr"))
					End If
					If rRecord("LANr") = 210 AndAlso Not (doc.Form.Fields(strLName_1_Field) Is Nothing) Then
						CType(doc.Form.Fields(strLName_1_Field), PDFTextBoxField).Text = String.Format("{0:f}", rRecord("LANr"))
					End If

					strSonstigeLA &= String.Format("{2:f3}{1}{3}{1}{4:f2} Std.{0}", vbNewLine, vbTab, rRecord("LANr"), rRecord("_LAName"), rRecord("totalstd"))


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Stundenlöhne füllen: {1}", strMethodeName, ex.ToString))
					Return String.Format("Error.{0}", ex.ToString)

				End Try
			End While

			FillRPFehlCodeDataIntoPDF(doc)

			Return strResult
		End Function

		Function FillRPFehlCodeDataIntoPDF(ByVal doc As PDFDocument) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "success"
			Dim SQL2Open As String = "[Get RPCFehlCodeData 4 Print in RPContent]"
			Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
			m_Logger.LogDebug(String.Format("{0}. RPNr: {1} wird bearbeitet...", strMethodeName, Me.GetSelectedRPNr))

			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(SQL2Open, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@RPNr", Me.GetSelectedRPNr)
			Dim rRecord As SqlDataReader = cmd.ExecuteReader

			While rRecord.Read
				Try
					For i As Integer = 1 To 31
						Dim strDbFieldName As String = String.Empty
						Dim strDocFieldName As String = String.Format("_Tag{0}", i)

						strDocFieldName = String.Format("100_{0}", i)

						If Not doc.Form.Fields(strDocFieldName) Is Nothing Then
							Dim strFieldValue As String = "{0}{1}"
							Dim strFehlcode As String = ""
							Dim strStd As String = ""

							If rRecord(String.Format("_Fehl{0}", i)) <> String.Empty Then  'And rRecord("LANr") = 100 Then
								strFehlcode = " (" & rRecord(String.Format("_Fehl{0}", i)) & ")"
							End If

							' möglicherweise ist das Feld bereits beschrieben!
							'If CType(doc.Form.Fields(strDocFieldName), PDFTextBoxField).Text <> String.Empty Then strFehlcode = String.Empty

							strStd = Val(CType(doc.Form.Fields(strDocFieldName), PDFTextBoxField).Text)
							If strStd = 0 Then strStd = "" Else strStd = String.Format("{0:f2}", Val(strStd))

							strFieldValue = String.Format(strFieldValue, strStd, strFehlcode)
							CType(doc.Form.Fields(strDocFieldName), PDFTextBoxField).Text = strFieldValue

						End If

					Next


				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Stundenlöhne füllen: {1}", strMethodeName, ex.ToString))
					Return String.Format("Error.{0}", ex.ToString)

				End Try
			End While


			Return strResult
		End Function

		Private Function LoadRPFehlTagData(ByVal rpNr As Integer) As IEnumerable(Of FehlData)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim result As List(Of FehlData) = Nothing
			Dim SQL2Open As String = "Select GetFeld, Bez_D From Tab_Fehlzeit Order By GetFeld"
			Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
			m_Logger.LogDebug(String.Format("{0}. RPNr: {1} wird bearbeitet...", strMethodeName, Me.GetSelectedRPNr))

			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(SQL2Open, Conn)
			cmd.CommandType = CommandType.Text
			Dim rRecord As SqlDataReader = cmd.ExecuteReader

			result = New List(Of FehlData)

			Try
				While rRecord.Read
					Dim FehlDayData As New FehlData

					FehlDayData.FehlCode = m_utility_SP.SafeGetString(rRecord, "GetFeld")
					FehlDayData.FehlBezeichnung = m_utility_SP.SafeGetString(rRecord, "Bez_D")

					result.Add(FehlDayData)

				End While


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				result = Nothing

			End Try


			Return result
		End Function



		Function FillRPSonstigeDataIntoPDF(ByVal doc As PDFDocument) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "success"
			Dim SQL2Open As String = "[Get RPSonstigeCData 4 Print in RPContent]"
			Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
			m_Logger.LogDebug(String.Format("{0}. RPNr: {1} wird bearbeitet...", strMethodeName, Me.GetSelectedRPNr))

			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(SQL2Open, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@RPNr", Me.GetSelectedRPNr)
			Dim rRecord As SqlDataReader = cmd.ExecuteReader
			Dim strDocFieldName As String = "Rapport"
			Dim strFieldValue As String = String.Empty

			While rRecord.Read
				Try
					strFieldValue &= String.Format("{2:f3}{1}{3}{1}{5:n2} * {6:n2} * {7:n2}% = {4:n2} CHF{0}", vbNewLine, vbTab,
																				 m_utility_SP.SafeGetDecimal(rRecord, "LANr", 0),
																				 m_utility_SP.SafeGetString(rRecord, "Bezeichnung"),
																				 m_utility_SP.SafeGetDecimal(rRecord, "Betrag", 0),
																				 m_utility_SP.SafeGetDecimal(rRecord, "Anzahl", 0),
																				 m_utility_SP.SafeGetDecimal(rRecord, "m_Basis", 0),
																				 m_utility_SP.SafeGetDecimal(rRecord, "m_Ansatz", 0))

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Sonstige Daten füllen: {1}", strMethodeName, ex.Message))
					Return String.Format("Error.{0}", ex.Message)

				End Try
			End While
			If strSonstigeLA <> String.Empty Then strFieldValue &= String.Format("{0}Stundenlohnarten:{0}{1}", vbNewLine, strSonstigeLA)

			Dim Fehldata = LoadRPFehlTagData(Me.GetSelectedRPNr)

			If Not Fehldata Is Nothing Then
				Dim FehlString As String = String.Empty

				For i = 0 To Fehldata.Count - 1
					FehlString &= String.Format("{1} = {2}{0}", vbNewLine, Fehldata(i).FehlCode, Fehldata(i).FehlBezeichnung)
				Next

				If FehlString <> String.Empty Then strFieldValue &= String.Format("{0}Fehlcodes:{0}{1}", vbNewLine, FehlString)

			End If


			If Not doc.Form.Fields(strDocFieldName) Is Nothing Then
				CType(doc.Form.Fields(strDocFieldName), PDFTextBoxField).Text = strFieldValue

			End If

			Return strResult
		End Function

		Function CreatedPDFDocWithMA(ByVal _iRPNr As Integer) As String

			Dim strResult As String = "Success..."
			Dim doc As New PDFDocument(Me.RPCFile2Open)

			Try
				doc.SerialNumber = _ClsProgSetting.GetPDF_O2SSerial
			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Serialnumber: {0}", ex.ToString))
				Return String.Format("Error.Serialnumber.{0}", ex.ToString)

			End Try

			strResult = FillMDDataIntoPDF(doc)
			If Not strResult.ToLower.Contains("error") Then strResult = FillRPDataIntoPDF(doc)
			If Not strResult.ToLower.Contains("error") Then strResult = FillRPTagDataIntoPDF(doc)
			If Not strResult.ToLower.Contains("error") Then strResult = FillRPSonstigeDataIntoPDF(doc)


			Dim strExportPfad As String = m_InitializationData.UserData.spTempRepportPath
			If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempRepportPath)

			Dim filename As String = String.Format("Rapport_{0}.pdf", Environment.TickCount)
			If File.Exists(Path.Combine(strExportPfad, filename)) Then
				Try
					File.Delete(Path.Combine(strExportPfad, filename))
				Catch ex As Exception
					filename = String.Format("Rapport_{0}.pdf", Guid.NewGuid.ToString)
				End Try
			End If

			Dim strFilename As String = Path.Combine(strExportPfad, filename)
			m_Logger.LogDebug(String.Format("RPNr {0} is processing in file {1}", _iRPNr, strFilename))
			Try
				doc.Form.FlattenFormFields()

				doc.Save(strFilename)
				liRPCFile.Add(strFilename)

			Catch ex As Exception
				Return String.Format("Error.{0}", ex.Message)

			End Try

			Return strFilename
		End Function


#Region "helpers"

		Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

		Private Function StrToBool(ByVal str As String) As Boolean

			Dim result As Boolean = False

			If String.IsNullOrWhiteSpace(str) Then
				Return False
			ElseIf str = "1" Then
				Return True

			End If

			Boolean.TryParse(str, result)

			Return result
		End Function

#End Region


	End Class


	Public Class FehlData

		Public Property FehlCode As String
		Public Property FehlBezeichnung As String

	End Class


End Namespace
