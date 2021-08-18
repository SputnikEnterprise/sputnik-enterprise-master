Imports SP.DatabaseAccess.Report
Imports SPProgUtility.Mandanten
Imports SP.Infrastructure

Public Class FlexibleTimeHelper

#Region "Private Fields"

  ''' <summary>
  ''' The data access object.
  ''' </summary>
  Protected m_ReportDataAccess As IReportDatabaseAccess

  ''' <summary>
  ''' The mandant.
  ''' </summary>
  Private m_md As Mandant

  ''' <summary>
  ''' The mandant number.
  ''' </summary>
  Private m_MDNumber As Integer

  ''' <summary>
  ''' The utility.
  ''' </summary>
  Private m_Utility As New Utility

#End Region

#Region "Constructor"

  ''' <summary>
  ''' The constructor.
  ''' </summary>
  Public Sub New(ByVal mdNumber As Integer, ByVal reportDataAccess As IReportDatabaseAccess)
    m_ReportDataAccess = reportDataAccess

    m_md = New Mandant

    m_MDNumber = mdNumber

  End Sub

#End Region

#Region "Private Properties"

  ''' <summary>
  ''' Gets the flexible time from database setting
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private ReadOnly Property GetflexibletimeFromDatabase As Boolean
    Get

      Dim FORM_XML_MAIN_KEY As String = "Forms_Normaly/Field_DefaultValues"
      Dim m_path As New SPProgUtility.ProgPath.ClsProgPath

      Dim value As Boolean? = m_Utility.ParseToBoolean(m_path.GetXMLNodeValue(m_md.GetSelectedMDFormDataXMLFilename(m_md.GetDefaultMDNr), String.Format("{0}/getflextimefrommandantdatabase", FORM_XML_MAIN_KEY)), False)

      Return value

    End Get
  End Property

#End Region

#Region "Public Methos"

  ''' <summary>
  ''' Determines the maximal working hours per working day.
  ''' </summary>
  ''' <param name="gavNumber">The gav number.</param>
  ''' <param name="gavStdWeek">The gav std week.</param>
  ''' <param name="rpYear">The report year.</param>
  ''' <returns>Maximal working hours in order to receive flexible time. Decimal.MaxValue is returned if no flexible time should be applied.</returns>
  ''' <remarks></remarks>
  Public Function DetermineMaximalWorkingHoursPerWorkingDay(ByVal gavNumber As Integer, ByVal gavStdWeek As Decimal, ByVal rpYear As Integer) As Decimal

    Dim value As Decimal = Decimal.MaxValue ' No flexible time

    If gavNumber > 0 Then

      If Not GetflexibletimeFromDatabase Then
        ' If is a new RPL data and a GAV is set then limit the working hours
        value = gavStdWeek / 5.0
      Else
        Dim numberOfWorkingHoursPerWeek As Decimal? = m_ReportDataAccess.LoadManantTSPLMVWorkingHoursPerWeek(gavNumber, rpYear)

        If numberOfWorkingHoursPerWeek.HasValue Then
          value = numberOfWorkingHoursPerWeek / 5D
        Else
          ' Fallback use GAVStdWeek from ES
          value = gavStdWeek / 5.0
        End If

      End If

    Else
      ' no flexible time
    End If

    Return value

  End Function

#End Region

End Class
