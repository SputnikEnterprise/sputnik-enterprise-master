
Imports System.Text.RegularExpressions
Imports SP.DatabaseAccess.Listing.DataObjects

Partial Class ClsDbFunc

	Private m_AGdata_Staging As List(Of SP.DatabaseAccess.Listing.DataObjects.DB1PayrollAGAnteilData)

	Function LoadPayrollDataForCalculatingAG_Staging(ByVal strKst3Bez As String) As IEnumerable(Of SP.DatabaseAccess.Listing.DataObjects.DB1PayrollData)
		Dim result As List(Of SP.DatabaseAccess.Listing.DataObjects.DB1PayrollData) = Nothing
		Dim FilterBez As String = String.Empty
		Dim strLOLQuery As String = String.Empty
		Dim amount As Decimal = 0D
		Dim agAmount As Decimal = 0D
		Dim d7100 As Decimal = 0D
		Dim d7995 As Decimal = 0D
		Dim agdata As New DB1PayrollData With {.agemployeeAmount = 0, .ahvemployeeAmount = 0, .payrollNumber = 0, ._agAmount = 0, ._ahvAmount = 0}


		m_AssignedKST = strKst3Bez

		Try
			strLOLQuery = GetPayrollNumbersForCalculatingAG_Staging()

			' FilterBez zuordnen
			FilterBez += String.Format("Von {0}/{1} bis {2}/{3} {4}", m_SearchCriteria.FirstMonth, m_SearchCriteria.FirstYear, m_SearchCriteria.LastMonth, m_SearchCriteria.LastYear, vbLf)

			Dim payrollData = m_ListingDatabaseAccess.LoadDB1PayrollData(strLOLQuery, Nothing)

			result = New List(Of SP.DatabaseAccess.Listing.DataObjects.DB1PayrollData)
			m_AGdata_Staging = New List(Of DB1PayrollAGAnteilData)

			For Each itm In payrollData

				Dim data = New SP.DatabaseAccess.Listing.DataObjects.DB1PayrollData With {.PayrollNumber = itm.PayrollNumber}

				Dim loAGData = m_ListingDatabaseAccess.LoadPayrollAGData(itm.PayrollNumber, m_SearchCriteria.employeeNumber, m_SearchCriteria.customerNumber, m_SearchCriteria.esNumber)
				If loAGData Is Nothing OrElse loAGData.AHVAnteil.GetValueOrDefault(0) = 0 Then Continue For

				If m_SearchCriteria.employeeNumber.GetValueOrDefault(0) + m_SearchCriteria.customerNumber.GetValueOrDefault(0) + m_SearchCriteria.esNumber.GetValueOrDefault(0) = 0 Then
					d7100 = loAGData.AHVAnteil    ' AHV-Basis
					d7995 = loAGData.AGAnteil       ' Arbeitgeberbeiträge gesamt

				Else

					d7100 = loAGData.AHVAmountEachEmployee    ' AHV-Basis
					d7995 = loAGData.AGAmountEachEmployee   ' Arbeitgeberbeiträge gesamt
				End If

				agdata.agemployeeAmount = loAGData.AGAmountEachEmployee
				agdata.agemployeebvgAmount = loAGData.AGBVGAmountEachEmployee
				agdata.ahvemployeeAmount = loAGData.AHVAmountEachEmployee
				agdata._agAmount = loAGData.AGAnteil
				agdata._ahvAmount = loAGData.AHVAnteil
				agdata.KST = m_AssignedKST

				Dim agdata_Staging As New DB1PayrollAGAnteilData With {.PayrollNumber = itm.PayrollNumber}
				agdata_Staging.AGAmountEachEmployee = loAGData.AGAmountEachEmployee
				agdata_Staging.AGBVGAmountEachEmployee = loAGData.AGBVGAmountEachEmployee
				agdata_Staging.AHVAmountEachEmployee = loAGData.AHVAmountEachEmployee
				agdata_Staging.AGAnteil = loAGData.AGAnteil
				agdata_Staging.AHVAnteil = loAGData.AHVAnteil



				Dim sqlQuery = GetLOLKSTQuery_Staging(itm.PayrollNumber)
				Dim loKSTDb1AHVAmount = m_ListingDatabaseAccess.LoadDB1LOLAmount(sqlQuery)
				If loKSTDb1AHVAmount.GetValueOrDefault(0) = 0 Then Continue For

				Dim KSTProz As Decimal = loKSTDb1AHVAmount.GetValueOrDefault(0) / d7100
				Dim sozialabzugAGKST As Decimal = d7995 * KSTProz



				Dim sqlEOQuery = GetLOLEOKstQuery_Staging(itm.PayrollNumber)
				Dim loKSTEOAmount = m_ListingDatabaseAccess.LoadDB1LOLAmount(sqlEOQuery)
				If loKSTEOAmount.GetValueOrDefault(0) <> 0 Then
					sozialabzugAGKST = sozialabzugAGKST - (loKSTEOAmount * 0.3081 * KSTProz)
				End If

				data.Amount = sozialabzugAGKST


				agdata_Staging.AGKSTProcent = KSTProz
				agdata_Staging.AGEOAmount = loKSTEOAmount


				amount = sozialabzugAGKST


				result.Add(data)
				m_AGdata_Staging.Add(agdata_Staging)

			Next

			Return result

		Catch ex As Exception
			m_Logger.LogError(String.Format("GetLOLBetrag: {0}", ex.ToString))

			Return Nothing
		End Try

	End Function


	Function GetPayrollNumbersForCalculatingAG_Staging() As String
		Dim i4What As Integer = -20
		Dim result As String

		result = "Select CONVERT(DECIMAL(8, 5),	0) AS LANr, CONVERT(DECIMAL(8, 5),	0) AS m_btr, LOL.LONr From Dbo.LOL "
		result &= "Left Join Dbo.LA On LOL.LANr = LA.LANr And LA.LAJahr = LOL.Jahr "
		result &= "Left Join Dbo.Mitarbeiter MA On LOL.MANr = MA.MANr "
		result &= "Left Join Dbo.Kunden KD On LOL.DestKDNr = KD.KDNr "
		result &= "Where "

		Dim whereQuery As String
		whereQuery = GetQueryParam4LOLDb_Staging(i4What)
		whereQuery = String.Format("LOL.MDNr = {0} {1} {2}", m_InitializationData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(whereQuery), "And", ""), whereQuery)
		Dim commonFilter = BuildCommonFilterCriterias(whereQuery, i4What)

		result = String.Format("{0} {1} Group By lol.lonr Order By LOL.LONr ", result, commonFilter)


		Return result
	End Function

	'Function getKstLOAGAnteil_Staging(ByVal iLONr As Integer) As Double

	'	Dim strQuery As String = String.Empty
	'	Dim data As New DB1PayrollData With {.agemployeeAmount = 0, .ahvemployeeAmount = 0, .payrollNumber = iLONr, ._agAmount = 0, ._ahvAmount = 0}
	'	Dim dAGBeitrag As Decimal = 0
	'	Dim cmd As System.Data.SqlClient.SqlCommand
	'	Dim dTotalPflichtigLAKst As Decimal = 0
	'	Dim dEOKSTBetrag As Decimal = 0

	'	Dim LOAGData = _clsSQLString.GetSQLString4AGDb(iLONr)

	'	Try

	'		If LOAGData Is Nothing Then Return dAGBeitrag
	'		If LOAGData.AHVAnteil = 0 Then Return dAGBeitrag

	'		Dim d7100 = If(m_SearchCriteria.employeeNumber.GetValueOrDefault(0) + m_SearchCriteria.customerNumber.GetValueOrDefault(0) + m_SearchCriteria.esNumber.GetValueOrDefault(0) = 0,
	'									 LOAGData.AHVAnteil,
	'									 LOAGData.AHVAmountEachEmployee)    ' AHV-Basis
	'		Dim d7995 = If(m_SearchCriteria.employeeNumber.GetValueOrDefault(0) + m_SearchCriteria.customerNumber.GetValueOrDefault(0) + m_SearchCriteria.esNumber.GetValueOrDefault(0) = 0,
	'									LOAGData.AGAnteil,
	'									LOAGData.AGAmountEachEmployee) ' Arbeitgeberbeiträge gesamt

	'		data.agemployeeAmount = LOAGData.AGAmountEachEmployee
	'		data.agemployeebvgAmount = LOAGData.AGBVGAmountEachEmployee
	'		data.ahvemployeeAmount = LOAGData.AHVAmountEachEmployee
	'		data._agAmount = LOAGData.AGAnteil
	'		data._ahvAmount = LOAGData.AHVAnteil
	'		data.KST = strKst3Bez


	'		strQuery = GetLOLKSTQuery_Staging(iLONr)
	'		cmd = New System.Data.SqlClient.SqlCommand(strQuery, ClsDataDetail.Conn)
	'		Dim rLOLrec As SqlDataReader = cmd.ExecuteReader
	'		While rLOLrec.Read
	'			dTotalPflichtigLAKst = Val(rLOLrec("TotalBtr").ToString)
	'		End While
	'		rLOLrec.Close()
	'		If dTotalPflichtigLAKst = 0 Then Return dAGBeitrag

	'		Dim dKSTProz As Decimal = dTotalPflichtigLAKst / d7100
	'		Dim dSozialabzugAGKST As Decimal = d7995 * dKSTProz



	'		' SozialabzugAGKST ist ein negativer Betrag...
	'		dEOKSTBetrag = getKstLOEOAnteil(strKst3Bez, iLONr, strFMonth, strLMonth, strVYear, strBYear)
	'		If dEOKSTBetrag <> 0 Then
	'			dSozialabzugAGKST = dSozialabzugAGKST - (dEOKSTBetrag * 0.3081 * dKSTProz)
	'		End If
	'		dAGBeitrag = dSozialabzugAGKST

	'	Catch ex As Exception

	'	Finally
	'		m_DB1PayrollData.Add(data)

	'	End Try

	'	Return dAGBeitrag
	'End Function


	Function GetLOLKSTQuery_Staging(ByVal iLONr As Integer) As String
		Dim i4What As Integer = -21
		Dim result As String

		result = "Select IsNull(Sum(LOL.m_Btr), 0) As m_btr From LOL Left Join LA On "
		result += "LOL.LANr = LA.LANr And LA.LAJahr = LOL.Jahr "
		result &= "Left Join Mitarbeiter MA On LOL.MANr = MA.MANr "
		result &= "Left Join Kunden KD On LOL.DestKDNr = KD.KDNr "
		result &= "Where "

		Dim whereQuery As String
		whereQuery = GetQueryParam4LOLDb_Staging(i4What)
		whereQuery = String.Format("LOL.MDNr = {0} AND LOL.LONr = {2} {1} {3}", m_InitializationData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(whereQuery), "AND", ""), iLONr, whereQuery)
		Dim commonFilter = BuildCommonFilterCriterias(whereQuery, i4What)

		result = String.Format("{0} {1} ", result, commonFilter)


		Return result
	End Function

	' LAPflichtige Daten pro Lohnabrechnung; EO-Entschädigung
	Function GetLOLEOKstQuery_Staging(ByVal iLONr As Integer) As String
		Dim i4What As Integer = -22
		Dim result As String

		result = "Select Sum(IsNull(LOL.m_Btr, 0)) As m_btr From LOL "
		result &= "Left Join Mitarbeiter MA On LOL.MANr = MA.MANr "
		result &= "Left Join Kunden KD On LOL.DestKDNr = KD.KDNr "
		result &= "Where "

		Dim whereQuery As String
		whereQuery = GetQueryParam4LOLDb_Staging(i4What)
		whereQuery = String.Format("LOL.MDNr = {0} AND LOL.LONr = {2} {1} {3}", m_InitializationData.MDData.MDNr, If(Not String.IsNullOrWhiteSpace(whereQuery), "AND", ""), iLONr, whereQuery)
		Dim commonFilter = BuildCommonFilterCriterias(whereQuery, i4What)

		result = String.Format("{0} {1} ", result, commonFilter)


		Return result
	End Function


End Class
