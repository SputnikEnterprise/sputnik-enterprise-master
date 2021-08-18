Imports SPProgUtility

Namespace Initialization

  Public Class InitializeClass

#Region "Private Fields"

    Private m_TranslationData As Dictionary(Of String, ClsTranslationData)
    Private m_PersonalizedData As Dictionary(Of String, ClsProsonalizedData)
    Private m_ClsMData As ClsMDData
    Private m_UserData As ClsUserData

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="translationData">The translation data.</param>
    ''' <param name="personalizedData">The personalized data.</param>
    ''' <param name="clsMDData">The clsMandant data.</param>
    ''' <param name="userData">The user data.</param>
    Public Sub New(ByVal translationData As Dictionary(Of String, ClsTranslationData),
                   ByVal personalizedData As Dictionary(Of String, ClsProsonalizedData),
                   ByVal clsMDData As ClsMDData,
                   ByVal userData As ClsUserData)

      Me.m_TranslationData = translationData
      Me.m_PersonalizedData = personalizedData
      Me.m_ClsMData = clsMDData
			If Not userData Is Nothing Then
				userData.UserFName = userData.UserFName.ToString.Trim
				userData.UserLName = userData.UserLName.ToString.Trim
			End If
			Me.m_UserData = userData

		End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property TranslationData As Dictionary(Of String, ClsTranslationData)
      Get
        Return m_TranslationData
      End Get
    End Property

    Public ReadOnly Property ProsonalizedData As Dictionary(Of String, ClsProsonalizedData)
      Get
        Return m_PersonalizedData
      End Get
    End Property

  
    Public ReadOnly Property MDData As SPProgUtility.ClsMDData
      Get
        Return m_ClsMData
      End Get
    End Property

    Public ReadOnly Property UserData As SPProgUtility.ClsUserData
      Get
        Return m_UserData
      End Get
    End Property

#End Region


  End Class

End Namespace
