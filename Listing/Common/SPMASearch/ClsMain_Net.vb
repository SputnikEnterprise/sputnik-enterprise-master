
Imports System.IO.File
Imports System.Reflection
Imports SP.Infrastructure.Logging

Public Class ClsMain_Net


#Region "Startfunktionen..."

	Sub ShowfrmMASearch()
		Dim frmTest As frmMASearch

		ClsDataDetail.GetVakNr = 0
		frmTest = New frmMASearch
		frmTest.Show()

	End Sub

	Sub ShowfrmMASearch_Temp_1(ByVal iVakNr As Integer)
		Dim frmTest_1 As frmVakSearch_Template_1

		ClsDataDetail.GetVakNr = iVakNr
		frmTest_1 = New frmVakSearch_Template_1
		frmTest_1.Show()

	End Sub

	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub

	Public Sub New(ByVal _setting As ClsSetting)

		Dim currentDomain As AppDomain = AppDomain.CurrentDomain

		AddHandler currentDomain.AssemblyResolve, AddressOf MyResolveEventHandler

		ClsDataDetail.ProgSettingData = _setting

		ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
		ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, 0)
		If _setting.SelectedMDNr = 0 Then ClsDataDetail.ProgSettingData.SelectedMDNr = ClsDataDetail.MDData.MDNr
		ClsDataDetail.GetSelectedMDConnstring = ClsDataDetail.MDData.MDDbConn

		If _setting.LogedUSNr = 0 Then ClsDataDetail.ProgSettingData.LogedUSNr = ClsDataDetail.UserData.UserNr

		If ClsDataDetail.ProgSettingData.TranslationItems Is Nothing Then
			ClsDataDetail.ProsonalizedData = ClsDataDetail.ProsonalizedName
			ClsDataDetail.TranslationData = ClsDataDetail.Translation
		Else
			ClsDataDetail.TranslationData = ClsDataDetail.ProgSettingData.TranslationItems
		End If
		ClsDataDetail.m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData)

		Application.EnableVisualStyles()

	End Sub

#End Region


#Region "Helpers"

	''' <summary>
	''' This handler is called only when the common language runtime tries to bind to the assembly and fails.        
	''' </summary>
	Private Function MyResolveEventHandler(sender As Object, args As ResolveEventArgs) As Assembly

		Dim m_Logger As ILogger = New Logger()

		'Retrieve the list of referenced assemblies in an array of AssemblyName.
		Dim objExecutingAssemblies As [Assembly]
		objExecutingAssemblies = [Assembly].GetExecutingAssembly()
		Dim arrReferencedAssmbNames() As AssemblyName
		arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies()

		'Loop through the array of referenced assembly names.
		Dim strAssmbName As AssemblyName
		For Each strAssmbName In arrReferencedAssmbNames

			'Look for the assembly names that have raised the "AssemblyResolve" event.
			If (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) = args.Name.Substring(0, args.Name.IndexOf(","))) Then

				'Build the path of the assembly from where it has to be loaded.
				Dim strTempAssmbPath As String = String.Empty
				strTempAssmbPath = IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), args.Name.Substring(0, args.Name.IndexOf(",")) & ".dll")

				If IO.File.Exists(strTempAssmbPath) Then
					Dim msg = String.Format("loading Assembly: {0}", strTempAssmbPath)
					m_Logger.LogWarning(msg)
					Trace.WriteLine(String.Format("loading Assembly: ", strTempAssmbPath))
					Dim MyAssembly As [Assembly]

					'Load the assembly from the specified path. 
					MyAssembly = [Assembly].LoadFrom(strTempAssmbPath)

					'Return the loaded assembly.
					Return MyAssembly
				Else
					Dim msg = String.Format("Assembly could not be found: {0}", strTempAssmbPath)
					m_Logger.LogWarning(msg)
					Trace.WriteLine(msg)
				End If

			End If
		Next

		Return Nothing

	End Function

#End Region


End Class
