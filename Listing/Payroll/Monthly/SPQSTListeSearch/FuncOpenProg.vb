
Option Strict Off

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports SPQSTListeSearch.Xsd.Ge
Imports System.Text
Imports SPProgUtility.MainUtilities

Imports SP.Infrastructure.Logging


Public Module FuncOpenProg

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	'Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private strMDPath As String = ""
	Private strInitPath As String = ""

	Private iLogedUSNr As Integer = 0

	Private strMDIniFile As String = _ClsProgSetting.GetMDIniFile()

	Private strMDProgFile As String = _ClsProgSetting.GetMDIniFile()
	Private strInitProgFile As String = _ClsProgSetting.GetInitIniFile()

	'Dim _ex As New ClsShowError

	Enum TypeActivite

		''' <summary>
		''' Hauptbeschäftigung
		''' </summary>
		''' <remarks></remarks>
		principale

		''' <summary>
		''' Nebenjob
		''' </summary>
		''' <remarks></remarks>
		accessoire

		''' <summary>
		''' Zusatz/Ergänzende Arbeit
		''' </summary>
		''' <remarks></remarks>
		complémentaire
	End Enum


	Sub GetMenuItems4Export(ByVal tsbMenu As ToolStripDropDownButton)
		'    Dim strFieldName As String = "Bezeichnung"
		Dim i As Integer = 0
		Dim strSqlQuery As String = _
		String.Format("Select RecNr, Bezeichnung, ToolTip, MnuName, Docname From ExportDb Where ModulName = '{0}' Order By RecNr", _
																							ClsDataDetail.GetAppGuidValue())

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMnurec As SqlDataReader = cmd.ExecuteReader					' PLZ-Datenbank

			tsbMenu.DropDownItems.Clear()
			tsbMenu.DropDown.SuspendLayout()

			Dim mnu As ToolStripMenuItem
			While rMnurec.Read
				i += 1

				If rMnurec("Bezeichnung").ToString = "-" Then
					Dim sep As New ToolStripSeparator()
					tsbMenu.DropDownItems.Add(sep)

				Else
					mnu = New ToolStripMenuItem()

					mnu.Text = rMnurec("Bezeichnung").ToString
					If Not IsDBNull(rMnurec("ToolTip")) Then
						mnu.ToolTipText = rMnurec("ToolTip").ToString
					End If
					If Not IsDBNull(rMnurec("MnuName").ToString) Then
						mnu.Name = rMnurec("MnuName").ToString
					End If
					tsbMenu.DropDownItems.Add(mnu)

				End If

			End While
			tsbMenu.DropDown.ResumeLayout()
			tsbMenu.ShowDropDown()


		Catch e As Exception
			MsgBox(Err.GetException.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

#Region "Funktionen für Exportieren..."

	Private Function ShowMyFileDlg(ByVal strFile2Search As String) As String
		Dim strFullFileName As String = String.Empty
		Dim strFilePath As String = String.Empty
		Dim myStream As Stream = Nothing
		Dim openFileDialog1 As New OpenFileDialog()

		openFileDialog1.Title = strFile2Search
		openFileDialog1.InitialDirectory = strFile2Search
		openFileDialog1.Filter = "EXE-Dateien (*.exe)|*.exe|Alle Dateien (*.*)|*.*"
		openFileDialog1.FilterIndex = 1
		openFileDialog1.RestoreDirectory = True

		If openFileDialog1.ShowDialog() = DialogResult.OK Then
			Try

				myStream = openFileDialog1.OpenFile()
				If (myStream IsNot Nothing) Then
					strFullFileName = openFileDialog1.FileName()

					' Insert code to read the stream here.
				End If

			Catch Ex As Exception
				MessageBox.Show("Kann keine Daten lesen: " & Ex.Message)
			Finally
				' Check this again, since we need to make sure we didn't throw an exception on open.
				If (myStream IsNot Nothing) Then
					myStream.Close()
				End If
			End Try
		End If

		Return strFullFileName
	End Function

	Function ExportQstXmlStandard(ByVal strTempSQL As String, ByVal strFullFilename As String) As Boolean
		Dim bResult As Boolean = True
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim strTranslationProgName As String = String.Empty

		Dim cmd As System.Data.SqlClient.SqlCommand
		cmd = New System.Data.SqlClient.SqlCommand(strTempSQL & " FOR XML AUTO", Conn)

		Try
			Conn.Open()

			Dim Xml_Reader As System.Xml.XmlReader

			Xml_Reader = cmd.ExecuteXmlReader()
			Dim sb As New System.Text.StringBuilder
			sb.Append("<xml>")
			Xml_Reader.Read()
			Do
				Dim node As String = Xml_Reader.ReadOuterXml()
				If node.Length = 0 Then Exit Do
				sb.Append(node)
			Loop
			sb.Append("</xml>")

			Xml_Reader.Close()

			Dim objDateiMacher As StreamWriter
			Dim path As String = strFullFilename ' _ClsProgSetting.GetPersonalFolder() & "Quellensteuerabrechnung.XML"
			objDateiMacher = New StreamWriter(path)
			objDateiMacher.Write(sb.ToString)
			objDateiMacher.Close()
			objDateiMacher.Dispose()

			'MessageBox.Show(String.Format("Die Quellensteuerliste wurde im Verzeichnis: {0}{1}{0} erfolgreich gespeichert.", _
			'                              vbLf, _
			'                              path), "Quellensteuerliste speichern")

		Catch e As Exception
			'_ex.MessageBoxShowError(iLogedUSNr, "XML-Datei erstellen", e)

			Return bResult
		End Try

		Return bResult
	End Function

	Sub ExportQstXmlVdPseudo(ByVal listenTypQST As String, ByVal leereDeklaration As Boolean, ByVal strFullFilename As String)
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim strTranslationProgName As String = String.Empty

		Dim cmd As SqlCommand
		cmd = New SqlCommand("", Conn)

		Try
			Conn.Open()

			Dim cmdText As String = String.Format("SELECT Sum(M_Btr) As montantBrut FROM {0}", ClsDataDetail.LLTabellennamen)
			cmd.CommandText = cmdText
			Dim montantBrut As Decimal = 0 ' PSEUDO
			Dim montantCommission As Decimal = 0 ' PSEUDO

			Dim sb As New System.Text.StringBuilder
			'      Dim dateiName As String = "XMLDatei.XML"
			Dim kundenNummerQST As Integer = 1501495 ' TODO: Wird vom Amt geliefert  (SWAG Kundennummer hartcodiert)
			Dim kandidatenNummerQST As Integer = 0 ' TODO: Wird vom Amt geliefert 
			Dim periodeDatumVon As DateTime = ClsDataDetail.SelPeriodeVon
			Dim periodeDatumBis As DateTime = ClsDataDetail.SelPeriodeBis

			' META-DATEN
			sb.Append("<?xml version=""1.0"" encoding=""UTF-8"" ?> ")
			sb.Append("<tns:listeImpotSource xmlns:tns=""http://www.vd.ch/fiscalite/impotsource/liste-impot-source/4"" ")
			sb.Append("xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
			sb.Append("xsi:schemaLocation=""http://www.vd.ch/fiscalite/impotsource/liste-impot-source/4")
			sb.Append(" D:\registre\empaci\trunk\04-Impl\xmlgen\src\main\resources\xsdEmpaci\v4\liste-impot-source-4.xsd"" ")
			sb.Append(String.Format("debutPeriodeDeclaration=""{0:yyyy-MM-dd}"" finPeriodeDeclaration=""{1:yyyy-MM-dd}"">", _
															periodeDatumVon, _
															periodeDatumBis))
			sb.Append(String.Format("<typeListe>{0}</typeListe>", "LR"))
			sb.Append(String.Format("<numDebiteur>{0}</numDebiteur>", kundenNummerQST))
			sb.Append(String.Format("<montantBrut>{0:0.00}</montantBrut>", montantBrut))
			sb.Append(String.Format("<montantCommission>{0:0.00}</montantCommission>", montantCommission))

			'' Dateiname (nach Richtlinie)
			'dateiName = String.Format("{0}_{1}_{2}_{3:00}{4:00}_{5:00}{6:00}.XML", _
			'                          listenTypQST, _
			'                          kundenNummerQST, _
			'                          periodeDatumVon.Year, _
			'                          periodeDatumVon.Day, _
			'                          periodeDatumVon.Month, _
			'                          periodeDatumBis.Day, _
			'                          periodeDatumBis.Month)

			'--------------------------------------------------------------------------------------------------------------------
			If Not leereDeklaration Then
				cmdText = "SELECT [MANR] ,[S_Kanton],[S_Gemeinde] "
				cmdText += ",IsNull((SELECT Min([ESAb]) FROM {0} T1 WHERE T1.MANR = T0.MANR And T1.ESAb <> ''),'') As [ESAb] "
				cmdText += ",(SELECT Max([ESEnde]) FROM {0} T1 WHERE T1.MANR = T0.MANR) As [ESEnde] "
				cmdText = String.Format(cmdText, ClsDataDetail.LLTabellennamen)

				cmdText += ",IsNull((SELECT TOP 1 LM.LAIndBez FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 3500 And "
				cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis]),'') As [HeiratDt] "

				cmdText += ",IsNull((SELECT TOP 1 LM.LAIndBez FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 3500.1 And "
				cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis] And "
				cmdText += "(LM.[Jahr Von] = T0.Jahr And LM.LP_VON >= T0.Monat OR "
				cmdText += " LM.[Jahr Bis] = T0.Jahr And LM.LP_BIS <= T0.Monat)),'') As [TodesfallDt] "

				cmdText += ",IsNull((SELECT Sum(LM.M_Btr) FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 1110 And "
				cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis] And "
				cmdText += "(LM.[Jahr Von] = T0.Jahr And LM.LP_VON >= T0.Monat OR "
				cmdText += " LM.[Jahr Bis] = T0.Jahr And LM.LP_BIS <= T0.Monat)),0) As [Gratifikation] "

				cmdText += ",IsNull((SELECT TOP 1 LM.LAIndBez FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 3510 And "
				cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis] And "
				cmdText += "(LM.[Jahr Von] = T0.Jahr And LM.LP_VON >= T0.Monat OR "
				cmdText += " LM.[Jahr Bis] = T0.Jahr And LM.LP_BIS <= T0.Monat)),'') As [GeburtDt] "

				cmdText += ",(SELECT Min(LP) FROM LO WHERE LO.MANR=T0.MANR And LO.Jahr=T0.Jahr And "
				cmdText += " LO.LP>=T0.VonMonat And LO.LP<=T0.BisMonat And LO.QSTTarif=T0.TarifCode ) as VonMonat "

				cmdText += ",(SELECT Max(LP) FROM LO WHERE LO.MANR=T0.MANR And LO.Jahr=T0.Jahr And "
				cmdText += " LO.LP>=T0.VonMonat And LO.LP<=T0.BisMonat And LO.QSTTarif=T0.TarifCode ) as BisMonat "

				cmdText += ",[Nachname],[Vorname],[Jahr],[GebDat] "
				cmdText += ",CASE WHEN LEN([AHV_Nr_New]) = 16 THEN [AHV_Nr_New] ELSE [AHV_Nr] END As AHV_Nr "
				cmdText += ",[Geschlecht],[MAPLZ],[Kinder],[Bewillig],Sum([M_Bas]) As M_Bas,Sum([M_Btr]) As M_Btr "
				cmdText += ",Sum([Bruttolohn]) As [Bruttolohn],Sum([QSTBasis]) As [QSTBasis] "
				cmdText += ",Sum([StdAnz]) As StdAnz,[TarifCode], [Arbeitspensum], lanr "
				cmdText += "INTO #tblTemp "
				cmdText += String.Format("FROM {0} T0 ", ClsDataDetail.LLTabellennamen)
				cmdText += "GROUP BY "
				cmdText += "[MANR],[Monat],[S_Kanton],[S_Gemeinde],[Nachname],[Vorname],[VonMonat],[BisMonat],[Jahr],[GebDat] "
				cmdText += ",[AHV_Nr_New],[AHV_Nr],[Geschlecht],[MAPLZ],[Kinder],[Bewillig],[TarifCode], [Arbeitspensum], lanr "
				cmdText += "ORDER BY Nachname "

				cmdText += "SELECT [MANR] ,[S_Kanton],[S_Gemeinde], "
				cmdText += "(SELECT Min([ESAb]) "
				cmdText += "	FROM #tblTemp T1 "
				cmdText += "	WHERE T1.MANR = T0.MANR) As [ESAb], "
				cmdText += "(SELECT Max([ESEnde]) "
				cmdText += "	FROM #tblTemp T1 "
				cmdText += "	WHERE T1.MANR = T0.MANR) As [ESEnde] "
				cmdText += ",[HeiratDt],[TodesfallDt],[Gratifikation],[GeburtDt],[Nachname],[Vorname], "
				cmdText += "[VonMonat],[BisMonat],[Jahr],[GebDat],[AHV_Nr] "
				cmdText += ",[Geschlecht],[MAPLZ],[Bewillig],Sum([M_Bas]) As M_Bas,Sum([M_Btr]) As M_Btr "
				cmdText += ",Sum([Bruttolohn]) As [Bruttolohn],Sum([QSTBasis]) As [QSTBasis],Sum([StdAnz]) As StdAnz "
				cmdText += ",[TarifCode], [Arbeitspensum], lanr "
				cmdText += "FROM #tblTemp T0 "
				cmdText += "GROUP BY[MANR],[S_Kanton],[S_Gemeinde],[HeiratDt],[TodesfallDt],[Gratifikation],[GeburtDt] "
				cmdText += ",[Nachname],[Vorname],[VonMonat],[BisMonat],[Jahr],[GebDat],[AHV_Nr] "
				cmdText += ",[Geschlecht],[MAPLZ],[Bewillig],[TarifCode], [Arbeitspensum], lanr  "
				cmdText += "ORDER BY Nachname "

				cmd.CommandText = cmdText
				Dim sqlReader As SqlDataReader = cmd.ExecuteReader()
				Dim sequenzNr As Integer = 0
				Dim sequenzNrTemp As Integer = 0 ' End-Tag kann nicht am Ende geschlossen werden, sondern bevor ein Neues beginnt.
				Dim salaerNr As Integer = 0	' Ein Kandidat kann mehere Einträge haben. D.h. 1-mal Identität, aber n-mal Salär,
				Dim manrTemp As String = ""	' weil der Steuer-Code bzw. Ansatz während der Periode ändern kann.

				While sqlReader.Read()
					' SEQUENZ
					Dim manr As String = sqlReader("MANR").ToString
					If manrTemp <> manr Then
						If manrTemp <> "" Then
							' Tag schliessen (ab zweiten Datensatz)
							sb.Append("</decompteContribuable>")
						End If
						manrTemp = manr
						sequenzNr += 1
						salaerNr = 0
						sb.Append(String.Format("<decompteContribuable noSequenceDecompte=""{0}"">", sequenzNr))
						' Identität
						sb.Append("<identite>")
						If kandidatenNummerQST > 0 Then
							' (Wird vom Amt geliefert und nicht eintragen, wenn nicht bekannt)
							sb.Append(String.Format("<numContribuable>{0}</numContribuable>", kandidatenNummerQST))
						End If
						sb.Append(String.Format("<nom>{0}</nom>", sqlReader("Nachname")))
						sb.Append(String.Format("<prenom>{0}</prenom>", sqlReader("Vorname")))
						sb.Append(String.Format("<dateNaissance>{0:yyyy-MM-dd}</dateNaissance>", _
																		DateTime.Parse(sqlReader("GebDat").ToString())))
						Dim geschlecht As Integer = 1
						If sqlReader("Geschlecht").ToString() = "W" Then
							geschlecht = 2
						End If
						sb.Append(String.Format("<codeSexe>{0}</codeSexe>", geschlecht))
						' AHV-Nr: Es dürfen keine Punkte mitgegeben werden.
						sb.Append(String.Format("<numAvs>{0}</numAvs>", sqlReader("AHV_Nr").ToString().Replace(".", "").Replace(" ", "")))
						sb.Append(String.Format("<permisTravail>{0:00}</permisTravail>", _
																		ClsDataDetail.GetBasiskategorie(sqlReader("Bewillig"))))
						Dim liMDData As New List(Of String)
						liMDData = GetMDData()
						Dim strGemeinde As String = liMDData(11).ToString
						If String.IsNullOrEmpty(sqlReader("S_Gemeinde")) Then
							strGemeinde = liMDData(11).ToString
						Else
							If sqlReader("S_Gemeinde").ToString.Length <= 4 Then
								strGemeinde = sqlReader("S_Gemeinde")
							End If
						End If
						sb.Append(String.Format("<commune>{0}</commune>", Val(strGemeinde))) ' _

						'sb.Append(String.Format("<commune>{0}</commune>", _
						'                        If(sqlReader("MAPLZ").ToString.Length > 4, _
						'                           liMDData(11).ToString, sqlReader("MAPLZ"))))
						'sb.Append("<identifiantSalarieChezEmployeur></identifiantSalarieChezEmployeur>") ' OBSOLET
						sb.Append("</identite>")
					End If
					' --> salaire (Salär)
					salaerNr += 1
					sb.Append(String.Format("<salaire noSequenceSalaire=""{0}""", salaerNr))
					' Einzahlungs-Periode
					Dim esAb As String = sqlReader("ESAb").ToString()
					Dim esEnde As String = sqlReader("ESEnde").ToString()
					If esAb.Length > 0 Then
						esAb = String.Format("{0:yyyy-MM-dd}", DateTime.Parse(esAb))
					Else
						Dim beginnDt As DateTime = DateTime.Parse(String.Format("1.{0}.{1}", sqlReader("VonMonat"), sqlReader("Jahr")))
						esAb = String.Format("{0:yyyy-MM-dd}", beginnDt)
					End If
					If esEnde.Length > 0 Then
						esEnde = String.Format("{0:yyyy-MM-dd}", DateTime.Parse(esEnde))
					Else
						Dim endeDt As DateTime = DateTime.Parse(String.Format("1.{0}.{1}", sqlReader("BisMonat"), sqlReader("Jahr")))
						esEnde = String.Format("{0:yyyy-MM-dd}", endeDt.AddMonths(1).AddDays(-1))
					End If
					sb.Append(String.Format(" debutVersement=""{0}"" finVersement=""{1}"">", _
																	esAb, esEnde))
					'--> revenuNonProportionel (Gratifikation)
					sb.Append(String.Format("<revenuNonProportionel>{0:0.00}</revenuNonProportionel>", _
																	CDec(sqlReader("Gratifikation"))))
					sb.Append(String.Format("<salaireVerseOuPrestationImposable>{0:0.00}</salaireVerseOuPrestationImposable>", _
																	0))	' PSEUDO
					sb.Append(String.Format("<retenueDImpot>{0:0.00}</retenueDImpot>", _
																	0))	' PSEUDO
					'--> Bareme (Tarifcode)
					Dim tarifCode As String = sqlReader("TarifCode").ToString().PadRight(3)
					Dim code As String = tarifCode.Substring(0, 1)
					Dim kinder As String = tarifCode.Substring(1, 1)
					'LANR=7600 Or LANR=7620
					If Not sqlReader("LANr").ToString.Contains("7600") And Not sqlReader("LANr").ToString.Contains("7620") Then
						code = "A"
					End If
					sb.Append("<bareme>")
					sb.Append(String.Format("<nombreAllocations>{0}</nombreAllocations>", kinder))
					Dim activite As TypeActivite = TypeActivite.principale
					If code = "D" Then
						activite = TypeActivite.accessoire
					End If
					sb.Append(String.Format("<typeActivite>{0}</typeActivite>", activite.ToString()))
					Dim arbeitspensum As Integer = 100
					If sqlReader("Arbeitspensum").ToString().Length > 0 Then
						arbeitspensum = CInt(sqlReader("Arbeitspensum"))
					End If
					sb.Append(String.Format("<tauxActivite>{0}</tauxActivite>", arbeitspensum))
					sb.Append(String.Format("<codeBareme>{0}{1}</codeBareme>", code, Val(kinder)))
					sb.Append("</bareme>")
					' --> evenement (Ereignis)
					' Ereignis (Eintritt)
					Dim ereignis As String = "A-Entrée"
					Dim ereignisDt As String = sqlReader("ESAb").ToString()
					' Ereignis (Todesfall)
					If ereignisDt.Length = 0 Then
						ereignis = "I-Décès"
						ereignisDt = sqlReader("TodesfallDt").ToString().Split(CChar(" "))(0)	' Nur Datum übernehmen
					End If
					' Ereignis (Heirat)
					If ereignisDt.Length = 0 Then
						ereignis = "C-Mariage"
						ereignisDt = sqlReader("HeiratDt").ToString().Split(CChar(" "))(0) ' Nur Datum übernehmen
						If ereignisDt.Length > 0 Then
							If Not (Month(DateTime.Parse(ereignisDt)) >= CInt(sqlReader("VonMonat")) And _
													 Month(DateTime.Parse(ereignisDt)) <= CInt(sqlReader("BisMonat"))) Then
								ereignisDt = ""
							End If
						End If
					End If
					' Ereignis (Austritt)
					If ereignisDt.Length = 0 Then
						ereignis = "B-Sortie"
						ereignisDt = sqlReader("ESEnde").ToString()
					End If
					' Ereignis (Kinder)
					If ereignisDt.Length = 0 Then
						If kinder = "0" Then
							ereignis = "E-FinDroitAllocation"
						Else
							ereignis = "D-DébutDroitAllocation"
						End If
						ereignisDt = sqlReader("GeburtDt").ToString().Split(CChar(" "))(0) ' Nur Datum übernehmen
						If ereignisDt.Length > 0 Then
							If Not (Month(DateTime.Parse(ereignisDt)) >= CInt(sqlReader("VonMonat")) And _
													 Month(DateTime.Parse(ereignisDt)) <= CInt(sqlReader("BisMonat"))) Then
								ereignisDt = ""
							End If
						End If
					End If
					If ereignisDt.Length > 0 Then
						sb.Append(String.Format("<evenement date=""{0:yyyy-MM-dd}"">{1}</evenement>", DateTime.Parse(ereignisDt), ereignis))
					End If
					sb.Append("</salaire>")


				End While

				' Letzer XML-Tag muss geschlossen werden
				sb.Append("</decompteContribuable>")
			End If
			'--------------------------------------------------------------------------------------------------------------------

			' XML-Datei abschliessen
			sb.Append("</tns:listeImpotSource>")

			' XML-Datei schreiben
			Dim objDateiMacher As StreamWriter
			Dim path As String = strFullFilename ' _ClsProgSetting.GetPersonalFolder() & dateiName
			objDateiMacher = New StreamWriter(path)
			objDateiMacher.Write(sb.ToString)
			objDateiMacher.Close()
			objDateiMacher.Dispose()

			'MessageBox.Show(String.Format("Die Quellensteuerliste wurde im Verzeichnis: {0}{1}{0} erfolgreich gespeichert.", _
			'                              vbLf, _
			'                              path), "Quellensteuerliste speichern")

		Catch ex As Exception
			'_ex.MessageBoxShowError(iLogedUSNr, "XML-Datei erstellen", ex)

		End Try

	End Sub

	Private Function GetMDData() As List(Of String)
		Dim liMDData As New List(Of String)
		Dim Conn As New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Conn.Open()

		Dim sSql As String = "Select Top 1 USNr, Anrede As USAnrede, Nachname As USNachname, Vorname As USVorname, eMail As USeMail, Telefon As USTelefon, Telefax As USTelefax From Benutzer Where USNr = " & _ClsProgSetting.GetLogedUSNr()
		Dim SQLOffCmd As SqlCommand = New SqlCommand(sSql, Conn)
		Dim rTemprec As SqlDataReader = SQLOffCmd.ExecuteReader					 ' Offertendatenbank

		Try

			rTemprec.Read()
			With liMDData
				.Add(rTemprec("USAnrede").ToString)
				.Add(rTemprec("USeMail").ToString)
				.Add(rTemprec("USNachname").ToString)
				.Add(rTemprec("USVorname").ToString)
				.Add(rTemprec("USTelefon").ToString)
				.Add(rTemprec("USTelefax").ToString)

			End With
			rTemprec.Close()

			sSql = "[Get MDData For Header] " & Year(Now) & ", " & _ClsProgSetting.GetLogedUSNr()
			SQLOffCmd = New SqlCommand(sSql, Conn)
			rTemprec = SQLOffCmd.ExecuteReader					' Offertendatenbank
			rTemprec.Read()

			With liMDData
				.Add(rTemprec("MDName").ToString)
				.Add(rTemprec("MDName2").ToString)
				.Add(rTemprec("MDName3").ToString)
				.Add(rTemprec("MDPostfach").ToString)
				.Add(rTemprec("MDStrasse").ToString)
				.Add(rTemprec("MDPLZ").ToString)				' 11
				.Add(rTemprec("MDOrt").ToString)
				.Add(rTemprec("MDLand").ToString)

				.Add(rTemprec("MDTelefon").ToString)
				.Add(rTemprec("MDTelefax").ToString)
				.Add(rTemprec("MDeMail").ToString)
				.Add(rTemprec("MDHomepage").ToString)

			End With

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetMDDataForPrint")

		Finally
			rTemprec.Close()
			Conn.Dispose()

		End Try

		Return liMDData
	End Function

	Function ExportQstXmlVd(ByVal listenTypQST As String, ByVal leereDeklaration As Boolean, ByVal strFullFilename As String) As Boolean

		Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim cmd As SqlCommand = New SqlCommand("", conn)
		Dim cmdText As String = String.Empty
		Dim strQstArt As String = If(listenTypQST.ToLower.Contains("korrektur"), "LC", "LR")

		Try
			conn.Open()

			' Summe der Beiträge
			Dim amountGross As Decimal = 0
			Try
				cmdText = String.Format("SELECT Sum(M_Btr) As amountGross FROM {0} ", ClsDataDetail.LLTabellennamen)
				cmd.CommandText = cmdText
				amountGross = CDec(cmd.ExecuteScalar()) * -1
			Catch ex As Exception
				MsgBox(String.Format("Keine Lohndaten wurden gefunden.{0}{1}{0}{2}", vbNewLine, ex.Message, cmdText), _
					MsgBoxStyle.Critical, "Daten exportieren")
				Return False
			End Try

			' Komission
			Dim amountCommission As Decimal = amountGross * 0.03 ' TODO: 3% der Summe der Beiträge

			Dim sb As New System.Text.StringBuilder
			Dim dateiName As String = "XMLDatei.XML"
			Dim kundenNummerQST As Integer = 1501495 ' TODO: Wird vom Amt geliefert  (SWAG Kundennummer hartcodiert)
			Dim kandidatenNummerQST As Integer = 0 ' TODO: Wird vom Amt geliefert 
			Dim periodeDatumVon As DateTime = ClsDataDetail.SelPeriodeVon
			Dim periodeDatumBis As DateTime = ClsDataDetail.SelPeriodeBis

			' META-DATEN
			sb.Append("<?xml version=""1.0"" encoding=""UTF-8"" ?> ")
			sb.Append("<tns:listeImpotSource xmlns:tns=""http://www.vd.ch/fiscalite/impotsource/liste-impot-source/4"" ")
			sb.Append("xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" ")
			sb.Append("xsi:schemaLocation=""http://www.vd.ch/fiscalite/impotsource/liste-impot-source/4")
			sb.Append(" D:\registre\empaci\trunk\04-Impl\xmlgen\src\main\resources\xsdEmpaci\v4\liste-impot-source-4.xsd"" ")
			sb.Append(String.Format("debutPeriodeDeclaration=""{0:yyyy-MM-dd}"" finPeriodeDeclaration=""{1:yyyy-MM-dd}"">", _
															periodeDatumVon, _
															periodeDatumBis))
			sb.Append(String.Format("<typeListe>{0}</typeListe>", strQstArt))	' listenTypQST))
			sb.Append(String.Format("<numDebiteur>{0}</numDebiteur>", kundenNummerQST))
			sb.Append(String.Format("<montantBrut>{0:0.00}</montantBrut>", amountGross * If(amountGross < 0, 1, 1)))
			sb.Append(String.Format("<montantCommission>{0:0.00}</montantCommission>", amountCommission * If(amountGross < 0, -1, 1)))

			' Dateiname (nach Richtlinie)
			dateiName = String.Format("{0}_{1}_{2}_{3:00}{4:00}_{5:00}{6:00}.XML", _
																strQstArt, _
																kundenNummerQST, _
																periodeDatumVon.Year, _
																periodeDatumVon.Day, _
																periodeDatumVon.Month, _
																periodeDatumBis.Day, _
																periodeDatumBis.Month)

			If Not leereDeklaration Then
				cmdText = "SELECT [MANR], IsNull((SELECT Min([ESAb]) "
				cmdText += "FROM {0} T1 WHERE T1.MANR = T0.MANR And T1.ESAb <> ''),'') As [ESAb] "
				cmdText += ",(SELECT Max([ESEnde]) FROM {0} T1 WHERE T1.MANR = T0.MANR) As [ESEnde] "
				cmdText = String.Format(cmdText, ClsDataDetail.LLTabellennamen)

				cmdText += ",IsNull((SELECT TOP 1 LM.LAIndBez FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 3500 And "
				cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis]),'') As [HeiratDt] "

				cmdText += ",IsNull((SELECT TOP 1 LM.LAIndBez FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 3500.1 And "
				cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis] And "
				cmdText += "(LM.[Jahr Von] = T0.Jahr And LM.LP_VON >= T0.Monat OR "
				cmdText += " LM.[Jahr Bis] = T0.Jahr And LM.LP_BIS <= T0.Monat)),'') As [TodesfallDt] "

				cmdText += ",IsNull((SELECT Sum(LM.M_Btr) FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 1110 And "
				cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis] And "
				cmdText += "(LM.[Jahr Von] = T0.Jahr And LM.LP_VON >= T0.Monat OR "
				cmdText += "LM.[Jahr Bis] = T0.Jahr And LM.LP_BIS <= T0.Monat)),0) As [Gratifikation], "

				cmdText += "IsNull((SELECT TOP 1 LM.LAIndBez FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 3510 And "
				cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis] And "
				cmdText += "(LM.[Jahr Von] = T0.Jahr And LM.LP_VON >= T0.Monat OR "
				cmdText += " LM.[Jahr Bis] = T0.Jahr And LM.LP_BIS <= T0.Monat)), '') As [GeburtDt], "

				cmdText += "ISNull((SELECT Min(LP) FROM LO WHERE LO.MANR = T0.MANR And LO.Jahr = T0.Jahr And "
				cmdText += "LO.LP >= T0.VonMonat And LO.LP <= T0.BisMonat And LO.QSTTarif = T0.TarifCode), "
				cmdText &= "(SELECT Min(LP) FROM LOL WHERE LOL.MANR = T0.MANR And LOL.Jahr = T0.Jahr And LOL.LP >= T0.VonMonat And LOL.LP <= T0.BisMonat And LOL.LANr = T0.LANr)) as VonMonat, "

				cmdText += "ISNull((SELECT Max(LP) FROM LO WHERE LO.MANR = T0.MANR And LO.Jahr = T0.Jahr And "
				cmdText += "LO.LP >= T0.VonMonat And LO.LP <= T0.BisMonat And LO.QSTTarif = T0.TarifCode), "
				cmdText &= "(SELECT Max(LP) FROM LOL WHERE LOL.MANR = T0.MANR And LOL.Jahr = T0.Jahr And LOL.LP >= T0.VonMonat And LOL.LP <= T0.BisMonat And LOL.LANr = T0.LANr)) as BisMonat, "

				cmdText += "[Nachname],[Vorname],[Jahr],[GebDat], "
				cmdText += "CASE WHEN LEN([AHV_Nr_New]) = 16 THEN [AHV_Nr_New] ELSE [AHV_Nr] END As AHV_Nr, "
				cmdText += "[Geschlecht],[MAPLZ],[Kinder],[Bewillig],Sum([M_Bas]) As M_Bas,Sum([M_Btr]) As M_Btr, "
				cmdText += "Sum([Bruttolohn]) As [Bruttolohn],Sum([QSTBasis]) As [QSTBasis], "
				cmdText += "Sum([StdAnz]) As StdAnz,[TarifCode], [Arbeitspensum], lanr "
				cmdText += "INTO #tblTemp "
				cmdText += String.Format("FROM {0} T0 ", ClsDataDetail.LLTabellennamen)
				cmdText += "GROUP BY "
				cmdText += "[MANR], [Monat], [Nachname], [Vorname], [VonMonat], [BisMonat], [Jahr], [GebDat] "
				cmdText += ",[AHV_Nr_New],[AHV_Nr],[Geschlecht],[MAPLZ],[Kinder],[Bewillig],[TarifCode], [Arbeitspensum], lanr "
				cmdText += "ORDER BY Nachname "

				cmdText += "SELECT [MANR], (SELECT QSTGemeinde FROM Mitarbeiter MA WHERE MA.MANR = T0.MANR) As [S_Gemeinde], "
				cmdText += "(SELECT Min([ESAb]) FROM #tblTemp T1 WHERE T1.MANR = T0.MANR) As [ESAb], "
				cmdText += "(SELECT Max([ESEnde]) FROM #tblTemp T1 WHERE T1.MANR = T0.MANR) As [ESEnde], "
				cmdText += "[HeiratDt], [TodesfallDt], [Gratifikation], [GeburtDt],[Nachname],[Vorname], "
				cmdText += "[VonMonat], [BisMonat], [Jahr], [GebDat], [AHV_Nr], "
				cmdText += "[Geschlecht], [MAPLZ], [Bewillig], Sum([M_Bas]) As M_Bas, Sum([M_Btr]) As M_Btr, "
				cmdText += "Sum([Bruttolohn]) As [Bruttolohn], Sum([QSTBasis]) As [QSTBasis], Sum([StdAnz]) As StdAnz, "
				cmdText += "[TarifCode], [Arbeitspensum], lanr "
				'If listenTypQST = "LC" Or listenTypQST = "Korrektur" Then
				'If listenTypQST.ToLower.Contains("korrektur") Then
				'  cmdText += "FROM #tblTemp T0 Where LANr Not In (7600, 7620) "
				'Else
				'  cmdText += "FROM #tblTemp T0 Where LANr In (7600, 7620) "
				'End If
				cmdText += "FROM #tblTemp T0 "

				cmdText += "GROUP BY [MANR], [HeiratDt], [TodesfallDt], [Gratifikation], [GeburtDt], "
				cmdText += "[Nachname], [Vorname], [VonMonat], [BisMonat], [Jahr], [GebDat], [AHV_Nr], "
				cmdText += "[Geschlecht], [MAPLZ], [Bewillig], [TarifCode], [Arbeitspensum], lanr  "
				cmdText += "ORDER BY Nachname, Vorname"

				cmd.CommandText = cmdText
				Dim sqlReader As SqlDataReader = cmd.ExecuteReader()
				Dim sequenzNr As Integer = 0
				Dim sequenzNrTemp As Integer = 0 ' End-Tag kann nicht am Ende geschlossen werden, sondern bevor ein Neues beginnt.
				Dim salaerNr As Integer = 0	' Ein Kandidat kann mehere Einträge haben. D.h. 1-mal Identität, aber n-mal Salär,
				Dim manrTemp As String = ""	' weil der Steuer-Code bzw. Ansatz während der Periode ändern kann.

				While sqlReader.Read()
					' SEQUENZ
					Dim manr As String = sqlReader("MANR").ToString
					If manrTemp <> manr Then
						If manrTemp <> "" Then
							' Tag schliessen (ab zweiten Datensatz)
							sb.Append("</decompteContribuable>")
						End If
						manrTemp = manr
						sequenzNr += 1
						salaerNr = 0
						sb.Append(String.Format("<decompteContribuable noSequenceDecompte=""{0}"">", sequenzNr))
						' Identität
						sb.Append("<identite>")
						If kandidatenNummerQST > 0 Then
							' (Wird vom Amt geliefert und nicht eintragen, wenn nicht bekannt)
							sb.Append(String.Format("<numContribuable>{0}</numContribuable>", kandidatenNummerQST))
						End If
						sb.Append(String.Format("<nom>{0}</nom>", sqlReader("Nachname")))
						sb.Append(String.Format("<prenom>{0}</prenom>", sqlReader("Vorname")))
						sb.Append(String.Format("<dateNaissance>{0:yyyy-MM-dd}</dateNaissance>", _
																		DateTime.Parse(sqlReader("GebDat").ToString())))
						Dim geschlecht As Integer = 1
						If sqlReader("Geschlecht").ToString() = "W" Then
							geschlecht = 2
						End If
						sb.Append(String.Format("<codeSexe>{0}</codeSexe>", geschlecht))
						' AHV-Nr: Es dürfen keine Punkte mitgegeben werden.
						sb.Append(String.Format("<numAvs>{0}</numAvs>", sqlReader("AHV_Nr").ToString().Replace(".", "").Replace(" ", "")))
						sb.Append(String.Format("<permisTravail>{0:00}</permisTravail>", _
																		ClsDataDetail.GetBasiskategorie(sqlReader("Bewillig"))))

						Dim liMDData As New List(Of String)
						liMDData = GetMDData()

						Dim strGemeinde As String = liMDData(11).ToString
						If String.IsNullOrEmpty(sqlReader("S_Gemeinde")) Then
							strGemeinde = liMDData(11).ToString
						Else
							If sqlReader("S_Gemeinde").ToString.Length <= 4 Then
								strGemeinde = sqlReader("S_Gemeinde")
							End If
						End If
						If sqlReader("MANr") = 4128 Then
							Trace.WriteLine(strGemeinde)
						End If
						sb.Append(String.Format("<commune>{0}</commune>", Val(strGemeinde))) ' _
						'If(sqlReader("S_Gemeinde").ToString.Length > 4, _
						'   liMDData(11).ToString, sqlReader("S_Gemeinde"))))
						'sb.Append("<identifiantSalarieChezEmployeur></identifiantSalarieChezEmployeur>") ' OBSOLET
						sb.Append("</identite>")
					End If
					' --> salaire (Salär)
					salaerNr += 1
					sb.Append(String.Format("<salaire noSequenceSalaire=""{0}""", salaerNr))
					' Einzahlungs-Periode
					Dim esAb As String = sqlReader("ESAb").ToString()
					Dim esEnde As String = sqlReader("ESEnde").ToString()

					If sqlReader("MANr") = 4065 Then
						'MsgBox("Phase0: " & salaerNr & ": " & esAb & vbTab & esEnde)
					End If

					Dim strVonmonat As String = sqlReader("VonMonat").ToString()
					If strVonmonat.Length = 0 Then strVonmonat = periodeDatumVon.Month
					Dim strBismonat As String = sqlReader("BisMonat").ToString()
					If strBismonat.Length = 0 Then strBismonat = periodeDatumBis.Month

					If sqlReader("MANr") = 4088 Then
						'MsgBox("Phase1: " & salaerNr & ": " & esAb & vbTab & esEnde)
					End If

					Dim beginnDt As DateTime = DateTime.Parse(String.Format("1.{0}.{1}", strVonmonat, sqlReader("Jahr")))
					Dim endeDt As DateTime = DateTime.Parse(String.Format("1.{0}.{1}", strBismonat, sqlReader("Jahr")))
					If esAb.Length > 0 Then
						'esAb = String.Format("{0:yyyy-MM-dd}", DateTime.Parse(esAb))
						esAb = esAb
					Else
						esAb = beginnDt
					End If
					If sqlReader("MANr") = 4088 Then
						'MsgBox("Phase2: " & salaerNr & ": " & esAb & vbTab & esEnde)
					End If

					If esEnde.Length > 0 Then
						esEnde = esEnde
					Else
						esEnde = endeDt.AddMonths(1).AddDays(-1)
					End If
					If sqlReader("MANr") = 4088 Then
						'MsgBox("Phase3: " & salaerNr & ": " & esAb & vbTab & esEnde)
					End If

					If CDate(esAb) > CDate(esEnde) Then
						esEnde = esAb
						esAb = beginnDt
					End If
					If sqlReader("MANr") = 4088 Then
						'MsgBox("Phase4: " & salaerNr & ": " & esAb & vbTab & esEnde)
					End If

					If esAb < beginnDt Then esAb = beginnDt
					'If esEnde > endeDt Then esEnde = endeDt
					If sqlReader("MANr") = 4065 Then
						'MsgBox("Phase5: " & salaerNr & ": " & esAb & vbTab & esEnde)
					End If

					sb.Append(String.Format(" debutVersement=""{0}"" finVersement=""{1}"">", _
																	String.Format("{0:yyyy-MM-dd}", DateTime.Parse(esAb)), _
																	String.Format("{0:yyyy-MM-dd}", DateTime.Parse(esEnde))))
					'--> revenuNonProportionel (Gratifikation)
					sb.Append(String.Format("<revenuNonProportionel>{0:0.00}</revenuNonProportionel>", _
																	CDec(sqlReader("Gratifikation"))))
					sb.Append(String.Format("<salaireVerseOuPrestationImposable>{0:0.00}</salaireVerseOuPrestationImposable>", _
																	CDec(sqlReader("QSTBasis"))))
					sb.Append(String.Format("<retenueDImpot>{0:0.00}</retenueDImpot>", _
																	Decimal.Parse(sqlReader("M_Btr").ToString()) * -1))	' (If(sqlReader("M_Btr") < 0, -1, 1))))
					'--> Bareme (Tarifcode)
					Dim tarifCode As String = sqlReader("TarifCode").ToString().PadRight(3)
					Dim code As String = tarifCode.Substring(0, 1)
					Dim kinder As String = tarifCode.Substring(1, 1)
					'LANR=7600 Or LANR=7620
					If Not sqlReader("LANr").ToString.Contains("7600") And Not sqlReader("LANr").ToString.Contains("7620") Then
						code = "A"
					End If
					sb.Append("<bareme>")
					sb.Append(String.Format("<nombreAllocations>{0}</nombreAllocations>", Val(kinder)))
					Dim activite As TypeActivite = TypeActivite.principale
					If code = "D" Then
						activite = TypeActivite.accessoire
					End If
					sb.Append(String.Format("<typeActivite>{0}</typeActivite>", activite.ToString()))
					Dim arbeitspensum As Integer = 100
					If sqlReader("Arbeitspensum").ToString().Length > 0 Then
						arbeitspensum = CInt(sqlReader("Arbeitspensum"))
					End If
					sb.Append(String.Format("<tauxActivite>{0}</tauxActivite>", arbeitspensum))
					sb.Append(String.Format("<codeBareme>{0}{1}</codeBareme>", code, Val(kinder)))
					sb.Append("</bareme>")
					' --> evenement (Ereignis)

					If sqlReader("MANr") = 4372 Then
						Trace.WriteLine("Phase5.1: A-Entrée" & salaerNr & ": " & esAb & vbTab & esEnde)
					End If
					' Ereignis (Eintritt)
					Dim ereignis As String = "A-Entrée"
					Dim ereignisDt As String = sqlReader("ESAb").ToString()
					If ereignisDt.Length > 0 Then
						If Not (Month(DateTime.Parse(ereignisDt)) >= CInt(sqlReader("VonMonat")) And _
														 Month(DateTime.Parse(ereignisDt)) <= CInt(sqlReader("BisMonat"))) Then
							ereignisDt = ""
						ElseIf Not Month(DateTime.Parse(ereignisDt)) < CInt(sqlReader("VonMonat")) Then
							ereignisDt = ""
						End If
					End If

					' Ereignis (Todesfall)
					If ereignisDt.Length = 0 Then
						ereignis = "I-Décès"
						ereignisDt = sqlReader("TodesfallDt").ToString().Split(CChar(" "))(0)	' Nur Datum übernehmen
					End If
					' Ereignis (Heirat)
					If ereignisDt.Length = 0 Then
						ereignis = "C-Mariage"
						ereignisDt = sqlReader("HeiratDt").ToString().Split(CChar(" "))(0) ' Nur Datum übernehmen
						If ereignisDt.Length > 0 Then
							If Not (Month(DateTime.Parse(ereignisDt)) >= CInt(sqlReader("VonMonat")) And _
													 Month(DateTime.Parse(ereignisDt)) <= CInt(sqlReader("BisMonat"))) Then
								ereignisDt = ""
							End If
						End If
					End If
					' Ereignis (Austritt)
					If ereignisDt.Length = 0 Then
						ereignis = "B-Sortie"
						ereignisDt = sqlReader("ESEnde").ToString()
					End If
					' Ereignis (Kinder)
					If ereignisDt.Length = 0 Then
						If kinder = "0" Then
							ereignis = "E-FinDroitAllocation"
						Else
							ereignis = "D-DébutDroitAllocation"
						End If
						ereignisDt = sqlReader("GeburtDt").ToString().Split(CChar(" "))(0) ' Nur Datum übernehmen
						If ereignisDt.Length > 0 Then
							If Not (Month(DateTime.Parse(ereignisDt)) >= CInt(sqlReader("VonMonat")) And _
													 Month(DateTime.Parse(ereignisDt)) <= CInt(sqlReader("BisMonat"))) Then
								ereignisDt = ""
							End If
						End If
					End If
					If ereignisDt.Length > 0 Then
						sb.Append(String.Format("<evenement date=""{0:yyyy-MM-dd}"">{1}</evenement>", DateTime.Parse(ereignisDt), ereignis))
					End If
					sb.Append("</salaire>")


				End While

				' Letzer XML-Tag muss geschlossen werden
				sb.Append("</decompteContribuable>")
			End If
			'--------------------------------------------------------------------------------------------------------------------

			' XML-Datei abschliessen
			sb.Append("</tns:listeImpotSource>")

			' XML-Datei schreiben
			Dim objDateiMacher As StreamWriter
			Dim path As String = strFullFilename
			objDateiMacher = New StreamWriter(path)
			objDateiMacher.Write(sb.ToString)
			objDateiMacher.Close()
			objDateiMacher.Dispose()

		Catch ex As Exception
			'_ex.MessageBoxShowError(iLogedUSNr, "XML-Datei erstellen", ex)
			Return False
		End Try

		Return True
	End Function

	''' <summary>
	''' Exportiert die Quellensteuerdaten in ein XML File für den Kanton Genf.
	''' </summary>
	''' <param name="mdNr">Mandant Nummber</param>
	''' <param name="periodDateFrom">Periode Datum von</param>
	''' <param name="periodDateTo">Periode Datum bis</param>
	''' <param name="listType">Listentyp 1...6</param>
	''' <param name="strFullFilename">Dateiname mit Pfad</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function ExportQstXmlGe(
		ByVal mdNr As Integer,
		ByVal periodDateFrom As Date,
		ByVal periodDateTo As Date,
		ByVal listType As Integer,
		ByVal strFullFilename As String
		) As ClsExtendedResult

		Dim result As New ClsExtendedResult()
		Dim utilities As New SPProgUtility.MainUtilities.Utilities()
		Dim utility_Infrastructure As New SP.Infrastructure.Utility


		Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim cmd As SqlCommand = New SqlCommand("", conn)
		Dim cmdText As String = String.Empty
		Dim sqlStringBuilder As StringBuilder

		Dim maNr As Integer = 0
		Dim employeeName As String = String.Empty


		Try
			conn.Open()

			' Summe der Beiträge
			Dim amountGross As Decimal = 0
			Try
				cmdText = String.Format("SELECT Sum(M_Btr) As amountGross FROM {0} ", ClsDataDetail.LLTabellennamen)
				cmd.CommandText = cmdText
				amountGross = CDec(cmd.ExecuteScalar()) * -1
			Catch ex As Exception
				result.AddError("Keine Lohndaten wurden gefunden.{0}{1}{0}{2}", vbNewLine, ex.Message, cmdText)
				Return result
			End Try

			' Komission
			Dim amountCommission As Decimal = amountGross * 0.03 ' 3% der Summe der Beiträge

			' Start typisierte XML Generierung

			' XSD Hauptelement
			Dim qstExportGe As DeclarationListeRecapitulative_Type2014 = New DeclarationListeRecapitulative_Type2014()

			' Steuerpflichtige Firma (DPI - Débiteurs de prestations imposables)
			Dim dpi As New IdentifiantDPI_Type()

			sqlStringBuilder = New StringBuilder()
			sqlStringBuilder.AppendLine("SELECT TOP 1 [MD_Name1], [Strasse], [PLZ], [Ort], [Land] ")
			sqlStringBuilder.AppendLine("FROM [Mandanten]")
			sqlStringBuilder.AppendLine("WHERE [MDNr] = @MDNr AND [Jahr] = @Jahr")
			cmd.CommandText = sqlStringBuilder.ToString()
			cmd.Parameters.AddWithValue("@MDNr", mdNr)
			cmd.Parameters.AddWithValue("@Jahr", periodDateFrom.Year)
			Using sqlReader As SqlDataReader = cmd.ExecuteReader()
				If sqlReader.Read() Then
					' TODO: Mandant Identifikationsnummer (max. 6 Stellen)
					dpi.numeroDPI = GetTaxCustomerNumber("GE")
					' TODO: Exakte Bezeichnung Hauptsitz
					dpi.nomRaisonSocialeDPI = sqlReader("MD_Name1").ToString()
					Dim adresseDpi = New AdresseDPI_Type()
					adresseDpi.CO = sqlReader("MD_Name1").ToString()
					Dim voieCasePostale As New VoieCasePostale_Type
					voieCasePostale.Items = New Object() {
							New Voie_Type() With {
								.voie = sqlReader("Strasse").ToString()
							}
						}
					voieCasePostale.ItemsElementName = New ItemsChoiceType() {
						ItemsChoiceType.voieSeule
					}
					adresseDpi.voieCasePostale = voieCasePostale
					adresseDpi.NPA = sqlReader("PLZ").ToString()
					adresseDpi.localite = sqlReader("Ort").ToString()
					adresseDpi.pays = Xsd.Ge.QstExportGe.GetPaysOuSuisse(sqlReader("Land").ToString())
					dpi.adresseDPI = adresseDpi
				End If
			End Using

			m_Logger.LogDebug("1. starting...")

			Dim periodeImpositionDPI As New PeriodeImposition_Type()
			periodeImpositionDPI.debut_imposition = periodDateFrom.ToString("dd.MM.yyyy")
			periodeImpositionDPI.fin_imposition = periodDateTo.ToString("dd.MM.yyyy")
			dpi.periodeImpositionDPI = periodeImpositionDPI

			qstExportGe.DPI = dpi

			cmdText = "SELECT [MANR], IsNull((SELECT Min([ESAb]) "
			cmdText += "FROM {0} T1 WHERE T1.MANR = T0.MANR And T1.ESAb <> ''),'') As [ESAb] "
			cmdText += ",(SELECT Max([ESEnde]) FROM {0} T1 WHERE T1.MANR = T0.MANR) As [ESEnde] "
			cmdText = String.Format(cmdText, ClsDataDetail.LLTabellennamen)

			cmdText += ",IsNull((SELECT TOP 1 LM.LAIndBez FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 3500 And "
			cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis]),'') As [HeiratDt] "

			cmdText += ",IsNull((SELECT TOP 1 LM.LAIndBez FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 3500.1 And "
			cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis] And "
			cmdText += "(LM.[Jahr Von] = T0.Jahr And LM.LP_VON >= T0.Monat OR "
			cmdText += " LM.[Jahr Bis] = T0.Jahr And LM.LP_BIS <= T0.Monat)),'') As [TodesfallDt] "

			cmdText += ",IsNull((SELECT Sum(LM.M_Btr) FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 1110 And "
			cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis] And "
			cmdText += "(LM.[Jahr Von] = T0.Jahr And LM.LP_VON >= T0.Monat OR "
			cmdText += "LM.[Jahr Bis] = T0.Jahr And LM.LP_BIS <= T0.Monat)),0) As [Gratifikation], "

			cmdText += "IsNull((SELECT TOP 1 LM.LAIndBez FROM LM WHERE LM.MANR = T0.MANR And LM.LANR = 3510 And "
			cmdText += "T0.Jahr Between LM.[Jahr Von] And LM.[Jahr Bis] And "
			cmdText += "(LM.[Jahr Von] = T0.Jahr And LM.LP_VON >= T0.Monat OR "
			cmdText += " LM.[Jahr Bis] = T0.Jahr And LM.LP_BIS <= T0.Monat)), '') As [GeburtDt], "

			cmdText += "ISNull((SELECT Min(LP) FROM LO WHERE LO.MANR = T0.MANR And LO.Jahr = T0.Jahr And "
			cmdText += "LO.LP >= T0.VonMonat And LO.LP <= T0.BisMonat And LO.QSTTarif = T0.TarifCode), "
			cmdText &= "(SELECT Min(LP) FROM LOL WHERE LOL.MANR = T0.MANR And LOL.Jahr = T0.Jahr And LOL.LP >= T0.VonMonat And LOL.LP <= T0.BisMonat And LOL.LANr = T0.LANr)) as VonMonat, "

			cmdText += "ISNull((SELECT Max(LP) FROM LO WHERE LO.MANR = T0.MANR And LO.Jahr = T0.Jahr And "
			cmdText += "LO.LP >= T0.VonMonat And LO.LP <= T0.BisMonat And LO.QSTTarif = T0.TarifCode), "
			cmdText &= "(SELECT Max(LP) FROM LOL WHERE LOL.MANR = T0.MANR And LOL.Jahr = T0.Jahr And LOL.LP >= T0.VonMonat And LOL.LP <= T0.BisMonat And LOL.LANr = T0.LANr)) as BisMonat, "

			cmdText += "[Nachname],[Vorname],[Jahr],[GebDat], "
			cmdText += "[AHV_Nr], [AHV_Nr_New], [ESStrasse], [ESPLZ], [ESOrt], [ESKanton], "
			cmdText += "[Geschlecht],[Zivilstand],[MAStrasse],[MAPLZ],[MAOrt],[Kinder],[Bewillig],Sum([M_Bas]) As M_Bas,Sum([M_Btr]) As M_Btr, "
			cmdText += "Sum([Bruttolohn]) As [Bruttolohn],Sum([QSTBasis]) As [QSTBasis], "
			cmdText += "Sum([StdAnz]) As StdAnz,Sum([WorkedDays]) As WorkedDays,[M_Ans],[TarifCode], [Arbeitspensum], [LANR], [LALOText] "
			cmdText += "INTO #tblTemp "
			cmdText += String.Format("FROM {0} T0 ", ClsDataDetail.LLTabellennamen)
			cmdText += "GROUP BY "
			cmdText += "[MANR], [Monat], [Nachname], [Vorname], [VonMonat], [BisMonat], [Jahr], [GebDat], "
			cmdText += "[AHV_Nr_New], [ESStrasse], [ESPLZ], [ESOrt], [ESKanton], [AHV_Nr],[Geschlecht], [Zivilstand],[MAStrasse],[MAPLZ],[MAOrt],[Kinder],[Bewillig],[M_Ans],[TarifCode], [Arbeitspensum], [LANR], [LALOText] "
			cmdText += "ORDER BY Nachname "

			cmdText += "SELECT [MANR], (SELECT QSTGemeinde FROM Mitarbeiter MA WHERE MA.MANR = T0.MANR) As [S_Gemeinde], "
			cmdText += "(SELECT Min([ESAb]) FROM #tblTemp T1 WHERE T1.MANR = T0.MANR) As [ESAb], "
			cmdText += "(SELECT Max([ESEnde]) FROM #tblTemp T1 WHERE T1.MANR = T0.MANR) As [ESEnde], "
			cmdText += "[HeiratDt], [TodesfallDt], [Gratifikation], [GeburtDt],[Nachname],[Vorname], "
			cmdText += "[VonMonat], [BisMonat], [Jahr], [GebDat], [AHV_Nr], [AHV_Nr_New], "
			cmdText &= "[ESStrasse], [ESPLZ], [ESOrt], [ESKanton], "
			cmdText += "[Geschlecht], [Zivilstand],[MAStrasse],[MAPLZ],[MAOrt],[Kinder],[Bewillig], Sum([M_Bas]) As M_Bas, Sum([M_Btr]) As M_Btr, "
			cmdText += "Sum([Bruttolohn]) As [Bruttolohn], Sum([QSTBasis]) As [QSTBasis], Sum([StdAnz]) As StdAnz,Sum([WorkedDays]) As WorkedDays, "
			cmdText += "[M_Ans],[TarifCode], [Arbeitspensum], [LANR], [LALOText] "
			cmdText += "FROM #tblTemp T0 "

			cmdText += "GROUP BY [MANR], [HeiratDt], [TodesfallDt], [Gratifikation], [GeburtDt], "
			cmdText += "[Nachname], [Vorname], [VonMonat], [BisMonat], [Jahr], [GebDat], [AHV_Nr], [AHV_Nr_New], [ESStrasse], [ESPLZ], [ESOrt], [ESKanton], "
			cmdText += "[Geschlecht], [Zivilstand],[MAStrasse],[MAPLZ],[MAOrt],[Kinder],[Bewillig],[M_Ans],[TarifCode], [Arbeitspensum], [LANR], [LALOText] "
			cmdText += "ORDER BY Nachname, Vorname"

			cmd.CommandText = cmdText
			m_Logger.LogDebug("2. starting...")

			Dim declarationContribuableCommonList As New List(Of DeclarationContribuableCommon_Type)()
			Using sqlReader As SqlDataReader = cmd.ExecuteReader()
				While sqlReader.Read()
					maNr = utilities.SafeGetInteger(sqlReader, "MANR", 0).Value
					employeeName = String.Format("{0}, {1}", utilities.SafeGetString(sqlReader, "Nachname"), utilities.SafeGetString(sqlReader, "Vorname"))

					' Hauptelement pro Arbeitnehmer
					Dim declarationContribuableCommon As New DeclarationContribuableCommon_Type()
					declarationContribuableCommon.TypeContribuable = 0 ' Default Wert wird verwendet
					' Personeninfo
					Dim infoContribuable As New InfoPersonne_Type()
					Dim ahvNrNew As String = sqlReader("AHV_Nr_New").ToString().Trim()
					Dim ahvNrNewAvailable As Boolean = Not String.IsNullOrEmpty(ahvNrNew)
					If ahvNrNewAvailable Then
						infoContribuable.nouveauNAVS13 = ahvNrNew
						' TODO: Gültigkeitsprüfung mit Regex an zentraler Stelle (jetzt 13 Zahlen und 3 Punkte)
						If ahvNrNew.Length <> 16 Then
							result.AddWarning("Kandidat {0} - {1}: Die neue AHV-Nummer '{2}' ist ungültig", maNr, employeeName, ahvNrNew)
						End If
					End If
					Dim ahvNrOld As String = utilities.SafeGetString(sqlReader, "AHV_Nr").Trim()
					If ahvNrOld.EndsWith(".000") Then ahvNrOld = String.Empty
					Dim ahvNrOldAvailable As Boolean = Not String.IsNullOrEmpty(ahvNrOld)
					If ahvNrOldAvailable Then
						infoContribuable.numeroAVS = ahvNrOld
						' TODO: Gültigkeitsprüfung mit Regex an zentrale Stelle (jetzt 11 Zahlen und 3 Punkte)
						If ahvNrOld.Length <> 14 Then
							result.AddWarning("Kandidat {0} - {1}: Die alte AHV-Nummer '{1}' ist ungültig", maNr, employeeName, ahvNrOld)
						End If
					End If
					If Not ahvNrNewAvailable AndAlso Not ahvNrOldAvailable Then
						result.AddWarning("Kandidat {0} - {1}: Es ist keine AHV-Nummer eingetragen", maNr, employeeName)
					End If
					m_Logger.LogDebug("3. starting...")

					infoContribuable.nomPersonne = utilities.SafeGetString(sqlReader, "Nachname")
					infoContribuable.prenomPersonne = utilities.SafeGetString(sqlReader, "Vorname")
					infoContribuable.dateNaissance = DateTime.Parse(sqlReader("GebDat")).ToString("dd.MM.yyyy")
					infoContribuable.sexe = Xsd.Ge.QstExportGe.GetSexe(utilities.SafeGetString(sqlReader, "Geschlecht"))
					infoContribuable.sexeSpecified = True
					infoContribuable.etatCivil = Xsd.Ge.QstExportGe.GetEtatCivil(utilities.SafeGetString(sqlReader, "Zivilstand"))
					infoContribuable.etatCivilSpecified = True
					declarationContribuableCommon.InfoContribuable = infoContribuable
					m_Logger.LogDebug("4. starting...")
					' Familieninfo
					' TODO Fardin: Kinderangaben aus der Datenbank holen...
					Dim famille As New FamillePersonne_Type2014()
					Dim childdata = LoadChilderdata(utilities.SafeGetInteger(sqlReader, "MANr", 0))

					If Not childdata Is Nothing Then
						Dim childCount As Integer = childdata.Count - 1	' Integer.Parse(utilities.SafeGetString(sqlReader, "Kinder"))
						If childCount > 0 Then
							Dim children As String() = New String(childCount) {}
							Dim i As Integer = 0
							For Each child In children
								child = childdata(i).childJahrgang	' Hack: jedes Kind hat Jahrgang 2000
								i += 1
							Next

							famille.dateNaissanceEnfantInf25 = children
						End If
					End If
					m_Logger.LogDebug("5. starting...")

					'famille.nbrEnfantsInf25 = utilities.SafeGetString(sqlReader, "Kinder")
					declarationContribuableCommon.Famille = famille
					' Wohnadresse
					Dim adresseDomicile = New AdresseDomicile_Type()
					Dim adresseDomicileSimple = New AdresseSimple_Type()
					adresseDomicileSimple.voie = New Voie_Type() With {.voie = utilities.SafeGetString(sqlReader, "MAStrasse")}
					adresseDomicileSimple.NPA = utilities.SafeGetString(sqlReader, "MAPLZ")
					adresseDomicile.adresse = adresseDomicileSimple
					adresseDomicile.localite = utilities.SafeGetString(sqlReader, "MAOrt")
					Dim communeCode As String = utilities.SafeGetString(sqlReader, "S_Gemeinde").Trim.Substring(0, 4)
					Dim communeGe As EnumCommuneGE_Type? = Xsd.Ge.QstExportGe.GetCommuneGE(communeCode)
					If communeGe.HasValue Then
						adresseDomicile.Item = communeGe.Value
					Else
						result.AddWarning("Kandidat {0} - {1}: Der QST-Gemeindecode '{2}' ist ungültig", maNr, employeeName, communeCode)
					End If
					m_Logger.LogDebug("6. starting...")

					declarationContribuableCommon.AdresseDomicile = adresseDomicile
					' TODO: Bevorzugter Steuersatz kann gewährt werden falls beantragt
					declarationContribuableCommon.Baremepreferentiel = False
					' TODO: Mehrere Anstellungen pro Person
					Dim assujettissementContribuable As New List(Of Assujettissement_Type2014)()
					Dim assujettissementContribuableItem As Assujettissement_Type2014
					assujettissementContribuableItem = New Assujettissement_Type2014()
					Dim periodeImposition As New PeriodeImposition_Type()
					' TODO: eine Exception wird bei Verwendung von SafeGetDateTime() geworfen
					' Dim assessmentDateFrom As Date = utilities.SafeGetDateTime(sqlReader, "ESAB", periodDateFrom).Value
					' Dim assessmentDateTo As Date = utilities.SafeGetDateTime(sqlReader, "ESEnde", periodDateTo).Value
					Dim assessmentDateFrom As Date
					Dim assessmentDateTo As Date
					If Not DateTime.TryParse(sqlReader("ESAB").ToString(), assessmentDateFrom) Then
						assessmentDateFrom = periodDateFrom
					End If
					If Not DateTime.TryParse(sqlReader("ESEnde").ToString(), assessmentDateTo) Then
						assessmentDateTo = periodDateTo
					End If
					m_Logger.LogDebug("7. starting...")

					periodeImposition.debut_imposition = assessmentDateFrom.ToString("dd.MM.yyyy")
					periodeImposition.fin_imposition = assessmentDateTo.ToString("dd.MM.yyyy")
					assujettissementContribuableItem.periodeImposition = periodeImposition

					' TODO: Arbeitsadresse
					Dim adresseTravail As New AdresseTravail_Type()
					Dim adresseTravailSimple = New AdresseSimple_Type()

					Dim esStrasse As String = utilities.SafeGetString(sqlReader, "ESStrasse").Trim
					Dim esPLZ As String = utilities.SafeGetString(sqlReader, "ESPLZ").Trim
					Dim esOrt As String = utilities.SafeGetString(sqlReader, "ESOrt").Trim
					Dim esCanton As String = utilities.SafeGetString(sqlReader, "ESKanton").Trim

					adresseTravailSimple.voie = New Voie_Type() With {.voie = esStrasse}
					adresseTravailSimple.NPA = esPLZ
					adresseTravail.adresse = adresseTravailSimple
					adresseTravail.localite = esOrt
					Dim canton As EnumCanton_Type? = Xsd.Ge.QstExportGe.GetCanton(esCanton)
					' canton GE is Nothing!
					If canton.HasValue Then
						adresseTravail.Item = canton.Value

					Else
						If communeGe.HasValue Then
							adresseTravail.Item = communeGe.Value
						Else
							result.AddWarning("Kunden {0} - {1}: Der QST-Gemeindecode '{2}' ist ungültig", maNr, employeeName, communeCode)
						End If

					End If
					m_Logger.LogDebug("8 starting...")

					assujettissementContribuableItem.adresseTravail = adresseTravail

					assujettissementContribuable.Add(assujettissementContribuableItem)
					declarationContribuableCommon.AssujettissementContribuable = assujettissementContribuable.ToArray()
					' Steuerwerte
					Select Case listType
						Case 1, 2
							Dim retenuePrestationsImpots As New RetenueType1_2_Type2014()
							' Steuersatz
							retenuePrestationsImpots.tauxImposition = Format(utilities.SafeGetDecimal(sqlReader, "M_Ans", 0D).Value, "f2")
							retenuePrestationsImpots.tauxImpositionSpecified = True
							' Brutto-Einkommen
							retenuePrestationsImpots.prestationsSoumisesImpot = Math.Round(utilities.SafeGetDecimal(sqlReader, "Bruttolohn", 0D).Value).ToString()
							Dim retenueSalarie As New RetenueSalaries_Type2014()
							' TODO: Berufskategorie (la catégorie professionnelle est obligatoire pour les salariés, les activités accessoires et les permis 120 jours)
							' retenueSalarie.categorieProfessionnelle = EnumCatProfessionnelle_Type.Item1
							' Anzahl Arbeitstage
							If utilities.SafeGetString(sqlReader, "Bewillig") = "B" Then
								retenueSalarie.nbrJourstravailEffectif = Math.Min(utilities.SafeGetInteger(sqlReader, "WorkedDays", 0).Value, 120)
							Else
								retenueSalarie.nbrJourstravailEffectif = utilities.SafeGetInteger(sqlReader, "WorkedDays", 0).Value
							End If

							' TODO: Kinderzulage
							' retenueSalarie.allocationsFamiliales = "0"
							' TODO: Si frais effectifs et/ou forfaitaires confomes à la Fédération des Entreprises Genevoises (0 : non, 1 : oui, 2 : non renseigné)
							' retenueSalarie.conformeFERGeneve = EnumBooleen3Etats_Type.Item0
							' Arbeitspensum
							' TODO: eine Exception wird bei Verwendung von SafeGetDecimal() geworfen
							Dim tauxActive As Decimal = Val(utilities.SafeGetString(sqlReader, "Arbeitspensum"))
							If tauxActive = 0 Then tauxActive = 100
							retenueSalarie.tauxActivite = tauxActive
							retenueSalarie.tauxActiviteSpecified = True

							retenuePrestationsImpots.retenueSalarie = retenueSalarie
							' Steuerbetrag

							retenuePrestationsImpots.impotsRetenus = Format((utility_Infrastructure.SwissCommercialRound(utilities.SafeGetDecimal(sqlReader, "M_Btr", 0D).Value * (-1))), "f2")
							' TODO: Kirchenbeitrag (Amount of the church contribution if the confession has been indicated. The rate of this contribution was fixed at 1% of gross income)
							' retenuePrestationsImpots.contributionEcclesiastique = 0D
							declarationContribuableCommon.RetenuePrestationsImpots = retenuePrestationsImpots
						Case 3, 4
							Dim retenuePrestationsImpots As New RetenueType3_4_Type2014()
							' TODO: Implementation für Typ 3 und 4
							declarationContribuableCommon.RetenuePrestationsImpots = retenuePrestationsImpots
						Case 5
							' TODO: Implementation für Typ 5
							Dim retenuePrestationsImpots As New RetenueType5_Type2014()
							declarationContribuableCommon.RetenuePrestationsImpots = retenuePrestationsImpots
						Case 6
							' TODO: Implementation für Typ 6
							Dim retenuePrestationsImpots As New RetenueType6_Type2014()
							declarationContribuableCommon.RetenuePrestationsImpots = retenuePrestationsImpots
						Case Else
							' Nicht unterstützt
					End Select
					m_Logger.LogDebug("9. starting...")

					' Postadresse (wird nicht gesetzt)
					' Dim adresseExpedition As New AdresseExpedition_Type()
					' declarationContribuableCommon.AdresseExpedition = adresseExpedition
					' Freitext
					Dim texteLibre As New TexteLibre_Type()
					texteLibre.ligne1 = sqlReader("LANR").ToString() + " " + sqlReader("LALOText").ToString()
					declarationContribuableCommon.TexteLibre = texteLibre
					declarationContribuableCommonList.Add(declarationContribuableCommon)
					m_Logger.LogDebug("10. starting...")

				End While
			End Using
			m_Logger.LogDebug("11. starting...")

			' Allgemeine Liste in typisierte Liste vom Typ 1...6 konvertieren
			qstExportGe.Item = DeclarationContribuableCommon_Type.GetListRecapitulativeArray(declarationContribuableCommonList, listType)

			' XSD Klasse in XML File serialisieren
			Xsd.XmlReaderWriter.Save(Of DeclarationListeRecapitulative_Type2014)(strFullFilename, qstExportGe)
			m_Logger.LogDebug("12. starting...")

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			result.AddError("MANr {0}: Unbehandelter Fehler", maNr)
			result.AddError(ex)
		End Try

		Return result

	End Function


