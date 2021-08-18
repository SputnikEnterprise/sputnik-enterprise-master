
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.UI.UtilityUI
Imports SPS.MainView.DataBaseAccess

Imports System.Data.SqlClient
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid

Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Metro.ColorTables
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports SP.Infrastructure.Logging
Imports DevExpress.XtraGrid.Views.Base
Imports System.IO
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports System.Security.AccessControl
Imports System.Net.NetworkInformation
Imports System.Net.Sockets
Imports SPProgUtility.ProgPath
Imports DevExpress.XtraSplashScreen
Imports System.Runtime.Serialization.Formatters.Binary

Public Class frmUpdate
	Private Shared m_Logger As ILogger = New Logger()


	Private Property Modul2Open As String

	Private m_translate As TranslateValues

	Private m_UitilityUI As UtilityUI

	Private m_FileVersion As String
	Private m_CopyList As CopyListEntry
	Private m_ServerUpdatePath As String
	Private m_UtilityUI As UtilityUI
	Private m_DoUpdate As Boolean

	Public Property m_UpdateFileresult As List(Of UpdateFileResult)


#Region "private consts"

	Private Const URL_DOWNLOAD_SPS_FILE As String = "http://downloads.domain.com/sps_downloads/prog/settings/ProgUpdatesetting.txt"
	Private Const SPS_SETTING_UPDATE_BACKUPLEVEL As Integer = 5

