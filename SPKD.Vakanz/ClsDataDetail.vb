
Imports SP.Infrastructure.Logging
'Imports SP.Infrastructure.UI

'Imports SPProgUtility.SPTranslation.ClsTranslation
'Imports SPProgUtility.ProgPath.ClsProgPath
'Imports SPProgUtility.ProgPath
'Imports SPProgUtility
'Imports SPProgUtility.Mandanten

'Imports System.Windows.Forms
'Imports System.Data.SqlClient

'Imports SPProgUtility.MainUtilities

Public Class ClsDataDetail

  Private m_Logger As ILogger = New Logger()

  Public Shared strKDData As String = String.Empty
  Public Shared strButtonValue As String = String.Empty
  Public Shared strUSSignFilename As String = String.Empty

  Public Shared Get4What As String = String.Empty

  Public Shared GetSortBez As String = String.Empty
  Public Shared LLExportFileName As String = String.Empty
  Public Shared GetFilterBez2 As String = String.Empty
  Public Shared GetFilterBez3 As String = String.Empty
  Public Shared GetFilterBez4 As String = String.Empty

	'Public Shared TranslationData As Dictionary(Of String, ClsTranslationData)
	'Public Shared PersonalizedData As Dictionary(Of String, ClsProsonalizedData)
	'Public Shared ProgSettingData As ClsSetting
	'Public Shared Property GetSelectedMDConnstring As String

	'Public Shared MDData As New ClsMDData
	'Public Shared UserData As New ClsUserData

	'Public Shared ChangedMDData As New ClsMDData
	'Public Shared ChangedUserData As New ClsUserData


	'Public Shared m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Public Shared m_InitialData As SP.Infrastructure.Initialization.InitializeClass
	Public Shared m_InitialChangedData As SP.Infrastructure.Initialization.InitializeClass


	'''' <summary>
	'''' alle übersetzte Elemente in 3 Sprachen
	'''' </summary>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Public Shared ReadOnly Property Translation() As Dictionary(Of String, ClsTranslationData)
	'   Get
	'     Try
	'       Dim m_Translation As New SPProgUtility.SPTranslation.ClsTranslation
	'       Return m_Translation.GetTranslationInObject

	'     Catch ex As Exception
	'       Return Nothing
	'     End Try
	'   End Get
	' End Property

	'''' <summary>
	'''' Individuelle Bezeichnungen für Labels
	'''' </summary>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Public Shared ReadOnly Property PersonalizedName() As Dictionary(Of String, ClsProsonalizedData)
	'  Get
	'    Try
	'      Dim m As New Mandant
	'      Return m.GetPersonalizedCaptionInObject(ProgSettingData.SelectedMDNr)

	'    Catch ex As Exception
	'      Return Nothing
	'    End Try
	'  End Get
	'End Property

	'''' <summary>
	'''' Datenbankeinstellungen für [Sputnik DBSelect]
	'''' </summary>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Public Shared ReadOnly Property SelectedDbSelectData() As ClsDbSelectData
	'   Get
	'     Dim m_Progpath As New ClsProgPath

	'     Return m_Progpath.GetDbSelectData()
	'   End Get
	' End Property

	'''' <summary>
	'''' Datenbankeinstellungen aus der \\Server\spenterprise$\bin\Programm.XML. gemäss Mandantennummer
	'''' </summary>
	'''' <param name="iMDNr"></param>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Public Shared ReadOnly Property SelectedMDData(ByVal iMDNr As Integer) As ClsMDData
	'  Get
	'    Dim m_md As New Mandant
	'    MDData = m_md.GetSelectedMDData(iMDNr) 'ProgSettingData.SelectedMDNr)
	'    ProgSettingData.SelectedMDNr = MDData.MDNr
	'    GetSelectedMDConnstring = MDData.MDDbConn

	'    Return MDData
	'  End Get
	'End Property

	'''' <summary>
	'''' Benutzerdaten aus der Datenbank
	'''' </summary>
	'''' <param name="iMDNr"></param>
	'''' <param name="iUserNr"></param>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Public Shared ReadOnly Property LogededUSData(ByVal iMDNr As Integer, ByVal iUserNr As Integer) As ClsUserData
	'  Get
	'    Dim m_md As New Mandant
	'    UserData = m_md.GetSelectedUserData(iMDNr, iUserNr)
	'    ProgSettingData.LogedUSNr = UserData.UserNr

	'    Return UserData
	'  End Get
	'End Property


	'''' <summary>
	'''' Datenbankeinstellungen aus der \\Server\spenterprise$\bin\Programm.XML. gemäss Mandantennummer
	'''' </summary>
	'''' <param name="iMDNr"></param>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Public Shared ReadOnly Property ChenagedSelectedMDData(ByVal iMDNr As Integer) As ClsMDData
	'   Get
	'     Dim m_md As New Mandant
	'     MDData = m_md.GetSelectedMDData(iMDNr)
	'		m_InitialData.MDData.MDNr = MDData.MDNr
	'		GetSelectedMDConnstring = MDData.MDDbConn

	'     Return MDData
	'   End Get
	' End Property

	'''' <summary>
	'''' Benutzerdaten aus der Datenbank
	'''' </summary>
	'''' <param name="iMDNr"></param>
	'''' <param name="UserKST"></param>
	'''' <value></value>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Public Shared ReadOnly Property ChangedSelectedUSData(ByVal iMDNr As Integer, ByVal UserKST As String) As ClsUserData
	'   Get
	'     Dim m_md As New Mandant
	'     ChangedUserData = m_md.GetSelectedUserDataWithKST(iMDNr, UserKST)

	'     Return ChangedUserData
	'   End Get
	' End Property







	Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "C4B5AC2E-56BA-4331-964A-AD9A8E303CF5"
    End Get
  End Property

	'Public Shared ReadOnly Property IsJobCHAllowed() As Boolean

	'  Get
	'    Dim sSQL As String = "Select Top 1 Organisation_ID From US_JobPlattforms "
	'    sSQL &= "Where Customer_Guid = @Customer_Guid And Organisation_ID <> 0 And Jobplattform_Art = 1 And [Organisation_Kontingent] > 0"
	'    Dim bResult As Boolean = False
	'    Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	'    Dim Conn As SqlClient.SqlConnection = New SqlClient.SqlConnection(ClsDataDetail.ChangedMDData.MDDbConn)

	'    Try
	'      Conn.Open()

	'      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
	'      cmd.CommandType = Data.CommandType.Text
	'      Dim param As System.Data.SqlClient.SqlParameter

	'      param = cmd.Parameters.AddWithValue("@Customer_Guid", ClsDataDetail.ChangedMDData.MDGuid)

	'      Dim rMyrec As SqlClient.SqlDataReader = cmd.ExecuteReader
	'      bResult = rMyrec.HasRows

	'    Catch e As Exception
	'      MsgBox(e.Message, MsgBoxStyle.Critical, "Anmeldedaten für Job-Plattformen")

	'    Finally
	'      Conn.Close()
	'      Conn.Dispose()

	'    End Try

	'    Return bResult
	'  End Get
	'End Property

	'Public Shared ReadOnly Property IsOstJobAllowed() As Boolean

	'  Get
	'    Dim sSQL As String = "Select Top 1 Organisation_ID From US_JobPlattforms "
	'    sSQL &= "Where Customer_Guid = @Customer_Guid And Organisation_ID <> 0 And Jobplattform_Art = 2 And [Organisation_Kontingent] > 0"
	'    Dim bResult As Boolean = False
	'    Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	'    Dim Conn As SqlClient.SqlConnection = New SqlClient.SqlConnection(ClsDataDetail.ChangedMDData.MDDbConn)

	'    Try
	'      Conn.Open()

	'      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
	'      cmd.CommandType = Data.CommandType.Text
	'      Dim param As System.Data.SqlClient.SqlParameter

	'      param = cmd.Parameters.AddWithValue("@Customer_Guid", ClsDataDetail.ChangedMDData.MDGuid)

	'      Dim rMyrec As SqlClient.SqlDataReader = cmd.ExecuteReader
	'      bResult = rMyrec.HasRows

	'    Catch e As Exception
	'      MsgBox(e.Message, MsgBoxStyle.Critical, "Anmeldedaten für Job-Plattformen")

	'    Finally
	'      Conn.Close()
	'      Conn.Dispose()

	'    End Try

	'    Return bResult
	'  End Get
	'End Property

	'Public Shared ReadOnly Property GetCountOfJCHExportedRec(ByVal iVakNr As Integer, ByVal UserKST As String) As JobCHCounterInfo

	'   Get
	'     Dim m_Translate As New TranslateValues
	'     Dim result As New JobCHCounterInfo
	'     Dim m_utility As New Utilities
	'     Dim sSQL As String = "[Get Count of Exported Vacancy to Jobs_CH]"

	'     Try
	'       Dim listOfParams As New List(Of SqlClient.SqlParameter)

	'       listOfParams.Add(New SqlClient.SqlParameter("@VakNr", iVakNr))
	'       listOfParams.Add(New SqlClient.SqlParameter("@UserKST", UserKST))
	'       listOfParams.Add(New SqlClient.SqlParameter("@Customer_Guid", ClsDataDetail.ChangedMDData.MDGuid))

	'       Dim resultTotalQuantity = New SqlClient.SqlParameter("@TotalRec", SqlDbType.Int)
	'       resultTotalQuantity.Direction = ParameterDirection.Output
	'       listOfParams.Add(resultTotalQuantity)

	'       Dim resultExportedQuantity = New SqlClient.SqlParameter("@AnzExportedRec", SqlDbType.Int)
	'       resultExportedQuantity.Direction = ParameterDirection.Output
	'       listOfParams.Add(resultExportedQuantity)

	'       Dim resultCounter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
	'       resultCounter.Direction = ParameterDirection.Output
	'       listOfParams.Add(resultCounter)


	'       Dim reader = m_utility.ExecuteNonQuery(ClsDataDetail.ChangedMDData.MDDbConn, sSQL, listOfParams, CommandType.StoredProcedure)

	'       If reader Then
	'         If Not resultTotalQuantity.Value Is Nothing Then
	'           result.AllowedJobQuantity = resultTotalQuantity.Value
	'         End If
	'         If Not resultExportedQuantity.Value Is Nothing Then
	'           result.ExportedJobQuantity = resultExportedQuantity.Value
	'         End If

	'         If resultCounter.Value > 0 Then
	'           Dim strMsg As String = "Achtung: Ihre Vakanz darf nicht Online gestellt werden. Sie würden Ihr Kontingent überschreiten."
	'           DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg),
	'                                                      m_Translate.GetSafeTranslationValue("Onlinestatus: jobs.ch"),
	'                                                      MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
	'           result.IsCounterOK = False
	'         Else
	'           result.IsCounterOK = True

	'         End If

	'       End If


	'     Catch e As Exception
	'       MsgBox(e.Message, MsgBoxStyle.Critical, m_Translate.GetSafeTranslationValue("Anzahl publizierte Datensätze in Jobs.ch"))

	'     Finally

	'     End Try

	'     Return result
	'   End Get
	' End Property

	'Public Shared ReadOnly Property GetCountOfOstJobExportedRec(ByVal iVakNr As Integer, ByVal UserKST As String) As OstJobCounterInfo

	'   Get
	'     Dim m_Translate As New TranslateValues
	'     Dim result As New OstJobCounterInfo
	'     Dim m_utility As New Utilities
	'     Dim sSQL As String = "[Get Count of Exported Vacancy to OstJob]"

	'     Try
	'       Dim listOfParams As New List(Of SqlClient.SqlParameter)

	'       listOfParams.Add(New SqlClient.SqlParameter("@VakNr", iVakNr))
	'       listOfParams.Add(New SqlClient.SqlParameter("@UserKST", UserKST))
	'       listOfParams.Add(New SqlClient.SqlParameter("@Customer_Guid", ClsDataDetail.ChangedMDData.MDGuid))

	'       Dim resultTotalQuantity = New SqlClient.SqlParameter("@TotalRec", SqlDbType.Int)
	'       resultTotalQuantity.Direction = ParameterDirection.Output
	'       listOfParams.Add(resultTotalQuantity)

	'       Dim resultExportedQuantity = New SqlClient.SqlParameter("@AnzExportedRec", SqlDbType.Int)
	'       resultExportedQuantity.Direction = ParameterDirection.Output
	'       listOfParams.Add(resultExportedQuantity)

	'       Dim resultCounter = New SqlClient.SqlParameter("@Result", SqlDbType.Int)
	'       resultCounter.Direction = ParameterDirection.Output
	'       listOfParams.Add(resultCounter)


	'       Dim reader = m_utility.ExecuteNonQuery(ClsDataDetail.ChangedMDData.MDDbConn, sSQL, listOfParams, CommandType.StoredProcedure)

	'       If reader Then
	'         If Not resultTotalQuantity.Value Is Nothing Then
	'           result.AllowedJobQuantity = resultTotalQuantity.Value
	'         End If
	'         If Not resultExportedQuantity.Value Is Nothing Then
	'           result.ExportedJobQuantity = resultExportedQuantity.Value
	'         End If

	'         If resultCounter.Value > 0 Then
	'           Dim strMsg As String = "Achtung: Ihre Vakanz darf nicht Online gestellt werden. Sie würden Ihr Kontingent überschreiten."
	'           DevExpress.XtraEditors.XtraMessageBox.Show(m_Translate.GetSafeTranslationValue(strMsg),
	'                                                      m_Translate.GetSafeTranslationValue("Onlinestatus: ostjob.ch"),
	'                                                      MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
	'           result.IsCounterOK = False
	'         Else
	'           result.IsCounterOK = True

	'         End If

	'       End If


	'     Catch e As Exception
	'       MsgBox(e.Message, MsgBoxStyle.Critical, m_Translate.GetSafeTranslationValue("Anzahl publizierte Datensätze in OstJob.ch"))

	'     Finally

	'     End Try

	'     Return result
	'   End Get
	' End Property


	'Public Shared ReadOnly Property GetCountOfInternJobExportedRec() As String

	'   Get
	'     Dim sSQL As String = "[Get Count of Exported Vacancy to own Homepage]"
	'     Dim strResult As String = "0"
	'     Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	'     Dim Conn As SqlClient.SqlConnection = New SqlClient.SqlConnection(ClsDataDetail.ChangedMDData.MDDbConn)

	'     Try
	'       Conn.Open()

	'       Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSQL, Conn)
	'       cmd.CommandType = Data.CommandType.StoredProcedure

	'       Dim rMyrec As SqlClient.SqlDataReader = cmd.ExecuteReader
	'       rMyrec.Read()
	'       If rMyrec.HasRows Then strResult = String.Format("{0}", rMyrec("AnzExportedRec"))

	'     Catch e As Exception
	'       MsgBox(e.Message, MsgBoxStyle.Critical, "Anzahl publizierte Datensätze in unserem Job-Plattform")

	'     Finally
	'       Conn.Close()
	'       Conn.Dispose()

	'     End Try

	'     Return strResult
	'   End Get
	' End Property





