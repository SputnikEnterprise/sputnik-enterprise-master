
Imports System.IO
Imports DevExpress.Skins
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors

Imports System.Data.SqlClient

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure
Imports SP.Infrastructure.UI

Public Class frmNewMDYear
	Inherits XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_progpaath As New ClsProgPath
	Private m_common As New CommonSetting
	Private m_utilitySP As New SPProgUtility.MainUtilities.Utilities
	Private m_Utility As Utility
	Private m_UtilityUI As UtilityUI

	Private m_mandant As New Mandant


	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_mandant = New Mandant

		m_utility = New Utility
		m_UtilityUI = New UtilityUI
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)


		Me.KeyPreview = True
		Dim strStyleName As String = m_mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		InitializeComponent()

		TranslateControls()


	End Sub

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)
		Me.Label2.Text = m_Translate.GetSafeTranslationValue(Me.Label2.Text)
		Me.Label1.Text = m_Translate.GetSafeTranslationValue(Me.Label1.Text)

		Me.LabelControl3.Text = m_Translate.GetSafeTranslationValue(Me.LabelControl3.Text)
		Me.LabelControl2.Text = m_Translate.GetSafeTranslationValue(Me.LabelControl2.Text)
		Me.LabelControl1.Text = m_Translate.GetSafeTranslationValue(Me.LabelControl1.Text)
		Me.CmdOpen.Text = m_Translate.GetSafeTranslationValue(Me.CmdOpen.Text)

	End Sub

	Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

		ListLOJahr(Me.cboFromYear)
		If Me.cboFromYear.Text = String.Empty Then
			Me.cboFromYear.Text = Now.Year
		End If
		Me.cboToYear.Properties.Items.Add(Me.cboFromYear.Text + 1)
		Me.cboToYear.SelectedIndex = 0

	End Sub

	Sub ListLOJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strEntry As String
		Dim strSqlQuery As String = "Select Convert(Int, MD.Jahr) Jahr From Mandanten MD "
		strSqlQuery += "Group By Convert(Int, MD.Jahr) Order By Convert(Int, MD.Jahr) DESC"
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim rFOPrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rFOPrec.Read
				strEntry = rFOPrec("Jahr").ToString
				cbo.Properties.Items.Add(rFOPrec("Jahr").ToString)

			End While
			'cbo.SelectedIndex = 0

		Catch ex As Exception ' Manager
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Private Sub CmdClose_Click(sender As System.Object, e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub CmdOpen_Click(sender As System.Object, e As System.EventArgs) Handles CmdOpen.Click
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty

		Try
			Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Import wurde gestartet..."), Now))
			Dim iCheckValue As Short = 0
			Me.ListBoxControl1.Items.Clear()

			Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: BVG-Daten (Männer) werden kontrolliert..."), Now))
			Try
				m_Logger.LogInfo(String.Format("TabBVGMann.{0}.{1}", strMethodeName, "gestartet..."))

				iCheckValue = CheckMDTable("TabBVGMann", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text))
				If iCheckValue = 0 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: BVG-Daten (Männer) werden kopiert..."), Now))
					If Not CopyBVGData("TabBVGMann", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text)) Then
						Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: BVG-Daten: Fehler bitte kontrollieren Sie die LOG-Datei!!!"), Now))
					End If

				ElseIf iCheckValue = 1 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** BVG-Daten (Männer) abgebrochen..."), Now))

				End If

			Catch ex As Exception
				Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** BVG-Daten (Männer): {1}..."), Now, ex.Message))
				m_Logger.LogError(String.Format("TabBVGMann.{0}.{1}", strMethodeName, ex.Message))

			End Try


			Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: BVG-Daten (Frauen) werden kontrolliert..."), Now))
			Try
				m_Logger.LogInfo(String.Format("TabBVGFrauen.{0}.{1}", strMethodeName, "gestartet..."))

				iCheckValue = CheckMDTable("TabBVGFrauen", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text))
				If iCheckValue = 0 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: BVG-Daten (Frauen) werden kopiert..."), Now))
					If Not CopyBVGData("TabBVGFrauen", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text)) Then
						Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: BVG-Daten: Fehler bitte kontrollieren Sie die LOG-Datei!!!"), Now))
					End If

				ElseIf iCheckValue = 1 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** BVG-Daten (Frauen) abgebrochen..."), Now))

				End If

			Catch ex As Exception
				Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** BVG-Daten (Frauen): {1}..."), Now, ex.Message))
				m_Logger.LogError(String.Format("TabBVGFrauen.{0}.{1}", strMethodeName, ex.Message))

			End Try

			Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Lohnartenstamm werden kontrolliert..."), Now))
			Try
				m_Logger.LogInfo(String.Format("LA.{0}.{1}", strMethodeName, "gestartet..."))
				iCheckValue = CheckMDTable("LA", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text))
				If iCheckValue = 0 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Lohnartenstamm werden kopiert..."), Now))
					If Not CopyLAData(CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text)) Then
						Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Lohnartenstamm: Fehler bitte kontrollieren Sie die LOG-Datei!!!"), Now))
					End If

				ElseIf iCheckValue = 1 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** Lohnartenstamm abgebrochen..."), Now))

				End If

			Catch ex As Exception
				Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** Lohnartenstamm: {1}..."), Now, ex.Message))
				m_Logger.LogError(String.Format("LA.{0}.{1}", strMethodeName, ex.Message))

			End Try

			Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: KTG-Daten für GAV werden kontrolliert..."), Now))
			Try
				m_Logger.LogInfo(String.Format("MD_KK_LMV.{0}.{1}", strMethodeName, "gestartet..."))
				iCheckValue = CheckMDTable("MD_KK_LMV", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text))
				If iCheckValue = 0 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: KTG-Daten für GAV werden kopiert..."), Now))
					If Not CopyLMVKTGData("MD_KK_LMV", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text)) Then
						Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: KTG-Daten: Fehler bitte kontrollieren Sie die LOG-Datei!!!"), Now))
					End If

				ElseIf iCheckValue = 1 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** MD_KK_LMV abgebrochen..."), Now))

				End If

			Catch ex As Exception
				Me.ListBoxControl1.Items.Add(String.Format("{0}: *** MD_KK_LMV: {1}...", Now, ex.Message))
				m_Logger.LogError(String.Format("MD_KK_LMV.{0}.{1}", strMethodeName, ex.Message))

			End Try

			Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Spesen-Regelung für GAV's werden kontrolliert..."), Now))
			Try
				m_Logger.LogInfo(String.Format("MD_TSP_LMV.{0}.{1}", strMethodeName, "gestartet..."))
				iCheckValue = CheckMDTable("MD_TSP_LMV", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text))
				If iCheckValue = 0 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Spesen-Regelung für GAV werden kopiert..."), Now))
					If Not CopyTSPLMVTSPData("MD_TSP_LMV", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text)) Then
						Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Spesen-Regelung: Fehler bitte kontrollieren Sie die LOG-Datei!!!"), Now))
					End If

				ElseIf iCheckValue = 1 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** MD_TSP_LMV abgebrochen..."), Now))

				End If

			Catch ex As Exception
				Me.ListBoxControl1.Items.Add(String.Format("{0}: *** MD_TSP_LMV: {1}...", Now, ex.Message))
				m_Logger.LogError(String.Format("MD_TSP_LMV.{0}.{1}", strMethodeName, ex.Message))

			End Try

			Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Kinder- / Ausbildungszulagen werden kontrolliert..."), Now))
			Try
				m_Logger.LogInfo(String.Format("MD_KiAu.{0}.{1}", strMethodeName, "gestartet..."))
				iCheckValue = CheckKiAuTable("MD_KiAu", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text))
				If iCheckValue = 0 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Kinder- / Ausbildungszulagen werden kopiert..."), Now))
					If Not CopyKiAuData("MD_KiAu", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text)) Then
						Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Kinder- / Ausbildungszulagen: Fehler bitte kontrollieren Sie die LOG-Datei!!!"), Now))
					End If

				ElseIf iCheckValue = 1 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** Kinder- / Ausbildungszulagen abgebrochen..."), Now))

				ElseIf iCheckValue = 2 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** Kinder- / Ausbildungszulagen übersprungen..."), Now))

				End If

			Catch ex As Exception
				Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** MD_KiAu: {1}..."), Now, ex.Message))
				m_Logger.LogError(String.Format("MD_KiAu.{0}.{1}", strMethodeName, ex.Message))

			End Try

			Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Mandantendaten werden kontrolliert..."), Now))
			Try
				m_Logger.LogInfo(String.Format("Mandanten.{0}.{1}", strMethodeName, "gestartet..."))

				iCheckValue = CheckMDTable("Mandanten", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text))
				If iCheckValue = 0 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Mandantendaten werden kopiert..."), Now))
					If Not CopyMDData("Mandanten", CInt(Me.cboFromYear.Text), CInt(Me.cboToYear.Text)) Then
						Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Mandantendaten: Fehler bitte kontrollieren Sie die LOG-Datei!!!"), Now))
					End If

				ElseIf iCheckValue = 1 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** Mandantendaten kopieren..."), Now))

				ElseIf iCheckValue = 2 Then
					Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** Mandantendaten übersprungen..."), Now))

				End If

			Catch ex As Exception
				Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** Mandanten: {1}..."), Now, ex.Message))
				m_Logger.LogError(String.Format("Mandanten.{0}.{1}", strMethodeName, ex.Message))

			End Try

			Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Verzeichnisstruktur wird angelegt..."), Now))
			Try
				m_Logger.LogInfo(String.Format("Verzeichnisstruktur.{0}.{1}", strMethodeName, "gestartet..."))

				Dim strOldMDYearPath As String = Path.Combine(m_InitializationData.MDData.MDMainPath, Me.cboFromYear.Text)
				Dim strNewMDYearPath As String = Path.Combine(m_InitializationData.MDData.MDMainPath, Me.cboToYear.Text)

				If Not Directory.Exists(strNewMDYearPath) Then Directory.CreateDirectory(strNewMDYearPath)
				CopyDirectory(String.Format("{0}", strOldMDYearPath), String.Format("{0}", strNewMDYearPath))

			Catch ex As Exception
				Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: *** Verzeichnisstruktur: {1}..."), Now, ex.Message))
				m_Logger.LogError(String.Format("Verzeichnisstruktur.{0}.{1}", strMethodeName, ex.Message))

			End Try

			Me.ListBoxControl1.Items.Add(String.Format(m_Translate.GetSafeTranslationValue("{0}: Der Vorgang wurde erfolgreich abgeschlossen..."), Now))


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			Me.ListBoxControl1.Items.Add(String.Format("{0}: {1}...", Now, ex.Message))

		End Try

	End Sub

	Private Sub CopyDirectory(sourcePath As String, destPath As String)
		If Not Directory.Exists(destPath) Then
			Directory.CreateDirectory(destPath)
		End If

		For Each file__1 As String In Directory.GetFiles(sourcePath)
			Dim dest As String = Path.Combine(destPath, Path.GetFileName(file__1))
			File.Copy(file__1, dest, True)
		Next

		For Each folder As String In Directory.GetDirectories(sourcePath)
			Dim dest As String = Path.Combine(destPath, Path.GetFileName(folder))
			CopyDirectory(folder, dest)
		Next
	End Sub


