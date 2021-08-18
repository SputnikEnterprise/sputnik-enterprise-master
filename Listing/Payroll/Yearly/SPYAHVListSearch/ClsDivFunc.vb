
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

Imports SPYAHVListSearch.ClsDataDetail


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

	Private _ClsDataSaver As DbDataSaver

#End Region


#Region "Contructor"

	Public Sub New(ByVal _setting As SearchCriteria)

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_SearchCriteria = _setting
		_ClsDataSaver = New DbDataSaver

		_ClsDataSaver.GetLA_7100 = 0
		_ClsDataSaver.GetLA_7110 = 0
		_ClsDataSaver.GetLA_7120 = 0
		_ClsDataSaver.GetLA_7220 = 0
		_ClsDataSaver.GetLA_7240 = 0

	End Sub

#End Region


#Region "Funktionen zur Suche nach Daten..."

	Function GetStartSQLString() As String
		Dim Sql As String = String.Empty

		' Datensätze löschen...
		If Not DeleteRecFromDb(m_SearchCriteria.FirstYear) Then Return Sql

		Sql = "Select LO.MANr, LO.LP, LO.Jahr, LO.[AHV-Basis] As AHVBasis, LO.[AHV-Lohn] As AHVLohn, "
		Sql += "LO.[AHV-Freibetrag] + LO.[Nicht AHV-pflichtig] As AHVFrei, LO.[ALV1-Lohn] As ALV1Lohn, LO.[ALV2-Lohn] As ALV2Lohn, "
		Sql += "MA.Nachname As MANachname, MA.Vorname As MAVorname, MA.Geschlecht, MA.MA_Kanton, Ma.Nationality As MANationality, "
		Sql += "MA.AHV_Nr, MA.AHV_Nr_New, MA.GebDat "
		Sql += "From dbo.LO "
		Sql &= "Left Join dbo.Mitarbeiter MA On LO.MANr = MA.MANr "


		Return Sql

	End Function

	Function GetQuerySQLString() As String
		Dim sSql As String = String.Empty
		Dim strFieldName As String = String.Empty

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strName As String()
		Dim strMyName As String = String.Empty


		' Mandantennummer -------------------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "LO.MDNr"
		sZusatzBez = CStr(m_InitialData.MDData.MDNr)
		sSql += strAndString & strFieldName & " = " & sZusatzBez & " "

		' Jahr -------------------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "LO.Jahr"
		If m_SearchCriteria.FirstYear > 0 Then
			sZusatzBez = CStr(m_SearchCriteria.FirstYear)
			FilterBez += "Jahr wie (" & sZusatzBez & ") " & vbLf

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			sSql += strAndString & strFieldName & " In (" & sZusatzBez & ")"
		End If

		' Kanton des Kandidaten -------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "MA.MA_Kanton"
		If Not String.IsNullOrWhiteSpace(m_SearchCriteria.kanton) Then
			sZusatzBez = m_SearchCriteria.kanton
			FilterBez += "Kanton wie (" & sZusatzBez & ") " & vbLf

			If InStr(UCase(sZusatzBez), UCase("Not defined")) > 0 Then
				sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else

				sZusatzBez = (sZusatzBez.Trim)
				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName

				sSql += strAndString & strFieldName & " In ('" & sZusatzBez.Trim & "')"
			End If

		End If

		' Filiale anhand der Berater -----------------------------------------------------------------------------------------
		Dim advisorAsignedBrunchoffice As String = String.Empty
		If Not String.IsNullOrWhiteSpace(m_SearchCriteria.filiale) Then
			advisorAsignedBrunchoffice = GetFilialKstData(m_SearchCriteria.filiale)
		End If

		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "MA.Kst"
		If Not String.IsNullOrWhiteSpace(m_SearchCriteria.filiale) Then
			sZusatzBez = m_SearchCriteria.filiale
			FilterBez += "Filiale wie (" & sZusatzBez & ") " & vbLf

			If sZusatzBez.ToLower.Contains("not defined") Then
				'sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "

			Else

				sZusatzBez = advisorAsignedBrunchoffice ' GetFilialKstData(sZusatzBez)
				If sZusatzBez <> String.Empty Then
					sZusatzBez = Replace(sZusatzBez, "'", "")
					strName = Regex.Split(sZusatzBez.Trim, ",")
					strMyName = String.Empty
					For i As Integer = 0 To strName.Length - 1
						If strName(i).Trim <> String.Empty Then
							strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " = '" & strName(i).Trim & "'"
						End If
					Next
					If strName.Length > 0 Then sZusatzBez = strMyName

					sSql += strAndString & " (" & sZusatzBez & ")"
				End If
			End If
		End If

		' Nationalität -------------------------------------------------------------------------------------------
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "MA.Nationality"
		If Not String.IsNullOrWhiteSpace(m_SearchCriteria.nationality) Then
			sZusatzBez = m_SearchCriteria.nationality
			FilterBez += "Nationalität wie (" & sZusatzBez & ") " & vbLf

			If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				sSql += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else

				'sZusatzBez = GetKantonPLZ(sZusatzBez)
				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName

				sSql += strAndString & strFieldName & " In ('" & sZusatzBez.Trim & "')"
			End If

		End If

		' Filialen Teilung...
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		If Not String.IsNullOrWhiteSpace(m_SearchCriteria.filiale) AndAlso Not m_SearchCriteria.filiale.ToLower.Contains("not defined") Then
			strFieldName = "MA.Kst"
			sZusatzBez = m_SearchCriteria.filiale
			FilterBez += "Filiale wie (" & sZusatzBez & ") " & vbLf

			sZusatzBez = advisorAsignedBrunchoffice ' GetFilialKstData(sZusatzBez)
			sZusatzBez = Replace(sZusatzBez, "'", "")
			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%" & strName(i).Trim & "%'"
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			sSql += strAndString & " (" & sZusatzBez & ")"

		End If

		' 0-Beträge nicht nehmen...
		strAndString = IIf(sSql <> String.Empty, " And ", String.Empty).ToString
		strFieldName = "(LO.[AHV-Basis] + LO.[AHV-Freibetrag] + LO.[Nicht AHV-pflichtig] + LO.[AHV-Lohn] + LO.[ALV1-Lohn] + LO.[ALV2-Lohn] <> 0) "
		sSql += strAndString & strFieldName


		ClsDataDetail.GetFilterBez = FilterBez

		Return sSql
	End Function

	Function GetStartSQLString_2() As String
		Dim sSql As String = String.Empty
		Dim sZusatzBez As String = String.Empty
		
		sSql = "[Get AHVYearList For Print_2] "

		sSql += m_InitialData.MDData.MDNr & ", " & m_InitialData.UserData.UserNr & ", " & m_SearchCriteria.FirstYear & ", '" & m_SearchCriteria.filiale & "'"

		Return sSql
	End Function

	Function GetSortString() As String
		Dim strSort As String = " Order By "
		Dim strSortBez As String = String.Empty
		Dim strMyName As String = String.Empty

		strMyName = "MA.Nachname, MA.Vorname, LO.LP, LO.Jahr"
		strSortBez = CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Kandidatenname, Monat, Jahr"

		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function

	Function GetSortString_2() As String
		Dim strSort As String = " Order By "
		Dim strSortBez As String = String.Empty
		Dim strMyName As String = String.Empty

		strMyName = "Db.MAName, Db.AbLP"
		strSortBez = CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Mitarbeitername"

		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function



