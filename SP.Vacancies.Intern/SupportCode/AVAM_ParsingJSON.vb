
Imports Newtonsoft.Json.Linq

Namespace AVAMWebServiceProcess


	Partial Class WebServiceProcess

		Private Function ParseCreateAddvertismentJSonResult(ByVal jsonString As String) As SPAVAMCreationResultData
			Dim success As Boolean = True
			Dim result As New SPAVAMCreationResultData

			Dim json As JObject = JObject.Parse(jsonString)
			Dim booleanValue As Boolean
			Dim dateValue As Date

			Try
				m_Logger.LogWarning(String.Format("ParseCreateAddvertismentJSonResult: start to parse ID."))

				result.JobroomID = json("id").ToString
				result.AVAMRecordState = json("status").ToString
				result.ExternalReference = json("externalReference").ToString
				result.StellennummerEgov = json("stellennummerEgov").ToString
				result.StellennummerAvam = json("stellennummerAvam").ToString
				result.Fingerprint = json("fingerprint").ToString
				result.JobCenterCode = json("jobCenterCode").ToString

				If Boolean.TryParse(json("reportingObligation").ToString, booleanValue) Then result.ReportingObligation = booleanValue
				If DateTime.TryParse(json("reportingObligationEndDate").ToString, dateValue) Then result.ReportingObligationEndDate = dateValue
				If DateTime.TryParse(json("approvalDate").ToString, dateValue) Then result.ApprovalDate = dateValue

				If DateTime.TryParse(json("rejectionDate").ToString, dateValue) Then result.RejectionDate = dateValue
				result.RejectionCode = json("rejectionCode").ToString
				result.RejectionReason = json("rejectionReason").ToString
				If DateTime.TryParse(json("cancellationDate").ToString, dateValue) Then result.CancellationDate = dateValue
				result.CancellationCode = json("cancellationCode").ToString

				result.ResultContent = jsonString


			Catch ex As Exception
				m_Logger.LogError(String.Format("parsing ID: {0}", ex.ToString))

				Return Nothing
			End Try


			Return result

		End Function

		Private Function ParseJSonError(ByVal jsonString As String) As SPAVAMCreationResultData
			Dim result As New SPAVAMCreationResultData
			Dim errordata As New SPErrorData With {.Content = String.Empty}

			Dim json As JObject = JObject.Parse(jsonString)

			Try
				Dim type As String = json("type").ToString
				Dim title As String = json("title").ToString
				Dim status As String = json("status").ToString
				Dim path As String = json("path").ToString
				Dim message As String = json("message").ToString

				If String.IsNullOrWhiteSpace(title) Then Return Nothing

				errordata.Content = json.ToString
				errordata.Title = title
				errordata.Message = message
				errordata.Status = status

				result.ErrorMessage = errordata


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

				Return Nothing
			End Try

			Return result
		End Function


	End Class


End Namespace
