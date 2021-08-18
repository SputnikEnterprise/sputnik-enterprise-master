Public Class Form1

  Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

    Dim jobCHUploader As New JobCHVacancyUploader(New InitializeClass With {.MDData = Nothing, .UserData = Nothing, .ProsonalizedData = Nothing, .TranslationData = Nothing})

		jobCHUploader.UploadVacancies("userguid", "mdguid", 0)


	End Sub


End Class
