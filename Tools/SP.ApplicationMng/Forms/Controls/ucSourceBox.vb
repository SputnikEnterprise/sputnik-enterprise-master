Imports System.Net
Imports System.Threading
Imports System.Collections.Specialized

''' <summary>
''' User Control für SourceBox Fernsteuerung via WebBrowser Control
''' </summary>
''' <remarks></remarks>
Public Class ucSourceBox

#Region "Public Methods"

  Public Sub New()
    ' Dieser Aufruf ist für den Designer erforderlich.
    InitializeComponent()

		ctrWebBrowser.Visible = False
		lblJSessionId.Text = String.Empty
  End Sub

  ''' <summary>
  ''' Meldet einen Benutzer an SourceBox an.
  ''' </summary>
  ''' <param name="account"></param>
  ''' <param name="username"></param>
  ''' <param name="password"></param>
  ''' <returns>True wenn erfolgreich.</returns>
  ''' <remarks></remarks>
  Public Function Login(account As String, username As String, password As String) As Boolean
    m_jSessionId = Nothing
    lblJSessionId.Text = String.Empty

    Dim uriBuilder = New UriBuilder(TEXTKERNEL_URI_LOGIN)
    Dim queryParams As NameValueCollection = New NameValueCollection()
    With queryParams
      .Add(TEXTKERNEL_QUERYPARAM_ACCOUNT, account)
      .Add(TEXTKERNEL_QUERYPARAM_USERNAME, username)
      .Add(TEXTKERNEL_QUERYPARAM_PASSWORD, password)
    End With
    uriBuilder.Query = HttpHelper.NvcToQueryString(queryParams)

    ctrWebBrowser.Navigate(uriBuilder.Uri)

    Dim maxTimeUtc = DateTime.UtcNow.AddMilliseconds(TEXTKERNEL_TIMEOUT_MS)
    While m_jSessionId Is Nothing AndAlso DateTime.UtcNow < maxTimeUtc
      ' Warten bis m_jSessionId in ctrWebBrowser_DocumentCompleted gesetzt ist
      Application.DoEvents()
      'Thread.Sleep(10)
      'HttpHelper.Delay(10.0R)
    End While
		Return Not String.IsNullOrWhiteSpace(m_jSessionId)
	End Function

	Public Sub Logout()
		ctrWebBrowser.Navigate(TEXTKERNEL_URI_LOGOUT)
		m_jSessionId = Nothing
		lblJSessionId.Text = String.Empty
		Refresh()
	End Sub

	Public Function LoadCv(trxmlId As String) As Boolean
		Dim success As Boolean

		ctrWebBrowser.Visible = False
		If Not String.IsNullOrWhiteSpace(m_jSessionId) Then
			Dim uriBuilder = New UriBuilder(TEXTKERNEL_URI_LOADTRXML)
			Dim queryParams As NameValueCollection = New NameValueCollection()
			With queryParams
				.Add(TEXTKERNEL_QUERYPARAM_JSESSIONID, m_jSessionId)
				.Add(TEXTKERNEL_QUERYPARAM_TRXMLID, trxmlId)
			End With
			uriBuilder.Query = HttpHelper.NvcToQueryString(queryParams)

			ctrWebBrowser.Navigate(uriBuilder.Uri)

			ctrWebBrowser.Visible = True

			success = True
		End If

		Return success
	End Function

	Public Function LoadCvOneFrame() As Boolean
		Dim success As Boolean
		'Return True
		Dim maxTimeUtc = DateTime.UtcNow.AddMilliseconds(TEXTKERNEL_TIMEOUT_MS)
		While m_jSessionId Is Nothing AndAlso DateTime.UtcNow < maxTimeUtc
			Application.DoEvents()
		End While

		If Not String.IsNullOrWhiteSpace(m_jSessionId) Then
			Do While True
				Application.DoEvents()
				If ctrWebBrowser.ReadyState = WebBrowserReadyState.Complete Then
					ctrWebBrowser.Navigate(TEXTKERNEL_URI_VIEWSTRUCTURE)

					Exit Do
				End If
				Refresh()
			Loop

			success = True
		End If
		ctrWebBrowser.Visible = True

		Return success
	End Function

	Public Function SaveCv()
		Dim success As Boolean

		If Not String.IsNullOrWhiteSpace(m_jSessionId) Then
			' Warten bis m_jSessionId in ctrWebBrowser_DocumentCompleted gesetzt ist
			ctrWebBrowser.Navigate(TEXTKERNEL_URI_STOREDATA)

			success = True
		End If


		Return success
	End Function

	''' <summary>
	''' Cleanup control.
	''' </summary>
	Public Sub CleanUp()
		Logout()
	End Sub

#End Region

#Region "Public Properties"

	''' <summary>
	''' Liefert die JSessionId.
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property JSessionId As String
		Get
			Return m_jSessionId
		End Get
	End Property

#End Region

#Region "Private Methods"

	Private Sub ctrWebBrowser_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles ctrWebBrowser.DocumentCompleted
		Debug.WriteLine(e.Url)
		If (String.IsNullOrWhiteSpace(m_jSessionId)) Then
			Dim cookie As String = ctrWebBrowser.Document.Cookie
			Debug.WriteLine(cookie)
			If Not cookie Is Nothing Then
				Dim cookieItemArray As String() = cookie.Split(";")
				For Each cookieItem In cookieItemArray
					Dim cookieParts As String() = cookieItem.Split("=")
					Select Case cookieParts(0).Trim()
						Case TEXTKERNAL_COOKIE_JSESSIONID
							m_jSessionId = cookieParts(1).Trim()
							lblJSessionId.Text = m_jSessionId
							Exit For
					End Select
				Next
			End If
		End If
		Refresh()
	End Sub

#End Region

#Region "Private Constants"

  Private Const TEXTKERNEL_URI_BASE As String = "https://staging.textkernel.nl/match/"
  Private Const TEXTKERNEL_URI_LOGIN As String = TEXTKERNEL_URI_BASE + "loginUser.do"
  Private Const TEXTKERNEL_URI_LOGOUT As String = TEXTKERNEL_URI_BASE + "logout.jsp"
  Private Const TEXTKERNEL_URI_LOADTRXML As String = TEXTKERNEL_URI_BASE + "loadTrxmlIntoSession.do"
  Private Const TEXTKERNEL_URI_VIEWSTRUCTURE As String = TEXTKERNEL_URI_BASE + "view-structure.jsp"
	Private Const TEXTKERNEL_URI_STOREDATA As String = TEXTKERNEL_URI_BASE + "storeDocument.do?buttonPressed=Store"

  Private Const TEXTKERNEL_QUERYPARAM_ACCOUNT As String = "account"
  Private Const TEXTKERNEL_QUERYPARAM_USERNAME As String = "username"
  Private Const TEXTKERNEL_QUERYPARAM_PASSWORD As String = "password"
  Private Const TEXTKERNEL_QUERYPARAM_JSESSIONID As String = "jsessionid"
  Private Const TEXTKERNEL_QUERYPARAM_TRXMLID As String = "trxmlid"

  Private Const TEXTKERNAL_COOKIE_JSESSIONID As String = "JSESSIONID"

  Private Const TEXTKERNEL_TIMEOUT_MS As Double = 5000.0R

#End Region

#Region "Private Fields"

  Private m_jSessionId As String

#End Region

  ' TODO: remove button
  Private Sub btnLoadCv_Click(sender As Object, e As EventArgs) Handles btnLoadCv.Click
    Dim success = Login("username", "username", "vcge6123")
    If success Then
      LoadCv("49964204")
    End If
  End Sub

  Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
    Logout()
  End Sub

  Private Sub btnSaveCv_Click(sender As Object, e As EventArgs) Handles btnSaveCv.Click
		SaveCv()
  End Sub

  Private Sub btnViewStructure_Click(sender As Object, e As EventArgs) Handles btnViewStructure.Click
    ctrWebBrowser.Navigate(TEXTKERNEL_URI_VIEWSTRUCTURE)
	End Sub


End Class
