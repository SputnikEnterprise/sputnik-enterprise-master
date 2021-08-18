

'Imports System.IO
'Imports System.Xml.XPath
'Imports SP.Infrastructure.UI
'Imports SP.Infrastructure.Logging


'Public Class ClsXMLUtility

'	Private Shared m_Logger As ILogger = New Logger()


'	''' <summary>
'	''' creates new \\server\mdxx\year\programm.xml file 
'	''' </summary>
'	''' <param name="strFileName"></param>
'	''' <param name="mandantNumber"></param>
'	''' <remarks></remarks>
'	Sub CreateXMLFile4Mandant(ByVal strFileName As String, ByVal mandantNumber As Integer)
'		Dim enc As New System.Text.UnicodeEncoding
'		Dim m_md As New SPProgUtility.Mandanten.Mandant
'		Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
'		Dim m_reg = New SPProgUtility.ClsDivReg

'		If mandantNumber = 0 Then mandantNumber = m_md.GetDefaultMDNr
'		Dim strStartElementName As String = String.Format("MD_{0}", mandantNumber)
'		Dim strAttribute As String = String.Empty
'		Dim strField_2 As String = "LV"
'		Dim strField_3 As String = ""
'		Dim m_PayrollSetting As String = String.Format("MD_{0}/Lohnbuchhaltung", mandantNumber)

'		Dim xmlMandantProfile = m_md.GetSelectedMDDataXMLFilename(mandantNumber, Now.Year)

'		Dim strMyMDDatFile As String = String.Format("{0}\Programm.dat", System.IO.Directory.GetParent(xmlMandantProfile).FullName)	 ' _ClsProgSetting.GetMDIniFile()
'		Dim strSection As String = String.Empty
'		Dim strQuery As String = String.Empty
'		Dim strAbschnitt As String = ""


'		strAttribute = "startnewbvgcalculationnow"
'		Dim value As String = m_path.GetXMLNodeValue(xmlMandantProfile, String.Format("{0}/startnewbvgcalculationnow", m_PayrollSetting))
'		Dim startNewBVGCalculationNow As Boolean?
'		If Not String.IsNullOrWhiteSpace(value) Then
'			startNewBVGCalculationNow = m_path.ParseToBoolean(value, Nothing)
'		End If

'		strAttribute = "donotcontrolbvgMinLohn"
'		value = m_path.GetXMLNodeValue(xmlMandantProfile, String.Format("{0}/donotcontrolbvgMinLohn", m_PayrollSetting))
'		Dim donotcontrolbvgMinLohn As Boolean?
'		If Not String.IsNullOrWhiteSpace(value) Then
'			donotcontrolbvgMinLohn = m_path.ParseToBoolean(value, Nothing)
'		End If

'		Dim strOldFilename As String = System.IO.Path.Combine(m_path.GetSpS2DeleteHomeFolder(), "OldFile.xml")	' _ClsProgSetting.GetSpSFiles2DeletePath & "OldFile.xml"
'		Try
'			System.IO.File.Copy(strFileName, strOldFilename, True)
'		Catch ex As Exception
'			m_Logger.LogError(String.Format("{0}", ex.ToString))

'		End Try

'		' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 

'		Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFileName, enc)
'		Dim strValue As String = String.Empty

'		With XMLobj
'			' Formatierung: 4er-Einzüge verwenden 
'			.Formatting = Xml.Formatting.Indented
'			.Indentation = 4

'			.WriteStartDocument()
'			.WriteStartElement(strStartElementName)


'			strSection = "Sonstiges"
'			.WriteStartElement(strSection)
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "BewName"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "BewPLZOrt"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "CreditInfo"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			' Bewilligungsbehörden --------------------------------------------------------------------------------------------
'			strAttribute = "BewPostfach"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "BewStrasse"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "BewSeco"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "BURNumber"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "UIDNumber"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAR.-MitgliedNr"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "MDSelectedColor"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "Extension4Deletedrecs"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "KassaStartEndepflicht"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "KassaStartEndeAfter"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHV-Ausgleichskasse"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "Unfallversicherung"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "LL_IndentSize"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "20")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FontSize"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "11")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FontName"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "Calibri")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "LLTemplateExtension"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "DOC")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "MailFontName"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "Calibri")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "MailFontSize"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "11")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strQuery = "//Sonstiges/EnableLLDebug"
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
'			.WriteStartElement("EnableLLDebug")
'			.WriteString(strValue)
'			.WriteEndElement()

