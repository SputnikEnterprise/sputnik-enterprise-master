
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Public Class SuportedCodes


#Region "private consts"

	Private Const DATE_COLUMN_NAME As String = "magebdat;date;es_ab;es_ende;datum;fakdate;birthdate;aus_dat;paidon;krediton;reminder_1date;reminder_2date;reminder_3date;buchungdate;checkedon;avamreportingdate;avamfrom"
	Private Const DATE_COLUMN_NAME_LONG As String = "createdon;datum;printdate;checkedon;avamreportingdate;avamuntil;changedon"
	Private Const INTEGER_COLUMN_NAME As String = "id;number;vacancynumber;ofnr;vaknr;fopnr;manr;kdnr;zhdnr;esnr;rpnr;zgnr;lonr;renr;vgnr;monat;jahr;jobchannelpriority;OstJobWillbeExpireSoon;JobsCHWillbeExpireSoon;kdznr"
	Private Const DECIMAL_COLUMN_NAME As String = "betragex;bezahlt;betragink;betragmwst;betragtotal;zebetrag;lobetrag;betrag;aus_dat"
	Private Const CHECKBOX_COLUMN_NAME As String = "actives;offen?;rpdone;isout;isaslo;transferedwos;jchisonline;ojisonline;AVAMReportingObligation;AVAMIsNowOnline"

#End Region


#Region "private Fields"

	Private Shared m_Logger As ILogger = New Logger()

	Private m_UtilityUI As UtilityUI
	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_common As CommonSetting
	Private m_path As ClsProgPath
	Private m_translate As TranslateValues

	Private m_DatabaseAccess As DataBaseAccess.MainGrid

	Private m_ModulName As String
	Private m_MainGriddata As GridData

	Private m_DATE_COLUMN_NAME As String
	Private m_DATE_COLUMN_NAME_LONG As String
	Private m_INTEGER_COLUMN_NAME As String
	Private m_DECIMAL_COLUMN_NAME As String
	Private m_CHECKBOX_COLUMN_NAME As String

	Private m_CHECK_EDIT_NOT_ALLOWED As String
	Private m_CHECK_EDIT_WARNING As String
	Private m_CHECK_EDIT_COMPLETED As String
	Private m_CHECK_EDIT_EXPIREE As String

	Private m_CheckEditCompleted As RepositoryItemCheckEdit
	Private m_CheckEditExpire As RepositoryItemCheckEdit
	Private m_CheckEditWarning As RepositoryItemCheckEdit
	Private m_CheckEditNotAllowed As RepositoryItemCheckEdit


#End Region

#Region "Contructor"

	Public Sub New()

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUI = New UtilityUI
		m_common = New CommonSetting
		m_path = New ClsProgPath
		m_translate = New TranslateValues

		m_DatabaseAccess = New DataBaseAccess.MainGrid

	End Sub

#End Region

#Region "Public Properties"

	Public Property ChangeColumnNamesToLowercase As Boolean

	Public ReadOnly Property LoadMainGridData() As GridData
		Get
			Return m_MainGriddata
		End Get
	End Property

