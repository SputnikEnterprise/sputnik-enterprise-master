Namespace Caching

  Public Class FormCache

#Region "Private Fields"

    Private m_FormProvider As IFormProvider
    Private m_FormCacheGroups As IDictionary(Of Type, FormCacheGroup)

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="formProvider">The form provider.</param>
    Public Sub New(ByVal formProvider As IFormProvider)
      m_FormProvider = formProvider
      m_FormCacheGroups = New Dictionary(Of Type, FormCacheGroup)
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Registers a form.
    ''' </summary>
    ''' <param name="type">The form type.</param>
    ''' <param name="cacheSize">The cache size.</param>
    ''' <returns>Boolean flag indicating success.</returns>
    Public Function RegisterForm(ByVal type As Type, ByVal cacheSize As Integer) As Boolean

      If GetType(System.Windows.Forms.Form).IsAssignableFrom(type) Then

        If Not m_FormCacheGroups.Keys.Contains(type) Then

          Dim cacheGroup As New FormCacheGroup(type, m_FormProvider, cacheSize)

          m_FormCacheGroups.Add(type, cacheGroup)

        End If

        Dim group = m_FormCacheGroups(type)
        group.CacheSize = cacheSize

        Return True
      End If

      Return False
    End Function

    ''' <summary>
    ''' Gets a form by type.
    ''' </summary>
    ''' <param name="type">The type of the form.</param>
    ''' <returns>The form or nothing.</returns>
    Public Function GetFormByType(ByVal type As Type) As System.Windows.Forms.Form

      Dim group = GetFormCacheGroup(type)

      If Not group Is Nothing Then
        Return group.GetForm()
      Else
        Return Nothing
      End If

    End Function

    Public Sub ClearCache()

      For Each grp In m_FormCacheGroups.Values
        grp.Clear()
      Next

    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Gets a form cache group.
    ''' </summary>
    ''' <param name="type">The type.</param>
    ''' <returns>The cache group or nothing</returns>
    Private Function GetFormCacheGroup(ByVal type As Type) As FormCacheGroup

      If m_FormCacheGroups.Keys.Contains(type) Then
        Return m_FormCacheGroups(type)
      End If

      Return Nothing

    End Function

#End Region

  End Class

End Namespace
