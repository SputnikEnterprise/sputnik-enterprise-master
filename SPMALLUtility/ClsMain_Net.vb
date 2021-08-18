

Public Class ClsMain_Net

	Private m_init As SP.Infrastructure.Initialization.InitializeClass


  Protected Overrides Sub Finalize()
    MyBase.Finalize()
  End Sub

  Public Sub New()

		m_init = CreateInitialData(0, 0)
    Application.EnableVisualStyles()

  End Sub

  Sub Showfrmll(ByVal iMANr As Long)

		'm_init = CreateInitialData(0, 0)

		ClsDataDetail.GetMANumber = iMANr

		Dim frmLL = New RibbonForm1(m_init)
		frmLL.EmployeeNumber = iMANr
		frmLL.LoadCVTemplate()

		frmLL.Show()
		frmLL.BringToFront()

	End Sub

	Sub ShowfrmllWithTemplate(ByVal iMANr As Long, ByVal llName As String)

		'm_init = CreateInitialData(0, 0)
		ClsDataDetail.GetMANumber = iMANr

		Dim frmLL = New RibbonForm1(m_init) ', llName)
		frmLL.EmployeeNumber = iMANr
		frmLL.CVTemplateLabel = llName
		frmLL.LoadCVTemplate()

		frmLL.Show()
		frmLL.BringToFront()

	End Sub


#Region "Helpers"

	Private Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

		Dim m_md As New SPProgUtility.Mandanten.Mandant
		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
		Dim translate = clsTransalation.GetTranslationInObject

		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

	End Function

#End Region


End Class
