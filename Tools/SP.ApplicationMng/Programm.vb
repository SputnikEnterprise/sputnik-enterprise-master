
Imports System.Threading
Imports SP.Infrastructure.Logging

Namespace UI

	Friend NotInheritable Class Program

		Private Const APPNAME As String = "SP.ApplicationMng"

		Private Shared m_Logger As ILogger
		Private Shared mutex As Mutex = Nothing

		''' <summary>
		''' The main entry point for the application.
		''' </summary>
		Private Sub New()
			m_Logger = New Logger
		End Sub

		<STAThread()> _
		Shared Sub Main()
			m_Logger = New Logger

			AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf GlobalExceptionHandler

			Try
				Dim createdNew As Boolean
				mutex = New Mutex(True, appName, createdNew)

				If Not createdNew Then
					DevExpress.XtraEditors.XtraMessageBox.Show("Application is allready processing!", "Aplication", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)

					Return
				End If

				Application.Run(New frmStartProgram())


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				DevExpress.XtraEditors.XtraMessageBox.Show(ex.ToString, "Application-Management", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
			End Try

		End Sub

		Private Shared Sub GlobalExceptionHandler(ByVal sender As Object, ByVal t As UnhandledExceptionEventArgs)

			If m_Logger Is Nothing Then m_Logger = New Logger
			m_Logger.LogError(t.ExceptionObject.ToString)
			DevExpress.XtraEditors.XtraMessageBox.Show(t.ExceptionObject.ToString, "Application-Management", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)

		End Sub


#Region "Helpers"

		Private Shared Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

#End Region


	End Class


End Namespace

