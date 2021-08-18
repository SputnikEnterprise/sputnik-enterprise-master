
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SP.Infrastructure.Logging

Imports SPProposeUtility.ClsDataDetail


Module FuncLv

  Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Dim _ClsLog As New SPProgUtility.ClsEventLog
  Private m_xml As New ClsXML



	Public Enum Language
		German = 0
		Italian = 1
		French = 2
		English = 3
	End Enum

  Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

    Dim m_md As New SPProgUtility.Mandanten.Mandant
    Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
    Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
    Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

    Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
    Dim translate = clsTransalation.GetTranslationInObject

    Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

  End Function


  Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
    Dim lstStuff As ListViewItem = New ListViewItem()
    Dim lvwColumn As ColumnHeader

    With Lv
      .Clear()

      ' Nr;Nummer;Name;Strasse;PLZ Ort
      If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
      If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

      Dim strCaption As String() = Regex.Split(strColumnList, ";")
      ' 0-1;0-1;2000-0;2000-0;2500-0
      Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
      Dim strFieldWidth As String
      Dim strFieldAlign As String = "0"
      Dim strFieldData As String()

      For i As Integer = 0 To strCaption.Length - 1
        lvwColumn = New ColumnHeader()
        lvwColumn.Text = m_Translate.GetSafeTranslationValue(strCaption(i).ToString)
        strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

        If strFieldInfo(i).ToString.StartsWith("-") Then
          strFieldWidth = strFieldData(1)
          lvwColumn.Width = CInt(strFieldWidth) * -1
          If strFieldData.Count > 1 Then
            strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
          End If
        Else
          strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
          lvwColumn.Width = CInt(strFieldWidth) '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
          If strFieldData.Count > 1 Then
            strFieldAlign = strFieldData(1)
          End If
        End If
        If strFieldAlign = "1" Then
          lvwColumn.TextAlign = HorizontalAlignment.Right
        ElseIf strFieldAlign = "2" Then
          lvwColumn.TextAlign = HorizontalAlignment.Center
        Else
          lvwColumn.TextAlign = HorizontalAlignment.Left

        End If
        lstStuff.BackColor = Color.Yellow
        .Columns.Add(lvwColumn)
      Next

      lvwColumn = Nothing
    End With

  End Sub

