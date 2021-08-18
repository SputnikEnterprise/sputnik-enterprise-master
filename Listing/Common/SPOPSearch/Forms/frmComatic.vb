
Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports SPProgUtility.SPExceptionsManager.ClsErrorExceptions

Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPOPSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class frmComatic
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private _docname As String

	Private strTempSQL As String = String.Empty

	Private m_utility As New Utilities
	Private m_md As New Mandant


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Close()
		Me.Dispose()
	End Sub

	Public Sub New(ByVal strSQL As String)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()
		InitializeComponent()

		Me.txt_Filename.Text = My.Settings.Filename4Comatic
		Me.chkArtikel.CheckState = If(My.Settings.bComaticWithArtikel, CheckState.Checked, CheckState.Unchecked)
		Me.strTempSQL = strSQL

	End Sub

	Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click
		Dim myDirName As String = m_utility.GetMyDocumentsPathWithBackSlash

		If Me.txt_Filename.Text = String.Empty Then
			Me.txt_Filename.Text = myDirName & "ExportComatic.csv"

		Else
			If Not Me.txt_Filename.Text.ToUpper.EndsWith(".csv".ToUpper) Then Me.txt_Filename.Text &= ".csv"
			Dim MyFile As FileInfo = New FileInfo(Me.txt_Filename.Text)
			'If Not MyFile.Directory.Exists  .Exists Then Me.txt_Filename.Text = myDirName & "TempFile.taf"

		End If

		Try
			'      IO.File.WriteAllLines(Me.txt_Filename.Text, GetDataForSWIFAC().ToArray, System.Text.Encoding.Default)
			Dim bResult As Boolean
			If Me.chkArtikel.CheckState = CheckState.Checked Then
				bResult = ExportDataToComatic_2(Me.strTempSQL)
			Else
				bResult = ExportDataToComatic(Me.strTempSQL)
			End If
			If bResult Then
				MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Die Datei {0} wurde erfolgreich gespeichert."), Me.txt_Filename.Text),
												("Daten gespeichert"),
												MessageBoxButtons.OK, MessageBoxIcon.Information)

			Else
				MessageBox.Show(m_Translate.GetSafeTranslationValue("Die Daten konnten nicht erfolgreich exportiert werden."),
												m_Translate.GetSafeTranslationValue("Export für Comatic"),
												MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

			End If
			Dim strDirectory As String = Path.GetDirectoryName(Me.txt_Filename.Text)
			System.Diagnostics.Process.Start(strDirectory)

			' Einstellungen speichern...
			My.Settings.Filename4Comatic = Me.txt_Filename.Text
			My.Settings.bComaticWithArtikel = If(Me.chkArtikel.CheckState = CheckState.Checked, True, False)

			My.Settings.Save()

		Catch ex As Exception
			MessageBox.Show(ex.Message, m_Translate.GetSafeTranslationValue("Speicherung nicht durchgeführt"),
											MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	Function ExportDataToComatic(ByVal strTempSQL As String) As Boolean
		Dim bResult As Boolean = False
		Dim strFileContent As String = String.Empty

		Try
			Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
			Dim cmd As SqlCommand = New SqlCommand(strTempSQL, conn)

			' Header
			Dim header As String = "Belegnummer; Remote-Id; Datum; Menge; Buchungstext; Konto; Kostenstelle; "
			header += "Währung; Betrag; MWST; Name; Strasse; PLZ; Ort"

			conn.Open()
			Dim reader As SqlDataReader = cmd.ExecuteReader()
			Dim mwstCode As String

			While reader.Read()

				' MwSt.-Code
				Select Case CDec(reader("MWSTProz"))
					Case CDec(7.6)
						mwstCode = "D01"
					Case CDec(8)
						mwstCode = "D02"
					Case 0
						mwstCode = "N01"

					Case Else
						mwstCode = CStr(CDec(reader("MWSTProz")))

				End Select

				' Zeilen
				strFileContent &= (String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}{14}",
																	 reader("RENR"),
																	 reader("KDNR"),
																	 reader("FAK_DAT"),
																	 1,
																	 "",
																	 reader("FKHaben1"),
																	 reader("KST"),
																	 reader("Currency"),
																	 reader("BetragEx"),
																	 mwstCode,
																	 reader("R_Name1"),
																	 reader("R_Strasse"),
																	 reader("R_PLZ"),
																	 reader("R_Ort"),
																	 vbNewLine))

			End While

			Dim bw As BinaryWriter = New BinaryWriter(File.OpenWrite(Me.txt_Filename.Text))
			Dim NewContent() As Byte = Encoding.Default.GetBytes(header & vbNewLine & strFileContent)
			bw.Write(NewContent)
			bw.Close()
			bResult = True


		Catch ex As Exception ' Manager
			MessageBoxShowError("ExportDataToComatic", ex)

		Finally

		End Try

		Return bResult
	End Function

	Function ExportDataToComatic_2(ByVal strTempSQL As String) As Boolean
		Dim bResult As Boolean = False
		Dim strFileContent As String = String.Empty

		Try
			Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
			Dim cmd As SqlCommand = New SqlCommand(strTempSQL, conn)

			' Header
			Dim header As String = "Adress-Schlüssel;Anrede;Vorname;Name;Adresse1;Adresse2;Adresse3;PLZ;Ort;Land;E-Mail;"
			header &= "Telefon;Datum;Belegnummer;Währung;Referenz;Menge;Bezeichnung;Beschreibung;Konto;Kostenstelle;Betrag/Position;"
			header &= "Rabatt in Prozent;MWSt"

			conn.Open()
			Dim reader As SqlDataReader = cmd.ExecuteReader()
			Dim mwstCode As String

			While reader.Read()

				' MwSt.-Code
				Select Case CDec(reader("MWSTProz"))
					Case CDec(7.6)
						mwstCode = "D01"
					Case CDec(8)
						mwstCode = "D02"
					Case 0
						mwstCode = "N01"

					Case Else
						mwstCode = CStr(CDec(reader("MWSTProz")))

				End Select

				' Zeilen
				strFileContent &= "{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};"
				strFileContent &= "{20};{21};{22};{23};{24}"
				strFileContent = (String.Format(strFileContent,
																				reader("KDNR"),
																	 String.Empty,
																	 String.Empty,
																	 reader("R_Name1"),
																	 reader("R_Strasse"),
																	 String.Empty,
																	 String.Empty,
																	 reader("R_PLZ"),
																	 reader("R_Ort"),
																	 reader("R_Land"),
																	 String.Empty,
																	 String.Empty,
																	 Format(reader("Fak_Dat"), "d"),
																	 reader("RENr"),
																	 reader("Currency"),
																	 String.Empty,
																	 1,
																	 String.Empty,
																	 String.Empty,
																	 reader("FKHaben1"),
																	 reader("KST"),
																	 reader("BetragEx"),
																	 String.Empty,
																	 mwstCode,
																	 vbNewLine))

				'reader("RENR"), _
				'reader("KDNR"), _
				'reader("FAK_DAT"), _
				'1, _
				'"", _
				'reader("FKHaben1"), _
				'reader("KST"), _
				'reader("Currency"), _
				'reader("BetragEx"), _
				'mwstCode, _
				'reader("R_Name1"), _
				'reader("R_Strasse"), _
				'reader("R_PLZ"), _
				'reader("R_Ort"), _
				'vbNewLine))

			End While

			Dim bw As BinaryWriter = New BinaryWriter(File.OpenWrite(Me.txt_Filename.Text))
			Dim NewContent() As Byte = Encoding.Default.GetBytes(header & vbNewLine & strFileContent)
			bw.Write(NewContent)
			bw.Close()
			bResult = True


		Catch ex As Exception ' Manager
			MessageBoxShowError("ExportDataToComatic", ex)

		Finally

		End Try

		Return bResult
	End Function

	Private Sub StartTranslation()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)
		Me.cmdOK.Text = m_Translate.GetSafeTranslationValue(Me.cmdOK.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)

		Me.lblHeader1.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblHeader2.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader2.Text)
		Me.lblDatei.Text = m_Translate.GetSafeTranslationValue(Me.lblDatei.Text)

		Me.chkArtikel.Text = m_Translate.GetSafeTranslationValue(Me.chkArtikel.Text)

	End Sub

	Private Sub frmSWIFAC_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		StartTranslation()

		Try
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitialData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formstyle. {1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub txt_Filename_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txt_Filename.ButtonClick

		If e.Button.Index = 0 Then
			Dim fldlgList As New FolderBrowserDialog

			With fldlgList
				.Description = m_Translate.GetSafeTranslationValue("Bitte wählen Sie ein Verzeichnis für Export der Datei.")
				.SelectedPath = Me.txt_Filename.Text

				.ShowNewFolderButton = True
				If .ShowDialog() = DialogResult.OK Then
					Me.txt_Filename.Text = m_utility.AddDirSep(.SelectedPath) + _docname
				End If

			End With

		End If

	End Sub


End Class