
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.Utils.VisualEffects


Namespace UI


	Partial Class UtilityUI


		Private badge As DevExpress.Utils.VisualEffects.Badge

		Public Sub ShowStandardBadgeNotification(ByVal targetControl As Control, ByVal bdg As Badge, ByVal adornerMgr As AdornerUIManager, ByVal body As String,
										ByVal location As ContentAlignment, ByVal targetRegion As TargetElementRegion, ByVal paintStyle As BadgePaintStyle)

			adornerMgr.Elements.Remove(bdg)
			adornerMgr.BeginUpdate()

			bdg = New DevExpress.Utils.VisualEffects.Badge()
			bdg.TargetElement = targetControl ' grpvakanz
			bdg.TargetElementRegion = targetRegion ' TargetElementRegion.Header
			bdg.Visible = True
			bdg.Properties.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True ' GetDefaultBoolean(cbAllowGlyphSkinning.Checked)
			bdg.Properties.AllowImage = DevExpress.Utils.DefaultBoolean.True ' GetDefaultBoolean(cbAllowImage.Checked)
			bdg.Properties.StretchImage = DevExpress.Utils.DefaultBoolean.True ' GetDefaultBoolean(cbStretchImage.Checked)
			bdg.Properties.Location = location ' CType(ieLocation.EditValue, ContentAlignment)
			bdg.Properties.Text = String.Format("{0} ", body) ' "Der Datensatz wurde dupliziert"
			SetBadgeOffset()
			bdg.Properties.TextMargin = New System.Windows.Forms.Padding(3)
			bdg.Properties.ImageStretchMargins = New System.Windows.Forms.Padding(14)
			bdg.Properties.AllowHtmDrawText = DevExpress.Utils.DefaultBoolean.True
			bdg.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			bdg.Properties.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True
			'bdg.Properties.Offset = New Point(0 + 0, 0)

			bdg.Properties.PaintStyle = paintStyle


			adornerMgr.Elements.Add(bdg)
			adornerMgr.Show()
			adornerMgr.EndUpdate()

		End Sub

		Public Sub ShowCustomBadgeNotification(ByVal targetControl As Control, ByVal bdg As Badge, ByVal adornerMgr As AdornerUIManager, ByVal body As String,
											   ByVal textMargin As System.Windows.Forms.Padding, ByVal imageMargin As System.Windows.Forms.Padding,
											   ByVal location As ContentAlignment, ByVal targetRegion As TargetElementRegion, ByVal paintStyle As BadgePaintStyle,
											   ByVal imgBackColor As Integer?, ByVal textColor As Integer?)

			adornerMgr.Elements.Remove(bdg)
			adornerMgr.BeginUpdate()

			bdg = New DevExpress.Utils.VisualEffects.Badge()
			bdg.TargetElement = targetControl
			bdg.TargetElementRegion = targetRegion ' TargetElementRegion.Header
			bdg.Visible = True
			bdg.Properties.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True ' GetDefaultBoolean(cbAllowGlyphSkinning.Checked)
			bdg.Properties.AllowImage = DevExpress.Utils.DefaultBoolean.True ' GetDefaultBoolean(cbAllowImage.Checked)
			bdg.Properties.StretchImage = DevExpress.Utils.DefaultBoolean.True ' GetDefaultBoolean(cbStretchImage.Checked)
			bdg.Properties.Location = location ' CType(ieLocation.EditValue, ContentAlignment)
			bdg.Properties.Text = String.Format(" {0} ", body) ' "Der Datensatz wurde dupliziert"
			SetBadgeOffset()
			bdg.Properties.TextMargin = textMargin 'new System.Windows.Forms.Padding(3)
			bdg.Properties.ImageStretchMargins = imageMargin ' New System.Windows.Forms.Padding(14)
			bdg.Properties.AllowHtmDrawText = DevExpress.Utils.DefaultBoolean.True
			bdg.Properties.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True

			If imgBackColor Is Nothing OrElse textColor Is Nothing Then
				bdg.Properties.PaintStyle = paintStyle
			Else
				bdg.Appearance.BackColor = Color.FromArgb(imgBackColor)
				bdg.Appearance.ForeColor = Color.FromArgb(textColor)
			End If


			adornerMgr.Elements.Add(bdg)
			adornerMgr.Show()
			adornerMgr.EndUpdate()

		End Sub

		Private Sub SetBadgeOffset()
			If badge IsNot Nothing Then
				badge.Properties.Offset = New Point(Convert.ToInt32(0), Convert.ToInt32(0))
			End If
		End Sub




	End Class


End Namespace
