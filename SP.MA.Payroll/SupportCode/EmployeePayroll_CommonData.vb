Imports SP.DatabaseAccess.PayrollMng.DataObjects
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI
Imports SP.Infrastructure
Imports SP.DatabaseAccess.Employee
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath
Imports SP.DatabaseAccess.PayrollMng
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.DatabaseAccess.Employee.DataObjects.Salary
Imports SP.DatabaseAccess.AdvancePaymentMng
Imports SP.Infrastructure.Misc
Imports SP.DatabaseAccess

Partial Public Class EmployeePayroll_CommonData


#Region "Private Fields"

	Private Shared m_Logger As ILogger = New Logger()
	Private m_UtilityUI As UtilityUI
	Private m_Utility As Utility
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_PayrollDatabaseAccess As IPayrollDatabaseAccess

	Private m_md As Mandant
	Private m_ProgPath As ClsProgPath
	Private m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Private m_LAData_Verwendung_0 As List(Of LAData)
	Private m_LAData_Verwendung_1_3 As List(Of LAData)
	Private m_LAData_Verwendung_2_3 As List(Of LAData)
	Private m_LAData_Verwendung_4 As List(Of LAData)

	Private m_MandantData As MandantData

	Private m_LABezDataLookup As Dictionary(Of Decimal, LA_BezData)

#End Region

#Region "Constructor"
	Public Sub New(ByVal mdNr As Integer, ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try
			m_md = New Mandant
			m_ProgPath = New ClsProgPath
			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

		m_UtilityUI = New SP.Infrastructure.UI.UtilityUI
		m_Utility = New Utility

		Dim conStr = m_md.GetSelectedMDData(mdNr).MDDbConn

		m_PayrollDatabaseAccess = New DatabaseAccess.PayrollMng.PayrollDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
	End Sub
#End Region

#Region "Public Properties"

	Public ReadOnly Property LAData_Verwendung_0 As List(Of LAData)
		Get
			Return m_LAData_Verwendung_0
		End Get
	End Property

	Public ReadOnly Property LAData_Verwendung_1_3 As List(Of LAData)
		Get
			Return m_LAData_Verwendung_1_3
		End Get
	End Property

	Public ReadOnly Property LAData_Verwendung_2_3 As List(Of LAData)
		Get
			Return m_LAData_Verwendung_2_3
		End Get
	End Property


	Public ReadOnly Property LAData_Verwendung_4 As List(Of LAData)
		Get
			Return m_LAData_Verwendung_4
		End Get
	End Property

#End Region

#Region "Public Methods"

	Public Function LoadLAData(ByVal year) As Boolean

		Dim verwendung_0 = New Integer() {0}
		m_LAData_Verwendung_0 = (m_PayrollDatabaseAccess.LoadLAData(year, verwendung_0))
		ThrowExceptionOnError(m_LAData_Verwendung_0 Is Nothing, "Lohnartendaten konnten nicht geladen werden (Verwendung 0).")

		Dim verwendung_1_3 = New Integer() {1, 3}
		m_LAData_Verwendung_1_3 = (m_PayrollDatabaseAccess.LoadLAData(year, verwendung_1_3))
		ThrowExceptionOnError(m_LAData_Verwendung_1_3 Is Nothing, "Lohnartendaten konnten nicht geladen werden (Verwendung 1 und 3).")

		Dim verwendung_2_3 = New Integer() {2, 3}
		m_LAData_Verwendung_2_3 = (m_PayrollDatabaseAccess.LoadLAData(year, verwendung_2_3))
		ThrowExceptionOnError(m_LAData_Verwendung_1_3 Is Nothing, "Lohnartendaten konnten nicht geladen werden (Verwendung 2 und 3).")

		Dim verwendung_4 = New Integer() {4}
		m_LAData_Verwendung_4 = (m_PayrollDatabaseAccess.LoadLAData(year, verwendung_4))
		ThrowExceptionOnError(m_LAData_Verwendung_1_3 Is Nothing, "Lohnartendaten konnten nicht geladen werden (Verwendung 4).")

		Return True
	End Function

	Public Function LoadLABezData() As Boolean

		Dim laBezData = m_PayrollDatabaseAccess.LoadLABezDataForPayroll()
		ThrowExceptionOnError(laBezData Is Nothing, "Lohnartenbezeichnungen konnten nicht geladen werden (LA_Bez 4).")

		m_LABezDataLookup = New Dictionary(Of Decimal, LA_BezData)
		Dim i As Integer = 0
		For Each laBez In laBezData
			'Trace.WriteLine(String.Format("{0}: {1}", i, laBez.LANr))

			m_LABezDataLookup.Add(laBez.LANr.GetValueOrDefault(0), laBez)

		Next

		Return True

	End Function

	Public Function GetTranslatedLABez(ByVal laNr As Decimal, ByVal language As String, ByVal defaulText As String)

		If m_LABezDataLookup.Keys.Contains(laNr) Then

			Dim laBezData = m_LABezDataLookup(laNr)
			Dim translation As String = String.Empty

			Select Case language
				Case "I"
					translation = laBezData.Name_I
				Case "E"
					translation = laBezData.Name_E
				Case "F"
					translation = laBezData.Name_F
				Case Else
					translation = defaulText
			End Select

			Return translation
		Else
			Return defaulText
		End If

	End Function

	Private Sub ThrowExceptionOnError(ByVal err As Boolean, ByVal errorText As String)
		If err Then
			Throw New Exception(errorText)
		End If

	End Sub

#End Region

End Class
