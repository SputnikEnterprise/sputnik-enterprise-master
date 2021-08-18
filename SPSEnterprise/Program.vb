

Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports NLog

Imports DevExpress.XtraSplashScreen



Friend NotInheritable Class Program


	'Private Shared m_Translate As TranslateValues
	Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

	''' <summary>
	''' The main entry point for the application.
	''' </summary>
	Private Sub New()
		'logger = logger
	End Sub

	<STAThread()> _
	Shared Sub Main()

		AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf GlobalExceptionHandler

		Try
			'm_Translate = New TranslateValues

			'ModulConstants.MDData = ModulConstants.SelectedMDData(0)
			'ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)
			'ModulConstants.SecLevelData = ModulConstants.LogedUSSecData(ModulConstants.MDData.MDNr, ModulConstants.UserData.UserNr)

			'ModulConstants.ProsonalizedData = ModulConstants.ProsonalizedValues
			'ModulConstants.TranslationData = ModulConstants.TranslationValues

			'SplashScreenManager.ShowForm(GetType(SplashScreen2))


			Application.Run(New frmSelectMD())

		Catch ex As Exception
			logger.Error(ex.ToString)
			DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString, "Hauptübersicht", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
		End Try

	End Sub

	Private Shared Sub GlobalExceptionHandler(ByVal sender As Object, ByVal t As UnhandledExceptionEventArgs)

		'm_Logger.LogError(t.ExceptionObject.ToString)
		DevExpress.XtraEditors.XtraMessageBox.Show(t.ExceptionObject.ToString, "Sputnik Enterprise Suite",
																							 MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)

		logger.Error(t.ExceptionObject.ToString)

	End Sub


End Class


