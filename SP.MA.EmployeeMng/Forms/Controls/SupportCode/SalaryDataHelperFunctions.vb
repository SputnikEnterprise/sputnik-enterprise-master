Imports SP.DatabaseAccess.Employee.DataObjects.MasterdataMng

Public Class SalaryDataHelperFunctions

#Region "Private Consts"

  Private Const Empty_Code As String = Nothing
  Private Const Code_NoQST As String = "0"
  Private Const Empty_ChurchTaxtCode As String = Nothing
  Private Const Empty_NumberOfChildren As Short = 0

  Private Const QST_CODE_G = "G"
	Private Const CHURCH_TAX_CODE_PLUS = "Y"

	Private Const PERMISSION_CODE_C = "C"
	Private Const PERMISSION_CODE_S = ""
	Private Const PERMISSION_CODE_Q = "Q"
  Private Const PERMISSION_CODE_G = "G"

  Private Const COUNTRY_CODE_DE = "DE"

#End Region

#Region "Public Methods"

	''' <summary>
	''' Determines the QST code data.
	''' </summary>
	''' <param name="permission">The permission code.</param>
	''' <param name="countryCode">The countryCode code.</param>
	''' <param name="certificateForResidenceReceived">The certificate for residence received value.</param>
	''' <param name="currentQSTCode">The current qst code.</param>
	''' <param name="taxHelper">The tax helper.</param>
	''' <param name="qstTranslationData">The qst translation data.</param>
	''' <param name="newListOfCodeViewData">The new list of qst codes.</param>
	''' <param name="newQSTCode">The new qst code.</param>
	Public Sub DetermineQSTCodeData(
						ByVal permission As String,
						ByVal countryCode As String,
						ByVal certificateForResidenceReceived As Boolean,
						ByVal currentQSTCode As String,
						ByVal taxHelper As TaxDataHelper,
						ByVal qstTranslationData As IEnumerable(Of SP.Internal.Automations.TaxCodeData),
						ByRef newListOfCodeViewData As List(Of QSTCodeViewData),
						ByRef newQSTCode As String)

		newListOfCodeViewData = New List(Of QSTCodeViewData)
		newQSTCode = currentQSTCode

		'If countryCode = "CH" AndAlso (permission = PERMISSION_CODE_C Or
		'	permission = PERMISSION_CODE_S Or
		'	taxHelper Is Nothing) Then
		If taxHelper Is Nothing Then
			newListOfCodeViewData.Add(CreateNoQSTViewDataObject(qstTranslationData))
			newQSTCode = Code_NoQST
		Else

			Dim codes = taxHelper.DistinctQSTCodes

			For Each c In codes
				Dim cviewData = New QSTCodeViewData With {.Code = c, .Translation = taxHelper.GetTranslationForCode(c)}
				newListOfCodeViewData.Add(cviewData)
			Next

			' Check for border cross entry must be added
			If ((Not countryCode Is Nothing) AndAlso
					(countryCode.ToUpper() = COUNTRY_CODE_DE And
					certificateForResidenceReceived And
					(permission = PERMISSION_CODE_Q Or permission = PERMISSION_CODE_G))) Then
				Dim borderCrosserEntry As New QSTCodeViewData With {.Code = QST_CODE_G, .Translation = taxHelper.GetTranslationForCode(QST_CODE_G)}
				newListOfCodeViewData.Add(borderCrosserEntry)
			End If

		End If

		' Still empty => add no QST value
		If newListOfCodeViewData.Count = 0 Then
			newListOfCodeViewData.Add(CreateNoQSTViewDataObject(qstTranslationData))
			newQSTCode = Code_NoQST
		End If

		' Evalue settings for current QST Code.
		Dim stringToFind As String = newQSTCode
		If Not newListOfCodeViewData.Any(Function(data) data.Code = stringToFind) Then

			If Not newListOfCodeViewData.Any(Function(data) data.Code = Code_NoQST) Then
				newQSTCode = Empty_Code
			Else
				newQSTCode = Code_NoQST
			End If

			If permission Is Nothing Then   ' OrElse (countryCode = "CH" AndAlso permission <> PERMISSION_CODE_S AndAlso permission <> PERMISSION_CODE_C) Then
				newQSTCode = Nothing
			End If

		End If

	End Sub

	''' <summary>
	''' Determine church tax code data.
	''' </summary>
	''' <param name="qstCode">The qst code.</param>
	''' <param name="permission">The permission code.</param>
	''' <param name="currentChurchTaxCode">The curren church tax code.</param>
	''' <param name="taxHelper">The tax helper.</param>
	''' <param name="newListOfChurchTaxCode">The new list of church tax codes.</param>
	''' <param name="newChurchTaxCode">The nech church tax codel</param>
	Public Sub DetermineChurchTaxCodeData(ByVal qstCode As String,
                                 ByVal permission As String,
                                 ByVal currentChurchTaxCode As String,
                                 ByVal taxHelper As TaxDataHelper,
                                 ByRef newListOfChurchTaxCode As List(Of ChurchViewData),
                                 ByRef newChurchTaxCode As String)


    newListOfChurchTaxCode = New List(Of ChurchViewData)
    newChurchTaxCode = currentChurchTaxCode

		If taxHelper Is Nothing Then
			'newListOfChurchTaxCode.Clear()					' don't care about CH, S
			newChurchTaxCode = Empty_ChurchTaxtCode
		End If

		Dim churchTaxes = taxHelper.GetDistinctChurchTaxCodes(qstCode)

		For Each tax In churchTaxes
			Dim cviewData = New ChurchViewData With {
			.ChurchTaxCode = tax,
			.Translation = taxHelper.GetTranslationForChurchTaxCode(tax)}
			newListOfChurchTaxCode.Add(cviewData)
		Next


		' Evalue settings for current church tax code.
		Dim stringToFind As String = newChurchTaxCode
		If Not newListOfChurchTaxCode.Any(Function(data) data.ChurchTaxCode = stringToFind) Then

			newChurchTaxCode = Empty_ChurchTaxtCode

		End If

  End Sub

  ''' <summary>
  ''' Determines number of children data.
  ''' </summary>
  ''' <param name="qstCode">The qst code.</param>
  ''' <param name="churchTaxCode">The church tax code.</param>
  ''' <param name="permission">The permission.</param>
  ''' <param name="taxHelper">The tax helper.</param>
  ''' <param name="currentChildren">The current number of children.</param>
  ''' <param name="newListOfChildren">The new list of children.</param>
  ''' <param name="newChildren">The new number of children.</param>
  Public Sub DetermineNumberOfChildrenData(ByVal qstCode As String,
                                ByVal churchTaxCode As String,
                                ByVal permission As String,
                                ByVal taxHelper As TaxDataHelper,
                                ByVal currentChildren As Short,
                                ByRef newListOfChildren As List(Of NumberOfChildrenViewData),
                                ByRef newChildren As Short)

    If String.IsNullOrEmpty(churchTaxCode) Then
      churchTaxCode = CHURCH_TAX_CODE_PLUS ' empty church tax code is like '+'
    End If

    newListOfChildren = New List(Of NumberOfChildrenViewData)
    newChildren = currentChildren

		'If permission = PERMISSION_CODE_C OrElse permission = PERMISSION_CODE_S OrElse taxHelper Is Nothing Then
		If permission = PERMISSION_CODE_C OrElse taxHelper Is Nothing Then
			newListOfChildren.Add(CreateZeroNumberOfChildrenViewDataObject())
		End If
		If taxHelper Is Nothing Then newChildren = Empty_NumberOfChildren

		Dim childrens = taxHelper.GetDistinctChildren(qstCode, churchTaxCode)
		For Each number In childrens
			Dim cviewData = New NumberOfChildrenViewData With {.NumberOfChildren = number}
			newListOfChildren.Add(cviewData)
		Next


		' Still empty => add zero kids
		If newListOfChildren.Count = 0 Then
			newListOfChildren.Add(CreateZeroNumberOfChildrenViewDataObject())
			newChildren = Empty_NumberOfChildren
		End If

		' Evalue settings for current children.
		Dim valueToFind As Short = newChildren
		If Not newListOfChildren.Any(Function(data) data.NumberOfChildren = valueToFind) Then

			newChildren = Empty_NumberOfChildren

		End If

	End Sub

