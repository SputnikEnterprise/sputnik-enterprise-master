
Imports SP.Infrastructure.Logging

Imports System.Data.SqlClient

Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid
Imports System.Threading
Imports SPProgUtility.Mandanten

Public Class ClsESModuls
	Private Shared m_Logger As ILogger = New Logger()

	Private _ClsSetting As New ClsESSetting
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

	Dim _PrintjobSetting As New ClsPrintSetting
	Dim mgr As New DevExpress.XtraBars.BarManager
	Dim itm As New DevExpress.XtraBars.BarButtonItem
	Private m_translate As TranslateValues

	Private m_md As Mandant

	Public Sub New(ByVal _setting As ClsESSetting)
		m_md = New Mandant

		Me._ClsSetting = _setting
		m_translate = New TranslateValues

	End Sub

#Region "Funktionen für Kontextmenü..."


#End Region


	Sub ListAktivMandanten(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
		Dim strQuery As String = "[Get MandantListing In MainView]"

		cbo.Properties.Items.Clear()
		Try

			Conn.Open()
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim rFrec As SqlDataReader = cmd.ExecuteReader
			While rFrec.Read
				cbo.Properties.Items.Add(New ComboValue(rFrec("Mandantenname"), rFrec("MDNr")))
				cbo.Properties.ValueMember = rFrec("MDNr")
			End While

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
			ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

		End Try

	End Sub

End Class

