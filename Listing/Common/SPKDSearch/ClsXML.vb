
Imports System.Data.SqlClient

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports System.IO

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

    Try
      If ClsDataDetail.TranslationData.ContainsKey(strPersonalizedItem) Then
        Return ClsDataDetail.TranslationData.Item(strPersonalizedItem).LogedUserLanguage

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
        If ClsDataDetail.ProsonalizedData.ContainsKey(dicKey) Then
          strPersonalizedItem = ClsDataDetail.ProsonalizedData.Item(dicKey).CaptionValue

        Else
          strPersonalizedItem = strPersonalizedItem

        End If
      End If

      If ClsDataDetail.TranslationData.ContainsKey(strPersonalizedItem) Then
        Return ClsDataDetail.TranslationData.Item(strPersonalizedItem).LogedUserLanguage

      Else
        Return strPersonalizedItem

      End If

    Catch ex As Exception
      Return strPersonalizedItem
    End Try

  End Function

  Function GetColorValue() As String
    Dim strModul2print As String = String.Empty
    Dim m_md As New Mandant
    Dim m_utility As New utilities

    Dim strQuery As String = "//All_FieldsColor/Gruppenüberschrift"

    Dim strBez As String = m_utility.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(ClsDataDetail.ProgSettingData.SelectedMDNr), strQuery)
    If strBez <> String.Empty Then
      strModul2print = strBez

    Else
      strBez = "128;0;0"

    End If

    Return strBez
  End Function


End Class