#End Region


#Region "Helpers"

	Function GetTaxCustomerNumber(ByVal canton As String) As String
		Dim strResult As String = String.Empty
		Dim strSqlQuery As String = "Select Top 1 StammNr From MD_QstAddress Where MDNr = @MDNr And Kanton = @Kanton "
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MDNr", ClsDataDetail.m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@Kanton", canton)

			Dim rMyrec As SqlDataReader = cmd.ExecuteReader

			While rMyrec.Read
				strResult = GetColumnTextStr(rMyrec, "StammNr", "")

			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function

	Function LoadChilderdata(ByVal employeeNumber) As IEnumerable(Of ChildData)
		Dim result As List(Of ChildData) = Nothing
		Dim strSqlQuery As String = "Select * From MA_KIAddress Where MANr = @MANr And year(getDate()) - Year(GebDat) <= 24 "
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim utilities As New SPProgUtility.MainUtilities.Utilities()

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MANr", employeeNumber)

			Dim reader As SqlDataReader = cmd.ExecuteReader

			If (Not reader Is Nothing) Then

				result = New List(Of ChildData)

				While reader.Read()
					Dim overviewData As New ChildData

					overviewData.MANr = utilities.SafeGetInteger(reader, "MANr", 0)

					overviewData.Nachname = utilities.SafeGetString(reader, "Nachname")
					overviewData.Vorname = utilities.SafeGetString(reader, "Vorname")
					overviewData.GebDate = utilities.SafeGetDateTime(reader, "GebDat", Nothing)

					result.Add(overviewData)

				End While

				reader.Close()

			End If


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return result
	End Function


