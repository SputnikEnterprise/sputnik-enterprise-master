
Option Strict Off
Option Explicit On

Imports System.IO
Imports System.Management
Imports SP.Infrastructure.Logging

Imports System
Imports System.Net

Module IPFunctions

	Private m_Logger As ILogger

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
		'For Each prog In Process.GetProcesses
		'	Trace.WriteLine(prog.ProcessName)
		'Next
		Return (System.Diagnostics.Process.GetProcessesByName(strMyApp).Length > 0)
	End Function

	Public Function IsProcessRunningUnderLogedUserName_1(ByVal ProcessName As String) As Boolean
		Dim selectQuery As SelectQuery = New SelectQuery("Win32_Process")
		Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher(selectQuery)
		'Dim y As System.Management.ManagementObjectCollection
		Dim serverName = System.Net.Dns.GetHostName
		Dim strObject = "winmgmts://" & serverName
		Dim strNameOfUser = serverName


		m_Logger = New Logger

		'y = searcher.Get
		For Each Proc In GetObject(strObject).InstancesOf("win32_process")
			If UCase(Proc.Name) = UCase(ProcessName) Then
				Proc.GetOwner(strNameOfUser)
				MsgBox(strNameOfUser)
				Return True
			End If

		Next


		Return False

	End Function

	Public Function IsProcessRunningUnderLogedUserName(ByVal ProcessName As String) As Boolean
		Dim selectQuery As SelectQuery = New SelectQuery("Win32_Process")
		selectQuery.Condition = "Name = '" & ProcessName & ".exe'"
		Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher(selectQuery)
		Dim y As System.Management.ManagementObjectCollection
		Dim serverName = System.Net.Dns.GetHostName
		Dim userName = Environment.UserName
		Dim strObject = "winmgmts://" & serverName

		m_Logger = New Logger

		y = searcher.Get
		For Each proc As ManagementObject In y
			Dim s(1) As String
			proc.InvokeMethod("GetOwner", CType(s, Object()))
			Dim n As String = proc("Name").ToString().ToUpper

			Trace.WriteLine(String.Format("{0}: \\{1}\{2}", n, s(1), s(0)))
			If n = String.Format("{0}.exe".ToUpper, ProcessName.ToString.ToUpper) AndAlso s(0) = userName Then

				Return True	' ("User: " & s(1) & "\\" & s(0))
			End If
		Next


		Return False

	End Function

	Function IsProcessRunning_WithExtension()
		Dim Process, strObject
		IsProcessRunning_WithExtension = False
		strObject = "winmgmts://" & System.Net.Dns.GetHostName
		For Each Process In GetObject(strObject).InstancesOf("win32_process")
			Trace.WriteLine(Process.name)

			'If UCase(Process.name) = UCase(strProcess) Then
			'	IsProcessRunning_2 = True
			'	Exit Function
			'End If
		Next
	End Function

	Public Function isRunning(ByVal Process As String) As Boolean
		m_Logger = New Logger

		Dim objWMIService, colProcesses

		objWMIService = GetObject("winmgmts:")

		colProcesses = objWMIService.ExecQuery("Select * from Win32_Process")	' where name='" & Process & "'")
		For Each itm In colProcesses
			m_Logger.LogDebug(itm.ToString)



		Next

		If colProcesses.Count Then

			isRunning = True

		Else

			isRunning = False

		End If

	End Function

	Public Function IsProcessRunning_2(ByVal Process As String) As Boolean
		m_Logger = New Logger

		Dim strComputer = "."
		Dim strOwner = ""
		Dim strUserDomain = ""
		Dim strNameOfUser = ""

		Dim colProcesses = GetObject("winmgmts:" & _
			 "{impersonationLevel=impersonate}!\\" & strComputer & _
			 "\root\cimv2").ExecQuery("Select * from Win32_Process")

		For Each objProcess In colProcesses
			Dim test = objProcess.GetOwner() 'strNameOfUser)
			Dim processName As String = objProcess.Name

			If test <> 0 Then
				'm_Logger.LogDebug(String.Format("Name: {0}", processName))

			Else
				'm_Logger.LogDebug(String.Format("Name: {0} Is Owner", processName))
				Trace.WriteLine(String.Format("Name: {0} Is Owner", processName))
			End If
			If processName.ToUpper.Contains(Process.ToUpper) Then
				Return True
			End If

		Next


		Return False

	End Function


End Module