
Imports SP.DatabaseAccess.PayrollMng.DataObjects

Partial Public Class EmployeePayroll

	Private Sub GetRPGAV_U_Var(ByVal laData As LAData,
					  ByVal cTempAnz As Decimal,
					  ByVal cTempwert As Decimal,
					  ByVal SUVACode As String,
					  ByVal ModulName As String,
					  Optional ByVal Far As Boolean = False)
		Dim cFANProz As Decimal
		Dim cFAGProz As Decimal

		Dim cWANProz As Decimal
		Dim cWAGProz As Decimal
		Dim cVANProz As Decimal
		Dim cVAGProz As Decimal

		Dim cWANsBetrag As Decimal
		Dim cWAGsBetrag As Decimal
		Dim cVANsBetrag As Decimal
		Dim cVAGsBetrag As Decimal

		Dim cWANmBetrag As Decimal
		Dim cWAGmBetrag As Decimal
		Dim cVANmBetrag As Decimal
		Dim cVAGmBetrag As Decimal

		Dim cGleitBetrag As Decimal
		Dim cGleitStd As Decimal

		Dim cKTGANProz As Decimal
		Dim cKTGAGProz As Decimal
		Dim bKTG60 As Boolean

		Dim iGAVNr As Integer
		Dim iGruppe1Nr As Integer
		Dim iGAVKantonNr As Integer

		Dim cAnzStunden As Decimal
		Dim cAHVLohn As Decimal
		Dim cSUVALohn As Decimal
		Dim cKTGLohn As Decimal
		Dim bGleitTime As Boolean


		If LPMonth < 4 And LPYear <= 2006 Then Return
		With currRplData
			If currRplData.RPGAV_Nr <= 1 OrElse cTempwert = 0 Then Return

			iGAVNr = GetGAVNr(currRplData.RPGAV_Beruf)
			iGruppe1Nr = GetGruppe1Nr(currRplData.RPGAV_Gruppe1)
			iGAVKantonNr = GetGAVKantonNr(currRplData.RPGAV_Kanton)

			If laData.LANr = 500 AndAlso m_EmployeeLOSetting.FeierBack AndAlso m_IsBackBrutto Then Return
			If laData.LANr = 600 AndAlso m_EmployeeLOSetting.FerienBack AndAlso m_IsBackBrutto Then Return
			If laData.LANr = 700 AndAlso m_EmployeeLOSetting.Lohn13Back AndAlso m_IsBackBrutto Then Return

			'If laData.LANr = 500 AndAlso m_EmployeeLOSetting.FeierBack AndAlso U(12) = 0 Then Return
			'If laData.LANr = 600 AndAlso m_EmployeeLOSetting.FerienBack AndAlso U(12) = 0 Then Return
			'If laData.LANr = 700 AndAlso m_EmployeeLOSetting.Lohn13Back AndAlso U(12) = 0 Then Return

			'If laData.LANr = 500 AndAlso m_EmployeeLOSetting.FeierBack Then Return
			'If laData.LANr = 600 AndAlso m_EmployeeLOSetting.FerienBack Then Return
			'If laData.LANr = 700 AndAlso m_EmployeeLOSetting.Lohn13Back Then Return

			If laData.GleitTime Then bGleitTime = True

			cFANProz = currRplData.RPGAV_FAN
			cFAGProz = currRplData.RPGAV_FAG

			' 'Arbeitnehmer, Weiterbildungen...
			If currRplData.RPGAV_WAN > 0 Then
				cWANProz = currRplData.RPGAV_WAN

			ElseIf currRplData.RPGAV_WAN_S > 0 Then
				cWANsBetrag = currRplData.RPGAV_WAN_S

			ElseIf currRplData.RPGAV_WAN_M > 0 Then
				cWANmBetrag = currRplData.RPGAV_WAN_M

			End If

			' Arbeitgeber
			If currRplData.RPGAV_WAG > 0 Then
				cWAGProz = currRplData.RPGAV_WAG

			ElseIf currRplData.RPGAV_WAG_S > 0 Then
				cWAGsBetrag = currRplData.RPGAV_WAG_S

			ElseIf currRplData.RPGAV_WAG_M > 0 Then
				cWAGmBetrag = currRplData.RPGAV_WAG_M

			End If


			' Arbeitnehmer, Vollzugskosten...
			If currRplData.RPGAV_VAN > 0 Then cVANProz = currRplData.RPGAV_VAN ' in %

			If currRplData.RPGAV_VAN_S > 0 Then cVANsBetrag = currRplData.RPGAV_VAN_S '* Val(DivFunc.vFieldVal(!m_Anzahl))     ' in Stunden

			If currRplData.RPGAV_VAN_M > 0 Then cVANmBetrag = currRplData.RPGAV_VAN_M ' in Monat

			' Kompensierte Stunden...
			If currRplData.KompStd > 0 Then cGleitStd = currRplData.KompStd ' in RP KompStd
			If currRplData.KompBetrag > 0 Then cGleitBetrag = currRplData.KompBetrag ' in RP KompBetrag


			' Arbeitgeber
			If currRplData.RPGAV_VAG > 0 Then cVAGProz = currRplData.RPGAV_VAG ' in %

			If currRplData.RPGAV_VAG_S > 0 Then cVAGsBetrag = currRplData.RPGAV_VAG_S     ' in Stunden

			If currRplData.RPGAV_VAG_M > 0 Then cVAGmBetrag = currRplData.RPGAV_VAG_M ' in Monat

			Call GetGAVKTGProz(currRplData.RPGAV_Nr, cKTGANProz, cKTGAGProz, bKTG60)
		End With

		With laData
			If .AHVPflichtig Then cAHVLohn = cTempwert
			If .NBUVPflichtig Then cSUVALohn = cTempwert
			If .KKPflichtig Then cKTGLohn = cTempwert

			'If (.Sum0Anzahl = 2 OrElse .Sum1Anzahl = 2) And .ProTag Then cAnzStunden = cTempAnz
			If (.Sum0Anzahl = 2 OrElse .Sum1Anzahl = 2) OrElse (.Sum0Anzahl = -2 OrElse .Sum1Anzahl = -2) Then
				If (.Sum0Anzahl = -2 OrElse .Sum1Anzahl = -2) Then
					cAnzStunden = cTempAnz * (-1)
				Else
					cAnzStunden = cTempAnz
				End If
				cTempAnz = cAnzStunden
			End If

		End With

		' FAR ist von AHV-pflichtigen Lohn
		If cFANProz <> 0 Then cFANBasis = cFANBasis + cAHVLohn
		If cFAGProz <> 0 Then cFAGBasis = cFAGBasis + cAHVLohn

		' Parifond ist von SUVA-pflichtigen Lohn
		' Diese Daten können in Stunden oder auch Monat abgezogen werden...
		' Arbeitnehmer, Weiterbildungsfonds
		If laData.LANr = 103.01 OrElse laData.GroupKey = 152 Then
			cWANProz = 0
			cWANsBetrag = 0
			cWANmBetrag = 0
			cWAGProz = 0
			cWAGsBetrag = 0
			cWAGmBetrag = 0

			cVANProz = 0
			cVANsBetrag = 0
			cVANmBetrag = 0
			cVAGProz = 0
			cVAGsBetrag = 0
			cVAGmBetrag = 0

		Else
			If cWANProz <> 0 Then cWANBasis = cWANBasis + If(LPYear > 2016, cAHVLohn, cSUVALohn)
			If cWANsBetrag <> 0 AndAlso cAnzStunden <> 0 Then cWANsBetrag = cWANsBetrag Else cWANsBetrag = 0
			If cWANmBetrag <> 0 Then cWANmBetrag = cWANmBetrag

			' Arbeitgeber, Weiterbildungsfonds
			If cWAGProz <> 0 Then cWAGBasis = cWAGBasis + If(LPYear > 2016, cAHVLohn, cSUVALohn)
			If cWAGsBetrag <> 0 AndAlso cAnzStunden <> 0 Then cWAGsBetrag = cWAGsBetrag Else cWAGsBetrag = 0
			If cWAGmBetrag <> 0 Then cWAGmBetrag = cWAGmBetrag

			' Arbeitnehmer, Vollzugskosten
			If cVANProz <> 0 Then cVANBasis = cVANBasis + If(LPYear > 2016, cAHVLohn, cSUVALohn)
			If cVANsBetrag <> 0 AndAlso cAnzStunden <> 0 Then cVANsBetrag = cVANsBetrag Else cVANsBetrag = 0
			If cVANmBetrag <> 0 Then cVANmBetrag = cVANmBetrag

			' Arbeitgeber, Vollzugsfonds
			If cVAGProz <> 0 Then cVAGBasis = cVAGBasis + If(LPYear > 2016, cAHVLohn, cSUVALohn)
			If cVAGsBetrag <> 0 AndAlso cAnzStunden <> 0 Then cVAGsBetrag = cVAGsBetrag Else cVAGsBetrag = 0
			If cVAGmBetrag <> 0 Then cVAGmBetrag = cVAGmBetrag

		End If

		cGAVKTGANBasis = cGAVKTGANBasis + cKTGLohn
		cGAVKTGAGBasis = cGAVKTGAGBasis + cKTGLohn

		aGAVKKAN(iGAVNr) = cGAVKTGANBasis
		aGAVKKAG(iGAVNr) = cGAVKTGAGBasis

		aGAVFAN(iGAVNr) = cFANBasis
		aGAVFAG(iGAVNr) = cFAGBasis

		aGAVWAN(iGAVNr) = cWANBasis
		aGAVWAG(iGAVNr) = cWAGBasis
		aGAVVAN(iGAVNr) = cVANBasis
		aGAVVAG(iGAVNr) = cVAGBasis

		Trace.WriteLine(String.Format("LANr: {0} >>> Wert: {1}", laData.LANr, cTempwert))
		Trace.WriteLine(String.Format("LANr: {0} >>> cVANBasis: {1}", laData.LANr, cVANBasis))
		Trace.WriteLine(String.Format("LANr: {0} >>> cVAGBasis: {1}", laData.LANr, cVAGBasis))
		Trace.WriteLine(String.Format("-------"))

		With currRplData
			' die Komplette Auflistung...
			If cAnzStunden <> 0 Then AddToGAVStd(currRplData.RPGAV_Beruf,
										currRplData.RPGAV_Gruppe1,
										iGAVNr,
										iGruppe1Nr,
										cTempAnz, 100,
										currRplData.RPGAV_Kanton, iGAVKantonNr, bGleitTime)

			' FAR AG...
			If cFAGProz <> 0 Then AddToFAG(currRplData.RPGAV_Beruf,
										iGAVNr,
										cAHVLohn, cFAGProz,
										currRplData.RPGAV_Kanton, iGAVKantonNr)

			' AN
			If cFANProz <> 0 Then AddToFAN(currRplData.RPGAV_Beruf,
										iGAVNr,
										cAHVLohn, cFANProz,
										currRplData.RPGAV_Kanton, iGAVKantonNr)

			' Weiterbildungskosten AG...
			If cWAGProz <> 0 Then AddToWAG(currRplData.RPGAV_Beruf,
																			currRplData.RPGAV_Gruppe1,
																			iGAVNr,
																			iGruppe1Nr,
																			If(LPYear > 2016, cAHVLohn, cSUVALohn), cWAGProz,
																			currRplData.RPGAV_Kanton, iGAVKantonNr)
			If cWAGsBetrag <> 0 Then AddToWAGStd(currRplData.RPGAV_Beruf,
										currRplData.RPGAV_Gruppe1,
										iGAVNr,
										iGruppe1Nr,
										cWAGsBetrag, 100,
										currRplData.RPGAV_Kanton, iGAVKantonNr)

			' AN
			If cWANProz <> 0 Then AddToWAN(currRplData.RPGAV_Beruf,
																			currRplData.RPGAV_Gruppe1,
																			iGAVNr,
																			iGruppe1Nr,
																			If(LPYear > 2016, cAHVLohn, cSUVALohn), cWANProz,
																			currRplData.RPGAV_Kanton, iGAVKantonNr)
			If cWANsBetrag <> 0 Then AddToWANStd(currRplData.RPGAV_Beruf,
										currRplData.RPGAV_Gruppe1,
										iGAVNr,
										iGruppe1Nr,
										cWANsBetrag, 100,
										currRplData.RPGAV_Kanton, iGAVKantonNr)

			' Vollzugskosten AG...
			If cVAGProz <> 0 Then AddToVAG(currRplData.RPGAV_Beruf,
																			currRplData.RPGAV_Gruppe1,
																			iGAVNr,
																			iGruppe1Nr,
																			If(LPYear > 2016, cAHVLohn, cSUVALohn), cVAGProz,
																			currRplData.RPGAV_Kanton, iGAVKantonNr)
			If cVAGsBetrag <> 0 Then AddToVAGStd(currRplData.RPGAV_Beruf,
										currRplData.RPGAV_Gruppe1,
										iGAVNr,
										iGruppe1Nr,
										cVAGsBetrag, 100,
										currRplData.RPGAV_Kanton, iGAVKantonNr)
			' AN
			If cVANProz <> 0 Then AddToVAN(currRplData.RPGAV_Beruf,
																			currRplData.RPGAV_Gruppe1,
																			iGAVNr,
																			iGruppe1Nr,
																			If(LPYear > 2016, cAHVLohn, cSUVALohn), cVANProz,
																			currRplData.RPGAV_Kanton, iGAVKantonNr)
			If cVANsBetrag <> 0 Then AddToVANStd(currRplData.RPGAV_Beruf,
										currRplData.RPGAV_Gruppe1,
										iGAVNr,
										iGruppe1Nr,
										cVANsBetrag, 100,
										currRplData.RPGAV_Kanton, iGAVKantonNr)

			If cGleitBetrag <> 0 Then AddToGleitBetrag(currRplData.RPGAV_Beruf,
										currRplData.RPGAV_Gruppe1,
										iGAVNr,
										iGruppe1Nr,
										Val(currRplData.KompBetrag), 100,
										currRplData.RPGAV_Kanton, iGAVKantonNr)

			If cGleitStd <> 0 Then AddToGleitStd(currRplData.RPGAV_Beruf,
										currRplData.RPGAV_Gruppe1,
										iGAVNr,
										iGruppe1Nr,
										Val(currRplData.KompStd), 100,
										currRplData.RPGAV_Kanton, iGAVKantonNr)


			' KTG AG...
			If cKTGAGProz <> 0 Then AddToKTGAG(currRplData.RPGAV_Beruf, iGAVNr,
										cKTGLohn, cKTGAGProz,
										currRplData.RPGAV_Kanton, iGAVKantonNr, bKTG60)

			' AN
			If cKTGANProz <> 0 Then AddToKTGAN(currRplData.RPGAV_Beruf, iGAVNr,
										cKTGLohn, cKTGANProz,
										currRplData.RPGAV_Kanton, iGAVKantonNr, bKTG60)

		End With
		Trace.WriteLine(String.Format("finished============="))

	End Sub

	Private Sub GetLMGAV_U_Var(ByVal LArec As LAData,
						ByVal cTempAnz As Decimal,
						ByVal cTempwert As Decimal,
						ByVal SUVACode As String,
						ByVal ModulName As String,
						Optional ByVal Far As Boolean = False)
		Dim cFANProz As Decimal
		Dim cFAGProz As Decimal
		Dim cWANProz As Decimal
		Dim cWAGProz As Decimal
		Dim cVANProz As Decimal
		Dim cVAGProz As Decimal
		Dim cWANsBetrag As Decimal
		Dim cWAGsBetrag As Decimal
		Dim cVANsBetrag As Decimal
		Dim cVAGsBetrag As Decimal
		Dim cWANmBetrag As Decimal
		Dim cWAGmBetrag As Decimal
		Dim cVANmBetrag As Decimal
		Dim cVAGmBetrag As Decimal
		Dim cKTGANProz As Decimal
		Dim cKTGAGProz As Decimal
		'Dim cKTGANProz60 As Currency
		'Dim cKTGAGProz60 As Currency
		Dim bKTG60 As Boolean
		Dim iGAVNr As Integer
		Dim iGruppe1Nr As Integer
		Dim iGAVKantonNr As Integer
		Dim cAnzStunden As Decimal
		Dim cAHVLohn As Decimal
		Dim cSUVALohn As Decimal
		Dim cKTGLohn As Decimal

		If LPMonth < 4 And LPYear <= 2006 Then Exit Sub
		With currLMData
			If Val(.GAVNr) <= 1 Or cTempwert = 0 Then Exit Sub

			iGAVNr = GetGAVNr(.GAVGruppe0)
			iGruppe1Nr = GetGruppe1Nr(.GAVGruppe1)
			iGAVKantonNr = GetGAVKantonNr(.GAVKanton)

			cFANProz = Val(.GAV_FAN)
			cFAGProz = Val(.GAV_FAG)

			' Weiterbildungen...
			If Val(.GAV_WAN) > 0 Then
				cWANProz = Val(.GAV_WAN)            ' Arbeitnehmer
			ElseIf Val(.GAV_WAN_S) > 0 Then
				cWANsBetrag = Val(.GAV_WAN_S)
			End If
			If Val(.GAV_WAG) > 0 Then
				cWAGProz = Val(.GAV_WAG)        ' Arbeitgeber
			ElseIf Val(.GAV_WAG_S) > 0 Then
				cWAGsBetrag = Val(.GAV_WAG_S)
			End If

			' Vollzugskosten...
			If Val(.GAV_VAN) > 0 Then
				cVANProz = Val(.GAV_VAN)        ' Arbeitnehmer, in %
			ElseIf Val(.GAV_VAN_S) > 0 Then
				cVANsBetrag = Val(.GAV_VAN_S)
			End If
			If Val(.GAV_VAG) > 0 Then
				cVAGProz = Val(.GAV_VAG)        ' Arbeitgeber in %
			ElseIf Val(.GAV_VAG_S) > 0 Then
				cVAGsBetrag = Val(.GAV_VAG_S)        ' Arbeitgeber in %
			End If

			If LArec.LANr = 1000 OrElse LArec.GroupKey = 152 Then      ' NUR wenn Monatslohn gibt...
				If Val(.GAV_WAN_M) > 0 Then cWANmBetrag = Val(.GAV_WAN_M) ' Arbeitnehmer
				If Val(.GAV_WAG_M) > 0 Then cWAGmBetrag = Val(.GAV_WAG_M) ' Arbeitgeber

				' Vollzugskosten...
				If Val(.GAV_VAN_M) > 0 Then cVANmBetrag = Val(.GAV_VAN_M) ' Arbeitnehmer, in %
				If Val(.GAV_VAG_M) > 0 Then cVAGmBetrag = Val(.GAV_VAG_M) ' Arbeitgeber in %
			End If

			Call GetGAVKTGProz(Val(.GAVNr), cKTGANProz, cKTGAGProz, bKTG60) ' DivFunc.vFieldVal(!GAVGruppe0)
		End With

		With LArec
			If .AHVPflichtig Then cAHVLohn = cTempwert
			If .NBUVPflichtig Then cSUVALohn = cTempwert
			If .KKPflichtig Then cKTGLohn = cTempwert
			If (.Sum0Anzahl = 2 OrElse .Sum1Anzahl = 2) Then cAnzStunden = cTempAnz
			If (.Sum0Anzahl = -2 OrElse .Sum1Anzahl = -2) Then cAnzStunden = cTempAnz * (-1)
		End With

		' FAR ist von AHV-pflichtigen Lohn
		If cFANProz <> 0 Then cFANBasis = cFANBasis + cAHVLohn
		If cFAGProz <> 0 Then cFAGBasis = cFAGBasis + cAHVLohn

		'' FAR ist von AHV-pflichtigen Lohn
		'If cFANProz <> 0 Then cFANBasis = cFANBasis + cAHVLohn
		'If cFAGProz <> 0 Then cFAGBasis = cFAGBasis + cAHVLohn


		If LArec.LANr = 1000.01 OrElse LArec.GroupKey = 152 Then
			If LArec.LANr = 1000.01 OrElse LArec.GroupKey = 152 Then cAHVLohn = cTempwert
			cFANProz = 0
			cFAGProz = 0

			' Arbeitnehmer, Vollzugskosten
			If cVANProz <> 0 Then cVANBasis = (cVANBasis * (1)) + If(LPYear > 2016, cAHVLohn, cSUVALohn)
			If cVANsBetrag <> 0 And cAnzStunden <> 0 Then cVANsBetrag = cVANsBetrag Else cVANsBetrag = 0

			' Arbeitgeber, Vollzugsfonds
			If cVAGProz <> 0 Then cVAGBasis = (cVAGBasis * (1)) + If(LPYear > 2016, cAHVLohn, cSUVALohn)
			If cVAGsBetrag <> 0 And cAnzStunden <> 0 Then cVAGsBetrag = cVAGsBetrag Else cVAGsBetrag = 0

		End If

		' Parifond ist von SUVA-pflichtigen Lohn
		' Diese Daten können in Stunden oder auch Monat abgezogen werden...
		' Arbeitnehmer, Weiterbildungsfonds
		If cWANProz <> 0 Then cWANBasis = cWANBasis + If(LPYear > 2016, cAHVLohn, cSUVALohn)
			If cWANsBetrag <> 0 And cAnzStunden <> 0 Then cWANsBetrag = cWANsBetrag Else cWANsBetrag = 0

			' Arbeitgeber, Weiterbildungsfonds
			If cWAGProz <> 0 Then cWAGBasis = cWAGBasis + If(LPYear > 2016, cAHVLohn, cSUVALohn)
			If cWAGsBetrag <> 0 And cAnzStunden <> 0 Then cWAGsBetrag = cWAGsBetrag Else cWAGsBetrag = 0

			' Arbeitnehmer, Vollzugskosten
			If cVANProz <> 0 Then cVANBasis = cVANBasis + If(LPYear > 2016, cAHVLohn, cSUVALohn)
			If cVANsBetrag <> 0 And cAnzStunden <> 0 Then cVANsBetrag = cVANsBetrag Else cVANsBetrag = 0

			' Arbeitgeber, Vollzugsfonds
			If cVAGProz <> 0 Then cVAGBasis = cVAGBasis + If(LPYear > 2016, cAHVLohn, cSUVALohn)
			If cVAGsBetrag <> 0 And cAnzStunden <> 0 Then cVAGsBetrag = cVAGsBetrag Else cVAGsBetrag = 0
		'End If


		'If bKTG60 Then
		'  cGAVKTGANBasis60 = cGAVKTGANBasis60 + cKTGLohn
		'  cGAVKTGAGBasis60 = cGAVKTGAGBasis60 + cKTGLohn
		'
		'  aGAVKKAN60(iGAVNr) = cGAVKTGANBasis60
		'  aGAVKKAG60(iGAVNr) = cGAVKTGAGBasis60
		'
		'Else
		cGAVKTGANBasis = cGAVKTGANBasis + cKTGLohn
		cGAVKTGAGBasis = cGAVKTGAGBasis + cKTGLohn

		aGAVKKAN(iGAVNr) = cGAVKTGANBasis
		aGAVKKAG(iGAVNr) = cGAVKTGAGBasis

		'End If

		aGAVFAN(iGAVNr) = cFANBasis
		aGAVFAG(iGAVNr) = cFAGBasis

		aGAVWAN(iGAVNr) = cWANBasis
		aGAVWAG(iGAVNr) = cWAGBasis
		aGAVVAN(iGAVNr) = cVANBasis
		aGAVVAG(iGAVNr) = cVAGBasis


		With currLMData
			If cAnzStunden <> 0 Then AddToGAVStd(.GAVGruppe0,
									   .GAVGruppe1,
										iGAVNr,
										iGruppe1Nr,
										cTempAnz, 100,
										.GAVKanton, iGAVKantonNr)

			' FAR AG...
			If cFAGProz <> 0 Then AddToFAG(.GAVGruppe0,
									   iGAVNr,
										cAHVLohn, cFAGProz,
										.GAVKanton, iGAVKantonNr)

			' AN
			If cFANProz <> 0 Then AddToFAN(.GAVGruppe0,
										iGAVNr,
										cAHVLohn, cFANProz,
										.GAVKanton, iGAVKantonNr)

			' Weiterbildungskosten AG...
			If cWAGProz <> 0 Then AddToWAG(.GAVGruppe0,
																			.GAVGruppe1,
																			iGAVNr,
																			iGruppe1Nr,
																			If(LPYear > 2016, cAHVLohn, cSUVALohn), cWAGProz,
																		 .GAVKanton, iGAVKantonNr)
			If cWAGsBetrag <> 0 Then AddToWAGStd(.GAVGruppe0,
										.GAVGruppe1,
										iGAVNr,
										iGruppe1Nr,
										cWAGsBetrag, 100,
										.GAVKanton, iGAVKantonNr)

			If cWAGmBetrag <> 0 Then AddToWAGM(.GAVGruppe0,
									   .GAVGruppe1,
										iGAVNr,
										iGruppe1Nr,
										cWAGmBetrag, 100,
									   .GAVKanton, iGAVKantonNr)

			' AN
			If cWANProz <> 0 Then AddToWAN(.GAVGruppe0,
																			.GAVGruppe1,
																			iGAVNr,
																			iGruppe1Nr,
																			If(LPYear > 2016, cAHVLohn, cSUVALohn), cWANProz,
																		 .GAVKanton, iGAVKantonNr)
			If cWANsBetrag <> 0 Then AddToWANStd(.GAVGruppe0,
									   .GAVGruppe1,
										iGAVNr,
										iGruppe1Nr,
										cWANsBetrag, 100,
									   .GAVKanton, iGAVKantonNr)
			If cWANmBetrag <> 0 Then AddToWANM(.GAVGruppe0,
										.GAVGruppe1,
										iGAVNr,
										iGruppe1Nr,
										cWANmBetrag, 100,
										.GAVKanton, iGAVKantonNr)


			' Vollzugskosten AG...
			If cVAGProz <> 0 Then AddToVAG(.GAVGruppe0,
																			.GAVGruppe1,
																			iGAVNr,
																			iGruppe1Nr,
																			If(LPYear > 2016, cAHVLohn, cSUVALohn), cVAGProz,
																			.GAVKanton, iGAVKantonNr)
			If cVAGsBetrag <> 0 Then AddToVAGStd(.GAVGruppe0,
									   .GAVGruppe1,
										iGAVNr,
										iGruppe1Nr,
										cVAGsBetrag, 100,
										.GAVKanton, iGAVKantonNr)
			If cVAGmBetrag <> 0 Then AddToVAGM(.GAVGruppe0,
										.GAVGruppe1,
										iGAVNr,
										iGruppe1Nr,
										cVAGmBetrag, 100,
									   .GAVKanton, iGAVKantonNr)

			' AN
			If cVANProz <> 0 Then AddToVAN(.GAVGruppe0,
																		 .GAVGruppe1,
																			iGAVNr,
																			iGruppe1Nr,
																			If(LPYear > 2016, cAHVLohn, cSUVALohn), cVANProz,
																			.GAVKanton, iGAVKantonNr)
			If cVANsBetrag <> 0 Then AddToVANStd(.GAVGruppe0,
									   .GAVGruppe1,
										iGAVNr,
										iGruppe1Nr,
										cVANsBetrag, 100,
									   .GAVKanton, iGAVKantonNr)
			If cVANmBetrag <> 0 Then AddToVANM(.GAVGruppe0,
									   .GAVGruppe1,
										iGAVNr,
										iGruppe1Nr,
										cVANmBetrag, 100,
									   .GAVKanton, iGAVKantonNr)


			' KTG AG...
			If cKTGAGProz <> 0 Then AddToKTGAG(.GAVGruppe0, iGAVNr,
										cKTGLohn, cKTGAGProz,
									   .GAVKanton, iGAVKantonNr, bKTG60)

			' AN
			If cKTGANProz <> 0 Then AddToKTGAN(.GAVGruppe0, iGAVNr,
										cKTGLohn, cKTGANProz,
										.GAVKanton, iGAVKantonNr, bKTG60)

		End With

	End Sub

	Private Sub GetVarKorrektur(ByVal laData As LAData,
						ByVal cTempwert As Decimal,
						ByVal SUVACode As String,
						ByVal ModulName As String,
						Optional ByVal Far As Boolean = False)
		Dim i As Integer
		'Dim bKKAN As Boolean
		'Dim bKKAG As Boolean
		Dim cKTGBasis As Decimal
		'Dim cKTGBasis60 As Currency

		If LPMonth < 4 And LPYear <= 2006 Then Exit Sub

		cKTGBasis = 0
		'cKTGBasis60 = 0
		' für KTG...
		If laData.KKPflichtig Then
			cGAVKTGAGBasis = 0
			cGAVKTGANBasis = 0
			'  cGAVKTGAGBasis60 = 0
			'  cGAVKTGANBasis60 = 0

			For i = 0 To 50
				' Arbeitgeberanteil
				cKTGBasis = aGComplettKKAG.GetData(i).Basis
				'    cKTGBasis60 = aGComplettKKAG60(i, 1, 0)
				cGAVKTGAGBasis = cKTGBasis + cGAVKTGAGBasis
				'    cGAVKTGAGBasis60 = cKTGBasis60 + cGAVKTGAGBasis60


				' Arbeitnehmeranteil
				cKTGBasis = aGComplettKKAN.GetData(i).Basis
				'    cKTGBasis60 = aGComplettKKAN60(i, 1, 0)
				cGAVKTGANBasis = cKTGBasis + cGAVKTGANBasis
				'    cGAVKTGANBasis60 = cKTGBasis60 + cGAVKTGANBasis60
			Next i
		End If

		'cKTGBasis = 0
		'If LArec!KKpflichtig Then
		'  cGAVKTGANBasis = 0
		'  For i = 0 To 50
		'    cKTGBasis = aGComplettKKAN(i, 1, 0)
		'    cGAVKTGANBasis = cKTGBasis + cGAVKTGANBasis
		'  Next i
		'End If

	End Sub

	Private Function GetKTGNormalValue() As Decimal

		If LPMonth < 4 And LPYear <= 2006 Then Exit Function
		'If (cGAVKTGANBasis + cGAVKTGANBasis60) = 0 Then Exit Function
		If (cGAVKTGANBasis) = 0 Then Exit Function

		' für KTG...
		'If U(6) <> 0 And (cGAVKTGANBasis + cGAVKTGANBasis60) <> 0 Then
		If U(6) <> 0 And (cGAVKTGANBasis) <> 0 Then
			'  U(6) = (cGAVKTGANBasis + cGAVKTGANBasis60) - U(6)
			U(6) = (cGAVKTGANBasis) - U(6)
		End If

		'If U(7) <> 0 And (cGAVKTGANBasis + cGAVKTGANBasis60) <> 0 Then
		If U(7) <> 0 And (cGAVKTGANBasis) <> 0 Then
			'  U(7) = (cGAVKTGANBasis + cGAVKTGANBasis60) - U(7)
			U(7) = (cGAVKTGANBasis) - U(7)
		End If

		'If (cGAVKTGANBasis + cGAVKTGANBasis60) <> 0 Then
		If (cGAVKTGANBasis) <> 0 Then
			U(6) = Math.Min(U(6), 0)
			U(7) = Math.Min(U(7), 0)
		End If
		If U(6) < 0 Then U(6) = U(6) * (-1) ' Betrieb
		If U(7) < 0 Then U(7) = U(7) * (-1) ' Büro

	End Function

End Class
