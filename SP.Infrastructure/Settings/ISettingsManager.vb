
Namespace Settings

    ''' <summary>
    ''' Interface of settings manager.
    ''' </summary>
    Public Interface ISettingsManager
       
        Function ReadString(ByVal settingName As String) As String
        Sub WriteString(ByVal settingName As String, ByVal value As String)

        Function ReadInteger(ByVal settingName As String) As Integer
        Sub WriteInteger(ByVal settingName As String, ByVal value As Integer)

        Function ReadBoolean(ByVal settingName As String) As Boolean
        Sub WriteBoolean(ByVal settingName As String, ByVal value As Boolean)

        Sub SaveSettings()
    End Interface

End Namespace
