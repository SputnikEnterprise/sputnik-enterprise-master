
Imports SPProgUtility

Public Class ClsLLZGSearchPrintSetting

	Public Property frmhwnd As String
	Public Property SelectedMDNr As Integer
  Public Property SelectedMDYear As Integer
  Public Property SelectedMDGuid As String
  Public Property LogedUSNr As Integer

  Public PerosonalizedData As Dictionary(Of String, ClsProsonalizedData)
  Public TranslationData As Dictionary(Of String, ClsTranslationData)

	Public Property ShowAsDesgin As Boolean

  Public Property DbConnString2Open As String
  Public Property ZGNr2Print As Integer
  Public Property ListSortBez As String
  Public Property ListFilterBez As List(Of String)
  Public Property USSignFileName As String

  Public Property JobNr2Print As String
  Public Property SQL2Open As String

  Public Property ShowBetragAsPositiv As Boolean

End Class


Public Class ClsLLDTAPrintSetting

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Public Property m_initData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Public Property m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Public Property m_dtaDataList As List(Of SP.DatabaseAccess.DTAUtility.DataObjects.DtaDataForListing)


	Public Property frmhwnd As String
	Public Property ListSortBez As String
	Public Property ListFilterBez As List(Of String)
	Public Property dtaNumber As Integer()
	Public Property JobNr2Print As String
	Public Property ShowAsDesgin As Boolean
	''' <summary>
	''' Date of DTA-File
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property DTAfiledate As DateTime?

	''' <summary>
	''' DTA-Filename
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property DTAFileName As String
	Public Property MandantBankIDNumber As Integer?

End Class




Public Class EmployeeDataForPrintTemplate
	Public Property manr As Integer?
	Public Property employeeLName As String
	Public Property employeeFName As String
	Public Property employeeAHVNr As String
	Public Property gebdate As Date?
	Public Property employeesex As String
	Public Property nationality As String
	Public Property gebort As String
	Public Property chadresse As String
	Public Property plz As String
	Public Property ort As String
	Public Property land As String

End Class

Public Class EmployeeESDataForPrintTemplate
	Public Property esnr As Integer?
	Public Property es_ab As Date?
	Public Property es_ende As Date?
	Public Property esart As String
	Public Property customername As String
	Public Property customerstreet As String
	Public Property customerplz As String
	Public Property customerort As String
	Public Property esals As String

	Public ReadOnly Property zeitraum As String
		Get
			Return String.Format("{0:d} - {1:d}", es_ab, es_ende)
		End Get
	End Property

	Public ReadOnly Property customerdata As String
		Get
			Return String.Format("{0}, {1} {2} {3}", customername, customerstreet, customerplz, customerort)
		End Get
	End Property


End Class
