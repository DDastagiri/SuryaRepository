Public Class XmlCommonResponse
    Private _ResultId As String
    Private _Message As String

    Public Property ResultId() As String
        Get
            Return _ResultId
        End Get
        Set(ByVal Value As String)
            _ResultId = Value
        End Set
    End Property

    Public Property Message() As String
        Get
            Return _Message
        End Get
        Set(ByVal Value As String)
            _Message = Value
        End Set
    End Property

End Class
