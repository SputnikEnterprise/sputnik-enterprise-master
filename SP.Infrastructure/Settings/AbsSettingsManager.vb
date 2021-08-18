Imports System.Reflection

Namespace Settings

    ''' <summary>
    ''' Settings manager implementation.
    ''' </summary>
    Public MustInherit Class AbsSettingsManager
        Implements ISettingsManager

#Region "Private Fields"

        Private m_SettingsSource As Object

#End Region

        ''' <summary>
        ''' The constructor.
        ''' </summary>
        ''' <param name="settingsSource">The settings source object.</param>
        Public Sub New(ByVal settingsSource As Object)
            m_SettingsSource = settingsSource
        End Sub

#Region "Public Methods"

        ''' <summary>
        ''' Reads a string value.
        ''' </summary>
        ''' <param name="settingName">The setting name.</param>
        ''' <returns>The string value. Returns string.empty in error case.</returns>
        Public Function ReadString(ByVal settingName As String) As String Implements ISettingsManager.ReadString
            Dim value As Object = ReadSetting(settingName)
            Return CType(value, String)
        End Function

        ''' <summary>
        ''' Writes a string value.
        ''' </summary>
        ''' <param name="settingName">The setting name.</param>
        ''' <param name="value">The string value.</param>
        Public Sub WriteString(ByVal settingName As String, ByVal value As String) Implements ISettingsManager.WriteString
            WriteSetting(settingName, value)
        End Sub

        ''' <summary>
        ''' Reads a integer value.
        ''' </summary>
        ''' <param name="settingName">The setting name.</param>
        ''' <returns>The setting value. Zero is returned in error case.</returns>
        Public Function ReadInteger(ByVal settingName As String) As Integer Implements ISettingsManager.ReadInteger
            Dim value = ReadSetting(settingName)
            Return CType(value, Integer)
        End Function

        ''' <summary>
        ''' Writes a integer value.
        ''' </summary>
        ''' <param name="settingName">The settings name.</param>
        ''' <param name="value">The esttings value.</param>
        Public Sub WriteInteger(ByVal settingName As String, ByVal value As Integer) Implements ISettingsManager.WriteInteger
            WriteSetting(settingName, value)
        End Sub

        ''' <summary>
        ''' Reads a boolean value.
        ''' </summary>
        ''' <param name="settingName">The setting name.</param>
        ''' <returns>The setting value. Zero is returned in error case.</returns>
        Public Function ReadBoolean(ByVal settingName As String) As Boolean Implements ISettingsManager.ReadBoolean
            Dim value = ReadSetting(settingName)
            Return CType(value, Boolean)
        End Function

        ''' <summary>
        ''' Writes a boolean value.
        ''' </summary>
        ''' <param name="settingName">The settings name.</param>
        ''' <param name="value">The settings value.</param>
        Public Sub WriteBoolean(ByVal settingName As String, ByVal value As Boolean) Implements ISettingsManager.WriteBoolean
            WriteSetting(settingName, value)
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Reads as setting.
        ''' </summary>
        ''' <param name="settingsName">The setting name.</param>
        ''' <returns>The setting value</returns>
        Private Function ReadSetting(ByVal settingsName As String) As Object
            Dim value As Object = Nothing
            value = ReadPropertyValueOfObject(m_SettingsSource, settingsName)
            Return value
        End Function

        ''' <summary>
        ''' Writes a setting.
        ''' </summary>
        ''' <param name="settingsName">The setting name.</param>
        ''' <param name="value">The value.</param>
        Private Sub WriteSetting(ByVal settingsName As String, ByVal value As Object)
            WritePropertyValueOfObject(m_SettingsSource, settingsName, value)
        End Sub

        ''' <summary>
        ''' Saves settings.
        ''' </summary>
        Public MustOverride Sub SaveSettings() Implements ISettingsManager.SaveSettings

        ''' <summary>
        ''' Reads property value of an object via reflection.
        ''' </summary>
        ''' <param name="obj">The object.</param>
        ''' <param name="propertyName">The property name.</param>
        ''' <returns>The property value.</returns>
        Private Function ReadPropertyValueOfObject(ByVal obj As Object, ByVal propertyName As String) As Object

            Dim pinfo As PropertyInfo = obj.GetType().GetProperty(propertyName)
            Dim value As Object = pinfo.GetValue(obj, Nothing)

            Return value
        End Function

        ''' <summary>
        ''' Writes a property value of an object via reflection.
        ''' </summary>
        ''' <param name="obj">The object.</param>
        ''' <param name="propertyName">The property name.</param>
        ''' <param name="value">The value.</param>
        Private Sub WritePropertyValueOfObject(ByVal obj As Object, ByVal propertyName As String, ByVal value As Object)

            Dim pinfo As PropertyInfo = obj.GetType().GetProperty(propertyName)
            pinfo.SetValue(obj, value, Nothing)

        End Sub

#End Region

    End Class

End Namespace


