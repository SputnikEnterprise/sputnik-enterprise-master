
Option Strict Off

Imports System.Reflection.Assembly

Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten

Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports DevExpress.LookAndFeel

Imports SPS.MD.GAVAddressUtility.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class frmGAVAdressen
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private TotalLVAdressenData As New List(Of LvDataAdresse) ' GAV-Adressen
	Private _CopyHashTable As Hashtable = New Hashtable() ' Die Werte kopieren

	Private m_progpaath As New ClsProgPath
	Private m_common As New CommonSetting
	Private m_utility As New Utilities
	Private m_md As New Mandant



	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_InitializationData = _setting

	End Sub



	Public Structure GAVBerufAdresse
		Private _bezeichnung As String
		Private _kanton As List(Of String)
		Public Property Bezeichnung() As String
			Get
				Return _bezeichnung
			End Get
			Set(ByVal value As String)
				_bezeichnung = value
			End Set
		End Property
		Public ReadOnly Property Kanton() As List(Of String)
			Get
				If _kanton Is Nothing Then
					_kanton = New List(Of String)
				End If
				Return _kanton
			End Get
		End Property
	End Structure

	Public Structure LvDataAdresse
		Public iID As String
		Public RecNr As String
		Public BerufBez As String
		Public Name As String
		Public ZHD As String
		Public Postfach As String
		Public Strasse As String
		Public PLZ As String
		Public Ort As String
		Public AdressNummer As String
		Public BankName As String
		Public BankPLZ As String
		Public PCKonto As String
		Public IBAN As String
		Public Kanton As String
		Public Organ As String

		Public Sub New(ByVal recDoc As SqlClient.SqlDataReader)
			Try
				With recDoc
					Me.iID = recDoc("ID").ToString
					Me.RecNr = recDoc("RecNr").ToString
					Me.BerufBez = recDoc("BerufBez").ToString
					Me.Name = recDoc("GAV_Name").ToString
					Me.ZHD = recDoc("GAV_ZHD").ToString
					Me.Postfach = recDoc("GAV_Postfach").ToString
					Me.Strasse = recDoc("GAV_Strasse").ToString
					Me.PLZ = recDoc("GAV_PLZ").ToString
					Me.Ort = recDoc("GAV_Ort").ToString
					Me.AdressNummer = "" 'recDoc("GAV_AdressNr").ToString
					Me.BankName = recDoc("GAV_Bank").ToString
					Me.BankPLZ = recDoc("GAV_BankPLZOrt").ToString
					Me.PCKonto = recDoc("GAV_Bankkonto").ToString
					Me.IBAN = recDoc("GAV_IBAN").ToString
					Me.Kanton = recDoc("Kanton").ToString
					Me.Organ = recDoc("Organ").ToString
				End With
			Catch ex As Exception
				' Kein Fehler
			End Try


		End Sub

	End Structure


