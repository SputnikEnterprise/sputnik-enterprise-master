
Public Class ClsLLOfferSearchPrintSetting

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Public Property m_initData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Public Property m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Public Property frmhwnd As String
	Public Property USSignFileName As String
	Public Property JobNr2Print As String
	Public Property ShowAsDesgin As Boolean
	Public Property ShowAsExport As Boolean

	Public Property offerNumber As Integer
	Public Property customerNumber As Integer
	Public Property cresponsibleNumber As Integer?


End Class
