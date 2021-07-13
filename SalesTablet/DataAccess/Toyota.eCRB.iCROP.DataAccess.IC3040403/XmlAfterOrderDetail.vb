Public Class XmlAfterOrderDetail

    '' Common要素 
     
    ''' <summary>
    ''' 販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private _dealerCode As String

    ''' <summary>
    ''' 店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private _branchCode As String

    ''' <summary>
    ''' スケジュールID
    ''' </summary>
    ''' <remarks>来店予約：Follow-up BoxテーブルのFollow-up Box内連番  入庫予約：ストール予約テーブルの予約ID</remarks>
    Private _scheduleId As String


    ''' <summary>
    ''' 処理区分
    ''' </summary>
    ''' <remarks>スケジュール情報の反映時の処理を判断をするためのフラグ(1:登録・完了 2:更新)</remarks>
    Private _actionType As String

    ''' <summary>
    ''' 活動作成スタッフコード
    ''' </summary>
    ''' <remarks>活動を作成したスタッフのコード</remarks>
    Private _activityCreateStaff As String


    '' ScheduleInfo要素
    ''' <summary>
    ''' ScheduleInfo要素が存在するかどうか
    ''' </summary>
    ''' <remarks></remarks>
    Private _scheduleInfoFlg As Boolean

    ''' <summary>
    ''' 顧客区分
    ''' </summary>
    ''' <remarks>スケジュールの対象となる顧客の種別を識別する区分(0:自社客 1:副顧客 2:未取引客)</remarks>
    Private _customerDiv As String

    ''' <summary>
    ''' 顧客コード
    ''' </summary>
    ''' <remarks>スケジュールの対象となる顧客のコード</remarks>
    Private _customerCode As String

    ''' <summary>
    ''' 顧客名
    ''' </summary>
    '''<History>2014/06/26 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 START</History>
    ''' <remarks>スケジュールの対象となる顧客のコード</remarks>
    Private _customerName As String


    ''' <summary>
    ''' DMSID
    ''' </summary>
    ''' <remarks>スケジュールの対象となる顧客のDMSID</remarks>
    Private _dmsId As String


    ''' <summary>
    ''' キャンセル日
    ''' </summary>
    ''' <remarks>入庫予約をキャンセルした日</remarks>
    Private _DeleteDate As String

    ''' <summary>
    ''' Schedule要素
    ''' </summary>
    ''' <remarks>複数個存在するのでList化</remarks>
    Private _scheduleList As List(Of XmlAfterOrderSchedule)

    ''' <summary>
    ''' 販売店コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DealerCode As String
        Get
            Return _dealerCode
        End Get
        Set(ByVal value As String)
            _dealerCode = value
        End Set
    End Property

    ''' <summary>
    ''' 店舗コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BranchCode As String
        Get
            Return _branchCode
        End Get
        Set(ByVal value As String)
            _branchCode = value
        End Set
    End Property


    ''' <summary>
    ''' スケジュールID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ScheduleId As String
        Get
            Return _scheduleId
        End Get
        Set(ByVal value As String)
            _scheduleId = value
        End Set
    End Property

    ''' <summary>
    ''' 処理区分
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActionType As String
        Get
            Return _actionType
        End Get
        Set(ByVal value As String)
            _actionType = value
        End Set
    End Property

    ''' <summary>
    ''' 活動生成スタッフコード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActivityCreateStaff As String
        Get
            Return _activityCreateStaff
        End Get
        Set(ByVal value As String)
            _activityCreateStaff = value
        End Set
    End Property

    ''' <summary>
    ''' ScheduleInfoフラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ScheduleInfoFlg As Boolean
        Get
            Return _scheduleInfoFlg
        End Get
        Set(ByVal value As Boolean)
            _scheduleInfoFlg = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客区分
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CustomerDiv As String
        Get
            Return _customerDiv
        End Get
        Set(ByVal value As String)
            _customerDiv = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CustomerCode As String
        Get
            Return _customerCode
        End Get
        Set(ByVal value As String)
            _customerCode = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客名
    ''' </summary>
    '''<History>2014/06/26 SKFC 渡邊 NEXTSTEP_CALDAV 不具合修正 START</History>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CustomerName As String
        Get
            Return _customerName
        End Get
        Set(ByVal value As String)
            _customerName = value
        End Set
    End Property

    ''' <summary>
    ''' DMSID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DmsId As String
        Get
            Return _dmsId
        End Get
        Set(ByVal value As String)
            _dmsId = value
        End Set
    End Property

    ''' <summary>
    ''' 削除日
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DeleteDate As String
        Get
            Return _DeleteDate
        End Get
        Set(ByVal value As String)
            _DeleteDate = value
        End Set
    End Property


    ''' <summary>
    ''' スケジュール要素
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ScheduleList As List(Of XmlAfterOrderSchedule)
        Get
            Return _scheduleList
        End Get
    End Property

    ''' <summary>
    ''' スケジュール要素初期化
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitialScheduleList()
        _scheduleList = New List(Of XmlAfterOrderSchedule)
    End Sub

End Class
