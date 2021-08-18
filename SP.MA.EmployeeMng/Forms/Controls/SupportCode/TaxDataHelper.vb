
Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng
Imports SP.Internal.Automations

''' <summary>
''' The tax data helper.
''' </summary>
Public Class TaxDataHelper

#Region "Privte Fields"

	'Private m_Data As EmployeeTaxInfoWebService.TaxDataDTO
	Private m_Data As SP.Internal.Automations.TaxData
	Private m_QstTranslations As IEnumerable(Of SP.Internal.Automations.TaxCodeData)
	Private m_ChurchTaxCodeTranslations As IEnumerable(Of SP.Internal.Automations.TaxChurchCodeData)

#End Region

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	''' <param name="data">The tax data.</param>
	Public Sub New(ByVal data As SP.Internal.Automations.TaxData,
				 ByVal qstTranslations As IEnumerable(Of SP.Internal.Automations.TaxCodeData),
				 ByVal churchTaxCodeTranslations As IEnumerable(Of SP.Internal.Automations.TaxChurchCodeData))
		'Public Sub New(ByVal data As EmployeeTaxInfoWebService.TaxDataDTO,
		'		 ByVal qstTranslations As IEnumerable(Of QSTData),
		'		 ByVal churchTaxCodeTranslations As IEnumerable(Of ChurchTaxCodeData))
		m_Data = data
		m_QstTranslations = qstTranslations
		m_ChurchTaxCodeTranslations = churchTaxCodeTranslations
	End Sub

#End Region

#Region "Public Properties and Methods"

	''' <summary>
	''' Gets the distinct QST codes.
	''' </summary>
	''' <returns>The distinct QST Codes.</returns>
	Public ReadOnly Property DistinctQSTCodes As String()
    Get
      Dim codes = (From d In m_Data.Data
           Select d.Gruppe).Distinct().ToArray()

      Return codes
    End Get
  End Property

  ''' <summary>
  ''' Gets the distinct church taxes codes.
  ''' </summary>
  ''' <param name="qstCode">The qst code.</param>
  ''' <returns>The distinct church taxes codes.</returns>
  Public Function GetDistinctChurchTaxCodes(ByVal qstCode As String) As String()
    Dim churchTaxes = (From d In m_Data.Data
                 Where d.Gruppe = qstCode And Not String.IsNullOrEmpty(d.Kirchensteuer)
                Select d.Kirchensteuer).Distinct().ToArray()

    Return churchTaxes
  End Function

  ''' <summary>
  ''' Gets the distinct children.
  ''' </summary>
  ''' <param name="qstCode">The qst code.</param>
  ''' <param name="churchTaxCode">The church tax code.</param>
  ''' <returns>The distinct children</returns>
  Public Function GetDistinctChildren(ByVal qstCode As String, ByVal churchTaxCode As String) As Short()
    Dim children = (From d In m_Data.Data
                 Where d.Gruppe = qstCode And d.Kirchensteuer = churchTaxCode
                Select d.Kinder).Distinct().ToArray()

    Return children
  End Function

  ''' <summary>
  ''' Gets the translation for a QST code.
  ''' </summary>
  ''' <param name="qstCode">The qst code.</param>
  ''' <returns>The translation for the QST code.</returns>
  Public Function GetTranslationForCode(ByVal qstCode As String) As String

		Dim translation = (From t In m_QstTranslations
						   Where t.Rec_Value = qstCode
						   Select t.Translated_Value).FirstOrDefault()

		Return translation

  End Function

  ''' <summary>
  ''' Gets the translation a the church tax code.
  ''' </summary>
  ''' <param name="churchTaxCode">The church tax code.</param>
  ''' <returns>The translation for the church tax code.</returns>
  Public Function GetTranslationForChurchTaxCode(ByVal churchTaxCode As String) As String

		Dim translation = (From t In m_ChurchTaxCodeTranslations
						   Where t.Rec_Value = churchTaxCode
						   Select t.Translated_Value).FirstOrDefault()

		Return translation

  End Function

#End Region

End Class
