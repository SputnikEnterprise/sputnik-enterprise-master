
Option Strict Off

Imports SP.Infrastructure.UI.UtilityUI

Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports DevExpress.XtraEditors.Controls
Imports System.Threading
Imports SPProgUtility.SPUserSec.ClsUserSec

Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings
Imports SPProgUtility.Mandanten

Imports DevExpress.LookAndFeel
Imports SPKDSearch.ClsDataDetail
Imports SP.Infrastructure.Logging

Public Class frmKDSearch
	Inherits DevExpress.XtraEditors.XtraForm

	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_xml As New ClsXML
	Private _ClsFunc As New ClsDivFunc

	Private b1_1 As Boolean
	Private b1_2 As Boolean
	Private b1_3 As Boolean

	Private b2_1 As Boolean
	Private b2_2 As Boolean
	Private b2_3 As Boolean

	Private b3_1 As Boolean
	Private b3_2 As Boolean
	Private b3_3 As Boolean

	Private b4_2 As Boolean

	Private m_md As Mandant

	Public Shared frmMyLV As frmKDSearch_LV

	Private Property ShortSQLQuery As String
	Private Property GetMDDbName As String
	Private Property GetMDGuid As String

	Private Property PrintJobNr As String
	Private Property SQL4Print As String
	Private Property bPrintAsDesign As Boolean

	Private Property TranslatedPage As New List(Of Boolean)

	Private m_UtilityUI As SP.Infrastructure.UI.UtilityUI



#Region "Constructor..."

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		m_md = New Mandant
		m_UtilityUI = New SP.Infrastructure.UI.UtilityUI

		AddHandler Cbo_KDKontakt.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_KD1Stat.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_KD2Stat.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler Cbo_KD1Res.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_KD2Res.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_KD3Res.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_KD4Res.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler btnHideKDBerufe.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideKDBranchen.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideKDGAV.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideKDAnstellung.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideKDStichwort.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideKDFiliale.Click, AddressOf OnHideShow_ButtonClick

		AddHandler Cbo_KDCurrency.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_KDMahn.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_KDFaktura.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler Cbo_ZKontakt.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_Z1Stat.ButtonClick, AddressOf OnDropDown_ButtonClick
		AddHandler Cbo_Z2Stat.ButtonClick, AddressOf OnDropDown_ButtonClick

		AddHandler btnHideZKom.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideZVersand.Click, AddressOf OnHideShow_ButtonClick

		AddHandler btnHideZBerufe.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideZBranche.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideZRes1.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideZRes2.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideZRes3.Click, AddressOf OnHideShow_ButtonClick
		AddHandler btnHideZRes4.Click, AddressOf OnHideShow_ButtonClick

		ResetMandantenDropDown()

		Cbo_KDKontakt.Properties.SeparatorChar = "#"
		Cbo_KD1Res.Properties.SeparatorChar = "#"
		Cbo_KD2Res.Properties.SeparatorChar = "#"
		Cbo_KD3Res.Properties.SeparatorChar = "#"
		Cbo_KD4Res.Properties.SeparatorChar = "#"
		cbo_KDArtHide.Properties.SeparatorChar = "#"

	End Sub

#End Region


#Region "Lookup Edit Reset und Load..."

	''' <summary>
	''' Resets the Mandanten drop down.
	''' </summary>
	Private Sub ResetMandantenDropDown()

		lueMandant.Properties.Items.Clear()

		Dim _ClsFunc As New ClsDbFunc
		Dim Data = _ClsFunc.LoadMandantenData()

		lueMandant.Properties.DisplayMember = "MDName"
		lueMandant.Properties.ValueMember = "MDNr"

		lueMandant.Properties.DataSource = Data

	End Sub

	''' <summary>
	''' handle editvalue changed
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub OnlueMandant_EditValueChanged(sender As Object, e As System.EventArgs)
		Dim bSetToDefault As Boolean = False

		If String.IsNullOrWhiteSpace(Me.lueMandant.Properties.GetCheckedItems) Then Exit Sub

		If ClsDataDetail.MDData.MultiMD = 0 AndAlso Me.lueMandant.EditValue.ToString.Contains(",") Then
			m_UtilityUI.ShowInfoDialog("Es kann nur aus einer Mandant gesucht werden. Ich wähle den Hauptmandant.")

			bSetToDefault = True

		End If
		If Me.lueMandant.EditValue.ToString.Contains(",") Then bSetToDefault = True

		If Not bSetToDefault Then
			Dim SelectedData = lueMandant.Properties.GetItems.GetCheckedValues(0)

			If Not SelectedData Is Nothing Then
				ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(Me.lueMandant.EditValue)
				ClsDataDetail.UserData = ClsDataDetail.LogededUSData(Me.lueMandant.EditValue, ClsDataDetail.UserData.UserLName, ClsDataDetail.UserData.UserFName)

				bSetToDefault = False

			Else
				bSetToDefault = True

			End If

		Else
			ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
			ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, ClsDataDetail.UserData.UserNr)
			ClsDataDetail.m_InitialData = New SP.Infrastructure.Initialization.InitializeClass(ClsDataDetail.TranslationData, ClsDataDetail.ProsonalizedData, ClsDataDetail.MDData, ClsDataDetail.UserData)


		End If

		Me.bbiSearch.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiClear.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiPrint.Enabled = Not (ClsDataDetail.MDData Is Nothing)
		Me.bbiExport.Enabled = Not (ClsDataDetail.MDData Is Nothing)

	End Sub


#End Region


	Public ReadOnly Property ExcludeSelectedComboboxValue(ByVal sender As DevExpress.XtraEditors.ComboBoxEdit) As Boolean
		Get
			Dim cb As DevExpress.XtraEditors.ComboBoxEdit = CType(sender, DevExpress.XtraEditors.ComboBoxEdit)
			If cb.Properties.Buttons.Count > 1 Then
				Return CInt(cb.Properties.Buttons(1).Tag) = 1
			Else
				Return False
			End If

		End Get
	End Property

	Public ReadOnly Property ExcludeSelectedCheckedComboboxValue(ByVal sender As DevExpress.XtraEditors.CheckedComboBoxEdit) As Boolean
		Get
			Dim cb As DevExpress.XtraEditors.CheckedComboBoxEdit = CType(sender, DevExpress.XtraEditors.CheckedComboBoxEdit)
			If cb.Properties.Buttons.Count > 1 Then
				Return CInt(cb.Properties.Buttons(1).Tag) = 1
			Else
				Return False
			End If

		End Get
	End Property

	Public ReadOnly Property ExcludeSelectedLSTValue(ByVal sender As DevExpress.XtraEditors.SimpleButton) As Boolean
		Get
			Dim btn As DevExpress.XtraEditors.SimpleButton = CType(sender, DevExpress.XtraEditors.SimpleButton)
			Return CInt(btn.Tag) = 1
		End Get
	End Property

	''' <summary>
	''' Handles drop down button clicks.
	''' </summary>
	Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

		Const ID_OF_HIDESHOW_BUTTON As Int32 = 1

		' If hide/show button has been clicked reset the drop down.
		If e.Button.Index = ID_OF_HIDESHOW_BUTTON Then

			If e.Button.Tag = 0 Then
				e.Button.Image = My.Resources.hide_16x16
				e.Button.Tag = 1

			Else
				e.Button.Image = My.Resources.show_16x16
				e.Button.Tag = 0

			End If

		End If

	End Sub

	Private Sub OnHideShow_ButtonClick(sender As Object, e As EventArgs)

		' If hide/show button has been clicked reset the drop down.
		Dim btn As DevExpress.XtraEditors.SimpleButton = CType(sender, DevExpress.XtraEditors.SimpleButton)

		If btn.Tag = 0 Then
			btn.Image = My.Resources.hide_16x16
			btn.Tag = 1

		Else
			btn.Image = My.Resources.show_16x16
			btn.Tag = 0

		End If

	End Sub



