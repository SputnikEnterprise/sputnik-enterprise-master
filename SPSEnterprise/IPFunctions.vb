
Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Management

Imports System
Imports System.Net

Module IPFunctions

  Function GetMACAddress() As String
    Dim mc As System.Management.ManagementClass
    Dim mo As ManagementObject
    Dim strResult As String = ""

    Try
      mc = New ManagementClass("Win32_NetworkAdapterConfiguration")
      Dim moc As ManagementObjectCollection = mc.GetInstances()
      For Each mo In moc
        If mo.Item("IPEnabled") = True Then
          strResult = mo.Item("MacAddress").ToString
        End If
      Next

    Catch e As Exception
      MsgBox(e.Message & vbCrLf & "Die MAC-Adresse kann nicht ermittelt werden.", MsgBoxStyle.Exclamation, "GetMACAddress_0")

    End Try

    Return strResult
  End Function

  Public Function GetIP(Optional ByVal Computer As String = "") As ArrayList
    Dim ComputerName As String = String.Empty
    Dim Host As IPHostEntry
    Dim HostAdresses() As IPAddress
    Dim Num As Integer
    Dim IPs As ArrayList = New ArrayList

    Try
      If Computer = String.Empty Then
        ComputerName = Dns.GetHostName
      End If

      Host = Dns.GetHostEntry(ComputerName)
      HostAdresses = Host.AddressList
      Console.WriteLine("HostAdresses: " & HostAdresses.ToString & vbTab & HostAdresses.GetLength(0) - 1)

      For Num = 0 To HostAdresses.GetLength(0) - 1
        IPs.Add(HostAdresses(Num).ToString)
      Next

    Catch e As Exception
      MsgBox(e.Message & vbCrLf & _
            "Möglicherweise kann die IP-Adresse nicht ermittelt werden.", MsgBoxStyle.Critical, "GetIP_0")

    End Try


    Return IPs
  End Function

  Public Function GetIPHostName() As String
    Dim strResult As String = String.Empty

    Try
      strResult = Dns.GetHostName

    Catch e As Exception
      MsgBox(e.Message & vbCrLf & _
            "Möglicherweise kann keinen DNS-Namen ermittelt werden.", MsgBoxStyle.Critical, "GetIPHostName_0")

    End Try

    Return strResult
  End Function

  Public Function IsProcessRunning(ByVal strMyApp As String) As Boolean
    Return (System.Diagnostics.Process.GetProcessesByName(strMyApp).Length > 0)
  End Function

End Module