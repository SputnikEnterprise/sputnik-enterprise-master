
Imports System.Data.SqlClient
Imports System.IO

Imports DevExpress.XtraRichEdit.Commands

Imports DevExpress.XtraRichEdit.Services
Imports DevExpress.XtraRichEdit
Imports DevExpress.Utils
Imports DevExpress.Services

Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports DevExpress.XtraSplashScreen

Module PubFunc


	' Private m_UtilityUI As UtilityUI
	'Private m_Utility As SP.Infrastructure.Utility
	Private m_Logger As ILogger = New Logger()

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsLog As New SPProgUtility.ClsEventLog




	'Function TranslateMyText(ByVal strBez As String) As String
	'	Dim strOrgText As String = strBez
	'	Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez, "SPMALLUtility")

	'	Return strTranslatedText
	'End Function

	'Function TranslateMyText(ByVal strFuncName As String, _
	'												 ByVal strOrgControlBez As String, _
	'												 ByVal strBez As String) As String
	'	Dim strOrgText As String = strBez
	'	Dim strTranslatedText As String = _ClsProgSetting.TranslateText(strBez)
	'	Dim _clsLog As New SPProgUtility.ClsEventLog

	'	Return strTranslatedText
	'End Function



#Region "Menübars individuell ausführen..."


	'Function GetFileToByte(ByVal filePath As String) As Byte()
	'	Dim stream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
	'	Dim reader As BinaryReader = New BinaryReader(stream)

	'	Dim photo() As Byte = Nothing
	'	Try

	'		photo = reader.ReadBytes(CInt(stream.Length))
	'		reader.Close()
	'		stream.Close()

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)

	'	End Try

	'	Return photo
	'End Function

#End Region

End Module



Public Class LoadData


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_UtilityUI As UtilityUI
	Private m_Utility As SP.Infrastructure.Utility
	Private m_Logger As ILogger = New Logger()
	Private m_UtilityProg As SPProgUtility.MainUtilities.Utilities

	Private _ClsProgSetting As SPProgUtility.ClsProgSettingPath


#Region "Constructor"


	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

		m_UtilityUI = New UtilityUI
		m_Utility = New SP.Infrastructure.Utility
		m_UtilityProg = New SPProgUtility.MainUtilities.Utilities
		_ClsProgSetting = New SPProgUtility.ClsProgSettingPath

	End Sub


#End Region


#Region "private property"

	Private ReadOnly Property GetLLExtentionValue() As String
		Get
			Return _ClsProgSetting.GetMDProfilValue("Sonstiges", "LLTemplateExtension", "DOC")
		End Get
	End Property

	Private ReadOnly Property GetLLDocFormat() As DevExpress.XtraRichEdit.DocumentFormat
		Get
			Dim strValue As String = GetLLExtentionValue()

			Select Case strValue.ToLower
				Case "doc"
					Return DevExpress.XtraRichEdit.DocumentFormat.Doc

				Case "odt"
					Return DevExpress.XtraRichEdit.DocumentFormat.OpenDocument

				Case "rtf"
					Return DevExpress.XtraRichEdit.DocumentFormat.Rtf

				Case "txt"
					Return DevExpress.XtraRichEdit.DocumentFormat.PlainText

				Case Else
					Return DevExpress.XtraRichEdit.DocumentFormat.Doc

			End Select

		End Get
	End Property

