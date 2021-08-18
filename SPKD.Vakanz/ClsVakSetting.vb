
Imports SPProgUtility

'Public Class ClsSetting

'  Public Property SelectedMDNr As Integer
'  Public Property SelectedMDYear As Integer
'  Public Property SelectedMDGuid As String
'  Public Property LogedUSNr As Integer

'  Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
'  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

'End Class


Public Class PublicationFields

  Public Property vorspann As Boolean
  Public Property taetigkeit As Boolean
  Public Property anforderung As Boolean
  Public Property wirbieten As Boolean

End Class


'Public Class JobCHCounterInfo

'  Public Property AllowedJobQuantity As Integer
'  Public Property ExportedJobQuantity As Integer
'  Public Property IsCounterOK As Boolean

'End Class

'Public Class OstJobCounterInfo

'  Public Property AllowedJobQuantity As Integer
'  Public Property ExportedJobQuantity As Integer
'  Public Property IsCounterOK As Boolean

'End Class


'Public Class MandantenData

'  Public Property MDNr As Integer
'  Public Property MDName As String
'  Public Property MDGuid As String
'  Public Property MDConnStr As String

'End Class


Public Class VacanciesGroupData

  Public Property grouptitle_value As String

  Public Property grouptitle As String

End Class


Public Class ClsVakSetting

	Public Property SelectedVakNr As Integer?
	Public Property SelectedKDNr As Integer?
	Public Property SelectedZHDNr As Integer?

	Public Property IsAllowedJCH As Boolean
  Public Property IsAllowedOstJob As Boolean
	Public Property IsAsDuplicated As Boolean?

	Public Property IsJCHExported As Boolean
  Public Property IsOstJobExported As Boolean
  Public Property IsInternExported As Boolean

  Public Property CountExportedJCH As Integer
  Public Property CountExportedOstJob As Integer
  Public Property CountExportedIntern As Integer

  Public Property CountTotalJCH As Integer
  Public Property CountTotalOstJob As Integer

  Public Property AddDaysToStartDate As Integer

End Class



Public Class OstJobData

  Public Property id As Integer?
  Public Property VakNr As Integer?
  Public Property UserNr As Integer?

  Public Property interneid As String
  Public Property keywords As String
  Public Property linkiframe As String
	Public Property USOSJDirekt_URL As String
	Public Property bewerberlink As String

  Public Property startdate As Date?
  Public Property enddate As Date?

  Public Property ostjob As Boolean?
  Public Property zentraljob As Boolean?
  Public Property minisite As Boolean?
  Public Property nicejob As Boolean?
  Public Property westjob As Boolean?

  Public Property companyhomepage As Boolean?
  Public Property lehrstelle As Boolean?

  Public Property layoutid As Integer?

  Public Property createdon As Date?
  Public Property createdfrom As String
  Public Property changedon As Date?
  Public Property changedfrom As String

  Public Property isonline As Boolean?

	Public ReadOnly Property Direkt_Link As String
		Get
			If String.IsNullOrWhiteSpace(linkiframe) OrElse linkiframe Is Nothing Then
				Return USOSJDirekt_URL
			Else
				Return linkiframe
			End If
		End Get
	End Property

End Class


''' <summary>
''' result of exporting vacancy to jobplattforms
''' </summary>
''' <remarks></remarks>
Public Class UploadResult

  Public Property value As Boolean
  Public Property message As String

End Class
