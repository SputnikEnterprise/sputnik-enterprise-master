
Imports System
Imports System.Reflection
Imports System.Collections

Imports System.Reflection.Assembly

Imports System.Data.SqlClient

Imports SP.Infrastructure.Logging
Imports SPS.Listing.Print.Utility.ClsLLLOSearchPrintSetting
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports System.IO
Imports System.Net.Mime.MediaTypeNames

Imports SP.Infrastructure.Utility
Imports SPS.Listing.Print.Utility.ClsMainSetting

Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath



Namespace MainUtilities

  Module Utilities

		''' <summary>
		''' The logger.
		''' </summary>
		Private m_Logger As ILogger = New Logger()

    Private m_path As New ClsProgPath
    Private m_md As New Mandant
		Private m_Conn As String


#Region "Helpers"




    Function TranslateMyText(ByVal strBez As String) As String
      Dim strOrgText As String = strBez

      Dim strTranslatedText As String = GetSafeTranslationValue(strBez)

      Return strTranslatedText
    End Function

    Function GetSafeTranslationValue(ByVal dicKey As String) As String
      Dim strPersonalizedItem As String = dicKey

			Try
				If ClsMainSetting.TranslationData Is Nothing Then Return strPersonalizedItem

				If ClsMainSetting.TranslationData.ContainsKey(strPersonalizedItem) Then
					Return ClsMainSetting.TranslationData.Item(strPersonalizedItem).LogedUserLanguage

				Else
					Return strPersonalizedItem

				End If

			Catch ex As Exception
				Return strPersonalizedItem
			End Try

    End Function

    Function GetSafeTranslationValue(ByVal dicKey As String, ByVal strDestLanguage As String) As String
      Dim strPersonalizedItem As String = dicKey

      Try
        If ClsMainSetting.TranslationData.ContainsKey(strPersonalizedItem) Then

          If strDestLanguage = "I" Then
            Return ClsMainSetting.TranslationData.Item(strPersonalizedItem).Translation_IT
          ElseIf strDestLanguage = "F" Then
            Return ClsMainSetting.TranslationData.Item(strPersonalizedItem).Translation_FR
          Else
            Return ClsMainSetting.TranslationData.Item(strPersonalizedItem).LogedUserLanguage
          End If

        Else
          Return strPersonalizedItem

        End If

      Catch ex As Exception
        Return strPersonalizedItem
      End Try

    End Function

    Function GetSafeTranslationValue(ByVal dicKey As String, ByVal bCheckPersonalizedItem As Boolean) As String
      Dim strPersonalizedItem As String = dicKey

      Try
        If bCheckPersonalizedItem Then
          If ClsMainSetting.PerosonalizedData.ContainsKey(dicKey) Then
            strPersonalizedItem = ClsMainSetting.PerosonalizedData.Item(dicKey).CaptionValue

          Else
            strPersonalizedItem = strPersonalizedItem

          End If
        End If

        If ClsMainSetting.TranslationData.ContainsKey(strPersonalizedItem) Then
          Return ClsMainSetting.TranslationData.Item(strPersonalizedItem).LogedUserLanguage

        Else
          Return strPersonalizedItem

        End If

      Catch ex As Exception
        Return strPersonalizedItem
      End Try

    End Function

		''' <summary>
		''' Opens a SqlClient.SqlDataReader object. 
		''' </summary>
		''' <param name="sql">The sql string.</param>
		''' <param name="parameters">The parameters collection.</param>
		''' <returns>The open reader or nothing in error case.</returns>
		''' <remarks>The reader is opened with the CloseConnection option, so when the reader is closed the underlying database connection will also be closed.</remarks>
		Public Function OpenReader(ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter), Optional ByVal commandType As System.Data.CommandType = CommandType.Text) As SqlClient.SqlDataReader

			Dim Conn As SqlClient.SqlConnection = Nothing

			' Open a new connection
			Conn = New SqlClient.SqlConnection(m_Conn)
			
			Dim reader As SqlClient.SqlDataReader

			Try

				' Only open connection if its not an explicit connection
				Conn.Open()
				
				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
				cmd.CommandType = commandType


				If Not parameters Is Nothing Then
					For Each param In parameters
						cmd.Parameters.Add(param)

					Next
				End If

				reader = cmd.ExecuteReader()
				

			Catch e As Exception
				m_Logger.LogError(String.Format("SQL={0}. Exception={1}", sql, e.ToString()))

				Conn.Close()
				Conn.Dispose()

				reader = Nothing
			End Try

			Return reader

		End Function

		Public Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
			Dim result As Boolean
			If (Not Boolean.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function

		Public Function ParseToInteger(ByVal stringvalue As String, ByVal value As Integer?) As Integer
			Dim result As Integer
			If (Not Integer.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function

		Public Function ParseToDec(ByVal stringvalue As String, ByVal value As Decimal?) As Decimal
			Dim result As Decimal
			If (Not Decimal.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function

		Public Function StrToBool(ByVal str As String) As Boolean

			Dim result As Boolean = False

			If String.IsNullOrWhiteSpace(str) Then
				Return False
			ElseIf str = "1" Then
				Return True
			End If

			Boolean.TryParse(str, result)

			Return result
		End Function

		' Helps extracting a column value form a data reader.
		Function GetColumnTextStr(ByVal dr As SqlClient.SqlDataReader, _
																						ByVal columnName As String, ByVal replacementOnNull As String) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			If Not dr.IsDBNull(dr.GetOrdinal(columnName)) Then
				If String.IsNullOrEmpty(CStr(dr(columnName))) Then
					Return replacementOnNull
				End If
				Return CStr(dr(columnName))
			End If

			Return replacementOnNull
		End Function

		''' <summary>
		''' Returns a string or the default value if its nothing.
		''' </summary>
		''' <param name="reader">The reader.</param>
		''' <param name="columnName">The column name.</param>
		''' <param name="defaultValue">The default value.</param>
		''' <returns>Value or default value if the value is nothing</returns>
		Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				Return reader.GetString(columnIndex)
			Else
				Return defaultValue
			End If
		End Function


		''' <summary>
		''' Replaces a missing object with another object.
		''' </summary>
		''' <param name="obj">The object.</param>
		''' <param name="replacementObject">The replacement object.</param>
		''' <returns>The object or the replacement object it the object is nothing.</returns>
		Public Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

			If (obj Is Nothing) Then
				Return replacementObject
			Else
				Return obj
			End If

		End Function


#End Region


		Function BuildPrintJob(ByVal iMDNr As Integer, ByVal strConnString As String, ByVal JobNr As String, ByVal _ClsLLFunc As ClsLLFunc) As Boolean
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Dim sSql As String = "Select * From DokPrint Where [JobNr] = @JobNr"
			Dim Conn As SqlConnection = New SqlConnection(strConnString)
			Dim bResult As Boolean = True

			If JobNr = String.Empty Then
				Dim strMessage As String = "Sie haben keine Vorlage ausgewählt.{0}Bitte wählen Sie aus der Liste eine Vorlage aus."
				MsgBox(String.Format(TranslateMyText(strMessage), vbNewLine), _
							MsgBoxStyle.Critical, TranslateMyText("Leere Vorlage"))
				Return False
			End If
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			Dim param As System.Data.SqlClient.SqlParameter

			Try
				Conn.Open()

				param = cmd.Parameters.AddWithValue("@JobNr", JobNr)
				Dim rDocrec As SqlDataReader = cmd.ExecuteReader					' Dokumentendatenbank
				rDocrec.Read()
				If Not String.IsNullOrEmpty(rDocrec("DocName").ToString) Then
					_ClsLLFunc.LLDocName = m_md.GetSelectedMDDocPath(iMDNr) & rDocrec("DocName").ToString
					_ClsLLFunc.LLDocLabel = rDocrec("Bezeichnung").ToString

					If String.IsNullOrEmpty(rDocrec("ParamCheck").ToString) Then
						_ClsLLFunc.LLParamCheck = 0
					Else
						_ClsLLFunc.LLParamCheck = CInt(IIf(CBool(rDocrec("ParamCheck")), 1, 0))
					End If

					If String.IsNullOrEmpty(rDocrec("KonvertName").ToString) Then
						_ClsLLFunc.LLKonvertName = 0
					Else
						_ClsLLFunc.LLKonvertName = CInt(IIf(CBool(rDocrec("KonvertName")), 1, 0))
					End If

					If String.IsNullOrEmpty(rDocrec("ZoomProz").ToString) Then
						_ClsLLFunc.LLParamCheck = 100
					Else
						_ClsLLFunc.LLZoomProz = CInt(IIf(CInt(rDocrec("ZoomProz")) = 0, 150, CInt(rDocrec("ZoomProz"))))
					End If

					If String.IsNullOrEmpty(rDocrec("Anzahlkopien").ToString) Then
						_ClsLLFunc.LLCopyCount = 1
					Else
						_ClsLLFunc.LLCopyCount = CInt(IIf(CInt(rDocrec("Anzahlkopien")) = 0, 1, CInt(rDocrec("Anzahlkopien"))))
					End If

					If String.IsNullOrEmpty((rDocrec("TempDocPath").ToString)) Then
						_ClsLLFunc.LLExportedFilePath = m_path.GetSpS2DeleteHomeFolder
					Else
						_ClsLLFunc.LLExportedFilePath = rDocrec("TempDocPath").ToString
						If Not _ClsLLFunc.LLExportedFilePath.EndsWith("\") Then _ClsLLFunc.LLExportedFilePath = _ClsLLFunc.LLExportedFilePath & "\"
					End If

					If String.IsNullOrEmpty(rDocrec("ExportedFileName").ToString) Then
						_ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("DocName").ToString)
					Else
						_ClsLLFunc.LLExportedFileName = Path.GetFileNameWithoutExtension(rDocrec("ExportedFileName").ToString)
					End If

					If String.IsNullOrEmpty(rDocrec("DokNameToShow").ToString) Or String.IsNullOrEmpty(rDocrec("DokNameToShow").ToString) Then
						_ClsLLFunc.ListBez = rDocrec("Bezeichnung").ToString
					Else
						_ClsLLFunc.ListBez = rDocrec("DokNameToShow").ToString
					End If

					If IsDBNull(rDocrec("PrintInDiffColor")) Or (rDocrec("PrintInDiffColor").ToString = String.Empty) Then
						_ClsLLFunc.LLPrintInDiffColor = False
					Else
						_ClsLLFunc.LLPrintInDiffColor = CBool(rDocrec("PrintInDiffColor"))
					End If

				Else
					_ClsLLFunc.LLDocName = String.Empty
					_ClsLLFunc.LLDocLabel = String.Empty
					_ClsLLFunc.LLParamCheck = 0
					_ClsLLFunc.LLKonvertName = 0

					_ClsLLFunc.LLParamCheck = 100
					_ClsLLFunc.LLCopyCount = 1
					_ClsLLFunc.LLExportedFilePath = m_path.GetPrinterHomeFolder
					_ClsLLFunc.LLExportedFileName = String.Empty
					bResult = False

				End If
				rDocrec.Close()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

				bResult = False

			Finally
				cmd.Dispose()
				Conn.Close()

			End Try

			Return bResult
		End Function

		Function OpenDb4PrintListing(ByVal strConString As String, ByVal strSQL2Open As String) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = strSQL2Open
			Dim Conn As SqlConnection = New SqlConnection(strConString)
			Dim rFoundedrec As SqlDataReader = Nothing

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.Text

				rFoundedrec = cmd.ExecuteReader

				rFoundedrec.Read()
				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(TranslateMyText(strMessage), _
								 MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))
					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

		Function OpenDb4PrintMonthlyTaxListing(ByVal strConString As String, ByVal MDNr As Integer, ByVal USNr As Integer) As SqlDataReader
			Dim strQuery As String = "[Load Montly Tax Data For Search In TAX Listing]"
			Dim Conn As SqlConnection = New SqlConnection(strConString)
			Dim rFoundedrec As SqlDataReader = Nothing

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("MDNr", MDNr)
				param = cmd.Parameters.AddWithValue("USNr", USNr)

				rFoundedrec = cmd.ExecuteReader

				rFoundedrec.Read()
				If (rFoundedrec Is Nothing) OrElse Not rFoundedrec.HasRows Then
					Dim strMessage As String = "Quellensteuer-Liste: {1}{0}Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.{0}MDNr: {1} >>> USNr: {2}"
					MsgBox(String.Format(TranslateMyText(strMessage), vbNewLine, MDNr, USNr), MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))

					m_Logger.LogDebug(String.Format("{0}.QST-Listing: {1}", strQuery, strMessage))
					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

			Return rFoundedrec
		End Function

		Function OpenDb4PrintESTemplate(ByVal strConString As String, ByVal iESNr As Integer) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = "[Get ESData For Print ESVertrag]" ' "[Get ESData For ArbVertrag]"
			Dim Conn As SqlConnection = New SqlConnection(strConString)
			Dim rFoundedrec As SqlDataReader = Nothing

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
				param = cmd.Parameters.AddWithValue("@ESLNr", 0)

				rFoundedrec = cmd.ExecuteReader

				rFoundedrec.Read()
				If (rFoundedrec Is Nothing) OrElse Not rFoundedrec.HasRows Then
					Dim strMessage As String = "Einsatznummer: {1}{0}Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben."
					m_Logger.LogDebug(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(String.Format(TranslateMyText(strMessage), vbNewLine, iESNr),
								 MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))

					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.ToString))

			End Try

			Return rFoundedrec
		End Function

		Function OpenDb4ESGAVStatistiken(ByVal setting As ClsLLESSearchPrintSetting) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim sql As String = setting.SQL2Open
			Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
			Dim rFoundedrec As SqlDataReader = Nothing
			Dim userLanguage = m_InitialData.UserData.UserLanguage

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand

				sql = "DECLARE @t TABLE"
				sql &= " ("
				sql &= " id INT ,"
				sql &= " usnr INT ,"
				sql &= " kstbez NVARCHAR(255) ,"
				sql &= " kst1count MONEY ,"
				sql &= " kst2count MONEY ,"
				sql &= " usnachname NVARCHAR(255) ,"
				sql &= " usvorname NVARCHAR(255) ,"
				sql &= " KSTFiliale NVARCHAR(255) ,"
				sql &= " gavinfo_string VARCHAR(50)"
				sql &= " );"

				sql &= String.Format(" INSERT  INTO @t EXEC dbo.[Get KST Statistik For ESSearch] {0}, '{1}', 'a';", m_InitialData.UserData.UserNr, setting.DbTblName)

				sql &= "DECLARE @tbl TABLE"
				sql &= " ("
				sql &= " Filiale NVARCHAR(50) ,"
				sql &= " GAVNumber INT ,"
				sql &= " AnzahlGAV DECIMAL(8, 2) ,"
				sql &= " AnzahlES DECIMAL(8, 2) ,"
				sql &= " Anteilprozent DECIMAL(8, 2)"
				sql &= " );"

				sql &= " DECLARE @FilialName NVARCHAR(255) = '';"
				sql &= " DECLARE @FilialCount MONEY;"
				sql &= " DECLARE @Exists INT = 1;"

				sql &= " DECLARE abc CURSOR"
				sql &= " FOR"
				sql &= " Select KSTFiliale"
				sql &= " FROM    @t"
				sql &= " GROUP BY KSTFiliale"
				sql &= " ORDER BY KSTFiliale;"
				sql &= " OPEN abc;"
				sql &= " FETCH NEXT FROM abc  INTO @FilialName;"
				sql &= " WHILE @@FETCH_STATUS = 0"
				sql &= " BEGIN"
				sql &= " PRINT @FilialName;"

				sql &= " BEGIN TRY"
				sql &= " DROP TABLE #t;"
				sql &= " End Try"
				sql &= " BEGIN CATCH"
				sql &= " END CATCH; "
				sql &= " DECLARE @anzES MONEY = ISNULL(( SELECT    SUM(ISNULL(kst1count, 0)"
				sql &= " + ISNULL(kst2count, 0))"
				sql &= " FROM      @t"
				sql &= " WHERE     KSTFiliale = @FilialName"
				sql &= " ), 1);"

				sql &= " SELECT  @FilialName Filiale ,"
				sql &= " COUNT(L.GAVNr) AnzahlGAV ,"
				sql &= " @anzES AnzahlES ,"
				sql &= " L.GAVNr"
				sql &= " INTO    #T"
				sql &= String.Format(" FROM    _EinsatzListe_{0} e", m_InitialData.UserData.UserNr)
				sql &= " LEFT JOIN ESLohn L ON e.ESNr = L.ESNr"
				sql &= " WHERE L.AktivLODaten = 1"
				sql &= " AND ( e.Filiale1 = @FilialName"
				sql &= " OR e.Filiale2 = @FilialName"
				sql &= " )"
				sql &= " GROUP BY L.GAVNr;"

				sql &= " DECLARE @anzESGAV MONEY = ISNULL(( SELECT    SUM(ISNULL(AnzahlGAV, 0))"
				sql &= " FROM      #T"
				sql &= " WHERE     Filiale = @FilialName"
				sql &= " ), 1);"

				sql &= " INSERT  INTO @tbl"
				sql &= " SELECT  #T.Filiale ,"
				sql &= " #T.GAVNr ,"
				sql &= " #T.AnzahlGAV ,"
				sql &= " @anzES AnzahlES ,"
				sql &= " CAST(#T.AnzahlGAV AS DECIMAL(8, 2)) * 100 / @anzESGAV"
				sql &= " FROM    #T;"

				sql &= " FETCH NEXT FROM abc INTO @FilialName;"
				sql &= " END;"

				sql &= " CLOSE abc;"
				sql &= " DEALLOCATE abc;"


				sql &= String.Format("SET @FilialName = '{0}'", setting.BranchesNameForGAVStatistik)
				sql &= " SELECT  t.Filiale ,"
				sql &= " t.GAVNumber ,"
				sql &= " ( SELECT TOP 1"
				If userLanguage.ToUpper.StartsWith("D") Then
					sql &= " Name_DE"
				ElseIf userLanguage.ToUpper.StartsWith("F") Then
					sql &= " IsNull(Name_FR, Name_DE)"
				ElseIf userLanguage.ToUpper.StartsWith("I") Then
					sql &= " IsNull(Name_IT, Name_DE)"
				End If
				sql &= " FROM tbl_PVLMetaData"
				sql &= " WHERE GAVNumber = t.GAVNumber"
				sql &= " ) GAVGruppe0 ,"
				sql &= " t.AnzahlGAV ,"
				sql &= " (CASE"
				sql &= " WHEN t.Anteilprozent > 100 THEN 100"
				sql &= " ELSE t.Anteilprozent"
				sql &= " END"
				sql &= " ) Anteilprozent ,"
				sql &= " t.AnzahlES"
				sql &= " FROM  @tbl t "
				sql &= " WHERE (@FilialName = '' OR t.Filiale = @FilialName)"
				sql &= " Order By t.Filiale ASC, AnteilProzent Desc;"


				cmd = New System.Data.SqlClient.SqlCommand(sql, Conn)
				cmd.CommandType = CommandType.Text

				rFoundedrec = cmd.ExecuteReader

				rFoundedrec.Read()
				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(TranslateMyText(strMessage), _
								 MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))
					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

		Function OpenDb4EmployeesATStatistiken(ByVal setting As ClsLLESSearchPrintSetting) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim sql As String = String.Empty
			Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
			Dim rFoundedrec As SqlDataReader = Nothing
			Dim userLanguage = m_InitialData.UserData.UserLanguage

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand

				sql &= "begin try drop table #Einsatzliste_1 end try begin catch end catch;"
				sql &= "begin try drop table #Einsatzliste_begin end try begin catch end catch;"
				sql &= "begin try drop table #Einsatzliste_End end try begin catch end catch;"
				sql &= "begin try drop table #Einsatzliste_Null end try begin catch end catch;"
				sql &= "begin try drop table #Einsatzliste_Final end try begin catch end catch;"

				sql &= " Select *, dbo.FirstDayInMonth('{1}') _FirstDayOfMonth, dbo.LastDayInMonth('{1}') _LastDayOfMonth "
				sql &= " Into #Einsatzliste_1 From {0} Where"
				sql &= " (es_ab BETWEEN dbo.FirstDayInMonth('{1}') And dbo.LastDayInMonth('{1}'))"
				sql &= " OR (es_ende BETWEEN dbo.FirstDayInMonth('{1}') And dbo.LastDayInMonth('{1}'))"

				sql &= " Select * "
				sql &= " Into #Einsatzliste_begin From #Einsatzliste_1 Where"
				sql &= " (es_ab BETWEEN dbo.FirstDayInMonth('{1}') And dbo.LastDayInMonth('{1}'))"
				sql &= " AND ES_Ende IS NOT NULL"

				sql &= " Select * "
				sql &= " Into #Einsatzliste_End From #Einsatzliste_begin Where"
				sql &= " (ES_Ende BETWEEN dbo.FirstDayInMonth('{1}') And dbo.LastDayInMonth('{1}'))"

				sql &= " UPDATE  #Einsatzliste_begin SET ES_Ende = NULL"
				sql &= " UPDATE  #Einsatzliste_End SET ES_Ab = NULL"

				sql &= " SELECT  *"
				sql &= " INTO    #Einsatzliste_Null"
				sql &= " FROM    #Einsatzliste_1"
				sql &= " WHERE (es_ende Is NULL)"
				sql &= " ORDER BY ESID;"

				sql &= " SELECT  *"
				sql &= " INTO    #Einsatzliste_Final"
				sql &= " FROM    #Einsatzliste_begin"

				sql &= " INSERT  INTO #Einsatzliste_Final"
				sql &= " SELECT  *"
				sql &= " FROM    #Einsatzliste_End "

				sql &= " INSERT  INTO #Einsatzliste_Final"
				sql &= " SELECT  *"
				sql &= " FROM    #Einsatzliste_Null"

				sql &= " SELECT  *"
				sql &= " FROM    #Einsatzliste_Final"
				sql &= " ORDER by Nachname, Vorname "

				sql = String.Format(sql, setting.DbTblName, setting.FirstEmployDate)

				m_Logger.LogDebug(sql)

				cmd = New System.Data.SqlClient.SqlCommand(sql, Conn)
				cmd.CommandType = CommandType.Text

				rFoundedrec = cmd.ExecuteReader

				rFoundedrec.Read()
				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(TranslateMyText(strMessage), _
								 MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))
					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

		Function OpenDb4PrintLMListing(ByVal strConString As String, ByVal iLMNr As Integer?) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = "Select LM.*, "
			strQuery &= "MA.Nachname As MANachname, MA.Vorname As MAVorname, MA.Strasse As MAStrasse, MA.PLZ As MAPLZ, MA.Ort As MAOrt, MA.Land As MALand "
			strQuery &= "From LM Left Join Mitarbeiter MA On LM.MANr = MA.MANr Where LM.LMNr = @LMNr Order By LM.[Jahr Von] Desc, LM.[LP_Von] Desc"
			Dim Conn As SqlConnection = New SqlConnection(strConString)
			Dim rFoundedrec As SqlDataReader = Nothing

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.Text

				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@LMNr", iLMNr)

				rFoundedrec = cmd.ExecuteReader

				rFoundedrec.Read()
				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(TranslateMyText(strMessage), MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))

					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

		Function OpenDb4PrintBVGListing(ByVal strConString As String, ByVal sql As String) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = sql
			'"Select LM.*, MA.Nachname As MANachname, MA.Vorname As MAVorname, MA.Strasse As MAStrasse, MA.PLZ As MAPLZ, MA.Ort As MAOrt, MA.Land As MALand "
			'		strQuery &= "From LM Left Join Mitarbeiter MA On LM.MANr = MA.MANr Where LM.LMNr = @LMNr Order By LM.[Jahr Von] Desc, LM.[LP_Von] Desc"
			Dim Conn As SqlConnection = New SqlConnection(strConString)
			Dim rFoundedrec As SqlDataReader = Nothing

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.Text

				'Dim param As System.Data.SqlClient.SqlParameter
				'param = cmd.Parameters.AddWithValue("@LMNr", iLMNr)

				rFoundedrec = cmd.ExecuteReader

				rFoundedrec.Read()
				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(TranslateMyText(strMessage), MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))

					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

		Function OpenDb4PrintKDZHDListing(ByVal strConString As String, ByVal customerNumber As Integer, ByVal zhdNumbers As List(Of Integer)) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Dim strQuery As String = "[Get Selected ZHDData For Print]"
			Dim zhdNumbersBuffer As String = String.Empty

			For Each number In zhdNumbers
				zhdNumbersBuffer = zhdNumbersBuffer & IIf(zhdNumbersBuffer <> "", ", ", "") & number
			Next

			Dim Conn As SqlConnection = New SqlConnection(strConString)
			Dim rFoundedrec As SqlDataReader = Nothing

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@KDNr", customerNumber)
				param = cmd.Parameters.AddWithValue("@ZHDNrListe", zhdNumbersBuffer)

				rFoundedrec = cmd.ExecuteReader

				rFoundedrec.Read()
				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(TranslateMyText(strMessage), MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))

					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

		Function OpenVakDb4PrintDoc(ByVal strConString As String, ByVal strSQL2Open As String, _
																ByVal _Setting As ClsLLVakSearchPrintSetting, _
																ByVal strParam As String) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = strSQL2Open
			Dim Conn As SqlConnection = New SqlConnection(strConString)
			Dim rFoundedrec As SqlDataReader = Nothing
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath


			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@VakNr", strParam)

				rFoundedrec = cmd.ExecuteReader
				_Setting.ExtraVakFieldData.Clear()

				rFoundedrec.Read()
				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(TranslateMyText(strMessage), _
								 MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))
					Return Nothing

				Else

					Try
						_Setting.ExtraVakFieldData.Add(m_path.SafeGetString(rFoundedrec, "JobProzent")) ' 0
					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.JobProzent: {1}", strMethodeName, ex.tostring))

					End Try
					Try
						_Setting.ExtraVakFieldData.Add(m_path.SafeGetString(rFoundedrec, "Anstellung")) ' 1
					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.Anstellung: {1}", strMethodeName, ex.tostring))

					End Try
					Try
						_Setting.ExtraVakFieldData.Add(m_path.SafeGetString(rFoundedrec, "MAAge")) ' 2
					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.MAAge: {1}", strMethodeName, ex.tostring))

					End Try
					Try
						_Setting.ExtraVakFieldData.Add(m_path.SafeGetString(rFoundedrec, "MSprachen")) ' 3
					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.MSprachen: {1}", strMethodeName, ex.ToString))

					End Try
					'Try
					'	_Setting.ExtraVakFieldData.Add(rFoundedrec("SprachNiveauBez")) ' 4
					'Catch ex As Exception
					'	m_Logger.LogError(String.Format("{0}.SprachNiveauBez: {1}", strMethodeName, ex.tostring))

					'End Try
					Try
						_Setting.ExtraVakFieldData.Add(m_path.SafeGetString(rFoundedrec, "JobCHBerufGruppen")) ' 5
					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}.JobCHBerufGruppen: {1}", strMethodeName, ex.ToString))

					End Try
					Try
						_Setting.ExtraVakFieldData.Add(m_path.SafeGetString(rFoundedrec, "JobCHBeruferfahrung"))  ' 6
					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}JobCHBeruferfahrunge: {1}", strMethodeName, ex.ToString))

					End Try

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

		Function OpenProposeDb4PrintDoc(ByVal strConString As String, ByVal strSQL2Open As String, _
														ByVal _Setting As ClsLLProposeSearchPrintSetting, _
														ByVal strParam As String) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = strSQL2Open
			Dim Conn As SqlConnection = New SqlConnection(strConString)
			Dim rFoundedrec As SqlDataReader = Nothing

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@iProposeNr", strParam)

				rFoundedrec = cmd.ExecuteReader

				rFoundedrec.Read()
				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(MainUtilities.TranslateMyText(strMessage), _
								 MsgBoxStyle.Critical, MainUtilities.TranslateMyText("Daten suchen"))
					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function


		Sub ChangeMyFileWithText(ByVal strFilename As String, _
												 ByVal strOrgText As List(Of String), ByVal strNewText As List(Of String), _
												 ByVal str2Check As String)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim TextDateiInhalte As String = String.Empty
			Dim Time_1 As Double = System.Environment.TickCount
			Dim bChanged As Boolean = False
			'Dim Options As Text.RegularExpressions.RegexOptions
			'Options = RegexOptions.IgnoreCase Or RegexOptions.IgnorePatternWhitespace

			Try
				Using myReader As New System.IO.StreamReader(strFilename, System.Text.ASCIIEncoding.Default)
					TextDateiInhalte = myReader.ReadToEnd
				End Using
				If str2Check = "" Or Not TextDateiInhalte.Contains(str2Check) Then
					Try
						For i As Integer = 0 To strOrgText.Count - 1
							'TextDateiInhalte = Regex.Replace(TextDateiInhalte, strOrgText(i), strNewText(i), Options)
							If TextDateiInhalte.ToLower.Contains(strOrgText(i).ToLower) Then
								TextDateiInhalte = Replace(TextDateiInhalte, strOrgText(i), strNewText(i), 1, -1, CompareMethod.Text)
								bChanged = True
							End If
						Next
						If bChanged Then
							Using myWriter As New System.IO.StreamWriter(strFilename, False, System.Text.ASCIIEncoding.Default)
								myWriter.Write(TextDateiInhalte)
							End Using
						End If

					Catch ex As Exception
						m_Logger.LogError(String.Format("File2Change beschreiben.{0}: {1}", strMethodeName, ex.ToString))

					End Try
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("File2Change lesen.{0}: {1}", strMethodeName, ex.ToString))

			End Try
			Trace.WriteLine(String.Format("Änderung der File: {0} | {1} s.", strFilename, (System.Environment.TickCount - Time_1) / 1000))

		End Sub

		Sub GetUSData(ByVal strConnString As String, ByVal _ClsLLFunc As ClsLLFunc, ByVal recordMandantNumber As Integer)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath
			Dim strUSKst As String = String.Empty
			Dim strUSNachname As String = String.Empty
			Dim iUSNr As Integer = 0
			Dim Conn As New SqlConnection(strConnString)
			Conn.Open()

			Dim sSql As String = "[Get USData 4 Templates With MDNumber And USNumber]"
			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@USNr", ClsMainSetting.UserData.UserNr)
			'param = cmd.Parameters.AddWithValue("@USVorname", ClsMainSetting.UserData.UserFName)
			param = cmd.Parameters.AddWithValue("@MDNr", recordMandantNumber)

			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Try
				rTemprec.Read()
				With _ClsLLFunc
					.iSelectedUSNr = m_path.SafeGetInteger(rTemprec, "USNr", 0)

					.USAnrede = m_path.SafeGetString(rTemprec, "USAnrede")
					.USeMail = m_path.SafeGetString(rTemprec, "USeMail")
					.USNachname = m_path.SafeGetString(rTemprec, "USNachname")
					.USVorname = m_path.SafeGetString(rTemprec, "USVorname")
					.USTelefon = m_path.SafeGetString(rTemprec, "USTelefon")
					.USTelefax = m_path.SafeGetString(rTemprec, "USTelefax")
					.USNatel = m_path.SafeGetString(rTemprec, "USNatel")

					.USTitel_1 = m_path.SafeGetString(rTemprec, "USTitel_1")
					.USTitel_2 = m_path.SafeGetString(rTemprec, "USTitel_2")

					.USAbteilung = m_path.SafeGetString(rTemprec, "USAbteilung")
					.USPostfach = m_path.SafeGetString(rTemprec, "USPostfach")
					.USStrasse = m_path.SafeGetString(rTemprec, "USStrasse")
					.USPLZ = m_path.SafeGetString(rTemprec, "USPLZ")
					.USLand = m_path.SafeGetString(rTemprec, "USLand")
					.USOrt = m_path.SafeGetString(rTemprec, "USOrt")

					.Exchange_USName = m_path.SafeGetString(rTemprec, "EMail_UserName")
					.Exchange_USPW = m_path.SafeGetString(rTemprec, "EMail_UserPW")

					.USMDname = m_path.SafeGetString(rTemprec, "MDName")
					.USMDname2 = m_path.SafeGetString(rTemprec, "MDName2")
					.USMDname3 = m_path.SafeGetString(rTemprec, "MDName3")
					.USMDPostfach = m_path.SafeGetString(rTemprec, "MDPostfach")
					.USMDStrasse = m_path.SafeGetString(rTemprec, "MDStrasse")
					.USMDPlz = m_path.SafeGetString(rTemprec, "MDPLZ")
					.USMDOrt = m_path.SafeGetString(rTemprec, "MDOrt")
					.USMDLand = m_path.SafeGetString(rTemprec, "MDLand")

					.USMDTelefon = m_path.SafeGetString(rTemprec, "MDTelefon")
					.USMDDTelefon = m_path.SafeGetString(rTemprec, "MDDTelefon")
					.USMDTelefax = m_path.SafeGetString(rTemprec, "MDTelefax")
					.USMDeMail = m_path.SafeGetString(rTemprec, "MDeMail")
					.USMDHomepage = m_path.SafeGetString(rTemprec, "MDHomepage")

				End With
				rTemprec.Close()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.ToString))

			Finally
				rTemprec.Close()
				Conn.Close()

			End Try

		End Sub

		Sub GetMandantData(ByVal strConnString As String, ByVal Jahr As Integer, ByVal _ClsLLFunc As ClsLLFunc)
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

			Dim strUSKst As String = String.Empty
			Dim strUSNachname As String = String.Empty
			Dim iUSNr As Integer = 0
			Dim Conn As New SqlConnection(strConnString)
			Conn.Open()

			Dim sSql As String = "[Get MandantenData For Print]"
			Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@Year", Jahr)
			param = cmd.Parameters.AddWithValue("@MDNr", ClsMainSetting.MDData.MDNr)

			Dim rTemprec As SqlDataReader = cmd.ExecuteReader

			Try
				rTemprec.Read()
				With _ClsLLFunc
					' 24'570.-
					.bvgkoordinationlohnjahr = m_path.SafeGetDecimal(rTemprec, "BVG_Koordination_Jahr", 0)
					' 11.25
					.bvgkoordinationlohnstd = m_path.SafeGetDecimal(rTemprec, "BVG_Koordination_Std", 0)
					' 84'240.-
					.bvgmaximallohnjahr = m_path.SafeGetDecimal(rTemprec, "BVG_Max_Jahr", 0)
					' 3'510.-
					.bvgminmallohnjahr = m_path.SafeGetDecimal(rTemprec, "BVG_Min_Jahr", 0)
					' 2'184.0
					.bvgstd = m_path.SafeGetDecimal(rTemprec, "BVG_Std", 0)
					' 52
					.bvgwoche = m_path.SafeGetDecimal(rTemprec, "BVG_Woche", 0)

				End With
				rTemprec.Close()


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.tostring))

			Finally
				rTemprec.Close()
				Conn.Close()

			End Try

		End Sub


		''' <summary>
		''' Ist für Benutzerunterschrift anhand Benutzernummer
		''' </summary>
		''' <param name="strConnString"></param>
		''' <param name="lUSNr"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetUSSign(ByVal strConnString As String, ByVal lUSNr As Integer) As String
			Dim Conn As New SqlConnection(strConnString)
			Dim strFullFilename As String = Path.Combine(m_path.GetSpS2DeleteHomeFolder, String.Format("Bild_{0}_{1}.JPG", ClsMainSetting.MDData.MDNr, ClsMainSetting.UserData.UserGuid))
			Try
				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{4} >>> Die alte Unterschrift-Datei konnte nicht gelöscht werden. {0} >>> {1} | {2} | {3}", ex.ToString, m_path.GetSpS2DeleteHomeFolder, ClsMainSetting.MDData.MDNr, ClsMainSetting.UserData.UserGuid, strFullFilename))
				Return String.Empty
			End Try

			Dim strFiles As String = String.Empty
			Dim BA As Byte()
			Dim sUSSql As String = "Select USSign, USNr From Benutzer US Where "
			sUSSql &= String.Format("USNr = {0} And USSign Is Not Null", lUSNr)

			Conn.Open()
			Dim SQLCmd As SqlCommand = New SqlCommand(sUSSql, Conn)
			Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)

			Try

				Try
					Try
						BA = CType(SQLCmd_1.ExecuteScalar, Byte())
						If BA Is Nothing Then Return String.Empty

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}: {1}", strFullFilename, ex.ToString))
						Return String.Empty

					End Try

					Dim ArraySize As New Integer
					ArraySize = BA.GetUpperBound(0)

					Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
					fs.Write(BA, 0, ArraySize + 1)
					fs.Close()
					fs.Dispose()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}: {1}", strFullFilename, ex.ToString))
					Return String.Empty

				End Try

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strFullFilename, ex.ToString))
				Return String.Empty

			End Try

			Return strFullFilename
		End Function

		''' <summary>
		''' Ist für Benutzerunterschrift anhand Benutzer Vor- und Nachname
		''' </summary>
		''' <param name="strConnString"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetUSSign(ByVal strConnString As String) As String
			Dim Conn As New SqlConnection(strConnString)

			Dim strFullFilename As String = Path.Combine(m_path.GetSpS2DeleteHomeFolder, String.Format("Bild_{0}_{1}.JPG", ClsMainSetting.MDData.MDNr, ClsMainSetting.UserData.UserGuid))
			Try
				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{4} >>> Die alte Unterschrift-Datei konnte nicht gelöscht werden. {0} >>> {1} | {2} | {3}", ex.ToString, m_path.GetSpS2DeleteHomeFolder, ClsMainSetting.MDData.MDNr, ClsMainSetting.UserData.UserGuid, strFullFilename))
				Return String.Empty
			End Try

			'Dim strFullFilename As String = String.Empty
			Dim strFiles As String = String.Empty
			Dim BA As Byte()
			Dim sUSSql As String = "Select USSign, USNr, MDNr From Benutzer US Where "
			sUSSql &= String.Format("Nachname = @Nachname And Vorname = @Vorname And USSign Is Not Null")

			Dim i As Integer = 0

			Conn.Open()
			Dim SQLCmd As SqlCommand = New SqlCommand(sUSSql, Conn)
			Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)

			Dim param As System.Data.SqlClient.SqlParameter
			param = SQLCmd_1.Parameters.AddWithValue("@Nachname", ClsMainSetting.UserData.UserLName)
			param = SQLCmd_1.Parameters.AddWithValue("@Vorname", ClsMainSetting.UserData.UserFName)

			Try
				Try
					BA = CType(SQLCmd_1.ExecuteScalar, Byte())
					If BA Is Nothing Then Return String.Empty

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}: {1}", strFullFilename, ex.ToString))
					Return String.Empty
				End Try

				Dim ArraySize As New Integer
				ArraySize = BA.GetUpperBound(0)

				Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
				fs.Write(BA, 0, ArraySize + 1)
				fs.Close()
				fs.Dispose()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strFullFilename, ex.ToString))
				Return String.Empty

			End Try

			Return strFullFilename
		End Function

		''' <summary>
		''' Ist für user picture for given first and lastname
		''' </summary>
		''' <param name="strConnString"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetUSPicture(ByVal strConnString As String) As String
			Dim Conn As New SqlConnection(strConnString)

			Dim strFullFilename As String = Path.Combine(m_path.GetSpS2DeleteHomeFolder, String.Format("USPicture_{0}_{1}.JPG", ClsMainSetting.MDData.MDNr, ClsMainSetting.UserData.UserGuid))
			Try
				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{4} >>> Die alte Benutzerbild-Datei konnte nicht gelöscht werden. {0} >>> {1} | {2} | {3}", ex.ToString, m_path.GetSpS2DeleteHomeFolder, ClsMainSetting.MDData.MDNr, ClsMainSetting.UserData.UserGuid, strFullFilename))
				Return String.Empty
			End Try

			Dim strFiles As String = String.Empty
			Dim BA As Byte()
			Dim sUSSql As String = "Select USBild, USNr, MDNr From Benutzer US Where "
			sUSSql &= String.Format("Nachname = @Nachname And Vorname = @Vorname And USBild Is Not Null")

			Dim i As Integer = 0

			Conn.Open()
			Dim SQLCmd As SqlCommand = New SqlCommand(sUSSql, Conn)
			Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)

			Dim param As System.Data.SqlClient.SqlParameter
			param = SQLCmd_1.Parameters.AddWithValue("@Nachname", ClsMainSetting.UserData.UserLName)
			param = SQLCmd_1.Parameters.AddWithValue("@Vorname", ClsMainSetting.UserData.UserFName)

			Try

				Try
					Try
						BA = CType(SQLCmd_1.ExecuteScalar, Byte())
						If BA Is Nothing Then Return String.Empty

					Catch ex As Exception
						Return String.Empty

					End Try

					Dim ArraySize As New Integer
					ArraySize = BA.GetUpperBound(0)

					Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
					fs.Write(BA, 0, ArraySize + 1)
					fs.Close()
					fs.Dispose()
					'Me.USSignFileName = strFullFilename

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))

				End Try


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

			Return strFullFilename
		End Function

		''' <summary>
		''' employee picture for given employeenumber
		''' </summary>
		''' <param name="employeeNumber"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetEmployeePicture(ByVal employeeNumber As Integer) As String
			Dim Conn As New SqlConnection(ClsMainSetting.MDData.MDDbConn)

			Dim strFullFilename As String = Path.Combine(m_path.GetSpS2DeleteHomeFolder, String.Format("EmployeePicture_{0}_{1}.JPG", ClsMainSetting.MDData.MDNr, employeeNumber))
			Try
				If File.Exists(strFullFilename) Then File.Delete(strFullFilename)

			Catch ex As Exception
				m_Logger.LogError(String.Format("{4} >>> Die alte Kandidatenbild-Datei konnte nicht gelöscht werden. {0} >>> {1} | {2} | {3}", ex.ToString, m_path.GetSpS2DeleteHomeFolder, ClsMainSetting.MDData.MDNr, employeeNumber, strFullFilename))
				Return String.Empty
			End Try

			Dim strFiles As String = String.Empty
			Dim BA As Byte()
			Dim sUSSql As String = "Select MABild, MDNr From Mitarbeiter MA Where "
			sUSSql &= "MANr = @MANr And MABild Is Not Null"

			Conn.Open()
			Dim SQLCmd As SqlCommand = New SqlCommand(sUSSql, Conn)
			Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)

			Dim param As System.Data.SqlClient.SqlParameter
			param = SQLCmd_1.Parameters.AddWithValue("@MANr", employeeNumber)

			Try

				Try
					Try
						BA = CType(SQLCmd_1.ExecuteScalar, Byte())
						If BA Is Nothing Then Return String.Empty

					Catch ex As Exception
						Return String.Empty

					End Try

					Dim ArraySize As New Integer
					ArraySize = BA.GetUpperBound(0)

					Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
					fs.Write(BA, 0, ArraySize + 1)
					fs.Close()
					fs.Dispose()

				Catch ex As Exception
					m_Logger.LogError(String.Format("{0}", ex.ToString))
					strFullFilename = String.Empty

				End Try


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				strFullFilename = String.Empty

			End Try

			Return strFullFilename
		End Function

		Function SavePrintedDocToDb(ByVal iMANr As Integer, _
											 ByVal strDocArt As String, _
											 ByVal strDocBeschreibung As String, _
											 ByVal strFullFileToSave As String, _
											 ByVal strTblName As String) As String
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strValue As String = "Success"

			Dim Time_1 As Double = System.Environment.TickCount
			Dim strUSName As String = String.Format("{0} {1}", ClsMainSetting.UserData.UserFName, ClsMainSetting.UserData.UserLName)
			Dim Conn As New SqlConnection(ClsMainSetting.MDData.MDDbConn)
			'Dim strLogFileName As String = _ClsProgSetting.GetProzessLOGFile()
			Dim sSql As String = String.Empty

			sSql = String.Format("Insert Into {0} (MANr, DocName, UserName, CreatedOn, ScanDoc) Values ", strTblName)
			sSql &= "(@MANr, @DocName, @UserName, @CreatedOn, @ScanDoc)"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
			Dim param As System.Data.SqlClient.SqlParameter

			Try
				Conn.Open()
				cmd.Connection = Conn

				If strFullFileToSave <> String.Empty Then
					Dim myFile() As Byte = GetFileToByte(strFullFileToSave)
					Dim fi As New System.IO.FileInfo(strFullFileToSave)
					Dim strFileExtension As String = fi.Extension

					Try
						cmd.CommandType = CommandType.Text
						cmd.CommandText = sSql

						param = cmd.Parameters.AddWithValue("@MANr", iMANr)
						param = cmd.Parameters.AddWithValue("@DocName", String.Format("{0} {1}", strDocArt, strDocBeschreibung).Trim)
						param = cmd.Parameters.AddWithValue("@UserName", strUSName)
						param = cmd.Parameters.AddWithValue("@CreatedOn", Now.ToString("G"))

						param = cmd.Parameters.AddWithValue("@ScanDoc", myFile)

						cmd.Connection = Conn
						cmd.ExecuteNonQuery()

						cmd.Parameters.Clear()

					Catch ex As Exception
						m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))
						strValue = String.Format("***SavePrintedDocToDb_1: {0}", ex.tostring)

					End Try
				End If
				m_Logger.LogInfo(String.Format("Success: MANr: {0} / DocName: {1}", iMANr, strDocArt, strFullFileToSave))

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))
				strValue = String.Format("***SaveFileIntoDb_2: {0}", ex.tostring)

			Finally
				cmd.Dispose()
				Conn.Close()

			End Try
			Dim Time_2 As Double = System.Environment.TickCount
			Console.WriteLine("Zeit für SavePrintedDocToDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

			Return strValue
		End Function


		Function GetFileToByte(ByVal filePath As String) As Byte()
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim stream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
			Dim reader As BinaryReader = New BinaryReader(stream)
			Dim photo() As Byte = Nothing

			Try
				photo = reader.ReadBytes(CInt(stream.Length))
				reader.Close()
				stream.Close()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return photo
		End Function



		Function LoadZGADataForDTAList(ByVal _conn As String, ByVal mdNr As Integer, ByVal dtaNumbers As Integer()) As SqlDataReader

			Dim rFoundedrec As SqlDataReader = Nothing
			m_Conn = _conn

			Try

				Dim sql As String = String.Empty

				Dim dtaNumbersBuffer As String = String.Empty

				For Each number In dtaNumbers

					dtaNumbersBuffer = dtaNumbersBuffer & IIf(dtaNumbersBuffer <> "", ", ", "") & number

				Next

				sql &= "Select ZG.ZGNr, ZG.MANr, ZG.LONr, ZG.Betrag, ZG.Currency, Convert(Int, ZG.LP) As LP, Convert(Int, ZG.Jahr) As Jahr, "
				sql &= "ZG.ZGGrund, ZG.ClearingNr, ZG.KontoNr, ZG.IBANNr, ZG.Bank, ZG.BankOrt, "
				sql &= "ZG.BLZ, ZG.Swift, ZG.DTAAdr1, ZG.DTAAdr2, ZG.DTAAdr3, ZG.DTAAdr4, "
				sql &= "MA.Nachname, MA.Vorname, MA.Strasse, MA.PLZ, MA.Ort, MA.Land "
				sql &= ",(SELECT TOP 1 Bankname FROM dbo.MD_ESRDTA WHERE MDNr = @MDNr AND ModulArt = 1 ORDER BY Jahr Desc) AS MDBankname "
				sql &= ",(SELECT TOP 1 BankAdresse FROM dbo.MD_ESRDTA WHERE MDNr = @MDNr AND ModulArt = 1 ORDER BY Jahr Desc) AS MDBankAdresse "
				sql &= "From ZG "
				sql &= "Left Join Mitarbeiter MA On ZG.MANr = MA.MANr "
				sql &= "Where ZG.MDNr = @mdNr And ZG.VGNr In (" & dtaNumbersBuffer & ") "
				sql &= "Order By MA.Nachname, MA.Vorname, VGNr"


				' Parameters
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

				Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

				Return reader

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.tostring))
				Return Nothing

			End Try

		End Function

		Function LoadZGADataForLOLDTAList(ByVal _conn As String, ByVal mdNr As Integer, ByVal dtaNumbers As Integer()) As SqlDataReader

			Dim rFoundedrec As SqlDataReader = Nothing
			m_Conn = _conn

			Try

				Dim sql As String = String.Empty

				Dim dtaNumbersBuffer As String = String.Empty

				For Each number In dtaNumbers

					dtaNumbersBuffer = dtaNumbersBuffer & IIf(dtaNumbersBuffer <> "", ", ", "") & number

				Next

				sql &= "Select LOL.ID, LOL.LANr, LOL.MANr, LOL.LP, LOL.Jahr, LOL.m_Btr, LOL.m_Btr As Betrag, LOL.VGNr, "
				sql &= "LOL.DTADate, LOL.ZGGrund, LOL.Currency, LOL.RPText As ZGGrund, "
				sql &= "Mitarbeiter.Nachname, Mitarbeiter.Vorname, (Mitarbeiter.Nachname + ' ' + Mitarbeiter.Vorname) As MAName, "
				sql &= "MA_Bank.Bank, MA_Bank.BankOrt, convert(money,MA_Bank.DTABCNr) As ClearingNr, MA_Bank.KontoNr, "
				sql &= "MA_Bank.DTAADR1, MA_Bank.DTAADR2, MA_Bank.DTAADR3, MA_Bank.DTAADR4, "
				sql &= "MA_Bank.Swift, MA_Bank.BLZ, MA_Bank.IBANNr "

				'sql &= ", @ZGNr As ZGNr "

				sql &= "From LOL Left Join Mitarbeiter On LOL.MANr = Mitarbeiter.MANr "
				sql &= "Left Join MA_Bank On LOL.BnkNr = MA_Bank.RecNr And LOL.MANr = MA_Bank.MANr "

				sql &= "Where LOL.VGNr In (" & dtaNumbersBuffer & ") "
				sql &= "AND LOL.MDNr = @MDNr "
				sql &= "Order By Mitarbeiter.Nachname ASC, LOL.M_Btr ASC"

				' Parameters
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))

				Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

				Return reader

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.tostring))
				Return Nothing

			End Try

		End Function


		Function LoadZEDataForESRList(ByVal _conn As String, ByVal mdNr As Integer, ByVal firstPaymentNumber As Integer()) As SqlDataReader

			Dim rFoundedrec As SqlDataReader = Nothing
			m_Conn = _conn

			Try

				Dim sql As String = String.Empty

				Dim paymentNumbersBuffer As String = String.Empty

				For Each number In firstPaymentNumber

					paymentNumbersBuffer = paymentNumbersBuffer & IIf(paymentNumbersBuffer <> "", ", ", "") & number

				Next

				sql &= "Select ZE.*, RE.R_Name1 From ZE Left Join RE On ZE.RENr = RE.RENr Where ZE.MDNr = @MDNr And ZE.ZENr >= @paymentNumbersBuffer "
				sql &= "And ZE.Storniert = 0"
				sql &= "Order By RE.R_Name1, RE.RENR"

				' Parameters
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
				listOfParams.Add(New SqlClient.SqlParameter("paymentNumbersBuffer", paymentNumbersBuffer))

				Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

				Return reader

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.tostring))
				Return Nothing

			End Try



		End Function

		Function LoadESRDataForList(ByVal _conn As String, ByVal mdNr As Integer, ByVal DiskIdentity As String) As SqlDataReader

			Dim rFoundedrec As SqlDataReader = Nothing
			m_Conn = _conn

			Try

				Dim sql As String = String.Empty
				sql &= "Select * From ESR where rec <> '' AND MDnr = @mdnr And DiskInfo = @DiskIdentity order by ID"

				' Parameters
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("mdNr", mdNr))
				listOfParams.Add(New SqlClient.SqlParameter("DiskIdentity", DiskIdentity))

				Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

				Return reader

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.tostring))
				Return Nothing

			End Try

		End Function


		Function LoadOfferDataForTemplate(ByVal offerData As ClsLLOfferSearchPrintSetting) As SqlDataReader

			Dim rFoundedrec As SqlDataReader = Nothing
			Dim bWithoutMA As Boolean = True
			m_Conn = offerData.m_initData.MDData.MDDbConn

			Try

				Dim sql As String = String.Empty
				sql = "Select MANr From OFF_MASelection Where OfNr = @offNumber"

				' Parameters
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("offNumber", offerData.offerNumber))

				m_Logger.LogInfo(String.Format("sql: {0} >>> offerNumber: {1}", sql, offerData.offerNumber))
				Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

				If (Not reader Is Nothing AndAlso reader.Read()) Then
					sql = "[Get OfferData For Print In MailMerge]"
					bWithoutMA = False

				Else
					sql = "[Get OfferData For Print In MailMerge Without MA]"
					bWithoutMA = True

				End If
				listOfParams.Clear()
				listOfParams.Add(New SqlClient.SqlParameter("offNumber", offerData.offerNumber))
				listOfParams.Add(New SqlClient.SqlParameter("ofKDNr", offerData.customerNumber))
				listOfParams.Add(New SqlClient.SqlParameter("OfKDZNr", ReplaceMissing(offerData.cresponsibleNumber, 0)))

				m_Logger.LogInfo(String.Format("LoadOfferDataForTemplate: sqlquery: {0} >>> offerNumber: {1} | customerNumber:{2} | cresponsibleNumber: {3}",
																			 sql, offerData.offerNumber, offerData.customerNumber, offerData.cresponsibleNumber))
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				Return reader

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.tostring))
				Return Nothing

			End Try

		End Function


		Function GetEmployeeSecJob(ByVal employeeNumber As Integer, ByVal offerData As ClsLLOfferSearchPrintSetting) As String
			Dim rFoundedrec As SqlDataReader = Nothing
			Dim result As String = String.Empty
			m_Conn = offerData.m_initData.MDData.MDDbConn


			Dim sql As String = String.Empty
			sql = "Select BerufsText From MA_ES_Als Where MANr = @MANr"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", employeeNumber))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

			Try

				If (Not reader Is Nothing) Then
					While reader.Read()
						result &= String.Format("{0}#@", SafeGetString(reader, "BerufsText"))

					End While

					If result.EndsWith("#@") Then result = Mid(result, 1, Len(result) - 2)
				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))
				Return String.Empty

			End Try

			Return result

		End Function

	End Module




