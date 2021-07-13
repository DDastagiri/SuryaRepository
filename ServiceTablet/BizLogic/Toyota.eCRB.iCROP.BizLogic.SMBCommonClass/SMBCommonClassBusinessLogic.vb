'-------------------------------------------------------------------------
'SMBCommonClassBusinessLogic.vb
'-------------------------------------------------------------------------
'機能：共通関数API
'補足：
'作成：2012/05/11 KN 河原
'更新：2012/06/06 KN 小澤 STEP2事前準備対応
'更新：2012/06/19 KN 小澤 STEP2対応(事前準備用の処理削除)
'更新：2012/07/12 TMEJ 小澤 STEP2対応(ステータス判定処理追加)
'更新：2012/07/12 TMEJ 小澤 STEP2対応(入庫日時付替え処理追加)
'更新：2012/08/15 TMEJ 日比野 STEP2対応(顧客区分がNULLの場合は未取引客とするように修正)
'更新：2012/09/11 TMEJ 日比野 SMメインメニューの古いチップが消えない不具合対応
'更新：2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応
'更新：2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応
'更新：2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応
'更新：2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
'更新：2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新：2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新：2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
'更新：2014/02/08 TMEJ 小澤 BTS対応
'更新：2014/04/08 TMEJ 小澤 BTS-378対応
'更新：2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加
'更新：2015/02/09 TMEJ 明瀬 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発
'更新：2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新：
'─────────────────────────────────────
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess
'2015/02/09 TMEJ 明瀬 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess.SMBCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess.SMBCommonClassDataSetTableAdapters
'2015/02/09 TMEJ 明瀬 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
''2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804.IC3800804DataSet
''2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応

''2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801012
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801012
''2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
'2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

'2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
'2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START

Public Class SMBCommonClassBusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "デフォルトコンストラクタ処理"

    ''' <summary>
    ''' デフォルトコンストラクタ処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        DTStallTime = New SMBCommonClassDataSet.StallTimeDataTable
        DTNonWorkDays = New SMBCommonClassDataSet.NonWorkDaysDataTable
        DTStandardLTList = New IC3810701DataSet.StandardLTListDataTable
    End Sub

#End Region

#Region "PublicConst"

    ''' <summary>
    ''' 登録区分
    ''' </summary>
    Public Enum RegisterType

        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        ' ''' <summary>
        ' ''' 新規登録
        ' ''' </summary>
        'ReserveHisNew = 0

        ' ''' <summary>
        ' ''' 個別登録
        ' ''' </summary>
        'ReserveHisIndividual = 1

        ' ''' <summary>
        ' ''' 全て登録
        ' ''' </summary>
        'ReserveHisAll = 2

        ' ''' <summary>
        ' ''' 削除時の登録
        ' ''' </summary>
        'ReserveHisDelete = 9

        ''' <summary>
        ''' サービス入庫テーブル
        ''' </summary>
        RegisterServiceIn = 0

        ''' <summary>
        ''' 作業内容テーブル
        ''' </summary>
        RegisterJobDetail = 1

        ''' <summary>
        ''' ストール利用テーブル
        ''' </summary>
        RegisterStallUse = 2

        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
    End Enum

    ''' <summary>
    ''' リターンコード
    ''' </summary>
    Public Enum ReturnCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        Success = 0

        ''' <summary>
        ''' DBタイムアウト
        ''' </summary>
        ErrDBTimeout = 901

        ''' <summary>
        ''' 失敗(0件)
        ''' </summary>
        ErrNoCases = 902

        ''' <summary>
        ''' 引数エラー
        ''' </summary>
        ErrArgument = 903

        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        ''' <summary>
        ''' 他端末に更新されていた場合
        ''' </summary>
        ErrorDBConcurrency = 904

        ''' <summary>
        ''' データ取得件数が0件の場合
        ''' </summary>
        ErrorNoDataFound = 905
        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        ''' <summary>
        ''' 登録区分エラー
        ''' </summary>
        ErrType = 999

    End Enum

    ''' <summary>
    ''' 活動ID（未設定）
    ''' </summary>
    Public Const NoActivityId As Long = 0

    ''' <summary>
    ''' ストールロック用キャンセルフラグ（0：キャンセルチップを含まない）
    ''' </summary>
    Public Const StallLockCancelTypeNone As String = "0"

    ''' <summary>
    ''' ストールロック用キャンセルフラグ（1：キャンセルチップを含む）
    ''' </summary>
    Public Const StallLockCancelTypeAllChip As String = "1"
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    ''' <summary>
    ''' 表示区分
    ''' </summary>
    Public Enum DisplayType

        ''' <summary>
        ''' 受付中
        ''' </summary>
        Invalid = 1
        ''' <summary>
        ''' 追加承認
        ''' </summary>
        AddApprove = 2
        ''' <summary>
        ''' 納車準備
        ''' </summary>
        DeliveryPreparation = 3
        ''' <summary>
        ''' 納車作業
        ''' </summary>
        DeliveryWork = 4
        ''' <summary>
        ''' 作業中
        ''' </summary>
        Work = 5
        ''' <summary>
        ''' 表示区分不正
        ''' </summary>
        Err = 0

    End Enum

    ''' <summary>
    ''' 管理予約IDが存在しない場合
    ''' </summary>
    Public Const NoReserveId As Long = -1

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' 来店無し
    ''' </summary>
    Public Const NoVisit As String = "0"
    ''' <summary>
    ''' 来店有り
    ''' </summary>
    Public Const Visit As String = "1"
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' SMBのチップ位置情報
    ''' </summary>
    Public Enum SmbChipAreaType

        ''' <summary>
        ''' なし
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' ストール
        ''' </summary>
        ''' <remarks></remarks>
        Stall = 1

        ''' <summary>
        ''' 受付タブ
        ''' </summary>
        ''' <remarks></remarks>
        Receptionist = 2

        ''' <summary>
        ''' 追加作業タブ
        ''' </summary>
        ''' <remarks></remarks>
        AddWord = 3

        ''' <summary>
        ''' 完成検査タブ
        ''' </summary>
        ''' <remarks></remarks>
        Inspection = 4

        ''' <summary>
        ''' 洗車
        ''' </summary>
        ''' <remarks></remarks>
        CarWash = 5

        ''' <summary>
        ''' 納車待ち
        ''' </summary>
        ''' <remarks></remarks>
        WaitDelivery = 6

        ''' <summary>
        ''' 中断
        ''' </summary>
        ''' <remarks></remarks>
        ChipStop = 7

        ''' <summary>
        ''' NoShow
        ''' </summary>
        ''' <remarks></remarks>
        NoShow = 8

    End Enum

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

#End Region

