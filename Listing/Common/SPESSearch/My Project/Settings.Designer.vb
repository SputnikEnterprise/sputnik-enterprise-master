﻿'------------------------------------------------------------------------------
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


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
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
        
        Public Shared ReadOnly Property [Default]() As MySettings
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
        Public Property ifrmWidth() As Integer
            Get
                Return CType(Me("ifrmWidth"),Integer)
            End Get
            Set
                Me("ifrmWidth") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ifrmHeight() As Integer
            Get
                Return CType(Me("ifrmHeight"),Integer)
            End Get
            Set
                Me("ifrmHeight") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property frm_Location() As String
            Get
                Return CType(Me("frm_Location"),String)
            End Get
            Set
                Me("frm_Location") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property LV_1_Size_ESSearch() As String
            Get
                Return CType(Me("LV_1_Size_ESSearch"),String)
            End Get
            Set
                Me("LV_1_Size_ESSearch") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property LV_2_Size_ESSearch() As String
            Get
                Return CType(Me("LV_2_Size_ESSearch"),String)
            End Get
            Set
                Me("LV_2_Size_ESSearch") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property LV_3_Size_ESSearch() As String
            Get
                Return CType(Me("LV_3_Size_ESSearch"),String)
            End Get
            Set
                Me("LV_3_Size_ESSearch") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property LV_4_Size_ESSearch() As String
            Get
                Return CType(Me("LV_4_Size_ESSearch"),String)
            End Get
            Set
                Me("LV_4_Size_ESSearch") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property LV_5_Size_ESSearch() As String
            Get
                Return CType(Me("LV_5_Size_ESSearch"),String)
            End Get
            Set
                Me("LV_5_Size_ESSearch") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property LV_6_Size_ESSearch() As String
            Get
                Return CType(Me("LV_6_Size_ESSearch"),String)
            End Get
            Set
                Me("LV_6_Size_ESSearch") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property Filename4CSV() As String
            Get
                Return CType(Me("Filename4CSV"),String)
            End Get
            Set
                Me("Filename4CSV") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property ExportFields4KD_CSV() As String
            Get
                Return CType(Me("ExportFields4KD_CSV"),String)
            End Get
            Set
                Me("ExportFields4KD_CSV") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property ExportFields4MA_CSV() As String
            Get
                Return CType(Me("ExportFields4MA_CSV"),String)
            End Get
            Set
                Me("ExportFields4MA_CSV") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property TrennzeichenCSV() As String
            Get
                Return CType(Me("TrennzeichenCSV"),String)
            End Get
            Set
                Me("TrennzeichenCSV") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property DarstellungszeichenCSV() As String
            Get
                Return CType(Me("DarstellungszeichenCSV"),String)
            End Get
            Set
                Me("DarstellungszeichenCSV") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property ExportFields() As String
            Get
                Return CType(Me("ExportFields"),String)
            End Get
            Set
                Me("ExportFields") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property iESSearchWidth() As Integer
            Get
                Return CType(Me("iESSearchWidth"),Integer)
            End Get
            Set
                Me("iESSearchWidth") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property iESSearchHeight() As Integer
            Get
                Return CType(Me("iESSearchHeight"),Integer)
            End Get
            Set
                Me("iESSearchHeight") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property frmLocation_LVESSearch() As String
            Get
                Return CType(Me("frmLocation_LVESSearch"),String)
            End Get
            Set
                Me("frmLocation_LVESSearch") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property iLVESSearch_Height() As Integer
            Get
                Return CType(Me("iLVESSearch_Height"),Integer)
            End Get
            Set
                Me("iLVESSearch_Height") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property iLVESSearch_Width() As Integer
            Get
                Return CType(Me("iLVESSearch_Width"),Integer)
            End Get
            Set
                Me("iLVESSearch_Width") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property SortBez() As String
            Get
                Return CType(Me("SortBez"),String)
            End Get
            Set
                Me("SortBez") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ifrmLVWidth() As Integer
            Get
                Return CType(Me("ifrmLVWidth"),Integer)
            End Get
            Set
                Me("ifrmLVWidth") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ifrmLVHeight() As Integer
            Get
                Return CType(Me("ifrmLVHeight"),Integer)
            End Get
            Set
                Me("ifrmLVHeight") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property frm_LVLocation() As String
            Get
                Return CType(Me("frm_LVLocation"),String)
            End Get
            Set
                Me("frm_LVLocation") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_KDNr() As String
            Get
                Return CType(Me("bShowColumn_KDNr"),String)
            End Get
            Set
                Me("bShowColumn_KDNr") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_ESNr() As String
            Get
                Return CType(Me("bShowColumn_ESNr"),String)
            End Get
            Set
                Me("bShowColumn_ESNr") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_MANr() As String
            Get
                Return CType(Me("bShowColumn_MANr"),String)
            End Get
            Set
                Me("bShowColumn_MANr") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_esals() As String
            Get
                Return CType(Me("bShowColumn_esals"),String)
            End Get
            Set
                Me("bShowColumn_esals") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_esbeginn() As String
            Get
                Return CType(Me("bShowColumn_esbeginn"),String)
            End Get
            Set
                Me("bShowColumn_esbeginn") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_esend() As String
            Get
                Return CType(Me("bShowColumn_esend"),String)
            End Get
            Set
                Me("bShowColumn_esend") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_esbeginnend() As String
            Get
                Return CType(Me("bShowColumn_esbeginnend"),String)
            End Get
            Set
                Me("bShowColumn_esbeginnend") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_gavqualification() As String
            Get
                Return CType(Me("bShowColumn_gavqualification"),String)
            End Get
            Set
                Me("bShowColumn_gavqualification") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_esmargewithbvg() As String
            Get
                Return CType(Me("bShowColumn_esmargewithbvg"),String)
            End Get
            Set
                Me("bShowColumn_esmargewithbvg") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_esmargewithoutbvg() As String
            Get
                Return CType(Me("bShowColumn_esmargewithoutbvg"),String)
            End Get
            Set
                Me("bShowColumn_esmargewithoutbvg") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_estarif() As String
            Get
                Return CType(Me("bShowColumn_estarif"),String)
            End Get
            Set
                Me("bShowColumn_estarif") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_esstdlohn() As String
            Get
                Return CType(Me("bShowColumn_esstdlohn"),String)
            End Get
            Set
                Me("bShowColumn_esstdlohn") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_employeename() As String
            Get
                Return CType(Me("bShowColumn_employeename"),String)
            End Get
            Set
                Me("bShowColumn_employeename") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_employeeaddress() As String
            Get
                Return CType(Me("bShowColumn_employeeaddress"),String)
            End Get
            Set
                Me("bShowColumn_employeeaddress") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_employeeemail() As String
            Get
                Return CType(Me("bShowColumn_employeeemail"),String)
            End Get
            Set
                Me("bShowColumn_employeeemail") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_employeepermission() As String
            Get
                Return CType(Me("bShowColumn_employeepermission"),String)
            End Get
            Set
                Me("bShowColumn_employeepermission") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_employeepermissionuntil() As String
            Get
                Return CType(Me("bShowColumn_employeepermissionuntil"),String)
            End Get
            Set
                Me("bShowColumn_employeepermissionuntil") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_customername() As String
            Get
                Return CType(Me("bShowColumn_customername"),String)
            End Get
            Set
                Me("bShowColumn_customername") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_customeraddress() As String
            Get
                Return CType(Me("bShowColumn_customeraddress"),String)
            End Get
            Set
                Me("bShowColumn_customeraddress") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_customertelefon() As String
            Get
                Return CType(Me("bShowColumn_customertelefon"),String)
            End Get
            Set
                Me("bShowColumn_customertelefon") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_customeremail() As String
            Get
                Return CType(Me("bShowColumn_customeremail"),String)
            End Get
            Set
                Me("bShowColumn_customeremail") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_responsiblename() As String
            Get
                Return CType(Me("bShowColumn_responsiblename"),String)
            End Get
            Set
                Me("bShowColumn_responsiblename") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_responsibleemail() As String
            Get
                Return CType(Me("bShowColumn_responsibleemail"),String)
            End Get
            Set
                Me("bShowColumn_responsibleemail") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_foffice() As String
            Get
                Return CType(Me("bShowColumn_foffice"),String)
            End Get
            Set
                Me("bShowColumn_foffice") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_soffice() As String
            Get
                Return CType(Me("bShowColumn_soffice"),String)
            End Get
            Set
                Me("bShowColumn_soffice") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_kst1() As String
            Get
                Return CType(Me("bShowColumn_kst1"),String)
            End Get
            Set
                Me("bShowColumn_kst1") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_kst2() As String
            Get
                Return CType(Me("bShowColumn_kst2"),String)
            End Get
            Set
                Me("bShowColumn_kst2") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_employeeadvisor() As String
            Get
                Return CType(Me("bShowColumn_employeeadvisor"),String)
            End Get
            Set
                Me("bShowColumn_employeeadvisor") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_customeradvisor() As String
            Get
                Return CType(Me("bShowColumn_customeradvisor"),String)
            End Get
            Set
                Me("bShowColumn_customeradvisor") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_esadvisor() As String
            Get
                Return CType(Me("bShowColumn_esadvisor"),String)
            End Get
            Set
                Me("bShowColumn_esadvisor") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_esvbacked() As String
            Get
                Return CType(Me("bShowColumn_esvbacked"),String)
            End Get
            Set
                Me("bShowColumn_esvbacked") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_vvbacked() As String
            Get
                Return CType(Me("bShowColumn_vvbacked"),String)
            End Get
            Set
                Me("bShowColumn_vvbacked") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_esvisexported() As String
            Get
                Return CType(Me("bShowColumn_esvisexported"),String)
            End Get
            Set
                Me("bShowColumn_esvisexported") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property bShowColumn_vvisexported() As String
            Get
                Return CType(Me("bShowColumn_vvisexported"),String)
            End Get
            Set
                Me("bShowColumn_vvisexported") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("")>  _
        Public Property frmLibraryInfo_Location() As String
            Get
                Return CType(Me("frmLibraryInfo_Location"),String)
            End Get
            Set
                Me("frmLibraryInfo_Location") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ifrmLibraryInfoWidth() As Integer
            Get
                Return CType(Me("ifrmLibraryInfoWidth"),Integer)
            End Get
            Set
                Me("ifrmLibraryInfoWidth") = value
            End Set
        End Property
        
        <Global.System.Configuration.UserScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("0")>  _
        Public Property ifrmLibraryInfoHeight() As Integer
            Get
                Return CType(Me("ifrmLibraryInfoHeight"),Integer)
            End Get
            Set
                Me("ifrmLibraryInfoHeight") = value
            End Set
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.SPESSearch.My.MySettings
            Get
                Return Global.SPESSearch.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace
