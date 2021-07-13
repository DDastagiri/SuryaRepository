﻿' "データ格納クラス"
' XMLの<TodoEvent>要素の値一覧です。
Public Class XmlSchedule

    ''' <summary>
    ''' スケジュール作成区分
    ''' </summary>
    ''' <remarks></remarks>
    Private _createScheduleDiv As String

    ''' <summary>
    ''' 活動生成スタッフ店舗コード
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
    ''' <remarks>活動内容を表すコード(1:来店予約 2:CALL-IN 3:CALL-OUT 4:SMS 5:E-Mail 6:DM)</remarks>
    Private _contactNo As String

    ''' <summary>
    ''' タイトル
    ''' </summary>
    ''' <remarks>表示するタイトル名</remarks>
    Private _summary As String

    ''' <summary>
    ''' 開始日時
    ''' </summary>
    ''' <remarks>スケジュールの開始日時</remarks>
    Private _startTime As String

    ''' <summary>
    ''' 終了日時
    ''' </summary>
    ''' <remarks>スケジュールの終了日時</remarks>
    Private _endTime As String

    ''' <summary>
    ''' 説明(メモ)
    ''' </summary>
    ''' <remarks>イベントに表示するメモ</remarks>
    Private _memo As String

    ''' <summary>
    ''' 色設定
    ''' </summary>
    ''' <remarks>イベントの色設定(16進RGBA)</remarks>
    Private _xIcropColor As String

    ''' <summary>
    ''' アラーム起動タイミング
    ''' </summary>
    ''' <remarks>アラームを起動するタイミング(0:なし 1:5分 2:15分 3:30分 4:1時間前 5:2時間 6:1日前 7:2日前 )</remarks>
    Private _alarmTriggerList As List(Of String)

    ''' <summary>
    ''' TodoID
    ''' </summary>
    ''' <remarks>登録・完了：Eventを追加する場合の、親となるTodoのID  更新：更新対象のスケジュールのTodoのID</remarks>
    Private _todoId As String


    ''' <summary>
    ''' 親子区分
    ''' </summary>
    ''' <remarks>完了フラグを立てるとき、時間が同一である場合の判別フラグ</remarks>
    Private _parentDiv As String

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
    ''' 受付担当
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
    ''' アラーム起動タイミングリスト初期化
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitialAlarmTriggerList()
        _alarmTriggerList = New List(Of String)
    End Sub

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
    ''' 親子区分
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ParentDiv As String
        Get
            Return _parentDiv
        End Get
        Set(value As String)
            _parentDiv = value
        End Set
    End Property

End Class
