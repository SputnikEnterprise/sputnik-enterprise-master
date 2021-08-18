
''Imports System.IO.File
'Imports System.Windows.Forms
'Imports SPProgUtility.SPTranslation

''Imports NLog

'Public Class ClsMain_Net
'	'Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

'#Region "Startfunktionen..."

'	'Function ShowfrmQualifications(ByVal bSelectMulti As Boolean, ByVal strSex As String) As String
'	'	Dim strResult As String = String.Empty
'	'	Dim init = CreateInitialData(0, 0)

'	'	Dim frmTest = New frmQualification(init)
'	'	frmTest.SelectMultirecords = bSelectMulti
'	'	Dim result = frmTest.LoadQualificationData(strSex)
'	'	If Not result Then Return String.Empty

'	'	frmTest.ShowDialog()
'	'	strResult = frmTest.GetSelectedData

'	'	Return strResult
'	'End Function

'	'Function ShowfrmBranches(ByVal bSelectMulti As Boolean) As String
'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'	Dim strResult As String = String.Empty

'	'	ClsDataDetail.AllowedMultiSelect = bSelectMulti
'	'	Dim frmTest As Form = New frmBranches
'	'	frmTest.ShowDialog()
'	'	strResult = ClsDataDetail.GetReturnValue

'	'	Return strResult
'	'End Function

'	Protected Overrides Sub Finalize()
'		MyBase.Finalize()
'	End Sub

'	Public Sub New()

'		Application.EnableVisualStyles()

'		ModulConstants.MDData = ModulConstants.SelectedMDData(0)
'		ModulConstants.UserData = ModulConstants.LogededUSData(ModulConstants.MDData.MDNr, 0)

'		ModulConstants.ProsonalizedData = ModulConstants.ProsonalizedValues
'		ModulConstants.TranslationData = ModulConstants.TranslationValues

'	End Sub

'#End Region


'	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

'		Dim m_md As New SPProgUtility.Mandanten.Mandant
'		Dim clsMandant = ModulConstants.MDData
'		Dim logedUserData = ModulConstants.UserData
'		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

'		Dim clsTransalation As New ClsTranslation
'		Dim translate = clsTransalation.GetTranslationInObject

'		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

'	End Function

'End Class