#Region "Dropdown Funktionen 1. Seite..."

	Private Sub Cbo_KDKanton_QueryPopUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDKanton.QueryPopUp
		ListKDKanton(Me.Cbo_KDKanton)
	End Sub

	Private Sub Cbo_KDBerater_QueryPopUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDBerater.QueryPopUp
		ListBerater(Cbo_KDBerater)
	End Sub


	Private Sub Cbo_KDKontakt_QueryPopUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDKontakt.QueryPopUp
		ListKDKontakt(Cbo_KDKontakt)
	End Sub

	Private Sub Cbo_KD1Res_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KD1Res.QueryPopUp
		ListKDRes(Me.Cbo_KD1Res, 1)
	End Sub

	Private Sub Cbo_KD2Res_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KD2Res.QueryPopUp
		ListKDRes(Me.Cbo_KD2Res, 2)
	End Sub

	Private Sub Cbo_KD3Res_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KD3Res.QueryPopUp
		ListKDRes(Me.Cbo_KD3Res, 3)
	End Sub

	Private Sub Cbo_KD4Res_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KD4Res.QueryPopUp
		ListKDRes(Me.Cbo_KD4Res, 4)
	End Sub

	Private Sub Cbo_KD1Stat_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KD1Stat.QueryPopUp
		ListKDStat(Me.Cbo_KD1Stat, 1)
	End Sub

	Private Sub Cbo_KDZKond_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDZKond.QueryPopUp
		ListKDZKond(Me.Cbo_KDZKond)
	End Sub

	Private Sub Cbo_KD2Stat_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KD2Stat.QueryPopUp
		ListKDStat(Me.Cbo_KD2Stat, 2)
	End Sub

	Private Sub Cbo_KDBCount_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDBCount.QueryPopUp
		ListKDBCount(Me.Cbo_KDBCount)
	End Sub

	Private Sub Cbo_ESStop_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ESStop.QueryPopUp
		ListForActivate(Me.Cbo_ESStop)
	End Sub

	Private Sub Cbo_WWW_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_WWW.QueryPopUp
		ListForActivate(Me.Cbo_WWW)
	End Sub

	Private Sub Cbo_Transport_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Transport.QueryPopUp
		ListForActivate(Me.Cbo_Transport)
	End Sub

	Private Sub Cbo_Kantine_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Kantine.QueryPopUp
		ListForActivate(Me.Cbo_Kantine)
	End Sub

#End Region


#Region "Lib Clicks 2. Seite..."

	Private Sub OnbtnAddKDBerufe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddKDBerufe.Click
		Dim frmTest As New frmSearchRec("kdjob")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_KDBerufe.Items.Count - 1
				If InStr(Me.Lst_KDBerufe.Items(i).ToString, m.ToString) > 0 Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_KDBerufe.Items.Add(strEintrag(i))
				Next
			End If
		End If

		frmTest.Dispose()

	End Sub

	Private Sub OnbtnAddKDBranchen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddKDBranchen.Click
		Dim frmTest As New frmSearchRec("kdbranches")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_KDBranche.Items.Count - 1
				If Me.Lst_KDBranche.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_KDBranche.Items.Add(strEintrag(i))
				Next
			End If
		End If
		frmTest.Dispose()

	End Sub

	Private Sub OnbtnAddKDStichwort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddKDStichwort.Click
		Dim frmTest As New frmSearchRec("kdstichwort")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_KDStichwort.Items.Count - 1
				If Me.Lst_KDStichwort.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i
			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_KDStichwort.Items.Add(strEintrag(i))
				Next
			End If
		End If

		frmTest.Dispose()

	End Sub

	Private Sub OnbtnAddKDGAV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddKDGAV.Click
		Dim frmTest As New frmSearchRec("kdgav")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_KDGAV.Items.Count - 1
				If InStr(Me.Lst_KDGAV.Items(i).ToString, m.ToString) > 0 Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_KDGAV.Items.Add(strEintrag(i))
				Next
			End If
		End If

		frmTest.Dispose()

	End Sub

	Private Sub OnbtnAddKDFiliale_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddKDFiliale.Click
		Dim frmTest As New frmSearchRec("kdFiliale")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_KDFiliale.Items.Count - 1
				If Me.Lst_KDFiliale.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_KDFiliale.Items.Add(strEintrag(i))
				Next
			End If
		End If

		frmTest.Dispose()

	End Sub

	Private Sub OnbtnAddKDAnstellung_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddKDAnstellung.Click
		Dim frmTest As New frmSearchRec("kdAnstellung")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_KDAnstellung.Items.Count - 1
				If Me.Lst_KDAnstellung.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_KDAnstellung.Items.Add(strEintrag(i))
				Next
			End If
		End If

		frmTest.Dispose()

	End Sub

#End Region


#Region "DropDown Funktionen 2. Seite"

	Private Sub Cbo_KDCurrency_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDCurrency.QueryPopUp
		ListKDCurrency(Me.Cbo_KDCurrency)
	End Sub

	Private Sub Cbo_KDMahn_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDMahn.QueryPopUp
		ListKDMahnCode(Me.Cbo_KDMahn)
	End Sub

	Private Sub Cbo_KDFaktura_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDFaktura.QueryPopUp
		ListKDFakturaCode(Me.Cbo_KDFaktura)
	End Sub

	Private Sub Cbo_KD1KLimite_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KD1KLimite.QueryPopUp
		ListKD1KLimite(Me.Cbo_KD1KLimite)
	End Sub

	Private Sub Cbo_KD2KLimite_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KD2KLimite.QueryPopUp
		ListKD2KLimite(Me.Cbo_KD2KLimite)
	End Sub

	Private Sub OnKDVerguetung_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbo_KDVerguetung.QueryPopUp
		ListKDVerguetung(Me.cbo_KDVerguetung)
	End Sub

	Private Sub Cbo_MwSt_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_MwSt.QueryPopUp
		ListForActivate(Me.Cbo_MwSt)
	End Sub

	Private Sub Cbo_RPPrint_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_RPPrint.QueryPopUp
		ListForActivate(Me.Cbo_RPPrint)
	End Sub

	Private Sub Cbo_1Kredit_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_1Kredit.QueryPopUp
		ListForActivate(Me.Cbo_1Kredit)
	End Sub

	Private Sub Cbo_ES_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ES.QueryPopUp
		ListForActivate(Me.Cbo_ES)
	End Sub

	Private Sub Cbo_KDWithOP_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDWithOP.QueryPopUp
		ListKDWithOP(Me.Cbo_KDWithOP)
	End Sub



#End Region


#Region "Lib clicks 3. Seite..."

	Private Sub OnbtnAddZKom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddZKom.Click
		Dim frmTest As New frmSearchRec("zhdkomm")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_ZKom.Items.Count - 1
				If Me.Lst_ZKom.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_ZKom.Items.Add(strEintrag(i))
				Next
			End If
		End If
		frmTest.Dispose()

	End Sub

	Private Sub OnbtnAddZVersand_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddZVersand.Click
		Dim frmTest As New frmSearchRec("zhdversand")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_ZVersand.Items.Count - 1
				If Me.Lst_ZVersand.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_ZVersand.Items.Add(strEintrag(i))
				Next
			End If
		End If
		frmTest.Dispose()

	End Sub

#End Region


