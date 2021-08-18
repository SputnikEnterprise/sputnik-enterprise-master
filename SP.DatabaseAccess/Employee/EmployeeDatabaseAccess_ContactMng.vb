Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.Employee

Namespace Employee

  Partial Public Class EmployeeDatabaseAccess
    Inherits DatabaseAccessBase
    Implements IEmployeeDatabaseAccess

    ''' <summary>
    ''' Loads employee contact overview data by search criteria.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>List of employee contact data or nothing in error case.</returns>
    Public Function LoadEmployeeContactOverviewDataBySearchCriteria(ByVal employeeNumber As Integer, ByVal bHideTel As Boolean, ByVal bHideOffer As Boolean, ByVal bHideMail As Boolean, ByVal bHideSMS As Boolean, ByVal years As Integer()) As IEnumerable(Of EmployeeContactOverviewdata) Implements IEmployeeDatabaseAccess.LoadEmployeeContactOverviewDataBySearchCriteria

      Dim result As List(Of EmployeeContactOverviewdata) = Nothing

      Dim sql As String

      sql = "SELECT MAK.ID, MAK.MANr, MAK.RecNr, MAK.KontaktDate, MAK.Kontakte, MAK.KontaktDauer, MAK.KontaktWichtig, MAK.KontaktErledigt, MAK.CreatedFrom, MAK.KontaktDocID, MAK.KDKontaktRecID, "
      sql &= "(Select Top 1 min(KontaktDate) As MinKontaktDate FROM MA_Kontakte WHERE MANr = @maNumber) As MinKontaktDate, "
      sql &= "(Select Top 1 max(KontaktDate) As MaxKontaktDate FROM MA_Kontakte WHERE MANr = @maNumber) As MaxKontaktDate "
      sql = sql & "FROM MA_Kontakte MAK WHERE MANr = @maNumber "

      If Not bHideTel Then sql = sql & "And MAK.Kontakte Not Like '%wurde telefoniert" & "%' "
      If Not bHideOffer Then sql = sql & "And MAK.Kontakte Not Like '%Offerte geschickt" & "%' "
      If Not bHideMail Then sql = sql & "And MAK.Kontakte Not Like '%Mail-Nachricht gesendet" & "%' "
      If Not bHideSMS Then sql = sql & "And MAK.Kontakte Not Like '%SMS-Nachricht gesendet" & "%' "

      If Not years Is Nothing AndAlso years.Count > 0 Then
        sql = sql & "And year(MAK.KontaktDate) IN ("
        sql = sql & String.Join(", ", years)
        sql = sql & ") "
      End If
      sql = sql & " ORDER BY MAK.KontaktDate DESC"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("@maNumber", ReplaceMissing(employeeNumber, DBNull.Value)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of EmployeeContactOverviewdata)

          While reader.Read()
            Dim searchData As New EmployeeContactOverviewdata
            searchData.ID = SafeGetInteger(reader, "ID", 0)
            searchData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
            searchData.RecNr = SafeGetInteger(reader, "RecNr", Nothing)
            searchData.ContactDate = SafeGetDateTime(reader, "KontaktDate", Nothing)
            searchData.PersonOrSubject = SafeGetString(reader, "KontaktDauer")
            searchData.Description = SafeGetString(reader, "Kontakte")
            searchData.IsImportant = SafeGetBoolean(reader, "KontaktWichtig", False)
            searchData.IsCompleted = SafeGetBoolean(reader, "KontaktErledigt", False)
            searchData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
            searchData.DocumentID = SafeGetInteger(reader, "KontaktDocID", Nothing)
            searchData.KDKontactRecID = SafeGetInteger(reader, "KDKontaktRecID", Nothing)

            searchData.minContactDate = SafeGetDateTime(reader, "MinKontaktDate", Nothing)
            searchData.maxContactDate = SafeGetDateTime(reader, "MaxKontaktDate", Nothing)
            result.Add(searchData)

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
    ''' Loads employee contact (MA_Kontakte) distinct years.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>List of years or nothing in error case.</returns>
    Public Function LoadEmployeeContactDistinctYears(ByVal employeeNumber As Integer) As IEnumerable(Of Integer) Implements IEmployeeDatabaseAccess.LoadEmployeeContactDistinctYears
      Dim result As List(Of Integer) = Nothing

      Dim sql As String = String.Empty

      sql = sql & "SELECT ContactYear FROM "
      sql = sql & "(SELECT DISTINCT YEAR(KontaktDate) as ContactYear "
      sql = sql & " FROM MA_Kontakte "
      sql = sql & " WHERE(MANr = @employeeNumber And KontaktDate Is Not NULL)"
      sql = sql & " UNION SELECT YEAR(GetDate()) as ContactYear)AS ContactYears "
      sql = sql & "ORDER BY ContactYear desc"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", employeeNumber))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of Integer)

          While reader.Read
            Dim year As Integer = SafeGetInteger(reader, "ContactYear", 0)
            result.Add(year)
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
    ''' Loads an employee contact.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="recordNumber">The contact data record number.</param>
    ''' <returns>Contact of employee.</returns>
    Public Function LoadEmployeeContact(ByVal employeeNumber As Integer, ByVal recordNumber As Integer) As EmployeeContactData Implements IEmployeeDatabaseAccess.LoadEmployeeContact

      Dim result As EmployeeContactData = Nothing

      Dim sql As String

      sql = "SELECT Kontakte.*, KDKontakte.ID as KDKontakte_ID, KDKontakte.KDNr as KDKontakte_KDNr, KDKontakte.RecNr as KDKontakte_RecNr " &
            "FROM MA_Kontakte Kontakte LEFT JOIN KD_KontaktTotal KDKontakte ON Kontakte.KDKontaktRecID = KDKontakte.ID " +
            " Where Kontakte.MANr = @maNumber AND Kontakte.RecNr = @recordNumber "

      ' Parameters
      Dim employeeNumberParameter As New SqlClient.SqlParameter("maNumber", employeeNumber)
      Dim recordNumberParameter As New SqlClient.SqlParameter("recordNumber", recordNumber)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(employeeNumberParameter)
      listOfParams.Add(recordNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            result = New EmployeeContactData
            result.ID = SafeGetInteger(reader, "ID", 0)
            result.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
            result.ContactsString = SafeGetString(reader, "Kontakte")
            result.RecordNumber = SafeGetInteger(reader, "RecNr", Nothing)
            result.ContactType1 = SafeGetString(reader, "KontaktType1")
            result.ContactType2 = SafeGetShort(reader, "KontaktType2", Nothing)
            result.ContactDate = SafeGetDateTime(reader, "KontaktDate", Nothing)
            result.ContactPeriodString = SafeGetString(reader, "KontaktDauer")
            result.ContactImportant = SafeGetBoolean(reader, "KontaktWichtig", False)
            result.ContactFinished = SafeGetBoolean(reader, "KontaktErledigt", False)
            result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
            result.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
            result.ChangedFrom = SafeGetString(reader, "ChangedFrom")
            result.ProposeNr = SafeGetInteger(reader, "Proposenr", Nothing)
            result.VacancyNumber = SafeGetInteger(reader, "VakNr", Nothing)
            result.OfNumber = SafeGetInteger(reader, "OfNr", Nothing)
            result.Mail_ID = SafeGetInteger(reader, "Mail_ID", Nothing)
            result.TaskRecNr = SafeGetInteger(reader, "TaskRecNr", Nothing)
            result.UsNr = SafeGetInteger(reader, "USNr", Nothing)
            result.ESNr = SafeGetInteger(reader, "ESNr", Nothing)
            result.CustomerNumber = SafeGetInteger(reader, "KDNr", Nothing)
            result.CustomerContactRecId = SafeGetInteger(reader, "KDKontakte_ID", Nothing)
            result.CustomerContactKDNr = SafeGetInteger(reader, "KDKontakte_KDNr", Nothing)
            result.CustomerContactRecNr = SafeGetInteger(reader, "KDKontakte_RecNr", Nothing)
            result.KontaktDocID = SafeGetInteger(reader, "KontaktDocID", Nothing)

          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        result = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return result

    End Function

    ''' <summary>
    ''' Adds a employee contact.
    ''' </summary>
    ''' <param name="contactData">The contact data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddEmployeeContact(ByVal contactData As EmployeeContactData) As Boolean Implements IEmployeeDatabaseAccess.AddEmployeeContact

      Dim success = True

      Dim sql As String

			sql = "[Create New Employee Contact]"

			' Parameters

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(contactData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(contactData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Kontakte", ReplaceMissing(contactData.ContactsString, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktType1", ReplaceMissing(contactData.ContactType1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktType2", ReplaceMissing(contactData.ContactType2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktDate", ReplaceMissing(contactData.ContactDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktDauer", ReplaceMissing(contactData.ContactPeriodString, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktWichtig", ReplaceMissing(contactData.ContactImportant, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktErledigt", ReplaceMissing(contactData.ContactFinished, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ProposeNr", ReplaceMissing(contactData.ProposeNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(contactData.VacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OfNr", ReplaceMissing(contactData.OfNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Mail_ID", ReplaceMissing(contactData.Mail_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaskRecNr", ReplaceMissing(contactData.TaskRecNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(contactData.UsNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(contactData.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDKontaktRecID", ReplaceMissing(contactData.CustomerContactRecId, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktDocID", ReplaceMissing(contactData.KontaktDocID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedOn", ReplaceMissing(contactData.CreatedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(contactData.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedUserNumber", ReplaceMissing(contactData.CreatedUserNumber, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewContactID", SqlDbType.Int)
      newIdParameter.Direction = ParameterDirection.Output
      listOfParams.Add(newIdParameter)

      Dim recNrParameter = New SqlClient.SqlParameter("@RecNr ", SqlDbType.Int)
      recNrParameter.Direction = ParameterDirection.Output
      listOfParams.Add(recNrParameter)

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

      If Not newIdParameter.Value Is Nothing AndAlso
          Not recNrParameter.Value Is Nothing Then
        contactData.ID = CType(newIdParameter.Value, Integer)
        contactData.RecordNumber = CType(recNrParameter.Value, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Updates employee contact data.
    ''' </summary>
    ''' <param name="contactData">The contact data.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Public Function UpdateEmployeeContact(ByVal contactData As EmployeeContactData) As Boolean Implements IEmployeeDatabaseAccess.UpdateEmployeeContact

      Dim success = True

      Dim sql As String

			'sql = "UPDATE MA_Kontakte SET "
			'sql = sql & "MANr = @maNumber, "
			'sql = sql & "Kontakte = @contactsString, "
			'sql = sql & "RecNr = @recordNumber, "
			'sql = sql & "KontaktType1 = @contactType1, "
			'sql = sql & "KontaktType2 = @contactType2, "
			'sql = sql & "KontaktDate = @contactDate, "
			'sql = sql & "KontaktDauer = @contactDurationString, "
			'sql = sql & "KontaktWichtig = @contactImportant, "
			'sql = sql & "KontaktErledigt = @contactFinished, "
			'sql = sql & "CreatedOn = @createdOn, "
			'sql = sql & "CreatedFrom = @createdFrom, "
			'sql = sql & "ChangedOn = @changedOn, "
			'sql = sql & "ChangedFrom = @changedFrom, "
			'sql = sql & "ProposeNr = @proposeNr, "
			'sql = sql & "VakNr = @vacanyNr, "
			'sql = sql & "OfNr = @ofNr, "
			'sql = sql & "Mail_ID = @mailId, "
			'sql = sql & "TaskRecNr = @taskRecNr, "
			'sql = sql & "USNr = @usnr, "
			'sql = sql & "EsNr = @esnr, "
			'sql = sql & "KDNr = @customerNumber, "
			'sql = sql & "KDKontaktRecID = @kdKontaktRecID, "
			'sql = sql & "KontaktDocID = @kontaktDocID "

			'sql = sql & "WHERE ID = @id "

			sql = "[Update Assigned Employee Contact]"
			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(contactData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("recordNumber", ReplaceMissing(contactData.RecordNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(contactData.CustomerNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Kontakte", ReplaceMissing(contactData.ContactsString, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktType1", ReplaceMissing(contactData.ContactType1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktType2", ReplaceMissing(contactData.ContactType2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktDate", ReplaceMissing(contactData.ContactDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktDauer", ReplaceMissing(contactData.ContactPeriodString, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktWichtig", ReplaceMissing(contactData.ContactImportant, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktErledigt", ReplaceMissing(contactData.ContactFinished, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ProposeNr", ReplaceMissing(contactData.ProposeNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(contactData.VacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OfNr", ReplaceMissing(contactData.OfNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Mail_ID", ReplaceMissing(contactData.Mail_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TaskRecNr", ReplaceMissing(contactData.TaskRecNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("USNr", ReplaceMissing(contactData.UsNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(contactData.ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KDKontaktRecID", ReplaceMissing(contactData.CustomerContactRecId, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KontaktDocID", ReplaceMissing(contactData.KontaktDocID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedOn", ReplaceMissing(contactData.ChangedOn, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedFrom", ReplaceMissing(contactData.ChangedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ChangedUserNumber", ReplaceMissing(contactData.ChangedUserNumber, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("id", contactData.ID))









			'listOfParams.Add(New SqlClient.SqlParameter("maNumber", ReplaceMissing(contactData.EmployeeNumber, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("contactsString", ReplaceMissing(contactData.ContactsString, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("recordNumber", ReplaceMissing(contactData.RecordNumber, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("contactType1", ReplaceMissing(contactData.ContactType1, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("contactType2", ReplaceMissing(contactData.ContactType2, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("contactDate", ReplaceMissing(contactData.ContactDate, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("contactDurationString", ReplaceMissing(contactData.ContactPeriodString, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("contactImportant", ReplaceMissing(contactData.ContactImportant, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("contactFinished", ReplaceMissing(contactData.ContactFinished, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("createdOn", ReplaceMissing(contactData.CreatedOn, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(contactData.CreatedFrom, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("changedOn", ReplaceMissing(contactData.ChangedOn, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("changedFrom", ReplaceMissing(contactData.ChangedFrom, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("proposeNr", ReplaceMissing(contactData.ProposeNr, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("vacanyNr", ReplaceMissing(contactData.VacancyNumber, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("ofNr", ReplaceMissing(contactData.OfNumber, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("mailId", ReplaceMissing(contactData.Mail_ID, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("taskRecNr", ReplaceMissing(contactData.TaskRecNr, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("usnr", ReplaceMissing(contactData.UsNr, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("esnr", ReplaceMissing(contactData.ESNr, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(contactData.CustomerNumber, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("kdKontaktRecID", ReplaceMissing(contactData.CustomerContactRecId, DBNull.Value)))
			'   listOfParams.Add(New SqlClient.SqlParameter("kontaktDocID", ReplaceMissing(contactData.KontaktDocID, DBNull.Value)))

			'   listOfParams.Add(New SqlClient.SqlParameter("id", contactData.ID))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success

    End Function

		''' <summary>
		''' Deletes a employee contact.
		''' </summary>
		''' <param name="id">The database id of the contact.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Function DeleteEmployeeContact(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteEmployeeContact

      Dim success = True

      Dim sql As String

      sql = "DELETE FROM MA_Kontakte WHERE ID = @id"

      Dim idParameter As New SqlClient.SqlParameter("id", id)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function


    ''' <summary>
    ''' Loads contact document data.
    ''' </summary>
    ''' <param name="contactId">The contact id.</param>
    ''' <param name="includeFileBytes">Boolean flag indicating if file bytes should be included.</param>
    ''' <returns>Document data.</returns>
    Public Function LoadContactDocumentData(ByVal contactId As Integer, ByVal includeFileBytes As Boolean) As ContactDoc Implements IEmployeeDatabaseAccess.LoadContactDocumentData

      Dim contactDocData As ContactDoc = Nothing

      Dim sql As String

			sql = "SELECT ID, KontaktID, CreatedOn, CreatedFrom, FileExtension "

      If includeFileBytes Then
        sql = sql & ", DocScan "
      End If

      sql = sql & "FROM Kontakt_Doc Where ID = @id"

      ' Parameters
      Dim idParameter As New SqlClient.SqlParameter("id", contactId)
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(idParameter)
      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            contactDocData = New ContactDoc
            contactDocData.ID = SafeGetInteger(reader, "ID", 0)

            If includeFileBytes Then
              contactDocData.FileBytes = SafeGetByteArray(reader, "DocScan")
            End If
						contactDocData.FileExtension = SafeGetString(reader, "FileExtension")

            contactDocData.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
            contactDocData.CreatedFrom = SafeGetString(reader, "CreatedFrom")

          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        contactDocData = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return contactDocData

    End Function

    ''' <summary>
    ''' Adds a contact document.
    ''' </summary>
    ''' <param name="contactDocument">The contact document.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function AddContactDocument(ByVal contactDocument As ContactDoc) As Boolean Implements IEmployeeDatabaseAccess.AddContactDocument

      Dim success = True

      Dim sql As String

			sql = "INSERT INTO Kontakt_Doc (KontaktID, DocScan, FileExtension, CreatedOn, CreatedFrom, IsMA) " &
						" VALUES(@contactId, @fileBytes, @FileExtension, @createdOn, @createdFrom, 1); SELECT @@IDENTITY"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("contactId", DBNull.Value))
      listOfParams.Add(New SqlClient.SqlParameter("fileBytes", ReplaceMissing(contactDocument.FileBytes, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FileExtension", ReplaceMissing(contactDocument.FileExtension, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("createdOn", ReplaceMissing(contactDocument.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(contactDocument.CreatedFrom, DBNull.Value)))

      Dim id = ExecuteScalar(sql, listOfParams)

      If Not id Is Nothing Then
        contactDocument.ID = CType(id, Integer)
      Else
        success = False
      End If

      Return success

    End Function

    ''' <summary>
    ''' Updates contact document data.
    ''' </summary>
    ''' <param name="contactDocument">The contact document data.</param>
    ''' <param name="ignoreFileBytes">Boolean flag indiciating if file bytes should be ignored.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function UpdateContactDocumentData(ByVal contactDocument As ContactDoc, ByVal ignoreFileBytes As Boolean) As Boolean Implements IEmployeeDatabaseAccess.UpdateContactDocumentData

      Dim success = True

      Dim sql As String

      sql = "UPDATE Kontakt_Doc SET "
      sql = sql & "CreatedOn = @createdOn, "
			sql = sql & "CreatedFrom = @createdFrom, "
			sql = sql & "FileExtension = @FileExtension "

      If Not ignoreFileBytes Then
        sql = sql & ", DocScan = @fileBytes "
      End If

      sql = sql & "WHERE ID = @id"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("createdOn", ReplaceMissing(contactDocument.CreatedOn, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("createdFrom", ReplaceMissing(contactDocument.CreatedFrom, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FileExtension", ReplaceMissing(contactDocument.FileExtension, DBNull.Value)))

      If Not ignoreFileBytes Then
        listOfParams.Add(New SqlClient.SqlParameter("fileBytes", ReplaceMissing(contactDocument.FileBytes, DBNull.Value)))
      End If

      listOfParams.Add(New SqlClient.SqlParameter("id", contactDocument.ID))

      success = ExecuteNonQuery(sql, listOfParams)

      Return success

    End Function

    ''' <summary>
    ''' Deletes a contact document.
    ''' </summary>
    ''' <param name="id">The id of the document.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function DeleteContactDocument(ByVal id As Integer) As Boolean Implements IEmployeeDatabaseAccess.DeleteContactDocument
      Dim success = True

      Dim sql As String

      sql = "DELETE FROM Kontakt_Doc WHERE ID = @id"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))

      success = ExecuteNonQuery(sql, listOfParams)

      listOfParams = New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))
      success = success AndAlso ExecuteNonQuery("UPDATE KD_KontaktTotal SET KontaktDocID = NULL WHERE KontaktDocID = @id", listOfParams, CommandType.Text, False)

      listOfParams = New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("id", id))
      success = success AndAlso ExecuteNonQuery("UPDATE MA_Kontakte SET KontaktDocID = NULL WHERE KontaktDocID = @id", listOfParams, CommandType.Text, False)

      Return success

    End Function

    ''' <summary>
    ''' Load customer data for job contact management.
    ''' </summary>
    ''' <returns>List of customer data or nothing in error case.</returns>
    Function LoadCustomerDataForContactMng() As IEnumerable(Of CustomerDataForContactMng) Implements IEmployeeDatabaseAccess.LoadCustomerDataForContactMng
      Dim result As List(Of CustomerDataForContactMng) = Nothing

      Dim sql As String

      sql = "SELECT KDNr, Firma1, Strasse, PLZ, Ort FROM Kunden ORDER BY Firma1 Asc"

      ' Parameters
      Dim listOfParams As New List(Of SqlClient.SqlParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of CustomerDataForContactMng)

          While reader.Read()
            Dim customerData As New CustomerDataForContactMng
            customerData.CustomerNumber = SafeGetInteger(reader, "KDNr", 0)
            customerData.Company1 = SafeGetString(reader, "Firma1", Nothing)
            customerData.Street = SafeGetString(reader, "Strasse", Nothing)
            customerData.Postcode = SafeGetString(reader, "PLZ", Nothing)
            customerData.Location = SafeGetString(reader, "Ort", Nothing)

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
    ''' Loads vacancy data.
    ''' </summary>
    ''' <param name="customerNumber">The customer number.</param>
    ''' <returns>List of vacancy data.</returns>
    Public Function LoadVacancyData(Optional ByVal customerNumber As Integer? = Nothing) As IEnumerable(Of VacancyDataForContactMng) Implements IEmployeeDatabaseAccess.LoadVacancyDataForContactMng
      Dim result As List(Of VacancyDataForContactMng) = Nothing

			Dim sql As String

			sql = "SELECT V.ID, V.VakNr, V.Bezeichnung, V.CreatedOn, V.CreatedFrom, KD.Firma1, "
			sql &= "("
			sql &= "CASE  "
			sql &= " When isnumeric(V.VakState) = 1 then (Select Top 1 bez_d From tbl_base_VakState where RecValue = V.VakState) "
			sql &= " ELSE VakState "
			sql &= " End "
			sql &= ") as Vakstate "

			sql &= " FROM Vakanzen V Left Join Kunden KD On V.KDNr = KD.KDNR "
			sql &= " WHERE (@kdNr IS NULL OR V.KDNr = @kdNr) "
			sql &= " ORDER BY V.CreatedOn Desc"

      Dim customerNumberParameter As New SqlClient.SqlParameter("kdNr", ReplaceMissing(customerNumber, DBNull.Value))
      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(customerNumberParameter)

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of VacancyDataForContactMng)

          While reader.Read

            Dim vacancyData = New VacancyDataForContactMng()
            vacancyData.ID = SafeGetInteger(reader, "ID", 0)
            vacancyData.VacancyNumber = SafeGetInteger(reader, "VakNr", Nothing)
            vacancyData.Description = SafeGetString(reader, "Bezeichnung")

						vacancyData.CreatedOn = SafeGetDateTime(reader, "createdon", Nothing)
						vacancyData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						vacancyData.VakState = SafeGetString(reader, "VakState")

						vacancyData.Customername = SafeGetString(reader, "Firma1")

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
    ''' Loads propose data.
    ''' </summary>
    ''' <returns>List of propose data.</returns>
    Public Function LoadProposeData(Optional ByVal employeeNumber As Integer? = Nothing, Optional ByVal customerNumber As Integer? = Nothing) As IEnumerable(Of ProposeDataForContactMng) Implements IEmployeeDatabaseAccess.LoadProposeDataForContactMng
      Dim result As List(Of ProposeDataForContactMng) = Nothing

      Dim sql As String = String.Empty

			sql = sql & "SELECT P.ID, P.ProposeNr, P.MANr, P.Bezeichnung, P.CreatedOn, P.CreatedFrom, P.P_State, KD.Firma1 "
			sql = sql & "FROM Propose P Left Join Kunden KD On P.KDNr = KD.KDNr "
			sql = sql & "WHERE (@maNumber IS NULL OR P.MANr = @maNumber) AND (@kdNumber IS NULL OR P.KDNR = @kdNumber) "
			sql = sql & "ORDER BY P.CreatedOn Desc"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("maNumber", ReplaceMissing(employeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("kdNumber", ReplaceMissing(customerNumber, DBNull.Value)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ProposeDataForContactMng)

          While reader.Read

            Dim proposeData = New ProposeDataForContactMng()
            proposeData.ID = SafeGetInteger(reader, "ID", 0)
            proposeData.ProposeNumber = SafeGetInteger(reader, "ProposeNr", Nothing)
            proposeData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
            proposeData.Description = SafeGetString(reader, "Bezeichnung")

						proposeData.CreatedOn = SafeGetDateTime(reader, "createdon", Nothing)
						proposeData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						proposeData.P_State = SafeGetString(reader, "P_State")

						proposeData.Customername = SafeGetString(reader, "Firma1")

            result.Add(proposeData)

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
    ''' Loads ES (Einsatz) data.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <returns>List of ES data.</returns>
    Public Function LoadESData(Optional ByVal employeeNumber As Integer? = Nothing, Optional ByVal customerNumber As Integer? = Nothing) As IEnumerable(Of ESDataForContactMng) Implements IEmployeeDatabaseAccess.LoadESDataForContactMng
      Dim result As List(Of ESDataForContactMng) = Nothing

      Dim sql As String = String.Empty

			sql = sql & "SELECT ES.ID, ES.ESNR, ES.MANr, ES.ES_Als, ES.ES_Ab, ES.ES_Ende, KD.Firma1 "
			sql = sql & "FROM ES Left Join Kunden KD On ES.KDNr = KD.KDNr "
			sql = sql & "WHERE (@maNumber IS NULL OR ES.MANr = @maNumber) AND (@kdNumber IS NULL OR ES.KDNR = @kdNumber) "
			sql = sql & "ORDER BY ES.ES_Ab Desc"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("maNumber", ReplaceMissing(employeeNumber, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("kdNumber", ReplaceMissing(customerNumber, DBNull.Value)))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If (Not reader Is Nothing) Then

          result = New List(Of ESDataForContactMng)

          While reader.Read

            Dim esData = New ESDataForContactMng()
            esData.ID = SafeGetInteger(reader, "ID", 0)
            esData.ESNumber = SafeGetInteger(reader, "ESNR", Nothing)
            esData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
            esData.ES_As = SafeGetString(reader, "ES_Als")
            esData.ES_FromDate = SafeGetDateTime(reader, "ES_Ab", Nothing)
            esData.ES_ToDate = SafeGetDateTime(reader, "ES_Ende", Nothing)

						esData.customername = SafeGetString(reader, "Firma1")

            result.Add(esData)

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
    ''' Loads employee dependent customer contact data.
    ''' </summary>
    Public Function LoadEmployeeDependentCustomerContactData(ByVal maRecID As Integer) As EmployeeDependentCustomerContactData Implements IEmployeeDatabaseAccess.LoadEmployeeDependentCustomerContactData


      Dim employeeDependentCustomerContact As EmployeeDependentCustomerContactData = Nothing

      Dim sql As String = String.Empty

      sql = sql & "SELECT K.ID AS KDKontaktID, K.RecNr AS KDKontaktRecNr, K.KDNr AS KDNr "
      sql = sql & "FROM KD_KontaktTotal  K "
      sql = sql & "WHERE MAKontaktRecID = @maRecID "

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("maRecID", maRecID))

      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

      Try

        If Not reader Is Nothing Then

          If reader.Read Then
            employeeDependentCustomerContact = New EmployeeDependentCustomerContactData
            employeeDependentCustomerContact.KDKontaktID = SafeGetInteger(reader, "KDKontaktID", 0)
            employeeDependentCustomerContact.KDKontaktRecNr = SafeGetInteger(reader, "KDKontaktRecNr", 0)
            employeeDependentCustomerContact.KDNr = SafeGetInteger(reader, "KDNr", 0)

          End If

        End If

      Catch ex As Exception
        m_Logger.LogError(ex.ToString())
        employeeDependentCustomerContact = Nothing
      Finally
        CloseReader(reader)
      End Try

      Return employeeDependentCustomerContact

      Return Nothing

    End Function


		''' <summary>
		''' Loads employee contact overview data by search criteria for Propose.
		''' </summary>
		''' <param name="employeeNumber">The employee number.</param>
		''' <returns>List of employee contact data or nothing in error case.</returns>
		Public Function LoadEmployeeContactOverviewDataForPropose(ByVal employeeNumber As Integer, ByVal proposeNumber As Integer,
																															 ByVal bHideTel As Boolean, ByVal bHideOffer As Boolean, ByVal bHideMail As Boolean, ByVal bHideSMS As Boolean,
																															 ByVal years As Integer()) As IEnumerable(Of EmployeeContactOverviewdata) Implements IEmployeeDatabaseAccess.LoadEmployeeContactOverviewDataForPropose

			Dim result As List(Of EmployeeContactOverviewdata) = Nothing

			Dim sql As String

			sql = "SELECT MAK.ID, MAK.MANr, MAK.RecNr, MAK.KontaktDate, MAK.Kontakte, MAK.KontaktDauer, MAK.KontaktWichtig, MAK.KontaktErledigt, MAK.CreatedFrom, MAK.KontaktDocID, MAK.KDKontaktRecID, "
			sql &= "(Select Top 1 min(KontaktDate) As MinKontaktDate FROM MA_Kontakte WHERE MANr = @maNumber) As MinKontaktDate, "
			sql &= "(Select Top 1 max(KontaktDate) As MaxKontaktDate FROM MA_Kontakte WHERE MANr = @maNumber) As MaxKontaktDate "
			sql = sql & "FROM MA_Kontakte MAK WHERE MAK.MANr = @maNumber And MAK.ProposeNr = @proposeNumber "

			If Not bHideTel Then sql = sql & "And MAK.Kontakte Not Like '%wurde telefoniert" & "%' "
			If Not bHideOffer Then sql = sql & "And MAK.Kontakte Not Like '%Offerte geschickt" & "%' "
			If Not bHideMail Then sql = sql & "And MAK.Kontakte Not Like '%Mail-Nachricht gesendet" & "%' "
			If Not bHideSMS Then sql = sql & "And MAK.Kontakte Not Like '%SMS-Nachricht gesendet" & "%' "

			If Not years Is Nothing AndAlso years.Count > 0 Then
				sql = sql & "And year(MAK.KontaktDate) IN ("
				sql = sql & String.Join(", ", years)
				sql = sql & ") "
			End If
			sql = sql & " ORDER BY MAK.KontaktDate DESC"

			' Parameters
			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@maNumber", ReplaceMissing(employeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@proposeNumber", ReplaceMissing(proposeNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams)

			Try

				If (Not reader Is Nothing) Then

					result = New List(Of EmployeeContactOverviewdata)

					While reader.Read()
						Dim searchData As New EmployeeContactOverviewdata
						searchData.ID = SafeGetInteger(reader, "ID", 0)
						searchData.EmployeeNumber = SafeGetInteger(reader, "MANr", Nothing)
						searchData.RecNr = SafeGetInteger(reader, "RecNr", Nothing)
						searchData.ContactDate = SafeGetDateTime(reader, "KontaktDate", Nothing)
						searchData.PersonOrSubject = SafeGetString(reader, "KontaktDauer")
						searchData.Description = SafeGetString(reader, "Kontakte")
						searchData.IsImportant = SafeGetBoolean(reader, "KontaktWichtig", False)
						searchData.IsCompleted = SafeGetBoolean(reader, "KontaktErledigt", False)
						searchData.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						searchData.DocumentID = SafeGetInteger(reader, "KontaktDocID", Nothing)
						searchData.KDKontactRecID = SafeGetInteger(reader, "KDKontaktRecID", Nothing)

						searchData.minContactDate = SafeGetDateTime(reader, "MinKontaktDate", Nothing)
						searchData.maxContactDate = SafeGetDateTime(reader, "MaxKontaktDate", Nothing)
						result.Add(searchData)

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


  End Class

End Namespace