
'Imports SP.Infrastructure.Logging
'Imports SP.Infrastructure.UI
'Imports SP.Internal.Automations

'Public Class ClsJobCH

'	Private m_Logger As ILogger = New Logger()
'	Protected m_UtilityUI As UtilityUI
'	Private m_Translate As TranslateValues
'	Private m_utility As Utilities

'	Public Property GetVakNr() As Integer

'	Public Property GetTitelforSearch() As String
'	Public Property GetShortDescription() As String

'	Public Property GetJCHOrganisation_ID() As Integer
'	Public Property GetJCHOrganisation_SubID() As Integer
'	Public Property GetJCHInserat_ID() As Integer
'	Public Property GetJCHLayout_ID() As Integer
'	Public Property GetJCHLogo_ID() As Integer
'	Public Property GetJCHAngebot_Value() As String


'	Public Property GetJCHOur_URL() As String
'	Public Property GetJCHDirekt_URL() As String
'	Public Property GetJCHBewerber_URL() As String
'	Public Property GetJCHSprache() As String

'	Public Property GetJCHPosition() As New List(Of Dictionary(Of Integer, String))

'	Public Property GetJCHBildung_0() As New List(Of Dictionary(Of Integer, String))
'	Public Property GetJCHBildung_1() As New List(Of Dictionary(Of Integer, String))
'	Public Property GetJCHBildung_2() As New List(Of Dictionary(Of Integer, String))

'	Public Property GetJCHBerufErfahrung_0() As New List(Of Dictionary(Of Integer, String))
'	Public Property GetJCHBerufErfahrung_1() As New List(Of Dictionary(Of Integer, String))

'	Public Property GetJCHAnstellungsart() As String
'	Public Property GetJCHAnstellungsgrad() As String
'	Public Property GetJCHAnstellungsgrad_Bis() As String
'	Public Property GetJCHBranche As VacancyJobCHPeripheryViewData

'	Public Property GetJCHXing_Poster_URL() As String
'	Public Property GetJCHXing_Comapany_Profile_URL() As String
'	Public Property GetJCHXing_Company_Is_Poc() As Boolean

'	Public Property GetJCHStart_Date() As Date
'	Public Property GetJCHEnd_Date() As Date
'	Public Property GetJCHIsOnline() As Boolean


'	' Normale Vakanzenfelder...
'	Public Property GetVak_Antrittper() As String
'	Public Property GetVak_ArbPensum() As String
'	Public Property GetVak_Anstellung() As String
'	Public Property GetVak_Dauer() As String
'	Public Property GetVak_MAAge() As String
'	Public Property GetVak_MASex() As String
'	Public Property GetVak_JobCanton() As String
'	Public Property GetVak_Region() As Dictionary(Of Integer, String)
'	Public Property GetVak_IEExport() As String

'	Public Property GetUserKontakt As String
'	Public Property GetUserEMail As String


'#Region "Constructor"

'	Public Sub New()

'		m_UtilityUI = New UtilityUI
'		m_Translate = New TranslateValues
'		m_utility = New Utilities

'	End Sub

'#End Region


'	'Sub DisplayJobCHData(ByVal frmSource As frmJobsCH, ByVal iVakNr As Integer)
'	'  Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'  Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
'	'  Dim i As Integer = 0
'	'  Dim _ClsDb As New ClsDbFunc
'	'  Dim strBez As String = String.Empty

'	'  Try
'	'    Conn.Open()
'	'    Dim cmd As System.Data.SqlClient.SqlCommand
'	'	Dim strQuery As String = "[Get JobCHData For Assinged Vacancy]"	' _ClsDb.GetLocalSQL4JobCHString()
'	'	cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'	cmd.CommandType = CommandType.StoredProcedure
'	'    Dim param As System.Data.SqlClient.SqlParameter
'	'	param = cmd.Parameters.AddWithValue("@CustomerID", ClsDataDetail.ChangedMDData.MDGuid)
'	'	param = cmd.Parameters.AddWithValue("@iVakNr", iVakNr)
'	'    param = cmd.Parameters.AddWithValue("@USNr", ClsDataDetail.ChangedUserData.UserNr)

'	'	Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
'	'    Dim Time_1 As Double = System.Environment.TickCount

'	'    While rFoundedrec.Read
'	'      With frmSource
'	'        .txtTitelforSearch.Text = String.Format("{0}", m_utility.SafeGetString(rFoundedrec, "TitelForSearch"))
'	'        .txtShortDescription.Text = String.Format("{0}", m_utility.SafeGetString(rFoundedrec, "ShortDescription"))

'	'        Try
'	'          .cboJCHAngebot_Art.Properties.Items.Clear()
'	'          .cboJCHAngebot_Art.Properties.Items.Add(String.Format("27 - {0}", m_Translate.GetSafeTranslationValue("Clasic Konto")))
'	'          .cboJCHAngebot_Art.Properties.Items.Add(String.Format("28 - {0}", m_Translate.GetSafeTranslationValue("Clasic Kontingent")))
'	'          .cboJCHAngebot_Art.Properties.Items.Add(String.Format("29 - {0}", m_Translate.GetSafeTranslationValue("Plus Konto")))
'	'          .cboJCHAngebot_Art.Properties.Items.Add(String.Format("30 - {0}", m_Translate.GetSafeTranslationValue("Plus Kontingent")))
'	'          .cboJCHAngebot_Art.Properties.Items.Add(String.Format("31 - {0}", m_Translate.GetSafeTranslationValue("Premium Konto")))
'	'          .cboJCHAngebot_Art.Properties.Items.Add(String.Format("32 - {0}", m_Translate.GetSafeTranslationValue("Premium Kontingent")))

'	'          .deStart.EditValue = CDate(Format(If(String.IsNullOrEmpty(rFoundedrec("StartDate").ToString), Now.Date, rFoundedrec("StartDate")), "d"))
'	'          .deEnd.EditValue = CDate(Format(If(String.IsNullOrEmpty(rFoundedrec("EndDate").ToString),
'	'                                       Now.AddDays(m_utility.SafeGetInteger(rFoundedrec, "DaysToAdd", 0)),
'	'                                       m_utility.SafeGetDateTime(rFoundedrec, "EndDate", Nothing)), "d"))

'	'        Catch ex As Exception
'	'          m_Logger.LogError(String.Format("{0}.cboJCHAngebot_Art: {1}", strMethodeName, ex.Message))

'	'        End Try

'	'        Dim adValues As New List(Of Dictionary(Of Integer, String))
'	'        Try
'	'          .lblJPosition.Text = String.Format("{0}", rFoundedrec("Position_Value").ToString)
'	'          .luePosition.Properties.NullText = String.Format("{0}", rFoundedrec("Position").ToString)

'	'        Catch ex As Exception
'	'          m_Logger.LogError(String.Format("{0}.lblJPosition: {1}", strMethodeName, ex.Message))
'	'          .lblJBranchenID.Text = String.Empty

