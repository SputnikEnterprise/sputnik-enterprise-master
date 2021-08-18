
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Xml.Serialization
''' <summary>
''' A generic class used to serialize objects.
''' </summary>
Public Class GenericSerializer

  ''' <summary>
  ''' Serializes the given object.
  ''' </summary>
  ''' <typeparam name="T">The type of the object to be serialized.</typeparam>
  ''' <param name="obj">The object to be serialized.</param>
  ''' <returns>String representation of the serialized object.</returns>
  Public Overloads Shared Function Serialize(Of T)(ByVal obj As T) As String
    Dim xs As XmlSerializer = Nothing
    Dim sw As StringWriter = Nothing
    Try
      xs = New XmlSerializer(GetType(T))
      sw = New StringWriter
      xs.Serialize(sw, obj)
      sw.Flush()
      Return sw.ToString
    Catch ex As Exception
      Throw ex
    Finally
      If (Not (sw) Is Nothing) Then
        sw.Close()
        sw.Dispose()
      End If

    End Try

  End Function

  Public Overloads Shared Function Serialize(Of T)(ByVal obj As T, ByVal extraTypes() As Type) As String
    Dim xs As XmlSerializer = Nothing
    Dim sw As StringWriter = Nothing
    Try
      xs = New XmlSerializer(GetType(T), extraTypes)
      sw = New StringWriter
      xs.Serialize(sw, obj)
      sw.Flush()
      Return sw.ToString
    Catch ex As Exception
      Throw ex
    Finally
      If (Not (sw) Is Nothing) Then
        sw.Close()
        sw.Dispose()
      End If

    End Try

  End Function

  ''' <summary>
  ''' Serializes the given object.
  ''' </summary>
  ''' <typeparam name="T">The type of the object to be serialized.</typeparam>
  ''' <param name="obj">The object to be serialized.</param>
  ''' <param name="writer">The writer to be used for output in the serialization.</param>
  Public Overloads Shared Sub Serialize(Of T)(ByVal obj As T, ByVal writer As XmlWriter)
    Dim xs As XmlSerializer = New XmlSerializer(GetType(T))
    xs.Serialize(writer, obj)
  End Sub

  ''' <summary>
  ''' Serializes the given object.
  ''' </summary>
  ''' <typeparam name="T">The type of the object to be serialized.</typeparam>
  ''' <param name="obj">The object to be serialized.</param>
  ''' <param name="writer">The writer to be used for output in the serialization.</param>
  ''' <param name="extraTypes"><c>Type</c> array
  '''       of additional object types to serialize.</param>
  Public Overloads Shared Sub Serialize(Of T)(ByVal obj As T, ByVal writer As XmlWriter, ByVal extraTypes() As Type)
    Dim xs As XmlSerializer = New XmlSerializer(GetType(T), extraTypes)
    xs.Serialize(writer, obj)
  End Sub

  ''' <summary>
  ''' Deserializes the given object.
  ''' </summary>
  ''' <typeparam name="T">The type of the object to be deserialized.</typeparam>
  ''' <param name="reader">The reader used to retrieve the serialized object.</param>
  ''' <returns>The deserialized object of type T.</returns>
  Public Overloads Shared Function Deserialize(Of T)(ByVal reader As XmlReader) As T
    Dim xs As XmlSerializer = New XmlSerializer(GetType(T))
    Return CType(xs.Deserialize(reader), T)
  End Function

  ''' <summary>
  ''' Deserializes the given object.
  ''' </summary>
  ''' <typeparam name="T">The type of the object to be deserialized.</typeparam>
  ''' <param name="reader">The reader used to retrieve the serialized object.</param>
  ''' <param name="extraTypes"><c>Type</c> array
  '''           of additional object types to deserialize.</param>
  ''' <returns>The deserialized object of type T.</returns>
  Public Overloads Shared Function Deserialize(Of T)(ByVal reader As XmlReader, ByVal extraTypes() As Type) As T
    Dim xs As XmlSerializer = New XmlSerializer(GetType(T), extraTypes)
    Return CType(xs.Deserialize(reader), T)
  End Function

  ''' <summary>
  ''' Deserializes the given object.
  ''' </summary>
  ''' <typeparam name="T">The type of the object to be deserialized.</typeparam>
  ''' <param name="XML">The XML file containing the serialized object.</param>
  ''' <returns>The deserialized object of type T.</returns>
  Public Overloads Shared Function Deserialize(Of T)(ByVal XML As String) As T
    If ((XML Is Nothing) OrElse (XML = String.Empty)) Then
      Return Nothing
    End If

    Dim xs As XmlSerializer = Nothing
    Dim sr As StringReader = Nothing
    Try
      xs = New XmlSerializer(GetType(T))
      sr = New StringReader(XML)
      Return CType(xs.Deserialize(sr), T)
    Catch ex As Exception
      Throw ex
    Finally
      If (Not (sr) Is Nothing) Then
        sr.Close()
        sr.Dispose()
      End If

    End Try

  End Function

  Public Overloads Shared Function Deserialize(Of T)(ByVal XML As String, ByVal extraTypes() As Type) As T
    If ((XML Is Nothing) _
                    OrElse (XML = String.Empty)) Then
      Return Nothing
    End If

    Dim xs As XmlSerializer = Nothing
    Dim sr As StringReader = Nothing
    Try
      xs = New XmlSerializer(GetType(T), extraTypes)
      sr = New StringReader(XML)
      Return CType(xs.Deserialize(sr), T)
    Catch ex As Exception
      Throw ex
    Finally
      If (Not (sr) Is Nothing) Then
        sr.Close()
        sr.Dispose()
      End If

    End Try

  End Function

  Public Overloads Shared Sub SaveAs(Of T)(ByVal Obj As T, ByVal FileName As String, ByVal encoding As Encoding, ByVal extraTypes() As Type)
    If File.Exists(FileName) Then
      File.Delete(FileName)
    End If

    Dim di As DirectoryInfo = New DirectoryInfo(Path.GetDirectoryName(FileName))
    If Not di.Exists Then
      di.Create()
    End If

    Dim document As XmlDocument = New XmlDocument
    Dim wSettings As XmlWriterSettings = New XmlWriterSettings
    wSettings.Indent = True
    wSettings.Encoding = encoding
    wSettings.CloseOutput = True
    wSettings.CheckCharacters = False
    Dim writer As XmlWriter = XmlWriter.Create(FileName, wSettings)
    If (Not (extraTypes) Is Nothing) Then
      GenericSerializer.Serialize(Of T)(Obj, writer, extraTypes)
    Else
      GenericSerializer.Serialize(Of T)(Obj, writer)
    End If

    writer.Flush()
    document.Save(writer)
  End Sub

  Public Overloads Shared Sub SaveAs(Of T)(ByVal Obj As T, ByVal FileName As String, ByVal extraTypes() As Type)
    GenericSerializer.SaveAs(Of T)(Obj, FileName, Encoding.UTF8, extraTypes)
  End Sub

  Public Overloads Shared Sub SaveAs(Of T)(ByVal Obj As T, ByVal FileName As String, ByVal encoding As Encoding)
    GenericSerializer.SaveAs(Of T)(Obj, FileName, encoding, Nothing)
  End Sub

  Public Overloads Shared Sub SaveAs(Of T)(ByVal Obj As T, ByVal FileName As String)
    GenericSerializer.SaveAs(Of T)(Obj, FileName, Encoding.UTF8)
  End Sub

  Public Overloads Shared Function Open(Of T)(ByVal FileName As String, ByVal extraTypes() As Type) As T
    Dim obj As T
    If File.Exists(FileName) Then
      Dim rSettings As XmlReaderSettings = New XmlReaderSettings
      rSettings.CloseInput = True
      rSettings.CheckCharacters = False
      Dim reader As XmlReader = XmlReader.Create(FileName, rSettings)
      reader.ReadOuterXml()

      If (Not (extraTypes) Is Nothing) Then
        obj = GenericSerializer.Deserialize(Of T)(reader, extraTypes)
      Else
        obj = GenericSerializer.Deserialize(Of T)(reader)
      End If

    End If

    Return obj
  End Function

  Public Overloads Shared Function Open(Of T)(ByVal FileName As String) As T
    Return GenericSerializer.Open(Of T)(FileName, Nothing)
  End Function
End Class
