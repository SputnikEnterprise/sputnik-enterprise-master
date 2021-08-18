
Imports System.Drawing

Namespace ColorUtility


  Public Class ClsColorUtility

    Dim _ClsProgSetting As New ClsProgSettingPath
    Dim _ClsReg As New ClsDivReg

#Region "Farbdarstellungen..."

    ''' <summary>
    ''' Gibt die BackColor für ToastNotification aus...
    ''' </summary>
    ''' <param name="Art"> INFO | WARN | ERROR</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetToastBackColor(ByVal Art As String) As Color
      Dim ColorValue As Color = Color.Orange
      Dim strValue As String = String.Empty
      Dim strQuery As String = String.Empty

      strQuery = String.Format("//Layouts/ToastNotifications/BackColorProperty_{0}", Art)
      strValue = GetColorValue(strQuery)
      If strValue = "0;0;0" Then Return ColorValue

      If strValue.Contains(";") Then
        Dim aColor As String() = strValue.Split(CChar(";"))
        ColorValue = Color.FromArgb(CInt(aColor(0).Trim), CInt(aColor(1).Trim), CInt(aColor(2).Trim))

      Else
        ColorValue = Color.FromName(strValue)

      End If

      Return ColorValue
    End Function

    ''' <summary>
    ''' Gibt die Textfarbe für ToastNotification aus...
    ''' </summary>
    ''' <param name="Art"> INFO | WARN | ERROR</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetToastForeColor(ByVal Art As String) As Color
      Dim ColorValue As Color = Color.Black
      Dim strValue As String = String.Empty
      Dim strQuery As String = String.Empty

      strQuery = String.Format("//Layouts/ToastNotifications/ForeColorProperty_{0}", Art)
      strValue = GetColorValue(strQuery)
      If strValue = "0;0;0" Then Return ColorValue

      If strValue.Contains(";") Then
        Dim aColor As String() = strValue.Split(CChar(";"))
        ColorValue = Color.FromArgb(CInt(aColor(0).Trim), CInt(aColor(1).Trim), CInt(aColor(2).Trim))

      Else
        ColorValue = Color.FromName(strValue)

      End If

      Return ColorValue
    End Function


    ''' <summary>
    ''' 1. Color ist für ForeColor
    ''' 2. Color ist für BorderColor
    ''' </summary>
    ''' <param name="Art"></param>
    ''' <returns>ist als List(Of Color)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMetroColor(ByVal Art As String) As List(Of Color)
      Dim ColorValue As New List(Of Color)
      Dim strValue As String = String.Empty
      Dim strQuery As String = String.Empty
      Dim aValue As String() = {""}

      strQuery = String.Format("//Layouts/MetroWindows/WindowColorProperty_{0}", Art)
      strValue = GetColorValue(strQuery)
      If strValue.Contains(CChar("-")) Then
        aValue = strValue.Split(CChar("-"))
        For i As Short = 0 To CShort(aValue.Length) - 1
          If aValue(i).Contains(";") Then
            Dim aColor As String() = aValue(i).Split(CChar(";"))
            ColorValue.Add(Color.FromArgb(CInt(aColor(0).Trim), CInt(aColor(1).Trim), CInt(aColor(2).Trim)))

          Else
            ColorValue.Add(Color.FromName(strValue))

          End If
        Next
      Else
        'If liColor.Count < 1 Then liColor = New List(Of Color)(New Color() {Color.White, Color.Orange})
        Return New List(Of Color)(New Color() {Color.White, Color.Orange})
      End If

      Return ColorValue
    End Function

    Public Shared Function GetColorValue(ByVal strQuery As String) As String
      Dim _ClsProgSetting As New ClsProgSettingPath
      Dim _ClsReg As New ClsDivReg
      Dim strModul2print As String = String.Empty

      Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsProgSetting.GetFormDataFile(), strQuery)
      If strBez = String.Empty Then Return "0;0;0"

      Return strBez
    End Function

#End Region

  End Class


End Namespace
