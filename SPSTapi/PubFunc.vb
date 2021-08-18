
Imports System
Imports System.IO
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel

Imports System.Text
Imports System.Security.Cryptography

Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.ProgPath

Imports SPSTapi.ClsDataDetail
Imports SP.Infrastructure.Logging

Module PubFunc

	Private m_Logger As ILogger = New Logger()

	Private m_Utility As New SPProgUtility.MainUtilities.Utilities


#Region "Consts"

	Const strEncryptionKey As String = "%3/NJA98ASJAG7S634JAHZH&\GHK21&74575m823w6467KS#J578iH4HS61648@567Qq635766A836=58)" &
																			"73425(JKJASDKJ?238ASuD852sKJKJASDJK5ADSAKJ34WKJASJKAW4598lKJSDnFKJs"

#End Region



	Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant As SPProgUtility.ClsMDData = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData As SPProgUtility.ClsUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)

		Dim personalizedData As Dictionary(Of String, SPProgUtility.ClsProsonalizedData) = m_md.GetPersonalizedCaptionInObject(iMDNr)
		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate As Dictionary(Of String, SPProgUtility.ClsTranslationData) = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function



#Region "Funktion für Datenbanksuche..."

	Function GetMyNumberInfo(ByVal strNrID As String) As String
		Dim strResult As String = strNrID
		Dim strModulName As String = m_Translate.GetSafeTranslationValue("Kandidat") & ": "
		' Mitarbeiter-Datenbank zuerst suchen...
		Dim strSqlQuery As String = "[KDZHD_Tel_1]"
		Dim str1Param As String = "@TeleNr"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		ClsDataDetail.GetModeNr = 0
		ClsDataDetail.GetMANr = 0
		ClsDataDetail.GetKDNr = 0
		ClsDataDetail.GetKDZhdNr = 0
		ClsDataDetail.GetRecNr = 0
		If Len(strNrID.Replace(" ", "").Trim) <= ClsDataDetail.GetInterLineLen() Then Return strResult

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue(str1Param, strNrID)
			'Throw New InvalidDataException

			Dim rTelrec As SqlDataReader = cmd.ExecuteReader

			If Not rTelrec.HasRows Then
				strSqlQuery = "[MA_Tel_1]"

				rTelrec.Close()

				cmd = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				cmd.Parameters.Clear()
				param = cmd.Parameters.AddWithValue(str1Param, strNrID)

				rTelrec = cmd.ExecuteReader
				If rTelrec.HasRows Then
					ClsDataDetail.GetModeNr = 1
					strModulName = m_Translate.GetSafeTranslationValue("Kandidat") & ": "

				Else
					Return strResult

				End If

			Else


			End If

			If rTelrec.Read Then
				ClsDataDetail.GetMDNr = CInt(m_Utility.SafeGetInteger(rTelrec, "MDNr", 0))

				If Not IsDBNull(rTelrec("RecNr")) Then
					If rTelrec("AllNumbers").ToString.ToUpper.IndexOf(";KD-Telefon: ".ToUpper & strNrID.Replace(" ", "").Trim) > 0 Then
						ClsDataDetail.GetModeNr = 2
						strModulName = m_Translate.GetSafeTranslationValue("Kunde") & ": "
					Else
						ClsDataDetail.GetModeNr = 3
						strModulName = m_Translate.GetSafeTranslationValue("Zuständige Person") & ": "

					End If

				ElseIf ClsDataDetail.GetModeNr <> 1 Then
					ClsDataDetail.GetModeNr = 2
					strModulName = m_Translate.GetSafeTranslationValue("Kunde") & ": "

				End If
			End If

			If ClsDataDetail.GetModeNr = 1 Then          ' Kandidaten
				strResult = rTelrec("MANr").ToString & vbCrLf
				ClsDataDetail.GetMANr = CInt(rTelrec("MANr"))

				ClsDataDetail.GetKDNr = 0
				ClsDataDetail.GetKDZhdNr = 0

			ElseIf ClsDataDetail.GetModeNr = 2 Then
				strResult = rTelrec("KDNr").ToString & vbCrLf
				ClsDataDetail.GetMANr = 0
				ClsDataDetail.GetKDNr = CInt(rTelrec("KDNr").ToString)
				' If ClsDataDetail.GetModeNr = 2 Then      ' Kunden
				ClsDataDetail.GetKDZhdNr = 0

			ElseIf ClsDataDetail.GetModeNr = 3 Then      ' ZHD
				ClsDataDetail.GetMANr = 0
				If Not IsDBNull(rTelrec("RecNr")) Then
					ClsDataDetail.GetKDZhdNr = CInt(rTelrec("RecNr").ToString)
				End If
				ClsDataDetail.GetKDNr = CInt(rTelrec("KDNr").ToString)

				'        End If
			End If
			strModulName &= vbCrLf & strResult

			strResult = strModulName & rTelrec("Call_Name").ToString &
									CStr(IIf(rTelrec("Call_Name").ToString <> String.Empty, vbCrLf, "")) &
									rTelrec("Call_Strasse").ToString & vbCrLf &
									rTelrec("Call_Adresse").ToString & vbCrLf &
									rTelrec("Call_Anrede").ToString & " " &
									rTelrec("Call_Vorname").ToString & " " & rTelrec("Call_Nachname").ToString & vbCrLf &
									strNrID


		Catch ex As InvalidDataException
			MsgBox(ex.Message.ToString, MsgBoxStyle.Exclamation, "GetNumberInfo: InvalidDataException")

		Catch e As Exception
			MsgBox(e.Message, MsgBoxStyle.Exclamation, "GetNumberInfo: Exception")


		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function

	Function GetMyNumberInfo(ByVal iKDNr As Integer) As String
		Dim strResult As String = String.Empty
		Dim strModulName As String = m_Translate.GetSafeTranslationValue("Kandidat") & ": "
		Dim strSqlQuery As String = "[KDZHD_Tel_1 With KDNr]"
		Dim str1Param As String = "@TeleNr"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			ClsDataDetail.GetModeNr = 0
			ClsDataDetail.GetMANr = 0
			ClsDataDetail.GetKDZhdNr = 0
			ClsDataDetail.GetRecNr = 0

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
			param = cmd.Parameters.AddWithValue("@KDZNr", 0)
			Dim rTelrec As SqlDataReader = cmd.ExecuteReader

			If rTelrec.Read Then
				ClsDataDetail.GetMDNr = CInt(m_Utility.SafeGetInteger(rTelrec, "MDNr", 0))

				ClsDataDetail.GetModeNr = 2
				strModulName = m_Translate.GetSafeTranslationValue("Kunde") & ": "

				strResult = rTelrec("KDNr").ToString & vbCrLf
				ClsDataDetail.GetKDNr = CInt(rTelrec("KDNr").ToString)
				strModulName &= vbCrLf & strResult

				strResult = strModulName & rTelrec("Call_Name").ToString &
										CStr(IIf(rTelrec("Call_Name").ToString <> String.Empty, vbCrLf, "")) &
										rTelrec("Call_Strasse").ToString & vbCrLf &
										rTelrec("Call_Adresse").ToString & vbCrLf &
										rTelrec("Call_Anrede").ToString & " " &
										rTelrec("Call_Vorname").ToString & " " & rTelrec("Call_Nachname").ToString
			End If


		Catch ex As InvalidDataException
			Return "(InvalidDataException): " & ex.Message.ToString

		Catch e As Exception
			Return "(Exception): " & e.Message.ToString

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function

	Function GetMyNumberInfo(ByVal iKDNr As Integer, ByVal iKDZNr As Integer) As String
		Dim strResult As String = String.Empty
		Dim strModulName As String = String.Empty
		Dim strSqlQuery As String = "[KDZHD_Tel_1 With KDNr]"
		Dim str1Param As String = "@TeleNr"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			ClsDataDetail.GetModeNr = 0
			ClsDataDetail.GetMANr = 0
			ClsDataDetail.GetRecNr = 0

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
			param = cmd.Parameters.AddWithValue("@KDZNr", iKDZNr)
			Dim rTelrec As SqlDataReader = cmd.ExecuteReader

			If rTelrec.Read Then
				ClsDataDetail.GetMDNr = CInt(m_Utility.SafeGetInteger(rTelrec, "MDNr", 0))
				ClsDataDetail.GetModeNr = 3
				strModulName = m_Translate.GetSafeTranslationValue("Zuständige Person") & ": "

				strResult = rTelrec("KDNr").ToString & vbCrLf
				ClsDataDetail.GetKDNr = CInt(rTelrec("KDNr").ToString)
				ClsDataDetail.GetKDZhdNr = CInt(rTelrec("RecNr").ToString)
				strModulName &= vbCrLf & strResult

				strResult = strModulName & rTelrec("Call_Name").ToString &
										CStr(IIf(rTelrec("Call_Name").ToString <> String.Empty, vbCrLf, "")) &
										rTelrec("Call_Strasse").ToString & vbCrLf &
										rTelrec("Call_Adresse").ToString & vbCrLf &
										rTelrec("Call_Anrede").ToString & " " &
										rTelrec("Call_Vorname").ToString & " " & rTelrec("Call_Nachname").ToString
			End If


		Catch ex As InvalidDataException
			Return "(InvalidDataException): " & ex.Message.ToString & vbNewLine &
							String.Format("KDNr: {0} / KDZNr: {1}", iKDNr, iKDZNr)

		Catch e As Exception
			Return "(Exception): " & e.Message.ToString & vbNewLine &
							String.Format("KDNr: {0} / KDZNr: {1}", iKDNr, iKDZNr)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function

	Function GetMyNumberInfo(ByVal iKDNr As Integer, ByVal iKDzNr As Integer, ByVal iMANr As Integer) As String
		Dim strResult As String = m_Translate.GetSafeTranslationValue("Kandidat: ({1}){0}{2} {3}, {4}{0}{5}{0}{6}")
		'Dim strModulName As String = m_Translate.GetSafeTranslationValue("Kandidat") & ": "
		Dim strSqlQuery As String = "[MA_Tel_1 With MANr]"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			ClsDataDetail.GetModeNr = 0
			ClsDataDetail.GetKDNr = 0
			ClsDataDetail.GetKDZhdNr = 0
			ClsDataDetail.GetRecNr = 0

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MANr", iMANr)

			Dim rTelrec As SqlDataReader = cmd.ExecuteReader

			If rTelrec.Read Then
				ClsDataDetail.GetMDNr = CInt(m_Utility.SafeGetInteger(rTelrec, "MDNr", 0))

				ClsDataDetail.GetModeNr = 1
				'strModulName = m_Translate.GetSafeTranslationValue("Kandidat") & ": "
				'strResult = rTelrec("MANr").ToString & vbCrLf

				ClsDataDetail.GetMANr = CInt(m_Utility.SafeGetInteger(rTelrec, "MANr", 0))
				'strModulName &= vbCrLf & strResult

				strResult = String.Format(strResult, vbNewLine,
																	m_Utility.SafeGetInteger(rTelrec, "MANr", 0),
																	m_Utility.SafeGetString(rTelrec, "Call_Anrede"),
																	m_Utility.SafeGetString(rTelrec, "Call_Nachname"),
																	m_Utility.SafeGetString(rTelrec, "Call_Vorname"),
																	m_Utility.SafeGetString(rTelrec, "Call_Strasse"),
																	m_Utility.SafeGetString(rTelrec, "Call_Adresse"))

			Else
				Return strResult

			End If


		Catch ex As InvalidDataException
			Return "(InvalidDataException): " & ex.Message.ToString & vbNewLine &
							String.Format("MANr: {0}", iMANr)

		Catch e As Exception
			Return "(Exception): " & e.Message.ToString & vbNewLine &
							String.Format("MANr: {0}", iMANr)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function

