Imports System.Text

Public Class ReferenceNumberUtility

  ''' <summary>
  ''' Formats the reference number.
  ''' </summary>
  Public Function FormatReferenceNumbers(ByVal esrID As String, ByVal kdNr As String,
                                         ByVal opNr As String, ByVal betragInk As Decimal,
                                         ByVal refKonoNr As String, ByVal esrKonto As String,
                                         ByVal refNrTo10 As Boolean) As Tuple(Of String, String)

    Dim tempKDNr = GetPaddedOPOrKDNr(kdNr, refNrTo10)
    Dim tempOPNr = GetPaddedOPOrKDNr(opNr, refNrTo10)

    Dim refNr0 As New StringBuilder
    Dim refNr1 As New StringBuilder

    If refNrTo10 Then
      refNr0.Append(esrID.PadLeft(6, "0"))
      refNr0.Append(Space(1))
      refNr0.Append(Left(tempKDNr, 5))
      refNr0.Append(Space(1))
      refNr0.Append(Mid(tempKDNr, 6))
      refNr0.Append(Space(1))
      refNr0.Append(Left(tempOPNr, 5))
      refNr0.Append(Space(1))
      refNr0.Append(Mid(tempOPNr, 6))
      refNr0.Append(Space(1))
      refNr0.Append(Mod10(refNr0.ToString().Replace(" ", ""), False))

      refNr1.Append(FormatsPartOneOfFooterReferenceNumber(betragInk))
      refNr1.Append(">")
      refNr1.Append(Replace(refNr0.ToString(), " ", ""))
      refNr1.Append("+ ")
      refNr1.Append(refKonoNr)
      refNr1.Append(">")

    Else
      refNr0.Append(FormatMDBesr(esrID))
      refNr0.Append("0")
      refNr0.Append(Space(1))
      refNr0.Append(Left(tempKDNr, 5))
      refNr0.Append(Space(1))
      refNr0.Append(Right(tempKDNr, 2))
      refNr0.Append("000")
      refNr0.Append(Space(1))
      refNr0.Append(Left(tempOPNr, 5))
      refNr0.Append(Space(1))
      refNr0.Append(Right(tempOPNr, 2))
      refNr0.Append("00")
      refNr0.Append(Mod10(String.Format("{0}0{1}000{2}00", esrID, tempKDNr, tempOPNr), False))

      refNr1.Append(FormatsPartOneOfFooterReferenceNumber(betragInk))
      refNr1.Append(">")
      refNr1.Append(FormatPartTwoOfFooterReferenceNumber(tempKDNr, tempOPNr, esrID, esrKonto))
      refNr1.Append(">")

    End If

    Return New Tuple(Of String, String)(refNr0.ToString(), refNr1.ToString())

  End Function

  ''' <summary>
  ''' Formats part one of the footer reference number.
  ''' </summary>
  Private Function FormatsPartOneOfFooterReferenceNumber(ByVal betrag As Decimal) As String

    Dim buffer As New StringBuilder
    buffer.Append("01")
    buffer.Append(String.Format("{0:00000000}", Math.Truncate(betrag)))
    buffer.Append(Right(TwoDecimals(betrag), 2))
    Dim temp = buffer.ToString()
    buffer.Append(Mod10(buffer.ToString(), False))

    Return buffer.ToString()
  End Function


  ''' <summary>
  ''' Formats part two of the footer reference number.
  ''' </summary>
  Private Function FormatPartTwoOfFooterReferenceNumber(ByVal kdNr As String, ByVal opNr As String, ByVal mdBesr As String, ByVal esrKonto As String) As String

    Dim buffer As New StringBuilder

    buffer.Append(Left(mdBesr, 2))
    buffer.Append(Right(mdBesr, 4))
    buffer.Append("0")
    buffer.Append(Left(kdNr, 5))
    buffer.Append(Right(kdNr, 2))
    buffer.Append("000")
    buffer.Append(Left(opNr, 5))
    buffer.Append(Right(opNr, 2))
    buffer.Append("00")
    buffer.Append(Mod10(String.Format("{0}0{1}000{2}00", mdBesr, kdNr, opNr), False))
    buffer.Append("+ ")
    buffer.Append(esrKonto)

    Return buffer.ToString()

  End Function

  ''' <summary>
  ''' Formats reference number line1.
  ''' </summary>
  Private Function FormatReferenceNumberLine1(ByVal kdNr As String, ByVal opNr As String, ByVal mdBesr As String) As String

    Dim buffer As New StringBuilder

    buffer.Append(Left(mdBesr, 2))
    buffer.Append(Space(1))
    buffer.Append(Right(mdBesr, 4))
    buffer.Append("0")
    buffer.Append(Space(1))
    buffer.Append(Left(kdNr, 5))
    buffer.Append(Space(1))
    buffer.Append(Right(kdNr, 2))
    buffer.Append("000")
    buffer.Append(Space(1))
    buffer.Append(Left(opNr, 5))
    buffer.Append(Space(1))
    buffer.Append(Right(opNr, 2))
    buffer.Append("00")
    buffer.Append(Mod10(String.Format("{0}0{1}000{2}00", mdBesr, kdNr, opNr), False))

    Return buffer.ToString()

  End Function

  ''' <summary>
  ''' Formats the MDBesnr.
  ''' </summary>
  Private Function FormatMDBesr(ByVal mdEsr As String) As String

    Dim result As String = Left(mdEsr, 2) & " " & Right(mdEsr, 4)

    Return result

  End Function

  ''' <summary>
  ''' Gets a padded OP or KDNr.
  ''' </summary>
  Private Function GetPaddedOPOrKDNr(ByVal kdNr As String, Optional ByVal refWith10 As Boolean = False) As String

    Dim tempKDNr As String = kdNr ' TODO mit Fardin besprechen

    If refWith10 Then
      If Len(kdNr) < 10 Then
        tempKDNr = kdNr.PadLeft(10, "0")
      End If
    Else
      If Len(kdNr) < 7 Then
        tempKDNr = kdNr.PadLeft(7, "0")
      End If
    End If
    Return tempKDNr
  End Function

  ''' <summary>
  ''' Mod10 calculation.
  ''' </summary>
  Private Function Mod10(ByVal pString As String, Optional mitPStr As Boolean = True) As String

    Dim Pos As Integer = 0
    Dim Uet As Integer = 0

    Do While Pos < Len(pString)
      Pos = Pos + 1
      Uet = Val(Mid("0946827135", (Val(Mid(pString, Pos, 1)) + Uet) Mod 10 + 1, 1))
    Loop

    Mod10 = IIf(mitPStr, pString, "")
    Mod10 = Trim(Mod10 + Str((10 - Uet) Mod 10))

  End Function

  ''' <summary>
  ''' Formats to two decimals.
  ''' </summary>
  Private Function TwoDecimals(ByVal betrag As Decimal) As String

    TwoDecimals = String.Format("{0:0.00}", betrag)

  End Function

End Class
