
Imports SPProgUtility

Public Class ClsSetting

  Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PersonalizedItems As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationItems As Dictionary(Of String, ClsTranslationData)

End Class


Public Class MandantenData

  Public Property MDNr As Integer
  Public Property MDName As String
  Public Property MDGuid As String
  Public Property MDConnStr As String
  Public Property MultiMD As Short

End Class


Public Class FoundedData

  Public Property ZENr As Integer
  Public Property RENr As Integer
  Public Property KDNr As Integer

  Public Property fakdat As Date?
  Public Property vdate As Date?
  Public Property bdate As Date?
  Public Property currency As String

  Public Property betrag As Decimal?
  Public Property mwstbetrag As Decimal?

  Public Property vd As Date?
  Public Property vt As String
  Public Property fbmonat As Integer?
  Public Property fbdat As String

  Public Property fksoll As Integer?
  Public Property fkhaben As Integer?
  Public Property storniert As Integer?
  Public Property mwst As Integer?

  Public Property diskinfo As String

  Public Property createdon As Date?
  Public Property createdfrom As String
  Public Property changedon As Date?
  Public Property changedfrom As String

  Public Property mdnr As Integer?

  Public Property rname1 As String
  Public Property rname2 As String
  Public Property rname3 As String

  Public Property rzhd As String
  Public Property rabteilung As String
  Public Property rpostfach As String

  Public Property rstrasse As String
  Public Property rplz As String
  Public Property rort As String
  Public Property rland As String

  Public Property refkhaben0 As Integer?
  Public Property refkhaben1 As Integer?


  Public Property zahlkond As String
  Public Property faellig As Date?
  Public Property mwstproz As Decimal?

  Public Property kst As String
  Public Property rekst1 As String
  Public Property rekst2 As String
  Public Property reart As String

  Public Property kreditrefnr As String
  Public Property kreditlimite As Decimal?

  Public Property employeeadvisor As String
  Public Property customeradvisor As String
	Public Property InvoiceCreatedOn As Date?


	Public ReadOnly Property VerfallTagFakDate As Integer
		Get
			Return DateDiff(DateInterval.Day, fakdat.GetValueOrDefault(Date.MaxValue), vdate.GetValueOrDefault(Date.MinValue), FirstDayOfWeek.System, FirstWeekOfYear.System)
		End Get
	End Property

	Public ReadOnly Property VerfallTagCreatedOn As Integer
		Get
			Return DateDiff(DateInterval.Day, InvoiceCreatedOn.GetValueOrDefault(Date.MaxValue), vdate.GetValueOrDefault(Date.MinValue), FirstDayOfWeek.System, FirstWeekOfYear.System)
		End Get
	End Property

End Class
