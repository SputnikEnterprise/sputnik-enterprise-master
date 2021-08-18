
Imports System.Data.SqlClient

Imports System.IO
Imports System.Xml

Imports System.Xml.XmlTextWriter
Imports System.Xml.XmlTextReader
Imports System.Windows.Forms

Public Class ClsXMLFunc

  Dim _ClsReg As New ClsDivReg
  Dim _ClsSystem As New ClsMain_Net
  Dim _ClsSettingPath As New ClsProgSettingPath

  Dim strConnString As String = _ClsSettingPath.GetConnString()
  Dim Conn As New SqlConnection(strConnString)

  Sub WriteDataToXML(ByVal strFileName As String)

    ' Auswahl einer Kodierungsart für die Zeichenablage 
    Dim enc As New System.Text.UnicodeEncoding
    Dim strStartElementName As String = "KDFrame"

    ' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
    Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFileName, enc)
    Dim strValue As String = String.Empty

    With XMLobj

      ' Formatierung: 4er-Einzüge verwenden 
      .Formatting = Xml.Formatting.Indented
      .Indentation = 4

      ' Dann fangen wir mal an: 
      .WriteStartDocument()

      ' Beginn eines Elements "Personen". Darin werden wir mehrere 
      ' Elemente "Person" unterbringen. 
      .WriteStartElement(strStartElementName)

			.WriteStartElement(strStartElementName) ' <KD 
			.WriteAttributeString("Name", strStartElementName)

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "BeraterIn")
      XMLobj.WriteAttributeString("CtlLable", "BeraterIn")
      XMLobj.WriteEndElement() ' KD /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "Lbl_KD_1Property")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "Lbl_KD_1Property", "1. Eigenschaft"))
      XMLobj.WriteEndElement() ' KD /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "Lbl_KD_2Property")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "Lbl_KD_2Property", "2. Eigenschaft"))
      XMLobj.WriteEndElement() ' KD /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "KD1Res")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "KD1Res", "1. Res"))
      XMLobj.WriteEndElement() ' KD /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "KD2Res")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "KD2Res", "2. Res"))
      XMLobj.WriteEndElement() ' KD /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "KD3Res")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "KD3Res", "3. Res"))
      XMLobj.WriteEndElement() ' KD /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "KD4Res")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "KD4Res", "4. Res"))
      XMLobj.WriteEndElement() ' KD /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "KD1Status")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "KD1Status", "1. Status"))
      XMLobj.WriteEndElement() ' KD /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "KD2Status")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "KD2Status", "2. Status"))
      XMLobj.WriteEndElement() ' KD /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "KDKontakt")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "KDKontakt", "Kontakt"))
      XMLobj.WriteEndElement() ' KD /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "KDResLbl")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "KDResLbl", "Reserve Felder"))
      XMLobj.WriteEndElement() ' KD /> 


      ' Zuständige Person
      strStartElementName = "KDZhdFrame"
      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "Lbl_ZHD_Kontakt")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "Lbl_ZHD_Kontakt", "Kontakt"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "Lbl_ZHD_1State")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "Lbl_ZHD_1State", "Kontakt"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "Lbl_ZHD_2State")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "Lbl_ZHD_2State", "Kontakt"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "Lbl_ZHD_Kommunikation")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), "FieldsLable", "Lbl_ZHD_Kommunikation", "Kontakt"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "Lbl_ZHD_Versand")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "Lbl_ZHD_Versand", "Kontakt"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "Lbl_ZHD_Res1")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "Lbl_ZHD_Res1", "Kontakt"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "Lbl_ZHD_Res2")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "Lbl_ZHD_Res2", "Kontakt"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "Lbl_ZHD_Res3")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "Lbl_ZHD_Res3", "Kontakt"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "Lbl_ZHD_Res4")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "FieldsLable", "Lbl_ZHD_Res4", "Kontakt"))
      XMLobj.WriteEndElement() ' KDZhd /> 


      ' Einsatzverwaltung
      strStartElementName = "ES"
      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "BezESTSpesen")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "Sonstiges", "BezESTSpesen", "Tagesspesen"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "BezESZuSatz1")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "Sonstiges", "BezESZuSatz1", "1. Zustatz"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "BezESZuSatz2")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "Sonstiges", "BezESZuSatz2", "2. Zustatz"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "BezESZuSatz3")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "Sonstiges", "BezESZuSatz3", "3. Zustatz"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "BezESZuSatz4")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "Sonstiges", "BezESZuSatz4", "3. Zustatz"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "BezESZuSatz5")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "Sonstiges", "BezESZuSatz5", "3. Zustatz"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "BezESZuSatz6")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "Sonstiges", "BezESZuSatz6", "3. Zustatz"))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "BezKST1")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "Sonstiges", "BezKST1", "1. Kst."))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "BezKST2")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "Sonstiges", "BezKST2", "2. Kst."))
      XMLobj.WriteEndElement() ' KDZhd /> 

      XMLobj.WriteAttributeString("Name", strStartElementName)
      XMLobj.WriteAttributeString("CtlName", "BezKST3")
      XMLobj.WriteAttributeString("CtlLable", _ClsReg.GetINIString(_ClsSettingPath.GetMDIniFile(), _
                                                                   "Sonstiges", "BezKST3", "3. Kst."))
      XMLobj.WriteEndElement() ' KDZhd /> 





			.WriteEndElement() ' </Personen> 

			' ... und schließen das XML-Dokument (und die Datei) 
			.Close() ' Document 

    End With


	End Sub

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


	End Sub




