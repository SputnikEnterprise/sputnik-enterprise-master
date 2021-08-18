
'Imports System.Data.SqlClient

'Imports System.IO
'Imports System.Xml
'Imports System.Xml.Linq

'Imports System.Xml.XmlTextWriter
'Imports System.Xml.XmlTextReader
'Imports System.Xml.XPath

'Public Class ClsXML

'  Dim _ClsReg As New SPProgUtility.ClsDivReg
'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
'  Dim _ClsLog As New SPProgUtility.ClsEventLog

'  Dim strUSLang As String = ""
'  Dim xmlDoc As New Xml.XmlDocument()
'  Dim strLogFilename As String = _ClsProgSetting.GetSpSTempPath & "DeinFile.txt"
'  Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)

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


'      If TypeOf (cControl) Is Label Or TypeOf (cControl) Is LinkLabel Or TypeOf (cControl) Is Form Or _
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

'        If TypeOf (cControl) Is StatusStrip Then
'          Dim ts As ToolStrip = CType(cControl, ToolStrip)
'          For Each itm As ToolStripItem In ts.Items
'            itm.Text = _ClsProgSetting.TranslateText(itm.Text)
'            Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
'          Next

'        Else
'          strQuery = "//Control[@Name=" & Chr(34) & strOrgControlBez & Chr(34) & "]/CtlLabel" & strUSLang
'          xni = xpNav.Select(strQuery)
'          Do While xni.MoveNext()
'            strBez = xni.Current.Value
'            strChangedControlBez = strBez
'          Loop
'          cControl.Text = TranslateMyText(strChangedControlBez)

'          If cControl.HasChildren Then
'            For Each ctrl_1 As Control In cControl.Controls
'              GetChildChildBez(CType(ctrl_1, Control))
'            Next
'          End If

'        End If
'      End If

'    Catch ex As Exception

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

'  Public Sub New()
'    xmlDoc.Load(_ClsProgSetting.GetFormDataFile)
'  End Sub

'End Class


