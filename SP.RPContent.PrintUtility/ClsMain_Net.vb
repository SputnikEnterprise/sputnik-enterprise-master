

Imports SP.Infrastructure.Logging


Public Class ClsMain_Net

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()


#Region "Constructor"

	Public Sub New(ByVal _setting As ClsRPCSetting)

		ClsDataDetail.ProgSettingData = _setting

		ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
		ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, 0)
		If _setting.SelectedMDNr = 0 Then ClsDataDetail.ProgSettingData.SelectedMDNr = ClsDataDetail.MDData.MDNr
		ClsDataDetail.GetSelectedMDConnstring = ClsDataDetail.MDData.MDDbConn

		If _setting.LogedUSNr = 0 Then ClsDataDetail.ProgSettingData.LogedUSNr = ClsDataDetail.UserData.UserNr

		If ClsDataDetail.ProgSettingData.TranslationItems Is Nothing Then
			ClsDataDetail.ProsonalizedData = ClsDataDetail.ProsonalizedName
			ClsDataDetail.TranslationData = ClsDataDetail.Translation
		Else
			ClsDataDetail.TranslationData = ClsDataDetail.ProgSettingData.TranslationItems
		End If

		Application.EnableVisualStyles()


	End Sub

#End Region

#Region "Startfunktionen..."

	Sub ShowfrmRPContent4Print()
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim frmTest As frmContent

    m_Logger.Loginfo(String.Format("{0}. Modul wird gestartet...", strMethodeName))
    If ClsDataDetail.ProgSettingData.SelectedYear Is Nothing Then ClsDataDetail.ProgSettingData.SelectedYear = New List(Of Integer)(New Integer() {Now.Year})
    If ClsDataDetail.ProgSettingData.SelectedMonth Is Nothing Then ClsDataDetail.ProgSettingData.SelectedMonth = New List(Of Short)(New Short() {Now.Month})

		frmTest = New frmContent(ClsDataDetail.ProgSettingData)
		frmTest.Show()

  End Sub

  Function CreateRPContentFiles() As String
    Dim strResult As String = String.Empty
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    m_Logger.Loginfo(String.Format("{0}. Modul wird gestartet...", strMethodeName))

    If ClsDataDetail.ProgSettingData.SelectedYear Is Nothing Then ClsDataDetail.ProgSettingData.SelectedYear = New List(Of Integer)(New Integer() {Now.Year})
    If ClsDataDetail.ProgSettingData.SelectedMonth Is Nothing Then ClsDataDetail.ProgSettingData.SelectedMonth = New List(Of Short)(New Short() {Now.Month})
    ClsDataDetail.ProgSettingData.PrintCreatedPDFFile = False
    ClsDataDetail.ProgSettingData.FoundedRPNr = ClsDataDetail.ProgSettingData.FoundedRPNr.Distinct.ToList()

		ClsDataDetail.ProgSettingData.ShowMessage = False

		If ClsDataDetail.ProgSettingData.FoundedRPNr.Count > 0 Then
      Dim _clsPDFFill As New FillRPContent.ClsFillRPContent(_frm:=Nothing, _setting:=ClsDataDetail.ProgSettingData)
      strResult = _clsPDFFill.StartFillingRPCFile()
    End If

    Return (strResult)
  End Function

  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub


#End Region

End Class
