

Imports System.Data.SqlClient
Imports SP.RPContent.PrintUtility

Public Class ClsRPContentUtility


	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private _PControlSetting As New ClsParifondSetting
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


#Region "Construct"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal _MySetting As ClsParifondSetting)
		m_InitializationData = _setting
		Me._PControlSetting = _MySetting
	End Sub

#End Region


	Function CreateAllData4RPContent() As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."

		Dim _locSetting As ClsRPCSetting = New ClsRPCSetting With {.DbConnString2Open = _PControlSetting.DbConnString2Open, .FoundedRPNr = Me._PControlSetting.liRPNr2Print}

		Dim obj As New SP.RPContent.PrintUtility.ClsMain_Net(_locSetting)
		strResult = obj.CreateRPContentFiles

		Return strResult
	End Function

End Class

