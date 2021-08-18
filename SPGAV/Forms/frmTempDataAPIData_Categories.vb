Imports System.ComponentModel
Imports DevExpress.XtraEditors
Imports DevExpress.XtraEditors.Controls
Imports SP.Infrastructure
Imports SP.Infrastructure.Logging
Imports SPGAV.SPPVLGAVUtilWebService
Imports SPGAV.TempData

Namespace UI

	Partial Class frmTempDataPVL



#Region "reset"

		Private Sub ResetCategoryControls()

			pnlCategoryValue.BorderStyle = BorderStyles.NoBorder

			lblCat0.Text = String.Empty
			lue0.Properties.DataSource = Nothing
			lblCat0.Visible = False
			lblCatValue0.Visible = False
			lue0.Visible = False

			lblCat1.Text = String.Empty
			lue1.Properties.DataSource = Nothing
			lblCat1.Visible = False
			lblCatValue1.Visible = False
			lue1.Visible = False

			lblCat2.Text = String.Empty
			lue2.Properties.DataSource = Nothing
			lblCat2.Visible = False
			lblCatValue2.Visible = False
			lue2.Visible = False

			lblCat3.Text = String.Empty
			lue3.Properties.DataSource = Nothing
			lblCat3.Visible = False
			lblCatValue3.Visible = False
			lue3.Visible = False

			lblCat4.Text = String.Empty
			lue4.Properties.DataSource = Nothing
			lblCat4.Visible = False
			lblCatValue4.Visible = False
			lue4.Visible = False

			lblCat5.Text = String.Empty
			lue5.Properties.DataSource = Nothing
			lblCat5.Visible = False
			lblCatValue5.Visible = False
			lue5.Visible = False

			lblCat6.Text = String.Empty
			lue6.Properties.DataSource = Nothing
			lblCat6.Visible = False
			lblCatValue6.Visible = False
			lue6.Visible = False

			lblCat7.Text = String.Empty
			lue7.Properties.DataSource = Nothing
			lblCat7.Visible = False
			lblCatValue7.Visible = False
			lue7.Visible = False

			lblCat8.Text = String.Empty
			lue8.Properties.DataSource = Nothing
			lblCat8.Visible = False
			lblCatValue8.Visible = False
			lue8.Visible = False

			lblCat9.Text = String.Empty
			lue9.Properties.DataSource = Nothing
			lblCat9.Visible = False
			lblCatValue9.Visible = False
			lue9.Visible = False

			lblCat10.Text = String.Empty
			lue10.Properties.DataSource = Nothing
			lblCat10.Visible = False
			lblCatValue10.Visible = False
			lue10.Visible = False

			lblCat11.Text = String.Empty
			lue11.Properties.DataSource = Nothing
			lblCat11.Visible = False
			lblCatValue11.Visible = False
			lue11.Visible = False

			lblCat12.Text = String.Empty
			lue12.Properties.DataSource = Nothing
			lblCat12.Visible = False
			lblCatValue12.Visible = False
			lue12.Visible = False

		End Sub

		Private Sub ClearCategorieDropDown()

			m_SuppressUIEvents = True

			For i As Integer = 0 To 12
				ResetCategoryDropDown(i)
			Next

			RemoveHandler lue0.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue1.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue2.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue3.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue4.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue5.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue6.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue7.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue8.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue9.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue10.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue11.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged
			RemoveHandler lue12.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged

			ResetCategoryControls()

			btnTestXML.Visible = False
			grpGAVCategory.Visible = False
			m_SuppressUIEvents = False

		End Sub

		Private Sub ResetCategoryDropDown(ByVal i As Integer)

			Dim ctl As New DevExpress.XtraEditors.LookUpEdit
			Select Case i
				Case 0
					ctl = lue0
				Case 1
					ctl = lue1
				Case 2
					ctl = lue2
				Case 3
					ctl = lue3
				Case 4
					ctl = lue4
				Case 5
					ctl = lue5
				Case 6
					ctl = lue6
				Case 7
					ctl = lue7
				Case 8
					ctl = lue8
				Case 9
					ctl = lue9
				Case 10
					ctl = lue10
				Case 11
					ctl = lue11
				Case 12
					ctl = lue12

			End Select

			ctl.Properties.DisplayMember = "Value"
			ctl.Properties.ValueMember = "CriteriaListEntryID"

			Dim columns = ctl.Properties.Columns
			columns.Clear()
			columns.Add(New LookUpColumnInfo("Value", 0, m_Translate.GetSafeTranslationValue("Bezeichnung")))

			ctl.Properties.BestFitMode = BestFitMode.BestFitResizePopup
			ctl.Properties.SearchMode = SearchMode.AutoComplete
			ctl.Properties.AutoSearchColumnIndex = 0

			ctl.Properties.NullText = String.Empty
			ctl.EditValue = Nothing

		End Sub


#End Region


