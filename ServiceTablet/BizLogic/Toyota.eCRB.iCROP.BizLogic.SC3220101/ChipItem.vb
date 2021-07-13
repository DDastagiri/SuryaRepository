''' <summary>
''' チップクラス
''' </summary>
''' <remarks></remarks>
Public Class ChipItem

    '来店実績連番
    Private _Id As Long
    '表示区分(工程) 1: 受付、2: 追加承認、3: 納車準備、4: 納車作業、5: 作業中、6：受付待ち
    Private _Stats As Integer
    '車両登録No
    Private _VehiclesRegNo As String
    '納車予定日時
    Private _DeliTime As String
    '納車見込遅れ日時
    Private _DelayTime As String
    '洗車フラグ
    Private _Wash As String
    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    '重要車両フラグ
    Private _ImpVclFlg As String
    'スマイル年間保守フラグ　SML_AMC_FLG
    Private _SmlAmcFlg As String
    '延長保守フラグ　EW_FLG
    Private _EwFlg As String
    'テレマ会員クラブTLM_MBR_FLG
    Private _TlmMbrFlg As String
    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

    Public Property Id As Long
        Get
            Return _Id
        End Get
        Set(value As Long)
            _Id = value
        End Set
    End Property

    Public Property Stats As Integer
        Get
            Return _Stats
        End Get
        Set(value As Integer)
            _Stats = value
        End Set
    End Property

    Public Property VehiclesRegNo As String
        Get
            Return _VehiclesRegNo
        End Get
        Set(value As String)
            _VehiclesRegNo = value
        End Set
    End Property

    Public Property DeliTime As String
        Get
            Return _DeliTime
        End Get
        Set(value As String)
            _DeliTime = value
        End Set
    End Property

    Public Property DelayTime As String
        Get
            Return _DelayTime
        End Get
        Set(value As String)
            _DelayTime = value
        End Set
    End Property

    Public Property Wash As String
        Get
            Return _Wash
        End Get
        Set(value As String)
            _Wash = value
        End Set
    End Property

    Public Property ImpVclFlg As String
        Get
            Return _ImpVclFlg
        End Get
        Set(value As String)
            _ImpVclFlg = value
        End Set
    End Property
    Public Property SmlAmcFlg As String
        Get
            Return _SmlAmcFlg
        End Get
        Set(value As String)
            _SmlAmcFlg = value
        End Set
    End Property
    Public Property EwFlg As String
        Get
            Return _EwFlg
        End Get
        Set(value As String)
            _EwFlg = value
        End Set
    End Property
    Public Property TlmMbrFlg As String
        Get
            Return _TlmMbrFlg
        End Get
        Set(value As String)
            _TlmMbrFlg = value
        End Set
    End Property
End Class
