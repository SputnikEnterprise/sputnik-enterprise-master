
Module FuncTest
  Dim _ClsReg As New SPProgUtility.ClsDivReg

  Dim Fragenkatalog As XElement = _
    <Fragenkatalog>
      <Frage Frage="Ist heute Montag?" AntwortA="Ja." AntwortB="Nein." Korrekt="B"/>
      <Frage Frage="Ist heute Dienstag?" AntwortA="Ja." AntwortB="Nein." Korrekt="A"/>
      <Frage Frage="Ist heute Mittwoch?" AntwortA="Ja." AntwortB="Nein." Korrekt="B"/>
      <Frage Frage="Ist heute Donnerstag?" AntwortA="Ja." AntwortB="Nein." Korrekt="B"/>
    </Fragenkatalog>


  Function GetXMLValueByQuery(ByVal strFilename As String, _
                                ByVal strQuery As String, _
                                ByVal strValuebyNull As String) As String
    Dim bResult As String = String.Empty
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strFilename, strQuery)

    If strBez = String.Empty Then strBez = strValuebyNull

    Return strBez
  End Function

  Sub Testme()
    Dim Abfrage = From Frage In Fragenkatalog.<Frage> _
                  Where Frage.@Frage Like "*Montag*" _
                  Select Frage

    For Each Frage In Abfrage
      Console.WriteLine(Frage.@Frage)
    Next

    Console.ReadKey()
  End Sub

End Module
