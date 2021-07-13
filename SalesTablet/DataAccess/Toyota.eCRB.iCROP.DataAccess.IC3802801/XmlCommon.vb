Public Class XmlCommon
    Private _DealerCode As String
    Private _BranchCode As String
    Private _IcropDealerCode As String
    Private _IcropBranchCode As String

    Public Property DealerCode() As String
        Get
            Return _DealerCode
        End Get
        Set(ByVal Value As String)
            _DealerCode = Value
        End Set
    End Property

    Public Property BranchCode() As String
        Get
            Return _BranchCode
        End Get
        Set(ByVal Value As String)
            _BranchCode = Value
        End Set
    End Property

    Public Property IcropDealerCode() As String
        Get
            Return _IcropDealerCode
        End Get
        Set(ByVal Value As String)
            _IcropDealerCode = Value
        End Set
    End Property

    Public Property IcropBranchCode() As String
        Get
            Return _IcropBranchCode
        End Get
        Set(ByVal Value As String)
            _IcropBranchCode = Value
        End Set
    End Property
End Class
