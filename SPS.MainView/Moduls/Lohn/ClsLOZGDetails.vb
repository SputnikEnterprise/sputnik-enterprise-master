

Imports SP.Infrastructure.Logging

Imports System.Data.SqlClient
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid
Imports System.Threading

Public Class ClsLOZGDetails
  Private Shared m_Logger As ILogger = New Logger()

  Private _ClsSetting As New ClsLOSetting

  Public Sub New(ByVal _setting As ClsLOSetting)
    Me._ClsSetting = _setting
  End Sub


#Region "Funktionen für Vorschüsse..."

  Sub FillMAOpenZG(ByVal grd As DevExpress.XtraGrid.GridControl)
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

    Try
      Try
        Me._ClsSetting.gvDetailDisplayMember = "Auszahlung-Nr."
        grd.DataSource = GetDbData4MAZG()

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

      End Try


    Catch ex As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

    End Try

  End Sub

  Function GetDbData4MAZG() As DataTable
    Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)

    Try
      Dim strDbQuery As String = String.Empty
      If Me._ClsSetting.Data4SelectedMA Then
        strDbQuery = "[Get ZGData 4 Selected MA In MainView]"
      Else
        strDbQuery = "[Get ZGData 4 All MA In MainView]"
      End If

      Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure

      Dim objAdapter As New SqlDataAdapter
      objAdapter.SelectCommand = cmd

      If Me._ClsSetting.Data4SelectedMA Then
        Dim param As System.Data.SqlClient.SqlParameter
        param = cmd.Parameters.AddWithValue("@MANr", Me._ClsSetting.SelectedMANr)
      End If

      objAdapter.Fill(ds, "Kandidat")

    Catch ex As Exception
      m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
      ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

    End Try

    Return ds.Tables(0)
  End Function

#End Region

End Class
