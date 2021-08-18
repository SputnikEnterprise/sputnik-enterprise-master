
Imports DevExpress.XtraRichEdit.Services
Imports DevExpress.XtraRichEdit
Imports DevExpress.XtraRichEdit.Commands
Imports DevExpress.Utils

'Module Utilities
'  Private logger As Logger = LogManager.GetCurrentClassLogger()

'  Dim _ClsReg As New SPProgUtility.ClsDivReg
'  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
'  Dim _clsLog As New SPProgUtility.ClsEventLog


'  'Public Sub ShowErrorDialog(ByVal text As String)
'  '  DevExpress.XtraEditors.XtraMessageBox.Show(text, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error)
'  'End Sub

'#Region "Funktionen zur Übersetzung..."

'  Function GetXMLValueByQuery(ByVal strFilename As String, _
'                              ByVal strQuery As String, _
'                              ByVal strValuebyNull As String) As String
'    Dim bResult As String = String.Empty
'    Dim strBez As String = _ClsReg.GetXMLNodeValue(strFilename, strQuery)

'    If strBez = String.Empty Then strBez = strValuebyNull

'    Return strBez
'  End Function

'#End Region

'End Module

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
      'MyBase.ExecuteCore()

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

    Public Function CreateCommand(ByVal id As RichEditCommandId) As RichEditCommand _
    Implements IRichEditCommandFactoryService.CreateCommand
      If id = RichEditCommandId.FileSave Then
        Return New CustomSaveDocumentCommand(control)
      ElseIf id = RichEditCommandId.QuickPrint Then
        Return New CustomPrintDocumentCommand(control)
      ElseIf id = RichEditCommandId.Print Then
        Return New CustomPrintDocumentCommand(control)

      End If

      Return service.CreateCommand(id)
    End Function

#End Region

  End Class

#End Region


End Namespace
