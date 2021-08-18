
Public Class ClsVakSetting

	Public Property SelectedMDNr As Integer?
	Public Property SelectedVakNr As Integer?
	Public Property SelectedKDNr As Integer?
  Public Property SelectedKDzNr As Integer?

  Public Property Data4SelectedVak As Boolean

  Public Property SelectedDetailNr As Integer?
  Public Property OpenDetailModul As String
  Public Property gvDetailDisplayMember As String

End Class

Public Class JobPlattformsInfoData
	Public Property Customer_ID As String
	Public Property MD_Name1 As String
	Public Property JobplattformLabel As String
	Public Property TranferedJobs As Integer
	Public Property TotalAllowedJobsSlot As Integer
	Public Property TotalSoonExpireJobs As Integer

	Public ReadOnly Property TotalOpenJobs As Integer
		Get
			If JobplattformLabel = "website" Then Return 0
			Return TotalAllowedJobsSlot - TranferedJobs
		End Get
	End Property


End Class
