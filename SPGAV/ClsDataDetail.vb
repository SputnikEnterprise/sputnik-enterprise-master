
Imports System.Windows.Forms
Imports System.Data.SqlClient

Public Class ClsDataDetail

  Public Shared strGAVData As String
  Public Shared _ClsProgSetting As New SPProgUtility.ClsProgSettingPath


	''' <summary>
	''' The translation value helper.
	''' </summary>
	Public Shared m_Translate As SP.Infrastructure.Initialization.TranslateValuesHelper
	Public Shared m_InitialData As SP.Infrastructure.Initialization.InitializeClass


  Public Shared ReadOnly Property GetAppGuidValue() As String
    Get
      Return "581B1795-B04A-410c-B514-35C888EFC03E"
    End Get
  End Property

  ' Helps extracting a column value form a data reader.
  Public Shared Function GetColumnTextStr(ByVal dr As SqlDataReader, _
                                          ByVal columnName As String, _
                                          ByVal replacementOnNull As String) As String

    If Not dr.IsDBNull(dr.GetOrdinal(columnName)) Then
      Return CStr(dr(columnName))
    End If

    Return replacementOnNull
  End Function

  ' Helps extracting a column value form a data reader.
  Public Shared Function GetColumnTextStr(ByVal dataRow As DataRow, _
                                          ByVal columnName As String, _
                                          ByVal replacementOnNull As String) As String

    If Not dataRow.IsNull(columnName) Then
      Return CStr(dataRow(columnName))
    End If

    Return replacementOnNull
  End Function

  Shared _liESGAVData As New List(Of String)
  Public Shared Property SelectedGAVData() As List(Of String)
    Get
      Return _liESGAVData
    End Get
    Set(ByVal value As List(Of String))
      _liESGAVData = value
    End Set
  End Property

  Shared _iMANr As Integer
  Public Shared Property SelectedMANumber() As Integer
    Get
      Return _iMANr
    End Get
    Set(ByVal value As Integer)
      _iMANr = value
    End Set
  End Property
  Shared _iKDNr As Integer
  Public Shared Property SelectedKDNumber() As Integer
    Get
      Return _iKDNr
    End Get
    Set(ByVal value As Integer)
      _iKDNr = value
    End Set
  End Property
  Shared _iESNr As Integer
  Public Shared Property SelectedESNumber() As Integer
    Get
      Return _iESNr
    End Get
    Set(ByVal value As Integer)
      _iESNr = value
    End Set
  End Property
  Shared _strModulName As String
  Public Shared Property SelectedModulName() As String
    Get
      Return _strModulName
    End Get
    Set(ByVal value As String)
      _strModulName = value
    End Set
  End Property
  Shared _strKanton As String
  Public Shared Property SelectedKDKanton() As String
    Get
      Return _strKanton
    End Get
    Set(ByVal value As String)
      _strKanton = value
    End Set
  End Property
  Shared _strOldGAVInfo As String
  Public Shared Property GetOldGAVInfo() As String
    Get
      Return _strOldGAVInfo
    End Get
    Set(ByVal value As String)
      _strOldGAVInfo = value
    End Set
  End Property

  Shared _strLanguage As String = "DE"
  Public Shared Property SelectedLanguage() As String
    Get
      Return _strLanguage
    End Get
    Set(ByVal value As String)
      _strLanguage = value
    End Set
  End Property



  '// Query für Datensuche
  Shared _strConnString As String
  Public Shared Property GetDbConnString() As String
    Get
      Dim _strConnString As String = _ClsProgSetting.GetConnString()

      Return (_strConnString)
    End Get
    Set(ByVal value As String)
      _strConnString = value
    End Set
  End Property
End Class


''' <summary>
''' Klasse für die ComboBox, um Text und Wert zu haben.
''' Das Item wird mit den Parameter Text für die Anzeige und
''' Value für den Wert zur ComboBox hinzugefügt.
''' </summary>
''' <remarks></remarks>
Class ComboBoxItem
  Public Text As String
  Public Value_0 As String
  Public Value_1 As String
  Public Value_2 As String
  Public Value_3 As String
  Public Value_4 As String
  Public Value_5 As String
  Public Value_6 As String
  Public Value_7 As String
  Public Value_8 As String
  Public Value_9 As String
  Public Value_10 As String
  Public Value_11 As String
  Public Value_12 As String

  Public Value_13 As String
  Public Value_14 As String
  Public Value_15 As String

  Public Value_16 As String
  Public Value_17 As String

  Public Value_18 As String
  Public Value_19 As String
  Public Value_20 As String
  Public Value_21 As String
  Public Value_22 As String

  Public Value_23 As String
  Public Value_24 As String
  Public Value_25 As String
  Public Value_26 As String
  Public Value_27 As String
  Public Value_28 As String

  Public Value_29 As String
  Public Value_30 As String
  Public Value_31 As String
  Public Value_32 As String
  Public Value_33 As String
  Public Value_34 As String
	Public Value_35 As String


  Public Sub New(ByVal text As String, ByVal val As String)
    Me.Text = text
    Me.Value_0 = val
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, ByVal val_1 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
                 ByVal val_1 As String, ByVal val_2 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
                 ByVal val_1 As String, ByVal val_2 As String, _
                 ByVal val_3 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
               ByVal val_1 As String, ByVal val_2 As String, _
               ByVal val_3 As String, ByVal val_4 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
             ByVal val_1 As String, ByVal val_2 As String, _
             ByVal val_3 As String, ByVal val_4 As String, ByVal val_5 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
    Me.Value_5 = val_5
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
             ByVal val_1 As String, ByVal val_2 As String, _
             ByVal val_3 As String, ByVal val_4 As String, _
             ByVal val_5 As String, ByVal val_6 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
    Me.Value_5 = val_5
    Me.Value_6 = val_6
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
             ByVal val_1 As String, ByVal val_2 As String, _
             ByVal val_3 As String, ByVal val_4 As String, _
             ByVal val_5 As String, ByVal val_6 As String, ByVal val_7 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
    Me.Value_5 = val_5
    Me.Value_6 = val_6
    Me.Value_7 = val_7
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
             ByVal val_1 As String, ByVal val_2 As String, _
             ByVal val_3 As String, ByVal val_4 As String, _
             ByVal val_5 As String, ByVal val_6 As String, _
             ByVal val_7 As String, ByVal val_8 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
    Me.Value_5 = val_5
    Me.Value_6 = val_6

    Me.Value_7 = val_7
    Me.Value_8 = val_8
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
             ByVal val_1 As String, ByVal val_2 As String, _
             ByVal val_3 As String, ByVal val_4 As String, _
             ByVal val_5 As String, ByVal val_6 As String, _
             ByVal val_7 As String, ByVal val_8 As String, ByVal val_9 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
    Me.Value_5 = val_5
    Me.Value_6 = val_6
    Me.Value_7 = val_7
    Me.Value_8 = val_8
    Me.Value_9 = val_9
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
             ByVal val_1 As String, ByVal val_2 As String, _
             ByVal val_3 As String, ByVal val_4 As String, _
             ByVal val_5 As String, ByVal val_6 As String, _
             ByVal val_7 As String, ByVal val_8 As String, _
             ByVal val_9 As String, ByVal val_10 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
    Me.Value_5 = val_5
    Me.Value_6 = val_6
    Me.Value_7 = val_7
    Me.Value_8 = val_8
    Me.Value_9 = val_9
    Me.Value_10 = val_10

  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
             ByVal val_1 As String, ByVal val_2 As String, _
             ByVal val_3 As String, ByVal val_4 As String, _
             ByVal val_5 As String, ByVal val_6 As String, _
             ByVal val_7 As String, ByVal val_8 As String, _
             ByVal val_9 As String, ByVal val_10 As String, ByVal val_11 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
    Me.Value_5 = val_5
    Me.Value_6 = val_6
    Me.Value_7 = val_7
    Me.Value_8 = val_8
    Me.Value_9 = val_9
    Me.Value_10 = val_10
    Me.Value_11 = val_11

  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
             ByVal val_1 As String, ByVal val_2 As String, _
             ByVal val_3 As String, ByVal val_4 As String, _
             ByVal val_5 As String, ByVal val_6 As String, _
             ByVal val_7 As String, ByVal val_8 As String, _
             ByVal val_9 As String, ByVal val_10 As String, _
             ByVal val_11 As String, ByVal val_12 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
    Me.Value_5 = val_5
    Me.Value_6 = val_6
    Me.Value_7 = val_7
    Me.Value_8 = val_8
    Me.Value_9 = val_9
    Me.Value_10 = val_10
    Me.Value_11 = val_11
    Me.Value_12 = val_12

  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
             ByVal val_1 As String, ByVal val_2 As String, _
             ByVal val_3 As String, ByVal val_4 As String, _
             ByVal val_5 As String, ByVal val_6 As String, _
             ByVal val_7 As String, ByVal val_8 As String, _
             ByVal val_9 As String, ByVal val_10 As String, _
             ByVal val_11 As String, ByVal val_12 As String, _
             ByVal val_13 As String, ByVal val_14 As String, _
             ByVal val_15 As String, ByVal val_16 As String, _
             ByVal val_17 As String, ByVal val_18 As String, _
 _
             ByVal val_19 As String, ByVal val_20 As String, _
             ByVal val_21 As String, ByVal val_22 As String, _
             ByVal val_23 As String, ByVal val_24 As String, _
 _
             ByVal val_25 As String, ByVal val_26 As String, _
             ByVal val_27 As String, ByVal val_28 As String, _
             ByVal val_29 As String, ByVal val_30 As String, _
             ByVal val_31 As String, ByVal val_32 As String)

    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
    Me.Value_5 = val_5
    Me.Value_6 = val_6
    Me.Value_7 = val_7
    Me.Value_8 = val_8
    Me.Value_9 = val_9
    Me.Value_10 = val_10
    Me.Value_11 = val_11
    Me.Value_12 = val_12

    Me.Value_13 = val_13
    Me.Value_14 = val_14
    Me.Value_15 = val_15

    Me.Value_16 = val_16
    Me.Value_17 = val_17
    Me.Value_18 = val_18

    Me.Value_19 = val_19
    Me.Value_20 = val_20
    Me.Value_21 = val_21
    Me.Value_22 = val_22
    Me.Value_23 = val_23
    Me.Value_24 = val_24

    Me.Value_25 = val_25
    Me.Value_26 = val_26
    Me.Value_27 = val_27
    Me.Value_28 = val_28
    Me.Value_29 = val_29
    Me.Value_30 = val_30
    Me.Value_31 = val_31
    Me.Value_32 = val_32

  End Sub

	Public Sub New(ByVal text As String, ByVal val_0 As String, _
					 ByVal val_1 As String, ByVal val_2 As String, _
					 ByVal val_3 As String, ByVal val_4 As String, _
					 ByVal val_5 As String, ByVal val_6 As String, _
					 ByVal val_7 As String, ByVal val_8 As String, _
					 ByVal val_9 As String, ByVal val_10 As String, _
					 ByVal val_11 As String, ByVal val_12 As String, _
					 ByVal val_13 As String, ByVal val_14 As String, _
					 ByVal val_15 As String, ByVal val_16 As String, _
					 ByVal val_17 As String, ByVal val_18 As String, _
 _
					 ByVal val_19 As String, ByVal val_20 As String, _
					 ByVal val_21 As String, ByVal val_22 As String, _
					 ByVal val_23 As String, ByVal val_24 As String, _
 _
					 ByVal val_25 As String, ByVal val_26 As String, _
					 ByVal val_27 As String, ByVal val_28 As String, _
					 ByVal val_29 As String, ByVal val_30 As String, _
					 ByVal val_31 As String, ByVal val_32 As String,
					 ByVal val_33 As String, ByVal val_34 As String)

		Me.Text = text
		Me.Value_0 = val_0
		Me.Value_1 = val_1
		Me.Value_2 = val_2
		Me.Value_3 = val_3
		Me.Value_4 = val_4
		Me.Value_5 = val_5
		Me.Value_6 = val_6
		Me.Value_7 = val_7
		Me.Value_8 = val_8
		Me.Value_9 = val_9
		Me.Value_10 = val_10
		Me.Value_11 = val_11
		Me.Value_12 = val_12

		Me.Value_13 = val_13
		Me.Value_14 = val_14
		Me.Value_15 = val_15

		Me.Value_16 = val_16
		Me.Value_17 = val_17
		Me.Value_18 = val_18

		Me.Value_19 = val_19
		Me.Value_20 = val_20
		Me.Value_21 = val_21
		Me.Value_22 = val_22
		Me.Value_23 = val_23
		Me.Value_24 = val_24

		Me.Value_25 = val_25
		Me.Value_26 = val_26
		Me.Value_27 = val_27
		Me.Value_28 = val_28
		Me.Value_29 = val_29
		Me.Value_30 = val_30
		Me.Value_31 = val_31
		Me.Value_32 = val_32
		Me.Value_33 = val_33
		Me.Value_34 = val_34

	End Sub


  Public Overrides Function ToString() As String
    Return Text
  End Function

End Class


''' <summary>
''' Klasse für die ComboBox, um Text und Wert zu haben.
''' Das Item wird mit den Parameter Text für die Anzeige und
''' Value für den Wert zur ComboBox hinzugefügt.
''' </summary>
''' <remarks></remarks>
Class TextBoxItem
  Public Text As String
  Public Value_0 As String
  Public Value_1 As String
  Public Value_2 As String
  Public Value_3 As String
  Public Value_4 As String
  Public Value_5 As String

  Public Sub New(ByVal text As String, ByVal val As String)
    Me.Text = text
    Me.Value_0 = val
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, ByVal val_1 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
                 ByVal val_1 As String, ByVal val_2 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
                 ByVal val_1 As String, ByVal val_2 As String, _
                 ByVal val_3 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
               ByVal val_1 As String, ByVal val_2 As String, _
               ByVal val_3 As String, ByVal val_4 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
  End Sub

  Public Sub New(ByVal text As String, ByVal val_0 As String, _
             ByVal val_1 As String, ByVal val_2 As String, _
             ByVal val_3 As String, ByVal val_4 As String, ByVal val_5 As String)
    Me.Text = text
    Me.Value_0 = val_0
    Me.Value_1 = val_1
    Me.Value_2 = val_2
    Me.Value_3 = val_3
    Me.Value_4 = val_4
    Me.Value_5 = val_5
  End Sub


  Public Overrides Function ToString() As String
    Return Text
  End Function

End Class
