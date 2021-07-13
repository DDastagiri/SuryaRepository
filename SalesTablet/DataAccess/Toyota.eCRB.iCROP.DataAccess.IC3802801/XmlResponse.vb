Public Class XmlResponse
    Private _HeadResponse As XmlHeadResponse
    Private _CommonResponse As XmlCommonResponse
    Private _FollowUpInfo As XmlFollowUpInfo

    Public Property HeadResponse() As XmlHeadResponse
        Get
            Return _HeadResponse
        End Get
        Set(ByVal Value As XmlHeadResponse)
            _HeadResponse = Value
        End Set
    End Property

    Public Property CommonResponse() As XmlCommonResponse
        Get
            Return _CommonResponse
        End Get
        Set(ByVal Value As XmlCommonResponse)
            _CommonResponse = Value
        End Set
    End Property

    Public Property FollowUpInfo() As XmlFollowUpInfo
        Get
            Return _FollowUpInfo
        End Get
        Set(ByVal Value As XmlFollowUpInfo)
            _FollowUpInfo = Value
        End Set
    End Property
End Class
