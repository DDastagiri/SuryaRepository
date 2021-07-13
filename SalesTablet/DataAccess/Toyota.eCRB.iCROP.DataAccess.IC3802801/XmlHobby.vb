Public Class XmlHobby
    Private _HobbyCode As String
    Private _HobbyName As String

    Public Property HobbyCode() As String
        Get
            Return _HobbyCode
        End Get
        Set(ByVal Value As String)
            _HobbyCode = Value
        End Set
    End Property

    Public Property HobbyName() As String
        Get
            Return _HobbyName
        End Get
        Set(ByVal Value As String)
            _HobbyName = Value
        End Set
    End Property

End Class
