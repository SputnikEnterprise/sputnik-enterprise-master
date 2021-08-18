
Imports DevExpress.XtraEditors.Controls
Imports SP.DatabaseAccess.ES.DataObjects.ESMng
Imports DevExpress.XtraEditors
Imports SP.DatabaseAccess.Common.DataObjects

Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Namespace UI

	Public Class ucKostenteilung


#Region "Private Consts"

		Private Const FORM_XML_REQUIREDFIEKDS_KEY As String = "Forms_Normaly/requiredfields"

#End Region

#Region "Private Fields"

		''' <summary>
		''' The advisors.
		''' </summary>
		Private m_Advisors As List(Of DatabaseAccess.Common.DataObjects.AdvisorData)

		''' <summary>
		''' The cost centers.
		''' </summary>
		Private m_CostCenters As SP.DatabaseAccess.Common.DataObjects.CostCenters

		Private m_IsInitializing As Boolean

		Private m_CurrentGAvDetailData As List(Of GAVDetailData)
		Private m_CurrentSalaryData As ESSalaryData

		Private m_Mandant As Mandant
		Private m_path As ClsProgPath


#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

			m_Mandant = New Mandant
			m_path = New ClsProgPath

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

			AddHandler lueKst1.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueKst2.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueAdvisorMA.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueAdvisorKD.ButtonClick, AddressOf OnDropDown_ButtonClick
			AddHandler lueSubscriber.ButtonClick, AddressOf OnDropDown_ButtonClick
		End Sub

#End Region

