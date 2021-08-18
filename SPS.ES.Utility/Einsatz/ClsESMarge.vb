
Imports System.Data.SqlClient
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.XtraEditors

Imports SPS.ES.Utility.SPRPSUtility.ClsRPFunktionality
Imports SPS.MA.Lohn.Utility
Imports SPS.MA.Lohn.Utility.SPSLohnUtility

Imports SPS.MA.Lohn.Utility.SPSLohnUtility.ClsLOFunktionality

Imports SPS.ES.Utility.SPSESUtility.ClsESFunktionality

Imports SPProgUtility.SPTranslation.ClsTranslation

Imports System.IO
Imports SPProgUtility.Mandanten

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPProgUtility.ProgPath

Namespace CalculateESMarge

	Public Class ClsESMarge

#Region "private Consts"

		Private Const FORM_XML_DEFAULTVALUES_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region


		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI
		Private m_md As Mandant
		Private m_MandantFormXMLFile As String
		Private m_ProgPath As ClsProgPath


		Public Property _ESSetting As New ClsESDataSetting
		Private Property sAGBeitragOhneBVG As Single
		Private Property sAGBeitragWithBVG As Single
		Private Property MDrecord As SqlClient.SqlDataReader



#Region "Constructor"

		Public Sub New(ByVal _setting As ClsESDataSetting)

			m_md = New Mandant
			m_ProgPath = New ClsProgPath
			m_UtilityUI = New UtilityUI


			ModulConstants.MDData = ModulConstants.SelectedMDData(_setting.selectedMDNr)
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.PersonalizedData = ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

			m_MandantFormXMLFile = m_md.GetSelectedMDFormDataXMLFilename(ModulConstants.MDData.MDNr)


			Me._ESSetting = _setting
			Me.MDrecord = GetSelectedData4CalcESMargeInDr()

		End Sub


