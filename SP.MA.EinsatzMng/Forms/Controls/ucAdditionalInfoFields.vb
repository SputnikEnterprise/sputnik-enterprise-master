
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls

Namespace UI

  Public Class ucAdditionalInfoFields


#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
      ' Dieser Aufruf ist für den Designer erforderlich.
      InitializeComponent()

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			xtabAdditionalInfoFields.SelectedTabPage = xtabZusatzFelder

			AddHandler dateEditam.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler dateEditauf.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueArt.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler luedismissalwho.ButtonClick, AddressOf OnDropDown_ButtonClick

    End Sub

#End Region


#Region "Public Methods"

    ''' <summary>
    ''' Loads data.
    ''' </summary>
    ''' <param name="esData">The es data.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Overrides Function LoadData(ByVal esData As ESMasterData) As Boolean

      Dim success As Boolean = True

      If Not m_IsIntialControlDataLoaded Then
        success = success AndAlso LoadDropDownData()
        m_IsIntialControlDataLoaded = True
      End If

      memoEsBeurteilung.Text = esData.LeistungsDoc

      txtEinsatzvertrag.Text = esData.Bemerk_MA
      txtVerleihvertrag.Text = esData.Bemerk_KD

      txtAdditionalText1.Text = esData.Bemerk_RE
      txtAdditionalText2.Text = esData.Bemerk_Lo
      txtAdditionalText3.Text = esData.Bemerk_P
      txtAdditionalText4.Text = esData.Bemerk_1
      txtAdditionalText5.Text = esData.Bemerk_2
      txtAdditionalText6.Text = esData.Bemerk_3

      dateEditam.EditValue = esData.dismissalon
      dateEditauf.EditValue = esData.dismissalfor

      lueArt.EditValue = esData.dismissalkind
      txtdismissalreason.EditValue = esData.dismissalreason
      txtdismissalreason.Properties.ShowIcon = Not txtdismissalreason.Text.Length > 0
      luedismissalwho.EditValue = esData.dismissalwho

      Return True

    End Function

    ''' <summary>
    ''' Merges ES master data.
    ''' </summary>
    ''' <param name="esData">The es data.</param>
    Public Overrides Sub MergeESMasterData(ByVal esData As ESMasterData)

      esData.LeistungsDoc = memoEsBeurteilung.Text
      esData.Bemerk_MA = txtEinsatzvertrag.Text
      esData.Bemerk_KD = txtVerleihvertrag.Text

      esData.Bemerk_RE = txtAdditionalText1.Text
      esData.Bemerk_Lo = txtAdditionalText2.Text
      esData.Bemerk_P = txtAdditionalText3.Text
      esData.Bemerk_1 = txtAdditionalText4.Text
      esData.Bemerk_2 = txtAdditionalText5.Text
      esData.Bemerk_3 = txtAdditionalText6.Text

      esData.dismissalon = dateEditam.EditValue
      esData.dismissalfor = dateEditauf.EditValue

      esData.dismissalkind = lueArt.EditValue
      esData.dismissalreason = txtdismissalreason.EditValue
      esData.dismissalwho = luedismissalwho.EditValue

    End Sub

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      memoEsBeurteilung.Text = String.Empty

			txtEinsatzvertrag.Text = String.Empty

			txtAdditionalText1.Text = String.Empty

			txtAdditionalText2.Text = String.Empty

			txtAdditionalText3.Text = String.Empty

			txtAdditionalText4.Text = String.Empty

			txtAdditionalText5.Text = String.Empty

			txtAdditionalText6.Text = String.Empty

			dateEditam.EditValue = Nothing
      dateEditauf.EditValue = Nothing

      lueArt.EditValue = Nothing
      txtdismissalreason.Text = String.Empty

			luedismissalwho.Text = String.Empty

			ResetDismissalArtDropDown()
      ResetDismissalWhoDropDown()

    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()

      Me.lblESBeurteilung.Text = m_Translate.GetSafeTranslationValue(Me.lblESBeurteilung.Text, True)
      Me.lblEinsatzvertrag.Text = m_Translate.GetSafeTranslationValue(Me.lblEinsatzvertrag.Text, True)
      Me.lblVerleihvertrag.Text = m_Translate.GetSafeTranslationValue(Me.lblVerleihvertrag.Text, True)

      Me.xtabZusatzFelder.Text = m_Translate.GetSafeTranslationValue(Me.xtabZusatzFelder.Text)
      Me.xtabZVARG.Text = m_Translate.GetSafeTranslationValue(Me.xtabZVARG.Text)

      Me.lblZusatz1.Text = m_Translate.GetSafeTranslationValue(Me.lblZusatz1.Text, True)
      Me.lblZusatz2.Text = m_Translate.GetSafeTranslationValue(Me.lblZusatz2.Text, True)
      Me.lblZusatz3.Text = m_Translate.GetSafeTranslationValue(Me.lblZusatz3.Text, True)
      Me.lblZusatz4.Text = m_Translate.GetSafeTranslationValue(Me.lblZusatz4.Text, True)
      Me.lblZusatz5.Text = m_Translate.GetSafeTranslationValue(Me.lblZusatz5.Text, True)
      Me.lblZusatz6.Text = m_Translate.GetSafeTranslationValue(Me.lblZusatz6.Text, True)

      Me.lblErfolgtam.Text = m_Translate.GetSafeTranslationValue(Me.lblErfolgtam.Text, True)
      Me.lblAuf.Text = m_Translate.GetSafeTranslationValue(Me.lblAuf.Text, True)
      Me.lblArtderKuendigung.Text = m_Translate.GetSafeTranslationValue(Me.lblArtderKuendigung.Text, True)
      Me.lblBegruendung.Text = m_Translate.GetSafeTranslationValue(Me.lblBegruendung.Text, True)
      Me.lblKuendungdurch.Text = m_Translate.GetSafeTranslationValue(Me.lblKuendungdurch.Text, True)

    End Sub

    ''' <summary>
    ''' Loads the drop down data.
    ''' </summary>
    ''' <returns>Boolean value indicating success.</returns>
    Private Function LoadDropDownData() As Boolean
      Dim success As Boolean = True

      success = success AndAlso LoadDismissalArtDropDownData()
      success = success AndAlso LoadDismissalWhoDropDownData()

      Return success
    End Function

    ''' <summary>
    ''' Resets the Art of dismissalkind drop down data.
    ''' </summary>
    Private Sub ResetDismissalArtDropDown()

      lueArt.Properties.DisplayMember = "DisplayText"
      lueArt.Properties.ValueMember = "Value"

      Dim columns = lueArt.Properties.Columns
      columns.Clear()

      columns.Add(New LookUpColumnInfo("DisplayText", 0))

      lueArt.Properties.ShowHeader = False
      lueArt.Properties.ShowFooter = False
      lueArt.Properties.DropDownRows = 10
      lueArt.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      lueArt.Properties.SearchMode = SearchMode.AutoComplete
      lueArt.Properties.AutoSearchColumnIndex = 0
      lueArt.Properties.NullText = String.Empty

      Dim suppressUIEventsState = m_SuppressUIEvents
      m_SuppressUIEvents = True
      lueArt.EditValue = Nothing
      m_SuppressUIEvents = suppressUIEventsState

    End Sub

    ''' <summary>
    ''' Loads the dismissalkind drop down data.
    ''' </summary>
    Private Function LoadDismissalArtDropDownData() As Boolean
      Dim dayMonthStdData = New List(Of DismissalKindData) From {
      New DismissalKindData With {.DisplayText = m_Translate.GetSafeTranslationValue("Mündlich"), .Value = "mündlich"},
       New DismissalKindData With {.DisplayText = m_Translate.GetSafeTranslationValue("Schriftlich"), .Value = "schriftlich"}
      }

      lueArt.Properties.DataSource = dayMonthStdData
      lueArt.Properties.ForceInitialize()

      Return True
    End Function

    ''' <summary>
    ''' Resets the Art of dismissalwho drop down data.
    ''' </summary>
    Private Sub ResetDismissalWhoDropDown()

      luedismissalwho.Properties.DisplayMember = "DisplayText"
      luedismissalwho.Properties.ValueMember = "Value"

      Dim columns = luedismissalwho.Properties.Columns
      columns.Clear()

      columns.Add(New LookUpColumnInfo("DisplayText", 0))

      luedismissalwho.Properties.ShowHeader = False
      luedismissalwho.Properties.ShowFooter = False
      luedismissalwho.Properties.DropDownRows = 10
      luedismissalwho.Properties.BestFitMode = BestFitMode.BestFitResizePopup
      luedismissalwho.Properties.SearchMode = SearchMode.AutoComplete
      luedismissalwho.Properties.AutoSearchColumnIndex = 0
      luedismissalwho.Properties.NullText = String.Empty

      Dim suppressUIEventsState = m_SuppressUIEvents
      m_SuppressUIEvents = True
      luedismissalwho.EditValue = Nothing
      m_SuppressUIEvents = suppressUIEventsState

    End Sub

    ''' <summary>
    ''' Loads the dismissalwho drop down data.
    ''' </summary>
    Private Function LoadDismissalWhoDropDownData() As Boolean
			Dim DismissalWhoData = New List(Of DismissalWhoData) From {
			New DismissalWhoData With {.DisplayText = m_Translate.GetSafeTranslationValue("Arbeitgeber"), .Value = "arbeitgeber"},
			 New DismissalWhoData With {.DisplayText = m_Translate.GetSafeTranslationValue("Arbeitnehmer"), .Value = "arbeitnehmer"}
			}

      luedismissalwho.Properties.DataSource = dismissalwhoData
      luedismissalwho.Properties.ForceInitialize()

      Return True
    End Function

    ''' <summary>
    ''' Handles drop down button clicks.
    ''' </summary>
    Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

      Const ID_OF_DELETE_BUTTON As Int32 = 1

      ' If delete button has been clicked reset the drop down.
      If e.Button.Index = ID_OF_DELETE_BUTTON Then
        If TypeOf sender Is BaseEdit Then
          If CType(sender, BaseEdit).Properties.ReadOnly Then
            ' nothing
          Else
            CType(sender, BaseEdit).EditValue = Nothing
          End If
        End If
      End If
    End Sub


#End Region


    Private Class DismissalKindData

      Public Property DisplayText As String
      Public Property Value As String

    End Class


    Private Class DismissalWhoData

      Public Property DisplayText As String
      Public Property Value As String

    End Class


    Private Sub lblZusatz2_Click(sender As System.Object, e As System.EventArgs) Handles lblZusatz2.Click

    End Sub
  End Class


End Namespace

