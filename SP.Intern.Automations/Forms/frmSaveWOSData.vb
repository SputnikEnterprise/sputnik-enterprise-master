
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports SP.Infrastructure.UI
Imports SP.Infrastructure.Logging

Imports System.Data.SqlClient
Imports System.IO
Imports System.Web
Imports System.Text
Imports System.Data


Namespace WOSUtilityUI


	Public Class frmSaveWOSData


#Region "Private Consts"

		Private Const DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPCustomerPaymentServices.asmx" ' "http://asmx.domain.com/wsSPS_services/SPCustomerPaymentServices.asmx"
		Private Const DEFAULT_SPUTNIK_IBAN_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPIBANUtil.asmx" ' "http://asmx.domain.com/wsSPS_services/SPIBANUtil.asmx"
		Private Const DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI As String = "wsSPS_services/SPNotification.asmx" ' "http://asmx.domain.com/wsSPS_services/SPNotification.asmx"
		Public Const DEFAULT_SPUTNIK_CUSTOMERWOS_WEBSERVICE_URI As String = "wsSPS_services/SPWOSCustomerUtilities.asmx" ' "http://asmx.domain.com/wsSPS_services/SPWOSCustomerUtilities.asmx"
		Private Const DEFAULT_SPUTNIK_EMPLOYEEWOS_WEBSERVICE_URI As String = "wsSPS_services/SPWOSEmployeeUtilities.asmx" ' "http://asmx.domain.com/wsSPS_services/SPWOSEmployeeUtilities.asmx"

		Private Const MANDANT_XML_SETTING_SPUTNIK_PAYMENT_UTIL_WEBSERVICE_URI As String = "MD_{0}/Interfaces/webservices/webservicepaymentservices"
		Private Const MANDANT_XML_SETTING_SPUTNIK_ECALL_URI As String = "https://soap.ecall.ch/eCall.asmx"

		Private Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERNAME As String = "MD_{0}/Mailing/faxusername"
		Private Const MANDANT_XML_SETTING_SPUTNIK_ECALL_USERPW As String = "MD_{0}/Mailing/faxuserpw"
		Private Const MANDANT_XML_SETTING_WOS_EMPLOYEE_GUID As String = "MD_{0}/Export/MA_SPUser_ID"
		Private Const MANDANT_XML_SETTING_WOS_CUSTOMER_GUID As String = "MD_{0}/Export/KD_SPUser_ID"

