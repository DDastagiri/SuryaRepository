Public Class XmlSalesCondition
    Private _SalesConditionNo As String
    Private _SelectItem As XmlSelectItem
    Private _ItemNo As String
    Private _Other As String

    Public Property SalesConditionNo() As String
        Get
            Return _SalesConditionNo
        End Get
        Set(ByVal Value As String)
            _SalesConditionNo = Value
        End Set
    End Property

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

    'Public Property SelectItem() As XmlSelectItem
    '    Get
    '        Return _SelectItem
    '    End Get
    '    Set(ByVal Value As XmlSelectItem)
    '        _SelectItem = Value
    '    End Set
    'End Property
End Class
