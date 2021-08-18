
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure

Public Class frmComatic
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI

	''' <summary>
	''' Utility functions.
	''' </summary>
	Private m_Utility As Utility

	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _docname As String
	Private m_Mandant As Mandant
	Private m_Utility_SP As New SPProgUtility.MainUtilities.Utilities



#Region "public properties"
	Public Property strTempSQL As String

#End Region

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_UtilityUI = New UtilityUI
		m_Utility = New Utility
		m_Utility_SP = New SPProgUtility.MainUtilities.Utilities

		' Mandantendaten
		m_Mandant = New Mandant

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		TranslateControls()

		Me.txt_Filename.Text = My.Settings.Filename4Comatic

	End Sub

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Close()
		Me.Dispose()
	End Sub

	Private Sub TranslateControls()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		CmdClose.Text = m_Translate.GetSafeTranslationValue(CmdClose.Text)
		cmdOK.Text = m_Translate.GetSafeTranslationValue(cmdOK.Text)

		lblHeaderFett.Text = m_Translate.GetSafeTranslationValue(lblHeaderFett.Text)
		lblHeaderNormal.Text = m_Translate.GetSafeTranslationValue(lblHeaderNormal.Text)
		LblSetting.Text = m_Translate.GetSafeTranslationValue(LblSetting.Text)
		LblSort.Text = m_Translate.GetSafeTranslationValue(LblSort.Text)

		bsiInfo.Caption = m_Translate.GetSafeTranslationValue(bsiInfo.Caption)

	End Sub

	Function ExportDataToComatic(ByVal strTempSQL As String) As Boolean
		Dim bResult As Boolean = False
		Dim strFileContent As String = String.Empty

		Try
			Dim conn As SqlConnection = New SqlConnection(ClsDataDetail.m_InitialData.MDData.MDDbConn)
			Dim cmd As SqlCommand = New SqlCommand(strTempSQL, conn)

			' Header
			Dim header As String = m_Translate.GetSafeTranslationValue("Buchungsdatum;RE-Nr.;Firmenname;Konto;Gegenkonto;Betrag (SOLL);Betrag (Haben);MwSt")
			'sw.WriteLine(header)

			conn.Open()
			Dim reader As SqlDataReader = cmd.ExecuteReader()
			Dim mwstCode As String

			While reader.Read()

				' MwSt.-Code
				Select Case CDec(reader("MWSTProz"))
					Case CDec(7.6)
						mwstCode = "102"
					Case CDec(8)
						mwstCode = "103"
					Case 0
						mwstCode = "101"

					Case Else
						mwstCode = "100"

				End Select

				' Zeilen
				strFileContent &= (String.Format("{0};{1};{2};{3};{4};{5};{6};{7}{8}",
																	 Format(reader("V_Date"), "d"),
																	 reader("RENR"),
																	 reader("R_Name1"),
																	 reader("FKSoll"),
																	 reader("REFKHaben1"),
																	 reader("Betrag"),
																	 0,
																	 mwstCode,
																	 vbNewLine))

			End While
			'sw.Close()
			'fs.Close()

			Dim bw As BinaryWriter = New BinaryWriter(File.OpenWrite(Me.txt_Filename.Text))
			Dim NewContent() As Byte = Encoding.Default.GetBytes(header & vbNewLine & strFileContent)
			bw.Write(NewContent)
			bw.Close()
			bResult = True


		Catch ex As Exception ' Manager
			m_Logger.LogError(ex.ToString)

		Finally

		End Try

		Return bResult
	End Function

	Private Sub frmSWIFAC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(ClsDataDetail.m_InitialData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formstyle. {1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub txt_Filename_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_Filename.ButtonClick
		Dim fldlgList As New FolderBrowserDialog

		With fldlgList
			'set the RootFolder
			'      .RootFolder = Environment.SpecialFolder.Personal
			' optional Description to provide additional instructions. 
			.Description = m_Translate.GetSafeTranslationValue("Bitte wählen Sie ein Verzeichnis für Export der Datei.")
			.SelectedPath = Me.txt_Filename.Text

			.ShowNewFolderButton = True
			If .ShowDialog() = DialogResult.OK Then
				Me.txt_Filename.Text = _ClsReg.AddDirSep(.SelectedPath) + _docname
			End If

		End With

	End Sub

	Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
		Dim myDirName As String = m_Utility_SP.GetSpSREHomeFolder

		If Me.txt_Filename.Text = String.Empty Then
			Me.txt_Filename.Text = myDirName & "ZE_ExportComatic.csv"

		Else
			If Not Me.txt_Filename.Text.ToUpper.EndsWith(".csv".ToUpper) Then Me.txt_Filename.Text &= ".csv"
			Dim MyFile As FileInfo = New FileInfo(Me.txt_Filename.Text)
			'If Not MyFile.Directory.Exists  .Exists Then Me.txt_Filename.Text = myDirName & "TempFile.taf"

		End If
		Try
			If File.Exists(Me.txt_Filename.Text) Then File.Delete(Me.txt_Filename.Text)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Try
			'      IO.File.WriteAllLines(Me.txt_Filename.Text, GetDataForSWIFAC().ToArray, System.Text.Encoding.Default)
			If ExportDataToComatic(Me.strTempSQL) Then
				MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Die Datei {0} wurde erfolgreich gespeichert."),
												Me.txt_Filename.Text),
												m_Translate.GetSafeTranslationValue("Daten gespeichert"),
												MessageBoxButtons.OK, MessageBoxIcon.Information)

				Dim strDirectory As String = Path.GetDirectoryName(Me.txt_Filename.Text)
				System.Diagnostics.Process.Start(strDirectory)

			Else
				MessageBox.Show(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht erfolgreich exportiert werden."),
												m_Translate.GetSafeTranslationValue("Export für Comatic"),
												MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

			End If

			' Einstellungen speichern...
			My.Settings.Filename4Comatic = Me.txt_Filename.Text
			My.Settings.Save()

		Catch ex As Exception
			MessageBox.Show(ex.Message, m_Translate.GetSafeTranslationValue("Speicherung nicht durchgeführt"),
											MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try


	End Sub
End Class