
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading
Imports DevExpress.LookAndFeel
Imports SPProgUtility.Mandanten
Imports SPProgUtility.MainUtilities
Imports System.IO
Imports SP.KD.CustomerMng.UI
Imports SP.Infrastructure.Logging

Public Class frmZESearch_LV
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_Utility_SP As SPProgUtility.MainUtilities.Utilities
	Private m_Mandant As Mandant

	Public Property RecCount As Integer
	Private Property Sql2Open As String
	Private Property _dBetragTotal As Decimal?


#Region "Constructor..."

	Public Sub New(ByVal strQuery As String)

		m_InitializationData = ClsDataDetail.m_InitialData
		m_Translate = ClsDataDetail.m_Translate

		m_Mandant = New Mandant
		m_Utility_SP = New SPProgUtility.MainUtilities.Utilities

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()


		Me.pnlMain.Dock = DockStyle.Fill
		Me.Sql2Open = strQuery
		_dBetragTotal = 0

		ResetGridCustomerData()

	End Sub

#End Region

	''' <summary>
	''' Gets the selected employee.
	''' </summary>
	''' <returns>The selected employee or nothing if none is selected.</returns>
	Public ReadOnly Property SelectedRecord As FoundedData
		Get
			Dim gvRP = TryCast(grdRP.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

			If Not (gvRP Is Nothing) Then

				Dim selectedRows = gvRP.GetSelectedRows()

				If (selectedRows.Count > 0) Then
					Dim employee = CType(gvRP.GetRow(selectedRows(0)), FoundedData)
					Return employee
				End If

			End If

			Return Nothing
		End Get

	End Property


	Function GetDbCustomerData4Show() As IEnumerable(Of FoundedData)
		Dim result As List(Of FoundedData) = Nothing
		m_Utility_SP = New Utilities

		Dim sql As String

		sql = Sql2Open

		Dim reader As SqlClient.SqlDataReader = m_Utility_SP.OpenReader(ClsDataDetail.m_InitialData.MDData.MDDbConn, sql, Nothing)

		Try

			If (Not reader Is Nothing) Then

				result = New List(Of FoundedData)

				While reader.Read()
					Dim overviewData As New FoundedData
					overviewData.ZENr = CInt(m_Utility_SP.SafeGetInteger(reader, "ZENr", 0))
					overviewData.RENr = CInt(m_Utility_SP.SafeGetInteger(reader, "RENr", 0))
					overviewData.KDNr = CInt(m_Utility_SP.SafeGetInteger(reader, "KDNr", 0))

					overviewData.fakdat = m_Utility_SP.SafeGetDateTime(reader, "fak_dat", Nothing)
					overviewData.vdate = m_Utility_SP.SafeGetDateTime(reader, "v_date", Nothing)
					overviewData.bdate = m_Utility_SP.SafeGetDateTime(reader, "b_date", Nothing)
					overviewData.InvoiceCreatedOn = Format(m_Utility_SP.SafeGetDateTime(reader, "InvoiceCreatedOn", Nothing), "d")

					overviewData.currency = String.Format("{0}", m_Utility_SP.SafeGetString(reader, "currency"))
					overviewData.betrag = m_Utility_SP.SafeGetDecimal(reader, "betrag", Nothing)
					_dBetragTotal += overviewData.betrag

					overviewData.mwstbetrag = m_Utility_SP.SafeGetDecimal(reader, "mwst-betrag", Nothing)
					overviewData.vd = m_Utility_SP.SafeGetDateTime(reader, "vd", Nothing)
					overviewData.vt = m_Utility_SP.SafeGetString(reader, "vt")

					overviewData.fbmonat = m_Utility_SP.SafeGetInteger(reader, "fbmonat", 0)
					overviewData.fksoll = m_Utility_SP.SafeGetInteger(reader, "fksoll", 0)
					overviewData.fkhaben = m_Utility_SP.SafeGetInteger(reader, "fkhaben", 0)
					overviewData.mwst = m_Utility_SP.SafeGetInteger(reader, "mwst", 0)

					overviewData.diskinfo = String.Format("{0}", m_Utility_SP.SafeGetString(reader, "diskinfo"))
					overviewData.createdon = m_Utility_SP.SafeGetDateTime(reader, "createdon", Nothing)
					overviewData.createdfrom = String.Format("{0}", m_Utility_SP.SafeGetString(reader, "createdfrom"))

					overviewData.changedon = m_Utility_SP.SafeGetDateTime(reader, "changedon", Nothing)
					overviewData.changedfrom = m_Utility_SP.SafeGetString(reader, "changedfrom")

					overviewData.mdnr = m_Utility_SP.SafeGetInteger(reader, "mdnr", 0)
					overviewData.rname1 = m_Utility_SP.SafeGetString(reader, "r_name1")
					overviewData.rname2 = m_Utility_SP.SafeGetString(reader, "r_name2")
					overviewData.rname3 = m_Utility_SP.SafeGetString(reader, "r_name3")
					overviewData.rzhd = m_Utility_SP.SafeGetString(reader, "r_zhd")
					overviewData.rabteilung = m_Utility_SP.SafeGetString(reader, "r_abteilung")
					overviewData.rpostfach = m_Utility_SP.SafeGetString(reader, "r_postfach")
					overviewData.rstrasse = m_Utility_SP.SafeGetString(reader, "r_strasse")
					overviewData.rplz = m_Utility_SP.SafeGetString(reader, "r_plz")
					overviewData.rort = m_Utility_SP.SafeGetString(reader, "r_ort")
					overviewData.rland = m_Utility_SP.SafeGetString(reader, "r_land")
					overviewData.refkhaben0 = m_Utility_SP.SafeGetInteger(reader, "refkhaben0", Nothing)
					overviewData.refkhaben1 = m_Utility_SP.SafeGetInteger(reader, "refkhaben1", Nothing)

					overviewData.zahlkond = m_Utility_SP.SafeGetString(reader, "zahlkond")
					overviewData.faellig = m_Utility_SP.SafeGetDateTime(reader, "faellig", Nothing)
					overviewData.mwstproz = m_Utility_SP.SafeGetDecimal(reader, "mwstproz", Nothing)
					overviewData.kst = m_Utility_SP.SafeGetString(reader, "kst")
					overviewData.rekst1 = m_Utility_SP.SafeGetString(reader, "rekst1")
					overviewData.rekst2 = m_Utility_SP.SafeGetString(reader, "rekst2")

					overviewData.reart = m_Utility_SP.SafeGetString(reader, "reart")
					overviewData.kreditrefnr = m_Utility_SP.SafeGetString(reader, "kredit_refnr")
					overviewData.kreditlimite = m_Utility_SP.SafeGetDecimal(reader, "kreditlimite", Nothing)

					overviewData.employeeadvisor = m_Utility_SP.SafeGetString(reader, "employeeadvisor")
					overviewData.customeradvisor = m_Utility_SP.SafeGetString(reader, "customeradvisor")

					result.Add(overviewData)

				End While

			End If

		Catch e As Exception
			result = Nothing
			m_Logger.LogError(e.ToString())

		Finally
			m_Utility_SP.CloseReader(reader)

		End Try

		Return result
	End Function

	Private Function LoadFoundedCustomerList() As Boolean

		Dim listOfEmployees = GetDbCustomerData4Show()

		Dim responsiblePersonsGridData = (From person In listOfEmployees
																			Select New FoundedData With
																						 {.ZENr = person.ZENr,
																							.RENr = person.RENr,
																							.KDNr = person.KDNr,
																									.fakdat = person.fakdat,
																							.vdate = person.vdate,
																							.bdate = person.bdate,
																							.currency = person.currency,
																							.betrag = person.betrag,
																						.mwstbetrag = person.mwstbetrag,
																						.vd = person.vd,
																						 .vt = person.vt,
																						 .fbmonat = person.fbmonat,
																						 .fbdat = person.fbdat,
																						 .fksoll = person.fksoll,
																						 .fkhaben = person.fkhaben,
																						 .mwst = person.mwst,
																						 .diskinfo = person.diskinfo,
																						 .createdon = person.createdon,
																						 .createdfrom = person.createdfrom,
																						 .changedon = person.changedon,
																						 .changedfrom = person.changedfrom,
																						 .mdnr = person.mdnr,
																						 .rname1 = person.rname1,
																						 .rname2 = person.rname2,
																						 .rname3 = person.rname3,
																						 .rzhd = person.rzhd,
																						 .rabteilung = person.rabteilung,
																						 .rpostfach = person.rpostfach,
																						 .rplz = person.rplz,
																						 .rort = person.rort,
																						 .rland = person.rland,
																						 .refkhaben0 = person.refkhaben0,
																						 .refkhaben1 = person.refkhaben1,
																						 .zahlkond = person.zahlkond,
																						 .faellig = person.faellig,
																						 .mwstproz = person.mwstproz,
																						 .kst = person.kst,
																						 .rekst1 = person.rekst1,
																							.rekst2 = person.rekst2,
																						 .reart = person.reart,
																						 .kreditrefnr = person.kreditrefnr,
																						 .kreditlimite = person.kreditlimite,
																						 .employeeadvisor = person.employeeadvisor,
																						 .customeradvisor = person.customeradvisor,
																						 .InvoiceCreatedOn = person.InvoiceCreatedOn
																						 }).ToList()

		Dim listDataSource As BindingList(Of FoundedData) = New BindingList(Of FoundedData)

		For Each p In responsiblePersonsGridData
			listDataSource.Add(p)
		Next

		grdRP.DataSource = listDataSource

		Return Not listOfEmployees Is Nothing
	End Function

	Private Sub ResetGridCustomerData()

		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvRP.Columns.Clear()

		Dim columnZENr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnZENr.Caption = m_Translate.GetSafeTranslationValue("Zahlungseingang-Nr.")
		columnZENr.Name = "ZENr"
		columnZENr.FieldName = "ZENr"
		columnZENr.Visible = True
		gvRP.Columns.Add(columnZENr)

		Dim columnRENr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnRENr.Caption = m_Translate.GetSafeTranslationValue("Debitoren-Nr.")
		columnRENr.Name = "RENr"
		columnRENr.FieldName = "RENr"
		columnRENr.Visible = True
		gvRP.Columns.Add(columnRENr)

		Dim columnKDNr As New DevExpress.XtraGrid.Columns.GridColumn()
		columnKDNr.Caption = m_Translate.GetSafeTranslationValue("Kunden-Nr.")
		columnKDNr.Name = "KDNr"
		columnKDNr.FieldName = "KDNr"
		columnKDNr.Visible = False
		gvRP.Columns.Add(columnKDNr)

		Dim columnrname1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrname1.Caption = m_Translate.GetSafeTranslationValue("Kunde")
		columnrname1.Name = "rname1"
		columnrname1.FieldName = "rname1"
		columnrname1.Visible = True
		gvRP.Columns.Add(columnrname1)

		Dim columnrname2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrname2.Caption = m_Translate.GetSafeTranslationValue("Firma2")
		columnrname2.Name = "rname2"
		columnrname2.FieldName = "rname2"
		columnrname2.Visible = False
		gvRP.Columns.Add(columnrname2)

		Dim columnrname3 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrname3.Caption = m_Translate.GetSafeTranslationValue("Firma3")
		columnrname3.Name = "rname3"
		columnrname3.FieldName = "rname3"
		columnrname3.Visible = False
		gvRP.Columns.Add(columnrname3)

		Dim columnfakdat As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfakdat.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnfakdat.Caption = m_Translate.GetSafeTranslationValue("Fakturadatum")
		columnfakdat.Name = "fakdat"
		columnfakdat.FieldName = "fakdat"
		columnfakdat.BestFit()
		columnfakdat.Visible = True
		gvRP.Columns.Add(columnfakdat)

		Dim columnvdate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnvdate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnvdate.Caption = m_Translate.GetSafeTranslationValue("Valutadatum")
		columnvdate.Name = "vdate"
		columnvdate.FieldName = "vdate"
		columnvdate.BestFit()
		columnvdate.Visible = True
		gvRP.Columns.Add(columnvdate)

		Dim columnbdate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbdate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnbdate.Caption = m_Translate.GetSafeTranslationValue("Buchungsdatum")
		columnbdate.Name = "bdate"
		columnbdate.FieldName = "bdate"
		columnbdate.BestFit()
		columnbdate.Visible = True
		gvRP.Columns.Add(columnbdate)

		Dim columnbetrag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnbetrag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnbetrag.Caption = m_Translate.GetSafeTranslationValue("Betrag")
		columnbetrag.Name = "betrag"
		columnbetrag.FieldName = "betrag"
		columnbetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnbetrag.DisplayFormat.FormatString = "n2"
		columnbetrag.BestFit()
		columnbetrag.Visible = True
		gvRP.Columns.Add(columnbetrag)

		Dim columnmwstbetrag As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmwstbetrag.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnmwstbetrag.Caption = m_Translate.GetSafeTranslationValue("MwSt.-Betrag")
		columnmwstbetrag.Name = "mwstbetrag"
		columnmwstbetrag.FieldName = "mwstbetrag"
		columnmwstbetrag.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmwstbetrag.DisplayFormat.FormatString = "n2"
		columnmwstbetrag.BestFit()
		columnmwstbetrag.Visible = False
		gvRP.Columns.Add(columnmwstbetrag)

		Dim columnmwstproz As New DevExpress.XtraGrid.Columns.GridColumn()
		columnmwstproz.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnmwstproz.Caption = m_Translate.GetSafeTranslationValue("MwSt.-Prozent")
		columnmwstproz.Name = "mwstproz"
		columnmwstproz.FieldName = "mwstproz"
		columnmwstproz.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnmwstproz.DisplayFormat.FormatString = "n2"
		columnmwstproz.BestFit()
		columnmwstproz.Visible = False
		gvRP.Columns.Add(columnmwstproz)

		Dim columnkonto As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkonto.Caption = m_Translate.GetSafeTranslationValue("Konto")
		columnkonto.Name = "fksoll"
		columnkonto.FieldName = "fksoll"
		columnkonto.Visible = True
		columnkonto.BestFit()
		gvRP.Columns.Add(columnkonto)

		Dim columnfkhaben As New DevExpress.XtraGrid.Columns.GridColumn()
		columnfkhaben.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnfkhaben.Caption = m_Translate.GetSafeTranslationValue("HABAN")
		columnfkhaben.Name = "fkhaben"
		columnfkhaben.FieldName = "fkhaben"
		columnfkhaben.Visible = False
		columnfkhaben.BestFit()
		gvRP.Columns.Add(columnfkhaben)

		Dim columnkst As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkst.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkst.Caption = m_Translate.GetSafeTranslationValue("KST")
		columnkst.Name = "kst"
		columnkst.FieldName = "kst"
		columnkst.Visible = False
		columnkst.BestFit()
		gvRP.Columns.Add(columnkst)

		Dim columnkst2 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkst2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkst2.Caption = m_Translate.GetSafeTranslationValue("2. KST")
		columnkst2.Name = "kst2"
		columnkst2.FieldName = "kst2"
		columnkst2.Visible = False
		columnkst2.BestFit()
		gvRP.Columns.Add(columnkst2)

		Dim columnkst3 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkst3.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkst3.Caption = m_Translate.GetSafeTranslationValue("3. KST")
		columnkst3.Name = "kst3"
		columnkst3.FieldName = "kst3"
		columnkst3.Visible = False
		columnkst3.BestFit()
		gvRP.Columns.Add(columnkst3)

		Dim columnkreditref As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkreditref.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkreditref.Caption = m_Translate.GetSafeTranslationValue("Kredit-Referenznummer")
		columnkreditref.Name = "kreditrefnr"
		columnkreditref.FieldName = "kreditrefnr"
		columnkreditref.Visible = False
		columnkreditref.BestFit()
		gvRP.Columns.Add(columnkreditref)

		Dim columnkreditlimite As New DevExpress.XtraGrid.Columns.GridColumn()
		columnkreditlimite.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnkreditlimite.Caption = m_Translate.GetSafeTranslationValue("Kreditlimit")
		columnkreditlimite.Name = "kreditlimite"
		columnkreditlimite.FieldName = "kreditlimite"
		columnkreditlimite.Visible = False
		columnkreditlimite.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
		columnkreditlimite.DisplayFormat.FormatString = "f2"
		columnkreditlimite.BestFit()
		gvRP.Columns.Add(columnkreditlimite)

		Dim columnemployeeadvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columnemployeeadvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnemployeeadvisor.Caption = m_Translate.GetSafeTranslationValue("Kandidaten-Berater")
		columnemployeeadvisor.Name = "employeeadvisor"
		columnemployeeadvisor.FieldName = "employeeadvisor"
		columnemployeeadvisor.Visible = False
		columnemployeeadvisor.BestFit()
		gvRP.Columns.Add(columnemployeeadvisor)

		Dim columncustomeradvisor As New DevExpress.XtraGrid.Columns.GridColumn()
		columncustomeradvisor.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncustomeradvisor.Caption = m_Translate.GetSafeTranslationValue("Kunden-Berater")
		columncustomeradvisor.Name = "customeradvisor"
		columncustomeradvisor.FieldName = "customeradvisor"
		columncustomeradvisor.Visible = False
		columncustomeradvisor.BestFit()
		gvRP.Columns.Add(columncustomeradvisor)

		Dim columnreart As New DevExpress.XtraGrid.Columns.GridColumn()
		columnreart.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnreart.Caption = m_Translate.GetSafeTranslationValue("Art")
		columnreart.Name = "reart"
		columnreart.FieldName = "reart"
		columnreart.Visible = False
		columnreart.BestFit()
		gvRP.Columns.Add(columnreart)


		Dim columnrefkhaben0 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrefkhaben0.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnrefkhaben0.Caption = m_Translate.GetSafeTranslationValue("HAHBEN 0")
		columnrefkhaben0.Name = "refkhaben0"
		columnrefkhaben0.FieldName = "refkhaben0"
		columnrefkhaben0.Visible = False
		columnrefkhaben0.BestFit()
		gvRP.Columns.Add(columnrefkhaben0)

		Dim columnrefkhaben1 As New DevExpress.XtraGrid.Columns.GridColumn()
		columnrefkhaben1.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnrefkhaben1.Caption = m_Translate.GetSafeTranslationValue("HABEN 1")
		columnrefkhaben1.Name = "refkhaben1"
		columnrefkhaben1.FieldName = "refkhaben1"
		columnrefkhaben1.Visible = False
		columnrefkhaben1.BestFit()
		gvRP.Columns.Add(columnrefkhaben1)

		Dim columncreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedon.Caption = m_Translate.GetSafeTranslationValue("Erstellt am")
		columncreatedon.Name = "createdon"
		columncreatedon.FieldName = "createdon"
		columncreatedon.Visible = False
		columncreatedon.BestFit()
		gvRP.Columns.Add(columncreatedon)

		Dim columncreatedfrom As New DevExpress.XtraGrid.Columns.GridColumn()
		columncreatedfrom.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columncreatedfrom.Caption = m_Translate.GetSafeTranslationValue("Erstellt durch")
		columncreatedfrom.Name = "createdfrom"
		columncreatedfrom.FieldName = "createdfrom"
		columncreatedfrom.Visible = False
		columncreatedfrom.BestFit()
		gvRP.Columns.Add(columncreatedfrom)

		Dim columnInvoiceCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnInvoiceCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnInvoiceCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Erstelldatum (Rechnung)")
		columnInvoiceCreatedOn.Name = "InvoiceCreatedOn"
		columnInvoiceCreatedOn.FieldName = "InvoiceCreatedOn"
		columnInvoiceCreatedOn.BestFit()
		columnInvoiceCreatedOn.Visible = False
		gvRP.Columns.Add(columnInvoiceCreatedOn)

		Dim columnVerfallTagFakDate As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVerfallTagFakDate.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnVerfallTagFakDate.Caption = m_Translate.GetSafeTranslationValue("Verfalltage: Fakturadatum")
		columnVerfallTagFakDate.Name = "VerfallTagFakDate"
		columnVerfallTagFakDate.FieldName = "VerfallTagFakDate"
		columnVerfallTagFakDate.Visible = False
		columnVerfallTagFakDate.BestFit()
		gvRP.Columns.Add(columnVerfallTagFakDate)

		Dim columnVerfallTagCreatedOn As New DevExpress.XtraGrid.Columns.GridColumn()
		columnVerfallTagCreatedOn.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnVerfallTagCreatedOn.Caption = m_Translate.GetSafeTranslationValue("Verfalltage: Erstellungsdatum")
		columnVerfallTagCreatedOn.Name = "VerfallTagCreatedOn"
		columnVerfallTagCreatedOn.FieldName = "VerfallTagCreatedOn"
		columnVerfallTagCreatedOn.Visible = False
		columnVerfallTagCreatedOn.BestFit()
		gvRP.Columns.Add(columnVerfallTagCreatedOn)


		grdRP.DataSource = Nothing

	End Sub


	Private Sub frmSearchKD_LV_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_LVLocation = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmLVHeight = Me.Height
				My.Settings.ifrmLVWidth = Me.Width

				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Sub TranslateControls()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)
			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
			bsiTotalbetrag.Caption = m_Translate.GetSafeTranslationValue(bsiTotalbetrag.Caption)
			bbiPrintList.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrintList.Caption)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub frmOnLoad(sender As Object, e As System.EventArgs) Handles Me.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim strStyleName As String = m_Mandant.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, 0, String.Empty)
		If strStyleName <> String.Empty Then
			UserLookAndFeel.Default.SetSkinStyle(strStyleName)
		End If

		TranslateControls()
		Try
			Me.Width = Math.Max(My.Settings.ifrmLVWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmLVHeight, Me.Height)

			If My.Settings.frm_LVLocation <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_LVLocation.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Setting FormSize:{1}", strMethodeName, ex.Message))

		End Try

		Try
			LoadFoundedCustomerList()

			Me.RecCount = gvRP.RowCount
			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("Anzahl Datensätze: {0}"), Me.RecCount)
			Me.bsiTotalbetrag.Caption = String.Format(m_Translate.GetSafeTranslationValue("Totalbetrag: {0}"), Format(Me._dBetragTotal, "n2"))

			AddHandler gvRP.RowCellClick, AddressOf Ongv_RowCellClick

		Catch ex As Exception

		End Try

	End Sub

	Sub Ongv_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)

		If (e.Clicks = 2) Then

			Dim column = e.Column
			Dim dataRow = gvRP.GetRow(e.RowHandle)
			If Not dataRow Is Nothing Then
				Dim viewData = CType(dataRow, FoundedData)

				Select Case column.Name.ToLower
					Case "rname1", "kdnr", "rname2", "rname3"
						If viewData.rname1.Length > 0 Then RunOpenKDForm(viewData.KDNr)

					Case "renr", "fakdat"
						If viewData.RENr > 0 Then RunOpenOPForm(viewData.RENr)

					Case Else
						If viewData.ZENr > 0 Then RunOpenZEForm(viewData.ZENr)


				End Select

			End If

		End If

	End Sub

	Private Sub bbiPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrintList.ItemClick
		If gvRP.RowCount > 0 Then
			' Opens the Preview window. 
			grdRP.ShowPrintPreview()
		End If

	End Sub

	Private Sub btnTotalBetrag_DropDownOpening(sender As Object, e As System.EventArgs)
		Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
		For Each itm As ToolStripItem In ts.DropDownItems
			itm.Text = m_Translate.GetSafeTranslationValue(itm.Text)
			Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
		Next
	End Sub


End Class