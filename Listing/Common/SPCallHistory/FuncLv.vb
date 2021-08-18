
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Reflection


Module FuncLv


  Private _ClsFunc As New ClsDivFunc
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

  Sub ListSort(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

    cbo.Properties.Items.Clear()
    cbo.Properties.Items.Add(String.Format("0 - {0}", m_xml.GetSafeTranslationValue("BeraterIn")))
    cbo.Properties.Items.Add(String.Format("1 - {0}", m_xml.GetSafeTranslationValue("Datum")))

  End Sub

  Sub ListCustomerContactSort(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

    cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(String.Format("0 - {0}", m_xml.GetSafeTranslationValue("Firmenname + Datum aufsteigend")))
		cbo.Properties.Items.Add(String.Format("1 - {0}", m_xml.GetSafeTranslationValue("Firmenname + Datum absteigend")))

		cbo.Properties.Items.Add(String.Format("2 - {0}", m_xml.GetSafeTranslationValue("Kontaktdatum aufsteigend")))
		cbo.Properties.Items.Add(String.Format("3 - {0}", m_xml.GetSafeTranslationValue("Kontaktdatum absteigend")))

  End Sub

  Sub ListEmployeeContactSort(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

    cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(String.Format("0 - {0}", m_xml.GetSafeTranslationValue("Kandidatenname + Datum aufsteigend")))
		cbo.Properties.Items.Add(String.Format("1 - {0}", m_xml.GetSafeTranslationValue("Kandidatenname + Datum absteigend")))

		cbo.Properties.Items.Add(String.Format("2 - {0}", m_xml.GetSafeTranslationValue("Kontaktdatum aufsteigend")))
		cbo.Properties.Items.Add(String.Format("3 - {0}", m_xml.GetSafeTranslationValue("Kontaktdatum absteigend")))

  End Sub

  Sub ListArt(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

    cbo.Properties.Items.Clear()
    cbo.Properties.Items.Add(String.Format("0 - {0}", m_xml.GetSafeTranslationValue("Alle Anrufe")))
    cbo.Properties.Items.Add(String.Format("1 - {0}", m_xml.GetSafeTranslationValue("Nur Telefonate mit gefundenen Kunden")))
    cbo.Properties.Items.Add(String.Format("2 - {0}", m_xml.GetSafeTranslationValue("Nur Telefonate mit gefundenen Kandidaten")))

  End Sub

  Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strEntry As String
    Dim strSqlQuery As String = "[Show BeraterData For Search In CallHistory]"

    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      Dim rESrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      While rESrec.Read
        strEntry = String.Format("{0}", rESrec("USName").ToString)
        cbo.Properties.Items.Add(New ComboValue(strEntry, rESrec("USName").ToString))
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

	Sub ListEmployeeCreatedFrom(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As String
		Dim strSqlQuery As String = "[List Contact CreatedFrom For Employee]"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rESrec.Read
				strEntry = String.Format("{0}", rESrec("CreatedFrom").ToString)
				cbo.Properties.Items.Add(New ComboValue(strEntry, rESrec("CreatedFrom").ToString))
			End While
			cbo.Properties.DropDownRows = 20

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListCustomerCreatedFrom(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As String
		Dim strSqlQuery As String = "[List Contact CreatedFrom For Customer]"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rESrec.Read
				strEntry = String.Format("{0}", rESrec("CreatedFrom").ToString)
				cbo.Properties.Items.Add(New ComboValue(strEntry, rESrec("CreatedFrom").ToString))
			End While
			cbo.Properties.DropDownRows = 20

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListKontaktArt(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As String
		Dim strSqlQuery As String = "[List MAK Type1]"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			Dim rESrec As SqlDataReader = cmd.ExecuteReader									 '

			cbo.Properties.Items.Clear()
			While rESrec.Read
				strEntry = String.Format("{0}", rESrec("KontaktType1").ToString)
				cbo.Properties.Items.Add(New ComboValue(strEntry, rESrec("KontaktType1").ToString))
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

#Region "Dropdown-Funktionen für 2. Seite..."

  Sub ListMABetreuer(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strEntry As String
    Dim strSqlQuery As String = "[Get KST3Data From Benutzer]"
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure

      Dim rMArec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      While rMArec.Read
        strEntry = String.Format("{0}, {1}", rMArec("Nachname"), rMArec("Vorname"))
        cbo.Properties.Items.Add(New ComboValue(strEntry, rMArec("KST").ToString))
      End While
      cbo.Properties.DropDownRows = 20


    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub


  Sub ListESUnterzeichner(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strEntry As String
    Dim strSqlQuery As String = "Select ESUnterzeichner From ES"
    strSqlQuery += " Group By ESUnterzeichner"
    strSqlQuery += " Order By ESUnterzeichner"
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rMArec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      While rMArec.Read
        strEntry = rMArec("ESUnterzeichner").ToString
        cbo.Properties.Items.Add(New ComboValue(strEntry, rMArec("ESUnterzeichner").ToString))
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub ListMAKorrSprachen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "Sprache"
    Dim strSqlQuery As String = "[Get MASprachen]"
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure

      Dim rMArec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rMArec.Read
        cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
      End While


    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub


#End Region

#Region "DropDown-Funktionen für Kundendaten"

  Sub ListKDKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "Kanton"

    Dim strSqlQuery As String = "Select PLZ.Kanton From Kunden"
    strSqlQuery += " Left Join PLZ On"
    strSqlQuery += " IsNumeric(Kunden.PLZ) = 1 And"
		strSqlQuery += " Convert(varchar(10), PLZ.PLZ) = Replace(Replace(Kunden.PLZ, ' ', ''), '-', '') AND PLZ.Land = 'CH'"
		strSqlQuery += " Where Kunden.PLZ <> '' And PLZ.Kanton Is Not Null"
    strSqlQuery += " Group by PLZ.Kanton"
    strSqlQuery += " Order By PLZ.Kanton"
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rMArec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rMArec.Read
        cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
      End While

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub ListKDSprachen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "Sprache"
    Dim strSqlQuery As String = "Select Kunden.Sprache From Kunden"
    strSqlQuery += " Inner Join ES On"
    strSqlQuery += " ES.KDNr = Kunden.KDNr"
    strSqlQuery += " Where Sprache <> ''"
    strSqlQuery += " Group by Sprache"
    strSqlQuery += " Order By Sprache"

    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rMArec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rMArec.Read
        cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
      End While

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub ListMAArbeitspensum(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "Arbeitspensum"

    Dim strSqlQuery As String = "Select Arbeitspensum From MAKontakt_Komm"
    strSqlQuery += " Where Not (Arbeitspensum is Null Or Arbeitspensum = '') "
    strSqlQuery += " Group By Arbeitspensum Order By Arbeitspensum"
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rMArec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("")
      While rMArec.Read
        cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
      End While

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
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


  Function GetKantonPLZ(ByVal strKanton As String) As String
    Dim strPLZResult As String = ","
    Dim strFieldName As String = "PLZ"

    Dim strSqlQuery As String = "Select PLZ.PLZ, PLZ.Kanton From PLZ "
    strSqlQuery += "Where PLZ.Kanton In ('" & strKanton & "') "
    strSqlQuery += "Group By PLZ.PLZ, PLZ.Kanton Order By PLZ.PLZ, PLZ.Kanton"

    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rPLZrec As SqlDataReader = cmd.ExecuteReader

      While rPLZrec.Read
        strPLZResult += rPLZrec(strFieldName).ToString & ","

      End While
      If strPLZResult.Length > 1 Then
        strPLZResult = Mid(strPLZResult, 2, Len(strPLZResult) - 2)
        strPLZResult = Replace(strPLZResult, ",", "','")
      Else
        strPLZResult = String.Empty
      End If

    Catch e As Exception
      strPLZResult = String.Empty
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

    Return strPLZResult
  End Function

#End Region




End Module
