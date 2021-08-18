
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Imports DevExpress.XtraSplashScreen
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SP.Main.Notify.UI
Imports System.Reflection
Imports DevExpress.XtraEditors

Friend NotInheritable Class Program

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

			WindowsFormsSettings.ColumnAutoFilterMode = ColumnAutoFilterMode.Text
			WindowsFormsSettings.AllowAutoFilterConditionChange = DevExpress.Utils.DefaultBoolean.False

			Dim init = CreateInitialData(0, 0)

			'Application.Run(New frmNotify(init))
			Dim frm As New frmNotify(init)
			frm.PreselectionData = New PreselectionData With {.CustomerID = init.MDData.MDGuid, .UserNumber = init.UserData.UserNr}
			'frm.StartLoadingData()
			Application.Run(frm)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString, "Notifying User",
																																													 MessageBoxButtons.OK, MessageBoxIcon.Error,
																																													 MessageBoxDefaultButton.Button1)
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
		m_Logger = New Logger

		m_Logger.LogError(t.ExceptionObject.ToString)
		DevExpress.XtraEditors.XtraMessageBox.Show(t.ExceptionObject.ToString, DevExpress.Utils.DefaultBoolean.Default)

	End Sub

	Private Shared Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


End Class

