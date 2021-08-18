
'Imports System.Data.SqlClient
'Imports SPProgUtility.SPTranslation.ClsTranslation

'Imports DevExpress.XtraGrid.Views.Grid
'Imports SP.Infrastructure.Logging

'' functions for Navbar in candidate
'Namespace MAProperties

'  '  ''' <summary>
'  '  ''' ES-Functionality for Candidate
'  '  ''' </summary>
'  '  ''' <remarks></remarks>
'  '  Public Class ClsMAES
'  '    Private Shared m_Logger As ILogger = New Logger()

'  '    Private _ClsSetting As New ClsMAPropertySetting

'  '    Public Sub New(ByVal _setting As ClsMAPropertySetting)
'  '      Me._ClsSetting = _setting
'  '    End Sub

'  '#Region "Funktionen für aktive Einsätze..."

'  '    ''' <summary>
'  '    ''' fills Top 10 activ ES in Navbar
'  '    ''' </summary>
'  '    ''' <remarks></remarks>
'  '    Sub FillMAAktiveES()
'  '      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'  '      Me._ClsSetting.gES.DataSource = Nothing
'  '      Try
'  '        Try
'  '          Me._ClsSetting.gES.DataSource = GetDbData4MAES()

'  '        Catch ex As Exception
'  '          m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
'  '          ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '        End Try


'  '      Catch ex As Exception
'  '        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '      End Try

'  '    End Sub

'  '    ''' <summary>
'  '    ''' Get Database for Top 10 activ ES in Navbar
'  '    ''' </summary>
'  '    ''' <returns></returns>
'  '    ''' <remarks></remarks>
'  '    Function GetDbData4MAES() As DataTable
'  '      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'  '      Dim ds As New DataSet
'  '      Dim dt As New DataTable
'  '      Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
'  '      Try
'  '        Dim strDbQuery As String = "[Get New Top ESData 4 Selected MA In MainView]"
'  '        Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
'  '        cmd.CommandType = Data.CommandType.StoredProcedure

'  '        Dim objAdapter As New SqlDataAdapter
'  '        objAdapter.SelectCommand = cmd
'  '        Dim param As System.Data.SqlClient.SqlParameter
'  '        param = cmd.Parameters.AddWithValue("@MANr", Me._ClsSetting.SelectedMANr)

'  '        objAdapter.Fill(ds, "Kandidat")

'  '      Catch ex As Exception
'  '        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '      End Try

'  '      Return ds.Tables(0)
'  '    End Function

'  '#End Region

'  '  End Class

'  ' ''' <summary>
'  ' ''' proposal-Functionality for Candidate
'  ' ''' </summary>
'  ' ''' <remarks></remarks>
'  'Public Class ClsMAPropose
'  '  Private Shared m_Logger As ILogger = New Logger()

'  '  Private _ClsSetting As New ClsMAPropertySetting

'  '  Public Sub New(ByVal _setting As ClsMAPropertySetting)
'  '    Me._ClsSetting = _setting
'  '  End Sub

'  '  ''' <summary>
'  '  ''' set propose datasource
'  '  ''' </summary>
'  '  ''' <remarks></remarks>
'  '  Sub FillMAAktivePropose()
'  '    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'  '    Try
'  '      Try
'  '        Me._ClsSetting.grdPropose.DataSource = GetDbData4MAPropose()

'  '      Catch ex As Exception
'  '        m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
'  '        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '      End Try

'  '    Catch ex As Exception
'  '      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '    End Try

'  '  End Sub

'  '  ''' <summary>
'  '  ''' get database for proposal
'  '  ''' </summary>
'  '  ''' <returns></returns>
'  '  ''' <remarks></remarks>
'  '  Function GetDbData4MAPropose() As DataTable
'  '    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'  '    Dim ds As New DataSet
'  '    Dim dt As New DataTable
'  '    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
'  '    Try
'  '      Dim strDbQuery As String = "[Get New Top ProposeData 4 Selected MA In MainView]"
'  '      Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
'  '      cmd.CommandType = Data.CommandType.StoredProcedure