#End Region



	Function ListLLTemplateName(ByVal strFieldName As String) As List(Of String)
		Dim liResult As New List(Of String)

		Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
			Conn.Open()

			Dim sSql As String = "Select * From tab_LLZusatzFields_Template Where DbFieldName = @DBFieldName "
			sSql &= "Order By RecNr, Bezeichnung"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@DBFieldName", strFieldName)
			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

			While rFoundedrec.Read
				liResult.Add(String.Format("{0}#{1}", m_Translate.GetSafeTranslationValue(m_UtilityProg.SafeGetString(rFoundedrec, "Bezeichnung")),
																														 m_UtilityProg.SafeGetString(rFoundedrec, "Filename")))
			End While


		Catch ex As Exception

		End Try

		Return liResult
	End Function

	Function CreateMAScanDocFile(ByVal cLstDocName As DevExpress.XtraEditors.CheckedListBoxControl, ByVal iMANr As Integer) As String
		Dim strResult As String = String.Empty
		Dim strDocBez As String = String.Empty
		Dim strFilePath As String = _ClsProgSetting.GetSpSMATempPath
		Dim strFullFilename As String = String.Empty

		For i As Integer = 1 To cLstDocName.Items.Count - 2
			If cLstDocName.Items(i).CheckState = CheckState.Checked Then
				strDocBez &= If(strDocBez.Length = 0, "'", "', '") & cLstDocName.Items(i).Value.ToString
			End If
		Next
		If strDocBez = String.Empty Then Return strResult
		If Not Directory.Exists(strFilePath) Then Directory.CreateDirectory(strFilePath)
		Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
			Conn.Open()

			Dim sSql As String = "Select [ID], DocScan, ScanExtension From MA_LLDoc Where MANr = @MANr "
			sSql &= String.Format("And Bezeichnung In ({0}')", strDocBez)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MANr", iMANr)
			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
			Dim aDocFiles As New List(Of String)

			While rFoundedrec.Read
				Try
					Dim BA As Byte()
					BA = CType(rFoundedrec("DocScan"), Byte())

					Dim ArraySize As New Integer
					ArraySize = BA.GetUpperBound(0)
					strFullFilename = strFilePath & rFoundedrec("ID").ToString & _
															If(rFoundedrec("ScanExtension").ToString.Contains("."), _
																 rFoundedrec("ScanExtension").ToString, _
																 "." & rFoundedrec("ScanExtension").ToString)

					If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
					Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
					fs.Write(BA, 0, ArraySize + 1)
					fs.Close()
					fs.Dispose()

					aDocFiles.Add(strFullFilename)

				Catch ex As Exception
					m_Logger.LogError(ex.ToString)
					m_UtilityUI.ShowErrorDialog(String.Format("Fehler: {0}", ex.ToString))

				End Try
			End While

			strResult = aDocFiles(0)
			If aDocFiles.Count > 1 Then
				Dim obj As New SP.PDFO2S.ClsPDF4Net
				obj.Merg2PDFFiles(strFilePath & "MADoc.PDF", aDocFiles.ToArray)
				strResult = strFilePath & "MADoc.PDF"
			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			m_UtilityUI.ShowErrorDialog(String.Format("Fehler: {0}", ex.ToString))

		Finally

		End Try

		Return strResult
	End Function


	Function LoadEmployeeData(ByVal employeeNumber As Integer) As EmployeeMaskData
		Dim result As EmployeeMaskData = Nothing

		Try
			Dim sql As String

			sql = "Select Top 1 MA.Nachname, MA.Vorname, MA.Strasse, MA.PLZ, MA.Ort, MA.Land, MA.Telefon_P, MA.Natel, "
			sql &= "MA.eMail, MA.GebDat, MA.Zivilstand, MA.Bild, MA.Beruf, MA.GebOrt, MA.Bewillig, MA.Geschlecht, "
			sql &= "IsNull((Select Top 1 Land From LND L Where MA.Nationality = L.Code), '') As NationalityBez, "
			sql &= "MA.Nationality, "
			sql &= "MAS.Fahrzeug, "
			sql &= "IsNull((Select Top 1 Description From Tab_Fahrzeug t Where MAS.Fahrzeug = t.GetFeld), '') As Fahrzeug_Bez, "
			sql &= "MAS.F_Schein1, "
			sql &= "IsNull((Select Top 1 Description From Tab_F_Schein t Where MAS.F_Schein1 = t.GetFeld), '') As F_Schein_1_Bez, "
			sql &= "MAS.F_Schein2,"
			sql &= "IsNull((Select Top 1 Description From Tab_F_Schein t Where MAS.F_Schein2 = t.GetFeld), '') As F_Schein_2_Bez, "
			sql &= "MAS.F_Schein3,"
			sql &= "IsNull((Select Top 1 Description From Tab_F_Schein t Where MAS.F_Schein3 = t.GetFeld), '') As F_Schein_3_Bez, "
			sql &= "IsNull((LND.Land), '') As LandBez "
			sql &= "From Mitarbeiter MA Left Join MASonstiges MAS On MA.MANr = MAS.MANr "
			sql &= "Left Join LND LND On MA.Nationality = LND.Code "
			sql &= "Where MA.MANr = @MANr"


			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))
			Dim reader As SqlClient.SqlDataReader = m_UtilityProg.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams)


			If (Not reader Is Nothing) Then

				result = New EmployeeMaskData

				While reader.Read

					Dim data = New EmployeeMaskData()

					data.Geschlecht = m_UtilityProg.SafeGetString(reader, "Geschlecht")
					data.MANachname = m_UtilityProg.SafeGetString(reader, "Nachname")
					data.MAVorname = m_UtilityProg.SafeGetString(reader, "Vorname")
					data.MAStrasse = m_UtilityProg.SafeGetString(reader, "Strasse")
					data.MAPLZ = m_UtilityProg.SafeGetString(reader, "PLZ")
					data.MAOrt = m_UtilityProg.SafeGetString(reader, "Ort")
					data.MATelefon = m_UtilityProg.SafeGetString(reader, "Telefon_P")
					data.MANatel = m_UtilityProg.SafeGetString(reader, "Natel")
					data.MAEMail = m_UtilityProg.SafeGetString(reader, "eMail")
					data.MAGebDat = m_UtilityProg.SafeGetDateTime(reader, "GebDat", Nothing)
					data.MAHBeruf = m_UtilityProg.SafeGetString(reader, "Beruf")
					data.MAHeimatort = m_UtilityProg.SafeGetString(reader, "GebOrt")
          data.MANationality = m_UtilityProg.SafeGetString(reader, "Nationality")
					data.MANationalityLabel = m_UtilityProg.SafeGetString(reader, "NationalityBez")
					data.MACountryName = m_UtilityProg.SafeGetString(reader, "LandBez")
					data.MAMobility_Bez = m_UtilityProg.SafeGetString(reader, "Fahrzeug_Bez")
					data.MAFahrzeug = m_UtilityProg.SafeGetString(reader, "Fahrzeug")

					data.F_Schein_1_Bez = m_UtilityProg.SafeGetString(reader, "F_Schein_1_Bez")
					data.F_Schein_2_Bez = m_UtilityProg.SafeGetString(reader, "F_Schein_2_Bez")
					data.F_Schein_3_Bez = m_UtilityProg.SafeGetString(reader, "F_Schein_3_Bez")
					data.F_Schein1 = m_UtilityProg.SafeGetString(reader, "F_Schein1")
					data.F_Schein2 = m_UtilityProg.SafeGetString(reader, "F_Schein2")
					data.F_Schein3 = m_UtilityProg.SafeGetString(reader, "F_Schein3")
					data.MAZivilstand = m_UtilityProg.SafeGetString(reader, "Zivilstand")
					data.MABewillig = m_UtilityProg.SafeGetString(reader, "Bewillig")


					result = data

				End While

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return Nothing

		End Try

		Return result
	End Function

	Function LoadAdvisorMaskData(ByVal advisorNumber As Integer) As AdvisorMaskData
		Dim result As AdvisorMaskData = Nothing

		Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
			Conn.Open()

			Dim sql As String
			sql = "[Get USData 4 Templates]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("USNr", advisorNumber))
			Dim reader As SqlClient.SqlDataReader = m_UtilityProg.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams, CommandType.StoredProcedure)


			If (Not reader Is Nothing) Then

				result = New AdvisorMaskData

				While reader.Read

					Dim data = New AdvisorMaskData()

					data.USFName = m_UtilityProg.SafeGetString(reader, "USVorname")
					data.USLName = m_UtilityProg.SafeGetString(reader, "USNachname")
					data.USAnrede = m_UtilityProg.SafeGetString(reader, "USAnrede")
					data.USTelefon = m_UtilityProg.SafeGetString(reader, "USTelefon")
					data.USTelefax = m_UtilityProg.SafeGetString(reader, "USTelefax")
					data.USEmail = m_UtilityProg.SafeGetString(reader, "USeMail")
					data.USTelefon = m_UtilityProg.SafeGetString(reader, "USTelefon")
					data.USTelefax = m_UtilityProg.SafeGetString(reader, "USTelefax")
					data.USEmail = m_UtilityProg.SafeGetString(reader, "USeMail")
					data.USTitel_1 = m_UtilityProg.SafeGetString(reader, "USTitel_1")
					data.USTitel_2 = m_UtilityProg.SafeGetString(reader, "USTitel_2")
					data.USMDName = m_UtilityProg.SafeGetString(reader, "MDName")
					data.USMDName2 = m_UtilityProg.SafeGetString(reader, "MDName2")
					data.USMDName3 = m_UtilityProg.SafeGetString(reader, "MDName3")

					data.USMDPostfach = m_UtilityProg.SafeGetString(reader, "MDPostfach")
					data.USMDStrasse = m_UtilityProg.SafeGetString(reader, "MDStrasse")
					data.USMDPLZ = m_UtilityProg.SafeGetString(reader, "MDPLZ")
					data.USMDOrt = m_UtilityProg.SafeGetString(reader, "MDOrt")
					data.USMDLand = m_UtilityProg.SafeGetString(reader, "MDLand")
					data.USMDTelefon = m_UtilityProg.SafeGetString(reader, "MDTelefon")
					data.USMDTelefax = m_UtilityProg.SafeGetString(reader, "MDTelefax")
					data.USMDEMail = m_UtilityProg.SafeGetString(reader, "MDeMail")

					data.USMDHomepage = m_UtilityProg.SafeGetString(reader, "MDHomepage")


					result = data

				End While

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return Nothing

		End Try

		Return result
	End Function


	Public Function GetBewBez(ByVal myBez As String) As String
		Dim strResult As String = myBez

		Try
			Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
			Conn.Open()

			Dim sSql As String = "Select Description "
			sSql &= "From TAB_Bewillig Where GetFeld = @Bez"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@Bez", myBez)
			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

			While rFoundedrec.Read
				strResult = rFoundedrec("Description").ToString
			End While

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

		End Try

		Return strResult
	End Function

	'Public Function GetZivilBez(ByVal myBez As String, ByVal lang As String) As String
	'	Dim strResult As String = myBez

	'	Try
	'		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
	'		Conn.Open()
	'		' TODO
	'		Dim sSql As String = "Select Description "
	'		sSql &= "From TAB_Zivilstand Where GetFeld = @Bez"

	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
	'		cmd.CommandType = Data.CommandType.Text
	'		Dim param As System.Data.SqlClient.SqlParameter
	'		param = cmd.Parameters.AddWithValue("@Bez", myBez)
	'		Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

	'		While rFoundedrec.Read
	'			strResult = rFoundedrec("Description").ToString
	'		End While

	'	Catch ex As Exception
	'		m_Logger.LogError(ex.ToString)

	'	End Try

	'	Return strResult
	'End Function

	Public Function LoadEmployeeChildData(ByVal employeeNumber As Integer) As List(Of EmployeeChildMaskData)
		Dim result As List(Of EmployeeChildMaskData) = Nothing

		Try
			Dim sql As String
			sql = "Select * From MA_KIAddress Where MANr = @MANr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))
			Dim reader As SqlClient.SqlDataReader = m_UtilityProg.OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams)


			If (Not reader Is Nothing) Then

				result = New List(Of EmployeeChildMaskData)

				While reader.Read

					Dim data = New EmployeeChildMaskData()

					data.Geschlecht = m_UtilityProg.SafeGetString(reader, "Geschlecht")
					data.MANachname = m_UtilityProg.SafeGetString(reader, "Nachname")
					data.MAVorname = m_UtilityProg.SafeGetString(reader, "Vorname")
					data.GebDate = m_UtilityProg.SafeGetDateTime(reader, "GebDat", Nothing)


					result.Add(data)

				End While

			End If


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			result = Nothing

		End Try

		Return result
	End Function


	Function LoadAdvisorSign(ByVal lUSNr As Integer) As String
		Dim Conn As New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte()
		Dim sUSSql As String = "Select USSign, USNr From Benutzer US Where "
		sUSSql &= String.Format("USNr = {0} And USSign Is Not Null", lUSNr)

		Dim i As Integer = 0

		Conn.Open()
		Dim SQLCmd As SqlCommand = New SqlCommand(sUSSql, Conn)
		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)

		Try

			strFullFilename = String.Format("{0}Bild_{1}.JPG", _ClsProgSetting.GetSpSBildFiles2DeletePath, _
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

				i += 1

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(String.Format("Fehler: {0}", ex.ToString))
				Return String.Empty

			End Try


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return String.Empty

		End Try

		Return strFullFilename
	End Function

	Function LoadAdvisorPicture(ByVal lUSNr As Integer) As String
		Dim Conn As New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte()
		Dim sUSSql As String = "Select USBild, USNr From Benutzer US Where "
		sUSSql &= String.Format("USNr = {0} And USBild Is Not Null", lUSNr)

		Dim i As Integer = 0

		Conn.Open()
		Dim SQLCmd As SqlCommand = New SqlCommand(sUSSql, Conn)
		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)

		Try

			strFullFilename = String.Format("{0}Bild_{1}.JPG", _ClsProgSetting.GetSpSBildFiles2DeletePath, _
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

				i += 1

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(String.Format("Fehler: {0}", ex.ToString))
				Return String.Empty

			End Try


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return String.Empty

		End Try

		Return strFullFilename
	End Function

	Function LoadEmployeePicture(ByVal lMANr As Integer) As String
		Dim Conn As New SqlConnection(m_InitializationData.MDData.MDDbConn)
		Dim strFullFilename As String = String.Empty
		Dim strFiles As String = String.Empty
		Dim BA As Byte()
		Dim sMASql As String = "Select MABild, MANr From Mitarbeiter Where "
		sMASql &= String.Format("MANr = {0} And MABild Is Not Null", lMANr)

		Dim i As Integer = 0

		Conn.Open()
		Dim SQLCmd As SqlCommand = New SqlCommand(sMASql, Conn)
		Dim SQLCmd_1 As SqlCommand = New SqlCommand(sMASql, Conn)

		Try

			strFullFilename = String.Format("{0}Bild_{1}_{2}.JPG", _ClsProgSetting.GetSpSBildFiles2DeletePath, _
																			 lMANr, System.Guid.NewGuid.ToString())

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

				i += 1

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				m_UtilityUI.ShowErrorDialog(String.Format("Fehler: {0}", ex.ToString))
				Return String.Empty

			End Try


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return String.Empty

		End Try

		Return strFullFilename
	End Function

	Sub SaveLLText(ByVal strFieldName As String, ByVal strTemplateName As String, ByVal iMANr As Integer, ByVal rtfControl As DevExpress.XtraRichEdit.RichEditControl)

		Try
			If String.IsNullOrWhiteSpace(strTemplateName) Then 'OrElse strTemplateName = "Standard" Then
				m_Logger.LogWarning("template was not selected!")
				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Sie haben noch keine Vorlage ausgewählt."))

				Return
			End If

			Dim strFile2Export As String = ClsDataDetail.strFilename4Print_Save
			Dim fileExtension = GetLLExtentionValue

			Dim documentFilename As String = Path.GetTempFileName
			documentFilename = Path.ChangeExtension(documentFilename, fileExtension)
			rtfControl.SaveDocument(documentFilename, GetLLDocFormat)


			If String.IsNullOrWhiteSpace(strFile2Export) Then
				strFile2Export = Path.Combine(_ClsProgSetting.GetSpSFiles2DeletePath, String.Format("Lebenslauf_{0}.pdf", iMANr))
				ClsDataDetail.strFilename4Print_Save = strFile2Export
			End If

			If strFile2Export.ToLower.Contains("fehler") Then Return
			Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

			Dim sql As String
			sql = "If Not Exists(Select ID From MA_Lebenslauf Where LL_Name = @RecName And MANr = @MANr) "
			sql &= "Insert Into MA_Lebenslauf (MANr, LL_Name, CreatedOn, CreatedFrom) Values (@MANr, @RecName, GetDate(), @ChangedFrom) "

			sql &= String.Format("Update MA_Lebenslauf Set _{0} = @MyRTFText, DocumentContent = @DocumentContent, {0} = @MyText, ", strFieldName)
			sql &= "PDFFile = @BinaryFile, ChangedOn = GetDate(), ChangedFrom = @ChangedFrom "
			sql &= "Where MANr = @MANr And (LL_Name = @RecName)"

			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			Dim myFile() As Byte = m_Utility.LoadFileBytes(strFile2Export)

			param = cmd.Parameters.AddWithValue("@MyRTFText", String.Empty)	' rtfControl.Document.RtfText) ' RtfText)
			param = cmd.Parameters.AddWithValue("@DocumentContent", m_Utility.LoadFileBytes(documentFilename))
			param = cmd.Parameters.AddWithValue("@MyText", rtfControl.Text)
			param = cmd.Parameters.AddWithValue("@MANr", iMANr)
			param = cmd.Parameters.AddWithValue("@RecName", strTemplateName)
			param = cmd.Parameters.AddWithValue("@BinaryFile", myFile)
			param = cmd.Parameters.AddWithValue("@ChangedFrom", m_InitializationData.UserData.UserFullNameWithComma)

			cmd.ExecuteNonQuery()

			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowInfoDialog(m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich gespeichert."))


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			SplashScreenManager.CloseForm(False)
			DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "SaveLLText_0", MessageBoxButtons.OK, MessageBoxIcon.Error)

		Finally
			SplashScreenManager.CloseForm(False)
		End Try

	End Sub