#End Region


		''' <summary>
		''' The logger.
		''' </summary>
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
		''' UI Utility functions.
		''' </summary>
		Private m_UtilityUI As UtilityUI

		Private m_PaymentUtilWebServiceUri As String

		''' <summary>
		''' Service Uri of Sputnik bank util webservice.
		''' </summary>
		Private m_CustomerWosUtilWebServiceUri As String

		''' <summary>
		''' Service Uri of Sputnik bank util webservice.
		''' </summary>
		Private m_EmployeeWosUtilWebServiceUri As String
		Private m_NotificationUtilWebServiceUri As String

		'''<summary>
		'''Service Uri of eCall webservice.
		'''</summary>
		Private m_eCallWebServiceUri As String


		Public Sub New(ByVal _setting As SP.Infrastructure.Initialization.InitializeClass)

			m_UtilityUI = New UtilityUI
			m_InitializationData = _setting

			' Dieser Aufruf ist für den Designer erforderlich.
			InitializeComponent()

			' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
			Dim domainName As String = m_InitializationData.MDData.WebserviceDomain ' "http://asmx.domain.com"

			m_NotificationUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_NOTIFICATION_UTIL_WEBSERVICE_URI)
			m_PaymentUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_BANK_UTIL_WEBSERVICE_URI)
			m_CustomerWosUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_CUSTOMERWOS_WEBSERVICE_URI)
			m_EmployeeWosUtilWebServiceUri = String.Format("{0}/{1}", domainName, DEFAULT_SPUTNIK_EMPLOYEEWOS_WEBSERVICE_URI)

		End Sub

		Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveData.Click

			Dim sSql As String = String.Empty
			Dim iIDNr As Integer = CInt(Val(Me.txtID.Text))

			Me.Label5.Text = String.Empty
			If iIDNr = 0 Then Exit Sub

			Dim Time_1 As Double = System.Environment.TickCount
			Dim Conn As New SqlConnection("Data Source=dbserver;Initial Catalog=SpContract;User ID=username;Password=password")
			Dim strFullFile4LOGO As String = Me.txtLOGO.Text
			Dim strFullFile4AGB As String = Me.txtAGB.Text

			Dim strFullFile4AGB_I As String = Me.txtAGB_I.Text
			Dim strFullFile4AGB_F As String = Me.txtAGB_F.Text
			Dim strFullFile4AGB_E As String = Me.txtAGB_E.Text

			Dim strFullFile4AGBFest As String = Me.txtAGBFest.Text
			Dim strFullFile4AGBFest_I As String = Me.txtAGBFest_I.Text
			Dim strFullFile4AGBFest_F As String = Me.txtAGBFest_F.Text
			Dim strFullFile4AGBFest_E As String = Me.txtAGBFest_E.Text

			Dim strFullFile4AGBSonst As String = Me.txtAGBSonst.Text
			Dim strFullFile4AGBSonst_I As String = Me.txtAGBSonst_I.Text
			Dim strFullFile4AGBSonst_F As String = Me.txtAGBSonst_F.Text
			Dim strFullFile4AGBSonst_E As String = Me.txtAGBSonst_E.Text

			Dim strFullFile4Rahmen As String = Me.txtRahmen.Text
			Dim strFullFile4Rahmen_I As String = Me.txtRahmen_I.Text
			Dim strFullFile4Rahmen_F As String = Me.txtRahmen_F.Text
			Dim strFullFile4Rahmen_E As String = Me.txtRahmen_E.Text

			If strFullFile4AGB_I = String.Empty Then strFullFile4AGB_I = strFullFile4AGB
			If strFullFile4AGB_F = String.Empty Then strFullFile4AGB_F = strFullFile4AGB
			If strFullFile4AGB_E = String.Empty Then strFullFile4AGB_E = strFullFile4AGB

			If strFullFile4AGBFest = String.Empty Then strFullFile4AGBFest = strFullFile4AGB
			If strFullFile4AGBFest_I = String.Empty Then strFullFile4AGBFest_I = strFullFile4AGB
			If strFullFile4AGBFest_F = String.Empty Then strFullFile4AGBFest_F = strFullFile4AGB
			If strFullFile4AGBFest_E = String.Empty Then strFullFile4AGBFest_E = strFullFile4AGB

			If strFullFile4AGBSonst = String.Empty Then strFullFile4AGBSonst = strFullFile4AGB
			If strFullFile4AGBSonst_I = String.Empty Then strFullFile4AGBSonst_I = strFullFile4AGB
			If strFullFile4AGBSonst_F = String.Empty Then strFullFile4AGBSonst_F = strFullFile4AGB
			If strFullFile4AGBSonst_E = String.Empty Then strFullFile4AGBSonst_E = strFullFile4AGB

			If strFullFile4Rahmen = String.Empty Then strFullFile4Rahmen = strFullFile4AGB
			If strFullFile4Rahmen_I = String.Empty Then strFullFile4Rahmen_I = strFullFile4Rahmen
			If strFullFile4Rahmen_F = String.Empty Then strFullFile4Rahmen_F = strFullFile4Rahmen
			If strFullFile4Rahmen_E = String.Empty Then strFullFile4Rahmen_E = strFullFile4Rahmen

			sSql = String.Format("Update MySetting Set Customer_Logo = @BinaryLogo, ")

			sSql &= "Customer_AGB = @BinaryAGB, Customer_AGB_I = @BinaryAGB_I, Customer_AGB_F = @BinaryAGB_F, Customer_AGB_E = @BinaryAGB_E, "
			sSql &= "Customer_AGBFest = @BinaryAGBFest, Customer_AGBFest_I = @BinaryAGBFest_I, Customer_AGBFest_F = @BinaryAGBFest_F, "
			sSql &= "Customer_AGBFest_E = @BinaryAGBFest_E, "

			sSql &= "Customer_AGBSonst = @BinaryAGBSonst, Customer_AGBSonst_I = @BinaryAGBSonst_I, Customer_AGBSonst_F = @BinaryAGBSonst_F, "
			sSql &= "Customer_AGBSonst_E = @BinaryAGBSonst_E, "

			sSql &= "Rahmenvertrag = @BinaryRahmen, Rahmenvertrag_I = @BinaryRahmen_I, Rahmenvertrag_F = @BinaryRahmen_F, "
			sSql &= "Rahmenvertrag_E = @BinaryRahmen_E "

			sSql &= "Where ID = @IDNr"

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
			Dim param As System.Data.SqlClient.SqlParameter

			Try
				Conn.Open()
				cmd.Connection = Conn

				If strFullFile4LOGO <> String.Empty And strFullFile4AGB <> String.Empty Then
					Dim myFile_Logo() As Byte = GetFileToByte(strFullFile4LOGO)
					Dim myFile_AGB() As Byte = GetFileToByte(strFullFile4AGB)
					Dim myFile_AGB_I() As Byte = If(strFullFile4AGB_I <> "", GetFileToByte(strFullFile4AGB_I), Nothing)
					Dim myFile_AGB_F() As Byte = If(strFullFile4AGB_F <> "", GetFileToByte(strFullFile4AGB_F), Nothing)
					Dim myFile_AGB_E() As Byte = If(strFullFile4AGB_E <> "", GetFileToByte(strFullFile4AGB_E), Nothing)

					Dim myFile_AGBFest() As Byte = If(strFullFile4AGBFest <> "", GetFileToByte(strFullFile4AGBFest), Nothing)
					Dim myFile_AGBFest_I() As Byte = If(strFullFile4AGBFest_I <> "", GetFileToByte(strFullFile4AGBFest_I), Nothing)
					Dim myFile_AGBFest_F() As Byte = If(strFullFile4AGBFest_F <> "", GetFileToByte(strFullFile4AGBFest_F), Nothing)
					Dim myFile_AGBFest_E() As Byte = If(strFullFile4AGBFest_E <> "", GetFileToByte(strFullFile4AGBFest_E), Nothing)

					Dim myFile_AGBSonst() As Byte = If(strFullFile4AGBSonst <> "", GetFileToByte(strFullFile4AGBSonst), Nothing)
					Dim myFile_AGBSonst_I() As Byte = If(strFullFile4AGBSonst_I <> "", GetFileToByte(strFullFile4AGBSonst_I), Nothing)
					Dim myFile_AGBSonst_F() As Byte = If(strFullFile4AGBSonst_F <> "", GetFileToByte(strFullFile4AGBSonst_F), Nothing)
					Dim myFile_AGBSonst_E() As Byte = If(strFullFile4AGBSonst_E <> "", GetFileToByte(strFullFile4AGBSonst_E), Nothing)

					Dim myFile_Rahmen() As Byte = If(strFullFile4Rahmen <> "", GetFileToByte(strFullFile4Rahmen), Nothing)
					Dim myFile_Rahmen_I() As Byte = If(strFullFile4Rahmen_I <> "", GetFileToByte(strFullFile4Rahmen_I), Nothing)
					Dim myFile_Rahmen_F() As Byte = If(strFullFile4Rahmen_F <> "", GetFileToByte(strFullFile4Rahmen_F), Nothing)
					Dim myFile_Rahmen_E() As Byte = If(strFullFile4Rahmen_E <> "", GetFileToByte(strFullFile4Rahmen_E), Nothing)

					Try
						cmd.CommandType = CommandType.Text
						cmd.CommandText = sSql

						param = cmd.Parameters.AddWithValue("@BinaryLogo", myFile_Logo)
						param = cmd.Parameters.AddWithValue("@BinaryAGB", myFile_AGB)
						param = cmd.Parameters.AddWithValue("@BinaryAGB_I", myFile_AGB_I)
						param = cmd.Parameters.AddWithValue("@BinaryAGB_F", myFile_AGB_F)
						param = cmd.Parameters.AddWithValue("@BinaryAGB_E", myFile_AGB_E)

						param = cmd.Parameters.AddWithValue("@BinaryAGBFest", myFile_AGBFest)
						param = cmd.Parameters.AddWithValue("@BinaryAGBFest_I", myFile_AGBFest_I)
						param = cmd.Parameters.AddWithValue("@BinaryAGBFest_F", myFile_AGBFest_F)
						param = cmd.Parameters.AddWithValue("@BinaryAGBFest_E", myFile_AGBFest_E)

						param = cmd.Parameters.AddWithValue("@BinaryAGBSonst", myFile_AGBSonst)
						param = cmd.Parameters.AddWithValue("@BinaryAGBSonst_I", myFile_AGBSonst_I)
						param = cmd.Parameters.AddWithValue("@BinaryAGBSonst_F", myFile_AGBSonst_F)
						param = cmd.Parameters.AddWithValue("@BinaryAGBSonst_E", myFile_AGBSonst_E)

						param = cmd.Parameters.AddWithValue("@BinaryRahmen", myFile_Rahmen)
						param = cmd.Parameters.AddWithValue("@BinaryRahmen_I", myFile_Rahmen_I)
						param = cmd.Parameters.AddWithValue("@BinaryRahmen_F", myFile_Rahmen_F)
						param = cmd.Parameters.AddWithValue("@BinaryRahmen_E", myFile_Rahmen_E)

						param = cmd.Parameters.AddWithValue("@IDNr", iIDNr)

						cmd.Connection = Conn
						cmd.ExecuteNonQuery()

						cmd.Parameters.Clear()
						Me.Label5.Text = "Erfolgreich..."

					Catch ex As Exception
						Me.Label5.Text = ex.Message

					End Try

				End If


			Catch ex As Exception
				Me.Label5.Text = ex.Message

			End Try

			Dim Time_2 As Double = System.Environment.TickCount
			Console.WriteLine("Zeit für SaveFileIntoDb: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")

		End Sub

		Function GetFileToByte(ByVal filePath As String) As Byte()
			Dim stream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
			Dim reader As BinaryReader = New BinaryReader(stream)

			Dim photo() As Byte
			Try

				photo = reader.ReadBytes(CInt(stream.Length))
				reader.Close()
				stream.Close()

			Catch ex As Exception

			End Try

			Return photo
		End Function

		Private Sub btnCalculateJobApp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalculateJobApp.Click
			Dim paramStream As ListDictionary = New ListDictionary

			paramStream.Add("MandantGuid", Me.txtJobMDGuid.Text)
			paramStream.Add("Sprache", Me.txtJobLang.Text) ' Or “DE”, “FR”, “IT”
			paramStream.Add("InfoUrl", Me.txtJobUrlLink.Text)

			Dim test As String = SerializeParameterObject(paramStream) ' (paramStream)
			Me.txtJobLink.Text = test


		End Sub

		Public Shared Function SerializeParameterObject(ByVal obj As Object) As String
			Dim binaryFormatter = New Runtime.Serialization.Formatters.Binary.BinaryFormatter()
			Dim stream = New MemoryStream()

			binaryFormatter.Serialize(stream, obj)
			stream.Position = 0
			Dim byteArray = stream.ToArray()

			Dim base64String = Convert.ToBase64String(byteArray)

		End Function



	End Class

End Namespace