End Namespace





Namespace ESDbDatabases

	Public Class ClsESDb4Print

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Function TranslateText(ByVal strBez As String) As String
			Dim strOrgText As String = strBez
			Dim strTranslatedText As String = strBez

			Return strTranslatedText
		End Function

		Public Shared Function OpenDb4PrintESVertrag(ByVal _setting As ClsLLESVertragSetting) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = "[Get ESData For Print ESVertrag]"
			Dim Conn As SqlConnection = New SqlConnection(_setting.DbConnString2Open)
			Dim rFoundedrec As SqlDataReader = Nothing
			Dim sSql As String = String.Empty
			Dim _ClsReg As New SPProgUtility.ClsDivReg
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@ESNr", _setting.SelectedESNr2Print)
				param = cmd.Parameters.AddWithValue("@ESLNr", _setting.SelectedESLohnNr2Print)

				rFoundedrec = cmd.ExecuteReader
				rFoundedrec.Read()
				m_Logger.LogDebug(String.Format("ESNr: {0} | ESLNr: {1}", _setting.SelectedESNr2Print, _setting.SelectedESLohnNr2Print))

				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(MainUtilities.TranslateMyText(strMessage), MsgBoxStyle.Critical, strMethodeName)

					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

		Public Shared Function OpenMDDb4PrintESVertrag(ByVal _setting As ClsLLESVertragSetting, ByVal jahr As Integer) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strUSKst As String = String.Empty
			Dim strUSNachname As String = String.Empty
			Dim iUSNr As Integer = 0
			Dim Conn As New SqlConnection(_setting.DbConnString2Open)
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim rFoundedrec As SqlDataReader = Nothing

			Try
				Conn.Open()

				Dim sSql As String = "[Get MDData For Print ESVertrag]"
				Dim cmd As SqlCommand = New SqlCommand(sSql, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@Year", jahr)
				param = cmd.Parameters.AddWithValue("@USNr", _ClsProgSetting.GetLogedUSNr)
				rFoundedrec = cmd.ExecuteReader

				rFoundedrec.Read()

				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Mandanteninformationen sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.{0}Jahr: {1} | Benutzer: {2}")
					strMessage = String.Format(strMessage, vbNewLine, jahr, _ClsProgSetting.GetLogedUSNr)

					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(MainUtilities.TranslateMyText(strMessage), _
								 MsgBoxStyle.Critical, strMethodeName)
					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

	End Class

End Namespace




Namespace AdvancePaymentDbDatabases

	Public Class ClsAdvancedPaymentDb4Print

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
		Private m_UtilityUI As New SP.Infrastructure.UI.UtilityUI


		Function TranslateText(ByVal strBez As String) As String
			Dim strOrgText As String = strBez
			Dim strTranslatedText As String = strBez

			Return strTranslatedText
		End Function

		''' <summary>
		''' check-zahlung
		''' </summary>
		''' <param name="_setting"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function LoadDb4PrintAdvancePaymentDocument(ByVal _setting As ClsLLAdvancePaymentPrintSetting) As SqlDataReader
			Dim m_UtilityUI As New SP.Infrastructure.UI.UtilityUI
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = "[Get ZGData For Print Document]"
			Dim Conn As SqlConnection = New SqlConnection(_setting.DbConnString2Open)
			Dim rFoundedrec As SqlDataReader = Nothing
			Dim sSql As String = String.Empty
			Dim _ClsReg As New SPProgUtility.ClsDivReg
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@ZGNr", _setting.ZGNr)

				rFoundedrec = cmd.ExecuteReader
				rFoundedrec.Read()

				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = String.Format(MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.{0}{0}Möglicherweise wurde das Dokument bereits gedruckt!"), vbNewLine)
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					m_UtilityUI.ShowErrorDialog(strMessage)

					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))
				Return Nothing

			End Try

			Return rFoundedrec
		End Function

		''' <summary>
		''' Bar-Auszahlung
		''' </summary>
		''' <param name="_setting"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function LoadDb4PrintReceiptDocument(ByVal _setting As ClsLLAdvancePaymentPrintSetting) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = "[Get ZGData For Print Document]"
			Dim Conn As SqlConnection = New SqlConnection(_setting.DbConnString2Open)
			Dim rFoundedrec As SqlDataReader = Nothing
			Dim sSql As String = String.Empty
			Dim _ClsReg As New SPProgUtility.ClsDivReg
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@ZGNr", _setting.ZGNr)

				rFoundedrec = cmd.ExecuteReader
				rFoundedrec.Read()

				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(MainUtilities.TranslateMyText(strMessage), _
								 MsgBoxStyle.Critical, strMethodeName)
					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

		''' <summary>
		''' überweisung auf konto
		''' </summary>
		''' <param name="_setting"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function LoadDb4PrintRemittanceDocument(ByVal _setting As ClsLLAdvancePaymentPrintSetting) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = "[Get ZGData For Print Document]"
			Dim Conn As SqlConnection = New SqlConnection(_setting.DbConnString2Open)
			Dim rFoundedrec As SqlDataReader = Nothing
			Dim sSql As String = String.Empty
			Dim _ClsReg As New SPProgUtility.ClsDivReg
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@ZGNr", _setting.ZGNr)

				rFoundedrec = cmd.ExecuteReader
				rFoundedrec.Read()

				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(MainUtilities.TranslateMyText(strMessage), _
								 MsgBoxStyle.Critical, strMethodeName)
					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

		Public Shared Function UpdateAdvancePaymentPrintDate(ByVal _setting As ClsLLAdvancePaymentPrintSetting) As Boolean
			Dim sql As String = "Update ZG Set Printed_Dat = GetDate() Where ZGNr = @zgnr"
			Dim Conn As SqlConnection = New SqlConnection(_setting.DbConnString2Open)

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(sql, Conn)
				cmd.CommandType = CommandType.Text
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@zgnr", _setting.ZGNr)

				cmd.ExecuteNonQuery()

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

			End Try

			Return True
		End Function


	End Class

