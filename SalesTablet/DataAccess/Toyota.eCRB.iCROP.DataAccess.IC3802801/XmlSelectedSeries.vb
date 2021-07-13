Public Class XmlSelectedSeries
    Private _SelectedSeriesNo As String
    Private _PreferredVehicleFlg As String
    Private _SeriesCode As String
    Private _GradeCode As String
    Private _ExteriorColorCode As String
    Private _InteriorColorCode As String
    Private _ModelSuffix As String
    Private _Quantity As String
    Private _QuotationPrice As String

    Public Property SelectedSeriesNo() As String
        Get
            Return _SelectedSeriesNo
        End Get
        Set(ByVal Value As String)
            _SelectedSeriesNo = Value
        End Set
    End Property

    Public Property PreferredVehicleFlg() As String
        Get
            Return _PreferredVehicleFlg
        End Get
        Set(ByVal Value As String)
            _PreferredVehicleFlg = Value
        End Set
    End Property

    Public Property SeriesCode() As String
        Get
            Return _SeriesCode
        End Get
        Set(ByVal Value As String)
            _SeriesCode = Value
        End Set
    End Property

    Public Property GradeCode() As String
        Get
            Return _GradeCode
        End Get
        Set(ByVal Value As String)
            _GradeCode = Value
        End Set
    End Property

    Public Property ExteriorColorCode() As String
        Get
            Return _ExteriorColorCode
        End Get
        Set(ByVal Value As String)
            _ExteriorColorCode = Value
        End Set
    End Property

    Public Property InteriorColorCode() As String
        Get
            Return _InteriorColorCode
        End Get
        Set(ByVal Value As String)
            _InteriorColorCode = Value
        End Set
    End Property

    Public Property ModelSuffix() As String
        Get
            Return _ModelSuffix
        End Get
        Set(ByVal Value As String)
            _ModelSuffix = Value
        End Set
    End Property

    Public Property Quantity() As String
        Get
            Return _Quantity
        End Get
        Set(ByVal Value As String)
            _Quantity = Value
        End Set
    End Property

    Public Property QuotationPrice() As String
        Get
            Return _QuotationPrice
        End Get
        Set(ByVal Value As String)
            _QuotationPrice = Value
        End Set
    End Property

End Class
