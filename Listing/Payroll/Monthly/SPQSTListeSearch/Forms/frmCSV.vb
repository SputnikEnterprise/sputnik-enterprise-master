
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Utility
Imports System.Globalization

Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects

Imports System.IO
Imports DevExpress.XtraGrid
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Common
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.Export
Imports DevExpress.XtraPrinting
Imports DevExpress.Utils
Imports DevExpress.Export.Xl
Imports DevExpress.Printing.ExportHelpers
Imports DevExpress.Office.Crypto
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Internal.Automations.BaseTable
Imports System.ComponentModel

Public Class frmCSV
	Inherits DevExpress.XtraEditors.XtraForm

#Region "private consts"

	Private Const MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING As String = "MD_{0}/Sonstiges"

#End Region


	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_ConnectionString As String

	''' <summary>
	''' The data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess


	Private m_Mandant As Mandant
	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_QSTCantonData As QSTCantonMasterData

	Private m_docName As String
	Private m_moduleName As String
	Private m_mdNr As Integer
	Private m_canton As String
	Private m_qstArt As String
	Private m_emptyDeclartion As Boolean

	Private m_AccountNumber As String = String.Empty  ' 1501495 ' TODO: Wird vom Amt geliefert  (SWAG Kundennummer hartcodiert)
	Private m_kandidatenNummerQST As Integer = 0 ' TODO: Wird vom Amt geliefert 
	Private m_periodeDatumVon As DateTime = ClsDataDetail.SelPeriodeVon
	Private m_periodeDatumBis As DateTime = ClsDataDetail.SelPeriodeBis

	Private m_sql As String

	Private m_BaseTableUtil As SPSBaseTables

	Private m_MandantXMLFile As String
	Private m_SonstigesSetting As String
	Private m_MandantSettingsXml As SPProgUtility.CommonXmlUtility.SettingsXml

	Private m_EmployeeMasterData As EmployeeMasterData
	Private m_CommunityData As BindingList(Of SP.Internal.Automations.CommunityData)


#Region "public properties"

	Public Property CallerModulName As CallerModulNum
	Public Property AssignedSQLQuery As String
	Public Property Canton As String
	Public Property ListArt As String
	Public Property EmptyDeclaration As Boolean

	Public Property LoadedQSTData As IEnumerable(Of FoundedData)

	Public Enum CallerModulNum
		XMLGENEVE
		XMLVAUD
		EQUEST
	End Enum

