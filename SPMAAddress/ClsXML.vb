
'Imports System.IO
'Imports System.Xml
'Imports System.Xml.Linq

'Imports System.Xml.XmlTextWriter
'Imports System.Xml.XmlTextReader
'Imports System.Xml.XPath

'Imports NLog

'Public Class ClsXML

'  Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

'  Dim _ClsReg As New SPProgUtility.ClsDivReg
'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingpath

'  Dim _ClsLog As New SPProgUtility.ClsEventLog

'  Dim strUSLang As String = _ClsProgSetting.GetUSLanguage()
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
'      If TypeOf (cControl) Is Label Or TypeOf (cControl) Is LinkLabel Or TypeOf (cControl) Is Form Or _
'                  TypeOf (cControl) Is CheckBox Or TypeOf (cControl) Is Button Or _
'                  TypeOf (cControl) Is TabPage Or TypeOf (cControl) Is GroupBox Or _
'                  TypeOf (cControl) Is Panel Or _
'                  TypeOf (cControl) Is TabControl Or TypeOf (cControl) Is TabPage Or _
'                  TypeOf (cControl) Is Button Or _
'                  TypeOf (cControl) Is StatusBar Or _
'                  TypeOf (cControl) Is StatusStrip Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.PanelControl Or _
'                  TypeOf (cControl) Is DevExpress.XtraTab.XtraTabPage Or _
'                  TypeOf (cControl) Is DevExpress.XtraTab.XtraTabControl Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.CheckEdit Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.SimpleButton Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.CheckButton Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.PopupContainerControl Or _
'                  TypeOf (cControl) Is DevExpress.XtraEditors.GroupControl Or _
'                  TypeOf (cControl) Is System.Windows.Forms.RadioButton Then
'        Trace.WriteLine(String.Format("Innen: name: {0}, bez: {1} / Typeof: {2}", _
'                              cControl.Name, cControl.Text, _
'                              cControl.GetType))

'        If TypeOf (cControl) Is LinkLabel Then cControl.TabStop = False
'        If TypeOf (cControl) Is DevExpress.XtraEditors.PanelControl Then
'          Trace.WriteLine(cControl.Name)
'        End If

'        Dim strOrgControlBez As String = cControl.Text.Trim
'        Dim strChangedControlBez As String = cControl.Text.Trim
'        If strOrgControlBez = String.Empty Then
'          Trace.WriteLine("leere Text")
'        End If
'        If TypeOf (cControl) Is StatusStrip Then
'          Dim ts As ToolStrip = CType(cControl, ToolStrip)
'          For Each itm As ToolStripItem In ts.Items
'            If itm.Text <> String.Empty Then
'              itm.Text = _ClsProgSetting.TranslateText(itm.Text)
'              Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
'            End If
'          Next

'        Else
'          If strOrgControlBez <> String.Empty Then

'            strQuery = "//Control[@Name=" & Chr(34) & strOrgControlBez & Chr(34) & "]/CtlLabel" & strUSLang
'            xni = xpNav.Select(strQuery)
'            Do While xni.MoveNext()
'              strBez = xni.Current.Value
'              strChangedControlBez = strBez
'            Loop
'            cControl.Invoke(Sub()
'                              cControl.Text = _ClsProgSetting.TranslateText(strChangedControlBez)
'                            End Sub)
'          End If

'          If cControl.HasChildren Then
'            For Each ctrl_1 As Control In cControl.Controls
'              GetChildChildBez(CType(ctrl_1, Control))
'            Next
'          End If

'        End If
'      Else
'        Trace.WriteLine(String.Format("Aussen: name: {0}, bez: {1} / Typeof: {2}", _
'                                      cControl.Name, cControl.Text, _
'                                      cControl.GetType))

'      End If

'    Catch ex As Exception
'      logger.Error(String.Format("{0}.{1}", strMethodeName, ex.Message))

'    End Try

'  End Sub

'  Function GetColorValue() As String
'    Dim strModul2print As String = String.Empty

'    'Bruttoumsatzliste Rapporte Total
'    Dim strQuery As String = "//All_FieldsColor/ToasNotification/Information"

'    Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetFormDataFile(), strQuery)
'    If strBez <> String.Empty Then
'      strModul2print = strBez

'    Else
'      strBez = "128;0;0"

'    End If

'    Return strBez
'  End Function

