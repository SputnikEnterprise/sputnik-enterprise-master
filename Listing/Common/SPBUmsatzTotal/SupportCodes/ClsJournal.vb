
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Utility

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPRPUmsatzTotal.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class ClsJournal

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _LstoExistJData As New List(Of Double)
	Private _strKstName As String = String.Empty
	Private _strKstFilale As String = String.Empty

	Private m_md As Mandant
	Private m_common As CommonSetting
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As New SearchCriteria

	Public Property m_aRPKstbez As String()
	Public Property m_LstoLOLData As New List(Of Double)
	Public Property m_cDbMyConn As SqlClient.SqlConnection
	Public Property m_XMargeFromMD As Decimal

	Sub New(ByVal _SearchCriteria As SearchCriteria)

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_SearchCriteria = _SearchCriteria

	End Sub


#Region "Properties zur Speicherung der Variable..."

	ReadOnly Property GetLOData() As List(Of Double)

		Get
			Dim lstoMyData As New List(Of Double)

			For i As Integer = 0 To m_LstoLOLData.Count - 1
				lstoMyData.Add(CDbl(IIf(Me.GetKstBez.Count > 1, m_LstoLOLData(i) / 2, m_LstoLOLData(i))))
			Next
			Return lstoMyData
		End Get
	End Property

	ReadOnly Property GetExistJDbValue() As List(Of Double)
		Get
			Return _LstoExistJData
		End Get
	End Property

	ReadOnly Property GetDbConn() As SqlConnection
		Get
			Return m_cDbMyConn
		End Get
	End Property

	'ReadOnly Property GetRPDb() As SqlDataReader
	'	Get
	'		Return m_rRPDatarec
	'	End Get
	'End Property

	ReadOnly Property GetKstBez() As String()
		Get
			Return m_aRPKstbez
		End Get
	End Property

	'ReadOnly Property GetiFMonth() As Integer
	'	Get
	'		Return _iFMonth
	'	End Get
	'End Property

	'ReadOnly Property GetiLMonth() As Integer
	'	Get
	'		Return _iLMonth
	'	End Get
	'End Property

	'ReadOnly Property GetstrYear() As Integer
	'	Get
	'		Return _strVYear
	'	End Get
	'End Property

#End Region


#Region "methodes for KST"

	Sub InsertDataToJournalDb()
		Dim lsoMyData As New List(Of Double)
		Dim bKstTeilung As Boolean = GetKstBez.Count > 1
		Dim aKstbez As String() = GetKstBez()

		lsoMyData = Me.GetLOData()
		Dim strSQLQuery As String = String.Empty
		Dim strSelectedKstBez As String = String.Empty


		For i As Integer = 0 To aKstbez.Count - 1
			strSelectedKstBez = aKstbez(i)

			' Vollständige Kst-Daten von US-Db
			Dim strKstDataFromUSDb As String = GetKSTName(strSelectedKstBez)

			_strKstName = strKstDataFromUSDb.ToString.Split(CChar("@"))(0)
			If strKstDataFromUSDb.Contains("@") Then
				_strKstFilale = strKstDataFromUSDb.ToString.Split(CChar("@"))(1)
			Else
				_strKstFilale = String.Empty
			End If
			If Not ExistKstInJDb(strSelectedKstBez) Then
				InsertMyNewDataToJDb(strSelectedKstBez, _strKstName, _strKstFilale)

			Else
				UpdateMyDataInJDb(strSelectedKstBez)

			End If
		Next

	End Sub

	Function ExistKstInJDb(ByVal strMyKst As String) As Boolean
		Dim bResult As Boolean
		Try
			Dim Sql = String.Format("Select * From #UmsatzJournal_New Where Kst3_1 = '{0}' ", strMyKst)
			Sql &= String.Format("And UserNr = {0}", m_InitialData.UserData.UserNr)

			Dim cmd As SqlClient.SqlCommand = New SqlCommand(Sql, GetDbConn)
			Dim reader As SqlDataReader = cmd.ExecuteReader

			' habe ich es eingefügt? existiert der Datensatz von mir?
			bResult = reader.HasRows

		Catch ex As Exception

		End Try

		Return bResult
	End Function

#End Region


#Region "methodes for KST"

	Public Function InsertCustomerDataToJournalDb() As Boolean
		Dim success As Boolean = True

		Dim lsoMyData As New List(Of Double)

		lsoMyData = Me.GetLOData()
		Dim strSQLQuery As String = String.Empty
		Dim strSelectedKstBez As String = String.Empty

		If Not ExistCustomerInJDb(m_SearchCriteria.customerNumber) Then
			InsertMyNewDataToJDb(strSelectedKstBez, _strKstName, _strKstFilale)
		Else
			UpdateJournalWithCustomerNumber(m_SearchCriteria.customerNumber)
		End If

	End Function

	Private Function ExistCustomerInJDb(ByVal customerNumber As Integer) As Boolean
		Dim bResult As Boolean
		Try
			Dim Sql = String.Format("Select customerNumber From #UmsatzJournal_New Where CustomerNumber = '{0}' ", customerNumber)
			Sql &= String.Format("And UserNr = {0}", m_InitialData.UserData.UserNr)

			Dim cmd As SqlClient.SqlCommand = New SqlCommand(Sql, GetDbConn)
			Dim reader As SqlDataReader = cmd.ExecuteReader

			' habe ich es eingefügt? existiert der Datensatz von mir?
			bResult = reader.HasRows

		Catch ex As Exception

		End Try

		Return bResult
	End Function

