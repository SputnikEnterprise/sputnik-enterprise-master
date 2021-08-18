
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices
Imports System.Xml

Module FuncXML

  Dim _ClsFunc As New ClsDivFunc

  Dim _ClsReg As New SPProgUtility.ClsDivReg
  'Dim _ClsSystem As New SPProgUtility.ClsMain_Net
  Dim _ClsProgSetting As New SPProgUtility.ClsProgSettingPath

  Function GetXMLValueByQuery(ByVal strFilename As String, _
                              ByVal strQuery As String, _
                              ByVal strValuebyNull As String) As String
    Dim bResult As String = String.Empty
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strFilename, strQuery)

    If strBez = String.Empty Then strBez = strValuebyNull

    Return strBez
  End Function

  Sub XMLWriter4MDFile(ByVal strSection As String, ByVal strAttribute As String, strValue As String)
    Dim strXMLFile As String = _ClsProgSetting.GetMDData_XMLFile
    Dim xDoc As XmlDocument = New XmlDocument()
    Dim xNode As XmlNode
    Dim xElmntFamily As XmlElement = Nothing

    xDoc.Load(strXMLFile)

    '    xNode = xDoc.SelectSingleNode("*//All_FieldsColor")
    xNode = xDoc.SelectSingleNode(String.Format("*//{0}", strSection))

    If xNode IsNot Nothing Then
      If TypeOf xNode Is XmlElement Then xElmntFamily = CType(xNode, XmlElement)

      ' Attribute
      If xElmntFamily.SelectSingleNode(strAttribute) IsNot Nothing Then _
            xElmntFamily.RemoveChild(xElmntFamily.SelectSingleNode(strAttribute))
      InsertTextNode(xDoc, xElmntFamily, strAttribute, strValue)

      xDoc.Save(strXMLFile)
    End If

  End Sub

  Private Function InsertTextNode(ByVal xDoc As XmlDocument, ByVal xNode As XmlNode, _
                                  ByVal strTag As String, ByVal strText As String) As XmlElement
    ' Insert a text node as a child of xNode.
    ' Set the tag to be strTag, and the
    ' text to be strText. Return a pointer
    ' to the new node.

    Dim xNodeTemp As XmlNode

    xNodeTemp = xDoc.CreateElement(strTag)
    xNodeTemp.AppendChild(xDoc.CreateTextNode(strText))
    xNode.AppendChild(xNodeTemp)

    Return CType(xNodeTemp, XmlElement)
  End Function

End Module
