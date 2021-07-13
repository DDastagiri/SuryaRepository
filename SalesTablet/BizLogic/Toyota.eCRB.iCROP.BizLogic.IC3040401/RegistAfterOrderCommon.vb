'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'XmlCommon.vb
'──────────────────────────────────
'機能： CalDAV連携インターフェース
'補足： 
'作成： 2014/04/24 TMEJ t.mizumoto 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計
'──────────────────────────────────
Namespace Toyota.eCRB.iCROP.BizLogic.IC3040401

    Public Class RegistAfterOrderCommon

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
        ''' <remarks></remarks>
        Private _scheduleId As String

        ''' <summary>
        ''' 処理区分
        ''' </summary>
        ''' <remarks></remarks>
        Private _actionType As String

        ''' <summary>
        ''' 活動作成スタッフコード
        ''' </summary>
        ''' <remarks></remarks>
        Private _activityCreateStaff As String


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
        ''' 活動作成スタッフコード
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

    End Class

End Namespace
