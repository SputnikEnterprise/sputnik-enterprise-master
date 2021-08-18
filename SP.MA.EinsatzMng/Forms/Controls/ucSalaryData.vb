
Imports SPS.SalaryValueCalculation
Imports SPS.SalaryValueCalculation.ESSalaryValueCalculation
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Customer
Imports System.ComponentModel
Imports DevExpress.XtraEditors.Repository
Imports SP.DatabaseAccess.ES
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten

Namespace UI

  Public Class ucSalaryData

#Region "Private Fields"

    Private m_ESSalayDataList As IEnumerable(Of ESSalaryData)

    ''' <summary>
    ''' The current ESNr.
    ''' </summary>
    Private m_CurrentESNr As Integer?

    ''' <summary>
    ''' The current es Lohn nr.
    ''' </summary>
    Private m_CurrentESLohnNr As Integer?

    ''' <summary>
    ''' The employee database access.
    ''' </summary>
    Private m_EmployeeDatabaseAccess As IEmployeeDatabaseAccess

    ''' <summary>
    ''' The customer database access.
    ''' </summary>
    Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

    ''' <summary>
    ''' Check edit for active symbol.
    ''' </summary>
    Private m_CheckEditActive As RepositoryItemCheckEdit

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    Public Sub New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      m_SuppressUIEvents = True
      InitializeComponent()
      m_SuppressUIEvents = False

      ' Active symbol.
      m_CheckEditActive = CType(gridSalaryData.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
      m_CheckEditActive.PictureChecked = My.Resources.Checked
      m_CheckEditActive.PictureUnchecked = Nothing
      m_CheckEditActive.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the selected ES salary data.
    ''' </summary>
    ''' <returns>The selected ES salary data or nothing if none is selected.</returns>
    Public ReadOnly Property SelectedESSalaryData As ESSalaryData
      Get
        Dim grdView = TryCast(gridSalaryData.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

        If Not (grdView Is Nothing) Then

          Dim selectedRows = grdView.GetSelectedRows()

          If (selectedRows.Count > 0) Then
            Dim esSalary = CType(grdView.GetRow(selectedRows(0)), ESSalaryData)
            Return esSalary
          End If

        End If

        Return Nothing
      End Get

    End Property

    ''' <summary>
    ''' Gets the first ES salary in the list of ES salary data.
    ''' </summary>
    ''' <returns>First ES salary in list or nothing.</returns>
    Public ReadOnly Property FirstESSalaryInList As ESSalaryData
      Get
        If gvSalaryData.RowCount > 0 Then

          Dim rowHandle = gvSalaryData.GetVisibleRowHandle(0)
          Return CType(gvSalaryData.GetRow(rowHandle), ESSalaryData)
        Else
          Return Nothing
        End If

      End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Loads data.
    ''' </summary>
    ''' <param name="esData">The es data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Overrides Function LoadData(ByVal esData As ESMasterData) As Boolean

      Dim success As Boolean = True
      success = success AndAlso LoadESSalaryData(esData.ESNR)

      Dim firstESSalaryData = FirstESSalaryInList

      If Not firstESSalaryData Is Nothing Then
        PresentESSalaryDetailData(firstESSalaryData)
      End If

      m_CurrentESNr = If(success, esData.ESNR, Nothing)

      m_SuppressUIEvents = False

      Return success
    End Function

    ''' <summary>
    ''' Inits the control with configuration information.
    ''' </summary>
    '''<param name="initializationClass">The initialization class.</param>
    '''<param name="translationHelper">The translation helper.</param>
    Public Overrides Sub InitWithConfigurationData(ByVal initializationClass As SP.Infrastructure.Initialization.InitializeClass, ByVal translationHelper As SP.Infrastructure.Initialization.TranslateValuesHelper)
      MyBase.InitWithConfigurationData(initializationClass, translationHelper)

      m_EmployeeDatabaseAccess = New DatabaseAccess.Employee.EmployeeDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)
      m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(m_InitializationData.MDData.MDDbConn, m_InitializationData.UserData.UserLanguage)

    End Sub

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      m_CurrentESNr = Nothing
      m_CurrentESLohnNr = Nothing

      lblGAVVertragValue.Text = String.Empty

      txtGrundlohn.EditValue = 0D

      txtBasisFeiertag.EditValue = 0D
      txtAnsatzFeiertag.EditValue = 0D
      txtBetragFeiertag.EditValue = 0D

      txtBasisFerien.EditValue = 0D
      txtAnsatzFerien.EditValue = 0D
      txtBetragFerien.EditValue = 0D

      txtBasisLohn13.EditValue = 0D
      txtAnsatzLohn13.EditValue = 0D
      txtBetragLohn13.EditValue = 0D

      txtStundenLohn.EditValue = 0D
      txtLohnspesen.EditValue = 0D
      txtTagesspesenMA.EditValue = 0D

      txtTarif.EditValue = 0D
      txtTagesspesenKD.EditValue = 0D
      txtMwStBetrag.EditValue = 0D
      lblKDKostenstelleValue.Text = String.Empty

      dateEditSalaryDataFrom.EditValue = Nothing

      btnActivateESSalaryData.Enabled = True

      '  Reset drop downs and lists

      ResetESSalaryGrid()

    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.lblGAVVertrag.Text = m_Translate.GetSafeTranslationValue(Me.lblGAVVertrag.Text)
      Me.lblGrundlohn.Text = m_Translate.GetSafeTranslationValue(Me.lblGrundlohn.Text)
      Me.lblFeiertag.Text = m_Translate.GetSafeTranslationValue(Me.lblFeiertag.Text)
      Me.lblFerien.Text = m_Translate.GetSafeTranslationValue(Me.lblFerien.Text)
      Me.lblLohn13.Text = m_Translate.GetSafeTranslationValue(Me.lblLohn13.Text)
      Me.lblStundenlohn.Text = m_Translate.GetSafeTranslationValue(Me.lblStundenlohn.Text)
      Me.lblLohnspesen.Text = m_Translate.GetSafeTranslationValue(Me.lblLohnspesen.Text)
      Me.lblTagesspesenMA.Text = m_Translate.GetSafeTranslationValue(Me.lblTagesspesenMA.Text, True)

      Me.lblTarif.Text = m_Translate.GetSafeTranslationValue(Me.lblTarif.Text)
      Me.lblTagesspesenKD.Text = m_Translate.GetSafeTranslationValue(Me.lblTagesspesenKD.Text, True)
      Me.lblMwStBetrag.Text = m_Translate.GetSafeTranslationValue(Me.lblMwStBetrag.Text)
      Me.lblKDKostenstelle.Text = m_Translate.GetSafeTranslationValue(Me.lblKDKostenstelle.Text, True)

      Me.lblLohndatenVon.Text = m_Translate.GetSafeTranslationValue(Me.lblLohndatenVon.Text)
      Me.btnNewESLohn.Text = m_Translate.GetSafeTranslationValue(Me.btnNewESLohn.Text)
      Me.btnActivateESSalaryData.Text = m_Translate.GetSafeTranslationValue(Me.btnActivateESSalaryData.Text)

    End Sub

    ''' <summary>
    ''' Resets the ES salary grid.
    ''' </summary>
    Private Sub ResetESSalaryGrid()

      ' Reset the grid
      gvSalaryData.OptionsView.ShowIndicator = False
      gvSalaryData.OptionsView.ColumnAutoWidth = False

      gvSalaryData.Columns.Clear()

      Dim activeColumn As New DevExpress.XtraGrid.Columns.GridColumn()
      activeColumn.Caption = m_Translate.GetSafeTranslationValue("Aktiv")
      activeColumn.Name = "AktivLODaten"
      activeColumn.FieldName = "AktivLODaten"
      activeColumn.Visible = True
      activeColumn.ColumnEdit = m_CheckEditActive
      gvSalaryData.Columns.Add(activeColumn)

      Dim columnfarpflicht As New DevExpress.XtraGrid.Columns.GridColumn()
      columnfarpflicht.Caption = m_Translate.GetSafeTranslationValue("FAR")
      columnfarpflicht.Name = "FARPflichtig"
      columnfarpflicht.FieldName = "FARPflichtig"
      columnfarpflicht.Visible = True
      columnfarpflicht.ColumnEdit = m_CheckEditActive
      gvSalaryData.Columns.Add(columnfarpflicht)

      Dim columnESLohnNr As New DevExpress.XtraGrid.Columns.GridColumn()
      columnESLohnNr.Caption = m_Translate.GetSafeTranslationValue("Lohnnr")
      columnESLohnNr.Name = "ESLohnNr"
      columnESLohnNr.FieldName = "ESLohnNr"
      columnESLohnNr.Visible = False
      gvSalaryData.Columns.Add(columnESLohnNr)

      Dim columnGrundLohn As New DevExpress.XtraGrid.Columns.GridColumn()
      columnGrundLohn.Caption = m_Translate.GetSafeTranslationValue("Grundlohn")
      columnGrundLohn.Name = "GrundLohn"
      columnGrundLohn.FieldName = "GrundLohn"
      columnGrundLohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      columnGrundLohn.AppearanceHeader.Options.UseTextOptions = True
      columnGrundLohn.Visible = True
      columnGrundLohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
      columnGrundLohn.DisplayFormat.FormatString = "N2"
      gvSalaryData.Columns.Add(columnGrundLohn)

      Dim columnStundenLohn As New DevExpress.XtraGrid.Columns.GridColumn()
      columnStundenLohn.Caption = m_Translate.GetSafeTranslationValue("Stundenlohn")
      columnStundenLohn.Name = "StundenLohn"
      columnStundenLohn.FieldName = "StundenLohn"
      columnStundenLohn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      columnStundenLohn.AppearanceHeader.Options.UseTextOptions = True
      columnStundenLohn.Visible = False
      columnStundenLohn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
      columnStundenLohn.DisplayFormat.FormatString = "N2"
      gvSalaryData.Columns.Add(columnStundenLohn)

      Dim columnTarif As New DevExpress.XtraGrid.Columns.GridColumn()
      columnTarif.Caption = m_Translate.GetSafeTranslationValue("Tarif")
      columnTarif.Name = "Tarif"
      columnTarif.FieldName = "Tarif"
      columnTarif.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      columnTarif.AppearanceHeader.Options.UseTextOptions = True
      columnTarif.Visible = True
      columnTarif.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
      columnTarif.DisplayFormat.FormatString = "N2"
      gvSalaryData.Columns.Add(columnTarif)

      Dim columnKSTBez As New DevExpress.XtraGrid.Columns.GridColumn()
      columnKSTBez.Caption = m_Translate.GetSafeTranslationValue("KSTBez")
      columnKSTBez.Name = "KSTBez"
      columnKSTBez.FieldName = "KSTBez"
      columnKSTBez.Visible = False
      gvSalaryData.Columns.Add(columnKSTBez)

      Dim columnGAVText As New DevExpress.XtraGrid.Columns.GridColumn()
      columnGAVText.Caption = m_Translate.GetSafeTranslationValue("GavText")
      columnGAVText.Name = "GavText"
      columnGAVText.FieldName = "GavText"
      columnGAVText.Visible = False
      gvSalaryData.Columns.Add(columnGAVText)

      Dim columnErfasst As New DevExpress.XtraGrid.Columns.GridColumn()
      columnErfasst.Caption = m_Translate.GetSafeTranslationValue("Erfasst")
      columnErfasst.Name = "Erfasst"
      columnErfasst.FieldName = "Erfasst"
      columnErfasst.Visible = False
      gvSalaryData.Columns.Add(columnErfasst)

      Dim columnLOVon As New DevExpress.XtraGrid.Columns.GridColumn()
      columnLOVon.Caption = m_Translate.GetSafeTranslationValue("Lohn Ab")
      columnLOVon.Name = "LOVon"
      columnLOVon.FieldName = "LOVon"
      columnLOVon.Visible = True
      gvSalaryData.Columns.Add(columnLOVon)

      Dim columnMargeOhneBVG As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMargeOhneBVG.Caption = m_Translate.GetSafeTranslationValue("Marge ohne BVG")
      columnMargeOhneBVG.Name = "BruttoMarge"
      columnMargeOhneBVG.FieldName = "BruttoMarge"
      columnMargeOhneBVG.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      columnMargeOhneBVG.AppearanceHeader.Options.UseTextOptions = True
      columnMargeOhneBVG.Visible = True
      columnMargeOhneBVG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
      columnMargeOhneBVG.DisplayFormat.FormatString = "N2"
      gvSalaryData.Columns.Add(columnMargeOhneBVG)

      Dim columnMargeMitBVG As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMargeMitBVG.Caption = m_Translate.GetSafeTranslationValue("Marge mit BVG")
      columnMargeMitBVG.Name = "MargeMitBVG"
      columnMargeMitBVG.FieldName = "MargeMitBVG"
      columnMargeMitBVG.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      columnMargeMitBVG.AppearanceHeader.Options.UseTextOptions = True
      columnMargeMitBVG.Visible = True
      columnMargeMitBVG.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
      columnMargeMitBVG.DisplayFormat.FormatString = "N2"
      gvSalaryData.Columns.Add(columnMargeMitBVG)

      Dim columnMargeOhneBVGInPorzent As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMargeOhneBVGInPorzent.Caption = m_Translate.GetSafeTranslationValue("Marge ohne BVG %")
      columnMargeOhneBVGInPorzent.Name = "MargeOhneBVGInProzent"
      columnMargeOhneBVGInPorzent.FieldName = "MargeOhneBVGInProzent"
      columnMargeOhneBVGInPorzent.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      columnMargeOhneBVGInPorzent.AppearanceHeader.Options.UseTextOptions = True
      columnMargeOhneBVGInPorzent.Visible = True
      columnMargeOhneBVGInPorzent.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
      columnMargeOhneBVGInPorzent.DisplayFormat.FormatString = "N2"
      gvSalaryData.Columns.Add(columnMargeOhneBVGInPorzent)

      Dim columnMargeMitBVGInProzent As New DevExpress.XtraGrid.Columns.GridColumn()
      columnMargeMitBVGInProzent.Caption = m_Translate.GetSafeTranslationValue("Marge mit BVG %")
      columnMargeMitBVGInProzent.Name = "MargeMitBVGInProzent"
      columnMargeMitBVGInProzent.FieldName = "MargeMitBVGInProzent"
      columnMargeMitBVGInProzent.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
      columnMargeMitBVGInProzent.AppearanceHeader.Options.UseTextOptions = True
      columnMargeMitBVGInProzent.Visible = True
      columnMargeMitBVGInProzent.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
      columnMargeMitBVGInProzent.DisplayFormat.FormatString = "N2"
      gvSalaryData.Columns.Add(columnMargeMitBVGInProzent)

      gridSalaryData.DataSource = Nothing

    End Sub

    ''' <summary>
    ''' Loads ES salary data.
    ''' </summary>
    ''' <param name="esNr">The ES number.</param>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadESSalaryData(ByVal esNr As Integer) As Boolean

      Dim esSalaryDataList = m_ESDataAccess.LoadESSalaryData(esNr)

      If esSalaryDataList Is Nothing Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohndaten konnten nicht geladen werden."))
        Return False
      End If

      m_ESSalayDataList = esSalaryDataList

      Dim suppressUIEventsState = m_SuppressUIEvents
      m_SuppressUIEvents = True
      gridSalaryData.DataSource = esSalaryDataList
      m_SuppressUIEvents = suppressUIEventsState

      Return True
    End Function

    ''' <summary>
    ''' Present es salary detail data.
    ''' </summary>
    ''' <param name="esSalaryData">The ES salary data.</param>
    Private Sub PresentESSalaryDetailData(ByVal esSalaryData As ESSalaryData)

      m_CurrentESLohnNr = esSalaryData.ESLohnNr

      lblGAVVertragValue.Text = esSalaryData.GavText
      lblKDKostenstelleValue.Text = esSalaryData.KSTBez

      txtGrundlohn.EditValue = esSalaryData.GrundLohn

      ' Feiertag
      txtBasisFeiertag.EditValue = esSalaryData.FeierBasis
      txtAnsatzFeiertag.EditValue = esSalaryData.FeierProz
      txtBetragFeiertag.EditValue = esSalaryData.Feier

      ' Ferien
      txtBasisFerien.EditValue = esSalaryData.FerBasis
      txtAnsatzFerien.EditValue = esSalaryData.FerienProz
      txtBetragFerien.EditValue = esSalaryData.Ferien

      ' 13.Lohn
      txtBasisLohn13.EditValue = esSalaryData.Basis13
      txtAnsatzLohn13.EditValue = esSalaryData.Lohn13Proz
      txtBetragLohn13.EditValue = esSalaryData.Lohn13

      ' Stundenenlohn / Stunde
      txtStundenLohn.EditValue = esSalaryData.StundenLohn
      txtLohnspesen.EditValue = esSalaryData.MAStdSpesen
      txtTagesspesenMA.EditValue = esSalaryData.MATSpesen

      txtTarif.EditValue = esSalaryData.Tarif
      txtTagesspesenKD.EditValue = esSalaryData.KDTSpesen
      txtMwStBetrag.EditValue = esSalaryData.MWSTBetrag

      Dim suppressState = m_SuppressUIEvents
      m_SuppressUIEvents = True
      dateEditSalaryDataFrom.EditValue = esSalaryData.LOVon
      m_SuppressUIEvents = suppressState

      btnActivateESSalaryData.Enabled = Not (esSalaryData.AktivLODaten.HasValue AndAlso esSalaryData.AktivLODaten.Value)

      m_UCMediator.LoadGAVDetailData(esSalaryData)
    End Sub

    ''' <summary>
    ''' Handles click on new ESLohn button.
    ''' </summary>
    Private Sub OnBtnNewESLohn_Click(sender As System.Object, e As System.EventArgs) Handles btnNewESLohn.Click

			If m_InitializationData.MDData.ClosedMD = 1 Then Return
			' Load ES data from database.
			Dim esMasterDataFromDB = m_ESDataAccess.LoadESMasterData(m_CurrentESNr)

      If esMasterDataFromDB Is Nothing Then
        m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Einsatzdaten konnten nicht geladen werden"))
        Return
      End If

      ' Load mandant, employee, customer and ZHD data.
      Dim mandantData = m_CommonDatabaseAccess.LoadMandantData(esMasterDataFromDB.MDNr)
      Dim employeeMasterData = m_EmployeeDatabaseAccess.LoadEmployeeMasterData(esMasterDataFromDB.EmployeeNumber, False)
      Dim customerMasterData = m_CustomerDatabaseAccess.LoadCustomerMasterData(esMasterDataFromDB.CustomerNumber, m_InitializationData.UserData.UserFiliale)
      Dim responsiblePersonMasterData = Nothing

      If esMasterDataFromDB.KDZHDNr.HasValue Then
        responsiblePersonMasterData = m_CustomerDatabaseAccess.LoadResponsiblePersonMasterData(esMasterDataFromDB.CustomerNumber, esMasterDataFromDB.KDZHDNr)
      End If

      Dim selectedCandidateAndCustomerData As New InitCandidateAndCustomerData With
        {.MandantData = mandantData,
         .EmployeeData = employeeMasterData,
         .CustomerData = customerMasterData,
         .ResponsiblePersondata = responsiblePersonMasterData
          }

      Dim selectedESData As New InitESData With {
        .ESAls = esMasterDataFromDB.ES_Als,
        .ESStartDate = esMasterDataFromDB.ES_Ab,
        .ESEndDate = esMasterDataFromDB.ES_Ende,
        .Uhrzeit = esMasterDataFromDB.ES_Uhr,
        .SUVA = esMasterDataFromDB.SUVA,
        .Kst1 = esMasterDataFromDB.ESKST1,
        .Kst2 = esMasterDataFromDB.ESKST2,
        .MA_KD_Berater = esMasterDataFromDB.ESKst,
        .Unterzeichner = esMasterDataFromDB.ESUnterzeichner,
        .ESEinstufung = esMasterDataFromDB.Einstufung,
        .Branche = esMasterDataFromDB.ESBranche
       }

      Dim frmNewESLohn As New frmNewESLohn(m_InitializationData, esMasterDataFromDB.ESNR, selectedCandidateAndCustomerData, selectedESData)
      Dim dialogResult = frmNewESLohn.ShowDialog()

      If dialogResult = DialogResult.OK Then

        Dim newlyCreatedESNumber As Integer = frmNewESLohn.NewESLohnNr
        Dim successReloadSalaryList = LoadESSalaryData(esMasterDataFromDB.ESNR)

        If successReloadSalaryList Then
          FocusESSalaryData(newlyCreatedESNumber)
          Dim esSalaryData = GetESSalaryDataByNumber(newlyCreatedESNumber)
          PresentESSalaryDetailData(esSalaryData)
        End If

        m_UCMediator.SendESDataChangedNotification(m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CurrentESNr)
        m_UCMediator.RefreshAdditionalSalaryData()
      End If

    End Sub

    ''' <summary>
    ''' Handles key down on salary data grid.
    ''' </summary>
    Private Sub OnGridSalaryData_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs) Handles gridSalaryData.KeyDown
      If (Not m_CurrentESNr.HasValue Or Not m_CurrentESLohnNr.HasValue) Then
        Return
      End If

      If (e.KeyCode = Keys.Delete) Then

        Dim grdView = TryCast(sender.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

        If Not (grdView Is Nothing) Then

          Dim selectedRows = grdView.GetSelectedRows()

          If (selectedRows.Count > 0) Then
            Dim esSalaryData = CType(grdView.GetRow(selectedRows(0)), ESSalaryData)

            If (m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Wollen Sie den Datensatz wirklich löschen?"),
                                            m_Translate.GetSafeTranslationValue("Datensatz löschen")) = False) Then
              Return
            End If

            Dim result = m_ESDataAccess.DeleteESSalaryData(esSalaryData.ID, ConstantValues.ModulName, m_ClsProgSetting.GetUserName(), m_InitializationData.UserData.UserNr)

            Select Case result
              Case DeleteESSalaryResult.ResultCanNotDeleteBecauseOfRPL
                m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Lohndaten können nich gelöscht werden, da sie bereits mit einem Rapport verbunden sind."))
              Case DeleteESSalaryResult.ResultCanNotDeleteBecauseOfAdditionalSalaryType
                m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Die Lohndaten können nich gelöscht werden, da zusätzliche Lohndaten vorhanden sind."))
              Case DeleteESSalaryResult.ResultOnlyOneRecordLeft

                m_UtilityUI.ShowOKDialog(m_Translate.GetSafeTranslationValue("Es existiert nur ein Datensatz. Bitte erfassen Sie einen neuen Datensatz und dann löschen Sie die gewünschten Daten."),
                                                                 m_Translate.GetSafeTranslationValue("Daten löschen"), MessageBoxIcon.Exclamation)

              Case DeleteESSalaryResult.ResultDeleteError
                m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Lohndaten konnte nicht gelöscht werden."))

              Case DeleteESSalaryResult.ResultDeleteOk
                LoadESSalaryData(m_CurrentESNr)

                Dim firstESSalaryData = FirstESSalaryInList

                If Not firstESSalaryData Is Nothing Then
                  PresentESSalaryDetailData(firstESSalaryData)
                End If

                m_UCMediator.SendESDataChangedNotification(m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CurrentESNr)
                m_UCMediator.RefreshAdditionalSalaryData()
            End Select

          End If
        End If
      End If

    End Sub

    ''' <summary>
    ''' Handles click on activate ES salary data button.
    ''' </summary>
    Private Sub OnBtnActivateESSalaryData_Click(sender As System.Object, e As System.EventArgs) Handles btnActivateESSalaryData.Click
			If m_InitializationData.MDData.ClosedMD = 1 Then Return

			If (m_CurrentESNr.HasValue And m_CurrentESLohnNr.HasValue) Then
				Dim success = m_ESDataAccess.ActivateESSalaryData(m_CurrentESNr, m_CurrentESLohnNr)

				If Not success Then
					m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohndaten konnten nicht aktiviert werden."))
					Return
				End If

				LoadESSalaryData(m_CurrentESNr)
				FocusESSalaryData(m_CurrentESLohnNr)

				m_UCMediator.SendESDataChangedNotification(m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CurrentESNr)
				m_UCMediator.RefreshAdditionalSalaryData()

			End If

		End Sub

    ''' <summary>
    ''' Handles focus change of ES salary row.
    ''' </summary>
		Private Sub OnESSalary_FocusedRowChanged(sender As System.Object, e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles gvSalaryData.FocusedRowChanged

			If m_SuppressUIEvents Then
				Return
			End If

			Dim selectedESSalary = SelectedESSalaryData

			If Not selectedESSalary Is Nothing Then
				PresentESSalaryDetailData(selectedESSalary)
				m_UCMediator.RefreshAdditionalSalaryData()
				m_UCMediator.CheckGAVValidityOfESSalaryData(selectedESSalary)
			End If

		End Sub

		''' <summary>
		''' Handles Click on ES salary row (cell).
		''' </summary>
		Private Sub OnESSalary_RowClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowClickEventArgs) Handles gvSalaryData.RowClick

			If m_SuppressUIEvents Then
				Return
			End If

			Dim selectedESSalary = SelectedESSalaryData

			If Not selectedESSalary Is Nothing Then
				PresentESSalaryDetailData(selectedESSalary)
				m_UCMediator.RefreshAdditionalSalaryData()
				m_UCMediator.CheckGAVValidityOfESSalaryData(selectedESSalary)
			End If

		End Sub

		''' <summary>
		''' Handles click of show customer data checkbox.
		''' </summary>
		Private Sub OncmdVisibleCustomer_click(sender As System.Object, e As System.EventArgs) Handles cmdVisibleCustomer.Click
			pnlKD.Visible = Not pnlKD.Visible
		End Sub

		''' <summary>
		''' Handles button click on dateEditSalaryDataFrom.
		''' </summary>
		Private Sub OnDateEditSalaryDataFrom_ButtonClick(sender As System.Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles dateEditSalaryDataFrom.ButtonClick

			If e.Button.Index = 1 Then

				' when we change manuelly value, editvalue does not firing!!!
				If dateEditSalaryDataFrom.EditValue <> dateEditSalaryDataFrom.DateTime Then dateEditSalaryDataFrom.EditValue = dateEditSalaryDataFrom.DateTime
				If m_CurrentESLohnNr.HasValue AndAlso m_CurrentESNr.HasValue AndAlso DateTime.TryParse(dateEditSalaryDataFrom.EditValue, Nothing) Then

					Dim esSalaryData = GetESSalaryDataByNumber(m_CurrentESLohnNr)
					If esSalaryData.LOVon = dateEditSalaryDataFrom.EditValue Then Return


					Dim success = m_ESDataAccess.CheckIfLOVonDateCanBeSet(m_CurrentESNr, m_CurrentESLohnNr, dateEditSalaryDataFrom.EditValue)
					If Not success.GetValueOrDefault(False) Then
						Dim result = m_UtilityUI.ShowYesNoDialog(m_Translate.GetSafeTranslationValue("Achtung, für diese Zeitperiode bestehen bereits erfasste Rapporte?"),
																										 m_Translate.GetSafeTranslationValue("Einsatzlohn erfassen"), MessageBoxDefaultButton.Button2)
						If result = False Then
							dateEditSalaryDataFrom.EditValue = esSalaryData.LOVon

							Return
						Else
							success = True
						End If

					End If




					success = success AndAlso m_ESDataAccess.UpdateESSalrayDataForESMng(m_CurrentESNr, m_CurrentESLohnNr, dateEditSalaryDataFrom.EditValue)

					If Not success Then
						m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Das Datum konnte nicht aktualisiert werden."))
					Else
						LoadESSalaryData(m_CurrentESNr)
						FocusESSalaryData(m_CurrentESLohnNr)
						m_UCMediator.SendESDataChangedNotification(m_InitializationData.UserData.UserNr, m_InitializationData.MDData.MDNr, m_CurrentESNr)
						m_UCMediator.RefreshAdditionalSalaryData()
					End If

				End If

			End If

		End Sub

		''' <summary>
		''' Focuses a ES salary data.
		''' </summary>
		''' <param name="esLohnNr">The ESLohnNr.</param>
		Private Sub FocusESSalaryData(ByVal esLohnNr As Integer)

			If Not gridSalaryData.DataSource Is Nothing Then

				Dim esSalaryData = CType(gvSalaryData.DataSource, List(Of ESSalaryData))

				Dim index = esSalaryData.ToList().FindIndex(Function(data) data.ESLohnNr = esLohnNr)

				Dim suppressState = m_SuppressUIEvents
				m_SuppressUIEvents = True
				Dim rowHandle = gvSalaryData.GetRowHandle(index)
				gvSalaryData.FocusedRowHandle = rowHandle
				m_SuppressUIEvents = suppressState
			End If

		End Sub

		''' <summary>
		''' Gets ES salary my number.
		''' </summary>
		''' <param name="esLohnNumber">The ES Lohn number.</param>
		''' <returns>The monthly salary view data or nothing.</returns>
		Private Function GetESSalaryDataByNumber(ByVal esLohnNumber As Integer) As ESSalaryData

			If gvSalaryData.DataSource Is Nothing Then
				Return Nothing
			End If

			Dim esSalaryDataList = CType(gvSalaryData.DataSource, List(Of ESSalaryData))

			Dim salaryDta = esSalaryDataList.Where(Function(data) data.ESLohnNr = esLohnNumber).FirstOrDefault

			Return salaryDta
		End Function

		'Private Sub cmdExtraLANr_Click(sender As System.Object, e As System.EventArgs) Handles cmdExtraLANr.Click
		'	Dim oMyProg As Object


		'	Dim esMasterDataFromDB = m_ESDataAccess.LoadESMasterData(m_CurrentESNr)
		'	If esMasterDataFromDB Is Nothing Then
		'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Einsatzdaten konnten nicht gefunden werden."))
		'	End If
		'	Try
		'		oMyProg = CreateObject("ESUtility.ClsMain")
		'		oMyProg.OpenESZLohnForm(m_CurrentESNr, m_CurrentESLohnNr, esMasterDataFromDB.EmployeeNumber, esMasterDataFromDB.CustomerNumber)

		'	Catch ex As Exception
		'		m_Logger.LogError(ex.ToString)
		'		m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Fehler zum Anzeigen der Zusatzlohndaten."))

		'	End Try

		'End Sub

#End Region

	End Class

End Namespace

