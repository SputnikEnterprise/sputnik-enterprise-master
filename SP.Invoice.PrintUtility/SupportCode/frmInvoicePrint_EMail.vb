
Imports System.IO
Imports System.IO.Compression
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraSplashScreen
Imports SPS.Listing.Print.Utility
Imports SPSSendMail
Imports SPSSendMail.RichEditSendMail

Partial Class frmInvoicePrint

	Private m_EMailInvoicesData As List(Of PrintedInvoiceViewData)


	'Private Sub OnbtnItems_ButtonClick(sender As Object, e As ButtonPressedEventArgs) Handles btnItems.ButtonClick

	'	Try
	'		Dim tag As Integer = Val(e.Button.Tag)
	'		Dim itemToInsert As String = "{Nummer}"

	'		Select Case tag
	'			Case 1
	'				itemToInsert = "{Nummer}"

	'			Case 2
	'				itemToInsert = "{KDNr}"

	'			Case 3
	'				itemToInsert = "{MDName}"

	'			Case 4
	'				itemToInsert = "{MDOrt}"


	'			Case Else
	'				Return

	'		End Select

	'		Dim insertPos As Integer = txtEachFileName.SelectionStart
	'		txtEachFileName.Text = txtEachFileName.Text.Insert(insertPos, itemToInsert)
	'		txtEachFileName.SelectionStart = insertPos + itemToInsert.Length

	'	Catch ex As Exception

	'	End Try

	'End Sub

	Private Sub OnbbiSendMail_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiSendMail.ItemClick

		Dim result As Boolean = PrepareEMailAttachmentFiles()
		bbiSendMail.Enabled = Not result  'False

		result = result AndAlso StartMailMerge()

	End Sub

	Private Function StartMailMerge() As Boolean
		Dim result As Boolean = True

		If m_EMailInvoicesData Is Nothing OrElse m_EMailInvoicesData.Count = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

			Return False
		End If


		Try
			Dim attachmentfiles As New List(Of String)

			Dim frmMail = New frmInvoiceEMailMerge(m_InitializationData)

			Dim preselectionSetting As New PreselectionMailData With {.MailType = MailTypeEnum.MOREINVOICES}
			Dim preselectionMailmergeSetting As New PreselectionInvoiceEMailMergeData
			Dim invoicemailData As New List(Of InvoiceEMailMergeData)

			frmMail.PreselectionData = preselectionSetting

			For Each email In m_EMailInvoicesData
				Dim data As New InvoiceEMailMergeData

				data.InvoiceNumber = email.InvoiceNumber
				data.CustomerNumber = email.CustomerNumber
				data.REEMail = email.REEMail
				data.Companyname = email.Companyname

				data.Attachment = email.ExportedFileName
				data.IndividualAttachments = If(email.SendAsZip, Nothing, email.IndividualFiles)
				data.invoiceNumbers = email.InvoiceNumbers
				data.NumberOfInvoices = email.NumberOfInvoicesInJob

				invoicemailData.Add(data)
			Next

			frmMail.InvoiceEMailMergeData = invoicemailData
			frmMail.LoadData()

			frmMail.Show()
			frmMail.BringToFront()

		Catch ex As Exception
			Return False

		End Try

		Return result
	End Function

	Private Function PrepareEMailAttachmentFiles() As Boolean
		Dim success As Boolean = True

		Dim exportPath As String = m_InitializationData.UserData.spTempInvoicePath
		Dim eachFilename As String = m_InvoiceEachFileName
		Dim finalFilename As String = m_InvoiceZipFileName

		Dim invoiceNumbers = GetSelectedInvoiceEMailNumbers()
		Dim invoiceData = GetSelectedInvoiceEMailData()

		If invoiceData Is Nothing OrElse invoiceData.Count = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Rechnungsdaten konnten nicht geladen werden."))

			Return False
		End If


		eachFilename = eachFilename.Replace("{MDName}", m_InitializationData.MDData.MDName).Replace("{MDOrt}", m_InitializationData.MDData.MDCity)
		eachFilename = String.Format("{0}.pdf", Path.Combine(exportPath, eachFilename))

		finalFilename = finalFilename.Replace("{MDName}", m_InitializationData.MDData.MDName).Replace("{MDOrt}", m_InitializationData.MDData.MDCity)
		finalFilename = String.Format("{0}.pdf", Path.Combine(exportPath, finalFilename))

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Abfrage wird durchgeführt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			If invoiceNumbers Is Nothing OrElse invoiceNumbers.Count = 0 Then
				Throw New Exception(m_Translate.GetSafeTranslationValue("Sie haben keine Daten ausgewählt."))
			End If
			Dim result = ExportInvoices(invoiceNumbers)
			If result Is Nothing OrElse Not result.Printresult OrElse result.JobResultInvoiceData.Count = 0 Then Return False


			success = success AndAlso CreateAttachmentFile(result.JobResultInvoiceData, eachFilename, finalFilename)
			If Not success Then
				Dim msg As String = m_Translate.GetSafeTranslationValue("Ihre Daten konnten nicht erfolgreich gespeichert werden.")
				msg &= "<br>{0}<br>{1}"
				Dim strMsg As String = String.Format(msg, exportPath, result.PrintresultMessage)

				SplashScreenManager.CloseForm(False)
				m_UtilityUI.ShowErrorDialog(strMsg)

				Return False
			End If


		Catch ex As Exception
			SplashScreenManager.CloseForm(False)
			m_UtilityUI.ShowErrorDialog(ex.ToString)

			Return False
		Finally
			SplashScreenManager.CloseForm(False)

		End Try

		Return success
	End Function

	Private Function CreateAttachmentFile(ByVal fileNames As List(Of PrintedInvoiceData), ByVal eachFilename As String, ByVal finalFilename As String) As Boolean
		Dim result As Boolean = True
		Dim exportPath As String = m_InitializationData.UserData.spTempInvoicePath

		Dim filename = finalFilename
		Dim strMsg As String = String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich in {0} gespeichert."), Path.Combine(exportPath, filename))

		Dim fileOperationSetting = 0 'rep_MailAttachmentSetting.EditValue

		If fileNames.Count = 0 Then Return False
		finalFilename = Path.ChangeExtension(finalFilename, "pdf")
		m_EMailInvoicesData = New List(Of PrintedInvoiceViewData)

		Select Case fileOperationSetting
			Case 0
				' make zip each invoice
				finalFilename = Path.ChangeExtension(finalFilename, "zip")
				result = result AndAlso BuildAttachmentForEachInvoice(fileNames, eachFilename, finalFilename, Not tgsIndividalFiles.EditValue)

			Case 1
				' make one pdf as invoice
				result = result AndAlso BuildAttachmentForEachInvoice(fileNames, eachFilename, finalFilename, False)

			Case 2
				' each invoice one
				result = result AndAlso BuildAttachmentForEachInvoice(fileNames, eachFilename, finalFilename, False)


			Case Else
				Return False

		End Select
		SplashScreenManager.CloseForm(False)

		Return result
	End Function

	Private Function BuildAttachmentForEachInvoice(ByVal fileNames As List(Of PrintedInvoiceData), ByVal eachFilename As String, ByVal finalFilename As String, ByVal makeZIP As Boolean) As Boolean
		Dim result As Boolean = True
		Dim filenameFormula As String = eachFilename
		Dim finalFilenameFormula As String = finalFilename
		Dim exportPath As String = m_InitializationData.UserData.spTempInvoicePath
		Dim exportPathForEachFile As String

		Dim groupedEMail = fileNames.GroupBy(Function(m) New With {Key m.CustomerNumber, Key m.REEMail}).Where(Function(g) g.Count() >= 1).Select(Function(g) g.Key).ToList()
		If groupedEMail Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten mit EMail-Adresse gefunden."))

			Return False
		End If
		For Each email In groupedEMail
			Dim emailData = fileNames.Where(Function(x) x.REEMail = email.REEMail And x.CustomerNumber = email.CustomerNumber).ToList()
			If emailData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Anhänge mit EMail-Adresse {0} wurde nicht gefunden."), email.REEMail))

				Return False
			End If
			exportPathForEachFile = Path.Combine(exportPath, Guid.NewGuid.ToString)


			If emailData(0).OneInvoicePerMail.GetValueOrDefault(False) Then

				result = result AndAlso MergInvoiceFiles(emailData, exportPathForEachFile, eachFilename, finalFilename, makeZIP)
			Else
				result = result AndAlso MergInvoiceFiles(emailData, exportPathForEachFile, eachFilename, finalFilename, makeZIP)
			End If


		Next

		Return result
	End Function

	Private Function MergInvoiceFiles(ByVal invoiceEMailData As List(Of PrintedInvoiceData), ByVal exportPath As String, ByVal eachFilename As String, ByVal finalFilename As String, ByVal makeZIP As Boolean) As Boolean
		Dim result As Boolean = True
		Dim filenameFormula As String = eachFilename
		Dim finalFilenameFormula As String = finalFilename
		Dim customerNumber As Integer = invoiceEMailData(0).CustomerNumber
		Dim invoiceNumbers As New List(Of Integer)
		Dim emailAddress As String = invoiceEMailData(0).REEMail
		Dim sendAsZip As Boolean = invoiceEMailData(0).SendAsZip.GetValueOrDefault(False)
		Dim customerName As String = invoiceEMailData(0).Companyname
		Dim firstInvoiceNumber As String = invoiceEMailData(0).InvoiceNumber
		Dim exportPathForEachFile As String = Path.Combine(exportPath, customerNumber)
		Dim pathIndividualfiles As String = Path.Combine(exportPath, customerNumber, "Individual")

		Dim automatedMakeZip As Boolean = sendAsZip AndAlso Not invoiceEMailData(0).OneInvoicePerMail.GetValueOrDefault(False)

		result = result AndAlso VerifyDirectory(pathIndividualfiles)

		If result AndAlso automatedMakeZip Then
			For Each itm In invoiceEMailData
				Dim existsFilename As String = Path.GetFileName(itm.ExportedFileName)
				Dim newFilename As String = filenameFormula.Replace("{Nummer}", itm.InvoiceNumber).Replace("{KDNr}", itm.CustomerNumber)

				File.Move(itm.ExportedFileName, Path.Combine(exportPathForEachFile, Path.GetFileName(newFilename)))
				itm.ExportedFileName = Path.Combine(exportPathForEachFile, Path.Combine(exportPathForEachFile, Path.GetFileName(newFilename)))
				invoiceNumbers.Add(itm.InvoiceNumber)
			Next

			If File.Exists(finalFilenameFormula) Then
				finalFilenameFormula = Path.Combine(Path.GetDirectoryName(finalFilename), String.Format("{0} - {1}{2}", Path.GetFileNameWithoutExtension(finalFilename), DateTime.Now.Ticks.ToString, Path.GetExtension(finalFilename)))
			End If

			ZipFile.CreateFromDirectory(exportPathForEachFile, finalFilenameFormula, CompressionLevel.Optimal, False)

			m_EMailInvoicesData.Add(New PrintedInvoiceViewData With {.SelectedRec = tgsSelection.EditValue,
									.CustomerNumber = customerNumber, .InvoiceNumber = firstInvoiceNumber, .Companyname = customerName,
									.InvoiceNumbers = invoiceNumbers,
									.MandantLocation = m_InitializationData.MDData.MDCity,
									.MandantName = m_InitializationData.MDData.MDName, .REEMail = emailAddress, .NumberOfInvoicesInJob = invoiceEMailData.Count,
									.ExportedFileName = finalFilenameFormula})

			Try
				m_Utility.ClearAssignedFolder(exportPath)

			Catch ex As Exception

			End Try

		Else
			Dim individualFiles As New List(Of String)

			For Each itm In invoiceEMailData
				Dim existsFilename As String = Path.GetFileName(itm.ExportedFileName)
				Dim newFilename As String = filenameFormula.Replace("{Nummer}", itm.InvoiceNumber).Replace("{KDNr}", itm.CustomerNumber).Replace("{CustomerNumber}", itm.CustomerNumber)

				File.Move(itm.ExportedFileName, Path.Combine(pathIndividualfiles, Path.GetFileName(newFilename)))
				itm.ExportedFileName = Path.Combine(pathIndividualfiles, Path.Combine(pathIndividualfiles, Path.GetFileName(newFilename)))

				individualFiles.Add(itm.ExportedFileName)
				invoiceNumbers.Add(itm.InvoiceNumber)

				If invoiceEMailData(0).OneInvoicePerMail.GetValueOrDefault(False) Then
					m_EMailInvoicesData.Add(New PrintedInvoiceViewData With {.SelectedRec = tgsSelection.EditValue,
									.CustomerNumber = customerNumber, .InvoiceNumber = firstInvoiceNumber, .Companyname = customerName,
									.InvoiceNumbers = invoiceNumbers,
									.MandantLocation = m_InitializationData.MDData.MDCity,
									.MandantName = m_InitializationData.MDData.MDName, .REEMail = emailAddress, .NumberOfInvoicesInJob = 1,
									.IndividualFiles = New List(Of String)(individualFiles)})

					individualFiles.Clear()
					invoiceNumbers.Clear()
				End If

			Next

			If Not invoiceEMailData(0).OneInvoicePerMail.GetValueOrDefault(False) Then
				m_EMailInvoicesData.Add(New PrintedInvoiceViewData With {.SelectedRec = tgsSelection.EditValue,
									.CustomerNumber = customerNumber, .InvoiceNumber = firstInvoiceNumber, .Companyname = customerName,
									.InvoiceNumbers = invoiceNumbers,
									.MandantLocation = m_InitializationData.MDData.MDCity,
									.MandantName = m_InitializationData.MDData.MDName, .REEMail = emailAddress, .NumberOfInvoicesInJob = invoiceEMailData.Count,
									.IndividualFiles = New List(Of String)(individualFiles)})
			End If

		End If


			Return result
	End Function

	Private Function VerifyDirectory(ByVal pathName As String) As Boolean
		Dim result As Boolean = True

		Try

			If Not Directory.Exists(pathName) Then
				Directory.CreateDirectory(pathName)

			Else
				result = result AndAlso m_Utility.ClearAssignedFolder(pathName)

			End If

		Catch ex As Exception
			m_Logger.LogError(ex.ToString)

			Return False
		End Try

		Return result
	End Function


	Private Class PrintedInvoiceViewData
		Inherits PrintedInvoiceData

		Public Property SelectedRec As Boolean?
		Public Property NumberOfInvoicesInJob As Integer?
		Public Property IndividualFiles As List(Of String)
		Public Property InvoiceNumbers As List(Of Integer)

	End Class
End Class