'	'        End Try

'	'        Try
'	'          .lblJBranchenID.Text = rFoundedrec("BranchenValue").ToString
'	'          .lueBranche.Properties.NullText = rFoundedrec("BranchenBez").ToString

'	'        Catch ex As Exception
'	'          m_Logger.LogError(String.Format("{0}.lblJBranchenID: {1}", strMethodeName, ex.Message))
'	'          .lblJBranchenID.Text = String.Empty

'	'        End Try

'	'        Try
'	'          adValues.Clear()
'	'          adValues = getBildungsNiveauValue(m_utility.SafeGetInteger(rFoundedrec, "VakNr", 0))
'	'          If adValues.Count > 0 Then
'	'            .lblJBildungID_1.Text = String.Format("{0}", adValues(0).Keys(0))
'	'            .lueBNiveau_1.Properties.NullText = String.Format("{0}", adValues(0).Values(0))

'	'            If adValues.Count > 1 Then
'	'              .lblJBildungID_2.Text = String.Format("{0}", adValues(1).Keys(0))
'	'              .lueBNiveau_2.Properties.NullText = String.Format("{0}", adValues(1).Values(0))

'	'              If adValues.Count > 2 Then
'	'                .lblJBildungID_3.Text = String.Format("{0}", adValues(2).Keys(0))
'	'                .lueBNiveau_3.Properties.NullText = String.Format("{0}", adValues(2).Values(0))
'	'              End If

'	'            End If

'	'          End If

'	'        Catch ex As Exception
'	'				m_Logger.LogError(String.Format("{0}.getBildungsNiveauValue: {1}", strMethodeName, ex.ToString))
'	'          .lueBNiveau_1.Properties.NullText = String.Empty
'	'          .lueBNiveau_2.Properties.NullText = String.Empty
'	'          .lueBNiveau_3.Properties.NullText = String.Empty

'	'          .lblJBildungID_1.Text = String.Empty
'	'          .lblJBildungID_2.Text = String.Empty
'	'          .lblJBildungID_3.Text = String.Empty

'	'        End Try

'	'        Try
'	'          Dim liValues As New List(Of String)
'	'          liValues = getBerufErfahrungValue(rFoundedrec("VakNr").ToString)

'	'          For i = 0 To liValues.Count - 1
'	'            Dim aRec As String() = liValues(i).Split(CChar("|"))
'	'            Dim strBeruf As String() = aRec(0).Split(CChar("#"))
'	'            Dim strFachrichtung As String() = aRec(1).Split(CChar("#"))
'	'            Dim strPosition As String() = aRec(2).Split(CChar("#"))

'	'            If i = 0 Then
'	'              .lblJBerufErfahrung_0.Text = String.Format("{0}", strBeruf(0))
'	'              .lueBeruf_1.Properties.NullText = String.Format("{0}", strBeruf(1))
'	'              .lblJBerufErfahrungFach_0.Text = String.Format("{0}", strFachrichtung(0))
'	'              .lueErfahrung_1.Properties.NullText = String.Format("{0}", strFachrichtung(1))
'	'              .lblJBerufErfahrungPosition_0.Text = String.Format("{0}", strPosition(0))
'	'              .lueBEPosition_1.Properties.NullText = String.Format("{0}", strPosition(1))
'	'            Else
'	'              .lblJBerufErfahrung_1.Text = String.Format("{0}", strBeruf(0))
'	'              .lueBeruf_2.Properties.NullText = String.Format("{0}", strBeruf(1))
'	'              .lblJBerufErfahrungFach_1.Text = String.Format("{0}", strFachrichtung(0))
'	'              .lueErfahrung_2.Properties.NullText = String.Format("{0}", strFachrichtung(1))
'	'              .lblJBerufErfahrungPosition_1.Text = String.Format("{0}", strPosition(0))
'	'              .lueBEPosition_2.Properties.NullText = String.Format("{0}", strPosition(1))
'	'            End If
'	'          Next i


'	'        Catch ex As Exception
'	'          m_Logger.LogError(String.Format("{0}.getBerufErfahrungValue: {1}", strMethodeName, ex.Message))
'	'          .lueBeruf_1.Properties.NullText = String.Empty
'	'          .lueErfahrung_1.Properties.NullText = String.Empty
'	'          .lueBEPosition_1.Properties.NullText = String.Empty
'	'          .lueBeruf_2.Properties.NullText = String.Empty
'	'          .lueErfahrung_2.Properties.NullText = String.Empty
'	'          .lueBEPosition_2.Properties.NullText = String.Empty

'	'          .lblJBerufErfahrung_0.Text = String.Empty
'	'          .lblJBerufErfahrungFach_0.Text = String.Empty
'	'          .lblJBerufErfahrungPosition_0.Text = String.Empty
'	'          .lblJBerufErfahrung_1.Text = String.Empty
'	'          .lblJBerufErfahrungFach_1.Text = String.Empty
'	'          .lblJBerufErfahrungPosition_1.Text = String.Empty

'	'        End Try

'	'        Dim strAngebotValue As String = String.Empty

'	'        .cboJCHOrganisation_ID.Text = m_utility.SafeGetString(rFoundedrec, "Organisation_ID")
'	'        .cboJCHOrganisation_SubID.Text = m_utility.SafeGetString(rFoundedrec, "Organisation_SubID")
'	'			strAngebotValue = m_utility.SafeGetString(rFoundedrec, "Angebot_Value")
'	'        .cboJCHLayout_ID.EditValue = m_utility.SafeGetString(rFoundedrec, "Layout_ID")
'	'        .cboJCHLogo_ID.EditValue = m_utility.SafeGetString(rFoundedrec, "Logo_ID")
'	'			.txtJCHOur_URL.Text = m_utility.SafeGetString(rFoundedrec, "Our_URL")

'	'			Dim direkt_url As String = m_utility.SafeGetString(rFoundedrec, "Direkt_URL", "")
'	'			If String.IsNullOrWhiteSpace(direkt_url) OrElse direkt_url Is Nothing Then
'	'				direkt_url = m_utility.SafeGetString(rFoundedrec, "USJCHDirekt_URL", "")
'	'			End If
'	'			.txtJCHDirekt_Link.EditValue = String.Format(direkt_url, iVakNr)
'	'        .txtJCH_Bewerben_URL.Text = m_utility.SafeGetString(rFoundedrec, "Bewerben_URL")

'	'        .cboVakLanguage.Text = m_utility.SafeGetString(rFoundedrec, "Vak_Sprache")

'	'        .txtJCH_Xing_Poster_URL.Text = m_utility.SafeGetString(rFoundedrec, "Xing_Poster_URL")
'	'        .txtJCH_Xing_Company_Profile_URL.Text = m_utility.SafeGetString(rFoundedrec, "Xing_Company_Profile_URL")
'	'        .chkJCHXingIsPoc.Checked = m_utility.SafeGetBoolean(rFoundedrec, "Xing_Company_Is_Poc", False)

