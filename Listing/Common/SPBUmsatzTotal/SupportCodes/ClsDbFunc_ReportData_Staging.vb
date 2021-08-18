

Imports System.Text.RegularExpressions
Imports SP.DatabaseAccess.Listing.DataObjects

Partial Class ClsDbFunc


	Function GetLOLQuery_ReportData_Staging() As String
		Dim result As String = String.Empty

		result = "Select RP.lonr, RPL.lanr, Sum(IsNull(RPL.KompBetrag, 0)*(-1)) As m_btr From dbo.RPL "
		result &= "Left Join dbo.RP On RPL.RPNr = RP.RPNr "
		result &= "Left Join dbo.Mitarbeiter MA On RPL.MANr = MA.MANr "
		result &= "Left Join dbo.Kunden KD On RPL.KDNr = KD.KDNr "
		result &= "Where "

		Dim whereQuery As String
		whereQuery = GetQueryLOLDb_ReportData_Staging()
		whereQuery = String.Format("RP.MDNr = {0} {1} {2}", m_InitializationData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(whereQuery), "And", ""), whereQuery)
		Dim commonFilter = BuildCommonFilterCriterias(whereQuery, -6)

		result = String.Format("{0} {1} Group By RP.LONr, RPL.LANr Order By RP.LONr, RPL.LANr ", result, commonFilter)

		Return result
	End Function

	Function GetQueryLOLDb_ReportData_Staging() As String
		Dim result As String = String.Empty

		Dim FilterBez As String = String.Empty
		Dim strAndString As String = String.Empty

		Dim strMyName As String = String.Empty
		Dim strFieldName As String = String.Empty

		If m_AssignedKST.ToLower.Contains("bh/sg") Then
			'Trace.WriteLine(strSelectedKst3Bez)
		End If

		result += "RPL.m_Betrag <> 0 "
		result += "And IsNull(RPL.KompBetrag, 0) <> 0 And IsNull(RP.LONr, 0) <> 0 "

		' Monat Von/Bis + Jahr Von/Bis ----------------------------------------------------------------------------
		strAndString = IIf(String.IsNullOrWhiteSpace(result), String.Empty, " And ")
		Dim jahrVon = m_SearchCriteria.FirstYear
		Dim jahrBis = m_SearchCriteria.LastYear

		result &= strAndString & String.Format("((RP.Jahr = {0} And RP.Monat >= {1} And ", jahrVon, m_SearchCriteria.FirstMonth)
		result &= String.Format("({0} <> {1} Or ", jahrVon, jahrBis)
		result &= String.Format("(RP.Monat >= {0} And RP.Monat <= {1}))) Or ", m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)

		result &= String.Format("(RP.Jahr > {0} And RP.Jahr < {1}) Or ", jahrVon, jahrBis)
		result &= String.Format("(RP.Jahr = {0} And RP.Monat <= {1} And ", jahrBis, m_SearchCriteria.LastMonth)
		result &= String.Format("({0} <> {1} Or ", jahrVon, jahrBis)
		result &= String.Format("(RP.Monat >= {0} And RP.Monat <= {1})))) ", m_SearchCriteria.FirstMonth, m_SearchCriteria.LastMonth)



		Return result
	End Function



End Class
