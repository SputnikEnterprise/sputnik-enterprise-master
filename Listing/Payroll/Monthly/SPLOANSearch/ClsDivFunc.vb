
Imports System.Data.SqlClient
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPLOANSearch.ClsDataDetail
Imports SP.Infrastructure.Logging
Imports System.Data

Public Class ClsDivFunc

#Region "Diverses"

	'// Get4What._strModul4What
	Dim _strModul4What As String
	Public Property Get4What() As String
		Get
			Return _strModul4What
		End Get
		Set(ByVal value As String)
			_strModul4What = value
		End Set
	End Property

	'// Query.GetSearchQuery
	Dim _strQuery As String
	Public Property GetSearchQuery() As String
		Get
			Return _strQuery
		End Get
		Set(ByVal value As String)
			_strQuery = value
		End Set
	End Property

	'// Query.GetSearchCommand
	Dim _strCommand As SqlCommand
	Public Property GetSearchCommand() As SqlCommand
		Get
			Return _strCommand
		End Get
		Set(ByVal value As SqlCommand)
			_strCommand = value
		End Set
	End Property

	'// LargerLV
	Dim _bLargerLV As Boolean
	Public Property GetLargerLV() As Boolean
		Get
			Return _bLargerLV
		End Get
		Set(ByVal value As Boolean)
			_bLargerLV = value
		End Set
	End Property

#End Region

#Region "Funktionen für LvClick in der Suchmaske..."

	'// Allgemeiner Zwischenspeicher
	Dim _strSelektion As String
	Public Property GetSelektion() As String
		Get
			Return _strSelektion
		End Get
		Set(ByVal value As String)
			_strSelektion = value
		End Set
	End Property

	' // ID
	Dim _strID As String
	Public Property GetID() As String
		Get
			Return _strID
		End Get
		Set(ByVal value As String)
			_strID = value
		End Set
	End Property

	' // KrediNr
	Dim _strKrediNr As String
	Public Property GetKrediNr() As String
		Get
			Return _strKrediNr
		End Get
		Set(ByVal value As String)
			_strKrediNr = value
		End Set
	End Property

	' // LONr
	Dim _strLONr As String
	Public Property GetLONr() As String
		Get
			Return _strLONr
		End Get
		Set(ByVal value As String)
			_strLONr = value
		End Set
	End Property

	' // LP
	Dim _strLP As String
	Public Property GetLP() As String
		Get
			Return _strLP
		End Get
		Set(ByVal value As String)
			_strLP = value
		End Set
	End Property

	' // Jahr 
	Dim _strJahr As String
	Public Property GetJahr() As String
		Get
			Return _strJahr
		End Get
		Set(ByVal value As String)
			_strJahr = value
		End Set
	End Property

	'// MANr
	Dim _strMANr As String
	Public Property GetMANr() As String
		Get
			Return _strMANr
		End Get
		Set(ByVal value As String)
			_strMANr = value
		End Set
	End Property

	'// KDNr
	Dim _strKDNr As String
	Public Property GetKDNr() As String
		Get
			Return _strKDNr
		End Get
		Set(ByVal value As String)
			_strKDNr = value
		End Set
	End Property

	'// Kundennamen
	Dim _strKDName As String
	Public Property GetKDName() As String
		Get
			Return _strKDName
		End Get
		Set(ByVal value As String)
			_strKDName = value
		End Set
	End Property

	'// Kandidatenname
	Dim _strMAName As String
	Public Property GetMAName() As String
		Get
			Return _strMAName
		End Get
		Set(ByVal value As String)
			_strMAName = value
		End Set
	End Property

	'// Kandidatenvorname
	Dim _strMAVorname As String
	Public Property GetMAVorname() As String
		Get
			Return _strMAVorname
		End Get
		Set(ByVal value As String)
			_strMAVorname = value
		End Set
	End Property

	'// GAV-Beruf
	Dim _strESGAVBeruf As String
	Public Property GetESGAVBeruf() As String
		Get
			Return _strESGAVBeruf
		End Get
		Set(ByVal value As String)
			_strESGAVBeruf = value
		End Set
	End Property

	'// Einsatz als
	Dim _strESEinsatzAls As String
	Public Property GetESEinsatzAls() As String
		Get
			Return _strESEinsatzAls
		End Get
		Set(ByVal value As String)
			_strESEinsatzAls = value
		End Set
	End Property

	'// Query.GetSearchQuery
	Dim _strTelNr As String
	Public Property GetTelNr() As String
		Get
			Return _strTelNr
		End Get
		Set(ByVal value As String)
			_strTelNr = value
		End Set
	End Property

#End Region