'			strQuery = "//Sonstiges/mandantcolor"
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
'			.WriteStartElement("mandantcolor")
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "payrollluepaymentmethodifempty"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, "Zahlart", "")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			' Sonstiges fertig schreiben...
'			.WriteEndElement()




'			'-------------------------------------------------------------------------------------------------------------------



'			strAbschnitt = "Lohnbuchhaltung"
'			.WriteStartElement("Lohnbuchhaltung")
'			strSection = "guthaben"
'			.WriteStartElement("guthaben")

'			strAttribute = "showguthabenpereaches"
'			strQuery = String.Format("//{0}/guthaben/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			' Sonstiges fertig schreiben...
'			.WriteEndElement()


'			strSection = "report"
'			.WriteStartElement("report")
'			strAttribute = "tagesspesenstdab"
'			strQuery = String.Format("//{0}/report/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then
'				strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "TagesSpesenStd", "8.25")
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			.WriteEndElement()



'			If startNewBVGCalculationNow.HasValue Then

'				strAttribute = "startnewbvgcalculationnow"
'				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'				strValue = GetXMLValueByQuery(strOldFilename, strQuery, startNewBVGCalculationNow)
'				.WriteStartElement(strAttribute)
'				.WriteString(strValue)
'				.WriteEndElement()

'			End If

'			If donotcontrolbvgMinLohn.HasValue Then
'				strAttribute = "donotcontrolbvgMinLohn"
'				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'				strValue = GetXMLValueByQuery(strOldFilename, strQuery, donotcontrolbvgMinLohn)
'				.WriteStartElement(strAttribute)
'				.WriteString(strValue)
'				.WriteEndElement()

'			End If

'			strAttribute = "payrollcheckfee"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "CheckVorschuss", "0")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "advancepaymentcheckfee"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "CheckVorschuss", "0")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "advancepaymenttransferfee"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "ZGKontoGebühr", "0")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "advancepaymentcashfee"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "ZGGebührBar", "0")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "advancepaymenttransferinternationalfee"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "ZGGebührAusland", "0")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "payrolltransferinternationalfee"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "LOGebührAusland", "0")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "bvgafter"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "BVGAbzugAfter", "0")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "bvginterval"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "BVGIntervalString", "ww")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "bvgintervaladd"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "BVGIntervalAdd", "")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "calculatebvgwithesdays"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "BVG_ArbTage", "0")
'			'End If
'			If strValue = "1" Or strValue = "true" Then
'				strValue = "true"
'			Else
'				strValue = "false"
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "kizulagenotiflanrcontains"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "KIAuZulagenNotLANr", "")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "stringforadvancepaymentwithfee"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "CharForZGAbzug", "")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "deletepayrollwithbruttozero"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "DeleteLOWithBrutto0", "0")
'			'End If
'			If strValue = "1" Or strValue = "true" Then
'				strValue = "true"
'			Else
'				strValue = "false"
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "calculatetaxbasiswithnot180"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "QSTBasisNot180", "0")
'			'End If
'			If strValue = "1" Or strValue = "true" Then
'				strValue = "true"
'			Else
'				strValue = "false"
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "taxprocentforborderforeigner"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			'If strValue = String.Empty Then
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "QSTAnsatzByGrenz", "0")
'			'End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			' Lohnbuchhaltung fertig schreiben...
'			.WriteEndElement()





'			strSection = "Export"	' ........................................................................................
'			.WriteStartElement(strSection)

'			' Achtung: (Export-Abschnitt)----------------------------------------------------------------------------------
'			strAttribute = "MA_SPUser_ID"
'			strQuery = String.Format("//Export/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "KD_SPUser_ID"
'			strQuery = String.Format("//Export/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Vak_SPUser_ID"
'			strQuery = String.Format("//Export/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Ver_SPUser_ID"
'			strQuery = String.Format("//Export/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			' Sonstiges fertig schreiben...
'			.WriteEndElement()


'			' Test ---------------------

