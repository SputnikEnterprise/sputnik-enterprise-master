
Imports System.ComponentModel
Imports System.Reflection
Imports DevExpress.LookAndFeel
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository
Imports SP.DatabaseAccess.Common
Imports SP.DatabaseAccess.Customer
Imports SP.DatabaseAccess.Customer.DataObjects
Imports SP.DatabaseAccess.Employee
Imports SP.DatabaseAccess.Employee.DataObjects.ContactMng
Imports SP.DatabaseAccess.ES
Imports SP.Infrastructure.Logging
Imports SP.Infrastructure.Messaging
Imports SP.Infrastructure.Messaging.Messages
Imports SP.Infrastructure.UI
Imports SP.MA.KontaktMng.frmContacts
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten
Imports Traysoft.AddTapi


Namespace UI

	Public Class frmTelephonySetting



#Region "private fields"

		Private Shared m_Logger As ILogger = New Logger()

		''' <summary>
		''' The Initialization data.
		''' </summary>
		Private m_InitializationData As SP.Infrastructure.Initialization.InitializeClass

		''' <summary>
		''' The translation value helper.
		''' </summary>
		Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

		''' <summary>
		''' Utility functions.
		''' </summary>
		Private m_Utility As SP.Infrastructure.Utility

		''' <summary>
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		Private m_MandantData As Mandant
		Private m_utilitySP As Utilities


#End Region


#Region "constructor"

		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			' Dieser Aufruf ist für den Designer erforderlich.
			DevExpress.UserSkins.BonusSkins.Register()
			DevExpress.Skins.SkinManager.EnableFormSkins()

			m_InitializationData = _setting
			m_Translate = New SP.Infrastructure.Initialization.TranslateValuesHelper(m_InitializationData.TranslationData, m_InitializationData.ProsonalizedData)

			m_MandantData = New Mandant
			m_utilitySP = New Utilities
			m_UtilityUI = New UtilityUI


			InitializeComponent()

			Me.KeyPreview = True
			Dim strStyleName As String = m_MandantData.GetSelectedUILayoutName(m_InitializationData.MDData.MDNr, m_InitializationData.UserData.UserNr, String.Empty)
			If strStyleName <> String.Empty Then
				UserLookAndFeel.Default.SetSkinStyle(strStyleName)
			End If

			TranslateControls()
			Reset()


		End Sub

#End Region


		Sub TranslateControls()

			Me.Text = m_Translate.GetSafeTranslationValue(Me.Text)

			lblamtkennziffer.Text = m_Translate.GetSafeTranslationValue(lblamtkennziffer.Text)
			chkReplacePlusWithZero.Text = m_Translate.GetSafeTranslationValue(chkReplacePlusWithZero.Text)
			chkCreateAutoContact.Text = m_Translate.GetSafeTranslationValue(chkCreateAutoContact.Text)

			bbiSave.Caption = m_Translate.GetSafeTranslationValue(bbiSave.Caption)

		End Sub


#Region "reset"


		Private Sub Reset()

			txtAmtsziffer.EditValue = String.Empty
			chkReplacePlusWithZero.Checked = False

		End Sub


#End Region

		Private Sub OnfrmTelephonySetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load

			txtAmtsziffer.EditValue = My.Settings.AmtsZiffer
			chkReplacePlusWithZero.Checked = If(My.Settings.replacepluswithzero, CheckState.Checked, CheckState.Unchecked)
			chkCreateAutoContact.Checked = If(My.Settings.createautocontact, CheckState.Checked, CheckState.Unchecked)

		End Sub

		Private Sub OnbbiSave_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiSave.ItemClick

			If String.IsNullOrWhiteSpace(txtAmtsziffer.EditValue) Then My.Settings.AmtsZiffer = String.Empty Else My.Settings.AmtsZiffer = Val(txtAmtsziffer.EditValue)
			My.Settings.replacepluswithzero = chkReplacePlusWithZero.Checked
			My.Settings.createautocontact = chkCreateAutoContact.Checked

			My.Settings.Save()

			Me.Close()

		End Sub


	End Class

End Namespace
