
Imports NLog
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports System.Xml.XPath
Imports System.IO
Imports System.Data.SqlClient
Imports SPProgUtility.Mandanten


Namespace MainUtilities

  Public Class Utilities

    Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

    Private m_reg As ClsDivReg


    Public ReadOnly Property TransFilename As String
      Get
        Return "TranslationData.xml"
      End Get
    End Property

    Function AddDirSep(ByVal strPathName As String) As String

      Const gstrSEP_URLDIR As String = "/"                      ' Separator for dividing directories in URL addresses.
      Const gstrSEP_DIR As String = "\"                         ' Directory separator character

      If Right(Trim(strPathName), Len(gstrSEP_URLDIR)) <> gstrSEP_URLDIR And _
                Right(Trim(strPathName), Len(gstrSEP_DIR)) <> gstrSEP_DIR Then
        strPathName = RTrim$(strPathName) & gstrSEP_DIR
      End If
      AddDirSep = strPathName

    End Function


#Region "Lokale Verzeichnisse ermitteln..."

    ' Without Backslash!!!
    Public Function GetMyDocumentsPath() As String
      Return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
    End Function

    ''' <summary>
    ''' Benutzerverzeichnis "\"
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetMyDocumentsPathWithBackSlash() As String
      Return AddDirSep(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
    End Function

    ''' <summary>
    ''' Verzeichnis für Lokale Dateien vom Sputnik (GetMyDocumentsPathWithBackSlash + "Sputnik\")
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSHomeFolder() As String
      Dim strValue As String = GetMyDocumentsPathWithBackSlash() & "Sputnik\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' ("Sputnik\Temp\")
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSTempFolder() As String
      Dim strValue As String = GetSpSHomeFolder() & "Temp\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' Die Dateien in diesem Verzeichnis können jeder zeitputnkt gelöscht werden... 
    ''' ("GetPersonalFolder + Sputnik\Allowed2Delete\")
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpS2DeleteHomeFolder() As String
      Dim strValue As String = GetSpSHomeFolder() & "Allowed2Delete\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' Die Dateien in diesem Verzeichnis können jeder zeitputnkt gelöscht werden... 
    ''' ("GetPersonalFolder + Sputnik\Allowed2Delete\Bilder\")
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSPictureHomeFolder() As String
      Dim strValue As String = GetSpS2DeleteHomeFolder() & "Bilder\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' Pfad für Lokale Drucker (P) Einstellungsdateien. Wenn nicht existiert dann wird der Pfad angelegt.
    ''' (GetPersonalFolder +  "Sputnik\Printerfiles\)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSPrinterHomeFolder() As String
      Dim strValue As String = GetSpSHomeFolder() & "Printerfiles\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' (GetPersonalFolder +  "Sputnik\Temp\Kandidat\)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSMAHomeFolder() As String
      Dim strValue As String = GetSpSTempFolder() & "Kandidat\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' (GetPersonalFolder +  "Sputnik\Temp\Kunde\)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSKDHomeFolder() As String
      Dim strValue As String = GetSpSTempFolder() & "Kunde\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' (GetPersonalFolder +  "Sputnik\Temp\Offer\)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSOfferHomeFolder() As String
      Dim strValue As String = GetSpSTempFolder() & "Offer\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' (GetPersonalFolder +  "Sputnik\Temp\RP\)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSRPHomeFolder() As String
      Dim strValue As String = GetSpSTempFolder() & "RP\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' (GetPersonalFolder +  "Sputnik\Temp\RE\)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSREHomeFolder() As String
      Dim strValue As String = GetSpSTempFolder() & "RE\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' (GetPersonalFolder +  "Sputnik\Temp\LO\)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSLOHomeFolder() As String
      Dim strValue As String = GetSpSTempFolder() & "LO\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' (GetPersonalFolder +  "Sputnik\Temp\NLA\)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSNLAHomeFolder() As String
      Dim strValue As String = GetSpSTempFolder() & "NLA\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' (GetPersonalFolder +  "Sputnik\Temp\ES\)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSpSESHomeFolder() As String
      Dim strValue As String = GetSpSTempFolder() & "ES\"
      If Not Directory.Exists(strValue) Then Directory.CreateDirectory(strValue)

      Return strValue
    End Function

    ''' <summary>
    ''' ist gemäss Einstellungen->lokale Einstellungen(PrintFileSaveIn)
    ''' Wenn leer ist dann nimmt den (GetPersonalFolder +  "Sputnik\Printerfiles\)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetPrinterHomeFolder() As String
      m_reg = New ClsDivReg
      Dim strValue As String = AddDirSep(m_reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "PrintFileSaveIn"))
			If strValue = String.Empty OrElse strValue.ToUpper = "%UserProfile%".ToUpper OrElse Not Directory.Exists(strValue) Then strValue = GetMyDocumentsPathWithBackSlash()

			Return strValue
    End Function


#End Region





#Region "Databaseaccess..."

    ''' <summary>
    ''' Opens a SqlClient.SqlDataReader object. 
    ''' </summary>
    ''' <param name="sql">The sql string.</param>
    ''' <param name="parameters">The parameters collection.</param>
    ''' <returns>The open reader or nothing in error case.</returns>
    ''' <remarks>The reader is opened with the CloseConnection option, so when the reader is closed the underlying database connection will also be closed.</remarks>
    Function OpenReader(ByVal DbConnString As String, ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter), Optional ByVal commandType As System.Data.CommandType = CommandType.Text) As SqlClient.SqlDataReader

      Dim Conn As SqlClient.SqlConnection = New SqlClient.SqlConnection(DbConnString)
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
				Dim msg As String = String.Format("{0} >>> {1}", sql, String.Join(",", parameters))
				logger.Error(String.Format("{1}{0}{2}{0}{3}", vbNewLine, DbConnString, msg, e.ToString()))

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
    Function ExecuteNonQuery(ByVal DbConnString As String, ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter), Optional ByVal commandType As System.Data.CommandType = CommandType.Text, Optional checkRowCount As Boolean = False) As Boolean
      Dim success As Boolean = True

      Dim Conn As SqlClient.SqlConnection = New SqlClient.SqlConnection(DbConnString)

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
				Dim msg As String = sql
				logger.Error(String.Format("{1}{0}{2}{0}{3}", vbNewLine, DbConnString, msg, e.ToString()))

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
    Function ExecuteScalar(ByVal DbConnString As String, ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter)) As Object
      Dim result As Object = Nothing

      Dim Conn As SqlClient.SqlConnection = New SqlClient.SqlConnection(DbConnString)

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

				Dim msg As String = sql
				logger.Error(String.Format("{1}{0}{2}{0}{3}", vbNewLine, DbConnString, msg, e.ToString()))

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
    Sub CloseReader(ByVal reader As SqlClient.SqlDataReader)

      If Not reader Is Nothing Then

        Try
          reader.Close()
        Catch ex As Exception
          logger.Error(ex.ToString())
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
    Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

      Dim columnIndex As Integer = reader.GetOrdinal(columnName)

      If (Not reader.IsDBNull(columnIndex)) Then
        'Return reader.GetString(columnIndex)
        Return CStr(reader(columnIndex))
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
    Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

      Dim columnIndex As Integer = reader.GetOrdinal(columnName)

      If (Not reader.IsDBNull(columnIndex)) Then
        'Return reader.GetBoolean(columnIndex)
        Return CBool(reader(columnIndex))
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
    Function SafeGetInteger(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Integer?) As Integer?

      Dim columnIndex As Integer = reader.GetOrdinal(columnName)

      If (Not reader.IsDBNull(columnIndex)) Then
        'Return reader.GetInt32(columnIndex)
        Return CInt(reader(columnIndex))
      Else
        Return defaultValue
      End If
    End Function

    ''' <summary>
    ''' Returns an short integer or the default value if its nothing.
    ''' </summary>
    ''' <param name="reader">The reader.</param>
    ''' <param name="columnName">The column name.</param>
    ''' <param name="defaultValue">The default value.</param>
    ''' <returns>Value or default value if the value is nothing</returns>
    Function SafeGetShort(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Short?) As Short?

      Dim columnIndex As Integer = reader.GetOrdinal(columnName)

      If (Not reader.IsDBNull(columnIndex)) Then
        'Return reader.GetInt16(columnIndex)
        Return CShort(reader(columnIndex))
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
    Protected Shared Function SafeGetByte(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Byte?) As Byte?

      Dim columnIndex As Integer = reader.GetOrdinal(columnName)

      If (Not reader.IsDBNull(columnIndex)) Then
        'Return reader.GetByte(columnIndex)
        Return CByte(reader(columnIndex))
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
    Function SafeGetDecimal(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Decimal?) As Decimal?

      Dim columnIndex As Integer = reader.GetOrdinal(columnName)

      If (Not reader.IsDBNull(columnIndex)) Then
        'Return reader.GetDecimal(columnIndex)
        Return CDec(reader(columnIndex))
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
    Function SafeGetDateTime(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As DateTime?) As DateTime?

      Dim columnIndex As Integer = reader.GetOrdinal(columnName)

      If (Not reader.IsDBNull(columnIndex)) Then
        'Return reader.GetDateTime(columnIndex)
        Return CDate(reader(columnIndex))
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
    Function SafeGetByteArray(ByVal reader As SqlDataReader, ByVal columnName As String) As Byte()

      Dim columnIndex As Integer = reader.GetOrdinal(columnName)

      If (Not reader.IsDBNull(columnIndex)) Then
        Return reader(columnIndex)
      Else
        Return Nothing
      End If
    End Function

#End Region


    Public Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
      Dim result As Boolean
      If (Not Boolean.TryParse(stringvalue, result)) Then
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



#Region "Convert To and From Base64..."

    ' String nach Base64 codieren
    Public Function ToBase64(ByVal sText As String) As String
      ' String zunächst in ein Byte-Array umwandeln
      Dim nBytes() As Byte = System.Text.Encoding.Default.GetBytes(sText)

      ' jetzt das Byte-Array nach Base64 codieren
      Return System.Convert.ToBase64String(nBytes)
    End Function
    ' Base64-String in lesbaren String umwandeln
    Public Function FromBase64(ByVal sText As String) As String
      ' Base64-String zunächst in ByteArray konvertieren
      Dim nBytes() As Byte = System.Convert.FromBase64String(sText)

      ' ByteArray in String umwandeln
      Return System.Text.Encoding.Default.GetString(nBytes)
    End Function


#End Region


#Region "XML-Functions..."

    Public Function GetSafeStringFromXElement(ByVal xelment As XElement)

      If xelment Is Nothing Then
        Return String.Empty
      Else

        Return xelment.Value
      End If

    End Function

    ''' <summary>
    ''' Gibt die XML-Value einer Datei und Query aus...
    ''' </summary>
    ''' <param name="strFilename"></param>
    ''' <param name="strQuery"></param>
    ''' <param name="strValuebyNull"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetXMLValueByQueryWithFilename(ByVal strFilename As String, _
                                ByVal strQuery As String, _
                                ByVal strValuebyNull As String) As String
      Dim bResult As String = String.Empty
      Dim strBez As String = GetXMLNodeValue(strFilename, strQuery)

      If strBez = String.Empty Then strBez = strValuebyNull

      Return strBez
    End Function

    Function GetXMLNodeValue(ByVal strFileName As String, ByVal strQuery As String) As String
      Dim strValue As String = String.Empty
      Dim xmlDoc As New Xml.XmlDocument()
      Dim xpNav As XPathNavigator
      Dim xni As XPathNodeIterator

      Try
        If File.Exists(strFileName) Then
          xmlDoc.Load(strFileName)
          xpNav = xmlDoc.CreateNavigator()

          xni = xpNav.Select(strQuery)
          Do While xni.MoveNext()
            strValue = xni.Current.Value

          Loop
        End If

      Catch ex As Exception
        logger.Error(ex.ToString)

      End Try

      Return strValue
    End Function



#Region "Fileutility"

		''' <summary>
		''' creates new \\server\mdxx\profiles\UserProfilexx.xml file 
		''' </summary>
		''' <param name="connstring"></param>
		''' <param name="userNumber"></param>
		''' <param name="mandantNumber"></param>
		''' <remarks></remarks>
		Sub CreateUserProfileXMLFile(ByVal connstring As String, ByVal userNumber As Integer, ByVal mandantNumber As Integer)
			Dim enc As New System.Text.UnicodeEncoding
			Dim strStartElementName As String = "UserProfile"
			Dim strAttribute As String = "UserNr"
			Dim strField_2 As String = "Document"
			Dim strField_3 As String = ""
			If userNumber = 0 Then Return
			If mandantNumber = 0 Then Return
			Dim m_md As New Mandant
			If m_reg Is Nothing Then m_reg = New ClsDivReg
			Dim xmlUserProfile = m_md.GetSelectedMDUserProfileXMLFilename(mandantNumber, userNumber)

			Dim strOldUsINIFile As String = String.Format("{0}\UserPro{1}", System.IO.Directory.GetParent(xmlUserProfile).FullName, userNumber)
			Dim strOldFilename As String = String.Format("{0}OldFile.xml", GetSpS2DeleteHomeFolder())
			Try
				File.Copy(xmlUserProfile, strOldFilename, True)
			Catch ex As Exception

			End Try

			' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 
			Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(xmlUserProfile, enc)
			Dim strValue As String = String.Empty
			Dim bResult As Boolean = True
			Dim _clsreg As New ClsDivReg

			Try

				With XMLobj
					' Formatierung: 4er-Einzüge verwenden 
					.Formatting = Xml.Formatting.Indented
					.Indentation = 4

					.WriteStartDocument()
					.WriteStartElement(strStartElementName)


					' Die Masken-Vorlage für Kundensuche...
					' Layouts
					.WriteStartElement("Layouts")
					.WriteStartElement("Form_DevEx")

					Dim strQuery As String = "//Layouts/Form_DevEx/FormStyle"
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("FormStyle")
					.WriteString(strValue)
					.WriteEndElement()

					strQuery = "//Layouts/Form_DevEx/NavbarStyle"
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("NavbarStyle")
					.WriteString(strValue)
					.WriteEndElement()

					.WriteEndElement()

					.WriteStartElement("Report")
					strQuery = "//Layouts/Report/matrixprintername"
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("matrixprintername")
					.WriteString(strValue)
					.WriteEndElement()

					.WriteEndElement()

					.WriteEndElement()



					.WriteStartElement(String.Format("User_{0}", userNumber))
					.WriteStartElement("Documents")


					Try
						Dim sSql As String = "Select JobNr, DocName From DokPrint Where (JobNr <> '' Or JobNr Is Not Null) And "
						sSql &= "(DocName <> '' or DocName is not null) Order By JobNr"
						Dim Conn As SqlConnection = New SqlConnection(connstring)

						Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
						Conn.Open()
						Dim rDocrec As SqlDataReader = cmd.ExecuteReader

						While rDocrec.Read

							strValue = m_reg.GetINIString(strOldUsINIFile, "Export", rDocrec("DocName"))
							If strValue = String.Empty Then strValue = "0"

							.WriteStartElement("DocName")
							.WriteAttributeString("ID", rDocrec("JobNr"))
							.WriteStartElement("Export")
							.WriteString(strValue)
							.WriteEndElement()
							.WriteEndElement()

						End While

					Catch ex As Exception
						MsgBox(String.Format("DokPrint Error: {0}", ex.Message))

					End Try

					.WriteEndElement()


					' Die Masken-Vorlage für Kundensuche...
					.WriteStartElement("FormControls")
					.WriteStartElement("FormName")
					.WriteAttributeString("ID", "4c2db8b0-0521-4862-a640-d895e02100f9")
					.WriteStartElement("TemplateFile")
					.WriteString("")
					.WriteEndElement()
					.WriteEndElement()
					' Fertig

					' Die Sonstigen Einstellungen...
					.WriteStartElement("USSetting")
					.WriteStartElement("SettingName")
					.WriteAttributeString("ID", "Cockpit.DayAgo4GebDat")
					.WriteStartElement("USValue")
					.WriteString("")
					.WriteEndElement()
					.WriteEndElement()
					' Fertig
					.WriteEndElement()
					.WriteEndElement()
					.WriteEndElement()


					' CSV-Setting... ----------------------------------------------------------------------
					' ----------------------------------------------------------------------
					strQuery = ""

					.WriteComment("Export Einstellungen")
					strAttribute = "Name"

					.WriteStartElement("CSV-Setting")
					.WriteAttributeString(strAttribute, "MASearch")

					' SelectedFields
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "MASearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("SelectedFields")
					.WriteString(strValue)
					.WriteEndElement()

					' FieldIn
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "MASearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("FieldIn")
					.WriteString(strValue)
					.WriteEndElement()

					' FieldSep
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "MASearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
					.WriteStartElement("FieldSep")
					.WriteString(strValue)
					.WriteEndElement()

					' ExportFileName
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "MASearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("ExportFileName")
					.WriteString(strValue)
					.WriteEndElement()
					.WriteEndElement()


					.WriteStartElement("CSV-Setting")
					.WriteAttributeString(strAttribute, "KDSearch")

					' SelectedFields ---------------------------------------------------------------------------
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "KDSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("SelectedFields")
					.WriteString(strValue)
					.WriteEndElement()

					' FieldIn
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "KDSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("FieldIn")
					.WriteString(strValue)
					.WriteEndElement()

					' FieldSep
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "KDSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
					.WriteStartElement("FieldSep")
					.WriteString(strValue)
					.WriteEndElement()

					' ExportFileName
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "KDSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("ExportFileName")
					.WriteString(strValue)
					.WriteEndElement()
					.WriteEndElement()


					.WriteStartElement("CSV-Setting")
					.WriteAttributeString(strAttribute, "ESSearch")

					' SelectedFields -------------------------------------------------------------------------------------------
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "ESSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("SelectedFields")
					.WriteString(strValue)
					.WriteEndElement()

					' FieldIn
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "ESSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("FieldIn")
					.WriteString(strValue)
					.WriteEndElement()

					' FieldSep
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "ESSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
					.WriteStartElement("FieldSep")
					.WriteString(strValue)
					.WriteEndElement()

					' ExportFileName
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "ESSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("ExportFileName")
					.WriteString(strValue)
					.WriteEndElement()
					.WriteEndElement()


					.WriteStartElement("CSV-Setting")
					.WriteAttributeString(strAttribute, "ESKDSearch")

					' SelectedFields -------------------------------------------------------------------------------------------
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "ESKDSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("SelectedFields")
					.WriteString(strValue)
					.WriteEndElement()

					' FieldIn
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "ESKDSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("FieldIn")
					.WriteString(strValue)
					.WriteEndElement()

					' FieldSep
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "ESKDSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
					.WriteStartElement("FieldSep")
					.WriteString(strValue)
					.WriteEndElement()

					' ExportFileName
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "ESKDSearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("ExportFileName")
					.WriteString(strValue)
					.WriteEndElement()
					.WriteEndElement()


					.WriteStartElement("CSV-Setting")
					.WriteAttributeString(strAttribute, "ESMASearch")

					' SelectedFields -------------------------------------------------------------------------------------------
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/SelectedFields", Chr(34), "ESMASearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("SelectedFields")
					.WriteString(strValue)
					.WriteEndElement()

					' FieldIn
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldIn", Chr(34), "ESMASearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("FieldIn")
					.WriteString(strValue)
					.WriteEndElement()

					' FieldSep
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/FieldSep", Chr(34), "ESMASearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, ";")
					.WriteStartElement("FieldSep")
					.WriteString(strValue)
					.WriteEndElement()

					' ExportFileName
					strQuery = String.Format("//CSV-Setting[@Name={0}{1}{0}]/ExportFileName", Chr(34), "ESMASearch")
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
					.WriteStartElement("ExportFileName")
					.WriteString(strValue)
					.WriteEndElement()
					.WriteEndElement()


					' ExportSetting LO -------------------------------------------------------------------------------------------
					Dim sValue As String = String.Empty
					Dim strGuid As String = "SP.LO.PrintUtility"
					Dim strMainKey As String = "//ExportSetting[@Name={0}{1}{0}]/{2}"
					.WriteComment("Exporteinstellung für den Druck und Export Lohnabrechnungen...")
					.WriteStartElement("ExportSetting")
					.WriteAttributeString(strAttribute, strGuid)

					Dim strKeyName As String = "ExportPfad".ToLower
					strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
					.WriteStartElement(strKeyName)
					.WriteString(strValue)
					.WriteEndElement()

					strKeyName = "ExportFinalFileFilename".ToLower
					strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
					.WriteStartElement(strKeyName)
					.WriteString(strValue)
					.WriteEndElement()

					strKeyName = "ExportFilename".ToLower
					strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
					.WriteStartElement(strKeyName)
					.WriteString(strValue)
					.WriteEndElement()
					.WriteEndElement()


					' ExportSetting ES -------------------------------------------------------------------------------------------
					sValue = String.Empty
					.WriteComment("Exporteinstellung für den Druck und Export Einsatzverträge...")
					strGuid = "SP.ES.PrintUtility"
					strMainKey = "//ExportSetting[@Name={0}{1}{0}]/{2}"
					.WriteStartElement("ExportSetting")
					.WriteAttributeString(strAttribute, strGuid)

					strKeyName = "ESUnterzeichner_ESVertrag".ToLower
					strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, 0)
					.WriteStartElement(strKeyName)
					.WriteString(strValue)
					.WriteEndElement()

					strKeyName = "ExportPfad".ToLower
					strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
					.WriteStartElement(strKeyName)
					.WriteString(strValue)
					.WriteEndElement()

					strKeyName = "ExportFilename_ESVertrag".ToLower
					strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
					.WriteStartElement(strKeyName)
					.WriteString(strValue)
					.WriteEndElement()

					strKeyName = "ExportFilename_Verleih".ToLower
					strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
					.WriteStartElement(strKeyName)
					.WriteString(strValue)
					.WriteEndElement()

					strKeyName = "ExportFinalFileFilename_ESVertrag".ToLower
					strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
					.WriteStartElement(strKeyName)
					.WriteString(strValue)
					.WriteEndElement()

					strKeyName = "ExportFinalFileFilename_Verleih".ToLower
					strQuery = String.Format(strMainKey, Chr(34), strGuid, strKeyName)
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
					.WriteStartElement(strKeyName)
					.WriteString(strValue)
					.WriteEndElement()

					.WriteEndElement()


					' AdvancePayment Quitttung & Check ----------------------------------------------------------------------------------------
					sValue = String.Empty
					.WriteComment("AdvancePayment Quitttung and Check...")
					strKeyName = "advancepayment"
					.WriteStartElement(strKeyName)

					'-------------------------------------------------------------------------------------------------------------------
					strAttribute = "maxvalue8900"
					strValue = _clsreg.GetINIString(strOldUsINIFile, "ZG", "ZGMaxBetragBar", "0")
					.WriteStartElement(strAttribute)
					.WriteString(strValue)
					.WriteEndElement()

					strAttribute = "maxvalue8930"
					strValue = _clsreg.GetINIString(strOldUsINIFile, "ZG", "ZGMaxBetragCheck", "0")
					.WriteStartElement(strAttribute)
					.WriteString(strValue)
					.WriteEndElement()

					.WriteEndElement()



					' askonexit ----------------------------------------------------------------------------------------
					sValue = String.Empty
					strKeyName = "programsetting"
					.WriteStartElement(strKeyName)

					'-------------------------------------------------------------------------------------------------------------------
					strAttribute = "askonexit"
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, "false")
					.WriteStartElement(strAttribute)
					.WriteString(strValue)
					.WriteEndElement()


					.WriteEndElement()





					.WriteEndElement()
					.Close()


				End With

			Catch ex As Exception
				logger.Error(String.Format("CreateUserProfileXMLFile: {0}", ex.Message))

			End Try

		End Sub


		''' <summary>
		''' creates new \\server\mdxx\year\programm.xml file 
		''' </summary>
		''' <param name="strFileName"></param>
		''' <param name="mandantNumber"></param>
		''' <remarks></remarks>
		Sub CreateXMLFile4Mandant(ByVal strFileName As String, ByVal mandantNumber As Integer)
			Return


			Dim enc As New System.Text.UnicodeEncoding
			Dim m_md As New Mandant
			Dim m_path As New ProgPath.ClsProgPath
			If m_reg Is Nothing Then m_reg = New ClsDivReg

			If mandantNumber = 0 Then mandantNumber = m_md.GetDefaultMDNr
			Dim strStartElementName As String = String.Format("MD_{0}", mandantNumber)
			Dim strAttribute As String = String.Empty
			Dim strField_2 As String = "LV"
			Dim strField_3 As String = ""
			Dim m_PayrollSetting As String = String.Format("MD_{0}/Lohnbuchhaltung", mandantNumber)

			Dim xmlMandantProfile = m_md.GetSelectedMDDataXMLFilename(mandantNumber, Now.Year)

			Dim strMyMDDatFile As String = String.Format("{0}\Programm.dat", System.IO.Directory.GetParent(xmlMandantProfile).FullName)	 ' _ClsProgSetting.GetMDIniFile()
			Dim strSection As String = String.Empty
			Dim strQuery As String = String.Empty
			Dim strAbschnitt As String = ""


			strAttribute = "startnewbvgcalculationnow"
			Dim value As String = m_path.GetXMLNodeValue(xmlMandantProfile, String.Format("{0}/startnewbvgcalculationnow", m_PayrollSetting))
			Dim startNewBVGCalculationNow As Boolean?
			If Not String.IsNullOrWhiteSpace(value) Then
				startNewBVGCalculationNow = m_path.ParseToBoolean(value, Nothing)
			End If

			strAttribute = "donotcontrolbvgMinLohn"
			value = m_path.GetXMLNodeValue(xmlMandantProfile, String.Format("{0}/donotcontrolbvgMinLohn", m_PayrollSetting))
			Dim donotcontrolbvgMinLohn As Boolean?
			If Not String.IsNullOrWhiteSpace(value) Then
				donotcontrolbvgMinLohn = m_path.ParseToBoolean(value, Nothing)
			End If

			Dim strOldFilename As String = Path.Combine(GetSpS2DeleteHomeFolder(), "OldFile.xml")	' _ClsProgSetting.GetSpSFiles2DeletePath & "OldFile.xml"
			Try
				File.Copy(strFileName, strOldFilename, True)
			Catch ex As Exception
				logger.Error(String.Format("{0}", ex.ToString))

			End Try

			' XmlTextWriter-Objekt für unsere Ausgabedatei erzeugen: 

			Dim XMLobj As Xml.XmlTextWriter = New Xml.XmlTextWriter(strFileName, enc)
			Dim strValue As String = String.Empty

			With XMLobj
				' Formatierung: 4er-Einzüge verwenden 
				.Formatting = Xml.Formatting.Indented
				.Indentation = 4

				.WriteStartDocument()
				.WriteStartElement(strStartElementName)


				strSection = "Sonstiges"
				.WriteStartElement(strSection)
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "BewName"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "BewPLZOrt"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "CreditInfo"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				' Bewilligungsbehörden --------------------------------------------------------------------------------------------
				strAttribute = "BewPostfach"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "BewStrasse"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "BewSeco"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "BURNumber"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "UIDNumber"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAR.-MitgliedNr"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "MDSelectedColor"
				strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "Extension4Deletedrecs"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				''-------------------------------------------------------------------------------------------------------------------
				'strAttribute = "KassaStartEndepflicht"
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				'.WriteStartElement(strAttribute)
				'.WriteString(strValue)
				'.WriteEndElement()

				''-------------------------------------------------------------------------------------------------------------------
				'strAttribute = "KassaStartEndeAfter"
				'strValue = m_reg.GetINIString(strMyMDDatFile, strSection, strAttribute, "")
				'.WriteStartElement(strAttribute)
				'.WriteString(strValue)
				'.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHV-Ausgleichskasse"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "Unfallversicherung"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "LL_IndentSize"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FontSize"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FontName"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Calibri")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "LLTemplateExtension"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "DOC")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "MailFontName"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Calibri")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "MailFontSize"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "11")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strQuery = "//Sonstiges/EnableLLDebug"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				.WriteStartElement("EnableLLDebug")
				.WriteString(strValue)
				.WriteEndElement()

				strQuery = "//Sonstiges/mandantcolor"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement("mandantcolor")
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "payrollluepaymentmethodifempty"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				' Sonstiges fertig schreiben...
				.WriteEndElement()




				'-------------------------------------------------------------------------------------------------------------------



				strAbschnitt = "Lohnbuchhaltung"
				.WriteStartElement("Lohnbuchhaltung")
				strSection = "guthaben"
				.WriteStartElement("guthaben")

				strAttribute = "showguthabenpereaches"
				strQuery = String.Format("//{0}/guthaben/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				' Sonstiges fertig schreiben...
				.WriteEndElement()


				strSection = "report"
				.WriteStartElement("report")
				strAttribute = "tagesspesenstdab"
				strQuery = String.Format("//{0}/report/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then
					strValue = m_reg.GetINIString(strMyMDDatFile, "Lohnbuchhaltung", "TagesSpesenStd", "8.25")
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				.WriteEndElement()



				If startNewBVGCalculationNow.HasValue Then

					strAttribute = "startnewbvgcalculationnow"
					strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, startNewBVGCalculationNow)
					.WriteStartElement(strAttribute)
					.WriteString(strValue)
					.WriteEndElement()

				End If

				If donotcontrolbvgMinLohn.HasValue Then
					strAttribute = "donotcontrolbvgMinLohn"
					strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
					strValue = GetXMLValueByQuery(strOldFilename, strQuery, donotcontrolbvgMinLohn)
					.WriteStartElement(strAttribute)
					.WriteString(strValue)
					.WriteEndElement()

				End If

				strAttribute = "payrollcheckfee"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "advancepaymentcheckfee"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "advancepaymenttransferfee"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "advancepaymentcashfee"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "advancepaymenttransferinternationalfee"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "payrolltransferinternationalfee"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "bvgafter"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "bvginterval"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "ww")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "bvgintervaladd"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "calculatebvgwithesdays"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				'End If
				If strValue = "1" Or strValue = "true" Then
					strValue = "true"
				Else
					strValue = "false"
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "kizulagenotiflanrcontains"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "stringforadvancepaymentwithfee"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "deletepayrollwithbruttozero"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				'End If
				If strValue = "1" Or strValue = "true" Then
					strValue = "true"
				Else
					strValue = "false"
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "calculatetaxbasiswithnot180"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				'End If
				If strValue = "1" Or strValue = "true" Then
					strValue = "true"
				Else
					strValue = "false"
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "taxprocentforborderforeigner"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				'strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				'If strValue = String.Empty Then
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "0")
				'End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				' Lohnbuchhaltung fertig schreiben...
				.WriteEndElement()





				strSection = "Export"	' ........................................................................................
				.WriteStartElement(strSection)

				' Achtung: (Export-Abschnitt)----------------------------------------------------------------------------------
				strAttribute = "MA_SPUser_ID"
				strQuery = String.Format("//Export/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "KD_SPUser_ID"
				strQuery = String.Format("//Export/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Vak_SPUser_ID"
				strQuery = String.Format("//Export/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Ver_SPUser_ID"
				strQuery = String.Format("//Export/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				' Sonstiges fertig schreiben...
				.WriteEndElement()


				' Test ---------------------

				strAbschnitt = "Export_1"
				Dim strOrgAbschnitt As String = "Export"
				.WriteStartElement(strAbschnitt)

				strAttribute = "MA_SPUser_ID"
				strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "KD_SPUser_ID"
				strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Vak_SPUser_ID"
				strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Ver_SPUser_ID"
				strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				' Export_1 fertig schreiben...
				.WriteEndElement()


				strAbschnitt = "Template_1"
				strOrgAbschnitt = "Template"
				.WriteStartElement(strAbschnitt)

				strAttribute = "cockpit-url"
				strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "cockpit-picture"
				strQuery = String.Format("//{0}/{1}", strOrgAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, String.Empty)
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				' Template_1 fertig schreiben...
				.WriteEndElement()


				' Abschluss der Test -------------------------------

				strSection = "Templates" ' .....................................................................................
				.WriteStartElement(strSection)
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "cockpit-picture"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "cockpit-url"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "ZeugnisDeckblatt"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AGBDeckblatt"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AGB4Temp"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "AGBTemp.PDF")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AGB4Fest"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "AGBFest.PDF")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "PMail_tplDocNr"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "18.0.1")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "customer-contact-subject"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "customer-mail-contact-subject"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "customer-fax-contact-subject"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "customer-contact-body"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "customer-sms-contact-subject"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "customer-sms-contact-body"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "CreatedLLFileAs"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "Dossier {0} {1}")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "eMailImageVar1"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "eMailImageValue1"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "eMailImageVar2"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "eMailImageValue2"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "eMailImageVar3"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "eMailImageValue3"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				' Achtung: (Templates-Abschnitt)----------------------------------------------------------------------------------
				strAttribute = "Zwischenverdienstformular_Doc-eMail-Template"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_ZVDoc.txt")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "Arbeitgeberbescheinigung_Doc-eMail-Template"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_ArbgDoc.txt")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "MADoc-eMail-Template"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_MADoc.txt")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "KDDoc-eMail-Template"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_KDDoc.txt")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "ZHDDoc-eMail-Template"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_ZHDDoc.txt")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "Cockpit-eMail-Template"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "MailTempl_CockpitWOS.txt")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				' Achtung: (Templates-Abschnitt)----------------------------------------------------------------------------------
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "eMail-Template-JobNr"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "eMail-Template"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "MassenOffer-eMail-Template-JobNr"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "MassenOffer-eMail-Template"
				strQuery = String.Format("//Templates/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				' Sonstiges fertig schreiben...
				.WriteEndElement()




				strAbschnitt = "Mailing"
				strSection = "Mailing"
				.WriteStartElement(strAbschnitt)

				strAttribute = "faxusername"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "faxuserpw"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "ecalljobid"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "ecallfromtext"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "ecallheaderid"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "ecallheaderinfo"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "ecallsubject"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "ecallnotification"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "ecalltoken"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "faxextension"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "faxforwarder"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "davidserver"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "cockpitwww"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "SMTP-Server"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "SMTP-Port"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()



				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "Zwischenverdienstformular_Doc-eMail-Betreff"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "Arbeitgeberbescheinigung_Doc-eMail-Betreff"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "MADoc-eMail-Betreff"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "MADoc-WWW"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "KDDoc-eMail-Betreff"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "KDDoc-WWW"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "ZHDDoc-eMail-Betreff"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "ZHDDoc-WWW"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "Mail-Database"
				strQuery = String.Format("//Mailing/{0}", strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				' Sonstiges fertig schreiben...
				.WriteEndElement()





				strSection = "SUVA-Daten"	' ....................................................................................
				.WriteStartElement(strSection)
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "SUVAAddressZusatz"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "SUVAAddressZHD"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "SUVAAddressPostfach"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "SUVAAddressStrasse"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "SUVAAddressPLZOrt"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "Abrechnungsnummer"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "SUVAAddressSub1"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "SUVAAddressSub2"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "SUVAAddressSub3"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "SUVAAddressSub4"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "SUVAAddressSub5"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "SUVAAddressSub6"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				.WriteEndElement()


				.WriteStartElement("AHV-Daten")

				' AHV-Adressen -----------------------------------------------------------------------------------------------------
				strAttribute = "AHVAddressZusatz"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHVAddressZHD"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHVAddressPostfach"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHVAddressStrasse"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHVAddressPLZOrt"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHVMitgliedNr"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AusgNummer"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHVSub1"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHVSub2"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHVSub3"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHVSub4"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHVSub5"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "AHVSub6"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				.WriteEndElement()


				.WriteStartElement("Fak-Daten")

				' FAK-Kasse -------------------------------------------------------------------------------------------------------
				strAttribute = "FAKKassenname"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressZusatz"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressZHD"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressPostfach"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressStrasse"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressPLZOrt"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressMitgliednr"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressNr"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressSub1"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressSub2"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressSub3"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressSub4"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressSub5"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "FAKAddressSub6"
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				'-------------------------------------------------------------------------------------------------------------------
				.WriteStartElement("BuchungsKonten")

				For i As Integer = 0 To 37
					strAttribute = Str(i + 1).Trim
					strValue = m_reg.GetINIString(strMyMDDatFile, "BuchungsKonten", strAttribute, "")
					.WriteStartElement("_" & strAttribute)
					.WriteString(strValue)
					.WriteEndElement()
				Next

				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				'-------------------------------------------------------------------------------------------------------------------
				strAbschnitt = "StartNr"
				.WriteStartElement(strAbschnitt)

				strAttribute = "Mitarbeiter"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then
					strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				End If
				If strValue = String.Empty Then
					strValue = "0"
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Kunden"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then
					strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				End If
				If strValue = String.Empty Then
					strValue = "0"
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Vakanzennr"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Offers"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Einsatzverwaltung"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Rapporte"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Vorschussverwaltung"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Fakturen"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Zahlungseingänge"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Mahnungen"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Lohnabrechnung"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "FremdOP"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "Kassenbuch"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "FirstKassenBetrag"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "StartNr", strAttribute, "")
				If strValue = String.Empty Then strValue = "0"
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------


				'-------------------------------------------------------------------------------------------------------------------
				'-------------------------------------------------------------------------------------------------------------------
				strAbschnitt = "Debitoren"
				.WriteStartElement(strAbschnitt)

				strAttribute = "mwstnr"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Or Not strValue.StartsWith("CHE-") Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", strAttribute, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "mwstsatz"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Or Val(strValue) < 8 Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "mwst-satz", "8")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "ref10forfactoring"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "RefNrTo10", "0")
				If strValue = "1" Or strValue = "true" Then
					strValue = "true"
				Else
					strValue = "false"
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "ezonsepratedpage"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "Trenne die Einzahlungsscheine", "0")
				If strValue = "1" Or strValue = "true" Then
					strValue = "true"
				Else
					strValue = "false"
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "setfakdatetoendofreportmonth"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "FakDateAsRPEndDate", "0")
				If strValue = "1" Or strValue = "true" Then
					strValue = "true"
				Else
					strValue = "false"
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "create3mahnasuntilnotpaid"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "Create3MahnByNotPaid", "0")
				If strValue = "1" Or strValue = "true" Then
					strValue = "true"
				Else
					strValue = "false"
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "printezwithmahn"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "MahnWithEZ", "0")
				If strValue = "1" Or strValue = "true" Then
					strValue = "true"
				Else
					strValue = "false"
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "printguonmahnung"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "MahnWithGutschrift", "0")
				If strValue = "1" Or strValue = "true" Then
					strValue = "true"
				Else
					strValue = "false"
				End If
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				strAttribute = "mahnspesenab"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "Mahnspesen ab", "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "mahnspesenchf"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "mahnspesen", "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "verzugszinsdaysafter"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "verzugszinson", "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "verzugszinspercent"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "verzugszinsen", "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "verzugszinsabchf"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "Verzugszins Ab", "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "factoringcustomernumber"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "factoskdnr", "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "invoicezipfilename"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				If strValue = String.Empty Then strValue = m_reg.GetINIString(strMyMDDatFile, "Debitoren", "opzipfilename", "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				'-------------------------------------------------------------------------------------------------------------------


				strAbschnitt = "Licencing"
				.WriteStartElement(strAbschnitt)

				strAttribute = "sesam"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "abacus"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "swifac"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "comatic"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "kmufactoring"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "csoplist"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "parifond"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "customermng_deltavistaSolvencyCheckReferenceNumber"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "customermng_deltavistaWebServiceUserName"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "customermng_deltavistaWebServicePassword"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				strAttribute = "customermng_deltavistaWebServiceUrl"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				.WriteEndElement()
				'-------------------------------------------------------------------------------------------------------------------

				strAbschnitt = "Interfaces"
				strSection = "Interfaces"
				.WriteStartElement(strAbschnitt)

				strAttribute = "abalofieldtrennzeichen"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abalodarstellungszeichen"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abalorefnr"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abalogegenkonto"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abalomwstcode"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				strAttribute = "abaopfieldtrennzeichen"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abaopdarstellungszeichen"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abaoprefnr"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abaopgegenkonto"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abaopmwstcode"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				strAttribute = "abazefieldtrennzeichen"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abazedarstellungszeichen"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abazerefnr"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abazegegenkonto"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "abazemwstcode"
				strQuery = String.Format("//{0}/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				.WriteStartElement("webservices")

				strAttribute = "webservicejobdatabase"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webservicebankdatabase"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wssps_services/spbankutil.asmx")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				' TODO: Muss nachher gelöscht werden!!!
				strAttribute = "webserviceqstdatabase"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webservicegavdatabase"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webservicegavutility"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webservicewosemployeedatabase"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webservicewoscustomer"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webservicewosvacancies"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webservicejobchvacancies"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webserviceostjobvacancies"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webservicesuedostvacancies"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webserviceecall"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webservicepaymentservices"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wssps_services/spcustomerpaymentservices.asmx")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webservicetaxinfoservices"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wsSPS_services/SPEmployeeTaxInfoService.asmx")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webservicealkdatabase"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wsSPS_services/SPALKUtil.asmx")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()

				strAttribute = "webserviceupdateinfoservices"
				strQuery = String.Format("//{0}/webservices/{1}", strAbschnitt, strAttribute)
				strValue = GetXMLValueByQuery(strOldFilename, strQuery, "http://asmx.domain.com/wssps_services/SPUpdateUtilities.asmx")
				.WriteStartElement(strAttribute)
				.WriteString(strValue)
				.WriteEndElement()


				.WriteEndElement()

				'-------------------------------------------------------------------------------------------------------------------
				'-------------------------------------------------------------------------------------------------------------------
				'-------------------------------------------------------------------------------------------------------------------

				' Write the XML to file and close the writer.
				.WriteEndElement()
				.Flush()
				.Close()

			End With

		End Sub



		Private Function GetXMLValueByQuery(ByVal strFilename As String, ByVal strQuery As String, ByVal strValuebyNull As String) As String
			Dim bResult As String = String.Empty
			Dim strBez As String = GetXMLNodeValue(strFilename, strQuery)

			If strBez = String.Empty Then strBez = strValuebyNull

			Return strBez
		End Function


#End Region


#End Region



  End Class



End Namespace
