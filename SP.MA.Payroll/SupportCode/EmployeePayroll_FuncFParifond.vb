Imports SP.DatabaseAccess.PayrollMng.DataObjects

Partial Public Class EmployeePayroll

	Private Sub AddToGAVStd(ByVal strBeruf As String,
						 ByVal strGruppe1 As String,
						 ByVal iGAVNr As Integer,
						 ByVal iGruppe1Nr As Integer,
						 ByVal cTempBasis As Decimal,
						 ByVal cTempProz As Decimal,
						 ByVal strKanton As String,
						 ByVal iGAVKantonNr As Integer,
						 Optional bMakeGleitTime As Boolean = False)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		aGComplettStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cCurrentBas + cTempBasis
		aGComplettStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

		If Not bMakeGleitTime Then Exit Sub
		cCurrentBas = aGAVStdAnzGleit.GetData(iGAVNr).Basis
		aGAVStdAnzGleit.GetData(iGAVNr).GAVBeruf = strBeruf
		aGAVStdAnzGleit.GetData(iGAVNr).Basis = cCurrentBas + cTempBasis

		' TODO Fardin: Prüfen Zugriff auf RPGAV_Std obwohl auch von LM Seite zugeriffen wird.
		aGAVStdAnzGleit.GetData(iGAVNr).Anzahl = currRplData.RPGAV_StdMonth ' Val(DivFunc.vFieldVal(AllRPLOrec!RPGAV_Stdmonth))

	End Sub

	Private Sub AddToFAG(ByVal strBeruf As String,
							ByVal iGAVNr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettFAG.GetData(iGAVNr, iGAVKantonNr).Basis

		aGComplettFAG.GetData(iGAVNr, iGAVKantonNr).GAVBeruf = strBeruf
		aGComplettFAG.GetData(iGAVNr, iGAVKantonNr).Basis = cCurrentBas + cTempBasis
		aGComplettFAG.GetData(iGAVNr, iGAVKantonNr).Ansatz = cTempProz
		aGComplettFAG.GetData(iGAVNr, iGAVKantonNr).Kanton = strKanton

		Trace.WriteLine(String.Format("AddToFAG: CurrentBasis: {0:n2} >>> NewBasis: {1:n2}", cCurrentBas, cTempBasis))

	End Sub

	Private Sub AddToFAN(ByVal strBeruf As String,
						 ByVal iGAVNr As Integer,
						 ByVal cTempBasis As Decimal,
						 ByVal cTempProz As Decimal,
						 ByVal strKanton As String,
						 ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettFAN.GetData(iGAVNr, iGAVKantonNr).Basis

		aGComplettFAN.GetData(iGAVNr, iGAVKantonNr).GAVBeruf = strBeruf
		aGComplettFAN.GetData(iGAVNr, iGAVKantonNr).Basis = cCurrentBas + cTempBasis
		aGComplettFAN.GetData(iGAVNr, iGAVKantonNr).Ansatz = cTempProz
		aGComplettFAN.GetData(iGAVNr, iGAVKantonNr).Kanton = strKanton

		Trace.WriteLine(String.Format("AddToFAN: CurrentBasis: {0:n2} >>> NewBasis: {1:n2}", cCurrentBas, cTempBasis))

	End Sub

	Private Sub AddToWAG(ByVal strBeruf As String,
							ByVal strGruppe1 As String,
							ByVal iGAVNr As Integer,
							ByVal iGruppe1Nr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettWAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettWAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		aGComplettWAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cCurrentBas + cTempBasis
		aGComplettWAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettWAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettWAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Private Sub AddToWAGStd(ByVal strBeruf As String,
							ByVal strGruppe1 As String,
							ByVal iGAVNr As Integer,
							ByVal iGruppe1Nr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettWAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettWAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		' Achtung keine zusammenaddieren
		aGComplettWAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cTempBasis
		aGComplettWAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettWAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettWAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Private Sub AddToWAN(ByVal strBeruf As String,
							ByVal strGruppe1 As String,
							ByVal iGAVNr As Integer,
							ByVal iGruppe1Nr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettWAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettWAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		aGComplettWAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cCurrentBas + cTempBasis
		aGComplettWAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettWAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettWAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Private Sub AddToWANStd(ByVal strBeruf As String,
							ByVal strGruppe1 As String,
							ByVal iGAVNr As Integer,
							ByVal iGruppe1Nr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettWANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettWANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		' Achtung keine zusammenaddieren
		aGComplettWANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cTempBasis
		aGComplettWANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettWANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettWANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Private Sub AddToVAG(ByVal strBeruf As String,
								ByVal strGruppe1 As String,
								ByVal iGAVNr As Integer,
								ByVal iGruppe1Nr As Integer,
								ByVal cTempBasis As Decimal,
								ByVal cTempProz As Decimal,
								ByVal strKanton As String,
								ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettVAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettVAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		aGComplettVAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cCurrentBas + cTempBasis
		aGComplettVAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettVAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettVAG.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

		Trace.WriteLine(String.Format("AddToVAG: CurrentBasis: {0:n2} >>> NewBasis: {1:n2}", cCurrentBas, cTempBasis))

	End Sub

	Private Sub AddToVAGStd(ByVal strBeruf As String,
							ByVal strGruppe1 As String,
							ByVal iGAVNr As Integer,
							ByVal iGruppe1Nr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettVAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettVAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		' Achtung keine zusammenaddieren
		aGComplettVAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cTempBasis
		aGComplettVAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettVAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettVAGStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Private Sub AddToVAN(ByVal strBeruf As String,
						 ByVal strGruppe1 As String,
						 ByVal iGAVNr As Integer,
						 ByVal iGruppe1Nr As Integer,
						 ByVal cTempBasis As Decimal,
						 ByVal cTempProz As Decimal,
						 ByVal strKanton As String,
						 ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettVAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettVAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		aGComplettVAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cCurrentBas + cTempBasis
		aGComplettVAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettVAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettVAN.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

		Trace.WriteLine(String.Format("AddToVAN: CurrentBasis: {0:n2} >>> NewBasis: {1:n2}", cCurrentBas, cTempBasis))

	End Sub

	Private Sub AddToVANStd(ByVal strBeruf As String,
							ByVal strGruppe1 As String,
							ByVal iGAVNr As Integer,
							ByVal iGruppe1Nr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettVANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettVANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		' Achtung keine zusammenaddieren
		aGComplettVANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cTempBasis
		aGComplettVANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettVANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettVANStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Sub AddToWAGM(ByVal strBeruf As String,
							ByVal strGruppe1 As String,
							ByVal iGAVNr As Integer,
							ByVal iGruppe1Nr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettWAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettWAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		' Achtung keine zusammenaddieren
		aGComplettWAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cTempBasis
		aGComplettWAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettWAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettWAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Sub AddToWANM(ByVal strBeruf As String,
							ByVal strGruppe1 As String,
							ByVal iGAVNr As Integer,
							ByVal iGruppe1Nr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettWANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettWANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		' Achtung keine zusammenaddieren
		aGComplettWANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cTempBasis
		aGComplettWANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettWANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettWANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Sub AddToVANM(ByVal strBeruf As String,
							ByVal strGruppe1 As String,
							ByVal iGAVNr As Integer,
							ByVal iGruppe1Nr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettVANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettVANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		' Achtung keine zusammenaddieren
		aGComplettVANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cTempBasis
		aGComplettVANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettVANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettVANM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Sub AddToVAGM(ByVal strBeruf As String,
							ByVal strGruppe1 As String,
							ByVal iGAVNr As Integer,
							ByVal iGruppe1Nr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettVAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettVAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		' Achtung keine zusammenaddieren
		aGComplettVAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cTempBasis
		aGComplettVAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettVAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettVAGM.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Private Sub AddToGleitBetrag(ByVal strBeruf As String,
								ByVal strGruppe1 As String,
								ByVal iGAVNr As Integer,
								ByVal iGruppe1Nr As Integer,
								ByVal cTempBasis As Decimal,
								ByVal cTempProz As Decimal,
								ByVal strKanton As String,
							 ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettGleitBetrag.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettGleitBetrag.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		' Achtung keine zusammenaddieren
		aGComplettGleitBetrag.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cCurrentBas + cTempBasis
		aGComplettGleitBetrag.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettGleitBetrag.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettGleitBetrag.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Private Sub AddToGleitStd(ByVal strBeruf As String,
							ByVal strGruppe1 As String,
							ByVal iGAVNr As Integer,
							ByVal iGruppe1Nr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer)
		Dim cCurrentBas As Decimal

		cCurrentBas = aGComplettGleitStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis

		aGComplettGleitStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).GAVBeruf = strBeruf
		' Achtung keine zusammenaddieren
		aGComplettGleitStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Basis = cCurrentBas + cTempBasis
		aGComplettGleitStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Ansatz = cTempProz
		aGComplettGleitStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Kanton = strKanton
		aGComplettGleitStd.GetData(iGAVNr, iGAVKantonNr, iGruppe1Nr).Gruppe = strGruppe1

	End Sub

	Sub AddToKTGAG(ByVal strBeruf As String,
							ByVal iGAVNr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer,
							ByVal bKTG60_ As Boolean)
		Dim cCurrentBas As Decimal

		'If bKTG60_ Then
		'  cCurrentBas = Val(aGComplettKKAG60(iGAVNr, 1, 0))
		'
		'  aGComplettKKAG60(iGAVNr, 0, 0) = strBeruf
		'  aGComplettKKAG60(iGAVNr, 1, 0) = cCurrentBas + cTempBasis
		'  aGComplettKKAG60(iGAVNr, 1, 1) = cTempProz
		'
		'Else
		cCurrentBas = aGComplettKKAG.GetData(iGAVNr).Basis

		aGComplettKKAG.GetData(iGAVNr).GAVBeruf = strBeruf
		aGComplettKKAG.GetData(iGAVNr).Basis = cCurrentBas + cTempBasis
		aGComplettKKAG.GetData(iGAVNr).Ansatz = cTempProz
		'End If

	End Sub

	Private Sub AddToKTGAN(ByVal strBeruf As String,
							ByVal iGAVNr As Integer,
							ByVal cTempBasis As Decimal,
							ByVal cTempProz As Decimal,
							ByVal strKanton As String,
							ByVal iGAVKantonNr As Integer,
							ByVal bKTG60_ As Boolean)
		Dim cCurrentBas As Decimal

		'If bKTG60_ Then
		'  cCurrentBas = Val(aGComplettKKAN60(iGAVNr, 1, 0))
		'
		'  aGComplettKKAN60(iGAVNr, 0, 0) = strBeruf
		'  aGComplettKKAN60(iGAVNr, 1, 0) = cCurrentBas + cTempBasis
		'  aGComplettKKAN60(iGAVNr, 1, 1) = cTempProz
		'
		'Else
		cCurrentBas = aGComplettKKAN.GetData(iGAVNr).Basis

		aGComplettKKAN.GetData(iGAVNr).GAVBeruf = strBeruf
		aGComplettKKAN.GetData(iGAVNr).Basis = cCurrentBas + cTempBasis
		aGComplettKKAN.GetData(iGAVNr).Ansatz = cTempProz
		'End If

	End Sub

	Private Function GetKTGAGBasis(ByVal iArrayNr As Integer) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettKKAG.GetData(iArrayNr).Basis

		Return result
	End Function

	Private Function GetKTGAGAns(ByVal iArrayNr As Integer) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettKKAG.GetData(iArrayNr).Ansatz

		Return result
	End Function

	Private Function GetKTGANBasis(ByVal iArrayNr As Integer) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettKKAN.GetData(iArrayNr).Basis

		Return result
	End Function

	Private Function GetKTGANAns(ByVal iArrayNr As Integer) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettKKAN.GetData(iArrayNr).Ansatz

		Return result
	End Function

	Private Function GetFAGBasis(ByVal iBerufNr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim Basvalue = aGComplettFAG.GetData(iBerufNr, iKantonNr).Basis

		Return Math.Min(Basvalue, S(6))

	End Function

	Private Function GetFAGAns(ByVal iBerufNr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettFAG.GetData(iBerufNr, iKantonNr).Ansatz

		Return result
	End Function

	Private Function GetFANBasis(ByVal iBerufNr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim Basvalue = aGComplettFAN.GetData(iBerufNr, iKantonNr).Basis
		If S(6) < Basvalue Then
			Dim sMsgText As String
			sMsgText = "Achtung in der Lohnabrechnung ({0}): Der Betrag von AHV-Basis {1:n2} ist tiefer als FAR-Basis {2:n2}. Möglicherweise ist das Kandidatenalter >= 65."
			sMsgText = String.Format(sMsgText, LONewNr, S(6), Basvalue)

			WriteToProtocol(Padright("M -> (GetFANBasis): ", 30, " ") & sMsgText)

		End If

		Return Math.Min(Basvalue, S(6))

	End Function

	Private Function GetFANAns(ByVal iBerufNr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettFAN.GetData(iBerufNr, iKantonNr).Ansatz

		Return result
	End Function

	Private Function GetWAGBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		Dim result As Decimal = aGComplettWAG.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis

		Return result
	End Function

	Private Function GetWAGMBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		Dim result As Decimal = Val(aGComplettWAGM.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis)

		Return result
	End Function

	Private Function GetWAGAns(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettWAG.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Ansatz

		Return result
	End Function

	Private Function GetWANBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettWAN.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis

		Return result
	End Function

	Private Function GetWANAns(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettWAN.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Ansatz

		Return result
	End Function

	Private Function GetWANStdBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettWANStd.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis

		Return result
	End Function

	Private Function GetWANMBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = Val(aGComplettWANM.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis)

		Return result
	End Function

	Private Function GetWAGStdBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		Dim result As Decimal = aGComplettWAGStd.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis

		Return result
	End Function

	Private Function GetVAGBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		If IsToYoung Then Return 0

		Dim result As Decimal = aGComplettVAG.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis

		If IsRentner Then

			Dim monthCumulativeData = New MonthCumulativeData
			monthCumulativeData.MANummer = m_EmployeeData.EmployeeNumber
			monthCumulativeData.LONewNr = LONewNr
			monthCumulativeData.LANr = 7835.5D
			monthCumulativeData.LP = LPMonth
			monthCumulativeData.MDYear = LPYear
			monthCumulativeData.MDNr = MDNr

			Dim vanAmountThisMonth As Decimal? = m_PayrollDatabaseAccess.LoadLATotalBasisInMonth(monthCumulativeData)
			Dim currentVANAmount As Decimal = result + Math.Abs(vanAmountThisMonth.GetValueOrDefault(0))
			Dim currentAHVBasis As Decimal = S(6)

			If currentVANAmount > currentAHVBasis Then
				result = currentAHVBasis - Math.Abs(vanAmountThisMonth.GetValueOrDefault(0))
				aGComplettVAG.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis = result
			End If

		End If




		Return result
	End Function

	Private Function GetVAGAns(ByVal iBerufNr As Integer,
												ByVal iGruppe1Nr As Integer,
												Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettVAG.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Ansatz

		Return result
	End Function

	Private Function GetVANBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		If IsToYoung Then Return 0
		Dim result As Decimal = aGComplettVAN.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis

		If IsRentner Then

			Dim monthCumulativeData = New MonthCumulativeData
			monthCumulativeData.MANummer = m_EmployeeData.EmployeeNumber
			monthCumulativeData.LONewNr = LONewNr
			monthCumulativeData.LANr = 7395.5D
			monthCumulativeData.LP = LPMonth
			monthCumulativeData.MDYear = LPYear
			monthCumulativeData.MDNr = MDNr

			Dim vanAmountThisMonth As Decimal? = m_PayrollDatabaseAccess.LoadLATotalBasisInMonth(monthCumulativeData)
			Dim currentVANAmount As Decimal = result + Math.Abs(vanAmountThisMonth.GetValueOrDefault(0))
			Dim currentAHVBasis As Decimal = S(6)

			If currentVANAmount > currentAHVBasis Then
				'Dim prozVANBasis = (result * 100) / U(2)
				'Dim currentFullBasis = currentVANAmount - currentAHVBasis
				'result = (currentFullBasis * prozVANBasis) / 100 ' - currentAHVBasis

				result = currentAHVBasis - Math.Abs(vanAmountThisMonth.GetValueOrDefault(0))
				aGComplettVAN.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis = result
			End If

		End If


		Return result
	End Function

	Private Function GetVANAns(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettVAN.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Ansatz

		Return result
	End Function

	Private Function GetVANStdBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettVANStd.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis

		Return result
	End Function

	Private Function GetVANMBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = Val(aGComplettVANM.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis)

		Return result
	End Function

	Private Function GetVAGStdBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettVAGStd.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis

		Return result
	End Function

	Private Function GetVAGMBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = Val(aGComplettVAGM.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis)

		Return result
	End Function

	Private Function GetStdBasis(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0,
										Optional ByVal bShowMessage As Boolean = True) As Decimal
		Dim sMsgText As String
		' Variable ausfüllen...
		GetStdBasis = aGComplettStd.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis

		If Val(aGAVStdAnzGleit.GetData(iBerufNr).Basis) > Val(aGAVStdAnzGleit.GetData(iBerufNr).Anzahl) And
							Val(aGAVStdAnzGleit.GetData(iBerufNr).Anzahl) > 0 And
							bShowMessage Then
			' Stunden für die Gleitzeit sind vorhanden...
			sMsgText = "Achtung: Die geleisteten Arbeitsstunden sind höher als im GAV erlaubten Soll-Stunden.{0}Geleistet sind: {1:N2}{0}Soll-Stunden: {2:N2}{0}GAV-Beruf: {3}{0}Bitte kontrollieren Sie die Lohnabrechnung: {4}."
			sMsgText = String.Format(sMsgText, vbNewLine, aGAVStdAnzGleit.GetData(iBerufNr).Basis, aGAVStdAnzGleit.GetData(iBerufNr).Anzahl, aGAVStdAnzGleit.GetData(iBerufNr).GAVBeruf, LONewNr)

			WriteToProtocol(Padright("M -> (GetStdBasis): ", 30, " ") & sMsgText)

		End If

	End Function

	Private Function GetGleitAnz(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettGleitStd.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis

		Return result
	End Function

	Private Function GetGleitBas(ByVal iBerufNr As Integer,
										ByVal iGruppe1Nr As Integer,
										Optional ByVal iKantonNr As Integer = 0) As Decimal

		' Variable ausfüllen...
		Dim result As Decimal = aGComplettGleitBetrag.GetData(iBerufNr, iKantonNr, iGruppe1Nr).Basis

		Return result
	End Function

End Class