End Class

Public Class LLTemplateData

	Public Property RecNr As Integer
	Public Property DbFieldName As String
	Public Property Bezeichnung As String

	Public Property FileName As String


End Class

Public Class EmployeeMaskData

	Public Property Geschlecht As String
	Public Property MANachname As String
	Public Property MAVorname As String
	Public Property MAStrasse As String
	Public Property MAPLZ As String
	Public Property MAOrt As String
	Public Property MATelefon As String
	Public Property MANatel As String
	Public Property MAEMail As String
	Public Property MAGebDat As DateTime?
	Public Property MAHBeruf As String
	Public Property MAHeimatort As String
	Public Property MAAnrede As String
	Public Property MANationality As String
	Public Property MANationalityLabel As String
	Public Property MACountryName As String
	Public Property MAMobility_Bez As String
  Public Property MAFahrzeug As String

  Public Property F_Schein_1_Bez As String
	Public Property F_Schein_2_Bez As String
	Public Property F_Schein_3_Bez As String
	Public Property F_Schein1 As String
	Public Property F_Schein2 As String
	Public Property F_Schein3 As String
	Public Property MAKiAnz As String
	Public Property MAKiJahrgang As String
	Public Property MAZivilstand As String
	Public Property MABewillig As String


	Public ReadOnly Property MAName As String
		Get
			Return String.Format("{0} {1}", MAVorname, MANachname)
		End Get
	End Property

	Public ReadOnly Property EmployeeAnrede As String
		Get
			Return String.Format("{0}", If(Geschlecht = "M", "Herr", "Frau"))
		End Get
	End Property

	Public ReadOnly Property EmployeeAge As Integer
		Get
			Return Now.Year - MAGebDat.GetValueOrDefault(Now).Year
		End Get
	End Property

	Public ReadOnly Property EmployeeBirthdayYear As Integer
		Get
			Return MAGebDat.GetValueOrDefault(Now).Year
		End Get
	End Property