#End Region

	Sub InsertMyNewDataToJDb(ByVal strMyKst As String,
												ByVal strMyFullKSTName As String,
												ByVal strMyFiliale As String)
		Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr

		Dim strQuery As String = "Insert Into #UmsatzJournal_New (Kst3_1, KST3Bez, USFiliale, USFilialeNr, "
		strQuery &= "Bruttolohn, AHVLohn, AGBetrag, _XMarge, Fremdleistung, "
		strQuery &= "FerBack, FeierBack, LO13Back, TimeBack, "
		strQuery &= "FerAus, FeierAus, LO13Aus, TimeAus, BackAus, "

		strQuery &= "KDTotal, KDTotalInd, KDTotalFest, "
		strQuery &= "KDTotalG, KDTotalFOp, "
		strQuery &= "KDTotalTempSKonto, KDTotalIndSKonto, KDTotalFSKonto, "
		strQuery &= "KDTotalTempErlos, KDTotalIndErlos, KDTotalFErlos, KDSVerluste, "

		strQuery &= "KDVerlustA, KDVerlustInd, KDVerlustF, "
		strQuery &= "KDRuA, KDRuInd, KDRuF, "
		strQuery &= "KDGuA, KDGuInd, KDGuF, "

		strQuery &= "UserNr, Monat, Jahr, CreatedOn, CreatedFrom, "

		strQuery &= "[Adminkosten], [AGProz], [AGBetrag_2], [TotalFilialUmsatz], [TotalFilialTUmsatz], [TotalFilialLohn], "
		strQuery &= "[U_TempUmsatz], [U_IndUmsatz], [U_FestUmsatz], [F_TempUmsatz], [F_IndUmsatz], [F_FestUmsatz], [_TempUmsatz], "
		strQuery &= "[_IndUmsatz], [_FestUmsatz], [_Lohnaufwand_1], [_Lohnaufwand_2], "
		strQuery &= "[F_Lohnaufwand_1], [F_Lohnaufwand_2], [U_Lohnaufwand_1], [U_Lohnaufwand_2], [_Marge], "
		strQuery &= "[_PAnteil_F_BGT], [_PAnteil_F_BGI], [_PAnteil_F_BGF], "
		strQuery &= "[_PAnteil_U_BGT], [_PAnteil_U_BGI], [_PAnteil_U_BGF], "
		strQuery &= "[_BGTemp], [_BGInd], [_BGFest], [F_BGTemp], [F_BGInd], [F_BGFest], [U_BGTemp], [U_BGInd], [U_BGFest] "

		strQuery &= ",CustomerNumber, EmployeeNumber, EmploymentNumber "
		strQuery &= ") Values ("

		strQuery &= "@strMyKst, @strMyFullKSTName, @strMyFiliale, @USFilialeNr, "
		strQuery &= "@dblBruttolohn, @dblAHVLohn, @dblAGBeitrag, @dblXMarge, @dblFremdLeistung, "
		strQuery &= "@dblFerBack, @dblFeierBack, @dbl13Back, @dblTimeBack, "
		strQuery &= "@dblFerAus, @dblFeierAus, @dbl13Aus, @dblTimeAus, @dblBackAus, "

		strQuery &= "@KDTotal, @KDTotalInd, @KDTotalFest, "
		strQuery &= "@KDTotalG, @KDTotalFOp, @KDTotalTempSKonto, @KDTotalIndSKonto, @KDTotalFSKonto, "
		strQuery &= "@KDTotalTempErlos, @KDTotalIndErlos, @KDTotalFErlos, @KDSVerluste, "

		strQuery &= "@KDVerlustA, @KDVerlustInd, @KDVerlustF, "
		strQuery &= "@KDRuA, @KDRuInd, @KDRuF, "
		strQuery &= "@KDGuA, @KDGuInd, @KDGuF, "

		strQuery &= "@lUSNr, @iMonthVonBis, @strYear, @dNow, @UsFullName, "
		strQuery &= "0,0,0,0,0,0,0,0,0,0,"
		strQuery &= "0,0,0,0,0,0,0,0,0,0,"
		strQuery &= "0,0,0,0,0,0,0,0,0,0,"
		strQuery &= "0,0,0,0,0,0,0"

		strQuery &= ",@CustomerNumber, @EmployeeNumber, @EmploymentNumber "
		strQuery &= ")"

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, GetDbConn)
		Dim param As System.Data.SqlClient.SqlParameter

		param = cmd.Parameters.AddWithValue("@strMyKst", strMyKst)
		param = cmd.Parameters.AddWithValue("@strMyFullKSTName", strMyFullKSTName)
		param = cmd.Parameters.AddWithValue("@strMyFiliale", strMyFiliale)
		param = cmd.Parameters.AddWithValue("@USFilialeNr", 1)

		param = cmd.Parameters.AddWithValue("@dblBruttolohn", Me.GetLOData.Item(0))
		param = cmd.Parameters.AddWithValue("@dblAHVLohn", Me.GetLOData.Item(1))
		param = cmd.Parameters.AddWithValue("@dblAGBeitrag", Me.GetLOData.Item(13))
		param = cmd.Parameters.AddWithValue("@dblXMarge", m_XMargeFromMD)
		param = cmd.Parameters.AddWithValue("@dblFremdLeistung", Me.GetLOData.Item(12))

		param = cmd.Parameters.AddWithValue("@dblFerBack", Me.GetLOData.Item(2))
		param = cmd.Parameters.AddWithValue("@dblFeierBack", Me.GetLOData.Item(3))
		param = cmd.Parameters.AddWithValue("@dbl13Back", Me.GetLOData.Item(4))
		param = cmd.Parameters.AddWithValue("@dblTimeBack", Me.GetLOData.Item(5))

		param = cmd.Parameters.AddWithValue("@dblFerAus", Me.GetLOData.Item(6))
		param = cmd.Parameters.AddWithValue("@dblFeierAus", Me.GetLOData.Item(7))
		param = cmd.Parameters.AddWithValue("@dbl13Aus", Me.GetLOData.Item(8))
		param = cmd.Parameters.AddWithValue("@dblTimeAus", Me.GetLOData.Item(9))

		param = cmd.Parameters.AddWithValue("@dblBackAus",
																				(Val(Me.GetLOData.Item(2)) - Val(Me.GetLOData.Item(3)) -
																				 Val(Me.GetLOData.Item(4)) - Val(Me.GetLOData.Item(5))) +
																				 (Val(Me.GetLOData.Item(6)) + Val(Me.GetLOData.Item(7)) +
																					Val(Me.GetLOData.Item(8)) + Val(Me.GetLOData.Item(9))))

		param = cmd.Parameters.AddWithValue("@KDTotal", Me.GetLOData.Item(20))
		param = cmd.Parameters.AddWithValue("@KDTotalInd", Me.GetLOData.Item(21))
		param = cmd.Parameters.AddWithValue("@KDTotalFest", Me.GetLOData.Item(22))
		param = cmd.Parameters.AddWithValue("@KDTotalG", Me.GetLOData.Item(23))
		param = cmd.Parameters.AddWithValue("@KDTotalFOp", Me.GetLOData.Item(24))

		param = cmd.Parameters.AddWithValue("@KDTotalTempSKonto", Me.GetLOData.Item(25))
		param = cmd.Parameters.AddWithValue("@KDTotalIndSKonto", Me.GetLOData.Item(26))
		param = cmd.Parameters.AddWithValue("@KDTotalFSKonto", Me.GetLOData.Item(27))
		param = cmd.Parameters.AddWithValue("@KDTotalTempErlos", Me.GetLOData.Item(28))
		param = cmd.Parameters.AddWithValue("@KDTotalIndErlos", Me.GetLOData.Item(29))
		param = cmd.Parameters.AddWithValue("@KDTotalFErlos", Me.GetLOData.Item(30))

		If strMyKst.ToUpper.Contains("mse".ToUpper) Then
			Trace.WriteLine(Me.GetLOData.Item(31))
		End If
		param = cmd.Parameters.AddWithValue("@KDSVerluste", Me.GetLOData.Item(31))

		param = cmd.Parameters.AddWithValue("@KDVerlustA", Me.GetLOData.Item(32))
		param = cmd.Parameters.AddWithValue("@KDVerlustInd", Me.GetLOData.Item(33))
		param = cmd.Parameters.AddWithValue("@KDVerlustF", Me.GetLOData.Item(34))

		param = cmd.Parameters.AddWithValue("@KDRuA", Me.GetLOData.Item(35))
		param = cmd.Parameters.AddWithValue("@KDRuInd", Me.GetLOData.Item(36))
		param = cmd.Parameters.AddWithValue("@KDRuF", Me.GetLOData.Item(37))

		param = cmd.Parameters.AddWithValue("@KDGuA", Me.GetLOData.Item(38))
		param = cmd.Parameters.AddWithValue("@KDGuInd", Me.GetLOData.Item(39))
		param = cmd.Parameters.AddWithValue("@KDGuF", Me.GetLOData.Item(40))

		param = cmd.Parameters.AddWithValue("@lUSNr", iSelectedUSNr)
		param = cmd.Parameters.AddWithValue("@iMonthVonBis", m_SearchCriteria.FirstMonth) ' CStr(Me.GetiFMonth()))
		param = cmd.Parameters.AddWithValue("@strYear", m_SearchCriteria.FirstYear) ' Me.GetstrYear())
		param = cmd.Parameters.AddWithValue("@dNow", Format(Now, "g"))

		param = cmd.Parameters.AddWithValue("@UsFullName", m_InitialData.UserData.UserFullName)

		param = cmd.Parameters.AddWithValue("@CustomerNumber", m_SearchCriteria.customerNumber)
		param = cmd.Parameters.AddWithValue("@EmployeeNumber", m_SearchCriteria.employeeNumber)
		param = cmd.Parameters.AddWithValue("@EmploymentNumber", m_SearchCriteria.esNumber)

		cmd.ExecuteNonQuery()
		cmd.Parameters.Clear()


		ClsDataDetail.GetTotalTemp += Me.GetLOData.Item(20)
		ClsDataDetail.GetTotalInd += Me.GetLOData.Item(21)
		ClsDataDetail.GetTotalFest += Me.GetLOData.Item(22)


	End Sub

	Sub UpdateMyDataInJDb(ByVal strMyKst As String)
		Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr

		Dim Sql As String = "Update #UmsatzJournal_New set "
		Sql &= "Bruttolohn = Bruttolohn + " & Me.GetLOData(0) & ", "
		Sql &= "AHVlohn = AHVlohn + " & Me.GetLOData(1) & ", "

		Sql &= "FerBack = FerBack + " & Me.GetLOData(2) & ", "
		Sql &= "FeierBack = FeierBack + " & Me.GetLOData(3) & ", "
		Sql &= "LO13Back = LO13Back + " & Me.GetLOData(4) & ", "
		Sql &= "TimeBack = TimeBack + " & Me.GetLOData(5) & ", "

		Sql &= "FerAus = FerAus + " & Me.GetLOData(6) & ", "
		Sql &= "FeierAus = FeierAus + " & Me.GetLOData(7) & ", "
		Sql &= "LO13Aus = LO13Aus + " & Me.GetLOData(8) & ", "
		Sql &= "TimeAus = TimeAus + " & Me.GetLOData(9) & ", "

		' Achtung es ist negativer Betrag!!!
		Sql &= "Fremdleistung = Fremdleistung + " & Me.GetLOData(12) & ", "
		Sql &= "AGBetrag = AGBetrag + " & Me.GetLOData(13) & ", "

		Sql &= "KDTotal = KDTotal + " & Me.GetLOData(20) & ", "
		Sql &= "KDTotalInd = KDTotalInd + " & Me.GetLOData(21) & ", "
		Sql &= "KDTotalFest = KDTotalFest + " & Me.GetLOData(22) & ", "
		Sql &= "KDTotalFOp = KDTotalFOp + " & Me.GetLOData(24) & ", "

		' Achtung es ist negativer Betrag!!!
		Sql &= "KDTotalG = KDTotalG + " & Me.GetLOData(23) & ", "

		Sql &= "KDTotalTempSKonto = KDTotalTempSKonto + " & Me.GetLOData(25) & ", "
		Sql &= "KDTotalIndSKonto = KDTotalIndSKonto + " & Me.GetLOData(26) & ", "
		Sql &= "KDTotalFSKonto = KDTotalFSKonto + " & Me.GetLOData(27) & ", "

		Sql &= "KDTotalTempErlos = KDTotalTempErlos + " & Me.GetLOData(28) & ", "
		Sql &= "KDTotalIndErlos = KDTotalIndErlos + " & Me.GetLOData(29) & ", "
		Sql &= "KDTotalFErlos = KDTotalFErlos + " & Me.GetLOData(30) & ", "

		Sql &= "KDSVerluste = KDSVerluste + " & Me.GetLOData(31) & ", "

		Sql &= "KDVerlustA = KDVerlustA + " & Me.GetLOData(32) & ", "
		Sql &= "KDVerlustInd = KDVerlustInd + " & Me.GetLOData(33) & ", "
		Sql &= "KDVerlustF = KDVerlustF + " & Me.GetLOData(34) & ", "

		Sql &= "KDRuA = KDRuA + " & Me.GetLOData(35) & ", "
		Sql &= "KDRuInd = KDRuInd + " & Me.GetLOData(36) & ", "
		Sql &= "KDRuF = KDRuF + " & Me.GetLOData(37) & ", "

		Sql &= "KDGuA = KDGuA + " & Me.GetLOData(38) & ", "
		Sql &= "KDGuInd = KDGuInd + " & Me.GetLOData(39) & ", "
		Sql &= "KDGuF = KDGuF + " & Me.GetLOData(40) & " "


		Sql &= "Where Kst3_1 = '" & strMyKst & "' And UserNr = " & m_InitialData.UserData.UserNr  ' iSelectedUSNr

		Dim cmd As SqlClient.SqlCommand = New SqlCommand(Sql, GetDbConn)
		cmd.ExecuteNonQuery()

		ClsDataDetail.GetTotalTemp += Me.GetLOData.Item(20)
		ClsDataDetail.GetTotalInd += Me.GetLOData.Item(21)
		ClsDataDetail.GetTotalFest += Me.GetLOData.Item(22)

	End Sub

	Sub UpdateJournalWithCustomerNumber(ByVal customerNumber As Integer)
		Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr

		Dim Sql As String = "Update #UmsatzJournal_New set "
		Sql &= "Bruttolohn = Bruttolohn + " & Me.GetLOData(0) & ", "
		Sql &= "AHVlohn = AHVlohn + " & Me.GetLOData(1) & ", "

		Sql &= "FerBack = FerBack + " & Me.GetLOData(2) & ", "
		Sql &= "FeierBack = FeierBack + " & Me.GetLOData(3) & ", "
		Sql &= "LO13Back = LO13Back + " & Me.GetLOData(4) & ", "
		Sql &= "TimeBack = TimeBack + " & Me.GetLOData(5) & ", "

		Sql &= "FerAus = FerAus + " & Me.GetLOData(6) & ", "
		Sql &= "FeierAus = FeierAus + " & Me.GetLOData(7) & ", "
		Sql &= "LO13Aus = LO13Aus + " & Me.GetLOData(8) & ", "
		Sql &= "TimeAus = TimeAus + " & Me.GetLOData(9) & ", "

		' Achtung es ist negativer Betrag!!!
		Sql &= "Fremdleistung = Fremdleistung + " & Me.GetLOData(12) & ", "
		Sql &= "AGBetrag = AGBetrag + " & Me.GetLOData(13) & ", "

		Sql &= "KDTotal = KDTotal + " & Me.GetLOData(20) & ", "
		Sql &= "KDTotalInd = KDTotalInd + " & Me.GetLOData(21) & ", "
		Sql &= "KDTotalFest = KDTotalFest + " & Me.GetLOData(22) & ", "
		Sql &= "KDTotalFOp = KDTotalFOp + " & Me.GetLOData(24) & ", "

		' Achtung es ist negativer Betrag!!!
		Sql &= "KDTotalG = KDTotalG + " & Me.GetLOData(23) & ", "

		Sql &= "KDTotalTempSKonto = KDTotalTempSKonto + " & Me.GetLOData(25) & ", "
		Sql &= "KDTotalIndSKonto = KDTotalIndSKonto + " & Me.GetLOData(26) & ", "
		Sql &= "KDTotalFSKonto = KDTotalFSKonto + " & Me.GetLOData(27) & ", "

		Sql &= "KDTotalTempErlos = KDTotalTempErlos + " & Me.GetLOData(28) & ", "
		Sql &= "KDTotalIndErlos = KDTotalIndErlos + " & Me.GetLOData(29) & ", "
		Sql &= "KDTotalFErlos = KDTotalFErlos + " & Me.GetLOData(30) & ", "

		Sql &= "KDSVerluste = KDSVerluste + " & Me.GetLOData(31) & ", "

		Sql &= "KDVerlustA = KDVerlustA + " & Me.GetLOData(32) & ", "
		Sql &= "KDVerlustInd = KDVerlustInd + " & Me.GetLOData(33) & ", "
		Sql &= "KDVerlustF = KDVerlustF + " & Me.GetLOData(34) & ", "

		Sql &= "KDRuA = KDRuA + " & Me.GetLOData(35) & ", "
		Sql &= "KDRuInd = KDRuInd + " & Me.GetLOData(36) & ", "
		Sql &= "KDRuF = KDRuF + " & Me.GetLOData(37) & ", "

		Sql &= "KDGuA = KDGuA + " & Me.GetLOData(38) & ", "
		Sql &= "KDGuInd = KDGuInd + " & Me.GetLOData(39) & ", "
		Sql &= "KDGuF = KDGuF + " & Me.GetLOData(40) & " "


		Sql &= "Where CustomerNumber = " & customerNumber & " And UserNr = " & m_InitialData.UserData.UserNr

		Dim cmd As SqlClient.SqlCommand = New SqlCommand(Sql, GetDbConn)
		cmd.ExecuteNonQuery()

		ClsDataDetail.GetTotalTemp += Me.GetLOData.Item(20)
		ClsDataDetail.GetTotalInd += Me.GetLOData.Item(21)
		ClsDataDetail.GetTotalFest += Me.GetLOData.Item(22)

	End Sub


