
Imports System.IO.File

Imports SPS.Listing.Print.Utility
Imports SPS.Listing.Print.Utility.ESVertrag
Imports SPS.Listing.Print.Utility.ESVerleih
Imports SPS.ES.Utility.SPSESUtility.ClsESFunktionality
Imports SPS.ES.Utility.SPRPSUtility.ClsRPFunktionality

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility
Imports SPProgUtility.Mandanten

Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.IO
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.CommonSettings


Public Class ClsMain_Net
	Protected Shared m_Logger As ILogger = New Logger()

	Public Shared frmTest As frmESPrint

	Protected m_Utility As SPProgUtility.MainUtilities.Utilities
	Protected m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_xml As New ClsXML


  Private Property SelectedESNr As New List(Of Integer)
  Private Property SelectedMANr As New List(Of Integer)
  Private Property SelectedKDNr As New List(Of Integer)
  Private Property SelectedKDZHDNr As New List(Of Integer)

  Private Property SelectedMAData2WOS As New List(Of Boolean)
  Private Property SelectedKDData2WOS As New List(Of Boolean)

  Private Property SelectedMALang As New List(Of String)
  Private Property SelectedKDLang As New List(Of String)

  Private _ESSetting As New ClsESSetting
  Private m_md As Mandant

  Private WithEvents mESVertragWorker As BackgroundWorker
  Private WithEvents mESVerleihWorker As BackgroundWorker
  Private WithEvents mESDeleteWorker As BackgroundWorker

  Private Property ResultOFExportingESVertragRec As String
  Private Property ResultOFExportingESVerleihRec As String

  Private Property ResultOFDeletingESRec As String



#Region "Constructor"

	Public Sub New(ByVal _setting As ClsESSetting)

		_ESSetting = _setting

		If _ESSetting.MDData Is Nothing Or _ESSetting.MDData.MDNr = 0 Then
			ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
			ClsDataDetail.UserData = ClsDataDetail.LogededUSData(ClsDataDetail.MDData.MDNr, 0)

			ClsDataDetail.ProsonalizedData = ClsDataDetail.ProsonalizedValues
			ClsDataDetail.TranslationData = ClsDataDetail.TranslationValues

			_ESSetting.TranslationData = ClsDataDetail.TranslationData

		Else
			ClsDataDetail.MDData = _ESSetting.MDData
			ClsDataDetail.UserData = _ESSetting.UserData
			ClsDataDetail.ProsonalizedData = _ESSetting.PerosonalizedData
			ClsDataDetail.TranslationData = _ESSetting.TranslationData

		End If
		m_md = New Mandant
		m_Utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.PerosonalizedData)
		If _ESSetting.ShowDesign Is Nothing Then _ESSetting.ShowDesign = False

		Application.EnableVisualStyles()


	End Sub

#End Region



