
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SP.Infrastructure.UI
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPRPUmsatzTotal.ClsDataDetail
Imports SPProgUtility.ProgPath
Imports SP.Infrastructure.Logging
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Listing
Imports SP.DatabaseAccess.Listing.DataObjects
Imports SP.DatabaseAccess.Common.DataObjects

Public Class ClsDbFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Protected m_CommonDatabaseAccess As ICommonDatabaseAccess
	Private m_ListingDatabaseAccess As IListingDatabaseAccess


	Private m_md As Mandant
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As SearchCriteria
	Private m_DB1PayrollData As List(Of DB1PayrollData)
	Private m_DB1PayrollData_Staging As List(Of SP.DatabaseAccess.Listing.DataObjects.DB1CalculationData)
	Private m_CurrentKSTPayrollData_Staging As List(Of SP.DatabaseAccess.Listing.DataObjects.DB1PayrollKSTDetailData)

	Private m_DB1InvoiceData As List(Of DB1InvoiceData)
	Private m_Konti As List(Of Integer)
	Private m_AdvisorList As IEnumerable(Of AdvisorData)

	Private Property SelectedYear As Integer


#Region "Contructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		'ByVal _SearchCriteria As SearchCriteria
		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_SearchCriteria = New SearchCriteria
		m_DB1PayrollData = New List(Of DB1PayrollData)
		m_DB1PayrollData_Staging = New List(Of SP.DatabaseAccess.Listing.DataObjects.DB1CalculationData)
		m_DB1InvoiceData = New List(Of DB1InvoiceData)

		m_InitializationData = _setting


		m_CommonDatabaseAccess = New CommonDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_ListingDatabaseAccess = New ListingDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
		m_AdvisorList = m_CommonDatabaseAccess.LoadAllAdvisorsData()


	End Sub

#End Region

#Region "public properties"

	Public Property SearchCriterias As SearchCriteria
	Public Property LbKST3() As ListBox

	Public ReadOnly Property GetPayrollData() As List(Of DB1PayrollData)
		Get
			Return m_DB1PayrollData
		End Get
	End Property

	Public ReadOnly Property GetPayrollData_Staging() As List(Of SP.DatabaseAccess.Listing.DataObjects.DB1CalculationData)
		Get
			Return m_DB1PayrollData_Staging
		End Get
	End Property

	Public ReadOnly Property GetInvoiceData() As List(Of DB1InvoiceData)
		Get
			Return m_DB1InvoiceData
		End Get
	End Property

