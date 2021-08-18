
Imports System.Data.SqlClient
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports DevExpress.XtraEditors

Imports SP.Infrastructure.Logging
Imports System.IO
Imports SPS.MA.Lohn.Utility.ModulConstants


Module Utilities

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath



	''' <summary>
	''' Ob der gewünschte Monat und Jahr abgeschlossen ist.
	''' </summary>
	''' <param name="_sMonth"></param>
	''' <param name="_iYear"></param>
	''' <returns>Boolean (True | False)</returns>
	''' <remarks></remarks>
	Public Function IsLOMonthClosed(ByVal _MDNr As Integer, ByVal _sMonth As Short, ByVal _iYear As Integer) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strResult As String = String.Empty
		Dim sSql As String = "[Get Data For ClosedMonth With Mandant]"
		Dim ConnDbSelect As New SqlConnection(_ClsProgSetting.GetConnString)

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)
			cmd.CommandType = CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			Dim rFrec As SqlClient.SqlDataReader

			Try
				ConnDbSelect.Open()
				param = cmd.Parameters.AddWithValue("@MDNr", _MDNr)
				param = cmd.Parameters.AddWithValue("@sMonth", _sMonth)
				param = cmd.Parameters.AddWithValue("@Year", _iYear)

				rFrec = cmd.ExecuteReader
				rFrec.Read()
				If rFrec.HasRows Then
					strResult = String.Format("{0}|{1}", rFrec("UserName"), rFrec("CreatedOn"))
				End If
				rFrec.Close()

			Catch ex As Exception
				m_logger.logerror(String.Format("{0}.Datenbank lesen:{1}", strMethodeName, ex.tostring))

			Finally

			End Try

		Catch ex As Exception
			m_logger.logerror(String.Format("{0}.{1}", strMethodeName, ex.tostring))

		Finally
			ConnDbSelect.Close()

		End Try
		Return strResult

	End Function




	Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

End Module