#Region "Funktionen für LV Füllen..."

  Sub SetLvwHeader(ByVal Lv As ListView)
    Dim strColumnString As String = String.Empty
    Dim strColumnWidthInfo As String = String.Empty
    Dim strUSLang As String = _ClsProgSetting.GetUSLanguage
    Dim strQuery As String = String.Empty
    Dim strBez As String = String.Empty

    If Lv.Name.ToLower.Contains("Vorstellung".ToLower) Then
      strColumnString = "ID;RecNr;Datum;Betreff;Ergebnis;Status"

      strColumnWidthInfo = "0-0;0-0;110-0;200-0;200-0;100-0"
      If My.Settings.LV_5_Size_SPProposeUtility <> String.Empty Then strColumnWidthInfo = My.Settings.LV_5_Size_SPProposeUtility

    ElseIf Lv.Name.ToLower.Contains("MAKontakt".ToLower) Then
      strColumnString = "ID;RecNr;Datum;Beschreibung;Betreff;Erledigt"

      strColumnWidthInfo = "0-0;0-0;110-0;400-0;100-0;100-0"
      If My.Settings.LV_6_Size_SPProposeUtility <> String.Empty Then strColumnWidthInfo = My.Settings.LV_6_Size_SPProposeUtility

    ElseIf Lv.Name.ToLower.Contains("KDKontakt".ToLower) Then
      strColumnString = "ID;RecNr;Datum;Beschreibung;Betreff;Erledigt"
      strColumnWidthInfo = "0-0;0-0;110-0;400-0;100-0;100-0"
      If My.Settings.LV_7_Size_SPProposeUtility <> String.Empty Then strColumnWidthInfo = My.Settings.LV_7_Size_SPProposeUtility

    End If
    FillDataHeaderLv(Lv, m_Translate.GetSafeTranslationValue(strColumnString), strColumnWidthInfo)

  End Sub

  Sub FillVorstellungLV(ByVal Lv As ListView, ByVal iRecNr As Integer)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim i As Integer = 0
		Dim _ClsDb As New ClsDbFunc
		Dim strQuery As String = String.Empty

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			strQuery = _ClsDb.GetVorstellungSQLString(iRecNr)
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			If iRecNr > 0 Then
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@iRecNr", iRecNr)
			End If
			Trace.WriteLine(strQuery)

			Dim rAdressrec As SqlDataReader = cmd.ExecuteReader					 ' Vorstellungstermine
			Lv.Items.Clear()
			Lv.FullRowSelect = True
			If Lv.Columns.Count = 0 Then SetLvwHeader(Lv)

			Dim Time_1 As Double = System.Environment.TickCount

			Lv.BeginUpdate()
			While rAdressrec.Read
				With Lv
					.Items.Add(rAdressrec("ID").ToString)
					.Items(i).SubItems.Add(rAdressrec("RecNr").ToString)
					.Items(i).SubItems.Add(Format(rAdressrec("TerminDate"), "g"))
					.Items(i).SubItems.Add(rAdressrec("JobTitel").ToString)
					.Items(i).SubItems.Add(rAdressrec("Ergebnis").ToString)
					.Items(i).SubItems.Add(rAdressrec("JobTerminStatus").ToString)

				End With

				i += 1
			End While
			Lv.EndUpdate()
			Console.WriteLine(String.Format("Zeit für FillVorstellungLV: {0} s", _
																			((System.Environment.TickCount - Time_1) / 1000).ToString()))


		Catch e As Exception
			Lv.Items.Clear()
			MsgBox(e.Message, MsgBoxStyle.Critical, "FillVorstellungLV")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub FillMAKontaktLV(ByVal Lv As ListView, ByVal iRecNr As Integer)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim i As Integer = 0
		Dim _ClsDb As New ClsDbFunc
		Dim strQuery As String = String.Empty

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			strQuery = _ClsDb.GetMAKontaktSQLString(iRecNr)
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			If iRecNr > 0 Then
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@iRecNr", iRecNr)
			End If
			Trace.WriteLine(strQuery)

			Dim rAdressrec As SqlDataReader = cmd.ExecuteReader					 ' Vorstellungstermine
			Lv.Items.Clear()
			Lv.FullRowSelect = True
			If Lv.Columns.Count = 0 Then SetLvwHeader(Lv)

			Dim Time_1 As Double = System.Environment.TickCount

			Lv.BeginUpdate()
			While rAdressrec.Read
				With Lv
					.Items.Add(rAdressrec("ID").ToString)
					.Items(i).SubItems.Add(rAdressrec("RecNr").ToString)
					.Items(i).SubItems.Add(Format(rAdressrec("KontaktDate"), "g"))
					.Items(i).SubItems.Add(rAdressrec("Kontakte").ToString)
					.Items(i).SubItems.Add(rAdressrec("KontaktDauer").ToString)
					.Items(i).SubItems.Add(If(String.IsNullOrEmpty(rAdressrec("KontaktErledigt").ToString), "Nicht erledigt", "Erledigt"))

				End With

				i += 1
			End While
			Lv.EndUpdate()
			Console.WriteLine(String.Format("Zeit für FillMAKontaktLV: {0} s", _
																			((System.Environment.TickCount - Time_1) / 1000).ToString()))


		Catch e As Exception
			Lv.Items.Clear()
			MsgBox(e.Message, MsgBoxStyle.Critical, "FillMAKontaktLV")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub FillKDKontaktLV(ByVal Lv As ListView, ByVal iRecNr As Integer)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim i As Integer = 0
		Dim _ClsDb As New ClsDbFunc
		Dim strQuery As String = String.Empty

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			strQuery = _ClsDb.GetKDKontaktSQLString(iRecNr)
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			If iRecNr > 0 Then
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@iRecNr", iRecNr)
			End If
			Trace.WriteLine(strQuery)

			Dim rAdressrec As SqlDataReader = cmd.ExecuteReader					 ' Vorstellungstermine
			Lv.Items.Clear()
			Lv.FullRowSelect = True
			If Lv.Columns.Count = 0 Then SetLvwHeader(Lv)

			Dim Time_1 As Double = System.Environment.TickCount

			Lv.BeginUpdate()
			While rAdressrec.Read
				With Lv
					.Items.Add(rAdressrec("ID").ToString)
					.Items(i).SubItems.Add(rAdressrec("RecNr").ToString)
					.Items(i).SubItems.Add(Format(rAdressrec("KontaktDate"), "g"))
					.Items(i).SubItems.Add(rAdressrec("Kontakte").ToString)
					.Items(i).SubItems.Add(rAdressrec("KontaktDauer").ToString)
					.Items(i).SubItems.Add(If(String.IsNullOrEmpty(rAdressrec("KontaktErledigt").ToString), "Nicht erledigt", "Erledigt"))

				End With

				i += 1
			End While
			Lv.EndUpdate()
			Console.WriteLine(String.Format("Zeit für FillKDKontaktLV: {0} s", _
																			((System.Environment.TickCount - Time_1) / 1000).ToString()))


		Catch e As Exception
			Lv.Items.Clear()
			MsgBox(e.Message, MsgBoxStyle.Critical, "FillKDKontaktLV")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function GetESRENr4Propose(ByVal iProposeNr As Integer) As Integer
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim iResult As Integer = 0
		Dim _ClsDb As New ClsDbFunc
		Dim strQuery As String = String.Empty

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			strQuery = _ClsDb.GetESSQLString(iProposeNr)
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			If iProposeNr > 0 Then
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@iRecNr", iProposeNr)
			End If
			Trace.WriteLine(strQuery)

			Dim rAdressrec As SqlDataReader = cmd.ExecuteReader					 ' Vorstellungstermine
			Dim Time_1 As Double = System.Environment.TickCount

			rAdressrec.Read()
			If rAdressrec.HasRows Then iResult = CInt(Val(rAdressrec("ESNr").ToString)) + CInt(Val(rAdressrec("RENr").ToString))

			Console.WriteLine(String.Format("Zeit für GetESRENr4Propose: {0} s", _
																			((System.Environment.TickCount - Time_1) / 1000).ToString()))


		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "GetESRENr4Propose")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Function

