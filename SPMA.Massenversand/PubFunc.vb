
Imports System.Data.SqlClient
Imports System.IO

Imports DevExpress.XtraRichEdit.Commands
Imports System.Windows.Forms
Imports DevExpress.XtraRichEdit.Services
Imports DevExpress.XtraRichEdit
Imports DevExpress.Utils

Module PubFunc

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private _ClsLog As New SPProgUtility.ClsEventLog

#Region "Benutzerdaten einlesen..."

	Function GetUSSign(ByVal lUSNr As Integer) As String
    Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim strFullFilename As String = String.Empty
    Dim strFiles As String = String.Empty
    Dim BA As Byte()
    Dim sUSSql As String = "Select USSign, USNr From Benutzer US Where "
    sUSSql &= String.Format("USNr = {0} And USSign Is Not Null", lUSNr)

    Dim i As Integer = 0

    Conn.Open()
    Dim SQLCmd As SqlCommand = New SqlCommand(sUSSql, Conn)
    Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)

    Try

      strFullFilename = String.Format("{0}Bild_{1}.JPG", _ClsProgSetting.GetSpSBildFiles2DeletePath, _
                                       System.Guid.NewGuid.ToString())

      Try
        Try
          BA = CType(SQLCmd_1.ExecuteScalar, Byte())
          If BA Is Nothing Then Return String.Empty

        Catch ex As Exception
          Return String.Empty

        End Try

        Dim ArraySize As New Integer
        ArraySize = BA.GetUpperBound(0)

        If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
        Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
        fs.Write(BA, 0, ArraySize + 1)
        fs.Close()
        fs.Dispose()

        i += 1

      Catch ex As Exception
        _ClsLog.WriteToEventLog(String.Format("***GetUSSign: {0}", ex.Message))
        MsgBox(String.Format("Fehler: {0}", ex.Message), MsgBoxStyle.Critical, "GetUSSign")


      End Try


    Catch ex As Exception
      _ClsLog.WriteToEventLog(String.Format("***GetUSSign: {0}", ex.Message))

    End Try

    Return strFullFilename
  End Function

  Function GetUSBild(ByVal lUSNr As Integer) As String
    Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim strFullFilename As String = String.Empty
    Dim strFiles As String = String.Empty
    Dim BA As Byte()
    Dim sUSSql As String = "Select USBild, USNr From Benutzer US Where "
    sUSSql &= String.Format("USNr = {0} And USSign Is Not Null", lUSNr)

    Dim i As Integer = 0

    Conn.Open()
    Dim SQLCmd As SqlCommand = New SqlCommand(sUSSql, Conn)
    Dim SQLCmd_1 As SqlCommand = New SqlCommand(sUSSql, Conn)

    Try

      strFullFilename = String.Format("{0}Bild_{1}.JPG", _ClsProgSetting.GetSpSBildFiles2DeletePath, _
                                       System.Guid.NewGuid.ToString())

      Try
        Try
          BA = CType(SQLCmd_1.ExecuteScalar, Byte())
          If BA Is Nothing Then Return String.Empty

        Catch ex As Exception
          Return String.Empty

        End Try

        Dim ArraySize As New Integer
        ArraySize = BA.GetUpperBound(0)

        If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
        Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
        fs.Write(BA, 0, ArraySize + 1)
        fs.Close()
        fs.Dispose()

        i += 1

      Catch ex As Exception
        _ClsLog.WriteToEventLog(String.Format("***GetUSBild: {0}", ex.Message))
        MsgBox(String.Format("Fehler: {0}", ex.Message), MsgBoxStyle.Critical, "GetUSBild")


      End Try


    Catch ex As Exception
      _ClsLog.WriteToEventLog(String.Format("***GetUSBild: {0}", ex.Message))

    End Try

    Return strFullFilename
  End Function