'			strAbschnitt = "Export_1"
'			Dim strOrgAbschnitt As String = "Export"
'			.WriteStartElement(strAbschnitt)

'			strAttribute = "MA_SPUser_ID"
'			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "KD_SPUser_ID"
'			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Vak_SPUser_ID"
'			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Ver_SPUser_ID"
'			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			' Export_1 fertig schreiben...
'			.WriteEndElement()


'			strAbschnitt = "Template_1"
'			strOrgAbschnitt = "Template"
'			.WriteStartElement(strAbschnitt)

'			strAttribute = "cockpit-url"
'			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "cockpit-picture"
'			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			' Template_1 fertig schreiben...
'			.WriteEndElement()


'			' Abschluss der Test -------------------------------

'			strSection = "Templates" ' .....................................................................................
'			.WriteStartElement(strSection)
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "cockpit-picture"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "cockpit-url"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "ZeugnisDeckblatt"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AGBDeckblatt"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AGB4Temp"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "AGBTemp.PDF")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AGB4Fest"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "AGBFest.PDF")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "PMail_tplDocNr"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "18.0.1")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "customer-contact-subject"
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "customer-mail-contact-subject"
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "customer-fax-contact-subject"
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "customer-contact-body"
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "customer-sms-contact-subject"
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "customer-sms-contact-body"
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "CreatedLLFileAs"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "Dossier {0} {1}")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "eMailImageVar1"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "eMailImageValue1"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "eMailImageVar2"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "eMailImageValue2"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "eMailImageVar3"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "eMailImageValue3"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			' Achtung: (Templates-Abschnitt)----------------------------------------------------------------------------------
'			strAttribute = "Zwischenverdienstformular_Doc-eMail-Template"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_ZVDoc.txt")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "Arbeitgeberbescheinigung_Doc-eMail-Template"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_ArbgDoc.txt")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "MADoc-eMail-Template"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_MADoc.txt")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "KDDoc-eMail-Template"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_KDDoc.txt")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "ZHDDoc-eMail-Template"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_ZHDDoc.txt")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "Cockpit-eMail-Template"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_CockpitWOS.txt")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			' Achtung: (Templates-Abschnitt)----------------------------------------------------------------------------------
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "eMail-Template-JobNr"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "eMail-Template"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "MassenOffer-eMail-Template-JobNr"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "MassenOffer-eMail-Template"
'			strQuery = String.Format("//Templates/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			' Sonstiges fertig schreiben...
'			.WriteEndElement()




'			strAbschnitt = "Mailing"
'			strSection = "Mailing"
'			.WriteStartElement(strAbschnitt)

'			strAttribute = "faxusername"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "faxuserpw"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "ecalljobid"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "ecallfromtext"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "ecallheaderid"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "ecallheaderinfo"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "ecallsubject"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "ecallnotification"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "ecalltoken"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "faxextension"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "faxforwarder"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "davidserver"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "cockpitwww"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "SMTP-Server"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "SMTP-Port"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()



'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "Zwischenverdienstformular_Doc-eMail-Betreff"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "Arbeitgeberbescheinigung_Doc-eMail-Betreff"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "MADoc-eMail-Betreff"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "MADoc-WWW"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "KDDoc-eMail-Betreff"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "KDDoc-WWW"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "ZHDDoc-eMail-Betreff"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "ZHDDoc-WWW"
'			strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "Mail-Database"
'			strQuery = String.Format("//Mailing/{0}", strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			' Sonstiges fertig schreiben...
'			.WriteEndElement()





'			strSection = "SUVA-Daten"	' ....................................................................................
'			.WriteStartElement(strSection)
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "SUVAAddressZusatz"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "SUVAAddressZHD"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "SUVAAddressPostfach"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "SUVAAddressStrasse"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "SUVAAddressPLZOrt"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "Abrechnungsnummer"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "SUVAAddressSub1"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "SUVAAddressSub2"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "SUVAAddressSub3"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "SUVAAddressSub4"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "SUVAAddressSub5"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "SUVAAddressSub6"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			.WriteEndElement()


'			.WriteStartElement("AHV-Daten")

