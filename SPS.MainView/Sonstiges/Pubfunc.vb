

Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Web

Imports SPProgUtility.SPTranslation.ClsTranslation


Imports System
Imports System.Data.SqlClient
Imports System.IO
Imports DevExpress.XtraRichEdit
Imports DevExpress.Utils
Imports System.Reflection

Imports System.Management
Imports Microsoft.Win32
Imports System.Security.Principal

Imports SP.Infrastructure.Logging
Imports System.Collections.ObjectModel
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports System.Security.AccessControl
Imports SP.Infrastructure.UI



Module PubFunc

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private strConnString As String = _ClsProgSetting.GetConnString()
	Private strMDIniFile As String = _ClsProgSetting.GetMDIniFile()

	Private strMDProgFile As String = _ClsProgSetting.GetMDIniFile()
	Private strInitProgFile As String = _ClsProgSetting.GetInitIniFile()


#Region "Funktionen zur Ausfüllen der Nvabar..."

	Function GetImageFromResource(ByVal fileName As String) As Image
		'Dim resource As Stream = GetType(Form).Assembly.GetManifestResourceStream(fileName) '"Resources." & fileName)
		Dim _imageStream As Stream
		Dim _assembly As [Assembly]

		_assembly = [Assembly].GetExecutingAssembly()
		_imageStream = _assembly.GetManifestResourceStream(String.Format("{0}.{1}",
																																		 _assembly.GetName.Name.Trim, fileName))



		Return Image.FromStream(_imageStream)
	End Function

	Function GetImageData(ByVal fileName As String) As Byte()
		Dim img As Image = GetImageFromResource(fileName)
		Dim mem As New MemoryStream()
		img.Save(mem, System.Drawing.Imaging.ImageFormat.Bmp)
		Return mem.GetBuffer()
	End Function

