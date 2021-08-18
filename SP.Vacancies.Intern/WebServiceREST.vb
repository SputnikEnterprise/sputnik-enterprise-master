
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text

Imports System.Threading
Imports System.Threading.Tasks

Public Class clsWebserviceProcess

End Class

'	Public Function webserviceResponse(ByVal sb As StringBuilder, ByVal baseUri As Uri, ByVal Method As String, ByVal User As String, ByVal Password As String) As Task(Of HttpResponseMessage)

'		Dim client As HttpClient = New HttpClient()
'		client.BaseAddress = baseUri

'		If String.IsNullOrEmpty(User) Then

'			Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue("None")
'			client.DefaultRequestHeaders.Authorization = authHeader

'		Else

'			Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(String.Format("{0}:{1}", User, Password))))
'			client.DefaultRequestHeaders.Authorization = authHeader

'		End If

'		Dim timeout As TimeSpan = TimeSpan.FromMinutes(5)
'		client.Timeout = timeout

'		Dim content As New StringContent(sb.ToString, System.Text.Encoding.UTF8, "application/json")

'		Dim resp 'As HttpResponseMessage = Nothing
'		Dim cancellationToken As CancellationToken

'		If Method = "Post" Then
'			'resp = Await client.PostAsync(baseUri, content, cancellationToken)
'			resp = client.PostAsync(baseUri, content, cancellationToken)

'		ElseIf Method = "Put" Then
'			resp = client.PutAsync(baseUri, content, cancellationToken)

'		ElseIf Method = "Get" Then
'			resp = client.GetAsync(baseUri, cancellationToken)

'		End If


'		Dim clsGlobalVariables_1 As New clsGlobalVariables
'		clsGlobalVariables_1.httpStatus = CStr(resp.Status)

'		If (resp.StatusCode <> HttpStatusCode.OK AndAlso resp.StatusCode <> HttpStatusCode.NoContent AndAlso resp.StatusCode <> HttpStatusCode.Created) Then

'			If clsGlobalVariables_1.GVarMatchportalUsrID = 2 Then

'				If resp.StatusCode = HttpStatusCode.NotFound Then
'					Trace.WriteLine("Der Datensatz existiert nicht!", "Fehler bei Webservice-Vorgang!")

'				Else
'					Trace.WriteLine(String.Format("{0} | Fehler bei Webservice-Vorgang!", resp.ToString))

'				End If

'			End If

'			Return resp

'			Exit Function

'		End If

'		'WriteLine(resp.StatusCode)
'		'WriteLine(resp.ToString)

'		Return resp

'	End Function


'End Class


'Public Class clsGlobalVariables
'	Public Property httpStatus As String
'	Public Property GVarMatchportalUsrID As Integer

'End Class
