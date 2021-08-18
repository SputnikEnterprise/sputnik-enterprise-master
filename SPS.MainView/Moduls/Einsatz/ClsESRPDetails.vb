
'Imports SP.Infrastructure.Logging

'Imports System.Data.SqlClient
'Imports SPProgUtility.SPTranslation.ClsTranslation

'Imports DevExpress.XtraGrid.Views.Grid
'Imports System.Threading

'Public Class ClsESRPDetails
'  Private Shared m_Logger As ILogger = New Logger()

'  Private _ClsSetting As New ClsESSetting

'  Public Sub New(ByVal _setting As ClsESSetting)
'    Me._ClsSetting = _setting
'  End Sub


'#Region "Funktionen für Rapporte..."

'  Sub FillESOpenRP(ByVal grd As DevExpress.XtraGrid.GridControl)
'    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'    Try
'      Try
'        Me._ClsSetting.gvDetailDisplayMember = "Rapport-Nr."
'        grd.DataSource = GetDbData4ESRP()

'      Catch ex As Exception
'        m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
'        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'      End Try


'    Catch ex As Exception
'      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'    End Try

'  End Sub

'  Function GetDbData4ESRP() As DataTable
'    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'    Dim ds As New DataSet
'    Dim dt As New DataTable
'    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

'    Try
'      Dim strDbQuery As String = String.Empty
'      If Me._ClsSetting.Data4SelectedES Then
'        strDbQuery = "[Get RPData 4 Selected ES In MainView]"
'      Else
'        strDbQuery = "[Get RPData 4 All ES In MainView]"
'      End If

'      Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
'      cmd.CommandType = Data.CommandType.StoredProcedure

'      Dim objAdapter As New SqlDataAdapter
'      objAdapter.SelectCommand = cmd

'      If Me._ClsSetting.Data4SelectedES Then
'        Dim param As System.Data.SqlClient.SqlParameter
'        param = cmd.Parameters.AddWithValue("@ESNr", Me._ClsSetting.SelectedESNr)
'      End If

'      objAdapter.Fill(ds, "ES")

'    Catch ex As Exception
'      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'    End Try

'    Return ds.Tables(0)
'  End Function

'#End Region


'End Class