#Region "Form-Events"

	Sub TranslateForm()

		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.CmdClose.Text = m_Translate.GetSafeTranslationValue(Me.CmdClose.Text)

		Me.lblHeader1.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader1.Text)
		Me.lblHeader2.Text = m_Translate.GetSafeTranslationValue(Me.lblHeader2.Text)
		Me.lblDetail.Text = m_Translate.GetSafeTranslationValue(Me.lblDetail.Text)

		Me.lblgavberuf.Text = m_Translate.GetSafeTranslationValue(Me.lblgavberuf.Text)
		Me.lblname.Text = m_Translate.GetSafeTranslationValue(Me.lblname.Text)
		Me.lblNummer.Text = m_Translate.GetSafeTranslationValue(Me.lblNummer.Text)
		Me.lbliban.Text = m_Translate.GetSafeTranslationValue(Me.lbliban.Text)
		Me.lblJahr.Text = m_Translate.GetSafeTranslationValue(Me.lblJahr.Text)
		Me.lblkanton.Text = m_Translate.GetSafeTranslationValue(Me.lblkanton.Text)
		Me.lblorgen.Text = m_Translate.GetSafeTranslationValue(Me.lblorgen.Text)
		Me.lblpckonto.Text = m_Translate.GetSafeTranslationValue(Me.lblpckonto.Text)
		Me.lblplzort.Text = m_Translate.GetSafeTranslationValue(Me.lblplzort.Text)
		Me.lblzhd.Text = m_Translate.GetSafeTranslationValue(Me.lblzhd.Text)

		Me.lblstrasse.Text = m_Translate.GetSafeTranslationValue(Me.lblstrasse.Text)
		Me.lblpostfach.Text = m_Translate.GetSafeTranslationValue(Me.lblpostfach.Text)
		Me.lblbankplzort.Text = m_Translate.GetSafeTranslationValue(Me.lblbankplzort.Text)
		Me.lblbankname.Text = m_Translate.GetSafeTranslationValue(Me.lblbankname.Text)

		Me.lblbetriebsnummer.Text = m_Translate.GetSafeTranslationValue(Me.lblbetriebsnummer.Text)

		Me.bbiDelete.Caption = m_Translate.GetSafeTranslationValue(Me.bbiDelete.Caption)
		Me.bbiNew.Caption = m_Translate.GetSafeTranslationValue(Me.bbiNew.Caption)
		Me.bbiSave.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSave.Caption)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)

	End Sub


	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmGAVAdressen_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Dim time1 As New Stopwatch
			time1.Start()
			TranslateForm()
			time1.Stop()
			Trace.WriteLine(String.Format("1. Zeit: {0}", time1.Elapsed))

		Catch ex As Exception

		End Try

		Try
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(m_InitialData.MDData.MDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try
			If My.Settings.frmLocation <> String.Empty Then
				Me.Width = Math.Max(My.Settings.iWidth, Me.Width)
				Me.Height = Math.Max(My.Settings.iHeight, Me.Height)
				Dim aLoc As String() = My.Settings.frmLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		SetLvwHeader()
		FillLVAdressen()

	End Sub

	Private Sub frmESSearch_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
		If e.KeyCode = Keys.F12 And m_common.GetLogedUserNr = 1 Then
			Dim strRAssembly As String = ""
			Dim strMsg As String = "Information über Modul: {0}Bibliothek: {1}{0}Ort: {2}{0}{0}ReferencedAssamblies:{0}{3}{0}"
			For Each a In AppDomain.CurrentDomain.GetAssemblies()
				strRAssembly &= String.Format("-->> {2}{0}{1}({3}){0}", vbNewLine, vbTab, a, a.CodeBase)
			Next
			strMsg = String.Format(strMsg, vbNewLine,
														 GetExecutingAssembly().FullName,
														 GetExecutingAssembly().Location,
														 strRAssembly)
			DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Modul-Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
		End If
	End Sub


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmGAVAdressen_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.iWidth = Me.Width
				My.Settings.iHeight = Me.Height
				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! es ist nicht wichtig, wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmGAVAdressen_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
		Me.lvAdressen.Focus()
	End Sub

#End Region

#Region "Control-Events"

	Private Sub bbiSave_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick
		Dim Stopwatch As Stopwatch = New Stopwatch()
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Daten werden gespeichert...")

			If IsDataOK() Then
				If SaveAdresse() Then
					FillLVAdressen()
					Me.bbiDelete.Enabled = True

					SetListViewIndexByID(CInt(Me.txtRecNr.Text))
					'HighlightSelectedItem()
				End If
			End If


		Catch ex As Exception

		Finally
			Me.Timer1.Enabled = False
			conn.Close()
		End Try

	End Sub

	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiNew_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiNew.ItemClick

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		ResetAllEntries()

		' Default-Werte
		Me.txtRecNr.Text = "0"
		'RemoveHighlight()
		Me.txtGAV_Name.Focus()

		Me.bbiDelete.Enabled = False

	End Sub

	Private Sub lv_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnWidthChangedEventArgs) Handles lvAdressen.ColumnWidthChanged
		SaveLV_ColumnInfo(sender)
	End Sub


	Private Sub lvAdressen_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvAdressen.SelectedIndexChanged

		Try

			'RemoveHighlight()

			Dim lv As ListView = DirectCast(sender, ListView)

			If lv.SelectedItems.Count > 0 Then
				Dim adrId As Integer = CInt(lv.SelectedItems(0).SubItems(0).Text) ' ID
				ShowAdresse(adrId)
				'RemoveHighlight()
				Me.bbiDelete.Enabled = True

				'HighlightSelectedItem()
			End If

		Catch ex As Exception
			'_ex.MessageBoxShowError(_ClsProgSetting.GetLogedUSNr, "Adressen IndexChanged", ex)
		End Try

	End Sub


	Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdrIdCopy.Click
		_CopyHashTable.Clear()
		For Each con As Control In PanelControl1.Controls
			If TypeOf con Is TextBox Then
				_CopyHashTable.Add(con.Name, con.Text)
			End If
		Next
		Me.btnAdrIdInsert.Enabled = True

	End Sub

	Private Sub btnInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdrIdInsert.Click
		For Each key As Object In _CopyHashTable.Keys
			Me.Controls.Find(key, True)(0).Text = _CopyHashTable(key).ToString
		Next
	End Sub

	Private Sub txtAdresseID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
		'If IsNumeric(Me.txtAdresseID.Text) AndAlso CInt(Me.txtAdresseID.Text) > 0 Then
		'  Me.btnAdrIdCopy.Enabled = True
		'Else
		'  Me.btnAdrIdCopy.Enabled = False
		'End If
	End Sub