#End Region


	'''' <summary>
	'''' listet eine Auflistung der Mandantendaten
	'''' </summary>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Function LoadMandantenData() As IEnumerable(Of MandantenData)
	'	Dim m_utility As New Utilities
	'	Dim result As List(Of MandantenData) = Nothing
	'	m_md = New Mandant

	'	Dim sql As String = "[Mandanten. Get All Allowed MDData]"

	'	Dim reader = m_utility.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing, CommandType.StoredProcedure)

	'	Try

	'		If (Not reader Is Nothing) Then

	'			result = New List(Of MandantenData)

	'			While reader.Read()
	'				Dim advisorData As New MandantenData

	'				advisorData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
	'				advisorData.MDName = m_utility.SafeGetString(reader, "MDName")
	'				advisorData.MDGuid = m_utility.SafeGetString(reader, "MDGuid")
	'				advisorData.MDConnStr = m_md.GetSelectedMDData(advisorData.MDNr).MDDbConn

	'				result.Add(advisorData)

	'			End While

	'		End If

	'	Catch e As Exception
	'		result = Nothing
	'		m_Logger.LogError(e.ToString())

	'	Finally
	'		m_utility.CloseReader(reader)

	'	End Try

	'	Return result

	'End Function



	''// Get4What._strModul4What
	'Dim _LbKST3 As ListBox
	'Public Property LbKST3() As ListBox
	'	Get
	'		Return _LbKST3
	'	End Get
	'	Set(ByVal value As ListBox)
	'		_LbKST3 = value
	'	End Set
	'End Property


	Sub DeleteAllRecinUJDb_0()
		Dim sSql As String = "Select * Into #UmsatzJournal_New From UmsatzJournal_New Where ID = 0"

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand
			Try
				cmd = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)
				cmd.CommandType = Data.CommandType.Text
				cmd.ExecuteNonQuery()

			Catch ex As Exception

			End Try

			sSql = "[Get LA Info 4 Netto_2]"
			cmd = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As SqlParameter = New System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MDYear", Me.SelectedYear)
			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
			While rFoundedrec.Read
				Dim value = SafeGetInteger(rFoundedrec, "_FeierNetto_2", 0)
				ClsDataDetail.IsFeiertagAsNetto_2 = value
				ClsDataDetail.IsFerienAsNetto_2 = value
				ClsDataDetail.Is13LohnAsNetto_2 = value

			End While

#If DEBUG Then
			ClsDataDetail.IsFeiertagAsNetto_2 = True
			ClsDataDetail.IsFerienAsNetto_2 = True
			ClsDataDetail.Is13LohnAsNetto_2 = True
#End If

		Catch e As Exception
			'MsgBox(e.StackTrace, MsgBoxStyle.Critical, "DeleteAllRecinUJDb_0")

		Finally

		End Try

	End Sub

	Function GetXMargeFromMD() As Decimal
		Dim dblResult As Decimal = 0
		Dim strQuery As String = "Select MDNr, X_Marge From Mandanten Where Jahr = @MDYear"
		Dim cmd As SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, ClsDataDetail.Conn)
		Dim param As System.Data.SqlClient.SqlParameter

		param = cmd.Parameters.AddWithValue("@MDYear", m_SearchCriteria.FirstYear)
		Dim rMDrec As SqlDataReader = cmd.ExecuteReader

		' Vollständige Benutzername und Vorname
		While rMDrec.Read
			If Not String.IsNullOrEmpty(rMDrec("X_Marge").ToString) Then
				dblResult = CDec(rMDrec("X_Marge").ToString)
			End If
		End While
		rMDrec.Close()

		ClsDataDetail.GetXMarge = dblResult
		Return dblResult
	End Function

	Sub CalcDataForJournal(ByVal iMyFMonth As Integer, ByVal iMyLMonth As Integer, ByVal strMyVYear As Integer, ByVal strMyBYear As Integer)

		m_SearchCriteria = SearchCriterias
		Dim _clsSQLString As New ClsGetSQLString(m_SearchCriteria)
		Dim loiLOLBetrag As New List(Of Double)
		Dim loiOPBetrag As New List(Of Double)
		Dim bAddValue As Boolean = True

		ClsDataDetail.GetTotalTemp = 0
		ClsDataDetail.GetTotalInd = 0
		ClsDataDetail.GetTotalFest = 0
		Me.SelectedYear = m_SearchCriteria.FirstYear

		Try
			ClsDataDetail.Conn = New SqlConnection(m_InitialData.MDData.MDDbConn)
			ClsDataDetail.Conn.Open()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		End Try

		' UmsatzJournal leeren...
		DeleteAllRecinUJDb_0()

		If Not ClsDataDetail.IsFerienAsNetto_2 And
			(m_SearchCriteria.employeeNumber.GetValueOrDefault(0) + m_SearchCriteria.customerNumber.GetValueOrDefault(0) + m_SearchCriteria.esNumber.GetValueOrDefault(0) > 0) Then
			Dim msg As String = "Ihre Rückstellung für Ferien, Feiertag und 13. Lohn ist nicht Netto."
			msg &= "Daher kann keine Detailierte Liste nach Kandidaten, Kunden oder Einstäez erstellt werden. Ihre Auswahl wir ignoriert!"

			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

			m_SearchCriteria.employeeNumber = 0
			m_SearchCriteria.customerNumber = 0
			m_SearchCriteria.esNumber = 0

		End If

		Dim xMarge = GetXMargeFromMD()
		m_Konti = _clsSQLString.GetKontoNr()

		'Dim _clsEventlog As New ClsEventLog
		Dim Time_1 As Double = System.Environment.TickCount
		Dim strMyKSTBez As String = String.Empty
		Dim Time_2 As Double

		'_clsEventlog.WriteToLogFile(String.Format("Anzahl KST: {0} ", frmSource.lstKST3.Items.Count - 1), _
		Dim strMessage As String = String.Format("Anzahl KST: {0} {1}", Me.LbKST3.Items.Count - 1, vbNewLine)
		strMessage &= String.Format("Suchkriterien: {0}; Zeit: {1}-{2}/{3}-{4}; Berater: {5}-{6}{7}",
																ClsDataDetail.GetFormVars(0),
																ClsDataDetail.GetFormVars(1), ClsDataDetail.GetFormVars(2),
																ClsDataDetail.GetFormVars(3), ClsDataDetail.GetFormVars(4),
																ClsDataDetail.GetFormVars(5), ClsDataDetail.GetFormVars(6),
																vbNewLine)

		strMessage &= String.Format("Branche: {0}; Kundendaten: {1}; {2}; Kandidatendaten: {3}; {4}",
														ClsDataDetail.GetFormVars(8),
														ClsDataDetail.GetFormVars(8), ClsDataDetail.GetFormVars(9),
														ClsDataDetail.GetFormVars(10), ClsDataDetail.GetFormVars(11))

		m_Logger.LogInfo(String.Format("{0}", strMessage))

		' Nur einmal instazieren!
		Dim _clsInsertData As New ClsJournal(m_SearchCriteria)
		_clsInsertData.m_XMargeFromMD = xMarge

		For i As Integer = 0 To Me.LbKST3.Items.Count - 1
			strMyKSTBez = Me.LbKST3.Items(i).ToString()
			m_AssignedKST = strMyKSTBez

			Dim kstData = LoadAsignedAdvisorData()

			Trace.WriteLine(String.Format("zu bearbeitende KST: {0}", strMyKSTBez))
			Time_2 = System.Environment.TickCount

			Dim dbCalcData = New SP.DatabaseAccess.Listing.DataObjects.DB1CalculationData With {.KST = strMyKSTBez, .AdvisorName = kstData}
			m_CurrentKSTPayrollData_Staging = New List(Of DB1PayrollKSTDetailData)

			' Überlegen wie die Meldungen sein müssen...
			Dim strRPQuery As String = _clsSQLString.GetSQLStringFromRPDb(strMyKSTBez, iMyFMonth, iMyLMonth, strMyVYear, strMyBYear)

			' muss die Datenbank öffnen...
			If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Exit For
			Dim rRPDatarec As SqlDataReader = rExistRP4LOData(strRPQuery)
			If Not IsNothing(rRPDatarec) Then

				loiLOLBetrag = GetLOLKstData(strMyKSTBez, ClsDataDetail.Conn, iMyFMonth, iMyLMonth, strMyVYear, strMyBYear)
				If loiLOLBetrag Is Nothing Then Return

				dbCalcData.AGDetailData = m_AGdata_Staging
				dbCalcData.PayrollListData = m_CurrentKSTPayrollData_Staging

				If (Not dbCalcData.AGDetailData Is Nothing AndAlso dbCalcData.AGDetailData.Count > 0) OrElse (Not dbCalcData.PayrollListData Is Nothing AndAlso dbCalcData.PayrollListData.Count > 0) Then
					m_DB1PayrollData_Staging.Add(dbCalcData)
				End If


				' die OP Daten aussuchen, vielleicht sparrt man hier Zeit wegen der komplette Übergabe der Daten an Klasse...
				loiOPBetrag = FillMyLOiBetrag(25)

					' 25 Einträge...
					loiOPBetrag = GetOPKstData(strMyKSTBez, ClsDataDetail.Conn, iMyFMonth, iMyLMonth, strMyVYear, strMyBYear)

					For j As Integer = 0 To loiOPBetrag.Count - 1
						loiLOLBetrag(20 + j) = loiOPBetrag(j)
					Next
					Trace.WriteLine(String.Format("zu bearbeitende KST: {0} >>> loiOPBetrag(20): {1} | loiLOLBetrag(40): {2}", strMyKSTBez, loiOPBetrag(20), loiLOLBetrag(40)))

					Dim bInsertToDb As Boolean = False
					Dim dblTotalFields As Double = 0
					For j = 0 To loiLOLBetrag.Count - 1
						If loiLOLBetrag(j) <> 0 Then
							bInsertToDb = True

							Exit For
						End If
					Next

					If bInsertToDb Then
						' Lohndaten hinzufügen...
						'Dim _clsInsertData As New ClsJournal(loiLOLBetrag, rRPDatarec, strMyKSTBez, ClsDataDetail.Conn, iMyFMonth, iMyLMonth, strMyVYear, strMyBYear)

						'Dim _clsInsertData As New ClsJournal(m_SearchCriteria)
						_clsInsertData.m_aRPKstbez = strMyKSTBez.Split(CChar("/"))
						_clsInsertData.m_LstoLOLData = loiLOLBetrag
						'_clsInsertData.m_rRPDatarec = rRPDatarec
						_clsInsertData.m_cDbMyConn = ClsDataDetail.Conn

						loiLOLBetrag(14) = xMarge
						loiLOLBetrag(15) = 0 '(Val(frmSource.txtAGAnteil.Text))
						Try
							_clsInsertData.InsertDataToJournalDb()

						Catch ex As Exception
							m_Logger.LogError(String.Format("KST={0} | {1}", strMyKSTBez, ex.ToString))

							Return
						End Try
					End If

					'Console.WriteLine(String.Format("Gesamtzeit für KST {0}: {1} s", strMyKSTBez, ((System.Environment.TickCount - Time_2) / 1000).ToString()))

					Dim strMsg As String = String.Format("Daten für Kostenstelle {0}", strMyKSTBez) & vbCrLf &
																									 "Bruttolohn: " & vbTab & loiLOLBetrag(0) & vbCrLf &
																									 "AHVlohn: " & vbTab & loiLOLBetrag(1) & vbCrLf &
																									 "FerBack: " & vbTab & loiLOLBetrag(2) & vbCrLf &
																									 "FeierBack: " & vbTab & loiLOLBetrag(3) & vbCrLf &
																									 "LO13Back: " & vbTab & loiLOLBetrag(4) & vbCrLf &
																									 "GleitzeitBack: " & vbTab & loiLOLBetrag(5) & vbCrLf &
																									 "FerAus: " & vbTab & loiLOLBetrag(6) & vbCrLf &
																									 "FeierAus: " & vbTab & loiLOLBetrag(7) & vbCrLf &
																									 "LO13Aus: " & vbTab & loiLOLBetrag(8) & vbCrLf &
																									 "GleitzeitAus: " & vbTab & loiLOLBetrag(9) & vbCrLf &
																									 "TotalBack: " & vbTab & loiLOLBetrag(10) & vbCrLf &
																									 "TotalAus: " & vbTab & loiLOLBetrag(11) & vbCrLf &
																									 "FremdLohn: " & vbTab & loiLOLBetrag(12) & vbCrLf &
																									 "dAGBeitrag: " & vbTab & loiLOLBetrag(13)
				'm_Logger.LogInfo(String.Format("{0}", strMsg))

				ClsDataDetail.strAllKDNr &= String.Format("Gesamtzeit für KST {0}: {1} s", strMyKSTBez, ((System.Environment.TickCount - Time_2) / 1000).ToString()) & vbCrLf


				End If

		Next
		'Console.WriteLine(String.Format("Zeit für CalcDataForJournal: {0} s", ((System.Environment.TickCount - Time_1) / 1000).ToString()))

	End Sub

	Private Function LoadAsignedAdvisorData() As String
		Dim result As String = String.Empty
		Dim advisorList As New List(Of String)

		If m_AdvisorList Is Nothing OrElse m_AdvisorList.Count = 0 Then Return result
		For Each kst In m_AssignedKST.Split("/"c)
			If String.IsNullOrWhiteSpace(kst) Then Continue For

			Dim assignedData = m_AdvisorList.Where(Function(x) x.KST = kst).FirstOrDefault
			If Not assignedData Is Nothing Then
				advisorList.Add(assignedData.UserFullname)
			End If
		Next

		If Not advisorList Is Nothing AndAlso advisorList.Count > 0 Then
			result = String.Join(" / ", advisorList)
		End If

		Return result
	End Function

	Sub CalcDataForJournal_Test(ByVal iMyFMonth As Integer, ByVal iMyLMonth As Integer, ByVal strMyVYear As Integer, ByVal strMyBYear As Integer)

		m_SearchCriteria = SearchCriterias
		Dim _clsSQLString As New ClsGetSQLString(m_SearchCriteria)
		Dim loiLOLBetrag As New List(Of Double)
		Dim loiOPBetrag As New List(Of Double)
		Dim bAddValue As Boolean = True

		ClsDataDetail.GetTotalTemp = 0
		ClsDataDetail.GetTotalInd = 0
		ClsDataDetail.GetTotalFest = 0
		Me.SelectedYear = m_SearchCriteria.FirstYear

		Try
			ClsDataDetail.Conn = New SqlConnection(m_InitialData.MDData.MDDbConn)
			ClsDataDetail.Conn.Open()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		End Try

		' UmsatzJournal leeren...
		DeleteAllRecinUJDb_0()

		If Not ClsDataDetail.IsFerienAsNetto_2 And
			(m_SearchCriteria.employeeNumber.GetValueOrDefault(0) + m_SearchCriteria.customerNumber.GetValueOrDefault(0) + m_SearchCriteria.esNumber.GetValueOrDefault(0) > 0) Then
			Dim msg As String = "Ihre Rückstellung für Ferien, Feiertag und 13. Lohn ist nicht Netto."
			msg &= "Daher kann keine Detailierte Liste nach Kandidaten, Kunden oder Einstäez erstellt werden. Ihre Auswahl wir ignoriert!"

			m_UtilityUi.ShowErrorDialog(m_Translate.GetSafeTranslationValue(msg))

			m_SearchCriteria.employeeNumber = 0
			m_SearchCriteria.customerNumber = 0
			m_SearchCriteria.esNumber = 0

		End If

		Dim xMarge = GetXMargeFromMD()
		m_Konti = _clsSQLString.GetKontoNr()

		'Dim _clsEventlog As New ClsEventLog
		Dim Time_1 As Double = System.Environment.TickCount
		Dim strMyKSTBez As String = String.Empty
		Dim Time_2 As Double

		' Nur einmal instazieren!
		Dim _clsInsertData As New ClsJournal(m_SearchCriteria)
		_clsInsertData.m_XMargeFromMD = xMarge

		Dim data As ClsGetSQLString = Nothing
		data = New ClsGetSQLString(Nothing)
		Dim customerData As IEnumerable(Of DB1CustomerData) = data.LoadCustomerData4CustomerJournalSearch(m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth, m_SearchCriteria.FirstYear)
		Dim i As Integer = 1

		For Each currentCustomer In customerData
			m_SearchCriteria.customerNumber = currentCustomer.CustomerNumber

			Trace.WriteLine(String.Format("{0}. customer number: {1}", i, currentCustomer.CustomerNumber))
			Time_2 = System.Environment.TickCount


			' Überlegen wie die Meldungen sein müssen...
			Dim strRPQuery As String = _clsSQLString.GetSQLStringFromRPDb(strMyKSTBez, iMyFMonth, iMyLMonth, strMyVYear, strMyBYear)

			' muss die Datenbank öffnen...
			If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Exit For
			Dim rRPDatarec As SqlDataReader = rExistRP4LOData(strRPQuery)
			If Not IsNothing(rRPDatarec) Then
				loiLOLBetrag = GetLOLKstData(strMyKSTBez, ClsDataDetail.Conn, iMyFMonth, iMyLMonth, strMyVYear, strMyBYear)

				' die OP Daten aussuchen, vielleicht sparrt man hier Zeit wegen der komplette Übergabe der Daten an Klasse...
				loiOPBetrag = FillMyLOiBetrag(25)

				' 25 Einträge...
				loiOPBetrag = GetOPKstData(strMyKSTBez, ClsDataDetail.Conn, iMyFMonth, iMyLMonth, strMyVYear, strMyBYear)

				For j As Integer = 0 To loiOPBetrag.Count - 1
					loiLOLBetrag(20 + j) = loiOPBetrag(j)
				Next

				Dim bInsertToDb As Boolean = False
				Dim dblTotalFields As Double = 0
				For j = 0 To loiLOLBetrag.Count - 1
					If loiLOLBetrag(j) <> 0 Then
						bInsertToDb = True

						Exit For
					End If
				Next

				If bInsertToDb Then
					' Lohndaten hinzufügen...
					_clsInsertData.m_aRPKstbez = strMyKSTBez.Split(CChar("/"))
					_clsInsertData.m_LstoLOLData = loiLOLBetrag

					_clsInsertData.m_cDbMyConn = ClsDataDetail.Conn

					loiLOLBetrag(14) = xMarge
					loiLOLBetrag(15) = 0
					Try
						_clsInsertData.InsertCustomerDataToJournalDb()

					Catch ex As Exception
						m_Logger.LogError(String.Format("customerNumber={0} | {1}", currentCustomer.CustomerNumber, ex.ToString))

						Return
					End Try
				End If

				ClsDataDetail.strAllKDNr &= String.Format("Gesamtzeit für customernumber {0}: {1} s", currentCustomer.CustomerNumber, ((System.Environment.TickCount - Time_2) / 1000).ToString()) & vbCrLf
			End If

			i += 1
		Next
		'Console.WriteLine(String.Format("Zeit für CalcDataForJournal: {0} s", ((System.Environment.TickCount - Time_1) / 1000).ToString()))

	End Sub

	Sub CreateMySP4FilialProz()
		If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return
		Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr
		Dim strQuery As String = String.Format("BEGIN TRY DROP Procedure [dbo].[Get New FilialProzSatz From UJournal_{0}] ", _ClsProgSetting.GetLogedUSGuid) ' iSelectedUSNr)
		strQuery &= "END TRY BEGIN CATCH END CATCH"
		'strQuery &= String.Format("Drop Procedure [dbo].[Get New FilialProzSatz From UJournal_{0}] ", iSelectedUSNr)

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, ClsDataDetail.Conn)
			cmd.ExecuteNonQuery()

		Catch ex As SqlClient.SqlException

		Catch ex As Exception
			'MsgBox(ex.StackTrace, MsgBoxStyle.Critical, "CreateMySP4FilialProz_1")

		End Try

		strQuery = String.Format("Create Procedure [dbo].[Get New FilialProzSatz From UJournal_{0}] ", _ClsProgSetting.GetLogedUSGuid) ' iSelectedUSNr)
		strQuery &= "@LP smallint = 1, "
		strQuery &= "@MDYear nvarchar(4), "
		strQuery &= "@FilialBez nvarchar(50) "
		strQuery &= "As "

		strQuery &= "Declare @TotalBLohn float "
		strQuery &= "Declare @TotalFilialBLohn float "
		strQuery &= "Declare @FilialProz float "

		'strQuery &= "-- Total Bruttolohn im Monat / Jahr " & vbCrLf
		strQuery &= "Set @TotalBLohn = (Select IsNull((Select Top 1 "
		strQuery &= String.Format("Sum(UMJ_1.BruttoLohn) As TotalMontlyBLohn From [UmsatzJournal_new_{0}] UMJ_1 ", _ClsProgSetting.GetLogedUSGuid) ' iSelectedUSNr)
		strQuery &= "Where Monat = @LP And Jahr = @MDYear), 0)) "
		'strQuery &= "-- Total Bruttolohn im Monat / Jahr / Filiale " & vbCrLf
		strQuery &= "Set @TotalFilialBLohn = (Select IsNull((Select Top 1 "
		strQuery &= String.Format("Sum(UMJ_1.BruttoLohn) As TotalMontlyBLohn From [UmsatzJournal_new_{0}] UMJ_1 ", _ClsProgSetting.GetLogedUSGuid) ' iSelectedUSNr)
		strQuery &= "Where Monat = @LP And Jahr = @MDYear And USFiliale = @FilialBez), 0)) "
		strQuery &= "Set @FilialProz =(@TotalFilialBLohn / @TotalBLohn) * 100 "

		strQuery &= "Select @TotalBLohn As TotalBLohn, "
		strQuery &= "@TotalFilialBLohn As TotalFilialBLohn, "
		strQuery &= "@FilialProz As FilialProz "

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, ClsDataDetail.Conn)
			cmd.ExecuteNonQuery()


		Catch ex As Exception
			MsgBox(ex.StackTrace & vbNewLine & ex.ToString, MsgBoxStyle.Critical, "CreateMySP4FilialProz_0")

		End Try

	End Sub

	Function FillMyLOiBetrag(ByVal iCount As Integer) As List(Of Double)
		Dim loiBetrag As New List(Of Double)

		If iCount = 0 Then iCount = 30
		For i As Integer = 0 To iCount - 1
			loiBetrag.Add(0)
			loiBetrag(i) = 0
		Next

		Return loiBetrag
	End Function


#Region "Lohnbuchhaltung..."

	Function rExistRP4LOData(ByVal strQuery As String) As SqlDataReader
		If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing
		Dim cmd As SqlCommand = New SqlCommand(strQuery, ClsDataDetail.Conn)

		Return cmd.ExecuteReader
	End Function

	Function GetLOLKstData(ByVal strKst3Bez As String,
												 ByVal cConn As SqlConnection,
												 ByVal strFMonth As Integer,
												 ByVal strLMonth As Integer,
												 ByVal strVYear As Integer,
												 ByVal strBYear As Integer) As List(Of Double)

		' loiLOLBetrag(0) = Bruttolohn
		' loiLOLBetrag(1) = AHVlohn

		' loiLOLBetrag(2) = FerBack * -1
		' loiLOLBetrag(3) = FeierBack * -1
		' loiLOLBetrag(4) = LO13Back * -1
		' loiLOLBetrag(5) = GleitzeitBack 

		' loiLOLBetrag(6) = FerAus 
		' loiLOLBetrag(7) = FeierAus 
		' loiLOLBetrag(8) = LO13Aus 
		' loiLOLBetrag(9) = GleitzeitAus

		' loiLOLBetrag(10) = TotalBetragBack
		' loiLOLBetrag(11) = TotalBetragAus
		' loiLOLBetrag(12) = FremdLohn
		' loiLOLBetrag(13) = AGBeitrag
		' loiLOLBetrag(14) = xMarge aus dem Mandantenverwaltung
		' loiLOLBetrag(15) = AG-Anteil für Rück- / Auszahlung der Ferien, Feiertag und 13. Lohn

		Dim payrollListData As New List(Of SP.DatabaseAccess.Listing.DataObjects.DB1PayrollKSTDetailData)
		Dim _clsSQLString As New ClsGetSQLString(m_SearchCriteria)
		Dim loiLOLBetrag As New List(Of Double)

		Try

			' in LOL suchen...
			If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing

			Dim dAGBeitrag As Decimal = 0D
			'Dim cmd As System.Data.SqlClient.SqlCommand
			'Dim rMyLOLrec As SqlDataReader

			Try
				' B. Lohn, AHVlohn, FremdLeistungen,Rückstellungen, Auszahlungen
				loiLOLBetrag = FillMyLOiBetrag(50)

				Dim j As Integer = -1
				For i As Integer = 0 To 12

					Trace.WriteLine(String.Format("I: {0} | J: {1}", i, j))

					'If i <> 11 Or i <> 12 Then
					If j <> -11 AndAlso j <> -12 Then
						Trace.WriteLine(String.Format("Is Inside >>> I: {0} | J: {1}", i, j))

						'loiLOLBetrag(i) = GetLOLBetrag(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear, j)
						Dim dataType As New DB1DataRecordType
						Dim kstDetail As New DB1PayrollKSTDetailData

						Dim amount As Decimal = 0D

						Select Case i
							Case 0
								dataType = DB1DataRecordType.BRUTTOLOHN
							Case 1
								dataType = DB1DataRecordType.AHVLOHN
							Case 2
								dataType = DB1DataRecordType.FERIENBACKED
							Case 3
								dataType = DB1DataRecordType.FEIERBACKED
							Case 4
								dataType = DB1DataRecordType.LO13BACK
							Case 5
								dataType = DB1DataRecordType.GLEITZEITBACK
							Case 6
								dataType = DB1DataRecordType.FERIENAUS
							Case 7
								dataType = DB1DataRecordType.FEIERAUS
							Case 8
								dataType = DB1DataRecordType.LO13AUS
							Case 9
								dataType = DB1DataRecordType.GLEITZEITAUS
							Case 10
								dataType = DB1DataRecordType.TotalBetragBack
							Case 11
								dataType = DB1DataRecordType.TotalBetragAus
							Case 12
								dataType = DB1DataRecordType.FREMDLEISTUNGLOHN
							Case 13
								dataType = DB1DataRecordType.AGBEITRAG
							Case 14
								dataType = DB1DataRecordType.XMARGE
							Case 15
								dataType = DB1DataRecordType.AGANTEIL

							Case Else

						End Select
						If j <> -11 AndAlso j <> -12 Then
							'loiLOLBetrag(i) = GetLOLBetrag(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear, j)
							Dim payrollData = LoadPayrollData_Staging(strKst3Bez, j, dataType)

							For Each itm In payrollData
								amount += itm.Amount
							Next
							loiLOLBetrag(i) = amount
							If loiLOLBetrag(i) <> amount Then
								Trace.WriteLine(String.Format("Alt: {0:n5} >>> Neu: {1:n5}", loiLOLBetrag(i), amount))
							End If

							kstDetail.DataType = dataType
							kstDetail.DataDetails = payrollData
							kstDetail.DataTypeAmount = amount

							If amount <> 0 Then m_CurrentKSTPayrollData_Staging.Add(kstDetail)
							payrollListData.Add(kstDetail)
						End If

					Else
						Trace.WriteLine(String.Format("Is Inside >>> I: {0} | J: {1}", i, j))
					End If

					j -= 1
				Next

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUi.ShowErrorDialog(ex.ToString)

				Return Nothing
			End Try

			Try

				dAGBeitrag = 0D
				Dim payrollAGData = LoadPayrollDataForCalculatingAG_Staging(strKst3Bez)
				Dim kstDetail As New DB1PayrollKSTDetailData

				kstDetail.DataType = DB1DataRecordType.AGBEITRAG
				kstDetail.DataDetails = payrollAGData

				payrollListData.Add(kstDetail)


				For Each lo In payrollAGData
					dAGBeitrag += lo.Amount
				Next
				kstDetail.DataTypeAmount = dAGBeitrag

				loiLOLBetrag(13) = dAGBeitrag

				If dAGBeitrag <> 0 Then m_CurrentKSTPayrollData_Staging.Add(kstDetail)






				' Lohndatenzeile öffnen...
				'If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing
				'Dim strLOLQuery As String = _clsSQLString.GetSQLStringFromLODb(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear)

				'Dim loiLOData As New List(Of Double)
				'Dim iOldLONr As Integer = 0
				'dAGBeitrag = 0D

				'cmd = New System.Data.SqlClient.SqlCommand(strLOLQuery, ClsDataDetail.Conn)
				'rMyLOLrec = cmd.ExecuteReader

				'' die AG.-Beiträge aufadieren
				'loiLOData = FillMyLOiBetrag(3)
				'While rMyLOLrec.Read
				'	iOldLONr = CInt(Val(rMyLOLrec("LONr").ToString))
				'	dAGBeitrag += getKstLOAGAnteil(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear, iOldLONr)
				'End While

				'loiLOLBetrag(13) = (dAGBeitrag)        ' 13
				'rMyLOLrec.Close()


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUi.ShowErrorDialog(ex.ToString)

				Return Nothing
			End Try


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)

			Return Nothing
		End Try

		Return loiLOLBetrag
	End Function

	Function GetLOLBetrag(ByVal strKst3Bez As String,
												 ByVal strFMonth As Integer,
												 ByVal strLMonth As Integer,
												 ByVal strVYear As Integer,
												 ByVal strBYear As Integer,
												 ByVal i4What As Integer) As Double
		Dim _clsSQLString As New ClsGetSQLString(m_SearchCriteria)
		Dim strLOLQuery As String = String.Empty
		Dim dTotalBetrag As Double = 0

		If strKst3Bez.ToLower.Contains("rgu") Then
			'Trace.WriteLine(strKst3Bez)
		End If
		strLOLQuery = _clsSQLString.GetLOLQuery4Vars_1(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear, i4What)
		Dim cmd As New SqlCommand(strLOLQuery, ClsDataDetail.Conn)

		Try
			dTotalBetrag = CType(cmd.ExecuteScalar, Double)

		Catch ex As Exception
			m_Logger.LogError(String.Format("GetLOLBetrag: {0}", ex.ToString))
			Return 0
		End Try

		Return dTotalBetrag
	End Function


	Function getKstLOAGAnteil(ByVal strKst3Bez As String,
												 ByVal strFMonth As Integer,
												 ByVal strLMonth As Integer,
												 ByVal strVYear As Integer,
												 ByVal strBYear As Integer,
												 ByVal iLONr As Integer) As Double
		Dim _clsSQLString As New ClsGetSQLString(m_SearchCriteria)
		Dim strQuery As String = String.Empty
		Dim data As New DB1PayrollData With {.agemployeeAmount = 0, .ahvemployeeAmount = 0, .payrollNumber = iLONr, ._agAmount = 0, ._ahvAmount = 0}
		Dim dAGBeitrag As Decimal = 0
		Dim cmd As System.Data.SqlClient.SqlCommand
		Dim dTotalPflichtigLAKst As Decimal = 0
		Dim dEOKSTBetrag As Decimal = 0
		Dim LOAGData = _clsSQLString.GetSQLString4AGDb(iLONr)

		Try

			If LOAGData Is Nothing Then Return dAGBeitrag
			If LOAGData.AHVAnteil = 0 Then Return dAGBeitrag

			Dim d7100 = If(m_SearchCriteria.employeeNumber.GetValueOrDefault(0) + m_SearchCriteria.customerNumber.GetValueOrDefault(0) + m_SearchCriteria.esNumber.GetValueOrDefault(0) = 0,
										 LOAGData.AHVAnteil,
										 LOAGData.AHVAmountEachEmployee)    ' AHV-Basis
			Dim d7995 = If(m_SearchCriteria.employeeNumber.GetValueOrDefault(0) + m_SearchCriteria.customerNumber.GetValueOrDefault(0) + m_SearchCriteria.esNumber.GetValueOrDefault(0) = 0,
										LOAGData.AGAnteil,
										LOAGData.AGAmountEachEmployee) ' Arbeitgeberbeiträge gesamt

			data.agemployeeAmount = LOAGData.AGAmountEachEmployee
			data.agemployeebvgAmount = LOAGData.AGBVGAmountEachEmployee
			data.ahvemployeeAmount = LOAGData.AHVAmountEachEmployee
			data._agAmount = LOAGData.AGAnteil
			data._ahvAmount = LOAGData.AHVAnteil
			data.KST = strKst3Bez

			strQuery = _clsSQLString.GetLOLKSTQuery(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear, iLONr)
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, ClsDataDetail.Conn)
			Dim rLOLrec As SqlDataReader = cmd.ExecuteReader
			While rLOLrec.Read
				dTotalPflichtigLAKst = Val(rLOLrec("TotalBtr").ToString)
			End While
			rLOLrec.Close()
			If dTotalPflichtigLAKst = 0 Then Return dAGBeitrag

			Dim dKSTProz As Decimal = dTotalPflichtigLAKst / d7100
			Dim dSozialabzugAGKST As Decimal = d7995 * dKSTProz

			' SozialabzugAGKST ist ein negativer Betrag...
			dEOKSTBetrag = getKstLOEOAnteil(strKst3Bez, iLONr, strFMonth, strLMonth, strVYear, strBYear)
			If dEOKSTBetrag <> 0 Then
				dSozialabzugAGKST = dSozialabzugAGKST - (dEOKSTBetrag * 0.3081 * dKSTProz)
			End If
			dAGBeitrag = dSozialabzugAGKST

		Catch ex As Exception

		Finally
			m_DB1PayrollData.Add(data)

		End Try

		Return dAGBeitrag
	End Function

	''' Der Betrag für EO-Entschädigung 
	Function getKstLOEOAnteil(ByVal strKst3Bez As String,
											 ByVal iLONr As Integer,
											 ByVal strFMonth As Integer,
											 ByVal strLMonth As Integer,
											 ByVal strVYear As Integer,
											 ByVal strBYear As Integer) As Double
		Dim _clsSQLString As New ClsGetSQLString(m_SearchCriteria)
		Dim cmd As System.Data.SqlClient.SqlCommand
		Dim dEOKSTBetrag As Double = 0

		Dim strQuery As String = _clsSQLString.GetLOLEOKstQuery(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear, iLONr)
		cmd = New System.Data.SqlClient.SqlCommand(strQuery, ClsDataDetail.Conn)
		Dim rLOLrec As SqlDataReader = cmd.ExecuteReader
		While rLOLrec.Read
			dEOKSTBetrag = Val(rLOLrec("TotalBtr").ToString)
		End While
		rLOLrec.Close()

		Return dEOKSTBetrag
	End Function

