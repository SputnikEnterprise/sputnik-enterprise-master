''IClsDbFunc.vb  - Naas added on 23.08.2018
'Imports System.IO
'Imports SP.Infrastructure.Logging
'Imports SP.Infrastructure.UI
'Imports SPRPListSearch.ClsDataDetail


'Public Class ClsDbFunctions
'	'Naas - ToDo -- (Wollte diese wegen der Function 'ReplaceMissing' integrieren)
'	'Inherits SP.DatabaseAccess.DatabaseAccessBase    
'	'Implements IClsDbRegister2

'	''' <summary>
'	''' The logger.
'	''' </summary>
'	Private Shared m_Logger As ILogger = New Logger()

'  ''' <summary>
'  ''' Utility functions.
'  ''' </summary>
'  Private m_Utility As SPProgUtility.MainUtilities.Utilities

'  ''' <summary>
'  ''' UI Utility functions.
'  ''' </summary>
'  Private m_UtilityUI As UtilityUI



'#Region "Contructor"

'  Public Sub New()

'    m_Utility = New SPProgUtility.MainUtilities.Utilities
'    m_UtilityUI = New UtilityUI

'  End Sub

'#End Region

'	'''' <summary>
'	'''' Writes the database table 'RPPrint' for Printing
'	'''' </summary>
'	'''' <param name="data">Data list from the database table 'RP' </param>
'	'''' <param name="DeleteExistigRecords">Specifies whether the old data of the database table 'RPPrint' should be deleted </param>
'	'Public Function AddWeeklyRPPrintDataIntoTable(ByVal data As RPPrintRecordData, ByVal DeleteExistigRecords As Boolean) As Boolean Implements IClsDbRegister2.AssignedRPPrintDataForImport

'	'	'RPDataForRPPrint
'	'	Dim strMsgText = m_Translate.GetSafeTranslationValue("Die alten Wochenrapport-Daten konnten nicht gelöscht werden!")
'	'	Dim strMsgHeader = m_Translate.GetSafeTranslationValue("Daten löschen")
'	'	Dim success As Boolean = True
'	'	Dim sql As String

'	'	Try

'	'		If DeleteExistigRecords Then
'	'			' Delete the old' RPPrint 'data from the current user
'	'			Dim DelListOfParams As New List(Of SqlClient.SqlParameter)
'	'			sql = "DELETE [RPPrint]"
'	'			sql &= " Where USNr = " & data.UserNr
'	'			DelListOfParams.Add(New SqlClient.SqlParameter("USNr", data.UserNr))

'	'			success = success AndAlso m_Utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, DelListOfParams) ', CommandType.Text, False)
'	'			If Not success Then
'	'				m_Logger.LogError(strMsgText)
'	'				m_UtilityUI.ShowOKDialog(strMsgText, strMsgHeader, MessageBoxIcon.Exclamation)
'	'			End If

'	'		End If

'	'		If success Then
'	'			sql = "Insert Into RPPrint ("
'	'			sql &= "RPNr, "
'	'			sql &= "MANr, "
'	'			sql &= "KDNr, "
'	'			sql &= "ESNr, "
'	'			sql &= "Mo, "
'	'			sql &= "Di, "
'	'			sql &= "Mi, "
'	'			sql &= "Do, "
'	'			sql &= "Fr, "
'	'			sql &= "Sa, "
'	'			sql &= "So, "
'	'			sql &= "Monat, "
'	'			sql &= "Woche, "
'	'			sql &= "Jahr, "
'	'			sql &= "PrintedWeeks, "
'	'			sql &= "PrintedDate, "
'	'			sql &= "USNr) "

'	'			sql &= "Values ("
'	'			sql &= "@RPNr, "
'	'			sql &= "@MANr, "
'	'			sql &= "@KDNr, "
'	'			sql &= "@ESNr, "
'	'			sql &= "@Mo, "
'	'			sql &= "@Di, "
'	'			sql &= "@Mi, "
'	'			sql &= "@Do, "
'	'			sql &= "@Fr, "
'	'			sql &= "@Sa, "
'	'			sql &= "@So, "
'	'			sql &= "@Monat, "
'	'			sql &= "@Woche, "
'	'			sql &= "@Jahr, "
'	'			sql &= "@PrintedWeeks, "
'	'			sql &= "@PrintedDate, "
'	'			sql &= "@USNr) "

'	'			Dim listOfParams As New List(Of SqlClient.SqlParameter)
'	'			listOfParams.Add(New SqlClient.SqlParameter("RPNr", data.RPNr))
'	'			listOfParams.Add(New SqlClient.SqlParameter("MANr", data.MANr))
'	'			listOfParams.Add(New SqlClient.SqlParameter("KDNr", data.KDNr))
'	'			listOfParams.Add(New SqlClient.SqlParameter("ESNr", data.ESNr))
'	'			listOfParams.Add(New SqlClient.SqlParameter("Mo", ReplaceMissing(data.MondayDate, DBNull.Value)))
'	'			listOfParams.Add(New SqlClient.SqlParameter("Di", ReplaceMissing(data.TuesdayDate, DBNull.Value)))
'	'			listOfParams.Add(New SqlClient.SqlParameter("Mi", ReplaceMissing(data.WednesdayDate, DBNull.Value)))
'	'			listOfParams.Add(New SqlClient.SqlParameter("Do", ReplaceMissing(data.ThursdayDate, DBNull.Value)))
'	'			listOfParams.Add(New SqlClient.SqlParameter("Fr", ReplaceMissing(data.FridayDate, DBNull.Value)))
'	'			listOfParams.Add(New SqlClient.SqlParameter("Sa", ReplaceMissing(data.SaturdayDate, DBNull.Value)))
'	'			listOfParams.Add(New SqlClient.SqlParameter("So", ReplaceMissing(data.SundayDate, DBNull.Value)))
'	'			listOfParams.Add(New SqlClient.SqlParameter("Monat", data.Month))
'	'			listOfParams.Add(New SqlClient.SqlParameter("Woche", data.Week))
'	'			listOfParams.Add(New SqlClient.SqlParameter("Jahr", data.Year))
'	'			listOfParams.Add(New SqlClient.SqlParameter("PrintedWeeks", data.PrintedWeeks))
'	'			listOfParams.Add(New SqlClient.SqlParameter("PrintedDate", data.PrintedDates))
'	'			listOfParams.Add(New SqlClient.SqlParameter("USNr", data.UserNr))

'	'			success = success AndAlso m_Utility.ExecuteNonQuery(m_InitialData.MDData.MDDbConn, sql, listOfParams, CommandType.Text, False)
'	'		End If

'	'	Catch ex As Exception
'	'		m_Logger.LogError(String.Format("{0}", ex.ToString))
'	'		success = False

'	'	End Try

'	'	Return success

'	'End Function

'	'''' <summary>
'	'''' Replaces a missing object with another object.
'	'''' Naas - ToDo -- (Fardin  - Konnte die Public funktion von 'DatabaseAccessBase' nicht einbinden)
'	'''' </summary>
'	'''' <param name="obj">The object.</param>
'	'''' <param name="replacementObject">The replacement object.</param>
'	'''' <returns>The object or the replacement object it the object is nothing.</returns>
'	'Private Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object

'	'   If (obj Is Nothing) Then
'	'     Return replacementObject
'	'   Else
'	'     Return obj
'	'   End If

'	' End Function

'End Class
