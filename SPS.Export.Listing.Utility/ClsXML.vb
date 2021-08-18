
Imports System.Data.SqlClient

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports System.IO
Imports System.Windows.Forms

Public Class ClsXML


  Sub GetChildChildBez(ByVal cControl As Control)
    Dim strBez As String = String.Empty
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Try
      If cControl.Name = "LblChange_9" Then
        Trace.WriteLine(String.Format("Func_2: name: {0}, bez: {1} / Typeof: {2}", _
                                    cControl.Name, cControl.Text, _
                                    cControl.GetType))
      End If
      Trace.WriteLine(String.Format("Func_2: name: {0}, bez: {1} / Typeof: {2}", _
                                    cControl.Name, cControl.Text, _
                                    cControl.GetType))


      If TypeOf (cControl) Is Label Or TypeOf (cControl) Is LinkLabel Or TypeOf (cControl) Is Form Or _
                  TypeOf (cControl) Is CheckBox Or TypeOf (cControl) Is Button Or _
                  TypeOf (cControl) Is TabPage Or TypeOf (cControl) Is GroupBox Or _
                  TypeOf (cControl) Is Panel Or _
                  TypeOf (cControl) Is TabControl Or TypeOf (cControl) Is TabPage Or _
                  TypeOf (cControl) Is Button Or _
                  TypeOf (cControl) Is StatusBar Or _
                  TypeOf (cControl) Is StatusStrip Or _
                  TypeOf (cControl) Is DevExpress.XtraEditors.PanelControl Or _
                  TypeOf (cControl) Is DevExpress.XtraEditors.PanelControl Or _
                  TypeOf (cControl) Is DevExpress.XtraTab.XtraTabPage Or _
                  TypeOf (cControl) Is DevExpress.XtraTab.XtraTabControl Or _
                  TypeOf (cControl) Is DevExpress.XtraEditors.CheckEdit Or _
                  TypeOf (cControl) Is DevExpress.XtraEditors.SimpleButton Or _
                  TypeOf (cControl) Is DevExpress.XtraEditors.PopupContainerControl Or _
                  TypeOf (cControl) Is System.Windows.Forms.RadioButton Then
        If TypeOf (cControl) Is LinkLabel Then cControl.TabStop = False
        If TypeOf (cControl) Is Panel Then
          'Trace.WriteLine(cControl.Name)
        End If
        Dim strOrgControlBez As String = cControl.Text.Trim
        Dim strChangedControlBez As String = cControl.Text.Trim

        If TypeOf (cControl) Is StatusStrip Then
          Dim ts As ToolStrip = CType(cControl, ToolStrip)
          For Each itm As ToolStripItem In ts.Items
            itm.Text = GetSafeTranslationValue(itm.Text, True)
            Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
          Next

        Else
          cControl.Text = GetSafeTranslationValue(strChangedControlBez, True)

          If cControl.HasChildren Then
            For Each ctrl_1 As Control In cControl.Controls
              GetChildChildBez(CType(ctrl_1, Control))
            Next
          End If

        End If
      End If

    Catch ex As Exception

    End Try

  End Sub

  Function GetSafeTranslationValue(ByVal dicKey As String) As String
    Dim strPersonalizedItem As String = dicKey
		If ClsMainSetting.TranslationData Is Nothing Then Return dicKey

		Try
			If ClsMainSetting.TranslationData.ContainsKey(strPersonalizedItem) Then
				Return ClsMainSetting.TranslationData.Item(strPersonalizedItem).LogedUserLanguage

			Else
				Return strPersonalizedItem

			End If

		Catch ex As Exception
			Return strPersonalizedItem
		End Try

	End Function

  Function GetSafeTranslationValue(ByVal dicKey As String, ByVal bCheckPersonalizedItem As Boolean) As String
    Dim strPersonalizedItem As String = dicKey

    Try
      If bCheckPersonalizedItem Then
        If ClsMainSetting.ProsonalizedData.ContainsKey(dicKey) Then
          strPersonalizedItem = ClsMainSetting.ProsonalizedData.Item(dicKey).CaptionValue

        Else
          strPersonalizedItem = strPersonalizedItem

        End If
      End If

      If ClsMainSetting.TranslationData.ContainsKey(strPersonalizedItem) Then
        Return ClsMainSetting.TranslationData.Item(strPersonalizedItem).LogedUserLanguage

      Else
        Return strPersonalizedItem

      End If

    Catch ex As Exception
      Return strPersonalizedItem
    End Try

  End Function

  'Function GetColorValue() As String
  '  Dim strModul2print As String = String.Empty
  '  Dim m_md As New Mandant
  '  Dim m_utility As New Utilities

  '  Dim strQuery As String = "//All_FieldsColor/Gruppenüberschrift"

  '  Dim strBez As String = m_utility.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(ClsMainSetting.ProgSettingData.SelectedMDNr), strQuery)
  '  If strBez <> String.Empty Then
  '    strModul2print = strBez

  '  Else
  '    strBez = "128;0;0"

  '  End If

  '  Return strBez
  'End Function


