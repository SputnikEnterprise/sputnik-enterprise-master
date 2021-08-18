Imports SP.Infrastructure.Settings

''' <summary>
''' Settings manager.
''' </summary>
''' <remarks></remarks>
Public Class SettingsREManager
  Inherits AbsSettingsManager

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  Public Sub New()
    MyBase.New(My.Settings)
  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Save settings implementation.
  ''' </summary>
  Public Overrides Sub SaveSettings()
    My.Settings.Save()
  End Sub

#End Region

End Class


Public Class SettingsGUManager
  Inherits AbsSettingsManager

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  Public Sub New()
    MyBase.New(My.Settings)
  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Save settings implementation.
  ''' </summary>
  Public Overrides Sub SaveSettings()
    My.Settings.Save()
  End Sub

#End Region

End Class


Public Class SettingsZEManager
  Inherits AbsSettingsManager

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  Public Sub New()
    MyBase.New(My.Settings)
  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Save settings implementation.
  ''' </summary>
  Public Overrides Sub SaveSettings()
    My.Settings.Save()
  End Sub

#End Region

End Class

Public Class SettingsMahnManager
  Inherits AbsSettingsManager

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  Public Sub New()
    MyBase.New(My.Settings)
  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Save settings implementation.
  ''' </summary>
  Public Overrides Sub SaveSettings()
    My.Settings.Save()
  End Sub

#End Region

End Class


Public Class SettingsFOPManager
  Inherits AbsSettingsManager

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  Public Sub New()
    MyBase.New(My.Settings)
  End Sub

#End Region

#Region "Public Methods"

  ''' <summary>
  ''' Save settings implementation.
  ''' </summary>
  Public Overrides Sub SaveSettings()
    My.Settings.Save()
  End Sub

#End Region

End Class