End Namespace



Namespace LODbDatabases

	Public Class ClsLODb4Print

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Public Shared Function OpenDb4LOPrint(ByVal _setting As ClsLLLOSearchPrintSetting, ByVal sortkwlanr As Boolean) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = "[Get LOLData For Print LO Into LOPrintDb]"
			Dim Conn As SqlConnection = New SqlConnection(_setting.DbConnString2Open)
			Dim rFoundedrec As SqlDataReader = Nothing
			Dim sSql As String = String.Empty
			Dim _ClsReg As New SPProgUtility.ClsDivReg
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim sSortKWLANr As Short = If(sortkwlanr, 1, 0)


			'If Val(_ClsReg.GetINIString(_ClsProgSetting.GetMDIniFile, "Lohnbuchhaltung", "SortKW+LANr", "0")) <> 0 Then
			'	sSortKWLANr = 1
			'End If

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@LONr", _setting.SelectedLONr2Print)
				param = cmd.Parameters.AddWithValue("@MANr", _setting.SelectedMANr2Print)
				param = cmd.Parameters.AddWithValue("@USNr", _setting.LogedUSNr)
				param = cmd.Parameters.AddWithValue("@SortKWLANr", sSortKWLANr)

				rFoundedrec = cmd.ExecuteReader
				rFoundedrec.Read()

				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(MainUtilities.TranslateMyText(strMessage), _
								 MsgBoxStyle.Critical, MainUtilities.TranslateMyText("Daten suchen"))
					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: MANr: {1} | LONr: {2} | SortKWLANr: {3} | USNr: {4} | {5}", strMethodeName, _setting.SelectedMANr2Print, _setting.SelectedLONr2Print, sSortKWLANr, _setting.LogedUSNr, ex.tostring))

			End Try

			Return rFoundedrec
		End Function

	End Class