#End Region

#Region "Private Methods"

	''' <summary>
	''' Create a no QST code view data object.
	''' </summary>
	Private Function CreateNoQSTViewDataObject(ByVal qstTranslationData As IEnumerable(Of SP.Internal.Automations.TaxCodeData)) As QSTCodeViewData
		Return New QSTCodeViewData With {.Code = Code_NoQST,
									.Translation = qstTranslationData.Where(Function(data) data.Rec_Value = Code_NoQST).Select(Function(data) data.Translated_Value).FirstOrDefault()
									}
	End Function


	''' <summary>
	''' Create zero number of children view data object.
	''' </summary>
	Private Function CreateZeroNumberOfChildrenViewDataObject() As NumberOfChildrenViewData
    Return New NumberOfChildrenViewData With {.NumberOfChildren = Empty_NumberOfChildren}
  End Function

#End Region

End Class

''' <summary>
''' Code view data.
''' </summary>
Public Class QSTCodeViewData
  Public Property Code As String
  Public Property Translation As String
  Public ReadOnly Property CodeAndTranslation As String
    Get
      Return String.Format("{0} - {1}", Code, Translation)
    End Get
  End Property
End Class

''' <summary>
''' Church view data.
''' </summary>
Public Class ChurchViewData
  Public Property ChurchTaxCode As String
  Public Property Translation As String
  Public ReadOnly Property ChurchCodeAndTranslation As String
    Get
			Return String.Format("{0} - {1}", ChurchTaxCode, Translation)
		End Get
  End Property
End Class

''' <summary>
''' Number of children view data.
''' </summary>
Public Class NumberOfChildrenViewData
  Public Property NumberOfChildren As Short
End Class