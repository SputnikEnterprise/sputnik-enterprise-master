
Imports System.IO.File
Imports System.ComponentModel
Imports System.IO
Imports SP.Infrastructure.Logging


Public Class ClsMain_Net


	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	Public Shared frmTest As frmStammDaten

	Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private Property SelectedMANr As New List(Of Integer)
	Private Property SelectedKDNr As New List(Of Integer)
	Private Property SelectedKDZHDNr As New List(Of Integer)

	Private Property SelectedMALang As New List(Of String)
	Private Property SelectedKDLang As New List(Of String)

	Private _LocSetting As New ClsStammDatenSetting


#Region "Startfunktionen..."

	Private ReadOnly Property GetSQL2Open() As String
		Get
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

			Dim strWhereQuery As String = String.Empty
			Dim strESNr As String = String.Empty
			Dim strSqlQuery As String = String.Empty
			Try
				'strSqlQuery = "SELECT ES.ID, ES.ESNr, ES.MANr, ES.KDNr, ES.ES_Als, ES.KDZHDNr, ESL.GAVGruppe0, MA.Send2WOS As MAWOS, MA.Sprache As MASprache, "
				'strSqlQuery += "(Convert(nvarchar(10), ES.ES_Ab, 104) + ' - ' + IsNull(convert(nvarchar(10), ES.ES_Ende, 104), '')) As Zeitraum, "
				'strSqlQuery += "(MA.Nachname + ', ' + MA.Vorname) As MAName, "
				'strSqlQuery += "KD.Firma1, KD.Send2WOS As KDWOS, KD.Sprache As KDSprache "
				'strSqlQuery += "FROM ES "
				'strSqlQuery += "Left Join ESLohn ESL On ES.ESNr = ESL.ESNr And ESL.Aktivlodaten = 1 "
				'strSqlQuery += "Left Join Mitarbeiter MA On ES.MANr = MA.MANr "
				'strSqlQuery += "Left Join Kunden KD On ES.KDNr = KD.KDNr "
				'If Not String.IsNullOrWhiteSpace(strWhereQuery) Then strWhereQuery = String.Format("Where {0} ", strWhereQuery)
				'strSqlQuery = String.Format("{0} {1} Order By MA.Nachname ASC, MA.Vorname ASC, ES.ES_Ab ", strSqlQuery, strWhereQuery)

			Catch ex As Exception
				m_Logger.LogInfo(String.Format("{0}:{1}", strMethodeName, ex.Message))

			End Try
			Return strSqlQuery

		End Get

	End Property


	Sub ShowfrmStammDaten4Print()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		m_Logger.LogInfo(String.Format("{0}. Modul wird gestartet...", strMethodeName))
		If _LocSetting.SearchMANr Is Nothing Then _LocSetting.SearchMANr = New List(Of Integer)(New Integer() {0})
		If _LocSetting.SearchKDNr Is Nothing Then _LocSetting.SearchKDNr = New List(Of Integer)(New Integer() {0})
		If _LocSetting.SearchVakNr Is Nothing Then _LocSetting.SearchVakNr = New List(Of Integer)(New Integer() {0})
		If _LocSetting.SearchProposeNr Is Nothing Then _LocSetting.SearchProposeNr = New List(Of Integer)(New Integer() {0})
		_LocSetting.SelectedDocArt = ClsStammDatenSetting.DocArt.Kandidat	 ' ClsStammDatenSetting.DocArt.Kandidat

		frmTest = New frmStammDaten(_LocSetting)
		frmTest.Show()

	End Sub

	''' <summary>
	''' Druckt von Stammblätter...
	''' </summary>
	''' <remarks></remarks>
	Function PrintSelectedMAStamm() As String
		Dim strResult As String = "Success..."

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMASearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetDbConnString, _
																																									 .JobNr2Print = "1.0", _
																																									.ShowAsDesign = False, _
																																									.liMANr2Print = _LocSetting.SearchMANr}
		Dim obj As New SPS.Listing.Print.Utility.MAStammblatt.ClsPrintMAStammblatt(_Setting)
		If _LocSetting.PrintAsExport Then
			strResult = obj.ExportMAStammBlatt()

		Else
			strResult = obj.PrintMAStammBlatt()

		End If

		Return strResult
	End Function

	Function PrintSelectedKDStamm() As String
		Dim strResult As String = "Success..."
		Try

			Dim _Setting As New SPS.Listing.Print.Utility.ClsLLKDSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetDbConnString, _
																																										 .JobNr2Print = "2.0", _
																																										.ShowAsDesign = False, _
																																										.liKDNr2Print = _LocSetting.SearchKDNr}
			Dim obj As New SPS.Listing.Print.Utility.KDStammblatt.ClsPrintKDStammblatt(_Setting)
			If _LocSetting.PrintAsExport Then
				strResult = obj.ExportKDStammBlatt()

			Else
				strResult = obj.PrintKDStammBlatt()

			End If

		Catch ex As Exception

		Finally


		End Try

		Return strResult
	End Function

	Function PrintSelectedVakStamm(ByVal _liRecNr As List(Of Integer), ByVal _bPrintAsExport As Boolean) As String
		Dim strResult As String = "Success..."

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMASearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetDbConnString, _
																																									 .JobNr2Print = "19.0", _
																																									.ShowAsDesign = False, _
																																									.liMANr2Print = _liRecNr}
		Dim obj As New SPS.Listing.Print.Utility.MAStammblatt.ClsPrintMAStammblatt(_Setting)
		If _bPrintAsExport Then
			strResult = obj.ExportMAStammBlatt()

		Else
			strResult = obj.PrintMAStammBlatt()

		End If

		Return strResult
	End Function

	Function PrintSelectedProposeStamm(ByVal _liRecNr As List(Of Integer), ByVal _bPrintAsExport As Boolean) As String
		Dim strResult As String = "Success..."

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLMASearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetDbConnString, _
																																									 .JobNr2Print = "18.0", _
																																									.ShowAsDesign = False, _
																																									.liMANr2Print = _liRecNr}
		Dim obj As New SPS.Listing.Print.Utility.MAStammblatt.ClsPrintMAStammblatt(_Setting)
		If _bPrintAsExport Then
			strResult = obj.ExportMAStammBlatt()

		Else
			strResult = obj.PrintMAStammBlatt()

		End If

		Return strResult
	End Function


	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub

	Public Sub New(ByVal _Mysetting As ClsStammDatenSetting)

		Application.EnableVisualStyles()

		_LocSetting = _Mysetting

	End Sub

#End Region

End Class
