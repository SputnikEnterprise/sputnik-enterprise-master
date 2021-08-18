
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions


Module FuncLv

  Private m_xml As New ClsXML
	Private _ClsReg As New SPProgUtility.ClsDivReg
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

#Region "Dropdown-Funktionen für 1. Seite..."

  Sub ListSort(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)

    cbo.Properties.Items.Clear()
    cbo.Properties.Items.Add(String.Format("0 - {0}", m_xml.GetSafeTranslationValue("Kundennummer")))
    cbo.Properties.Items.Add(String.Format("1 - {0}", m_xml.GetSafeTranslationValue("Kundenname")))
    cbo.Properties.Items.Add(String.Format("2 - {0}", m_xml.GetSafeTranslationValue("Kundenplz + Kundenort")))
    cbo.Properties.Items.Add(String.Format("3 - {0}", m_xml.GetSafeTranslationValue("Ort")))
    cbo.Properties.Items.Add(String.Format("4 - {0}", m_xml.GetSafeTranslationValue("Kreditlimite")))
    cbo.Properties.Items.Add(String.Format("5 - {0}", m_xml.GetSafeTranslationValue("Telefon")))
    cbo.Properties.Items.Add(String.Format("6 - {0}", m_xml.GetSafeTranslationValue("Telefax")))
    cbo.Properties.Items.Add(String.Format("7 - {0}", m_xml.GetSafeTranslationValue("Nachname + Vorname")))

  End Sub

  Sub ListKDKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "Kanton"

		Dim strSqlQuery As String = "Select PLZ.Kanton From Kunden Left Join PLZ on Replace(Replace(Kunden.PLZ, ' ', ''), '-', '') = Convert(nvarchar(10), PLZ.Plz) AND PLZ.Land = 'CH' "
		strSqlQuery += "Where Kunden.Land = 'CH' "
    strSqlQuery += "And Kunden.ort <> '' And len(Kunden.PLZ) = 4 And PLZ.Kanton Is Not Null "
    strSqlQuery += "And Kunden.KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
    strSqlQuery += "Group By PLZ.Kanton Order By PLZ.Kanton"
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
      End While

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strSqlQuery As String = String.Empty
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      strSqlQuery = "[GetKDBetrater]"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure

      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(New ComboBoxItem(CStr(rKDrec("USName")), rKDrec("KST").ToString))
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try


  End Sub

  Sub ListFProperty(ByVal cbo As DevExpress.XtraEditors.GridLookUpEdit)
    Dim list As New List(Of MyColor)()
    Dim strSqlQuery As String = "[Get KD FProperty]"
    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      Dim iColorNr As Integer = 0
      Dim strColorname As String() = Regex.Split(ColorTranslator.FromWin32(iColorNr).ToString, " ")

      If rKDrec.HasRows Then
        While rKDrec.Read

          iColorNr = CInt(rKDrec("FProperty").ToString)
          If Not IsDBNull(rKDrec("FProperty")) Then
            If rKDrec("Bezeichnung").ToString.Trim = String.Empty Then
              strColorname = Regex.Split(If(iColorNr = 0, "White", ColorTranslator.FromOle(iColorNr).ToString), " ")
              strColorname(1) = Replace(strColorname(1), "[", "")
              strColorname(1) = Replace(strColorname(1), "]", "")
            Else
              strColorname(1) = rKDrec("Bezeichnung").ToString.Trim
            End If

            If iColorNr = 0 Then
              list.Add(New MyColor() With {.TestColor = ColorTranslator.FromOle(16777215), .DbValueString = "16777215", .ColorString = strColorname(1)})

            Else
              list.Add(New MyColor() With {.TestColor = ColorTranslator.FromOle(CInt(rKDrec("FProperty").ToString)), .DbValueString = rKDrec("FProperty").ToString, .ColorString = strColorname(1)})

            End If

          Else
            list.Add(New MyColor() With {.TestColor = Color.Black, .DbValueString = String.Empty, .ColorString = m_xml.GetSafeTranslationValue("Nicht definierte Farbe")})

          End If
        End While

      End If

      cbo.Properties.DataSource = list

      cbo.Properties.DisplayMember = "ColorString"
      cbo.Properties.ValueMember = "DbValueString"
      cbo.Properties.PopulateViewColumns()

      cbo.Properties.View.Columns(0).ColumnEdit = New DevExpress.XtraEditors.Repository.RepositoryItemColorEdit()
      cbo.Properties.View.Columns.Item(1).Visible = False

      cbo.Properties.View.Columns.Item(0).Caption = m_xml.GetSafeTranslationValue("Farbe")
      cbo.Properties.View.Columns.Item(2).Caption = m_xml.GetSafeTranslationValue("Bezeichnung")
      cbo.Properties.ShowFooter = False
      cbo.Properties.View.OptionsView.ShowIndicator = False

    Catch ex As Exception
      MsgBox(ex.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListFProperty(ByVal cbo As DevExpress.XtraEditors.ColorEdit)
    Dim strSqlQuery As String = "[Get KD FProperty]"
    'Dim strSqlQuery As String = "Select Kunden.FProperty, Tab_KDFProperty.Bezeichnung From Kunden "
    'strSqlQuery += "Left Join Tab_KDFProperty On Kunden.FProperty = Tab_KDFProperty.ColorCode "
    ''strSqlQuery += "Where Kunden.FProperty <> 0 "
    'strSqlQuery += "Group By Kunden.FProperty, Tab_KDFProperty.Bezeichnung "
    'strSqlQuery += "Order By Tab_KDFProperty.Bezeichnung"

    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      Dim iColorNr As Integer = 0
      Dim strColorname As String() = Regex.Split(ColorTranslator.FromWin32(iColorNr).ToString, " ")

      If rKDrec.HasRows Then
        While rKDrec.Read

          iColorNr = CInt(rKDrec("FProperty").ToString)
          If Not IsDBNull(rKDrec("FProperty")) Then
            If rKDrec("Bezeichnung").ToString.Trim = String.Empty Then
              strColorname = Regex.Split(If(iColorNr = 0, "White", ColorTranslator.FromWin32(iColorNr).ToString), " ")
              strColorname(1) = Replace(strColorname(1), "[", "")
              strColorname(1) = Replace(strColorname(1), "]", "")
            Else
              strColorname(1) = rKDrec("Bezeichnung").ToString.Trim
            End If

            If iColorNr = 0 Then
              'cbo.Properties.Items.Add("16777215" & vbTab & strColorname(1))
            Else
              'cbo.properties.Items.Add(rKDrec("FProperty").ToString & vbTab & strColorname(1))

            End If
            Debug.Print(ColorTranslator.FromWin32(CInt(rKDrec("FProperty"))).ToString)
          Else
            'cbo.properties.Items.Add("Nicht definierte Farbe")
          End If

          iWidth = CInt(IIf(iWidth > CInt(Len(rKDrec("FProperty").ToString & vbTab & strColorname(1))), _
                            iWidth, CInt(Len(rKDrec("FProperty").ToString & vbTab & strColorname(1)))))

          i += 1
        End While
        'cbo.DropDownWidth = CInt((iWidth * 7) + 20)

      End If

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

	Sub ListKDKontakt(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)
		Dim strSqlQuery As String = "Select HowKontakt From Kunden "
		strSqlQuery += "Where HowKontakt <> '' And HowKontakt Is Not Null "
		strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
		strSqlQuery += "Group By HowKontakt Order By HowKontakt"

		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			cbo.Properties.Items.Clear()
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader

			While rKDrec.Read
				cbo.Properties.Items.Add(rKDrec("HowKontakt").ToString)
			End While
			cbo.Properties.DropDownRows = 20

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListKDRes(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit, ByVal iResNr As Integer)
		Dim strFieldName As String = "KDRes" & iResNr.ToString
		Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
		strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
		strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
		strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			cbo.Properties.Items.Clear()

			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

			While rKDrec.Read
				cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
			End While
			cbo.Properties.DropDownRows = 20

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

  Sub ListKDStat(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal iStatNr As Integer)
    Dim strFieldName As String = "KDState" & iStatNr.ToString
    Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
    strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
    strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
    strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListKDZKond(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "ZahlKond"
    Dim strSqlQuery As String = "Select " & strFieldName & " From KD_RE_Address "
    strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
    'strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
    strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName

    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListKDBCount(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "MAAnzahl"
    Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
    strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
    strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
    strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName

    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListKDFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strSqlQuery As String = String.Empty
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      strSqlQuery = "[GetZGMAFiliale]"
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure

      Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

      cbo.Properties.Items.Clear()
      While rZGrec.Read
        cbo.Properties.Items.Add(rZGrec("Bezeichnung").ToString)
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

  Sub ListKDCurrency(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "Currency"
    Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
    strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
    strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
    strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListKDMahnCode(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "Mahncode"
    Dim strSqlQuery As String = "Select " & strFieldName & " From KD_RE_Address "
    strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
    strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListKDFakturaCode(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "Faktura"
    Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
    strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
    strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
    strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName

    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListKD1KLimite(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "Kreditlimite"
    Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
    strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
    strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
    strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(Format(Val(rKDrec(strFieldName).ToString), "0.00"))
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListKD2KLimite(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "Kreditlimite_2"
    Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
    strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
    strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
    strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text

      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(Format(Val(rKDrec(strFieldName).ToString), "0.00"))
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

	Sub ListKDVerguetung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "KD_UmsMin"
		Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
		strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
		strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rKDrec.Read
				cbo.Properties.Items.Add(Format(Val(rKDrec(strFieldName).ToString), "0.00"))
			End While
			cbo.Properties.DropDownRows = 20

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

  Sub ListKDWithOP(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strFieldName As String = "OPVersand"
    Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
    strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
    strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
    strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName

    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
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

#Region "DropDown-Funktionen für 3. Seite..."

  Sub ListZBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strSqlQuery As String = String.Empty
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()
      strSqlQuery = "[GetKDZBetrater]"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(New ComboBoxItem(CStr(rKDrec("USName")), rKDrec("Berater").ToString))
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListZAbteilung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strSqlQuery As String = String.Empty
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()
      strSqlQuery = "[GetKDZAbteilung]"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec("DbValue").ToString)
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListZPosition(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strSqlQuery As String = String.Empty
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()
      strSqlQuery = "[GetKDZPosition]"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec("DbValue").ToString)
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListZKontakt(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strSqlQuery As String = String.Empty
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      strSqlQuery = "[GetKDZKontakt]"
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec("DbValue").ToString)
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListZStat(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal iIndex As Integer)
    Dim strSqlQuery As String = String.Empty
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      strSqlQuery = "[GetKDZ" & iIndex.ToString & "Stat]"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec("DbValue").ToString)
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListZGebMonth(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strSqlQuery As String = String.Empty
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      strSqlQuery = "[GetKDZGebMonth]"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec("DbValue").ToString)
      End While
      cbo.Properties.DropDownRows = 20

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

  End Sub

  Sub ListZKontaktFrom(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strSqlQuery As String = String.Empty
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()

      strSqlQuery = "[GetKDZKontaktFrom]"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim rKDrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      While rKDrec.Read
        cbo.Properties.Items.Add(rKDrec("Berater").ToString)
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

#Region "Sonstige Funktions..."

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
          lvwColumn.Width = CInt(strFieldWidth) '* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
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


    'Dim lstStuff As ListViewItem = New ListViewItem()
    'Dim lvwColumn As ColumnHeader

    'With Lv
    '  .Clear()

    '  ' Nr;Nummer;Name;Strasse;PLZ Ort
    '  Dim strCaption As String() = Regex.Split(strColumnList, ";")
    '  ' 0-1;0-1;2000-0;2000-0;2500-0
    '  Dim strFieldInfo As String() = Regex.Split(strColumnInfo, ";")

    '  For i = 0 To strCaption.Length - 1
    '    lvwColumn = New ColumnHeader()
    '    lvwColumn.Text = strCaption(i).ToString

    '    lvwColumn.Width = CInt(Mid(strFieldInfo(i).ToString, 1, 1)) * Screen.PrimaryScreen.BitsPerPixel
    '    If CInt(Right(strFieldInfo(i).ToString, 1)) = 1 Then
    '      lvwColumn.TextAlign = HorizontalAlignment.Right
    '    ElseIf CInt(Right(strFieldInfo(i).ToString, 1)) = 2 Then
    '      lvwColumn.TextAlign = HorizontalAlignment.Center
    '    Else
    '      lvwColumn.TextAlign = HorizontalAlignment.Left
    '    End If

    '    .Columns.Add(lvwColumn)
    '  Next

    '  lvwColumn = Nothing
    'End With

  End Sub

  Sub FillFoundedKDData(ByVal Lv As ListView, ByVal strQuery As String)
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
    Dim i As Integer = 0

    Try
      Conn.Open()
      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)

      Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank
      Lv.Items.Clear()
      Lv.FullRowSelect = True

      Dim Time_1 As Double = System.Environment.TickCount

      Lv.BeginUpdate()
      While rKDrec.Read
        With Lv
          .Items.Add(rKDrec("KDNr").ToString)
          .Items(i).SubItems.Add(rKDrec("Firma1").ToString)
          .Items(i).SubItems.Add(rKDrec("KDLand").ToString & "-" & rKDrec("KDPLZ").ToString & " " & rKDrec("KDOrt").ToString)
          .Items(i).SubItems.Add(rKDrec("KDTelefon").ToString)
          .Items(i).SubItems.Add(rKDrec("KDTelefax").ToString)

          If Not IsDBNull(rKDrec("KDKreditLimite")) Then
            .Items(i).SubItems.Add(Format(rKDrec("KDKreditLimite"), "###,###,###,###,0.00"))
          Else
            .Items(i).SubItems.Add("")
          End If
          If Not IsDBNull(rKDrec("KDKreditLimiteAb")) Then
            .Items(i).SubItems.Add(Format(rKDrec("KDKreditLimiteAb"), "dd.MM.yyyy"))
          Else
            .Items(i).SubItems.Add("")
          End If
          If Not IsDBNull(rKDrec("KDKreditLimiteBis")) Then
            .Items(i).SubItems.Add(Format(rKDrec("KDKreditLimiteBis"), "dd.MM.yyyy"))
          Else
            .Items(i).SubItems.Add("")
          End If
          .Items(i).SubItems.Add(rKDrec("ZHDRecNr").ToString) ' 8
          .Items(i).SubItems.Add(rKDrec("Anrede").ToString & " " & rKDrec("Nachname").ToString & " " & rKDrec("Vorname").ToString)
          If Not IsDBNull(rKDrec("ZHDTelefon")) Then          ' 10
            .Items(i).SubItems.Add(rKDrec("ZHDTelefon").ToString)
          Else
            .Items(i).SubItems.Add("")
          End If
          If Not IsDBNull(rKDrec("ZHDNatel")) Then            ' 11
            .Items(i).SubItems.Add(rKDrec("ZHDNatel").ToString)
          Else
            .Items(i).SubItems.Add("")
          End If
          If Not IsDBNull(rKDrec("ZHDTelefax")) Then          ' 12
            .Items(i).SubItems.Add(rKDrec("ZHDTelefax").ToString)
          Else
            .Items(i).SubItems.Add("")
          End If
          If Not IsDBNull(rKDrec("ZHDeMail")) Then            ' 13
            .Items(i).SubItems.Add(rKDrec("ZHDeMail").ToString)
          Else
            .Items(i).SubItems.Add("")
          End If
          If Not IsDBNull(rKDrec("ZHDInteressen")) Then     ' 15
            .Items(i).SubItems.Add(rKDrec("ZHDInteressen").ToString)
          Else
            .Items(i).SubItems.Add("")
          End If
          If Not IsDBNull(rKDrec("ZHDBemerkung")) Then      ' 16
            .Items(i).SubItems.Add(rKDrec("ZHDBemerkung").ToString)
          Else
            .Items(i).SubItems.Add("")
          End If
          .Items(i).SubItems.Add("")    ' 17
          .Items(i).SubItems.Add("[]")  ' 18

        End With

        i += 1
        Lv.EndUpdate()

      End While

      Dim Time_2 As Double = System.Environment.TickCount
      Console.WriteLine("Zeit für ListMailToFields: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


    Catch e As Exception
      Lv.Items.Clear()
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try

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

      Dim rPLZrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

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

  Sub ListZKontaktTyp(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()
      Dim cmdText As String = "SELECT KontaktType1 FROM KD_KontaktTotal "
      cmdText += "WHERE KontaktType1 > '' "
      cmdText += "GROUP BY KontaktType1 Order By KontaktType1"
      Dim cmd As SqlCommand = New SqlCommand(cmdText, Conn)
      Dim reader As SqlDataReader = cmd.ExecuteReader()

      cbo.Properties.Items.Clear()
      While reader.Read()
        cbo.Properties.Items.Add(reader("KontaktType1"))
      End While


    Catch ex As Exception
      '_ex.MessageBoxShowError(_ClsProgSetting.GetLogedUSNr, "ListZKontaktTyp", ex)
    End Try

  End Sub

  Sub ListForActivate(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    cbo.Properties.Items.Clear()
    Try

      cbo.Properties.Items.Add("Aktiviert")
      cbo.Properties.Items.Add("Nicht Aktiviert")

    Catch e As Exception
      MsgBox(e.Message)

    Finally

    End Try

  End Sub

  Sub ListKontaktArten(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

    cbo.Properties.Items.Clear()
    Try
      cbo.Properties.Items.Add(New ComboBoxItem("Telefon/Fax/Mobile (KD-ZHD)", "0"))
      cbo.Properties.Items.Add(New ComboBoxItem("E-Mail-Adresse", "1"))
      cbo.Properties.Items.Add(New ComboBoxItem("Homepage", "2"))

      cbo.SelectedIndex = 0

    Catch e As Exception
      MsgBox(e.Message)

    Finally

    End Try

  End Sub

  Sub ListHideKontaktArt(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

    Try
      Conn.Open()
      Dim cmdText As String = "SELECT KontaktType1 FROM KD_KontaktTotal "
      cmdText += "WHERE KontaktType1 > '' "
      cmdText += "GROUP BY KontaktType1 Order By KontaktType1"
      Dim cmd As SqlCommand = New SqlCommand(cmdText, Conn)
      Dim reader As SqlDataReader = cmd.ExecuteReader()

      cbo.Properties.Items.Clear()
      While reader.Read()
        cbo.Properties.Items.Add(reader("KontaktType1"))
      End While


    Catch ex As Exception
      '_ex.MessageBoxShowError(_ClsProgSetting.GetLogedUSNr, "ListZKontaktTyp", ex)
    End Try

  End Sub


#End Region


  Function GetUSFormControlDataPath() As String
    Return _ClsProgSetting.GetSkinPath() & ClsDataDetail.GetAppGuidValue() & "\"
  End Function

  Function GetUSFormControlData_XMLFile() As String
    Dim bResult As Boolean
    Dim strUserProfileName As String = _ClsProgSetting.GetUserProfileFile()
    Dim strQuery As String = "//User_" & _ClsProgSetting.GetLogedUSNr & "/FormControls/FormName[@ID=" & Chr(34)
    strQuery &= ClsDataDetail.GetAppGuidValue() & Chr(34) & "]/TemplateFile"


    Dim strBez As String = _ClsReg.GetXMLNodeValue(GetUSFormControlDataPath(), strQuery)
    If strBez <> String.Empty Then
      If strBez = CStr(1) Then bResult = True
    End If
    If Not System.IO.File.Exists(GetUSFormControlDataPath() & strBez) Then strBez = String.Empty

    Return strBez
  End Function

  Function TranslateMyText(ByVal strBez As String) As String
    Dim strMethodeName As String = String.Format("{0}.{1}", New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().ReflectedType.FullName, _
                                            New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name)
    Dim strOrgText As String = strBez
    Dim strTranslatedText As String = m_xml.GetSafeTranslationValue(strBez)

    Return strTranslatedText
  End Function


  Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

    Dim m_md As New SPProgUtility.Mandanten.Mandant
    Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
    Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
    Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

    Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
    Dim translate = clsTransalation.GetTranslationInObject

    Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

  End Function

End Module

Public Class MyColor
  ' Fields...
  Private _ColorString As String
  Private _DbValueString As String
  Private _TestColor As Color

  Public Property TestColor() As Color
    Get
      Return _TestColor
    End Get
    Set(ByVal value As Color)
      _TestColor = value
    End Set
  End Property

  Public Property DbValueString() As String
    Get
      Return _DbValueString
    End Get
    Set(ByVal value As String)
      _DbValueString = value
    End Set
  End Property

  Public Property ColorString() As String
    Get
      Return _ColorString
    End Get
    Set(ByVal value As String)
      _ColorString = value
    End Set
  End Property


End Class
