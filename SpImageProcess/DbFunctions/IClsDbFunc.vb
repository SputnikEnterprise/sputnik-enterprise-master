
Public Interface IClsDbRegister

  Function SaveFileIntoDb(strFileToSave As String, bild As Image) As String


  Function StoreSelectedMAPhoto2FS() As String
  Function DeleteImageFromDb() As String
  Function GetFileToByte(ByVal filePath As String) As Byte()

End Interface


Public Class DBInformation

#Region "Properties"

  Public Property iCandidatNr As Integer

#End Region

End Class
