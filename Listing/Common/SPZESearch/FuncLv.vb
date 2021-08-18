
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports SP.Infrastructure.Logging

Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()



#Region "Datenbankabfragen für SearchRec..."

	Function GetZEDbData4ZESearch() As SqlDataReader ' DataTable
		'Dim ds As New DataSet
		'Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Conn.Open()
		Dim Sql As String = String.Empty

		Sql &= "Select ZE.ZENr, RE.R_Name1 As Firma, Sum(ZE.Betrag) As Betrag From ZE "
		Sql &= "Left Join RE On ZE.RENr = RE.RENr "
		Sql &= "Left Join Kunden KD On ZE.KDNr = KD.KDNr And RE.KDNr = KD.KDNr "
		Sql &= "Where RE.MDNr = @MDNr And RE.R_Name1 <> '' "
		Sql &= "And (@Filiale Like '%%' Or KD.KDFiliale Like @Filiale) "
		Sql &= "Group By ZE.ZENr, RE.R_Name1 "
		Sql &= "Order By RE.R_Name1 ASC "

		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(Sql, Conn)
		cmd.CommandType = CommandType.Text
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@mdnr", ClsDataDetail.m_InitialData.MDData.MDNr)
		param = cmd.Parameters.AddWithValue("@Filiale", "%" & ClsDataDetail.m_InitialData.UserData.UserFiliale & "%")
		Dim reader As SqlDataReader = cmd.ExecuteReader

		Return reader

		'Dim objAdapter As New SqlDataAdapter

		'objAdapter.SelectCommand = cmd
		'objAdapter.Fill(ds, "ESData")

		'Return ds.Tables(0)
	End Function

	Function GetZEREDbData4ZESearch() As SqlDataReader ' DataTable
		'Dim ds As New DataSet
		'Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Conn.Open()
		Dim Sql As String = String.Empty

		Sql &= "Select ZE.RENr, RE.R_Name1 As Firma, Sum(RE.BetragInk) As Betrag From ZE "
		Sql &= "Left Join RE On ZE.RENr = RE.RENr "
		Sql &= "Left Join Kunden KD On ZE.KDNr = KD.KDNr And RE.KDNr = KD.KDNr "
		Sql &= "Where RE.MDNr = @MDNr And RE.R_Name1 <> '' "
		Sql &= "And (@Filiale Like '%%' Or KD.KDFiliale Like @Filiale) "
		Sql &= "Group By ZE.RENr, RE.R_Name1 "
		Sql &= "Order By RE.R_Name1 ASC "

		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(Sql, Conn)
		cmd.CommandType = CommandType.Text
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@mdnr", ClsDataDetail.m_InitialData.MDData.MDNr)
		param = cmd.Parameters.AddWithValue("@Filiale", "%" & ClsDataDetail.m_InitialData.UserData.UserFiliale & "%")

		Dim reader As SqlDataReader = cmd.ExecuteReader

		Return reader

		'Dim objAdapter As New SqlDataAdapter

		'objAdapter.SelectCommand = cmd
		'objAdapter.Fill(ds, "ESData")

		'Return ds.Tables(0)
	End Function

	Function GetZEKDDbData4ZESearch() As SqlDataReader 'DataTable
		'Dim ds As New DataSet
		'Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Conn.Open()
		Dim Sql As String = String.Empty

		Sql &= "Select ZE.KDNr, RE.R_Name1 As Firma From ZE "
		Sql &= "Left Join RE On ZE.RENr = RE.RENr "
		Sql &= "Left Join Kunden KD On ZE.KDNr = KD.KDNr And RE.KDNr = KD.KDNr "
		Sql &= "Where RE.MDNr = @MDNr And RE.R_Name1 <> '' "
		Sql &= "And (@Filiale Like '%%' Or KD.KDFiliale Like @Filiale) "
		Sql &= "Group By ZE.KDNr, RE.R_Name1 "
		Sql &= "Order By RE.R_Name1 ASC "

		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(Sql, Conn)
		cmd.CommandType = CommandType.Text
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@mdnr", ClsDataDetail.m_InitialData.MDData.MDNr)
		param = cmd.Parameters.AddWithValue("@Filiale", "%" & ClsDataDetail.m_InitialData.UserData.UserFiliale & "%")

		Dim reader As SqlDataReader = cmd.ExecuteReader

		Return reader

		'Dim objAdapter As New SqlDataAdapter

		'objAdapter.SelectCommand = cmd
		'objAdapter.Fill(ds, "ESData")

		'Return ds.Tables(0)
	End Function

	Function GetZESonstDbData4ZESearch() As SqlDataReader ' DataTable
		'Dim ds As New DataSet
		'Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Conn.Open()
		Dim strQuery As String = "[List KDNameData For Search In ZESearch]"
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim reader As SqlDataReader = cmd.ExecuteReader

		Return reader

		'Dim objAdapter As New SqlDataAdapter

		'objAdapter.SelectCommand = cmd
		'objAdapter.Fill(ds, "ESData")

		'Return ds.Tables(0)
	End Function


