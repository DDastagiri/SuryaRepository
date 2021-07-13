Public Class XmlFollowUpInfo
    Private _FollowUpID As String

    Public Property FollowUpID() As String
        Get
            Return _FollowUpID
        End Get
        Set(ByVal Value As String)
            _FollowUpID = Value
        End Set
    End Property
End Class
