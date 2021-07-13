'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SAItem.vb
'─────────────────────────────────────
'機能： SA別データクラス
'補足： 
'作成： 2012/05/28 日比野 
'更新： 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応
'更新： 
'─────────────────────────────────────

''' <summary>
''' SA別データクラス
''' </summary>
''' <remarks></remarks>
Public Class SAItem

    'アカウント
    Private _Id As String
    'スタッフネーム
    Private _Name As String
    '在籍状況
    Private _Stats As String
    'アイコン情報
    Private _ChipList As List(Of ChipItem) = New List(Of ChipItem)

    ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START
    '受付待ちフラグ 1：受付待ち、0：受付待ち以外
    Private _Visit As String
    ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END

    Public Property Id As String
        Get
            Return _Id
        End Get
        Set(value As String)
            _Id = value
        End Set
    End Property

    Public Property Name As String
        Get
            Return _Name
        End Get
        Set(value As String)
            _Name = value
        End Set
    End Property

    Public Property Stats As String
        Get
            Return _Stats
        End Get
        Set(value As String)
            _Stats = value
        End Set
    End Property

    Public ReadOnly Property ChipList As List(Of ChipItem)
        Get
            Return _ChipList
        End Get
    End Property

    ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START
    Public Property Visit As String
        Get
            Return _Visit
        End Get
        Set(value As String)
            _Visit = value
        End Set
    End Property
    ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END
End Class