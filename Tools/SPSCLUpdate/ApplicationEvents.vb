

'Imports NLog

Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Net
Imports System.Net.Sockets

Namespace My

	' F�r MyApplication sind folgende Ereignisse verf�gbar:
	' 
	' Startup: Wird beim Starten der Anwendung noch vor dem Erstellen des Startformulars ausgel�st.
	' Shutdown: Wird nach dem Schlie�en aller Anwendungsformulare ausgel�st. Dieses Ereignis wird nicht ausgel�st, wenn die Anwendung nicht normal beendet wird.
	' UnhandledException: Wird ausgel�st, wenn in der Anwendung eine unbehandelte Ausnahme auftritt.
	' StartupNextInstance: Wird beim Starten einer Einzelinstanzanwendung ausgel�st, wenn diese bereits aktiv ist. 
	' NetworkAvailabilityChanged: Wird beim Herstellen oder Trennen der Netzwerkverbindung ausgel�st.
	Partial Friend Class MyApplication

		'Private Shared logger As Logger = LogManager.GetCurrentClassLogger()


#Region "private consts"

		Private Const URL_DOWNLOAD_SPS_FILE As String = "http://downloads.domain.com/sps_downloads/prog/settings/ProgUpdatesetting.txt"

		Public Sub New()

			'Dim frm As New frmMain()


			'If My.Application.CommandLineArgs.Count > 0 Then
			'	For Each locString As String In My.Application.CommandLineArgs

			'		'Alle unn�tigen Leerzeichen entfernen und 
			'		'Gro�-/Kleinschreibung 'Unsensiblisieren'
			'		'HINWEIS: Das funktioniert nur in der Windows-Welt;
			'		'kommt die Kopierlistendatei von einem Unix-Server, bitte darauf achten,
			'		'dass der Dateiname daf�r auch komplett in Gro�buchstaben gesetzt ist,
			'		'da Unix- (und Linux-) Derivate Gro�-/Kleinschreibung ber�cksichtigen!!!
			'		locString = locString.ToUpper.Trim

			'		If locString.Contains("/SILENT") Then
			'			frm.Visible = False
			'		End If

			'		If locString.Contains("/AUTOSTART") Then
			'			frm.ProcessUpdate()
			'		End If
			'	Next

			'Else
			'	frm.Show()
			'	frm.BringToFront()

			'End If


		End Sub

