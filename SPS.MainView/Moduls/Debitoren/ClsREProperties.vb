
'Imports SP.Infrastructure.Logging

'Imports System.Data.SqlClient
'Imports SPProgUtility.SPTranslation.ClsTranslation
'Imports DevExpress.XtraGrid.Views.Grid


'' functions for Navbar in Debitoren
'Namespace REProperties


'  ''' <summary>
'  ''' report-Functionality for Customer
'  ''' </summary>
'  ''' <remarks></remarks>
'  Public Class ClsRERP

'    Private m_Logger As ILogger = New Logger()

'    Private _ClsSetting As New ClsrePropertySetting

'    Public Sub New(ByVal _setting As ClsREPropertySetting)
'      Me._ClsSetting = _setting
'    End Sub

'    ''' <summary>
'    ''' setting reportdatasources
'    ''' </summary>
'    ''' <remarks></remarks>
'    Sub LoadRERPData()
'      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'      Try
'        Try
'          Me._ClsSetting.grdRP.DataSource = GetDbData4RERP()

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
'    Function GetDbData4RERP() As DataTable
'      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'      Dim ds As New DataSet
'      Dim dt As New DataTable
'      Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
'      Try
'        Dim strDbQuery As String = "[Get RPData 4 Selected RE In MainView]"
'        Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
'        cmd.CommandType = Data.CommandType.StoredProcedure

'        Dim objAdapter As New SqlDataAdapter
'        objAdapter.SelectCommand = cmd
'        Dim param As System.Data.SqlClient.SqlParameter
'        param = cmd.Parameters.AddWithValue("@RENr", Me._ClsSetting.SelectedRENr)

'        objAdapter.Fill(ds, "RE")

'      Catch ex As Exception
'        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'      End Try

'      Return ds.Tables(0)
'    End Function

'  End Class


'  ''' <summary>
'  ''' setting of data for Lohn
'  ''' </summary>
'  ''' <remarks></remarks>
'  Public Class ClsREZE
'    Private Shared m_Logger As ILogger = New Logger()


'    Private _ClsSetting As New ClsREPropertySetting

'    Public Sub New(ByVal _setting As ClsREPropertySetting)
'      Me._ClsSetting = _setting
'    End Sub

'    Sub FillKDOpenZE()
'      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'      Try
'        Try
'          Me._ClsSetting.grdZE.DataSource = GetDbData4REZE()

'        Catch ex As Exception
'          m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
'          ShowErrDetail(String.Format("{0}. Datenbank lesen: {1}", strMethodeName, ex.Message))

'        End Try

'      Catch ex As Exception
'        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'      End Try

'    End Sub

'    Function GetDbData4REZE() As DataTable
'      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'      Dim ds As New DataSet
'      Dim dt As New DataTable
'      Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
'      Try
'        Dim strDbQuery As String = "[Get ZEData 4 Selected RE In MainView]"
'        Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
'        cmd.CommandType = Data.CommandType.StoredProcedure

'        Dim objAdapter As New SqlDataAdapter
'        objAdapter.SelectCommand = cmd
'        Dim param As System.Data.SqlClient.SqlParameter
'        param = cmd.Parameters.AddWithValue("@RENr", Me._ClsSetting.SelectedRENr)

'        objAdapter.Fill(ds, "Kunden")

'      Catch ex As Exception
'        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'      End Try

'      Return ds.Tables(0)
'    End Function


'  End Class



'End Namespace
