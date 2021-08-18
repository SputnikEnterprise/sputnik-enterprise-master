
Imports System.IO
Imports System.Data.SqlClient

Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities

Imports SPFibuSearch.ClsDataDetail


Public Class ClsDivFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()


#Region "Diverses"

	'// Get4What._strModul4What
	Dim _strModul4What As String
  Public Property Get4What() As String
    Get
      Return _strModul4What
    End Get
    Set(ByVal value As String)
      _strModul4What = value
    End Set
  End Property

  '// Query.GetSearchQuery
  Dim _strQuery As String
  Public Property GetSearchQuery() As String
    Get
      Return _strQuery
    End Get
    Set(ByVal value As String)
      _strQuery = value
    End Set
  End Property

#End Region

#Region "Funktionen für LvClick in der Suchmaske..."

  '// RPNr
  Dim _strRPNr As String
  Public Property GetRPNr() As String
    Get
      Return _strRPNr
    End Get
    Set(ByVal value As String)
      _strRPNr = value
    End Set
  End Property

  '// MANr
  Dim _strMANr As String
  Public Property GetMANr() As String
    Get
      Return _strMANr
    End Get
    Set(ByVal value As String)
      _strMANr = value
    End Set
  End Property

  '// ESNr
  Dim _strESNr As String
  Public Property GetESNr() As String
    Get
      Return _strESNr
    End Get
    Set(ByVal value As String)
      _strESNr = value
    End Set
  End Property

  '// KDNr
  Dim _strKDNr As String
  Public Property GetKDNr() As String
    Get
      Return _strKDNr
    End Get
    Set(ByVal value As String)
      _strKDNr = value
    End Set
  End Property

#End Region

#Region "LL_Properties"
  '// Print.LLDocName
  Dim _LLDocName As String
  Public Property LLDocName() As String
    Get
      Return _LLDocName
    End Get
    Set(ByVal value As String)
      _LLDocName = value
    End Set
  End Property

  '// Print.LLDocLabel
  Dim _LLDocLabel As String
  Public Property LLDocLabel() As String
    Get
      Return _LLDocLabel
    End Get
    Set(ByVal value As String)
      _LLDocLabel = value
    End Set
  End Property

  '// Print.LLFontDesent
  Dim _LLFontDesent As Integer
  Public Property LLFontDesent() As Integer
    Get
      Return _LLFontDesent
    End Get
    Set(ByVal value As Integer)
      _LLFontDesent = value
    End Set
  End Property

  '// Print.LLIncPrv
  Dim _LLIncPrv As Integer
  Public Property LLIncPrv() As Integer
    Get
      Return _LLIncPrv
    End Get
    Set(ByVal value As Integer)
      _LLIncPrv = value
    End Set
  End Property

  '// Print.LLParamCheck
  Dim _LLParamCheck As Integer
  Public Property LLParamCheck() As Integer
    Get
      Return _LLParamCheck
    End Get
    Set(ByVal value As Integer)
      _LLParamCheck = value
    End Set
  End Property

  '// Print.LLKonvertName
  Dim _LLKonvertName As Integer
  Public Property LLKonvertName() As Integer
    Get
      Return _LLKonvertName
    End Get
    Set(ByVal value As Integer)
      _LLKonvertName = value
    End Set
  End Property

  '// Print.LLZoomProz
  Dim _LLZoomProz As Integer
  Public Property LLZoomProz() As Integer
    Get
      Return _LLZoomProz
    End Get
    Set(ByVal value As Integer)
      _LLZoomProz = value
    End Set
  End Property

  '// Print.LLCopyCount
  Dim _LLCopyCount As Integer
  Public Property LLCopyCount() As Integer
    Get
      Return _LLCopyCount
    End Get
    Set(ByVal value As Integer)
      _LLCopyCount = value
    End Set
  End Property

  '// Print.LLExportedFilePath
  Dim _LLExportedFilePath As String
  Public Property LLExportedFilePath() As String
    Get
      Return _LLExportedFilePath
    End Get
    Set(ByVal value As String)
      _LLExportedFilePath = value
    End Set
  End Property

  '// Print.LLExportedFileName
  Dim _LLExportedFileName As String
  Public Property LLExportedFileName() As String
    Get
      Return _LLExportedFileName
    End Get
    Set(ByVal value As String)
      _LLExportedFileName = value
    End Set
  End Property

  '// Print.LLExportfilter
  Dim _LLExportfilter As String
  Public Property LLExportfilter() As String
    Get
      Return _LLExportfilter
    End Get
    Set(ByVal value As String)
      _LLExportfilter = value
    End Set
  End Property

  '// Print.LLExporterName
  Dim _LLExporterName As String
  Public Property LLExporterName() As String
    Get
      Return _LLExporterName
    End Get
    Set(ByVal value As String)
      _LLExporterName = value
    End Set
  End Property

  '// Print.LLExporterFileName
  Dim _LLExporterFileName As String
  Public Property LLExporterFileName() As String
    Get
      Return _LLExporterFileName
    End Get
    Set(ByVal value As String)
      _LLExporterFileName = value
    End Set
  End Property

  '// Print.LLExporterFileName
  Dim _LLPrintInDiffColor As Boolean
  Public Property LLPrintDiffColor() As Boolean
    Get
      Return _LLPrintInDiffColor
    End Get
    Set(ByVal value As Boolean)
      _LLPrintInDiffColor = value
    End Set
  End Property