'	'        Try
'	'          If Not String.IsNullOrWhiteSpace(strAngebotValue) Then
'	'            If strAngebotValue = "27" Then
'	'              strAngebotValue = String.Format("27 - {0}", m_Translate.GetSafeTranslationValue("Clasic Konto"))
'	'            ElseIf strAngebotValue = "28" Then
'	'              strAngebotValue = String.Format("28 - {0}", m_Translate.GetSafeTranslationValue("Clasic Kontingent"))
'	'            ElseIf strAngebotValue = "29" Then
'	'              strAngebotValue = String.Format("29 - {0}", m_Translate.GetSafeTranslationValue("Plus Konto"))
'	'            ElseIf strAngebotValue = "30" Then
'	'              strAngebotValue = String.Format("30 - {0}", m_Translate.GetSafeTranslationValue("Plus Kontingent"))
'	'            ElseIf strAngebotValue = "31" Then
'	'              strAngebotValue = String.Format("31 - {0}", m_Translate.GetSafeTranslationValue("Premium Konto"))
'	'            ElseIf strAngebotValue = "32" Then
'	'              strAngebotValue = String.Format("32 - {0}", m_Translate.GetSafeTranslationValue("Premium Kontingent"))

'	'            End If
'	'					.cboJCHAngebot_Art.EditValue = strAngebotValue
'	'				End If

'	'        Catch ex As Exception
'	'          m_Logger.LogError(String.Format("{0}.strAngebotValue: {1}", strMethodeName, ex.Message))

'	'        End Try

'	'        .sbIsJCHOnline.Value = m_utility.SafeGetBoolean(rFoundedrec, "IsOnline", False) '  If(IsDBNull(rFoundedrec("IsOnline")), False, CBool(rFoundedrec("IsOnline")))
'	'        .sbIsInternOnline.Value = m_utility.SafeGetBoolean(rFoundedrec, "IEExport", False) '   If(IsDBNull(rFoundedrec("IEExport")), False, CBool(rFoundedrec("IEExport")))


'	'        ' Daten für Kategorien...
'	'			.txt_Antrittper.Text = m_utility.SafeGetString(rFoundedrec, "Beginn")

'	'			Dim jobProzent As String = m_utility.SafeGetString(rFoundedrec, "JobProzent")

'	'			Dim aValue As String()
'	'			If Not String.IsNullOrWhiteSpace(jobProzent) Then
'	'				aValue = jobProzent.Split(CChar("#"))
'	'				For i = 0 To aValue.Length - 1
'	'					If i = 0 Then .seArbpensum_Von.Value = Val(aValue(i)) Else .seArbpensum_Bis.Value = Val(aValue(i))
'	'				Next
'	'			End If

'	'			If Val(.seArbpensum_Von.Text) = 0 Then .seArbpensum_Von.Text = 100
'	'			If Val(.seArbpensum_Bis.Text) = 0 Then .seArbpensum_Bis.Text = 100

'	'			Dim anstellung As String = m_utility.SafeGetString(rFoundedrec, "Anstellung")
'	'			If Not String.IsNullOrWhiteSpace(anstellung) Then
'	'				aValue = anstellung.Split(CChar("#"))
'	'				For i = 0 To aValue.Length - 1
'	'					If Val(aValue(i)) = 1 Then
'	'						.chkAnstellung_1.CheckState = CheckState.Checked
'	'					ElseIf Val(aValue(i)) = 2 Then
'	'						.chkAnstellung_2.CheckState = CheckState.Checked
'	'					ElseIf Val(aValue(i)) = 3 Then
'	'						.chkAnstellung_3.CheckState = CheckState.Checked
'	'					ElseIf Val(aValue(i)) = 4 Then
'	'						.chkAnstellung_4.CheckState = CheckState.Checked
'	'					ElseIf Val(aValue(i)) = 5 Then
'	'						.chkAnstellung_5.CheckState = CheckState.Checked
'	'					ElseIf Val(aValue(i)) = 6 Then
'	'						.chkAnstellung_6.CheckState = CheckState.Checked
'	'					End If
'	'				Next
'	'			End If
'	'			.txt_Dauer.Text = m_utility.SafeGetString(rFoundedrec, "Dauer")

'	'			Dim employeeAge As String = m_utility.SafeGetString(rFoundedrec, "MAAge")
'	'			If Not String.IsNullOrWhiteSpace(employeeAge) Then
'	'				aValue = employeeAge.Split(CChar("#"))
'	'				For i = 0 To aValue.Length - 1
'	'					If Val(aValue(i)) = 1 Then
'	'						.chkAlter_1.CheckState = CheckState.Checked
'	'					ElseIf Val(aValue(i)) = 2 Then
'	'						.chkAlter_2.CheckState = CheckState.Checked
'	'					ElseIf Val(aValue(i)) = 3 Then
'	'						.chkAlter_3.CheckState = CheckState.Checked
'	'					ElseIf Val(aValue(i)) = 4 Then
'	'						.chkAlter_4.CheckState = CheckState.Checked
'	'					End If
'	'				Next
'	'			End If

'	'			Dim employeeGender As String = m_utility.SafeGetString(rFoundedrec, "MASex")
'	'			.lueGender.EditValue = employeeGender ' strValue

'	'			.lueCanton.EditValue = m_utility.SafeGetString(rFoundedrec, "Vak_Kanton")

'	'			Dim dt_1 As New DataTable
'	'			Dim dt_2 As New DataTable
'	'			Dim adRegions As New Dictionary(Of Integer, String)
'	'			.lueRegion_1.Properties.Columns.Clear()
'	'			.lueRegion_2.Properties.Columns.Clear()
'	'			adRegions = getRegionsValue(m_utility.SafeGetInteger(rFoundedrec, "VakNr", 0))

'	'			.lblRegion1_ID.Text = String.Format("{0}", adRegions.Keys(0))
'	'			.lblRegion2_ID.Text = String.Format("{0}", adRegions.Keys(1))

'	'			.lueRegion_1.Properties.NullText = String.Format("{0}", adRegions.Values(0))
'	'			.lueRegion_2.Properties.NullText = String.Format("{0}", adRegions.Values(1))

'	'			dt_1.Columns.Add("RecValue", GetType(String))
'	'			dt_1.Columns.Add("Bezeichnung", GetType(String))
'	'			dt_2.Columns.Add("RecValue", GetType(String))
'	'			dt_2.Columns.Add("Bezeichnung", GetType(String))

'	'			dt_1.Rows.Add(New Object() {adRegions.Keys(0), m_Translate.GetSafeTranslationValue(adRegions.Values(0))})
'	'			dt_2.Rows.Add(New Object() {adRegions.Keys(1), m_Translate.GetSafeTranslationValue(adRegions.Values(1))})