'  '      Dim objAdapter As New SqlDataAdapter
'  '      objAdapter.SelectCommand = cmd
'  '      Dim param As System.Data.SqlClient.SqlParameter
'  '      param = cmd.Parameters.AddWithValue("@MANr", Me._ClsSetting.SelectedMANr)

'  '      objAdapter.Fill(ds, "Kandidat")

'  '    Catch ex As Exception
'  '      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '    End Try

'  '    Return ds.Tables(0)
'  '  End Function


'  'End Class


'  ' ''' <summary>
'  ' ''' report-Functionality for Candidate
'  ' ''' </summary>
'  ' ''' <remarks></remarks>
'  'Public Class ClsMARP
'  '  Private Shared m_Logger As ILogger = New Logger()

'  '  Private _ClsSetting As New ClsMAPropertySetting

'  '  Public Sub New(ByVal _setting As ClsMAPropertySetting)
'  '    Me._ClsSetting = _setting
'  '  End Sub

'  '  ''' <summary>
'  '  ''' setting reportdatasources
'  '  ''' </summary>
'  '  ''' <remarks></remarks>
'  '  Sub FillMAOpenRP()
'  '    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'  '    Try
'  '      Try
'  '        Me._ClsSetting.grdRP.DataSource = GetDbData4MARP()

'  '      Catch ex As Exception
'  '        m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
'  '        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '      End Try

'  '    Catch ex As Exception
'  '      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '    End Try

'  '  End Sub

'  '  ''' <summary>
'  '  ''' find database for reports
'  '  ''' </summary>
'  '  ''' <returns></returns>
'  '  ''' <remarks></remarks>
'  '  Function GetDbData4MARP() As DataTable
'  '    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'  '    Dim ds As New DataSet
'  '    Dim dt As New DataTable
'  '    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
'  '    Try
'  '      Dim strDbQuery As String = "[Get New Top OpenRPData 4 Selected MA In MainView]"
'  '      Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
'  '      cmd.CommandType = Data.CommandType.StoredProcedure

'  '      Dim objAdapter As New SqlDataAdapter
'  '      objAdapter.SelectCommand = cmd
'  '      Dim param As System.Data.SqlClient.SqlParameter
'  '      param = cmd.Parameters.AddWithValue("@MANr", Me._ClsSetting.SelectedMANr)

'  '      objAdapter.Fill(ds, "Kandidat")

'  '    Catch ex As Exception
'  '      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '    End Try

'  '    Return ds.Tables(0)
'  '  End Function

'  'End Class


'  ' ''' <summary>
'  ' ''' Vorschuss-Functionality for Candidate
'  ' ''' </summary>
'  ' ''' <remarks></remarks>
'  'Public Class ClsMAZG
'  '  Private Shared m_Logger As ILogger = New Logger()


'  '  Private _ClsSetting As New ClsMAPropertySetting

'  '  Public Sub New(ByVal _setting As ClsMAPropertySetting)
'  '    Me._ClsSetting = _setting
'  '  End Sub

'  '  ''' <summary>
'  '  ''' set datasource of Vorschüsse
'  '  ''' </summary>
'  '  ''' <remarks></remarks>
'  '  Sub FillMAOpenZG()
'  '    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'  '    Try
'  '      Try
'  '        Me._ClsSetting.grdZG.DataSource = GetDbData4MAZG()

'  '      Catch ex As Exception
'  '        m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
'  '        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '      End Try

'  '    Catch ex As Exception
'  '      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '    End Try

'  '  End Sub

'  '  ''' <summary>
'  '  ''' find datarecords for Vorschüsse
'  '  ''' </summary>
'  '  ''' <returns></returns>
'  '  ''' <remarks></remarks>
'  '  Function GetDbData4MAZG() As DataTable
'  '    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'  '    Dim ds As New DataSet
'  '    Dim dt As New DataTable
'  '    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
'  '    Try
'  '      Dim strDbQuery As String = "[Get New Top OpenZGData 4 Selected MA In MainView]"
'  '      Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
'  '      cmd.CommandType = Data.CommandType.StoredProcedure