#End Region

#Region "US Setting"

  '// USeMail (= eMail des Personalvermittlers)
  Dim _USeMail As String
  Public Property USeMail() As String
    Get
      Return _USeMail
    End Get
    Set(ByVal value As String)
      _USeMail = value
    End Set
  End Property

  '// USTelefon (= USTelefon des Personalvermittlers)
  Dim _USTelefon As String
  Public Property USTelefon() As String
    Get
      Return _USTelefon
    End Get
    Set(ByVal value As String)
      _USTelefon = value
    End Set
  End Property

  '// USTelefax (= USTelefax des Personalvermittlers)
  Dim _USTelefax As String
  Public Property USTelefax() As String
    Get
      Return _USTelefax
    End Get
    Set(ByVal value As String)
      _USTelefax = value
    End Set
  End Property

  '// USVorname (= USVorname des Personalvermittlers)
  Dim _USVorname As String
  Public Property USVorname() As String
    Get
      Return _USVorname
    End Get
    Set(ByVal value As String)
      _USVorname = value
    End Set
  End Property

  '// USAnrede (= USAnrede des Personalvermittlers)
  Dim _USAnrede As String
  Public Property USAnrede() As String
    Get
      Return _USAnrede
    End Get
    Set(ByVal value As String)
      _USAnrede = value
    End Set
  End Property

  '// USNachname (= USNachname des Personalvermittlers)
  Dim _USNachname As String
  Public Property USNachname() As String
    Get
      Return _USNachname
    End Get
    Set(ByVal value As String)
      _USNachname = value
    End Set
  End Property

  '// USMDName (= MDName des Personalvermittlers)
  Dim _USMDname As String
  Public Property USMDname() As String
    Get
      Return _USMDname
    End Get
    Set(ByVal value As String)
      _USMDname = value
    End Set
  End Property

  '// MDName2 (= MDName2 des Personalvermittlers)
  Dim _USMDname2 As String
  Public Property USMDname2() As String
    Get
      Return _USMDname2
    End Get
    Set(ByVal value As String)
      _USMDname2 = value
    End Set
  End Property

  '// MDName3 (= MDName3 des Personalvermittlers)
  Dim _USMDname3 As String
  Public Property USMDname3() As String
    Get
      Return _USMDname3
    End Get
    Set(ByVal value As String)
      _USMDname3 = value
    End Set
  End Property

  '// USMDPostfach (= MDPostfach des Personalvermittlers)
  Dim _USMDPostfach As String
  Public Property USMDPostfach() As String
    Get
      Return _USMDPostfach
    End Get
    Set(ByVal value As String)
      _USMDPostfach = value
    End Set
  End Property

  '// USMDStrasse (= MDstrasse des Personalvermittlers)
  Dim _USMDStrasse As String
  Public Property USMDStrasse() As String
    Get
      Return _USMDStrasse
    End Get
    Set(ByVal value As String)
      _USMDStrasse = value
    End Set
  End Property

  '// USMDOrt (= MDOrt des Personalvermittlers)
  Dim _USMDOrt As String
  Public Property USMDOrt() As String
    Get
      Return _USMDOrt
    End Get
    Set(ByVal value As String)
      _USMDOrt = value
    End Set
  End Property

  '// USMDPLZ (= MDPLZ des Personalvermittlers)
  Dim _USMDPlz As String
  Public Property USMDPlz() As String
    Get
      Return _USMDPlz
    End Get
    Set(ByVal value As String)
      _USMDPlz = value
    End Set
  End Property

  '// USMDLand (= MDLand des Personalvermittlers)
  Dim _USMDLand As String
  Public Property USMDLand() As String
    Get
      Return _USMDLand
    End Get
    Set(ByVal value As String)
      _USMDLand = value
    End Set
  End Property

  '// USMDTelefon (= MDTelefon des Personalvermittlers)
  Dim _USMDTelefon As String
  Public Property USMDTelefon() As String
    Get
      Return _USMDTelefon
    End Get
    Set(ByVal value As String)
      _USMDTelefon = value
    End Set
  End Property

  '// USMDTelefax (= MDTelefax des Personalvermittlers)
  Dim _USMDTelefax As String
  Public Property USMDTelefax() As String
    Get
      Return _USMDTelefax
    End Get
    Set(ByVal value As String)
      _USMDTelefax = value
    End Set
  End Property

  '// USMDeMail (= MDeMail des Personalvermittlers)
  Dim _USMDeMail As String
  Public Property USMDeMail() As String
    Get
      Return _USMDeMail
    End Get
    Set(ByVal value As String)
      _USMDeMail = value
    End Set
  End Property

  '// USMDHomepage (= MDHomepage des Personalvermittlers)
  Dim _USMDHomepage As String
  Public Property USMDHomepage() As String
    Get
      Return _USMDHomepage
    End Get
    Set(ByVal value As String)
      _USMDHomepage = value
    End Set
  End Property