#Region "LL_Properties"
	'// Print.LLDocName
	Dim _LLDocName As String
	Public Property LLDocName() As String
		Get
			Return _LLDocName
		End Get
		Set(ByVal value As String)
			_LLDocName = value
		End Set
	End Property

	'// Print.LLDocLabel
	Dim _LLDocLabel As String
	Public Property LLDocLabel() As String
		Get
			Return _LLDocLabel
		End Get
		Set(ByVal value As String)
			_LLDocLabel = value
		End Set
	End Property

	'// Print.LLFontDesent
	Dim _LLFontDesent As Integer
	Public Property LLFontDesent() As Integer
		Get
			Return _LLFontDesent
		End Get
		Set(ByVal value As Integer)
			_LLFontDesent = value
		End Set
	End Property

	'// Print.LLIncPrv
	Dim _LLIncPrv As Integer
	Public Property LLIncPrv() As Integer
		Get
			Return _LLIncPrv
		End Get
		Set(ByVal value As Integer)
			_LLIncPrv = value
		End Set
	End Property

	'// Print.LLParamCheck
	Dim _LLParamCheck As Integer
	Public Property LLParamCheck() As Integer
		Get
			Return _LLParamCheck
		End Get
		Set(ByVal value As Integer)
			_LLParamCheck = value
		End Set
	End Property

	'// Print.LLKonvertName
	Dim _LLKonvertName As Integer
	Public Property LLKonvertName() As Integer
		Get
			Return _LLKonvertName
		End Get
		Set(ByVal value As Integer)
			_LLKonvertName = value
		End Set
	End Property

	'// Print.LLZoomProz
	Dim _LLZoomProz As Integer
	Public Property LLZoomProz() As Integer
		Get
			Return _LLZoomProz
		End Get
		Set(ByVal value As Integer)
			_LLZoomProz = value
		End Set
	End Property

	'// Print.LLCopyCount
	Dim _LLCopyCount As Integer
	Public Property LLCopyCount() As Integer
		Get
			Return _LLCopyCount
		End Get
		Set(ByVal value As Integer)
			_LLCopyCount = value
		End Set
	End Property

	'// Print.LLExportedFilePath
	Dim _LLExportedFilePath As String
	Public Property LLExportedFilePath() As String
		Get
			Return _LLExportedFilePath
		End Get
		Set(ByVal value As String)
			_LLExportedFilePath = value
		End Set
	End Property

	'// Print.LLExportedFileName
	Dim _LLExportedFileName As String
	Public Property LLExportedFileName() As String
		Get
			Return _LLExportedFileName
		End Get
		Set(ByVal value As String)
			_LLExportedFileName = value
		End Set
	End Property

	'// Print.LLPrintInDiffColor
	Dim _LLPrintInDiffColor As Boolean
	Public Property LLPrintInDiffColor() As Boolean
		Get
			Return _LLPrintInDiffColor
		End Get
		Set(ByVal value As Boolean)
			_LLPrintInDiffColor = value
		End Set
	End Property

	'// Print.LLExportfilter
	Dim _LLExportfilter As String
	Public Property LLExportfilter() As String
		Get
			Return _LLExportfilter
		End Get
		Set(ByVal value As String)
			_LLExportfilter = value
		End Set
	End Property

	'// Print.LLExporterName
	Dim _LLExporterName As String
	Public Property LLExporterName() As String
		Get
			Return _LLExporterName
		End Get
		Set(ByVal value As String)
			_LLExporterName = value
		End Set
	End Property

	'// Print.LLExporterFileName
	Dim _LLExporterFileName As String
	Public Property LLExporterFileName() As String
		Get
			Return _LLExporterFileName
		End Get
		Set(ByVal value As String)
			_LLExporterFileName = value
		End Set
	End Property

#End Region

