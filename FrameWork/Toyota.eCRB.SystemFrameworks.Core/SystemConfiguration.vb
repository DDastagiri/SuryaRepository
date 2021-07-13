Imports Toyota.eCRB.SystemFrameworks.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Core
    Public Enum SystemConfigurationType
        iCROPConnectionString
        DMSConnectionString
        ApplicationType
        ApplicationId
    End Enum

    Public Enum ApplicationType
        Web
        Batch
    End Enum

    Public Class SystemConfiguration
        Public Shared ReadOnly Property Current As SystemConfiguration
            Get
                Return _soleInstance
            End Get
        End Property

        Private Sub New()
        End Sub

        Public Property Manager As ConfigurationManager
            Get
                Return Me._configManager
            End Get
            Set(value As ConfigurationManager)
                Me._configManager = value
            End Set
        End Property

        Public Function GetRuntimeSetting(ByVal type As SystemConfigurationType) As String
            Return Me._runtimeSetting(type)
        End Function

        Public Sub SetRuntimeSetting(ByVal type As SystemConfigurationType, ByVal value As String)
            Me._runtimeSetting.Add(type, value)
        End Sub

        Public ReadOnly Property Hooks As List(Of Object)
            Get
                Return _hooks
            End Get
        End Property

        Private Shared _soleInstance As New SystemConfiguration()
        Private _configManager As ConfigurationManager = Nothing
        Private _runtimeSetting As New Dictionary(Of SystemConfigurationType, String)
        Private _hooks As New List(Of Object)

    End Class

End Namespace
