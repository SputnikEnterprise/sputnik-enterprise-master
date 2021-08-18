
Imports System.IO.File
Imports SPProgUtility.ClsProgSettingPath

Imports SP.Infrastructure.Logging
Imports System.Threading
Imports System.IO
Imports SPS.Listing.Print.Utility.ClsMainSetting
Imports SPS.Listing.Print.Utility.MainUtilities.Utilities
Imports SP.Infrastructure

Namespace KDStammblatt

	Public Class ClsPrintKDStammblatt

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Private _ClsSetting As New ClsLLKDSearchPrintSetting
		Private m_PDFUtility As PDFUtilities.Utilities


		Private Property ExportFinalFilename As String

		Public Property PrintData As ClsLLKDSearchPrintSetting



		Public Function PrintKDStammBlatt() As String
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty
			Dim liKDNr As New List(Of Integer)

			liKDNr = _ClsSetting.liKDNr2Print
			m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))
			For i As Integer = 0 To liKDNr.Count - 1

				_ClsSetting.KDNr2Print = liKDNr(i)
				Dim _clsKDPrint As New ClsLLKDStammblattPrint(_ClsSetting)
				If _ClsSetting.ShowAsDesign Then
					_clsKDPrint.ShowInDesign()
					_clsKDPrint.Dispose()
					Return String.Empty

				Else
					strResult = _clsKDPrint.ShowInPrint(i = 0)

				End If

				_clsKDPrint.Dispose()
			Next

			Return strResult
		End Function

		Public Function ExportKDStammBlatt() As String
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strResult As String = String.Empty
			Dim liKDNr As New List(Of Integer)

			liKDNr = _ClsSetting.liKDNr2Print

			m_Logger.LogInfo(String.Format("{0} {1}wurde gestartet", strMethodeName, vbTab))
			For i As Integer = 0 To liKDNr.Count - 1
				_ClsSetting.KDNr2Print = liKDNr(i)

				Dim _clsKDPrint As New ClsLLKDStammblattPrint(_ClsSetting)
				strResult = _clsKDPrint.ExportLLDoc()
				_clsKDPrint.Dispose()

				If strResult.ToLower.Contains("error") Then Exit For
			Next
			If strResult.ToLower.Contains("error") Then Return strResult

			Try
				If _ClsSetting.ListOfExportedFilesKDStamm.Count > 0 Then
					Dim strExportPfad As String = m_InitialData.UserData.spTempCustomerPath
					If Not Directory.Exists(strExportPfad) Then Directory.CreateDirectory(m_InitialData.UserData.spTempCustomerPath)

					ExportFinalFilename = Path.Combine(strExportPfad, String.Format("Kundenstamm_{0}.pdf", m_InitialData.MDData.MDName))
					If File.Exists(Path.Combine(strExportPfad, Me.ExportFinalFilename)) Then
						Try
							File.Delete(Path.Combine(strExportPfad, Me.ExportFinalFilename))
						Catch ex As Exception
							ExportFinalFilename = Path.Combine(strExportPfad, String.Format("Kundenstamm_{0}_{1}.pdf", m_InitialData.MDData.MDName, Environment.TickCount))
						End Try
					End If
					Dim strFinalFilename As String = Path.Combine(strExportPfad, Me.ExportFinalFilename)


					strResult = "Success..."
					If _ClsSetting.ListOfExportedFilesKDStamm.Count = 1 Then
						Try
							File.Copy(_ClsSetting.ListOfExportedFilesKDStamm(0), strFinalFilename, True)
							strResult = strFinalFilename

						Catch ex As Exception
							Dim strMsg As String = String.Format("{0}.Datei kopieren: {1}", strMethodeName, ex.ToString)
							m_Logger.LogError(strMsg)
							strResult = "Error: " & strMsg

						End Try

					Else
						Dim mergePDF = m_PDFUtility.MergePdfFiles(_ClsSetting.ListOfExportedFilesKDStamm.ToArray, strFinalFilename)
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

		Public Sub New(ByVal _Setting As ClsLLKDSearchPrintSetting)

			_ClsSetting = _Setting
			PrintData = _Setting

			Dim m_init = CreateInitialData(_ClsSetting.SelectedMDNr, _ClsSetting.LogedUSNr)
			m_InitialData = m_init
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)
			m_PDFUtility = New PDFUtilities.Utilities

			ClsMainSetting.MDData = m_init.MDData
			ClsMainSetting.UserData = m_init.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData
			'ClsMainSetting.MDData = ClsMainSetting.SelectedMDData(_Setting.SelectedMDNr)
			'ClsMainSetting.UserData = ClsMainSetting.LogededUSData(_Setting.SelectedMDNr, _Setting.LogedUSNr)

			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = MainUtilities.GetUSSign(Me._ClsSetting.DbConnString2Open)

		End Sub

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitialData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitialData.TranslationData, m_InitialData.ProsonalizedData)
			m_PDFUtility = New PDFUtilities.Utilities

			ClsMainSetting.MDData = m_InitialData.MDData
			ClsMainSetting.UserData = m_InitialData.UserData
			ClsMainSetting.TranslationData = m_InitialData.TranslationData
			ClsMainSetting.PerosonalizedData = m_InitialData.ProsonalizedData

			ClsMainSetting.ProgSettingData = New ClsSetting
			Me._ClsSetting.USSignFileName = GetUSSign(m_InitialData.MDData.MDDbConn)

		End Sub

		Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

			Dim m_md As New SPProgUtility.Mandanten.Mandant
			Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
			Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
			Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

			Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
			Dim translate = clsTransalation.GetTranslationInObject

			Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

		End Function

	End Class



End Namespace



