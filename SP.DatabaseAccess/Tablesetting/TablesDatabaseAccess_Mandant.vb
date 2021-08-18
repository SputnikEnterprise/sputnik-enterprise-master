

Imports SP.DatabaseAccess.TableSetting.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language


Namespace TableSetting



	Partial Class TablesDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ITablesDatabaseAccess


#Region "Mandantendaten"


		''' <summary>
		''' Loads mandant data.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The mandant data.</returns>
		Public Function LoadMandantData(ByVal mdNr As Integer, ByVal year As Integer) As MandantData Implements ITablesDatabaseAccess.LoadMandantData

			Dim result As MandantData = Nothing

			Dim sql As String = String.Empty


			sql &= "SELECT "
			sql &= "Convert(int, MD.Jahr) As Jahr, "

			sql &= "M.MDName As MDAuflistung, "
			sql &= "M.DBName, "

			sql &= "MD.MD_Name1, "
			sql &= "MD.MD_Name2, "
			sql &= "MD.MD_Name3, "
			sql &= "MD.MDFullFileName, "
			sql &= "MD.Customer_ID, "
			sql &= "MD.Postfach, "
			sql &= "MD.Strasse, "
			sql &= "MD.PLZ, "
			sql &= "MD.Ort, "
			sql &= "MD.MD_Kanton, "
			sql &= "MD.Land, "
			sql &= "MD.Telefon, "
			sql &= "MD.Telefax, "
			sql &= "MD.Homepage, "
			sql &= "MD.eMail, "
			sql &= "MD.Passwort4, "

			sql &= "MD.KK_An_MA, KK_AG_MA, KK_An_MZ, KK_AG_MZ, KK_An_WA, KK_AG_WA, KK_An_WZ, KK_AG_WZ, Suva_HL, ALV1_HL, ALV2_HL, RentAlter_M, RentAlter_W, "
			sql &= "MD.BVG_Koordination_Jahr, BVG_Std, BVG_Max_Jahr, BVG_Min_Jahr, BVG_Aus1Woche, BVG_Aus2Woche, MindestAlter, "
			sql &= "MD.BVG_List, BVG_List_Grouped, "
			sql &= "MD.RentFrei_Monat, RentFrei_Jahr, NBUV_WStd, [AHV_AN], [AHV_2_AN], ([ALV1_HL] / 12) AS ALV1_HL_, ([ALV2_HL] / 12) AS ALV2_HL_, "
			sql &= "MD.[ALV_AN], MD.[ALV2_An], "
			sql &= "(MD.[SUVA_HL] / 12 ) AS SUVA_HL_, "
			sql &= "MD.NBUV_M, MD.NBUV_M_Z, MD.NBUV_W, MD.NBUV_W_Z, "
			sql &= "MD.[AHV_AG], MD.[AHV_2_AG], "
			sql &= "MD.[ALV_AG], MD.[ALV2_AG], "
			sql &= "MD.Suva_A, MD.Suva_Z, "
			sql &= "MD.UVGZ_A, MD.UVGZ_B, "
			sql &= "MD.UVGZ2_A, MD.UVGZ2_B, "
			sql &= "MD.Fak_Proz, "

			sql &= "MD.b_marge, "
			sql &= "MD.b_margep, "
			sql &= "MD.x_marge, "
			sql &= "MD.ag_tar_proz, "
			sql &= "MD.n_zhlg, "
			sql &= "MD.q_zhlg, "
			sql &= "MD.ma_kl "

			sql &= " FROM Mandanten MD, "
			sql &= "[Sputnik DbSelect].Dbo.Mandanten M "
			sql &= "WHERE MD.Jahr = @year AND MD.MDNr = @mdNr "
			sql &= "And M.MDNr = MD.MDNr And M.Customer_ID = MD.Customer_ID  "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("year", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New MandantData

					result.Jahr = SafeGetInteger(reader, "Jahr", 0)
					result.MD_Name1 = SafeGetString(reader, "MD_Name1")
					result.MD_Name2 = SafeGetString(reader, "MD_Name2")
					result.MD_Name3 = SafeGetString(reader, "MD_Name3")
					result.MDFullFileName = SafeGetString(reader, "MDFullFileName")

					result.MDAuflistung = SafeGetString(reader, "MDAuflistung")
					result.dbname = SafeGetString(reader, "dbname")

					result.Customer_ID = SafeGetString(reader, "Customer_ID")
					result.Postfach = SafeGetString(reader, "Postfach")
					result.Strasse = SafeGetString(reader, "Strasse")
					result.PLZ = SafeGetString(reader, "PLZ")
					result.Ort = SafeGetString(reader, "Ort")
					result.MD_Kanton = SafeGetString(reader, "MD_Kanton")
					result.Land = SafeGetString(reader, "Land")
					result.Telefon = SafeGetString(reader, "Telefon")
					result.Telefax = SafeGetString(reader, "Telefax")
					result.Homepage = SafeGetString(reader, "Homepage")
					result.eMail = SafeGetString(reader, "eMail")
					result.Passwort4 = SafeGetString(reader, "Passwort4")



					result.BVG_Koordination_Jahr = SafeGetDecimal(reader, "BVG_Koordination_Jahr", 0)
					result.BVG_Std = SafeGetInteger(reader, "BVG_Std", 0)
					result.BVG_Max_Jahr = SafeGetDecimal(reader, "BVG_Max_Jahr", 0)
					result.BVG_Min_Jahr = SafeGetDecimal(reader, "BVG_Min_Jahr", 0)
					result.BVG_Aus1Woche = SafeGetDecimal(reader, "BVG_Aus1Woche", 0)
					result.BVG_List = SafeGetString(reader, "BVG_List")
					result.BVG_List_Grouped = SafeGetString(reader, "BVG_List_Grouped", 13)

					result.KK_An_MA = SafeGetDecimal(reader, "KK_An_MA", 0)
					result.KK_AG_MA = SafeGetDecimal(reader, "KK_AG_MA", 0)
					result.KK_An_MZ = SafeGetDecimal(reader, "KK_An_MZ", 0)
					result.KK_AG_MZ = SafeGetDecimal(reader, "KK_AG_MZ", 0)
					result.KK_An_WA = SafeGetDecimal(reader, "KK_An_WA", 0)
					result.KK_AG_WA = SafeGetDecimal(reader, "KK_AG_WA", 0)
					result.KK_An_WZ = SafeGetDecimal(reader, "KK_An_WZ", 0)
					result.KK_AG_WZ = SafeGetDecimal(reader, "KK_AG_WZ", 0)
					result.Suva_HL = SafeGetDecimal(reader, "Suva_HL", 0)
					result.ALV1_HL = SafeGetDecimal(reader, "ALV1_HL", 0)
					result.ALV2_HL = SafeGetDecimal(reader, "ALV2_HL", 0)
					result.RentAlter_M = SafeGetShort(reader, "RentAlter_M", 0)
					result.RentAlter_W = SafeGetShort(reader, "RentAlter_W", 0)
					result.MindestAlter = SafeGetShort(reader, "MindestAlter", 0)


					result.RentFrei_Monat = SafeGetDecimal(reader, "RentFrei_Monat", 0)
					result.RentFrei_Jahr = SafeGetDecimal(reader, "RentFrei_Jahr", 0)
					result.NBUV_WStd = SafeGetDecimal(reader, "NBUV_WStd", 0)
					result.AHV_AN = SafeGetDecimal(reader, "AHV_AN", 0)
					result.AHV_2_AN = SafeGetDecimal(reader, "AHV_2_AN", 0)
					result.ALV1_HL_ = SafeGetDecimal(reader, "ALV1_HL_", 0)
					result.ALV2_HL_ = SafeGetDecimal(reader, "ALV2_HL_", 0)
					result.ALV_AN = SafeGetDecimal(reader, "ALV_AN", 0)
					result.ALV2_An = SafeGetDecimal(reader, "ALV2_An", 0)
					result.SUVA_HL_ = SafeGetDecimal(reader, "SUVA_HL_", 0)
					result.NBUV_M = SafeGetDecimal(reader, "NBUV_M", 0)
					result.NBUV_M_Z = SafeGetDecimal(reader, "NBUV_M_Z", 0)
					result.NBUV_W = SafeGetDecimal(reader, "NBUV_W", 0)
					result.NBUV_W_Z = SafeGetDecimal(reader, "NBUV_W_Z", 0)
					result.AHV_AG = SafeGetDecimal(reader, "AHV_AG", 0)
					result.AHV_2_AG = SafeGetDecimal(reader, "AHV_2_AG", 0)
					result.ALV_AG = SafeGetDecimal(reader, "ALV_AG", 0)
					result.ALV2_AG = SafeGetDecimal(reader, "ALV2_AG", 0)
					result.Suva_A = SafeGetDecimal(reader, "Suva_A", 0)
					result.Suva_Z = SafeGetDecimal(reader, "Suva_Z", 0)
					result.UVGZ_A = SafeGetDecimal(reader, "UVGZ_A", 0)
					result.UVGZ_B = SafeGetDecimal(reader, "UVGZ_B", 0)
					result.UVGZ2_A = SafeGetDecimal(reader, "UVGZ2_A", 0)
					result.UVGZ2_B = SafeGetDecimal(reader, "UVGZ2_B", 0)

					result.Fak_Proz = SafeGetDecimal(reader, "Fak_Proz", 0)


					result.b_marge = SafeGetDecimal(reader, "b_marge", 0)
					result.b_margep = SafeGetDecimal(reader, "b_margep", 0)
					result.x_marge = SafeGetDecimal(reader, "x_marge", 0)
					result.ag_tar_proz = SafeGetDecimal(reader, "ag_tar_proz", 0)
					result.n_zhlg = SafeGetDecimal(reader, "n_zhlg", 0)
					result.q_zhlg = SafeGetDecimal(reader, "q_zhlg", 0)
					result.ma_kl = SafeGetDecimal(reader, "ma_kl", 0)

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
		''' saves mandant data.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The mandant data.</returns>
		Public Function SaveMandantData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As MandantData) As Boolean Implements ITablesDatabaseAccess.SaveMandantData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Update Mandanten Set "

			sql &= "Jahr = @year, "
			sql &= "MD_Name1 = @MD_Name1, "
			sql &= "MD_Name2 = @MD_Name2, "
			sql &= "MD_Name3 = @MD_Name3, "
			sql &= "MDFullFileName = @MDFullFileName, "
			sql &= "Customer_ID = @Customer_ID, "
			sql &= "Postfach = @Postfach, "
			sql &= "Strasse = @Strasse, "
			sql &= "PLZ = @PLZ, "
			sql &= "Ort = @Ort, "
			sql &= "MD_Kanton = @MD_Kanton, "
			sql &= "Land = @Land, "
			sql &= "Telefon = @Telefon, "
			sql &= "Telefax = @Telefax, "
			sql &= "Homepage = @Homepage, "
			sql &= "eMail = @eMail, "
			sql &= "Passwort4 = @Passwort4, "

			sql &= "KK_An_MA = @KK_An_MA, "
			sql &= "KK_AG_MA = @KK_AG_MA, "
			sql &= "KK_An_MZ = @KK_An_MZ, "
			sql &= "KK_AG_MZ = @KK_AG_MZ, "
			sql &= "KK_An_WA = @KK_An_WA, "
			sql &= "KK_AG_WA = @KK_AG_WA, "
			sql &= "KK_An_WZ = @KK_An_WZ, "
			sql &= "KK_AG_WZ = @KK_AG_WZ, "
			sql &= "Suva_HL = @Suva_HL, "
			sql &= "ALV1_HL = @ALV1_HL, "
			sql &= "ALV2_HL = @ALV2_HL, "
			sql &= "RentAlter_M = @RentAlter_M, "
			sql &= "RentAlter_W = @RentAlter_W, "
			sql &= "BVG_Koordination_Jahr = @BVG_Koordination_Jahr, "
			sql &= "BVG_Std = @BVG_Std, "
			sql &= "BVG_Max_Jahr = @BVG_Max_Jahr, "
			sql &= "BVG_Min_Jahr = @BVG_Min_Jahr, "
			sql &= "BVG_Aus1Woche = @BVG_Aus1Woche, "
			sql &= "MindestAlter = @MindestAlter, "
			sql &= "BVG_List = @BVG_List, "
			sql &= "BVG_List_Grouped = @BVG_List_Grouped, "
			sql &= "RentFrei_Monat = @RentFrei_Monat, "
			sql &= "RentFrei_Jahr = @RentFrei_Jahr, "
			sql &= "NBUV_WStd = @NBUV_WStd, "
			sql &= "[AHV_AN] = @AHV_AN, "
			sql &= "[ALV_AN] = @ALV_AN, "
			sql &= "[ALV2_An] = @ALV2_An, "
			sql &= "NBUV_M = @NBUV_M, "
			sql &= "NBUV_M_Z = @NBUV_M_Z, "
			sql &= "NBUV_W = @NBUV_W, "
			sql &= "NBUV_W_Z = @NBUV_W_Z, "
			sql &= "[AHV_AG] = @AHV_AG, "
			sql &= "[AHV_2_AG] = @AHV_2_AG, "
			sql &= "[ALV_AG] = @ALV_AG, "
			sql &= "[ALV2_AG] = @ALV2_AG, "
			sql &= "Suva_A = @Suva_A, "
			sql &= "Suva_Z = @Suva_Z, "
			sql &= "UVGZ_A = @UVGZ_A, "
			sql &= "UVGZ_B = @UVGZ_B, "
			sql &= "UVGZ2_A = @UVGZ2_A, "
			sql &= "UVGZ2_B = @UVGZ2_B, "

			sql &= "Fak_Proz = @Fak_Proz, "

			sql &= "b_marge = @b_marge, "
			sql &= "b_margep = @b_margep, "
			sql &= "x_marge = @x_marge, "
			sql &= "ag_tar_proz = @ag_tar_proz, "
			sql &= "n_zhlg = @n_zhlg, "
			sql &= "q_zhlg = @q_zhlg, "
			sql &= "ma_kl = @ma_kl "

			sql &= "WHERE Jahr = @year AND MDNr = @mdNr; "

			sql &= "Update Dbo.[Mandant.AllowedMDList] Set "
			sql &= "MDName = @MDAuflistung, "
			sql &= "MDGuid = @Customer_ID, "
			sql &= "DBName = @DBName "

			sql &= "WHERE MDNr = @mdNr; "

			sql &= "Update [Sputnik DbSelect].Dbo.Mandanten Set "
			sql &= "MDName = @MDAuflistung, "
			sql &= "Customer_ID = @Customer_ID, "
			sql &= "DBName = @DBName,"
			sql &= "MDPath = @MDFullFileName, "
			sql &= "FileServerPath = @MDFullFileName "

			sql &= "WHERE MDNr = @mdNr; "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("year", year))

			listOfParams.Add(New SqlClient.SqlParameter("MD_Name1", MDData.MD_Name1))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Name2", MDData.MD_Name2))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Name3", MDData.MD_Name3))
			listOfParams.Add(New SqlClient.SqlParameter("MDFullFileName", MDData.MDFullFileName))

			listOfParams.Add(New SqlClient.SqlParameter("MDAuflistung", MDData.MDAuflistung))
			listOfParams.Add(New SqlClient.SqlParameter("DBName", MDData.dbname))

			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", MDData.Customer_ID))
			listOfParams.Add(New SqlClient.SqlParameter("Postfach", MDData.Postfach))
			listOfParams.Add(New SqlClient.SqlParameter("Strasse", MDData.Strasse))
			listOfParams.Add(New SqlClient.SqlParameter("PLZ", MDData.PLZ))
			listOfParams.Add(New SqlClient.SqlParameter("Ort", MDData.Ort))
			listOfParams.Add(New SqlClient.SqlParameter("MD_Kanton", MDData.MD_Kanton))
			listOfParams.Add(New SqlClient.SqlParameter("Land", MDData.Land))
			listOfParams.Add(New SqlClient.SqlParameter("Telefon", MDData.Telefon))
			listOfParams.Add(New SqlClient.SqlParameter("Telefax", MDData.Telefax))
			listOfParams.Add(New SqlClient.SqlParameter("Homepage", MDData.Homepage))
			listOfParams.Add(New SqlClient.SqlParameter("eMail", MDData.eMail))
			listOfParams.Add(New SqlClient.SqlParameter("Passwort4", MDData.Passwort4))


			listOfParams.Add(New SqlClient.SqlParameter("KK_An_MA", ReplaceMissing(MDData.KK_An_MA, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MA", ReplaceMissing(MDData.KK_AG_MA, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_An_MZ", ReplaceMissing(MDData.KK_An_MZ, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MZ", ReplaceMissing(MDData.KK_AG_MZ, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_An_WA", ReplaceMissing(MDData.KK_An_WA, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WA", ReplaceMissing(MDData.KK_AG_WA, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_An_WZ", ReplaceMissing(MDData.KK_An_WZ, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WZ", ReplaceMissing(MDData.KK_AG_WZ, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("Suva_HL", ReplaceMissing(MDData.Suva_HL, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ALV1_HL", ReplaceMissing(MDData.ALV1_HL, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ALV2_HL", ReplaceMissing(MDData.ALV2_HL, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("RentAlter_M", ReplaceMissing(MDData.RentAlter_M, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RentAlter_W", ReplaceMissing(MDData.RentAlter_W, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("BVG_Koordination_Jahr", ReplaceMissing(MDData.BVG_Koordination_Jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("BVG_Std", ReplaceMissing(MDData.BVG_Std, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("BVG_Max_Jahr", ReplaceMissing(MDData.BVG_Max_Jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("BVG_Min_Jahr", ReplaceMissing(MDData.BVG_Min_Jahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("BVG_Aus1Woche", ReplaceMissing(MDData.BVG_Aus1Woche, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("MindestAlter", ReplaceMissing(MDData.MindestAlter, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("BVG_List", ReplaceMissing(MDData.BVG_List, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("BVG_List_Grouped", ReplaceMissing(MDData.BVG_List_Grouped, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RentFrei_Monat", ReplaceMissing(MDData.RentFrei_Monat, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("RentFrei_Jahr", ReplaceMissing(MDData.RentFrei_Jahr, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("NBUV_WStd", ReplaceMissing(MDData.NBUV_WStd, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("AHV_AN", ReplaceMissing(MDData.AHV_AN, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ALV_AN", ReplaceMissing(MDData.ALV_AN, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ALV2_An", ReplaceMissing(MDData.ALV2_An, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("NBUV_M", ReplaceMissing(MDData.NBUV_M, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("NBUV_M_Z", ReplaceMissing(MDData.NBUV_M_Z, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("NBUV_W", ReplaceMissing(MDData.NBUV_W, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("NBUV_W_Z", ReplaceMissing(MDData.NBUV_W_Z, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("AHV_AG", ReplaceMissing(MDData.AHV_AG, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("AHV_2_AG", ReplaceMissing(MDData.AHV_2_AG, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ALV_AG", ReplaceMissing(MDData.ALV_AG, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ALV2_AG", ReplaceMissing(MDData.ALV2_AG, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Suva_A", ReplaceMissing(MDData.Suva_A, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Suva_Z", ReplaceMissing(MDData.Suva_Z, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("UVGZ_A", ReplaceMissing(MDData.UVGZ_A, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("UVGZ_B", ReplaceMissing(MDData.UVGZ_B, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("UVGZ2_A", ReplaceMissing(MDData.UVGZ2_A, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("UVGZ2_B", ReplaceMissing(MDData.UVGZ2_B, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("Fak_Proz", ReplaceMissing(MDData.Fak_Proz, 0)))


			listOfParams.Add(New SqlClient.SqlParameter("b_marge", ReplaceMissing(MDData.b_marge, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("b_margep", ReplaceMissing(MDData.b_margep, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("x_marge", ReplaceMissing(MDData.x_marge, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ag_tar_proz", ReplaceMissing(MDData.ag_tar_proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("n_zhlg", ReplaceMissing(MDData.n_zhlg, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("q_zhlg", ReplaceMissing(MDData.q_zhlg, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ma_kl", ReplaceMissing(MDData.ma_kl, 0)))

			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function


#End Region


#Region "Kinder- Ausbildungszulage"

		''' <summary>
		''' Loads Kinder- Ausbildungszulage data.
		''' </summary>
		''' <param name="recid">The id of record.</param>
		''' <returns>The child education data.</returns>
		Public Function LoadAssignedChildEducationData(ByVal recid As Integer) As ChildEducationData Implements ITablesDatabaseAccess.LoadAssignedChildEducationData

			Dim result As ChildEducationData = Nothing

			Dim sql As String = String.Empty

			sql &= "Select "
			sql &= "[ID] "
			sql &= ",[RecNr]"
			sql &= ",[MDNr]"
			sql &= ",Convert(int, [MDYear]) As MDYear"
			sql &= ",[Fak_Kanton]"
			sql &= ",[Ki1_FakMax]"
			sql &= ",[Ki2_FakMax]"
			sql &= ",[Ki1_Std]"
			sql &= ",[Ki2_Std]"
			sql &= ",[Ki1_Day]"
			sql &= ",[Ki2_Day]"
			sql &= ",[Ki1_Month]"
			sql &= ",[Ki2_Month]"
			sql &= ",[ChangeKiIn]"
			sql &= ",[Au1_Std]"
			sql &= ",[Au2_Std]"
			sql &= ",[Au1_Day]"
			sql &= ",[Au2_Day]"
			sql &= ",[Au1_Month]"
			sql &= ",[ChangeAuIn]"
			sql &= ",[CreatedOn]"
			sql &= ",[CreatedFrom]"
			sql &= ",[ChangedOn]"
			sql &= ",[ChangedFrom]"
			sql &= ",[Fak_Name]"
			sql &= ",[Fak_ZHD]"
			sql &= ",[Fak_Postfach]"
			sql &= ",[Fak_Strasse]"
			sql &= ",[Fak_PLZOrt]"
			sql &= ",[Fak_MNr]"
			sql &= ",[Fak_KNr]"
			sql &= ",[ChangeAuIn_2]"
			sql &= ",[ChangeKiIn_2]"
			sql &= ",[YMinLohn]"
			sql &= ",[Bemerkung_1]"
			sql &= ",[Bemerkung_2]"
			sql &= ",[Bemerkung_3]"
			sql &= ",[Bemerkung_4]"
			sql &= ",[Geb_Zulage]"
			sql &= ",[Ado_Zulage]"
			sql &= ",[Fak_Proz] "
			sql &= ",[AtEndBeginES]"
			sql &= ",[SeeAHVLohnForYear]"

			sql &= " From MD_KiAu Where ID = @recid"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", recid))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New ChildEducationData

					result.recid = SafeGetInteger(reader, "id", 0)
					result.recnr = SafeGetInteger(reader, "recnr", 0)
					result.mdnr = SafeGetInteger(reader, "mdnr", 0)
					result.mdyear = SafeGetInteger(reader, "MDYear", 0)
					result.fak_kanton = SafeGetString(reader, "fak_kanton")
					result.ki1_fakmax = SafeGetDecimal(reader, "ki1_fakmax", 0)
					result.ki2_fakmax = SafeGetDecimal(reader, "ki2_fakmax", 0)
					result.ki1_std = SafeGetDecimal(reader, "ki1_std", 0)

					result.ki2_std = SafeGetDecimal(reader, "ki2_std", 0)
					result.ki1_day = SafeGetDecimal(reader, "ki1_day", 0)

					result.ki2_day = SafeGetDecimal(reader, "ki2_day", 0)
					result.ki1_month = SafeGetDecimal(reader, "ki1_month", 0)
					result.ki2_month = SafeGetDecimal(reader, "ki2_month", 0)
					result.changekiin = SafeGetString(reader, "changekiin")
					result.au1_std = SafeGetDecimal(reader, "au1_std", 0)
					result.au2_std = SafeGetDecimal(reader, "au2_std", 0)
					result.au1_day = SafeGetDecimal(reader, "au1_day", 0)
					result.au2_day = SafeGetDecimal(reader, "au2_day", 0)
					result.au1_month = SafeGetDecimal(reader, "au1_month", 0)
					result.changeauin = SafeGetString(reader, "changeauin")

					result.createdon = SafeGetDateTime(reader, "createdon", Nothing)
					result.createdfrom = SafeGetString(reader, "createdfrom")
					result.changedon = SafeGetDateTime(reader, "changedon", Nothing)
					result.changedfrom = SafeGetString(reader, "changedfrom")

					result.fak_name = SafeGetString(reader, "fak_name")
					result.fak_zhd = SafeGetString(reader, "fak_zhd")
					result.fak_postfach = SafeGetString(reader, "fak_postfach")
					result.fak_strasse = SafeGetString(reader, "fak_strasse")
					result.fak_plzort = SafeGetString(reader, "fak_plzort")
					result.fak_mnr = SafeGetString(reader, "fak_mnr")
					result.fak_knr = SafeGetString(reader, "fak_knr")
					result.changeauin_2 = SafeGetString(reader, "changeauin_2")
					result.changekiin_2 = SafeGetString(reader, "changekiin_2")
					result.yminlohn = SafeGetDecimal(reader, "yminlohn", 0)

					result.bemerkung_1 = SafeGetString(reader, "bemerkung_1")
					result.bemerkung_2 = SafeGetString(reader, "bemerkung_2")
					result.bemerkung_3 = SafeGetString(reader, "bemerkung_3")
					result.bemerkung_4 = SafeGetString(reader, "bemerkung_4")
					result.AtEndBeginES = SafeGetBoolean(reader, "AtEndBeginES", False)
					result.SeeAHVLohnForYear = SafeGetBoolean(reader, "SeeAHVLohnForYear", False)


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
		''' Loads Kinder- Ausbildungszulage data.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The mandant data.</returns>
		Public Function LoadChildEducationData(ByVal mdNr As Integer, ByVal year As Integer) As IEnumerable(Of ChildEducationData) Implements ITablesDatabaseAccess.LoadChildEducationData

			Dim result As List(Of ChildEducationData) = Nothing

			Dim sql As String = String.Empty

			sql &= "Select "
			sql &= "[ID] "
			sql &= ",[RecNr]"
			sql &= ",[MDNr]"
			sql &= ",Convert(int, [MDYear]) As MDYear"
			sql &= ",[Fak_Kanton]"
			sql &= ",[Ki1_FakMax]"
			sql &= ",[Ki2_FakMax]"
			sql &= ",[Ki1_Std]"
			sql &= ",[Ki2_Std]"
			sql &= ",[Ki1_Day]"
			sql &= ",[Ki2_Day]"
			sql &= ",[Ki1_Month]"
			sql &= ",[Ki2_Month]"
			sql &= ",[ChangeKiIn]"
			sql &= ",[Au1_Std]"
			sql &= ",[Au2_Std]"
			sql &= ",[Au1_Day]"
			sql &= ",[Au2_Day]"
			sql &= ",[Au1_Month]"
			sql &= ",[ChangeAuIn]"
			sql &= ",[CreatedOn]"
			sql &= ",[CreatedFrom]"
			sql &= ",[ChangedOn]"
			sql &= ",[ChangedFrom]"
			sql &= ",[Fak_Name]"
			sql &= ",[Fak_ZHD]"
			sql &= ",[Fak_Postfach]"
			sql &= ",[Fak_Strasse]"
			sql &= ",[Fak_PLZOrt]"
			sql &= ",[Fak_MNr]"
			sql &= ",[Fak_KNr]"
			sql &= ",[ChangeAuIn_2]"
			sql &= ",[ChangeKiIn_2]"
			sql &= ",[YMinLohn]"
			sql &= ",[Bemerkung_1]"
			sql &= ",[Bemerkung_2]"
			sql &= ",[Bemerkung_3]"
			sql &= ",[Bemerkung_4]"
			sql &= ",[Geb_Zulage]"
			sql &= ",[Ado_Zulage]"
			sql &= ",[Fak_Proz] "

			sql &= " From MD_KiAu Where MDNr = @MDNr And MDYear = @MDYear Order BY Fak_Kanton"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("MDYear", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of ChildEducationData)

					While reader.Read

						Dim data = New ChildEducationData()

						data.recid = SafeGetInteger(reader, "id", 0)
						data.recnr = SafeGetInteger(reader, "recnr", 0)
						data.mdnr = SafeGetInteger(reader, "mdnr", 0)
						data.mdyear = SafeGetInteger(reader, "MDYear", 0)
						data.fak_kanton = SafeGetString(reader, "fak_kanton")
						data.ki1_fakmax = SafeGetDecimal(reader, "ki1_fakmax", 0)
						data.ki2_fakmax = SafeGetDecimal(reader, "ki2_fakmax", 0)
						data.ki1_std = SafeGetDecimal(reader, "ki1_std", 0)

						data.ki2_std = SafeGetDecimal(reader, "ki2_std", 0)
						data.ki1_day = SafeGetDecimal(reader, "ki1_day", 0)

						data.ki2_day = SafeGetDecimal(reader, "ki2_day", 0)
						data.ki1_month = SafeGetDecimal(reader, "ki1_month", 0)
						data.ki2_month = SafeGetDecimal(reader, "ki2_month", 0)
						data.changekiin = SafeGetString(reader, "changekiin")
						data.au1_std = SafeGetDecimal(reader, "au1_std", 0)
						data.au2_std = SafeGetDecimal(reader, "au2_std", 0)
						data.au1_day = SafeGetDecimal(reader, "au1_day", 0)
						data.au2_day = SafeGetDecimal(reader, "au2_day", 0)
						data.au1_month = SafeGetDecimal(reader, "au1_month", 0)
						data.changeauin = SafeGetString(reader, "changeauin")

						data.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						data.createdfrom = SafeGetString(reader, "createdfrom")
						data.changedon = SafeGetDateTime(reader, "changedon", Nothing)
						data.changedfrom = SafeGetString(reader, "changedfrom")

						data.fak_name = SafeGetString(reader, "fak_name")
						data.fak_zhd = SafeGetString(reader, "fak_zhd")
						data.fak_postfach = SafeGetString(reader, "fak_postfach")
						data.fak_strasse = SafeGetString(reader, "fak_strasse")
						data.fak_plzort = SafeGetString(reader, "fak_plzort")
						data.fak_mnr = SafeGetString(reader, "fak_mnr")
						data.fak_knr = SafeGetString(reader, "fak_knr")
						data.changeauin_2 = SafeGetString(reader, "changeauin_2")
						data.changekiin_2 = SafeGetString(reader, "changekiin_2")
						data.yminlohn = SafeGetDecimal(reader, "yminlohn", 0)

						data.bemerkung_1 = SafeGetString(reader, "bemerkung_1")
						data.bemerkung_2 = SafeGetString(reader, "bemerkung_2")
						data.bemerkung_3 = SafeGetString(reader, "bemerkung_3")
						data.bemerkung_4 = SafeGetString(reader, "bemerkung_4")


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
		''' saves child and education data.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The mandant data.</returns>
		Public Function UpdateAssignedChildEducationData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As ChildEducationData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedChildEducationData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Update MD_KiAu Set "

			sql &= "MDYear = @mdyear, "
			sql &= "mdnr = @mdnr, "
			sql &= "fak_kanton = @fak_kanton, "
			sql &= "ki1_fakmax = @ki1_fakmax, "
			sql &= "ki2_fakmax = @ki2_fakmax, "

			sql &= "ki1_std = @ki1_std, "
			sql &= "ki2_std = @ki2_std, "
			sql &= "ki1_day = @ki1_day, "
			sql &= "ki2_day = @ki2_day, "
			sql &= "ki1_month = @ki1_month, "
			sql &= "ki2_month = @ki2_month, "
			sql &= "changekiin = @changekiin, "

			sql &= "au1_std = @au1_std, "
			sql &= "au2_std = @au2_std, "
			sql &= "au1_day = @au1_day, "
			sql &= "au2_day = @au2_day, "
			sql &= "au1_month = @au1_month, "
			sql &= "changeauin = @changeauin, "

			sql &= "changedon = @changedon, "
			sql &= "changedfrom = @changedfrom, "

			sql &= "fak_name = @fak_name, "
			sql &= "fak_zhd = @fak_zhd, "
			sql &= "fak_postfach = @fak_postfach, "
			sql &= "fak_strasse = @fak_strasse, "
			sql &= "fak_plzort = @fak_plzort, "
			sql &= "fak_mnr = @fak_mnr, "
			sql &= "fak_knr = @fak_knr, "

			sql &= "changeauin_2 = @changeauin_2, "
			sql &= "changekiin_2 = @changekiin_2, "
			sql &= "AtEndBeginES = @AtEndBeginES, "
			sql &= "SeeAHVLohnForYear = @SeeAHVLohnForYear, "

			sql &= "yminlohn = @yminlohn "

			'sql &= "bemerkung_1 = @bemerkung_1, "
			'sql &= "bemerkung_2 = @bemerkung_2, "
			'sql &= "bemerkung_3 = @bemerkung_3, "
			'sql &= "bemerkung_4 = @bemerkung_4 "

			sql &= "WHERE "
			sql &= "ID = @recid "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("mdyear", year))

			listOfParams.Add(New SqlClient.SqlParameter("fak_kanton", MDData.fak_kanton))

			listOfParams.Add(New SqlClient.SqlParameter("ki1_fakmax", ReplaceMissing(MDData.ki1_fakmax, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki2_fakmax", ReplaceMissing(MDData.ki2_fakmax, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("ki1_std", ReplaceMissing(MDData.ki1_std, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki2_std", ReplaceMissing(MDData.ki2_std, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("ki1_day", ReplaceMissing(MDData.ki1_day, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki2_day", ReplaceMissing(MDData.ki2_day, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki1_month", ReplaceMissing(MDData.ki1_month, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki2_month", ReplaceMissing(MDData.ki2_month, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("changekiin", MDData.changekiin))

			listOfParams.Add(New SqlClient.SqlParameter("au1_std", ReplaceMissing(MDData.au1_std, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("au2_std", ReplaceMissing(MDData.au2_std, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("au1_day", ReplaceMissing(MDData.au1_day, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("au2_day", ReplaceMissing(MDData.au2_day, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("au1_month", ReplaceMissing(MDData.au1_month, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("changeauin", MDData.changeauin))

			listOfParams.Add(New SqlClient.SqlParameter("changedon", Now))
			listOfParams.Add(New SqlClient.SqlParameter("changedfrom", MDData.changedfrom))


			listOfParams.Add(New SqlClient.SqlParameter("fak_name", MDData.fak_name))
			listOfParams.Add(New SqlClient.SqlParameter("fak_zhd", MDData.fak_zhd))
			listOfParams.Add(New SqlClient.SqlParameter("fak_postfach", MDData.fak_postfach))
			listOfParams.Add(New SqlClient.SqlParameter("fak_strasse", MDData.fak_strasse))
			listOfParams.Add(New SqlClient.SqlParameter("fak_plzort", MDData.fak_plzort))
			listOfParams.Add(New SqlClient.SqlParameter("fak_mnr", MDData.fak_mnr))
			listOfParams.Add(New SqlClient.SqlParameter("fak_knr", MDData.fak_knr))
			listOfParams.Add(New SqlClient.SqlParameter("changeauin_2", MDData.changeauin))
			listOfParams.Add(New SqlClient.SqlParameter("changekiin_2", MDData.changekiin))
			listOfParams.Add(New SqlClient.SqlParameter("yminlohn", ReplaceMissing(MDData.yminlohn, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("AtEndBeginES", ReplaceMissing(MDData.AtEndBeginES, False)))
			listOfParams.Add(New SqlClient.SqlParameter("SeeAHVLohnForYear", ReplaceMissing(MDData.SeeAHVLohnForYear, False)))

			'listOfParams.Add(New SqlClient.SqlParameter("bemerkung_1", MDData.bemerkung_1))
			'listOfParams.Add(New SqlClient.SqlParameter("bemerkung_2", MDData.bemerkung_2))
			'listOfParams.Add(New SqlClient.SqlParameter("bemerkung_3", MDData.bemerkung_3))
			'listOfParams.Add(New SqlClient.SqlParameter("bemerkung_4", MDData.bemerkung_4))

			listOfParams.Add(New SqlClient.SqlParameter("recid", MDData.recid))


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
		''' Add child and education data.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The mandant data.</returns>
		Public Function AddChildEducationData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As ChildEducationData) As Boolean Implements ITablesDatabaseAccess.AddChildEducationData

			Dim success As Boolean = True

			Dim sql As String

			sql = "Delete dbo.MD_KiAu Where MDNr = @mdNr And MDYear = @mdyear And Fak_Kanton = @fak_kanton; "

			sql &= "Insert Into MD_KiAu ("

			sql &= "RecNr, "

			sql &= "MDYear , "
			sql &= "mdnr , "
			sql &= "fak_kanton , "
			sql &= "ki1_fakmax , "
			sql &= "ki2_fakmax , "
			sql &= "ki1_std , "
			sql &= "ki2_std , "
			sql &= "ki1_day , "
			sql &= "ki2_day , "
			sql &= "ki1_month , "
			sql &= "ki2_month , "
			sql &= "changekiin, "
			sql &= "au1_std , "
			sql &= "au2_std , "
			sql &= "au1_day , "
			sql &= "au2_day , "
			sql &= "au1_month , "
			sql &= "changeauin, "
			sql &= "createdon , "
			sql &= "createdfrom , "
			sql &= "fak_name , "
			sql &= "fak_zhd , "
			sql &= "fak_postfach , "
			sql &= "fak_strasse , "
			sql &= "fak_plzort , "
			sql &= "fak_mnr , "
			sql &= "fak_knr , "
			sql &= "changeauin_2 , "
			sql &= "changekiin_2 , "
			sql &= "AtEndBeginES , "
			sql &= "SeeAHVLohnForYear , "
			sql &= "yminlohn "
			'sql &= "bemerkung_1 , "
			'sql &= "bemerkung_2 , "
			'sql &= "bemerkung_3 , "
			'sql &= "bemerkung_4 "

			sql &= ") Values ("


			sql &= "(Select Top 1 RecNr + 1 From MD_KIAu Order By RecNr Desc) , "

			sql &= "@MDYear , "
			sql &= "@mdnr , "
			sql &= "@fak_kanton , "
			sql &= "@ki1_fakmax , "
			sql &= "@ki2_fakmax , "
			sql &= "@ki1_std , "
			sql &= "@ki2_std , "
			sql &= "@ki1_day , "
			sql &= "@ki2_day , "
			sql &= "@ki1_month , "
			sql &= "@ki2_month , "
			sql &= "@changekiin, "
			sql &= "@au1_std , "
			sql &= "@au2_std , "
			sql &= "@au1_day , "
			sql &= "@au2_day , "
			sql &= "@au1_month , "
			sql &= "@changeauin, "

			sql &= "@createdon , "
			sql &= "@createdfrom , "

			sql &= "@fak_name , "
			sql &= "@fak_zhd , "
			sql &= "@fak_postfach , "
			sql &= "@fak_strasse , "
			sql &= "@fak_plzort , "
			sql &= "@fak_mnr , "
			sql &= "@fak_knr , "
			sql &= "@changeauin_2 , "
			sql &= "@changekiin_2 , "
			sql &= "@AtEndBeginES , "
			sql &= "@SeeAHVLohnForYear , "
			sql &= "@yminlohn  "

			'sql &= "@bemerkung_1 , "
			'sql &= "@bemerkung_2 , "
			'sql &= "@bemerkung_3 , "
			'sql &= "@bemerkung_4 "

			sql &= ")"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdyear", year))
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
			listOfParams.Add(New SqlClient.SqlParameter("fak_kanton", MDData.fak_kanton))
			listOfParams.Add(New SqlClient.SqlParameter("ki1_fakmax", ReplaceMissing(MDData.ki1_fakmax, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki2_fakmax", ReplaceMissing(MDData.ki2_fakmax, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki1_std", ReplaceMissing(MDData.ki1_std, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki2_std", ReplaceMissing(MDData.ki2_std, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki1_day", ReplaceMissing(MDData.ki1_day, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki2_day", ReplaceMissing(MDData.ki2_day, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki1_month", ReplaceMissing(MDData.ki1_month, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("ki2_month", ReplaceMissing(MDData.ki2_month, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("changekiin", MDData.changekiin))
			listOfParams.Add(New SqlClient.SqlParameter("au1_std", ReplaceMissing(MDData.au1_std, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("au2_std", ReplaceMissing(MDData.au2_std, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("au1_day", ReplaceMissing(MDData.au1_day, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("au2_day", ReplaceMissing(MDData.au2_day, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("au1_month", ReplaceMissing(MDData.au1_month, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("changeauin", MDData.changeauin))

			listOfParams.Add(New SqlClient.SqlParameter("createdon", Now))
			listOfParams.Add(New SqlClient.SqlParameter("createdfrom", MDData.createdfrom))

			listOfParams.Add(New SqlClient.SqlParameter("fak_name", MDData.fak_name))
			listOfParams.Add(New SqlClient.SqlParameter("fak_zhd", MDData.fak_zhd))
			listOfParams.Add(New SqlClient.SqlParameter("fak_postfach", MDData.fak_postfach))
			listOfParams.Add(New SqlClient.SqlParameter("fak_strasse", MDData.fak_strasse))
			listOfParams.Add(New SqlClient.SqlParameter("fak_plzort", MDData.fak_plzort))
			listOfParams.Add(New SqlClient.SqlParameter("fak_mnr", MDData.fak_mnr))
			listOfParams.Add(New SqlClient.SqlParameter("fak_knr", MDData.fak_knr))
			listOfParams.Add(New SqlClient.SqlParameter("changeauin_2", MDData.changeauin))
			listOfParams.Add(New SqlClient.SqlParameter("changekiin_2", MDData.changekiin))
			listOfParams.Add(New SqlClient.SqlParameter("yminlohn", ReplaceMissing(MDData.yminlohn, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("AtEndBeginES", ReplaceMissing(MDData.AtEndBeginES, False)))
			listOfParams.Add(New SqlClient.SqlParameter("SeeAHVLohnForYear", ReplaceMissing(MDData.SeeAHVLohnForYear, False)))

			'listOfParams.Add(New SqlClient.SqlParameter("bemerkung_1", MDData.bemerkung_1))
			'listOfParams.Add(New SqlClient.SqlParameter("bemerkung_2", MDData.bemerkung_2))
			'listOfParams.Add(New SqlClient.SqlParameter("bemerkung_3", MDData.bemerkung_3))
			'listOfParams.Add(New SqlClient.SqlParameter("bemerkung_4", MDData.bemerkung_4))


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
		''' Delete child and education data.
		''' </summary>
		''' <param name="recid">The id of record.</param>
		Public Function DeleteChildEducationData(ByVal recid As Integer?) As Boolean Implements ITablesDatabaseAccess.DeleteChildEducationData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Delete MD_KiAu "
			sql &= "Where ID = @recid "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", ReplaceMissing(recid, 0)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function



#End Region



#Region "KTG for LMV"

		''' <summary>
		''' Loads KTG- for GAV data.
		''' </summary>
		''' <param name="recid">The id of record.</param>
		''' <returns>The lmvktg data.</returns>
		Public Function LoadAssignedKTGForLmvData(ByVal recid As Integer?, ByVal mdNr As Integer, ByVal mdYear As Integer, ByVal gavNumber As Integer) As lmvKTGData Implements ITablesDatabaseAccess.LoadAssignedKTGForLmvData

			Dim result As lmvKTGData = Nothing

			Dim sql As String = String.Empty

			sql &= "Select "
			sql &= "[ID] "
			sql &= ",[RecNr]"
			sql &= ",Convert(Int, [MDYear]) As MDYear "
			sql &= ",[GAVKanton]"
			sql &= ",[BerufBez]"
			sql &= ",[KK_AN_WA_Proz]"
			sql &= ",[KK_AN_WZ_Proz]"
			sql &= ",[KK_AN_MA_Proz]"
			sql &= ",[KK_AN_MZ_Proz]"
			sql &= ",[KK_AG_WA_Proz]"
			sql &= ",[KK_AG_WZ_Proz]"
			sql &= ",[KK_AG_MA_Proz]"
			sql &= ",[KK_AG_MZ_Proz]"
			sql &= ",[KK_AN_WA_Btr]"
			sql &= ",[KK_AN_WZ_Btr]"
			sql &= ",[KK_AN_MA_Btr]"
			sql &= ",[KK_AN_MZ_Btr]"
			sql &= ",[KK_AG_WA_Btr]"
			sql &= ",[KK_AG_WZ_Btr]"
			sql &= ",[KK_AG_MA_Btr]"
			sql &= ",[KK_AG_MZ_Btr]"
			sql &= ",[GAVNumber]"
			sql &= ",[KK_AN_WA_Proz_72]"
			sql &= ",[KK_AN_WZ_Proz_72]"
			sql &= ",[KK_AN_MA_Proz_72]"
			sql &= ",[KK_AN_MZ_Proz_72]"
			sql &= ",[KK_AG_WA_Proz_72]"
			sql &= ",[KK_AG_WZ_Proz_72]"
			sql &= ",[KK_AG_MA_Proz_72]"
			sql &= ",[KK_AG_MZ_Proz_72]"
			sql &= ",[KK_AN_WA_Btr_72]"
			sql &= ",[KK_AN_WZ_Btr_72]"
			sql &= ",[KK_AN_MA_Btr_72]"
			sql &= ",[KK_AN_MZ_Btr_72]"
			sql &= ",[KK_AG_WA_Btr_72]"
			sql &= ",[KK_AG_WZ_Btr_72]"
			sql &= ",[KK_AG_MA_Btr_72]"
			sql &= ",[KK_AG_MZ_Btr_72]"
			sql &= ",[Createdon]"
			sql &= ",[CreatedFrom]"
			sql &= ",[Changedon]"
			sql &= ",[ChangedFrom]"

			sql &= " From MD_KK_LMV Where "
			If recid.HasValue Then sql &= "ID = @recid And "
			sql &= "GAVNumber = @gavnumber "
			sql &= "And MDYear = @mdyear "
			sql &= "And MDNr = @mdNr"



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If recid.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("recid", recid))
			listOfParams.Add(New SqlClient.SqlParameter("gavnumber", gavNumber))
			listOfParams.Add(New SqlClient.SqlParameter("mdyear", mdYear))
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New lmvKTGData

					result.recid = SafeGetInteger(reader, "id", 0)
					result.recnr = SafeGetInteger(reader, "recnr", 0)
					result.mdyear = SafeGetInteger(reader, "MDYear", 0)
					result.gavnumber = SafeGetInteger(reader, "gavnumber", 0)

					result.berufbez = SafeGetString(reader, "BerufBez")

					result.KK_AN_WA_Proz = SafeGetDecimal(reader, "KK_AN_WA_Proz", 0)
					result.KK_AN_WZ_Proz = SafeGetDecimal(reader, "KK_AN_WZ_Proz", 0)
					result.KK_AN_MA_Proz = SafeGetDecimal(reader, "KK_AN_MA_Proz", 0)
					result.KK_AN_MZ_Proz = SafeGetDecimal(reader, "KK_AN_MZ_Proz", 0)

					result.KK_AG_WA_Proz = SafeGetDecimal(reader, "KK_AG_WA_Proz", 0)
					result.KK_AG_WZ_Proz = SafeGetDecimal(reader, "KK_AG_WZ_Proz", 0)
					result.KK_AG_MA_Proz = SafeGetDecimal(reader, "KK_AG_MA_Proz", 0)
					result.KK_AG_MZ_Proz = SafeGetDecimal(reader, "KK_AG_MZ_Proz", 0)


					result.KK_AN_WA_Btr = SafeGetDecimal(reader, "KK_AN_WA_Btr", 0)
					result.KK_AN_WZ_Btr = SafeGetDecimal(reader, "KK_AN_WZ_Btr", 0)
					result.KK_AN_MA_Btr = SafeGetDecimal(reader, "KK_AN_MA_Btr", 0)
					result.KK_AN_MZ_Btr = SafeGetDecimal(reader, "KK_AN_MZ_Btr", 0)

					result.KK_AG_WA_Btr = SafeGetDecimal(reader, "KK_AG_WA_Btr", 0)
					result.KK_AG_WZ_Btr = SafeGetDecimal(reader, "KK_AG_WZ_Btr", 0)
					result.KK_AG_MA_Btr = SafeGetDecimal(reader, "KK_AG_MA_Btr", 0)
					result.KK_AG_MZ_Btr = SafeGetDecimal(reader, "KK_AG_MZ_Btr", 0)


					result.KK_AN_WA_Proz_72 = SafeGetDecimal(reader, "KK_AN_WA_Proz_72", 0)
					result.KK_AN_WZ_Proz_72 = SafeGetDecimal(reader, "KK_AN_WZ_Proz_72", 0)
					result.KK_AN_MA_Proz_72 = SafeGetDecimal(reader, "KK_AN_MA_Proz_72", 0)
					result.KK_AN_MZ_Proz_72 = SafeGetDecimal(reader, "KK_AN_MZ_Proz_72", 0)

					result.KK_AG_WA_Proz_72 = SafeGetDecimal(reader, "KK_AG_WA_Proz_72", 0)
					result.KK_AG_WZ_Proz_72 = SafeGetDecimal(reader, "KK_AG_WZ_Proz_72", 0)
					result.KK_AG_MA_Proz_72 = SafeGetDecimal(reader, "KK_AG_MA_Proz_72", 0)
					result.KK_AG_MZ_Proz_72 = SafeGetDecimal(reader, "KK_AG_MZ_Proz_72", 0)


					result.KK_AN_WA_Btr_72 = SafeGetDecimal(reader, "KK_AN_WA_Btr_72", 0)
					result.KK_AN_WZ_Btr_72 = SafeGetDecimal(reader, "KK_AN_WZ_Btr_72", 0)
					result.KK_AN_MA_Btr_72 = SafeGetDecimal(reader, "KK_AN_MA_Btr_72", 0)
					result.KK_AN_MZ_Btr_72 = SafeGetDecimal(reader, "KK_AN_MZ_Btr_72", 0)

					result.KK_AG_WA_Btr_72 = SafeGetDecimal(reader, "KK_AG_WA_Btr_72", 0)
					result.KK_AG_WZ_Btr_72 = SafeGetDecimal(reader, "KK_AG_WZ_Btr_72", 0)
					result.KK_AG_MA_Btr_72 = SafeGetDecimal(reader, "KK_AG_MA_Btr_72", 0)
					result.KK_AG_MZ_Btr_72 = SafeGetDecimal(reader, "KK_AG_MZ_Btr_72", 0)

					result.createdon = SafeGetDateTime(reader, "createdon", Nothing)
					result.createdfrom = SafeGetString(reader, "createdfrom")
					result.changedon = SafeGetDateTime(reader, "changedon", Nothing)
					result.changedfrom = SafeGetString(reader, "changedfrom")

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
		''' Loads KTG- for GAV data.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The lmvktg data.</returns>
		Public Function LoadKTGForLmvData(ByVal mdNr As Integer, ByVal year As Integer) As IEnumerable(Of lmvKTGData) Implements ITablesDatabaseAccess.LoadKTGForLmvData

			Dim result As List(Of lmvKTGData) = Nothing

			Dim sql As String = String.Empty

			sql &= "Select "
			sql &= "[ID] "
			sql &= ",[RecNr]"
			sql &= ",Convert(Int, [MDYear]) As MDYear "
			sql &= ",[GAVKanton]"
			sql &= ",[BerufBez]"
			sql &= ",[KK_AN_WA_Proz]"
			sql &= ",[KK_AN_WZ_Proz]"
			sql &= ",[KK_AN_MA_Proz]"
			sql &= ",[KK_AN_MZ_Proz]"
			sql &= ",[KK_AG_WA_Proz]"
			sql &= ",[KK_AG_WZ_Proz]"
			sql &= ",[KK_AG_MA_Proz]"
			sql &= ",[KK_AG_MZ_Proz]"
			sql &= ",[KK_AN_WA_Btr]"
			sql &= ",[KK_AN_WZ_Btr]"
			sql &= ",[KK_AN_MA_Btr]"
			sql &= ",[KK_AN_MZ_Btr]"
			sql &= ",[KK_AG_WA_Btr]"
			sql &= ",[KK_AG_WZ_Btr]"
			sql &= ",[KK_AG_MA_Btr]"
			sql &= ",[KK_AG_MZ_Btr]"
			sql &= ",[GAVNumber]"
			sql &= ",[KK_AN_WA_Proz_72]"
			sql &= ",[KK_AN_WZ_Proz_72]"
			sql &= ",[KK_AN_MA_Proz_72]"
			sql &= ",[KK_AN_MZ_Proz_72]"
			sql &= ",[KK_AG_WA_Proz_72]"
			sql &= ",[KK_AG_WZ_Proz_72]"
			sql &= ",[KK_AG_MA_Proz_72]"
			sql &= ",[KK_AG_MZ_Proz_72]"
			sql &= ",[KK_AN_WA_Btr_72]"
			sql &= ",[KK_AN_WZ_Btr_72]"
			sql &= ",[KK_AN_MA_Btr_72]"
			sql &= ",[KK_AN_MZ_Btr_72]"
			sql &= ",[KK_AG_WA_Btr_72]"
			sql &= ",[KK_AG_WZ_Btr_72]"
			sql &= ",[KK_AG_MA_Btr_72]"
			sql &= ",[KK_AG_MZ_Btr_72]"
			sql &= ",[Createdon]"
			sql &= ",[CreatedFrom]"
			sql &= ",[Changedon]"
			sql &= ",[ChangedFrom]"

			sql &= " From MD_KK_LMV "
			sql &= "Where MDYear = @mdyear "
			sql &= "And MDNr = @mdNr"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDYear", year))
			listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of lmvKTGData)

					While reader.Read


						Dim data = New lmvKTGData()

						data.recid = SafeGetInteger(reader, "id", 0)
						data.recnr = SafeGetInteger(reader, "recnr", 0)
						data.mdyear = SafeGetInteger(reader, "MDYear", 0)
						data.gavnumber = SafeGetInteger(reader, "gavnumber", 0)
						data.mdnr = mdNr

						data.berufbez = SafeGetString(reader, "BerufBez")

						data.KK_AN_WA_Proz = SafeGetDecimal(reader, "KK_AN_WA_Proz", 0)
						data.KK_AN_WZ_Proz = SafeGetDecimal(reader, "KK_AN_WZ_Proz", 0)
						data.KK_AN_MA_Proz = SafeGetDecimal(reader, "KK_AN_MA_Proz", 0)
						data.KK_AN_MZ_Proz = SafeGetDecimal(reader, "KK_AN_MZ_Proz", 0)

						data.KK_AG_WA_Proz = SafeGetDecimal(reader, "KK_AG_WA_Proz", 0)
						data.KK_AG_WZ_Proz = SafeGetDecimal(reader, "KK_AG_WZ_Proz", 0)
						data.KK_AG_MA_Proz = SafeGetDecimal(reader, "KK_AG_MA_Proz", 0)
						data.KK_AG_MZ_Proz = SafeGetDecimal(reader, "KK_AG_MZ_Proz", 0)


						data.KK_AN_WA_Btr = SafeGetDecimal(reader, "KK_AN_WA_Btr", 0)
						data.KK_AN_WZ_Btr = SafeGetDecimal(reader, "KK_AN_WZ_Btr", 0)
						data.KK_AN_MA_Btr = SafeGetDecimal(reader, "KK_AN_MA_Btr", 0)
						data.KK_AN_MZ_Btr = SafeGetDecimal(reader, "KK_AN_MZ_Btr", 0)

						data.KK_AG_WA_Btr = SafeGetDecimal(reader, "KK_AG_WA_Btr", 0)
						data.KK_AG_WZ_Btr = SafeGetDecimal(reader, "KK_AG_WZ_Btr", 0)
						data.KK_AG_MA_Btr = SafeGetDecimal(reader, "KK_AG_MA_Btr", 0)
						data.KK_AG_MZ_Btr = SafeGetDecimal(reader, "KK_AG_MZ_Btr", 0)


						data.KK_AN_WA_Proz_72 = SafeGetDecimal(reader, "KK_AN_WA_Proz_72", 0)
						data.KK_AN_WZ_Proz_72 = SafeGetDecimal(reader, "KK_AN_WZ_Proz_72", 0)
						data.KK_AN_MA_Proz_72 = SafeGetDecimal(reader, "KK_AN_MA_Proz_72", 0)
						data.KK_AN_MZ_Proz_72 = SafeGetDecimal(reader, "KK_AN_MZ_Proz_72", 0)

						data.KK_AG_WA_Proz_72 = SafeGetDecimal(reader, "KK_AG_WA_Proz_72", 0)
						data.KK_AG_WZ_Proz_72 = SafeGetDecimal(reader, "KK_AG_WZ_Proz_72", 0)
						data.KK_AG_MA_Proz_72 = SafeGetDecimal(reader, "KK_AG_MA_Proz_72", 0)
						data.KK_AG_MZ_Proz_72 = SafeGetDecimal(reader, "KK_AG_MZ_Proz_72", 0)


						data.KK_AN_WA_Btr_72 = SafeGetDecimal(reader, "KK_AN_WA_Btr_72", 0)
						data.KK_AN_WZ_Btr_72 = SafeGetDecimal(reader, "KK_AN_WZ_Btr_72", 0)
						data.KK_AN_MA_Btr_72 = SafeGetDecimal(reader, "KK_AN_MA_Btr_72", 0)
						data.KK_AN_MZ_Btr_72 = SafeGetDecimal(reader, "KK_AN_MZ_Btr_72", 0)

						data.KK_AG_WA_Btr_72 = SafeGetDecimal(reader, "KK_AG_WA_Btr_72", 0)
						data.KK_AG_WZ_Btr_72 = SafeGetDecimal(reader, "KK_AG_WZ_Btr_72", 0)
						data.KK_AG_MA_Btr_72 = SafeGetDecimal(reader, "KK_AG_MA_Btr_72", 0)
						data.KK_AG_MZ_Btr_72 = SafeGetDecimal(reader, "KK_AG_MZ_Btr_72", 0)

						data.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						data.createdfrom = SafeGetString(reader, "createdfrom")
						data.changedon = SafeGetDateTime(reader, "changedon", Nothing)
						data.changedfrom = SafeGetString(reader, "changedfrom")


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
		''' Add KTG data for LMV.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The mandant data.</returns>
		Public Function AddKTGForLmvData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As lmvKTGData) As Boolean Implements ITablesDatabaseAccess.AddKTGForLmvData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Insert Into MD_KK_LMV ("

			sql &= "RecNr "
			sql &= ",MDNr "
			sql &= ",MDYear "
			sql &= ",[BerufBez]"
			sql &= ",[KK_AN_WA_Proz]"
			sql &= ",[KK_AN_WZ_Proz]"
			sql &= ",[KK_AN_MA_Proz]"
			sql &= ",[KK_AN_MZ_Proz]"
			sql &= ",[KK_AG_WA_Proz]"
			sql &= ",[KK_AG_WZ_Proz]"
			sql &= ",[KK_AG_MA_Proz]"
			sql &= ",[KK_AG_MZ_Proz]"
			sql &= ",[KK_AN_WA_Btr]"
			sql &= ",[KK_AN_WZ_Btr]"
			sql &= ",[KK_AN_MA_Btr]"
			sql &= ",[KK_AN_MZ_Btr]"
			sql &= ",[KK_AG_WA_Btr]"
			sql &= ",[KK_AG_WZ_Btr]"
			sql &= ",[KK_AG_MA_Btr]"
			sql &= ",[KK_AG_MZ_Btr]"
			sql &= ",[GAVNumber]"
			sql &= ",[KK_AN_WA_Proz_72]"
			sql &= ",[KK_AN_WZ_Proz_72]"
			sql &= ",[KK_AN_MA_Proz_72]"
			sql &= ",[KK_AN_MZ_Proz_72]"
			sql &= ",[KK_AG_WA_Proz_72]"
			sql &= ",[KK_AG_WZ_Proz_72]"
			sql &= ",[KK_AG_MA_Proz_72]"
			sql &= ",[KK_AG_MZ_Proz_72]"
			sql &= ",[KK_AN_WA_Btr_72]"
			sql &= ",[KK_AN_WZ_Btr_72]"
			sql &= ",[KK_AN_MA_Btr_72]"
			sql &= ",[KK_AN_MZ_Btr_72]"
			sql &= ",[KK_AG_WA_Btr_72]"
			sql &= ",[KK_AG_WZ_Btr_72]"
			sql &= ",[KK_AG_MA_Btr_72]"
			sql &= ",[KK_AG_MZ_Btr_72]"
			sql &= ",[Createdon]"
			sql &= ",[CreatedFrom]"

			sql &= ") Values ("

			sql &= "(Select Top 1 RecNr + 1 From MD_KK_LMV Order By RecNr Desc) "
			sql &= ",@MDNr "
			sql &= ",@MDYear "
			sql &= ",@BerufBez "
			sql &= ",@KK_AN_WA_Proz "
			sql &= ",@KK_AN_WZ_Proz "
			sql &= ",@KK_AN_MA_Proz "
			sql &= ",@KK_AN_MZ_Proz "
			sql &= ",@KK_AG_WA_Proz "
			sql &= ",@KK_AG_WZ_Proz "
			sql &= ",@KK_AG_MA_Proz "
			sql &= ",@KK_AG_MZ_Proz "
			sql &= ",@KK_AN_WA_Btr "
			sql &= ",@KK_AN_WZ_Btr "
			sql &= ",@KK_AN_MA_Btr "
			sql &= ",@KK_AN_MZ_Btr "
			sql &= ",@KK_AG_WA_Btr "
			sql &= ",@KK_AG_WZ_Btr "
			sql &= ",@KK_AG_MA_Btr "
			sql &= ",@KK_AG_MZ_Btr "
			sql &= ",@GAVNumber "
			sql &= ",@KK_AN_WA_Proz_72 "
			sql &= ",@KK_AN_WZ_Proz_72 "
			sql &= ",@KK_AN_MA_Proz_72 "
			sql &= ",@KK_AN_MZ_Proz_72 "
			sql &= ",@KK_AG_WA_Proz_72 "
			sql &= ",@KK_AG_WZ_Proz_72 "
			sql &= ",@KK_AG_MA_Proz_72 "
			sql &= ",@KK_AG_MZ_Proz_72 "
			sql &= ",@KK_AN_WA_Btr_72 "
			sql &= ",@KK_AN_WZ_Btr_72 "
			sql &= ",@KK_AN_MA_Btr_72 "
			sql &= ",@KK_AN_MZ_Btr_72 "
			sql &= ",@KK_AG_WA_Btr_72 "
			sql &= ",@KK_AG_WZ_Btr_72 "
			sql &= ",@KK_AG_MA_Btr_72 "
			sql &= ",@KK_AG_MZ_Btr_72 "
			sql &= ",@Createdon"
			sql &= ",@CreatedFrom"

			sql &= ")"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mdyear", ReplaceMissing(year, Now.Year)))

			listOfParams.Add(New SqlClient.SqlParameter("BerufBez", ReplaceMissing(MDData.berufbez, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WA_Proz", ReplaceMissing(MDData.KK_AN_WA_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WZ_Proz", ReplaceMissing(MDData.KK_AN_WZ_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MA_Proz", ReplaceMissing(MDData.KK_AN_MA_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MZ_Proz", ReplaceMissing(MDData.KK_AN_MZ_Proz, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WA_Proz", ReplaceMissing(MDData.KK_AG_WA_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WZ_Proz", ReplaceMissing(MDData.KK_AG_WZ_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MA_Proz", ReplaceMissing(MDData.KK_AG_MA_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MZ_Proz", ReplaceMissing(MDData.KK_AG_MZ_Proz, 0)))


			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WA_Btr", ReplaceMissing(MDData.KK_AN_WA_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WZ_Btr", ReplaceMissing(MDData.KK_AN_WZ_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MA_Btr", ReplaceMissing(MDData.KK_AN_MA_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MZ_Btr", ReplaceMissing(MDData.KK_AN_MZ_Btr, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WA_Btr", ReplaceMissing(MDData.KK_AG_WA_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WZ_Btr", ReplaceMissing(MDData.KK_AG_WZ_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MA_Btr", ReplaceMissing(MDData.KK_AG_MA_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MZ_Btr", ReplaceMissing(MDData.KK_AG_MZ_Btr, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("gavnumber", ReplaceMissing(MDData.gavnumber, 0)))


			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WA_Proz_72", ReplaceMissing(MDData.KK_AN_WA_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WZ_Proz_72", ReplaceMissing(MDData.KK_AN_WZ_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MA_Proz_72", ReplaceMissing(MDData.KK_AN_MA_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MZ_Proz_72", ReplaceMissing(MDData.KK_AN_MZ_Proz_72, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WA_Proz_72", ReplaceMissing(MDData.KK_AG_WA_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WZ_Proz_72", ReplaceMissing(MDData.KK_AG_WZ_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MA_Proz_72", ReplaceMissing(MDData.KK_AG_MA_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MZ_Proz_72", ReplaceMissing(MDData.KK_AG_MZ_Proz_72, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WA_Btr_72", ReplaceMissing(MDData.KK_AN_WA_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WZ_Btr_72", ReplaceMissing(MDData.KK_AN_WZ_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MA_Btr_72", ReplaceMissing(MDData.KK_AN_MA_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MZ_Btr_72", ReplaceMissing(MDData.KK_AN_MZ_Btr_72, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WA_Btr_72", ReplaceMissing(MDData.KK_AG_WA_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WZ_Btr_72", ReplaceMissing(MDData.KK_AG_WZ_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MA_Btr_72", ReplaceMissing(MDData.KK_AG_MA_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MZ_Btr_72", ReplaceMissing(MDData.KK_AG_MZ_Btr_72, 0)))


			listOfParams.Add(New SqlClient.SqlParameter("createdon", Now))
			listOfParams.Add(New SqlClient.SqlParameter("createdfrom", MDData.createdfrom))


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
		''' Update KTG data for LMV.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The mandant data.</returns>
		Public Function UpdateAssignedKTGForLmvData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As lmvKTGData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedKTGForLmvData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Update MD_KK_LMV Set "

			sql &= "MDYear = @MDYear"
			sql &= ",[BerufBez] = @BerufBez"
			sql &= ",[KK_AN_WA_Proz] = @KK_AN_WA_Proz"
			sql &= ",[KK_AN_WZ_Proz] = @KK_AN_WZ_Proz"
			sql &= ",[KK_AN_MA_Proz] = @KK_AN_MA_Proz"
			sql &= ",[KK_AN_MZ_Proz] = @KK_AN_MZ_Proz"
			sql &= ",[KK_AG_WA_Proz] = @KK_AG_WA_Proz"
			sql &= ",[KK_AG_WZ_Proz] = @KK_AG_WZ_Proz"
			sql &= ",[KK_AG_MA_Proz] = @KK_AG_MA_Proz"
			sql &= ",[KK_AG_MZ_Proz] = @KK_AG_MZ_Proz"
			sql &= ",[KK_AN_WA_Btr] = @KK_AN_WA_Btr"
			sql &= ",[KK_AN_WZ_Btr] = @KK_AN_WZ_Btr"
			sql &= ",[KK_AN_MA_Btr] = @KK_AN_MA_Btr"
			sql &= ",[KK_AN_MZ_Btr] = @KK_AN_MZ_Btr"
			sql &= ",[KK_AG_WA_Btr] = @KK_AG_WA_Btr"
			sql &= ",[KK_AG_WZ_Btr] = @KK_AG_WZ_Btr"
			sql &= ",[KK_AG_MA_Btr] = @KK_AG_MA_Btr"
			sql &= ",[KK_AG_MZ_Btr] = @KK_AG_MZ_Btr"
			sql &= ",[GAVNumber] = @GAVNumber"
			sql &= ",[KK_AN_WA_Proz_72] = @KK_AN_WA_Proz_72"
			sql &= ",[KK_AN_WZ_Proz_72] = @KK_AN_WZ_Proz_72"
			sql &= ",[KK_AN_MA_Proz_72] = @KK_AN_MA_Proz_72"
			sql &= ",[KK_AN_MZ_Proz_72] = @KK_AN_MZ_Proz_72"
			sql &= ",[KK_AG_WA_Proz_72] = @KK_AG_WA_Proz_72"
			sql &= ",[KK_AG_WZ_Proz_72] = @KK_AG_WZ_Proz_72"
			sql &= ",[KK_AG_MA_Proz_72] = @KK_AG_MA_Proz_72"
			sql &= ",[KK_AG_MZ_Proz_72] = @KK_AG_MZ_Proz_72"
			sql &= ",[KK_AN_WA_Btr_72] = @KK_AN_WA_Btr_72"
			sql &= ",[KK_AN_WZ_Btr_72] = @KK_AN_WZ_Btr_72"
			sql &= ",[KK_AN_MA_Btr_72] = @KK_AN_MA_Btr_72"
			sql &= ",[KK_AN_MZ_Btr_72] = @KK_AN_MZ_Btr_72"
			sql &= ",[KK_AG_WA_Btr_72] = @KK_AG_WA_Btr_72"
			sql &= ",[KK_AG_WZ_Btr_72] = @KK_AG_WZ_Btr_72"
			sql &= ",[KK_AG_MA_Btr_72] = @KK_AG_MA_Btr_72"
			sql &= ",[KK_AG_MZ_Btr_72] = @KK_AG_MZ_Btr_72"
			sql &= ",[Changedon] = @ChangedOn"
			sql &= ",[ChangedFrom] = @ChangedFrom"


			sql &= " Where ID = @recid"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", MDData.recid))
			listOfParams.Add(New SqlClient.SqlParameter("mdyear", year))

			listOfParams.Add(New SqlClient.SqlParameter("BerufBez", ReplaceMissing(MDData.berufbez, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WA_Proz", ReplaceMissing(MDData.KK_AN_WA_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WZ_Proz", ReplaceMissing(MDData.KK_AN_WZ_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MA_Proz", ReplaceMissing(MDData.KK_AN_MA_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MZ_Proz", ReplaceMissing(MDData.KK_AN_MZ_Proz, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WA_Proz", ReplaceMissing(MDData.KK_AG_WA_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WZ_Proz", ReplaceMissing(MDData.KK_AG_WZ_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MA_Proz", ReplaceMissing(MDData.KK_AG_MA_Proz, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MZ_Proz", ReplaceMissing(MDData.KK_AG_MZ_Proz, 0)))


			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WA_Btr", ReplaceMissing(MDData.KK_AN_WA_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WZ_Btr", ReplaceMissing(MDData.KK_AN_WZ_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MA_Btr", ReplaceMissing(MDData.KK_AN_MA_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MZ_Btr", ReplaceMissing(MDData.KK_AN_MZ_Btr, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WA_Btr", ReplaceMissing(MDData.KK_AG_WA_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WZ_Btr", ReplaceMissing(MDData.KK_AG_WZ_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MA_Btr", ReplaceMissing(MDData.KK_AG_MA_Btr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MZ_Btr", ReplaceMissing(MDData.KK_AG_MZ_Btr, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("gavnumber", ReplaceMissing(MDData.gavnumber, 0)))


			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WA_Proz_72", ReplaceMissing(MDData.KK_AN_WA_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WZ_Proz_72", ReplaceMissing(MDData.KK_AN_WZ_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MA_Proz_72", ReplaceMissing(MDData.KK_AN_MA_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MZ_Proz_72", ReplaceMissing(MDData.KK_AN_MZ_Proz_72, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WA_Proz_72", ReplaceMissing(MDData.KK_AG_WA_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WZ_Proz_72", ReplaceMissing(MDData.KK_AG_WZ_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MA_Proz_72", ReplaceMissing(MDData.KK_AG_MA_Proz_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MZ_Proz_72", ReplaceMissing(MDData.KK_AG_MZ_Proz_72, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WA_Btr_72", ReplaceMissing(MDData.KK_AN_WA_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_WZ_Btr_72", ReplaceMissing(MDData.KK_AN_WZ_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MA_Btr_72", ReplaceMissing(MDData.KK_AN_MA_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AN_MZ_Btr_72", ReplaceMissing(MDData.KK_AN_MZ_Btr_72, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WA_Btr_72", ReplaceMissing(MDData.KK_AG_WA_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_WZ_Btr_72", ReplaceMissing(MDData.KK_AG_WZ_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MA_Btr_72", ReplaceMissing(MDData.KK_AG_MA_Btr_72, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("KK_AG_MZ_Btr_72", ReplaceMissing(MDData.KK_AG_MZ_Btr_72, 0)))


			listOfParams.Add(New SqlClient.SqlParameter("changedon", Now))
			listOfParams.Add(New SqlClient.SqlParameter("changedfrom", MDData.changedfrom))


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
		''' Delete ktg data for lmv.
		''' </summary>
		''' <param name="recid">The id of record.</param>
		Public Function DeleteKTGForLmvData(ByVal recid As Integer?) As Boolean Implements ITablesDatabaseAccess.DeleteKTGForLmvData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Delete MD_KK_LMV "
			sql &= " Where ID = @recid "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", ReplaceMissing(recid, 0)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function



#End Region





#Region "Tagesspesen for LMV"

		''' <summary>
		''' Loads TSpesen- for GAV data.
		''' </summary>
		''' <param name="recid">The id of record.</param>
		''' <returns>The lmvktg data.</returns>
		Public Function LoadAssignedTSpesenForLmvData(ByVal recid As Integer?, ByVal mdNr As Integer, ByVal mdYear As Integer, ByVal gavNumber As Integer) As lmvTSpesenData Implements ITablesDatabaseAccess.LoadAssignedTSpesenForLmvData

			Dim result As lmvTSpesenData = Nothing

			Dim sql As String = String.Empty

			sql &= "Select "
			sql &= "[ID] "
			sql &= ",[RecNr]"
			sql &= ",Convert(Int, [MDYear]) As MDYear "
			sql &= ",[BerufBez]"
			sql &= ",[TSpesen]"
			sql &= ",[TWochenstunden]"
			sql &= ",[GAVNumber]"
			sql &= ",[Createdon]"
			sql &= ",[CreatedFrom]"
			sql &= ",[Changedon]"
			sql &= ",[ChangedFrom]"

			sql &= " From MD_TSP_LMV Where "
			If recid.HasValue Then sql &= "ID = @recid And "
			sql &= "GAVNumber = @gavnumber And MDYear = @mdyear"



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If recid.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("recid", recid))
			listOfParams.Add(New SqlClient.SqlParameter("gavnumber", gavNumber))
			listOfParams.Add(New SqlClient.SqlParameter("mdyear", mdYear))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then
					result = New lmvTSpesenData

					result.recid = SafeGetInteger(reader, "id", 0)
					result.recnr = SafeGetInteger(reader, "recnr", 0)
					result.mdyear = SafeGetInteger(reader, "MDYear", 0)
					result.gavnumber = SafeGetInteger(reader, "gavnumber", 0)
					result.berufbez = SafeGetString(reader, "BerufBez")

					result.TSpesen = SafeGetDecimal(reader, "TSpesen", 0)
					result.TWochenstunden = SafeGetDecimal(reader, "TWochenstunden", 0)

					result.createdon = SafeGetDateTime(reader, "createdon", Nothing)
					result.createdfrom = SafeGetString(reader, "createdfrom")
					result.changedon = SafeGetDateTime(reader, "changedon", Nothing)
					result.changedfrom = SafeGetString(reader, "changedfrom")

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
		''' Loads TSpesen- for GAV data.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The lmvktg data.</returns>
		Public Function LoadTSpesenForLmvData(ByVal mdNr As Integer, ByVal year As Integer) As IEnumerable(Of lmvTSpesenData) Implements ITablesDatabaseAccess.LoadTSpesenForLmvData

			Dim result As List(Of lmvTSpesenData) = Nothing

			Dim sql As String = String.Empty

			sql &= "Select "
			sql &= "[ID] "
			sql &= ",[RecNr]"
			sql &= ",Convert(Int, [MDYear]) As MDYear "
			sql &= ",[BerufBez]"
			sql &= ",[GAVNumber]"

			sql &= ",[TSpesen]"
			sql &= ",[TWochenstunden]"

			sql &= ",[Createdon]"
			sql &= ",[CreatedFrom]"
			sql &= ",[Changedon]"
			sql &= ",[ChangedFrom]"

			sql &= " From MD_TSP_LMV Where MDYear = @mdyear"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdyear", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try
				If (Not reader Is Nothing) Then

					result = New List(Of lmvTSpesenData)

					While reader.Read

						Dim data = New lmvTSpesenData()

						data.recid = SafeGetInteger(reader, "id", 0)
						data.recnr = SafeGetInteger(reader, "recnr", 0)
						data.mdyear = SafeGetInteger(reader, "MDYear", 0)
						data.gavnumber = SafeGetInteger(reader, "gavnumber", 0)
						data.berufbez = SafeGetString(reader, "BerufBez")

						data.TSpesen = SafeGetDecimal(reader, "TSpesen", 0)
						data.TWochenstunden = SafeGetDecimal(reader, "TWochenstunden", 0)

						data.createdon = SafeGetDateTime(reader, "createdon", Nothing)
						data.createdfrom = SafeGetString(reader, "createdfrom")
						data.changedon = SafeGetDateTime(reader, "changedon", Nothing)
						data.changedfrom = SafeGetString(reader, "changedfrom")


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
		''' Add TSpesen data for LMV.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The mandant data.</returns>
		Public Function AddTSpesenForLmvData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As lmvTSpesenData) As Boolean Implements ITablesDatabaseAccess.AddTSpesenForLmvData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Insert Into MD_TSP_LMV ("

			sql &= "RecNr "
			sql &= ",MDNr "
			sql &= ",MDYear "
			sql &= ",[BerufBez]"
			sql &= ",[GAVNumber]"
			sql &= ",[TSpesen]"
			sql &= ",[TWochenstunden]"

			sql &= ",[Createdon]"
			sql &= ",[CreatedFrom]"

			sql &= ") Values ("

			sql &= "(Select Top 1 RecNr + 1 From MD_TSP_LMV Order By RecNr Desc) "
			sql &= ",@MDNr "
			sql &= ",@MDYear "
			sql &= ",@BerufBez "
			sql &= ",@GAVNumber "
			sql &= ",@TSpesen"
			sql &= ",@TWochenstunden"

			sql &= ",@Createdon"
			sql &= ",@CreatedFrom"

			sql &= ")"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("mdnr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mdyear", ReplaceMissing(year, Now.Year)))

			listOfParams.Add(New SqlClient.SqlParameter("BerufBez", ReplaceMissing(MDData.berufbez, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("gavnumber", ReplaceMissing(MDData.gavnumber, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("TSpesen", ReplaceMissing(MDData.TSpesen, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("TWochenstunden", ReplaceMissing(MDData.TWochenstunden, 0)))


			listOfParams.Add(New SqlClient.SqlParameter("createdon", Now))
			listOfParams.Add(New SqlClient.SqlParameter("createdfrom", MDData.createdfrom))


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
		''' Update TSpesen data for LMV.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The mandant data.</returns>
		Public Function UpdateAssignedTSpesenForLmvData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As lmvTSpesenData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedTSpesenForLmvData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Update MD_TSP_LMV Set "

			sql &= "MDNr = @MDnr"
			sql &= ",MDYear = @MDYear"
			sql &= ",[BerufBez] = @BerufBez"
			sql &= ",[GAVNumber] = @GAVNumber"
			sql &= ",[TSpesen] = @TSpesen"
			sql &= ",[TWochenstunden] = @TWochenstunden"
			sql &= ",[Changedon] = @ChangedOn"
			sql &= ",[ChangedFrom] = @ChangedFrom"

			sql &= " Where ID = @recid"


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", MDData.recid))
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(mdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("mdyear", ReplaceMissing(year, Now.Year)))

			listOfParams.Add(New SqlClient.SqlParameter("BerufBez", ReplaceMissing(MDData.berufbez, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("gavnumber", ReplaceMissing(MDData.gavnumber, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("TSpesen", ReplaceMissing(MDData.TSpesen, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("TWochenstunden", ReplaceMissing(MDData.TWochenstunden, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("changedon", Now))
			listOfParams.Add(New SqlClient.SqlParameter("changedfrom", MDData.changedfrom))


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
		''' Delete ktg data for lmv.
		''' </summary>
		''' <param name="recid">The id of record.</param>
		Public Function DeleteTSpesenForLmvData(ByVal recid As Integer?) As Boolean Implements ITablesDatabaseAccess.DeleteTSpesenForLmvData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Delete MD_TSP_LMV "
			sql &= " Where ID = @recid "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", ReplaceMissing(recid, 0)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function



#End Region






#Region "Lohnartenstammdaten"


		''' <summary>
		''' Load assigned LAStamm 
		''' </summary>
		''' <param name="recid">The Rec number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The LAStamm data.</returns>
		Public Function LoadAssignedLAStammData(ByVal recid As Integer, ByVal year As Integer) As LAStammData Implements ITablesDatabaseAccess.LoadAssignedLAStammData

			Dim result As LAStammData = Nothing

			Dim sql As String = String.Empty


			sql &= "SELECT "
			sql &= "[LANr]"
			sql &= ",[LAText]"
			sql &= ",[LALoText]"
			sql &= ",[LAOpText]"
			sql &= ",[Bedingung]"
			sql &= ",[RunFuncBefore]"
			sql &= ",[Verwendung]"
			sql &= ",[Vorzeichen]"
			sql &= ",Convert(int, [Rundung]) Rundung"
			sql &= ",[TypeAnzahl]"
			sql &= ",[MAAnzVar]"
			sql &= ",[FixAnzahl]"
			sql &= ",[Sum0Anzahl]"
			sql &= ",[Sum1Anzahl]"
			sql &= ",[PrintAnzahl]"
			sql &= ",[TypeBasis]"
			sql &= ",[MABasVar]"
			sql &= ",[FixBasis]"
			sql &= ",[Sum0Basis]"
			sql &= ",[Sum1Basis]"
			sql &= ",[Sum2Basis]"
			sql &= ",[PrintBasis]"
			sql &= ",[TypeAnsatz]"
			sql &= ",[MAAnsVar]"
			sql &= ",[FixAnsatz]"
			sql &= ",[SumAnsatz]"
			sql &= ",[PrintAnsatz]"
			sql &= ",[Sum0Betrag]"
			sql &= ",[Sum1Betrag]"
			sql &= ",[Sum2Betrag]"
			sql &= ",[Sum3Betrag]"
			sql &= ",[PrintBetrag]"
			sql &= ",convert(bit, [PrintLA]) PrintLA"
			sql &= ",[BruttoPflichtig]"
			sql &= ",[AHVPflichtig]"
			sql &= ",[ALVPflichtig]"
			sql &= ",[NBUVPflichtig]"
			sql &= ",[UVPflichtig]"
			sql &= ",[BVGPflichtig]"
			sql &= ",[KKPflichtig]"
			sql &= ",[QSTPflichtig]"
			sql &= ",[MWSTPflichtig]"
			sql &= ",[Reserve1]"
			sql &= ",[Reserve2]"
			sql &= ",[Reserve3]"
			sql &= ",[Reserve4]"
			sql &= ",[Reserve5]"
			sql &= ",[FerienInklusiv]"
			sql &= ",[FeierInklusiv]"
			sql &= ",[13Inklusiv]"
			sql &= ",[ByNullCreate]"
			sql &= ",[KDAnzahl]"
			sql &= ",[KDBasis]"
			sql &= ",[KDAnsatz]"
			sql &= ",[SKonto]"
			sql &= ",[HKonto]"
			sql &= ",[VorzeichenLAW]"
			sql &= ",[BruttoLAWPflichtig]"
			sql &= ",[Kumulativ]"
			sql &= ",[LAWFeld]"
			sql &= ",[ES_Pflichtig]"
			sql &= ",[DuppInKD]"
			sql &= ",[Result]"
			sql &= ",Convert(int, [LAJahr]) As LAJahr "
			sql &= ",[nolisting]"
			sql &= ",[ShowInZG]"
			sql &= ",[KumulativMonth]"
			sql &= ",[TagesSpesen]"
			sql &= ",[StdSpesen]"
			sql &= ",[KumLANr]"
			sql &= ",[ID]"
			sql &= ",[LADeactivated]"
			sql &= ",[AGLA]"
			sql &= ",[ProTag]"
			sql &= ",[LOBeleg1]"
			sql &= ",[LOBeleg2]"
			sql &= ",[GleitTime]"
			sql &= ",[AllowedMore_Anz]"
			sql &= ",[AllowedMore_Bas]"
			sql &= ",[AllowedMore_Ans]"
			sql &= ",[AllowedMore_Btr]"
			sql &= ",[Vorzeichen_2]"
			sql &= ",[WarningByZero]"
			sql &= ",[SeeKanton]"
			sql &= ",[ARGB_Verdienst_Unterkunft]"
			sql &= ",[ARGB_Verdienst_Mahlzeit]"
			sql &= ",[CreatedOn]"
			sql &= ",[CreatedFrom]"
			sql &= ",[ChangedOn]"
			sql &= ",[ChangedFrom]"
			sql &= ",[NotForZV]"
			sql &= ",[Vorzeichen_3]"
			sql &= ",[CalcFer13BasAsStd]"
			sql &= ",[DB1_Bruttopflichtig]"
			sql &= ",[Db1_AHVpflichtig]"
			sql &= ",[MoreProz4Fer]"
			sql &= ",[MoreProz4Feier]"
			sql &= ",[MoreProz413]"

			sql &= ",[MoreProz4FerAmount]"
			sql &= ",[MoreProz4FeierAmount]"
			sql &= ",[MoreProz413Amount]"

			sql &= ",[MDNr]"
			sql &= ",[USNr]"
			sql &= ",[LSE_Field]"
			sql &= ",[GroupKey]"

			sql &= " FROM [dbo].[LA] "

			sql &= "Where [LAJahr] = @LAJahr "
			sql &= "And ID = @recid "
			sql &= "Order By LA.LANr "



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", recid))
			listOfParams.Add(New SqlClient.SqlParameter("LAJahr", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New LAStammData

					result.LANr = SafeGetDecimal(reader, "lanr", 0)
					result.LAText = SafeGetString(reader, "latext")
					result.LALoText = SafeGetString(reader, "LALoText")
					result.LAOpText = SafeGetString(reader, "LAOpText")
					result.Bedingung = SafeGetString(reader, "Bedingung")
					result.RunFuncBefore = SafeGetString(reader, "RunFuncBefore")
					result.Verwendung = SafeGetString(reader, "Verwendung")
					result.Vorzeichen = SafeGetString(reader, "Vorzeichen")
					result.Rundung = SafeGetInteger(reader, "Rundung", 0)
					result.TypeAnzahl = SafeGetShort(reader, "TypeAnzahl", 0)
					result.MAAnzVar = SafeGetString(reader, "MAAnzVar")
					result.FixAnzahl = SafeGetDecimal(reader, "FixAnzahl", 0)
					result.Sum0Anzahl = SafeGetShort(reader, "Sum0Anzahl", 0)
					result.Sum1Anzahl = SafeGetShort(reader, "Sum1Anzahl", 0)
					result.PrintAnzahl = SafeGetBoolean(reader, "PrintAnzahl", False)
					result.TypeBasis = SafeGetShort(reader, "TypeBasis", 0)
					result.MABasVar = SafeGetString(reader, "MABasVar")
					result.FixBasis = SafeGetDecimal(reader, "FixBasis", 0)
					result.Sum0Basis = SafeGetShort(reader, "Sum0Basis", 0)
					result.Sum1Basis = SafeGetShort(reader, "Sum1Basis", 0)
					result.Sum2Basis = SafeGetShort(reader, "Sum2Basis", 0)
					result.PrintBasis = SafeGetBoolean(reader, "PrintBasis", False)
					result.TypeAnsatz = SafeGetShort(reader, "TypeAnsatz", 0)
					result.MAAnsVar = SafeGetString(reader, "MAAnsVar")
					result.FixAnsatz = SafeGetDecimal(reader, "FixAnsatz", 0)
					result.SumAnsatz = SafeGetShort(reader, "SumAnsatz", 0)
					result.PrintAnsatz = SafeGetBoolean(reader, "PrintAnsatz", False)
					result.Sum0Betrag = SafeGetShort(reader, "Sum0Betrag", 0)
					result.Sum1Betrag = SafeGetShort(reader, "Sum1Betrag", 0)
					result.Sum2Betrag = SafeGetShort(reader, "Sum2Betrag", 0)
					result.Sum3Betrag = SafeGetShort(reader, "Sum3Betrag", 0)
					result.PrintBetrag = SafeGetBoolean(reader, "PrintBetrag", False)
					result.PrintLA = SafeGetBoolean(reader, "PrintLA", False)
					result.BruttoPflichtig = SafeGetBoolean(reader, "BruttoPflichtig", False)
					result.AHVPflichtig = SafeGetBoolean(reader, "AHVPflichtig", False)
					result.ALVPflichtig = SafeGetBoolean(reader, "ALVPflichtig", False)
					result.NBUVPflichtig = SafeGetBoolean(reader, "NBUVPflichtig", False)
					result.UVPflichtig = SafeGetBoolean(reader, "UVPflichtig", False)
					result.BVGPflichtig = SafeGetBoolean(reader, "BVGPflichtig", False)
					result.KKPflichtig = SafeGetBoolean(reader, "KKPflichtig", False)
					result.QSTPflichtig = SafeGetBoolean(reader, "QSTPflichtig", False)
					result.MWSTPflichtig = SafeGetBoolean(reader, "MWSTPflichtig", False)
					result.Reserve1 = SafeGetBoolean(reader, "Reserve1", False)
					result.Reserve2 = SafeGetBoolean(reader, "Reserve2", False)
					result.Reserve3 = SafeGetBoolean(reader, "Reserve3", False)
					result.Reserve4 = SafeGetBoolean(reader, "Reserve4", False)
					result.Reserve5 = SafeGetBoolean(reader, "Reserve5", False)
					result.FerienInklusiv = SafeGetBoolean(reader, "FerienInklusiv", False)
					result.FeierInklusiv = SafeGetBoolean(reader, "FeierInklusiv", False)
					result._13Inklusiv = SafeGetBoolean(reader, "13Inklusiv", False)
					result.ByNullCreate = SafeGetBoolean(reader, "ByNullCreate", False)
					result.KDAnzahl = SafeGetString(reader, "KDAnzahl")
					result.KDBasis = SafeGetString(reader, "KDBasis")
					result.KDAnsatz = SafeGetString(reader, "KDAnsatz")
					result.SKonto = SafeGetInteger(reader, "SKonto", 0)
					result.HKonto = SafeGetInteger(reader, "HKonto", 0)
					result.VorzeichenLAW = SafeGetString(reader, "VorzeichenLAW")
					result.BruttoLAWPflichtig = SafeGetBoolean(reader, "BruttoLAWPflichtig", False)
					result.Kumulativ = SafeGetBoolean(reader, "Kumulativ", False)
					result.LAWFeld = SafeGetString(reader, "LAWFeld")
					result.ES_Pflichtig = SafeGetBoolean(reader, "ES_Pflichtig", False)
					result.DuppInKD = SafeGetBoolean(reader, "DuppInKD", False)
					result.Result = SafeGetString(reader, "Result")
					result.LAJahr = SafeGetInteger(reader, "LAJahr", 0)
					result.nolisting = SafeGetBoolean(reader, "nolisting", False)
					result.ShowInZG = SafeGetBoolean(reader, "ShowInZG", False)
					result.KumulativMonth = SafeGetBoolean(reader, "KumulativMonth", False)
					result.TagesSpesen = SafeGetBoolean(reader, "TagesSpesen", False)
					result.StdSpesen = SafeGetBoolean(reader, "StdSpesen", False)
					result.KumLANr = SafeGetInteger(reader, "KumLANr", 0)

					result.recid = SafeGetInteger(reader, "ID", 0)
					result.LADeactivated = SafeGetBoolean(reader, "LADeactivated", False)
					result.AGLA = SafeGetBoolean(reader, "AGLA", False)
					result.ProTag = SafeGetBoolean(reader, "ProTag", False)
					result.LOBeleg1 = SafeGetString(reader, "LOBeleg1")
					result.LOBeleg2 = SafeGetString(reader, "LOBeleg2")
					result.GleitTime = SafeGetBoolean(reader, "GleitTime", False)
					result.AllowedMore_Anz = SafeGetBoolean(reader, "AllowedMore_Anz", False)
					result.AllowedMore_Bas = SafeGetBoolean(reader, "AllowedMore_Bas", False)
					result.AllowedMore_Ans = SafeGetBoolean(reader, "AllowedMore_Ans", False)
					result.AllowedMore_Btr = SafeGetBoolean(reader, "AllowedMore_Btr", False)
					result.Vorzeichen_2 = SafeGetString(reader, "Vorzeichen_2")
					result.WarningByZero = SafeGetBoolean(reader, "WarningByZero", False)
					result.SeeKanton = SafeGetBoolean(reader, "SeeKanton", False)
					result.ARGB_Verdienst_Unterkunft = SafeGetBoolean(reader, "ARGB_Verdienst_Unterkunft", False)
					result.ARGB_Verdienst_Mahlzeit = SafeGetBoolean(reader, "ARGB_Verdienst_Mahlzeit", False)

					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					result.ChangedFrom = SafeGetString(reader, "ChangedFrom")
					result.NotForZV = SafeGetBoolean(reader, "NotForZV", False)
					result.Vorzeichen_3 = SafeGetString(reader, "Vorzeichen_3")
					result.CalcFer13BasAsStd = SafeGetBoolean(reader, "CalcFer13BasAsStd", False)
					result.DB1_Bruttopflichtig = SafeGetBoolean(reader, "DB1_Bruttopflichtig", False)
					result.Db1_AHVpflichtig = SafeGetBoolean(reader, "Db1_AHVpflichtig", False)
					result.MoreProz4Fer = SafeGetBoolean(reader, "MoreProz4Fer", False)
					result.MoreProz4Feier = SafeGetBoolean(reader, "MoreProz4Feier", False)
					result.MoreProz413 = SafeGetBoolean(reader, "MoreProz413", False)

					result.MoreProz4FerAmount = SafeGetDecimal(reader, "MoreProz4FerAmount", 0)
					result.MoreProz4FeierAmount = SafeGetDecimal(reader, "MoreProz4FeierAmount", 0)
					result.MoreProz413Amount = SafeGetDecimal(reader, "MoreProz413Amount", 0)

					result.MDNr = SafeGetInteger(reader, "MDNr", 0)
					result.USNr = SafeGetInteger(reader, "USNr", 0)

					result.LSE_Field = SafeGetString(reader, "LSE_Field")
					result.GroupKey = SafeGetDecimal(reader, "GroupKey", Nothing)

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
		''' Loads all LAStamm 
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The LAStamm data.</returns>
		Public Function LoadLAStammData(ByVal mdNr As Integer, ByVal year As Integer) As IEnumerable(Of LAStammData) Implements ITablesDatabaseAccess.LoadLAStammData

			Dim result As List(Of LAStammData) = Nothing

			Dim sql As String = String.Empty


			sql &= "SELECT "
			sql &= "[LANr]"
			sql &= ",[LAText]"
			sql &= ",[LALoText]"
			sql &= ",[LAOpText]"
			sql &= ",IsNull([Bedingung], '') Bedingung"
			sql &= ",[RunFuncBefore]"
			sql &= ",[Verwendung]"
			sql &= ",[Vorzeichen]"
			sql &= ",Convert(Int, [Rundung]) Rundung"
			sql &= ",[TypeAnzahl]"
			sql &= ",[MAAnzVar]"
			sql &= ",[FixAnzahl]"
			sql &= ",[Sum0Anzahl]"
			sql &= ",[Sum1Anzahl]"
			sql &= ",[PrintAnzahl]"
			sql &= ",[TypeBasis]"
			sql &= ",[MABasVar]"
			sql &= ",[FixBasis]"
			sql &= ",[Sum0Basis]"
			sql &= ",[Sum1Basis]"
			sql &= ",[Sum2Basis]"
			sql &= ",[PrintBasis]"
			sql &= ",[TypeAnsatz]"
			sql &= ",[MAAnsVar]"
			sql &= ",[FixAnsatz]"
			sql &= ",[SumAnsatz]"
			sql &= ",[PrintAnsatz]"
			sql &= ",[Sum0Betrag]"
			sql &= ",[Sum1Betrag]"
			sql &= ",[Sum2Betrag]"
			sql &= ",[Sum3Betrag]"
			sql &= ",[PrintBetrag]"
			sql &= ",Convert(Bit, [PrintLA]) PrintLA"
			sql &= ",[BruttoPflichtig]"
			sql &= ",[AHVPflichtig]"
			sql &= ",[ALVPflichtig]"
			sql &= ",[NBUVPflichtig]"
			sql &= ",[UVPflichtig]"
			sql &= ",[BVGPflichtig]"
			sql &= ",[KKPflichtig]"
			sql &= ",[QSTPflichtig]"
			sql &= ",[MWSTPflichtig]"
			sql &= ",[Reserve1]"
			sql &= ",[Reserve2]"
			sql &= ",[Reserve3]"
			sql &= ",[Reserve4]"
			sql &= ",[Reserve5]"
			sql &= ",[FerienInklusiv]"
			sql &= ",[FeierInklusiv]"
			sql &= ",[13Inklusiv]"
			sql &= ",[ByNullCreate]"
			sql &= ",[KDAnzahl]"
			sql &= ",[KDBasis]"
			sql &= ",[KDAnsatz]"
			sql &= ",[SKonto]"
			sql &= ",[HKonto]"
			sql &= ",[VorzeichenLAW]"
			sql &= ",[BruttoLAWPflichtig]"
			sql &= ",[Kumulativ]"
			sql &= ",[LAWFeld]"
			sql &= ",[ES_Pflichtig]"
			sql &= ",[DuppInKD]"
			sql &= ",[Result]"
			sql &= ",Convert(int, [LAJahr]) As LAJahr "
			sql &= ",[nolisting]"
			sql &= ",[ShowInZG]"
			sql &= ",[KumulativMonth]"
			sql &= ",[TagesSpesen]"
			sql &= ",[StdSpesen]"
			sql &= ",[KumLANr]"
			sql &= ",[ID]"
			sql &= ",[LADeactivated]"
			sql &= ",[AGLA]"
			sql &= ",[ProTag]"
			sql &= ",[LOBeleg1]"
			sql &= ",[LOBeleg2]"
			sql &= ",[GleitTime]"
			sql &= ",[AllowedMore_Anz]"
			sql &= ",[AllowedMore_Bas]"
			sql &= ",[AllowedMore_Ans]"
			sql &= ",[AllowedMore_Btr]"
			sql &= ",[Vorzeichen_2]"
			sql &= ",[WarningByZero]"
			sql &= ",[SeeKanton]"
			sql &= ",[ARGB_Verdienst_Unterkunft]"
			sql &= ",[ARGB_Verdienst_Mahlzeit]"
			sql &= ",[CreatedOn]"
			sql &= ",[CreatedFrom]"
			sql &= ",[ChangedOn]"
			sql &= ",[ChangedFrom]"
			sql &= ",[NotForZV]"
			sql &= ",[Vorzeichen_3]"
			sql &= ",[CalcFer13BasAsStd]"
			sql &= ",[DB1_Bruttopflichtig]"
			sql &= ",[Db1_AHVpflichtig]"
			sql &= ",[MoreProz4Fer]"
			sql &= ",[MoreProz4Feier]"
			sql &= ",[MoreProz413]"
			sql &= ",[MoreProz4FerAmount]"
			sql &= ",[MoreProz4FeierAmount]"
			sql &= ",[MoreProz413Amount]"
			sql &= ",[MDNr]"
			sql &= ",[USNr]"
			sql &= ",[LSE_Field]"
			sql &= ",[GroupKey]"

			sql &= " FROM [dbo].[LA]"

			sql &= " Where [LAJahr] = @LAJahr "
			sql &= "Order By LA.LANr "



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("LAJahr", year))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of LAStammData)

					While reader.Read

						Dim data = New LAStammData()

						data.LANr = SafeGetDecimal(reader, "lanr", 0)
						data.LAText = SafeGetString(reader, "latext")
						data.LALoText = SafeGetString(reader, "LALoText")
						data.LAOpText = SafeGetString(reader, "LAOpText")
						data.Bedingung = SafeGetString(reader, "Bedingung")
						data.RunFuncBefore = SafeGetString(reader, "RunFuncBefore")
						data.Verwendung = SafeGetString(reader, "Verwendung")
						data.Vorzeichen = SafeGetString(reader, "Vorzeichen")
						data.Rundung = SafeGetInteger(reader, "Rundung", 0)
						data.TypeAnzahl = SafeGetShort(reader, "TypeAnzahl", 0)
						data.MAAnzVar = SafeGetString(reader, "MAAnzVar")
						data.FixAnzahl = SafeGetDecimal(reader, "FixAnzahl", 0)
						data.Sum0Anzahl = SafeGetShort(reader, "Sum0Anzahl", 0)
						data.Sum1Anzahl = SafeGetShort(reader, "Sum1Anzahl", 0)
						data.PrintAnzahl = SafeGetBoolean(reader, "PrintAnzahl", False)
						data.TypeBasis = SafeGetShort(reader, "TypeBasis", 0)
						data.MABasVar = SafeGetString(reader, "MABasVar")
						data.FixBasis = SafeGetDecimal(reader, "FixBasis", 0)
						data.Sum0Basis = SafeGetShort(reader, "Sum0Basis", 0)
						data.Sum1Basis = SafeGetShort(reader, "Sum1Basis", 0)
						data.Sum2Basis = SafeGetShort(reader, "Sum2Basis", 0)
						data.PrintBasis = SafeGetBoolean(reader, "PrintBasis", False)
						data.TypeAnsatz = SafeGetShort(reader, "TypeAnsatz", 0)
						data.MAAnsVar = SafeGetString(reader, "MAAnsVar")
						data.FixAnsatz = SafeGetDecimal(reader, "FixAnsatz", 0)
						data.SumAnsatz = SafeGetShort(reader, "SumAnsatz", 0)
						data.PrintAnsatz = SafeGetBoolean(reader, "PrintAnsatz", False)
						data.Sum0Betrag = SafeGetShort(reader, "Sum0Betrag", 0)
						data.Sum1Betrag = SafeGetShort(reader, "Sum1Betrag", 0)
						data.Sum2Betrag = SafeGetShort(reader, "Sum2Betrag", 0)
						data.Sum3Betrag = SafeGetShort(reader, "Sum3Betrag", 0)
						data.PrintBetrag = SafeGetBoolean(reader, "PrintBetrag", False)
						data.PrintLA = SafeGetBoolean(reader, "PrintLA", False)
						data.BruttoPflichtig = SafeGetBoolean(reader, "BruttoPflichtig", False)
						data.AHVPflichtig = SafeGetBoolean(reader, "AHVPflichtig", False)
						data.ALVPflichtig = SafeGetBoolean(reader, "ALVPflichtig", False)
						data.NBUVPflichtig = SafeGetBoolean(reader, "NBUVPflichtig", False)
						data.UVPflichtig = SafeGetBoolean(reader, "UVPflichtig", False)
						data.BVGPflichtig = SafeGetBoolean(reader, "BVGPflichtig", False)
						data.KKPflichtig = SafeGetBoolean(reader, "KKPflichtig", False)
						data.QSTPflichtig = SafeGetBoolean(reader, "QSTPflichtig", False)
						data.MWSTPflichtig = SafeGetBoolean(reader, "MWSTPflichtig", False)
						data.Reserve1 = SafeGetBoolean(reader, "Reserve1", False)
						data.Reserve2 = SafeGetBoolean(reader, "Reserve2", False)
						data.Reserve3 = SafeGetBoolean(reader, "Reserve3", False)
						data.Reserve4 = SafeGetBoolean(reader, "Reserve4", False)
						data.Reserve5 = SafeGetBoolean(reader, "Reserve5", False)
						data.FerienInklusiv = SafeGetBoolean(reader, "FerienInklusiv", False)
						data.FeierInklusiv = SafeGetBoolean(reader, "FeierInklusiv", False)
						data._13Inklusiv = SafeGetBoolean(reader, "13Inklusiv", False)
						data.ByNullCreate = SafeGetBoolean(reader, "ByNullCreate", False)
						data.KDAnzahl = SafeGetString(reader, "KDAnzahl")
						data.KDBasis = SafeGetString(reader, "KDBasis")
						data.KDAnsatz = SafeGetString(reader, "KDAnsatz")
						data.SKonto = SafeGetInteger(reader, "SKonto", 0)
						data.HKonto = SafeGetInteger(reader, "HKonto", 0)
						data.VorzeichenLAW = SafeGetString(reader, "VorzeichenLAW")
						data.BruttoLAWPflichtig = SafeGetBoolean(reader, "BruttoLAWPflichtig", False)
						data.Kumulativ = SafeGetBoolean(reader, "Kumulativ", False)
						data.LAWFeld = SafeGetString(reader, "LAWFeld")
						data.ES_Pflichtig = SafeGetBoolean(reader, "ES_Pflichtig", False)
						data.DuppInKD = SafeGetBoolean(reader, "DuppInKD", False)
						data.Result = SafeGetString(reader, "Result")
						data.LAJahr = SafeGetInteger(reader, "LAJahr", 0)
						data.nolisting = SafeGetBoolean(reader, "nolisting", False)
						data.ShowInZG = SafeGetBoolean(reader, "ShowInZG", False)
						data.KumulativMonth = SafeGetBoolean(reader, "KumulativMonth", False)
						data.TagesSpesen = SafeGetBoolean(reader, "TagesSpesen", False)
						data.StdSpesen = SafeGetBoolean(reader, "StdSpesen", False)
						data.KumLANr = SafeGetInteger(reader, "KumLANr", 0)

						data.recid = SafeGetInteger(reader, "ID", 0)
						data.LADeactivated = SafeGetBoolean(reader, "LADeactivated", False)
						data.AGLA = SafeGetBoolean(reader, "AGLA", False)
						data.ProTag = SafeGetBoolean(reader, "ProTag", False)
						data.LOBeleg1 = SafeGetString(reader, "LOBeleg1")
						data.LOBeleg2 = SafeGetString(reader, "LOBeleg2")
						data.GleitTime = SafeGetBoolean(reader, "GleitTime", False)
						data.AllowedMore_Anz = SafeGetBoolean(reader, "AllowedMore_Anz", False)
						data.AllowedMore_Bas = SafeGetBoolean(reader, "AllowedMore_Bas", False)
						data.AllowedMore_Ans = SafeGetBoolean(reader, "AllowedMore_Ans", False)
						data.AllowedMore_Btr = SafeGetBoolean(reader, "AllowedMore_Btr", False)
						data.Vorzeichen_2 = SafeGetString(reader, "Vorzeichen_2")
						data.WarningByZero = SafeGetBoolean(reader, "WarningByZero", False)
						data.SeeKanton = SafeGetBoolean(reader, "SeeKanton", False)
						data.ARGB_Verdienst_Unterkunft = SafeGetBoolean(reader, "ARGB_Verdienst_Unterkunft", False)
						data.ARGB_Verdienst_Mahlzeit = SafeGetBoolean(reader, "ARGB_Verdienst_Mahlzeit", False)

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.createdfrom = SafeGetString(reader, "CreatedFrom")
						data.changedon = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.changedfrom = SafeGetString(reader, "ChangedFrom")
						data.NotForZV = SafeGetBoolean(reader, "NotForZV", False)
						data.Vorzeichen_3 = SafeGetString(reader, "Vorzeichen_3")
						data.CalcFer13BasAsStd = SafeGetBoolean(reader, "CalcFer13BasAsStd", False)
						data.DB1_Bruttopflichtig = SafeGetBoolean(reader, "DB1_Bruttopflichtig", False)
						data.Db1_AHVpflichtig = SafeGetBoolean(reader, "Db1_AHVpflichtig", False)
						data.MoreProz4Fer = SafeGetBoolean(reader, "MoreProz4Fer", False)
						data.MoreProz4Feier = SafeGetBoolean(reader, "MoreProz4Feier", False)
						data.MoreProz413 = SafeGetBoolean(reader, "MoreProz413", False)

						data.MoreProz4FerAmount = SafeGetDecimal(reader, "MoreProz4FerAmount", 0)
						data.MoreProz4FeierAmount = SafeGetDecimal(reader, "MoreProz4FeierAmount", 0)
						data.MoreProz413Amount = SafeGetDecimal(reader, "MoreProz413Amount", 0)

						data.MDNr = SafeGetInteger(reader, "MDNr", 0)
						data.USNr = SafeGetInteger(reader, "USNr", 0)

						data.LSE_Field = SafeGetString(reader, "LSE_Field")
						data.GroupKey = SafeGetDecimal(reader, "GroupKey", Nothing)


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
		''' update assigned lastamm data.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The mandant data.</returns>
		Public Function UpdateAssignedLAStammData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As LAStammData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedLAStammData

			Dim success As Boolean = True

			Dim sql As String


			sql = "Update LA Set "
			sql &= "[LANr] = @LANr"
			sql &= ",[LAText] = @LAText"
			sql &= ",[LALoText] = @LALoText"
			sql &= ",[LAOpText] = @LAOpText"
			sql &= ",[Bedingung] = @Bedingung"
			sql &= ",[RunFuncBefore] = @RunFuncBefore"
			sql &= ",[Verwendung] = @Verwendung"
			sql &= ",[Vorzeichen] = @Vorzeichen"
			sql &= ",[Rundung] = @Rundung"
			sql &= ",[TypeAnzahl] = @TypeAnzahl"
			sql &= ",[MAAnzVar] = @MAAnzVar"
			sql &= ",[FixAnzahl] = @FixAnzahl"
			sql &= ",[Sum0Anzahl] = @Sum0Anzahl"
			sql &= ",[Sum1Anzahl] = @Sum1Anzahl"
			sql &= ",[PrintAnzahl] = @PrintAnzahl"
			sql &= ",[TypeBasis] = @TypeBasis"
			sql &= ",[MABasVar] = @MABasVar"
			sql &= ",[FixBasis] = @FixBasis"
			sql &= ",[Sum0Basis] = @Sum0Basis"
			sql &= ",[Sum1Basis] = @Sum1Basis"
			sql &= ",[Sum2Basis] = @Sum2Basis"
			sql &= ",[PrintBasis] = @PrintBasis"
			sql &= ",[TypeAnsatz] = @TypeAnsatz"
			sql &= ",[MAAnsVar] = @MAAnsVar"
			sql &= ",[FixAnsatz] = @FixAnsatz"
			sql &= ",[SumAnsatz] = @SumAnsatz"
			sql &= ",[PrintAnsatz] = @PrintAnsatz"
			sql &= ",[Sum0Betrag] = @Sum0Betrag"
			sql &= ",[Sum1Betrag] = @Sum1Betrag"
			sql &= ",[Sum2Betrag] = @Sum2Betrag"
			sql &= ",[Sum3Betrag] = @Sum3Betrag"
			sql &= ",[PrintBetrag] = @PrintBetrag"
			sql &= ",[PrintLA] = @PrintLA"
			sql &= ",[BruttoPflichtig] = @BruttoPflichtig"
			sql &= ",[AHVPflichtig] = @AHVPflichtig"
			sql &= ",[ALVPflichtig] = @ALVPflichtig"
			sql &= ",[NBUVPflichtig] = @NBUVPflichtig"
			sql &= ",[UVPflichtig] = @UVPflichtig"
			sql &= ",[BVGPflichtig] = @BVGPflichtig"
			sql &= ",[KKPflichtig] = @KKPflichtig"
			sql &= ",[QSTPflichtig] = @QSTPflichtig"
			sql &= ",[MWSTPflichtig] = @MWSTPflichtig"
			sql &= ",[Reserve1] = @Reserve1"
			sql &= ",[Reserve2] = @Reserve2"
			sql &= ",[Reserve3] = @Reserve3"
			sql &= ",[Reserve4] = @Reserve4"
			sql &= ",[Reserve5] = @Reserve5"
			sql &= ",[FerienInklusiv] = @FerienInklusiv"
			sql &= ",[FeierInklusiv] = @FeierInklusiv"
			sql &= ",[13Inklusiv] = @_13Inklusiv"
			sql &= ",[ByNullCreate] = @ByNullCreate"
			sql &= ",[KDAnzahl] = @KDAnzahl"
			sql &= ",[KDBasis] = @KDBasis"
			sql &= ",[KDAnsatz] = @KDAnsatz"
			sql &= ",[SKonto] = @SKonto"
			sql &= ",[HKonto] = @HKonto"
			sql &= ",[VorzeichenLAW] = @VorzeichenLAW"
			sql &= ",[BruttoLAWPflichtig] = @BruttoLAWPflichtig"
			sql &= ",[Kumulativ] = @Kumulativ"
			sql &= ",[LAWFeld] = @LAWFeld"
			sql &= ",[ES_Pflichtig] = @ES_Pflichtig"
			sql &= ",[DuppInKD] = @DuppInKD"
			sql &= ",[LAJahr] = @LAJahr"
			sql &= ",[nolisting] = @nolisting"
			sql &= ",[ShowInZG] = @ShowInZG"
			sql &= ",[KumulativMonth] = @KumulativMonth"
			sql &= ",[TagesSpesen] = @TagesSpesen"
			sql &= ",[StdSpesen] = @StdSpesen"
			sql &= ",[KumLANr] = @KumLANr"
			sql &= ",[LADeactivated] = @LADeactivated"
			sql &= ",[AGLA] = @AGLA"
			sql &= ",[ProTag] = @ProTag"
			sql &= ",[LOBeleg1] = @LOBeleg1"
			sql &= ",[LOBeleg2] = @LOBeleg2"
			sql &= ",[GleitTime] = @GleitTime"
			sql &= ",[AllowedMore_Anz] = @AllowedMore_Anz"
			sql &= ",[AllowedMore_Bas] = @AllowedMore_Bas"
			sql &= ",[AllowedMore_Ans] = @AllowedMore_Ans"
			sql &= ",[AllowedMore_Btr] = @AllowedMore_Btr"
			sql &= ",[Vorzeichen_2] = @Vorzeichen_2"
			sql &= ",[WarningByZero] = @WarningByZero"
			sql &= ",[SeeKanton] = @SeeKanton"
			sql &= ",[ARGB_Verdienst_Unterkunft] = @ARGB_Verdienst_Unterkunft"
			sql &= ",[ARGB_Verdienst_Mahlzeit] = @ARGB_Verdienst_Mahlzeit"

			sql &= ",[ChangedOn] = GetDate()"
			sql &= ",[ChangedFrom] = @ChangedFrom"
			sql &= ",[NotForZV] = @NotForZV"
			sql &= ",[Vorzeichen_3] = @Vorzeichen_3"
			sql &= ",[CalcFer13BasAsStd] = @CalcFer13BasAsStd"
			sql &= ",[DB1_Bruttopflichtig] = @DB1_Bruttopflichtig"
			sql &= ",[Db1_AHVpflichtig] = @Db1_AHVpflichtig"
			sql &= ",[MoreProz4Fer] = @MoreProz4Fer"
			sql &= ",[MoreProz4Feier] = @MoreProz4Feier"
			sql &= ",[MoreProz413] = @MoreProz413"

			sql &= ",[MoreProz4FerAmount] = @MoreProz4FerAmount"
			sql &= ",[MoreProz4FeierAmount] = @MoreProz4FeierAmount"
			sql &= ",[MoreProz413Amount] = @MoreProz413Amount"

			sql &= ",[MDNr] = @MDNr"
			sql &= ",[USNr] = @USNr "
			sql &= ",[LSE_Field] = @LSE_Field "
			sql &= ",[GroupKey] = @GroupKey "

			sql &= "Where [ID] = @recid; "

			sql &= "If EXISTS(SELECT 1 FROM LA_Translated WHERE LANr = @LANr ) "
			sql &= "BEGIN "
			sql &= "UPDATE LA_Translated "
			sql &= "SET LAText = @latext "
			sql &= "WHERE LANr = @LANr "
			sql &= "End "
			sql &= "Else "
			sql &= "BEGIN "

			sql &= "Insert Into LA_Translated ("
			sql &= "[LANr]"
			sql &= ",[LAText]"

			sql &= ") Values ("

			sql &= "@LANr"
			sql &= ",@latext"

			sql &= "); "

			sql &= "End; "


			'sql &= "Update LA_Translated Set "
			'sql &= "LAText = @latext "
			'sql &= "Where LANr = @lanr; "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", MDData.recid))

			listOfParams.Add(New SqlClient.SqlParameter("lanr", MDData.LANr))
			listOfParams.Add(New SqlClient.SqlParameter("latext", ReplaceMissing(MDData.LAText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LALoText", MDData.LALoText))
			listOfParams.Add(New SqlClient.SqlParameter("LAOpText", MDData.LAOpText))
			listOfParams.Add(New SqlClient.SqlParameter("Bedingung", MDData.Bedingung))
			listOfParams.Add(New SqlClient.SqlParameter("RunFuncBefore", MDData.RunFuncBefore))
			listOfParams.Add(New SqlClient.SqlParameter("Verwendung", MDData.Verwendung))
			listOfParams.Add(New SqlClient.SqlParameter("Vorzeichen", ReplaceMissing(MDData.Vorzeichen, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Rundung", ReplaceMissing(MDData.Rundung, 2)))
			listOfParams.Add(New SqlClient.SqlParameter("TypeAnzahl", MDData.TypeAnzahl))

			listOfParams.Add(New SqlClient.SqlParameter("MAAnzVar", MDData.MAAnzVar))
			listOfParams.Add(New SqlClient.SqlParameter("FixAnzahl", MDData.FixAnzahl))
			listOfParams.Add(New SqlClient.SqlParameter("Sum0Anzahl", MDData.Sum0Anzahl))
			listOfParams.Add(New SqlClient.SqlParameter("Sum1Anzahl", MDData.Sum1Anzahl))
			listOfParams.Add(New SqlClient.SqlParameter("PrintAnzahl", MDData.PrintAnzahl))
			listOfParams.Add(New SqlClient.SqlParameter("TypeBasis", MDData.TypeBasis))
			listOfParams.Add(New SqlClient.SqlParameter("MABasVar", MDData.MABasVar))
			listOfParams.Add(New SqlClient.SqlParameter("FixBasis", MDData.FixBasis))
			listOfParams.Add(New SqlClient.SqlParameter("Sum0Basis", MDData.Sum0Basis))
			listOfParams.Add(New SqlClient.SqlParameter("Sum1Basis", MDData.Sum1Basis))
			listOfParams.Add(New SqlClient.SqlParameter("Sum2Basis", MDData.Sum2Basis))
			listOfParams.Add(New SqlClient.SqlParameter("PrintBasis", MDData.PrintBasis))
			listOfParams.Add(New SqlClient.SqlParameter("TypeAnsatz", MDData.TypeAnsatz))
			listOfParams.Add(New SqlClient.SqlParameter("MAAnsVar", MDData.MAAnsVar))
			listOfParams.Add(New SqlClient.SqlParameter("FixAnsatz", MDData.FixAnsatz))


			listOfParams.Add(New SqlClient.SqlParameter("SumAnsatz", MDData.SumAnsatz))
			listOfParams.Add(New SqlClient.SqlParameter("PrintAnsatz", MDData.PrintAnsatz))
			listOfParams.Add(New SqlClient.SqlParameter("Sum0Betrag", MDData.Sum0Betrag))
			listOfParams.Add(New SqlClient.SqlParameter("Sum1Betrag", MDData.Sum1Betrag))
			listOfParams.Add(New SqlClient.SqlParameter("Sum2Betrag", MDData.Sum2Betrag))
			listOfParams.Add(New SqlClient.SqlParameter("Sum3Betrag", MDData.Sum3Betrag))
			listOfParams.Add(New SqlClient.SqlParameter("PrintBetrag", ReplaceMissing(MDData.PrintBetrag, False)))

			listOfParams.Add(New SqlClient.SqlParameter("PrintLA", ReplaceMissing(MDData.PrintLA, False)))
			listOfParams.Add(New SqlClient.SqlParameter("BruttoPflichtig", ReplaceMissing(MDData.BruttoPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("AHVPflichtig", ReplaceMissing(MDData.AHVPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ALVPflichtig", ReplaceMissing(MDData.ALVPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("NBUVPflichtig", ReplaceMissing(MDData.NBUVPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("UVPflichtig", ReplaceMissing(MDData.UVPflichtig, False)))


			listOfParams.Add(New SqlClient.SqlParameter("BVGPflichtig", ReplaceMissing(MDData.BVGPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("KKPflichtig", ReplaceMissing(MDData.KKPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("QSTPflichtig", ReplaceMissing(MDData.QSTPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("MWSTPflichtig", ReplaceMissing(MDData.MWSTPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve1", ReplaceMissing(MDData.Reserve1, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve2", ReplaceMissing(MDData.Reserve2, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve3", ReplaceMissing(MDData.Reserve3, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve4", ReplaceMissing(MDData.Reserve4, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve5", ReplaceMissing(MDData.Reserve5, False)))
			listOfParams.Add(New SqlClient.SqlParameter("FerienInklusiv", ReplaceMissing(MDData.FerienInklusiv, False)))
			listOfParams.Add(New SqlClient.SqlParameter("FeierInklusiv", ReplaceMissing(MDData.FeierInklusiv, False)))
			listOfParams.Add(New SqlClient.SqlParameter("_13Inklusiv", ReplaceMissing(MDData._13Inklusiv, False)))


			listOfParams.Add(New SqlClient.SqlParameter("ByNullCreate", ReplaceMissing(MDData.ByNullCreate, False)))
			listOfParams.Add(New SqlClient.SqlParameter("BruttoLAWPflichtig", ReplaceMissing(MDData.BruttoLAWPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ES_Pflichtig", ReplaceMissing(MDData.ES_Pflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("DuppInKD", ReplaceMissing(MDData.DuppInKD, False)))
			listOfParams.Add(New SqlClient.SqlParameter("nolisting", ReplaceMissing(MDData.nolisting, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ShowInZG", ReplaceMissing(MDData.ShowInZG, False)))
			listOfParams.Add(New SqlClient.SqlParameter("KumulativMonth", ReplaceMissing(MDData.KumulativMonth, False)))
			listOfParams.Add(New SqlClient.SqlParameter("TagesSpesen", ReplaceMissing(MDData.TagesSpesen, False)))
			listOfParams.Add(New SqlClient.SqlParameter("StdSpesen", ReplaceMissing(MDData.StdSpesen, False)))
			listOfParams.Add(New SqlClient.SqlParameter("KumLANr", ReplaceMissing(MDData.KumLANr, False)))
			listOfParams.Add(New SqlClient.SqlParameter("LADeactivated", ReplaceMissing(MDData.LADeactivated, False)))
			listOfParams.Add(New SqlClient.SqlParameter("AGLA", ReplaceMissing(MDData.AGLA, False)))

			listOfParams.Add(New SqlClient.SqlParameter("ProTag", ReplaceMissing(MDData.ProTag, False)))
			listOfParams.Add(New SqlClient.SqlParameter("GleitTime", ReplaceMissing(MDData.GleitTime, False)))
			listOfParams.Add(New SqlClient.SqlParameter("LAJahr", ReplaceMissing(MDData.LAJahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("WarningByZero", ReplaceMissing(MDData.WarningByZero, False)))
			listOfParams.Add(New SqlClient.SqlParameter("SeeKanton", ReplaceMissing(MDData.SeeKanton, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ARGB_Verdienst_Unterkunft", ReplaceMissing(MDData.ARGB_Verdienst_Unterkunft, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ARGB_Verdienst_Mahlzeit", ReplaceMissing(MDData.ARGB_Verdienst_Mahlzeit, False)))


			listOfParams.Add(New SqlClient.SqlParameter("KDAnzahl", MDData.KDAnzahl))
			listOfParams.Add(New SqlClient.SqlParameter("KDBasis", MDData.KDBasis))
			listOfParams.Add(New SqlClient.SqlParameter("KDAnsatz", MDData.KDAnsatz))


			listOfParams.Add(New SqlClient.SqlParameter("SKonto", ReplaceMissing(MDData.SKonto, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("HKonto", ReplaceMissing(MDData.HKonto, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("VorzeichenLAW", ReplaceMissing(MDData.VorzeichenLAW, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Kumulativ", ReplaceMissing(MDData.Kumulativ, False)))
			listOfParams.Add(New SqlClient.SqlParameter("LAWFeld", ReplaceMissing(MDData.LAWFeld, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("LOBeleg1", ReplaceMissing(MDData.LOBeleg1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("LOBeleg2", ReplaceMissing(MDData.LOBeleg2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedMore_Anz", ReplaceMissing(MDData.AllowedMore_Anz, False)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedMore_Bas", ReplaceMissing(MDData.AllowedMore_Bas, False)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedMore_Ans", ReplaceMissing(MDData.AllowedMore_Ans, False)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedMore_Btr", ReplaceMissing(MDData.AllowedMore_Btr, False)))



			listOfParams.Add(New SqlClient.SqlParameter("Vorzeichen_2", ReplaceMissing(MDData.Vorzeichen_2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", MDData.ChangedFrom))
			listOfParams.Add(New SqlClient.SqlParameter("Vorzeichen_3", ReplaceMissing(MDData.Vorzeichen_3, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("NotForZV", ReplaceMissing(MDData.NotForZV, False)))
			listOfParams.Add(New SqlClient.SqlParameter("CalcFer13BasAsStd", ReplaceMissing(MDData.CalcFer13BasAsStd, False)))
			listOfParams.Add(New SqlClient.SqlParameter("DB1_Bruttopflichtig", ReplaceMissing(MDData.DB1_Bruttopflichtig, False)))


			listOfParams.Add(New SqlClient.SqlParameter("Db1_AHVpflichtig", ReplaceMissing(MDData.Db1_AHVpflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("MoreProz4Fer", ReplaceMissing(MDData.MoreProz4Fer, False)))
			listOfParams.Add(New SqlClient.SqlParameter("MoreProz4Feier", ReplaceMissing(MDData.MoreProz4Feier, False)))
			listOfParams.Add(New SqlClient.SqlParameter("MoreProz413", ReplaceMissing(MDData.MoreProz413, False)))

			listOfParams.Add(New SqlClient.SqlParameter("MoreProz4FerAmount", ReplaceMissing(MDData.MoreProz4FerAmount, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("MoreProz4FeierAmount", ReplaceMissing(MDData.MoreProz4FeierAmount, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("MoreProz413Amount", ReplaceMissing(MDData.MoreProz413Amount, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(MDData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(MDData.USNr, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("LSE_Field", ReplaceMissing(MDData.LSE_Field, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("GroupKey", ReplaceMissing(MDData.GroupKey, DBNull.Value)))

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
		''' update assigned lastamm data.
		''' </summary>
		''' <param name="mdNr">The mandant number.</param>
		''' <param name="year">The year.</param>
		''' <returns>The mandant data.</returns>
		Public Function AddLAStammData(ByVal mdNr As Integer, ByVal year As Integer, ByVal MDData As LAStammData) As Boolean Implements ITablesDatabaseAccess.AddLAStammData

			Dim success As Boolean = True

			Dim sql As String


			sql = "Insert Into LA ("
			sql &= "[LANr]"
			sql &= ",[LAText]"
			sql &= ",[LALoText]"
			sql &= ",[LAOpText]"
			sql &= ",[Bedingung]"
			sql &= ",[RunFuncBefore]"
			sql &= ",[Verwendung]"
			sql &= ",[Vorzeichen]"
			sql &= ",[Rundung]"
			sql &= ",[TypeAnzahl]"
			sql &= ",[MAAnzVar]"
			sql &= ",[FixAnzahl]"
			sql &= ",[Sum0Anzahl]"
			sql &= ",[Sum1Anzahl]"
			sql &= ",[PrintAnzahl]"
			sql &= ",[TypeBasis]"
			sql &= ",[MABasVar]"
			sql &= ",[FixBasis]"
			sql &= ",[Sum0Basis]"
			sql &= ",[Sum1Basis]"
			sql &= ",[Sum2Basis]"
			sql &= ",[PrintBasis]"
			sql &= ",[TypeAnsatz]"
			sql &= ",[MAAnsVar]"
			sql &= ",[FixAnsatz]"
			sql &= ",[SumAnsatz]"
			sql &= ",[PrintAnsatz]"
			sql &= ",[Sum0Betrag]"
			sql &= ",[Sum1Betrag]"
			sql &= ",[Sum2Betrag]"
			sql &= ",[Sum3Betrag]"
			sql &= ",[PrintBetrag]"
			sql &= ",[PrintLA]"
			sql &= ",[BruttoPflichtig]"
			sql &= ",[AHVPflichtig]"
			sql &= ",[ALVPflichtig]"
			sql &= ",[NBUVPflichtig]"
			sql &= ",[UVPflichtig]"
			sql &= ",[BVGPflichtig]"
			sql &= ",[KKPflichtig]"
			sql &= ",[QSTPflichtig]"
			sql &= ",[MWSTPflichtig]"
			sql &= ",[Reserve1]"
			sql &= ",[Reserve2]"
			sql &= ",[Reserve3]"
			sql &= ",[Reserve4]"
			sql &= ",[Reserve5]"
			sql &= ",[FerienInklusiv]"
			sql &= ",[FeierInklusiv]"
			sql &= ",[13Inklusiv]"
			sql &= ",[ByNullCreate]"
			sql &= ",[KDAnzahl]"
			sql &= ",[KDBasis]"
			sql &= ",[KDAnsatz]"
			sql &= ",[SKonto]"
			sql &= ",[HKonto]"
			sql &= ",[VorzeichenLAW]"
			sql &= ",[BruttoLAWPflichtig]"
			sql &= ",[Kumulativ]"
			sql &= ",[LAWFeld]"
			sql &= ",[ES_Pflichtig]"
			sql &= ",[DuppInKD]"
			sql &= ",[LAJahr]"
			sql &= ",[nolisting]"
			sql &= ",[ShowInZG]"
			sql &= ",[KumulativMonth]"
			sql &= ",[TagesSpesen]"
			sql &= ",[StdSpesen]"
			sql &= ",[KumLANr]"
			sql &= ",[LADeactivated]"
			sql &= ",[AGLA]"
			sql &= ",[ProTag]"
			sql &= ",[LOBeleg1]"
			sql &= ",[LOBeleg2]"
			sql &= ",[GleitTime]"
			sql &= ",[AllowedMore_Anz]"
			sql &= ",[AllowedMore_Bas]"
			sql &= ",[AllowedMore_Ans]"
			sql &= ",[AllowedMore_Btr]"
			sql &= ",[Vorzeichen_2]"
			sql &= ",[WarningByZero]"
			sql &= ",[SeeKanton]"
			sql &= ",[ARGB_Verdienst_Unterkunft]"
			sql &= ",[ARGB_Verdienst_Mahlzeit]"

			sql &= ",[CreatedOn]"
			sql &= ",[CreatedFrom]"
			sql &= ",[NotForZV]"
			sql &= ",[Vorzeichen_3]"
			sql &= ",[CalcFer13BasAsStd]"
			sql &= ",[DB1_Bruttopflichtig]"
			sql &= ",[Db1_AHVpflichtig]"
			sql &= ",[MoreProz4Fer]"
			sql &= ",[MoreProz4Feier]"
			sql &= ",[MoreProz413]"
			sql &= ",[MoreProz4FerAmount]"
			sql &= ",[MoreProz4FeierAmount]"
			sql &= ",[MoreProz413Amount]"
			sql &= ",[MDNr]"
			sql &= ",[USNr]"
			sql &= ",[LSE_Field]"
			sql &= ",[GroupKey]"

			sql &= ") Values ("

			sql &= "@LANr"
			sql &= ", @LAText"
			sql &= ", @LALoText"
			sql &= ", @LAOpText"
			sql &= ", @Bedingung"
			sql &= ", @RunFuncBefore"
			sql &= ", @Verwendung"
			sql &= ", @Vorzeichen"
			sql &= ", @Rundung"
			sql &= ", @TypeAnzahl"
			sql &= ", @MAAnzVar"
			sql &= ", @FixAnzahl"
			sql &= ", @Sum0Anzahl"
			sql &= ", @Sum1Anzahl"
			sql &= ", @PrintAnzahl"
			sql &= ", @TypeBasis"
			sql &= ", @MABasVar"
			sql &= ", @FixBasis"
			sql &= ", @Sum0Basis"
			sql &= ", @Sum1Basis"
			sql &= ", @Sum2Basis"
			sql &= ", @PrintBasis"
			sql &= ", @TypeAnsatz"
			sql &= ", @MAAnsVar"
			sql &= ", @FixAnsatz"
			sql &= ", @SumAnsatz"
			sql &= ", @PrintAnsatz"
			sql &= ", @Sum0Betrag"
			sql &= ", @Sum1Betrag"
			sql &= ", @Sum2Betrag"
			sql &= ", @Sum3Betrag"
			sql &= ", @PrintBetrag"
			sql &= ", @PrintLA"
			sql &= ", @BruttoPflichtig"
			sql &= ", @AHVPflichtig"
			sql &= ", @ALVPflichtig"
			sql &= ", @NBUVPflichtig"
			sql &= ", @UVPflichtig"
			sql &= ", @BVGPflichtig"
			sql &= ", @KKPflichtig"
			sql &= ", @QSTPflichtig"
			sql &= ", @MWSTPflichtig"
			sql &= ", @Reserve1"
			sql &= ", @Reserve2"
			sql &= ", @Reserve3"
			sql &= ", @Reserve4"
			sql &= ", @Reserve5"
			sql &= ", @FerienInklusiv"
			sql &= ", @FeierInklusiv"
			sql &= ", @_13Inklusiv"
			sql &= ", @ByNullCreate"
			sql &= ", @KDAnzahl"
			sql &= ", @KDBasis"
			sql &= ", @KDAnsatz"
			sql &= ", @SKonto"
			sql &= ", @HKonto"
			sql &= ", @VorzeichenLAW"
			sql &= ", @BruttoLAWPflichtig"
			sql &= ", @Kumulativ"
			sql &= ", @LAWFeld"
			sql &= ", @ES_Pflichtig"
			sql &= ", @DuppInKD"
			sql &= ", @LAJahr"
			sql &= ", @nolisting"
			sql &= ", @ShowInZG"
			sql &= ", @KumulativMonth"
			sql &= ", @TagesSpesen"
			sql &= ", @StdSpesen"
			sql &= ", @KumLANr"
			sql &= ", @LADeactivated"
			sql &= ", @AGLA"
			sql &= ", @ProTag"
			sql &= ", @LOBeleg1"
			sql &= ", @LOBeleg2"
			sql &= ", @GleitTime"
			sql &= ", @AllowedMore_Anz"
			sql &= ", @AllowedMore_Bas"
			sql &= ", @AllowedMore_Ans"
			sql &= ", @AllowedMore_Btr"
			sql &= ", @Vorzeichen_2"
			sql &= ", @WarningByZero"
			sql &= ", @SeeKanton"
			sql &= ", @ARGB_Verdienst_Unterkunft"
			sql &= ", @ARGB_Verdienst_Mahlzeit"
			sql &= ", GetDate()"
			sql &= ", @CreatedFrom"
			sql &= ", @NotForZV"
			sql &= ", @Vorzeichen_3"
			sql &= ", @CalcFer13BasAsStd"
			sql &= ", @DB1_Bruttopflichtig"
			sql &= ", @Db1_AHVpflichtig"
			sql &= ", @MoreProz4Fer"
			sql &= ", @MoreProz4Feier"
			sql &= ", @MoreProz413"
			sql &= ", @MoreProz4FerAmount"
			sql &= ", @MoreProz4FeierAmount"
			sql &= ", @MoreProz413Amount"
			sql &= ", @MDNr"
			sql &= ", @USNr"
			sql &= ", @LSE_Field"
			sql &= ", @GroupKey"

			sql &= "); "

			sql &= "If EXISTS(SELECT 1 FROM LA_Translated WHERE LANr = @LANr ) "
			sql &= "BEGIN "
			sql &= "UPDATE LA_Translated "
			sql &= "SET LAText = @latext "
			sql &= "WHERE LANr = @LANr "
			sql &= "End "
			sql &= "Else "
			sql &= "BEGIN "

			sql &= "Insert Into LA_Translated ("
			sql &= "[LANr]"
			sql &= ",[LAText]"

			sql &= ") Values ("

			sql &= "@LANr"
			sql &= ",@latext"

			sql &= "); "

			sql &= "End; "

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("lanr", MDData.LANr))
			listOfParams.Add(New SqlClient.SqlParameter("latext", MDData.LAText))
			listOfParams.Add(New SqlClient.SqlParameter("LALoText", MDData.LALoText))
			listOfParams.Add(New SqlClient.SqlParameter("LAOpText", MDData.LAOpText))
			listOfParams.Add(New SqlClient.SqlParameter("Bedingung", MDData.Bedingung))
			listOfParams.Add(New SqlClient.SqlParameter("RunFuncBefore", MDData.RunFuncBefore))
			listOfParams.Add(New SqlClient.SqlParameter("Verwendung", MDData.Verwendung))
			listOfParams.Add(New SqlClient.SqlParameter("Vorzeichen", ReplaceMissing(MDData.Vorzeichen, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Rundung", ReplaceMissing(MDData.Rundung, 2)))
			listOfParams.Add(New SqlClient.SqlParameter("TypeAnzahl", MDData.TypeAnzahl))

			listOfParams.Add(New SqlClient.SqlParameter("MAAnzVar", MDData.MAAnzVar))
			listOfParams.Add(New SqlClient.SqlParameter("FixAnzahl", MDData.FixAnzahl))
			listOfParams.Add(New SqlClient.SqlParameter("Sum0Anzahl", MDData.Sum0Anzahl))
			listOfParams.Add(New SqlClient.SqlParameter("Sum1Anzahl", MDData.Sum1Anzahl))
			listOfParams.Add(New SqlClient.SqlParameter("PrintAnzahl", MDData.PrintAnzahl))
			listOfParams.Add(New SqlClient.SqlParameter("TypeBasis", MDData.TypeBasis))
			listOfParams.Add(New SqlClient.SqlParameter("MABasVar", MDData.MABasVar))
			listOfParams.Add(New SqlClient.SqlParameter("FixBasis", MDData.FixBasis))
			listOfParams.Add(New SqlClient.SqlParameter("Sum0Basis", MDData.Sum0Basis))
			listOfParams.Add(New SqlClient.SqlParameter("Sum1Basis", MDData.Sum1Basis))
			listOfParams.Add(New SqlClient.SqlParameter("Sum2Basis", MDData.Sum2Basis))
			listOfParams.Add(New SqlClient.SqlParameter("PrintBasis", MDData.PrintBasis))
			listOfParams.Add(New SqlClient.SqlParameter("TypeAnsatz", MDData.TypeAnsatz))
			listOfParams.Add(New SqlClient.SqlParameter("MAAnsVar", MDData.MAAnsVar))
			listOfParams.Add(New SqlClient.SqlParameter("FixAnsatz", MDData.FixAnsatz))

			listOfParams.Add(New SqlClient.SqlParameter("SumAnsatz", MDData.SumAnsatz))
			listOfParams.Add(New SqlClient.SqlParameter("PrintAnsatz", MDData.PrintAnsatz))
			listOfParams.Add(New SqlClient.SqlParameter("Sum0Betrag", MDData.Sum0Betrag))
			listOfParams.Add(New SqlClient.SqlParameter("Sum1Betrag", MDData.Sum1Betrag))
			listOfParams.Add(New SqlClient.SqlParameter("Sum2Betrag", MDData.Sum2Betrag))
			listOfParams.Add(New SqlClient.SqlParameter("Sum3Betrag", MDData.Sum3Betrag))
			listOfParams.Add(New SqlClient.SqlParameter("PrintBetrag", ReplaceMissing(MDData.PrintBetrag, False)))

			listOfParams.Add(New SqlClient.SqlParameter("PrintLA", ReplaceMissing(MDData.PrintLA, False)))
			listOfParams.Add(New SqlClient.SqlParameter("BruttoPflichtig", ReplaceMissing(MDData.BruttoPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("AHVPflichtig", ReplaceMissing(MDData.AHVPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ALVPflichtig", ReplaceMissing(MDData.ALVPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("NBUVPflichtig", ReplaceMissing(MDData.NBUVPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("UVPflichtig", ReplaceMissing(MDData.UVPflichtig, False)))


			listOfParams.Add(New SqlClient.SqlParameter("BVGPflichtig", ReplaceMissing(MDData.BVGPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("KKPflichtig", ReplaceMissing(MDData.KKPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("QSTPflichtig", ReplaceMissing(MDData.QSTPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("MWSTPflichtig", ReplaceMissing(MDData.MWSTPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve1", ReplaceMissing(MDData.Reserve1, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve2", ReplaceMissing(MDData.Reserve2, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve3", ReplaceMissing(MDData.Reserve3, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve4", ReplaceMissing(MDData.Reserve4, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Reserve5", ReplaceMissing(MDData.Reserve5, False)))
			listOfParams.Add(New SqlClient.SqlParameter("FerienInklusiv", ReplaceMissing(MDData.FerienInklusiv, False)))
			listOfParams.Add(New SqlClient.SqlParameter("FeierInklusiv", ReplaceMissing(MDData.FeierInklusiv, False)))
			listOfParams.Add(New SqlClient.SqlParameter("_13Inklusiv", ReplaceMissing(MDData._13Inklusiv, False)))


			listOfParams.Add(New SqlClient.SqlParameter("ByNullCreate", ReplaceMissing(MDData.ByNullCreate, False)))
			listOfParams.Add(New SqlClient.SqlParameter("BruttoLAWPflichtig", ReplaceMissing(MDData.BruttoLAWPflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ES_Pflichtig", ReplaceMissing(MDData.ES_Pflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("DuppInKD", ReplaceMissing(MDData.DuppInKD, False)))
			listOfParams.Add(New SqlClient.SqlParameter("nolisting", ReplaceMissing(MDData.nolisting, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ShowInZG", ReplaceMissing(MDData.ShowInZG, False)))
			listOfParams.Add(New SqlClient.SqlParameter("KumulativMonth", ReplaceMissing(MDData.KumulativMonth, False)))
			listOfParams.Add(New SqlClient.SqlParameter("TagesSpesen", ReplaceMissing(MDData.TagesSpesen, False)))
			listOfParams.Add(New SqlClient.SqlParameter("StdSpesen", ReplaceMissing(MDData.StdSpesen, False)))
			listOfParams.Add(New SqlClient.SqlParameter("KumLANr", ReplaceMissing(MDData.KumLANr, False)))
			listOfParams.Add(New SqlClient.SqlParameter("LADeactivated", ReplaceMissing(MDData.LADeactivated, False)))
			listOfParams.Add(New SqlClient.SqlParameter("AGLA", ReplaceMissing(MDData.AGLA, False)))

			listOfParams.Add(New SqlClient.SqlParameter("ProTag", ReplaceMissing(MDData.ProTag, False)))
			listOfParams.Add(New SqlClient.SqlParameter("GleitTime", ReplaceMissing(MDData.GleitTime, False)))
			listOfParams.Add(New SqlClient.SqlParameter("LAJahr", ReplaceMissing(MDData.LAJahr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("WarningByZero", ReplaceMissing(MDData.WarningByZero, False)))
			listOfParams.Add(New SqlClient.SqlParameter("SeeKanton", ReplaceMissing(MDData.SeeKanton, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ARGB_Verdienst_Unterkunft", ReplaceMissing(MDData.ARGB_Verdienst_Unterkunft, False)))
			listOfParams.Add(New SqlClient.SqlParameter("ARGB_Verdienst_Mahlzeit", ReplaceMissing(MDData.ARGB_Verdienst_Mahlzeit, False)))

			listOfParams.Add(New SqlClient.SqlParameter("KDAnzahl", MDData.KDAnzahl))
			listOfParams.Add(New SqlClient.SqlParameter("KDBasis", MDData.KDBasis))
			listOfParams.Add(New SqlClient.SqlParameter("KDAnsatz", MDData.KDAnsatz))

			listOfParams.Add(New SqlClient.SqlParameter("SKonto", ReplaceMissing(MDData.SKonto, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("HKonto", ReplaceMissing(MDData.HKonto, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("VorzeichenLAW", ReplaceMissing(MDData.VorzeichenLAW, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Kumulativ", ReplaceMissing(MDData.Kumulativ, False)))
			listOfParams.Add(New SqlClient.SqlParameter("LAWFeld", ReplaceMissing(MDData.LAWFeld, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("LOBeleg1", ReplaceMissing(MDData.LOBeleg1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("LOBeleg2", ReplaceMissing(MDData.LOBeleg2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedMore_Anz", ReplaceMissing(MDData.AllowedMore_Anz, False)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedMore_Bas", ReplaceMissing(MDData.AllowedMore_Bas, False)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedMore_Ans", ReplaceMissing(MDData.AllowedMore_Ans, False)))
			listOfParams.Add(New SqlClient.SqlParameter("AllowedMore_Btr", ReplaceMissing(MDData.AllowedMore_Btr, False)))

			listOfParams.Add(New SqlClient.SqlParameter("Vorzeichen_2", ReplaceMissing(MDData.Vorzeichen_2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", MDData.ChangedFrom))
			listOfParams.Add(New SqlClient.SqlParameter("Vorzeichen_3", ReplaceMissing(MDData.Vorzeichen_3, String.Empty)))

			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(MDData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("NotForZV", ReplaceMissing(MDData.NotForZV, False)))
			listOfParams.Add(New SqlClient.SqlParameter("CalcFer13BasAsStd", ReplaceMissing(MDData.CalcFer13BasAsStd, False)))
			listOfParams.Add(New SqlClient.SqlParameter("DB1_Bruttopflichtig", ReplaceMissing(MDData.DB1_Bruttopflichtig, False)))

			listOfParams.Add(New SqlClient.SqlParameter("Db1_AHVpflichtig", ReplaceMissing(MDData.Db1_AHVpflichtig, False)))
			listOfParams.Add(New SqlClient.SqlParameter("MoreProz4Fer", ReplaceMissing(MDData.MoreProz4Fer, False)))
			listOfParams.Add(New SqlClient.SqlParameter("MoreProz4Feier", ReplaceMissing(MDData.MoreProz4Feier, False)))
			listOfParams.Add(New SqlClient.SqlParameter("MoreProz413", ReplaceMissing(MDData.MoreProz413, False)))

			listOfParams.Add(New SqlClient.SqlParameter("MoreProz4FerAmount", ReplaceMissing(MDData.MoreProz4FerAmount, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("MoreProz4FeierAmount", ReplaceMissing(MDData.MoreProz4FeierAmount, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("MoreProz413Amount", ReplaceMissing(MDData.MoreProz413Amount, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ReplaceMissing(MDData.MDNr, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(MDData.USNr, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("LSE_Field", ReplaceMissing(MDData.LSE_Field, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("GroupKey", ReplaceMissing(MDData.GroupKey, DBNull.Value)))

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
		''' Delete lastamm data.
		''' </summary>
		''' <param name="recid">The id of record.</param>
		Public Function DeleteLAStammData(ByVal recid As Integer?, ByVal userName As String, ByVal userNr As Integer) As Boolean Implements ITablesDatabaseAccess.DeleteLAStammData

			Dim success As Boolean = True

			Dim sql As String

			sql = "DECLARE @DeleteInfo NVARCHAR(1000) = 'Löschung Lohnartenstamm (LANr=' + Convert(nvarchar(20), IsNull((Select LANr From LA Where ID = @recID), -1)) + ', Jahr=' +  CONVERT(varchar(4), IsNull((Select LAJahr From LA Where ID = @recID), -1), 0)  +')'"
			sql &= "INSERT INTO DeleteInfo(DeletedModul, DeletedDate, UserName, DeletedNr, RecInfo, Result, DeletedDoc, USNr) "
			sql &= "VALUES('LA',  Convert(nvarchar(50), GetDate(), 113), @UserName, IsNull((Select LANr From LA Where ID = @recID), -1),"
			sql &= "@DeleteInfo,"
			sql &= "NULL, Null, @usnr); "

			sql &= "Delete LA_Translated Where LANr = IsNull((Select LANr From LA Where ID = @recID), -1); "

			sql &= "Delete LA "
			sql &= "Where ID = @recid "



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", ReplaceMissing(recid, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("UserName", ReplaceMissing(userName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("usnr", ReplaceMissing(userNr, 0)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

			Catch e As Exception
				success = False
				m_Logger.LogError(e.ToString())

			Finally

			End Try

			Return success

		End Function


#End Region




#Region "Lohnartenstamm translation daten"


		''' <summary>
		''' Load assigned LA translation
		''' </summary>
		''' <param name="recid">The Rec number.</param>
		''' <returns>The LA translation data.</returns>
		Public Function LoadAssignedLATranslationData(ByVal recid As Integer) As LATranslationData Implements ITablesDatabaseAccess.LoadAssignedLATranslationData

			Dim result As LATranslationData = Nothing

			Dim sql As String = String.Empty


			sql = "SELECT "
			sql &= "[LANr]"
			sql &= ",[LAText]"
			sql &= ",[Name_I]"
			sql &= ",[Name_F]"
			sql &= ",[Name_E]"
			sql &= ",[Name_LO_I]"
			sql &= ",[Name_LO_F]"
			sql &= ",[Name_LO_E]"
			sql &= ",[Name_OP_I]"
			sql &= ",[Name_OP_F]"
			sql &= ",[Name_OP_E]"

			sql &= " FROM [dbo].[LA_Translated] "

			sql &= "Where "
			sql &= "ID = @recid "
			sql &= "Order By LANr "



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", recid))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing AndAlso reader.Read()) Then

					result = New LATranslationData

					result.LANr = SafeGetDecimal(reader, "lanr", 0)
					result.LAText = SafeGetString(reader, "latext")
					result.Name_I = SafeGetString(reader, "Name_I")
					result.Name_F = SafeGetString(reader, "Name_F")
					result.Name_E = SafeGetString(reader, "Name_E")
					result.Name_OP_I = SafeGetString(reader, "Name_OP_I")
					result.Name_OP_F = SafeGetString(reader, "Name_OP_F")
					result.Name_OP_E = SafeGetString(reader, "Name_OP_E")
					result.Name_LO_I = SafeGetString(reader, "Name_lO_I")
					result.Name_LO_F = SafeGetString(reader, "Name_lO_F")
					result.Name_LO_E = SafeGetString(reader, "Name_lO_E")

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
		''' Loads all LA translation 
		''' </summary>
		''' <returns>The la translation data.</returns>
		Public Function LoadLATranslationData() As IEnumerable(Of LATranslationData) Implements ITablesDatabaseAccess.LoadLATranslationData

			Dim result As List(Of LATranslationData) = Nothing

			Dim sql As String


			sql = "SELECT "
			sql &= "[ID]"
			sql &= ",[LANr]"
			sql &= ",[LAText]"
			sql &= ",[Name_I]"
			sql &= ",[Name_F]"
			sql &= ",[Name_E]"
			sql &= ",[Name_LO_I]"
			sql &= ",[Name_LO_F]"
			sql &= ",[Name_LO_E]"
			sql &= ",[Name_OP_I]"
			sql &= ",[Name_OP_F]"
			sql &= ",[Name_OP_E]"

			sql &= " FROM [dbo].[LA_Translated] "
			sql &= " Order By LANr "



			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of LATranslationData)

					While reader.Read

						Dim data = New LATranslationData()

						data.recid = SafeGetInteger(reader, "ID", 0)
						data.LANr = SafeGetDecimal(reader, "lanr", 0)
						data.LAText = SafeGetString(reader, "latext")
						data.Name_I = SafeGetString(reader, "Name_I")
						data.Name_F = SafeGetString(reader, "Name_F")
						data.Name_E = SafeGetString(reader, "Name_E")
						data.Name_OP_I = SafeGetString(reader, "Name_OP_I")
						data.Name_OP_F = SafeGetString(reader, "Name_OP_F")
						data.Name_OP_E = SafeGetString(reader, "Name_OP_E")
						data.Name_LO_I = SafeGetString(reader, "Name_lO_I")
						data.Name_LO_F = SafeGetString(reader, "Name_lO_F")
						data.Name_LO_E = SafeGetString(reader, "Name_lO_E")


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
		''' update assigned la translation data.
		''' </summary>
		''' <returns>The mandant data.</returns>
		Public Function UpdateAssignedLATranslationData(ByVal data As LATranslationData) As Boolean Implements ITablesDatabaseAccess.UpdateAssignedLATranslationData

			Dim success As Boolean = True

			Dim sql As String


			sql = "Update LA_Translated Set "
			sql &= "[LANr] = @LANr"
			sql &= ",[LAText] = @LAText"
			sql &= ",[Name_I] = @Name_I"
			sql &= ",[Name_F] = @Name_F"
			sql &= ",[Name_E] = @Name_E"
			sql &= ",[Name_LO_I] = @Name_LO_I"
			sql &= ",[Name_LO_F] = @Name_LO_F"
			sql &= ",[Name_LO_E] = @Name_LO_E"
			sql &= ",[Name_OP_I] = @Name_OP_I"
			sql &= ",[Name_OP_F] = @Name_OP_F"
			sql &= ",[Name_OP_E] = @Name_OP_E"

			sql &= " Where [ID] = @recid "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", ReplaceMissing(data.recid, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("lanr", ReplaceMissing(data.LANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("latext", ReplaceMissing(data.LAText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_I", ReplaceMissing(data.Name_I, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_F", ReplaceMissing(data.Name_F, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_E", ReplaceMissing(data.Name_E, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_LO_I", ReplaceMissing(data.Name_LO_I, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_LO_F", ReplaceMissing(data.Name_LO_F, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_LO_E", ReplaceMissing(data.Name_LO_E, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_OP_I", ReplaceMissing(data.Name_OP_I, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_OP_F", ReplaceMissing(data.Name_OP_F, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_OP_E", ReplaceMissing(data.Name_OP_E, DBNull.Value)))


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
		''' update assigned la translation data.
		''' </summary>
		''' <returns>The la data.</returns>
		Public Function AddLATranslationData(ByVal data As LATranslationData) As Boolean Implements ITablesDatabaseAccess.AddLATranslationData

			Dim success As Boolean = True

			Dim sql As String


			sql = "Insert Into LA_Translated ("
			sql &= "[LANr]"
			sql &= ",[LAText]"
			sql &= ",[Name_I]"
			sql &= ",[Name_F]"
			sql &= ",[Name_E]"
			sql &= ",[Name_LO_I]"
			sql &= ",[Name_LO_F]"
			sql &= ",[Name_LO_E]"
			sql &= ",[Name_OP_I]"
			sql &= ",[Name_OP_F]"
			sql &= ",[Name_OP_E]"

			sql &= ") Values ("

			sql &= "@LANr"
			sql &= ",@LAText"
			sql &= ",@Name_I"
			sql &= ",@Name_F"
			sql &= ",@Name_E"
			sql &= ",@Name_LO_I"
			sql &= ",@Name_LO_F"
			sql &= ",@Name_LO_E"
			sql &= ",@Name_OP_I"
			sql &= ",@Name_OP_F"
			sql &= ",@Name_OP_E"

			sql &= ") "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("lanr", ReplaceMissing(data.LANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("latext", ReplaceMissing(data.LAText, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_I", ReplaceMissing(data.Name_I, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_F", ReplaceMissing(data.Name_F, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_E", ReplaceMissing(data.Name_E, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_LO_I", ReplaceMissing(data.Name_LO_I, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_LO_F", ReplaceMissing(data.Name_LO_F, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_LO_E", ReplaceMissing(data.Name_LO_E, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_OP_I", ReplaceMissing(data.Name_OP_I, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Name_OP_F", ReplaceMissing(data.Name_OP_F, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Name_OP_E", ReplaceMissing(data.Name_OP_E, DBNull.Value)))


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
		''' Delete lastamm data.
		''' </summary>
		''' <param name="recid">The id of record.</param>
		Public Function DeleteLATranslationData(ByVal recid As Integer?) As Boolean Implements ITablesDatabaseAccess.DeleteLATranslationData

			Dim success As Boolean = True

			Dim sql As String = String.Empty

			sql &= "Delete LA_Translated "
			sql &= "Where ID = @recid "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("recid", ReplaceMissing(recid, 0)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)

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
