
Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml.Linq
'Imports SP.Infrastructure.Logging


''' <summary>
''' Base class for database access.
''' </summary>
Public Class DatabaseAccessBase



#Region "Private Fields"

	Private m_ConnectionString As String

	''' <summary>
	''' Stores an explicit connection.
	''' </summary>
	''' <remarks>An explicit connection can be user across multiple database calls.</remarks>
	Public m_ExcplicitConnection As SqlConnection

	''' <summary>
	''' Stores an explicit transaction.
	''' </summary>
	''' <remarks>An explicit transaction can be user across multiple database calls.</remarks>
	Private m_ExplicitTransaction As SqlTransaction

#End Region

#Region "Protected Fields"

	''' <summary>
	''' The logger.
	''' </summary>
	Protected Shared m_Logger As Logging.ILogger = New Logging.Logger()


#End Region

#Region "Constructor"

	''' <summary>
	''' Constructor.
	''' </summary>
	''' <param name="connectionString">The connection string.</param>
	''' <param name="translationLanguage">The translation language.</param>
	Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
		SelectedTranslationLanguage = translationLanguage

		If String.IsNullOrWhiteSpace(connectionString) Then
			m_ConnectionString = GetDbConnectionString(0, My.Settings.SPSEnterpriseFolder)
		Else
			m_ConnectionString = connectionString
		End If



	End Sub

	''' <summary>
	''' Constructor.
	''' </summary>
	''' <param name="connectionString">The connection string.</param>
	''' <param name="translationLanguage">The translation language</param>
	Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)

		If String.IsNullOrEmpty(translationLanguage) Then
			translationLanguage = String.Empty
		End If

		Select Case translationLanguage.ToLower().TrimEnd()
			Case "de", "d"
				Me.SelectedTranslationLanguage = Language.German
			Case "it", "i"
				Me.SelectedTranslationLanguage = Language.Italian
			Case "fr", "f"
				Me.SelectedTranslationLanguage = Language.French
			Case "en", "e"
				Me.SelectedTranslationLanguage = Language.English
			Case Else
				Me.SelectedTranslationLanguage = Language.German
		End Select

		m_ConnectionString = connectionString

	End Sub

#End Region

#Region "Properties"

	''' <summary>
	''' Gets the DB connection string.
	''' </summary>
	''' <returns>The DB connection string.</returns>
	Protected ReadOnly Property DBConnectionstring() As String
		Get
			Return m_ConnectionString
		End Get
	End Property

	''' <summary>
	''' Gets or sets the selected translation language.
	''' </summary>
	''' <returns>The selected translation language.</returns>
	Public Property SelectedTranslationLanguage As Language = Language.German

	''' <summary>
	''' Gets a boolean flag indicating if an explicit connection is available.
	''' </summary>
	''' <returns>Boolean flag.</returns>
	Public ReadOnly Property IsExplicitConnectionOpen
		Get
			Return Not m_ExcplicitConnection Is Nothing
		End Get
	End Property

	''' <summary>
	''' Gets a boolean flag indicating if an explicit transaction is available.
	''' </summary>
	''' <returns>Boolean flag.</returns>
	Public ReadOnly Property IsExplicitTransactionAvailable
		Get
			Return Not m_ExplicitTransaction Is Nothing
		End Get
	End Property

#End Region

