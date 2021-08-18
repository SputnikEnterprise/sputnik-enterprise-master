
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

'Imports NLog
'Imports SP.Infrastructure.UI
'Imports SP.Infrastructure
'Imports SP.Infrastructure.Logging


Friend NotInheritable Class Program


	Private Shared m_Logger As Logging.ILogger

	''' <summary>
	''' The main entry point for the application.
	''' </summary>
	Private Sub New()

	End Sub

	<STAThread()> _
	Shared Sub Main()
		m_Logger = New Logging.Logger

		AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf GlobalExceptionHandler

		Try
			'Application.Run(New frmMain())


			If My.Application.CommandLineArgs.Count > 0 Then
				For Each locString As String In My.Application.CommandLineArgs

					'Alle unnötigen Leerzeichen entfernen und 
					'Groß-/Kleinschreibung 'Unsensiblisieren'
					'HINWEIS: Das funktioniert nur in der Windows-Welt;
					'kommt die Kopierlistendatei von einem Unix-Server, bitte darauf achten,
					'dass der Dateiname dafür auch komplett in Großbuchstaben gesetzt ist,
					'da Unix- (und Linux-) Derivate Groß-/Kleinschreibung berücksichtigen!!!
					locString = locString.ToUpper.Trim
					m_Logger.LogInfo(String.Format("starting program with parameter: {0}", locString.ToString))

					If locString.Contains("/SILENT") Then
						Dim frm As New frmMain()
						frm.AUTOSTART = False
						frm.Visible = False
					End If

					If locString.Contains("/AUTOSTART") Then
						Dim frm As New frmMain()
						frm.AUTOSTART = True
						'frm.Show()
						frm.ProcessUpdate()
						'End
					End If
				Next

			Else
				m_Logger.LogInfo(String.Format("starting program with no parameter"))
				Application.Run(New frmMain())

				'Dim frm As New frmMain()
				'frm.AUTOSTART = False
				''frm.ProcessUpdate()
				''End
				'frm.Show()
				'frm.BringToFront()

			End If



		Catch ex As Exception
			DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString, "Update-Download", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
		End Try

	End Sub

	Private Shared Sub GlobalExceptionHandler(ByVal sender As Object, ByVal t As UnhandledExceptionEventArgs)

		DevExpress.XtraEditors.XtraMessageBox.Show(t.ExceptionObject.ToString, ("Update-Download"), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)

		'm_Logger.LogError(t.Exception.ToString)

	End Sub


End Class