#End Region

#Region "Funktionen..."

  Sub GetBetragSign()
    Dim bResult As Boolean
    Dim _ClsSystem As New SPProgUtility.ClsProgSettingPath
    Dim _ClsReg As New SPProgUtility.ClsDivReg
    Dim strQuery As String = "//SPSZGSearch/ZGSearch/SQLString[@ID=" & Chr(34) & ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/BetragSign"

    Dim strBez As String = _ClsReg.GetXMLNodeValue(_ClsSystem.GetSQLDataFile(), strQuery)
    If strBez <> String.Empty Then
      If strBez = CStr(1) Then bResult = True
    End If

  End Sub

#End Region

End Class

Public Class ClsDbFunc

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsFunc As New ClsDivFunc
	Private _ClsReg As New SPProgUtility.ClsDivReg

	Private iLogedUSNr As Integer = 0
	Private rRPrec As SqlDataReader

	Private m_md As Mandant
	Private m_common As CommonSetting
	Private m_utility As Utilities
	Private m_UtilityUi As SP.Infrastructure.UI.UtilityUI

#Region "Contructor"

	Public Sub New()

		m_md = New Mandant
		m_utility = New Utilities
		m_UtilityUi = New UtilityUI

	End Sub

#End Region


	Sub DeleteAllRecinDb()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = String.Format("BEGIN TRY DROP TABLE [##_LOLFibu_{0}] ", m_InitialData.UserData.UserGuid)
		sSql &= "END TRY BEGIN CATCH END CATCH "

		sSql &= String.Format("BEGIN TRY Drop Table [##_LOLBetrag_{0}] ", m_InitialData.UserData.UserGuid)
		sSql &= "END TRY BEGIN CATCH END CATCH "

		Try
			If IsNothing(ClsDataDetail.Conn) Then Exit Sub
			If ClsDataDetail.Conn.State = ConnectionState.Closed Then Exit Sub

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.CommandType = Data.CommandType.Text
			cmd.ExecuteNonQuery()


		Catch e As SqlException
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))

		Finally

		End Try

	End Sub