#Region "BVG-Männer und Frauen..."

	Public Function CopyBVGData(ByVal strTableName As String, ByVal iVonJahr As Integer, ByVal iBisJahr As Integer) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sql As String = String.Empty

		Try

			sql = "[Copy BVGData For NewMDYear]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@tblName", strTableName))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrVon", iVonJahr))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrBis", iBisJahr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitializationData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@USNr", m_InitializationData.UserData.UserNr))

			Return m_utilitySP.ExecuteNonQuery(m_InitializationData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{2}.{0}.{1}", strMethodeName, ex.Message, strTableName))

		End Try

	End Function

#End Region


#Region "Lohnartenstamm..."

	Public Function CopyLAData(ByVal iVonJahr As Integer, ByVal iBisJahr As Integer) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty

		Try
			strSqlQuery = "[Copy LaData For NewMDYear]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@jahrVon", iVonJahr))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrBis", iBisJahr))

			Return m_utilitySP.ExecuteNonQuery(m_InitializationData.MDData.MDDbConn, strSqlQuery, listOfParams, CommandType.StoredProcedure)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

		End Try

	End Function

#End Region


#Region "KTG für LMV..."

	Public Function CopyLMVKTGData(ByVal strTableName As String, ByVal iVonJahr As Integer, ByVal iBisJahr As Integer) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty

		Try

			'Conn.Open()

			strSqlQuery = "[Copy LMVKTGData For NewMDYear]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@tblName", strTableName))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrVon", iVonJahr))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrBis", iBisJahr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitializationData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@USNr", m_InitializationData.UserData.UserNr))

			Return m_utilitySP.ExecuteNonQuery(m_InitializationData.MDData.MDDbConn, strSqlQuery, listOfParams, CommandType.StoredProcedure)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{2}.{0}.{1}", strMethodeName, ex.Message, strTableName))

		End Try

	End Function

	Public Function CopyTSPLMVTSPData(ByVal strTableName As String, ByVal iVonJahr As Integer, ByVal iBisJahr As Integer) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty

		Try
			strSqlQuery = "[Copy TSPLMVData For NewMDYear]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@tblName", strTableName))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrVon", iVonJahr))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrBis", iBisJahr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitializationData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@USNr", m_InitializationData.UserData.UserNr))

			Return m_utilitySP.ExecuteNonQuery(m_InitializationData.MDData.MDDbConn, strSqlQuery, listOfParams, CommandType.StoredProcedure)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{2}.{0}.{1}", strMethodeName, ex.Message, strTableName))

		End Try

	End Function

