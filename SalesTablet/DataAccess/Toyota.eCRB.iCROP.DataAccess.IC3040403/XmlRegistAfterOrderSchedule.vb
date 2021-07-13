' "データ格納クラス"
' 取得XML内に存在する要素を格納する。
Public Class XmlRegistAfterOrderSchedule

    '' Head要素

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks>メッセージに割り当てられた識別コード:ICXXXXX</remarks>
    Private _messageId As String

    ''' <summary>
    ''' 国コード
    ''' </summary>
    ''' <remarks>各国に割り当てられた識別コード</remarks>
    Private _countryCode As String

    ''' <summary>
    ''' SYSTEM識別コード
    ''' </summary>
    ''' <remarks>基本的に'0'固定 （連携元の基幹システムが複数存在する場合は0以外が登場）</remarks>
    Private _linkSystemCode As String

    ''' <summary>
    ''' 送信日付
    ''' </summary>
    ''' <remarks>送信された日時</remarks>
    Private _transmissionDate As String

    ''' <summary>
    ''' Detail要素
    ''' </summary>
    ''' <remarks>複数個存在するのでList化</remarks>
    Private _detailList As List(Of XmlAfterOrderDetail)

    ' メッセージID
    Public Property MessageId As String
        Get
            Return _messageId
        End Get
        Set(ByVal value As String)
            _messageId = value
        End Set
    End Property

    ''' <summary>
    ''' 国コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CountryCode As String
        Get
            Return _countryCode
        End Get
        Set(ByVal value As String)
            _countryCode = value
        End Set
    End Property

    ''' <summary>
    ''' SYSTEM識別コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LinkSystemCode As String
        Get
            Return _linkSystemCode
        End Get
        Set(ByVal value As String)
            _linkSystemCode = value
        End Set
    End Property

    ''' <summary>
    ''' 送信日付
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TransmissionDate As String
        Get
            Return _transmissionDate
        End Get
        Set(ByVal value As String)
            _transmissionDate = value
        End Set
    End Property

    ''' <summary>
    ''' Detail要素
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DetailList As List(Of XmlAfterOrderDetail)
        Get
            Return _detailList
        End Get
    End Property

    ''' <summary>
    ''' Detail要素初期化
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitialDetailList()
        _detailList = New List(Of XmlAfterOrderDetail)
    End Sub

End Class
