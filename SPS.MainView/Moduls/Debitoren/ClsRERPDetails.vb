
'Imports SP.Infrastructure.Logging

'Imports System.Data.SqlClient
'Imports SPProgUtility.SPTranslation.ClsTranslation

'Imports DevExpress.XtraGrid.Views.Grid
'Imports System.Threading

'Public Class ClsRERPDetails
'  Private Shared m_Logger As ILogger = New Logger()

'  Private _ClsSetting As New ClsRESetting

'  Public Sub New(ByVal _setting As ClsRESetting)
'    Me._ClsSetting = _setting
'  End Sub


'#Region "Funktionen für Rapporte..."

'  Sub FillKDOpenRP(ByVal grd As DevExpress.XtraGrid.GridControl)
'    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'    Try
'      Try
'        Me._ClsSetting.gvDetailDisplayMember = "Rapport-Nr."
'        grd.DataSource = GetDbData4KDRP()

'      Catch ex As Exception
'        m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
'        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'      End Try


'    Catch ex As Exception
'      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'    End Try

'  End Sub

'  Function GetDbData4KDRP() As DataTable
'    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'    Dim ds As New DataSet
'    Dim dt As New DataTable
'    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

'    Try
'      Dim strDbQuery As String = String.Empty
'      If Me._ClsSetting.Data4SelectedKD Then
'        strDbQuery = "[Get RPData 4 Selected KD In MainView]"
'      Else
'        strDbQuery = "[Get RPData 4 All KD In MainView]"
'      End If

'      Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
'      cmd.CommandType = Data.CommandType.StoredProcedure

'      Dim objAdapter As New SqlDataAdapter
'      objAdapter.SelectCommand = cmd

'      If Me._ClsSetting.Data4SelectedKD Then
'        Dim param As System.Data.SqlClient.SqlParameter
'        param = cmd.Parameters.AddWithValue("@KDNr", Me._ClsSetting.SelectedKDNr)
'      End If

'      objAdapter.Fill(ds, "Kunden")

'    Catch ex As Exception
'      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'    End Try

'    Return ds.Tables(0)
'  End Function

'#End Region



'End Class