#Region "DropDown Funktionen 3. Seite..."

	Private Sub Cbo_ZBerater_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ZBerater.QueryPopUp
		ListZBerater(Cbo_ZBerater)
	End Sub

	Private Sub Cbo_ZPosition_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ZPosition.QueryPopUp
		ListZPosition(Cbo_ZPosition)
	End Sub

	Private Sub Cbo_ZMGeb_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ZMGeb.QueryPopUp
		ListZGebMonth(Cbo_ZMGeb)
	End Sub

	Private Sub Cbo_ZKontakt_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ZKontakt.QueryPopUp
		ListZKontakt(Cbo_ZKontakt)
	End Sub

	Private Sub Cbo_ZAbteilung_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ZAbteilung.QueryPopUp
		ListZAbteilung(Cbo_ZAbteilung)
	End Sub

	Private Sub Cbo_Z1Stat_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Z1Stat.QueryPopUp
		ListZStat(Cbo_Z1Stat, 1)
	End Sub

	Private Sub Cbo_Z2Stat_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_Z2Stat.QueryPopUp
		ListZStat(Cbo_Z2Stat, 2)
	End Sub

	Private Sub Cbo_ZKontaktTyp_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cbo_ZKontaktTyp.QueryPopUp
		ListZKontaktTyp(Cbo_ZKontaktTyp)
	End Sub

	Private Sub Cbo_ZKontaktDown_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ZKontaktDown.QueryPopUp
		ListForActivate(Me.Cbo_ZKontaktDown)
	End Sub

	Private Sub Cbo_ZKontaktFrom_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ZKontaktFrom.QueryPopUp
		ListZKontaktFrom(Me.Cbo_ZKontaktFrom)
	End Sub


#End Region


#Region "Lib clicks 4. Seite..."

	Private Sub OnbtnAddZBerufe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddZBerufe.Click

		Dim frmTest As New frmSearchRec("zhdjob")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_ZBerufe.Items.Count - 1
				If Me.Lst_ZBerufe.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_ZBerufe.Items.Add(strEintrag(i))
				Next
			End If
		End If

		frmTest.Dispose()

	End Sub

	Private Sub OnbtnAddZBranche_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddZBranche.Click
		Dim frmTest As New frmSearchRec("zhdbranches")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_ZBranche.Items.Count - 1
				If Me.Lst_ZBranche.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_ZBranche.Items.Add(strEintrag(i))
				Next
			End If
		End If

		frmTest.Dispose()

	End Sub

	Private Sub OnbtnAddZRes1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddZRes1.Click

		Dim frmTest As New frmSearchRec("zhdres1")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_Z1Res.Items.Count - 1
				If Me.Lst_Z1Res.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_Z1Res.Items.Add(strEintrag(i))
				Next
			End If
		End If
		frmTest.Dispose()

	End Sub

	Private Sub OnbtnAddZRes2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddZRes2.Click
		Dim frmTest As New frmSearchRec("zhdres2")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_Z2Res.Items.Count - 1
				If Me.Lst_Z2Res.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_Z2Res.Items.Add(strEintrag(i))
				Next
			End If
		End If
		frmTest.Dispose()

	End Sub

	Private Sub OnbtnAddZRes3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddZRes3.Click

		Dim frmTest As New frmSearchRec("zhdres3")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_Z3Res.Items.Count - 1
				If Me.Lst_Z3Res.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_Z3Res.Items.Add(strEintrag(i))
				Next
			End If
		End If
		frmTest.Dispose()

	End Sub

	Private Sub OnbtnAddZRes4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddZRes4.Click

		Dim frmTest As New frmSearchRec("zhdres4")
		Dim i As Integer = 0
		Dim bInsert2Lst As Boolean

		Dim m As String
		frmTest.ShowDialog()
		frmTest.MdiParent = Me.MdiParent

		m = frmTest.iKDValue()
		If m.ToString <> String.Empty Then
			bInsert2Lst = True
			For i = 0 To Me.Lst_Z4Res.Items.Count - 1
				If Me.Lst_Z4Res.Items(i).ToString = m.ToString Then
					bInsert2Lst = False
					Exit For
				End If
			Next i

			Dim strEintrag As String() = Regex.Split(m.ToString, "#@")
			If bInsert2Lst Then
				For i = 0 To strEintrag.Count - 1
					Lst_Z4Res.Items.Add(strEintrag(i))
				Next
			End If
		End If
		frmTest.Dispose()

	End Sub