#Region "Sonstige Funktionen..."

	Function GetKSTName(ByVal strMyKST As String) As String
		Dim strResult As String = String.Empty
		Dim Sql = "Select Top 1 USNr, Vorname, Nachname, IsNull(USFiliale, '') As Bezeichnung From Benutzer Where KST = '"
		Sql &= strMyKST & "'"
		'  And Deaktiviert = 0"

		'Dim strQuery As String = _clsSQLString.GetKSTNameQuery(strMyKST)
		Dim cmd As SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(Sql, GetDbConn)
		Dim rUSrec As SqlDataReader = cmd.ExecuteReader

		' Vollständige Benutzername und Vorname
		While rUSrec.Read
			If Not String.IsNullOrWhiteSpace(rUSrec("Nachname").ToString) Then
				strResult = String.Format("{0} {1}@{2}", rUSrec("Vorname").ToString, rUSrec("Nachname"), rUSrec("Bezeichnung"))

				'If Not String.IsNullOrWhiteSpace(rUSrec("Bezeichnung").ToString) Then
				'	strResult &= "@" & rUSrec("Bezeichnung").ToString
				'Else
				'	strResult &= "@"
				'End If



				'Sql = "Select Top 1 Bezeichnung From US_Filiale Where USNr = " & rUSrec("USNr")

				'Sql = _clsSQLString.GetKSTFilialQuery(CInt(rUSrec("USNr").ToString))
			End If
		End While
		rUSrec.Close()

		'cmd = New System.Data.SqlClient.SqlCommand(Sql, GetDbConn)
		'  rUSrec = cmd.ExecuteReader

		'  ' Welcher Filiale gehört der User?
		'  While rUSrec.Read
		'	If Not String.IsNullOrEmpty(rUSrec("Bezeichnung").ToString) Then
		'		strResult += "@" & rUSrec("Bezeichnung").ToString
		'	Else
		'		strResult += "@Nicht definiert..."
		'	End If
		'  End While
		'  rUSrec.Close()

		Return strResult
	End Function

	'Function GetXMargeFromMD() As Decimal
	'	Dim dblResult As Decimal = 0
	'	Dim strQuery As String = _clsSQLString.GetMDXMargeQuery()
	'	Dim cmd As SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, GetDbConn)
	'	Dim param As System.Data.SqlClient.SqlParameter

	'	param = cmd.Parameters.AddWithValue("@MDYear", _strBYear)
	'	Dim rMDrec As SqlDataReader = cmd.ExecuteReader

	'	' Vollständige Benutzername und Vorname
	'	While rMDrec.Read
	'		If Not String.IsNullOrEmpty(rMDrec("X_Marge").ToString) Then
	'			dblResult = CDec(rMDrec("X_Marge").ToString)
	'		End If
	'	End While
	'	rMDrec.Close()

	'	ClsDataDetail.GetXMarge = dblResult
	'	Return dblResult
	'End Function

	Function FillMyLstoExistJData(ByVal iCount As Integer) As List(Of Double)
		Dim loiBetrag As New List(Of Double)

		If iCount = 0 Then iCount = 30
		For i As Integer = 0 To iCount - 1
			loiBetrag.Add(0)
			loiBetrag(i) = 0
		Next

		Return loiBetrag
	End Function