#End Region

#Region "Methoden"

	Sub SetLvwHeader()
		' Default
		Dim strColumnString As String = "ID;RecNr;GAV-Beruf;Organ;Kanton;Name;Strasse;PLZ;Ort;Reserve;Bank;Bank Ort;PC-Konto;IBAN"
		Dim strColumnWidthInfo As String = "0-0;0-0;150-0;100-0;100-0;100-0;100-0;40-0;0-0;20-0;100-0;50-0;60-0;100-0"


		If My.Settings.LV_Adressen_Size <> String.Empty Then
			If strColumnString.Split(CChar(";")).Count = My.Settings.LV_Adressen_Size.Split(CChar(";")).Count Then
				strColumnWidthInfo = My.Settings.LV_Adressen_Size
			End If
		End If
		FillDataHeaderLv(Me.lvAdressen, m_Translate.GetSafeTranslationValue(strColumnString), strColumnWidthInfo)

	End Sub

	Sub SaveLV_ColumnInfo(ByVal sender As System.Object)
		Dim strColInfo As String = String.Empty
		Dim strColInfo_1 As String = String.Empty
		Dim strColAlign As String = String.Empty
		Dim lv As ListView = DirectCast(sender, ListView)


		For i As Integer = 0 To lvAdressen.Columns.Count - 1
			If lv.Columns.Item(i).TextAlign = HorizontalAlignment.Center Then
				strColAlign = "2"

			ElseIf lv.Columns.Item(i).TextAlign = HorizontalAlignment.Right Then
				strColAlign = "1"
			Else
				strColAlign = "0"
			End If
			strColInfo &= CStr(IIf(strColInfo = String.Empty, "", ";")) & (lv.Columns.Item(i).Width) & "-" & strColAlign

		Next

		Try

			My.Settings.LV_Adressen_Size = strColInfo

			Trace.WriteLine(strColInfo)
			My.Settings.Save()

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub FillLVAdressen()
		Dim Stopwatch As Stopwatch = New Stopwatch()
		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			conn.Open()

			' Initialisieren
			lvAdressen.SelectedItems.Clear()
			lvAdressen.Items.Clear()
			TotalLVAdressenData.Clear()

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Adress-Daten wird gesucht...")

			' Vorbereitung
			Dim cmdText As String = ""
			Dim cmd As SqlCommand = New SqlCommand("", conn)
			cmd.CommandType = CommandType.Text
			Dim reader As SqlDataReader

			' Alle Adressen
			cmdText = "SELECT * FROM MD_GAV_Adresse ORDER BY BerufBez, Organ, Kanton"
			cmd.CommandText = cmdText
			reader = cmd.ExecuteReader()
			GetLvAdressenDataCount(reader)
			reader.Close()

			' Die gesammelten Adressen werden ins Listview übertragen
			Dim lvItem As ListViewItem
			For index As Integer = 0 To TotalLVAdressenData.Count - 1
				lvItem = New ListViewItem(New String() {
																								 TotalLVAdressenData.Item(index).iID,
																								 TotalLVAdressenData.Item(index).RecNr,
																								 TotalLVAdressenData.Item(index).BerufBez,
																								 TotalLVAdressenData.Item(index).Organ,
																								 TotalLVAdressenData.Item(index).Kanton,
																								 TotalLVAdressenData.Item(index).Name,
																								 TotalLVAdressenData.Item(index).Strasse,
																								 TotalLVAdressenData.Item(index).PLZ,
																								 TotalLVAdressenData.Item(index).Ort,
																								 TotalLVAdressenData.Item(index).AdressNummer,
																								 TotalLVAdressenData.Item(index).BankName,
																								 TotalLVAdressenData.Item(index).BankPLZ,
																								 TotalLVAdressenData.Item(index).PCKonto,
																								 TotalLVAdressenData.Item(index).IBAN
																							 })
				lvAdressen.Items.Add(lvItem)

			Next

			' Zusatzinfos
			Me.LblTimeValue.Text = String.Format("Datenauflistung für {0} Einträge: in {1} ms", lvAdressen.Items.Count.ToString, Stopwatch.ElapsedMilliseconds().ToString)
			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet..."), lvAdressen.Items.Count.ToString)


		Catch ex As Exception
			MessageBox.Show(ex.Message, "FillLVAdressen")

		Finally
			conn.Close()
			Me.Timer1.Enabled = False

		End Try
	End Sub

	Protected Friend Function SaveAdresse() As Boolean
		Dim allesOK As Boolean = True

		Try

			Dim cmdText As String = ""

			' Bestehende Adresse mutieren
			Dim iNewIDNr As Integer = CInt(Val(Me.txtRecNr.Text))
			If CInt(Me.txtRecNr.Text) > 0 Then
				cmdText = "UPDATE MD_GAV_Adresse "
				cmdText += "SET MDNr = @MDNr, [BerufBez] = @gavBeruf, [GAV_Name] = @name, [GAV_ZHD] = @zhd, [GAV_Postfach] = @postfach, "
				cmdText += "[GAV_Strasse] = @strasse, [GAV_PLZ] = @plz, [GAV_Ort] = @ort, [GAV_AdressNr] = @adressNr, "
				cmdText += "[GAV_Bank] = @bank, [GAV_BankPLZOrt] = @bankPLZOrt, [GAV_BankKonto] = @bankKonto, "
				cmdText += "[GAV_IBAN] = @iban, [Result] = '', [Kanton] = @kanton, [Organ] = @organ "
				cmdText += "WHERE "
				cmdText += "RecNr = @RecNr" ' And MDNr = @MDNr"

				'' Die ID holen (muss nicht, aber programmatisch einfacher)
				'cmdText += "SELECT ID As ID FROM MD_GAV_Adresse WHERE ID = @id "
			Else
				' KANTON
				'pKanton.Value = Me.Cbo_Kanton.Text
				iNewIDNr = GetNewIDNr()

				' Neue Adresse hinzufügen
				'cmdText += "Declare @NewID int "
				'cmdText += "set @NewID =IDENT_CURRENT('MD_GAV_Adresse') "

				cmdText += "INSERT INTO MD_GAV_Adresse "
				cmdText += "(MDNr, [RecNr], [BerufBez], [GAV_Name], [GAV_ZHD], [GAV_Postfach], [GAV_Strasse], [GAV_PLZ], [GAV_Ort], "
				cmdText += "[GAV_AdressNr], [GAV_Bank], [GAV_BankPLZOrt], [GAV_BankKonto], [GAV_IBAN], [Result], [Kanton], [Organ]) "
				cmdText += "VALUES ("
				cmdText += "@MDNr, @RecNr, "
				cmdText += "@gavBeruf, @name, @zhd, @postfach, @strasse, @plz, @ort, @adressNr, "
				cmdText += "@bank, @bankPLZOrt, @bankKonto, @iban, '', @kanton, @organ) "

				'' Die ID holen
				'cmdText += "SELECT Max(ID) As ID FROM MD_GAV_Adresse "
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitialData.MDData.MDNr))
			'listOfParams.Add(New SqlClient.SqlParameter("@id", Me.txtAdresseID.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@recNr", iNewIDNr))
			listOfParams.Add(New SqlClient.SqlParameter("@gavBeruf", Me.Cbo_GAVBeruf.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@name", Me.txtGAV_Name.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@zhd", Me.txtGAV_ZHD.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@postfach", Me.txtGAV_Postfach.Text))

			listOfParams.Add(New SqlClient.SqlParameter("@strasse", Me.txtGAV_Strasse.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@plz", Me.txtGAV_PLZ.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@ort", Me.txtGAV_Ort.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@adressNr", Me.txtGAV_AdressNr.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@bank", Me.txtGAV_Bank.Text))

			listOfParams.Add(New SqlClient.SqlParameter("@bankPLZOrt", Me.txtGAV_BankPLZOrt.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@bankKonto", Me.txtGAV_BankKonto.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@iban", Me.txtGAV_IBAN.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@kanton", Me.Cbo_Kanton.Text))
			listOfParams.Add(New SqlClient.SqlParameter("@organ", Me.Cbo_Organ.Text))

			Dim reader = m_utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, cmdText, listOfParams)

			If reader Then
				Me.txtRecNr.Text = iNewIDNr
				Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Daten gespeichert...")

				allesOK = True

			Else
				DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gespeichert werden."),
												m_Translate.GetSafeTranslationValue("Dokument speichern"), MessageBoxButtons.OK, MessageBoxIcon.Error)
				allesOK = False

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			allesOK = False
		Finally

		End Try

		Return allesOK
	End Function

	Function GetNewIDNr() As Integer
		Dim result As Integer = 0

		Dim sSql As String = "Select Top 1 ID From MD_GAV_Adresse Order By ID Desc"
		Dim reader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, sSql, Nothing)
		If Not (reader Is Nothing) Then
			If reader.Read Then
				result = m_utility.SafeGetInteger(reader, "ID", 0) + 1
			End If
			reader.Close()
		End If

		Return result
	End Function

	Function IsDataOK() As Boolean
		Dim allesOk As Boolean = True
		Dim meldung As String = ""
		Dim highlightList As ArrayList = New ArrayList()

		'RemoveHighlight()

		If Me.txtGAV_Name.Text.Trim.Length = 0 Then
			allesOk = False
			meldung += String.Format(m_Translate.GetSafeTranslationValue("Sie müssen einen Namen angeben.{0}"), vbLf)
			highlightList.Add(Me.txtGAV_Name)
		End If

		If Not allesOk Then
			If meldung.Length > 0 Then
				MessageBox.Show(String.Format(m_Translate.GetSafeTranslationValue("Die Adresse kann nicht gespeichert werden.{0}{0}{1}"), vbLf, meldung),
												m_Translate.GetSafeTranslationValue("Dokument speichern"), MessageBoxButtons.OK, MessageBoxIcon.Warning)
				DirectCast(highlightList.Item(0), Control).Focus()
				'SetHighlight(highlightList)
			End If
		End If

		Return allesOk
	End Function

	'Sub SetHighlight(ByVal arrayList As ArrayList)
	'  For Each con As Control In arrayList
	'    con.BackColor = Color.FromArgb(255, 235, 235)
	'  Next
	'End Sub

	'Sub RemoveHighlight()
	'  'For Each con As Control In PanelControl1.Controls
	'  '  If TypeOf (con) Is TextBox Then
	'  '    DirectCast(con, TextBox).BackColor = Color.FromKnownColor(KnownColor.Window)
	'  '  End If
	'  'Next
	'End Sub

	''' <summary>
	''' Alle Textfelder leeren um eine neue Adresse einzugeben.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllEntries()

		ResetControl(Me)
		'RemoveHighlight()

		' Default-Werte
		Me.txtRecNr.Text = 0

	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Funktion mit rekursivem Aufruf.</remarks>
	Private Sub ResetControl(ByVal con As Control)

		If TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
			Dim tb As DevExpress.XtraEditors.TextEdit = con
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.ComboBoxEdit Then
			Dim cbo As DevExpress.XtraEditors.ComboBoxEdit = con
			cbo.Properties.Items.Clear()
			cbo.Text = String.Empty

		ElseIf con.HasChildren Then
			For Each conChild As Control In con.Controls
				ResetControl(conChild)
			Next
		End If

	End Sub

	Private Sub ShowAdresse(ByVal iRecNr As Integer)
		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Try
			Dim cmdText As String = "SELECT * FROM MD_GAV_Adresse WHERE RecNr = @RecNr And MDNr = @MDNr "
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@RecNr", iRecNr))
			listOfParams.Add(New SqlClient.SqlParameter("@MDNr", m_InitialData.MDData.MDNr))

			Dim reader = m_utility.OpenReader(m_InitialData.MDData.MDDbConn, cmdText, listOfParams)

			'Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
			'Dim padrId As SqlParameter = New SqlParameter("@RecNr", SqlDbType.Int)
			'Dim padrMDNr As SqlParameter = New SqlParameter("@MDNr", SqlDbType.Int)

			'cmd.Parameters.Add(padrId)
			'cmd.Parameters.Add(padrMDNr)

			'padrId.Value = iRecNr
			'padrMDNr.Value = ClsDataDetail.ProgSettingData.SelectedMDNr

			'conn.Open()
			'Dim reader As SqlDataReader = cmd.ExecuteReader()
			If Not (reader Is Nothing) Then
				If reader.Read() Then
					Me.txtRecNr.Text = m_utility.SafeGetInteger(reader, "RecNr", 0)
					Me.txtGAV_AdressNr.Text = m_utility.SafeGetString(reader, "GAV_AdressNr", "")

					Me.Cbo_GAVBeruf.Text = m_utility.SafeGetString(reader, "BerufBez", "")
					Me.Cbo_Kanton.Text = m_utility.SafeGetString(reader, "Kanton", "")
					Me.Cbo_Organ.Text = m_utility.SafeGetString(reader, "Organ", "")

					Me.txtGAV_Name.Text = m_utility.SafeGetString(reader, "GAV_Name", "")
					Me.txtGAV_ZHD.Text = m_utility.SafeGetString(reader, "GAV_ZHD", "")
					Me.txtGAV_Postfach.Text = m_utility.SafeGetString(reader, "GAV_Postfach", "")
					Me.txtGAV_Strasse.Text = m_utility.SafeGetString(reader, "GAV_Strasse", "")
					Me.txtGAV_PLZ.Text = m_utility.SafeGetString(reader, "GAV_PLZ", "")
					Me.txtGAV_Ort.Text = m_utility.SafeGetString(reader, "GAV_Ort", "")

					Me.txtGAV_Bank.Text = m_utility.SafeGetString(reader, "GAV_Bank", "")
					Me.txtGAV_BankPLZOrt.Text = m_utility.SafeGetString(reader, "GAV_BANKPLZOrt", "")
					Me.txtGAV_BankKonto.Text = m_utility.SafeGetString(reader, "GAV_BankKonto", "")
					Me.txtGAV_IBAN.Text = m_utility.SafeGetString(reader, "GAV_IBAN", "")

				End If
			End If

			'reader.Close()
			Check4RemoteGAVData()

		Catch ex As Exception
			'_ex.MessageBoxShowError(_ClsProgSetting.GetLogedUSNr, "ShowAdressItem", ex)
		Finally
			conn.Close()
		End Try
	End Sub

	Sub Check4RemoteGAVData()
		' Damit die nicht mehr gültigen GAV-Berufe angezeigt werden können. (Aus dem Web-Service geliefert)
		Dim aReomteGAVData As List(Of String) = GetGAVAdressData(Me.Cbo_GAVBeruf.Text, Me.Cbo_Organ.Text, Me.Cbo_Kanton.Text)
		Me.libLoadAddress.Visible = aReomteGAVData.Count > 0

	End Sub

	Sub SetListViewIndexByID(ByVal iRecNr As Integer)
		For Each item As ListViewItem In lvAdressen.Items
			If CInt(item.SubItems(1).Text) = iRecNr Then
				lvAdressen.SelectedIndices.Add(item.Index)
			End If
		Next
	End Sub

	'Sub HighlightSelectedItem()
	'  If lvAdressen.SelectedItems.Count > 0 Then
	'    For Each item As ListViewItem In lvAdressen.Items
	'      item.BackColor = Color.FromKnownColor(KnownColor.Window)
	'    Next
	'    lvAdressen.SelectedItems(0).BackColor = Color.LightGray
	'  End If

	'  lvAdressen.SelectedItems(0).EnsureVisible()
	'End Sub


#End Region


#Region "Funktionen"

	Function GetLvAdressenDataCount(ByVal rKDrec As SqlClient.SqlDataReader) As Integer
		Dim i As Integer = 0

		While rKDrec.Read
			TotalLVAdressenData.Add(New LvDataAdresse(rKDrec))

			i += 1
		End While

		Return i
	End Function

#End Region

	Private Sub bbiDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiDelete.ItemClick
		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Try
			'If Me.Cbo_Kanton.Text.Length = 0 Then
			If DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue("Möchten Sie die Adresse löschen?"),
																										m_Translate.GetSafeTranslationValue("Achtung"),
																										MessageBoxButtons.YesNo,
																										MessageBoxIcon.Question,
																										MessageBoxDefaultButton.Button2) = DialogResult.Yes Then

				Dim cmdText As String = ""

				cmdText = "DELETE MD_GAV_Adresse WHERE RecNr = @RecNr"
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("@RecNr", CInt(Val(Me.txtRecNr.Text))))

				Dim m_utilites As New Utilities
				Dim reader = m_utilites.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, cmdText, listOfParams)
				If reader Then
					DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich gelöscht."),
																										 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxButtons.OK, MessageBoxIcon.Information)
					Me.txtRecNr.Text = "0"
					Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich gelöscht.")

					FillLVAdressen()

				Else
					DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gelöscht werden."),
																										 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxButtons.OK, MessageBoxIcon.Error)

				End If
			End If


		Catch ex As Exception

		Finally
			conn.Close()
		End Try
	End Sub

	Private Sub Cbo_GAVBeruf_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_GAVBeruf.QueryPopUp
		FillGAVBerufeDropDown()
	End Sub

	Private Sub Cbo_Kanton_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Kanton.QueryPopUp
		FillKantoneDropDown()
	End Sub

	Private Sub Cbo_Organ_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Organ.QueryPopUp
		FillOrganDropDown()
	End Sub

	Private Sub lnkAdresseLaden_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles libLoadAddress.LinkClicked
		' Damit die nicht mehr gültigen GAV-Berufe angezeigt werden können. (Aus dem Web-Service geliefert)
		Dim aReomteGAVData As List(Of String) = GetGAVAdressData(Me.Cbo_GAVBeruf.Text, Me.Cbo_Organ.Text, Me.Cbo_Kanton.Text)

		For i As Integer = 0 To aReomteGAVData.Count - 1
			Me.txtGAV_Name.Text = aReomteGAVData(1).ToString

			Me.txtGAV_ZHD.Text = aReomteGAVData(2).ToString
			Me.txtGAV_Postfach.Text = aReomteGAVData(3).ToString
			Me.txtGAV_Strasse.Text = aReomteGAVData(4).ToString
			Me.txtGAV_PLZ.Text = aReomteGAVData(5).ToString
			Me.txtGAV_Ort.Text = aReomteGAVData(6).ToString

			Me.txtGAV_Bank.Text = aReomteGAVData(8).ToString
			Me.txtGAV_BankPLZOrt.Text = aReomteGAVData(9).ToString
			Me.txtGAV_BankKonto.Text = aReomteGAVData(10).ToString
			Me.txtGAV_IBAN.Text = aReomteGAVData(11).ToString

			Me.txtGAV_AdressNr.Text = String.Empty

		Next

	End Sub


	Private Sub Cbo_GAVBeruf_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_GAVBeruf.SelectedIndexChanged,
		Cbo_Kanton.SelectedIndexChanged, Cbo_Organ.SelectedIndexChanged
		Check4RemoteGAVData()
	End Sub

