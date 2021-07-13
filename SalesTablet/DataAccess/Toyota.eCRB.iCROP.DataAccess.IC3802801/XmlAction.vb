Public Class XmlAction
    Private _ActionSeqNo As String
    Private _ActionCode As String
    Private _ActionName As String
    Private _ActionMemo As String
    Private _PlannedActionDate As String
    Private _StartActionDate As String
    Private _ActionDate As String
    Private _ActionBranchCode As String
    Private _ActionAccount As String

    Public Property ActionSeqNo() As String
        Get
            Return _ActionSeqNo
        End Get
        Set(ByVal Value As String)
            _ActionSeqNo = Value
        End Set
    End Property

    Public Property ActionCode() As String
        Get
            Return _ActionCode
        End Get
        Set(ByVal Value As String)
            _ActionCode = Value
        End Set
    End Property

    Public Property ActionName() As String
        Get
            Return _ActionName
        End Get
        Set(ByVal Value As String)
            _ActionName = Value
        End Set
    End Property

    Public Property ActionMemo() As String
        Get
            Return _ActionMemo
        End Get
        Set(ByVal Value As String)
            _ActionMemo = Value
        End Set
    End Property

    Public Property PlannedActionDate() As String
        Get
            Return _PlannedActionDate
        End Get
        Set(ByVal Value As String)
            _PlannedActionDate = Value
        End Set
    End Property

    Public Property StartActionDate() As String
        Get
            Return _StartActionDate
        End Get
        Set(ByVal Value As String)
            _StartActionDate = Value
        End Set
    End Property

    Public Property ActionDate() As String
        Get
            Return _ActionDate
        End Get
        Set(ByVal Value As String)
            _ActionDate = Value
        End Set
    End Property


    Public Property ActionBranchCode() As String
        Get
            Return _ActionBranchCode
        End Get
        Set(ByVal Value As String)
            _ActionBranchCode = Value
        End Set
    End Property

    Public Property ActionAccount() As String
        Get
            Return _ActionAccount
        End Get
        Set(ByVal Value As String)
            _ActionAccount = Value
        End Set
    End Property


End Class
