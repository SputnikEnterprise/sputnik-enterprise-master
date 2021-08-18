
Imports System.Reflection.Assembly
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Threading
Imports SPProgUtility.SPUserSec.ClsUserSec
Imports SPProgUtility.SPTranslation.ClsTranslation
Imports SPProgUtility.Mandanten

Imports DevExpress.XtraEditors.Controls
Imports DevExpress.LookAndFeel
Imports SP.Infrastructure.Logging

Public Class frmLibraryInfo


	Inherits DevExpress.XtraEditors.XtraForm
	''' <summary>
	''' The logger.
	''' </summary>
	Private m_Logger As ILogger = New Logger()

	Private m_xml As New ClsXML


#Region "Constructor..."

	Public Sub New()

		' Dieser Aufruf ist für den Designer erforderlich.
		DevExpress.UserSkins.BonusSkins.Register()
		DevExpress.Skins.SkinManager.EnableFormSkins()

		InitializeComponent()

		' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
		ResetGrid()
		StartTranslation()

	End Sub

#End Region


	Sub StartTranslation()
		Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name

		Try
			Me.Text = m_xml.GetSafeTranslationValue(Me.Text)

		Catch ex As Exception
			m_Logger.LogError(String.Format("1=>{0}.{1}", strMethodeName, ex.Message))
		End Try

	End Sub

	Private Sub ResetGrid()

		' Create Columns
		gvRP.OptionsView.ShowIndicator = False
		gvRP.OptionsView.ShowAutoFilterRow = True
		gvRP.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never

		gvRP.Columns.Clear()

		Dim columnFilename As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFilename.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFilename.Caption = m_xml.GetSafeTranslationValue("Filename")
		columnFilename.Name = "Filename"
		columnFilename.FieldName = "Filename"
		columnFilename.BestFit()
		columnFilename.Visible = True
		columnFilename.Width = 100
		gvRP.Columns.Add(columnFilename)

		Dim columnFilelocation As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFilelocation.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFilelocation.Caption = m_xml.GetSafeTranslationValue("Filelocation")
		columnFilelocation.Name = "Filelocation"
		columnFilelocation.FieldName = "Filelocation"
		columnFilelocation.Visible = True
		columnFilelocation.Width = 300
		gvRP.Columns.Add(columnFilelocation)

		Dim columnFileCreatedon As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileCreatedon.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFileCreatedon.Caption = m_xml.GetSafeTranslationValue("Erstellt am")
		columnFileCreatedon.Name = "FileCreatedon"
		columnFileCreatedon.FieldName = "FileCreatedon"
		columnFileCreatedon.Visible = True
		columnFileCreatedon.Width = 30
		gvRP.Columns.Add(columnFileCreatedon)

		Dim columnFileVersion As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileVersion.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains
		columnFileVersion.Caption = m_xml.GetSafeTranslationValue("Version")
		columnFileVersion.Name = "FileVersion"
		columnFileVersion.FieldName = "FileVersion"
		columnFileVersion.Visible = True
		columnFileVersion.Width = 20
		gvRP.Columns.Add(columnFileVersion)

		Dim columnFileProcessArchitecture As New DevExpress.XtraGrid.Columns.GridColumn()
		columnFileProcessArchitecture.Caption = m_xml.GetSafeTranslationValue("ProcessArchitecture")
		columnFileProcessArchitecture.Name = "FileProcessArchitecture"
		columnFileProcessArchitecture.FieldName = "FileProcessArchitecture"
		columnFileProcessArchitecture.Visible = True
		columnFileProcessArchitecture.Width = 20
		gvRP.Columns.Add(columnFileProcessArchitecture)

		grdRP.DataSource = Nothing

	End Sub

	Public Function LoadAssemblyData() As Boolean

		Dim assembly As List(Of AssamlyInfo) = Nothing
		assembly = New List(Of AssamlyInfo)

		For Each a In AppDomain.CurrentDomain.GetAssemblies()

			Dim data = New AssamlyInfo

			data.Filename = a.GetName.Name
			data.Filelocation = a.Location
			data.FileVersion = a.GetName.Version.ToString
			data.FileProcessArchitecture = a.GetName.ProcessorArchitecture.ToString

			Dim fileinfo As System.IO.FileInfo = New System.IO.FileInfo(a.Location)
			Dim lastModified As DateTime = fileinfo.CreationTime

			data.FileCreatedon = lastModified

			assembly.Add(data)

		Next
		grdRP.DataSource = assembly

		Return (assembly Is Nothing)

	End Function

	Private Sub OnfrmLoad(sender As Object, e As System.EventArgs) Handles Me.Load

		Try
			If My.Settings.frmLibraryInfo_Location <> String.Empty Then
				Me.Width = Math.Max(My.Settings.ifrmLibraryInfoWidth, Me.Width)
				Me.Height = Math.Max(My.Settings.ifrmLibraryInfoHeight, Me.Height)
				Dim aLoc As String() = My.Settings.frmLibraryInfo_Location.Split(CChar(";"))

				If Screen.AllScreens.Length = 1 Then
					If Val(aLoc(0)) < 0 Or Val(aLoc(0)) > Screen.AllScreens(0).WorkingArea.Width Then aLoc(0) = CStr(0)
				End If
				Me.Location = New System.Drawing.Point(CInt(Math.Max(Val(aLoc(0)), 0)), CInt(Math.Max(Val(aLoc(1)), 0)))
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("{0}", ex.ToString()))

		End Try

	End Sub

	Private Sub OnfrmDisposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed

		Try
			If Not Me.WindowState = FormWindowState.Minimized Then
				My.Settings.frmLibraryInfo_Location = String.Format("{0};{1}", Me.Location.X, Me.Location.Y)
				My.Settings.ifrmLibraryInfoWidth = Me.Width
				My.Settings.ifrmLibraryInfoHeight = Me.Height

				My.Settings.Save()
			End If

		Catch ex As Exception
			' keine Fehlermeldung! ist es nicht wichtig wegen Berechtigungen...
		End Try

	End Sub



End Class