End Class


Public Class EmployeeChildMaskData

	Public Property Geschlecht As String
	Public Property MANachname As String
	Public Property MAVorname As String

	Public Property GebDate As DateTime?

	Public ReadOnly Property ChildGentName As String
		Get
			Return String.Format("{0}", If(Geschlecht = "M", "Sohn", "Tochter"))
		End Get
	End Property

	Public ReadOnly Property ChildBirthdayYear As Integer
		Get
			Return GebDate.GetValueOrDefault(Now).Year
		End Get
	End Property


End Class


Public Class AdvisorMaskData


	Public Property USFName As String
	Public Property USLName As String
	Public Property USAnrede As String
	Public Property USTelefon As String
	Public Property USTelefax As String
	Public Property USEmail As String
	Public Property USTitel_1 As String
	Public Property USTitel_2 As String
	Public Property USNatel As String
	Public Property USMDName As String
	Public Property USMDName2 As String
	Public Property USMDName3 As String
	Public Property USMDPostfach As String
	Public Property USMDStrasse As String

	Public Property USMDPLZ As String
	Public Property USMDOrt As String
	Public Property USMDLand As String
	Public Property USMDTelefon As String
	Public Property USMDTelefax As String
	Public Property USMDEMail As String
	Public Property USMDHomepage As String


	Public ReadOnly Property Advisor As String
		Get
			Return String.Format("{0} {1}", USFName, USLName)
		End Get
	End Property

	Public ReadOnly Property YourAdvisor As String
		Get
			Return String.Format("Ihr{0}", If(USAnrede.StartsWith("H"), " Berater", "e Beraterin"))
		End Get
	End Property

