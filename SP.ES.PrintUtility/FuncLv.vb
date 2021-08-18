
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions
Imports SPProgUtility.SPConverter.ClsConvert

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports SP.Infrastructure.UI.UtilityUI
Imports SPProgUtility.MainUtilities.Utilities


Imports SP.Infrastructure.Logging

Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_xml As New ClsXML
	Private _ClsFunc As New ClsDivFunc
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private m_utility As New SP.Infrastructure.UI.UtilityUI
  Private m_proputility As New SPProgUtility.MainUtilities.Utilities


  Function GetMADbData4PrintLO(ByVal _strGAVNr As String, ByVal _strGAVKanton As String, _
															 ByVal _iYear As Integer, ByVal _iMonth As Integer) As DataTable
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim strQuery As String = "[List ESData For Search In ESPrint]"
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter
		Dim param As System.Data.SqlClient.SqlParameter

		param = cmd.Parameters.AddWithValue("@GAVBez", _strGAVNr)
		param = cmd.Parameters.AddWithValue("@GAVKanton", _strGAVKanton)
		param = cmd.Parameters.AddWithValue("@iMonth", _iMonth)
		param = cmd.Parameters.AddWithValue("@iYear", _iYear)

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "ESData")

		Return ds.Tables(0)
	End Function

	Sub ListLOMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal _iYear As String)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

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
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListLOYear(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Dim strValue As String
			Conn.Open()

			'Dim strSqlQuery As String = "[List LOYear For Search In LO Liste]"

			Dim strSqlQuery As String = "Select Jahr From Mandanten Where MDNr = @MDNr Group By Jahr Order By Jahr Desc"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.MDData.MDNr)

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
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListPVLBez(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal _setting As ClsESSetting)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim strWhereQuery As String = String.Empty
		Dim strSql As String = String.Empty

		Try
			If _setting.SelectedYear.Count <> 0 And _setting.SelectedMonth.Count <> 0 Then
				strSql = "[List PVLData With Month For Search In ESPrint]"
			Else
				strSql = "[List PVLData Without Month For Search In ESPrint]"
			End If
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@GAVKanton", _setting.SelectedKanton)
			If _setting.SelectedYear.Count <> 0 And _setting.SelectedMonth.Count <> 0 Then
				param = cmd.Parameters.AddWithValue("@iMonth", _setting.SelectedMonth(0))
				param = cmd.Parameters.AddWithValue("@iYear", _setting.SelectedYear(0))
			End If

			Dim rFrec As SqlDataReader = cmd.ExecuteReader
			Dim strValue As String = String.Empty

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add(String.Empty)
			While rFrec.Read
				Try
					strValue = String.Format("{0}", rFrec("GAVGruppe0").ToString)
					cbo.Properties.Items.Add(strValue) ', CheckState.Unchecked, True)

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.SQL-Abfragefehler. {1}", strMethodeName, ex.Message))

				End Try

			End While
			'cbo.Properties.SeparatorChar = CChar(",")
			cbo.Properties.DropDownRows = 20


		Catch ex As Exception	' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListPVLKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal _setting As ClsESSetting)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim strWhereQuery As String = String.Empty
		Dim strMANr As String = String.Empty
		Dim strSql As String = String.Empty

		Try
			If _setting.SelectedYear.Count <> 0 And _setting.SelectedMonth.Count <> 0 Then
				strSql = "[List PVLKantonData With Month For Search In ESPrint]"
			Else
				strSql = "[List PVLKantonData Without Month For Search In ESPrint]"
			End If
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@GAVGruppe0", _setting.SelectedPVLBez)
			If _setting.SelectedYear.Count <> 0 And _setting.SelectedMonth.Count <> 0 Then
				param = cmd.Parameters.AddWithValue("@iMonth", _setting.SelectedMonth(0))
				param = cmd.Parameters.AddWithValue("@iYear", _setting.SelectedYear(0))
			End If

			Dim rFrec As SqlDataReader = cmd.ExecuteReader
			Dim strValue As String = String.Empty

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add(String.Empty)
			While rFrec.Read
				Try
					strValue = String.Format("{0}", rFrec("GAVKanton").ToString)
					cbo.Properties.Items.Add(strValue) ', CheckState.Unchecked, True)

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.SQL-Abfragefehler. {1}", strMethodeName, ex.Message))

				End Try

			End While
			cbo.Properties.DropDownRows = 20


		Catch ex As Exception	' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListFoundedrec(ByVal lv As DevComponents.DotNetBar.Controls.ListViewEx, ByVal _setting As ClsESSetting)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		Dim strWhereQuery As String = String.Empty
		Dim dBegin As Date
		Dim dEnd As Date
		Dim strESNr As String = ""

		Try
			If _setting.SelectedYear.Count <> 0 Then
				If _setting.SelectedMonth.Count <> 0 Then
					dBegin = CDate("01." & _setting.SelectedMonth(0) & "." & _setting.SelectedYear(0))
					dEnd = DateAdd("d", -1, DateAdd("m", 1, dBegin))

				Else
					dBegin = CDate("01.01." & _setting.SelectedYear(0))
					dEnd = CDate("31.12." & _setting.SelectedYear(0))

				End If

				'((ES.ES_Ende >= @dMonthBegin Or ES.ES_Ende Is Null) And ES.ES_Ab <= @dMonthEnd) 
				strWhereQuery = String.Format("((ES.ES_Ende >= '{0}' Or ES.ES_Ende Is Null) And ES.ES_Ab <= '{1}') ", Format(dBegin, "d"), Format(dEnd, "d"))
			End If

			Dim strAndString As String = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If _setting.SelectedPVLBez <> String.Empty Then
				strWhereQuery &= String.Format("{0}ESL.GAVGruppe0 = '{1}' ", strAndString, _setting.SelectedPVLBez)
			End If

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If _setting.SelectedKanton <> String.Empty Then
				strWhereQuery &= String.Format("{0}ESL.GAVKanton = '{1}' ", strAndString, _setting.SelectedKanton)
			End If

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If Not _setting.SelectedESNr Is Nothing And _setting.SelectedESNr(0) <> 0 Then
				For i As Integer = 0 To _setting.SelectedESNr.Count - 1
					strESNr &= If(strESNr.Length > 0, ",", "") & _setting.SelectedESNr.Item(i)
				Next
				strWhereQuery &= String.Format("{0}ESL.ESNr In ({1}) ", strAndString, strESNr)
			End If

			strAndString = IIf(strWhereQuery <> String.Empty, " And ", String.Empty).ToString
			If _setting.SelectedWOSProperty <> 2 Then
				strWhereQuery &= String.Format("{0}(MA.Send2WOS = ({1}) Or KD.Send2WOS = ({1})) ", strAndString, _setting.SelectedWOSProperty)
			End If

			Dim strSqlQuery As String = "SELECT ES.ID, ES.ESNr, ES.MANr, ES.KDNr, ES.ES_Als, ES.KDZHDNr, ESL.GAVGruppe0, MA.Send2WOS As MAWOS, MA.Sprache As MASprache, "
			strSqlQuery += "(Convert(nvarchar(10), ES.ES_Ab, 104) + ' - ' + IsNull(convert(nvarchar(10), ES.ES_Ende, 104), '')) As Zeitraum, "
			strSqlQuery += "(MA.Nachname + ', ' + MA.Vorname) As MAName, "
			strSqlQuery += "KD.Firma1, IsNull(KD.Send2WOS, 0) As KDWOS, KD.Sprache As KDSprache "
			strSqlQuery += "FROM ES "
			strSqlQuery += "Left Join ESLohn ESL On ES.ESNr = ESL.ESNr And ESL.Aktivlodaten = 1 "
			strSqlQuery += "Left Join Mitarbeiter MA On ES.MANr = MA.MANr "
			strSqlQuery += "Left Join Kunden KD On ES.KDNr = KD.KDNr "
			If Not String.IsNullOrWhiteSpace(strWhereQuery) Then strWhereQuery = String.Format("Where {0} ", strWhereQuery)
			strSqlQuery = String.Format("{0} {1} Order By MA.Nachname ASC, MA.Vorname ASC, ES.ES_Ab ", strSqlQuery, strWhereQuery)

			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim rFrec As SqlDataReader = cmd.ExecuteReader

			Dim strColumnBez As String = m_xml.GetSafeTranslationValue("ID;ESNr;MANr;KDNr;ZHDNr;bMAWOS;bKDWOS;MASprache;KDSprache;Zeitraum;Kandidat;Kunde;Einsatz als;GAV-Beruf")
			Dim strColumnWidth As String = If(My.Settings.LV_ColumnWidth = String.Empty Or _
																				My.Settings.LV_ColumnWidth.Split(CChar(";")).Length <> strColumnBez.Split(CChar(";")).Length, _
																				"0-0;50-1;0-0;0-0;0-0;0-0;0-1;0-0;0-0;100-0;100-0;100-0;100-0;100-0", My.Settings.LV_ColumnWidth)
			FillDataHeaderLv(lv, strColumnBez, strColumnWidth)
			lv.Items.Clear()
			lv.FullRowSelect = True
			lv.MultiSelect = True

			Try
				Dim i As Integer = 0
				While rFrec.Read
					With lv
						'Dim item As DevComponents.DotNetBar.Controls.ListViewEx.ListViewItemCollection
						.Items.Add(rFrec("ID").ToString)
						.Items(i).SubItems.Add(rFrec("ESNr").ToString)
						.Items(i).SubItems.Add(rFrec("MANr").ToString)
						.Items(i).SubItems.Add(rFrec("KDNr").ToString)
						.Items(i).SubItems.Add(m_proputility.SafeGetInteger(rFrec, "KDZHDNr", 0))
						.Items(i).SubItems.Add(If(CBool(m_proputility.SafeGetBoolean(rFrec, "mawos", False)), "1", "0"))
						.Items(i).SubItems.Add(If(CBool(m_proputility.SafeGetBoolean(rFrec, "kdwos", False)), "1", "0"))
						.Items(i).SubItems.Add(rFrec("MASprache").ToString.Substring(0, 1).ToUpper)
						.Items(i).SubItems.Add(rFrec("KDSprache").ToString.Substring(0, 1).ToUpper)

						.Items(i).SubItems.Add(rFrec("Zeitraum").ToString)
						.Items(i).SubItems.Add(String.Format("{0}", rFrec("MAName").ToString))
						.Items(i).SubItems.Add(String.Format("{0}", rFrec("Firma1").ToString))
						.Items(i).SubItems.Add(String.Format("{0}", rFrec("ES_Als").ToString))
						.Items(i).SubItems.Add(String.Format("{0}", m_proputility.SafeGetString(rFrec, "GAVGruppe0")))

					End With

					i += 1
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.SQL-Abfragefehler. {1}", strMethodeName, ex.Message))

			End Try


		Catch ex As Exception	' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MessageBoxShowError(strMethodeName, ex)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

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
					lvwColumn.Width = CInt(strFieldWidth)	'* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
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
