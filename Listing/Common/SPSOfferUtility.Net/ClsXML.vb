
Imports System.Xml.XPath
Imports SP.Infrastructure.Logging

Public Class ClsXML

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Dim _ClsReg As New SPProgUtility.ClsDivReg
	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Dim _ClsLog As New SPProgUtility.ClsEventLog

	Dim strUSLang As String = _ClsProgSetting.GetUSLanguage()
	Dim xmlDoc As New Xml.XmlDocument()
	Dim strLogFilename As String = _ClsProgSetting.GetSpSTempPath & "DeinFile.txt"

	Sub GetChildChildBez(ByVal cControl As Control)
		Dim strBez As String = String.Empty
		Dim strQuery As String = String.Empty
		Dim strMainNode As String = "//Form_Normaly"

		Dim xpNav As XPathNavigator
		Dim xni As XPathNodeIterator
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Try
			'If cControl.Name = "LibZRes1" Then
			'  Trace.WriteLine(String.Format("Func_2: name: {0}, bez: {1} / Typeof: {2}", _
			'                              cControl.Name, cControl.Text, _
			'                              cControl.GetType))
			'End If
			xpNav = xmlDoc.CreateNavigator()
			If TypeOf (cControl) Is Label Or TypeOf (cControl) Is LinkLabel Or TypeOf (cControl) Is Form Or
									TypeOf (cControl) Is CheckBox Or TypeOf (cControl) Is Button Or
									TypeOf (cControl) Is TabPage Or TypeOf (cControl) Is GroupBox Or
									TypeOf (cControl) Is Panel Or
									TypeOf (cControl) Is TabControl Or TypeOf (cControl) Is TabPage Or
									TypeOf (cControl) Is Button Or
									TypeOf (cControl) Is StatusBar Or
									TypeOf (cControl) Is StatusStrip Or
									TypeOf (cControl) Is DevExpress.XtraEditors.LabelControl Or
									TypeOf (cControl) Is DevExpress.XtraEditors.PanelControl Or
									TypeOf (cControl) Is DevExpress.XtraTab.XtraTabPage Or
									TypeOf (cControl) Is DevExpress.XtraTab.XtraTabControl Or
									TypeOf (cControl) Is DevExpress.XtraEditors.CheckEdit Or
									TypeOf (cControl) Is DevExpress.XtraEditors.SimpleButton Or
									TypeOf (cControl) Is DevExpress.XtraEditors.CheckButton Or
									TypeOf (cControl) Is DevExpress.XtraEditors.PopupContainerControl Or
									TypeOf (cControl) Is DevExpress.XtraEditors.GroupControl Or
									TypeOf (cControl) Is System.Windows.Forms.RadioButton Then
				Trace.WriteLine(String.Format("Innen: name: {0}, bez: {1} / Typeof: {2}",
															cControl.Name, cControl.Text,
															cControl.GetType))

				If TypeOf (cControl) Is LinkLabel Then cControl.TabStop = False
				If TypeOf (cControl) Is DevExpress.XtraBars.Controls.DockedBarControl Or TypeOf (cControl) Is DevExpress.XtraBars.BarDockControl Then
					Trace.WriteLine(cControl.Name)
				End If

				Dim strOrgControlBez As String = cControl.Text.Trim
				Dim strChangedControlBez As String = cControl.Text.Trim
				If strOrgControlBez = String.Empty Then
					Trace.WriteLine("leere Text")
				End If
				If TypeOf (cControl) Is StatusStrip Then
					Dim ts As ToolStrip = CType(cControl, ToolStrip)
					For Each itm As ToolStripItem In ts.Items
						If itm.Text <> String.Empty Then
							itm.Text = _ClsProgSetting.TranslateText(itm.Text)
							Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
						End If
					Next

				Else
					If strOrgControlBez <> String.Empty Then

						strQuery = "//Control[@Name=" & Chr(34) & strOrgControlBez & Chr(34) & "]/CtlLabel" & strUSLang
						xni = xpNav.Select(strQuery)
						Do While xni.MoveNext()
							strBez = xni.Current.Value
							strChangedControlBez = strBez
						Loop
						cControl.Invoke(Sub()
															cControl.Text = _ClsProgSetting.TranslateText(strChangedControlBez)
														End Sub)
					End If

					If cControl.HasChildren Then
						For Each ctrl_1 As Control In cControl.Controls
							GetChildChildBez(CType(ctrl_1, Control))
						Next
					End If

				End If
			Else
				Trace.WriteLine(String.Format("Aussen: name: {0}, bez: {1} / Typeof: {2}",
																			cControl.Name, cControl.Text,
																			cControl.GetType))

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub


End Class
