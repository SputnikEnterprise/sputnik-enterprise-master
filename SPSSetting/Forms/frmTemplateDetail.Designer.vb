<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTemplateDetail
  Inherits DevExpress.XtraEditors.XtraForm

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTemplateDetail))
		Dim EditorButtonImageOptions1 As DevExpress.XtraEditors.Controls.EditorButtonImageOptions = New DevExpress.XtraEditors.Controls.EditorButtonImageOptions()
		Dim SerializableAppearanceObject1 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject2 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject3 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Dim SerializableAppearanceObject4 As DevExpress.Utils.SerializableAppearanceObject = New DevExpress.Utils.SerializableAppearanceObject()
		Me.cmdClose = New DevExpress.XtraEditors.SimpleButton()
		Me.Label4 = New System.Windows.Forms.Label()
		Me.Label3 = New System.Windows.Forms.Label()
		Me.PictureBox1 = New DevExpress.XtraEditors.PanelControl()
		Me.cmdOK = New DevExpress.XtraEditors.SimpleButton()
		Me.txtFaxExtension = New DevExpress.XtraEditors.TextEdit()
		Me.Label7 = New System.Windows.Forms.Label()
		Me.Label8 = New System.Windows.Forms.Label()
		Me.txtFaxForwarder = New DevExpress.XtraEditors.TextEdit()
		Me.Label9 = New System.Windows.Forms.Label()
		Me.pnlECall = New DevExpress.XtraEditors.PanelControl()
		Me.Label67 = New System.Windows.Forms.Label()
		Me.txteCallToken = New DevExpress.XtraEditors.TextEdit()
		Me.Label68 = New System.Windows.Forms.Label()
		Me.txteCallNotification = New DevExpress.XtraEditors.TextEdit()
		Me.lbleCallJobID = New System.Windows.Forms.Label()
		Me.txteCallJobID = New DevExpress.XtraEditors.TextEdit()
		Me.Label66 = New System.Windows.Forms.Label()
		Me.txteCallSubject = New DevExpress.XtraEditors.TextEdit()
		Me.Label65 = New System.Windows.Forms.Label()
		Me.txteCallHeaderInfo = New DevExpress.XtraEditors.TextEdit()
		Me.Label64 = New System.Windows.Forms.Label()
		Me.txteCallHeaderID = New DevExpress.XtraEditors.TextEdit()
		Me.Label63 = New System.Windows.Forms.Label()
		Me.txteCallFromText = New DevExpress.XtraEditors.TextEdit()
		Me.Label5 = New System.Windows.Forms.Label()
		Me.GroupBox4 = New System.Windows.Forms.GroupBox()
		Me.Label1 = New System.Windows.Forms.Label()
		Me.chkSSL = New DevExpress.XtraEditors.CheckEdit()
		Me.BarManager1 = New DevExpress.XtraBars.BarManager(Me.components)
		Me.Bar1 = New DevExpress.XtraBars.Bar()
		Me.bsiInfo_1 = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiInfo_2 = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiInfo_3 = New DevExpress.XtraBars.BarStaticItem()
		Me.bsiInfo_4 = New DevExpress.XtraBars.BarStaticItem()
		Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
		Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
		Me.Label20 = New System.Windows.Forms.Label()
		Me.txtSMTP = New DevExpress.XtraEditors.TextEdit()
		Me.Label21 = New System.Windows.Forms.Label()
		Me.txtSMTPPort = New DevExpress.XtraEditors.TextEdit()
		Me.cboSmtpDeliveryMethod = New DevExpress.XtraEditors.LookUpEdit()
		Me.GroupBox2 = New System.Windows.Forms.GroupBox()
		Me.chkJobChannelPriority = New DevExpress.XtraEditors.CheckEdit()
		Me.chksendproposeattachmenttowos = New DevExpress.XtraEditors.CheckEdit()
		Me.chkenablecustomerwos = New DevExpress.XtraEditors.CheckEdit()
		Me.chkenableemployeewos = New DevExpress.XtraEditors.CheckEdit()
		Me.chkenablevacancywos = New DevExpress.XtraEditors.CheckEdit()
		Me.txt_Ver_ID = New DevExpress.XtraEditors.TextEdit()
		Me.Label14 = New System.Windows.Forms.Label()
		Me.txt_KD_ID = New DevExpress.XtraEditors.TextEdit()
		Me.Label12 = New System.Windows.Forms.Label()
		Me.txt_MA_ID = New DevExpress.XtraEditors.TextEdit()
		Me.Label11 = New System.Windows.Forms.Label()
		Me.txt_Vak_ID = New DevExpress.XtraEditors.TextEdit()
		Me.Label10 = New System.Windows.Forms.Label()
		Me.GroupBox7 = New System.Windows.Forms.GroupBox()
		Me.lblTemplatePath_1 = New System.Windows.Forms.Label()
		Me.Label22 = New System.Windows.Forms.Label()
		Me.txtCockpitPicture = New DevExpress.XtraEditors.TextEdit()
		Me.Label15 = New System.Windows.Forms.Label()
		Me.txtCockpitWWW = New DevExpress.XtraEditors.TextEdit()
		Me.GroupBox3 = New System.Windows.Forms.GroupBox()
		Me.Label23 = New System.Windows.Forms.Label()
		Me.txtZHDDocWWW = New DevExpress.XtraEditors.TextEdit()
		Me.Label24 = New System.Windows.Forms.Label()
		Me.txtZHDDocMailBetreff = New DevExpress.XtraEditors.TextEdit()
		Me.Label18 = New System.Windows.Forms.Label()
		Me.txtKDDocWWW = New DevExpress.XtraEditors.TextEdit()
		Me.Label16 = New System.Windows.Forms.Label()
		Me.txtKDDocMailBetreff = New DevExpress.XtraEditors.TextEdit()
		Me.GroupBox6 = New System.Windows.Forms.GroupBox()
		Me.txtMADocWWW = New DevExpress.XtraEditors.TextEdit()
		Me.txtMADocMailBetreff = New DevExpress.XtraEditors.TextEdit()
		Me.Label19 = New System.Windows.Forms.Label()
		Me.Label17 = New System.Windows.Forms.Label()
		Me.GroupBox5 = New System.Windows.Forms.GroupBox()
		Me.Label29 = New System.Windows.Forms.Label()
		Me.txtArbgDocMailBetreff = New DevExpress.XtraEditors.TextEdit()
		Me.Label26 = New System.Windows.Forms.Label()
		Me.txtZVDocMailBetreff = New DevExpress.XtraEditors.TextEdit()
		Me.cleLNormal_0 = New DevExpress.XtraEditors.ColorEdit()
		Me.Label51 = New System.Windows.Forms.Label()
		Me.Label54 = New System.Windows.Forms.Label()
		Me.cboLFormStyle = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.pGlobal = New DevExpress.XtraEditors.PanelControl()
		Me.gGlobal = New DevExpress.XtraEditors.GroupControl()
		Me.GroupPanel10 = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.chkopeninvoiceformmorethanonce = New DevExpress.XtraEditors.CheckEdit()
		Me.chkopenadvancedpaymentformmorethanonce = New DevExpress.XtraEditors.CheckEdit()
		Me.chkopenreportsformmorethanonce = New DevExpress.XtraEditors.CheckEdit()
		Me.chkopeneinsatzformmorethanonce = New DevExpress.XtraEditors.CheckEdit()
		Me.chkopencustomerformmorethanonce = New DevExpress.XtraEditors.CheckEdit()
		Me.chkopenemployeeformmorethanonce = New DevExpress.XtraEditors.CheckEdit()
		Me.GroupPanel9 = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.chkAllowAutoFilterConditionChange = New DevExpress.XtraEditors.CheckEdit()
		Me.Label61 = New System.Windows.Forms.Label()
		Me.Label53 = New System.Windows.Forms.Label()
		Me.ceMandantColor = New DevExpress.XtraEditors.ColorEdit()
		Me.Label97 = New System.Windows.Forms.Label()
		Me.chkGLLDebug = New DevExpress.XtraEditors.CheckEdit()
		Me.cboautofilterconditionnr = New DevExpress.XtraEditors.LookUpEdit()
		Me.cboAutoFilterConditionDate = New DevExpress.XtraEditors.LookUpEdit()
		Me.GroupPanel8 = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.chksavecustomerinvoicescanintowos = New DevExpress.XtraEditors.CheckEdit()
		Me.chksavecustomerreportscanintowos = New DevExpress.XtraEditors.CheckEdit()
		Me.chksavecustomeremploymentscanintowos = New DevExpress.XtraEditors.CheckEdit()
		Me.chksaveemployeepayrollscanintowos = New DevExpress.XtraEditors.CheckEdit()
		Me.chksaveemployeereportscanintowos = New DevExpress.XtraEditors.CheckEdit()
		Me.chksaveemployeeemploymentscanintowos = New DevExpress.XtraEditors.CheckEdit()
		Me.Label40 = New System.Windows.Forms.Label()
		Me.chkimportscanreportzeroamount = New DevExpress.XtraEditors.CheckEdit()
		Me.chkimportscanreporttoboth = New DevExpress.XtraEditors.CheckEdit()
		Me.txtnotificationintervalperiodeforreport = New DevExpress.XtraEditors.TextEdit()
		Me.Label38 = New System.Windows.Forms.Label()
		Me.chkchangeownreportforfinishingflag = New DevExpress.XtraEditors.CheckEdit()
		Me.Label6 = New System.Windows.Forms.Label()
		Me.txtnotificationintervalperiode = New DevExpress.XtraEditors.TextEdit()
		Me.Label2 = New System.Windows.Forms.Label()
		Me.Label57 = New System.Windows.Forms.Label()
		Me.seRPScanSize = New DevExpress.XtraEditors.SpinEdit()
		Me.chkComatic = New DevExpress.XtraEditors.CheckEdit()
		Me.chkCSOPList = New DevExpress.XtraEditors.CheckEdit()
		Me.chkKMUFactoring = New DevExpress.XtraEditors.CheckEdit()
		Me.chkParifond = New DevExpress.XtraEditors.CheckEdit()
		Me.chkSwifac = New DevExpress.XtraEditors.CheckEdit()
		Me.chkAbacus = New DevExpress.XtraEditors.CheckEdit()
		Me.chkSesam = New DevExpress.XtraEditors.CheckEdit()
		Me.chkaskonexit = New DevExpress.XtraEditors.CheckEdit()
		Me.chkLUpdate = New DevExpress.XtraEditors.CheckEdit()
		Me.Label41 = New System.Windows.Forms.Label()
		Me.Label35 = New System.Windows.Forms.Label()
		Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl()
		Me.seAnzTestRP = New DevExpress.XtraEditors.SpinEdit()
		Me.Label47 = New System.Windows.Forms.Label()
		Me.seAnzTestCheck = New DevExpress.XtraEditors.SpinEdit()
		Me.Label46 = New System.Windows.Forms.Label()
		Me.chkRPOpenWeek = New DevExpress.XtraEditors.CheckEdit()
		Me.chkRPWeekMust = New DevExpress.XtraEditors.CheckEdit()
		Me.pLColor = New DevExpress.XtraEditors.PanelControl()
		Me.gLColor = New DevExpress.XtraEditors.GroupControl()
		Me.glRPData = New DevExpress.XtraEditors.GroupControl()
		Me.GroupPanel7 = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.GroupPanel6 = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.Label45 = New System.Windows.Forms.Label()
		Me.lblLMDPfad = New System.Windows.Forms.Label()
		Me.Label49 = New System.Windows.Forms.Label()
		Me.txtLPfad_3 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Label44 = New System.Windows.Forms.Label()
		Me.txtLPfad_2 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtLPfad_1 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.txtLPfad_0 = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.GroupPanel5 = New DevComponents.DotNetBar.Controls.GroupPanel()
		Me.Label48 = New System.Windows.Forms.Label()
		Me.cboLNavStyle = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.Label13 = New System.Windows.Forms.Label()
		Me.pNavbar = New DevExpress.XtraEditors.PanelControl()
		Me.nvMain = New DevExpress.XtraNavBar.NavBarControl()
		Me.nvgLGSetting = New DevExpress.XtraNavBar.NavBarGroup()
		Me.nviLFieldColor = New DevExpress.XtraNavBar.NavBarItem()
		Me.nviGlobalSetting = New DevExpress.XtraNavBar.NavBarItem()
		Me.nbgFeldinformationen = New DevExpress.XtraNavBar.NavBarGroup()
		Me.nviLicense = New DevExpress.XtraNavBar.NavBarItem()
		Me.nvLayout = New DevExpress.XtraNavBar.NavBarGroup()
		Me.nviMetro_0 = New DevExpress.XtraNavBar.NavBarItem()
		Me.nvgCom = New DevExpress.XtraNavBar.NavBarGroup()
		Me.nviWOS = New DevExpress.XtraNavBar.NavBarItem()
		Me.nviMailFax = New DevExpress.XtraNavBar.NavBarItem()
		Me.nviMailTpl = New DevExpress.XtraNavBar.NavBarItem()
		Me.nvgJob = New DevExpress.XtraNavBar.NavBarGroup()
		Me.nviJCH = New DevExpress.XtraNavBar.NavBarItem()
		Me.nviJWinner = New DevExpress.XtraNavBar.NavBarItem()
		Me.xscMain = New DevExpress.XtraEditors.XtraScrollableControl()
		Me.pMail_Tpl = New DevExpress.XtraEditors.PanelControl()
		Me.gMail_Tpl = New DevExpress.XtraEditors.GroupControl()
		Me.pCom_WOS = New DevExpress.XtraEditors.PanelControl()
		Me.gWOS = New DevExpress.XtraEditors.GroupControl()
		Me.pMailFax = New DevExpress.XtraEditors.PanelControl()
		Me.gMailFax = New DevExpress.XtraEditors.GroupControl()
		Me.xtabFax = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabeCall = New DevExpress.XtraTab.XtraTabPage()
		Me.xtabDavidFaxserver = New DevExpress.XtraTab.XtraTabPage()
		Me.PanelControl5 = New DevExpress.XtraEditors.PanelControl()
		Me.Label73 = New System.Windows.Forms.Label()
		Me.txtDavidfaxserver = New DevExpress.XtraEditors.TextEdit()
		Me.GroupBox10 = New System.Windows.Forms.GroupBox()
		Me.chkCresus = New DevExpress.XtraEditors.CheckEdit()
		Me.chkpmsearch = New DevExpress.XtraEditors.CheckEdit()
		Me.chkallowedemployeeweeklypayment = New DevExpress.XtraEditors.CheckEdit()
		Me.chkCVDropIn = New DevExpress.XtraEditors.CheckEdit()
		Me.chkScanDropIN = New DevExpress.XtraEditors.CheckEdit()
		Me.GroupBox8 = New System.Windows.Forms.GroupBox()
		Me.Label77 = New System.Windows.Forms.Label()
		Me.txt_DVURL = New DevExpress.XtraEditors.TextEdit()
		Me.Label76 = New System.Windows.Forms.Label()
		Me.txt_DVPW = New DevExpress.XtraEditors.TextEdit()
		Me.Label74 = New System.Windows.Forms.Label()
		Me.txt_DVRefNo = New DevExpress.XtraEditors.TextEdit()
		Me.Label75 = New System.Windows.Forms.Label()
		Me.txt_DVUSName = New DevExpress.XtraEditors.TextEdit()
		Me.StyleManager1 = New DevComponents.DotNetBar.StyleManager(Me.components)
		Me.sccMain = New DevExpress.XtraEditors.SplitContainerControl()
		Me.pJobplattforms = New DevExpress.XtraEditors.PanelControl()
		Me.XtraTabControl3 = New DevExpress.XtraTab.XtraTabControl()
		Me.xtabSettingJobCH = New DevExpress.XtraTab.XtraTabPage()
		Me.gJCH = New DevExpress.XtraEditors.GroupControl()
		Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl()
		Me.cmdJCHKDSubDelete = New DevExpress.XtraEditors.SimpleButton()
		Me.cmdSaveSubNr = New DevExpress.XtraEditors.SimpleButton()
		Me.txtJCHKDSubNr = New DevExpress.XtraEditors.TextEdit()
		Me.seJCHKDSubAnz = New DevExpress.XtraEditors.SpinEdit()
		Me.Label71 = New System.Windows.Forms.Label()
		Me.lstJCHKDSubNr = New DevExpress.XtraEditors.ListBoxControl()
		Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl()
		Me.Label79 = New System.Windows.Forms.Label()
		Me.seJCHAddDayToDate = New DevExpress.XtraEditors.SpinEdit()
		Me.Label78 = New System.Windows.Forms.Label()
		Me.Label72 = New System.Windows.Forms.Label()
		Me.cboJCHAngebot_Art = New DevExpress.XtraEditors.ComboBoxEdit()
		Me.cmdSaveJCHAccount = New DevExpress.XtraEditors.SimpleButton()
		Me.seJCHKDAnz = New DevExpress.XtraEditors.SpinEdit()
		Me.chJCH_Xing_Company_Is_Poc = New DevExpress.XtraEditors.CheckEdit()
		Me.txtJCH_Xing_Company_Profile_URL = New DevExpress.XtraEditors.TextEdit()
		Me.Label70 = New System.Windows.Forms.Label()
		Me.Label43 = New System.Windows.Forms.Label()
		Me.txtJCH_Xing_Poster_URL = New DevExpress.XtraEditors.TextEdit()
		Me.Label69 = New System.Windows.Forms.Label()
		Me.Label37 = New System.Windows.Forms.Label()
		Me.txtJCH_Bewerber_URL = New DevExpress.XtraEditors.TextEdit()
		Me.Label32 = New System.Windows.Forms.Label()
		Me.txtJCH_Direkt_URL = New DevExpress.XtraEditors.TextEdit()
		Me.txtJCH_Our_URL = New DevExpress.XtraEditors.TextEdit()
		Me.txtJCH_Logo_ID = New DevExpress.XtraEditors.TextEdit()
		Me.txtJCH_Layout_ID = New DevExpress.XtraEditors.TextEdit()
		Me.txtJCH_Organisation_ID = New DevExpress.XtraEditors.TextEdit()
		Me.Label39 = New System.Windows.Forms.Label()
		Me.Label42 = New System.Windows.Forms.Label()
		Me.Label36 = New System.Windows.Forms.Label()
		Me.Label33 = New System.Windows.Forms.Label()
		Me.xtabOurJobplattform = New DevExpress.XtraTab.XtraTabPage()
		Me.grpOurJobPlattformData = New DevExpress.XtraEditors.GroupControl()
		Me.PanelControl19 = New DevExpress.XtraEditors.PanelControl()
		Me.picJCHLogo = New DevExpress.XtraEditors.PictureEdit()
		Me.Label34 = New System.Windows.Forms.Label()
		Me.xtabSettingOstjob = New DevExpress.XtraTab.XtraTabPage()
		Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl()
		Me.PanelControl7 = New DevExpress.XtraEditors.PanelControl()
		Me.txtOstjob_Bewerberform = New DevExpress.XtraEditors.TextEdit()
		Me.Label52 = New System.Windows.Forms.Label()
		Me.txtOstjob_DirectlinkiFrame = New DevExpress.XtraEditors.TextEdit()
		Me.Label50 = New System.Windows.Forms.Label()
		Me.Label94 = New System.Windows.Forms.Label()
		Me.seOstJobAddDayToDate = New DevExpress.XtraEditors.SpinEdit()
		Me.Label95 = New System.Windows.Forms.Label()
		Me.cmdSaveOstJobAccount = New DevExpress.XtraEditors.SimpleButton()
		Me.seOstJobKDAnz = New DevExpress.XtraEditors.SpinEdit()
		Me.Label98 = New System.Windows.Forms.Label()
		Me.Label100 = New System.Windows.Forms.Label()
		Me.txtOstJob_OrganisationID = New DevExpress.XtraEditors.TextEdit()
		Me.xtabSettingsuedost = New DevExpress.XtraTab.XtraTabPage()
		Me.gLizenzen = New DevExpress.XtraEditors.GroupControl()
		Me.pLizenzen = New DevExpress.XtraEditors.PanelControl()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PictureBox1.SuspendLayout()
		CType(Me.txtFaxExtension.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtFaxForwarder.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pnlECall, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pnlECall.SuspendLayout()
		CType(Me.txteCallToken.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txteCallNotification.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txteCallJobID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txteCallSubject.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txteCallHeaderInfo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txteCallHeaderID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txteCallFromText.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox4.SuspendLayout()
		CType(Me.chkSSL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtSMTP.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtSMTPPort.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cboSmtpDeliveryMethod.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox2.SuspendLayout()
		CType(Me.chkJobChannelPriority.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chksendproposeattachmenttowos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkenablecustomerwos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkenableemployeewos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkenablevacancywos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Ver_ID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_KD_ID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_MA_ID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_Vak_ID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox7.SuspendLayout()
		CType(Me.txtCockpitPicture.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtCockpitWWW.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox3.SuspendLayout()
		CType(Me.txtZHDDocWWW.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtZHDDocMailBetreff.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtKDDocWWW.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtKDDocMailBetreff.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox6.SuspendLayout()
		CType(Me.txtMADocWWW.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtMADocMailBetreff.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox5.SuspendLayout()
		CType(Me.txtArbgDocMailBetreff.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtZVDocMailBetreff.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cleLNormal_0.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cboLFormStyle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pGlobal, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pGlobal.SuspendLayout()
		CType(Me.gGlobal, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.gGlobal.SuspendLayout()
		Me.GroupPanel10.SuspendLayout()
		CType(Me.chkopeninvoiceformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkopenadvancedpaymentformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkopenreportsformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkopeneinsatzformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkopencustomerformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkopenemployeeformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupPanel9.SuspendLayout()
		CType(Me.chkAllowAutoFilterConditionChange.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.ceMandantColor.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkGLLDebug.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cboautofilterconditionnr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cboAutoFilterConditionDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupPanel8.SuspendLayout()
		CType(Me.chksavecustomerinvoicescanintowos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chksavecustomerreportscanintowos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chksavecustomeremploymentscanintowos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chksaveemployeepayrollscanintowos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chksaveemployeereportscanintowos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chksaveemployeeemploymentscanintowos.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkimportscanreportzeroamount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkimportscanreporttoboth.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtnotificationintervalperiodeforreport.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkchangeownreportforfinishingflag.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtnotificationintervalperiode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.seRPScanSize.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkComatic.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkCSOPList.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkKMUFactoring.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkParifond.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkSwifac.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkAbacus.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkSesam.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkaskonexit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkLUpdate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl3.SuspendLayout()
		CType(Me.seAnzTestRP.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.seAnzTestCheck.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkRPOpenWeek.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkRPWeekMust.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pLColor, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pLColor.SuspendLayout()
		CType(Me.gLColor, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.gLColor.SuspendLayout()
		CType(Me.glRPData, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.glRPData.SuspendLayout()
		Me.GroupPanel7.SuspendLayout()
		Me.GroupPanel6.SuspendLayout()
		CType(Me.txtLPfad_3.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtLPfad_2.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtLPfad_1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtLPfad_0.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupPanel5.SuspendLayout()
		CType(Me.cboLNavStyle.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pNavbar, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pNavbar.SuspendLayout()
		CType(Me.nvMain, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.pMail_Tpl, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pMail_Tpl.SuspendLayout()
		CType(Me.gMail_Tpl, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.gMail_Tpl.SuspendLayout()
		CType(Me.pCom_WOS, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pCom_WOS.SuspendLayout()
		CType(Me.gWOS, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.gWOS.SuspendLayout()
		CType(Me.pMailFax, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pMailFax.SuspendLayout()
		CType(Me.gMailFax, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.gMailFax.SuspendLayout()
		CType(Me.xtabFax, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabFax.SuspendLayout()
		Me.xtabeCall.SuspendLayout()
		Me.xtabDavidFaxserver.SuspendLayout()
		CType(Me.PanelControl5, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl5.SuspendLayout()
		CType(Me.txtDavidfaxserver.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox10.SuspendLayout()
		CType(Me.chkCresus.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkpmsearch.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkallowedemployeeweeklypayment.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkCVDropIn.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chkScanDropIN.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupBox8.SuspendLayout()
		CType(Me.txt_DVURL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_DVPW.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_DVRefNo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txt_DVUSName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.sccMain.SuspendLayout()
		CType(Me.pJobplattforms, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pJobplattforms.SuspendLayout()
		CType(Me.XtraTabControl3, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.XtraTabControl3.SuspendLayout()
		Me.xtabSettingJobCH.SuspendLayout()
		CType(Me.gJCH, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.gJCH.SuspendLayout()
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl1.SuspendLayout()
		CType(Me.txtJCHKDSubNr.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.seJCHKDSubAnz.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lstJCHKDSubNr, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl2.SuspendLayout()
		CType(Me.seJCHAddDayToDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.cboJCHAngebot_Art.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.seJCHKDAnz.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.chJCH_Xing_Company_Is_Poc.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtJCH_Xing_Company_Profile_URL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtJCH_Xing_Poster_URL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtJCH_Bewerber_URL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtJCH_Direkt_URL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtJCH_Our_URL.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtJCH_Logo_ID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtJCH_Layout_ID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtJCH_Organisation_ID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabOurJobplattform.SuspendLayout()
		CType(Me.grpOurJobPlattformData, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.grpOurJobPlattformData.SuspendLayout()
		CType(Me.PanelControl19, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl19.SuspendLayout()
		CType(Me.picJCHLogo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.xtabSettingOstjob.SuspendLayout()
		CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.GroupControl1.SuspendLayout()
		CType(Me.PanelControl7, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelControl7.SuspendLayout()
		CType(Me.txtOstjob_Bewerberform.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtOstjob_DirectlinkiFrame.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.seOstJobAddDayToDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.seOstJobKDAnz.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtOstJob_OrganisationID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.gLizenzen, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.gLizenzen.SuspendLayout()
		CType(Me.pLizenzen, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.pLizenzen.SuspendLayout()
		Me.SuspendLayout()
		'
		'cmdClose
		'
		Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdClose.Location = New System.Drawing.Point(1606, 22)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.Size = New System.Drawing.Size(91, 29)
		Me.cmdClose.TabIndex = 10
		Me.cmdClose.Text = "Schliessen"
		'
		'Label4
		'
		Me.Label4.AutoSize = True
		Me.Label4.BackColor = System.Drawing.Color.Transparent
		Me.Label4.Location = New System.Drawing.Point(92, 44)
		Me.Label4.Name = "Label4"
		Me.Label4.Size = New System.Drawing.Size(225, 13)
		Me.Label4.TabIndex = 14
		Me.Label4.Text = "Wählen Sie Ihre gewünschten Kriterien aus..."
		'
		'Label3
		'
		Me.Label3.AutoSize = True
		Me.Label3.BackColor = System.Drawing.Color.Transparent
		Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.Location = New System.Drawing.Point(70, 22)
		Me.Label3.Name = "Label3"
		Me.Label3.Size = New System.Drawing.Size(122, 13)
		Me.Label3.TabIndex = 13
		Me.Label3.Text = "Systemeinstellungen"
		'
		'PictureBox1
		'
		Me.PictureBox1.Appearance.BackColor = System.Drawing.Color.White
		Me.PictureBox1.Appearance.Options.UseBackColor = True
		Me.PictureBox1.Controls.Add(Me.cmdOK)
		Me.PictureBox1.Dock = System.Windows.Forms.DockStyle.Top
		Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(1710, 74)
		Me.PictureBox1.TabIndex = 12
		'
		'cmdOK
		'
		Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdOK.Location = New System.Drawing.Point(1497, 22)
		Me.cmdOK.Name = "cmdOK"
		Me.cmdOK.Size = New System.Drawing.Size(91, 29)
		Me.cmdOK.TabIndex = 52
		Me.cmdOK.Text = "Speichern"
		'
		'txtFaxExtension
		'
		Me.txtFaxExtension.Location = New System.Drawing.Point(164, 49)
		Me.txtFaxExtension.Name = "txtFaxExtension"
		Me.txtFaxExtension.Size = New System.Drawing.Size(256, 20)
		Me.txtFaxExtension.TabIndex = 2
		'
		'Label7
		'
		Me.Label7.AutoSize = True
		Me.Label7.Location = New System.Drawing.Point(9, 53)
		Me.Label7.Name = "Label7"
		Me.Label7.Size = New System.Drawing.Size(139, 13)
		Me.Label7.TabIndex = 36
		Me.Label7.Text = "Erweiterung an Faxnummer"
		'
		'Label8
		'
		Me.Label8.AutoSize = True
		Me.Label8.Location = New System.Drawing.Point(426, 79)
		Me.Label8.Name = "Label8"
		Me.Label8.Size = New System.Drawing.Size(108, 13)
		Me.Label8.TabIndex = 37
		Me.Label8.Text = "(bitte mit @-Zeichen)"
		Me.Label8.Visible = False
		'
		'txtFaxForwarder
		'
		Me.txtFaxForwarder.Location = New System.Drawing.Point(164, 75)
		Me.txtFaxForwarder.Name = "txtFaxForwarder"
		Me.txtFaxForwarder.Size = New System.Drawing.Size(256, 20)
		Me.txtFaxForwarder.TabIndex = 3
		'
		'Label9
		'
		Me.Label9.AutoSize = True
		Me.Label9.Location = New System.Drawing.Point(9, 79)
		Me.Label9.Name = "Label9"
		Me.Label9.Size = New System.Drawing.Size(112, 13)
		Me.Label9.TabIndex = 39
		Me.Label9.Text = "Fax-Absender (SMTP)"
		'
		'pnlECall
		'
		Me.pnlECall.Appearance.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(236, Byte), Integer), CType(CType(239, Byte), Integer))
		Me.pnlECall.Appearance.Options.UseBackColor = True
		Me.pnlECall.Controls.Add(Me.Label67)
		Me.pnlECall.Controls.Add(Me.txteCallToken)
		Me.pnlECall.Controls.Add(Me.Label68)
		Me.pnlECall.Controls.Add(Me.txteCallNotification)
		Me.pnlECall.Controls.Add(Me.lbleCallJobID)
		Me.pnlECall.Controls.Add(Me.txteCallJobID)
		Me.pnlECall.Controls.Add(Me.Label66)
		Me.pnlECall.Controls.Add(Me.txteCallSubject)
		Me.pnlECall.Controls.Add(Me.Label65)
		Me.pnlECall.Controls.Add(Me.txteCallHeaderInfo)
		Me.pnlECall.Controls.Add(Me.Label64)
		Me.pnlECall.Controls.Add(Me.txteCallHeaderID)
		Me.pnlECall.Controls.Add(Me.Label63)
		Me.pnlECall.Controls.Add(Me.txteCallFromText)
		Me.pnlECall.Controls.Add(Me.Label5)
		Me.pnlECall.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pnlECall.Location = New System.Drawing.Point(5, 5)
		Me.pnlECall.Name = "pnlECall"
		Me.pnlECall.Size = New System.Drawing.Size(648, 224)
		Me.pnlECall.TabIndex = 40
		'
		'Label67
		'
		Me.Label67.AutoSize = True
		Me.Label67.BackColor = System.Drawing.Color.Transparent
		Me.Label67.Location = New System.Drawing.Point(13, 185)
		Me.Label67.Name = "Label67"
		Me.Label67.Size = New System.Drawing.Size(68, 13)
		Me.Label67.TabIndex = 43
		Me.Label67.Text = "Token-String"
		'
		'txteCallToken
		'
		Me.txteCallToken.Location = New System.Drawing.Point(164, 181)
		Me.txteCallToken.Name = "txteCallToken"
		Me.txteCallToken.Size = New System.Drawing.Size(233, 20)
		Me.txteCallToken.TabIndex = 8
		'
		'Label68
		'
		Me.Label68.AutoSize = True
		Me.Label68.BackColor = System.Drawing.Color.Transparent
		Me.Label68.Location = New System.Drawing.Point(13, 159)
		Me.Label68.Name = "Label68"
		Me.Label68.Size = New System.Drawing.Size(93, 13)
		Me.Label68.TabIndex = 42
		Me.Label68.Text = "Notification-String"
		'
		'txteCallNotification
		'
		Me.txteCallNotification.Location = New System.Drawing.Point(164, 155)
		Me.txteCallNotification.Name = "txteCallNotification"
		Me.txteCallNotification.Size = New System.Drawing.Size(233, 20)
		Me.txteCallNotification.TabIndex = 7
		'
		'lbleCallJobID
		'
		Me.lbleCallJobID.Image = CType(resources.GetObject("lbleCallJobID.Image"), System.Drawing.Image)
		Me.lbleCallJobID.Location = New System.Drawing.Point(403, 26)
		Me.lbleCallJobID.Name = "lbleCallJobID"
		Me.lbleCallJobID.Size = New System.Drawing.Size(17, 19)
		Me.lbleCallJobID.TabIndex = 39
		'
		'txteCallJobID
		'
		Me.txteCallJobID.EditValue = ""
		Me.txteCallJobID.Location = New System.Drawing.Point(164, 25)
		Me.txteCallJobID.Name = "txteCallJobID"
		Me.txteCallJobID.Size = New System.Drawing.Size(233, 20)
		Me.txteCallJobID.TabIndex = 2
		'
		'Label66
		'
		Me.Label66.AutoSize = True
		Me.Label66.BackColor = System.Drawing.Color.Transparent
		Me.Label66.Location = New System.Drawing.Point(13, 133)
		Me.Label66.Name = "Label66"
		Me.Label66.Size = New System.Drawing.Size(65, 13)
		Me.Label66.TabIndex = 36
		Me.Label66.Text = "Fax-Subject"
		'
		'txteCallSubject
		'
		Me.txteCallSubject.Location = New System.Drawing.Point(164, 129)
		Me.txteCallSubject.Name = "txteCallSubject"
		Me.txteCallSubject.Size = New System.Drawing.Size(233, 20)
		Me.txteCallSubject.TabIndex = 6
		'
		'Label65
		'
		Me.Label65.AutoSize = True
		Me.Label65.BackColor = System.Drawing.Color.Transparent
		Me.Label65.Location = New System.Drawing.Point(13, 107)
		Me.Label65.Name = "Label65"
		Me.Label65.Size = New System.Drawing.Size(84, 13)
		Me.Label65.TabIndex = 34
		Me.Label65.Text = "FaxHeader-Info"
		'
		'txteCallHeaderInfo
		'
		Me.txteCallHeaderInfo.Location = New System.Drawing.Point(164, 103)
		Me.txteCallHeaderInfo.Name = "txteCallHeaderInfo"
		Me.txteCallHeaderInfo.Size = New System.Drawing.Size(233, 20)
		Me.txteCallHeaderInfo.TabIndex = 5
		'
		'Label64
		'
		Me.Label64.AutoSize = True
		Me.Label64.BackColor = System.Drawing.Color.Transparent
		Me.Label64.Location = New System.Drawing.Point(13, 81)
		Me.Label64.Name = "Label64"
		Me.Label64.Size = New System.Drawing.Size(75, 13)
		Me.Label64.TabIndex = 32
		Me.Label64.Text = "FaxHeader-ID"
		'
		'txteCallHeaderID
		'
		Me.txteCallHeaderID.Location = New System.Drawing.Point(164, 77)
		Me.txteCallHeaderID.Name = "txteCallHeaderID"
		Me.txteCallHeaderID.Size = New System.Drawing.Size(233, 20)
		Me.txteCallHeaderID.TabIndex = 4
		'
		'Label63
		'
		Me.Label63.AutoSize = True
		Me.Label63.BackColor = System.Drawing.Color.Transparent
		Me.Label63.Location = New System.Drawing.Point(13, 55)
		Me.Label63.Name = "Label63"
		Me.Label63.Size = New System.Drawing.Size(57, 13)
		Me.Label63.TabIndex = 30
		Me.Label63.Text = "From-Text"
		'
		'txteCallFromText
		'
		Me.txteCallFromText.Location = New System.Drawing.Point(164, 51)
		Me.txteCallFromText.Name = "txteCallFromText"
		Me.txteCallFromText.Size = New System.Drawing.Size(233, 20)
		Me.txteCallFromText.TabIndex = 3
		'
		'Label5
		'
		Me.Label5.AutoSize = True
		Me.Label5.BackColor = System.Drawing.Color.Transparent
		Me.Label5.Location = New System.Drawing.Point(13, 28)
		Me.Label5.Name = "Label5"
		Me.Label5.Size = New System.Drawing.Size(81, 13)
		Me.Label5.TabIndex = 28
		Me.Label5.Text = "Anmelde-Daten"
		'
		'GroupBox4
		'
		Me.GroupBox4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox4.BackColor = System.Drawing.Color.Transparent
		Me.GroupBox4.Controls.Add(Me.Label1)
		Me.GroupBox4.Controls.Add(Me.chkSSL)
		Me.GroupBox4.Controls.Add(Me.Label20)
		Me.GroupBox4.Controls.Add(Me.txtSMTP)
		Me.GroupBox4.Controls.Add(Me.Label21)
		Me.GroupBox4.Controls.Add(Me.txtSMTPPort)
		Me.GroupBox4.Controls.Add(Me.cboSmtpDeliveryMethod)
		Me.GroupBox4.Location = New System.Drawing.Point(20, 40)
		Me.GroupBox4.Name = "GroupBox4"
		Me.GroupBox4.Size = New System.Drawing.Size(660, 83)
		Me.GroupBox4.TabIndex = 41
		Me.GroupBox4.TabStop = False
		Me.GroupBox4.Text = "E-Mail Versand"
		'
		'Label1
		'
		Me.Label1.AutoSize = True
		Me.Label1.Location = New System.Drawing.Point(58, 56)
		Me.Label1.Name = "Label1"
		Me.Label1.Size = New System.Drawing.Size(106, 13)
		Me.Label1.TabIndex = 29
		Me.Label1.Text = "SmtpDeliveryMethod"
		'
		'chkSSL
		'
		Me.chkSSL.Location = New System.Drawing.Point(509, 26)
		Me.chkSSL.MenuManager = Me.BarManager1
		Me.chkSSL.Name = "chkSSL"
		Me.chkSSL.Properties.AllowFocused = False
		Me.chkSSL.Properties.Appearance.Options.UseTextOptions = True
		Me.chkSSL.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkSSL.Properties.Caption = "SSL"
		Me.chkSSL.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkSSL.Size = New System.Drawing.Size(57, 20)
		Me.chkSSL.TabIndex = 27
		'
		'BarManager1
		'
		Me.BarManager1.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.Bar1})
		Me.BarManager1.DockControls.Add(Me.barDockControlTop)
		Me.BarManager1.DockControls.Add(Me.barDockControlBottom)
		Me.BarManager1.DockControls.Add(Me.barDockControlLeft)
		Me.BarManager1.DockControls.Add(Me.barDockControlRight)
		Me.BarManager1.Form = Me
		Me.BarManager1.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.bsiInfo_1, Me.bsiInfo_2, Me.bsiInfo_3, Me.bsiInfo_4})
		Me.BarManager1.MaxItemId = 4
		Me.BarManager1.StatusBar = Me.Bar1
		'
		'Bar1
		'
		Me.Bar1.BarName = "Benutzerdefiniert 2"
		Me.Bar1.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
		Me.Bar1.DockCol = 0
		Me.Bar1.DockRow = 0
		Me.Bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
		Me.Bar1.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo_1), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo_2), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo_3), New DevExpress.XtraBars.LinkPersistInfo(Me.bsiInfo_4)})
		Me.Bar1.OptionsBar.AllowQuickCustomization = False
		Me.Bar1.OptionsBar.DrawDragBorder = False
		Me.Bar1.OptionsBar.UseWholeRow = True
		Me.Bar1.Text = "Benutzerdefiniert 2"
		'
		'bsiInfo_1
		'
		Me.bsiInfo_1.Caption = " "
		Me.bsiInfo_1.Id = 0
		Me.bsiInfo_1.Name = "bsiInfo_1"
		'
		'bsiInfo_2
		'
		Me.bsiInfo_2.Caption = " "
		Me.bsiInfo_2.Id = 1
		Me.bsiInfo_2.Name = "bsiInfo_2"
		'
		'bsiInfo_3
		'
		Me.bsiInfo_3.Caption = " "
		Me.bsiInfo_3.Id = 2
		Me.bsiInfo_3.Name = "bsiInfo_3"
		'
		'bsiInfo_4
		'
		Me.bsiInfo_4.Caption = " "
		Me.bsiInfo_4.Id = 3
		Me.bsiInfo_4.Name = "bsiInfo_4"
		'
		'barDockControlTop
		'
		Me.barDockControlTop.CausesValidation = False
		Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
		Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlTop.Manager = Me.BarManager1
		Me.barDockControlTop.Size = New System.Drawing.Size(1710, 0)
		'
		'barDockControlBottom
		'
		Me.barDockControlBottom.CausesValidation = False
		Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.barDockControlBottom.Location = New System.Drawing.Point(0, 736)
		Me.barDockControlBottom.Manager = Me.BarManager1
		Me.barDockControlBottom.Size = New System.Drawing.Size(1710, 22)
		'
		'barDockControlLeft
		'
		Me.barDockControlLeft.CausesValidation = False
		Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
		Me.barDockControlLeft.Location = New System.Drawing.Point(0, 0)
		Me.barDockControlLeft.Manager = Me.BarManager1
		Me.barDockControlLeft.Size = New System.Drawing.Size(0, 736)
		'
		'barDockControlRight
		'
		Me.barDockControlRight.CausesValidation = False
		Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
		Me.barDockControlRight.Location = New System.Drawing.Point(1710, 0)
		Me.barDockControlRight.Manager = Me.BarManager1
		Me.barDockControlRight.Size = New System.Drawing.Size(0, 736)
		'
		'Label20
		'
		Me.Label20.AutoSize = True
		Me.Label20.Location = New System.Drawing.Point(26, 30)
		Me.Label20.Name = "Label20"
		Me.Label20.Size = New System.Drawing.Size(138, 13)
		Me.Label20.TabIndex = 24
		Me.Label20.Text = "EMail-Server (Smtp-Server)"
		'
		'txtSMTP
		'
		Me.txtSMTP.Location = New System.Drawing.Point(170, 26)
		Me.txtSMTP.Name = "txtSMTP"
		Me.txtSMTP.Size = New System.Drawing.Size(233, 20)
		Me.txtSMTP.TabIndex = 0
		'
		'Label21
		'
		Me.Label21.AutoSize = True
		Me.Label21.Location = New System.Drawing.Point(414, 30)
		Me.Label21.Name = "Label21"
		Me.Label21.Size = New System.Drawing.Size(27, 13)
		Me.Label21.TabIndex = 26
		Me.Label21.Text = "Port"
		'
		'txtSMTPPort
		'
		Me.txtSMTPPort.Location = New System.Drawing.Point(447, 26)
		Me.txtSMTPPort.Name = "txtSMTPPort"
		Me.txtSMTPPort.Size = New System.Drawing.Size(35, 20)
		Me.txtSMTPPort.TabIndex = 1
		'
		'cboSmtpDeliveryMethod
		'
		Me.cboSmtpDeliveryMethod.Location = New System.Drawing.Point(170, 52)
		Me.cboSmtpDeliveryMethod.Name = "cboSmtpDeliveryMethod"
		Me.cboSmtpDeliveryMethod.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboSmtpDeliveryMethod.Properties.NullText = ""
		Me.cboSmtpDeliveryMethod.Size = New System.Drawing.Size(233, 20)
		Me.cboSmtpDeliveryMethod.TabIndex = 28
		'
		'GroupBox2
		'
		Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox2.Controls.Add(Me.chkJobChannelPriority)
		Me.GroupBox2.Controls.Add(Me.chksendproposeattachmenttowos)
		Me.GroupBox2.Controls.Add(Me.chkenablecustomerwos)
		Me.GroupBox2.Controls.Add(Me.chkenableemployeewos)
		Me.GroupBox2.Controls.Add(Me.chkenablevacancywos)
		Me.GroupBox2.Controls.Add(Me.txt_Ver_ID)
		Me.GroupBox2.Controls.Add(Me.Label14)
		Me.GroupBox2.Controls.Add(Me.txt_KD_ID)
		Me.GroupBox2.Controls.Add(Me.Label12)
		Me.GroupBox2.Controls.Add(Me.txt_MA_ID)
		Me.GroupBox2.Controls.Add(Me.Label11)
		Me.GroupBox2.Controls.Add(Me.txt_Vak_ID)
		Me.GroupBox2.Controls.Add(Me.Label10)
		Me.GroupBox2.Location = New System.Drawing.Point(20, 40)
		Me.GroupBox2.Name = "GroupBox2"
		Me.GroupBox2.Size = New System.Drawing.Size(243, 151)
		Me.GroupBox2.TabIndex = 25
		Me.GroupBox2.TabStop = False
		Me.GroupBox2.Text = "Transfer-ID"
		'
		'chkJobChannelPriority
		'
		Me.chkJobChannelPriority.Location = New System.Drawing.Point(513, 33)
		Me.chkJobChannelPriority.MenuManager = Me.BarManager1
		Me.chkJobChannelPriority.Name = "chkJobChannelPriority"
		Me.chkJobChannelPriority.Properties.AllowFocused = False
		Me.chkJobChannelPriority.Properties.Caption = "JobChannelPriority"
		Me.chkJobChannelPriority.Size = New System.Drawing.Size(151, 20)
		Me.chkJobChannelPriority.TabIndex = 35
		'
		'chksendproposeattachmenttowos
		'
		Me.chksendproposeattachmenttowos.Location = New System.Drawing.Point(404, 109)
		Me.chksendproposeattachmenttowos.MenuManager = Me.BarManager1
		Me.chksendproposeattachmenttowos.Name = "chksendproposeattachmenttowos"
		Me.chksendproposeattachmenttowos.Properties.AllowFocused = False
		Me.chksendproposeattachmenttowos.Properties.Caption = "Vorschlag-Anhänge"
		Me.chksendproposeattachmenttowos.Size = New System.Drawing.Size(253, 20)
		Me.chksendproposeattachmenttowos.TabIndex = 34
		'
		'chkenablecustomerwos
		'
		Me.chkenablecustomerwos.Location = New System.Drawing.Point(404, 84)
		Me.chkenablecustomerwos.MenuManager = Me.BarManager1
		Me.chkenablecustomerwos.Name = "chkenablecustomerwos"
		Me.chkenablecustomerwos.Properties.AllowFocused = False
		Me.chkenablecustomerwos.Properties.Caption = "Kunden und Zuständige Personen"
		Me.chkenablecustomerwos.Size = New System.Drawing.Size(253, 20)
		Me.chkenablecustomerwos.TabIndex = 33
		'
		'chkenableemployeewos
		'
		Me.chkenableemployeewos.Location = New System.Drawing.Point(404, 59)
		Me.chkenableemployeewos.MenuManager = Me.BarManager1
		Me.chkenableemployeewos.Name = "chkenableemployeewos"
		Me.chkenableemployeewos.Properties.AllowFocused = False
		Me.chkenableemployeewos.Properties.Caption = "Kandidaten"
		Me.chkenableemployeewos.Size = New System.Drawing.Size(253, 20)
		Me.chkenableemployeewos.TabIndex = 32
		'
		'chkenablevacancywos
		'
		Me.chkenablevacancywos.Location = New System.Drawing.Point(404, 33)
		Me.chkenablevacancywos.MenuManager = Me.BarManager1
		Me.chkenablevacancywos.Name = "chkenablevacancywos"
		Me.chkenablevacancywos.Properties.AllowFocused = False
		Me.chkenablevacancywos.Properties.Caption = "Vakanzen"
		Me.chkenablevacancywos.Size = New System.Drawing.Size(89, 20)
		Me.chkenablevacancywos.TabIndex = 31
		'
		'txt_Ver_ID
		'
		Me.txt_Ver_ID.Location = New System.Drawing.Point(144, 111)
		Me.txt_Ver_ID.Name = "txt_Ver_ID"
		Me.txt_Ver_ID.Size = New System.Drawing.Size(254, 20)
		Me.txt_Ver_ID.TabIndex = 3
		'
		'Label14
		'
		Me.Label14.AutoSize = True
		Me.Label14.Location = New System.Drawing.Point(22, 110)
		Me.Label14.Name = "Label14"
		Me.Label14.Size = New System.Drawing.Size(75, 13)
		Me.Label14.TabIndex = 30
		Me.Label14.Text = "Verleihvertrag"
		'
		'txt_KD_ID
		'
		Me.txt_KD_ID.Location = New System.Drawing.Point(144, 85)
		Me.txt_KD_ID.Name = "txt_KD_ID"
		Me.txt_KD_ID.Size = New System.Drawing.Size(254, 20)
		Me.txt_KD_ID.TabIndex = 2
		'
		'Label12
		'
		Me.Label12.AutoSize = True
		Me.Label12.Location = New System.Drawing.Point(22, 84)
		Me.Label12.Name = "Label12"
		Me.Label12.Size = New System.Drawing.Size(43, 13)
		Me.Label12.TabIndex = 28
		Me.Label12.Text = "Kunden"
		'
		'txt_MA_ID
		'
		Me.txt_MA_ID.Location = New System.Drawing.Point(144, 59)
		Me.txt_MA_ID.Name = "txt_MA_ID"
		Me.txt_MA_ID.Size = New System.Drawing.Size(254, 20)
		Me.txt_MA_ID.TabIndex = 1
		'
		'Label11
		'
		Me.Label11.AutoSize = True
		Me.Label11.Location = New System.Drawing.Point(22, 58)
		Me.Label11.Name = "Label11"
		Me.Label11.Size = New System.Drawing.Size(61, 13)
		Me.Label11.TabIndex = 26
		Me.Label11.Text = "Kandidaten"
		'
		'txt_Vak_ID
		'
		Me.txt_Vak_ID.Location = New System.Drawing.Point(144, 33)
		Me.txt_Vak_ID.Name = "txt_Vak_ID"
		Me.txt_Vak_ID.Size = New System.Drawing.Size(254, 20)
		Me.txt_Vak_ID.TabIndex = 0
		'
		'Label10
		'
		Me.Label10.AutoSize = True
		Me.Label10.Location = New System.Drawing.Point(22, 32)
		Me.Label10.Name = "Label10"
		Me.Label10.Size = New System.Drawing.Size(53, 13)
		Me.Label10.TabIndex = 24
		Me.Label10.Text = "Vakanzen"
		'
		'GroupBox7
		'
		Me.GroupBox7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox7.Controls.Add(Me.lblTemplatePath_1)
		Me.GroupBox7.Controls.Add(Me.Label22)
		Me.GroupBox7.Controls.Add(Me.txtCockpitPicture)
		Me.GroupBox7.Controls.Add(Me.Label15)
		Me.GroupBox7.Controls.Add(Me.txtCockpitWWW)
		Me.GroupBox7.Location = New System.Drawing.Point(20, 198)
		Me.GroupBox7.Name = "GroupBox7"
		Me.GroupBox7.Size = New System.Drawing.Size(243, 86)
		Me.GroupBox7.TabIndex = 1
		Me.GroupBox7.TabStop = False
		Me.GroupBox7.Text = "Variable für MyCockpit"
		'
		'lblTemplatePath_1
		'
		Me.lblTemplatePath_1.AutoEllipsis = True
		Me.lblTemplatePath_1.Location = New System.Drawing.Point(141, 53)
		Me.lblTemplatePath_1.Name = "lblTemplatePath_1"
		Me.lblTemplatePath_1.Size = New System.Drawing.Size(273, 13)
		Me.lblTemplatePath_1.TabIndex = 52
		Me.lblTemplatePath_1.Text = "\\"
		'
		'Label22
		'
		Me.Label22.AutoSize = True
		Me.Label22.Location = New System.Drawing.Point(18, 53)
		Me.Label22.Name = "Label22"
		Me.Label22.Size = New System.Drawing.Size(82, 13)
		Me.Label22.TabIndex = 48
		Me.Label22.Text = "Bild auf 1. Seite"
		'
		'txtCockpitPicture
		'
		Me.txtCockpitPicture.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtCockpitPicture.Location = New System.Drawing.Point(420, 49)
		Me.txtCockpitPicture.Name = "txtCockpitPicture"
		Me.txtCockpitPicture.Size = New System.Drawing.Size(0, 20)
		Me.txtCockpitPicture.TabIndex = 1
		'
		'Label15
		'
		Me.Label15.AutoSize = True
		Me.Label15.Location = New System.Drawing.Point(17, 27)
		Me.Label15.Name = "Label15"
		Me.Label15.Size = New System.Drawing.Size(74, 13)
		Me.Label15.TabIndex = 46
		Me.Label15.Text = "WOS-Adresse"
		'
		'txtCockpitWWW
		'
		Me.txtCockpitWWW.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtCockpitWWW.Location = New System.Drawing.Point(144, 23)
		Me.txtCockpitWWW.Name = "txtCockpitWWW"
		Me.txtCockpitWWW.Size = New System.Drawing.Size(81, 20)
		Me.txtCockpitWWW.TabIndex = 0
		'
		'GroupBox3
		'
		Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox3.Controls.Add(Me.Label23)
		Me.GroupBox3.Controls.Add(Me.txtZHDDocWWW)
		Me.GroupBox3.Controls.Add(Me.Label24)
		Me.GroupBox3.Controls.Add(Me.txtZHDDocMailBetreff)
		Me.GroupBox3.Controls.Add(Me.Label18)
		Me.GroupBox3.Controls.Add(Me.txtKDDocWWW)
		Me.GroupBox3.Controls.Add(Me.Label16)
		Me.GroupBox3.Controls.Add(Me.txtKDDocMailBetreff)
		Me.GroupBox3.Location = New System.Drawing.Point(26, 242)
		Me.GroupBox3.Name = "GroupBox3"
		Me.GroupBox3.Size = New System.Drawing.Size(187, 143)
		Me.GroupBox3.TabIndex = 1
		Me.GroupBox3.TabStop = False
		Me.GroupBox3.Text = "Variable für WOS (Kunden und Zustängdige Personen)"
		'
		'Label23
		'
		Me.Label23.AutoSize = True
		Me.Label23.Location = New System.Drawing.Point(19, 110)
		Me.Label23.Name = "Label23"
		Me.Label23.Size = New System.Drawing.Size(103, 13)
		Me.Label23.TabIndex = 51
		Me.Label23.Text = "Web-Adresse (ZHD)"
		'
		'txtZHDDocWWW
		'
		Me.txtZHDDocWWW.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtZHDDocWWW.Location = New System.Drawing.Point(146, 106)
		Me.txtZHDDocWWW.Name = "txtZHDDocWWW"
		Me.txtZHDDocWWW.Size = New System.Drawing.Size(22, 20)
		Me.txtZHDDocWWW.TabIndex = 7
		'
		'Label24
		'
		Me.Label24.AutoSize = True
		Me.Label24.Location = New System.Drawing.Point(19, 84)
		Me.Label24.Name = "Label24"
		Me.Label24.Size = New System.Drawing.Size(100, 13)
		Me.Label24.TabIndex = 50
		Me.Label24.Text = "EMail-Betreff (ZHD)"
		'
		'txtZHDDocMailBetreff
		'
		Me.txtZHDDocMailBetreff.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtZHDDocMailBetreff.Location = New System.Drawing.Point(146, 80)
		Me.txtZHDDocMailBetreff.Name = "txtZHDDocMailBetreff"
		Me.txtZHDDocMailBetreff.Size = New System.Drawing.Size(22, 20)
		Me.txtZHDDocMailBetreff.TabIndex = 6
		'
		'Label18
		'
		Me.Label18.AutoSize = True
		Me.Label18.Location = New System.Drawing.Point(19, 58)
		Me.Label18.Name = "Label18"
		Me.Label18.Size = New System.Drawing.Size(96, 13)
		Me.Label18.TabIndex = 44
		Me.Label18.Text = "Web-Adresse (KD)"
		'
		'txtKDDocWWW
		'
		Me.txtKDDocWWW.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtKDDocWWW.Location = New System.Drawing.Point(146, 54)
		Me.txtKDDocWWW.Name = "txtKDDocWWW"
		Me.txtKDDocWWW.Size = New System.Drawing.Size(22, 20)
		Me.txtKDDocWWW.TabIndex = 5
		'
		'Label16
		'
		Me.Label16.AutoSize = True
		Me.Label16.Location = New System.Drawing.Point(19, 32)
		Me.Label16.Name = "Label16"
		Me.Label16.Size = New System.Drawing.Size(93, 13)
		Me.Label16.TabIndex = 40
		Me.Label16.Text = "EMail-Betreff (KD)"
		'
		'txtKDDocMailBetreff
		'
		Me.txtKDDocMailBetreff.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtKDDocMailBetreff.Location = New System.Drawing.Point(146, 28)
		Me.txtKDDocMailBetreff.Name = "txtKDDocMailBetreff"
		Me.txtKDDocMailBetreff.Size = New System.Drawing.Size(22, 20)
		Me.txtKDDocMailBetreff.TabIndex = 4
		'
		'GroupBox6
		'
		Me.GroupBox6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox6.Controls.Add(Me.txtMADocWWW)
		Me.GroupBox6.Controls.Add(Me.txtMADocMailBetreff)
		Me.GroupBox6.Controls.Add(Me.Label19)
		Me.GroupBox6.Controls.Add(Me.Label17)
		Me.GroupBox6.Location = New System.Drawing.Point(26, 141)
		Me.GroupBox6.Name = "GroupBox6"
		Me.GroupBox6.Size = New System.Drawing.Size(187, 89)
		Me.GroupBox6.TabIndex = 0
		Me.GroupBox6.TabStop = False
		Me.GroupBox6.Text = "Variable für WOS (Kandidaten)"
		'
		'txtMADocWWW
		'
		Me.txtMADocWWW.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtMADocWWW.Location = New System.Drawing.Point(144, 52)
		Me.txtMADocWWW.Name = "txtMADocWWW"
		Me.txtMADocWWW.Size = New System.Drawing.Size(24, 20)
		Me.txtMADocWWW.TabIndex = 3
		'
		'txtMADocMailBetreff
		'
		Me.txtMADocMailBetreff.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtMADocMailBetreff.Location = New System.Drawing.Point(144, 26)
		Me.txtMADocMailBetreff.Name = "txtMADocMailBetreff"
		Me.txtMADocMailBetreff.Size = New System.Drawing.Size(24, 20)
		Me.txtMADocMailBetreff.TabIndex = 2
		'
		'Label19
		'
		Me.Label19.AutoSize = True
		Me.Label19.Location = New System.Drawing.Point(17, 56)
		Me.Label19.Name = "Label19"
		Me.Label19.Size = New System.Drawing.Size(72, 13)
		Me.Label19.TabIndex = 43
		Me.Label19.Text = "Web-Adresse"
		'
		'Label17
		'
		Me.Label17.AutoSize = True
		Me.Label17.Location = New System.Drawing.Point(17, 30)
		Me.Label17.Name = "Label17"
		Me.Label17.Size = New System.Drawing.Size(69, 13)
		Me.Label17.TabIndex = 38
		Me.Label17.Text = "EMail-Betreff"
		'
		'GroupBox5
		'
		Me.GroupBox5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox5.Controls.Add(Me.Label29)
		Me.GroupBox5.Controls.Add(Me.txtArbgDocMailBetreff)
		Me.GroupBox5.Controls.Add(Me.Label26)
		Me.GroupBox5.Controls.Add(Me.txtZVDocMailBetreff)
		Me.GroupBox5.Location = New System.Drawing.Point(26, 40)
		Me.GroupBox5.Name = "GroupBox5"
		Me.GroupBox5.Size = New System.Drawing.Size(187, 89)
		Me.GroupBox5.TabIndex = 0
		Me.GroupBox5.TabStop = False
		Me.GroupBox5.Text = "Variable für ZV und Arbeitgeberbescheinigung"
		'
		'Label29
		'
		Me.Label29.AutoSize = True
		Me.Label29.Location = New System.Drawing.Point(17, 56)
		Me.Label29.Name = "Label29"
		Me.Label29.Size = New System.Drawing.Size(101, 13)
		Me.Label29.TabIndex = 51
		Me.Label29.Text = "EMail-Betreff (ARG)"
		'
		'txtArbgDocMailBetreff
		'
		Me.txtArbgDocMailBetreff.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtArbgDocMailBetreff.Location = New System.Drawing.Point(144, 52)
		Me.txtArbgDocMailBetreff.Name = "txtArbgDocMailBetreff"
		Me.txtArbgDocMailBetreff.Size = New System.Drawing.Size(24, 20)
		Me.txtArbgDocMailBetreff.TabIndex = 1
		'
		'Label26
		'
		Me.Label26.AutoSize = True
		Me.Label26.Location = New System.Drawing.Point(17, 30)
		Me.Label26.Name = "Label26"
		Me.Label26.Size = New System.Drawing.Size(92, 13)
		Me.Label26.TabIndex = 46
		Me.Label26.Text = "EMail-Betreff (ZV)"
		'
		'txtZVDocMailBetreff
		'
		Me.txtZVDocMailBetreff.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtZVDocMailBetreff.Location = New System.Drawing.Point(144, 26)
		Me.txtZVDocMailBetreff.Name = "txtZVDocMailBetreff"
		Me.txtZVDocMailBetreff.Size = New System.Drawing.Size(24, 20)
		Me.txtZVDocMailBetreff.TabIndex = 0
		'
		'cleLNormal_0
		'
		Me.cleLNormal_0.EditValue = System.Drawing.Color.Empty
		Me.cleLNormal_0.Location = New System.Drawing.Point(126, 11)
		Me.cleLNormal_0.Name = "cleLNormal_0"
		Me.cleLNormal_0.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cleLNormal_0.Size = New System.Drawing.Size(189, 20)
		Me.cleLNormal_0.TabIndex = 194
		Me.cleLNormal_0.Visible = False
		'
		'Label51
		'
		Me.Label51.AutoSize = True
		Me.Label51.Location = New System.Drawing.Point(13, 14)
		Me.Label51.Name = "Label51"
		Me.Label51.Size = New System.Drawing.Size(64, 13)
		Me.Label51.TabIndex = 183
		Me.Label51.Text = "Pflichtfelder"
		Me.Label51.Visible = False
		'
		'Label54
		'
		Me.Label54.AutoSize = True
		Me.Label54.Location = New System.Drawing.Point(13, 40)
		Me.Label54.Name = "Label54"
		Me.Label54.Size = New System.Drawing.Size(74, 13)
		Me.Label54.TabIndex = 190
		Me.Label54.Text = "Fensterlayout"
		'
		'cboLFormStyle
		'
		Me.cboLFormStyle.Location = New System.Drawing.Point(126, 37)
		Me.cboLFormStyle.Name = "cboLFormStyle"
		Me.cboLFormStyle.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboLFormStyle.Size = New System.Drawing.Size(189, 20)
		Me.cboLFormStyle.TabIndex = 2
		'
		'pGlobal
		'
		Me.pGlobal.Controls.Add(Me.gGlobal)
		Me.pGlobal.Location = New System.Drawing.Point(873, 96)
		Me.pGlobal.Name = "pGlobal"
		Me.pGlobal.Size = New System.Drawing.Size(653, 101)
		Me.pGlobal.TabIndex = 184
		Me.pGlobal.Visible = False
		'
		'gGlobal
		'
		Me.gGlobal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.gGlobal.Controls.Add(Me.GroupPanel10)
		Me.gGlobal.Controls.Add(Me.GroupPanel9)
		Me.gGlobal.Controls.Add(Me.GroupPanel8)
		Me.gGlobal.Location = New System.Drawing.Point(11, 15)
		Me.gGlobal.Name = "gGlobal"
		Me.gGlobal.Size = New System.Drawing.Size(631, 73)
		Me.gGlobal.TabIndex = 1
		Me.gGlobal.Text = "Globale Einstellungen"
		'
		'GroupPanel10
		'
		Me.GroupPanel10.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel10.CanvasColor = System.Drawing.Color.Transparent
		Me.GroupPanel10.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.GroupPanel10.Controls.Add(Me.chkopeninvoiceformmorethanonce)
		Me.GroupPanel10.Controls.Add(Me.chkopenadvancedpaymentformmorethanonce)
		Me.GroupPanel10.Controls.Add(Me.chkopenreportsformmorethanonce)
		Me.GroupPanel10.Controls.Add(Me.chkopeneinsatzformmorethanonce)
		Me.GroupPanel10.Controls.Add(Me.chkopencustomerformmorethanonce)
		Me.GroupPanel10.Controls.Add(Me.chkopenemployeeformmorethanonce)
		Me.GroupPanel10.Location = New System.Drawing.Point(20, 272)
		Me.GroupPanel10.Name = "GroupPanel10"
		Me.GroupPanel10.Size = New System.Drawing.Size(260, 188)
		'
		'
		'
		Me.GroupPanel10.Style.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel10.Style.BackColor2 = System.Drawing.Color.Transparent
		Me.GroupPanel10.Style.BackColorGradientAngle = 90
		Me.GroupPanel10.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel10.Style.BorderBottomWidth = 1
		Me.GroupPanel10.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.GroupPanel10.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel10.Style.BorderLeftWidth = 1
		Me.GroupPanel10.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel10.Style.BorderRightWidth = 1
		Me.GroupPanel10.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel10.Style.BorderTopWidth = 1
		Me.GroupPanel10.Style.CornerDiameter = 4
		Me.GroupPanel10.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.GroupPanel10.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.GroupPanel10.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.GroupPanel10.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.GroupPanel10.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.GroupPanel10.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.GroupPanel10.TabIndex = 196
		Me.GroupPanel10.Text = "Gleichzeitiges Modulstart"
		'
		'chkopeninvoiceformmorethanonce
		'
		Me.chkopeninvoiceformmorethanonce.Location = New System.Drawing.Point(16, 135)
		Me.chkopeninvoiceformmorethanonce.MenuManager = Me.BarManager1
		Me.chkopeninvoiceformmorethanonce.Name = "chkopeninvoiceformmorethanonce"
		Me.chkopeninvoiceformmorethanonce.Properties.Caption = "Öffnen mehrere Rechnungen (2 x)"
		Me.chkopeninvoiceformmorethanonce.Size = New System.Drawing.Size(206, 20)
		Me.chkopeninvoiceformmorethanonce.TabIndex = 5
		'
		'chkopenadvancedpaymentformmorethanonce
		'
		Me.chkopenadvancedpaymentformmorethanonce.Location = New System.Drawing.Point(15, 110)
		Me.chkopenadvancedpaymentformmorethanonce.MenuManager = Me.BarManager1
		Me.chkopenadvancedpaymentformmorethanonce.Name = "chkopenadvancedpaymentformmorethanonce"
		Me.chkopenadvancedpaymentformmorethanonce.Properties.Caption = "Öffnen mehrere Vorschüsse (2 x)"
		Me.chkopenadvancedpaymentformmorethanonce.Size = New System.Drawing.Size(206, 20)
		Me.chkopenadvancedpaymentformmorethanonce.TabIndex = 4
		'
		'chkopenreportsformmorethanonce
		'
		Me.chkopenreportsformmorethanonce.Location = New System.Drawing.Point(15, 85)
		Me.chkopenreportsformmorethanonce.MenuManager = Me.BarManager1
		Me.chkopenreportsformmorethanonce.Name = "chkopenreportsformmorethanonce"
		Me.chkopenreportsformmorethanonce.Properties.Caption = "Öffnen mehrere Rapporte (2 x)"
		Me.chkopenreportsformmorethanonce.Size = New System.Drawing.Size(206, 20)
		Me.chkopenreportsformmorethanonce.TabIndex = 3
		'
		'chkopeneinsatzformmorethanonce
		'
		Me.chkopeneinsatzformmorethanonce.Location = New System.Drawing.Point(15, 60)
		Me.chkopeneinsatzformmorethanonce.MenuManager = Me.BarManager1
		Me.chkopeneinsatzformmorethanonce.Name = "chkopeneinsatzformmorethanonce"
		Me.chkopeneinsatzformmorethanonce.Properties.Caption = "Öffnen mehrere Einsätze (2 x)"
		Me.chkopeneinsatzformmorethanonce.Size = New System.Drawing.Size(206, 20)
		Me.chkopeneinsatzformmorethanonce.TabIndex = 2
		'
		'chkopencustomerformmorethanonce
		'
		Me.chkopencustomerformmorethanonce.Location = New System.Drawing.Point(15, 35)
		Me.chkopencustomerformmorethanonce.MenuManager = Me.BarManager1
		Me.chkopencustomerformmorethanonce.Name = "chkopencustomerformmorethanonce"
		Me.chkopencustomerformmorethanonce.Properties.Caption = "Öffnen mehrere Kunden (2 x)"
		Me.chkopencustomerformmorethanonce.Size = New System.Drawing.Size(206, 20)
		Me.chkopencustomerformmorethanonce.TabIndex = 1
		'
		'chkopenemployeeformmorethanonce
		'
		Me.chkopenemployeeformmorethanonce.Location = New System.Drawing.Point(15, 10)
		Me.chkopenemployeeformmorethanonce.MenuManager = Me.BarManager1
		Me.chkopenemployeeformmorethanonce.Name = "chkopenemployeeformmorethanonce"
		Me.chkopenemployeeformmorethanonce.Properties.Caption = "Öffnen mehrere Kandidaten (2 x)"
		Me.chkopenemployeeformmorethanonce.Size = New System.Drawing.Size(206, 20)
		Me.chkopenemployeeformmorethanonce.TabIndex = 0
		'
		'GroupPanel9
		'
		Me.GroupPanel9.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel9.CanvasColor = System.Drawing.Color.Transparent
		Me.GroupPanel9.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.GroupPanel9.Controls.Add(Me.chkAllowAutoFilterConditionChange)
		Me.GroupPanel9.Controls.Add(Me.Label61)
		Me.GroupPanel9.Controls.Add(Me.Label53)
		Me.GroupPanel9.Controls.Add(Me.ceMandantColor)
		Me.GroupPanel9.Controls.Add(Me.Label97)
		Me.GroupPanel9.Controls.Add(Me.chkGLLDebug)
		Me.GroupPanel9.Controls.Add(Me.cboautofilterconditionnr)
		Me.GroupPanel9.Controls.Add(Me.cboAutoFilterConditionDate)
		Me.GroupPanel9.Location = New System.Drawing.Point(298, 272)
		Me.GroupPanel9.Name = "GroupPanel9"
		Me.GroupPanel9.Size = New System.Drawing.Size(325, 188)
		'
		'
		'
		Me.GroupPanel9.Style.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel9.Style.BackColor2 = System.Drawing.Color.Transparent
		Me.GroupPanel9.Style.BackColorGradientAngle = 90
		Me.GroupPanel9.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel9.Style.BorderBottomWidth = 1
		Me.GroupPanel9.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.GroupPanel9.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel9.Style.BorderLeftWidth = 1
		Me.GroupPanel9.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel9.Style.BorderRightWidth = 1
		Me.GroupPanel9.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel9.Style.BorderTopWidth = 1
		Me.GroupPanel9.Style.CornerDiameter = 4
		Me.GroupPanel9.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.GroupPanel9.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.GroupPanel9.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.GroupPanel9.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.GroupPanel9.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.GroupPanel9.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.GroupPanel9.TabIndex = 195
		Me.GroupPanel9.Text = "Sonstige Einstellungen"
		'
		'chkAllowAutoFilterConditionChange
		'
		Me.chkAllowAutoFilterConditionChange.Location = New System.Drawing.Point(102, 113)
		Me.chkAllowAutoFilterConditionChange.MenuManager = Me.BarManager1
		Me.chkAllowAutoFilterConditionChange.Name = "chkAllowAutoFilterConditionChange"
		Me.chkAllowAutoFilterConditionChange.Properties.Appearance.Options.UseTextOptions = True
		Me.chkAllowAutoFilterConditionChange.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkAllowAutoFilterConditionChange.Properties.Caption = "AllowAutoFilterConditionChange"
		Me.chkAllowAutoFilterConditionChange.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkAllowAutoFilterConditionChange.Size = New System.Drawing.Size(211, 20)
		Me.chkAllowAutoFilterConditionChange.TabIndex = 37
		'
		'Label61
		'
		Me.Label61.AutoSize = True
		Me.Label61.Location = New System.Drawing.Point(23, 91)
		Me.Label61.Name = "Label61"
		Me.Label61.Size = New System.Drawing.Size(133, 13)
		Me.Label61.TabIndex = 36
		Me.Label61.Text = "AutoFilterCondition (Date)"
		'
		'Label53
		'
		Me.Label53.AutoSize = True
		Me.Label53.Location = New System.Drawing.Point(22, 65)
		Me.Label53.Name = "Label53"
		Me.Label53.Size = New System.Drawing.Size(121, 13)
		Me.Label53.TabIndex = 34
		Me.Label53.Text = "AutoFilterCondition (Nr)"
		'
		'ceMandantColor
		'
		Me.ceMandantColor.EditValue = System.Drawing.Color.Empty
		Me.ceMandantColor.Location = New System.Drawing.Point(173, 35)
		Me.ceMandantColor.Name = "ceMandantColor"
		SerializableAppearanceObject1.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject1.Options.UseForeColor = True
		SerializableAppearanceObject2.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject2.Options.UseForeColor = True
		SerializableAppearanceObject3.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject3.Options.UseForeColor = True
		SerializableAppearanceObject4.ForeColor = System.Drawing.Color.Red
		SerializableAppearanceObject4.Options.UseForeColor = True
		Me.ceMandantColor.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo), New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", -1, True, True, False, EditorButtonImageOptions1, New DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), SerializableAppearanceObject1, SerializableAppearanceObject2, SerializableAppearanceObject3, SerializableAppearanceObject4, "", Nothing, Nothing, DevExpress.Utils.ToolTipAnchor.[Default])})
		Me.ceMandantColor.Properties.ShowColorDialog = False
		Me.ceMandantColor.Properties.ShowCustomColors = False
		Me.ceMandantColor.Size = New System.Drawing.Size(140, 20)
		Me.ceMandantColor.TabIndex = 31
		'
		'Label97
		'
		Me.Label97.AutoSize = True
		Me.Label97.Location = New System.Drawing.Point(20, 39)
		Me.Label97.Name = "Label97"
		Me.Label97.Size = New System.Drawing.Size(87, 13)
		Me.Label97.TabIndex = 32
		Me.Label97.Text = "Mandantenfarbe"
		'
		'chkGLLDebug
		'
		Me.chkGLLDebug.Location = New System.Drawing.Point(102, 10)
		Me.chkGLLDebug.MenuManager = Me.BarManager1
		Me.chkGLLDebug.Name = "chkGLLDebug"
		Me.chkGLLDebug.Properties.Appearance.Options.UseTextOptions = True
		Me.chkGLLDebug.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkGLLDebug.Properties.Caption = "Druckausgabe debugen"
		Me.chkGLLDebug.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkGLLDebug.Size = New System.Drawing.Size(211, 20)
		Me.chkGLLDebug.TabIndex = 0
		'
		'cboautofilterconditionnr
		'
		Me.cboautofilterconditionnr.EditValue = ""
		Me.cboautofilterconditionnr.Location = New System.Drawing.Point(173, 61)
		Me.cboautofilterconditionnr.Name = "cboautofilterconditionnr"
		Me.cboautofilterconditionnr.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboautofilterconditionnr.Properties.NullText = ""
		Me.cboautofilterconditionnr.Properties.PopupSizeable = False
		Me.cboautofilterconditionnr.Size = New System.Drawing.Size(140, 20)
		Me.cboautofilterconditionnr.TabIndex = 33
		'
		'cboAutoFilterConditionDate
		'
		Me.cboAutoFilterConditionDate.EditValue = ""
		Me.cboAutoFilterConditionDate.Location = New System.Drawing.Point(173, 87)
		Me.cboAutoFilterConditionDate.Name = "cboAutoFilterConditionDate"
		Me.cboAutoFilterConditionDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboAutoFilterConditionDate.Properties.NullText = ""
		Me.cboAutoFilterConditionDate.Properties.PopupSizeable = False
		Me.cboAutoFilterConditionDate.Size = New System.Drawing.Size(140, 20)
		Me.cboAutoFilterConditionDate.TabIndex = 35
		'
		'GroupPanel8
		'
		Me.GroupPanel8.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel8.CanvasColor = System.Drawing.Color.Transparent
		Me.GroupPanel8.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.GroupPanel8.Controls.Add(Me.chksavecustomerinvoicescanintowos)
		Me.GroupPanel8.Controls.Add(Me.chksavecustomerreportscanintowos)
		Me.GroupPanel8.Controls.Add(Me.chksavecustomeremploymentscanintowos)
		Me.GroupPanel8.Controls.Add(Me.chksaveemployeepayrollscanintowos)
		Me.GroupPanel8.Controls.Add(Me.chksaveemployeereportscanintowos)
		Me.GroupPanel8.Controls.Add(Me.chksaveemployeeemploymentscanintowos)
		Me.GroupPanel8.Controls.Add(Me.Label40)
		Me.GroupPanel8.Controls.Add(Me.chkimportscanreportzeroamount)
		Me.GroupPanel8.Controls.Add(Me.chkimportscanreporttoboth)
		Me.GroupPanel8.Controls.Add(Me.txtnotificationintervalperiodeforreport)
		Me.GroupPanel8.Controls.Add(Me.Label38)
		Me.GroupPanel8.Controls.Add(Me.chkchangeownreportforfinishingflag)
		Me.GroupPanel8.Controls.Add(Me.Label6)
		Me.GroupPanel8.Controls.Add(Me.txtnotificationintervalperiode)
		Me.GroupPanel8.Controls.Add(Me.Label2)
		Me.GroupPanel8.Controls.Add(Me.Label57)
		Me.GroupPanel8.Controls.Add(Me.seRPScanSize)
		Me.GroupPanel8.Location = New System.Drawing.Point(20, 27)
		Me.GroupPanel8.Name = "GroupPanel8"
		Me.GroupPanel8.Size = New System.Drawing.Size(603, 236)
		'
		'
		'
		Me.GroupPanel8.Style.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel8.Style.BackColor2 = System.Drawing.Color.Transparent
		Me.GroupPanel8.Style.BackColorGradientAngle = 90
		Me.GroupPanel8.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel8.Style.BorderBottomWidth = 1
		Me.GroupPanel8.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.GroupPanel8.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel8.Style.BorderLeftWidth = 1
		Me.GroupPanel8.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel8.Style.BorderRightWidth = 1
		Me.GroupPanel8.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel8.Style.BorderTopWidth = 1
		Me.GroupPanel8.Style.CornerDiameter = 4
		Me.GroupPanel8.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.GroupPanel8.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.GroupPanel8.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.GroupPanel8.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.GroupPanel8.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.GroupPanel8.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.GroupPanel8.TabIndex = 194
		Me.GroupPanel8.Text = "Scanning und Notifying"
		'
		'chksavecustomerinvoicescanintowos
		'
		Me.chksavecustomerinvoicescanintowos.Location = New System.Drawing.Point(314, 172)
		Me.chksavecustomerinvoicescanintowos.Name = "chksavecustomerinvoicescanintowos"
		Me.chksavecustomerinvoicescanintowos.Properties.Caption = "Kundenrechnungen ins WOS"
		Me.chksavecustomerinvoicescanintowos.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chksavecustomerinvoicescanintowos.Size = New System.Drawing.Size(219, 20)
		Me.chksavecustomerinvoicescanintowos.TabIndex = 208
		'
		'chksavecustomerreportscanintowos
		'
		Me.chksavecustomerreportscanintowos.Location = New System.Drawing.Point(314, 147)
		Me.chksavecustomerreportscanintowos.Name = "chksavecustomerreportscanintowos"
		Me.chksavecustomerreportscanintowos.Properties.Caption = "Kundenrapporte ins WOS"
		Me.chksavecustomerreportscanintowos.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chksavecustomerreportscanintowos.Size = New System.Drawing.Size(219, 20)
		Me.chksavecustomerreportscanintowos.TabIndex = 207
		'
		'chksavecustomeremploymentscanintowos
		'
		Me.chksavecustomeremploymentscanintowos.Location = New System.Drawing.Point(314, 122)
		Me.chksavecustomeremploymentscanintowos.Name = "chksavecustomeremploymentscanintowos"
		Me.chksavecustomeremploymentscanintowos.Properties.Caption = "Verleihverträge ins WOS"
		Me.chksavecustomeremploymentscanintowos.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chksavecustomeremploymentscanintowos.Size = New System.Drawing.Size(219, 20)
		Me.chksavecustomeremploymentscanintowos.TabIndex = 206
		'
		'chksaveemployeepayrollscanintowos
		'
		Me.chksaveemployeepayrollscanintowos.Location = New System.Drawing.Point(16, 172)
		Me.chksaveemployeepayrollscanintowos.Name = "chksaveemployeepayrollscanintowos"
		Me.chksaveemployeepayrollscanintowos.Properties.Caption = "Lohnabrechnung ins WOS"
		Me.chksaveemployeepayrollscanintowos.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chksaveemployeepayrollscanintowos.Size = New System.Drawing.Size(219, 20)
		Me.chksaveemployeepayrollscanintowos.TabIndex = 205
		'
		'chksaveemployeereportscanintowos
		'
		Me.chksaveemployeereportscanintowos.Location = New System.Drawing.Point(16, 147)
		Me.chksaveemployeereportscanintowos.Name = "chksaveemployeereportscanintowos"
		Me.chksaveemployeereportscanintowos.Properties.Caption = "Kandidatenrapporte ins WOS"
		Me.chksaveemployeereportscanintowos.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chksaveemployeereportscanintowos.Size = New System.Drawing.Size(219, 20)
		Me.chksaveemployeereportscanintowos.TabIndex = 204
		'
		'chksaveemployeeemploymentscanintowos
		'
		Me.chksaveemployeeemploymentscanintowos.Location = New System.Drawing.Point(16, 122)
		Me.chksaveemployeeemploymentscanintowos.Name = "chksaveemployeeemploymentscanintowos"
		Me.chksaveemployeeemploymentscanintowos.Properties.Caption = "Einsatzvertrag ins WOS"
		Me.chksaveemployeeemploymentscanintowos.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chksaveemployeeemploymentscanintowos.Size = New System.Drawing.Size(219, 20)
		Me.chksaveemployeeemploymentscanintowos.TabIndex = 203
		'
		'Label40
		'
		Me.Label40.AutoSize = True
		Me.Label40.Location = New System.Drawing.Point(541, 42)
		Me.Label40.Name = "Label40"
		Me.Label40.Size = New System.Drawing.Size(35, 13)
		Me.Label40.TabIndex = 202
		Me.Label40.Text = "(min.)"
		'
		'chkimportscanreportzeroamount
		'
		Me.chkimportscanreportzeroamount.Location = New System.Drawing.Point(18, 64)
		Me.chkimportscanreportzeroamount.Name = "chkimportscanreportzeroamount"
		Me.chkimportscanreportzeroamount.Properties.Caption = "Rapportzeilen mit 0-Betrag hinterlegen"
		Me.chkimportscanreportzeroamount.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkimportscanreportzeroamount.Size = New System.Drawing.Size(219, 20)
		Me.chkimportscanreportzeroamount.TabIndex = 201
		'
		'chkimportscanreporttoboth
		'
		Me.chkimportscanreporttoboth.Location = New System.Drawing.Point(18, 39)
		Me.chkimportscanreporttoboth.Name = "chkimportscanreporttoboth"
		Me.chkimportscanreporttoboth.Properties.Caption = "Gescannte Rapporte in KD und MA"
		Me.chkimportscanreporttoboth.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkimportscanreporttoboth.Size = New System.Drawing.Size(219, 20)
		Me.chkimportscanreporttoboth.TabIndex = 200
		'
		'txtnotificationintervalperiodeforreport
		'
		Me.txtnotificationintervalperiodeforreport.EditValue = ""
		Me.txtnotificationintervalperiodeforreport.Location = New System.Drawing.Point(474, 38)
		Me.txtnotificationintervalperiodeforreport.Name = "txtnotificationintervalperiodeforreport"
		Me.txtnotificationintervalperiodeforreport.Size = New System.Drawing.Size(61, 20)
		Me.txtnotificationintervalperiodeforreport.TabIndex = 198
		'
		'Label38
		'
		Me.Label38.AutoSize = True
		Me.Label38.Location = New System.Drawing.Point(316, 42)
		Me.Label38.Name = "Label38"
		Me.Label38.Size = New System.Drawing.Size(137, 13)
		Me.Label38.TabIndex = 199
		Me.Label38.Text = "Such-Interval für Rapporte"
		'
		'chkchangeownreportforfinishingflag
		'
		Me.chkchangeownreportforfinishingflag.Location = New System.Drawing.Point(316, 64)
		Me.chkchangeownreportforfinishingflag.MenuManager = Me.BarManager1
		Me.chkchangeownreportforfinishingflag.Name = "chkchangeownreportforfinishingflag"
		Me.chkchangeownreportforfinishingflag.Properties.Caption = "Eigene Rapporte kontrollieren"
		Me.chkchangeownreportforfinishingflag.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chkchangeownreportforfinishingflag.Size = New System.Drawing.Size(219, 20)
		Me.chkchangeownreportforfinishingflag.TabIndex = 197
		'
		'Label6
		'
		Me.Label6.AutoSize = True
		Me.Label6.Location = New System.Drawing.Point(541, 14)
		Me.Label6.Name = "Label6"
		Me.Label6.Size = New System.Drawing.Size(35, 13)
		Me.Label6.TabIndex = 196
		Me.Label6.Text = "(min.)"
		'
		'txtnotificationintervalperiode
		'
		Me.txtnotificationintervalperiode.EditValue = ""
		Me.txtnotificationintervalperiode.Location = New System.Drawing.Point(474, 12)
		Me.txtnotificationintervalperiode.Name = "txtnotificationintervalperiode"
		Me.txtnotificationintervalperiode.Size = New System.Drawing.Size(61, 20)
		Me.txtnotificationintervalperiode.TabIndex = 194
		'
		'Label2
		'
		Me.Label2.AutoSize = True
		Me.Label2.Location = New System.Drawing.Point(387, 15)
		Me.Label2.Name = "Label2"
		Me.Label2.Size = New System.Drawing.Size(72, 13)
		Me.Label2.TabIndex = 195
		Me.Label2.Text = "Such-Interval"
		'
		'Label57
		'
		Me.Label57.AutoSize = True
		Me.Label57.Location = New System.Drawing.Point(18, 13)
		Me.Label57.Name = "Label57"
		Me.Label57.Size = New System.Drawing.Size(109, 13)
		Me.Label57.TabIndex = 193
		Me.Label57.Text = "Grösse der Scandatei"
		'
		'seRPScanSize
		'
		Me.seRPScanSize.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
		Me.seRPScanSize.Location = New System.Drawing.Point(176, 9)
		Me.seRPScanSize.Name = "seRPScanSize"
		Me.seRPScanSize.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.seRPScanSize.Properties.Mask.EditMask = "f0"
		Me.seRPScanSize.Properties.MaxLength = 2
		Me.seRPScanSize.Properties.MaxValue = New Decimal(New Integer() {30, 0, 0, 0})
		Me.seRPScanSize.Size = New System.Drawing.Size(61, 20)
		Me.seRPScanSize.TabIndex = 193
		'
		'chkComatic
		'
		Me.chkComatic.Location = New System.Drawing.Point(21, 101)
		Me.chkComatic.MenuManager = Me.BarManager1
		Me.chkComatic.Name = "chkComatic"
		Me.chkComatic.Properties.Caption = "Comatic"
		Me.chkComatic.Size = New System.Drawing.Size(111, 20)
		Me.chkComatic.TabIndex = 6
		'
		'chkCSOPList
		'
		Me.chkCSOPList.Location = New System.Drawing.Point(154, 51)
		Me.chkCSOPList.MenuManager = Me.BarManager1
		Me.chkCSOPList.Name = "chkCSOPList"
		Me.chkCSOPList.Properties.Caption = "CSOPListe"
		Me.chkCSOPList.Size = New System.Drawing.Size(111, 20)
		Me.chkCSOPList.TabIndex = 5
		'
		'chkKMUFactoring
		'
		Me.chkKMUFactoring.Location = New System.Drawing.Point(154, 25)
		Me.chkKMUFactoring.MenuManager = Me.BarManager1
		Me.chkKMUFactoring.Name = "chkKMUFactoring"
		Me.chkKMUFactoring.Properties.Caption = "KMU-Factoring"
		Me.chkKMUFactoring.Size = New System.Drawing.Size(111, 20)
		Me.chkKMUFactoring.TabIndex = 4
		'
		'chkParifond
		'
		Me.chkParifond.Location = New System.Drawing.Point(154, 76)
		Me.chkParifond.MenuManager = Me.BarManager1
		Me.chkParifond.Name = "chkParifond"
		Me.chkParifond.Properties.Caption = "Parifond"
		Me.chkParifond.Size = New System.Drawing.Size(111, 20)
		Me.chkParifond.TabIndex = 3
		'
		'chkSwifac
		'
		Me.chkSwifac.Location = New System.Drawing.Point(21, 76)
		Me.chkSwifac.MenuManager = Me.BarManager1
		Me.chkSwifac.Name = "chkSwifac"
		Me.chkSwifac.Properties.Caption = "SwiFac"
		Me.chkSwifac.Size = New System.Drawing.Size(111, 20)
		Me.chkSwifac.TabIndex = 2
		'
		'chkAbacus
		'
		Me.chkAbacus.Location = New System.Drawing.Point(21, 51)
		Me.chkAbacus.MenuManager = Me.BarManager1
		Me.chkAbacus.Name = "chkAbacus"
		Me.chkAbacus.Properties.Caption = "ABACUS"
		Me.chkAbacus.Size = New System.Drawing.Size(111, 20)
		Me.chkAbacus.TabIndex = 1
		'
		'chkSesam
		'
		Me.chkSesam.Location = New System.Drawing.Point(21, 25)
		Me.chkSesam.MenuManager = Me.BarManager1
		Me.chkSesam.Name = "chkSesam"
		Me.chkSesam.Properties.Caption = "Sesam"
		Me.chkSesam.Size = New System.Drawing.Size(111, 20)
		Me.chkSesam.TabIndex = 0
		'
		'chkaskonexit
		'
		Me.chkaskonexit.Location = New System.Drawing.Point(4, 11)
		Me.chkaskonexit.Name = "chkaskonexit"
		Me.chkaskonexit.Properties.Caption = "Beenden des Programms bestätigen"
		Me.chkaskonexit.Size = New System.Drawing.Size(359, 20)
		Me.chkaskonexit.TabIndex = 1
		'
		'chkLUpdate
		'
		Me.chkLUpdate.Location = New System.Drawing.Point(4, 36)
		Me.chkLUpdate.Name = "chkLUpdate"
		Me.chkLUpdate.Properties.Caption = "Update-Programm beim Programmstart ausführen"
		Me.chkLUpdate.Size = New System.Drawing.Size(359, 20)
		Me.chkLUpdate.TabIndex = 0
		'
		'Label41
		'
		Me.Label41.AutoSize = True
		Me.Label41.Location = New System.Drawing.Point(6, 89)
		Me.Label41.Name = "Label41"
		Me.Label41.Size = New System.Drawing.Size(49, 13)
		Me.Label41.TabIndex = 183
		Me.Label41.Text = "Vorlagen"
		'
		'Label35
		'
		Me.Label35.AutoSize = True
		Me.Label35.Location = New System.Drawing.Point(6, 16)
		Me.Label35.Name = "Label35"
		Me.Label35.Size = New System.Drawing.Size(72, 13)
		Me.Label35.TabIndex = 179
		Me.Label35.Text = "Daten-Server"
		'
		'PanelControl3
		'
		Me.PanelControl3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.PanelControl3.Controls.Add(Me.seAnzTestRP)
		Me.PanelControl3.Controls.Add(Me.Label47)
		Me.PanelControl3.Controls.Add(Me.seAnzTestCheck)
		Me.PanelControl3.Controls.Add(Me.Label46)
		Me.PanelControl3.Controls.Add(Me.chkRPOpenWeek)
		Me.PanelControl3.Controls.Add(Me.chkRPWeekMust)
		Me.PanelControl3.Location = New System.Drawing.Point(12, 34)
		Me.PanelControl3.Name = "PanelControl3"
		Me.PanelControl3.Size = New System.Drawing.Size(308, 102)
		Me.PanelControl3.TabIndex = 182
		'
		'seAnzTestRP
		'
		Me.seAnzTestRP.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
		Me.seAnzTestRP.Location = New System.Drawing.Point(282, 66)
		Me.seAnzTestRP.Name = "seAnzTestRP"
		Me.seAnzTestRP.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.seAnzTestRP.Properties.Mask.EditMask = "f0"
		Me.seAnzTestRP.Properties.MaxLength = 2
		Me.seAnzTestRP.Properties.MaxValue = New Decimal(New Integer() {20, 0, 0, 0})
		Me.seAnzTestRP.Size = New System.Drawing.Size(48, 20)
		Me.seAnzTestRP.TabIndex = 195
		'
		'Label47
		'
		Me.Label47.AutoSize = True
		Me.Label47.Location = New System.Drawing.Point(201, 70)
		Me.Label47.Name = "Label47"
		Me.Label47.Size = New System.Drawing.Size(77, 13)
		Me.Label47.TabIndex = 194
		Me.Label47.Text = "Test-Rapporte"
		'
		'seAnzTestCheck
		'
		Me.seAnzTestCheck.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
		Me.seAnzTestCheck.Location = New System.Drawing.Point(135, 66)
		Me.seAnzTestCheck.Name = "seAnzTestCheck"
		Me.seAnzTestCheck.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.seAnzTestCheck.Properties.Mask.EditMask = "f0"
		Me.seAnzTestCheck.Properties.MaxValue = New Decimal(New Integer() {20, 0, 0, 0})
		Me.seAnzTestCheck.Size = New System.Drawing.Size(48, 20)
		Me.seAnzTestCheck.TabIndex = 0
		'
		'Label46
		'
		Me.Label46.AutoSize = True
		Me.Label46.Location = New System.Drawing.Point(17, 70)
		Me.Label46.Name = "Label46"
		Me.Label46.Size = New System.Drawing.Size(101, 13)
		Me.Label46.TabIndex = 191
		Me.Label46.Text = "Anzahl Test-Checks"
		'
		'chkRPOpenWeek
		'
		Me.chkRPOpenWeek.Location = New System.Drawing.Point(18, 15)
		Me.chkRPOpenWeek.Name = "chkRPOpenWeek"
		Me.chkRPOpenWeek.Properties.Caption = "Wochenauswahl im Rapport aufklappen"
		Me.chkRPOpenWeek.Size = New System.Drawing.Size(400, 20)
		Me.chkRPOpenWeek.TabIndex = 3
		'
		'chkRPWeekMust
		'
		Me.chkRPWeekMust.Location = New System.Drawing.Point(18, 40)
		Me.chkRPWeekMust.Name = "chkRPWeekMust"
		Me.chkRPWeekMust.Properties.Caption = "Wochenauswahl im Rapport ist zwingend"
		Me.chkRPWeekMust.Size = New System.Drawing.Size(400, 20)
		Me.chkRPWeekMust.TabIndex = 2
		'
		'pLColor
		'
		Me.pLColor.Controls.Add(Me.gLColor)
		Me.pLColor.Location = New System.Drawing.Point(560, 99)
		Me.pLColor.Name = "pLColor"
		Me.pLColor.Size = New System.Drawing.Size(287, 79)
		Me.pLColor.TabIndex = 186
		Me.pLColor.Visible = False
		'
		'gLColor
		'
		Me.gLColor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.gLColor.Controls.Add(Me.glRPData)
		Me.gLColor.Controls.Add(Me.GroupPanel7)
		Me.gLColor.Controls.Add(Me.GroupPanel6)
		Me.gLColor.Controls.Add(Me.GroupPanel5)
		Me.gLColor.Location = New System.Drawing.Point(0, 12)
		Me.gLColor.Name = "gLColor"
		Me.gLColor.Size = New System.Drawing.Size(282, 62)
		Me.gLColor.TabIndex = 185
		Me.gLColor.Text = "Lokale Einstellungen"
		'
		'glRPData
		'
		Me.glRPData.Controls.Add(Me.PanelControl3)
		Me.glRPData.Location = New System.Drawing.Point(406, 224)
		Me.glRPData.Name = "glRPData"
		Me.glRPData.Size = New System.Drawing.Size(332, 149)
		Me.glRPData.TabIndex = 0
		Me.glRPData.Text = "Rapportangaben"
		Me.glRPData.Visible = False
		'
		'GroupPanel7
		'
		Me.GroupPanel7.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel7.CanvasColor = System.Drawing.Color.Transparent
		Me.GroupPanel7.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.GroupPanel7.Controls.Add(Me.chkaskonexit)
		Me.GroupPanel7.Controls.Add(Me.chkLUpdate)
		Me.GroupPanel7.Location = New System.Drawing.Point(20, 224)
		Me.GroupPanel7.Name = "GroupPanel7"
		Me.GroupPanel7.Size = New System.Drawing.Size(372, 149)
		'
		'
		'
		Me.GroupPanel7.Style.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel7.Style.BackColor2 = System.Drawing.Color.Transparent
		Me.GroupPanel7.Style.BackColorGradientAngle = 90
		Me.GroupPanel7.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel7.Style.BorderBottomWidth = 1
		Me.GroupPanel7.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.GroupPanel7.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel7.Style.BorderLeftWidth = 1
		Me.GroupPanel7.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel7.Style.BorderRightWidth = 1
		Me.GroupPanel7.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel7.Style.BorderTopWidth = 1
		Me.GroupPanel7.Style.CornerDiameter = 4
		Me.GroupPanel7.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.GroupPanel7.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.GroupPanel7.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.GroupPanel7.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.GroupPanel7.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.GroupPanel7.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.GroupPanel7.TabIndex = 185
		Me.GroupPanel7.Text = "Autostart"
		'
		'GroupPanel6
		'
		Me.GroupPanel6.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel6.CanvasColor = System.Drawing.Color.Transparent
		Me.GroupPanel6.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.GroupPanel6.Controls.Add(Me.Label45)
		Me.GroupPanel6.Controls.Add(Me.lblLMDPfad)
		Me.GroupPanel6.Controls.Add(Me.Label49)
		Me.GroupPanel6.Controls.Add(Me.txtLPfad_3)
		Me.GroupPanel6.Controls.Add(Me.Label44)
		Me.GroupPanel6.Controls.Add(Me.txtLPfad_2)
		Me.GroupPanel6.Controls.Add(Me.txtLPfad_1)
		Me.GroupPanel6.Controls.Add(Me.Label35)
		Me.GroupPanel6.Controls.Add(Me.Label41)
		Me.GroupPanel6.Controls.Add(Me.txtLPfad_0)
		Me.GroupPanel6.Location = New System.Drawing.Point(20, 40)
		Me.GroupPanel6.Name = "GroupPanel6"
		Me.GroupPanel6.Size = New System.Drawing.Size(372, 168)
		'
		'
		'
		Me.GroupPanel6.Style.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel6.Style.BackColor2 = System.Drawing.Color.Transparent
		Me.GroupPanel6.Style.BackColorGradientAngle = 90
		Me.GroupPanel6.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel6.Style.BorderBottomWidth = 1
		Me.GroupPanel6.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.GroupPanel6.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel6.Style.BorderLeftWidth = 1
		Me.GroupPanel6.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel6.Style.BorderRightWidth = 1
		Me.GroupPanel6.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel6.Style.BorderTopWidth = 1
		Me.GroupPanel6.Style.CornerDiameter = 4
		Me.GroupPanel6.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.GroupPanel6.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.GroupPanel6.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.GroupPanel6.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.GroupPanel6.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.GroupPanel6.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.GroupPanel6.TabIndex = 184
		Me.GroupPanel6.Text = "Verzeichnisangaben"
		'
		'Label45
		'
		Me.Label45.AutoSize = True
		Me.Label45.Location = New System.Drawing.Point(6, 40)
		Me.Label45.Name = "Label45"
		Me.Label45.Size = New System.Drawing.Size(49, 13)
		Me.Label45.TabIndex = 197
		Me.Label45.Text = "Mandant"
		'
		'lblLMDPfad
		'
		Me.lblLMDPfad.AutoEllipsis = True
		Me.lblLMDPfad.Location = New System.Drawing.Point(154, 40)
		Me.lblLMDPfad.Name = "lblLMDPfad"
		Me.lblLMDPfad.Size = New System.Drawing.Size(195, 13)
		Me.lblLMDPfad.TabIndex = 195
		Me.lblLMDPfad.Text = "#"
		Me.lblLMDPfad.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Label49
		'
		Me.Label49.AutoSize = True
		Me.Label49.Location = New System.Drawing.Point(6, 63)
		Me.Label49.Name = "Label49"
		Me.Label49.Size = New System.Drawing.Size(70, 13)
		Me.Label49.TabIndex = 193
		Me.Label49.Text = "Druckdateien"
		'
		'txtLPfad_3
		'
		Me.txtLPfad_3.Location = New System.Drawing.Point(82, 111)
		Me.txtLPfad_3.Name = "txtLPfad_3"
		Me.txtLPfad_3.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.txtLPfad_3.Size = New System.Drawing.Size(267, 20)
		Me.txtLPfad_3.TabIndex = 190
		'
		'Label44
		'
		Me.Label44.AutoSize = True
		Me.Label44.Location = New System.Drawing.Point(6, 115)
		Me.Label44.Name = "Label44"
		Me.Label44.Size = New System.Drawing.Size(70, 13)
		Me.Label44.TabIndex = 189
		Me.Label44.Text = "Druckdateien"
		'
		'txtLPfad_2
		'
		Me.txtLPfad_2.Location = New System.Drawing.Point(258, 85)
		Me.txtLPfad_2.Name = "txtLPfad_2"
		Me.txtLPfad_2.Size = New System.Drawing.Size(91, 20)
		Me.txtLPfad_2.TabIndex = 187
		'
		'txtLPfad_1
		'
		Me.txtLPfad_1.Location = New System.Drawing.Point(258, 59)
		Me.txtLPfad_1.Name = "txtLPfad_1"
		Me.txtLPfad_1.Size = New System.Drawing.Size(91, 20)
		Me.txtLPfad_1.TabIndex = 186
		'
		'txtLPfad_0
		'
		Me.txtLPfad_0.Location = New System.Drawing.Point(82, 13)
		Me.txtLPfad_0.Name = "txtLPfad_0"
		Me.txtLPfad_0.Size = New System.Drawing.Size(267, 20)
		Me.txtLPfad_0.TabIndex = 182
		'
		'GroupPanel5
		'
		Me.GroupPanel5.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel5.CanvasColor = System.Drawing.Color.Transparent
		Me.GroupPanel5.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.GroupPanel5.Controls.Add(Me.Label48)
		Me.GroupPanel5.Controls.Add(Me.cboLNavStyle)
		Me.GroupPanel5.Controls.Add(Me.Label51)
		Me.GroupPanel5.Controls.Add(Me.Label54)
		Me.GroupPanel5.Controls.Add(Me.cleLNormal_0)
		Me.GroupPanel5.Controls.Add(Me.cboLFormStyle)
		Me.GroupPanel5.Location = New System.Drawing.Point(406, 40)
		Me.GroupPanel5.Name = "GroupPanel5"
		Me.GroupPanel5.Size = New System.Drawing.Size(332, 168)
		'
		'
		'
		Me.GroupPanel5.Style.BackColor = System.Drawing.Color.Transparent
		Me.GroupPanel5.Style.BackColor2 = System.Drawing.Color.Transparent
		Me.GroupPanel5.Style.BackColorGradientAngle = 90
		Me.GroupPanel5.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel5.Style.BorderBottomWidth = 1
		Me.GroupPanel5.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.GroupPanel5.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel5.Style.BorderLeftWidth = 1
		Me.GroupPanel5.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel5.Style.BorderRightWidth = 1
		Me.GroupPanel5.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
		Me.GroupPanel5.Style.BorderTopWidth = 1
		Me.GroupPanel5.Style.CornerDiameter = 4
		Me.GroupPanel5.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
		Me.GroupPanel5.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
		Me.GroupPanel5.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.GroupPanel5.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
		'
		'
		'
		Me.GroupPanel5.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
		'
		'
		'
		Me.GroupPanel5.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
		Me.GroupPanel5.TabIndex = 183
		Me.GroupPanel5.Text = "Layout"
		'
		'Label48
		'
		Me.Label48.AutoSize = True
		Me.Label48.Location = New System.Drawing.Point(13, 66)
		Me.Label48.Name = "Label48"
		Me.Label48.Size = New System.Drawing.Size(93, 13)
		Me.Label48.TabIndex = 196
		Me.Label48.Text = "Navigationslayout"
		'
		'cboLNavStyle
		'
		Me.cboLNavStyle.Location = New System.Drawing.Point(126, 63)
		Me.cboLNavStyle.Name = "cboLNavStyle"
		Me.cboLNavStyle.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboLNavStyle.Size = New System.Drawing.Size(189, 20)
		Me.cboLNavStyle.TabIndex = 195
		'
		'Label13
		'
		Me.Label13.BackColor = System.Drawing.Color.Transparent
		Me.Label13.Image = Global.SPSSetting.My.Resources.Resources.SETTINGS
		Me.Label13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.Label13.Location = New System.Drawing.Point(15, 9)
		Me.Label13.Name = "Label13"
		Me.Label13.Size = New System.Drawing.Size(49, 48)
		Me.Label13.TabIndex = 42
		'
		'pNavbar
		'
		Me.pNavbar.Controls.Add(Me.nvMain)
		Me.pNavbar.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pNavbar.Location = New System.Drawing.Point(5, 5)
		Me.pNavbar.Name = "pNavbar"
		Me.pNavbar.Size = New System.Drawing.Size(206, 446)
		Me.pNavbar.TabIndex = 43
		'
		'nvMain
		'
		Me.nvMain.ActiveGroup = Me.nvgLGSetting
		Me.nvMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.nvMain.Groups.AddRange(New DevExpress.XtraNavBar.NavBarGroup() {Me.nvgLGSetting, Me.nbgFeldinformationen, Me.nvLayout, Me.nvgCom, Me.nvgJob})
		Me.nvMain.Items.AddRange(New DevExpress.XtraNavBar.NavBarItem() {Me.nviWOS, Me.nviMailFax, Me.nviMailTpl, Me.nviMetro_0, Me.nviJCH, Me.nviJWinner, Me.nviLFieldColor, Me.nviGlobalSetting, Me.nviLicense})
		Me.nvMain.Location = New System.Drawing.Point(2, 2)
		Me.nvMain.Name = "nvMain"
		Me.nvMain.OptionsNavPane.ExpandedWidth = 202
		Me.nvMain.Size = New System.Drawing.Size(202, 442)
		Me.nvMain.TabIndex = 44
		Me.nvMain.Text = "NavBarControl1"
		'
		'nvgLGSetting
		'
		Me.nvgLGSetting.Caption = "Einstellungen"
		Me.nvgLGSetting.Expanded = True
		Me.nvgLGSetting.ItemLinks.AddRange(New DevExpress.XtraNavBar.NavBarItemLink() {New DevExpress.XtraNavBar.NavBarItemLink(Me.nviLFieldColor), New DevExpress.XtraNavBar.NavBarItemLink(Me.nviGlobalSetting)})
		Me.nvgLGSetting.Name = "nvgLGSetting"
		'
		'nviLFieldColor
		'
		Me.nviLFieldColor.Caption = "Lokale Einstellungen"
		Me.nviLFieldColor.Name = "nviLFieldColor"
		'
		'nviGlobalSetting
		'
		Me.nviGlobalSetting.Caption = "Globale Einstellungen"
		Me.nviGlobalSetting.Name = "nviGlobalSetting"
		'
		'nbgFeldinformationen
		'
		Me.nbgFeldinformationen.Caption = "Feldinformationen"
		Me.nbgFeldinformationen.Expanded = True
		Me.nbgFeldinformationen.ItemLinks.AddRange(New DevExpress.XtraNavBar.NavBarItemLink() {New DevExpress.XtraNavBar.NavBarItemLink(Me.nviLicense)})
		Me.nbgFeldinformationen.Name = "nbgFeldinformationen"
		'
		'nviLicense
		'
		Me.nviLicense.Caption = "Lizenzen verwalten"
		Me.nviLicense.Name = "nviLicense"
		'
		'nvLayout
		'
		Me.nvLayout.Caption = "Layout"
		Me.nvLayout.Expanded = True
		Me.nvLayout.ItemLinks.AddRange(New DevExpress.XtraNavBar.NavBarItemLink() {New DevExpress.XtraNavBar.NavBarItemLink(Me.nviMetro_0)})
		Me.nvLayout.Name = "nvLayout"
		'
		'nviMetro_0
		'
		Me.nviMetro_0.Caption = "Farb-Einstellungen"
		Me.nviMetro_0.Name = "nviMetro_0"
		'
		'nvgCom
		'
		Me.nvgCom.Caption = "Kommunikation"
		Me.nvgCom.Expanded = True
		Me.nvgCom.ItemLinks.AddRange(New DevExpress.XtraNavBar.NavBarItemLink() {New DevExpress.XtraNavBar.NavBarItemLink(Me.nviWOS), New DevExpress.XtraNavBar.NavBarItemLink(Me.nviMailFax), New DevExpress.XtraNavBar.NavBarItemLink(Me.nviMailTpl)})
		Me.nvgCom.Name = "nvgCom"
		'
		'nviWOS
		'
		Me.nviWOS.Caption = "WOS-Einstellungen"
		Me.nviWOS.Name = "nviWOS"
		'
		'nviMailFax
		'
		Me.nviMailFax.Caption = "E-Mail und Fax Einstellungen"
		Me.nviMailFax.Name = "nviMailFax"
		'
		'nviMailTpl
		'
		Me.nviMailTpl.Caption = "E-Mail-Variable"
		Me.nviMailTpl.Name = "nviMailTpl"
		'
		'nvgJob
		'
		Me.nvgJob.Caption = "Job-Plattformen"
		Me.nvgJob.Expanded = True
		Me.nvgJob.ItemLinks.AddRange(New DevExpress.XtraNavBar.NavBarItemLink() {New DevExpress.XtraNavBar.NavBarItemLink(Me.nviJCH), New DevExpress.XtraNavBar.NavBarItemLink(Me.nviJWinner)})
		Me.nvgJob.Name = "nvgJob"
		'
		'nviJCH
		'
		Me.nviJCH.Caption = "Jobs.ch"
		Me.nviJCH.Name = "nviJCH"
		'
		'nviJWinner
		'
		Me.nviJWinner.Caption = "Jobwinner.ch"
		Me.nviJWinner.Name = "nviJWinner"
		Me.nviJWinner.Visible = False
		'
		'xscMain
		'
		Me.xscMain.Appearance.BackColor = System.Drawing.Color.White
		Me.xscMain.Appearance.Options.UseBackColor = True
		Me.xscMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.xscMain.Location = New System.Drawing.Point(5, 5)
		Me.xscMain.Name = "xscMain"
		Me.xscMain.Size = New System.Drawing.Size(7, 446)
		Me.xscMain.TabIndex = 44
		'
		'pMail_Tpl
		'
		Me.pMail_Tpl.Controls.Add(Me.gMail_Tpl)
		Me.pMail_Tpl.Location = New System.Drawing.Point(1732, 757)
		Me.pMail_Tpl.Name = "pMail_Tpl"
		Me.pMail_Tpl.Size = New System.Drawing.Size(265, 108)
		Me.pMail_Tpl.TabIndex = 185
		Me.pMail_Tpl.Visible = False
		'
		'gMail_Tpl
		'
		Me.gMail_Tpl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.gMail_Tpl.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.gMail_Tpl.Appearance.Options.UseBackColor = True
		Me.gMail_Tpl.Controls.Add(Me.GroupBox3)
		Me.gMail_Tpl.Controls.Add(Me.GroupBox5)
		Me.gMail_Tpl.Controls.Add(Me.GroupBox6)
		Me.gMail_Tpl.Location = New System.Drawing.Point(5, 4)
		Me.gMail_Tpl.Name = "gMail_Tpl"
		Me.gMail_Tpl.Size = New System.Drawing.Size(237, 99)
		Me.gMail_Tpl.TabIndex = 0
		Me.gMail_Tpl.Text = "E-Mail-Variable"
		'
		'pCom_WOS
		'
		Me.pCom_WOS.Controls.Add(Me.gWOS)
		Me.pCom_WOS.Location = New System.Drawing.Point(271, 371)
		Me.pCom_WOS.Name = "pCom_WOS"
		Me.pCom_WOS.Size = New System.Drawing.Size(310, 320)
		Me.pCom_WOS.TabIndex = 186
		Me.pCom_WOS.Visible = False
		'
		'gWOS
		'
		Me.gWOS.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
			Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.gWOS.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.gWOS.Appearance.Options.UseBackColor = True
		Me.gWOS.Controls.Add(Me.GroupBox7)
		Me.gWOS.Controls.Add(Me.GroupBox2)
		Me.gWOS.Location = New System.Drawing.Point(13, 8)
		Me.gWOS.Name = "gWOS"
		Me.gWOS.Size = New System.Drawing.Size(285, 294)
		Me.gWOS.TabIndex = 185
		Me.gWOS.Text = "WOS-Einstellungen"
		'
		'pMailFax
		'
		Me.pMailFax.Controls.Add(Me.gMailFax)
		Me.pMailFax.Location = New System.Drawing.Point(1001, 239)
		Me.pMailFax.Name = "pMailFax"
		Me.pMailFax.Size = New System.Drawing.Size(709, 407)
		Me.pMailFax.TabIndex = 0
		Me.pMailFax.Visible = False
		'
		'gMailFax
		'
		Me.gMailFax.Controls.Add(Me.xtabFax)
		Me.gMailFax.Controls.Add(Me.GroupBox4)
		Me.gMailFax.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gMailFax.Location = New System.Drawing.Point(2, 2)
		Me.gMailFax.Name = "gMailFax"
		Me.gMailFax.Size = New System.Drawing.Size(705, 403)
		Me.gMailFax.TabIndex = 183
		Me.gMailFax.Text = "Einstellungen für E-Mail und Fax"
		'
		'xtabFax
		'
		Me.xtabFax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.xtabFax.Location = New System.Drawing.Point(20, 127)
		Me.xtabFax.Name = "xtabFax"
		Me.xtabFax.SelectedTabPage = Me.xtabeCall
		Me.xtabFax.Size = New System.Drawing.Size(660, 259)
		Me.xtabFax.TabIndex = 43
		Me.xtabFax.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabeCall, Me.xtabDavidFaxserver})
		'
		'xtabeCall
		'
		Me.xtabeCall.Controls.Add(Me.pnlECall)
		Me.xtabeCall.Name = "xtabeCall"
		Me.xtabeCall.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabeCall.Size = New System.Drawing.Size(658, 234)
		Me.xtabeCall.Text = "eCall(TM) Fax-Portal"
		'
		'xtabDavidFaxserver
		'
		Me.xtabDavidFaxserver.Controls.Add(Me.PanelControl5)
		Me.xtabDavidFaxserver.Name = "xtabDavidFaxserver"
		Me.xtabDavidFaxserver.Padding = New System.Windows.Forms.Padding(5)
		Me.xtabDavidFaxserver.Size = New System.Drawing.Size(658, 234)
		Me.xtabDavidFaxserver.Text = "David-Faxserver"
		'
		'PanelControl5
		'
		Me.PanelControl5.Controls.Add(Me.Label73)
		Me.PanelControl5.Controls.Add(Me.txtDavidfaxserver)
		Me.PanelControl5.Controls.Add(Me.Label7)
		Me.PanelControl5.Controls.Add(Me.txtFaxExtension)
		Me.PanelControl5.Controls.Add(Me.Label9)
		Me.PanelControl5.Controls.Add(Me.Label8)
		Me.PanelControl5.Controls.Add(Me.txtFaxForwarder)
		Me.PanelControl5.Dock = System.Windows.Forms.DockStyle.Fill
		Me.PanelControl5.Location = New System.Drawing.Point(5, 5)
		Me.PanelControl5.Name = "PanelControl5"
		Me.PanelControl5.Size = New System.Drawing.Size(648, 224)
		Me.PanelControl5.TabIndex = 0
		'
		'Label73
		'
		Me.Label73.AutoSize = True
		Me.Label73.Location = New System.Drawing.Point(9, 27)
		Me.Label73.Name = "Label73"
		Me.Label73.Size = New System.Drawing.Size(86, 13)
		Me.Label73.TabIndex = 41
		Me.Label73.Text = "David Faxserver"
		'
		'txtDavidfaxserver
		'
		Me.txtDavidfaxserver.Location = New System.Drawing.Point(164, 23)
		Me.txtDavidfaxserver.Name = "txtDavidfaxserver"
		Me.txtDavidfaxserver.Size = New System.Drawing.Size(256, 20)
		Me.txtDavidfaxserver.TabIndex = 40
		'
		'GroupBox10
		'
		Me.GroupBox10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox10.BackColor = System.Drawing.Color.Transparent
		Me.GroupBox10.Controls.Add(Me.chkCresus)
		Me.GroupBox10.Controls.Add(Me.chkpmsearch)
		Me.GroupBox10.Controls.Add(Me.chkallowedemployeeweeklypayment)
		Me.GroupBox10.Controls.Add(Me.chkCVDropIn)
		Me.GroupBox10.Controls.Add(Me.chkScanDropIN)
		Me.GroupBox10.Controls.Add(Me.chkComatic)
		Me.GroupBox10.Controls.Add(Me.chkSesam)
		Me.GroupBox10.Controls.Add(Me.chkCSOPList)
		Me.GroupBox10.Controls.Add(Me.chkAbacus)
		Me.GroupBox10.Controls.Add(Me.chkKMUFactoring)
		Me.GroupBox10.Controls.Add(Me.chkSwifac)
		Me.GroupBox10.Controls.Add(Me.chkParifond)
		Me.GroupBox10.Location = New System.Drawing.Point(20, 38)
		Me.GroupBox10.Name = "GroupBox10"
		Me.GroupBox10.Size = New System.Drawing.Size(658, 151)
		Me.GroupBox10.TabIndex = 198
		Me.GroupBox10.TabStop = False
		Me.GroupBox10.Text = "Lizenzangaben"
		'
		'chkCresus
		'
		Me.chkCresus.Location = New System.Drawing.Point(154, 102)
		Me.chkCresus.MenuManager = Me.BarManager1
		Me.chkCresus.Name = "chkCresus"
		Me.chkCresus.Properties.Caption = "Cresus"
		Me.chkCresus.Size = New System.Drawing.Size(111, 20)
		Me.chkCresus.TabIndex = 11
		'
		'chkpmsearch
		'
		Me.chkpmsearch.Location = New System.Drawing.Point(321, 76)
		Me.chkpmsearch.MenuManager = Me.BarManager1
		Me.chkpmsearch.Name = "chkpmsearch"
		Me.chkpmsearch.Properties.Caption = "Profilmatcher"
		Me.chkpmsearch.Size = New System.Drawing.Size(111, 20)
		Me.chkpmsearch.TabIndex = 10
		'
		'chkallowedemployeeweeklypayment
		'
		Me.chkallowedemployeeweeklypayment.Location = New System.Drawing.Point(321, 101)
		Me.chkallowedemployeeweeklypayment.MenuManager = Me.BarManager1
		Me.chkallowedemployeeweeklypayment.Name = "chkallowedemployeeweeklypayment"
		Me.chkallowedemployeeweeklypayment.Properties.Caption = "Employee Weeklypayment"
		Me.chkallowedemployeeweeklypayment.Size = New System.Drawing.Size(162, 20)
		Me.chkallowedemployeeweeklypayment.TabIndex = 9
		'
		'chkCVDropIn
		'
		Me.chkCVDropIn.Location = New System.Drawing.Point(321, 50)
		Me.chkCVDropIn.MenuManager = Me.BarManager1
		Me.chkCVDropIn.Name = "chkCVDropIn"
		Me.chkCVDropIn.Properties.Caption = "CV Drop-In"
		Me.chkCVDropIn.Size = New System.Drawing.Size(111, 20)
		Me.chkCVDropIn.TabIndex = 8
		'
		'chkScanDropIN
		'
		Me.chkScanDropIN.Location = New System.Drawing.Point(321, 25)
		Me.chkScanDropIN.MenuManager = Me.BarManager1
		Me.chkScanDropIN.Name = "chkScanDropIN"
		Me.chkScanDropIN.Properties.Caption = "Scan Drop-In"
		Me.chkScanDropIN.Size = New System.Drawing.Size(111, 20)
		Me.chkScanDropIN.TabIndex = 7
		'
		'GroupBox8
		'
		Me.GroupBox8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.GroupBox8.BackColor = System.Drawing.Color.Transparent
		Me.GroupBox8.Controls.Add(Me.Label77)
		Me.GroupBox8.Controls.Add(Me.txt_DVURL)
		Me.GroupBox8.Controls.Add(Me.Label76)
		Me.GroupBox8.Controls.Add(Me.txt_DVPW)
		Me.GroupBox8.Controls.Add(Me.Label74)
		Me.GroupBox8.Controls.Add(Me.txt_DVRefNo)
		Me.GroupBox8.Controls.Add(Me.Label75)
		Me.GroupBox8.Controls.Add(Me.txt_DVUSName)
		Me.GroupBox8.Location = New System.Drawing.Point(20, 207)
		Me.GroupBox8.Name = "GroupBox8"
		Me.GroupBox8.Size = New System.Drawing.Size(658, 151)
		Me.GroupBox8.TabIndex = 197
		Me.GroupBox8.TabStop = False
		Me.GroupBox8.Text = "Einstellungen für Deltavista"
		'
		'Label77
		'
		Me.Label77.Location = New System.Drawing.Point(20, 108)
		Me.Label77.Name = "Label77"
		Me.Label77.Size = New System.Drawing.Size(245, 13)
		Me.Label77.TabIndex = 30
		Me.Label77.Text = "customermng_deltavistaWebServiceUrl"
		Me.Label77.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txt_DVURL
		'
		Me.txt_DVURL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_DVURL.Location = New System.Drawing.Point(271, 104)
		Me.txt_DVURL.Name = "txt_DVURL"
		Me.txt_DVURL.Size = New System.Drawing.Size(373, 20)
		Me.txt_DVURL.TabIndex = 29
		'
		'Label76
		'
		Me.Label76.Location = New System.Drawing.Point(20, 82)
		Me.Label76.Name = "Label76"
		Me.Label76.Size = New System.Drawing.Size(245, 13)
		Me.Label76.TabIndex = 28
		Me.Label76.Text = "customermng_deltavistaWebServicePassword"
		Me.Label76.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txt_DVPW
		'
		Me.txt_DVPW.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_DVPW.Location = New System.Drawing.Point(271, 78)
		Me.txt_DVPW.Name = "txt_DVPW"
		Me.txt_DVPW.Size = New System.Drawing.Size(373, 20)
		Me.txt_DVPW.TabIndex = 27
		'
		'Label74
		'
		Me.Label74.Location = New System.Drawing.Point(20, 30)
		Me.Label74.Name = "Label74"
		Me.Label74.Size = New System.Drawing.Size(245, 13)
		Me.Label74.TabIndex = 24
		Me.Label74.Text = "CustomerManagementSolvencyCheckRefNo"
		Me.Label74.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txt_DVRefNo
		'
		Me.txt_DVRefNo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_DVRefNo.Location = New System.Drawing.Point(271, 26)
		Me.txt_DVRefNo.Name = "txt_DVRefNo"
		Me.txt_DVRefNo.Size = New System.Drawing.Size(373, 20)
		Me.txt_DVRefNo.TabIndex = 0
		'
		'Label75
		'
		Me.Label75.Location = New System.Drawing.Point(20, 56)
		Me.Label75.Name = "Label75"
		Me.Label75.Size = New System.Drawing.Size(245, 13)
		Me.Label75.TabIndex = 26
		Me.Label75.Text = "customermng_deltavistaWebServiceUserName"
		Me.Label75.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'txt_DVUSName
		'
		Me.txt_DVUSName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txt_DVUSName.Location = New System.Drawing.Point(271, 52)
		Me.txt_DVUSName.Name = "txt_DVUSName"
		Me.txt_DVUSName.Size = New System.Drawing.Size(373, 20)
		Me.txt_DVUSName.TabIndex = 1
		'
		'StyleManager1
		'
		Me.StyleManager1.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro
		Me.StyleManager1.MetroColorParameters = New DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(26, Byte), Integer)))
		'
		'sccMain
		'
		Me.sccMain.Location = New System.Drawing.Point(18, 102)
		Me.sccMain.Name = "sccMain"
		Me.sccMain.Panel1.Controls.Add(Me.pNavbar)
		Me.sccMain.Panel1.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel1.Text = "Panel1"
		Me.sccMain.Panel2.Controls.Add(Me.xscMain)
		Me.sccMain.Panel2.Padding = New System.Windows.Forms.Padding(5)
		Me.sccMain.Panel2.Text = "Panel2"
		Me.sccMain.Size = New System.Drawing.Size(243, 456)
		Me.sccMain.SplitterPosition = 216
		Me.sccMain.TabIndex = 192
		Me.sccMain.Text = "SplitContainerControl1"
		'
		'pJobplattforms
		'
		Me.pJobplattforms.Controls.Add(Me.XtraTabControl3)
		Me.pJobplattforms.Location = New System.Drawing.Point(624, 652)
		Me.pJobplattforms.Name = "pJobplattforms"
		Me.pJobplattforms.Size = New System.Drawing.Size(564, 512)
		Me.pJobplattforms.TabIndex = 198
		Me.pJobplattforms.Visible = False
		'
		'XtraTabControl3
		'
		Me.XtraTabControl3.Dock = System.Windows.Forms.DockStyle.Fill
		Me.XtraTabControl3.Location = New System.Drawing.Point(2, 2)
		Me.XtraTabControl3.Name = "XtraTabControl3"
		Me.XtraTabControl3.SelectedTabPage = Me.xtabSettingJobCH
		Me.XtraTabControl3.Size = New System.Drawing.Size(560, 508)
		Me.XtraTabControl3.TabIndex = 203
		Me.XtraTabControl3.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.xtabOurJobplattform, Me.xtabSettingJobCH, Me.xtabSettingOstjob, Me.xtabSettingsuedost})
		'
		'xtabSettingJobCH
		'
		Me.xtabSettingJobCH.Controls.Add(Me.gJCH)
		Me.xtabSettingJobCH.Name = "xtabSettingJobCH"
		Me.xtabSettingJobCH.Size = New System.Drawing.Size(558, 483)
		Me.xtabSettingJobCH.Text = "jobs.ch"
		'
		'gJCH
		'
		Me.gJCH.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.gJCH.Appearance.Options.UseBackColor = True
		Me.gJCH.Controls.Add(Me.PanelControl1)
		Me.gJCH.Controls.Add(Me.PanelControl2)
		Me.gJCH.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gJCH.Location = New System.Drawing.Point(0, 0)
		Me.gJCH.Name = "gJCH"
		Me.gJCH.Padding = New System.Windows.Forms.Padding(120, 0, 0, 0)
		Me.gJCH.Size = New System.Drawing.Size(558, 483)
		Me.gJCH.TabIndex = 0
		'
		'PanelControl1
		'
		Me.PanelControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.PanelControl1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.PanelControl1.Appearance.Options.UseBackColor = True
		Me.PanelControl1.Controls.Add(Me.cmdJCHKDSubDelete)
		Me.PanelControl1.Controls.Add(Me.cmdSaveSubNr)
		Me.PanelControl1.Controls.Add(Me.txtJCHKDSubNr)
		Me.PanelControl1.Controls.Add(Me.seJCHKDSubAnz)
		Me.PanelControl1.Controls.Add(Me.Label71)
		Me.PanelControl1.Controls.Add(Me.lstJCHKDSubNr)
		Me.PanelControl1.Location = New System.Drawing.Point(20, 349)
		Me.PanelControl1.Name = "PanelControl1"
		Me.PanelControl1.Size = New System.Drawing.Size(503, 86)
		Me.PanelControl1.TabIndex = 176
		'
		'cmdJCHKDSubDelete
		'
		Me.cmdJCHKDSubDelete.ImageOptions.Image = CType(resources.GetObject("cmdJCHKDSubDelete.ImageOptions.Image"), System.Drawing.Image)
		Me.cmdJCHKDSubDelete.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
		Me.cmdJCHKDSubDelete.Location = New System.Drawing.Point(525, 13)
		Me.cmdJCHKDSubDelete.Name = "cmdJCHKDSubDelete"
		Me.cmdJCHKDSubDelete.Size = New System.Drawing.Size(28, 28)
		Me.cmdJCHKDSubDelete.TabIndex = 191
		'
		'cmdSaveSubNr
		'
		Me.cmdSaveSubNr.ImageOptions.Image = CType(resources.GetObject("cmdSaveSubNr.ImageOptions.Image"), System.Drawing.Image)
		Me.cmdSaveSubNr.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter
		Me.cmdSaveSubNr.Location = New System.Drawing.Point(279, 13)
		Me.cmdSaveSubNr.Name = "cmdSaveSubNr"
		Me.cmdSaveSubNr.Size = New System.Drawing.Size(28, 28)
		Me.cmdSaveSubNr.TabIndex = 57
		'
		'txtJCHKDSubNr
		'
		Me.txtJCHKDSubNr.Location = New System.Drawing.Point(114, 13)
		Me.txtJCHKDSubNr.Name = "txtJCHKDSubNr"
		Me.txtJCHKDSubNr.Size = New System.Drawing.Size(104, 20)
		Me.txtJCHKDSubNr.TabIndex = 0
		'
		'seJCHKDSubAnz
		'
		Me.seJCHKDSubAnz.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
		Me.seJCHKDSubAnz.Location = New System.Drawing.Point(221, 13)
		Me.seJCHKDSubAnz.MenuManager = Me.BarManager1
		Me.seJCHKDSubAnz.Name = "seJCHKDSubAnz"
		Me.seJCHKDSubAnz.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.seJCHKDSubAnz.Properties.Mask.EditMask = "f0"
		Me.seJCHKDSubAnz.Size = New System.Drawing.Size(52, 20)
		Me.seJCHKDSubAnz.TabIndex = 1
		'
		'Label71
		'
		Me.Label71.AutoSize = True
		Me.Label71.Location = New System.Drawing.Point(12, 17)
		Me.Label71.Name = "Label71"
		Me.Label71.Size = New System.Drawing.Size(68, 13)
		Me.Label71.TabIndex = 188
		Me.Label71.Text = "Sub-Nummer"
		'
		'lstJCHKDSubNr
		'
		Me.lstJCHKDSubNr.Location = New System.Drawing.Point(365, 13)
		Me.lstJCHKDSubNr.Name = "lstJCHKDSubNr"
		Me.lstJCHKDSubNr.Size = New System.Drawing.Size(154, 56)
		Me.lstJCHKDSubNr.TabIndex = 2
		'
		'PanelControl2
		'
		Me.PanelControl2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.PanelControl2.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.PanelControl2.Appearance.Options.UseBackColor = True
		Me.PanelControl2.Controls.Add(Me.Label79)
		Me.PanelControl2.Controls.Add(Me.seJCHAddDayToDate)
		Me.PanelControl2.Controls.Add(Me.Label78)
		Me.PanelControl2.Controls.Add(Me.Label72)
		Me.PanelControl2.Controls.Add(Me.cboJCHAngebot_Art)
		Me.PanelControl2.Controls.Add(Me.cmdSaveJCHAccount)
		Me.PanelControl2.Controls.Add(Me.seJCHKDAnz)
		Me.PanelControl2.Controls.Add(Me.chJCH_Xing_Company_Is_Poc)
		Me.PanelControl2.Controls.Add(Me.txtJCH_Xing_Company_Profile_URL)
		Me.PanelControl2.Controls.Add(Me.Label70)
		Me.PanelControl2.Controls.Add(Me.Label43)
		Me.PanelControl2.Controls.Add(Me.txtJCH_Xing_Poster_URL)
		Me.PanelControl2.Controls.Add(Me.Label69)
		Me.PanelControl2.Controls.Add(Me.Label37)
		Me.PanelControl2.Controls.Add(Me.txtJCH_Bewerber_URL)
		Me.PanelControl2.Controls.Add(Me.Label32)
		Me.PanelControl2.Controls.Add(Me.txtJCH_Direkt_URL)
		Me.PanelControl2.Controls.Add(Me.txtJCH_Our_URL)
		Me.PanelControl2.Controls.Add(Me.txtJCH_Logo_ID)
		Me.PanelControl2.Controls.Add(Me.txtJCH_Layout_ID)
		Me.PanelControl2.Controls.Add(Me.txtJCH_Organisation_ID)
		Me.PanelControl2.Controls.Add(Me.Label39)
		Me.PanelControl2.Controls.Add(Me.Label42)
		Me.PanelControl2.Controls.Add(Me.Label36)
		Me.PanelControl2.Controls.Add(Me.Label33)
		Me.PanelControl2.Location = New System.Drawing.Point(20, 40)
		Me.PanelControl2.Name = "PanelControl2"
		Me.PanelControl2.Size = New System.Drawing.Size(503, 297)
		Me.PanelControl2.TabIndex = 175
		'
		'Label79
		'
		Me.Label79.AutoSize = True
		Me.Label79.Location = New System.Drawing.Point(334, 68)
		Me.Label79.Name = "Label79"
		Me.Label79.Size = New System.Drawing.Size(31, 13)
		Me.Label79.TabIndex = 202
		Me.Label79.Text = "Tage"
		Me.Label79.Visible = False
		'
		'seJCHAddDayToDate
		'
		Me.seJCHAddDayToDate.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
		Me.seJCHAddDayToDate.Location = New System.Drawing.Point(276, 66)
		Me.seJCHAddDayToDate.MenuManager = Me.BarManager1
		Me.seJCHAddDayToDate.Name = "seJCHAddDayToDate"
		Me.seJCHAddDayToDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.seJCHAddDayToDate.Properties.Mask.EditMask = "f0"
		Me.seJCHAddDayToDate.Size = New System.Drawing.Size(52, 20)
		Me.seJCHAddDayToDate.TabIndex = 200
		'
		'Label78
		'
		Me.Label78.Location = New System.Drawing.Point(174, 70)
		Me.Label78.Name = "Label78"
		Me.Label78.Size = New System.Drawing.Size(96, 13)
		Me.Label78.TabIndex = 201
		Me.Label78.Text = "End-Datum +"
		Me.Label78.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'Label72
		'
		Me.Label72.ForeColor = System.Drawing.Color.Black
		Me.Label72.Location = New System.Drawing.Point(12, 43)
		Me.Label72.Name = "Label72"
		Me.Label72.Size = New System.Drawing.Size(71, 13)
		Me.Label72.TabIndex = 199
		Me.Label72.Text = "Angebotsart"
		Me.Label72.Visible = False
		'
		'cboJCHAngebot_Art
		'
		Me.cboJCHAngebot_Art.Location = New System.Drawing.Point(114, 40)
		Me.cboJCHAngebot_Art.Name = "cboJCHAngebot_Art"
		Me.cboJCHAngebot_Art.Properties.Appearance.BackColor = System.Drawing.Color.White
		Me.cboJCHAngebot_Art.Properties.Appearance.ForeColor = System.Drawing.Color.Black
		Me.cboJCHAngebot_Art.Properties.Appearance.Options.UseBackColor = True
		Me.cboJCHAngebot_Art.Properties.Appearance.Options.UseForeColor = True
		Me.cboJCHAngebot_Art.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
		Me.cboJCHAngebot_Art.Size = New System.Drawing.Size(214, 20)
		Me.cboJCHAngebot_Art.TabIndex = 1
		Me.cboJCHAngebot_Art.Visible = False
		'
		'cmdSaveJCHAccount
		'
		Me.cmdSaveJCHAccount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdSaveJCHAccount.ImageOptions.Image = CType(resources.GetObject("cmdSaveJCHAccount.ImageOptions.Image"), System.Drawing.Image)
		Me.cmdSaveJCHAccount.Location = New System.Drawing.Point(392, 11)
		Me.cmdSaveJCHAccount.Name = "cmdSaveJCHAccount"
		Me.cmdSaveJCHAccount.Size = New System.Drawing.Size(92, 28)
		Me.cmdSaveJCHAccount.TabIndex = 11
		Me.cmdSaveJCHAccount.Text = "Speichern"
		'
		'seJCHKDAnz
		'
		Me.seJCHKDAnz.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
		Me.seJCHKDAnz.Location = New System.Drawing.Point(114, 66)
		Me.seJCHKDAnz.MenuManager = Me.BarManager1
		Me.seJCHKDAnz.Name = "seJCHKDAnz"
		Me.seJCHKDAnz.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.seJCHKDAnz.Properties.Mask.EditMask = "f0"
		Me.seJCHKDAnz.Size = New System.Drawing.Size(52, 20)
		Me.seJCHKDAnz.TabIndex = 2
		'
		'chJCH_Xing_Company_Is_Poc
		'
		Me.chJCH_Xing_Company_Is_Poc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.chJCH_Xing_Company_Is_Poc.Location = New System.Drawing.Point(308, 263)
		Me.chJCH_Xing_Company_Is_Poc.Name = "chJCH_Xing_Company_Is_Poc"
		Me.chJCH_Xing_Company_Is_Poc.Properties.AllowFocused = False
		Me.chJCH_Xing_Company_Is_Poc.Properties.Appearance.Options.UseTextOptions = True
		Me.chJCH_Xing_Company_Is_Poc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chJCH_Xing_Company_Is_Poc.Properties.Caption = "Xing_Company_Is_Proc"
		Me.chJCH_Xing_Company_Is_Poc.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far
		Me.chJCH_Xing_Company_Is_Poc.Size = New System.Drawing.Size(176, 20)
		Me.chJCH_Xing_Company_Is_Poc.TabIndex = 10
		'
		'txtJCH_Xing_Company_Profile_URL
		'
		Me.txtJCH_Xing_Company_Profile_URL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtJCH_Xing_Company_Profile_URL.Location = New System.Drawing.Point(161, 237)
		Me.txtJCH_Xing_Company_Profile_URL.Name = "txtJCH_Xing_Company_Profile_URL"
		Me.txtJCH_Xing_Company_Profile_URL.Size = New System.Drawing.Size(323, 20)
		Me.txtJCH_Xing_Company_Profile_URL.TabIndex = 9
		'
		'Label70
		'
		Me.Label70.AutoSize = True
		Me.Label70.Location = New System.Drawing.Point(12, 18)
		Me.Label70.Name = "Label70"
		Me.Label70.Size = New System.Drawing.Size(81, 13)
		Me.Label70.TabIndex = 186
		Me.Label70.Text = "Kundennummer"
		'
		'Label43
		'
		Me.Label43.AutoSize = True
		Me.Label43.Location = New System.Drawing.Point(12, 241)
		Me.Label43.Name = "Label43"
		Me.Label43.Size = New System.Drawing.Size(139, 13)
		Me.Label43.TabIndex = 163
		Me.Label43.Text = "Xing_Company_Profile_URL"
		'
		'txtJCH_Xing_Poster_URL
		'
		Me.txtJCH_Xing_Poster_URL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtJCH_Xing_Poster_URL.Location = New System.Drawing.Point(161, 211)
		Me.txtJCH_Xing_Poster_URL.Name = "txtJCH_Xing_Poster_URL"
		Me.txtJCH_Xing_Poster_URL.Size = New System.Drawing.Size(323, 20)
		Me.txtJCH_Xing_Poster_URL.TabIndex = 8
		'
		'Label69
		'
		Me.Label69.AutoSize = True
		Me.Label69.Location = New System.Drawing.Point(12, 70)
		Me.Label69.Name = "Label69"
		Me.Label69.Size = New System.Drawing.Size(71, 13)
		Me.Label69.TabIndex = 184
		Me.Label69.Text = "Anzahl Plätze"
		'
		'Label37
		'
		Me.Label37.AutoSize = True
		Me.Label37.Location = New System.Drawing.Point(12, 215)
		Me.Label37.Name = "Label37"
		Me.Label37.Size = New System.Drawing.Size(89, 13)
		Me.Label37.TabIndex = 160
		Me.Label37.Text = "Xing_Poster_URL"
		'
		'txtJCH_Bewerber_URL
		'
		Me.txtJCH_Bewerber_URL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtJCH_Bewerber_URL.Location = New System.Drawing.Point(114, 185)
		Me.txtJCH_Bewerber_URL.Name = "txtJCH_Bewerber_URL"
		Me.txtJCH_Bewerber_URL.Size = New System.Drawing.Size(370, 20)
		Me.txtJCH_Bewerber_URL.TabIndex = 7
		Me.txtJCH_Bewerber_URL.Visible = False
		'
		'Label32
		'
		Me.Label32.AutoSize = True
		Me.Label32.Location = New System.Drawing.Point(12, 189)
		Me.Label32.Name = "Label32"
		Me.Label32.Size = New System.Drawing.Size(78, 13)
		Me.Label32.TabIndex = 180
		Me.Label32.Text = "Bewerber_URL"
		Me.Label32.Visible = False
		'
		'txtJCH_Direkt_URL
		'
		Me.txtJCH_Direkt_URL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtJCH_Direkt_URL.Location = New System.Drawing.Point(114, 159)
		Me.txtJCH_Direkt_URL.Name = "txtJCH_Direkt_URL"
		Me.txtJCH_Direkt_URL.Size = New System.Drawing.Size(370, 20)
		Me.txtJCH_Direkt_URL.TabIndex = 6
		'
		'txtJCH_Our_URL
		'
		Me.txtJCH_Our_URL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtJCH_Our_URL.Location = New System.Drawing.Point(114, 133)
		Me.txtJCH_Our_URL.Name = "txtJCH_Our_URL"
		Me.txtJCH_Our_URL.Size = New System.Drawing.Size(370, 20)
		Me.txtJCH_Our_URL.TabIndex = 5
		'
		'txtJCH_Logo_ID
		'
		Me.txtJCH_Logo_ID.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtJCH_Logo_ID.Location = New System.Drawing.Point(380, 107)
		Me.txtJCH_Logo_ID.Name = "txtJCH_Logo_ID"
		Me.txtJCH_Logo_ID.Size = New System.Drawing.Size(104, 20)
		Me.txtJCH_Logo_ID.TabIndex = 177
		Me.txtJCH_Logo_ID.Visible = False
		'
		'txtJCH_Layout_ID
		'
		Me.txtJCH_Layout_ID.Location = New System.Drawing.Point(114, 107)
		Me.txtJCH_Layout_ID.Name = "txtJCH_Layout_ID"
		Me.txtJCH_Layout_ID.Size = New System.Drawing.Size(104, 20)
		Me.txtJCH_Layout_ID.TabIndex = 4
		Me.txtJCH_Layout_ID.Visible = False
		'
		'txtJCH_Organisation_ID
		'
		Me.txtJCH_Organisation_ID.Location = New System.Drawing.Point(114, 14)
		Me.txtJCH_Organisation_ID.Name = "txtJCH_Organisation_ID"
		Me.txtJCH_Organisation_ID.Size = New System.Drawing.Size(104, 20)
		Me.txtJCH_Organisation_ID.TabIndex = 0
		'
		'Label39
		'
		Me.Label39.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.Label39.AutoSize = True
		Me.Label39.Location = New System.Drawing.Point(327, 111)
		Me.Label39.Name = "Label39"
		Me.Label39.Size = New System.Drawing.Size(47, 13)
		Me.Label39.TabIndex = 173
		Me.Label39.Text = "Logo_ID"
		Me.Label39.Visible = False
		'
		'Label42
		'
		Me.Label42.AutoSize = True
		Me.Label42.Location = New System.Drawing.Point(12, 111)
		Me.Label42.Name = "Label42"
		Me.Label42.Size = New System.Drawing.Size(57, 13)
		Me.Label42.TabIndex = 172
		Me.Label42.Text = "Layout_ID"
		Me.Label42.Visible = False
		'
		'Label36
		'
		Me.Label36.AutoSize = True
		Me.Label36.Location = New System.Drawing.Point(12, 137)
		Me.Label36.Name = "Label36"
		Me.Label36.Size = New System.Drawing.Size(50, 13)
		Me.Label36.TabIndex = 170
		Me.Label36.Text = "Our_URL"
		'
		'Label33
		'
		Me.Label33.AutoSize = True
		Me.Label33.Location = New System.Drawing.Point(12, 163)
		Me.Label33.Name = "Label33"
		Me.Label33.Size = New System.Drawing.Size(60, 13)
		Me.Label33.TabIndex = 171
		Me.Label33.Text = "Direkt_URL"
		'
		'xtabOurJobplattform
		'
		Me.xtabOurJobplattform.Controls.Add(Me.grpOurJobPlattformData)
		Me.xtabOurJobplattform.Name = "xtabOurJobplattform"
		Me.xtabOurJobplattform.Size = New System.Drawing.Size(558, 483)
		Me.xtabOurJobplattform.Text = "Unsere Jobplattform"
		'
		'grpOurJobPlattformData
		'
		Me.grpOurJobPlattformData.Controls.Add(Me.PanelControl19)
		Me.grpOurJobPlattformData.Dock = System.Windows.Forms.DockStyle.Fill
		Me.grpOurJobPlattformData.Location = New System.Drawing.Point(0, 0)
		Me.grpOurJobPlattformData.Name = "grpOurJobPlattformData"
		Me.grpOurJobPlattformData.Size = New System.Drawing.Size(558, 483)
		Me.grpOurJobPlattformData.TabIndex = 0
		'
		'PanelControl19
		'
		Me.PanelControl19.Controls.Add(Me.picJCHLogo)
		Me.PanelControl19.Controls.Add(Me.Label34)
		Me.PanelControl19.Location = New System.Drawing.Point(20, 40)
		Me.PanelControl19.Name = "PanelControl19"
		Me.PanelControl19.Size = New System.Drawing.Size(741, 103)
		Me.PanelControl19.TabIndex = 195
		'
		'picJCHLogo
		'
		Me.picJCHLogo.EditValue = CType(resources.GetObject("picJCHLogo.EditValue"), Object)
		Me.picJCHLogo.Location = New System.Drawing.Point(134, 22)
		Me.picJCHLogo.MenuManager = Me.BarManager1
		Me.picJCHLogo.Name = "picJCHLogo"
		Me.picJCHLogo.Size = New System.Drawing.Size(214, 40)
		Me.picJCHLogo.TabIndex = 3
		'
		'Label34
		'
		Me.Label34.AutoSize = True
		Me.Label34.Location = New System.Drawing.Point(32, 26)
		Me.Label34.Name = "Label34"
		Me.Label34.Size = New System.Drawing.Size(67, 13)
		Me.Label34.TabIndex = 194
		Me.Label34.Text = "Unsere Logo"
		'
		'xtabSettingOstjob
		'
		Me.xtabSettingOstjob.Controls.Add(Me.GroupControl1)
		Me.xtabSettingOstjob.Name = "xtabSettingOstjob"
		Me.xtabSettingOstjob.Size = New System.Drawing.Size(558, 483)
		Me.xtabSettingOstjob.Text = "ostjob.ch"
		'
		'GroupControl1
		'
		Me.GroupControl1.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.GroupControl1.Appearance.Options.UseBackColor = True
		Me.GroupControl1.Controls.Add(Me.PanelControl7)
		Me.GroupControl1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.GroupControl1.Location = New System.Drawing.Point(0, 0)
		Me.GroupControl1.Name = "GroupControl1"
		Me.GroupControl1.Padding = New System.Windows.Forms.Padding(120, 0, 0, 0)
		Me.GroupControl1.Size = New System.Drawing.Size(558, 483)
		Me.GroupControl1.TabIndex = 1
		'
		'PanelControl7
		'
		Me.PanelControl7.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.PanelControl7.Appearance.BackColor = System.Drawing.Color.Transparent
		Me.PanelControl7.Appearance.Options.UseBackColor = True
		Me.PanelControl7.Controls.Add(Me.txtOstjob_Bewerberform)
		Me.PanelControl7.Controls.Add(Me.Label52)
		Me.PanelControl7.Controls.Add(Me.txtOstjob_DirectlinkiFrame)
		Me.PanelControl7.Controls.Add(Me.Label50)
		Me.PanelControl7.Controls.Add(Me.Label94)
		Me.PanelControl7.Controls.Add(Me.seOstJobAddDayToDate)
		Me.PanelControl7.Controls.Add(Me.Label95)
		Me.PanelControl7.Controls.Add(Me.cmdSaveOstJobAccount)
		Me.PanelControl7.Controls.Add(Me.seOstJobKDAnz)
		Me.PanelControl7.Controls.Add(Me.Label98)
		Me.PanelControl7.Controls.Add(Me.Label100)
		Me.PanelControl7.Controls.Add(Me.txtOstJob_OrganisationID)
		Me.PanelControl7.Location = New System.Drawing.Point(20, 40)
		Me.PanelControl7.Name = "PanelControl7"
		Me.PanelControl7.Size = New System.Drawing.Size(515, 137)
		Me.PanelControl7.TabIndex = 175
		'
		'txtOstjob_Bewerberform
		'
		Me.txtOstjob_Bewerberform.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtOstjob_Bewerberform.Location = New System.Drawing.Point(114, 92)
		Me.txtOstjob_Bewerberform.Name = "txtOstjob_Bewerberform"
		Me.txtOstjob_Bewerberform.Size = New System.Drawing.Size(387, 20)
		Me.txtOstjob_Bewerberform.TabIndex = 205
		'
		'Label52
		'
		Me.Label52.AutoSize = True
		Me.Label52.Location = New System.Drawing.Point(12, 96)
		Me.Label52.Name = "Label52"
		Me.Label52.Size = New System.Drawing.Size(93, 13)
		Me.Label52.TabIndex = 206
		Me.Label52.Text = "Bewerberformular"
		'
		'txtOstjob_DirectlinkiFrame
		'
		Me.txtOstjob_DirectlinkiFrame.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
			Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtOstjob_DirectlinkiFrame.Location = New System.Drawing.Point(114, 66)
		Me.txtOstjob_DirectlinkiFrame.Name = "txtOstjob_DirectlinkiFrame"
		Me.txtOstjob_DirectlinkiFrame.Size = New System.Drawing.Size(387, 20)
		Me.txtOstjob_DirectlinkiFrame.TabIndex = 203
		'
		'Label50
		'
		Me.Label50.AutoSize = True
		Me.Label50.Location = New System.Drawing.Point(12, 70)
		Me.Label50.Name = "Label50"
		Me.Label50.Size = New System.Drawing.Size(85, 13)
		Me.Label50.TabIndex = 204
		Me.Label50.Text = "Direktlink iFrame"
		'
		'Label94
		'
		Me.Label94.AutoSize = True
		Me.Label94.Location = New System.Drawing.Point(334, 42)
		Me.Label94.Name = "Label94"
		Me.Label94.Size = New System.Drawing.Size(31, 13)
		Me.Label94.TabIndex = 202
		Me.Label94.Text = "Tage"
		Me.Label94.Visible = False
		'
		'seOstJobAddDayToDate
		'
		Me.seOstJobAddDayToDate.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
		Me.seOstJobAddDayToDate.Location = New System.Drawing.Point(276, 40)
		Me.seOstJobAddDayToDate.MenuManager = Me.BarManager1
		Me.seOstJobAddDayToDate.Name = "seOstJobAddDayToDate"
		Me.seOstJobAddDayToDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.seOstJobAddDayToDate.Properties.Mask.EditMask = "f0"
		Me.seOstJobAddDayToDate.Size = New System.Drawing.Size(52, 20)
		Me.seOstJobAddDayToDate.TabIndex = 200
		'
		'Label95
		'
		Me.Label95.Location = New System.Drawing.Point(174, 44)
		Me.Label95.Name = "Label95"
		Me.Label95.Size = New System.Drawing.Size(96, 13)
		Me.Label95.TabIndex = 201
		Me.Label95.Text = "End-Datum +"
		Me.Label95.TextAlign = System.Drawing.ContentAlignment.TopRight
		'
		'cmdSaveOstJobAccount
		'
		Me.cmdSaveOstJobAccount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdSaveOstJobAccount.ImageOptions.Image = CType(resources.GetObject("cmdSaveOstJobAccount.ImageOptions.Image"), System.Drawing.Image)
		Me.cmdSaveOstJobAccount.Location = New System.Drawing.Point(409, 11)
		Me.cmdSaveOstJobAccount.Name = "cmdSaveOstJobAccount"
		Me.cmdSaveOstJobAccount.Size = New System.Drawing.Size(92, 28)
		Me.cmdSaveOstJobAccount.TabIndex = 11
		Me.cmdSaveOstJobAccount.Text = "Speichern"
		'
		'seOstJobKDAnz
		'
		Me.seOstJobKDAnz.EditValue = New Decimal(New Integer() {0, 0, 0, 0})
		Me.seOstJobKDAnz.Location = New System.Drawing.Point(114, 40)
		Me.seOstJobKDAnz.MenuManager = Me.BarManager1
		Me.seOstJobKDAnz.Name = "seOstJobKDAnz"
		Me.seOstJobKDAnz.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
		Me.seOstJobKDAnz.Properties.Mask.EditMask = "f0"
		Me.seOstJobKDAnz.Size = New System.Drawing.Size(52, 20)
		Me.seOstJobKDAnz.TabIndex = 2
		'
		'Label98
		'
		Me.Label98.AutoSize = True
		Me.Label98.Location = New System.Drawing.Point(12, 18)
		Me.Label98.Name = "Label98"
		Me.Label98.Size = New System.Drawing.Size(81, 13)
		Me.Label98.TabIndex = 186
		Me.Label98.Text = "Kundennummer"
		'
		'Label100
		'
		Me.Label100.AutoSize = True
		Me.Label100.Location = New System.Drawing.Point(12, 44)
		Me.Label100.Name = "Label100"
		Me.Label100.Size = New System.Drawing.Size(71, 13)
		Me.Label100.TabIndex = 184
		Me.Label100.Text = "Anzahl Plätze"
		'
		'txtOstJob_OrganisationID
		'
		Me.txtOstJob_OrganisationID.Location = New System.Drawing.Point(114, 14)
		Me.txtOstJob_OrganisationID.Name = "txtOstJob_OrganisationID"
		Me.txtOstJob_OrganisationID.Size = New System.Drawing.Size(104, 20)
		Me.txtOstJob_OrganisationID.TabIndex = 0
		'
		'xtabSettingsuedost
		'
		Me.xtabSettingsuedost.Name = "xtabSettingsuedost"
		Me.xtabSettingsuedost.Size = New System.Drawing.Size(558, 483)
		Me.xtabSettingsuedost.Text = "südostschweiz.ch"
		'
		'gLizenzen
		'
		Me.gLizenzen.Controls.Add(Me.GroupBox8)
		Me.gLizenzen.Controls.Add(Me.GroupBox10)
		Me.gLizenzen.Dock = System.Windows.Forms.DockStyle.Fill
		Me.gLizenzen.Location = New System.Drawing.Point(12, 12)
		Me.gLizenzen.Name = "gLizenzen"
		Me.gLizenzen.Size = New System.Drawing.Size(695, 373)
		Me.gLizenzen.TabIndex = 203
		Me.gLizenzen.Text = "Lizenzen verwalten"
		'
		'pLizenzen
		'
		Me.pLizenzen.Controls.Add(Me.gLizenzen)
		Me.pLizenzen.Location = New System.Drawing.Point(271, 164)
		Me.pLizenzen.Name = "pLizenzen"
		Me.pLizenzen.Padding = New System.Windows.Forms.Padding(10)
		Me.pLizenzen.Size = New System.Drawing.Size(719, 397)
		Me.pLizenzen.TabIndex = 204
		Me.pLizenzen.Visible = False
		'
		'frmTemplateDetail
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(1710, 758)
		Me.Controls.Add(Me.pLizenzen)
		Me.Controls.Add(Me.pMail_Tpl)
		Me.Controls.Add(Me.pLColor)
		Me.Controls.Add(Me.pGlobal)
		Me.Controls.Add(Me.pCom_WOS)
		Me.Controls.Add(Me.pJobplattforms)
		Me.Controls.Add(Me.sccMain)
		Me.Controls.Add(Me.pMailFax)
		Me.Controls.Add(Me.cmdClose)
		Me.Controls.Add(Me.Label13)
		Me.Controls.Add(Me.Label4)
		Me.Controls.Add(Me.Label3)
		Me.Controls.Add(Me.PictureBox1)
		Me.Controls.Add(Me.barDockControlLeft)
		Me.Controls.Add(Me.barDockControlRight)
		Me.Controls.Add(Me.barDockControlBottom)
		Me.Controls.Add(Me.barDockControlTop)
		Me.IconOptions.Icon = CType(resources.GetObject("frmTemplateDetail.IconOptions.Icon"), System.Drawing.Icon)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.MinimumSize = New System.Drawing.Size(1181, 706)
		Me.Name = "frmTemplateDetail"
		Me.Text = "Einstellungen"
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PictureBox1.ResumeLayout(False)
		CType(Me.txtFaxExtension.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtFaxForwarder.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pnlECall, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pnlECall.ResumeLayout(False)
		Me.pnlECall.PerformLayout()
		CType(Me.txteCallToken.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txteCallNotification.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txteCallJobID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txteCallSubject.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txteCallHeaderInfo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txteCallHeaderID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txteCallFromText.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox4.ResumeLayout(False)
		Me.GroupBox4.PerformLayout()
		CType(Me.chkSSL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.BarManager1, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtSMTP.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtSMTPPort.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cboSmtpDeliveryMethod.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox2.ResumeLayout(False)
		Me.GroupBox2.PerformLayout()
		CType(Me.chkJobChannelPriority.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chksendproposeattachmenttowos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkenablecustomerwos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkenableemployeewos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkenablevacancywos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Ver_ID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_KD_ID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_MA_ID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_Vak_ID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox7.ResumeLayout(False)
		Me.GroupBox7.PerformLayout()
		CType(Me.txtCockpitPicture.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtCockpitWWW.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox3.ResumeLayout(False)
		Me.GroupBox3.PerformLayout()
		CType(Me.txtZHDDocWWW.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtZHDDocMailBetreff.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtKDDocWWW.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtKDDocMailBetreff.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox6.ResumeLayout(False)
		Me.GroupBox6.PerformLayout()
		CType(Me.txtMADocWWW.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtMADocMailBetreff.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox5.ResumeLayout(False)
		Me.GroupBox5.PerformLayout()
		CType(Me.txtArbgDocMailBetreff.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtZVDocMailBetreff.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cleLNormal_0.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cboLFormStyle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pGlobal, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pGlobal.ResumeLayout(False)
		CType(Me.gGlobal, System.ComponentModel.ISupportInitialize).EndInit()
		Me.gGlobal.ResumeLayout(False)
		Me.GroupPanel10.ResumeLayout(False)
		CType(Me.chkopeninvoiceformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkopenadvancedpaymentformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkopenreportsformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkopeneinsatzformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkopencustomerformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkopenemployeeformmorethanonce.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupPanel9.ResumeLayout(False)
		Me.GroupPanel9.PerformLayout()
		CType(Me.chkAllowAutoFilterConditionChange.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.ceMandantColor.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkGLLDebug.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cboautofilterconditionnr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cboAutoFilterConditionDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupPanel8.ResumeLayout(False)
		Me.GroupPanel8.PerformLayout()
		CType(Me.chksavecustomerinvoicescanintowos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chksavecustomerreportscanintowos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chksavecustomeremploymentscanintowos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chksaveemployeepayrollscanintowos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chksaveemployeereportscanintowos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chksaveemployeeemploymentscanintowos.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkimportscanreportzeroamount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkimportscanreporttoboth.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtnotificationintervalperiodeforreport.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkchangeownreportforfinishingflag.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtnotificationintervalperiode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.seRPScanSize.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkComatic.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkCSOPList.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkKMUFactoring.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkParifond.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkSwifac.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkAbacus.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkSesam.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkaskonexit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkLUpdate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl3.ResumeLayout(False)
		Me.PanelControl3.PerformLayout()
		CType(Me.seAnzTestRP.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.seAnzTestCheck.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkRPOpenWeek.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkRPWeekMust.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pLColor, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pLColor.ResumeLayout(False)
		CType(Me.gLColor, System.ComponentModel.ISupportInitialize).EndInit()
		Me.gLColor.ResumeLayout(False)
		CType(Me.glRPData, System.ComponentModel.ISupportInitialize).EndInit()
		Me.glRPData.ResumeLayout(False)
		Me.GroupPanel7.ResumeLayout(False)
		Me.GroupPanel6.ResumeLayout(False)
		Me.GroupPanel6.PerformLayout()
		CType(Me.txtLPfad_3.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtLPfad_2.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtLPfad_1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtLPfad_0.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupPanel5.ResumeLayout(False)
		Me.GroupPanel5.PerformLayout()
		CType(Me.cboLNavStyle.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pNavbar, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pNavbar.ResumeLayout(False)
		CType(Me.nvMain, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.pMail_Tpl, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pMail_Tpl.ResumeLayout(False)
		CType(Me.gMail_Tpl, System.ComponentModel.ISupportInitialize).EndInit()
		Me.gMail_Tpl.ResumeLayout(False)
		CType(Me.pCom_WOS, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pCom_WOS.ResumeLayout(False)
		CType(Me.gWOS, System.ComponentModel.ISupportInitialize).EndInit()
		Me.gWOS.ResumeLayout(False)
		CType(Me.pMailFax, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pMailFax.ResumeLayout(False)
		CType(Me.gMailFax, System.ComponentModel.ISupportInitialize).EndInit()
		Me.gMailFax.ResumeLayout(False)
		CType(Me.xtabFax, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabFax.ResumeLayout(False)
		Me.xtabeCall.ResumeLayout(False)
		Me.xtabDavidFaxserver.ResumeLayout(False)
		CType(Me.PanelControl5, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl5.ResumeLayout(False)
		Me.PanelControl5.PerformLayout()
		CType(Me.txtDavidfaxserver.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox10.ResumeLayout(False)
		CType(Me.chkCresus.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkpmsearch.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkallowedemployeeweeklypayment.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkCVDropIn.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chkScanDropIN.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupBox8.ResumeLayout(False)
		CType(Me.txt_DVURL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_DVPW.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_DVRefNo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txt_DVUSName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.sccMain, System.ComponentModel.ISupportInitialize).EndInit()
		Me.sccMain.ResumeLayout(False)
		CType(Me.pJobplattforms, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pJobplattforms.ResumeLayout(False)
		CType(Me.XtraTabControl3, System.ComponentModel.ISupportInitialize).EndInit()
		Me.XtraTabControl3.ResumeLayout(False)
		Me.xtabSettingJobCH.ResumeLayout(False)
		CType(Me.gJCH, System.ComponentModel.ISupportInitialize).EndInit()
		Me.gJCH.ResumeLayout(False)
		CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl1.ResumeLayout(False)
		Me.PanelControl1.PerformLayout()
		CType(Me.txtJCHKDSubNr.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.seJCHKDSubAnz.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lstJCHKDSubNr, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl2.ResumeLayout(False)
		Me.PanelControl2.PerformLayout()
		CType(Me.seJCHAddDayToDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cboJCHAngebot_Art.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.seJCHKDAnz.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.chJCH_Xing_Company_Is_Poc.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtJCH_Xing_Company_Profile_URL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtJCH_Xing_Poster_URL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtJCH_Bewerber_URL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtJCH_Direkt_URL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtJCH_Our_URL.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtJCH_Logo_ID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtJCH_Layout_ID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtJCH_Organisation_ID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabOurJobplattform.ResumeLayout(False)
		CType(Me.grpOurJobPlattformData, System.ComponentModel.ISupportInitialize).EndInit()
		Me.grpOurJobPlattformData.ResumeLayout(False)
		CType(Me.PanelControl19, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl19.ResumeLayout(False)
		Me.PanelControl19.PerformLayout()
		CType(Me.picJCHLogo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		Me.xtabSettingOstjob.ResumeLayout(False)
		CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.GroupControl1.ResumeLayout(False)
		CType(Me.PanelControl7, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelControl7.ResumeLayout(False)
		Me.PanelControl7.PerformLayout()
		CType(Me.txtOstjob_Bewerberform.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtOstjob_DirectlinkiFrame.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.seOstJobAddDayToDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.seOstJobKDAnz.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.txtOstJob_OrganisationID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.gLizenzen, System.ComponentModel.ISupportInitialize).EndInit()
		Me.gLizenzen.ResumeLayout(False)
		CType(Me.pLizenzen, System.ComponentModel.ISupportInitialize).EndInit()
		Me.pLizenzen.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()

	End Sub
	Friend WithEvents cmdClose As DevExpress.XtraEditors.SimpleButton ' System.Windows.Forms.Button
	Friend WithEvents Label4 As System.Windows.Forms.Label
	Friend WithEvents Label3 As System.Windows.Forms.Label
	Friend WithEvents PictureBox1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents txtFaxExtension As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label7 As System.Windows.Forms.Label
	Friend WithEvents Label8 As System.Windows.Forms.Label
	Friend WithEvents txtFaxForwarder As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label9 As System.Windows.Forms.Label
	Friend WithEvents pnlECall As DevExpress.XtraEditors.PanelControl
	Friend WithEvents Label10 As System.Windows.Forms.Label
	Friend WithEvents txt_Vak_ID As DevExpress.XtraEditors.TextEdit
	Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
	Friend WithEvents txt_KD_ID As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label12 As System.Windows.Forms.Label
	Friend WithEvents txt_MA_ID As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label11 As System.Windows.Forms.Label
	Friend WithEvents txt_Ver_ID As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label14 As System.Windows.Forms.Label
	Friend WithEvents Label20 As System.Windows.Forms.Label
	Friend WithEvents txtSMTP As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label21 As System.Windows.Forms.Label
	Friend WithEvents txtSMTPPort As DevExpress.XtraEditors.TextEdit
	Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
	Friend WithEvents Label18 As System.Windows.Forms.Label
	Friend WithEvents txtKDDocWWW As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label19 As System.Windows.Forms.Label
	Friend WithEvents txtMADocWWW As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label16 As System.Windows.Forms.Label
	Friend WithEvents txtKDDocMailBetreff As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label17 As System.Windows.Forms.Label
	Friend WithEvents txtMADocMailBetreff As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label23 As System.Windows.Forms.Label
	Friend WithEvents txtZHDDocWWW As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label24 As System.Windows.Forms.Label
	Friend WithEvents txtZHDDocMailBetreff As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label5 As System.Windows.Forms.Label
	Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
	Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
	Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
	Friend WithEvents Label29 As System.Windows.Forms.Label
	Friend WithEvents txtArbgDocMailBetreff As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label26 As System.Windows.Forms.Label
	Friend WithEvents txtZVDocMailBetreff As DevExpress.XtraEditors.TextEdit
	Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
	Friend WithEvents Label15 As System.Windows.Forms.Label
	Friend WithEvents txtCockpitWWW As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label13 As System.Windows.Forms.Label
	Friend WithEvents lblTemplatePath_1 As System.Windows.Forms.Label
	Friend WithEvents Label22 As System.Windows.Forms.Label
	Friend WithEvents txtCockpitPicture As DevExpress.XtraEditors.TextEdit
	Friend WithEvents pNavbar As DevExpress.XtraEditors.PanelControl
	Friend WithEvents nvMain As DevExpress.XtraNavBar.NavBarControl
	Friend WithEvents nvgCom As DevExpress.XtraNavBar.NavBarGroup
	Friend WithEvents nvgLGSetting As DevExpress.XtraNavBar.NavBarGroup
	Friend WithEvents nvgJob As DevExpress.XtraNavBar.NavBarGroup
	Friend WithEvents xscMain As DevExpress.XtraEditors.XtraScrollableControl
	Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents seAnzTestRP As DevExpress.XtraEditors.SpinEdit
	Friend WithEvents Label47 As System.Windows.Forms.Label
	Friend WithEvents seAnzTestCheck As DevExpress.XtraEditors.SpinEdit
	Friend WithEvents Label46 As System.Windows.Forms.Label
	Friend WithEvents chkRPOpenWeek As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkRPWeekMust As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents Label41 As System.Windows.Forms.Label
	Friend WithEvents Label35 As System.Windows.Forms.Label
	Friend WithEvents chkaskonexit As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkLUpdate As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents cleLNormal_0 As DevExpress.XtraEditors.ColorEdit
	Friend WithEvents Label54 As System.Windows.Forms.Label
	Friend WithEvents cboLFormStyle As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Label51 As System.Windows.Forms.Label
	Friend WithEvents nviWOS As DevExpress.XtraNavBar.NavBarItem
	Friend WithEvents nviMailFax As DevExpress.XtraNavBar.NavBarItem
	Friend WithEvents nvLayout As DevExpress.XtraNavBar.NavBarGroup
	Friend WithEvents pCom_WOS As DevExpress.XtraEditors.PanelControl
	Friend WithEvents gWOS As DevExpress.XtraEditors.GroupControl
	Friend WithEvents pMailFax As DevExpress.XtraEditors.PanelControl
	Friend WithEvents gMailFax As DevExpress.XtraEditors.GroupControl
	Friend WithEvents pGlobal As DevExpress.XtraEditors.PanelControl
	Friend WithEvents pMail_Tpl As DevExpress.XtraEditors.PanelControl
	Friend WithEvents gMail_Tpl As DevExpress.XtraEditors.GroupControl
	Friend WithEvents nviMailTpl As DevExpress.XtraNavBar.NavBarItem
	Friend WithEvents nviJCH As DevExpress.XtraNavBar.NavBarItem
	Friend WithEvents nviJWinner As DevExpress.XtraNavBar.NavBarItem
	Friend WithEvents nviMetro_0 As DevExpress.XtraNavBar.NavBarItem
	Friend WithEvents pLColor As DevExpress.XtraEditors.PanelControl
	Friend WithEvents nviLFieldColor As DevExpress.XtraNavBar.NavBarItem
	Friend WithEvents glRPData As DevExpress.XtraEditors.GroupControl
	Friend WithEvents BarManager1 As DevExpress.XtraBars.BarManager
	Friend WithEvents barDockControlTop As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlBottom As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlLeft As DevExpress.XtraBars.BarDockControl
	Friend WithEvents barDockControlRight As DevExpress.XtraBars.BarDockControl
	Friend WithEvents nviGlobalSetting As DevExpress.XtraNavBar.NavBarItem
	Friend WithEvents seRPScanSize As DevExpress.XtraEditors.SpinEdit
	Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton ' System.Windows.Forms.Button
	Friend WithEvents StyleManager1 As DevComponents.DotNetBar.StyleManager
	Friend WithEvents GroupPanel5 As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents gLColor As DevExpress.XtraEditors.GroupControl
	Friend WithEvents Label48 As System.Windows.Forms.Label
	Friend WithEvents cboLNavStyle As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents GroupPanel6 As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents txtLPfad_2 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents txtLPfad_1 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents txtLPfad_0 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents GroupPanel7 As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents txtLPfad_3 As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents Label44 As System.Windows.Forms.Label
	Friend WithEvents Label49 As System.Windows.Forms.Label
	Friend WithEvents Label45 As System.Windows.Forms.Label
	Friend WithEvents lblLMDPfad As System.Windows.Forms.Label
	Friend WithEvents gGlobal As DevExpress.XtraEditors.GroupControl
	Friend WithEvents GroupPanel8 As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents Label57 As System.Windows.Forms.Label
	Friend WithEvents GroupPanel9 As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents chkGLLDebug As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents nbgFeldinformationen As DevExpress.XtraNavBar.NavBarGroup
	Friend WithEvents chkParifond As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkSwifac As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkAbacus As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkSesam As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkComatic As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkCSOPList As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkKMUFactoring As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents Label66 As System.Windows.Forms.Label
	Friend WithEvents txteCallSubject As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label65 As System.Windows.Forms.Label
	Friend WithEvents txteCallHeaderInfo As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label64 As System.Windows.Forms.Label
	Friend WithEvents txteCallHeaderID As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label63 As System.Windows.Forms.Label
	Friend WithEvents txteCallFromText As DevExpress.XtraEditors.TextEdit
	Friend WithEvents xtabFax As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabDavidFaxserver As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents PanelControl5 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents xtabeCall As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents txteCallJobID As DevExpress.XtraEditors.TextEdit
	Friend WithEvents lbleCallJobID As System.Windows.Forms.Label
	Friend WithEvents Label67 As System.Windows.Forms.Label
	Friend WithEvents txteCallToken As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label68 As System.Windows.Forms.Label
	Friend WithEvents txteCallNotification As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label73 As System.Windows.Forms.Label
	Friend WithEvents txtDavidfaxserver As DevExpress.XtraEditors.TextEdit
	Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
	Friend WithEvents Label77 As System.Windows.Forms.Label
	Friend WithEvents txt_DVURL As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label76 As System.Windows.Forms.Label
	Friend WithEvents txt_DVPW As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label74 As System.Windows.Forms.Label
	Friend WithEvents txt_DVRefNo As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label75 As System.Windows.Forms.Label
	Friend WithEvents txt_DVUSName As DevExpress.XtraEditors.TextEdit
	Friend WithEvents sccMain As DevExpress.XtraEditors.SplitContainerControl
	Friend WithEvents pJobplattforms As DevExpress.XtraEditors.PanelControl
	Friend WithEvents gJCH As DevExpress.XtraEditors.GroupControl
	Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents cmdJCHKDSubDelete As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents cmdSaveSubNr As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents txtJCHKDSubNr As DevExpress.XtraEditors.TextEdit
	Friend WithEvents seJCHKDSubAnz As DevExpress.XtraEditors.SpinEdit
	Friend WithEvents Label71 As System.Windows.Forms.Label
	Friend WithEvents lstJCHKDSubNr As DevExpress.XtraEditors.ListBoxControl
	Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents Label72 As System.Windows.Forms.Label
	Friend WithEvents cboJCHAngebot_Art As DevExpress.XtraEditors.ComboBoxEdit
	Friend WithEvents picJCHLogo As DevExpress.XtraEditors.PictureEdit
	Friend WithEvents Label34 As System.Windows.Forms.Label
	Friend WithEvents cmdSaveJCHAccount As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents seJCHKDAnz As DevExpress.XtraEditors.SpinEdit
	Friend WithEvents chJCH_Xing_Company_Is_Poc As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents txtJCH_Xing_Company_Profile_URL As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label70 As System.Windows.Forms.Label
	Friend WithEvents Label43 As System.Windows.Forms.Label
	Friend WithEvents txtJCH_Xing_Poster_URL As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label69 As System.Windows.Forms.Label
	Friend WithEvents Label37 As System.Windows.Forms.Label
	Friend WithEvents txtJCH_Bewerber_URL As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label32 As System.Windows.Forms.Label
	Friend WithEvents txtJCH_Direkt_URL As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtJCH_Our_URL As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtJCH_Logo_ID As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtJCH_Layout_ID As DevExpress.XtraEditors.TextEdit
	Friend WithEvents txtJCH_Organisation_ID As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label39 As System.Windows.Forms.Label
	Friend WithEvents Label42 As System.Windows.Forms.Label
	Friend WithEvents Label36 As System.Windows.Forms.Label
	Friend WithEvents Label33 As System.Windows.Forms.Label
	Friend WithEvents Label79 As System.Windows.Forms.Label
	Friend WithEvents seJCHAddDayToDate As DevExpress.XtraEditors.SpinEdit
	Friend WithEvents Label78 As System.Windows.Forms.Label
	Friend WithEvents nviLicense As DevExpress.XtraNavBar.NavBarItem
	Friend WithEvents XtraTabControl3 As DevExpress.XtraTab.XtraTabControl
	Friend WithEvents xtabSettingJobCH As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabSettingOstjob As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents xtabSettingsuedost As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl
	Friend WithEvents PanelControl7 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents Label94 As System.Windows.Forms.Label
	Friend WithEvents seOstJobAddDayToDate As DevExpress.XtraEditors.SpinEdit
	Friend WithEvents Label95 As System.Windows.Forms.Label
	Friend WithEvents cmdSaveOstJobAccount As DevExpress.XtraEditors.SimpleButton
	Friend WithEvents seOstJobKDAnz As DevExpress.XtraEditors.SpinEdit
	Friend WithEvents Label98 As System.Windows.Forms.Label
	Friend WithEvents Label100 As System.Windows.Forms.Label
	Friend WithEvents txtOstJob_OrganisationID As DevExpress.XtraEditors.TextEdit
	Friend WithEvents xtabOurJobplattform As DevExpress.XtraTab.XtraTabPage
	Friend WithEvents grpOurJobPlattformData As DevExpress.XtraEditors.GroupControl
	Friend WithEvents PanelControl19 As DevExpress.XtraEditors.PanelControl
	Friend WithEvents Bar1 As DevExpress.XtraBars.Bar
	Friend WithEvents bsiInfo_1 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bsiInfo_2 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bsiInfo_3 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents bsiInfo_4 As DevExpress.XtraBars.BarStaticItem
	Friend WithEvents ceMandantColor As DevExpress.XtraEditors.ColorEdit
	Friend WithEvents Label97 As System.Windows.Forms.Label
	Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
	Friend WithEvents GroupPanel10 As DevComponents.DotNetBar.Controls.GroupPanel
	Friend WithEvents chkopeninvoiceformmorethanonce As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkopenadvancedpaymentformmorethanonce As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkopenreportsformmorethanonce As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkopeneinsatzformmorethanonce As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkopencustomerformmorethanonce As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkopenemployeeformmorethanonce As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents pLizenzen As DevExpress.XtraEditors.PanelControl
	Friend WithEvents gLizenzen As DevExpress.XtraEditors.GroupControl
	Friend WithEvents Label6 As System.Windows.Forms.Label
	Friend WithEvents txtnotificationintervalperiode As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label2 As System.Windows.Forms.Label
	Friend WithEvents chkchangeownreportforfinishingflag As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents txtnotificationintervalperiodeforreport As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label38 As System.Windows.Forms.Label
	Friend WithEvents chkimportscanreporttoboth As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkimportscanreportzeroamount As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents Label40 As System.Windows.Forms.Label
	Friend WithEvents txtOstjob_DirectlinkiFrame As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label50 As System.Windows.Forms.Label
	Friend WithEvents txtOstjob_Bewerberform As DevExpress.XtraEditors.TextEdit
	Friend WithEvents Label52 As System.Windows.Forms.Label
	Friend WithEvents chkCVDropIn As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkScanDropIN As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chksaveemployeeemploymentscanintowos As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chksaveemployeereportscanintowos As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chksavecustomeremploymentscanintowos As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chksaveemployeepayrollscanintowos As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chksavecustomerinvoicescanintowos As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chksavecustomerreportscanintowos As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkallowedemployeeweeklypayment As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkAllowAutoFilterConditionChange As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents Label61 As Label
	Friend WithEvents Label53 As Label
	Friend WithEvents cboautofilterconditionnr As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents cboAutoFilterConditionDate As DevExpress.XtraEditors.LookUpEdit
	Friend WithEvents chkSSL As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkpmsearch As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chksendproposeattachmenttowos As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkenablecustomerwos As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkenableemployeewos As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkenablevacancywos As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkCresus As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents chkJobChannelPriority As DevExpress.XtraEditors.CheckEdit
	Friend WithEvents Label1 As Label
	Friend WithEvents cboSmtpDeliveryMethod As DevExpress.XtraEditors.LookUpEdit
End Class
