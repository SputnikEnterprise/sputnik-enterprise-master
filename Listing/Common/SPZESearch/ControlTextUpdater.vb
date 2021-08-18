

Public Class ControlLVUpdater

  Public Sub New(ByVal controlToUpdate As ListView)
    Me._ControlToUpdate = controlToUpdate
  End Sub


  ' Der Verweis wird als generisches Steuerelement gespeichert, so dass
  ' diese Hilfsklasse in anderen Szenarien wiederverwendet werden kann.
  Private _ControlToUpdate As ListView
  Public ReadOnly Property ControlToUpdate() As ListView
    Get
      Return _ControlToUpdate
    End Get
  End Property

  ' Speichert den hinzuzufügenden Text.
  Private _NewText As String
  Public Sub AddText(ByVal strNewText As String)
    SyncLock Me
      Me._NewText = strNewText
      ControlToUpdate.Invoke(New MethodInvoker(AddressOf ThreadSafeAddText))
    End SyncLock
  End Sub

  Private Sub ThreadSafeAddText()
    Dim strLineResult As String()
    Dim strAllLines As String()
    Dim i As Integer = 0

    Me.ControlToUpdate.Text &= _NewText

    strAllLines = Split(_NewText, vbCrLf)       ' Zeilenweise
    i = Me.ControlToUpdate.Items.Count

    For i = 0 To strAllLines.Count
      strLineResult = Split(strAllLines(i), ";")
      If strLineResult(0).ToString.Trim = String.Empty Then Exit For

      Me.ControlToUpdate.Items.Add(strLineResult(0).ToString)
      Me.ControlToUpdate.Items(i).SubItems.Add(strLineResult(1).ToString) ' (rOPrec("KDNr").ToString)
      Me.ControlToUpdate.Items(i).SubItems.Add(strLineResult(2).ToString) 'rOPrec("R_Name1").ToString)
      Me.ControlToUpdate.Items(i).SubItems.Add(strLineResult(3).ToString) 'rOPrec("R_Name1").ToString)
      Me.ControlToUpdate.Items(i).SubItems.Add(strLineResult(4).ToString) 'rOPrec("R_Name1").ToString)
      Me.ControlToUpdate.Items(i).SubItems.Add(strLineResult(5).ToString) 'rOPrec("R_Name1").ToString)
      Me.ControlToUpdate.Items(i).SubItems.Add(strLineResult(6).ToString) 'rOPrec("R_Name1").ToString)

    Next

    'With Lv
    '  .Items.Add(rOPrec("RENr").ToString)
    '  .Items(i).SubItems.Add(rOPrec("KDNr").ToString)
    '  .Items(i).SubItems.Add(rOPrec("R_Name1").ToString)

    '  If Not IsDBNull(rOPrec("Fak_Dat")) Then
    '    .Items(i).SubItems.Add(Format(rOPrec("Fak_Dat"), "d"))
    '  Else
    '    .Items(i).SubItems.Add("")
    '  End If

    '  If Not IsDBNull(CDbl(rOPrec("BetragInk").ToString) * -1) Then
    '    .Items(i).SubItems.Add(Format(CDbl(rOPrec("BetragInk").ToString) * -1, "###,###,###,###,0.00"))
    '  Else
    '    .Items(i).SubItems.Add("")
    '  End If
    '  If Not IsDBNull(rOPrec("PrintedDate")) Then
    '    .Items(i).SubItems.Add(Format(rOPrec("PrintedDate"), "d"))
    '  Else
    '    .Items(i).SubItems.Add("")
    '  End If

    '  If Not IsDBNull(rOPrec("CreatedOn")) And Not IsDBNull(rOPrec("CreatedFrom")) Then
    '    .Items(i).SubItems.Add(Format(rOPrec("CreatedOn"), "d") & " " & rOPrec("CreatedFrom").ToString)
    '  Else
    '    .Items(i).SubItems.Add("")
    '  End If

    '  TotalBetrag += CDbl(rOPrec("BetragInk").ToString)

    'End With


  End Sub

  Public Sub ReplaceText(ByVal newText As String)
    SyncLock Me
      Me._NewText = newText
      ControlToUpdate.Invoke(New MethodInvoker(AddressOf ThreadSafeReplaceText))
    End SyncLock
  End Sub

  Private Sub ThreadSafeReplaceText()
    Me.ControlToUpdate.Text = _NewText
  End Sub
End Class


Public Class ControlTextUpdater

  ' Der Verweis wird als generisches Steuerelement gespeichert, so dass
  ' diese Hilfsklasse in anderen Szenarien wiederverwendet werden kann.
  Private _ControlToUpdate As Control
  Public ReadOnly Property ControlToUpdate() As Control
    Get
      Return _ControlToUpdate
    End Get
  End Property
  Public Sub New(ByVal controlToUpdate As Control)
    Me._ControlToUpdate = controlToUpdate
  End Sub
  ' Speichert den hinzuzufügenden Text.
  Private _NewText As String
  Public Sub AddText(ByVal newText As String)
    SyncLock Me
      Me._NewText = newText
      ControlToUpdate.Invoke(New MethodInvoker(AddressOf ThreadSafeAddText))
    End SyncLock
  End Sub
  Private Sub ThreadSafeAddText()
    Me.ControlToUpdate.Text &= _NewText
  End Sub
  Public Sub ReplaceText(ByVal newText As String)
    SyncLock Me
      Me._NewText = newText
      ControlToUpdate.Invoke(New MethodInvoker(AddressOf ThreadSafeReplaceText))
    End SyncLock
  End Sub
  Private Sub ThreadSafeReplaceText()
    Me.ControlToUpdate.Text = _NewText
  End Sub
End Class
