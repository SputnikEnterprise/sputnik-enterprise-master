
Imports System.Data.SqlClient
Imports System.IO
Imports SPProposeUtility.ClsDataDetail

Imports DevExpress.XtraRichEdit.Commands
Imports DevExpress.XtraRichEdit.Services
Imports DevExpress.XtraRichEdit
Imports DevExpress.Utils

Imports System.Threading

Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Customer

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages



Module PubFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()


	Private m_Utility As SPProgUtility.MainUtilities.Utilities
	Private m_UtilityUI As SP.Infrastructure.UI.UtilityUI

	Private _ClsDb As New ClsDbFunc

	Private Property strBez2Translate As String


#Region "Kandidatendaten abrufen und Speichern..."

	Function ListDBFieldsName4PZusatz() As List(Of String)
		Dim liResult As New List(Of String)

		Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
			Conn.Open()

			Dim sSql As String = "Select * From tab_LLZusatzFields Where ShowinProposeNavBar = 1 And ShowInMAVersand <> 1 "
			sSql &= "And DBFieldName Like 'p_zusatz%' Order By RecNr, Bezeichnung"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

			While rFoundedrec.Read
				liResult.Add(String.Format("{0}#{1}#{2}", _
																	 ClsDataDetail.GetColumnTextStr(rFoundedrec, "GroupNr", ""), _
																	 Trim(ClsDataDetail.GetColumnTextStr(rFoundedrec, "DBFieldName", "")), _
																	 Trim(ClsDataDetail.GetColumnTextStr(rFoundedrec, "Bezeichnung", ""))))
			End While


		Catch ex As Exception
			liResult.Add("Fehler...")
			m_Logger.LogError(ex.ToString)

		End Try

		Return liResult
	End Function

	Function ListDBFieldsName() As List(Of String)
		Dim liResult As New List(Of String)

		Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
			Conn.Open()

			Dim sSql As String = "Select * From tab_LLZusatzFields Where ShowInMAVersand = 1 And ShowinProposeNavBar = 1 "
			sSql &= "Order By RecNr, Bezeichnung"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

			While rFoundedrec.Read
				liResult.Add(String.Format("{0}#{1}#{2}", _
																	 ClsDataDetail.GetColumnTextStr(rFoundedrec, "GroupNr", ""), _
																	 Trim(ClsDataDetail.GetColumnTextStr(rFoundedrec, "DBFieldName", "")), _
																	 Trim(ClsDataDetail.GetColumnTextStr(rFoundedrec, "Bezeichnung", ""))))
			End While


		Catch ex As Exception
			liResult.Add("Fehler...")
			m_Logger.LogError(ex.ToString)

		End Try

		Return liResult
	End Function

	'Function ListLLTemplateName(ByVal strFieldName As String) As List(Of String)
	'	Dim liResult As New List(Of String)

	'	Try
	'		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'		Conn.Open()

	'		Dim sSql As String = "Select * From tab_LLZusatzFields_Template Where DbFieldName = @DBFieldName "
	'		sSql &= "Order By RecNr, Bezeichnung"

	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'		cmd.CommandType = Data.CommandType.Text
	'		Dim param As System.Data.SqlClient.SqlParameter
	'		param = cmd.Parameters.AddWithValue("@DBFieldName", strFieldName)
	'		Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

	'		While rFoundedrec.Read
	'			liResult.Add(String.Format("{0}#{1}", _
	'																 Trim(ClsDataDetail.GetColumnTextStr(rFoundedrec, "Bezeichnung", "")), _
	'																 Trim(ClsDataDetail.GetColumnTextStr(rFoundedrec, "Filename", ""))))
	'		End While


	'	Catch ex As Exception
	'		liResult.Add("Fehler...")
	'		m_Logger.LogError(ex.ToString)

	'	End Try

	'	Return liResult
	'End Function

	Function GetMALLDbFieldValue(ByVal myDbFieldName As String, ByVal lMANr As Integer) As String
		Dim strResult As String = String.Empty

		Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
			Conn.Open()

			Dim sSql As String = "Select _{0} From MA_LL Where MANr = @MANr"
			sSql = String.Format(sSql, myDbFieldName)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MANr", lMANr)
			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

			While rFoundedrec.Read
				strResult = ClsDataDetail.GetColumnTextStr(rFoundedrec, "_" & myDbFieldName, "")
			End While

		Catch ex As Exception
			strResult = String.Format("Fehler: {0}", ex.InnerException)
			m_Logger.LogError(ex.ToString)

		End Try

		Return strResult
	End Function

	Function SaveMALLDbFieldValue(ByVal myDbFieldName As String, ByVal strRtfValue As String, _
													ByVal strStringValue As String, ByVal lMANr As Integer) As String
		Dim strResult As String = String.Empty

		Try
			myDbFieldName = myDbFieldName.ToLower.Replace("ma_ll_", "")
			Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
			Conn.Open()

			Dim sSql As String = "Update MA_LL Set _{0} = @rtfText, {0} = @MyText Where MANr = @MANr"
			sSql = String.Format(sSql, myDbFieldName)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@rtfText", strRtfValue)
			param = cmd.Parameters.AddWithValue("@MyText", strStringValue)
			param = cmd.Parameters.AddWithValue("@MANr", lMANr)

			cmd.ExecuteNonQuery()

		Catch ex As Exception
			strResult = String.Format("Fehler: {0}", ex.InnerException)
			m_Logger.LogError(ex.ToString)

		End Try

		Return strResult
	End Function

	Function GetMAProposeDbFieldValue(ByVal myDbFieldName As String, ByVal lProposeNr As Integer) As String
		Dim strResult As String = String.Empty

		Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
			Conn.Open()

			Dim sSql As String = "Select _{0} From Propose Where ProposeNr = @ProposeNr"
			sSql = String.Format(sSql, myDbFieldName)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@ProposeNr", lProposeNr)
			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

			While rFoundedrec.Read
				strResult = ClsDataDetail.GetColumnTextStr(rFoundedrec, "_" & myDbFieldName, "")
			End While

		Catch ex As Exception
			strResult = String.Format("Fehler: {0}", ex.InnerException)
			m_Logger.LogError(ex.ToString)

		End Try

		Return strResult
	End Function

	Function SaveMAProposeDbFieldValue(ByVal myDbFieldName As String, ByVal strRtfValue As String, _
														ByVal strStringValue As String, ByVal lProposeNr As Integer) As String
		Dim strResult As String = String.Empty

		Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
			Conn.Open()

			Dim sSql As String = "Update Propose Set _{0} = @rtfText, {0} = @MyText Where ProposeNr = @ProposeNr"
			sSql = String.Format(sSql, myDbFieldName)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@rtfText", strRtfValue)
			param = cmd.Parameters.AddWithValue("@MyText", strStringValue)
			param = cmd.Parameters.AddWithValue("@ProposeNr", lProposeNr)

			cmd.ExecuteNonQuery()

		Catch ex As Exception
			strResult = String.Format("Fehler: {0}", ex.InnerException)
			m_Logger.LogError(ex.ToString)

		End Try

		Return strResult
	End Function

