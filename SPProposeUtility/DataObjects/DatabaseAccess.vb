
Imports System.Data.SqlClient
Imports SP.Infrastructure.Logging

Imports SPProposeUtility.ClsDataDetail


''' <summary>
''' Database access class.
''' </summary>
Public Class DBAccess
  Implements IDatabaseAccess

#Region "Private Fields"

  Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Private m_Logger As ILogger = New Logger()

  ''' <summary>
  ''' Gets or sets the selected translation language.
  ''' </summary>
  ''' <returns>The selected translation language.</returns>
  Public Property SelectedTranslationLanguage As Language = Language.German

#End Region

#Region "Public Methods"


  ''' <summary>
  ''' Loads employee master data (Mitarbeiter).
  ''' </summary>
  ''' <param name="employeeNumber">The employee number.</param>
  ''' <param name="includeImageData">Optional flag indicating if image data should also be loaded.</param>
  ''' <returns>Employee master data or nothing in error case.</returns>
  Public Function LoadEmployeeMasterData(ByVal employeeNumber As Integer, Optional includeImageData As Boolean = False) As EmployeeMasterData Implements IDatabaseAccess.LoadEmployeeMasterData

    Dim employeeMasterData As EmployeeMasterData = Nothing

    Dim sql As String = String.Empty

    sql = sql & "SELECT [ID]"
    sql = sql & ",[MANr]"
    sql = sql & ",[Nachname]"
    sql = sql & ",[Vorname]"
    sql = sql & ",[Postfach]"
    sql = sql & ",[Strasse]"
    sql = sql & ",[PLZ]"
    sql = sql & ",[Ort]"
    sql = sql & ",[Land]"
    sql = sql & ",[Sprache]"
    sql = sql & ",[GebDat]"
    sql = sql & ",[Geschlecht]"
    sql = sql & ",[AHV_Nr]"
    sql = sql & ",[Nationality]"
    sql = sql & ",[Zivilstand]"
    sql = sql & ",[Telefon_P]"
    sql = sql & ",[Telefon2]"
    sql = sql & ",[Telefon3]"
    sql = sql & ",[Telefon_G]"
    sql = sql & ",[Natel]"
    sql = sql & ",[Homepage]"
    sql = sql & ",[eMail]"
    sql = sql & ",[Facebook]"
    sql = sql & ",[Xing]"
    sql = sql & ",[Bewillig]"
    sql = sql & ",[Bew_Bis]"
    sql = sql & ",[GebOrt]"
    sql = sql & ",[Q_Steuer]"
    sql = sql & ",[S_Kanton]"
    sql = sql & ",[Kirchensteuer]"
    sql = sql & ",[Ansaessigkeit]"
    sql = sql & ",[Kinder]"
    sql = sql & ",[Beruf]"
    sql = sql & ",[Wohnt_bei]"
    sql = sql & ",[V_Hinweis]"
    sql = sql & ",[CreatedOn]"
    sql = sql & ",[ChangedOn]"
    sql = sql & ",[CreatedFrom]"
    sql = sql & ",[ChangedFrom]"
    sql = sql & ",[Bild]"

    If (includeImageData) Then
      sql = sql & ",[MABild]"
    End If

    sql = sql & ",[Result]"
    sql = sql & ",[KST]"
    sql = sql & ",[ErstKontakt]"
    sql = sql & ",[LetztKontakt]"
    sql = sql & ",[QSTGemeinde]"
    sql = sql & ",[Filiale]"
    sql = sql & ",[GAVBez]"
    sql = sql & ",[Zivilstand2]"
    sql = sql & ",[QLand]"
    sql = sql & ",[MAFiliale]"
    sql = sql & ",[AHV_Nr_New]"
    sql = sql & ",[MA_Kanton]"
    sql = sql & ",[Ans_QST_Bis]"
    sql = sql & ",[Transfered_Guid]"
    sql = sql & ",[Transfered_User]"
    sql = sql & ",[Transfered_On]"
    sql = sql & ",[Send2WOS]"
    sql = sql & ",[MA_SMS_Mailing]"
    sql = sql & ",[BerufCode]"
    sql = sql & ",[Transfered_Guid]"
    sql = sql & ",[MDNr]"
    sql = sql & ",[Natel2]"

    sql &= " FROM Mitarbeiter MA "
    sql &= "WHERE MA.MANr = @maNumber "

    ' Parameters
    Dim employeeNumberParameter As New SqlClient.SqlParameter("maNumber", employeeNumber)
    Dim listOfParams As New List(Of SqlClient.SqlParameter)
    listOfParams.Add(employeeNumberParameter)

    Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

    Try

      If Not reader Is Nothing Then

        If reader.Read Then
          employeeMasterData = New EmployeeMasterData

          employeeMasterData.ID = SafeGetInteger(reader, "ID", 0)
          employeeMasterData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
          employeeMasterData.Lastname = SafeGetString(reader, "Nachname")
          employeeMasterData.Firstname = SafeGetString(reader, "Vorname")
          employeeMasterData.PostOfficeBox = SafeGetString(reader, "Postfach")
          employeeMasterData.Street = SafeGetString(reader, "Strasse")
          employeeMasterData.Postcode = SafeGetString(reader, "PLZ")
          employeeMasterData.Location = SafeGetString(reader, "Ort")
          employeeMasterData.Country = SafeGetString(reader, "Land")
          employeeMasterData.Language = SafeGetString(reader, "Sprache")
          employeeMasterData.Birthdate = SafeGetDateTime(reader, "GebDat", Nothing)
          employeeMasterData.Gender = SafeGetString(reader, "Geschlecht")
          employeeMasterData.AHV_Nr = SafeGetString(reader, "AHV_Nr")
          employeeMasterData.Nationality = SafeGetString(reader, "Nationality")
          employeeMasterData.CivilStatus = SafeGetString(reader, "Zivilstand")
          employeeMasterData.Telephone_P = SafeGetString(reader, "Telefon_P")
          employeeMasterData.Telephone2 = SafeGetString(reader, "Telefon2")
          employeeMasterData.Telephone3 = SafeGetString(reader, "Telefon3")
          employeeMasterData.Telephone_G = SafeGetString(reader, "Telefon_G")
          employeeMasterData.MobilePhone = SafeGetString(reader, "Natel")
          employeeMasterData.Homepage = SafeGetString(reader, "Homepage")
          employeeMasterData.Email = SafeGetString(reader, "eMail")
          employeeMasterData.Permission = SafeGetString(reader, "Bewillig")
          employeeMasterData.PermissionToDate = SafeGetDateTime(reader, "Bew_Bis", Nothing)
          employeeMasterData.BirthPlace = SafeGetString(reader, "GebOrt")
          employeeMasterData.Q_Steuer = SafeGetString(reader, "Q_Steuer")
          employeeMasterData.S_Canton = SafeGetString(reader, "S_Kanton")
          employeeMasterData.ChurchTax = SafeGetString(reader, "Kirchensteuer")
          employeeMasterData.Residence = SafeGetBoolean(reader, "Ansaessigkeit", Nothing)
          employeeMasterData.ChildsCount = SafeGetShort(reader, "Kinder", Nothing)
          employeeMasterData.Profession = SafeGetString(reader, "Beruf")
          employeeMasterData.StaysAt = SafeGetString(reader, "Wohnt_bei")
          employeeMasterData.V_Hint = SafeGetString(reader, "V_Hinweis")
          employeeMasterData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
          employeeMasterData.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
          employeeMasterData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
          employeeMasterData.ChangedFrom = SafeGetString(reader, "ChangedFrom")
          employeeMasterData.HasImage = SafeGetBoolean(reader, "Bild", False)
          If (includeImageData) Then
            employeeMasterData.MABild = SafeGetByteArray(reader, "MABild")
          End If
          employeeMasterData.Result = SafeGetString(reader, "Result")
          employeeMasterData.KST = SafeGetString(reader, "KST")
          employeeMasterData.FirstContact = SafeGetDateTime(reader, "ErstKontakt", Nothing)
          employeeMasterData.LastContact = SafeGetDateTime(reader, "LetztKontakt", Nothing)
          employeeMasterData.QSTCommunity = SafeGetString(reader, "QSTGemeinde")
          employeeMasterData.BusinessBranch = SafeGetString(reader, "Filiale")
          employeeMasterData.GAVBez = SafeGetString(reader, "GAVBez")
          employeeMasterData.CivilState2 = SafeGetString(reader, "Zivilstand2")
          employeeMasterData.QLand = SafeGetString(reader, "QLand")
          employeeMasterData.MABusinessBranch = SafeGetString(reader, "MAFiliale")
          employeeMasterData.AHV_Nr_New = SafeGetString(reader, "AHV_Nr_New")
          employeeMasterData.MA_Canton = SafeGetString(reader, "MA_Kanton")
          employeeMasterData.ANS_OST_Bis = SafeGetDateTime(reader, "Ans_QST_Bis", Nothing)
          employeeMasterData.Transfered_Guid = SafeGetString(reader, "Transfered_Guid")
          employeeMasterData.Transfered_On = SafeGetDateTime(reader, "Transfered_On", Nothing)
          employeeMasterData.Send2WOS = SafeGetBoolean(reader, "Send2WOS", Nothing)
          employeeMasterData.MA_SMS_Mailing = SafeGetBoolean(reader, "MA_SMS_Mailing", Nothing)
          employeeMasterData.ProfessionCode = SafeGetInteger(reader, "BerufCode", Nothing)
          employeeMasterData.WOSGuid = SafeGetString(reader, "Transfered_Guid")
          employeeMasterData.MDNr = SafeGetInteger(reader, "MDnr", Nothing)
          employeeMasterData.Facebook = SafeGetString(reader, "facebook")
          employeeMasterData.Xing = SafeGetString(reader, "xing")
          employeeMasterData.MobilePhone2 = SafeGetString(reader, "Natel2")

        End If

      End If

    Catch ex As Exception
      m_Logger.LogError(ex.ToString())
      employeeMasterData = Nothing
    Finally
      CloseReader(reader)
    End Try

    Return employeeMasterData

    Return Nothing
  End Function

  ''' <summary>
  ''' Loads employee data.
  ''' </summary>
  ''' <returns>List of employee data.</returns>
  Public Function LoadEmployeeData() As IEnumerable(Of EmployeeData) Implements IDatabaseAccess.LoadEmployeeData

    Dim result As List(Of EmployeeData) = Nothing

    Dim sql As String

    sql = "SELECT MA.MANr, MA.Nachname, MA.Vorname, MA.PLZ, MA.Ort, mk.KStat1, mk.DStellen, mk.NoES FROM Mitarbeiter MA "
    sql &= "Left Join MAKontakt_Komm mk On MA.MANr = mk.MANr "
    sql &= "ORDER BY MA.Nachname, MA.Vorname"

    Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

    Try

      If (Not reader Is Nothing) Then

        result = New List(Of EmployeeData)

        While reader.Read

          Dim employeeData = New EmployeeData()
          employeeData.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
          employeeData.LastName = SafeGetString(reader, "Nachname")
          employeeData.Firstname = SafeGetString(reader, "Vorname")
          employeeData.Postcode = SafeGetString(reader, "PLZ")
          employeeData.Location = SafeGetString(reader, "Ort")

          employeeData.fstate = SafeGetString(reader, "KStat1", "")
          employeeData.DStellen = SafeGetBoolean(reader, "DStellen", False)
          employeeData.NoES = SafeGetBoolean(reader, "NoES", False)

          result.Add(employeeData)

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
  ''' Loads employee contact comm data(MAKontakt_Komm).
  ''' </summary>
  ''' <param name="employeeNumber">The employee number.</param>
  ''' <returns>List ofemployee contact comm data.</returns>
  Public Function LoadEmployeeContactCommData(ByVal employeeNumber As Integer) As EmployeeContactComm Implements IDatabaseAccess.LoadEmployeeContactCommData

    Dim result As EmployeeContactComm = Nothing

    Dim sql As String
    sql = "SELECT * FROM MAKontakt_Komm WHERE MANr = @employeeNumber ORDER BY ID ASC"

    Dim listOfParams As New List(Of SqlClient.SqlParameter)

    listOfParams.Add(New SqlClient.SqlParameter("@employeeNumber", ReplaceMissing(employeeNumber, DBNull.Value)))

    Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

    Try

      If Not reader Is Nothing Then

        If reader.Read Then
          result = New EmployeeContactComm
          result.ID = SafeGetInteger(reader, "ID", 0)
          result.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
          result.AnredeForm = SafeGetString(reader, "AnredeForm")
          result.BriefAnrede = SafeGetString(reader, "BriefAnrede")
          result.KontaktHow = SafeGetString(reader, "KontaktHow")
          result.KStat1 = SafeGetString(reader, "KStat1", "")
          result.KStat2 = SafeGetString(reader, "KStat2", "")
          result.WebExport = SafeGetBoolean(reader, "WebExport", False)
          result.ESAb = SafeGetDateTime(reader, "ESAb", Nothing)
          result.ESEnde = SafeGetDateTime(reader, "ESEnde", Nothing)
          result.Absenzen = SafeGetString(reader, "Absenzen")
          result.NoWorkAS = SafeGetString(reader, "NoWorkAS")
          result.InLandSeit = SafeGetString(reader, "InLandSeit")
          result.GetAHVKarte = SafeGetBoolean(reader, "GetAHVKarte", Nothing)
          result.GetAHVKarteBez = SafeGetString(reader, "GetAHVKarteBez")
          result.AHVKarteBacked = SafeGetBoolean(reader, "AHVKarteBacked", Nothing)
          result.AHVKateBackedBez = SafeGetString(reader, "AHVKarteBackedBez")
          result.InZV = SafeGetBoolean(reader, "InZV", Nothing)
          result.InZVBez = SafeGetString(reader, "InZVBez")
          result.RahmenArbeit = SafeGetBoolean(reader, "RahmenArbeit", Nothing)
          result.RahemArbeitBez = SafeGetString(reader, "RahmenArbeitBez")
          result.Res1 = SafeGetString(reader, "Res1")
          result.Res2 = SafeGetString(reader, "Res2")
          result.Res3 = SafeGetString(reader, "Res3")
          result.Res4 = SafeGetString(reader, "Res4")
          result.KundFristen = SafeGetString(reader, "KundFristen")
          result.KundGrund = SafeGetString(reader, "KundGrund")
          result.Arbeitspensum = SafeGetString(reader, "Arbeitspensum")
          result.GehaltAlt = SafeGetDecimal(reader, "GehaltAlt", Nothing)
          result.GehaltNeu = SafeGetDecimal(reader, "GehaltNeu", Nothing)
          result.GotDocs = SafeGetBoolean(reader, "GotDocs", Nothing)
          result.Result = SafeGetString(reader, "Result")
          result.GehaltPerMonth = SafeGetDecimal(reader, "GehaltPerMonth", Nothing)
          result.GehaltPerStd = SafeGetDecimal(reader, "GehaltPerStd", Nothing)
          result.DStellen = SafeGetBoolean(reader, "DStellen", False)
          result.NoES = SafeGetBoolean(reader, "NoES", False)
          result.Res5 = SafeGetString(reader, "Res5")
          result.AGB_WOS = SafeGetString(reader, "AGB_WOS")
          result.ZVeMail = SafeGetString(reader, "ZVeMail")
          result.ZVVersand = SafeGetString(reader, "ZVVersand")

        End If

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
  ''' Lodas customer data.
  ''' </summary>
  ''' <returns>List of customer data.</returns>
  Public Function LoadCustomerData() As IEnumerable(Of CustomerData) Implements IDatabaseAccess.LoadCustomerData

    Dim result As List(Of CustomerData) = Nothing

    Dim sql As String

    sql = "SELECT KDNr, Firma1, Strasse, PLZ, Ort, KDState1, KDState2, HowKontakt, NoES FROM Kunden ORDER BY Firma1 ASC"

    Dim reader As SqlClient.SqlDataReader = OpenReader(sql, Nothing)

    Try

      If (Not reader Is Nothing) Then

        result = New List(Of CustomerData)

        While reader.Read

          Dim customerData = New CustomerData()
          customerData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
          customerData.Company1 = SafeGetString(reader, "Firma1")
          customerData.Street = SafeGetString(reader, "Strasse")
          customerData.Postcode = SafeGetString(reader, "PLZ")
          customerData.Location = SafeGetString(reader, "Ort")

          customerData.fstate = SafeGetString(reader, "KDState1")
          customerData.sstate = SafeGetString(reader, "KDState2")
          customerData.howcontact = SafeGetString(reader, "HowKontakt")

          customerData.noes = SafeGetBoolean(reader, "noes", False)

          result.Add(customerData)

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
  ''' Loads responsible person data.
  ''' </summary>
  ''' <param name="customerNumber">The customer number.</param>
  ''' <returns>Responsible person data.</returns>
  Public Function LoadResponsiblePersonDataActiv(ByVal customerNumber As Integer) As IEnumerable(Of ResponsiblePersonData) Implements IDatabaseAccess.LoadResponsiblePersonDataActiv

    Dim result As List(Of ResponsiblePersonData) = Nothing

    Dim sql As String

    sql = "Select Top 500 ZHD.*, "
    sql = sql & String.Format("IsNull((SELECT TOP 1 Anrede_{0} FROM Anrede ANR WHERE ANR.Anrede = ZHD.Anrede), '') AS TranslatedAnrede ", MapLanguageToShortLanguageCode(SelectedTranslationLanguage))
    sql = sql & "FROM KD_Zustaendig ZHD Where KDNr = @customerNumber "
    sql &= "And (ZHD.KDZState1 Not In ('nicht mehr aktiv') Or ZHD.KDZState1 Is Null) "
    sql = sql & "Order By Nachname Asc, Vorname Asc"

    ' Parameters
    Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
    Dim listOfParams As New List(Of SqlClient.SqlParameter)
    listOfParams.Add(customerNumberParameter)

    Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

    Try
      If Not reader Is Nothing Then
        result = New List(Of ResponsiblePersonData)

        While reader.Read
          Dim responsibleData As New ResponsiblePersonData
          responsibleData.ID = SafeGetInteger(reader, "ID", 0)
          responsibleData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
          responsibleData.RecordNumber = SafeGetInteger(reader, "RecNr", 0)
          responsibleData.Position = SafeGetString(reader, "Position")
          responsibleData.Department = SafeGetString(reader, "Abteilung")
          responsibleData.Salutation = SafeGetString(reader, "Anrede")
          responsibleData.Firstname = SafeGetString(reader, "Vorname")
          responsibleData.Lastname = SafeGetString(reader, "Nachname")
          responsibleData.Telephone = SafeGetString(reader, "Telefon")
          responsibleData.Telefax = SafeGetString(reader, "Telefax")
          responsibleData.MobilePhone = SafeGetString(reader, "Natel")
          responsibleData.Email = SafeGetString(reader, "eMail")
          responsibleData.TranslatedSalutation = SafeGetString(reader, "TranslatedAnrede")

          result.Add(responsibleData)
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
  ''' Lodas Vacancy data.
  ''' </summary>
  ''' <param name="vacancyNumber"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function LoadVacancyMasterData(ByVal vacancyNumber As Integer) As VacancyMasterData Implements IDatabaseAccess.LoadVacancyMasterData

    Dim result As VacancyMasterData = Nothing

    Dim sql As String

    sql = "SELECT v.VakNr, v.bezeichnung "
    sql &= "FROM dbo.Vakanzen V "
    sql &= "WHERE v.vakNr = @vacancyNumber "

    ' Parameters
    Dim vacancyNumberParameter As New SqlClient.SqlParameter("vacancyNumber", vacancyNumber)
    Dim listOfParams As New List(Of SqlClient.SqlParameter)
    listOfParams.Add(vacancyNumberParameter)

    Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

    Try

      If Not reader Is Nothing Then
        If reader.Read Then

          result = New VacancyMasterData()

          result.vacancynumber = SafeGetInteger(reader, "vakNr", 0)
          result.vacancybez = SafeGetString(reader, "Bezeichnung")

        End If

      End If


    Catch e As Exception
      result = Nothing
      m_Logger.LogError(e.StackTrace)

    Finally
      CloseReader(reader)
    End Try

    Return result

  End Function

  ''' <summary>
  ''' Lodas Vacancy data.
  ''' </summary>
  ''' <returns>List of vacancy data.</returns>
  Public Function LoadVacancyData(ByVal customerNumber As Integer) As IEnumerable(Of VacancyData) Implements IDatabaseAccess.LoadVacancyData

    Dim result As List(Of VacancyData) = Nothing

    Dim sql As String

    sql = "SELECT v.VakNr, v.bezeichnung, v.Createdon, "
    sql &= "( CASE ISNUMERIC(v.VakState) "
    sql &= "WHEN 1 THEN ISNULL((SELECT Bez_D FROM tbl_base_VakState tbl WHERE tbl.recvalue = v.VakState), '') "
    sql &= "ELSE v.VakState "
    sql &= "End "
    sql &= ") "
    sql &= "AS vState, "
    sql &= "kd.strasse, kd.plz, kd.ort  FROM dbo.Vakanzen V "
    sql &= "LEFT JOIN Kunden KD ON v.kdnr = kd.kdnr "
    sql &= "WHERE v.KDNr = @customerNumber "
    sql &= "Order By v.Createdon Desc "

    ' Parameters
    Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
    Dim listOfParams As New List(Of SqlClient.SqlParameter)
    listOfParams.Add(customerNumberParameter)

    Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

    Try

      If (Not reader Is Nothing) Then

        result = New List(Of VacancyData)

        While reader.Read

          Dim vacancyData = New VacancyData()
          vacancyData.vacancynumber = SafeGetInteger(reader, "vakNr", 0)
          vacancyData.vacancybez = SafeGetString(reader, "Bezeichnung")
          vacancyData.vacancystate = SafeGetString(reader, "vState")
          vacancyData.street = SafeGetString(reader, "Strasse")
          vacancyData.Postcode = SafeGetString(reader, "PLZ")
          vacancyData.Location = SafeGetString(reader, "Ort")
          vacancyData.createdon = SafeGetDateTime(reader, "createdon", Nothing)

          result.Add(vacancyData)

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
  ''' Loads responsible person data.
  ''' </summary>
  ''' <param name="customerNumber">The customer number.</param>
  ''' <returns>Responsible person data.</returns>
  Public Function LoadResponsiblePersonData(ByVal customerNumber As Integer) As IEnumerable(Of ResponsiblePersonData) Implements IDatabaseAccess.LoadResponsiblePersonData

    Dim result As List(Of ResponsiblePersonData) = Nothing

    Dim sql As String

    sql = "Select ID, KDNr, RecNr, Position, Abteilung, Anrede, Vorname, Nachname, Telefon, Telefax, Natel, eMail,"
    sql &= "KDZState1, KDZState2, KDZHowKontakt From KD_Zustaendig Where KDNr = @customerNumber "
    sql = sql & "Order By Nachname Asc, Vorname Asc"


    ' Parameters
    Dim customerNumberParameter As New SqlClient.SqlParameter("customerNumber", customerNumber)
    Dim listOfParams As New List(Of SqlClient.SqlParameter)
    listOfParams.Add(customerNumberParameter)

    Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.Text)

    Try
      If Not reader Is Nothing Then
        result = New List(Of ResponsiblePersonData)

        While reader.Read
          Dim responsibleData As New ResponsiblePersonData
          responsibleData.ID = SafeGetInteger(reader, "ID", 0)
          responsibleData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
          responsibleData.RecordNumber = SafeGetInteger(reader, "RecNr", 0)
          responsibleData.Position = SafeGetString(reader, "Position")
          responsibleData.Department = SafeGetString(reader, "Abteilung")
          responsibleData.Salutation = SafeGetString(reader, "Anrede")
          responsibleData.Firstname = SafeGetString(reader, "Vorname")
          responsibleData.Lastname = SafeGetString(reader, "Nachname")
          responsibleData.LastNameFirstName = String.Format("{0}, {1}", responsibleData.Lastname, responsibleData.Firstname)

          responsibleData.Telephone = SafeGetString(reader, "Telefon")
          responsibleData.Telefax = SafeGetString(reader, "Telefax")
          responsibleData.MobilePhone = SafeGetString(reader, "Natel")
          responsibleData.Email = SafeGetString(reader, "eMail")

          responsibleData.fstate = SafeGetString(reader, "KDZState1")
          responsibleData.sstate = SafeGetString(reader, "KDZState2")
          responsibleData.howcontact = SafeGetString(reader, "KDZHowKontakt")

          result.Add(responsibleData)
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

	'''' <summary>
	'''' Loads advisor data.
	'''' </summary>
	'''' <returns>List of advisor data.</returns>
	'Function LoadAdvisorData() As IEnumerable(Of AdvisorData) Implements IDatabaseAccess.LoadAdvisorData

	'  Dim result As List(Of AdvisorData) = Nothing

	'  Dim sql As String

	'  sql = "[Get DispoName For Unterzeichner]"

	'  ' Parameters
	'  Dim listOfParams As New List(Of SqlClient.SqlParameter)
	'  listOfParams.Add(New SqlClient.SqlParameter("@USNachname", ReplaceMissing("%%", String.Empty)))
	'  listOfParams.Add(New SqlClient.SqlParameter("@Filiale1", ReplaceMissing("%%", String.Empty)))

	'  Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

	'  Try

	'    If (Not reader Is Nothing) Then

	'      result = New List(Of AdvisorData)

	'      While reader.Read()
	'        Dim advisorData As New AdvisorData

	'        advisorData.UserNumber = SafeGetInteger(reader, "USNR", 0)
	'        advisorData.Firstname = SafeGetString(reader, "Vorname")
	'        advisorData.Lastname = SafeGetString(reader, "Nachname")
	'        advisorData.Salutation = SafeGetString(reader, "Anrede")
	'        advisorData.KST = SafeGetString(reader, "KST")

	'        result.Add(advisorData)

	'      End While

	'    End If

	'  Catch e As Exception
	'    result = Nothing
	'    m_Logger.LogError(e.ToString())

	'  Finally
	'    CloseReader(reader)

	'  End Try

	'  Return result

	'End Function


	''' <summary>
	''' Opens a SqlClient.SqlDataReader object. 
	''' </summary>
	''' <param name="sql">The sql string.</param>
	''' <param name="parameters">The parameters collection.</param>
	''' <returns>The open reader or nothing in error case.</returns>
	''' <remarks>The reader is opened with the CloseConnection option, so when the reader is closed the underlying database connection will also be closed.</remarks>
	Private Function OpenReader(ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter), Optional ByVal commandType As System.Data.CommandType = CommandType.Text) As SqlClient.SqlDataReader

		Dim Conn As SqlClient.SqlConnection = New SqlClient.SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim reader As SqlClient.SqlDataReader

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = commandType

			If Not parameters Is Nothing Then
				For Each param In parameters
					cmd.Parameters.Add(param)

				Next
			End If

			reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

		Catch e As Exception
			m_Logger.LogError(e.ToString())
			Conn.Close()
			Conn.Dispose()
			reader = Nothing
		End Try

		Return reader
	End Function


	''' <summary>
	''' Executes a non query command.
	''' </summary>
	''' <param name="sql">The sql string.</param>
	''' <param name="parameters">The parameters.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Private Function ExecuteNonQuery(ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter), Optional ByVal commandType As System.Data.CommandType = CommandType.Text, Optional checkRowCount As Boolean = True) As Boolean
		Dim success As Boolean = True

		Dim Conn As SqlClient.SqlConnection = New SqlClient.SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = commandType

			If Not parameters Is Nothing Then
				For Each param In parameters
					cmd.Parameters.Add(param)

				Next
			End If

			If (checkRowCount) Then
				success = (cmd.ExecuteNonQuery() > 0)
			Else
				cmd.ExecuteNonQuery()
			End If

		Catch e As Exception
			success = False
			m_Logger.LogError(e.ToString())

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return success

	End Function

	''' <summary>
	''' Executes a scalar command.
	''' </summary>
	''' <param name="sql">The sql string.</param>
	''' <param name="parameters">The parameters.</param>
	''' <returns>Result of execure scalar operation.</returns>
	Private Function ExecuteScalar(ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter)) As Object
		Dim result As Object = Nothing

		Dim Conn As SqlClient.SqlConnection = New SqlClient.SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = CommandType.Text

			If Not parameters Is Nothing Then
				For Each param In parameters
					cmd.Parameters.Add(param)

				Next
			End If

			result = cmd.ExecuteScalar()

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return result

	End Function

  ''' <summary>
  ''' Closes a SqlClient.SqlDataReader
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  Private Sub CloseReader(ByVal reader As SqlClient.SqlDataReader)

    If Not reader Is Nothing Then

      Try
        reader.Close()
      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
      End Try

    End If

  End Sub

  ''' <summary>
  ''' Returns a string or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <param name="defaultValue">The default value.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Private Shared Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader.GetString(columnIndex)
    Else
      Return defaultValue
    End If
  End Function

  ''' <summary>
  ''' Returns a boolean or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <param name="defaultValue">The default value.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Private Shared Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader.GetBoolean(columnIndex)
    Else
      Return defaultValue
    End If
  End Function

  ''' <summary>
  ''' Returns an integer or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <param name="defaultValue">The default value.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Private Shared Function SafeGetInteger(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Integer?) As Integer?

    Try
      Dim columnIndex As Integer = reader.GetOrdinal(columnName)

      If (Not reader.IsDBNull(columnIndex)) Then
        Return reader.GetInt32(columnIndex)
      Else
        Return defaultValue
      End If

    Catch ex As Exception
      
    Finally

    End Try

  End Function

  ''' <summary>
  ''' Returns an long (int64) or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader"></param>
  ''' <param name="columnName"></param>
  ''' <param name="defaultValue"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Shared Function SafeGetLong(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Long?) As Long

    Try
      Dim columnIndex As Integer = reader.GetOrdinal(columnName)

      If (Not reader.IsDBNull(columnIndex)) Then
        Return reader.GetInt64(columnIndex) ' CInt(Val(reader(columnIndex))) 
      Else
        Return defaultValue
      End If

    Catch ex As Exception

    Finally

    End Try

  End Function

  ''' <summary>
  ''' Returns an short integer or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <param name="defaultValue">The default value.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Private Shared Function SafeGetShort(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Short?) As Short?

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader.GetInt16(columnIndex)
    Else
      Return defaultValue
    End If
  End Function

  ''' <summary>
  ''' Returns an byte or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <param name="defaultValue">The default value.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Private Shared Function SafeGetByte(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Byte?) As Byte?

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader.GetByte(columnIndex)
    Else
      Return defaultValue
    End If
  End Function

  ''' <summary>
  ''' Returns an decimal or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <param name="defaultValue">The default value.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Private Shared Function SafeGetDecimal(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Decimal?) As Decimal?

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader.GetDecimal(columnIndex)
    Else
      Return defaultValue
    End If
  End Function

  ''' <summary>
  ''' Returns an datetime or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <param name="defaultValue">The default value.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Private Shared Function SafeGetDateTime(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As DateTime?) As DateTime?

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader.GetDateTime(columnIndex)
    Else
      Return defaultValue
    End If
  End Function


  ''' <summary>
  ''' Returns an byte array or nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Private Shared Function SafeGetByteArray(ByVal reader As SqlDataReader, ByVal columnName As String) As Byte()

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader(columnIndex)
    Else
      Return Nothing
    End If
  End Function

  ''' <summary>
  ''' Replaces a missing object with another object.
  ''' </summary>
  ''' <param name="obj">The object.</param>
  ''' <param name="replacementObject">The replacement object.</param>
  ''' <returns>The object or the replacement object it the object is nothing.</returns>
  Private Shared Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

    If (obj Is Nothing) Then
      Return replacementObject
    Else
      Return obj
    End If

  End Function

  ''' <summary>
  ''' Maps from language to column name.
  ''' </summary>
  ''' <param name="language">The language.</param>
  ''' <returns>The language column name.</returns>
  Protected Shared Function MapLanguageToColumnName(ByVal language As Language) As String

    Select Case language
      Case language.German
        Return "Bez_D"
      Case language.Italian
        Return "Bez_I"
      Case language.French
        Return "Bez_F"
      Case language.English
        Return "Bez_E"
      Case Else
        Return "Bez_D"
    End Select

  End Function

  ''' <summary>
  ''' Maps a language to a short language code.
  ''' </summary>
  ''' <param name="language">The language.</param>
  ''' <returns>The short string code.</returns>
  Protected Shared Function MapLanguageToShortLanguageCode(ByVal language As Language) As String

    Select Case language
      Case language.German
        Return "D"
      Case language.Italian
        Return "I"
      Case language.French
        Return "F"
      Case language.English
        Return "E"
      Case Else
        Return "D"
    End Select

  End Function



#End Region

End Class
