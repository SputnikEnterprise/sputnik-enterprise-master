Imports SP.DatabaseAccess.ES
Imports SP.Infrastructure.UI

Namespace UI

  ''' <summary>
  ''' ES validation utility.
  ''' </summary>
  Public Class ESValidationUtility

#Region "Private Fields"

    ''' <summary>
    ''' Thre translation value helper.
    ''' </summary>
    Private m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper

    ''' <summary>
    ''' The es data access object.
    ''' </summary>
    Private m_ESDatabaseAccess As IESDatabaseAccess

    ''' <summary>
    ''' UI Utility functions.
    ''' </summary>
    Private m_UtilityUI As UtilityUI

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="esDatabaseAccess">The ES database access.</param>
    ''' <param name="translatae">The translate  object.</param>
    Public Sub New(ByVal esDatabaseAccess As IESDatabaseAccess, ByVal translatae As SP.Infrastructure.Initialization.TranslateValuesHelper)

      m_ESDatabaseAccess = esDatabaseAccess
      m_Translate = translatae
      m_UtilityUI = New SP.Infrastructure.UI.UtilityUI

    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Checks for conflicting LO in perdiod.
    ''' </summary>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="mdNumber">The mandant number.</param>
    ''' <param name="startDate">The start date.</param>
    ''' <param name="endDate">The end date.</param>
    ''' <returns>Boolean flag indicating if conflicting LO records exist.</returns>
    Public Function CheckForConflictingLOInPeriod(ByVal employeeNumber As Integer, ByVal mdNumber As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean

      Dim isValid As Boolean = True

      Const RESULT_OK As Integer = 0
      Const RESULT_CONFLICT As Integer = 1

      Dim resultCode As Integer = 0
      Dim conflictedLOL = m_ESDatabaseAccess.LoadConflictedLORecordsInPeriod(employeeNumber, mdNumber, startDate, endDate, resultCode)

      If conflictedLOL Is Nothing OrElse resultCode = -1 Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("Lohn: Fehler bei Konfliktprüfung. Möglicherweise existiert bereits eine Lohnabrechnung.{0}{1:d}-{2:d}"), vbNewLine, startDate, endDate))
				Return False
      Else
        Select Case resultCode
          Case RESULT_OK
            ' no conflicts
          Case RESULT_CONFLICT
            isValid = False

            Dim frmConflictedLOL As New frmConflictedLO(m_Translate)
            frmConflictedLOL.ShowConflicts(conflictedLOL)

        End Select

      End If

      Return isValid
    End Function

    ''' <summary>
    ''' Checks for conflicting RPL in perdiod.
    ''' </summary>
    ''' <param name="esNumber">The ES number.</param>
    ''' <param name="employeeNumber">The employee number.</param>
    ''' <param name="startDate">The start date.</param>
    ''' <param name="endDate">The end date.</param>
    ''' <returns>Boolean flag indicating if conflicting RPL records exist.</returns>
    Public Function CheckForConflictingRPLInPeriod(ByVal esNumber As Integer, ByVal employeeNumber As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean

      Dim isValid As Boolean = True

      Const RESULT_OK As Integer = 0
      Const RESULT_CONFLICT As Integer = 1

      Dim resultCode As Integer = 0
      Dim conflictedRPL = m_ESDatabaseAccess.LoadConflictedRPLRecordsInPeriod(esNumber, employeeNumber, startDate, endDate, resultCode)

      If conflictedRPL Is Nothing OrElse resultCode = -1 Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("RPL: Fehler bei Konfliktprüfung{0}{1:d}-{2:d}"), vbNewLine, startDate, endDate))
        Return False
      Else
        Select Case resultCode
          Case RESULT_OK
            ' no conflicts
          Case RESULT_CONFLICT
            isValid = False

            Dim frmConflictedRPL As New frmConflictedRPL(m_Translate)
            frmConflictedRPL.ShowConflicts(conflictedRPL)

        End Select

      End If

      Return isValid
    End Function

    ''' <summary>
    ''' Checks for conflicting MonthClose records in perdiod.
    ''' </summary>
    ''' <param name="startDate">The start date.</param>
    ''' <param name="mdNumber">The mandant number.</param>
    ''' <param name="endDate">The end date.</param>
    ''' <returns>Boolean flag indicating if conflicting MonthClose records exist.</returns>
    Public Function CheckForConflictingMonthCloseRecordsInPeriod(ByVal mdNumber As Integer, ByVal startDate As DateTime, ByVal endDate As DateTime) As Boolean

      Dim isValid As Boolean = True

      Const RESULT_OK As Integer = 0
      Const RESULT_CONFLICT As Integer = 1

      Dim resultCode As Integer = 0
      Dim conflictedRPL = m_ESDatabaseAccess.LoadConflictedMonthCloseRecordsInPeriod(mdNumber, startDate, endDate, resultCode)

      If conflictedRPL Is Nothing OrElse resultCode = -1 Then
				m_UtilityUI.ShowErrorDialog(String.Format(m_Translate.GetSafeTranslationValue("MonthClosed: Fehler bei Konfliktprüfung{0}{1:d}-{2:d}"), vbNewLine, startDate, endDate))
				Return False
      Else
        Select Case resultCode
          Case RESULT_OK
            ' no conflicts
          Case RESULT_CONFLICT
            isValid = False

            Dim frmConflictedMonthCloseRecoreds As New frmConflictedMonthClose(m_Translate)
            frmConflictedMonthCloseRecoreds.ShowConflicts(conflictedRPL)

        End Select

      End If

      Return isValid
    End Function

#End Region

  End Class

End Namespace
