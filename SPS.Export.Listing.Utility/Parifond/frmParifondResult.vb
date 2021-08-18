
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.IO.Compression

Imports DevComponents.DotNetBar
Imports DevComponents.DotNetBar.Metro.ColorTables
Imports SPProgUtility.ColorUtility.ClsColorUtility
Imports System.IO
Imports O2S.Components.PDFView4NET
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Public Class frmParifondResult


#Region "Private fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private m_xml As New ClsXML
	Private m_md As Mandant
	Private m_path As ClsProgPath
	Private m_PDFUtility As PDFUtilities.Utilities

#End Region


#Region "private properties"

	Public Property PControlSetting As ClsParifondSetting
	'Public Property MetroForeColor As System.Drawing.Color
	'Public Property MetroBorderColor As System.Drawing.Color

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_md = New Mandant
		m_path = New ClsProgPath
		m_PDFUtility = New PDFUtilities.Utilities

		InitializeComponent()

		TranslateControls()

	End Sub


#End Region


	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

	End Sub

	Private Sub frmParifondResult_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim pdfFiles = New List(Of String)

		For i As Integer = 0 To Me.PControlSetting.liCreatedError.Count - 1
			Me.lstResult.Items.Add(Me.PControlSetting.liCreatedError(i))
		Next
		If lstResult.ItemCount > 0 Then Me.lstResult.Items.Add(String.Empty)

		For i As Integer = 0 To Me.PControlSetting.liCreatedFinalFile.Count - 1
			Me.lstResult.Items.Add(Me.PControlSetting.liCreatedFinalFile(i))
			If PControlSetting.liCreatedFinalFile(i).ToLower.EndsWith(".pdf") Then
				pdfFiles.Add(PControlSetting.liCreatedFinalFile(i))
			End If
		Next
		If pdfFiles.Count = 0 Then
			lstResult.Items.Add(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden!"))

			Return
		End If
		PControlSetting.liCreatedFinalFile = pdfFiles

		Try
			If Me.PControlSetting.liCreatedFinalFile.Count > 0 Then
				Me.lstResult.Items.Add(String.Empty)
				'Dim strFinalFilename As String = Path.Combine(m_path.GetSpSTempFolder, String.Format("{0}_{1}.pdf", m_InitializationData.MDData.MDName, PControlSetting.SelectedPeriodeTime))

				Dim strExportPfad As String = m_InitializationData.UserData.SPTempPath
				If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(m_InitializationData.UserData.spTempEmplymentPath)

				Dim fileName = String.Format("{0}_{1}.pdf", m_InitializationData.MDData.MDName, PControlSetting.SelectedPeriodeTime)

				If File.Exists(Path.Combine(strExportPfad, fileName)) Then
					Try
						File.Delete(Path.Combine(strExportPfad, fileName))
					Catch ex As Exception
						fileName = String.Format("{0}_{1}_{2}.pdf", m_InitializationData.MDData.MDName, PControlSetting.SelectedPeriodeTime, Environment.TickCount)
					End Try
				End If
				Dim strFinalFilename As String = Path.Combine(strExportPfad, fileName)
				m_Logger.LogDebug(String.Format("gav controllist FinalFilename: {0}", strFinalFilename))


				If Me.PControlSetting.liCreatedFinalFile.Count = 1 Then
					m_Logger.LogDebug(String.Format("Me._PControlSetting.liCreatedFinalFile(0): {0} -> strFinalFilename: {1}", Me.PControlSetting.liCreatedFinalFile(0), strFinalFilename))

					Try
						File.Copy(Me.PControlSetting.liCreatedFinalFile(0), strFinalFilename, True)

					Catch ex As Exception
						strFinalFilename = Path.Combine(m_InitializationData.UserData.SPTempPath, String.Format("{0} - {1}.ex", Path.GetRandomFileName, System.Guid.NewGuid.ToString))
						strFinalFilename = Path.ChangeExtension(strFinalFilename, ".pdf")

						File.Copy(Me.PControlSetting.liCreatedFinalFile(0), strFinalFilename, True)

					End Try

				Else

					If PControlSetting.MergePDFFiles Then
						Dim mergePDF As Boolean = m_PDFUtility.MergePdfFiles(PControlSetting.liCreatedFinalFile.ToArray, strFinalFilename)

						If mergePDF Then
							Me.lstResult.Items.Add(m_xml.GetSafeTranslationValue("Alle Dateien wurden in einer PDF-Datei zusammengestellt:"))
							Me.lstResult.Items.Add(strFinalFilename)

						Else
							Me.lstResult.Items.Add(m_xml.GetSafeTranslationValue("Ihre Datien konten nicht erfolgreich zusammengestellt werden!"))
							strFinalFilename = String.Empty
						End If

					Else
						Dim zipPath As String = Path.Combine(strExportPfad, "ZIP")
						Dim zipArchiveFile As String = Path.ChangeExtension(fileName, "ZIP")

						If File.Exists(Path.Combine(strExportPfad, zipArchiveFile)) Then
							Try
								File.Delete(Path.Combine(strExportPfad, zipArchiveFile))
							Catch ex As Exception
								zipArchiveFile = String.Format("{0}_{1}_{2}.zip", m_InitializationData.MDData.MDName, PControlSetting.SelectedPeriodeTime, Environment.TickCount)
							End Try
						End If

						If Directory.Exists(zipPath) Then
							Dim dir = New DirectoryInfo(zipPath)
							For Each file In dir.EnumerateFiles("*.*")
								Try
									file.Delete()
								Catch ex As Exception
									zipPath = Path.Combine(strExportPfad, String.Format("ZIP_{0}", Environment.TickCount))
									Directory.CreateDirectory(zipPath)
								End Try
							Next
						Else
							Directory.CreateDirectory(zipPath)
						End If

						For Each itm In PControlSetting.liCreatedFinalFile
							Try
								File.Copy(itm, Path.Combine(zipPath, Path.GetFileName(itm)))

								'System.IO.File.Open(itm, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.ReadWrite)
								'FileClose(1)

								'File.Delete(itm)
							Catch ex As Exception

							End Try
						Next

						ZipFile.CreateFromDirectory(zipPath, Path.Combine(strExportPfad, zipArchiveFile), CompressionLevel.Optimal, False)

						lstResult.Items.Add(m_xml.GetSafeTranslationValue("Alle Dateien wurden als einzeln und ZIP-Datei in einem Verzeichnis erstellt:"))
						lstResult.Items.Add(Path.Combine(strExportPfad, zipArchiveFile))

					End If

					If File.Exists(strFinalFilename) Then
					End If
				End If
				If File.Exists(strFinalFilename) Then
					Dim strDirectory As String = Path.GetDirectoryName(strFinalFilename)
					Process.Start("explorer.exe", "/select," & strFinalFilename)

					Process.Start(strFinalFilename)
				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Daten zusammenstellen. {1}", strMethodeName, ex.Message))
			Me.lstResult.Items.Add(String.Format(m_xml.GetSafeTranslationValue("{0}.Daten zusammenstellen. {1}"), strMethodeName, ex.Message))
		End Try

	End Sub



	Private Sub lstResult_DoubleClick(sender As Object, e As System.EventArgs) Handles lstResult.DoubleClick
		Dim strFilename As String = Me.lstResult.SelectedItem

		If File.Exists(strFilename) Then
			Dim strDirectory As String = Path.GetDirectoryName(strFilename)
			System.Diagnostics.Process.Start(strDirectory)

			Process.Start(strFilename)
		End If

	End Sub


End Class