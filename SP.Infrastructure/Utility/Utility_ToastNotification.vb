
Imports System.Windows.Forms
Imports DevExpress.Utils

Namespace UI

	Partial Class UtilityUI

		''' <summary>
		''' Shows an error message.
		''' </summary>
		''' <param name="text">The text to show.</param>
		Public Sub ShowToastNotification(ByVal text As String)
			If Not String.IsNullOrWhiteSpace(text) Then
				Dim defaultTooltipController As ToolTipController = DevExpress.Utils.ToolTipController.DefaultController
				Dim args As ToolTipControllerShowEventArgs = defaultTooltipController.CreateShowArgs()

				''Dim sTooltip As SuperToolTip = Nothing
				''Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPropose))
				''Dim resImage As Image = CType(resources.GetObject("resource.Vorschlag 32.png"), System.Drawing.Image)
				''Dim titleItem1 As ToolTipTitleItem = New ToolTipTitleItem
				''args.Text = m_Translate.GetSafeTranslationValue("Daten speichern")

				''Dim item1 As ToolTipItem = New ToolTipItem
				''item1.Image = resImage
				''item1.AllowHtmlText = DefaultBoolean.True
				''args.Contents.Text = m_Translate.GetSafeTranslationValue("Die Daten wurden gespeichert.")
				''sTooltip.Items.Clear()
				''sTooltip.Items.Add(item1)
				''txt_Bezeichnung.SuperTip = sTooltip


				args.ToolTip = text
				args.IconType = ToolTipIconType.Information
				args.ImageIndex = -1
				args.IconSize = ToolTipIconSize.Small
				defaultTooltipController.ShowHint(args)

			End If

		End Sub

	End Class


End Namespace
