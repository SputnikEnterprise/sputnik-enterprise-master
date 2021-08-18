Public Class Form1

  Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

    Dim ostJobCHUploader As New OstJobCHVacancyUploader(New InitializeClass With {.MDData = Nothing, .UserData = Nothing, .ProsonalizedData = Nothing, .TranslationData = Nothing})

    ostJobCHUploader.UploadVacancies("14", "DE699163-B76A-4FCD-895D-6326ECB1198F", 14)

  End Sub
End Class