'	'			.lueRegion_1.Properties.DataSource = dt_1
'	'			.lueRegion_2.Properties.DataSource = dt_2

'	'			.lueRegion_1.Properties.DisplayMember = "Bezeichnung"
'	'			.lueRegion_1.Properties.ValueMember = "RecValue"
'	'			.lueRegion_2.Properties.DisplayMember = "Bezeichnung"
'	'			.lueRegion_2.Properties.ValueMember = "RecValue"

'	'			Dim Col0 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID_1", "RecValue", 0)
'	'			Dim Col1 As New DevExpress.XtraEditors.Controls.LookUpColumnInfo("Bezeichnung", m_Translate.GetSafeTranslationValue("Bezeichnung"), 100)

'	'			Col0.Visible = False
'	'			.lueRegion_1.Properties.Columns.Add(Col0)
'	'			.lueRegion_1.Properties.Columns.Add(Col1)

'	'			.lueRegion_2.Properties.Columns.Add(Col0)
'	'			.lueRegion_2.Properties.Columns.Add(Col1)

'	'			.lueRegion_1.Properties.NullText = String.Format("{0}", adRegions.Values(0))
'	'			.lueRegion_2.Properties.NullText = String.Format("{0}", adRegions.Values(1))

'	'			DisplayFoundedLanguageData(iVakNr, .lstMSprache, "Vak_MSprachen")

'	'			.urlUserContact.HtmlContent = m_utility.SafeGetString(rFoundedrec, "UserKontakt")
'	'			.txtUserEMail.Text = m_utility.SafeGetString(rFoundedrec, "UserEMail")

'	'		End With

'	'      i += 1
'	'    End While
'	'    Console.WriteLine(String.Format("Zeit für DisplayJobCHData: {0} s", _
'	'                                    ((System.Environment.TickCount - Time_1) / 1000).ToString()))

'	'  Catch e As Exception
'	'    m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.StackTrace))
'	'    m_UtilityUI.ShowErrorDialog(e.Message)

'	'  Finally
'	'    Conn.Close()
'	'    Conn.Dispose()

'	'  End Try

'	'End Sub

'	'Function getBildungsNiveauValue(ByVal iVakNr As Integer) As List(Of Dictionary(Of Integer, String))
'	'  Dim adValue As New List(Of Dictionary(Of Integer, String))
'	'  Dim strQuery As String = "Select * From [Vak_JobCHBildungsNiveauData] Where VakNr = @VakNr Order By ID"
'	'  Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

'	'  Try
'	'    Conn.Open()

'	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'    cmd.CommandType = Data.CommandType.Text
'	'    Dim param As System.Data.SqlClient.SqlParameter
'	'	param = cmd.Parameters.AddWithValue("@VakNr", iVakNr)

'	'	Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
'	'    While rFoundedrec.Read
'	'      adValue.Add(New Dictionary(Of Integer, String) From { _
'	'                  {CInt(Val(rFoundedrec("Bez_Value"))), rFoundedrec("Bezeichnung").ToString}})

'	'    End While

'	'  Catch e As Exception
'	'    m_Logger.LogError(e.StackTrace)

'	'    Dim strMessage As String = String.Format("Error (getBildungsNiveauValue): {0}", e.Message)
'	'    m_UtilityUI.ShowErrorDialog(strMessage)

'	'  Finally
'	'    Conn.Close()
'	'    Conn.Dispose()

'	'  End Try

'	'  Return adValue
'	'End Function

'	'Function getBerufErfahrungValue(ByVal iVakNr As Integer) As List(Of String) 'Dictionary(Of Integer, String))
'	'  Dim adValue As New List(Of String) ' Dictionary(Of Integer, String))
'	'  Dim strQuery As String = "Select * From [Vak_JobCHBerufgruppeData] "
'	'  strQuery &= "Where VakNr = @VakNr And ForExperience = 1 Order By ID"
'	'  Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

'	'  Try
'	'    Conn.Open()

'	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'    cmd.CommandType = Data.CommandType.Text
'	'    Dim param As System.Data.SqlClient.SqlParameter
'	'	param = cmd.Parameters.AddWithValue("@VakNr", iVakNr)

'	'	Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
'	'    While rFoundedrec.Read
'	'      Dim strBeruf As String = String.Format("{0}#{1}|{2}#{3}|{4}#{5}",
'	'                                             CInt(Val(rFoundedrec("BerufGruppe_Value"))), rFoundedrec("BerufGruppe").ToString,
'	'                                             CInt(Val(rFoundedrec("Fachrichtung_Value"))), rFoundedrec("Fachrichtung").ToString,
'	'                                             CInt(Val(rFoundedrec("Position_Value"))), rFoundedrec("Position").ToString)
'	'      adValue.Add(strBeruf)

'	'    End While

'	'  Catch e As Exception
'	'    m_Logger.LogError(e.StackTrace)

'	'    Dim strMessage As String = String.Format("Error (getBerufErfahrungValue): {0}", e.Message)
'	'    m_UtilityUI.ShowErrorDialog(strMessage)

'	'  Finally
'	'    Conn.Close()
'	'    Conn.Dispose()

'	'  End Try

'	'  Return adValue
'	'End Function

'	'Sub DisplayFoundedBrancheData(ByVal iVakNr As Integer, ByVal lst As DevExpress.XtraEditors.ListBoxControl, ByVal strTblName As String)
'	'	Dim strQuery As String = "Select * From {0} Where VakNr = @VakNr Order By Bezeichnung"
'	'	strQuery = String.Format(strQuery, strTblName)
'	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

'	'	Try
'	'		Conn.Open()

'	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'		cmd.CommandType = Data.CommandType.Text
'	'		Dim param As System.Data.SqlClient.SqlParameter
'	'		param = cmd.Parameters.AddWithValue("@VakNr", iVakNr)

'	'		Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
'	'		While rFoundedrec.Read
'	'			lst.Items.Add(String.Format("{0}", rFoundedrec("Bezeichnung")))

'	'		End While

'	'	Catch e As Exception
'	'		m_Logger.LogError(e.StackTrace)

'	'		Dim strMessage As String = String.Format("Fehler (DisplayFoundedBrancheData): {0}", e.Message)
'	'		m_UtilityUI.ShowErrorDialog(strMessage)

'	'	Finally
'	'		Conn.Close()
'	'		Conn.Dispose()

'	'	End Try

'	'End Sub

'	'Public Function VacancyPublicationFields(ByVal vacancyNumber As Integer) As PublicationFields
'	'   Dim result As New PublicationFields
'	'   Dim sql As String

'	'   sql = "Select Top 1 J_Zusatz_Jobs_Vorspann, J_Zusatz_Jobs_Aufgabe, J_Zusatz_Jobs_Anforderung, J_Zusatz_Jobs_WirBieten From Vak_JobCHData where VakNr = @vacancyNumber "
'	'   Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

