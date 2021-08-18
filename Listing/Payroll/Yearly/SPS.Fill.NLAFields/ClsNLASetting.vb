
Imports SP.DatabaseAccess.Listing
Imports SPProgUtility.Mandanten
Imports SPProgUtility.ProgPath

Public Class ClsNLASetting

	Public Property SelectedYear As Integer
	Public Property SQL2Open As String
	Public Property DbConn2Open As SqlClient.SqlConnection

	' Einträge aus der Mandantenverwaltung
	Public NLA_2_3 As String
	Public NLA_3_0 As String
	Public NLA_4_0 As String
	Public NLA_7_0 As String
	Public NLA_13_1_2 As String

	Public NLA_13_2_3 As String

	Public NLA_14_1_1 As String
	Public NLA_14_1_2 As String
	Public NLA_15_1_1 As String
	Public NLA_15_1_2 As String

	Public Property CreateJobsForExport As Boolean?
	Public Property Send2WOS As WOSSENDValue

	Public ReadOnly Property GetJobID(ByVal mdNr As Integer) As Short
		Get
			Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Lohnausweis_NLA"
			Dim m_path As New ClsProgPath
			Dim m_mandant As New Mandant
			Dim strXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(mdNr)

			Dim strKeyName As String = "Orientation".ToLower
			Dim strValue As String = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))
			If String.IsNullOrWhiteSpace(strValue) Then strValue = "2"


			Return CShort(Val(strValue))
		End Get
	End Property

	Public ReadOnly Property Get2021JobID(ByVal mdNr As Integer) As Boolean
		Get
			Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Lohnausweis_NLA"
			Dim m_path As New ClsProgPath
			Dim m_mandant As New Mandant
			Dim strXMLFile = m_mandant.GetSelectedMDFormDataXMLFilename(mdNr)

			Dim strKeyName As String = "version2021".ToLower
			Dim strValue As String = m_path.GetXMLNodeValue(strXMLFile, String.Format("{0}/{1}", FORM_XML_MAIN_KEY, strKeyName))
			If String.IsNullOrWhiteSpace(strValue) Then
				Return False
			Else
				Return CBool(strValue)
			End If


			Return strValue
		End Get
	End Property

End Class
