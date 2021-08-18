
Imports HtmlAgilityPack
Imports System
Imports System.IO

Namespace HTMLUtiles

	Public Class Utilities
		Public Function ConvertToPlainText(ByVal html As String) As String
			Dim doc As HtmlDocument = New HtmlDocument()
			doc.LoadHtml(html)
			Dim sw As StringWriter = New StringWriter()
			ConvertTo(doc.DocumentNode, sw)
			sw.Flush()
			Return sw.ToString()
		End Function

		Public Function CountWords(ByVal plainText As String) As Integer
			Return If(Not String.IsNullOrEmpty(plainText), plainText.Split(" "c, vbLf).Length, 0)
		End Function

		Public Function Cut(ByVal text As String, ByVal length As Integer) As String
			If Not String.IsNullOrEmpty(text) AndAlso text.Length > length Then
				text = text.Substring(0, length - 4) & " ..."
			End If

			Return text
		End Function

		Private Sub ConvertContentTo(ByVal node As HtmlNode, ByVal outText As TextWriter)
			For Each subnode As HtmlNode In node.ChildNodes
				ConvertTo(subnode, outText)
			Next
		End Sub

		Private Sub ConvertTo(ByVal node As HtmlNode, ByVal outText As TextWriter)
			Dim html As String

			Select Case node.NodeType
				Case HtmlNodeType.Comment
				Case HtmlNodeType.Document
					ConvertContentTo(node, outText)
				Case HtmlNodeType.Text
					Dim parentName As String = node.ParentNode.Name
					If (parentName = "script") OrElse (parentName = "style") Then Exit Select
					html = (CType(node, HtmlTextNode)).Text
					If HtmlNode.IsOverlappedClosingElement(html) Then Exit Select

					If html.Trim().Length > 0 Then
						outText.Write(HtmlEntity.DeEntitize(html))
					End If

				Case HtmlNodeType.Element

					Select Case node.Name
						Case "p"
							outText.Write(vbCrLf)
						Case "br"
							outText.Write(vbCrLf)
					End Select

					If node.HasChildNodes Then
						ConvertContentTo(node, outText)
					End If
			End Select
		End Sub

		Public Function ConvertHtmlToMarkDown(ByVal htmlCode As String) As String
			Dim result As String = htmlCode
			Dim htmlToMarkdown As New Html2Markdown.Converter

			result = htmlToMarkdown.Convert(htmlCode).ToString

			Return result
		End Function

	End Class


End Namespace