'	'   Try
'	'     Conn.Open()

'	'     Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
'	'     cmd.CommandType = Data.CommandType.Text
'	'     Dim param As System.Data.SqlClient.SqlParameter
'	'     param = cmd.Parameters.AddWithValue("@vacancyNumber", vacancyNumber)

'	'     Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader
'	'     While rFoundedrec.Read

'	'       result.vorspann = Not String.IsNullOrEmpty(m_utility.SafeGetString(rFoundedrec, "J_Zusatz_Jobs_Vorspann"))
'	'       result.taetigkeit = Not String.IsNullOrEmpty(m_utility.SafeGetString(rFoundedrec, "J_Zusatz_Jobs_Aufgabe"))
'	'       result.anforderung = Not String.IsNullOrEmpty(m_utility.SafeGetString(rFoundedrec, "J_Zusatz_Jobs_Anforderung"))
'	'       result.wirbieten = Not String.IsNullOrEmpty(m_utility.SafeGetString(rFoundedrec, "J_Zusatz_Jobs_WirBieten"))

'	'     End While

'	'   Catch e As Exception
'	'     m_Logger.LogError(e.ToString)

'	'     Dim strMessage As String = String.Format("Fehler: {0}", e.Message)
'	'     m_UtilityUI.ShowErrorDialog(strMessage)

'	'   Finally
'	'     Conn.Close()
'	'     Conn.Dispose()

'	'   End Try


'	'   Return result

'	' End Function



'	'Function GetSQLString4JobCH(ByVal iRecNr As Integer) As String
'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'	Dim strSQL As String = String.Empty

'	'	If iRecNr <> 0 Then
'	'		strSQL &= "Update Vak_JobCHData Set "
'	'		strSQL &= "Organisation_ID = @Organisation_ID, Organisation_SubID = @Organisation_SubID, Inserat_ID = @Inserat_ID, "
'	'		strSQL &= "Our_URL = @Our_URL, "
'	'		strSQL &= "Direkt_URL = @Direkt_URL, Vak_Sprache = @Vak_Sprache, "
'	'		strSQL &= "Position_Value = @Position_Value, Position = @Position, "

'	'		strSQL &= "Layout_ID = @Layout_ID, Logo_ID = @Logo_ID, Bewerben_URL = @Bewerben_URL, "
'	'		strSQL &= "Angebot_Value = @Angebot_Value, Xing_Poster_URL = @Xing_Poster_URL, "
'	'		strSQL &= "Xing_Company_Profile_URL = @Xing_Company_Profile_URL, "
'	'		strSQL &= "Xing_Company_Is_Poc = @Xing_Company_Is_Poc, "
'	'		strSQL &= "StartDate = @StartDate, EndDate = @EndDate, IsOnline = @IsOnline, "

'	'		strSQL &= "ChangedOn = @ChangedOn, ChangedFrom = @ChangedFrom "
'	'		strSQL &= "Where VakNr = @VakNr "

'	'		strSQL &= "Update Vakanzen Set TitelForSearch = @TitelForSearch, ShortDescription = @ShortDescription, "
'	'		strSQL &= "Beginn = @Beginn, "
'	'		strSQL &= "JobProzent = @JobProzent, Anstellung = @Anstellung, Dauer = @Dauer, "
'	'		strSQL &= "MAAge = @MAAge, MASex = @MASex, "
'	'		strSQL &= "IEExport = @IEExport, "
'	'		strSQL &= "Vak_Kanton = @Vak_Kanton, "
'	'		strSQL &= "UserKontakt = @UserKontakt, UserEMail = @UserEMail "

'	'		strSQL &= "Where VakNr = @VakNr "

'	'	End If
'	'	ClsDataDetail.SQLQuery = strSQL

'	'	Return strSQL
'	'End Function

'	'Function SaveJobCHDataToVakanzDb(ByVal iVakNr As Integer) As Boolean
'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
'	'	Dim strQuery As String = GetSQLString4JobCH(iVakNr)
'	'	Dim iNewRecNr As Integer = iVakNr
'	'	Dim bAsNewrec As Boolean = iVakNr = 0
'	'	Dim bResult As Boolean
'	'	Dim strMessage As New StringBuilder()

'	'	Try
'	'		Conn.Open()
'	'		Dim cmd As System.Data.SqlClient.SqlCommand
'	'		cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'		Dim param As System.Data.SqlClient.SqlParameter

'	'		param = cmd.Parameters.AddWithValue("@VakNr", iNewRecNr)
'	'		param = cmd.Parameters.AddWithValue("@TitelForSearch", Me.GetTitelforSearch)
'	'		param = cmd.Parameters.AddWithValue("@ShortDescription", Me.GetShortDescription)

'	'		param = cmd.Parameters.AddWithValue("@Organisation_ID", Me.GetJCHOrganisation_ID)
'	'		param = cmd.Parameters.AddWithValue("@Organisation_SubID", Me.GetJCHOrganisation_SubID)
'	'		param = cmd.Parameters.AddWithValue("@Inserat_ID", Me.GetJCHInserat_ID)

'	'		param = cmd.Parameters.AddWithValue("@Our_URL", Me.GetJCHOur_URL)
'	'		param = cmd.Parameters.AddWithValue("@Direkt_URL", Me.GetJCHDirekt_URL)
'	'		param = cmd.Parameters.AddWithValue("@Vak_Sprache", Me.GetJCHSprache)

'	'		If Me.GetJCHPosition.Count > 0 Then
'	'			param = cmd.Parameters.AddWithValue("@Position_Value", Me.GetJCHPosition(0).Keys(0))
'	'			param = cmd.Parameters.AddWithValue("@Position", Me.GetJCHPosition(0).Values(0))
'	'		Else
'	'			param = cmd.Parameters.AddWithValue("@Position_Value", 0)
'	'			param = cmd.Parameters.AddWithValue("@Position", String.Empty)

'	'		End If

'	'		param = cmd.Parameters.AddWithValue("@Layout_ID", Me.GetJCHLayout_ID)
'	'		param = cmd.Parameters.AddWithValue("@Logo_ID", Me.GetJCHLogo_ID)

'	'		param = cmd.Parameters.AddWithValue("@Bewerben_URL", Me.GetJCHBewerber_URL)
'	'		param = cmd.Parameters.AddWithValue("@Angebot_Value", Me.GetJCHAngebot_Value)
'	'		param = cmd.Parameters.AddWithValue("@Xing_Poster_URL", Me.GetJCHXing_Poster_URL)
'	'		param = cmd.Parameters.AddWithValue("@Xing_Company_Profile_URL", Me.GetJCHXing_Comapany_Profile_URL)
'	'		param = cmd.Parameters.AddWithValue("@Xing_Company_Is_Poc", Me.GetJCHXing_Company_Is_Poc)
'	'		param = cmd.Parameters.AddWithValue("@StartDate", Me.GetJCHStart_Date)
'	'		param = cmd.Parameters.AddWithValue("@EndDate", Me.GetJCHEnd_Date)
'	'		param = cmd.Parameters.AddWithValue("@IsOnline", Me.GetJCHIsOnline)

