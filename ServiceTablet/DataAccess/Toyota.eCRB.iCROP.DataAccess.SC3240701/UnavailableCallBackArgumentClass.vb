'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
' UnavailableCallBackArgumentClass.vb
'─────────────────────────────────────
'機能： ストール使用不可画面
'補足： 
'作成： 2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
'更新： 
'─────────────────────────────────────

''' <summary>
''' コールバック用引数のクラス
''' </summary>
''' <remarks></remarks>
Public Class UnavailableCallBackArgumentClass

    Private _method As String
    Private _dlrCD As String
    Private _strCD As String
    Private _showDate As String
    Private _stallId As String
    Private _startIdleTime As Date
    Private _finishIdleTime As Date
    Private _idleTime As String
    Private _idleMemo As String
    Private _validateCode As Integer
    Private _rowLockVersion As Long
    Private _stallIdleId As Decimal


    ''' <summary>
    ''' ストール非稼働ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StallIdleId() As Decimal
        Get
            Return Me._stallIdleId
        End Get
        Set(ByVal value As Decimal)
            Me._stallIdleId = value
        End Set
    End Property

    ''' <summary>
    ''' コールバックメソッド名
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Method() As String
        Get
            Return Me._method
        End Get
        Set(ByVal value As String)
            Me._method = value
        End Set
    End Property

    ''' <summary>
    ''' 販売店コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DlrCD() As String
        Get
            Return Me._dlrCD
        End Get
        Set(ByVal value As String)
            Me._dlrCD = value
        End Set
    End Property

    ''' <summary>
    ''' 店舗コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StrCD() As String
        Get
            Return Me._strCD
        End Get
        Set(ByVal value As String)
            Me._strCD = value
        End Set
    End Property

    ''' <summary>
    ''' 工程管理画面で表示中の日付
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowDate() As String
        Get
            Return Me._showDate
        End Get
        Set(ByVal value As String)
            Me._showDate = value
        End Set
    End Property


    ''' <summary>
    ''' ストールID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StallId() As String
        Get
            Return Me._stallId
        End Get
        Set(ByVal value As String)
            Me._stallId = value
        End Set
    End Property

    ''' <summary>
    ''' 非稼働開始日時
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StartIdleTime() As Date
        Get
            Return Me._startIdleTime
        End Get
        Set(ByVal value As Date)
            Me._startIdleTime = value
        End Set
    End Property

    ''' <summary>
    ''' 非稼働終了日時
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FinishIdleTime() As Date
        Get
            Return Me._finishIdleTime
        End Get
        Set(ByVal value As Date)
            Me._finishIdleTime = value
        End Set
    End Property

    ''' <summary>
    ''' 非稼働時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdleTime() As String
        Get
            Return Me._idleTime
        End Get
        Set(ByVal value As String)
            Me._idleTime = value
        End Set
    End Property

    ''' <summary>
    ''' 非稼働メモ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdleMemo() As String
        Get
            Return Me._idleMemo
        End Get
        Set(ByVal value As String)
            Me._idleMemo = value
        End Set
    End Property

    ''' <summary>
    ''' 入力項目チェック結果コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ValidateCode() As Integer
        Get
            Return Me._validateCode
        End Get
        Set(ByVal value As Integer)
            Me._validateCode = value
        End Set
    End Property

    ''' <summary>
    ''' 行ロックバージョン
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RowLockVersion() As Long
        Get
            Return Me._rowLockVersion
        End Get
        Set(ByVal value As Long)
            Me._rowLockVersion = value
        End Set
    End Property

End Class