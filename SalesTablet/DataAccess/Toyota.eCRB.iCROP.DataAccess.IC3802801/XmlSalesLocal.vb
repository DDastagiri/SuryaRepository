'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'XmlSalesLocal.vb
'─────────────────────────────────────
'Ver.  : SA01_LC_001
'Create:  2018/07/05[SA01_LC_001] NSK Niiya TKM Next Gen e-CRB Project Application development Block B-2 SI No.1,2,4,5,6 $01
'Update:
'─────────────────────────────────────
Public Class XmlSalesLocal
    Private _TradeinDate As String
    Private _DemandStructureCd As String
    Private _TradeincarEnabledFlg As String
    Private _MakerName As String
    Private _SeriesName As String
    Private _ModelYear As String
    Private _CreateDate As String
    Private _DistanceCovered As String

    Public Property TradeinDate() As String
        Get
            Return _TradeinDate
        End Get
        Set(ByVal Value As String)
            _TradeinDate = Value
        End Set
    End Property

    Public Property DemandStructureCd() As String
        Get
            Return _DemandStructureCd
        End Get
        Set(ByVal Value As String)
            _DemandStructureCd = Value
        End Set
    End Property

    Public Property TradeincarEnabledFlg() As String
        Get
            Return _TradeincarEnabledFlg
        End Get
        Set(ByVal Value As String)
            _TradeincarEnabledFlg = Value
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

    Public Property SeriesName() As String
        Get
            Return _SeriesName
        End Get
        Set(ByVal Value As String)
            _SeriesName = Value
        End Set
    End Property

    Public Property ModelYear() As String
        Get
            Return _ModelYear
        End Get
        Set(ByVal Value As String)
            _ModelYear = Value
        End Set
    End Property

    Public Property CreateDate() As String
        Get
            Return _CreateDate
        End Get
        Set(ByVal Value As String)
            _CreateDate = Value
        End Set
    End Property

    Public Property DistanceCovered() As String
        Get
            Return _DistanceCovered
        End Get
        Set(ByVal Value As String)
            _DistanceCovered = Value
        End Set
    End Property

End Class
