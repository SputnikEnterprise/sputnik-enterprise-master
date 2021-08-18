Imports TinyMessenger

Namespace Messaging.Messages

  ''' <summary>
  ''' Notify refresh reponsible person address.
  ''' </summary>
	Public Class RefreshMainViewStatebar
		Inherits TinyMessageBase

#Region "Private Fields"

		''' <summary>
		''' The user number.
		''' </summary>
		Private m_Recordcount As Integer?

		''' <summary>
		''' The mandant number.
		''' </summary>
		Private m_MDNr As Integer?

		Private m_Mandantname As String
		Private m_MandantCity As String
		Private m_MandantPath As String
		Private m_MandantDocPath As String
		Private m_MandantUsername As String
		Private m_DbServer As String
		Private m_DbName As String

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New(ByVal sender As Object,
									 ByVal Recordcount As Integer?,
									 ByVal MDNr As Integer?,
									 ByVal Mandantname As String,
									 ByVal MandantCity As String,
									 ByVal MandantPath As String,
									 ByVal MandantDocPath As String,
									 ByVal MandantUsername As String,
									 ByVal DbServer As String,
									 ByVal DbName As String)
			MyBase.New(sender)

			m_Recordcount = Recordcount
			m_MDNr = MDNr
			m_Mandantname = Mandantname
			m_MandantCity = MandantCity
			m_MandantPath = MandantPath
			m_MandantDocPath = MandantDocPath
			m_MandantUsername = MandantUsername
			m_DbServer = DbServer
			m_DbName = DbName

		End Sub

#End Region

#Region "Public Properties"

		Public ReadOnly Property Recordcount As Integer?
			Get
				Return m_Recordcount
			End Get
		End Property

		''' <summary>
		''' Gets the mandant number.
		''' </summary>
		''' <returns>The mandant number.</returns>
		Public ReadOnly Property MDNr As Integer?
			Get
				Return m_MDNr
			End Get
		End Property

		Public ReadOnly Property Mandantname As String
			Get
				Return m_Mandantname
			End Get
		End Property

		Public ReadOnly Property MandantCity As String
			Get
				Return m_MandantCity
			End Get
		End Property

		Public ReadOnly Property MandantPath As String
			Get
				Return m_MandantPath
			End Get
		End Property

		Public ReadOnly Property MandantDocPath As String
			Get
				Return m_MandantDocPath
			End Get
		End Property

		Public ReadOnly Property MandantUsername As String
			Get
				Return m_MandantUsername
			End Get
		End Property

		Public ReadOnly Property DbServer As String
			Get
				Return m_DbServer
			End Get
		End Property

		Public ReadOnly Property DbName As String
			Get
				Return m_DbName
			End Get
		End Property


#End Region

	End Class
End Namespace