#End Region


	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Dispose()
	End Sub

	Private Sub frmKDSearch_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		FormIsLoaded("frmKDSearch_LV", True)

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frm_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmHeight = Me.Height
				My.Settings.ifrmWidth = Me.Width

				My.Settings.SortBez = Me.CboSort.Text

				My.Settings.bOpen_P_1_1 = b1_1
				My.Settings.bOpen_P_1_2 = b1_2
				My.Settings.bOpen_P_1_3 = b1_3

				My.Settings.bOpen_P_2_1 = b2_1
				My.Settings.bOpen_P_2_2 = b2_2
				My.Settings.bOpen_P_2_3 = b2_3

				My.Settings.bOpen_P_3_1 = b3_1
				My.Settings.bOpen_P_3_2 = b3_2
				My.Settings.bOpen_P_3_3 = b3_3

				My.Settings.bOpen_P_4_2 = b4_2

				My.Settings.Save()
			End If


		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub

	Private Sub frmKDSearch_Move(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Move
		If FormIsLoaded("frmKDSearch_LV", False) Then
			frmMyLV.Location = New Point(Me.Location.X - frmMyLV.Width - 5, Me.Location.Y)
			frmMyLV.TopMost = True
			frmMyLV.TopMost = False
		End If
	End Sub

	Private Sub frm_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

		If e.KeyCode = Keys.F12 And ClsDataDetail.ProgSettingData.LogedUSNr = 1 Then
			Dim frm As New frmLibraryInfo
			frm.LoadAssemblyData()

			frm.Show()
			frm.BringToFront()
		End If

	End Sub

	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Dim Time_1 As Double = System.Environment.TickCount
		For i As Integer = 0 To Me.xtabKDSearch.TabPages.Count - 1
			Me.TranslatedPage.Add(False)
		Next

		m_xml.GetChildChildBez(Me.xtabAllgemein)
		m_xml.GetChildChildBez(Me.PanelControl1)
		Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue(Me.bsiInfo.Caption)
		Me.bbiSearch.Caption = m_Translate.GetSafeTranslationValue(Me.bbiSearch.Caption)
		Me.bbiClear.Caption = m_Translate.GetSafeTranslationValue(Me.bbiClear.Caption)
		Me.bbiPrint.Caption = m_Translate.GetSafeTranslationValue(Me.bbiPrint.Caption)
		Me.bbiExport.Caption = m_Translate.GetSafeTranslationValue(Me.bbiExport.Caption)

		For Each tbp As DevExpress.XtraTab.XtraTabPage In Me.xtabKDSearch.TabPages
			tbp.Text = m_Translate.GetSafeTranslationValue(tbp.Text)
		Next

		TranslatedPage(0) = True

		Me.Cbo_KD1Res.Visible = Not String.IsNullOrWhiteSpace(lbl1Res.Text)
		Me.Cbo_KD2Res.Visible = Not String.IsNullOrWhiteSpace(lbl2Res.Text)
		Me.Cbo_KD3Res.Visible = Not String.IsNullOrWhiteSpace(lbl3Res.Text)
		Me.Cbo_KD4Res.Visible = Not String.IsNullOrWhiteSpace(lbl4Res.Text)
		Me.grpKDRes.Visible = Not String.IsNullOrWhiteSpace(lbl1Res.Text & lbl2Res.Text & lbl3Res.Text & lbl4Res.Text)

		Me.Lst_Z1Res.Visible = Not String.IsNullOrWhiteSpace(lblzhdres1.Text)
		Me.Lst_Z2Res.Visible = Not String.IsNullOrWhiteSpace(lblzhdres2.Text)
		Me.Lst_Z3Res.Visible = Not String.IsNullOrWhiteSpace(lblzhdres3.Text)
		Me.Lst_Z4Res.Visible = Not String.IsNullOrWhiteSpace(lblzhdres4.Text)
		Me.lblzhdreservefelder.Visible = Not String.IsNullOrWhiteSpace(lblzhdres1.Text & lblzhdres2.Text & lblzhdres3.Text & lblzhdres4.Text)

	End Sub

	''' <summary>
	''' Starten von Anwendung.
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub frmKDSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim Time_1 As Double = System.Environment.TickCount

		SetDefaultSortValues()
		Dim UpdateDelegate As New MethodInvoker(AddressOf StartTranslation)
		Me.Invoke(UpdateDelegate)

		Try
			luFProperty.Properties.NullText = String.Empty
			Me.KeyPreview = True
			Dim strStyleName As String = m_md.GetSelectedUILayoutName(ClsDataDetail.ProgSettingData.SelectedMDNr, 0, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Formstyle. {1}", strMethodeName, ex.Message))

		End Try

		Try
			Me.Width = Math.Max(My.Settings.ifrmWidth, Me.Width)
			Me.Height = Math.Max(My.Settings.ifrmHeight, Me.Height)
			If My.Settings.frm_Location <> String.Empty Then
				Dim aLoc As String() = My.Settings.frm_Location.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = 0
				End If
				Me.Location = New System.Drawing.Point(Math.Max(Val(aLoc(0)), 0), Math.Max(Val(aLoc(1)), 0))
			End If

		Catch ex As Exception

		End Try

		Me.bbiExport.Enabled = False
		Me.bbiPrint.Enabled = False

		' Berechtigung Mitarbeiterliste exportieren
		Dim _ClsDb As New ClsDbFunc
		_ClsDb.GetJobNr4Print(Me, 0)
		If Not IsUserAllowed4DocExport(ClsDataDetail.GetModulToPrint()) Then
			Me.bbiExport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
		End If

		ClsDataDetail.IsFirstTapiCall = True

		FillDefaultValues()

		If ClsDataDetail.ProgSettingData.LogedUSNr <> 1 Then
			Me.xtabKDSearch.TabPages.Remove(Me.xtabSQLAbfrage)
			Me.xtabKDSearch.TabPages.Remove(Me.xtabErweitert)
		End If

	End Sub

	Sub SetDefaultSortValues()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Try
				Dim strSort As String = My.Settings.SortBez
				If strSort = String.Empty Then strSort = String.Format("1 - {0}", m_Translate.GetSafeTranslationValue("Kundenname"))
				ListSort(Me.CboSort)
				Dim aSortlist As String() = strSort.Split(CChar(","))

				For Each bez As String In aSortlist
					CboSort.Properties.Items(CInt(Val(bez))).CheckState = CheckState.Checked
				Next

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Fill Sortinglist: {1}", strMethodeName, ex.Message))

			End Try

			Try
				Me.lueMandant.SetEditValue(ClsDataDetail.ProgSettingData.SelectedMDNr)
				Dim showMDSelection As Boolean = IsUserActionAllowed(ClsDataDetail.ProgSettingData.LogedUSNr, 642, ClsDataDetail.ProgSettingData.SelectedMDNr)
				Me.lueMandant.Visible = showMDSelection
				Me.lblMDName.Visible = showMDSelection

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}.Mandantenauswahl anzeigen: {1}", strMethodeName, ex.Message))
				Me.lueMandant.Visible = False
				Me.lblMDName.Visible = False
			End Try

			b1_1 = My.Settings.bOpen_P_1_1
			b1_2 = My.Settings.bOpen_P_1_2
			b1_3 = My.Settings.bOpen_P_1_3

			b2_1 = My.Settings.bOpen_P_2_1
			b2_2 = My.Settings.bOpen_P_2_2
			b2_3 = My.Settings.bOpen_P_2_3

			b3_1 = My.Settings.bOpen_P_3_1
			b3_2 = My.Settings.bOpen_P_3_2
			b3_3 = My.Settings.bOpen_P_3_3

			Me.lblzhdreservefelder.Text = Me.lblzhdreservefelder.Text.PadRight(Me.p4_2.Width)
			Me.lblzhdreservefelder.Width = Me.p4_2.Width

			Dim strQuery As String = m_xml.GetColorValue()
			If strQuery.Contains(";") Then
				Dim aColor As String() = strQuery.Split(";")
				Me.lblzhdreservefelder.ForeColor = Color.FromArgb(aColor(0), aColor(1), aColor(2))

			Else
				Me.lblzhdreservefelder.ForeColor = Color.FromName(strQuery)

			End If

			p4_2.Height = Me.lblzhdreservefelder.Height + If(b4_2, (Me.Lst_Z3Res.Top + Me.Lst_Z3Res.Height + 10), 0)

			Me.LibKDNr.TabStop = False
			Me.LibFirma1.TabStop = False
			Me.xtabKDSearch.SelectedTabPage = Me.xtabAllgemein

			AddHandler Me.lueMandant.EditValueChanged, AddressOf OnlueMandant_EditValueChanged

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.Definition der Sortierreihenfolge. {1}", strMethodeName, ex.Message))

		End Try

	End Sub

	Private Sub xtabKDSearch_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles xtabKDSearch.SelectedPageChanged

		If Not TranslatedPage(xtabKDSearch.SelectedTabPageIndex) Then
			m_xml.GetChildChildBez(xtabKDSearch.SelectedTabPage)
			TranslatedPage(xtabKDSearch.SelectedTabPageIndex) = True

		End If

	End Sub

	Sub _EnumFormControls(ByVal cControl As ControlCollection)
		Dim ctrl As Control

		For Each ctrl In cControl
			If ctrl.Controls.Count > 0 Then
				Trace.WriteLine(String.Format("_EnumFormControls: Typeof: {0}",
																			ctrl.ToString))
				_EnumFormControls(ctrl.Controls)
			End If
		Next
	End Sub


	''' <summary>
	''' Default-Werte in den Felder Monat und Jahr einfügen.
	''' </summary>
	''' <remarks></remarks>
	Sub FillDefaultValues()
		ListKontaktArten(Me.cbo_KDKontaktArten)
	End Sub

	''' <summary>
	''' Daten fürs Drucken bereit stellen.
	''' </summary>
	Sub GetKDData4Print(ByVal bForExport As Boolean, ByVal strJobInfo As String)
		Dim iKDNr As Integer = 0
		Dim iKDZNr As Integer = 0
		Dim bResult As Boolean = True
		Dim bWithKD As Boolean = True
		Dim msg As String = String.Empty

		Dim sSql As String = Me.ShortSQLQuery

		' WENN DIE KONTAKTLISTE GEDRUCKT WIRD, SO BESTEHENDE DATEN NEU AUFBEREITEN
		If strJobInfo = "2.3.3" Then
			sSql = "Begin Try Drop Table #tblKDZustaendig End Try Begin Catch End Catch "
			sSql = "SELECT * INTO #tblKDZustaendig FROM KD_Zustaendig "
			sSql += " WHERE "
			sSql += String.Format("RecNr In (SELECT ZHDRecNr FROM {0} GROUP BY ZHDRecNr) ", ClsDataDetail.LLTablename)
			'sSql += String.Format("RecNr In (SELECT KDZNR FROM {0} GROUP BY KDZNR) ", ClsDataDetail.LLTablename)

			sSql += "SELECT ZHDKontaktRecNr, Kundenliste.KDNR, Kundenliste.Firma1, KDPLZ, KDOrt, "
			sSql += "ZHDKontaktDateString, ZHDKontaktTimeString, IsNull(#tblKDZustaendig.Nachname, '-') As Nachname, "
			sSql += "IsNull(#tblKDZustaendig.Vorname, '-') As Vorname, ZHDKontaktType1, ZHDKontaktDauer, "
			sSql += "ZHDKontakte "
			sSql += String.Format("FROM {0} Kundenliste ", ClsDataDetail.LLTablename)
			sSql += "LEFT JOIN #tblKDZustaendig ON "
			sSql += "#tblKDZustaendig.RecNr = ZHDRecNr "
			'sSql += "#tblKDZustaendig.RecNr = KDZNr "

			sSql += String.Format("ORDER BY {0} Convert(Datetime,ZHDKontaktDateString,104) DESC, ZHDKontaktTimeString DESC",
														If(ClsDataDetail.LLOrderByStringKontaktliste.Length > 0,
															 ClsDataDetail.LLOrderByStringKontaktliste & ", ", ""))
		End If

		If sSql = String.Empty Then
			msg = m_Translate.GetSafeTranslationValue("Keine Suche wurde gestartet!")
			m_UtilityUI.ShowErrorDialog(msg)

			Return
		End If

		Try

			Me.SQL4Print = sSql
			Me.PrintJobNr = strJobInfo

			StartPrinting()

		Catch ex As Exception
			m_UtilityUI.ShowErrorDialog(ex.ToString)

		End Try

	End Sub

	Sub StartPrinting()
		Dim bShowDesign As Boolean = IsUserActionAllowed(0, 612)
		Dim ShowDesign As Boolean = (My.Computer.Keyboard.CtrlKeyDown AndAlso My.Computer.Keyboard.ShiftKeyDown) AndAlso bShowDesign

		Dim _Setting As New SPS.Listing.Print.Utility.ClsLLKDSearchPrintSetting With {.DbConnString2Open = ClsDataDetail.GetSelectedMDConnstring,
																																							 .SQL2Open = Me.SQL4Print,
																																							 .JobNr2Print = Me.PrintJobNr,
																																									.SelectedMDNr = ClsDataDetail.ProgSettingData.SelectedMDNr,
																																									.LogedUSNr = ClsDataDetail.UserData.UserNr}
		Dim obj As New SPS.Listing.Print.Utility.KDSearchListing.ClsPrintKDSearchList(_Setting)
		obj.PrintKDSearchList_1(ShowDesign, ClsDataDetail.GetSortBez,
														New List(Of String)(New String() {ClsDataDetail.GetFilterBez,
																															ClsDataDetail.GetFilterBez2,
																															ClsDataDetail.GetFilterBez3,
																															ClsDataDetail.GetFilterBez4}))
	End Sub

#Region "Funktionen zur Menüaufbau..."

	Private Sub bbiSearch_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSearch.ItemClick
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim bBuildAsVirtual As Boolean = CBool(Me.txt_SQLQuery.Text.Trim = String.Empty)

		Try
			Me.txt_SQLQuery.Text = String.Empty
			Me.ShortSQLQuery = String.Empty
			If Not Me.txtKDNr_2.Visible Then Me.txtKDNr_2.Text = Me.txtKDNr_1.Text
			If Not Me.txtKDName_2.Visible Then Me.txtKDName_2.Text = Me.txtKDName_1.Text

			Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Nach Daten wird gesucht...")

			FormIsLoaded("frmKDSearch_LV", True)

			' Die Query-String aufbauen...
			GetMyQueryString()

			' Daten auflisten...
			If Not FormIsLoaded("frmKDSearch_LV", True) Then
				Dim sqlQSTListe As String = String.Empty
				If Me.txt_IndSQLQuery.Text = String.Empty Then
					sqlQSTListe = Me.txt_SQLQuery.Text ' String.Format(" SELECT * FROM {0} ", ClsDataDetail.LLTablename)
				Else
					sqlQSTListe = Me.txt_IndSQLQuery.Text
				End If
				frmMyLV = New frmKDSearch_LV(sqlQSTListe)

				frmMyLV.Show()
				Me.Select()
			End If

			Me.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																					 frmMyLV.RecCount)
			frmMyLV.bsiInfo.Caption = String.Format(m_Translate.GetSafeTranslationValue("{0} Datensätze wurden aufgelistet") & "...",
																								frmMyLV.RecCount)

			' Buttons Drucken und Export aktivieren/deaktivieren
			If frmMyLV.RecCount > 0 Then
				CreatePrintPopupMenu()
				CreateExportPopupMenu()

				Me.bbiPrint.Enabled = True
				Me.bbiExport.Enabled = True

			Else
				Me.bbiPrint.Enabled = False
				Me.bbiExport.Enabled = False

			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "btnSearch_Click")

		Finally

		End Try

	End Sub

	Function GetMyQueryString() As Boolean
		Dim sSql1Query As String = String.Empty
		Dim sSql2Query As String = String.Empty
		Dim strSort As String = String.Empty
		Dim strArtQuery As String = String.Empty

		Dim _ClsDb As New ClsDbFunc

		Dim result = lueMandant.Properties.GetItems.GetCheckedValues()
		For Each itm In result
			If CInt(Val(itm.ToString)) > 0 Then _ClsDb.mandantNumber.Add(CInt(itm.ToString))
		Next

		If Me.txt_IndSQLQuery.Text = String.Empty Then
			_ClsDb.luColor = Me.luFProperty

			sSql1Query = _ClsDb.GetStartSQLString(Me)
			sSql2Query = _ClsDb.GetQuerySQLString(sSql1Query, Me)

			If Trim(sSql2Query) <> String.Empty Then
				sSql1Query += " Where "
			End If
			strSort = _ClsDb.GetSortString(Me)

			Me.txt_SQLQuery.Text = sSql1Query + sSql2Query & " "


			Me.txt_SQLQuery.Text &= String.Format("SELECT * FROM {0} {1}",
																					 ClsDataDetail.LLTablename, strSort)

			Me.ShortSQLQuery = String.Format("SELECT * FROM {0} {1}", ClsDataDetail.LLTablename, strSort)

		Else
			ClsDataDetail.LLTablename = String.Format("_Kundenliste_{0}", ClsDataDetail.ProgSettingData.LogedUSNr)
			Me.txt_SQLQuery.Text = Me.txt_IndSQLQuery.Text
			Me.ShortSQLQuery = Me.txt_SQLQuery.Text

		End If

		Return True
	End Function


	Private Sub bbiPrint_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiPrint.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiPrint.DropDownControl

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreatePrintPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim bShowDesign As Boolean = IsUserActionAllowed(0, 612)

		Dim bShowContactmnu As Boolean = Me.de_ZKontakt_1.Text + Me.de_ZKontakt_2.Text +
					Me.txt_ZKontaktBez.Text +
					Me.Cbo_ZKontaktTyp.Text +
					Me.Cbo_ZKontaktDown.Text +
					Me.Cbo_ZKontaktFrom.Text <> ""

		Dim liMnu As New List(Of String) From {"Kundenliste ohne ZHD#mnuKDListWithoutZHD",
																					 "Kundenliste mit ZHD#mnuKDListwithZHD",
																					 If(bShowContactmnu, "-Liste der Kontakte#mnukdListKontaktliste", ""),
																					 If(bShowContactmnu, "", "-") & "Liste der Kreditlimite#mnukdlistkreditlimite"}
		', _
		'																			 If(bShowDesign, "Entwurfanssicht ohne ZHD#mnuDesignWithoutZHD", ""),
		'																			 If(bShowDesign, "Entwurfansicht mit ZHD#mnuDesignwithZHD", ""),
		'																			 If(bShowDesign And bShowContactmnu, "Entwurfansicht der Kontaktliste#mnuDesignKontaktliste", ""),
		'																			 If(bShowDesign, "Entwurfansicht der Kreditlimite#mnuDesignkreditlimite", "")}
		If Not IsUserActionAllowed(0, 602) Then Exit Sub
		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()

			Me.bbiPrint.ActAsDropDown = False
			Me.bbiPrint.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiPrint.DropDownEnabled = True
			Me.bbiPrint.DropDownControl = popupMenu
			Me.bbiPrint.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				Dim captionLbl = myValue(0).ToString
				bshowMnu = Not String.IsNullOrWhiteSpace(captionLbl)

				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					If captionLbl.StartsWith("-") Then captionLbl = captionLbl.Substring(1, captionLbl.Length - 1)
					captionLbl = m_Translate.GetSafeTranslationValue(captionLbl)
					itm.Caption = captionLbl

					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)
					If myValue(0).ToString.ToLower.StartsWith("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)

					AddHandler itm.ItemClick, AddressOf GetPrintMenuItem
				End If

			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetPrintMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim _clsdb As New ClsDbFunc

		Me.SQL4Print = String.Empty
		Me.bPrintAsDesign = False

		Dim strModultoPrint As String = ClsDataDetail.GetModulToPrint
		Select Case e.Item.Name.ToUpper
			Case "mnuKDListWithoutZHD".ToUpper
				_clsdb.GetJobNr4Print(Me, 0)
				GetKDData4Print(False, ClsDataDetail.GetModulToPrint())

			Case "mnuKDListWithZHD".ToUpper
				_clsdb.GetJobNr4Print(Me, 1)
				GetKDData4Print(False, ClsDataDetail.GetModulToPrint())

			Case "mnukdListKontaktliste".ToUpper
				_clsdb.GetJobNr4Print(Me, 2)
				GetKDData4Print(False, ClsDataDetail.GetModulToPrint())

			Case "mnukdlistkreditlimite".ToUpper
				_clsdb.GetJobNr4Print(Me, 3)
				GetKDData4Print(False, ClsDataDetail.GetModulToPrint())


				'Case "mnuDesignWithoutZHD".ToUpper
				'	_clsdb.GetJobNr4Print(Me, 0)
				'	GetKDData4Print(True, False, ClsDataDetail.GetModulToPrint())

				'Case "mnuDesignwithZHD".ToUpper
				'	_clsdb.GetJobNr4Print(Me, 1)
				'	GetKDData4Print(True, False, ClsDataDetail.GetModulToPrint())

				'Case "mnuDesignKontaktliste".ToUpper
				'	_clsdb.GetJobNr4Print(Me, 2)
				'	GetKDData4Print(True, False, ClsDataDetail.GetModulToPrint())

				'Case "mnuDesignkreditlimite".ToUpper
				'	_clsdb.GetJobNr4Print(Me, 3)
				'	GetKDData4Print(True, False, ClsDataDetail.GetModulToPrint())


			Case Else
				Exit Sub

		End Select

	End Sub

	Private Sub bbiExport_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExport.ItemClick
		Dim popupMenu As DevExpress.XtraBars.PopupMenu = Me.bbiExport.DropDownControl

		If Not (popupMenu Is Nothing) Then popupMenu.ShowPopup(New Point(x:=MousePosition.X, y:=MousePosition.Y))

	End Sub

	Private Sub CreateExportPopupMenu()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
		Dim bshowMnu As Boolean = True
		Dim popupMenu As New DevExpress.XtraBars.PopupMenu
		Dim liMnu As List(Of String) = GetMenuItems4Export()

		Try
			bbiPrint.Manager = Me.BarManager1
			BarManager1.ForceInitialize()
			Me.bbiExport.ActAsDropDown = False
			Me.bbiExport.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown
			Me.bbiExport.DropDownEnabled = True
			Me.bbiExport.DropDownControl = popupMenu
			'Me.bbiExport.Visibility = BarItemVisibility.Always
			Me.bbiExport.Enabled = True

			For i As Integer = 0 To liMnu.Count - 1
				Dim myValue As String() = liMnu(i).Split(CChar("#"))
				If bshowMnu Then
					popupMenu.Manager = BarManager1

					Dim itm As New DevExpress.XtraBars.BarButtonItem
					itm.Caption = m_Translate.GetSafeTranslationValue(myValue(0).ToString)
					itm.Name = m_Translate.GetSafeTranslationValue(myValue(1).ToString)
					itm.AccessibleName = myValue(2).ToString
					If myValue(0).ToString.ToLower.Contains("-".ToLower) Then popupMenu.AddItem(itm).BeginGroup = True Else popupMenu.AddItem(itm)
					AddHandler itm.ItemClick, AddressOf GetExportMenuItem
				End If
			Next

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}.{1}", strMethodeName, ex.Message))

		End Try
	End Sub

	Sub GetExportMenuItem(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
		Dim strSQL As String = Me.ShortSQLQuery

		Select Case UCase(e.Item.Name.ToUpper)
			Case UCase("Offers-Email")
				Dim frmMailing As New SPSOfferUtility_Net.frmOfferSelect(m_InitialData)
				'ProgObj.ShowMainForm(strSQL)



				Dim strTestSql As String
				Dim strBeginTrySql As String
				strBeginTrySql = "BEGIN TRY DROP TABLE #KD_Mailing END TRY BEGIN CATCH END CATCH"

				strTestSql = String.Format("{0} SELECT * {1} FROM _Kundenliste_{2} ",
															 strBeginTrySql,
															 "Into #KD_Mailing",
															 m_InitialData.UserData.UserNr)
				strTestSql &= "Declare @Anzrec int "
				strTestSql &= "Set @Anzrec = (select COUNT(*) As Anz from #KD_Mailing) "
				strTestSql &= "Select @Anzrec As Anzrec, * From #KD_Mailing"

				frmMailing.m_GetSearchQuery = strTestSql
				If frmMailing.LoadData() Then
					frmMailing.Show()
					frmMailing.BringToFront()
				End If




			Case UCase("MULTI-Db")
				Dim obj As New ThreadTesting.OpenFormsWithThreading(strSQL)
				obj._OpenBewForm(strSQL)

			Case UCase("MAIL")
				Call RunMailModul(strSQL)


			Case UCase("Contact")
				Call ExportDataToOutlook(strSQL)

			Case UCase("TXT"), UCase("CSV")
				'Dim ExportThread As New Thread(AddressOf StartExportModul)
				'ExportThread.Name = "ExportKDListing"
				'ExportThread.Start()
				StartExportModul()

			Case UCase("FAX")
				Call RunTobitFaxModul(strSQL)

			Case UCase("eCall-Fax")
				Call RuneCallFaxModul(strSQL)

			Case UCase("eCall-SMS")
				Dim sql As String
				sql = "SELECT DISTINCT(KD.KDNr), KD.zhdrecnr, KD.Firma1, KD.KDStrasse Strasse, KD.KDLand Land, KD.KDPLZ PLZ, KD.KDOrt Ort, "
				sql &= "KD.Anredeform AS Anredeform, KD.Nachname, KD.Vorname, "
				sql &= "( "
				sql &= "Case KD.zhd_SMS_Mailing "
				sql &= "When 0 Then kd.zhdNatel Else '' End) As Natel "
				sql &= "FROM _Kundenliste_{0} KD "
				sql &= "WHERE (KD.zhdNatel <> '' And KD.zhdNatel Is Not Null ) And KD.ZHD_SMS_Mailing <> 1 "
				sql &= "Order by  KD.Firma1"

				sql = String.Format(sql, ClsDataDetail.UserData.UserNr)
				Call RuneCallSMSModul(sql)

			Case Else
				Return


		End Select

	End Sub

	Sub StartExportModul()
		Dim _Setting As New SPS.Export.Listing.Utility.ClsCSVSettings With {.DbConnString2Open = ClsDataDetail.GetSelectedMDConnstring,
																																							 .SQL2Open = Me.ShortSQLQuery,
																																							 .ModulName = "KDSearch",
																																				.SelectedMDNr = ClsDataDetail.MDData.MDNr,
																																				.LogedUSNr = ClsDataDetail.UserData.UserNr}
		Dim obj As New SPS.Export.Listing.Utility.ClsExportStart(_Setting)
		obj.ExportCSVFromKDSearchListing(Me.txt_SQLQuery.Text)

	End Sub




	''' <summary>
	''' Funktion für das Leeren der Felder...
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub bbiClear_ItemClick(sender As System.Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiClear.ItemClick

		FormIsLoaded("frmKDSearch_LV", True)
		Me.bsiInfo.Caption = m_Translate.GetSafeTranslationValue("Bereit")

		ResetAllTabEntries()
		FillDefaultValues()

		' Die Buttons Drucken und Export deaktivieren
		Me.bbiPrint.Enabled = False
		Me.bbiExport.Enabled = False

		Me.bbiSearch.Enabled = True

		' Checkbox defaults
		Me.chkKDKreditlimiteUeberschritten.Checked = False

		Me.txt_SQLQuery.Text = m_Translate.GetSafeTranslationValue("Wurde geleert...")

	End Sub

	''' <summary>
	''' Alle vorhandene TabPages und jedes darin befindliches Controls durchlaufen.
	''' </summary>
	''' <remarks></remarks>
	Private Sub ResetAllTabEntries()
		For Each tabPg As DevExpress.XtraTab.XtraTabPage In Me.Controls.Item("xtabKDSearch").Controls
			For Each ctrls In tabPg.Controls
				ResetControl(ctrls)
			Next
		Next
	End Sub

	''' <summary>
	''' Jedes Control wird anhand des Typs zurückgesetzt.
	''' </summary>
	''' <param name="con"></param>
	''' <remarks>Funktion ruft sich rekursiv auf.</remarks>
	Private Sub ResetControl(ByVal con As Control)

		If con.Name = "Cbo_KDFProperty" Or con.Name = "Cbo_KDFProperty" Then
			Trace.WriteLine(con.Text)
		End If

		If Not con.Enabled OrElse con.Name.ToLower.Contains("cbosort") Or con.Name.ToLower.Contains("cbo_KDKontaktArten".ToLower) Or con.Name.ToLower.Contains("luemandant") Then Exit Sub

		If TypeOf (con) Is TextBox Then
			Dim tb As TextBox = con
			tb.Text = String.Empty

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.DateEdit Then
			Dim de As DevExpress.XtraEditors.DateEdit = con
			de.EditValue = Nothing

		ElseIf TypeOf (con) Is DevExpress.XtraEditors.CheckEdit Then
			Dim de As DevExpress.XtraEditors.CheckEdit = con
			de.CheckState = CheckState.Unchecked
			de.EditValue = Nothing

		ElseIf TypeOf (con) Is CheckBox Then
			Dim cbo As CheckBox = con
			cbo.CheckState = CheckState.Unchecked

		ElseIf TypeOf (con) Is ComboBox Then
			Dim cbo As ComboBox = con
			cbo.Text = String.Empty

		ElseIf TypeOf (con) Is ListBox Then
			Dim lst As ListBox = con
			lst.Items.Clear()


		ElseIf TypeOf (con) Is DevExpress.XtraEditors.TextEdit Then
			Dim tb As DevExpress.XtraEditors.TextEdit = con
			tb.Text = String.Empty


		ElseIf TypeOf (con) Is DevExpress.XtraEditors.ListBoxControl Then
			Dim lst As DevExpress.XtraEditors.ListBoxControl = con
			lst.Items.Clear()


		ElseIf con.HasChildren Then
			For Each ctrl In con.Controls
				ResetControl(ctrl)
			Next
		End If

		'Trace.WriteLine(con.EditValue)

	End Sub

#End Region

#Region "Sonstige Funktionen..."


	Private Function LV_GetItemIndex(ByRef lv As ListView) As Integer

		Try
			If lv.Items.Count > 0 Then
				Dim lvi As ListViewItem = lv.SelectedItems(0)  '.Item(0)
				If lvi.Selected Then
					Return lvi.Index
				Else
					Return -1
				End If
			End If

		Catch ex As Exception

		End Try

	End Function

#End Region

#Region "KeyDown für Lst und Textfelder..."

	Private Sub Lst_KDBerufe_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_KDBerufe.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_KDBerufe.Items.Remove(Me.Lst_KDBerufe.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibKDBerufe_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_KDBranche_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_KDBranche.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_KDBranche.Items.Remove(Me.Lst_KDBranche.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibKDBranchen_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_KDStichwort_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_KDStichwort.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_KDStichwort.Items.Remove(Me.Lst_KDStichwort.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibStichwort_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_KDAnstellung_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_KDAnstellung.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_KDAnstellung.Items.Remove(Me.Lst_KDAnstellung.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibAnstellung_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_KDFiliale_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_KDFiliale.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_KDFiliale.Items.Remove(Me.Lst_KDFiliale.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibFiliale_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_KDGAV_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_KDGAV.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_KDGAV.Items.Remove(Me.Lst_KDGAV.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibGAV_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_Z1Res_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_Z1Res.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_Z1Res.Items.Remove(Me.Lst_Z1Res.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibZRes1_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_Z2Res_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_Z2Res.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_Z2Res.Items.Remove(Me.Lst_Z2Res.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibZRes2_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_Z3Res_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_Z3Res.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_Z3Res.Items.Remove(Me.Lst_Z3Res.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibZRes3_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_Z4Res_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_Z4Res.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_Z4Res.Items.Remove(Me.Lst_Z4Res.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibZRes4_LinkClicked(sender, New System.EventArgs)

			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_ZBerufe_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_ZBerufe.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_ZBerufe.Items.Remove(Me.Lst_ZBerufe.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibZBerufe_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_ZBranche_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_ZBranche.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_ZBranche.Items.Remove(Me.Lst_ZBranche.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibZBranche_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_ZKom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_ZKom.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_ZKom.Items.Remove(Me.Lst_ZKom.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibZKom_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub Lst_ZVersand_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Lst_ZVersand.KeyDown

		Try
			If e.KeyValue = Keys.Delete Then
				Me.Lst_ZVersand.Items.Remove(Me.Lst_ZVersand.SelectedItem)
			ElseIf e.KeyValue = Keys.F4 Then
				'LibZVersand_LinkClicked(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtKDName_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKDName_1.ButtonClick
		Dim frmTest As New frmSearchRec("firma1")

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()
		m = frmTest.iKDValue()
		If Not m Is Nothing Then
			m = CStr(m.ToString.Replace("#@", ","))
			txtKDName_1.Text = m
		End If

		frmTest.Dispose()

	End Sub

	Private Sub txtKDName_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKDName_1.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtKDName_1_ButtonClick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtKDName_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKDName_2.ButtonClick
		Dim frmTest As New frmSearchRec("Firma1")

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()
		m = frmTest.iKDValue()
		If Not m Is Nothing Then
			m = CStr(m.ToString.Replace("#@", ","))
			Me.txtKDName_2.Text = m
		End If

		frmTest.Dispose()

	End Sub

	Private Sub txtKDName_2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKDName_2.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtKDName_2_ButtonClick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtKDNr_1_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKDNr_1.ButtonClick
		Dim frmTest As New frmSearchRec("kdnr")

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()
		m = frmTest.iKDValue()
		If Not m Is Nothing Then
			m = CStr(m.ToString.Replace("#@", ","))
			Me.txtKDNr_1.Text = m
		End If
		frmTest.Dispose()

	End Sub

	Private Sub txtKDNr_1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKDNr_1.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtKDNr_1_ButtonClick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

	Private Sub txtKDNr_2_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles txtKDNr_2.ButtonClick
		Dim frmTest As New frmSearchRec("kdNr")

		Dim m As String = String.Empty
		Dim result = frmTest.ShowDialog()
		m = frmTest.iKDValue()
		If Not m Is Nothing Then
			m = CStr(m.ToString.Replace("#@", ","))
			Me.txtKDNr_2.Text = m
		End If
		frmTest.Dispose()

	End Sub

	Private Sub txtKDNr_2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKDNr_2.KeyDown

		Try
			If e.KeyValue = Keys.F4 Then
				txtKDNr_2_ButtonClick(sender, New System.EventArgs)
			End If

		Catch ex As Exception

		End Try

	End Sub

#End Region


	Private Sub txtKDNr_1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtKDNr_1.KeyPress, txtKDNr_2.KeyPress, txtKDName_1.KeyPress, txtKDName_2.KeyPress, txtKDLand_1.KeyPress, txtKDOrt_1.KeyPress, txtKDPLZ_1.KeyPress, Cbo_KDKanton.KeyPress, Cbo_KDBerater.KeyPress, Cbo_KDKontakt.KeyPress, Cbo_KD1Res.KeyPress, Cbo_KD2Res.KeyPress, Cbo_KD3Res.KeyPress, Cbo_KD4Res.KeyPress, Cbo_KD1Stat.KeyPress, Cbo_KD2Stat.KeyPress, Cbo_KDZKond.KeyPress, Cbo_KDBCount.KeyPress, luFProperty.KeyPress,
	Cbo_KDCurrency.KeyPress, Cbo_KDMahn.KeyPress, Cbo_KDFaktura.KeyPress, Cbo_KDWithOP.KeyPress, Cbo_KD1KLimite.KeyPress, Cbo_KD2KLimite.KeyPress,
	txtZName.KeyPress, txtZVorname.KeyPress, Cbo_ZBerater.KeyPress, Cbo_ZAbteilung.KeyPress, Cbo_ZPosition.KeyPress, Cbo_ZKontakt.KeyPress, Cbo_Z1Stat.KeyPress, Cbo_Z2Stat.KeyPress, Cbo_ZMGeb.KeyPress, txt_ZKontaktBez.KeyPress

		Try
			If e.KeyChar = Chr(13) Then
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

	End Sub

	''' <summary>
	''' Übergebene Controls mit dem Klick-Event verbinden
	''' </summary>
	''' <param name="Ctrls"></param>
	''' <remarks></remarks>
	Private Sub InitClickHandler(ByVal ParamArray Ctrls() As Control)

		For Each Ctrl As Control In Ctrls
			AddHandler Ctrl.KeyPress, AddressOf KeyPressEvent
			'      AddHandler Ctrl.Click, AddressOf ClickEvents
		Next

	End Sub

	''' <summary>
	''' Klick-Event der Controls auffangen und verarbeiten
	''' </summary>
	''' <param name="sender"></param>
	''' <param name="e"></param>
	''' <remarks></remarks>
	Private Sub KeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs) ' System.EventArgs)
		'   ToDo  Auswertung und Klick-Aktion ausführen
		'If sender Is TextBox1 Then

		Try
			If e.KeyChar = Chr(13) Then
				SendKeys.Send("{tab}")
				e.Handled = True
			End If

		Catch ex As Exception
			MessageBox.Show(ex.Message, "KeyPress", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try

		'End If
	End Sub

	Private Sub Cbo_KDFax_NoMailing_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDFax_NoMailing.QueryPopUp
		ListForActivate(Me.Cbo_KDFax_NoMailing)
	End Sub

	Private Sub Cbo_KDMail_NoMailing_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_KDMail_NoMailing.QueryPopUp
		ListForActivate(Me.Cbo_KDMail_NoMailing)
	End Sub

	Private Sub Cbo_ZFax_NoMailing_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ZFax_NoMailing.QueryPopUp
		ListForActivate(Me.Cbo_ZFax_NoMailing)
	End Sub

	Private Sub Cbo_ZSMS_NoMailing_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ZSMS_NoMailing.QueryPopUp
		ListForActivate(Me.Cbo_ZSMS_NoMailing)
	End Sub

	Private Sub Cbo_ZMail_NoMailing_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cbo_ZMail_NoMailing.QueryPopUp
		ListForActivate(Me.Cbo_ZMail_NoMailing)
	End Sub


	Private Sub cbo_KDKontaktArten_DropDown(ByVal sender As System.Object, ByVal e As System.EventArgs)
		ListKontaktArten(Me.cbo_KDKontaktArten)
	End Sub


#Region "Panel 4. Seite..."

	Private Sub LblHeader_17_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblzhdreservefelder.Click

		If Not b4_2 Then
			Me.p4_2.Height = Me.Lst_Z3Res.Top + Me.Lst_Z3Res.Height + 10
		Else
			Me.p4_2.Height = Me.lblzhdreservefelder.Height
		End If
		b4_2 = Not b4_2

	End Sub

#End Region



	Private Sub btnPrint_DropDownOpened(sender As Object, e As System.EventArgs)
		Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
		For Each itm As ToolStripItem In ts.DropDownItems
			itm.Text = m_Translate.GetSafeTranslationValue(itm.Text)
			Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
		Next
	End Sub

	Private Sub btnExport_DropDownOpening(sender As Object, e As System.EventArgs)
		Dim ts As ToolStripDropDownButton = CType(sender, ToolStripDropDownButton)
		For Each itm As ToolStripItem In ts.DropDownItems
			itm.Text = m_Translate.GetSafeTranslationValue(itm.Text)
			Trace.WriteLine(String.Format("ChildControlName: {0} Text: ", itm.Text))
		Next
	End Sub




	Private Sub SwitchButton1_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton1.ValueChanged
		Me.txtKDNr_2.Visible = Me.SwitchButton1.Value
		Me.txtKDNr_2.Text = String.Empty
	End Sub

	Private Sub SwitchButton2_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SwitchButton2.ValueChanged
		Me.txtKDName_2.Visible = Me.SwitchButton2.Value
		Me.txtKDName_2.Text = String.Empty
	End Sub

	Private Sub CboSort_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles CboSort.QueryPopUp
		If Me.CboSort.Properties.Items.Count = 0 Then ListSort(Me.CboSort)
	End Sub

	Private Sub cbo_KDArtHide_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles cbo_KDArtHide.QueryPopUp
		ListHideKontaktArt(cbo_KDArtHide)
	End Sub

	Private Sub luFProperty_QueryPopUp(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles luFProperty.QueryPopUp
		ListFProperty(Me.luFProperty)
	End Sub


End Class


Class ComboBoxItem
	Public Text As String
	Public Value As String
	Public Sub New(ByVal text As String, ByVal val As String)
		Me.Text = text
		Me.Value = val
	End Sub
	Public Overrides Function ToString() As String
		Return Text
	End Function
End Class