#Region "private properties"

		''' <summary>
		''' Gets the selected data.
		''' </summary>
		Private ReadOnly Property SelectedCurrentResultData As GAVDetailData
			Get
				Dim gvList = TryCast(gridGavDetailData.MainView, DevExpress.XtraGrid.Views.Grid.GridView)

				If Not (gvList Is Nothing) Then

					Dim selectedRows = gvList.GetSelectedRows()

					If (selectedRows.Count > 0) Then
						Dim employee = CType(gvList.GetRow(selectedRows(0)), GAVDetailData)
						Return employee
					End If

				End If

				Return Nothing
			End Get

		End Property


#End Region


#Region "Public Methods"

		''' <summary>
		''' Loads data.
		''' </summary>
		''' <param name="esData">The es data.</param>
		''' <returns>Boolean flag indicating success.</returns>
		Public Overrides Function LoadData(ByVal esData As ESMasterData) As Boolean

			Dim success As Boolean = True
			m_IsInitializing = True

			Try

				success = success AndAlso LoadDropDownData()

				SelectKst1(esData.ESKST1)
				lueKst2.EditValue = esData.ESKST2

				SetAdvisorData(esData.ESKst)

				lueSubscriber.EditValue = esData.ESUnterzeichner

				success = success AndAlso SetReadonlyStateOfControls(esData.ESNR)

			Finally
				m_IsInitializing = False
				m_SuppressUIEvents = False
			End Try

			Return success

		End Function

		''' <summary>
		''' Merges ES master data.
		''' </summary>
		''' <param name="esData">The es data.</param>
		Public Overrides Sub MergeESMasterData(ByVal esData As ESMasterData)

			esData.ESKST1 = lueKst1.EditValue
			esData.ESKST2 = lueKst2.EditValue
			esData.ESKst = lblAdvisorCombinedText.Text
			esData.ESUnterzeichner = lueSubscriber.EditValue

		End Sub

		''' <summary>
		''' Resets the control.
		''' </summary>
		Public Overrides Sub Reset()

			lblAdvisorCombinedText.Text = String.Empty

			m_CurrentSalaryData = Nothing
			m_CurrentGAvDetailData = Nothing

			Dim suppressState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			'  Reset drop downs and lists

			ResetKstDropDown()
			ResetEmployeeAdvisorDropDown()
			ResetCustomerAdvisorDropDown()
			ResetSubscriberDropDown()
			ResetGAVDetailGrid()
			m_SuppressUIEvents = suppressState

			xErrorProvider1.ClearErrors()

		End Sub

		''' <summary>
		''' Loads GAV detail data from ES salary data.
		''' </summary>
		''' <param name="esSalaryData">The ES salary data.</param>
		Public Sub LoadGAVDetailData(ByVal esSalaryData As ESSalaryData)

			Dim listOfGAvDetailData As New List(Of GAVDetailData)
			Dim strCategorieBez As String = String.Empty
			Dim strCategorieValue As String = String.Empty
			Dim twoDecimals = "{0:n2}"
			Dim dateFormat = "{0:g}"

			Dim originaGAvData As New GAVStringData()

			gridGavDetailData.DataSource = Nothing
			If String.IsNullOrWhiteSpace(esSalaryData.GAVInfo_String) Then Exit Sub
			originaGAvData.FillFromString(esSalaryData.GAVInfo_String)

			listOfGAvDetailData.Add(New GAVDetailData("GAV-Daten", String.Format("{0} - ({1}): {2}", originaGAvData.GAVNr, originaGAvData.Kanton, originaGAvData.Gruppe0)))
			Dim strFieldname As String = String.Empty
			Dim strValue As String = String.Empty
			Dim aValue As String() = originaGAvData.CompleteGAVString.Split("¦").ToArray

			'For i As Integer = 0 To aValue.Count - 1
			'	strValue = aValue(i).Replace(":.. ", "").Replace("..:", "").Replace("..", "")
			'	If strValue <> String.Empty Then

			'		Dim aKeys As String() = strValue.Split(":")

			'		strCategorieBez = aKeys(0)
			'		strCategorieValue = aKeys(1)
			'		Dim bInsertLine As Boolean = strCategorieValue <> String.Empty
			'		If bInsertLine Then
			'			listOfGAvDetailData.Add(New GAVDetailData(strCategorieBez.Trim, strCategorieValue.Trim))
			'		End If
			'	End If

			'Next

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


			'listOfGAvDetailData.Add(New GAVDetailData("GAVGruppe1", originaGAvData.Gruppe2.Replace("..:", "").Replace("..", "")))
			'listOfGAvDetailData.Add(New GAVDetailData("GAVGruppe3", originaGAvData.Gruppe3.Replace("..:", "").Replace("..", "")))
			'listOfGAvDetailData.Add(New GAVDetailData("GAVBezeichnung", originaGAvData.GAVText.Replace("..:", "").Replace("..", "")))



			'listOfGAvDetailData.Add(New GAVDetailData("ESLohnNr", esSalaryData.ESLohnNr))
			'listOfGAvDetailData.Add(New GAVDetailData("ESNr", esSalaryData.ESNr))
			'listOfGAvDetailData.Add(New GAVDetailData("MANr", esSalaryData.EmployeeNumber))
			'listOfGAvDetailData.Add(New GAVDetailData("KDNr", esSalaryData.CustomerNumber))
			'listOfGAvDetailData.Add(New GAVDetailData("KSTNr", esSalaryData.KSTNr))
			'listOfGAvDetailData.Add(New GAVDetailData("KSTBez", esSalaryData.KSTBez))

			'listOfGAvDetailData.Add(New GAVDetailData("GAV-Daten", String.Format("{0} - ({1}): {2}", esSalaryData.GAVNr, esSalaryData.GAVKanton, esSalaryData.GAVGruppe0)))
			'listOfGAvDetailData.Add(New GAVDetailData("GAVGruppe1", esSalaryData.GAVGruppe1))
			'listOfGAvDetailData.Add(New GAVDetailData("GAVGruppe2", esSalaryData.GAVGruppe2))
			'listOfGAvDetailData.Add(New GAVDetailData("GAVGruppe3", esSalaryData.GAVGruppe3))
			'listOfGAvDetailData.Add(New GAVDetailData("GAVBezeichnung", esSalaryData.GAVBezeichnung))

			'listOfGAvDetailData.Add(New GAVDetailData("GAVKanton", esSalaryData.GAVKanton))
			'listOfGAvDetailData.Add(New GAVDetailData("GAVGruppe0", esSalaryData.GAVGruppe0))

			'listOfGAvDetailData.Add(New GAVDetailData("GavText", esSalaryData.GavText))
			'listOfGAvDetailData.Add(New GAVDetailData("GrundLohn", esSalaryData.GrundLohn, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("StundenLohn", esSalaryData.StundenLohn, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("FerBasis", esSalaryData.FerBasis, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("Ferien", esSalaryData.Ferien, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("FerienProz", esSalaryData.FerienProz, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("Feier", esSalaryData.Feier, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("FeierProz", esSalaryData.FeierProz, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("Basis13", esSalaryData.Basis13, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("Lohn13", esSalaryData.Lohn13, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("Lohn13Proz", esSalaryData.Lohn13Proz, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("Tarif", esSalaryData.Tarif, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("MAStdSpesen", esSalaryData.MAStdSpesen, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("MATSpesen", esSalaryData.MATSpesen, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("KDTSpesen", esSalaryData.KDTSpesen, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("MATotal", esSalaryData.MATotal, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("KDTotal", esSalaryData.KDTotal, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("MWSTBetrag", esSalaryData.MWSTBetrag, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("AktivLODaten", esSalaryData.AktivLODaten))

			listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Bruttomarge"), esSalaryData.BruttoMarge, twoDecimals))
			listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Marge mit BVG"), esSalaryData.MargeMitBVG, twoDecimals))

			If esSalaryData.GAVStdLohn.HasValue AndAlso esSalaryData.GAVStdLohn > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("GAV Stundenlohn"), esSalaryData.GAVStdLohn, twoDecimals))
			If esSalaryData.GAV_FAG.HasValue AndAlso esSalaryData.GAV_FAG > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("FAR-AG, AN"), String.Format("{0:f2}, {1:f2}", esSalaryData.GAV_FAG, esSalaryData.GAV_FAN), twoDecimals))

			'If esSalaryData.GAV_FAN.HasValue AndAlso esSalaryData.GAV_FAN > 0 Then listOfGAvDetailData.Add(New GAVDetailData("FAR-AN", esSalaryData.GAV_FAN, twoDecimals))
			If esSalaryData.GAV_WAG.HasValue AndAlso esSalaryData.GAV_WAG > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Weiterbildung-AG, AN"), String.Format("{0:f2}, {1:f2}", esSalaryData.GAV_WAG, esSalaryData.GAV_WAN), twoDecimals))
			If esSalaryData.GAV_WAG_S.HasValue AndAlso esSalaryData.GAV_WAG_S > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Weiterbildung (Std.)-AG, AN"), String.Format("{0:f2}, {1:f2}", esSalaryData.GAV_WAG_S, esSalaryData.GAV_WAN_S), twoDecimals))

			'If esSalaryData.GAV_WAN.HasValue AndAlso esSalaryData.GAV_WAN > 0 Then listOfGAvDetailData.Add(New GAVDetailData("Weiterbildung-AN", esSalaryData.GAV_WAN, twoDecimals))
			If esSalaryData.GAV_VAG.HasValue AndAlso esSalaryData.GAV_VAG > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Vollzug-AG"), String.Format("{0:f2}, {1:f2}", esSalaryData.GAV_VAG, esSalaryData.GAV_VAN), twoDecimals))
			'If esSalaryData.GAV_VAN.HasValue AndAlso esSalaryData.GAV_VAN > 0 Then listOfGAvDetailData.Add(New GAVDetailData("Vollzug-AN", esSalaryData.GAV_VAN, twoDecimals))


			If esSalaryData.GAV_StdWeek.HasValue AndAlso esSalaryData.GAV_StdWeek > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Anzahl Std / Woche"), esSalaryData.GAV_StdWeek, twoDecimals))
			If esSalaryData.GAV_StdMonth.HasValue AndAlso esSalaryData.GAV_StdMonth > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Anzahl Std / Monat"), esSalaryData.GAV_StdMonth, twoDecimals))
			If esSalaryData.GAV_StdYear.HasValue AndAlso esSalaryData.GAV_StdYear > 0 Then listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("Anzahl Std / Jahr"), esSalaryData.GAV_StdYear, twoDecimals))

			'listOfGAvDetailData.Add(New GAVDetailData("GAV_FAG_S", esSalaryData.GAV_FAG_S, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_FAN_S", esSalaryData.GAV_FAN_S, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_WAG_S", esSalaryData.GAV_WAG_S, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_WAN_S", esSalaryData.GAV_WAN_S, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_VAG_S", esSalaryData.GAV_VAG_S, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_VAN_S", esSalaryData.GAV_VAN_S, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_FAG_M", esSalaryData.GAV_FAG_M, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_FAN_M", esSalaryData.GAV_FAN_M, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_WAG_M", esSalaryData.GAV_WAG_M, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_WAN_M", esSalaryData.GAV_WAN_M, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_VAG_M", esSalaryData.GAV_VAG_M, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_VAN_M", esSalaryData.GAV_VAN_M, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("FerienWay", esSalaryData.FerienWay, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("LO13Way", esSalaryData.LO13Way))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_FAG_J", esSalaryData.GAV_FAG_J, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_FAN_J", esSalaryData.GAV_FAN_J, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_WAG_J", esSalaryData.GAV_WAG_J, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_WAN_J", esSalaryData.GAV_WAN_J, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_VAG_J", esSalaryData.GAV_VAG_J, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("GAV_VAN_J", esSalaryData.GAV_VAN_J))
			'listOfGAvDetailData.Add(New GAVDetailData("IsPVL", esSalaryData.IsPVL))
			'listOfGAvDetailData.Add(New GAVDetailData("FeierBasis", esSalaryData.FeierBasis, twoDecimals))
			'listOfGAvDetailData.Add(New GAVDetailData("LOFeiertagWay", esSalaryData.LOFeiertagWay))
			listOfGAvDetailData.Add(New GAVDetailData(m_Translate.GetSafeTranslationValue("GAV-Datum"), esSalaryData.GavDate, dateFormat))

			' Dim originaGAvData As New GAVStringData()
			' originaGAvData.FillFromString(esSalaryData.GAVInfo_String)

			m_CurrentGAvDetailData = listOfGAvDetailData
			m_CurrentSalaryData = esSalaryData

			gridGavDetailData.DataSource = listOfGAvDetailData

		End Sub

		''' <summary>
		''' Validated data.
		''' </summary>
		Public Overrides Function ValidateData() As Boolean

			Dim isValid As Boolean = True
			Dim errorText As String = m_Translate.GetSafeTranslationValue("Bitte geben Sie einen Wert ein.")

			Dim mandantNumber As Integer = m_UCMediator.MandantNumber
			Dim mustKST1BeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																									String.Format("{0}/kst1selectionines", FORM_XML_REQUIREDFIEKDS_KEY)), False)
			Dim mustKST2BeSelected As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_Mandant.GetSelectedMDFormDataXMLFilename(mandantNumber),
																									String.Format("{0}/kst2selectionines", FORM_XML_REQUIREDFIEKDS_KEY)), False)
			Dim IsDataSelected As Boolean = Not (lueKst1.EditValue Is Nothing AndAlso String.IsNullOrWhiteSpace(lueKst1.EditValue))
			isValid = isValid And SetDXErrorIfInvalid(lueKst1, xErrorProvider1, (mustKST1BeSelected AndAlso (Not IsDataSelected)), errorText)

			IsDataSelected = Not (lueKst2.EditValue Is Nothing AndAlso String.IsNullOrWhiteSpace(lueKst2.EditValue))
			isValid = isValid And SetDXErrorIfInvalid(lueKst2, xErrorProvider1, (mustKST2BeSelected AndAlso (Not IsDataSelected)), errorText)

			isValid = isValid And SetDXErrorIfInvalid(lueAdvisorMA, xErrorProvider1, (lueAdvisorMA.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueAdvisorMA.EditValue)), errorText)
			isValid = isValid And SetDXErrorIfInvalid(lueAdvisorKD, xErrorProvider1, (lueAdvisorKD.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueAdvisorKD.EditValue)), errorText)
			isValid = isValid And SetDXErrorIfInvalid(lueSubscriber, xErrorProvider1, (lueSubscriber.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(lueSubscriber.EditValue)), errorText)


			Return isValid

			Return True
		End Function