'	'Function CheckForMustFields(ByVal frm As frmMAAddress) As Boolean
'	'  Dim bResult As Boolean = True

'	'  For Each ctrls As Control In frm.Controls

'	'    If TypeOf (ctrls) Is TabPage Or TypeOf (ctrls) Is DevExpress.XtraTab.XtraTabPage Then
'	'      Dim grp As Control = ctrls
'	'      For Each con2 As Control In grp.Controls
'	'        Dim liFieldInfo As String() = IsCtlOK(con2)
'	'        If CInt(Val(liFieldInfo(0))) = 1 Then
'	'          bResult = False
'	'          If liFieldInfo.Length > 1 Then
'	'            frm.ErrorProvider1.SetError(ctrls, TranslateMyText(liFieldInfo(1)))

'	'          Else
'	'            frm.ErrorProvider1.SetError(ctrls, _
'	'                                        If(liFieldInfo.Length > 1, _
'	'                                           TranslateMyText(liFieldInfo(1)), _
'	'                                           TranslateMyText("Keine Bezeichnung...")))
'	'          End If
'	'        End If
'	'      Next

'	'    ElseIf TypeOf (ctrls) Is GroupBox Or TypeOf (ctrls) Is DevExpress.XtraEditors.GroupControl Then
'	'      Dim grp As Control = ctrls
'	'      For Each con2 As Control In grp.Controls
'	'        Dim liFieldInfo As String() = IsCtlOK(con2)
'	'        If CInt(Val(liFieldInfo(0))) = 1 Then
'	'          bResult = False
'	'          frm.ErrorProvider1.SetError(ctrls, _
'	'                                      If(liFieldInfo.Length > 1, _
'	'                                         TranslateMyText(liFieldInfo(1)), _
'	'                                         TranslateMyText("Keine Bezeichnung...")))

'	'        End If
'	'      Next

'	'    ElseIf TypeOf (ctrls) Is TabControl Or TypeOf (ctrls) Is DevExpress.XtraTab.XtraTabControl Then
'	'      Dim grp As Control = ctrls
'	'      For Each con2 As Control In grp.Controls
'	'        Dim liFieldInfo As String() = IsCtlOK(con2)
'	'        If CInt(Val(liFieldInfo(0))) = 1 Then
'	'          bResult = False
'	'          frm.ErrorProvider1.SetError(ctrls, _
'	'                                     If(liFieldInfo.Length > 1, _
'	'                                        TranslateMyText(liFieldInfo(1)), _
'	'                                        TranslateMyText("Keine Bezeichnung...")))
'	'        End If
'	'      Next

'	'    ElseIf TypeOf (ctrls) Is TextBox Or TypeOf (ctrls) Is ComboBox Then
'	'      Dim liFieldInfo As String() = IsCtlOK(ctrls)
'	'      If CInt(Val(liFieldInfo(0))) = 1 Then
'	'        bResult = False
'	'        frm.ErrorProvider1.SetError(ctrls, _
'	'                          If(liFieldInfo.Length > 1, _
'	'                             TranslateMyText(liFieldInfo(1)), _
'	'                             TranslateMyText("Keine Bezeichnung...")))

'	'      End If

'	'    End If

'	'  Next

'	'  Return bResult
'	'End Function

'	'Function IsCtlOK(ByVal cControl As Control) As String()
'	'  Dim bSave As Boolean = True
'	'  Dim strFieldInfo As String() = {"0", "", ""}
'	'  Dim bPflichtig As Boolean = False
'	'  Dim strFieldDescription As String = String.Empty
'	'  Dim strErrorMsg As String = ": Leeres oder ungültiges Feld"

'	'  'If cControl.Name.ToLower.Contains("bezeichnung".ToLower) Then
'	'  '  Trace.WriteLine(cControl.Name)
'	'  'End If
'	'  If TypeOf (cControl) Is TextBox Or TypeOf (cControl) Is ComboBox Then
'	'    If Not IsNothing(cControl.AccessibleDescription) AndAlso _
'	'                                      cControl.AccessibleDescription.ToLower.Contains("Pflichtfeld".ToLower) Then
'	'      If cControl.Text = "0" Or (cControl.Text = String.Empty) Then
'	'        strFieldInfo(0) = "1"
'	'        strFieldInfo(1) = cControl.AccessibleDescription
'	'      End If
'	'    End If

'	'  End If

'	'  Return strFieldInfo
'	'End Function



'End Class
