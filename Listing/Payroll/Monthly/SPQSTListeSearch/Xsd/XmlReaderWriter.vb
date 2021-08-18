Imports System.IO
Imports System.Xml.Serialization

Namespace Xsd

  ''' <summary>
  ''' Class for serializing/deserializing classes to/from XML files.
  ''' <author>egle</author>
  ''' </summary>
  ''' <remarks></remarks>
  Public Class XmlReaderWriter

    ''' <summary>
    ''' Saves a class instance of the specified TypeName to an XML file.
    ''' </summary>
    ''' <typeparam name="ClassType"></typeparam>
    ''' <param name="filename"></param>
    ''' <param name="classInstance"></param>
    ''' <remarks></remarks>
    Public Shared Sub Save(Of ClassType)(filename As String, classInstance As ClassType)
      Using streamWriter As New StreamWriter(filename)
        Dim xmlSerializer As New XmlSerializer(GetType(ClassType))
        xmlSerializer.Serialize(streamWriter, classInstance)
      End Using
    End Sub

  End Class

End Namespace