End Class




Namespace CustomCommand

#Region "customsavecommand"

	Public Class CustomSaveDocumentCommand
		Inherits SaveDocumentCommand

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		Private m_EmployeeNumber As Integer


		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal control As IRichEditControl, ByVal iMANr As Integer)
			MyBase.New(control)
			m_InitializationData = _setting

			m_EmployeeNumber = iMANr
		End Sub

		Protected Overrides Sub ExecuteCore()
			Try

				If m_EmployeeNumber = 0 Then
					Dim strMsg As String = "Fehler in der Anwendung. Der Kandidat kann nicht gefunden werden! Ihre Daten werden nicht gespeichert!"
					DevExpress.XtraEditors.XtraMessageBox.Show(strMsg, "Datei speichern", MessageBoxButtons.OK, MessageBoxIcon.Error)
					Return
				End If
				Dim loadDataUtility As New LoadData(m_InitializationData)
				loadDataUtility.SaveLLText("Reserve0", ClsDataDetail.GetSelectedTemplateName, m_EmployeeNumber, Control)
				ClsDataDetail.bContentChanged = False

			Catch ex As Exception

			End Try

		End Sub

	End Class

	Public Class CustomPrintDocumentCommand
		Inherits QuickPrintCommand

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		Private m_EmployeeNumber As Integer

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal control As IRichEditControl, ByVal iMANr As Integer)

			MyBase.New(control)

			m_InitializationData = _setting
			m_EmployeeNumber = iMANr

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

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		Private ReadOnly service As IRichEditCommandFactoryService
		Private ReadOnly control As RichEditControl
		Private m_EmployeeNumber As Integer

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal control As RichEditControl, ByVal service As IRichEditCommandFactoryService, ByVal iMANr As Integer)
			Try

				m_InitializationData = _setting

				Guard.ArgumentNotNull(control, "control")
				Guard.ArgumentNotNull(service, "service")

				Trace.WriteLine(control.Name)
				Me.control = control
				Me.service = service
				m_EmployeeNumber = iMANr

			Catch ex As Exception

			End Try

		End Sub


