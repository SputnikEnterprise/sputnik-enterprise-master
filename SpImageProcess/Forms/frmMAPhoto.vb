
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data

Public Class frmMAPhoto

	Private _ClsProgSetting As New SPProgUtility.ClsProgSettingPath
	Private reportDBPersister As IClsDbRegister

	Private strFilename As String = String.Empty
	Private iSelectedMANr As Integer = 0
	Private strKleinerFilename As String = String.Empty
	Private strDrehFilename As String = String.Empty
	Private strFilename2Save As String = String.Empty

	Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    Me.picWait.Visible = False
    If strFilename = String.Empty Then
      Me.picMA.Image = Me.picMA.ErrorImage
      Me.ribLblOrgSize.Text = String.Empty
    Else
      Dim oldBitmap As New Bitmap(strFilename)
      Me.picMA.Image = oldBitmap
      Me.ribLblOrgSize.Text = String.Format("{0} x {1} Pixel", oldBitmap.Width, oldBitmap.Height)

    End If

    Me.sImgResizer.Maximum = 100
    Me.sImgResizer.Minimum = 5
    Me.sImgResizer.Value = 100

  End Sub

  Public Sub New()

    ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

  End Sub

  Public Sub New(ByVal iMANr As Integer)

    ' Dieser Aufruf ist für den Windows Form-Designer erforderlich.
    InitializeComponent()

    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
    Me.reportDBPersister = New ClsDbFunc(iMANr)
    ClsDataDetail.strNewFileName = Me.reportDBPersister.StoreSelectedMAPhoto2FS()

    iSelectedMANr = iMANr
    strFilename = ClsDataDetail.strNewFileName

    strKleinerFilename = strFilename
    strDrehFilename = strFilename
    strFilename2Save = strFilename
    Me.ribLblFileName.Text = If(strFilename = String.Empty, String.Empty, System.IO.Path.GetFileName(strFilename))
    Me.ribLblOrgSize.Text = String.Empty
    Me.ribLblNewSize.Text = String.Empty

  End Sub

  Sub ResizeImage()
    If strFilename = String.Empty Then Exit Sub

    Dim dZoomfaktor As Double = Val(Me.sImgResizer.Value)
    If dZoomfaktor = 0 Then dZoomfaktor = 10
    Dim dWidth As Integer = 0
    Dim dHeight As Integer = 0
    Dim oldBitmap As New Bitmap(strFilename)

    Try

      Me.picMA.Image = oldBitmap
      dWidth = CInt(oldBitmap.Width * dZoomfaktor / 100)
      dHeight = CInt(oldBitmap.Height * dZoomfaktor / 100)
      If dWidth = 0 Or dHeight = 0 Then Exit Sub
      Dim newBitmap As New Bitmap(dWidth, dHeight)

      Try
        Dim g As Graphics
        g = Graphics.FromImage(newBitmap)
        g.DrawImage(oldBitmap, New Rectangle(0, 0, newBitmap.Width, newBitmap.Height))
        g.Dispose()
        oldBitmap.Dispose()

        strKleinerFilename = _ClsProgSetting.GetSpSBildFiles2DeletePath & "TempKleiner.JPG"
        strDrehFilename = strKleinerFilename

        newBitmap.Save(strKleinerFilename)
        Me.picMA.Image = newBitmap

        Me.ribLblNewSize.Text = String.Format("{0} x {1} Pixel", newBitmap.Width, newBitmap.Height)

      Catch ex As Exception
        MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical, "ResizeImage_1")

      End Try

    Catch ex As Exception
      MsgBox(ex.Message & vbNewLine & ex.StackTrace, MsgBoxStyle.Critical, "ResizeImage_0")

    End Try

  End Sub

  Sub RotateImage(ByVal strFilename As String, ByVal rDirection As RotateFlipType)
    Dim myBitmap As New Bitmap(strDrehFilename)

    myBitmap.RotateFlip(rDirection)
    strDrehFilename = _ClsProgSetting.GetSpSBildFiles2DeletePath & "TempDreh.JPG"
    myBitmap.Save(strDrehFilename)

    Me.picMA.Image = myBitmap

  End Sub

  Private Sub sImgResizer_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sImgResizer.ValueChanged
    Me.picWait.Visible = Not Me.picWait.Visible
    ResizeImage()
    Me.sImgResizer.Text = String.Format("{0} %", Me.sImgResizer.Value)
    Me.picWait.Visible = Not Me.picWait.Visible
  End Sub

  Private Sub ribBtnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ribBtnOpen.Click
    Dim DlgOpen As New OpenFileDialog

    DlgOpen.FileName = ""
    DlgOpen.ShowDialog()
    If DlgOpen.FileName = "" Then
      Return
    End If

    Try
      Me.picMA.Image.Dispose()

    Catch ex As Exception

    End Try

    strFilename = DlgOpen.FileName

    strKleinerFilename = strFilename
    strDrehFilename = strFilename
    strFilename2Save = strFilename

    Try

      Dim myBitmap As New Bitmap(strFilename)
      Me.sImgResizer.Value = 100
      Me.picMA.Image = myBitmap

      Me.ribLblFileName.Text = System.IO.Path.GetFileName(strFilename)
      Me.ribLblOrgSize.Text = String.Format("{0} x {1} Pixel", myBitmap.Width, myBitmap.Height)
      Me.ribLblNewSize.Text = String.Format("{0} x {1} Pixel", myBitmap.Width, myBitmap.Height)

    Catch ex As Exception
      Try
        Me.picMA.Image.Dispose()
        Me.picMA.Image = Me.picMA.ErrorImage

      Catch ex_0 As Exception

      End Try
    End Try

  End Sub

  Private Sub btnVor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVor.Click

    Me.picWait.Visible = Not Me.picWait.Visible
    RotateImage(strFilename, RotateFlipType.RotateNoneFlipX)
    Me.picWait.Visible = Not Me.picWait.Visible

  End Sub

  Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click

    Me.picWait.Visible = Not Me.picWait.Visible
    RotateImage(strFilename, RotateFlipType.Rotate90FlipNone)
    Me.picWait.Visible = Not Me.picWait.Visible

  End Sub

  Private Sub ribBtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ribBtnSave.Click
    'Dim _ClsDbFunc As New ClsDbFunc

    Me.reportDBPersister.SaveFileIntoDb(strDrehFilename, Me.picMA.Image)
    ClsDataDetail.strNewFileName = strDrehFilename

  End Sub

  Private Sub ribBtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ribBtnDelete.Click
    Dim Result As MsgBoxResult
    Result = MsgBox("Hiermit löschen Sie das Bild aus der Datenbank endgültig. Sind Sie sicher?", _
                   CType(MsgBoxStyle.Question + MsgBoxStyle.YesNo, MsgBoxStyle), "Bild löschen")


    If Result = MsgBoxResult.Yes Then
      Me.picMA.Image.Dispose()
      Me.picMA.Image = Me.picMA.ErrorImage

      Me.reportDBPersister.DeleteImageFromDb()

      Me.ribLblFileName.Text = String.Empty
      Me.ribLblOrgSize.Text = String.Empty
      Me.ribLblNewSize.Text = String.Empty

    End If

  End Sub

  Private Sub picMA_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles picMA.Disposed
  End Sub


End Class
