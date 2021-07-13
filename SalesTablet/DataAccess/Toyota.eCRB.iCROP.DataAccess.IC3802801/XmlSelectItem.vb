Public Class XmlSelectItem
    Private _ItemNo As String
    Private _Other As String

    Public Property ItemNo() As String
        Get
            Return _ItemNo
        End Get
        Set(ByVal Value As String)
            _ItemNo = Value
        End Set
    End Property

    Public Property Other() As String
        Get
            Return _Other
        End Get
        Set(ByVal Value As String)
            _Other = Value
        End Set
    End Property
End Class
