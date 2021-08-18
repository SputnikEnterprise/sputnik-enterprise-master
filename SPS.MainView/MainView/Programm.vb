
Imports DevExpress.XtraSplashScreen
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.Reflection

Friend NotInheritable Class Program

	Private Shared m_Translate As TranslateValues

	Private Shared m_Logger As ILogger

	''' <summary>
	''' The main entry point for the application.
	''' </summary>
	Private Sub New()
		m_Logger = New Logger
	End Sub


	<STAThread()>
	Shared Sub Main()
		m_Logger = New Logger
		Dim currentDomain As AppDomain = AppDomain.CurrentDomain

		AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf GlobalExceptionHandler
		AddHandler currentDomain.AssemblyResolve, AddressOf MyResolveEventHandler

		Try
			m_Translate = New TranslateValues

			ModulConstants.MDData = ModulConstants.SelectedMDData(0)
			If ModulConstants.MDData Is Nothing Then
				Throw New Exception("Die Mandantendaten konnten nicht geladen werden. Das Programm wird beendet.")
			End If
			ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)
			m_Logger.LogDebug(String.Format("Starting: ModulConstants.LogedUSSecData: {0}", ModulConstants.UserData.UserNr))
			ModulConstants.SecLevelData = ModulConstants.LogedUSSecData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)
			m_Logger.LogDebug(String.Format("Ending: ModulConstants.LogedUSSecData: {0}", ModulConstants.UserData.UserNr))

			ModulConstants.ProsonalizedData = ModulConstants.ProsonalizedValues
			ModulConstants.TranslationData = ModulConstants.TranslationValues

			SplashScreenManager.ShowForm(GetType(SplashScreen2))

			Application.Run(New frmMainView())

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString, "Hauptübersicht", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
		End Try

	End Sub


	Private Shared Function MyResolveEventHandler(sender As Object, args As ResolveEventArgs) As Assembly
		'This handler is called only when the common language runtime tries to bind to the assembly and fails.        
		m_Logger = New Logger

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


	Private Shared Sub GlobalExceptionHandler(ByVal sender As Object, ByVal t As UnhandledExceptionEventArgs)

		If m_Logger Is Nothing Then m_Logger = New Logger
		m_Logger.LogError(t.ExceptionObject.ToString)
		DevExpress.XtraEditors.XtraMessageBox.Show(t.ExceptionObject.ToString, m_Translate.GetSafeTranslationValue("Hauptübersicht"), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)

		'm_Logger.LogError(t.Exception.ToString)

	End Sub


End Class

