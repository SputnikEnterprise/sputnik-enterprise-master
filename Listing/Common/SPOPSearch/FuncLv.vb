
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
Imports System.Threading.Thread


Imports SPOPSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Module FuncLv

	''' <summary>
	''' The logger.
	''' </summary>
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



#Region "Dropdown-Funktionen für 1. Seite..."

	Sub ListSort(ByVal cbo As DevExpress.XtraEditors.CheckedComboBoxEdit)

		cbo.Properties.Items.Add(String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Rechnungsnummer")))
		cbo.Properties.Items.Add(String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Kundennummer")))
		cbo.Properties.Items.Add(String.Format("2 - {0}", m_Translate.GetSafeTranslationValue("Rechnungsempfänger")))
		cbo.Properties.Items.Add(String.Format("3 - {0}", m_Translate.GetSafeTranslationValue("Valutadatum (Aufsteigend)")))
		cbo.Properties.Items.Add(String.Format("4 - {0}", m_Translate.GetSafeTranslationValue("Valutadatum (Absteigend)")))
		cbo.Properties.Items.Add(String.Format("5 - {0}", m_Translate.GetSafeTranslationValue("Totalbetrag (Aufsteigend)")))
		cbo.Properties.Items.Add(String.Format("6 - {0}", m_Translate.GetSafeTranslationValue("Totalbetrag (Absteigend)")))

		cbo.Properties.Items.Add(String.Format("7 - {0}", m_Translate.GetSafeTranslationValue("Rechnungsempfänger + Rechnungsnummer (Absteigend)")))
		cbo.Properties.Items.Add(String.Format("8 - {0}", m_Translate.GetSafeTranslationValue("Rechnungsempfänger + Rechnungsnummer (Aufsteigend)")))

		cbo.Properties.DropDownRows = 9

	End Sub

	Sub ListArt(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		cbo.Properties.Items.Add(String.Format("0 - {0}", m_Translate.GetSafeTranslationValue("Alle Rechnungen")))
		cbo.Properties.Items.Add(String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Offene/Teil-Offene Rechnungen")))
		cbo.Properties.Items.Add(String.Format("2 - {0}", m_Translate.GetSafeTranslationValue("Gebuchte (bezahlte) Rechnungen")))
		cbo.Properties.Items.Add(String.Format("3 - {0}", m_Translate.GetSafeTranslationValue("Gemahnte Rechnungen")))
		cbo.Properties.Items.Add(String.Format("4 - {0}", m_Translate.GetSafeTranslationValue("Gemahnte und heute noch offene Rechnungen")))
		cbo.Properties.Items.Add(String.Format("5 - {0}", m_Translate.GetSafeTranslationValue("MwSt.-pflichtige Liste aller Rechnungen")))
		cbo.Properties.Items.Add(String.Format("6 - {0}", m_Translate.GetSafeTranslationValue("MwSt.-freie Liste aller Rechnungen")))

		cbo.Properties.DropDownRows = 7
	End Sub

	Sub ListKDFiliale(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetKDFiliale]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("Bezeichnung").ToString)

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
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()
			strSqlQuery = "[Get OPBerater]" ' "[GetREKDBetrater]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim rFrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			Dim lst As New ListBox
			While rFrec.Read
				cbo.Properties.Items.Add(rFrec("USName").ToString)
			End While
			cbo.Properties.DropDownRows = 20
			'ListOPKst(cbo, lst)

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListOPKst(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal Lst As ListBox)
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
		Dim strUserName As String = String.Empty

		'Lst.Items.Clear()
		Dim Time_1 As Double = System.Environment.TickCount
		Try
			cbo.Properties.Items.Clear()

			Dim strKSTbez As String()
			Dim strKst As String
			Dim strAllKst As String = String.Empty
			Dim bInsertItem As Boolean

			cbo.Properties.Items.BeginUpdate()
			For i As Integer = 0 To Lst.Items.Count - 1
				With cbo
					strKst = Lst.Items(i).ToString
					strKSTbez = strKst.Split(CChar("/"))
					For j As Integer = 0 To strKSTbez.Length - 1
						strUserName = GetUSNameFromKst(strKSTbez(j))
						bInsertItem = AllowedtoInsertToCbo(cbo, strUserName)

						If bInsertItem Then
							.Properties.Items.Add(String.Format("{0} ({1})", strUserName, strKSTbez(j)))
						End If

					Next
				End With
			Next
			cbo.Properties.Items.EndUpdate()
			cbo.Properties.Sorted = True
			cbo.Properties.DropDownRows = 20

			Dim Time_2 As Double = System.Environment.TickCount
			Console.WriteLine("Zeit für LOL.KST: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))
			MsgBox(e.StackTrace)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Function AllowedtoInsertToCbo(ByVal Cbo As DevExpress.XtraEditors.ComboBoxEdit, ByVal strBez As String) As Boolean
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		For i As Integer = 0 To Cbo.Properties.Items.Count - 1
			If Cbo.Properties.Items(i).ToString.ToLower.Contains(strBez.ToLower) Then
				Return False
			End If
		Next

		Return True
	End Function

	Function GetUSNameFromKst(ByVal strSelectedKst As String) As String
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim strSqlQuery As String = "[Get UserData With USKst]"
		Dim strResult As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@USKst", strSelectedKst)
			Dim rRec As SqlDataReader = cmd.ExecuteReader

			While rRec.Read
				strResult = String.Format("{0}, {1}", rRec("Nachname"), rRec("Vorname"))
			End While

		Catch e As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, e.Message))

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strResult
	End Function

	Sub ListESEinstufung(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetREEinstufung]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("ES_Einstufung").ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListREBranche(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetREBranche]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("KDBranche").ToString)

			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListESRBankname(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetREESRBankname]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(New ComboValue(rZGrec("ESRBankname").ToString, rZGrec("KontoNr").ToString))
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListREMahnCode(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetREMahnCode]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("MahnCode").ToString)

			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	'Sub ListREMonth(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strSqlQuery As String = String.Empty
	'  Dim i As Integer = 0
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'  Dim iWidth As Integer

	'  Try
	'    Conn.Open()

	'    strSqlQuery = "[GetREMonth]"

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.StoredProcedure

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    'cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec("Fak_Month").ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec("Fak_Month").ToString)), iWidth, CInt(Len(rZGrec("Fak_Month").ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	'Sub ListREYear(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strSqlQuery As String = String.Empty
	'  Dim i As Integer = 0
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'  Dim iWidth As Integer

	'  Try
	'    Conn.Open()

	'    strSqlQuery = "[GetREYear]"

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.StoredProcedure

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("")
	'    'cbo.Items.Add("Leere Felder")
	'    'cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec("Fak_Year").ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec("Fak_Year").ToString)), iWidth, CInt(Len(rZGrec("Fak_Year").ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	Sub ListREArt(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			strSqlQuery = "[GetREArt]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

			cbo.Properties.Items.Clear()
			Dim codeAusgeschrieben As String = ""
			While rZGrec.Read
				Select Case rZGrec("Art").ToString
					Case "A"
						codeAusgeschrieben = "Automatische"
					Case "F"
						codeAusgeschrieben = "Festanstellung"
					Case "G"
						codeAusgeschrieben = "Gutschriften"
					Case "I"
						codeAusgeschrieben = "Individuelle"
				End Select
				'cbo.Items.Add(New ComboBoxItem(String.Format("({0}) {1}", rZGrec("Art").ToString, codeAusgeschrieben), rZGrec("Art").ToString))
				cbo.Properties.Items.Add(rZGrec("Art").ToString)

			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	'Sub ListRECurrency(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strSqlQuery As String = String.Empty
	'  Dim i As Integer = 0
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'  Dim iWidth As Integer

	'  Try
	'    Conn.Open()

	'    strSqlQuery = "[GetRECurrency]"

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.StoredProcedure

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Vorschussdatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("")
	'    'cbo.Items.Add("Leere Felder")
	'    'cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec("Currency").ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec("Currency").ToString)), iWidth, CInt(Len(rZGrec("Currency").ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	Sub ListREKst1(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()
			strSqlQuery = "[GetREKst1]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("Bezeichnung").ToString)
			End While

		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

	Sub ListREKst2(ByVal cbo As DevExpress.XtraEditors.ComboBoxEdit)
		Dim strSqlQuery As String = String.Empty
		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()
			strSqlQuery = "[GetREKst2]"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure

			Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

			cbo.Properties.Items.Clear()
			While rZGrec.Read
				cbo.Properties.Items.Add(rZGrec("Bezeichnung").ToString)
			End While


		Catch e As Exception
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

	End Sub

#End Region

#Region "Dropdown-Funktionen für 2. Seite..."

	'Sub ListKDCurrency(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strFieldName As String = "Currency"
	'  Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
	'  strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
	'  strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
	'  strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName

	'  Dim i As Integer = 0
	'  Dim iWidth As Integer = 0
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.Text

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec(strFieldName).ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec(strFieldName).ToString)), iWidth, CInt(Len(rZGrec(strFieldName).ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	'Sub ListKDMahnCode(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strFieldName As String = "Mahncode"
	'  Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
	'  strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
	'  strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
	'  strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName

	'  Dim i As Integer = 0
	'  Dim iWidth As Integer = 0
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.Text

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec(strFieldName).ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec(strFieldName).ToString)), iWidth, CInt(Len(rZGrec(strFieldName).ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	'Sub ListKDFakturaCode(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strFieldName As String = "Faktura"
	'  Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
	'  strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
	'  strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
	'  strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName

	'  Dim i As Integer = 0
	'  Dim iWidth As Integer = 0
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.Text

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec(strFieldName).ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec(strFieldName).ToString)), iWidth, CInt(Len(rZGrec(strFieldName).ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	'Sub ListKD1KLimite(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strFieldName As String = "Kreditlimite"
	'  Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
	'  strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
	'  strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
	'  strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName

	'  Dim i As Integer = 0
	'  Dim iWidth As Integer = 0
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.Text

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec(strFieldName).ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec(strFieldName).ToString)), iWidth, CInt(Len(rZGrec(strFieldName).ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	'Sub ListKD2KLimite(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strFieldName As String = "Kreditlimite_2"
	'  Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
	'  strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
	'  strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
	'  strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName

	'  Dim i As Integer = 0
	'  Dim iWidth As Integer = 0
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.Text

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(Format(Val(rZGrec(strFieldName).ToString), "0.00"))
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec(strFieldName).ToString)), iWidth, CInt(Len(rZGrec(strFieldName).ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	'Sub ListKDWithOP(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strFieldName As String = "OPVersand"
	'  Dim strSqlQuery As String = "Select " & strFieldName & " From Kunden "
	'  strSqlQuery += "Where " & strFieldName & " <> '' And " & strFieldName & " Is Not Null "
	'  strSqlQuery += "And KDFiliale Like '%" & _ClsProgSetting.GetUSFiliale() & "%' "
	'  strSqlQuery += "Group By " & strFieldName & " Order By " & strFieldName

	'  Dim i As Integer = 0
	'  Dim iWidth As Integer = 0
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.Text

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec(strFieldName).ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec(strFieldName).ToString)), iWidth, CInt(Len(rZGrec(strFieldName).ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

#End Region

#Region "DropDown-Funktionen für 3. Seite..."

	'Sub ListZBerater(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strSqlQuery As String = String.Empty
	'  Dim i As Integer = 0
	'  Dim iWidth As Integer
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    strSqlQuery = "[GetKDZBetrater]"

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.StoredProcedure

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec("Berater").ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec("Berater").ToString)), iWidth, CInt(Len(rZGrec("Berater").ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	'Sub ListZAbteilung(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strSqlQuery As String = String.Empty
	'  Dim i As Integer = 0
	'  Dim iWidth As Integer
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    strSqlQuery = "[GetKDZAbteilung]"

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.StoredProcedure

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec("DbValue").ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec("DbValue").ToString)), iWidth, CInt(Len(rZGrec("DbValue").ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	'Sub ListZPosition(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strSqlQuery As String = String.Empty
	'  Dim i As Integer = 0
	'  Dim iWidth As Integer
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    strSqlQuery = "[GetKDZPosition]"

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.StoredProcedure

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec("DbValue").ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec("DbValue").ToString)), iWidth, CInt(Len(rZGrec("DbValue").ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	'Sub ListZKontakt(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strSqlQuery As String = String.Empty
	'  Dim i As Integer = 0
	'  Dim iWidth As Integer
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    strSqlQuery = "[GetKDZKontakt]"

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.StoredProcedure

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec("DbValue").ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec("DbValue").ToString)), iWidth, CInt(Len(rZGrec("DbValue").ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	'Sub ListZStat(ByVal cbo As SPOPSearch.myCbo, ByVal iIndex As Integer)
	'  Dim strSqlQuery As String = String.Empty
	'  Dim i As Integer = 0
	'  Dim iWidth As Integer
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    strSqlQuery = "[GetKDZ" & iIndex.ToString & "Stat]"

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.StoredProcedure

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec("DbValue").ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec("DbValue").ToString)), iWidth, CInt(Len(rZGrec("DbValue").ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

	'Sub ListZGebMonth(ByVal cbo As SPOPSearch.myCbo)
	'  Dim strSqlQuery As String = String.Empty
	'  Dim i As Integer = 0
	'  Dim iWidth As Integer
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

	'  Try
	'    Conn.Open()

	'    strSqlQuery = "[GetKDZGebMonth]"

	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
	'    cmd.CommandType = Data.CommandType.StoredProcedure

	'    Dim rZGrec As SqlDataReader = cmd.ExecuteReader          ' Kundendatenbank

	'    cbo.Items.Clear()
	'    cbo.Items.Add("Leere Felder")
	'    cbo.Items.Add("---Daten")
	'    While rZGrec.Read
	'      cbo.Items.Add(rZGrec("DbValue").ToString)
	'      iWidth = CInt(IIf(iWidth > CInt(Len(rZGrec("DbValue").ToString)), iWidth, CInt(Len(rZGrec("DbValue").ToString))))

	'      i += 1
	'    End While
	'    cbo.DropDownWidth = CInt((iWidth * 7) + 20)

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

#End Region

#Region "Auflistungsfunktionen..."

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

	'Function FillFoundedKDData_Thrad_1(ByVal Lv As ListView, ByVal strQuery As String, ByVal frmTest As frmOPSearch) As Boolean
	'	Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'	Dim i As Integer = 0
	'	Dim TotalBetrag As Double = 0
	'	Dim strResult As String = String.Empty

	'	Try
	'		Conn.Open()
	'		Dim cmd As System.Data.SqlClient.SqlCommand
	'		cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)

	'		Dim rOPrec As SqlDataReader = cmd.ExecuteReader					 ' Offertendatenbank
	'		Lv.Items.Clear()
	'		Lv.FullRowSelect = True

	'		Dim Time_1 As Double = System.Environment.TickCount

	'		'      Lv.BeginUpdate()
	'		strResult = String.Empty
	'		Dim _ClsCtlUpdater As New ControlLVUpdater(Lv)
	'		While rOPrec.Read

	'			strResult += rOPrec("RENr").ToString
	'			strResult += ";" & rOPrec("KDNr").ToString
	'			strResult += ";" & rOPrec("R_Name1").ToString

	'			If Not IsDBNull(rOPrec("Fak_Dat")) Then
	'				strResult += ";" & Format(rOPrec("Fak_Dat"), "dd.MM.yyyy")
	'			Else
	'				strResult += ";" & ""
	'			End If

	'			If Not IsDBNull(rOPrec("BetragInk")) Then
	'				strResult += ";" & Format(CDbl(rOPrec("BetragInk")), "###,###,###,###,0.00")
	'			Else
	'				strResult += ";" & ""
	'			End If
	'			If Not IsDBNull(rOPrec("PrintedDate")) Then
	'				strResult += ";" & Format(CDate(rOPrec("PrintedDate")), "dd.MM.yyyy")
	'			Else
	'				strResult += ";" & ("")
	'			End If

	'			If Not IsDBNull(rOPrec("CreatedOn")) And Not IsDBNull(rOPrec("CreatedFrom")) Then
	'				strResult += ";" & Format(rOPrec("CreatedOn"), "dd.MM.yyyy") & " " & rOPrec("CreatedFrom").ToString
	'			Else
	'				strResult += ";" & ("")
	'			End If

	'			strResult += vbCrLf
	'			TotalBetrag += CDbl(rOPrec("BetragInk").ToString)

	'			'        _ClsCtlUpdater.AddText(strResult)

	'			'i += 1

	'			ClsDataDetail.GetTotalBetrag = TotalBetrag

	'		End While
	'		_ClsCtlUpdater.AddText(strResult)


	'		Dim Time_2 As Double = System.Environment.TickCount
	'		Console.WriteLine("Zeit für Datenbankauflistung: (" & ((Time_2 - Time_1)).ToString() + " ms)")


	'	Catch e As Exception
	'		Lv.Items.Clear()
	'		MsgBox(e.Message)

	'	Finally
	'		Conn.Close()
	'		Conn.Dispose()

	'	End Try

	'End Function


	'Sub FillFoundedKDData_Original(ByVal Lv As ListView, ByVal strQuery As String)
	'  Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)
	'  Dim i As Integer = 0
	'  Dim TotalBetrag As Double = 0

	'  Try
	'    Conn.Open()
	'    Dim cmd As System.Data.SqlClient.SqlCommand
	'    cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)

	'    Dim rOPrec As SqlDataReader = cmd.ExecuteReader          ' Offertendatenbank
	'    Lv.Items.Clear()
	'    Lv.FullRowSelect = True

	'    Dim Time_1 As Double = System.Environment.TickCount

	'    Lv.BeginUpdate()
	'    While rOPrec.Read
	'      With Lv
	'        .Items.Add(rOPrec("RENr").ToString)
	'        .Items(i).SubItems.Add(rOPrec("KDNr").ToString)
	'        .Items(i).SubItems.Add(rOPrec("R_Name1").ToString)

	'        If Not IsDBNull(rOPrec("Fak_Dat")) Then
	'          .Items(i).SubItems.Add(Format(rOPrec("Fak_Dat"), "dd.MM.yyyy"))
	'        Else
	'          .Items(i).SubItems.Add("")
	'        End If

	'        If Not IsDBNull(CDbl(rOPrec("BetragInk").ToString) * -1) Then
	'          .Items(i).SubItems.Add(Format(CDbl(rOPrec("BetragInk").ToString) * -1, "###,###,###,###,0.00"))
	'        Else
	'          .Items(i).SubItems.Add("")
	'        End If
	'        If Not IsDBNull(rOPrec("PrintedDate")) Then
	'          .Items(i).SubItems.Add(Format(rOPrec("PrintedDate"), "dd.MM.yyyy"))
	'        Else
	'          .Items(i).SubItems.Add("")
	'        End If

	'        If Not IsDBNull(rOPrec("CreatedOn")) And Not IsDBNull(rOPrec("CreatedFrom")) Then
	'          .Items(i).SubItems.Add(Format(rOPrec("CreatedOn"), "dd.MM.yyyy") & " " & rOPrec("CreatedFrom").ToString)
	'        Else
	'          .Items(i).SubItems.Add("")
	'        End If

	'        TotalBetrag += CDbl(rOPrec("BetragInk").ToString)

	'      End With

	'      i += 1
	'      Lv.EndUpdate()
	'      ClsDataDetail.GetTotalBetrag = TotalBetrag

	'    End While

	'    Dim Time_2 As Double = System.Environment.TickCount
	'    Console.WriteLine("Zeit für ListMailToFields: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


	'  Catch e As Exception
	'    Lv.Items.Clear()
	'    MsgBox(e.Message)

	'  Finally
	'    Conn.Close()
	'    Conn.Dispose()

	'  End Try

	'End Sub

#End Region

#Region "Sonstige Funktions..."

	Function GetKantonPLZ(ByVal strKanton As String) As String
		Dim strPLZResult As String = ","
		Dim strFieldName As String = "PLZ"

		Dim strSqlQuery As String = "Select PLZ.PLZ, PLZ.Kanton From PLZ "
		strSqlQuery += "Where PLZ.Kanton In ('" & strKanton & "') "
		strSqlQuery += "Group By PLZ.PLZ, PLZ.Kanton Order By PLZ.PLZ, PLZ.Kanton"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

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

	Function GetFilialKstData(ByVal strFiliale As String) As String
		Dim strKSTResult As String = ","
		Dim strFieldName As String = "KST"

		Dim strSqlQuery As String = "Select Benutzer.KST From Benutzer Left Join US_Filiale on Benutzer.USNr = US_Filiale.USNr "
		strSqlQuery += "Where US_Filiale.Bezeichnung = '" & strFiliale & "' And "
		strSqlQuery += "US_Filiale.Bezeichnung <> '' "
		strSqlQuery += "And US_Filiale.Bezeichnung Is Not Null Group By Benutzer.KST Order By Benutzer.KST"

		Dim Conn As SqlConnection = New SqlConnection(m_InitialData.MDData.MDDbConn)

		Try
			Conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSqlQuery, Conn)
			cmd.CommandType = Data.CommandType.Text

			Dim rPLZrec As SqlDataReader = cmd.ExecuteReader          ' PLZ-Datenbank

			While rPLZrec.Read
				strKSTResult += rPLZrec(strFieldName).ToString & ","

			End While
			Console.WriteLine("strKSTResult: " & strKSTResult)
			If strKSTResult.Length > 1 Then
				strKSTResult = Mid(strKSTResult, 2, Len(strKSTResult) - 2)
				strKSTResult = Replace(strKSTResult, ",", "','")
			Else
				strKSTResult = String.Empty
			End If

		Catch e As Exception
			strKSTResult = String.Empty
			MsgBox(e.Message)

		Finally
			Conn.Close()
			Conn.Dispose()

		End Try

		Return strKSTResult
	End Function

	'Sub ListForActivate(ByVal cbo As ComboBox)
	'  cbo.Items.Clear()
	'  Try
	'    cbo.Items.Add("")

	'    cbo.Items.Add("Aktiviert")
	'    cbo.Items.Add("Nicht Aktiviert")

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally

	'  End Try

	'End Sub

	'Sub ListForActivate_1(ByVal cbo As ComboBox)
	'  cbo.Items.Clear()
	'  Try
	'    cbo.Items.Add("")

	'    cbo.Items.Add("Aktiviert")
	'    cbo.Items.Add("Nicht Aktiviert")

	'    cbo.Items.Add("Leere Felder")

	'  Catch e As Exception
	'    MsgBox(e.Message)

	'  Finally

	'  End Try

	'End Sub

#End Region




End Module
