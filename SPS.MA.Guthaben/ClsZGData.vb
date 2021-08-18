
Imports System.Data.SqlClient
Imports SP.Infrastructure.Logging


Namespace SPS.MA.ZGFunctionality

	Public Class GuthabenData

		Public Property MAQstCode As String
		Public Property RPTotalBetrag As Decimal
		Public Property RPTotalStd As Decimal
		Public Property ZGTotalBetrag As Decimal
		Public Property LMNegativLohnBetrag As Decimal

		Public Property QSTBetrag As Single

	End Class


	Public Class MAInfo

		Public Property MANachname As String
		Public Property MAVorname As String
		Public Property MAGeschlecht As String
		Public Property MAAnrede As String

		Public Property MAGebDat As Date

	End Class


	Public Class ClsZGData

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Public Shared Function GetMAGuthaben4ZG(ByVal iMANr As Integer) As List(Of GuthabenData)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim dRueckstellung As Decimal = 0
			Dim dBezahlt As Decimal = 0
			Dim dBasis As New List(Of GuthabenData)
			Dim sSql As String = "[Get MAGuthaben Data in ZG]"
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim _ClsReg As New SPProgUtility.ClsDivReg

			Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
			Dim strNegativLohn As String = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, _
													"Vorschusszahlungen", "LANr_4_Allowedpayout")
			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)
				param = cmd.Parameters.AddWithValue("@Monat", Month(Now))
				param = cmd.Parameters.AddWithValue("@Jahr", Year(Now))
				param = cmd.Parameters.AddWithValue("@LANrList", strNegativLohn)

				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader									'
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					dBasis.Add(New GuthabenData With {.MAQstCode = rFoundedrec("MAQstCode"), _
																						.RPTotalBetrag = rFoundedrec("TotalBetragAlle"), _
																						.RPTotalStd = rFoundedrec("TotalBetragStd"), _
																						.ZGTotalBetrag = rFoundedrec("TotalZGBetrag"), _
																						.LMNegativLohnBetrag = rFoundedrec("TotalLMNegativBetrag"), _
																						.QSTBetrag = rFoundedrec("QSTBetrag")})
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return dBasis

			Finally
				Conn.Close()
				Conn.Dispose()

			End Try

			Return dBasis
		End Function

		Public Shared Function GetMAGuthaben4ZGInDetail(ByVal iMANr As Integer) As DataTable
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim _ClsReg As New SPProgUtility.ClsDivReg
			Dim ds As New DataSet
			Dim dt As New DataTable
			Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
			Dim strQuery As String = "[Get MAGuthaben Data in ZG In Detail]"
			Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim strNegativLohn As String = _ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, _
										"Vorschusszahlungen", "LANr_4_Allowedpayout")

			Dim objAdapter As New SqlDataAdapter
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@MANr", iMANr)
			param = cmd.Parameters.AddWithValue("@Monat", Month(Now))
			param = cmd.Parameters.AddWithValue("@Jahr", Year(Now))
			param = cmd.Parameters.AddWithValue("@LANrList", strNegativLohn)

			Try
				objAdapter.SelectCommand = cmd
				objAdapter.Fill(ds, "ZGGuthaben")


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return ds.Tables(0)

			Finally
				Conn.Close()
				Conn.Dispose()

			End Try

			Return ds.Tables(0)
		End Function

		Public Shared Function GetMAInfo(ByVal iMANr As Integer) As List(Of MAInfo)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim MAData As New List(Of MAInfo)
			Dim sSql As String = "[Get MAInfo]"
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim _ClsReg As New SPProgUtility.ClsDivReg

			Dim Conn As SqlConnection = New SqlConnection(_ClsProgSetting.GetConnString)
			Try
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", iMANr)

				cmd.CommandType = Data.CommandType.StoredProcedure

				Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader									'
				rFoundedrec.Read()
				If rFoundedrec.HasRows Then
					MAData.Add(New MAInfo With {.MANachname = rFoundedrec("MANachname"), _
																						.MAVorname = rFoundedrec("MAVorname"), _
																						.MAGeschlecht = rFoundedrec("MAGeschlecht"), _
																						.MAAnrede = rFoundedrec("MAAnrede"), _
																						.MAGebDat = rFoundedrec("MAGebDat")})
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))
				Return MAData

			Finally
				Conn.Close()
				Conn.Dispose()

			End Try

			Return MAData
		End Function

	End Class

End Namespace
