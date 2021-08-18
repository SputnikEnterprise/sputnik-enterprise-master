
Imports System.Data.SqlClient

Imports System.IO
Imports System.Xml

Imports System.Xml.XmlTextWriter
Imports System.Xml.XmlTextReader
Imports Microsoft.Win32
Imports SPProgUtility.Mandanten.Mandant
Imports SPProgUtility.ProgPath.ClsProgPath


Public Class ClsXMLFunc

  Private Declare Unicode Function GetPrivateProfileString Lib "kernel32" _
 Alias "GetPrivateProfileStringW" (ByVal lpApplicationName As String, _
 ByVal lpKeyName As String, ByVal lpDefault As String, _
 ByVal lpReturnedString As String, ByVal nSize As Int32, _
 ByVal lpFileName As String) As Int32

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private strConnString As String = _ClsProgSetting.GetConnString()
	Private Conn As New SqlConnection(strConnString)
	Private m_md As SPProgUtility.Mandanten.Mandant
	Private m_path As SPProgUtility.ProgPath.ClsProgPath



	Sub WriteFormDataToXML(ByVal strFileName As String, ByVal bShowMessasge As Boolean, ByVal mandantNumber As Integer)
		Dim enc As New System.Text.UnicodeEncoding ' .UTF8Encoding
		If mandantNumber = 0 Then mandantNumber = _ClsProgSetting.GetMDNr
		Return

		Dim strStartElementName As String = "Forms_Normaly"
		Dim strStartElementUName_1 As String = "KD_Fields"
		Dim strField_1 As String = "CtlName"
		Dim strAttribute As String = "Name"
		Dim strField_2 As String = "CtlLabel"
		Dim strField_3 As String = "CtlAlignment"
		Dim strQuery As String = String.Empty
		Dim strSection As String = String.Empty
		Dim strAbschnitt As String = String.Empty


		Dim xmlMandantProfile = m_md.GetSelectedMDDataXMLFilename(mandantNumber, Now.Year)

		Dim strMyMDDatFile As String = String.Format("{0}\Programm.dat", System.IO.Directory.GetParent(xmlMandantProfile).FullName)	 ' _ClsProgSetting.GetMDIniFile()

		'Dim strMyMDDatFile As String = _ClsProgSetting.GetMDIniFile()
		Dim strOldFilename As String = _ClsProgSetting.GetSpSFiles2DeletePath & "OldFile.xml"
		Try
			File.Copy(strFileName, strOldFilename, True)
		Catch ex As Exception

		End Try


		If bShowMessasge Then
			If File.Exists(strFileName) Then
				If MessageBox.Show("Die Datei existiert bereits. Soll die Datei neu angelegt werden?", "XML-Datei", _
										MessageBoxButtons.YesNo, _
										MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
			End If
		Else
			'If File.Exists(strFileName) Then Exit Sub
		End If

		Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFileName, enc)
		Dim strValue As String = String.Empty

		With XMLobj

			' Formatierung: 4er-Einzüge verwenden 
			.Formatting = Xml.Formatting.Indented
			.Indentation = 4

			' Dann fangen wir mal an: 
			.WriteStartDocument()

			.WriteStartElement(strStartElementName)

			.WriteComment("Headerinfos for Listviews")
			.WriteComment("Debitorenliste")
			.WriteStartElement("LV_Control") ' <Debitorenliste 
			.WriteAttributeString(strAttribute, "15be882f-5eb5-4f83-ad94-5721af563ca8")
			.WriteStartElement("HeaderString")
			.WriteString("_;OPNr;KDNr;Firmenname;Valuta;Betrag;Gedruckt am;Erstellt am")
			.WriteEndElement()
			.WriteStartElement("HeaderWidth")
			.WriteString("0-1;200-1;200-1;500-0;300-0;300-1;500-0;500-0")
			.WriteEndElement()
			.WriteEndElement()

			.WriteComment("Auszahlungsliste")
			.WriteStartElement("LV_Control") ' <ZG-Liste 
			.WriteAttributeString(strAttribute, "b0ff184d-9750-4aec-b79a-930163b36ab2")
			.WriteStartElement("HeaderString")
			.WriteString("ZGNr;MANr;Nach- / Vorname;Monat / Jahr;LANr;Betrag;Zahlung am; Gedruckt am;Erstellt am / durch")
			.WriteEndElement()
			.WriteStartElement("HeaderWidth")
			.WriteString("0-1;200-1;500-0;200-0;200-1;300-1;300-0;300-0;600-0")
			.WriteEndElement()
			.WriteEndElement()

			.WriteComment("Kundenumsatzliste")
			.WriteStartElement("LV_Control") ' <KDUmsatz-Liste 
			.WriteAttributeString(strAttribute, "34e02b48-59a7-4c5d-8b13-6bba72c98f33")
			.WriteStartElement("HeaderString")
			strValue = "Res;KDNr;Firmenname;Adresse;"
			strValue += "1. BetragOhne;1. Betragex;1. MwSt;1. Total;"
			strValue += "2. BetragOhne;2. Betragex;2. MwSt;2. Total;"
			strValue += "1. KST;2. KST;3. KST"
			.WriteString(strValue)
			.WriteEndElement()
			.WriteStartElement("HeaderWidth")

			strValue = "0-0;200-1;500-0;500-0;"
			strValue += "250-1;300-1;250-1;300-1;"
			strValue += "0-1;100-0;100-0;100-0;100-0;100-0;"
			strValue += "250-1;300-1;250-1;300-1;"
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			.WriteComment("Liste nicht erfasste Rapporte")
			.WriteStartElement("LV_Control") ' <Liste nicht erfasste Rapporte
			.WriteAttributeString(strAttribute, "9cce2b1b-8fcc-4965-9839-420736d7149c")
			.WriteStartElement("HeaderString")
			strValue = "Test;RPNr;MANr;KDNr;ESNr;Nach- / Vorname;Firma;"
			strValue += "Monat / Jahr;ESData;ES_Als;"
			strValue += "RPData;Wochennummer"
			.WriteString(strValue)
			.WriteEndElement()
			.WriteStartElement("HeaderWidth")

			strValue = "0-1;200-1;200-1;200-1;200-1;500-0;500-0;"
			strValue += "200-0;300-0;400-0;"
			strValue += "300-0;300-0"
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			' ---------------------------------------------------------------------------------------------------------
			.WriteComment("Feld automatisch ausfüllen: Kandidatenverwaltung")
			.WriteStartElement("TXT_Control")
			.WriteAttributeString(strAttribute, "f6c163f6-3dab-4db8-b8dd-a7cd19b7017c")

			.WriteStartElement("TXT_1")
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Currency", "Currency")
			If strValue <> String.Empty Then strValue = "CHF"
			.WriteString(strValue)
			.WriteEndElement()

			.WriteStartElement("TXT_2")
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Zahlart", "Zahlart")
			If strValue <> String.Empty Then strValue = "K"
			.WriteString(strValue)
			.WriteEndElement()

			.WriteStartElement("TXT_3")
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BVGCode", "BVGCode")
			If strValue <> String.Empty Then strValue = "9"
			.WriteString(strValue)
			.WriteEndElement()

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "QSTCodeB/L", "QSTCodeB/L")
			If strValue <> String.Empty Then
				.WriteStartElement("TXT_4")
				.WriteString(strValue)
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "QSTKanton", "QSTKanton")
			If strValue <> String.Empty Then
				.WriteStartElement("TXT_5")
				.WriteString(strValue)
				.WriteEndElement()
			End If

			.WriteStartElement("TXT_6")
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Rahmenarbeitsvertrag", "Rahmenarbeitsvertrag")
			If strValue <> String.Empty Then strValue = "0"
			.WriteString(strValue)
			.WriteEndElement()

			.WriteStartElement("TXT_7")
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "FerienBack", "FerienBack")
			If strValue <> String.Empty Then strValue = "0"
			.WriteString(strValue)
			.WriteEndElement()

			.WriteStartElement("TXT_8")
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "FeiertagBack", "FeiertagBack")
			If strValue <> String.Empty Then strValue = "0"
			.WriteString(strValue)
			.WriteEndElement()

			.WriteStartElement("TXT_9")
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "13. LohnBack", "13. LohnBack")
			If strValue <> String.Empty Then strValue = "0"
			.WriteString(strValue)
			.WriteEndElement()

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "NoKINoQSTKI", "NoKINoQSTKI")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement("TXT_10")
			.WriteString(strValue)
			.WriteEndElement()

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "NOVertragNoLO", "NOVertragNoLO")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement("TXT_11")
			.WriteString(strValue)
			.WriteEndElement()



			.WriteEndElement()



			' -----------------------------------------------------------
			.WriteComment("Feld automatisch ausfüllen: Kundenverwaltung")

			strAbschnitt = "Field_DefaultValues"
			.WriteStartElement(strAbschnitt)

			strAttribute = "currencyvalue"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "currency", "CHF")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "mainlanguagevalue"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Sprachfeld", "deutsch")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			strAttribute = "firstcreditlimitamount"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "secondcreditlimitamount"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "invoicetype"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Fakturacode", "R")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "invoiceremindercode"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Mahncode", "A")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "conditionalcash"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			strAttribute = "vattabable"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MWSTPflicht", "true")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "vatable"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MWSTPflicht", "true")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "copykstintoreportline"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "hoursroundkind"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "1")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			strAttribute = "warnbycreditlimitexceeded"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "customernotuse"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "NOKDES", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			' Employee ___________________________________________________________________________________________________________________

			strAttribute = "employeezahlart"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Zahlart", "K")
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "employeebvgcode"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BVGCode", "9")
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "employeebvgcodewithchild"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BVGCode_2", "1")
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			strAttribute = "employeerahmenarbeitsvertrag"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Rahmenarbeitsvertrag", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "employeeferienback"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "FerienBack", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "employeefeiertagback"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "FeiertagBack", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "employee13lohnback"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "13. LohnBack", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "employeenoes"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "false")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "employeenolo"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "false")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "employeenozg"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "false")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "employeedes"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "false")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "childerlacantonsameastaxcanton"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "false")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "employeesecsuvacode"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "essuvacode"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "SuvaCode", "A1")
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "esendebynull"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "unbestimmt")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "eszeit"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "esort"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "esvertrag"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "esverleih"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "esreportsnotprint"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, 2)
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "escalcferienway"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "Ferien", "0")
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "escalc13lohnway"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "13. Lohn", "2")
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			strAttribute = "ask4transferverleihtowos"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "WOS_KD_Verleih", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "warnbynocustomercreditlimit"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Warning_IF_NOKDKreditLimitExists", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "companyallowednopvl"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "NotPVL", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "selectadvisorkst"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "GetIndKstInES", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "calculatecustomerrefundinmarge"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "ES_KD_UmsMin", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "setsalarydatetotodayines"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()




			strAttribute = "employeesstate"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "emplyoeefstate"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "employeecontact"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()



			' Reports ---------------------------------------------------------------------------------

			strAttribute = "allowedflextimeinreports"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "KompensMoreStdInRP", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "getflextimefrommandantdatabase"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()




			' Vorschuss ---------------------------------------------------------------------------------

			strAttribute = "advancepaymentdefaultpaymenttype"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "setpayoutdatetotodayinadvancepayment"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "advancepaymentwithfee"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "advancepaymentpaymentreason"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "à Konto")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "advancepaymentprintaftercreate8900and8930"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "advancepaymentprintaftercreate8920"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "advancepaymentopenformaftercreate"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			strAttribute = "payrollopenprintformaftercreate"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			.WriteEndElement()





			strAbschnitt = ""
			strSection = ""
			strAttribute = "Name"

			' -----------------------------------------------------------
			.WriteComment("Feld automatisch ausfüllen: Vorschlagverwaltung")
			.WriteStartElement("TXT_Control")
			.WriteAttributeString(strAttribute, "DAFD82EB-5D35-4675-84DF-DD6DB3009B17")

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "P_State", "Offen")
			If strValue <> String.Empty Then
				.WriteStartElement("Cbo_Status")
				strValue &= "##Status des Vorschlages#1"
				.WriteString(strValue)
				.WriteEndElement()
			End If
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "P_Art")
			If strValue <> String.Empty Then
				.WriteStartElement("Cbo_Art")
				.WriteString(strValue)
				.WriteEndElement()
			End If
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "P_Anstellung")
			If strValue <> String.Empty Then
				.WriteStartElement("Cbo_Anstellung")
				.WriteString(strValue)
				.WriteEndElement()
			End If

			.WriteStartElement("txt_Bezeichnung")
			strValue = "##Bezeichnung#1"
			.WriteString(strValue)
			.WriteEndElement()

			.WriteStartElement("Cbo_Berater_1")
			strValue = "##1. Betreuer#1"
			.WriteString(strValue)
			.WriteEndElement()

			.WriteStartElement("txt_MANr")
			strValue = "##Kandidatennummer#1"
			.WriteString(strValue)
			.WriteEndElement()

			.WriteStartElement("txt_KDNr")
			strValue = "##Kundennummer#1"
			.WriteString(strValue)
			.WriteEndElement()

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "P_Arbbegin")
			If strValue <> String.Empty Then
				.WriteStartElement("txt_ArbBegin")
				.WriteString(strValue)
				.WriteEndElement()
			End If
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "P_Tarif")
			If strValue <> String.Empty Then
				.WriteStartElement("txt_Tarif")
				.WriteString(strValue)
				.WriteEndElement()
			End If
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "P_Lohn")
			If strValue <> String.Empty Then
				.WriteStartElement("txt_Lohn")
				.WriteString(strValue)
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "P_Zusatz_1")
			If strValue <> String.Empty Then
				.WriteStartElement("txt_Zusatz1")
				.WriteString(strValue)
				.WriteEndElement()
			End If
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "P_Zusatz_2")
			If strValue <> String.Empty Then
				.WriteStartElement("txt_Zusatz2")
				.WriteString(strValue)
				.WriteEndElement()
			End If
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "P_Zusatz_3")
			If strValue <> String.Empty Then
				.WriteStartElement("txt_Zusatz3")
				.WriteString(strValue)
				.WriteEndElement()
			End If
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "P_Zusatz_4")
			If strValue <> String.Empty Then
				.WriteStartElement("txt_Zusatz4")
				.WriteString(strValue)
				.WriteEndElement()
			End If


			.WriteEndElement()


			' -----------------------------------------------------------
			.WriteComment("Feld automatisch ausfüllen: Offertenverwaltung")
			.WriteStartElement("TXT_Control")
			.WriteAttributeString(strAttribute, "a08c1a00-c12a-425f-a3d7-4f5fdf249c0c")

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OfferESBeginn", "OfferESBeginn")
			If strValue <> String.Empty Then
				.WriteStartElement("TXT_1")
				.WriteString(strValue)
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OfferESSpesen", "OfferESSpesen")
			If strValue <> String.Empty Then
				.WriteStartElement("TXT_2")
				.WriteString(strValue)
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OfferESArbzeit", "OfferESArbzeit")
			If strValue <> String.Empty Then
				.WriteStartElement("TXT_3")
				.WriteString(strValue)
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Offer1Bemerk", "Offer1Bemerk")
			If strValue <> String.Empty Then
				.WriteStartElement("TXT_4")
				.WriteString(strValue)
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Offer2Bemerk", "Offer2Bemerk")
			If strValue <> String.Empty Then
				.WriteStartElement("TXT_5")
				.WriteString(strValue)
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Offer3Bemerk", "Offer3Bemerk")
			If strValue <> String.Empty Then
				.WriteStartElement("TXT_6")
				.WriteString(strValue)
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Offer4Bemerk", "Offer4Bemerk")
			If strValue <> String.Empty Then
				.WriteStartElement("TXT_7")
				.WriteString(strValue)
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Offer5Bemerk", "Offer5Bemerk")
			If strValue <> String.Empty Then
				.WriteStartElement("TXT_8")
				.WriteString(strValue)
				.WriteEndElement()
			End If


			.WriteEndElement()


			' -----------------------------------------------------------
			.WriteComment("Feld automatisch ausfüllen: Vorschuss")
			.WriteStartElement("TXT_Control")
			.WriteAttributeString(strAttribute, "ce1dbf0c-d3f4-4a3b-a815-5699eddc07c0")
			.WriteStartElement("TXT_1")
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Vorschussauszahlung", "ZGGrund", "ZGGrund")
			If strValue = String.Empty Then strValue = "à Konto"
			.WriteString(strValue)
			.WriteEndElement()

			.WriteEndElement()

			' -----------------------------------------------------------
			.WriteComment("Feld automatisch anfügen: Lohnabrechnung")
			.WriteStartElement("TXT_Control")
			.WriteAttributeString(strAttribute, "2d98634a-9e5b-439a-a3ec-a726b93e96a7")
			.WriteStartElement("TXT_1")
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "CharForZGAbzug", "CharForZGAbzug")
			If strValue = String.Empty Then strValue = "*"
			.WriteString(strValue)
			.WriteEndElement()

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "ShowAllMAInLO", "ShowAllMAInLO")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement("TXT_2")
			.WriteString(strValue)
			.WriteEndElement()

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "DeletLOWithBrutto0", "DeletLOWithBrutto0")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement("TXT_3")
			.WriteString(strValue)
			.WriteEndElement()

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "QSTBasisNot180", "QSTBasisNot180")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement("TXT_4")
			.WriteString(strValue)
			.WriteEndElement()

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "QSTAnsatzByGrenz", "QSTAnsatzByGrenz")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement("TXT_5")
			.WriteString(strValue)
			.WriteEndElement()



			.WriteEndElement()


			' -----------------------------------------------------------
			.WriteComment("Feld automatisch anfügen: Lohnartenstamm")
			.WriteStartElement("TXT_Control")
			.WriteAttributeString(strAttribute, "20667772-b02b-490d-8b61-1a61b124633d")

			.WriteStartElement("TXT_1")
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "FerFeierNoInDistrain", "FerFeierNoInDistrain")
			If strValue = String.Empty Then strValue = "0"
			.WriteString(strValue)
			.WriteEndElement()


			.WriteEndElement()



			' -----------------------------------------------------------
			' Lohnabrechnungen drucken...
			strSection = "Lohnbuchhaltung"
			.WriteStartElement(strSection)

			Dim strKey As String = "PrintFeiertagInLO".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strKey)
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strKey, "0")
			.WriteStartElement(strKey)
			.WriteString(strValue)
			.WriteEndElement()

			strKey = "PrintFerienInLO".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strKey)
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strKey, "0")
			.WriteStartElement(strKey)
			.WriteString(strValue)
			.WriteEndElement()

			strKey = "Print13LohnInLO".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strKey)
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strKey, "0")
			.WriteStartElement(strKey)
			.WriteString(strValue)
			.WriteEndElement()

			strKey = "PrintDarlehenInLO".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strKey)
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strKey, "0")
			.WriteStartElement(strKey)
			.WriteString(strValue)
			.WriteEndElement()

			strKey = "PrintGleitStdInLO".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strKey)
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strKey, "0")
			.WriteStartElement(strKey)
			.WriteString(strValue)
			.WriteEndElement()

			strKey = "PrintNightStdInLO".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strKey)
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strKey, "0")
			.WriteStartElement(strKey)
			.WriteString(strValue)
			.WriteEndElement()

			.WriteEndElement()




			' -----------------------------------------------------------
			.WriteComment("Feld automatisch anfügen: Rapportverwaltung")
			.WriteStartElement("TXT_Control")
			.WriteAttributeString(strAttribute, "f8a5e8b8-4e13-4030-ad81-a5022314f478")

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "AutoProzentsatz", "AutoProzentsatz")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement("TXT_1")
			.WriteString(strValue)
			.WriteEndElement()

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "KompensMoreStdInRP", "KompensMoreStdInRP")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement("TXT_2")
			.WriteString(strValue)
			.WriteEndElement()


			.WriteEndElement()



			' ===================================================================================================

			.WriteComment("Pflichtfelder")

			strAbschnitt = "requiredfields"
			.WriteStartElement(strAbschnitt)

			strAttribute = "gavselectionines"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ESGAVPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "kst1selectionines"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ESKST1Pflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "kst2selectionines"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ESKST2Pflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "timeselectionines"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ESUhr", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "eseinstufungselectionines"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "EinsatzeinstufungPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "esbrancheselectionines"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "EsBranchePflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			' employee
			strAttribute = "emplyoeeadvisorselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MABetreuerPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "emplyoeequalificationselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MABerufPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "emplyoeefstateselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MAStat1Pflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "emplyoeesstateselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MAStat2Pflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "emplyoeecontactselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MAKontaktPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "emplyoeepermitselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MABewPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "emplyoeepermitdateselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MABewBisPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "emplyoeehometownselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MAHeimatOrt", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "emplyoeesteuerselectionifnotch"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Or strValue = "1" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "emplyoeekirchensteuerselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KirchenQSTpflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "emplyoeesteuercantonselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KQST", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			' Vacancy

			strAttribute = "vacancygruppeselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "VakGruppePflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "vacancycontactselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "VakKontaktPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "vacancystateselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "VakStatPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "vacancyjobpostcodeselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = "" Then strValue = "false"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "vacancyjobcityselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "VakArbOrtPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "vacancyvorspannselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "vacancyactivityselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "vacancyrequirementselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "vacancyweofferselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "vacancycontonselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "VakKantonPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "vacancyregionselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "VakRegionPflichtig", "false")
			End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			' Customer
			strAttribute = "customeradvisorselection"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = "false"
			Else
				If strValue = "1" Or strValue = "true" Then
					strValue = "true"
				Else
					strValue = "false"
				End If
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			.WriteEndElement()



			.WriteComment("Pflichtfelder: Module")
			.WriteStartElement("Pflicht_Control")
			strAttribute = "Name"
			.WriteAttributeString(strAttribute, "f6c163f6-3dab-4db8-b8dd-a7cd19b7017c")

			.WriteEndElement()



			.WriteComment("Pflichtfelder: Quellesteueradresse")
			.WriteStartElement("Pflicht_Control")
			.WriteAttributeString(strAttribute, "28AF8263-4894-41e8-AAF9-AB6EC87851A8")

			.WriteStartElement("cbo_Kanton")
			strValue = "1#Kanton"
			.WriteString(strValue)
			.WriteEndElement()

			.WriteStartElement("txt_PLZ")
			strValue = "1#PLZ"
			.WriteString(strValue)
			.WriteEndElement()

			.WriteStartElement("txt_Ort")
			strValue = "1#Ortschaft"
			.WriteString(strValue)
			.WriteEndElement()


			.WriteEndElement()


			' Farben für Felder (Normal und Pflichtfeld!!!)
			.WriteStartElement("All_FieldsColor")

			strValue = CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\Colour", _
																								 "LostFocusNormalColor")))
			If strValue = String.Empty Then strValue = "16777215"
			.WriteStartElement("NormalFields")
			.WriteString(strValue)
			.WriteEndElement()

			' Pflichtfelder
			strValue = CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\Colour", _
																								 "LostFocusPflichtColor")))
			If strValue = String.Empty Then strValue = "12779261"
			.WriteStartElement("MustFields")
			.WriteString(strValue)
			.WriteEndElement()

			' Forecolor für Gruppenüberschrift
			strQuery = "//All_FieldsColor/Gruppenüberschrift"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, 12779261)
			.WriteStartElement("Gruppenüberschrift")
			.WriteString(strValue)
			.WriteEndElement()

			' Backcolor für Popup-Fenster
			strQuery = "//All_FieldsColor/PopupBackColor"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, 12779261)
			.WriteStartElement("PopupBackColor")
			.WriteString(strValue)
			.WriteEndElement()


			.WriteEndElement()


			' Die Metro Design vom LOliste ... ----------------------------------------------------------------------
			' ----------------------------------------------------------------------

			.WriteComment("LOListe Metro Design")
			strAttribute = "Name"

			.WriteStartElement("MetroDesign")
			.WriteAttributeString(strAttribute, "d1679073-4d16-4136-8b3f-8d0bd2fdf797")

			' Tile-Size
			strQuery = String.Format("//MetroDesign[@Name={0}{1}{0}]/TileSize", _
																					 Chr(34), "d1679073-4d16-4136-8b3f-8d0bd2fdf797")
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, 120)
			.WriteStartElement("TileSize")
			.WriteString(strValue)
			.WriteEndElement()

			' TileGroup-Indent
			strQuery = String.Format("//MetroDesign[@Name={0}{1}{0}]/TileGroupIndent", _
																					 Chr(34), "d1679073-4d16-4136-8b3f-8d0bd2fdf797")
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, 56)
			.WriteStartElement("TileGroupIndent")
			.WriteString(strValue)
			.WriteEndElement()

			' TileItems-Indent
			strQuery = String.Format("//MetroDesign[@Name={0}{1}{0}]/TileItemsIndent", _
																					 Chr(34), "d1679073-4d16-4136-8b3f-8d0bd2fdf797")
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, 10)
			.WriteStartElement("TileItemsIndent")
			.WriteString(strValue)
			.WriteEndElement()

			' Tile-Backcolor
			For i As Integer = 0 To 27
				strQuery = String.Format("//MetroDesign[@Name={0}{1}{0}]/TileBackColor_{2}", _
																						 Chr(34), "d1679073-4d16-4136-8b3f-8d0bd2fdf797", i)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue <> "" Then
					.WriteStartElement(String.Format("TileBackColor_{0}", i))
					.WriteString(strValue)
					.WriteEndElement()
				End If

				strQuery = String.Format("//MetroDesign[@Name={0}{1}{0}]/TileForeColor_{2}", _
																						 Chr(34), "d1679073-4d16-4136-8b3f-8d0bd2fdf797", i)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue <> "" Then
					.WriteStartElement(String.Format("TileForeColor_{0}", i))
					.WriteString(strValue)
					.WriteEndElement()
				End If

				strQuery = String.Format("//MetroDesign[@Name={0}{1}{0}]/TileIsLarge_{2}", _
																		 Chr(34), "d1679073-4d16-4136-8b3f-8d0bd2fdf797", i)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue <> "" Then
					.WriteStartElement(String.Format("TileIsLarge_{0}", i))
					.WriteString(strValue)
					.WriteEndElement()
				End If

			Next


			.WriteEndElement()


			' Rapportdaten... ----------------------------------------------------------------------
			' ----------------------------------------------------------------------
			' Layouts
			'.WriteStartElement("Layouts")




			strAbschnitt = "Layouts"
			.WriteStartElement(strAbschnitt)

			strAttribute = "openemployeeformmorethanonce"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "opencustomerformmorethanonce"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "openeinsatzformmorethanonce"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "openreportsformmorethanonce"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "openadvancedpaymentformmorethanonce"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "openinvoiceformmorethanonce"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "true")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()




			'.WriteEndElement()







			.WriteStartElement("MetroWindows")

			strQuery = "//Layouts/MetroWindows/WindowColorProperty_INFO"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "255; 255; 255-255; 165; 0")
			.WriteStartElement("WindowColorProperty_INFO")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//Layouts/MetroWindows/WindowColorProperty_WARN"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "255; 255; 255-255; 165; 0")
			.WriteStartElement("WindowColorProperty_WARN")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//Layouts/MetroWindows/WindowColorProperty_ERROR"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "255; 255; 255-255; 165; 0")
			.WriteStartElement("WindowColorProperty_ERROR")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			.WriteStartElement("ToastNotifications")

			strQuery = "//Layouts/ToastNotifications/BackColorProperty_INFO"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "255; 165; 0")
			.WriteStartElement("BackColorProperty_INFO")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//Layouts/ToastNotifications/ForeColorProperty_INFO"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0; 0; 0")
			.WriteStartElement("ForeColorProperty_INFO")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//Layouts/ToastNotifications/BackColorProperty_WARN"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "255; 165; 0")
			.WriteStartElement("BackColorProperty_WARN")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//Layouts/ToastNotifications/ForeColorProperty_WARN"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0; 0; 0")
			.WriteStartElement("ForeColorProperty_WARN")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//Layouts/ToastNotifications/BackColorProperty_ERROR"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "255; 165; 0")
			.WriteStartElement("BackColorProperty_ERROR")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//Layouts/ToastNotifications/ForeColorProperty_ERROR"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0; 0; 0")
			.WriteStartElement("ForeColorProperty_ERROR")
			.WriteString(strValue)
			.WriteEndElement()

			.WriteEndElement()
			.WriteEndElement()


			' Rapportdaten... ----------------------------------------------------------------------
			' ----------------------------------------------------------------------

			' Rapportdaten
			.WriteStartElement("RPSetting")

			strQuery = "//RPSetting/OpenRPWeekDropDown"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, 0)
			.WriteStartElement("OpenRPWeekDropDown")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//RPSetting/RPWeekDropDownMust"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, 0)
			.WriteStartElement("RPWeekDropDownMust")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//RPSetting/CheckTestCount"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, 5)
			.WriteStartElement("CheckTestCount")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//RPSetting/RPTestCount"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, 5)
			.WriteStartElement("RPTestCount")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//RPSetting/RPFilesize"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, 5)
			.WriteStartElement("RPFilesize")
			.WriteString(strValue)
			.WriteEndElement()


			.WriteEndElement()


			' Die Label-Bezeichnungen auf den Formen... ----------------------------------------------------------------------
			' ----------------------------------------------------------------------




			'strStartElementUName_1 = "BeraterIn"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()





			'.WriteAttributeString(strAttribute, "BeraterIn")
			'.WriteStartElement(strField_2) ' <KD 
			'.WriteString("BeraterIn")
			'.WriteEndElement()
			'.WriteEndElement()

			'' 1. Eigenschaft
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_KD_1Property")
			'If strValue = String.Empty Then strValue = "1. Eigenschaft"
			'If strValue <> String.Empty Then
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, "KD_1Property")

			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'' 2. Eigenschaft
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_KD_2Property")
			'If strValue = String.Empty Then strValue = "2. Eigenschaft"
			'If strValue <> String.Empty Then
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, "KD_2Property")
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD1Res", "1. Res")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "KD1Res"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD2Res", "2. Res")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "KD2Res"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD3Res", "3. Res")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "KD3Res"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD4Res", "4. Res")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "KD4Res"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD1Status", "1. Status")
			'If strValue = String.Empty Then strValue = "1. Status"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "KD1Status"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD2Status", "2. Status")
			'If strValue = String.Empty Then strValue = "2. Status"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "KD2Status"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KDKontakt", "Kontakt")
			'If strValue = String.Empty Then strValue = "Kontakt"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "KDKontakt"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KDResLbl", "Reserve Felder")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "KDResLbl"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If


			'' Zuständige Person
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Kontakt", "Kontakt")
			'If strValue = String.Empty Then strValue = "Kontakt"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ZHD_Kontakt"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_1State", "1. Status")
			'If strValue = String.Empty Then strValue = "1. Status"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ZHD_1State"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_2State", "2. Status")
			'If strValue = String.Empty Then strValue = "2. Status"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ZHD_2State"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Kommunikation", "Kommunikation")
			'If strValue = String.Empty Then strValue = "Kommunikation"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ZHD_Kommunikation"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Versand", "Versand")
			'If strValue = String.Empty Then strValue = "Versand"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ZHD_Versand"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Res1", "1. Res")
			'If strValue = String.Empty Then strValue = "1. Res"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ZHD_Res1"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Res2", "2. Res")
			'If strValue = String.Empty Then strValue = "2. Res"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ZHD_Res2"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Res3", "3. Res")
			'If strValue = String.Empty Then strValue = "3. Res"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ZHD_Res3"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Res4", "4. Res")
			'If strValue = String.Empty Then strValue = "4. Res"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ZHD_Res4"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If


			'' Einsatzverwaltung
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESTSpesen", "Tagesspesen")
			'If strValue = String.Empty Then strValue = "Tagesspesen"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ES_TSpesen"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ESEinstufung", "Einstufung")
			'If strValue = String.Empty Then strValue = "Einstufung"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ES_Einstufung"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If


			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz1", "1. Zusatz")
			'If strValue = String.Empty Then strValue = "1. Zusatz"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ES_1Zusatz"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz2", "2. Zusatz")
			'If strValue = String.Empty Then strValue = "2. Zusatz"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ES_2Zusatz"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz3", "3. Zusatz")
			'If strValue = String.Empty Then strValue = "3. Zusatz"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ES_3Zusatz"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz4", "4. Zusatz")
			'If strValue = String.Empty Then strValue = "4. Zusatz"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ES_4Zusatz"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz5", "5. Zusatz")
			'If strValue = String.Empty Then strValue = "5. Zusatz"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ES_5Zusatz"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz6", "6. Zusatz")
			'If strValue = String.Empty Then strValue = "6. Zusatz"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ES_6Zusatz"
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'' Allgemeine Felder...
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezKST1", "1. Kst.")
			'If strValue = String.Empty Then strValue = "1. Kst."
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "1. Kst."
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezKST2", "2. Kst.")
			'If strValue = String.Empty Then strValue = "2. Kst."
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "2. Kst."
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezKST3", "3. Kst.")
			'If strValue = String.Empty Then strValue = "3. Kst."
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "3. Kst."
			'  .WriteStartElement("Control") ' <KD 
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2) ' <KD 
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'' KD_Anstellungsart...
			'strValue = "Anstellungsart"
			'strStartElementUName_1 = "KD_Anstellungsarten"
			'.WriteStartElement("Control") ' <KD 
			'.WriteAttributeString(strAttribute, strStartElementUName_1)
			'.WriteStartElement(strField_2) ' <KD 
			'.WriteString(strValue)
			'.WriteEndElement()
			'.WriteEndElement()


			'' Kandidatenmaske
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezTelefon-P", "Telefon")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "Telefon privat"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezTelefon-1", "Fax privat")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "Fax privat"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezTelefon-2", "Telefon G.")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "Telefon G."
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezTelefon-3", "Fax G.")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "Fax G."
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "HQualifikation", "Haupqualifikation")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "Qualifikation"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Lbl_MA_Gemeinde", "Gemeinde")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "Gemeinde"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ResAuto", "Reserv")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "ResAuto"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Beurteilung", "Beurteilung")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "Beurteilung"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "SQualifikation", "Sonstige Qualifikation")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "Sonstige Qualifikation"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MAKommArt", "Kommunikationsart")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "Kommunikationsart"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MAKontakt", "Kontakt")
			'If strValue = String.Empty Then strValue = "Kontakt"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MAKontakt"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA1Status", "1. Status")
			'If strValue = String.Empty Then strValue = "1. Status"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MA1Status"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA2Status", "2. Status")
			'If strValue = String.Empty Then strValue = "2. Status"
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MA2Status"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Einstellungsart", "Anstellungsart")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "Anstellungsart"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA1Res", "1. Reserve")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MA1Res"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA2Res", "2. Reserve")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MA2Res"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA3Res", "3. Reserve")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MA3Res"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA4Res", "4. Reserve")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MA4Res"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA5Res", "5. Reserve")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MA5Res"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MAResLbl", "Reservefelder")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MAResLbl"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA1LOCheck", "MA1LOCheck")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MA1LOCheck"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA2LOCheck", "MA2LOCheck")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MA2LOCheck"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If

			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA3LOCheck", "MA3LOCheck")
			'If strValue <> String.Empty Then
			'  strStartElementUName_1 = "MA3LOCheck"
			'  .WriteStartElement("Control")
			'  .WriteAttributeString(strAttribute, strStartElementUName_1)
			'  .WriteStartElement(strField_2)
			'  .WriteString(strValue)
			'  .WriteEndElement()
			'  .WriteEndElement()
			'End If




			strAttribute = "Name"

			.WriteComment("Label.Text for Forms")


			strStartElementUName_1 = "BeraterIn"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, strStartElementUName_1)
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "Telefon privat"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, strStartElementUName_1)
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "Fax privat"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, strStartElementUName_1)
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "Telefon G."
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, strStartElementUName_1)
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "Fax G."
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, strStartElementUName_1)
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "Qualifikation"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, strStartElementUName_1)
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "Gemeinde"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, strStartElementUName_1)
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "ResAuto"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "Beurteilung"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "Sonstige Qualifikation"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, strStartElementUName_1)
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "Kommunikationsart"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, strStartElementUName_1)
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "MAKontakt"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Kontakt")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "MA1Status"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "1. Status")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "MA2Status"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "MA_Anstellungsarten"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Anstellungsarten")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "MA1Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "MA2Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "MA3Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "MA4Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "MA5Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "MAResLbl"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "KD_1Property"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "1. Eigenschaft")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "KD_2Property"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "KDKontakt"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Kontakt")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "KD1Status"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "1. Status")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "KD2Status"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "KD_Anstellungsarten"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Anstellungsarten")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "KD1Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "KD2Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "KD3Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "KD4Res"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "KDResLbl"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "KDMwStLbl"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "ZHD_Res1"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "ZHD_Res2"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "ZHD_Res3"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "ZHD_Res4"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "ZHD_Kontakt"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Kontakt")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "ZHD_1State"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "1. Status")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "ZHD_2State"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "ZHD_Kommunikation"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Kommunikation")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "ZHD_Versand"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Versand")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "es_einstufung"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Einstufung")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "es_branche"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Branche")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "ES_TSpesen"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Tagesspesen / Tag")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "ES_1Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "ES_2Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "ES_3Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "ES_4Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "ES_5Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "ES_6Zusatz"
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "1. Kst."
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "1. Kst.")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()


			strStartElementUName_1 = "2. Kst."
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "2. Kst.")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()

			strStartElementUName_1 = "3. Kst."
			strQuery = String.Format("//Control[@Name={0}{1}{0}]/CtlLabel", Chr(34), strStartElementUName_1)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Berater")
			.WriteStartElement("Control")
			.WriteAttributeString(strAttribute, strStartElementUName_1)
			.WriteStartElement("CtlLabel")
			.WriteString(strValue)
			.WriteEndElement()
			.WriteEndElement()














			' Kassenbuch...
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "CashRegBermerk1", "CashRegBermerk1")
			If strValue = String.Empty Then strValue = "1. Bemerkung"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "CashRegBermerk1"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "CashRegBermerk2", "CashRegBermerk2")
			If strValue = String.Empty Then strValue = "2. Bemerkung"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "CashRegBermerk2"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "CashRegBermerk3", "CashRegBermerk3")
			If strValue = String.Empty Then strValue = "3. Bemerkung"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "CashRegBermerk3"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			' Lohnartenstamm ----------------------------------------------------------------------------------------------------
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezReserve1", "BezReserve1")
			If strValue <> String.Empty Then
				strStartElementUName_1 = "LARes1"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezReserve2", "BezReserve2")
			If strValue <> String.Empty Then
				strStartElementUName_1 = "LARes2"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezReserve3", "BezReserve3")
			If strValue <> String.Empty Then
				strStartElementUName_1 = "LARes3"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezReserve4", "BezReserve4")
			If strValue <> String.Empty Then
				strStartElementUName_1 = "LARes4"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezReserve5", "BezReserve5")
			If strValue <> String.Empty Then
				strStartElementUName_1 = "LARes5"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			' Vorschlagverwaltung ------------------------------------------------------------------------------------------------
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "Bez_P_Zusatz_1", "Zusatz_1")
			If strValue = String.Empty Then strValue = "Zusatz_1"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Bez_P_Zusatz_1"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "Bez_P_Zusatz_2", "Zusatz_2")
			If strValue = String.Empty Then strValue = "Zusatz_2"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Bez_P_Zusatz_2"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "Bez_P_Zusatz_3", "Zusatz_3")
			If strValue = String.Empty Then strValue = "Zusatz_3"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Bez_P_Zusatz_3"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Propose", "Bez_P_Zusatz_4", "Zusatz_4")
			If strValue = String.Empty Then strValue = "Zusatz_4"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Bez_P_Zusatz_4"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If


			' Offertenverwaltung ------------------------------------------------------------------------------------------------
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenstatusBez", "OffertenstatusBez")
			If strValue = String.Empty Then strValue = "Status"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_Status"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenGruppeBez", "OffertenGruppeBez")
			If strValue = String.Empty Then strValue = "Gruppe"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_Gruppe"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If


			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenKontakteBez", "OffertenKontakteBez")
			If strValue = String.Empty Then strValue = "Kontakt"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_Kontakt"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenRes1Bez", "OffertenRes1Bez")
			If strValue = String.Empty Then strValue = "1. Reserve"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_Reserve1"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenRes2Bez", "OffertenRes2Bez")
			If strValue = String.Empty Then strValue = "2. Reserve"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_Reserve2"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenRes3Bez", "OffertenRes3Bez")
			If strValue = String.Empty Then strValue = "3. Reserve"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_Reserve3"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenRes4Bez", "OffertenRes4Bez")
			If strValue = String.Empty Then strValue = "4. Reserve"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_Reserve4"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenRes5Bez", "OffertenRes5Bez")
			If strValue = String.Empty Then strValue = "5. Reserve"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_Reserve5"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenBem1Bez", "OffertenBem1Bez")
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_MA_Bem1"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenBem2Bez", "OffertenBem2Bez")
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_MA_Bem2"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenBem3Bez", "OffertenBem3Bez")
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_MA_Bem3"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenBem4Bez", "OffertenBem4Bez")
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_MA_Bem4"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "OffertenBem5Bez", "OffertenBem5Bez")
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Offer_MA_Bem5"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If


			' Cockpit... --------------------------------------------------------------------------------------------------------
			strValue = "Willkommen {0} zu "
			If strValue = String.Empty Then strValue = "Willkommen {0} zu "
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Cockpit_LblHeader"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = "Statistik über {0}"
			If strValue = String.Empty Then strValue = "Statistik über {0}"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Cockpit_LblStatistik"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = "({0}) Aktivitäten der letzten {1} Tagen"
			If strValue = String.Empty Then strValue = "({0}) Aktivitäten der letzten {1} Tagen"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "lblAktivity"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = "({0}) Kontakte der letzten {1} Tagen"
			If strValue = String.Empty Then strValue = "({0}) Kontakte der letzten {1} Tagen"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "lblKontakt"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = "({0}) Offene Umsätze seit letzten {1} Tagen"
			If strValue = String.Empty Then strValue = "({0}) Offene Umsätze seit letzten {1} Tagen"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "lblOpenReBetragArt"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = "({0}) Umsätze seit letzten {1} Tagen"
			If strValue = String.Empty Then strValue = "({0}) Umsätze seit letzten {1} Tagen"
			If strValue <> String.Empty Then
				strStartElementUName_1 = "lblTotalReBetragArt"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			strValue = "({0}) Aktive Einsätze per Heute: {1}" & New String(" ", 1000)
			If strValue = String.Empty Then strValue = "({0}) Aktive Einsätze per Heute: {1}" & New String(" ", 1000)
			If strValue <> String.Empty Then
				strStartElementUName_1 = "Liste der aktiven Einsätze von Heute"
				.WriteStartElement("Control")
				.WriteAttributeString(strAttribute, strStartElementUName_1)
				.WriteStartElement(strField_2)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()
			End If

			' Cockpit... --------------------------------------------------------------------------------------------------------

			strSection = "Lohnausweis_NLA"
			.WriteStartElement(strSection)

			' Reserve Felder -------------------------------------------------------------------------------------------------------
			strAttribute = "NLA_2_3".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "NLA_3_0".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "NLA_4_0".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "NLA_7_0".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "NLA_13_1_2".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "NLA_13_2_3".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "NLA_Nebenleistung_1".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "NLA_Nebenleistung_2".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "NLA_Bemerkung_1".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "NLA_Bemerkung_2".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Orientation".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "DisableUserName".ToLower
			strQuery = String.Format("//{0}/{1}", strSection, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			'-------------------------------------------------------------------------------------------------------------------



			.WriteEndElement()

			' ... und schließen das XML-Dokument (und die Datei) 
			.Close()

			If bShowMessasge Then
				DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Die Datei {0} wurde erfolgreich erstellt.", strFileName), _
																							 "WriteDataToXML", MessageBoxButtons.OK, MessageBoxIcon.Information)
			End If

		End With

	End Sub


	Sub WriteDocExportSettingToXML()
		Dim enc As New System.Text.UnicodeEncoding
		Dim strFilename As String = _ClsProgSetting.GetUserProfileFile()
		Dim strStartElementName As String = "UserProfile"
		Dim strAttribute As String = "UserNr"
		Dim strField_2 As String = "Document"
		Dim strField_3 As String = ""
		Dim strXMLFile As String = _ClsProgSetting.GetFormDataFile
		Dim strOldUSProFile As String = System.IO.Directory.GetParent(_ClsProgSetting.GetUserProfileFile).FullName & "\UserPro" & _ClsProgSetting.GetLogedUSNr
		Dim strValue As String = String.Empty

		Dim strOldFilename As String = _ClsProgSetting.GetSpSFiles2DeletePath & "OldFile.xml"
		Try
			File.Copy(strFileName, strOldFilename, True)
		Catch ex As Exception

		End Try


		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing

		xDoc.Load(strXMLFile)

		Try

			Dim sSql As String = "Select JobNr, DocName From DokPrint Where (JobNr <> '' Or JobNr Is Not Null) And "
			sSql &= "(DocName <> '' or DocName is not null) Order By JobNr"
			Dim Conn As SqlConnection = New SqlConnection(strConnString)
			Dim bResult As Boolean = True

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			Conn.Open()
			Dim rDocrec As SqlDataReader = cmd.ExecuteReader


			xNode = xDoc.SelectSingleNode("*//Documents")
			If xNode Is Nothing Then
				xNode = xDoc.CreateNode(XmlNodeType.Element, "Documents", "")
				xDoc.DocumentElement.AppendChild(xNode)
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If

				Dim strKey As String = String.Empty
			End If



			Dim strMainKey As String = "//DocName[@ID='{0}']"
			Dim strGuid As String = String.Empty

			xDoc.Load(strXMLFile)


			While rDocrec.Read
				Try

					strValue = _ClsReg.GetINIString(strOldUSProFile, "Export", rDocrec("DocName"))
					If strValue = String.Empty Then strValue = "0"

					strGuid = rDocrec("JobNr")
					AddOrUpdateFieldLabelNode(xDoc, strGuid, strMainKey, strValue)


				Catch ex As Exception
					MsgBox(rDocrec("JobNr") & ": " & ex.Message, MsgBoxStyle.Critical, "WriteUSDocDataToXML_1")


				End Try

			End While


		Catch ex As Exception

		End Try


	End Sub


	Sub WriteUsDocDataToXML(ByVal strFileName As String, ByVal bShowMessasge As Boolean, ByVal userNumber As Integer, ByVal mandantNumber As Integer)
		Dim enc As New System.Text.UnicodeEncoding
		Dim strStartElementName As String = "UserProfile"
		Dim strAttribute As String = "UserNr"
		Dim strField_2 As String = "Document"
		Dim strField_3 As String = ""
		If userNumber = 0 Then userNumber = _ClsProgSetting.GetLogedUSNr
		If mandantNumber = 0 Then mandantNumber = _ClsProgSetting.GetMDNr

		Dim strOldUsINIFile As String = String.Format("{0}\UserPro{1}",
																									 System.IO.Directory.GetParent(strFileName).FullName,
																									 userNumber)

		Dim strOldFilename As String = _ClsProgSetting.GetSpSFiles2DeletePath & "OldFile.xml"
		Try
			File.Copy(strFileName, strOldFilename, True)
		Catch ex As Exception

		End Try

		' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
		If bShowMessasge Then
			If File.Exists(strFileName) Then
				If MessageBox.Show("Die Datei existiert bereits. Soll die Datei neu angelegt werden?", "XML-Datei", _
										MessageBoxButtons.YesNo, _
										MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
			End If
		Else
			'If File.Exists(strFileName) Then Exit Sub
		End If
		Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFileName, enc)
		Dim strValue As String = String.Empty

		Dim sSql As String = "Select JobNr, DocName From DokPrint Where (JobNr <> '' Or JobNr Is Not Null) And "
		sSql &= "(DocName <> '' or DocName is not null) Order By JobNr"
		Dim Conn As SqlConnection = New SqlConnection(strConnString)
		Dim bResult As Boolean = True

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		Conn.Open()
		Dim rDocrec As SqlDataReader = cmd.ExecuteReader

		Try

			With XMLobj
				' Formatierung: 4er-Einzüge verwenden 
				.Formatting = Xml.Formatting.Indented
				.Indentation = 4

				.WriteStartDocument()
				.WriteStartElement(strStartElementName)


				' Die Masken-Vorlage für Kundensuche...
				' Layouts
				.WriteStartElement("Layouts")
				.WriteStartElement("Form_DevEx")

				Dim strQuery As String = "//Layouts/Form_DevEx/FormStyle"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FormStyle")
				.WriteString(strValue)
				.WriteEndElement()

				strQuery = "//Layouts/Form_DevEx/NavbarStyle"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("NavbarStyle")
				.WriteString(strValue)
				.WriteEndElement()

				.WriteEndElement()
				.WriteEndElement()


				.WriteStartElement(String.Format("User_{0}", userNumber))
				.WriteStartElement("Documents")

				While rDocrec.Read
					Try

						strValue = _ClsReg.GetINIString(strOldUsINIFile, "Export", rDocrec("DocName"))
						If strValue = String.Empty Then strValue = "0"

						.WriteStartElement("DocName")
						.WriteAttributeString("ID", rDocrec("JobNr"))
						.WriteStartElement("Export")
						.WriteString(strValue)
						.WriteEndElement()
						.WriteEndElement()

					Catch ex As Exception
						MsgBox(rDocrec("JobNr") & ": " & ex.Message, MsgBoxStyle.Critical, "WriteUSDocDataToXML_1")


					End Try

				End While
				.WriteEndElement()


				' Die Masken-Vorlage für Kundensuche...
				.WriteStartElement("FormControls")
				.WriteStartElement("FormName")
				.WriteAttributeString("ID", "4c2db8b0-0521-4862-a640-d895e02100f9")
				.WriteStartElement("TemplateFile")
				.WriteString("")
				.WriteEndElement()
				.WriteEndElement()
				' Fertig

				' Die Sonstigen Einstellungen...
				.WriteStartElement("USSetting")
				.WriteStartElement("SettingName")
				.WriteAttributeString("ID", "Cockpit.DayAgo4GebDat")
				.WriteStartElement("USValue")
				.WriteString("")
				.WriteEndElement()
				.WriteEndElement()
				' Fertig
				.WriteEndElement()
				.WriteEndElement()
				.WriteEndElement()


				' CSV-Setting... ----------------------------------------------------------------------
				' ----------------------------------------------------------------------
				strQuery = ""

				.WriteComment("Export Einstellungen")
				strAttribute = "Name"

				.WriteStartElement("CSV-Setting")
				.WriteAttributeString(strAttribute, "MASearch")

				' SelectedFields
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "MASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("SelectedFields")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldIn
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "MASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FieldIn")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldSep
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "MASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
				.WriteStartElement("FieldSep")
				.WriteString(strValue)
				.WriteEndElement()

				' ExportFileName
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "MASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("ExportFileName")
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				.WriteStartElement("CSV-Setting")
				.WriteAttributeString(strAttribute, "KDSearch")

				' SelectedFields ---------------------------------------------------------------------------
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "KDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("SelectedFields")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldIn
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "KDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FieldIn")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldSep
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "KDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
				.WriteStartElement("FieldSep")
				.WriteString(strValue)
				.WriteEndElement()

				' ExportFileName
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "KDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("ExportFileName")
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				.WriteStartElement("CSV-Setting")
				.WriteAttributeString(strAttribute, "ESSearch")

				' SelectedFields -------------------------------------------------------------------------------------------
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "ESSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("SelectedFields")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldIn
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "ESSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FieldIn")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldSep
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "ESSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
				.WriteStartElement("FieldSep")
				.WriteString(strValue)
				.WriteEndElement()

				' ExportFileName
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "ESSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("ExportFileName")
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				.WriteStartElement("CSV-Setting")
				.WriteAttributeString(strAttribute, "ESKDSearch")

				' SelectedFields -------------------------------------------------------------------------------------------
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "ESKDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("SelectedFields")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldIn
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "ESKDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FieldIn")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldSep
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "ESKDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
				.WriteStartElement("FieldSep")
				.WriteString(strValue)
				.WriteEndElement()

				' ExportFileName
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "ESKDSearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("ExportFileName")
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				.WriteStartElement("CSV-Setting")
				.WriteAttributeString(strAttribute, "ESMASearch")

				' SelectedFields -------------------------------------------------------------------------------------------
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "ESMASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("SelectedFields")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldIn
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "ESMASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("FieldIn")
				.WriteString(strValue)
				.WriteEndElement()

				' FieldSep
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "ESMASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
				.WriteStartElement("FieldSep")
				.WriteString(strValue)
				.WriteEndElement()

				' ExportFileName
				strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "ESMASearch")
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement("ExportFileName")
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				' ExportSetting LO -------------------------------------------------------------------------------------------
				Dim sValue As String = String.Empty
				Dim strGuid As String = "SP.LO.PrintUtility"
				Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
				.WriteComment("Exporteinstellung für den Druck und Export Lohnabrechnungen...")
				.WriteStartElement("ExportSetting")
				.WriteAttributeString(strAttribute, strGuid)

				Dim strKeyName As String = "ExportPfad".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFinalFileFilename".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = _ClsProgSetting.GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFilename".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = _ClsProgSetting.GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				' ExportSetting ES -------------------------------------------------------------------------------------------
				sValue = String.Empty
				.WriteComment("Exporteinstellung für den Druck und Export Einsatzverträge...")
				strGuid = "SP.ES.PrintUtility"
				strMainKey = "//ExportSetting[@Name={0}{1}{0}]/{2}"
				.WriteStartElement("ExportSetting")
				.WriteAttributeString(strAttribute, strGuid)

				strKeyName = "ESUnterzeichner_ESVertrag".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = _ClsProgSetting.GetXMLValueByQuery(strOldFilename, strQuery, 0)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportPfad".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFilename_ESVertrag".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = _ClsProgSetting.GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFilename_Verleih".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = _ClsProgSetting.GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFinalFileFilename_ESVertrag".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = _ClsProgSetting.GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()

				strKeyName = "ExportFinalFileFilename_Verleih".ToLower
				strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
				strValue = _ClsProgSetting.GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strKeyName)
				.WriteString(strValue)
				.WriteEndElement()
				.WriteEndElement()


				' AdvancePayment Quitttung & Check ----------------------------------------------------------------------------------------
				sValue = String.Empty
				.WriteComment("AdvancePayment Quitttung and Check...")
				strKeyName = "advancepayment"
				.WriteStartElement(strKeyName)

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "maxvalue8900"
				strValue = _ClsReg.GetINIString(strOldUsINIFile, "ZG", "ZGMaxBetragBar", "0")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "maxvalue8930"
				strValue = _ClsReg.GetINIString(strOldUsINIFile, "ZG", "ZGMaxBetragCheck", "0")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				.WriteEndElement()


				.WriteEndElement()
				.Close()

				If bShowMessasge Then
					DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Die Datei {0} wurde erfolgreich erstellt.", strFileName), _
																										 "WriteUserDataToXML", MessageBoxButtons.OK, MessageBoxIcon.Information)
				End If

			End With

		Catch ex As Exception
			DevExpress.XtraEditors.XtraMessageBox.Show(String.Format("Erro: {0}", ex.Message), _
																								 "WriteUserDataToXML", MessageBoxButtons.OK, MessageBoxIcon.Error)

		End Try

	End Sub

	'Sub WriteUsFrmCtlDataToXML(ByVal strFileName As String, ByVal bShowMessasge As Boolean)
	'  Dim enc As New System.Text.UnicodeEncoding
	'  Dim strStartElementName As String = "UserProfile"
	'  Dim strAttribute As String = "UserNr"
	'  Dim strField_2 As String = "Document"
	'  Dim strField_3 As String = ""
	'  Dim strFilepath As String = _ClsProgSetting.GetSkinPath() & "4c2db8b0-0521-4862-a640-d895e02100f9\"

	'  Dim strOldFilename As String = _ClsProgSetting.GetSpSFiles2DeletePath & "OldFile.xml"
	'  Try
	'    File.Copy(strFileName, strOldFilename, True)
	'  Catch ex As Exception

	'  End Try

	'  Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFileName, enc)
	'  Dim strValue As String = String.Empty

	'  With XMLobj
	'    ' Formatierung: 4er-Einzüge verwenden 
	'    .Formatting = Xml.Formatting.Indented
	'    .Indentation = 4

	'    .WriteStartDocument()
	'    .WriteStartElement(strStartElementName)

	'    .WriteStartElement("FormControls")
	'    .WriteAttributeString("Name", "4c2db8b0-0521-4862-a640-d895e02100f9")
	'    .WriteStartElement("LibKDNr1")
	'    .WriteAttributeString("Visible", "true")
	'    .WriteEndElement()

	'    .WriteStartElement("txtKDNr_1")
	'    .WriteAttributeString("Visible", "true")
	'    .WriteEndElement()

	'    .WriteStartElement("txtKDNr_2")
	'    .WriteAttributeString("Visible", "true")
	'    .WriteEndElement()

	'    .WriteStartElement("LibKDNr2")
	'    .WriteAttributeString("Visible", "true")
	'    .WriteEndElement()

	'    .WriteEndElement()


	'    .WriteEndElement()
	'    .Close()

	'    If bShowMessasge Then
	'      MessageBox.Show("Die Datei " & strFileName & " wurde erfolgreich erstellt.", _
	'                      "WriteUsFrmCtlDataToXML", MessageBoxButtons.OK, MessageBoxIcon.Information)
	'    End If

	'  End With

	'End Sub

  Sub WriteSQLDataToXML(ByVal strFileName As String, ByVal bShowMessasge As Boolean)
    Dim enc As New System.Text.UnicodeEncoding
    Dim strStartElementName As String = "SQLProfile"
    Dim strAttribute As String = "Form"
    Dim strField_2 As String = "LV"
    Dim strField_3 As String = ""

    Dim strOldFilename As String = _ClsProgSetting.GetSpSFiles2DeletePath & "OldFile.xml"
    Try
      File.Copy(strFileName, strOldFilename, True)
    Catch ex As Exception

    End Try

    ' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
    If bShowMessasge Then
      If File.Exists(strFileName) Then
        If MessageBox.Show("Die Datei existiert bereits. Soll die Datei neu angelegt werden?", "XML-Datei", _
                    MessageBoxButtons.YesNo, _
                    MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
      End If
    Else
      'If File.Exists(strFileName) Then Exit Sub
    End If

    Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFileName, enc)
    Dim strValue As String = String.Empty

    With XMLobj
      ' Formatierung: 4er-Einzüge verwenden 
      .Formatting = Xml.Formatting.Indented
      .Indentation = 4

      .WriteStartDocument()
      .WriteStartElement(strStartElementName)

      ' -------------------------------------------------------------------------------------------------------------------
      .WriteStartElement("SPSOPSearch")
      .WriteStartElement("OPSearch")

      .WriteStartElement("SQLString")
      .WriteAttributeString("ID", "15be882f-5eb5-4f83-ad94-5721af563ca8")
      .WriteStartElement("SQL")
      strValue = "Select RE.*, Kunden.KreditLimite, Kunden.KreditLimiteAb As KDKreditlimiteAb, "
      strValue += "Kunden.KreditLimiteBis As KDKreditlimiteBis, Kunden.KreditLimite_2, Kunden.KDFiliale "
      strValue += "From RE Left Join Kunden "
      strValue += "On RE.KDNr = Kunden.KDNr"

      .WriteString(strValue)
      .WriteEndElement()
      .WriteEndElement()
      .WriteEndElement()
      .WriteEndElement()

      ' Liste der Auszahlung ... -----------------------------------------------------------------------------------------
      .WriteStartElement("SPSZGSearch")
      .WriteStartElement("ZGSearch")

      .WriteStartElement("SQLString")
      .WriteAttributeString("ID", "b0ff184d-9750-4aec-b79a-930163b36ab2")
      .WriteStartElement("SQL")
      strValue = "Select ZG.*, "
      strValue += "Mitarbeiter.Nachname, Mitarbeiter.Vorname From ZG "
      strValue += "Left Join Mitarbeiter On ZG.MANr = Mitarbeiter.MANr "
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("BetragSign")
      strValue = "0"
      .WriteString(strValue)

      .WriteEndElement()
      .WriteEndElement()
      .WriteEndElement()
      .WriteEndElement()
      ' -----------------------------------------------------------------------------------------------------

      ' Liste der KD-Umsatz ---------------------------------------------------------------------------------
      .WriteStartElement("SPSKDUmsatz")
      .WriteStartElement("frmKDUmsatz")

      .WriteStartElement("SQLString")
      .WriteAttributeString("ID", "34e02b48-59a7-4c5d-8b13-6bba72c98f33")
      .WriteStartElement("SQL_0")
      strValue = "Select RE.KDNr, "
      strValue += "Sum(RE.BetragOhne) As tBetragOhne, Sum(RE.BetragEx) As tBetragEx, Sum(RE.MwSt1) As tMwSt1, "
      strValue += "Sum(RE.BetragInk) As tBetragInk "
      strValue += "From RE Left Join Kunden On RE.KDNr = Kunden.KDNr "
      strValue += "Where RE.Art In ('A', 'I', 'G', 'F') "

      .WriteString(strValue)
      .WriteEndElement()

      ' _1 Zeile...
      .WriteStartElement("SQL_1")
      strValue = "Select KDUms.KDNr, "
      strValue += "KDUms.FBetragOhne As fBetragOhne, KDUms.fBetragEx As fBetragEx, "
      strValue += "KDUms.fBetragMwSt As fBetragMwSt, "
      strValue += "KDUms.fBetragInk As fBetragInk, "
      strValue += "KDUms.sBetragOhne As sBetragOhne, KDUms.sBetragEx As sBetragEx, "
      strValue += "KDUms.sBetragMwSt As sBetragMwSt, "
      strValue += "KDUms.sBetragInk As sBetragInk, "
      strValue += "Kunden.Firma1 As R_Name1, Kunden.Strasse As R_Strasse, Kunden.Land As R_Land, "
      strValue += "Kunden.PLZ As R_PLZ, Kunden.Ort As R_Ort, '' As REKst1, '' As REKst2, '' As Kst "
      strValue += "From KDRPUmsatz KDUms Left Join Kunden On KDUms.KDNr = Kunden.KDNr "

      .WriteString(strValue)
      .WriteEndElement()
      ' Ferting _1

      ' _2 Zeile ....
      .WriteStartElement("SQL_2")
      strValue = "Select RE.RENr, RE.KDNr, RE.R_Name1, RE.BetragOhne As fBetragOhne, "
      strValue += "RE.BetragEx As fBetragEx, RE.MwSt1 As fBetragMwSt, RE.BetragInk As fBetragInk, "
      strValue += "RE.R_Strasse, RE.R_PLZ, RE.R_Ort, RE.R_Land, RE.Fak_Dat, RE.KST, "
      strValue += "RE.REKST1, RE.REKST2, 0 As sBetragOhne, 0 As sBetragEx, 0 As sBetragMwSt, 0 As sBetragInk "

      strValue += "From RE Left Join Kunden On RE.KDNr = Kunden.KDNr "
      strValue += "Where RE.Art In ('A', 'I', 'G', 'F') "

      .WriteString(strValue)
      .WriteEndElement()
      ' Fertig _2

      .WriteEndElement()
      .WriteEndElement()
      .WriteEndElement()

      ' -----------------------------------------------------------------------------------------------------
      ' Liste der KD-Search ---------------------------------------------------------------------------------
      .WriteStartElement("SPSKDSearch")
      .WriteStartElement("frmKDSearch")

      .WriteStartElement("SQLString")
      .WriteAttributeString("ID", "4c2db8b0-0521-4862-a640-d895e02100f9")
      .WriteStartElement("SQL")
      strValue = "Select "
      strValue += "Kunden.KDNr, Kunden.FProperty, Kunden.Firma1, Kunden.PLZ As KDPLZ, "
      strValue += "Kunden.Ort As KDOrt, Kunden.Land As KDLand, "
      strValue += "Kunden.Telefon As KDTelefon, Kunden.Telefax As KDTelefax, "
      strValue += "Kunden.Strasse As KDStrasse, Kunden.KDState1, Kunden.KDState2, "
      strValue += "Kunden.HowKontakt As KDKontakt, Kunden.eMail As KDeMail, "
      strValue += "Kunden.Currency As KDCurrency, Kunden.Bemerkung As KDBemerkung, "
      strValue += "Kunden.Kreditlimite As KDKreditlimite, "
      strValue += "Kunden.KreditlimiteAb As KDKreditLimiteAb, "
      strValue += "Kunden.KreditlimiteBis As KDKreditLimiteBis, "

      strValue += "Kunden.Kreditlimite_2 As KDKreditlimite_2, "
      strValue += "Kunden.KDFiliale As KDAllFiliale, "

      strValue += "Kunden.Faktura As KDFaktura, "
      strValue += "Kunden.ZahlKond As KDZahlKond, Kunden.MahnCode As KDMahnCode, "
      strValue += "Kunden.MwStNr As KDMwStNr, Kunden.MwSt As KDMwSt, "
      strValue += "Kunden.KD_Telefax_Mailing, Kunden.KD_Mail_Mailing, "

      strValue += "KD_Zustaendig.RecNr As ZHDRecNr, "
      strValue += "KD_Zustaendig.Anrede, KD_Zustaendig.Nachname, "
      strValue += "KD_Zustaendig.Vorname, KD_Zustaendig.AnredeForm, "
      strValue += "KD_Zustaendig.Telefon As ZHDTelefon, "
      strValue += "KD_Zustaendig.Telefax As ZHDTelefax, "
      strValue += "KD_Zustaendig.Natel As ZHDNatel, "
      strValue += "KD_Zustaendig.eMail As ZHDeMail, "
      strValue += "KD_Zustaendig.ZHD_Telefax_Mailing, KD_Zustaendig.ZHD_SMS_Mailing, KD_Zustaendig.ZHD_Mail_Mailing, "

      strValue += "KD_Zustaendig.Postfach As ZHDPostfach, "
      strValue += "KD_Zustaendig.Strasse As ZHDStrasse, "
      strValue += "KD_Zustaendig.PLZ As ZHDPLZ, "
      strValue += "KD_Zustaendig.Ort As ZHDOrt, "
      strValue += "KD_Zustaendig.Abteilung As ZHDAbt, "
      strValue += "KD_Zustaendig.Position As ZHDPos, "
      strValue += "KD_Zustaendig.Interessen As ZHDInteressen, "
      strValue += "KD_Zustaendig.Bemerkung As ZHDBemerkung, "
      strValue += "KD_Zustaendig.Geb_Dat As ZHDGebdat, "
      strValue += "KD_Zustaendig.KDZHowKontakt, "
      strValue += "KD_Zustaendig.KDZState1, "
      strValue += "KD_Zustaendig.KDZState2, "
      strValue += "KD_Zustaendig.Berater As ZHDBerater "

      .WriteString(strValue)
      .WriteEndElement()

      .WriteEndElement()
      .WriteEndElement()
      .WriteEndElement()

      ' -----------------------------------------------------------------------------------------------------
      ' Liste der Liste nicht erfasste Rapporte -------------------------------------------------------------
      .WriteStartElement("SPRPListSearch")
      .WriteStartElement("frmRPListSearch")

      .WriteStartElement("SQLString")
      .WriteAttributeString("ID", "9cce2b1b-8fcc-4965-9839-420736d7149c")
      .WriteStartElement("SQL_0")
      strValue = "Select RP.RPNr, RP.Monat, RP.Jahr, RP.Von, RP.Bis From RP "

      .WriteString(strValue)
      .WriteEndElement()

      ' _1 Zeile...
      .WriteStartElement("SQL_1")
      strValue = "Select RP.RPNr, RP.MANr, RP.ESNr, RP.KDNr, RP.Monat, RP.Jahr, RP.Result, RP.PrintedWeeks, "
      strValue += "RP.Von, RP.Bis, MA.Nachname As MANachname, MA.Vorname As MAVorname, "
      strValue += "KD.Firma1, KD.Ort As KSOrt, ES.ES_Ab, ES.ES_Ende, ES.ES_Als, "
      strValue += "RPDayDb.Day1, RPDayDb.Day2, RPDayDb.Day3, RPDayDb.Day4, RPDayDb.Day5, "
      strValue += "RPDayDb.Day6, RPDayDb.Day7, RPDayDb.Day8, RPDayDb.Day9, RPDayDb.Day10, "
      strValue += "RPDayDb.Day11, RPDayDb.Day12, RPDayDb.Day13, RPDayDb.Day14, RPDayDb.Day15, "
      strValue += "RPDayDb.Day16, RPDayDb.Day17, RPDayDb.Day18, RPDayDb.Day19, RPDayDb.Day20,  "
      strValue += "RPDayDb.Day21, RPDayDb.Day22, RPDayDb.Day23, RPDayDb.Day24, RPDayDb.Day25, "
      strValue += "RPDayDb.Day26, RPDayDb.Day27, RPDayDb.Day28, RPDayDb.Day29, RPDayDb.Day30, "
      strValue += "RPDayDb.Day31, RPDayDb.WeekNr "
      strValue += "From RPDayDb Left Join RP On RP.RPNr = RPDayDb.RPNr	"
      strValue += "Left Join Mitarbeiter MA On RP.MANr = MA.MANr "
      strValue += "Left Join Kunden KD On RP.KDNr = KD.KDNr "
      strValue += "Left Join ES On RP.ESNr = ES.ESNr Where USNr = "

      .WriteString(strValue)
      .WriteEndElement()
      ' Ferting _1

      .WriteEndElement()
      .WriteEndElement()
      .WriteEndElement()

      ' -----------------------------------------------------------------------------------------------------
      ' Liste der Kinder- und Ausbildungszulage Jährlich ----------------------------------------------------
      .WriteStartElement("SPYFakListSearch")
      .WriteStartElement("frmYFakListSearch")

      .WriteStartElement("LANrString")
      .WriteAttributeString("ID", "335c767b-2873-4c64-b53f-e338c054f836")
      .WriteStartElement("LANr")
      strValue = "3600, 3602, 3650, 3700, 3750, 3800, 3850, 3900, 3901"

      .WriteString(strValue)
      .WriteEndElement()

      .WriteEndElement()
      .WriteEndElement()
      .WriteEndElement()


      ' -----------------------------------------------------------------------------------------------------
      ' Liste der Buchungsbelege monatlich ----------------------------------------------------
      .WriteStartElement("SPBUmsatzTotal")
      .WriteStartElement("frmUmsatz")

      .WriteStartElement("DiffSetting")
      .WriteAttributeString("ID", "09dbe069-23e5-40b4-93f1-a57b47b615bc")

      .WriteStartElement("MARS")
      strValue = "true"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Pooling")
      strValue = "true"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("DocJobNr_0")
      strValue = "3.5"
      .WriteString(strValue)

      .WriteEndElement()

      .WriteEndElement()
      .WriteEndElement()
      .WriteEndElement()

      ' -----------------------------------------------------------------------------------------------------
      ' Liste der Buchungsbelege monatlich ----------------------------------------------------
      .WriteStartElement("SPFibuSearch")
      .WriteStartElement("frmFibuSearch")

      .WriteStartElement("DiffSetting")
      .WriteAttributeString("ID", "FEF43DC1-E0B4-45fd-9BE2-BE11946863E5")

      .WriteStartElement("MARS")
      strValue = "true"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Pooling")
      strValue = "true"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("DocJobNr_0")
      strValue = "11.8"
      .WriteString(strValue)

      .WriteEndElement()

      .WriteEndElement()
      .WriteEndElement()
      .WriteEndElement()

      ' -----------------------------------------------------------------------------------------------------

      ' Lebenslauf ---------------------------------------------------------------------------------
      .WriteStartElement("MALLUtility")
      .WriteStartElement("frmLL")

      .WriteStartElement("SQLString")
      .WriteAttributeString("ID", "363028C8-74D4-4dbd-97A5-3A8BB207CC28")
      .WriteStartElement("SQL_LLFields2Print_Standard")
      strValue = "_Reserve0, _Reserve1, _Reserve2, _Reserve3, _Reserve4, _Reserve5, "
      strValue &= "_Reserve6, _Reserve7, _Reserve8, _Reserve9, _Reserve10, "
      strValue &= "_Reserve11, _Reserve12, _Reserve13, _Reserve14, _Reserve15, _ScannedDocs"

      .WriteString(strValue)
      .WriteEndElement()


      .WriteEndElement()
      .Close()

      If bShowMessasge Then
        MessageBox.Show("Die Datei " & strFileName & " wurde erfolgreich erstellt.", _
                        "SQL-String", MessageBoxButtons.OK, MessageBoxIcon.Information)
      End If

    End With

  End Sub

	Sub WriteMDDataToXML(ByVal strFileName As String, ByVal bShowMessasge As Boolean, ByVal mandantNumber As Integer)
		Dim enc As New System.Text.UnicodeEncoding
		If mandantNumber = 0 Then mandantNumber = _ClsProgSetting.GetMDNr
		Dim strStartElementName As String = String.Format("MD_{0}", mandantNumber)
		Dim strAttribute As String = String.Empty
		Dim strField_2 As String = "LV"
		Dim strField_3 As String = ""
		Dim m_PayrollSetting As String = String.Format("MD_{0}/Lohnbuchhaltung", mandantNumber)
		Return

		Dim xmlMandantProfile = m_md.GetSelectedMDDataXMLFilename(mandantNumber, Now.Year)



		Dim strMyMDDatFile As String = String.Format("{0}\Programm.dat", System.IO.Directory.GetParent(xmlMandantProfile).FullName)	 ' _ClsProgSetting.GetMDIniFile()
		Dim strSection As String = String.Empty
		Dim strQuery As String = String.Empty
		Dim strAbschnitt As String = ""


		strAttribute = "startnewbvgcalculationnow"
		Dim value As String = m_path.GetXMLNodeValue(xmlMandantProfile, String.Format("{0}/startnewbvgcalculationnow", m_PayrollSetting))
		Dim startNewBVGCalculationNow As Boolean?
		If Not String.IsNullOrWhiteSpace(value) Then
			startNewBVGCalculationNow = m_path.ParseToBoolean(value, Nothing)
		End If

		strAttribute = "donotcontrolbvgMinLohn"
		value = m_path.GetXMLNodeValue(xmlMandantProfile, String.Format("{0}/donotcontrolbvgMinLohn", m_PayrollSetting))
		Dim donotcontrolbvgMinLohn As Boolean?
		If Not String.IsNullOrWhiteSpace(value) Then
			donotcontrolbvgMinLohn = m_path.ParseToBoolean(value, Nothing)
		End If

		Dim strOldFilename As String = _ClsProgSetting.GetSpSFiles2DeletePath & "OldFile.xml"
		Try
			File.Copy(strFileName, strOldFilename, True)
		Catch ex As Exception

		End Try

		' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
		If bShowMessasge Then
			If File.Exists(strFileName) Then
				If MessageBox.Show("Die Datei existiert bereits. Soll die Datei neu angelegt werden?", "XML-Datei", _
										MessageBoxButtons.YesNo, _
										MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
			End If
		Else
			'If File.Exists(strFileName) Then Exit Sub
		End If

		Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFileName, enc)
		Dim strValue As String = String.Empty

		With XMLobj
			' Formatierung: 4er-Einzüge verwenden 
			.Formatting = Xml.Formatting.Indented
			.Indentation = 4

			.WriteStartDocument()
			.WriteStartElement(strStartElementName)


			strSection = "Sonstiges"
			.WriteStartElement(strSection)
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "BewName"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "BewPLZOrt"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "CreditInfo"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			' Bewilligungsbehörden --------------------------------------------------------------------------------------------
			strAttribute = "BewPostfach"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "BewStrasse"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "BewSeco"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "BURNumber"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "UIDNumber"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAR.-MitgliedNr"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "MDSelectedColor"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "Extension4Deletedrecs"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "KassaStartEndepflicht"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "KassaStartEndeAfter"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHV-Ausgleichskasse"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "Unfallversicherung"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "LL_IndentSize"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "20")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FontSize"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "11")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FontName"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "Calibri")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "LLTemplateExtension"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "DOC")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "MailFontName"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "Calibri")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "MailFontSize"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "11")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strQuery = "//Sonstiges/EnableLLDebug"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
			.WriteStartElement("EnableLLDebug")
			.WriteString(strValue)
			.WriteEndElement()

			strQuery = "//Sonstiges/mandantcolor"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
			.WriteStartElement("mandantcolor")
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "payrollluepaymentmethodifempty"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, "Zahlart", "")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			' Sonstiges fertig schreiben...
			.WriteEndElement()




			'-------------------------------------------------------------------------------------------------------------------



			strAbschnitt = "Lohnbuchhaltung"
			.WriteStartElement("Lohnbuchhaltung")
			strSection = "guthaben"
			.WriteStartElement("guthaben")

			strAttribute = "showguthabenpereaches"
			strQuery = String.Format("//{0}/guthaben/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			' Sonstiges fertig schreiben...
			.WriteEndElement()


			strSection = "report"
			.WriteStartElement("report")
			strAttribute = "tagesspesenstdab"
			strQuery = String.Format("//{0}/report/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "TagesSpesenStd", "8.25")
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			.WriteEndElement()



			If startNewBVGCalculationNow.HasValue Then

				strAttribute = "startnewbvgcalculationnow"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, startNewBVGCalculationNow)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

			End If

			If donotcontrolbvgMinLohn.HasValue Then
				strAttribute = "donotcontrolbvgMinLohn"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, donotcontrolbvgMinLohn)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

			End If

			strAttribute = "payrollcheckfee"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "CheckVorschuss", "0")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "advancepaymentcheckfee"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "CheckVorschuss", "0")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "advancepaymenttransferfee"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "ZGKontoGebühr", "0")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "advancepaymentcashfee"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "ZGGebührBar", "0")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "advancepaymenttransferinternationalfee"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "ZGGebührAusland", "0")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "payrolltransferinternationalfee"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "LOGebührAusland", "0")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "bvgafter"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "BVGAbzugAfter", "0")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "bvginterval"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "BVGIntervalString", "ww")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "bvgintervaladd"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "BVGIntervalAdd", "")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "calculatebvgwithesdays"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "BVG_ArbTage", "0")
			'End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "kizulagenotiflanrcontains"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "KIAuZulagenNotLANr", "")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "stringforadvancepaymentwithfee"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "CharForZGAbzug", "")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "deletepayrollwithbruttozero"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "DeleteLOWithBrutto0", "0")
			'End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "calculatetaxbasiswithnot180"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "QSTBasisNot180", "0")
			'End If
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "taxprocentforborderforeigner"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "QSTAnsatzByGrenz", "0")
			'End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			' Lohnbuchhaltung fertig schreiben...
			.WriteEndElement()





			strSection = "Export"	' ........................................................................................
			.WriteStartElement(strSection)
			'-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "MA_SPUser_ID"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "SaveVersandKontakt_0"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "0")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "SaveVersandKontakt_1"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "0")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "FaxRecipient_1"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "0")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "FaxRecipient_2"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "0")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			' Achtung: (Export-Abschnitt)----------------------------------------------------------------------------------
			strAttribute = "MA_SPUser_ID"
			strQuery = String.Format("//Export/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "KD_SPUser_ID"
			strQuery = String.Format("//Export/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Vak_SPUser_ID"
			strQuery = String.Format("//Export/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Ver_SPUser_ID"
			strQuery = String.Format("//Export/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			' Sonstiges fertig schreiben...
			.WriteEndElement()


			' Test ---------------------

			strAbschnitt = "Export_1"
			Dim strOrgAbschnitt As String = "Export"
			.WriteStartElement(strAbschnitt)

			strAttribute = "MA_SPUser_ID"
			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "KD_SPUser_ID"
			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Vak_SPUser_ID"
			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Ver_SPUser_ID"
			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			' Export_1 fertig schreiben...
			.WriteEndElement()


			strAbschnitt = "Template_1"
			strOrgAbschnitt = "Template"
			.WriteStartElement(strAbschnitt)

			strAttribute = "cockpit-url"
			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "cockpit-picture"
			strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			' Template_1 fertig schreiben...
			.WriteEndElement()


			' Abschluss der Test -------------------------------











			strSection = "Templates" ' .....................................................................................
			.WriteStartElement(strSection)
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "cockpit-picture"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "cockpit-url"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "ZeugnisDeckblatt"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AGBDeckblatt"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AGB4Temp"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "AGBTemp.PDF")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AGB4Fest"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "AGBFest.PDF")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "PMail_tplDocNr"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "18.0.1")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "customer-contact-subject"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "customer-mail-contact-subject"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "customer-fax-contact-subject"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "customer-contact-body"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "customer-sms-contact-subject"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "customer-sms-contact-body"
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "CreatedLLFileAs"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "Dossier {0} {1}")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "eMailImageVar1"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "eMailImageValue1"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "eMailImageVar2"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "eMailImageValue2"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "eMailImageVar3"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "eMailImageValue3"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			' Achtung: (Templates-Abschnitt)----------------------------------------------------------------------------------
			strAttribute = "Zwischenverdienstformular_Doc-eMail-Template"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_ZVDoc.txt")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "Arbeitgeberbescheinigung_Doc-eMail-Template"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_ArbgDoc.txt")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "MADoc-eMail-Template"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_MADoc.txt")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "KDDoc-eMail-Template"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_KDDoc.txt")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "ZHDDoc-eMail-Template"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_ZHDDoc.txt")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "Cockpit-eMail-Template"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_CockpitWOS.txt")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			' Achtung: (Templates-Abschnitt)----------------------------------------------------------------------------------
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "eMail-Template-JobNr"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "eMail-Template"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "MassenOffer-eMail-Template-JobNr"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "MassenOffer-eMail-Template"
			strQuery = String.Format("//Templates/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			' Sonstiges fertig schreiben...
			.WriteEndElement()




			strAbschnitt = "Mailing"
			strSection = "Mailing"
			.WriteStartElement(strAbschnitt)

			strAttribute = "faxusername"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "faxuserpw"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "ecalljobid"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "ecallfromtext"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "ecallheaderid"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "ecallheaderinfo"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "ecallsubject"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "ecallnotification"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "ecalltoken"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "faxextension"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "faxforwarder"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "davidserver"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "cockpitwww"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "SMTP-Server"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "SMTP-Port"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()



			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "Zwischenverdienstformular_Doc-eMail-Betreff"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "Arbeitgeberbescheinigung_Doc-eMail-Betreff"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "MADoc-eMail-Betreff"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "MADoc-WWW"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "KDDoc-eMail-Betreff"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "KDDoc-WWW"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "ZHDDoc-eMail-Betreff"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "ZHDDoc-WWW"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "Mail-Database"
			strQuery = String.Format("//Mailing/{0}", strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			' Sonstiges fertig schreiben...
			.WriteEndElement()





			strSection = "SUVA-Daten"	' ....................................................................................
			.WriteStartElement(strSection)
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "SUVAAddressZusatz"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "SUVAAddressZHD"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "SUVAAddressPostfach"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "SUVAAddressStrasse"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "SUVAAddressPLZOrt"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "Abrechnungsnummer"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "SUVAAddressSub1"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "SUVAAddressSub2"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "SUVAAddressSub3"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "SUVAAddressSub4"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "SUVAAddressSub5"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "SUVAAddressSub6"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			.WriteEndElement()


			.WriteStartElement("AHV-Daten")

			' AHV-Adressen -----------------------------------------------------------------------------------------------------
			strAttribute = "AHVAddressZusatz"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHVAddressZHD"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHVAddressPostfach"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHVAddressStrasse"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHVAddressPLZOrt"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHVMitgliedNr"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AusgNummer"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHVSub1"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHVSub2"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHVSub3"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHVSub4"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHVSub5"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "AHVSub6"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			.WriteEndElement()


			.WriteStartElement("Fak-Daten")

			' FAK-Kasse -------------------------------------------------------------------------------------------------------
			strAttribute = "FAKKassenname"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressZusatz"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressZHD"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressPostfach"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressStrasse"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressPLZOrt"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressMitgliednr"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressNr"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressSub1"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressSub2"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressSub3"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressSub4"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressSub5"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "FAKAddressSub6"
			strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			.WriteEndElement()







			'      .WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			'-------------------------------------------------------------------------------------------------------------------
			.WriteStartElement("BuchungsKonten")

			For i As Integer = 0 To 37
				strAttribute = Str(i + 1).Trim
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "BuchungsKonten", strAttribute, "")
				.WriteStartElement("_" & strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
			Next

			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			'-------------------------------------------------------------------------------------------------------------------
			strAbschnitt = "StartNr"
			.WriteStartElement(strAbschnitt)

			strAttribute = "Mitarbeiter"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			End If
			If strValue = String.Empty Then
				strValue = "0"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Kunden"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then
				strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			End If
			If strValue = String.Empty Then
				strValue = "0"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Vakanzennr"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Offers"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Einsatzverwaltung"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Rapporte"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Vorschussverwaltung"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Fakturen"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Zahlungseingänge"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Mahnungen"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Lohnabrechnung"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "FremdOP"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "Kassenbuch"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "FirstKassenBetrag"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
			If strValue = String.Empty Then strValue = "0"
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------


			'-------------------------------------------------------------------------------------------------------------------
			'-------------------------------------------------------------------------------------------------------------------
			strAbschnitt = "Debitoren"
			.WriteStartElement(strAbschnitt)

			strAttribute = "mwstnr"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Or Not strValue.StartsWith("CHE-") Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "mwstsatz"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Or Val(strValue) < 8 Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "mwst-satz", "8")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "ref10forfactoring"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "RefNrTo10", "0")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "ezonsepratedpage"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "Trenne die Einzahlungsscheine", "0")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "setfakdatetoendofreportmonth"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "FakDateAsRPEndDate", "0")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "create3mahnasuntilnotpaid"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "Create3MahnByNotPaid", "0")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "printezwithmahn"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "MahnWithEZ", "0")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "printguonmahnung"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "MahnWithGutschrift", "0")
			If strValue = "1" Or strValue = "true" Then
				strValue = "true"
			Else
				strValue = "false"
			End If
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()



			'strAttribute = "esrart"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			'strAttribute = "esr-id"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			'strAttribute = "esrkonto"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			'strAttribute = "esrfile"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			'strAttribute = "rounddiff"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			strAttribute = "mahnspesenab"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "Mahnspesen ab", "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "mahnspesenchf"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "mahnspesen", "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "verzugszinsdaysafter"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "verzugszinson", "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "verzugszinspercent"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "verzugszinsen", "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "verzugszinsabchf"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "Verzugszins Ab", "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()



			'strAttribute = "anzahljtage"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			'strAttribute = "anzahlmtage"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			'strAttribute = "refnrto10"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			'strAttribute = "kontonr"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			'   strAttribute = "dtafilename"
			'   strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'   strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'   If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'   .WriteStartElement(strAttribute)
			'   .WriteString(strValue)
			'   .WriteEndElement()

			'strAttribute = "sepesrtoprint"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "Trenne die Einzahlungsscheine", "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			'strAttribute = "create3mahnbynotpaid"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			'strAttribute = "fakdateasrpenddate"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			strAttribute = "factoringcustomernumber"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "factoskdnr", "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'   strAttribute = "opfilename"
			'   strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'   strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "opfilename", "")
			'   .WriteStartElement(strAttribute)
			'   .WriteString(strValue)
			'   .WriteEndElement()

			strAttribute = "invoicezipfilename"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", "opzipfilename", "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'strAttribute = "opzipfilelen"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			'If strValue = String.Empty Then strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			'-------------------------------------------------------------------------------------------------------------------





			'strAbschnitt = "Debitoren"
			'.WriteStartElement(strAbschnitt)

			'strAttribute = "ESEndByNullValue"
			'strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "unbestimmt")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()

			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "ES_KD_UmsMin"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "MWSTNr"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "MWST-Satz"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "ESRArt"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(strAttribute)
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "ESR-ID"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "ESRKonto"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "ESRFile"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "RoundDiff"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "VerzugszinsOn"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "Mahnspesen ab"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "Mahnspesen"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "VerzugsZinsen"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "Verzugszins Ab"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "AnzahlJTage"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "AnzahlMTage"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "RefNrTo10"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "KontoNr"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "DTAFileName"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "Trenne die Einzahlungsscheine"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "Create3MahnByNotPaid"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "FakDateAsRPEndDate"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "FACTOSKDNr"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "OPFileName"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "OPZipFileName"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()
			''-------------------------------------------------------------------------------------------------------------------
			'strAttribute = "OPZipFileLen"
			'strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
			'.WriteStartElement(Replace(strAttribute, " ", "_"))
			'.WriteString(strValue)
			'.WriteEndElement()

			'.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			'-------------------------------------------------------------------------------------------------------------------


			strAbschnitt = "Licencing"
			.WriteStartElement(strAbschnitt)

			strAttribute = "sesam"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "abacus"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "swifac"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "comatic"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "kmufactoring"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "csoplist"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "parifond"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "customermng_deltavistaSolvencyCheckReferenceNumber"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "customermng_deltavistaWebServiceUserName"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "customermng_deltavistaWebServicePassword"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			'-------------------------------------------------------------------------------------------------------------------
			strAttribute = "customermng_deltavistaWebServiceUrl"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			.WriteEndElement()
			'-------------------------------------------------------------------------------------------------------------------

			strAbschnitt = "Interfaces"
			strSection = "Interfaces"
			.WriteStartElement(strAbschnitt)

			strAttribute = "abalofieldtrennzeichen"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abalodarstellungszeichen"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abalorefnr"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abalogegenkonto"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abalomwstcode"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			strAttribute = "abaopfieldtrennzeichen"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abaopdarstellungszeichen"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abaoprefnr"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abaopgegenkonto"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abaopmwstcode"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			strAttribute = "abazefieldtrennzeichen"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abazedarstellungszeichen"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abazerefnr"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abazegegenkonto"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "abazemwstcode"
			strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			.WriteStartElement("webservices")

			strAttribute = "webservicejobdatabase"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webservicebankdatabase"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wssps_services/spbankutil.asmx")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			' TODO: Muss nachher gelöscht werden!!!
			strAttribute = "webserviceqstdatabase"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webservicegavdatabase"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webservicegavutility"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webservicewosemployeedatabase"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webservicewoscustomer"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webservicewosvacancies"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webservicejobchvacancies"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webserviceostjobvacancies"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webservicesuedostvacancies"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webserviceecall"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webservicepaymentservices"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wssps_services/spcustomerpaymentservices.asmx")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webservicetaxinfoservices"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wsSPS_services/SPEmployeeTaxInfoService.asmx")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webservicealkdatabase"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wsSPS_services/SPALKUtil.asmx")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()

			strAttribute = "webserviceupdateinfoservices"
			strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
			strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wssps_services/SPUpdateUtilities.asmx")
			.WriteStartElement(strAttribute)
			.WriteString(strValue)
			.WriteEndElement()


			.WriteEndElement()


			'-------------------------------------------------------------------------------------------------------------------


			'-------------------------------------------------------------------------------------------------------------------


			'-------------------------------------------------------------------------------------------------------------------
			' Write the XML to file and close the writer.
			.WriteEndElement()
			.Flush()
			.Close()

			If bShowMessasge Then
				MessageBox.Show("Die Datei " & strFileName & " wurde erfolgreich erstellt.", _
												"SQL-String", MessageBoxButtons.OK, MessageBoxIcon.Information)
			End If

		End With

	End Sub

  Public Overloads Sub INIRead(ByVal INIPath As String, ByVal SectionName As String, ByVal KeyName As String(), ByRef KeyValue As String())
    ' primary version of call gets single value given all parameters         
    Dim n As Int32
    Dim sData As String

    sData = Space$(1024) ' allocate some room         
    For i As Integer = 1 To KeyName.Length - 1
      If KeyName(i) <> "" Then
        n = GetPrivateProfileString(SectionName, KeyName(i), KeyValue(i), sData, sData.Length, LTrim(RTrim((INIPath))))
        If n > 0 Then ' return whatever it gave us                     
          KeyValue(i) = sData.Substring(0, n)
        Else
          KeyValue(i) = ""
        End If
      End If
    Next

  End Sub



  Sub WriteMSGDataToXML(ByVal bShowMessasge As Boolean)
    Dim strFilename = _ClsProgSetting.GetMSGData_XMLFile()
    Dim enc As New System.Text.UnicodeEncoding
    Dim strStartElementName As String = "Messages"
    Dim strAttribute As String = String.Empty
    Dim strField_2 As String = "LV"
    Dim strField_3 As String = ""
    Dim strMyMDDatFile As String = _ClsProgSetting.GetMDIniFile()

    ' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
    If bShowMessasge Then
      If File.Exists(strFilename) Then
        If MessageBox.Show("Die Datei existiert bereits. Soll die Datei neu angelegt werden?", "XML-Datei", _
                    MessageBoxButtons.YesNo, _
                    MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
      End If
    Else
      'If File.Exists(strFilename) Then Exit Sub
    End If

    Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFilename, enc)
    Dim strValue As String = String.Empty

    With XMLobj
      ' Formatierung: 4er-Einzüge verwenden 
      .Formatting = Xml.Formatting.Indented
      .Indentation = 4

      .WriteStartDocument()
      .WriteStartElement(strStartElementName)
      .WriteStartElement("MSGID")

      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1000"
      strValue = "Bitte warten Sie einen Moment..."
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1001"
      strValue = "Nach Daten wird gesucht..."
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1002"
      strValue = "Datenauflistung für {0} Einträge: in {1} ms"
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1003"
      strValue = "{0} Datensätze wurden aufgelistet..."
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1004"
      strValue = "Keine Suche wurde gestartet!"
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1005"
      strValue = "Ich konnte leider Keine Daten finden."
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1006"
      strValue = "Folgende Datei wurde nicht gefunden. Bitte wählen Sie das Programm aus."
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1007"
      strValue = "Programm wurde nicht gefunden"
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1008"
      strValue = "Dieser Vorgang kann mehrer Minuten dauern. Sind Sie sicher?"
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1009"
      strValue = "Daten exportieren"
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1010"
      strValue = "Ihre Daten wurden erfolgreich exportiert."
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1011"
      strValue = "Datenexport in XML"
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1012"
      strValue = "Vorgang wurde abgebrochen!"
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MSGID1013"
      strValue = "Bereit"
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()






      ' Die Label-Bezeichnungen für sonstige Ausgaben... ----------------------------------------------------------------------
      Dim strMyString As String = "Falsche Format. Z. B. 4.0"

      .WriteEndElement()

      strField_2 = "MyLabel"
      strAttribute = "Label"
      .WriteStartElement("DivString")

      .WriteComment("Label.Text für sonstige Ausgaben")
      .WriteStartElement("MyString")
      .WriteAttributeString(strAttribute, strMyString)
      .WriteStartElement(strField_2) ' <KD 
      .WriteString(strMyString)
      .WriteEndElement()
      .WriteEndElement()

      strMyString = "Leeres Feld ist nicht erlaubt."
      .WriteStartElement("MyString")
      .WriteAttributeString(strAttribute, strMyString)
      .WriteStartElement(strField_2) ' <KD 
      .WriteString(strMyString)
      .WriteEndElement()
      .WriteEndElement()







      '-------------------------------------------------------------------------------------------------------------------
      ' Write the XML to file and close the writer.
      .WriteEndElement()
      .Flush()
      .Close()

      If bShowMessasge Then
        MessageBox.Show("Die Datei " & strFilename & " wurde erfolgreich erstellt.", _
                        "WriteMSGDataToXML", MessageBoxButtons.OK, MessageBoxIcon.Information)
      End If

    End With

  End Sub




  Public Function GetAusgleichsNr() As String
    Dim _ClsReg As New SPProgUtility.ClsDivReg
    Dim strUserProfileName As String = _ClsProgSetting.GetMDData_XMLFile()
    Dim strQuery As String = "//MD_" & _ClsProgSetting.GetMDNr() & "/Sonstiges/FAR.-MitgliedNr"
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)

    Return strBez
  End Function