#End Region


#Region "Kinder- / Ausbildungszulagen..."

	Function CheckKiAuTable(ByVal strTableName As String, ByVal iVonJahr As Integer, ByVal iBisJahr As Integer) As Short
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty
		Dim cmd As System.Data.SqlClient.SqlCommand
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim rFoundedrec As SqlDataReader

		Try

			Conn.Open()

			strSqlQuery = String.Format("SELECT Top 1 * FROM {0} WHERE MDYear = @MDVomJahr ", strTableName)
			cmd = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			cmd.Parameters.AddWithValue("@MDVomJahr", iVonJahr)
			rFoundedrec = cmd.ExecuteReader

			' LA-Stamm
			rFoundedrec.Read()
			If Not rFoundedrec.HasRows() Then
				Dim strMessage As String = m_Translate.GetSafeTranslationValue("Die Quelldaten sind nicht vorhanden.")
				DevExpress.XtraEditors.XtraMessageBox.Show(strMessage,
																									 m_Translate.GetSafeTranslationValue("Kinder- / Ausbildungszulagen"),
																									 MessageBoxButtons.OK,
																									 MessageBoxIcon.Exclamation)
				m_Logger.LogWarning(String.Format("{2}.{0}.{1}", strMethodeName, strMessage, strTableName))
				Return 1
			End If

			strSqlQuery = String.Format("SELECT Top 1 * FROM {0} WHERE MDYear = @MDVomJahr ", strTableName)
			cmd = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			cmd.Parameters.AddWithValue("@MDVomJahr", iBisJahr)
			rFoundedrec.Close()
			rFoundedrec = cmd.ExecuteReader

			' LA-Stamm
			rFoundedrec.Read()
			If rFoundedrec.HasRows() Then
				Dim strMessage As String = m_Translate.GetSafeTranslationValue("Für das ausgewählte Jahr existieren Daten. Möchten Sie die Daten überschreiben?")
				Dim msgResult As MsgBoxResult = DevExpress.XtraEditors.XtraMessageBox.Show(strMessage,
																									 m_Translate.GetSafeTranslationValue("Kinder- / Ausbildungszulagen"),
																									 MessageBoxButtons.YesNoCancel,
																									 MessageBoxIcon.Question,
																									 MessageBoxDefaultButton.Button1)
				m_Logger.LogInfo(String.Format("{2}.{0}.{1}:{3}", strMethodeName, strMessage, strTableName, msgResult))
				If msgResult = MsgBoxResult.Cancel Then
					Return 1

				ElseIf msgResult = MsgBoxResult.Ok Then
					strSqlQuery = "Delete {0} Where MDYear = @MDVomJahr"
					cmd.CommandType = Data.CommandType.Text
					cmd.Parameters.AddWithValue("@MDVomJahr", Me.cboToYear.Text)
					cmd.ExecuteNonQuery()

					Return 0

				ElseIf msgResult = MsgBoxResult.No Then
					Return 2

				End If
			End If


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
		Return 0
	End Function

	Public Function CopyKiAuData(ByVal strTableName As String, ByVal iVonJahr As Integer, ByVal iBisJahr As Integer) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty

		Try

			strSqlQuery = "[Copy KiAuData For NewMDYear]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@tblName", strTableName))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrVon", iVonJahr))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrBis", iBisJahr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitializationData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@USNr", m_InitializationData.UserData.UserNr))

			Return m_utilitySP.ExecuteNonQuery(m_InitializationData.MDData.MDDbConn, strSqlQuery, listOfParams, CommandType.StoredProcedure)


		Catch ex As Exception
			m_Logger.LogError(String.Format("{2}.{0}.{1}", strMethodeName, ex.Message, strTableName))

		End Try

	End Function

