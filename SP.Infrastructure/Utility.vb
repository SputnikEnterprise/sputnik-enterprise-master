Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

''' <summary>
'''  Utility functions.
''' </summary>
Public Class Utility

	''' <summary>
	''' Round amount in swiss rappen
	''' </summary>
	''' <param name="input"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function SwissCommercialRound(input As Decimal) As Decimal
		Return Math.Round(input * 20D, MidpointRounding.AwayFromZero) * 0.05D
	End Function

	''' <summary>
	''' Loads the bytes of a file.
	''' </summary>
	''' <param name="filePath">The file path.</param>
	''' <returns>The bytes of the file or nothing in error case.</returns>
	Public Function LoadFileBytes(ByVal filePath) As Byte()

		Dim bytes As Byte() = Nothing

		Try

			Using fs As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)

				Using br As BinaryReader = New BinaryReader(fs)

					bytes = br.ReadBytes(Convert.ToInt32(fs.Length))

				End Using
			End Using

		Catch ex As Exception
			bytes = Nothing
		End Try

		Return bytes

	End Function

	''' <summary>
	''' Writes bytes to a file.
	''' </summary>
	''' <param name="filePath">The file path.</param>
	''' <param name="bytes">The file bytes.</param>
	''' <returns>Boolean value indicating success.</returns>
	Function WriteFileBytes(ByVal filePath As String, ByVal bytes As Byte()) As Boolean
		Dim success = True

		Try
			File.WriteAllBytes(filePath, bytes)

		Catch ex As Exception
			success = False
		End Try

		Return success

	End Function

	Function IsEMailAddressValid(ByVal eMailAddress As String) As Boolean
		Dim result As Boolean = True

		Dim eMailPattern As New Regex("\A[a-z0-9!#$%&'*+/=?^_‘{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_‘{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\z")
		result = result AndAlso eMailPattern.IsMatch(eMailAddress)

		Return result
	End Function

	''' <summary>
	''' Opens a file with the default program.
	''' </summary>
	''' <param name="filePath">The file path.</param>
	''' <returns>Boolean flag indicating success.</returns>
	Function OpenFileWithDefaultProgram(ByVal filePath As String)

		Dim success As Boolean = True

		Try

			Dim p As New System.Diagnostics.Process
			Dim s As New System.Diagnostics.ProcessStartInfo(filePath)
			s.UseShellExecute = True
			s.WindowStyle = ProcessWindowStyle.Normal
			p.StartInfo = s
			p.Start()

		Catch ex As Exception
			success = False
		End Try

		Return success
	End Function

	''' <summary>
	''' Splits the street and house number. 
	''' </summary>
	''' <param name="streetWithHousenumber">The street with the house number.</param>
	''' <returns>Tuble with street an housenumber seperated</returns>
	Public Function SimpleSplitStreetAndHouseNumber(ByVal streetWithHousenumber As String) As Tuple(Of String, String)
		Dim regex = New Regex("^(?<street>.+)\s(?<housenumber>\S+)$")

		Dim match = regex.Match(streetWithHousenumber)

		If (match.Success) Then
			Dim street As String = match.Groups("street").Value
			Dim houseNumber As String = match.Groups("housenumber").Value
			Dim resultTuple As New Tuple(Of String, String)(street, houseNumber)

			Return resultTuple

		End If

		Return Nothing
	End Function

	''' <summary>
	''' verifying if selected directory is writeable
	''' </summary>
	''' <param name="directoryName"></param>
	''' <returns></returns>
	Public Function IsDirectoryAccessible(ByVal directoryName As String) As Boolean
		Dim result As Boolean = True

		Dim tmpFilename = Path.GetRandomFileName()
		Dim testFile As String = Path.Combine(directoryName, tmpFilename)
		Try
			Dim fileExists As Boolean = File.Exists(testFile)
			Using sw As New StreamWriter(File.Open(testFile, FileMode.OpenOrCreate))
				sw.WriteLine(
							IIf(fileExists,
									"file was already created " & DateTime.Now,
									"file is now created"))
			End Using
			File.Delete(testFile)

		Catch ex As Exception
			Return False

		End Try

		Return result
	End Function

	Public Function ClearAssignedFiles(ByVal pathName As String, ByVal filePattern As String, ByVal subFolderOption As SearchOption) As Boolean
		Dim success As Boolean = True

		Try
			If Directory.Exists(pathName) Then
				For Each deleteFile In Directory.GetFiles(pathName, filePattern, subFolderOption)
					Try
						File.Delete(deleteFile)

					Catch ex As Exception
						Try
							Dim fs = File.OpenWrite(deleteFile)
							Dim info = New UTF8Encoding(True).GetBytes("Test of the OpenWrite method.")
							fs.Write(info, 0, info.Length)
							fs.Close()

							File.Delete(deleteFile)

						Catch ex2 As Exception
							Return False
						End Try

					End Try
				Next
			End If

		Catch ex As Exception
			success = False

		End Try

		Return success
	End Function

	Public Function ClearAssignedFolder(ByVal pathName As String) As Boolean
		Dim success As Boolean = True

		Try
			If Directory.Exists(pathName) Then
				success = success AndAlso ClearAssignedFiles(pathName, "*.*", SearchOption.AllDirectories)

				For Each _folder As String In Directory.GetDirectories(pathName)
					Directory.Delete(_folder, True)
				Next

				Directory.Delete(pathName, True)
			End If

		Catch ex As Exception
			success = False

		End Try

		Return success
	End Function

	Public Function MoveAllFilesToAnotherFolder(ByVal sourceFolder As String, ByVal destFolder As String, ByVal extension As String, ByVal moveFiels As Boolean, ByVal searchallDirectory As SearchOption, ByVal overWrite As Boolean) As Boolean
		Dim result As Boolean = True

		Try

			For Each x In New DirectoryInfo(sourceFolder).GetFiles(extension, searchallDirectory)
				If moveFiels Then
					File.Move(x.FullName, Path.Combine(destFolder, x.Name, overWrite))
				Else
					File.Copy(x.FullName, Path.Combine(destFolder, x.Name, overWrite))
				End If

			Next

		Catch ex As Exception

		End Try

		Return result
	End Function

	Public Function IsValidFileNameOrPath(ByVal fileName As String, Optional ByVal allowPathDefinition As Boolean = False, Optional ByRef firstCharIndex As Integer = Nothing) As Boolean

		Dim file As String = String.Empty
		Dim directory As String = String.Empty

		If allowPathDefinition Then
			file = Path.GetFileName(fileName)
			directory = Path.GetDirectoryName(fileName)
		Else
			file = fileName
		End If

		If Not IsNothing(firstCharIndex) Then
			Dim f As IEnumerable(Of Char)
			f = file.Intersect(Path.GetInvalidFileNameChars())
			If f.Any Then
				firstCharIndex = Len(directory) + file.IndexOf(f.First)
				Return False
			End If

			f = directory.Intersect(Path.GetInvalidPathChars())
			If f.Any Then
				firstCharIndex = directory.IndexOf(f.First)
				Return False
			Else
				Return True
			End If
		Else
			Return Not (file.Intersect(Path.GetInvalidFileNameChars()).Any() OrElse directory.Intersect(Path.GetInvalidPathChars()).Any())
		End If


	End Function


	''' <summary>
	''' truncate a demial number to fix decimal places, without any round!
	''' </summary>
	''' <param name="number"></param>
	''' <param name="digits"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Function FixDecimal(ByVal number As Decimal, ByVal digits As Integer) As Decimal
		Dim x As Integer = 10 ^ digits
		Return Fix(number * x) / x
	End Function

	Public Function ParseToString(ByVal stringvalue As String, ByVal valuebyEmpty As String) As String
		If String.IsNullOrWhiteSpace(stringvalue) Then
			Return valuebyEmpty
		End If
		Return stringvalue
	End Function

	Public Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
		Dim result As Boolean
		If (Not Boolean.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Public Function StringToBoolean(ByVal str As String, ByVal value As Boolean?) As Boolean
		Dim result As Boolean = False

		If str Is Nothing OrElse String.IsNullOrWhiteSpace(str) Then
			Return value.GetValueOrDefault(False)

		ElseIf str.Equals("1") Then
			Return True
		End If

		Boolean.TryParse(str, result)

		Return result
	End Function

	Public Function ParseToDec(ByVal stringvalue As String, ByVal value As Decimal?) As Decimal
		Dim result As Decimal
		If (Not Decimal.TryParse(stringvalue, result)) Then
			Return value
		End If
		Return result
	End Function

	Public Function SafeTrim(ByVal str As String)

		If str Is Nothing Then
			Return str
		End If

		Return str.Trim

	End Function

End Class