'	'		param = cmd.Parameters.AddWithValue("@ChangedOn", Format(Now, "g"))
'	'		param = cmd.Parameters.AddWithValue("@ChangedFrom", String.Format("{0}, {1}", ClsDataDetail.UserData.UserLName, ClsDataDetail.UserData.UserFName))

'	'		' Normale Vakanzenfelder
'	'		param = cmd.Parameters.AddWithValue("@Beginn", Me.GetVak_Antrittper)
'	'		param = cmd.Parameters.AddWithValue("@JobProzent", Me.GetVak_ArbPensum)
'	'		param = cmd.Parameters.AddWithValue("@Anstellung", Me.GetVak_Anstellung)
'	'		param = cmd.Parameters.AddWithValue("@Dauer", Me.GetVak_Dauer)
'	'		param = cmd.Parameters.AddWithValue("@MAAge", Me.GetVak_MAAge)
'	'		param = cmd.Parameters.AddWithValue("@MASex", m_utility.ReplaceMissing(Me.GetVak_MASex, DBNull.Value))
'	'		param = cmd.Parameters.AddWithValue("@IEExport", Me.GetVak_IEExport)
'	'		param = cmd.Parameters.AddWithValue("@Vak_Kanton", m_utility.ReplaceMissing(Me.GetVak_JobCanton, DBNull.Value))

'	'		param = cmd.Parameters.AddWithValue("@UserKontakt", Me.GetUserKontakt.ToString)
'	'		param = cmd.Parameters.AddWithValue("@UserEMail", Me.GetUserEMail)


'	'		For i As Integer = 0 To cmd.Parameters.Count - 1
'	'			strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
'	'																			cmd.Parameters(i).ParameterName,
'	'																			cmd.Parameters(i).DbType,
'	'																			cmd.Parameters(i).Size,
'	'																			cmd.Parameters(i).Value,
'	'																			ControlChars.NewLine))
'	'		Next

'	'		cmd.ExecuteNonQuery()
'	'		cmd.Parameters.Clear()
'	'		Dim _clsRegion As New InsertData2Region
'	'		If Me.GetVak_Region.Count = 0 Then
'	'			_clsRegion.DeleteRegionsFromDb(iNewRecNr)
'	'		Else
'	'			_clsRegion.SaveRegionsIntoDb(iVakNr, Me.GetVak_Region)

'	'		End If

'	'		Dim _ClsJobCH As New InsertJCHBerufGruppe
'	'		If Me.GetJCHBranche.Count > 0 Then _ClsJobCH.SaveJCHBranchenIntoDb(iVakNr, Me.GetJCHBranche)
'	'		If Me.GetJCHBerufErfahrung_0.Count > 0 Then _ClsJobCH.SaveJCHBerufErfahrungIntoDb(iVakNr, If(Me.GetJCHBerufErfahrung_1.Count > Me.GetJCHBerufErfahrung_0.Count, Me.GetJCHBerufErfahrung_1,
'	'				Me.GetJCHBerufErfahrung_0), If(Me.GetJCHBerufErfahrung_1.Count > Me.GetJCHBerufErfahrung_0.Count, Me.GetJCHBerufErfahrung_0, Me.GetJCHBerufErfahrung_1))
'	'		If Me.GetJCHBildung_0.Count > 0 Then _ClsJobCH.SaveJCHBildungsniveauIntoDb(iVakNr, Me.GetJCHBildung_0(0), Me.GetJCHBildung_1(0), Me.GetJCHBildung_2(0))

'	'		bResult = True

'	'	Catch ex As SqlException
'	'		Dim errorMessages As New StringBuilder()
'	'		For i = 0 To ex.Errors.Count - 1
'	'			errorMessages.Append("Index #" & i.ToString() & ControlChars.NewLine _
'	'					& "Message: " & ex.Errors(i).Message & ControlChars.NewLine _
'	'					& "LineNumber: " & ex.Errors(i).LineNumber & ControlChars.NewLine _
'	'					& "Source: " & ex.Errors(i).Source & ControlChars.NewLine _
'	'					& "Procedure: " & ex.Errors(i).Procedure & ControlChars.NewLine)
'	'		Next i
'	'		m_Logger.LogError(String.Format("{0}: {1}.{2}", strMethodeName, strMessage, ex.Message))


'	'		MsgBox(errorMessages.ToString, MsgBoxStyle.Critical, "Daten speichern")
'	'		Trace.WriteLine(errorMessages.ToString())


'	'		Me.GetVakNr = If(bAsNewrec, 0, iNewRecNr)
'	'		bResult = False

'	'	Catch ex As Exception
'	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))
'	'		MsgBox(ex.Message, MsgBoxStyle.Critical, strMethodeName)
'	'		Me.GetVakNr = If(bAsNewrec, 0, iNewRecNr)
'	'		bResult = False

'	'	End Try

'	'	Return bResult
'	'End Function


'	'Function LoadOJData(ByVal iVakNr As Integer) As OstJobData
'	'  Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'  Dim result = m_DataAccess.loadOstJobData(iVakNr, ClsDataDetail.ChangedMDData.MDGuid, ClsDataDetail.ChangedUserData.UserNr)

'	'  Return result
'	'End Function


'	'Private Class InsertData2Region

'	'Sub SaveRegionsIntoDb(ByVal iVakNr As Integer, ByVal adValue As Dictionary(Of Integer, String))
'	'	Dim strQuery As String = "Delete Vak_JobCHRegionData Where VakNr = @VakNr "
'	'	strQuery &= "Insert Into Vak_JobCHRegionData (VakNr, Bez_Value, Bezeichnung) Values "
'	'	strQuery &= "(@VakNr, @Bez_Value_0, @Bezeichnung_0) "
'	'	If adValue.Count > 1 Then
'	'		strQuery &= "Insert Into Vak_JobCHRegionData (VakNr, Bez_Value, Bezeichnung) Values "
'	'		strQuery &= "(@VakNr, @Bez_Value_1, @Bezeichnung_1)"
'	'	End If


'	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

'	'	Try
'	'		Conn.Open()

'	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'		cmd.CommandType = Data.CommandType.Text
'	'		Dim param As System.Data.SqlClient.SqlParameter

'	'		param = cmd.Parameters.AddWithValue("@VakNr", iVakNr)
'	'		param = cmd.Parameters.AddWithValue("@Bez_Value_0", adValue.Keys(0))
'	'		param = cmd.Parameters.AddWithValue("@Bezeichnung_0", adValue.Values(0))