#Region "funktionen für die Übertragung der Daten vom Ini in die XML..."


  Sub WriteFormDataToXML(ByVal strFileName As String, ByVal bShowMessasge As Boolean)
    Dim enc As New System.Text.UnicodeEncoding ' .UTF8Encoding
    Dim strStartElementName As String = "Forms_Normaly"
    Dim strStartElementUName_1 As String = "KD_Fields"
    Dim strField_1 As String = "CtlName"
    Dim strAttribute As String = "Name"
    Dim strField_2 As String = "CtlLabel"
    Dim strField_3 As String = "CtlAlignment"

    Dim strMyMDDatFile As String = _ClsSettingPath.GetMDIniFile()

		If bShowMessasge Then
			If File.Exists(strFileName) Then
				If MessageBox.Show("Die Datei existiert bereits. Soll die Datei neu angelegt werden?", "XML-Datei",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
			End If
		End If

		Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFileName, enc)
    Dim strValue As String = String.Empty

    With XMLobj

      ' Formatierung: 4er-Einzüge verwenden 
      .Formatting = Xml.Formatting.Indented
      .Indentation = 4

      ' Dann fangen wir mal an: 
      .WriteStartDocument()

      ' Beginn eines Elements "Personen". Darin werden wir mehrere 
      ' Elemente "Person" unterbringen. 
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
      .WriteStartElement("TXT_Control")
      .WriteAttributeString(strAttribute, "50b03d02-746e-4ce9-b583-f7107a19f356")

      .WriteStartElement("TXT_1")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Mahncode", "Mahncode")
      If strValue <> String.Empty Then strValue = "A"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("TXT_2")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Fakturacode", "Fakturacode")
      If strValue <> String.Empty Then strValue = "R"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("TXT_3")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "NOKDES", "NOKDES")
      If strValue <> String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("TXT_4")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MWSTPflicht", "MWSTPflicht")
      If strValue <> String.Empty Then strValue = "1"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteEndElement()


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
      .WriteComment("Feld automatisch ausfüllen: Einsatzverwaltung")
      .WriteStartElement("TXT_Control")
      .WriteAttributeString(strAttribute, "a7baaf67-b259-4bb5-a74e-60ae9ee292e1")

      .WriteStartElement("TXT_1")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "SuvaCode", "SuvaCode")
      If strValue = String.Empty Then strValue = "A1"
      .WriteString(strValue)
      .WriteEndElement()

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "QualifikationsFeld", "QualifikationsFeld")
      If strValue <> String.Empty Then
        .WriteStartElement("TXT_2")
        .WriteString(strValue)
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "GetIndKstInES", "GetIndKstInES")
      If strValue = String.Empty Then strValue = "0"
      .WriteStartElement("TXT_3")
      .WriteString(strValue)
      .WriteEndElement()

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "GetLastFeiertagProz", "GetLastFeiertagProz")
      If strValue = String.Empty Then strValue = "0"
      .WriteStartElement("TXT_4")
      .WriteString(strValue)
      .WriteEndElement()

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "SignFromUnterzeichner", "SignFromUnterzeichner")
      If strValue = String.Empty Then strValue = "0"
      .WriteStartElement("TXT_5")
      .WriteString(strValue)
      .WriteEndElement()

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "AllowedChangeGAVKanton", "AllowedChangeGAVKanton")
      If strValue = String.Empty Then strValue = "0"
      .WriteStartElement("TXT_6")
      .WriteString(strValue)
      .WriteEndElement()

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "AllowedGAVKontroll", "AllowedGAVKontroll")
      If strValue = String.Empty Then strValue = "0"
      .WriteStartElement("TXT_7")
      .WriteString(strValue)
      .WriteEndElement()



      .WriteEndElement()


      ' -----------------------------------------------------------
      .WriteComment("Feld automatisch ausfüllen: Allgemein")
      .WriteStartElement("TXT_Control")
      .WriteAttributeString(strAttribute, "9086e928-694a-409f-8acf-e0388fd0f6bb")

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "SprachFeld", "SprachFeld")
      If strValue = String.Empty Then strValue = "deutsch"
      .WriteStartElement("TXT_1")
      .WriteString(strValue)
      .WriteEndElement()

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ExternDb_Country", "ExternDb_Country")
      If strValue = String.Empty Then strValue = "0"
      .WriteStartElement("TXT_2")
      .WriteString(strValue)
      .WriteEndElement()

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ExternDb_Job", "ExternDb_Job")
      If strValue = String.Empty Then strValue = "0"
      .WriteStartElement("TXT_3")
      .WriteString(strValue)
      .WriteEndElement()

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ExternDb_Bank", "ExternDb_Bank")
      If strValue = String.Empty Then strValue = "0"
      .WriteStartElement("TXT_4")
      .WriteString(strValue)
      .WriteEndElement()

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ExterDb_Qst", "ExterDb_Qst")
      If strValue = String.Empty Then strValue = "0"
      .WriteStartElement("TXT_5")
      .WriteString(strValue)
      .WriteEndElement()


      .WriteEndElement()


      ' -----------------------------------------------------------
      .WriteComment("Feld automatisch ausfüllen: Kassenbuch")
      .WriteStartElement("TXT_Control")
      .WriteAttributeString(strAttribute, "a542a44b-2fb7-4b79-9305-4fa04c9d8a64")

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KassaStartEndepflicht", "KassaStartEndepflicht")
      If strValue = String.Empty Then strValue = "0"
      .WriteStartElement("TXT_1")
      .WriteString(strValue)
      .WriteEndElement()

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KassaStartEndeAfter", "KassaStartEndeAfter")
      If strValue <> String.Empty Then
        .WriteStartElement("TXT_2")
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
      .WriteComment("Feld automatisch anfügen: Lohnabrechnung drucken")
      .WriteStartElement("TXT_Control")
      .WriteAttributeString(strAttribute, "83833a67-5477-4c16-b099-19698d1e8dd6")

      .WriteStartElement("TXT_1")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "PrintFeiertagInLO", "PrintFeiertagInLO")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("TXT_2")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "PrintFerienInLO", "PrintFerienInLO")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("TXT_3")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "Print13LohnInLO", "Print13LohnInLO")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("TXT_4")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "PrintDarlehenInLO", "PrintDarlehenInLO")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("TXT_5")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "PrintGleitStdInLO", "PrintGleitStdInLO")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("TXT_6")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "PrintNightStdInLO", "PrintNightStdInLO")
      If strValue = String.Empty Then strValue = "0"
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
      .WriteComment("Pflichtfelder: Module")
      .WriteStartElement("Pflicht_Control")
      .WriteAttributeString(strAttribute, "f6c163f6-3dab-4db8-b8dd-a7cd19b7017c")

      .WriteStartElement("Qualifikation")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MABerufPflichtig", "MABerufPflichtig")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("MABeraterIn")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MABetreuerPflichtig", "MABetreuerPflichtig")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Bewilligung")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MABewPflichtig", "MABewPflichtig")
      If strValue = String.Empty Then strValue = "1"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Gültigbis")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MABewBisPflichtig", "MABewBisPflichtig")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Kirchensteuer")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KirchenQSTpflichtig", "KirchenQSTpflichtig")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Kanton")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KQST", "KQST")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Kanton")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KQST", "KQST")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()


      .WriteEndElement()



      ' Einsatzpflichtfelder
      .WriteStartElement("Pflicht_Control")
      .WriteAttributeString(strAttribute, "Einsatz")

      .WriteStartElement("_1_Kst")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ESKST1Pflichtig", "ESKST1Pflichtig")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("_2_Kst")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ESKST2Pflichtig", "ESKST2Pflichtig")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("_3_Kst")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ESKST3Pflichtig", "ESKST3Pflichtig")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Uhrzeit")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ESUhr", "ESUhr")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("GAV-Vertrag")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ESGAVPflichtig", "ESGAVPflichtig")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Einstufung")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "EinsatzeinstufungPflichtig", "EinsatzeinstufungPflichtig")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Branche")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "EsBranchePflichtig", "EsBranchePflichtig")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()


      .WriteEndElement()

      ' Kassenbuch Pflichtfelder
      .WriteStartElement("Pflicht_Control")
      .WriteAttributeString(strAttribute, "a542a44b-2fb7-4b79-9305-4fa04c9d8a64")

      .WriteStartElement("Belegnummer")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "CashRegBelegNummerPflicht", "CashRegBelegNummerPflicht")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Filiale")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "CashRegFilialPflicht", "CashRegFilialPflicht")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

      .WriteStartElement("Benutzer")
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "CashRegDispoPflicht", "CashRegDispoPflicht")
      If strValue = String.Empty Then strValue = "0"
      .WriteString(strValue)
      .WriteEndElement()

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

      strValue = CStr(CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\Colour", _
                                                 "LostFocusNormalColor"))))
      If strValue = String.Empty Then strValue = "16777215"
      .WriteStartElement("NormalFields")
      .WriteString(strValue)
      .WriteEndElement()

      ' Pflichtfelder
      strValue = CStr(CInt(Val(_ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Forms\Colour", _
                                                 "LostFocusPflichtColor"))))
      If strValue = String.Empty Then strValue = "12779261"
      .WriteStartElement("MustFields")
      .WriteString(strValue)
      .WriteEndElement()

      .WriteEndElement()


      ' Die Label-Bezeichnungen auf den Formen... ----------------------------------------------------------------------
      ' ----------------------------------------------------------------------

      strAttribute = "Name"

      .WriteComment("Label.Text for Forms")
      .WriteStartElement("Control") ' <KD 
      .WriteAttributeString(strAttribute, "BeraterIn")
      .WriteStartElement(strField_2) ' <KD 
      .WriteString("BeraterIn")
      .WriteEndElement()
      .WriteEndElement()

      ' 1. Eigenschaft
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_KD_1Property")
      If strValue = String.Empty Then strValue = "1. Eigenschaft"
      If strValue <> String.Empty Then
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, "KD_1Property")

        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      ' 2. Eigenschaft
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_KD_2Property")
      If strValue = String.Empty Then strValue = "2. Eigenschaft"
      If strValue <> String.Empty Then
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, "KD_2Property")
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD1Res", "1. Res")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "KD1Res"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD2Res", "2. Res")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "KD2Res"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD3Res", "3. Res")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "KD3Res"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD4Res", "4. Res")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "KD4Res"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD1Status", "1. Status")
      If strValue = String.Empty Then strValue = "1. Status"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "KD1Status"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KD2Status", "2. Status")
      If strValue = String.Empty Then strValue = "2. Status"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "KD2Status"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KDKontakt", "Kontakt")
      If strValue = String.Empty Then strValue = "Kontakt"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "KDKontakt"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "KDResLbl", "Reserve Felder")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "KDResLbl"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If


      ' Zuständige Person
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Kontakt", "Kontakt")
      If strValue = String.Empty Then strValue = "Kontakt"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ZHD_Kontakt"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_1State", "1. Status")
      If strValue = String.Empty Then strValue = "1. Status"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ZHD_1State"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_2State", "2. Status")
      If strValue = String.Empty Then strValue = "2. Status"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ZHD_2State"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Kommunikation", "Kommunikation")
      If strValue = String.Empty Then strValue = "Kommunikation"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ZHD_Kommunikation"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Versand", "Versand")
      If strValue = String.Empty Then strValue = "Versand"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ZHD_Versand"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Res1", "1. Res")
      If strValue = String.Empty Then strValue = "1. Res"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ZHD_Res1"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Res2", "2. Res")
      If strValue = String.Empty Then strValue = "2. Res"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ZHD_Res2"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Res3", "3. Res")
      If strValue = String.Empty Then strValue = "3. Res"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ZHD_Res3"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "FieldsLable", "Lbl_ZHD_Res4", "4. Res")
      If strValue = String.Empty Then strValue = "4. Res"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ZHD_Res4"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If


      ' Einsatzverwaltung
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESTSpesen", "Tagesspesen")
      If strValue = String.Empty Then strValue = "Tagesspesen"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ES_TSpesen"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz1", "1. Zusatz")
      If strValue = String.Empty Then strValue = "1. Zusatz"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ES_1Zusatz"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz2", "2. Zusatz")
      If strValue = String.Empty Then strValue = "2. Zusatz"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ES_2Zusatz"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz3", "3. Zusatz")
      If strValue = String.Empty Then strValue = "3. Zusatz"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ES_3Zusatz"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz4", "4. Zusatz")
      If strValue = String.Empty Then strValue = "4. Zusatz"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ES_4Zusatz"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz5", "5. Zusatz")
      If strValue = String.Empty Then strValue = "5. Zusatz"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ES_5Zusatz"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezESZuSatz6", "6. Zusatz")
      If strValue = String.Empty Then strValue = "6. Zusatz"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ES_6Zusatz"
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      ' Allgemeine Felder...
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezKST1", "1. Kst.")
      If strValue = String.Empty Then strValue = "1. Kst."
      If strValue <> String.Empty Then
        strStartElementUName_1 = "1. Kst."
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezKST2", "2. Kst.")
      If strValue = String.Empty Then strValue = "2. Kst."
      If strValue <> String.Empty Then
        strStartElementUName_1 = "2. Kst."
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezKST3", "3. Kst.")
      If strValue = String.Empty Then strValue = "3. Kst."
      If strValue <> String.Empty Then
        strStartElementUName_1 = "3. Kst."
        .WriteStartElement("Control") ' <KD 
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2) ' <KD 
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      ' KD_Anstellungsart...
      strValue = "Anstellungsart"
      strStartElementUName_1 = "KD_Anstellungsarten"
      .WriteStartElement("Control") ' <KD 
      .WriteAttributeString(strAttribute, strStartElementUName_1)
      .WriteStartElement(strField_2) ' <KD 
      .WriteString(strValue)
      .WriteEndElement()
      .WriteEndElement()


      ' Kandidatenmaske
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezTelefon-P", "Telefon")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "Telefon privat"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezTelefon-1", "Fax privat")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "Fax privat"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezTelefon-2", "Telefon G.")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "Telefon G."
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "BezTelefon-3", "Fax G.")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "Fax G."
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "HQualifikation", "Haupqualifikation")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "Qualifikation"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Lbl_MA_Gemeinde", "Gemeinde")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "Gemeinde"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "ResAuto", "Reserv")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "ResAuto"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Beurteilung", "Beurteilung")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "Beurteilung"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "SQualifikation", "Sonstige Qualifikation")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "Sonstige Qualifikation"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MAKommArt", "Kommunikationsart")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "Kommunikationsart"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MAKontakt", "Kontakt")
      If strValue = String.Empty Then strValue = "Kontakt"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "MAKontakt"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA1Status", "1. Status")
      If strValue = String.Empty Then strValue = "1. Status"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "MA1Status"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA2Status", "2. Status")
      If strValue = String.Empty Then strValue = "2. Status"
      If strValue <> String.Empty Then
        strStartElementUName_1 = "MA2Status"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "Einstellungsart", "Anstellungsart")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "Anstellungsart"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA1Res", "1. Reserve")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "MA1Res"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA2Res", "2. Reserve")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "MA2Res"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA3Res", "3. Reserve")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "MA3Res"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA4Res", "4. Reserve")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "MA4Res"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MAResLbl", "Reservefelder")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "MAResLbl"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA1LOCheck", "MA1LOCheck")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "MA1LOCheck"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA2LOCheck", "MA2LOCheck")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "MA2LOCheck"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Sonstiges", "MA3LOCheck", "MA3LOCheck")
      If strValue <> String.Empty Then
        strStartElementUName_1 = "MA3LOCheck"
        .WriteStartElement("Control")
        .WriteAttributeString(strAttribute, strStartElementUName_1)
        .WriteStartElement(strField_2)
        .WriteString(strValue)
        .WriteEndElement()
        .WriteEndElement()
      End If

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
      If strValue = String.Empty Then strValue = "1. Bemerkung"
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
      If strValue = String.Empty Then strValue = "2. Bemerkung"
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
      If strValue = String.Empty Then strValue = "3. Bemerkung"
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
      If strValue = String.Empty Then strValue = "4. Bemerkung"
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
      If strValue = String.Empty Then strValue = "5. Bemerkung"
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

      strValue = "({0}) Aktive Einsätze per Heute: {1}" & New String(CChar(" "), 1000)
      If strValue = String.Empty Then strValue = "({0}) Aktive Einsätze per Heute: {1}" & New String(CChar(" "), 1000)
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


      .WriteEndElement()

      ' ... und schließen das XML-Dokument (und die Datei) 
      .Close() ' Document 

      If bShowMessasge Then
        MessageBox.Show("Die Datei " & strFileName & " wurde erfolgreich erstellt.", _
                        "WriteDataToXML", MessageBoxButtons.OK, MessageBoxIcon.Information)
      End If

    End With

  End Sub

  Sub WriteUsDocDataToXML(ByVal strFileName As String, ByVal bShowMessasge As Boolean)
    Dim enc As New System.Text.UnicodeEncoding
    Dim strStartElementName As String = "UserProfile"
    Dim strAttribute As String = "UserNr"
    Dim strField_2 As String = "Document"
    Dim strField_3 As String = ""
    Dim strOldUSProFile As String = _ClsSettingPath.GetMDMainPath() & "Profiles\UserPro" & _ClsSettingPath.GetLogedUSNr

    ' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
    If bShowMessasge Then
      If File.Exists(strFileName) Then
        If MessageBox.Show("Die Datei existiert bereits. Soll die Datei neu angelegt werden?", "XML-Datei", _
                    MessageBoxButtons.YesNo, _
                    MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
      End If
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
        .WriteStartElement("User_" & _ClsSettingPath.GetLogedUSNr)
        .WriteStartElement("Documents")

        While rDocrec.Read
          Try

            strValue = _ClsReg.GetINIString(strOldUSProFile, "Export", CStr(rDocrec("DocName")))
            If strValue = String.Empty Then strValue = "0"

            .WriteStartElement("DocName")
            .WriteAttributeString("ID", CStr(rDocrec("JobNr")))
            .WriteStartElement("Export")
            .WriteString(strValue)
            .WriteEndElement()
            .WriteEndElement()

          Catch ex As Exception
            MsgBox(CStr(rDocrec("JobNr")) & ": " & ex.Message, MsgBoxStyle.Critical, "WriteUSDocDataToXML_1")


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
        .Close()

        If bShowMessasge Then
          MessageBox.Show("Die Datei " & strFileName & " wurde erfolgreich erstellt.", _
                          "WriteUserDataToXML", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

      End With

    Catch ex As Exception
      MsgBox(ex.Message, MsgBoxStyle.Critical, "WriteUSDocDataToXML_0")

    End Try

  End Sub

  Sub WriteUsFrmCtlDataToXML(ByVal strFileName As String, ByVal bShowMessasge As Boolean)
    Dim enc As New System.Text.UnicodeEncoding
    Dim strStartElementName As String = "UserProfile"
    Dim strAttribute As String = "UserNr"
    Dim strField_2 As String = "Document"
    Dim strField_3 As String = ""
    Dim strFilepath As String = _ClsSettingPath.GetSkinPath() & "4c2db8b0-0521-4862-a640-d895e02100f9\"

    Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFileName, enc)
    Dim strValue As String = String.Empty

    With XMLobj
      ' Formatierung: 4er-Einzüge verwenden 
      .Formatting = Xml.Formatting.Indented
      .Indentation = 4

      .WriteStartDocument()
      .WriteStartElement(strStartElementName)

      .WriteStartElement("FormControls")
      .WriteAttributeString("Name", "4c2db8b0-0521-4862-a640-d895e02100f9")
      .WriteStartElement("LibKDNr1")
      .WriteAttributeString("Visible", "true")
      .WriteEndElement()

      .WriteStartElement("txtKDNr_1")
      .WriteAttributeString("Visible", "true")
      .WriteEndElement()

      .WriteStartElement("txtKDNr_2")
      .WriteAttributeString("Visible", "true")
      .WriteEndElement()

      .WriteStartElement("LibKDNr2")
      .WriteAttributeString("Visible", "true")
      .WriteEndElement()

      .WriteEndElement()


      .WriteEndElement()
      .Close()

      If bShowMessasge Then
        MessageBox.Show("Die Datei " & strFileName & " wurde erfolgreich erstellt.", _
                        "WriteUsFrmCtlDataToXML", MessageBoxButtons.OK, MessageBoxIcon.Information)
      End If

    End With

  End Sub

  Sub WriteSQLDataToXML(ByVal strFileName As String, ByVal bShowMessasge As Boolean)
    Dim enc As New System.Text.UnicodeEncoding
    Dim strStartElementName As String = "SQLProfile"
    Dim strAttribute As String = "Form"
    Dim strField_2 As String = "LV"
    Dim strField_3 As String = ""

    ' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
    If bShowMessasge Then
      If File.Exists(strFileName) Then
        If MessageBox.Show("Die Datei existiert bereits. Soll die Datei neu angelegt werden?", "XML-Datei", _
                    MessageBoxButtons.YesNo, _
                    MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
      End If
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

  Sub WriteMDDataToXML(ByVal strFileName As String, ByVal bShowMessasge As Boolean)
    Dim enc As New System.Text.UnicodeEncoding
    Dim strStartElementName As String = "MD_" & _ClsSettingPath.GetMDNr
    Dim strAttribute As String = String.Empty
    Dim strField_2 As String = "LV"
    Dim strField_3 As String = ""
    Dim strMyMDDatFile As String = _ClsSettingPath.GetMDIniFile()
    Dim strSection As String = String.Empty

    ' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
    If bShowMessasge Then
      If File.Exists(strFileName) Then
        If MessageBox.Show("Die Datei existiert bereits. Soll die Datei neu angelegt werden?", "XML-Datei", _
                    MessageBoxButtons.YesNo, _
                    MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
      End If
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

      ' Sonstiges fertig schreiben...
      .WriteEndElement()




      strSection = "Export" ' ........................................................................................
      .WriteStartElement(strSection)
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MA_SPUser_ID"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "SaveVersandKontakt_0"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "0")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "SaveVersandKontakt_1"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "0")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "FaxRecipient_1"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "0")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "FaxRecipient_2"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "0")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "KD_SPUser_ID"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Vak_SPUser_ID"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()

      ' Sonstiges fertig schreiben...
      .WriteEndElement()







      strSection = "Templates" ' .....................................................................................
      .WriteStartElement(strSection)
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Cockpit-Picture"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
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


      ' Sonstiges fertig schreiben...
      .WriteEndElement()



      strSection = "Mailing" ' ........................................................................................
      .WriteStartElement(strSection)
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Fax-Server"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Fax-Extension"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "David-Server"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Fax-Forwarder"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "SMTP-Server"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "SMTP-Port"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()

      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Cockpit-WWW"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
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
      strAttribute = "Zwischenverdienstformular_Doc-eMail-Template"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "MailTempl_ZVDoc.txt")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Arbeitgeberbescheinigung_Doc-eMail-Template"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "MailTempl_ArbgDoc.txt")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MADoc-eMail-Template"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "MailTempl_MADoc.txt")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "KDDoc-eMail-Template"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "MailTempl_KDDoc.txt")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "ZHDDoc-eMail-Template"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "MailTempl_ZHDDoc.txt")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Cockpit-eMail-Template"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, strSection, strAttribute, "MailTempl_CockpitWOS.txt")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()


      ' Sonstiges fertig schreiben...
      .WriteEndElement()





      strSection = "SUVA-Daten" ' ....................................................................................
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

      For i As Integer = 0 To 19
        strAttribute = Str(i + 1).Trim
        strValue = _ClsReg.GetINIString(strMyMDDatFile, "BuchungsKonten", strAttribute, "")
        .WriteStartElement("_" & strAttribute)
        .WriteString(strValue)
        .WriteEndElement()
      Next

      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      '-------------------------------------------------------------------------------------------------------------------

      .WriteStartElement("StartNr")

      strAttribute = "Mahnungen"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Mitarbeiter"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Kunden"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Vakanzennr"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Offers"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Rapporte"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Vorschussverwaltung"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Fakturen"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Zahlungseingänge"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Lohnabrechnung"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "FremdOP"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Kassenbuch"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "FirstKassenBetrag"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()

      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      '-------------------------------------------------------------------------------------------------------------------

      .WriteStartElement("Debitoren")

      strAttribute = "MWSTNr"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "MWST-Satz"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "ESRArt"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(strAttribute)
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "ESR-ID"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "ESRKonto"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "ESRFile"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "RoundDiff"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "VerzugszinsOn"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Mahnspesen ab"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Mahnspesen"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "VerzugsZinsen"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Verzugszins Ab"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "AnzahlJTage"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "AnzahlMTage"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "RefNrTo10"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "KontoNr"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "DTAFileName"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Trenne die Einzahlungsscheine"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "Create3MahnByNotPaid"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "FakDateAsRPEndDate"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "FACTOSKDNr"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "OPFileName"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "OPZipFileName"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
      .WriteString(strValue)
      .WriteEndElement()
      '-------------------------------------------------------------------------------------------------------------------
      strAttribute = "OPZipFileLen"
      strValue = _ClsReg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
      .WriteStartElement(Replace(strAttribute, " ", "_"))
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

  Sub WriteMSGDataToXML(ByVal bShowMessasge As Boolean)
    Dim strFilename = _ClsSettingPath.GetMSGData_XMLFile()
    Dim enc As New System.Text.UnicodeEncoding
    Dim strStartElementName As String = "Messages"
    Dim strAttribute As String = String.Empty
    Dim strField_2 As String = "LV"
    Dim strField_3 As String = ""
    Dim strMyMDDatFile As String = _ClsSettingPath.GetMDIniFile()

    ' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
    If bShowMessasge Then
      If File.Exists(strFilename) Then
        If MessageBox.Show("Die Datei existiert bereits. Soll die Datei neu angelegt werden?", "XML-Datei", _
                    MessageBoxButtons.YesNo, _
                    MessageBoxIcon.Question) <> DialogResult.Yes Then Exit Sub
      End If
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

#End Region


End Class



Public Class ClsXML_1


End Class
