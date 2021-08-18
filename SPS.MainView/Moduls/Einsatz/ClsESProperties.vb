
'Imports System.Data.SqlClient
'Imports SPProgUtility.SPTranslation.ClsTranslation

'Imports DevExpress.XtraGrid.Views.Grid
'Imports SP.Infrastructure.Logging

'' functions for Navbar in Einsatz
'Namespace ESProperties

'  ''' <summary>
'  ''' report-Functionality for ES
'  ''' </summary>
'  ''' <remarks></remarks>
'  Public Class ClsESRP
'    Private Shared m_Logger As ILogger = New Logger()


'    Private _ClsSetting As New ClsESPropertySetting

'    Public Sub New(ByVal _setting As ClsESPropertySetting)
'      Me._ClsSetting = _setting
'    End Sub

'    ''' <summary>
'    ''' setting reportdatasources
'    ''' </summary>
'    ''' <remarks></remarks>
'    Sub FillESOpenRP()
'      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'      Try
'        Try
'          Me._ClsSetting.grdReport.DataSource = GetDbData4ESRP()

'        Catch ex As Exception
'          m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
'          ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'        End Try

'      Catch ex As Exception
'        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'      End Try

'    End Sub

'    ''' <summary>
'    ''' find database for reports
'    ''' </summary>
'    ''' <returns></returns>
'    ''' <remarks></remarks>
'    Function GetDbData4ESRP() As DataTable
'      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'      Dim ds As New DataSet
'      Dim dt As New DataTable
'      Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
'      Try
'        Dim strDbQuery As String = "[Get New Top OpenRPData 4 Selected ES In MainView]"
'        Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
'        cmd.CommandType = Data.CommandType.StoredProcedure

'        Dim objAdapter As New SqlDataAdapter
'        objAdapter.SelectCommand = cmd
'        Dim param As System.Data.SqlClient.SqlParameter
'        param = cmd.Parameters.AddWithValue("@ESNr", Me._ClsSetting.SelectedESNr)

'        objAdapter.Fill(ds, "RP")

'      Catch ex As Exception
'        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'      End Try

'      Return ds.Tables(0)
'    End Function

'  End Class


'End Namespace