#Region "setting labels"

		Sub SetCategoriesLabels()
			Dim iTop As Integer = 40
			Dim iLeft As Integer = 30
			Dim iTopCbo As Integer = 50
			Dim iLeftCbo As Integer = 150
			Dim iOldBaseCategoryNr As Integer = 0
			Dim i As Integer = 0
			Dim orglabelLeft As Integer = lblCat0.Left
			Dim orgLueLeft As Integer = lue0.Left
			Dim orgLueWidth As Integer = lue0.Width
			Dim hasBaseCat As Boolean = False
			Dim hasSubBaseCat As Boolean = False
			grpGAVCategory.Visible = False


			Dim data = m_SalaryCriteriasData
			If data Is Nothing Then
				m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Kategorie Daten konnten geladen werden."))
				Return
			End If


			ClearCategorieDropDown()
			Try

				For Each cat In data
					Dim strCategoryName As String = cat.Name
					Dim strIDCategory As String = cat.ID
					'Dim strIDBaseCategory As Integer? = If(cat.Children.Count > 0, 1, 0)

					Dim criteraData = LoadInputValuesForAssignedCriteriaStructureIDData(cat.ID, m_ContractEditionNumber)

					Dim ctl As DevExpress.XtraEditors.LabelControl
					Dim ctlValue As DevExpress.XtraEditors.LabelControl
					Dim lue As DevExpress.XtraEditors.LookUpEdit
					Select Case i
						Case 0
							ctl = lblCat0
							ctlValue = lblCatValue0
							lue = lue0
						Case 1
							ctl = lblCat1
							ctlValue = lblCatValue1
							lue = lue1
						Case 2
							ctl = lblCat2
							ctlValue = lblCatValue2
							lue = lue2
						Case 3
							ctl = lblCat3
							ctlValue = lblCatValue3
							lue = lue3
						Case 4
							ctl = lblCat4
							ctlValue = lblCatValue4
							lue = lue4
						Case 5
							ctl = lblCat5
							ctlValue = lblCatValue5
							lue = lue5
						Case 6
							ctl = lblCat6
							ctlValue = lblCatValue6
							lue = lue6
						Case 7
							ctl = lblCat7
							ctlValue = lblCatValue7
							lue = lue7
						Case 8
							ctl = lblCat8
							ctlValue = lblCatValue8
							lue = lue8
						Case 9
							ctl = lblCat9
							ctlValue = lblCatValue9
							lue = lue9
						Case 10
							ctl = lblCat10
							ctlValue = lblCatValue10
							lue = lue10
						Case 11
							ctl = lblCat11
							ctlValue = lblCatValue11
							lue = lue11
						Case 12
							ctl = lblCat12
							ctlValue = lblCatValue12
							lue = lue12


						Case Else
							m_Logger.LogWarning(String.Format("SetCategoriesLabels: index is not registered: {0} >>> gav_number: {1} more fields are available!!!", i, lueContracts.EditValue))
							Continue For

					End Select
					lue.Properties.DataSource = criteraData
					AddHandler lue.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged

					ctl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Default
					ctl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
					ctl.BackColor = Color.Transparent
					'ctl.Appearance.Font = New Font(ctl.Appearance.GetFont.FontFamily, 8.25!, System.Drawing.FontStyle.Bold)

					lue.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
					lue.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

					ctl.Text = String.Format("{0} ({1})", cat.Title, strIDCategory)
					If cat.Name = "Kanton" Then m_CantonID = i
					If cat.Name = "Alter" OrElse cat.Name = "Età" Then m_AlterID = i
					If cat.Name = "Jahr" OrElse cat.Name = "Anno" Then m_YearID = i

					lue.Left = orgLueLeft
					orglabelLeft = lue.Left - ctl.Width - 5
					ctl.Left = orglabelLeft

					lue.Width = orgLueWidth

					ctl.Enabled = True
					ctl.Visible = True

					ctlValue.Enabled = True
					ctlValue.Visible = True

					lue.Visible = True
					lue.Enabled = True


					Trace.WriteLine(String.Format("strCategoryName: {0} >>> {1}", strCategoryName, cat.ID))

					If Not criteraData Is Nothing Then
						SetStandardValue(lue, ctlValue, cat.Name, criteraData)
					End If

					i += 1
					For Each child In cat.Children
						orglabelLeft += 20
						orgLueLeft += 20
						orgLueWidth -= 20

						Trace.WriteLine(String.Format("main child: {0} >>> {1}", child.Title, child.ID))
						CreateSubCategoryLabels(child, i, orglabelLeft, orgLueLeft, orgLueWidth, True)


						i += 1

						For Each itm In child.Children
							orglabelLeft += 20
							orgLueLeft += 20
							orgLueWidth -= 20

							Trace.WriteLine(String.Format("1. sub child: {0} >>> {1}", itm.Title, itm.ID))
							CreateSubCategoryLabels(itm, i, orglabelLeft, orgLueLeft, orgLueWidth, True)

							i += 1

							For Each secChild In itm.Children
								orglabelLeft += 20
								orgLueLeft += 20
								orgLueWidth -= 20

								Trace.WriteLine(String.Format("2. sub child: {0} >>> {1}", secChild.Title, secChild.ID))
								CreateSubCategoryLabels(secChild, i, orglabelLeft, orgLueLeft, orgLueWidth, True)

								orglabelLeft -= 20
								orgLueLeft -= 20
								orgLueWidth += 20

								i += 1
							Next


							orglabelLeft -= 20
							orgLueLeft -= 20
							orgLueWidth += 20

						Next

						'If child.Children.Count = 0 Then
						'	orglabelLeft -= (20 * (child.Children.Count + 1))
						'	orgLueLeft -= (20 * (child.Children.Count + 1))
						'	orgLueWidth += (20 * (child.Children.Count + 1))
						'End If

						orglabelLeft -= 20
						orgLueLeft -= 20
						orgLueWidth += 20

					Next

					orglabelLeft = ctl.Left
					orgLueLeft = lue.Left
					orgLueWidth = lue.Width

				Next
				ReadSelectedCategoryValues()

			Catch ex As Exception
				m_Logger.LogError(String.Format("Building Categories: {0}", ex.ToString))
			End Try

			Me.grpGAVCategory.Visible = True

		End Sub

		Private Sub CreateSubCategoryLabels(ByVal data As ChildrenData, ByVal ctlIteral As Integer, ByVal orglabelLeft As Integer, ByVal orgLueLeft As Integer, ByVal orgLueWidth As Integer, ByVal enabledCtl As Boolean)
			Dim ctl As DevExpress.XtraEditors.LabelControl
			Dim ctlValue As DevExpress.XtraEditors.LabelControl
			Dim lue As DevExpress.XtraEditors.LookUpEdit

			Dim strCategoryName As String = data.Name
			Dim strIDCategory As String = data.ID
			Dim strIDBaseCategory As Integer? = If(data.Children.Count > 0, 1, 0)
			Dim criteraData = LoadInputValuesForAssignedCriteriaStructureIDData(data.ID, m_ContractEditionNumber)

			Select Case ctlIteral
				Case 0
					ctl = lblCat0
					ctlValue = lblCatValue0
					lue = lue0
				Case 1
					ctl = lblCat1
					ctlValue = lblCatValue1
					lue = lue1
				Case 2
					ctl = lblCat2
					ctlValue = lblCatValue2
					lue = lue2
				Case 3
					ctl = lblCat3
					ctlValue = lblCatValue3
					lue = lue3
				Case 4
					ctl = lblCat4
					ctlValue = lblCatValue4
					lue = lue4
				Case 5
					ctl = lblCat5
					ctlValue = lblCatValue5
					lue = lue5
				Case 6
					ctl = lblCat6
					ctlValue = lblCatValue6
					lue = lue6
				Case 7
					ctl = lblCat7
					ctlValue = lblCatValue7
					lue = lue7
				Case 8
					ctl = lblCat8
					ctlValue = lblCatValue8
					lue = lue8
				Case 9
					ctl = lblCat9
					ctlValue = lblCatValue9
					lue = lue9
				Case 10
					ctl = lblCat10
					ctlValue = lblCatValue10
					lue = lue10
				Case 11
					ctl = lblCat11
					ctlValue = lblCatValue11
					lue = lue11
				Case 12
					ctl = lblCat12
					ctlValue = lblCatValue12
					lue = lue12

				Case Else
					m_Logger.LogWarning(String.Format("more fields are not available!!!", ctlIteral))
					Return

			End Select
			lue.Properties.DataSource = criteraData
			AddHandler lue.EditValueChanged, AddressOf OnlueMindestLohnCategories_EditValueChanged

			ctl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Default
			ctl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
			ctl.BackColor = Color.Transparent

			lue.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
			lue.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

			ctl.Text = String.Format("{0} ({1})", data.Title, strIDCategory)
			lue.Left = orgLueLeft
			orglabelLeft = lue.Left - ctl.Width - 5
			ctl.Left = orglabelLeft
			lue.Width = orgLueWidth

			If data.Name = "Kanton" Then m_CantonID = ctlIteral
			If data.Name = "Alter" OrElse data.Name = "Età" Then m_AlterID = ctlIteral
			If data.Name = "Jahr" OrElse data.Name = "Anno" Then m_YearID = ctlIteral

			ctl.Visible = True
			ctlValue.Visible = True
			lue.Visible = True

			ctl.Enabled = enabledCtl
			ctlValue.Enabled = enabledCtl
			lue.Enabled = enabledCtl


		End Sub

