Public Class XmlHeadResponse
    Private _MessageID As String
    Private _CountryCode As String
    Private _ReceptionDate As String
    Private _TransmissionDate As String

    Public Property MessageID() As String
        Get
            Return _MessageID
        End Get
        Set(ByVal Value As String)
            _MessageID = Value
        End Set
    End Property

    Public Property CountryCode() As String
        Get
            Return _CountryCode
        End Get
        Set(ByVal Value As String)
            _CountryCode = Value
        End Set
    End Property

    Public Property ReceptionDate() As String
        Get
            Return _ReceptionDate
        End Get
        Set(ByVal Value As String)
            _ReceptionDate = Value
        End Set
    End Property

    Public Property TransmissionDate() As String
        Get
            Return _TransmissionDate
        End Get
        Set(ByVal Value As String)
            _TransmissionDate = Value
        End Set
    End Property
End Class
