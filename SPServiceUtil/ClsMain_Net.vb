
''Imports System.IO.File
''Imports System.Threading
''Imports SP.Infrastructure.Logging

'Public Class ClsMain_Net
'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
'	'Dim _clsEventlog As New SPProgUtility.ClsEventLog

'	'Private m_Logger As ILogger = New Logger()

'	'Function GetUserID(ByVal strIDNr As String) As String
'	'	Dim strResult As String = _ClsProgSetting.GetMDGuid
'	'	' "7BDD25A4FBB1A9AA5FA5DD9BC8BB6546EFF712FAE3B8C01E20241B03EC9697F04E2DB80838B3F33C"
'	'	Return strResult
'	'End Function


'#Region "Setzen von Start-Eigenschaften..."

'	'Function SaveUserDataIntoWSDatabase() As String
'	'  Dim strResult As String = SaveLogingintoWSDatabase() ' th_SaveLogingintoWSDatabase() 'SaveLogingintoWSDatabase()

'	'  Return strResult
'	'End Function

'	'''' <summary>
'	'''' Holt alle Länder via Webservice ab
'	'''' </summary>
'	'''' <param name="strCountryCode"></param>
'	'''' <returns></returns>
'	'''' <remarks></remarks>
'	'Function ListAllCountries(ByVal strCountryCode As String) As DataSet
'	'  Dim ds As DataSet = GetCountryList(strCountryCode)

'	'  Return ds
'	'End Function

'	'''' <summary>
'	'''' Holt alle Seco-Berufe via Webservice ab
'	'''' </summary>
'	'''' <param name="strLanguage"></param>
'	'''' <param name="strSex"></param>
'	'''' <returns></returns>
'	'''' <remarks></remarks>
'	'Function ListAllJobs(ByVal strLanguage As String, ByVal strSex As String) As DataSet
'	'   Dim ds As DataSet = GetJobList(strLanguage, strSex)

'	'   Return ds
'	' End Function

'	''''' <summary>
'	''''' Holt alle HBB-Berufe via Webservice ab
'	''''' </summary>
'	''''' <param name="strLanguage"></param>
'	''''' <param name="strSex"></param>
'	''''' <returns></returns>
'	''''' <remarks></remarks>
'	'Function ListAllHBBJobs(ByVal strLanguage As String, ByVal strSex As String) As DataSet
'	'	Dim ds As DataSet = GetHBBJobList(strLanguage, strSex)

'	'	Return ds
'	'End Function

'	''''' <summary>
'	''''' Holt alle BGB-Berufe via Webservice ab
'	''''' </summary>
'	''''' <param name="strLanguage"></param>
'	''''' <param name="strSex"></param>
'	''''' <returns></returns>
'	''''' <remarks></remarks>
'	'Function ListAllBGBJobs(ByVal strLanguage As String, ByVal strSex As String) As DataSet
'	'	Dim ds As DataSet = GetBGBJobList(strLanguage, strSex)

'	'	Return ds
'	'End Function

'	'Function ListAllBGBHBBJobs(ByVal strLanguage As String, ByVal strSex As String) As DataSet
'	'	Dim ds As DataSet = GetBGBHBBJobList(strLanguage, strSex)

'	'	Return ds
'	'End Function

'	'''' <summary>
'	'''' Holt alle Regionen via Webservice ab
'	'''' </summary>
'	'''' <param name="strRegionCode"></param>
'	'''' <param name="strLanguage"></param>
'	'''' <returns></returns>
'	'''' <remarks></remarks>
'	'Function ListAllRegions(ByVal strRegionCode As String, ByVal strLanguage As String) As DataSet
'	'	Dim ds As DataSet = GetRegionsList(strRegionCode, strLanguage)

'	'	Return ds
'	'End Function

'	'Function GAVVersion(ByVal iGAVNr As Integer, ByVal dGAVDate As Date) As List(Of String)
'	'   Dim strInfo As List(Of String) = GetGAVVersionFromWS(iGAVNr, dGAVDate)

'	'   Return strInfo
'	' End Function

'#Region "Funktionen zur Quellensteuer..."

'	'Function IsAllowedQSTCode(ByVal strKanton As String, ByVal iYear As Integer, _
'	'                      ByVal iKinder As Integer, _
'	'                      ByVal strGruppe As String, ByVal strKichensteuer As String, _
'	'                      ByVal strGeschlecht As String) As String
'	'  Dim strResult As String = AllowedQSTData(strKanton, iYear, iKinder, strGruppe, strKichensteuer, strGeschlecht)

'	'  Return strResult
'	'End Function

'	'Function GetQSteuerData(ByVal strKanton As String, ByVal iYear As Integer, _
'	'                        ByVal cEinkommen As Double, ByVal iKinder As Integer, _
'	'                        ByVal strGruppe As String, ByVal strKichensteuer As String, _
'	'                        ByVal strGeschlecht As String) As String()
'	'  Dim strResult As String() = GetQSTInfo(strKanton, iYear, cEinkommen, iKinder, strGruppe, strKichensteuer, strGeschlecht)

'	'  Return strResult
'	'End Function

'	'Function GetQSTData4Code(ByVal strKanton As String, ByVal iYear As Integer, ByVal strGeschlecht As String) As String()
'	'   Dim strResult As String() = GetDataQSTCode(strKanton, iYear, strGeschlecht)

'	'   Return strResult
'	' End Function

'	' Function GetQSTData4Kirchensteuer(ByVal strKanton As String, ByVal iYear As Integer, _
'	'                                   ByVal strGruppe As String, ByVal strGeschlecht As String) As String()
'	'   Dim strResult As String() = GetDataQSTKirchensteuer(strKanton, iYear, strGruppe, strGeschlecht)

'	'   Return strResult
'	' End Function

'	'Function GetQSTData4Kinder(ByVal strKanton As String, ByVal iYear As Integer, _
'	'                            ByVal strGruppe As String, ByVal strGeschlecht As String) As String()
'	'   Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'	'   Dim strResult As String() = GetDataQSTKinder(strKanton, iYear, strGruppe, strGeschlecht)
'	'	'logger.Info(strResult)

'	'	Return strResult
'	' End Function

'#End Region


'	Protected Overrides Sub Finalize()
'    MyBase.Finalize()
'  End Sub

'  Public Sub New()
'    Application.EnableVisualStyles()
'  End Sub

'#End Region

'End Class
