Imports System.Text

Partial Public Class EmployeePayroll

  Private Sub GetQSTGemeinde()

    Dim qstGemeinde As String = String.Empty

    If Not m_EmployeeData.QSTCommunity Is Nothing Then
      qstGemeinde = m_EmployeeData.QSTCommunity.Trim()
    End If

    strQstGemeinde = qstGemeinde
  End Sub

  Private Sub InitialGAVArayy()

    '' Zuerst die Variable (1. Element) mit GAV-Berufen deklarieren...
    'aGAVBerufe(0) = "Ausbaugewerbe Westschweiz"
    'aGAVBerufe(1) = "Bauhauptgewerbe"
    'aGAVBerufe(2) = "Carrosseriegewerbe"
    'aGAVBerufe(3) = "Coiffeurgewerbe"
    'aGAVBerufe(4) = "Dach- und Wandgewerbe"
    'aGAVBerufe(5) = "Decken- und Innenausbausysteme"
    'aGAVBerufe(6) = "Elektro- /Telekom. Installation"
    'aGAVBerufe(7) = "Gastgewerbe"
    'aGAVBerufe(8) = "Gebäudetechnik"
    'aGAVBerufe(9) = "Gerüstbau"
    'aGAVBerufe(10) = "Isoliergewerbe"
    'aGAVBerufe(11) = "Maler- und Gipsergewerbe"
    'aGAVBerufe(12) = "Marmor- und Granitgewerbe"
    'aGAVBerufe(13) = "Metzgereigewerbe"
    'aGAVBerufe(14) = "Möbelindustrie"
    'aGAVBerufe(15) = "Plattenlegergewerbe"
    'aGAVBerufe(16) = "Reinigungsbranche Deutschschweiz"
    'aGAVBerufe(17) = "Reinigungssektor der Westschweiz"
    'aGAVBerufe(18) = "Sicherheitsdienst privat"
    'aGAVBerufe(19) = "Zahntechnik"
    'aGAVBerufe(20) = "Betonwaren-Industrie"
    'aGAVBerufe(21) = "Geleisebau"
    'aGAVBerufe(22) = "Gipser Zürich*"
    'aGAVBerufe(23) = "Industrie- und Unterlagsböden"
    'aGAVBerufe(24) = "Schreinereigewerbe"
    'aGAVBerufe(25) = "Ziegelindustrie"
    'aGAVBerufe(26) = "Metallgewerbe"
    'aGAVBerufe(27) = "Gärtnergewerbe"
    'aGAVBerufe(28) = "Elektro- / Telekom. Installation"
    'aGAVBerufe(29) = "Maschinenbaugewerbe"

    ' GAV-Kantone
    aGAVKanton(0) = "FL"
    aGAVKanton(1) = "AG"
    aGAVKanton(2) = "AI"
    aGAVKanton(3) = "AR"
    aGAVKanton(4) = "BE"
    aGAVKanton(5) = "BL"
    aGAVKanton(6) = "BS"
    aGAVKanton(7) = "FR"
    aGAVKanton(8) = "GE"
    aGAVKanton(9) = "GL"
    aGAVKanton(10) = "GR"
    aGAVKanton(11) = "JU"
    aGAVKanton(12) = "LU"
    aGAVKanton(13) = "NE"
    aGAVKanton(14) = "NW"
    aGAVKanton(15) = "OW"
    aGAVKanton(16) = "SG"
    aGAVKanton(17) = "SH"
    aGAVKanton(18) = "SO"
    aGAVKanton(19) = "SZ"
    aGAVKanton(20) = "TG"
    aGAVKanton(21) = "TI"
    aGAVKanton(22) = "UR"
    aGAVKanton(23) = "VD"
    aGAVKanton(24) = "VS"
    aGAVKanton(25) = "ZG"
    aGAVKanton(26) = "ZH"

  End Sub

  Private Sub InitialALLGAVGruppe0()
		Dim gavGruppe0(50) As String
		Dim gavNumber(50) As Integer
		Dim i As Integer = 0

		For i = 0 To gavGruppe0.Count - 1
			gavGruppe0(i) = String.Empty
			gavNumber(i) = 0
		Next

		Dim data = m_PayrollDatabaseAccess.LoadGAVGruppe0Data(LPMonth, LPYear, m_EmployeeData.EmployeeNumber, MDNr)
		If data Is Nothing Then
			Throw New Exception("GAV Gruppe 0 Daten konnten nicht geladen werden.")
		End If

		i = 0
		For Each itm In data
			gavNumber(i + 1) = itm.GAVNumber
			gavGruppe0(i + 1) = itm.GAVGruppe0

			i += 1
		Next

		aGAVPVLNumber = gavNumber
		aGAVBerufe = gavGruppe0

	End Sub

  Private Sub InitialALLGAVGruppe1()
    Dim data = m_PayrollDatabaseAccess.LoadGAVGruppe1Data(LPMonth, LPYear, m_EmployeeData.EmployeeNumber, MDNr)

    If data Is Nothing Then
      Throw New Exception("GAV Gruppe 1 Daten konnten nicht geladen werden.")
    End If

    aGGruppe1 = data

  End Sub

  Private Sub CheckForBVG()
    Dim rTemprec As Boolean?
    Dim dStartofMonth As Date
    Dim dStartofMonth_BVG As Date
    Dim dEndofMonth As Date

    dStartofMonth = CDate("01." & LPMonth & "." & LPYear)
    'dEndofMonth = CDate(DateAdd("m", 1, dStartofMonth - Day(dStartofMonth) + 1)) - 1

    dStartofMonth_BVG = DateAdd("m", -3, dStartofMonth)
    dEndofMonth = CDate(DateAdd("m", 1, dStartofMonth_BVG.AddDays(-dStartofMonth_BVG.Day + 1))).AddDays(-1)

    rTemprec = m_PayrollDatabaseAccess.CheckIfESExistsForCheckForBVG(m_EmployeeData.EmployeeNumber, MDNr, dStartofMonth, dEndofMonth)
    ThrowExceptionOnError(Not rTemprec.HasValue, "Einsatzdaten konnten nicht geladen werden für Check For BVG.")

    If rTemprec.Value > 0 Then
			If S(13) + S(24) = 0 AndAlso S(7) + S(2) <> 0 Then
				Dim msg_1 = "Möglicherweise sollte der Kandidat BVG-beitragspflichtig sein." & vbLf &
																				 "Bitte überprüfen Sie die Lohnabrechnung " & LONewNr & " von " & IIf(m_EmployeeData.Gender = "M", "Herrn", "Frau") &
																				 " " & m_EmployeeData.Lastname & ", " & m_EmployeeData.Firstname & "."
				WriteToProtocol(Padright("M -> (AddLMLOLrec): ", 30, " ") & msg_1)
				' TODO:
				'm_TaskHelper.InUIAndWait(Function()
				'                               m_UtilityUI.ShowOKDialog("Möglicherweise sollte der Kandidat BVG-beitragspflichtig sein." & vbLf & _
				'                                     "Bitte überprüfen Sie die Lohnabrechnung " & LONewNr & " von " & IIf(m_EmployeeData.Gender = "M", "Herrn", "Frau") & _
				'                                     " " & m_EmployeeData.Lastname & ", " & m_EmployeeData.Firstname & ".", "BVG-Überprüfung", vbExclamation, MessageBoxIcon.Information)

				'                               Return True
				'                             End Function)


			End If
		End If

  End Sub

  Private Sub GetGAVKTGProz(ByVal iGAVNumber As Integer, _
                     ByRef cANProz As Decimal, _
                     ByRef cAGProz As Decimal, _
                     ByRef bKTG60_ As Boolean)

    Dim cTempKTGANProz As Decimal
    Dim cTempKTGAGProz As Decimal
    Dim bGAVNrIsAnhang1 As Boolean

    bGAVNrIsAnhang1 = InStr(1, Anhang1GAV & ",", iGAVNumber & ",") > 0

    Dim err As Boolean = False
		Dim md_kk_lmv_data = m_PayrollDatabaseAccess.LoadMD_KK_LMV_Data(MDNr, LPYear, iGAVNumber, err)

		If err Then
      Throw New Exception("MD_KK_LMV Daten konnten nicht geladen werden.")
      Return
    End If

    If md_kk_lmv_data Is Nothing Then
      If m_EmployeeData.Gender = "M" Then
        If U(4) <> 0 Then
          cTempKTGANProz = m_MandantData.KK_An_MA
          cTempKTGAGProz = m_MandantData.KK_AG_MA
        Else
          cTempKTGANProz = m_MandantData.KK_An_MZ
          cTempKTGAGProz = m_MandantData.KK_AG_MZ
        End If
      Else
        If U(4) <> 0 Then
          cTempKTGANProz = m_MandantData.KK_An_WA
          cTempKTGAGProz = m_MandantData.KK_AG_WA
        Else
          cTempKTGANProz = m_MandantData.KK_An_WZ
          cTempKTGAGProz = m_MandantData.KK_AG_WZ
        End If
      End If

    Else
      If bIs60KTGDay And bGAVNrIsAnhang1 Then
        bIs60KTGDayOK = True
        If m_EmployeeData.Gender = "M" Then
          If U(4) <> 0 Then
            cTempKTGANProz = IIf(md_kk_lmv_data.KK_AN_MA_Proz = 0, m_MandantData.KK_An_MA, md_kk_lmv_data.KK_AN_MA_Proz)
            cTempKTGAGProz = IIf(md_kk_lmv_data.KK_AG_MA_Proz = 0, m_MandantData.KK_AG_MA, md_kk_lmv_data.KK_AG_MA_Proz)
          Else
            cTempKTGANProz = IIf(md_kk_lmv_data.KK_AN_MZ_Proz = 0, m_MandantData.KK_An_MZ, md_kk_lmv_data.KK_AN_MZ_Proz)
            cTempKTGAGProz = IIf(md_kk_lmv_data.KK_AG_MZ_Proz = 0, m_MandantData.KK_AG_MZ, md_kk_lmv_data.KK_AG_MZ_Proz)
          End If

        Else
          If U(4) <> 0 Then
            cTempKTGANProz = IIf(md_kk_lmv_data.KK_AN_WA_Proz = 0, m_MandantData.KK_An_WA, md_kk_lmv_data.KK_AN_WA_Proz)
            cTempKTGAGProz = IIf(md_kk_lmv_data.KK_AG_WA_Proz = 0, m_MandantData.KK_AG_WA, md_kk_lmv_data.KK_AG_WA_Proz)
          Else
            cTempKTGANProz = IIf(md_kk_lmv_data.KK_AN_WZ_Proz = 0, m_MandantData.KK_An_WZ, md_kk_lmv_data.KK_AN_WZ_Proz)
            cTempKTGAGProz = IIf(md_kk_lmv_data.KK_AG_WZ_Proz = 0, m_MandantData.KK_AG_WZ, md_kk_lmv_data.KK_AG_WZ_Proz)
          End If
        End If
        bKTG60_ = True

      Else
        bIs60KTGDayOK = False
        If m_EmployeeData.Gender = "M" Then
          If U(4) <> 0 Then
            cTempKTGANProz = IIf(md_kk_lmv_data.KK_AN_MA_Proz_72 = 0, m_MandantData.KK_An_MA, md_kk_lmv_data.KK_AN_MA_Proz_72)
            cTempKTGAGProz = IIf(md_kk_lmv_data.KK_AG_MA_Proz_72 = 0, m_MandantData.KK_AG_MA, md_kk_lmv_data.KK_AG_MA_Proz_72)
          Else
            cTempKTGANProz = IIf(md_kk_lmv_data.KK_AN_MZ_Proz_72 = 0, m_MandantData.KK_An_MZ, md_kk_lmv_data.KK_AN_MZ_Proz_72)
            cTempKTGAGProz = IIf(md_kk_lmv_data.KK_AG_MZ_Proz_72 = 0, m_MandantData.KK_AG_MZ, md_kk_lmv_data.KK_AG_MZ_Proz_72)
          End If
        Else
          If U(4) <> 0 Then
            cTempKTGANProz = IIf(md_kk_lmv_data.KK_AN_WA_Proz_72 = 0, m_MandantData.KK_An_WA, md_kk_lmv_data.KK_AN_WA_Proz_72)
            cTempKTGAGProz = IIf(md_kk_lmv_data.KK_AG_WA_Proz_72 = 0, m_MandantData.KK_AG_WA, md_kk_lmv_data.KK_AG_WA_Proz_72)
          Else
            cTempKTGANProz = IIf(md_kk_lmv_data.KK_AN_WZ_Proz_72 = 0, m_MandantData.KK_An_WZ, md_kk_lmv_data.KK_AN_WZ_Proz_72)
            cTempKTGAGProz = IIf(md_kk_lmv_data.KK_AG_WZ_Proz_72 = 0, m_MandantData.KK_AG_WZ, md_kk_lmv_data.KK_AG_WZ_Proz_72)
          End If
        End If

      End If
    End If

    cANProz = cTempKTGANProz
    cAGProz = cTempKTGAGProz

  End Sub

	Private Function GetGAVNr(ByVal strGAVGruppe0 As String) As Integer
		Dim i As Integer
		Dim iGAVNr As Integer

		' Kompensiert leer Zeichen welche von VB 6.0 Funktion DivFunc.vFieldVal entfernt wird
		strGAVGruppe0 = strGAVGruppe0.Trim()

		iGAVNr = -1
		For i = 0 To 50
			If UCase(aGAVBerufe(i).Trim) = UCase(strGAVGruppe0) Then
				iGAVNr = i

				Exit For
			End If

		Next i

		Return Math.Max(-1, iGAVNr)

	End Function

	Private Function GetGruppe1Nr(ByVal strGAVGruppe1 As String) As Integer
		Dim i As Integer
		Dim iGruppe1Nr As Integer

		' Kompensiert leer Zeichen welche von VB 6.0 Funktion DivFunc.vFieldVal entfernt wird
		strGAVGruppe1 = strGAVGruppe1.Trim()

		iGruppe1Nr = -1
		For i = 0 To 26
			If UCase(aGGruppe1(i).Trim) = UCase(strGAVGruppe1) Then
				iGruppe1Nr = i

				Exit For
			End If

		Next i

		Return Math.Max(-1, iGruppe1Nr)

	End Function

	Private Function GetGAVKantonNr(ByVal strGAVKanton As String) As Integer
    Dim i As Integer
    Dim iGAVNr As Integer

    ' Kompensiert leer Zeichen welche von VB 6.0 Funktion DivFunc.vFieldVal entfernt wird
    strGAVKanton = strGAVKanton.Trim()

    iGAVNr = -1
    For i = 0 To 26
			If UCase(aGAVKanton(i).Trim) = UCase(strGAVKanton) Then
				iGAVNr = i

				Exit For
			End If

		Next i

		Return Math.Max(-1, iGAVNr)

  End Function

  ' TODO  New String("0", Digit) prüfen
  Public Function NumberRound(varZahl As Object, Optional ByVal Digit As Integer = 0) As Decimal

    If Digit = 0 Then Digit = 4
    'Auf 0,05 Rappen runden
    NumberRound = Convert.ToDecimal(Format(CLng(varZahl / 0.05) * 0.05, "0." & New String("0", Digit)))

  End Function

  Private Function FindNewLONr() As Integer

    Dim DefinedNr As Integer

    DefinedNr = ReadPayrollOffsetFromSettings() 'Val(DivReg.GetINIString(MDIniFullname,   LoadResString(395), LoadResString(394)))

    Dim newLONr As Integer? = m_PayrollDatabaseAccess.FindNewLONr(DefinedNr)
    ThrowExceptionOnError(Not newLONr.HasValue, "Neue LONr konnte nicht gefunden werden.")

		Return newLONr.Value

  End Function

	''' <summary>
	''' Reads the payroll offset from the settings.
	''' </summary>
	''' <returns>ES offset or zero if it could not be read.</returns>
	Private Function ReadPayrollOffsetFromSettings() As Integer

    Dim strQuery As String = "//StartNr/Lohnabrechnung"

    Dim esNumberStartNumberSetting As String = m_md.GetSelectedMDProfilValue(MDNr, LPYear, "StartNr", "Lohnabrechnung", 0)
    Dim intVal As Integer

    If Integer.TryParse(esNumberStartNumberSetting, intVal) Then
      Return intVal
    Else
      Return 0
    End If

  End Function

	Function FindNewLMNr() As Integer

    Dim newLMNr As Integer? = m_PayrollDatabaseAccess.FindNewLMNr
    ThrowExceptionOnError(Not newLMNr.HasValue, "Neue LMNr konnte nicht gefunden werden.")

    FindNewLMNr = newLMNr
  End Function

  Function GetLastLMID() As Integer

    GetLastLMID = m_PayrollDatabaseAccess.LoadHighestLMIDForSaveFinalData

  End Function

	Public Function Padright(ByVal Tempwert As Object, ByVal StrAfter As Integer, ByVal FillStr As String) As String

		Padright = Tempwert & New String(FillStr, StrAfter - Math.Min(Len(Tempwert), StrAfter))

	End Function

	Public Sub WriteToProtocol(ByVal strData As String)

		If strData <> "" Then m_Protocol.AppendLine("<br>" & Now & vbTab & strData)

	End Sub

End Class