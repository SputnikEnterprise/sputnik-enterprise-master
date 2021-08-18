
Imports System.Collections.Generic
Imports System.IO
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Text
Imports DevExpress.Office.Services
Imports DevExpress.Office.Utils
Imports DevExpress.Utils
Imports DevExpress.XtraRichEdit
Imports DevExpress.XtraRichEdit.Export

Namespace RichEditSendMail


	Public Class PreselectionMailData
		Public Property MailType As MailTypeEnum

		Public Property EmployeeNumber As Integer?
		Public Property ApplicationNumber As Integer?
		Public Property VacancyNumber As Integer?
		Public Property ProposeNumber As Integer?
		Public Property CustomerNumber As Integer?
		Public Property InvoiceNumber As Integer?
		Public Property NumberOfInvoices As Integer?
		Public Property PayrollNumber As Integer?
		Public Property NumberOfPayrolls As Integer?

		Public Property Receiver As String
		Public Property Sender As String
		Public Property MailTemplate As String

		Public Property PDFFilesToSend As List(Of String)

	End Class

	Public Enum MailTypeEnum

		ZV
		ARGB
		PAYROLL
		INVOICE

		EmployeeWOS
		CustomerWOS

		ApplicantOKNotification
		ApplicantCancelNotification
		ApplicationOKNotification
		ApplicationCancelNotification

		MOREINVOICES
		MOREPAYROLLS

		EMPLOYEECOMMON
		CUSTOMERCOMMON

		NOTDEFINED

	End Enum




	Public Class RichEditMailMessageExporter
		Implements IUriProvider
		Private ReadOnly control As RichEditControl
		Private ReadOnly message As MailMessage
		Private attachments As List(Of AttachementInfo)
		Private imageId As Integer

		Public Sub New(ByVal control As RichEditControl, ByVal message As MailMessage)
			Guard.ArgumentNotNull(control, "control")
			Guard.ArgumentNotNull(message, "message")

			Me.control = control
			Me.message = message

		End Sub

		Public Overridable Sub Export()
			Me.attachments = New List(Of AttachementInfo)()

			Dim htmlView As AlternateView = CreateHtmlView()
			message.AlternateViews.Add(htmlView)
			message.IsBodyHtml = True
		End Sub

		Protected Friend Overridable Function CreateHtmlView() As AlternateView
			AddHandler control.BeforeExport, AddressOf OnBeforeExport
			Dim htmlBody As String = control.Document.GetHtmlText(control.Document.Range, Me)
			Dim view As AlternateView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, MediaTypeNames.Text.Html)
			RemoveHandler control.BeforeExport, AddressOf OnBeforeExport

			Dim count As Integer = attachments.Count
			For i As Integer = 0 To count - 1
				Dim info As AttachementInfo = attachments(i)
				Dim resource As New LinkedResource(info.Stream, info.MimeType)
				resource.ContentId = info.ContentId
				view.LinkedResources.Add(resource)
			Next i
			Return view
		End Function

		Private Sub OnBeforeExport(ByVal sender As Object, ByVal e As BeforeExportEventArgs)
			Dim options As HtmlDocumentExporterOptions = TryCast(e.Options, HtmlDocumentExporterOptions)
			If options IsNot Nothing Then
				options.Encoding = Encoding.UTF8
			End If
		End Sub


#Region "IUriProvider Members"

		Public Function CreateCssUri(ByVal rootUri As String, ByVal styleText As String, ByVal relativeUri As String) As String Implements IUriProvider.CreateCssUri
			Return String.Empty
		End Function
		Public Function CreateImageUri(ByVal rootUri As String, ByVal image As OfficeImage, ByVal relativeUri As String) As String Implements IUriProvider.CreateImageUri
			Dim imageName As String = String.Format("image{0}", imageId)
			imageId += 1

			Dim imageFormat As OfficeImageFormat = GetActualImageFormat(image.RawFormat)
			Dim stream As Stream = New MemoryStream(image.GetImageBytes(imageFormat))
			Dim mediaContentType As String = OfficeImage.GetContentType(imageFormat)
			Dim info As New AttachementInfo(stream, mediaContentType, imageName)
			attachments.Add(info)

			Return "cid:" & imageName
		End Function

		Private Function GetActualImageFormat(ByVal _officeImageFormat As OfficeImageFormat) As OfficeImageFormat
			If _officeImageFormat = OfficeImageFormat.Exif OrElse _officeImageFormat = OfficeImageFormat.MemoryBmp Then
				Return OfficeImageFormat.Png
			Else
				Return _officeImageFormat
			End If
		End Function
#End Region
	End Class

	Public Class AttachementInfo
		Private stream_Renamed As Stream
		Private mimeType_Renamed As String
		Private contentId_Renamed As String

		Public Sub New(ByVal stream As Stream, ByVal mimeType As String, ByVal contentId As String)
			Me.stream_Renamed = stream
			Me.mimeType_Renamed = mimeType
			Me.contentId_Renamed = contentId
		End Sub

		Public ReadOnly Property Stream() As Stream
			Get
				Return stream_Renamed
			End Get
		End Property
		Public ReadOnly Property MimeType() As String
			Get
				Return mimeType_Renamed
			End Get
		End Property
		Public ReadOnly Property ContentId() As String
			Get
				Return contentId_Renamed
			End Get
		End Property
	End Class

	Public Class EMailAttachmentDocumentData
		Public Property DocumentLabel As String
		Public Property DocumentFilename As String
		Public Property ScanExtension As String
		Public Property DocContent As Byte()
		Public Property Checked As Boolean?

	End Class



	Public Class MailSendValue
		Public Property Value As Boolean
		Public Property ValueLable As String

	End Class

End Namespace
