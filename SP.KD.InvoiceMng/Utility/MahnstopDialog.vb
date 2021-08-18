Imports DevExpress.XtraEditors

''' <summary>
''' Input dialog for a mahnstop date.
''' </summary>
Public Class MahnstopDialog

#Region "Private Fields"

  Private m_SelectedDate As DateTime?

#End Region

#Region "Constructor"

  Public Sub New()

    ' Dieser Aufruf ist für den Designer erforderlich.
    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    AddHandler dateEditControl.ButtonClick, AddressOf OnDropDownButtonClick

  End Sub

#End Region


#Region "Public Properties"

  ''' <summary>
  ''' Gets or sets the selected date.
  ''' </summary>
  Public Property SelectedDate As DateTime?
    Get
      Return m_SelectedDate
    End Get
    Set(value As DateTime?)
      m_SelectedDate = value
      dateEditControl.EditValue = value
    End Set
  End Property

  ''' <summary>
  ''' Gets or sets the message text.
  ''' </summary>
  Public Property MessageText
    Get
      Return lblMessage.Text
    End Get
    Set(value)
      lblMessage.Text = value
    End Set
  End Property

  ''' <summary>
  ''' Gets or sets the title text.
  ''' </summary>
  Public Property TitleText
    Get
      Return Me.Text
    End Get
    Set(value)
      Me.Text = value
    End Set
  End Property

#End Region

#Region "Event Handlers"

  ''' <summary>
  ''' Handles click on the ok button.
  ''' </summary>
  Private Sub OnBtnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
    DialogResult = DialogResult.OK
    m_SelectedDate = dateEditControl.EditValue
    Close()
  End Sub

  ''' <summary>
  ''' Handles click on cancel button.
  ''' </summary>
  Private Sub OnBtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
    DialogResult = DialogResult.Cancel
    m_SelectedDate = Nothing
    Close()
  End Sub

#End Region

#Region "Support Code"

  ''' <summary>
  ''' Handles drop down button clicks.
  ''' </summary>
  Private Sub OnDropDownButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

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

End Class