#Region "Speichervariable"

	'// SQL-Query
	Shared _SQLString As String
  Public Shared Property SQLQuery() As String
    Get
      Return _SQLString
    End Get
    Set(ByVal value As String)
      _SQLString = value
    End Set
  End Property

  '// SelectedNumbers
  Shared _SelectedNumbers As String = ""
  Public Shared Property GetSelectedNumbers() As String
    Get
      Return _SelectedNumbers
    End Get
    Set(ByVal value As String)
      _SelectedNumbers = value
    End Set
  End Property

  '// SelectedBez
  Shared _SelectedBez As String = ""
  Public Shared Property GetSelectedBez() As String
    Get
      Return _SelectedBez
    End Get
    Set(ByVal value As String)
      _SelectedBez = value
    End Set
  End Property

  '// Darf geändert werden?
  Shared _bAllowedChange As Boolean
  Public Shared Property bAllowedToChange() As Boolean
    Get
      Return _bAllowedChange
    End Get
    Set(ByVal value As Boolean)
      _bAllowedChange = value
    End Set
  End Property

	''// Darf KST geändert werden?
	'Shared _bAllowedChangeKST As Boolean
	'Public Shared Property bAllowedTochangeKST() As Boolean
	'  Get
	'    Return _bAllowedChangeKST
	'  End Get
	'  Set(ByVal value As Boolean)
	'    _bAllowedChangeKST = value
	'  End Set
	'End Property

	'// Ist der Satz New?
	Shared _bAsNew As Boolean
  Public Shared Property bAsNew() As Boolean
    Get
      Return _bAsNew
    End Get
    Set(ByVal value As Boolean)
      _bAsNew = value
    End Set
  End Property


	''// Vakanzen-Nummer
	'Shared _iVNr As Integer
	'Public Shared Property GetVakanzNr() As Integer
	'  Get
	'    Return _iVNr
	'  End Get
	'  Set(ByVal value As Integer)
	'    _iVNr = value
	'  End Set
	'End Property

	'// Kandidatennummer
	Shared _iMANr As Integer
  Public Shared Property GetProposalMANr() As Integer
    Get
      Return _iMANr
    End Get
    Set(ByVal value As Integer)
      _iMANr = value
    End Set
  End Property

  '// Kundenennummer
  Shared _iKDNr As Integer
  Public Shared Property GetProposalKDNr() As Integer
    Get
      Return _iKDNr
    End Get
    Set(ByVal value As Integer)
      _iKDNr = value
    End Set
  End Property

  '// ZHD-Nummer
  Shared _iZHDNr As Integer
  Public Shared Property GetProposalZHDNr() As Integer
    Get
      Return _iZHDNr
    End Get
    Set(ByVal value As Integer)
      _iZHDNr = value
    End Set
  End Property


