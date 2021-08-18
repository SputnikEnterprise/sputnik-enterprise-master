Imports SP.Infrastructure.Logging
Imports SPProgUtility

Namespace Initialization

	''' <summary>
	''' Translate given or founded text
	''' </summary>
	Public Class TranslateValuesHelper

#Region "Private Fields"

		Private m_Logger As ILogger = New Logger()

		Private m_TranslationData As Dictionary(Of String, ClsTranslationData)
		Private m_PersonalizedData As Dictionary(Of String, ClsProsonalizedData)

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		''' <param name="translationData">The translation data.</param>
		''' <param name="personalizedData">The persnalized data.</param>
		Public Sub New(ByVal translationData As Dictionary(Of String, ClsTranslationData),
					   ByVal personalizedData As Dictionary(Of String, ClsProsonalizedData))

			Me.m_TranslationData = translationData
			Me.m_PersonalizedData = personalizedData

		End Sub

#End Region

#Region "Methods"

		Function GetSafeTranslationValue(ByVal dicKey As String) As String
			dicKey = String.Format("{0}", dicKey)
			dicKey = Trim(dicKey)
			Dim strPersonalizedItem As String = dicKey

			Try
				If dicKey.Contains("Achtung: Der Benutzer kann nicht ermittelt werden.") Then
					Trace.WriteLine(dicKey)
				End If
				If m_TranslationData.ContainsKey(strPersonalizedItem) Then
					Return m_TranslationData.Item(strPersonalizedItem).LogedUserLanguage

				Else
#If DEBUG Then
					m_Logger.LogWarning(String.Format("not translated key: {0}", dicKey))
#End If

					Return strPersonalizedItem

				End If

			Catch ex As Exception
				Return strPersonalizedItem
			End Try

		End Function

		Function GetSafeTranslationValue(ByVal dicKey As String, ByVal bCheckPersonalizedItem As Boolean) As String
			dicKey = String.Format("{0}", dicKey)
			dicKey = Trim(dicKey)
			Dim strPersonalizedItem As String = dicKey

			Try
				If bCheckPersonalizedItem Then
					If m_PersonalizedData.ContainsKey(dicKey) Then
						strPersonalizedItem = m_PersonalizedData.Item(dicKey).CaptionValue

					Else
						strPersonalizedItem = strPersonalizedItem

					End If
				End If

				If m_TranslationData.ContainsKey(strPersonalizedItem) Then
					Return m_TranslationData.Item(strPersonalizedItem).LogedUserLanguage

				Else
#If DEBUG Then
					m_Logger.LogWarning(String.Format("not translated key: {0}", dicKey))
#End If
					Return strPersonalizedItem

				End If

			Catch ex As Exception
				Return strPersonalizedItem
			End Try

		End Function

		Function GetSafeTranslationValueInOtherLanguage(ByVal dicKey As String, ByVal destLanguage As String) As String
			dicKey = String.Format("{0}", dicKey)
			dicKey = Trim(dicKey)

			Dim strPersonalizedItem As String = dicKey

			Try
				If m_TranslationData.ContainsKey(strPersonalizedItem) Then

					Select Case destLanguage.ToUpper
						Case "IT", "I", "IT-CH"
							Return m_TranslationData.Item(strPersonalizedItem).Translation_IT

						Case "FR", "F", "FR-CH"
							Return m_TranslationData.Item(strPersonalizedItem).Translation_FR
						Case "EN", "E", "EN-CH"
							Return m_TranslationData.Item(strPersonalizedItem).Translation_EN

						Case Else
							Return m_TranslationData.Item(strPersonalizedItem).LogedUserLanguage

					End Select

				Else
#If DEBUG Then
					m_Logger.LogWarning(String.Format("not translated key: {0}", dicKey))
#End If

					Return strPersonalizedItem

				End If

			Catch ex As Exception
				Return strPersonalizedItem
			End Try

		End Function

		Function GetSafeTranslationValueInOtherLanguage(ByVal dicKey As String, ByVal destLanguage As String, ByVal bCheckPersonalizedItem As Boolean) As String
			dicKey = String.Format("{0}", dicKey)
			dicKey = Trim(dicKey)
			Dim strPersonalizedItem As String = dicKey

			Try
				If bCheckPersonalizedItem Then
					If m_PersonalizedData.ContainsKey(dicKey) Then
						strPersonalizedItem = m_PersonalizedData.Item(dicKey).CaptionValue

					Else
						strPersonalizedItem = strPersonalizedItem

					End If
				End If

				If m_TranslationData.ContainsKey(strPersonalizedItem) Then
					Select Case destLanguage.ToUpper
						Case "IT", "I", "IT-CH"
							Return m_TranslationData.Item(strPersonalizedItem).Translation_IT

						Case "FR", "F", "FR-CH"
							Return m_TranslationData.Item(strPersonalizedItem).Translation_FR
						Case "EN", "E", "EN-CH"
							Return m_TranslationData.Item(strPersonalizedItem).Translation_EN

						Case Else
							Return m_TranslationData.Item(strPersonalizedItem).LogedUserLanguage

					End Select

				Else
#If DEBUG Then
					m_Logger.LogWarning(String.Format("not translated key: {0}", dicKey))
#End If
					Return strPersonalizedItem

				End If

			Catch ex As Exception
				Return strPersonalizedItem
			End Try

		End Function


#End Region

	End Class

End Namespace
