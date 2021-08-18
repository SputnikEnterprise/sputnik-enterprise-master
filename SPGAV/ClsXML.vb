
Imports System.Data.SqlClient

Imports System.IO
Imports System.Xml

Imports System.Xml.XmlTextWriter
Imports System.Xml.XmlTextReader
Imports System.Xml.XPath

Public Class ClsXML

  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Dim _ClsLog As New SPProgUtility.ClsEventLog

  Dim strUSLang As String = ""
  Dim xmlDoc As New Xml.XmlDocument()
  Dim strLogFilename As String = _ClsProgSetting.GetSpSTempPath & "DeinFile.txt"
  Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)

  Sub GetChildChildBez(ByVal cControl As Control)
    Dim strBez As String = String.Empty
    Dim strQuery As String = String.Empty
    Dim strMainNode As String = "//Form_Normaly"

    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Try
      If cControl.Name = "bbiRotate_" Then
        Trace.WriteLine(String.Format("Func_2: name: {0} | bez: {1} | Typeof: {2}", _
                                    cControl.Name, cControl.Text, _
                                    cControl.GetType))
      End If
      Trace.WriteLine(String.Format("Func_2: name: {0} | bez: {1} | Typeof: {2}", _
                                  cControl.Name, cControl.Text, _
                                  cControl.GetType))

      If TypeOf (cControl) Is Label Or TypeOf (cControl) Is LinkLabel Or _
                  TypeOf (cControl) Is CheckBox Or TypeOf (cControl) Is Button Or _
                  TypeOf (cControl) Is TabPage Or TypeOf (cControl) Is GroupBox Or _
                  TypeOf (cControl) Is Panel Or _
                  TypeOf (cControl) Is TabControl Or TypeOf (cControl) Is TabPage Or _
                  TypeOf (cControl) Is Button Or _
                  TypeOf (cControl) Is StatusBar Or _
                  TypeOf (cControl) Is StatusStrip Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.XtraScrollableControl Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.GroupControl Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.PanelControl Or _
                      TypeOf (cControl) Is DevExpress.XtraTab.XtraTabPage Or _
                      TypeOf (cControl) Is DevExpress.XtraTab.XtraTabControl Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.CheckEdit Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.SimpleButton Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.PopupContainerControl Or _
                      TypeOf (cControl) Is System.Windows.Forms.RadioButton Or _
                      TypeOf (cControl) Is DevExpress.XtraBars.Ribbon.RibbonStatusBar Or _
                      TypeOf (cControl) Is DevExpress.XtraBars.Ribbon.RibbonControl Then
        If TypeOf (cControl) Is LinkLabel Then cControl.TabStop = False
        If TypeOf (cControl) Is Panel Then
          'Trace.WriteLine(cControl.Name)
        End If
        Dim strOrgControlBez As String = cControl.Text.Trim
        Dim strChangedControlBez As String = strOrgControlBez

        strQuery = "//Control[@Name=" & Chr(34) & strOrgControlBez & Chr(34) & "]/CtlLabel" & strUSLang
        strBez = GetXMLValueByQuery(_ClsProgSetting.GetFormDataFile, strQuery, strChangedControlBez)

        cControl.Text = _ClsProgSetting.TranslateText(strBez)
        'cControl.Text = TranslateSelectedText(strBez)

        If cControl.HasChildren Then
          For Each ctrl_1 As Control In cControl.Controls
            GetChildChildBez(CType(ctrl_1, Control))
          Next
        End If

      Else
        If cControl.HasChildren Then
          Trace.WriteLine(String.Format("2. AussenControl (WithChild): Name: {0} | bez: {1} | Typeof: {2}", _
                                      cControl.Name, cControl.Text, _
                                      cControl.GetType))
          For Each ctrl_1 As Control In cControl.Controls
            GetChildChildBez(CType(ctrl_1, Control))
          Next
        Else
          Trace.WriteLine(String.Format("2. AussenControl (NoChild): Name: {0} | bez: {1} | Typeof: {2}", _
                                      cControl.Name, cControl.Text, _
                                      cControl.GetType))
        End If

      End If


    Catch ex As Exception

    End Try

  End Sub

  Sub GetFormDataFromXML(ByVal frm As DevExpress.XtraEditors.XtraForm)
    Dim strBez As String = String.Empty
    Dim strQuery As String = String.Empty
    Dim strMainNode As String = "//Form_Normaly"
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Try
      strUSLang = String.Empty

      With frm
        frm.Text = _ClsProgSetting.TranslateText(frm.Text)
        'frm.Text = TranslateSelectedText(frm.Text)

        For Each cControl As Control In frm.Controls
          Trace.WriteLine(String.Format("Func_1: name: {0} | bez: {1} | Typeof: {2}", _
                                      cControl.Name, cControl.Text, _
                                      cControl.GetType))

          If cControl.Name = "bbiRotate_" Then
            Trace.WriteLine(String.Format("Func_1: name: {0} | bez: {1} | Typeof: {2}", _
                                        cControl.Name, cControl.Text, _
                                        cControl.GetType))
          End If

          If TypeOf (cControl) Is Label Or TypeOf (cControl) Is LinkLabel Or _
                      TypeOf (cControl) Is CheckBox Or TypeOf (cControl) Is Button Or _
                      TypeOf (cControl) Is TabPage Or TypeOf (cControl) Is GroupBox Or _
                      TypeOf (cControl) Is Panel Or _
                      TypeOf (cControl) Is TabControl Or _
                      TypeOf (cControl) Is StatusBar Or _
                      TypeOf (cControl) Is StatusStrip Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.XtraScrollableControl Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.GroupControl Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.PanelControl Or _
                      TypeOf (cControl) Is DevExpress.XtraTab.XtraTabPage Or _
                      TypeOf (cControl) Is DevExpress.XtraTab.XtraTabControl Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.CheckEdit Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.PopupContainerControl Or _
                      TypeOf (cControl) Is DevExpress.XtraEditors.SimpleButton Or _
                      TypeOf (cControl) Is System.Windows.Forms.RadioButton Or _
                      TypeOf (cControl) Is DevExpress.XtraBars.Ribbon.RibbonStatusBar Or _
                      TypeOf (cControl) Is DevExpress.XtraBars.Ribbon.RibbonControl Then

            'TypeOf (cControl) Is DevExpress.XtraBars.Ribbon.RibbonBarManager Or _
            Dim strOrgControlBez As String = cControl.Text.Trim
            Dim strChangedControlBez As String = cControl.Text.Trim


            If TypeOf (cControl) Is StatusStrip Then
              Dim ts As ToolStrip = CType(cControl, ToolStrip)
              For Each itm As ToolStripItem In ts.Items
                itm.Text = _ClsProgSetting.TranslateText(itm.Text)
                'itm.Text = TranslateSelectedText(itm.Text)
                Trace.WriteLine(String.Format("1. ChildControlName: {0} Text: ", itm.Text))
              Next

            Else
              If cControl.HasChildren Then
                For Each ctrl In cControl.Controls
                  GetChildChildBez(CType(ctrl, Control))
                Next

              Else
                strQuery = "//Control[@Name=" & Chr(34) & strOrgControlBez & Chr(34) & "]/CtlLabel" & strUSLang
                strBez = GetXMLValueByQuery(_ClsProgSetting.GetFormDataFile, strQuery, strChangedControlBez)
                cControl.Text = _ClsProgSetting.TranslateText(strBez)

              End If
            End If

          Else
            If cControl.HasChildren Then
              Trace.WriteLine(String.Format("1. AussenControl (WithChild): Name: {0} | bez: {1} | Typeof: {2}", _
                                          cControl.Name, cControl.Text, _
                                          cControl.GetType))
              For Each ctrl_1 As Control In cControl.Controls
                GetChildChildBez(CType(ctrl_1, Control))
              Next
            Else
              Trace.WriteLine(String.Format("1. AussenControl (NoChild): Name: {0} | bez: {1} | Typeof: {2}", _
                                          cControl.Name, cControl.Text, _
                                          cControl.GetType))
            End If
          End If
        Next

      End With

    Catch ex As Exception
      Dim strMessage As String = "Ein Fehler ist während der Ausführung des Programms aufgetreten. {0}"
      MessageBox.Show(String.Format(_ClsProgSetting.TranslateText(strMessage), _
                                      ex.Message, strMethodeName), strMethodeName)
    End Try

  End Sub

End Class
