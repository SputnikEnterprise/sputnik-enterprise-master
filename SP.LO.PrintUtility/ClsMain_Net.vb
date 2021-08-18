
Imports System.IO.File

Imports SPS.Listing.Print.Utility
Imports SPProgUtility.Mandanten

Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient

Imports SP.LO.PrintUtility.ClsDataDetail


Public Class ClsMain_Net

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

  Private Property SelectedLONr As New List(Of Integer)
  Private Property SelectedMANr As New List(Of Integer)
  Private Property SelectedData2WOS As New List(Of Boolean)
  Private Property SelectedMALang As New List(Of String)

  Private _LOSetting As New ClsLOSetting



#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal losetting As ClsLOSetting)

		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		_LOSetting = losetting

		Application.EnableVisualStyles()

	End Sub

	Public Sub New(ByVal _setting As ClsLOSetting)

		_LOSetting = _setting


		Dim m_md As New SPProgUtility.Mandanten.Mandant

		Dim init = CreateInitialData(m_md.GetDefaultMDNr, m_md.GetDefaultUSNr)
		ClsDataDetail.m_InitialData = init
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(init.TranslationData, init.ProsonalizedData)

		Application.EnableVisualStyles()


	End Sub

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


#End Region



#Region "Startfunktionen..."

	Sub ShowfrmLO4Details()

		If _LOSetting.SelectedYear Is Nothing Then _LOSetting.SelectedYear = New List(Of Integer)(New Integer() {Now.Year})
		If _LOSetting.SelectedMonth Is Nothing Then _LOSetting.SelectedMonth = New List(Of Integer)(New Integer() {Now.Month})
		_LOSetting.ShowLODetails = True

		'Dim frmTest As frmLOPrint
		'frmTest = New frmLOPrint(_LOSetting)

		'frmTest.Show()
		'frmTest.BringToFront()

	End Sub

	Sub ShowfrmLO4Print()

		If _LOSetting.SelectedYear Is Nothing Then _LOSetting.SelectedYear = New List(Of Integer)(New Integer() {Now.Year})
		If _LOSetting.SelectedMonth Is Nothing Then _LOSetting.SelectedMonth = New List(Of Integer)(New Integer() {Now.Month})

		'Dim frmTest As frmLOPrint
		'frmTest = New frmLOPrint(_LOSetting)

		'frmTest.Show()
		'frmTest.BringToFront()

	End Sub

	Sub ShowfrmLO4Delete()

		_LOSetting.FormOpenArt = ClsLOSetting.OpenFormArt.DeleteingLO
		If _LOSetting.SelectedYear Is Nothing Then _LOSetting.SelectedYear = New List(Of Integer)(New Integer() {Now.Year})
		If _LOSetting.SelectedMonth Is Nothing Then _LOSetting.SelectedMonth = New List(Of Integer)(New Integer() {Now.Month})

		'Dim frmTest As frmLOPrint
		'frmTest = New frmLOPrint(_LOSetting)

		'frmTest.Show()
		'frmTest.BringToFront()

	End Sub

	''' <summary>
	''' Druckt / Sendet die ausgewählte Lohnabrechnung aus...
	''' </summary>
	''' <param name="strLONr"></param>
	''' <param name="sSend2WOS">0 = NUR drucken | 1 = Drucken und Senden | 2 = NUR Senden</param>
	''' <remarks></remarks>
	Sub PrintSelectedLO(ByVal strLONr As String, ByVal sSend2WOS As Short)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim _setting As ClsLOSetting = _LOSetting
		Dim iYear As Integer = 0
		Dim iMonth As Short = 0
		Dim strWhereQuery As String = String.Empty
		Dim bSendPrintJob2WOS As Boolean = False
		Dim bSend_And_PrintJob2WOS As Boolean = False
		Dim m_md As New Mandant

		_LOSetting.FormOpenArt = ClsLOSetting.OpenFormArt.PrintingLO
		If sSend2WOS = 0 Then

		ElseIf sSend2WOS = 1 Then
			bSend_And_PrintJob2WOS = True
		ElseIf sSend2WOS = 2 Then
			bSendPrintJob2WOS = True
		End If

		If Not String.IsNullOrWhiteSpace(strLONr) And strLONr.Trim.Length > 0 Then
			strWhereQuery &= String.Format("LO.LONr In ({0}) ", strLONr)
		End If

		Dim strSqlQuery As String = "SELECT LO.ID, LO.LONr, LO.MANr, IsNull(MA.Send2WOS, 0) As Send2WOS, IsNull(MA.Sprache, 'deutsch') As Sprache, "
		strSqlQuery += "(Convert(nvarchar(2), LO.LP) + ' / ' + convert(nvarchar(4), LO.Jahr)) As Zeitraum, "
		strSqlQuery += "(MA.Vorname + ' ' + MA.Nachname) As MAName, LO.CreatedOn, LO.CreatedFrom "
		strSqlQuery += "FROM LO Left Join Mitarbeiter MA On LO.MANr = MA.MANr "
		If Not String.IsNullOrWhiteSpace(strWhereQuery) Then strWhereQuery = String.Format("Where {0} ", strWhereQuery)
		strSqlQuery = String.Format("{0} {1} Order By MA.Nachname ASC, MA.Vorname ASC, LO.CreatedOn Desc", strSqlQuery, strWhereQuery)

		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
		cmd.CommandType = Data.CommandType.Text
		Dim rFrec As SqlDataReader = cmd.ExecuteReader

		Dim bAllowedWOS As Boolean = m_md.AllowedExportEmployee2WOS(_LOSetting.SelectedMDNr, Now.Year)
		If sSend2WOS = 0 Then bAllowedWOS = False

		Do While rFrec.Read
			SelectedLONr.Add(CInt(rFrec("LONr")))
			SelectedMANr.Add(CInt(rFrec("MANr")))
			SelectedData2WOS.Add(If(Not bAllowedWOS, False, CBool(rFrec("Send2WOS"))))
			SelectedMALang.Add((CStr(rFrec("Sprache").ToString.Substring(0, 1).ToUpper)))
		Loop

		Dim _PSetting As New ClsLLLOSearchPrintSetting
		_PSetting = New ClsLLLOSearchPrintSetting With {.DbConnString2Open = m_InitialData.MDData.MDDbConn, _
																																						 .SQL2Open = String.Empty, _
																																						 .JobNr2Print = "9.1", _
																																						 .Is4Export = False, _
																																						 .SendData2WOS = bSendPrintJob2WOS, _
																																						 .SendAndPrintData2WOS = bSend_And_PrintJob2WOS, _
																																						 .liLONr2Print = Me.SelectedLONr, _
																																						 .liMANr2Print = Me.SelectedMANr, _
																																						 .liLOSend2WOS = Me.SelectedData2WOS, _
																																						 .LiMALang = Me.SelectedMALang, _
																																						 .SelectedLONr2Print = 0, _
																																						 .SelectedMANr2Print = 0,
																										.SelectedMDNr = m_InitialData.MDData.MDNr,
																										.LogedUSNr = m_InitialData.UserData.UserNr,
																										.PerosonalizedData = ClsDataDetail.ProsonalizedData,
																										.TranslationData = ClsDataDetail.TranslationData
																																						 }

		Dim obj As New LOSearchListing.ClsPrintLOSearchList(_PSetting)
		Dim strResult = obj.PrintLOSearchList(False)

		If strResult.WOSresult.HasValue Then
			If strResult.WOSresult Then
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich in WOS-Plattform veröffentlicht.")

				DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_Translate.GetSafeTranslationValue("Daten exportieren"), _
																									 MessageBoxButtons.OK, MessageBoxIcon.Information)

			Else
				Dim strMsg As String = m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich in WOS-Plattform veröffentlicht werden!")

				DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_Translate.GetSafeTranslationValue("Daten exportieren"), _
																									 MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
			End If
		End If

	End Sub

	'Function ExportSelectedLO(ByVal sSend2WOS As Short) As String
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim _setting As ClsLOSetting = _LOSetting
	'	Dim iYear As Integer = 0
	'	Dim iMonth As Short = 0
	'	Dim strWhereQuery As String = String.Empty
	'	Dim strResult As String = "Success"
	'	Dim bSendPrintJob2WOS As Boolean = False
	'	Dim bSend_And_PrintJob2WOS As Boolean = False
	'	Dim strLONr As String = String.Empty
	'	Dim m_md As New Mandant

	'	_LOSetting.FormOpenArt = ClsLOSetting.OpenFormArt.PrintingLO
	'	If sSend2WOS = 0 Then
	'	ElseIf sSend2WOS = 1 Then
	'		bSend_And_PrintJob2WOS = True
	'	ElseIf sSend2WOS = 2 Then
	'		bSendPrintJob2WOS = True
	'	End If

	'	Try

	'		Dim strAndString As String = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
	'		If Not _setting.SelectedLONr Is Nothing And _setting.SelectedLONr.Count > 0 Then
	'			For i As Integer = 0 To _setting.SelectedLONr.Count - 1
	'				strLONr &= If(strLONr.Length > 0, ",", "") & _setting.SelectedLONr.Item(i)
	'			Next
	'			strWhereQuery &= String.Format("{0}LO.LONr In ({1}) ", strAndString, strLONr)
	'		End If

	'		Dim strSqlQuery As String = "SELECT LO.ID, LO.LONr, LO.MANr, IsNull(MA.Send2WOS, 0) As Send2WOS, IsNull(MA.Sprache, 'deutsch') As Sprache, "
	'		strSqlQuery += "(Convert(nvarchar(2), LO.LP) + ' / ' + convert(nvarchar(4), LO.Jahr)) As Zeitraum, "
	'		strSqlQuery += "(MA.Vorname + ' ' + MA.Nachname) As MAName, LO.CreatedOn, LO.CreatedFrom "
	'		strSqlQuery += "FROM LO Left Join Mitarbeiter MA On LO.MANr = MA.MANr "
	'		If Not String.IsNullOrWhiteSpace(strWhereQuery) Then strWhereQuery = String.Format("Where {0} ", strWhereQuery)
	'		strSqlQuery = String.Format("{0} {1} Order By MA.Nachname ASC, MA.Vorname ASC, LO.CreatedOn Desc", strSqlQuery, strWhereQuery)

	'		Conn.Open()

	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'		cmd.CommandType = Data.CommandType.Text
	'		Dim rFrec As SqlDataReader = cmd.ExecuteReader

	'		Dim bAllowedWOS As Boolean = m_md.AllowedExportEmployee2WOS(_LOSetting.SelectedMDNr, Now.Year)
	'		If Not sSend2WOS = 0 Then bAllowedWOS = False

	'		Do While rFrec.Read
	'			SelectedLONr.Add(CInt(rFrec("LONr")))
	'			SelectedMANr.Add(CInt(rFrec("MANr")))
	'			SelectedData2WOS.Add(If(Not bAllowedWOS, False, CBool(rFrec("Send2WOS"))))
	'			SelectedMALang.Add((CStr(rFrec("Sprache").ToString.Substring(0, 1).ToUpper)))
	'		Loop

	'	Catch ex As Exception
	'		m_logger.loginfo(String.Format("{0}:Query-Abfrage: {1}", strMethodeName, ex.Message))
	'		Return String.Format("Error: {0}", ex.Message)

	'	End Try

	'	Try

	'		Dim _PSetting As New ClsLLLOSearchPrintSetting
	'		_PSetting = New ClsLLLOSearchPrintSetting With {.DbConnString2Open = m_InitialData.MDData.MDDbConn, _
	'																																						 .SQL2Open = String.Empty, _
	'																																						 .JobNr2Print = "9.1", _
	'																																						 .Is4Export = True, _
	'																																						 .SendData2WOS = bSendPrintJob2WOS, _
	'																																						 .SendAndPrintData2WOS = bSend_And_PrintJob2WOS, _
	'																																						 .liLONr2Print = Me.SelectedLONr, _
	'																																						 .liMANr2Print = Me.SelectedMANr, _
	'																																						 .liLOSend2WOS = Me.SelectedData2WOS, _
	'																																						 .LiMALang = Me.SelectedMALang, _
	'																																						 .SelectedLONr2Print = 0, _
	'																																						 .SelectedMANr2Print = 0,
	'																										.SelectedMDNr = m_InitialData.MDData.MDNr,
	'																									.LogedUSNr = m_InitialData.UserData.UserNr,
	'																									.PerosonalizedData = ClsDataDetail.ProsonalizedData,
	'																									.TranslationData = ClsDataDetail.TranslationData
	'																																					 }
	'		Dim obj As New LOSearchListing.ClsPrintLOSearchList(_PSetting)
	'		strResult = obj.ExportLOSearchList()

	'	Catch ex As Exception
	'		m_logger.loginfo(String.Format("{0}:Daten exportieren. {1}", strMethodeName, ex.Message))
	'		Return String.Format("Error: Daten exportieren. {0}", ex.Message)

	'	End Try

	'	Return strResult
	'End Function

	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub


#End Region

End Class
