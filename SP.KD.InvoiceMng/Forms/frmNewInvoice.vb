Imports SPProgUtility.ProgPath
Imports SPProgUtility.Mandanten
Imports SPProgUtility.CommonSettings
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure
Imports SP.Infrastructure.UI
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Invoice
Imports SP.DatabaseAccess.Customer
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages

Namespace UI

  Public Class frmNewInvoice

#Region "Private Fields"

    ''' <summary>
    ''' The Initialization data.
    ''' </summary>
    Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

    ''' <summary>
    ''' The translation value helper.
    ''' </summary>
    Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

    ''' <summary>
    ''' The common database access.
    ''' </summary>
    Protected m_CommonDatabaseAccess As ICommonDatabaseAccess

    ''' <summary>
    ''' The invoice data access object.
    ''' </summary>
    Private m_InvoiceDatabaseAccess As IInvoiceDatabaseAccess

    ''' <summary>
    ''' The customer database access.
    ''' </summary>
    Private m_CustomerDatabaseAccess As ICustomerDatabaseAccess

    ''' <summary>
    ''' UI Utility functions.
    ''' </summary>
    Private m_UtilityUI As UtilityUI

    ''' <summary>
    ''' Utility functions.
    ''' </summary>
    Private m_Utility As Utility

    ''' <summary>
    ''' The logger.
    ''' </summary>
    Private Shared m_Logger As ILogger = New Logger()

    ''' <summary>
    ''' Boolean flag indicating if initial data has been loaded.
    ''' </summary>
    Private m_IsInitialDataLoaded As Boolean = False

    ''' <summary>
    ''' List of tab controls.
    ''' </summary>
    Private m_ListOfPageControls As New List(Of ucWizardPageBaseControl)

    ''' <summary>
    ''' Boolean value that allows to suppress UI events while manipulating controls programmatically.
    ''' </summary>
    Private m_SuppressUIEvents As Boolean = False

    ''' <summary>
    ''' Communication support between controls.
    ''' </summary>
    Protected m_UCMediator As NewInvoiceUserControlFormMediator

    ''' <summary>
    ''' The common settings.
    ''' </summary>
    Private m_Common As CommonSetting

    ''' <summary>
    ''' The SPProgUtility object.
    ''' </summary>
    Protected m_ClsProgSetting As New SPProgUtility.ClsProgSettingPath

    ''' <summary>
    ''' The mandant.
    ''' </summary>
    ''' <remarks></remarks>
    Private m_Mandant As Mandant

    Private m_path As ClsProgPath

    ''' <summary>
    ''' The current connection string.
    ''' </summary>
    Private m_CurrentConnectionString = String.Empty

    ''' <summary>
    ''' The preselection data.
    ''' </summary>
    Private m_PreselectionData As PreselectionData

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="_setting">The settings.</param>
    ''' <param name="preselectionData">The preselection data.</param>
    Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass, ByVal preselectionData As PreselectionData)

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
      Try
        ' Mandantendaten
        m_Mandant = New Mandant
        m_path = New ClsProgPath
        m_Common = New CommonSetting

        m_InitializationData = _setting
        m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
        m_PreselectionData = preselectionData
      Catch ex As Exception
        m_Logger.LogError(ex.ToString)

      End Try

      ' Dieser Aufruf ist für den Designer erforderlich.
      DevExpress.UserSkins.BonusSkins.Register()
      DevExpress.Skins.SkinManager.EnableFormSkins()

      m_SuppressUIEvents = True
      InitializeComponent()
      m_SuppressUIEvents = False

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

      m_ListOfPageControls.Add(ucPageSelectMandant)
      m_ListOfPageControls.Add(ucPageSelectInvoiceData)
      m_ListOfPageControls.Add(ucPageCreateInvoice)

      ' Init sub controls with configuration information
      For Each ctrl In m_ListOfPageControls
        ctrl.InitWithConfigurationData(m_InitializationData, m_Translate)
        ctrl.PreselectionData = preselectionData
      Next

      ' Create the user control mediator
      m_UCMediator = New NewInvoiceUserControlFormMediator(Me,
                                                ucPageSelectMandant,
                                                ucPageSelectInvoiceData,
                                                ucPageCreateInvoice,
                                                m_Translate)

      ChangeMandant(m_InitializationData.MDData.MDNr)

      m_UtilityUI = New UtilityUI
      m_Utility = New Utility

      ' Translate controls.
      TranslateControls()

      Reset()

      ucPageSelectMandant.ActivatePage()

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the common db access object.
    ''' </summary>
    Public ReadOnly Property CommonDbAccess As ICommonDatabaseAccess
      Get
        Return m_CommonDatabaseAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the invoice db access object.
    ''' </summary>
    Public ReadOnly Property InvoiceDbAccess As IInvoiceDatabaseAccess
      Get
        Return m_InvoiceDatabaseAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the customer db access object.
    ''' </summary>
    Public ReadOnly Property CustomerDbAccess As ICustomerDatabaseAccess
      Get
        Return m_CustomerDatabaseAccess
      End Get
    End Property

    ''' <summary>
    ''' Gets the selected manant number.
    ''' </summary>
    ''' <returns>The selected MDNr.</returns>
    Public ReadOnly Property SelectedMDNr As Integer?
      Get
        Dim mandantData = ucPageSelectMandant.SelecteData.MandantData

        If mandantData Is Nothing Then
          Return Nothing
        Else
          Return mandantData.MandantNumber
        End If
      End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Changes the mandant nr.
    ''' </summary>
    ''' <param name="mdNr">The mandant number.</param>
    Public Sub ChangeMandant(ByVal mdNr As Integer)

      Dim conStr = m_Mandant.GetSelectedMDData(mdNr).MDDbConn

      If Not m_CurrentConnectionString = conStr Then

        m_CurrentConnectionString = conStr

        m_CommonDatabaseAccess = New DatabaseAccess.Common.CommonDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
        m_InvoiceDatabaseAccess = New DatabaseAccess.Invoice.InvoiceDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
        m_CustomerDatabaseAccess = New DatabaseAccess.Customer.CustomerDatabaseAccess(conStr, m_InitializationData.UserData.UserLanguage)
      End If

    End Sub

    ''' <summary>
    ''' Validates all data.
    ''' </summary>
    Public Function ValidateData() As Boolean

      Dim valid As Boolean = True

      For Each ctrl In m_ListOfPageControls
        valid = valid AndAlso ctrl.ValidateData()
      Next

      Return valid
    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Translates the controls
    ''' </summary>
    Private Sub TranslateControls()

      Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			wizardCtrl.Text = m_Translate.GetSafeTranslationValue(wizardCtrl.Text)
			wizardCtrl.NextText = String.Format("{0} >", m_Translate.GetSafeTranslationValue("Weiter"))
			wizardCtrl.PreviousText = String.Format("< {0}", m_Translate.GetSafeTranslationValue("Zurück"))
			wizardCtrl.CancelText = m_Translate.GetSafeTranslationValue("Abbrechen")
			wizardCtrl.FinishText = m_Translate.GetSafeTranslationValue("Rechnung erstellen")

			pageMandantData.Text = String.Format("1) {0}", m_Translate.GetSafeTranslationValue("Mandant, Kostenstellen und BeraterIn eingeben"))
      pageMandantData.DescriptionText = m_Translate.GetSafeTranslationValue("Bitte wählen Sie den Mandant, die Kostenstellen sowie den Berater/In")

			pageInvoiceData.Text = String.Format("2) {0}", m_Translate.GetSafeTranslationValue("Rechnungsdaten eingeben"))
      pageInvoiceData.DescriptionText = m_Translate.GetSafeTranslationValue("Bitte geben Sie die Rechnungsdaten ein")

      pageCreateInvoice.Text = m_Translate.GetSafeTranslationValue("Zusammenfassung")
      pageCreateInvoice.DescriptionText = m_Translate.GetSafeTranslationValue("Prüfen Sie nochmals die Daten und erstellen Sie anschliessend die Rechnung")

    End Sub

    ''' <summary>
    ''' Handles page changing event of wizard.
    ''' </summary>
    Private Sub OnSelectedPageChanging(sender As System.Object, e As DevExpress.XtraWizard.WizardPageChangingEventArgs) Handles wizardCtrl.SelectedPageChanging

      If e.Direction = DevExpress.XtraWizard.Direction.Forward Then

        Dim allowForward As Boolean = True

        If e.PrevPage.Name = pageMandantData.Name Then
          allowForward = ucPageSelectMandant.ValidateData()
        ElseIf e.PrevPage.Name = pageInvoiceData.Name Then
          allowForward = ucPageSelectInvoiceData.ValidateData()
        ElseIf e.PrevPage.Name = pageCreateInvoice.Name Then
          allowForward = ucPageCreateInvoice.ValidateData()
        End If

        If Not allowForward Then
          e.Cancel = True
          Return
        End If

      End If

      If e.Page.Name = pageMandantData.Name Then
        ucPageSelectMandant.ActivatePage()
      ElseIf e.Page.Name = pageInvoiceData.Name Then
        ucPageSelectInvoiceData.ActivatePage()
      ElseIf e.Page.Name = pageCreateInvoice.Name Then
        ucPageCreateInvoice.ActivatePage()
      End If
    End Sub

    ''' <summary>
    ''' Handles click on finish button.
    ''' </summary>
    Private Sub OnWizardCtrl_FinishClick(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles wizardCtrl.FinishClick

      If (m_UCMediator.HandleFinishClick()) Then

        If Not ucPageCreateInvoice.ReNrOfNewlyCreatedInvoice.HasValue Then
          m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnung konnte nicht angelegt werden."))
          DialogResult = DialogResult.Cancel
        Else
          DialogResult = DialogResult.OK

          ' Send a request to open a einsatzMng form.
          Dim hub = MessageService.Instance.Hub
          Dim openCustomerMng As New OpenInvoiceMngRequest(Me, m_InitializationData.UserData.UserNr, SelectedMDNr, ucPageCreateInvoice.ReNrOfNewlyCreatedInvoice)
          hub.Publish(openCustomerMng)

        End If

        Close()

      End If

    End Sub

    ''' <summary>
    ''' Handles click on cancel button.
    ''' </summary>
    Private Sub OnWizardCtrl_CancelClick(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles wizardCtrl.CancelClick
      DialogResult = DialogResult.Cancel
      Close()
    End Sub


    ''' <summary>
    ''' Resets the from.
    ''' </summary>
    Private Sub Reset()

      ' Reset all the child controls
      For Each ctrl In m_ListOfPageControls
        ctrl.Reset()
      Next

    End Sub

#End Region

  End Class

End Namespace
