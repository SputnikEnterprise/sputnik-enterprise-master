
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports SP.Infrastructure.Logging

Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_xml As New ClsXML


#Region "Dropdown-Funktionen für 1. Seite..."

	' Sortierung -------------------------------------------------------------------------------------------------
	Sub ListSort(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)

		cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(String.Format("0 - {0}", m_xml.GetSafeTranslationValue("Kandidatennnummer")))
		cbo.Properties.Items.Add(String.Format("1 - {0}", m_xml.GetSafeTranslationValue("Kandidatennname")))
		cbo.Properties.Items.Add(String.Format("2 - {0}", m_xml.GetSafeTranslationValue("Kandidatenstrasse")))
		cbo.Properties.Items.Add(String.Format("3 - {0}", m_xml.GetSafeTranslationValue("Kandidatenort")))
		cbo.Properties.Items.Add(String.Format("4 - {0}", m_xml.GetSafeTranslationValue("Kandidaten-PLZ")))
		cbo.Properties.Items.Add(String.Format("5 - {0}", m_xml.GetSafeTranslationValue("Geburtsdatum (nach Datum)")))
		cbo.Properties.Items.Add(String.Format("6 - {0}", m_xml.GetSafeTranslationValue("Geburtstage (nach Tage)")))

	End Sub

	Sub ListMABewilligung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Bewilligung"

		Dim strSqlQuery As String = "Select Mitarbeiter.Bewillig as Bewilligung From Mitarbeiter "
		strSqlQuery += "Where Not (Mitarbeiter.Bewillig = '' Or Mitarbeiter.Bewillig Is Null) "
		strSqlQuery += "Group By Mitarbeiter.Bewillig Order By Mitarbeiter.Bewillig"

		Dim i As Integer = 0
		Dim iWidth As Integer = 0
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
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

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
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' 

			cbo.Properties.Items.Clear()
			While rKDrec.Read
				cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
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

	Sub ListMABewBisJahr(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Jahr"
		Dim strSqlQuery As String = "Select Year(Bew_Bis) as Jahr From Mitarbeiter "
		strSqlQuery += "Where Bew_Bis Is Not Null "
		strSqlQuery += "And Year(Bew_Bis) >= Year(GetDate())"
		strSqlQuery += "Group By Year(Bew_Bis) Order By Year(Bew_Bis)"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rKDrec As SqlDataReader = cmd.ExecuteReader          ' 

			cbo.Properties.Items.Clear()
			While rKDrec.Read
				cbo.Properties.Items.Add(rKDrec(strFieldName).ToString)
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

	Sub ListHeimatort(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strEntry As String
		Dim strSqlQuery As String = "[GetMAGebOrt]"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rMArec As SqlDataReader = cmd.ExecuteReader                  '

			cbo.Properties.Items.Clear()
			While rMArec.Read
				strEntry = rMArec("GebOrt").ToString
				cbo.Properties.Items.Add(New ComboBoxItem(strEntry, rMArec("GebOrt").ToString))
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

	Sub ListMAQSteuer(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "QSteuer"
		Dim strSqlQuery As String = "Select Mitarbeiter.Q_Steuer as QSteuer From Mitarbeiter "
		strSqlQuery += "Where Not (Mitarbeiter.Q_Steuer = '' Or Mitarbeiter.Q_Steuer Is Null) "
		strSqlQuery += "Group By Mitarbeiter.Q_Steuer Order By Mitarbeiter.Q_Steuer"

		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader          ' 

			cbo.Properties.Items.Clear()
			While rMArec.Read
				cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
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

	Sub ListMAAnsaessigkeit(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Ansaessigkeit"

		Dim strSqlQuery As String = "Select Mitarbeiter.Ansaessigkeit From Mitarbeiter "
		strSqlQuery += "Where Not (Mitarbeiter.Ort = '' Or Mitarbeiter.Ort is Null) "
		strSqlQuery += "Group By Mitarbeiter.Ansaessigkeit Order By Mitarbeiter.Ansaessigkeit"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim rMArec As SqlDataReader = cmd.ExecuteReader          ' 

			cbo.Properties.Items.Clear()
			While rMArec.Read
				cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
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

	Sub ListMAKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Kanton"

		Dim strSqlQuery As String = "Select Mitarbeiter.MA_Kanton as Kanton From Mitarbeiter "
		strSqlQuery += "Where Not (Mitarbeiter.MA_Kanton = '' Or Mitarbeiter.MA_Kanton is Null)"
		strSqlQuery += "Group By Mitarbeiter.MA_Kanton Order By Mitarbeiter.MA_Kanton"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

			cbo.Properties.Items.Clear()
			While rMArec.Read
				cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
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

	Sub ListMASKanton(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Kanton"

		Dim strSqlQuery As String = "Select Mitarbeiter.S_Kanton as Kanton From Mitarbeiter "
		strSqlQuery += "Where Not (Mitarbeiter.S_Kanton = '' Or Mitarbeiter.S_Kanton is Null)"
		strSqlQuery += "Group By Mitarbeiter.S_Kanton Order By Mitarbeiter.S_Kanton"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

			cbo.Properties.Items.Clear()
			While rMArec.Read
				cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
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

			Dim rMArec As SqlDataReader = cmd.ExecuteReader          '

			cbo.Properties.Items.Clear()
			Dim valueText As String
			Dim valueName As String

			While rMArec.Read
				valueText = m_uttility.SafeGetString(rMArec, "LandName")
				valueName = m_uttility.SafeGetString(rMArec, strFieldName)

				cbo.Properties.Items.Add(New ComboBoxItem(If(String.IsNullOrWhiteSpace(valueText), valueName, valueText), valueName))
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
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader          '

			cbo.Properties.Items.Clear()
			While rMArec.Read
				cbo.Properties.Items.Add(New ComboBoxItem(rMArec("Description").ToString, rMArec("GetFeld").ToString))
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

	Sub ListMANationialitaet(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Nationality"
		Dim m_uttility As New SPProgUtility.MainUtilities.Utilities

		Dim strSqlQuery As String = "Select MA.Nationality, ISNull( (Select Top 1 Land From LND Where Code = MA.Nationality), '') As LandName From Mitarbeiter MA "
		strSqlQuery += "Where Not (MA.Nationality = '' Or MA.Nationality Is Null) "
		strSqlQuery += "Group By MA.Nationality Order By MA.Nationality"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader          '

			cbo.Properties.Items.Clear()
			Dim valueText As String
			Dim valueName As String


			While rMArec.Read
				valueText = m_uttility.SafeGetString(rMArec, "LandName")
				valueName = m_uttility.SafeGetString(rMArec, strFieldName)

				cbo.Properties.Items.Add(New ComboBoxItem(If(String.IsNullOrWhiteSpace(valueText), valueName, valueText), valueName))
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
#End Region


#Region "Dropdown-Funktionen für 2. Seite..."

	Sub ListMABetreuer(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = "[GetMABetrater]"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rMArec.Read
				cbo.Properties.Items.Add(New ComboBoxItem(String.Format("{0} ({1})", CStr(rMArec("USName")), rMArec("KST").ToString), rMArec("KST").ToString))
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
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

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
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMAGeschäftsstellen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Bezeichnung"

		Dim strSqlQuery As String = "[GetMAFiliale]"
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
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
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
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMAFahrzeug(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Fahrzeug"

		Dim strSqlQuery As String = "Select SON.Fahrzeug as Fahrzeug From "
		strSqlQuery += "MASonstiges SON "
		strSqlQuery += "Where Not (SON.Fahrzeug is null Or SON.Fahrzeug = '') "
		strSqlQuery += "Group By SON.Fahrzeug Order By SON.Fahrzeug"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("Leere Felder")
			cbo.Properties.Items.Add("---Alle")
			While rMArec.Read
				cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
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
	' Führerausweis
	Sub ListMAFuehrerausweis(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Fuehrerschein"

		Dim strSqlQuery As String = "[Get MAFuehrerscheine]"
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
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

#End Region

#Region "DropDown-Funktionen für 4. Seite..."
	' Aktiviert - Nicht aktiviert
	Sub ListCboAktiviertNichtAktiviert(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Nicht aktiviert"), "0"))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Aktiviert"), "1"))

	End Sub

	Sub ListCboVollstaendigNichtVoll(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Nicht vollständig"), "0"))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Vollständig"), "1"))

	End Sub

	Sub ListCboJaNein(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Nein"), "0"))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Ja"), "1"))

	End Sub

	Sub ListCboVorhanden(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Nicht vorhanden"), "0"))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Vorhanden"), "1"))

	End Sub

	Sub ListCboJaNeinK(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Nein mit Zahlungsart K"), "0"))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Ja mit Zahlungsart K"), "1"))

	End Sub

	Sub ListCboMitOhne(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		cbo.Properties.Items.Add(New ComboBoxItem("", ""))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Ohne"), "0"))
		cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue("Mit"), "1"))
		cbo.Properties.Items.Clear()

	End Sub

	Sub ListMAReserve(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal num As Integer)
		Dim strFieldName As String = String.Format("Res{0}", num)

		Dim strSqlQuery As String = "Select Res{0} From MAKontakt_Komm "
		strSqlQuery += "Where Res{0} is not null and Res{0} <> '' "
		strSqlQuery += "Group By Res{0} Order By Res{0}"

		strSqlQuery = String.Format(strSqlQuery, num)
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
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

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
			If rMArec.HasRows Then
				While rMArec.Read
					cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
				End While
				cbo.Properties.DropDownRows = 20
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMAKuendigungsfristen(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "KundFristen"

		Dim strSqlQuery As String = "Select KundFristen From MAKontakt_Komm"
		strSqlQuery += " Where KundFristen is not Null and KundFristen <> ''"
		strSqlQuery += " Group By KundFristen Order By KundFristen"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			cbo.Properties.Items.Add("")
			If rMArec.HasRows Then
				While rMArec.Read
					cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
				End While
				cbo.Properties.DropDownRows = 20
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' AGB für WOS
	Sub ListMAAGBfürWOS(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "AGB_WOS"

		Dim strSqlQuery As String = "Select AGB_WOS From MAKontakt_Komm"
		strSqlQuery += " Where AGB_WOS is not Null and AGB_WOS <> ''"
		strSqlQuery += " Group By AGB_WOS Order By AGB_WOS"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			If rMArec.HasRows Then
				While rMArec.Read
					cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
				End While
				cbo.Properties.DropDownRows = 20
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' Zahlungsarten
	Sub ListMAZahlungsarten(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Zahlungsarten"

		Dim strSqlQuery As String = "Select malo.Zahlart As Zahlungsarten, tb.Description as Bezeichnung From MA_LOSetting malo Left Join TAB_Zahlart tb On MALo.Zahlart = tb.GetFeld "
		strSqlQuery += " Where malo.Zahlart is not null and malo.Zahlart <> ''"
		strSqlQuery += " Group By malo.Zahlart, tb.Description Order By malo.Zahlart"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			If rMArec.HasRows Then
				While rMArec.Read
					cbo.Properties.Items.Add(New ComboBoxItem(m_xml.GetSafeTranslationValue(rMArec("Bezeichnung").ToString), rMArec(strFieldName).ToString))
				End While
				cbo.Properties.DropDownRows = 20
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' Währung
	Sub ListMAWaehrung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Waehrung"

		Dim strSqlQuery As String = "Select MA_LOSetting.Currency as Waehrung From "
		strSqlQuery += "MA_LOSetting Where Currency <> '' And Currency Is Not Null "
		strSqlQuery += "Group By MA_LOSetting.Currency Order By MA_LOSetting.Currency"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			If rMArec.HasRows Then
				While rMArec.Read
					cbo.Properties.Items.Add(rMArec(strFieldName).ToString)
				End While
				cbo.Properties.DropDownRows = 20
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	' AHV-Code
	Sub ListMAAHVCode(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strFieldName As String = "Description"

		Dim strSqlQuery As String = "Select MA_LOSetting.AHVCode, TAB_AHV.Description From Mitarbeiter"
		strSqlQuery += " Inner Join MA_LOSetting On"
		strSqlQuery += " MA_LOSetting.MANr = Mitarbeiter.MANr"
		strSqlQuery += " Inner Join TAB_AHV On"
		strSqlQuery += " TAB_AHV.GetFeld = MA_LOSetting.AHVCode"
		strSqlQuery += " Group By MA_LOSetting.AHVCode, TAB_AHV.Description Order By MA_LOSetting.AHVCode"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			If rMArec.HasRows Then
				While rMArec.Read
					cbo.Properties.Items.Add(New ComboBoxItem(rMArec("Description").ToString, rMArec("AHVCode").ToString))
				End While
				cbo.Properties.DropDownRows = 20
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

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
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			If rMArec.HasRows Then
				While rMArec.Read
					cbo.Properties.Items.Add(New ComboBoxItem(rMArec("Description").ToString, rMArec("ALVCode").ToString))
				End While
				cbo.Properties.DropDownRows = 20
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

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
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			If rMArec.HasRows Then
				While rMArec.Read
					cbo.Properties.Items.Add(New ComboBoxItem(rMArec("Description").ToString, rMArec("BVGCode").ToString))
				End While
			End If
			cbo.Properties.DropDownRows = 20

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListMASUVACode2(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		Dim strSqlQuery As String = "Select MA_LOSetting.SecSuvaCode, TAB_2Suva.Bezeichnung From MA_LOSetting"
		strSqlQuery += " Inner Join TAB_2Suva On"
		strSqlQuery += " TAB_2Suva.GetFeld = MA_LOSetting.SecSuvaCode"
		strSqlQuery += " Group By MA_LOSetting.SecSuvaCode, TAB_2Suva.Bezeichnung Order By MA_LOSetting.SecSuvaCode"
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			' Leeres Feld hinzufügen
			cbo.Properties.Items.Add("")

			If rMArec.HasRows Then
				While rMArec.Read
					cbo.Properties.Items.Add(New ComboBoxItem(rMArec("Bezeichnung").ToString, rMArec("SecSuvaCode").ToString))
				End While
				cbo.Properties.DropDownRows = 20
			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try
	End Sub

	Sub ListEmployeeALKKasse(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		Dim strSqlQuery As String = "Select ALKNumber, ALKName, ALKLocation From MAKontakt_Komm Where ALKName <> '' AND ALKNumber IS NOT Null Group By ALKNumber, ALKName, ALKLocation Order By ALKName, ALKLocation "
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rMArec As SqlDataReader = cmd.ExecuteReader          '

			cbo.Properties.Items.Clear()
			While rMArec.Read
				cbo.Properties.Items.Add(New ComboBoxItem(String.Format("{0}, {1}", rMArec("ALKName").ToString, rMArec("ALKLocation").ToString), rMArec("ALKNumber").ToString))
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

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			strPLZResult = String.Empty
			MsgBox(ex.Message)


		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strPLZResult
	End Function

	Sub ListForActivate(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		Try
			cbo.Properties.Items.Add(m_xml.GetSafeTranslationValue("Aktiviert"))
			cbo.Properties.Items.Add(m_xml.GetSafeTranslationValue("Nicht Aktiviert"))

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally

		End Try

	End Sub

	Sub ListForActivate_1(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)

		cbo.Properties.Items.Clear()
		Try
			cbo.Properties.Items.Add(m_xml.GetSafeTranslationValue("Aktiviert"))
			cbo.Properties.Items.Add(m_xml.GetSafeTranslationValue("Nicht Aktiviert"))

		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally

		End Try

	End Sub

	Function GetUSTitel(ByVal iUSNr As Integer) As String
		Dim strResult As String = String.Empty
		Dim sSql As String
		Dim ConnDbSelect As New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)

		Try
			sSql = "Select USTitel_1, USTitel_2 From Benutzer Where USNr = @USNr"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, ConnDbSelect)
			Dim param As System.Data.SqlClient.SqlParameter
			Dim rUSrec As SqlClient.SqlDataReader

			Try
				ConnDbSelect.Open()
				param = cmd.Parameters.AddWithValue("@USNr", iUSNr)

				rUSrec = cmd.ExecuteReader          ' User-Datenbank
				rUSrec.Read()
				If rUSrec.HasRows Then
					If Not IsDBNull(rUSrec("USTitel_1")) Then
						strResult = rUSrec("USTitel_1").ToString
					End If
					If Not IsDBNull(rUSrec("USTitel_2")) Then
						strResult &= rUSrec("USTitel_2").ToString
					End If
				End If
				rUSrec.Close()

			Catch ex As Exception
				MsgBox(Err.Description, MsgBoxStyle.Critical, "GetUSTitel_1")

			Finally

			End Try


		Catch ex As Exception
			m_Logger.LogError(ex.ToString())
			MsgBox(ex.Message)

		Finally
			ConnDbSelect.Close()

		End Try
		Return strResult

	End Function

	Function IsAllowedInsert2Lst(ByVal lst As DevExpress.XtraEditors.ListBoxControl, ByVal strSearchText As String) As Boolean

		For i As Integer = 0 To lst.Items.Count - 1
			If lst.Items(i).ToString = strSearchText Then
				Return False
			End If
		Next

		Return True
	End Function
#End Region

#Region "Funktionen für Vakanzen..."

	Function GetVakDb(ByVal iVakNr As Integer,
										ByVal strTablename As String,
										ByVal strFieldName As String) As String
		Dim strSqlQuery As String = String.Format("Select {0} From {1} Where {0} <> '' And {0} Is Not Null ",
																							strFieldName, strTablename)
		strSqlQuery &= String.Format("And VakNr = @VakNr Group By {0} Order By {0} ", strFieldName, strTablename)
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Dim strResult As String = String.Empty

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text
			cmd.Parameters.AddWithValue("@VakNr", iVakNr)

			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          '

			While rVakrec.Read
				strResult &= If(strResult = String.Empty, "", "#@") & (rVakrec(strFieldName).ToString)

			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function

#End Region

#Region "Funktionen für Gridview..."

	Function GetDbData4BerufGruppe() As DataTable
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Dim strQuery As String = "[List MABerufgruppenData For Search In MASearch]"
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "MABerufGruppe")

		Return ds.Tables(0)
	End Function

	Function GetDbData4Fachbereich() As DataTable
		Dim ds As New DataSet
		Dim dt As New DataTable
		Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetSelectedMDConnstring)
		Dim strQuery As String = "[List MAFachbereichData For Search In MASearch]"
		Dim cmd As System.Data.SqlClient.SqlCommand = New SqlCommand(strQuery, Conn)
		cmd.CommandType = CommandType.StoredProcedure

		Dim objAdapter As New SqlDataAdapter

		objAdapter.SelectCommand = cmd
		objAdapter.Fill(ds, "MAFachbereich")

		Return ds.Tables(0)
	End Function

#End Region



End Module
