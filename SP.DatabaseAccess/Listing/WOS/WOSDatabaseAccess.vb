
Imports SP.DatabaseAccess.WOS.DataObjects
Imports SPProgUtility.Mandanten
Imports SP.DatabaseAccess
Imports SP.DatabaseAccess.Language
Imports SP.DatabaseAccess.Report.DataObjects
Imports SP.Infrastructure.DateAndTimeCalculation
Imports SP.DatabaseAccess.Report

Namespace WOS

	''' <summary>
	''' Listing database access class.
	''' </summary>
	Public Class WOSDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IWOSDatabaseAccess

#Region "Constructor"

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		''' <remarks></remarks>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
			MyBase.New(connectionString, translationLanguage)

		End Sub

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		''' <remarks></remarks>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
			MyBase.New(connectionString, translationLanguage)
		End Sub

#End Region



		Function UpdateEmployeeGuidData(ByVal employeeNumber As Integer, ByVal newGuid As String) As Boolean Implements IWOSDatabaseAccess.UpdateEmployeeGuidData
			Dim success As Boolean = True

			Dim sql As String

			sql = "Update Mitarbeiter Set Transfered_Guid = @newGuid Where MANr = @MANr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newGuid", ReplaceMissing(newGuid, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		Function UpdateEmployeeWebExportData(ByVal employeeNumber As Integer, ByVal value As Boolean) As Boolean Implements IWOSDatabaseAccess.UpdateEmployeeWebExportData
			Dim success As Boolean = True

			Dim sql As String

			sql = "Update Dbo.MAKontakt_Komm Set WebExport = @value Where MANr = @MANr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@value", ReplaceMissing(value, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function
		Function UpdateCustomerGuidData(ByVal customerNumber As Integer, ByVal newGuid As String) As Boolean Implements IWOSDatabaseAccess.UpdateCustomerGuidData
			Dim success As Boolean = True

			Dim sql As String

			sql = "Update Kunden Set Transfered_Guid = @newGuid Where KDNr = @KDNr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newGuid", ReplaceMissing(newGuid, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		Function UpdateCustomerResponsibleGuidData(ByVal customerNumber As Integer, ByVal recNumber As Integer, ByVal newGuid As String) As Boolean Implements IWOSDatabaseAccess.UpdateCustomerResponsibleGuidData
			Dim success As Boolean = True

			Dim sql As String

			sql = "Update KD_Zustaendig Set Transfered_Guid = @newGuid Where KDNr = @KDNr And RecNr = @recNumber"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(customerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("recNumber", ReplaceMissing(recNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newGuid", ReplaceMissing(newGuid, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		Function UpdateEmploymentGuidData(ByVal esNumber As Integer, ByVal newGuid As String) As Boolean Implements IWOSDatabaseAccess.UpdateEmploymentGuidData
			Dim success As Boolean = True

			Dim sql As String

			sql = "Update ESLohn Set ESDoc_Guid = @newGuid "
			sql &= "Where "
			sql &= "ESNr = @ESNr And Aktivlodaten = 1"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(esNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newGuid", ReplaceMissing(newGuid, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		Function UpdateCustomerEmploymentGuidData(ByVal esNumber As Integer, ByVal newGuid As String) As Boolean Implements IWOSDatabaseAccess.UpdateCustomerEmploymentGuidData
			Dim success As Boolean = True

			Dim sql As String

			sql = "Update ESLohn Set VerleihDoc_Guid = @newGuid "
			sql &= "Where "
			sql &= "ESNr = @ESNr And Aktivlodaten = 1"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(esNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newGuid", ReplaceMissing(newGuid, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		Function UpdateReportGuidData(ByVal rpNumber As Integer, ByVal newGuid As String) As Boolean Implements IWOSDatabaseAccess.UpdateReportGuidData
			Dim success As Boolean = True

			Dim sql As String

			sql = "Update RP Set RPDoc_Guid = @newGuid Where RPNr = @rpNumber"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("rpNumber", ReplaceMissing(rpNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newGuid", ReplaceMissing(newGuid, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		Function UpdateReportLineGuidData(ByVal reportNumber As Integer, ByVal reportlineNumber As Integer, ByVal newGuid As String) As Boolean Implements IWOSDatabaseAccess.UpdateReportLineGuidData
			Dim success As Boolean = True

			Dim sql As String

			sql = "Update RP_ScanDoc Set RPDoc_Guid = @newGuid "
			sql &= "Where RPNr = @reportNumber "
			sql &= "And RPLNr = @reportlineNumber"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("reportNumber", ReplaceMissing(reportNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("reportLineNumber", ReplaceMissing(reportlineNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newGuid", ReplaceMissing(newGuid, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		Function UpdatePayrollGuidData(ByVal loNumber As Integer, ByVal newGuid As String) As Boolean Implements IWOSDatabaseAccess.UpdatePayrollGuidData
			Dim success As Boolean = True

			Dim sql As String

			sql = "Update LO Set LODoc_Guid = @newGuid Where LONr = @loNumber"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("loNumber", ReplaceMissing(loNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newGuid", ReplaceMissing(newGuid, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		Function UpdateInvoiceGuidData(ByVal reNumber As Integer, ByVal newGuid As String) As Boolean Implements IWOSDatabaseAccess.UpdateInvoiceGuidData
			Dim success As Boolean = True

			Dim sql As String

			sql = "Update RE Set REDoc_Guid = @newGuid Where RENr = @reNumber"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("reNumber", ReplaceMissing(reNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newGuid", ReplaceMissing(newGuid, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		Function UpdateProposeGuidData(ByVal proposeNumber As Integer, ByVal newGuid As String) As Boolean Implements IWOSDatabaseAccess.UpdateProposeGuidData
			Dim success As Boolean = True

			Dim sql As String

			sql = "Update Propose Set Doc_Guid = @newGuid "
			sql &= "Where "
			sql &= "ProposeNr = @proposeNr"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("proposeNr", ReplaceMissing(proposeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newGuid", ReplaceMissing(newGuid, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Return success

		End Function

		''' <summary>
		''' Loads RPL list data.
		''' </summary>
		''' <returns>List of RPL list data or nothing in error case.</returns>
		Function LoadRPLineData(ByVal rpNr As Integer, ByVal rplDataType As RPLType, ByVal rpLineNumber As Integer?) As RPLListData Implements IWOSDatabaseAccess.LoadRPLineData

			Dim result As RPLListData = Nothing

			Dim translatedLAColumn As String = String.Empty
			Dim m_DateUtility As New DateAndTimeUtily

			Select Case rplDataType

				Case RPLType.Employee

					translatedLAColumn = "LALoText"

				Case RPLType.Customer
					translatedLAColumn = "LAOpText"

			End Select

			Dim sql As String

			sql = "SELECT RPL.ID, RPL.RPNR, RPL.RPLNr, RPL.KSTNr, RPL.LANR, RPL.ESLohnNr, RPL.RENr, RPL.RPZusatzText, "
			sql &= "RPL.M_Anzahl, RPL.M_Basis, RPL.M_Ansatz, RPL.M_Betrag, "
			sql &= "RPL.K_Anzahl, RPL.K_Basis, RPL.K_Ansatz, RPL.K_Betrag, "
			sql &= "RPL.MWST, RPL.VonDate, RPL.BisDate, RPL.ChangedOn, RPL.ChangedFrom, "
			sql &= "(SELECT COUNT(ID) FROM RP_ScanDoc WHERE RPNR = @rpNr And RPLNr =  RPL.RPLNr) As HasDocument, "
			sql &= String.Format("{0} as TranslatedLAColumn, ", translatedLAColumn)
			sql &= "LA.Vorzeichen "
			sql &= ",ISNULL( (SELECT TOP 1 KST.Bezeichnung KSTBez FROM KD_KST KST WHERE KST.KDNR = RPL.KDNr And KST.RecNr = RPL.KSTNr), '') AS KSTBez "

			sql &= "FROM RPL "
			sql &= "LEFT JOIN LA_Translated ON RPL.LANR = LA_Translated.LANR "
			sql &= "LEFT JOIN LA ON RPL.LANR = LA.LANr AND Year(RPL.VonDate) = LA.LAJahr AND LA.LADeactivated = 0 "
			sql &= "LEFT JOIN RP ON RP.RPNr = RPL.RPNr "

			sql &= "WHERE RPL.rpNr = @rpNr And RPL.ShowinList = 1 AND RPL.KD = @isKD "
			sql &= "And RPL.RPLNr = @RPLNr "
			sql &= "And RPL.VonDate Is Not Null "
			sql &= "And RP.rpNr = @rpNr "

			sql &= "ORDER BY RPL.VonDate DESC, RPL.LANr ASC "


			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("rpNr", rpNr))
			listOfParams.Add(New SqlClient.SqlParameter("isKD", (rplDataType.Equals(RPLType.Customer))))
			listOfParams.Add(New SqlClient.SqlParameter("RPLNr", ReplaceMissing(rpLineNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				result = New RPLListData
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					Dim rplListData = New RPLListData()
					rplListData.ID = SafeGetInteger(reader, "ID", 0)
					rplListData.RPNr = SafeGetInteger(reader, "RPNR", Nothing)
					rplListData.RPLNr = SafeGetInteger(reader, "RPLNr", Nothing)
					rplListData.KSTNr = SafeGetInteger(reader, "KSTNr", Nothing)
					rplListData.kstname = SafeGetString(reader, "KSTBez")
					rplListData.LANr = SafeGetDecimal(reader, "LANr", Nothing)
					rplListData.ESLohnNr = SafeGetInteger(reader, "ESLohnNr", Nothing)
					rplListData.RENr = SafeGetInteger(reader, "RENr", Nothing)
					rplListData.RPZusatzText = SafeGetString(reader, "RPZusatzText")

					Select Case rplDataType
						Case RPLType.Employee
							rplListData.Anzahl = SafeGetDecimal(reader, "M_Anzahl", Nothing)
							rplListData.Basis = SafeGetDecimal(reader, "M_Basis", Nothing)
							rplListData.Ansatz = SafeGetDecimal(reader, "M_Ansatz", Nothing)
							rplListData.Betrag = SafeGetDecimal(reader, "M_Betrag", Nothing)
						Case RPLType.Customer
							rplListData.Anzahl = SafeGetDecimal(reader, "K_Anzahl", Nothing)
							rplListData.Basis = SafeGetDecimal(reader, "K_Basis", Nothing)
							rplListData.Ansatz = SafeGetDecimal(reader, "K_Ansatz", Nothing)
							rplListData.Betrag = SafeGetDecimal(reader, "K_Betrag", Nothing)
						Case Else
							' Do nothing
					End Select

					rplListData.MWST = SafeGetDecimal(reader, "MWST", Nothing)
					rplListData.VonDate = SafeGetDateTime(reader, "VonDate", Nothing)
					rplListData.BisDate = SafeGetDateTime(reader, "BisDate", Nothing)
					rplListData.rpltime = String.Format("{0:d} - {1:d}", rplListData.VonDate, rplListData.BisDate)


					Dim kwdata As Integer() = m_DateUtility.GetCalendarWeeksBetweenDates(rplListData.VonDate, rplListData.BisDate)
					rplListData.rplkwvon = Val(kwdata(0))
					rplListData.rplkwbis = kwdata(kwdata.Length - 1)

					rplListData.rplkw = String.Format("{0}{1}{2}", rplListData.rplkwvon, If(rplListData.rplkwvon = rplListData.rplkwbis, "", " - "),
																								If(rplListData.rplkwvon = rplListData.rplkwbis, "", rplListData.rplkwbis))

					rplListData.Sign = SafeGetString(reader, "Vorzeichen", String.Empty)
					rplListData.TranslatedLAText = SafeGetString(reader, "TranslatedLAColumn")
					rplListData.HasDocument = (SafeGetInteger(reader, "HasDocument", 0) > 0)
					rplListData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
					rplListData.ChangedFrom = SafeGetString(reader, "ChangedFrom", Nothing)
					rplListData.Type = rplDataType

					result = rplListData

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString()))
				result = Nothing

			Finally
				CloseReader(reader)
			End Try

			Return result

		End Function


	End Class


End Namespace


