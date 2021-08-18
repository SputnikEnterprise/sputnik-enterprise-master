
Imports System.Text.RegularExpressions
Imports SP.DatabaseAccess.Listing.DataObjects

Partial Class ClsDbFunc

	Private m_AssignedKST As String


	Function LoadPayrollData_Staging(ByVal strKst3Bez As String, ByVal i4What As Integer, ByVal dataType As DB1DataRecordType) As IEnumerable(Of SP.DatabaseAccess.Listing.DataObjects.DB1PayrollData)
		Dim FilterBez As String = String.Empty
		Dim strLOLQuery As String = String.Empty
		Dim amount As Decimal = 0D


		m_AssignedKST = strKst3Bez

		Try
			'If m_AssignedKST.ToLower.Contains("rgu") Then Trace.WriteLine(m_AssignedKST)

			If i4What = -6 Then
				strLOLQuery = GetLOLQuery_ReportData_Staging()
			Else
				strLOLQuery = GetLOLQuery4Vars_Staging(i4What)
			End If

			' FilterBez zuordnen
			FilterBez += String.Format("Von {0}/{1} bis {2}/{3} {4}", m_SearchCriteria.FirstMonth, m_SearchCriteria.FirstYear, m_SearchCriteria.LastMonth, m_SearchCriteria.LastYear, vbLf)

			Dim payrollData = m_ListingDatabaseAccess.LoadDB1PayrollData(strLOLQuery, dataType)
			For Each itm In payrollData
				amount += itm.Amount
			Next

			Return payrollData

		Catch ex As Exception
			m_Logger.LogError(String.Format("GetLOLBetrag: {0}", ex.ToString))

			Return Nothing
		End Try

	End Function

	Function GetLOLBetrag_Staging(ByVal strKst3Bez As String, ByVal i4What As Integer, ByVal dataType As DB1DataRecordType) As Decimal
		Dim FilterBez As String = String.Empty
		Dim strLOLQuery As String = String.Empty
		Dim amount As Decimal = 0D

		m_AssignedKST = strKst3Bez

		Try
			'If m_AssignedKST.ToLower.Contains("rgu") Then Trace.WriteLine(m_AssignedKST)

			If i4What = -6 Then
				strLOLQuery = GetLOLQuery_ReportData_Staging()
			Else
				strLOLQuery = GetLOLQuery4Vars_Staging(i4What)
			End If

			' FilterBez zuordnen
			FilterBez += String.Format("Von {0}/{1} bis {2}/{3} {4}", m_SearchCriteria.FirstMonth, m_SearchCriteria.FirstYear, m_SearchCriteria.LastMonth, m_SearchCriteria.LastYear, vbLf)

			Dim payrollData = m_ListingDatabaseAccess.LoadDB1PayrollData(strLOLQuery, dataType)
			For Each itm In payrollData
				amount += itm.Amount
			Next


			Return amount

		Catch ex As Exception
			m_Logger.LogError(String.Format("GetLOLBetrag: {0}", ex.ToString))
			Return 0
		End Try

	End Function

	Function GetLOLQuery4Vars_Staging(ByVal i4What As Integer) As String
		Dim result As String

		result = "Select lol.lonr, lol.kst, lol.lanr, Sum(IsNull(LOL.M_Btr, 0)) As m_btr From dbo.LOL "
		result &= "Left Join dbo.LA On LOL.LANr = LA.Lanr And LA.LAJahr = LOL.Jahr "
		result &= "Left Join dbo.Mitarbeiter MA On LOL.MANr = MA.MANr "
		result &= "Left Join dbo.Kunden KD On LOL.DestKDNr = KD.KDNr "
		result &= "Where "


		Dim whereQuery As String
		whereQuery = GetQueryParam4LOLDb_Staging(i4What)
		whereQuery = String.Format("LOL.MDNr = {0} {1} {2}", m_InitializationData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(whereQuery), "And", ""), whereQuery)
		Dim commonFilter = BuildCommonFilterCriterias(whereQuery, i4What)

		result = String.Format("{0} {1} Group By lol.lonr, lol.kst, lol.lanr Order By LOL.LONr, LOL.LANr ", result, commonFilter)


		Return result
	End Function

	Function GetQueryParam4LOLDb_Staging(ByVal i4What As Integer) As String
		Dim result As String = String.Empty

		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strMyName As String = String.Empty
		Dim strFieldName As String = String.Empty

		If i4What <= -12 Then Trace.WriteLine(String.Format("i4What: {0}", i4What))
		'If m_AssignedKST.ToLower.Contains("bh/sg") Then Trace.WriteLine(m_AssignedKST)


		result += "LOL.m_Btr <> 0 "
		Select Case i4What
			Case -1        ' Bruttolohn
				result += "And LA.DB1_Bruttopflichtig = 1 "

			Case -2    ' AHV-Lohn
				result += "And LA.DB1_AHVpflichtig = 1 "

			Case -3    ' Ferback
				result += String.Format("And LOL.LANr = {0} ", If(ClsDataDetail.IsFerienAsNetto_2, 8000.02, 602)) ' "And LOL.LANr = 602 "

			Case -4    ' Feierback
				result += String.Format("And LOL.LANr = {0} ", If(ClsDataDetail.IsFeiertagAsNetto_2, 8000.01, 502)) '502 "

			Case -5   ' 13. Lohnback
				result += String.Format("And LOL.LANr = {0} ", If(ClsDataDetail.Is13LohnAsNetto_2, 8000.03, 702)) ' "And LOL.LANr = 702 "

			Case -7    ' FerAus
				result += "And LOL.LANr = 660 "

			Case -8    ' FeierAus
				result += "And LOL.LANr = 560 "

			Case -9    ' 13. LohnAus
				result += "And LOL.LANr = 760 "

			Case -10    ' Gleitzeit Aus
				result += "And LOL.LANr = 800 "

			Case -13    ' Fremdleistungen
				result += "And (LA.Sum0Betrag = '36' or LA.Sum1Betrag = '36' or LA.Sum2Betrag = '36' or LA.Sum3Betrag = '36') "
				result += "And (LA.Sum0Betrag Is Not Null And LA.Sum1Betrag Is Not Null And LA.Sum2Betrag Is Not Null "
				result &= "And LA.Sum3Betrag Is Not Null) "

			Case -20    ' AG-Anteile; AHV-Basis und AG-Beitrag
				result += "And LA.DB1_AHVPflichtig = 1 "

			Case -21    ' AG-Anteile; AHV-pflichtige Lohnarten
				result += "And LA.DB1_AHVPflichtig = 1 "

			Case -22    ' AG-Anteile; EO-Entschädigung
				result += "And LOL.LANr In (2500) "


			Case Else
				'Return String.Empty

		End Select


		' Monat Von/Bis + Jahr Von/Bis ----------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(result), String.Empty, " And ")
		Dim jahrVon = m_SearchCriteria.FirstYear
		Dim jahrBis = m_SearchCriteria.LastYear

		result &= strAndString & String.Format("((LOL.Jahr = {0} And LOL.LP >= {1} And ", jahrVon, m_SearchCriteria.FirstMonth)
		result &= String.Format("({0} <> {1} Or ", jahrVon, jahrBis)
		result &= String.Format("(LOL.LP >= {0} And LOL.LP <= {1}))) Or ", m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)
		result &= String.Format("(LOL.Jahr > {0} And LOL.Jahr < {1}) Or ", jahrVon, jahrBis)
		result &= String.Format("(LOL.Jahr = {0} And LOL.LP <= {1} And ", jahrBis, m_SearchCriteria.LastMonth)
		result &= String.Format("({0} <> {1} Or ", jahrVon, jahrBis)
		result &= String.Format("(LOL.LP >= {0} And LOL.LP <= {1})))) ", m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)


		Return result
	End Function

	Private Function BuildCommonFilterCriterias(ByVal sqlQuery As String, ByVal i4What As Integer) As String
		Dim result As String = String.Empty
		Dim strAndString As String
		Dim strFieldName As String
		Dim sZusatzBez As String
		Dim FilterBez As New List(Of String)
		Dim strName As String()
		Dim strMyName As String

		' 3. KST -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sqlQuery), String.Empty, " And ")
		strFieldName = CStr(IIf(i4What = -6, "RP.RPKST", "LOL.KST"))
		If UCase(m_AssignedKST) <> String.Empty Then
			sZusatzBez = m_AssignedKST
			FilterBez.Add("Berater wie (" & sZusatzBez & ") ")

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName

			If InStr(UCase(sZusatzBez), UCase("Leere Felder")) > 0 Then
				sqlQuery += strAndString & " (" & strFieldName & " = '' Or " & strFieldName & " Is Null) "
			Else
				If InStr(sZusatzBez, ",") > 0 Then sZusatzBez = Replace(sZusatzBez, ",", "','")

				sqlQuery += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If

		End If

		' Branche -----------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sqlQuery), String.Empty, " And ")
		' i4What = -6 heisst Kompensationszeiten von Gleitzeit, daher muss es vom RP nehmen
		strFieldName = CStr(IIf(i4What = -6, "RP.KDBranche", "LOL.ESBranche"))
		If UCase(ClsDataDetail.GetFormVars(7).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(7).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, "# ", "#")
			FilterBez.Add("Einsatzbranche wie (" & sZusatzBez & ") ")

			If InStr(ClsDataDetail.GetFormVars(7).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sqlQuery += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sqlQuery += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If

		' Kanton -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sqlQuery), String.Empty, " And ")
		strFieldName = "KD.PLZ"
		If UCase(ClsDataDetail.GetFormVars(8).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(8).ToString.Trim

			strName = Regex.Split(sZusatzBez.Trim, ",")
			strMyName = String.Empty
			For i As Integer = 0 To strName.Length - 1
				strMyName += CStr(IIf(strMyName.Length > 0, ",", "")) & strName(i).ToString.Trim
			Next
			If strName.Length > 0 Then sZusatzBez = strMyName
			FilterBez.Add("Kanton wie (" & sZusatzBez & ") ")

			If InStr(sZusatzBez.Trim, ",") > 0 Then
				sZusatzBez = Replace(sZusatzBez.Trim, ",", "','")
			End If
			sqlQuery += strAndString & strFieldName & " In ('" & GetKantonPLZ(sZusatzBez.Trim) & "')"

		End If

		' Ort -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sqlQuery), String.Empty, " And ")
		strFieldName = "KD.Ort"
		If UCase(ClsDataDetail.GetFormVars(9).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(9).ToString.Trim.Replace("'", "''")
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez.Add("Ort wie (" & sZusatzBez & ") ")

			If InStr(ClsDataDetail.GetFormVars(9).ToString, "#") > 0 Then sZusatzBez = Replace(sZusatzBez, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sqlQuery += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sqlQuery += strAndString & strFieldName & " In ('" & sZusatzBez & "')"
			End If
		End If

		' Nationalität --------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sqlQuery), String.Empty, " And ")
		strFieldName = "MA.Nationality"
		If UCase(ClsDataDetail.GetFormVars(10).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(10).ToString.Trim
			sZusatzBez = Replace(GetLandCode(sZusatzBez), ", ", ",")
			FilterBez.Add("Nationalität wie (" & sZusatzBez & ") ")

			If InStr(sZusatzBez, "#") > 0 Then sZusatzBez = Replace(sZusatzBez.Trim, "#", "','")
			sqlQuery += strAndString & strFieldName & " "
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sqlQuery += "Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sqlQuery += "In ('" & sZusatzBez & "')"
			End If
		End If

		' Land -------------------------------------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(sqlQuery), String.Empty, " And ")
		strFieldName = "MA.Land"
		If UCase(ClsDataDetail.GetFormVars(11).ToString) <> String.Empty Then
			sZusatzBez = ClsDataDetail.GetFormVars(11).ToString.Trim
			sZusatzBez = Replace(sZusatzBez, ", ", ",")
			FilterBez.Add("Land wie (" & sZusatzBez & ") ")

			If InStr(sZusatzBez, "#") > 0 Then sZusatzBez = Replace(sZusatzBez.Trim, "#", "','")
			If sZusatzBez.Contains("*") Or sZusatzBez.Contains("%") Then
				sqlQuery += strFieldName & " Like '" & Replace(sZusatzBez, "*", "%") & "'"
			Else
				sqlQuery += strAndString & strFieldName & " In ('" & GetLandCode(sZusatzBez) & "')"
			End If
		End If


		' MANr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.employeeNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(sqlQuery), String.Empty, " And ")
			strFieldName = "MA.MANr"
			sZusatzBez = m_SearchCriteria.employeeNumber
			FilterBez.Add("Kandidatennummer wie (" & sZusatzBez & ") ")

			sqlQuery += String.Format("{0}{1} IN ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If

		' KDNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.customerNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(sqlQuery), String.Empty, " And ")
			strFieldName = "KD.KDNr"
			sZusatzBez = m_SearchCriteria.customerNumber
			FilterBez.Add("Kundennummer wie (" & sZusatzBez & ") ")

			sqlQuery += String.Format("{0}{1} IN ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If

		' ESNr -------------------------------------------------------------------------------------------------------
		If m_SearchCriteria.esNumber.GetValueOrDefault(0) > 0 Then
			strAndString = IIf(String.IsNullOrWhiteSpace(sqlQuery), String.Empty, " And ")
			strFieldName = CStr(IIf(i4What = -6, "RP.ESNr", "LOL.DestESNr"))
			sZusatzBez = m_SearchCriteria.esNumber
			FilterBez.Add("Einsatznummer wie (" & sZusatzBez & ") ")

			sqlQuery += String.Format("{0}{1} IN ({2}) ", strAndString, strFieldName, sZusatzBez)
		End If

		result = sqlQuery

		'ClsDataDetail.GetFilterBez = FilterBez


		Return result
	End Function

End Class