#End Region


#Region "Funktionen zur Mailmodul..."

	Function GetUserDaten(ByVal iPNr As Integer, ByVal iUSNr As Integer) As List(Of String)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim liResult As New List(Of String)
		Dim _ClsDb As New ClsDbFunc
		Dim strQuery As String = String.Empty

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			strQuery = _ClsDb.GetUserSQLString()
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@PNr", iPNr)
			param = cmd.Parameters.AddWithValue("@USNr", iUSNr)

			Dim rAdressrec As SqlDataReader = cmd.ExecuteReader
			Dim Time_1 As Double = System.Environment.TickCount

			rAdressrec.Read()
			If rAdressrec.HasRows Then
				liResult.Add(String.Format("{0}", rAdressrec("USeMail").ToString))
				liResult.Add(String.Format("{0}", rAdressrec("MDeMail").ToString))
				liResult.Add(String.Format("{0}", rAdressrec("USPeMail").ToString))
			End If
			Console.WriteLine(String.Format("Zeit für GetUserDaten: {0} s", _
																			((System.Environment.TickCount - Time_1) / 1000).ToString()))


		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "GetUserDaten")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return liResult
	End Function

	Function GetKandidatenDaten(ByVal iMANr As Integer) As List(Of String)
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim liResult As New List(Of String)
		Dim _ClsDb As New ClsDbFunc
		Dim strQuery As String = String.Empty

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			strQuery = _ClsDb.GetMASQLString(iMANr)
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@iMANr", iMANr)

			Dim rAdressrec As SqlDataReader = cmd.ExecuteReader
			Dim Time_1 As Double = System.Environment.TickCount

			rAdressrec.Read()
			If rAdressrec.HasRows Then
				liResult.Add(String.Format("{0}#{1}#{2}", rAdressrec("MAeMail").ToString, _
																	 rAdressrec("MAVorname").ToString, rAdressrec("MANachname").ToString))
			End If
			Console.WriteLine(String.Format("Zeit für GetKandidatenDaten: {0} s", _
																			((System.Environment.TickCount - Time_1) / 1000).ToString()))


		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "GetKandidatenDaten")

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return liResult
	End Function

	Function TranslateSeletedTemplate(ByVal rtfControl As DevExpress.XtraRichEdit.RichEditControl) As String
		Dim _regex As New ClsDivFunc
		Dim sSql As String = String.Empty
		Dim Conn As SqlConnection
		Dim strResult As String = String.Empty

		' Kundendaten suchen...
		sSql = "[Get All ProposeData 4 eMail From Propse]"
		Conn = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()
			Dim SQLDocCmd As SqlCommand = New SqlCommand(sSql, Conn)
			SQLDocCmd.CommandType = CommandType.StoredProcedure
			Dim param As SqlParameter
			param = SQLDocCmd.Parameters.AddWithValue("@PNr", ClsDataDetail.GetProposalNr)
			SQLDocCmd.Connection = Conn
			Dim rFoundedrec As SqlDataReader = SQLDocCmd.ExecuteReader

			While rFoundedrec.Read
				With _regex
					_regex.PMANr = ClsDataDetail.GetColumnTextStr(rFoundedrec, "MANr", 0)
					_regex.PKDNr = ClsDataDetail.GetColumnTextStr(rFoundedrec, "KDNr", 0)
					_regex.PKDzNr = ClsDataDetail.GetColumnTextStr(rFoundedrec, "KDZHDNr", 0)

					_regex.KdzAnredeform = rFoundedrec("KDzAnredeForm")
					_regex.KdzNachname = rFoundedrec("KDzNachname")
					_regex.KdzVorname = rFoundedrec("KDzVorname")

					' Kandidatendaten
					_regex.MANachname = ClsDataDetail.GetColumnTextStr(rFoundedrec, "MANachname", "")
					_regex.MAVorname = ClsDataDetail.GetColumnTextStr(rFoundedrec, "MAVorname", "")
					_regex.MAGeschlecht = ClsDataDetail.GetColumnTextStr(rFoundedrec, "MAGeschlecht", "")
					_regex.MAGebDat = ClsDataDetail.GetColumnTextStr(rFoundedrec, "MAGebDat", "")
					_regex.MAAlter = ClsDataDetail.GetColumnTextStr(rFoundedrec, "MAAlter", "")
					_regex.MABeruf = ClsDataDetail.GetColumnTextStr(rFoundedrec, "MABeruf", "")

					_regex.MARes1 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes1", "")
					_regex.MARes2 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes2", "")
					_regex.MARes3 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes3", "")
					_regex.MARes4 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes4", "")
					_regex.MARes5 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes5", "")
					_regex.MARes6 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes6", "")
					_regex.MARes7 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes7", "")
					_regex.MARes8 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes8", "")
					_regex.MARes9 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes9", "")
					_regex.MARes10 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes10", "")
					_regex.MARes11 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes11", "")
					_regex.MARes12 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes12", "")
					_regex.MARes13 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes13", "")
					_regex.MARes14 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes14", "")
					_regex.MARes15 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "LLRes15", "")

					' Vorschlagdaten
					_regex.PBez = ClsDataDetail.GetColumnTextStr(rFoundedrec, "Bezeichnung", "")
					_regex.PArbBegin = ClsDataDetail.GetColumnTextStr(rFoundedrec, "P_Arbbegin", "")
					_regex.PESAls = ClsDataDetail.GetColumnTextStr(rFoundedrec, "MA_ESAls", "")
					_regex.PKDTarif = ClsDataDetail.GetColumnTextStr(rFoundedrec, "KD_Tarif", "")
					Dim aValue As String() = ClsDataDetail.GetColumnTextStr(rFoundedrec, "Berater", "").ToString.Split(CChar("/"))
					_regex.PBerater1 = aValue(0).ToString
					_regex.PBerater2 = If(aValue.Length > 1, aValue(1).ToString, String.Empty) ' rFoundedrec("Bezeichnung")
					_regex.PZusatz1 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "_P_Zusatz1", "")
					_regex.PZusatz2 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "_P_Zusatz2", "")
					_regex.PZusatz3 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "_P_Zusatz3", "")
					_regex.PZusatz4 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "_P_Zusatz4", "")
					_regex.PZusatz5 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "_P_Zusatz5", "")

					' Mandantendaten
					_regex.USMDname = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPMDName1", "")
					_regex.USMDname2 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPMDName2", "")
					_regex.USMDname3 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPMDName3", "")
					_regex.USMDPostfach = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPostfach", "")
					_regex.USMDStrasse = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPStrasse", "")
					_regex.USMDLand = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPLand", "")
					_regex.USMDPlz = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPLZ", "")
					_regex.USMDOrt = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPOrt", "")
					_regex.USMDTelefon = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPTelefon", "")
					_regex.USMDTelefax = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPTelefax", "")
					_regex.USMDeMail = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPeMail", "")
					_regex.USMDHomepage = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPHomepage", "")

					' Private Daten
					_regex.USNachname = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPNachname", "")
					_regex.USVorname = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPVorname", "")
					_regex.USTitel_1 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPTitel_1", "")
					_regex.USTitel_2 = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPTitel_2", "")
					_regex.USAbteilung = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPAbteilung", "")

					_regex.USPostfach = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPPostfach", "")
					_regex.USStrasse = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPStrasse", "")
					_regex.USLand = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPLand", "")
					_regex.USPLZ = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPPLZ", "")
					_regex.USOrt = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPOrt", "")
					_regex.USNatel = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPNatel", "")
					_regex.USTelefon = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPTelefon", "")
					_regex.USTelefax = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPTelefax", "")
					_regex.USeMail = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPeMail", "")
					_regex.USHomepage = ClsDataDetail.GetColumnTextStr(rFoundedrec, "USPPHomepage", "")

				End With

			End While
			_regex.ParseTemplateFile(rtfControl)
			strResult = rtfControl.RtfText


		Catch ex As Exception
			MsgBox(String.Format("***Fehler (TranslateSeletedTemplate): {0}", ex.Message))
			_ClsLog.WriteTempLogFile(String.Format("***Fehler (TranslateSeletedTemplate): {0}", ex.Message))

		Finally
			Conn.Close()

		End Try

		Return strResult
	End Function

