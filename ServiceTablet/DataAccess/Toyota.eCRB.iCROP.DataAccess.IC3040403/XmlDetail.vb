Public Class XmlDetail

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
    ''' スケジュール区分
    ''' </summary>
    ''' <remarks>イベントの情報の種別を判別する区分(0:来店予約 1:入庫予約)"</remarks>
    Private _scheduleDiv As String

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
    ''' DMSID
    ''' </summary>
    ''' <remarks>スケジュールの対象となる顧客のDMSID</remarks>
    Private _dmsId As String

    ''' <summary>
    ''' 顧客名
    ''' </summary>
    ''' <remarks>スケジュールの対象となる顧客の名前</remarks>
    Private _customerName As String

    ''' <summary>
    ''' 受付納車区分
    ''' </summary>
    ''' <remarks>入庫予約で指定される受付納車の区分を表すコード(0:待ち 1:引取納車　2:引取　3:納車　4:預り)</remarks>
    Private _receptionDiv As String

    ''' <summary>
    ''' サービスコード
    ''' </summary>
    ''' <remarks>入庫予約で指定されるサービスの名称　ストール　予約.サービスコード</remarks>
    Private _serviceCode As String

    ''' <summary>
    ''' 商品コード
    ''' </summary>
    ''' <remarks>入庫予約で指定される商品の名称　ストール　予約.商品コード</remarks>
    Private _merchandiseCD As String

    ''' <summary>
    ''' 入庫ステータス
    ''' </summary>
    ''' <remarks>入庫状態を表すステータス (0:未入庫 1:入庫)</remarks>
    Private _strStatus As String

    ''' <summary>
    ''' 予約ステータス
    ''' </summary>
    ''' <remarks>入庫予約の種別を表すステータス (1:ストール本予約 2:ストール仮予約 3:使用不可 4:引取納車)</remarks>
    Private _rezStatus As String

    ''' <summary>
    ''' 完了フラグ
    ''' </summary>
    ''' <remarks>スケジュールの完了動作を判断するためのフラグ(1:新規登録 2:Continue 3:活動完了)</remarks>
    Private _completionDiv As String

    ''' <summary>
    ''' 完了日
    ''' </summary>
    ''' <remarks></remarks>
    Private _completionDate As String

    ''' <summary>
    ''' キャンセル日
    ''' </summary>
    ''' <remarks>入庫予約をキャンセルした日</remarks>
    Private _DeleteDate As String

    ''' <summary>
    ''' Schedule要素
    ''' </summary>
    ''' <remarks>複数個存在するのでList化</remarks>
    Private _scheduleList As List(Of XmlSchedule)

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
        Set(value As String)
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
        Set(value As String)
            _branchCode = value
        End Set
    End Property

    ''' <summary>
    ''' スケジュール区分
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ScheduleDiv As String
        Get
            Return _scheduleDiv
        End Get
        Set(value As String)
            _scheduleDiv = value
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
        Set(value As String)
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
        Set(value As String)
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
        Set(value As String)
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
        Set(value As Boolean)
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
        Set(value As String)
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
        Set(value As String)
            _customerCode = value
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
        Set(value As String)
            _dmsId = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客名
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CustomerName As String
        Get
            Return _customerName
        End Get
        Set(value As String)
            _customerName = value
        End Set
    End Property

    ''' <summary>
    ''' 受付納車区分
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ReceptionDiv As String
        Get
            Return _receptionDiv
        End Get
        Set(value As String)
            _receptionDiv = value
        End Set
    End Property

    ''' <summary>
    ''' サービスコード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ServiceCode As String
        Get
            Return _serviceCode
        End Get
        Set(value As String)
            _serviceCode = value
        End Set
    End Property

    ''' <summary>
    ''' 商品コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MerchandiseCD As String
        Get
            Return _merchandiseCD
        End Get
        Set(value As String)
            _merchandiseCD = value
        End Set
    End Property

    ''' <summary>
    ''' 入庫ステータス
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StrStatus As String
        Get
            Return _strStatus
        End Get
        Set(value As String)
            _strStatus = value
        End Set
    End Property

    ''' <summary>
    ''' 予約ステータス
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RezStatus As String
        Get
            Return _rezStatus
        End Get
        Set(value As String)
            _rezStatus = value
        End Set
    End Property

    ''' <summary>
    ''' 完了区分
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CompletionDiv As String
        Get
            Return _completionDiv
        End Get
        Set(value As String)
            _completionDiv = value
        End Set
    End Property

    ''' <summary>
    ''' 完了日
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CompletionDate As String
        Get
            Return _completionDate
        End Get
        Set(value As String)
            _completionDate = value
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
        Set(value As String)
            _DeleteDate = value
        End Set
    End Property

    ''' <summary>
    ''' スケジュール要素
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ScheduleList As List(Of XmlSchedule)
        Get
            Return _scheduleList
        End Get
    End Property

    ''' <summary>
    ''' スケジュール要素初期化
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitialScheduleList()
        _scheduleList = New List(Of XmlSchedule)
    End Sub

End Class
