
Imports SPProgUtility

Public Class ClsLLZESearchPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationData As Dictionary(Of String, ClsTranslationData)

  Public Property DbConnString2Open As String
  Public Property ZENr2Print As Integer
  Public Property ListSortBez As String
  Public Property ListFilterBez As List(Of String)
  Public Property USSignFileName As String

  Public Property JobNr2Print As String
  Public Property SQL2Open As String
	Public Property CalculateVerfallTagWithCreatedOn As Boolean?
	Public Property ShouldDivideAmount As Boolean?

	Public Property TotalOpenBetrag4Date As Double

End Class


Public Class ClsLLESRPrintSetting

	'''' <summary>
	'''' The Initialization data.
	'''' </summary>
	'Public Property m_initData As SP.Infrastructure.Initialization.InitializeClass

	'''' <summary>
	'''' The translation value helper.
	'''' </summary>
	'Public Property m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	'Public Property m_dtaDataList As List(Of SP.DatabaseAccess.DTAUtility.DataObjects.DtaDataForListing)


	Public Property frmhwnd As String
	Public Property ListSortBez As String
	Public Property ListFilterBez As List(Of String)
	Public Property firstPaymentNumber As Integer()
	Public Property DiskIdentity As String
	Public Property JobNr2Print As String
	Public Property ESRKontoNumber As String

	Public Property ShowAsDesgin As Boolean

	''' <summary>
	''' Date of ESR-File
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property ESRfiledate As DateTime?

	''' <summary>
	''' ESR-Filename
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property ESRFileName As String
	Public Property MandantBankIDNumber As Integer?

End Class
