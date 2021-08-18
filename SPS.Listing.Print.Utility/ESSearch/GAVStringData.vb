

Imports SP.Infrastructure.Logging


' GAV string data.
Public Class GAVStringData

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()


	Public Property GAVNr As Integer ' Element 0
	Public Property MetaNr As Integer	' Element 1
	Public Property CalNr As Integer ' Element 2
	Public Property CatNr As Integer ' Element 3
	Public Property CatBaseNr As Integer ' Element 4
	Public Property CatValueNr As Integer	' Element 5
	Public Property LONr As String	 ' Element 6   ' Could be multiple e.g. 3252, 0, 3331, 3317, 0, 3332, 3333
	Public Property Kanton As String ' Element 7
	Public Property Gruppe0 As String	' Element 8
	Public Property Gruppe1 As String	' Element 9
	Public Property Gruppe2 As String	' Element 10
	Public Property Gruppe3 As String	' Element 11
	Public Property GAVText As String	' Element 12
	Public Property Res_D As String	' Element 13
	Public Property Res_E As String	' Element 14
	Public Property Res_F As String	' Element 15
	Public Property Res_15 As String ' Element 16
	Public Property Res_16 As String ' Element 17
	Public Property Res_17 As String	' Element 18
	Public Property Res_18 As String ' Element 19
	Public Property GAVPublicationDate As String ' Element 20
	Public Property Monatslohn As Decimal	' Element 21
	Public Property FeiertagJahr As Integer	' Element 22
	Public Property FerienJahr As Integer	' Element 23
	Public Property Hat13erLohn As Boolean ' Element 24
	Public Property BasisLohn As Decimal ' Element 25
	Public Property FerienBetrag As Decimal	' Element 26
	Public Property FerienProz As Decimal	' Element 27
	Public Property FeierBetrag As Decimal ' Element 28
	Public Property FeierProz As Decimal ' Element 29
	Public Property Betrag_Lohn13 As Decimal ' Element 30
	Public Property Proz_Lohn13 As Decimal ' Element 31
	Public Property CalcFerien As Integer	' Element 32
	Public Property CalcFeier As Integer ' Element 33
	Public Property Calc13 As Integer	' Element 34
	Public Property StdLohn As Decimal ' Element 35
	Public Property FARAN As Decimal ' Element 36
	Public Property FARAG As Decimal ' Element 37
	Public Property VAN_Value As Decimal ' Element 38
	Public Property VAG_Value As Decimal ' Element 39
	Public Property StdWeek As Decimal ' Element 40
	Public Property StdMonth As Decimal	' Element 41
	Public Property StdYear As Decimal ' Element 42
	Public Property IsPVL As Integer ' Element 43
	Public Property _WAG As Decimal	' Element 44
	Public Property _WAN As Decimal	' Element 45
	Public Property _WAG_S As Decimal	' Element 46
	Public Property _WAN_S As Decimal	' Element 47
	Public Property _WAG_J As Decimal	' Element 48
	Public Property _WAN_J As Decimal	' Element 49
	Public Property _VAG As Decimal	' Element 50
	Public Property _VAN As Decimal	' Element 51
	Public Property _VAG_S As Decimal	' Element 52
	Public Property _VAN_S As Decimal	' Element 53
	Public Property _VAG_J As Decimal	' Element 54
	Public Property _VAN_J As Decimal	' Element 55
	Public Property _FAG As Decimal	' Element 56
	Public Property _FAN As Decimal	' Element 57
	Public Property BauQ12 As Decimal	' Element 58
	Public Property iFANCalc As Integer	' Element 59
	Public Property bFANWithBVG As Boolean ' Element 60

	' Additional Fiels not delivered by GAV string
	Public Property _FAG_M As Decimal
	Public Property _FAN_M As Decimal
	Public Property _WAG_M As Decimal
	Public Property _WAN_M As Decimal
	Public Property _VAG_M As Decimal
	Public Property _VAN_M As Decimal

	Public Property CompleteGAVString As String

	''' <summary>
	''' Helper varialbe to used while parsing GAV values from a string.
	''' </summary>
	Private m_Index As Integer = 0

	''' <summary>
	''' Fills the object form a string value.
	''' </summary>
	''' <param name="gavDataString">The GAV string.</param>
	Public Sub FillFromString(ByVal gavDataString As String)

		' gavDataString = "GAVNr:390001¦MetaNr:56¦CalNr:56¦CatNr:177¦CatBaseNr:176¦CatValueNr:3333¦LONr:3252, 0, 3331, 3317, 0, 3332, 3333¦Kanton:SG¦Beruf:Personalverleih Holzindustrie Schweiz¦Alter:30¦:.. Dienstjahre:¦Jahr:2013¦:.. Kanton:SG¦:.. Bezirk:¦Mitarbeiterkategorie:Berufsarbeiter¦:.. Erfahrungsjahre:ab 4. Jahr nach LAP (100% vom Regelmindestlohn)¦Res_15:¦Res_16:¦Res_17:¦Res_18:¦Res_19:¦Monatslohn:4801¦FeiertagJahr:20¦FierienJahr:9¦13.Lohn:True¦BasisLohn:25.95¦FerienBetrag:2.16¦FerienProz:0.0833¦FeierBetrag:0.93¦FeierProz:0.0359¦13.Betrag:2.42¦13.Proz:0.0833¦CalcFerien:1¦CalcFeier:1¦Calc13:4¦StdLohn:31.46¦FARAN:0¦FARAG:0¦VAN:0.7¦VAG:0.3¦StdWeek:50.0000¦StdMonth:182.0000¦StdYear:2216.0000¦IsPVL:0¦_WAG:0¦_WAN:0¦_WAG_S:0¦_WAN_S:0¦_WAG_J:0¦_WAN_J:0¦_VAG:0¦_VAN:0¦_VAG_S:0¦_VAN_S:0¦_VAG_J:0¦_VAN_J:0¦_FAG:0¦_FAN:0¦BauQ12:0¦iFANCalc:0¦bFANWithBVG:False¦"

		CompleteGAVString = gavDataString

		If String.IsNullOrWhiteSpace(gavDataString) Then
			Return
		End If

		Dim keyValuePairs = gavDataString.Split("¦")

		Dim tuples As New List(Of Tuple(Of String, String))
		For Each keyValue In keyValuePairs

			Dim tuple As Tuple(Of String, String) = SplitKeyValue(keyValue)
			If Not tuple Is Nothing Then tuples.Add(tuple)
		Next

		m_Index = 0

		Try
			GAVNr = Convert.ToInt32(tuples(PostIncIndex()).Item2)
			MetaNr = Convert.ToInt32(tuples(PostIncIndex()).Item2)
			CalNr = Convert.ToInt32(tuples(PostIncIndex()).Item2)
			CatNr = Convert.ToInt32(tuples(PostIncIndex()).Item2)
			CatBaseNr = Convert.ToInt32(tuples(PostIncIndex()).Item2)
			CatValueNr = Convert.ToInt32(tuples(PostIncIndex()).Item2)
			LONr = tuples(PostIncIndex()).Item2
			Kanton = tuples(PostIncIndex()).Item2
			Gruppe0 = tuples(PostIncIndex()).Item2
			Gruppe1 = tuples(PostIncIndex()).Item2
			Gruppe2 = tuples(PostIncIndex()).Item2
			Gruppe3 = tuples(PostIncIndex()).Item2
			GAVText = tuples(PostIncIndex()).Item2
			Res_D = tuples(PostIncIndex()).Item2
			Res_E = tuples(PostIncIndex()).Item2
			Res_F = tuples(PostIncIndex()).Item2
			Res_15 = tuples(PostIncIndex()).Item2
			Res_16 = tuples(PostIncIndex()).Item2
			Res_17 = tuples(PostIncIndex()).Item2
			Res_18 = tuples(PostIncIndex()).Item2
			GAVPublicationDate = tuples(PostIncIndex()).Item2
			Monatslohn = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			FeiertagJahr = Convert.ToInt32(Val(tuples(PostIncIndex()).Item2))
			FerienJahr = Convert.ToInt32(Val(tuples(PostIncIndex()).Item2))
			Hat13erLohn = Convert.ToBoolean(tuples(PostIncIndex()).Item2)
			BasisLohn = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			FerienBetrag = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			FerienProz = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			FeierBetrag = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			FeierProz = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			Betrag_Lohn13 = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			Proz_Lohn13 = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			CalcFerien = Convert.ToInt32(tuples(PostIncIndex()).Item2)
			CalcFeier = Convert.ToInt32(tuples(PostIncIndex()).Item2)
			Calc13 = Convert.ToInt32(tuples(PostIncIndex()).Item2)
			StdLohn = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			FARAN = StrToNullableDecimal(tuples(PostIncIndex()).Item2, 0D)
			FARAG = StrToNullableDecimal(tuples(PostIncIndex()).Item2, 0D)
			VAN_Value = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			VAG_Value = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			StdWeek = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			StdMonth = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			StdYear = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			IsPVL = Convert.ToInt32(tuples(PostIncIndex()).Item2)
			_WAG = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_WAN = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_WAG_S = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_WAN_S = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_WAG_J = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_WAN_J = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_VAG = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_VAN = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_VAG_S = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_VAN_S = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_VAG_J = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_VAN_J = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_FAG = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			_FAN = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			BauQ12 = Convert.ToDecimal(tuples(PostIncIndex()).Item2)
			iFANCalc = Convert.ToInt32(tuples(PostIncIndex()).Item2)
			bFANWithBVG = Convert.ToBoolean(tuples(PostIncIndex()).Item2)

		Catch ex As Exception
			m_Logger.LogError(String.Format("GAVNr: {0} | GAVBeruf: {1} | Index: {2}{3}{4}{3}{5}", GAVNr, Gruppe0, m_Index, vbNewLine, CompleteGAVString, ex.ToString))

		End Try

		m_Index = 0

	End Sub

	''' <summary>
	''' Splits a key value pair.
	''' </summary>
	''' <param name="keyValuePair">The key value pair.</param>
	''' <returns>Tuple of key and value.</returns>
	Private Function SplitKeyValue(ByVal keyValuePair As String) As Tuple(Of String, String)

		Dim result As Tuple(Of String, String) = Nothing

		If String.IsNullOrWhiteSpace(keyValuePair) Then
			Return Nothing
		End If

		Dim indexOfFirstColon = keyValuePair.IndexOf(":"c)
		If indexOfFirstColon < 0 Then Return result

		Dim key As String = keyValuePair.Substring(0, indexOfFirstColon)
		Dim value As String = keyValuePair.Substring(indexOfFirstColon + 1, keyValuePair.Length - (indexOfFirstColon + 1))
		If value.IndexOf("|") > 0 Then
			indexOfFirstColon = value.IndexOf("|")
			value = value.Substring(0, indexOfFirstColon)
		End If

		result = New Tuple(Of String, String)(key, value)

		Return result
	End Function

	''' <summary>
	''' Converts an string to a nullable integer.
	''' </summary>
	''' <param name="intStr">The integer string.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>The nullable integer.</returns>
	Private Shared Function StrToNullableInteger(ByVal intStr As String, ByVal defaultValue As Integer) As Integer?

		If String.IsNullOrWhiteSpace(intStr) Then
			Return defaultValue
		End If

		Return Convert.ToInt32(intStr)
	End Function


	''' <summary>
	''' Converts an string to a nullable decimal.
	''' </summary>
	''' <param name="decStr">The decimal string.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>The nullable integer.</returns>
	Private Shared Function StrToNullableDecimal(ByVal decStr As String, ByVal defaultValue As Decimal) As Decimal?

		If String.IsNullOrWhiteSpace(decStr) Then
			Return defaultValue
		End If

		Return Convert.ToDecimal(decStr)
	End Function

	''' <summary>
	''' Post increment index (like i++ in C#)
	''' </summary>
	''' <returns>Old index value.</returns>
	''' <remarks>Incements index by one but returns old value.</remarks>
	Private Function PostIncIndex() As Integer
		m_Index = m_Index + 1

		Return m_Index - 1
	End Function

	''' <summary>
	''' Creates a shallow copy of the object.
	''' </summary>
	Public Function ShallowCopy() As GAVStringData
		Return DirectCast(Me.MemberwiseClone(), GAVStringData)
	End Function

End Class
