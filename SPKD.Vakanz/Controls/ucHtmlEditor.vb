Imports System.IO
Imports SP.Infrastructure.Logging
Imports SPProgUtility.CommonXmlUtility
Imports SPProgUtility.Mandanten
Imports System.Threading.Tasks
Imports SpiceLogic.WinHTMLEditor.WinForm
Imports SPKD.Vakanz.ClsDataDetail


Public Class ucHtmlEditor

	Implements IDisposable
	Protected Shadows disposed As Boolean = False


	Private m_Logger As ILogger = New Logger()

	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The mandant.
	''' </summary>
	Private m_MandantData As Mandant

	''' <summary>
	''' Boolean flag indicating if initialization is done.
	''' </summary>
	Private m_InitDone As Boolean = False

#Region "Private properties"

	''' <summary>
	''' Gets the font size to show.
	''' </summary>
	''' <returns>Font size to show</returns>
	Private ReadOnly Property FontSize2Show() As String
		Get
			Dim fontSize As Integer = Math.Max(10, CInt(Val(m_MandantData.GetSelectedMDProfilValue(m_InitializationData.MDData.MDNr, Now.Year, "Sonstiges", "FontSize", "0"))))

			Return String.Format("{0}pt", fontSize)

		End Get
	End Property

#End Region

#Region "Public properties"

	''' <summary>
	''' Gets or sets the HTML content.
	''' </summary>
	Public Property HtmlContent
		Get
			Return ctrl1.BodyHtml
		End Get

		Set(value)
			If value Is Nothing Then value = String.Empty
			ctrl1.BodyHtml = value
		End Set
	End Property

#End Region


#Region "Contructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	Public Sub New()

		m_InitializationData = m_InitialData
		' Dieser Aufruf ist für den Designer erforderlich.
		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		Try
			m_MandantData = New Mandant

			AddHandler ctrl1.Pasting, AddressOf ctrl1_Pasting


		Catch ex As Exception
			m_Logger.LogError(ex.ToString)
		End Try

	End Sub

#End Region

#Region "Public Methods"

	''' <summary>
	''' Creates the control.
	''' </summary>
	Public Sub CreateEditorControl()

		If Not m_InitDone Then

			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Cut)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Copy)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Paste)

			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Separators.Separator1)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Bold)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Italic)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Underline)

			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Separators.Separator2)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.UnOrderedList)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.OrderedList)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Indent)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Outdent)

			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Separators.Separator3)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Hyperlink)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Subscript)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.SuperScript)

			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Separators.Separator4)
			CustomHeader.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Print)

			CustomFooter.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.WYSIWYGDesign)
			CustomFooter.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.HtmlPreview)
			CustomFooter.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.HTMLSourceEdit)
			CustomFooter.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.FormatReset)

			CustomFooter.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.Paste)
			CustomFooter.Items.Add(ctrl1.ToolbarItemOverrider.ToolbarItems.PasteFromMSWord)



			ctrl1.DefaultFontSizeInPt = FontSize2Show

			m_InitDone = True
		End If

	End Sub


	Sub ctrl1_Pasting(sender As Object, e As SpiceLogic.HtmlEditorControl.Domain.BOs.EditorEventArgs.PastingHtmlEventArgs)

		Dim text = Clipboard.GetText(TextDataFormat.Text)
		'text = "TreeViewHitTestInfo"
		Clipboard.SetText(text, TextDataFormat.Text)

		'e.Cancel = True
		e.PastingHtml = text
		'	e.PastingRawHtml = text

		'	'HtmlContent = text

	End Sub

	'Private Sub ctrl1_KeyDown(sender As Object, e As SpiceLogic.HtmlEditorControl.Domain.BOs.EditorEventArgs.EditorKeyEventArgs) Handles ctrl1.KeyDown
	'	'If e.CtrlKey AndAlso e.KeyCode = Keys.V Then

	'	'	Dim text = Clipboard.GetText(TextDataFormat.Text)
	'	'	text = text.Replace(vbNewLine, "<p>")
	'	'	HtmlContent = text

	'	'	e.Cancel = True
	'	'End If

	'End Sub

#End Region


	'	Protected Overridable Overloads Sub Dispose( _
	'	 ByVal disposing As Boolean)
	'		If Not Me.disposed Then
	'			If disposing Then

	'			End If
	'			' Add code here to release the unmanaged resource.

	'			' Note that this is not thread safe.
	'		End If
	'		Me.disposed = True
	'	End Sub

	'#Region " IDisposable Support "
	'	' Do not change or add Overridable to these methods.
	'	' Put cleanup code in Dispose(ByVal disposing As Boolean).
	'	Public Overloads Sub Dispose() Implements IDisposable.Dispose
	'		Dispose(True)
	'		GC.SuppressFinalize(Me)
	'	End Sub
	'	Protected Overrides Sub Finalize()
	'		Dispose(False)
	'		MyBase.Finalize()
	'	End Sub
	'#End Region


End Class