#End Region







	Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


#Region "Dropdown-Funktionen für 1. Seite..."

	Sub ListSort(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)

		cbo.Properties.Items.Add(New ComboValue(String.Format("0 - {0}", ClsDataDetail.m_Translate.GetSafeTranslationValue("Zahlungsnummer")), "0"))
		cbo.Properties.Items.Add(New ComboValue(String.Format("1 - {0}", ClsDataDetail.m_Translate.GetSafeTranslationValue("Rechnungsnummer")), "1"))
		cbo.Properties.Items.Add(New ComboValue(String.Format("2 - {0}", ClsDataDetail.m_Translate.GetSafeTranslationValue("Kundennummer")), "2"))
		cbo.Properties.Items.Add(New ComboValue(String.Format("3 - {0}", ClsDataDetail.m_Translate.GetSafeTranslationValue("Rechnungsempfänger")), "3"))

		cbo.Properties.Items.Add(New ComboValue(String.Format("4 - {0}", ClsDataDetail.m_Translate.GetSafeTranslationValue("Valutadatum (Aufsteigend)")), "4"))
		cbo.Properties.Items.Add(New ComboValue(String.Format("5 - {0}", ClsDataDetail.m_Translate.GetSafeTranslationValue("Valutadatum (Absteigend)")), "5"))

		cbo.Properties.Items.Add(New ComboValue(String.Format("6 - {0}", ClsDataDetail.m_Translate.GetSafeTranslationValue("Buchungsdatum (Aufsteigend)")), "6"))
		cbo.Properties.Items.Add(New ComboValue(String.Format("7 - {0}", ClsDataDetail.m_Translate.GetSafeTranslationValue("Buchungsdatum (Absteigend)")), "7"))

		cbo.Properties.Items.Add(New ComboValue(String.Format("8 - {0}", ClsDataDetail.m_Translate.GetSafeTranslationValue("Betrag (Aufsteigend)")), "8"))
		cbo.Properties.Items.Add(New ComboValue(String.Format("9 - {0}", ClsDataDetail.m_Translate.GetSafeTranslationValue("Betrag (Absteigend)")), "9"))

	End Sub

	Sub ListKDFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetKDFiliale]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("Bezeichnung").ToString)

			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	'Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
	'  Dim strSqlQuery As String = String.Empty
	'  Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    strSqlQuery = "[GetREKDBetrater]"

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.StoredProcedure

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Properties.items.clear()
	'    While rZGrec.Read
	'      cbo.Properties.Items.Add(rZGrec("KST").ToString)
	'    End While


	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()
			strSqlQuery = "[Get OPBerater]" ' "[GetREKDBetrater]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim rFrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			'Dim lst As New ListBox
			While rFrec.Read
				cbo.Properties.Items.Add(rFrec("USName").ToString)
			End While
			cbo.Properties.DropDownRows = 20
			'ListOPKst(cbo, lst)

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListOPKst(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal Lst As ListBox)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim strUserName As String = String.Empty

		'Lst.Items.Clear()
		Dim Time_1 As Double = System.Environment.TickCount
		Try
			cbo.Properties.Items.Clear()

			Dim strKSTbez As String()
			Dim strKst As String
			Dim strAllKst As String = String.Empty
			Dim bInsertItem As Boolean

			cbo.Properties.Items.BeginUpdate()
			For i As Integer = 0 To Lst.Items.Count - 1
				With cbo
					strKst = Lst.Items(i).ToString
					strKSTbez = strKst.Split(CChar("/"))
					For j As Integer = 0 To strKSTbez.Length - 1
						strUserName = GetUSNameFromKst(strKSTbez(j))
						bInsertItem = AllowedtoInsertToCbo(cbo, strUserName)

						If bInsertItem Then
							.Properties.Items.Add(String.Format("{0} ({1})", strUserName, strKSTbez(j)))
						End If

					Next
				End With
			Next
			cbo.Properties.Items.EndUpdate()
			cbo.Properties.Sorted = True
			cbo.Properties.DropDownRows = 20

			Dim Time_2 As Double = System.Environment.TickCount

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function AllowedtoInsertToCbo(ByVal Cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal strBez As String) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		For i As Integer = 0 To Cbo.Properties.Items.Count - 1
			If Cbo.Properties.Items(i).ToString.ToLower.Contains(strBez.ToLower) Then
				Return False
			End If
		Next

		Return True
	End Function

	Function GetUSNameFromKst(ByVal strSelectedKst As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = "[Get UserData With USKst]"
		Dim strResult As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@USKst", strSelectedKst)
			Dim rRec As SqlDataReader = cmd.ExecuteReader

			While rRec.Read
				strResult = String.Format("{0}, {1}", rRec("Nachname"), rRec("Vorname"))
			End While

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function




	Sub ListREArt(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetREArt For ZESearch]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

			cbo.Properties.Items.Clear()

			Dim codeAusgeschrieben As String = ""
			While rZGrec.Read
				Select Case rZGrec("Art").ToString
					Case "A"
						codeAusgeschrieben = "Automatische"
					Case "F"
						codeAusgeschrieben = "Festanstellung"
					Case "G"
						codeAusgeschrieben = "Gutschriften"
					Case "I"
						codeAusgeschrieben = "Individuelle"
				End Select
				'cbo.Properties.items.add(new ComboValue(String.Format("({0}) {1}", rZGrec("Art").ToString, codeAusgeschrieben), rZGrec("Art").ToString))
				cbo.Properties.Items.Add(rZGrec("Art").ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListRECurrency(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetRECurrency]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Vorschussdatenbank

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

	Sub ListREKst1(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal filiale As String)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetREKst1]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("Bezeichnung").ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListREKst2(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetREKst2]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("Bezeichnung").ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListBuKonto(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal filiale As String)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "SELECT ZE.FKSoll, IsNull(FBK.KontoName,'') as Bezeichnung FROM ZE "
			strSqlQuery += "LEFT JOIN Kunden ON ZE.KDNr = Kunden.KDNr "
			strSqlQuery += "LEFT JOIN FBK ON FBK.KontoNr = ZE.FKSoll "
			strSqlQuery += "WHERE "
			strSqlQuery += "(ZE.FKSoll Is Not Null Or ZE.FKSoll <> '') And "
			strSqlQuery += String.Format("('{0}' = '' Or Kunden.KDFiliale Like '%{0}%') ", filiale)
			strSqlQuery += "GROUP BY ZE.FKSoll, FBK.KontoName ORDER BY ZE.FKSoll "

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				Dim text As String = String.Format("{0} {1}", rZGrec("FKSoll"), rZGrec("Bezeichnung"))
				Dim wert As String = rZGrec("FKSoll").ToString
				cbo.Properties.Items.Add(New ComboValue(text, wert))

			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMwSt(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "SELECT MWSTProz FROM RE "
			strSqlQuery += "WHERE "
			strSqlQuery += "MWSTProz > 0 "
			strSqlQuery += "GROUP BY MWSTProz "
			strSqlQuery += "ORDER BY MWSTProz DESC"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMWST As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("")
			cbo.Properties.Items.Add(New ComboValue("pflichtig", "1"))
			While rMWST.Read
				Dim text As String = String.Format("{0:#.00} %", CDec(rMWST("MWSTProz").ToString))
				Dim wert As String = rMWST("MWSTProz").ToString
				cbo.Properties.Items.Add(New ComboValue(text, wert))

			End While
			cbo.Properties.Items.Add(New ComboValue("frei", "0"))

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

#End Region


#Region "Auflistungsfunktionen..."

	Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
		Dim lstStuff As ListViewItem = New ListViewItem()
		Dim lvwColumn As ColumnHeader

		With Lv
			.Clear()

			' Nr;Nummer;Name;Strasse;PLZ Ort
			Dim strCaption As String() = Regex.Split(strColumnList, ";")
			' 0-1;0-1;2000-0;2000-0;2500-0
			Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")

			If strCaption.Length <> strFieldInfo.Length Then
				MessageBox.Show(String.Format("Ungültige Spaltenanzahl:{0}{1}{0}{2}", vbLf, strColumnList, strColumnInfo), "FillDataHeaderLv", MessageBoxButtons.OK, MessageBoxIcon.Error)
			End If

			For i = 0 To strCaption.Length - 1
				lvwColumn = New ColumnHeader()
				lvwColumn.Text = strCaption(i).ToString

				'lvwColumn.Width = CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel
				lvwColumn.Width = CInt(strFieldInfo(i).ToString.Split(CChar("-"))(0))
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


#End Region

#Region "Sonstige Funktions..."

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


#End Region




End Module