#End Region



	Sub ListAllKanton(ByVal cbo As ComboBox)
		Dim strFieldName As String = "Kanton"

		Dim strSqlQuery As String = "Select PLZ.Kanton From PLZ Where Land = 'CH' and len(PLZ.Kanton) = 2 "
		strSqlQuery += "Group By PLZ.Kanton Order By PLZ.Kanton"

		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader					 ' PLZ-Datenbank

			cbo.Items.Clear()
			While rKDrec.Read
				cbo.Items.Add(rKDrec(strFieldName).ToString)
				iWidth = CInt(IIf(iWidth > CInt(Len(rKDrec(strFieldName).ToString)), iWidth, CInt(Len(rKDrec(strFieldName).ToString))))

				i += 1
			End While
			cbo.DropDownWidth = CInt((iWidth * 7) + 20)

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub


	''' <summary>
	''' Replaces a missing object with another object.
	''' </summary>
	''' <param name="obj">The object.</param>
	''' <param name="replacementObject">The replacement object.</param>
	''' <returns>The object or the replacement object it the object is nothing.</returns>
	Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function


#Region "Sonstige Funktions..."

	Function ListBerater() As List(Of String)
		Dim strSqlQuery As String = "[List Aktive Benutzer]"
		Dim liResult As New List(Of String)
		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			While rESrec.Read
				liResult.Add(String.Format("{0}, {1} ({2})", rESrec("Nachname").ToString, rESrec("Vorname"), rESrec("KST")))

			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return liResult
	End Function

	Function ListKDZHD(ByVal iKDNr As Integer) As List(Of String)
		Dim strSqlQuery As String = "[Show KDZuStellData For ES]"
		Dim liResult As New List(Of String)
		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@KDNumber", iKDNr)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			While rESrec.Read
				liResult.Add(String.Format("{0}, {1} ({2})", rESrec("Nachname").ToString, rESrec("Vorname"), rESrec("RecNr")))

			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return liResult
	End Function

	Function GetMAInfo(ByVal iMANr As Integer) As String
		Dim strSqlQuery As String = "[List MAData For Select In Propose]"
		Dim strResult As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@Nachname", String.Empty)
			cmd.Parameters.AddWithValue("@Filiale1", _ClsProgSetting.GetUSFiliale)
			cmd.Parameters.AddWithValue("@MANr", iMANr)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			While rESrec.Read
				strResult = String.Format("{0}, {1}", rESrec("Nachname").ToString, rESrec("Vorname"))
				Exit While
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function

	Function GetKDInfo(ByVal iKDNr As Integer) As String
		Dim strSqlQuery As String = "[List KDData For Search In ES]"
		Dim strResult As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@Firma", String.Empty)
			cmd.Parameters.AddWithValue("@Filiale1", _ClsProgSetting.GetUSFiliale)
			cmd.Parameters.AddWithValue("@KDNr", iKDNr)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			While rESrec.Read
				strResult = String.Format("{0}", rESrec("Firma1").ToString)
				Exit While
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function

	Function GetVakInfo(ByVal iVakNr As Integer, ByVal iKDNr As Integer) As String
		Dim strSqlQuery As String = "[List VakData For Search In Propose]"
		Dim strResult As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@Bezeichnung", String.Empty)
			cmd.Parameters.AddWithValue("@KDNr", iKDNr)
			cmd.Parameters.AddWithValue("@VakNr", iVakNr)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			While rESrec.Read
				strResult = String.Format("{0}", rESrec("Bezeichnung").ToString)
				Exit While
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function

	Function ListAllData(ByVal strTableName As String) As List(Of String)
		Dim userLanguage As String = MapLanguageToShortLanguageCode(m_InitialData.UserData.UserLanguage)
		Dim strSqlQuery As String = String.Format("Select Bez_{0} From {1} Order By Bez_{0}", userLanguage, strTableName)
		Dim liResult As New List(Of String)
		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			While rESrec.Read
				liResult.Add(String.Format("{0}", rESrec(String.Format("Bez_{0}", userlanguage)).ToString))

			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return liResult
	End Function


	Function MapLanguageToShortLanguageCode(ByVal language As String) As String

		Select Case language
			Case "de", "d"
				Return "D"
			Case "it", "i"
				Return "I"
			Case "fr", "f"
				Return "F"
			Case "en", "e"
				Return "E"

			Case Else
				Return "D"
		End Select

	End Function



#End Region


End Module