#End Region



	'Sub GetUSData(ByVal myRegx As ClsDivFunc)
	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
	'	Dim strUSKst As String = String.Empty
	'	Dim strUSNachname As String = String.Empty
	'	Dim iUSNr As Integer = 0
	'	Dim Conn As New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Conn.Open()

	'	Dim sSql As String = "[Get USData 4 Templates]"
	'	Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
	'	cmd.CommandType = CommandType.StoredProcedure
	'	Dim param As System.Data.SqlClient.SqlParameter
	'	param = cmd.Parameters.AddWithValue("@USNr", m_InitialData.UserData.UserNr)

	'	Dim rTemprec As SqlDataReader = cmd.ExecuteReader					 ' Benutzerdatenbank

	'	Try
	'		rTemprec.Read()
	'		With _ClsLLFunc
	'			.USAnrede = rTemprec("USAnrede").ToString
	'			.USeMail = rTemprec("USeMail").ToString
	'			.USNachname = rTemprec("USNachname").ToString
	'			.USVorname = rTemprec("USVorname").ToString
	'			.USTelefon = rTemprec("USTelefon").ToString
	'			.USTelefax = rTemprec("USTelefax").ToString

	'			.USTitel_1 = rTemprec("USTitel_1").ToString
	'			.USTitel_2 = rTemprec("USTitel_2").ToString

	'			.USAbteilung = rTemprec("USAbteilung").ToString
	'			.USPostfach = rTemprec("USPostfach").ToString
	'			.USStrasse = rTemprec("USStrasse").ToString
	'			.USPLZ = rTemprec("USPLZ").ToString
	'			.USLand = rTemprec("USLand").ToString
	'			.USOrt = rTemprec("USOrt").ToString

	'			.Exchange_USName = rTemprec("EMail_UserName").ToString
	'			.Exchange_USPW = rTemprec("EMail_UserPW").ToString

	'			.USMDname = rTemprec("MDName").ToString
	'			.USMDname2 = rTemprec("MDName2").ToString
	'			.USMDname3 = rTemprec("MDName3").ToString
	'			.USMDPostfach = rTemprec("MDPostfach").ToString
	'			.USMDStrasse = rTemprec("MDStrasse").ToString
	'			.USMDPlz = rTemprec("MDPLZ").ToString
	'			.USMDOrt = rTemprec("MDOrt").ToString
	'			.USMDLand = rTemprec("MDLand").ToString

	'			.USMDTelefon = rTemprec("MDTelefon").ToString
	'			.USMDTelefax = rTemprec("MDTelefax").ToString
	'			.USMDeMail = rTemprec("MDeMail").ToString
	'			.USMDHomepage = rTemprec("MDHomepage").ToString

	'			.strUSSignFilename = GetUSSign(m_InitialData.UserData.UserNr)
	'		End With
	'		rTemprec.Close()


	'	Catch ex As Exception
	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

	'	Finally
	'		rTemprec.Close()
	'		Conn.Close()

	'	End Try

	'End Sub

	Function GetUSSign(ByVal lUSNr As Integer) As String
		Dim Conn As New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

		Dim BA As Byte()
		Dim sUSSql As String = "Select USSign, USNr From Benutzer US Where "
		sUSSql &= String.Format("USNr = {0} And USSign Is Not Null", lUSNr)

		Dim i As Integer = 0

		Conn.Open()
		'Dim SQLCmd As SqlCommand = New SqlCommand(sUSSql, Conn)
		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)

		Try

			strFullFilename = String.Format("{0}Bild_{1}.JPG", m_path.GetSpSPictureHomeFolder, _
																			 System.Guid.NewGuid.ToString())

			Try
				Try
					BA = CType(SQLCmd_1.ExecuteScalar, Byte())
					If BA Is Nothing Then Return String.Empty

				Catch ex As Exception
					Return String.Empty

				End Try

				Dim ArraySize As New Integer
				ArraySize = BA.GetUpperBound(0)

				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
				Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
				fs.Write(BA, 0, ArraySize + 1)
				fs.Close()
				fs.Dispose()
				ClsDataDetail.strUSSignFilename = strFullFilename

				i += 1


			Catch ex As Exception
				m_Logger.LogError(String.Format("***GetUSSign: {0}", ex.Message))
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Fehler: {0}"), ex.ToString))


			End Try


		Catch ex As Exception
			m_Logger.LogError(String.Format("***GetUSSign: {0}", ex.Message))

		End Try

		Return strFullFilename
	End Function



	'#Region "Funktionen zum Kontakt erfassen..."

	'Sub CreateLogToKontaktDb(ByVal lKDNr As Integer, ByVal lZHDNr As Integer?, ByVal iMANr As Integer, _
	'													 ByVal iProposeNr As Integer, ByVal iVakNr As Integer?, _
	'													 ByVal strProposeBez As String)
	'	Dim Time_1 As Double = System.Environment.TickCount
	'	Dim strUSName As String = m_InitialData.UserData.UserFullName
	'	Dim Conn As New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim strTableName As String = String.Format("{0}", If(iMANr = 0, "KD_KontaktTotal", "MA_Kontakte"))
	'	Dim lNewRecNr As Integer
	'	Dim sKDZSql As String = String.Format("Insert Into {0} (", strTableName)
	'	sKDZSql &= String.Format("{0}, ", If(iMANr > 0, "MANr", "KDNr, KDZNr"))
	'	sKDZSql &= "RecNr, KontaktDate, Kontakte, "
	'	sKDZSql &= "KontaktType1, KontaktType2, Kontaktwichtig, "
	'	sKDZSql &= "KontaktDauer, KontaktErledigt, "
	'	sKDZSql &= String.Format("{0} ProposeNr, VakNr, OfNr, ", If(iMANr > 0, "", "MANr,"))
	'	sKDZSql &= "CreatedOn, CreatedFrom) Values ("

	'	sKDZSql &= String.Format("{0}, ", If(iMANr > 0, "@MANr", "@KDNr, @ZHDNr"))
	'	sKDZSql &= "@RecNr, @KontaktDate, @Beschreibung, "
	'	sKDZSql &= "'Information', 2, 0, "
	'	sKDZSql &= "@Betreff, 0, "
	'	sKDZSql &= String.Format("{0} @ProposeNr, @VakNr, 0, @CreatedOn, @USName)", If(iMANr > 0, "", "0, "))

	'	Try
	'		Conn.Open()
	'		lNewRecNr = GetNewKontaktNr(lKDNr, lZHDNr, iMANr)

	'		Dim rKontaktrec As New SqlDataAdapter()

	'		rKontaktrec.SelectCommand = New SqlCommand(sKDZSql, Conn)
	'		If iMANr > 0 Then
	'			rKontaktrec.SelectCommand.Parameters.AddWithValue("@MANr", iMANr)

	'		Else
	'			rKontaktrec.SelectCommand.Parameters.AddWithValue("@KDNr", lKDNr)
	'			rKontaktrec.SelectCommand.Parameters.AddWithValue("@ZHDNr", ReplaceMissing(lZHDNr, DBNull.Value))

	'		End If
	'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@RecNr", lNewRecNr)

	'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@KontaktDate", Now.ToString("G"))
	'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@Beschreibung", _
	'																											String.Format("Neuer Vorschlag als {0} erfasst", _
	'																																		strProposeBez))
	'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@Betreff", _
	'																											String.Format("Neuer Vorschlag als {0} erfasst", _
	'																																		strProposeBez))

	'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@ProposeNr", iProposeNr)
	'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@VakNr", ReplaceMissing(iVakNr, DBNull.Value))

	'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@CreatedOn", Now.ToString("G"))
	'		rKontaktrec.SelectCommand.Parameters.AddWithValue("@USName", strUSName)

	'		Dim dt As DataTable = New DataTable()
	'		rKontaktrec.Fill(dt)

	'	Catch ex As Exception
	'		MsgBox(ex.Message.ToString & vbCrLf & ex.GetBaseException.ToString, _
	'					 MsgBoxStyle.Critical, "CreateLogToKDKontaktDb_0")

	'	End Try

	'	Conn.Close()

	'	Dim Time_2 As Double = System.Environment.TickCount
	'	Console.WriteLine("Zeit für CreateLogToKDKontaktDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

	'End Sub

	'Function GetNewKontaktNr(ByVal lKDNr As Integer, ByVal lKDZNr As Integer?, ByVal iMANr As Integer) As Integer
	'	Dim lRecNr As Integer = 1
	'	Dim strTableName As String = String.Format("{0}", If(iMANr = 0, "KD_KontaktTotal", "MA_Kontakte"))
	'	Dim Conn As New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Conn.Open()

	'	Dim sSql As String = String.Format("Select Top 1 ID From {0} Order By ID Desc", strTableName)
	'	Try
	'		Dim SQLOffCmd As SqlCommand = New SqlCommand(sSql, Conn)
	'		Dim rTemprec As SqlDataReader = SQLOffCmd.ExecuteReader

	'		rTemprec.Read()
	'		If rTemprec.HasRows Then
	'			lRecNr = CInt(rTemprec("ID").ToString) + 1
	'		Else
	'			lRecNr = 1
	'		End If
	'		rTemprec.Close()

	'	Catch ex As Exception
	'		m_UtilityUI.ShowErrorDialog(String.Format("Error: {1}{0}{2}", vbNewLine, ex.Message.ToString, ex.GetBaseException.ToString))

	'	Finally
	'		Conn.Close()

	'	End Try

	'	Return lRecNr
	'End Function


	'#End Region


	'#Region "Funktionen zur Komprimierung..."

	' Datei komprimieren (GZIP)
	'Public Function ZipFile(ByVal sFile As String, _
	'                        Optional ByVal sZipFile As String = "") As Boolean
	'  Dim zipPath As String = "C:\Path\Compression\myzip.zip"
	'  Dim fileToAdd As String = "C:\Path\Compression\Compress Me.txt"

	'  'Open the zip file if it exists, else create a new one 
	'  Dim zip As Package = ZipPackage.Open(zipPath, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)

	'  'Replace spaces with an underscore (_) 
	'  Dim uriFileName As String = fileToAdd.Replace(" ", "_")

	'  'A Uri always starts with a forward slash "/" 
	'  Dim zipUri As String = String.Concat("/", IO.Path.GetFileName(uriFileName))

	'  Dim partUri As New Uri(zipUri, UriKind.Relative)
	'  Dim contentType As String = Net.Mime.MediaTypeNames.Application.Zip

	'  'The PackagePart contains the information: 
	'  ' Where to extract the file when it's extracted (partUri) 
	'  ' The type of content stream (MIME type) - (contentType) 
	'  ' The type of compression to use (CompressionOption.Normal) 
	'  Dim pkgPart As PackagePart = zip.CreatePart(partUri, contentType, CompressionOption.Normal)

	'  'Read all of the bytes from the file to add to the zip file 
	'  Dim bites As Byte() = File.ReadAllBytes(fileToAdd)

	'  'Compress and write the bytes to the zip file 
	'  pkgPart.GetStream().Write(bites, 0, bites.Length)
	'  zip.Close() 'Close the zip file

	'End Function

	'Function MakeZipWithFiles(ByVal strZipFilename As String, ByVal liFiles2Zip As List(Of String)) As String
	'  Dim strResult As String = "Erfolgreich..."

	'  Try
	'    Try
	'      If IO.File.Exists(strZipFilename) Then Kill(strZipFilename)
	'    Catch ex As Exception

	'    End Try
	'    'Open the zip file if it exists, else create a new one 
	'    Dim zip As Package = ZipPackage.Open(strZipFilename, IO.FileMode.OpenOrCreate, IO.FileAccess.ReadWrite)

	'    'Add as many files as you like:
	'    For i As Integer = 0 To liFiles2Zip.Count - 1
	'      AddToArchive(zip, liFiles2Zip(i).ToString)
	'    Next
	'    zip.Close()

	'  Catch ex As Exception
	'    strResult = String.Format("Fehler (MakeZipWithFiles): {0}", ex.Message)

	'  End Try

	'  Return strResult
	'End Function

	'Private Sub AddToArchive(ByVal zip As Package, ByVal strfileToAdd As String)
	'  'Replace spaces with an underscore (_) 
	'  Dim uriFileName As String = strfileToAdd.Replace(" ", "_").Replace(",", "_")

	'  'A Uri always starts with a forward slash "/" 
	'  Dim zipUri As String = String.Concat("/", IO.Path.GetFileName(uriFileName))

	'  Dim partUri As New Uri(zipUri, UriKind.Relative)
	'  Dim contentType As String = Net.Mime.MediaTypeNames.Application.Zip

	'  'The PackagePart contains the information: 
	'  ' Where to extract the file when it's extracted (partUri) 
	'  ' The type of content stream (MIME type):  (contentType) 
	'  ' The type of compression:  (CompressionOption.Normal)   
	'  Dim pkgPart As PackagePart = zip.CreatePart(partUri, contentType, CompressionOption.Maximum)

	'  'Read all of the bytes from the file to add to the zip file 
	'  Dim bites As Byte() = File.ReadAllBytes(strfileToAdd)

	'  'Compress and write the bytes to the zip file 
	'  pkgPart.GetStream().Write(bites, 0, bites.Length)

	'End Sub

	'#End Region

	'#Region "Funktionen zur Übersetzung der Daten..."


	'	Private Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
	'		target = value

	'		Return value
	'	End Function


	'#End Region