#End Region


#Region "Funktionen zur Zusammenzählen..."

	Sub CallculateAllFields()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		If ClsDataDetail.Conn Is Nothing OrElse ClsDataDetail.Conn.State = ConnectionState.Closed Then Return
		Dim iSelectedUSNr As Integer = ClsDataDetail.GetAutoUserNr
		Dim strDbName As String = String.Format("[UmsatzJournal_New_{0}]", _ClsProgSetting.GetLogedUSGuid) ' iSelectedUSNr)
		Dim strLAPflicht As String = GetLAAHVPflicht(m_SearchCriteria.FirstYear) ' Me._strVYear)
		Dim bNetto530 As Boolean = Not CBool(strLAPflicht.Split(CChar("#"))(0))
		Dim bNetto630 As Boolean = Not CBool(strLAPflicht.Split(CChar("#"))(1))
		Dim bNetto730 As Boolean = Not CBool(strLAPflicht.Split(CChar("#"))(2))
		Dim bNetto800 As Boolean = True

		Dim Sql As String = String.Format("BEGIN TRY DROP Procedure [dbo].[Callculate Db1Fields UJournal_{0}] ", m_InitialData.UserData.UserGuid) ' _ClsProgSetting.GetLogedUSGuid)	' iSelectedUSNr)
		Sql &= "END TRY BEGIN CATCH END CATCH "
		Sql &= "Alter Table {0} Add _530 money "
		Sql &= "Alter Table {0} Add _630 money "
		Sql &= "Alter Table {0} Add _730 money "
		Sql &= "Alter Table {0} Add _800 money "
		Sql &= "Alter Table {0} Add _AGProz money "
		Sql &= "Alter Table {0} Add _530_ Bit "
		Sql &= "Alter Table {0} Add _630_ Bit "
		Sql &= "Alter Table {0} Add _730_ Bit "
		Sql = String.Format(Sql, strDbName)

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(Sql, ClsDataDetail.Conn)
			cmd.ExecuteNonQuery()

		Catch ex As SqlClient.SqlException

		Catch ex As Exception
			'MsgBox(ex.StackTrace, MsgBoxStyle.Critical, "CreateMySP4FilialProz_1")

		End Try

		Sql = String.Format("Create Procedure [dbo].[Callculate Db1Fields UJournal_{0}] ", m_InitialData.UserData.UserGuid) ' iSelectedUSNr)

		Sql &= "As "

		' -- Totalisierung der Einzelnen Kostenstellen
		' ---------------------------------------------------------------------------------------------------------------------------
		Sql &= "Update {0} Set "
		Sql &= "_TempUmsatz = "
		Sql &= "(KDTotal - (kdtotaltempskonto + KDVerlustA + KDRuA + KDGuA) ), "
		Sql &= "_IndUmsatz = "
		Sql &= "(KDTotalInd - (kdtotalIndskonto + KDTotalFOP + KDVerlustInd + KDRuInd + KDGuInd) ), "
		Sql &= "_FestUmsatz = "
		Sql &= "(KDTotalFest - (kdtotalFskonto + KDVerlustF + KDRuF + KDGuF) ), "

		Sql &= "Adminkosten = "
		Sql &= "((AHVLohn)*_XMarge/100), "
		Sql &= "_Lohnaufwand_1 = "
		Sql &= "(Bruttolohn)+(AGBetrag*-1)+((AHVLohn)*_XMarge/100)-(fremdleistung), "

		Sql &= "_BGInd = "
		Sql &= "(KDTotalInd - (kdtotalInderlos+kdtotalindskonto+KDsVerluste+KDTotalFOP+KDVerlustInd+KDRuInd+KDGuInd) ), "
		Sql &= "_BGFest = "
		Sql &= "(KDTotalFest - (kdtotalFerlos + kdtotalFskonto + KDVerlustF + KDRuF + KDGuF) ) "

		Sql &= "Update {0} Set AGProz = 0 Where AGProz Is Null "
		Sql &= "Update {0} Set AGBetrag_2 = 0 Where AGBetrag_2 Is Null "
		Sql &= "Update {0} Set _BGTemp = 0 Where _BGTemp Is Null "
		Sql &= "Update {0} Set _PAnteil_U_BGT = 0 Where _PAnteil_U_BGT Is Null "
		Sql &= "Update {0} Set _PAnteil_F_BGT = 0 Where _PAnteil_F_BGT Is Null "

		Sql &= "Update {0} Set _530 = 0 Where _530 Is Null "
		Sql &= "Update {0} Set _630 = 0 Where _630 Is Null "
		Sql &= "Update {0} Set _730 = 0 Where _730 Is Null "
		Sql &= "Update {0} Set _800 = 0 Where _800 Is Null "
		Sql &= "Update {0} Set _AGProz = 0 Where _AGProz Is Null "

		'    strQuery &= "From " & strDbName & " UmJ "

		' Testing...
		'strQuery &= "Update {0} Set _530 = isnull(FeierBack, 0), "
		'strQuery &= "_630 = isnull(FerBack,0), "
		'strQuery &= "_730 = isnull(LO13Back,0), "
		'strQuery &= "_800 = isnull(TimeBack,0) "
		'strQuery &= "Update {0} Set _AGProz = (((AGBetrag *-1)*100) / AHVLohn) Where AHVLohn <> 0 "

		Sql &= "Update {0} Set _530 = isnull(FeierAus, 0) + isnull(FeierBack, 0), "
		Sql &= "_630 = isnull(FerAus,0)+isnull(FerBack,0), "
		Sql &= "_730 = isnull(LO13Aus,0)+isnull(LO13Back,0), "
		Sql &= "_800 =isnull(TimeAus,0)+isnull(TimeBack,0) "
		Sql &= "Update {0} Set _AGProz = (((AGBetrag *-1)*100) / AHVLohn) Where AHVLohn <> 0 "

		' ---------------------------------------------------------------------
		Sql &= "Update {0} Set _530_ = "
		If bNetto530 Then Sql &= "0" Else Sql &= "1"
		Sql &= "Update {0} Set _630_ = "
		If bNetto630 Then Sql &= "0" Else Sql &= "1"
		Sql &= "Update {0} Set _730_ = "
		If bNetto730 Then Sql &= "0" Else Sql &= "1"
		Sql &= " "

		' ---------------------------------------------------------------------
		Sql &= "Update {0} Set "
		Sql &= "BackAus = "
		'If bNettoBackAus Then
		'  strQuery &= "(TimeAus+TimeBack) "
		'Else
		'  strQuery &= "((FerAus+FeierAus+LO13Aus+TimeAus)+(FerBack+FeierBack+LO13Back+TimeBack)) "
		'End If

		'    strQuery &= "((FerAus+FeierAus+LO13Aus+TimeAus)+(FerBack+FeierBack+LO13Back+TimeBack)) "


		Sql &= "(_530+_630+_730+_800) "

		' ----------------------------------------------------------------------------------------------------------------------------

		' NUR wenn AHV-Lohn <> 0
		Sql &= "Update {0} Set "
		Sql &= "_Lohnaufwand_2 = "

		Sql &= "0,"
		Sql &= "AGProz = _AGProz, " '(((AGBetrag *-1)*100)/(AHVLohn)), "
		Sql &= "AGBetrag_2 = ("
		If Not bNetto530 Then Sql &= "(_530 + " Else Sql &= "(0 + "
		If Not bNetto630 Then Sql &= "_630 + " Else Sql &= "0 + "
		If Not bNetto730 Then Sql &= "_730 + " Else Sql &= "0 + "

		Sql &= "_800) * _AGProz)/100 "


		'    strQuery &= "( ( (AGBetrag*-100) / AHVLohn) /100), "

		Sql &= "Update {0} Set "
		Sql &= "_BGTemp = "
		Sql &= "(KDTotal - (kdtotaltempskonto + KDVerlustA + KDRuA + KDGuA) - "
		Sql &= "(Bruttolohn+(AGBetrag*-1)+(Adminkosten)-fremdleistung)) + "

		Sql &= "AGBetrag_2 + "
		Sql &= "("
		If Not bNetto530 Then Sql &= "(_530 + " Else Sql &= "(0 + "
		If Not bNetto630 Then Sql &= "_630 + " Else Sql &= "0 + "
		If Not bNetto730 Then Sql &= "_730 + " Else Sql &= "0 + "

		Sql &= "_800) ) "



		' ----------------------------------------------------------------------------------------------------------------------------
		' wenn AHV = 0 aber Bruttolohn <> 0
		'strQuery &= "Update {0} Set "
		'strQuery &= "_BGTemp = "
		'strQuery &= "(KDTotal - (kdtotaltempskonto + KDVerlustA + KDRuA + KDGuA) - "
		'strQuery &= "(Bruttolohn+(AGBetrag*-1)+(Adminkosten)-fremdleistung)) + "
		'strQuery &= "0 "
		'strQuery &= "Where AHVLohn = 0 And Bruttolohn <> 0 "


		' ----------------------------------------------------------------------------------------------------------------------------
		' Wenn AHV-Lohn = 0 aber Temporäre Debitorenverluste gibt!!!
		'strQuery &= "Update {0} Set "
		'strQuery &= "_BGTemp = "
		'strQuery &= "KDTotal - (kdtotaltempskonto + KDVerlustA + KDRuA + KDGuA) "
		'strQuery &= "Where AHVLohn = 0 And Bruttolohn = 0 "


		' ----------------------------------------------------------------------------------------------------------------------------
		'-- Marge berechnen
		Sql &= "Update {0} Set "
		Sql &= "[_Marge] = "
		Sql &= "(_BGTemp/KDTotal)*100 "
		Sql &= "Where KDTotal <> 0 "


		' ----------------------------------------------------------------------------------------------------------------------------
		'-- Totalisierung der Filiale
		Sql &= "Update {0} Set "
		Sql &= "F_TempUmsatz = (select sum(_TempUmsatz) From {0} Umj "
		Sql &= "Where Umj.USFiliale = {0}.USFiliale Group By Umj.USFiliale), "
		Sql &= "F_IndUmsatz = (select sum(_IndUmsatz) From {0} Umj "
		Sql &= "Where Umj.USFiliale = {0}.USFiliale Group By Umj.USFiliale), "
		Sql &= "F_FestUmsatz = (select sum(_FestUmsatz) From {0} Umj "
		Sql &= "Where Umj.USFiliale = {0}.USFiliale Group By Umj.USFiliale), "

		Sql &= "F_Lohnaufwand_1 = (select sum(_Lohnaufwand_1) From {0} Umj "
		Sql &= "Where Umj.USFiliale = {0}.USFiliale Group By Umj.USFiliale), "
		Sql &= "F_Lohnaufwand_2 = (select sum(_Lohnaufwand_2) From {0} Umj "
		Sql &= "Where Umj.USFiliale = {0}.USFiliale Group By Umj.USFiliale), "

		Sql &= "F_BGTemp = (select sum(_BGTemp) From {0} Umj Where Umj.USFiliale = {0}.USFiliale Group By Umj.USFiliale), "
		Sql &= "F_BGInd = (select sum(_BGInd) From {0} Umj Where Umj.USFiliale = {0}.USFiliale Group By Umj.USFiliale), "
		Sql &= "F_BGFest = (select sum(_BGFest) From {0} Umj Where Umj.USFiliale = {0}.USFiliale Group By Umj.USFiliale) "


		' ----------------------------------------------------------------------------------------------------------------------------
		'-- Einzelne Felder ausfüllen...
		Sql &= "Update {0} Set "
		Sql &= "[_PAnteil_F_BGT] = _BGTemp / F_BGTemp * 100 "
		Sql &= "Where (F_BGTemp <> 0) "

		' ----------------------------------------------------------------------------------------------------------------------------
		Sql &= "Update {0} Set "
		Sql &= "[_PAnteil_F_BGI] = _BGInd / F_BGInd * 100 "
		Sql &= "Where (F_BGInd <> 0) "

		' ----------------------------------------------------------------------------------------------------------------------------
		Sql &= "Update {0} Set "
		Sql &= "[_PAnteil_F_BGF] = _BGFest / F_BGFest * 100 "
		Sql &= "Where (F_BGFest <> 0) "


		' ----------------------------------------------------------------------------------------------------------------------------
		' -- Totalisierung der Unternehmen
		Sql &= "Update {0} Set "
		Sql &= "U_TempUmsatz = (select sum(_TempUmsatz) From {0}), "
		Sql &= "U_IndUmsatz = (select sum(_IndUmsatz) From {0}), "
		Sql &= "U_FestUmsatz = (select sum(_FestUmsatz) From {0} ), "
		Sql &= "U_Lohnaufwand_1 = (select sum(_Lohnaufwand_1) From {0} ), "
		Sql &= "U_Lohnaufwand_2 = (select sum(_Lohnaufwand_2) From {0} ), "
		Sql &= "U_BGTemp = (select sum(_BGTemp) From {0} ), "
		Sql &= "U_BGInd = (select sum(_BGInd) From {0} ), "
		Sql &= "U_BGFest = (select sum(_BGFest) From {0} ) "


		' ----------------------------------------------------------------------------------------------------------------------------
		' -- Einzelne Felder ausfüllen...
		Sql &= "Update {0} Set "
		Sql &= "[_PAnteil_U_BGT] = _BGTemp / U_BGTemp * 100 "
		Sql &= "where (U_BGTemp <> 0) "

		' ----------------------------------------------------------------------------------------------------------------------------
		Sql &= "Update {0} Set "
		Sql &= "[_PAnteil_U_BGI] = _BGInd / U_BGInd * 100 "
		Sql &= "where (U_BGInd <> 0) "

		' ----------------------------------------------------------------------------------------------------------------------------
		Sql &= "Update {0} Set "
		Sql &= "[_PAnteil_U_BGF] = _BGFest / U_BGFest * 100 "
		Sql &= "where (U_BGFest <> 0) "

		' ______________________________________________________________________________________________________________________
		Sql = String.Format(Sql, strDbName)

		Dim Time_1 As Double = System.Environment.TickCount
		Dim sSql As String = Sql  '"[Callculate Db1Fields]"

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)

			Try
				cmd.ExecuteNonQuery()


				Try
					sSql = String.Format("[Callculate Db1Fields UJournal_{0}]", m_InitialData.UserData.UserGuid)  ' iSelectedUSNr)
					cmd = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)
					cmd.ExecuteNonQuery()


				Catch ex As Exception

				End Try


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}:Query ausführen. {1}", strMethodeName, ex.Message))
				m_UtilityUi.ShowErrorDialog(ex.ToString)
				'MsgBox(ex.StackTrace & vbNewLine & ex.Message, MsgBoxStyle.Critical, "SQL:CallculateAllFields_0")

			End Try


		Catch ex As SqlException
			m_Logger.LogError(String.Format("{0}:Datenbank öffnen. {1}", strMethodeName, ex.Message))
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}:Datenbank öffnen. {1}", strMethodeName, ex.Message))
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally

		End Try

	End Sub

#End Region

End Class
