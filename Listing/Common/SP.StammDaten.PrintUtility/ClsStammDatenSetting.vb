
Public Class ClsStammDatenSetting

  Public Property SearchMANr As List(Of Integer)
  Public Property SearchKDNr As List(Of Integer)
  Public Property SearchVakNr As List(Of Integer)
  Public Property SearchProposeNr As List(Of Integer)

  Public Property PrintAsExport As Boolean

  Public Property SelectedDocArt As DocArt
  Enum DocArt
    Kandidat = 0
    Kunde = 1
    Vakanz = 2
    Vorschlag = 3
  End Enum

End Class