#End Region

	Public Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
		Dim bResult As Boolean = False

		' alle geöffneten Forms durchlaufen
		For Each oForm As Form In Application.OpenForms
			If oForm.Name.ToLower = sName.ToLower Then
				If bDisposeForm Then oForm.Dispose() : Exit For
				bResult = True : Exit For
			End If
		Next

		Return (bResult)
	End Function

	'Function ExtraRights(ByVal lModulNr As Integer) As Boolean
	'	Dim bAllowed As Boolean
	'	Dim strModulCode As String

	'	' 10200        ' Fremdrechnung
	'	' 10201        ' Rapportinhalt
	'	' 10202        ' Export nach Abacus
	'	' 10206        ' Export nach Sesam
	'	strModulCode = _ClsReg.GetINIString(_ClsProgSetting.GetInitIniFile, "ExtraModuls", CStr(lModulNr))
	'	If InStr(1, strModulCode, "+" & lModulNr & "+") > 0 Then bAllowed = True

	'	ExtraRights = bAllowed

	'End Function


	Sub MailTo(ByVal An As String, Optional ByVal Betreff As String = "")
		System.Diagnostics.Process.Start(String.Format("mailto:{0}?subject={1}", An, Betreff))
	End Sub

#Region "Übersetzung der Controls..."

	Function TranslateMyText(ByVal strBez As String) As String
		Dim strOrgText As String = strBez
		Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez)
		Dim _clsLog As New SPProgUtility.ClsEventLog

		If _ClsProgSetting.GetLogedUSNr = 1 Then
			_clsLog.WriteTempLogFile(String.Format("Progbez: {0}{1}{0} Translatedbez: {0}{2}{0}", _
																	Chr(34), strBez, strTranslatedText), _
																_ClsProgSetting.GetSpSTempPath & "DeinFile.txt")
		End If

		Return strTranslatedText
	End Function

	Function TranslateMyText(ByVal strFuncName As String, _
													 ByVal strOrgControlBez As String, _
													 ByVal strBez As String) As String
		Dim strOrgText As String = strBez
		Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez)
		Dim _clsLog As New SPProgUtility.ClsEventLog

		If _ClsProgSetting.GetLogedUSNr = 1 Then
			_clsLog.WriteTempLogFile(String.Format("{1}: Progbez: {0}{2}{0} Namedbez: {0}{3}{0}, Translatedbez: {0}{4}{0}", _
																	Chr(34), strFuncName, strOrgControlBez, strBez, strTranslatedText), _
																_ClsProgSetting.GetSpSTempPath & "DeinFile.txt")
		End If

		Return strTranslatedText
	End Function

#End Region

End Module