#End Region


#Region "setting lue data"

		Private Sub CreateSubCategory(ByVal data As InputValueData, ByVal ctlIteral As Integer)
			Dim ctl As DevExpress.XtraEditors.LabelControl
			Dim nextCtl As DevExpress.XtraEditors.LabelControl = Nothing
			Dim nextCatValue As DevExpress.XtraEditors.LabelControl = Nothing
			Dim ctlValue As DevExpress.XtraEditors.LabelControl
			Dim lue As DevExpress.XtraEditors.LookUpEdit
			Dim nextLue As DevExpress.XtraEditors.LookUpEdit = Nothing
			Dim afterNextCtl As DevExpress.XtraEditors.LabelControl = Nothing
			Dim afterNextLue As DevExpress.XtraEditors.LookUpEdit = Nothing
			Dim afterNextCatValue As DevExpress.XtraEditors.LabelControl = Nothing

			CreateSubCategory_Staging(data, ctlIteral)
			Return

			Dim value As String = data.Value


			Try

				Select Case ctlIteral
					Case 0
						ctl = lblCat0
						ctlValue = lblCatValue0
						lue = lue0
						nextCatValue = lblCatValue1
						If lblCat1.Visible Then
							nextCtl = lblCat1
							nextLue = lue1
							If lue2.Visible Then
								afterNextLue = lue2
								afterNextCtl = lblCat2
								afterNextCatValue = lblCatValue2
							End If
						End If

					Case 1
						ctl = lblCat1
						ctlValue = lblCatValue1
						lue = lue1
						nextCatValue = lblCatValue2
						If lblCat2.Visible Then
							nextCtl = lblCat2
							nextLue = lue2
							If lue3.Visible Then
								afterNextLue = lue3
								afterNextCtl = lblCat3
								afterNextCatValue = lblCatValue3
							End If
						End If
					Case 2
						ctl = lblCat2
						ctlValue = lblCatValue2
						lue = lue2
						nextCatValue = lblCatValue3
						If lblCat3.Visible Then
							nextCtl = lblCat3
							nextLue = lue3
							If lue4.Visible Then
								afterNextLue = lue4
								afterNextCtl = lblCat4
								afterNextCatValue = lblCatValue4
							End If
						End If
					Case 3
						ctl = lblCat3
						ctlValue = lblCatValue3
						lue = lue3
						nextCatValue = lblCatValue4
						If lblCat4.Visible Then
							nextCtl = lblCat4
							nextLue = lue4
							If lue5.Visible Then
								afterNextLue = lue5
								afterNextCtl = lblCat5
								afterNextCatValue = lblCatValue5
							End If
						End If
					Case 4
						ctl = lblCat4
						ctlValue = lblCatValue4
						lue = lue4
						nextCatValue = lblCatValue5
						If lblCat5.Visible Then
							nextCtl = lblCat5
							nextLue = lue5
							If lue6.Visible Then
								afterNextLue = lue6
								afterNextCtl = lblCat6
								afterNextCatValue = lblCatValue6
							End If
						End If
					Case 5
						ctl = lblCat5
						ctlValue = lblCatValue5
						lue = lue5
						nextCatValue = lblCatValue6
						If lblCat6.Visible Then
							nextCtl = lblCat6
							nextLue = lue6
							If lue7.Visible Then
								afterNextLue = lue7
								afterNextCtl = lblCat7
								afterNextCatValue = lblCatValue7
							End If
						End If

					Case 6
						ctl = lblCat6
						ctlValue = lblCatValue6
						lue = lue6
						nextCatValue = lblCatValue7
						If lblCat7.Visible Then
							nextCtl = lblCat7
							nextLue = lue7
							If lue8.Visible Then
								afterNextLue = lue8
								afterNextCtl = lblCat8
								afterNextCatValue = lblCatValue8
							End If
						End If
					Case 7
						ctl = lblCat7
						ctlValue = lblCatValue7
						lue = lue7
						nextCatValue = lblCatValue8
						If lblCat8.Visible Then
							nextCtl = lblCat8
							nextLue = lue8
							If lue9.Visible Then
								afterNextLue = lue9
								afterNextCtl = lblCat9
								afterNextCatValue = lblCatValue9
							End If
						End If

					Case 8
						ctl = lblCat8
						ctlValue = lblCatValue8
						lue = lue8
						nextCatValue = lblCatValue9
						If lblCat9.Visible Then
							nextCtl = lblCat9
							nextLue = lue9
							If lue10.Visible Then
								afterNextLue = lue10
								afterNextCtl = lblCat10
								afterNextCatValue = lblCatValue10
							End If
						End If

					Case 9
						ctl = lblCat9
						ctlValue = lblCatValue9
						lue = lue9
						nextCatValue = lblCatValue10
						If lblCat10.Visible Then
							nextCtl = lblCat10
							nextLue = lue10
							If lue11.Visible Then
								afterNextLue = lue11
								afterNextCtl = lblCat11
								afterNextCatValue = lblCatValue11
							End If
						End If
					Case 10
						ctl = lblCat10
						ctlValue = lblCatValue10
						lue = lue10
						nextCatValue = lblCatValue11
						If lblCat11.Visible Then
							nextCtl = lblCat11
							nextLue = lue11
							If lue12.Visible Then
								afterNextLue = lue12
								afterNextCtl = lblCat12
								afterNextCatValue = lblCatValue12
							End If
						End If


					Case 11
						ctl = lblCat11
						ctlValue = lblCatValue11
						lue = lue11
						nextCatValue = lblCatValue12
						If lblCat12.Visible Then
							nextCtl = lblCat12
							nextLue = lue12
						End If

					Case 12
						ctl = lblCat12
						ctlValue = lblCatValue12
						lue = lue12


					Case Else
						nextCtl = Nothing
						nextLue = Nothing

						m_Logger.LogWarning(String.Format("more fields are not available!!!", ctlIteral))
						Return

				End Select

				If Not nextCtl Is Nothing AndAlso nextCtl.Visible Then
					m_SuppressUIEvents = True

					nextLue.EditValue = Nothing
					If Not afterNextLue Is Nothing Then
						afterNextLue.EditValue = Nothing
						afterNextCatValue.Text = String.Empty
					End If
					m_SuppressUIEvents = False

					nextLue.Properties.DataSource = Nothing
					If Not nextCatValue Is Nothing Then nextCatValue.Text = String.Empty

					Dim nextCtlID As Integer = Val(nextCtl.Text.Split("(")(1))

					m_Logger.LogDebug(String.Format("{0} >>>", nextCtl.Text))
					Dim nextlueData = LoadInputValuesForAssignedCriteriaStructureIDData(nextCtlID, m_ContractEditionNumber)

					If nextlueData Is Nothing OrElse (nextlueData.Count = 1 AndAlso nextlueData(0).Value Is Nothing) OrElse nextlueData.Count = 0 Then
						nextLue.Enabled = False
						nextLue.Properties.DataSource = nextlueData

						If ctlIteral + 1 = m_CantonID Then
							SetStandardValue(nextLue, nextCatValue, "Kanton", nextlueData)
						ElseIf ctlIteral + 1 = m_AlterID Then
							SetStandardValue(nextLue, nextCatValue, "Alter", nextlueData)
						ElseIf ctlIteral + 1 = m_YearID Then
							SetStandardValue(nextLue, nextCatValue, "Jahr", nextlueData)
						End If

						If Not afterNextLue Is Nothing Then
							nextCtlID = Val(afterNextCtl.Text.Split("(")(1))

							m_Logger.LogDebug(String.Format("{0} >>>", afterNextCtl.Text))
							nextlueData = LoadInputValuesForAssignedCriteriaStructureIDData(nextCtlID, m_ContractEditionNumber)

							If nextlueData Is Nothing OrElse (nextlueData.Count = 1 AndAlso nextlueData(0).Value Is Nothing) Then
								afterNextLue.Enabled = False
								afterNextLue.Properties.DataSource = nextlueData
								'LoadMinumumSalaryData()

							Else
								afterNextLue.Enabled = True
								afterNextLue.Properties.DataSource = nextlueData

								If ctlIteral + 2 = m_CantonID Then
									SetStandardValue(afterNextLue, afterNextCatValue, "Kanton", nextlueData)
								ElseIf ctlIteral + 2 = m_AlterID Then
									SetStandardValue(afterNextLue, afterNextCatValue, "Alter", nextlueData)
								ElseIf ctlIteral + 2 = m_YearID Then
									SetStandardValue(afterNextLue, afterNextCatValue, "Jahr", nextlueData)
								End If

							End If

						Else
							'LoadMinumumSalaryData()

						End If

					Else
						nextLue.Enabled = True
						nextLue.Properties.DataSource = nextlueData
						If ctlIteral + 1 = m_CantonID Then
							SetStandardValue(nextLue, nextCatValue, "Kanton", nextlueData)
						ElseIf ctlIteral + 1 = m_AlterID Then
							SetStandardValue(nextLue, nextCatValue, "Alter", nextlueData)
						ElseIf ctlIteral + 1 = m_YearID Then
							SetStandardValue(nextLue, nextCatValue, "Jahr", nextlueData)
						End If

						If Not String.IsNullOrWhiteSpace(nextLue.Text) Then nextCatValue.Text = nextLue.EditValue

						If Not afterNextLue Is Nothing Then
							nextCtlID = Val(afterNextCtl.Text.Split("(")(1))

							m_Logger.LogDebug(String.Format("{0} >>>", afterNextCtl.Text))
							nextlueData = LoadInputValuesForAssignedCriteriaStructureIDData(nextCtlID, m_ContractEditionNumber)

							afterNextLue.Enabled = Not nextlueData Is Nothing AndAlso nextlueData.Count > 0 'False
							afterNextLue.Properties.DataSource = nextlueData

							If afterNextLue.Enabled Then
								If ctlIteral + 2 = m_CantonID Then
									SetStandardValue(afterNextLue, afterNextCatValue, "Kanton", nextlueData)
								ElseIf ctlIteral + 2 = m_AlterID Then
									SetStandardValue(afterNextLue, afterNextCatValue, "Alter", nextlueData)
								ElseIf ctlIteral + 2 = m_YearID Then
									SetStandardValue(afterNextLue, afterNextCatValue, "Jahr", nextlueData)
								End If
							End If

						Else

						End If

					End If

				Else
					'LoadMinumumSalaryData()

				End If
				lue.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
				lue.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

				LoadMinumumSalaryData()

				lue.Visible = True

			Catch ex As Exception

			End Try

		End Sub

		Private Sub CreateSubCategory_Staging(ByVal data As InputValueData, ByVal ctlIteral As Integer)
			Dim ctl As DevExpress.XtraEditors.LabelControl
			Dim nextCtl As DevExpress.XtraEditors.LabelControl = Nothing
			Dim nextCatValue As DevExpress.XtraEditors.LabelControl = Nothing
			Dim ctlValue As DevExpress.XtraEditors.LabelControl
			Dim lue As DevExpress.XtraEditors.LookUpEdit
			Dim nextLue As DevExpress.XtraEditors.LookUpEdit = Nothing
			Dim afterNextCtl As DevExpress.XtraEditors.LabelControl = Nothing
			Dim afterNextLue As DevExpress.XtraEditors.LookUpEdit = Nothing
			Dim afterNextCatValue As DevExpress.XtraEditors.LabelControl = Nothing

			Dim value As String = data.Value

			Try
				pnlCategoryValue.SuspendLayout()

				For i As Integer = ctlIteral To 12
					Dim nextCtlID As Integer = 0
					Trace.WriteLine(String.Format("starting i-value: {0}", i))

					Select Case i
						Case 0
							nextLue = lue1
							ctl = lblCat1
							ctlValue = lblCatValue1
						Case 1
							nextLue = lue2
							ctl = lblCat2
							ctlValue = lblCatValue2
						Case 2
							nextLue = lue3
							ctl = lblCat3
							ctlValue = lblCatValue3
						Case 3
							nextLue = lue4
							ctl = lblCat4
							ctlValue = lblCatValue4
						Case 4
							nextLue = lue5
							ctl = lblCat5
							ctlValue = lblCatValue5
						Case 5
							nextLue = lue6
							ctl = lblCat6
							ctlValue = lblCatValue6
						Case 6
							nextLue = lue7
							ctl = lblCat7
							ctlValue = lblCatValue7
						Case 7
							nextLue = lue8
							ctl = lblCat8
							ctlValue = lblCatValue8
						Case 8
							nextLue = lue9
							ctl = lblCat9
							ctlValue = lblCatValue9
						Case 9
							nextLue = lue10
							ctl = lblCat10
							ctlValue = lblCatValue10
						Case 10
							nextLue = lue11
							ctl = lblCat11
							ctlValue = lblCatValue11
						Case 11
							nextLue = lue12
							ctl = lblCat12
							ctlValue = lblCatValue12

						Case Else
							Exit For

					End Select
					If ctl Is Nothing OrElse Not ctl.Visible Then Exit For

					nextCtlID = Val(ctl.Text.Split("(")(1))

					m_Logger.LogDebug(String.Format("{0} >>> {1}", ctl.Text, nextCtlID))
					Dim nextlueData = LoadInputValuesForAssignedCriteriaStructureIDData(nextCtlID, m_ContractEditionNumber)
					nextLue.Properties.DataSource = nextlueData


					If nextlueData Is Nothing OrElse nextlueData.Count = 0 OrElse (nextlueData.Count = 1 AndAlso nextlueData(0).CriteriaListEntryID_Org Is Nothing) Then
						nextLue.Enabled = False
						ctl.Enabled = False
						ctlValue.Enabled = False

					Else
						nextLue.Enabled = True
						ctl.Enabled = True
						ctlValue.Enabled = True
					End If
					If Not nextLue.Enabled OrElse nextLue.EditValue Is Nothing OrElse String.IsNullOrWhiteSpace(nextLue.Text) Then ctlValue.Text = String.Empty

				Next
				LoadMinumumSalaryData()
				bbiSave.Enabled = True

				'If lue Is Nothing Then Return

				'lue.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap
				'lue.Properties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True

				''LoadMinumumSalaryData()

				'lue.Visible = True

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				bbiSave.Enabled = False

			Finally
				pnlCategoryValue.ResumeLayout()

			End Try

		End Sub

		'Select Case ctlIteral
		'	Case 0
		'		ctl = lblCat0
		'		ctlValue = lblCatValue0
		'		lue = lue0
		'		nextCatValue = lblCatValue1
		'		If lblCat1.Visible Then
		'			nextCtl = lblCat1
		'			nextLue = lue1
		'			If lue2.Visible Then
		'				afterNextLue = lue2
		'				afterNextCtl = lblCat2
		'				afterNextCatValue = lblCatValue2
		'			End If
		'		End If

		'	Case 1
		'		ctl = lblCat1
		'		ctlValue = lblCatValue1
		'		lue = lue1
		'		nextCatValue = lblCatValue2
		'		If lblCat2.Visible Then
		'			nextCtl = lblCat2
		'			nextLue = lue2
		'			If lue3.Visible Then
		'				afterNextLue = lue3
		'				afterNextCtl = lblCat3
		'				afterNextCatValue = lblCatValue3
		'			End If
		'		End If
		'	Case 2
		'		ctl = lblCat2
		'		ctlValue = lblCatValue2
		'		lue = lue2
		'		nextCatValue = lblCatValue3
		'		If lblCat3.Visible Then
		'			nextCtl = lblCat3
		'			nextLue = lue3
		'			If lue4.Visible Then
		'				afterNextLue = lue4
		'				afterNextCtl = lblCat4
		'				afterNextCatValue = lblCatValue4
		'			End If
		'		End If
		'	Case 3
		'		ctl = lblCat3
		'		ctlValue = lblCatValue3
		'		lue = lue3
		'		nextCatValue = lblCatValue4
		'		If lblCat4.Visible Then
		'			nextCtl = lblCat4
		'			nextLue = lue4
		'			If lue5.Visible Then
		'				afterNextLue = lue5
		'				afterNextCtl = lblCat5
		'				afterNextCatValue = lblCatValue5
		'			End If
		'		End If
		'	Case 4
		'		ctl = lblCat4
		'		ctlValue = lblCatValue4
		'		lue = lue4
		'		nextCatValue = lblCatValue5
		'		If lblCat5.Visible Then
		'			nextCtl = lblCat5
		'			nextLue = lue5
		'			If lue6.Visible Then
		'				afterNextLue = lue6
		'				afterNextCtl = lblCat6
		'				afterNextCatValue = lblCatValue6
		'			End If
		'		End If
		'	Case 5
		'		ctl = lblCat5
		'		ctlValue = lblCatValue5
		'		lue = lue5
		'		nextCatValue = lblCatValue6
		'		If lblCat6.Visible Then
		'			nextCtl = lblCat6
		'			nextLue = lue6
		'			If lue7.Visible Then
		'				afterNextLue = lue7
		'				afterNextCtl = lblCat7
		'				afterNextCatValue = lblCatValue7
		'			End If
		'		End If

		'	Case 6
		'		ctl = lblCat6
		'		ctlValue = lblCatValue6
		'		lue = lue6
		'		nextCatValue = lblCatValue7
		'		If lblCat7.Visible Then
		'			nextCtl = lblCat7
		'			nextLue = lue7
		'			If lue8.Visible Then
		'				afterNextLue = lue8
		'				afterNextCtl = lblCat8
		'				afterNextCatValue = lblCatValue8
		'			End If
		'		End If
		'	Case 7
		'		ctl = lblCat7
		'		ctlValue = lblCatValue7
		'		lue = lue7
		'		nextCatValue = lblCatValue8
		'		If lblCat8.Visible Then
		'			nextCtl = lblCat8
		'			nextLue = lue8
		'			If lue9.Visible Then
		'				afterNextLue = lue9
		'				afterNextCtl = lblCat9
		'				afterNextCatValue = lblCatValue9
		'			End If
		'		End If

		'	Case 8
		'		ctl = lblCat8
		'		ctlValue = lblCatValue8
		'		lue = lue8
		'		nextCatValue = lblCatValue9
		'		If lblCat9.Visible Then
		'			nextCtl = lblCat9
		'			nextLue = lue9
		'			If lue10.Visible Then
		'				afterNextLue = lue10
		'				afterNextCtl = lblCat10
		'				afterNextCatValue = lblCatValue10
		'			End If
		'		End If

		'	Case 9
		'		ctl = lblCat9
		'		ctlValue = lblCatValue9
		'		lue = lue9
		'		nextCatValue = lblCatValue10
		'		If lblCat10.Visible Then
		'			nextCtl = lblCat10
		'			nextLue = lue10
		'			If lue11.Visible Then
		'				afterNextLue = lue11
		'				afterNextCtl = lblCat11
		'				afterNextCatValue = lblCatValue11
		'			End If
		'		End If
		'	Case 10
		'		ctl = lblCat10
		'		ctlValue = lblCatValue10
		'		lue = lue10
		'		nextCatValue = lblCatValue11
		'		If lblCat11.Visible Then
		'			nextCtl = lblCat11
		'			nextLue = lue11
		'			If lue12.Visible Then
		'				afterNextLue = lue12
		'				afterNextCtl = lblCat12
		'				afterNextCatValue = lblCatValue12
		'			End If
		'		End If


		'	Case 11
		'		ctl = lblCat11
		'		ctlValue = lblCatValue11
		'		lue = lue11
		'		nextCatValue = lblCatValue12
		'		If lblCat12.Visible Then
		'			nextCtl = lblCat12
		'			nextLue = lue12
		'		End If

		'	Case 12
		'		ctl = lblCat12
		'		ctlValue = lblCatValue12
		'		lue = lue12


		'	Case Else
		'		nextCtl = Nothing
		'		nextLue = Nothing

		'		m_Logger.LogWarning(String.Format("more fields are not available!!!", ctlIteral))
		'		Return

		'End Select

		'If Not nextCtl Is Nothing AndAlso nextCtl.Visible Then
		'	m_SuppressUIEvents = True

		'	nextLue.EditValue = Nothing
		'	If Not afterNextLue Is Nothing Then
		'		afterNextLue.EditValue = Nothing
		'		afterNextCatValue.Text = String.Empty
		'	End If
		'	m_SuppressUIEvents = False

		'	nextLue.Properties.DataSource = Nothing
		'	If Not nextCatValue Is Nothing Then nextCatValue.Text = String.Empty

		'	Dim nextCtlID As Integer = Val(nextCtl.Text.Split("(")(1))

		'	m_Logger.LogDebug(String.Format("{0} >>>", nextCtl.Text))
		'	Dim nextlueData = LoadInputValuesForAssignedCriteriaStructureIDData(nextCtlID, m_ContractEditionNumber)

		'	If nextlueData Is Nothing OrElse (nextlueData.Count = 1 AndAlso nextlueData(0).Value Is Nothing) OrElse nextlueData.Count = 0 Then
		'		nextLue.Enabled = False
		'		nextLue.Properties.DataSource = nextlueData

		'		If ctlIteral + 1 = m_CantonID Then
		'			SetStandardValue(nextLue, nextCatValue, "Kanton", nextlueData)
		'		ElseIf ctlIteral + 1 = m_AlterID Then
		'			SetStandardValue(nextLue, nextCatValue, "Alter", nextlueData)
		'		ElseIf ctlIteral + 1 = m_YearID Then
		'			SetStandardValue(nextLue, nextCatValue, "Jahr", nextlueData)
		'		End If

		'		If Not afterNextLue Is Nothing Then
		'			nextCtlID = Val(afterNextCtl.Text.Split("(")(1))

		'			m_Logger.LogDebug(String.Format("{0} >>>", afterNextCtl.Text))
		'			nextlueData = LoadInputValuesForAssignedCriteriaStructureIDData(nextCtlID, m_ContractEditionNumber)

		'			If nextlueData Is Nothing OrElse (nextlueData.Count = 1 AndAlso nextlueData(0).Value Is Nothing) Then
		'				afterNextLue.Enabled = False
		'				afterNextLue.Properties.DataSource = nextlueData
		'				'LoadMinumumSalaryData()

		'			Else
		'				afterNextLue.Enabled = True
		'				afterNextLue.Properties.DataSource = nextlueData

		'				If ctlIteral + 2 = m_CantonID Then
		'					SetStandardValue(afterNextLue, afterNextCatValue, "Kanton", nextlueData)
		'				ElseIf ctlIteral + 2 = m_AlterID Then
		'					SetStandardValue(afterNextLue, afterNextCatValue, "Alter", nextlueData)
		'				ElseIf ctlIteral + 2 = m_YearID Then
		'					SetStandardValue(afterNextLue, afterNextCatValue, "Jahr", nextlueData)
		'				End If

		'			End If

		'		Else
		'			'LoadMinumumSalaryData()

		'		End If

		'	Else
		'		nextLue.Enabled = True
		'		nextLue.Properties.DataSource = nextlueData
		'		If ctlIteral + 1 = m_CantonID Then
		'			SetStandardValue(nextLue, nextCatValue, "Kanton", nextlueData)
		'		ElseIf ctlIteral + 1 = m_AlterID Then
		'			SetStandardValue(nextLue, nextCatValue, "Alter", nextlueData)
		'		ElseIf ctlIteral + 1 = m_YearID Then
		'			SetStandardValue(nextLue, nextCatValue, "Jahr", nextlueData)
		'		End If

		'		If Not String.IsNullOrWhiteSpace(nextLue.Text) Then nextCatValue.Text = nextLue.EditValue

		'		If Not afterNextLue Is Nothing Then
		'			nextCtlID = Val(afterNextCtl.Text.Split("(")(1))

		'			m_Logger.LogDebug(String.Format("{0} >>>", afterNextCtl.Text))
		'			nextlueData = LoadInputValuesForAssignedCriteriaStructureIDData(nextCtlID, m_ContractEditionNumber)

		'			afterNextLue.Enabled = Not nextlueData Is Nothing AndAlso nextlueData.Count > 0 'False
		'			afterNextLue.Properties.DataSource = nextlueData

		'			If afterNextLue.Enabled Then
		'				If ctlIteral + 2 = m_CantonID Then
		'					SetStandardValue(afterNextLue, afterNextCatValue, "Kanton", nextlueData)
		'				ElseIf ctlIteral + 2 = m_AlterID Then
		'					SetStandardValue(afterNextLue, afterNextCatValue, "Alter", nextlueData)
		'				ElseIf ctlIteral + 2 = m_YearID Then
		'					SetStandardValue(afterNextLue, afterNextCatValue, "Jahr", nextlueData)
		'				End If
		'			End If

		'		Else

		'		End If

		'	End If

		'Else
		'	'LoadMinumumSalaryData()

		'End If

		Private Sub SetStandardValue(ByVal lue As LookUpEdit, ByVal ctlvalue As LabelControl, ByVal catName As String, ByVal criteraData As List(Of InputValueData))

			Try
				Dim prevalidatedValue As New InputValueData
				'Dim strCategoryName As String = cat.Name

				If catName = "Alter" OrElse catName = "Età" Then
					m_SuppressUIEvents = True

					prevalidatedValue = criteraData.Where(Function(x) Val(x.Value) = m_EmployeeMasterData.EmployeeSUVABirthdateAge).FirstOrDefault
					If prevalidatedValue Is Nothing Then
						lue.EditValue = Nothing
						ctlvalue.Text = String.Empty
					Else
						lue.EditValue = prevalidatedValue.CriteriaListEntryID
						ctlvalue.Text = prevalidatedValue.CriteriaListEntryID

					End If

					m_SuppressUIEvents = False

				ElseIf catName = "Jahr" OrElse catName = "Anno" Then
					m_SuppressUIEvents = True

					prevalidatedValue = criteraData.Where(Function(x) Val(x.Value) = Now.Year).FirstOrDefault

					If prevalidatedValue Is Nothing Then
						lue.EditValue = Nothing
						ctlvalue.Text = String.Empty
					Else
						lue.EditValue = prevalidatedValue.CriteriaListEntryID
						ctlvalue.Text = prevalidatedValue.CriteriaListEntryID

					End If

					m_SuppressUIEvents = False

				ElseIf catName = "Kanton" OrElse catName = "Canton" Then
					m_SuppressUIEvents = True

					prevalidatedValue = criteraData.Where(Function(x) x.Value = CustomerCanton).FirstOrDefault
					If prevalidatedValue Is Nothing Then
						lue.EditValue = Nothing
						ctlvalue.Text = String.Empty
					Else
						lue.EditValue = prevalidatedValue.CriteriaListEntryID
						ctlvalue.Text = prevalidatedValue.CriteriaListEntryID

					End If

					m_SuppressUIEvents = False

				End If

			Catch ex As Exception
				lue.EditValue = Nothing
				ctlvalue.Text = String.Empty

				m_SuppressUIEvents = False

			End Try

		End Sub


