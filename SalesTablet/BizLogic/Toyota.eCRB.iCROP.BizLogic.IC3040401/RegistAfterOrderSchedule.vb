'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'XmlRegistAfterOrderSchedule.vb
'──────────────────────────────────
'機能： CalDAV連携インターフェース
'補足： 
'作成： 2014/04/24 TMEJ t.mizumoto 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計
'──────────────────────────────────
Namespace Toyota.eCRB.iCROP.BizLogic.IC3040401

    Public Class RegistAfterOrderSchedule

        ''' <summary>
        ''' スケジュール作成区分
        ''' </summary>
        ''' <remarks></remarks>
        Private _createScheduleDiv As String

        ''' <summary>
        ''' 活動担当スタッフ店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        Private _activityStaffBranchCode As String

        ''' <summary>
        ''' 活動スタッフコード
        ''' </summary>
        ''' <remarks></remarks>
        Private _activityStaffCode As String

        ''' <summary>
        ''' 受付担当スタッフ店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        Private _receptionStaffBranchCode As String

        ''' <summary>
        ''' 受付担当スタッフコード
        ''' </summary>
        ''' <remarks></remarks>
        Private _receptionStaffCode As String

        ''' <summary>
        ''' 接触方法No
        ''' </summary>
        ''' <remarks></remarks>
        Private _contactNo As String

        ''' <summary>
        ''' 接触方法名
        ''' </summary>
        ''' <remarks></remarks>
        Private _contactName As String

        ''' <summary>
        ''' 受注後活動名称
        ''' </summary>
        ''' <remarks></remarks>
        Private _actOdrName As String

        ''' <summary>
        ''' タイトル
        ''' </summary>
        ''' <remarks></remarks>
        Private _summary As String

        ''' <summary>
        ''' 開始日時
        ''' </summary>
        ''' <remarks></remarks>
        Private _startTime As String

        ''' <summary>
        ''' 終了日時
        ''' </summary>
        ''' <remarks></remarks>
        Private _endTime As String

        ''' <summary>
        ''' 説明(メモ)
        ''' </summary>
        ''' <remarks></remarks>
        Private _memo As String

        ''' <summary>
        ''' 色設定
        ''' </summary>
        ''' <remarks></remarks>
        Private _xIcropColor As String

        ''' <summary>
        ''' アラーム起動タイミング
        ''' </summary>
        ''' <remarks></remarks>
        Private _alarmTriggerList As List(Of String)

        ''' <summary>
        ''' 受注区分
        ''' </summary>
        ''' <remarks></remarks>
        Private _odrDiv As String

        ''' <summary>
        ''' 受注後活動ID
        ''' </summary>
        ''' <remarks></remarks>
        Private _afterOdrActId As String

        ''' <summary>
        ''' TodoID
        ''' </summary>
        ''' <remarks></remarks>
        Private _todoId As String

        ''' <summary>
        ''' 工程区分
        ''' </summary>
        ''' <remarks></remarks>
        Private _processDiv As String

        ''' <summary>
        ''' 実績日
        ''' </summary>
        ''' <remarks></remarks>
        Private _resultDate As String


        ''' <summary>
        ''' スケジュール作成区分
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CreateScheduleDiv As String
            Get
                Return _createScheduleDiv
            End Get
            Set(value As String)
                _createScheduleDiv = value
            End Set
        End Property

        ''' <summary>
        ''' 活動担当スタッフ店舗コード
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ActivityStaffBranchCode As String
            Get
                Return _activityStaffBranchCode
            End Get
            Set(value As String)
                _activityStaffBranchCode = value
            End Set
        End Property

        ''' <summary>
        ''' 活動スタッフコード
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ActivityStaffCode As String
            Get
                Return _activityStaffCode
            End Get
            Set(value As String)
                _activityStaffCode = value
            End Set
        End Property

        ''' <summary>
        ''' 受付担当スタッフ店舗コード
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ReceptionStaffBranchCode As String
            Get
                Return _receptionStaffBranchCode
            End Get
            Set(value As String)
                _receptionStaffBranchCode = value
            End Set
        End Property

        ''' <summary>
        ''' 受付担当スタッフコード
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ReceptionStaffCode As String
            Get
                Return _receptionStaffCode
            End Get
            Set(value As String)
                _receptionStaffCode = value
            End Set
        End Property

        ''' <summary>
        ''' 接触方法No
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContactNo As String
            Get
                Return _contactNo
            End Get
            Set(value As String)
                _contactNo = value
            End Set
        End Property

        ''' <summary>
        ''' 接触方法名
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContactName As String
            Get
                Return _contactName
            End Get
            Set(value As String)
                _contactName = value
            End Set
        End Property

        ''' <summary>
        ''' 受注後活動名称
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ActOdrName As String
            Get
                Return _actOdrName
            End Get
            Set(value As String)
                _actOdrName = value
            End Set
        End Property

        ''' <summary>
        ''' タイトル
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Summary As String
            Get
                Return _summary
            End Get
            Set(value As String)
                _summary = value
            End Set
        End Property

        ''' <summary>
        ''' 開始日時
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StartTime As String
            Get
                Return _startTime
            End Get
            Set(value As String)
                _startTime = value
            End Set
        End Property

        ''' <summary>
        ''' 終了日時
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EndTime As String
            Get
                Return _endTime
            End Get
            Set(value As String)
                _endTime = value
            End Set
        End Property

        ''' <summary>
        ''' 説明（メモ）
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Memo As String
            Get
                Return _memo
            End Get
            Set(value As String)
                _memo = value
            End Set
        End Property

        ''' <summary>
        ''' 色指定
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property XIcropColor As String
            Get
                Return _xIcropColor
            End Get
            Set(value As String)
                _xIcropColor = value
            End Set
        End Property

        ''' <summary>
        ''' アラーム起動タイミングリスト
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property AlarmTriggerList As List(Of String)
            Get
                Return _alarmTriggerList
            End Get
        End Property

        ''' <summary>
        ''' 受注区分
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OdrDiv As String
            Get
                Return _odrDiv
            End Get
            Set(value As String)
                _odrDiv = value
            End Set
        End Property

        ''' <summary>
        ''' 受注後活動ID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AfterOdrActId As String
            Get
                Return _afterOdrActId
            End Get
            Set(value As String)
                _afterOdrActId = value
            End Set
        End Property

        ''' <summary>
        ''' TodoId
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TodoId As String
            Get
                Return _todoId
            End Get
            Set(value As String)
                _todoId = value
            End Set
        End Property

        ''' <summary>
        ''' 工程区分
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProcessDiv As String
            Get
                Return _processDiv
            End Get
            Set(value As String)
                _processDiv = value
            End Set
        End Property

        ''' <summary>
        ''' 実績日
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ResultDate As String
            Get
                Return _resultDate
            End Get
            Set(value As String)
                _resultDate = value
            End Set
        End Property


        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            Me._alarmTriggerList = New List(Of String)

        End Sub

    End Class

End Namespace