#End Region

#Region "Sonstige Funktionen für Datenbank..."

	Function BuildYAHVDb(ByVal strQuery As String, ByVal strFiliale As String, ByVal year As Integer?) As Boolean
		Dim success As Boolean = True
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim i As Integer = 0
		Dim iMANr As Integer? = 0
		Dim cBLohnProz As Decimal = 100
		Dim cAHVLohnProz As Decimal = 100
		Dim cBetrag As Decimal = 0
		Dim sFMonth As Integer? = 0
		Dim sLMonth As Integer? = 0

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)

			Dim rYLOrec As SqlDataReader = cmd.ExecuteReader

			If rYLOrec.HasRows() Then
				While True

					If iMANr = 0 Then rYLOrec.Read()
					iMANr = m_utility.SafeGetInteger(rYLOrec, "MANr", 0)

					year = m_utility.SafeGetInteger(rYLOrec, "Jahr", 0)
					sFMonth = m_utility.SafeGetInteger(rYLOrec, "LP", 0)
					sLMonth = m_utility.SafeGetInteger(rYLOrec, "LP", 0)

					While iMANr = m_utility.SafeGetInteger(rYLOrec, "MANr", 0) 'And m_utility.SafeGetInteger(rYLOrec, "LP", 0) = sLMonth + 1

						'If iMANr = m_utility.SafeGetInteger(rYLOrec, "MANr", 0) Then
						_ClsDataSaver.GetMANachName = m_utility.SafeGetString(rYLOrec, "MANachname")
						_ClsDataSaver.GetMAVorName = m_utility.SafeGetString(rYLOrec, "MAVorname")

						_ClsDataSaver.GetMAGebDat = m_utility.SafeGetDateTime(rYLOrec, "GebDat", Nothing)
						_ClsDataSaver.GetMAAHVOld = m_utility.SafeGetString(rYLOrec, "AHV_Nr")
						_ClsDataSaver.GetMAAHVNew = m_utility.SafeGetString(rYLOrec, "AHV_Nr_New")

						_ClsDataSaver.GetLA_7100 += (m_utility.SafeGetDecimal(rYLOrec, "AHVBasis", 0) * cAHVLohnProz) / 100
						_ClsDataSaver.GetLA_7110 += (m_utility.SafeGetDecimal(rYLOrec, "AHVLohn", 0) * cAHVLohnProz) / 100
						_ClsDataSaver.GetLA_7120 += (m_utility.SafeGetDecimal(rYLOrec, "AHVFrei", 0) * cAHVLohnProz) / 100
						_ClsDataSaver.GetLA_7220 += (m_utility.SafeGetDecimal(rYLOrec, "ALV1Lohn", 0) * cAHVLohnProz) / 100
						_ClsDataSaver.GetLA_7240 += (m_utility.SafeGetDecimal(rYLOrec, "ALV2Lohn", 0) * cAHVLohnProz) / 100
						'End If

						If m_utility.SafeGetInteger(rYLOrec, "LP", 0) = sLMonth + 1 Then
							sLMonth = m_utility.SafeGetInteger(rYLOrec, "LP", 0)
						End If

						rYLOrec.Read()

						Try
							If _ClsDataSaver.GetMANachName.ToLower.Contains("tipan") Then
								Trace.WriteLine(_ClsDataSaver.GetMANachName)
							End If

							If rYLOrec.HasRows Then
								'If iMANr <> m_utility.SafeGetInteger(rYLOrec, "MANr", 0) Then Exit While
								If m_utility.SafeGetInteger(rYLOrec, "LP", 0) <> sLMonth + 1 Then Exit While
								'If m_utility.SafeGetInteger(rYLOrec, "MANr", Nothing) Is Nothing Then Exit While
							End If

						Catch ex As SqlException
							m_Logger.LogError(ex.Message)
						Catch ex As InvalidOperationException
							Exit While
						Catch ex As Exception

							Exit While

						End Try

					End While

					' Datensatz hinzufügen...
					success = success AndAlso GetInsertString(iMANr, year, sFMonth, sLMonth)
					If Not success Then Return False

					_ClsDataSaver.GetMANachName = String.Empty
					_ClsDataSaver.GetMAVorName = String.Empty
					_ClsDataSaver.GetMAGebDat = Nothing
					_ClsDataSaver.GetMAAHVOld = String.Empty
					_ClsDataSaver.GetMAAHVNew = String.Empty

					_ClsDataSaver.GetLA_7100 = 0
					_ClsDataSaver.GetLA_7110 = 0
					_ClsDataSaver.GetLA_7120 = 0
					_ClsDataSaver.GetLA_7220 = 0
					_ClsDataSaver.GetLA_7240 = 0

					Try
						If IsDBNull(rYLOrec("MANr")) Then Exit While

					Catch ex As Exception
						'm_Logger.LogError(ex.ToString)
						Exit While
					End Try

				End While
			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return True

	End Function

	Private Function GetInsertString(ByVal iMANr As Integer?, ByVal year As Integer?, ByVal sFirstMonth As Integer?, ByVal sLastMonth As Integer?) As Boolean
		Dim success As Boolean = True
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim i As Integer = 0

		Dim strResult As String = "Insert Into AHV_Year (MDNr, MANr, Jahr, USNr, AbLP, BisLP, _7100, _7110, _7120, "
		strResult += "_7220, _7240, _3600, MAName, AHVNr, GebDat, "
		strResult += "CreatedFrom, CreatedOn) "
		strResult += "Values (@MDNr, @MANr, @Jahr, @USNr, @AbLP, @BisLP, "
		strResult += "@_7100, @_7110, @_7120, @_7220, @_7240, @_3600, "
		strResult += "@MAName, @AHVNr, @GebDat, "
		strResult += "@CreatedFrom, getdate())"


		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strResult, Conn)

			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
			param = cmd.Parameters.AddWithValue("@MANr", iMANr)
			param = cmd.Parameters.AddWithValue("@Jahr", year)
			param = cmd.Parameters.AddWithValue("@USNr", m_InitialData.UserData.UserNr)

			param = cmd.Parameters.AddWithValue("@AbLP", sFirstMonth)
			param = cmd.Parameters.AddWithValue("@BisLP", sLastMonth)

			param = cmd.Parameters.AddWithValue("@_7100", ReplaceMissing(_ClsDataSaver.GetLA_7100, 0))
			param = cmd.Parameters.AddWithValue("@_7110", ReplaceMissing(_ClsDataSaver.GetLA_7110, 0))
			param = cmd.Parameters.AddWithValue("@_7120", ReplaceMissing(_ClsDataSaver.GetLA_7120, 0))
			param = cmd.Parameters.AddWithValue("@_7220", ReplaceMissing(_ClsDataSaver.GetLA_7220, 0))
			param = cmd.Parameters.AddWithValue("@_7240", ReplaceMissing(_ClsDataSaver.GetLA_7240, 0))
			'Dim FAKZusalgen As Decimal = GetFAKZulagen(iMANr, sFirstMonth, sLastMonth, year)

			param = cmd.Parameters.AddWithValue("@_3600", ReplaceMissing(_ClsDataSaver.GetLA_7240, 0))


			param = cmd.Parameters.AddWithValue("@MAName", String.Format("{0}, {1}", _ClsDataSaver.GetMANachName, _ClsDataSaver.GetMAVorName))

			If Len(_ClsDataSaver.GetMAAHVNew) > 14 Then
				param = cmd.Parameters.AddWithValue("@AHVNr", ReplaceMissing(_ClsDataSaver.GetMAAHVNew, String.Empty))
				param = cmd.Parameters.AddWithValue("@GebDat", String.Empty)
			ElseIf Len(_ClsDataSaver.GetMAAHVOld) > 13 Then
				param = cmd.Parameters.AddWithValue("@AHVNr", ReplaceMissing(_ClsDataSaver.GetMAAHVOld, String.Empty))
				param = cmd.Parameters.AddWithValue("@GebDat", String.Empty)
			Else
				param = cmd.Parameters.AddWithValue("@AHVNr", String.Empty)
				param = cmd.Parameters.AddWithValue("@GebDat", ReplaceMissing(_ClsDataSaver.GetMAGebDat, DBNull.Value))
			End If

			param = cmd.Parameters.AddWithValue("@CreatedFrom", ReplaceMissing(m_InitialData.UserData.UserFullNameWithComma, DBNull.Value))
			'param = cmd.Parameters.AddWithValue("@CreatedOn", Now.Date)


			cmd.ExecuteNonQuery()     ' Datensatz hinzufügen...


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUi.ShowErrorDialog(ex.ToString)
			Return False

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return success

	End Function

	Private Function LoadFAKZulagen(ByVal mdNumber As Integer, ByVal employeeNumber As Integer, ByVal firstMonth As Integer, ByVal lastMonth As Integer, ByVal mdYear As Integer) As Decimal
		Dim result As Decimal = 0

		Dim sql As String
		sql = "Select Sum(m_btr) Betrag"
		sql &= " From LOL"
		sql &= " Where LOL.MDNr = @MDNr"
		sql &= " AND LOL.MANr = @MANr"
		sql &= " AND LOL.LP Between @firstMonth And @lastMonth"
		sql &= " AND LOL.Jahr = @mdYear"
		sql &= " AND LOL.LANr In (3600, 3650, 3700, 3750, 3800, 3900)"



		Return result

	End Function



