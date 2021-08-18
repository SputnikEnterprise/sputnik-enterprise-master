
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging

Namespace TempData

	Public Class JsonUtility

		Protected Shared m_Logger As ILogger = New Logger()

		Private m_HtmlUtil As HTMLUtiles.Utilities

		Public Sub New()
			m_HtmlUtil = New HTMLUtiles.Utilities
		End Sub

		Public Function ParseKeywordsJSonResult(ByVal jsonString As String) As List(Of KeywordsData)
			Dim success As Boolean = True
			Dim result As New List(Of KeywordsData)


			Try
				Dim value = Newtonsoft.Json.JsonConvert.DeserializeObject(Of String())(jsonString)

				For Each itm In value
					Dim branchData As New KeywordsData

					branchData.Value = itm
					result.Add(branchData)

				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseKeywordsJSonResult: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function

		Public Function ParseBrachesContractJSonResult(ByVal jsonString As String) As List(Of BranchesData)
			Dim success As Boolean = True
			Dim result As New List(Of BranchesData)


			Try
				Dim value = Newtonsoft.Json.JsonConvert.DeserializeObject(Of BranchesData())(jsonString)

				For Each itm In value
					Dim branchData As New BranchesData
					Dim contractList As New List(Of BranchesContrants)

					For Each Contract In itm.Contracts
						Dim data As New BranchesContrants

						data = Contract

						contractList.Add(data)
					Next
					branchData.Branch = itm.Branch
					branchData.Contracts = contractList


					result.Add(branchData)

				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseBrachesContractJSonResult: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function

		Public Function ParseContractJSonResult(ByVal jsonString As String) As List(Of ContractData)
			Dim success As Boolean = True
			Dim result As New List(Of ContractData)


			Try
				Dim contracts = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ContractData())(jsonString)

				For Each itm In contracts
					Dim data As New ContractData

					data.ContractNumber = itm.ContractNumber
					data.ContractName = itm.ContractName
					data.ContractLanguage = itm.ContractLanguage
					data.ContractResponsible = itm.ContractResponsible
					data.Link = itm.Link
					data.FirstContractConclusion = itm.FirstContractConclusion

					result.Add(data)
				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseContractJSonResult: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function

		Public Function ParseContractVersionsJSonResult(ByVal jsonString As String) As List(Of ContractVersionData)
			Dim success As Boolean = True
			Dim result As New List(Of ContractVersionData)


			Try
				Dim contracts = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ContractVersionData())(jsonString)

				For Each itm In contracts
					Dim data As New ContractVersionData

					data = itm

					result.Add(data)

				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseContractVersionsJSonResult: {0} >>> {1}", jsonString, ex.ToString))

				result = Nothing
			End Try

			Return result
		End Function

		Public Function ParseEditionJSonResult(ByVal jsonString As String) As List(Of ContractVersionEditionData)
			Dim success As Boolean = True
			Dim result As New List(Of ContractVersionEditionData)


			Try
				Dim contracts = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ContractVersionEditionData())(jsonString)

				For Each itm In contracts
					Dim data As New ContractVersionEditionData

					data.ID = itm.ID
					data.slEditionID = itm.slEditionID
					data.Status = itm.Status
					data.Created = itm.Created

					data.clauses = itm.clauses
					data.Cantons = itm.Cantons


					result.Add(data)
				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseEditionJSonResult: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function

		Public Function ParseContractDocumentJSonResult(ByVal jsonString As String) As List(Of ContractEditionDocumentData)
			Dim success As Boolean = True
			Dim result As New List(Of ContractEditionDocumentData)


			Try
				Dim contracts = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ContractEditionDocumentData())(jsonString)

				For Each itm In contracts
					Dim data As New ContractEditionDocumentData
					data = itm


					result.Add(data)

				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseContractDocumentJSonResult: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function

		Public Function ParseDocumentAssignedFileJSonResult(ByVal jsonString As String) As DocumentFileData
			Dim success As Boolean = True
			Dim result As New DocumentFileData


			Try
				Dim contracts = Newtonsoft.Json.JsonConvert.DeserializeObject(Of DocumentFileData)(jsonString)

				'Dim data As New DocumentFileData
				result = contracts


			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseDocumentAssignedFileJSonResult: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function

		Public Function ParseContractLinksJSonResult(ByVal jsonString As String) As List(Of ContractEditionLinksData)
			Dim success As Boolean = True
			Dim result As New List(Of ContractEditionLinksData)


			Try
				Dim contracts = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ContractEditionLinksData())(jsonString)

				For Each itm In contracts
					Dim data As New ContractEditionLinksData
					data = itm


					result.Add(data)

				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseContractLinksJSonResult: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function

		Public Function ParseMindestLohnCriteriasJSonResult(ByVal jsonString As String) As List(Of MindestLohnInputCriteriasData)
			Dim success As Boolean = True
			Dim result As New List(Of MindestLohnInputCriteriasData)

			Try
				Dim contracts = Newtonsoft.Json.JsonConvert.DeserializeObject(Of MindestLohnInputCriteriasData())(jsonString)

				For Each itm In contracts
					Dim data As New MindestLohnInputCriteriasData

					data.ID = itm.ID
					data.Title = itm.Title
					data.Name = itm.Name
					data.ParentID = itm.ParentID

					data.Children = itm.Children

					result.Add(data)
				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseMindestLohnCriteriasData: parsing ID: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function

		Public Function ParseInputValuesForAssignedCriteriaStructureID(ByVal jsonString As String) As List(Of InputValueData)
			Dim success As Boolean = True
			Dim result As New List(Of InputValueData)

			Try
				Dim contracts = Newtonsoft.Json.JsonConvert.DeserializeObject(Of InputValueData())(jsonString)

				For Each itm In contracts
					Dim data As New InputValueData

					data.CriteriaStructureID = itm.CriteriaStructureID

					If Not itm.Value Is Nothing Then
						data.Value = m_HtmlUtil.ConvertToPlainText(itm.Value)
					Else
						data.Value = itm.Value
					End If

					If itm.CriteriaListEntryID Is Nothing Then
						data.CriteriaListEntryID = Val(itm.Value)
						data.CriteriaListEntryID_Org = Nothing
					Else
						data.CriteriaListEntryID = itm.CriteriaListEntryID
						data.CriteriaListEntryID_Org = itm.CriteriaListEntryID
					End If

					result.Add(data)
				Next


			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseInputValuesForAssignedCriteriaStructureID: parsing ID: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function

		Public Function ParseMindestLohnResultJSonResult(ByVal jsonString As String) As MindestLohnResultData
			Dim success As Boolean = True
			Dim result As New MindestLohnResultData


			Try

				Dim itm = Newtonsoft.Json.JsonConvert.DeserializeObject(Of MindestLohnResultData)(jsonString)

				Dim data As New MindestLohnResultData

				data.Salary = itm.Salary
				data.VariableText = itm.VariableText
				data.AdditionalText = itm.AdditionalText
				data.Footnotes = itm.Footnotes
				data.AlternativeTexte = itm.AlternativeTexte


				result = data

			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseMindestLohnResultData: parsing ID: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function

		Public Function ParsePublicationNewsResultJSonResult(ByVal jsonString As String) As List(Of PublicationNewsData)
			Dim success As Boolean = True
			Dim result As New List(Of PublicationNewsData)


			Try
				Dim itm = Newtonsoft.Json.JsonConvert.DeserializeObject(Of PublicationNewsData())(jsonString)


				result = itm.ToList

			Catch ex As Exception
				m_Logger.LogError(String.Format("ParsePublicationNewsResultJSonResult: parsing ID: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function

		Public Function ParseJSonError(ByVal jsonString As String) As ResponseErrorData
			Dim result As New ResponseErrorData With {.Content = String.Empty}

			Try
				If String.IsNullOrWhiteSpace(jsonString) Then Return Nothing

				Dim itm = Newtonsoft.Json.JsonConvert.DeserializeObject(Of ResponseErrorData)(jsonString)

				result = itm

			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseJSonError: {0} >>> {1}", jsonString, ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function


	End Class

End Namespace