#Region "DropDown Functions..."
	''' <summary>
	''' Füllt die Dropdownlistbox mit GAV-Berufe, die bei Kunden zugewiesen sind.
	''' </summary>
	''' <remarks></remarks>
	Sub FillGAVBerufeDropDown()
		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			conn.Open()
			Dim cmdText As String = ""
			Dim adresseProGAVKanton As GAVBerufAdresse = New GAVBerufAdresse()
			Dim tempGAVBeruf As String = ""
			cmdText = "SELECT  Bezeichnung "
			cmdText += "FROM KD_GAVGruppe Where Bezeichnung Not In ('Allgemeine GAV') "
			cmdText += "GROUP BY Bezeichnung "
			cmdText += "ORDER BY Bezeichnung "
			Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
			cmd.CommandText = cmdText
			Dim reader As SqlDataReader = cmd.ExecuteReader()

			Me.Cbo_GAVBeruf.Properties.Items.Clear()
			Me.Cbo_GAVBeruf.Properties.Items.Add("")
			While reader.Read()
				Me.Cbo_GAVBeruf.Properties.Items.Add(reader("Bezeichnung").ToString())
			End While


		Catch ex As Exception
			'_ex.MessageBoxShowError(iLogedUSNr, "", ex)
		Finally
			conn.Close()
		End Try

	End Sub

	Private Sub FillKantoneDropDown()
		Dim conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			conn.Open()
			Dim cmdText As String = ""
			cmdText = "SELECT GetFeld "
			cmdText += "FROM TAB_Kanton "
			cmdText += "ORDER BY GetFeld "
			Dim cmd As SqlCommand = New SqlCommand(cmdText, conn)
			cmd.CommandText = cmdText
			Dim reader As SqlDataReader = cmd.ExecuteReader()

			Me.Cbo_Kanton.Properties.Items.Clear()
			Me.Cbo_Kanton.Properties.Items.Add("")
			While reader.Read()
				Me.Cbo_Kanton.Properties.Items.Add(m_utility.SafeGetString(reader, "GetFeld", ""))
			End While
			Me.Cbo_Kanton.Properties.DropDownRows = 27

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			'_ex.MessageBoxShowError(_ClsProgSetting.GetLogedUSNr, "FillKantoneDropDown", ex)
		Finally
			conn.Close()
		End Try
	End Sub

	Private Sub FillOrganDropDown()

		Cbo_Organ.Properties.Items.Clear()
		Cbo_Organ.Properties.Items.Add("")
		Cbo_Organ.Properties.Items.Add("Parifond")
		Cbo_Organ.Properties.Items.Add("FAR/VRM")

	End Sub


