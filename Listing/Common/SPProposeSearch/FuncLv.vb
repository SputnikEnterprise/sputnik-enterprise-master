
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging

Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsFunc As New ClsDivFunc
	Private _ClsReg As New SPProgUtility.ClsDivReg

	Private m_xml As New ClsXML



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
	' Sortierung
	Sub ListSort(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)

		cbo.Properties.Items.Add(String.Format("0 - {0}", m_xml.GetSafeTranslationValue("Vorschlagnummer")))
		cbo.Properties.Items.Add(String.Format("1 - {0}", m_xml.GetSafeTranslationValue("Bezeichnung")))
		cbo.Properties.Items.Add(String.Format("2 - {0}", m_xml.GetSafeTranslationValue("Kandidatenname")))
		cbo.Properties.Items.Add(String.Format("3 - {0}", m_xml.GetSafeTranslationValue("Firmenname")))
		cbo.Properties.Items.Add(String.Format("4 - {0}", m_xml.GetSafeTranslationValue("Vakanzennummer")))

		cbo.Properties.Items.Add(String.Format("5 - {0}", m_xml.GetSafeTranslationValue("Erfassdatum")))
		cbo.Properties.Items.Add(String.Format("6 - {0}", m_xml.GetSafeTranslationValue("Status")))

	End Sub

	' Antritt per
	Sub ListPArbbegin(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		ListComboBox(cbo, "P_Arbbegin")
	End Sub
	' Art
	Sub ListPArt(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		ListComboBox(cbo, "P_Art")
	End Sub
	' Anstellung
	Sub ListPAnstellung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		ListComboBox(cbo, "P_Anstellung")
	End Sub
	' Lohn
	Sub ListPLohn(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		ListComboBox(cbo, "Ab_Lohnbetrag")
	End Sub
	' Tarif
	Sub ListPTarif(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		ListComboBox(cbo, "KD_Tarif")
	End Sub
	' Status
	Sub ListPStatus(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		ListComboBox(cbo, "P_State")
	End Sub
	' Berater
	Sub ListPBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		ListComboBox(cbo, "KST")
	End Sub
	' Bezeichnung
	Sub ListBezeichnung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		ListComboBox(cbo, "Bezeichnung")
	End Sub

#End Region

#Region "Allgemeine Funktionen"

	Sub FillFoundedKstBez(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, Optional ByVal strFiliale As String = "")
		Dim LoUSKstBez As New List(Of String)
		Dim LoUSNameBez As New List(Of String)
		Dim Time_1 As Double = System.Environment.TickCount

		Try
			Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
			Dim i As Integer = 0
			Dim sSql As String = "[List Berater From Exists Propose]"

			cbo.Properties.Items.Clear()
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			If strFiliale <> "" Then cmd.Parameters.AddWithValue("@Filiale", strFiliale)
			Dim rUSrec As SqlDataReader = cmd.ExecuteReader

			While rUSrec.Read
				cbo.Properties.Items.Add(New ComboValue(String.Format("{0}", rUSrec("Berater").ToString.Trim), rUSrec("KST").ToString.Trim))
				i += 1
			End While


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

	End Sub

	Function AllowedtoInsertToCbo(ByVal Cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal strBez As String) As Boolean

		For i As Integer = 0 To Cbo.Properties.Items.Count - 1
			If Cbo.Properties.Items(i).ToString.ToLower.Contains(strBez.ToLower) Then
				Return False
			End If
		Next

		Return True
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
					lvwColumn.Width = CInt(strFieldWidth)
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

	Sub ListUSFilialen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, Optional ByVal kst As String = "")
		Dim strEntry As String
		Dim strSqlQuery As String = "Select USFiliale From Benutzer "
		strSqlQuery += "Where USFiliale <> '' And USFiliale Is Not Null "
		If kst.Length > 0 Then
			strSqlQuery += String.Format(" And KST = '{0}' ", kst)
		End If
		strSqlQuery += "Group By USFiliale "
		strSqlQuery += "Order By USFiliale "

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rESrec As SqlDataReader = cmd.ExecuteReader                  '

			cbo.Properties.Items.Clear()
			While rESrec.Read
				strEntry = rESrec("USFiliale").ToString
				cbo.Properties.Items.Add(New ComboValue(strEntry, rESrec("USFiliale").ToString))
			End While


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Function GetFilialKstData(ByVal strFiliale As String) As String
		Dim strKSTResult As String = ","
		Dim strFieldName As String = "KST"

		Dim strSqlQuery As String = "Select Benutzer.KST From Benutzer Left Join US_Filiale on Benutzer.USNr = US_Filiale.USNr Where "
		If strFiliale = "Leere Felder" Then
			strSqlQuery += "US_Filiale.Bezeichnung Is Null "
		Else
			strSqlQuery += "US_Filiale.Bezeichnung = '" & strFiliale & "' "
		End If

		strSqlQuery += "Group By Benutzer.KST Order By Benutzer.KST"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

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

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			strKSTResult = String.Empty
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strKSTResult
	End Function

	''' <summary>
	''' Füllt die ComboBox nur für die Tabelle Vakanzen. Der Feldname muss als Parameter angegeben werden.
	''' </summary>
	''' <param name="cbo"></param>
	''' <param name="Feldname">Entspricht der Spalte in der Tabelle Vakanzen</param>
	''' <remarks></remarks>
	Sub ListComboBox(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal Feldname As String)
		Dim strEntry As String
		Dim strSqlQuery As String
		strSqlQuery = String.Format("Select {0} From Propose Where {0} <> '' And {0} Is Not Null Group By {0} Order By {0}",
																Feldname)

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rProposerec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rProposerec.Read
				strEntry = rProposerec(Feldname).ToString
				cbo.Properties.Items.Add(New ComboValue(strEntry, strEntry))
			End While
			cbo.Properties.DropDownRows = 20


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

#End Region


End Module
