'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'XmlRegistAfterOrderDetail.vb
'──────────────────────────────────
'機能： CalDAV連携インターフェース
'補足： 
'作成： 2014/04/24 TMEJ t.mizumoto 受注後フォロー機能開発（スタッフ活動KPI）に向けたシステム設計
'──────────────────────────────────
Namespace Toyota.eCRB.iCROP.BizLogic.IC3040401

    Public Class RegistAfterOrderDetail

        ''' <summary>
        ''' Common要素
        ''' </summary>
        ''' <remarks></remarks>
        Private _common As RegistAfterOrderCommon

        ''' <summary>
        ''' ScheduleInfo要素
        ''' </summary>
        ''' <remarks></remarks>
        Private _scheduleInfo As RegistAfterOrderScheduleInfo

        ''' <summary>
        ''' Schedule要素のリスト
        ''' </summary>
        ''' <remarks></remarks>
        Private _scheduleList As List(Of RegistAfterOrderSchedule)


        ''' <summary>
        ''' Common要素
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Common As RegistAfterOrderCommon
            Get
                Return _common
            End Get
            Set(value As RegistAfterOrderCommon)
                _common = value
            End Set
        End Property

        ''' <summary>
        ''' ScheduleInfo要素
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScheduleInfo As RegistAfterOrderScheduleInfo
            Get
                Return _scheduleInfo
            End Get
            Set(value As RegistAfterOrderScheduleInfo)
                _scheduleInfo = value
            End Set
        End Property

        ''' <summary>
        ''' Schedule要素のリスト
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ScheduleList As List(Of RegistAfterOrderSchedule)
            Get
                Return _scheduleList
            End Get
        End Property


        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            Me._scheduleList = New List(Of RegistAfterOrderSchedule)

        End Sub

    End Class

End Namespace
