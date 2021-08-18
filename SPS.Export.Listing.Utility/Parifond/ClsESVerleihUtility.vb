
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient
Imports SPS.Listing.Print.Utility.ESVertrag
Imports SPS.Listing.Print.Utility
Imports SPS.Listing.Print.Utility.ESVerleih

Public Class ClsESVerleihUtility

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

  Private _PControlSetting As New ClsParifondSetting
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private m_Utility As SPProgUtility.MainUtilities.Utilities


  Private ReadOnly Property GetSQL2Open() As String
    Get
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

      Dim strWhereQuery As String = String.Empty
      Dim strESNr As String = String.Empty
      Dim strSqlQuery As String = String.Empty
      Try
        If _PControlSetting.liESNr2Print.Count > 0 Then
          For i As Integer = 0 To _PControlSetting.liESNr2Print.Count - 1
            strESNr &= If(strESNr.Length > 0, ",", "") & _PControlSetting.liESNr2Print.Item(i)
          Next
          strWhereQuery &= String.Format("ES.ESNr In ({0}) ", strESNr)
        End If

				strSqlQuery = "SELECT ES.ID, ES.ESNr, ES.MANr, ES.KDNr, ES.ES_Als, IsNull(ES.KDZHDNr, 0) As KDZHDNr, ESL.GAVGruppe0, MA.Send2WOS As MAWOS, "
				strSqlQuery &= "IsNull(MA.Sprache, 'Deutsch') As MASprache, "
				strSqlQuery &= "(Convert(nvarchar(10), ES.ES_Ab, 104) + ' - ' + IsNull(convert(nvarchar(10), ES.ES_Ende, 104), '')) As Zeitraum, "
				strSqlQuery &= "(MA.Nachname + ', ' + MA.Vorname) As MAName, "
				strSqlQuery &= "KD.Firma1, KD.Send2WOS As KDWOS, IsNull(KD.Sprache, 'Deutsch') As KDSprache "
				strSqlQuery &= "FROM ES "
				strSqlQuery &= "Left Join ESLohn ESL On ES.ESNr = ESL.ESNr And ESL.Aktivlodaten = 1 "
				strSqlQuery &= "Left Join Mitarbeiter MA On ES.MANr = MA.MANr "
				strSqlQuery &= "Left Join Kunden KD On ES.KDNr = KD.KDNr "
        If Not String.IsNullOrWhiteSpace(strWhereQuery) Then strWhereQuery = String.Format("Where {0} ", strWhereQuery)
        strSqlQuery = String.Format("{0} {1} Order By ES.MANr, ES.KDNr", strSqlQuery, strWhereQuery)

      Catch ex As Exception
				m_Logger.LogInfo(String.Format("{0}:{1}", strMethodeName, ex.Message))

			End Try
			Return strSqlQuery

		End Get

	End Property

	Function CreateData4Verleih() As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."
		Dim Conn As SqlConnection = New SqlConnection(_PControlSetting.DbConnString2Open)
		Dim iYear As Integer = 0
		Dim iMonth As Short = 0
		Dim strWhereQuery As String = String.Empty
		Dim bSendPrintJob2WOS As Boolean = False
		Dim bSend_And_PrintJob2WOS As Boolean = False

		Dim SelectedESNr As New List(Of Integer)
		Dim SelectedMANr As New List(Of Integer)
		Dim SelectedKDNr As New List(Of Integer)
		Dim SelectedKDZHDNr As New List(Of Integer)
		Dim SelectedMAData2WOS As New List(Of Boolean)
		Dim SelectedKDData2WOS As New List(Of Boolean)
		Dim SelectedMALang As New List(Of String)
		Dim SelectedKDLang As New List(Of String)

		Try
			SelectedESNr.Clear()
			SelectedMANr.Clear()
			SelectedKDNr.Clear()
			SelectedMAData2WOS.Clear()
			SelectedKDData2WOS.Clear()
			SelectedMALang.Clear()
			SelectedKDLang.Clear()

			Dim strSqlQuery As String = GetSQL2Open()
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim rFrec As SqlDataReader = cmd.ExecuteReader

			Try
				Do While rFrec.Read
					SelectedESNr.Add(m_Utility.SafeGetInteger(rFrec, "ESNr", 0))
					SelectedMANr.Add(m_Utility.SafeGetInteger(rFrec, "MANr", 0))
					SelectedKDNr.Add(m_Utility.SafeGetInteger(rFrec, "KDNr", 0))
					SelectedKDZHDNr.Add(m_Utility.SafeGetInteger(rFrec, "KDZHDNr", 0))

					SelectedMAData2WOS.Add(False)
					SelectedKDData2WOS.Add(False)

					SelectedMALang.Add(m_Utility.SafeGetString(rFrec, "MASprache").Substring(0, 1).ToUpper)
					SelectedKDLang.Add(m_Utility.SafeGetString(rFrec, "KDSprache").Substring(0, 1).ToUpper)

				Loop

			Catch ex As Exception
				m_Logger.LogInfo(String.Format("{0}:Variablen füllen.{1}", strMethodeName, ex.Message))

			End Try

		Catch ex As Exception
			m_Logger.LogInfo(String.Format("{0}:Datenbank-Abfrage.{1}", strMethodeName, ex.Message))

		End Try

		Dim _locSetting As ClsLLESVertragSetting = New ClsLLESVertragSetting With {.DbConnString2Open = _PControlSetting.DbConnString2Open, _
																																						 .SQL2Open = String.Empty, _
																																						 .liESNr2Print = SelectedESNr, _
																																						 .liMANr2Print = SelectedMANr, _
																																						 .liKDNr2Print = SelectedKDNr, _
																																						 .liKDZHDNr2Print = SelectedKDZHDNr, _
																																						 .liSendESMAData2WOS = SelectedMAData2WOS, _
																																						 .liSendESKDData2WOS = SelectedKDData2WOS, _
																																						 .SendAndPrintData2WOS = bSend_And_PrintJob2WOS, _
																																						 .liESSend2WOS = SelectedMAData2WOS, _
																																						 .LiMALang = SelectedMALang, _
																																						 .LiKDLang = SelectedKDLang, _
																																						 .SelectedESNr2Print = 0, _
																																						 .SelectedMANr2Print = 0}
		_locSetting.IsPrintAsVerleih = True
		_locSetting.JobNr2Print = "4.2"

		Dim obj As New ClsPrintESVerleihvertrag(_locSetting)
		strResult = obj.ExportESVerleihvertrag

		Return strResult
	End Function

  Public Sub New(ByVal _MySetting As ClsParifondSetting)
		m_Utility = New SPProgUtility.MainUtilities.Utilities

		Me._PControlSetting = _MySetting
  End Sub

End Class

