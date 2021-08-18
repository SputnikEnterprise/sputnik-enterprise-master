Partial Public Class EmployeePayroll

  Private Function AgeYear(ByVal iSelYear As Integer, ByVal dGebdat As Date) As Integer

    AgeYear = iSelYear - Year(dGebdat)

  End Function

	Private Function Get_BVGAns(ByVal dMAGebDat As Date, ByVal strGeschlecht As String) As Decimal

		Dim cResult As Decimal
		Dim ProzentSatz As Decimal?
		Dim iAlter As Integer
		Dim sMsgText As String

		If Val(m_EmployeeLOSetting.BVGCode) = 2 Then
			iAlter = 100

		ElseIf Val(m_EmployeeLOSetting.BVGCode) = 3 Then
			iAlter = 101

		ElseIf Val(m_EmployeeLOSetting.BVGCode) <> 0 Then
			iAlter = AgeYear(Int(LPYear), dMAGebDat)

		End If

		If strGeschlecht = "M" Then
			ProzentSatz = m_PayrollDatabaseAccess.LoadBVGAnsMForPayroll(iAlter, LPYear, MDNr)
		Else
			ProzentSatz = m_PayrollDatabaseAccess.LoadBVGAnsWForPayroll(iAlter, LPYear, MDNr)
		End If

		If Not ProzentSatz.HasValue Then
			sMsgText = String.Format("Fehler in Ihrer BVG-Datenbank. Der BVG-Ansatz kann nicht ermittelt werden. Der Mitarbeiter ist {0} Jahre alt.", iAlter)

			WriteToProtocol(Padright("M -> (Get_BVGAns): ", 30, " ") & sMsgText)

			cResult = 0

		Else
			cResult = ProzentSatz.Value

		End If
		strOriginData &= String.Format("<br>BVG-Prozent für Alter {0:f0} = {1:n2}<br>", iAlter, cResult)


		Return cResult

	End Function

	Private Function Get_BVGAns_WithRentner(ByVal dMAGebDat As Date, ByVal strGeschlecht As String) As Decimal

		Dim cResult As Decimal
		Dim ProzentSatz As Decimal?
		Dim iAlter As Integer
		Dim sMsgText As String

		If Val(m_EmployeeLOSetting.BVGCode) = 2 Then
			iAlter = 100

		ElseIf Val(m_EmployeeLOSetting.BVGCode) = 3 Then
			iAlter = 101

		ElseIf Val(m_EmployeeLOSetting.BVGCode) <> 0 Then
			iAlter = AgeYear(Int(LPYear), dMAGebDat)

		End If

		If IsRentner Then
			Return 0

		Else

			If strGeschlecht = "M" Then
				ProzentSatz = m_PayrollDatabaseAccess.LoadBVGAnsMForPayroll(iAlter, LPYear, MDNr)
			Else
				ProzentSatz = m_PayrollDatabaseAccess.LoadBVGAnsWForPayroll(iAlter, LPYear, MDNr)
			End If

		End If

		If Not ProzentSatz.HasValue Then
			sMsgText = String.Format("Fehler in Ihrer BVG-Datenbank. Der BVG-Ansatz kann nicht ermittelt werden. Der Mitarbeiter ist {0} Jahre alt.", iAlter)

			WriteToProtocol(Padright("M -> (Get_BVGAns_WithRentner): ", 30, " ") & sMsgText)

			cResult = 0

		Else
			cResult = ProzentSatz.Value

		End If
		strOriginData &= String.Format("<br>BVG-Prozent für Alter {0:f0} = {1:n2}<br>", iAlter, cResult)


		Return cResult

	End Function

	Private Function Get_AnzZGABank(ByVal lMANr As Integer) As Decimal
    Dim rTemprec As Integer?
    Dim iAnzAuBank As Integer
    Dim cBasLO As Decimal
    Dim cBasZG As Decimal
    Dim cBetragZG As Decimal
    Dim cBetragLO As Decimal
    Dim err As Boolean

    Dim advancepaymenttransferinternationalfee As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/advancepaymenttransferinternationalfee", m_PayrollSetting))
    Dim payrolltransferinternationalfee As String = m_path.GetXMLNodeValue(m_md.GetSelectedMDDataXMLFilename(MDNr, LPYear), String.Format("{0}/payrolltransferinternationalfee", m_PayrollSetting))

    cBasZG = Val(advancepaymenttransferinternationalfee) 'Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), LoadResString(110)))
    cBasLO = Val(payrolltransferinternationalfee) 'Val(DivReg.GetINIString(MDIniFullname, LoadResString(377), LoadResString(111)))

    rTemprec = m_PayrollDatabaseAccess.LoadAnzZGABank(m_EmployeeData.EmployeeNumber, MDNr, LONewNr)

    If rTemprec.HasValue Then
      iAnzAuBank = rTemprec.Value
      ' Betrag für Vorschussüberweisung
      cBetragZG = iAnzAuBank * cBasZG
    End If

    iAnzAuBank = 0

    If m_EmployeeLOSetting.Zahlart = "K" Then

      Dim aktivBankData = m_PayrollDatabaseAccess.LoadAktivMABankDataForPayroll(m_EmployeeData.EmployeeNumber, err)
      ThrowExceptionOnError(err, "Aktive Bankdaten konnten nicht geladen werden.")

      If Not aktivBankData Is Nothing Then
        If aktivBankData.BnkAu.HasValue AndAlso aktivBankData.BnkAu.Value Then
          iAnzAuBank = 1
          ' Betrag für Lohnüberweisung
          cBetragLO = iAnzAuBank * cBasLO
        End If
      End If
    End If
		S(14) = cBetragZG + cBetragLO


		Return (cBetragZG + cBetragLO)

	End Function

  ' Guthaben von Feiertag Jahresübergreifend...
  Function Get_FeierGuthaben(ByVal iMANr As Integer) As Decimal

    Dim feiertagGuthaben = m_PayrollDatabaseAccess.LoadFeiertagGuthaben(iMANr)
    ThrowExceptionOnError(feiertagGuthaben Is Nothing, "Feiertag Guthaben konnte nicht geladen werden.")

    ' Rückstellung - Auszahlung = Das Guthaben von Ferien
    Get_FeierGuthaben = (-1 * feiertagGuthaben.BackedBetrag) - feiertagGuthaben.PayedBetrag

  End Function

  ' Guthaben von Feiertag Jahresübergreifend...
  Function Get_FeierGuthaben_1(ByVal iMANr As Integer) As Decimal

    Dim feiertagGuthaben1 = m_PayrollDatabaseAccess.LoadFeiertagGuthaben1(iMANr)
    ThrowExceptionOnError(feiertagGuthaben1 Is Nothing, "Feiertag Guthaben1 konnte nicht geladen werden.")

    ' Rückstellung - Auszahlung = Das Guthaben von Ferien
    Get_FeierGuthaben_1 = (-1 * feiertagGuthaben1.BackedBetrag) - feiertagGuthaben1.PayedBetrag

  End Function

  ' Guthaben von Ferien Jahresübergreifend...
  Function Get_FerGuthaben(ByVal iMANr As Integer) As Decimal

    Dim ferienGuthaben = m_PayrollDatabaseAccess.LoadFerienGuthaben(iMANr)
    ThrowExceptionOnError(ferienGuthaben Is Nothing, "Ferien Guthaben konnte nicht geladen werden.")

    ' Rückstellung - Auszahlung = Das Guthaben von Feiertag
    Get_FerGuthaben = (-1 * ferienGuthaben.BackedBetrag) - ferienGuthaben.PayedBetrag

  End Function

  ' Guthaben von Ferien Jahresübergreifend...
  ' Guthaben von Ferien Jahresübergreifend (LANr: 629.10)...
  ' Wenn lESNr > 0 dann kommt pro
  Function Get_FerGuthaben_1(ByVal iMANr As Integer) As Decimal

    Dim ferienGuthaben1 = m_PayrollDatabaseAccess.LoadFerienGuthaben1(iMANr)
    ThrowExceptionOnError(ferienGuthaben1 Is Nothing, "Ferien Guthaben1 konnte nicht geladen werden.")

    ' Rückstellung - Auszahlung = Das Guthaben von Feiertag
    Get_FerGuthaben_1 = (-1 * ferienGuthaben1.BackedBetrag) - ferienGuthaben1.PayedBetrag

  End Function

  ' Guthaben von 13. Monatslohn Jahresübergreifend...
  Function Get_13Guthaben(ByVal iMANr As Integer) As Decimal

    Dim lohn13Guthaben = m_PayrollDatabaseAccess.Load13LohnGuthaben(iMANr)
    ThrowExceptionOnError(lohn13Guthaben Is Nothing, "Lohn 13 Guthaben konnte nicht geladen werden.")

    'Rückstellung - Auszahlung = Das Guthaben von 13. Lohn
    Get_13Guthaben = (-1 * lohn13Guthaben.BackedBetrag) - lohn13Guthaben.PayedBetrag

  End Function

  ' Guthaben von 13. Monatslohn Jahresübergreifend...
  Function Get_13Guthaben_1(ByVal iMANr As Long) As Decimal

    Dim lohn13Guthaben1 = m_PayrollDatabaseAccess.Load13LohnGuthaben1(iMANr)
    ThrowExceptionOnError(lohn13Guthaben1 Is Nothing, "Lohn 13 Guthaben1 konnte nicht geladen werden.")

    ' Rückstellung - Auszahlung = Das Guthaben von 13. Lohn
    Get_13Guthaben_1 = (-1 * lohn13Guthaben1.BackedBetrag) - lohn13Guthaben1.PayedBetrag

  End Function

End Class
