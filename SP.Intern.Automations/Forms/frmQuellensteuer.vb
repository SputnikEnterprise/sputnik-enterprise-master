
Imports System.IO
Imports SP.Infrastructure.UI


Public Class frmQuellensteuer
	Inherits DevExpress.XtraEditors.XtraForm


	Private Const FILE_PATTERN As String = "*.txt"
	Private Const PATH_NAME As String = "C:\Path\{0}"



	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI
	Private m_Year As String


	Public Sub New()

		DevExpress.Skins.SkinManager.Default.RegisterAssembly(GetType(DevExpress.UserSkins.BonusSkins).Assembly)
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_UtilityUI = New UtilityUI

		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		txt_Year.Text = If(Now.Month > 5, Now.Year + 1, Now.Year)
		txt_Path.Text = String.Format(PATH_NAME, txt_Year.Text)
		txt_Filepattern.Text = FILE_PATTERN
		txtSQL.Text = String.Empty

		lblInfo.Text = "Bereit..."

	End Sub

	Private Sub txtQSTFile_LostFocus(sender As Object, e As EventArgs) Handles txt_Filepattern.LostFocus

		If String.IsNullOrWhiteSpace(txt_Filepattern.EditValue) Then Return
		If Not File.Exists(txt_Filepattern.EditValue) Then Return
		Dim myfile = New FileInfo(txt_Filepattern.EditValue)
		Dim jahr As Integer = Val(myfile.Name.ToString.Substring(3, 2))
		If txt_Year.EditValue Is Nothing Then txt_Year.EditValue = jahr
		If Not txt_Year.EditValue.ToString.EndsWith(jahr) Then
			txt_Year.EditValue = txt_Year.EditValue.ToString.Substring(2, 2)
		End If
		m_Year = txt_Year.EditValue

	End Sub

	Private Function CreateQSTString(ByVal line As String, ByVal setGo As Boolean) As QSTData
		Dim r As New QSTData
		Dim Sql As String = String.Empty
		Dim jahr As String = m_Year

		r.jahr = jahr
		r.recordart = Mid(line, 1, 2)        ' Recordart

		Dim newTableName As String = String.Empty
		Dim oldTableName As String = String.Empty


		If r.recordart = "00" Then

				r.kanton = Mid(line, 3, 2)        ' Kanton
				r.datum = Mid(line, 20, 8)      ' Datum gültig ab
				newTableName = String.Format("[Tar{0}{1}]", r.jahr, r.kanton)
				oldTableName = String.Format("[Tar{0}{1}]", r.jahr - 1, r.kanton)

			If chkTruncateTables.Checked Then
				Sql = String.Format("SET NOCOUNT ON; {0}", vbNewLine)

				Sql &= String.Format("IF (EXISTS (SELECT 1 FROM sys.Objects WHERE  Object_id = OBJECT_ID(N'dbo.{0}') AND Type = N'U'))", newTableName, vbNewLine) ' .Replace("[", "").Replace("]", ""), vbNewLine)
				Sql &= String.Format("Begin {0}", vbNewLine)
				Sql &= String.Format("Drop Table dbo.{0}; {1}", newTableName, vbNewLine)
				Sql &= String.Format("End; {0}", vbNewLine)
				Sql &= String.Format("Go {0}", vbNewLine)
				Sql &= String.Format("SELECT * INTO dbo.{0} FROM dbo.{1} WHERE Gruppe IS NULL;{2}", newTableName, oldTableName, vbNewLine)
				Sql &= String.Format("Go {0}", vbNewLine)

				Sql &= String.Format("Truncate Table Dbo.[{1}]{0}", vbNewLine, newTableName.Replace("[", "").Replace("]", ""))
				Sql &= String.Format("Go{0}", vbNewLine)

			Else


				Sql &= String.Format("CREATE TABLE [dbo].{0}( {1}", newTableName, vbNewLine)
				Sql &= String.Format("[Recordart] [smallint] NULL, {0}", vbNewLine)
				Sql &= String.Format("[Art] [int] NULL, {0}", vbNewLine)
				Sql &= String.Format("[Kanton] [nvarchar](2) NULL, {0}", vbNewLine)
				Sql &= String.Format("[Gruppe] [nvarchar](1) NULL, {0}", vbNewLine)
				Sql &= String.Format("[Kinder] [smallint] NULL, {0}", vbNewLine)
				Sql &= String.Format("[Kirchensteuer] [nvarchar](1) NULL, {0}", vbNewLine)
				Sql &= String.Format("[GA] [nvarchar](2) NULL, {0}", vbNewLine)
				Sql &= String.Format("[Datum] [nvarchar](8) NULL, {0}", vbNewLine)
				Sql &= String.Format("[Einkommen] [money] NULL, {0}", vbNewLine)
				Sql &= String.Format("[Schritt] [money] NULL, {0}", vbNewLine)
				Sql &= String.Format("[G] [nvarchar](1) NULL, {0}", vbNewLine)
				Sql &= String.Format("[Kinder2] [smallint] NULL, {0}", vbNewLine)
				Sql &= String.Format("[Steuer_Fr] [money] NULL, {0}", vbNewLine)
				Sql &= String.Format("[Steuer_Proz] [money] NULL, {0}", vbNewLine)
				Sql &= String.Format("[Mindest_Abzug] [money] NULL, {0}", vbNewLine)
				Sql &= String.Format("[CreatedOn] [Datetime] NULL {0}", vbNewLine)
				Sql &= String.Format(") On [PRIMARY] {0}", vbNewLine)
				Sql &= String.Format("GO {0}", vbNewLine)

			End If

			Sql &= "Declare @Mindest_Abzug money = "
			Sql &= String.Format("(Select MAX(ISNULL(Mindest_Abzug, 0)) FROM {0} GROUP BY Mindest_Abzug){1}", oldTableName, vbNewLine) ' mindest_abzug


		ElseIf r.recordart = "99" Then

				r.kanton = Mid(line, 18, 2)        ' Kanton
				newTableName = String.Format("[Tar{0}{1}] ", r.jahr, r.kanton)
				oldTableName = String.Format("[Tar{0}{1}] ", r.jahr - 1, r.kanton)

			Sql &= String.Format("Insert Into {0} ", newTableName)
			Sql &= "(RecordArt, "
				Sql &= "Art, "
				Sql &= "Kanton, "
				Sql &= "Gruppe, "
				Sql &= "Kinder, "
				Sql &= "Kirchensteuer, "
				Sql &= "GA, "
				Sql &= "Datum, "
				Sql &= "Einkommen, "
				Sql &= "Schritt, "
				Sql &= "G, "
				Sql &= "Kinder2, "
				Sql &= "Steuer_Fr, "
				Sql &= "Steuer_Proz, "
				Sql &= "Mindest_Abzug, "
				Sql &= "CreatedOn "
				Sql &= ") "
				Sql &= "Values ("

				Sql &= 0 & ", "            ' RecordArt
				Sql &= 0 & ", '"           ' Transaktion
				Sql &= Trim(r.kanton) & "', '"         ' Kanton
				Sql &= String.Empty & "', "          ' Gruppe
				Sql &= 0 & ", '"          ' Kinder
				Sql &= String.Empty & "', '"        ' Kirchensteuer
				Sql &= String.Empty & "', '"        ' GA
			Sql &= String.Format("{0}0101", m_Year) & "', "         ' Datum
			Sql &= 0 & ", "           ' Einkommen
				Sql &= 0 & ", '"          ' Schritt
				Sql &= String.Empty & "', "         ' G
				Sql &= 0 & ", "           ' Kinder2
				Sql &= 0 & ", "           ' Steuer_Fr
				Sql &= 0 & ", "           ' Steuer_Proz
				Sql &= "@Mindest_Abzug, "                  ' mindest_abzug
				Sql &= "GetDate() "                  ' mindest_abzug
			Sql &= ") "


		Else

				r.transaktion = Mid(line, 3, 2)        ' Transaktion
				r.kanton = Mid(line, 5, 2)        ' Kanton
				newTableName = String.Format("[Tar{0}{1}] ", r.jahr, r.kanton)
				oldTableName = String.Format("[Tar{0}{1}] ", r.jahr - 1, r.kanton)

				r.tarif = Mid(line, 7, 10)       ' CodeTarif

				r.gruppe = Mid(line, 7, 1)        ' Tarifgruppe
				r.kinder = Mid(line, 8, 1)       ' Anzahl Kinder
				r.kirchensteuer = Mid(line, 9, 1)      ' Kirchensteuer
				r.ga = Mid(line, 10, 2)      ' Grenzgänger

				r.datum = Mid(line, 17, 8)      ' Datum gültig ab
				r.jahr = Mid(line, 17, 4)      ' Jahr gültig ab
				r.einkommen = Mid(line, 25, 9)      ' Steuerbares Einkommen ab Fr.
				r.schritt = Mid(line, 34, 9)      ' Terifschritt in Fr.

				r.geschlecht = Mid(line, 43, 1)      ' Geschlecht
				r.kinder2 = Mid(line, 44, 2)      ' Anzahl Kinder
				r.steuer_fr = Mid(line, 46, 9)      ' Steuer in Fr.
				r.steuer_proz = Mid(line, 55, 5)      ' Steuer %-Satz
				r.codestatus = Mid(line, 60, 3)      ' Code Status

				Sql &= String.Format("Insert Into {0} ", newTableName)
				Sql &= "(RecordArt, "
				Sql &= "Art, "
				Sql &= "Kanton, "
				Sql &= "Gruppe, "
				Sql &= "Kinder, "
				Sql &= "Kirchensteuer, "
				Sql &= "GA, "
				Sql &= "Datum, "
				Sql &= "Einkommen, "
				Sql &= "Schritt, "
				Sql &= "G, "
				Sql &= "Kinder2, "
				Sql &= "Steuer_Fr, "
				Sql &= "Steuer_Proz, "
				Sql &= "Mindest_Abzug, "
				Sql &= "CreatedOn "
				Sql &= ") "
				Sql &= "Values ("

				Sql &= Val(r.recordart) & ", "            ' RecordArt
				Sql &= Val(r.transaktion) & ", '"           ' Transaktion
				Sql &= Trim(r.kanton) & "', '"         ' Kanton
				Sql &= Trim(r.gruppe) & "', "          ' Gruppe
				Sql &= Val(r.kinder) & ", '"          ' Kinder
				Sql &= Trim(r.kirchensteuer) & "', '"        ' Kirchensteuer
				Sql &= Trim(r.ga) & "', '"        ' GA
				Sql &= Trim(r.datum) & "', "         ' Datum
				Sql &= Val(r.einkommen) & ", "           ' Einkommen
				Sql &= Val(r.schritt) & ", '"          ' Schritt
				Sql &= Trim(r.geschlecht) & "', "         ' G
				Sql &= Val(r.kinder2) & ", "           ' Kinder2
				Sql &= Val(r.steuer_fr) & ", "           ' Steuer_Fr
				Sql &= Val(r.steuer_proz) & ", "           ' Steuer_Proz
				Sql &= "@Mindest_Abzug, "                  ' mindest_abzug
				Sql &= "GetDate() "                  ' mindest_abzug
			Sql &= ") "

		End If

		If setGo Then
			Sql &= String.Format("{0}Go{0}", vbNewLine)
			Sql &= String.Format("{0}Declare @Mindest_Abzug money = ", vbNewLine)
			Sql &= String.Format("(Select MAX(ISNULL(Mindest_Abzug, 0)) FROM {0} GROUP BY Mindest_Abzug)  {1}", oldTableName, vbNewLine) ' mindest_abzug
		End If


		r.sqlstring = Sql

		Return r

	End Function

	Private Function CreateFinalProcedureString(ByVal line As String) As QSTData
		Dim r As New QSTData
		Dim Sql As String = String.Empty
		Dim jahr As String = m_Year

		r.jahr = jahr
		r.recordart = Mid(line, 1, 2)				 ' Recordart

		Dim tableName As String = String.Empty

		If r.recordart = "00" Then

		ElseIf r.recordart = "99" Then

			r.kanton = Mid(line, 18, 2)              ' Kanton
			r.einkommen = Mid(line, 20, 8)           ' Anzahl Recs
			tableName = String.Format("[Tar{0}{1}] ", r.jahr, r.kanton)

			Sql = String.Format("Begin Try Drop PROCEDURE [dbo].[Get QSTTarife For {0}{1}] End Try Begin Catch End Catch; {2}", r.jahr, r.kanton, vbNewLine)
			Sql &= String.Format("Go {0}", vbNewLine)
			Sql &= String.Format("CREATE PROCEDURE [dbo].[Get QSTTarife For {0}{1}] {2}", r.jahr, r.kanton, vbNewLine)
			Sql &= String.Format("@MANr int, {0}", vbNewLine)
			Sql &= String.Format("@Einkommen money, {0}", vbNewLine)
			Sql &= String.Format("@Kirchensteuer nvarchar(1) = '', {0}", vbNewLine)
			Sql &= String.Format("@Gruppe nvarchar(5) = '', {0}", vbNewLine)
			Sql &= String.Format("@Geschlecht nvarchar(1) = '', {0}", vbNewLine)
			Sql &= String.Format("@Kinder smallint = 0 {0}", vbNewLine)
			Sql &= String.Format("As {0}", vbNewLine)

			Sql &= String.Format("if @Gruppe <> 'L' AND @Gruppe <> 'M' AND  @Gruppe <> 'N' AND  @Gruppe <> 'P' {0}", vbNewLine)
			Sql &= String.Format("begin {0}", vbNewLine)

			Sql &= String.Format("if @Gruppe = 'D' OR @Gruppe = 'E' {0}", vbNewLine)

			Sql &= String.Format("begin {0}", vbNewLine)

			Sql &= String.Format("Select  Top 5 * From {0} ", tableName)
			Sql &= "Where (Einkommen > @Einkommen - Schritt) "
			Sql &= "And Gruppe = @Gruppe And Kinder = @Kinder "
			Sql &= "And (Kirchensteuer = @Kirchensteuer Or "
			Sql &= "Kirchensteuer is null Or Kirchensteuer = '') "
			Sql &= "And (G = @Geschlecht Or G is null Or G = '') And Recordart <> 11 "
			Sql &= String.Format("Order By Kirchensteuer Desc, G Desc, Einkommen ASC {0}", vbNewLine)

			Sql &= String.Format("End {0}", vbNewLine)

			Sql &= String.Format("Else {0}", vbNewLine)

			Sql &= String.Format("begin {0}", vbNewLine)

			Sql &= String.Format("Select  Top 5 * From {0} ", tableName)
			Sql &= "Where (Einkommen > @Einkommen - Schritt And Einkommen <> 100) "
			Sql &= "And Gruppe = @Gruppe And Kinder = @Kinder "
			Sql &= "And (Kirchensteuer = @Kirchensteuer Or "
			Sql &= "Kirchensteuer is null Or Kirchensteuer = '') "
			Sql &= "And (G = @Geschlecht Or G is null Or G = '')  And Recordart <> 11 "
			Sql &= String.Format("Order By Kirchensteuer Desc, G Desc, Einkommen ASC {0}", vbNewLine)

			Sql &= String.Format("End {0}", vbNewLine)

			Sql &= String.Format("End {0}", vbNewLine)

			Sql &= String.Format("Else {0}", vbNewLine)

			Sql &= String.Format("begin {0}", vbNewLine)

			Sql &= String.Format("Select  Top 5 * From {0} ", tableName)
			Sql &= "Where (Einkommen > @Einkommen - Schritt And Einkommen + Schritt <> 100) "
			Sql &= "And Gruppe = @Gruppe And Kinder = @Kinder "
			Sql &= "And (Kirchensteuer = @Kirchensteuer Or "
			Sql &= "Kirchensteuer is null Or Kirchensteuer = '') "
			Sql &= "And (G = @Geschlecht Or G is null Or G = '')  And Recordart <> 11 "
			Sql &= String.Format("Order By Kirchensteuer Desc, G Desc, Einkommen ASC {0}", vbNewLine)

			Sql &= String.Format("End {0}", vbNewLine)
			Sql &= String.Format("Go {0}", vbNewLine)

		Else

		End If

		r.sqlstring = Sql


		Return r

	End Function

	Private Sub OnbtnReadWrite_Click(sender As System.Object, e As System.EventArgs) Handles btnReadWrite.Click
		If txt_Path.EditValue Is Nothing OrElse txt_Filepattern.EditValue Is Nothing OrElse Not Directory.Exists(txt_Path.EditValue) Then
			m_UtilityUI.ShowErrorDialog("Keine Datei- bzw. Verzeichnisse-Angaben vorhanden!")

			Return
		End If
		Dim oDir As New System.IO.DirectoryInfo(Directory.Exists(txt_Path.EditValue))
		Dim recordData As New List(Of String)
		Dim result As Boolean = True


		Me.lblInfo.Text = "Bitte warten..."
		btnReadWrite.Enabled = False
		txtSQL.EditValue = String.Empty

		Try
			Dim stpw As New Stopwatch
			Trace.WriteLine(String.Format("{0} Import wurde gestartet: {1}", stpw.Elapsed.ToString(), txt_Path.EditValue))
			stpw.Start()

			If Not txt_Filepattern.EditValue.ToString.StartsWith("*.") Then
				Dim myfile = New FileInfo(Path.Combine(txt_Path.EditValue, txt_Filepattern.EditValue))
				m_Year = String.Format("20{0}", myfile.Name.ToString.Substring(3, 2))
				ReadQSTFile(Path.Combine(txt_Path.EditValue, txt_Filepattern.EditValue), m_Year)

			Else
				Dim oFiles() = IO.Directory.GetFiles(txt_Path.EditValue, txt_Filepattern.EditValue)

				For Each myfile In oFiles
					m_Year = txt_Year.EditValue

					If chkFinalfile.Checked Then
						Dim recordline = AnalyseQSTRecord(myfile, m_Year)
						If Not recordline Is Nothing AndAlso recordline.Count > 0 Then recordData.Add(recordline(0))

					Else
						ReadQSTFile(myfile, m_Year)
					End If

					Me.Refresh()
				Next
				If Not recordData Is Nothing AndAlso recordData.Count > 0 Then
					result = result AndAlso CreateNewSQLQueryFile(recordData, String.Format("tar{0}.SQL", m_Year))
				End If

			End If
			stpw.Stop()
			Trace.WriteLine(String.Format("{0} Import wurde beendet: {1}", stpw.Elapsed.ToString(), txt_Path.EditValue))

			If result Then lblInfo.Text = "Import war erfolgreich." Else lblInfo.Text = "Import war NICHT erfolgreich!!!"


		Catch ex As Exception
			lblInfo.Text = String.Format("Import fehlerhaft!")

			txtSQL.EditValue = String.Format("Import fehlerhaft!<br>{0}", ex.ToString)

		End Try

		btnReadWrite.Enabled = True

	End Sub

	Sub ReadQSTFile(ByVal strFile As String, ByVal jahr As String)

		Dim stpw As New Stopwatch
		Trace.WriteLine(String.Format("{0} Import wurde gestartet: {1}", stpw.Elapsed.ToString(), strFile))

		Dim lineNumber As Integer = 0

		Dim qstdata As QSTData = Nothing
		Dim lqst As New List(Of QSTData)
		Dim myarray As New List(Of String)

		stpw.Start()
		Dim i As Integer = 0

		Using f As System.IO.FileStream = System.IO.File.OpenRead(strFile)
			Using s As System.IO.StreamReader = New System.IO.StreamReader(f)
				While Not s.EndOfStream
					Dim line As String = s.ReadLine
					qstdata = New QSTData

					If Mid(line, 1, 2) = "99" Then

						If chkFinalfile.Checked Then
							qstdata = CreateFinalProcedureString(line)
						Else
							qstdata = CreateQSTString(line, lineNumber > 1000)
						End If

					Else
						If Not chkFinalfile.Checked Then qstdata = CreateQSTString(line, lineNumber > 1000)

					End If

					If Not qstdata Is Nothing AndAlso Not String.IsNullOrWhiteSpace(qstdata.sqlstring) Then myarray.Add(qstdata.sqlstring)

					If lineNumber > 1000 Then
						lineNumber = 0
					End If
					lineNumber += 1

				End While
			End Using
		End Using

		Dim result = CreateNewSQLQueryFile(myarray, strFile)

	End Sub

	Private Function AnalyseQSTRecord(ByVal strFile As String, ByVal jahr As String) As List(Of String)
		Dim result As New List(Of String)

		Dim stpw As New Stopwatch
		Trace.WriteLine(String.Format("{0} Import wurde gestartet: {1}", stpw.Elapsed.ToString(), strFile))

		Dim lineNumber As Integer = 0

		Dim qstdata As QSTData = Nothing
		Dim lqst As New List(Of QSTData)

		stpw.Start()
		Dim i As Integer = 0


		Dim lastLine As String = File.ReadLines(strFile).Last()
		If Mid(lastLine, 1, 2) = "99" Then
			If chkFinalfile.Checked Then qstdata = CreateFinalProcedureString(lastLine)
			result.Add(qstdata.sqlstring)
		End If

		Return result
	End Function


	Private Function CreateNewSQLQueryFile(ByVal data As List(Of String), ByVal queryFilename As String) As Boolean
		Dim result As Boolean = True
		Dim newPath As String = Path.Combine(txt_Path.EditValue, "Quellensteuer", m_Year)
		Dim newfilename As String

		If chkFinalfile.Checked Then
			newfilename = String.Format("{0}_Final_.sql", Path.GetFileNameWithoutExtension(queryFilename))
		Else
			newfilename = String.Format("{0}_.sql", Path.GetFileNameWithoutExtension(queryFilename))
		End If

		Dim FileName As String = Path.Combine(newPath, newfilename)

		Try
			If Not Directory.Exists(newPath) Then Directory.CreateDirectory(newPath)
			If File.Exists(FileName) Then File.Delete(FileName)

		Catch ex As Exception
			m_UtilityUI.ShowOKDialog(Me, ex.ToString, "CreateNewSQLQueryFile", MessageBoxIcon.Error)

			Return False
		End Try

		IO.File.WriteAllLines(FileName, data)
		txtSQL.EditValue &= If(String.IsNullOrWhiteSpace(txtSQL.EditValue), "", vbNewLine) & String.Format("{0}", queryFilename)


		Return result
	End Function


