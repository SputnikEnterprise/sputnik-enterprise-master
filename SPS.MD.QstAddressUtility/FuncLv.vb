
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.Logging

Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()


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

	Sub FillFoundedData(ByVal Lv As ListView, ByVal strQuery As String)
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
		Dim i As Integer = 0
		Dim _ClsDb As New ClsDbFunc
		Dim m_utility As New Utilities

		Try
			Lv.Items.Clear()
			Lv.FullRowSelect = True

			'Conn.Open()
			'Dim cmd As System.Data.SqlClient.SqlCommand
			If strQuery = String.Empty Then strQuery = _ClsDb.GetLocalSQLString()

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RecNr", 0))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ClsDataDetail.ProgSettingData.SelectedMDNr))

			Dim reader = m_utility.OpenReader(ClsDataDetail.GetDbConnString, strQuery, listOfParams)

			'cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)




			'Dim rAdressrec As SqlDataReader = cmd.ExecuteReader          ' Quellensteuer

			Dim Time_1 As Double = System.Environment.TickCount

			Lv.BeginUpdate()
			If Not (reader Is Nothing) Then
				While reader.Read
					With Lv
						.Items.Add(m_utility.SafeGetInteger(reader, "RecNr", 0).ToString)
						.Items(i).SubItems.Add(m_utility.SafeGetString(reader, "Kanton", String.Empty))
						.Items(i).SubItems.Add(m_utility.SafeGetString(reader, "Gemeinde", String.Empty))
						.Items(i).SubItems.Add(m_utility.SafeGetString(reader, "Adresse1", String.Empty))

						.Items(i).SubItems.Add(m_utility.SafeGetString(reader, "PLZ", String.Empty) & " " & m_utility.SafeGetString(reader, "Ort", String.Empty))
						.Items(i).SubItems.Add(m_utility.SafeGetDecimal(reader, "Provision", 0).ToString)
						i += 1
					End With
				End While
			End If
			Lv.EndUpdate()


		Catch e As Exception
			Lv.Items.Clear()
			MsgBox(e.Message, MsgBoxStyle.Critical, "FillFoundedData")

		Finally
			'Conn.Close()
			'Conn.Dispose()

		End Try

	End Sub

	Sub DisplayFoundedData(ByVal frmSource As frmQSTAddress, ByVal iRecNr As Integer)
		'Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
		Dim i As Integer = 0
		Dim _ClsDb As New ClsDbFunc
		Dim m_utility As New Utilities

		Try
			'Conn.Open()
			'Dim cmd As System.Data.SqlClient.SqlCommand
			Dim strQuery As String = _ClsDb.GetLocalSQLString()

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RecNr", iRecNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", ClsDataDetail.ProgSettingData.SelectedMDNr))

			Dim reader = m_utility.OpenReader(ClsDataDetail.GetDbConnString, strQuery, listOfParams)




			'cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			'Dim param As System.Data.SqlClient.SqlParameter
			'param = cmd.Parameters.AddWithValue("@iRecID", iRecNr)

			'Dim rAdressrec As SqlDataReader = cmd.ExecuteReader

			Dim Time_1 As Double = System.Environment.TickCount
			If Not (reader Is Nothing) Then
				If reader.Read Then
					With frmSource
						'.LblRecID.Text = reader("ID").ToString
						.LblRecNr.Text = m_utility.SafeGetInteger(reader, "RecNr", 0).ToString

						.Cbo_Kanton.Text = m_utility.SafeGetString(reader, "Kanton", String.Empty)
						.txt_Gemeinde.Text = m_utility.SafeGetString(reader, "Gemeinde", String.Empty)
						.txt_Adresse.Text = m_utility.SafeGetString(reader, "Adresse1", String.Empty)
						.txt_Zusatz.Text = m_utility.SafeGetString(reader, "Zusatz", String.Empty)
						.txt_ZHD.Text = m_utility.SafeGetString(reader, "ZHD", String.Empty)
						.txt_Postfach.Text = m_utility.SafeGetString(reader, "Postfach")

						.txt_Strasse.Text = m_utility.SafeGetString(reader, "Strasse")
						.txt_Land.Text = m_utility.SafeGetString(reader, "Land")
						.txt_PLZ.Text = m_utility.SafeGetString(reader, "PLZ")
						.txt_Ort.Text = m_utility.SafeGetString(reader, "Ort")

						.txt_StammNr.Text = m_utility.SafeGetString(reader, "StammNr")
						.txt_Provision.Text = m_utility.SafeGetDecimal(reader, "Provision", 0).ToString
						.txt_Bemerkung.Text = m_utility.SafeGetString(reader, "Bemerkung")
						.LblRecInfo.Text = String.Format("Erstellt: {0} um {1}" & vbNewLine & "Geändert: {2} um {3}",
																					 m_utility.SafeGetDateTime(reader, "CreatedOn", Nothing), m_utility.SafeGetString(reader, "CreatedFrom"),
																					 m_utility.SafeGetDateTime(reader, "ChangedOn", Nothing), m_utility.SafeGetString(reader, "ChangedFrom"))

						For Each ctl As Control In .Controls
							.ErrorProvider1.SetError(ctl, String.Empty)
						Next

					End With

				End If
			End If
			'While rAdressrec.Read
			'  With frmSource
			'    .LblRecID.Text = rAdressrec("ID").ToString
			'    .LblRecNr.Text = rAdressrec("RecNr").ToString

			'    .Cbo_Kanton.Text = rAdressrec("Kanton").ToString
			'    .txt_gemeinde.Text = rAdressrec("Gemeinde").ToString
			'    .txt_Adresse.Text = rAdressrec("Adresse1").ToString
			'    .txt_Zusatz.Text = rAdressrec("Zusatz").ToString
			'    .txt_ZHD.Text = rAdressrec("ZHD").ToString
			'    .txt_Postfach.Text = rAdressrec("Postfach").ToString

			'    .txt_Strasse.Text = rAdressrec("Strasse").ToString
			'    .txt_Land.Text = rAdressrec("Land").ToString
			'    .txt_PLZ.Text = rAdressrec("PLZ").ToString
			'    .txt_Ort.Text = rAdressrec("Ort").ToString

			'    .txt_StammNr.Text = rAdressrec("StammNr").ToString
			'    .txt_Provision.Text = rAdressrec("Provision").ToString
			'    .txt_Bemerkung.Text = rAdressrec("Bemerkung").ToString
			'    .LblRecInfo.Text = String.Format("Erstellt: {0} um {1}" & vbNewLine & "Geändert: {2} um {3}", _
			'                                   rAdressrec("CreatedOn").ToString, rAdressrec("CreatedFrom").ToString, _
			'                                   rAdressrec("ChangedOn").ToString, rAdressrec("ChangedFrom").ToString)

			'    For Each ctl As Control In .Controls
			'      .ErrorProvider1.SetError(ctl, String.Empty)
			'    Next

			'  End With


			'i += 1

			'End While


		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Critical, "DisplayFoundedData")

		Finally
			'Conn.Close()
			'Conn.Dispose()

		End Try

	End Sub

	Sub ListAllKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Kanton"

		Dim strSqlQuery As String = "Select PLZ.Kanton From PLZ Where Land = 'CH' and len(PLZ.Kanton) = 2 "
		strSqlQuery += "Group By PLZ.Kanton Order By PLZ.Kanton"

		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

			cbo.Properties.Items.Clear()
			While rKDrec.Read
				cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
				iWidth = CInt(IIf(iWidth > CInt(Len(rKDrec(strFieldName).ToString)), iWidth, CInt(Len(rKDrec(strFieldName).ToString))))

				i += 1
			End While
			cbo.Properties.DropDownRows = 27

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function GetKantonPLZ(ByVal strKanton As String) As String
		Dim strPLZResult As String = ","
		Dim strFieldName As String = "PLZ"

		Dim strSqlQuery As String = "Select PLZ.PLZ, PLZ.Kanton From PLZ "
		strSqlQuery += "Where PLZ.Kanton In ('" & strKanton & "') "
		strSqlQuery += "Group By PLZ.PLZ, PLZ.Kanton Order By PLZ.PLZ, PLZ.Kanton"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rPLZrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

			While rPLZrec.Read
				strPLZResult += rPLZrec(strFieldName).ToString & ","

			End While
			If strPLZResult.Length > 1 Then
				strPLZResult = Mid(strPLZResult, 2, Len(strPLZResult) - 2)
				strPLZResult = Replace(strPLZResult, ",", "','")
			Else
				strPLZResult = String.Empty
			End If

		Catch e As Exception
			strPLZResult = String.Empty
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strPLZResult
	End Function


#End Region


#Region "Helpers..."

	Function GetCitynameFromBox(ByVal strPLZ As String) As String
		Dim strResult As String = String.Empty
		Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)

		If strPLZ = String.Empty Then Return strResult

		Dim strQuery As String = "Select Top 1 Ort From PLZ Where Land = 'CH' And PLZ = @Plz"

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@Plz", strPLZ)

			Dim rDbrec As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank
			While rDbrec.Read()
				strResult = rDbrec("Ort").ToString
				Exit While
			End While


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetCitynameFromBox_0")

		End Try

		Return strResult
	End Function

#End Region



End Module