'  '      Dim objAdapter As New SqlDataAdapter
'  '      objAdapter.SelectCommand = cmd
'  '      Dim param As System.Data.SqlClient.SqlParameter
'  '      param = cmd.Parameters.AddWithValue("@MANr", Me._ClsSetting.SelectedMANr)

'  '      objAdapter.Fill(ds, "Kandidat")

'  '    Catch ex As Exception
'  '      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '    End Try

'  '    Return ds.Tables(0)
'  '  End Function

'  'End Class


'  ' ''' <summary>
'  ' ''' setting of data for Lohn
'  ' ''' </summary>
'  ' ''' <remarks></remarks>
'  'Public Class ClsMALO
'  '  Private Shared m_Logger As ILogger = New Logger()


'  '  Private _ClsSetting As New ClsMAPropertySetting

'  '  Public Sub New(ByVal _setting As ClsMAPropertySetting)
'  '    Me._ClsSetting = _setting
'  '  End Sub

'  '  Sub FillMAOpenLO()
'  '    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'  '    Try
'  '      Try
'  '        Me._ClsSetting.grdLO.DataSource = GetDbData4MALO()

'  '      Catch ex As Exception
'  '        m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
'  '        ShowErrDetail(String.Format("{0}. Datenbank lesen: {1}", strMethodeName, ex.Message))

'  '      End Try

'  '    Catch ex As Exception
'  '      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '    End Try

'  '  End Sub

'  '  Function GetDbData4MALO() As DataTable
'  '    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'  '    Dim ds As New DataSet
'  '    Dim dt As New DataTable
'  '    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
'  '    Try
'  '      Dim strDbQuery As String = "[Get New Top NewLOData 4 Selected MA In MainView]"
'  '      Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
'  '      cmd.CommandType = Data.CommandType.StoredProcedure

'  '      Dim objAdapter As New SqlDataAdapter
'  '      objAdapter.SelectCommand = cmd
'  '      Dim param As System.Data.SqlClient.SqlParameter
'  '      param = cmd.Parameters.AddWithValue("@MANr", Me._ClsSetting.SelectedMANr)

'  '      objAdapter.Fill(ds, "Kandidat")

'  '    Catch ex As Exception
'  '      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '    End Try

'  '    Return ds.Tables(0)
'  '  End Function

'  'End Class


'  ' ''' <summary>
'  ' ''' functions for contact for candidates
'  ' ''' </summary>
'  ' ''' <remarks></remarks>
'  'Public Class ClsMAContact
'  '  Private Shared m_Logger As ILogger = New Logger()


'  '  Private _ClsSetting As New ClsMAPropertySetting

'  '  Public Sub New(ByVal _setting As ClsMAPropertySetting)
'  '    Me._ClsSetting = _setting
'  '  End Sub

'  '  Sub FillMAOpenKontakte()
'  '    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

'  '    Try
'  '      Try
'  '        Me._ClsSetting.grdContact.DataSource = GetDbData4MAKontakte()

'  '      Catch ex As Exception
'  '        m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
'  '        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '      End Try

'  '    Catch ex As Exception
'  '      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '    End Try

'  '  End Sub

'  '  Function GetDbData4MAKontakte() As DataTable
'  '    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'  '    Dim ds As New DataSet
'  '    Dim dt As New DataTable
'  '    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
'  '    Try
'  '      Dim strDbQuery As String = "[Get New Top KontaktData 4 Selected MA In MainView]"
'  '      Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
'  '      cmd.CommandType = Data.CommandType.StoredProcedure

'  '      Dim objAdapter As New SqlDataAdapter
'  '      objAdapter.SelectCommand = cmd
'  '      Dim param As System.Data.SqlClient.SqlParameter
'  '      param = cmd.Parameters.AddWithValue("@MANr", Me._ClsSetting.SelectedMANr)

'  '      objAdapter.Fill(ds, "Kandidat")

'  '    Catch ex As Exception
'  '      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'  '      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

'  '    End Try

'  '    Return ds.Tables(0)
'  '  End Function

'  'End Class


'End Namespace