#End Region

  Function GetMAData() As List(Of String)
    Dim liResult As New List(Of String)

    Try
      Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
      Conn.Open()

      Dim sSql As String = "Select Top 1 Nachname, Vorname From Mitarbeiter Where MANr = @MANr"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetMANumber)
      Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

      While rFoundedrec.Read
        liResult.Add(String.Format("{0} {1}", rFoundedrec("Vorname").ToString, rFoundedrec("Nachname").ToString))
      End While


    Catch ex As Exception

    End Try

    Return liResult
  End Function

  Function ListDBFieldsName() As List(Of String)
    Dim liResult As New List(Of String)

    Try
      Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
      Conn.Open()

      Dim sSql As String = "Select * From tab_LLZusatzFields Where ShowInMAVersand = 1 "
      sSql &= "Order By RecNr, Bezeichnung"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

      While rFoundedrec.Read
				liResult.Add(String.Format("{0}#{1}#{2}",
																	 rFoundedrec("GroupNr"),
																	 Trim(rFoundedrec("DBFieldName")),
																	 rFoundedrec("Bezeichnung").ToString))
			End While


    Catch ex As Exception

    End Try

    Return liResult
  End Function

	Function GetDbFieldValue(ByVal myDbFieldName As String) As String
		Dim strResult As String = String.Empty

		Try
			Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
			Conn.Open()

			Dim sSql As String = "If Not Exists(Select ID From MA_LL Where MANr = @MANr) "
			sSql &= "Insert Into MA_LL (MANr) Values (@MANr); "
			sSql &= "Select _{0} From MA_LL Where MANr = @MANr"

			sSql = String.Format(sSql, myDbFieldName)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
			cmd.CommandType = Data.CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetMANumber)
			Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

			While rFoundedrec.Read
				strResult = rFoundedrec("_" & myDbFieldName).ToString
			End While

		Catch ex As Exception
			strResult = String.Format("Fehler: {0}", ex.InnerException)

		End Try

		Return strResult
	End Function

	Function SaveDbFieldValue(ByVal myDbFieldName As String, ByVal strRtfValue As String, ByVal strStringValue As String) As String
    Dim strResult As String = String.Empty

    Try
      Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
      Conn.Open()

      Dim sSql As String = "Update MA_LL Set _{0} = @rtfText, {0} = @MyText Where MANr = @MANr"
      sSql = String.Format(sSql, myDbFieldName)

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@rtfText", strRtfValue)
      param = cmd.Parameters.AddWithValue("@MyText", strStringValue)
      param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetMANumber)

      cmd.ExecuteNonQuery()

    Catch ex As Exception
      strResult = String.Format("Fehler: {0}", ex.InnerException)

    End Try

    Return strResult
  End Function

  Function GetZivilBez(ByVal myBez As String) As String
    Dim strResult As String = myBez

    Try
      Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
      Conn.Open()

      Dim sSql As String = "Select Description "
      sSql &= "From TAB_Zivilstand Where GetFeld = @Bez"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@Bez", myBez)
      Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

      While rFoundedrec.Read
        strResult = rFoundedrec("Description").ToString
      End While

    Catch ex As Exception

    End Try

    Return strResult
  End Function

  Function GetKIInfo() As List(Of String)
    Dim liResult As New List(Of String)
    Dim liResult_1 As New List(Of String)
    Dim liResult_2 As New List(Of String)
    Dim liResult_3 As New List(Of String)
    Dim liResult_4 As New List(Of String)
    Dim liResult_5 As New List(Of String)

    Try
      Dim Conn As SqlConnection = New SqlConnection(ClsDataDetail.GetDbConnString)
      Conn.Open()

      Dim sSql As String = "Select * From MA_KIAddress Where MANr = @MANr"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@MANr", ClsDataDetail.GetMANumber)
      Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

      While rFoundedrec.Read
        liResult.Add(String.Format("{0}, {1}", rFoundedrec("Vorname").ToString, Year(rFoundedrec("GebDat").ToString)))
        liResult_1.Add(String.Format("{0}, {1} {2}", rFoundedrec("Vorname").ToString, Format(rFoundedrec("GebDat"), "d")))
        liResult_2.Add(String.Format("{0}", Year(rFoundedrec("GebDat").ToString)))

        liResult_3.Add(rFoundedrec("Vorname").ToString)
        liResult_4.Add(Format(rFoundedrec("GebDat"), "d"))
        liResult_5.Add(If(rFoundedrec("Geschlecht").ToString = "M", _
                        _ClsProgSetting.TranslateText("Sohn"), _
                        _ClsProgSetting.TranslateText("Tochter")))

      End While

    Catch ex As Exception

    End Try

    Return liResult
  End Function

  Function GetMAPicture(ByVal lMANr As Integer) As String
    Dim Conn As New SqlConnection(ClsDataDetail.GetDbConnString)
    Dim strFullFilename As String = String.Empty
    Dim strFiles As String = String.Empty
    Dim BA As Byte()
    Dim sMASql As String = "Select MABild, MANr From Mitarbeiter Where "
    sMASql &= String.Format("MANr = {0} And MABild Is Not Null", lMANr)

    Dim i As Integer = 0

    Conn.Open()
    Dim SQLCmd As SqlCommand = New SqlCommand(sMASql, Conn)
    Dim SQLCmd_1 As SqlCommand = New SqlCommand(sMASql, Conn)

    Try

      strFullFilename = String.Format("{0}Bild_{1}_{2}.JPG", _ClsProgSetting.GetSpSBildFiles2DeletePath, _
                                       ClsDataDetail.GetMANumber, System.Guid.NewGuid.ToString())

      Try
        Try
          BA = CType(SQLCmd_1.ExecuteScalar, Byte())
          If BA Is Nothing Then Return String.Empty

        Catch ex As Exception
          Return String.Empty

        End Try

        Dim ArraySize As New Integer
        ArraySize = BA.GetUpperBound(0)

        If File.Exists(strFullFilename) Then File.Delete(strFullFilename)
        Dim fs As New FileStream(strFullFilename, FileMode.CreateNew)
        fs.Write(BA, 0, ArraySize + 1)
        fs.Close()
        fs.Dispose()

        i += 1

      Catch ex As Exception
        _ClsLog.WriteToEventLog(String.Format("***GetMAPicture: {0}", ex.Message))
        MsgBox(String.Format("Fehler: {0}", ex.Message), MsgBoxStyle.Critical, "GetMAPicture")


      End Try


    Catch ex As Exception
      _ClsLog.WriteToEventLog(String.Format("***GetMAPicture: {0}", ex.Message))

    End Try

    Return strFullFilename
  End Function


