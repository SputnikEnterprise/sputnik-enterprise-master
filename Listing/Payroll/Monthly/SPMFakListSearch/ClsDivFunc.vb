
Imports System.Text.RegularExpressions
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SPMFakListSearch.ClsDataDetail


Public Class ClsDbFunc

#Region "Private Fields"

	'Private _ClsFunc As New ClsDivFunc
	'Private _ClsDataSaver As New DbDataSaver

	'Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


	Protected Shared m_Logger As ILogger = New Logger()

	Private m_md As Mandant

	Private m_common As CommonSetting
	Protected m_utility As Utilities
	Protected m_UtilityUi As SP.Infrastructure.UI.UtilityUI

	Private m_SearchCriteria As New SearchCriteria

#End Region


#Region "Contructor"

	Public Sub New(ByVal _search As SearchCriteria)

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

		m_SearchCriteria = _search

	End Sub

#End Region


	''' <summary>
	''' listet eine Auflistung der Mandantendaten
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Function LoadMandantenData() As IEnumerable(Of MandantenData)
		Dim m_utility As New Utilities
		Dim result As List(Of MandantenData) = Nothing
		m_md = New Mandant

		Dim sql As String = "[Mandanten. Get All Allowed MDData]"

		Dim reader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sql, Nothing, CommandType.StoredProcedure)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of MandantenData)

				While reader.Read()
					Dim advisorData As New MandantenData

					advisorData.MDNr = CInt(m_utility.SafeGetInteger(reader, "MDNr", 0))
					advisorData.MDName = m_utility.SafeGetString(reader, "MDName")
					advisorData.MDGuid = m_utility.SafeGetString(reader, "MDGuid")
					advisorData.MDConnStr = m_md.GetSelectedMDData(advisorData.MDNr).MDDbConn

					result.Add(advisorData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_utility.CloseReader(reader)

		End Try

		Return result

	End Function




#Region "Funktionen zur Suche nach Daten..."

	Function GetStartSQLString() As String
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		Dim _ClsReg As New SPProgUtility.ClsDivReg


		' Datensätze löschen...
		DeleteRecFromDb(m_SearchCriteria.firstYear)

		sSql = "Select LOL.LONr, LOL.MANr, LOL.LP, LOL.LANr, LOL.m_Anz, LOL.m_Bas, "
		sSql += "LOL.m_Bas, LOL.m_Ans, LOL.m_Btr, Convert(int, LOL.Jahr) As Jahr, LOL.S_Kanton, LOL.RPText, "
		sSql += "MA.Nachname As MANachname, MA.Vorname As MAVorname, MA.Geschlecht, MA.AHV_Nr_New, MA.AHV_Nr, MA.GebDat, MA.Nationality, "
		sSql += "(Select Count(*) From MA_KIAddress Where MANr = MA.MANr) As MAKIAnz, "
		sSql += "(Select Top 1 Bruttolohn From LO Where LO.LONr = LOL.LONr And MANr = MA.MANr And LO.Jahr = LOL.Jahr And LO.LP = LOL.LP) As BruttolohnBasis "
		sSql += "From LOL "
		sSql += "Left Join Mitarbeiter MA On LOL.MANr = MA.MANr "


		Return sSql

	End Function

	Function GetQuerySQLString(ByVal sSQLQuery As String) As String
		Dim sSql As String = String.Empty
		Dim sOldQuery As String = sSQLQuery
		Dim strFieldName As String = String.Empty

		Dim FilterBez As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strUSFiliale As String = m_InitialData.UserData.UserFiliale
		Dim iSQLLen As Integer = Len(sSQLQuery)

		Dim strName As String()
		Dim strMyName As String = String.Empty


		' Lohnartennummer ----------------------------------------------------------------------------------------------

		strFieldName = "LOL.Lanr"
		'sZusatzBez = "3600, 3602, 3650, 3700, 3750, 3800, 3850, 3900, 3901"
		sZusatzBez = "3600, 3602, 3650, 3700, 3750, 3800, 3850, 3900, 3900.1, 3901, 3901.1"

		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		sSql &= String.Format("{0} LOL.MDNr In ({1}) ", strAndString, m_InitialData.MDData.MDNr)

		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		sSql += strAndString & strFieldName & " In (" & sZusatzBez & ")"

		' Monat von -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "LOL.LP"

		If m_SearchCriteria.firstMonth = m_SearchCriteria.lastMonth Then
			FilterBez += "Monat = " & m_SearchCriteria.firstMonth & vbLf
			sSql &= String.Format("{0} {1} In ({2}) ", strAndString, strFieldName, m_SearchCriteria.firstMonth)

		ElseIf m_SearchCriteria.firstMonth <> m_SearchCriteria.lastMonth Then
			FilterBez += String.Format("Monat zwischen {1} und {2}{0} ", vbNewLine, m_SearchCriteria.firstMonth, m_SearchCriteria.lastMonth)
			sSql &= String.Format("{0} {1} Between {2} And {3} ", strAndString, strFieldName, m_SearchCriteria.firstMonth, m_SearchCriteria.lastMonth)

		End If

		' Jahr -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "LOL.Jahr"
		FilterBez += "Jahr = " & m_SearchCriteria.firstYear & vbLf
		sSql &= String.Format("{0} {1} In ({2}) ", strAndString, strFieldName, m_SearchCriteria.firstYear)

		' Kanton -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "LOL.S_Kanton"
		If Not String.IsNullOrWhiteSpace(m_SearchCriteria.s_kanton) Then
			sZusatzBez = m_SearchCriteria.s_kanton
			FilterBez += "Kanton wie (" & sZusatzBez & ") " & vbLf

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			If InStr(sZusatzBez.Trim, ",") > 0 Then
				sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
			End If

			sSql += strAndString & strFieldName & " In ('" & sZusatzBez.Trim & "')"

		End If

		' Filiale -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "MA.Kst"
		If Not String.IsNullOrWhiteSpace(m_SearchCriteria.filiale) Then
			sZusatzBez = m_SearchCriteria.filiale
			FilterBez += "Filiale wie (" & sZusatzBez & ") " & vbLf

			sZusatzBez = GetFilialKstData(sZusatzBez)
			If Not String.IsNullOrWhiteSpace(sZusatzBez) Then

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


		' Nationalität -------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		strFieldName = "MA.Nationality"
		If Not String.IsNullOrWhiteSpace(m_SearchCriteria.nationality) Then
			sZusatzBez = m_SearchCriteria.nationality
			FilterBez += "Nationalität wie (" & sZusatzBez & ") " & vbLf

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			sSql += strAndString & strFieldName & " In ('" & sZusatzBez.Trim & "')"

		End If

		' Filialen Teilung...
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		If Not String.IsNullOrWhiteSpace(strUSFiliale) Then
			strFieldName = "MA.Kst"
			If UCase(strUSFiliale) <> String.Empty Then
				sZusatzBez = strUSFiliale
				FilterBez += "Filiale wie (" & sZusatzBez & ") " & vbLf

				sZusatzBez = GetFilialKstData(sZusatzBez)
				sZusatzBez = Replace(sZusatzBez, "'", "")
				strName = Regex.Split(sZusatzBez.Trim, ",")
				strMyName = String.Empty
				For i As Integer = 0 To strName.Length - 1
					strMyName += CStr(IIf(strMyName.Length > 0, " Or ", "")) & strFieldName & " Like '%" & strName(i).Trim & "%'"
				Next
				If strName.Length > 0 Then sZusatzBez = strMyName

				sSql += strAndString & " (" & sZusatzBez & ")"

			End If
		End If

		' Nuller Datensätze nicht drucken
		strAndString = IIf(String.IsNullOrWhiteSpace(sSql), String.Empty, " And ")
		sSql &= String.Format("{0} LOL.m_Btr <> 0", strAndString)

		ClsDataDetail.GetFilterBez = FilterBez

		Return sSql

	End Function

	Function GetSortString() As String
		Dim strSort As String = " Order By "
		Dim strSortBez As String = String.Empty
		Dim strMyName As String = String.Empty

		strMyName = "MA.Nachname ASC, MA.Vorname ASC, LOL.LP ASC"
		strSortBez = CStr(IIf(strSortBez.Length > 0, ", ", "")) & "Mitarbeiternummer, Monat"

		strSort = strSort & strMyName
		ClsDataDetail.GetSortBez = strSortBez

		Return strSort
	End Function


#End Region


End Class