'	'		If adValue.Count > 1 Then
'	'			param = cmd.Parameters.AddWithValue("@Bez_Value_1", adValue.Keys(1))
'	'			param = cmd.Parameters.AddWithValue("@Bezeichnung_1", adValue.Values(1))
'	'		End If
'	'		cmd.ExecuteNonQuery()

'	'	Catch e As Exception
'	'		Dim strMessage As String = String.Format("Error (SaveRegionsIntoDb): {0}", e.Message)
'	'		MsgBox(strMessage, MsgBoxStyle.Critical, "Regionen speichern")

'	'	Finally
'	'		Conn.Close()
'	'		Conn.Dispose()

'	'	End Try

'	'End Sub

'	'Sub DeleteRegionsFromDb(ByVal iVakNr As Integer)
'	'	Dim strQuery As String = "Delete Vak_JobCHRegionData Where VakNr = @VakNr "
'	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

'	'	Try
'	'		Conn.Open()

'	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'		cmd.CommandType = Data.CommandType.Text
'	'		Dim param As System.Data.SqlClient.SqlParameter

'	'		param = cmd.Parameters.AddWithValue("@VakNr", iVakNr)
'	'		cmd.ExecuteNonQuery()

'	'	Catch e As Exception
'	'		Dim strMessage As String = String.Format("Error (DeleteRegionsFromDb): {0}", e.Message)
'	'		MsgBox(strMessage, MsgBoxStyle.Critical, "Regionen löschen")

'	'	Finally
'	'		Conn.Close()
'	'		Conn.Dispose()

'	'	End Try

'	'End Sub

'	'End Class



'	'Private Class InsertJCHBerufGruppe
'	'	Private m_Logger As ILogger = New Logger()
'	'	Protected m_DataAccess As IDatabaseAccess
'	'	Protected m_UtilityUI As UtilityUI
'	'	Private m_Translate As New TranslateValues

'	'	'Sub SaveJCHBerufErfahrungIntoDb(ByVal iVakNr As Integer, ByVal adValue_0 As List(Of Dictionary(Of Integer, String)),
'	'	'												 ByVal adValue_1 As List(Of Dictionary(Of Integer, String)))
'	'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'	'	'If adValue_0(0).Values(0) = String.Empty Then Exit Sub

'	'	'	Dim strQuery As String = "Delete Vak_JobCHBerufgruppeData Where VakNr = @VakNr And ForExperience = 1 "
'	'	'	If adValue_0(0).Values(0) <> String.Empty Then
'	'	'		strQuery &= "Insert Into Vak_JobCHBerufgruppeData (VakNr, "
'	'	'		strQuery &= "BerufGruppe_Value, BerufGruppe, "
'	'	'		strQuery &= "Fachrichtung_Value, Fachrichtung, "
'	'	'		strQuery &= "Position_Value, Position, ForExperience "

'	'	'		strQuery &= ") Values ("
'	'	'		strQuery &= "@VakNr, @BerufGruppe_Value, @BerufGruppe, "
'	'	'		strQuery &= "@Fachrichtung_Value, @Fachrichtung, "
'	'	'		strQuery &= "@Position_Value, @Position, @ForExperience) "

'	'	'		If adValue_1.Count > 0 Then
'	'	'			If adValue_1(0).Keys(0) <> 0 Then
'	'	'				strQuery &= "Insert Into Vak_JobCHBerufgruppeData (VakNr, "
'	'	'				strQuery &= "BerufGruppe_Value, BerufGruppe, "
'	'	'				strQuery &= "Fachrichtung_Value, Fachrichtung, "
'	'	'				strQuery &= "Position_Value, Position, ForExperience "

'	'	'				strQuery &= ") Values ("
'	'	'				strQuery &= "@VakNr, @BerufGruppe_Value_1, @BerufGruppe_1, "
'	'	'				strQuery &= "@Fachrichtung_Value_1, @Fachrichtung_1, "
'	'	'				strQuery &= "@Position_Value_1, @Position_1, @ForExperience) "
'	'	'			End If
'	'	'		End If
'	'	'	End If

'	'	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

'	'	'	Try
'	'	'		'If adValue_0(0).Values(0) = String.Empty Then Exit Sub

'	'	'		Conn.Open()

'	'	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'	'		cmd.CommandType = Data.CommandType.Text
'	'	'		Dim param As System.Data.SqlClient.SqlParameter

'	'	'		param = cmd.Parameters.AddWithValue("@VakNr", iVakNr)
'	'	'		If adValue_0(0).Values(0) <> String.Empty Then
'	'	'			param = cmd.Parameters.AddWithValue("@BerufGruppe_Value", adValue_0(0).Keys(0))
'	'	'			param = cmd.Parameters.AddWithValue("@BerufGruppe", adValue_0(0).Values(0))
'	'	'			param = cmd.Parameters.AddWithValue("@Fachrichtung_Value", adValue_0(1).Keys(0))
'	'	'			param = cmd.Parameters.AddWithValue("@Fachrichtung", adValue_0(1).Values(0))
'	'	'			param = cmd.Parameters.AddWithValue("@Position_Value", adValue_0(2).Keys(0))
'	'	'			param = cmd.Parameters.AddWithValue("@Position", adValue_0(2).Values(0))

'	'	'			If adValue_1.Count > 0 Then
'	'	'				If adValue_1(0).Keys(0) <> 0 Then
'	'	'					param = cmd.Parameters.AddWithValue("@BerufGruppe_Value_1", adValue_1(0).Keys(0))
'	'	'					param = cmd.Parameters.AddWithValue("@BerufGruppe_1", adValue_1(0).Values(0))
'	'	'					param = cmd.Parameters.AddWithValue("@Fachrichtung_Value_1", adValue_1(1).Keys(0))
'	'	'					param = cmd.Parameters.AddWithValue("@Fachrichtung_1", adValue_1(1).Values(0))
'	'	'					param = cmd.Parameters.AddWithValue("@Position_Value_1", adValue_1(2).Keys(0))
'	'	'					param = cmd.Parameters.AddWithValue("@Position_1", adValue_1(2).Values(0))
'	'	'				End If
'	'	'			End If
'	'	'			param = cmd.Parameters.AddWithValue("@ForExperience", 1)
'	'	'		End If

'	'	'		cmd.ExecuteNonQuery()

'	'	'	Catch e As Exception
'	'	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
'	'	'		Dim strMessage As String = String.Format("Error: {1}. {0}", e.Message, strMethodeName)
'	'	'		MsgBox(strMessage, MsgBoxStyle.Critical, "Berufsgruppen für Jobs.ch speichern")

'	'	'	Finally
'	'	'		Conn.Close()
'	'	'		Conn.Dispose()

'	'	'	End Try

'	'	'End Sub

