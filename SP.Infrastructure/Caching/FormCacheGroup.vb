Imports System.Windows.Forms

Namespace Caching

  Public Class FormCacheGroup

#Region "Private Fields"

    Private m_FormType As Type
    Private m_FormProvider As IFormProvider
    Private m_CacheSize As Integer = 0
    Private m_CachedForms As List(Of System.Windows.Forms.Form)
#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    Public Sub New(ByVal formType As Type, ByVal formProvider As IFormProvider, ByVal cacheSize As Integer)

      m_FormType = formType
      m_FormProvider = formProvider
      m_CacheSize = cacheSize
      m_CachedForms = New List(Of Form)

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets or sets the cache size.
    ''' </summary>
    ''' <returns>The cache size.</returns>
    Public Property CacheSize
      Get
        Return m_CacheSize
      End Get
      Set(value)

        If value > m_CacheSize Then
          m_CacheSize = value
        End If

      End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Gets a from.
    ''' </summary>
    ''' <returns>A cached or new form if none is in cache.</returns>
    Public Function GetForm() As Form

      Dim frm As Form = Nothing

      frm = FindHiddenForm()

      If Not frm Is Nothing Then
        Return frm
      End If

      ' No hidden forms -> create new one or use a visible form
      If m_CachedForms.Count < m_CacheSize Then

        ' Create new form
        frm = m_FormProvider.ProvideNewFormOfType(m_FormType)
        m_CachedForms.Add(frm)

      Else

        ' All forms are in use -> take the first visible form.
        frm = m_CachedForms(0)
      End If

      Return frm

    End Function

    ''' <summary>
    ''' Clears the cache gorup
    ''' </summary>
    Public Sub Clear()

      m_CachedForms.Clear()

    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Finds a hidden form.
    ''' </summary>
    ''' <returns>The found form or nothing.</returns>
    Private Function FindHiddenForm() As System.Windows.Forms.Form

      For Each frm In m_CachedForms

        If frm.Visible = False Then
          Return frm
        End If

      Next

      Return Nothing
    End Function

#End Region

  End Class

End Namespace