#Region "Funktionen zur Suche nach Daten..."

	Sub CalculateWithLA(ByVal frmTest As frmFibuSearch,
													 ByVal ifMonth As Short,
													 ByVal strYear As String,
													 ByVal strFiliale As String,
													 ByVal bWithUmsJDb As Boolean)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = GetSQLString4LOLBetrag(frmTest, ifMonth, strYear, strFiliale, bWithUmsJDb)
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		Dim _ClsReg As New SPProgUtility.ClsDivReg

		Console.WriteLine(String.Format("Datenbankverbindung: {0}", m_InitialData.MDData.MDDbConn))

		Try
			With frmTest
				Dim cmd As SqlCommand = New SqlCommand(sSql, ClsDataDetail.Conn)
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MDNr", m_InitialData.MDData.MDNr)
				param = cmd.Parameters.AddWithValue("@Month", ifMonth)
				param = cmd.Parameters.AddWithValue("@Year", strYear)
				cmd.ExecuteNonQuery()

			End With

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			m_UtilityUi.ShowErrorDialog(ex.StackTrace & vbNewLine & ex.Message)

		End Try

	End Sub

	Function GetStartSQLString(ByVal frmTest As frmFibuSearch,
														 ByVal ifMonth As Short,
														 ByVal strYear As String,
														 ByVal strFiliale As String,
														 ByVal bWithUmsJDb As Boolean) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim sSql As String = GetSQLString4LOL(frmTest, ifMonth, strYear, strFiliale, bWithUmsJDb)
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		Dim _ClsReg As New SPProgUtility.ClsDivReg
		Dim dFilialProz As Double

		ClsDataDetail.Conn = New SqlConnection(EnablingMarsintoConnString(m_InitialData.MDData.MDDbConn))
		ClsDataDetail.Conn.Open()
		Me.DeleteAllRecinDb()
		m_Logger.LogDebug(String.Format("m_InitialData.MDName: {0} | m_InitialData.MDNr: {1} | m_InitialData.MDDbConn: {2}", m_InitialData.MDData.MDName, m_InitialData.MDData.MDNr, m_InitialData.MDData.MDDbConn))

		CalculateWithLA(frmTest, ifMonth, strYear, strFiliale, bWithUmsJDb)

		With frmTest
			Try
				Dim cmd As SqlCommand = New SqlCommand(sSql, ClsDataDetail.Conn)
				cmd.ExecuteNonQuery()

				If .ChkUJListe.Checked Then
					For i = 1 To .Cbo_Filiale.Properties.Items.Count - 1
						dFilialProz = GetProzForFiliale(.Cbo_Filiale.Properties.Items(i).ToString.Trim, ifMonth, strYear)
						Trace.WriteLine("Prozenz (" & .Cbo_Filiale.Properties.Items(i).ToString & "): " & dFilialProz)
						UpdateFibuRecInDb(.Cbo_Filiale.Properties.Items(i).ToString, dFilialProz)
					Next

				ElseIf strFiliale.Trim <> String.Empty Then
				Else
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

			End Try

		End With

		Return sSql
	End Function

	Sub UpdateFibuRecInDb(ByVal strFieldName As String, ByVal dFilialProz As Double)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strLOLFibuTable As String = String.Format("[##_LOLFibu_{0}]", m_InitialData.UserData.UserGuid)
		Dim strLOLBetragTable As String = String.Format("[##_LOLBetrag_{0}]", m_InitialData.UserData.UserGuid)

		Dim sSql As String = "Update @LOLFibu Set [" & CStr(IIf(strFieldName = String.Empty, "Nicht definiert", strFieldName)) & "] = "
		sSql &= "TotalBetrag * @FilialProz"
		sSql = sSql.Replace("@LOLFibu", strLOLFibuTable)

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@FilialProz", dFilialProz / 100)

			cmd.ExecuteNonQuery()

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.Message, MsgBoxStyle.Critical, "UpdateFibuRecInDb_0")

		Finally

		End Try

	End Sub

	Function GetSQLString4LOL(ByVal frmTest As frmFibuSearch,
														 ByVal ifMonth As Integer,
														 ByVal strYear As String,
														 ByVal strFiliale As String,
														 ByVal bWithUmsJDb As Boolean) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		Dim _ClsReg As New SPProgUtility.ClsDivReg
		Dim strLOLFibuTable As String = String.Format("[##_LOLFibu_{0}]", m_InitialData.UserData.UserGuid)
		Dim strLOLBetragTable As String = String.Format("[##_LOLBetrag_{0}]", m_InitialData.UserData.UserGuid)

		With frmTest

			sSql = "Select @LOLBetrag.Lanr, Sum(@LOLBetrag.m_btr) As TotalBetrag, " & strYear & " As Jahr "
			If .ChkUJListe.Checked Then
				For i = 1 To .Cbo_Filiale.Properties.Items.Count - 1
					If .Cbo_Filiale.Properties.Items(i).ToString <> String.Empty Then
						sSql &= ", Sum(@LOLBetrag.m_btr) As [" & .Cbo_Filiale.Properties.Items(i).ToString.Trim & "] "
					Else
						sSql &= ", Sum(@LOLBetrag.m_btr) As [nicht definiert] "

					End If
				Next
			End If

			sSql &= "Into @LOLFibu From @LOLBetrag "
			sSql &= "Left Join LA On @LOLBetrag.LANr = LA.LANr And @LOLBetrag.Jahr = LA.LAJahr "

			sSql &= "Where LA.NoListing = 0 "
			sSql &= "And LA.SKonto + LA.HKonto <> 0 "
			If .ChkNot_0.Checked Then sSql &= "And @LOLBetrag.m_btr <> 0 "
			sSql &= "Group By @LOLBetrag.LANr Order By @LOLBetrag.LANr ASC"

		End With
		sSql = sSql.Replace("@LOLBetrag", strLOLBetragTable).Replace("@LOLFibu", strLOLFibuTable)


		Return sSql
	End Function

	Function GetSQLString4LOLBetrag(ByVal frmTest As frmFibuSearch,
													 ByVal ifMonth As Integer,
													 ByVal strYear As String,
													 ByVal strFiliale As String,
													 ByVal bWithUmsJDb As Boolean) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		Dim _ClsReg As New SPProgUtility.ClsDivReg
		Dim strLOLFibuTable As String = String.Format("[##_LOLFibu_{0}]", m_InitialData.UserData.UserGuid)
		Dim strLOLBetragTable As String = String.Format("[##_LOLBetrag_{0}]", m_InitialData.UserData.UserGuid)

		With frmTest

			sSql = "BEGIN TRY DROP TABLE @LOLBetrag "
			sSql &= "END TRY BEGIN CATCH END CATCH "

			sSql &= "Select LOL.Lanr, "

			sSql &= "LOL.m_btr, Jahr "
			sSql &= "Into @LOLBetrag From LOL "
			sSql &= "Left Join LA On LOL.LANr = LA.LANr And LOL.Jahr = LA.LAJahr "

			sSql &= "Where LOL.MDNr = @MDNr And LA.NoListing = 0 And "
			sSql &= "LOL.LP = @Month And LOL.Jahr = @Year "

			sSql &= "And LA.SKonto + LA.HKonto <> 0 "
			If .ChkNot_0.Checked Then sSql &= "And LOL.m_btr <> 0 "
			sSql &= "And RPtext Is Not Null "
			sSql &= "Order By LOL.LANr ASC"


			sSql = sSql.Replace("@LOLBetrag", strLOLBetragTable).Replace("@LOLFibu", strLOLFibuTable)
		End With

		Return sSql
	End Function

	Function GetBeraterForFiliale(ByVal strFiliale As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty

		Try
			Dim sSql As String = "Select Benutzer.KST, Benutzer.USFiliale From Benutzer " ' Left Join US_Filiale "
			sSql &= "Where Benutzer.USFiliale = @Filiale "
			sSql &= "And Benutzer.Kst <> '' "
			sSql &= "Order By Benutzer.Kst ASC"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ClsDataDetail.Conn)
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@Filiale", strFiliale)

			Dim rUSrec As SqlDataReader = cmd.ExecuteReader

			While rUSrec.Read
				strResult &= CStr(IIf(strResult = String.Empty, "", ", ")) & rUSrec("KST").ToString
			End While


		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.Message)

		Finally

		End Try

		Return strResult
	End Function

	Function GetProzForFiliale(ByVal strFiliale As String,
														 ByVal sMonth As Short,
														 ByVal strYear As String) As Double
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim dResult As Double = 0

		Try
			Dim sSql As String = "[Get New FilialProzSatz From UJournal_" & m_InitialData.UserData.UserGuid & "]"

			Dim cmd As SqlCommand = New SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@LP", sMonth)
			param = cmd.Parameters.AddWithValue("@MDYear", strYear)
			param = cmd.Parameters.AddWithValue("@FilialBez", strFiliale)

			Dim rUJrec As SqlDataReader = cmd.ExecuteReader

			While rUJrec.Read
				dResult = CDbl(rUJrec("FilialProz").ToString)
			End While


		Catch e As SqlException
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			Trace.WriteLine(e.Message, "GetProzForFiliale_1")

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			Trace.WriteLine(e.Message, "GetProzForFiliale_0")

		Finally

		End Try

		Return dResult
	End Function

	Sub GetJobNr4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strModul2print As String = String.Empty

		strModul2print = "11.8"

		ClsDataDetail.GetModulToPrint = strModul2print

	End Sub