'			' AHV-Adressen -----------------------------------------------------------------------------------------------------
'			strAttribute = "AHVAddressZusatz"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHVAddressZHD"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHVAddressPostfach"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHVAddressStrasse"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHVAddressPLZOrt"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHVMitgliedNr"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AusgNummer"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHVSub1"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHVSub2"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHVSub3"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHVSub4"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHVSub5"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "AHVSub6"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			.WriteEndElement()


'			.WriteStartElement("Fak-Daten")

'			' FAK-Kasse -------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKKassenname"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressZusatz"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressZHD"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressPostfach"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressStrasse"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressPLZOrt"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressMitgliednr"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressNr"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressSub1"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressSub2"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressSub3"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressSub4"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressSub5"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "FAKAddressSub6"
'			strValue = m_reg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			'-------------------------------------------------------------------------------------------------------------------
'			.WriteStartElement("BuchungsKonten")

'			For i As Integer = 0 To 37
'				strAttribute = Str(i + 1).Trim
'				strValue = m_reg.GetINIString(strMyMDDatFile, "BuchungsKonten", strAttribute, "")
'				.WriteStartElement("_" & strAttribute)
'				.WriteString(strValue)
'				.WriteEndElement()
'			Next

'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			'-------------------------------------------------------------------------------------------------------------------
'			strAbschnitt = "StartNr"
'			.WriteStartElement(strAbschnitt)

'			strAttribute = "Mitarbeiter"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then
'				strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			End If
'			If strValue = String.Empty Then
'				strValue = "0"
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Kunden"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then
'				strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			End If
'			If strValue = String.Empty Then
'				strValue = "0"
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Vakanzennr"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Offers"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Einsatzverwaltung"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Rapporte"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Vorschussverwaltung"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Fakturen"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Zahlungseingänge"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Mahnungen"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Lohnabrechnung"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "FremdOP"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "Kassenbuch"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "FirstKassenBetrag"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
'			If strValue = String.Empty Then strValue = "0"
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------


'			'-------------------------------------------------------------------------------------------------------------------
'			'-------------------------------------------------------------------------------------------------------------------
'			strAbschnitt = "Debitoren"
'			.WriteStartElement(strAbschnitt)

'			strAttribute = "mwstnr"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Or Not strValue.StartsWith("CHE-") Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "mwstsatz"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Or Val(strValue) < 8 Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "mwst-satz", "8")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "ref10forfactoring"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "RefNrTo10", "0")
'			If strValue = "1" Or strValue = "true" Then
'				strValue = "true"
'			Else
'				strValue = "false"
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "ezonsepratedpage"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "Trenne die Einzahlungsscheine", "0")
'			If strValue = "1" Or strValue = "true" Then
'				strValue = "true"
'			Else
'				strValue = "false"
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "setfakdatetoendofreportmonth"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "FakDateAsRPEndDate", "0")
'			If strValue = "1" Or strValue = "true" Then
'				strValue = "true"
'			Else
'				strValue = "false"
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "create3mahnasuntilnotpaid"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "Create3MahnByNotPaid", "0")
'			If strValue = "1" Or strValue = "true" Then
'				strValue = "true"
'			Else
'				strValue = "false"
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "printezwithmahn"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "MahnWithEZ", "0")
'			If strValue = "1" Or strValue = "true" Then
'				strValue = "true"
'			Else
'				strValue = "false"
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "printguonmahnung"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "MahnWithGutschrift", "0")
'			If strValue = "1" Or strValue = "true" Then
'				strValue = "true"
'			Else
'				strValue = "false"
'			End If
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			strAttribute = "mahnspesenab"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "Mahnspesen ab", "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "mahnspesenchf"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "mahnspesen", "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "verzugszinsdaysafter"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "verzugszinson", "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "verzugszinspercent"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "verzugszinsen", "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "verzugszinsabchf"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "Verzugszins Ab", "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "factoringcustomernumber"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "factoskdnr", "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "invoicezipfilename"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "opzipfilename", "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			'-------------------------------------------------------------------------------------------------------------------


'			strAbschnitt = "Licencing"
'			.WriteStartElement(strAbschnitt)

'			strAttribute = "sesam"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "abacus"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "swifac"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "comatic"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "kmufactoring"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "csoplist"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "parifond"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "customermng_deltavistaSolvencyCheckReferenceNumber"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "customermng_deltavistaWebServiceUserName"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "customermng_deltavistaWebServicePassword"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			strAttribute = "customermng_deltavistaWebServiceUrl"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			.WriteEndElement()
'			'-------------------------------------------------------------------------------------------------------------------

