Public Class XmlNegotiationMemo
    Private _CreateDate As String
    Private _CreateAccount As String
    Private _Memo As String

    Public Property CreateDate() As String
        Get
            Return _CreateDate
        End Get
        Set(ByVal Value As String)
            _CreateDate = Value
        End Set
    End Property

    Public Property CreateAccount() As String
        Get
            Return _CreateAccount
        End Get
        Set(ByVal Value As String)
            _CreateAccount = Value
        End Set
    End Property

    Public Property Memo() As String
        Get
            Return _Memo
        End Get
        Set(ByVal Value As String)
            _Memo = Value
        End Set
    End Property
End Class
