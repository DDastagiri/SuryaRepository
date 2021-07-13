Public Class ChipDetail

    ''' <summary>ステータス</summary>
    Public Property Status As String

    ''' <summary>納車見込時刻</summary>
    Public Property DeliveryHopeDate As String

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>納車見込時刻</summary>
    Public Property DeliveryHopeDateTime As Date
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    ''' <summary>納車予定時刻</summary>
    Public Property DeliveryPlanDate As String

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>納車見込時刻</summary>
    Public Property DeliveryPlanDateTime As Date
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    ''' <summary>納車予定時刻変更回数</summary>
    Public Property DeliveryPlanDateUpdateCount As Long

    ''' <summary>車両登録No</summary>
    Public Property VehicleRegNo As String

    ''' <summary>顧客名</summary>
    Public Property CustomerName As String

    ''' <summary>電話番号</summary>
    Public Property TelNo As String

    ''' <summary>携帯電話番号</summary>
    Public Property Mobile As String

    ''' <summary>整備内容</summary>
    Public Property MerchandiseName As String

    ''' <summary>待ち方</summary>
    Public Property ReserveReception As String

    ''' <summary>予約マーク</summary>
    Public Property WalkIn As String

    ''' <summary>車種</summary>
    Public Property VehicleName As String

    ''' <summary>グレード</summary>
    Public Property Grade As String

    ''' <summary>Jdpマーク</summary>
    Public Property JdpType As String

    ''' <summary>Sscマーク</summary>
    Public Property SscType As String

    ''' <summary>中断理由</summary>
    Private _StopReasonList As New List(Of StopReason)

    ''' <summary>納車予定時刻変更</summary>
    Private _DeliveryChgList As New List(Of DeliveryChg)

    ''' <summary>来店実績有無</summary>
    Public Property VisitType As String

    ''' <summary>表示区分</summary>
    Public Property DisplayType As Integer

    ''' <summary>顧客区分</summary>
    Public Property CustomerType As String

    ''' <summary>作業開始有無</summary>
    Public Property WorkStartType As String

    ''' <summary>中断有無</summary>
    Public Property StopType As String

    ''' <summary>洗車有無</summary>
    Public Property WashType As String

    ''' <summary>残作業時間(分)</summary>
    Public Property RemainingWorkTime As Long

    ''' <summary>作業終了予定時刻(最終)</summary>
    Public Property WorkEndPlanDateLast As DateTime

    ''' <summary>洗車開始時刻</summary>
    Public Property WashStartDate As DateTime

    ''' <summary>洗車終了時刻</summary>
    Public Property WashEndDate As DateTime

    ''' <summary>R/O有無</summary>
    Public Property OrderDataType As String

    ''' <summary>R/Oステータス</summary>
    Public Property OrderStatus As String

    ''' <summary>部品準備待ちフラグ</summary>
    Public Property PartsPreparationWaitType As String

    ''' <summary>完成検査フラグ</summary>
    Public Property CompleteExaminationType As String

    ''' <summary>追加作業ステータス</summary>
    Public Property AddWorkStatus As String

    ''' <summary>起票者</summary>
    Public Property ReissueVouchers As String

    ''' <summary>完成検査完了時刻</summary>
    Public Property CompleteExaminationEndDate As DateTime

    ''' <summary>清算書印刷時刻</summary>
    Public Property StatementPrintDate As DateTime

    ''' <summary>VIN</summary>
    Public Property Vin As String

    ''' <summary>モデル</summary>
    Public Property Model As String

    ''' <summary>ステータスコード(左)</summary>
    Public Property StatusLeft As String

    ''' <summary>ステータスコード(右)</summary>
    Public Property StatusRight As String

    ''' <summary>起票者ストール名</summary>
    Public Property AddAccountName As String
    '丁　START
    ''' <summary>更新日</summary>
    Public Property UpdateDate As DateTime
    ''' <summary>呼出No.</summary>
    Public Property CallNO As String
    ''' <summary>呼出場所</summary>
    Public Property CallPlace As String

    ''' <summary>呼出ステータス</summary>
    Public Property CallStatus As String

    ''' <summary>来店者氏名</summary>
    Public Property VisitName As String

    ''' <summary>来店者電話番号</summary>
    Public Property VisitTelNO As String
    '丁　END

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>ご用命</summary>
    Public Property OrderMemo As String

    ''' <summary>故障原因</summary>
    Public Property FailureCause As String

    ''' <summary>診断結果</summary>
    Public Property DiagnosticResult As String

    ''' <summary>作業結果及びアドバイス</summary>
    Public Property WorkResultAdvice As String
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ''' <summary>車両登録エリア名称</summary>
    Public Property RegisterAreaName As String

    ''' <summary>サービス入庫テーブル行ロックバージョン</summary>
    Public Property ServiceinLockVersion As Long
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
    ''' <summary>顧客車両区分</summary>
    Public Property CustomerVehicleType As String

    ''' <summary>顧客種別(顧客氏名用)</summary>
    Public Property NameCustomerType As String
    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

    Public ReadOnly Property DeliveryChgList() As List(Of DeliveryChg)
        Get
            Return _DeliveryChgList
        End Get
    End Property

    Public ReadOnly Property StopReasonList() As List(Of StopReason)
        Get
            Return _StopReasonList
        End Get
    End Property

End Class