#End Region




		' TODO: 
		'Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup

		'	'Das Verzeichnis f�r die Protokolldatei beim ersten Mal setzen...
		'	If String.IsNullOrEmpty(My.Settings.Option_AutoSaveProtocolPath) Then
		'		My.Settings.Option_AutoSaveProtocolPath = Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "\SPClUpdate Protokolle")
		'		Dim locDi As New DirectoryInfo(My.Settings.Option_AutoSaveProtocolPath)

		'		'�berpr�fen und
		'		If Not locDi.Exists Then
		'			'im Bedarfsfall anlegen
		'			locDi.Create()
		'		End If

		'		'Settings speichern
		'		My.Settings.Save()
		'	End If

		'	Dim locFrmMain As New frmMain
		'	'Dim args() As String
		'	'If My.Application.CommandLineArgs.Count > 0 Then
		'	'  args = {"/SILENT", "/AUTOSTART"}
		'	'Else
		'	'  args = Environment.GetCommandLineArgs()
		'	'End If

		'	'Kommandozeile auslesen
		'	If My.Application.CommandLineArgs.Count > 0 Then
		'		For Each locString As String In My.Application.CommandLineArgs

		'			'Alle unn�tigen Leerzeichen entfernen und 
		'			'Gro�-/Kleinschreibung 'Unsensiblisieren'
		'			'HINWEIS: Das funktioniert nur in der Windows-Welt;
		'			'kommt die Kopierlistendatei von einem Unix-Server, bitte darauf achten,
		'			'dass der Dateiname daf�r auch komplett in Gro�buchstaben gesetzt ist,
		'			'da Unix- (und Linux-) Derivate Gro�-/Kleinschreibung ber�cksichtigen!!!
		'			locString = locString.ToUpper.Trim

		'			If locString = "/SILENT" Then
		'				locFrmMain.SilentMode = True
		'			End If

		'			If locString.StartsWith("/AUTOSTART") Then
		'				locFrmMain.AutoStartMode = True
		'			End If
		'		Next

		'	Else
		'		locFrmMain.SilentMode = True
		'		locFrmMain.AutoStartMode = True
		'	End If
		'	'locFrmMain.SilentMode = False
		'	'locFrmMain.AutoStartMode = False

		'	'Silentmode bleibt nur "an", wenn AutoStart aktiv ist.
		'	locFrmMain.SilentMode = locFrmMain.SilentMode And locFrmMain.AutoStartMode

		'	'Und wenn Silentmode, erfolgt keine Bindung des Formulars an den Anwendungskontext!
		'	If locFrmMain.SilentMode Then

		'		Dim _ClsSystem As New ClsMain_Net
		'		'die Server und locale Verzeichnisse ermitteln...
		'		Dim strSrvRootPath As String = _ClsSystem.GetSrvRootPath()
		'		Dim txtSPSFile As String = My.Settings.txt_UpdateFile
		'		'If txtSPSFile.IndexOf("\") > -1 Then
		'		'  If txtSPSFile = String.Empty Then
		'		'    txtSPSFile = strSrvRootPath & "Bin\ProgUpdatesetting.sps"
		'		'  Else
		'		'    txtSPSFile = txtSPSFile.ToUpper.Replace("$ServerSputnik$".ToUpper, strSrvRootPath)
		'		'  End If
		'		'End If
		'		'If File.Exists(txtSPSFile) Then
		'		'  FileCopy(txtSPSFile, _ClsSystem.GetLocalPath() & "Binn\ProgUpdatesetting.sps")
		'		'  txtSPSFile = _ClsSystem.GetLocalPath() & "Binn\ProgUpdatesetting.sps"
		'		'End If



		'		Dim progpath As String = Directory.GetCurrentDirectory()
		'		txtSPSFile = Path.Combine(progpath, "ProgUpdatesetting.sps")
		'		'logger.Debug(txtSPSFile)

		'		If Not File.Exists(txtSPSFile) Then
		'			If DownloadFile(URL_DOWNLOAD_SPS_FILE, txtSPSFile) Then
		'				logger.Error(String.Format("Die Datei {0} konnte nicht geladen werden.", txtSPSFile))

		'				Return
		'			End If
		'		End If
		'		locFrmMain.AutoStartCopyList = txtSPSFile


		'		Dim _clsEventLog As New ClsEventLog
		'		'Alles wird in der nicht sichtbaren Instanz des Hauptforms durchgef�hrt,
		'		_clsEventLog.WriteToEventLog(Now.ToString & vbTab & "SPSClUpdate wird im Autostartmode gestartet...")

		'		locFrmMain.HandleAutoStart()
		'		'und bevor das "eigentliche" Programm durch das Hauptformular gestartet wird,
		'		'ist der ganze Zauber auch schon wieder vorbei.
		'		_clsEventLog.WriteToEventLog(Now.ToString & vbTab & "SPSClUpdate wird beendet...")

		'		_clsEventLog.SaveTextToFile(locFrmMain.txtProtocol.Text, _ClsSystem.GetUserHomePath & "UpdateLOG.txt")
		'		'_clsEventLog.SendFileWithWebServer(0, _ClsSystem.GetUserHomePath & "UpdateLOG.txt")

		'		e.Cancel = True
		'	Else
		'		'Im Nicht-Silent-Modus wird das Formular an die Anwendung gebunden,
		'		'und los geht's!
		'		My.Application.MainForm = locFrmMain
		'	End If
		'End Sub

		'Private Function DownloadFile(ByVal _url As String, ByVal _filename As String) As Boolean

		'	Try
		'		If File.Exists(_filename) Then File.Delete(_filename)
		'	Catch ex As Exception
		'		logger.Error(String.Format("{0}", ex.ToString))
		'	End Try

		'	Try
		'		Dim _webClient As New System.Net.WebClient
		'		_webClient.UseDefaultCredentials = True

		'		_webClient.DownloadFile(_url, _filename)
		'		Return IO.File.Exists(_filename)

		'	Catch ex As Exception
		'		logger.Error(String.Format("{0}", ex.ToString))

		'		Return False
		'	End Try

		'End Function

	End Class

End Namespace