#Region "Methods"

	''' <summary>
	''' Opens an explicit connection.
	''' </summary>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function OpenExplicitConnection() As Boolean

		Dim success As Boolean = True

		If m_ExcplicitConnection Is Nothing Then

			Try
				m_ExcplicitConnection = New SqlClient.SqlConnection(DBConnectionstring)
				m_ExcplicitConnection.Open()

			Catch ex As Exception
				success = False
				m_Logger.LogError(ex.ToString())
				m_ExcplicitConnection.Close()
				m_ExcplicitConnection.Dispose()
				m_ExcplicitConnection = Nothing

			End Try
		Else
			' Can not open another explicit connection.
			success = False
		End If

		Return success
	End Function

	''' <summary>
	''' Opens an explicit connection.
	''' </summary>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function OpenExplicitConnection(ByVal connStr As String) As Boolean

		Dim success As Boolean = True

		Try
			m_ExcplicitConnection = New SqlClient.SqlConnection(connStr)
			m_ExcplicitConnection.Open()

		Catch ex As Exception
			success = False
			m_Logger.LogError(ex.ToString())
			m_ExcplicitConnection.Close()
			m_ExcplicitConnection.Dispose()
			m_ExcplicitConnection = Nothing

		End Try


		Return success
	End Function

	''' <summary>
	''' Closes an explicit connection.
	''' </summary>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function CloseExplicitConnection() As Boolean
		Dim success As Boolean = True

		If m_ExcplicitConnection Is Nothing Then
			success = False
		Else
			Try
				m_ExcplicitConnection.Close()
				m_ExcplicitConnection.Dispose()
				m_ExcplicitConnection = Nothing
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())
				m_ExcplicitConnection = Nothing
				success = False
			End Try

		End If

		Return success
	End Function

	''' <summary>
	''' Opens an explicit transaction.
	''' </summary>
	Public Function OpenExplicitTransaction() As Boolean

		Dim success As Boolean = True

		If m_ExplicitTransaction Is Nothing Then

			If Not m_ExcplicitConnection Is Nothing Then

				Try
					m_ExplicitTransaction = m_ExcplicitConnection.BeginTransaction(IsolationLevel.ReadCommitted)
				Catch ex As Exception
					m_Logger.LogError(ex.ToString())
					m_ExplicitTransaction.Dispose()
					m_ExplicitTransaction = Nothing
				End Try

			Else
				success = False
			End If

		Else
			success = False
		End If

		Return success
	End Function

	''' <summary>
	''' Commits and explicit transaction.
	''' </summary>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function CommitExplicitTransaction() As Boolean

		Dim success As Boolean = True

		If m_ExplicitTransaction Is Nothing Then
			success = False
		Else
			Try
				m_ExplicitTransaction.Commit()
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			Finally
				m_ExplicitTransaction = Nothing
			End Try
		End If

		Return success
	End Function

	''' <summary>
	''' Rollback and explicit transaction.
	''' </summary>
	''' <returns>Boolean flag indicating success.</returns>
	Public Function RollbackExplicitTransaction() As Boolean

		Dim success As Boolean = True

		If m_ExplicitTransaction Is Nothing Then
			success = False
		Else
			Try
				m_ExplicitTransaction.Rollback()
			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

			Finally
				m_ExplicitTransaction = Nothing
			End Try
		End If

		Return success

	End Function

	''' <summary>
	''' Opens a SqlClient.SqlDataReader object. 
	''' </summary>
	''' <param name="sql">The sql string.</param>
	''' <param name="parameters">The parameters collection.</param>
	''' <returns>The open reader or nothing in error case.</returns>
	''' <remarks>The reader is opened with the CloseConnection option, so when the reader is closed the underlying database connection will also be closed.</remarks>
	Protected Function OpenReader(ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter), Optional ByVal commandType As System.Data.CommandType = CommandType.Text) As SqlClient.SqlDataReader

		Dim Conn As SqlClient.SqlConnection = Nothing

		If m_ExcplicitConnection Is Nothing Then
			' Open a new connection
			Conn = New SqlClient.SqlConnection(DBConnectionstring)
		Else
			' Use the explicit connection
			Conn = m_ExcplicitConnection
		End If

		Dim reader As SqlClient.SqlDataReader

		Try

			If Not IsExplicitConnectionOpen Then
				' Only open connection if its not an explicit connection
				Conn.Open()
			End If

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = commandType

			If IsExplicitTransactionAvailable Then
				' Attach explicit transaction if available
				cmd.Transaction = m_ExplicitTransaction
			End If

			If Not parameters Is Nothing Then
				For Each param In parameters
					cmd.Parameters.Add(param)

				Next
			End If

			If IsExplicitConnectionOpen Then
				reader = cmd.ExecuteReader()
			Else
				' Close connection together with reader close if its not an explicit connection.
				reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
			End If

		Catch e As Exception
			m_Logger.LogError(String.Format("SQL={0}. Exception={1}", sql, e.ToString()))

			If Not IsExplicitConnectionOpen Then
				' Only close local connection.
				Conn.Close()
				Conn.Dispose()
			End If

			reader = Nothing
		End Try

		Return reader
	End Function

	''' <summary>
	''' Fills a data table.
	''' </summary>
	''' <param name="sql">The sql string.</param>
	''' <param name="parameters">The parameters collection.</param>
	''' <param name="commandType">The command type.</param>
	''' <returns>The datatable or nothing in  error case.</returns>
	Protected Function FillDataTable(ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter), Optional ByVal commandType As System.Data.CommandType = CommandType.Text) As DataTable

		Dim dataTable As DataTable = Nothing

		Dim Conn As SqlClient.SqlConnection = Nothing

		If m_ExcplicitConnection Is Nothing Then
			' Open a new connection
			Conn = New SqlClient.SqlConnection(DBConnectionstring)
		Else
			' Use the explicit connection
			Conn = m_ExcplicitConnection
		End If

		Try
			If Not IsExplicitConnectionOpen Then
				' Only open connection if its not an explicit connection
				Conn.Open()
			End If

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = commandType

			If IsExplicitTransactionAvailable Then
				' Attach explicit transaction if available
				cmd.Transaction = m_ExplicitTransaction
			End If

			If Not parameters Is Nothing Then
				For Each param In parameters
					cmd.Parameters.Add(param)

				Next
			End If

			Dim adapter As New SqlDataAdapter(cmd)

			dataTable = New DataTable("result")
			adapter.Fill(dataTable)

		Catch e As Exception
			m_Logger.LogError(String.Format("SQL={0}. Exception={1}", sql, e.ToString()))

			If Not IsExplicitConnectionOpen Then
				' Only close local connection.
				Conn.Close()
				Conn.Dispose()
			End If

			dataTable = Nothing

		End Try

		Return dataTable

	End Function


	''' <summary>
	''' Executes a non query command.
	''' </summary>
	''' <param name="conStr">The connection string.</param>
	''' <param name="sql">The sql string.</param>
	''' <param name="parameters">The parameters.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Protected Function ExecuteNonQuery(ByVal conStr As String, ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter),
																		 Optional ByVal commandType As System.Data.CommandType = CommandType.Text, Optional checkRowCount As Boolean = True) As Boolean

		Dim success As Boolean = True

		Dim Conn As SqlClient.SqlConnection = Nothing

		If m_ExcplicitConnection Is Nothing Then
			' Open a new connection
			Conn = New SqlClient.SqlConnection(DBConnectionstring)
		Else
			' Use the explicit connection
			Conn = m_ExcplicitConnection
		End If

		Try
			If Not IsExplicitConnectionOpen Then
				' Only open connection if its not an explicit connection
				Conn.Open()
			End If

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = commandType

			If IsExplicitTransactionAvailable Then
				' Attach explicit transaction if available
				cmd.Transaction = m_ExplicitTransaction
			End If

			If Not parameters Is Nothing Then
				For Each param In parameters
					cmd.Parameters.Add(param)
					'Trace.WriteLine(String.Format("{0} {1} = {2}", param.ToString(), param.DbType, param.Value))
				Next
			End If

			If (checkRowCount) Then
				success = (cmd.ExecuteNonQuery() > 0)
			Else
				cmd.ExecuteNonQuery()
			End If

		Catch e As Exception
			Trace.WriteLine(e.ToString)
			success = False
			m_Logger.LogError(String.Format("Constring={0} | Geöffnet={1} | SQL={2}. Exception={3}", conStr, Conn.ConnectionString, sql, e.ToString()))

		Finally
			If Not IsExplicitConnectionOpen Then
				' Only close local connection.
				Conn.Close()
				Conn.Dispose()
			End If
		End Try

		Return success

	End Function

	''' <summary>
	''' Executes a non query command (overloaded).
	''' </summary>
	''' <param name="sql">The sql string.</param>
	''' <param name="parameters">The parameters.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Protected Function ExecuteNonQuery(ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter),
																		 Optional ByVal commandType As System.Data.CommandType = CommandType.Text, Optional checkRowCount As Boolean = True) As Boolean

		Return ExecuteNonQuery(DBConnectionstring, sql, parameters, commandType, checkRowCount)
	End Function

	''' <summary>
	''' Executes a scalar command.
	''' </summary>
	''' <param name="sql">The sql string.</param>
	''' <param name="parameters">The parameters.</param>
	''' <returns>Result of execure scalar operation.</returns>
	Protected Function ExecuteScalar(ByVal sql As String, ByVal parameters As IEnumerable(Of SqlParameter), Optional ByVal commandType As System.Data.CommandType = CommandType.Text) As Object
		Dim result As Object = Nothing

		Dim Conn As SqlClient.SqlConnection = Nothing

		If m_ExcplicitConnection Is Nothing Then
			' Open a new connection
			Conn = New SqlClient.SqlConnection(DBConnectionstring)
		Else
			' Use the explicit connection
			Conn = m_ExcplicitConnection
		End If

		Try
			If Not IsExplicitConnectionOpen Then
				' Only open connection if its not an explicit connection
				Conn.Open()
			End If

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
			cmd.CommandType = commandType

			If IsExplicitTransactionAvailable Then
				' Attach explicit transaction if available
				cmd.Transaction = m_ExplicitTransaction
			End If

			If Not parameters Is Nothing Then
				For Each param In parameters
					cmd.Parameters.Add(param)

				Next
			End If

			result = cmd.ExecuteScalar()

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(String.Format("SQL={0}. Exception={1}", sql, e.ToString()))

		Finally
			If Not IsExplicitConnectionOpen Then
				' Only close local connection.
				Conn.Close()
				Conn.Dispose()
			End If
		End Try

		Return result

	End Function

	''' <summary>
	''' Closes a SqlClient.SqlDataReader
	''' </summary>
	''' <param name="reader">The reader.</param>
	Protected Sub CloseReader(ByVal reader As SqlClient.SqlDataReader)

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
	Protected Shared Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

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
	Protected Shared Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

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
	Protected Shared Function SafeGetInteger(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Integer?) As Integer?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetInt32(columnIndex)
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
	Protected Shared Function SafeGetShort(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Short?) As Short?

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
	Protected Shared Function SafeGetByte(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Byte?) As Byte?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetByte(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns a decimal or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetDecimal(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Decimal?) As Decimal?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetDecimal(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns a double or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetDouble(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Double?) As Double?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetDouble(columnIndex)
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
	Protected Shared Function SafeGetDateTime(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As DateTime?) As DateTime?

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
	Protected Shared Function SafeGetByteArray(ByVal reader As SqlDataReader, ByVal columnName As String) As Byte()

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
	Protected Shared Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If

	End Function

#End Region


#Region "Helpers"

	Protected Function GetDbConnectionString(ByVal mandantNumber As Integer, ByVal sputnikRootPath As String) As String
		Dim result As String = String.Empty
		Dim GetFileServerXMLFullFilename = Path.Combine(sputnikRootPath, "Bin\Programm.xml")
		If Not File.Exists(GetFileServerXMLFullFilename) Then
			GetFileServerXMLFullFilename = Path.Combine(My.Settings.SPSEnterpriseFolder, "Bin\Programm.xml")
		End If
		Try
			m_Logger.LogInfo(String.Format("sputnikRootPath: {0} | sputnikRootPath: {1}", My.Settings.SPSEnterpriseFolder, sputnikRootPath))
			m_Logger.LogInfo(String.Format("programm.xml is located on: {0} | mandantNumber: {1} | sputnikRootPath: {2}", GetFileServerXMLFullFilename, mandantNumber, sputnikRootPath))
			If Not File.Exists(GetFileServerXMLFullFilename) Then

				Throw New Exception(String.Format("setting file was not founded: {0}", GetFileServerXMLFullFilename))
			End If
			Dim xDoc As XDocument = XDocument.Load(GetFileServerXMLFullFilename)
			Dim ConfigQuery = (From exportSetting In xDoc.Root.Elements("databaseconnections").Elements(String.Format("md_{0}", mandantNumber))
												 Select New With {.mdconnstr = GetSafeStringFromXElement(exportSetting.Element("connstr")),
													 .mddbname = GetSafeStringFromXElement(exportSetting.Element("dbname")),
													 .mddbserver = GetSafeStringFromXElement(exportSetting.Element("dbname"))
													 }).FirstOrDefault()

			result = FromBase64(ConfigQuery.mdconnstr)


		Catch ex As Exception
			m_Logger.LogError(String.Format("mandantNumber: {0} | sputnikRootPath: {1} | {2}", mandantNumber, sputnikRootPath, ex.ToString()))
			Return result

		End Try


		Return result

	End Function


	Private Function GetSafeStringFromXElement(ByVal xelment As XElement)

		If xelment Is Nothing Then
			Return String.Empty
		Else

			Return xelment.Value
		End If

	End Function

	Private Function FromBase64(ByVal sText As String) As String
		' Base64-String zunächst in ByteArray konvertieren
		Dim nBytes() As Byte = System.Convert.FromBase64String(sText)

		' ByteArray in String umwandeln
		Return System.Text.Encoding.Default.GetString(nBytes)
	End Function


#End Region


End Class



Public Enum Language
	German = 0
	Italian = 1
	French = 2
	English = 3
End Enum
