
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.UI

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

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPESSearch.ClsDataDetail


Module FuncLv
  Dim _ClsFunc As New ClsDivFunc
  Dim _ClsReg As New SPProgUtility.ClsDivReg

	'Private m_xml As New ClsXML
  Private m_UtilityUI As New UtilityUI
  Private m_Logger As ILogger = New Logger()


  Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

    Dim m_md As New SPProgUtility.Mandanten.Mandant
    Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
    Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
    Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

    Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
    Dim translate = clsTransalation.GetTranslationInObject

    Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

  End Function



#Region "Datenbankabfragen für SearchRec..."

  Function GetESDbData4ESSearch() As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
    Dim strQuery As String = "[List ESData For Search ESSearchList]"
    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.StoredProcedure

    Dim objAdapter As New SqlDataAdapter

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "ESData")

    Return ds.Tables(0)
  End Function

  Function GetESMADbData4ESSearch() As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
    Dim strQuery As String = "[List ESMAData For Search ESSearchList]"
    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.StoredProcedure

    Dim objAdapter As New SqlDataAdapter

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "ESData")

    Return ds.Tables(0)
  End Function

  Function GetESKDDbData4ESSearch() As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
    Dim strQuery As String = "[List ESKDData For Search ESSearchList]"
    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.StoredProcedure

    Dim objAdapter As New SqlDataAdapter

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "ESData")

    Return ds.Tables(0)
  End Function

  Function GetESAlsDbData4ESSearch() As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
    Dim strQuery As String = "[List ESAlsData For Search ESSearchList]"
    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.StoredProcedure

    Dim objAdapter As New SqlDataAdapter

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "ESData")

    Return ds.Tables(0)
  End Function

  Function GetESGAVBerufDbData4ESSearch() As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
    Dim strQuery As String = "[List ESGAVData For Search ESSearchList]"
    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.StoredProcedure

    Dim objAdapter As New SqlDataAdapter

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "ESData")

    Return ds.Tables(0)
  End Function

  Function GetESBrancheDbData4ESSearch() As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
    Dim strQuery As String = "[List ESBrancheData For Search ESSearchList]"
    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.StoredProcedure

    Dim objAdapter As New SqlDataAdapter

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "ESData")

    Return ds.Tables(0)
  End Function

  Function GetKDKredit_1DbData4ESSearch() As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
    Dim strQuery As String = "[List KDKreditlimite_1Data For Search ESSearchList]"
    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.StoredProcedure

    Dim objAdapter As New SqlDataAdapter

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "ESData")

    Return ds.Tables(0)
  End Function

  Function GetKDKredit_2DbData4ESSearch() As DataTable
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
    Dim strQuery As String = "[List KDKreditlimite_2Data For Search ESSearchList]"
    Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
    cmd.CommandType = CommandType.StoredProcedure

    Dim objAdapter As New SqlDataAdapter

    objAdapter.SelectCommand = cmd
    objAdapter.Fill(ds, "ESData")

    Return ds.Tables(0)
  End Function

#End Region


