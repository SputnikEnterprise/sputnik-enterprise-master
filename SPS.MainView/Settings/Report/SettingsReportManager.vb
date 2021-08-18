Imports SP.Infrastructure.Settings

''' <summary>
''' Settings manager.
''' </summary>
''' <remarks></remarks>
Public Class SettingsReportManager
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


''' <summary>
''' Settings manager.
''' </summary>
''' <remarks></remarks>
Public Class SettingsZGManager
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


''' <summary>
''' Settings manager.
''' </summary>
''' <remarks></remarks>
Public Class SettingsSalaryManager
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
