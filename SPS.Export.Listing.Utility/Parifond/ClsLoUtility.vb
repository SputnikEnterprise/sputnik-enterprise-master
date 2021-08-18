
Imports SP.Infrastructure.Logging
Imports System.Data.SqlClient
Imports SPS.Listing.Print.Utility.LOSearchListing
Imports SPS.Listing.Print.Utility
Imports SP.Infrastructure.UI

Public Class ClsLoUtility

	''' <summary>
	''' The logger.
	''' </summary>
	Private Shared m_Logger As ILogger = New Logger()

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	'Private _PControlSetting As New ClsParifondSetting

	''' <summary>
	''' UI Utility functions.
	''' </summary>
	Private m_UtilityUI As UtilityUI



#Region "public properties"

	Public Property ParifondSetting As ClsParifondSetting

#End Region


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)
		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		m_UtilityUI = New UtilityUI

	End Sub

	'Public Sub New(ByVal _MySetting As ClsParifondSetting)
	'	Me._PControlSetting = _MySetting
	'End Sub


#End Region

	Function CreateData4LO() As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = "Success..."
		Dim liFoundedLONr As New List(Of Integer)
		Dim liFoundedMANr As New List(Of Integer)
		Dim liFoundedMANang As New List(Of String)
		Dim liFoundedWOS As New List(Of Boolean)

		Try
			If ParifondSetting.liLONr2Print Is Nothing OrElse ParifondSetting.liLONr2Print.Count = 0 Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Lohnabrechnungen gefunden!"))
				Return "Keine Lohnabrechnungen gefunden."
			End If

			Dim strQuery As String = "Select LO.MDNr, LO.LONr, LO.MANr, "
			strQuery &= "IsNull(MA.Send2WOS, 0) As MAWos, MA.Sprache As MASprache "
			strQuery &= "From LO "
			strQuery &= "Left Join Mitarbeiter MA On LO.MANr = MA.MANr "
			strQuery &= "Where LO.LONr In ({0}) "
			strQuery &= "Order By LO.MANr, LO.Jahr, LO.LP"
			Dim Conn As SqlConnection = New SqlConnection(m_InitializationData.MDData.MDDbConn)
			Dim strLONr As String = String.Empty
			For i As Integer = 0 To ParifondSetting.liLONr2Print.Count - 1
				strLONr &= If(Not String.IsNullOrWhiteSpace(strLONr), ",", "") & ParifondSetting.liLONr2Print(i)
			Next
			strQuery = String.Format(strQuery, strLONr)

			Try
				Conn.Open()
				m_Logger.LogDebug("strQuery für LOAuswahl: " & strQuery)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
				cmd.CommandType = Data.CommandType.Text
				Dim rLOrec As SqlDataReader = cmd.ExecuteReader

				While rLOrec.Read
					liFoundedLONr.Add(rLOrec("LONr"))
					liFoundedMANr.Add(rLOrec("MANr"))
					liFoundedWOS.Add(False)
					liFoundedMANang.Add(rLOrec("MASprache").ToString.Substring(0, 1).ToUpper)
				End While

			Catch ex As Exception
				Dim strMsg As String = String.Format("{0}:LO-Db-Felder lesen. {1}", strMethodeName, ex.Message)
				m_Logger.LogError(strMsg)
				Return strMsg

			End Try

		Catch ex As Exception
			Dim strMsg As String = String.Format("{0}:LO-Db-Felder öffnen. {1}", strMethodeName, ex.Message)
			m_Logger.LogError(strMsg)
			Return strMsg
		End Try

		Try
			If liFoundedLONr.Count > 0 Then
				Dim _Setting As New ClsLLLOSearchPrintSetting
				_Setting = New ClsLLLOSearchPrintSetting With {.DbConnString2Open = m_InitializationData.MDData.MDDbConn,
																											 .SelectedMDNr = ModulConstants.MDData.MDNr,
																											 .SQL2Open = String.Empty,
																											 .JobNr2Print = "9.1",
																											 .Is4Export = True,
																											 .SendData2WOS = False,
																											 .SendAndPrintData2WOS = False,
																											 .liLONr2Print = liFoundedLONr,
																											 .liMANr2Print = liFoundedMANr,
																											 .liLOSend2WOS = liFoundedWOS,
																											 .LiMALang = liFoundedMANang,
																											 .SelectedLONr2Print = 0,
																											 .SelectedMANr2Print = 0}

				Dim obj As New ClsPrintLOSearchList(m_InitializationData)
				obj.PrintData = _Setting
				strResult = obj.ExportLOSearchList()
			End If

		Catch ex As Exception
			Dim strMsg As String = String.Format("{0}:Erstellen. {1}", strMethodeName, ex.Message)
			m_Logger.LogError(strMsg)
			Return strMsg

		End Try

		Return strResult
	End Function

End Class

