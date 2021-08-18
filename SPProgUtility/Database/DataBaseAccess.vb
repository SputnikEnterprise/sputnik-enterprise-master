
Imports System.Reflection.Assembly
Imports NLog
Imports System.Data.SqlClient
Imports SPProgUtility.ProgPath.ClsProgPath
Imports SPProgUtility.ProgPath
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports System.Text
Imports System.Security.Cryptography
Imports System.IO


Public Class DataBaseAccess
	Inherits Utilities


#Region "Constants"

	Private Const strEncryptionKey As String = "your crypt key"
	Private Const strExtraPass As String = "yourseckey"

#End Region


#Region "Private Fields"

	Private Shared m_logger As Logger = LogManager.GetCurrentClassLogger()
	Private m_Reg As ClsDivReg

#End Region


#Region "Methods..."

	Private ReadOnly Property GetDefaultMDNr() As Integer
		Get
			m_Reg = New ClsDivReg
			Return CInt(m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDNr"))
		End Get
	End Property

	Private ReadOnly Property GetDefaultUSNr() As Integer
		Get
			m_Reg = New ClsDivReg
			Return CInt(m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "UserNr").ToString)
		End Get
	End Property

#End Region


	Function OpenSelectedMDDatabase(ByVal iMDNr As Integer, ByVal strConn As String) As ClsMDData
		If iMDNr = 0 Then iMDNr = Me.GetDefaultMDNr
		Dim Conn As New SqlConnection(strConn)
		Dim result As New ClsMDData

		Dim bCreateFile As Boolean = False
		Dim sOffDocSql As String = "Select Top 1 Customer_ID, MD_Name1, MD_Name2, MD_Name3, Strasse, PLZ, Ort, Land, MD_Kanton, "
		sOffDocSql &= "Telefon, Telefax, eMail, Homepage, Convert(int, Jahr) As Jahr, MDFullFileName MDRootPath "
		sOffDocSql &= "From Mandanten Where Jahr = @Year And MDNr = @MDNr Order By Convert(Int, Jahr) Desc"

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("@Year", ReplaceMissing(Now.Year, DBNull.Value)))
		listOfParams.Add(New SqlClient.SqlParameter("@MDNr", iMDNr))

		Dim reader As SqlClient.SqlDataReader = OpenReader(strConn, sOffDocSql, listOfParams, CommandType.Text)

		Try
			If (Not reader Is Nothing) Then
				result = New ClsMDData

				If reader.Read Then
					result = New ClsMDData

					result.MDName = SafeGetString(reader, "MD_Name1")
					result.MDName_2 = SafeGetString(reader, "MD_Name2")
					result.MDName_3 = SafeGetString(reader, "MD_Name3")
					result.MDStreet = SafeGetString(reader, "Strasse")
					result.MDPostcode = SafeGetString(reader, "PLZ")
					result.MDCity = SafeGetString(reader, "Ort")
					result.MDCountry = SafeGetString(reader, "Land")
					result.MDCanton = SafeGetString(reader, "md_kanton")
					result.MDTelefon = SafeGetString(reader, "Telefon")
					result.MDTelefax = SafeGetString(reader, "Telefax")
					result.MDeMail = SafeGetString(reader, "eMail")
					result.MDHomepage = SafeGetString(reader, "Homepage")
					result.MDGuid = SafeGetString(reader, "Customer_ID")
					result.MDYear = SafeGetInteger(reader, "Jahr", 0)
					result.MDRootPath = SafeGetString(reader, "MDRootPath")

				End If
				reader.Close()

				If result.MDYear = 0 Then
					m_logger.Error(String.Format("({0}) MD Data for year was not founded!", iMDNr))

					sOffDocSql = "Select Top 1 Customer_ID, MD_Name1, MD_Name2, MD_Name3, Strasse, PLZ, Ort, Land, MD_Kanton, "
					sOffDocSql &= "Telefon, Telefax, eMail, Homepage, Convert(int, Jahr) As Jahr, MDFullFileName MDRootPath "
					sOffDocSql &= "From Mandanten Where MDNr = @MDNr Order By Convert(Int, Jahr) Desc"

					' Parameters
					listOfParams = New List(Of SqlClient.SqlParameter)
					listOfParams.Add(New SqlClient.SqlParameter("@MDNr", iMDNr))
					reader = OpenReader(strConn, sOffDocSql, listOfParams, CommandType.Text)

					Try

						If (Not reader Is Nothing) Then
							result = New ClsMDData

							If reader.Read Then
								result = New ClsMDData

								result.MDName = SafeGetString(reader, "MD_Name1")
								result.MDName_2 = SafeGetString(reader, "MD_Name2")
								result.MDName_3 = SafeGetString(reader, "MD_Name3")
								result.MDStreet = SafeGetString(reader, "Strasse")
								result.MDPostcode = SafeGetString(reader, "PLZ")
								result.MDCity = SafeGetString(reader, "Ort")
								result.MDCountry = SafeGetString(reader, "Land")
								result.MDCanton = SafeGetString(reader, "md_kanton")
								result.MDTelefon = SafeGetString(reader, "Telefon")
								result.MDTelefax = SafeGetString(reader, "Telefax")
								result.MDeMail = SafeGetString(reader, "eMail")
								result.MDHomepage = SafeGetString(reader, "Homepage")
								result.MDGuid = SafeGetString(reader, "Customer_ID")
								result.MDYear = SafeGetInteger(reader, "Jahr", 0)
								result.MDRootPath = SafeGetString(reader, "MDRootPath")

							End If
							m_logger.Error(String.Format("({0}) MD Data for year ({1}) was founded!", iMDNr, result.MDYear))
							reader.Close()

						End If

					Catch ex As Exception
						m_logger.Error(ex.ToString())

					End Try

				End If

			End If


		Catch ex As Exception
			result = Nothing
			m_logger.Error(ex.ToString())

		End Try

		Return result
	End Function

	Function OpenSelectedUserDatabase(ByVal strConn As String, ByVal iUserNr As Integer) As ClsUserData
		If iUserNr = 0 Then iUserNr = Me.GetDefaultUSNr
		Dim Conn As New SqlConnection(strConn)
		Dim result As New ClsUserData


		Dim bCreateFile As Boolean = False
		Dim Sql As String = "[Get Selected Userdata]"

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("@USNr", iUserNr))

		Dim reader As SqlClient.SqlDataReader = OpenReader(strConn, Sql, listOfParams, CommandType.StoredProcedure)

		Try
			If (Not reader Is Nothing) Then
				result = New ClsUserData

				If reader.Read Then
					result = New ClsUserData

					result.UserNr = SafeGetString(reader, "USNr")

					result.UserLoginname = DecryptWithClipper(SafeGetString(reader, "UserLoginname"), strEncryptionKey)
					result.UserLoginPassword = DecryptWithClipper(SafeGetString(reader, "UserLoginPassword"), strEncryptionKey)

					result.UserSalutation = SafeGetString(reader, "USAnrede")

					result.UserFName = SafeGetString(reader, "USVorname", "").ToString.Trim
					result.UserLName = SafeGetString(reader, "USNachname", "").ToString.Trim
					result.UserFullNameWithComma = String.Format("{0}, {1}", SafeGetString(reader, "USNachname", "").ToString.Trim, SafeGetString(reader, "USVorname", "").ToString.Trim)
					result.UserFullName = String.Format("{0} {1}", SafeGetString(reader, "USVorname", "").ToString.Trim, SafeGetString(reader, "USNachname", "").ToString.Trim)


					result.UserGuid = SafeGetString(reader, "USGuid")
					result.UserKST_1 = SafeGetString(reader, "USKST1")
					result.UserKST_2 = SafeGetString(reader, "USKST2")
					result.UserKST = SafeGetString(reader, "KST")
					result.UserFTitel = SafeGetString(reader, "USTitel_1")
					result.UserSTitel = SafeGetString(reader, "USTitel_2")
					result.UserFiliale = SafeGetString(reader, "USFiliale")
					result.UserBusinessBranch = SafeGetString(reader, "UserBusinessBranch")

					result.UserTelefon = SafeGetString(reader, "USTelefon")
					result.UserTelefax = SafeGetString(reader, "USTelefax")
					result.UserMobile = SafeGetString(reader, "USNatel")
					result.UsereMail = SafeGetString(reader, "USeMail")

					result.UserLanguage = SafeGetString(reader, "USLanguage")
					If String.IsNullOrWhiteSpace(result.UserLanguage) Then result.UserLanguage = "deutsch"
					result.usermdGuid = SafeGetString(reader, "UserMDGuid")

					result.UserMDName = SafeGetString(reader, "MDName")
					result.UserMDName2 = SafeGetString(reader, "MDName2")
					result.UserMDName3 = SafeGetString(reader, "MDName3")
					result.UserMDPostfach = SafeGetString(reader, "MDPostfach")
					result.UserMDStrasse = SafeGetString(reader, "MDStrasse")
					result.UserMDPLZ = SafeGetString(reader, "MDPLZ")
					result.UserMDOrt = SafeGetString(reader, "MDOrt")
					result.UserMDLand = SafeGetString(reader, "MDLand")
					result.UserMDCanton = SafeGetString(reader, "MD_Kanton")

					result.UserMDTelefon = SafeGetString(reader, "MDTelefon")
					result.UserMDdTelefon = SafeGetString(reader, "MDDTelefon")
					result.UserMDTelefax = SafeGetString(reader, "MDTelefax")
					result.UserMDeMail = SafeGetString(reader, "MDeMail")
					result.UserMDHomepage = SafeGetString(reader, "MDHomepage")


				End If

			End If

			reader.Close()

		Catch ex As Exception
			result = Nothing
			m_logger.Error(ex.ToString())

		End Try

		Return result
	End Function


	Function OpenSelectedUserDatabaseWithKST(ByVal strConn As String, ByVal strKst As String) As ClsUserData
		If strKst = String.Empty Then
			m_logger.Error("Suche nach User fehlt der KST...")
			Return OpenSelectedUserDatabase(strConn, Me.GetDefaultUSNr)
		End If

		Dim Conn As New SqlConnection(strConn)
		Dim result As New ClsUserData


		Dim bCreateFile As Boolean = False
		Dim Sql As String = "[Get Selected Userdata With KST]"

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("@KST", strKst))

		Dim reader As SqlClient.SqlDataReader = OpenReader(strConn, Sql, listOfParams, CommandType.StoredProcedure)

		Try
			If (Not reader Is Nothing) Then
				result = New ClsUserData

				If reader.Read Then
					result = New ClsUserData

					result.UserNr = SafeGetString(reader, "USNr")

					result.UserLoginname = DecryptWithClipper(SafeGetString(reader, "UserLoginname"), strEncryptionKey)
					result.UserLoginPassword = DecryptWithClipper(SafeGetString(reader, "UserLoginPassword"), strEncryptionKey)


					result.UserSalutation = SafeGetString(reader, "USAnrede")

					result.UserFName = SafeGetString(reader, "USVorname", "").ToString.Trim
					result.UserLName = SafeGetString(reader, "USNachname", "").ToString.Trim
					result.UserFullNameWithComma = String.Format("{0}, {1}", SafeGetString(reader, "USNachname", "").ToString.Trim, SafeGetString(reader, "USVorname", "").ToString.Trim)
					result.UserFullName = String.Format("{0} {1}", SafeGetString(reader, "USVorname", "").ToString.Trim, SafeGetString(reader, "USNachname", "").ToString.Trim)


					result.UserGuid = SafeGetString(reader, "USGuid")
					result.UserKST_1 = SafeGetString(reader, "USKST1")
					result.UserKST_2 = SafeGetString(reader, "USKST2")
					result.UserKST = SafeGetString(reader, "KST")
					result.UserFTitel = SafeGetString(reader, "USTitel_1")
					result.UserSTitel = SafeGetString(reader, "USTitel_2")
					result.UserFiliale = SafeGetString(reader, "USFiliale")
					result.UserBusinessBranch = SafeGetString(reader, "UserBusinessBranch")

					result.UserTelefon = SafeGetString(reader, "USTelefon")
					result.UserTelefax = SafeGetString(reader, "USTelefax")
					result.UsereMail = SafeGetString(reader, "USeMail")

					result.UserLanguage = SafeGetString(reader, "USLanguage")
					If String.IsNullOrWhiteSpace(result.UserLanguage) Then result.UserLanguage = "deutsch"
					result.UserMDGuid = SafeGetString(reader, "UserMDGuid")

					result.UserMDName = SafeGetString(reader, "MDName")
					result.UserMDName2 = SafeGetString(reader, "MDName2")
					result.UserMDName3 = SafeGetString(reader, "MDName3")
					result.UserMDPostfach = SafeGetString(reader, "MDPostfach")
					result.UserMDStrasse = SafeGetString(reader, "MDStrasse")
					result.UserMDPLZ = SafeGetString(reader, "MDPLZ")
					result.UserMDOrt = SafeGetString(reader, "MDOrt")
					result.UserMDLand = SafeGetString(reader, "MDLand")
					result.UserMDCanton = SafeGetString(reader, "MD_Kanton")

					result.UserMDTelefon = SafeGetString(reader, "MDTelefon")
					result.UserMDDTelefon = SafeGetString(reader, "MDDTelefon")
					result.UserMDTelefax = SafeGetString(reader, "MDTelefax")
					result.UserMDeMail = SafeGetString(reader, "MDeMail")
					result.UserMDHomepage = SafeGetString(reader, "MDHomepage")

				End If

			End If

			reader.Close()

		Catch ex As Exception
			result = Nothing
			m_logger.Error(ex.ToString())

		End Try

		Return result
	End Function

	Function OpenSelectedUserDatabaseWithUserName(ByVal strConn As String, ByVal strLastname As String, ByVal strFirstname As String) As ClsUserData
		If strLastname = String.Empty Or strFirstname = String.Empty Then
			m_logger.Error("Suche nach User fehlt der Last or Fistname...")
			Return OpenSelectedUserDatabase(strConn, Me.GetDefaultUSNr)
		End If

		Dim Conn As New SqlConnection(strConn)
		Dim result As New ClsUserData

		Dim bCreateFile As Boolean = False
		Dim Sql As String = "[Get Selected Userdata With Last And Firstname]"

		' Parameters
		Dim listOfParams As New List(Of SqlClient.SqlParameter)
		listOfParams.Add(New SqlClient.SqlParameter("@Lastname", strLastname))
		listOfParams.Add(New SqlClient.SqlParameter("@Firstname", strFirstname))

		Dim reader As SqlClient.SqlDataReader = OpenReader(strConn, Sql, listOfParams, CommandType.StoredProcedure)

		Try
			If (Not reader Is Nothing) Then
				result = New ClsUserData

				If reader.Read Then
					result = New ClsUserData

					result.UserNr = SafeGetString(reader, "USNr")

					result.UserLoginname = DecryptWithClipper(SafeGetString(reader, "UserLoginname"), strEncryptionKey)
					result.UserLoginPassword = DecryptWithClipper(SafeGetString(reader, "UserLoginPassword"), strEncryptionKey)

					result.UserSalutation = SafeGetString(reader, "USAnrede")

					result.UserFName = SafeGetString(reader, "USVorname", "").ToString.Trim
					result.UserLName = SafeGetString(reader, "USNachname", "").ToString.Trim
					result.UserFullNameWithComma = String.Format("{0}, {1}", SafeGetString(reader, "USNachname", "").ToString.Trim, SafeGetString(reader, "USVorname", "").ToString.Trim)
					result.UserFullName = String.Format("{0} {1}", SafeGetString(reader, "USVorname", "").ToString.Trim, SafeGetString(reader, "USNachname", "").ToString.Trim)


					result.UserGuid = SafeGetString(reader, "USGuid")
					result.UserKST_1 = SafeGetString(reader, "USKST1")
					result.UserKST_2 = SafeGetString(reader, "USKST2")
					result.UserKST = SafeGetString(reader, "KST")
					result.UserFTitel = SafeGetString(reader, "USTitel_1")
					result.UserSTitel = SafeGetString(reader, "USTitel_2")
					result.UserFiliale = SafeGetString(reader, "USFiliale")
					result.UserBusinessBranch = SafeGetString(reader, "UserBusinessBranch")

					result.UserTelefon = SafeGetString(reader, "USTelefon")
					result.UserTelefax = SafeGetString(reader, "USTelefax")
					result.UsereMail = SafeGetString(reader, "USeMail")

					result.UserLanguage = SafeGetString(reader, "USLanguage")
					If String.IsNullOrWhiteSpace(result.UserLanguage) Then result.UserLanguage = "deutsch"
					result.UserMDGuid = SafeGetString(reader, "UserMDGuid")

					result.UserMDName = SafeGetString(reader, "MDName")
					result.UserMDName2 = SafeGetString(reader, "MDName2")
					result.UserMDName3 = SafeGetString(reader, "MDName3")
					result.UserMDPostfach = SafeGetString(reader, "MDPostfach")
					result.UserMDStrasse = SafeGetString(reader, "MDStrasse")
					result.UserMDPLZ = SafeGetString(reader, "MDPLZ")
					result.UserMDOrt = SafeGetString(reader, "MDOrt")
					result.UserMDLand = SafeGetString(reader, "MDLand")
					result.UserMDCanton = SafeGetString(reader, "MD_Kanton")

					result.UserMDTelefon = SafeGetString(reader, "MDTelefon")
					result.UserMDDTelefon = SafeGetString(reader, "MDDTelefon")
					result.UserMDTelefax = SafeGetString(reader, "MDTelefax")
					result.UserMDeMail = SafeGetString(reader, "MDeMail")
					result.UserMDHomepage = SafeGetString(reader, "MDHomepage")

				End If

			End If

			reader.Close()

		Catch ex As Exception
			result = Nothing
			m_logger.Error(ex.ToString())

		End Try

		Return result
	End Function


#Region "Helpers"

	Private Function StripNullCharacters(ByVal vstrStringWithNulls As String) As String
		Dim intPosition As Integer
		Dim strStringWithOutNulls As String

		intPosition = 1
		strStringWithOutNulls = vstrStringWithNulls

		Do While intPosition > 0
			intPosition = InStr(intPosition, vstrStringWithNulls, vbNullChar)

			If intPosition > 0 Then
				strStringWithOutNulls = Left$(strStringWithOutNulls, intPosition - 1) &
													Right$(strStringWithOutNulls, Len(strStringWithOutNulls) - intPosition)
			End If

			If intPosition > strStringWithOutNulls.Length Then
				Exit Do
			End If
		Loop

		Return strStringWithOutNulls
	End Function


#End Region


End Class
