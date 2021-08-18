
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath

Imports SP.Infrastructure.Logging
Imports System.Threading
Imports System.IO
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities
Imports System.Drawing.Printing
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Namespace MAStammblatt

	Public Class ClsPrintMAStammblatt

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Private _ClsSetting As New ClsLLMASearchPrintSetting
		Private Property ExportFinalFilename As String
		Private m_UtilityUI As UtilityUI
		Private m_PDFUtility As PDFUtilities.Utilities

		Public Property PrintData As ClsLLMASearchPrintSetting

#Region "Constructor"

		Public Sub New(ByVal _Setting As ClsLLMASearchPrintSetting)

			_ClsSetting = _Setting
			PrintData = _Setting
			m_UtilityUI = New UtilityUI

			Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
			m_InitialData = m_init
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)
			m_PDFUtility = New PDFUtilities.Utilities

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			If _ClsSetting.DbConnString2Open = String.Empty Then _ClsSetting.DbConnString2Open = m_InitialData.MDData.MDDbConn
			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = GetUSSign(m_InitialData.MDData.MDDbConn)

		End Sub

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitialData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)
			m_UtilityUI = New UtilityUI

			m_PDFUtility = New PDFUtilities.Utilities

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = GetUSSign(m_InitialData.MDData.MDDbConn)

		End Sub

#End Region


#Region "public methodes"

		Public Function PrintMAStammBlatt() As String
			Dim strResult As String = String.Empty
			Dim liMANr As New List(Of Integer)

			Dim settings = New PrinterSettings()
			If settings.PrinterName Is Nothing OrElse String.IsNullOrWhiteSpace(settings.PrinterName) Then
				m_Logger.LogError("no default printer is defined.")
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben keinen Standard-Drucker definiert."))

				Return String.Empty
			Else
				m_Logger.LogInfo(String.Format("default printer: {0}", settings.PrinterName))
			End If

			PrintData.liMANr2Print = PrintData.liMANr2Print.Distinct.ToList()
			liMANr = PrintData.liMANr2Print

			For i As Integer = 0 To liMANr.Count - 1

				PrintData.MANr2Print = liMANr(i)
				Dim _clsMAPrint As New ClsLLMAStammblattPrint(PrintData)

				If PrintData.ShowAsDesign Then
					_clsMAPrint.ShowInDesign()
					_clsMAPrint.Dispose()
					Return String.Empty

				Else
					strResult = _clsMAPrint.ShowInPrint(i = 0)

				End If
				_clsMAPrint.Dispose()
			Next

			Return strResult
		End Function

		Public Function ExportMAStammBlatt() As String
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty
			Dim liMANr As New List(Of Integer)

			_ClsSetting.liMANr2Print = _ClsSetting.liMANr2Print.Distinct.ToList()
			liMANr = _ClsSetting.liMANr2Print

			For i As Integer = 0 To liMANr.Count - 1
				_ClsSetting.MANr2Print = liMANr(i)

				Dim _clsMAPrint As New ClsLLMAStammblattPrint(_ClsSetting)


				strResult = _clsMAPrint.ExportLLDoc()
				_clsMAPrint.Dispose()

				If strResult.ToLower.Contains("error") Then Exit For
			Next
			If strResult.ToLower.Contains("error") Then Return strResult

			Try
				If _ClsSetting.ListOfExportedFilesMAStamm.Count > 0 Then
					Dim strExportPfad As String = m_InitialData.UserData.spTempEmployeePath
					If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(m_InitialData.UserData.spTempEmployeePath)

					ExportFinalFilename = Path.Combine(strExportPfad, String.Format("Kandidatenstamm_{0}.pdf", m_InitialData.MDData.MDName))

					If File.Exists(Path.Combine(strExportPfad, Me.ExportFinalFilename)) Then
						Try
							File.Delete(Path.Combine(strExportPfad, Me.ExportFinalFilename))
						Catch ex As Exception
							ExportFinalFilename = Path.Combine(strExportPfad, String.Format("Kandidatenstamm_{0}_{1}.pdf", m_InitialData.MDData.MDName, Environment.TickCount))
						End Try
					End If
					Dim strFinalFilename As String = Path.Combine(strExportPfad, Me.ExportFinalFilename)


					strResult = "Success..."
					If _ClsSetting.ListOfExportedFilesMAStamm.Count = 1 Then
						Try
							File.Copy(_ClsSetting.ListOfExportedFilesMAStamm(0), strFinalFilename, True)
							strResult = strFinalFilename

						Catch ex As Exception
							Dim strMsg As String = String.Format("{0}.Datei kopieren: {1}", strMethodeName, ex.ToString)
							m_Logger.LogError(strMsg)
							strResult = "Error: " & strMsg

						End Try

					Else
						Dim mergePDF = m_PDFUtility.MergePdfFiles(_ClsSetting.ListOfExportedFilesMAStamm.ToArray, strFinalFilename)

						'Dim obj As New SP.PDFO2S.ClsPDF4Net
						'strResult = obj.Merg2PDFFiles(strFinalFilename, _ClsSetting.ListOfExportedFilesMAStamm.ToArray)
						If mergePDF Then
							strResult = strFinalFilename
						Else
							strResult = String.Empty
						End If

					End If
				Else
					strResult = "Error: keine Dateien wurden zum Export erstellt..."

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.MergePDF: {1}", strMethodeName, ex.ToString))
				strResult = ex.ToString

			End Try

			Return strResult
		End Function


#End Region


#Region "private methodes"

		Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

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


