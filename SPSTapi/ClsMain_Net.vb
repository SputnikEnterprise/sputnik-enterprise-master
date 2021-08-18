
Imports System.IO

Imports SPProgUtility.ProgPath
Imports System.Reflection
Imports SP.Infrastructure.Logging

Public Class ClsMain_Net

	Private Shared m_Logger As ILogger = New Logger()

	Public Shared frmTest As frmTapi

	Private Property strNumber As String
	Private Property iMANr As Integer
	Private Property iKDNr As Integer
	Private Property iKDZHDNr As Integer
	Private Property iModulNr As Short
	Private Property iRecID As Integer


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		Dim currentDomain As AppDomain = AppDomain.CurrentDomain

		AddHandler currentDomain.AssemblyResolve, AddressOf MyResolveEventHandler

		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Application.EnableVisualStyles()

	End Sub

	Public Sub New()

		Dim currentDomain As AppDomain = AppDomain.CurrentDomain

		AddHandler currentDomain.AssemblyResolve, AddressOf MyResolveEventHandler

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim _setting = CreateInitialData(m_md.GetDefaultMDNr, m_md.GetDefaultUSNr)
		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Application.EnableVisualStyles()

	End Sub


#End Region

	Function InsertKontaktRec(ByVal strModulName As String,
									ByVal iMANr As Integer,
									ByVal iKDNr As Integer,
									ByVal iKDZHDNr As Integer,
									Optional ByVal iModulNr As Short = 0) As Integer

		Try
			ClsDataDetail.GetMANr = iMANr
			ClsDataDetail.GetKDNr = iKDNr
			ClsDataDetail.GetKDZhdNr = iKDZHDNr
			ClsDataDetail.GetModeNr = iModulNr
			InsertInfoToKontaktDb(strModulName)

			Return 0

		Catch ex As Exception
			Return -1
		End Try

	End Function

	Protected Overrides Sub Finalize()
		MyBase.Finalize()
	End Sub


	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function


#Region "Helpers"

	''' <summary>
	''' This handler is called only when the common language runtime tries to bind to the assembly and fails.        
	''' </summary>
	Private Function MyResolveEventHandler(sender As Object, args As ResolveEventArgs) As Assembly

		'Retrieve the list of referenced assemblies in an array of AssemblyName.
		Dim objExecutingAssemblies As [Assembly]
		objExecutingAssemblies = [Assembly].GetExecutingAssembly()
		Dim arrReferencedAssmbNames() As AssemblyName
		arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies()

		'Loop through the array of referenced assembly names.
		Dim strAssmbName As AssemblyName
		For Each strAssmbName In arrReferencedAssmbNames

			'Look for the assembly names that have raised the "AssemblyResolve" event.
			If (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) = args.Name.Substring(0, args.Name.IndexOf(","))) Then

				'Build the path of the assembly from where it has to be loaded.
				Dim strTempAssmbPath As String = String.Empty
				strTempAssmbPath = IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), args.Name.Substring(0, args.Name.IndexOf(",")) & ".dll")

				If IO.File.Exists(strTempAssmbPath) Then
					Dim msg = String.Format("loading Assembly: {0}", strTempAssmbPath)
					m_Logger.LogWarning(msg)
					Trace.WriteLine(String.Format("loading Assembly: ", strTempAssmbPath))
					Dim MyAssembly As [Assembly]

					'Load the assembly from the specified path. 
					MyAssembly = [Assembly].LoadFrom(strTempAssmbPath)

					'Return the loaded assembly.
					Return MyAssembly
				Else
					Dim msg = String.Format("Assembly could not be found: {0}", strTempAssmbPath)
					m_Logger.LogWarning(msg)
					Trace.WriteLine(msg)
				End If

			End If
		Next

		Return Nothing

	End Function

#End Region

End Class