#End Region


#Region "Mandantendaten..."

	Function CheckMDTable(ByVal strTableName As String, ByVal iVonJahr As Integer, ByVal iBisJahr As Integer) As Short
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty
		Dim cmd As System.Data.SqlClient.SqlCommand
		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim rFoundedrec As SqlDataReader

		Try

			Conn.Open()
			If strTableName.ToLower = "mandanten".ToLower Then
				strSqlQuery = String.Format("SELECT Top 1 * FROM {0} WHERE Jahr = @MDVomJahr And MDNr = @MDNr", strTableName)

			ElseIf strTableName.ToLower = "TabBVGMann".ToLower Or strTableName.ToLower = "TabBVGFrauen".ToLower Then
				strSqlQuery = String.Format("SELECT Top 1 * FROM {0} WHERE prozjahr = @MDVomJahr And MDNr = @MDNr", strTableName)

			ElseIf strTableName.ToLower = "LA".ToLower Then
				strSqlQuery = String.Format("SELECT Top 1 * FROM {0} WHERE LAjahr = @MDVomJahr", strTableName)

			ElseIf strTableName.ToLower = "MD_KK_LMV".ToLower Or strTableName.ToLower = "MD_KiAu".ToLower Then
				strSqlQuery = String.Format("SELECT Top 1 * FROM {0} WHERE MDYear = @MDVomJahr And MDNr = @MDNr", strTableName)

			ElseIf strTableName.ToLower = "MD_TSP_LMV".ToLower Then
				strSqlQuery = String.Format("SELECT Top 1 * FROM {0} WHERE MDYear = @MDVomJahr", strTableName)

			End If

			cmd = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			cmd.Parameters.AddWithValue("@MDVomJahr", iVonJahr)
			If Not strTableName.ToLower = "MD_TSP_LMV".ToLower Then cmd.Parameters.AddWithValue("@MDNr", m_InitializationData.MDData.MDNr)
			rFoundedrec = cmd.ExecuteReader

			' LA-Stamm
			rFoundedrec.Read()
			If Not rFoundedrec.HasRows() Then
				Dim strMessage As String = String.Format(m_Translate.GetSafeTranslationValue("{0}: Die Quelldaten sind nicht vorhanden."), strTableName)

				m_Logger.LogWarning(String.Format("{2}.{0}.{1}", strMethodeName, strMessage, strTableName))
				If strTableName.ToLower = "MD_KK_LMV".ToLower OrElse strTableName.ToLower = "MD_KiAu".ToLower OrElse strTableName.ToLower = "MD_TSP_LMV".ToLower Then
					Return 0
				Else
					Return 1
				End If

			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
		Return 0
	End Function

	Public Function CopyMDData(ByVal strTableName As String, ByVal iVonJahr As Integer, ByVal iBisJahr As Integer) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = String.Empty

		Try

			strSqlQuery = "[Copy MDData For NewMDYear]"
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@tblName", strTableName))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrVon", iVonJahr))
			listOfParams.Add(New SqlClient.SqlParameter("@jahrBis", iBisJahr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitializationData.MDData.MDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@USNr", m_InitializationData.UserData.UserNr))

			Return m_utilitySP.ExecuteNonQuery(m_InitializationData.MDData.MDDbConn, strSqlQuery, listOfParams, CommandType.StoredProcedure)

		Catch ex As Exception
			m_Logger.LogError(String.Format("{2}.{0}.{1}", strMethodeName, ex.Message, strTableName))

		End Try

	End Function

#End Region



End Class