#Region "IRichEditCommandFactoryService Members"

		Public Function CreateCommand(ByVal id As RichEditCommandId) As RichEditCommand Implements IRichEditCommandFactoryService.CreateCommand

			Try
				Select Case id
					Case RichEditCommandId.FileSave
						Return New CustomSaveDocumentCommand(m_InitializationData, control, m_EmployeeNumber)

					Case RichEditCommandId.QuickPrint
						Return New CustomPrintDocumentCommand(m_InitializationData, control, m_EmployeeNumber)

					Case RichEditCommandId.Print
						Return New CustomPrintDocumentCommand(m_InitializationData, control, m_EmployeeNumber)

					Case Else
						'Trace.WriteLine("Was not defined: " & service.CreateCommand(id).ToString)
						Return service.CreateCommand(id)
						Return Nothing

				End Select

				'If id = RichEditCommandId.FileSave Then
				'	Return New CustomSaveDocumentCommand(control, _iMANr)
				'ElseIf id = RichEditCommandId.QuickPrint Then
				'	Return New CustomPrintDocumentCommand(control, _iMANr)
				'ElseIf id = RichEditCommandId.Print Then
				'	Return New CustomPrintDocumentCommand(control, _iMANr)

				'End If

			Catch ex As Exception

			End Try

			Return Nothing
			'Return service.CreateCommand(id)
		End Function

#End Region


	End Class


#End Region    ' #iricheditcommandfactoryservice

	' TODO: Update 20.1.4
	'Public Class MyKeyboardHandlerServiceWrapper
	'	Inherits KeyboardHandlerServiceWrapper
	'	Dim _iMANr As Integer

	'	Public Sub New(ByVal service As IKeyboardHandlerService, ByVal iMANr As Integer)
	'		MyBase.New(service)
	'		_iMANr = iMANr
	'	End Sub

	'	'Public Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
	'	Public Sub OnKeyDown(ByVal e As KeyEventArgs)

	'		Try

	'			If ((e.KeyCode = Keys.C) OrElse (e.KeyCode = Keys.X) OrElse (e.KeyCode = Keys.Insert)) AndAlso e.Control Then
	'				MsgBox(e.KeyCode.ToString)
	'				'Return
	'			Else
	'				MyBase.OnKeyDown(e)
	'			End If
	'			Trace.WriteLine("KeyCode: " & e.KeyCode.ToString)
	'		Catch ex As Exception

	'		End Try

	'	End Sub
	'End Class

End Namespace
