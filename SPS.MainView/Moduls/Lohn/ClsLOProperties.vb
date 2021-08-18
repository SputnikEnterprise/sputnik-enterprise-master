
Imports System.Data.SqlClient
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports DevExpress.XtraGrid.Views.Grid
Imports SP.Infrastructure.Logging

' functions for Navbar in candidate
Namespace LOProperties

  ''' <summary>
  ''' ES-Functionality for Candidate
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ClsLODetail
    Private Shared m_Logger As ILogger = New Logger()


    Private _ClsSetting As New ClsLOPropertySetting

    Public Sub New(ByVal _setting As ClsLOPropertySetting)
      Me._ClsSetting = _setting
    End Sub

#Region "Funktionen für aktive Einsätze..."


    '  Private Function LoadFoundedSalaryList() As Boolean
    '    Dim m_DataAccess As New MainGrid
    '    Dim listOfEmployees = m_DataAccess.GetDbSalaryData4Show(m_griddata.SQLQuery)

    '    Dim responsiblePersonsGridData = (From person In listOfEmployees
    '    Select New FoundedSalaryData With
    '           {.lonr = person.lonr,
    '            .manr = person.manr,
    '            .mdnr = person.mdnr,
    '            .zgnr = person.zgnr,
    '            .lmid = person.lmid,
    '            .vgnr = person.vgnr,
    '            .bruttobetrag = person.bruttobetrag,
    '            .zgbetrag = person.zgbetrag,
    '            .lmbetrag = person.lmbetrag,
    '            .lobetrag = person.lobetrag,
    '            .monat = person.monat,
    '            .jahr = person.jahr,
    '            .loperiode = person.loperiode,
    '            .erstelltam = person.erstelltam,
    '            .erstelltdurch = person.erstelltdurch,
    '            .maname = person.maname,
    '            .mastrasse = person.mastrasse,
    '            .maplzort = person.maplzort,
    '            .maaddress = person.maaddress,
    '            .matelefon = person.matelefon,
    '            .manatel = person.manatel,
    '            .maemail = person.maemail,
    '            .magebdat = person.magebdat,
    '            .maalterwithdate = person.maalterwithdate,
    '            .mabewilligung = person.mabewilligung,
    '            .maqualifikation = person.maqualifikation,
    '            .tempmabild = person.tempmabild
    '}).ToList()

    '    Dim listDataSource As BindingList(Of FoundedSalaryData) = New BindingList(Of FoundedSalaryData)

    '    For Each p In responsiblePersonsGridData
    '      listDataSource.Add(p)
    '    Next

    '    grdlol.DataSource = listDataSource

    '    Return Not listOfEmployees Is Nothing
    '  End Function

#End Region

  End Class


  ''' <summary>
  ''' report-Functionality for Candidate
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ClsLORP
    Private Shared m_Logger As ILogger = New Logger()


    Private _ClsSetting As New ClsLOPropertySetting

    Public Sub New(ByVal _setting As ClsLOPropertySetting)
      Me._ClsSetting = _setting
    End Sub

    ''' <summary>
    ''' setting reportdatasources
    ''' </summary>
    ''' <remarks></remarks>
    Sub FillLOOpenRP()
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

      Try
        Try
          Me._ClsSetting.grdRP.DataSource = GetDbData4LORP()

        Catch ex As Exception
          m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
          ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

        End Try

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

      End Try

    End Sub

    ''' <summary>
    ''' find database for reports
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetDbData4LORP() As DataTable
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim ds As New DataSet
      Dim dt As New DataTable
      Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
      Try
        Dim strDbQuery As String = "[Get New Top OpenRPData 4 Selected MA In MainView]"
        Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
        cmd.CommandType = Data.CommandType.StoredProcedure

        Dim objAdapter As New SqlDataAdapter
        objAdapter.SelectCommand = cmd
        Dim param As System.Data.SqlClient.SqlParameter
        param = cmd.Parameters.AddWithValue("@MANr", Me._ClsSetting.SelectedMANr)

        objAdapter.Fill(ds, "Kandidat")

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

      End Try

      Return ds.Tables(0)
    End Function

  End Class


  ''' <summary>
  ''' Vorschuss-Functionality for Candidate
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ClsloZG
    Private Shared m_Logger As ILogger = New Logger()


    Private _ClsSetting As New ClsLOPropertySetting

    Public Sub New(ByVal _setting As ClsLOPropertySetting)
      Me._ClsSetting = _setting
    End Sub

    ''' <summary>
    ''' set datasource of Vorschüsse
    ''' </summary>
    ''' <remarks></remarks>
    Sub FillLOOpenZG()
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

      Try
        Try
          Me._ClsSetting.grdZG.DataSource = GetDbData4LOZG()

        Catch ex As Exception
          m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
          ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

        End Try

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

      End Try

    End Sub

    ''' <summary>
    ''' find datarecords for Vorschüsse
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetDbData4LOZG() As DataTable
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim ds As New DataSet
      Dim dt As New DataTable
      Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
      Try
        Dim strDbQuery As String = "[Get New Top OpenZGData 4 Selected MA In MainView]"
        Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
        cmd.CommandType = Data.CommandType.StoredProcedure

        Dim objAdapter As New SqlDataAdapter
        objAdapter.SelectCommand = cmd
        Dim param As System.Data.SqlClient.SqlParameter
        param = cmd.Parameters.AddWithValue("@MANr", Me._ClsSetting.SelectedMANr)

        objAdapter.Fill(ds, "Kandidat")

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

      End Try

      Return ds.Tables(0)
    End Function

  End Class


  ''' <summary>
  ''' proposal-Functionality for Candidate
  ''' </summary>
  ''' <remarks></remarks>
  Public Class ClsLOLM
    Private Shared m_Logger As ILogger = New Logger()


    Private _ClsSetting As New ClsLOPropertySetting

    Public Sub New(ByVal _setting As ClsLOPropertySetting)
      Me._ClsSetting = _setting
    End Sub

    ''' <summary>
    ''' set propose datasource
    ''' </summary>
    ''' <remarks></remarks>
    Sub FillLOAktiveLM()
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

      Try
        Try
          Me._ClsSetting.grdLM.DataSource = GetDbData4LOLM()

        Catch ex As Exception
          m_Logger.LogError(String.Format("{0}.Datenbank lesen: {1}", strMethodeName, ex.Message))
          ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

        End Try

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

      End Try

    End Sub

    ''' <summary>
    ''' get database for proposal
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetDbData4LOLM() As DataTable
      Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
      Dim ds As New DataSet
      Dim dt As New DataTable
      Dim Conn As SqlConnection = New SqlConnection(ModulConstants.MDData.MDDbConn)
      Try
        Dim strDbQuery As String = "[Get New Top ProposeData 4 Selected MA In MainView]"
        Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strDbQuery, Conn)
        cmd.CommandType = Data.CommandType.StoredProcedure

        Dim objAdapter As New SqlDataAdapter
        objAdapter.SelectCommand = cmd
        Dim param As System.Data.SqlClient.SqlParameter
        param = cmd.Parameters.AddWithValue("@MANr", Me._ClsSetting.SelectedMANr)

        objAdapter.Fill(ds, "Kandidat")

      Catch ex As Exception
        m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
        ShowErrDetail(String.Format("{0}: {1}", strMethodeName, ex.Message))

      End Try

      Return ds.Tables(0)
    End Function


  End Class



End Namespace