#End Region



	Sub GettingTest()
		GetMACAddress()

	End Sub

	Function GetOsIdentitiy() As String
		Dim strResult As String = String.Empty

		AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal)

		Dim user As WindowsPrincipal = CType(System.Threading.Thread.CurrentPrincipal, WindowsPrincipal)
		Dim ident As WindowsIdentity = user.Identity
		Dim ident_W As WindowsIdentity = WindowsIdentity.GetCurrent()
		Dim user_w As New WindowsPrincipal(ident)



		Dim objIdentity As WindowsIdentity = WindowsIdentity.GetCurrent
		Dim objPrincipal As New Security.Principal.WindowsPrincipal(objIdentity)
		Trace.WriteLine(objPrincipal.Identity.IsAuthenticated.ToString())
		Trace.WriteLine(objIdentity.IsGuest.ToString())
		Trace.WriteLine(objIdentity.ToString())

		getOsArchitecture(My.Computer.Name)

		Return strResult
	End Function

	Function GetMACAddress() As String
		Dim strNetworkMacAdress As String = "MAC-Adresse: {0}"
		Dim strDiskID As String = "DiskID: {0}"
		Dim strProcessorID As String = "ProcessID: {0}"
		Dim strValues As String = String.Empty

		Try

			Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
			Dim moc As ManagementObjectCollection = mc.GetInstances()
			Dim strMyMACAddress As String = ""
			For Each mo As ManagementObject In moc
				If (strMyMACAddress.Equals(String.Empty)) Then
					If CBool(mo("IPEnabled")) Then strMyMACAddress = mo("MacAddress").ToString()
					mo.Dispose()
				End If
				strMyMACAddress = strMyMACAddress.Replace(":", String.Empty)
			Next
			strNetworkMacAdress = String.Format(strNetworkMacAdress, strMyMACAddress)

		Catch ex As Exception

		End Try

		Try
			Dim strMyDiskID As String = "Not defined!"
			Dim disk As ManagementObject = New ManagementObject(String.Format("win32_logicaldisk.deviceid=""{0}:""", "C"))
			disk.Get()
			strMyDiskID = disk("VolumeSerialNumber").ToString()

			strDiskID = String.Format(strDiskID, strMyDiskID)

		Catch ex As Exception

		End Try

		Try
			Dim query As New SelectQuery("Win32_processor")
			Dim search As New ManagementObjectSearcher(query)
			Dim info As ManagementObject
			Dim strMyProcID As String = "Not defined!"

			For Each info In search.Get()
				strMyProcID = info("processorId").ToString()
			Next

			strProcessorID = String.Format(strProcessorID, strMyProcID)

		Catch ex As Exception

		End Try

		strValues = String.Format("{0}|{1}|{2}", strNetworkMacAdress, strDiskID, strProcessorID)


		Dim irc As IdentityReferenceCollection
		Dim ir As IdentityReference
		irc = WindowsIdentity.GetCurrent().Groups
		Dim strGroupName As String

		For Each ir In irc
			Dim mktGroup As IdentityReference = ir.Translate(GetType(NTAccount))
			Trace.WriteLine(mktGroup.Value)
			Debug.WriteLine(mktGroup.Value)
			strGroupName = mktGroup.Value.ToString

		Next

		Return strValues
	End Function

	Public Function getOsArchitecture(ByVal Computer As String) As String
		Dim strValue As String = ""
		Dim objWMIService As Object
		Dim objItems As Object
		Dim objItem As Object

		Try

			objWMIService = GetObject("winmgmts:\\" & Computer & "\root\CIMV2")
			objItems = objWMIService.ExecQuery( _
					"SELECT * FROM Win32_OperatingSystem")
			For Each objItem In objItems
				strValue = objItem.OSArchitecture
			Next

			Dim searcher As New ManagementObjectSearcher(
								 "root\CIMV2",
								 "SELECT * FROM Win32_BaseBoard")   'Win32_BIOS")

			For Each queryObj As ManagementObject In searcher.Get()

				Console.WriteLine("-----------------------------------")
				Console.WriteLine("Win32_BIOS instance")
				Console.WriteLine("-----------------------------------")
				Console.WriteLine("SerialNumber: {0}", queryObj("SerialNumber"))
			Next


			Dim oConn As ConnectionOptions = New ConnectionOptions

			Dim oMs As System.Management.ManagementScope = New System.Management.ManagementScope("\\machineID", oConn)
			Dim oQuery As System.Management.ObjectQuery = New System.Management.ObjectQuery("select * from Win32_BaseBoard")
			Dim oSearcher As ManagementObjectSearcher = New ManagementObjectSearcher(oMs, oQuery)
			Dim oReturnCollection As ManagementObjectCollection = oSearcher.Get
			For Each oReturn As ManagementObject In oReturnCollection
				Console.WriteLine(("Serial Number : " + oReturn("SerialNumber").ToString))
			Next
			Console.ReadLine()



			objWMIService = Nothing
			objItems = Nothing
			objItem = Nothing

		Catch ex As Exception

		End Try

		Return strValue
	End Function

	Private Function GetWindowsSerialnumber() As String
		Dim BaseKey As RegistryKey
		Dim SubKey As RegistryKey
		Dim strKey As String = String.Empty

		If Environment.Is64BitOperatingSystem = True Then
			BaseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
			SubKey = BaseKey.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", False)
		Else
			SubKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Windows NT\CurrentVersion", False)
		End If

		If Not SubKey Is Nothing Then
			Try

				Dim rpk As Byte() = DirectCast(SubKey.GetValue("DigitalProductId", New _
					Byte(-1) {}), Byte())

				Const iRPKOffset As Integer = 52
				Const strPossibleChars As String = "BCDFGHJKMPQRTVWXY2346789"
				Dim i As Integer = 28

				Do
					Dim lAccu As Long = 0
					Dim j As Integer = 14

					Do

						lAccu *= 256
						lAccu += Convert.ToInt64(rpk(iRPKOffset + j))
						rpk(iRPKOffset + j) = Convert.ToByte(Convert.ToInt64(Math.Floor(CSng(lAccu) / 24.0F)) And Convert.ToInt64(255))
						lAccu = lAccu Mod 24

						j -= 1
					Loop While j >= 0

					i -= 1
					strKey = strPossibleChars(CInt(lAccu)).ToString() & strKey
					If (0 = ((29 - i) Mod 6)) AndAlso (-1 <> i) Then
						i -= 1
						strKey = "-" & strKey
					End If
				Loop While i >= 0

			Catch ex As Exception

				strKey = ex.ToString
			End Try

		End If

		Return strKey
	End Function

	Function sGetXPKey() As String

		'Registry öffnen und Wert auslesen (byte array) 
		Dim RegKey As RegistryKey = _
		Registry.LocalMachine.OpenSubKey("Software\Microsoft\Windows NT\CurrentVersion", False)
		Dim bytDPID() As Byte = RegKey.GetValue("DigitalProductID")
		'Nur die benötigten Teile ins Array laden
		' Key beginnt ab byte 52 und ist 15 Bytes lang.
		Dim bytKey(14) As Byte '0-14 = 15 bytes
		Array.Copy(bytDPID, 52, bytKey, 0, 15)
		'Unser "Array" beinhaltet nun  gültige Zeichen
		Dim strChar As String = "BCDFGHJKMPQRTVWXY2346789"
		'jetzt muss der decodierte Schlüssel zurückgegeben werden
		Dim strKey As String = ""

		For j As Integer = 0 To 24
			Dim nCur As Short = 0
			For i As Integer = 14 To 0 Step -1
				nCur = CShort(nCur * 256 Xor bytKey(i))
				bytKey(i) = CByte(Int(nCur / 24))
				nCur = CShort(nCur Mod 24)
			Next
			strKey = strChar.Substring(nCur, 1) & strKey
		Next
		'nun muss das Ganze in einen String eingestzt werden.
		For i As Integer = 4 To 1 Step -1
			strKey = strKey.Insert(i * 5, "-")
		Next


		Dim strPName As String = RegKey.GetValue("ProductName")
		Dim strPID As String = RegKey.GetValue("ProductID")

		Dim lvi As New ListViewItem
		lvi.Text = strPName

		Try
			lvi.SubItems.Add(strKey)
		Catch ex As Exception
			lvi.SubItems.Add("n.a.")
		End Try


		Try
			lvi.SubItems.Add(strPID)
		Catch ex As Exception
			lvi.SubItems.Add("n.a.")
		End Try

		RegKey.Close()

		Return lvi.ToString
	End Function

	Public Sub ShowErrDetail(ByVal strtext As String)
		Dim utilityUI As New SP.Infrastructure.UI.UtilityUI

		utilityUI.ShowErrorDialog(strtext, TranslateText("Fehler"), MessageBoxIcon.Error)

	End Sub

End Module




