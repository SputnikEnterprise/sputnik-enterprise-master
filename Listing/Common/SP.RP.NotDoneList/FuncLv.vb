
'Imports System.Data.SqlClient
'Imports System.IO
'Imports System.Text.RegularExpressions
'Imports System.Reflection
'Imports SPProgUtility.SPTranslation.ClsTranslation
'Imports System
'Imports System.Drawing
'Imports System.Collections
'Imports System.ComponentModel
'Imports System.Windows.Forms
'Imports System.Data
'Imports SPProgUtility.MainUtilities
'Imports SP.Infrastructure.Messaging
'Imports SP.Infrastructure.Messaging.Messages
'Imports SP.Infrastructure.Logging

'Module FuncLv

'	''' <summary>
'	''' The logger.
'	''' </summary>
'	Private m_Logger As ILogger = New Logger()

'	Private _ClsFunc As New ClsDivFunc
'	Private _ClsReg As New SPProgUtility.ClsDivReg


'	Function CreateInitialData(ByVal iMDNr As Integer, ByVal iLogedUSNr As Integer) As SP.Infrastructure.Initialization.InitializeClass

'		Dim m_md As New SPProgUtility.Mandanten.Mandant
'		Dim clsMandant = m_md.GetSelectedMDData(iMDNr)
'		Dim logedUserData = m_md.GetSelectedUserData(iMDNr, iLogedUSNr)
'		Dim personalizedData = m_md.GetPersonalizedCaptionInObject(iMDNr)

'		Dim clsTransalation As New SPProgUtility.SPTranslation.ClsTranslation
'		Dim translate = clsTransalation.GetTranslationInObject

'		Return New SP.Infrastructure.Initialization.InitializeClass(translate, personalizedData, clsMandant, logedUserData)

'	End Function






'#Region "Sonstige Funktions..."




'#End Region




'End Module
