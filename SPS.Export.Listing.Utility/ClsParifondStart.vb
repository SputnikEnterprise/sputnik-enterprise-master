
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient

Public Class ClsParifondStart

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

  Private Property _ParifondSetting As New ClsParifondSetting

#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal _MySetting As ClsParifondSetting)

		m_InitializationData = _setting
		Me._ParifondSetting = _MySetting

	End Sub


#End Region

  Function ShowfrmParifondControl() As String
		Dim strResult As String = String.Empty
		Dim strExportFilename As String = String.Empty
		Dim sql As String

		sql = "Select RP.RPNr, RP.Monat, RP.Jahr, RP.LONr, RP.ESNr, RP.MANr, RP.KDNr, "
		sql &= "RP.RPGAV_Beruf, "
		sql &= "Isnull(ES.KDZHDNr, 0) As KDZHDNr,"
		sql &= "IsNull(MA.Send2WOS, 0) As MAWos, MA.Sprache As MASprache, "
		sql &= "IsNull(KD.Send2WOS, 0) As KDWos, KD.Sprache As KDSprache "
		sql &= "From RP "
		sql &= "Left Join ES On RP.ESNr = ES.ESNr "
		sql &= "Left Join Mitarbeiter MA On RP.MANr = MA.MANr "
		sql &= "Left Join Kunden KD On RP.KDNr = KD.KDNr "
		sql &= "Where RP.ESNr In (Select ES_1.ESNr From {0} ES_1 Where ES_1.ESNr = RP.ESNr) "
		sql &= "And RP.LONr > 0 "
		sql &= "And "
		sql &= "((RP.Jahr = @FirstYear And RP.Monat >= @FirstMonth And (@FirstYear <> @LastYear Or "
		sql &= "(RP.Monat >= @FirstMonth And RP.Monat <= @LastMonth))) Or "
		sql &= "(RP.Jahr > @FirstYear And RP.Jahr < @LastYear) Or "
		sql &= "(RP.Jahr = @LastYear And RP.Monat <= @LastMonth And (@FirstYear <> @LastYear Or "
		sql &= "(RP.Monat >= @FirstMonth And RP.Monat <= @LastMonth)))) "

		sql &= "Order By RP.MANr, RP.KDNr, RP.ESNr"
		sql = String.Format(sql, _ParifondSetting.ExportTablename)

		Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@FirstYear", _ParifondSetting.FirstYear)
			param = cmd.Parameters.AddWithValue("@lastYear", _ParifondSetting.LastYear)

			param = cmd.Parameters.AddWithValue("@Firstmonth", _ParifondSetting.FirstMonth)
			param = cmd.Parameters.AddWithValue("@lastMonth", _ParifondSetting.LastMonth)


			Dim rESrec As SqlDataReader = cmd.ExecuteReader


			While rESrec.Read
				Me._ParifondSetting.liESNr2Print.Add(rESrec("ESNr"))
				Me._ParifondSetting.liMANr2Print.Add(rESrec("MANr"))
				Me._ParifondSetting.liKDNr2Print.Add(rESrec("KDNr"))
				Me._ParifondSetting.liKDZHDNr2Print.Add(rESrec("KDZHDNr"))

				Me._ParifondSetting.liRPNr2Print.Add(rESrec("RPNr"))
				Me._ParifondSetting.liLONr2Print.Add(rESrec("LONr"))

			End While

			Me._ParifondSetting.liESNr2Print = Me._ParifondSetting.liESNr2Print.Distinct.ToList()
			Me._ParifondSetting.liMANr2Print = Me._ParifondSetting.liMANr2Print.Distinct.ToList()
			Me._ParifondSetting.liKDNr2Print = Me._ParifondSetting.liKDNr2Print.Distinct.ToList()
			Me._ParifondSetting.liKDZHDNr2Print = Me._ParifondSetting.liKDZHDNr2Print.Distinct.ToList()

			Me._ParifondSetting.liRPNr2Print = Me._ParifondSetting.liRPNr2Print.Distinct.ToList()
			Me._ParifondSetting.liLONr2Print = Me._ParifondSetting.liLONr2Print.Distinct.ToList()


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString))

		End Try

		Dim frm As New frmPControl(m_InitializationData, Me._ParifondSetting)
		frm.Show()

		Return strResult
	End Function



End Class
