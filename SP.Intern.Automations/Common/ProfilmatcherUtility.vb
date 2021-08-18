
Imports System.ComponentModel
Imports System.Text
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web
Imports DevExpress.XtraEditors
Imports System.Threading
Imports System.IO
Imports System.Threading.Tasks



Namespace X28


	Public Class ProfilmatcherQueryData


		Private m_ValidationErrors As StringBuilder = New StringBuilder()

		Public Property ID As Integer
		Public Property CustomerID As String
		Public Property UserID As String
		Public Property Terms As List(Of String)
		Public Property Companies As List(Of String)
		Public Property Companysizes As List(Of String)
		Public Property Industries As List(Of String)
		Public Property Recruitmentagencies As Boolean
		Public Property Management As Boolean
		Public Property Temporary As Boolean
		Public Property Regions As List(Of String)
		Public Property Locations As List(Of ProfilMatcherLocationData)
		Public Property LocationDistance As String

		Public Property Clusters As List(Of String)
		Public Property Experiences As List(Of String)
		Public Property Educations As List(Of String)
		Public Property Skills As List(Of String)
		Public Property WorkquotaMinimum As Integer
		Public Property WorkquotaMaximum As Integer
		Public Property DateFrom As DateTime?
		Public Property DateTo As DateTime?
		Public Property Page As Integer?
		Public Property Size As Integer?
		Public Property CreatedOn As DateTime
		Public Property QueryContent As String


		''' <summary>
		''' Gets boolean flag indicating if the data is valid for xml export.
		''' </summary>
		Public ReadOnly Property IsDataValidForXml As Boolean
			Get

				m_ValidationErrors.Clear()

				Dim valid = True

				Return valid

			End Get

		End Property

		''' <summary>
		''' Gets the validation errors.
		''' </summary>
		''' <returns>The validation errors.</returns>
		Public ReadOnly Property ValidationErrors As String
			Get
				Return m_ValidationErrors.ToString()
			End Get
		End Property


	End Class

	Public Class ProfilMatcherLocationData
		Public Property Location As String
		Public Property LocationDistances As Integer

	End Class

	Public Class ProfilmatcherQueryResultData

		Public Property CustomerID As String

		Public Property Status As String
		Public Property Total As Integer
		Public Property Page As Integer
		Public Property Size As Integer
		Public Property ResultContent As String
		Public Property Jobs As List(Of ProfilmatcherQueryJob)

	End Class

	Public Class ProfilmatcherQueryJob
		Public Property ID As Long
		Public Property Title As String
		Public Property Location As String
		Public Property Company As String
		Public Property URL As String
		Public Property DateCreated As DateTime

	End Class


	Public Class ProfilmatcherWebserviceResult

		Public Property HttpState As String
		Public Property APIResponse As String
		Public Property APIResult As String

	End Class




	Public Class Profilmatcher


		Private Const X28_RM_WEB_REQUEST_URL As String = "https://api.x28.ch/search/api/jobs/xml"

		Private Const X28_RM_API_USERNAME As String = "username"
		Private Const X28_RM_API_USERPASSWORD As String = "password"

		Private httpStatus As String



		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
		Private m_APIResponse As String
		Private m_APIResult As String
		Private m_fileGuid As String
		Private m_ResultContent As String


#Region "Public properties"

		Public Property PMQueryStringToSend As String

#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_InitializationData = _setting
			m_fileGuid = Guid.NewGuid.ToString

		End Sub


#End Region

		Public ReadOnly Property GetPMAPIResponseData() As String
			Get
				Return m_APIResponse
			End Get
		End Property

		Public ReadOnly Property GetPMAPIResultData() As String
			Get
				Return m_APIResult
			End Get
		End Property

		Public ReadOnly Property GetPMAPIResultContent() As String
			Get
				Return m_ResultContent
			End Get
		End Property

		Public Function BuildPMQueryString(ByVal customerID As String, ByVal userID As String, ByVal queryData As ProfilmatcherQueryData) As String

			Dim result As String = String.Empty

			Try
				result = String.Empty

				'Dim vacanciesGenerator As New Profilmatcher()
				Dim xDoc = GenerateProfilmatcherXml(queryData)
				SaveXmlDocument(xDoc)

				result = xDoc.ToString()


			Catch ex As Exception
				Dim msgContent = String.Format("{0}", ex.ToString)
				'm_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.customerID = m_customerID, .CreatedFrom = userID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "BuildPMQueryString", .MessageContent = msgContent})
			Finally
			End Try


			' Return search data as an array.
			Return (result)

		End Function

		'Public Function LoadPMQueryResultData(ByVal customerID As String, ByVal userID As String, ByVal queryData As ProfilmatcherQueryData) As ProfilmatcherQueryResultData

		'	Dim result As ProfilmatcherQueryResultData = Nothing

		'	Try
		'		result = New ProfilmatcherQueryResultData
		'		Dim xmlQueryFile = BuildQueryFileName(queryData)

		'		Dim content As String = ""
		'		Using textReader As New System.IO.StreamReader(xmlQueryFile)
		'			content = textReader.ReadToEnd
		'		End Using
		'		Dim wsResult = LoadWebserviceProcess(content)
		'		Dim xmlData = New ParsX28ProfilMatcherXMLData(wsResult.APIResult) 'xmlQueryFile)

		'		result = xmlData.LoadProfilMatcherResultData()


		'	Catch ex As Exception
		'		Dim msgContent = ex.ToString
		'		'm_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.customerID = m_customerID, .CreatedFrom = userID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPMQueryData", .MessageContent = msgContent})
		'	Finally
		'	End Try


		'	' Return search data as an array.
		'	Return (result)

		'End Function

		Public Function LoadPMQueryResultObjectData(ByVal customerID As String, ByVal userID As String, ByVal queryString As String, ByVal resultXMLContent As String) As ProfilmatcherQueryResultData

			Dim result As ProfilmatcherQueryResultData = Nothing

			Try
				'result = New ProfilmatcherQueryResultData
				'Dim xmlQueryFile = BuildQueryFileName(queryData)

				'Dim content As String = ""
				'Using textReader As New System.IO.StreamReader(xmlQueryFile)
				'	content = textReader.ReadToEnd
				'End Using
				'Dim wsResult = LoadWebserviceProcess(queryString)
				'Dim xmlData = New ParsX28ProfilMatcherXMLData(wsResult.APIResult) 'xmlQueryFile)
				'result = xmlData.LoadProfilMatcherResultData()
				'result.ResultContent = wsResult.APIResult

				Dim xmlData = New ParsX28ProfilMatcherXMLData(resultXMLContent)
				result = xmlData.LoadProfilMatcherResultData()
				result.ResultContent = resultXMLContent

			Catch ex As Exception
				Dim msgContent = ex.ToString
				'm_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.customerID = m_customerID, .CreatedFrom = userID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPMQueryData", .MessageContent = msgContent})
			Finally
			End Try


			' Return search data as an array.
			Return (result)

		End Function

		Public Function LoadPMQueryResultDa(ByVal customerID As String, ByVal userID As String, ByVal queryData As ProfilmatcherQueryData) As String

			Dim result As String = String.Empty

			Try
				result = String.Empty

				'Dim vacanciesGenerator As New Profilmatcher()
				Dim xDoc = GenerateProfilmatcherXml(queryData)
				SaveXmlDocument(xDoc)

				result = xDoc.ToString()


			Catch ex As Exception
				Dim msgContent = String.Format("{0}", ex.ToString)
				'm_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.customerID = m_customerID, .CreatedFrom = userID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "BuildPMQueryString", .MessageContent = msgContent})
			Finally
			End Try


			' Return search data as an array.
			Return (result)

		End Function

		Public Function PostProfilmatcherSearchAPI() As ProfilmatcherWebserviceResult
			Dim baseUri As Uri = New Uri(X28_RM_WEB_REQUEST_URL)

			If PMQueryStringToSend Is Nothing OrElse String.IsNullOrWhiteSpace(PMQueryStringToSend) Then Return Nothing
			'Dim response As HttpWebResponse
			Dim result As ProfilmatcherWebserviceResult = New ProfilmatcherWebserviceResult
			m_APIResponse = String.Empty
			m_APIResult = String.Empty
			m_ResultContent = String.Empty

			'response = clsWebserviceProcess(PMQueryStringToSend, baseUri, "Post", X28_RM_API_USERNAME, X28_RM_API_USERPASSWORD)
			result = LoadWebserviceProcess(PMQueryStringToSend)

			'Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)

			'Dim xmlToDeserialize As String = myStreamReader.ReadToEnd()

			'm_APIResponse = response.ToString
			'm_APIResult = xmlToDeserialize

			'' saving result into a file
			'Dim ostJobChRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
			'Dim organsiationRootPath = System.IO.Path.Combine(ostJobChRoot, "00000")
			'Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, "pmQueryResult.xml")
			'Using sw As StreamWriter = New StreamWriter(xmlFileName, False, Encoding.UTF8)
			'	sw.Write(xmlToDeserialize)
			'	sw.Close()
			'End Using


			Return (result)

		End Function

		'Public Function clsWebserviceProcess(ByVal sb As String, ByVal baseUri As Uri, ByVal Method As String, ByVal User As String, ByVal Password As String) As HttpWebResponse

		'	Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue(
		'			"Basic",
		'			Convert.ToBase64String(
		'					System.Text.ASCIIEncoding.ASCII.GetBytes(
		'							String.Format("{0}:{1}", User, Password))))


		'	Dim xmlBytes() As Byte = System.Text.Encoding.UTF8.GetBytes(sb)

		'	Dim req As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(baseUri), System.Net.HttpWebRequest)
		'	req.Method = "POST"
		'	req.ContentType = "application/xml"
		'	req.Headers.Add("Authorization", authHeader.ToString)

		'	req.ContentLength = xmlBytes.Length
		'	Dim post As System.IO.Stream = req.GetRequestStream
		'	post.Write(xmlBytes, 0, xmlBytes.Length)

		'	Dim result As String = Nothing
		'	Dim resp As System.Net.HttpWebResponse = CType(req.GetResponse, System.Net.HttpWebResponse)
		'	Dim reader As System.IO.StreamReader = New System.IO.StreamReader(resp.GetResponseStream)
		'	result = reader.ReadToEnd
		'	reader.Close()

		'	httpStatus = CStr(resp.StatusCode)

		'	m_APIResponse = httpStatus
		'	m_APIResult = result

		'	Dim organsiationRootPath = System.IO.Path.Combine(m_InitializationData.UserData.spAllowedPath, m_InitializationData.MDData.MDGuid)
		'	Dim resultXmlFileName = System.IO.Path.Combine(organsiationRootPath, String.Format("QueryResult_{0}.xml", m_fileGuid))
		'	Using sw As StreamWriter = New StreamWriter(resultXmlFileName, False, Encoding.UTF8)
		'		sw.Write(m_APIResult)
		'		sw.Close()
		'	End Using
		'	m_APIResult = result

		'	If (resp.StatusCode <> HttpStatusCode.OK AndAlso resp.StatusCode <> HttpStatusCode.NoContent AndAlso resp.StatusCode <> HttpStatusCode.Created) Then

		'		XtraMessageBox.Show(resp.ToString, "Fehler bei Webservice-Vorgang!", MessageBoxButtons.OK, MessageBoxIcon.Error)

		'		Return resp

		'		Exit Function

		'	End If

		'	Return resp

		'End Function



#Region "private methodes"

		Private Function BuildQueryFileName(ByVal queryData As ProfilmatcherQueryData) As String
			Dim result As String = String.Empty

			Try
				'Dim vacanciesGenerator As New Profilmatcher()
				Dim xDoc = GenerateProfilmatcherXml(queryData)

				result = SaveXmlDocument(xDoc)


			Catch ex As Exception
				Dim msgContent = ex.ToString
				'm_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "BuildQueryFileName", .MessageContent = msgContent})
			Finally
			End Try


			Return result

		End Function

		Protected Function SaveXmlDocument(ByVal xDoc As XDocument) As String

			Dim organsiationRootPath = System.IO.Path.Combine(m_InitializationData.UserData.spAllowedPath, m_InitializationData.MDData.MDGuid)
			Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, String.Format("QueryString_{0}.xml", m_fileGuid))

			Try
				If File.Exists(xmlFileName) Then File.Delete(xmlFileName)
			Catch ex As Exception
				Threading.Thread.Sleep(500)
				File.Delete(xmlFileName)
			End Try

			If Not Directory.Exists(organsiationRootPath) Then
				Directory.CreateDirectory(organsiationRootPath)
			End If

			Dim xmlCode As String = String.Empty

			Using sw As StringWriter = New StringWriter()
				xDoc.Save(sw)
				xmlCode = sw.ToString()
			End Using


			Try
				xDoc.Save(xmlFileName)
			Catch ex As Exception
				' Maybe the file is currently in use -> wait a little bit.
				Threading.Thread.Sleep(500)
				xDoc.Save(xmlFileName)
			End Try


			Return xmlFileName
		End Function

		Private Function LoadWebserviceProcess(ByVal sb As String) As ProfilmatcherWebserviceResult ' HttpWebResponse
			Dim baseUri As Uri = New Uri(X28_RM_WEB_REQUEST_URL)
			Dim result = New ProfilmatcherWebserviceResult

			result.APIResponse = String.Empty
			result.APIResult = String.Empty

			Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(
						System.Text.ASCIIEncoding.ASCII.GetBytes(
								String.Format("{0}:{1}", X28_RM_API_USERNAME, X28_RM_API_USERPASSWORD))))

			Dim xmlBytes() As Byte = System.Text.Encoding.UTF8.GetBytes(sb)

			Dim req As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(baseUri), System.Net.HttpWebRequest)
			req.Method = "POST"
			req.ContentType = "application/xml"
			req.Headers.Add("Authorization", authHeader.ToString)

			req.ContentLength = xmlBytes.Length
			Dim post As System.IO.Stream = req.GetRequestStream
			post.Write(xmlBytes, 0, xmlBytes.Length)

			Dim value As String = Nothing
			Dim resp As System.Net.HttpWebResponse = CType(req.GetResponse, System.Net.HttpWebResponse)
			Dim reader As System.IO.StreamReader = New System.IO.StreamReader(resp.GetResponseStream)
			value = reader.ReadToEnd
			reader.Close()

			result.HttpState = CStr(resp.StatusCode)

			result.APIResponse = result.HttpState
			result.APIResult = value

			If (resp.StatusCode <> HttpStatusCode.OK AndAlso resp.StatusCode <> HttpStatusCode.NoContent AndAlso resp.StatusCode <> HttpStatusCode.Created) Then
				result.APIResult = String.Empty
				result.APIResponse = resp.StatusCode

				Return result
			End If


			Return result

		End Function


#End Region



	End Class


End Namespace