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

Imports System.Data

Namespace OstJobsCHVacancyService
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute([Namespace]:="http://asmx.domain.com/wsSPS_services/SPOstJobsCHVacancies.asmx/", ConfigurationName:="OstJobsCHVacancyService.SPOstJobsCHVacanciesSoap")>  _
    Public Interface SPOstJobsCHVacanciesSoap
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://asmx.domain.com/wsSPS_services/SPOstJobsCHVacancies.asmx/SaveOstJobCHV"& _ 
            "acancy", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function SaveOstJobCHVacancy(ByVal customerGuid As String, ByVal userGuid As String, ByVal vacancyData As System.Data.DataTable) As Boolean
    End Interface
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface SPOstJobsCHVacanciesSoapChannel
        Inherits OstJobsCHVacancyService.SPOstJobsCHVacanciesSoap, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class SPOstJobsCHVacanciesSoapClient
        Inherits System.ServiceModel.ClientBase(Of OstJobsCHVacancyService.SPOstJobsCHVacanciesSoap)
        Implements OstJobsCHVacancyService.SPOstJobsCHVacanciesSoap
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        Public Function SaveOstJobCHVacancy(ByVal customerGuid As String, ByVal userGuid As String, ByVal vacancyData As System.Data.DataTable) As Boolean Implements OstJobsCHVacancyService.SPOstJobsCHVacanciesSoap.SaveOstJobCHVacancy
            Return MyBase.Channel.SaveOstJobCHVacancy(customerGuid, userGuid, vacancyData)
        End Function
    End Class
End Namespace