#Region "Sonstige Funktionen..."

  Private Sub XMLReader()

    ' Wir benötigen einen XmlReader für das Auslesen der XML-Datei 
    Dim XMLReader As Xml.XmlReader _
      = New Xml.XmlTextReader("quickie.xml")

    ' Es folgt das Auslesen der XML-Datei 
    With XMLReader

      Do While .Read ' Es sind noch Daten vorhanden 

        ' Welche Art von Daten liegt an? 
        Select Case .NodeType

          ' Ein Element 
          Case Xml.XmlNodeType.Element

            Console.WriteLine("Es folgt ein Element vom Typ " & .Name)

            ' Alle Attribute (Name-Wert-Paare) abarbeiten 
            If .AttributeCount > 0 Then
              ' Es sind noch weitere Attribute vorhanden 
              While .MoveToNextAttribute ' nächstes 
                Console.WriteLine("Feldname: " & .Name & _
                " -> " & _
                "Feldwert: " & .Value)
              End While

            End If

            ' Ein Text 
          Case Xml.XmlNodeType.Text
            Console.WriteLine("Es folgt ein Text: " & .Value)

            ' Ein Kommentar 
          Case Xml.XmlNodeType.Comment
            Console.WriteLine("Es folgt ein Kommentar: " & .Value)

        End Select

      Loop  ' Weiter nach Daten schauen 

      .Close()  ' XMLTextReader schließen 

    End With

    ' Und so sieht das Ergebnis der Ausgabe aus: 
    ' ------------------------------------------ 
    'Es folgt ein Element vom Typ Personen 
    'Es folgt ein Element vom Typ Person 
    'Feldname: Titel -> Feldwert: Dr. 
    'Feldname: Name -> Feldwert: Meyer 
    'Feldname: Vorname -> Feldwert: Hans 
    'Es folgt ein Element vom Typ Person 
    'Feldname: Titel -> Feldwert: 
    'Feldname: Name -> Feldwert: Schmidt 
    'Feldname: Vorname -> Feldwert: Carlos 

  End Sub

	Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode, _
														ByVal strTag As String, ByVal strText As String) As XmlElement
		Dim xNodeTemp As XmlNode

		xNodeTemp = xDoc.CreateElement(strTag)
		xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
		xNode.AppendChild(xNodeTemp)

		Return CType(xNodeTemp, XmlElement)
	End Function

	Function AddOrUpdateFieldLabelNode(ByVal xDoc As XmlDocument,
														 ByVal strGuid As String, ByVal strMainKey As String,
														 ByVal KeyValue As String) As Boolean
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing
		Dim strKeyName As String = String.Empty
		strKeyName = "CtlLabel"

		xNode = xDoc.SelectSingleNode(String.Format(strMainKey, strGuid))
		If xNode Is Nothing Then
			Dim newNode As Xml.XmlElement = xDoc.CreateElement("Control")

			newNode.SetAttribute("Name", strGuid)
			xDoc.DocumentElement.AppendChild(newNode)
			xNode = xDoc.SelectSingleNode(String.Format(strMainKey, strGuid))
		End If

		If xNode IsNot Nothing Then
			If TypeOf xNode Is XmlElement Then
				xElmntFamily = CType(xNode, XmlElement)
			End If
		End If

		If TypeOf xNode Is XmlElement Then xElmntFamily = CType(xNode, XmlElement)
		If xElmntFamily.SelectSingleNode(strKeyName) IsNot Nothing Then xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strKeyName))
		'InsertTextNode(xDoc, xElmntFamily, strKeyName, (frm.txt_Telefonprivat.Text))

		With xElmntFamily
			.SetAttribute("Name", String.Format("{0}", strGuid))
			.AppendChild(xDoc.CreateElement("CtlLabel")).InnerText = KeyValue
		End With


		Return True
	End Function


#End Region



	Public Sub New()

		m_md = New SPProgUtility.Mandanten.Mandant
		m_path = New SPProgUtility.ProgPath.ClsProgPath

	End Sub
End Class


Public Class ClsXML_1

End Class