#End Region

		Private Sub ReadPreviousCategoryValue(ByVal previousCatNumber As Integer)

			Dim dataCategory0 = SelectedAssignedPVLCategoryData(0)
			Dim dataCategory1 = SelectedAssignedPVLCategoryData(1)
			Dim dataCategory2 = SelectedAssignedPVLCategoryData(2)
			Dim dataCategory3 = SelectedAssignedPVLCategoryData(3)
			Dim dataCategory4 = SelectedAssignedPVLCategoryData(4)
			Dim dataCategory5 = SelectedAssignedPVLCategoryData(5)
			Dim dataCategory6 = SelectedAssignedPVLCategoryData(6)
			Dim dataCategory7 = SelectedAssignedPVLCategoryData(7)
			Dim dataCategory8 = SelectedAssignedPVLCategoryData(8)
			Dim dataCategory9 = SelectedAssignedPVLCategoryData(9)
			Dim dataCategory10 = SelectedAssignedPVLCategoryData(10)
			Dim dataCategory11 = SelectedAssignedPVLCategoryData(11)
			Dim dataCategory12 = SelectedAssignedPVLCategoryData(12)

			Dim liGAVCategoryValues As New BindingList(Of GAVCategoryValueDTO)

			Try
				Trace.WriteLine(String.Format("sender.name.ToString.ToLower: lue{0}", previousCatNumber))

				ReadSelectedCategoryValues()

			Catch ex As Exception

			End Try

		End Sub

		Private Sub ReadSelectedCategoryValues()

			lblCatValue0.Text = lue0.EditValue
			lblCatValue1.Text = lue1.EditValue
			lblCatValue2.Text = lue2.EditValue
			lblCatValue3.Text = lue3.EditValue
			lblCatValue4.Text = lue4.EditValue
			lblCatValue5.Text = lue5.EditValue
			lblCatValue6.Text = lue6.EditValue
			lblCatValue7.Text = lue7.EditValue
			lblCatValue8.Text = lue8.EditValue
			lblCatValue9.Text = lue9.EditValue
			lblCatValue10.Text = lue10.EditValue
			lblCatValue11.Text = lue11.EditValue
			lblCatValue12.Text = lue12.EditValue

			lblCatValue0.Visible = Not String.IsNullOrWhiteSpace(lblCatValue0.Text)
			lblCatValue1.Visible = Not String.IsNullOrWhiteSpace(lblCatValue1.Text)
			lblCatValue2.Visible = Not String.IsNullOrWhiteSpace(lblCatValue2.Text)
			lblCatValue3.Visible = Not String.IsNullOrWhiteSpace(lblCatValue3.Text)
			lblCatValue4.Visible = Not String.IsNullOrWhiteSpace(lblCatValue4.Text)
			lblCatValue5.Visible = Not String.IsNullOrWhiteSpace(lblCatValue5.Text)
			lblCatValue6.Visible = Not String.IsNullOrWhiteSpace(lblCatValue6.Text)
			lblCatValue7.Visible = Not String.IsNullOrWhiteSpace(lblCatValue7.Text)
			lblCatValue8.Visible = Not String.IsNullOrWhiteSpace(lblCatValue8.Text)
			lblCatValue9.Visible = Not String.IsNullOrWhiteSpace(lblCatValue9.Text)
			lblCatValue10.Visible = Not String.IsNullOrWhiteSpace(lblCatValue10.Text)
			lblCatValue11.Visible = Not String.IsNullOrWhiteSpace(lblCatValue11.Text)
			lblCatValue12.Visible = Not String.IsNullOrWhiteSpace(lblCatValue12.Text)

		End Sub


	End Class



End Namespace