#End Region

		Public Shared Sub InitializeObject()

			ModulConstants.MDData = ModulConstants.SelectedMDData(0)
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

			ModulConstants.PersonalizedData = ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

		End Sub

		Private ReadOnly Property GetKDTarifMinerKDVer() As Boolean
			Get
				Dim calculatecustomerrefundinmarge As Boolean = ParseToBoolean(m_ProgPath.GetXMLNodeValue(m_MandantFormXMLFile, String.Format("{0}/calculatecustomerrefundinmarge", FORM_XML_DEFAULTVALUES_KEY)), False)

				Return calculatecustomerrefundinmarge
			End Get
		End Property

		Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
			Dim result As Boolean
			If (Not Boolean.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function


#Region "Einsatzmarge..."

		'Function CreateNewTempESLohnData() As SqlClient.SqlDataReader
		'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		'	Dim rFrec As SqlClient.SqlDataReader
		'	InitializeObject()

		'	Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

		'	Try
		'		Conn.Open()

		'		Dim sSql As String = "[Get Data For Calculating ESMarge In New ES]"
		'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
		'		Dim param As System.Data.SqlClient.SqlParameter

		'		param = New System.Data.SqlClient.SqlParameter
		'		param = cmd.Parameters.AddWithValue("@MDNr", Me._ESSetting.selectedMDNr)
		'		param = cmd.Parameters.AddWithValue("@MANr", Me._ESSetting.SelectedMANr)
		'		param = cmd.Parameters.AddWithValue("@ESNr", Me._ESSetting.SelectedESNr)
		'		param = cmd.Parameters.AddWithValue("@ESLNr", Me._ESSetting.SelectedESLohnNr)
		'		param = cmd.Parameters.AddWithValue("@KDNr", Me._ESSetting.SelectedKDNr)

		'		param = cmd.Parameters.AddWithValue("@ES_Ab", Me._ESSetting._dES_Ab)
		'		param = cmd.Parameters.AddWithValue("@GAVGruppe0", String.Empty)

		'		param = cmd.Parameters.AddWithValue("@StundenLohn", Me._ESSetting._sbLohn)
		'		param = cmd.Parameters.AddWithValue("@MAStdSpesen", Me._ESSetting._sMASSpesen)
		'		param = cmd.Parameters.AddWithValue("@MATSpesen", Me._ESSetting._sMATSpesen)
		'		param = cmd.Parameters.AddWithValue("@Tarif", Me._ESSetting._sKDTarif)
		'		param = cmd.Parameters.AddWithValue("@KDTSpesen", Me._ESSetting._sKDTSpesen)

		'		param = cmd.Parameters.AddWithValue("@GAV_FAG", Me._ESSetting._sFARProz)
		'		param = cmd.Parameters.AddWithValue("@GAV_WAG", Me._ESSetting._sWAGProz)
		'		param = cmd.Parameters.AddWithValue("@GAV_VAG", Me._ESSetting._sVAGProz)
		'		param = cmd.Parameters.AddWithValue("@GAV_WAG_s", Me._ESSetting._sWAGBtr)
		'		param = cmd.Parameters.AddWithValue("@GAV_VAG_s", Me._ESSetting._sVAGBtr)
		'		param = cmd.Parameters.AddWithValue("@SUVA", Me._ESSetting._strSuva)
		'		param = cmd.Parameters.AddWithValue("@GAVInfo_String", Me._ESSetting._strGAVInfo)

		'		cmd.CommandType = Data.CommandType.StoredProcedure
		'		rFrec = cmd.ExecuteReader
		'		rFrec.Read()

		'	Catch ex As Exception
		'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
		'		rFrec = Nothing

		'	End Try

		'	Return rFrec
		'End Function

		Function GetSelectedData4CalcESMargeInDr() As SqlClient.SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim rFrec As SqlClient.SqlDataReader
			Dim paramData As New List(Of String)

			InitializeObject()
			Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
			Dim sSql As String = String.Empty

			Try
				Conn.Open()

				sSql = If(Not Me._ESSetting.GetMarge4NewES, "[Load Existing Employment MargenData For Calculating]", "[Load New Employment MargenData For Calculating]")
				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter

				param = New System.Data.SqlClient.SqlParameter

				If Not Me._ESSetting.GetMarge4NewES Then
					param = cmd.Parameters.AddWithValue("@MDNr", Me._ESSetting.selectedMDNr)
					param = cmd.Parameters.AddWithValue("@MANr", Me._ESSetting.SelectedMANr)
					param = cmd.Parameters.AddWithValue("@ESNr", Me._ESSetting.SelectedESNr)
					param = cmd.Parameters.AddWithValue("@ESLNr", Me._ESSetting.SelectedESLohnNr)

					param = cmd.Parameters.AddWithValue("@Jahr", Me._ESSetting.SelectedYear)

				Else
					param = cmd.Parameters.AddWithValue("@MDNr", Me._ESSetting.selectedmdnr)
					param = cmd.Parameters.AddWithValue("@MANr", Me._ESSetting.SelectedMANr)
					param = cmd.Parameters.AddWithValue("@ESNr", Me._ESSetting.SelectedESNr)
					param = cmd.Parameters.AddWithValue("@ESLNr", Me._ESSetting.SelectedESLohnNr)
					param = cmd.Parameters.AddWithValue("@KDNr", Me._ESSetting.SelectedKDNr)

					param = cmd.Parameters.AddWithValue("@ES_Ab", Me._ESSetting._dES_Ab)
					param = cmd.Parameters.AddWithValue("@GAVGruppe0", String.Empty)

					param = cmd.Parameters.AddWithValue("@StundenLohn", Me._ESSetting._sbLohn)
					param = cmd.Parameters.AddWithValue("@MAStdSpesen", Me._ESSetting._sMASSpesen)
					param = cmd.Parameters.AddWithValue("@MATSpesen", Me._ESSetting._sMATSpesen)
					param = cmd.Parameters.AddWithValue("@Tarif", Me._ESSetting._sKDTarif)
					param = cmd.Parameters.AddWithValue("@KDTSpesen", Me._ESSetting._sKDTSpesen)

					param = cmd.Parameters.AddWithValue("@GAV_FAG", Me._ESSetting._sFARProz)
					param = cmd.Parameters.AddWithValue("@GAV_WAG", Me._ESSetting._sWAGProz)
					param = cmd.Parameters.AddWithValue("@GAV_VAG", Me._ESSetting._sVAGProz)
					param = cmd.Parameters.AddWithValue("@GAV_WAG_s", Me._ESSetting._sWAGBtr)
					param = cmd.Parameters.AddWithValue("@GAV_VAG_s", Me._ESSetting._sVAGBtr)
					param = cmd.Parameters.AddWithValue("@SUVA", Me._ESSetting._strSuva)
					param = cmd.Parameters.AddWithValue("@GAVInfo_String", Me._ESSetting._strGAVInfo)

				End If

				For Each itm In cmd.Parameters

					If itm.Direction = ParameterDirection.Output Then
						paramData.Add(String.Format("{0} {1} ({2}) = {3}", itm.ToString(), itm.DbType, "(OUTPUT)", itm.Value))

					Else
						paramData.Add(String.Format("{0} {1} ({2}) = {3}", itm.ToString(), itm.DbType, If(itm.DbType = DbType.String, Len(itm.Value.ToString()), 0), itm.Value))

					End If

				Next

				m_Logger.LogDebug(String.Format("{0} >>> {1} | Parameters: {2}", _ESSetting.GetMarge4NewES, sSql, String.Join(" | ", paramData)))
				cmd.CommandType = Data.CommandType.StoredProcedure
				rFrec = cmd.ExecuteReader
				rFrec.Read()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: SQL: {1} >>> {2}", ex.Message, sSql, String.Join(vbNewLine, paramData)))
				rFrec = Nothing

			End Try

			Return rFrec
		End Function

		'Function GetSelectedData4CalcESMargeInDt() As DataTable
		'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		'	Dim dt As DataTable = Nothing
		'	InitializeObject()
		'	Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

		'	Try

		'		Dim ds As New DataSet
		'		Dim sSql As String = "[Get Data For Calculating ESMarge]"
		'		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(sSql, Conn)
		'		cmd.CommandType = CommandType.StoredProcedure

		'		Dim objAdapter As New SqlDataAdapter

		'		cmd.Parameters.AddWithValue("@MDNr", Me._ESSetting.selectedMDNr)
		'		cmd.Parameters.AddWithValue("@MANr", Me._ESSetting.SelectedMANr)
		'		cmd.Parameters.AddWithValue("@ESNr", Me._ESSetting.SelectedESNr)
		'		cmd.Parameters.AddWithValue("@ESLNr", Me._ESSetting.SelectedESLohnNr)
		'		cmd.Parameters.AddWithValue("@Jahr", Me._ESSetting.SelectedYear)

		'		objAdapter.SelectCommand = cmd
		'		objAdapter.Fill(ds, "MargenData")

		'		Return ds.Tables(0)
		'	Catch ex As Exception

		'	End Try

		'	Return dt
		'End Function

		Function AnalyseGAVString() As Dictionary(Of String, String)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dResult As New Dictionary(Of String, String)
			Dim aGAVString As String() = Me.MDrecord("GAVInfo_String").ToString.Replace(":.. ", "").Split(CChar("¦"))

			Try

				For i As Integer = 0 To aGAVString.Count - 1
					If aGAVString(i) <> String.Empty Then
						Trace.WriteLine(aGAVString(i))
						If dResult.ContainsKey((aGAVString(i).Split(CChar(":"))(0))) Then
							'dResult.Add(String.Format("{0}|{1}", aGAVString(i).Split(CChar(":"))(0), i), CSng(Val(aGAVString(i).Split(CChar(":"))(1))))
							dResult.Add(String.Format("{0}|{1}", aGAVString(i).Split(CChar(":"))(0), i), aGAVString(i).Split(CChar(":"))(1))
						Else
							dResult.Add(aGAVString(i).Split(CChar(":"))(0), aGAVString(i).Split(CChar(":"))(1))
						End If

					End If
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

			End Try

			Return dResult
		End Function

		Function GetAGBeitraginES() As Single
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim sResult As String = 0

			Dim MAAlter As Integer
			Dim J_Lohn As Single
			Dim AHV_Lohn As Single
			Dim ALV_Lohn As Single
			Dim UV_Lohn As Single
			Dim BVG_Lohn As Single
			Dim J_Abzug As Single
			Dim sJ_AbzugWithBVG As Single
			Dim bIsMARentner As Boolean
			Dim lStdInYear As Long
			Dim aDebugValue As New Dictionary(Of String, String)

			If Me.MDrecord Is Nothing Or Not Me.MDrecord.HasRows Then
				Dim strMsg As String = String.Format("Ein Fehler in Ihrer Mandantendatenbank aufgetreten.{0}Bitte wiederholen Sie den Vorgang zu einem späteren Punkt.", vbNewLine)
				m_Logger.LogError(strMsg)
				DevExpress.XtraEditors.XtraMessageBox.Show(TranslateText(strMsg), TranslateText("AG-Beitrag"),
																										System.Windows.Forms.MessageBoxButtons.OK)
				Return 0
			End If

			Dim dGAVInfo As Dictionary(Of String, String) = AnalyseGAVString()
			Dim _sBLohn As Single = Me.MDrecord("Stundenlohn") ' Me._ESSetting.SelectedStdLohn
			Dim _strSuva As String = Me.MDrecord("Suva") ' Me._ESSetting.SelectedSuvaCode
			Dim _bMitBVG As Boolean = Me._ESSetting.ShowMargeWithBVG
			Dim _sFARProz As Single = Me.MDrecord("GAV_FAG") ' Me._ESSetting.SelectedFARProz
			Dim sParifondProz As Single = Me.MDrecord("ESParifon") ' Me._ESSetting.SelectedParifondProz
			Dim _sGavStdYear As Integer = If(CSng(dGAVInfo.ContainsKey("StdYear")), CSng(Math.Max(Val(dGAVInfo("StdYear")), 1800)), 0) ' Me._ESSetting.SelectedGAVJStd

			If _sBLohn = 0 Then Return 0
			aDebugValue.Add(String.Format("Bruttolohn"), Format(_sBLohn, "n"))

			MAAlter = GetMAAlter(Now.Year, Me.MDrecord("GebDat"))                    ' Alter vom Mitarbeiter
			aDebugValue.Add(String.Format("Kandidaten-Alter"), MAAlter)
			Try
				bIsMARentner = IsMARentner(Me.MDrecord("MANr"), _ESSetting.SelectedYear)
				aDebugValue.Add(String.Format("Ist als Rentner?"), TranslateText(If(bIsMARentner, "Ja", "Nein")))

				If _sGavStdYear > 0 Then
					lStdInYear = Val(_sGavStdYear)
				Else
					lStdInYear = IIf(Me.MDrecord("Geschlecht") = "M",
													 Val(Me.MDrecord("Jahrstd_M")), Val(Me.MDrecord("Jahrstd_W")))
				End If
				aDebugValue.Add(String.Format("Anzahl Arbeitsstunden"), Format(_sGavStdYear, "n"))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Jahresstunde. {1}", strMethodeName, ex.Message))
				Return 0

			End Try
			J_Lohn = _sBLohn * lStdInYear
			aDebugValue.Add(String.Format("{0}: ({1} * {2})", "Jahres Lohn", Format(_sBLohn, "n"), Format(lStdInYear, "n0")), Format(J_Lohn, "n"))
			J_Abzug = 0

			Try
				aDebugValue.Add("1. Teil: (AHV-Daten)", "")
				' (Rentner) mit Abzug
				If Not (Me.MDrecord("AHVCode") = "2" Or Me.MDrecord("AHVCode") = "0") And
																			MAAlter >= Me.MDrecord("MindestAlter") Then
					AHV_Lohn = J_Lohn
					aDebugValue.Add(String.Format("1.1. AHV_Lohn"), Format(AHV_Lohn, "n"))
					If bIsMARentner Then
						AHV_Lohn = AHV_Lohn - Me.MDrecord("Rentfrei_Jahr")
						aDebugValue.Add(String.Format("1.2. AHV_Lohn - Freibetrag {0} CHF", Format(Me.MDrecord("Rentfrei_Jahr"), "n")), Format(AHV_Lohn, "n"))
					End If

					' FAR-Beitrag einkalkulieren...
					'  If AHV_Lohn > 0 Then J_Abzug = AHV_Lohn * MDrec!AHV_AG / 100
					Dim sProz As Single = (Me.MDrecord("AHV_AG") + _sFARProz)
					If AHV_Lohn > 0 Then J_Abzug = AHV_Lohn * (sProz) / 100
					aDebugValue.Add(String.Format("1.1. Jahres-Abzug ({0} % + FAR {1} %))", Format(Me.MDrecord("AHV_AG"), "n"), _sFARProz), Format(J_Abzug, "n"))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.AHV_Lohn. {1}", strMethodeName, ex.Message))
				Return 0

			End Try

			Try
				aDebugValue.Add("2. Teil: (ALV-Daten)", "")
				If Not (Me.MDrecord("ALVCode") = "2" Or Me.MDrecord("ALVCode") = "0") And
																						MAAlter >= Me.MDrecord("MindestAlter") Then
					ALV_Lohn = J_Lohn
					If Not bIsMARentner Then
						If ALV_Lohn > Me.MDrecord("ALV1_HL") Then ALV_Lohn = Me.MDrecord("ALV1_HL")
						aDebugValue.Add(String.Format("2.1. ALV_Lohn bis höchstens {0} CHF", Format(Me.MDrecord("ALV1_HL"), "n")), Format(ALV_Lohn, "n"))

						J_Abzug += (ALV_Lohn * Me.MDrecord("ALV_AG") / 100)
						aDebugValue.Add(String.Format("2.1. Jahres-Abzug ({0} %)", Me.MDrecord("ALV_AG")), Format(J_Abzug, "n"))

					Else
						aDebugValue.Add(String.Format("2.1. ALV_Lohn (ist Rentner!)"), Format(ALV_Lohn, "n"))
						aDebugValue.Add(String.Format("2.1. Jahres-Abzug (bleibt wie oben weil Rentner ist!)"), Format(J_Abzug, "n"))

					End If
				End If
			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.ALV_Lohn. {1}", strMethodeName, ex.Message))
				Return 0

			End Try

			Try
				aDebugValue.Add("3. Teil: (SUVA-Daten)", "")
				If _strSuva.EndsWith("1") Or _strSuva.EndsWith("2") Then             ' mit Abzug
					UV_Lohn = J_Lohn
					If UV_Lohn > Me.MDrecord("SUVA_HL") Then UV_Lohn = Me.MDrecord("SUVA_HL")
					aDebugValue.Add(String.Format("3.1. UV_Lohn bis höchstens {0} CHF", Format(Me.MDrecord("SUVA_HL"), "n")), Format(UV_Lohn, "n"))

					Dim sProz As Single = (If(_strSuva.StartsWith("Z"), Me.MDrecord("Suva_Z"), Me.MDrecord("Suva_A")) + sParifondProz)
					J_Abzug = J_Abzug + (UV_Lohn * (sProz) / 100)
					aDebugValue.Add(String.Format("3.1. Jahres-Abzug ({0} % + Parifond {1} %))",
																				Format(If(_strSuva.StartsWith("Z"), Me.MDrecord("Suva_Z"), Me.MDrecord("Suva_A")), "n4"),
																							 sParifondProz), Format(J_Abzug, "n"))

					If _strSuva.EndsWith("2") Then                                   ' AG zahlt NBUV
						sProz = (IIf(Me.MDrecord("Geschlecht") = "M", Me.MDrecord("NBUV_M"), Me.MDrecord("NBUV_W")) + sParifondProz)
						J_Abzug += (UV_Lohn * (sProz) / 100)
						aDebugValue.Add(String.Format("3.2. Jahres-Abzug ({0} % + Parifond {1} %))",
																					Format(IIf(Me.MDrecord("Geschlecht") = "M", Me.MDrecord("NBUV_M"), Me.MDrecord("NBUV_W")), "n4"),
																					sParifondProz), Format(J_Abzug, "n"))
					End If
				End If
			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.SUVA_Lohn. {1}", strMethodeName, ex.Message))
				Return 0

			End Try


			Try
				aDebugValue.Add("4. Teil: (KTG-Daten)", "")
				Dim sProz As Single = 0
				If Me.MDrecord("Geschlecht") = "M" Then                                  ' KK-Abzug
					sProz = If(_strSuva.StartsWith("Z"), Me.MDrecord("KK_AG_MZ"), Me.MDrecord("KK_AG_MA"))

				Else
					sProz = If(_strSuva.StartsWith("Z"), Me.MDrecord("KK_AG_WZ"), Me.MDrecord("KK_AG_WA"))
				End If
				J_Abzug += (J_Lohn * sProz / 100)
				aDebugValue.Add(String.Format("4.1. Jahres-Abzug ({0} %)", sProz), Format(J_Abzug, "n"))

				sProz = (Me.MDrecord("Fak_Proz") + Me.MDrecord("X_Marge"))
				J_Abzug += (J_Lohn * sProz / 100)
				aDebugValue.Add(String.Format("4.2. Jahres-Abzug ((Verwaltungskosten für Sozialleistungen {0} % + FAK-Prozent {1}) %)",
																			Format(Me.MDrecord("X_Marge"), "n4"),
																			Format(Me.MDrecord("Fak_Proz"), "n4")), Format(J_Abzug, "n"))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.KTG-Lohn. {1}", strMethodeName, ex.Message))

			End Try

			Try
				aDebugValue.Add("5. Teil: (BVG-Daten)", "")
				BVG_Lohn = Math.Min((_sBLohn * Me.MDrecord("BVG_Std")), Me.MDrecord("BVG_Max_Jahr"))    ' 18.03.2013: (Leader) Altersgrenzen in BVG() berücksichtigt
				aDebugValue.Add(String.Format("5.1. BVG-Lohn bis höchsten {0} CHF", Format(Me.MDrecord("BVG_Max_Jahr"), "n")), Format(BVG_Lohn, "n"))

				BVG_Lohn = BVG_Lohn - Me.MDrecord("BVG_Koordination_Jahr")
				aDebugValue.Add(String.Format("5.2. BVG-Lohn - Koordinationslohn ({0} CHF)", Format(Me.MDrecord("BVG_Koordination_Jahr"), "n")), Format(BVG_Lohn, "n"))

				If BVG_Lohn > 0 Then
					Dim sProz As Single = GetBVGAns(Me.MDrecord("Gebdat"), Me.MDrecord("Geschlecht"), _ESSetting.SelectedYear)
					sJ_AbzugWithBVG = BVG_Lohn * sProz / 100
					aDebugValue.Add(String.Format("5.1. Jahres-Abzug ({0} %)", sProz), Format(J_Abzug + sJ_AbzugWithBVG, "n"))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.BVG_Lohn. {1}", strMethodeName, ex.Message))
				Return 0

			End Try

			Try
				sResult = J_Abzug / lStdInYear                                                                  ' AG-Beitrag ohne BVG
				Dim sBVGResult = (J_Abzug + sJ_AbzugWithBVG) / lStdInYear               ' AG-Beitrag mit BVG
				Me.sAGBeitragOhneBVG = sResult
				Me.sAGBeitragWithBVG = sBVGResult

				aDebugValue.Add(String.Format("6. Teil: AG-Beiträge {0} in CHF (Jahresabzug {1} / Jahresstunden {2})", "ohne BVG",
												Format(CSng(J_Abzug), "n"),
												Format(CSng(lStdInYear), "n")),
												Format(CSng(Me.sAGBeitragOhneBVG), "n"))
				aDebugValue.Add(String.Format("7. Teil: AG-Beiträge {0} in CHF (Jahresabzug {1} / Jahresstunden {2})", "mit BVG",
												Format(CSng(J_Abzug + sJ_AbzugWithBVG), "n"),
												Format(CSng(lStdInYear), "n")),
												Format(CSng(Me.sAGBeitragWithBVG), "n"))

				'aDebugValue.Add(String.Format("7. Teil: AG-Beiträge {0} in CHF (Jahresabzug / Jahresstunden)", "mit BVG"), Format(CSng(Me.sAGBeitragWithBVG), "n"))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Daten zusammenstellen. {1}", strMethodeName, ex.Message))

			End Try

			Me._ESSetting.aDebugMargenCalculation = aDebugValue

			Return sResult
		End Function

		''' <summary>
		''' Berechnet die Marge in einem Einsatz
		''' </summary>
		''' <returns>String.Format("{0}¦{1}¦{2}¦{3}¦{4}¦{5}", sMargeOhneBVG, sMargeMitBVG, sAvgMarge, sMargeInProz, Me.sAGBeitragOhneBVG, Me.sAGBeitragWithBVG)</returns>
		''' <remarks></remarks>
		Function GetSelectedESMarge() As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim sResult As String = String.Empty
			Me.sAGBeitragOhneBVG = 0
			Me.sAGBeitragWithBVG = 0

			Dim bAllowedKDVerInTarif As Boolean = GetKDTarifMinerKDVer

			If Me.MDrecord Is Nothing Or Not Me.MDrecord.HasRows Then
				Dim strMsg As String = String.Format("Ein Fehler in Ihrer Mandantendatenbank aufgetreten.{0}Bitte wiederholen Sie den Vorgang zu einem späteren Punkt.", vbNewLine)
				m_Logger.LogError(strMsg)
				DevExpress.XtraEditors.XtraMessageBox.Show(TranslateText(strMsg), TranslateText("Einsatzmarge"), System.Windows.Forms.MessageBoxButtons.OK)
				Return 0
			End If

			Try
				Dim aDebugValue As New Dictionary(Of String, String)
				Dim dGAVInfo As Dictionary(Of String, String) = AnalyseGAVString()

				Dim _sBLohn As Single = Val(Me.MDrecord("Stundenlohn"))
				Dim _sTarif As Single = Val(Me.MDrecord("Tarif"))
				Dim _sKDMinderung As Single = _sTarif * If(bAllowedKDVerInTarif, Val(Me.MDrecord("KD_UmsMin")), 0) / 100
				_sTarif = _sTarif - _sKDMinderung
				If _sKDMinderung <> 0 Then aDebugValue.Add(String.Format("Umsatzminderung %"), Format(Val(Me.MDrecord("KD_UmsMin")), "n"))
				aDebugValue.Add(String.Format("Tarif {0:n2} - {1:n2}%", Val(Me.MDrecord("Tarif")), Me.MDrecord("KD_UmsMin")), Format(_sTarif, "n"))


				Dim _strSuva As String = Me.MDrecord("Suva")
				Dim _bMitBVG As Boolean = Me._ESSetting.ShowMargeWithBVG
				Dim _sFARProz As Single = Val(Me.MDrecord("GAV_FAG"))
				Dim sParifondProz As Single = Val(Me.MDrecord("ESParifon"))
				Dim _sGavStdYear As Integer = If(Val(CSng(dGAVInfo.ContainsKey("StdYear"))), CSng(Math.Max(Val(dGAVInfo("StdYear")), 1800)), 0) ' Me._ESSetting.SelectedGAVJStd
				Dim _sLohnBruttoMarge As Single = 0

				Try

					If _sBLohn = 0 Or _sTarif = 0 Then Return 0
					_sLohnBruttoMarge = _sTarif - _sBLohn
					_sLohnBruttoMarge -= CSng(Val(Me.MDrecord("GAV_WAG_S"))) - CSng(Val(Me.MDrecord("GAV_VAG_S")))
					_sLohnBruttoMarge -= CSng(Val(Me.MDrecord("MAStdSpesen")))

					If CSng(Val(Me.MDrecord("MATSpesen"))) <> CSng(Val(Me.MDrecord("KDTSpesen"))) Then
						_sLohnBruttoMarge -= CSng(Val(Me.MDrecord("MATSpesen"))) / 8.25
						_sLohnBruttoMarge += CSng(Val(Me.MDrecord("KDTSpesen"))) / 8.25
						_sTarif = Val(Me.MDrecord("Tarif")) + (Val(Me.MDrecord("KDTSpesen")) / 8.25)
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Berechnung der Lohn und Tarif. {1}", strMethodeName, ex.Message))

				End Try
				Try
					Dim sAGAbzug As Single = GetAGBeitraginES()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Berechnung der AG-Beitrag. {1}", strMethodeName, ex.Message))

				End Try

				Try
					Dim sMargeOhneBVG As Single = _sLohnBruttoMarge - Me.sAGBeitragOhneBVG
					Dim sMargeMitBVG As Single = _sLohnBruttoMarge - Me.sAGBeitragWithBVG
					Dim sAvgMarge As Single = (sMargeOhneBVG + sMargeMitBVG) / 2
					Dim sAvgMargeInProz As Single = (sAvgMarge * 100) / _sTarif

					Dim sMargeOhneBVGInProz As Single = (sMargeOhneBVG * 100) / _sTarif
					Dim sMargeWithBVGInProz As Single = (sMargeMitBVG * 100) / _sTarif

					Dim strDebugValue As String = String.Empty
					For i As Integer = 0 To aDebugValue.ToList.Count - 1
						strDebugValue &= String.Format("¦{0}:{1}", aDebugValue.Keys(i), aDebugValue.Values(i))
					Next

					For i As Integer = 0 To Me._ESSetting.aDebugMargenCalculation.ToList.Count - 1
						strDebugValue &= String.Format("¦{0}:{1}", Me._ESSetting.aDebugMargenCalculation.Keys(i), Me._ESSetting.aDebugMargenCalculation.Values(i))
					Next
					sResult = String.Format("{0}¦{1}¦{2}¦{3}¦{4}¦{5}¦{6}¦{7}{8}", sMargeOhneBVG, sMargeMitBVG, sAvgMarge, sAvgMargeInProz,
																	Me.sAGBeitragOhneBVG, Me.sAGBeitragWithBVG, sMargeOhneBVGInProz, sMargeWithBVGInProz,
																	strDebugValue)

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}.Berechnung der Marge. {1}", strMethodeName, ex.Message))

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

			End Try

			Return sResult
		End Function


#End Region



		Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			'Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

	End Class


End Namespace