#Region "US Setting"

	'// USeMail (= eMail des Personalvermittlers)
	Dim _USeMail As String
	Public Property USeMail() As String
		Get
			Return _USeMail
		End Get
		Set(ByVal value As String)
			_USeMail = value
		End Set
	End Property

	'// USTelefon (= USTelefon des Personalvermittlers)
	Dim _USTelefon As String
	Public Property USTelefon() As String
		Get
			Return _USTelefon
		End Get
		Set(ByVal value As String)
			_USTelefon = value
		End Set
	End Property

	'// USTelefax (= USTelefax des Personalvermittlers)
	Dim _USTelefax As String
	Public Property USTelefax() As String
		Get
			Return _USTelefax
		End Get
		Set(ByVal value As String)
			_USTelefax = value
		End Set
	End Property

	'// USVorname (= USVorname des Personalvermittlers)
	Dim _USVorname As String
	Public Property USVorname() As String
		Get
			Return _USVorname
		End Get
		Set(ByVal value As String)
			_USVorname = value
		End Set
	End Property

	'// USAnrede (= USAnrede des Personalvermittlers)
	Dim _USAnrede As String
	Public Property USAnrede() As String
		Get
			Return _USAnrede
		End Get
		Set(ByVal value As String)
			_USAnrede = value
		End Set
	End Property

	'// USNachname (= USNachname des Personalvermittlers)
	Dim _USNachname As String
	Public Property USNachname() As String
		Get
			Return _USNachname
		End Get
		Set(ByVal value As String)
			_USNachname = value
		End Set
	End Property

	'// USMDName (= MDName des Personalvermittlers)
	Dim _USMDname As String
	Public Property USMDname() As String
		Get
			Return _USMDname
		End Get
		Set(ByVal value As String)
			_USMDname = value
		End Set
	End Property

	'// MDName2 (= MDName2 des Personalvermittlers)
	Dim _USMDname2 As String
	Public Property USMDname2() As String
		Get
			Return _USMDname2
		End Get
		Set(ByVal value As String)
			_USMDname2 = value
		End Set
	End Property

	'// MDName3 (= MDName3 des Personalvermittlers)
	Dim _USMDname3 As String
	Public Property USMDname3() As String
		Get
			Return _USMDname3
		End Get
		Set(ByVal value As String)
			_USMDname3 = value
		End Set
	End Property

	'// USMDPostfach (= MDPostfach des Personalvermittlers)
	Dim _USMDPostfach As String
	Public Property USMDPostfach() As String
		Get
			Return _USMDPostfach
		End Get
		Set(ByVal value As String)
			_USMDPostfach = value
		End Set
	End Property

	'// USMDStrasse (= MDstrasse des Personalvermittlers)
	Dim _USMDStrasse As String
	Public Property USMDStrasse() As String
		Get
			Return _USMDStrasse
		End Get
		Set(ByVal value As String)
			_USMDStrasse = value
		End Set
	End Property

	'// USMDOrt (= MDOrt des Personalvermittlers)
	Dim _USMDOrt As String
	Public Property USMDOrt() As String
		Get
			Return _USMDOrt
		End Get
		Set(ByVal value As String)
			_USMDOrt = value
		End Set
	End Property

	'// USMDPLZ (= MDPLZ des Personalvermittlers)
	Dim _USMDPlz As String
	Public Property USMDPlz() As String
		Get
			Return _USMDPlz
		End Get
		Set(ByVal value As String)
			_USMDPlz = value
		End Set
	End Property

	'// USMDLand (= MDLand des Personalvermittlers)
	Dim _USMDLand As String
	Public Property USMDLand() As String
		Get
			Return _USMDLand
		End Get
		Set(ByVal value As String)
			_USMDLand = value
		End Set
	End Property

	'// USMDTelefon (= MDTelefon des Personalvermittlers)
	Dim _USMDTelefon As String
	Public Property USMDTelefon() As String
		Get
			Return _USMDTelefon
		End Get
		Set(ByVal value As String)
			_USMDTelefon = value
		End Set
	End Property

	'// USMDTelefax (= MDTelefax des Personalvermittlers)
	Dim _USMDTelefax As String
	Public Property USMDTelefax() As String
		Get
			Return _USMDTelefax
		End Get
		Set(ByVal value As String)
			_USMDTelefax = value
		End Set
	End Property

	'// USMDeMail (= MDeMail des Personalvermittlers)
	Dim _USMDeMail As String
	Public Property USMDeMail() As String
		Get
			Return _USMDeMail
		End Get
		Set(ByVal value As String)
			_USMDeMail = value
		End Set
	End Property

	'// USMDHomepage (= MDHomepage des Personalvermittlers)
	Dim _USMDHomepage As String
	Public Property USMDHomepage() As String
		Get
			Return _USMDHomepage
		End Get
		Set(ByVal value As String)
			_USMDHomepage = value
		End Set
	End Property

#End Region

End Class

Public Class ClsDbFunc

#Region "Private Fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private Property SelectedMonatvon As String
	Private Property SelectedMonatbis As String
	Private Property SelectedJahrvon As String
	Private Property SelectedJahrbis As String

	Private Property IsKTG60Day As Boolean
	Private Property IsKTG720Day As Boolean

	Private Property SelectedMANr As String
	Private Property SelectedLANr As String
	Private Property SelectedGAVBeruf As String
	Private Property SelectedGAVG1 As String
	Private Property SelectedNullBetrag As Boolean

	Private m_md As Mandant
	Private m_common As CommonSetting
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

#End Region


#Region "Contructor"

	Public Sub New()

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

	End Sub

#End Region