#Region "Startfunktionen..."

  Private ReadOnly Property GetSQL2Open() As String
    Get
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

      Dim strWhereQuery As String = String.Empty
      Dim strESNr As String = String.Empty
      Dim strSqlQuery As String = String.Empty
      Try
        If _ESSetting.SelectedESNr.Count > 0 Then
          For i As Integer = 0 To _ESSetting.SelectedESNr.Count - 1
            strESNr &= If(strESNr.Length > 0, ",", "") & _ESSetting.SelectedESNr.Item(i)
          Next
          strWhereQuery &= String.Format("ES.ESNr In ({0}) ", strESNr)
        End If

        strSqlQuery = "SELECT ES.ID, ES.ESNr, ES.MANr, ES.KDNr, ES.ES_Als, isnull(ES.KDZHDNr, 0) As KDZHDNr, ESL.GAVGruppe0, "
				strSqlQuery += If(m_md.AllowedExportEmployee2WOS(ClsDataDetail.MDData.MDNr, Now.Year), "IsNull(MA.Send2WOS, 0)", "0 ") & " As MAWOS, "

				strSqlQuery += "(CASE "
				strSqlQuery += "WHEN UPPER(SUBSTRING(Ma.Sprache, 1, 1)) = 'F' THEN 'französisch' "
				strSqlQuery += "WHEN UPPER(SUBSTRING(Ma.Sprache, 1, 1)) = 'I' THEN 'italienisch' "
				strSqlQuery += "WHEN UPPER(SUBSTRING(Ma.Sprache, 1, 1)) = 'E' THEN 'english' "
				strSqlQuery += "ELSE 'deutsch'  "
				strSqlQuery += "End "
				strSqlQuery += ") As MASprache, "

				strSqlQuery += "(Convert(nvarchar(10), ES.ES_Ab, 104) + ' - ' + IsNull(convert(nvarchar(10), ES.ES_Ende, 104), '')) As Zeitraum, "
        strSqlQuery += "(MA.Nachname + ', ' + MA.Vorname) As MAName, "
        strSqlQuery += "KD.Firma1, "
        strSqlQuery += If(m_md.AllowedExportCustomer2WOS(ClsDataDetail.MDData.MDNr, Now.Year), "IsNull(KD.Send2WOS, 0)", "0 ") & " As KDWOS, "

				strSqlQuery += "(CASE "
				strSqlQuery += "WHEN KD.Sprache = '' THEN 'deutsch' "
				strSqlQuery += "WHEN UPPER(SUBSTRING(KD.Sprache, 1, 1)) = 'F' THEN 'französisch' "
				strSqlQuery += "WHEN UPPER(SUBSTRING(KD.Sprache, 1, 1)) = 'I' THEN 'italienisch' "
				strSqlQuery += "WHEN UPPER(SUBSTRING(KD.Sprache, 1, 1)) = 'E' THEN 'english' "
				strSqlQuery += "ELSE 'deutsch'  "
				strSqlQuery += "End "
				strSqlQuery += ") As KDSprache "

				strSqlQuery += "FROM ES "
        strSqlQuery += "Left Join ESLohn ESL On ES.ESNr = ESL.ESNr And ESL.Aktivlodaten = 1 "
        strSqlQuery += "Left Join Mitarbeiter MA On ES.MANr = MA.MANr "
        strSqlQuery += "Left Join Kunden KD On ES.KDNr = KD.KDNr "
        If Not String.IsNullOrWhiteSpace(strWhereQuery) Then strWhereQuery = String.Format("Where {0} ", strWhereQuery)
        strSqlQuery = String.Format("{0} {1} Order By MA.Nachname ASC, MA.Vorname ASC, ES.ES_Ab ", strSqlQuery, strWhereQuery)

      Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.StackTrace))

			End Try
			Return strSqlQuery

		End Get

	End Property


	Sub ShowfrmES4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		m_Logger.LogInfo(String.Format("{0}. Modul wird gestartet...", strMethodeName))
		If _ESSetting.SelectedYear Is Nothing Then _ESSetting.SelectedYear = New List(Of Integer)(New Integer() {0})
		If _ESSetting.SelectedMonth Is Nothing Then _ESSetting.SelectedMonth = New List(Of Short)(New Short() {0})
		frmTest = New frmESPrint(_ESSetting)
		frmTest.Show()

	End Sub

	Sub ShowfrmES4Delete()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		m_Logger.LogInfo(String.Format("{0}. Modul wird gestartet...", strMethodeName))

		_ESSetting.FormOpenArt = ClsESSetting.OpenFormArt.DeleteingES
		frmTest = New frmESPrint(_ESSetting)
		frmTest.Show()

	End Sub


	''' <summary>
	''' Druckt / Sendet die ausgewählte Lohnabrechnung aus...
	''' </summary>
	''' <param name="_bAsVerleih">True = Der Verleihvertrag drucken | False = Nur den Einsatzvertrag drucken</param>
	''' <param name="_bPrintWithVerleih">True = Verleihvertrag mit Einsatzvertrag drucken | False = Nur Einsatzvertrag drucken</param>
	''' <param name="sSend2WOS">0 = NUR drucken | 1 = Drucken und Senden | 2 = NUR Senden</param>
	''' <remarks></remarks>
	Function PrintSelectedES(ByVal _bAsVerleih As Boolean, ByVal _bPrintWithVerleih As Boolean, ByVal sSend2WOS As Short) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim _setting As ClsESSetting = _ESSetting
		Dim iYear As Integer = 0
		Dim iMonth As Short = 0
		Dim strWhereQuery As String = String.Empty
		Dim bSendPrintJob2WOS As Boolean = False
		Dim bSend_And_PrintJob2WOS As Boolean = False
		Dim strSqlQuery As String = String.Empty


		SelectedESNr.Clear()
		SelectedMANr.Clear()
		SelectedKDNr.Clear()
		SelectedMAData2WOS.Clear()
		SelectedKDData2WOS.Clear()
		SelectedMALang.Clear()
		SelectedKDLang.Clear()

		Try

			_ESSetting.FormOpenArt = ClsESSetting.OpenFormArt.PrintingES
			If sSend2WOS = 0 Then

			ElseIf sSend2WOS = 1 Then
				bSend_And_PrintJob2WOS = True
			ElseIf sSend2WOS = 2 Then
				bSendPrintJob2WOS = True
			End If

			Dim bAllowedWOS As Boolean = m_md.AllowedExportEmployee2WOS(ClsDataDetail.MDData.MDNr, Now.Year) Or m_md.AllowedExportCustomer2WOS(ClsDataDetail.MDData.MDNr, Now.Year)
			If sSend2WOS = 0 Then bAllowedWOS = False

		Catch ex As Exception
			Dim strMsg As String = String.Format("{0}:Datenbank-Abfrage.{1}", strMethodeName, ex.StackTrace)
			m_Logger.LogError(strMsg)
			Return "Error: " & strMsg

		End Try

		Try
			strSqlQuery = GetSQL2Open()
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim rFrec As SqlDataReader = cmd.ExecuteReader

			Do While rFrec.Read
				SelectedESNr.Add(m_Utility.SafeGetInteger(rFrec, "ESNr", 0))
				SelectedMANr.Add(m_Utility.SafeGetInteger(rFrec, "MANr", 0))
				SelectedKDNr.Add(m_Utility.SafeGetInteger(rFrec, "KDNr", 0))
				SelectedKDZHDNr.Add(m_Utility.SafeGetInteger(rFrec, "KDZHDNr", 0))

				SelectedMAData2WOS.Add(If(sSend2WOS = 0, False, If(m_md.AllowedExportEmployee2WOS(ClsDataDetail.MDData.MDNr, Now.Year), m_Utility.SafeGetBoolean(rFrec, "MAWOS", False), False)))
				SelectedKDData2WOS.Add(If(sSend2WOS = 0, False, If(m_md.AllowedExportCustomer2WOS(ClsDataDetail.MDData.MDNr, Now.Year), m_Utility.SafeGetBoolean(rFrec, "KDWOS", False), False)))

				SelectedMALang.Add(CStr((m_Utility.SafeGetString(rFrec, "MASprache")).Substring(0, 1).ToUpper))
				SelectedKDLang.Add(CStr((m_Utility.SafeGetString(rFrec, "KDSprache")).Substring(0, 1).ToUpper))

			Loop

		Catch ex As Exception
			Dim strMsg As String = String.Format("{0}: {1}. {2}", strMethodeName, ex.ToString, strSqlQuery)
			m_Logger.LogError(strMsg)
			Return "Error: " & strMsg

		End Try

		m_Logger.LogDebug(String.Format("bSendPrintJob2WOS: {0}", bSendPrintJob2WOS))

		Dim _locSetting As ClsLLESVertragSetting = New ClsLLESVertragSetting With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn, _
																																						 .SQL2Open = String.Empty, _
																																						 .liESNr2Print = Me.SelectedESNr, _
																																						 .liMANr2Print = Me.SelectedMANr, _
																																						 .liKDNr2Print = Me.SelectedKDNr, _
																																						 .liKDZHDNr2Print = Me.SelectedKDZHDNr, _
																																						 .liSendESMAData2WOS = Me.SelectedMAData2WOS, _
																																						 .liSendESKDData2WOS = Me.SelectedKDData2WOS, _
																																						 .SendAndPrintData2WOS = bSend_And_PrintJob2WOS, _
																																						 .liESSend2WOS = SelectedMAData2WOS, _
																																						 .LiMALang = Me.SelectedMALang, _
																																						 .LiKDLang = Me.SelectedKDLang, _
																																						 .SelectedESNr2Print = 0, _
																																						 .SelectedMANr2Print = 0,
																																							 .SelectedMDNr = ClsDataDetail.MDData.MDNr,
																																							 .LogedUSNr = ClsDataDetail.UserData.UserNr,
																																							 .PerosonalizedData = ClsDataDetail.ProsonalizedData, .TranslationData = ClsDataDetail.TranslationData}
		Dim strResult As String = String.Empty
		If _bPrintWithVerleih Then
			_locSetting.IsPrintAsVerleih = False
			_locSetting.JobNr2Print = If(String.IsNullOrWhiteSpace(_setting.JobNr2Print), "4.3", _setting.JobNr2Print)

			' ESVertrag drucken
			Dim obj As New ClsPrintESVertrag(_locSetting)
			strResult = obj.PrintESVertrag(_ESSetting.ShowDesign)	' False)

			' Verleihvertrag druck
			_locSetting.IsPrintAsVerleih = True
			_locSetting.JobNr2Print = If(String.IsNullOrWhiteSpace(_setting.JobNr2Print), "4.2", _setting.JobNr2Print)
			Dim objVerleih As New ClsPrintESVerleihvertrag(_locSetting)
			strResult = objVerleih.PrintESVerleih(_ESSetting.ShowDesign)

		Else
			If _bAsVerleih Then
				_locSetting.IsPrintAsVerleih = True
				_locSetting.JobNr2Print = If(String.IsNullOrWhiteSpace(_setting.JobNr2Print), "4.2", _setting.JobNr2Print)

				Dim obj As New ClsPrintESVerleihvertrag(_locSetting)
				strResult = obj.PrintESVerleih(_ESSetting.ShowDesign)

			Else
				_locSetting.IsPrintAsVerleih = False
				_locSetting.JobNr2Print = If(String.IsNullOrWhiteSpace(_setting.JobNr2Print), "4.3", _setting.JobNr2Print)
				Dim obj As New ClsPrintESVertrag(_locSetting)
				strResult = obj.PrintESVertrag(_ESSetting.ShowDesign)

			End If

		End If
		m_Logger.LogDebug(String.Format("strresult: {0}", strResult))
		If strResult.ToLower.Contains("wos-success") Then
			Dim strMsg As String = m_xml.GetSafeTranslationValue("Ihre Daten wurden erfolgreich in WOS-Plattform veröffentlicht.")
			m_UtilityUi.ShowInfoDialog(strMsg)

		ElseIf strResult.ToLower.Contains("wos-error") Then
			Dim strMsg As String = m_xml.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich in WOS-Plattform veröffentlicht werden!{0}{1}")
			strMsg = String.Format(strMsg, vbNewLine, strResult)
			m_UtilityUi.ShowErrorDialog(strMsg)

		Else

		End If

		Return strResult
	End Function

	Function ExportSelectedES(ByVal sSend2WOS As Short) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim _setting As ClsESSetting = _ESSetting
		Dim iYear As Integer = 0
		Dim iMonth As Short = 0
		Dim strWhereQuery As String = String.Empty
		Dim strResult As String = "Success"
		Dim bSendPrintJob2WOS As Boolean = False
		Dim bSend_And_PrintJob2WOS As Boolean = False
		Dim strESNr As String = String.Empty

		_ESSetting.FormOpenArt = ClsESSetting.OpenFormArt.PrintingES
		If sSend2WOS = 0 Then
		ElseIf sSend2WOS = 1 Then
			bSend_And_PrintJob2WOS = True
		ElseIf sSend2WOS = 2 Then
			bSendPrintJob2WOS = True
		End If

		Try
			Dim strSqlQuery As String = GetSQL2Open
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim rFrec As SqlDataReader = cmd.ExecuteReader

			Dim bAllowedWOS As Boolean = m_md.AllowedExportEmployee2WOS(ClsDataDetail.MDData.MDNr, Now.Year)
			If Not sSend2WOS = 0 Then bAllowedWOS = False

			SelectedESNr.Clear()
			SelectedMANr.Clear()
			SelectedKDNr.Clear()
			SelectedMAData2WOS.Clear()
			SelectedKDData2WOS.Clear()
			SelectedMALang.Clear()
			SelectedKDLang.Clear()
			Do While rFrec.Read
				SelectedESNr.Add(rFrec("ESNr"))
				SelectedMANr.Add(rFrec("MANr"))
				SelectedKDNr.Add(rFrec("KDNr"))

				SelectedMAData2WOS.Add(If(sSend2WOS = 0, False, CBool(rFrec("MAWOS"))))
				SelectedKDData2WOS.Add(If(sSend2WOS = 0, False, CBool(rFrec("KDWOS"))))

				SelectedMALang.Add(CStr(rFrec("MASprache").ToString.Substring(0, 1).ToUpper))
				SelectedKDLang.Add(CStr(rFrec("KDSprache").ToString.Substring(0, 1).ToUpper))

			Loop

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Query-Abfrage: {1}", strMethodeName, ex.Message))
			Return String.Format("Error: {0}", ex.Message)

		End Try

		Try
			mESVerleihWorker = New BackgroundWorker
			mESVerleihWorker.WorkerReportsProgress = True
			mESVerleihWorker.WorkerSupportsCancellation = True
			AddHandler mESVerleihWorker.DoWork, AddressOf StartExportingWithESVertrag
			AddHandler mESVerleihWorker.RunWorkerCompleted, AddressOf StartExportingWithESVertragCompleted

			mESVerleihWorker.RunWorkerAsync()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Daten exportieren. {1}", strMethodeName, ex.Message))
			Return String.Format("Error: Daten exportieren. {0}", ex.Message)

		End Try

		Return strResult
	End Function

	Sub StartExportingWithESVertrag()
		Dim _locSetting As New ClsLLESVertragSetting

		_locSetting = New ClsLLESVertragSetting With {.DbConnString2Open = ClsDataDetail.MDData.MDDbConn, _
																																						 .SQL2Open = String.Empty, _
																																						 .JobNr2Print = "4.3", _
																							 .IsPrintAsVerleih = False, _
																																						 .liESNr2Print = Me.SelectedESNr, _
																																						 .liMANr2Print = Me.SelectedMANr, _
																																						 .liKDNr2Print = Me.SelectedKDNr, _
																																						 .liKDZHDNr2Print = Me.SelectedKDZHDNr, _
																																						 .liSendESMAData2WOS = Me.SelectedMAData2WOS, _
																																						 .liSendESKDData2WOS = Me.SelectedKDData2WOS, _
																																						 .SendAndPrintData2WOS = False, _
																																						 .liESSend2WOS = SelectedMAData2WOS, _
																																						 .LiMALang = Me.SelectedMALang, _
																																						 .LiKDLang = Me.SelectedKDLang, _
																																						 .SelectedESNr2Print = 0, _
																																						 .SelectedMANr2Print = 0,
																																							 .SelectedMDNr = ClsDataDetail.MDData.MDNr,
																																							 .LogedUSNr = ClsDataDetail.UserData.UserNr}
		Dim obj As New ClsPrintESVerleihvertrag(_locSetting)
		ResultOFExportingESVerleihRec = obj.ExportESVerleihvertrag()

	End Sub

	Sub StartExportingWithESVertragCompleted()

		If File.Exists(ResultOFExportingESVerleihRec) Then
			Dim strMsg As String = String.Format(m_xml.GetSafeTranslationValue("Ihre Daten wurden erfolgreich in {0} gespeichert."), ResultOFExportingESVerleihRec)

			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Daten exportieren"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Information)
			Process.Start(ResultOFExportingESVerleihRec)

		Else
			Dim strMsg As String = String.Format(m_xml.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gespeichert werden.{0}{1}"), _
																					 vbNewLine, ResultOFExportingESVerleihRec)

			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Daten exportieren"), _
																								 MessageBoxButtons.OK, MessageBoxIcon.Error)
		End If

	End Sub

	Function StartDeleteingSelectedES(ByVal _bShowMsg As Boolean) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."
		Dim liESNr As New List(Of Integer)

		Try
			strResult = DeleteSelectedES(_ESSetting.SelectedESNr, _bShowMsg) 'New List(Of Integer)(New Integer() {CInt(Me.txtLONr.Text)}), false)
			ResultOFDeletingESRec = strResult

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:{1}", strMethodeName, ex.Message))

		End Try

		Return strResult
	End Function

  Sub StartDeleteingSelectedESCompleted()

    If Not ResultOFDeletingESRec.ToLower.Contains("error") Then
      Dim strMsg As String = m_xml.GetSafeTranslationValue("Ihre Daten wurden erfolgreich gelöscht.")

      DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Daten löschen"), _
                                                 MessageBoxButtons.OK, MessageBoxIcon.Information)
      ' Hauptübersicht aktuallisieren...
      RunLVUpdate()

    Else
      Dim strMsg As String = String.Format(m_xml.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gelöscht werden.{0}{1}"), _
                                           vbNewLine, _
                                           ResultOFExportingESVerleihRec)
      DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Daten löschen"), _
                                                 MessageBoxButtons.OK, MessageBoxIcon.Asterisk)

    End If

  End Sub




  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub




#End Region

End Class