'			strAbschnitt = "Interfaces"
'			strSection = "Interfaces"
'			.WriteStartElement(strAbschnitt)

'			strAttribute = "abalofieldtrennzeichen"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abalodarstellungszeichen"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abalorefnr"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abalogegenkonto"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abalomwstcode"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			strAttribute = "abaopfieldtrennzeichen"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abaopdarstellungszeichen"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abaoprefnr"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abaopgegenkonto"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abaopmwstcode"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			strAttribute = "abazefieldtrennzeichen"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abazedarstellungszeichen"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abazerefnr"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abazegegenkonto"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "abazemwstcode"
'			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			.WriteStartElement("webservices")

'			strAttribute = "webservicejobdatabase"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webservicebankdatabase"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wssps_services/spbankutil.asmx")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			' TODO: Muss nachher gelöscht werden!!!
'			strAttribute = "webserviceqstdatabase"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webservicegavdatabase"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webservicegavutility"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webservicewosemployeedatabase"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webservicewoscustomer"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webservicewosvacancies"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webservicejobchvacancies"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webserviceostjobvacancies"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webservicesuedostvacancies"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webserviceecall"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webservicepaymentservices"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wssps_services/spcustomerpaymentservices.asmx")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webservicetaxinfoservices"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wsSPS_services/SPEmployeeTaxInfoService.asmx")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webservicealkdatabase"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wsSPS_services/SPALKUtil.asmx")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()

'			strAttribute = "webserviceupdateinfoservices"
'			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
'			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wssps_services/SPUpdateUtilities.asmx")
'			.WriteStartElement(strAttribute)
'			.WriteString(strValue)
'			.WriteEndElement()


'			.WriteEndElement()

'			'-------------------------------------------------------------------------------------------------------------------
'			'-------------------------------------------------------------------------------------------------------------------
'			'-------------------------------------------------------------------------------------------------------------------

'			' Write the XML to file and close the writer.
'			.WriteEndElement()
'			.Flush()
'			.Close()

'		End With

'	End Sub


'	Public Function GetSafeStringFromXElement(ByVal xelment As XElement)

'		If xelment Is Nothing Then
'			Return String.Empty
'		Else

'			Return xelment.Value
'		End If

'	End Function

'	''' <summary>
'	''' Gibt die XML-Value einer Datei und Query aus...
'	''' </summary>
'	''' <param name="strFilename"></param>
'	''' <param name="strQuery"></param>
'	''' <param name="strValuebyNull"></param>
'	''' <returns></returns>
'	''' <remarks></remarks>
'	Function GetXMLValueByQueryWithFilename(ByVal strFilename As String, _
'															ByVal strQuery As String, _
'															ByVal strValuebyNull As String) As String
'		Dim bResult As String = String.Empty
'		Dim strBez As String = GetXMLNodeValue(strFilename, strQuery)

'		If strBez = String.Empty Then strBez = strValuebyNull

'		Return strBez
'	End Function

'	Function GetXMLNodeValue(ByVal strFileName As String, ByVal strQuery As String) As String
'		Dim strValue As String = String.Empty
'		Dim xmlDoc As New Xml.XmlDocument()
'		Dim xpNav As XPathNavigator
'		Dim xni As XPathNodeIterator

'		Try
'			If File.Exists(strFileName) Then
'				xmlDoc.Load(strFileName)
'				xpNav = xmlDoc.CreateNavigator()

'				xni = xpNav.Select(strQuery)
'				Do While xni.MoveNext()
'					strValue = xni.Current.Value

'				Loop
'			End If

'		Catch ex As Exception
'			m_Logger.LogError(ex.ToString)

'		End Try

'		Return strValue
'	End Function


'	Private Function GetXMLValueByQuery(ByVal strFilename As String, ByVal strQuery As String, ByVal strValuebyNull As String) As String
'		Dim bResult As String = String.Empty
'		Dim strBez As String = GetXMLNodeValue(strFilename, strQuery)

'		If strBez = String.Empty Then strBez = strValuebyNull

'		Return strBez
'	End Function




'End Class
