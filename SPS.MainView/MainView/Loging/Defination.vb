
Imports SPProgUtility.SPTranslation.ClsTranslation

Imports System.Data.SqlClient
Imports System.Collections.Generic

Module Defination

  Public _ClsData As New ClsPublicData

  Public Const strEncryptionKey As String = "your crypt key"
	Public Const strExtraPass As String = "yourseckey"

	Public Stamm_Db As SqlConnection

	Public RegStr As String = "Software\yourregistrykeyname\Sputnik Suite"		' Der Name des Reg.schlüssels
	Public RegStr_Net As String = "Software\yourregistrykeyname\Sputnik Suite"		' Der Name des Reg.schlüssels
	Public strSaveRegKey As String = "Software\yourregistrykeyname\Sputnik Suite" & "\Save"		' Der Name des Reg.schlüssels zum Sichern

  Public Property SelMDYearPath As String                  ' Selectierter Mandant
  Public Property SelMDMainPath As String                  ' Selectierter Mandant
  Public Property TemplatesPath As String
  Public Property SelMDSkinPath As String
  Public Property ExistsCurrentYearData() As Boolean

  Dim strCustomer_ID As String
  Public Property SelMDCustomer_ID() As String
    Get
      Return strCustomer_ID
    End Get
    Set(ByVal value As String)
      If strCustomer_ID = value Then
        Return
      End If
      strCustomer_ID = value
    End Set
  End Property

  Public Property SelectedDbName() As String
  Public Property SelectedMDName() As String
	Public Property SelectedMDGroupNr() As Integer
  Public Property SelectedFileServerPath() As String

  Public TranslationData As Dictionary(Of String, SPProgUtility.ClsTranslationData)


	Public IniFile As String = "PROGRAMM.DAT"	 ' Die Einstellungsdatei für den Mandanten

  Public SrvBinPath As String
  Public SrvSettingFile As String
  Public SrvSettingFullFileName As String
  Public ShowButtonInHead As Boolean
  Public GetUserColorSetting As Short
  '==========================================================================

  Public SQLDateformat As String ' Die Datumsformat wenn es SQL-Server/Desktop

End Module