#Region "Helpers"

	Private Sub CreateSQLFile(ByVal qstdata As QSTData)
		Dim sql As String

		sql = String.Format("Insert Into [Tar{0}{1}] ", qstdata.kanton, qstdata.jahr)
		sql &= "(RecordArt, "
		sql &= "Art, "
		sql &= "Kanton, "
		sql &= "Gruppe, "
		sql &= "Kinder, "
		sql &= "Kirchensteuer, "
		sql &= "GA, "
		sql &= "Datum, "
		sql &= "Einkommen, "
		sql &= "Schritt, "
		sql &= "G, "
		sql &= "Kinder2, "
		sql &= "Steuer_Fr, "
		sql &= "Steuer_Proz, "
		sql &= "CreatedOn "
		sql &= ") Values ("

		sql &= Val(qstdata.recordart) & ", "            ' RecordArt
		sql &= Val(qstdata.transaktion) & ", '"           ' Transaktion
		sql &= Trim(qstdata.kanton) & "', '"         ' Kanton
		sql &= Trim(qstdata.gruppe) & "', "          ' Gruppe
		sql &= Val(qstdata.kinder) & ", '"          ' Kinder
		sql &= Trim(qstdata.kirchensteuer) & "', '"        ' Kirchensteuer
		sql &= Trim(qstdata.ga) & "', '"        ' GA
		sql &= Trim(qstdata.datum) & "', "         ' Datum
		sql &= Val(qstdata.einkommen) & ", "           ' Einkommen
		sql &= Val(qstdata.schritt) & ", '"          ' Schritt
		sql &= Trim(qstdata.geschlecht) & "', "          ' G
		sql &= Val(qstdata.kinder2) & ", "               ' Kinder2
		sql &= Val(qstdata.steuer_fr) & ", "             ' Steuer_Fr
		sql &= Val(qstdata.steuer_proz) & ", "           ' Steuer_Proz
		sql &= "GetDate() "                              ' Steuer_Proz
		sql &= ")"

		Me.txtSQL.EditValue &= sql

	End Sub

#End Region


End Class


Public Class QSTData

	Public Property recordart As String
	Public Property transaktion As String
	Public Property kanton As String
	Public Property jahr As String
	Public Property tarif As String
	Public Property gruppe As String
	Public Property kinder As String
	Public Property kirchensteuer As String
	Public Property ga As String
	Public Property datum As String
	Public Property einkommen As String
	Public Property schritt As String
	Public Property geschlecht As String
	Public Property kinder2 As String
	Public Property steuer_fr As String
	Public Property steuer_proz As String
	Public Property codestatus As String

	Public Property sqlstring As String

End Class
