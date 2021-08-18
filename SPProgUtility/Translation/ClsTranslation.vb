
Imports System.IO
Imports System.Data.SqlClient

Imports NLog
Imports SPProgUtility.ProgPath
Imports SPProgUtility.CommonSettings


Namespace SPTranslation

  Public Class ClsTranslation

    
    Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

    ''' <summary>
    ''' gets translationvalues into one dictionary object
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTranslationInObject() As Dictionary(Of String, ClsTranslationData)
      Dim m_Progpath As New ClsProgPath
      Dim m_Common As New CommonSetting
      Dim translationLookup As New Dictionary(Of String, ClsTranslationData)
      Dim xDoc As XDocument
      Dim strLanguage As String = m_Common.GetLogedUserLanguage
			Dim xmlFileName As String = m_Progpath.GetNewTranslationFilename

			Try

				xDoc = XDocument.Load(xmlFileName)
				Dim lisOfControl = (From control In xDoc.Root.Elements("Control").ToList())

        For Each ctrl In lisOfControl

          Try
            Dim ctrolObject As New ClsTranslationData
						'ctrolObject.TranslationKey = ctrl.Element("CtlLabel").Value ' ctrl.Attribute("Name").Value
						ctrolObject.TranslationKey = ctrl.Attribute("Name").Value

						ctrolObject.Translation_DE = ctrl.Element("CtlLabel").Value
            ctrolObject.Translation_FR = ctrl.Element("CtlLabel_F").Value
            ctrolObject.Translation_IT = ctrl.Element("CtlLabel_I").Value
            ctrolObject.Translation_EN = ctrl.Element("CtlLabel").Value

            Select Case strLanguage.ToUpper
              Case "FR", "F"
                ctrolObject.LogedUserLanguage = ctrolObject.Translation_FR
              Case "IT", "I"
                ctrolObject.LogedUserLanguage = ctrolObject.Translation_IT
              Case "EN", "E"
                ctrolObject.LogedUserLanguage = ctrolObject.Translation_EN

              Case Else
                ctrolObject.LogedUserLanguage = ctrolObject.Translation_DE

            End Select

            translationLookup.Add(ctrolObject.TranslationKey, ctrolObject)


          Catch ex As Exception
						logger.Error(String.Format("File: {0} | Key: {1} >>> {2}", xmlFileName, ctrl.Attribute("Name").Value, ex.ToString))
					End Try


        Next


        Return translationLookup

      Catch ex As Exception

				logger.Error(String.Format("File: {0} >>> {1}", xmlFileName, ex.ToString))
				Return Nothing

      Finally
				'time1.Stop()
				'Dim ts As TimeSpan = time1.Elapsed

				'' Format and display the TimeSpan value.
				'Dim elapsedTime As String = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10)
				'Console.WriteLine("RunTime " + elapsedTime)

      End Try


      'Return Me.dTranslationsData
    End Function

		''' <summary>
		''' gets translationvalues into one dictionary object
		''' </summary>
		''' <param name="language"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetTranslationInObject(ByVal language As String) As Dictionary(Of String, ClsTranslationData)
			Dim m_Progpath As New ClsProgPath
			Dim m_Common As New CommonSetting
			Dim translationLookup As New Dictionary(Of String, ClsTranslationData)
			Dim xDoc As XDocument
			'Dim strLanguage As String = m_Common.GetLogedUserLanguage

			Try

				xDoc = XDocument.Load(m_Progpath.GetNewTranslationFilename)
				Dim lisOfControl = (From control In xDoc.Root.Elements("Control").ToList())

				For Each ctrl In lisOfControl

					Try
						Dim ctrolObject As New ClsTranslationData
						ctrolObject.TranslationKey = ctrl.Attribute("Name").Value

						ctrolObject.Translation_DE = ctrl.Element("CtlLabel").Value
						ctrolObject.Translation_FR = ctrl.Element("CtlLabel_F").Value
						ctrolObject.Translation_IT = ctrl.Element("CtlLabel_I").Value
						ctrolObject.Translation_EN = ctrl.Element("CtlLabel").Value

						Select Case language.ToUpper
							Case "FR", "F"
								ctrolObject.LogedUserLanguage = ctrolObject.Translation_FR
							Case "IT", "I"
								ctrolObject.LogedUserLanguage = ctrolObject.Translation_IT
							Case "EN", "E"
								ctrolObject.LogedUserLanguage = ctrolObject.Translation_EN

							Case Else
								ctrolObject.LogedUserLanguage = ctrolObject.Translation_DE

						End Select

						translationLookup.Add(ctrolObject.TranslationKey, ctrolObject)


					Catch ex As Exception
						logger.Error(String.Format("Key: {0} >>> {1}", ctrl.Attribute("Name").Value, ex.ToString))
					End Try


				Next


				Return translationLookup

			Catch ex As Exception

				logger.Error(ex.ToString)
				Return Nothing

			Finally
				'time1.Stop()
				'Dim ts As TimeSpan = time1.Elapsed

				'' Format and display the TimeSpan value.
				'Dim elapsedTime As String = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10)
				'Console.WriteLine("RunTime " + elapsedTime)

			End Try


			'Return Me.dTranslationsData
		End Function

    ''' <summary>
    ''' übersetzt ein(en) beliebiges(n) Wort/Satz via xml-Datei
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function TranslateText(ByVal strText As String) As String
      Dim m_Progpath As New ClsProgPath
      Dim strOriginText As String = strText
      Dim strResult As String = strText
      Dim _ClsProgSetting As New ClsProgSettingPath

      Dim strUSLang As String = _ClsProgSetting.GetUSLanguage()
      Dim strFilename As String = _ClsProgSetting.GetTranslateDataFile()
      Dim strLocalFilename As String = m_Progpath.GetNewTranslationFilename

      If strUSLang <> String.Empty Then strUSLang = "_" & strUSLang
      Dim strQuery As String = "//Control[@Name=" & Chr(34) & strText & Chr(34) & "]/CtlLabel" & strUSLang
      strResult = _ClsProgSetting.GetXMLValueByQuery(strLocalFilename, strQuery, String.Empty)
      If strResult = String.Empty Then strResult = strText

      If _ClsProgSetting.GetLogedUSNr = 1 Then
        Dim _clsEventlog As New ClsEventLog
        _clsEventlog.WriteToLogFile(String.Format("{0}{1}{0} >>> {0}{2}{0}", Chr(34), strOriginText, strResult), _
                                    String.Empty, "TranslateText", True, _ClsProgSetting.GetTranslationLOGFile())
      End If

      Return strResult
    End Function

  End Class

End Namespace