End Class





'Imports System.Data.SqlClient

'Imports System.IO
'Imports System.Xml
'Imports System.Xml.Linq

'Imports System.Xml.XmlTextWriter
'Imports System.Xml.XmlTextReader
'Imports System.Xml.XPath
'Imports System.Windows.Forms

'Public Class ClsXML

'  Dim _ClsReg As New SPProgUtility.ClsDivReg
'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
'  Dim _ClsLog As New SPProgUtility.ClsEventLog

'  Dim strUSLang As String = ""
'  Dim xmlDoc As New Xml.XmlDocument()
'  Dim strLogFilename As String = _ClsProgSetting.GetSpSTempPath & "DeinFile.txt"

'  Sub GetChildChildBez(ByVal cControl As Control)
'    Dim strBez As String = String.Empty
'    Dim strQuery As String = String.Empty
'    Dim strMainNode As String = "//Form_Normaly"

'    Dim xpNav As XPathNavigator
'    Dim xni As XPathNodeIterator
'    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'    Try
'      'If cControl.Name = "LibZRes1" Then
'      '  Trace.WriteLine(String.Format("Func_2: name: {0}, bez: {1} / Typeof: {2}", _
'      '                              cControl.Name, cControl.Text, _
'      '                              cControl.GetType))
'      'End If
'      xpNav = xmlDoc.CreateNavigator()
'      Trace.WriteLine(String.Format("Func_2: name: {0}, bez: {1} / Typeof: {2}", _
'                                    cControl.Name, cControl.Text, _
'                                    cControl.GetType))


'      If TypeOf (cControl) Is Label Or TypeOf (cControl) Is LinkLabel Or _
'                  TypeOf (cControl) Is CheckBox Or TypeOf (cControl) Is Button Or _
'                  TypeOf (cControl) Is TabPage Or TypeOf (cControl) Is GroupBox Or _
'                  TypeOf (cControl) Is Panel Or _
'                  TypeOf (cControl) Is TabControl Or TypeOf (cControl) Is TabPage Or _
'                  TypeOf (cControl) Is Button Or _
'                  TypeOf (cControl) Is StatusBar Or _
'                  TypeOf (cControl) Is StatusStrip Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.PanelControl Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.PanelControl Or _
'                  TypeOf (cControl) Is DevExpress.XtraTab.XtraTabPage Or _
'                  TypeOf (cControl) Is DevExpress.XtraTab.XtraTabControl Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.CheckEdit Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.SimpleButton Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.PopupContainerControl Or _
'                  TypeOf (cControl) Is System.Windows.Forms.RadioButton Then
'        If TypeOf (cControl) Is LinkLabel Then cControl.TabStop = False
'        If TypeOf (cControl) Is Panel Then
'          'Trace.WriteLine(cControl.Name)
'        End If
'        Dim strOrgControlBez As String = cControl.Text.Trim
'        Dim strChangedControlBez As String = cControl.Text.Trim

'        strQuery = "//Control[@Name=" & Chr(34) & strOrgControlBez & Chr(34) & "]/CtlLabel" & strUSLang
'        xni = xpNav.Select(strQuery)
'        Do While xni.MoveNext()
'          strBez = xni.Current.Value
'          strChangedControlBez = strBez
'        Loop
'        cControl.Text = m_xml.GetSafeTranslationValue(strChangedControlBez)

'        If cControl.HasChildren Then
'          For Each ctrl_1 As Control In cControl.Controls
'            GetChildChildBez(CType(ctrl_1, Control))
'          Next
'        End If

'      End If


'    Catch ex As Exception

'    End Try

'  End Sub

'  Sub GetFormDataFromXML(ByVal frm As Form, ByVal strFilename As String)
'    Dim strBez As String = String.Empty
'    Dim strQuery As String = String.Empty
'    Dim strMainNode As String = "//Form_Normaly"

'    Dim xpNav As XPathNavigator
'    Dim xni As XPathNodeIterator
'    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'    Try
'      If Not File.Exists(strFilename) Then Exit Sub
'      strUSLang = String.Empty

'      xmlDoc.Load(strFilename)
'      xpNav = xmlDoc.CreateNavigator()

'      With frm
'        frm.Text = m_xml.GetSafeTranslationValue(frm.Text)
'        For Each cControl As Control In frm.Controls
'          If TypeOf (cControl) Is Label Or TypeOf (cControl) Is LinkLabel Or _
'                      TypeOf (cControl) Is CheckBox Or TypeOf (cControl) Is Button Or _
'                      TypeOf (cControl) Is TabPage Or TypeOf (cControl) Is GroupBox Or _
'                      TypeOf (cControl) Is Panel Or _
'                      TypeOf (cControl) Is TabControl Or _
'                      TypeOf (cControl) Is StatusBar Or _
'                      TypeOf (cControl) Is StatusStrip Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.PanelControl Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.PanelControl Or _
'                  TypeOf (cControl) Is DevExpress.XtraTab.XtraTabPage Or _
'                  TypeOf (cControl) Is DevExpress.XtraTab.XtraTabControl Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.CheckEdit Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.SimpleButton Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.PopupContainerControl Or _
'                  TypeOf (cControl) Is System.Windows.Forms.RadioButton Then
'            Dim strOrgControlBez As String = cControl.Text.Trim
'            Dim strChangedControlBez As String = cControl.Text.Trim

