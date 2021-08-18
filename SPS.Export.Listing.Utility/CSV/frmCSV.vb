
Imports System.Reflection.Assembly
Imports System.IO
Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports System.Windows.Forms

Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging

Imports System.Drawing
Imports System.Xml
Imports System.Threading
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SPProgUtility.Mandanten
Imports DevExpress.LookAndFeel
Imports SP.DatabaseAccess.Listing.DataObjects
Imports System.Reflection
Imports DevExpress.XtraSplashScreen
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Listing

Public Class frmCSV
	Inherits DevExpress.XtraEditors.XtraForm


#Region "private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_UtilityUI As New UtilityUI
	Private m_md As Mandant

	Private m_ConnectionString As String

	''' <summary>
	''' The data access object.
	''' </summary>
	Private m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _docname As String

#End Region

	Private Property SQL2Select() As String
	Private Property SQLFields() As String


#Region "public property"

	Public Enum EnumModul
		Lohnkonti
		SEARCHRESULTTAXDATA
	End Enum

	Public Property CusotmerHourData As List(Of SP.DatabaseAccess.Listing.DataObjects.CustomertReportHoursData)
	Public Property EmployeeHourData As List(Of SP.DatabaseAccess.Listing.DataObjects.EmployeeReportHoursData)
	Public Property LohnKontiData As IEnumerable(Of ListingPayrollLohnkontiData)

	Public Property ExportSetting As ClsCSVSettings
	Public Property ExportModul As EnumModul

#End Region


#Region "Contructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_ConnectionString = m_InitializationData.MDData.MDDbConn
		m_md = New Mandant

		m_ConnectionString = m_InitializationData.MDData.MDDbConn
		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		m_EmployeeDatabaseAccess = New EmployeeDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_ConnectionString, m_InitializationData.UserData.UserLanguage)

		InitializeComponent()
		ChkIgnorEmptyRecords.Enabled = True
		chkRecordsAsHeader.Enabled = False

		Me.KeyPreview = True
		Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If Not String.IsNullOrWhiteSpace(strStyleName) Then UserLookAndFeel.Default.SetSkinStyle(strStyleName)

		TranslateControls()

	End Sub


#End Region

	Protected Function ExistsColumnInDb(ByRef reader As IDataReader, ByVal columnName As String) As Boolean
		'Trace.WriteLine(String.Format("Gesucht wird nach: {0}", columnName.ToUpper))

		For i As Integer = 0 To reader.FieldCount - 1
			'Trace.WriteLine(String.Format("{0} | {1}", reader.GetName(i).ToUpper, columnName.ToUpper))
			If reader.GetName(i).ToUpper.Equals(columnName.ToUpper) Then
				Return True
			End If
		Next

		Return False
	End Function

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Close()
		Me.Dispose()
	End Sub

	Private Sub txtFilename_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_Filename.ButtonClick
		ShowFolderBrowser()
	End Sub

	Sub ShowFolderBrowser()
		Dim dialog As New FolderBrowserDialog()

		dialog.Description = m_Translate.GetSafeTranslationValue("Bitte wählen Sie ein Verzeichnis für Export der Datei aus:")
		dialog.ShowNewFolderButton = True
		dialog.SelectedPath = If(Me.txt_Filename.Text = String.Empty, _ClsProgSetting.GetSpSFiles2DeletePath, Path.GetDirectoryName(Me.txt_Filename.Text))

		If dialog.ShowDialog() = DialogResult.OK Then
			Dim a As Action = Function() InlineAssignHelper(Me.txt_Filename.Text, String.Format("{0}\", dialog.SelectedPath))
			Me.Invoke(a)
		End If

	End Sub

	Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
		target = value

		Return value
	End Function



	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim myDirName As String = _ClsProgSetting.GetSpSFiles2DeletePath

		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		If Me.txt_Filename.Text = String.Empty Then
			Me.txt_Filename.Text = myDirName & "Exportliste.csv"

		Else
			If Not Me.txt_Filename.Text.ToUpper.EndsWith(".csv".ToUpper) And Not Me.txt_Filename.Text.ToUpper.EndsWith(".txt".ToUpper) Then
				Me.txt_Filename.Text &= ".csv"
			End If

			Try
				IO.File.WriteAllLines(Me.txt_Filename.Text, New String() {""}, System.Text.Encoding.Default)

			Catch ex As Exception
				txt_Filename.EditValue = Path.Combine(myDirName, Path.GetRandomFileName & ".csv")

			End Try


			'Dim MyFile As FileInfo = New FileInfo(Me.txt_Filename.Text)

		End If

		Try
			Dim result As Boolean = True

			If Not CusotmerHourData Is Nothing Then
				result = result AndAlso ExportCustomerHourDataToCSV(CusotmerHourData)

			ElseIf Not EmployeeHourData Is Nothing Then
				result = result AndAlso ExportEmployeehourDataToCSV(EmployeeHourData)

			ElseIf Not LohnKontiData Is Nothing Then
				If chkRecordsAsHeader.Checked Then
					result = result AndAlso ExportLohnkontiDataToCSV(LohnKontiData)
				Else
					result = result AndAlso ExportLohnkontiDataAsStandardToCSV(LohnKontiData)
				End If

			ElseIf ExportModul = EnumModul.SEARCHRESULTTAXDATA Then
				result = result AndAlso ExportQSTDataToCSV()


			Else
				result = result AndAlso ExportDataToCSV(Me.SQL2Select)

			End If

			Dim msg As String = String.Empty
			SplashScreenManager.CloseForm(False)

			If result Then
				msg = String.Format(m_Translate.GetSafeTranslationValue("Die Datei {0} wurde erfolgreich gespeichert."), Me.txt_Filename.Text)
				m_UtilityUI.ShowInfoDialog(msg)

				Dim strDirectory As String = Path.GetDirectoryName(Me.txt_Filename.Text)
				Process.Start("explorer.exe", "/select," & txt_Filename.EditValue)

				Me.ExportSetting.ExportFileName = Me.txt_Filename.Text

				' Einstellungen speichern...
				result = result AndAlso SaveCSVSetting()

			Else
				msg = String.Format(m_Translate.GetSafeTranslationValue("Die Datei {0} konnte nicht erstellt werden.{1}{2}"), Me.txt_Filename.Text, vbNewLine, result)
				m_UtilityUI.ShowErrorDialog(msg)

			End If


		Catch ex As Exception
			SplashScreenManager.CloseForm(False)
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UtilityUI.ShowErrorDialog(ex.ToString)


		Finally
			SplashScreenManager.CloseForm(False)

		End Try

	End Sub

	Function ExportDataToCSV(ByVal strTempSQL As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim result As Boolean = True

		Try
			Dim conn As SqlConnection = New SqlConnection(m_ConnectionString)
			Dim cmd As SqlCommand = New SqlCommand(strTempSQL, conn)
			Dim trennzeichen As String = Me.cbo_Trennzeichen.Text
			Dim darstellungszeichen As String = Me.cbo_Darstellungszeichen.Text
			If trennzeichen.Trim.Length = 0 Then trennzeichen = ";"


			conn.Open()
			Dim reader As SqlDataReader = cmd.ExecuteReader()
			Dim strHeaderlist As String = Me.ExportSetting.SQLFields
			If strHeaderlist.Trim.Length = 0 Then
				Throw New Exception(m_Translate.GetSafeTranslationValue("Keine Felder wurden ausgewählt. Bitte definieren Sie die Felder in <<EINSTELLUNGEN>>."))
			End If

			Dim headerList As New List(Of String)
			strHeaderlist = String.Format(strHeaderlist, If(trennzeichen = "{Tab}", vbTab, trennzeichen))
			If trennzeichen = "{Tab}" Then
				headerList = strHeaderlist.Split(CChar(vbTab)).ToList()
			Else
				headerList = strHeaderlist.Split(CChar(trennzeichen)).ToList()
			End If

			Dim bIsFieldValid As Boolean = True
			Dim header As String = strHeaderlist

			Dim zeile As String = String.Empty
			Dim FileContentList As New List(Of String)

			Dim headerListToImport As New List(Of String)
			Dim LineList As New List(Of String)
			Dim lineNumber As Integer = 1
			Dim ValueExists As Boolean = False

			While reader.Read()
				If lineNumber = 1 Then
					For i As Integer = 0 To headerList.Count - 1
						If ExistsColumnInDb(reader, headerList(i).Trim) Then
							headerListToImport.Add(headerList(i).Trim)
						End If
					Next i
				End If
				lineNumber += 1

				For i As Integer = 0 To headerListToImport.Count - 1

					Dim strValue As String = If(String.IsNullOrEmpty(reader(headerListToImport(i).Trim).ToString), String.Empty, reader(headerListToImport(i).Trim).ToString)
					If TypeOf (reader(headerListToImport(i).Trim)) Is System.DateTime Then
						strValue = String.Format(reader(headerListToImport(i).Trim), "G").ToString
					ElseIf TypeOf (reader(headerListToImport(i).Trim)) Is System.Decimal Then
						strValue = String.Format(reader(headerListToImport(i).Trim), "g").ToString
					End If
					If Not ValueExists Then ValueExists = Not String.IsNullOrWhiteSpace(strValue)
					zeile += String.Format("{0}{1}{0}{2}", darstellungszeichen, strValue, If(trennzeichen = "{Tab}", vbTab, trennzeichen))

				Next

				If Not ChkIgnorEmptyRecords.Checked Then ValueExists = True
				If ValueExists Then
					zeile = zeile.Substring(0, zeile.Length - 1)
					LineList.Add(zeile)
				End If

				zeile = String.Empty
				ValueExists = False
			End While
			FileContentList = LineList
			header = String.Join(If(trennzeichen = "{Tab}", vbTab, trennzeichen), headerListToImport.ToArray())
			FileContentList.Insert(0, header)

			IO.File.WriteAllLines(Me.txt_Filename.Text, FileContentList.ToArray, System.Text.Encoding.Default)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.ToString))
			result = False

		Finally

		End Try

		Return result
	End Function

	Private Function ExportQSTDataToCSV() As Boolean
		Dim result As Boolean = True
		Dim mdNumber As Integer = m_InitializationData.MDData.MDNr

		Try
			Dim data = m_ListingDatabaseAccess.LoadSearchResultOfTaxData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)
			If data Is Nothing OrElse data.Count = 0 Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden geladen."))

				Return result
			End If

			Dim trennzeichen As String = Me.cbo_Trennzeichen.Text
			Dim darstellungszeichen As String = Me.cbo_Darstellungszeichen.Text
			If String.IsNullOrWhiteSpace(trennzeichen) Then trennzeichen = ";"

			Dim strHeaderlist As String = Me.ExportSetting.SQLFields
			If String.IsNullOrWhiteSpace(strHeaderlist) Then
				Throw New Exception(m_Translate.GetSafeTranslationValue("Keine Felder wurden ausgewählt. Bitte definieren Sie die Felder in <<EINSTELLUNGEN>>."))
			End If

			Dim headerList As New List(Of String)
			strHeaderlist = String.Format(strHeaderlist, If(trennzeichen = "{Tab}", vbTab, trennzeichen))
			If trennzeichen = "{Tab}" Then
				headerList = strHeaderlist.Split(CChar(vbTab)).ToList()
			Else
				headerList = strHeaderlist.Split(CChar(trennzeichen)).ToList()
			End If

			Dim bIsFieldValid As Boolean = True
			Dim header As String = strHeaderlist

			Dim zeile As String = String.Empty
			Dim FileContentList As New List(Of String)

			Dim headerListToImport As New List(Of String)
			Dim LineList As New List(Of String)
			Dim lineNumber As Integer = 1
			Dim ValueExists As Boolean = False

			For Each itm In data
				If lineNumber = 1 Then
					For i As Integer = 0 To headerList.Count - 1
						If Not String.IsNullOrWhiteSpace(headerList(i).Trim) Then
							headerListToImport.Add(headerList(i).Trim)
						End If
					Next i
				End If
				lineNumber += 1

				'strFieldNameByEmpty = "CustomerNumber{0}Company1{0}Company2{0}Company3{0}PostOfficeBox{0}Street{0}"
				'strFieldNameByEmpty &= "CountryCode{0}Postcode{0}Location{0}Telephone{0}Telefax{0}EMail{0}TotalHours{0}"
				Dim objValue As String
				Dim fieldExists = True

				For Each headeritm In headerListToImport
					fieldExists = True
					Select Case headeritm.ToUpper
						Case "MDNr".ToUpper
							objValue = ReplaceMissing(mdnumber, 0)
						Case "MANr".ToUpper
							objValue = ReplaceMissing(itm.MANr, 0)

						Case "EmployeeFirstname".ToUpper, "Vorname".ToUpper
							objValue = ReplaceMissing(itm.EmployeeFirstname, String.Empty)
						Case "EmployeeLastname".ToUpper, "Nachname".ToUpper
							objValue = ReplaceMissing(itm.EmployeeLastname, String.Empty)
						Case "employeename".ToUpper
							objValue = ReplaceMissing(itm.employeename, String.Empty)
						Case "employeestreet".ToUpper, "MAStrasse".ToUpper
							objValue = ReplaceMissing(itm.employeestreet, String.Empty)
						Case "employeepostcode".ToUpper, "MAPLZ".ToUpper
							objValue = ReplaceMissing(itm.employeepostcode, String.Empty)
						Case "employeecity".ToUpper, "MAOrt".ToUpper
							objValue = ReplaceMissing(itm.employeecity, String.Empty)
						Case "employeecountry".ToUpper, "MALand".ToUpper
							objValue = ReplaceMissing(itm.employeecountry, String.Empty)

						Case "vonmonat".ToUpper
							objValue = ReplaceMissing(itm.vonmonat, String.Empty)
						Case "bismonat".ToUpper
							objValue = ReplaceMissing(itm.bismonat, String.Empty)
						Case "jahr".ToUpper
							objValue = ReplaceMissing(itm.jahr, String.Empty)
						Case "gebdat".ToUpper
							objValue = ReplaceMissing(itm.gebdat, String.Empty)
						Case "ahv_nr".ToUpper
							objValue = ReplaceMissing(itm.ahv_nr, String.Empty)
						Case "ahv_nr_new".ToUpper
							objValue = ReplaceMissing(itm.ahv_nr_new, String.Empty)
						Case "s_kanton".ToUpper
							objValue = ReplaceMissing(itm.s_kanton, String.Empty)
						Case "qstgemeinde".ToUpper
							objValue = ReplaceMissing(itm.qstgemeinde, String.Empty)
						Case "TaxCommunityLabel".ToUpper
							objValue = ReplaceMissing(itm.TaxCommunityLabel, String.Empty)
						Case "TaxCommunityCode".ToUpper
							objValue = ReplaceMissing(itm.TaxCommunityCode, String.Empty)
						Case "EmploymentType".ToUpper
							objValue = ReplaceMissing(itm.EmploymentType, String.Empty)
						Case "OtherEmploymentType".ToUpper
							objValue = ReplaceMissing(itm.OtherEmploymentType, String.Empty)
						Case "TypeofStay".ToUpper
							objValue = ReplaceMissing(itm.TypeofStay, String.Empty)
						Case "ForeignCategory".ToUpper
							objValue = ReplaceMissing(itm.ForeignCategory, String.Empty)
						Case "SocialInsuranceNumber".ToUpper
							objValue = ReplaceMissing(itm.SocialInsuranceNumber, String.Empty)
						Case "CivilState".ToUpper
							objValue = ReplaceMissing(itm.CivilState, String.Empty)
						Case "NumberOfChildren".ToUpper
							objValue = ReplaceMissing(itm.NumberOfChildren, String.Empty)
						Case "TaxChurchCode".ToUpper
							objValue = ReplaceMissing(itm.TaxChurchCode, String.Empty)
						Case "PartnerLastName".ToUpper
							objValue = ReplaceMissing(itm.PartnerLastName, String.Empty)
						Case "PartnerFirstname".ToUpper
							objValue = ReplaceMissing(itm.PartnerFirstname, String.Empty)
						Case "PartnerStreet".ToUpper
							objValue = ReplaceMissing(itm.PartnerStreet, String.Empty)
						Case "PartnerPostcode".ToUpper
							objValue = ReplaceMissing(itm.PartnerPostcode, String.Empty)
						Case "PartnerCity".ToUpper
							objValue = ReplaceMissing(itm.PartnerCity, String.Empty)
						Case "PartnerCountry".ToUpper
							objValue = ReplaceMissing(itm.PartnerCountry, String.Empty)
						Case "InEmployment".ToUpper
							objValue = ReplaceMissing(itm.InEmployment, String.Empty)
						Case "EmploymentLocation".ToUpper
							objValue = ReplaceMissing(itm.EmploymentLocation, String.Empty)
						Case "EmploymentPostcode".ToUpper
							objValue = ReplaceMissing(itm.EmploymentPostcode, String.Empty)
						Case "EmploymentStreet".ToUpper
							objValue = ReplaceMissing(itm.EmploymentStreet, String.Empty)
						Case "geschlecht".ToUpper
							objValue = ReplaceMissing(itm.geschlecht, String.Empty)
						Case "employeestreet".ToUpper
							objValue = ReplaceMissing(itm.employeestreet, String.Empty)
						Case "employeepostcode".ToUpper
							objValue = ReplaceMissing(itm.employeepostcode, String.Empty)
						Case "employeecity".ToUpper
							objValue = ReplaceMissing(itm.employeecity, String.Empty)
						Case "employeecountry".ToUpper
							objValue = ReplaceMissing(itm.employeecountry, String.Empty)
						Case "monat".ToUpper
							objValue = ReplaceMissing(itm.monat, String.Empty)
						Case "kinder".ToUpper
							objValue = ReplaceMissing(itm.kinder, String.Empty)
						Case "employeelanguage".ToUpper
							objValue = ReplaceMissing(itm.employeelanguage, String.Empty)
						Case "m_anz".ToUpper
							objValue = ReplaceMissing(itm.m_anz, 0)
						Case "m_bas".ToUpper
							objValue = ReplaceMissing(itm.m_bas, 0)
						Case "m_ans".ToUpper
							objValue = ReplaceMissing(itm.m_ans, 0)
						Case "m_btr".ToUpper
							objValue = ReplaceMissing(itm.m_btr, 0)
						Case "Bruttolohn".ToUpper
							objValue = ReplaceMissing(itm.Bruttolohn, 0)
						Case "qstbasis".ToUpper
							objValue = ReplaceMissing(itm.qstbasis, 0)
						Case "stdanz".ToUpper
							objValue = ReplaceMissing(itm.stdanz, 0)

						Case "tarifcode".ToUpper
							objValue = ReplaceMissing(itm.tarifcode, String.Empty)
						Case "workeddays".ToUpper
							objValue = ReplaceMissing(itm.workeddays, 0)
						Case "WorkingHoursMonth".ToUpper
							objValue = ReplaceMissing(itm.WorkingHoursMonth, 0)
						Case "WorkingHoursWeek".ToUpper
							objValue = ReplaceMissing(itm.WorkingHoursWeek, 0)
						Case "EmploymentNumber".ToUpper
							objValue = ReplaceMissing(itm.EmploymentNumber, 0)
						Case "ESLohnNumber".ToUpper
							objValue = ReplaceMissing(itm.ESLohnNumber, 0)
						Case "ReportNumber".ToUpper
							objValue = ReplaceMissing(itm.ReportNumber, 0)
						Case "WorkingPensum".ToUpper
							objValue = ReplaceMissing(itm.WorkingPensum, "")
						Case "GAVStringInfo".ToUpper
							objValue = ReplaceMissing(itm.GAVStringInfo, "")
						Case "Dismissalreason".ToUpper
							objValue = ReplaceMissing(itm.Dismissalreason, "")

						Case "EmployeePartnerRecID".ToUpper
							objValue = String.Format("{0:n2}", ReplaceMissing(itm.EmployeePartnerRecID, 0))
						Case "EmployeeLOHistoryID".ToUpper
							objValue = String.Format("{0:n2}", ReplaceMissing(itm.EmployeeLOHistoryID, 0))
						Case "esab".ToUpper
							objValue = ReplaceMissing(itm.esab, "")
						Case "esende".ToUpper
							objValue = ReplaceMissing(itm.esende, "")

						Case Else
							objValue = String.Empty
							fieldExists = False

					End Select

					If fieldExists Then zeile += String.Format("{0}{1}{0}{2}", darstellungszeichen, objValue.ToString, If(trennzeichen = "{Tab}", vbTab, trennzeichen))

				Next

				If ChkIgnorEmptyRecords.Checked AndAlso String.IsNullOrWhiteSpace(zeile) Then ValueExists = False Else ValueExists = True
				If ValueExists Then
					zeile = zeile.Substring(0, zeile.Length - 1)
					LineList.Add(zeile)
				End If

				zeile = String.Empty
				ValueExists = False
			Next

			FileContentList = LineList
			header = String.Join(If(trennzeichen = "{Tab}", vbTab, trennzeichen), headerListToImport.ToArray())
			FileContentList.Insert(0, header)


			IO.File.WriteAllLines(Me.txt_Filename.Text, FileContentList.ToArray, System.Text.Encoding.Default)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			result = False

		Finally

		End Try

		Return result
	End Function

	''' <summary>
	''' create exportfile for customer data
	''' </summary>
	''' <param name="customerData"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Private Function ExportCustomerHourDataToCSV(ByVal customerData As List(Of SP.DatabaseAccess.Listing.DataObjects.CustomertReportHoursData)) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim result As Boolean = True

		Try
			Dim trennzeichen As String = Me.cbo_Trennzeichen.Text
			Dim darstellungszeichen As String = Me.cbo_Darstellungszeichen.Text
			If trennzeichen.Trim.Length = 0 Then trennzeichen = ";"

			Dim strHeaderlist As String = Me.ExportSetting.SQLFields
			If String.IsNullOrWhiteSpace(strHeaderlist) Then
				Throw New Exception(m_Translate.GetSafeTranslationValue("Keine Felder wurden ausgewählt. Bitte definieren Sie die Felder in <<EINSTELLUNGEN>>."))
			End If

			Dim headerList As New List(Of String)
			strHeaderlist = String.Format(strHeaderlist, If(trennzeichen = "{Tab}", vbTab, trennzeichen))
			If trennzeichen = "{Tab}" Then
				headerList = strHeaderlist.Split(CChar(vbTab)).ToList()
			Else
				headerList = strHeaderlist.Split(CChar(trennzeichen)).ToList()
			End If

			Dim bIsFieldValid As Boolean = True
			Dim header As String = strHeaderlist

			Dim zeile As String = String.Empty
			Dim FileContentList As New List(Of String)

			Dim headerListToImport As New List(Of String)
			Dim LineList As New List(Of String)
			Dim lineNumber As Integer = 1
			Dim ValueExists As Boolean = False

			For Each itm In customerData
				If lineNumber = 1 Then
					For i As Integer = 0 To headerList.Count - 1
						If Not String.IsNullOrWhiteSpace(headerList(i).Trim) Then
							headerListToImport.Add(headerList(i).Trim)
						End If
					Next i
				End If
				lineNumber += 1

				'strFieldNameByEmpty = "CustomerNumber{0}Company1{0}Company2{0}Company3{0}PostOfficeBox{0}Street{0}"
				'strFieldNameByEmpty &= "CountryCode{0}Postcode{0}Location{0}Telephone{0}Telefax{0}EMail{0}TotalHours{0}"
				Dim objValue As String
				Dim fieldExists = True
				For Each headeritm In headerListToImport
					fieldExists = True
					Select Case headeritm.ToUpper
						Case "MDNr".ToUpper
							objValue = ReplaceMissing(itm.MDNr, 0)
						Case "CustomerNumber".ToUpper
							objValue = ReplaceMissing(itm.CustomerNumber, 0)
						Case "Company1".ToUpper
							objValue = ReplaceMissing(itm.Company1, "")
						Case "Company2".ToUpper
							objValue = ReplaceMissing(itm.Company2, "")
						Case "Company3".ToUpper
							objValue = ReplaceMissing(itm.Company3, "")
						Case "PostOfficeBox".ToUpper
							objValue = ReplaceMissing(itm.PostOfficeBox, "")
						Case "Street".ToUpper
							objValue = ReplaceMissing(itm.Street, "")
						Case "CountryCode".ToUpper
							objValue = ReplaceMissing(itm.CountryCode, "")
						Case "Postcode".ToUpper
							objValue = ReplaceMissing(itm.Postcode, "")
						Case "Location".ToUpper
							objValue = ReplaceMissing(itm.Location, "")
						Case "Telephone".ToUpper
							objValue = ReplaceMissing(itm.Telephone, "")
						Case "Telefax".ToUpper
							objValue = ReplaceMissing(itm.Telefax, "")
						Case "EMail".ToUpper
							objValue = ReplaceMissing(itm.EMail, "")
						Case "TotalHours".ToUpper
							objValue = String.Format("{0:n2}", ReplaceMissing(itm.TotalHours, 0))
						Case Else
							objValue = String.Empty
							fieldExists = False
					End Select
					If fieldExists Then zeile += String.Format("{0}{1}{0}{2}", darstellungszeichen, objValue.ToString, If(trennzeichen = "{Tab}", vbTab, trennzeichen))

				Next

				If ChkIgnorEmptyRecords.Checked AndAlso String.IsNullOrWhiteSpace(zeile) Then ValueExists = False Else ValueExists = True
				If ValueExists Then
					zeile = zeile.Substring(0, zeile.Length - 1)
					LineList.Add(zeile)
				End If

				zeile = String.Empty
				ValueExists = False
			Next

			FileContentList = LineList
			header = String.Join(If(trennzeichen = "{Tab}", vbTab, trennzeichen), headerListToImport.ToArray())
			FileContentList.Insert(0, header)

			IO.File.WriteAllLines(Me.txt_Filename.Text, FileContentList.ToArray, System.Text.Encoding.Default)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.Message))
			result = False

		Finally

		End Try

		Return result
	End Function

	''' <summary>
	''' export emplyoee hour data
	''' </summary>
	Private Function ExportEmployeehourDataToCSV(ByVal employeeData As List(Of SP.DatabaseAccess.Listing.DataObjects.EmployeeReportHoursData)) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim result As Boolean = True

		Try
			Dim trennzeichen As String = Me.cbo_Trennzeichen.Text
			Dim darstellungszeichen As String = Me.cbo_Darstellungszeichen.Text
			If trennzeichen.Trim.Length = 0 Then trennzeichen = ";"

			Dim strHeaderlist As String = Me.ExportSetting.SQLFields
			If String.IsNullOrWhiteSpace(strHeaderlist) Then
				Throw New Exception(m_Translate.GetSafeTranslationValue("Keine Felder wurden ausgewählt. Bitte definieren Sie die Felder in <<EINSTELLUNGEN>>."))
			End If

			Dim headerList As New List(Of String)
			strHeaderlist = String.Format(strHeaderlist, If(trennzeichen = "{Tab}", vbTab, trennzeichen))
			If trennzeichen = "{Tab}" Then
				headerList = strHeaderlist.Split(CChar(vbTab)).ToList()
			Else
				headerList = strHeaderlist.Split(CChar(trennzeichen)).ToList()
			End If

			Dim header As String = strHeaderlist

			Dim zeile As String = String.Empty
			Dim FileContentList As New List(Of String)

			Dim headerListToImport As New List(Of String)
			Dim LineList As New List(Of String)
			Dim lineNumber As Integer = 1
			Dim ValueExists As Boolean = False

			For Each itm In employeeData
				If lineNumber = 1 Then
					For i As Integer = 0 To headerList.Count - 1
						If Not String.IsNullOrWhiteSpace(headerList(i).Trim) Then
							headerListToImport.Add(headerList(i).Trim)
						End If
					Next i
				End If
				lineNumber += 1

				'strFieldNameByEmpty = "MDNr{0}Employeenumber{0}Lastname{0}Firstname{0}Street{0}PostOfficeBox{0}Street{0}"
				'strFieldNameByEmpty &= "CountryCode{0}Postcode{0}Location{0}Telephone_P{0}Telephone_2{0}Telephone_3{0}Telephone_G{0}Mobile{0}EMail{0}TotalHours{0}"
				Dim objValue As String
				Dim fieldExists = True
				For Each headeritm In headerListToImport
					fieldExists = True
					Trace.WriteLine(headeritm.ToUpper)
					Select Case headeritm.ToUpper
						Case "MDNr".ToUpper
							objValue = ReplaceMissing(itm.MDNr, 0)
						Case "Employeenumber".ToUpper
							objValue = ReplaceMissing(itm.Employeenumber, 0)
						Case "Lastname".ToUpper
							objValue = ReplaceMissing(itm.Lastname, "")
						Case "Firstname".ToUpper
							objValue = ReplaceMissing(itm.Firstname, "")
						Case "PostOfficeBox".ToUpper
							objValue = ReplaceMissing(itm.PostOfficeBox, "")
						Case "Street".ToUpper
							objValue = ReplaceMissing(itm.Street, "")
						Case "CountryCode".ToUpper
							objValue = ReplaceMissing(itm.CountryCode, "")
						Case "Postcode".ToUpper
							objValue = ReplaceMissing(itm.Postcode, "")
						Case "Location".ToUpper
							objValue = ReplaceMissing(itm.Location, "")
						Case "Telephone_P".ToUpper
							objValue = ReplaceMissing(itm.Telephone_P, "")
						Case "Telephone_2".ToUpper
							objValue = ReplaceMissing(itm.Telephone_2, "")
						Case "Telephone_3".ToUpper
							objValue = ReplaceMissing(itm.Telephone_3, "")
						Case "Telephone_G".ToUpper
							objValue = ReplaceMissing(itm.Telephone_G, "")
						Case "Mobile".ToUpper
							objValue = ReplaceMissing(itm.Mobile, "")
						Case "EMail".ToUpper
							objValue = ReplaceMissing(itm.EMail, "")
						Case "TotalHours".ToUpper
							objValue = String.Format("{0:n2}", ReplaceMissing(itm.TotalHours, 0))
						Case Else
							objValue = String.Empty
							fieldExists = False
					End Select
					If fieldExists Then zeile += String.Format("{0}{1}{0}{2}", darstellungszeichen, objValue.ToString, If(trennzeichen = "{Tab}", vbTab, trennzeichen))

				Next

				If ChkIgnorEmptyRecords.Checked AndAlso String.IsNullOrWhiteSpace(zeile) Then ValueExists = False Else ValueExists = True
				If ValueExists Then
					zeile = zeile.Substring(0, zeile.Length - 1)
					LineList.Add(zeile)
				End If

				zeile = String.Empty
				ValueExists = False
			Next

			FileContentList = LineList
			header = String.Join(If(trennzeichen = "{Tab}", vbTab, trennzeichen), headerListToImport.ToArray())
			FileContentList.Insert(0, header)

			IO.File.WriteAllLines(Me.txt_Filename.Text, FileContentList.ToArray, System.Text.Encoding.Default)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.Message))
			result = False

		Finally

		End Try

		Return result
	End Function



	''' <summary>
	''' export Lohnkonti data as standard table
	''' </summary>
	Private Function ExportLohnkontiDataAsStandardToCSV(ByVal lohnData As List(Of ListingPayrollLohnkontiData)) As Boolean
		Dim result As Boolean = True
		Dim FileContentList As New List(Of String)
		Dim LineList As New List(Of String)

		Try

			Dim trennzeichen As String = Me.cbo_Trennzeichen.Text
			Dim darstellungszeichen As String = Me.cbo_Darstellungszeichen.Text
			If String.IsNullOrWhiteSpace(trennzeichen) Then trennzeichen = ";"

			Dim strHeaderlist As String = ExportSetting.SQLFields
			If String.IsNullOrWhiteSpace(strHeaderlist) Then
				Throw New Exception(m_Translate.GetSafeTranslationValue("Keine Felder wurden ausgewählt. Bitte definieren Sie die Felder in <<EINSTELLUNGEN>>."))
			End If
			strHeaderlist = String.Format(strHeaderlist, trennzeichen)
			strHeaderlist = String.Join("", strHeaderlist.Where(Function(s) Not String.IsNullOrWhiteSpace(s)))
			Dim headerList As List(Of String) = strHeaderlist.Split(trennzeichen).ToList()


			Dim t1 As Type = lohnData(0).GetType

			'make a new instance of the class name we figured out to get its props
			Dim o As Object = Activator.CreateInstance(t1)

			'gets all properties
			Dim props() As PropertyInfo = o.GetType.GetProperties
			Dim newHeaderList As New List(Of String)

			For Each pi As PropertyInfo In props
				' write header
				Dim fieldItem = From header In headerList Where header.ToUpper = pi.Name.ToUpper
				If fieldItem.Any() Then
					newHeaderList.Add(pi.Name)
				End If
			Next
			headerList = newHeaderList
			If headerList.Count = 0 Then
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(String.Format("Keine Felder wurden ausgewählt!"))
				Return False
			End If

			'this acts as datarow
			For Each item In lohnData
				Dim value As New List(Of String)

				'this acts as datacolumn
				For Each pi In props
					Dim currentField As String = pi.Name.ToString

					'this is the row+col intersection (the value)
					Dim headerData = headerList.Where(Function(x) x = currentField).FirstOrDefault

					If Not headerData Is Nothing Then
						value.Add(Convert.ToString(item.GetType.GetProperty(currentField).GetValue(item, Nothing)))
					End If

				Next

				LineList.Add(String.Join(trennzeichen, value))
			Next

			FileContentList = LineList

			strHeaderlist = String.Join(trennzeichen, headerList)
			FileContentList.Insert(0, strHeaderlist)

			IO.File.WriteAllLines(Me.txt_Filename.Text, FileContentList.ToArray, System.Text.Encoding.Default)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = False

		Finally

		End Try

		Return result
	End Function


	''' <summary>
	''' export Lohnkonti data
	''' </summary>
	Private Function ExportLohnkontiDataToCSV(ByVal lohnData As List(Of ListingPayrollLohnkontiData)) As Boolean
		Dim result As Boolean = True
		Dim zeile As String = String.Empty
		Dim FileContentList As New List(Of String)

		Dim headerListToImport As New List(Of String)
		Dim LineList As New List(Of String)
		Dim lineNumber As Integer = 1
		Dim ValueExists As Boolean = False

		Try
			Dim trennzeichen As String = Me.cbo_Trennzeichen.Text
			Dim darstellungszeichen As String = Me.cbo_Darstellungszeichen.Text
			If String.IsNullOrWhiteSpace(trennzeichen) Then trennzeichen = ";"

			Dim headerList As New List(Of String)

			Dim employeeData = lohnData.GroupBy(Function(m) New With {Key m.MANr, Key m.EmployeeFullname}).Where(Function(g) g.Count() >= 1).Select(Function(g) g.Key).ToList()

			Dim headerData = lohnData.GroupBy(Function(m) New With {Key m.LANr, Key m.LALabel}).Where(Function(g) g.Count() >= 1).Select(Function(g) g.Key).ToList()
			If employeeData Is Nothing OrElse employeeData.Count = 0 OrElse headerData Is Nothing OrElse headerData.Count = 0 Then Return result
			headerData = headerData.OrderBy(Function(m) m.LANr).ToList()
			' first header line with alle fields
			headerList.Add("MANr")
			headerList.Add("Nach- / Vorname")
			For Each itm In headerData
				headerList.Add(String.Format("{0:F3}", itm.LANr))
			Next
			Dim header As String = String.Join(trennzeichen, headerList)
			headerList.Clear()

			' second header line with description of first header line
			headerList.Add("")
			headerList.Add("")
			For Each itm In headerData
				headerList.Add(String.Format("{0}", itm.LALabel))
			Next
			Dim header_2 As String = String.Join(trennzeichen, headerList)

			' each employee as one record
			For Each itm In employeeData
				Dim value As New List(Of String)

				value.Add(String.Format("{0:F0}", itm.MANr))
				value.Add(String.Format("{0}", itm.EmployeeFullname))

				For Each la In headerData
					Dim availableLA = lohnData.Where(Function(data) data.MANr = itm.MANr And data.LANr = la.LANr).FirstOrDefault
					Dim laValue As Decimal = 0

					If Not availableLA Is Nothing Then
						laValue = availableLA.Kumulativ
					End If
					value.Add(String.Format("{0:n2}", laValue))

				Next
				LineList.Add(String.Join(trennzeichen, value))
			Next

			FileContentList = LineList

			FileContentList.Insert(0, header)
			FileContentList.Insert(1, header_2)

			IO.File.WriteAllLines(Me.txt_Filename.Text, FileContentList.ToArray, System.Text.Encoding.Default)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			result = False

		Finally

		End Try

		Return result
	End Function


	Private Sub frmCSV_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed

		Me.pccSetting.HidePopup()
		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.frmCSVLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.iCSVWidth = Me.Width
			My.Settings.iCSVHeight = Me.Height

			My.Settings.Save()
		End If

	End Sub

	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)
		ChkIgnorEmptyRecords.Text = m_Translate.GetSafeTranslationValue(ChkIgnorEmptyRecords.Text)
		chkRecordsAsHeader.Text = m_Translate.GetSafeTranslationValue(chkRecordsAsHeader.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)
		Me.bbiSetting.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSetting.Caption)

		Me.lblHeader1.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblHeader2.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader2.Text)
		Me.lblDatei.Text = m_Translate.GetSafeTranslationValue(Me.lblDatei.Text)

		Me.grpSetting.Text = m_Translate.GetSafeTranslationValue(Me.grpSetting.Text)
		Me.lblFelder.Text = m_Translate.GetSafeTranslationValue(Me.lblFelder.Text)
		Me.lblAusgewaehltefelder.Text = m_Translate.GetSafeTranslationValue(Me.lblAusgewaehltefelder.Text)
		Me.lblfeldertrennen.Text = m_Translate.GetSafeTranslationValue(Me.lblfeldertrennen.Text)
		Me.lblfelderdarstellenin.Text = m_Translate.GetSafeTranslationValue(Me.lblfelderdarstellenin.Text)

	End Sub


	Private Sub frmCSV_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Width = Math.Max(My.Settings.iCSVWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.iCSVHeight, Me.Height)
			If My.Settings.frmCSVLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frmCSVLocation.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try
			Me.txt_Filename.Text = ExportSetting.ExportFileName
			'IO.File.WriteAllLines(Me.txt_Filename.Text, New String() {""}, System.Text.Encoding.Default)

		Catch ex As Exception
			txt_Filename.EditValue = Path.Combine(m_InitializationData.UserData.spAllowedPath, Path.GetRandomFileName & ".csv")

		End Try

		Me.cbo_Trennzeichen.Text = ExportSetting.FieldSeprator
		Me.cbo_Darstellungszeichen.Text = ExportSetting.FieldIn

		Me.SQL2Select = ExportSetting.SQL2Open
		Me.SQLFields = ExportSetting.SQLFields

	End Sub

	Private Sub bbiSetting_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSetting.ItemClick
		Dim strVorhandeneFelder As String = String.Empty
		Dim liFields As New List(Of String)
		Dim fieldNameByEmpty As String = String.Empty

		Me.lstSelektierteFelder.Items.Clear()
		Me.lstVorhandeneFelder.Items.Clear()

		If Not CusotmerHourData Is Nothing Then
			ShowpccWithCustomerHourData()
			Return

		ElseIf Not EmployeeHourData Is Nothing Then
			ShowpccWithEmployeeHourData()
			Return

		ElseIf Not LohnKontiData Is Nothing Then
			ShowpccWithLohnKontiData()
			Return

		ElseIf ExportModul = EnumModul.SEARCHRESULTTAXDATA Then
			ShowpccWithQSTData()
			Return

		Else
			Dim conn As SqlConnection = New SqlConnection(m_ConnectionString)
			Dim cmd As SqlCommand = New SqlCommand(Me.ExportSetting.SQL4FieldShow, conn)
			conn.Open()
			Dim reader As SqlDataReader = cmd.ExecuteReader()
			For i As Integer = 0 To reader.FieldCount - 1
				Me.lstVorhandeneFelder.Items.Add(reader.GetName(i).ToString)
			Next
			strVorhandeneFelder = Me.ExportSetting.SQLFields

			If String.IsNullOrWhiteSpace(strVorhandeneFelder) Then strVorhandeneFelder = fieldNameByEmpty
			strVorhandeneFelder = String.Format(strVorhandeneFelder, If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))
			Dim liVorhandeneFelder As List(Of String) = strVorhandeneFelder.Split(CChar(If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))).ToList
			For i As Integer = 0 To liVorhandeneFelder.Count - 1
				If liVorhandeneFelder(i) <> String.Empty Then Me.lstSelektierteFelder.Items.Add(liVorhandeneFelder(i))
			Next

			Me.pccSetting.ShowCloseButton = True
			Dim mouseposition As New Point(Cursor.Position.X + Me.Left - Me.pccSetting.Width, Cursor.Position.Y - 10)
			Me.pccSetting.Manager = New DevExpress.XtraBars.BarManager
			Me.pccSetting.ShowPopup(Cursor.Position)

		End If


	End Sub

	Private Function LoadSearchResultForTaxData() As IEnumerable(Of SearchRestulOfTaxData)
		Dim data = m_ListingDatabaseAccess.LoadSearchResultOfTaxData(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr)
		If data Is Nothing OrElse data.Count = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden geladen."))

			Return Nothing
		End If

		Return data
	End Function

	Private Function ShowpccWithQSTData() As Boolean
		Dim result As Boolean = True
		Dim selectedFields As String
		Dim liFields As New List(Of String)
		Dim existsFieldsToSelect As String

		Me.lstSelektierteFelder.Items.Clear()
		Me.lstVorhandeneFelder.Items.Clear()

		Dim trennzeichen As String = Me.cbo_Trennzeichen.Text
		Dim darstellungszeichen As String = Me.cbo_Darstellungszeichen.Text
		If trennzeichen.Trim.Length = 0 Then trennzeichen = ";"

		Try

			Dim data = LoadSearchResultForTaxData()
			Dim t1 As Type = data(0).GetType

			'make a new instance of the class name we figured out to get its props
			Dim o As Object = Activator.CreateInstance(t1)

			'gets all properties
			Dim props() As PropertyInfo = o.GetType.GetProperties
			Dim newHeaderList As New List(Of String)

			For Each pi As PropertyInfo In props
				newHeaderList.Add(pi.Name.ToUpper())
			Next
			existsFieldsToSelect = String.Join(trennzeichen, newHeaderList)
			selectedFields = ExportSetting.SQLFields.ToUpper()

			'existsFieldsToSelect = String.Format(existsFieldsToSelect, If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))
			'Dim liExistsFields As List(Of String) = newHeaderList ' existsFieldsToSelect.Split(New String() {CChar(If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))}, StringSplitOptions.RemoveEmptyEntries).ToList
			selectedFields = String.Format(selectedFields, If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))
			Dim liSelectedFields As List(Of String) = selectedFields.Split(New String() {CChar(If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))}, StringSplitOptions.RemoveEmptyEntries).ToList

			Dim foundedValue = String.Empty
			lstSelektierteFelder.Items.AddRange(liSelectedFields.ToArray()) '.DataSource = liSelectedFields

			Dim missingList = newHeaderList.Except(liSelectedFields)
			lstVorhandeneFelder.Items.AddRange(missingList.ToArray())

			Me.pccSetting.ShowCloseButton = True
			Dim mouseposition As New Point(Cursor.Position.X + Me.Left - Me.pccSetting.Width, Cursor.Position.Y - 10)
			Me.pccSetting.Manager = New DevExpress.XtraBars.BarManager
			Me.pccSetting.ShowPopup(Cursor.Position)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return False

		End Try


		Return result

	End Function

	Private Function ShowpccWithCustomerHourData() As Boolean
		Dim result As Boolean = True
		Dim selectedFields As String
		Dim liFields As New List(Of String)
		Dim existsFieldsToSelect As String

		Me.lstSelektierteFelder.Items.Clear()
		Me.lstVorhandeneFelder.Items.Clear()

		existsFieldsToSelect = "CustomerNumber{0}Company1{0}Company2{0}Company3{0}PostOfficeBox{0}Street{0}"
		existsFieldsToSelect &= "CountryCode{0}Postcode{0}Location{0}Telephone{0}Telefax{0}EMail{0}TotalHours{0}"
		selectedFields = Me.ExportSetting.SQLFields

		existsFieldsToSelect = String.Format(existsFieldsToSelect, If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))
		Dim liExistsFields As List(Of String) = existsFieldsToSelect.Split(New String() {CChar(If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))}, StringSplitOptions.RemoveEmptyEntries).ToList
		selectedFields = String.Format(selectedFields, If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))
		Dim liSelectedFields As List(Of String) = selectedFields.Split(New String() {CChar(If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))}, StringSplitOptions.RemoveEmptyEntries).ToList

		Dim foundedValue = String.Empty
		lstSelektierteFelder.Items.AddRange(liSelectedFields.ToArray()) '.DataSource = liSelectedFields

		Dim missingList = liExistsFields.Except(liSelectedFields)
		lstVorhandeneFelder.Items.AddRange(missingList.ToArray())

		Me.pccSetting.ShowCloseButton = True
		Dim mouseposition As New Point(Cursor.Position.X + Me.Left - Me.pccSetting.Width, Cursor.Position.Y - 10)
		Me.pccSetting.Manager = New DevExpress.XtraBars.BarManager
		Me.pccSetting.ShowPopup(Cursor.Position)


		Return result

	End Function

	Private Function ShowpccWithEmployeeHourData() As Boolean
		Dim result As Boolean = True
		Dim selectedFields As String
		Dim liFields As New List(Of String)
		Dim existsFieldsToSelect As String

		Me.lstSelektierteFelder.Items.Clear()
		Me.lstVorhandeneFelder.Items.Clear()

		existsFieldsToSelect = "MDNr{0}Employeenumber{0}Lastname{0}Firstname{0}Street{0}PostOfficeBox{0}Street{0}"
		existsFieldsToSelect &= "CountryCode{0}Postcode{0}Location{0}Telephone_P{0}Telephone_2{0}Telephone_3{0}Telephone_G{0}Mobile{0}EMail{0}TotalHours{0}"
		selectedFields = Me.ExportSetting.SQLFields

		existsFieldsToSelect = String.Format(existsFieldsToSelect, If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))
		Dim liExistsFields As List(Of String) = existsFieldsToSelect.Split(New String() {CChar(If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))}, StringSplitOptions.RemoveEmptyEntries).ToList
		selectedFields = String.Format(selectedFields, If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))
		Dim liSelectedFields As List(Of String) = selectedFields.Split(New String() {CChar(If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))}, StringSplitOptions.RemoveEmptyEntries).ToList

		Dim foundedValue = String.Empty
		lstSelektierteFelder.Items.AddRange(liSelectedFields.ToArray()) '.DataSource = liSelectedFields

		Dim missingList = liExistsFields.Except(liSelectedFields)
		lstVorhandeneFelder.Items.AddRange(missingList.ToArray())

		Me.pccSetting.ShowCloseButton = True
		Dim mouseposition As New Point(Cursor.Position.X + Me.Left - Me.pccSetting.Width, Cursor.Position.Y - 10)
		Me.pccSetting.Manager = New DevExpress.XtraBars.BarManager
		Me.pccSetting.ShowPopup(Cursor.Position)


		Return result

	End Function

	Private Function ShowpccWithLohnKontiData() As Boolean
		Dim result As Boolean = True
		Dim selectedFields As String
		Dim liFields As New List(Of String)
		Dim existsFieldsToSelect As String

		Me.lstSelektierteFelder.Items.Clear()
		Me.lstVorhandeneFelder.Items.Clear()

		Dim trennzeichen As String = Me.cbo_Trennzeichen.Text
		Dim darstellungszeichen As String = Me.cbo_Darstellungszeichen.Text
		If trennzeichen.Trim.Length = 0 Then trennzeichen = ";"

		Dim t1 As Type = LohnKontiData(0).GetType

		'make a new instance of the class name we figured out to get its props
		Dim o As Object = Activator.CreateInstance(t1)

		'gets all properties
		Dim props() As PropertyInfo = o.GetType.GetProperties
		Dim newHeaderList As New List(Of String)

		For Each pi As PropertyInfo In props
			newHeaderList.Add(pi.Name.ToUpper())
		Next
		existsFieldsToSelect = String.Join(trennzeichen, newHeaderList)

		selectedFields = ExportSetting.SQLFields.ToUpper()

		'existsFieldsToSelect = String.Format(existsFieldsToSelect, If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))
		'Dim liExistsFields As List(Of String) = newHeaderList ' existsFieldsToSelect.Split(New String() {CChar(If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))}, StringSplitOptions.RemoveEmptyEntries).ToList
		selectedFields = String.Format(selectedFields, If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))
		Dim liSelectedFields As List(Of String) = selectedFields.Split(New String() {CChar(If(Me.cbo_Trennzeichen.Text = "{Tab}", vbTab, Me.cbo_Trennzeichen.Text))}, StringSplitOptions.RemoveEmptyEntries).ToList

		Dim foundedValue = String.Empty
		lstSelektierteFelder.Items.AddRange(liSelectedFields.ToArray()) '.DataSource = liSelectedFields

		Dim missingList = newHeaderList.Except(liSelectedFields)
		lstVorhandeneFelder.Items.AddRange(missingList.ToArray())

		Me.pccSetting.ShowCloseButton = True
		Dim mouseposition As New Point(Cursor.Position.X + Me.Left - Me.pccSetting.Width, Cursor.Position.Y - 10)
		Me.pccSetting.Manager = New DevExpress.XtraBars.BarManager
		Me.pccSetting.ShowPopup(Cursor.Position)


		Return result

	End Function

	Private Sub lstVorhandeneFelder_DoubleClick(sender As Object, e As System.EventArgs) Handles lstVorhandeneFelder.DoubleClick

		Dim value = Me.lstVorhandeneFelder.SelectedItem

		lstVorhandeneFelder.Items.Remove(value)
		lstVorhandeneFelder.Refresh()
		lstSelektierteFelder.Items.Add(value)
		lstSelektierteFelder.Refresh()

	End Sub

	Private Sub lstSelektierteFelder_KeyUp(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles lstSelektierteFelder.KeyUp

		If Me.lstSelektierteFelder.SelectedItem Is Nothing Then Exit Sub
		Dim value = Me.lstSelektierteFelder.SelectedItem
		If e.KeyCode = Keys.Delete Then
			lstVorhandeneFelder.Items.Add(value)
			lstVorhandeneFelder.Refresh()

			lstSelektierteFelder.Items.Remove(value.ToString)
			lstSelektierteFelder.Refresh()

		End If

	End Sub

	Private Sub pccESExport_CloseUp(sender As Object, e As System.EventArgs) Handles pccSetting.CloseUp
		Dim strFelder As String = String.Empty
		'Return

		For i As Integer = 0 To Me.lstSelektierteFelder.Items.Count - 1
			strFelder &= If(strFelder.Length > 1, "{0}", "") & Me.lstSelektierteFelder.Items(i).ToString
		Next

		Me.ExportSetting.ExportFileName = Me.txt_Filename.Text
		Me.ExportSetting.SQLFields = strFelder
		Me.ExportSetting.FieldSeprator = Me.cbo_Trennzeichen.Text
		Me.ExportSetting.FieldIn = Me.cbo_Darstellungszeichen.Text

		SaveCSVSetting()

	End Sub


	Private Function SaveCSVSetting() As Boolean
		Dim success As Boolean = True
		Dim strXMLFile As String = _ClsProgSetting.GetUserProfileFile()
		Dim xDoc As XmlDocument = New XmlDocument()
		Dim xNode As XmlNode
		Dim xElmntFamily As XmlElement = Nothing


		Try
			xDoc.Load(strXMLFile)

			xNode = xDoc.SelectSingleNode(String.Format("//CSV-Setting[@Name='{0}']", Me.ExportSetting.ModulName))
			If xNode Is Nothing Then
				Dim newNode As Xml.XmlElement = xDoc.CreateElement("CSV-Setting")

				newNode.SetAttribute("Name", Me.ExportSetting.ModulName)
				xDoc.DocumentElement.AppendChild(newNode)
				xNode = xDoc.SelectSingleNode(String.Format("//CSV-Setting[@Name='{0}']", Me.ExportSetting.ModulName))
			End If
			If xNode IsNot Nothing Then
				If TypeOf xNode Is XmlElement Then
					xElmntFamily = CType(xNode, XmlElement)
				End If
				' Felder
				If xElmntFamily.SelectSingleNode("SelectedFields") IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("SelectedFields"))
				InsertTextNode(xDoc, xElmntFamily, "SelectedFields", Me.ExportSetting.SQLFields)

				If xElmntFamily.SelectSingleNode("FieldIn") IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("FieldIn"))
				InsertTextNode(xDoc, xElmntFamily, "FieldIn", Me.ExportSetting.FieldIn)

				If xElmntFamily.SelectSingleNode("FieldSep") IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("FieldSep"))
				InsertTextNode(xDoc, xElmntFamily, "FieldSep", Me.ExportSetting.FieldSeprator)

				If xElmntFamily.SelectSingleNode("ExportFileName") IsNot Nothing Then _
							xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode("ExportFileName"))
				InsertTextNode(xDoc, xElmntFamily, "ExportFileName", Me.ExportSetting.ExportFileName)

			End If
			xDoc.Save(strXMLFile)

		Catch ex As Exception
			success = False
			m_Logger.LogError(ex.ToString)
		End Try


		Return success

	End Function

	Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode,
															ByVal strTag As String, ByVal strText As String) As XmlElement
		Dim xNodeTemp As XmlNode

		xNodeTemp = xDoc.CreateElement(strTag)
		xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
		xNode.AppendChild(xNodeTemp)

		Return CType(xNodeTemp, XmlElement)
	End Function


	Private Sub CmdClose_Disposed(sender As Object, e As System.EventArgs)

		If Not Me.WindowState = FormWindowState.Minimized Then
			My.Settings.frmCSVLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
			My.Settings.iCSVWidth = Me.Width
			My.Settings.iCSVHeight = Me.Height

			My.Settings.Save()
		End If

	End Sub

	Private Sub frm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And _ClsProgSetting.GetLogedUSNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub

	Private Sub chkRecordsAsHeader_CheckStateChanged(sender As Object, e As EventArgs) Handles chkRecordsAsHeader.CheckStateChanged
		bbiSetting.Enabled = Not chkRecordsAsHeader.Checked
	End Sub