#End Region

	Public Function LoadFoundedMDList() As IEnumerable(Of MDData)
		Dim result As List(Of MDData) = Nothing

		Dim listOfFoundedData = m_DatabaseAccess.LoadAktivMandanten

		If listOfFoundedData Is Nothing OrElse listOfFoundedData.Count = 0 Then
			ShowErrDetail(m_translate.GetSafeTranslationValue("Keine Datenstätze wurden gefunden."))

			Return Nothing
		End If

		Dim foundedGridData = (From person In listOfFoundedData
							   Select New MDData With
								   {.MDNr = person.MDNr,
								   .MDName = person.MDName
								   }).ToList()

		Dim listDataSource As BindingList(Of MDData) = New BindingList(Of MDData)
		For Each p In foundedGridData
			listDataSource.Add(p)
		Next

		Return listOfFoundedData
	End Function

	Public Sub LoadMainModulGridPropertiesFromXML(ByVal mdNr As Integer, ByVal userNr As Integer, ByVal modulName As String)

		m_ModulName = modulName
		LoadColumnDataTypesFromXML(mdNr)

		m_MainGriddata = LoadGridPropertiesFromXML(ModulConstants.MDData.MDNr)

	End Sub

	Public Function ResetMainGrid(ByVal gvMain As GridView, ByVal grdMain As GridControl, ByVal modulName As String) As Boolean
		Dim result As Boolean = True

		If m_MainGriddata Is Nothing Then Return False
		m_ModulName = modulName
		gvMain.OptionsLayout.Columns.StoreAllOptions = False

		m_CheckEditCompleted = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
		m_CheckEditCompleted.PictureChecked = My.Resources.bullet_ball_green
		m_CheckEditCompleted.PictureUnchecked = My.Resources.bullet_ball_red
		m_CheckEditCompleted.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

		m_CheckEditWarning = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
		m_CheckEditWarning.PictureChecked = My.Resources.warning_16x16
		m_CheckEditWarning.PictureUnchecked = Nothing
		m_CheckEditWarning.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

		m_CheckEditNotAllowed = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
		m_CheckEditNotAllowed.PictureChecked = My.Resources.protectsheet_16x16 ' ImageCollection1.Images(2)
		m_CheckEditNotAllowed.PictureUnchecked = Nothing
		m_CheckEditNotAllowed.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

		m_CheckEditExpire = CType(grdMain.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
		m_CheckEditExpire.PictureChecked = My.Resources.bullet_ball_yellow
		m_CheckEditExpire.PictureUnchecked = Nothing
		m_CheckEditExpire.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

		gvMain.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvMain.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvMain.OptionsView.ShowGroupPanel = False
		gvMain.OptionsView.ShowIndicator = False
		gvMain.OptionsView.ShowAutoFilterRow = True

		gvMain.Columns.Clear()

		Try
			Dim repoHTML = New RepositoryItemHypertextLabel
			repoHTML.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.WordWrap = DevExpress.Utils.DefaultBoolean.True
			repoHTML.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top
			repoHTML.Appearance.Options.UseTextOptions = True
			grdMain.RepositoryItems.Add(repoHTML)

			Dim reproCheckbox = New RepositoryItemCheckEdit
			reproCheckbox.Appearance.Options.UseTextOptions = True
			grdMain.RepositoryItems.Add(reproCheckbox)

			Dim AutofilterconditionNumber = ModulConstants.MDData.AutoFilterConditionNumber
			Dim AutofilterconditionDate = ModulConstants.MDData.AutoFilterConditionDate

			For Each itm In m_MainGriddata.MainGridColumnData ' aColCaption.Length - 1

				Dim column As New DevExpress.XtraGrid.Columns.GridColumn()
				Dim columnCaption As String = m_translate.GetSafeTranslationValue(itm.GridColCaption.Trim)
				Dim columnName As String = itm.GridColFieldName.Trim

				If ChangeColumnNamesToLowercase Then columnName = columnName.ToLower
				If columnCaption.ToLower = "res" Then Continue For


				column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
				column.Caption = columnCaption
				column.Name = columnName
				column.FieldName = columnName

				Select Case itm.ColumnDataTye
					Case EnunColumnDataType.DATE_COLUMN_NAME
						column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
						column.DisplayFormat.FormatString = "d"
						column.OptionsFilter.AutoFilterCondition = AutofilterconditionDate

					Case EnunColumnDataType.DATE_COLUMN_NAME_LONG
						column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
						column.DisplayFormat.FormatString = "g"
						column.OptionsFilter.AutoFilterCondition = AutofilterconditionDate

					Case EnunColumnDataType.INTEGER_COLUMN_NAME
						If columnName.ToLower = "JobsCHWillbeExpireSoon".ToLower OrElse columnName.ToLower = "OstJobWillbeExpireSoon".ToLower OrElse column.FieldName.ToLower.Contains("jobchannelpriority".ToLower) Then
							column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
						Else
							column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
						End If

						column.AppearanceHeader.Options.UseTextOptions = True
						column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
						column.DisplayFormat.FormatString = "F0"
						column.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber

					Case EnunColumnDataType.DECIMAL_COLUMN_NAME
						column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
						column.AppearanceHeader.Options.UseTextOptions = True
						column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
						column.DisplayFormat.FormatString = "N2"
						column.OptionsFilter.AutoFilterCondition = AutofilterconditionNumber

					Case EnunColumnDataType.CHECKBOX_COLUMN_NAME
						column.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
						column.ColumnEdit = reproCheckbox
						column.AppearanceHeader.Options.UseTextOptions = True


					Case EnunColumnDataType.CHECK_EDIT_NOT_ALLOWED
						column.ColumnEdit = m_CheckEditNotAllowed

					Case EnunColumnDataType.CHECK_EDIT_WARNING
						column.ColumnEdit = m_CheckEditWarning
					Case EnunColumnDataType.CHECK_EDIT_COMPLETED
						column.ColumnEdit = m_CheckEditCompleted
					Case EnunColumnDataType.CHECK_EDIT_EXPIRE
						column.ColumnEdit = m_CheckEditExpire


					Case EnunColumnDataType.COMMONTYPE
						column.ColumnEdit = repoHTML
						column.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways

					Case Else


				End Select
				column.Visible = itm.ShowColumn
				column.MinWidth = 0

				Trace.WriteLine(String.Format("fieldName: {0} ({1})>>> ColumnDataTye: {2} | ShowColumn: {3}", itm.GridColFieldName, itm.GridColCaption, itm.ColumnDataTye, itm.ShowColumn))

				gvMain.Columns.Add(column)
			Next


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try
		grdMain.DataSource = Nothing

		Return result
	End Function

	Public Function ResetPropertyGrid(ByVal gvTopGrid As GridView, ByVal grdTopGrid As GridControl) As Boolean
		Dim result As Boolean = True
		If m_MainGriddata Is Nothing Then Return False

		gvTopGrid.Columns.Clear()
		Try
			For Each itm In m_MainGriddata.TopPropertyColumnData

				Dim column As New DevExpress.XtraGrid.Columns.GridColumn()
				column.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
				column.Caption = m_translate.GetSafeTranslationValue(itm.ColumnCaption.Trim)
				column.Name = itm.ColumnName.ToLower
				column.FieldName = itm.ColumnName.Trim.ToLower
				column.Visible = True

				gvTopGrid.Columns.Add(column)

			Next


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = False
		End Try

		grdTopGrid.DataSource = Nothing

		Return result
	End Function


	Private Sub LoadColumnDataTypesFromXML(ByVal iMDNr As Integer)
		If iMDNr = 0 Then Return

		Try
			Dim xDoc As XDocument = XDocument.Load(m_md.GetSelectedMDMainViewXMLFilename(iMDNr))
			Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("ColumnDataType")
							   Select New With {
																					.DATE_COLUMN_NAME = m_utility.GetSafeStringFromXElement(exportSetting.Element("DATE_COLUMN_NAME")),
																					.DATE_COLUMN_NAME_LONG = m_utility.GetSafeStringFromXElement(exportSetting.Element("DATE_COLUMN_NAME_LONG")),
																					.DECIMAL_COLUMN_NAME = m_utility.GetSafeStringFromXElement(exportSetting.Element("DECIMAL_COLUMN_NAME")),
																					.INTEGER_COLUMN_NAME = m_utility.GetSafeStringFromXElement(exportSetting.Element("INTEGER_COLUMN_NAME")),
																					.CHECKBOX_COLUMN_NAME = m_utility.GetSafeStringFromXElement(exportSetting.Element("CHECKBOX_COLUMN_NAME")),
																					.CHECK_EDIT_NOT_ALLOWED = m_utility.GetSafeStringFromXElement(exportSetting.Element("CHECK_EDIT_NOT_ALLOWED")),
																					.CHECK_EDIT_WARNING = m_utility.GetSafeStringFromXElement(exportSetting.Element("CHECK_EDIT_WARNING")),
																					.CHECK_EDIT_COMPLETED = m_utility.GetSafeStringFromXElement(exportSetting.Element("CHECK_EDIT_COMPLETED")),
																					.CHECK_EDIT_EXPIRE = m_utility.GetSafeStringFromXElement(exportSetting.Element("CHECK_EDIT_EXPIRE"))
																						}).FirstOrDefault()


			m_INTEGER_COLUMN_NAME = ConfigQuery.INTEGER_COLUMN_NAME.ToLower
			m_DATE_COLUMN_NAME = ConfigQuery.DATE_COLUMN_NAME.tolower
			m_DATE_COLUMN_NAME_LONG = ConfigQuery.DATE_COLUMN_NAME_LONG.tolower
			m_DECIMAL_COLUMN_NAME = ConfigQuery.DECIMAL_COLUMN_NAME.tolower
			m_CHECKBOX_COLUMN_NAME = ConfigQuery.CHECKBOX_COLUMN_NAME.tolower

			m_CHECK_EDIT_NOT_ALLOWED = ConfigQuery.CHECK_EDIT_NOT_ALLOWED.tolower
			m_CHECK_EDIT_WARNING = ConfigQuery.CHECK_EDIT_WARNING.tolower
			m_CHECK_EDIT_COMPLETED = ConfigQuery.CHECK_EDIT_COMPLETED.tolower
			m_CHECK_EDIT_EXPIREE = ConfigQuery.CHECK_EDIT_EXPIRE.tolower


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Private Function LoadGridPropertiesFromXML(ByVal iMDNr As Integer) As GridData
		Dim result As New GridData
		If iMDNr = 0 Then Return Nothing

		Try
			Dim xDoc As XDocument = XDocument.Load(m_md.GetSelectedMDMainViewXMLFilename(iMDNr))
			Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("Modul")
							   Where Not (exportSetting.Attribute("ID") Is Nothing) _
																And exportSetting.Attribute("ID").Value = m_ModulName
							   Select New With {
																					.SQLQuery = m_utility.GetSafeStringFromXElement(exportSetting.Element("SQL")),
																					.GridColFieldName = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColFieldName")),
																					.DisplayMember = m_utility.GetSafeStringFromXElement(exportSetting.Element("DisplayMember")),
																					.GridColCaption = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColCaption")),
																					.GridColWidth = m_utility.GetSafeStringFromXElement(exportSetting.Element("GridColWidth")),
																					.BackColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("BackColor")),
																					.ForeColor = m_utility.GetSafeStringFromXElement(exportSetting.Element("ForeColor")),
																					.PopupFields = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldName")),
																					.PopupCaptions = m_utility.GetSafeStringFromXElement(exportSetting.Element("PopupFieldCaption")),
																					.CountOfFieldsInHeader = m_utility.GetSafeStringFromXElement(exportSetting.Element("CountOfFieldsInHeaderToShow")),
																					.FieldsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("FieldsInHeaderToShow")),
																					.CaptionsInHeaderToShow = m_utility.GetSafeStringFromXElement(exportSetting.Element("CaptionInHeaderToShow"))
																						}).FirstOrDefault()


			If ConfigQuery Is Nothing AndAlso m_ModulName = "Kandidatenverwaltung" Then
				m_ModulName = "f6c163f6-3dab-4db8-b8dd-a7cd19b7017c"

				Return LoadGridPropertiesFromXML(iMDNr)

			End If

			result.SQLQuery = ConfigQuery.SQLQuery
			result.DisplayMember = ConfigQuery.DisplayMember
			result.BackColor = ConfigQuery.BackColor
			result.ForeColor = ConfigQuery.ForeColor

			result.CountOfFieldsInHeader = CShort(ConfigQuery.CountOfFieldsInHeader)
			result.IsUserProperty = False


			result.GridColFieldName = ConfigQuery.GridColFieldName.ToString.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "")
			result.GridColCaption = ConfigQuery.GridColCaption.ToString.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "")
			result.GridColWidth = ConfigQuery.GridColWidth.ToString.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "")

			result.PopupFields = ConfigQuery.PopupFields.ToString.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "")
			result.PopupCaptions = ConfigQuery.PopupCaptions.ToString.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "")
			result.FieldsInHeaderToShow = ConfigQuery.FieldsInHeaderToShow.ToString.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "")
			result.CaptionsInHeaderToShow = ConfigQuery.CaptionsInHeaderToShow.ToString.Replace(vbNewLine, "").Replace(vbLf, "").Replace(vbTab, "").Replace("&", "")


			Dim mainGridcolumns As New List(Of CridDataColumnProperties)
			Dim popupGridcolumns As New List(Of PopupCridColumnData)
			Dim topGridcolumns As New List(Of PopupCridColumnData)

			Dim fieldNames As List(Of String) = result.GridColFieldName.ToString.Split(";").ToList
			For Each colItm In fieldNames
				Dim data = New CridDataColumnProperties
				Dim value As String = colItm.Trim

				data.ColumnDataTye = EnunColumnDataType.COMMONTYPE
				If m_DATE_COLUMN_NAME.Split(";").ToList.Contains(value.ToLower) Then
					data.ColumnDataTye = EnunColumnDataType.DATE_COLUMN_NAME

				ElseIf m_DATE_COLUMN_NAME_LONG.Split(";").ToList.Contains(value.ToLower) Then
					data.ColumnDataTye = EnunColumnDataType.DATE_COLUMN_NAME_LONG

				ElseIf m_INTEGER_COLUMN_NAME.Split(";").ToList.Contains(value.ToLower) Then
					data.ColumnDataTye = EnunColumnDataType.INTEGER_COLUMN_NAME

				ElseIf m_DECIMAL_COLUMN_NAME.Split(";").ToList.Contains(value.ToLower) Then
					data.ColumnDataTye = EnunColumnDataType.DECIMAL_COLUMN_NAME

				ElseIf m_CHECKBOX_COLUMN_NAME.Split(";").ToList.Contains(value.ToLower) Then
					data.ColumnDataTye = EnunColumnDataType.CHECKBOX_COLUMN_NAME


				ElseIf m_CHECK_EDIT_NOT_ALLOWED.Split(";").ToList.Contains(value.ToLower) Then
					data.ColumnDataTye = EnunColumnDataType.CHECK_EDIT_NOT_ALLOWED

				ElseIf m_CHECK_EDIT_WARNING.Split(";").ToList.Contains(value.ToLower) AndAlso Not value.ToLower.Contains("AVAMIsNowOnline".ToLower) Then
					data.ColumnDataTye = EnunColumnDataType.CHECK_EDIT_WARNING

				ElseIf m_CHECK_EDIT_COMPLETED.Split(";").ToList.Contains(value.ToLower) AndAlso Not value.ToLower.Contains("AVAMIsNowOnline".ToLower) Then
					data.ColumnDataTye = EnunColumnDataType.CHECK_EDIT_COMPLETED

				ElseIf m_CHECK_EDIT_EXPIREE.Split(";").ToList.Contains(value.ToLower) Then
					data.ColumnDataTye = EnunColumnDataType.CHECK_EDIT_EXPIRE

				End If


				data.GridColFieldName = value

				mainGridcolumns.Add(data)
			Next

			Dim columnCaptions As List(Of String) = result.GridColCaption.ToString.Split(";").ToList
			Dim i As Integer = 0
			For Each colCaption In columnCaptions
				Dim data = New CridDataColumnProperties
				Dim value As String = colCaption.Trim

				data.GridColCaption = value


				mainGridcolumns(i).GridColCaption = value

				Trace.WriteLine(String.Format("fieldName: {0} ({1})>>> ColumnDataTye: {2} ", mainGridcolumns(i).GridColFieldName, value, mainGridcolumns(i).ColumnDataTye))

				i += 1
				If i > mainGridcolumns.Count - 1 Then Exit For
			Next

			Dim columnWidth As List(Of String) = result.GridColWidth.ToString.Split(";").ToList
			i = 0
			For Each colCaption In columnWidth
				Dim data = New CridDataColumnProperties
				Dim value As String = colCaption.Trim

				mainGridcolumns(i).GridColWidth = Val(value)
				mainGridcolumns(i).ShowColumn = Val(value) > 0

				i += 1
				If i > mainGridcolumns.Count - 1 Then Exit For
			Next


			fieldNames = result.PopupFields.ToString.Split(";").ToList
			For Each colItm In fieldNames
				Dim data = New PopupCridColumnData
				Dim value As String = colItm.Trim

				data.ColumnName = value

				popupGridcolumns.Add(data)
			Next

			i = 0
			fieldNames = result.PopupCaptions.ToString.Split(";").ToList
			For Each colItm In fieldNames
				Dim data = New PopupCridColumnData
				Dim value As String = colItm.Trim

				data.ColumnCaption = value

				popupGridcolumns(i).ColumnCaption = value
				i += 1
				If i > popupGridcolumns.Count - 1 Then Exit For
			Next

			fieldNames = result.FieldsInHeaderToShow.ToString.Split(";").ToList
			For Each colItm In fieldNames
				Dim data = New PopupCridColumnData
				Dim value As String = colItm.Trim

				data.ColumnName = value

				topGridcolumns.Add(data)
			Next

			i = 0
			fieldNames = result.CaptionsInHeaderToShow.ToString.Split(";").ToList
			For Each colItm In fieldNames
				Dim data = New PopupCridColumnData
				Dim value As String = colItm.Trim

				data.ColumnCaption = value

				topGridcolumns(i).ColumnCaption = value
				i += 1
				If i > topGridcolumns.Count - 1 Then Exit For
			Next


			result.PopupColumnData = popupGridcolumns
			result.TopPropertyColumnData = topGridcolumns
			result.MainGridColumnData = mainGridcolumns



		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Return result
	End Function


End Class