#End Region


	'' Helps extracting a column value form a data reader.
	'Public Shared Function GetColumnTextStr(ByVal dr As SqlClient.SqlDataReader, _
	'                                        ByVal columnName As String, ByVal replacementOnNull As String) As String

	'  If Not dr.IsDBNull(dr.GetOrdinal(columnName)) Then
	'    If String.IsNullOrEmpty(CStr(dr(columnName))) Then
	'      Return replacementOnNull
	'    End If
	'    Return CStr(dr(columnName))
	'  End If

	'  Return replacementOnNull
	'End Function



End Class


'''' <summary>
'''' Translate given or founded text
'''' </summary>
'''' <remarks></remarks>
'Public Class TranslateValues


'  Function GetSafeTranslationValue(ByVal dicKey As String) As String
'    Dim strPersonalizedItem As String = dicKey

'    Try
'      If ClsDataDetail.TranslationData.ContainsKey(strPersonalizedItem) Then
'        Return ClsDataDetail.TranslationData.Item(strPersonalizedItem).LogedUserLanguage

'      Else
'        Return strPersonalizedItem

'      End If

'    Catch ex As Exception
'      Return strPersonalizedItem
'    End Try

'  End Function

'  Function GetSafeTranslationValue(ByVal dicKey As String, ByVal bCheckPersonalizedItem As Boolean) As String
'    Dim strPersonalizedItem As String = dicKey

'    Try
'      If bCheckPersonalizedItem Then
'        If ClsDataDetail.PersonalizedData.ContainsKey(dicKey) Then
'          strPersonalizedItem = ClsDataDetail.PersonalizedData.Item(dicKey).CaptionValue

'        Else
'          strPersonalizedItem = strPersonalizedItem

'        End If
'      End If

'      If ClsDataDetail.TranslationData.ContainsKey(strPersonalizedItem) Then
'        Return ClsDataDetail.TranslationData.Item(strPersonalizedItem).LogedUserLanguage

'      Else
'        Return strPersonalizedItem

'      End If

'    Catch ex As Exception
'      Return strPersonalizedItem
'    End Try

'  End Function

'End Class
