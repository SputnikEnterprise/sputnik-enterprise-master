Imports System.Text.RegularExpressions
Imports SP.Infrastructure.DateAndTimeCalculation
Imports SP.Infrastructure.Initialization

Public Class TimeTableDayData

#Region "Private Fields"

	''' <summary>
	''' The translation value helper.
	''' </summary>
	Private m_translate As SP.Infrastructure.Initialization.TranslateValuesHelper

	''' <summary>
	''' Rundungsart für Arbeitszeit
	''' </summary>
	''' <remarks></remarks>
	Private m_roundKind As DateAndTimeUtily.HoursRoundKind

	Private m_workHours As Decimal?

	Private m_showMinutes As Boolean = False

#End Region ' Private Fields

#Region "Constructor"

	''' <summary>
	''' The constructor.
	''' </summary>
	''' <param name="translate">The translation object.</param>
	''' <param name="roundKind">Rundungsart für WorkHours. Default ist Minuten.</param>
	Public Sub New(ByVal translate As TranslateValuesHelper, Optional ByVal roundKind As DateAndTimeUtily.HoursRoundKind = DateAndTimeUtily.HoursRoundKind.Minutes)
		m_translate = translate
		m_roundKind = roundKind
	End Sub

#End Region ' Constructor

#Region "Public Properties"

	Public Property DayDate As DateTime

	Public Property WorkHours As Decimal?
		Get
			Return m_workHours
		End Get
		Set(value As Decimal?)
			If (value.HasValue) Then
				' Runden und auf Bereich 0...24h limitieren
				' TODO: Bereich Min/Max konfigurierbar
				Dim workHours As Decimal = Math.Min(Math.Max(DateAndTimeUtily.RoundHourValue(value.Value, m_roundKind), 0D), 24D)
				' Null wenn 0 Stunden
				m_workHours = If(workHours > 0D, workHours, Nothing)
			Else
				m_workHours = Nothing
			End If
		End Set
	End Property

	Public Property ShowMinutes As Boolean
		Get
			Return m_showMinutes
		End Get
		Set(value As Boolean)
			m_showMinutes = value
		End Set
	End Property

	Public Property WorkHourEditText As String
		Get
			If m_workHours.GetValueOrDefault(0D) > 0D Then
				If m_showMinutes Then
					Return String.Format("{0:0}:{1:00}", Math.Floor(m_workHours.Value), (m_workHours.Value Mod 1) * 60)
				Else
					Return String.Format("{0:0.00}", m_workHours.Value)
				End If
			Else
				Return String.Empty
			End If
		End Get
		Set(value As String)
			If String.IsNullOrEmpty(value) Then
				Me.WorkHours = Nothing
			Else
				value = String.Format("{0:0.00}", Val(Regex.Replace(value.ToString, "[;:,_-]", ".")))
				'value = String.Format("{0:0.00}", Val(value))
				value = value.Trim()
				If value.Length > 0 Then
					If m_showMinutes Then
						' String im Format 0:00
						Dim hours As Integer = 0
						Dim minutes As Integer = 0
						Dim valueParts() As String = value.Split(New Char() {":", ".", ","})
						If valueParts.Length >= 1 Then Integer.TryParse(valueParts(0), hours)
						If valueParts.Length >= 2 Then Integer.TryParse(valueParts(1), minutes)
						Me.WorkHours = Convert.ToDecimal(hours) + Convert.ToDecimal(minutes) / 60
					Else
						' String im Format 0.00
						Dim workhours As Decimal
						If Decimal.TryParse(value, workhours) Then
							Me.WorkHours = workhours
						Else
							Me.WorkHours = Nothing
						End If
					End If
				Else
					' Leerstring
					Me.WorkHours = Nothing
				End If
			End If
		End Set
	End Property

	Public Property FehlzeitCode As String
	Public Property Tagesspesen As Boolean
	Public Property Stundenspesen As Boolean


	''' <summary>
	''' Gets the translated day of week text.
	''' </summary>
	''' <returns>The translated day of week text.</returns>
	Public ReadOnly Property TranslatedDayOfWeekText As String
		Get

			Select Case DayDate.DayOfWeek
				Case System.DayOfWeek.Monday
					Return m_translate.GetSafeTranslationValue("Mo")
				Case System.DayOfWeek.Tuesday
					Return m_translate.GetSafeTranslationValue("Di")
				Case System.DayOfWeek.Wednesday
					Return m_translate.GetSafeTranslationValue("Mi")
				Case System.DayOfWeek.Thursday
					Return m_translate.GetSafeTranslationValue("Do")
				Case System.DayOfWeek.Friday
					Return m_translate.GetSafeTranslationValue("Fr")
				Case System.DayOfWeek.Saturday
					Return m_translate.GetSafeTranslationValue("Sa")
				Case System.DayOfWeek.Sunday
					Return m_translate.GetSafeTranslationValue("So")
				Case Else
					Return String.Empty
			End Select

		End Get
	End Property

	Public ReadOnly Property CalendarWeek As Integer
		Get

			Dim iFWeek As Integer = DatePart(DateInterval.WeekOfYear, DayDate,
								   FirstDayOfWeek.System, FirstWeekOfYear.System)

			Return iFWeek

		End Get
	End Property

#End Region ' Public Properties

End Class