#End Region


	Public Sub New(ByVal DoUpdate As Boolean)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		m_ServerUpdatePath = Path.Combine(Directory.GetCurrentDirectory, "ProgUpdatesetting.sps")
		m_DoUpdate = DoUpdate
		m_UtilityUI = New UtilityUI
		m_UpdateFileresult = New List(Of UpdateFileResult)

		m_translate = New TranslateValues
		reset()

		AddHandler Me.gvDetail.RowCellClick, AddressOf Ongv_RowCellClick
		AddHandler Me.gvDetail.CustomDrawEmptyForeground, AddressOf CustomDrawEmptyForeground

	End Sub


	Private Sub reset()
		ResetGrid()
		TranslateControls()
	End Sub

	Private Sub TranslateControls()
		Dim strTitle As String = "Liste der Updates"

		Me.Text = m_translate.GetSafeTranslationValue(Me.Text)
		Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvDetail.RowCount)

	End Sub

	Private Sub Onform_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.SETTING_UPDATE_LOCATION = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.SETTING_UPDATE_WIDTH = Me.Width
				My.Settings.SETTING_UPDATE_HEIGHT = Me.Height

				My.Settings.Save()
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

	End Sub


	Private Sub Onform_Load(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			If My.Settings.SETTING_UPDATE_HEIGHT > 0 Then Me.Height = Math.Max(Me.Height, My.Settings.SETTING_UPDATE_HEIGHT)
			If My.Settings.SETTING_UPDATE_WIDTH > 0 Then Me.Width = Math.Max(Me.Width, My.Settings.SETTING_UPDATE_WIDTH)
			If My.Settings.SETTING_UPDATE_LOCATION <> String.Empty Then
				Dim aLoc As String() = My.Settings.SETTING_UPDATE_LOCATION.Split(CChar(";"))
				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Val(aLoc(0)), Val(aLoc(1)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formsizing. {1}", strMethodeName, ex.Message))

		End Try

		Me.sccMain.Dock = DockStyle.Fill

	End Sub


	Public Sub LoadFilesListForUpdate()
		Dim result As List(Of CopyListEntry) = Nothing

		m_UpdateFileresult = New List(Of UpdateFileResult)

		m_Logger.LogInfo(String.Format("m_ServerUpdatePath: {0}", m_ServerUpdatePath))
		result = LoadCopyEntryList(m_ServerUpdatePath)

		Dim FileName As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SPSUpdateCommand.txt")
		Dim csv = String.Empty

		Using sw As New IO.StreamWriter(FileName, False)
			For Each per In result
				csv = String.Format("{0} {1}\{2} --->>> {3}", per.UpdateCommand, per.SourceFolder, per.SearchMask, per.DestFolder)
				sw.WriteLine(csv)
			Next
		End Using

		CopyFiles(result)
		bbiRunUpdate.Visibility = If(m_UpdateFileresult.Count > 0, DevExpress.XtraBars.BarItemVisibility.Always, DevExpress.XtraBars.BarItemVisibility.Never)

	End Sub


	Public Sub CompareDirectory(ByVal pathA As String, ByVal pathB As String, ByVal searchPatern As String)

		' Create two identical or different temporary folders  
		' on a local drive and add files to them. 
		' Then set these file paths accordingly. 

		' Take a snapshot of the file system.  
		Dim dir1 As New System.IO.DirectoryInfo(pathA)
		Dim dir2 As New System.IO.DirectoryInfo(pathB)

		Dim list1 = dir1.GetFiles(searchPatern, System.IO.SearchOption.TopDirectoryOnly)
		Dim list2 = dir2.GetFiles(searchPatern, System.IO.SearchOption.TopDirectoryOnly)

		If list1.Count = 0 Then Return
		' Create the FileCompare object we'll use in each query 
		Dim myFileCompare As New FileCompare

		' This query determines whether the two folders contain 
		' identical file lists, based on the custom file comparer 
		' that is defined in the FileCompare class. 
		' The query executes immediately because it returns a bool. 
		Dim areIdentical As Boolean = list1.SequenceEqual(list2, New FileCompare)	' myFileCompare)
		If areIdentical Then
			Trace.WriteLine("The two folders are the same.")
		Else
			Trace.WriteLine("The two folders are not the same.")
		End If

		' Find common files in both folders. It produces a sequence and doesn't execute 
		' until the foreach statement. 
		Dim queryCommonFiles = list1.Intersect(list2, New FileCompare) ' myFileCompare)

		For Each info In queryCommonFiles
			Trace.WriteLine(info.Name)
		Next

		If queryCommonFiles.Count() > 0 Then
			Trace.WriteLine("The following files are in both folders:")
			For Each fi As System.IO.FileInfo In queryCommonFiles
				Trace.WriteLine(fi.FullName)
			Next
		Else
			Trace.WriteLine("There are no common files in the two folders.")
		End If

		' Find the set difference between the two folders. 
		' For this example we only check one way. 
		Dim queryDirAOnly = list1.Except(list2, myFileCompare)
		Trace.WriteLine("The following files are in dirA but not dirB:")
		For Each fi As System.IO.FileInfo In queryDirAOnly
			Trace.WriteLine(fi.FullName)
		Next

		' Keep the console window open in debug mode
		Trace.WriteLine("Press any key to exit.")
		'Console.ReadKey()

	End Sub


	Private Function LoadCopyEntryList(ByVal copyEntryListPath As String) As IEnumerable(Of CopyListEntry)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim uVersion As String = String.Empty
		Dim uTime As String = String.Empty
		Dim uDate As String = String.Empty
		Dim value As String = String.Empty

		Dim locCopyListEntry As List(Of CopyListEntry) = Nothing

		If Not IsIPAdressAllowedForUpdate() Then Return locCopyListEntry

		Dim locParser As FileIO.TextFieldParser

		Try
			GetNewerConfigFile(copyEntryListPath)
			m_Logger.LogInfo(String.Format("File wird gelesen: {0}", copyEntryListPath))
			locParser = My.Computer.FileSystem.OpenTextFieldParser(copyEntryListPath, ";")

		Catch ex As Exception
			m_Logger.LogWarning(String.Format("Error ({0}): {1}", strMethodeName, ex.ToString))
			Return locCopyListEntry
		End Try

		'Und die einzelnen CSV-Zeilen, die es nun enthält, solange durchgehen,
		'wie Zeilen vorhanden sind.
		Dim strSourcePath As String = String.Empty
		Dim strDestPath As String = String.Empty

		locCopyListEntry = New List(Of CopyListEntry)

		Try
			Do While Not locParser.EndOfData
				Try
					'Die jeweils nächste Zeile lesen und die einzelnen Felder
					'in ein String-Array separieren.
					Dim locFields As String() = locParser.ReadFields
					Dim data As New CopyListEntry

					strSourcePath = String.Empty
					strDestPath = String.Empty
					If locFields.Length > 1 Then
						If locFields(0).ToString.Trim <> String.Empty Then

							If locFields(0).ToString.ToUpper = "Delete".ToUpper Then
								data.UpdateCommand = "Delete"
								data.SourceFolder = New DirectoryInfo((locFields(0)))
								data.SearchMask = locFields(1)

								strDestPath = TranslateUpdatePath(locFields(2))
								If strDestPath.Length > 5 Then
									If strDestPath.Last = "\" Then strDestPath = Mid(strDestPath, 1, Len(strDestPath) - 1)
									data.DestFolder = New DirectoryInfo(strDestPath)
									data.DestFileDescription = locFields(3)
								End If

							Else
								data.UpdateCommand = "Copy"

								If Not locFields(2).ToString.ToUpper.Contains("$MD$".ToUpper) Then
									strSourcePath = TranslateUpdatePath(locFields(0))
									If strSourcePath.Length > 5 Then
										If strSourcePath.Last = "\" Then strSourcePath = Mid(strSourcePath, 1, Len(strSourcePath) - 1)
										data.SourceFolder = New DirectoryInfo(strSourcePath)
										data.SearchMask = locFields(1)

										strDestPath = TranslateUpdatePath(locFields(2))
										If strDestPath.Length > 5 Then
											If strDestPath.Last = "\" Then strDestPath = Mid(strDestPath, 1, Len(strDestPath) - 1)
											data.DestFolder = New DirectoryInfo(strDestPath)
											data.DestFileDescription = locFields(3)
										End If
									End If

								ElseIf locFields(2).ToString.ToUpper.Contains("$MD$".ToUpper) Then
									'GetMDPathFromDb(locFields)
								End If

							End If
						ElseIf locFields(1).ToString.ToUpper.Contains("$MD$=".ToUpper) Then
							value = Mid(locFields(1).ToString, "$MD$=".Length + 1)

						ElseIf locFields(1).ToString.ToUpper.Contains("Version=".ToUpper) Then
							value = Mid(locFields(1).ToString, "Version=".Length + 1)
							m_FileVersion = value
							uVersion = value

						ElseIf locFields(1).ToString.ToUpper.Contains("FileTime=".ToUpper) Then
							value = Mid(locFields(1).ToString, "FileTime=".Length + 1)
							uTime = value

						ElseIf locFields(1).ToString.ToUpper.Contains("FileDate=".ToUpper) Then
							value = Mid(locFields(1).ToString, "FileDate=".Length + 1)
							uDate = value

						End If
					End If
					data.UpdateTime = uTime
					data.UpdateDate = uDate
					data.UpdateVersion = uVersion

					locCopyListEntry.Add(data)

				Catch ex As Exception
					locCopyListEntry = Nothing
					m_Logger.LogError(String.Format("Error ({0}): {1}", strMethodeName, ex.ToString))

				End Try

			Loop

		Catch ex As Exception
			locCopyListEntry = Nothing
			m_Logger.LogError(String.Format("Error ({0}): {1}", strMethodeName, ex.ToString))

		End Try

		Return locCopyListEntry

	End Function

	''' <summary>
	''' Die eigentliche Kopierroutine, die durch die My.Settings-Optionen gesteuert wird.
	''' </summary>
	''' <remarks></remarks>
	Public Sub CopyFiles(ByVal copylist As List(Of CopyListEntry))
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim locCopyTaskItems As New ArrayList
		Dim locStartTime As Date = Now
		Dim locNotCaught As Boolean
		Dim data As New UpdateFileResult
		Dim result As String = String.Empty

		Try

			'In dieser Schleife werden zunächst alle Dateien ermittelt,
			'die es daraufhin zu untersuchen gilt, ob sie kopiert werden
			'müssen oder nicht.
			'For locItemCount As Integer = 0 To lvwCopyEntries.Items.Count - 1
			For Each locItemCount In copylist	'lvwCopyEntries.Items.Count - 1
				'Dim locCopyListEntry As CopyListEntry
				'CType ist notwendig, damit .NET weiß, welcher Typ
				'in der Tag-Eigenschaft gepspeichert war.
				'locCopyListEntry = CType(lvwCopyEntries.Items(locItemCount).Tag, CopyListEntry)

				'GetFiles aus My.Computer.FileSystem liefert die Namen aller 
				'Dateien im Stamm- und in dessen Unterverzeichnissen.
				If Not locItemCount.DestFolder Is Nothing Then
					Dim locFiles As ReadOnlyCollection(Of String) = Nothing
					Try
						If locItemCount.UpdateCommand.ToUpper = "Delete".ToUpper Then
							locNotCaught = True

							For Each myFile In Directory.GetFiles(locItemCount.DestFolder.ToString, locItemCount.SearchMask)
								Try
									'If myFile.Contains("IBANKernel") Then
									'	Trace.WriteLine(myFile)
									'End If

									If m_DoUpdate Then File.Delete(myFile)
									If m_DoUpdate Then
										result = "Erfolgreich gelöscht."
									Else
										result = "Wird gelöscht."
									End If

								Catch ex As Exception
									result = String.Format("{0}: Löschen Fehlerhaft.", ex.ToString)
								End Try
								data = New UpdateFileResult
								data.sourcefilename = String.Empty
								data.destfilename = myFile
								data.sourcefiledate = Nothing
								data.destfiledate = Nothing
								data.result = result

								m_UpdateFileresult.Add(data)
							Next

						Else
							locNotCaught = False

							CompareDirectory(locItemCount.SourceFolder.ToString, locItemCount.DestFolder.ToString, locItemCount.SearchMask)
							locFiles = My.Computer.FileSystem.GetFiles(locItemCount.SourceFolder.ToString, FileIO.SearchOption.SearchTopLevelOnly, locItemCount.SearchMask)
						End If

					Catch ex As Exception
						m_Logger.LogError(String.Format("Error ({0}): {1}", strMethodeName, ex.ToString))

						' Es müsste wie "Catch" einen "NotCaught"-Zweig geben...
						locNotCaught = True
					End Try

					'Das Zusammenstellen der Dateien nur ausführen, wenn beim
					'Einlesen der Dateien kein Fehler (protokolliert in locNotCaught)
					'aufgetreten ist!
					If Not locNotCaught Then
						' Durch alle Dateien iterieren, und die Quellpfade bilden.
						For Each locFile As String In locFiles
							Dim locCopyTaskItem As New CopyTaskItem
							locCopyTaskItem.SourceFile = New FileInfo(locFile)

							'Verzeichnisverweise an dieser Stelle übergehen, sodass nur
							'eine Liste mit Dateien innerhalb der Verzeichnisse bleibt.
							If Not (locCopyTaskItem.SourceFile.Attributes And FileAttributes.Directory) = FileAttributes.Directory Then
								locCopyTaskItem.SourcePathPart = locItemCount.SourceFolder.ToString
								locCopyTaskItem.DestPathPart = locItemCount.DestFolder.ToString
								locCopyTaskItems.Add(locCopyTaskItem)
							End If

						Next
					End If
				End If
			Next

			Dim locDoCopy As Boolean
			Dim locFileCount As Integer = 1

			For Each locFileItem As CopyTaskItem In locCopyTaskItems
				Dim locDestPath As DirectoryInfo = Nothing
				Dim locOldDestPath As DirectoryInfo = Nothing
				Dim locDestFile As FileInfo = Nothing

				locDoCopy = True
				Try

					'locDestPath = New DirectoryInfo(locFileItem.DestPathPart & locFileItem.SourceFile.Directory.ToUpper.Replace(locFileItem.SourcePathPart.ToUpper, ""))
					locDestPath = New DirectoryInfo(locFileItem.DestPathPart)
					locDestFile = New FileInfo(Path.Combine(locFileItem.DestPathPart, locFileItem.SourceFile.Name))

					'Wenn sich die Datei schon am Zielort befindet,
					'Dateien aber grundsätzlich nicht überschrieben werden sollen...
					If locDestFile.Exists Then
						'...dann die Datei nicht kopieren.
						locDoCopy = True

					End If

					'Wenn die Datei kopiert werden soll
					locFileCount += 1
					If locDoCopy Then
						If Not locDestPath.Exists Then locDestPath.Create()

						'Datei mit Historiepflege kopieren
						If locOldDestPath Is Nothing Then
							locOldDestPath = New DirectoryInfo(locDestPath.ToString)
						End If
						If locOldDestPath.FullName <> locDestPath.FullName Then
							locOldDestPath = New DirectoryInfo(locDestPath.ToString)
						End If
						Application.DoEvents()

						If m_DoUpdate Then
							CopyFileInternal(locFileItem.SourceFile, locDestFile)
						Else
							If AnalyseFileForUpdate(locFileItem.SourceFile, locDestFile) Then
								data = New UpdateFileResult
								result = "Wird upgedatet."
								data.sourcefilename = locFileItem.SourceFile.ToString
								data.destfilename = locDestFile.ToString
								data.sourcefiledate = locFileItem.SourceFile.LastAccessTime
								If File.Exists(locDestFile.FullName) Then
									data.destfiledate = locDestFile.LastAccessTime
								Else
									result = "Wird neu erstellt."
								End If

								data.result = result

								m_UpdateFileresult.Add(data)
							End If

						End If

					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("Error ({0}): {1}", strMethodeName, ex.ToString))

				End Try
			Next

			If m_DoUpdate Then SaveNewUpdateVersionToDb()

			'Und im Bedarfsfall abspeichern
			'Dim locProtocolFilename As String = "DNC" & Now.ToString("yyMMdd-HHmmss") & ".log"
			'Dim locDi As New DirectoryInfo(My.Settings.Option_AutoSaveProtocolPath)
			'If Not locDi.Exists Then
			'	locDi.Create()
			'End If
			'My.Computer.FileSystem.WriteAllText(locDi.ToString & "\" & locProtocolFilename, txtProtocol.Text, False, System.Text.Encoding.Default)


		Catch ex As Exception
			m_Logger.LogError(String.Format("Error ({0}): {1}", strMethodeName, ex.ToString))

		End Try

	End Sub

	Private Function AnalyseFileForUpdate(ByVal sourceFile As FileInfo, ByVal DestFile As FileInfo) As Boolean
		Dim result As Boolean = False

		'If DestFile.Name.Contains("IBANKernel") Then
		'	Trace.WriteLine(DestFile.Name)
		'End If
		If Not DestFile.Exists Then
			Return True
		Else
			If sourceFile.LastWriteTime <> DestFile.LastWriteTime Then Return True
		End If

		Return result

	End Function

	''' <summary>
	''' Interne Kopierroutine, die den eigentlichen 'Job' des Kopierens übernimmt.
	''' Achtung: eine Service wird falls vorhanden, entladet, dann kopiert und geladen.
	''' </summary>
	''' <param name="SourceFile"></param>
	''' <param name="DestFile"></param>
	''' <remarks></remarks>
	Private Sub CopyFileInternal(ByVal SourceFile As FileInfo, ByVal DestFile As FileInfo)
		Dim result As String = "Nicht nötig."

		Try
			If Not DestFile.Exists Then
				Try
					System.IO.File.Copy(SourceFile.ToString, DestFile.ToString)

				Catch ex As ReadOnlyException
					result = String.Format("Error: {0}: {1}", ex.ToString, DestFile.ToString)
					m_Logger.LogError(result)
					Return

				Catch ex As Exception
					result = String.Format("Error: {0}: {1}", ex.ToString, DestFile.ToString)
					m_Logger.LogError(result)
					Return

				End Try
				result = String.Format("Kopiert OK: {0}", DestFile.ToString)
				m_Logger.LogInfo(result)

			Else
				'Datei nur kopieren, wenn Sie unterschiedlich ist
				If SourceFile.LastWriteTime <> DestFile.LastWriteTime Then
					ManageFileBackupHistory(DestFile)

					Try
						System.IO.File.Copy(SourceFile.ToString, DestFile.ToString)
						File.SetCreationTime(DestFile.ToString, File.GetCreationTime(SourceFile.ToString))
						File.SetLastAccessTime(DestFile.ToString, File.GetLastAccessTime(SourceFile.ToString))
						File.SetLastWriteTime(DestFile.ToString, File.GetLastWriteTime(SourceFile.ToString))

					Catch ex As ReadOnlyException
						result = String.Format("Error: {0}: {1}", ex.ToString, DestFile.ToString)
						m_Logger.LogError(result)
						Return

					Catch ex As Exception
						result = String.Format("Error: {0}: {1}", ex.ToString, DestFile.ToString)
						m_Logger.LogError(result)
						Return

					End Try

					result = String.Format("Kopiert OK: {0}", DestFile.ToString)
					m_Logger.LogInfo(result)

				Else
					Return

				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("Error: {0}", ex.ToString))

		Finally

			If Not result.ToLower.Contains("Nicht nötig.".ToLower) Then
				Dim data As New UpdateFileResult
				data.sourcefilename = SourceFile.ToString
				data.destfilename = DestFile.ToString
				data.sourcefiledate = SourceFile.LastAccessTime
				If DestFile.LastAccessTime.Year <= 1900 Then
					data.destfiledate = Nothing
				Else
					data.destfiledate = DestFile.LastAccessTime
				End If
				data.result = result

				m_UpdateFileresult.Add(data)
			End If

		End Try

	End Sub




	Sub SaveNewUpdateVersionToDb()
		Dim sSql As String = "Insert [Sputnik DBSelect].Dbo.tbl_UpdateInfo (UpdateVersion, UpdateDescription, UpdateDate) Values (@FileVerion, @StInfo, @UpdateDate)"
		'Dim _clsSystem As New ClsMain_Net
		Dim strConnString As String = ModulConstants.MDData.MDDbConn ' _clsSystem.GetDbSelectConnString()
		Dim ConnRoot As New SqlConnection(strConnString)

		Try
			ConnRoot.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnRoot)
			Dim param As System.Data.SqlClient.SqlParameter = New SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@StInfo", Environment.MachineName & " / " & Environment.UserName)
			param = cmd.Parameters.AddWithValue("@FileVerion", m_FileVersion)
			param = cmd.Parameters.AddWithValue("@UpdateDate", CDate(Now))

			cmd.ExecuteNonQuery()
			cmd.Parameters.Clear()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		Finally
			ConnRoot.Dispose()

		End Try

	End Sub




#Region "Details for UpdateFileResult"

	Sub ResetGrid()

		gvDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus
		gvDetail.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never
		gvDetail.OptionsView.ShowGroupPanel = False
		gvDetail.OptionsView.ShowIndicator = False
		gvDetail.OptionsView.ShowAutoFilterRow = True

		gvDetail.Columns.Clear()

		Try

			Dim columnmodulname As New DevExpress.XtraGrid.Columns.GridColumn()
			columnmodulname.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnmodulname.Caption = m_translate.GetSafeTranslationValue("Quelle")
			columnmodulname.Name = "sourcefilename"
			columnmodulname.FieldName = "sourcefilename"
			columnmodulname.Width = 300
			columnmodulname.Visible = True
			gvDetail.Columns.Add(columnmodulname)

			Dim columnMonth As New DevExpress.XtraGrid.Columns.GridColumn()
			columnMonth.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnMonth.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnMonth.Name = "sourcefiledate"
			columnMonth.FieldName = "sourcefiledate"
			columnMonth.Width = 60
			columnMonth.Visible = True
			gvDetail.Columns.Add(columnMonth)

			Dim columnLONr As New DevExpress.XtraGrid.Columns.GridColumn()
			columnLONr.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnLONr.Caption = m_translate.GetSafeTranslationValue("Ziel")
			columnLONr.Name = "destfilename"
			columnLONr.FieldName = "destfilename"
			columnLONr.Width = 300
			columnLONr.Visible = True
			gvDetail.Columns.Add(columnLONr)

			Dim columnYear As New DevExpress.XtraGrid.Columns.GridColumn()
			columnYear.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnYear.Caption = m_translate.GetSafeTranslationValue("Datum")
			columnYear.Name = "destfiledate"
			columnYear.FieldName = "destfiledate"
			columnYear.Width = 50
			columnYear.Visible = True
			gvDetail.Columns.Add(columnYear)

			Dim columnBezeichnung As New DevExpress.XtraGrid.Columns.GridColumn()
			columnBezeichnung.Caption = m_translate.GetSafeTranslationValue("Ergebnis")
			columnBezeichnung.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
			columnBezeichnung.Name = "result"
			columnBezeichnung.FieldName = "result"
			columnBezeichnung.Width = 100
			columnBezeichnung.Visible = True
			gvDetail.Columns.Add(columnBezeichnung)


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		grdDetailrec.DataSource = Nothing

	End Sub

	Public Function LoadUpdateresultList() As Boolean
		Dim m_DataAccess As New MainGrid
		Dim employeeNumber As Integer? = Nothing

		Dim listOfEmployees = m_UpdateFileresult

		If listOfEmployees Is Nothing Then
			m_UitilityUI.ShowErrorDialog("Fehler in der Update-Abfrage. Bitte versuchen Sie den Vorgang zu einem späteren Zeitpunkt.")
			Return False
		End If

		Dim responsiblePersonsGridData = (From person In listOfEmployees
		Select New UpdateFileResult With
					 {.sourcefilename = person.sourcefilename,
						.sourcefiledate = person.sourcefiledate,
						.destfilename = person.destfilename,
						.destfiledate = person.destfiledate,
						.result = person.result
					 }).ToList()

		Dim listDataSource As BindingList(Of UpdateFileResult) = New BindingList(Of UpdateFileResult)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdDetailrec.DataSource = listDataSource
		Me.bsiRecCount.Caption = String.Format(m_translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), gvDetail.RowCount)
		Dim valid = listOfEmployees.Where(Function(data) data.result.StartsWith("Wird ")).ToList()
		bbiRunUpdate.Enabled = valid.Count > 0

		Return Not listOfEmployees Is Nothing
	End Function

#End Region



	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvDetail.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then

				Select Case Me.Modul2Open.ToLower
					Case "maes"
						Dim viewData = CType(dataRow, FoundedEmployeeESDetailData)

						Select Case column.Name.ToLower
							'	Case "employeename"
							'		If viewData.manr > 0 Then
							'			Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedMANr = viewData.manr})
							'			_ClsKD.OpenSelectedEmployee(viewData.mdnr, ModulConstants.UserData.UserNr)
							'		End If

							'	Case "customername"
							'		If viewData.kdnr > 0 Then
							'			Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedKDNr = viewData.kdnr})
							'			_ClsKD.OpenSelectedCustomer(viewData.mdnr, ModulConstants.UserData.UserNr)
							'		End If

							'	Case Else
							'		If viewData.esnr > 0 Then
							'			Dim _ClsKD As New ClsOpenModul(New ClsSetting With {.SelectedMDNr = viewData.mdnr, .SelectedESNr = viewData.esnr, .SelectedMANr = viewData.manr, .SelectedKDNr = viewData.kdnr})
							'			_ClsKD.OpenSelectedES(viewData.mdnr, ModulConstants.UserData.UserNr)
							'		End If

						End Select

				End Select

			End If

		End If

	End Sub



	Private Sub CustomDrawEmptyForeground(ByVal sender As Object, ByVal e As CustomDrawEventArgs)
		Dim s As String = m_translate.GetSafeTranslationValue("Ihre Programmversion ist aktuell.")

		Try
			s = m_translate.GetSafeTranslationValue(s)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		Dim font As Font = New Font("Calibri", 8, FontStyle.Regular)
		Dim r As RectangleF = New RectangleF(e.Bounds.Left + 5, e.Bounds.Top + 5, e.Bounds.Width - 5, e.Bounds.Height - 5)
		e.Graphics.DrawString(s, font, Brushes.Black, r)

	End Sub




#Region "Helper methodes"


	Function SelectedFileVersion(ByVal filename As String) As String
		Dim result As String = String.Empty

		Dim locExistsParser As FileIO.TextFieldParser

		locExistsParser = My.Computer.FileSystem.OpenTextFieldParser(filename, ";")

		Do While Not locExistsParser.EndOfData
			Try
				'Die jeweils nächste Zeile lesen und die einzelnen Felder
				'in ein String-Array separieren.
				Dim locFields As String() = locExistsParser.ReadFields
				If locFields.Length > 1 Then

					If locFields(1).ToString.ToUpper.Contains("FileDate=".ToUpper) Then
						result = Mid(locFields(1).ToString, "FileDate=".Length + 1)

						Return result
					End If

				End If

			Catch ex As Exception

			End Try

		Loop

		Return result

	End Function

	''' <summary>
	''' Sorgt dafür, dass im Bedarfsfall Backupversionen einer Datei erstellt werden, 
	''' die durch eine neuere Version der Datei überschrieben werden soll.
	''' </summary>
	''' <param name="DestFile"></param>
	''' <remarks></remarks>
	Private Sub ManageFileBackupHistory(ByVal DestFile As FileInfo)
		Dim locFileToProcess As FileInfo

		Try
			locFileToProcess = New FileInfo(String.Format("{0}.DNCBackup{1}", DestFile.ToString, SPS_SETTING_UPDATE_BACKUPLEVEL))
			If locFileToProcess.Exists Then
				Try
					'Älteste Backupdatei löschen.
					locFileToProcess.Delete()
				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))

				End Try
			End If

			'History nach oben hoch benennen
			For locHistoryCount As Integer = SPS_SETTING_UPDATE_BACKUPLEVEL - 1 To 1 Step -1
				locFileToProcess = New FileInfo(DestFile.ToString & ".DNCBackup" & locHistoryCount.ToString("0"))

				If locFileToProcess.Exists Then
					Try
						Dim locFileInfo As New FileInfo(DestFile.ToString & ".DNCBackup" & (locHistoryCount + 1).ToString("0"))
						My.Computer.FileSystem.RenameFile(locFileToProcess.FullName, locFileInfo.Name)

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}", ex.ToString))
					End Try
				End If
			Next

			'Eigentliche "alte" Datei umbenennen
			Try
				My.Computer.FileSystem.RenameFile(DestFile.FullName, (New FileInfo(DestFile.ToString & ".DNCBackup1").Name))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Sub GetNewerConfigFile(ByVal existsFilename As String)

		Dim existsFileVersion As String = SelectedFileVersion(existsFilename)
		Dim newFileVersion As String
		m_Logger.LogDebug(String.Format("{0}: {1}", existsFilename, existsFileVersion))

		' File-Download
		Try

			Dim url As String = URL_DOWNLOAD_SPS_FILE
			Dim localfilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ProgUpdatesetting.sps")

			If DownloadFile(url, localfilename) Then

				Try
					newFileVersion = SelectedFileVersion(localfilename)
					m_Logger.LogDebug(String.Format("{0}: {1} | {2}: {3}", existsFilename, existsFileVersion, localfilename, newFileVersion))

					If existsFileVersion <> newFileVersion Then
						My.Computer.FileSystem.CopyFile(localfilename, existsFilename, True)
					End If

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))

				End Try

			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

	End Sub

	Private Function DownloadFile(ByVal _url As String, ByVal _filename As String) As Boolean

		Try
			If File.Exists(_filename) Then File.Delete(_filename)
		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))
		End Try

		Try
			Dim _webClient As New System.Net.WebClient
			_webClient.UseDefaultCredentials = True

			_webClient.DownloadFile(_url, _filename)
			Return IO.File.Exists(_filename)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

			Return False
		End Try

	End Function

	Private Function IsIPAdressAllowedForUpdate() As Boolean
		Dim success As Boolean = True
		Dim stationAdress As String = String.Empty
		Dim blacklist As New List(Of String)(New String() {"10.23.223.11", "10.23.223.12", "10.23.223.13", "10.23.223.14"})

		Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
		Dim adapter As NetworkInterface
		For Each adapter In adapters
			Dim properties As IPInterfaceProperties = adapter.GetIPProperties()
			If properties.UnicastAddresses.Count > 0 Then
				For Each unicastadress As UnicastIPAddressInformation In properties.UnicastAddresses
					Dim ip As Net.IPAddress = unicastadress.Address
					If ip.AddressFamily = AddressFamily.InterNetwork Then
						stationAdress &= If(String.IsNullOrWhiteSpace(stationAdress), "", "|") & ip.ToString
					End If
				Next unicastadress
			End If
		Next adapter
		m_Logger.LogDebug(String.Format("{0}", stationAdress))

		For Each itm As String In blacklist
			If stationAdress.Contains(itm) Then
				m_Logger.LogWarning(String.Format("{0} not allowed...", itm))
				success = False
				Exit For
			End If
		Next

		Return success

	End Function

	Private Function IsDriveReady(ByVal sDrive As String, Optional ByVal bCheckWriteAccess As Boolean = False) As Boolean
		Dim bSuccess As Boolean

		Try
			Dim oDrive As New System.IO.DriveInfo(sDrive)
			bSuccess = oDrive.IsReady
			' auch prüfen, ob Schreibzugriff möglich
			If bCheckWriteAccess Then
				oDrive.VolumeLabel = oDrive.VolumeLabel
			End If


		Catch ex As Exception
			Return False

		End Try

		Return bSuccess
	End Function

	' Adds an ACL entry on the specified directory for the specified account.
	Private Sub AddDirectorySecurity(ByVal FileName As String, ByVal Account As String, ByVal Rights As FileSystemRights, ByVal ControlType As AccessControlType)
		' Create a new DirectoryInfoobject.
		Dim dInfo As New DirectoryInfo(FileName)

		' Get a DirectorySecurity object that represents the 
		' current security settings.
		Dim dSecurity As DirectorySecurity = dInfo.GetAccessControl()

		' Add the FileSystemAccessRule to the security settings. 
		dSecurity.AddAccessRule(New FileSystemAccessRule(Account, Rights, ControlType))

		' Set the new access settings.
		dInfo.SetAccessControl(dSecurity)

	End Sub

	Private Function FileInUse(ByVal FN As String) As Boolean
		Try
			Using fs As IO.FileStream = IO.File.Open(FN, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.None)
				Return False
			End Using

		Catch ex As IO.IOException
			Return True
		End Try

	End Function

	Function TranslateUpdatePath(ByVal myString As String) As String
		Dim strResult As String = myString
		Dim _ClsSystem As New SPProgUtility.ClsProgSettingPath '.ClsMain_Net

		Dim strServerSputnik As String = _ClsSystem.GetSrvRootPath()
		Dim strServerSputnikInit As String = _ClsSystem.GetInitPath()
		Dim strServerSputnikUpdate As String = _ClsSystem.GetUpdatePath
		Dim strServerUpdateServices As String = Path.Combine(strServerSputnikUpdate, "Binn\Services\")
		Dim strServerSputnikUpdateNet As String = Path.Combine(strServerSputnikUpdate, "Binn\Net\")
		Dim dirLocal As String = _ClsSystem.GetLocalBinnPath()

		If dirLocal.EndsWith("\") Then
			dirLocal = Mid(dirLocal, 1, dirLocal.Length - 1)
		End If

		Dim strLocalSputnik As String = System.IO.Directory.GetParent(dirLocal).FullName & "\"
		Dim strLocalBinnSputnik As String = Path.Combine(strLocalSputnik, "Binn\")
		Dim strLocalServices As String = Path.Combine(strLocalBinnSputnik, "Services\")

		Try
			If myString.ToUpper.IndexOf("$ServerSputnik$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$ServerSputnik$".ToUpper, strServerSputnik)

			ElseIf myString.ToUpper.IndexOf("$ServerSputnikInit$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$ServerSputnikInit$".ToUpper, strServerSputnikInit)

			ElseIf myString.ToUpper.IndexOf("$LocalSputnik$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$LocalSputnik$".ToUpper, strLocalSputnik)

			ElseIf myString.ToUpper.IndexOf("$LocalSputnikBinn$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$LocalSputnikBinn$".ToUpper, strLocalBinnSputnik)

			ElseIf myString.ToUpper.IndexOf("$LocalSputnikServices$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$LocalSputnikServices$".ToUpper, strLocalServices)

			ElseIf myString.ToUpper.IndexOf("$ServerUpdateServices$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$ServerUpdateServices$".ToUpper, strServerUpdateServices)

			ElseIf myString.ToUpper.IndexOf("$ServerSputnikUpdate$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$ServerSputnikUpdate$".ToUpper, strServerSputnikUpdate)

			ElseIf myString.ToUpper.IndexOf("$ServerSputnikUpdateNet$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$ServerSputnikUpdateNet$".ToUpper, strServerSputnikUpdateNet)

			ElseIf myString.ToUpper.IndexOf("$System32$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$System32$".ToUpper, Environment.GetFolderPath(Environment.SpecialFolder.System) & "\")

			ElseIf myString.ToUpper.IndexOf("$sysWOW64$".ToUpper) > -1 Then
				If Environment.Is64BitOperatingSystem Then
					strResult = myString.ToUpper.Replace("$sysWOW64$".ToUpper, Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) & "\")
				Else
					strResult = String.Empty
				End If

			ElseIf myString.ToUpper.IndexOf("$DE$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$DE$".ToUpper, strLocalSputnik & "DE\")

			ElseIf myString.ToUpper.IndexOf("$FR$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$FR$".ToUpper, strLocalSputnik & "FR\")

			ElseIf myString.ToUpper.IndexOf("$IT$".ToUpper) > -1 Then
				strResult = myString.ToUpper.Replace("$IT$".ToUpper, strLocalSputnik & "IT\")

			ElseIf myString.ToUpper.IndexOf("$MD$".ToUpper) > -1 Then
				Dim strMDPath As New IO.DirectoryInfo(strServerSputnik)
				Dim diTarget As DirectoryInfo = New DirectoryInfo(strServerSputnik)
				Dim aMDList As New List(Of String)

				' Copy each file into it's new directory.
				For Each strMDPath In strMDPath.GetDirectories()
					Console.WriteLine("Current Directory {0}", strMDPath.FullName)
					If strMDPath.Name.ToUpper.Contains("MD".ToUpper) Then
						aMDList.Add(strMDPath.ToString)
					End If

				Next

				strResult = myString.ToUpper.Replace("$System32$".ToUpper, Environment.GetFolderPath(Environment.SpecialFolder.System) & "\")
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

		Return strResult

	End Function


#End Region


#Region "Helper Class"

	' This implementation defines a very simple comparison 
	' between two FileInfo objects. It only compares the name 
	' of the files being compared and their length in bytes. 
	Private Class FileCompare
		Implements System.Collections.Generic.IEqualityComparer(Of System.IO.FileInfo)

		Public Function Equals1(ByVal x As System.IO.FileInfo, ByVal y As System.IO.FileInfo) As Boolean Implements System.Collections.Generic.IEqualityComparer(Of System.IO.FileInfo).Equals

			If x Is y Then Return True
			If x Is Nothing OrElse y Is Nothing Then Return False

			If (x.Name = y.Name) And (x.Length = y.Length) AndAlso (x.LastWriteTime = y.LastWriteTime) Then
				Return True
			Else
				Return False
			End If

		End Function

		' Return a hash that reflects the comparison criteria. According to the  
		' rules for IEqualityComparer(Of T), if Equals is true, then the hash codes must 
		' also be equal. Because equality as defined here is a simple value equality, not 
		' reference identity, it is possible that two or more objects will produce the same 
		' hash code. 
		Public Function GetHashCode1(ByVal fi As System.IO.FileInfo) As Integer Implements System.Collections.Generic.IEqualityComparer(Of System.IO.FileInfo).GetHashCode
			Dim s As String = fi.Name & fi.Length
			Return s.GetHashCode()
		End Function

	End Class


#End Region


	Private Sub bbiOpenUpdatePDF_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiOpenUpdatePDF.ItemClick
		Dim m_path As New ClsProgPath
		Dim url As String = "http://downloads.domain.com/sps_downloads/PDF/anleitungen/Updateprotokoll/update_details.pdf"
		Dim fileName As String = Path.Combine(m_path.GetSpS2DeleteHomeFolder, "UpdateProtokoll.PDF")

		If DownloadFile(url, fileName) Then
			Dim frmPDFViewer As New frmViewPDF(fileName)
			frmPDFViewer.OpenPDFDocument()

			frmPDFViewer.ShowDialog()

		End If

	End Sub


	Private Sub bbiRunUpdate_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiRunUpdate.ItemClick

		Try
			m_DoUpdate = True

			SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, False)
			SplashScreenManager.Default.SetWaitFormCaption(m_translate.GetSafeTranslationValue("Ihre Abfrage wird ausgeführt") & Space(20))
			SplashScreenManager.Default.SetWaitFormDescription(m_translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

			LoadFilesListForUpdate()
			If Not m_UpdateFileresult Is Nothing Then
				LoadUpdateresultList()
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		Finally
			SplashScreenManager.CloseForm(False)

		End Try

	End Sub

End Class


Public Class UpdateFileResult

	Public Property sourcefilename As String
	Public Property destfilename As String
	Public Property result As String
	Public Property sourcefiledate As DateTime?
	Public Property destfiledate As DateTime?

End Class

Public Class CopyListEntry
	Public Property UpdateCommand As String
	Public Property UpdateTime As String
	Public Property UpdateDate As String
	Public Property UpdateVersion As String
	Public Property SourceFolder As DirectoryInfo
	Public Property SearchMask As String
	Public Property DestFolder As DirectoryInfo
	Public Property DestFileDescription As String

End Class

Public Class CopyTaskItem
	Public Property SourceFile As FileInfo
	Public Property SourcePathPart As String
	Public Property DestPathPart As String

End Class
