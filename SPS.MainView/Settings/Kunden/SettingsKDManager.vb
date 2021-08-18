﻿
Imports SP.Infrastructure.Settings


''' <summary>
''' Settings manager.
''' </summary>
''' <remarks></remarks>
Public Class SettingsKDManager
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
Public Class SettingsVacancyManager

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
Public Class SettingsProposeManager
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