#End Region

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_Mandant = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI


		m_ConnectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)

		m_MandantXMLFile = m_Mandant.GetSelectedMDDataXMLFilename(m_InitializationData.MDData.MDNr, Now.Year)
		m_SonstigesSetting = String.Format(MANDANT_XML_SETTING_SPUTNIK_SONSTIGES_SETTING, m_InitializationData.MDData.MDNr)
		m_MandantSettingsXml = New SPProgUtility.CommonXmlUtility.SettingsXml(m_MandantXMLFile)
		m_BaseTableUtil = New SPSBaseTables(m_InitializationData)


		InitializeComponent()

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("FormStyle: {0}", ex.ToString))
		End Try

		If String.IsNullOrWhiteSpace(My.Settings.Filename4CSV) Then
			My.Settings.Filename4VAUD = My.Settings.Filename4CSV
			My.Settings.Save()
		End If

		Reset()
		TranslateControls()

		'If Not LoadFQSTCantonData() Then Return

		AddHandler gveQuestValidData.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler gveQuestInvalidData.RowCellClick, AddressOf Ongv_RowCellClick

	End Sub

	Private ReadOnly Property LoadUIDNumber As String

		Get
			Dim UIDNumber As String = m_MandantSettingsXml.GetSettingByKey(String.Format("{0}/UIDNumber", m_SonstigesSetting))

			Return UIDNumber
		End Get

	End Property

	Public Function LoadExporterData() As Boolean
		Dim result As Boolean = True

		m_sql = AssignedSQLQuery
		m_qstArt = ListArt
		m_canton = Canton
		m_emptyDeclartion = EmptyDeclaration
		m_mdNr = m_InitializationData.MDData.MDNr

		If Not LoadFQSTCantonData() Then Return False
		m_AccountNumber = String.Format("{0}-000", LoadUIDNumber)
		xtabMain.ShowTabHeader = DefaultBoolean.False
		bbiPrintlist.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

		Select Case CallerModulName

			Case CallerModulNum.XMLGENEVE, CallerModulNum.XMLVAUD
				Me.Height = 400
				Me.Width = 700

				LoadXMLData()
				xtabMain.SelectedTabPage = xtabExportGenf

			Case CallerModulNum.EQUEST
				bbiPrintlist.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
				Me.Height = 1000
				Me.Width = 900

				ResetForeQuest()
				xtabMain.SelectedTabPage = xtabExporteQuest

			Case Else
				Return False

		End Select


		Return result

	End Function

	Private Sub Reset()

		bbiPossibleValues.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
		lbl_ListTypeGe.Visible = False
		lueXMLArt.Visible = False
		lbLstlInfo.Visible = False

		ResetXMLListArtDropDown()

	End Sub

	Private Sub ResetXMLListArtDropDown()

		lueXMLArt.Enabled = True

		lueXMLArt.Properties.DisplayMember = "DisplayLable"
		lueXMLArt.Properties.ValueMember = "Value"

		Dim columns = lueXMLArt.Properties.Columns
		columns.Clear()
		columns.Add(New LookUpColumnInfo("DisplayLable", 0))

		lueXMLArt.Properties.ShowHeader = False
		lueXMLArt.Properties.ShowFooter = False
		lueXMLArt.Properties.DropDownRows = 10
		lueXMLArt.Properties.BestFitMode = BestFitMode.BestFitResizePopup
		lueXMLArt.Properties.SearchMode = SearchMode.AutoComplete
		lueXMLArt.Properties.AutoSearchColumnIndex = 0

		lueXMLArt.Properties.NullText = String.Empty

		lueXMLArt.EditValue = Nothing

	End Sub

	''' <summary>
	'''  Trannslate controls.
	''' </summary>
	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		btnClose.Text = m_Translate.GetSafeTranslationValue(btnClose.Text)

		xtabExportGenf.Text = m_Translate.GetSafeTranslationValue(xtabExportGenf.Text)
		lblGEDateiname.Text = m_Translate.GetSafeTranslationValue(lblGEDateiname.Text)
		lbl_ListTypeGe.Text = m_Translate.GetSafeTranslationValue(lbl_ListTypeGe.Text)
		lbLstlInfo.Text = m_Translate.GetSafeTranslationValue(lbLstlInfo.Text)

		xtabExporteQuest.Text = m_Translate.GetSafeTranslationValue(xtabExporteQuest.Text)
		lbleQuestPfad.Text = m_Translate.GetSafeTranslationValue(lbleQuestPfad.Text)

		bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)
		bbiExport.Caption = m_Translate.GetSafeTranslationValue(bbiExport.Caption)
		bbiPossibleValues.Caption = m_Translate.GetSafeTranslationValue(bbiPossibleValues.Caption)

	End Sub


	Private Function LoadXMLData() As Boolean
		Dim result As Boolean = True
		Dim fileNamePart1 As String = String.Empty


		Select Case CallerModulName

			Case CallerModulNum.XMLGENEVE
				LoadXMLDeklarationnArt()
				txt_GEFilename.EditValue = My.Settings.Filename4XML

				Dim listTypeGeValue = lueXMLArt.EditValue
				fileNamePart1 = String.Format("LR{0}", listTypeGeValue)

				Dim xmlFileName = String.Format("{0}_{1}_{2}_{3:00}{4:00}_{5:00}{6:00}.XML",
															fileNamePart1,
															m_AccountNumber,
															m_periodeDatumVon.Year,
															m_periodeDatumVon.Day,
															m_periodeDatumVon.Month,
															m_periodeDatumBis.Day,
															m_periodeDatumBis.Month)
				Dim myFile As FileInfo = New FileInfo(Me.txt_GEFilename.Text)
				txt_GEFilename.EditValue = Path.Combine(myFile.DirectoryName, xmlFileName)

				lbl_ListTypeGe.Visible = True
				lueXMLArt.Visible = True
				lbLstlInfo.Visible = True
				bbiPossibleValues.Visibility = DevExpress.XtraBars.BarItemVisibility.Always

			Case CallerModulNum.XMLVAUD
				txt_GEFilename.EditValue = My.Settings.Filename4VAUD
				If String.IsNullOrWhiteSpace(txt_GEFilename.EditValue) Then
					fileNamePart1 = If(m_qstArt.ToLower.Contains("korrektur"), "LC", "LR")

					Dim xmlFileName = String.Format("{0}_{1}_{2}_{3:00}{4:00}_{5:00}{6:00}.XML",
														fileNamePart1,
														m_AccountNumber,
														m_periodeDatumVon.Year,
														m_periodeDatumVon.Day,
														m_periodeDatumVon.Month,
														m_periodeDatumBis.Day,
														m_periodeDatumBis.Month)
					Dim myFile As FileInfo = New FileInfo(If(String.IsNullOrWhiteSpace(txt_GEFilename.EditValue), m_InitializationData.UserData.SPTempPath, txt_GEFilename.EditValue))
					txt_GEFilename.EditValue = Path.Combine(myFile.DirectoryName, xmlFileName)

				End If


			Case Else
				Return False


		End Select


		Return result

	End Function


	Private Function LoadFQSTCantonData() As Boolean
		Dim success As Boolean = True

		m_QSTCantonData = m_ListingDatabaseAccess.LoadQSTCantonMasterData(New PayrollListingSearchData With {.MDNr = m_InitializationData.MDData.MDNr, .Canton = m_canton})
		If m_QSTCantonData Is Nothing Then
			Dim msg As String = "Quellensteuer Daten für Kanton {1} konnten nicht geladen werden.{0}Bitte versuchen Sie die Daten in Mandantenverwaltung > Steuerverwaltungen zu erfassen."
			msg = String.Format(m_Translate.GetSafeTranslationValue(msg), vbNewLine, m_canton)

			m_UtilityUi.ShowErrorDialog(msg)

			Return False
		End If


		Return Not m_QSTCantonData Is Nothing

	End Function

	Private Function LoadXMLDeklarationnArt() As Boolean
		Dim result As Boolean = True

		Dim dataList = New List(Of ComboboxValue)
		dataList.Add(New ComboboxValue With {.DisplayLable = "Typ 1", .Value = "1"})
		dataList.Add(New ComboboxValue With {.DisplayLable = "Typ 2", .Value = "2"})
		dataList.Add(New ComboboxValue With {.DisplayLable = "Typ 3", .Value = "3"})
		dataList.Add(New ComboboxValue With {.DisplayLable = "Typ 4", .Value = "4"})
		dataList.Add(New ComboboxValue With {.DisplayLable = "Typ 5", .Value = "5"})
		dataList.Add(New ComboboxValue With {.DisplayLable = "Typ 6", .Value = "6"})


		lueXMLArt.Properties.DataSource = dataList
		lueXMLArt.EditValue = 1

		Return result

	End Function

	Private Sub OnbbiExport_itemClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bbiExport.ItemClick

		Try

			Select Case CallerModulName

				Case CallerModulNum.XMLGENEVE
					LoadDataForGeneva()

				Case CallerModulNum.XMLVAUD
					LoadDataForVaud()

				Case CallerModulNum.EQUEST
					LoadDataForeQuest()


				Case Else
					Return

			End Select

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)

			Return

		End Try

	End Sub

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
		Me.Close()
		Me.Dispose()
	End Sub

	Private Sub OnbbiPossibleValues_itemClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bbiPossibleValues.ItemClick
		Dim frm = New frmGEPossibleValues

		frm.Show()
		frm.LoadFoundedCommunityList()
		frm.BringToFront()

	End Sub

	Private Function LoadDataForGeneva() As Boolean
		Dim success As Boolean = True
		Dim myDirName As String = m_InitializationData.UserData.spAllowedPath

		If Not txt_GEFilename.Text.ToLower.Contains(":\") Then
			txt_GEFilename.Text = Path.Combine(myDirName, "QSTListe.XML")

		ElseIf Not txt_GEFilename.Text.ToUpper.EndsWith(".xml".ToUpper) Then
			txt_GEFilename.Text &= ".XML"

		End If

		Try
			Dim result As Boolean
			Dim extendedResult As ClsExtendedResult = Nothing

			Dim fileNamePart1 As String = String.Format("LR{0}", lueXMLArt.EditValue)
			Dim xmlFileName = String.Format("{0}_{1}_{2}_{3:00}{4:00}_{5:00}{6:00}.XML",
															fileNamePart1,
															m_AccountNumber,
															m_periodeDatumVon.Year,
															m_periodeDatumVon.Day,
															m_periodeDatumVon.Month,
															m_periodeDatumBis.Day,
															m_periodeDatumBis.Month)
			Dim myFile As FileInfo = New FileInfo(Me.txt_GEFilename.Text)
			If myFile.Name.ToUpper <> xmlFileName.ToUpper Then txt_GEFilename.EditValue = Path.Combine(myFile.DirectoryName, xmlFileName)


			Dim periodDateFrom As Date = m_periodeDatumVon
			Dim periodDateTo As Date = m_periodeDatumBis.AddMonths(1).AddDays(-1) ' letzter Tag vom Monat
			extendedResult = ExportQstXmlGe(m_mdNr, periodDateFrom, periodDateTo, lueXMLArt.EditValue, txt_GEFilename.EditValue)

			' TODO: Erweitertes Resultat bei allen Exportfunktionen setzen
			If extendedResult Is Nothing Then
				extendedResult = New ClsExtendedResult()
				If (result = False) Then extendedResult.AddError(m_Translate.GetSafeTranslationValue("Allgemeiner Fehler"))
			End If

			If extendedResult.HasErrorOrWarning Then
				Dim extendedResultMessage As String = String.Join(vbNewLine, extendedResult.ResultList)
				Dim msg As String = String.Format(m_Translate.GetSafeTranslationValue("Beim Erstellen der Datei {0} sind Fehler oder Warnungen aufgetreten."), txt_GEFilename.EditValue)
				msg &= String.Format("{0}{0}{1}", vbNewLine, extendedResultMessage)

				m_UtilityUi.ShowErrorDialog(msg)

				success = False
			Else
				Dim msg As String = String.Format(TranslateMyText("Die Datei {0} wurde erfolgreich gespeichert."), txt_GEFilename.EditValue)
				m_UtilityUi.ShowOKDialog(msg)

				Dim strDirectory As String = Path.GetDirectoryName(txt_GEFilename.EditValue)
				Process.Start(strDirectory)

			End If
			My.Settings.Filename4XML = Me.txt_GEFilename.Text
			My.Settings.Save()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(String.Format("{0}", ex.ToString))

			Return False

		End Try


		Return success

	End Function

	Private Function LoadDataForVaud() As Boolean
		Dim success As Boolean = True
		Dim myDirName As String = m_InitializationData.UserData.spAllowedPath

		If Not txt_GEFilename.Text.ToLower.Contains(":\") Then
			txt_GEFilename.EditValue = Path.Combine(myDirName, "QSTListe.XML")

		ElseIf Not txt_GEFilename.Text.ToUpper.EndsWith(".xml".ToUpper) Then
			txt_GEFilename.EditValue &= ".XML"

		End If

		Try
			Dim result As Boolean
			Dim extendedResult As ClsExtendedResult = Nothing

			Dim fileNamePart1 As String = If(m_qstArt.ToLower.Contains("korrektur"), "LC", "LR")
			Dim xmlFileName = String.Format("{0}_{1}_{2}_{3:00}{4:00}_{5:00}{6:00}.XML",
														fileNamePart1,
														m_AccountNumber,
														m_periodeDatumVon.Year,
														m_periodeDatumVon.Day,
														m_periodeDatumVon.Month,
														m_periodeDatumBis.Day,
														m_periodeDatumBis.Month)
			Dim myFile As FileInfo = New FileInfo(txt_GEFilename.EditValue)
			If myFile.Name.ToUpper <> xmlFileName.ToUpper Then txt_GEFilename.EditValue = Path.Combine(myFile.DirectoryName, xmlFileName)


			result = ExportQstXmlVd(m_qstArt, m_emptyDeclartion, txt_GEFilename.EditValue)

			' TODO: Erweitertes Resultat bei allen Exportfunktionen setzen
			If extendedResult Is Nothing Then
				extendedResult = New ClsExtendedResult()
				If (result = False) Then extendedResult.AddError(m_Translate.GetSafeTranslationValue("Allgemeiner Fehler"))
			End If

			If extendedResult.HasErrorOrWarning Then
				Dim extendedResultMessage As String = String.Join(vbNewLine, extendedResult.ResultList)
				Dim msg As String = String.Format(m_Translate.GetSafeTranslationValue("Beim Erstellen der Datei {0} sind Fehler oder Warnungen aufgetreten."), txt_GEFilename.EditValue)
				msg &= String.Format("{0}{0}{1}", vbNewLine, extendedResultMessage)

				m_UtilityUi.ShowErrorDialog(msg)
				success = False
			Else
				Dim msg As String = String.Format(TranslateMyText("Die Datei {0} wurde erfolgreich gespeichert."), txt_GEFilename.EditValue)
				m_UtilityUi.ShowOKDialog(msg)

				Dim strDirectory As String = Path.GetDirectoryName(txt_GEFilename.EditValue)
				System.Diagnostics.Process.Start(strDirectory)
			End If
			My.Settings.Filename4VAUD = txt_GEFilename.EditValue
			My.Settings.Save()


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(String.Format("{0}", ex.ToString))

			Return False

		End Try


		Return success

	End Function


#Region "#AfterAddRowEvent"
	Private Sub options_AfterAddRow(ByVal e As AfterAddRowEventArgs)
		' Merge cells in rows that correspond to the grid's group rows.
		If e.DataSourceRowIndex < 0 Then
			e.ExportContext.MergeCells(New XlCellRange(New XlCellPosition(0, e.DocumentRow - 1), New XlCellPosition(5, e.DocumentRow - 1)))
		End If
	End Sub
#End Region ' #AfterAddRowEvent

#Region "#CustomizeCellEvent"
	' Specify the value alignment for Discontinued field.
	Private aligmentForDiscontinuedColumn As New XlCellAlignment() With {.HorizontalAlignment = XlHorizontalAlignment.Center, .VerticalAlignment = XlVerticalAlignment.Center}

	Private Sub options_CustomizeCell(ByVal e As CustomizeCellEventArgs)
		' Substitute Boolean values within the Discontinued column by special symbols.
		If e.ColumnFieldName = "Discontinued" Then
			If TypeOf e.Value Is Boolean Then
				e.Handled = True
				e.Formatting.Alignment = aligmentForDiscontinuedColumn
				e.Value = If(CBool(e.Value), "☑", "☐")
			End If
		End If
	End Sub
#End Region ' #CustomizeCellEvent

#Region "#CustomizeSheetHeaderEvent"
	Private Delegate Sub AddCells(ByVal e As ContextEventArgs, ByVal formatFirstCell As XlFormattingObject, ByVal formatSecondCell As XlFormattingObject)

	Private methods As Dictionary(Of Integer, AddCells) = CreateMethodSet()

	Private Shared Function CreateMethodSet() As Dictionary(Of Integer, AddCells)
		Dim dictionary = New Dictionary(Of Integer, AddCells)()
		dictionary.Add(9, AddressOf AddAddressRow)
		dictionary.Add(10, AddressOf AddAddressLocationCityRow)
		dictionary.Add(11, AddressOf AddPhoneRow)
		dictionary.Add(12, AddressOf AddFaxRow)
		dictionary.Add(13, AddressOf AddEmailRow)
		Return dictionary
	End Function
	Private Sub options_CustomizeSheetHeader(ByVal e As ContextEventArgs)
		' Specify cell formatting. 
		Dim formatFirstCell = CreateXlFormattingObject(True, 24)
		Dim formatSecondCell = CreateXlFormattingObject(True, 18)
		' Add new rows displaying custom information. 
		For i = 0 To 14
			Dim addCellMethod As AddCells = Nothing
			If methods.TryGetValue(i, addCellMethod) Then
				addCellMethod(e, formatFirstCell, formatSecondCell)
			Else
				e.ExportContext.AddRow()
			End If
		Next i
		' Merge specific cells.
		MergeCells(e)
		' Add an image to the top of the document.
		Dim file = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Resources.1.jpg")
		If file IsNot Nothing Then
			Dim imageToHeader = New Bitmap(Image.FromStream(file))
			Dim imageToHeaderRange = New XlCellRange(New XlCellPosition(0, 0), New XlCellPosition(5, 7))
			e.ExportContext.MergeCells(imageToHeaderRange)
			e.ExportContext.InsertImage(imageToHeader, imageToHeaderRange)
		End If
		e.ExportContext.MergeCells(New XlCellRange(New XlCellPosition(0, 8), New XlCellPosition(5, 8)))
	End Sub

	Private Shared Sub AddEmailRow(ByVal e As ContextEventArgs, ByVal formatFirstCell As XlFormattingObject, ByVal formatSecondCell As XlFormattingObject)
		Dim emailCellName = CreateCell("Email :", formatFirstCell)
		Dim emailCellLocation = CreateCell("corpsales@devav.com", formatSecondCell)
		emailCellLocation.Hyperlink = "corpsales@devav.com"
		e.ExportContext.AddRow({emailCellName, Nothing, emailCellLocation})
	End Sub
	Private Shared Sub AddFaxRow(ByVal e As ContextEventArgs, ByVal formatFirstCell As XlFormattingObject, ByVal formatSecondCell As XlFormattingObject)
		Dim faxCellName = CreateCell("Fax :", formatFirstCell)
		Dim faxCellLocation = CreateCell("+ 1 (213) 555-1824", formatSecondCell)
		e.ExportContext.AddRow({faxCellName, Nothing, faxCellLocation})
	End Sub
	Private Shared Sub AddPhoneRow(ByVal e As ContextEventArgs, ByVal formatFirstCell As XlFormattingObject, ByVal formatSecondCell As XlFormattingObject)
		Dim phoneCellName = CreateCell("Phone :", formatFirstCell)
		Dim phoneCellLocation = CreateCell("+ 1 (213) 555-2828", formatSecondCell)
		e.ExportContext.AddRow({phoneCellName, Nothing, phoneCellLocation})
	End Sub
	Private Shared Sub AddAddressLocationCityRow(ByVal e As ContextEventArgs, ByVal formatFirstCell As XlFormattingObject, ByVal formatSecondCell As XlFormattingObject)
		Dim AddressLocationCityCell = CreateCell("Los Angeles CA 90731 USA", formatSecondCell)
		e.ExportContext.AddRow({Nothing, Nothing, AddressLocationCityCell})
	End Sub
	Private Shared Sub AddAddressRow(ByVal e As ContextEventArgs, ByVal formatFirstCell As XlFormattingObject, ByVal formatSecondCell As XlFormattingObject)
		Dim AddressCellName = CreateCell("Address: ", formatFirstCell)
		Dim AddresssCellLocation = CreateCell("807 West Paseo Del Mar", formatSecondCell)
		e.ExportContext.AddRow({AddressCellName, Nothing, AddresssCellLocation})
	End Sub

	' Create a new cell with a specified value and format settings.
	Private Shared Function CreateCell(ByVal value As Object, ByVal formatCell As XlFormattingObject) As CellObject
		Return New CellObject With {.Value = value, .Formatting = formatCell}
	End Function

	' Merge specific cells.
	Private Shared Sub MergeCells(ByVal e As ContextEventArgs)
		MergeCells(e, 2, 9, 5, 9)
		MergeCells(e, 0, 9, 1, 10)
		MergeCells(e, 2, 10, 5, 10)
		MergeCells(e, 0, 11, 1, 11)
		MergeCells(e, 2, 11, 5, 11)
		MergeCells(e, 0, 12, 1, 12)
		MergeCells(e, 2, 12, 5, 12)
		MergeCells(e, 0, 13, 1, 13)
		MergeCells(e, 2, 13, 5, 13)
		MergeCells(e, 0, 14, 5, 14)
	End Sub
	Private Shared Sub MergeCells(ByVal e As ContextEventArgs, ByVal left As Integer, ByVal top As Integer, ByVal right As Integer, ByVal bottom As Integer)
		e.ExportContext.MergeCells(New XlCellRange(New XlCellPosition(left, top), New XlCellPosition(right, bottom)))
	End Sub

	' Specify a cell's alignment and font settings. 
	Private Shared Function CreateXlFormattingObject(ByVal bold As Boolean, ByVal size As Double) As XlFormattingObject
		Dim cellFormat = New XlFormattingObject With {
				.Font = New XlCellFont With {.Bold = bold, .Size = size},
				.Alignment = New XlCellAlignment With {.RelativeIndent = 10, .HorizontalAlignment = XlHorizontalAlignment.Center, .VerticalAlignment = XlVerticalAlignment.Center}
			}
		Return cellFormat
	End Function
#End Region ' #CustomizeSheetHeaderEvent

#Region "#CustomizeSheetFooterEvent"
	Private Sub options_CustomizeSheetFooter(ByVal e As ContextEventArgs)
		' Add an empty row to the document's footer.
		e.ExportContext.AddRow()

		' Create a new row.
		Dim firstRow = New CellObject()
		' Specify row values.
		firstRow.Value = "The report is generated from the NorthWind database."
		' Specify the cell content alignment and font settings.
		Dim rowFormatting = CreateXlFormattingObject(True, 18)
		rowFormatting.Alignment.HorizontalAlignment = XlHorizontalAlignment.Left
		firstRow.Formatting = rowFormatting
		' Add the created row to the output document. 
		e.ExportContext.AddRow({firstRow})

		' Create one more row.
		Dim secondRow = New CellObject()
		' Specify the row value. 
		secondRow.Value = "The addresses and phone numbers are fictitious."
		' Change the row's font settings.
		rowFormatting.Font.Size = 14
		rowFormatting.Font.Bold = False
		rowFormatting.Font.Italic = True
		secondRow.Formatting = rowFormatting
		' Add this row to the output document.
		e.ExportContext.AddRow({secondRow})
	End Sub
#End Region ' #CustomizeSheetFooterEvent

#Region "#CustomizeSheetSettingsEvent"
	Private Sub options_CustomizeSheetSettings(ByVal e As CustomizeSheetEventArgs)
		' Anchor the output document's header to the top and set its fixed height. 
		Const lastHeaderRowIndex As Integer = 15
		e.ExportContext.SetFixedHeader(lastHeaderRowIndex)
		' Add the AutoFilter button to the document's cells corresponding to the grid column headers.
		e.ExportContext.AddAutoFilter(New XlCellRange(New XlCellPosition(0, lastHeaderRowIndex), New XlCellPosition(5, 100)))
	End Sub
#End Region ' #CustomizeSheetSettingsEvent


#Region "Helper methodes"

	Private Function LoadEmployeeData(ByVal employeeNumber As Integer) As Boolean

		m_EmployeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(employeeNumber, False)
		If m_EmployeeDatabaseAccess Is Nothing Then
			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Stammdaten konnten nicht geladen werden."))

			Return False
		End If

		Return Not m_EmployeeMasterData Is Nothing
	End Function


	Private Sub Ontxt_GEFilename_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_GEFilename.ButtonClick
		Dim fldlgList As New FolderBrowserDialog

		With fldlgList
			.Description = TranslateMyText("Bitte wählen Sie ein Verzeichnis für Export der Datei.")
			.SelectedPath = Me.txt_GEFilename.Text

			.ShowNewFolderButton = True
			If .ShowDialog() = DialogResult.OK Then
				Dim filePath As String = .SelectedPath
				If Not filePath.EndsWith("\") Then
					filePath = String.Concat(filePath, "\")
				End If

				Me.txt_GEFilename.Text = String.Format("{0}{1}", filePath, m_docName)
			End If

		End With

	End Sub

	Private Sub Ontxt_eQuestFilename_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_eQuestFilename.ButtonClick
		Dim fldlgList As New FolderBrowserDialog

		With fldlgList
			.Description = TranslateMyText("Bitte wählen Sie ein Verzeichnis für Export der Datei.")
			.SelectedPath = Me.txt_GEFilename.Text

			.ShowNewFolderButton = True
			If .ShowDialog() = DialogResult.OK Then
				Dim filePath As String = .SelectedPath
				If Not filePath.EndsWith("\") Then
					filePath = String.Concat(filePath, "\")
				End If

				Me.txt_eQuestFilename.Text = String.Format("{0}", filePath)
			End If

		End With

	End Sub

	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintlist.ItemClick

		If xtabMain.SelectedTabPage Is xtabExporteQuest Then

			If tpeQuestData.SelectedPage Is tnpInvalidData Then
				If gveQuestInvalidData.RowCount > 0 Then
					grdeQuestInvalidData.ShowPrintPreview()
				End If

			Else
				If gveQuestValidData.RowCount > 0 Then
					grdeQuestValidData.ShowPrintPreview()
				End If

			End If

		End If

	End Sub


#End Region


#Region "Helper classes"

	Class ComboboxValue
		Public Property DisplayLable As String
		Public Property Value As String
	End Class

#End Region


End Class