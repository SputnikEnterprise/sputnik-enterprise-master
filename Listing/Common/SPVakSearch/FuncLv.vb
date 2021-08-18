
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data

Module FuncLv

  Private _ClsFunc As New ClsDivFunc
  Private _ClsReg As New SPProgUtility.ClsDivReg
  'Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Private m_xml As New ClsXML


  Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

    Dim m_md As New SPProgUtility.Mandanten.Mandant
    Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
    Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
    Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

    Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
    Dim translate = clsTransalation.GetTranslationInObject

    Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

  End Function


#Region "Dropdown-Funktionen für 1. Seite..."
  ' Sortierung
  Sub ListSort(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)
    cbo.Properties.Items.Clear()
    cbo.Properties.Items.Add(String.Format("0 - {0}", m_xml.GetSafeTranslationValue("Vakanzennummer")))
    cbo.Properties.Items.Add(String.Format("1 - {0}", m_xml.GetSafeTranslationValue("Bezeichnung")))
    cbo.Properties.Items.Add(String.Format("2 - {0}", m_xml.GetSafeTranslationValue("Erfassdatum")))
    cbo.Properties.Items.Add(String.Format("3 - {0}", m_xml.GetSafeTranslationValue("Firma")))
  End Sub
  ' Antritt per
  Sub ListVakAntrittPer(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    ListComboBox(cbo, "Beginn")
  End Sub
  ' Beschäftigung
  Sub ListVakBeschaeftigung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    ListComboBox(cbo, "JobProzent")
  End Sub

  ' Dauer
  Sub ListVakDauer(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    ListComboBox(cbo, "Dauer")
  End Sub

  ' Geschlecht
  Sub ListVakGeschlecht(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    ListComboBox(cbo, "MASex")
  End Sub
  ' Zivilstand
  Sub ListVakZivilstand(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    ListComboBox(cbo, "MAZivil")
  End Sub
  ' Lohn
  Sub ListVakLohn(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    ListComboBox(cbo, "MALohn")
  End Sub
  ' Arbeitszeit
  Sub ListVakArbeitszeit(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    ListComboBox(cbo, "Jobtime")
  End Sub
  ' Arbeitsort
  Sub ListVakArbeitsort(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    ListComboBox(cbo, "JobOrt")
  End Sub

  ' Gruppe
  Sub ListVakGruppe(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    ListComboBox(cbo, "Gruppe")
  End Sub

  ' Geschäftsstelle
  Sub ListVakGeschaeftsstelle(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    ListComboBox(cbo, "Filiale")
  End Sub
  ' Berater
  Sub ListVakBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    ListBerater(cbo)
  End Sub
  ' Im Web veröffentlicht
  Sub ListVakImWeb(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    cbo.Properties.Items.Add(New ComboValue("", ""))
    cbo.Properties.Items.Add(New ComboValue("veröffentlicht", "1"))
    cbo.Properties.Items.Add(New ComboValue("nicht veröffentlicht", "0"))
  End Sub

	Sub ListVakAVAM(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Add(New ComboValue("", ""))
		cbo.Properties.Items.Add(New ComboValue("übermittelt", "1"))
		cbo.Properties.Items.Add(New ComboValue("nicht übermittelt", "0"))

	End Sub

	Sub ListVakAVAMState(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Add(New ComboValue("", ""))
		cbo.Properties.Items.Add(New ComboValue("INSPECTING", "0"))

		cbo.Properties.Items.Add(New ComboValue("PUBLISHED_RESTRICTED", "1"))
		cbo.Properties.Items.Add(New ComboValue("PUBLISHED_PUBLIC", "2"))

		cbo.Properties.Items.Add(New ComboValue("ARCHIVED", "3"))

		cbo.Properties.Items.Add(New ComboValue("CANCELLED", "10"))
		cbo.Properties.Items.Add(New ComboValue("REJECTED", "11"))

	End Sub



#End Region

#Region "Allgemeine Funktionen"

	Sub FillDataHeaderLv(ByVal Lv As ListView, ByRef strColumnList As String, ByRef strColumnInfo As String)
    Dim lstStuff As ListViewItem = New ListViewItem()
    Dim lvwColumn As ColumnHeader

    With Lv
      .Clear()

      ' Nr;Nummer;Name;Strasse;PLZ Ort
      If strColumnList.EndsWith(";") Then strColumnInfo = Mid(strColumnList, 1, strColumnList.Length - 1)
      If strColumnInfo.EndsWith(";") Then strColumnInfo = Mid(strColumnInfo, 1, strColumnInfo.Length - 1)

      Dim strCaption As String() = Regex.Split(strColumnList, ";")
      ' 0-1;0-1;2000-0;2000-0;2500-0
      Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")
      Dim strFieldWidth As String
      Dim strFieldAlign As String = "0"
      Dim strFieldData As String()

      For i = 0 To strCaption.Length - 1
        lvwColumn = New ColumnHeader()
        lvwColumn.Text = strCaption(i).ToString
        strFieldData = Regex.Split(strFieldInfo(i).ToString, "-")

        If strFieldInfo(i).ToString.StartsWith("-") Then
          strFieldWidth = strFieldData(1)
          lvwColumn.Width = CInt(strFieldWidth) * -1
          If strFieldData.Count > 1 Then
            strFieldAlign = CStr(IIf(strFieldData(0) = String.Empty, strFieldData(2), strFieldData(1)))
          End If
        Else
          strFieldWidth = Regex.Split(strFieldInfo(i).ToString, "-")(0)
          lvwColumn.Width = CInt(strFieldWidth)   '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
          If strFieldData.Count > 1 Then
            strFieldAlign = strFieldData(1)
          End If
          'CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
        End If
        If strFieldAlign = "1" Then
          lvwColumn.TextAlign = HorizontalAlignment.Right
        ElseIf strFieldAlign = "2" Then
          lvwColumn.TextAlign = HorizontalAlignment.Center
        Else
          lvwColumn.TextAlign = HorizontalAlignment.Left

        End If

        lstStuff.BackColor = Color.Yellow

        .Columns.Add(lvwColumn)
      Next

      lvwColumn = Nothing
    End With

  End Sub

  ' Aktiviert - Nicht aktiviert
  Sub ListCboAktiviertNichtAktiviert(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    cbo.Properties.Items.Clear()
    cbo.Properties.Items.Add(New ComboValue("", ""))
    cbo.Properties.Items.Add(New ComboValue(m_xml.GetSafeTranslationValue("Nicht aktiviert"), "0"))
    cbo.Properties.Items.Add(New ComboValue(m_xml.GetSafeTranslationValue("Aktiviert"), "1"))
  End Sub
  ' Vollständig - Nicht vollständig
  Sub ListCboVollstaendigNichtVoll(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    cbo.Properties.Items.Clear()
    cbo.Properties.Items.Add(New ComboValue("", ""))
    cbo.Properties.Items.Add(New ComboValue(m_xml.GetSafeTranslationValue("Nicht vollständig"), "0"))
    cbo.Properties.Items.Add(New ComboValue(m_xml.GetSafeTranslationValue("Vollständig"), "1"))
  End Sub

  ' Ja - Nein
  Sub ListJaNein(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    cbo.Properties.Items.Clear()
    cbo.Properties.Items.Add(New ComboValue("", ""))
    cbo.Properties.Items.Add(New ComboValue(m_xml.GetSafeTranslationValue("Nein"), "0"))
    cbo.Properties.Items.Add(New ComboValue(m_xml.GetSafeTranslationValue("Ja"), "1"))
  End Sub

  ''' <summary>
  ''' Füllt die ComboBox nur für die Tabelle Vakanzen. Der Feldname muss als Parameter angegeben werden.
  ''' </summary>
  ''' <param name="cbo"></param>
  ''' <param name="Feldname">Entspricht der Spalte in der Tabelle Vakanzen</param>
  ''' <remarks></remarks>
  Sub ListComboBox(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal Feldname As String)
    Dim strEntry As String
    Dim strSqlQuery As String
    strSqlQuery = String.Format("Select {0} From Vakanzen Where {0} <> '' And {0} Is Not Null Group By {0} Order By {0} ", _
                                              Feldname)

    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rVAKrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("")
      While rVAKrec.Read
        strEntry = rVAKrec(Feldname).ToString
        cbo.Properties.Items.Add(New ComboValue(strEntry, strEntry))
        iWidth = CInt(IIf(iWidth > CInt(Len(strEntry).ToString), iWidth, CInt(Len(strEntry).ToString)))

        i += 1
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub LoadVakKontaktData(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strTextToShow As String
    Dim strTextToSave As String
    Dim strSqlQuery As String
    strSqlQuery = String.Format("Select RecValue, Bez_d As Bez From tbl_base_VakKontakt tk Where RecValue In (Select vakkontakt From Vakanzen v Where tk.RecValue = v.vakkontakt AND IsNumeric(V.vakkontakt) <> 0)")

    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rVAKrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("")
      While rVAKrec.Read
        strTextToShow = rVAKrec("Bez").ToString
        strTextToSave = rVAKrec("RecValue").ToString
        cbo.Properties.Items.Add(New ComboValue(strTextToShow, strTextToSave))

      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub LoadVakStateData(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strTextToShow As String
    Dim strTextToSave As String
    Dim strSqlQuery As String
    strSqlQuery = String.Format("Select RecValue, Bez_d As Bez From tbl_base_VakState ts Where RecValue In (Select VakState From Vakanzen v Where ts.RecValue = v.vakState  AND IsNumeric(V.vakState) <> 0)")

    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rVAKrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("")
      While rVAKrec.Read
        strTextToShow = rVAKrec("Bez").ToString
        strTextToSave = rVAKrec("RecValue").ToString
        cbo.Properties.Items.Add(New ComboValue(strTextToShow, strTextToSave))

      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub LoadAgeData(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strTextToShow As String
    Dim strTextToSave As String
    Dim strSqlQuery As String
    strSqlQuery = "Begin Try Drop Table #Vak End Try Begin Catch End Catch; SELECT " &
"	(CASE " &
"	WHEN v.MAAge LIKE '%1%' THEN 'Bis 24' " &
"	WHEN v.MAAge LIKE '%2%' THEN '25-34' " &
"	WHEN v.MAAge LIKE '%3%' THEN '35-44' " &
"	WHEN v.MAAge LIKE '%4%' THEN '45-65' " &
"	ELSE '0' " &
"	END ) AS Alter_Von, " &
"	(CASE " &
"	WHEN v.MAAge LIKE '%1%' THEN '1' " &
"	WHEN v.MAAge LIKE '%2%' THEN '2' " &
"	WHEN v.MAAge LIKE '%3%' THEN '3' " &
"	WHEN v.MAAge LIKE '%4%' THEN '4' " &
"	ELSE '0' " &
"	END ) AS AlterValue " &
"	Into #Vak FROM Vakanzen v " &
"	Where v.MAAge <> '' " &
"	Group By v.MAAge; " &
" Select * From #Vak Group By Alter_Von, AlterValue Order By AlterValue  "

    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rVAKrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("")
      While rVAKrec.Read
        strTextToShow = rVAKrec("Alter_Von").ToString
        strTextToSave = rVAKrec("AlterValue").ToString
        cbo.Properties.Items.Add(New ComboValue(strTextToShow.ToString, strTextToSave))
        
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub LoadAnstellungData(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strTextToShow As String
    Dim strTextToSave As String
    Dim strSqlQuery As String
    strSqlQuery = "SELECT " &
"	(CASE " &
"	WHEN v.Anstellung LIKE '%2%' THEN 'Freelancer' " &
"	WHEN v.Anstellung LIKE '%3%' THEN 'Praktikum' " &
"	WHEN v.Anstellung LIKE '%4%' THEN 'Nebenerwerb' " &
"	WHEN v.Anstellung LIKE '%5%' THEN 'Festanstellung' " &
"	WHEN v.Anstellung LIKE '%6%' THEN 'Lehrstelle' " &
"	WHEN v.Anstellung LIKE '%1%' THEN 'Temporär' " &
"	ELSE '' " &
"	END ) AS Anstellung, " &
"	(CASE  " &
"	WHEN v.Anstellung LIKE '%2%' THEN '2' " &
"	WHEN v.Anstellung LIKE '%3%' THEN '3' " &
"	WHEN v.Anstellung LIKE '%4%' THEN '4' " &
"	WHEN v.Anstellung LIKE '%5%' THEN '5' " &
"	WHEN v.Anstellung LIKE '%6%' THEN '6' " &
"	WHEN v.Anstellung LIKE '%1%' THEN '1' " &
"	ELSE '' " &
"	END ) AS AnstellungValue " &
"	FROM Vakanzen v  " &
"	Where v.Anstellung <> '' " &
"	Group By v.Anstellung "


    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rVAKrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("")
      While rVAKrec.Read
        strTextToShow = rVAKrec("Anstellung").ToString
        strTextToSave = rVAKrec("AnstellungValue").ToString
        cbo.Properties.Items.Add(New ComboValue(strTextToShow.ToString, strTextToSave))
        iWidth = CInt(IIf(iWidth > CInt(Len(strTextToShow).ToString), iWidth, CInt(Len(strTextToShow).ToString)))

        i += 1
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strSqlQuery As String = "[List Users 4 Vacancies]"
    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim rESrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rESrec.Read
        cbo.Properties.Items.Add(New ComboValue(String.Format("{0}, {1} ({2})", rESrec("Nachname"), rESrec("Vorname"), rESrec("KST")), _
                                                rESrec("KST").ToString))

      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

#End Region

End Module
