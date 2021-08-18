
Imports System.IO
Imports System.Data.SqlClient

Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Utility

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPUVGListeSearch.ClsDataDetail


Public Class ClsDbFunc

#Region "Private Fields"

	Protected Shared m_Logger As ILogger = New Logger()

	Private m_md As Mandant

	Private m_common As CommonSetting
	Protected m_utility As Utilities
	Protected m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As SearchCriteria

	Private Property SelectedMonatvon As String
	Private Property SelectedMonatbis As String
	Private Property SelectedJahrvon As String
	Private Property SelectedJahrbis As String

	Private Property SelectedMANr As String


#End Region


#Region "Contructor"

	Public Sub New(ByVal _setting As SearchCriteria)

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_SearchCriteria = _setting

	End Sub

#End Region

	Structure ZeitPeriode
		Public LANR As Integer
		Public JahrVon As Integer
		Public JahrBis As Integer
		Public MonatVon As Integer
		Public MonatBis As Integer
		Public SuvaCode As String
		Public Index As Integer
		Public AusgabeZeile As Boolean
		Public AufgrundZeit As Boolean ' Diese Periode ist wegen einer 'Pause' entstanden. (Ab 1 Monat)
	End Structure


#Region "Funktionen zur Suche nach Daten..."

	Enum SuvaCodeEnum As Integer
		A0 = 7320
		A1 = 7321
		A2 = 7322
		A3 = 7323
		Z0 = 7324
		Z1 = 7325
		Z2 = 7326
		Z3 = 7327
		A = 7340
		Z = 7341
	End Enum


	''' <summary>
	''' Dynamische SQL-Query-Zusammenstellung der benötigten Tabellen.
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function GetStartSQLString() As String
		Dim secSuvaList As ArrayList = New ArrayList()
		Dim sql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			conn.Open()
		Catch ex As SqlException
			For Each Err As SqlError In ex.Errors
				m_UtilityUi.ShowErrorDialog(String.Format("Die Verbindung zur Datenbank kann nicht geöffnet werden.{0}{1}", Err.Message))
			Next
			Throw ex
		End Try

		Try

			Dim manrListe As String = CStr(ReplaceMissing(m_SearchCriteria.MANrList, String.Empty))
			Dim jahrVon As Integer = m_SearchCriteria.firstYear
			Dim jahrBis As Integer = m_SearchCriteria.lastYear
			Dim monatVon As Integer = m_SearchCriteria.firstMonth
			Dim monatBis As Integer = m_SearchCriteria.lastMonth
			Dim tabellenName As String = String.Format("_UVGListe_{0}", m_InitialData.UserData.UserNr)
			Dim loarten As String = ""
			Dim betragNull As Integer = Convert.ToInt32(True)	'.Chk_NullBetrag.Checked)

			ClsDataDetail.LLTabellennamen = tabellenName

			' JAHR FÜR ANZAHL MITARBEITER BIS ENDE SEPTEMBER IM LL
			' Nur wenn der September innerhalb der selektierten Zeitperiode liegt und 
			' das letzte Jahr von mehreren, sonst nicht anzeigen.
			ClsDataDetail.LLShowESAnzJahr = False
			Dim datumVon As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", monatVon, jahrVon))
			Dim datumBis As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", monatBis, jahrBis))
			For monat As Integer = 0 To CInt(DateDiff(DateInterval.Month, datumVon, datumBis))
				If datumVon.AddMonths(monat).Month = 9 Then
					ClsDataDetail.LLShowESAnzJahr = True
					ClsDataDetail.LLESAnzJahr = datumVon.AddMonths(monat).Year
				End If
			Next

			' JAHR FÜR SUVA-HL UND LL
			ClsDataDetail.LLSuvaHLJahr = CInt(jahrBis)


			' SQL-ABFRAGE
			'sSql = String.Format("SELECT * FROM {0} ORDER BY ID, Nachname, Vorname, SuvaCode, JahrVon, JahrBis, MonatVon, MonatBis ", tabellenName)

			' DATATABLE UVG-LISTE
			Dim dtUVGListe As DataTable = New DataTable("UVGListe")	' frmTest.dsUVGListe.Tables("UVGListe")
			dtUVGListe.Columns.Add(New DataColumn("ID", System.Type.GetType("System.Int32")))
			dtUVGListe.Columns.Add(New DataColumn("MANR", System.Type.GetType("System.Int32")))
			dtUVGListe.Columns.Add(New DataColumn("JahrVon", System.Type.GetType("System.Int32")))
			dtUVGListe.Columns.Add(New DataColumn("MonatVon", System.Type.GetType("System.Int32")))
			dtUVGListe.Columns.Add(New DataColumn("JahrBis", System.Type.GetType("System.Int32")))
			dtUVGListe.Columns.Add(New DataColumn("MonatBis", System.Type.GetType("System.Int32")))
			dtUVGListe.Columns.Add(New DataColumn("Nachname", System.Type.GetType("System.String")))
			dtUVGListe.Columns.Add(New DataColumn("Vorname", System.Type.GetType("System.String")))
			dtUVGListe.Columns.Add(New DataColumn("Geschlecht", System.Type.GetType("System.String")))
			dtUVGListe.Columns.Add(New DataColumn("AHV_Nr", System.Type.GetType("System.String")))
			dtUVGListe.Columns.Add(New DataColumn("AHV_Nr_New", System.Type.GetType("System.String")))
			dtUVGListe.Columns.Add(New DataColumn("GebDat", System.Type.GetType("System.DateTime")))
			dtUVGListe.Columns.Add(New DataColumn("SuvaCode", System.Type.GetType("System.String")))
			dtUVGListe.Columns.Add(New DataColumn("SecSuvaCode", System.Type.GetType("System.String")))
			dtUVGListe.Columns.Add(New DataColumn("NoSuva", System.Type.GetType("System.String")))
			dtUVGListe.Columns.Add(New DataColumn("Bruttolohn", System.Type.GetType("System.Decimal")))
			dtUVGListe.Columns.Add(New DataColumn("Suvabasis", System.Type.GetType("System.Decimal")))
			dtUVGListe.Columns.Add(New DataColumn("Suvalohn", System.Type.GetType("System.Decimal")))

			' Alle Kandidaten, die während Zeitperiode eine Lohnabrechnung mit SUVA-Abzug hatten
			Dim cmdMANR As SqlCommand = New SqlCommand("", conn)
			cmdMANR.CommandType = CommandType.Text
			Dim cmdMANRText As String = "SELECT LOL.MANR, LOL.LANR, LOL.Jahr, LOL.LP, Sum(LOL.M_BTR) As Betrag "
			cmdMANRText += "FROM LOL "
			cmdMANRText += "WHERE "
			cmdMANRText += "LOL.MDNR = @MDNr And "
			cmdMANRText += "LOL.LANR IN (7000,7320,7321,7322,7323,7324,7325,7326,7327,7340,7341) And "
			If manrListe.Length > 0 Then
				cmdMANRText += String.Format("LOL.MANR IN ({0}) And ", manrListe)
			End If
			cmdMANRText += "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And " & _
													"(@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or "
			cmdMANRText += " (LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
			cmdMANRText += " (LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And " & _
														"(@jahrVon <> @jahrBis Or (LOL.LP >= @monatVon And LOL.LP <= @monatBis))) "
			cmdMANRText += ") "

			cmdMANRText += "And LOL.M_BTR <> 0 " ' 18.06.2012
			cmdMANRText += "GROUP BY LOL.MANR, LOL.LANR, LOL.Jahr, LOL.LP "
			cmdMANRText += "ORDER BY LOL.MANR, LOL.LANR, LOL.Jahr, LOL.LP "
			cmdMANR.CommandText = cmdMANRText
			Dim pMANRJahrVon As SqlParameter = New SqlParameter("@jahrVon", SqlDbType.Int, 4)
			Dim pMANRJahrBis As SqlParameter = New SqlParameter("@jahrBis", SqlDbType.Int, 4)
			Dim pMANRMonatVon As SqlParameter = New SqlParameter("@monatVon", SqlDbType.Int, 2)
			Dim pMANRMonatBis As SqlParameter = New SqlParameter("@monatBis", SqlDbType.Int, 2)
			pMANRJahrVon.Value = jahrVon
			pMANRJahrBis.Value = jahrBis
			pMANRMonatVon.Value = monatVon
			pMANRMonatBis.Value = monatBis

			cmdMANR.Parameters.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
			cmdMANR.Parameters.Add(pMANRJahrVon)
			cmdMANR.Parameters.Add(pMANRJahrBis)
			cmdMANR.Parameters.Add(pMANRMonatVon)
			cmdMANR.Parameters.Add(pMANRMonatBis)
			Dim daMANR As SqlDataAdapter = New SqlDataAdapter(cmdMANR)
			Dim dtMANR As DataTable = New DataTable("MANR")

			If daMANR.Fill(dtMANR) > 0 Then
				' VORBEREITUNGEN
				Dim uvgListeRow As DataRow

				' ALLE MANR SAMMELN, FALLS NOCH NICHT BEREITS ANGEGEBEN
				If manrListe.Length = 0 Then
					For Each rManr As DataRow In dtMANR.Rows
						If Not manrListe.Contains(rManr("MANR").ToString) Then
							manrListe += String.Format("{0},", rManr("MANR"))
						End If
					Next
					manrListe = manrListe.Substring(0, manrListe.Length - 1) ' Letztes Komma entfernen
				End If

				' MITARBEITER-DATEN ALLER MANR
				Dim cmdMA As SqlCommand = New SqlCommand("", conn)
				cmdMA.CommandType = CommandType.Text
				Dim cmdMAText As String = "SELECT Mitarbeiter.MANR, Mitarbeiter.Nachname, Mitarbeiter.Vorname, "
				cmdMAText += "Mitarbeiter.Geschlecht, Mitarbeiter.AHV_Nr, "
				cmdMAText += "Mitarbeiter.AHV_Nr_New, Mitarbeiter.GebDat, IsNull(MA_LOSetting.SecSuvaCode,'') As SecSuvaCode "
				cmdMAText += "FROM Mitarbeiter "
				cmdMAText += "LEFT JOIN MA_LOSetting ON "
				cmdMAText += "MA_LOSetting.MANR = Mitarbeiter.MANR "
				cmdMAText += "WHERE "
				cmdMAText += String.Format("Mitarbeiter.MANR IN ({0}) ", manrListe)
				cmdMAText += "GROUP BY Mitarbeiter.MANR, Mitarbeiter.Nachname, Mitarbeiter.Vorname, "
				cmdMAText += "Mitarbeiter.Geschlecht, Mitarbeiter.AHV_Nr, "
				cmdMAText += "Mitarbeiter.AHV_Nr_New, Mitarbeiter.GebDat, MA_LOSetting.SecSuvaCode "
				cmdMAText += "ORDER BY Mitarbeiter.Nachname, Mitarbeiter.Vorname, Mitarbeiter.GebDat "
				cmdMA.CommandText = cmdMAText
				Dim daMA As SqlDataAdapter = New SqlDataAdapter(cmdMA)
				Dim dtMA As DataTable = New DataTable("MA")
				daMA.Fill(dtMA)


				' SUVALOHN A ALLER MITARBEITER
				' SUVALOHN Z ALLER MITARBEITER
				Dim cmdSuvalohn As SqlCommand = New SqlCommand("", conn)
				cmdSuvalohn.CommandType = CommandType.Text
				Dim cmdSuvalohnText As String = "SELECT LOL.MANR, LOL.LANR, LOL.Jahr, LOL.LP, Sum(LOL.M_Btr) As Suvalohn "
				cmdSuvalohnText += "FROM LOL "
				cmdSuvalohnText += "WHERE LOL.MDNr = @MDNr And LOL.LANR IN (7000, 7340,7341) And "		' 18.06.2012 (7000, )
				cmdSuvalohnText += String.Format("LOL.MANR IN ({0}) And ", manrListe)
				cmdSuvalohnText += "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or "
				cmdSuvalohnText += "(LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or "
				cmdSuvalohnText += " (LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
				cmdSuvalohnText += " (LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or "
				cmdSuvalohnText += "(LOL.LP >= @monatVon And LOL.LP <= @monatBis)))) "
				cmdSuvalohnText += "And LOL.M_BTR <> 0 " ' 18.06.2012
				cmdSuvalohnText += "GROUP BY LOL.MANR, LOL.LANR, LOL.Jahr, LOL.LP "
				cmdSuvalohnText += "ORDER BY LOL.MANR, LOL.Jahr, LOL.LP "
				cmdSuvalohn.CommandText = cmdSuvalohnText
				Dim pSuvalohnJahrVon As SqlParameter = New SqlParameter("@jahrVon", SqlDbType.Int, 4)
				Dim pSuvalohnJahrBis As SqlParameter = New SqlParameter("@jahrBis", SqlDbType.Int, 4)
				Dim pSuvalohnMonatVon As SqlParameter = New SqlParameter("@monatVon", SqlDbType.Int, 2)
				Dim pSuvalohnMonatBis As SqlParameter = New SqlParameter("@monatBis", SqlDbType.Int, 2)
				pSuvalohnJahrVon.Value = jahrVon
				pSuvalohnJahrBis.Value = jahrBis
				pSuvalohnMonatVon.Value = monatVon
				pSuvalohnMonatBis.Value = monatBis

				cmdSuvalohn.Parameters.Add(New SqlClient.SqlParameter("MDNr", m_InitialData.MDData.MDNr))
				cmdSuvalohn.Parameters.Add(pSuvalohnJahrVon)
				cmdSuvalohn.Parameters.Add(pSuvalohnJahrBis)
				cmdSuvalohn.Parameters.Add(pSuvalohnMonatVon)
				cmdSuvalohn.Parameters.Add(pSuvalohnMonatBis)
				Dim dtSuvalohn As DataTable = New DataTable("Suvalohn")
				Dim daSuvalohn As SqlDataAdapter = New SqlDataAdapter(cmdSuvalohn)
				daSuvalohn.Fill(dtSuvalohn)


				' TOTALMA12, TOTALMA3, TOTALFA12, TOTALFA3, TOTALMZ12, TOTALMZ3, TOTALFZ12, TOTALFZ3
				Dim totalMA12, totalMA3, totalFA12, totalFA3, totalMZ12, totalMZ3, totalFZ12, totalFZ3 As Decimal

				Dim z As Integer = 1 ' Zähler für diverse Felder (z.B. damit diese nur einmal pro Zeile erscheinen.) (Default=1)
				Try

					' PRO KANDIDAT UND LOHNABRECHNUNG
					For i As Integer = 0 To dtMA.Rows.Count - 1
						Dim manr As Integer = Int32.Parse(dtMA.Rows(i)("MANR").ToString)
						Dim jahrVonZeile As Integer = CInt(jahrVon)
						Dim jahrBisZeile As Integer = CInt(jahrBis)
						Dim monatVonZeile As Integer = CInt(monatVon)
						Dim monatBisZeile As Integer = CInt(monatBis)
						Dim lanrLohnTemp As Integer = 0	' Der Suvalohn kommt je für A und Z nur einmal pro Kandidat und Monat vor
						Dim zpTemp As ZeitPeriode = New ZeitPeriode

						' Mitarbeiter-Daten ermitteln
						Dim rowMA As DataRow = dtMA.Select(String.Format("MANR={0}", manr))(0)


						' Pausen zwischen Lohnabrechnungen müssen deklariert werden. (Ab 1 Monat Differenz)
						' Pro Zeitperiode eine Zeile schreiben
						' Zeitperioden ermitteln und hinzufügen
						Dim zeitPeriodenList As ArrayList = New ArrayList()
						InsertRowInToPeriod(zeitPeriodenList, dtMANR.Select(String.Format("MANR={0} And LANR=7320", manr)))	' A0
						InsertRowInToPeriod(zeitPeriodenList, dtMANR.Select(String.Format("MANR={0} And LANR=7321", manr)))	' A1
						InsertRowInToPeriod(zeitPeriodenList, dtMANR.Select(String.Format("MANR={0} And LANR=7322", manr)))	' A2
						InsertRowInToPeriod(zeitPeriodenList, dtMANR.Select(String.Format("MANR={0} And LANR=7323", manr)))	' A3
						InsertRowInToPeriod(zeitPeriodenList, dtMANR.Select(String.Format("MANR={0} And LANR=7324", manr)))	' Z0
						InsertRowInToPeriod(zeitPeriodenList, dtMANR.Select(String.Format("MANR={0} And LANR=7325", manr)))	' Z1
						InsertRowInToPeriod(zeitPeriodenList, dtMANR.Select(String.Format("MANR={0} And LANR=7326", manr)))	' Z2
						InsertRowInToPeriod(zeitPeriodenList, dtMANR.Select(String.Format("MANR={0} And LANR=7327", manr)))	' Z3

						' Die hinzugefügten SuvaCode-Zeilen chronologisch sortieren
						SortListItem(zeitPeriodenList)

						' Die Ausgabezeile festlegen
						SetAusgabezeile(zeitPeriodenList)


						' SuvaBasis in der Periode angegeben?
						Dim suvaBasisAangegeben As Boolean = False
						Dim suvaBasisZangegeben As Boolean = False
						Dim suvaBasisA0 As Decimal = 0
						Dim suvaBasisA1 As Decimal = 0
						Dim suvaBasisA2 As Decimal = 0
						Dim suvaBasisA3 As Decimal = 0
						Dim suvaBasisZ0 As Decimal = 0
						Dim suvaBasisZ1 As Decimal = 0
						Dim suvaBasisZ2 As Decimal = 0
						Dim suvaBasisZ3 As Decimal = 0

						' Suvalohn in der Periode angegeben?
						Dim suvaLohnAangegeben As Boolean = False
						Dim suvaLohnZangegeben As Boolean = False

						For Each zP As ZeitPeriode In zeitPeriodenList
							' Zeile erzeugen
							uvgListeRow = dtUVGListe.NewRow()
							' Default-Werte der Zeile zuweisen
							uvgListeRow("ID") = i
							uvgListeRow("SuvaCode") = zP.SuvaCode
							uvgListeRow("SecSuvaCode") = rowMA("SecSuvaCode")
							uvgListeRow("MANR") = 0
							uvgListeRow("Nachname") = ""
							uvgListeRow("Vorname") = ""
							uvgListeRow("Geschlecht") = rowMA("Geschlecht")
							uvgListeRow("AHV_Nr") = ""
							uvgListeRow("AHV_Nr_New") = ""
							uvgListeRow("Bruttolohn") = 0
							uvgListeRow("Suvabasis") = 0
							uvgListeRow("Suvalohn") = 0
							' Die Zeitperioden der Zeile in der Tabelle hinzufügen
							uvgListeRow("JahrVon") = zP.JahrVon
							uvgListeRow("JahrBis") = zP.JahrBis
							uvgListeRow("MonatVon") = zP.MonatVon
							uvgListeRow("MonatBis") = zP.MonatBis


							' Eine Zeile pro Zeitperiode und SuvaCode
							Dim bruttoBetrag As Decimal = 0

							Dim suvaLohnA As Decimal = 0
							Dim suvaLohnZ As Decimal = 0
							Dim filterText As String = "MANR={0} And LANR={1} And ("
							filterText += "(Jahr = {2} And LP >= {3} And ({2} <> {4} Or (LP >= {3} And LP <= {5}))) Or "
							filterText += " (Jahr > {2} And Jahr < {4}) Or "
							filterText += " (Jahr = {4} And LP <= {5} And ({2} <> {4} Or (LP >= {3} And LP <= {5})))) "
							Dim filterTextFormated As String = String.Format(filterText, manr, zP.LANR, zP.JahrVon, zP.MonatVon, zP.JahrBis, zP.MonatBis)

							' Suvabasis pro SuvaCode und Zeitperiode
							For Each row As DataRow In dtMANR.Select(filterTextFormated)
								If zP.LANR = 7320 Then
									suvaBasisA0 += CDec(row("Betrag"))
								ElseIf zP.LANR = 7321 Then
									suvaBasisA1 += CDec(row("Betrag"))
								ElseIf zP.LANR = 7322 Then
									suvaBasisA2 += CDec(row("Betrag"))
								ElseIf zP.LANR = 7323 Then
									suvaBasisA3 += CDec(row("Betrag"))
								ElseIf zP.LANR = 7324 Then
									suvaBasisZ0 += CDec(row("Betrag"))
								ElseIf zP.LANR = 7325 Then
									suvaBasisZ1 += CDec(row("Betrag"))
								ElseIf zP.LANR = 7326 Then
									suvaBasisZ2 += CDec(row("Betrag"))
								ElseIf zP.LANR = 7327 Then
									suvaBasisZ3 += CDec(row("Betrag"))
								End If
							Next

							' Bruttolohn nur bei neuer Zeitperiode hinzufügen.
							If zP.AusgabeZeile Then

								' Mitarbeiterdaten zuweisen
								uvgListeRow("MANR") = manr
								uvgListeRow("Nachname") = rowMA("Nachname")
								uvgListeRow("Vorname") = rowMA("Vorname")
								uvgListeRow("AHV_Nr") = rowMA("AHV_Nr")
								uvgListeRow("AHV_Nr_New") = rowMA("AHV_Nr_New")
								uvgListeRow("GebDat") = rowMA("GebDat")

								uvgListeRow("Suvabasis") = 0
								uvgListeRow("Suvalohn") = 0

								' Default-Werte erste Periode
								Dim jVon As Integer = zP.JahrVon
								Dim jBis As Integer = zP.JahrBis
								Dim mVon As Integer = zP.MonatVon
								Dim mBis As Integer = zP.MonatBis


								' Bruttolohn
								filterTextFormated = String.Format(filterText, manr, 7000, zP.JahrVon, zP.MonatVon, zP.JahrBis, zP.MonatBis)
								For Each bruttoRow As DataRow In dtMANR.Select(filterTextFormated)
									bruttoBetrag += CDec(bruttoRow("Betrag"))
								Next

								suvaLohnAangegeben = False
								suvaLohnZangegeben = False

							End If

							uvgListeRow("Bruttolohn") = bruttoBetrag


							' Suvalohn A
							If zP.LANR >= SuvaCodeEnum.A0 And zP.LANR <= SuvaCodeEnum.A3 And Not suvaLohnAangegeben Then
								filterTextFormated = String.Format(filterText, manr, 7340, zP.JahrVon, zP.MonatVon, zP.JahrBis, zP.MonatBis)
								For Each suvalohnRow As DataRow In dtMANR.Select(filterTextFormated)
									suvaLohnA += CDec(suvalohnRow("Betrag"))
									suvaLohnAangegeben = True
								Next
							End If

							' Suvalohn Z
							If zP.LANR >= SuvaCodeEnum.Z0 And zP.LANR <= SuvaCodeEnum.Z3 And Not suvaLohnZangegeben Then
								filterTextFormated = String.Format(filterText, manr, 7341, zP.JahrVon, zP.MonatVon, zP.JahrBis, zP.MonatBis)
								For Each suvalohnRow As DataRow In dtMANR.Select(filterTextFormated)
									suvaLohnZ += CDec(suvalohnRow("Betrag"))
									suvaLohnZangegeben = True
								Next
							End If



							' Suvabasis A 
							If zP.LANR >= SuvaCodeEnum.A0 And zP.LANR <= SuvaCodeEnum.A3 Then
								uvgListeRow("Suvabasis") = suvaBasisA0 + suvaBasisA1 + suvaBasisA2 + suvaBasisA3
								suvaBasisAangegeben = True
								suvaBasisA0 = 0
								suvaBasisA1 = 0
								suvaBasisA2 = 0
								suvaBasisA3 = 0
							End If

							' Suvabasis Z
							If zP.LANR >= SuvaCodeEnum.Z0 And zP.LANR <= SuvaCodeEnum.Z3 Then
								uvgListeRow("Suvabasis") = suvaBasisZ0 + suvaBasisZ1 + suvaBasisZ2 + suvaBasisZ3
								suvaBasisZangegeben = True
								suvaBasisZ0 = 0
								suvaBasisZ1 = 0
								suvaBasisZ2 = 0
								suvaBasisZ3 = 0
							End If

							' Falls Suvabasis Z noch nicht ausgegeben, so Suvabasis und Suvalohn angeben, wenn Ausgabezeile SuvaCode A hat.
							If Not suvaBasisZangegeben And Not zP.AusgabeZeile And zP.LANR >= SuvaCodeEnum.Z0 And _
								zP.LANR <= SuvaCodeEnum.Z3 Then
								uvgListeRow("Suvabasis") = suvaBasisZ0 + suvaBasisZ1 + suvaBasisZ2 + suvaBasisZ3
								' Suvalohn Z
								filterTextFormated = String.Format(filterText, manr, 7341, zP.JahrVon, zP.MonatVon, zP.JahrBis, zP.MonatBis)
								For Each suvalohnRow As DataRow In dtMANR.Select(filterTextFormated)
									suvaLohnZ += CDec(suvalohnRow("Betrag"))
								Next
							End If

							' Falls Suvabasis A noch nicht ausgegeben, so Suvabasis und Suvalohn angeben, wenn Ausgabezeile SuvaCode Z hat.
							If Not suvaBasisAangegeben And Not zP.AusgabeZeile And zP.LANR >= SuvaCodeEnum.A0 And _
								zP.LANR <= SuvaCodeEnum.A3 Then
								uvgListeRow("Suvabasis") = suvaBasisA0 + suvaBasisA1 + suvaBasisA2 + suvaBasisA3
								' Suvalohn A
								filterTextFormated = String.Format(filterText, manr, 7340, zP.JahrVon, zP.MonatVon, zP.JahrBis, zP.MonatBis)
								For Each suvalohnRow As DataRow In dtMANR.Select(filterTextFormated)
									suvaLohnA += CDec(suvalohnRow("Betrag"))
								Next
							End If

							' Wenn beide Suvacode vorhanden, so für beide je eine Zeile schreiben.
							' In der zweiten Zeile jedoch Infos nicht wiederholen
							If suvaLohnA <> 0 Then
								uvgListeRow("Suvalohn") = suvaLohnA
								' Zeile hinzufügen
								dtUVGListe.Rows.Add(uvgListeRow)
							End If


							If suvaLohnZ <> 0 Then
								If suvaLohnA <> 0 Then
									' Zurücksetzen, da bereits beim Suvalohn A enthalten. Neue Zeile erzeugen.
									uvgListeRow = dtUVGListe.NewRow()
									uvgListeRow("ID") = i
									uvgListeRow("SuvaCode") = zP.SuvaCode
									uvgListeRow("SecSuvaCode") = rowMA("SecSuvaCode")
									uvgListeRow("MANR") = 0
									uvgListeRow("Nachname") = ""
									uvgListeRow("Vorname") = ""
									uvgListeRow("Geschlecht") = rowMA("Geschlecht")
									uvgListeRow("AHV_Nr") = ""
									uvgListeRow("AHV_Nr_New") = ""
								End If
								uvgListeRow("Suvalohn") = suvaLohnZ
								' Zeile hinzufügen
								dtUVGListe.Rows.Add(uvgListeRow)
							End If

							' "Leere" Zeile
							If suvaLohnA = 0 And suvaLohnZ = 0 Then
								dtUVGListe.Rows.Add(uvgListeRow)
							End If

							' TotalMAxx, TotalFAxx (im LL direkt hinzufügen)
							If rowMA("Geschlecht").ToString.ToUpper = "M" Then
								If zP.SuvaCode.StartsWith("A") Then
									If zP.SuvaCode.ToUpper = "A3" Then

										totalMA3 += suvaLohnA
									Else
										totalMA12 += suvaLohnA
									End If
								Else
									If zP.SuvaCode.ToUpper = "Z3" Then
										totalMZ3 += suvaLohnZ
									Else
										totalMZ12 += suvaLohnZ
									End If
								End If
							Else
								If zP.SuvaCode.StartsWith("A") Then
									If zP.SuvaCode.ToUpper = "A3" Then
										totalFA3 += suvaLohnA
									Else
										totalFA12 += suvaLohnA
									End If
								Else
									If zP.SuvaCode.ToUpper = "Z3" Then
										totalFZ3 += suvaLohnZ
									Else
										totalFZ12 += suvaLohnZ
									End If
								End If
							End If

						Next ' Zeitperiode

					Next ' Kandidat

				Catch ex As Exception
					m_UtilityUi.ShowErrorDialog(String.Format("Fehler in der Zeile {0}. MANR={1}{2}{3}", z, dtMANR.Rows(z)("MANR"), vbNewLine, ex.ToString))
				End Try

				' TotalMAxx, TotalFAxx, TotalMZxx, TotalFZxx im LL-Variabeln hinzufügen
				ClsDataDetail.LLTotalMA12 = totalMA12
				ClsDataDetail.LLTotalMA3 = totalMA3
				ClsDataDetail.LLTotalFA12 = totalFA12
				ClsDataDetail.LLTotalFA3 = totalFA3
				ClsDataDetail.LLTotalMZ12 = totalMZ12
				ClsDataDetail.LLTotalMZ3 = totalMZ3
				ClsDataDetail.LLTotalFZ12 = totalFZ12
				ClsDataDetail.LLTotalFZ3 = totalFZ3

				' Für die Rekapitulation die Variabeln im LL setzen
				' Alle möglichen SecSuvaCodes
				'Dim secSuvaList As ArrayList = New ArrayList()
				For Each secSuvaCodeRow As DataRow In dtMA.Rows
					If Not secSuvaList.Contains(secSuvaCodeRow("SecSuvaCode")) Then
						secSuvaList.Add(secSuvaCodeRow("SecSuvaCode"))
					End If
				Next

			End If

			ClsDataDetail.UVGListeDataTable = dtUVGListe

			' Eine bestehende Tabelle auf der Datenbank löschen
			Dim cmdCreateTable As SqlCommand = _
				New SqlCommand(String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH ", ClsDataDetail.LLTabellennamen), conn)
			cmdCreateTable.ExecuteNonQuery()

			' Die erstellte Tabelle auf die Datenbank erzeugen
			cmdCreateTable.CommandText = String.Format("CREATE TABLE {0} (", ClsDataDetail.LLTabellennamen)
			For Each col As DataColumn In dtUVGListe.Columns
				If col.DataType.Name = "Decimal" Then
					cmdCreateTable.CommandText += String.Format(" {0} {1}(18,2),", col.ColumnName, col.DataType.Name)
				Else
					cmdCreateTable.CommandText += String.Format(" {0} {1},", col.ColumnName, col.DataType.Name)
				End If
			Next
			cmdCreateTable.CommandText = cmdCreateTable.CommandText.Remove(cmdCreateTable.CommandText.Length - 1, 1) ' letztes Komma entfernen
			cmdCreateTable.CommandText += " )"
			cmdCreateTable.CommandText = cmdCreateTable.CommandText.Replace("Int32", "Int").Replace("Int16", "Int").Replace("String", "nvarchar(255)")
			cmdCreateTable.ExecuteNonQuery()

			' Die erzeugte Tabelle mit der erstellten Tabelle füllen
			cmdCreateTable.CommandText = String.Format("INSERT INTO {0} VALUES (", ClsDataDetail.LLTabellennamen)
			For Each col As DataColumn In dtUVGListe.Columns
				Dim typeObj As Object = SqlDbType.Int
				Select Case col.DataType.Name.ToUpper
					Case "String".ToUpper
						typeObj = SqlDbType.NVarChar
					Case "DateTime".ToUpper
						typeObj = SqlDbType.DateTime
					Case "Decimal".ToUpper
						typeObj = SqlDbType.Decimal
				End Select
				' CommandText ergänzen
				cmdCreateTable.CommandText += String.Format("@{0}, ", col.ColumnName)
				'Parameter hinzufügen
				Dim p As SqlParameter = New SqlParameter(String.Format("@{0}", col.ColumnName), DirectCast(typeObj, SqlDbType))
				cmdCreateTable.Parameters.Add(p)

			Next
			cmdCreateTable.CommandText = cmdCreateTable.CommandText.Remove(cmdCreateTable.CommandText.Length - 2, 2) ' letztes Komma entfernen
			cmdCreateTable.CommandText += ")"
			'Dim iCount As Integer = 0
			' Jeden Datensatz auf der Datenbank übertragen
			For Each rowToInsert As DataRow In dtUVGListe.Rows
				' Parameter füllen
				For Each p As SqlParameter In cmdCreateTable.Parameters
					If p.SqlDbType.ToString.ToUpper = "NVARCHAR" Then
						p.Value = rowToInsert(p.ParameterName.Replace("@", "")).ToString
					Else
						p.Value = rowToInsert(p.ParameterName.Replace("@", ""))
					End If
				Next
				'iCount += 1
				'Trace.WriteLine(iCount)
				' Zeile schreiben
				cmdCreateTable.ExecuteNonQuery()
			Next

			'conn.Close()

			' FILTERBEDINGUNGEN für die Anzeige auf der Liste
			ClsDataDetail.GetFilterBez = ""
			If manrListe.Length > 0 Then
				ClsDataDetail.GetFilterBez += String.Format("Kandidaten-Nr.: {0}{1}", manrListe, vbLf)
			End If
			ClsDataDetail.GetFilterBez += String.Format("Vom {0} / {1} bis {2} / {3}{4}", monatVon, jahrVon, monatBis, jahrBis, vbLf)

			sql = "SELECT U.*, " ', tabellenName)

			sql &= "	dbo.DateMax( ISNULL( (SELECT MIN(ES.ES_Ab) FROM ES  Where ES.MDNr = {1} AND ES.MANr = U.MANr "
			sql &= "AND (ES.ES_Ende >= '01.' + CONVERT(NVARCHAR(2),U.MonatVon) + '.' + CONVERT(NVARCHAR(4), U.JahrVon) Or ES.ES_Ende Is Null) And ES.ES_Ab <= dbo.EndOfMonth "
			sql &= "(U.Jahrvon, u.Monatbis)  And ES.NoListing = 0 ), "
			sql &= "'01.' + CONVERT(NVARCHAR(2),U.MonatVon) + '.' + CONVERT(NVARCHAR(4), U.JahrVon)), '01.' + CONVERT(NVARCHAR(2),U.MonatVon) + '.' + CONVERT(NVARCHAR(4), "
			sql &= "U.JahrVon)) AS ES_Ab, "

			sql &= "dbo.Datemin( IsNull( (SELECT Max(ES.ES_Ende) FROM ES  Where ES.MDNr = {1} AND ES.MANr = U.MANr "
			sql &= "AND (ES.ES_Ende >= '01.' + CONVERT(NVARCHAR(2),U.MonatVon) + '.' + CONVERT(NVARCHAR(4), U.JahrVon) Or ES.ES_Ende Is Null) And ES.ES_Ab <= dbo.EndOfMonth(U.Jahrvon, u.Monatbis)			And ES.NoListing = 0 ), "
			sql &= "dbo.EndOfMonth(U.Jahrvon, u.Monatbis)), dbo.EndOfMonth(U.Jahrvon, u.Monatbis)) AS ES_Ende "

			sql &= "FROM {0} U "	', tabellenName)

			sql &= "Order By U.ID"

			sql = String.Format(sql, tabellenName, m_InitialData.MDData.MDNr)
			m_SearchCriteria.sqlsearchstring = Sql

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Sql = String.Empty

		End Try



		'iCount = 0
		ClsDataDetail.LLSecSuvaItems.Clear()
		'For Each secSuvaCode As String In secSuvaList
		'  Dim item As ClsDataDetail.SecSuvaItem = New ClsDataDetail.SecSuvaItem()
		'  Dim betragM As Decimal = 0
		'  Dim betragF As Decimal = 0
		'  ' Pro SecSuvaCode alle Kandidaten ermitteln

		'  Dim strGroupsSql As String = "Select Geschlecht, Sum(SuvaLohn) As SuvaLohn From {0} Where SecSuvaCode = '{1}' Group By Geschlecht Order By Geschlecht"
		'  strGroupsSql = String.Format(strGroupsSql, ClsDataDetail.LLTabellennamen, secSuvaCode)
		'  Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strGroupsSql, conn)
		'  cmd.CommandType = Data.CommandType.Text

		'  Dim rRec As SqlDataReader = cmd.ExecuteReader
		'  While rRec.Read
		'    iCount += 1
		'    ' Zur Liste im LL hinzufügen
		'    item.Bezeichnung = secSuvaCode
		'    If rRec("Geschlecht").ToString = "M" Then
		'      item.M = CDec(rRec("SuvaLohn").ToString)

		'    Else
		'      item.F = CDec(rRec("SuvaLohn").ToString)

		'    End If

		'  End While
		'  Trace.WriteLine(iCount)
		'  ClsDataDetail.LLSecSuvaItems.Add(item)

		'Next
		conn.Close()


		Return sql
	End Function

	''' <summary>
	''' Die SuvaCode-Zeile wird zur Liste hinzugefügt.
	''' </summary>
	''' <param name="zPList"></param>
	''' <param name="zeitPeriodenRows"></param>
	''' <remarks></remarks>
	Sub InsertRowInToPeriod(ByRef zPList As ArrayList, ByVal zeitPeriodenRows As DataRow())

		If zeitPeriodenRows.Count = 0 Then
			Return
		End If


		Dim zeitPer As ZeitPeriode = New ZeitPeriode

		' Suva-Code-Bezeichnung aus Enum anhand LANR
		Dim suvaEnum As SuvaCodeEnum

		' Aktuelle Zeile
		zeitPer.LANR = CInt(zeitPeriodenRows(0)("LANR"))
		zeitPer.JahrVon = CInt(zeitPeriodenRows(0)("Jahr"))
		zeitPer.JahrBis = CInt(zeitPeriodenRows(0)("Jahr"))
		zeitPer.MonatVon = CInt(zeitPeriodenRows(0)("LP"))
		zeitPer.MonatBis = CInt(zeitPeriodenRows(0)("LP"))

		zeitPer.AufgrundZeit = False

		For x As Integer = 1 To zeitPeriodenRows.Count - 1
			If CInt(zeitPeriodenRows(x)("Jahr")) = CInt(zeitPeriodenRows(x - 1)("Jahr")) Then	' Gleichen Jahr
				If CInt(zeitPeriodenRows(x)("LP")) > CInt(zeitPeriodenRows(x - 1)("LP")) + 1 Then
					' Aktuelle Zeile schreiben (Nächster Datensatz hat mehr als 1 Monat Differenz im gleichen Jahr)
					zeitPer.SuvaCode = [Enum].GetName(suvaEnum.GetType(), zeitPer.LANR)
					zeitPer.Index = zPList.Count ' Index hinzufügen
					zeitPer.AufgrundZeit = True
					zPList.Add(zeitPer)
					' Nächsten Datensatz erzeugen und vorbelegen
					zeitPer = New ZeitPeriode()
					zeitPer.LANR = CInt(zeitPeriodenRows(x)("LANR"))
					zeitPer.JahrVon = CInt(zeitPeriodenRows(x)("Jahr"))
					zeitPer.JahrBis = CInt(zeitPeriodenRows(x)("Jahr"))
					zeitPer.MonatVon = CInt(zeitPeriodenRows(x)("LP"))
					zeitPer.MonatBis = CInt(zeitPeriodenRows(x)("LP"))
				Else
					zeitPer.MonatBis = CInt(zeitPeriodenRows(x)("LP")) ' Nächsten Monat
				End If
			ElseIf CInt(zeitPeriodenRows(x)("Jahr")) = CInt(zeitPeriodenRows(x - 1)("Jahr")) + 1 Then	' Nächstes Jahr
				' Schwelle Dezember Vorjahr und Januar im nächsten Jahr
				If CInt(zeitPeriodenRows(x)("LP")) = 1 And CInt(zeitPeriodenRows(x - 1)("LP")) = 12 Then
					zeitPer.JahrBis = CInt(zeitPeriodenRows(x)("Jahr"))	' Nächstes Jahr
					zeitPer.MonatBis = CInt(zeitPeriodenRows(x)("LP")) ' Nächsten Monat
				Else
					'Aktuelle Zeile schreiben (Nächster Datensatz hat mehr als 1 Monat Differenz zwischen Vorjahr und Folgejahr)
					zeitPer.SuvaCode = [Enum].GetName(suvaEnum.GetType(), zeitPer.LANR)
					zeitPer.Index = zPList.Count ' Index hinzufügen
					zeitPer.AufgrundZeit = True
					zPList.Add(zeitPer)
					' Nächsten Datensatz erzeugen und vorbelegen
					zeitPer = New ZeitPeriode()
					zeitPer.LANR = CInt(zeitPeriodenRows(x)("LANR"))
					zeitPer.JahrVon = CInt(zeitPeriodenRows(x)("Jahr"))
					zeitPer.JahrBis = CInt(zeitPeriodenRows(x)("Jahr"))
					zeitPer.MonatVon = CInt(zeitPeriodenRows(x)("LP"))
					zeitPer.MonatBis = CInt(zeitPeriodenRows(x)("LP"))
				End If
			Else ' Mehr als 1 Jahr
				' Aktuelle Zeile schreiben (Nächster Datensatz hat mehr als 1 Jahr Differenz)
				zeitPer.JahrBis = CInt(zeitPeriodenRows(x)("Jahr"))	' Mehr als 1 Jahr
				zeitPer.MonatBis = CInt(zeitPeriodenRows(x)("LP")) ' Ein Monat
				zeitPer.SuvaCode = [Enum].GetName(suvaEnum.GetType(), zeitPer.LANR)
				zeitPer.Index = zPList.Count ' Index hinzufügen
				zeitPer.AufgrundZeit = True
				zPList.Add(zeitPer)
				' Nächsten Datensatz erzeugen und vorbelegen
				zeitPer = New ZeitPeriode()
				zeitPer.LANR = CInt(zeitPeriodenRows(x)("LANR"))
				zeitPer.JahrVon = CInt(zeitPeriodenRows(x)("Jahr"))
				zeitPer.JahrBis = CInt(zeitPeriodenRows(x)("Jahr"))
				zeitPer.MonatVon = CInt(zeitPeriodenRows(x)("LP"))
				zeitPer.MonatBis = CInt(zeitPeriodenRows(x)("LP"))
			End If
		Next


		' Letzter Datensatz hinzufügen
		zeitPer.SuvaCode = [Enum].GetName(suvaEnum.GetType(), zeitPer.LANR)
		zeitPer.Index = zPList.Count ' Index hinzufügen
		' Die vorletzte Zeitperiode kann Aufgrund der 'Pause' sein, somit muss die letzte Zeitperiode folglich aus gleichem Grund.
		If zPList.Count > 0 Then
			If DirectCast(zPList.Item(zeitPer.Index - 1), ZeitPeriode).AufgrundZeit Then
				zeitPer.AufgrundZeit = True
			End If
		End If
		zPList.Add(zeitPer)

	End Sub

	''' <summary>
	''' Die SuvaCode-Zeilen werden chronologisch sortiert. Die Zeitperioden werden bei Bedarf so angepasst,
	''' dass das Von-Datum zurückversetzt wird, oder/und das Bis-Datum vorversetzt wird.
	''' </summary>
	''' <param name="zpList"></param>
	''' <remarks></remarks>
	Private Sub SortListItem(ByRef zpList As ArrayList)
		' Falls Zeitperiode einer folgenden SuvaCode vorher beginnt, so diese vor der anderen Zeitperiode setzten
		Dim sortieren As Boolean = True
		While sortieren
			sortieren = False
			Dim index As Integer = 0
			If zpList.Count > 0 Then
				For Each zp As ZeitPeriode In zpList
					If zpList.Count > zp.Index + 1 Then	' Solange Elemente gibt...
						Dim zpItemVor As ZeitPeriode = DirectCast(zpList.Item(zp.Index), ZeitPeriode)
						Dim zpItemNach As ZeitPeriode = DirectCast(zpList.Item(zp.Index + 1), ZeitPeriode)
						Dim dtItemVor As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", zp.MonatVon, zp.JahrVon))
						Dim dtItemNach As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", zpItemNach.MonatVon, zpItemNach.JahrVon))

						' Fängt die nächste Periode vor der vorgänger Periode an?
						If DateDiff(DateInterval.Month, dtItemVor, dtItemNach) < 0 Then
							sortieren = True
							index = zpItemNach.Index
						End If
					End If
				Next
			End If
			' Die Zeitperiodenliste sortieren
			If sortieren Then
				MoveListItemUp(zpList, index)
			End If
		End While

		' Nachdem sortiert ist, die Zeitperioden durchgehen und Überschneidungen anpassen
		Dim zpListTemp As ArrayList = New ArrayList()
		For Each zp As ZeitPeriode In zpList
			If zp.Index + 1 < zpList.Count Then	' Solange Elemente im Array gibt...
				Dim zpNach As ZeitPeriode = DirectCast(zpList.Item(zp.Index + 1), ZeitPeriode)
				Dim dtVor As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", zp.MonatBis, zp.JahrBis))
				Dim dtNach As DateTime = DateTime.Parse(String.Format("01.{0}.{1}", zpNach.MonatVon, zpNach.JahrVon)).AddDays(-1).AddMonths(1)
				' Wenn keine Diefferenz besteht, so beginnt die nächste Periode bevor bzw. genau wenn die erste endet.
				If DateDiff(DateInterval.Month, dtVor, dtNach) < 1 Then
					' Die erste Periode muss mindestens so lange dauern wie die nächste.
					If zp.JahrBis = zpNach.JahrBis And zp.MonatBis < zpNach.MonatBis Then
						' Wenn im gleichen Jahr endet...
						zp.MonatBis = zpNach.MonatBis
					ElseIf zp.JahrBis < zpNach.JahrBis Then
						' Wenn im folgenden Jahr endet...
						zp.JahrBis = zpNach.JahrBis
						zp.MonatBis = zpNach.MonatBis
					End If

				End If
			End If
			zpListTemp.Add(zp)
		Next

		zpList = zpListTemp

	End Sub

	''' <summary>
	''' Die bestimmte SuvaCode-Zeile um eine Zeile nach oben verschieben.
	''' </summary>
	''' <param name="zpList"></param>
	''' <param name="index"></param>
	''' <remarks></remarks>
	Private Sub MoveListItemUp(ByRef zpList As ArrayList, ByVal index As Integer)
		Dim arrayListTemp As ArrayList = New ArrayList()
		Dim ausgabeUmgestellt As Boolean = False
		For Each item As ZeitPeriode In zpList
			'Dim itemVor As ZeitPeriode = DirectCast(zpList.Item(item.Index - 1), ZeitPeriode)
			'Dim itemNach As ZeitPeriode = DirectCast(zpList.Item(item.Index + 1), ZeitPeriode)
			If item.Index = index Then
				arrayListTemp.Insert(item.Index - 1, item)
			Else
				arrayListTemp.Add(item)
			End If
		Next
		zpList = New ArrayList()
		For Each item As ZeitPeriode In arrayListTemp
			item.Index = zpList.Count
			zpList.Add(item)
		Next
	End Sub

	''' <summary>
	''' Entscheiden, welche Zeile eine Ausgabezeile ist und welche nicht.
	''' Eine Ausgabezeile wird für die Periode zusätzlich die Bruttolöhne zusammengerechnet und ausgegeben.
	''' Die anderen Zeilen werden für diese Periode nur die Suva-Basis und -Lohn ausgegeben.
	''' </summary>
	''' <param name="zpList"></param>
	''' <remarks></remarks>
	Private Sub SetAusgabezeile(ByRef zpList As ArrayList)
		' Perioden, die sich überschneiden, wählt man für die Ausgabe die längste Periode
		' Die Ausgabezeile an der richtigen Stelle platzieren. Nichtübergreifende Perioden je eine Ausgabenzeile.
		' [Info: Die hinzugefügten Objekte lassen sich leider nicht mehr ändern!!! Darum neue Objekte.]

		Dim longestPeriod As Integer = 0
		Dim longestPeriodIndex As Integer = 0
		Dim gruppe As ArrayList = New ArrayList()
		For Each zp As ZeitPeriode In zpList
			If zp.Index + 1 < zpList.Count Then
				Dim zpNach As ZeitPeriode = DirectCast(zpList.Item(zp.Index + 1), ZeitPeriode)
				If zpNach.JahrVon < zp.JahrBis Or (zpNach.JahrVon = zp.JahrBis And zpNach.MonatVon <= zp.MonatBis) Then
					' Überschneidung
					If longestPeriod < (zp.JahrBis - zp.JahrVon) * 12 + (zp.MonatBis - zp.MonatVon) + 1 Then
						longestPeriod = (zp.JahrBis - zp.JahrVon) * 12 + (zp.MonatBis - zp.MonatVon) + 1
						longestPeriodIndex = zp.Index
					ElseIf longestPeriod < (zpNach.JahrBis - zpNach.JahrVon) * 12 + (zpNach.MonatBis - zpNach.MonatVon) + 1 Then
						longestPeriod = (zpNach.JahrBis - zpNach.JahrVon) * 12 + (zpNach.MonatBis - zpNach.MonatVon) + 1
						longestPeriodIndex = zpNach.Index
					End If
				Else
					' Keine Überschneidung
					' Gruppe bilden für die Ausgabezeile
					gruppe.Add(longestPeriodIndex)
					longestPeriodIndex = zpNach.Index	' Default-Wert
				End If
			Else
				' Letztes Item
				gruppe.Add(longestPeriodIndex)
			End If
		Next

		' Pro Gruppe die Ausgabezeile setzen
		Dim zeitPeriodenListTemp As ArrayList = New ArrayList()	' Um die Ausgabezeile festzulegen, müssen neue Objekte erzeugt werden!!!
		For Each zp As ZeitPeriode In zpList
			If gruppe.Contains(zp.Index) Then
				zp.AusgabeZeile = True
			Else
				zp.AusgabeZeile = False
			End If
			zeitPeriodenListTemp.Add(zp)
		Next

		zpList = zeitPeriodenListTemp

	End Sub

	Function GetLstItems(ByVal lst As ListBox) As String
		Dim strBerufItems As String = String.Empty

		For i = 0 To lst.Items.Count - 1
			strBerufItems += lst.Items(i).ToString & "#@"
		Next

		Return Left(strBerufItems, Len(strBerufItems) - 2)
	End Function

#End Region


End Class

