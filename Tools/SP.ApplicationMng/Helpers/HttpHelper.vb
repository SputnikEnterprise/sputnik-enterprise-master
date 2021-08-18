

Imports System.Collections.Specialized
Imports System.Threading.Tasks

Public Class HttpHelper

  ''' <summary>
  ''' Liefert einen Query String aus einer <see cref="NameValueCollection"/>.
  ''' </summary>
  ''' <param name="nvc"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Shared Function NvcToQueryString(nvc As NameValueCollection) As String
    Dim array = (
      From keyString In nvc.AllKeys
      From valueString In nvc.GetValues(keyString)
      Select String.Format("{0}={1}", HttpUtility.UrlEncode(keyString), HttpUtility.UrlEncode(valueString))
      ).ToArray()

    Return String.Join("&", array)
  End Function

  Public Shared Function Delay(milliseconds As Double) As Task
    Dim tcs = New TaskCompletionSource(Of Boolean)()
    Dim timer = New System.Timers.Timer()
    AddHandler timer.Elapsed, Function() (tcs.TrySetResult(True))
    timer.Interval = milliseconds
    timer.AutoReset = False
    timer.Start()
    Return tcs.Task
  End Function

End Class


