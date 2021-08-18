
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions
Imports SPProgUtility.SPConverter.ClsConvert

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data

Imports SP.Infrastructure.Logging
Imports SP.LO.PrintUtility.ClsDataDetail


Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()



  Sub ListSort(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

    cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Kandidatenname")))
		cbo.Properties.Items.Add(String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Kandidatennummer")))

		cbo.Properties.Items.Add(String.Format("2 - {0}", m_Translate.GetSafeTranslationValue("Erstellt am")))

  End Sub

	Function GetMADbData4PrintLO(ByVal mdnr As Integer, ByVal iYear As Integer, ByVal iMonth As Integer) As DataTable
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim strQuery As String = "[List MAData For Search In LOPrint]"
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter
		Dim param As System.Data.SqlClient.SqlParameter

		param = cmd.Parameters.AddWithValue("@MDNr", mdnr)
		param = cmd.Parameters.AddWithValue("@iYear", iYear)
		param = cmd.Parameters.AddWithValue("@iMonth", iMonth)

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "MAData")

		Return ds.Tables(0)
	End Function

	Sub ListLOMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal _iYear As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Dim strValue As String
			Conn.Open()

			Dim strSqlQuery As String = "[List LOMonth For Search In LO Liste]"
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim pYear As SqlParameter = New SqlParameter("@iYear", SqlDbType.NVarChar, 50)

			Dim rFrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rFrec.Read

				Try
					strValue = String.Format("{0}", CInt(rFrec("LP")))
					cbo.Properties.Items.Add(strValue)

				Catch ex As Exception

				End Try

			End While

		Catch ex As Exception	' Manager
			m_logger.logError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListLOYear(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Dim strValue As String
			Conn.Open()

			Dim strSqlQuery As String = "[List LOYear For Search In LO Liste]"
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rFrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rFrec.Read

				Try
					strValue = String.Format("{0}", String.Format("{0:#####}", CDec(rFrec("Jahr").ToString)))
					cbo.Properties.Items.Add(strValue)

				Catch ex As Exception

				End Try

			End While

		Catch ex As Exception	' Manager
			m_logger.logError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListLohnNr(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit, ByVal _setting As ClsLOSetting)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim strWhereQuery As String = String.Empty
		Dim strYear As String = ConvListObject2String(_setting.SelectedYear, ",")
		Dim sMonth As String = ConvListObject2String(_setting.SelectedMonth, ",")
		Dim strMANr As String = String.Empty

		Try
			strWhereQuery = String.Format("LO.MDNr In ({0}) ", m_InitialData.MDData.MDNr)
			
			Dim strAndString As String = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If _setting.SelectedYear.Count <> 0 And strYear <> CStr(0) Then
				strWhereQuery &= String.Format("{0}LO.Jahr In ({1}) ", strAndString, strYear)
			End If

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If _setting.SelectedMonth.Count <> 0 And sMonth <> CStr(0) Then
				strWhereQuery &= String.Format("{0}LO.LP In ({1}) ", strAndString, sMonth)
			End If

			If Not IsNothing(_setting.SelectedMANr) AndAlso _setting.SelectedMANr.Count > 0 Then
				Dim manr As String = String.Empty
				For Each maNumber In _setting.SelectedMANr
					manr = If(String.IsNullOrWhiteSpace(manr), "", ",") & maNumber
				Next
				strWhereQuery &= String.Format("{0}LO.MANr In ({1}) ", strAndString, manr)
				'If _setting.SelectedMANr.Count <> 0 And strMANr <> CStr(0) Then
			End If
			'End If

			Dim strSqlQuery As String = "SELECT LO.LONr, LP, Jahr, (MA.Nachname + ', ' + MA.Vorname) As MAName, LO.CreatedOn, LO.CreatedFrom "
			strSqlQuery += "FROM LO Left Join Mitarbeiter MA On LO.MANr = MA.MANr "

			strSqlQuery = String.Format("{0} {1} {2} {3}", strSqlQuery, If(Not String.IsNullOrWhiteSpace(strWhereQuery), "Where", ""), strWhereQuery, "ORDER BY MA.Nachname ASC, MA.Vorname ASC, LO.Jahr desc, LO.LP desc")

			Dim i As Integer = 0

			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rFrec As SqlDataReader = cmd.ExecuteReader
			Dim strValue As String = String.Empty

			cbo.Properties.Items.Clear()
			While rFrec.Read
				Try
					strValue = String.Format("{0} ({1} / {2}) {3}", String.Format("{0:#####}", _
																														CDec(rFrec("LONr").ToString)), _
																													String.Format("{0:0}", rFrec("LP")), _
																													rFrec("Jahr"), _
																													rFrec("MAName").ToString)
					cbo.Properties.Items.Add(strValue, CheckState.Unchecked, True)

				Catch ex As Exception
					m_logger.logError(String.Format("{0}.SQL-Abfragefehler. {1}", strMethodeName, ex.Message))

				End Try

				i += 1
			End While
			cbo.Properties.SeparatorChar = CChar(",")
			cbo.Properties.DropDownRows = 20


		Catch ex As Exception	' Manager
			m_logger.logError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListFoundedrec(ByVal lv As DevComponents.DotNetBar.Controls.ListViewEx, ByVal _setting As ClsLOSetting)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim strWhereQuery As String = String.Empty
		Dim strMANr As String = String.Empty
		Dim strLONr As String = String.Empty
		Dim strYear As String = ConvListObject2String(_setting.SelectedYear, ",")
		Dim sMonth As String = ConvListObject2String(_setting.SelectedMonth, ",")

		Try
			lv.Items.Clear()

			If _setting.SelectedYear.Count <> 0 And strYear <> CStr(0) Then
				strWhereQuery = String.Format("LO.Jahr In ({0}) ", strYear)
			End If

			Dim strAndString As String = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If _setting.SelectedMonth.Count <> 0 And sMonth <> CStr(0) Then
				strWhereQuery &= String.Format("{0}LO.LP In ({1}) ", strAndString, sMonth)
			End If

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If Not _setting.SelectedLONr Is Nothing Then 'And _setting.SelectedLONr.Count > 0 Then
				If _setting.SelectedLONr.Count > 0 Then
					For i As Integer = 0 To _setting.SelectedLONr.Count - 1
						strLONr &= If(strLONr.Length > 0, ",", "") & _setting.SelectedLONr.Item(i)
					Next
					strWhereQuery &= String.Format("{0}LO.LONr In ({1}) ", strAndString, strLONr)
				End If
			End If

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If _setting.SelectedWOSProperty <> 2 Then
				strWhereQuery &= String.Format("{0}MA.Send2WOS = ({1}) ", strAndString, _setting.SelectedWOSProperty)
			End If

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If Not _setting.SelectedMANr Is Nothing Then
				If _setting.SelectedMANr.Count > 0 Then
					For i As Integer = 0 To _setting.SelectedMANr.Count - 1
						strMANr &= If(strMANr.Length > 0, ",", "") & _setting.SelectedMANr.Item(i)
					Next
					strWhereQuery &= String.Format("{0}LO.MANr In ({1}) ", strAndString, strMANr)
				End If
			End If

			' Mandantennummer -------------------------------------------------------------------------------------------------------
			If m_InitialData.MDData.MultiMD = 1 Then
				strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
				strWhereQuery += String.Format("{0}LO.MDNr = {1}", strAndString, m_InitialData.MDData.MDNr)
			End If

			Dim strSqlQuery As String = "SELECT LO.ID, LO.LONr, LO.MANr, IsNull(MA.Send2WOS, 0) As Send2WOS, IsNull(MA.Sprache, 'deutsch') As Sprache, "
			strSqlQuery += "(Convert(nvarchar(2), LO.LP) + ' / ' + convert(nvarchar(4), LO.Jahr)) As Zeitraum, "
			strSqlQuery += "(MA.Nachname + ', ' + MA.Vorname) As MAName, LO.CreatedOn, LO.CreatedFrom "
			strSqlQuery += "FROM LO Left Join Mitarbeiter MA On LO.MANr = MA.MANr "
			If Not String.IsNullOrWhiteSpace(strWhereQuery) Then strWhereQuery = String.Format("Where {0} ", strWhereQuery)
			Dim strSortQuery As String = GetSortString(_setting)
			strSqlQuery = String.Format("{0} {1} {2}", strSqlQuery, strWhereQuery, strSortQuery)

			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim rFrec As SqlDataReader = cmd.ExecuteReader

			' LV ausfüllen...
			Try
				Dim strColumnBez As String = "ID;LONr;MANr;bWOS;MASprache;Zeitraum;Kandidat;Erstellt durch;Erstellt am"
				Dim strColumnWidth As String = If(My.Settings.LV_ColumnWidth = String.Empty Or _
																					My.Settings.LV_ColumnWidth.Split(CChar(";")).Length <> strColumnBez.Split(CChar(";")).Length, _
																					"0-0;50-1;0-0;0-0;0-0;50-1;100-0;100-0;100-0", My.Settings.LV_ColumnWidth)
				Try
					strColumnBez = m_Translate.GetSafeTranslationValue(strColumnBez)
				Catch ex As Exception
					m_logger.logError(String.Format("{0}.Übersetzungsfehler. {1}", strMethodeName, ex.Message))
				End Try

				FillDataHeaderLv(lv, strColumnBez, strColumnWidth)
				lv.FullRowSelect = True
				lv.MultiSelect = True	' _setting.FormOpenArt = ClsLOSetting.OpenFormArt.PrintingLO

				Dim i As Integer = 0
				While rFrec.Read
					With lv
						'Dim item As DevComponents.DotNetBar.Controls.ListViewEx.ListViewItemCollection
						.Items.Add(rFrec("ID").ToString)
						.Items(i).SubItems.Add(rFrec("LONr").ToString)
						.Items(i).SubItems.Add(rFrec("MANr").ToString)
						.Items(i).SubItems.Add(If(CBool(rFrec("Send2WOS")), "1", "0"))
						.Items(i).SubItems.Add(rFrec("Sprache").ToString.Substring(0, 1).ToUpper)
						.Items(i).SubItems.Add(rFrec("Zeitraum").ToString)
						.Items(i).SubItems.Add(String.Format("{0}", rFrec("MAName").ToString))
						.Items(i).SubItems.Add(String.Format("{0}", rFrec("CreatedFrom")))
						.Items(i).SubItems.Add(String.Format("{0}", rFrec("CreatedOn"), "d"))
					End With

					i += 1
				End While

			Catch ex As Exception
				m_logger.logError(String.Format("{0}.SQL-Abfragefehler. {1}", strMethodeName, ex.Message))

			End Try


		Catch ex As Exception	' Manager
			m_logger.logError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListFoundedLODetailrec(ByVal lv As DevComponents.DotNetBar.Controls.ListViewEx, ByVal _setting As ClsLOSetting)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim strWhereQuery As String = String.Empty
		Dim strMANr As String = String.Empty
		Dim strLONr As String = String.Empty
		Dim strYear As String = ConvListObject2String(_setting.SelectedYear, ",")
		Dim sMonth As String = ConvListObject2String(_setting.SelectedMonth, ",")

		Try
			lv.Items.Clear()

			If _setting.SelectedYear.Count <> 0 And strYear <> CStr(0) Then
				strWhereQuery = String.Format("LOL.Jahr In ({0}) ", strYear)
			End If

			Dim strAndString As String = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If _setting.SelectedMonth.Count <> 0 And sMonth <> CStr(0) Then
				strWhereQuery &= String.Format("{0}LOL.LP In ({1}) ", strAndString, sMonth)
			End If

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If Not _setting.SelectedLONr Is Nothing And _setting.SelectedLONr.Count > 0 Then
				For i As Integer = 0 To _setting.SelectedLONr.Count - 1
					strLONr &= If(strLONr.Length > 0, ",", "") & _setting.SelectedLONr.Item(i)
				Next
				strWhereQuery &= String.Format("{0}LOL.LONr In ({1}) ", strAndString, strLONr)
			End If

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If _setting.SelectedWOSProperty <> 2 Then
				strWhereQuery &= String.Format("{0}MA.Send2WOS = ({1}) ", strAndString, _setting.SelectedWOSProperty)
			End If

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If Not _setting.SelectedMANr Is Nothing And _setting.SelectedMANr.Count > 0 Then
				For i As Integer = 0 To _setting.SelectedMANr.Count - 1
					strMANr &= If(strMANr.Length > 0, ",", "") & _setting.SelectedMANr.Item(i)
				Next
				strWhereQuery &= String.Format("{0}LOL.MANr In ({1}) ", strAndString, strMANr)
			End If

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			strWhereQuery &= String.Format("{0}(LOL.RPText <> '' And LOL.RPText Is Not Null) ", strAndString)

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If _setting.ShowNullBetrag Then strWhereQuery &= String.Format("{0}(LOL.m_Btr <> 0) ", strAndString)

			' Mandantennummer -------------------------------------------------------------------------------------------------------
			If m_InitialData.MDData.MultiMD = 1 Then
				strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
				strWhereQuery += String.Format("{0}LOL.MDNr = {1}", strAndString, m_InitialData.MDData.MDNr)
			End If

			Dim strSqlQuery As String = "SELECT LOL.ID, LOL.LONr, LOL.MANr, "
			strSqlQuery += "LOL.m_Anz As Anzahl, LOL.m_Bas As Basis, LOL.m_Ans As Ansatz, LOL.m_btr As Betrag, "
			strSqlQuery += "(Convert(nvarchar(2), LOL.LP) + ' / ' + convert(nvarchar(4), LOL.Jahr)) As Zeitraum, "
			strSqlQuery += "(MA.Nachname + ', ' + MA.Vorname) As MAName, LOL.LANr, LOL.RPText "
			strSqlQuery += "FROM LOL Left Join Mitarbeiter MA On LOL.MANr = MA.MANr "
			If Not String.IsNullOrWhiteSpace(strWhereQuery) Then strWhereQuery = String.Format("Where {0} ", strWhereQuery)
			strSqlQuery = String.Format("{0} {1} Order By MA.Nachname ASC, MA.Vorname ASC, LOL.LONr, LOL.Jahr Desc, LOL.LP Desc, LOL.LANr ASC", _
																	strSqlQuery, strWhereQuery)

			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim rFrec As SqlDataReader = cmd.ExecuteReader

			Dim strColumnBez As String = m_Translate.GetSafeTranslationValue("ID;LONr;MANr;Zeitraum;Kandidat;LANr;Bezeichnung;Anzahl;Basis;Ansatz;Betrag")
			Dim strColumnWidth As String = If(My.Settings.LV_ColumnWidth = String.Empty Or _
																				My.Settings.LV_ColumnWidth.Split(CChar(";")).Length <> strColumnBez.Split(CChar(";")).Length, _
																				"0-0;50-1;0-0;50-0;100-0;50-1;200-0;50-1;50-1;50-1;100-1", My.Settings.LV_ColumnWidth)
			FillDataHeaderLv(lv, strColumnBez, strColumnWidth)
			lv.FullRowSelect = True
			lv.MultiSelect = False

			Try
				Dim i As Integer = 0
				While rFrec.Read
					With lv
						'Dim item As DevComponents.DotNetBar.Controls.ListViewEx.ListViewItemCollection
						.Items.Add(rFrec("ID").ToString)
						.Items(i).SubItems.Add(rFrec("LONr").ToString)
						.Items(i).SubItems.Add(rFrec("MANr").ToString)
						.Items(i).SubItems.Add(rFrec("Zeitraum").ToString)
						.Items(i).SubItems.Add(String.Format("{0}", rFrec("MAName").ToString))
						.Items(i).SubItems.Add(String.Format("{0}", Format(rFrec("LANr"), "f3")))
						.Items(i).SubItems.Add(String.Format("{0}", rFrec("RPText")))
						.Items(i).SubItems.Add(String.Format("{0}", Format(rFrec("Anzahl"), "n")))
						.Items(i).SubItems.Add(String.Format("{0}", Format(rFrec("Basis"), "n")))
						.Items(i).SubItems.Add(String.Format("{0}", Format(rFrec("Ansatz"), "n")))
						.Items(i).SubItems.Add(String.Format("{0}", Format(rFrec("Betrag"), "n")))
					End With

					i += 1
				End While

			Catch ex As Exception
				m_logger.logError(String.Format("{0}.SQL-Abfragefehler. {1}", strMethodeName, ex.Message))

			End Try


		Catch ex As Exception	' Manager
			m_logger.logError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub


  Function GetSortString(ByVal _setting As ClsLOSetting) As String
    Dim strSort As String = " Order by "
    Dim strSortBez As String = String.Empty
    Dim strName As String()
    Dim strMyName As String = String.Empty

    '0 - Kandidatennname
    '1 - Kandidatennnummer
    '2 - Erstellt am

    'strName = Regex.Split(cbo.Text.Trim, ",")
    strName = Regex.Split(_setting.SortBezeichnung.Trim, ",")
    strMyName = String.Empty
    If _setting.SortBezeichnung = String.Empty Then Return String.Format("{0} MA.Nachname ASC, MA.Vorname ASC, LO.Jahr DESC, LO.LP Desc, LO.CreatedOn Desc ", strSort)

    If strName(0).Contains("-") Then
      For i As Integer = 0 To strName.Length - 1
        Select Case CInt(Val(Left(strName(i).ToString.Trim, 1))) ' Das erste Zeichen der Sortierung
          Case 0          ' Nach Kandidatenname
            strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MA.Nachname ASC, MA.Vorname ASC, LO.Jahr DESC, LO.LP Desc, LO.CreatedOn Desc "

          Case 1          ' Nach Kandidatennummer
            strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " LO.MANr "

          Case 2          ' Erstellt am
            strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " LO.CreatedOn "

          Case Else
            strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & " MA.Nachname ASC, MA.Vorname ASC, LO.Jahr DESC, LO.LP Desc, LO.CreatedOn Desc "

        End Select
      Next i
    Else
      'strMyName = cbo.Text
			strSortBez = m_Translate.GetSafeTranslationValue("Benutzerdefiniert")
    End If


    If strMyName.Trim = "" Then
      Return String.Format("{0} MA.Nachname ASC, MA.Vorname ASC, LO.Jahr DESC, LO.LP Desc, LO.CreatedOn Desc ", strSort)
    Else
      strSort = String.Format("{0} {1} ", strSort, strMyName)
      Return strSort
    End If

  End Function


#Region "Sonstige Funktions..."

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

      For i = 0 To strCaption.Length - 1
        lvwColumn = New ColumnHeader()
        lvwColumn.Text = strCaption(i).ToString
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

  Sub SetRowColor(ByVal LV As ListView, ByVal iIndex As Integer, ByVal bDiffColor As Boolean)

    For j As Integer = 0 To LV.Items(iIndex).SubItems.Count - 1
      LV.Items(iIndex).BackColor = If(bDiffColor, Color.Yellow, LV.BackColor)
    Next j

  End Sub

#End Region


End Module