#Region "Funktionen zur Suche nach Daten..."

	Function GetQuerySQLString(ByVal frmTest As frmLOANSearch) As String
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		m_Logger.LogDebug(String.Format("TimeoutException: ({0}) {1}", conn.ConnectionTimeout, conn.ConnectionString))

		ClsDataDetail.SelectedDataTable.Clear()
		Dim selItem As ClsDataDetail.SelectionItem

		With frmTest
			' Default-Werte
			Dim manrList As String = ""
			Dim jahrVon As String = Date.Now.Year.ToString
			Dim jahrBis As String = Date.Now.Year.ToString
			Dim monatVon As String = "1"
			Dim monatBis As String = "12"
			Dim lohnartenList As String = ""
			Dim lohnartenBez As String = ""
			Dim beruf As String = ""
			Dim gruppe1 As String = ""
			Dim betragNull As Integer = 1
			ClsDataDetail.SelectedContainer.Clear()

			' MANR
			If .txt_MANr.Text.Length > 0 Then
				For Each manr As String In .txt_MANr.Text.Split(CChar(","))
					manrList += String.Format("{0},", manr)
				Next
				manrList = manrList.Substring(0, manrList.Length - 1)

				' Filterbezeichnung
				selItem = New ClsDataDetail.SelectionItem
				selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.MANR
				selItem.Text = manrList
				ClsDataDetail.SelectedContainer.Add(String.Format(m_Translate.GetSafeTranslationValue("MANr.: {0}"), manrList))
			End If
			Me.SelectedMANr = manrList

			' MONAT VON
			If .Cbo_MonatVon.Text.Length > 0 Then
				monatVon = .Cbo_MonatVon.Text
				' Filterbezeichnung
				selItem = New ClsDataDetail.SelectionItem
				selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.MonatVon
				selItem.Text = monatVon
				ClsDataDetail.SelectedContainer.Add(String.Format(m_Translate.GetSafeTranslationValue("Monat: {0}"), monatVon))
			End If
			Me.SelectedMonatvon = monatVon

			' MONAT BIS
			If .Cbo_MonatBis.Text.Length > 0 And .Cbo_MonatBis.Visible Then
				monatBis = .Cbo_MonatBis.Text
				' Filterbezeichnung
				selItem = New ClsDataDetail.SelectionItem
				selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.MonatBis
				selItem.Text = monatBis
				ClsDataDetail.SelectedContainer(ClsDataDetail.SelectedContainer.Count - 1) = String.Format("{0} - {1}",
																																																	 ClsDataDetail.SelectedContainer(ClsDataDetail.SelectedContainer.Count - 1).ToString,
																																																	 monatBis)
			Else
				monatBis = .Cbo_MonatVon.Text
				If .Cbo_MonatBis.Visible Or .Cbo_JahrBis.Visible Then
					' Filterbezeichnung
					selItem = New ClsDataDetail.SelectionItem
					selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.MonatBis
					selItem.Text = monatBis
				End If

			End If
			Me.SelectedMonatbis = monatBis

			' JAHR VON
			If .Cbo_JahrVon.Text.Length > 0 Then
				jahrVon = .Cbo_JahrVon.Text
				' Filterbezeichnung
				selItem = New ClsDataDetail.SelectionItem
				selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.JahrVon
				selItem.Text = jahrVon
				ClsDataDetail.SelectedContainer.Add(String.Format(m_Translate.GetSafeTranslationValue("Jahr: {0}"), jahrVon)) 'selItem)
			End If
			Me.SelectedJahrvon = jahrVon

			' JAHR BIS
			If .Cbo_JahrBis.Text.Length > 0 And .Cbo_JahrBis.Visible Then
				jahrBis = .Cbo_JahrBis.Text
				' Filterbezeichnung
				selItem = New ClsDataDetail.SelectionItem
				selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.JahrBis
				selItem.Text = jahrBis
				ClsDataDetail.SelectedContainer(ClsDataDetail.SelectedContainer.Count - 1) = String.Format("{0} - {1}",
																																																	 ClsDataDetail.SelectedContainer(ClsDataDetail.SelectedContainer.Count - 1).ToString, jahrBis)
			Else
				jahrBis = .Cbo_JahrVon.Text
				If .Cbo_MonatBis.Visible Or .Cbo_JahrBis.Visible Then
					' Filterbezeichnung
					selItem = New ClsDataDetail.SelectionItem
					selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.JahrBis
					selItem.Text = jahrBis
				End If
			End If
			Me.SelectedJahrbis = jahrBis

			' LOHNARTEN
			If .Cbo_Lohnart.Text.Length > 0 Then
				Dim strValue As String = .Cbo_Lohnart.Text.Trim
				Dim aValues As String() = strValue.Split(CChar(","))

				For Each la As String In aValues
					Dim dLANr As Decimal = CDec(Val(la.Trim.Split(CChar(" "))(0)))
					lohnartenList += String.Format("{0},", dLANr)
					lohnartenBez = strValue.Trim
				Next
				lohnartenList = lohnartenList.Substring(0, lohnartenList.Length - 1)
				If lohnartenList.Split(CChar(",")).Count > 1 Then
					lohnartenBez = ""
				End If
				ClsDataDetail.SelectedContainer.Add(String.Format(m_Translate.GetSafeTranslationValue("Lohnart: {0}"), strValue))
			End If
			Me.SelectedLANr = lohnartenList

			' BERUF
			If .cbo_Beruf.Text.Trim.Length > 0 Then
				beruf = .cbo_Beruf.Text

				' Filterbezeichnung
				selItem = New ClsDataDetail.SelectionItem
				selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.Beruf
				selItem.Text = beruf
				ClsDataDetail.SelectedContainer.Add(String.Format(m_Translate.GetSafeTranslationValue("Beruf: {0}"), beruf)) 'selItem)
			End If
			Me.SelectedGAVBeruf = beruf

			' 1. KATEGORIE (GRUPPE1)
			If .Cbo_1Kategorie.Text.Trim.Length > 0 Then
				gruppe1 = .Cbo_1Kategorie.Text

				' Filterbezeichnung
				selItem = New ClsDataDetail.SelectionItem
				selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.Kategorie
				selItem.Text = gruppe1
				ClsDataDetail.SelectedContainer.Add(String.Format(m_Translate.GetSafeTranslationValue("Berufkategorie: {0}"), gruppe1))
			End If
			Me.SelectedGAVG1 = gruppe1

			' BETRAG NULL (immer akiviert)
			' Filterbezeichnung
			selItem = New ClsDataDetail.SelectionItem
			selItem.Bezeichnung = ClsDataDetail.SuchkriterienList.BetragNull
			If .ChkNullBetrag.Checked Then
				betragNull = 1
				selItem.Text = m_Translate.GetSafeTranslationValue("Die Nuller-Beträge ausblenden")
			Else
				betragNull = 0
				selItem.Text = m_Translate.GetSafeTranslationValue("Die Nuller-Beträge einblenden")
			End If
			Me.SelectedNullBetrag = If(betragNull = 0, False, True)
			ClsDataDetail.SelectedContainer.Add(String.Format("{0}", selItem.Text))

			DropMyTableInDb()
			If ClsDataDetail.SelectedListArt = ClsDataDetail.ListArt.Mitarbeiterlohnart Then
				sSql = Me.GetSQLSelectString4MALohnListe
				m_Logger.LogDebug(sSql)

				Dim cmd As SqlCommand = New SqlCommand(sSql, conn)
				Dim pjahrVon As SqlParameter = New SqlParameter("@jahrVon", SqlDbType.Int, 4)
				Dim pjahrBis As SqlParameter = New SqlParameter("@jahrBis", SqlDbType.Int, 4)
				Dim pmonatVon As SqlParameter = New SqlParameter("@monatVon", SqlDbType.Int, 2)
				Dim pmonatBis As SqlParameter = New SqlParameter("@monatBis", SqlDbType.Int, 2)
				Dim pberuf As SqlParameter = New SqlParameter("@beruf", SqlDbType.NVarChar, 100)
				Dim pgruppe1 As SqlParameter = New SqlParameter("@gruppe1", SqlDbType.NVarChar, 200)
				pjahrVon.Value = Val(jahrVon)
				pjahrBis.Value = Val(jahrBis)
				pmonatVon.Value = Val(monatVon)
				pmonatBis.Value = Val(monatBis)
				pberuf.Value = beruf
				pgruppe1.Value = gruppe1

				cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
				cmd.Parameters.Add(pjahrVon)
				cmd.Parameters.Add(pjahrBis)
				cmd.Parameters.Add(pmonatVon)
				cmd.Parameters.Add(pmonatBis)
				cmd.Parameters.Add(pberuf)
				cmd.Parameters.Add(pgruppe1)

				Dim daLOAN As SqlDataAdapter = New SqlDataAdapter(cmd)
				Dim dt As DataTable = New DataTable("LOAN")

				If daLOAN.Fill(dt) > 0 Then
					' Tabelle auf der Datenbank speichern
					InsertDataTableToDatabase(dt)
					ClsDataDetail.SelectedDataTable = dt
				End If

				' Die Query für die Anzeige anpassen
				For Each param As SqlParameter In cmd.Parameters
					Dim replaceValue As String = ""
					If param.SqlDbType = SqlDbType.NVarChar Then
						replaceValue = String.Format("'{0}'", param.Value)
					Else
						replaceValue = param.Value.ToString
					End If
					sSql = sSql.Replace(param.ParameterName, replaceValue)
				Next

			ElseIf ClsDataDetail.SelectedListArt = ClsDataDetail.ListArt.KTGLohnarten Then
				sSql = Me.GetSQLSelectString4KTGListe
				m_Logger.LogDebug(sSql)

				Dim cmd As SqlCommand = New SqlCommand(sSql, conn)
				Dim pjahrVon As SqlParameter = New SqlParameter("@jahrVon", SqlDbType.Int, 4)
				Dim pjahrBis As SqlParameter = New SqlParameter("@jahrBis", SqlDbType.Int, 4)
				Dim pmonatVon As SqlParameter = New SqlParameter("@monatVon", SqlDbType.Int, 2)
				Dim pmonatBis As SqlParameter = New SqlParameter("@monatBis", SqlDbType.Int, 2)
				Dim pberuf As SqlParameter = New SqlParameter("@beruf", SqlDbType.NVarChar, 100)
				Dim pgruppe1 As SqlParameter = New SqlParameter("@gruppe1", SqlDbType.NVarChar, 200)
				pjahrVon.Value = Val(jahrVon)
				pjahrBis.Value = Val(jahrBis)
				pmonatVon.Value = Val(monatVon)
				pmonatBis.Value = Val(monatBis)
				pberuf.Value = beruf
				pgruppe1.Value = gruppe1

				cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
				cmd.Parameters.Add(pjahrVon)
				cmd.Parameters.Add(pjahrBis)
				cmd.Parameters.Add(pmonatVon)
				cmd.Parameters.Add(pmonatBis)
				cmd.Parameters.Add(pberuf)
				cmd.Parameters.Add(pgruppe1)

				Dim daLOAN As SqlDataAdapter = New SqlDataAdapter(cmd)
				Dim dt As DataTable = New DataTable("LOAN")

				If daLOAN.Fill(dt) > 0 Then
					' Tabelle auf der Datenbank speichern
					InsertDataTableToDatabase(dt)
					ClsDataDetail.SelectedDataTable = dt
				End If

				' Die Query für die Anzeige anpassen
				For Each param As SqlParameter In cmd.Parameters
					Dim replaceValue As String = ""
					If param.SqlDbType = SqlDbType.NVarChar Then
						replaceValue = String.Format("'{0}'", param.Value)
					Else
						replaceValue = param.Value.ToString
					End If
					sSql = sSql.Replace(param.ParameterName, replaceValue)
				Next

			ElseIf ClsDataDetail.SelectedListArt = ClsDataDetail.ListArt.Lohnartenrekapitulation Then
				' LOHNARTENREKAPITULATION ARBEITNEHMER
				Dim cmd As SqlCommand = New SqlCommand(sSql, conn)
				cmd.CommandText = "[Add Selected LOLData To LORekapDb For AGAN With Mandant]"
				cmd.CommandType = CommandType.StoredProcedure
				Dim pMANR As SqlParameter = New SqlParameter("@manrListe", SqlDbType.NVarChar, 4000)
				Dim pUSNr As SqlParameter = New SqlParameter("@USNr", SqlDbType.Int, 4)
				Dim pjahrVon As SqlParameter = New SqlParameter("@MDYear", SqlDbType.Int, 4)
				Dim pmonatVon As SqlParameter = New SqlParameter("@LP", SqlDbType.Int, 2)
				Dim pbetragNull As SqlParameter = New SqlParameter("@betragNull", SqlDbType.Bit, 1)
				Dim pforAG As SqlParameter = New SqlParameter("@forAG", SqlDbType.Bit, 1)
				pMANR.Value = manrList
				pUSNr.Value = _ClsProgSetting.GetLogedUSNr
				pjahrVon.Value = Val(jahrVon)
				pmonatVon.Value = Val(monatVon)
				pbetragNull.Value = betragNull
				pforAG.Value = 0 ' 0 = für Arbeitnehmerlohnarten

				cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
				cmd.Parameters.Add(pMANR)
				cmd.Parameters.Add(pUSNr)
				cmd.Parameters.Add(pjahrVon)
				cmd.Parameters.Add(pmonatVon)
				cmd.Parameters.Add(pbetragNull)
				cmd.Parameters.Add(pforAG)
				Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
				Dim dt As DataTable = New DataTable("REKAP")

				If da.Fill(dt) > 0 Then ClsDataDetail.SelectedDataTable = dt
				sSql = String.Format("EXEC [Add Selected LOLData To LORekapDb For AGAN With Mandant] @MDNr int = {0}, @manrListe='{1}', " &
														 "@USNR={2}, @MDYear='{3}', @LP={4}, @betragNull={5}, @forAG={6}",
														 m_InitialData.MDData.MDNr, manrList, m_InitialData.UserData.UserNr, jahrVon, monatVon, betragNull, 0)

				m_Logger.LogDebug(sSql)

			End If
			sSqlLen = Len(sSql)

		End With

		Return sSql

	End Function

	Function GetSQLSelectString4MALohnListe() As String
		Dim strResult As String = String.Empty
		' LISTE DER MITARBETERLOHNARTEN
		strResult = "SELECT LOL.MANR, LOL.Jahr, LOL.LP, LOL.LANR, LA.LALOText, LOL.M_Bas, LOL.M_Anz, LOL.M_Ans, LOL.M_Btr, "
		strResult &= "LOL.GAV_Kanton, "
		strResult &= "LOL.GAV_Beruf, LOL.GAV_Gruppe1, MA.Nachname, MA.Vorname, "
		strResult &= "MA.AHV_Nr, MA.AHV_Nr_New, MA.Land As MALand, MA.PLZ, MA.Ort, MA.Geschlecht, MA.Nationality, "
		strResult &= "MA.Strasse, MA.GebDat, MA.KST As MAKST, @jahrVon As VonJahr, "
		strResult &= "@monatVon As VonMonat, @jahrBis As BisJahr, @monatBis As BisMonat, LOL.GAV_Kanton, "
		strResult &= " MA.PLZ, MA.Ort "
		strResult &= "FROM LOL "
		strResult &= "LEFT JOIN Mitarbeiter MA ON "
		strResult &= "LOL.MANR = MA.MANR "
		strResult &= "LEFT JOIN LA ON "
		strResult &= "LA.LANR = LOL.LANR And "
		strResult &= "LA.LAJahr = LOL.Jahr "

		strResult &= "WHERE LOL.MDNr = @MDNr And "

		If Me.SelectedMANr.Length > 0 Then
			strResult &= String.Format("LOL.MANR In ({0}) And ", Me.SelectedMANr)
		End If
		If Me.SelectedNullBetrag Then
			strResult &= "LOL.M_Btr <> 0 And "
		End If
		strResult &= "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or "
		strResult &= "(LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or "
		strResult &= "(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
		strResult &= "(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or "
		strResult &= "(LOL.LP >= @monatVon And LOL.LP <= @monatBis)))) And "

		strResult &= String.Format("LOL.LANR In ({0}) And ", Me.SelectedLANr)
		If Me.SelectedGAVBeruf = "Leere Felder" Then
			strResult &= "LOL.GAV_Beruf = '' And "
		Else
			strResult &= "(@beruf = '' Or LOL.GAV_Beruf = @beruf) And "
		End If
		If Me.SelectedGAVG1 = "Leere Felder" Then
			strResult &= "LOL.GAV_Gruppe1 = '' "
		Else
			strResult &= "(@gruppe1 = '' Or LOL.GAV_Gruppe1 = @gruppe1) "
		End If
		strResult &= "ORDER BY MA.Nachname ASC, MA.Vorname ASC, LOL.Jahr ASC, LOL.LP ASC, LOL.LANR ASC "

		Return (strResult)
	End Function

	Function GetSQLSelectString4KTGListe() As String
		Dim strResult As String = String.Empty
		' LISTE DER MITARBETERLOHNARTEN
		strResult = "SELECT LOL.LONr, LOL.MANR, LOL.Jahr, LOL.LP, LOL.LANR, LA.LALOText, LOL.M_Bas, LOL.M_Anz, LOL.M_Ans, LOL.M_Btr, "
		strResult &= "LOL.GAV_Kanton, "
		strResult &= "LOL.GAV_Beruf, LOL.GAV_Gruppe1, MA.Nachname, MA.Vorname, "
		strResult &= "MA.AHV_Nr, MA.AHV_Nr_New, MA.Land As MALand, MA.PLZ, MA.Ort, MA.Geschlecht, MA.Nationality, "
		strResult &= "MA.Strasse, MA.GebDat, MA.KST As MAKST, @jahrVon As VonJahr, "
		strResult &= "@monatVon As VonMonat, @jahrBis As BisJahr, @monatBis As BisMonat, LOL.GAV_Kanton, "
		strResult &= "MA.PLZ, MA.Ort, tblBVG.ESBegin, tblBVG.BVGBEgin "
		strResult &= "FROM LOL "
		strResult &= "LEFT JOIN Mitarbeiter MA ON "
		strResult &= "LOL.MANR = MA.MANR "
		strResult &= "LEFT JOIN LA ON "
		strResult &= "LA.LANR = LOL.LANR And "
		strResult &= "LA.LAJahr = LOL.Jahr "
		strResult &= "LEFT JOIN LO ON "
		strResult &= "LOL.LONR = LO.MANR And LOL.MANr = LO.MANr And LOL.LP = LO.LP And LOL.Jahr = LO.Jahr "
		strResult &= "LEFT JOIN tbl_LOBVGData tblBVG ON "
		strResult &= "LOL.LONR = tblBVG.MANR And LOL.MANr = tblBVG.MANr And LOL.LP = tblBVG.LP And LOL.Jahr = tblBVG.Jahr "

		strResult &= "WHERE LOL.MDNr = @MDNr And "

		If Me.SelectedMANr.Length > 0 Then
			strResult &= String.Format("LOL.MANR In ({0}) And ", Me.SelectedMANr)
		End If
		If Me.SelectedNullBetrag Then
			strResult &= "LOL.M_Btr <> 0 And "
		End If
		strResult &= "((LOL.Jahr = @jahrVon And LOL.LP >= @monatVon And (@jahrVon <> @jahrBis Or "
		strResult &= "(LOL.LP >= @monatVon And LOL.LP <= @monatBis))) Or "
		strResult &= "(LOL.Jahr > @jahrVon And LOL.Jahr < @jahrBis) Or "
		strResult &= "(LOL.Jahr = @jahrBis And LOL.LP <= @monatBis And (@jahrVon <> @jahrBis Or "
		strResult &= "(LOL.LP >= @monatVon And LOL.LP <= @monatBis)))) And "

		strResult &= String.Format("LOL.LANR In ({0}) And ", Me.SelectedLANr)
		If Val(Me.SelectedJahrvon) > 2012 Then
			If Not Me.IsKTG60Day Or Not Me.IsKTG720Day Then
				If Me.IsKTG60Day And Not Me.IsKTG720Day Then
					strResult &= "LOL.ZGGrund = '1' And "
				ElseIf Not Me.IsKTG60Day And Me.IsKTG720Day Then
					strResult &= "(LOL.ZGGrund = '0' Or LOL.ZGGrund = '') And "
				End If
			End If
		End If

		If Me.SelectedGAVBeruf = "Leere Felder" Then
			strResult &= "LOL.GAV_Beruf = '' And "
		Else
			strResult &= "(@beruf = '' Or LOL.GAV_Beruf = @beruf) And "
		End If
		If Me.SelectedGAVG1 = "Leere Felder" Then
			strResult &= "LOL.GAV_Gruppe1 = '' "
		Else
			strResult &= "(@gruppe1 = '' Or LOL.GAV_Gruppe1 = @gruppe1) "
		End If
		strResult &= "ORDER BY MA.Nachname ASC, MA.Vorname ASC, LOL.Jahr ASC, LOL.LP ASC, LOL.LANR ASC "

		Return (strResult)
	End Function

	Function GetLstItems(ByVal lst As ListBox) As String
		Dim strBerufItems As String = String.Empty

		For i = 0 To lst.Items.Count - 1
			strBerufItems += lst.Items(i).ToString & "#@"
		Next

		Return Left(strBerufItems, Len(strBerufItems) - 2)
	End Function

	Private Sub DropMyTableInDb()
		Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)

		Try

			conn.Open()
			' Eine bestehende Tabelle auf der Datenbank löschen
			Dim cmdCreateTable As SqlCommand = New SqlCommand(String.Format("BEGIN TRY DROP TABLE {0} END TRY BEGIN CATCH END CATCH ", _
																																			ClsDataDetail.LLTablename), _
																																			conn)
			cmdCreateTable.ExecuteNonQuery()

		Catch ex As Exception

		End Try

	End Sub

	Private Sub InsertDataTableToDatabase(ByVal TableToInsert As DataTable)
		Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
		Dim strQuery As String = String.Format("CREATE TABLE {0} (", ClsDataDetail.LLTablename)

		Try
			conn.Open()
			' Die erstellte Tabelle auf die Datenbank erzeugen
			Dim cmdCreateTable As New SqlCommand()
			cmdCreateTable.CommandText = String.Format("CREATE TABLE {0} (", ClsDataDetail.LLTablename)

			For Each col As DataColumn In TableToInsert.Columns
				If col.DataType.Name = "Decimal" Then
					cmdCreateTable.CommandText += String.Format(" {0} {1}(18,2),", col.ColumnName, col.DataType.Name)
				Else
					' Konvertierung String zu nvarchar muss die Anzahl Zeichen genau bestimmt sein.
					If col.DataType.Name = "String" Then
						Dim max As Integer = 1
						For Each row As DataRow In TableToInsert.Rows
							If Not IsDBNull(row(col.ColumnName)) Then
								If max < row(col.ColumnName).ToString.Length Then
									max = row(col.ColumnName).ToString.Length
								End If
							End If
						Next
						cmdCreateTable.CommandText += String.Format(" {0} nvarchar ({1}),", col.ColumnName, max)
					Else
						cmdCreateTable.CommandText += String.Format(" {0} {1},", col.ColumnName, col.DataType.Name)
					End If
				End If
			Next
			' letztes Komma entfernen
			cmdCreateTable.CommandText = cmdCreateTable.CommandText.Remove(cmdCreateTable.CommandText.Length - 1, 1)
			cmdCreateTable.CommandText += " )"
			cmdCreateTable.CommandText = cmdCreateTable.CommandText.Replace("Int32", "Int").Replace("Int16", "Int")
			cmdCreateTable.Connection = conn
			cmdCreateTable.ExecuteNonQuery()

			' Die erzeugte Tabelle mit der erstellten Tabelle füllen
			cmdCreateTable.CommandText = String.Format("INSERT INTO {0} VALUES (", ClsDataDetail.LLTablename)
			For Each col As DataColumn In TableToInsert.Columns
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
			' letztes Komma entfernen
			cmdCreateTable.CommandText = cmdCreateTable.CommandText.Remove(cmdCreateTable.CommandText.Length - 2, 2)
			cmdCreateTable.CommandText += ")"

			' Jeden Datensatz auf der Datenbank übertragen
			For Each rowToInsert As DataRow In TableToInsert.Rows
				' Parameter füllen
				For Each p As SqlParameter In cmdCreateTable.Parameters
					If p.SqlDbType.ToString.ToUpper = "NVARCHAR" Then
						p.Value = rowToInsert(p.ParameterName.Replace("@", "")).ToString
					Else
						p.Value = rowToInsert(p.ParameterName.Replace("@", ""))
					End If
				Next
				' Zeile schreiben
				cmdCreateTable.ExecuteNonQuery()
			Next
		Catch ex As SqlException
			Dim msg As String = ""
			For Each Err As SqlError In ex.Errors
				msg += String.Format("{0}: {1}{2}", Err.Number, Err.Message, vbLf)
			Next
			MessageBox.Show(String.Format("Die Daten konnten nicht ordnungsgemäss auf der Datenbank gespeichert werden.{1}{0}{1}", _
																		msg, vbLf), "InsertDataTableToDatabase: SQLException", _
																		MessageBoxButtons.OK, MessageBoxIcon.Error)
			Throw ex

		Catch ex As Exception
			MessageBox.Show(String.Format("Folgender Fehler kann nicht behandelt werden: {0}{1}", _
																		ex.Message, vbLf), _
																		"InsertDataTableToDatabase: Exception", _
																		MessageBoxButtons.OK, MessageBoxIcon.Error)
			Throw ex

		Finally
			conn.Close()
		End Try
	End Sub

#End Region

End Class