End Module

Namespace CustomCommand

#Region "customsavecommand"

	Public Class CustomSaveDocumentCommand
		Inherits SaveDocumentCommand

		Public Sub New(ByVal control As IRichEditControl)
			MyBase.New(control)
		End Sub

		Protected Overrides Sub ExecuteCore()

			' Nichts tun. Die Daten wurden bereits in der Datenbank gespeichert.

			'SaveLLText("Reserve0", ClsDataDetail.GetSelectedTemplateName, Control)
			'ClsDataDetail.bContentChanged = False

			'If Control.Document.Paragraphs.Count > 7 Then
			'MyBase.ExecuteCore()

			'Else
			'MessageBox.Show("You should type at least 7 paragraphs" & ControlChars.CrLf & "  to be able to save the document.", _
			'                "Please be creative", _
			'                MessageBoxButtons.OK, _
			'                MessageBoxIcon.Information)
			'End If

		End Sub

	End Class

	Public Class CustomPrintDocumentCommand
		Inherits QuickPrintCommand

		Public Sub New(ByVal control As IRichEditControl)
			MyBase.New(control)
		End Sub

		Protected Overrides Sub ExecuteCore()

			'MyBase.ExecuteCore()
			'LoadOrgDoc(Control)
			'ClsDataDetail.bContentChanged = False

			'If Control.Document.Paragraphs.Count > 7 Then
			'  MyBase.ExecuteCore()
			'Else
			'  MessageBox.Show("You should type at least 7 paragraphs" & ControlChars.CrLf & "  to be able to save the document.", _
			'                  "Please be creative", _
			'                  MessageBoxButtons.OK, _
			'                  MessageBoxIcon.Information)
			'End If

		End Sub

	End Class

#End Region	' customsavecommand


#Region "#iricheditcommandfactoryservice"

	Public Class CustomRichEditCommandFactoryService
		Implements IRichEditCommandFactoryService
		Private ReadOnly service As IRichEditCommandFactoryService
		Private ReadOnly control As RichEditControl

		Public Sub New(ByVal control As RichEditControl, ByVal service As IRichEditCommandFactoryService)
			Guard.ArgumentNotNull(control, "control")
			Guard.ArgumentNotNull(service, "service")
			Me.control = control
			Me.service = service
		End Sub

#Region "IRichEditCommandFactoryService Members"

		Public Function CreateCommand(ByVal id As RichEditCommandId) As RichEditCommand _
		Implements IRichEditCommandFactoryService.CreateCommand
			If id = RichEditCommandId.FileSave Then
				Return New CustomSaveDocumentCommand(control)
			ElseIf id = RichEditCommandId.QuickPrint Then
				Return New CustomPrintDocumentCommand(control)
			ElseIf id = RichEditCommandId.Print Then
				Return New CustomPrintDocumentCommand(control)

			End If

			Return service.CreateCommand(id)
		End Function

#End Region

	End Class

#End Region


End Namespace
