
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient
Imports System.IO

Namespace ExportAba

	Public Class sClsExportOPInAba

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private m_xml As New ClsXML
		Private Property _ExportSetting As New ClsCSVSettings



		Public Sub New(ByVal _setting As ClsCSVSettings)
			m_UtilityUI = New UtilityUI
			Me._ExportSetting = _setting
		End Sub

		Function GetOPValue4Abacus() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "Success"

			Dim Conn As SqlConnection = New SqlConnection(Me._ExportSetting.DbConnString2Open)
			Dim i As Integer = 2
			Dim strValue As String = String.Empty
			Dim iLine As Integer = 1
			Dim strSepString As String = Me._ExportSetting.FieldSeprator
			Dim strFieldIn As String = Me._ExportSetting.FieldIn ' Chr(34)
			Dim sSql As String
			Dim strRecValue As String = String.Empty
			Dim strMsg As String
			Dim lValue As Long
			Dim strKonto As String


			If strFieldIn = String.Empty Then strFieldIn = Chr(34)
			If strSepString = String.Empty Then strSepString = ","

			strValue = String.Empty
			sSql = Me._ExportSetting.SQL2Open
			Conn.Open()

			Dim cmd As New SqlCommand(sSql, Conn)
			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			iLine = 1
			While rDbrec.Read
				' von 1 bis 19
				strKonto = rDbrec("FKSoll")
				If Me._ExportSetting.KDRefNrAsFKSoll Then strKonto = IIf(String.IsNullOrWhiteSpace(rDbrec("Kredit_RefNr").ToString), strKonto, rDbrec("Kredit_RefNr"))

				strValue = strValue &
															iLine & strSepString &
															"I" & strSepString &
															Replace(Format(rDbrec("Fak_Dat"), "d"), ".", "/") & strSepString &
															strKonto & strSepString &
															rDbrec("FKHaben0") & strSepString &
															strFieldIn & Mid(rDbrec("R_Name1"), 1, 30) & strFieldIn & strSepString &
															Format(rDbrec("Betragink"), "0.00") & strSepString &
															strFieldIn & "" & strFieldIn & strSepString &
															"S" & strSepString &
															"" & strSepString &
															String.Format("{0}", If(Me._ExportSetting.MitGegenKostenart, 0, String.Empty)) & strSepString &
															strFieldIn & rDbrec("RENr") & strFieldIn & strSepString &
															"0" & strSepString &
															strSepString &
															"0" & strSepString &
															"" & strSepString &
															strSepString &
															strSepString &
															strSepString

				' von 20 bis 40
				strValue = strValue &
															Replace(Format(rDbrec("Fak_Dat"), "d"), ".", "/") & strSepString &
															"" & strSepString &
															strSepString &
															_ClsProgSetting.GetMDNr & strSepString &
															"CHF" & strSepString &
															"CHF" & strSepString &
															"0" & strSepString &
															"0" & strSepString &
															"0" & strSepString &
															"0" & strSepString &
															"" & strSepString &
															"" & strSepString &
															"" & strSepString &
															"" & strSepString &
															"" & strSepString &
															"" & strSepString &
															String.Format("{0}", Me._ExportSetting.MwStCode) & strSepString &
															Format(rDbrec("MwStProz"), "f2") & strSepString &
															"I" & strSepString &
															"1" & strSepString &
															"" & strSepString

				' von 41 bis 50
				Dim strQuery As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetMDData_XMLFile, "//BuchungsKonten/_6", "")
				strValue = strValue &
															"100" & strSepString &
															strQuery & strSepString &
															rDbrec("FKHaben0") & strSepString &
															"1" & strSepString &
															(Val(rDbrec("MwSt1")) * -1) & strSepString &
															"0" & strSepString &
															"0" & strSepString &
															"0" & strSepString &
															"" & strSepString &
															"1" & strSepString

				' von 51 bis 63
				strValue = strValue &
															"" & strSepString &
															"" & strSepString &
															"" & strSepString &
															"0" & strSepString &
															"0" & strSepString &
															"0" & strSepString &
															"0" & strSepString &
															"" & strSepString &
															"" & strSepString &
															"" & strSepString &
															"" & strSepString &
															"" & strSepString &
															"E"

				If strValue <> "" Then
					strRecValue = strRecValue & IIf(strRecValue = "", strValue, Chr(13) & Chr(10) & strValue)
					strValue = String.Empty

					lValue = lValue + 1
					iLine = iLine + 1
				End If

			End While

			If Not String.IsNullOrWhiteSpace(strRecValue) Then
				strResult = Me.WriteContent2File(strRecValue, Me._ExportSetting.ExportFileName)

				If strResult.ToLower.Contains("error") Then
					strMsg = "Fehler bei Erstellung der Datei.{0}{1}"
					strMsg = String.Format(m_xml.GetSafeTranslationValue(strMsg), vbNewLine, strResult)

					m_UtilityUI.ShowErrorDialog(strMsg, m_xml.GetSafeTranslationValue("Dateiexport"), System.Windows.Forms.MessageBoxIcon.Asterisk)
					Return strResult

				Else
					strMsg = "Die Datei wurde erfolgreich erstellt.{0}{1}"
					strMsg = String.Format(m_xml.GetSafeTranslationValue(strMsg), vbNewLine, strResult)

					m_UtilityUI.ShowInfoDialog(strMsg, m_xml.GetSafeTranslationValue("Dateiexport"))

					Dim strDirectory As String = Path.GetDirectoryName(strResult)
					System.Diagnostics.Process.Start(strDirectory)
				End If
			End If

			Return strResult
		End Function

		Private Function WriteContent2File(ByVal msg As String, ByVal strFullFilename As String) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strPathForFiles As String = String.Empty
			Dim strResult As String = "Success"

			Try
				If strFullFilename = String.Empty Then
					strPathForFiles = String.Format("{0}Aba_OP.txt", _ClsProgSetting.GetSpSTempPath)

				Else
					Try
						Kill(strFullFilename)

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.Datei löschen: {1}", strMethodeName, ex.Message))

					End Try

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Dateiname festlegen: {1}", strMethodeName, ex.Message))
				Return String.Format("Error: {0}.Dateiname festlegen: {1}", strMethodeName, ex.Message)

			End Try

			Try
				Dim MyFile As FileInfo = New FileInfo(strFullFilename)
				If Not MyFile.Directory.Exists Then MyFile.Directory.Create()

				'check the file
				Dim fs As FileStream = New FileStream(strFullFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite)
				Dim s As StreamWriter = New StreamWriter(fs)
				s.Close()
				fs.Close()

				'log it
				Dim fs1 As FileStream = New FileStream(strFullFilename, FileMode.Append, FileAccess.Write)
				Dim s1 As StreamWriter = New StreamWriter(fs1)

				s1.Write(msg)
				s1.Close()
				fs1.Close()

				strResult = Me._ExportSetting.ExportFileName

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				Return String.Format("Error: {0}.{1}", strMethodeName, ex.Message)

			End Try

			Return strResult
		End Function

	End Class


	Public Class sClsExportZEInAba

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private m_xml As New ClsXML

		Private Property _ExportSetting As New ClsCSVSettings
		Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI


#Region "Constructor"

		Public Sub New(ByVal _setting As ClsCSVSettings, ByVal _init As SP.Infrastructure.Initialization.InitializeClass)

			Me._ExportSetting = _setting
			m_InitializationData = _init
			m_UtilityUi = New UtilityUI

		End Sub

#End Region

		Function GetZEValue4Abacus() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "Success"

			Dim Conn As SqlConnection = New SqlConnection(Me._ExportSetting.DbConnString2Open)
			Dim i As Integer = 2
			Dim strValue As String = String.Empty
			Dim iLine As Integer = 1
			Dim strSepString As String = Me._ExportSetting.FieldSeprator
			Dim strFieldIn As String = Me._ExportSetting.FieldIn ' Chr(34)
			Dim sSql As String
			Dim strRecValue As String = String.Empty
			Dim strMsg As String
			Dim lValue As Long
			Dim strKonto As String

			'Dim strSHeader As String = "Zähler{0} Version{0} Datum{0} Konto/Kart{0} Gkto/Gkart{0} " & _
			'                              "Buchtungstext1{0} Betrag{0} Buchungstext2{0} Soll/Haben{0} " & _
			'                              "Koststelle{0} Gegenkostenstelle{0} Belegnummer{0} Kurs{0} Kurtart{0} " & _
			'                              "Betrag FW{0} Sammelbuchungs-ID{0} Spec2{0} Spec1{0} Reserve{0} Valutadatum{0} " & _
			'                              "Sammelbuchungspos.{0} Reserve{0} Mandanten-Nummer{0} ISO-Code Konto{0} " & _
			'                              "ISO-Code GKonto{0} Menge{0} Ansatz{0} KTR 2. Ebene{0} Fonds 1{0} Fonds2{0} " & _
			'                              "Reserve{0} Reserve{0} Reserve{0} Codefeld{0} MWSTCODE{0} " & _
			'                              "MWSTSATZ{0} MWSTINCL{0} MWSTMETH{0} MWSTLAND{0} MWSTKOEFF{0} MWSTKTO{0} " & _
			'                              "MWSTKTO2{0} MWSTSH{0} MWSTBETR{0} MWSTFWBETR{0} MWSTRESTBetr{0} " & _
			'                              "MWSTRest FW-Betr.{0} Reserve{0} MWST-Typ{0} Reserve{0} Reserve{0} Reserve{0} " & _
			'                              "Geschäftsbereich{0} Ist-Obder Planzahlen{0} Haben Verdichtung bei Sammelbuchung{0} " & _
			'                              "Haben Verdichtung bei Sammelbuchung in FW{0} EURO Koeff. 1{0} " & _
			'                              "EURO Koeff. 2{0} IC{0} Kurs2{0} Konscode{0} konstante"

			'strSHeader = String.Format(strSHeader, strSepString)
			If strFieldIn = String.Empty Then strFieldIn = Chr(34)
			If strSepString = String.Empty Then strSepString = ","

			'' Zur Hilfe die Header aufbauen ..............................................................................
			'For i = 1 To 63
			'  strValue = strValue & IIf(strValue = "", strFieldIn, strSepString & strFieldIn) & _
			'                        i & strFieldIn
			'Next i
			'strValue = IIf(strRecValue = "", strValue, Chr(13) & Chr(10) & strSHeader)
			'strValue = strValue & Chr(13) & Chr(10)
			'' Ende der Header ...........................................................................................

			strValue = String.Empty
			sSql = String.Format("Select * From [_ZEListe_{0}]", _ClsProgSetting.GetLogedUSGuid)
			'Trim(Mid(strTempSql, InStr(1, strTempSql, String.Format("Select * From [_DebitorenListe_{0}]" , _ClsProgSetting.GetLogedUSGuid)))
			Conn.Open()

			Dim cmd As New SqlCommand(sSql, Conn)
			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			iLine = 1
			'Dim _Clskonten As New FibuKonten
			Dim _ClsKonten As New ExportSage.ClsKonten(m_InitializationData)
			Dim liKonen As List(Of Integer) = _ClsKonten.GetKontoNr()
			Dim bAsVerlust As Boolean = False
			Dim amountExclusiveVAT As Decimal = 0
			Dim VATFromExclusiveAmount As Decimal = 0

			While rDbrec.Read
				' von 1 bis 19
				strKonto = rDbrec("FKSoll")
				If Me._ExportSetting.KDRefNrAsFKSoll Then strKonto = IIf(String.IsNullOrWhiteSpace(rDbrec("Kredit_RefNr").ToString), strKonto, rDbrec("Kredit_RefNr"))
				bAsVerlust = liKonen.Contains(rDbrec("FKSoll"))

				strValue = strValue & _
															iLine & strSepString & _
															"I" & strSepString & _
															Replace(Format(rDbrec("V_Date"), "d"), ".", "/") & strSepString & _
															strKonto & strSepString & _
															rDbrec("FKHaben") & strSepString & _
															strFieldIn & Mid(rDbrec("R_Name1"), 1, 30) & strFieldIn & strSepString & _
															Format(rDbrec("Betrag"), "0.00") & strSepString & _
															strFieldIn & "" & strFieldIn & strSepString & _
															"S" & strSepString & _
															"" & strSepString & _
															String.Format("{0}", If(Me._ExportSetting.MitGegenKostenart, 0, String.Empty)) & strSepString & _
															strFieldIn & rDbrec("ZENr") & strFieldIn & strSepString & _
															"0" & strSepString & _
															strSepString & _
															"0" & strSepString & _
															"" & strSepString & _
															strSepString & _
															strSepString & _
															strSepString

				' von 20 bis 40
				strValue = strValue & _
															Replace(Format(rDbrec("V_Date"), "d"), ".", "/") & strSepString & _
															iLine & strSepString & _
															strSepString & _
															_ClsProgSetting.GetMDNr & strSepString & _
															"CHF" & strSepString & _
															"CHF" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															String.Format("{0}", Me._ExportSetting.MwStCode) & strSepString & _
															"" & strSepString & _
															If(Not bAsVerlust, "", "I") & strSepString & _
															"1" & strSepString & _
															"" & strSepString
				'String.Format("{0}", If(Not bAsVerlust, "", Me._ExportSetting.MwStCode)) & strSepString & _
				'Format(If(Not bAsVerlust, 0, rDbrec("MwStProz")), "f2") & strSepString & _

				' von 41 bis 50
				'Dim strQuery As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetMDData_XMLFile, "//BuchungsKonten/_6", "")
				amountExclusiveVAT = Val(rDbrec("Betrag")) / If(rDbrec("MWSTProz") = 0, 1, (rDbrec("MWSTProz") / 100)) ' (rDbrec("MWSTProz") / 100)
				VATFromExclusiveAmount = amountExclusiveVAT * (If(Not bAsVerlust, 0, rDbrec("MWSTProz")) / 100)

				strValue = strValue & _
															"0" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"1" & strSepString & _
															Format(VATFromExclusiveAmount * -1, "f2") & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"" & strSepString & _
															"1" & strSepString

				'strValue = strValue & _
				'              "0" & strSepString & _
				'              "0" & strSepString & _
				'              "0" & strSepString & _
				'              "1" & strSepString & _
				'              Format((Val(rDbrec("Betrag") * If(Not bAsVerlust, 0, (rDbrec("MWSTProz") / 100))) * -1), "f2") & strSepString & _
				'              "0" & strSepString & _
				'              "0" & strSepString & _
				'              "0" & strSepString & _
				'              "" & strSepString & _
				'              "1" & strSepString

				' von 51 bis 63
				strValue = strValue & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"E"

				If strValue <> "" Then
					strRecValue = strRecValue & IIf(strRecValue = "", strValue, Chr(13) & Chr(10) & strValue)
					strValue = String.Empty

					lValue = lValue + 1
					iLine = iLine + 1
				End If

			End While

			If Not String.IsNullOrWhiteSpace(strRecValue) Then
				strResult = Me.WriteContent2File(strRecValue, Me._ExportSetting.ExportFileName)

				If strResult.ToLower.Contains("error") Then
					strMsg = "Fehler bei Erstellung der Datei.{0}{1}"
					strMsg = String.Format(m_xml.GetSafeTranslationValue(strMsg), vbNewLine, strResult)

					m_UtilityUi.ShowErrorDialog(strMsg)
					Return strResult

				Else
					strMsg = "Die Datei wurde erfolgreich erstellt.{0}{1}"
					strMsg = String.Format(m_xml.GetSafeTranslationValue(strMsg), vbNewLine, strResult)

					m_UtilityUi.ShowInfoDialog(strMsg)

					Dim strDirectory As String = Path.GetDirectoryName(strResult)
					System.Diagnostics.Process.Start(strDirectory)
				End If
			End If

			Return strResult
		End Function

		Private Function WriteContent2File(ByVal msg As String, ByVal strFullFilename As String) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strPathForFiles As String = String.Empty
			Dim strResult As String = "Success"

			Try
				If strFullFilename = String.Empty Then
					strPathForFiles = String.Format("{0}Aba_OP.txt", _ClsProgSetting.GetSpSTempPath)

				Else
					Try
						Kill(strFullFilename)

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.Datei löschen: {1}", strMethodeName, ex.Message))

					End Try

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Dateiname festlegen: {1}", strMethodeName, ex.Message))
				Return String.Format("Error: {0}.Dateiname festlegen: {1}", strMethodeName, ex.Message)

			End Try

			Try
				Dim MyFile As FileInfo = New FileInfo(strFullFilename)
				If Not MyFile.Directory.Exists Then MyFile.Directory.Create()

				'check the file
				Dim fs As FileStream = New FileStream(strFullFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite)
				Dim s As StreamWriter = New StreamWriter(fs)
				s.Close()
				fs.Close()

				'log it
				Dim fs1 As FileStream = New FileStream(strFullFilename, FileMode.Append, FileAccess.Write)
				Dim s1 As StreamWriter = New StreamWriter(fs1)

				s1.Write(msg)
				s1.Close()
				fs1.Close()

				strResult = Me._ExportSetting.ExportFileName

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				Return String.Format("Error: {0}.{1}", strMethodeName, ex.Message)

			End Try

			Return strResult
		End Function

	End Class


	Public Class ClsExportLOInAba

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private m_xml As New ClsXML

		Private Property _ExportSetting As New ClsCSVSettings
		Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI


		Public Sub New(ByVal _setting As ClsCSVSettings, ByVal _init As SP.Infrastructure.Initialization.InitializeClass)
			Me._ExportSetting = _setting
			m_InitializationData = _init
			m_UtilityUi = New UtilityUI
		End Sub

		Private Function GetLastDayOfMonth(ByVal dtDate As DateTime) As DateTime
			Dim dtTo As New DateTime(dtDate.Year, dtDate.Month, 1)

			dtTo = dtTo.AddMonths(1)
			dtTo = dtTo.AddDays(-(dtTo.Day))

			Return dtTo
		End Function

		Function GetLOValue4Abacus() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "Success"

			Dim Conn As SqlConnection = New SqlConnection(Me._ExportSetting.DbConnString2Open)
			Dim i As Integer = 2
			Dim strValue As String = String.Empty
			Dim iLine As Integer = 1
			Dim strSepString As String = ","
			Dim strFieldIn As String = Chr(34)
			Dim strTableName As String = String.Format("[_LOLFibu_{0}]", _ClsProgSetting.GetLogedUSGuid)
			Dim sSql As String = "Select LANr, Bezeichnung, SKonto, HKonto, "

			If _ExportSetting.SQL2Open.ToLower.Contains(String.Format("[LOLFibu_{0}]", _ClsProgSetting.GetLogedUSGuid).ToLower) Then
				strTableName = String.Format("[LOLFibu_{0}]", _ClsProgSetting.GetLogedUSGuid)
				sSql &= "SUM(TotalBetrag) As TotalBetrag, "

			Else
				sSql &= "SUM(Betrag_2) As TotalBetrag, "

			End If

			sSql &= "Vorzeichen_2, Vorzeichen_3 "
			sSql &= String.Format("From {0} ", strTableName)
			sSql &= "Group by LANr, Bezeichnung, SKonto, HKonto, Vorzeichen_2, Vorzeichen_3 "
			sSql = sSql & "Order By LANr"

			Conn.Open()

			Dim cmd As New SqlCommand(sSql, Conn)
			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			iLine = 1
			While rDbrec.Read()
				' von 1 bis 19
				If strValue <> String.Empty Then strValue &= vbNewLine
				Dim strVorzeichen_2 As String = rDbrec("Vorzeichen_2").ToString
				Dim strVorzeichen_3 As String = rDbrec("Vorzeichen_3").ToString
				Dim dBetrag As Double = CDbl(rDbrec("Totalbetrag"))
				dBetrag = GetLAValueWithVorZeichen(Val(rDbrec("TotalBetrag").ToString), _
																						 strVorzeichen_2, _
																						 strVorzeichen_3)

				strValue &= iLine & strSepString & _
														 "I" & strSepString & _
														 Format(GetLastDayOfMonth(CDate(String.Format("01.{0}.{1}", Me._ExportSetting.SelectedMonth, Me._ExportSetting.SelectedYear))), "d").Replace(".", "/") & strSepString & _
														 rDbrec("SKonto").ToString & strSepString & _
														 rDbrec("HKonto").ToString & strSepString & _
														 strFieldIn & Mid(rDbrec("Bezeichnung").ToString, 1, 30) & strFieldIn & strSepString & _
														 Format(dBetrag, "f") & strSepString & strSepString & _
														 "S" & strSepString & _
														 "0" & strSepString & _
														 "0" & strSepString & _
														 strFieldIn & "100" & strFieldIn & strSepString & _
														 "0" & strSepString & _
														 strSepString & _
														 strSepString & _
														 strSepString & _
														 strSepString & _
														 strSepString & _
														 strSepString

				' von 20 bis 40
				strValue &= Format(Now, "d").Replace(".", "/") & strSepString & _
															strSepString & _
															strSepString & _
															_ClsProgSetting.GetMDNr & strSepString & _
															"CHF" & strSepString & _
															"CHF" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															strFieldIn & "" & strFieldIn & strSepString & _
															"0" & strSepString & _
															"" & strSepString & _
															"0" & strSepString & _
															"" & strSepString

				' von 41 bis 50
				strValue &= "0" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"0" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"0" & strSepString & _
															"" & strSepString

				' von 51 bis 63
				strValue &= "" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"" & strSepString & _
															"E"

				iLine += 1
			End While

			If Not String.IsNullOrWhiteSpace(strValue) Then
				strResult = Me.WriteContent2File(strValue, Me._ExportSetting.ExportFileName)
				Dim strMsg As String = String.Empty

				If strResult.ToLower.Contains("error") Then
					strMsg = "Fehler bei Erstellung der Datei.{0}{1}"
					strMsg = String.Format(m_xml.GetSafeTranslationValue(strMsg), vbNewLine, strResult)

					m_UtilityUi.ShowErrorDialog(strMsg)
					Return strResult

				Else
					strMsg = "Die Datei wurde erfolgreich erstellt.{0}{1}"
					strMsg = String.Format(m_xml.GetSafeTranslationValue(strMsg), vbNewLine, strResult)

					m_UtilityUi.showinfodialog(strMsg)

					Dim strDirectory As String = Path.GetDirectoryName(strResult)
					System.Diagnostics.Process.Start(strDirectory)
				End If
			End If

			Return strResult
		End Function

		Function GetLAValueWithVorZeichen(ByVal myValue As Double, _
																ByVal strVorzeichen_2 As String, _
																ByVal strVorzeichen_3 As String) As Double
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dMyValue As Double = myValue

			' Den Betrag gemäss Vorzeichen_2, Vorzeichen_3 anpassen
			If dMyValue > 0 Then
				' Vorzeichen_2
				If strVorzeichen_2 = "-" Then
					dMyValue *= -1
				End If

			ElseIf dMyValue < 0 Then
				' Vorzeichen_3
				If strVorzeichen_3 <> "-" Then
					dMyValue *= -1
				End If

			End If

			Return dMyValue
		End Function

		Private Function WriteContent2File(ByVal msg As String, ByVal strFullFilename As String) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strPathForFiles As String = String.Empty
			Dim strResult As String = "Success"

			Try
				If strFullFilename = String.Empty Then
					strPathForFiles = String.Format("{0}Aba_LO.txt", _ClsProgSetting.GetSpSTempPath)

				Else
					Try
						Kill(strFullFilename)

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.Datei löschen: {1}", strMethodeName, ex.Message))

					End Try

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Dateiname festlegen: {1}", strMethodeName, ex.Message))
				Return String.Format("Error: {0}.Dateiname festlegen: {1}", strMethodeName, ex.Message)

			End Try

			Try
				Dim MyFile As FileInfo = New FileInfo(strFullFilename)
				If Not MyFile.Directory.Exists Then MyFile.Directory.Create()

				'check the file
				Dim fs As FileStream = New FileStream(strFullFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite)
				Dim s As StreamWriter = New StreamWriter(fs)
				s.Close()
				fs.Close()

				'log it
				Dim fs1 As FileStream = New FileStream(strFullFilename, FileMode.Append, FileAccess.Write)
				Dim s1 As StreamWriter = New StreamWriter(fs1)

				s1.Write(msg)
				s1.Close()
				fs1.Close()

				strResult = Me._ExportSetting.ExportFileName

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
				Return String.Format("Error: {0}.{1}", strMethodeName, ex.Message)

			End Try

			Return strResult
		End Function


	End Class

End Namespace