'            If TypeOf (cControl) Is StatusStrip Then
'              Dim ts As ToolStrip = CType(cControl, ToolStrip)
'              For Each itm As ToolStripItem In ts.Items
'                itm.Text = m_xml.GetSafeTranslationValue(itm.Text)
'                Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
'              Next
'            Else

'              If cControl.HasChildren Then
'                For Each ctrl In cControl.Controls
'                  GetChildChildBez(CType(ctrl, Control))
'                Next

'              Else
'                strQuery = "//Control[@Name=" & Chr(34) & strOrgControlBez & Chr(34) & "]/CtlLabel" & strUSLang
'                xni = xpNav.Select(strQuery)
'                Do While xni.MoveNext()
'                  strBez = xni.Current.Value
'                  strChangedControlBez = strBez
'                Loop
'                cControl.Text = m_xml.GetSafeTranslationValue(strChangedControlBez)

'              End If
'            End If

'          Else
'            Trace.WriteLine(String.Format("Aussen: name: {0}, bez: {1} / Typeof: {2}", _
'                                          cControl.Name, cControl.Text, _
'                                          cControl.GetType))
'          End If
'        Next


'        Try
'          .StartPosition = FormStartPosition.CenterScreen

'        Catch ex As Exception
'          .StartPosition = FormStartPosition.CenterScreen

'        End Try

'      End With

'    Catch ex As Exception
'      Dim strMessage As String = "Ein Fehler ist während der Ausführung des Programms aufgetreten. {0}"
'      MessageBox.Show(String.Format(m_xml.GetSafeTranslationValue(strMessage), _
'                                      ex.Message, strMethodeName), strMethodeName)
'    End Try

'  End Sub

'  Sub WriteUsDocDataToXML(ByVal strFileName As String, ByVal bShowMessasge As Boolean)
'    Dim enc As New System.Text.UnicodeEncoding
'    Dim strStartElementName As String = "UserProfile"
'    Dim strAttribute As String = "UserNr"
'    Dim strField_2 As String = "Document"
'    Dim strField_3 As String = ""
'    Dim strOldUSProFile As String = System.IO.Directory.GetParent(_ClsProgSetting.GetUserProfileFile).FullName & _
'      "\UserPro" & _ClsProgSetting.GetLogedUSNr

'    Dim strOldFilename As String = _ClsProgSetting.GetSpSFiles2DeletePath & "OldFile.xml"
'    Try
'      File.Copy(strFileName, strOldFilename, True)
'    Catch ex As Exception

'    End Try

'    Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFileName, enc)
'    Dim strValue As String = String.Empty


'    Try

'      With XMLobj
'        ' Formatierung: 4er-Einzüge verwenden 
'        .Formatting = Xml.Formatting.Indented
'        .Indentation = 4

'        .WriteStartDocument()
'        .WriteStartElement(strStartElementName)
'        .WriteStartElement("User_" & _ClsProgSetting.GetLogedUSNr)
'        .WriteStartElement("Documents")



'        ' Die Masken-Vorlage für Kundensuche...
'        .WriteStartElement("FormControls")
'        .WriteStartElement("FormName")
'        .WriteAttributeString("ID", "4c2db8b0-0521-4862-a640-d895e02100f9")
'        .WriteStartElement("TemplateFile")
'        .WriteString("")
'        .WriteEndElement()
'        .WriteEndElement()
'        ' Fertig

'        ' Die Sonstigen Einstellungen...
'        .WriteStartElement("USSetting")
'        .WriteStartElement("SettingName")
'        .WriteAttributeString("ID", "Cockpit.DayAgo4GebDat")
'        .WriteStartElement("USValue")
'        .WriteString("")
'        .WriteEndElement()
'        .WriteEndElement()
'        ' Fertig


'        .WriteEndElement()
'        .Close()

'        If bShowMessasge Then
'          MessageBox.Show("Die Datei " & strFileName & " wurde erfolgreich erstellt.", _
'                          "WriteUserDataToXML", MessageBoxButtons.OK, MessageBoxIcon.Information)
'        End If

'      End With

'    Catch ex As Exception
'      MsgBox(ex.Message, MsgBoxStyle.Critical, "WriteUSDocDataToXML_0")

'    End Try

'  End Sub


'  Function GetColorValue() As String
'    Dim strModul2print As String = String.Empty

'    'Bruttoumsatzliste Rapporte Total
'    Dim strQuery As String = "//All_FieldsColor/Gruppenüberschrift"

'    Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetFormDataFile(), strQuery)
'    If strBez <> String.Empty Then
'      strModul2print = strBez

'    Else
'      strBez = "128;0;0"

'    End If

'    Return strBez
'  End Function


'End Class