#End Region

#Region "Private Methods"

		''' <summary>
		'''  Translate controls.
		''' </summary>
		Protected Overrides Sub TranslateControls()

			Me.grpKostenteilungData.Text = m_Translate.GetSafeTranslationValue(Me.grpKostenteilungData.Text)
			Me.lblKST1.Text = m_Translate.GetSafeTranslationValue(Me.lblKST1.Text)
			Me.lblKST2.Text = m_Translate.GetSafeTranslationValue(Me.lblKST2.Text)
			Me.lblBeraterIn.Text = m_Translate.GetSafeTranslationValue(Me.lblBeraterIn.Text)
			Me.lblMABerater.Text = m_Translate.GetSafeTranslationValue(Me.lblMABerater.Text)
			Me.lblKDBerater.Text = m_Translate.GetSafeTranslationValue(Me.lblKDBerater.Text)

			Me.lblUnterzeichner.Text = m_Translate.GetSafeTranslationValue(Me.lblUnterzeichner.Text)

			Me.grpGAVDetails.Text = m_Translate.GetSafeTranslationValue(Me.grpGAVDetails.Text)

		End Sub

		''' <summary>
		''' Resets the Kst1 and Kst2 drop down.
		''' </summary>
		Private Sub ResetKstDropDown()
			'Kst1
			lueKst1.Properties.ReadOnly = False
			lueKst1.Properties.DisplayMember = "KSTBezeichnung"
			lueKst1.Properties.ValueMember = "KSTName"

			lueKst1.Properties.Columns.Clear()
			lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
			lueKst1.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))

			lueKst1.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueKst1.Properties.SearchMode = SearchMode.AutoComplete
			lueKst1.Properties.AutoSearchColumnIndex = 1
			lueKst1.Properties.NullText = String.Empty
			lueKst1.EditValue = Nothing

			'Kst2
			lueKst2.Properties.ReadOnly = False
			lueKst2.Properties.DisplayMember = "KSTBezeichnung"
			lueKst2.Properties.ValueMember = "KSTName"

			lueKst2.Properties.Columns.Clear()
			lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTName", 0))
			lueKst2.Properties.Columns.Add(New LookUpColumnInfo("KSTBezeichnung", 0))

			lueKst2.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueKst2.Properties.SearchMode = SearchMode.AutoComplete
			lueKst2.Properties.AutoSearchColumnIndex = 1
			lueKst2.Properties.NullText = String.Empty
			lueKst2.EditValue = Nothing
		End Sub

		''' <summary>
		''' Resets the customer advisors drop down.
		''' </summary>
		Private Sub ResetCustomerAdvisorDropDown()

			lueAdvisorKD.Properties.ReadOnly = False
			lueAdvisorKD.Properties.DropDownRows = 20

			lueAdvisorKD.Properties.DisplayMember = "UserFullname"
			lueAdvisorKD.Properties.ValueMember = "KST"

			Dim columns = lueAdvisorKD.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("KST", 0))
			columns.Add(New LookUpColumnInfo("UserFullname", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

			lueAdvisorKD.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueAdvisorKD.Properties.SearchMode = SearchMode.AutoComplete
			lueAdvisorKD.Properties.AutoSearchColumnIndex = 1

			lueAdvisorKD.Properties.NullText = String.Empty
			lueAdvisorKD.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the employee advisors drop down.
		''' </summary>
		Private Sub ResetEmployeeAdvisorDropDown()

			lueAdvisorMA.Properties.ReadOnly = False
			lueAdvisorMA.Properties.DropDownRows = 20

			lueAdvisorMA.Properties.DisplayMember = "UserFullname"
			lueAdvisorMA.Properties.ValueMember = "KST"

			Dim columns = lueAdvisorMA.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("KST", 0))
			columns.Add(New LookUpColumnInfo("UserFullname", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

			lueAdvisorMA.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueAdvisorMA.Properties.SearchMode = SearchMode.AutoComplete
			lueAdvisorMA.Properties.AutoSearchColumnIndex = 1

			lueAdvisorMA.Properties.NullText = String.Empty
			lueAdvisorMA.EditValue = Nothing

		End Sub

		''' <summary>
		''' Resets the subscriber drop down.
		''' </summary>
		Private Sub ResetSubscriberDropDown()

			lueSubscriber.Properties.DropDownRows = 20

			lueSubscriber.Properties.DisplayMember = "UserFullnameReversedWithoutComma"
			lueSubscriber.Properties.ValueMember = "UserFullnameReversedWithoutComma"

			Dim columns = lueSubscriber.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("UserFullnameReversedWithoutComma", 0, m_Translate.GetSafeTranslationValue("BeraterIn")))

			lueSubscriber.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			lueSubscriber.Properties.SearchMode = SearchMode.AutoComplete
			lueSubscriber.Properties.AutoSearchColumnIndex = 1

			lueSubscriber.Properties.NullText = String.Empty
			lueSubscriber.EditValue = Nothing

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
			columnKey.MinWidth = 150
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
		''' Loads drop down data.
		''' </summary>
		Private Function LoadDropDownData() As Boolean

			Dim success As Boolean = True

			success = success AndAlso LoadAdvisorDropDownData()
			success = success AndAlso LoadKst1DropDownData()

			Return success
		End Function

		''' <summary>
		''' Loads the Kst1 drop down data.
		''' </summary>
		Private Function LoadKst1DropDownData() As Boolean
			' Load data
			m_CostCenters = m_CommonDatabaseAccess.LoadCostCenters()

			If m_CostCenters Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kostenstellen konnten nicht geladen werden."))
				Return False
			End If

			' Kst1
			lueKst1.EditValue = Nothing
			lueKst1.Properties.DataSource = m_CostCenters.CostCenter1
			lueKst1.Properties.ForceInitialize()

			' Kst2
			lueKst2.EditValue = Nothing
			lueKst2.Properties.DataSource = Nothing
			lueKst2.Properties.ForceInitialize()

			Return True

		End Function

		''' <summary>
		''' Loads the Kst2 drop down data.
		''' </summary>
		Private Sub LoadKst2DropDown()

			If (m_CostCenters Is Nothing) Then
				Return
			End If

			Dim kst1Name = lueKst1.EditValue
			Dim kst2Data = m_CostCenters.GetCostCenter2ForCostCenter1(kst1Name)

			' Kst2
			lueKst2.EditValue = Nothing
			lueKst2.Properties.DataSource = kst2Data
			lueKst2.Properties.ForceInitialize()

		End Sub

		''' <summary>
		''' Loads the advisor drop down data.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadAdvisorDropDownData() As Boolean

			m_Advisors = m_CommonDatabaseAccess.LoadAllAdvisorsData() 'LoadAdvisorData()

			If m_Advisors Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Beraterdaten konnten nicht geladen werden."))
			End If

			' Customer advisor
			lueAdvisorKD.Properties.DataSource = m_Advisors
			lueAdvisorKD.Properties.ForceInitialize()

			' Employee advisor
			lueAdvisorMA.Properties.DataSource = m_Advisors
			lueAdvisorMA.Properties.ForceInitialize()

			' Subscriber
			lueSubscriber.Properties.DataSource = m_Advisors
			lueSubscriber.Properties.ForceInitialize()

			Return Not m_Advisors Is Nothing
		End Function

		''' <summary>
		''' Loads the Kst1 and Kst2 values for given User.
		''' </summary>
		'''<returns>Boolean flag indicating success.</returns>
		Private Function LoadAdvisorKst12Data() As Boolean

			Dim m_selectedAdvisor = lueAdvisorMA.EditValue
			If m_selectedAdvisor Is Nothing Then
				lueKst1.EditValue = Nothing
				lueKst2.EditValue = Nothing
				lueKst1.Properties.DataSource = Nothing
				lueKst2.Properties.DataSource = Nothing

				Return False
			End If

			Dim givenAdvisors = m_CommonDatabaseAccess.LoadAdvisorDataforGivenAdvisor(m_selectedAdvisor)

			If givenAdvisors Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die gesuchten Beraterdaten konnten nicht geladen werden."))
			End If

			LoadKst1DropDownData()
			LoadKst2DropDown()

			lueKst1.EditValue = givenAdvisors.KST1
			lueKst2.EditValue = givenAdvisors.KST2

			Return Not givenAdvisors Is Nothing
		End Function

		''' <summary>
		''' Handles change of KST1.
		''' </summary>
		Private Sub OnLueKst1_EditValueChanged(sender As Object, e As EventArgs) Handles lueKst1.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			LoadKst2DropDown()
		End Sub

		''' <summary>
		''' Handles change of customer or employee advisor.
		''' </summary>
		Private Sub OnlueBeraterInEditValueChanged(sender As System.Object, e As System.EventArgs) Handles lueAdvisorKD.EditValueChanged, lueAdvisorMA.EditValueChanged

			If m_SuppressUIEvents Then
				Return
			End If

			Dim advisorKD = lueAdvisorKD.EditValue
			Dim advisorMA = lueAdvisorMA.EditValue

			If Not m_IsInitializing Then
				LoadAdvisorKst12Data()
			End If

			If String.IsNullOrWhiteSpace(advisorKD) Then
				lblAdvisorCombinedText.Text = advisorMA
			ElseIf String.IsNullOrWhiteSpace(advisorMA) Then
				lblAdvisorCombinedText.Text = advisorKD
			ElseIf advisorKD = advisorMA Then
				lblAdvisorCombinedText.Text = advisorKD
			Else
				lblAdvisorCombinedText.Text = advisorMA + "/" + advisorKD
			End If
		End Sub

		''' <summary>
		''' Selects the Kst1.
		''' </summary>
		''' <param name="kst1">The kst1</param>
		Private Sub SelectKst1(ByVal kst1 As String)

			Dim suppressUIEventState = m_SuppressUIEvents
			m_SuppressUIEvents = True

			lueKst1.EditValue = kst1
			LoadKst2DropDown()

			m_SuppressUIEvents = suppressUIEventState

		End Sub

		''' <summary>
		''' Selects an advisor and add missing advisor
		''' </summary>
		''' <param name="lueAdvisor">The advisor lookup edit.</param>
		''' <param name="advisorKST">The advisor Kst.</param>
		Private Sub SelectAdvisor(lueAdvisor As LookUpEdit, advisorKST As String)
			Dim advisor = (From a In m_Advisors Where a.KST = advisorKST).FirstOrDefault
			If advisor Is Nothing Then
				'Add missing advisor
				m_Advisors.Add(New DatabaseAccess.Common.DataObjects.AdvisorData With {.KST = advisorKST})
			End If
			lueAdvisor.EditValue = advisorKST
		End Sub

		''' <summary>
		''' Set the advisor data (MA und KD-Berater)
		''' </summary>
		''' <param name="esKst">The ES Kst.</param>
		Private Sub SetAdvisorData(ByVal esKst As String)
			Dim advisorMA As String = Nothing
			Dim advisorKD As String = Nothing

			If Not String.IsNullOrWhiteSpace(esKst) Then

				' split Advisors
				Dim advisors = esKst.Split({"/"c})
				If advisors.Length = 1 Then
					SelectAdvisor(lueAdvisorMA, advisors(0))
					SelectAdvisor(lueAdvisorKD, advisors(0))
				ElseIf advisors.Length = 2 Then
					SelectAdvisor(lueAdvisorMA, advisors(0))
					SelectAdvisor(lueAdvisorKD, advisors(1))
				End If
			Else
				lueAdvisorMA.EditValue = Nothing
				lueAdvisorKD.EditValue = Nothing
			End If

		End Sub

		''' <summary>
		''' Sets enable state of controls.
		''' </summary>
		''' <param name="esNumber">The es number.</param>
		''' <returns>Boolean falg indicating success.</returns>
		Private Function SetReadonlyStateOfControls(ByVal esNumber As Integer) As Boolean

			Dim isKostenteilungReadonly As Boolean? = Not m_ESDataAccess.CheckIfKostenteilungCanBeChanged(esNumber)

			Dim success As Boolean = True

			If Not isKostenteilungReadonly.HasValue Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Abfrage Kostenteilungänderung  fehlgeschlagen."))
				success = False
				isKostenteilungReadonly = False
			End If

			lueKst1.Properties.ReadOnly = isKostenteilungReadonly
			lueKst2.Properties.ReadOnly = isKostenteilungReadonly
			lueAdvisorMA.Properties.ReadOnly = isKostenteilungReadonly
			lueAdvisorKD.Properties.ReadOnly = isKostenteilungReadonly

			Return success
		End Function

		''' <summary>
		''' Handles drop down button clicks.
		''' </summary>
		Private Sub OnDropDown_ButtonClick(sender As Object, e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs)

			Const ID_OF_DELETE_BUTTON As Int32 = 1

			' If delete button has been clicked reset the drop down.
			If e.Button.Index = ID_OF_DELETE_BUTTON Then

				If TypeOf sender Is BaseEdit Then
					If CType(sender, BaseEdit).Properties.ReadOnly Then
						' nothing
					Else
						CType(sender, BaseEdit).EditValue = Nothing
					End If
				End If

			End If
		End Sub

		Sub OngvGavDetail_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs) Handles gvGavDetail.RowCellClick

			If m_InitializationData.UserData.UserNr <> 1 Then Return
			Dim data = SelectedCurrentResultData
			If data Is Nothing Then Return

			If (e.Clicks = 2) Then
				Dim dataRow = gvGavDetail.GetRow(e.RowHandle)
				If Not dataRow Is Nothing AndAlso data.Key = "Bruttomarge" Then

					Dim margenString As String = m_CurrentSalaryData.MargenInfo_String
					If Not String.IsNullOrWhiteSpace(margenString) Then
						LoadMargenInfoData(margenString)
					End If

				End If
			End If

		End Sub

		Private Sub LoadMargenInfoData(ByVal margenString As String)

			Dim frmMarge As New frmMargenInfo(m_InitializationData, m_Translate, margenString)
			frmMarge.LoadMargenInfoList()

			frmMarge.Show()
			frmMarge.BringToFront()

		End Sub

#End Region

	End Class

#Region "Helper Classes"

	''' <summary>
	''' GAV detail data.
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

End Namespace
