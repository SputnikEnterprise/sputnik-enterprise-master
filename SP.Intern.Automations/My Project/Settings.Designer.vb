'------------------------------------------------------------------------------
' <auto-generated>
'     Dieser Code wurde von einem Tool generiert.
'     Laufzeitversion:4.0.30319.42000
'
'     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
'     der Code erneut generiert wird.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On



<Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
 Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0"),  _
 Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
Partial Friend NotInheritable Class Settings
    Inherits Global.System.Configuration.ApplicationSettingsBase
    
    Private Shared defaultInstance As Settings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New Settings()),Settings)
    
#Region "Funktion zum automatischen Speichern von My.Settings"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(ByVal sender As Global.System.Object, ByVal e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
    
    Public Shared ReadOnly Property [Default]() As Settings
        Get
            
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
            Return defaultInstance
        End Get
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property ecallLogs_ifrmWidth() As Integer
        Get
            Return CType(Me("ecallLogs_ifrmWidth"),Integer)
        End Get
        Set
            Me("ecallLogs_ifrmWidth") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property ecallLogs_ifrmHeight() As Integer
        Get
            Return CType(Me("ecallLogs_ifrmHeight"),Integer)
        End Get
        Set
            Me("ecallLogs_ifrmHeight") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property ecallLogs_frmLocation() As String
        Get
            Return CType(Me("ecallLogs_frmLocation"),String)
        End Get
        Set
            Me("ecallLogs_frmLocation") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("Password=password;Persist Security Info=True;User ID=GAVUser;Initial Catalog=Sputnik "& _ 
        "InfoSystem;Data Source=dbserver")>  _
    Public Property Connstr_InfoService() As String
        Get
            Return CType(Me("Connstr_InfoService"),String)
        End Get
        Set
            Me("Connstr_InfoService") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("Password=password;Persist Security Info=True;User ID=username;I"& _ 
        "nitial Catalog=Sputnik InfoSystem;Data Source=dbserver")>  _
    Public Property ConnStr_SputnikInfoSystem() As String
        Get
            Return CType(Me("ConnStr_SputnikInfoSystem"),String)
        End Get
        Set
            Me("ConnStr_SputnikInfoSystem") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("Password=password;Persist Security Info=True;User ID=username;I"& _ 
        "nitial Catalog=spContract;Data Source=dbserver")>  _
    Public Property ConnStr_JobCh() As String
        Get
            Return CType(Me("ConnStr_JobCh"),String)
        End Get
        Set
            Me("ConnStr_JobCh") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property ecallLogs_lastselectedservice() As String
        Get
            Return CType(Me("ecallLogs_lastselectedservice"),String)
        End Get
        Set
            Me("ecallLogs_lastselectedservice") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property pm_ifrmWidth() As Integer
        Get
            Return CType(Me("pm_ifrmWidth"),Integer)
        End Get
        Set
            Me("pm_ifrmWidth") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
    Public Property pm_ifrmHeight() As Integer
        Get
            Return CType(Me("pm_ifrmHeight"),Integer)
        End Get
        Set
            Me("pm_ifrmHeight") = value
        End Set
    End Property
    
    <Global.System.Configuration.UserScopedSettingAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Configuration.DefaultSettingValueAttribute("")>  _
    Public Property pm_frmLocation() As String
        Get
            Return CType(Me("pm_frmLocation"),String)
        End Get
        Set
            Me("pm_frmLocation") = value
        End Set
    End Property
End Class

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.SP.Internal.Automations.Settings
            Get
                Return Global.SP.Internal.Automations.Settings.Default
            End Get
        End Property
    End Module
End Namespace
