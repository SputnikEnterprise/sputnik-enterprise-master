
Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting



	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess


#Region "UserData"


		''' <summary>
		''' Load assigned user address data
		''' </summary>
		''' <param name="userNumber">The user number.</param>
		''' <returns>The user address data.</returns>
		Public Function LoadAssignedUserAddressData(ByVal userNumber As Integer) As UserAddressData Implements ITablesDatabaseAccess.LoadAssignedUserAddressData

			Dim result As UserAddressData = Nothing

			Dim sql As String

			sql = "IF Not EXISTS(SELECT 1 FROM US_FilialAddress WHERE USNr = @UsNr) "
			sql &= "Begin "
			sql &= "INSERT INTO US_FilialAddress ("
			sql &= "[USNr]"
			sql &= ",[MD_Name1]"
			sql &= ",[MD_Name2]"
			sql &= ",[MD_Name3]"
			sql &= ",[MD_Postfach]"
			sql &= ",[MD_Strasse]"
			sql &= ",[MD_PLZ]"
			sql &= ",[MD_Ort]"
			sql &= ",[MD_Land]"
			sql &= ",[MD_Telefon]"
			sql &= ",[MD_DTelefon]"
			sql &= ",[MD_Telefax]"
			sql &= ",[MD_EMail]"
			sql &= ",[MD_Homepage]"
			sql &= ",[CreatedOn]"

			sql &= ") Values ("

			sql &= "@UsNr"
			sql &= ",IsNull( (SELECT TOP 1 MD_Name1 FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 MD_Name2 FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 MD_Name3 FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Postfach FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Strasse FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 PLZ FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Ort FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Land FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Telefon FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ", '' "
			sql &= ",IsNull( (SELECT TOP 1 Telefax FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 EMail FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Homepage FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "

			sql &= ",GetDate()"

			sql &= ") "
			sql &= "End; "

			sql &= "SELECT Top 1 [ID]"
			sql &= ",[USNr]"
			sql &= ",[MD_Name1]"
			sql &= ",[MD_Name2]"
			sql &= ",[MD_Name3]"
			sql &= ",[MD_Postfach]"
			sql &= ",[MD_Strasse]"
			sql &= ",[MD_PLZ]"
			sql &= ",[MD_Ort]"
			sql &= ",[MD_Land]"
			sql &= ",[MD_Telefon]"
			sql &= ",[MD_DTelefon]"
			sql &= ",[MD_Telefax]"
			sql &= ",[MD_EMail]"
			sql &= ",[MD_Homepage]"
			sql &= ",[MD_Bewillig]"
			sql &= ",[CreatedFrom]"
			sql &= ",[CreatedOn]"
			sql &= ",[ChangedFrom]"
			sql &= ",[ChangedOn]"

			sql &= " FROM [dbo].[US_FilialAddress] "

			sql &= "Where "
			sql &= "UsNr = @UsNr "
			sql &= "Order By ID Desc "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("UsNr", userNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New UserAddressData
					result.USNr = SafeGetInteger(reader, "USNr", 0)
					result.MD_Name1 = SafeGetString(reader, "MD_Name1")
					result.MD_Name2 = SafeGetString(reader, "MD_Name2")
					result.MD_Name3 = SafeGetString(reader, "MD_Name3")
					result.MD_Postfach = SafeGetString(reader, "MD_Postfach")
					result.MD_Strasse = SafeGetString(reader, "MD_Strasse")
					result.MD_PLZ = SafeGetString(reader, "MD_PLZ")
					result.MD_Ort = SafeGetString(reader, "MD_Ort")
					result.MD_Land = SafeGetString(reader, "MD_Land")
					result.MD_Telefon = SafeGetString(reader, "MD_Telefon")
					result.MD_DTelefon = SafeGetString(reader, "MD_DTelefon")
					result.MD_Telefax = SafeGetString(reader, "MD_Telefax")
					result.MD_eMail = SafeGetString(reader, "MD_eMail")
					result.MD_Homepage = SafeGetString(reader, "MD_Homepage")
					result.MD_Bewilligung = SafeGetString(reader, "MD_Bewillig")

					result.recid = SafeGetInteger(reader, "ID", Nothing)
					result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.ChangedFrom = SafeGetString(reader, "ChangedFrom")
					result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' update assigned user address data.
		''' </summary>
		''' <returns>boolean</returns>
		Public Function UpdateAssignedUserAddressData(ByVal data As UserAddressData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedUserAddressData

			Dim success As Boolean = True

			Dim sql As String

			sql = "IF Not EXISTS(SELECT 1 FROM US_FilialAddress WHERE USNr = @USNr) "
			sql &= "Begin "
			sql &= "INSERT INTO US_FilialAddress ("
			sql &= "[USNr]"
			sql &= ",[MD_Name1]"
			sql &= ",[MD_Name2]"
			sql &= ",[MD_Name3]"
			sql &= ",[MD_Postfach]"
			sql &= ",[MD_Strasse]"
			sql &= ",[MD_PLZ]"
			sql &= ",[MD_Ort]"
			sql &= ",[MD_Land]"
			sql &= ",[MD_Telefon]"
			sql &= ",[MD_DTelefon]"
			sql &= ",[MD_Telefax]"
			sql &= ",[MD_EMail]"
			sql &= ",[MD_Homepage]"
			sql &= ",[CreatedOn]"

			sql &= ") Values ("

			sql &= "@UsNr"
			sql &= ",IsNull( (SELECT TOP 1 MD_Name1 FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 MD_Name2 FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 MD_Name3 FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Postfach FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Strasse FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 PLZ FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Ort FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Land FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Telefon FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ", '' "
			sql &= ",IsNull( (SELECT TOP 1 Telefax FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 EMail FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "
			sql &= ",IsNull( (SELECT TOP 1 Homepage FROM Mandanten WHERE MDNr = IsNull( (Select Top 1 MDNr From Benutzer Where USNr = @UsNr), 0) ORDER BY ID Desc), '') "

			sql &= ",GetDate()"

			sql &= ") "
			sql &= "End; "

			sql &= "Update US_FilialAddress Set "
			sql &= "[USNr] = @UsNr"
			sql &= ",[MD_Name1] = @MD_Name1"
			sql &= ",[MD_Name2] = @MD_Name2"
			sql &= ",[MD_Name3] = @MD_Name3"
			sql &= ",[MD_Postfach] = @MD_Postfach"
			sql &= ",[MD_Strasse] = @MD_Strasse"
			sql &= ",[MD_PLZ] = @MD_PLZ"
			sql &= ",[MD_Ort] = @MD_Ort"
			sql &= ",[MD_Land] = @MD_Land"
			sql &= ",[MD_Telefon] = @MD_Telefon"
			sql &= ",[MD_DTelefon] = @MD_DTelefon"
			sql &= ",[MD_Telefax] = @MD_Telefax"
			sql &= ",[MD_eMail] = @MD_eMail"
			sql &= ",[MD_Homepage] = @MD_Homepage"
			sql &= ",[MD_Bewillig] = @MD_Bewillig"
			sql &= ",[ChangedFrom] = @ChangedFrom"
			sql &= ",[ChangedOn] = GetDate()"

			sql &= " Where [ID] = @recid "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", data.recid))

			listOfParams.Add(New SqlClient.SqlParameter("USNr", data.USNr))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Name1", ReplaceMissing(data.MD_Name1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Name2", ReplaceMissing(data.MD_Name2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Name3", ReplaceMissing(data.MD_Name3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Postfach", ReplaceMissing(data.MD_Postfach, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Strasse", ReplaceMissing(data.MD_Strasse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_PLZ", ReplaceMissing(data.MD_PLZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Ort", ReplaceMissing(data.MD_Ort, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Land", ReplaceMissing(data.MD_Land, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Telefon", ReplaceMissing(data.MD_Telefon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_DTelefon", ReplaceMissing(data.MD_DTelefon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Telefax", ReplaceMissing(data.MD_Telefax, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_eMail", ReplaceMissing(data.MD_eMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Homepage", ReplaceMissing(data.MD_Homepage, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Bewillig", ReplaceMissing(data.MD_Bewilligung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function


		''' <summary>
		''' add user address data.
		''' </summary>
		''' <returns>boolean.</returns>
		Public Function AddUserAddressData(ByVal userrecID As Integer, ByVal data As UserAddressData) As Boolean Implements ITablesDatabaseAccess.AddUserAddressData

			Dim success As Boolean = True

			Dim sql As String


			sql = "INSERT INTO US_FilialAddress ("
			sql &= "[USNr]"
			sql &= ",[MD_Name1]"
			sql &= ",[MD_Name2]"
			sql &= ",[MD_Name3]"
			sql &= ",[MD_Postfach]"
			sql &= ",[MD_Strasse]"
			sql &= ",[MD_PLZ]"
			sql &= ",[MD_Ort]"
			sql &= ",[MD_Land]"
			sql &= ",[MD_Telefon]"
			sql &= ",[MD_DTelefon]"
			sql &= ",[MD_Telefax]"
			sql &= ",[MD_eMail]"
			sql &= ",[MD_Homepage]"
			sql &= ",[MD_Bewillig]"
			sql &= ",[CreatedFrom]"
			sql &= ",[CreatedOn]"

			sql &= ") Values ("

			sql &= "IsNull( (Select Top 1 USNr From Benutzer Where ID = @UserrecID), 0) "
			sql &= ",@MD_Name1"
			sql &= ",@MD_Name2"
			sql &= ",@MD_Name3"
			sql &= ",@MD_Postfach"
			sql &= ",@MD_Strasse"
			sql &= ",@MD_PLZ"
			sql &= ",@MD_Ort"
			sql &= ",@MD_Land"
			sql &= ",@MD_Telefon"
			sql &= ",@MD_DTelefon"
			sql &= ",@MD_Telefax"
			sql &= ",@MD_eMail"
			sql &= ",@MD_Homepage"
			sql &= ",@MD_Bewillig"
			sql &= ",@CreatedFrom"
			sql &= ",GetDate()"

			sql &= ") "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("UserrecID", userrecID))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Name1", ReplaceMissing(data.MD_Name1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Name2", ReplaceMissing(data.MD_Name2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Name3", ReplaceMissing(data.MD_Name3, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Postfach", ReplaceMissing(data.MD_Postfach, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Strasse", ReplaceMissing(data.MD_Strasse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_PLZ", ReplaceMissing(data.MD_PLZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Ort", ReplaceMissing(data.MD_Ort, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Land", ReplaceMissing(data.MD_Land, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Telefon", ReplaceMissing(data.MD_Telefon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_DTelefon", ReplaceMissing(data.MD_DTelefon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Telefax", ReplaceMissing(data.MD_Telefax, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_eMail", ReplaceMissing(data.MD_eMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Homepage", ReplaceMissing(data.MD_Homepage, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Bewillig", ReplaceMissing(data.MD_Bewilligung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function



		''' <summary>
		''' Load assigned user data
		''' </summary>
		''' <param name="recid">The Rec number.</param>
		''' <returns>The user data.</returns>
		Public Function LoadAssignedUserData(ByVal recid As Integer) As UserData Implements ITablesDatabaseAccess.LoadAssignedUserData

			Dim result As UserData = Nothing

			Dim sql As String

			sql = "[Get UserData For User]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("RecID", ReplaceMissing(recid, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New UserData
					result.USNr = SafeGetInteger(reader, "USNr", 0)
					result.US_Name = SafeGetString(reader, "US_Name")
					result.Nachname = SafeGetString(reader, "Nachname")
					result.Vorname = SafeGetString(reader, "Vorname")
					result.Anrede = SafeGetString(reader, "Anrede")
					result.PW = SafeGetString(reader, "PW")
					result.Postfach = SafeGetString(reader, "Postfach")
					result.Strasse = SafeGetString(reader, "Strasse")
					result.PLZ = SafeGetString(reader, "PLZ")
					result.Ort = SafeGetString(reader, "Ort")
					result.Land = SafeGetString(reader, "Land")
					result.Telefon = SafeGetString(reader, "Telefon")
					result.Telefax = SafeGetString(reader, "Telefax")
					result.Natel = SafeGetString(reader, "Natel")
					result.eMail = SafeGetString(reader, "eMail")
					result.Homepage = SafeGetString(reader, "Homepage")
					result.Abteilung = SafeGetString(reader, "Abteilung")
					result.GebDat = SafeGetDateTime(reader, "GebDat", Nothing)
					result.Sprache = SafeGetString(reader, "Sprache")
					result.KST = SafeGetString(reader, "KST")
					result.SecLevel = SafeGetInteger(reader, "SecLevel", 0)
					result.Logged = SafeGetBoolean(reader, "Logged", Nothing)
					result.Deaktiviert = SafeGetBoolean(reader, "Deaktiviert", False)
					result.Result = SafeGetString(reader, "Result")
					result.USKst1 = SafeGetString(reader, "USKst1")
					result.USKst2 = SafeGetString(reader, "USKst2")
					result.PlanerDb = SafeGetString(reader, "PlanerDb")
					result.AktivUntil = SafeGetDateTime(reader, "AktivUntil", Nothing)
					result.recid = SafeGetInteger(reader, "ID", Nothing)
					result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.ChangedFrom = SafeGetString(reader, "ChangedFrom")
					result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					result.USBild = SafeGetByteArray(reader, "USBild")
					result.USSign = SafeGetByteArray(reader, "USSign")
					result.USFiliale = SafeGetString(reader, "USFiliale")
					result.USLanguage = SafeGetString(reader, "USLanguage")
					result.USTitel_1 = SafeGetString(reader, "USTitel_1")
					result.USTitel_2 = SafeGetString(reader, "USTitel_2")
					result.Transfered_Guid = SafeGetString(reader, "Transfered_Guid")
					result.EMail_UserName = SafeGetString(reader, "EMail_UserName")
					result.EMail_UserPW = SafeGetString(reader, "EMail_UserPW")
					result.USRightsTemplate = SafeGetString(reader, "USRightsTemplate")
					result.MDNr = SafeGetInteger(reader, "MDNr", 0)
					result.jch_layoutID = SafeGetInteger(reader, "jch_layoutID", 0)
					result.jch_logoID = SafeGetInteger(reader, "jch_logoID", 0)
					result.OstJob_ID = SafeGetString(reader, "OstJob_ID")
					result.ostjob_Kontingent = SafeGetInteger(reader, "ostjob_Kontingent", 0)
					result.JCH_SubID = SafeGetInteger(reader, "JCH_SubID", 0)
					result.AsCostCenter = SafeGetBoolean(reader, "AsCostCenter", 0)
					result.LogonMorePlaces = SafeGetBoolean(reader, "LogonMorePlaces", 0)

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function


		''' <summary>
		''' Loads all user Data
		''' </summary>
		''' <returns>The user data.</returns>
		Public Function LoadUserData(ByVal userNumber As Integer?) As IEnumerable(Of UserData) Implements ITablesDatabaseAccess.LoadUserData

			Dim result As List(Of UserData) = Nothing

			Dim sql As String


			sql = "SELECT [USNr]"
			sql &= ",[US_Name]"
			sql &= ",LTRIM(RTRIM([Nachname])) Nachname"
			sql &= ",LTRIM(RTRIM([Vorname])) Vorname"
			sql &= ",[Anrede]"
			sql &= ",[PW]"
			sql &= ",[Postfach]"
			sql &= ",[Strasse]"
			sql &= ",[PLZ]"
			sql &= ",[Ort]"
			sql &= ",[Land]"
			sql &= ",[Telefon]"
			sql &= ",[Telefax]"
			sql &= ",[Natel]"
			sql &= ",[eMail]"
			sql &= ",[Homepage]"
			sql &= ",[Abteilung]"
			sql &= ",[GebDat]"
			sql &= ",[Sprache]"
			sql &= ",[KST]"
			sql &= ",Convert(int, [SecLevel]) SecLevel "
			sql &= ",[Logged]"
			sql &= ",[Deaktiviert]"
			sql &= ",[Result]"
			sql &= ",[USKst1]"
			sql &= ",[USKst2]"
			sql &= ",[PlanerDb]"
			sql &= ",[AktivUntil]"
			sql &= ",[ID]"
			sql &= ",[CreatedFrom]"
			sql &= ",[CreatedOn]"
			sql &= ",[ChangedFrom]"
			sql &= ",[ChangedOn]"
			sql &= ",[USBild]"
			sql &= ",[USSign]"
			sql &= ",[USFiliale]"
			sql &= ",[USLanguage]"
			sql &= ",[USTitel_1]"
			sql &= ",[USTitel_2]"
			sql &= ",[Transfered_Guid]"
			sql &= ",[EMail_UserName]"
			sql &= ",[EMail_UserPW]"
			sql &= ",[USRightsTemplate]"
			sql &= ",[MDNr]"
			sql &= ",[jch_layoutID]"
			sql &= ",[jch_logoID]"
			sql &= ",[OstJob_ID]"
			sql &= ",[ostjob_Kontingent]"
			sql &= ",[JCH_SubID]"
			sql &= ",[AsCostCenter]"
			sql &= ",[LogonMorePlaces]"

			sql &= " FROM [dbo].[Benutzer] "

			sql &= "Where (USNr = @userNumber Or 0 = @userNumber) "

			sql &= "Order By Nachname, Vorname "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("userNumber", ReplaceMissing(userNumber, 0)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of UserData)

					While reader.Read

						Dim data = New UserData()

						data.USNr = SafeGetInteger(reader, "USNr", 0)
						data.US_Name = SafeGetString(reader, "US_Name")
						data.Nachname = SafeGetString(reader, "Nachname")
						data.Vorname = SafeGetString(reader, "Vorname")
						data.Anrede = SafeGetString(reader, "Anrede")
						data.PW = SafeGetString(reader, "PW")
						data.Postfach = SafeGetString(reader, "Postfach")
						data.Strasse = SafeGetString(reader, "Strasse")
						data.PLZ = SafeGetString(reader, "PLZ")
						data.Ort = SafeGetString(reader, "Ort")
						data.Land = SafeGetString(reader, "Land")
						data.Telefon = SafeGetString(reader, "Telefon")
						data.Telefax = SafeGetString(reader, "Telefax")
						data.Natel = SafeGetString(reader, "Natel")
						data.eMail = SafeGetString(reader, "eMail")
						data.Homepage = SafeGetString(reader, "Homepage")
						data.Abteilung = SafeGetString(reader, "Abteilung")
						data.GebDat = SafeGetDateTime(reader, "GebDat", Nothing)
						data.Sprache = SafeGetString(reader, "Sprache")
						data.KST = SafeGetString(reader, "KST")
						data.SecLevel = SafeGetInteger(reader, "SecLevel", 0)
						data.Logged = SafeGetBoolean(reader, "Logged", Nothing)
						data.Deaktiviert = SafeGetBoolean(reader, "Deaktiviert", False)
						data.Result = SafeGetString(reader, "Result")
						data.USKst1 = SafeGetString(reader, "USKst1")
						data.USKst2 = SafeGetString(reader, "USKst2")
						data.PlanerDb = SafeGetString(reader, "PlanerDb")
						data.AktivUntil = SafeGetDateTime(reader, "AktivUntil", Nothing)
						data.recid = SafeGetInteger(reader, "ID", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.USBild = SafeGetByteArray(reader, "USBild")
						data.USSign = SafeGetByteArray(reader, "USSign")
						data.USFiliale = SafeGetString(reader, "USFiliale")
						data.USLanguage = SafeGetString(reader, "USLanguage")
						data.USTitel_1 = SafeGetString(reader, "USTitel_1")
						data.USTitel_2 = SafeGetString(reader, "USTitel_2")
						data.Transfered_Guid = SafeGetString(reader, "Transfered_Guid")
						data.EMail_UserName = SafeGetString(reader, "EMail_UserName")
						data.EMail_UserPW = SafeGetString(reader, "EMail_UserPW")
						data.USRightsTemplate = SafeGetString(reader, "USRightsTemplate")
						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.jch_layoutID = SafeGetInteger(reader, "jch_layoutID", 0)
						data.jch_logoID = SafeGetInteger(reader, "jch_logoID", 0)
						data.OstJob_ID = SafeGetString(reader, "OstJob_ID")
						data.ostjob_Kontingent = SafeGetInteger(reader, "ostjob_Kontingent", 0)
						data.JCH_SubID = SafeGetInteger(reader, "JCH_SubID", 0)
						data.AsCostCenter = SafeGetBoolean(reader, "AsCostCenter", 0)
						data.LogonMorePlaces = SafeGetBoolean(reader, "LogonMorePlaces", 0)


						result.Add(data)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' update assigned user data.
		''' </summary>
		''' <returns>boolean</returns>
		Public Function UpdateAssignedUserData(ByVal data As UserData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedUserData

			Dim success As Boolean = True

			Dim sql As String


			sql = "Update Benutzer Set "
			sql &= "[USNr] = @UsNr"
			sql &= ",[US_Name] = @US_Name"
			sql &= ",[Nachname] = @Nachname"
			sql &= ",[Vorname] = @Vorname"
			sql &= ",[Anrede] = @Anrede"
			sql &= ",[PW] = @PW"
			sql &= ",[Postfach] = @Postfach"
			sql &= ",[Strasse] = @Strasse"
			sql &= ",[PLZ] = @PLZ"
			sql &= ",[Ort] = @Ort"
			sql &= ",[Land] = @Land"
			sql &= ",[Telefon] = @Telefon"
			sql &= ",[Telefax] = @Telefax"
			sql &= ",[Natel] = @Natel"
			sql &= ",[eMail] = @eMail"
			sql &= ",[Homepage] = @Homepage"
			sql &= ",[Abteilung] = @Abteilung"
			sql &= ",[GebDat] = @GebDat"
			sql &= ",[Sprache] = @Sprache"
			sql &= ",[KST] = @KST"
			sql &= ",[SecLevel] = @SecLevel"
			sql &= ",[Logged] = @Logged"
			sql &= ",[Deaktiviert] = @Deaktiviert"
			sql &= ",[Result] = @Result"
			sql &= ",[USKst1] = @USKst1"
			sql &= ",[USKst2] = @USKst2"
			sql &= ",[PlanerDb] = @PlanerDb"
			sql &= ",[AktivUntil] = @AktivUntil"
			sql &= ",[ChangedFrom] = @ChangedFrom"
			sql &= ",[ChangedOn] = GetDate()"
			sql &= ",[USFiliale] = @USFiliale"
			sql &= ",[USLanguage] = @USLanguage"
			sql &= ",[USTitel_1] = @USTitel_1"
			sql &= ",[USTitel_2] = @USTitel_2"
			sql &= ",[Transfered_Guid] = @Transfered_Guid"
			sql &= ",[EMail_UserName] = @EMail_UserName"
			sql &= ",[EMail_UserPW] = @EMail_UserPW"
			sql &= ",[USRightsTemplate] = @USRightsTemplate"
			sql &= ",[MDNr] = @MDNr"
			sql &= ",[jch_layoutID] = @jch_layoutID"
			sql &= ",[jch_logoID] = @jch_logoID"
			sql &= ",[OstJob_ID] = @OstJob_ID"
			sql &= ",[ostjob_Kontingent] = @ostjob_Kontingent"
			sql &= ",[JCH_SubID] = @JCH_SubID"
			sql &= ",[AsCostCenter] = @AsCostCenter"
			sql &= ",[LogonMorePlaces] = @LogonMorePlaces"

			sql &= " Where [ID] = @recid; "

			sql &= " Delete US_Filiale Where USNr = @USNr; "
			If Not String.IsNullOrWhiteSpace(data.USFiliale) Then
				sql &= " Insert Into US_Filiale (USNr, Bezeichnung) Values (@USNr, @USFiliale); "
			End If


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", data.recid))

			listOfParams.Add(New SqlClient.SqlParameter("USNr", data.USNr))
			listOfParams.Add(New SqlClient.SqlParameter("US_Name", ReplaceMissing(data.US_Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Nachname", data.Nachname.ToString.Trim))
			listOfParams.Add(New SqlClient.SqlParameter("Vorname", data.Vorname.ToString.Trim))
			listOfParams.Add(New SqlClient.SqlParameter("Anrede", data.Anrede))
			listOfParams.Add(New SqlClient.SqlParameter("PW", data.PW))
			listOfParams.Add(New SqlClient.SqlParameter("Postfach", data.Postfach))
			listOfParams.Add(New SqlClient.SqlParameter("Strasse", data.Strasse))
			listOfParams.Add(New SqlClient.SqlParameter("PLZ", data.PLZ))
			listOfParams.Add(New SqlClient.SqlParameter("Ort", data.Ort))
			listOfParams.Add(New SqlClient.SqlParameter("Land", data.Land))
			listOfParams.Add(New SqlClient.SqlParameter("Telefon", data.Telefon))
			listOfParams.Add(New SqlClient.SqlParameter("Telefax", data.Telefax))
			listOfParams.Add(New SqlClient.SqlParameter("Natel", data.Natel))
			listOfParams.Add(New SqlClient.SqlParameter("eMail", data.eMail))
			listOfParams.Add(New SqlClient.SqlParameter("Homepage", data.Homepage))
			listOfParams.Add(New SqlClient.SqlParameter("Abteilung", data.Abteilung))
			listOfParams.Add(New SqlClient.SqlParameter("GebDat", ReplaceMissing(data.GebDat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Sprache", data.Sprache))
			listOfParams.Add(New SqlClient.SqlParameter("KST", data.KST.ToString.Trim))
			listOfParams.Add(New SqlClient.SqlParameter("SecLevel", ReplaceMissing(data.SecLevel, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Logged", ReplaceMissing(data.Logged, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Deaktiviert", ReplaceMissing(data.Deaktiviert, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Result", ReplaceMissing(data.Result, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USKst1", ReplaceMissing(data.USKst1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USKst2", ReplaceMissing(data.USKst2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PlanerDb", ReplaceMissing(data.PlanerDb, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AktivUntil", ReplaceMissing(data.AktivUntil, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", data.ChangedFrom))
			listOfParams.Add(New SqlClient.SqlParameter("USFiliale", ReplaceMissing(data.USFiliale, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USLanguage", ReplaceMissing(data.USLanguage, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USTitel_1", ReplaceMissing(data.USTitel_1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USTitel_2", ReplaceMissing(data.USTitel_2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Transfered_Guid", ReplaceMissing(data.Transfered_Guid, Guid.NewGuid.ToString)))
			listOfParams.Add(New SqlClient.SqlParameter("EMail_UserName", ReplaceMissing(data.EMail_UserName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMail_UserPW", ReplaceMissing(data.EMail_UserPW, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USRightsTemplate", ReplaceMissing(data.USRightsTemplate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(data.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jch_layoutID", ReplaceMissing(data.jch_layoutID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("jch_logoID", ReplaceMissing(data.jch_logoID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OstJob_ID", ReplaceMissing(data.OstJob_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ostjob_Kontingent", ReplaceMissing(data.ostjob_Kontingent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JCH_SubID", ReplaceMissing(data.JCH_SubID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AsCostCenter", ReplaceMissing(data.AsCostCenter, False)))
			listOfParams.Add(New SqlClient.SqlParameter("LogonMorePlaces", ReplaceMissing(data.LogonMorePlaces, False)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function


		''' <summary>
		''' update assigned user data.
		''' </summary>
		''' <returns>boolean</returns>
		Public Function UpdateAssignedUserPictureAndSignData(ByVal data As UserData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedUserPictureAndSignData

			Dim success As Boolean = True

			Dim sql As String


			sql = "Update Benutzer Set "
			sql &= "[USBild] = @USBild"
			sql &= ",[USSign] = @USSign"

			sql &= " Where [ID] = @recid "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", data.recid))

			Dim pictureBytesParameter = New SqlClient.SqlParameter("@USBild", DbType.Binary, If(data.USBild Is Nothing, 0, data.USBild.Length))
			pictureBytesParameter.Value = If(data.USBild.Length = 0, DBNull.Value, data.USBild)
			listOfParams.Add(pictureBytesParameter)

			Dim signBytesParameter = New SqlClient.SqlParameter("@USSign", DbType.Binary, If(data.USSign Is Nothing, 0, data.USSign.Length))
			signBytesParameter.Value = If(data.USSign.Length = 0, DBNull.Value, data.USSign)
			listOfParams.Add(signBytesParameter)

			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function



		''' <summary>
		''' add user data.
		''' </summary>
		''' <returns>boolean.</returns>
		Public Function AddUserData(ByVal data As UserData) As Boolean Implements ITablesDatabaseAccess.AddUserData

			Dim success As Boolean = True

			Dim sql As String = "[Create New User]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("US_Name", ReplaceMissing(data.US_Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Nachname", ReplaceMissing(data.Nachname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Vorname", ReplaceMissing(data.Vorname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Anrede", ReplaceMissing(data.Anrede, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PW", ReplaceMissing(data.PW, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Postfach", ReplaceMissing(data.Postfach, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Strasse", ReplaceMissing(data.Strasse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PLZ", ReplaceMissing(data.PLZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Ort", ReplaceMissing(data.Ort, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Land", ReplaceMissing(data.Land, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Telefon", ReplaceMissing(data.Telefon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Telefax", ReplaceMissing(data.Telefax, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Natel", ReplaceMissing(data.Natel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("eMail", ReplaceMissing(data.eMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Homepage", ReplaceMissing(data.Homepage, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Abteilung", ReplaceMissing(data.Abteilung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("GebDat", ReplaceMissing(data.GebDat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Sprache", ReplaceMissing(data.Sprache, "deutsch")))
			listOfParams.Add(New SqlClient.SqlParameter("KST", ReplaceMissing(data.KST, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SecLevel", ReplaceMissing(data.SecLevel, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Result", ReplaceMissing(data.Result, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USKst1", ReplaceMissing(data.USKst1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USKst2", ReplaceMissing(data.USKst2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PlanerDb", ReplaceMissing(data.PlanerDb, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AktivUntil", ReplaceMissing(data.AktivUntil, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", data.ChangedFrom))
			listOfParams.Add(New SqlClient.SqlParameter("USBild", ReplaceMissing(data.USBild, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USSign", ReplaceMissing(data.USSign, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USFiliale", ReplaceMissing(data.USFiliale, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USLanguage", ReplaceMissing(data.USLanguage, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USTitel_1", ReplaceMissing(data.USTitel_1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USTitel_2", ReplaceMissing(data.USTitel_2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMail_UserName", ReplaceMissing(data.EMail_UserName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMail_UserPW", ReplaceMissing(data.EMail_UserPW, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USRightsTemplate", ReplaceMissing(data.USRightsTemplate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(data.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("jch_layoutID", ReplaceMissing(data.jch_layoutID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("jch_logoID", ReplaceMissing(data.jch_logoID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OstJob_ID", ReplaceMissing(data.OstJob_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ostjob_Kontingent", ReplaceMissing(data.ostjob_Kontingent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JCH_SubID", ReplaceMissing(data.JCH_SubID, DBNull.Value)))


			Try
				' New ID of user
				Dim newIdParameter = New SqlClient.SqlParameter("NewUSERID", SqlDbType.Int)
				newIdParameter.Direction = ParameterDirection.Output
				listOfParams.Add(newIdParameter)

				' New number of user
				Dim newNumberParameter = New SqlClient.SqlParameter("NewUSERNR", SqlDbType.Int)
				newNumberParameter.Direction = ParameterDirection.Output
				listOfParams.Add(newNumberParameter)


				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If success Then
					If Not newIdParameter.Value Is Nothing Then
						data.recid = CType(newIdParameter.Value, Integer)
					End If
					If Not newNumberParameter.Value Is Nothing Then
						data.USNr = CType(newNumberParameter.Value, Integer)
					End If

				Else
					success = False
				End If

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

		''' <summary>
		''' Delete user data.
		''' </summary>
		''' <param name="recid">The id of record.</param>
		''' <param name="operatorNumber">The number of operator.</param>
		Public Function DeleteUserData(ByVal recid As Integer?, ByVal operatorNumber As Integer) As DeleteUserResult Implements ITablesDatabaseAccess.DeleteUserData

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Delete UserData]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", ReplaceMissing(recid, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("operatorNr", ReplaceMissing(operatorNumber, 0)))


			Dim resultParameter = New SqlClient.SqlParameter("@result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim resultEnum As DeleteUserResult

			If Not resultParameter.Value Is Nothing Then
				Try
					resultEnum = CType(resultParameter.Value, DeleteUserResult)
				Catch
					resultEnum = DeleteUserResult.ResultDeleteError
				End Try
			Else
				resultEnum = DeleteUserResult.ResultDeleteError
			End If

			Return resultEnum

		End Function


#End Region




#Region "Rights group"

		''' <summary>
		''' Load assigned rights group data
		''' </summary>
		''' <param name="recid">The Rec number.</param>
		''' <returns>The user data.</returns>
		Public Function LoadAssignedRightsData(ByVal recid As Integer) As RightsData Implements ITablesDatabaseAccess.LoadAssignedRightsData

			Dim result As RightsData = Nothing

			Dim sql As String


			sql = "SELECT ID, Bezeichnung, RightProc "
			sql &= "FROM [dbo].[tblTpl_USRights] "

			sql &= "Where "
			sql &= "ID = @recid "



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", recid))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New RightsData
					result.recid = SafeGetInteger(reader, "id", 0)
					result.Bezeichnung = SafeGetString(reader, "Bezeichnung")
					result.RightProc = SafeGetString(reader, "RightProc")


				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		''' <summary>
		''' Loads all rights group
		''' </summary>
		''' <returns>The rights data.</returns>
		Public Function LoadRightsData() As IEnumerable(Of RightsData) Implements ITablesDatabaseAccess.LoadRightsData

			Dim result As List(Of RightsData) = Nothing

			Dim sql As String


			sql = "SELECT ID, Bezeichnung, RightProc "
			sql &= "FROM [dbo].[tblTpl_USRights] "
			sql &= "Order By ID"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RightsData)

					While reader.Read

						Dim data = New RightsData()

						data.recid = SafeGetInteger(reader, "id", 0)
						data.Bezeichnung = SafeGetString(reader, "Bezeichnung")
						data.RightProc = SafeGetString(reader, "RightProc")


						result.Add(data)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function


		''' <summary>
		''' create new user rights with template.
		''' </summary>
		''' <returns>boolean.</returns>
		Public Function SaveUSRightsWithSelectedTemplates(ByVal mandantenNumber As Integer, ByVal userNumber As Integer, ByVal rightProc As String) As Boolean Implements ITablesDatabaseAccess.SaveUSRightsWithSelectedTemplates

			Dim success As Boolean = True

			Dim sql As String = rightProc


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mandantenNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNumber", ReplaceMissing(userNumber, DBNull.Value)))


			Try

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function


#End Region



#Region "user rights"


		''' <summary>
		''' Loads all rights group
		''' </summary>
		''' <returns>The rights data.</returns>
		Public Function LoadAssignedUserRightsData(ByVal userNumber As Integer, ByVal mandantenNumber As Integer, ByVal ModulName As String) As IEnumerable(Of UserRightData) Implements ITablesDatabaseAccess.LoadAssignedUserRightsData

			Dim result As List(Of UserRightData) = Nothing

			Dim sql As String


			sql = "SELECT ID, USNr, MDNr, SecNr, CONVERT(BIT, Autorized) Autorized, SecNrBez, ModulName, "
			sql &= "ChangedOn, ChangedFrom "
			sql &= "FROM [dbo].[USSecLevel] "

			sql &= "Where MDNr = @MDNr "
			sql &= "AND USNr = @USNr "
			sql &= "AND (@ModulName = '' OR ModulName = @ModulName) "

			sql &= "Order By MDNr, SecNr, ModulName"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mandantenNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(userNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ModulName", ReplaceMissing(ModulName, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of UserRightData)

					While reader.Read

						Dim data = New UserRightData()

						data.recid = SafeGetInteger(reader, "id", 0)
						data.USNr = SafeGetInteger(reader, "USNr", 0)
						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.SecNr = SafeGetInteger(reader, "SecNr", 0)
						data.Autorized = SafeGetBoolean(reader, "Autorized", False)
						data.SecNrBez = SafeGetString(reader, "SecNrBez")
						data.ModulName = SafeGetString(reader, "ModulName")
						If data.SecNr = 665 Then data.UserRightsChecked = data.Autorized
						If data.SecNr = 672 Then data.AllowedToSeeAllData = data.Autorized

						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")

						result.Add(data)

					End While

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function

		''' <summary>
		''' update user rights.
		''' </summary>
		''' <returns>boolean.</returns>
		Function UpdateAssignedUserRightsData(ByVal data As UserRightData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedUserRightsData

			Dim success As Boolean = True

			Dim sql As String

			sql = "Update [dbo].[USSecLevel] Set "
			sql &= "USNr = @USNr,"
			sql &= "MDNr = @MDNr,"
			sql &= "Autorized = @Autorized,"
			sql &= "ChangedOn = GetDate(),"
			sql &= "ChangedFrom = @ChangedFrom"

			sql &= " Where [ID] = @recid "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("recid", ReplaceMissing(data.recid, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(data.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(data.USNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SecNr", ReplaceMissing(data.SecNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Autorized", ReplaceMissing(data.Autorized, False)))
			listOfParams.Add(New SqlClient.SqlParameter("SecNrBez", ReplaceMissing(data.SecNrBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ModulName", ReplaceMissing(data.ModulName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))


			Try

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

		Function DeleteAssignedUserRightsData(ByVal data As UserRightData, ByVal userName As String, ByVal usnr As Integer, ByVal deleteAllUsers As Boolean) As Boolean Implements ITablesDatabaseAccess.DeleteAssignedUserRightsData

			Dim success As Boolean = True

			Dim sql As String

			If deleteAllUsers Then
				sql = "[Delete Assinged SecNumber For All User Data]"

			Else
				sql = "[Delete Selected USSecLevel Data]"
			End If


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("ID", data.recid))
			listOfParams.Add(New SqlClient.SqlParameter("Modul", "USSecLevel"))
			listOfParams.Add(New SqlClient.SqlParameter("Username", ReplaceMissing(userName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Usnr", ReplaceMissing(usnr, DBNull.Value)))


			Dim resultParameter = New SqlClient.SqlParameter("@result", SqlDbType.Int)
			resultParameter.Direction = ParameterDirection.Output
			listOfParams.Add(resultParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Dim resultEnum As DeleteUserRightsResult

			If Not resultParameter.Value Is Nothing Then
				Try
					resultEnum = CType(resultParameter.Value, DeleteUserRightsResult)
					success = (resultEnum = DeleteUserRightsResult.ResultDeleteOk)

				Catch
					resultEnum = DeleteUserRightsResult.ResultDeleteError
					success = False
				End Try
			Else
				resultEnum = DeleteUserRightsResult.ResultDeleteError
				success = False
			End If

			Return success

		End Function

		''' <summary>
		''' add new user rights for all users.
		''' </summary>
		''' <returns>boolean.</returns>
		Public Function AddAssignedRightsForAllUsersData(ByVal data As UserRightData) As Boolean Implements ITablesDatabaseAccess.AddAssignedRightsForAllUsersData

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Add New User rightData For All Users]"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(data.USNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SecNr", ReplaceMissing(data.SecNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SecNrBez", ReplaceMissing(data.SecNrBez, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ModulName", ReplaceMissing(data.ModulName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Autorized", ReplaceMissing(data.Autorized, False)))


			Try

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

		''' <summary>
		''' update user rights for all users.
		''' </summary>
		''' <returns>boolean.</returns>
		Public Function UpdateUserRightsForAllUserData(ByVal data As UserRightData) As Boolean Implements ITablesDatabaseAccess.UpdateUserRightsForAllUserData

			Dim success As Boolean = True

			Dim sql As String

			sql = "Update [dbo].[USSecLevel] Set "
			sql &= "Autorized = @Autorized,"
			sql &= "ChangedOn = GetDate(),"
			sql &= "ChangedFrom = @ChangedFrom"

			sql &= " Where USNr <> 1 And MDNr = @MDNr And SecNr = @SecNr And ModulName = @ModulName"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(data.MDNr, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Autorized", ReplaceMissing(data.Autorized, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ModulName", ReplaceMissing(data.ModulName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("SecNr", ReplaceMissing(data.SecNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))


			Try

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function


		''' <summary>
		''' add user rights.
		''' </summary>
		''' <returns>boolean.</returns>
		Public Function CopyUserRightsFromAnotherUser(ByVal newUserNumber As Integer, ByVal data As UserRightData) As Boolean Implements ITablesDatabaseAccess.CopyUserRightsFromAnotherUser

			Dim success As Boolean = True

			Dim sql As String

			sql = "Insert Into [dbo].[USSecLevel] ("
			sql &= "USNr,"
			sql &= "MDNr,"
			sql &= "ModulName,"
			sql &= "SecNr,"
			sql &= "SecNrBez,"
			sql &= "Autorized,"
			sql &= "ChangedOn,"
			sql &= "ChangedFrom"

			sql &= ") Values ("

			sql &= "@USNr,"
			sql &= "@MDNr,"
			sql &= "IsNull( (Select ModulName From [dbo].[USSecLevel] Where ID = @recid), '') ,"
			sql &= "IsNull( (Select SecNr From [dbo].[USSecLevel] Where ID = @recid), 0) ,"
			sql &= "IsNull( (Select SecNrBez From [dbo].[USSecLevel] Where ID = @recid), '') ,"
			sql &= "IsNull( (Select Autorized From [dbo].[USSecLevel] Where ID = @recid), 0) ,"
			sql &= "GetDate(),"
			sql &= "@ChangedFrom)"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("recid", ReplaceMissing(data.recid, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(data.MDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(newUserNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(data.ChangedFrom, DBNull.Value)))


			Try

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

		''' <summary>
		''' add user rights.
		''' </summary>
		''' <returns>boolean.</returns>
		Function CopyUserRightsFromMainMandantToAnotherSubMandant(ByVal mandantFrom As Integer, ByVal mandantTo As Integer, ByVal userNumber As Integer) As Boolean Implements ITablesDatabaseAccess.CopyUserRightsFromMainMandantToAnotherSubMandant

			Dim success As Boolean = True

			Dim sql As String

			sql = "[Copy User Rights From One Mandant to another SubMandant]"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("USNumber", ReplaceMissing(userNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNrFrom", ReplaceMissing(mandantFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MDNrTo", ReplaceMissing(mandantTo, DBNull.Value)))


			Try

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function

#End Region



	End Class


End Namespace

