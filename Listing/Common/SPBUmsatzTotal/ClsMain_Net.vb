
Imports System.IO.File

Public Class ClsMain_Net

	'Dim _ClsReg As New SPProgUtility.ClsDivReg

  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		If Not _setting Is Nothing Then
			_setting.MDData.MDDbConn = EnablingMarsintoConnString(_setting.MDData.MDDbConn)
			ClsDataDetail.m_InitialData = _setting
			ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		End If

		Application.EnableVisualStyles()

	End Sub

	Public Sub New()
		Dim m_md As New SPProgUtility.Mandanten.Mandant

		Dim _setting = CreateInitialData(m_md.GetDefaultMDNr, m_md.GetDefaultUSNr)
		_setting.MDData.MDDbConn = EnablingMarsintoConnString(_setting.MDData.MDDbConn)
		ClsDataDetail.m_InitialData = _setting
		ClsDataDetail.m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)

		Application.EnableVisualStyles()

	End Sub

  Sub ShowFrmUmsDb1()
		Dim frmTest As frmUmsatz
		frmTest = New frmUmsatz(ClsDataDetail.m_InitialData)

    ClsDataDetail.IsAutomaatedStart = False
    ClsDataDetail.frmUms = frmTest

		frmTest.Show()
		frmTest.BringToFront()

	End Sub

  Sub CreateAutomatedUmsDb1(ByVal iUSNr As Integer, _
                            ByVal libFormData As List(Of String), _
                            ByVal strConnString As String)
    ClsDataDetail.GetAutoConnString = strConnString
		Dim m_SearchCriteria As New SearchCriteria

		m_SearchCriteria.FirstMonth = CInt(Val(libFormData(1).ToString))
		m_SearchCriteria.LastMonth = CInt(Val(libFormData(2).ToString))
		m_SearchCriteria.FirstYear = CInt(Val(libFormData(3).ToString))
		m_SearchCriteria.LastYear = CInt(Val(libFormData(4).ToString))

		Dim _ClsDb As New ClsDbFunc(ClsDataDetail.m_InitialData)
		_ClsDb.SearchCriterias = m_SearchCriteria

		Dim _ClsQuery As New ClsGetSQLString(m_SearchCriteria)
    Dim sSql1Query As String = String.Empty
    Dim sSql2Query As String = String.Empty
    Dim strSort As String = String.Empty
    Dim lstKst3 As New ListBox
    Dim strMySelectedKST3 As String = libFormData(5).ToString

		Dim strFMonth As Integer = CInt(Val(libFormData(1).ToString))
		Dim strLMonth As Integer = CInt(Val(libFormData(2).ToString))
		Dim strVYear As Integer = CInt(Val(libFormData(3).ToString))
		Dim strBYear As Integer = CInt(Val(libFormData(4).ToString))

    ClsDataDetail.GetFormVars = libFormData
    ClsDataDetail.IsAutomaatedStart = True
		'    m_InitialData.MDData.MDDbConn = strConnString
    ClsDataDetail.GetAutoUserNr = iUSNr

    Dim _clsEventlog As New SPProgUtility.ClsEventLog
    _clsEventlog.WriteMainLog("CreateAutomatedUmsDb1")

    Try
      If Not IsNothing(ClsDataDetail.Conn) Then ClsDataDetail.Conn.Dispose()
      ' KST3-Daten in Lst füllen...
      lstKst3.Items.Clear()
			FillFoundedKstBez(lstKst3, m_SearchCriteria) ' CInt(libFormData(1)), CInt(libFormData(2)), CInt(libFormData(3)), CInt(libFormData(4)))

			If lstKst3.Items.Count = 0 Then Exit Sub


    Catch ex As Exception
      MsgBox(ex.StackTrace & vbNewLine & ex.Message, MsgBoxStyle.Critical, "CreateAutomatedUmsDb1_0")

    End Try

    If strMySelectedKST3 <> String.Empty Then
      lstKst3.BeginUpdate()
      For i As Integer = lstKst3.Items.Count - 1 To 0 Step -1
        Trace.WriteLine(String.Format("KST: {0}", lstKst3.Items(i).ToString))

        If Not lstKst3.Items(i).ToString.ToUpper.Contains(strMySelectedKST3.Trim.ToUpper) Then
          lstKst3.Items.RemoveAt(i)
        End If
      Next
      lstKst3.EndUpdate()
    End If

    _ClsDb.LbKST3 = lstKst3
    Try
      If IsNothing(ClsDataDetail.Conn) Then
        _ClsDb.CalcDataForJournal(CInt(strFMonth), CInt(strLMonth), strVYear, strBYear)
      Else
        If ClsDataDetail.Conn.State <> ConnectionState.Open Then
          _ClsDb.CalcDataForJournal(CInt(strFMonth), CInt(strLMonth), strVYear, strBYear)
        End If
      End If

    Catch ex As Exception
      MessageBox.Show(ex.StackTrace & vbNewLine & ex.Message, "CreateAutomatedUmsDb1_1")

    End Try

    Try
      ' SQL_Query mit Order Klausel...
      ClsDataDetail.GetSQLQuery() = _ClsQuery.GetUJournalQueryForOutput(strFMonth, strLMonth, strVYear)

			Dim _ClsJournal As New ClsJournal(m_SearchCriteria)	' strVYear, strBYear)
			_ClsJournal.m_cDbMyConn = ClsDataDetail.Conn
			_ClsJournal.m_XMargeFromMD = _ClsDb.GetXMargeFromMD()

      _ClsJournal.CallculateAllFields()
      _ClsDb.CreateMySP4FilialProz()


    Catch ex As Exception
      MessageBox.Show(ex.StackTrace & vbNewLine & ex.Message, "CreateAutomatedUmsDb1_2")

    End Try


  End Sub


	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function



End Class