'	'	'Sub SaveJCHBildungsniveauIntoDb(ByVal iVakNr As Integer, ByVal adValue_0 As Dictionary(Of Integer, String),
'	'	'																ByVal adValue_1 As Dictionary(Of Integer, String),
'	'	'																ByVal adValue_2 As Dictionary(Of Integer, String))
'	'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'	'	Dim strQuery As String = "Delete Vak_JobCHBildungsniveauData Where VakNr = @VakNr "

'	'	'	If adValue_0.Count > 0 Then
'	'	'		If adValue_0.Values(0) <> String.Empty Then
'	'	'			strQuery &= "Insert Into Vak_JobCHBildungsniveauData (VakNr, "
'	'	'			strQuery &= "Bez_Value, Bezeichnung "
'	'	'			strQuery &= ") Values ("
'	'	'			strQuery &= "@VakNr, @Bez_Value, @Bezeichnung) "
'	'	'		End If
'	'	'	End If
'	'	'	If adValue_1.Count > 0 Then
'	'	'		If adValue_1.Values(0) <> String.Empty Then
'	'	'			strQuery &= "Insert Into Vak_JobCHBildungsniveauData (VakNr, "
'	'	'			strQuery &= "Bez_Value, Bezeichnung "

'	'	'			strQuery &= ") Values ("
'	'	'			strQuery &= "@VakNr, @Bez_Value_1, @Bezeichnung_1) "
'	'	'		End If
'	'	'	End If

'	'	'	If adValue_2.Count > 0 Then
'	'	'		If adValue_2.Values(0) <> String.Empty Then
'	'	'			strQuery &= "Insert Into Vak_JobCHBildungsniveauData (VakNr, "
'	'	'			strQuery &= "Bez_Value, Bezeichnung "

'	'	'			strQuery &= ") Values ("
'	'	'			strQuery &= "@VakNr, @Bez_Value_2, @Bezeichnung_2) "
'	'	'		End If
'	'	'	End If

'	'	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

'	'	'	Try
'	'	'		Conn.Open()

'	'	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'	'		cmd.CommandType = Data.CommandType.Text
'	'	'		Dim param As System.Data.SqlClient.SqlParameter

'	'	'		param = cmd.Parameters.AddWithValue("@VakNr", iVakNr)
'	'	'		If adValue_0.Count > 0 Then
'	'	'			If adValue_0.Values(0) <> String.Empty Then
'	'	'				param = cmd.Parameters.AddWithValue("@Bez_Value", adValue_0.Keys(0))
'	'	'				param = cmd.Parameters.AddWithValue("@Bezeichnung", adValue_0.Values(0))
'	'	'			End If
'	'	'		End If

'	'	'		If adValue_1.Count > 0 Then
'	'	'			If adValue_1.Values(0) <> String.Empty Then
'	'	'				param = cmd.Parameters.AddWithValue("@Bez_Value_1", adValue_1.Keys(0))
'	'	'				param = cmd.Parameters.AddWithValue("@Bezeichnung_1", adValue_1.Values(0))
'	'	'			End If
'	'	'		End If
'	'	'		If adValue_2.Count > 0 Then
'	'	'			If adValue_2.Values(0) <> String.Empty Then
'	'	'				param = cmd.Parameters.AddWithValue("@Bez_Value_2", adValue_2.Keys(0))
'	'	'				param = cmd.Parameters.AddWithValue("@Bezeichnung_2", adValue_2.Values(0))
'	'	'			End If
'	'	'		End If

'	'	'		cmd.ExecuteNonQuery()

'	'	'	Catch e As Exception
'	'	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
'	'	'		Dim strMessage As String = String.Format("Error: {1}. {0}", e.Message, strMethodeName)
'	'	'		MsgBox(strMessage, MsgBoxStyle.Critical, "Bildungsniveau für Jobs.ch speichern")

'	'	'	Finally
'	'	'		Conn.Close()
'	'	'		Conn.Dispose()

'	'	'	End Try

'	'	'End Sub

'	'	'Sub SaveJCHBranchenIntoDb(ByVal iVakNr As Integer, ByVal adValue_0 As Dictionary(Of Integer, String))
'	'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'	'	Dim strQuery As String = "Delete Vak_JobCHBranchenData Where VakNr = @VakNr "

'	'	'	If adValue_0.Values(0) <> String.Empty Then
'	'	'		strQuery &= "Insert Into Vak_JobCHBranchenData (VakNr, "
'	'	'		strQuery &= "Bez_Value, Bezeichnung "

'	'	'		strQuery &= ") Values ("
'	'	'		strQuery &= "@VakNr, @Bez_Value, @Bezeichnung) "
'	'	'	End If

'	'	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

'	'	'	Try
'	'	'		Conn.Open()

'	'	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'	'		cmd.CommandType = Data.CommandType.Text
'	'	'		Dim param As System.Data.SqlClient.SqlParameter

'	'	'		param = cmd.Parameters.AddWithValue("@VakNr", iVakNr)
'	'	'		If adValue_0.Values(0) <> String.Empty Then
'	'	'			param = cmd.Parameters.AddWithValue("@Bez_Value", adValue_0.Keys(0))
'	'	'			param = cmd.Parameters.AddWithValue("@Bezeichnung", adValue_0.Values(0))
'	'	'		End If

'	'	'		cmd.ExecuteNonQuery()

'	'	'	Catch e As Exception
'	'	'		m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
'	'	'		Dim strMessage As String = String.Format("Error: {1}. {0}", e.Message, strMethodeName)
'	'	'		MsgBox(strMessage, MsgBoxStyle.Critical, "Bildungsniveau für Jobs.ch speichern")

'	'	'	Finally
'	'	'		Conn.Close()
'	'	'		Conn.Dispose()

'	'	'	End Try

'	'	'End Sub

'	'	'Sub DeleteJCHBerufErfahrungFromDb(ByVal iVakNr As Integer)
'	'	'	Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
'	'	'	Dim strQuery As String = "Delete Vak_JobCHBerufgruppeData Where VakNr = @VakNr And ForExperience = 1 "
'	'	'	Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

'	'	'	Try
'	'	'		Conn.Open()

'	'	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
'	'	'		cmd.CommandType = Data.CommandType.Text
'	'	'		Dim param As System.Data.SqlClient.SqlParameter

'	'	'		param = cmd.Parameters.AddWithValue("@VakNr", iVakNr)
'	'	'		cmd.ExecuteNonQuery()

'	'	'	Catch e As Exception
'	'	'		m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, e.Message))
'	'	'		Dim strMessage As String = String.Format("Error: {1}. {0}", e.Message, strMethodeName)
'	'	'		MsgBox(strMessage, MsgBoxStyle.Critical, "Beruferfahrungen löschen")

'	'	'	Finally
'	'	'		Conn.Close()
'	'	'		Conn.Dispose()

'	'	'	End Try

'	'	'End Sub

'	'End Class




'End Class
