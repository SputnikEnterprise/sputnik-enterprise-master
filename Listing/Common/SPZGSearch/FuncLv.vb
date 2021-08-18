
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions


Module FuncLv

#Region "Dropdown-Funktionen für 1. Seite..."

	Sub ListZGFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim i As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim iWidth As Integer

		Try
			Conn.Open()

			strSqlQuery = "[GetZGMAFiliale]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("Bezeichnung").ToString)
				iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec("Bezeichnung").ToString)), iWidth, CInt(Len(rZGrec("Bezeichnung").ToString))))

				i += 1
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetZGMABetrater]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(String.Format("{0} ({1})", rZGrec("USName").ToString, rZGrec("Kst").ToString))
			End While
			cbo.Properties.DropDownRows = 20


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListZGMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetZGLP]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("LP").ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListZGYear(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetZGYear]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("Jahr").ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListZGLANr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetZGLANr]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("LANr").ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListZGCurrency(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetZGCurrency]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("Currency").ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

#End Region


#Region "Sonstige Funktions..."

	Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
		Dim lstStuff As ListViewItem = New ListViewItem()
		Dim lvwColumn As ColumnHeader

		With Lv
			.Clear()

			' Nr;Nummer;Name;Strasse;PLZ Ort
			Dim strCaption As String() = Regex.Split(strColumnList, ";")
			' 0-1;0-1;2000-0;2000-0;2500-0
			Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")

			For i = 0 To strCaption.Length - 1
				lvwColumn = New ColumnHeader()
				lvwColumn.Text = strCaption(i).ToString

				lvwColumn.Width = CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel
				If CInt(Right(strFieldInfo(i).ToString, 1)) = 1 Then
					lvwColumn.TextAlign = HorizontalAlignment.Right
				ElseIf CInt(Right(strFieldInfo(i).ToString, 1)) = 2 Then
					lvwColumn.TextAlign = HorizontalAlignment.Center
				Else
					lvwColumn.TextAlign = HorizontalAlignment.Left
				End If

				.Columns.Add(lvwColumn)
			Next

			lvwColumn = Nothing
		End With

	End Sub

	Sub FillFoundedKDData(ByVal Lv As ListView, ByVal strQuery As String)
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim i As Integer = 0
		Dim TotalBetrag As Double = 0

		Try
			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank
			Lv.Items.Clear()
			Lv.FullRowSelect = True

			Dim Time_1 As Double = System.Environment.TickCount

			Lv.BeginUpdate()
			While rZGrec.Read
				With Lv
					.Items.Add(rZGrec("ZGNr").ToString)
					.Items(i).SubItems.Add(rZGrec("MANr").ToString)
					.Items(i).SubItems.Add(rZGrec("Nachname").ToString & " " & rZGrec("Vorname").ToString)

					.Items(i).SubItems.Add(Format(rZGrec("LP"), "00") & " / " & rZGrec("Jahr").ToString)

					If Not IsDBNull(rZGrec("LANr")) Then
						.Items(i).SubItems.Add(rZGrec("LANr").ToString)
					Else
						.Items(i).SubItems.Add("")
					End If

					If Not IsDBNull(rZGrec("Betrag")) Then
						.Items(i).SubItems.Add(Format(CDbl(rZGrec("Betrag").ToString) * CInt(IIf(CBool(ClsDataDetail.ShowBetragAsPositiv), -1, 1)), "###,###,###,###,0.00"))
					Else
						.Items(i).SubItems.Add("")
					End If
					If Not IsDBNull(rZGrec("Aus_Dat")) Then
						.Items(i).SubItems.Add(Format(rZGrec("Aus_Dat"), "dd.MM.yyyy"))
					Else
						.Items(i).SubItems.Add("")
					End If
					If Not IsDBNull(rZGrec("Printed_Dat")) Then
						.Items(i).SubItems.Add(Format(rZGrec("Printed_Dat"), "dd.MM.yyyy"))
					Else
						.Items(i).SubItems.Add("")
					End If

					If Not IsDBNull(rZGrec("CreatedOn")) And Not IsDBNull(rZGrec("CreatedFrom")) Then
						.Items(i).SubItems.Add(Format(rZGrec("CreatedOn"), "dd.MM.yyyy") & " " & rZGrec("CreatedFrom").ToString)
					Else
						.Items(i).SubItems.Add("")
					End If

					TotalBetrag += CDbl(rZGrec("Betrag").ToString)

				End With

				i += 1
				Lv.EndUpdate()
				ClsDataDetail.GetTotalBetrag = TotalBetrag

			End While

			Dim Time_2 As Double = System.Environment.TickCount
			Console.WriteLine("Zeit für ListMailToFields: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


		Catch e As Exception
			Lv.Items.Clear()
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

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

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

	Function GetFilialKstData(ByVal strFiliale As String) As String
		Dim strKSTResult As String = ","
		Dim strFieldName As String = "KST"

		Dim strSqlQuery As String = "Select Benutzer.KST From Benutzer Left Join US_Filiale on Benutzer.USNr = US_Filiale.USNr "
		strSqlQuery += "Where US_Filiale.Bezeichnung = '" & strFiliale & "' And "
		strSqlQuery += "US_Filiale.Bezeichnung <> '' "
		strSqlQuery += "And US_Filiale.Bezeichnung Is Not Null Group By Benutzer.KST Order By Benutzer.KST"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rPLZrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

			While rPLZrec.Read
				strKSTResult += rPLZrec(strFieldName).ToString & ","

			End While
			Console.WriteLine("strKSTResult: " & strKSTResult)
			If strKSTResult.Length > 1 Then
				strKSTResult = Mid(strKSTResult, 2, Len(strKSTResult) - 2)
				strKSTResult = Replace(strKSTResult, ",", "','")
			Else
				strKSTResult = String.Empty
			End If

		Catch e As Exception
			strKSTResult = String.Empty
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strKSTResult
	End Function


	Sub ListForActivate(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		cbo.Properties.Items.Clear()
		Try

			cbo.Properties.Items.Add(New ComboBoxItem With {.ValueMember = "1", .DisplayValue = "Aktiviert"})
			cbo.Properties.Items.Add(New ComboBoxItem With {.ValueMember = "0", .DisplayValue = "Nicht Aktiviert"})


		Catch e As Exception
			MsgBox(e.Message)

		Finally

		End Try

	End Sub


#End Region




End Module
