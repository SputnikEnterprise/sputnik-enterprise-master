Imports SP.DatabaseAccess.Common.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace Common


	Partial Class CommonDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICommonDatabaseAccess


		''' <summary>
		''' Loads mandant list.
		''' </summary>
		''' <returns>List of mdant data.</returns>
		Function LoadCompaniesListData() As IEnumerable(Of MandantData) Implements ICommonDatabaseAccess.LoadCompaniesListData
			Dim result As List(Of MandantData) = Nothing
			Dim m_md As New Mandant

			Dim sql As String = "[Mandanten. Get All Allowed MDData]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.StoredProcedure)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of MandantData)

					While reader.Read()
						Dim recData As New MandantData

						recData.MandantNumber = SafeGetInteger(reader, "MDNr", 0)
						recData.MandantName1 = SafeGetString(reader, "MDName")
						recData.MandantGuid = SafeGetString(reader, "MDGuid")
						' TODO: nicht für jeden Datensatz alles laden, obwohl nur das Feld MDDbConn benötigt wird!!! (Empfehlung Franz)
						recData.MandantDbConnection = m_md.GetSelectedMDData(recData.MandantNumber).MDDbConn

						result.Add(recData)

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

		Function LoadMandantAllowedListData() As IEnumerable(Of MandantData) Implements ICommonDatabaseAccess.LoadMandantAllowedListData
			Dim result As List(Of MandantData) = Nothing
			Dim m_md As New Mandant

			Dim sql As String
			sql = "SELECT "
			sql &= "MDA.[MDGuid] "
			sql &= ",MD.[ID] "
			sql &= ",MD.[MDNr] "
			sql &= ",MD.[MD_Name1] "
			sql &= ",MD.[MD_Name2] "
			sql &= ",MD.[MD_Kanton] "
			sql &= "FROM "
			sql &= "[Mandant.AllowedMDList] MDA "
			sql &= "INNER JOIN [Mandanten] MD ON MDA.[MDNr] = MD.[MDNr] "
			sql &= "WHERE "
			sql &= "MD.[Jahr] = DATEPART(year, GETDATE()) "
			sql &= "ORDER BY "
			sql &= "MD.[MD_Name1]"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing, CommandType.Text)
			Try
				If (Not reader Is Nothing) Then
					result = New List(Of MandantData)
					While reader.Read()
						Dim mandantData As New MandantData
						mandantData.MandantGuid = SafeGetString(reader, "MDGuid")
						mandantData.ID = SafeGetInteger(reader, "ID", 0)
						mandantData.MandantNumber = SafeGetInteger(reader, "MDNr", Nothing)
						mandantData.MandantName1 = SafeGetString(reader, "MD_Name1")
						mandantData.MandantName2 = SafeGetString(reader, "MD_Name2")
						mandantData.MandantCanton = SafeGetString(reader, "MD_Kanton")
						result.Add(mandantData)
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
		''' Loads mandant data.
		''' </summary>
		''' <param name="mandantNumber">The mandant number.</param>
		''' <returns>Mandant data or nothing in error case.</returns>
		Function LoadMandantData(ByVal mandantNumber) As MandantData Implements ICommonDatabaseAccess.LoadMandantData
			Dim result As MandantData = Nothing

			Dim sql As String

			sql = "SELECT TOP 1 "
			sql &= "ID"
			sql &= ",MDNr"
			sql &= ",MD_Name1"
			sql &= ",MD_Name2"
			sql &= ",MD_Kanton "
			sql &= ",Strasse "
			sql &= ",PLZ "
			sql &= ",Ort "
			sql &= ",Land "
			sql &= ",Telefon "
			sql &= ",Telefax "
			sql &= ",eMail "
			sql &= ",Homepage "

			sql &= "FROM Mandanten "
			sql &= "WHERE MDNr = @mandantNumber "
			sql &= "Order By CONVERT(int, Jahr) DESC "


			' Parameters
			Dim mandantNumberParameter As New SqlClient.SqlParameter("mandantNumber", mandantNumber)
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(mandantNumberParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New MandantData

					result.ID = SafeGetInteger(reader, "ID", 0)
					result.MandantNumber = SafeGetInteger(reader, "MDNr", Nothing)
					result.MandantName1 = SafeGetString(reader, "MD_Name1")
					result.MandantName2 = SafeGetString(reader, "MD_Name2")
					result.MandantCanton = SafeGetString(reader, "MD_Kanton")

					result.Street = SafeGetString(reader, "Strasse")
					result.Postcode = SafeGetString(reader, "PLZ")
					result.Location = SafeGetString(reader, "Ort")
					result.Telephon = SafeGetString(reader, "Telefon")
					result.Telefax = SafeGetString(reader, "Telefax")
					result.EMail = SafeGetString(reader, "EMail")
					result.Homepage = SafeGetString(reader, "Homepage")

				End If

			Catch e As Exception
				result = Nothing
				m_Logger.LogError(e.ToString())

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function

		Function LoadUpdateMandantData() As IEnumerable(Of RootMandantData) Implements ICommonDatabaseAccess.LoadUpdateMandantData

			Dim result As List(Of RootMandantData) = Nothing

			Dim sql As String

			sql = "SELECT ID ,"
			sql &= " MDNr ,"
			sql &= " MDName ,"
			sql &= " MDPath ,"
			sql &= " Deaktiviert ,"
			sql &= " DbName ,"
			sql &= " DbConnectionstr ,"
			sql &= " DbServerName ,"
			sql &= " Customer_id ,"
			sql &= " MDGroupNr ,"
			sql &= " FileServerPath "
			sql &= " FROM [Sputnik DbSelect].dbo.mandanten "
			sql &= " Where "
			'sql &= " IsNull(DbConnectionstr, '') <> '' "
			'sql &= " AND IsNull(Deaktiviert, 1) = 0 "
			sql &= " IsNull(MDNr, 0) > 0 "

			sql &= " ORDER BY mdnr"

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of RootMandantData)

					While reader.Read

						Dim overviewData = New RootMandantData

						overviewData.ID = SafeGetInteger(reader, "ID", 0)
						overviewData.MandantNumber = SafeGetInteger(reader, "MDNr", 0)
						overviewData.MandantName1 = SafeGetString(reader, "MDName")
						overviewData.MDPath = SafeGetString(reader, "MDPath")
						overviewData.Deaktiviert = SafeGetBoolean(reader, "Deaktiviert", False)
						overviewData.DbName = SafeGetString(reader, "DbName")
						'overviewData.DbConnectionstr = GetDbConnectionString(overviewData.MDNr, IO.Directory.GetDirectoryRoot(overviewData.MDPath))
						overviewData.DbServerName = SafeGetString(reader, "DbServerName")
						overviewData.MandantGuid = SafeGetString(reader, "Customer_id")
						overviewData.MDGroupNr = SafeGetInteger(reader, "MDGroupNr", 0)
						overviewData.FileServerPath = SafeGetString(reader, "FileServerPath")

						'If Not String.IsNullOrWhiteSpace(overviewData.DbConnectionstr) Then
						result.Add(overviewData)
						'End If

					End While

				End If

			Catch e As Exception
				m_Logger.LogError(e.ToString())
				result = Nothing

			Finally
				CloseReader(reader)
			End Try


			Return result

		End Function



	End Class

End Namespace
