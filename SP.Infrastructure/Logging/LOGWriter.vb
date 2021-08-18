
Imports System.IO

Namespace LOGWriter

	Public Class LOGWriter

		Public Property WriteDateTime As Boolean
		Public Property UserFullName As String
		Public Property CurrentLogFileName As String


		Public ReadOnly Property GetCurrentLogFilename As String
			Get
				Return CurrentLogFileName
			End Get
		End Property


		Public Sub New()

		End Sub

		Public Sub WriteTempLogFile(ByVal msg As String, ByVal logFilename As String)

			Try
				If String.IsNullOrWhiteSpace(CurrentLogFileName) Then CurrentLogFileName = Path.GetTempFileName

				WriteHTMLToLOGFile(msg)

			Catch ex As Exception

			End Try

		End Sub

		Private Sub WriteHTMLToLOGFile(ByVal msg As String)

			Try
				Dim fs As FileStream = New FileStream(CurrentLogFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)
				Dim s As StreamWriter = New StreamWriter(fs)
				s.Close()
				fs.Close()

				'log it
				Dim fs1 As FileStream = New FileStream(CurrentLogFileName, FileMode.Append, FileAccess.Write)
				Dim s1 As StreamWriter = New StreamWriter(fs1)
				If msg.Trim.Length > 5 Then
					s1.Write(String.Format("{0:dd.MM.yyyy hh:mm:ss}::{1}{2}<br>", DateTimeOffset.Now, msg, vbNewLine))
				End If

				s1.Close()
				fs1.Close()

			Catch ex As Exception

			End Try

		End Sub


	End Class

End Namespace