#Region "Helpers"


	Sub CreateCSVFromGenericList(Of T)(ByVal list As List(Of T), ByVal sepString As String, ByVal csvNameWithExt As String)
		If ((list Is Nothing) OrElse (list.Count = 0)) Then
			Return
		End If

		'get type from 0th member
		Dim t1 As Type = list(0).GetType
		Dim newLine As String = Environment.NewLine
		Dim sw = New StreamWriter(csvNameWithExt)

		'make a new instance of the class name we figured out to get its props
		Dim o As Object = Activator.CreateInstance(t1)
		'gets all properties
		Dim props() As PropertyInfo = o.GetType.GetProperties
		'foreach of the properties in class above, write out properties
		'this is the header row
		For Each pi As PropertyInfo In props
			sw.Write((pi.Name.ToUpper + sepString))
		Next
		sw.Write(newLine)

		'this acts as datarow
		For Each item As T In list
			'this acts as datacolumn
			For Each pi As PropertyInfo In props
				'this is the row+col intersection (the value)
				Dim whatToWrite As String = (Convert.ToString(item.GetType.GetProperty(pi.Name).GetValue(item, Nothing)).Replace(Microsoft.VisualBasic.ChrW(44), Microsoft.VisualBasic.ChrW(32)) + Microsoft.VisualBasic.ChrW(44))
				sw.Write(whatToWrite)
			Next

			sw.Write(newLine)
		Next

		sw.Close()
		sw.Dispose()

	End Sub

	Private Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function

#End Region


End Class


