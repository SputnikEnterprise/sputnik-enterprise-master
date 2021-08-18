Imports System.Threading.Tasks
Imports System.Threading

Public Class TaskHelper

#Region "Private Fields"

    Private m_UISynchronizationContext As TaskScheduler

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="uiSyncContext">The UI sync context.</param>
    Public Sub New(ByVal uiSyncContext As TaskScheduler)
        m_UISynchronizationContext = uiSyncContext
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Executes a function in UI.
    ''' </summary>
    ''' <typeparam name="T">The type.</typeparam>
    ''' <param name="func">The function.</param>
    ''' <returns>The task.</returns>
    Public Function ExecuteInUI(Of T)(ByVal func As Func(Of T)) As Task(Of T)
        Dim startedTask = Task(Of T).Factory.StartNew(func, CancellationToken.None,
                                           TaskCreationOptions.None,
                                           m_UISynchronizationContext)
        Return startedTask
    End Function
    ''' <summary>
    ''' Executs a function in UI and waits.
    ''' </summary>
    ''' <typeparam name="T">The type.</typeparam>
    ''' <param name="func">The function.</param>
    Public Sub InUIAndWait(Of T)(ByVal func As Func(Of T))
        Dim startedTask = ExecuteInUI(func)
        startedTask.Wait()
    End Sub


#End Region

End Class
