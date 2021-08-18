
Imports SPProgUtility.ProgPath
Imports SPProgUtility.MainUtilities
Imports SPProgUtility.Mandanten

Imports NLog


Namespace CommonSettings


  Public Class CommonSetting

    Private Shared logger As Logger = LogManager.GetCurrentClassLogger()

    Private m_Reg As ClsDivReg
    Private m_utilitiy As New Utilities


		Function GetLLLicenceInfo(ByVal iVersion As Integer) As String
			Dim strValue As String = String.Empty

			If iVersion = 20 Then Return "yourlicencekey"
			If iVersion = 18 Then Return "yourlicencekey"
			If iVersion = 17 Then Return "yourlicencekey"
			If iVersion = 16 Then Return "yourlicencekey"
			If iVersion = 15 Then Return "yourlicencekey"
			If iVersion = 14 Then Return "yourlicencekey"
			If iVersion = 13 Then Return "yourlicencekey"

			Return strValue
		End Function

		''' <summary>
		''' Seriennummer und Lizenzschlüssel für topTapi3
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetTapi3Data() As TapiData 'Implements iPath.ICommonSetting.GetTapi3Data
			Return New TapiData With {.TapiSerialnumber = "yourserialnumber",
								.TapiLicenseKey = "yourlicencekey"}
		End Function

		''' <summary>
		''' Setting for O2Solution
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetO2SolutionData() As O2SolutionData
			Return New O2SolutionData With {.PDFVWSerialnumber = "yourlicencekey",
									  .PDFSerialnumber = "yourlicencekey"}
		End Function


#Region "Userinfos..."

		''' <summary>
		''' Benutzervorname aus der Registry
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetLogedUserFirstName() As String 'Implements iPath.ICommonSetting.GetLogedUserFirstName
      m_Reg = New ClsDivReg
      Return m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname")
    End Function

    ''' <summary>
    ''' Benutzernachname aus der Registry
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetLogedUserLastName() As String 'Implements iPath.ICommonSetting.GetLogedUserLastName
      m_Reg = New ClsDivReg
      Return m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
    End Function

    ''' <summary>
    ''' Benutzernachname + , + Benutzervorname
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetLogedUserNameWithComma() As String 'Implements iPath.ICommonSetting.GetLogedUserNameWithComma
      Return String.Format("{0}, {1}", Me.GetLogedUserLastName, Me.GetLogedUserFirstName)
    End Function

    ''' <summary>
    ''' Benutzervorname und Nachname
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetLogedUserName() As String 'Implements iPath.ICommonSetting.GetLogedUserName
      Return String.Format("{0} {1}", Me.GetLogedUserFirstName, Me.GetLogedUserLastName())
    End Function

    ''' <summary>
    ''' Benutzer EMail-Adresse aus der Registry
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetLogedUserMail() As String 'Implements iPath.ICommonSetting.GetLogedUserMail
      m_Reg = New ClsDivReg
      Return m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "spUsereMail")
    End Function

    ''' <summary>
    ''' Angemeldete Benutzernummer aus der Registry
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetLogedUserNr() As Integer 'Implements iPath.ICommonSetting.GetLogedUserNr
      m_Reg = New ClsDivReg
      Return CInt(m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "UserNr").ToString)
    End Function

    ''' <summary>
    ''' Die Guid-Nummer der jeweiligen Benutzer. Wird aus Registry gelesen ("\Sputnik Suite\ProgOptions", "MyGuid")
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetLogedUserGuid() As String 'Implements iPath.ICommonSetting.GetLogedUserGuid
      m_Reg = New ClsDivReg
      Return m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "MyGuid").ToString
    End Function

    ''' <summary>
    ''' Benutzer Sprache aus der Registry
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetLogedUserLanguage() As String 'Implements iPath.ICommonSetting.GetLogedUserLanguage
      m_Reg = New ClsDivReg
      Return m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USLanguage")
    End Function


    ''' <summary>
    ''' Filiale der angemeldete Benutzer für Datenfreigabe
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetLogedUserFiliale() As String 'Implements iPath.ICommonSetting.GetUSFiliale
      m_Reg = New ClsDivReg
      Return m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USFiliale")
    End Function

    Function GetLogedUserKst() As String 'Implements iPath.ICommonSetting.GetLogedUserKst
      m_Reg = New ClsDivReg
      Return m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USKSt")
    End Function

    ''' <summary>
    ''' Haupt Mandantennummer der angemeldete Benutzer
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetLogedUserMainMDNr() As String 'Implements iPath.ICommonSetting.GetLogedUserMainMDNr
      m_Reg = New ClsDivReg
      Return m_Reg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USMainMDNr")
    End Function

#End Region



  End Class



End Namespace