#End Region


#Region "Debitorenbuchhaltung..."

	Function rExistRP4OPData(ByVal strQuery As String) As SqlDataReader
		Dim cmd As SqlCommand = New SqlCommand(strQuery, ClsDataDetail.Conn)

		Return cmd.ExecuteReader
	End Function

	Function GetOPKstData(ByVal strKst3Bez As String,
											 ByVal cConn As SqlConnection,
											 ByVal strFMonth As Integer,
											 ByVal strLMonth As Integer,
											 ByVal strVYear As Integer,
											 ByVal strBYear As Integer) As List(Of Double)
		If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing

		Dim _clsSQLString As New ClsGetSQLString(m_SearchCriteria)
		Dim loiOPBetrag As New List(Of Double)
		Dim data As DB1InvoiceData = Nothing
		Dim sharedCosts As Boolean = strKst3Bez.ToString.Contains("/")

		If m_SearchCriteria.customerNumber = 22103 Then
			Trace.WriteLine("")
		End If
		' in OP suchen...
		Dim Time_1 As Double = System.Environment.TickCount
		Dim Time_2 As Double = System.Environment.TickCount

		Dim strOPQuery As String = _clsSQLString.GetRPLOPQuery4Vars(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear)
		Dim cmd As SqlCommand = New SqlCommand(strOPQuery, cConn)
		Dim reader As SqlDataReader = cmd.ExecuteReader

		Try

			loiOPBetrag = FillMyLOiBetrag(25)

			' A. Rechnungen ...........................................................................
			While reader.Read
				data = New DB1InvoiceData
				loiOPBetrag(0) += m_utility.SafeGetDecimal(reader, "BetragTTotal", 0)       ' 0, Rapportrechnungen

				data.KST = strKst3Bez
				data.invoiceArt = "A"
				data.reportNumber = m_utility.SafeGetInteger(reader, "RPNr", 0)
				data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
				data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragTTotal", 0)

				If Not sharedCosts Then m_DB1InvoiceData.Add(data)

				If sharedCosts Then
					Dim costs = strKst3Bez.ToString.Split("/")
					For Each cost In costs
						data = New DB1InvoiceData

						data.KST = cost
						data.invoiceArt = "A"
						data.reportNumber = m_utility.SafeGetInteger(reader, "RPNr", 0)
						data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
						data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragTTotal", 0) / 2

						m_DB1InvoiceData.Add(data)
					Next
				End If



			End While
			reader.Close()

			' I. Rechnungen ...........................................................................
			If ClsDataDetail.GetFormVars(11).ToString + ClsDataDetail.GetFormVars(10).ToString = String.Empty Then
				If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing
				If (m_SearchCriteria.employeeNumber.GetValueOrDefault(0) + m_SearchCriteria.customerNumber.GetValueOrDefault(0) + m_SearchCriteria.esNumber.GetValueOrDefault(0) = 0) OrElse
						m_SearchCriteria.customerNumber.GetValueOrDefault(0) > 0 Then
					Try
						strOPQuery = _clsSQLString.GetIOPQuery4Vars(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear)
						cmd = New System.Data.SqlClient.SqlCommand(strOPQuery, ClsDataDetail.Conn)
						reader = cmd.ExecuteReader
						While reader.Read
							data = New DB1InvoiceData
							loiOPBetrag(1) += m_utility.SafeGetDecimal(reader, "BetragITotal", 0)        ' 1, Individuelle Rechnungen

							data.KST = strKst3Bez
							data.invoiceArt = "I"
							data.reportNumber = 0
							data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
							data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragITotal", 0)

							If Not sharedCosts Then m_DB1InvoiceData.Add(data)

							If sharedCosts Then
								Dim costs = strKst3Bez.ToString.Split("/")
								For Each cost In costs
									data = New DB1InvoiceData

									data.KST = cost
									data.invoiceArt = "I"
									data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
									data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragITotal", 0) / 2

									m_DB1InvoiceData.Add(data)
								Next
							End If

						End While
						reader.Close()


					Catch ex As Exception
						m_Logger.LogError(String.Format("GetIOPQuery4Vars: {0}", ex.ToString))
						m_UtilityUi.ShowErrorDialog(ex.ToString)

					End Try

					' F. Rechnungen ........................................................................
					Try
						strOPQuery = _clsSQLString.GetFOPQuery4Vars(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear)
						If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing
						cmd = New System.Data.SqlClient.SqlCommand(strOPQuery, ClsDataDetail.Conn)
						reader = cmd.ExecuteReader
						While reader.Read
							data = New DB1InvoiceData
							loiOPBetrag(2) += m_utility.SafeGetDecimal(reader, "BetragFTotal", 0)        ' 2, Festanstellungen

							data.KST = strKst3Bez
							data.invoiceArt = "F"
							data.reportNumber = 0
							data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
							data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragFTotal", 0)

							If Not sharedCosts Then m_DB1InvoiceData.Add(data)

							If sharedCosts Then
								Dim costs = strKst3Bez.ToString.Split("/")
								For Each cost In costs
									data = New DB1InvoiceData

									data.KST = cost
									data.invoiceArt = "F"
									data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
									data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragFTotal", 0) / 2

									m_DB1InvoiceData.Add(data)
								Next
							End If

						End While
						reader.Close()


					Catch ex As Exception
						m_Logger.LogError(String.Format("GetFOPQuery4Vars: {0}", ex.ToString))
						m_UtilityUi.ShowErrorDialog(ex.ToString)
					End Try


					' Gutschriften automatische ............................................................................
					Try
						strOPQuery = _clsSQLString.GetGOPQuery4Vars(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear)
						If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing
						cmd = New System.Data.SqlClient.SqlCommand(strOPQuery, ClsDataDetail.Conn)
						reader = cmd.ExecuteReader
						While reader.Read
							data = New DB1InvoiceData
							loiOPBetrag(3) += -1 * m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)        ' 3, Gutschriften

							data.KST = strKst3Bez
							data.invoiceArt = "G"
							data.reportNumber = 0
							data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
							data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)

							If Not sharedCosts Then m_DB1InvoiceData.Add(data)

							If sharedCosts Then
								Dim costs = strKst3Bez.ToString.Split("/")
								For Each cost In costs
									data = New DB1InvoiceData

									data.KST = cost
									data.invoiceArt = "G"
									data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
									data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0) / 2

									m_DB1InvoiceData.Add(data)
								Next
							End If

						End While
						reader.Close()
					Catch ex As Exception
						m_Logger.LogError(String.Format("GetGOPQuery4Vars: {0}", ex.ToString))
						m_UtilityUi.ShowErrorDialog(ex.ToString)
					End Try

					' SKonto .........................................................................
					Try
						Dim loiOPSK As List(Of Double) = GetSKonto_Erloes(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear)

						If strKst3Bez.ToUpper.Contains("mse".ToUpper) Then
							'Trace.WriteLine(loiOPSK.Item(7))
						End If

						For i As Integer = 0 To loiOPSK.Count - 1
							loiOPBetrag(5 + i) = loiOPSK(i)        ' SKonto, Erlöse, Diverse Verlüste, Verlust, Rückvergütung, Gutschrift
						Next


					Catch ex As Exception
						m_Logger.LogError(String.Format("GetSKonto_Erloes: {0}", ex.ToString))
						m_UtilityUi.ShowErrorDialog(ex.ToString)
					End Try

				End If
				loiOPBetrag(18) = loiOPBetrag(3)        ' 18, Gutschriften automatische Rechnungen (KDGuA)

				' Gutschriften individuelle ............................................................................
				Try
					If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing
					strOPQuery = _clsSQLString.GetGOPQuery4Vars(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear, "G", "I")
					cmd = New System.Data.SqlClient.SqlCommand(strOPQuery, ClsDataDetail.Conn)
					reader = cmd.ExecuteReader
					While reader.Read
						data = New DB1InvoiceData
						loiOPBetrag(19) += -1 * m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)        ' 19, Gutschriften I(KDGuInd)

						data.KST = strKst3Bez
						data.invoiceArt = "G-I"
						data.reportNumber = 0
						data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
						data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)

						If Not sharedCosts Then m_DB1InvoiceData.Add(data)

						If sharedCosts Then
							Dim costs = strKst3Bez.ToString.Split("/")
							For Each cost In costs
								data = New DB1InvoiceData

								data.KST = cost
								data.invoiceArt = "G-I"
								data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
								data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0) / 2

								m_DB1InvoiceData.Add(data)
							Next
						End If

					End While
					reader.Close()

				Catch ex As Exception
					m_Logger.LogError(String.Format("GetGOPQuery4Vars (GI): {0}", ex.ToString))
					m_UtilityUi.ShowErrorDialog(ex.ToString)
				End Try

				' Gutschriften Festanstellung ............................................................................
				Try
					If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing
					strOPQuery = _clsSQLString.GetGOPQuery4Vars(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear, "G", "F")
					cmd = New System.Data.SqlClient.SqlCommand(strOPQuery, ClsDataDetail.Conn)
					reader = cmd.ExecuteReader
					While reader.Read
						data = New DB1InvoiceData
						loiOPBetrag(20) += -1 * m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)        ' 20, Gutschriften F (KDGuF)

						data.KST = strKst3Bez
						data.invoiceArt = "G-F"
						data.reportNumber = 0
						data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
						data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)

						If Not sharedCosts Then m_DB1InvoiceData.Add(data)

						If sharedCosts Then
							Dim costs = strKst3Bez.ToString.Split("/")
							For Each cost In costs
								data = New DB1InvoiceData

								data.KST = cost
								data.invoiceArt = "G-F"
								data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
								data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0) / 2

								m_DB1InvoiceData.Add(data)
							Next
						End If

					End While
					reader.Close()
				Catch ex As Exception
					m_Logger.LogError(String.Format("GetGOPQuery4Vars (GF): {0}", ex.ToString))
					m_UtilityUi.ShowErrorDialog(ex.ToString)
				End Try


				' Rückvergütung automatische ............................................................................
				Try
					If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing
					strOPQuery = _clsSQLString.GetGOPQuery4Vars(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear, "R", "A")
					cmd = New System.Data.SqlClient.SqlCommand(strOPQuery, ClsDataDetail.Conn)
					reader = cmd.ExecuteReader
					While reader.Read
						data = New DB1InvoiceData
						loiOPBetrag(15) += (-1 * (m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)))         ' -15, Rückvergütung A (KDRuA)

						data.KST = strKst3Bez
						data.invoiceArt = "R-A"
						data.reportNumber = 0
						data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
						data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)

						If Not sharedCosts Then m_DB1InvoiceData.Add(data)

						If sharedCosts Then
							Dim costs = strKst3Bez.ToString.Split("/")
							For Each cost In costs
								data = New DB1InvoiceData

								data.KST = cost
								data.invoiceArt = "R-A"
								data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
								data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0) / 2

								m_DB1InvoiceData.Add(data)
							Next
						End If

					End While
					reader.Close()

				Catch ex As Exception
					m_Logger.LogError(String.Format("GetGOPQuery4Vars (RA): {0}", ex.ToString))
					m_UtilityUi.ShowErrorDialog(ex.ToString)
				End Try

				' Rückvergütung Ind ............................................................................
				Try
					If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing
					strOPQuery = _clsSQLString.GetGOPQuery4Vars(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear, "R", "I")
					cmd = New System.Data.SqlClient.SqlCommand(strOPQuery, ClsDataDetail.Conn)
					reader = cmd.ExecuteReader
					While reader.Read
						data = New DB1InvoiceData
						loiOPBetrag(16) += (-1 * (m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)))        ' 16, Rückvergütung Ind (KDRuInd)

						data.KST = strKst3Bez
						data.invoiceArt = "R-I"
						data.reportNumber = 0
						data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
						data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)

						If Not sharedCosts Then m_DB1InvoiceData.Add(data)

						If sharedCosts Then
							Dim costs = strKst3Bez.ToString.Split("/")
							For Each cost In costs
								data = New DB1InvoiceData

								data.KST = cost
								data.invoiceArt = "R-I"
								data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
								data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0) / 2

								m_DB1InvoiceData.Add(data)
							Next
						End If

					End While
					reader.Close()

				Catch ex As Exception
					m_Logger.LogError(String.Format("GetGOPQuery4Vars (RI): {0}", ex.ToString))
					m_UtilityUi.ShowErrorDialog(ex.ToString)
				End Try

				' Rückvergütung Fest ............................................................................
				Try
					If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing
					strOPQuery = _clsSQLString.GetGOPQuery4Vars(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear, "R", "F")
					cmd = New System.Data.SqlClient.SqlCommand(strOPQuery, ClsDataDetail.Conn)
					reader = cmd.ExecuteReader
					While reader.Read
						data = New DB1InvoiceData
						loiOPBetrag(17) += (-1 * (m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)))        ' 17, Rückvergütung F (KDRuF)

						data.KST = strKst3Bez
						data.invoiceArt = "R-F"
						data.reportNumber = 0
						data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
						data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0)

						If Not sharedCosts Then m_DB1InvoiceData.Add(data)

						If sharedCosts Then
							Dim costs = strKst3Bez.ToString.Split("/")
							For Each cost In costs
								data = New DB1InvoiceData

								data.KST = cost
								data.invoiceArt = "R-F"
								data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
								data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragGTotal", 0) / 2

								m_DB1InvoiceData.Add(data)
							Next
						End If

					End While
					reader.Close()
				Catch ex As Exception
					m_Logger.LogError(String.Format("GetGOPQuery4Vars (RF): {0}", ex.ToString))
					m_UtilityUi.ShowErrorDialog(ex.ToString)
				End Try

			End If


			' Fremdrechnungen .........................................................................
			Try
				If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing
				strOPQuery = _clsSQLString.GetFremdOPQuery4Vars(strKst3Bez, strFMonth, strLMonth, strVYear, strBYear)
				cmd = New System.Data.SqlClient.SqlCommand(strOPQuery, ClsDataDetail.Conn)
				reader = cmd.ExecuteReader
				While reader.Read
					data = New DB1InvoiceData
					loiOPBetrag(4) += m_utility.SafeGetDecimal(reader, "BetragFopTotal", 0) '(CDbl(Val(reader("BetragFopTotal").ToString)))				' 4, Fremdrechnungen

					data.KST = strKst3Bez
					data.invoiceArt = "Fremd-Rechnungen"
					data.reportNumber = 0
					data.invoicenNumber = m_utility.SafeGetInteger(reader, "FOpNr", 0)
					data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragFopTotal", 0)

					If Not sharedCosts Then m_DB1InvoiceData.Add(data)

					If sharedCosts Then
						Dim costs = strKst3Bez.ToString.Split("/")
						For Each cost In costs
							data = New DB1InvoiceData

							data.KST = cost
							data.invoiceArt = "Fremd-Rechnungen"
							data.invoicenNumber = m_utility.SafeGetInteger(reader, "RENr", 0)
							data.invoiceAmount = m_utility.SafeGetDecimal(reader, "BetragFopTotal", 0) / 2

							m_DB1InvoiceData.Add(data)
						Next
					End If

				End While
				reader.Close()

			Catch ex As Exception
				m_Logger.LogError(String.Format("GetFremdOPQuery4Vars: {0}", ex.ToString))
				m_UtilityUi.ShowErrorDialog(ex.ToString)
			End Try

			Time_2 = System.Environment.TickCount
			'Console.WriteLine("Zeit für GetOPKstData (1): (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
			m_UtilityUi.ShowErrorDialog(ex.ToString)
		End Try


		Return loiOPBetrag
	End Function

	Function GetSKonto_Erloes(ByVal strSelectedKst3Bez As String,
														ByVal strFMonth As Integer,
														ByVal strLMonth As Integer,
														ByVal strVYear As Integer,
														ByVal strBYear As Integer) As List(Of Double)
		Dim loiSEOPBetrag As New List(Of Double)
		'Dim _ClsReg As New SPProgUtility.ClsDivReg

		Dim _ClsSQLString As New ClsGetSQLString(m_SearchCriteria)
		Dim ssql As String = _ClsSQLString.GetSKontoQuery4Vars(strSelectedKst3Bez, strFMonth, strLMonth, strVYear, strBYear)
		Dim liKontoNr As List(Of Integer) = m_Konti ' _ClsSQLString.GetKontoNr()
		Dim data As DB1InvoiceData = Nothing

		If strSelectedKst3Bez.ToUpper.Contains("mse".ToUpper) Then
			Trace.WriteLine(ssql)
		End If
		If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return Nothing

		Using Conn As New SqlConnection(m_InitialData.MDData.MDDbConn)
			Dim command As New SqlCommand(ssql, Conn)
			Dim dZEBetrag As Double = 0

			Dim dZESKAOp As Double = 0
			Dim dZESKIOp As Double = 0
			Dim dZESKFOp As Double = 0

			Dim dZEErAOp As Double = 0
			Dim dZEErIOp As Double = 0
			Dim dZEErFOp As Double = 0
			Dim dZESonstigeOp As Double = 0

			Dim dZEVerAOp As Double = 0
			Dim dZEVerIOp As Double = 0
			Dim dZEVerFOp As Double = 0

			Dim dZERuAOp As Double = 0
			Dim dZERuIOp As Double = 0
			Dim dZERuFOp As Double = 0

			Dim dZEGuAOp As Double = 0
			Dim dZEGuIOp As Double = 0
			Dim dZEGuFOp As Double = 0

			'Dim iVerAOPMwSt As Integer		13
			'Dim iVerIOPMwSt As Integer		14
			'Dim iVerFOPMwSt As Integer		15
			'Dim iVerAOP As Integer		16
			'Dim iVerIOP As Integer		17
			'Dim iVerFOP As Integer		18

			'Dim iRuAOPMwSt As Integer		19
			'Dim iRuIOPMwSt As Integer		20
			'Dim iRuFOPMwSt As Integer		21
			'Dim iRuAOP As Integer		22
			'Dim iRuIOP As Integer		23
			'Dim iRuFOP As Integer		24

			'Dim iGuAOPMwSt As Integer		25
			'Dim iGuIOPMwSt As Integer		26
			'Dim iGuFOPMwSt As Integer		27
			'Dim iGuAOP As Integer		28
			'Dim iGuIOP As Integer		29
			'Dim iGuFOP As Integer		30

			'0  strResult.Add(iSKAOPMwSt)
			'1  strResult.Add(iSKIOPMwSt)
			'2  strResult.Add(iSKFOPMwSt)
			'3  strResult.Add(iSKAOP)
			'4  strResult.Add(iSKIOP)
			'5  strResult.Add(iSKFOP)

			'6  strResult.Add(iErAOPMwSt)
			'7  strResult.Add(iErIOPMwSt)
			'8  strResult.Add(iErFOPMwSt)
			'9  strResult.Add(iErAOP)
			'10 strResult.Add(iErIOP)
			'11 strResult.Add(iErFOP)

			'12 strResult.Add(CInt(strSOPVerluste))

			'13 strResult.Add(CInt(dZEVerAOp))
			'14 strResult.Add(CInt(dZEVerIOp))
			'15 strResult.Add(CInt(dZEVerFOp))

			'16 strResult.Add(CInt(dZERuAOp))
			'17 strResult.Add(CInt(dZERuIOp))
			'18 strResult.Add(CInt(dZERuFOp))

			'19 strResult.Add(CInt(dZEGuAOp))
			'20 strResult.Add(CInt(dZEGuIOp))
			'21 strResult.Add(CInt(dZEGuFOp))

			Conn.Open()
			Dim rZErec As SqlDataReader = command.ExecuteReader()

			Try

				' Call Read before accessing data.
				While rZErec.Read()
					data = New DB1InvoiceData

					If CDbl(rZErec("MwStProz").ToString) > 0 Then
						'dZEBetrag /= (Val(rZErec("MwStProz").ToString) + 100) / 100

						'dZEBetrag = Val(Val(rZErec("Betrag").ToString) - _
						'(Val(rZErec("Betrag").ToString)) * _
						'((100 + Val(rZErec("MwStProz").ToString)) / 100))

						dZEBetrag = Val(rZErec("Betrag").ToString) / ((100 + Val(rZErec("MwStProz").ToString)) / 100)

					Else
						dZEBetrag = Val(rZErec("Betrag").ToString)

					End If

					Select Case CInt(rZErec("FkSoll").ToString)
						' SKonto ..............................................................
						Case liKontoNr(0), liKontoNr(3) ' iSKAOPMwSt, iSKAOP
							dZESKAOp += dZEBetrag

						Case liKontoNr(1), liKontoNr(4)  'iSKIOPMwSt, iSKIOP
							dZESKIOp += dZEBetrag

						Case liKontoNr(2), liKontoNr(5)  'iSKFOPMwSt, iSKFOP
							dZESKFOp += dZEBetrag

							' Erlöse ..............................................................
						Case liKontoNr(6), liKontoNr(9)  'iErAOPMwSt, iErAOP
							dZEErAOp += dZEBetrag

						Case liKontoNr(7), liKontoNr(10)  'iErIOPMwSt, iErIOP
							dZEErIOp += dZEBetrag

						Case liKontoNr(8), liKontoNr(11)  'iErFOPMwSt, iErFOP
							dZEErFOp += dZEBetrag


							' Verlust ..............................................................
						Case liKontoNr(13), liKontoNr(16)  'iverAOPMwSt, iverAOP
							dZEVerAOp += dZEBetrag

						Case liKontoNr(14), liKontoNr(17)  'iverIOPMwSt, iverIOP
							dZEVerIOp += dZEBetrag

						Case liKontoNr(15), liKontoNr(18)  'iverFOPMwSt, iverFOP
							dZEVerFOp += dZEBetrag

							' Rückvergügung ..............................................................
						Case liKontoNr(19), liKontoNr(22)  'iRuAOPMwSt, iRuAOP
							dZERuAOp += dZEBetrag

						Case liKontoNr(20), liKontoNr(23)  'iRuIOPMwSt, iRuIOP
							dZERuIOp += dZEBetrag

						Case liKontoNr(21), liKontoNr(24)  'iRuFOPMwSt, iRuFOP
							dZERuFOp += dZEBetrag

							' Gutschrift ..............................................................
						Case liKontoNr(25), liKontoNr(28)  'iGuAOPMwSt, iGuAOP
							dZEGuAOp += dZEBetrag

						Case liKontoNr(26), liKontoNr(29)  'iGuIOPMwSt, iGuIOP
							dZEGuIOp += dZEBetrag

						Case liKontoNr(27), liKontoNr(30)  'iGuFOPMwSt, iGuFOP
							dZEGuFOp += dZEBetrag


						Case Else
							dZESonstigeOp += dZEBetrag

					End Select

					data.KST = strSelectedKst3Bez
					data.invoiceArt = String.Format("ZENr: {0} | OPNr: {1} - SOLL-Konto: {2}", m_utility.SafeGetInteger(rZErec, "ZENr", 0), m_utility.SafeGetInteger(rZErec, "RENr", 0), m_utility.SafeGetInteger(rZErec, "FkSoll", 0))
					data.reportNumber = 0
					data.invoicenNumber = m_utility.SafeGetInteger(rZErec, "RENr", 0)
					data.invoiceAmount = dZEBetrag

					m_DB1InvoiceData.Add(data)

					'Console.WriteLine(String.Format("GetSKonto_Erloes: {0} / {1}", rZErec(0), rZErec(1)))
				End While

			Catch ex As Exception
				m_Logger.LogError(String.Format("GetSKonto_Erloes: {0}", ex.ToString))
				m_UtilityUi.ShowErrorDialog(ex.ToString)

			End Try

			' Alte Version
			'loiSEOPBetrag.Add(dZESKAOp)   ' 5
			'loiSEOPBetrag.Add(dZESKIOp + dZESKFOp)   ' 6
			'loiSEOPBetrag.Add(dZEErAOp)   ' 8
			'loiSEOPBetrag.Add(dZEErIOp + dZEErFOp)   ' 9
			'loiSEOPBetrag.Add(dZESonstigeOp)   ' 11
			' Ende der Alte Version

			loiSEOPBetrag.Add(dZESKAOp)       ' 0
			loiSEOPBetrag.Add(dZESKIOp)       ' 1
			loiSEOPBetrag.Add(dZESKFOp)       ' 2

			loiSEOPBetrag.Add(dZEErAOp)       ' 3
			loiSEOPBetrag.Add(dZEErIOp)       ' 4
			loiSEOPBetrag.Add(dZEErFOp)       ' 5

			loiSEOPBetrag.Add(dZESonstigeOp)  ' 6

			loiSEOPBetrag.Add(dZEVerAOp)      ' 7
			loiSEOPBetrag.Add(dZEVerIOp)      ' 8
			loiSEOPBetrag.Add(dZEVerFOp)      ' 9

			loiSEOPBetrag.Add(dZERuAOp)     ' 10
			loiSEOPBetrag.Add(dZERuIOp)     ' 11
			loiSEOPBetrag.Add(dZERuFOp)     ' 12

			loiSEOPBetrag.Add(dZEGuAOp)     ' 13
			loiSEOPBetrag.Add(dZEGuIOp)     ' 14
			loiSEOPBetrag.Add(dZEGuFOp)     ' 15


			If dZESonstigeOp <> 0 Then
				'MsgBox(dZESonstigeOp)
			End If

			' Call Close when done reading.
			rZErec.Close()

		End Using


		Return loiSEOPBetrag
	End Function

#End Region


	'Sub GetJobNr4Print(ByVal sListArt As Short)
	'  Dim strModul2print As String = String.Empty

	'strModul2print = "3.5." & CStr(sListArt)

	'  ClsDataDetail.GetModulToPrint = strModul2print
	'End Sub

End Class
