'-------------------------------------------------------------------------
'XmlRequestNotice.vb
'-------------------------------------------------------------------------
'機能：通知API子クラス
'補足：
'作成：-
'更新：----/--/-- --   -- Sales Step2 $01
'更新：2013/05/29 TMEJ tshimamura  【A.STEP2】次世代e-CRB新車タブレット　新DB適応に向けた機能開発 $02
Public Class XmlRequestNotice
    Implements IDisposable

    ''' <summary>
    ''' 販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private _DealerCode As String

    ''' <summary>
    ''' 店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private _StoreCode As String

    ''' <summary>
    ''' 依頼種別
    ''' </summary>
    ''' <remarks></remarks>
    Private _RequestClass As String

    ''' <summary>
    ''' ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private _Status As String

    ''' <summary>
    ''' 依頼ID
    ''' </summary>
    ''' <remarks></remarks>
    Private _RequestId As Long

    '$02 start 桁数変更対応
    ''' <summary>
    ''' 依頼種別ID
    ''' </summary>
    ''' <remarks></remarks>
    Private _RequestClassId As Decimal
    '$02 end 桁数変更対応

    ''' <summary>
    ''' スタッフコード(送信元)
    ''' </summary>
    ''' <remarks></remarks>
    Private _FromAccount As String

    ''' <summary>
    ''' 端末ID(送信元)
    ''' </summary>
    ''' <remarks></remarks>
    Private _FromClientId As String

    ''' <summary>
    ''' スタッフ名(送信元)
    ''' </summary>
    ''' <remarks></remarks>
    Private _FromAccountName As String

    ''' <summary>
    ''' お客様名ID
    ''' </summary>
    ''' <remarks></remarks>
    Private _CustomId As String

    ''' <summary>
    ''' お客様名
    ''' </summary>
    ''' <remarks></remarks>
    Private _CustomName As String

    ''' <summary>
    ''' 顧客分類
    ''' </summary>
    ''' <remarks></remarks>
    Private _CustomerClass As String

    ''' <summary>
    ''' 顧客種別
    ''' </summary>
    ''' <remarks></remarks>
    Private _CustomerKind As String

    ''' <summary>
    ''' 表示内容
    ''' </summary>
    ''' <remarks></remarks>
    Private _Message As String

    ''' <summary>
    ''' セッション設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private _SessionValue As String

    ''' <summary>
    ''' 顧客担当セールススタッフコード
    ''' </summary>
    ''' <remarks></remarks>
    Private _SalesStaffCode As String

    ''' <summary>
    ''' 車両シーケンス№
    ''' </summary>
    ''' <remarks></remarks>
    Private _VehicleSequenceNumber As String

    ''' <summary>
    ''' Follow-up Box店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private _FollowUpBoxStoreCode As String

    '$02 start 桁数変更対応
    ''' <summary>
    ''' Follow-up Box番号
    ''' </summary>
    ''' <remarks></remarks>
    Private _FollowUpBoxNumber As Decimal
    '$02 end 桁数変更対応

    ' $01 start step2開発
    ''' <summary>
    ''' 用紙名
    ''' </summary>
    ''' <remarks></remarks>
    Private _CSPaperName As String
    ' $01 end   step2開発

    ''' <summary>
    ''' Push情報
    ''' </summary>
    ''' <remarks></remarks>
    Private _PushInfo As String

    ''' <summary>
    ''' メッセージ情報
    ''' </summary>
    ''' <remarks></remarks>
    Private _NoticeMessage As String

    Public Property NoticeMessage() As String
        Get
            Return _NoticeMessage
        End Get
        Set(ByVal value As String)
            _NoticeMessage = value
        End Set
    End Property

    ''' <summary>
    ''' Push情報
    ''' </summary>
    ''' <remarks></remarks>
    Public Property PushInfo() As String
        Get
            Return _PushInfo
        End Get
        Set(ByVal value As String)
            _PushInfo = value
        End Set
    End Property

    ' $01 start step2開発
    ''' <summary>
    ''' 用紙名
    ''' </summary>
    ''' <remarks></remarks>
    Public Property CSPaperName() As String
        Get
            Return _CSPaperName
        End Get
        Set(ByVal value As String)
            _CSPaperName = value
        End Set
    End Property
    ' $01 end   step2開発
    '$02 start 桁数変更対応
    ''' <summary>
    ''' Follow-up Box番号
    ''' </summary>
    ''' <remarks></remarks>
    Public Property FollowUpBoxNumber() As Decimal
        Get
            Return _FollowUpBoxNumber
        End Get
        Set(ByVal value As Decimal)
            _FollowUpBoxNumber = value
        End Set
    End Property
    '$02 end 桁数変更対応

    ''' <summary>
    ''' Follow-up Box店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Public Property FollowUpBoxStoreCode() As String
        Get
            Return _FollowUpBoxStoreCode
        End Get
        Set(ByVal value As String)
            _FollowUpBoxStoreCode = value
        End Set
    End Property

    ''' <summary>
    ''' 車両シーケンス№
    ''' </summary>
    ''' <remarks></remarks>
    Public Property VehicleSequenceNumber() As String
        Get
            Return _VehicleSequenceNumber
        End Get
        Set(ByVal value As String)
            _VehicleSequenceNumber = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客担当セールススタッフコード
    ''' </summary>
    ''' <remarks></remarks>
    Public Property SalesStaffCode() As String
        Get
            Return _SalesStaffCode
        End Get
        Set(ByVal value As String)
            _SalesStaffCode = value
        End Set
    End Property

    ''' <summary>
    ''' セッション設定値
    ''' </summary>
    ''' <remarks></remarks>
    Public Property SessionValue() As String
        Get
            Return _SessionValue
        End Get
        Set(ByVal value As String)
            _SessionValue = value
        End Set
    End Property

    ''' <summary>
    ''' 表示内容
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Message() As String
        Get
            Return _Message
        End Get
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客種別
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CustomerKind() As String
        Get
            Return _CustomerKind
        End Get
        Set(ByVal value As String)
            _CustomerKind = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客分類
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CustomerClass() As String
        Get
            Return _CustomerClass
        End Get
        Set(ByVal value As String)
            _CustomerClass = value
        End Set
    End Property

    ''' <summary>
    ''' お客様名
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CustomName() As String
        Get
            Return _CustomName
        End Get
        Set(ByVal value As String)
            _CustomName = value
        End Set
    End Property

    ''' <summary>
    ''' お客様名ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CustomId() As String
        Get
            Return _CustomId
        End Get
        Set(ByVal value As String)
            _CustomId = value
        End Set
    End Property

    ''' <summary>
    ''' スタッフ名(送信元)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FromAccountName() As String
        Get
            Return _FromAccountName
        End Get
        Set(ByVal value As String)
            _FromAccountName = value
        End Set
    End Property

    ''' <summary>
    ''' 端末ID(送信元)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FromClientId() As String
        Get
            Return _FromClientId
        End Get
        Set(ByVal value As String)
            _FromClientId = value
        End Set
    End Property

    ''' <summary>
    ''' スタッフコード(送信元)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FromAccount() As String
        Get
            Return _FromAccount
        End Get
        Set(ByVal value As String)
            _FromAccount = value
        End Set
    End Property

    ' $02 start 桁数変更対応
    ''' <summary>
    ''' 依頼種別ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RequestClassId() As Decimal
        Get
            Return _RequestClassId
        End Get
        Set(ByVal value As Decimal)
            _RequestClassId = value
        End Set
    End Property
    ' $02 end 桁数変更対応

    ''' <summary>
    ''' 依頼ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RequestId() As Long
        Get
            Return _RequestId
        End Get
        Set(ByVal value As Long)
            _RequestId = value
        End Set
    End Property

    ''' <summary>
    ''' ステータス
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Status() As String
        Get
            Return _Status
        End Get
        Set(ByVal value As String)
            _Status = value
        End Set
    End Property

    ''' <summary>
    ''' 依頼種別
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RequestClass() As String
        Get
            Return _RequestClass
        End Get
        Set(ByVal value As String)
            _RequestClass = value
        End Set
    End Property

    ''' <summary>
    ''' 店舗コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StoreCode() As String
        Get
            Return _StoreCode
        End Get
        Set(ByVal value As String)
            _StoreCode = value
        End Set
    End Property

    ''' <summary>
    ''' 販売店コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DealerCode() As String
        Get
            Return _DealerCode
        End Get
        Set(ByVal value As String)
            _DealerCode = value
        End Set
    End Property

    ''' <summary>
    ''' Disposeメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
        End If
    End Sub
End Class