#Region "Dropdown-Funktionen für Allgemeine"


  Sub ListSort(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)

    '    cbo.Properties.Items.Clear()
    cbo.Properties.Items.Add(String.Format("0 - {0}", TranslateText("Einsatznummer")))
    cbo.Properties.Items.Add(String.Format("1 - {0}", TranslateText("Kandidatennummer")))
    cbo.Properties.Items.Add(String.Format("2 - {0}", TranslateText("Kundennummer")))
    cbo.Properties.Items.Add(String.Format("3 - {0}", TranslateText("Einsatzbeginn und Ende")))
    cbo.Properties.Items.Add(String.Format("4 - {0}", TranslateText("Kandidatenname")))
    cbo.Properties.Items.Add(String.Format("5 - {0}", TranslateText("Firmenname")))

  End Sub

  Sub ListKontaktArten(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

    '    cbo.Properties.Items.Clear()
    If cbo.Properties.Items.Count > 0 Then Exit Sub
    Try
      cbo.Properties.Items.Add(New ComboValue(TranslateText("Alle Nummern (Telefone und Mobile)"), "0"))
      cbo.Properties.Items.Add(New ComboValue(TranslateText("E-Mail-Adresse"), "1"))
      cbo.Properties.Items.Add(New ComboValue(TranslateText("Homepage"), "2"))

      cbo.SelectedIndex = 0

    Catch e As Exception
      MsgBox(e.Message)

    Finally

    End Try

  End Sub

  Sub ListESKST1(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strEntry As String
    Dim strSqlQuery As String = "[Show KST1Data For Search In ESSearch]"

    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim parm As New SqlParameter("@KSTBez", SqlDbType.VarChar, 30)
      parm.Value = " "
      cmd.Parameters.Add(parm)


      Dim rESrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("")
      While rESrec.Read
        strEntry = rESrec("KSTBezeichnung").ToString
        cbo.Properties.Items.Add(New ComboValue(strEntry, rESrec("KSTName").ToString))

      End While
      cbo.Properties.DropDownRows = 15

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  ' Kostenstelle 2 ---------------------------------------------------------------------------------------------
  Sub ListESKST2(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strEntry As String
    Dim strSqlQuery As String = "[Show KST2Data For Search In ESSearch]"

    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim parm As New SqlParameter("@KSTBez", SqlDbType.VarChar, 30)
      parm.Value = " "
      cmd.Parameters.Add(parm)


      Dim rESrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("")
      While rESrec.Read
        strEntry = rESrec("KSTBezeichnung").ToString
        cbo.Properties.Items.Add(New ComboValue(strEntry, rESrec("KSTName").ToString))

      End While
      cbo.Properties.DropDownRows = 15

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub ListBerater(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, Optional ByVal filiale As String = "")
    Dim strSqlQuery As String = "[List Benutzer]"
    Dim i As Integer = 0
    Dim iWidth As Integer = 0
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      cmd.Parameters.AddWithValue("@filiale", filiale)
      Dim rESrec As SqlDataReader = cmd.ExecuteReader

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("")
      While rESrec.Read
        cbo.Properties.Items.Add(New ComboValue(String.Format("{0}, {1}", rESrec("Nachname").ToString, rESrec("Vorname")), _
                                                rESrec("KST").ToString))

      End While
      cbo.Properties.DropDownRows = 15

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  Sub ListESWaehrung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strEntry As String
    Dim strSqlQuery As String = "[GetESCurrency]"

    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure

      Dim rESrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("")
      While rESrec.Read
        strEntry = rESrec("Currency").ToString
        cbo.Properties.Items.Add(strEntry) ' New ComboBoxItem(strEntry, rESrec("Currency").ToString))

      End While

    Catch e As Exception
      MsgBox(e.Message)

    Finally
      Conn.Close()
      Conn.Dispose()

    End Try
  End Sub

  ' Suva
  Sub ListSuva(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
    Dim strSqlQuery As String = "[GetESSuva]"
    Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

    Try
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure

      Dim rESrec As SqlDataReader = cmd.ExecuteReader                  '

      cbo.Properties.Items.Clear()
      cbo.Properties.Items.Add("")
      While rESrec.Read
        'strEntry = String.Format("{0} {1}", rESrec("Code").ToString, rESrec("Description").ToString)
				cbo.Properties.Items.Add(New ComboValue(m_translate.GetSafeTranslationValue(rESrec("Description").ToString), rESrec("Code").ToString))
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListUSFilialen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, Optional ByVal kst As String = "")
		Dim strEntry As String
		Dim strSqlQuery As String = "Select USFiliale From Benutzer "
		strSqlQuery += "Where USFiliale <> '' And USFiliale Is Not Null "
		If kst.Length > 0 Then
			strSqlQuery += String.Format(" And Nachname + ', ' + Vorname = '{0}' ", kst)
		End If
		strSqlQuery += "Group By USFiliale "
		strSqlQuery += "Order By USFiliale "

		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rESrec As SqlDataReader = cmd.ExecuteReader									 '

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("")
			While rESrec.Read
				strEntry = rESrec("USFiliale").ToString
				cbo.Properties.Items.Add(strEntry) ' New ComboBoxItem(strEntry, rESrec("USFiliale").ToString))

			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' Ansässigkeit alt -----------------------------------------------------------------------------------------------
	Sub ListMAAnsaessigkeit(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Ansaessigkeit"

		Dim strSqlQuery As String = "Select Mitarbeiter.Ansaessigkeit From Mitarbeiter "
		strSqlQuery += "Where Not (Mitarbeiter.Ort = '' Or Mitarbeiter.Ort is Null) "
		strSqlQuery += "Group By Mitarbeiter.Ansaessigkeit Order By Mitarbeiter.Ansaessigkeit"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader									 ' 

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

	' Kanton alt -----------------------------------------------------------------------------------------------------
	Sub ListMAKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Kanton"

		Dim strSqlQuery As String = "Select Mitarbeiter.MA_Kanton as Kanton From Mitarbeiter "
		strSqlQuery += "Where Not (Mitarbeiter.MA_Kanton = '' Or Mitarbeiter.MA_Kanton is Null)"
		strSqlQuery += "Group By Mitarbeiter.MA_Kanton Order By Mitarbeiter.MA_Kanton"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader									 ' PLZ-Datenbank

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

#Region "Dropdown-Funktionen für Sonstiges"
	' Einstufung
	Sub ListEinstufung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As String
		Dim strSqlQuery As String = "Select ES.Einstufung "
		strSqlQuery += "From ES "
		strSqlQuery += "Where ES.Einstufung <> '' And ES.Einstufung Is Not Null "
		strSqlQuery += "Group By ES.Einstufung "
		strSqlQuery += "Order By ES.Einstufung "

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rESrec As SqlDataReader = cmd.ExecuteReader									 '

			cbo.properties.items.Clear()
			cbo.Properties.Items.Add("")
			While rESrec.Read
				strEntry = rESrec("Einstufung").ToString
				cbo.Properties.Items.Add(strEntry) ' New ComboBoxItem(strEntry, rESrec("Einstufung").ToString))
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub
	' Gruppe 1
	Sub ListGruppe1(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As String
		Dim strSqlQuery As String = "[GetGAVGruppe1]"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rESrec As SqlDataReader = cmd.ExecuteReader									 '

			cbo.properties.Items.Clear()
			cbo.Properties.Items.Add("")
			While rESrec.Read
				strEntry = rESrec("Bezeichnung").ToString
				cbo.Properties.Items.Add(strEntry) ' New ComboBoxItem(strEntry, rESrec("Bezeichnung").ToString))
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListGAVKanton(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)
		Dim strEntry As String
		Dim strSqlQuery As String = "[List GAVKanton For Search In ESSearch]"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim parm As New SqlParameter("@GAVKanton", SqlDbType.VarChar, 5)
			parm.Value = " "
			cmd.Parameters.Add(parm)


			Dim rESrec As SqlDataReader = cmd.ExecuteReader									 '

			cbo.Properties.Items.Clear()
			'cbo.Properties.Items.Add("")
			While rESrec.Read
				strEntry = rESrec("GAVKanton").ToString
				If strEntry <> String.Empty Then cbo.Properties.Items.Add(New ComboValue(strEntry, TranslateText(rESrec("Bezeichnung").ToString)))
			End While
			cbo.Properties.DropDownRows = 26

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' Betreuer alt---------------------------------------------------------------------------------------------------
	Sub ListMABetreuer(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As String
		Dim strSqlQuery As String = "[Get KST3Data From Benutzer]"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rMArec As SqlDataReader = cmd.ExecuteReader									 '

			cbo.properties.items.Clear()

			While rMArec.Read
				strEntry = String.Format("{0}, {1}", rMArec("Nachname"), rMArec("Vorname"))
				cbo.Properties.Items.Add(strEntry) ' New ComboBoxItem(strEntry, rMArec("KST").ToString))
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	'Sub FillUnterschrieben(ByVal cbo As ComboBox)
	'    cbo.properties.items.Add(New ComboBoxItem("", ""))
	'    cbo.properties.items.Add(New ComboBoxItem("Nicht unterschrieben", "0"))
	'    cbo.properties.items.Add(New ComboBoxItem("Unterschrieben", "1"))
	'End Sub


	Sub ListESUnterzeichner(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As String
		Dim strSqlQuery As String = "Select ESUnterzeichner From ES"
		strSqlQuery += " Group By ESUnterzeichner"
		strSqlQuery += " Order By ESUnterzeichner"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader									 '

			cbo.properties.items.Clear()
			cbo.Properties.Items.Add("")
			While rMArec.Read
				strEntry = rMArec("ESUnterzeichner").ToString
				cbo.Properties.Items.Add(strEntry) ' New ComboBoxItem(strEntry, rMArec("ESUnterzeichner").ToString))
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' Geschäftsstellen alt
	Sub ListMAGeschäftsstellen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Bezeichnung"
		Dim strSqlQuery As String = "[GetMAFiliale]"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.properties.items.Clear()
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

	' Kontakt alt
	Sub ListMAKontakt(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Kontakt"

		Dim strSqlQuery As String = "Select KON.KontaktHow as Kontakt From "
		strSqlQuery += "MAKontakt_Komm KON "
		strSqlQuery += "Where Not (KON.KontaktHow is null Or KON.KontaktHow = '') "
		strSqlQuery += "Group By KON.KontaktHow Order By KON.KontaktHow"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.properties.items.Clear()
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

	' Fahrzeug alt
	Sub ListMAFahrzeug(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Fahrzeug"

		Dim strSqlQuery As String = "Select SON.Fahrzeug as Fahrzeug From "
		strSqlQuery += "MASonstiges SON "
		strSqlQuery += "Where Not (SON.Fahrzeug is null Or SON.Fahrzeug = '') "
		strSqlQuery += "Group By SON.Fahrzeug Order By SON.Fahrzeug"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.properties.items.Clear()
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

	' Führerausweis alt
	Sub ListMAFuehrerausweis(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Fuehrerschein"
		Dim strSqlQuery As String = "[Get MAFuehrerscheine]"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.properties.items.Clear()
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

#Region "Dropdown-Funktionen für Kandidaten"

	Sub ListMABewilligung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Bewilligung"

		Dim strSqlQuery As String = "Select tbl.Description As Bezeichnung, MA.Bewillig as Bewilligung From Mitarbeiter MA "
		strSqlQuery &= "Left Join tab_Quell tbl On MA.Bewillig = tbl.GetFeld "
		strSqlQuery += "Where Not (MA.Bewillig = '' Or MA.Bewillig Is Null) "
		strSqlQuery += "Group By tbl.Description, MA.Bewillig Order By MA.Bewillig"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("")
			While rMArec.Read
				cbo.Properties.Items.Add(rMArec("Bewilligung").ToString) ' New ComboValue(rMArec("Bezeichnung").ToString, rMArec("Bewilligung").ToString))
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListBewBisMonat(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Monat"
		Dim strSqlQuery As String = "Select Month(Bew_Bis) as Monat From Mitarbeiter "
		strSqlQuery += "Where Bew_Bis Is Not Null "
		strSqlQuery += "Group By Month(Bew_Bis) Order By Month(Bew_Bis)"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader									 ' 

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("")
			While rKDrec.Read
				cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
			End While
			cbo.Properties.DropDownRows = 12

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListMABewBisJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Jahr"
		Dim strSqlQuery As String = "Select Year(Bew_Bis) as Jahr From Mitarbeiter "
		strSqlQuery += "Where Bew_Bis Is Not Null "
		strSqlQuery += "And Year(Bew_Bis) >= Year(GetDate())"
		strSqlQuery += "Group By Year(Bew_Bis) Order By Year(Bew_Bis)"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader									 ' 

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("")
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

	' Quellensteuer ----------------------------------------------------------------------------------------------
	Sub ListMAQSteuer(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)
		Dim strFieldName As String = "QSteuer"
		Dim strSqlQuery As String = "Select tbl.Description As Bezeichnung, MA.Q_Steuer as QSteuer From Mitarbeiter MA "
		strSqlQuery &= "Left Join tab_Quell tbl On MA.Q_Steuer = tbl.GetFeld "
		strSqlQuery += "Where Not (MA.Q_Steuer = '' Or MA.Q_Steuer Is Null) "
		strSqlQuery += "Group By tbl.Description, MA.Q_Steuer Order By MA.Q_Steuer"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rMArec.Read
				cbo.Properties.Items.Add(rMArec("QSteuer").ToString) 'New ComboValue(rMArec("Bezeichnung").ToString, rMArec("QSteuer").ToString))
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMASKanton(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)
		Dim strFieldName As String = "Kanton"

		Dim strSqlQuery As String = "Select MA.S_Kanton as "
		strSqlQuery &= "Kanton From Mitarbeiter MA "
		strSqlQuery += "Where Not (MA.S_Kanton = '' Or MA.S_Kanton is Null)"
		strSqlQuery += "Group By MA.S_Kanton Order By MA.S_Kanton"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader									 ' PLZ-Datenbank

			cbo.Properties.Items.Clear()
			While rMArec.Read
				cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
			End While
			cbo.Properties.DropDownRows = 26

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMALand(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Land"
		Dim m_uttility As New SPProgUtility.MainUtilities.Utilities

		Dim strSqlQuery As String = "Select MA.Land,  ISNull( (Select Top 1 Land From LND Where Code = MA.Land), '') As LandName From Mitarbeiter MA "
		strSqlQuery += "Where Not (MA.Land = '' Or MA.Land is Null)"
		strSqlQuery += "Group By MA.Land Order By MA.Land"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader					 '

			cbo.Properties.Items.Clear()
			Dim valueText As String
			Dim valueName As String

			While rMArec.Read
				valueText = m_uttility.SafeGetString(rMArec, "LandName")
				valueName = m_uttility.SafeGetString(rMArec, strFieldName)

				cbo.Properties.Items.Add(New ComboValue(If(String.IsNullOrWhiteSpace(valueText), valueName, valueText), valueName))
			End While
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMAZivilstand(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Description"

		Dim strSqlQuery As String = "Select TAB_Zivilstand.GetFeld, TAB_Zivilstand.Description"
		strSqlQuery += " From TAB_Zivilstand Where TAB_Zivilstand.GetFeld In"
		strSqlQuery += " (Select Mitarbeiter.Zivilstand From Mitarbeiter "
		strSqlQuery += " Where Not (Mitarbeiter.Zivilstand = '' Or Mitarbeiter.Zivilstand is Null)"
		strSqlQuery += " Group By Mitarbeiter.Zivilstand) Order By TAB_Zivilstand.GetFeld"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader									 '

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("")
			While rMArec.Read
				cbo.Properties.Items.Add(New ComboValue(rMArec("Description").ToString, rMArec("GetFeld").ToString))
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMANationialitaet(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Nationality"
		Dim m_uttility As New SPProgUtility.MainUtilities.Utilities

		Dim strSqlQuery As String = "Select MA.Nationality,  ISNull( (Select Top 1 Land From LND Where Code = MA.Nationality), '') As LandName From Mitarbeiter MA "
		strSqlQuery += "Where Not (MA.Nationality = '' Or MA.Nationality Is Null) "
		strSqlQuery += "Group By MA.Nationality Order By MA.Nationality"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader					 '

			cbo.Properties.Items.Clear()
			Dim valueText As String
			Dim valueName As String


			While rMArec.Read
				valueText = m_uttility.SafeGetString(rMArec, "LandName")
				valueName = m_uttility.SafeGetString(rMArec, strFieldName)

				cbo.Properties.Items.Add(New ComboValue(If(String.IsNullOrWhiteSpace(valueText), valueName, valueText), valueName))
			End While
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMAKorrSprachen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Sprache"
		Dim strSqlQuery As String = "[Get MASprachen]"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

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

	Sub ListCboJaNeinK(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Add("") 'New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboValue(TranslateText("Nein mit Zahlungsart K"), "0"))
		cbo.Properties.Items.Add(New ComboValue(TranslateText("Ja mit Zahlungsart K"), "1"))
		'    SetComboBoxWidth(cbo)

	End Sub

	Sub ListMAKontakt(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)
		Dim strFieldName As String = "Kontakt"

		Dim strSqlQuery As String = "Select KON.KontaktHow as Kontakt From "
		strSqlQuery += "MAKontakt_Komm KON "
		strSqlQuery += "Where Not (KON.KontaktHow is null Or KON.KontaktHow = '') "
		strSqlQuery += "Group By KON.KontaktHow Order By KON.KontaktHow"
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
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMAStatus1(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)
		Dim strFieldName As String = "Status"

		Dim strSqlQuery As String = "Select KON.KStat1 as Status From "
		strSqlQuery += "MAKontakt_Komm KON "
		strSqlQuery += "Where Not (KON.KStat1 is null Or KON.KStat1 = '') "
		strSqlQuery += "Group By KON.KStat1 Order By KON.KStat1"
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
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMAStatus2(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)
		Dim strFieldName As String = "Status"

		Dim strSqlQuery As String = "Select KON.KStat2 as Status From "
		strSqlQuery += "MAKontakt_Komm KON "
		strSqlQuery += "Where Not (KON.KStat2 is null Or KON.KStat2 = '') "
		strSqlQuery += "Group By KON.KStat2 Order By KON.KStat2"
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
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListEmployeeALKKasse(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim m_utility As New SPProgUtility.MainUtilities.Utilities

		Dim Sql As String = "Select ALKNumber, ALKName, ALKLocation From MAKontakt_Komm Where ALKName <> '' AND ALKNumber IS NOT Null And MANr IN (SELECT MANr FROM ES Where ES.MDNr = @MDNr GROUP BY MANr) "
		Sql &= "Group By ALKNumber, ALKName, ALKLocation Order By ALKName, ALKLocation "
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			cbo.Properties.Items.Clear()
			'Conn.Open()

			'Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			'cmd.CommandType = Data.CommandType.Text

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("MDNr", ClsDataDetail.MDData.MDNr))
			Dim reader As SqlClient.SqlDataReader = m_utility.OpenReader(ClsDataDetail.MDData.MDDbConn, Sql, listOfParams, CommandType.Text)

			While reader.Read
				cbo.Properties.Items.Add(New ComboValue(String.Format("{0}, {1}", m_utility.SafeGetString(reader, "ALKName"), m_utility.SafeGetString(reader, "ALKLocation")),
																								m_utility.SafeGetString(reader, "ALKNumber")))
			End While
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

#End Region

#Region "DropDown-Funktionen für Kunden"

	Sub ListKDKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Kanton"

		Dim strSqlQuery As String = "Select PLZ.Kanton From Kunden"
		strSqlQuery += " Left Join PLZ On"
		strSqlQuery += " IsNumeric(Kunden.PLZ) = 1 And"
		strSqlQuery += " Convert(varchar(10), PLZ.PLZ) = Replace(Replace(Kunden.PLZ, ' ', ''), '-', '')"
		strSqlQuery += " Where Kunden.PLZ <> '' And PLZ.Kanton Is Not Null"
		strSqlQuery += " Group by PLZ.Kanton"
		strSqlQuery += " Order By PLZ.Kanton"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

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

		Catch ex As Exception
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' Sprache

	Sub ListKDSprachen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Sprache"

		Dim strSqlQuery As String = "Select Kunden.Sprache From Kunden"
		strSqlQuery += " Inner Join ES On"
		strSqlQuery += " ES.KDNr = Kunden.KDNr"
		strSqlQuery += " Where Sprache <> ''"
		strSqlQuery += " Group by Sprache"
		strSqlQuery += " Order By Sprache"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.properties.items.Clear()
			cbo.Properties.Items.Add("")
			While rMArec.Read
				cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
			End While

		Catch ex As Exception
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub


	' Arbeitspensum
	Sub ListMAArbeitspensum(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Arbeitspensum"

		Dim strSqlQuery As String = "Select Arbeitspensum From MAKontakt_Komm"
		strSqlQuery += " Where Not (Arbeitspensum is Null Or Arbeitspensum = '') "
		strSqlQuery += " Group By Arbeitspensum Order By Arbeitspensum"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.properties.items.Clear()
			cbo.properties.items.Add("")
			While rMArec.Read
				cbo.properties.items.Add(rMArec(strFieldName).ToString)
			End While

		Catch ex As Exception
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' Kündigungsfristen
	Sub ListMAKuendigungsfristen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "KundFristen"

		Dim strSqlQuery As String = "Select KundFristen From MAKontakt_Komm"
		strSqlQuery += " Where KundFristen is not Null and KundFristen <> ''"
		strSqlQuery += " Group By KundFristen Order By KundFristen"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.properties.items.Clear()
			cbo.properties.items.Add("")
			While rMArec.Read
				cbo.properties.items.Add(rMArec(strFieldName).ToString)
			End While

		Catch ex As Exception
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' Zahlungsarten
	Sub ListMAZahlungsarten(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Zahlungsarten"

		Dim strSqlQuery As String = "Select Zahlart as Zahlungsarten From Mitarbeiter"
		strSqlQuery += " Where Zahlart is not null and Zahlart <> ''"
		strSqlQuery += " Group By Zahlart Order By Zahlart"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.properties.items.Clear()
			cbo.Properties.Items.Add("")
			While rMArec.Read
				cbo.properties.items.Add(rMArec(strFieldName).ToString)
			End While

		Catch ex As Exception
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' Währung


	' AHV-Code
	Sub ListMAAHVCode(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Description"

		Dim strSqlQuery As String = "Select MA_LOSetting.AHVCode, TAB_AHV.Description From Mitarbeiter"
		strSqlQuery += " Inner Join MA_LOSetting On"
		strSqlQuery += " MA_LOSetting.MANr = Mitarbeiter.MANr"
		strSqlQuery += " Inner Join TAB_AHV On"
		strSqlQuery += " TAB_AHV.GetFeld = MA_LOSetting.AHVCode"
		strSqlQuery += " Group By MA_LOSetting.AHVCode, TAB_AHV.Description Order By MA_LOSetting.AHVCode"

		Dim i As Integer = 0
		Dim iWidth As Integer = 0
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.properties.items.Clear()
			cbo.Properties.Items.Add("")
			While rMArec.Read
				'cbo.properties.items.Add(rMArec(strFieldName).ToString.Trim)
				cbo.Properties.Items.Add(rMArec("Description"))	' New ComboBoxItem(rMArec("Description").ToString, rMArec("AHVCode").ToString))
			End While

		Catch ex As Exception
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' ALV-Code
	Sub ListMAALVCode(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Description"

		Dim strSqlQuery As String = "Select MA_LOSetting.ALVCode, TAB_ALV.Description From Mitarbeiter "
		strSqlQuery += " Inner Join MA_LOSetting On"
		strSqlQuery += " MA_LOSetting.MANr = Mitarbeiter.MANr"
		strSqlQuery += " Inner Join TAB_ALV On"
		strSqlQuery += " TAB_ALV.GetFeld = MA_LOSetting.ALVCode"
		strSqlQuery += " Group By MA_LOSetting.ALVCode, TAB_ALV.Description Order By MA_LOSetting.ALVCode"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.properties.items.Clear()
			cbo.properties.items.Add("")
			While rMArec.Read
				cbo.Properties.Items.Add(New ComboValue(rMArec("Description").ToString, rMArec("ALVCode").ToString))
			End While

		Catch ex As Exception
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMABVGCode(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Description"

		Dim strSqlQuery As String = "Select MA_LOSetting.BVGCode, TAB_BVG.Description From Mitarbeiter"
		strSqlQuery += " Inner Join MA_LOSetting On"
		strSqlQuery += " MA_LOSetting.MANr = Mitarbeiter.MANr"
		strSqlQuery += " Inner Join TAB_BVG On"
		strSqlQuery += " TAB_BVG.GetFeld = MA_LOSetting.BVGCode"
		strSqlQuery += " Group By MA_LOSetting.BVGCode, TAB_BVG.Description Order By MA_LOSetting.BVGCode"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("")
			While rMArec.Read
				cbo.Properties.Items.Add(New ComboValue(m_translate.GetSafeTranslationValue(("Description").ToString), rMArec("BVGCode").ToString))
			End While

		Catch ex As Exception
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

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
					lvwColumn.Width = CInt(strFieldWidth)		'* Screen.PrimaryScreen.BitsPerPixel  ' TwipsPerPixelX
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
		cbo.Properties.Items.Add("") ' New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboValue(m_translate.GetSafeTranslationValue("Nicht aktiviert"), "0"))
		cbo.Properties.Items.Add(New ComboValue(m_translate.GetSafeTranslationValue("Aktiviert"), "1"))
	End Sub
	' Vollständig - Nicht vollständig
	Sub ListCboVollstaendigNichtVoll(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		cbo.Properties.Items.Add("") ' New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboValue(m_translate.GetSafeTranslationValue("Nicht vollständig"), "0"))
		cbo.Properties.Items.Add(New ComboValue(m_translate.GetSafeTranslationValue("Vollständig"), "1"))
	End Sub

	' Ja - Nein
	Sub ListJaNein(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		cbo.Properties.Items.Add("")
		cbo.Properties.Items.Add(New ComboValue(m_translate.GetSafeTranslationValue("Nein"), "0"))
		cbo.Properties.Items.Add(New ComboValue(m_translate.GetSafeTranslationValue("Ja"), "1"))
	End Sub

	Sub ListVorhanden(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		cbo.Properties.Items.Add("")
		cbo.Properties.Items.Add(New ComboValue(m_translate.GetSafeTranslationValue("Nicht vorhanden"), "0"))
		cbo.Properties.Items.Add(New ComboValue(m_translate.GetSafeTranslationValue("Vorhanden"), "1"))
	End Sub

	Function GetKantonPLZ(ByVal strKanton As String) As String
		Dim strPLZResult As String = ","
		Dim strFieldName As String = "PLZ"

		Dim strSqlQuery As String = "Select PLZ.PLZ, PLZ.Kanton From PLZ "
		strSqlQuery += "Where PLZ.Kanton In ('" & strKanton & "') "
		strSqlQuery += "Group By PLZ.PLZ, PLZ.Kanton Order By PLZ.PLZ, PLZ.Kanton"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rPLZrec As SqlDataReader = cmd.ExecuteReader										' PLZ-Datenbank

			While rPLZrec.Read
				strPLZResult += rPLZrec(strFieldName).ToString & ","

			End While
			If strPLZResult.Length > 1 Then
				strPLZResult = Mid(strPLZResult, 2, Len(strPLZResult) - 2)
				strPLZResult = Replace(strPLZResult, ",", "','")
			Else
				strPLZResult = String.Empty
			End If

		Catch ex As Exception
			strPLZResult = String.Empty
			m_Logger.LogInfo(ex.ToString)
			m_UtilityUI.ShowErrorDialog(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strPLZResult
	End Function

	Sub FillMandantenCbo(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit) ' ComboBox)
		'Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.MDData.MDDbConn)
		'Conn.Open()
		'' Achtung: Hier darf es nicht in anderen Mandanten die Datenbank öffnen. 
		'' Hier handelt es sich um Mandantenliste innert lokaler MD.
		'Dim strQuery As String = "[Cockpit. Get All Allowed MDData]"
		'Dim cmd As SqlCommand = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
		'cmd.CommandType = CommandType.StoredProcedure

		'Dim rMDGuidrec As SqlDataReader = cmd.ExecuteReader
		'cbo.Properties.Items.Clear()

		'Dim Time_1 As Double = System.Environment.TickCount
		''Dim itm As DevExpress.XtraEditors.ComboBoxEdit

		'Try
		'  Dim i As Integer = 0
		'  While rMDGuidrec.Read
		'    cbo.Properties.Items.Add(New ComboBoxItem(String.Format("{0}", rMDGuidrec("MDName").ToString), _
		'                                   String.Format("{0}", rMDGuidrec("MDGuid").ToString), _
		'                                   String.Format("{0}", rMDGuidrec("DBName").ToString), _
		'                                   String.Format("{0}", rMDGuidrec("ImportedOn").ToString)))

		'    i += 1
		'  End While

		'  Dim Time_2 As Double = System.Environment.TickCount
		'  Console.WriteLine("Zeit für Mandanten: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

		'Catch e As Exception
		'  MsgBox(String.Format("Fehler (FillMandantenCbo): {0}", e.StackTrace))

		'Finally
		'  Conn.Close()
		'  Conn.Dispose()

		'End Try

	End Sub


#End Region




End Module
