Imports SP.DatabaseAccess.Report.DataObjects

Namespace UI

  Public Class ucReportDetailData2

#Region "Public Methods"

    ''' <summary>
    ''' Resets the control.
    ''' </summary>
    Public Overrides Sub Reset()

      Dim previousState = SetSuppressUIEventsState(True)

      ResetGAVDetailGrid()

      SetSuppressUIEventsState(previousState)

    End Sub

    ''' <summary>
    ''' Loads data of the active report.
    ''' </summary>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Overrides Function LoadDataOfActiveReport() As Boolean

      Dim activeReportData = m_UCMediator.ActiveReportData

      Dim listOfGAvDetailData As New List(Of GAVDetailData)

      If activeReportData Is Nothing Then

        ' Set empty list.
        gridGavDetailData.DataSource = listOfGAvDetailData

        Return True
      End If

      SetSuppressUIEventsState(True)

			Dim report = activeReportData.ReportData

			Dim esData = activeReportData.ESDataOfActiveReport
			Dim selectedSalaryData = m_UCMediator.SelectedESSalaryData
			listOfGAvDetailData = LoadGAVDetailData(selectedSalaryData)


			Dim twoDecimals = "{0:n2}"
      Dim dateFormat = "{0:dd.MM.yyyy}"

			'listOfGAvDetailData.Add(New GAVDetailData("GAV-Daten", String.Format("{0} - ({1}): {2}", report.RPGAV_Nr, report.RPGAV_Kanton, report.RPGAV_Beruf)))
			'listOfGAvDetailData.Add(New GAVDetailData("GAVGruppe1", report.RPGAV_Gruppe1))
			'listOfGAvDetailData.Add(New GAVDetailData("GAVGruppe2", report.RPGAV_Gruppe2))
			'listOfGAvDetailData.Add(New GAVDetailData("GAVGruppe3", report.RPGAV_Gruppe3))
			'listOfGAvDetailData.Add(New GAVDetailData("GAVBezeichnung", report.RPGAV_Text))

			If report.RPGAV_FAG.HasValue AndAlso report.RPGAV_FAG > 0 Then listOfGAvDetailData.Add(New GAVDetailData("FAR-AG", report.RPGAV_FAG, twoDecimals))
      If report.RPGAV_FAN.HasValue AndAlso report.RPGAV_FAN > 0 Then listOfGAvDetailData.Add(New GAVDetailData("FAR-AN", report.RPGAV_FAN, twoDecimals))
      If report.RPGAV_WAG.HasValue AndAlso report.RPGAV_WAG > 0 Then listOfGAvDetailData.Add(New GAVDetailData("Weiterbildung-AG", report.RPGAV_WAG, twoDecimals))
      If report.RPGAV_WAN.HasValue AndAlso report.RPGAV_WAN > 0 Then listOfGAvDetailData.Add(New GAVDetailData("Weiterbildung-AN", report.RPGAV_WAN, twoDecimals))
      If report.RPGAV_VAG.HasValue AndAlso report.RPGAV_VAG > 0 Then listOfGAvDetailData.Add(New GAVDetailData("Vollzug-AG", report.RPGAV_VAG, twoDecimals))
      If report.RPGAV_VAN.HasValue AndAlso report.RPGAV_VAN > 0 Then listOfGAvDetailData.Add(New GAVDetailData("Vollzug-AN", report.RPGAV_VAN, twoDecimals))
      If report.RPGAV_StdWeek.HasValue AndAlso report.RPGAV_StdWeek > 0 Then listOfGAvDetailData.Add(New GAVDetailData("Anzahl Std / Woche", report.RPGAV_StdWeek, twoDecimals))
      If report.RPGAV_StdMonth.HasValue AndAlso report.RPGAV_StdMonth > 0 Then listOfGAvDetailData.Add(New GAVDetailData("Anzahl Std / Month", report.RPGAV_StdMonth, twoDecimals))
      If report.RPGAV_StdYear.HasValue AndAlso report.RPGAV_StdYear > 0 Then listOfGAvDetailData.Add(New GAVDetailData("Anzahl Std / Year", report.RPGAV_StdYear, twoDecimals))
      'If report.RPGAV_StdLohn.HasValue AndAlso esSalaryData.GAVStdLohn > 0 Then listOfGAvDetailData.Add(New GAVDetailData("GAV Stundenlohn", esSalaryData.GAVStdLohn, twoDecimals))

      ' listOfGAvDetailData.Add(New GAVDetailData("GAV-Datum", report.RPGavDate, dateFormat))

      listOfGAvDetailData.Add(New GAVDetailData("Erstellt", String.Format(" {0:f}, {1}", report.CreatedOn, report.CreatedFrom)))

      gridGavDetailData.DataSource = listOfGAvDetailData

      SetSuppressUIEventsState(False)

      Return True
    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    '''  Translate controls.
    ''' </summary>
    Protected Overrides Sub TranslateControls()
    End Sub

    ''' <summary>
    ''' Resets the GAV detail grid.
    ''' </summary>
    Private Sub ResetGAVDetailGrid()

      ' Reset the grid
      gvGavDetail.OptionsView.ShowIndicator = False
      gvGavDetail.OptionsView.ColumnAutoWidth = True
      gvGavDetail.OptionsView.ShowColumnHeaders = False

      gvGavDetail.Columns.Clear()

      Dim columnKey As New DevExpress.XtraGrid.Columns.GridColumn()
      columnKey.Caption = String.Empty
      columnKey.Name = "Key"
      columnKey.FieldName = "Key"
      columnKey.Visible = True
      columnKey.MinWidth = 120
      gvGavDetail.Columns.Add(columnKey)

      Dim columnValue As New DevExpress.XtraGrid.Columns.GridColumn()
      columnValue.Caption = String.Empty
      columnValue.Name = "FormattedValue"
      columnValue.FieldName = "FormattedValue"
      columnValue.Visible = True
      columnValue.MinWidth = 150
      gvGavDetail.Columns.Add(columnValue)

      gridGavDetailData.DataSource = Nothing

    End Sub


		''' <summary>
		''' Loads GAV detail data from ES salary data.
		''' </summary>
		''' <param name="esSalaryData">The ES salary data.</param>
		Private Function LoadGAVDetailData(ByVal esSalaryData As ESSalaryData) As List(Of GAVDetailData)

			Dim listOfGAvDetailData As New List(Of GAVDetailData)
			Dim strCategorieBez As String = String.Empty
			Dim strCategorieValue As String = String.Empty
			Dim twoDecimals = "{0:n2}"
			Dim dateFormat = "{0:g}"

			Dim originaGAvData As New GAVStringData()

			If String.IsNullOrWhiteSpace(esSalaryData.GAVInfo_String) Then Return listOfGAvDetailData
			originaGAvData.FillFromString(esSalaryData.GAVInfo_String)

			listOfGAvDetailData.Add(New GAVDetailData("GAV-Daten", String.Format("{0} - ({1}): {2}", originaGAvData.GAVNr, originaGAvData.Kanton, originaGAvData.Gruppe0)))
			Dim strFieldname As String = String.Empty
			Dim strValue As String = String.Empty
			Dim aValue As String() = originaGAvData.CompleteGAVString.Split("¦").ToArray

			' Gruppe1
			strValue = aValue(9).Replace(":.. ", "")
			If strValue.Contains(":") Then
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)

			Else
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)

			End If
			If strCategorieValue <> String.Empty Then
				listOfGAvDetailData.Add(New GAVDetailData(strCategorieBez.Replace("..:", "").Replace("..", "").Trim, strCategorieValue.Replace("..:", "").Replace("..", "")))
			End If

			' Gruppe2
			strValue = aValue(10).Replace(":.. ", "")
			If strValue.Contains(":") Then
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)

			Else
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)

			End If
			If strCategorieValue <> String.Empty Then
				listOfGAvDetailData.Add(New GAVDetailData(strCategorieBez.Replace("..:", "").Replace("..", "").Trim, strCategorieValue.Replace("..:", "").Replace("..", "")))
			End If

			' Gruppe3
			strValue = aValue(11).Replace(":.. ", "")
			If strValue.Contains(":") Then
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)
			Else
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)

			End If
			If strCategorieValue <> String.Empty Then
				listOfGAvDetailData.Add(New GAVDetailData(strCategorieBez.Replace("..:", "").Replace("..", "").Trim, strCategorieValue.Replace("..:", "").Replace("..", "")))
			End If

			' Bezirk
			strValue = aValue(12).Replace(":.. ", "")
			If strValue.Contains(":") Then
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)
			Else
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)

			End If
			If strCategorieValue <> String.Empty Then
				listOfGAvDetailData.Add(New GAVDetailData(strCategorieBez.Replace("..:", "").Replace("..", "").Trim, strCategorieValue.Replace("..:", "").Replace("..", "")))
			End If

			' Mitarbeiterkategorie
			strValue = aValue(13).Replace(":.. ", "")
			If strValue.Contains(":") Then
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)
			End If
			If strCategorieValue <> String.Empty Then
				listOfGAvDetailData.Add(New GAVDetailData(strCategorieBez.Replace("..:", "").Replace("..", "").Trim, strCategorieValue.Replace("..:", "").Replace("..", "")))
			End If

			' Erfahrung
			strValue = aValue(14).Replace(":.. ", "")
			If strValue.Contains(":") Then
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)
			End If
			If strCategorieValue <> String.Empty Then
				listOfGAvDetailData.Add(New GAVDetailData(strCategorieBez.Replace("..:", "").Replace("..", "").Trim, strCategorieValue.Replace("..:", "").Replace("..", "")))
			End If

			' Res_14
			strValue = aValue(15).Replace(":.. ", "")
			If strValue.Contains(":") Then
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)
			End If
			If strCategorieValue <> String.Empty Then
				listOfGAvDetailData.Add(New GAVDetailData(strCategorieBez.Replace("..:", "").Replace("..", "").Trim, strCategorieValue.Replace("..:", "").Replace("..", "")))
			End If

			' Res_15
			strValue = aValue(16).Replace(":.. ", "")
			If strValue.Contains(":") Then
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)
			End If
			If strCategorieValue <> String.Empty Then
				listOfGAvDetailData.Add(New GAVDetailData(strCategorieBez.Replace("..:", "").Replace("..", "").Trim, strCategorieValue.Replace("..:", "").Replace("..", "")))
			End If

			' Res_16
			strValue = aValue(17).Replace(":.. ", "")
			If strValue.Contains(":") Then
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)
			End If
			If strCategorieValue <> String.Empty Then
				listOfGAvDetailData.Add(New GAVDetailData(strCategorieBez.Replace("..:", "").Replace("..", "").Trim, strCategorieValue.Replace("..:", "").Replace("..", "")))
			End If

			' Res_17
			strValue = aValue(18).Replace(":.. ", "")
			If strValue.Contains(":") Then
				strCategorieBez = strValue.Split(":")(0)
				strCategorieValue = strValue.Split(":")(1)
			End If
			If strCategorieValue <> String.Empty Then
				listOfGAvDetailData.Add(New GAVDetailData(strCategorieBez.Replace("..:", "").Replace("..", "").Trim, strCategorieValue.Replace("..:", "").Replace("..", "")))
			End If

			' TODO: to work!!!
			'If esSalaryData.GAVStdLohn.HasValue AndAlso esSalaryData.GAVStdLohn > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("GAV Stundenlohn"), esSalaryData.GAVStdLohn, twoDecimals))
			'If esSalaryData.GAV_FAG.HasValue AndAlso esSalaryData.GAV_FAG > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("FAR-AG, AN"), String.Format("{0:f2}, {1:f2}", esSalaryData.GAV_FAG, esSalaryData.GAV_FAN), twoDecimals))

			'If esSalaryData.GAV_WAG.HasValue AndAlso esSalaryData.GAV_WAG > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Weiterbildung-AG, AN"), String.Format("{0:f2}, {1:f2}", esSalaryData.GAV_WAG, esSalaryData.GAV_WAN), twoDecimals))
			'If esSalaryData.GAV_WAG_S.HasValue AndAlso esSalaryData.GAV_WAG_S > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Weiterbildung (Std.)-AG, AN"), String.Format("{0:f2}, {1:f2}", esSalaryData.GAV_WAG_S, esSalaryData.GAV_WAN_S), twoDecimals))

			'If esSalaryData.GAV_VAG.HasValue AndAlso esSalaryData.GAV_VAG > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Vollzug-AG"), String.Format("{0:f2}, {1:f2}", esSalaryData.GAV_VAG, esSalaryData.GAV_VAN), twoDecimals))

			'If esSalaryData.GAV_StdWeek.HasValue AndAlso esSalaryData.GAV_StdWeek > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Anzahl Std / Woche"), esSalaryData.GAV_StdWeek, twoDecimals))
			'If esSalaryData.GAV_StdMonth.HasValue AndAlso esSalaryData.GAV_StdMonth > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Anzahl Std / Monat"), esSalaryData.GAV_StdMonth, twoDecimals))
			'If esSalaryData.GAV_StdYear.HasValue AndAlso esSalaryData.GAV_StdYear > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Anzahl Std / Jahr"), esSalaryData.GAV_StdYear, twoDecimals))

			'listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("GAV-Datum"), esSalaryData.GavDate, dateFormat))


			Return listOfGAvDetailData

		End Function


#End Region

#Region "Helper Classes"

		''' <summary>
		''' Gav detail data.
		''' </summary>
		Class GAVDetailData

#Region "Constrctor"

      Public Sub New(ByVal key As String, ByVal value As Object, Optional ByVal formatString As String = Nothing)
        Me.Key = key
        Me.Value = value
        Me.FormatString = formatString
      End Sub

#End Region

#Region "Public Properties"

      Public Property Key As String
      Public Property Value As Object
      Public Property FormatString As String

      Public ReadOnly Property FormattedValue As String
        Get
          If String.IsNullOrEmpty(FormatString) Then
            Return If(Value Is Nothing, String.Empty, Value.ToString())
          Else
            Dim formatted = String.Format(FormatString, Value)

            Return formatted
          End If
        End Get
      End Property

#End Region

    End Class

#End Region

  End Class

End Namespace
