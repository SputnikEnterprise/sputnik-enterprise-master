
Imports System
Imports System.IO
Imports System.IO.Compression
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraSplashScreen
Imports SPS.Listing.Print.Utility
Imports SP.DatabaseAccess.Listing.DataObjects

Imports SPSSendMail.RichEditSendMail


Partial Class frmLOPrint

	Private m_EMailPayrollsData As List(Of PrintedPayrollViewData)


	Private Sub OnbbiSendMail_ItemClick(sender As Object, e As ItemClickEventArgs) Handles bbiSendMail.ItemClick
		Dim result As Boolean = True

		result = result AndAlso PrepareEMailAttachmentFiles()
		bbiSendMail.Enabled = Not result  'False

		result = result AndAlso StartMailMerge()

	End Sub

	Private Function StartMailMerge() As Boolean
		Dim result As Boolean = True

		If m_EMailPayrollsData Is Nothing OrElse m_EMailPayrollsData.Count = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten wurden gefunden."))

			Return False
		End If


		Try
			Dim attachmentfiles As New List(Of String)

			Dim frmMail = New frmPayrollEMailMerge(m_InitializationData)

			Dim preselectionSetting As New PreselectionMailData With {.MailType = MailTypeEnum.MOREPAYROLLS}
			Dim payrollmailData As New List(Of PayrollEMailMergeData)

			frmMail.PreselectionData = preselectionSetting

			For Each email In m_EMailPayrollsData
				Dim data As New PayrollEMailMergeData

				data.PayrollNumber = email.PayrollNumber
				data.EmployeeNumber = email.EmployeeNumber
				data.EMail = email.EmployeeEMail
				data.Lastname = email.Lastname
				data.Firstname = email.Firstname

				data.Attachment = email.ExportedFileName
				data.IndividualAttachments = If(email.SendAsZip, Nothing, email.IndividualFiles)
				data.payrollNumbers = email.PayrollNumbers
				data.NumberOfPayrolls = email.NumberOfPayrollsInJob

				payrollmailData.Add(data)
			Next

			frmMail.PayrollEMailMergeData = payrollmailData
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

		Dim exportPath As String = m_InitializationData.UserData.spTempPayrollPath
		Dim eachFilename As String = m_PayrollEachFileName
		Dim finalFilename As String = m_PayrollZipFileName

		m_Utility.ClearAssignedFolder(exportPath)

		Dim payrollData = GetSelectedPayrollEMailItems()

		If payrollData Is Nothing OrElse payrollData.Count = 0 Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Lohnabrechnung Daten konnten nicht geladen werden."))

			Return False
		End If

		eachFilename = eachFilename.Replace("{MDName}", m_InitializationData.MDData.MDName).Replace("{MDOrt}", m_InitializationData.MDData.MDCity)
		eachFilename = String.Format("{0}.pdf", Path.Combine(exportPath, eachFilename))

		finalFilename = finalFilename.Replace("{MDName}", m_InitializationData.MDData.MDName).Replace("{MDOrt}", m_InitializationData.MDData.MDCity)
		finalFilename = String.Format("{0}.pdf", Path.Combine(exportPath, finalFilename))

		SplashScreenManager.CloseForm(False)
		SplashScreenManager.ShowForm(Me, GetType(WaitForm1), True, True, ParentFormState.Unlocked)
		SplashScreenManager.Default.SetWaitFormCaption(m_Translate.GetSafeTranslationValue("Ihre Daten werden zusammengestellt") & Space(20))
		SplashScreenManager.Default.SetWaitFormDescription(m_Translate.GetSafeTranslationValue("Dies kann einige Sekunden dauern") & "...")

		Try
			Dim result = ExportPayroll(payrollData)
			If result Is Nothing OrElse Not result.Printresult OrElse result.JobResultPayrollData.Count = 0 Then Return False


			success = success AndAlso CreateAttachmentFile(result.JobResultPayrollData, eachFilename, finalFilename)
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

	Private Function CreateAttachmentFile(ByVal fileNames As List(Of PrintedPayrollData), ByVal eachFilename As String, ByVal finalFilename As String) As Boolean
		Dim result As Boolean = True
		Dim exportPath As String = m_InitializationData.UserData.spTempPayrollPath

		Dim filename = finalFilename
		Dim strMsg As String = String.Format(m_Translate.GetSafeTranslationValue("Ihre Daten wurden erfolgreich in {0} gespeichert."), Path.Combine(exportPath, filename))

		If fileNames.Count = 0 Then Return False
		finalFilename = Path.ChangeExtension(finalFilename, "pdf")
		m_EMailPayrollsData = New List(Of PrintedPayrollViewData)

		' make zip each payroll
		finalFilename = Path.ChangeExtension(finalFilename, "zip")
		result = result AndAlso BuildAttachmentForEachPayroll(fileNames, eachFilename, finalFilename, Not tgsIndividalFiles.EditValue)

		SplashScreenManager.CloseForm(False)

		Return result
	End Function

	Private Function BuildAttachmentForEachPayroll(ByVal fileNames As List(Of PrintedPayrollData), ByVal eachFilename As String, ByVal finalFilename As String, ByVal makeZIP As Boolean) As Boolean
		Dim result As Boolean = True
		Dim filenameFormula As String = eachFilename
		Dim finalFilenameFormula As String = finalFilename
		Dim exportPath As String = m_InitializationData.UserData.spTempPayrollPath
		Dim exportPathForEachFile As String


		Dim groupedEMail = fileNames.GroupBy(Function(m) New With {Key m.EmployeeNumber, Key m.EmployeeEMail}).Where(Function(g) g.Count() >= 1).Select(Function(g) g.Key).ToList()
		If groupedEMail Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Keine Daten mit EMail-Adresse gefunden."))

			Return False
		End If
		For Each email In groupedEMail
			Dim emailData = fileNames.Where(Function(x) x.EmployeeEMail = email.EmployeeEMail And x.EmployeeNumber = email.EmployeeNumber).ToList()
			If emailData Is Nothing Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Anhänge mit EMail-Adresse {0} wurde nicht gefunden."), email.EmployeeEMail))

				Return False
			End If
			exportPathForEachFile = Path.Combine(exportPath, Guid.NewGuid.ToString)

			result = result AndAlso MergePayrollFiles(emailData, exportPathForEachFile, eachFilename, finalFilename, makeZIP)

		Next

		Return result
	End Function

	Private Function MergePayrollFiles(ByVal payrollEMailData As List(Of PrintedPayrollData), ByVal exportPath As String, ByVal eachFilename As String, ByVal finalFilename As String, ByVal makeZIP As Boolean) As Boolean
		Dim result As Boolean = True
		Dim filenameFormula As String = eachFilename
		Dim finalFilenameFormula As String = finalFilename
		Dim payrollNumbers As New List(Of Integer)
		Dim employeeNumber As Integer = payrollEMailData(0).EmployeeNumber
		Dim emailAddress As String = payrollEMailData(0).EmployeeEMail
		Dim lastName As String = payrollEMailData(0).Lastname
		Dim firstName As String = payrollEMailData(0).Firstname
		Dim firstPayrollNumber As String = payrollEMailData(0).PayrollNumber
		Dim sendAsZip As Boolean = payrollEMailData(0).SendAsZip.GetValueOrDefault(False)
		Dim exportPathForEachFile As String = Path.Combine(exportPath, employeeNumber)
		Dim pathIndividualfiles As String = Path.Combine(exportPath, employeeNumber, "Individual")

		' employee gets always zip file
		Dim automatedMakeZip As Boolean = sendAsZip
		result = result AndAlso VerifyDirectory(pathIndividualfiles)


		If result AndAlso automatedMakeZip Then
			For Each itm In payrollEMailData
				Dim existsFilename As String = Path.GetFileName(itm.ExportedFileName)
				Dim newFilename As String = filenameFormula.Replace("{Nummer}", itm.PayrollNumber).Replace("{MANr}", itm.EmployeeNumber).Replace("{EmployeeNumber}", itm.EmployeeNumber)

				File.Move(itm.ExportedFileName, Path.Combine(exportPathForEachFile, Path.GetFileName(newFilename)))
				itm.ExportedFileName = Path.Combine(exportPathForEachFile, Path.Combine(exportPathForEachFile, Path.GetFileName(newFilename)))

				payrollNumbers.Add(itm.PayrollNumber)
			Next

			If File.Exists(finalFilenameFormula) Then
				finalFilenameFormula = Path.Combine(Path.GetDirectoryName(finalFilename), String.Format("{0} - {1}{2}", Path.GetFileNameWithoutExtension(finalFilename), DateTime.Now.Ticks.ToString, Path.GetExtension(finalFilename)))
			End If

			ZipFile.CreateFromDirectory(exportPathForEachFile, finalFilenameFormula, CompressionLevel.Optimal, False)

			m_EMailPayrollsData.Add(New PrintedPayrollViewData With {.SelectedRec = tgsSelection.EditValue,
									.EmployeeNumber = employeeNumber, .PayrollNumber = firstPayrollNumber, .Firstname = firstName, .Lastname = lastName,
									.MandantLocation = m_InitializationData.MDData.MDCity, .SendAsZip = True,
									.MandantName = m_InitializationData.MDData.MDName, .EmployeeEMail = emailAddress, .NumberOfPayrollsInJob = payrollEMailData.Count,
									.PayrollNumbers = payrollNumbers,
									.ExportedFileName = finalFilenameFormula})

			Try
				m_Utility.ClearAssignedFolder(exportPath)
				'm_Utility.ClearAssignedFiles(exportPath, "*.pdf", SearchOption.AllDirectories)
				'm_Utility.ClearAssignedFolder(exportPath)

			Catch ex As Exception

			End Try

		Else
			Dim individualFiles As New List(Of String)

			For Each itm In payrollEMailData
				Dim existsFilename As String = Path.GetFileName(itm.ExportedFileName)
				Dim newFilename As String = filenameFormula.Replace("{Nummer}", itm.PayrollNumber).Replace("{MANr}", itm.EmployeeNumber).Replace("{EmployeeNumber}", itm.EmployeeNumber)

				File.Move(itm.ExportedFileName, Path.Combine(pathIndividualfiles, Path.GetFileName(newFilename)))
				itm.ExportedFileName = Path.Combine(pathIndividualfiles, Path.Combine(pathIndividualfiles, Path.GetFileName(newFilename)))

				individualFiles.Add(itm.ExportedFileName)
				payrollNumbers.Add(itm.PayrollNumber)
			Next

			m_EMailPayrollsData.Add(New PrintedPayrollViewData With {.SelectedRec = tgsSelection.EditValue,
									.EmployeeNumber = employeeNumber, .PayrollNumber = firstPayrollNumber, .Firstname = firstName, .Lastname = lastName,
									.MandantLocation = m_InitializationData.MDData.MDCity, .SendAsZip = False,
									.MandantName = m_InitializationData.MDData.MDName, .EmployeeEMail = emailAddress, .NumberOfPayrollsInJob = payrollEMailData.Count,
									.PayrollNumbers = payrollNumbers,
									.IndividualFiles = New List(Of String)(individualFiles)})

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


	Private Class PrintedPayrollViewData
		Inherits PrintedPayrollData

		Public Property SelectedRec As Boolean?
		Public Property NumberOfPayrollsInJob As Integer?
		Public Property IndividualFiles As List(Of String)
		Public Property PayrollNumbers As List(Of Integer)

	End Class

End Class


