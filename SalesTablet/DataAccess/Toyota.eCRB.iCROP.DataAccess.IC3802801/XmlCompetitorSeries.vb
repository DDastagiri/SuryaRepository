Public Class XmlCompetitorSeries
    Private _MakerCode As String
    Private _MakerName As String
    Private _SeriesCode As String
    Private _SeriesName As String
    Private _DeleteDate As String

    Public Property MakerCode() As String
        Get
            Return _MakerCode
        End Get
        Set(ByVal Value As String)
            _MakerCode = Value
        End Set
    End Property

    Public Property MakerName() As String
        Get
            Return _MakerName
        End Get
        Set(ByVal Value As String)
            _MakerName = Value
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

    Public Property SeriesName() As String
        Get
            Return _SeriesName
        End Get
        Set(ByVal Value As String)
            _SeriesName = Value
        End Set
    End Property

    Public Property DeleteDate() As String
        Get
            Return _DeleteDate
        End Get
        Set(ByVal Value As String)
            _DeleteDate = Value
        End Set
    End Property

End Class