#End Region


#Region "Helpers..."

	Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
		Dim lstStuff As ListViewItem = New ListViewItem()
		Dim lvwColumn As ColumnHeader

		With Lv
			.Clear()

			' Nr;Nummer;Name;Strasse;PLZ Ort
			If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
			If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

			Dim strCaption As String() = Regex.Split(strColumnList, ";")
			' 0-1;0-1;2000-0;2000-0;2500-0
			Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
			Dim strFieldWidth As String
			Dim strFieldAlign As String = "0"
			Dim strFieldData As String()

			For i = 0 To strCaption.Length - 1
				lvwColumn = New ColumnHeader()
				lvwColumn.Text = strCaption(i).ToString
				strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

				If strFieldInfo(i).ToString.StartsWith("-") Then
					strFieldWidth = strFieldData(1)
					lvwColumn.Width = CInt(strFieldWidth) * -1
					If strFieldData.Count > 1 Then
						strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
					End If
				Else
					strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
					lvwColumn.Width = CInt(strFieldWidth) '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
					If strFieldData.Count > 1 Then
						strFieldAlign = strFieldData(1)
					End If
					'CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
				End If
				If strFieldAlign = "1" Then
					lvwColumn.TextAlign = HorizontalAlignment.Right
				ElseIf strFieldAlign = "2" Then
					lvwColumn.TextAlign = HorizontalAlignment.Center
				Else
					lvwColumn.TextAlign = HorizontalAlignment.Left

				End If

				lstStuff.BackColor = Color.Yellow

				.Columns.Add(lvwColumn)
			Next

			lvwColumn = Nothing
		End With

	End Sub



#End Region


End Class

