''' <summary>
''' An input dialog for a single text line.
''' </summary>
Public Class SingleLineTextInputDialog

#Region "Private Fields"

    Private m_InputText As String

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets or sets the input data.
    ''' </summary>
    Public Property InputText
        Get
            Return m_InputText
        End Get
        Set(value)
            m_InputText = value
            txtInput.Text = value
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
        m_InputText = txtInput.Text
        Close()
    End Sub

    ''' <summary>
    ''' Handles click on cancel button.
    ''' </summary>
    Private Sub OnBtnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        m_InputText = Nothing
        Close()
    End Sub

#End Region

End Class