End Namespace


Namespace MADbDatabases

	Public Class ClsMAStammDb4Print

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		Private m_path As New ClsProgPath
		Private m_md As New Mandant

		Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Public Sub New()

		End Sub

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)
			m_InitializationData = _setting
		End Sub

		Public Shared Function TranslateMyText(ByVal strBez As String) As String
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim strOrgText As String = strBez
			Dim strTranslatedText As String = MainUtilities.TranslateMyText(strBez)
			Dim _clsLog As New SPProgUtility.ClsEventLog

			Return strTranslatedText
		End Function

		Public Shared Function OpenDb4MAStammPrint(ByVal _setting As ClsLLMASearchPrintSetting) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = "[Get MAData For Print In Stammblatt]"
			Dim Conn As SqlConnection = New SqlConnection(_setting.DbConnString2Open)
			Dim rFoundedrec As SqlDataReader = Nothing
			Dim sSql As String = String.Empty
			Dim _ClsReg As New SPProgUtility.ClsDivReg
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim sSortKWLANr As Short = 0

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@MANr", _setting.MANr2Print)

				rFoundedrec = cmd.ExecuteReader
				rFoundedrec.Read()

				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows And _setting.ShowMessageIFNotFounded Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(MainUtilities.TranslateMyText(strMessage), _
								 MsgBoxStyle.Critical, TranslateMyText("Daten suchen"))
					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function


		Public Function LoadEmployeeDataForTemplate(ByVal manr As Integer) As EmployeeDataForPrintTemplate

			Dim result As EmployeeDataForPrintTemplate = Nothing

			Dim rFoundedrec As SqlDataReader = Nothing

			Try

				Dim sql As String = String.Empty

				sql &= "SELECT MA.Nachname, MA.Vorname, MA.GebDat, MA.Strasse, Ma.PLZ, MA.Ort, MA.Land, "
				sql &= "MA.ahv_Nr_New, MA.Geschlecht, ISNULL((Select TOP 1 Land FROM LND WHERE LND.Code = MA.Nationality), MA.Nationality) AS Nationality "
				sql &= "From Mitarbeiter MA "
				sql &= "Where MANr = @MANr"

				' Parameters
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("MANr", manr))

				Dim reader As SqlClient.SqlDataReader = OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

				If reader IsNot Nothing Then

					result = New EmployeeDataForPrintTemplate

					While reader.Read

						Dim data = New EmployeeDataForPrintTemplate

						data.employeeLName = m_path.SafeGetString(reader, "Nachname")
						data.employeeFName = m_path.SafeGetString(reader, "Vorname")
						data.employeeAHVNr = m_path.SafeGetString(reader, "ahv_nr_new")

						data.gebdate = m_path.SafeGetDateTime(reader, "gebDat", Nothing)
						data.employeesex = m_path.SafeGetString(reader, "Geschlecht")

						data.chadresse = m_path.SafeGetString(reader, "Strasse")
						data.plz = m_path.SafeGetString(reader, "plz")
						data.ort = m_path.SafeGetString(reader, "ort")
						data.land = m_path.SafeGetString(reader, "land")

						data.nationality = m_path.SafeGetString(reader, "nationality")

						result = data

					End While

				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.tostring))
				Return Nothing

			End Try

			Return result

		End Function

		Public Function LoadEmployeeESDataForTemplate(ByVal mdnr As Integer, ByVal manr As Integer) As IEnumerable(Of EmployeeESDataForPrintTemplate)

			Dim result As List(Of EmployeeESDataForPrintTemplate) = Nothing

			Dim rFoundedrec As SqlDataReader = Nothing

			Try

				Dim sql As String = String.Empty

				sql &= "SELECT Top 7 ES.ES_Ab, ES.ES_Ende, ES.ES_Als, ES.Arbort, KD.Firma1, KD.Strasse, KD.PLZ, KD.Ort FROM dbo.ES "
				sql &= "LEFT JOIN Kunden KD ON ES.KDNr = KD.KDNr "
				sql &= "Where ES.MDNr = @MDNr And ES.MANr = @MANr "
				sql &= "Order By ES.ES_Ab Desc"

				' Parameters
				Dim listOfParams As New List(Of SqlClient.SqlParameter)
				listOfParams.Add(New SqlClient.SqlParameter("MANr", manr))
				listOfParams.Add(New SqlClient.SqlParameter("mdnr", mdnr))

				Dim reader As SqlClient.SqlDataReader = OpenReader(m_InitializationData.MDData.MDDbConn, sql, listOfParams, CommandType.Text)

				If reader IsNot Nothing Then

					result = New List(Of EmployeeESDataForPrintTemplate)

					While reader.Read

						Dim data = New EmployeeESDataForPrintTemplate

						data.es_ab = m_path.SafeGetDateTime(reader, "ES_Ab", Nothing)
						data.es_ende = m_path.SafeGetDateTime(reader, "ES_Ende", Nothing)
						data.esart = 1
						data.customername = m_path.SafeGetString(reader, "Firma1")
						data.customerstreet = m_path.SafeGetString(reader, "strasse")
						data.customerplz = m_path.SafeGetString(reader, "plz")
						data.customerort = m_path.SafeGetString(reader, "ort")

						data.esals = m_path.SafeGetString(reader, "ES_Als")

						result.Add(data)

					End While

				End If


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.tostring))
				Return Nothing

			End Try

			Return result

		End Function


		''' <summary>
		''' Opens a SqlClient.SqlDataReader object. 
		''' </summary>
		''' <param name="sql">The sql string.</param>
		''' <param name="parameters">The parameters collection.</param>
		''' <returns>The open reader or nothing in error case.</returns>
		''' <remarks>The reader is opened with the CloseConnection option, so when the reader is closed the underlying database connection will also be closed.</remarks>
		Private Function OpenReader(ByVal _conn As String, ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter), Optional ByVal commandType As System.Data.CommandType = CommandType.Text) As SqlClient.SqlDataReader

			Dim Conn As SqlClient.SqlConnection = Nothing

			' Open a new connection
			Conn = New SqlClient.SqlConnection(_conn)

			Dim reader As SqlClient.SqlDataReader

			Try

				' Only open connection if its not an explicit connection
				Conn.Open()

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
				cmd.CommandType = commandType


				If Not parameters Is Nothing Then
					For Each param In parameters
						cmd.Parameters.Add(param)

					Next
				End If

				reader = cmd.ExecuteReader()


			Catch e As Exception
				m_Logger.LogError(String.Format("SQL={0}. Exception={1}", sql, e.ToString()))

				Conn.Close()
				Conn.Dispose()

				reader = Nothing
			End Try

			Return reader

		End Function


	End Class

End Namespace



Namespace KDDbDatabases

	Public Class ClsKDStammDb4Print

		''' <summary>
		''' The logger.
		''' </summary>
		Private Shared m_Logger As ILogger = New Logger()

		Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

		Function TranslateText(ByVal strBez As String) As String
			Dim strOrgText As String = strBez
			Dim strTranslatedText As String = strBez

			Return strTranslatedText
		End Function

		Public Shared Function TranslateMyText(ByVal strBez As String) As String
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim strOrgText As String = strBez
			Dim strTranslatedText As String = MainUtilities.TranslateMyText(strBez)
			Dim _clsLog As New SPProgUtility.ClsEventLog

			Return strTranslatedText
		End Function

		Public Shared Function OpenDb4KDStammPrint(ByVal _setting As ClsLLKDSearchPrintSetting) As SqlDataReader
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim strQuery As String = "[Get KDData For Print In Stammblatt]"
			Dim Conn As SqlConnection = New SqlConnection(_setting.DbConnString2Open)
			Dim rFoundedrec As SqlDataReader = Nothing
			Dim sSql As String = String.Empty
			Dim _ClsReg As New SPProgUtility.ClsDivReg
			Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
			Dim sSortKWLANr As Short = 0

			Try
				Conn.Open()
				Dim cmd As System.Data.SqlClient.SqlCommand
				cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@KDNr", _setting.KDNr2Print)

				rFoundedrec = cmd.ExecuteReader
				rFoundedrec.Read()

				If IsNothing(rFoundedrec) Or Not rFoundedrec.HasRows And _setting.ShowMessageIFNotFounded Then
					Dim strMessage As String = MainUtilities.TranslateMyText("Die gesuchten Daten sind nicht vorhanden. Bitte kontrollieren sie Ihre Angaben.")
					m_Logger.LogError(String.Format("{0}.rFoundedrec: {1}", strMethodeName, strMessage))
					MsgBox(MainUtilities.TranslateMyText(strMessage), _
								 MsgBoxStyle.Critical, strMethodeName)
					Return Nothing

				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}: {1}", strMethodeName, ex.tostring))

			End Try

			Return rFoundedrec
		End Function


	End Class

End Namespace