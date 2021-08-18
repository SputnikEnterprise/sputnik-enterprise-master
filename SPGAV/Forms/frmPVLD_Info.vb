Option Strict Off

Imports System.ComponentModel
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.UI
Imports SPGAV.SPPVLGAVUtilWebService
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Public Class frmPVLD_Info
	Inherits DevExpress.XtraEditors.XtraForm


#Region "Constants"

	Private Const DEFAULT_SPUTNIK_PVL_UTILITIES_WEBSERVICE_URI = "wsSPS_services/SPPVLGAVUtil.asmx" ' "http://asmx.domain.com/wsSPS_services/SPPVLGAVUtil.asmx"
	Private Const FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"

#End Region

	''' <summary>
	''' The Initialization data.
	''' </summary>
	Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	Private m_iMetaNr As Integer
	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private m_SPPVLUtilitiesServiceUrl As String

	Private m_path As ClsProgPath
	Private m_md As Mandant
	Private m_UtilityUI As UtilityUI
	Private m_PVLArchiveDbName As String


#Region "Constructor"

	Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

		' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		m_InitializationData = _setting
		m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(_setting.TranslationData, _setting.ProsonalizedData)
		m_md = New Mandant
		m_path = New ClsProgPath
		m_UtilityUI = New UtilityUI

		m_PVLArchiveDbName = String.Empty
		Dim domainName As String = "http://asmx.domain.com"

		m_SPPVLUtilitiesServiceUrl = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_PVL_UTILITIES_WEBSERVICE_URI)


		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

	End Sub

#End Region


#Region "Form Eigenschaften..."

	Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
		Me.Close()
	End Sub

#End Region


	Public Sub GetGAV_Details(ByVal iMeta As Integer)
		Dim iTop As Integer = 10
		Dim iLeft As Integer = 10
		Dim i As Integer = 0

		m_iMetaNr = iMeta
		ClearGAVD_Fields()
		Dim liGAVCriterion = PerformPVLCriteriasWebservice(iMeta)
		If liGAVCriterion Is Nothing Then
			m_UtilityUI.ShowErrorDialog(m_Translate.GetSafeTranslationValue("Die Kriterien Daten konnten nicht geladen werden."))

			Return
		End If

		For Each criteria In liGAVCriterion
			'Dim aGAVValues As String() = liGAVCriterion(i).Split(CChar("¦"))
			Dim strIDCritrion As String = criteria.ID_Criterion ' aGAVValues(0)
			Dim strIDContract As String = criteria.ID_Contract '  aGAVValues(1)
			Dim strElementID As String = criteria.Element_ID '  aGAVValues(2)
			Dim strCriterionName As String = criteria.name_de '  aGAVValues(3)
			Dim strCriterionName_fr As String = criteria.name_fr '  aGAVValues(4)
			Dim strCriterionName_it As String = criteria.name_it '  aGAVValues(5)

			If (strCriterionName + strCriterionName_fr + strCriterionName_it).Trim.Length > 0 Then

				Dim ctl As New DevExpress.XtraEditors.LabelControl ' Label

				ctl.Location = New Point(iLeft, iTop)
				ctl.AutoSize = True
				ctl.Text = strCriterionName
				ctl.Cursor = Cursors.Hand

				ctl.Name = "lblCriterion_" & strIDCritrion
				ctl.Tag = strIDCritrion ' strCriterionValue
				ctl.ForeColor = Color.Blue
				ctl.BackColor = Color.Transparent
				ctl.Show()

				AddHandler ctl.Click, AddressOf ctl_click
				If i <= liGAVCriterion.Count / 2 Then
					Me.scD_1.Controls.Add(ctl)
				Else
					Me.scD_2.Controls.Add(ctl)
				End If
				ctl.Show()

				If i = liGAVCriterion.Count / 2 Then iTop = 10 Else iTop += 20
			End If

		Next

		Me.grpD_1.Visible = True
	End Sub

	Private Sub ctl_click(ByVal sender As System.Object, ByVal e As System.EventArgs)
		Dim iCriterionNr As Integer = sender.name.split("_")(1)

		ClearGAVD_InfoFields()
		Dim ctl As New Label
		If Val(iCriterionNr) = 0 Then Exit Sub

		Dim liGAVCriterionValue = PerformPVLCriteriasByIDWebservice(Val(iCriterionNr))

		ctl.Location = New Point(10, 10)
		'ctl.Anchor = AnchorStyles.Top
		ctl.AutoSize = True
		'ctl.Size = New Size(Me.scGAVBerufInfo.Width - 10, Me.scD_Info.Height - 10)
		ctl.TextAlign = ContentAlignment.TopLeft
		ctl.Text = sender.Text

		ctl.Name = "lblDCriterionHeader_"
		ctl.ForeColor = Color.Blue
		ctl.Font = New Font(ctl.Font, FontStyle.Bold)
		ctl.BackColor = Color.Transparent
		ctl.Show()

		Me.scD_Info.Controls.Add(ctl)
		ctl.Show()

		Try

			ctl = New Label
			ctl.Location = New Point(10, 25)
			ctl.AutoSize = False ' True
			ctl.Size = New Size(Me.scD_Info.Width + 10, Me.scD_Info.Height + 50)
			ctl.TextAlign = ContentAlignment.TopLeft
			Dim liCtlValue = PerformPVLCriteriasByIDWebservice(sender.tag.ToString)
			For Each cr In liCtlValue
				'Dim aCtlValue As String() = liCtlValue(i).Split("¦")
				Dim strTableValue As String = cr.txtTable.Replace("<th><![CDATA[", "").Replace("]]></th>", String.Empty)

				strTableValue = strTableValue.Replace("<td><![CDATA[", "").Replace("]]></td>", String.Empty)
				strTableValue = strTableValue.Replace("</tr>", String.Empty).Replace("<tr>", String.Empty)
				strTableValue = strTableValue.Replace("</tr><tr>", String.Empty)

				ctl.Text = ctl.Name & strTableValue & vbNewLine & cr.txtText ' aCtlValue(2) '.Replace("|", vbTab)

			Next

			ctl.Name = "lblDCriterion_"
			ctl.ForeColor = Color.Black
			ctl.BackColor = Color.Transparent
			ctl.Show()

			Me.scD_Info.Controls.Add(ctl)
			ctl.Show()

		Catch ex As Exception

		End Try

		btnCopyGAVD_Info.Visible = ctl.Text.Trim.Length > 0
		Me.btnCopyGAVD_Info.Top = Me.scD_Info.Top - 10
		Me.btnCopyGAVD_Info.Left = Me.scD_Info.Width - Me.btnCopyGAVD_Info.Width - 0
	End Sub

	Private Function PerformPVLCriteriasWebservice(ByVal id_meta As Integer) As BindingList(Of GAVCriteriasResultDTO)

		Dim listDataSource As BindingList(Of GAVCriteriasResultDTO) = New BindingList(Of GAVCriteriasResultDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetPVLCriteriasData(m_InitializationData.MDData.MDGuid, m_PVLArchiveDbName, id_meta, m_InitializationData.UserData.UserLanguage).ToList

		For Each result In searchResult

			Dim viewData = New GAVCriteriasResultDTO With {
						.ID_Criterion = result.ID_Criterion,
						.ID_Contract = result.ID_Contract,
						.Element_ID = result.Element_ID,
						.name_de = result.name_de,
						.name_fr = result.name_fr,
						.name_it = result.name_it
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	Private Function PerformPVLCriteriasByIDWebservice(ByVal criteriaID As Integer) As BindingList(Of GAVCriteriaValueResultDTO)

		Dim listDataSource As BindingList(Of GAVCriteriaValueResultDTO) = New BindingList(Of GAVCriteriaValueResultDTO)

		Dim webservice = New SPPVLGAVUtilWebService.SPPVLGAVUtilSoapClient
		webservice.Endpoint.Address = New System.ServiceModel.EndpointAddress(m_SPPVLUtilitiesServiceUrl)

		' Read data over webservice
		Dim searchResult = webservice.GetPVLCriteriaValuesByIDData(m_InitializationData.MDData.MDGuid, m_PVLArchiveDbName, criteriaID, m_InitializationData.UserData.UserLanguage).ToList

		For Each result In searchResult

			Dim viewData = New GAVCriteriaValueResultDTO With {
						.ID_Criterion = result.ID_Criterion,
						.ID_CriterionValue = result.ID_CriterionValue,
						.txtText = result.txtText,
						.txtTable = result.txtTable
			}

			listDataSource.Add(viewData)

		Next


		Return listDataSource

	End Function

	Sub ClearGAVD_Fields()

		Try
			Me.scD_1.Controls.Clear()
			Me.scD_2.Controls.Clear()
			ClearGAVD_InfoFields()

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "ClearGAVD_Fields")

		End Try

		Me.scD_1.Refresh()
		Me.scD_2.Refresh()
	End Sub

	Sub ClearGAVD_InfoFields()

		Try
			Me.scD_Info.Controls.Clear()

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "ClearGAVD_InfoFields")

		End Try

		Me.scD_Info.Refresh()
	End Sub

#Region "Sonstige Hilfsfunktionen..."

	Function GetControlbyName(ByVal ControlName As String) As Object
		Try

			For Each obj As Control In Me.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next

			For Each obj As Control In Me.scD_Info.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next
			For Each obj As Control In Me.scD_1.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next
			For Each obj As Control In Me.scD_2.Controls
				If obj.Name = ControlName Then
					Return obj
				End If
			Next

		Catch ex As Exception
			MsgBox(ex.Message, MsgBoxStyle.Critical, "GetControlbyName")

		End Try


		Return Nothing

	End Function

#End Region



	Private Sub scD_1_Click(sender As System.Object, e As System.EventArgs) Handles scD_1.Click

	End Sub
End Class