
Imports System.IO.File
Imports System.Threading
Imports System.Windows.Forms

Public Class ClsMain_Net

  Implements IDisposable
  Protected disposed As Boolean = False

  Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
  Private ProposeMailSetting As New ClsMailSetting


  Sub ShowfrmProposalMail()

    ClsDataDetail.GetProposalNr = ProposeMailSetting.ProposeNr2Send

    Dim frmMail = New frmPMail(ProposeMailSetting)

    frmMail.Show()

  End Sub

  Public Sub New(ByVal _Proposesetting As ClsMailSetting, ByVal _setting As ClsSetting)

    ProposeMailSetting = _Proposesetting
    ClsDataDetail.ProgSettingData = _setting

    ClsDataDetail.MDData = ClsDataDetail.SelectedMDData(0)
    ClsDataDetail.UserData = ClsDataDetail.LogededUSData(0, 0)
    If _setting.SelectedMDNr = 0 Then ClsDataDetail.ProgSettingData.SelectedMDNr = ClsDataDetail.MDData.MDNr
    ClsDataDetail.GetSelectedMDConnstring = ClsDataDetail.MDData.MDDbConn

    If _setting.LogedUSNr = 0 Then ClsDataDetail.ProgSettingData.LogedUSNr = ClsDataDetail.UserData.UserNr

    If ClsDataDetail.ProgSettingData.TranslationItems Is Nothing Then
      ClsDataDetail.ProsonalizedData = ClsDataDetail.ProsonalizedName
      ClsDataDetail.TranslationData = ClsDataDetail.Translation
    Else
      ClsDataDetail.TranslationData = ClsDataDetail.ProgSettingData.TranslationItems
    End If

    Application.EnableVisualStyles()

  End Sub


  Protected Overridable Overloads Sub Dispose( _
   ByVal disposing As Boolean)
    If Not Me.disposed Then
      If disposing Then

      End If
      ' Add code here to release the unmanaged resource.
      'LL.Dispose()
      'LL.Core.Dispose()
      ' Note that this is not thread safe.
    End If
    Me.disposed = True
  End Sub

#Region " IDisposable Support "
  ' Do not change or add Overridable to these methods.
  ' Put cleanup code in Dispose(ByVal disposing As Boolean).
  Public Overloads Sub Dispose() Implements IDisposable.Dispose
    Dispose(True)
    GC.SuppressFinalize(Me)
  End Sub
  Protected Overrides Sub Finalize()
    Dispose(False)
    MyBase.Finalize()
  End Sub
#End Region


End Class