#Region "定数"
    ''' <summary>
    ''' R/O無効
    ''' </summary>
    Private Const ROInvalid As String = "0"
    ''' <summary>
    '''  R/O有効
    ''' </summary>
    Private Const ROEffective As String = "1"

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' R/Oｽﾃｰﾀｽ(受付)
    ' ''' </summary>
    'Private Const ROReceptionist As String = "1"
    ' ''' <summary>
    ' ''' R/Oｽﾃｰﾀｽ(整備中)
    ' ''' </summary>
    'Private Const ROMaintenance As String = "2"
    ' ''' <summary>
    ' ''' R/Oｽﾃｰﾀｽ(売上済)
    ' ''' </summary>
    'Private Const ROFinSales As String = "3"
    ' ''' <summary>
    ' ''' R/Oｽﾃｰﾀｽ(部品待ち)
    ' ''' </summary>
    'Private Const ROParts As String = "4"
    ' ''' <summary>
    ' ''' R/Oｽﾃｰﾀｽ(見積確定待ち)
    ' ''' </summary>
    'Private Const ROEstimate As String = "5"
    ' ''' <summary>
    ' ''' ROｽﾃｰﾀｽ(整備完了)
    ' ''' </summary>
    'Private Const ROFinMaintenance As String = "6"
    ' ''' <summary>
    ' ''' ROｽﾃｰﾀｽ(検査完了)
    ' ''' </summary>
    'Private Const ROFinInspection As String = "7"
    ' ''' <summary>
    ' ''' ROｽﾃｰﾀｽ(納車完了)
    ' ''' </summary>
    'Private Const ROFinDelivery As String = "8"

    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(R/O未起票)
    ''' </summary>
    Private Const RONoneReissuing As String = "00"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(SA起票中)
    ''' </summary>
    Private Const ROReissuingSA As String = "10"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(TC起票中)
    ''' </summary>
    Private Const ROReissuingTC As String = "15"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(FM承認待ち)
    ''' </summary>
    Private Const ROWaitRecognitionFM As String = "20"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(部品仮見積中)
    ''' </summary>
    Private Const ROPartsDemoEstimate As String = "25"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(部品本見積中)
    ''' </summary>
    Private Const ROPartsMasterEstimate As String = "30"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(顧客承認待ち)
    ''' </summary>
    Private Const ROWaitCustomerRecognition As String = "40"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(着工指示待ち(顧客承認完了))
    ''' </summary>
    Private Const ROWaitStruct As String = "50"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(作業開始待ち)
    ''' </summary>
    Private Const ROWaitWorkStart As String = "55"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(作業中)
    ''' </summary>
    Private Const ROWorking As String = "60"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(完成検査依頼中)
    ''' </summary>
    Private Const ROInspectRequest As String = "65"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(完成検査完了)
    ''' </summary>
    Private Const ROInspectFinish As String = "70"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(納車準備待ち)
    ''' </summary>
    Private Const ROWaitDeliveryPreparation As String = "80"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(納車作業中(清算中))
    ''' </summary>
    Private Const ROWorkingDelivery As String = "85"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(納車済み)
    ''' </summary>
    Private Const RODeliveryFinish As String = "90"
    ''' <summary>
    ''' R/Oｽﾃｰﾀｽ(R/Oキャンセル)
    ''' </summary>
    Private Const ROCancel As String = "99"

    ''' <summary>
    ''' 作業終了有無（0:作業中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WorkEndTypeWorking As String = "0"
    ''' <summary>
    ''' 作業終了有無（1:作業終了）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WorkEndTypeWorkEnd As String = "1"

    ''' <summary>
    ''' 洗車終了有無（0:洗車中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CarWashEndTypeWashing As String = "0"
    ''' <summary>
    ''' 洗車終了有無（1:洗車終了）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CarWashEndTypeWashEnd As String = "1"

    ''' <summary>
    ''' 振当ステータス（2:SA振当済）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AssignStatusAssignment As String = "2"

    ''' <summary>
    ''' RO情報有無（0:RO情報なし）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderTypeNone As String = "0"
    ''' <summary>
    ''' RO情報有無（1:RO情報あり）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderTypeExist As String = "1"

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ' ''' <summary>
    ' ''' ROｽﾃｰﾀｽ(洗車待ち)
    ' ''' </summary>
    'Private Const SMWaitWash As String = "40"
    ' ''' <summary>
    ' ''' ROｽﾃｰﾀｽ(洗車中)
    ' ''' </summary>
    'Private Const SMWash As String = "41"
    ' ''' <summary>
    ' ''' ROｽﾃｰﾀｽ(預かり中)
    ' ''' </summary>
    'Private Const SMCustody As String = "50"
    ' ''' <summary>
    ' ''' ROｽﾃｰﾀｽ(納車待ち)
    ' ''' </summary>
    'Private Const SMDelivery As String = "60"
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '2012/09/11 TMEJ 日比野 SMメインメニューの古いチップが消えない不具合対応 START

    ''' <summary>
    ''' ROｽﾃｰﾀｽ(完了)
    ''' </summary>
    Private Const SMFinish As String = "99"
    '2012/09/11 TMEJ 日比野 SMメインメニューの古いチップが消えない不具合対応 END

    ''' <summary>
    ''' 洗車無し
    ''' </summary>
    Private Const NoWashFlag As String = "0"
    ''' <summary>
    '''  洗車有り
    ''' </summary>
    Private Const WashFlag As String = "1"

    '2012/07/12 KN 小澤 STEP2対応(ステータス判定処理追加) START
    ''' <summary>
    ''' プログラムID
    ''' </summary>
    Private Const WordProgramID As String = "SMBCommonClass"

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ' ''' <summary>
    ' ''' 来店無し
    ' ''' </summary>
    'Private Const NoVisit As String = "0"
    ' ''' <summary>
    ' ''' 来店有り
    ' ''' </summary>
    'Private Const Visit As String = "1"
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    ''' <summary>
    ''' 自社客
    ''' </summary>
    Private Const CompanyVisitor As String = "1"
    ''' <summary>
    ''' 未取引客
    ''' </summary>
    Private Const NonBusinessGuest As String = "2"

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ' ''' <summary>
    ' ''' 未着工
    ' ''' </summary>
    'Private Const NoGroundbreaking As String = "0"
    ' ''' <summary>
    ' ''' 着工準備
    ' ''' </summary>
    'Private Const GroundbreakingPreparation As String = "2"

    ''' <summary>
    ''' 未着工
    ''' </summary>
    Private Const NoGroundbreaking As String = "00"
    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 部品準備待ち
    ' ''' </summary>
    'Private Const PartsPreparationWaiting As String = "0"
    ' ''' <summary>
    ' ''' 部品準備中
    ' ''' </summary>
    'Private Const PartsInPreparation As String = "1"
    ' ''' <summary>
    ' ''' 部品準備済み
    ' ''' </summary>
    'Private Const PartsPreparationFinish As String = "2"
    ' ''' <summary>
    ' ''' 部品準備不要
    ' ''' </summary>
    'Private Const PartsPreparationNeedlessness As String = "3"
    ''' <summary>
    ''' 部品準備待ち
    ''' </summary>
    Private Const PartsPreparationWaiting As String = "0"
    ''' <summary>
    ''' 部品準備中
    ''' </summary>
    Private Const PartsInPreparation As String = "1"
    ''' <summary>
    ''' 部品準備済み
    ''' </summary>
    Private Const PartsPreparationFinish As String = "8"
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 作業開始無し
    ''' </summary>
    Private Const NoWorkStart As String = "0"
    ''' <summary>
    ''' 作業開始有り
    ''' </summary>
    Private Const WorkStart As String = "1"

    ''' <summary>
    ''' 中断無し
    ''' </summary>
    Private Const NoDiscontinuation As String = "0"
    ''' <summary>
    ''' 中断有り
    ''' </summary>
    Private Const Discontinuation As String = "1"

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 完成検査無し
    ' ''' </summary>
    'Private Const NoCompleteExamination As String = "0"
    ' ''' <summary>
    ' ''' 完成検査有り
    ' ''' </summary>
    'Private Const CompleteExamination As String = "1"
    ''' <summary>
    ''' 完成検査未完了
    ''' </summary>
    Private Const NoCompleteExamination As String = "0"
    ''' <summary>
    ''' 完成検査承認待ち
    ''' </summary>
    Private Const RequestCompleteExamination As String = "1"
    ''' <summary>
    ''' 完成検査承認済み
    ''' </summary>
    Private Const FinishCompleteExamination As String = "2"
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' 追加作業ステータス(TC起票中)
    ' ''' </summary>
    'Private Const TechnicianInVouchers As String = "1"
    ' ''' <summary>
    ' ''' 追加作業ステータス(CT承認待ち)
    ' ''' </summary>
    'Private Const ControllerRecognitionWaiting As String = "2"
    ' ''' <summary>
    ' ''' 追加作業ステータス(PS部品見積待ち)
    ' ''' </summary>
    'Private Const PartStaffEstimateWaiting As String = "3"
    ' ''' <summary>
    ' ''' 追加作業ステータス(SA見積確定待ち)
    ' ''' </summary>
    'Private Const ServiceAssitantEstimateWaiting As String = "4"
    ' ''' <summary>
    ' ''' 追加作業ステータス(顧客承認待ち)
    ' ''' </summary>
    'Private Const CustomerRecognitionWaiting As String = "5"
    ' ''' <summary>
    ' ''' 追加作業ステータス(CT着工指示・部品準備待ち)
    ' ''' </summary>
    'Private Const ControllerGroundbreakingParts As String = "6"
    ' ''' <summary>
    ' ''' 追加作業ステータス(着工指示待ち/部品準備待ち)
    ' ''' </summary>
    'Private Const GroundbreakingPartsWaiting As String = "7"
    ' ''' <summary>
    ' ''' 追加作業ステータス(整備待ち)
    ' ''' </summary>
    'Private Const MaintenanceWaiting As String = "8"
    ' ''' <summary>
    ' ''' 追加作業ステータス(完成検査完了)
    ' ''' </summary>
    'Private Const CompleteExaminationFinish As String = "9"
    ''' <summary>
    ''' 追加作業ステータス(10:SA起票中)
    ''' </summary>
    Private Const ServiceAssistantInVouchers As String = "10"
    ''' <summary>
    ''' 追加作業ステータス(15:TC起票中)
    ''' </summary>
    Private Const TechnicianInVouchers As String = "15"
    ''' <summary>
    ''' 追加作業ステータス(20:CT承認待ち)
    ''' </summary>
    Private Const ControllerRecognitionWaiting As String = "20"
    ''' <summary>
    ''' 追加作業ステータス(25:PS部品仮見積待ち)
    ''' </summary>
    Private Const PartStaffDummyEstimateWaiting As String = "25"
    ''' <summary>
    ''' 追加作業ステータス(30:PS部品見積待ち)
    ''' </summary>
    Private Const PartStaffEstimateWaiting As String = "30"
    ''' <summary>
    ''' 追加作業ステータス(35:SA見積確定待ち)
    ''' </summary>
    Private Const ServiceAssitantEstimateWaiting As String = "35"
    ''' <summary>
    ''' 追加作業ステータス(40:顧客承認待ち)
    ''' </summary>
    Private Const CustomerRecognitionWaiting As String = "40"
    ''' <summary>
    ''' 追加作業ステータス(50:CT着工指示・部品準備待ち)
    ''' </summary>
    Private Const ControllerGroundbreakingParts As String = "50"
    ''' <summary>
    ''' 追加作業ステータス(60:着工指示待ち/部品準備待ち)
    ''' </summary>
    Private Const GroundbreakingPartsWaiting As String = "60"
    ''' <summary>
    ''' 追加作業ステータス(80:整備待ち)
    ''' </summary>
    Private Const MaintenanceWaiting As String = "80"
    ''' <summary>
    ''' 追加作業ステータス(85:完成検査完了)
    ''' </summary>
    Private Const CompleteExaminationFinish As String = "85"
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' ステータスコード：101：新規お客様登録待ち
    ''' </summary>
    Private Const StatusCodeLeft101 As String = "101"
    ''' <summary>
    ''' ステータスコード：102：R/O作成待ち
    ''' </summary>
    Private Const StatusCodeLeft102 As String = "102"
    ''' <summary>
    ''' ステータスコード：103：R/O作成中
    ''' </summary>
    Private Const StatusCodeLeft103 As String = "103"
    ''' <summary>
    ''' ステータスコード：104：新規お客様登録待ち
    ''' </summary>
    Private Const StatusCodeLeft104 As String = "104"
    ''' <summary>
    ''' ステータスコード：105：R/O作成待ち
    ''' </summary>
    Private Const StatusCodeLeft105 As String = "105"
    ''' <summary>
    ''' ステータスコード：106：R/O作成中
    ''' </summary>
    Private Const StatusCodeLeft106 As String = "106"
    ''' <summary>
    ''' ステータスコード：108：着工指示待ち/部品準備待ち
    ''' </summary>
    Private Const StatusCodeLeft108 As String = "108"
    ''' <summary>
    ''' ステータスコード：109：着工指示待ち/部品準備中
    ''' </summary>
    Private Const StatusCodeLeft109 As String = "109"
    ''' <summary>
    ''' ステータスコード：111：着工指示済み/部品準備待ち
    ''' </summary>
    Private Const StatusCodeLeft111 As String = "111"
    ''' <summary>
    ''' ステータスコード：112：着工指示済み/部品準備中
    ''' </summary>
    Private Const StatusCodeLeft112 As String = "112"
    ''' <summary>
    ''' ステータスコード：110：着工指示待ち/部品準備済み
    ''' </summary>
    Private Const StatusCodeLeft110 As String = "110"
    ''' <summary>
    ''' ステータスコード：107：着工指示待ち
    ''' </summary>
    Private Const StatusCodeLeft107 As String = "107"
    ''' <summary>
    ''' ステータスコード：113：作業開始待ち
    ''' </summary>
    Private Const StatusCodeLeft113 As String = "113"
    ''' <summary>
    ''' ステータスコード：115：中断中
    ''' </summary>
    Private Const StatusCodeLeft115 As String = "115"
    ''' <summary>
    ''' ステータスコード：114：作業中
    ''' </summary>
    Private Const StatusCodeLeft114 As String = "114"
    ''' <summary>
    ''' ステータスコード：116：完成検査待ち
    ''' </summary>
    Private Const StatusCodeLeft116 As String = "116"
    ''' <summary>
    ''' ステータスコード：117：納車準備待ち
    ''' </summary>
    Private Const StatusCodeLeft117 As String = "117"
    ''' <summary>
    ''' ステータスコード：118：洗車待ち/納車準備待ち
    ''' </summary>
    Private Const StatusCodeLeft118 As String = "118"
    ''' <summary>
    ''' ステータスコード：119：洗車中/納車準備待ち
    ''' </summary>
    Private Const StatusCodeLeft119 As String = "119"
    ''' <summary>
    ''' ステータスコード：120：洗車完了/納車準備待ち
    ''' </summary>
    Private Const StatusCodeLeft120 As String = "120"
    ''' <summary>
    ''' ステータスコード：121：納車待ち
    ''' </summary>
    Private Const StatusCodeLeft121 As String = "121"
    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    ''' <summary>
    ''' ステータスコード：122：中断中
    ''' </summary>
    Private Const StatusCodeLeft122 As String = "122"
    ''' <summary>
    ''' ステータスコード：123：作業開始待ち
    ''' </summary>
    Private Const StatusCodeLeft123 As String = "123"
    ''' <summary>
    ''' ステータスコード：124：作業中
    ''' </summary>
    Private Const StatusCodeLeft124 As String = "124"
    ''' <summary>
    ''' ステータスコード：125：作業中
    ''' </summary>
    Private Const StatusCodeLeft125 As String = "125"
    ''' <summary>
    ''' ステータスコード：126：完成検査承認待ち
    ''' </summary>
    Private Const StatusCodeLeft126 As String = "126"
    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END
    ' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' ステータスコード：127：仮R/O作成待ち
    ''' </summary>
    Private Const StatusCodeLeft127 As String = "127"
    ''' <summary>
    ''' ステータスコード：128：仮R/O作成中
    ''' </summary>
    Private Const StatusCodeLeft128 As String = "128"
    ''' <summary>
    ''' ステータスコード：129：仮R/O作成済み
    ''' </summary>
    Private Const StatusCodeLeft129 As String = "129"
    ''' <summary>
    ''' ステータスコード：130：作業完了
    ''' </summary>
    Private Const StatusCodeLeft130 As String = "130"
    ''' <summary>
    ''' ステータスコード：131：追加作業承認待ち
    ''' </summary>
    Private Const StatusCodeLeft131 As String = "131"
    ''' <summary>
    ''' ステータスコード：132：洗車待ち/納車準備済み
    ''' </summary>
    Private Const StatusCodeLeft132 As String = "132"
    ''' <summary>
    ''' ステータスコード：133：洗車中/納車準備済み
    ''' </summary>
    Private Const StatusCodeLeft133 As String = "133"
    ''' <summary>
    ''' ステータスコード：134：納車完了
    ''' </summary>
    Private Const StatusCodeLeft134 As String = "134"
    ' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' ステータスコード：135：SA振当待ち
    ''' </summary>
    Private Const StatusCodeLeft135 As String = "135"
    ''' <summary>
    ''' ステータスコード：136：SA振当待ち
    ''' </summary>
    Private Const StatusCodeLeft136 As String = "136"
    ''' <summary>
    ''' ステータスコード：137：SA振当待ち
    ''' </summary>
    Private Const StatusCodeLeft137 As String = "137"
    ''' <summary>
    ''' ステータスコード：138：完成検査承認済み
    ''' </summary>
    Private Const StatusCodeLeft138 As String = "138"
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' ステータスコード：199：非表示
    ''' </summary>
    Private Const StatusCodeLeft199 As String = "199"

    ''' <summary>
    ''' ステータスコード：201：TC追加作業起票中
    ''' </summary>
    Private Const StatusCodeRight201 As String = "201"
    ''' <summary>
    ''' ステータスコード：202：CT承認待ち
    ''' </summary>
    Private Const StatusCodeRight202 As String = "202"
    ''' <summary>
    ''' ステータスコード：203：部品見積り待ち
    ''' </summary>
    Private Const StatusCodeRight203 As String = "203"
    ''' <summary>
    ''' ステータスコード：205：SA追加作業起票中
    ''' </summary>
    Private Const StatusCodeRight205 As String = "205"
    ''' <summary>
    ''' ステータスコード：206：SA見積り確定待ち
    ''' </summary>
    Private Const StatusCodeRight206 As String = "206"
    ''' <summary>
    ''' ステータスコード：207：お客様承認待ち
    ''' </summary>
    Private Const StatusCodeRight207 As String = "207"
    ''' <summary>
    ''' ステータスコード：208：非表示
    ''' </summary>
    Private Const StatusCodeRight208 As String = "208"
    ''' <summary>
    ''' ステータスコード：209：非表示
    ''' </summary>
    Private Const StatusCodeRight209 As String = "209"
    ''' <summary>
    ''' ステータスコード：210：非表示
    ''' </summary>
    Private Const StatusCodeRight210 As String = "210"
    ''' <summary>
    ''' ステータスコード：211：非表示
    ''' </summary>
    Private Const StatusCodeRight211 As String = "211"
    ''' <summary>
    ''' ステータスコード：299：非表示
    ''' </summary>
    Private Const StatusCodeRight299 As String = "299"

    ''' <summary>
    ''' 起票者：1：TC
    ''' </summary>
    Private Const ReissueVouchersTC As String = "1"
    ''' <summary>
    ''' 起票者：2：SA
    ''' </summary>
    Private Const ReissueVouchersSA As String = "2"
    '2012/07/12 KN 小澤 STEP2対応(ステータス判定処理追加) END

    ''' <summary>
    ''' 店舗の営業開始・終了時刻
    ''' </summary>
    Private DTStallTime As SMBCommonClassDataSet.StallTimeDataTable
    ''' <summary>
    ''' 店舗の非稼働日取得
    ''' </summary>
    Private DTNonWorkDays As SMBCommonClassDataSet.NonWorkDaysDataTable
    ''' <summary>
    ''' サービス標準LT
    ''' </summary>
    Private DTStandardLTList As IC3810701DataSet.StandardLTListDataTable

    '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 START

    ''' <summary>
    ''' 予約無効
    ''' </summary>
    Private Const ReserveInvalid As String = "0"
    ''' <summary>
    '''  予約有効
    ''' </summary>
    Private Const ReserveEffective As String = "1"
    '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 END

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' 日付最小値文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DateMinValue As String = "1900/01/01 00:00:00"

    ''' <summary>
    ''' サービスステータス（02：キャンセル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusCancel As String = "02"

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' サービスステータス(07：洗車待ち)
    ''' </summary>
    Private Const ServiceStatusWaitCarWash As String = "07"
    ''' <summary>
    ''' サービスステータス(08：洗車中)
    ''' </summary>
    Private Const ServiceStatusCarWashing As String = "08"
    ''' <summary>
    ''' サービスステータス(11：預かり中)
    ''' </summary>
    Private Const ServiceStatusDropOff As String = "11"
    ''' <summary>
    ''' サービスステータス(12：納車待ち)
    ''' </summary>
    Private Const ServiceStatusWaitDalivery As String = "12"
    ''' <summary>
    ''' サービスステータス(13：納車済み)
    ''' </summary>
    Private Const ServiceStatusFinishDelivery As String = "13"

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 基幹顧客ID変換パターン取得キー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BaseCustomerCodeKey As String = "DMSCustomerCodeFlg"

    ''' <summary>
    ''' 基幹顧客コード置換フラグ（0：販売店コード追加）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReplaceBaseCodeAdd As String = "0"

    ''' <summary>
    ''' ストール利用テータス（05：中断）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusStop As String = "05"

    ''' <summary>
    ''' ストールロック経過時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallLockCheckSecond As Long = 10

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START

    ''' <summary>
    ''' 全販売店を意味するワイルドカード販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllDealerCode As String = "XXXXX"

    ''' <summary>
    ''' 全店舗を意味するワイルドカード店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllBranchCode As String = "XXX"

    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' RO情報ステータス（60:作業中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderStatusWorking As String = "60"

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    '2014/02/08 TMEJ 小澤 BTS対応 START

    ''' <summary>
    ''' サービスステータス（01：Noshow）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusNoShow As String = "01"

    '2014/02/08 TMEJ 小澤 BTS対応 END

    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
    ''' <summary>
    ''' 残完成検査区分(0：完成検査入力未完了)
    ''' </summary>
    Private Const NotFinishFinalInspection As String = "0"
    ''' <summary>
    ''' 残完成検査区分(1：完成検査承認待ち)
    ''' </summary>
    Private Const WaitingFinalInspection As String = "1"
    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

#End Region

    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START
    Private LogServiceCommonBiz As New ServiceCommonClassBusinessLogic(True)
    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 END

#Region "Publicメソッド"

    ''' <summary>
    ''' ストール予約履歴登録
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inWhereKey">サービス入庫ID or 作業内容ID or ストール利用ID</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inRegisterType">登録区分</param>
    ''' <param name="inAccount">アカウント</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <returns>登録結果</returns>
    ''' <history>2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発</history>
    ''' <remarks></remarks>
    Public Function RegisterStallReserveHis(ByVal inDealerCode As String, _
                                            ByVal inStoreCode As String, _
                                            ByVal inWhereKey As Decimal, _
                                            ByVal inPresentTime As Date, _
                                            ByVal inRegisterType As RegisterType, _
                                            ByVal inAccount As String, _
                                            ByVal inSystem As String, _
                                            ByVal inActionId As Decimal) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , inDealerCode, inStoreCode, inWhereKey, inPresentTime, inRegisterType, inAccount, inSystem, inActionId))
        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        'Public Function RegistDBStallReserveHis(ByVal inDealerCode As String, _
        '                                        ByVal inStoreCode As String, _
        '                                        ByVal inReserveId As Long, _
        '                                        ByVal inPresentTime As Date, _
        '                                        ByVal inRegisterType As Integer) As Integer
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6}" _
        '            , Me.GetType.ToString _
        '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '            , inDealerCode, inStoreCode, inReserveId, inPresentTime, inRegisterType))
        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        Dim dataSet As SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter = Nothing
        Try
            Dim insertCount As Integer '登録件数
            Dim returnValue As Long '返却コード
            '登録区分の判定
            Select Case inRegisterType
                '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'Case RegisterType.ReserveHisNew, _
                '     RegisterType.ReserveHisIndividual, _
                '     RegisterType.ReserveHisAll, _
                '     RegisterType.ReserveHisDelete
                Case RegisterType.RegisterServiceIn, _
                     RegisterType.RegisterJobDetail, _
                     RegisterType.RegisterStallUse
                    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                    dataSet = New SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter
                    '履歴登録処理
                    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'insertCount = dataSet.RegisterDBStallReserveHis(inDealerCode, _
                    '                                              inStoreCode, _
                    '                                              inReserveId, _
                    '                                              inPresentTime, _
                    '                                              inRegisterType)
                    insertCount = dataSet.RegisterDBStallReserveHis(inDealerCode, _
                                                                  inStoreCode, _
                                                                  inWhereKey, _
                                                                  inPresentTime, _
                                                                  inRegisterType, _
                                                                  inAccount, _
                                                                  inSystem, _
                                                                  inActionId)
                    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                    '登録件数の確認
                    If insertCount > 0 Then
                        returnValue = ReturnCode.Success '登録成功
                    Else
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} OUT:RETURN = {2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ReturnCode.ErrNoCases))

                        returnValue = ReturnCode.ErrNoCases '失敗
                    End If
                Case Else '登録区分が不明(エラー)
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} OUT:RETURN = {2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ReturnCode.ErrType))

                    returnValue = ReturnCode.ErrType '登録区分が不明(エラー)
            End Select

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , returnValue))

            Return returnValue
            'DBタイムアウト
        Catch ex As OracleExceptionEx When ex.Number = 1013
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.ErrDBTimeout))
            Return ReturnCode.ErrDBTimeout
        Finally
            If dataSet IsNot Nothing Then dataSet.Dispose()
        End Try
    End Function

    '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START
    '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 START
    ''' <summary>
    ''' 表示区分判定
    ''' </summary>
    ''' <param name="inROExistence">R/O有無</param>
    ''' <param name="inMaxROStatus">R/Oステータス（最大）</param>
    ''' <param name="inReserveExistence">予約有無</param>
    ''' <param name="inCarWashEndType">洗車終了有無(0：洗車未終了、1：洗車終了)</param>
    ''' <param name="inServiceStatus">サービスステータス</param>
    ''' <param name="inMinROStatus">R/Oステータス（最小）</param>
    ''' <returns>表示区分</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' 2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
    ''' </History>
    Public Function GetChipArea(ByVal inROExistence As String, _
                                ByVal inMaxROStatus As String, _
                                ByVal inReserveExistence As String, _
                                ByVal inCarWashEndType As String, _
                                ByVal inServiceStatus As String, _
                                ByVal inMinROStatus As String) As Integer
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        'Public Function GetChipArea(ByVal inROExistence As String, _
        '                    ByVal inROStatus As String, _
        '                    ByVal inReserveExistence As String, _
        '                    ByVal inWorkEndType As String, _
        '                    ByVal inCarWashEndType As String, _
        '                    ByVal inServiceStatus As String) As Integer
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        'Public Function GetChipArea(ByVal inSMStatus As String, _
        '                            ByVal inROExistence As String, _
        '                            ByVal inROStatus As String, _
        '                            ByVal inReserveExistence As String) As Integer

        'Public Function GetChipArea(ByVal inSMStatus As String, _
        '                            ByVal inROExistence As String, _
        '                            ByVal inROStatus As String) As Integer
        '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 END
        '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 END
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Public Function GetChipArea(ByVal inROExistence As String, _
        '                            ByVal inROStatus As String, _
        '                            ByVal inReserveExistence As String, _
        '                            ByVal inDTAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable) As Integer
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応START

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
        '            , Me.GetType.ToString _
        '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '            , inSMStatus, inROExistence, inROStatus))
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} inROExistence:{2} inROStatus:{3} inReserveExistence:{4} inWorkEndType:{5} inServiceStatus:{6}" _
        '            , Me.GetType.ToString _
        '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '            , inROExistence, inROStatus, inReserveExistence, inWorkEndType, inServiceStatus))
        '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} inROExistence:{2} inMaxROStatus:{3} inReserveExistence:{4} inServiceStatus:{5} inMinROStatus:{6}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inROExistence, inMaxROStatus, inReserveExistence, inServiceStatus, inMinROStatus))
        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END



        Try
            Dim returnType As Integer '表示区分
            'R/O有無
            If ROInvalid.Equals(inROExistence) Then 'R/O無し
                returnType = DisplayType.Invalid '★受付中★
            Else 'R/O有り
                'R/Oｽﾃｰﾀｽ判定
                Select Case inMaxROStatus
                    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                    ''受付または見積確定待ち
                    'Case ROReceptionist,
                    '     ROEstimate

                    '    returnType = DisplayType.Invalid '★受付中★   

                    '    '整備中または部品待ちまたは検査完了
                    'Case ROMaintenance,
                    '     ROParts,
                    '     ROFinInspection

                    '    '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 START
                    '    If ReserveEffective.Equals(inReserveExistence) Then
                    '        '予約ありの場合
                    '        '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 END

                    '        '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START

                    '        '    '洗車待ちまたは洗車中または預かり中または納車待ち
                    '        '    If SMWaitWash.Equals(inSMStatus) _
                    '        'OrElse SMWash.Equals(inSMStatus) _
                    '        'OrElse SMCustody.Equals(inSMStatus) _
                    '        'OrElse SMDelivery.Equals(inSMStatus) Then
                    '        '        '検査完了の場合のみ
                    '        '        If ROFinInspection.Equals(inROStatus) Then
                    '        '            returnType = DisplayType.DeliveryPreparation  '★納車準備★ 
                    '        '        Else
                    '        '            returnType = DisplayType.Err '★表示区分不正★
                    '        '        End If
                    '        '        '2012/09/11 TMEJ 日比野 SMメインメニューの古いチップが消えない不具合対応 START
                    '        '    ElseIf SMFinish.Equals(inSMStatus) Then
                    '        '        returnType = DisplayType.Err '★表示区分不正★

                    '        '        '2012/09/11 TMEJ 日比野 SMメインメニューの古いチップが消えない不具合対応 START
                    '        '    Else
                    '        '        returnType = DisplayType.Work '★作業中★   
                    '        '    End If

                    '        '検査完了の場合
                    '        If ROFinInspection.Equals(inROStatus) Then
                    '            '追加作業が存在している場合
                    '            If Not inDTAddRepairStatus Is Nothing _
                    '                OrElse 0 < inDTAddRepairStatus.Count Then

                    '                '追加作業ステータスが9(検査完了)以外の検索
                    '                Dim rowAddList As IC3800804AddRepairStatusDataTableRow() = _
                    '                    (From col In inDTAddRepairStatus _
                    '                     Where col.STATUS <> CompleteExaminationFinish _
                    '                     Select col).ToArray
                    '                '追加作業ステータスが9(完成検査完了)以外がある場合
                    '                If 0 < rowAddList.Count Then
                    '                    returnType = DisplayType.Work                   '★作業中★  
                    '                Else '追加作業ステータスがすべて9(完成検査完了)の場合
                    '                    returnType = DisplayType.DeliveryPreparation    '★納車準備★ 
                    '                End If

                    '            Else '追加作業が存在しない
                    '                returnType = DisplayType.DeliveryPreparation        '★納車準備★ 
                    '            End If
                    '        Else '部品待ちOR整備中
                    '            returnType = DisplayType.Work                           '★作業中★  
                    '        End If

                    '        '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 END

                    '        '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 START
                    '    Else
                    '        '予約なしの場合
                    '        returnType = DisplayType.Err '★表示区分不正★
                    '    End If
                    '    '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 END

                    '    '売上済または整備完了
                    'Case ROFinSales,
                    '     ROFinMaintenance

                    '    returnType = DisplayType.DeliveryWork '★納車作業★ 

                    '    '上記以外
                    'Case Else
                    '    returnType = DisplayType.Err '★表示区分不正★
                    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
                    Case RONoneReissuing,
                         ROReissuingSA,
                         ROReissuingTC,
                         ROWaitRecognitionFM,
                         ROPartsDemoEstimate,
                         ROPartsMasterEstimate,
                         ROWaitCustomerRecognition
                        '「00:R/O未起票」「10:SA起票中」「15:TC起票中」「20:FM承認待ち」
                        '「25:部品仮見積中」「30:部品本見積中」「40:顧客承認待ち」

                        returnType = DisplayType.Invalid                                    '★受付中★   


                    Case ROWaitStruct,
                         ROWaitWorkStart,
                         ROWorking,
                         ROInspectRequest,
                         ROInspectFinish
                        '「50:着工指示待ち」「55:作業開始待ち」「60:作業中」
                        '「65:完成検査依頼中」「70:完成検査完了」

                        If ReserveEffective.Equals(inReserveExistence) Then
                            '予約ありの場合
                            returnType = DisplayType.Work                                   '★作業中★

                        Else
                            '予約なしの場合
                            returnType = DisplayType.Err                                    '★表示区分不正★

                        End If

                    Case ROWaitDeliveryPreparation
                        '「80:納車準備待ち」
                        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        'If WorkEndTypeWorking.Equals(inWorkEndType) Then
                        '    '作業が完了していないものがある場合
                        '    returnType = DisplayType.Work                                   '★作業中★

                        'ElseIf WorkEndTypeWorkEnd.Equals(inWorkEndType) Then
                        '    '作業が完了していないものがない場合
                        '    returnType = DisplayType.DeliveryPreparation                    '★納車準備★ 

                        'Else
                        '    '上記以外
                        '    returnType = DisplayType.Err                                    '★表示区分不正★

                        'End If

                        If ROWaitDeliveryPreparation.Equals(inMinROStatus) Then
                            'ROステータス（最小）が80:納車準備待ちの場合
                            returnType = DisplayType.DeliveryPreparation                    '★納車準備★

                        Else
                            '上記以外
                            returnType = DisplayType.Work                                   '★作業中★

                        End If
                        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    Case ROWorkingDelivery
                        '「85:納車作業中(清算中)」

                        If ServiceStatusDropOff.Equals(inServiceStatus) OrElse _
                           ServiceStatusWaitDalivery.Equals(inServiceStatus) Then
                            '「11：預かり中」「12：納車待ち」の場合
                            returnType = DisplayType.DeliveryWork                           '★納車作業★

                        Else
                            '2017/10/03 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                            'If CarWashEndTypeWashing.Equals(inCarWashEndType) Then
                            '    '洗車が終了していない場合
                            '    returnType = DisplayType.DeliveryPreparation                '★納車準備★

                            'ElseIf CarWashEndTypeWashEnd.Equals(inCarWashEndType) Then
                            '    '洗車が終了している場合
                            '    returnType = DisplayType.DeliveryWork                       '★納車作業★

                            'Else
                            '    '上記以外
                            '    returnType = DisplayType.Err                                '★表示区分不正★

                            'End If
                            If CarWashEndTypeWashEnd.Equals(inCarWashEndType) Then
                                '洗車が終了している場合
                                returnType = DisplayType.DeliveryWork                       '★納車作業★

                            Else
                                '上記以外
                                returnType = DisplayType.DeliveryPreparation                '★納車準備★


                            End If
                        End If
                    Case Else
                        '上記以外
                        returnType = DisplayType.Err                                        '★表示区分不正★

                End Select
            End If

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:DisplayType = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , returnType))

            Return returnType
        Finally
        End Try
    End Function

    ''' <summary>
    ''' 初期処理
    ''' </summary>
    ''' <param name="indealerCode">販売店コード</param>
    ''' <param name="instoreCode">店舗コード</param>
    ''' <param name="inStartDay">取得開始日</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function InitCommon(ByVal inDealerCode As String, _
                               ByVal inStoreCode As String, _
                               ByVal inStartDay As Date) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inStoreCode, inStartDay))

        Dim biz As IC3810701BusinessLogic = Nothing
        Try
            'パラメータチェック
            If String.IsNullOrEmpty(inDealerCode) _
               OrElse String.IsNullOrEmpty(inStoreCode) _
               OrElse inStartDay = Date.MinValue Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.ErrArgument))
                'リターン引数無し
                Return ReturnCode.ErrArgument
            End If

            '■■■■■SQL_SMBCommonClass_002 店舗の営業開始・終了時刻取得 2-12 1-3-5-1-0-0 START■■■■■
            LogServiceCommonBiz.OutputLog(46, "●■● 2.1.3.5.1 SMBCommonClass_002 START")

            '店舗の営業開始・終了時刻取得
            DTStallTime = GetStallTime(inDealerCode, inStoreCode)

            LogServiceCommonBiz.OutputLog(46, "●■● 2.1.3.5.1 SMBCommonClass_002 END")
            '■■■■■SQL_SMBCommonClass_002 店舗の営業開始・終了時刻取得 2-12 1-3-5-1-0-0 END■■■■■

            '店舗の営業開始が取得できない場合
            If DTStallTime.Count = 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT[StallTime]:RETURN = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ReturnCode.ErrNoCases))
                'リターン件数0件
                Return ReturnCode.ErrNoCases
            End If

            'サービス標準LT取得()
            biz = New IC3810701BusinessLogic

            '■■■■■SQL_IC3810701_001 サービス標準LT取得 2-13 1-3-5-2-0-0 START■■■■■
            LogServiceCommonBiz.OutputLog(47, "●■● 2.1.3.5.2 IC3810701_001 START")

            DTStandardLTList = biz.GetStandardLTList(inDealerCode, inStoreCode)

            LogServiceCommonBiz.OutputLog(47, "●■● 2.1.3.5.2 IC3810701_001 END")
            '■■■■■SQL_IC3810701_001 サービス標準LT取得 2-13 1-3-5-2-0-0 START■■■■■

            'End Using

            'サービス標準LT取得が取得できない場合
            If DTStandardLTList.Count = 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT[StandardLTList]:RETURN = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ReturnCode.ErrNoCases))
                'リターン件数0件
                Return ReturnCode.ErrNoCases
            End If

            '■■■■■SQL_SMBCommonClass_003 店舗の非稼働日取得 2-14 1-3-5-3-0-0 START■■■■■
            LogServiceCommonBiz.OutputLog(48, "●■● 2.1.3.5.3 SMBCommonClass_003 START")

            '店舗の非稼働日取得
            DTNonWorkDays = GetNonWorkingDays(inDealerCode, inStoreCode, inStartDay)

            LogServiceCommonBiz.OutputLog(48, "●■● 2.1.3.5.3 SMBCommonClass_003 END")
            '■■■■■SQL_SMBCommonClass_003 店舗の非稼働日取得 2-14 1-3-5-3-0-0 END■■■■■

            '成功
            Return ReturnCode.Success
            'DBタイムアウト
        Catch ex As OracleExceptionEx When ex.Number = 1013
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.ErrDBTimeout))
            Return ReturnCode.ErrDBTimeout
        Finally
            If biz IsNot Nothing Then biz.Dispose()
        End Try
    End Function

    ''' <summary>
    ''' 納車見込み時刻取得
    ''' </summary>
    ''' <param name="inDisplayType">表示区分</param>
    ''' <param name="inWorkEndTime">作業終了予定時刻</param>
    ''' <param name="inInspectEndTime">完成検査完了時刻</param>
    ''' <param name="inWashStartTime">洗車開始時刻</param>
    ''' <param name="inWashEndTime">洗車終了時刻</param>
    ''' <param name="inPrintTime">清算書印刷時刻</param>
    ''' <param name="inRestTime">残作業時間(分)</param>
    ''' <param name="inWashExistence">洗車有無</param>
    ''' <param name="inPresentTime">現在時刻</param>
    ''' <param name="inInspectionType">残完成検査区分</param>
    ''' <returns>納車見込み時刻</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' 2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
    ''' </history>
    Public Function GetDeliveryDate(ByVal inDisplayType As DisplayType, _
                                    ByVal inWorkEndTime As Date, _
                                    ByVal inInspectEndTime As Date, _
                                    ByVal inWashStartTime As Date, _
                                    ByVal inWashEndTime As Date, _
                                    ByVal inPrintTime As Date, _
                                    ByVal inRestTime As Long, _
                                    ByVal inWashExistence As String, _
                                    ByVal inPresentTime As Date, _
                                    ByVal inInspectionType As String) As Date

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDisplayType, inWorkEndTime, inInspectEndTime, inWashStartTime, inWashEndTime, inPrintTime, inRestTime, inWashExistence, inPresentTime _
                    , inInspectionType))
        Try
            '補正時間
            Dim reviseTime() As Date
            'リターンコード
            Dim returnValue As Integer
            '納車見込み時間
            Dim deliveryTime As Date

            '表示区分の判定
            Select Case inDisplayType
                '納車作業
                Case DisplayType.DeliveryWork
                    'パラメーターチェック
                    If inPrintTime = Date.MinValue Then
                        returnValue = ReturnCode.ErrArgument
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} RETURNCODE = {2} ErrPram:inPrintTime = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , returnValue, inPrintTime))
                        Throw New ArgumentException

                    End If

                    '時間補正
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'reviseTime = {GetTimeCorrection(inPrintTime, DTStandardLTList(0).DELIVERYPRE_STANDARD_LT), _
                    '              inPresentTime}
                    reviseTime = {GetTimeCorrection(inPrintTime, DTStandardLTList(0).DELIVERYWR_STANDARD_LT), _
                                  inPresentTime}
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START

                    '配列をソート
                    Array.Sort(reviseTime)
                    Array.Reverse(reviseTime)
                    '一番遅い時間を納車見込み時間とする
                    deliveryTime = reviseTime(0)
                    '成功
                    returnValue = ReturnCode.Success

                    '納車準備
                Case DisplayType.DeliveryPreparation

                    'パラメーターチェック
                    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                    'If inInspectEndTime = Date.MinValue Then
                    '    returnValue = ReturnCode.ErrArgument
                    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '                , "{0}.{1} RETURNCODE = {2} ErrPram:inInspectEndTime = {3}" _
                    '                , Me.GetType.ToString _
                    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '                , returnValue, inInspectEndTime))
                    '    Throw New ArgumentException
                    'End If

                    If inInspectEndTime = Date.MinValue AndAlso _
                       inWorkEndTime = Date.MinValue Then
                        '完成検査承認日時、作業終了日時が存在しない場合
                        'エラー出力
                        returnValue = ReturnCode.ErrArgument

                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} RETURNCODE = {2} ErrPram:inInspectEndTime = {3} and inWorkEndTime = {4}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , returnValue, inInspectEndTime, inWorkEndTime))

                        Throw New ArgumentException

                    ElseIf inInspectEndTime = Date.MinValue AndAlso _
                           inWorkEndTime <> Date.MinValue Then
                        '完成検査承認日時が存在しない場合
                        '完成検査容認日時に作業終了日時を設定する
                        inInspectEndTime = inWorkEndTime

                    End If
                    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                    '洗車無しまたは洗車済み
                    If NoWashFlag.Equals(inWashExistence) _
                       OrElse inWashEndTime <> Date.MinValue Then

                        '時間補正
                        reviseTime = {GetTimeCorrection(inInspectEndTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT)), _
                                      GetTimeCorrection(inPresentTime, DTStandardLTList(0).DELIVERYWR_STANDARD_LT)}

                    Else '洗車有り
                        '洗車未開始
                        If inWashStartTime = Date.MinValue Then
                            '時間補正
                            reviseTime = {GetTimeCorrection(inInspectEndTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT)), _
                                          GetTimeCorrection(inPresentTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).WASHTIME))}

                        Else '洗車中または洗車開始済
                            '時間補正
                            reviseTime = {GetTimeCorrection(inInspectEndTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT)), _
                                          GetTimeCorrection(inWashStartTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).WASHTIME)), _
                                          GetTimeCorrection(inPresentTime, DTStandardLTList(0).DELIVERYWR_STANDARD_LT)}

                        End If
                    End If

                    '配列をソート
                    Array.Sort(reviseTime)
                    Array.Reverse(reviseTime)
                    '一番遅い時間を納車見込み時間とする
                    deliveryTime = reviseTime(0)
                    '成功
                    returnValue = ReturnCode.Success

                    '追加承認または作業中
                Case DisplayType.AddApprove, DisplayType.Work
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'パラメーターチェック
                    'If inWorkEndTime = Date.MinValue Then
                    '    returnValue = ReturnCode.ErrArgument
                    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '                , "{0}.{1} RETURNCODE = {2} ErrPram:inWorkEndTime = {3}" _
                    '                , Me.GetType.ToString _
                    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '                , returnValue, inWorkEndTime))
                    '    Throw New ArgumentException

                    'End If
                    'reviseTime = Nothing
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    '洗車無
                    If NoWashFlag.Equals(inWashExistence) Then
                        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        '時間補正
                        'reviseTime = {GetTimeCorrection(inWorkEndTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT)), _
                        '              GetTimeCorrection(inPresentTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + inRestTime))}

                        '時間補正
                        If NotFinishFinalInspection.Equals(inInspectionType) Then
                            '完成検査入力未完了
                            reviseTime = {GetTimeCorrection(inWorkEndTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + DTStandardLTList(0).STD_INSPECTION_TIME)), _
                                          GetTimeCorrection(inPresentTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + DTStandardLTList(0).STD_INSPECTION_TIME + inRestTime))}

                        ElseIf WaitingFinalInspection.Equals(inInspectionType) Then
                            '完成検査承認待ち
                            reviseTime = {GetTimeCorrection(inWorkEndTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + DTStandardLTList(0).STD_INSPECTION_TIME)), _
                                          GetTimeCorrection(inPresentTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + inRestTime))}

                        Else
                            'その他（完成検査承認完了または不要）
                            reviseTime = {GetTimeCorrection(inWorkEndTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT)), _
                                          GetTimeCorrection(inPresentTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + inRestTime))}
                        End If

                        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    Else '洗車有り
                        '納車準備時間と洗車時間の長いほうを選択
                        Dim addLongTime As Long = System.Math.Max(DTStandardLTList(0).DELIVERYPRE_STANDARD_LT, DTStandardLTList(0).WASHTIME)

                        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        '時間補正
                        'reviseTime = {GetTimeCorrection(inWorkEndTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime)), _
                        '              GetTimeCorrection(inPresentTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + inRestTime))}

                        '時間補正
                        If NotFinishFinalInspection.Equals(inInspectionType) Then
                            '完成検査入力未完了
                            reviseTime = {GetTimeCorrection(inWorkEndTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + DTStandardLTList(0).STD_INSPECTION_TIME)), _
                                          GetTimeCorrection(inPresentTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + DTStandardLTList(0).STD_INSPECTION_TIME + inRestTime))}

                        ElseIf WaitingFinalInspection.Equals(inInspectionType) Then
                            '完成検査承認待ち
                            reviseTime = {GetTimeCorrection(inWorkEndTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + DTStandardLTList(0).STD_INSPECTION_TIME)), _
                                          GetTimeCorrection(inPresentTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + inRestTime))}

                        Else
                            'その他（完成検査承認完了または不要）
                            reviseTime = {GetTimeCorrection(inWorkEndTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime)), _
                                          GetTimeCorrection(inPresentTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + inRestTime))}
                        End If

                        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    End If

                    '配列をソート
                    Array.Sort(reviseTime)
                    Array.Reverse(reviseTime)
                    '一番遅い時間を納車見込み時間とする
                    deliveryTime = reviseTime(0)
                    '成功
                    returnValue = ReturnCode.Success

            End Select
            '計算に使用したパラメーター
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} DELIVERYWR_STANDARD_LT:{2} DELIVERYPRE_STANDARD_LT:{3} WASHTIME:{4} INSPECTION_STANDARD_LT:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , DTStandardLTList(0).DELIVERYWR_STANDARD_LT, DTStandardLTList(0).DELIVERYPRE_STANDARD_LT, DTStandardLTList(0).WASHTIME _
                        , DTStandardLTList(0).STD_INSPECTION_TIME))

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} RETURNCODE = {2} DELIVERYTIME = {3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , returnValue, deliveryTime))

            Return deliveryTime
        Finally
        End Try
    End Function

    '2015/02/09 TMEJ 明瀬 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
    ''' <summary>
    ''' 納車見込時刻取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inVisitId">訪問ID</param>
    ''' <param name="inChipArea">表示区分</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <returns>納車見込時刻</returns>
    ''' <remarks></remarks>
    Public Function GetDeliveryDate(ByVal inDealerCode As String, _
                                    ByVal inBranchCode As String, _
                                    ByVal inServiceInId As Decimal, _
                                    ByVal inVisitId As Long, _
                                    ByVal inChipArea As DisplayType, _
                                    ByVal inNowDate As Date) As Date

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'チップ詳細情報テーブル
        Dim dtChipDetailProcess As ChipDetailProcessDataTable = Nothing

        'RO情報テーブル
        Dim dtChipDetailRepairOrderInfo As ChipDetailRepairOrderInfoDataTable = Nothing

        Using taSMBCommonClass As New SMBCommonClassTableAdapter

            'チップ詳細情報取得
            dtChipDetailProcess = _
                taSMBCommonClass.GetChipDetailProcessData(inDealerCode, _
                                                          inBranchCode, _
                                                          inServiceInId)

            'RO情報取得
            dtChipDetailRepairOrderInfo = _
                taSMBCommonClass.GetRepariOrderInfo(inDealerCode, _
                                                    inBranchCode, _
                                                    inVisitId)

        End Using

        'チップ詳細情報がある場合はデータを格納
        Dim washStartDate As DateTime                           '洗車開始時刻
        Dim washEndDate As DateTime                             '洗車終了時刻
        Dim workEndPlanDateLast As DateTime                     '作業終了予定時刻(最後)
        Dim remainingWorkTime As Long                           '残作業時間
        Dim washType As String                                  '洗車有無

        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        Dim remainingInspectionType As String                   '残完成検査区分
        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        If Not IsNothing(dtChipDetailProcess) _
        AndAlso 0 < dtChipDetailProcess.Count Then

            Dim drChipDetailProcess As ChipDetailProcessRow = dtChipDetailProcess(0)

            If Not (drChipDetailProcess.IsRESULT_WASH_STARTNull) Then
                washStartDate = _
                    DateTimeFunc.FormatString(FORMAT_DATE, _
                                              drChipDetailProcess.RESULT_WASH_START)
            End If

            If Not (drChipDetailProcess.IsRESULT_WASH_ENDNull) Then
                washEndDate = _
                    DateTimeFunc.FormatString(FORMAT_DATE, _
                                              drChipDetailProcess.RESULT_WASH_END)
            End If

            workEndPlanDateLast = drChipDetailProcess.ENDTIME
            remainingWorkTime = drChipDetailProcess.WORKTIME
            washType = drChipDetailProcess.WASHFLG

            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            remainingInspectionType = drChipDetailProcess.REMAINING_INSPECTION_TYPE
            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        Else
            washStartDate = Nothing
            washEndDate = Nothing
            workEndPlanDateLast = Nothing
            remainingWorkTime = Nothing
            washType = Nothing

            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            remainingInspectionType = Nothing
            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        End If

        'RO情報がある場合はデータを格納
        Dim statementPrintDate As DateTime                      '清算書印刷時刻
        Dim completeExaminationEndDate As DateTime              '完成検査完了時刻

        If dtChipDetailRepairOrderInfo IsNot Nothing _
        AndAlso 0 < dtChipDetailRepairOrderInfo.Count Then

            statementPrintDate = Me.checkDateRowData(dtChipDetailRepairOrderInfo(0), _
                                                     "INVOICE_PRINT_DATETIME")
            completeExaminationEndDate = Me.checkDateRowData(dtChipDetailRepairOrderInfo(0), _
                                                             "INSPECTION_APPROVAL_DATETIME")
        Else

            statementPrintDate = Nothing
            completeExaminationEndDate = Nothing
        End If

        '納車見込時刻取得
        Try
            '共通関数の初期処理が失敗した場合はDate.MinValueを返す
            If Me.InitCommon(inDealerCode, inBranchCode, inNowDate) <> 0 Then

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} OUT:RETURN = {2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , Date.MinValue))

                Return Date.MinValue

            End If

            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            ''納車見込時刻取得
            'Dim returnDeliveryDate As DateTime = _
            '    Me.GetDeliveryDate(inChipArea, _
            '                       workEndPlanDateLast, _
            '                       completeExaminationEndDate, _
            '                       washStartDate, _
            '                       washEndDate, _
            '                       statementPrintDate, _
            '                       remainingWorkTime, _
            '                       washType, _
            '                       inNowDate)
            '納車見込時刻取得
            Dim returnDeliveryDate As DateTime = _
                Me.GetDeliveryDate(inChipArea, _
                                   workEndPlanDateLast, _
                                   completeExaminationEndDate, _
                                   washStartDate, _
                                   washEndDate, _
                                   statementPrintDate, _
                                   remainingWorkTime, _
                                   washType, _
                                   inNowDate, _
                                   remainingInspectionType)
            '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} OUT:RETURN = {2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , returnDeliveryDate))

            Return returnDeliveryDate

        Catch ex As Exception

            'エラーになった場合はDate.Minvalueを返す
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} OUT:RETURN = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , Date.MinValue), ex)

            Return Date.MinValue

        End Try

    End Function
    '2015/02/09 TMEJ 明瀬 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

    ''' <summary>
    ''' 納車見込み遅れ時刻取得
    ''' </summary>
    ''' <param name="inDisplayType">表示区分</param>
    ''' <param name="inDeliveryTime">納車予定時刻</param>
    ''' <param name="inWorkEndTime">作業終了予定時刻</param>
    ''' <param name="inInspectEndTime">完成検査完了時刻</param>
    ''' <param name="inWashStartTime">洗車開始時刻</param>
    ''' <param name="inWashEndTime">洗車終了時刻</param>
    ''' <param name="inPrintTime">清算書印刷時刻</param>
    ''' <param name="inRestTime">残作業時間(分)</param>
    ''' <param name="inWashExistence">洗車有無</param>
    ''' <param name="inPresentTime">現在時刻</param>
    ''' <param name="inInspectionType">残完成検査区分</param>
    ''' <returns>納車見込み遅れ時刻</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' 2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
    ''' </history>
    Public Function GetDeliveryDelayDate(ByVal inDisplayType As DisplayType, _
                                         ByVal inDeliveryTime As Date, _
                                         ByVal inWorkEndTime As Date, _
                                         ByVal inInspectEndTime As Date, _
                                         ByVal inWashStartTime As Date, _
                                         ByVal inWashEndTime As Date, _
                                         ByVal inPrintTime As Date, _
                                         ByVal inRestTime As Long, _
                                         ByVal inWashExistence As String, _
                                         ByVal inPresentTime As Date, _
                                         ByVal inInspectionType As String) As Date

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11} P11:{12}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , inDisplayType, inDeliveryTime, inWorkEndTime, inInspectEndTime, inWashStartTime, inWashEndTime, inPrintTime, inRestTime, inWashExistence, inPresentTime _
                   , inInspectionType))
        Try
            '補正時間
            Dim reviseTime As Date

            'リターンコード
            Dim returnValue As Integer

            '納車見込み遅れ時間
            Dim deliveryDelayTime As Date

            '表示区分の判定
            Select Case inDisplayType
                '納車作業
                Case DisplayType.DeliveryWork
                    'パラメーターチェック
                    If inPrintTime = Date.MinValue Then
                        returnValue = ReturnCode.ErrArgument
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} RETURNCODE = {2} ErrPram:inPrintTime = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , returnValue, inPrintTime))
                        Throw New ArgumentException

                    End If

                    '時間補正
                    reviseTime = GetTimeCorrection(inDeliveryTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT * -1))

                    '納車見込み遅れ判定
                    If inPrintTime > reviseTime Then
                        '現在時間を納車見込み遅れ時間
                        deliveryDelayTime = inPresentTime

                    Else
                        '納車見込み時間を納車見込み遅れ時間
                        deliveryDelayTime = inDeliveryTime

                    End If

                    '成功
                    returnValue = ReturnCode.Success

                    '納車準備
                Case DisplayType.DeliveryPreparation

                    'パラメーターチェック
                    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                    'If inInspectEndTime = Date.MinValue Then
                    '    returnValue = ReturnCode.ErrArgument
                    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '                , "{0}.{1} RETURNCODE = {2} ErrPram:inInspectEndTime = {3}" _
                    '                , Me.GetType.ToString _
                    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '                , returnValue, inInspectEndTime))
                    '    Throw New ArgumentException

                    'End If

                    If inInspectEndTime = Date.MinValue AndAlso _
                       inWorkEndTime = Date.MinValue Then
                        '完成検査承認日時、作業終了日時が存在しない場合
                        'エラー
                        returnValue = ReturnCode.ErrArgument

                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} RETURNCODE = {2} ErrPram:inInspectEndTime = {3} and inWorkEndTime = {4}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , returnValue, inInspectEndTime, inWorkEndTime))

                        Throw New ArgumentException

                    ElseIf inInspectEndTime = Date.MinValue AndAlso _
                           inWorkEndTime <> Date.MinValue Then
                        '完成検査承認日時が存在しない場合
                        '完成検査容認日時に作業終了日時を設定する
                        inInspectEndTime = inWorkEndTime

                    End If
                    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                    '洗車無しまたは洗車済み
                    If NoWashFlag.Equals(inWashExistence) OrElse _
                       inWashEndTime <> Date.MinValue Then

                        '時間補正
                        reviseTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT) * -1))
                        '納車見込み遅れ判定
                        If inInspectEndTime > reviseTime Then
                            '現在時間を納車見込み遅れ時間
                            deliveryDelayTime = inPresentTime

                        Else
                            '納車見込み時間を納車見込み遅れ時間
                            '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                            'deliveryDelayTime = inDeliveryTime.AddMinutes((DTStandardLTList(0).DELIVERYWR_STANDARD_LT * -1))
                            deliveryDelayTime = GetTimeCorrection(inDeliveryTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT * -1))
                            '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                        End If

                    Else '洗車有り
                        '洗車未開始
                        If inWashStartTime = Date.MinValue Then
                            '時間補正
                            reviseTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT) * -1))

                            '納車見込み遅れ判定
                            If inInspectEndTime > reviseTime Then
                                '現在時間を納車見込み遅れ時間
                                deliveryDelayTime = inPresentTime

                            Else
                                '納車見込み時間を納車見込み遅れ時間
                                '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                                'deliveryDelayTime = inDeliveryTime.AddMinutes(((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).WASHTIME) * -1))
                                deliveryDelayTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).WASHTIME) * -1))
                                '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                            End If

                        Else '洗車開始済
                            '時間補正
                            reviseTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT) * -1))

                            '時間補正その2
                            Dim anotherReviseTime As Date = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).WASHTIME) * -1))

                            '納車見込み遅れ判定
                            If inInspectEndTime > reviseTime OrElse _
                               inWashStartTime > anotherReviseTime Then
                                '現在時間を納車見込み遅れ時間
                                deliveryDelayTime = inPresentTime

                            Else
                                '納車見込み時間を納車見込み遅れ時間
                                '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                                'deliveryDelayTime = inDeliveryTime.AddMinutes((DTStandardLTList(0).DELIVERYWR_STANDARD_LT) * -1)
                                deliveryDelayTime = GetTimeCorrection(inDeliveryTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT) * -1)
                                '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                            End If

                        End If

                    End If

                    '成功
                    returnValue = ReturnCode.Success

                    '追加承認または作業中
                Case DisplayType.AddApprove, DisplayType.Work
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'パラメーターチェック
                    'If inWorkEndTime = Date.MinValue Then
                    '    returnValue = ReturnCode.ErrArgument
                    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '                , "{0}.{1} RETURNCODE = {2} ErrPram:inWorkEndTime = {3}" _
                    '                , Me.GetType.ToString _
                    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '                , returnValue, inWorkEndTime))
                    '    Throw New ArgumentException

                    'End If
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    '洗車無
                    If NoWashFlag.Equals(inWashExistence) Then

                        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        '時間補正
                        'reviseTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT) * -1))

                        ''納車見込み遅れ判定
                        'If inWorkEndTime > reviseTime Then
                        '    '現在時間を納車見込み遅れ時間
                        '    deliveryDelayTime = inPresentTime

                        'Else
                        '    '納車見込み時間を納車見込み遅れ時間
                        '    deliveryDelayTime = inDeliveryTime.AddMinutes(((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + inRestTime) * -1))

                        'End If

                        '時間補正
                        If NotFinishFinalInspection.Equals(inInspectionType) Then
                            '完成検査入力未完了
                            reviseTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + DTStandardLTList(0).STD_INSPECTION_TIME) * -1))

                            '納車見込み遅れ判定
                            If inWorkEndTime > reviseTime Then
                                '現在時間を納車見込み遅れ時間
                                deliveryDelayTime = inPresentTime

                            Else
                                '納車見込み時間を納車見込み遅れ時間
                                deliveryDelayTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + DTStandardLTList(0).STD_INSPECTION_TIME + inRestTime) * -1))

                            End If
                        ElseIf WaitingFinalInspection.Equals(inInspectionType) Then
                            '完成検査承認待ち
                            reviseTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + DTStandardLTList(0).STD_INSPECTION_TIME) * -1))

                            '納車見込み遅れ判定
                            If inWorkEndTime > reviseTime Then
                                '現在時間を納車見込み遅れ時間
                                deliveryDelayTime = inPresentTime

                            Else
                                '納車見込み時間を納車見込み遅れ時間
                                deliveryDelayTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + inRestTime) * -1))

                            End If
                        Else
                            'その他（完成検査承認完了または不要）
                            reviseTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT) * -1))

                            '納車見込み遅れ判定
                            If inWorkEndTime > reviseTime Then
                                '現在時間を納車見込み遅れ時間
                                deliveryDelayTime = inPresentTime

                            Else
                                '納車見込み時間を納車見込み遅れ時間
                                deliveryDelayTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + DTStandardLTList(0).DELIVERYPRE_STANDARD_LT + inRestTime) * -1))

                            End If
                        End If
                        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    Else '洗車有り
                        Dim addLongTime As Long = System.Math.Max(DTStandardLTList(0).DELIVERYPRE_STANDARD_LT, DTStandardLTList(0).WASHTIME)
                        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        ''reviseTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime) * -1))

                        ''納車見込み遅れ判定
                        'If inWorkEndTime > reviseTime Then
                        '    '現在時間を納車見込み遅れ時間
                        '    deliveryDelayTime = inPresentTime

                        'Else
                        '    '納車見込み時間を納車見込み遅れ時間
                        '    deliveryDelayTime = inDeliveryTime.AddMinutes(((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + inRestTime) * -1))

                        'End If

                        '時間補正
                        If NotFinishFinalInspection.Equals(inInspectionType) Then
                            '完成検査入力未完了
                            reviseTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + DTStandardLTList(0).STD_INSPECTION_TIME) * -1))

                            '納車見込み遅れ判定
                            If inWorkEndTime > reviseTime Then
                                '現在時間を納車見込み遅れ時間
                                deliveryDelayTime = inPresentTime

                            Else
                                '納車見込み時間を納車見込み遅れ時間
                                deliveryDelayTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + DTStandardLTList(0).STD_INSPECTION_TIME + inRestTime) * -1))

                            End If
                        ElseIf WaitingFinalInspection.Equals(inInspectionType) Then
                            '完成検査承認待ち
                            reviseTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + DTStandardLTList(0).STD_INSPECTION_TIME) * -1))

                            '納車見込み遅れ判定
                            If inWorkEndTime > reviseTime Then
                                '現在時間を納車見込み遅れ時間
                                deliveryDelayTime = inPresentTime

                            Else
                                '納車見込み時間を納車見込み遅れ時間
                                deliveryDelayTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + inRestTime) * -1))
                            End If
                        Else
                            'その他（完成検査承認完了または不要）
                            reviseTime = GetTimeCorrection(inDeliveryTime, ((DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime) * -1))

                            '納車見込み遅れ判定
                            If inWorkEndTime > reviseTime Then
                                '現在時間を納車見込み遅れ時間
                                deliveryDelayTime = inPresentTime

                            Else
                                '納車見込み時間を納車見込み遅れ時間
                                deliveryDelayTime = GetTimeCorrection(inDeliveryTime, (DTStandardLTList(0).DELIVERYWR_STANDARD_LT + addLongTime + inRestTime) * -1)
                            End If
                        End If
                        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    End If

                    '成功
                    returnValue = ReturnCode.Success
            End Select

            '計算に使用したパラメーター
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} DELIVERYWR_STANDARD_LT:{2} DELIVERYPRE_STANDARD_LT:{3} WASHTIME:{4} INSPECTION_STANDARD_LT:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , DTStandardLTList(0).DELIVERYWR_STANDARD_LT, DTStandardLTList(0).DELIVERYPRE_STANDARD_LT, DTStandardLTList(0).WASHTIME _
                        , DTStandardLTList(0).STD_INSPECTION_TIME))

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} RETURNCODE = {2} DELIVERYDELAYTIME = {3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , returnValue, deliveryDelayTime))
            Return deliveryDelayTime
        Finally
        End Try

    End Function

    ''' <summary>
    ''' 店舗の稼働日取得
    ''' </summary>
    ''' <param name="inStandardDay">取得基準日</param>
    ''' <param name="inIncreaseDay">増加日</param>
    ''' <returns>指定した販売店の稼働日</returns>
    ''' <remarks></remarks>
    Public Function GetWorkingDays(ByVal inStandardDay As Date, _
                                   ByVal inIncreaseDay As Integer) _
                                   As Date
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inStandardDay, inIncreaseDay))

        Try
            '増加日カウンター
            Dim addDayCounter As Integer = 0
            '条件に使うため取得基準日をSTRING型にする変数
            Dim workDate As String
            '稼働日確認ROW
            Dim drWorkDay As DataRow()

            '増加日カウンターが増加日と一致
            Do Until addDayCounter = inIncreaseDay
                '基準日プラス１
                inStandardDay = inStandardDay.AddDays(1)
                '条件に使うため取得基準日をSTRINGに変換
                workDate = inStandardDay.ToString("yyyyMMdd", CultureInfo.CurrentCulture)
                'DATATABLEの中をSELECT
                drWorkDay = DTNonWorkDays.Select(String.Format(CultureInfo.CurrentCulture, "WORKDATE = {0}", workDate), "")
                '稼働日かチェック
                If drWorkDay.Count = 0 Then
                    addDayCounter = addDayCounter + 1
                End If
            Loop
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} STANDARDDAY = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inStandardDay))
            Return inStandardDay
        Finally
        End Try
    End Function

    '2012/07/12 TMEJ 小澤 STEP2対応(入庫日時付替え処理追加) START
    ''' <summary>
    ''' 入庫日時付替え処理
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <param name="inFormerReserveId">付替元予約ID</param>
    ''' <param name="inPlaceReserveId">付替先予約ID</param>
    ''' <param name="inPlaceStorageTime">付替え先入庫日時</param>
    ''' <param name="inAccount">更新者</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <returns>登録結果</returns>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2014/02/08 TMEJ 小澤 BTS対応
    ''' 2014/04/08 TMEJ 小澤 BTS-378対応
    ''' </history>
    ''' <remarks></remarks>
    Public Function ChangeCarInDate(ByVal inDealerCode As String, _
                                    ByVal inStoreCode As String, _
                                    ByVal inFormerReserveId As Decimal, _
                                    ByVal inPlaceReserveId As Decimal, _
                                    ByVal inPlaceStorageTime As DateTime, _
                                    ByVal inAccount As String, _
                                    ByVal inPresentTime As DateTime, _
                                    ByVal inSystem As String) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inDealerCode, inStoreCode, inFormerReserveId, inPlaceReserveId _
                  , inPlaceStorageTime, inAccount, inPresentTime, inSystem))
        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        'Public Function ChangeCarInDate(ByVal inDealerCode As String, _
        '                                ByVal inStoreCode As String, _
        '                                ByVal inFormerReserveId As Long, _
        '                                ByVal inPlaceReserveId As Long, _
        '                                ByVal inPlaceStorageTime As DateTime, _
        '                                ByVal inAccount As String, _
        '                                ByVal inPresentTime As DateTime) As Long
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '              , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} " _
        '              , Me.GetType.ToString _
        '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '              , inDealerCode, inStoreCode, inFormerReserveId, inPlaceReserveId _
        '              , inPlaceStorageTime, inAccount, inPresentTime))
        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        Dim daSMBCommonClass As New SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter

        Try
            '付替元予約IDと付替先予約IDが未設定の場合はエラー
            If (IsNothing(inFormerReserveId) OrElse inFormerReserveId < 0) AndAlso _
               (IsNothing(inPlaceReserveId) OrElse inPlaceReserveId < 0) Then
                'エラーログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURN = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ReturnCode.ErrArgument))
                Return ReturnCode.ErrArgument

            End If

            '2014/04/08 TMEJ 小澤 BTS-378対応 START
            '後で使用する可能性があるためここで宣言しておく
            Dim dtFormer As SMBCommonClassDataSet.ChipDetailReserveDataTable = Nothing
            '2014/04/08 TMEJ 小澤 BTS-378対応 END

            '付替元予約IDが存在する場合は「付替元予約ID」のストール情報を取得する
            If 0 < inFormerReserveId Then
                'チップ詳細情報取得(予約)

                '2014/04/08 TMEJ 小澤 BTS-378対応 START
                'Dim dtFormer As SMBCommonClassDataSet.ChipDetailReserveDataTable = _
                '    daSMBCommonClass.GetChipDetailReserveData(inDealerCode, _
                '                                              inStoreCode, _
                '                                              inFormerReserveId)

                dtFormer = daSMBCommonClass.GetChipDetailReserveData(inDealerCode, _
                                                                     inStoreCode, _
                                                                     inFormerReserveId)
                '2014/04/08 TMEJ 小澤 BTS-378対応 END

                '取得件数が0件の場合はエラーにする
                If dtFormer.Count = 0 Then
                    'エラーログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} OUT:RETURN = {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ReturnCode.ErrNoCases))
                    Return ReturnCode.ErrNoCases

                End If

                '入庫日時を取得する
                Dim dr As SMBCommonClassDataSet.ChipDetailReserveRow = _
                    DirectCast(dtFormer.Rows(0), SMBCommonClassDataSet.ChipDetailReserveRow)

                If Not (dr.IsSTRDATENull) Then
                    inPlaceStorageTime = dr.STRDATE

                Else
                    inPlaceStorageTime = Nothing

                End If

            End If

            '「付替先予約ID」が存在する場合
            If 0 < inPlaceReserveId Then
                '2014/02/08 TMEJ 小澤 BTS対応 START
                '付替先チップ詳細情報取得(予約)
                Dim dtPlace As SMBCommonClassDataSet.ChipDetailReserveDataTable = _
                    daSMBCommonClass.GetChipDetailReserveData(inDealerCode, _
                                                              inStoreCode, _
                                                              inPlaceReserveId)

                '取得件数が0件の場合はエラーにする
                If dtPlace.Count = 0 Then
                    'エラーログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} OUT:RETURN = {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ReturnCode.ErrNoCases))
                    Return ReturnCode.ErrNoCases

                End If

                'サービスステータスのチェック
                If Not (ServiceStatusNoShow.Equals(dtPlace(0).SVC_STATUS)) Then
                    '「01：NoShow」でない場合

                    '付替先予約IDの入庫日時を更新する
                    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'Dim updateCount As Long = daSMBCommonClass.UpdateReserveStrDate(inDealerCode, _
                    '                                                                inStoreCode, _
                    '                                                                inPlaceReserveId, _
                    '                                                                inPlaceStorageTime, _
                    '                                                                inAccount, _
                    '                                                                inPresentTime)
                    Dim updateCount As Long = daSMBCommonClass.UpdateReserveStrDate(inDealerCode, _
                                                                                    inStoreCode, _
                                                                                    inPlaceReserveId, _
                                                                                    inPlaceStorageTime, _
                                                                                    inAccount, _
                                                                                    inPresentTime, _
                                                                                    inSystem)
                    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                    '更新件数が0件の場合はエラーにする
                    If updateCount = 0 Then
                        'エラーログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} OUT:RETURN = {2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ReturnCode.ErrNoCases))
                        Return ReturnCode.ErrNoCases

                    End If

                End If

                '2014/02/08 TMEJ 小澤 BTS対応 END

            End If

            '「付替元予約ID」が存在する場合
            If 0 < inFormerReserveId Then
                '2014/04/08 TMEJ 小澤 BTS-378対応 START
                ''付替元予約IDの入庫日時を更新する
                ''2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                ''Dim updateCount As Long = daSMBCommonClass.UpdateReserveStrDate(inDealerCode, _
                ''                                                                inStoreCode, _
                ''                                                                inFormerReserveId, _
                ''                                                                Nothing, _
                ''                                                                inAccount, _
                ''                                                                inPresentTime)
                'Dim updateCount As Long = daSMBCommonClass.UpdateReserveStrDate(inDealerCode, _
                '                                                                inStoreCode, _
                '                                                                inFormerReserveId, _
                '                                                                Nothing, _
                '                                                                inAccount, _
                '                                                                inPresentTime, _
                '                                                                inSystem)
                ''2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''更新件数が0件の場合はエラーにする
                'If updateCount = 0 Then
                '    'エラーログの出力
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                , "{0}.{1} OUT:RETURN = {2}" _
                '                , Me.GetType.ToString _
                '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                , ReturnCode.ErrNoCases))
                '    Return ReturnCode.ErrNoCases
                'End If

                'サービスステータスのチェック
                If Not (ServiceStatusNoShow.Equals(dtFormer(0).SVC_STATUS)) Then
                    '「01：NoShow」でない場合

                    Dim updateCount As Long = daSMBCommonClass.UpdateReserveStrDate(inDealerCode, _
                                                                                    inStoreCode, _
                                                                                    inFormerReserveId, _
                                                                                    Nothing, _
                                                                                    inAccount, _
                                                                                    inPresentTime, _
                                                                                    inSystem)

                    '更新件数チェック
                    If updateCount = 0 Then
                        '0件の場合
                        'エラーログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} OUT:RETURN = {2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ReturnCode.ErrNoCases))
                        Return ReturnCode.ErrNoCases

                    End If

                End If

                '2014/04/08 TMEJ 小澤 BTS-378対応 START

            End If

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.Success))
            Return ReturnCode.Success

            'DBタイムアウト
        Catch ex As OracleExceptionEx When ex.Number = 1013
            ''エラーログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURN = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ReturnCode.ErrDBTimeout))
            Return ReturnCode.ErrDBTimeout

        Finally
            If daSMBCommonClass IsNot Nothing Then daSMBCommonClass.Dispose()

        End Try

    End Function
    '2012/07/12 TMEJ 小澤 STEP2対応(入庫日時付替え処理追加) END

    '2012/07/12 TMEJ 小澤 STEP2対応(ステータス判定処理追加) START
    ''' <summary>
    ''' ステータス判定処理
    ''' </summary>
    ''' <param name="inVisitType">来店実績有無(0：無、1：有)</param>
    ''' <param name="inAssignStatus">振当ステータス</param>
    ''' <param name="inCustomerType">顧客区分(1：自社客、2：未取引客)</param>
    ''' <param name="inWorkStartType">作業開始有無(0：無、1：有)</param>
    ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ''' <param name="inWashType">洗車有無(0：無、1：有)</param>
    ''' <param name="inOrderDataType">R/O有無(0：無、1：有)</param>
    ''' <param name="inOrderStatus">R/Oステータス</param>
    ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、2：部品準備済み、3：部品不要)</param>
    ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み)</param>
    ''' <param name="inAddWorkStatus">追加作業ステータス</param>
    ''' <param name="inReissueVouchers">起票者(1：TC、2：SA)</param>
    ''' <param name="inInstruct">ストール利用ステータス</param>
    ''' <param name="inResultWashStart">洗車開始実績日時</param>
    ''' <param name="inResultWashEnd">洗車終了実績日時</param>
    ''' <param name="inWorkEndType">作業終了有無(0：作業中、1：作業終了)</param>
    ''' <param name="inServiceStatus">サービスステータス</param>
    ''' <returns>ステータス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Public Function GetChipDetailStatus(ByVal inVisitType As String, _
                                        ByVal inAssignStatus As String, _
                                        ByVal inCustomerType As String, _
                                        ByVal inWorkStartType As String, _
                                        ByVal inStopType As String, _
                                        ByVal inWashType As String, _
                                        ByVal inOrderDataType As String, _
                                        ByVal inOrderStatus As String, _
                                        ByVal inPartsPreparationWaitType As String, _
                                        ByVal inCompleteExaminationType As String, _
                                        ByVal inAddWorkStatus As String, _
                                        ByVal inReissueVouchers As String, _
                                        ByVal inInstruct As String, _
                                        ByVal inResultWashStart As String, _
                                        ByVal inResultWashEnd As String, _
                                        ByVal inWorkEndType As String, _
                                        ByVal inServiceStatus As String) As String()
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
        'Public Function GetChipDetailStatus(ByVal inVisitType As String, _
        '                                    ByVal inCustomerType As String, _
        '                                    ByVal inWorkStartType As String, _
        '                                    ByVal inStopType As String, _
        '                                    ByVal inWashType As String, _
        '                                    ByVal inOrderDataType As String, _
        '                                    ByVal inOrderStatus As String, _
        '                                    ByVal inPartsPreparationWaitType As String, _
        '                                    ByVal inCompleteExaminationType As String, _
        '                                    ByVal inAddWorkStatus As String, _
        '                                    ByVal inReissueVouchers As String, _
        '                                    ByVal inInstruct As String, _
        '                                    ByVal inResultWashStart As String, _
        '                                    ByVal inResultWashEnd As String) As String()
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Public Function GetChipDetailStatus(ByVal inVisitType As String, _
        '                                    ByVal inCustomerType As String, _
        '                                    ByVal inWorkStartType As String, _
        '                                    ByVal inStopType As String, _
        '                                    ByVal inWashType As String, _
        '                                    ByVal inOrderDataType As String, _
        '                                    ByVal inOrderStatus As String, _
        '                                    ByVal inPartsPreparationWaitType As String, _
        '                                    ByVal inCompleteExaminationType As String, _
        '                                    ByVal inAddWorkStatus As String, _
        '                                    ByVal inReissueVouchers As String, _
        '                                    ByVal inInstruct As String, _
        '                                    ByVal inResultWashStart As String, _
        '                                    ByVal inResultWashEnd As String, _
        '                                    ByVal dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable) As String()
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11} P11:{12} P12:{13} P13:{14} P14:{15} P15:{16} P16:{17} P17:{18} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inVisitType, inAssignStatus, inCustomerType, inWorkStartType, inStopType _
                  , inWashType, inOrderDataType, inOrderStatus _
                  , inPartsPreparationWaitType, inCompleteExaminationType, inAddWorkStatus _
                  , inReissueVouchers, inInstruct, inResultWashStart, inResultWashEnd, inWorkEndType, inServiceStatus))

        'ステータスコード(左側)を取得
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
        'Dim statusCodeLeft As String = _
        '    Me.getStatusCodeLeft(inVisitType, inCustomerType, inWorkStartType, _
        '                         inStopType, inWashType, _
        '                         inOrderDataType, inOrderStatus, _
        '                         inPartsPreparationWaitType, _
        '                         inCompleteExaminationType, inAddWorkStatus, inInstruct, inResultWashStart, inResultWashEnd)
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Dim statusCodeLeft As String = _
        '    Me.getStatusCodeLeft(inVisitType, inCustomerType, inWorkStartType, _
        '                         inStopType, inWashType, _
        '                         inOrderDataType, inOrderStatus, _
        '                         inPartsPreparationWaitType, _
        '                         inCompleteExaminationType, inAddWorkStatus, _
        '                         inInstruct, inResultWashStart, inResultWashEnd, dtAddRepairStatus)
        Dim statusCodeLeft As String = _
            Me.getStatusCodeLeft(inVisitType, _
                                 inAssignStatus, _
                                 inCustomerType, _
                                 inWorkStartType, _
                                 inStopType, _
                                 inWashType, _
                                 inOrderDataType, _
                                 inOrderStatus, _
                                 inPartsPreparationWaitType, _
                                 inCompleteExaminationType, _
                                 inAddWorkStatus, _
                                 inInstruct, _
                                 inResultWashStart, _
                                 inResultWashEnd, _
                                 inWorkEndType, _
                                 inServiceStatus)
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END
        'ステータス(左側)文言を取得
        Dim statusLeftWord As String = _
            WebWordUtility.GetWord(WordProgramID, CType(statusCodeLeft, Decimal))

        'ステータスコード(右側)を取得
        Dim statusCodeRight As String = _
            Me.getStatusCodeRight(inOrderDataType,
                                  inOrderStatus,
                                  inAddWorkStatus,
                                  inReissueVouchers)
        'ステータス(右側)文言を取得
        Dim statusRightWord As String = _
            WebWordUtility.GetWord(WordProgramID, CType(statusCodeRight, Decimal))

        'ステータス名称を設定
        Dim returnStatus(2) As String
        If Not (String.IsNullOrEmpty(statusLeftWord)) AndAlso _
           Not (String.IsNullOrEmpty(statusRightWord)) Then
            '2つともある場合は「左側/右側」で文字列結合する
            Dim statusWord As New StringBuilder
            statusWord.Append(statusLeftWord)
            statusWord.Append(WebWordUtility.GetWord(WordProgramID, 1))
            statusWord.Append(statusRightWord)
            returnStatus(0) = statusWord.ToString

        ElseIf Not (String.IsNullOrEmpty(statusLeftWord)) Then
            'ステータス(左側)の文言を入れる
            returnStatus(0) = statusLeftWord

        ElseIf Not (String.IsNullOrEmpty(statusRightWord)) Then
            'ステータス(右側)の文言を入れる
            returnStatus(0) = statusRightWord

        Else
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} DataCheckLog [" + _
                        "inVisitType:{2} ][inCustomerType:{3} ][inWorkStartType:{4} ][" + _
                        "inStopType:{5} ][inWashType:{6} ][inOrderDataType:{7} ][" + _
                        "inOrderStatus:{8} ][inPartsPreparationWaitType:{9} ][inCompleteExaminationType:{10} ][inAddWorkStatus:{11} " + _
                        "inReissueVouchers:{12} ][inInstruct:{13} ][inResultWashStart:{14} ][inResultWashEnd:{15} ]" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inVisitType, inCustomerType, inWorkStartType, inStopType _
                      , inWashType, inOrderDataType, inOrderStatus _
                      , inPartsPreparationWaitType, inCompleteExaminationType, inAddWorkStatus _
                      , inReissueVouchers, inInstruct, inResultWashStart, inResultWashEnd))
        End If

        'ステータスコード設定
        returnStatus(1) = statusCodeLeft
        returnStatus(2) = statusCodeRight

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN1 = {2}：RETURN1 = {3}：RETURN1 = {4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnStatus(0), returnStatus(1), returnStatus(2)))
        Return returnStatus
    End Function
    '2012/07/12 TMEJ 小澤 STEP2対応(ステータス判定処理追加) END

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' サービス入庫テーブルロック処理
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inRowLockVersion">サービス入庫テーブルの行ロックバージョン</param>
    ''' <param name="inCancelType">キャンセルフラグ有無（0：キャンセルチップは対象外、1：キャンセルチップも対象）</param>
    ''' <param name="inAccount">アカウント</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' </history>
    ''' <remarks></remarks>
    Public Function LockServiceInTable(ByVal inServiceInId As Decimal, _
                                       ByVal inRowLockVersion As Long, _
                                       ByVal inCancelType As String, _
                                       ByVal inAccount As String, _
                                       ByVal inNowDate As Date, _
                                       ByVal inSystem As String) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inServiceInId, inRowLockVersion, inCancelType, inAccount, inNowDate, inSystem))

        Using da As New SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter
            Try

                'ログインユーザー情報取得
                Dim staffInfo As StaffContext = StaffContext.Current

                'ロック処理実行
                Dim dt As SMBCommonClassDataSet.LockInfoDataTable = _
                    da.LockDBServiceInTable(staffInfo.DlrCD, _
                                            staffInfo.BrnCD, _
                                            inServiceInId, _
                                            inCancelType)

                '取得できなかった場合はエラー
                If dt.Count = 0 Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR:{2}:NO DATA" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ReturnCode.ErrorNoDataFound))
                    Return ReturnCode.ErrorNoDataFound
                End If

                'ROW取得
                Dim dr As SMBCommonClassDataSet.LockInfoRow = _
                    DirectCast(dt.Rows(0), SMBCommonClassDataSet.LockInfoRow)

                '行ロックバージョンが異なる場合はエラー
                If inRowLockVersion <> dr.ROW_LOCK_VERSION Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR:{2}:ROW_VERSION ERROR" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ReturnCode.ErrorDBConcurrency))
                    Return ReturnCode.ErrorDBConcurrency
                End If

                '予約が全てキャンセルされている場合はエラー
                If ServiceStatusCancel.Equals(dr.SVC_STATUS) Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR:{2}:RESERVE CANCEL" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ReturnCode.ErrorNoDataFound))
                    Return ReturnCode.ErrorNoDataFound
                End If

                'サービス入庫テーブルの行ロックバージョンの更新
                Dim updateCount As Long = da.UpdateDBServiceInLockVersion(inServiceInId, _
                                                                          inRowLockVersion, _
                                                                          inAccount, _
                                                                          inNowDate, _
                                                                          inSystem)

                '更新できなかった場合はエラー
                If updateCount = 0 Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR:{2}:ROW_VERSION ERROR" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ReturnCode.ErrNoCases))
                    Return ReturnCode.ErrNoCases
                End If
            Catch ex As OracleExceptionEx When ex.Number = 30006
                'テーブルロックのタイムアウト
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:{2}:RECORD LOCK TIMEOUT" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ReturnCode.ErrorDBConcurrency))
                Return ReturnCode.ErrorDBConcurrency

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'DBタイムアウト
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:{2}:DATABASE TIMEOUT" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ReturnCode.ErrDBTimeout))
                Return ReturnCode.ErrDBTimeout
            End Try
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END:TABLE LOCK SUCCESS" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return ReturnCode.Success
    End Function

    ''' <summary>
    ''' 顧客テーブルロック処理
    ''' </summary>
    ''' <param name="inCustomerId">サービス入庫ID</param>
    ''' <param name="inRowLockVersion">顧客テーブルの行ロックバージョン</param>
    ''' <param name="inAccount">アカウント</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' </history>
    ''' <remarks></remarks>
    Public Function LockCustomerTable(ByVal inCustomerId As Decimal, _
                                      ByVal inRowLockVersion As Long, _
                                      ByVal inAccount As String, _
                                      ByVal inNowDate As Date, _
                                      ByVal inSystem As String) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inCustomerId, inRowLockVersion, inAccount, inNowDate, inSystem))

        Using da As New SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter
            Try
                'ロック処理実行
                Dim dt As SMBCommonClassDataSet.LockInfoDataTable = _
                    da.LockDBCustomerTable(inCustomerId)

                '取得できなかった場合はエラー
                If dt.Count = 0 Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR:{2}:NO DATA" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ReturnCode.ErrorNoDataFound))
                    Return ReturnCode.ErrorNoDataFound
                End If

                'ROW取得
                Dim dr As SMBCommonClassDataSet.LockInfoRow = _
                    DirectCast(dt.Rows(0), SMBCommonClassDataSet.LockInfoRow)

                '行ロックバージョンが異なる場合はエラー
                If inRowLockVersion <> dr.ROW_LOCK_VERSION Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR:{2}:ROW_VERSION ERROR" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ReturnCode.ErrorDBConcurrency))
                    Return ReturnCode.ErrorDBConcurrency
                End If

                '行ロックバージョンの更新
                Dim updateCount As Long = da.UpdateDBCustomerLockVersion(inCustomerId, _
                                                                         inRowLockVersion, _
                                                                         inAccount, _
                                                                         inNowDate, _
                                                                         inSystem)
                '更新できなかった場合はエラー
                If updateCount = 0 Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR:{2}:ROW_VERSION ERROR" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ReturnCode.ErrNoCases))
                    Return ReturnCode.ErrNoCases
                End If
            Catch ex As OracleExceptionEx When ex.Number = 30006
                'テーブルロックのタイムアウト
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:{2}:RECORD LOCK TIMEOUT" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ReturnCode.ErrorDBConcurrency))
                Return ReturnCode.ErrorDBConcurrency

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'DBタイムアウト
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:{2}:DATABASE TIMEOUT" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ReturnCode.ErrDBTimeout))
                Return ReturnCode.ErrDBTimeout
            End Try
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END:TABLE LOCK SUCCESS" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return ReturnCode.Success
    End Function

    ''' <summary>
    ''' ストールロックテーブル登録処理
    ''' </summary>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inLockDate">対象ロック日付</param>
    ''' <param name="inAccount">アカウント</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <remarks></remarks>
    Public Function RegisterStallLock(ByVal inStallId As Decimal, _
                                      ByVal inLockDate As Date, _
                                      ByVal inAccount As String, _
                                      ByVal inNowDate As Date, _
                                      ByVal inSystem As String) As Integer
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inStallId, inLockDate, inAccount, inNowDate, inSystem))

        Using da As New SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter
            Try
                'ロックテーブルの件数確認
                Dim dt As SMBCommonClassDataSet.StallLockInfoDataTable = _
                    da.GetLockTableCount(inStallId, inLockDate)

                If dt.Count = 0 Then
                    'データがない場合はストールロックテーブルに登録する
                    da.RegisterDBStallLock(inStallId, _
                                           inLockDate, _
                                           inAccount, _
                                           inNowDate, _
                                           inSystem)

                Else
                    'データがある場合
                    'ロック経過時間を確認する
                    If dt(0).ROW_UPDATE_DATETIME.AddSeconds(StallLockCheckSecond) < inNowDate Then
                        'ロックしてから確認時間を経過している場合
                        '削除して新しいストールロックテーブルを登録する
                        da.DeleteDBStallLock(inStallId, inLockDate)
                        da.RegisterDBStallLock(inStallId, _
                                               inLockDate, _
                                               inAccount, _
                                               inNowDate, _
                                               inSystem)

                    Else
                        'ロックしてから確認時間を経過していない場合はエラーにする
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} ERROR:{2}:STALL ALREADY REGSITER" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ReturnCode.ErrorDBConcurrency))
                        Return ReturnCode.ErrorDBConcurrency

                    End If

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:{2}:DATABASE TIMEOUT" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ReturnCode.ErrDBTimeout))
                Return ReturnCode.ErrDBTimeout

            End Try

        End Using
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END:{2}]STALL LOCK SUCCESS" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ReturnCode.Success))
        Return ReturnCode.Success
    End Function

    ''' <summary>
    ''' ストールロックテーブル削除処理
    ''' </summary>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inLockDate">対象ロック日付</param>
    ''' <param name="inAccount">アカウント</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <remarks></remarks>
    Public Function DeleteStallLock(ByVal inStallId As Decimal, _
                                    ByVal inLockDate As Date, _
                                    ByVal inAccount As String, _
                                    ByVal inNowDate As Date, _
                                    ByVal inSystem As String) As Integer
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inStallId, inLockDate, inAccount, inNowDate, inSystem))

        Using da As New SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter
            Try

                LogServiceCommonBiz.OutputLog(31, "●■● 1.7.1 SMBCommonClass_014 START")

                'ストールロックテーブルから削除する
                Dim updateCount As Integer = da.DeleteDBStallLock(inStallId, inLockDate)

                LogServiceCommonBiz.OutputLog(31, "●■● 1.7.1 SMBCommonClass_014 END")

                If updateCount = 0 Then
                    'データが削除されている場合はエラーにする
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR:{2}:STALL ALREADY REGSITER" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ReturnCode.ErrorDBConcurrency))
                    Return ReturnCode.ErrorDBConcurrency
                End If
            Catch ex As OracleExceptionEx When ex.Number = 1013
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:RETURN = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ReturnCode.ErrDBTimeout))
                Return ReturnCode.ErrDBTimeout
            End Try

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END TABLE LOCK SUCCESS" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return ReturnCode.Success
    End Function

    ''' <summary>
    ''' 基幹顧客コード変換処理
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBaseCustomerCode">基幹顧客コード</param>
    ''' <returns>置換文字列</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' </history>
    Public Function ReplaceBaseCustomerCode(ByVal inDealerCode As String, _
                                            ByVal inBaseCustomerCode As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} P1:{2} P2:{3} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inBaseCustomerCode))

        '戻り値宣言
        Dim returnCustomerCode As New StringBuilder

        '変換パターンをwebconfigから取得
        Dim replaceType As String = System.Configuration.ConfigurationManager.AppSettings(BaseCustomerCodeKey)

        '基幹顧客コード変換
        If ReplaceBaseCodeAdd.Equals(replaceType) Then
            '販売店コード追加
            returnCustomerCode.Append(inDealerCode).Append("@").Append(inBaseCustomerCode)
        Else
            '追加しないでよい場合はそのまま返す
            returnCustomerCode.Append(inBaseCustomerCode)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnCustomerCode.ToString))
        Return returnCustomerCode.ToString
    End Function

    ''' <summary>
    ''' SMBチップのステータス判定処理
    ''' </summary>
    ''' <param name="inChipAreaType">チップエリア(1：ストール、2：受付、3：追加作業、4：完成検査、5：洗車、6：納車待ち、7：中断、8：NoShow)</param>
    ''' <param name="inVisitSequence">来店実績連番</param>
    ''' <param name="inOrderType">R/O情報有無(0：無、1：有)</param>
    ''' <param name="inOrderNo">R/O番号</param>
    ''' <param name="inWorkStartDate">実績開始日時</param>
    ''' <param name="inStallUseStatus">ストール利用ステータス</param>
    ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、8：部品準備済み、NULL：部品不要)</param>
    ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ''' <param name="inWorkEndDate">実績終了日時</param>
    ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み、2：完成検査承認済み)</param>
    ''' <param name="inServiceinStatus">サービス入庫ステータス</param>
    ''' <param name="inInvoicePrintDate">清算書印刷日時</param>
    ''' <returns>ステータス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Public Function GetSmbChipDetailStatus(ByVal inChipAreaType As Integer, _
                                           ByVal inVisitSequence As Long, _
                                           ByVal inOrderType As String, _
                                           ByVal inOrderNo As String, _
                                           ByVal inWorkStartDate As Date, _
                                           ByVal inStallUseStatus As String, _
                                           ByVal inPartsPreparationWaitType As String, _
                                           ByVal inStopType As String, _
                                           ByVal inWorkEndDate As Date, _
                                           ByVal inCompleteExaminationType As String, _
                                           ByVal inServiceinStatus As String, _
                                           ByVal inInvoicePrintDate As Date) As String
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Public Function GetSmbChipDetailStatus(ByVal inVisitType As String, _
        '                                       ByVal inWorkStartDate As Date, _
        '                                       ByVal inWorkEndDate As Date, _
        '                                       ByVal inStopType As String, _
        '                                       ByVal inWashType As String, _
        '                                       ByVal inOrderNo As String, _
        '                                       ByVal inOrderStatus As String, _
        '                                       ByVal inPartsPreparationWaitType As String, _
        '                                       ByVal inCompleteExaminationType As String, _
        '                                       ByVal inStallUseStatus As String, _
        '                                       ByVal inResultWashStart As String, _
        '                                       ByVal inResultWashEnd As String, _
        '                                       ByVal dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable, _
        '                                       ByVal drReserveROStatusList As IC3801012DataSet.REZROStatusListRow, _
        '                                       ByVal inSequenceNo As Long, _
        '                                       ByVal inDeliveryDate As Date) As String
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11} P11:{12} P12:{13} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inChipAreaType, inVisitSequence, inOrderType, inOrderNo, inWorkStartDate, inStallUseStatus, inPartsPreparationWaitType _
                  , inStopType, inWorkEndDate, inCompleteExaminationType, inServiceinStatus, inInvoicePrintDate))

        'ステータスコード(左側)を取得
        Dim statusCode As String = Me.GetSMBStatusCodeLeft(inChipAreaType, _
                                                           inVisitSequence, _
                                                           inOrderType, _
                                                           inOrderNo, _
                                                           inWorkStartDate, _
                                                           inStallUseStatus, _
                                                           inPartsPreparationWaitType, _
                                                           inStopType, _
                                                           inWorkEndDate, _
                                                           inCompleteExaminationType, _
                                                           inServiceinStatus, _
                                                           inInvoicePrintDate)

        'ステータス(左側)文言を取得
        Dim statusWord As String = _
            WebWordUtility.GetWord(WordProgramID, CType(statusCode, Decimal))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusWord))
        Return statusWord
    End Function

    ''' <summary>
    ''' サービス入庫IDから作業内容ID(最小値)を取得する処理
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <returns>作業内容ID</returns>
    ''' <remarks></remarks>
    Public Function GetServiceInIdToJobDetailId(ByVal inServiceInId As Decimal) As Decimal
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inServiceInId.ToString(CultureInfo.CurrentCulture)))

        '作業内容ID(最小値)を取得する
        Dim returnJobDetailId As Decimal = Me.GetServiceInIdOrJobDetailId(inServiceInId, Nothing)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnJobDetailId))
        Return returnJobDetailId
    End Function

    ''' <summary>
    ''' 作業内容IDからサービス入庫IDを取得する処理
    ''' </summary>
    ''' <param name="inJobDetailId">作業内容ID</param>
    ''' <returns>作業内容ID</returns>
    ''' <remarks></remarks>
    Public Function GetJobDetailIdToServiceInId(ByVal inJobDetailId As Decimal) As Decimal
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inJobDetailId.ToString(CultureInfo.CurrentCulture)))

        'サービス入庫IDを取得する
        Dim returnServiceInId As Decimal = Me.GetServiceInIdOrJobDetailId(Nothing, inJobDetailId)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnServiceInId))
        Return returnServiceInId
    End Function

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 店舗の営業開始・終了時刻取得
    ''' </summary>
    ''' <param name="indealerCode">販売店コード</param>
    ''' <param name="instoreCode">店舗コード</param>
    ''' <returns>店舗の営業開始・終了時刻</returns>
    ''' <remarks></remarks>
    Private Function GetStallTime(ByVal inDealerCode As String, _
                                  ByVal inStoreCode As String) _
                                  As SMBCommonClassDataSet.StallTimeDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} P1:{2} P2:{3} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inStoreCode))
        Try
            Dim dt As SMBCommonClassDataSet.StallTimeDataTable
            '店舗の営業開始・終了時刻取得
            Using dataSet As New SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter
                dt = dataSet.GetStallTime(inDealerCode, inStoreCode)


                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))
                Return dt
            End Using
        Finally
        End Try
    End Function

    ''' <summary>
    ''' 店舗の非稼働日取得
    ''' </summary>
    ''' <param name="indealerCode">販売店コード</param>
    ''' <param name="instoreCode">店舗コード</param>
    ''' <param name="inStandardDay">取得開始日</param>
    ''' <returns>店舗の非稼働日</returns>
    ''' <remarks></remarks>
    Private Function GetNonWorkingDays(ByVal inDealerCode As String, _
                                       ByVal inStoreCode As String, _
                                       ByVal inStandardDay As Date) _
                                       As SMBCommonClassDataSet.NonWorkDaysDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} P1:{2} P2:{3} P3:{4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inStoreCode, inStandardDay))
        Try
            Dim dt As SMBCommonClassDataSet.NonWorkDaysDataTable
            'STRINGに変換
            Dim startDay As String = inStandardDay.ToString("yyyyMMdd", CultureInfo.CurrentCulture)
            '店舗の非稼働日取得
            Using dataSet As New SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter
                dt = dataSet.GetNonWorkingDays(inDealerCode, inStoreCode, startDay)

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))
                Return dt
            End Using
        Finally
        End Try
    End Function

    ''' <summary>
    ''' 開始・終了時刻補正
    ''' </summary>
    ''' <param name="inStandardTime">基準時間</param>
    ''' <param name="inVaryTime">増減時間(分)</param>
    ''' <returns>補正時刻</returns>
    ''' <remarks></remarks>
    Private Function GetTimeCorrection(ByVal inStandardTime As Date, _
                                       ByVal inVaryTime As Long) As Date
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} P1:{2} P2:{3}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inStandardTime, inVaryTime))
        Try
            '処理日
            Dim dealDay As Date = inStandardTime.Date
            '補正時間
            Dim reviseTime As Date
            '営業開始時間(分)
            Dim startTime As Integer = (CType(DTStallTime(0).PSTARTTIME, Date).Hour * 60) + CType(DTStallTime(0).PSTARTTIME, Date).Minute
            '営業終了時間(分)
            Dim endTime As Integer = (CType(DTStallTime(0).PENDTIME, Date).Hour * 60) + CType(DTStallTime(0).PENDTIME, Date).Minute

            '処理日の営業開始時刻
            Dim salesStsartTime As Date = dealDay.AddMinutes(startTime)
            '処理日の営業終了予定時間
            Dim salesEndTime As Date = dealDay.AddMinutes(endTime)

            '日跨ぎ営業の補正
            If startTime > endTime Then
                '24時間加算
                salesEndTime = salesEndTime.AddHours(24)
                '日跨ぎのため24時間分加える
                endTime = endTime + (24 * 60)
            End If

            '営業外時間
            Dim nonSalesTime As Long = (24 * 60) - (endTime - startTime)
            '処理日(STRING型)
            Dim workDate As String
            '非稼働日チェックROW
            Dim drNonWorkDay As DataRow()

            '増加時間の増減判定
            If inVaryTime >= 0 Then '増減時間(プラス)
                '補正時間
                reviseTime = inStandardTime.AddMinutes(inVaryTime)
                'ループ変数
                Dim i As Integer = 0

                '営業内時間になるまで繰り返す
                Do Until i = 1
                    '作業終了時間が営業終了時間を超えた場合
                    If reviseTime > salesEndTime Then
                        '翌営業時間に変換
                        reviseTime = reviseTime.AddMinutes(nonSalesTime)
                        '処理日付に+1
                        dealDay = dealDay.AddDays(1)
                        '翌営業日の営業終了時間とする
                        salesEndTime = salesEndTime.AddHours(24)
                    Else
                        i = 1
                    End If
                Loop

                Do '作業完了日が非稼動日かチェック
                    '条件に使うためSTRINGに変換
                    workDate = dealDay.ToString("yyyyMMdd", CultureInfo.CurrentCulture)
                    'DATATABLEの中をSELECT
                    drNonWorkDay = DTNonWorkDays.Select(String.Format(CultureInfo.CurrentCulture, "WORKDATE = {0}", workDate), "")
                    '非稼働日かチェック
                    If drNonWorkDay.Count > 0 Then
                        '翌営業時間に変換(+24時間)
                        reviseTime = reviseTime.AddHours(24)
                        '処理日付に+1
                        dealDay = dealDay.AddDays(1)
                    End If
                Loop Until drNonWorkDay.Count = 0

            Else '増減時間(マイナス)
                reviseTime = inStandardTime.AddMinutes(inVaryTime)
                'ループ変数
                Dim j As Integer = 0

                '営業内時間になるまで繰り返す
                Do Until j = 1
                    '作業終了時間が営業終了時間を超えた場合
                    If salesStsartTime > reviseTime Then
                        '前営業時間に変換
                        reviseTime = reviseTime.AddMinutes((nonSalesTime * -1))
                        '処理日付に-1
                        dealDay = dealDay.AddDays((1 * -1))
                        '翌営業日の営業終了時間とする
                        salesStsartTime = salesStsartTime.AddHours((24 * -1))
                    Else
                        j = 1
                    End If
                Loop

                Do '作業完了日が非稼動日かチェック
                    '条件に使うためSTRINGに変換
                    workDate = dealDay.ToString("yyyyMMdd", CultureInfo.CurrentCulture)
                    'DATATABLEの中をSELECT
                    drNonWorkDay = DTNonWorkDays.Select(String.Format(CultureInfo.CurrentCulture, "WORKDATE = {0}", workDate), "")
                    '非稼働日かチェック
                    If drNonWorkDay.Count > 0 Then
                        '翌営業時間に変換(-24時間)
                        reviseTime = reviseTime.AddHours((24 * -1))
                        '処理日付に-1
                        dealDay = dealDay.AddDays((1 * -1))
                    End If
                Loop Until drNonWorkDay.Count = 0

            End If
            '計算に使用したパラメーター
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} PSTARTTIME:{2} PENDTIME:{3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , DTStallTime(0).PSTARTTIME, DTStallTime(0).PENDTIME))

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:REVISETIME = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , reviseTime))

            Return reviseTime
        Finally
        End Try
    End Function

    '2012/07/12 TMEJ 小澤 STEP2対応(ステータス判定処理追加) START
    ''' <summary>
    ''' ステータスコード(左側)取得
    ''' </summary>
    ''' <param name="inVisitType">来店実績有無(0：無、1：有)</param>
    ''' <param name="inAssignStatus">振当ステータス</param>
    ''' <param name="inCustomerType">顧客区分(1：自社客、2：未取引客)</param>
    ''' <param name="inWorkStartType">作業開始有無(0：無、1：有)</param>
    ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ''' <param name="inWashType">洗車有無(0：無、1：有)</param>
    ''' <param name="inOrderDataType">R/O有無(0：無、1：有)</param>
    ''' <param name="inOrderStatus">R/Oステータス</param>
    ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、2：部品準備済み、3：部品不要)</param>
    ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み)</param>
    ''' <param name="inAddWorkStatus">追加作業ステータス</param>
    ''' <param name="inInstruct">着工指示区分(0：未着工、2：着工準備)</param>
    ''' <param name="inResultWashStart">洗車開始実績日時</param>
    ''' <param name="inResultWashEnd">洗車終了実績日時</param>
    ''' <param name="inWorkEndType">作業終了有無(0：作業中、1：作業終了)</param>
    ''' <param name="inServiceStatus">サービスステータス</param>
    ''' <returns>ステータスコード(左側)</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/07/12 TMEJ 小澤 STEP2対応(ステータス判定処理追加)
    ''' 2012/08/15 TMEJ 日比野 STEP2対応(顧客区分がNULLの場合は未取引客とするように修正)
    ''' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function getStatusCodeLeft(ByVal inVisitType As String, _
                                       ByVal inAssignStatus As String, _
                                       ByVal inCustomerType As String, _
                                       ByVal inWorkStartType As String, _
                                       ByVal inStopType As String, _
                                       ByVal inWashType As String, _
                                       ByVal inOrderDataType As String, _
                                       ByVal inOrderStatus As String, _
                                       ByVal inPartsPreparationWaitType As String, _
                                       ByVal inCompleteExaminationType As String, _
                                       ByVal inAddWorkStatus As String, _
                                       ByVal inInstruct As String, _
                                       ByVal inResultWashStart As String, _
                                       ByVal inResultWashEnd As String, _
                                       ByVal inWorkEndType As String, _
                                       ByVal inServiceStatus As String) As String
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
        'Private Function getStatusCodeLeft(ByVal inVisitType As String, _
        '                                   ByVal inCustomerType As String, _
        '                                   ByVal inWorkStartType As String, _
        '                                   ByVal inStopType As String, _
        '                                   ByVal inWashType As String, _
        '                                   ByVal inOrderDataType As String, _
        '                                   ByVal inOrderStatus As String, _
        '                                   ByVal inPartsPreparationWaitType As String, _
        '                                   ByVal inCompleteExaminationType As String, _
        '                                   ByVal inAddWorkStatus As String, _
        '                                   ByVal inInstruct As String, _
        '                                   ByVal inResultWashStart As String, _
        '                                   ByVal inResultWashEnd As String) As String
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function getStatusCodeLeft(ByVal inVisitType As String, _
        '                                   ByVal inCustomerType As String, _
        '                                   ByVal inWorkStartType As String, _
        '                                   ByVal inStopType As String, _
        '                                   ByVal inWashType As String, _
        '                                   ByVal inOrderDataType As String, _
        '                                   ByVal inOrderStatus As String, _
        '                                   ByVal inPartsPreparationWaitType As String, _
        '                                   ByVal inCompleteExaminationType As String, _
        '                                   ByVal inAddWorkStatus As String, _
        '                                   ByVal inInstruct As String, _
        '                                   ByVal inResultWashStart As String, _
        '                                   ByVal inResultWashEnd As String, _
        '                                   ByVal dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable) As String
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11} P11:{12} P12:{13} P13:{14} P14:{15} P15:{16} P16:{17} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inVisitType, inAssignStatus, inCustomerType, inWorkStartType, inStopType _
                  , inWashType, inOrderDataType, inOrderStatus _
                  , inPartsPreparationWaitType, inCompleteExaminationType, inAddWorkStatus _
                  , inInstruct, inResultWashStart, inResultWashEnd, inWorkEndType, inServiceStatus))

        Dim statusLeft As String = String.Empty
        'ステータス(左側)
        If NoVisit.Equals(inVisitType) Then
            '来店有無(0：無)

            ' 2012/08/15 TMEJ 日比野 STEP2対応(顧客区分がNULLの場合は未取引客とするように修正) START
            'If NonBusinessGuest.Equals(inCustomerType) Then
            '    '顧客区分(2：未取引客)
            '    statusLeft = StatusCodeLeft101                                          '★101：新規お客様登録待ち
            'ElseIf CompanyVisitor.Equals(inCustomerType) Then
            '    '顧客区分(1：自社客)
            '    If ROInvalid.Equals(inOrderDataType) Then
            '        'R/O有無(0：無)
            '        statusLeft = StatusCodeLeft102                                      '★102：R/O作成待ち
            '    ElseIf ROEffective.Equals(inOrderDataType) Then
            '        'R/O有無(1：有)
            '        statusLeft = StatusCodeLeft103                                      '★103：R/O作成中
            '    End If
            'End If
            If CompanyVisitor.Equals(inCustomerType) Then
                '顧客区分(1：自社客)
                If ROInvalid.Equals(inOrderDataType) Then
                    'R/O有無(0：無)
                    statusLeft = StatusCodeLeft102                                      '★102：R/O作成待ち
                ElseIf ROEffective.Equals(inOrderDataType) Then
                    'R/O有無(1：有)
                    statusLeft = StatusCodeLeft103                                      '★103：R/O作成中
                End If
            Else
                '顧客区分(2/Null：未取引客)
                statusLeft = StatusCodeLeft101                                          '★101：新規お客様登録待ち
            End If
            ' 2012/08/15 TMEJ 日比野 STEP2対応(顧客区分がNULLの場合は未取引客とするように修正) END
        ElseIf Visit.Equals(inVisitType) Then
            '来店有無(1：有)

            ' 2012/08/15 TMEJ 日比野 STEP2対応(顧客区分がNULLの場合は未取引客とするように修正) START
            'If NonBusinessGuest.Equals(inCustomerType) Then
            '    '顧客区分(2：未取引客)
            '    If ROInvalid.Equals(inOrderDataType) Then
            '        'R/O有無(0：無)
            '        statusLeft = StatusCodeLeft104                                      '★104：新規お客様登録待ち
            '    End If
            'ElseIf CompanyVisitor.Equals(inCustomerType) Then
            '    '顧客区分(1：自社客)
            '    If ROInvalid.Equals(inOrderDataType) Then
            '        'R/O有無(0：無)
            '        statusLeft = StatusCodeLeft105                                      '★105：R/O作成待ち
            '    ElseIf ROEffective.Equals(inOrderDataType) Then
            '        'R/O有無(1：有)
            '        If ROReceptionist.Equals(inOrderStatus) OrElse _
            '           ROEstimate.Equals(inOrderStatus) Then
            '            'R/Oステータス(1：受付 or 5：見積確定待ち)
            '            statusLeft = StatusCodeLeft106                                  '★106：R/O作成中
            '        ElseIf ROParts.Equals(inOrderStatus) Then
            '            'R/Oステータス(4：部品待ち)
            '            statusLeft = Me.GetStatusLeftParts(inPartsPreparationWaitType, inInstruct)

            '        ElseIf ROMaintenance.Equals(inOrderStatus) OrElse _
            '               ROFinInspection.Equals(inOrderStatus) Then
            '            'R/Oステータス(2：整備中 or 7：検査完了)
            '            statusLeft = Me.GetStatusLeftNotParts(inWorkStartType, _
            '                                                  inStopType, _
            '                                                  inWashType, _
            '                                                  inResultStatus, _
            '                                                  inOrderStatus, _
            '                                                  inPartsPreparationWaitType, _
            '                                                  inCompleteExaminationType, _
            '                                                  inInstruct)

            '        ElseIf ROFinSales.Equals(inOrderStatus) OrElse _
            '               ROFinMaintenance.Equals(inOrderStatus) Then
            '            'R/Oステータス(3：売上済 or 6：整備完了)
            '            statusLeft = StatusCodeLeft121                                  '★121：納車待ち
            '        End If
            '    End If
            'End If

            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'If CompanyVisitor.Equals(inCustomerType) Then
            '    '顧客区分(1：自社客)
            '    If ROInvalid.Equals(inOrderDataType) Then
            '        'R/O有無(0：無)
            '        statusLeft = StatusCodeLeft105                                      '★105：R/O作成待ち
            '    ElseIf ROEffective.Equals(inOrderDataType) Then
            '        'R/O有無(1：有)
            '        If ROReceptionist.Equals(inOrderStatus) OrElse _
            '           ROEstimate.Equals(inOrderStatus) Then
            '            'R/Oステータス(1：受付 or 5：見積確定待ち)
            '            statusLeft = StatusCodeLeft106                                  '★106：R/O作成中
            '        ElseIf ROParts.Equals(inOrderStatus) Then
            '            'R/Oステータス(4：部品待ち)
            '            statusLeft = Me.GetStatusLeftParts(inPartsPreparationWaitType, inInstruct)

            '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
            '            'ElseIf ROMaintenance.Equals(inOrderStatus) OrElse _
            '            '       ROFinInspection.Equals(inOrderStatus) Then
            '            '    'R/Oステータス(2：整備中 or 7：検査完了)
            '            '    statusLeft = Me.GetStatusLeftNotParts(inWorkStartType, _
            '            '                                          inStopType, _
            '            '                                          inWashType, _
            '            '                                          inResultStatus, _
            '            '                                          inOrderStatus, _
            '            '                                          inPartsPreparationWaitType, _
            '            '                                          inCompleteExaminationType, _
            '            '                                          inAddWorkStatus, _
            '            '                                          inInstruct, _
            '            '                                          inResultWashStart, _
            '            '                                          inResultWashEnd)
            '        ElseIf ROMaintenance.Equals(inOrderStatus) Then
            '            'R/Oステータス(2：整備中)
            '            statusLeft = Me.GetStatusLeftROMaintenance(inWorkStartType, _
            '                                                       inStopType, _
            '                                                       inPartsPreparationWaitType, _
            '                                                       inCompleteExaminationType, _
            '                                                       inInstruct)
            '        ElseIf ROFinInspection.Equals(inOrderStatus) Then
            '            'R/Oステータス(7：検査完了)
            '            '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
            '            'statusLeft = Me.GetStatusLeftROFinInspection(inWorkStartType, _
            '            '                                             inStopType, _
            '            '                                             inWashType, _
            '            '                                             inPartsPreparationWaitType, _
            '            '                                             inCompleteExaminationType, _
            '            '                                             inAddWorkStatus, _
            '            '                                             inResultWashStart, _
            '            '                                             inResultWashEnd)
            '            statusLeft = Me.GetStatusLeftROFinInspection(inWorkStartType, _
            '                                                         inStopType, _
            '                                                         inWashType, _
            '                                                         inPartsPreparationWaitType, _
            '                                                         inCompleteExaminationType, _
            '                                                         inAddWorkStatus, _
            '                                                         inResultWashStart, _
            '                                                         inResultWashEnd, _
            '                                                         dtAddRepairStatus)
            '            '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END
            '            ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END

            '        ElseIf ROFinSales.Equals(inOrderStatus) OrElse _
            '               ROFinMaintenance.Equals(inOrderStatus) Then
            '            'R/Oステータス(3：売上済 or 6：整備完了)
            '            statusLeft = StatusCodeLeft121                                         '★121：納車待ち
            '        End If
            '    End If
            'Else
            '    '顧客区分(2/Null：未取引客)
            '    If ROInvalid.Equals(inOrderDataType) Then
            '        'R/O有無(0：無)
            '        statusLeft = StatusCodeLeft104                                             '★104：新規お客様登録待ち
            '    End If
            'End If
            ' 2012/08/15 TMEJ 日比野 STEP2対応(顧客区分がNULLの場合は未取引客とするように修正) END

            If AssignStatusAssignment.Equals(inAssignStatus) Then
                '振当ステータス(2：SA振当済み)
                If CompanyVisitor.Equals(inCustomerType) Then
                    '顧客区分(1：自社客)
                    If ROInvalid.Equals(inOrderDataType) Then
                        'R/O有無(0：無)
                        statusLeft = StatusCodeLeft105                                          '★105：R/O作成待ち

                    ElseIf ROEffective.Equals(inOrderDataType) Then
                        'R/O有無(1：有)
                        statusLeft = Me.GetStatusLeftStallROExist(inWorkStartType, _
                                                                  inStopType, _
                                                                  inWashType, _
                                                                  inOrderStatus, _
                                                                  inPartsPreparationWaitType, _
                                                                  inCompleteExaminationType, _
                                                                  inAddWorkStatus, _
                                                                  inInstruct, _
                                                                  inResultWashStart, _
                                                                  inResultWashEnd, _
                                                                  inWorkEndType, _
                                                                  inServiceStatus)

                    End If
                Else
                    '顧客区分(2/Null：未取引客)
                    statusLeft = StatusCodeLeft104                                              '★104：新規お客様登録待ち

                End If

            Else
                '振当ステータス(2：SA振当済み以外)
                If CompanyVisitor.Equals(inCustomerType) Then
                    '顧客区分(1：自社客)
                    If ROInvalid.Equals(inOrderDataType) Then
                        'R/O有無(0：無)
                        statusLeft = StatusCodeLeft136                                          '★136：SA振当待ち

                    ElseIf ROEffective.Equals(inOrderDataType) Then
                        'R/O有無(1：有)
                        statusLeft = StatusCodeLeft137                                          '★137：SA振当待ち

                    End If
                Else
                    '顧客区分(2/Null：未取引客)
                    statusLeft = StatusCodeLeft135                                              '★135：SA振当待ち

                End If
            End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
        End If

        'どの条件にも当てはまらなかった場合
        If String.IsNullOrEmpty(statusLeft) Then
            statusLeft = StatusCodeLeft199                                                      '★199：非表示

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusLeft))
        Return statusLeft
    End Function

    ''' <summary>
    ''' ステータスコード(左側)取得
    ''' ※R/Oステータス(50:着工指示待ち)の場合
    ''' </summary>
    ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、2：部品準備済み、3：部品不要)</param>
    ''' <param name="inInstruct">着工指示区分(00：未着工、00以外：着工指示済み)</param>
    ''' <returns>ステータスコード(左側)</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetStatusLeftParts(ByVal inPartsPreparationWaitType As String, _
                                        ByVal inInstruct As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inPartsPreparationWaitType, inInstruct))

        Dim statusLeft As String = String.Empty
        If NoGroundbreaking.Equals(inInstruct) Then
            '着工指示(00：未着工)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'If PartsPreparationWaiting.Equals(inPartsPreparationWaitType) Then
            '    '部品準備待ちフラグ(0：部品準備待ち)
            '    statusLeft = StatusCodeLeft108                          '★108：着工指示待ち/部品準備待ち

            'ElseIf PartsInPreparation.Equals(inPartsPreparationWaitType) _
            '    OrElse PartsPreparationFinish.Equals(inPartsPreparationWaitType) Then
            '    '部品準備待ちフラグ(1：部品準備中 or 2：部品準備済み)
            '    statusLeft = StatusCodeLeft109                          '★109：着工指示待ち/部品準備中

            'End If
            ''2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            ''ElseIf GroundbreakingPreparation.Equals(inInstruct) Then

            If PartsPreparationWaiting.Equals(inPartsPreparationWaitType) Then
                '部品準備待ちフラグ(0：部品準備待ち)
                statusLeft = StatusCodeLeft108                          '★108：着工指示待ち/部品準備待ち

            ElseIf PartsInPreparation.Equals(inPartsPreparationWaitType) Then
                '部品準備待ちフラグ(1：部品準備中)
                statusLeft = StatusCodeLeft109                          '★109：着工指示待ち/部品準備中

            ElseIf PartsPreparationFinish.Equals(inPartsPreparationWaitType) Then
                '部品準備待ちフラグ(8：部品準備済み)
                statusLeft = StatusCodeLeft110                          '★110：着工指示待ち/部品準備済み

            Else
                '上記以外の場合
                statusLeft = StatusCodeLeft107                          '★107：着工指示待ち

            End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Else
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            ''2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
            ''着工指示(00以外：着工指示済み)
            'If PartsPreparationWaiting.Equals(inPartsPreparationWaitType) Then
            '    '部品準備待ちフラグ(0：部品準備待ち)
            '    statusLeft = StatusCodeLeft111                          '★111：着工指示済み/部品準備待ち

            'ElseIf PartsInPreparation.Equals(inPartsPreparationWaitType) _
            '    OrElse PartsPreparationFinish.Equals(inPartsPreparationWaitType) Then
            '    '部品準備待ちフラグ(1：部品準備中 or 2：部品準備済み)
            '    statusLeft = StatusCodeLeft112                          '★112：着工指示済み/部品準備中

            'End If

            '着工指示(00以外：着工指示済み)
            If PartsPreparationWaiting.Equals(inPartsPreparationWaitType) Then
                '部品準備待ちフラグ(0：部品準備待ち)
                statusLeft = StatusCodeLeft111                          '★111：着工指示済み/部品準備待ち

            ElseIf PartsInPreparation.Equals(inPartsPreparationWaitType) Then
                '部品準備待ちフラグ(1：部品準備中)
                statusLeft = StatusCodeLeft112                          '★112：着工指示済み/部品準備中

            ElseIf PartsPreparationFinish.Equals(inPartsPreparationWaitType) OrElse _
                   String.IsNullOrEmpty(inPartsPreparationWaitType) Then
                '部品準備待ちフラグ(8：部品準備済み or データ無)
                statusLeft = StatusCodeLeft113                          '★113：作業開始待ち

            End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusLeft))
        Return statusLeft
    End Function

    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 START
    ' ''' <summary>
    ' ''' ステータスコード(左側)取得
    ' ''' ※R/Oステータス(2：整備中、7：検査完了)の場合
    ' ''' </summary>
    ' ''' <param name="inWorkStartType">作業開始有無(0：無、1：有)</param>
    ' ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ' ''' <param name="inWashType">洗車有無(0：無、1：有)</param>
    ' ''' <param name="inResultStatus">実績ステータス</param>
    ' ''' <param name="inOrderStatus">R/Oステータス</param>
    ' ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、2：部品準備済み、3：部品不要)</param>
    ' ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み)</param>
    ' ''' <param name="inInstruct">着工指示区分(0：未着工、2：着工準備)</param>
    ' ''' <param name="inResultWashStart">洗車開始実績日時</param>
    ' ''' <param name="inResultWashEnd">洗車終了実績日時</param>
    ' ''' <returns>ステータスコード(左側)</returns>
    ' ''' <remarks></remarks>
    'Private Function GetStatusLeftNotParts(ByVal inWorkStartType As String, _
    '                                       ByVal inStopType As String, _
    '                                       ByVal inWashType As String, _
    '                                       ByVal inResultStatus As String, _
    '                                       ByVal inOrderStatus As String, _
    '                                       ByVal inPartsPreparationWaitType As String, _
    '                                       ByVal inCompleteExaminationType As String, _
    '                                       ByVal inAddWorkStatus As String, _
    '                                       ByVal inInstruct As String, _
    '                                       ByVal inResultWashStart As String, _
    '                                       ByVal inResultWashEnd As String) As String
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} " _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '              , inWorkStartType, inStopType _
    '              , inWashType, inResultStatus, inOrderStatus _
    '              , inPartsPreparationWaitType, inCompleteExaminationType, inInstruct))

    '    Dim statusLeft As String = String.Empty
    '    If Not (New String() {SMWaitWash, SMWash, SMCustody, SMDelivery}.Contains(inResultStatus)) Then
    '        'SMBステータス(Not(40：洗車待ち or 41：洗車中 or 50：預かり中 or 60：納車待ち))
    '        If NoWorkStart.Equals(inWorkStartType) Then
    '            '作業開始有無(0：無)
    '            If NoGroundbreaking.Equals(inInstruct) Then
    '                '着工指示(0：未着工)
    '                If PartsPreparationFinish.Equals(inPartsPreparationWaitType) Then
    '                    '部品準備待ちフラグ(2：部品準備済み)
    '                    statusLeft = StatusCodeLeft110                      '★110：着工指示待ち/部品準備済み
    '                ElseIf PartsPreparationNeedlessness.Equals(inPartsPreparationWaitType) Then
    '                    '部品準備待ちフラグ(3：部品準備不要)
    '                    statusLeft = StatusCodeLeft107                      '★107：着工指示待ち
    '                End If
    '            ElseIf GroundbreakingPreparation.Equals(inInstruct) Then
    '                '着工指示(2：着工準備)
    '                statusLeft = StatusCodeLeft113                          '★113：作業開始待ち
    '            End If
    '        ElseIf Discontinuation.Equals(inWorkStartType) Then
    '            '作業開始有無(1：有)
    '            If Discontinuation.Equals(inStopType) Then
    '                '中断有無(1：有)
    '                statusLeft = StatusCodeLeft115                          '★115：中断中
    '            ElseIf NoDiscontinuation.Equals(inStopType) Then
    '                '中断有無(0：無)
    '                If NoCompleteExamination.Equals(inCompleteExaminationType) Then
    '                    '完成検査フラグ(0：無)
    '                    statusLeft = StatusCodeLeft114                      '★114：作業中
    '                ElseIf CompleteExamination.Equals(inCompleteExaminationType) Then
    '                    '完成検査フラグ(1：有)
    '                    statusLeft = StatusCodeLeft116                      '★116：完成検査待ち
    '                End If
    '            End If
    '        End If
    '    ElseIf ROFinInspection.Equals(inOrderStatus) Then
    '        'R/Oステータス(7：検査完了)
    '        If SMCustody.Equals(inResultStatus) OrElse _
    '           SMDelivery.Equals(inResultStatus) Then
    '            'SMBステータス(50：預かり中 or 60：納車待ち)
    '            If NoWashFlag.Equals(inWashType) Then
    '                '洗車有無(0：無)
    '                statusLeft = StatusCodeLeft117                          '★117：納車準備待ち
    '            ElseIf WashFlag.Equals(inWashType) Then
    '                '洗車有無(1：有)
    '                statusLeft = StatusCodeLeft120                          '★120：洗車完了/納車準備待ち
    '            End If
    '        ElseIf SMWaitWash.Equals(inResultStatus) Then
    '            'SMBステータス(40：洗車待ち)
    '            If WashFlag.Equals(inWashType) Then
    '                '洗車有無(1：有)
    '                statusLeft = StatusCodeLeft118                          '★118：洗車待ち/納車準備待ち
    '            End If
    '        ElseIf SMWash.Equals(inResultStatus) Then
    '            'SMBステータス(41：洗車中)
    '            If WashFlag.Equals(inWashType) Then
    '                '洗車有無(1：有)
    '                statusLeft = StatusCodeLeft119                          '★119：洗車中/納車準備待ち
    '            End If
    '        End If
    '    End If
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} OUT:RETURN = {2}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , statusLeft))
    '    Return statusLeft
    'End Function
    ' 2012/11/02 TMEJ 小澤 【SERVICE_2】次世代サービスROステータス切り離し対応 END

    ''' <summary>
    ''' ステータスコード(左側)取得
    ''' ※R/Oステータス(55:作業開始待ち、60:作業中、65:完成検査依頼中、70:完成検査完了)の場合
    ''' </summary>
    ''' <param name="inWorkStartType">作業開始有無(0：無、1：有)</param>
    ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、2：部品準備済み、3：部品不要)</param>
    ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み)</param>
    ''' <param name="inInstruct">着工指示区分(00：未着工、00以外：着工指示済み)</param>
    ''' <returns>ステータスコード(左側)</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' 2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加
    ''' </history>
    Private Function GetStatusLeftROMaintenance(ByVal inWorkStartType As String, _
                                                ByVal inStopType As String, _
                                                ByVal inPartsPreparationWaitType As String, _
                                                ByVal inCompleteExaminationType As String, _
                                                ByVal inInstruct As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inWorkStartType, inStopType _
                  , inPartsPreparationWaitType, inCompleteExaminationType, inInstruct))

        Dim statusLeft As String = String.Empty
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If NoWorkStart.Equals(inWorkStartType) Then
        '    '作業開始有無(0：無)
        '    If NoGroundbreaking.Equals(inInstruct) Then
        '        '着工指示(0：未着工)
        '        If PartsPreparationFinish.Equals(inPartsPreparationWaitType) Then
        '            '部品準備待ちフラグ(2：部品準備済み)
        '            statusLeft = StatusCodeLeft110                      '★110：着工指示待ち/部品準備済み

        '        ElseIf PartsPreparationNeedlessness.Equals(inPartsPreparationWaitType) Then
        '            '部品準備待ちフラグ(3：部品準備不要)
        '            statusLeft = StatusCodeLeft107                      '★107：着工指示待ち

        '        End If
        '        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        '        'ElseIf GroundbreakingPreparation.Equals(inInstruct) Then

        '    Else
        '        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
        '        '着工指示(2：着工準備)
        '        statusLeft = StatusCodeLeft113                          '★113：作業開始待ち

        '    End If

        'ElseIf Discontinuation.Equals(inWorkStartType) Then
        '    '作業開始有無(1：有)
        '    If Discontinuation.Equals(inStopType) Then
        '        '中断有無(1：有)
        '        statusLeft = StatusCodeLeft115                          '★115：中断中

        '    ElseIf NoDiscontinuation.Equals(inStopType) Then
        '        '中断有無(0：無)
        '        If NoCompleteExamination.Equals(inCompleteExaminationType) Then
        '            '完成検査フラグ(0：無)
        '            statusLeft = StatusCodeLeft114                      '★114：作業中

        '        ElseIf CompleteExamination.Equals(inCompleteExaminationType) Then
        '            '完成検査フラグ(1：有)
        '            statusLeft = StatusCodeLeft116                      '★116：完成検査待ち

        '        End If

        '    End If

        'End If

        If Discontinuation.Equals(inStopType) Then
            '中断有無(1：有)
            statusLeft = StatusCodeLeft115                          '★115：中断中

        ElseIf NoDiscontinuation.Equals(inStopType) Then
            '中断有無(0：無)

            '2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加 START

            'If NoCompleteExamination.Equals(inCompleteExaminationType) Then
            '    '完成検査フラグ(0：無)
            '    statusLeft = StatusCodeLeft114                      '★114：作業中

            'ElseIf RequestCompleteExamination.Equals(inCompleteExaminationType) Then
            '    '完成検査フラグ(1：有)
            '    statusLeft = StatusCodeLeft116                      '★116：完成検査待ち

            'End If

            If NoCompleteExamination.Equals(inCompleteExaminationType) Then
                '完成検査フラグ(0：承認依頼前)
                statusLeft = StatusCodeLeft114                      '★114：作業中

            ElseIf RequestCompleteExamination.Equals(inCompleteExaminationType) Then
                '完成検査フラグ(1：承認依頼中)
                statusLeft = StatusCodeLeft116                      '★116：完成検査待ち

            ElseIf FinishCompleteExamination.Equals(inCompleteExaminationType) Then
                '完成検査フラグ(2：承認済み)
                '※浮いているJOBが存在するということ
                statusLeft = StatusCodeLeft114                      '★114：作業中

            End If
            '2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加 END

        End If

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusLeft))
        Return statusLeft
    End Function

    ''' <summary>
    ''' ステータスコード(左側)取得
    ''' ※R/Oステータス(80:納車準備待ち)の場合
    ''' </summary>
    ''' <param name="inWorkStartType">作業開始有無(0：無、1：有)</param>
    ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ''' <param name="inWashType">洗車有無(0：無、1：有)</param>
    ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、2：部品準備済み、3：部品不要)</param>
    ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み)</param>
    ''' <param name="inResultWashStart">洗車開始実績日時</param>
    ''' <param name="inResultWashEnd">洗車終了実績日時</param>
    ''' <param name="inWorkEndType">作業終了有無(0：作業中、1：作業終了)</param>
    ''' <returns>ステータスコード(左側)</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応
    ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' 2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加
    ''' </history>
    Private Function GetStatusLeftROFinInspection(ByVal inWorkStartType As String, _
                                                  ByVal inStopType As String, _
                                                  ByVal inWashType As String, _
                                                  ByVal inPartsPreparationWaitType As String, _
                                                  ByVal inCompleteExaminationType As String, _
                                                  ByVal inAddWorkStatus As String, _
                                                  ByVal inResultWashStart As String, _
                                                  ByVal inResultWashEnd As String, _
                                                  ByVal inWorkEndType As String, _
                                                  ByVal inServiceStatus As String) As String
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
        'Private Function GetStatusLeftROFinInspection(ByVal inWorkStartType As String, _
        '                                              ByVal inStopType As String, _
        '                                              ByVal inWashType As String, _
        '                                              ByVal inPartsPreparationWaitType As String, _
        '                                              ByVal inCompleteExaminationType As String, _
        '                                              ByVal inAddWorkStatus As String, _
        '                                              ByVal inResultWashStart As String, _
        '                                              ByVal inResultWashEnd As String) As String
        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function GetStatusLeftROFinInspection(ByVal inWorkStartType As String, _
        '                                              ByVal inStopType As String, _
        '                                              ByVal inWashType As String, _
        '                                              ByVal inPartsPreparationWaitType As String, _
        '                                              ByVal inCompleteExaminationType As String, _
        '                                              ByVal inAddWorkStatus As String, _
        '                                              ByVal inResultWashStart As String, _
        '                                              ByVal inResultWashEnd As String, _
        '                                              ByVal dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable) As String
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inWorkStartType, inStopType _
                  , inWashType, inPartsPreparationWaitType, inCompleteExaminationType _
                  , inAddWorkStatus, inResultWashStart, inResultWashEnd, inWorkEndType, inServiceStatus))

        '2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 START
        'Dim statusLeft As String = String.Empty
        'If String.IsNullOrEmpty(inAddWorkStatus) OrElse "9".Equals(inAddWorkStatus) Then
        '    '追加作業ステータス(無) AndAlso 追加作業ステータス(9:検査完了)
        '    If NoWashFlag.Equals(inWashType) Then
        '        '洗車有無(0：無)
        '        statusLeft = StatusCodeLeft117                              '★117：納車準備待ち
        '    ElseIf WashFlag.Equals(inWashType) Then
        '        '洗車有無(1：有)
        '        If String.IsNullOrEmpty(inResultWashStart) Then
        '            '洗車開始実績日時(データ無)
        '            statusLeft = StatusCodeLeft118                          '★118：洗車待ち/納車準備待ち
        '        ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
        '               String.IsNullOrEmpty(inResultWashEnd) Then
        '            '洗車開始実績日時(データ有)、洗車終了実績日時(データ無)
        '            statusLeft = StatusCodeLeft119                          '★119：洗車中/納車準備待ち
        '        ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
        '               Not (String.IsNullOrEmpty(inResultWashEnd)) Then
        '            '洗車開始実績日時(データ有)、洗車終了実績日時(データ有)
        '            statusLeft = StatusCodeLeft120                          '★120：洗車完了/納車準備待ち
        '        End If
        '    End If
        'Else
        '    '追加作業ステータス(有) AndAlso 追加作業ステータス(9:検査完了)以外
        '    If Discontinuation.Equals(inStopType) Then
        '        '中断有無(1：有)
        '        statusLeft = StatusCodeLeft122                              '★122：中断中
        '    ElseIf NoDiscontinuation.Equals(inStopType) Then
        '        '中断有無(0：無)
        '        If NoWorkStart.Equals(inWorkStartType) Then
        '            '作業開始有無(0：無)
        '            If PartsPreparationFinish.Equals(inPartsPreparationWaitType) OrElse _
        '               PartsPreparationNeedlessness.Equals(inPartsPreparationWaitType) Then
        '                '部品準備待ちフラグ(2：部品準備済み、3：部品準備不要)
        '                statusLeft = StatusCodeLeft123                      '★123：作業開始待ち
        '            End If
        '        ElseIf Discontinuation.Equals(inWorkStartType) Then
        '            '作業開始有無(1：有)
        '            If NoCompleteExamination.Equals(inCompleteExaminationType) Then
        '                '完成検査フラグ(0：無)
        '                statusLeft = StatusCodeLeft124                      '★124：作業中
        '            ElseIf CompleteExamination.Equals(inCompleteExaminationType) Then
        '                '完成検査フラグ(1：有)
        '                If "8".Equals(inAddWorkStatus) Then
        '                    '追加作業ステータス(8：整備中)
        '                    statusLeft = StatusCodeLeft126                  '★126：完成検査待ち
        '                Else
        '                    '追加作業ステータス(8：整備中)以外
        '                    statusLeft = StatusCodeLeft125                  '★125：作業中
        '                End If
        '            End If
        '        End If
        '    End If
        'End If
        Dim statusLeft As String = String.Empty
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If Not IsNothing(dtAddRepairStatus) OrElse 0 < dtAddRepairStatus.Count Then
        '    '追加作業ありの場合
        '    Dim rowAddList As IC3800804AddRepairStatusDataTableRow() = _
        '        (From col In dtAddRepairStatus Where col.STATUS <> "9" Select col).ToArray
        '    If 0 = rowAddList.Count Then
        '        If NoWashFlag.Equals(inWashType) Then
        '            '洗車有無(0：無)
        '            statusLeft = StatusCodeLeft117                              '★117：納車準備待ち
        '        ElseIf WashFlag.Equals(inWashType) Then
        '            '洗車有無(1：有)
        '            If String.IsNullOrEmpty(inResultWashStart) Then
        '                '洗車開始実績日時(データ無)
        '                statusLeft = StatusCodeLeft118                          '★118：洗車待ち/納車準備待ち
        '            ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
        '                   String.IsNullOrEmpty(inResultWashEnd) Then
        '                '洗車開始実績日時(データ有)、洗車終了実績日時(データ無)
        '                statusLeft = StatusCodeLeft119                          '★119：洗車中/納車準備待ち
        '            ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
        '                   Not (String.IsNullOrEmpty(inResultWashEnd)) Then
        '                '洗車開始実績日時(データ有)、洗車終了実績日時(データ有)
        '                statusLeft = StatusCodeLeft120                          '★120：洗車完了/納車準備待ち
        '            End If
        '        End If
        '    Else
        '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　Strat
        '        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        '        'statusLeft = GetStatusLeftROFinInspectionSub(inStopType, inWorkStartType, _
        '        '                                             inPartsPreparationWaitType, _
        '        '                                             inCompleteExaminationType, _
        '        '                                             rowAddList)
        '        statusLeft = GetStatusLeftROFinInspectionSub(inWorkStartType, _
        '                                                     inStopType, _
        '                                                     inPartsPreparationWaitType, _
        '                                                     inCompleteExaminationType, _
        '                                                     rowAddList)
        '        '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
        '        'If Discontinuation.Equals(inStopType) Then
        '        '    '中断有無(1：有)
        '        '    statusLeft = StatusCodeLeft122                              '★122：中断中
        '        'ElseIf NoDiscontinuation.Equals(inStopType) Then
        '        '    '中断有無(0：無)
        '        '    If NoWorkStart.Equals(inWorkStartType) Then
        '        '        '作業開始有無(0：無)
        '        '        If PartsPreparationFinish.Equals(inPartsPreparationWaitType) OrElse _
        '        '           PartsPreparationNeedlessness.Equals(inPartsPreparationWaitType) Then
        '        '            '部品準備待ちフラグ(2：部品準備済み、3：部品準備不要)
        '        '            statusLeft = StatusCodeLeft123                      '★123：作業開始待ち
        '        '        End If
        '        '    ElseIf Discontinuation.Equals(inWorkStartType) Then
        '        '        '作業開始有無(1：有)
        '        '        If NoCompleteExamination.Equals(inCompleteExaminationType) Then
        '        '            '完成検査フラグ(0：無)
        '        '            statusLeft = StatusCodeLeft124                      '★124：作業中
        '        '        ElseIf CompleteExamination.Equals(inCompleteExaminationType) Then
        '        '            '完成検査フラグ(1：有)

        '        '            Dim drAddRepairStatus As IC3800804AddRepairStatusDataTableRow() = _
        '        '                (From col In rowAddList Where col.STATUS <> "8" Select col).ToArray

        '        '            If 0 < drAddRepairStatus.Count Then
        '        '                '追加作業ステータス(8：整備中)以外が存在する場合
        '        '                statusLeft = StatusCodeLeft125                  '★125：作業中
        '        '            Else
        '        '                '追加作業ステータス(8：整備中)が存在しない場合
        '        '                statusLeft = StatusCodeLeft126                  '★126：完成検査待ち
        '        '            End If
        '        '        End If
        '        '    End If
        '        'End If
        '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　End
        '    End If
        'Else
        '    '追加作業なしの場合
        '    If NoWashFlag.Equals(inWashType) Then
        '        '洗車有無(0：無)
        '        statusLeft = StatusCodeLeft117                              '★117：納車準備待ち
        '    ElseIf WashFlag.Equals(inWashType) Then
        '        '洗車有無(1：有)
        '        If String.IsNullOrEmpty(inResultWashStart) Then
        '            '洗車開始実績日時(データ無)
        '            statusLeft = StatusCodeLeft118                          '★118：洗車待ち/納車準備待ち
        '        ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
        '               String.IsNullOrEmpty(inResultWashEnd) Then
        '            '洗車開始実績日時(データ有)、洗車終了実績日時(データ無)
        '            statusLeft = StatusCodeLeft119                          '★119：洗車中/納車準備待ち
        '        ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
        '               Not (String.IsNullOrEmpty(inResultWashEnd)) Then
        '            '洗車開始実績日時(データ有)、洗車終了実績日時(データ有)
        '            statusLeft = StatusCodeLeft120                          '★120：洗車完了/納車準備待ち
        '        End If
        '    End If
        'End If
        ''2012/11/28 TMEJ 小澤 問連「GTMC121126088」対応 END

        If WorkEndTypeWorking.Equals(inWorkEndType) Then
            '終わっていない作業がある場合

            If Discontinuation.Equals(inStopType) Then
                '中断有無(1：有)
                Return StatusCodeLeft122                                '★122：中断中

            ElseIf NoDiscontinuation.Equals(inStopType) Then
                '中断有無(0：無)

                '2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加 START

                'If NoCompleteExamination.Equals(inCompleteExaminationType) Then
                '    '完成検査フラグ(0：承認依頼前)
                '    Return StatusCodeLeft124                            '★124：作業中

                'ElseIf RequestCompleteExamination.Equals(inCompleteExaminationType) OrElse _
                '       FinishCompleteExamination.Equals(inCompleteExaminationType) Then
                '    '完成検査フラグ(1：承認依頼中)、完成検査フラグ(2：承認済み)
                '    If ROWorking.Equals(inAddWorkStatus) Then
                '        '追加作業ROステータス(60：作業中)の場合
                '        Return StatusCodeLeft126                        '★126：完成検査待ち

                '    Else
                '        '追加作業ROステータス(60：作業中)以外の場合
                '        Return StatusCodeLeft125                        '★125：作業中

                '    End If
                'End If

                '中断有無(0：無)
                If NoCompleteExamination.Equals(inCompleteExaminationType) Then
                    '完成検査フラグ(0：承認依頼前)
                    Return StatusCodeLeft124                            '★124：作業中

                ElseIf RequestCompleteExamination.Equals(inCompleteExaminationType) Then
                    '完成検査フラグ(1：承認依頼中)
                    Return StatusCodeLeft126                        '★126：完成検査待ち

                ElseIf FinishCompleteExamination.Equals(inCompleteExaminationType) Then
                    '完成検査フラグ(2：承認済み)
                    '※浮いているJOBが存在するということ
                    Return StatusCodeLeft125                        '★125：作業中

                End If

                '2014/09/12 TMEJ 小澤 BTS対応 作業完了フラグの条件追加 END

            End If

        Else
            '終わっていない作業がない場合
            If NoWashFlag.Equals(inWashType) OrElse _
               ServiceStatusDropOff.Equals(inServiceStatus) OrElse _
               ServiceStatusWaitDalivery.Equals(inServiceStatus) Then
                '洗車有無(0：無)、サービスステータス（11：預かり中）、サービスステータス（12：納車待ち）
                statusLeft = StatusCodeLeft117                              '★117：納車準備待ち

            ElseIf WashFlag.Equals(inWashType) Then
                '洗車有無(1：有)
                If String.IsNullOrEmpty(inResultWashStart) Then
                    '洗車開始実績日時(データ無)
                    statusLeft = StatusCodeLeft118                          '★118：洗車待ち/納車準備待ち

                ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
                       String.IsNullOrEmpty(inResultWashEnd) Then
                    '洗車開始実績日時(データ有)、洗車終了実績日時(データ無)
                    statusLeft = StatusCodeLeft119                          '★119：洗車中/納車準備待ち

                ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
                       Not (String.IsNullOrEmpty(inResultWashEnd)) Then
                    '洗車開始実績日時(データ有)、洗車終了実績日時(データ有)
                    statusLeft = StatusCodeLeft120                          '★120：洗車完了/納車準備待ち

                End If

            End If

        End If
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusLeft))
        Return statusLeft
    End Function

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
    ''2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　Strat
    ' ''' <summary>
    ' ''' ステータスコード(左側)取得サブ処理
    ' ''' ※R/Oステータス(7：検査完了)の場合
    ' ''' </summary>
    ' ''' <param name="inWorkStartType">作業開始有無(0：無、1：有)</param>
    ' ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ' ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、2：部品準備済み、3：部品不要)</param>
    ' ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み)</param>
    ' ''' <returns>ステータスコード(左側)</returns>
    ' ''' <remarks></remarks>
    'Private Function GetStatusLeftROFinInspectionSub(ByVal inWorkStartType As String, _
    '                                                 ByVal inStopType As String, _
    '                                                 ByVal inPartsPreparationWaitType As String, _
    '                                                 ByVal inCompleteExaminationType As String, _
    '                                                 ByVal inrowAddList As IC3800804AddRepairStatusDataTableRow()) As String
    '    If Discontinuation.Equals(inStopType) Then
    '        '中断有無(1：有)
    '        Return StatusCodeLeft122                              '★122：中断中
    '    ElseIf NoDiscontinuation.Equals(inStopType) Then
    '        '中断有無(0：無)
    '        If NoWorkStart.Equals(inWorkStartType) Then
    '            '作業開始有無(0：無)
    '            If PartsPreparationFinish.Equals(inPartsPreparationWaitType) OrElse _
    '               PartsPreparationNeedlessness.Equals(inPartsPreparationWaitType) Then
    '                '部品準備待ちフラグ(2：部品準備済み、3：部品準備不要)
    '                Return StatusCodeLeft123                      '★123：作業開始待ち
    '            End If
    '        ElseIf Discontinuation.Equals(inWorkStartType) Then
    '            '作業開始有無(1：有)
    '            If NoCompleteExamination.Equals(inCompleteExaminationType) Then
    '                '完成検査フラグ(0：無)
    '                Return StatusCodeLeft124                      '★124：作業中
    '            ElseIf CompleteExamination.Equals(inCompleteExaminationType) Then
    '                '完成検査フラグ(1：有)

    '                Dim drAddRepairStatus As IC3800804AddRepairStatusDataTableRow() = _
    '                    (From col In inrowAddList Where col.STATUS <> "8" Select col).ToArray

    '                If 0 < drAddRepairStatus.Count Then
    '                    '追加作業ステータス(8：整備中)以外が存在する場合
    '                    Return StatusCodeLeft125                  '★125：作業中
    '                Else
    '                    '追加作業ステータス(8：整備中)が存在しない場合
    '                    Return StatusCodeLeft126                  '★126：完成検査待ち
    '                End If
    '            End If
    '        End If
    '    End If
    '    Return String.Empty
    'End Function
    ''2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　End
    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' ステータスコード(右側)取得
    ''' </summary>
    ''' <param name="inOrderDataType">R/O有無(0：無、1：有)</param>
    ''' <param name="inOrderStatus">R/Oステータス</param>
    ''' <param name="inAddWorkStatus">追加作業ステータス</param>
    ''' <param name="inReissueVouchers">起票者(TC：TC、SA：SA)</param>
    ''' <returns>ステータスコード(右側)</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function getStatusCodeRight(ByVal inOrderDataType As String, _
                                        ByVal inOrderStatus As String, _
                                        ByVal inAddWorkStatus As String, _
                                        ByVal inReissueVouchers As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inOrderDataType, inOrderStatus, inAddWorkStatus, inReissueVouchers))

        Dim statusRight As String = String.Empty
        'ステータス(右側)
        If ROEffective.Equals(inOrderDataType) Then
            'R/O有無(1：有)
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
            'If ROMaintenance.Equals(inOrderStatus) OrElse _
            '   ROParts.Equals(inOrderStatus) OrElse _
            '   ROFinInspection.Equals(inOrderStatus) Then
            '    'R/Oステータス(2：整備中 or 4：部品準備待ち or 7：検査完了)
            '    If TechnicianInVouchers.Equals(inAddWorkStatus) Then
            '        '追加作業ステータス(1：TC起票中)
            '        statusRight = StatusCodeRight201                                '★201：TC追加作業起票中

            '    ElseIf ControllerRecognitionWaiting.Equals(inAddWorkStatus) Then
            '        '追加作業ステータス(2：CT承認待ち)
            '        statusRight = StatusCodeRight202                                '★202：CT承認待ち

            '    ElseIf PartStaffEstimateWaiting.Equals(inAddWorkStatus) Then
            '        '追加作業ステータス(3：PS部品見積待ち)
            '        statusRight = StatusCodeRight203                                '★203：部品見積待ち

            '    ElseIf ServiceAssitantEstimateWaiting.Equals(inAddWorkStatus) Then
            '        '追加作業ステータス(4：SA見積確定待ち)
            '        If ReissueVouchersSA.Equals(inReissueVouchers) Then
            '            '起票者(SA)
            '            statusRight = StatusCodeRight205                            '★205：SA追加作業起票中

            '        ElseIf ReissueVouchersTC.Equals(inReissueVouchers) Then
            '            '起票者(TC)
            '            statusRight = StatusCodeRight206                            '★206：SA見積確定待ち

            '        End If

            '    ElseIf CustomerRecognitionWaiting.Equals(inAddWorkStatus) Then
            '        '追加作業ステータス(5：顧客承認待ち)
            '        statusRight = StatusCodeRight207                                '★207：お客様承認待ち

            '    ElseIf ControllerGroundbreakingParts.Equals(inAddWorkStatus) Then
            '        '追加作業ステータス(6：CT着工指示・部品準備待ち)
            '        statusRight = StatusCodeRight208                                '★208：非表示

            '    ElseIf GroundbreakingPartsWaiting.Equals(inAddWorkStatus) Then
            '        '追加作業ステータス(7：TC作業開始待ち)
            '        statusRight = StatusCodeRight209                                '★209：非表示

            '    ElseIf MaintenanceWaiting.Equals(inAddWorkStatus) Then
            '        '追加作業ステータス(8：整備待ち)
            '        statusRight = StatusCodeRight210                                '★210：非表示

            '    ElseIf CompleteExaminationFinish.Equals(inAddWorkStatus) Then
            '        '追加作業ステータス(9：完成検査完了)
            '        statusRight = StatusCodeRight211                                '★211：非表示

            '    End If
            'End If
            If ROWaitStruct.Equals(inOrderStatus) OrElse _
               ROWorking.Equals(inOrderStatus) OrElse _
               ROWaitDeliveryPreparation.Equals(inOrderStatus) Then
                'R/Oステータス「50:着工指示待ち(顧客承認完了)」「60:作業中」「80:R/Oｽﾃｰﾀｽ(納車準備待ち)」
                If ServiceAssistantInVouchers.Equals(inAddWorkStatus) Then
                    '追加作業ステータス(10：SA起票中)
                    statusRight = StatusCodeRight205                                '★205：SA追加作業起票中

                ElseIf TechnicianInVouchers.Equals(inAddWorkStatus) Then
                    '追加作業ステータス(15：TC起票中)
                    statusRight = StatusCodeRight201                                '★201：TC追加作業起票中

                ElseIf ControllerRecognitionWaiting.Equals(inAddWorkStatus) Then
                    '追加作業ステータス(20：CT承認待ち)
                    statusRight = StatusCodeRight202                                '★202：CT承認待ち

                ElseIf PartStaffDummyEstimateWaiting.Equals(inAddWorkStatus) OrElse _
                       PartStaffEstimateWaiting.Equals(inAddWorkStatus) Then
                    '追加作業ステータス(25：PS部品仮見積待ち、30：PS部品本見積待ち）
                    statusRight = StatusCodeRight203                                '★203：部品見積待ち

                ElseIf ServiceAssitantEstimateWaiting.Equals(inAddWorkStatus) Then
                    '追加作業ステータス(35：SA見積確定待ち)
                    If ReissueVouchersSA.Equals(inReissueVouchers) Then
                        '起票者(SA)
                        statusRight = StatusCodeRight205                            '★205：SA追加作業起票中

                    ElseIf ReissueVouchersTC.Equals(inReissueVouchers) Then
                        '起票者(TC)
                        statusRight = StatusCodeRight206                            '★206：SA見積確定待ち

                    End If

                ElseIf CustomerRecognitionWaiting.Equals(inAddWorkStatus) Then
                    '追加作業ステータス(40：顧客承認待ち)
                    statusRight = StatusCodeRight207                                '★207：お客様承認待ち

                ElseIf ControllerGroundbreakingParts.Equals(inAddWorkStatus) Then
                    '追加作業ステータス(50：CT着工指示・部品準備待ち)
                    statusRight = StatusCodeRight208                                '★208：非表示

                ElseIf GroundbreakingPartsWaiting.Equals(inAddWorkStatus) Then
                    '追加作業ステータス(60：TC作業開始待ち)
                    statusRight = StatusCodeRight209                                '★209：非表示

                ElseIf MaintenanceWaiting.Equals(inAddWorkStatus) Then
                    '追加作業ステータス(80：整備待ち)
                    statusRight = StatusCodeRight210                                '★210：非表示

                ElseIf CompleteExaminationFinish.Equals(inAddWorkStatus) Then
                    '追加作業ステータス(85：完成検査完了)
                    statusRight = StatusCodeRight211                                '★211：非表示

                End If
            End If
            '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
        End If

        'どの条件にも当てはまらなかった場合
        If String.IsNullOrEmpty(statusRight) Then
            statusRight = StatusCodeRight299                                        '★299：非表示

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusRight))
        Return statusRight
    End Function
    '2012/07/12 TMEJ 小澤 STEP2対応(ステータス判定処理追加) END

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    ''' <summary>
    ''' SMBステータスコード(左側)取得
    ''' </summary>
    ''' <param name="inChipAreaType">チップエリア(1：ストール、2：受付、3：追加作業、4：完成検査、5：洗車、6：納車待ち、7：中断、8：NoShow)</param>
    ''' <param name="inVisitSequence">来店実績連番</param>
    ''' <param name="inOrderType">R/O情報有無(0：無、1：有)</param>
    ''' <param name="inOrderNo">R/O番号</param>
    ''' <param name="inWorkStartDate">実績開始日時</param>
    ''' <param name="inStallUseStatus">ストール利用ステータス</param>
    ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、8：部品準備済み、NULL：部品不要)</param>
    ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ''' <param name="inWorkEndDate">実績終了日時</param>
    ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み、2：完成検査承認済み)</param>
    ''' <param name="inServiceinStatus">サービス入庫ステータス</param>
    ''' <param name="inInvoicePrintDate">清算書印刷日時</param>
    ''' <returns>ステータスコード(左側)</returns>
    ''' <remarks></remarks>
    Private Function GetSMBStatusCodeLeft(ByVal inChipAreaType As Integer, _
                                          ByVal inVisitSequence As Long, _
                                          ByVal inOrderType As String, _
                                          ByVal inOrderNo As String, _
                                          ByVal inWorkStartDate As Date, _
                                          ByVal inStallUseStatus As String, _
                                          ByVal inPartsPreparationWaitType As String, _
                                          ByVal inStopType As String, _
                                          ByVal inWorkEndDate As Date, _
                                          ByVal inCompleteExaminationType As String, _
                                          ByVal inServiceinStatus As String, _
                                          ByVal inInvoicePrintDate As Date) As String
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'Private Function GetSMBStatusCodeLeft(ByVal inVisitType As String, _
        '                                      ByVal inWorkStartDate As Date, _
        '                                      ByVal inWorkEndDate As Date, _
        '                                      ByVal inStopType As String, _
        '                                      ByVal inWashType As String, _
        '                                      ByVal inOrderNo As String, _
        '                                      ByVal inOrderStatus As String, _
        '                                      ByVal inPartsPreparationWaitType As String, _
        '                                      ByVal inCompleteExaminationType As String, _
        '                                      ByVal inStallUseStatus As String, _
        '                                      ByVal inResultWashStart As String, _
        '                                      ByVal inResultWashEnd As String, _
        '                                      ByVal dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable, _
        '                                      ByVal drReserveROStatusList As IC3801012DataSet.REZROStatusListRow, _
        '                                      ByVal inSequenceNo As Long, _
        '                                      ByVal inDeliveryDate As Date) As String
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11} P11:{12} P12:{13} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inChipAreaType.ToString(CultureInfo.CurrentCulture), inVisitSequence.ToString(CultureInfo.CurrentCulture) _
                  , inOrderType, inOrderNo, inWorkStartDate.ToString(CultureInfo.CurrentCulture), inStallUseStatus _
                  , inPartsPreparationWaitType, inStopType, inWorkEndDate.ToString(CultureInfo.CurrentCulture) _
                  , inCompleteExaminationType, inServiceinStatus, inInvoicePrintDate.ToString(CultureInfo.CurrentCulture)))

        Dim statusLeft As String = String.Empty
        'ステータス(左側)
        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
        'If NoVisit.Equals(inVisitType) Then
        '    '来店有無(0：無)
        '    If String.IsNullOrEmpty(Trim(inOrderNo)) Then
        '        'RO番号(データ無)
        '        statusLeft = StatusCodeLeft127                                          '★127：仮R/O作成待ち

        '    Else
        '        'RO番号(データ有)
        '        If ROReceptionist.Equals(drReserveROStatusList.STATUS) Then
        '            'R/Oステータス(1：受付)
        '            statusLeft = StatusCodeLeft128                                      '★128：仮R/O作成中

        '        ElseIf ROEstimate.Equals(drReserveROStatusList.STATUS) Then
        '            'R/Oステータス(5：見積確定待ち)
        '            statusLeft = StatusCodeLeft129                                      '★129：仮R/O作成済み

        '        End If
        '    End If
        'ElseIf Visit.Equals(inVisitType) Then
        '    '来店有無(1：有)
        '    If String.IsNullOrEmpty(Trim(inOrderNo)) Then
        '        'R/O番号(データ無)
        '        statusLeft = StatusCodeLeft105                                          '★105：R/O作成待ち

        '    Else
        '        'R/O番号(データ有)
        '        If ROReceptionist.Equals(inOrderStatus) OrElse _
        '           ROEstimate.Equals(inOrderStatus) Then
        '            'R/Oステータス(1：受付 or 5：見積確定待ち)
        '            statusLeft = StatusCodeLeft106                                      '★106：R/O作成中

        '        ElseIf ROParts.Equals(inOrderStatus) Then
        '            'R/Oステータス(4：部品待ち)
        '            statusLeft = Me.GetSMBStatusLeftParts(inPartsPreparationWaitType, _
        '                                                  inStallUseStatus, _
        '                                                  dtAddRepairStatus, _
        '                                                  inSequenceNo)

        '        ElseIf ROMaintenance.Equals(inOrderStatus) Then
        '            'R/Oステータス(2：整備中)
        '            statusLeft = Me.GetSMBStatusLeftROMaintenance(inWorkStartDate, _
        '                                                          inWorkEndDate, _
        '                                                          inStopType, _
        '                                                          inPartsPreparationWaitType, _
        '                                                          inCompleteExaminationType, _
        '                                                          inStallUseStatus, _
        '                                                          dtAddRepairStatus, _
        '                                                          inSequenceNo)
        '        ElseIf ROFinInspection.Equals(inOrderStatus) Then
        '            'R/Oステータス(7：検査完了)
        '            statusLeft = Me.GetSMBStatusLeftROFinInspection(inWorkStartDate, _
        '                                                            inWorkEndDate, _
        '                                                            inStopType, _
        '                                                            inWashType, _
        '                                                            inPartsPreparationWaitType, _
        '                                                            inCompleteExaminationType, _
        '                                                            inStallUseStatus, _
        '                                                            inResultWashStart, _
        '                                                            inResultWashEnd, _
        '                                                            dtAddRepairStatus, _
        '                                                            inSequenceNo)
        '        ElseIf ROFinSales.Equals(inOrderStatus) OrElse _
        '               ROFinMaintenance.Equals(inOrderStatus) OrElse _
        '               ROFinDelivery.Equals(inOrderStatus) Then
        '            'R/Oステータス(3：売上済 or 6：整備完了 or 8：納車完了)
        '            statusLeft = Me.GetSMBStatusLeftROSalesMaintenance(inWashType, _
        '                                                               inResultWashStart, _
        '                                                               inResultWashEnd, _
        '                                                               inDeliveryDate)
        '        End If
        '    End If
        'End If

        'チップの位置チェック
        Select Case inChipAreaType
            Case SmbChipAreaType.Stall
                'ストールエリアの場合
                statusLeft = Me.GetSMBStatusLeftStall(inVisitSequence, _
                                                      inOrderType, _
                                                      inOrderNo, _
                                                      inWorkStartDate, _
                                                      inStallUseStatus, _
                                                      inPartsPreparationWaitType, _
                                                      inStopType, _
                                                      inWorkEndDate, _
                                                      inServiceinStatus, _
                                                      inInvoicePrintDate, _
                                                      inCompleteExaminationType)

            Case SmbChipAreaType.Receptionist
                '受付タブの場合
                statusLeft = Me.GetSMBStatusLeftReceptionist(inPartsPreparationWaitType)

            Case SmbChipAreaType.AddWord
                '追加作業タブの場合
                statusLeft = StatusCodeLeft131                                          '★131：追加作業承認待ち

            Case SmbChipAreaType.Inspection
                '完成検査の場合
                statusLeft = StatusCodeLeft126                                          '★126：完成検査承認待ち

            Case SmbChipAreaType.CarWash
                '洗車タブの場合
                statusLeft = Me.GetSMBStatusLeftCarWash(inServiceinStatus, _
                                                        inInvoicePrintDate)

            Case SmbChipAreaType.WaitDelivery
                '納車待ちタブの場合
                statusLeft = Me.GetSMBStatusLeftWaitDelivery(inInvoicePrintDate)

            Case SmbChipAreaType.ChipStop
                '中断タブの場合
                statusLeft = StatusCodeLeft115                                          '★115：中断中

            Case SmbChipAreaType.NoShow
                'NoShowタブの場合
                statusLeft = StatusCodeLeft127                                          '★127：仮R/O作成待ち

        End Select

        '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        'どの条件にも当てはまらなかった場合
        If String.IsNullOrEmpty(statusLeft) Then
            statusLeft = StatusCodeLeft199                                              '★199：非表示

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusLeft))
        Return statusLeft
    End Function

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' SMBステータスコード(左側)取得
    ' ''' ※R/Oステータス(4：部品待ち)の場合
    ' ''' </summary>
    ' ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、2：部品準備済み、3：部品不要)</param>
    ' ''' <param name="inStallUseStatus">ストール利用ステータス(00：着工指示待ち、01：作業開始待ち)</param>
    ' ''' <param name="dtAddRepairStatus">追加作業ステータス</param>
    ' ''' <param name="inSequenceNo">枝番</param>
    ' ''' <returns>ステータスコード(左側)</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ' ''' </history>
    'Private Function GetSMBStatusLeftParts(ByVal inPartsPreparationWaitType As String, _
    '                                       ByVal inStallUseStatus As String, _
    '                                       ByVal dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable, _
    '                                       ByVal inSequenceNo As Long) As String
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} " _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '              , inPartsPreparationWaitType, inStallUseStatus _
    '              , dtAddRepairStatus, inSequenceNo))

    '    Dim statusLeft As String = String.Empty

    '    If inSequenceNo = 0 Then
    '        '枝番(0：親の連番)
    '        If NoGroundbreaking.Equals(inStallUseStatus) Then
    '            '着工指示(00：着工指示待ち)
    '            If PartsPreparationWaiting.Equals(inPartsPreparationWaitType) Then
    '                '部品準備待ちフラグ(0：部品準備待ち)
    '                statusLeft = StatusCodeLeft108                          '★108：着工指示待ち/部品準備待ち

    '            ElseIf PartsInPreparation.Equals(inPartsPreparationWaitType) Then
    '                '部品準備待ちフラグ(1：部品準備中)
    '                statusLeft = StatusCodeLeft109                          '★109：着工指示待ち/部品準備中

    '            ElseIf PartsPreparationFinish.Equals(inPartsPreparationWaitType) Then
    '                '部品準備待ちフラグ(2：部品準備済み)
    '                statusLeft = StatusCodeLeft110                          '★110：着工指示待ち/部品準備済み

    '            End If
    '        Else
    '            '着工指示(00：着工指示待ち以外)
    '            If PartsPreparationWaiting.Equals(inPartsPreparationWaitType) Then
    '                '部品準備待ちフラグ(0：部品準備待ち)
    '                statusLeft = StatusCodeLeft111                          '★111：着工指示済み/部品準備待ち

    '            ElseIf PartsInPreparation.Equals(inPartsPreparationWaitType) Then
    '                '部品準備待ちフラグ(1：部品準備中)
    '                statusLeft = StatusCodeLeft112                          '★112：着工指示済み/部品準備中

    '            ElseIf PartsPreparationFinish.Equals(inPartsPreparationWaitType) Then
    '                '部品準備待ちフラグ(2：部品準備済み)
    '                statusLeft = StatusCodeLeft113                          '★113：作業開始待ち
    '            End If
    '        End If
    '    Else
    '        '枝番(0：親の連番以外)
    '        '追加作業ステータスの情報を取得
    '        Dim drList As IC3800804AddRepairStatusDataTableRow() = _
    '            (From dr As IC3800804AddRepairStatusDataTableRow In dtAddRepairStatus _
    '             Where dr.SRVADDSEQ = inSequenceNo.ToString(CultureInfo.CurrentCulture)).ToArray

    '        If 0 < drList.Length Then
    '            If ControllerRecognitionWaiting.Equals(drList(0).STATUS) Then
    '                '追加作業ステータス(2：CT承認待ち)
    '                statusLeft = StatusCodeLeft131                          '★131：追加作業承認待ち

    '            Else
    '                If NoGroundbreaking.Equals(inStallUseStatus) Then
    '                    '着工指示(00：着工指示待ち)
    '                    statusLeft = StatusCodeLeft107                      '★107：着工指示待ち

    '                Else
    '                    '着工指示(00：着工指示待ち以外)
    '                    statusLeft = StatusCodeLeft113                      '★113：作業開始待ち

    '                End If
    '            End If
    '        End If
    '    End If

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} OUT:RETURN = {2}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , statusLeft))
    '    Return statusLeft
    'End Function

    ' ''' <summary>
    ' ''' SMBステータスコード(左側)取得
    ' ''' ※R/Oステータス(2：整備中)の場合
    ' ''' </summary>
    ' ''' <param name="inWorkStartDate">実績開始日時</param>
    ' ''' <param name="inWorkEndDate">実績終了日時</param>
    ' ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ' ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、2：部品準備済み、3：部品不要)</param>
    ' ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み)</param>
    ' ''' <param name="inStallUseStatus">ストール利用ステータス(00：着工指示待ち、01：作業開始待ち)</param>
    ' ''' <param name="dtAddRepairStatus">追加作業ステータス</param>
    ' ''' <param name="inSequenceNo">枝番</param>
    ' ''' <returns>ステータスコード(左側)</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ' ''' </history>
    'Private Function GetSMBStatusLeftROMaintenance(ByVal inWorkStartDate As Date, _
    '                                               ByVal inWorkEndDate As Date, _
    '                                               ByVal inStopType As String, _
    '                                               ByVal inPartsPreparationWaitType As String, _
    '                                               ByVal inCompleteExaminationType As String, _
    '                                               ByVal inStallUseStatus As String, _
    '                                               ByVal dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable, _
    '                                               ByVal inSequenceNo As Long) As String
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} " _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '              , inWorkStartDate, inWorkEndDate, inStopType _
    '              , inPartsPreparationWaitType, inCompleteExaminationType, inStallUseStatus _
    '              , dtAddRepairStatus, inSequenceNo))

    '    Dim statusLeft As String = String.Empty

    '    If inWorkStartDate = Date.MinValue Then
    '        '実績開始日時(データ無)
    '        If inSequenceNo = 0 Then
    '            '枝番(0：親の連番)
    '            If NoGroundbreaking.Equals(inStallUseStatus) Then
    '                '着工指示(00：着工指示待ち)
    '                If PartsPreparationFinish.Equals(inPartsPreparationWaitType) Then
    '                    '部品準備待ちフラグ(2：部品準備済み)
    '                    statusLeft = StatusCodeLeft110                      '★110：着工指示待ち/部品準備済み

    '                ElseIf PartsPreparationNeedlessness.Equals(inPartsPreparationWaitType) Then
    '                    '部品準備待ちフラグ(3：部品準備不要)
    '                    statusLeft = StatusCodeLeft107                      '★107：着工指示待ち

    '                End If
    '            Else
    '                '着工指示(00：着工指示待ち以外)
    '                statusLeft = StatusCodeLeft113                          '★113：作業開始待ち

    '            End If
    '        Else
    '            '枝番(0：親の連番以外)
    '            '追加作業ステータスの情報を取得
    '            Dim drList As IC3800804AddRepairStatusDataTableRow() = _
    '                (From dr As IC3800804AddRepairStatusDataTableRow In dtAddRepairStatus _
    '                 Where dr.SRVADDSEQ = inSequenceNo.ToString(CultureInfo.CurrentCulture)).ToArray

    '            If 0 < drList.Length Then
    '                If ControllerRecognitionWaiting.Equals(drList(0).STATUS) Then
    '                    '追加作業ステータス(2：CT承認待ち)
    '                    statusLeft = StatusCodeLeft131                      '★131：追加作業承認待ち

    '                Else
    '                    If NoGroundbreaking.Equals(inStallUseStatus) Then
    '                        '着工指示(00：着工指示待ち)
    '                        statusLeft = StatusCodeLeft107                  '★107：着工指示待ち

    '                    Else
    '                        '着工指示(00：着工指示待ち以外)
    '                        statusLeft = StatusCodeLeft113                  '★113：作業開始待ち

    '                    End If
    '                End If
    '            End If
    '        End If
    '    Else
    '        '実績開始日時(データ有)
    '        If Discontinuation.Equals(inStopType) Then
    '            '中断有無(1：有)
    '            statusLeft = StatusCodeLeft115                              '★115：中断中

    '        ElseIf NoDiscontinuation.Equals(inStopType) Then
    '            '中断有無(0：無)
    '            If inWorkEndDate = Date.MinValue Then
    '                '実績終了日時(データ無)
    '                statusLeft = StatusCodeLeft114                          '★114：作業中

    '            Else
    '                If CompleteExamination.Equals(inCompleteExaminationType) Then
    '                    '完成検査フラグ(1：有)
    '                    statusLeft = StatusCodeLeft126                      '★126：完成検査承認待ち

    '                Else
    '                    '完成検査フラグ(1：有以外)
    '                    statusLeft = StatusCodeLeft130                      '★130：作業完了

    '                End If
    '            End If
    '        End If
    '    End If

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} OUT:RETURN = {2}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , statusLeft))
    '    Return statusLeft
    'End Function

    ' ''' <summary>
    ' ''' SMBステータスコード(左側)取得
    ' ''' ※R/Oステータス(7：検査完了)の場合
    ' ''' </summary>
    ' ''' <param name="inWorkStartDate">実績開始日時</param>
    ' ''' <param name="inWorkEndDate">実績終了日時</param>
    ' ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ' ''' <param name="inWashType">洗車有無(0：無、1：有)</param>
    ' ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、2：部品準備済み、3：部品不要)</param>
    ' ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み)</param>
    ' ''' <param name="inStallUseStatus">ストール利用ステータス(00：着工指示待ち、01：作業開始待ち)</param>
    ' ''' <param name="inResultWashStart">洗車開始実績日時</param>
    ' ''' <param name="inResultWashEnd">洗車終了実績日時</param>
    ' ''' <param name="dtAddRepairStatus">追加作業ステータス</param>
    ' ''' <param name="inSequenceNo">枝番</param>
    ' ''' <returns>ステータスコード(左側)</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ' ''' </history>
    'Private Function GetSMBStatusLeftROFinInspection(ByVal inWorkStartDate As Date, _
    '                                                 ByVal inWorkEndDate As Date, _
    '                                                 ByVal inStopType As String, _
    '                                                 ByVal inWashType As String, _
    '                                                 ByVal inPartsPreparationWaitType As String, _
    '                                                 ByVal inCompleteExaminationType As String, _
    '                                                 ByVal inStallUseStatus As String, _
    '                                                 ByVal inResultWashStart As String, _
    '                                                 ByVal inResultWashEnd As String, _
    '                                                 ByVal dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable, _
    '                                                 ByVal inSequenceNo As Long) As String
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11} P11:{12} " _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '              , inWorkStartDate, inWorkEndDate, inStopType, inWashType _
    '              , inPartsPreparationWaitType, inCompleteExaminationType, inStallUseStatus _
    '              , inResultWashStart, inResultWashEnd, dtAddRepairStatus, inSequenceNo))

    '    Dim statusLeft As String = String.Empty

    '    If Not IsNothing(dtAddRepairStatus) OrElse 0 < dtAddRepairStatus.Count Then
    '        '追加作業ありの場合
    '        Dim rowAddList As IC3800804AddRepairStatusDataTableRow() = _
    '            (From col In dtAddRepairStatus Where col.STATUS <> "9" Select col).ToArray

    '        If 0 = rowAddList.Count Then
    '            '全ての作業が完了している場合
    '            If NoWashFlag.Equals(inWashType) Then
    '                '洗車有無(0：無)
    '                statusLeft = StatusCodeLeft117                              '★117：納車準備待ち

    '            ElseIf WashFlag.Equals(inWashType) Then
    '                '洗車有無(1：有)
    '                If String.IsNullOrEmpty(inResultWashStart) Then
    '                    '洗車開始実績日時(データ無)
    '                    statusLeft = StatusCodeLeft118                          '★118：洗車待ち/納車準備待ち

    '                ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
    '                       String.IsNullOrEmpty(inResultWashEnd) Then
    '                    '洗車開始実績日時(データ有)、洗車終了実績日時(データ無)
    '                    statusLeft = StatusCodeLeft119                          '★119：洗車中/納車準備待ち

    '                ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
    '                       Not (String.IsNullOrEmpty(inResultWashEnd)) Then
    '                    '洗車開始実績日時(データ有)、洗車終了実績日時(データ有)
    '                    statusLeft = StatusCodeLeft117                          '★117：納車準備待ち

    '                End If
    '            End If
    '        Else
    '            '1つでも完了していない作業がある場合
    '            statusLeft = GetSMBStatusLeftROMaintenance(inWorkStartDate, _
    '                                                       inWorkEndDate, _
    '                                                       inStopType, _
    '                                                       inPartsPreparationWaitType, _
    '                                                       inCompleteExaminationType, _
    '                                                       inStallUseStatus, _
    '                                                       dtAddRepairStatus, _
    '                                                       inSequenceNo)
    '        End If
    '    Else
    '        '追加作業なしの場合
    '        If NoWashFlag.Equals(inWashType) Then
    '            '洗車有無(0：無)
    '            statusLeft = StatusCodeLeft117                                  '★117：納車準備待ち

    '        ElseIf WashFlag.Equals(inWashType) Then
    '            '洗車有無(1：有)
    '            If String.IsNullOrEmpty(inResultWashStart) Then
    '                '洗車開始実績日時(データ無)
    '                statusLeft = StatusCodeLeft118                              '★118：洗車待ち/納車準備待ち

    '            ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
    '                   String.IsNullOrEmpty(inResultWashEnd) Then
    '                '洗車開始実績日時(データ有)、洗車終了実績日時(データ無)
    '                statusLeft = StatusCodeLeft119                              '★119：洗車中/納車準備待ち

    '            ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
    '                   Not (String.IsNullOrEmpty(inResultWashEnd)) Then
    '                '洗車開始実績日時(データ有)、洗車終了実績日時(データ有)
    '                statusLeft = StatusCodeLeft117                              '★117：納車準備待ち

    '            End If
    '        End If
    '    End If

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} OUT:RETURN = {2}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , statusLeft))
    '    Return statusLeft
    'End Function

    ' ''' <summary>
    ' ''' SMBステータスコード(左側)取得
    ' ''' ※R/Oステータス(3：売上済 or 6：整備完了 or 8：納車完了)
    ' ''' </summary>
    ' ''' <param name="inWashType">洗車有無(0：無、1：有)</param>
    ' ''' <param name="inResultWashStart">洗車開始実績日時</param>
    ' ''' <param name="inResultWashEnd">洗車終了実績日時</param>
    ' ''' <param name="inDeliveryDate">実績納車日時</param>
    ' ''' <returns>ステータスコード(左側)</returns>
    ' ''' <remarks></remarks>
    ' ''' <history>
    ' ''' 2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ' ''' </history>
    'Private Function GetSMBStatusLeftROSalesMaintenance(ByVal inWashType As String, _
    '                                                    ByVal inResultWashStart As String, _
    '                                                    ByVal inResultWashEnd As String, _
    '                                                    ByVal inDeliveryDate As Date) As String
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} " _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '              , inWashType, inResultWashStart, inResultWashEnd, inDeliveryDate.ToString(CultureInfo.CurrentCulture)))

    '    Dim statusLeft As String = String.Empty

    '    If NoWashFlag.Equals(inWashType) Then
    '        '洗車有無(0：無)
    '        If inDeliveryDate = Date.MinValue Then
    '            '実績納車日時(データ無)
    '            statusLeft = StatusCodeLeft121                              '★121：納車待ち
    '        Else
    '            '実績納車日時(データ有)
    '            statusLeft = StatusCodeLeft134                              '★134：納車完了
    '        End If

    '    ElseIf WashFlag.Equals(inWashType) Then
    '        '洗車有無(1：有)
    '        If String.IsNullOrEmpty(inResultWashStart) Then
    '            '洗車開始実績日時(データ無)
    '            statusLeft = StatusCodeLeft132                              '★132：洗車待ち/納車準備済み

    '        ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
    '               String.IsNullOrEmpty(inResultWashEnd) Then
    '            '洗車開始実績日時(データ有)、洗車終了実績日時(データ無)
    '            statusLeft = StatusCodeLeft133                              '★133：洗車中/納車準備済み

    '        ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
    '               Not (String.IsNullOrEmpty(inResultWashEnd)) Then
    '            '洗車開始実績日時(データ有)、洗車終了実績日時(データ有)
    '            If inDeliveryDate = Date.MinValue Then
    '                '実績納車日時(データ無)
    '                statusLeft = StatusCodeLeft121                          '★121：納車待ち
    '            Else
    '                '実績納車日時(データ有)
    '                statusLeft = StatusCodeLeft134                          '★134：納車完了
    '            End If

    '        End If
    '    End If

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} OUT:RETURN = {2}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , statusLeft))
    '    Return statusLeft
    'End Function

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' サービス入庫ID又は作業内容IDを取得する処理
    ''' </summary>
    ''' <param name="JobDetailId">作業内容ID</param>
    ''' <returns>作業内容ID</returns>
    ''' <remarks></remarks>
    Private Function GetServiceInIdOrJobDetailId(ByVal inServiceInId As Decimal, _
                                                 ByVal JobDetailId As Decimal) As Decimal
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inServiceInId.ToString(CultureInfo.CurrentCulture) _
                  , JobDetailId.ToString(CultureInfo.CurrentCulture)))

        '戻り値
        Dim returnId As Decimal = -1

        Using da As New SMBCommonClassDataSetTableAdapters.SMBCommonClassTableAdapter
            'サービス入庫IDと作業内容ID(最小値)を取得する
            Dim dt As SMBCommonClassDataSet.ServiceinIdJobDetailMinIdDataTable = _
            da.GetServiceinIdJobDetailMinId(inServiceInId, JobDetailId)

            'データ確認
            If dt.Count = 0 Then
                '取得できなかった場合
                'エラーログを出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} ERROR:{2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , ReturnCode.ErrNoCases))

            Else
                '取得できた場合
                '引数のデータ確認
                If Not (IsNothing(inServiceInId)) AndAlso inServiceInId > 0 Then
                    'サービス入庫が存在する場合
                    '作業内容IDを戻り値に設定する
                    returnId = dt(0).JOB_DTL_ID_MIN

                ElseIf Not (IsNothing(JobDetailId)) AndAlso JobDetailId > 0 Then
                    '作業内容IDが存在する場合
                    'サービス入庫IDを戻り値に設定する
                    returnId = dt(0).SVCIN_ID

                End If
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnId))
        Return returnId
    End Function

    '2013/06/03 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' ステータスコード(左側)取得
    ''' ※RO番号が存在する場合
    ''' </summary>
    ''' <param name="inWorkStartType">作業開始有無(0：無、1：有)</param>
    ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ''' <param name="inWashType">洗車有無(0：無、1：有)</param>
    ''' <param name="inOrderStatus">R/Oステータス</param>
    ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、2：部品準備済み、3：部品不要)</param>
    ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み)</param>
    ''' <param name="inAddWorkStatus">追加作業ステータス</param>
    ''' <param name="inInstruct">着工指示区分(0：未着工、2：着工準備)</param>
    ''' <param name="inResultWashStart">洗車開始実績日時</param>
    ''' <param name="inResultWashEnd">洗車終了実績日時</param>
    ''' <param name="inWorkEndType">作業終了有無(0：作業中、1：作業終了)</param>
    ''' <param name="inServiceStatus">サービスステータス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Function GetStatusLeftStallROExist(ByVal inWorkStartType As String, _
                                       ByVal inStopType As String, _
                                       ByVal inWashType As String, _
                                       ByVal inOrderStatus As String, _
                                       ByVal inPartsPreparationWaitType As String, _
                                       ByVal inCompleteExaminationType As String, _
                                       ByVal inAddWorkStatus As String, _
                                       ByVal inInstruct As String, _
                                       ByVal inResultWashStart As String, _
                                       ByVal inResultWashEnd As String, _
                                       ByVal inWorkEndType As String, _
                                       ByVal inServiceStatus As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11} P11:{12} P12:{13} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inWorkStartType, inStopType _
                  , inWashType, inOrderStatus _
                  , inPartsPreparationWaitType, inCompleteExaminationType, inAddWorkStatus _
                  , inInstruct, inResultWashStart, inResultWashEnd, inWorkEndType, inServiceStatus))

        Dim statusLeft As String = String.Empty

        'R/O有無(1：有)
        Select Case inOrderStatus

            Case RONoneReissuing,
                 ROReissuingSA,
                 ROReissuingTC,
                 ROWaitRecognitionFM,
                 ROPartsDemoEstimate,
                 ROPartsMasterEstimate,
                 ROWaitCustomerRecognition
                '「00:R/O未起票」「10:SA起票中」「15:TC起票中」「20:FM承認待ち」
                '「25:部品仮見積中」「30:部品本見積中」「40:顧客承認待ち」
                statusLeft = StatusCodeLeft106                                  '★106：R/O作成中

            Case ROWaitStruct
                '「50:着工指示待ち」
                statusLeft = Me.GetStatusLeftParts(inPartsPreparationWaitType, inInstruct)

            Case ROWaitWorkStart,
                 ROWorking,
                 ROInspectRequest,
                 ROInspectFinish
                '「55:作業開始待ち」「60:作業中」
                '「65:完成検査依頼中」「70:完成検査完了」
                statusLeft = Me.GetStatusLeftROMaintenance(inWorkStartType, _
                                                           inStopType, _
                                                           inPartsPreparationWaitType, _
                                                           inCompleteExaminationType, _
                                                           inInstruct)

            Case ROWaitDeliveryPreparation
                '「80:納車準備待ち」
                statusLeft = Me.GetStatusLeftROFinInspection(inWorkStartType, _
                                                             inStopType, _
                                                             inWashType, _
                                                             inPartsPreparationWaitType, _
                                                             inCompleteExaminationType, _
                                                             inAddWorkStatus, _
                                                             inResultWashStart, _
                                                             inResultWashEnd, _
                                                             inWorkEndType, _
                                                             inServiceStatus)

            Case ROWorkingDelivery
                '「85:納車作業中(清算中)」
                statusLeft = Me.GetStatusLeftDeliveryWorking(inWashType, _
                                                             inResultWashStart, _
                                                             inResultWashEnd, _
                                                             inServiceStatus)

        End Select

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusLeft))
        Return statusLeft
    End Function

    ''' <summary>
    ''' ステータスコード(左側)取得
    ''' ※R/Oステータス(85:納車作業中(清算中))の場合
    ''' </summary>
    ''' <param name="inWashType">洗車有無(0：無、1：有)</param>
    ''' <param name="inResultWashStart">洗車開始実績日時</param>
    ''' <param name="inResultWashEnd">洗車終了実績日時</param>
    ''' <param name="inServiceStatus">サービスステータス</param>
    ''' <returns>ステータスコード(左側)</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetStatusLeftDeliveryWorking(ByVal inWashType As String, _
                                                  ByVal inResultWashStart As String, _
                                                  ByVal inResultWashEnd As String, _
                                                  ByVal inServiceStatus As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inWashType, inResultWashStart, inResultWashEnd, inServiceStatus))

        Dim statusLeft As String = String.Empty

        If NoWashFlag.Equals(inWashType) Then
            '洗車有無(0：無)
            statusLeft = StatusCodeLeft121                                  '★121：納車待ち

        ElseIf WashFlag.Equals(inWashType) Then
            '洗車有無(1：有)

            If ServiceStatusDropOff.Equals(inServiceStatus) OrElse
               ServiceStatusWaitDalivery.Equals(inServiceStatus) Then
                'サービスステータス（11：預かり中）、サービスステータス（12：納車待ち）
                statusLeft = StatusCodeLeft121                              '★121：納車待ち

            Else
                '上記以外
                If String.IsNullOrEmpty(inResultWashStart) Then
                    '洗車開始実績日時(データ無)
                    statusLeft = StatusCodeLeft132                          '★132：洗車待ち/納車準備済み

                ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
                       String.IsNullOrEmpty(inResultWashEnd) Then
                    '洗車開始実績日時(データ有)、洗車終了実績日時(データ無)
                    statusLeft = StatusCodeLeft133                          '★133：洗車中/納車準備済み

                ElseIf Not (String.IsNullOrEmpty(inResultWashStart)) AndAlso _
                       Not (String.IsNullOrEmpty(inResultWashEnd)) Then
                    '洗車開始実績日時(データ有)、洗車終了実績日時(データ有)
                    statusLeft = StatusCodeLeft121                          '★121：納車待ち

                End If

            End If

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusLeft))
        Return statusLeft
    End Function

    ''' <summary>
    ''' SMBステータスコード(左側)取得
    ''' ※ストールチップの場合
    ''' </summary>
    ''' <param name="inWorkStartDate">実績開始日時</param>
    ''' <param name="inStallUseStatus">ストール利用ステータス</param>
    ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、8：部品準備済み、NULL：部品不要)</param>
    ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ''' <param name="inWorkEndDate">実績終了日時</param>
    ''' <param name="inServiceinStatus">サービス入庫ステータス</param>
    ''' <param name="inInvoicePrintDate">清算書印刷日時</param>
    ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み、2：完成検査承認済み)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetSMBStatusLeftStall(ByVal inVisitSequence As Long, _
                                           ByVal inOrderType As String, _
                                           ByVal inOrderNo As String, _
                                           ByVal inWorkStartDate As Date, _
                                           ByVal inStallUseStatus As String, _
                                           ByVal inPartsPreparationWaitType As String, _
                                           ByVal inStopType As String, _
                                           ByVal inWorkEndDate As Date, _
                                           ByVal inServiceinStatus As String, _
                                           ByVal inInvoicePrintDate As Date, _
                                           ByVal inCompleteExaminationType As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11} P11:{12} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inVisitSequence.ToString(CultureInfo.CurrentCulture) _
                  , inOrderType, inOrderNo, inWorkStartDate.ToString(CultureInfo.CurrentCulture), inStallUseStatus _
                  , inPartsPreparationWaitType, inStopType, inWorkEndDate.ToString(CultureInfo.CurrentCulture) _
                  , inCompleteExaminationType, inServiceinStatus, inInvoicePrintDate.ToString(CultureInfo.CurrentCulture)))

        Dim statusLeft As String = String.Empty

        If inVisitSequence <= 0 Then
            '来店実績連番が0以下の場合
            statusLeft = StatusCodeLeft127                                          '★127：仮R/O作成待ち

        ElseIf 0 < inVisitSequence Then
            '来店実績連番が0より大きいの場合
            If RepairOrderTypeNone.Equals(inOrderType) Then
                'R/O情報有無(0：無)
                statusLeft = StatusCodeLeft105                                      '★105：R/O作成待ち

            ElseIf RepairOrderTypeExist.Equals(inOrderType) Then
                'R/O情報有無(1：有)
                If String.IsNullOrEmpty(inOrderNo) Then
                    'R/O番号(データ無)
                    statusLeft = StatusCodeLeft106                                  '★106：R/O作成中

                Else
                    'R/O番号(データ有)
                    statusLeft = Me.GetSMBStatusLeftStallROExist(inWorkStartDate, _
                                                                 inStallUseStatus, _
                                                                 inPartsPreparationWaitType, _
                                                                 inStopType, _
                                                                 inWorkEndDate, _
                                                                 inServiceinStatus, _
                                                                 inInvoicePrintDate, _
                                                                 inCompleteExaminationType)

                End If

            End If

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusLeft))
        Return statusLeft
    End Function

    ''' <summary>
    ''' SMBステータスコード(左側)取得
    ''' ※RO番号が存在する場合
    ''' </summary>
    ''' <param name="inWorkStartDate">実績開始日時</param>
    ''' <param name="inStallUseStatus">ストール利用ステータス</param>
    ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、8：部品準備済み、NULL：部品不要)</param>
    ''' <param name="inStopType">中断有無(0：無、1：有)</param>
    ''' <param name="inWorkEndDate">実績終了日時</param>
    ''' <param name="inServiceinStatus">サービス入庫ステータス</param>
    ''' <param name="inInvoicePrintDate">清算書印刷日時</param>
    ''' <param name="inCompleteExaminationType">完成検査フラグ(0：完成検査依頼未、1：完成検査依頼済み、2：完成検査承認済み)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetSMBStatusLeftStallROExist(ByVal inWorkStartDate As Date, _
                                                  ByVal inStallUseStatus As String, _
                                                  ByVal inPartsPreparationWaitType As String, _
                                                  ByVal inStopType As String, _
                                                  ByVal inWorkEndDate As Date, _
                                                  ByVal inServiceinStatus As String, _
                                                  ByVal inInvoicePrintDate As Date, _
                                                  ByVal inCompleteExaminationType As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inWorkStartDate.ToString(CultureInfo.CurrentCulture), inStallUseStatus, inPartsPreparationWaitType _
                  , inStopType, inWorkEndDate.ToString(CultureInfo.CurrentCulture), inCompleteExaminationType _
                  , inServiceinStatus, inInvoicePrintDate.ToString(CultureInfo.CurrentCulture)))

        Dim statusLeft As String = String.Empty

        If inWorkStartDate = Date.MinValue Then
            '作業開始前の場合
            If NoGroundbreaking.Equals(inStallUseStatus) Then
                '着工指示前の場合
                If PartsPreparationWaiting.Equals(inPartsPreparationWaitType) Then
                    '「0：部品準備待ち」の場合
                    statusLeft = StatusCodeLeft108                      '★108：着工指示待ち/部品準備待ち

                ElseIf PartsInPreparation.Equals(inPartsPreparationWaitType) Then
                    '「1：部品準備中」の場合
                    statusLeft = StatusCodeLeft109                      '★109：着工指示待ち/部品準備中

                ElseIf PartsPreparationFinish.Equals(inPartsPreparationWaitType) Then
                    '「8：部品準備済み」の場合
                    statusLeft = StatusCodeLeft110                      '★110：着工指示待ち/部品準備済み

                Else
                    '上記以外の場合
                    statusLeft = StatusCodeLeft107                      '★107：着工指示待ち

                End If

            Else
                '着工指示済みの場合
                If PartsPreparationWaiting.Equals(inPartsPreparationWaitType) Then
                    '「0：部品準備待ち」の場合
                    statusLeft = StatusCodeLeft111                      '★111：着工指示済み/部品準備待ち

                ElseIf PartsInPreparation.Equals(inPartsPreparationWaitType) Then
                    '「1：部品準備中」の場合
                    statusLeft = StatusCodeLeft112                      '★112：着工指示済み/部品準備中

                Else
                    '上記以外の場合
                    statusLeft = StatusCodeLeft113                      '★113：作業開始待ち

                End If

            End If

        Else
            '作業開始済みの場合
            If Discontinuation.Equals(inStopType) Then
                '「1：中断中」の場合
                statusLeft = StatusCodeLeft115                          '★115：中断中

            ElseIf NoDiscontinuation.Equals(inStopType) Then
                '「0：中断無」の場合
                If inWorkEndDate = Date.MinValue Then
                    '作業終了していない場合
                    statusLeft = StatusCodeLeft114                      '★114：作業中

                Else
                    '作業終了している場合
                    If ServiceStatusWaitCarWash.Equals(inServiceinStatus) Then
                        '「07：洗車待ち」の場合
                        If inInvoicePrintDate = Date.MinValue Then
                            '印刷前の場合
                            statusLeft = StatusCodeLeft118              '★118：洗車待ち/納車準備待ち

                        Else
                            '印刷済みの場合
                            statusLeft = StatusCodeLeft132              '★132：洗車待ち/納車準備済み

                        End If

                    ElseIf ServiceStatusCarWashing.Equals(inServiceinStatus) Then
                        '「08：洗車中」の場合
                        If inInvoicePrintDate = Date.MinValue Then
                            '印刷前の場合
                            statusLeft = StatusCodeLeft119              '★119：洗車中/納車準備待ち

                        Else
                            '印刷済みの場合
                            statusLeft = StatusCodeLeft133              '★133：洗車中/納車準備済み

                        End If

                    ElseIf ServiceStatusDropOff.Equals(inServiceinStatus) OrElse _
                           ServiceStatusWaitDalivery.Equals(inServiceinStatus) Then
                        '「11：預かり中」「12：納車待ち」の場合
                        If inInvoicePrintDate = Date.MinValue Then
                            '印刷前の場合
                            statusLeft = StatusCodeLeft117              '★117：納車準備待ち

                        Else
                            '印刷済みの場合
                            statusLeft = StatusCodeLeft121              '★121：納車待ち

                        End If

                    ElseIf ServiceStatusFinishDelivery.Equals(inServiceinStatus) Then
                        '「13：納車済み」の場合
                        statusLeft = StatusCodeLeft134                  '★134：納車完了

                    Else
                        '上記以外の場合
                        If NoCompleteExamination.Equals(inCompleteExaminationType) Then
                            '完成検査前の場合
                            statusLeft = StatusCodeLeft130              '★130：作業完了

                        ElseIf RequestCompleteExamination.Equals(inCompleteExaminationType) Then
                            '完成検査待ちの場合
                            statusLeft = StatusCodeLeft126              '★126：完成検査承認待ち

                        ElseIf FinishCompleteExamination.Equals(inCompleteExaminationType) Then
                            '完成検査済みの場合
                            statusLeft = StatusCodeLeft138              '★138：完成検査承認済み

                        End If

                    End If

                End If

            End If

        End If

        Return statusLeft
    End Function

    ''' <summary>
    ''' SMBステータスコード(左側)取得
    ''' ※受付タブチップの場合
    ''' </summary>
    ''' <param name="inPartsPreparationWaitType">部品準備待ちフラグ(0：部品準備待ち、1：部品準備中、8：部品準備済み、NULL：部品不要)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetSMBStatusLeftReceptionist(ByVal inPartsPreparationWaitType As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inPartsPreparationWaitType))

        Dim statusLeft As String = String.Empty

        If PartsPreparationWaiting.Equals(inPartsPreparationWaitType) Then
            '「0：部品準備待ち」の場合
            statusLeft = StatusCodeLeft108                                          '★108：着工指示待ち/部品準備待ち

        ElseIf PartsInPreparation.Equals(inPartsPreparationWaitType) Then
            '「1：部品準備中」の場合
            statusLeft = StatusCodeLeft109                                          '★109：着工指示待ち/部品準備中

        ElseIf PartsPreparationFinish.Equals(inPartsPreparationWaitType) Then
            '「8：部品準備済み」の場合
            statusLeft = StatusCodeLeft110                                          '★110：着工指示待ち/部品準備済み

        Else
            '上記以外の場合
            statusLeft = StatusCodeLeft107                                          '★107：着工指示待ち

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusLeft))
        Return statusLeft
    End Function

    ''' <summary>
    ''' SMBステータスコード(左側)取得
    ''' ※洗車タブチップの場合
    ''' </summary>
    ''' <param name="inServiceinStatus">サービス入庫ステータス</param>
    ''' <param name="inInvoicePrintDate">清算書印刷日時</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetSMBStatusLeftCarWash(ByVal inServiceinStatus As String, _
                                             ByVal inInvoicePrintDate As Date) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} P2:{3} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inServiceinStatus, inInvoicePrintDate.ToString(CultureInfo.CurrentCulture)))

        Dim statusLeft As String = String.Empty

        If ServiceStatusWaitCarWash.Equals(inServiceinStatus) Then
            '「07：洗車待ち」の場合
            If inInvoicePrintDate = Date.MinValue Then
                '印刷前の場合
                statusLeft = StatusCodeLeft118                                  '★118：洗車待ち/納車準備待ち

            Else
                '印刷済みの場合
                statusLeft = StatusCodeLeft132                                  '★132：洗車待ち/納車準備済み

            End If

        ElseIf ServiceStatusCarWashing.Equals(inServiceinStatus) Then
            '「08：洗車中」の場合
            If inInvoicePrintDate = Date.MinValue Then
                '印刷前の場合
                statusLeft = StatusCodeLeft119                                  '★119：洗車中/納車準備待ち

            Else
                '印刷済みの場合
                statusLeft = StatusCodeLeft133                                  '★133：洗車中/納車準備済み

            End If

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusLeft))
        Return statusLeft
    End Function

    ''' <summary>
    ''' SMBステータスコード(左側)取得
    ''' ※納車待ちタブチップの場合
    ''' </summary>
    ''' <param name="inInvoicePrintDate">清算書印刷日時</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Private Function GetSMBStatusLeftWaitDelivery(ByVal inInvoicePrintDate As Date) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} P1:{2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inInvoicePrintDate.ToString(CultureInfo.CurrentCulture)))

        Dim statusLeft As String = String.Empty

        If inInvoicePrintDate = Date.MinValue Then
            '印刷前の場合
            statusLeft = StatusCodeLeft117                                      '★117：納車準備待ち

        Else
            '印刷済みの場合
            statusLeft = StatusCodeLeft121                                      '★121：納車待ち

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURN = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , statusLeft))
        Return statusLeft
    End Function

    '2014/01/11 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

#End Region

    ''' <summary>
    ''' IDisposable.Dispoase
    ''' </summary>
    ''' <remarks></remarks>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            DTStallTime.Dispose()
            DTNonWorkDays.Dispose()
            DTStandardLTList.Dispose()
            DTStallTime = Nothing
            DTNonWorkDays = Nothing
            DTStandardLTList = Nothing
        End If
    End Sub

End Class