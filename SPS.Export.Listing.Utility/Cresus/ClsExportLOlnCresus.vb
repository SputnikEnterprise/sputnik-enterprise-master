
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient
Imports System.IO

Namespace ExportCresus

	'Public Class FibuKonten

	'	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	'	Private m_xml As New ClsXML


	'	Function GetKontoNr() As List(Of Integer)
	'		Dim strResult As New List(Of Integer)

	'		Dim _ClsReg As New SPProgUtility.ClsDivReg
	'		Dim strMDProgFile As String = _ClsProgSetting.GetMDIniFile()

	'		Dim iSKAOPMwSt As Integer
	'		Dim iSKAOP As Integer
	'		Dim iSKIOPMwSt As Integer
	'		Dim iSKIOP As Integer
	'		Dim iSKFOPMwSt As Integer
	'		Dim iSKFOP As Integer

	'		'Dim iErAOPMwSt As Integer
	'		'Dim iErAOP As Integer
	'		'Dim iErIOPMwSt As Integer
	'		'Dim iErIOP As Integer
	'		'Dim iErFOPMwSt As Integer
	'		'Dim iErFOP As Integer

	'		Dim iVerAOPMwSt As Integer
	'		Dim iVerAOP As Integer
	'		Dim iVerIOPMwSt As Integer
	'		Dim iVerIOP As Integer
	'		Dim iVerFOPMwSt As Integer
	'		Dim iVerFOP As Integer

	'		Dim iRuAOPMwSt As Integer
	'		Dim iRuAOP As Integer
	'		Dim iRuIOPMwSt As Integer
	'		Dim iRuIOP As Integer
	'		Dim iRuFOPMwSt As Integer
	'		Dim iRuFOP As Integer

	'		Dim iGuAOPMwSt As Integer
	'		Dim iGuAOP As Integer
	'		Dim iGuIOPMwSt As Integer
	'		Dim iGuIOP As Integer
	'		Dim iGuFOPMwSt As Integer
	'		Dim iGuFOP As Integer

	'		Dim strQuery As String = String.Empty
	'		Dim strBez As String = String.Empty

	'		Dim iADebitorMwStOp As Integer = 0
	'		Dim iIDebitorMwStOp As Integer = 0
	'		Dim iFDebitorMwStOp As Integer = 0
	'		Dim iMwStOp As Integer = 0

	'		Try

	'			' SKonto A Rechnungen -------------------------------------------------------------------------
	'			strQuery = "//BuchungsKonten/_10"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iSKAOPMwSt = CInt(Val(strBez))
	'			Else
	'				iSKAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "10")))
	'			End If
	'			strResult.Add(iSKAOPMwSt)

	'			' SKonto I Rechnungen
	'			strQuery = "//BuchungsKonten/_11"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iSKIOPMwSt = CInt(Val(strBez))
	'			Else
	'				iSKIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "11")))
	'			End If
	'			strResult.Add(iSKIOPMwSt)

	'			' SKonto F Rechnungen
	'			strQuery = "//BuchungsKonten/_19"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iSKFOPMwSt = CInt(Val(strBez))
	'			Else
	'				iSKFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "19")))
	'			End If
	'			strResult.Add(iSKFOPMwSt)

	'			' SKonto A Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_12"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iSKAOP = CInt(Val(strBez))
	'			Else
	'				iSKAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "12")))
	'			End If
	'			strResult.Add(iSKAOP)


	'			' SKonto I Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_13"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iSKIOP = CInt(Val(strBez))
	'			Else
	'				iSKIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "13")))
	'			End If
	'			strResult.Add(iSKIOP)


	'			' SKonto F Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_20"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iSKFOP = CInt(Val(strBez))
	'			Else
	'				iSKFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "20")))
	'			End If
	'			strResult.Add(iSKFOP)

	'			' ---------------------------------------------------------------------------------------------

	'			' Verlust A Rechnungen MwSt.
	'			strQuery = "//BuchungsKonten/_21"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iVerAOPMwSt = CInt(Val(strBez))
	'			Else
	'				iVerAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "21")))
	'			End If
	'			strResult.Add(iVerAOPMwSt)

	'			' Verlust I Rechnungen MwSt.
	'			strQuery = "//BuchungsKonten/_25"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iVerIOPMwSt = CInt(Val(strBez))
	'			Else
	'				iVerIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "25")))
	'			End If
	'			strResult.Add(iVerIOPMwSt)

	'			' Verlust F Rechnungen MwSt.
	'			strQuery = "//BuchungsKonten/_29"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iVerFOPMwSt = CInt(Val(strBez))
	'			Else
	'				iVerFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "29")))
	'			End If
	'			strResult.Add(iVerFOPMwSt)

	'			' Verlust A Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_22"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iVerAOP = CInt(Val(strBez))
	'			Else
	'				iVerAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "22")))
	'			End If
	'			strResult.Add(iVerAOP)

	'			' Verlust I Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_26"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iVerIOP = CInt(Val(strBez))
	'			Else
	'				iVerIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "26")))
	'			End If
	'			strResult.Add(iVerIOP)

	'			' Verlust F Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_30"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iVerFOP = CInt(Val(strBez))
	'			Else
	'				iVerFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "30")))
	'			End If
	'			strResult.Add(iVerFOP)


	'			' Vergütungen ----------------------------------------------------------------------------------------------
	'			' Vergütungen A Rechnungen MwSt.
	'			strQuery = "//BuchungsKonten/_23"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iRuAOPMwSt = CInt(Val(strBez))
	'			Else
	'				iRuAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "23")))
	'			End If
	'			strResult.Add(iRuAOPMwSt)

	'			' Vergütungen I Rechnungen MwSt.
	'			strQuery = "//BuchungsKonten/_27"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iRuIOPMwSt = CInt(Val(strBez))
	'			Else
	'				iRuIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "27")))
	'			End If
	'			strResult.Add(iRuIOPMwSt)

	'			' Vergütungen F Rechnungen MwSt.
	'			strQuery = "//BuchungsKonten/_31"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iRuFOPMwSt = CInt(Val(strBez))
	'			Else
	'				iRuFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "31")))
	'			End If
	'			strResult.Add(iRuFOPMwSt)

	'			' Vergütungen A Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_24"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iRuAOP = CInt(Val(strBez))
	'			Else
	'				iRuAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "24")))
	'			End If
	'			strResult.Add(iRuAOP)

	'			' Vergütungen I Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_28"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iRuIOP = CInt(Val(strBez))
	'			Else
	'				iRuIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "28")))
	'			End If
	'			strResult.Add(iRuIOP)

	'			' Vergütungen F Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_32"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iRuFOP = CInt(Val(strBez))
	'			Else
	'				iRuFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "32")))
	'			End If
	'			strResult.Add(iRuFOP)


	'			' Gutschrift ----------------------------------------------------------------------------------------------
	'			' Gutschrift A Rechnungen MwSt.
	'			strQuery = "//BuchungsKonten/_33"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iGuAOPMwSt = CInt(Val(strBez))
	'			Else
	'				iGuAOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "33")))
	'			End If
	'			strResult.Add(iGuAOPMwSt)

	'			' Gutschrift I Rechnungen MwSt.
	'			strQuery = "//BuchungsKonten/_35"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iGuIOPMwSt = CInt(Val(strBez))
	'			Else
	'				iGuIOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "35")))
	'			End If
	'			strResult.Add(iGuIOPMwSt)

	'			' Gutschrift F Rechnungen MwSt.
	'			strQuery = "//BuchungsKonten/_37"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iGuFOPMwSt = CInt(Val(strBez))
	'			Else
	'				iGuFOPMwSt = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "37")))
	'			End If
	'			strResult.Add(iGuFOPMwSt)

	'			' Gutschrift A Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_34"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iGuAOP = CInt(Val(strBez))
	'			Else
	'				iGuAOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "34")))
	'			End If
	'			strResult.Add(iGuAOP)

	'			' Gutschrift I Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_36"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iGuIOP = CInt(Val(strBez))
	'			Else
	'				iGuIOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "36")))
	'			End If
	'			strResult.Add(iGuIOP)

	'			' Gutschrift F Rechnungen MwSt.-frei
	'			strQuery = "//BuchungsKonten/_38"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iGuFOP = CInt(Val(strBez))
	'			Else
	'				iGuFOP = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "38")))
	'			End If
	'			strResult.Add(iGuFOP)

	'			' ---------------------------------------------------------------------------------------------
	'			' Debitorenkonten für automatische Verbuchung ...
	'			' Automatische Debitoren
	'			strQuery = "//BuchungsKonten/_1"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iADebitorMwStOp = CInt(Val(strBez))
	'			Else
	'				iADebitorMwStOp = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "1")))
	'			End If
	'			strResult.Add(CInt(Val(iADebitorMwStOp)))

	'			' Individuelle Debitoren
	'			strQuery = "//BuchungsKonten/_15"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iIDebitorMwStOp = CInt(Val(strBez))
	'			Else
	'				iIDebitorMwStOp = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "15")))
	'			End If
	'			strResult.Add(CInt(Val(iIDebitorMwStOp)))

	'			' Festanstellungen Debitoren
	'			strQuery = "//BuchungsKonten/_16"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iIDebitorMwStOp = CInt(Val(strBez))
	'			Else
	'				iFDebitorMwStOp = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "16")))
	'			End If
	'			strResult.Add(CInt(Val(iFDebitorMwStOp)))

	'			' MwSt.
	'			strQuery = "//BuchungsKonten/_6"
	'			strBez = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetMDData_XMLFile(), strQuery)
	'			If strBez <> String.Empty Then
	'				iMwStOp = CInt(Val(strBez))
	'			Else
	'				iMwStOp = CInt(Val(_ClsReg.GetINIString(strMDProgFile, "BuchungsKonten", "6")))
	'			End If
	'			strResult.Add(CInt(Val(iMwStOp)))


	'			' ---------------------------------------------------------------------------------------------

	'		Catch ex As Exception
	'			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetKontoNr_0")

	'		End Try

	'		Return strResult
	'	End Function

	'End Class


	Public Class ClsExportOPInCresus

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private m_xml As New ClsXML
		Private Property _ExportSetting As New ClsCSVSettings



		Public Sub New(ByVal _setting As ClsCSVSettings)
			Me._ExportSetting = _setting
		End Sub

		Function GetOPValue4Cresus() As String
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
			If strSepString = String.Empty Then strSepString = Chr(9)

			strValue = String.Empty
			sSql = Me._ExportSetting.SQL2Open
			Conn.Open()

			Dim cmd As New SqlCommand(sSql, Conn)
			Dim rDbrec As SqlDataReader = cmd.ExecuteReader

			iLine = 1
			While rDbrec.Read
				' Zeile 1 Debitor
				strKonto = rDbrec("FKSoll")
				If Me._ExportSetting.KDRefNrAsFKSoll Then strKonto = IIf(String.IsNullOrWhiteSpace(rDbrec("Kredit_RefNr").ToString), strKonto, rDbrec("Kredit_RefNr"))

				strValue = strValue &
									Replace(Format(rDbrec("Fak_Dat"), "d"), ".", "/") & strSepString &
									"..." & strSepString &
									rDbrec("FKHaben0") & strSepString &
									strSepString &
									Mid(rDbrec("R_Name1"), 1, 30) & strSepString &
									Format(rDbrec("BetragEx"), "0.00") & strSepString &
									0 & strSepString &
									0 & strSepString &
									rDbrec("RENr") & strSepString &
									0 & strSepString &
									strSepString &
									rDbrec("RENr") & strSepString &
									"TVA" & strSepString &
									Mid(rDbrec("R_Name1"), 1, 30) & strSepString &
									strSepString &
									strSepString &
									vbNewLine

				' Zeile 2 MWSt
				Dim strQuery As String = _ClsProgSetting.GetXMLValueByQuery(_ClsProgSetting.GetMDData_XMLFile, "//BuchungsKonten/_6", "")
				strValue = strValue &
									Replace(Format(rDbrec("Fak_Dat"), "d"), ".", "/") & strSepString &
									"..." & strSepString &
									strQuery & strSepString &
									strSepString &
									Mid(rDbrec("R_Name1"), 1, 30) & String.Format(" (MWSt {0})", Format(rDbrec("MwStProz"), "f2")) & strSepString &
									Val(rDbrec("MwSt1")) & strSepString &
									0 & strSepString &
									0 & strSepString &
									rDbrec("RENr") & strSepString &
									0 & strSepString &
									strSepString &
									rDbrec("RENr") & strSepString &
									"TVA" & strSepString &
									Mid(rDbrec("R_Name1"), 1, 30) & strSepString &
									strSepString &
									strSepString &
									vbNewLine

				' Zeile 3 Sammelbuchung
				strValue = strValue &
									Replace(Format(rDbrec("Fak_Dat"), "d"), ".", "/") & strSepString &
									strKonto & strSepString &
									"..." & strSepString &
									strSepString &
									Mid(rDbrec("R_Name1"), 1, 30) & strSepString &
									Format(rDbrec("BetragInk"), "0.00") & strSepString &
									0 & strSepString &
									0 & strSepString &
									rDbrec("RENr") & strSepString &
									0 & strSepString &
									strSepString &
									rDbrec("RENr") & strSepString &
									"TVA" & strSepString &
									Mid(rDbrec("R_Name1"), 1, 30) & strSepString &
									strSepString &
									strSepString


				' String.Format("{0}", If(Me._ExportSetting.MitGegenKostenart, 0, String.Empty)) & strSepString &
				' String.Format("{0}", Me._ExportSetting.MwStCode) & strSepString &
				' Format(rDbrec("MwStProz"), "f2") & strSepString

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

					DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Dateiexport"),
																										 System.Windows.Forms.MessageBoxButtons.OK,
																										 System.Windows.Forms.MessageBoxIcon.Asterisk)
					Return strResult

				Else
					strMsg = "Die Datei wurde erfolgreich erstellt.{0}{1}"
					strMsg = String.Format(m_xml.GetSafeTranslationValue(strMsg), vbNewLine, strResult)

					DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, m_xml.GetSafeTranslationValue("Dateiexport"),
																										 System.Windows.Forms.MessageBoxButtons.OK,
																										 System.Windows.Forms.MessageBoxIcon.Information)

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


	Public Class ClsExportZEInCresus

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

		Function GetZEValue4Cresus() As String
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
			If strSepString = String.Empty Then strSepString = Chr(9)

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

				strValue = strValue &
									Replace(Format(rDbrec("V_Date"), "d"), ".", "/") & strSepString &
									strKonto & strSepString &
									rDbrec("FKHaben") & strSepString &
									strSepString &
									Mid(rDbrec("R_Name1"), 1, 30) & strSepString &
									Format(rDbrec("Betrag"), "0.00") & strSepString &
									0 & strSepString &
									0 & strSepString &
									0 & strSepString &
									0 & strSepString &
									strSepString &
									strSepString &
									strSepString &
									Mid(rDbrec("R_Name1"), 1, 30) & strSepString &
									strSepString &
									strSepString

				'Methode vor Änderung am 21.02.2020
				'strValue = strValue &
				'					Replace(Format(rDbrec("V_Date"), "d"), ".", "/") & strSepString &
				'					strKonto & strSepString &
				'					rDbrec("FKHaben") & strSepString &
				'					strSepString &
				'					Mid(rDbrec("R_Name1"), 1, 30) & strSepString &
				'					Format(rDbrec("Betrag"), "0.00") & strSepString &
				'					0 & strSepString &
				'					0 & strSepString &
				'					0 & strSepString &
				'					1 & strSepString &
				'					strSepString &
				'					rDbrec("ZENr") & strSepString &
				'					"TVA" & strSepString &
				'					Mid(rDbrec("R_Name1"), 1, 30) & strSepString &
				'					strSepString &
				'					strSepString


				'amountExclusiveVAT = Val(rDbrec("Betrag")) / If(rDbrec("MWSTProz") = 0, 1, (rDbrec("MWSTProz") / 100)) ' (rDbrec("MWSTProz") / 100)
				'VATFromExclusiveAmount = amountExclusiveVAT * (If(Not bAsVerlust, 0, rDbrec("MWSTProz")) / 100)
				'Format(VATFromExclusiveAmount * -1, "f2") & strSepString &


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


	Public Class ClsExportLOInCresus

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

		Function GetLOValue4Cresus() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = "Success"

			Dim Conn As SqlConnection = New SqlConnection(Me._ExportSetting.DbConnString2Open)
			Dim i As Integer = 2
			Dim strValue As String = String.Empty
			Dim iLine As Integer = 1
			Dim strSepString As String = Chr(9)
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
				If strValue <> String.Empty Then strValue &= vbNewLine
				Dim strVorzeichen_2 As String = rDbrec("Vorzeichen_2").ToString
				Dim strVorzeichen_3 As String = rDbrec("Vorzeichen_3").ToString
				Dim dBetrag As Double = CDbl(rDbrec("Totalbetrag"))
				dBetrag = GetLAValueWithVorZeichen(Val(rDbrec("TotalBetrag").ToString),
																						 strVorzeichen_2,
																						 strVorzeichen_3)

				strValue &= Format(GetLastDayOfMonth(CDate(String.Format("01.{0}.{1}", Me._ExportSetting.SelectedMonth, Me._ExportSetting.SelectedYear))), "d").Replace(".", "/") & strSepString &
								 rDbrec("SKonto").ToString & strSepString &
								 rDbrec("HKonto").ToString & strSepString &
								 strSepString &
								 Mid(rDbrec("Bezeichnung").ToString, 1, 30) & strSepString &
								 Format(dBetrag, "f") & strSepString & strSepString &
								 "0" & strSepString &
								 "0" & strSepString &
								 "0" & strSepString &
								 "0" & strSepString &
								 strSepString &
								 "0" & strSepString &
								 strSepString &
								 strSepString &
								 strSepString &
								 strSepString
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

					m_UtilityUi.ShowInfoDialog(strMsg)

					Dim strDirectory As String = Path.GetDirectoryName(strResult)
					System.Diagnostics.Process.Start(strDirectory)
				End If
			End If

			Return strResult
		End Function

		Function GetLAValueWithVorZeichen(ByVal myValue As Double,
																ByVal strVorzeichen_2 As String,
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