#End Region

#Region "Final-Query..."

	Function GetFinalQueryForOutput(ByVal frmSource As frmFibuSearch) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Format("Select * From [LOLFibu_{0}] ", m_InitialData.UserData.UserGuid)
		strResult &= "Order By LANr"
		InsertDataToFinalDb(frmSource)

		Return strResult
	End Function

	Function GetOutputQueryForFilial(ByVal strFieldName As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strTableName As String = String.Format("[LOLFibu_{0}]", m_InitialData.UserData.UserGuid)
		Dim strResult As String = "Select @TableName.LANr, @TableName.Bezeichnung, @TableName.Jahr, "
		If strFieldName <> String.Empty Then
			'      strResult &= "@TableName.TotalBetrag, @TableName.[@FieldName] As FilialName, "
			strResult &= "@TableName.TotalBetrag As AllBetrag, @TableName.[@FieldName] As TotalBetrag, "
		Else
			strResult &= "@TableName.TotalBetrag As TotalBetrag, "
		End If
		strResult &= "LA.HKonto, LA.SKonto, LA.Vorzeichen, LA.Vorzeichen_2, LA.Vorzeichen_3 From @TableName "
		strResult &= "Left Join LA On @TableName.LANr = LA.LANr And @TableName.Jahr = LA.LAJahr "
		strResult &= "Order By @TableName.LANr"

		strResult = strResult.Replace("@TableName", strTableName).Replace("@FieldName", strFieldName)
		strResult = strResult.Replace("Alle", "TotalBetrag")

		Return strResult
	End Function

	Function InsertDataToFinalDb(ByVal frmSource As frmFibuSearch) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim sSql As String = String.Empty
		Dim sSqlLen As Integer = 0
		Dim sZusatzBez As String = String.Empty
		Dim i As Integer = 0
		Dim _ClsReg As New SPProgUtility.ClsDivReg
		Dim strFilterBez As String = String.Empty


		Try
			sSql = String.Format("BEGIN TRY DROP TABLE [LOLFibu_{0}] ", m_InitialData.UserData.UserGuid)
			sSql &= "END TRY BEGIN CATCH END CATCH "

			Dim cmd As SqlClient.SqlCommand = New SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.ExecuteNonQuery()

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try

		Try
			sSql = String.Format("Select lFibu.*, LA.HKonto, LA.SKonto, LA.LALOText As Bezeichnung, ", m_InitialData.UserData.UserGuid)
			sSql &= String.Format("LA.Vorzeichen_2, LA.Vorzeichen_3, LA.Reserve2 ", m_InitialData.UserData.UserGuid)
			sSql &= String.Format("Into [LOLFibu_{0}] ", m_InitialData.UserData.UserGuid)
			sSql &= String.Format("From [##_LOLFibu_{0}] LFibu ", m_InitialData.UserData.UserGuid)
			sSql &= "Left Join LA On lFibu.LANr = LA.LANr And lFibu.Jahr = LA.LAJahr"
			Dim cmd As SqlClient.SqlCommand = New SqlCommand(sSql, ClsDataDetail.Conn)
			cmd.ExecuteNonQuery()

			With frmSource
				strFilterBez &= "Monat/Jahr " & .Cbo_Month.Text & " / " & .Cbo_Year.Text
				If .ChkUJListe.Checked Then strFilterBez &= vbCrLf & "Mit Teilung gemäss Umsatzliste"

			End With
			ClsDataDetail.GetFilterBez = strFilterBez


		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			MsgBox(ex.StackTrace & vbNewLine & ex.Message, MsgBoxStyle.Critical, "InsertDataToFinalDb")

		End Try

		Return sSql
	End Function

#End Region

End Class