#End Region

	''' <summary>
	''' Formatierung von Zeiten für Telefonie
	''' </summary>
	''' <param name="MyTime">ist in Sekunden</param>
	''' <returns>ist als "00:00:00"</returns>
	''' <remarks>
	''' 1 Milisecound =  0.001 Secound
	''' 1 Secound = 1/60 Minute
	''' 1 Minute = 60 Secounds
	''' 1 Houre = 60 Minutes = 3600 Secounds = 3600000 Milisecounds
	''' </remarks>
	''' 
	Function GetMyTime(ByVal MyTime As Long) As String
		Dim strResult As String = "00:00:00"
		Dim lHoure As Double = 0
		Dim lMinute As Long = 0
		Dim lSecound As Long = 0
		Dim dSec As Double = 0

		lSecound = CLng(MyTime)
		lMinute = CLng(lSecound / 60)
		dSec = CDbl(Right(CStr(CDbl(Format((lSecound / 60), "0.00"))), 3))
		If dSec > 0 Then lSecound = CLng(dSec * 60) : lMinute -= CLng(IIf(lMinute = 0, 0, 1))
		If lMinute > 60 Then

		End If

		lHoure = CLng(lMinute / 60)

		If lSecound = 0 And lMinute = 0 And lHoure = 0 Then
			' Ist im Bereich Milisekunden...
			strResult = "00:00:00"
			Return strResult
		Else
			If lMinute = 0 And lHoure = 0 Then
				strResult = "00:00:" & Format(lSecound, "00")
				Return strResult

			ElseIf lHoure = 0 Then
				strResult = "00:" & Format(lMinute, "00") & ":" & Format((dSec * 60), "00")
				Return strResult

			Else
				strResult = Format(lMinute \ 60, "00") & ":" & Format(lMinute Mod 60, "00") & ":" & Format((dSec * 60), "00")
				Return strResult

			End If

		End If
		'Dim strTest As String = Format(lMinute \ 60, "00") & "h " & Format(lMinute Mod 60, "00") & " min"


		Return strResult
	End Function

	Public Function ReadFromFile(ByVal msg As String) As String
		Dim strFileContent As String = String.Empty
		Dim m_path As New ClsProgPath
		Dim strfullFilename As String = String.Format("{0}{1}", m_path.GetSpS2DeleteHomeFolder, "MyPhoneBook.tmp")
		If Not File.Exists(strfullFilename) Then Return strFileContent

		Try
			'check the file
			Dim FileName As String = strfullFilename
			Dim CheckFile As New FileInfo(strfullFilename)
			Dim r As StreamReader = CheckFile.OpenText()
			strFileContent = r.ReadToEnd()

			r.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strfullFilename, ex.ToString))

		End Try

		Return strFileContent
	End Function

	Public Sub WriteToFile(ByVal msg As String) ', ByVal stkTrace As String, ByVal title As String)
		Dim m_path As New ClsProgPath
		Dim strfullFilename As String = String.Format("{0}{1}", m_path.GetSpS2DeleteHomeFolder, "MyPhoneBook.tmp")
		Dim strFileContent As String = String.Empty

		Try
			Try
				Dim strExistsFileContent As String = ReadFromFile(strfullFilename)
				Dim aNumber As String() = strExistsFileContent.Split(CChar(vbCrLf))

				For i As Integer = 0 To aNumber.Length - 1
					If aNumber(i).ToString = msg Then Exit Sub
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strfullFilename, ex.ToString))

			End Try

			'check the file
			Dim CheckFile As New FileInfo(strfullFilename)
			If Not CheckFile.Exists Then SaveTextToFile("")

			Dim r As StreamReader = CheckFile.OpenText()
			strFileContent = msg & vbCrLf & r.ReadToEnd()

			r.Close()

			'log it
			Dim fs1 As FileStream = New FileStream(strfullFilename, FileMode.Create, FileAccess.Write)
			Dim s1 As StreamWriter = New StreamWriter(fs1)
			s1.Write(strFileContent)

			s1.Close()
			fs1.Close()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strfullFilename, ex.ToString))

		End Try

	End Sub


	Function SaveTextToFile(ByVal strData As String) As Boolean
		Dim m_path As New ClsProgPath
		Dim strfullFilename As String = String.Format("{0}{1}", m_path.GetSpS2DeleteHomeFolder, "MyPhoneBook.tmp")

		Dim bAns As Boolean = False
		Dim objReader As StreamWriter

		Try
			objReader = New StreamWriter(strfullFilename)
			objReader.Write(strData)
			objReader.Close()
			bAns = True

		Catch Ex As Exception
			m_Logger.LogError(Ex.ToString)

		End Try

		Return bAns
	End Function

	Function FormatTime(ByVal lngInputValue As Long,
											Optional ByVal sInputType As String = "s",
											Optional ByVal sFormat As String = "hh:mm:ss",
											Optional ByVal sMaxValue As String = "d") As String
		FormatTime = sFormat
	End Function

	Public Function encryptmessage(ByVal message As String) As String
		Dim rd As New RijndaelManaged
		Dim md5 As New MD5CryptoServiceProvider
		Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes("Testme")) 'strEncryptionKey))

		Try
			md5.Clear()
			rd.Key = key
			rd.GenerateIV()

			Dim iv() As Byte = rd.IV
			Dim ms As New MemoryStream

			Dim cs As New CryptoStream(ms, rd.CreateEncryptor, CryptoStreamMode.Write)
			Dim data() As Byte = System.Text.Encoding.UTF8.GetBytes(message)

			cs.Write(data, 0, data.Length)
			cs.FlushFinalBlock()

			Dim encdata() As Byte = ms.ToArray

			cs.Close()
			rd.Clear()

			Return Convert.ToBase64String(encdata)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return String.Empty

		End Try

	End Function

	Public Function decryptmessage(ByVal incommingmessage As String) As String
		Dim rd As New RijndaelManaged
		Dim RijndaelovLength As Integer = 16
		Dim md5 As New MD5CryptoServiceProvider
		Dim key() As Byte = md5.ComputeHash(Encoding.UTF8.GetBytes("Testme")) 'strEncryptionKey))

		Try
			md5.Clear()

			Dim encdata() As Byte = Convert.FromBase64String(incommingmessage)
			Dim ms As New MemoryStream(encdata)
			Dim iv(15) As Byte

			ms.Read(iv, 0, RijndaelovLength)
			rd.IV = iv
			rd.Key = key

			Dim cs As New CryptoStream(ms, rd.CreateDecryptor, CryptoStreamMode.Read)
			Dim data(CInt(ms.Length) - RijndaelovLength) As Byte
			If data.Length <= 1 Then Return String.Empty
			Dim i As Integer = cs.Read(data, 0, data.Length)

			Return System.Text.Encoding.UTF8.GetString(data, 0, i)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
			Return String.Empty

		End Try

	End Function

	Public Function FormIsLoaded(ByVal sName As String, ByVal bDisposeForm As Boolean) As Boolean
		Dim bResult As Boolean = False

		' alle geöffneten Forms durchlauden
		For Each oForm As Form In Application.OpenForms
			If oForm.Name.ToLower = sName.ToLower Then
				If bDisposeForm Then oForm.Dispose() : Exit For
				bResult = True : Exit For
			End If
		Next

		Return (bResult)
	End Function


End Module