End Module


Namespace CustomCommand

#Region "customsavecommand"

  Public Class CustomSaveDocumentCommand
    Inherits SaveDocumentCommand

    Public Sub New(ByVal control As IRichEditControl)
      MyBase.New(control)
    End Sub

    Protected Overrides Sub ExecuteCore()

      ' Nichts tun. Die Daten wurden bereits in der Datenbank gespeichert.

      'SaveLLText("Reserve0", ClsDataDetail.GetSelectedTemplateName, Control)
      'ClsDataDetail.bContentChanged = False

      'If Control.Document.Paragraphs.Count > 7 Then
      '  MyBase.ExecuteCore()
      'Else
      'MessageBox.Show("You should type at least 7 paragraphs" & ControlChars.CrLf & "  to be able to save the document.", _
      '                "Please be creative", _
      '                MessageBoxButtons.OK, _
      '                MessageBoxIcon.Information)
      'End If

    End Sub

  End Class

  Public Class CustomPrintDocumentCommand
    Inherits QuickPrintCommand

    Public Sub New(ByVal control As IRichEditControl)
      MyBase.New(control)
    End Sub

    Protected Overrides Sub ExecuteCore()

      'MyBase.ExecuteCore()
      'LoadOrgDoc(Control)
      'ClsDataDetail.bContentChanged = False

      'If Control.Document.Paragraphs.Count > 7 Then
      '  MyBase.ExecuteCore()
      'Else
      '  MessageBox.Show("You should type at least 7 paragraphs" & ControlChars.CrLf & "  to be able to save the document.", _
      '                  "Please be creative", _
      '                  MessageBoxButtons.OK, _
      '                  MessageBoxIcon.Information)
      'End If

    End Sub

  End Class

#End Region ' customsavecommand


#Region "#iricheditcommandfactoryservice"

  Public Class CustomRichEditCommandFactoryService
    Implements IRichEditCommandFactoryService
    Private ReadOnly service As IRichEditCommandFactoryService
    Private ReadOnly control As RichEditControl

    Public Sub New(ByVal control As RichEditControl, ByVal service As IRichEditCommandFactoryService)
      Guard.ArgumentNotNull(control, "control")
      Guard.ArgumentNotNull(service, "service")
      Me.control = control
      Me.service = service
    End Sub

#Region "IRichEditCommandFactoryService Members"

		Public Function CreateCommand(ByVal id As RichEditCommandId) As RichEditCommand Implements IRichEditCommandFactoryService.CreateCommand

			If id = RichEditCommandId.FileSave Then
				Return New CustomSaveDocumentCommand(control)
			ElseIf id = RichEditCommandId.QuickPrint Then
				Return New CustomPrintDocumentCommand(control)
			ElseIf id = RichEditCommandId.Print Then
				Return New CustomPrintDocumentCommand(control)

			End If
			Trace.WriteLine(control.Modified)

			Return service.CreateCommand(id)
		End Function

#End Region

	End Class

#End Region ' #iricheditcommandfactoryservice

End Namespace


Public Class LLTemplateData

	Public Property RecNr As Integer
	Public Property DbFieldName As String
	Public Property Bezeichnung As String

	Public Property FileName As String


End Class