#End Region

End Class

Class DbDataSaver

	Public Sub New()

		For i As Integer = 0 To 8
			Me.GetLAValues(i) = 0
		Next

	End Sub

	'// LANr
	Dim _LANr(8) As Decimal
	Property GetLAValues() As Decimal()
		Get
			Return _LANr
		End Get
		Set(ByVal value As Decimal())
			_LANr = value
		End Set
	End Property

	'// MAName
	Property GetMANachName() As String

	'// MAVorname
	Property GetMAVorName() As String

	'// Geschlecht
	Dim _strGeschlecht As String
	Property GetMAGeschlecht() As String
		Get
			Return _strGeschlecht
		End Get
		Set(ByVal value As String)
			If _strGeschlecht = "W" Then
				_strGeschlecht = "F"
			Else
				_strGeschlecht = "M"
			End If
			_strGeschlecht = value
		End Set
	End Property

	'// AHV_Nr
	Property GetMAAHVOld() As String

	'// AHV_Nr
	Property GetMAAHVNew() As String

	'// GebDat
	Property GetMAGebDat() As DateTime?


	'// 7000 Bruttolohn Prozent pro Filiale
	Property GetFilialBLohnProz() As Decimal?
	

	'// 7100 AHVlohn Prozent pro Filiale
	Property GetFilialAHVLohnProz() As Decimal?



	'// 7100 AHV-Basis
	Property GetLA_7100() As Decimal?

	'// 7120 AHV-Freibetrag
	Property GetLA_7120() As Decimal?

	'// 7110 AHV-Lohn
	Property GetLA_7110() As Decimal?

	'// 7220 ALV1-Lohn
	Property GetLA_7220() As Decimal?

	'// 7240 ALV2-Lohn
	Property GetLA_7240() As Decimal?


End Class

