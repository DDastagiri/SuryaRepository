'------------------------------------------------------------------------------
'SC3150102BusinessLogic.vb
'------------------------------------------------------------------------------
'機能：TCメインメニュー_R/O情報タブ
'補足：
'作成：2012/01/30 KN 渡辺
'更新：2012/03/12 KN 西田 【SERVICE_1】課題管理番号-BMTS_0310_YW_03の不具合修正 作業進捗エリアのR/ONoに枝番表示
'更新：2012/03/13 KN 上田 追加承認チップ部品準備完了情報を考慮するように修正
'更新：2012/03/14 KN 西田 作業項目の単価、合計項目が参照しているデータ変更
'更新：2012/03/14 KN 西田 【SERVICE_1】課題管理番号-KN_0307_HH_1の不具合修正 B/O項目フラグ変更
'更新：2012/03/27 KN 森下【SERVICE_1】システムテストの不具合修正No82 作業開始チップの部品エリアのグレーアウト誤り
'更新：2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加
'更新：2012/06/01 KN 西田 STEP1 重要課題対応
'更新：2012/06/06 KN 彭健 コード分析対応
'更新：2012/08/14 KN 彭健 SAストール予約受付機能開発（No.27カゴナンバー表示）
'更新：2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）
'更新：2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新：2013/06/17 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発
'更新：2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発
'更新：2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成
'更新：2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新：2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
'更新：2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
'更新：2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
'更新：2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新：
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Core
'2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801001
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.BizLogic.IC3801110
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801110
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.BizLogic.IC3801113
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801113
'2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

'2012/03/01 子チップ作業・部品情報取得対応 上田 Start
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801006
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801006
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801007
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801007
'2012/03/01 子チップ作業・部品情報取得対応 上田 End
Imports System.Text
'2012/03/12 nishida 【SERVICE_1】課題管理番号-BMTS_0310_YW_03の不具合修正 作業進捗エリアのR/ONoに枝番表示 START
'Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800804
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804
'2012/03/12 nishida 【SERVICE_1】課題管理番号-BMTS_0310_YW_03の不具合修正 作業進捗エリアのR/ONoに枝番表示 END

'2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 START
'2012/04/09 KN 西田【SERVICE_1】プレユーザーテスト No.14 当日処理の開始判定追加 END

'Imports Toyota.eCRB.iCROP.BizLogic.IC3810801
'Imports Toyota.eCRB.iCROP.DataAccess.IC3810801
Imports System.Globalization

'2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
'Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801104
'Imports Toyota.eCRB.iCROP.DataAccess.IC3801801
'Imports Toyota.eCRB.iCROP.BizLogic.IC3801801
'2013/12/12　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

'2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）START
'Imports Toyota.eCRB.DMSLinkage.OrderHistory.DataAccess.IC3801601
'Imports Toyota.eCRB.DMSLinkage.OrderHistory.BizLogic.IC3801601
'2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75）END

'2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
Imports Toyota.eCRB.Technician.MainMenu
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
'2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

'2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic.ServiceCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

'2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
Public Class SC3150102BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"
    '2012/03/13 上田 追加承認チップ部品準備完了情報を考慮するように修正 START
    ''' <summary>
    ''' 部品準備未完了状態
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PARTS_REPARE_UNPREPARED As String = "0"
    ''' <summary>
    ''' 部品準備完了状態
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PARTS_REPARE_PREPARED As String = "1"
    ''' <summary>
    ''' TACTステータス_TC作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TACT_STATUS_TC_WORK_START_WAIT As String = "7"
    '2012/03/27 KN 森下【SERVICE_1】システムテストの不具合修正No82 作業開始チップの部品エリアのグレーアウト誤り START
    ''' <summary>
    ''' TACTステータス_整備中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TACT_STATUS_WORKING As String = "8"
    ''' <summary>
    ''' TACTステータス_完成検査完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TACT_STATUS_FINISHED As String = "9"

    '2012/03/27 KN 森下【SERVICE_1】システムテストの不具合修正No82 作業開始チップの部品エリアのグレーアウト誤り END
    '2012/03/13 上田 追加承認チップ部品準備完了情報を考慮するように修正 END

    ' 2012/06/01 KN 西田 STEP1 重要課題対応 START
    ''' <summary>
    ''' 作業連番
    ''' 0：未計画/親作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORKSEQ_NOPLAN_PARENT As String = "0"

    ' 2012/06/01 KN 西田 STEP1 重要課題対応 END


    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "SC3150102"
    ''' <summary>
    ''' 作業開始フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const workStartFlg As String = "0"
    ''' <summary>
    ''' 作業終了フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const workFinishFlg As String = "1"
    ''' <summary>
    ''' 作業中断フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const workStopFlg As String = "2"
    ''' <summary>
    ''' 休憩時間データの格納文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_DATA_STRING_LENGTH = 4
    ''' <summary>
    ''' 休憩時間データの時間情報開始位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_HOUR_INDEX = 0
    ''' <summary>
    ''' 休憩時間データの時間情報文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_HOUR_LENGTH = 2
    ''' <summary>
    ''' 休憩時間データの分情報開始位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_MINUTE_INDEX = 2
    ''' <summary>
    ''' 休憩時間データの分情報文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BREAK_TIME_MINUTE_LENGTH = 2
    ''' <summary>
    ''' 休憩であることを示す、ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_BLEAK As Integer = 99
    ''' <summary>
    ''' 使用不可であることを示す、ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATUS_UNAVAILABLE = 3
    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd HH:mm"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDDHHMM As Integer = 2
    ''' <summary>
    ''' DateTimeFuncにて、"yyyyMMdd"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYYMMDD As Integer = 9
    ''' <summary>
    ''' DateTimeFuncにて、"yyyy/MM/dd"形式をコンバートするためのID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_CONVERT_ID_YYYY_MM_DD As Integer = 21
    ''' <summary>
    ''' オペレーションコードCT権限
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeCT As Integer = 55
    ''' <summary>
    ''' オペレーションコードChT
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeChT As Integer = 62
    ''' <summary>
    ''' オペレーションコードSA
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeSA As Integer = 9
    ''' <summary>
    ''' オペレーションコードPS
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodePS As Integer = 54
    ''' <summary>
    ''' オペレーションコードTC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationCodeTC As Integer = 14
    ''' <summary>
    '''日付最小値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MINDATE As String = "1900/01/01 0:00:00"
    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

    ''' <summary>
    ''' サービスステータス：洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStetus_WaitingWashing As String = "07"

    ''' <summary>
    ''' CWメインリフレッシュ関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CWRefreshFunction As String = "MainRefresh()"

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

#End Region

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

#Region "メンバー変数"
    ''' <summary>単独Job開始した後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushStartSingleJob As Boolean
    ''' <summary>単独Job終了した後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushFinishSingleJob As Boolean
    ''' <summary>単独Job中断した後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushStopSingleJob As Boolean

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>サブエリア更新Pushフラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushSubAreaRefresh As Boolean
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
#End Region

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

#Region "取得系処理"


    ''' <summary>
    ''' R/O基本情報の取得処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="repairOrderNumber">オーダーNo.</param>
    ''' <returns>R/O基本情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetRepairOrderBaseData(ByVal dealerCode As String, _
                                           ByVal branchCode As String, _
                                           ByVal repairOrderNumber As String, _
                                           ByVal workSeq As Integer) As SC3150102DataSet.SC3150102GetRoInfoDataTable


        'Public Function GetRepairOrderBaseData(ByVal dealerCode As String, ByVal repairOrderNumber As String) _
        '                                                        As IC3801001DataSet.IC3801001OrderCommDataTable

        'Logger.Info("GetRepairOrderBaseData Start param1:" + dealerCode + _
        '                                        " param2:" + repairOrderNumber)

        'Dim IC3801001BizLogic As IC3801001BusinessLogic = New IC3801001BusinessLogic
        'Dim dt As IC3801001DataSet.IC3801001OrderCommDataTable

        'If (String.IsNullOrEmpty(repairOrderNumber)) Then
        '    'dt = New IC3801001DataSet.IC3801001OrderCommDataTable
        '    dt = Nothing
        'Else
        '    'R/O基本情報の取得.
        '    dt = IC3801001BizLogic.GetROBaseInfoList(dealerCode, repairOrderNumber)

        '    '2012/03/03 日比野 ログ出力処理追加 START 
        '    OutPutIFLog(dt, "IC3801001BizLogic.GetROBaseInfoList")
        '    '2012/03/03 日比野 ログ出力処理追加 END
        'End If

        'Logger.Info("GetRepairOrderBaseData End")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} IN.DLR_CD:{2},RO_NUM:{3}, RO_SEQ:{4}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , dealerCode _
                   , repairOrderNumber _
                   , workSeq))

        'データテーブル宣言
        Dim dtRet As SC3150102DataSet.SC3150102GetRoInfoDataTable
        'アダプター宣言
        Using adapter As New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter

            'RO番号が空白の場合
            If (String.IsNullOrEmpty(repairOrderNumber)) Then
                dtRet = Nothing
            Else

                'RO基本情報取得
                dtRet = adapter.GetROInfo(dealerCode, branchCode, repairOrderNumber)

                OutPutIFLog(dtRet, "SC3150102DataSet.SC3150102GetRoInfo")
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dtRet

    End Function
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    '2012/06/01 KN 西田 STEP1 重要課題対応 START
    '2012/03/01 子チップ作業・部品情報取得対応 上田 Start

    ' ''' <summary>
    ' ''' 作業項目情報の取得処理
    ' ''' </summary>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="repairOrderNumber">オーダーNo.</param>
    ' ''' <param name="childNumber">子予約連番</param>
    ' ''' <returns>作業項目データ</returns>
    ' ''' <remarks></remarks>
    'Public Function GetServiceDetailData(ByVal dealerCode As String, _
    '                                     ByVal repairOrderNumber As String, _
    '                                     ByVal childNumber As String) As IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable
    ''' <summary>
    ''' 作業項目情報の取得処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="repairOrderNumber">オーダーNo.</param>
    ''' <param name="workSeq">作業連番</param>
    ''' <returns>作業項目データ</returns>
    ''' <remarks></remarks>
    Public Function GetServiceDetailData(ByVal dealerCode As String, _
                                         ByVal branchCode As String, _
                                         ByVal repairOrderNumber As String, _
                                         ByVal workSeq As Integer) As SC3150102DataSet.SC3150102OperationDetailInfoDataTable

        'Public Function GetServiceDetailData(ByVal dealerCode As String, _
        '                                ByVal repairOrderNumber As String, _
        '                                ByVal workSeq As Integer) As IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable

        Logger.Info("GetServiceDetailData Start param1:" + dealerCode + _
                                                " param2:" + repairOrderNumber + _
                                                " param3:" + workSeq.ToString(CultureInfo.CurrentCulture))

        Dim dtRet As SC3150102DataSet.SC3150102OperationDetailInfoDataTable
        'Dim dtRet As IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable

        If (String.IsNullOrEmpty(repairOrderNumber)) Then
            dtRet = Nothing
            '2012/03/03 日比野 ログ出力処理追加 START 
            Logger.Info("ServiceDetailData is Nothing")
            '2012/03/03 日比野 ログ出力処理追加 END
        Else
            'If WORKSEQ_NOPLAN_PARENT.Equals(workSeq.ToString(CultureInfo.CurrentCulture)) Then
            '関連チップがないチップ 又は、関連チップ(親チップ)
            Using adapter As New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter
                'Dim IC3801110BizLogic As IC3801110BusinessLogic = New IC3801110BusinessLogic

                '作業内容の取得
                dtRet = adapter.GetOperationDetailInfo(dealerCode, branchCode, repairOrderNumber)

            End Using

            '2012/03/03 日比野 ログ出力処理追加 START 
            'OutPutIFLog(dtRet, "IC3801110BizLogic.GetSrvDetailList")
            '2012/03/03 日比野 ログ出力処理追加 END

            'Else
            '    '関連チップ(子チップ)
            '    Dim IC3801006BizLogic As IC3801006BusinessLogic = New IC3801006BusinessLogic
            '    Dim dtChildInfomation As IC3801006DataSet.IC3801006ServiceDetailInfoDataTable

            '    'Dim addSeq As Integer = 0
            '    'Integer.TryParse(strWorkSeq, addSeq)

            '    '作業内容取得(子チップ)
            '    dtChildInfomation = IC3801006BizLogic.GetServiceDetailList(dealerCode, _
            '                                                               repairOrderNumber, _
            '                                                               workSeq)

            '    '2012/03/03 日比野 ログ出力処理追加 START 
            '    OutPutIFLog(dtChildInfomation, "IC3801006BizLogic.GetServiceDetailList")
            '    '2012/03/03 日比野 ログ出力処理追加 END

            '    Using dt As New IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable

            '        '戻り値データ用に変換する
            '        For i = 0 To dtChildInfomation.Rows.Count - 1

            '            Dim dr As IC3801110DataSet.IC3801110SrvDetailDataTableCommRow = dt.NewIC3801110SrvDetailDataTableCommRow
            '            Dim drChildInfomation As IC3801006DataSet.IC3801006ServiceDetailInfoRow = DirectCast(dtChildInfomation.Rows(i), IC3801006DataSet.IC3801006ServiceDetailInfoRow)

            '            With dr
            '                .DEALERCODE = drChildInfomation.DealerCode                  '販売店コード
            '                .BRNCD = drChildInfomation.BrnCd                            '店舗コード
            '                .ORDERNO = drChildInfomation.OrderNo                        '受注NO
            '                .SRVNAME = drChildInfomation.SrvName                        '整備名称
            '                .HRTYPE = drChildInfomation.HRType                          'HR区分
            '                .WORKHOURS = drChildInfomation.WorkHours                    '工数
            '                .SELLHOURRATE = drChildInfomation.SellHourRate              '技術料(単価)
            '                .SELLWORKPRICE = drChildInfomation.SellWorkPrice            '技術料

            '                '予約ID
            '                If (drChildInfomation.IsRezIDNull) Then
            '                    .rezid = 0
            '                Else
            '                    .rezid = CType(drChildInfomation.RezID, Long)
            '                End If

            '                '整備コード
            '                If (drChildInfomation.IsSrvCodeNull) Then
            '                    .SRVCODE = String.Empty
            '                Else
            '                    .SRVCODE = drChildInfomation.SrvCode
            '                End If

            '                '整備SEQ
            '                If (drChildInfomation.IsSrvSeqUeceNull) Then
            '                    .SRVSEQUECE = 0
            '                Else
            '                    .SRVSEQUECE = CType(drChildInfomation.SrvSeqUece, Integer)
            '                End If

            '                '作業Gコード
            '                If drChildInfomation.IsWorkByCodeNull Then
            '                    .WORKBYCODE = String.Empty
            '                Else
            '                    .WORKBYCODE = drChildInfomation.WorkByCode
            '                End If
            '            End With

            '            '行追加
            '            dt.Rows.Add(dr)
            '        Next

            '        dtRet = dt
            '    End Using
            'End If
        End If

        'Logger.Info("GetServiceDetailData End")
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} OUT" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtRet

    End Function

    ' 2012/06/01 KN 西田 STEP1 重要課題対応 END

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    ' 2012/06/01 KN 西田 STEP1 重要課題対応 START

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 部品詳細情報の取得処理
    ' ''' </summary>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="repairOrderNumber">オーダーNo.</param>
    ' ''' <param name="childNumber">子予約連番</param>
    ' ''' <returns>部品詳細情報</returns>
    ' ''' <remarks></remarks>
    'Public Function GetPartsDetailData(ByVal dealerCode As String, _
    '                                   ByVal repairOrderNumber As String, _
    '                                   ByVal childNumber As String) As IC3801113DataSet.IC3801113PartsDataTable

    ''' <summary>
    ''' 部品詳細情報の取得処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="repairOrderNumber">オーダーNo.</param>
    ''' <param name="workSeq">作業連番</param>
    ''' <returns>部品詳細情報</returns>
    ''' <remarks></remarks>
    Public Function GetPartsDetailData(ByVal dealerCode As String, _
                                       ByVal branchCode As String, _
                                       ByVal repairOrderNumber As String, _
                                       ByVal workSeq As ArrayList) As IC3802504DataSet.IC3802504PartsDetailDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} IN,DLR_CD:{2},BRN_CD,{3},RO_NUM{4},RO_SEQ_COUNT{5}" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name _
               , dealerCode _
               , branchCode _
               , repairOrderNumber _
               , workSeq.Count.ToString(CultureInfo.CurrentCulture())))

        'Public Function GetPartsDetailData(ByVal dealerCode As String, _
        '                              ByVal repairOrderNumber As String, _
        '                              ByVal workSeq As Integer) As IC3801113DataSet.IC3801113PartsDataTable

        'Logger.Info("GetPartsDetailData Start param1:" + dealerCode + _
        '                                    " param2:" + repairOrderNumber + _
        '                                    " param3:" + workSeq.ToString(CultureInfo.CurrentCulture))

        ''Dim dtRet As IC3801113DataSet.IC3801113PartsDataTable

        'If (String.IsNullOrEmpty(repairOrderNumber)) Then
        '    dtRet = Nothing
        '    '2012/03/03 日比野 ログ出力処理追加 START 
        '    Logger.Info("PartsDetailData is Nothing")
        '    '2012/03/03 日比野 ログ出力処理追加 END
        'Else
        '    If WORKSEQ_NOPLAN_PARENT.Equals(workSeq.ToString(CultureInfo.CurrentCulture)) Then
        '        '関連チップがないチップ 又は、関連チップ(親チップ)
        '        Dim IC3801113BizLogic As IC3801113BusinessLogic = New IC3801113BusinessLogic

        '        '部品詳細情報の取得
        '        dtRet = IC3801113BizLogic.GetSrvPartsDetailList(dealerCode, repairOrderNumber)

        '        '2012/03/03 日比野 ログ出力処理追加 START
        '        OutPutIFLog(dtRet, "IC3801113BizLogic.GetSrvPartsDetailList")
        '        '2012/03/03 日比野 ログ出力処理追加 END

        '    Else
        '        '関連チップ(子チップ)
        '        Dim IC3801007BizLogic As IC3801007BusinessLogic = New IC3801007BusinessLogic
        '        Dim dtChildInfomation As IC3801007DataSet.IC3801007PartsDetailInfoDataTable

        '        'Dim addSeq As Integer = 0
        '        'Integer.TryParse(strWorkSeq, addSeq)

        '        '部品詳細情報の取得
        '        dtChildInfomation = IC3801007BizLogic.GetPartsDetailList(dealerCode,
        '                                                                 repairOrderNumber,
        '                                                                 workSeq)

        '        '2012/03/03 日比野 ログ出力処理追加 START
        '        OutPutIFLog(dtChildInfomation, "IC3801007BizLogic.GetPartsDetailList")
        '        '2012/03/03 日比野 ログ出力処理追加 END

        '        Using dt As New IC3801113DataSet.IC3801113PartsDataTable

        '            '戻り値データ用に変換する
        '            For i = 0 To dtChildInfomation.Rows.Count - 1

        '                Dim dr As IC3801113DataSet.IC3801113PartsRow = dt.NewIC3801113PartsRow
        '                Dim drChildInfomation As IC3801007DataSet.IC3801007PartsDetailInfoRow = DirectCast(dtChildInfomation.Rows(i), IC3801007DataSet.IC3801007PartsDetailInfoRow)

        '                With dr
        '                    .Dealercode = drChildInfomation.DealerCode                                      '販売店コード
        '                    .Brncd = drChildInfomation.BrnCd                                                '店舗コード
        '                    .Orderno = drChildInfomation.OrderNo                                            '受注NO
        '                    .Partstype = drChildInfomation.PartsType                                        '部品区分
        '                    .Partsname = drChildInfomation.PartsName                                        '品名
        '                    .Quantity = drChildInfomation.Quantity.ToString(CultureInfo.InvariantCulture)   '数量
        '                    .Srvtypename = drChildInfomation.SrvTypeName                                    '整備区分名称
        '                    .Unit = drChildInfomation.Unit                                                  '単位
        '                    '2012/03/14 nishida 【SERVICE_1】課題管理番号-KN_0307_HH_1の不具合修正 B/O項目フラグ変更 START
        '                    If Not drChildInfomation.IsBoFlgNull Then
        '                        .Boflag = drChildInfomation.BoFlg                   'BOFLG
        '                    Else
        '                        .Boflag = String.Empty                              'BOFLG
        '                    End If
        '                    '.Boflag = drChildInfomation.BoFlg                       'BOFLG
        '                    '2012/03/14 nishida 【SERVICE_1】課題管理番号-KN_0307_HH_1の不具合修正 B/O項目フラグ変更 END
        '                End With

        '                '行追加
        '                dt.Rows.Add(dr)
        '            Next

        '            dtRet = dt
        '        End Using
        '    End If
        'End If

        Dim dtRet As IC3802504DataSet.IC3802504PartsDetailDataTable

        If (String.IsNullOrEmpty(repairOrderNumber)) Then
            dtRet = Nothing

            Logger.Info("PartsDetailData is Nothing")

        Else
            Dim IC3802504BizLogic As IC3802504BusinessLogic = New IC3802504BusinessLogic

            '引数用のデータセット宣言
            Using dt As New IC3802504DataSet.IC3802504RONumInfoDataTable


                'RO作業連番の数だけ繰り返す
                For i = 0 To workSeq.Count - 1

                    'データロウ宣言
                    Dim drPartsDetail As IC3802504DataSet.IC3802504RONumInfoRow = dt.NewIC3802504RONumInfoRow

                    'RO番号とRO作業連番を格納
                    drPartsDetail.R_O = repairOrderNumber
                    drPartsDetail.R_O_SEQNO = workSeq(i).ToString
                    '行に追加

                    dt.Rows.Add(drPartsDetail)
                Next


                '部品詳細情報の取得
                dtRet = IC3802504BizLogic.GetPartsDetailList(dealerCode, branchCode, dt)

            End Using
            IC3802504BizLogic.Dispose()
            OutPutIFLog(dtRet, "IC3802504BizLogic.GetSrvPartsDetailList")

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} OUT" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtRet

    End Function
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    ' 2012/06/01 KN 西田 STEP1 重要課題対応 END

    '2012/03/01 子チップ作業・部品情報取得対応 上田 End

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' TACTの該当追加作業のDataRowを取得
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="orderNo">予約ID</param>
    ' ''' <param name="workSeq">作業連番</param>
    ' ''' <returns>TACTの該当追加作業のDataRow</returns>
    ' ''' <remarks></remarks>
    'Public Function GetChildChipDataRow(ByVal dlrCD As String, ByVal orderNo As String, ByVal workSeq As Integer) As IC3800804DataSet.IC3800804AddRepairStatusDataTableRow

    '    Logger.Info("GetChildChipDataRow Start param1:" + dlrCD + _
    '                                    " param2:" + orderNo + _
    '                                    " param3:" + CType(workSeq, String))

    '    Dim dRow As IC3800804DataSet.IC3800804AddRepairStatusDataTableRow = Nothing

    '    Dim IC3800804 As New IC3800804BusinessLogic

    '    '追加作業API取得
    '    Dim dt As DataTable = IC3800804.GetAddRepairStatusList(dlrCD, orderNo)
    '    OutPutIFLog(dt, "IC3800804.GetAddRepairStatusList")

    '    '枝番（追加作業番号）が取得件数以上ない場合、データ不整合
    '    If Not IsNothing(dt) AndAlso workSeq <= dt.Rows.Count Then
    '        'テーブルの配列は0からのため、-1
    '        dRow = DirectCast(dt.Rows(workSeq - 1), IC3800804DataSet.IC3800804AddRepairStatusDataTableRow)
    '    End If

    '    If IsNothing(dRow) Then
    '        Logger.Error("GetChildChipDataRow End. Failed to found the IC3800804AddRepairStatusDataTableRow.")
    '    Else
    '        Logger.Info("GetChildChipDataRow End.")
    '    End If


    '    Return dRow
    'End Function

   
    ' ''' <summary>
    ' ''' ストール情報取得
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="strCD">店舗コード</param>
    ' ''' <param name="orderNo">R/O No.</param>
    ' ''' <returns>ストール情報</returns>
    ' ''' <remarks>作業項目欄のストール項目に記載するストール情報を取得</remarks>
    'Public Function GetStallInfoForWork(ByVal dlrCD As String, ByVal strCD As String, ByVal orderNo As String) As IC3810801DataSet.IC3810801StallDataTable

    '    Logger.Info("GetStallInfoForWork Start param1:" + dlrCD + _
    '                                         " param2:" + strCD + _
    '                                         " param3:" + orderNo)

    '    Dim dtStallInfo As IC3810801DataSet.IC3810801StallDataTable = Nothing
    '    Dim drInRow As IC3810801DataSet.IC3810801InGetStallRow

    '    Using dtStall As New IC3810801DataSet.IC3810801InGetStallDataTable()
    '        drInRow = CType(dtStall.NewRow(), IC3810801DataSet.IC3810801InGetStallRow)
    '    End Using

    '    ' 引数設定
    '    With drInRow
    '        .DLRCD = dlrCD
    '        .STRCD = strCD
    '        .ORDERNO = orderNo
    '    End With

    '    Using adapter As IC3810801BusinessLogic = New IC3810801BusinessLogic
    '        dtStallInfo = adapter.GetStall(drInRow)
    '    End Using

    '    Logger.Info("GetStallInfoForWork End")

    '    Return dtStallInfo
    'End Function
    
    ' ''' <summary>
    ' ''' 作業G情報取得
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <returns>作業Gの名前情報</returns>
    'Public Function GetWorkgroupList(ByVal dlrCD As String) As IC3801104DataSet.IC3801104WorkGroupInfoDataTable

    '    Logger.Info("GetWorkgroupList Start dlrCD=" & dlrCD)

    '    Dim dtWorkerInfo As IC3801104DataSet.IC3801104WorkGroupInfoDataTable = Nothing

    '    Dim ta As New IC3801104TableAdapter
    '    dtWorkerInfo = ta.GetWorkgroupInfo(dlrCD)

    '    Logger.Info("GetWorkgroupList End")

    '    Return dtWorkerInfo
    'End Function

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' ログ出力(IF戻り値用)
    ''' </summary>
    ''' <param name="dt">戻り値(DataTable)</param>
    ''' <param name="ifName">使用IF名</param>
    ''' <remarks></remarks>
    Private Sub OutPutIFLog(ByVal dt As DataTable, ByVal ifName As String)

        If dt Is Nothing Then
            Return
        End If

        Logger.Info(ifName + " Result START " + " OutPutCount: " + (dt.Rows.Count).ToString(CultureInfo.InvariantCulture))

        Dim log As New Text.StringBuilder

        For j = 0 To dt.Rows.Count - 1

            log = New Text.StringBuilder()
            Dim dr As DataRow = dt.Rows(j)

            log.Append("RowNum: " + (j + 1).ToString(CultureInfo.InvariantCulture) + " -- ")

            For i = 0 To dt.Columns.Count - 1
                log.Append(dt.Columns(i).Caption)
                If IsDBNull(dr(i)) Then
                    log.Append(" IS NULL")
                Else
                    log.Append(" = ")
                    log.Append(dr(i).ToString)
                End If

                If i <= dt.Columns.Count - 2 Then
                    log.Append(", ")
                End If
            Next

            Logger.Info(log.ToString)
        Next

        Logger.Info(ifName + " Result END ")

    End Sub

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 作業グループの登録
    ' ''' </summary>
    ' ''' <param name="dt">作業グループのDataTable</param>
    ' ''' <returns>処理結果 成功：>1 / 失敗：0</returns>
    ' ''' <remarks></remarks>
    'Public Function UpdateWorkGroup(ByVal dt As IC3801801DataSet.IC3801801WorkGroupInfoDataTable) As Long
    '    Dim IC3801801 As New IC3801801BusinessLogic

    '    Me.OutPutIFLog(dt, "IC3801801.UpdateWorkGroup")

    '    '追加作業更新処理
    '    Dim rtnVal As Long = IC3801801.UpdateWorkGroup(dt)

    '    Return rtnVal
    'End Function

    '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） START　GetServiceInHistory
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' 全ての履歴情報取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="repairOrderNumber">RO番号</param>
    ''' <returns>全ての履歴情報</returns>
    ''' <remarks></remarks>
    Public Function GetAllHistoryInfo(ByVal dealerCode As String, _
                                      ByVal branchCode As String, _
                                      ByVal repairOrderNumber As String, _
                                      ByVal getCount As Integer) As SC3150102DataSet.SC3150102GetServiceInHistoryDataTable
        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        'Public Function GetAllHistoryInfo(ByVal inRegisterNo As String, _
        '                             ByVal inVinNo As String) As IC3801601DataSet.ORDERHISTORYDataTable
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '           , "{0}.{1} START RegisterNo:{2}, VinNo:{3}. CALL IC3801601BusinessLogic.GetAllOrderHistory" _
        '           , Me.GetType.ToString _
        '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '           , inRegisterNo _
        '           , inVinNo))

        'Dim dtOrderHistory As IC3801601DataSet.ORDERHISTORYDataTable

        'Dim blIC3801601 As New IC3801601BusinessLogic
        'dtOrderHistory = blIC3801601.GetAllOrderHistory(inRegisterNo, inVinNo)

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} END " _
        '            , Me.GetType.ToString _
        '            , System.Reflection.MethodBase.GetCurrentMethod.Name))
        'Return dtOrderHistory


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1}  START DLR_CD:{2}, BNR_CD:{3}, DMS_CST_CD: {4}. " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dealerCode _
                    , branchCode _
                    , repairOrderNumber))

        Dim dtServiceInHistory As SC3150102DataSet.SC3150102GetServiceInHistoryDataTable

        Using adapter As SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter = _
                          New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter


            '入庫履歴情報の取得
            dtServiceInHistory = adapter.GetServiceInHistory(dealerCode, _
                                                             branchCode, _
                                                             repairOrderNumber, _
                                                             getCount)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtServiceInHistory
        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END
    End Function

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 自店の入庫履歴を5件取得する
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店CD</param>
    ' ''' <param name="inRegisterNo">車両登録No</param>
    ' ''' <param name="inVinNo">車両VinNo</param>
    ' ''' <param name="inBeginRow">開始行</param>
    ' ''' <param name="inEndRow">終了行</param>
    ' ''' <returns>全ての履歴情報</returns>
    ' ''' <remarks></remarks>
    'Public Function GetAllHistoryInfoInit(ByVal inDealerCode As String, _
    '                                      ByVal inRegisterNo As String, _
    '                                      ByVal inVinNo As String, _
    '                                      ByVal inBeginRow As Integer, _
    '                                      ByVal inEndRow As Integer) As IC3801601DataSet.ORDERHISTORYDataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START DlrCD:{2}, RegisterNo:{3}, VinNo:{4}, BeginRow:{5}, EndRow:{6}. CALL IC3801601BusinessLogic.GetAllOrderHistory" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , inDealerCode, inRegisterNo, inVinNo _
    '                , inBeginRow.ToString(CultureInfo.CurrentCulture), inEndRow.ToString(CultureInfo.CurrentCulture)))

    '    Dim dtOrderHistory As IC3801601DataSet.ORDERHISTORYDataTable

    '    Dim blIC3801601 As New IC3801601BusinessLogic
    '    dtOrderHistory = blIC3801601.GetAllOrderHistory(inDealerCode, _
    '                                                    inRegisterNo, _
    '                                                    inVinNo, _
    '                                                    inBeginRow, _
    '                                                    inEndRow)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END " _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return dtOrderHistory
    'End Function

    '2012/11/30 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.75） END
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END


    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' 画面連携URL取得
    ''' </summary>
    ''' <param name="displayNumber">画面番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDisplay(ByVal displayNumber As Integer) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1}  START DISP_NUM:{2}. " _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name _
              , displayNumber))

        '返却用変数
        Dim resultValue As String = Nothing

        'データテーブル宣言
        Dim dtdisplayUrl As SC3150102DataSet.SC3150102DisplayUrlDataTable = Nothing

        'アダプター宣言
        Using adapter As SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter = _
                           New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter

            '顧客情報の取得
            dtdisplayUrl = adapter.GetDisplayUrl(displayNumber)

        End Using

        'データロウ宣言
        Dim drdisplayUrl As SC3150102DataSet.SC3150102DisplayUrlRow = _
               DirectCast(dtdisplayUrl.Rows(0), SC3150102DataSet.SC3150102DisplayUrlRow)

        '返却用変数に取得した値を格納
        resultValue = drdisplayUrl.DMS_DISP_URL

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return resultValue
    End Function

    ''' <summary>
    ''' 画面連携の引数取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="jobDetailId">作業内容ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetScreenLinkageInfo(ByVal dealerCode As String, _
                                         ByVal branchCode As String, _
                                         ByVal jobDetailId As Decimal) As SC3150102DataSet.SC3150102ScreenLinkageInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1}  START DLR_CD:{2}, BNR_CD:{3}, JOB_DTL_ID:{4}. " _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name _
               , dealerCode _
               , branchCode _
               , jobDetailId))

        'データテーブル宣言
        Dim dtScreenLinkageInfo As SC3150102DataSet.SC3150102ScreenLinkageInfoDataTable

        'アダプター宣言
        Using adapter As SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter = _
                           New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter

            '画面連携の引数取得
            dtScreenLinkageInfo = adapter.GetScreenLinkageInfo(dealerCode, _
                                                              branchCode, _
                                                              jobDetailId)

        End Using



        Logger.Info(String.Format(CultureInfo.CurrentCulture _
             , "{0}.{1} END" _
             , Me.GetType.ToString _
             , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtScreenLinkageInfo
    End Function

    ''' <summary>
    ''' 顧客情報の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="repairOrderNumber">RO番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCustomerInfo(ByVal dealerCode As String, _
                                    ByVal branchCode As String, _
                                    ByVal repairOrderNumber As String) As SC3150102DataSet.SC3150102CutomerInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1}  START DLR_CD:{2}, BNR_CD:{3}, RO_NUM:{4}. " _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name _
               , dealerCode _
               , branchCode _
               , repairOrderNumber))

        'データテーブル宣言
        Dim dtServiceInHistory As SC3150102DataSet.SC3150102CutomerInfoDataTable

        'アダプター宣言
        Using adapter As SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter = _
                           New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter

            '顧客情報の取得
            dtServiceInHistory = adapter.GetCustomerInfo(dealerCode, _
                                                         branchCode, _
                                                         repairOrderNumber)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
             , "{0}.{1} END" _
             , Me.GetType.ToString _
             , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtServiceInHistory
    End Function
    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' チップ情報の取得
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChipInfo(ByVal stallUseId As Decimal) As SC3150102DataSet.SC3150102ChipDateTimeInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


        ' 実績チップ情報を取得
        Dim selectChipInfo As SC3150102DataSet.SC3150102ChipDateTimeInfoDataTable

        Using adapter As New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter

            selectChipInfo = adapter.GetSelectChipTimeInfo(stallUseId)

        End Using


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} END" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return selectChipInfo

    End Function

    ''' <summary>
    ''' 休憩時間を取得.
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBreakData(ByVal dealerCode As String _
                               , ByVal branchCode As String _
                               , ByVal stallId As Decimal) As SC3150102DataSet.SC3150102BreakChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START,　DLR_CD:{2}, BRN_CD{3}, STALL_ID{4}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , dealerCode _
                                  , branchCode _
                                  , CType(stallId, String)))

        Dim dtBreskChip As SC3150102DataSet.SC3150102BreakChipInfoDataTable
        Dim userContext As StaffContext = StaffContext.Current

        Using adapter As New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter

            '休憩チップデータを取得.
            dtBreskChip = adapter.GetBreakChipInfo(dealerCode, branchCode, stallId)

        End Using

        For Each dr As DataRow In dtBreskChip.Rows

            '開始時間と終了時間をDate型に変換する.
            Dim startTimeDate As Date = ExchangeBreakHourToDate(userContext.DlrCD, CType(dr("STARTTIME"), String))
            Dim endTimeDate As Date = ExchangeBreakHourToDate(userContext.DlrCD, CType(dr("ENDTIME"), String))

            '終了時間が開始時間以下の場合、終了時間のほうが大きくなるように終了時間に1日ずつ加算していく.
            While endTimeDate <= startTimeDate

                endTimeDate = endTimeDate.AddDays(1)

            End While

            dr("STARTTIME") = startTimeDate
            '2020/01/09 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            '既存不具合を修正
            'dr("ENDTIME") = startTimeDate
            dr("ENDTIME") = endTimeDate
            '2020/01/09 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        Next


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} END" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtBreskChip


    End Function

    ''' <summary>
    ''' 使用不可チップ情報の取得.
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="fromDate">ストール稼動開始時間</param>
    ''' <param name="toDate">ストール稼動終了時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUnavailableData(ByVal stallId As Decimal,
                                       ByVal fromDate As Date, _
                                       ByVal toDate As Date) As SC3150102DataSet.SC3150102UnavailableChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START,param1:{2}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , CType(stallId, String)))

        'データテーブルを宣言
        Dim dtUnavailable As SC3150102DataSet.SC3150102UnavailableChipInfoDataTable

        Using adapter As New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter
            '使用不可チップデータを取得.
            dtUnavailable = adapter.GetUnavailableChipInfo(stallId, fromDate, toDate)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtUnavailable

    End Function

    ''' <summary>
    ''' 中断Job存在判定
    ''' </summary>
    ''' <param name="stallUseId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HasStopJob(ByVal stallUseId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} START. STALL_USE_ID_CD:{2}, " _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                     , stalluseId.ToString(CultureInfo.CurrentCulture()) _
                     ))
        '戻り値
        Dim resultValue As Boolean = False

        Using smbCommonClass As New TabletSMBCommonClassBusinessLogic

            resultValue = smbCommonClass.HasStopJob(stallUseId)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return resultValue

    End Function

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END

    '2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　START
    ''' <summary>
    ''' かご番号取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="repairOrderNumber">RO番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCageNumber(ByVal dealerCode As String, _
                                  ByVal branchCode As String, _
                                  ByVal repairOrderNumber As String) As SC3150102DataSet.SC3150102CageInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START.DLR_CD:{2},BRN_CD:{3},RO_NUM:{4} " _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , dealerCode _
                                , branchCode _
                                , repairOrderNumber))
        '戻り値
        Dim dtCageInfo As SC3150102DataSet.SC3150102CageInfoDataTable

        Using adapter As New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter

            'かご情報を取得.
            dtCageInfo = adapter.GetCageNoInfo(dealerCode, branchCode, repairOrderNumber)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtCageInfo

    End Function

    '2014/08/29 TMEJ 成澤 【開発】IT9737_NextSTEPサービス ロケ管理の効率化に向けた評価用アプリ作成　END
#End Region

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 START

#Region "更新系処理"

    ''' <summary>
    ''' Job開始処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="resultStartDateTime">実績終了日時</param>
    ''' <param name="jobInstructId">中断時間</param>
    ''' <param name="jobInstructSeq">中断メモ</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="UpdateDateTime">更新日時</param>
    ''' <param name="rowUpdateCount">行ロックバージョン</param>
    ''' <param name="applicationId">画面ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </History>
    <EnableCommit()>
    Public Function JobStart(ByVal stallUseId As Decimal, _
                             ByVal resultStartDateTime As Date, _
                             ByVal restFlg As String, _
                             ByVal jobInstructId As String, _
                             ByVal jobInstructSeq As Long, _
                             ByVal updateDateTime As Date, _
                             ByVal rowUpdateCount As Long,
                             ByVal applicationId As String) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} START" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '戻り値
        Dim resultCode As Long = 0

        Try

            'SMBコモンクラスのインスタンスを生成
            Using smbCommonClass As New TabletSMBCommonClassBusinessLogic

                'Job開始処理
                resultCode = smbCommonClass.StartSingleJob(stallUseId, _
                                                           resultStartDateTime, _
                                                           restFlg, _
                                                           jobInstructId, _
                                                           jobInstructSeq, _
                                                           updateDateTime, _
                                                           rowUpdateCount, _
                                                           applicationId)
                'PUSH送信フラグの取得
                NeedPushStartSingleJob = smbCommonClass.NeedPushAfterStartSingleJob()

            End Using

            'Job開始処理に失敗した場合
            If resultCode <> ActionResult.Success Then
                '出力するエラーメッセージの文言設定
                resultCode = OtherSystemsReturnCodeSelect(resultCode, workStartFlg)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''Job開始処理に失敗しました
                'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                     , "{0}.{1}.{2}" _
                '                     , Me.GetType.ToString _
                '                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                     , "Failed to  start of processing jop."))
                'Exit Try

                'エラー内容チェック
                If resultCode <> ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」でない場合
                    'エラーを返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1}.{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , "Failed to  start of processing jop."))
                    Exit Try

                End If

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If
        Finally

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            '' 正常終了以外はロールバック
            'If resultCode <> ActionResult.Success Then
            '    Me.Rollback = True
            'End If

            ' 「0：正常終了」「-9000：DMS除外エラーの警告」以外はロールバック
            If resultCode <> 0 AndAlso resultCode <> ActionResult.WarningOmitDmsError Then
                Me.Rollback = True

            End If

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} START" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return resultCode

    End Function

    ''' <summary>
    ''' Job終了処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="resultEndDateTime">実績終了日時</param>
    ''' <param name="jobInstructId">中断時間</param>
    ''' <param name="jobInstructSeq">中断メモ</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="UpdateDateTime">更新日時</param>
    ''' <param name="rowUpdateCount">行ロックバージョン</param>
    ''' <param name="applicationId">画面ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </History>
    <EnableCommit()>
    Public Function JobFinish(ByVal stallUseId As Decimal, _
                              ByVal resultEndDateTime As Date, _
                              ByVal restFlg As String, _
                              ByVal jobInstructId As String, _
                              ByVal jobInstructSeq As Long, _
                              ByVal updateDateTime As Date, _
                              ByVal rowUpdateCount As Long,
                              ByVal applicationId As String) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} START" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '戻り値
        Dim resultCode As Long = 0

        Try

            'SMBコモンクラスのインスタンスを生成
            Using smbCommonClass As New TabletSMBCommonClassBusinessLogic

                'Job終了処理
                resultCode = smbCommonClass.FinishSingleJob(stallUseId, _
                                                           resultEndDateTime, _
                                                           restFlg, _
                                                           jobInstructId, _
                                                           jobInstructSeq, _
                                                           updateDateTime, _
                                                           rowUpdateCount, _
                                                           applicationId)

                'PUSH送信フラグの取得
                NeedPushFinishSingleJob = smbCommonClass.NeedPushAfterFinishSingleJob()

                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                'サブエリア更新Pushフラグの取得
                NeedPushSubAreaRefresh = smbCommonClass.NeedPushSubAreaRefresh()
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
            End Using

            'Job終了処理に失敗した場合
            If resultCode <> ActionResult.Success Then
                '出力するエラーメッセージの文言設定
                resultCode = OtherSystemsReturnCodeSelect(resultCode, workFinishFlg)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''Job終了処理に失敗しました
                'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                     , "{0}.{1}.{2}" _
                '                     , Me.GetType.ToString _
                '                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                     , "Failed to finish of processing jop."))
                'Exit Try

                'エラー内容チェック
                If resultCode <> ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」でない場合
                    'エラーを返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1}.{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , "Failed to finish of processing jop."))
                    Exit Try

                End If

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If

        Finally

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            '' 正常終了以外はロールバック
            'If resultCode <> ActionResult.Success Then
            '    Me.Rollback = True
            'End If

            ' 「0：正常終了」「-9000：DMS除外エラーの警告」以外はロールバック
            If resultCode <> 0 AndAlso resultCode <> ActionResult.WarningOmitDmsError Then
                Me.Rollback = True

            End If

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} START" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return resultCode

    End Function

    ''' <summary>
    ''' Job中断処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="resultEndDateTime">実績終了日時</param>
    ''' <param name="stopTime">中断時間</param>
    ''' <param name="stopMemo">中断メモ</param>
    ''' <param name="stopReasonType">中断理由区分</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="UpdateDateTime">更新日時</param>
    ''' <param name="rowUpdateCount">行ロックバージョン</param>
    ''' <param name="applicationId">画面ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function JobStop(ByVal stallUseId As Decimal, _
                            ByVal resultEndDateTime As Date, _
                            ByVal stopTime As Long, _
                            ByVal stopMemo As String, _
                            ByVal stopReasonType As String, _
                            ByVal restFlg As String, _
                            ByVal jobInstructId As String, _
                            ByVal jobInstructSeq As Long, _
                            ByVal updateDateTime As Date, _
                            ByVal rowUpdateCount As Long,
                            ByVal applicationId As String) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} START" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '戻り値
        Dim resultCode As Long = 0

        Try

            'SMBコモンクラスのインスタンスを生成
            Using smbCommonClass As New TabletSMBCommonClassBusinessLogic

                '中断処理
                resultCode = smbCommonClass.StopSingleJob(stallUseId, _
                                                          resultEndDateTime, _
                                                          stopTime, _
                                                          stopMemo, _
                                                          stopReasonType, _
                                                          restFlg, _
                                                          jobInstructId, _
                                                          jobInstructSeq, _
                                                          updateDateTime, _
                                                          rowUpdateCount, _
                                                          applicationId)

                'PUSH送信フラグの取得
                NeedPushStopSingleJob = smbCommonClass.NeedPushAfterStopSingleJob()

                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                'サブエリア更新Pushフラグの取得
                NeedPushSubAreaRefresh = smbCommonClass.NeedPushSubAreaRefresh()
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
            End Using

            '中断処理に失敗した場合
            If resultCode <> ActionResult.Success Then
                '出力するエラーメッセージの文言設定
                resultCode = OtherSystemsReturnCodeSelect(resultCode, workStopFlg)

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''中断処理に失敗しました
                'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                     , "{0}.{1}.{2}" _
                '                     , Me.GetType.ToString _
                '                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                     , "Failed to interruption of processing job."))
                'Exit Try

                'エラー内容チェック
                If resultCode <> ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」でない場合
                    'エラーを返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1}.{2}" _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                         , "Failed to interruption of processing job."))
                    Exit Try

                End If

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If

        Finally

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            '' 正常終了以外はロールバック
            'If resultCode <> ActionResult.Success Then
            '    Me.Rollback = True
            'End If

            ' 「0：正常終了」「-9000：DMS除外エラーの警告」以外はロールバック
            If resultCode <> 0 AndAlso resultCode <> ActionResult.WarningOmitDmsError Then
                Me.Rollback = True

            End If

            '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} START" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return resultCode

    End Function

    ''' <summary>
    '''Push送信判定
    ''' </summary>
    ''' <param name="processFlg">処理フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HasSendPush(ByVal processFlg As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                     , "{0}.{1} START" _
                     , Me.GetType.ToString _
                     , System.Reflection.MethodBase.GetCurrentMethod.Name
                     ))
        '戻り値
        Dim resultValue As Boolean = False

        '開始処理の場合
        If processFlg.Equals(workStartFlg) Then
            resultValue = NeedPushStartSingleJob

        ElseIf processFlg.Equals(workFinishFlg) Then
            '終了処理の場合、終了、中断両方のフラグで判断する
            If NeedPushFinishSingleJob Or _
                NeedPushStopSingleJob Then

                resultValue = True

            End If
        Else
            '中断処理の場合
            resultValue = NeedPushStopSingleJob
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return resultValue

    End Function

    ''' <summary>
    ''' 他システムの戻り値からエラーコードを判断する
    ''' </summary>
    ''' <param name="inReturnCode">tabletSmbからの戻り値</param>
    ''' <param name="workFlg">開始・日跨ぎ・終了の判断フラグ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Private Function OtherSystemsReturnCodeSelect(ByVal inReturnCode As Long, _
                                                  Optional ByVal workFlg As String = "4") As Integer

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} START. RETURN_CODE{2},WORK_FLG{3}." _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name _
               , inReturnCode.ToString(CultureInfo.CurrentCulture()) _
               , workFlg))

        '戻り値
        Dim returnValue As Integer = 0

        Select Case inReturnCode

            Case ActionResult.OutOfWorkingTimeError
                returnValue = 905
                '開始できませんでした。ストールの稼動時間外です。

            Case ActionResult.HasWorkingChipInOneStallError
                returnValue = 906
                '開始できませんでした。すでに作業中のチップがあります。

            Case ActionResult.RowLockVersionError
                returnValue = 907
                'そのチップは、既に他のオペレータによって変更が加えられています。画面を再表示してから再度処理を行ってください。

            Case ActionResult.LockStallError
                returnValue = 912
                '該当ストールに対して、他のユーザーが変更を行なっています。時間を置いて再度処理を行なってください。

            Case ActionResult.DBTimeOutError
                returnValue = 908
                'データベースとの接続でタイムアウトが発生しました。再度処理を行ってください。

            Case ActionResult.DmsLinkageError
                returnValue = 909
                '他システムとの連携時にエラーが発生しました。システム管理者に連絡してください。

            Case ActionResult.ParentroNotStartedError
                returnValue = 910
                '開始できませんでした。親R/Oが開始されていません。

            Case ActionResult.NoTechnicianError

                If workFlg.Equals(workStartFlg) Then
                    returnValue = 911
                    '開始できませんでした。作業担当者が存在しません。
                ElseIf workFlg.Equals(workFinishFlg) Then
                    returnValue = 915
                    '終了できませんでした。作業担当者が存在しません。
                ElseIf workFlg.Equals(workStopFlg) Then
                    returnValue = 918
                    '中断できませんでした。作業担当者が存在しません。
                End If

            Case ActionResult.OverlapError
                returnValue = 914
                '他のチップと配置時間が重複します。

            Case ActionResult.OverlapUnavailableError
                returnValue = 917
                '使用不可チップを他のチップの上に重複させることができません。

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            Case ActionResult.WarningOmitDmsError
                returnValue = ActionResult.WarningOmitDmsError
                'このチップはDMSによって不適切に完了されています。システム内の記録された時間は、実際の作業時間ではありません。

                '2015/05/13 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            Case IC3802503BusinessLogic.Result.TimeOutError
                ' タイムアウトエラー
                returnValue = 901
            Case IC3802503BusinessLogic.Result.DmsError
                ' 基幹側のエラー
                returnValue = 902
            Case IC3802503BusinessLogic.Result.OtherError
                ' その他のエラー
                returnValue = 903

                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START

            Case ActionResult.HasStartedRelationChipError
                '関連チップが作業中
                returnValue = 927

                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END

            Case Else

                If workFlg.Equals(workStartFlg) Then
                    returnValue = 904
                    '開始できませんでした。
                ElseIf workFlg.Equals(workFinishFlg) Then
                    returnValue = 913
                    '終了できませんでした。
                ElseIf workFlg.Equals(workStopFlg) Then
                    returnValue = 916
                    '中断できませんでした。
                End If

        End Select

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1} END" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return returnValue
    End Function

#End Region

#Region "計算処理"

    ''' <summary>
    ''' DBより取得した4桁の休憩時間を当日付けのDate型に変換する.
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="breakHour">4桁の休憩時間</param>
    ''' <returns>Date型の休憩時間</returns>
    ''' <remarks></remarks>
    Private Function ExchangeBreakHourToDate(ByVal dealerCode As String, ByVal breakHour As String) As Date

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1} START,param1:{2},param2:{3}" _
                                  , Me.GetType.ToString _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                  , dealerCode _
                                  , breakHour))

        '返す値の初期値として、当日の0時を設定する.
        Dim breakDate As Date = DateTimeFunc.Now(dealerCode).Date

        '取得した引数が4桁である場合、変換処理を実施する.
        If (breakHour.Length = BREAK_TIME_DATA_STRING_LENGTH) Then

            '時間と分に分割する
            Dim hourUnit As String = breakHour.Substring(0, 2)
            Dim minuteUnit As String = breakHour.Substring(2)

            '時間と分を格納
            breakDate = breakDate.AddHours(Double.Parse(hourUnit, CultureInfo.CurrentCulture()))
            breakDate = breakDate.AddMinutes(Double.Parse(minuteUnit, CultureInfo.CurrentCulture()))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} END return:{2}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , DateTimeFunc.FormatDate(DATE_CONVERT_ID_YYYYMMDDHHMM, breakDate)))

        Return breakDate

    End Function

#End Region

#Region "通知&Push送信"

#Region "Push用定数"

    ''' <summary>
    ''' 削除されていないユーザ(delflg=0)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DelFlgNone As String = "0"
    ''' <summary>
    ''' 通知API用(カテゴリータイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPushCategory As String = "1"

    ''' <summary>
    ''' 通知API用(表示位置)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPotisionType As String = "1"

    ''' <summary>
    ''' 通知API用(表示時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyTime As Integer = 3

    ''' <summary>
    ''' 通知API用(表示タイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispType As String = "1"

    ''' <summary>
    ''' 通知API用(色)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyColor As String = "1"

    ''' <summary>
    ''' 通知API用(呼び出し関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispFunction As String = "icropScript.ui.setNotice()"

    ''' <summary>
    ''' 通知履歴のSessionValue(カンマ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueKanma As String = ","
    ''' <summary>
    ''' 通知履歴のSessionValue(文字列)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueString As String = "String"

    ''' <summary>
    ''' 通知履歴のSessionValue(販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDealerCode As String = "Session.Param1,"

    ''' <summary>
    ''' 通知履歴のSessionValue(店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueBranchCode As String = "Session.Param2,"

    ''' <summary>
    ''' 通知履歴のSessionValue(スタッフコード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueLoginUserID As String = "Session.Param3,"

    ''' <summary>
    ''' 通知履歴のSessionValue(SAチップID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSAChipID As String = "Session.Param4,"

    ''' <summary>
    ''' 通知履歴のSessionValue(基幹作業内容ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueBasRezId As String = "Session.Param5,"

    ''' <summary>
    ''' 通知履歴のSessionValue(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueRepiarOrder As String = "Session.Param6,"

    ''' <summary>
    ''' 通知履歴のSessionValue(RO作業連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueRepiarOrderSeq As String = "Session.Param7,"

    ''' <summary>
    ''' 通知履歴のSessionValue(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueVinNo As String = "Session.Param8,"

    ''' <summary>
    ''' 通知履歴のSessionValue(「0：編集」固定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueViewMode As String = "Session.Param9,"

    ''' <summary>
    ''' 通知履歴のSessionValue(「0：プレビュー」固定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueFormat As String = "Session.Param10,"
    ''' <summary>
    ''' 通知履歴のSessionValue(画面番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDisplayNumber As String = "Session.DISP_NUM,"

    ''' <summary>
    ''' 通知履歴のSessionValue(基幹顧客ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDmsCustomerCode As String = "SessionKey.DMS_CST_CD,"

    ''' <summary>
    ''' 通知履歴のSessionValue(基幹顧客ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueVin As String = "SessionKey.VIN,"
    ''' <summary>
    ''' 基幹顧客IDの分解文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUNCTUATION_STRING As String = "@"

    ''' <summary>
    ''' 13：ROプレビュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPLAY_NUMBER_13 As String = "13"
    ''' <summary>
    ''' 0：編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ViewMode As String = "0"
    ''' <summary>
    ''' 0：プレビュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FormatPreview As String = "0"
    ''' <summary>
    ''' 通知履歴のSessionValue(顧客詳細フラグ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueType As String = "Redirect.FLAG,String,"
    ''' <summary>
    ''' ROプレビューリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const repiarOrderPreviewLink As String = "<a id='SC30105010' Class='SC3010501' href='/Website/Pages/SC3010501.aspx' onclick='return ServiceLinkClick(event)'>"
    ''' <summary>
    ''' 顧客詳細リンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerDetailLink As String = "<a id='SC30802251' Class='SC3080225' href='/Website/Pages/SC3080225.aspx' onclick='return ServiceLinkClick(event)'>"
    ''' <summary>
    ''' Aタグ終了文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EndLikTag As String = "</a>"

    ''' <summary>
    ''' TabletSMBリフレッシュ関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const smbRefreshFunction As String = "RefreshSMB()"
    ''' <summary>
    ''' PSメインリフレッシュ関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const psRefreshFunction As String = "MainRefresh()"
    ''' <summary>
    ''' SAメインリフレッシュ関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const saRefreshFunction As String = "MainRefresh()"
    '2014/08/13 TMEJ 成澤 IT9729_NextSTEPサービス CfTC進捗管理に向けた評価用アプリ作成 START
    ''' <summary>
    ''' お客様端末リフレッシュ関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerTerminalRefreshFunction As String = "RefreshCustomerTerminal()"
    ''' <summary>
    ''' お客様端末リフレッシュ関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TcTerminalRefreshFunction As String = "RefreshJob("
    '2014/08/13 TMEJ 成澤 IT9729_NextSTEPサービス CfTC進捗管理に向けた評価用アプリ作成 END
#End Region

    ''' <summary>
    ''' 通知メイン処理
    ''' </summary>
    ''' <param name="jobDatilId">RO番号</param>
    ''' <param name="braunchCode" >販売店コード</param>
    ''' <param name="dealerCode">店舗コード</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <remarks></remarks>
    Public Sub NoticeMainProcessing(ByVal jobDatilId As Decimal, _
                                    ByVal dealerCode As String, _
                                    ByVal braunchCode As String, _
                                    ByVal inStaffInfo As StaffContext)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim dtVisitInfo As SC3150102DataSet.SC3150102ScreenLinkageInfoDataTable

        Using adapter As New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter

            dtVisitInfo = adapter.GetScreenLinkageInfo(dealerCode, braunchCode, jobDatilId)
        End Using

        If (dtVisitInfo.Rows.Count > 0) AndAlso
            (Not dtVisitInfo(0).IsSACODENull) AndAlso
            (Not String.IsNullOrEmpty(dtVisitInfo(0).SACODE)) Then

            Dim drVisitInfo As SC3150102DataSet.SC3150102ScreenLinkageInfoRow = _
                DirectCast(dtVisitInfo.Rows(0), SC3150102DataSet.SC3150102ScreenLinkageInfoRow)


            '送信先アカウント情報設定
            Dim account As XmlAccount = Me.CreateAccount(drVisitInfo)

            '通知履歴登録情報の設定
            Dim requestNotice As XmlRequestNotice = Me.CreateRequestNotice(drVisitInfo, inStaffInfo)

            'Push内容設定
            Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(drVisitInfo)

            Dim userContext As StaffContext = StaffContext.Current

            '設定したものを格納し、通知APIをコール
            Using noticeData As New XmlNoticeData

                '現在時間データの格納
                noticeData.TransmissionDate = DateTimeFunc.Now(userContext.DlrCD)
                '送信ユーザーデータ格納
                noticeData.AccountList.Add(account)
                '通知履歴用のデータ格納
                noticeData.RequestNotice = requestNotice
                'Pushデータ格納
                noticeData.PushInfo = pushInfo

                '通知処理実行
                Using ic3040801Biz As New IC3040801BusinessLogic

                    '通知処理実行
                    ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)

                End Using
            End Using
        End If
        dtVisitInfo.Dispose()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 送信先アカウント情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateAccount(ByVal inRowVisitInfo As SC3150102DataSet.SC3150102ScreenLinkageInfoRow) As XmlAccount

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using account As New XmlAccount

            Dim usersClass As New Users

            Dim rowUsers As UsersDataSet.USERSRow

            If (Not inRowVisitInfo.IsSACODENull) AndAlso _
                (Not String.IsNullOrEmpty(inRowVisitInfo.SACODE)) Then

                'SACODEでユーザー情報の取得
                rowUsers = usersClass.GetUser(inRowVisitInfo.SACODE, DelFlgNone)

                '受信先のアカウント設定
                account.ToAccount = rowUsers.ACCOUNT

                '受信者名設定
                account.ToAccountName = rowUsers.USERNAME
            End If
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return account

        End Using
    End Function

    ''' <summary>
    ''' 通知履歴登録情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo"></param>
    ''' <param name="inStaffInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateRequestNotice(ByVal inRowVisitInfo As SC3150102DataSet.SC3150102ScreenLinkageInfoRow, _
                                         ByVal inStaffInfo As StaffContext) As XmlRequestNotice

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using requestNotice As New XmlRequestNotice

            '販売店コード設定
            requestNotice.DealerCode = inStaffInfo.DlrCD
            '店舗コード設定
            requestNotice.StoreCode = inStaffInfo.BrnCD
            'スタッフコード(送信元)設定
            requestNotice.FromAccount = inStaffInfo.Account
            'スタッフ名(送信元)設定
            requestNotice.FromAccountName = inStaffInfo.UserName
            '表示内容設定
            requestNotice.Message = Me.CreateNoticeRequestMessage(inRowVisitInfo)
            'セッション設定値設定
            requestNotice.SessionValue = Me.CreateNoticeRequestSession(inRowVisitInfo, inStaffInfo)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return requestNotice
        End Using
    End Function

    ''' <summary>
    ''' 通知履歴用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報表示欄</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateNoticeRequestSession(ByVal inRowVisitInfo As SC3150102DataSet.SC3150102ScreenLinkageInfoRow, _
                                                ByVal inStaffInfo As StaffContext) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder

        'インスタンスの宣言
        Using serviceCommon As New ServiceCommonClassBusinessLogic

            '基幹情報の取得
            Using dtServiceCommon As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
                 serviceCommon.GetIcropToDmsCode(inStaffInfo.DlrCD, _
                                                DmsCodeType.BranchCode, _
                                                inStaffInfo.DlrCD, _
                                                inStaffInfo.BrnCD, _
                                                Nothing, _
                                                inStaffInfo.Account)

                'ロウに格納
                Dim drServiceCommon As ServiceCommonClassDataSet.DmsCodeMapRow = _
                     DirectCast(dtServiceCommon.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

                '販売店コードのセッション設定
                Me.SetSessionValueWord(workSession, SessionValueDealerCode, drServiceCommon.CODE1)

                '販売店コードのセッション値設定
                Me.SetSessionValueWord(workSession, SessionValueBranchCode, drServiceCommon.CODE2)

                'ログインスタッフコードのセッション値設定
                Me.SetSessionValueWord(workSession, SessionValueLoginUserID, drServiceCommon.ACCOUNT)
            End Using
        End Using

        'VINの設定
        If Not inRowVisitInfo.IsVINNull Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueVinNo, inRowVisitInfo.VIN)

        End If

        '訪問連番の設定
        If Not inRowVisitInfo.IsVISITSEQNull Then
            '訪問連番がある場合は設定

            '訪問連番のセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueSAChipID, inRowVisitInfo.VISITSEQ.ToString(CultureInfo.CurrentCulture()))

        End If

        '基幹作業内容IDの設定
        If Not inRowVisitInfo.IsDMS_JOB_DTL_IDNull Then
            '基幹作業内容IDがある場合は設定

            '基幹作業内容IDのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueBasRezId, inRowVisitInfo.DMS_JOB_DTL_ID)

        End If

        'RO番号の設定
        If Not inRowVisitInfo.IsRO_NUMNull Then
            'RO番号がある場合は設定

            'RO番号のセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueRepiarOrder, inRowVisitInfo.RO_NUM)

        End If

        'RO連番の設定
        If Not inRowVisitInfo.IsRO_SEQNull Then
            '予約IDがある場合は設定

            '予約IDのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueRepiarOrderSeq, inRowVisitInfo.RO_SEQ.ToString(CultureInfo.CurrentCulture))
        End If

        'ROプレビュー設定値(ViewMode)のセッション設定
        Me.SetSessionValueWord(workSession, SessionValueViewMode, ViewMode)
        'ROプレビュー設定値(Format)のセッション設定
        Me.SetSessionValueWord(workSession, SessionValueFormat, FormatPreview)
        'ROプレビュー画面番号のセッション設定
        Me.SetSessionValueWord(workSession, SessionValueDisplayNumber, DISPLAY_NUMBER_13)

        '別画面のセッションに切り替え
        workSession.Append(vbTab)

        '基幹顧客IDの設定
        If Not inRowVisitInfo.IsDMS_CST_CDNull Then

            '文字列の分割位置の取得
            Dim StringIndex As Integer = inRowVisitInfo.DMS_CST_CD.IndexOf(PUNCTUATION_STRING, StringComparison.CurrentCulture)
            Dim stringDmsCustomerCode As String = inRowVisitInfo.DMS_CST_CD

            If StringIndex > 0 Then
                '入庫管理番号を分割し、RO番号の部分を取得
                stringDmsCustomerCode = inRowVisitInfo.DMS_CST_CD.Substring(StringIndex + 1)
            End If

            '顧客詳細セッション「基幹顧客ID」設定
            Me.SetSessionValueWord(workSession, SessionValueDmsCustomerCode, stringDmsCustomerCode, False)

        End If

        'VINの設定
        If Not inRowVisitInfo.IsVINNull Then
            'VINがある場合は設定

            '顧客詳細セッション「Vin」設定
            Me.SetSessionValueWord(workSession, SessionValueVin, inRowVisitInfo.VIN)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession.ToString

    End Function

    ''' <summary>
    ''' SessionValue文字列作成
    ''' </summary>
    ''' <param name="workSession">追加元文字列</param>
    ''' <param name="SessionValueWord">追加するSESSIONKEY</param>
    ''' <param name="SessionValueData">追加するデータ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetSessionValueWord(ByVal workSession As StringBuilder, _
                                         ByVal SessionValueWord As String, _
                                         ByVal SessionValueData As String, _
                                         Optional ByVal switchFlg As Boolean = True) As StringBuilder

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'カンマの設定
        If (switchFlg) AndAlso (workSession.Length <> 0) Then
            'データがある場合

            '「,」を結合する
            workSession.Append(SessionValueKanma)

        End If

        'セッションキーを設定
        workSession.Append(SessionValueWord)
        '文字列型式に設定
        workSession.Append(SessionValueString)
        '「,」を結合する
        workSession.Append(SessionValueKanma)
        'セッション値を設定
        workSession.Append(SessionValueData)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession

    End Function

    ''' <summary>
    ''' Push情報作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreatePushInfo(ByVal inRowVisitInfo As SC3150102DataSet.SC3150102ScreenLinkageInfoRow) As XmlPushInfo

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'PUSH内容設定
        Using pushInfo As New XmlPushInfo

            'カテゴリータイプ設定
            pushInfo.PushCategory = NotifyPushCategory
            '表示位置設定
            pushInfo.PositionType = NotifyPotisionType
            '表示時間設定
            pushInfo.Time = NotifyTime
            '表示タイプ設定
            pushInfo.DisplayType = NotifyDispType
            '表示内容設定
            pushInfo.DisplayContents = Me.CreatePusuMessage(inRowVisitInfo)
            '色設定
            pushInfo.Color = NotifyColor
            '表示時関数設定
            pushInfo.DisplayFunction = NotifyDispFunction

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return pushInfo

        End Using
    End Function

    ''' <summary>
    ''' 通知履歴用メッセージ作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ''' <returns>作成したメッセージ文言</returns>
    ''' <history>
    ''' </history>
    ''' <remarks></remarks>
    Private Function CreateNoticeRequestMessage(ByVal inRowVisitInfo As SC3150102DataSet.SC3150102ScreenLinkageInfoRow) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workContents As New StringBuilder
        '文言「お客様」を設定
        Dim customerWording As String = WebWordUtility.GetWord(APPLICATION_ID, 342)
        '文言「整備完了」を設定
        workContents.Append(WebWordUtility.GetWord(APPLICATION_ID, 341))

        'メッセージ組立：RO番号
        If Not (inRowVisitInfo.IsRO_NUMNull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.RO_NUM)) Then
            'RO番号がある場合

            'メッセージ間にスペースの設定
            workContents.Append(Space(3))

            'ROプレビューリンクのAタグを設定
            workContents.Append(repiarOrderPreviewLink)

            'RO番号を設定
            workContents.Append(inRowVisitInfo.RO_NUM)

            'Aタグ終了を設定
            workContents.Append(EndLikTag)
        End If

        'メッセージ間にスペースの設定
        workContents.Append(Space(3))

        '基幹顧客IDがある場合
        If Not (inRowVisitInfo.IsDMS_CST_CDNull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.DMS_CST_CD)) Then


            '顧客詳細リンクのAタグを設定
            workContents.Append(CustomerDetailLink)
        End If

        'メッセージ組立：車両登録番号
        If Not (inRowVisitInfo.IsVCLREGNONull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.VCLREGNO)) Then
            '車両登録番号がある場合

            '車両登録番号を設定
            workContents.Append(inRowVisitInfo.VCLREGNO)

            'メッセージ間にスペースの設定
            workContents.Append(Space(3))
        End If

        'メッセージ組立：名前
        '名前の確認
        If Not (inRowVisitInfo.IsCST_NAMENull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.CST_NAME)) Then
            '名前がある場合

            '敬称と配置区分の確認
            If Not (inRowVisitInfo.IsNAMETITLE_NAMENull) _
                AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.NAMETITLE_NAME)) _
                AndAlso Not (inRowVisitInfo.IsPOSITION_TYPENull) _
                AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.POSITION_TYPE)) Then

                '敬称の配置判断
                If inRowVisitInfo.POSITION_TYPE.Equals("1") Then

                    '名前＋「様」を設定
                    workContents.Append(inRowVisitInfo.CST_NAME)
                    workContents.Append(inRowVisitInfo.NAMETITLE_NAME)

                ElseIf inRowVisitInfo.POSITION_TYPE.Equals("2") Then

                    '「様」＋名前を設定
                    workContents.Append(inRowVisitInfo.NAMETITLE_NAME)
                    workContents.Append(inRowVisitInfo.CST_NAME)
                End If

            Else
                '敬称を付けずに顧客氏名のみ表示
                workContents.Append(inRowVisitInfo.CST_NAME)
            End If

        Else
            '顧客氏名がNULLの場合、文言「お客様」を表示
            workContents.Append(customerWording)
        End If

        'Aタグ終了を設定
        workContents.Append(EndLikTag)

        '戻り値設定
        Dim notifyMessage As String = workContents.ToString()

        '開放処理
        workContents = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return notifyMessage

    End Function

    ''' <summary>
    ''' Push用メッセージ作成メソッド
    ''' </summary>
    ''' <param name="inRowVisitInfo">来店者情報欄表示情報</param>
    ''' <returns>作成したメッセージ文言</returns>
    ''' <history>
    ''' </history>
    ''' <remarks></remarks>
    Private Function CreatePusuMessage(ByVal inRowVisitInfo As SC3150102DataSet.SC3150102ScreenLinkageInfoRow) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workContents As New StringBuilder
        '文言「お客様」を設定
        Dim customerWording As String = WebWordUtility.GetWord(APPLICATION_ID, 30)
        '文言「整備完了」を設定
        workContents.Append(WebWordUtility.GetWord(APPLICATION_ID, 28))

        'メッセージ組立：RO番号
        If Not (inRowVisitInfo.IsRO_NUMNull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.RO_NUM)) Then
            'RO番号がある場合

            'メッセージ間にスペースの設定
            workContents.Append(Space(3))

            'RO番号を設定
            workContents.Append(inRowVisitInfo.RO_NUM)

        End If

        'メッセージ間にスペースの設定
        workContents.Append(Space(3))

        'メッセージ組立：車両登録番号
        If Not (inRowVisitInfo.IsVCLREGNONull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.VCLREGNO)) Then
            '車両登録番号がある場合

            '車両登録番号を設定
            workContents.Append(inRowVisitInfo.VCLREGNO)

            'メッセージ間にスペースの設定
            workContents.Append(Space(3))
        End If

        'メッセージ組立：名前
        '名前の確認
        If Not (inRowVisitInfo.IsCST_NAMENull) _
            AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.CST_NAME)) Then
            '名前がある場合

            '敬称と配置区分の確認
            If Not (inRowVisitInfo.IsNAMETITLE_NAMENull) _
                AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.NAMETITLE_NAME)) _
                AndAlso Not (inRowVisitInfo.IsPOSITION_TYPENull) _
                AndAlso Not (String.IsNullOrEmpty(inRowVisitInfo.POSITION_TYPE)) Then

                '敬称の配置判断
                If inRowVisitInfo.POSITION_TYPE.Equals("1") Then

                    '名前＋「様」を設定
                    workContents.Append(inRowVisitInfo.CST_NAME)
                    workContents.Append(inRowVisitInfo.NAMETITLE_NAME)

                ElseIf inRowVisitInfo.POSITION_TYPE.Equals("2") Then

                    '「様」＋名前を設定
                    workContents.Append(inRowVisitInfo.NAMETITLE_NAME)
                    workContents.Append(inRowVisitInfo.CST_NAME)
                End If

            Else
                '敬称を付けずに顧客氏名のみ表示
                workContents.Append(inRowVisitInfo.CST_NAME)
            End If

        Else
            '顧客氏名がNULLの場合、文言「お客様」を表示
            workContents.Append(customerWording)
        End If

        '戻り値設定
        Dim pushMessage As String = workContents.ToString()

        '開放処理
        workContents = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return pushMessage

    End Function

    '2013/08/08 TMEJ 成澤 【開発】IT9560_タブレット版SMB機能開発(工程管理) START
    ''' <summary>
    ''' 作業開始時のPush処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="staffCode">ログインスタッフコード</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Public Sub WorkStartSendPush(ByVal dealerCode As String, _
                                 ByVal branchCode As String, _
                                 ByVal staffCode As String, _
                                 ByVal stallId As Decimal, _
                                 ByVal stallUseId As Decimal)

        'スタッフ情報の取得
        Dim stuffCodeList As New List(Of Decimal)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        'stuffCodeList.Add(OperationCodeCT)
        'stuffCodeList.Add(OperationCodeChT)
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
        stuffCodeList.Add(OperationCodeSA)
        stuffCodeList.Add(OperationCodePS)
        'stuffCodeList.Add(OperationCodeFM)

        'アダプターのインスタンス宣言
        Using adapter As New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter

            '================================================================
            '                SA通知送信処理
            '================================================================

            'データセット宣言 
            Dim dtFirstWorkChip As SC3150102DataSet.SC3150102FirstWorkChipDataTable

            '担当SAコードと最初の開始チップの作業内容ID取得
            dtFirstWorkChip = adapter.GetFirstWorkChip(dealerCode, branchCode, stallUseId)

            If dtFirstWorkChip.Rows.Count > 0 Then

                Dim drFirstWorkChip As SC3150102DataSet.SC3150102FirstWorkChipRow =
                    DirectCast(dtFirstWorkChip.Rows(0), SC3150102DataSet.SC3150102FirstWorkChipRow)

                '開始したチップがリレーション内の最初開始の場合、通知する
                If (Not drFirstWorkChip.IsPIC_SA_STF_CDNull) AndAlso _
                    (Not String.IsNullOrEmpty(drFirstWorkChip.PIC_SA_STF_CD)) AndAlso _
                    (stallUseId = drFirstWorkChip.JOB_DTL_ID) Then

                    'SA権限の場合
                    TransmissionForCall(drFirstWorkChip.PIC_SA_STF_CD, saRefreshFunction)

                End If
            End If

            dtFirstWorkChip.Dispose()
            '================================================================
            '         　　　　　　　CT・ChT通知送信処理
            '================================================================
            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
            'SendPushChiefTechnician(staffCode, stallId)
            SendPushCtChtToStall(dealerCode, branchCode, staffCode, stallId)
            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

            '================================================================
            '                CT・PS通知送信処理
            '================================================================

            'オンラインユーザー情報の取得
            Dim utility As New VisitUtilityBusinessLogic
            Dim sendPushUsers As VisitUtilityUsersDataTable = _
                utility.GetOnlineUsers(dealerCode, branchCode, stuffCodeList)
            utility = Nothing

            '通知命令の送信
            For Each userRow As VisitUtilityUsersRow In sendPushUsers

                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                'If userRow.OPERATIONCODE.Equals(OperationCodeCT) Then

                '    'CT権限の場合
                '    TransmissionForCall(userRow.ACCOUNT, smbRefreshFunction)

                'ElseIf userRow.OPERATIONCODE.Equals(OperationCodePS) Then

                '    'PS権限の場合
                '    TransmissionForCall(userRow.ACCOUNT, psRefreshFunction)
                'End If

                'PS権限の場合
                If userRow.OPERATIONCODE.Equals(OperationCodePS) Then
                    TransmissionForCall(userRow.ACCOUNT, psRefreshFunction)
                End If
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
            Next

        End Using
    End Sub

    ''' <summary>
    ''' 作業終了時のPush処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="repairOrderNumber">RO番号</param>
    ''' <param name="jobDetailId">作業内容ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
    ''' </history>
    Public Sub WorkEndSendPush(ByVal dealerCode As String, _
                               ByVal branchCode As String, _
                               ByVal staffCode As String, _
                               ByVal repairOrderNumber As String, _
                               ByVal jobDetailId As Decimal, _
                               ByVal stallId As Decimal)

        'スタッフ情報の取得
        Dim stuffCodeList As New List(Of Decimal)

        '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

        'stuffCodeList.Add(OperationCodeCT)

        Dim utility As New VisitUtilityBusinessLogic

        '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

        'アダプターのインスタンス宣言
        Using adapter As New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter

            '================================================================
            '                SA通知送信処理
            '================================================================

            'データセット宣言
            Dim dtGatLastWorkChip As SC3150102DataSet.SC3150102GetLastWorkChipDataTable

            '最後の作業チップと着工指示フラグの立っていない整備数の取得
            dtGatLastWorkChip = adapter.GetLastWorkChip(dealerCode, branchCode, repairOrderNumber)

            If dtGatLastWorkChip.Rows.Count > 0 Then

                'データロウの宣言、データセットの格納
                Dim drGatLastWorkChip As SC3150102DataSet.SC3150102GetLastWorkChipRow = _
                    DirectCast(dtGatLastWorkChip.Rows(0), SC3150102DataSet.SC3150102GetLastWorkChipRow)

                '着工指示フラグが全て立っており、終了するチップが最後作業の場合、通知する
                If (Not drGatLastWorkChip.IsPIC_SA_STF_CDNull) AndAlso _
                    (Not String.IsNullOrEmpty(drGatLastWorkChip.PIC_SA_STF_CD)) AndAlso _
                    (drGatLastWorkChip.NO_FLG_COUNT = 0) AndAlso _
                    (jobDetailId = drGatLastWorkChip.JOB_DTL_ID) Then

                    'SA権限の場合
                    TransmissionForCall(drGatLastWorkChip.PIC_SA_STF_CD, saRefreshFunction)

                End If

                '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

                'サービスステータスのチェック
                If ServiceStetus_WaitingWashing.Equals(drGatLastWorkChip.SVC_STATUS) Then
                    '「07：洗車待ち」の場合

                    'CW権限を設定
                    stuffCodeList.Add(Operation.CW)

                    'オンラインのCW権限リスト取得
                    Dim dtSendCWUser As VisitUtilityUsersDataTable = utility.GetOnlineUsers(dealerCode, _
                                                                                            branchCode, _
                                                                                            stuffCodeList)

                    '取得件数分リフレッシュPushをする
                    For Each drSendCWUser As VisitUtilityUsersRow In dtSendCWUser
                        Me.TransmissionForCall(drSendCWUser.ACCOUNT, CWRefreshFunction)

                    Next

                    '権限リスト初期化
                    stuffCodeList.Clear()

                End If

                '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END


            End If

            dtGatLastWorkChip.Dispose()

            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
            '================================================================
            '         　　　　　　　CT/ChT通知送信処理
            '================================================================
            If NeedPushSubAreaRefresh Then
                ' サブエリアリフレッシュフラグがTureの場合
                ' すべてのユーザーにPush通知を送信する
                SendPushCtChtAll(dealerCode, branchCode, staffCode)
            Else
                ' サブエリアリフレッシュフラグがFalseの場合
                ' 対象ストールに紐づくユーザーのみPush通知を送信する
                SendPushCtChtToStall(dealerCode, branchCode, staffCode, stallId)
            End If

            ''================================================================
            ''                ChT通知送信処理
            ''================================================================

            'SendPushChiefTechnician(staffCode, stallId)

            ''================================================================
            ''                CT通知送信処理
            ''================================================================

            ''2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

            'stuffCodeList.Add(OperationCodeCT)

            ''2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

            ''オンラインユーザー情報の取得

            ''2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

            ''Dim utility As New VisitUtilityBusinessLogic

            ''2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

            'Dim sendPushUsers As VisitUtilityUsersDataTable = _
            '    utility.GetOnlineUsers(dealerCode, branchCode, stuffCodeList)
            'utility = Nothing

            ''来店通知命令の送信
            'For Each userRow As VisitUtilityUsersRow In sendPushUsers
            '    If userRow.OPERATIONCODE.Equals(OperationCodeCT) Then
            '        'CT権限の場合
            '        Me.TransmissionForCall(userRow.ACCOUNT, smbRefreshFunction)

            '    End If

            'Next

            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
        End Using

    End Sub

    ''' <summary>
    ''' チーフテクニシャンにPush送信処理
    ''' </summary>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Private Sub SendPushChiefTechnician(ByVal staffCode As String, ByVal stallId As Decimal)

        Using adapter As New SC3150102DataSetTableAdapters.SC3150102StallInfoDataTableAdapter

            'データセット宣言 
            Dim dtChtAccount As SC3150102DataSet.SC3150102ChtStaffCodeDataTable

            'チーフテクニシャンのスタッフコード取得 
            dtChtAccount = adapter.GetChtTechnicianAccount(stallId)

            '取得したチーフテクニシャン分繰り返す
            For Each drChtAccount As SC3150102DataSet.SC3150102ChtStaffCodeRow In dtChtAccount

                '自分以外のチーフテクニシャンに通知する
                If (Not drChtAccount.IsSTF_CDNull) AndAlso _
                    (Not String.IsNullOrEmpty(drChtAccount.STF_CD)) AndAlso
                    (Not staffCode.Equals(drChtAccount.STF_CD)) Then
                    TransmissionForCall(drChtAccount.STF_CD, smbRefreshFunction)
                End If
            Next

            dtChtAccount.Dispose()
        End Using
    End Sub

    ''' <summary>
    ''' Push送信処理
    ''' </summary>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <remarks></remarks>
    Private Sub TransmissionForCall(ByVal staffCode As String, ByVal refreshFunction As String)

        'POST送信メッセージの作成
        Dim postSendMessage As New StringBuilder
        postSendMessage.Append("cat=action")
        postSendMessage.Append("&type=main")
        postSendMessage.Append("&sub=js")
        postSendMessage.Append("&uid=" & staffCode)
        postSendMessage.Append("&time=0")
        postSendMessage.Append("&js1=" & refreshFunction)

        '送信処理
        Dim visitUtility As New VisitUtility
        visitUtility.SendPush(postSendMessage.ToString)

    End Sub

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>
    ''' CT/CHTへのPush処理(ストールに紐づくユーザーのみ)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="staffCode">ログインスタッフコード</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Private Sub SendPushCtChtToStall(ByVal dealerCode As String, _
                                     ByVal branchCode As String, _
                                     ByVal staffCode As String, _
                                     ByVal stallId As Decimal)

        Using serviceCommonbiz As New ServiceCommonClassBusinessLogic
            ' ストールIDのリスト生成
            Dim stallIdList As List(Of Decimal) = New List(Of Decimal)
            stallIdList.Add(stallId)

            ' 権限コードリスト生成(CT・ChT)
            Dim stuffCodeList As New List(Of Decimal)
            stuffCodeList.Add(OperationCodeCT)
            stuffCodeList.Add(OperationCodeChT)

            ' ストールIDよりPush通知アカウントリスト取得
            Dim staffInfoDataTable As ServiceCommonClassDataSet.StaffInfoDataTable
            staffInfoDataTable = serviceCommonbiz.GetNoticeSendAccountListToStall(dealerCode, branchCode, stallIdList, stuffCodeList)

            For Each row As ServiceCommonClassDataSet.StaffInfoRow In staffInfoDataTable.Rows
                ' 自分以外の場合Push送信する
                If Not String.Equals(row.ACCOUNT, staffCode) Then
                    TransmissionForCall(row.ACCOUNT, smbRefreshFunction)
                End If
            Next
        End Using
    End Sub

    ''' <summary>
    ''' CT/CHTへのPush処理(全て)
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="staffCode">ログインスタッフコード</param>
    ''' <remarks></remarks>
    Private Sub SendPushCtChtAll(ByVal dealerCode As String, _
                                 ByVal branchCode As String, _
                                 ByVal staffCode As String)

        Dim utility As New VisitUtilityBusinessLogic

        ' 権限コードリスト生成(CT・ChT)
        Dim stuffCodeList As New List(Of Decimal)
        stuffCodeList.Add(OperationCodeCT)
        stuffCodeList.Add(OperationCodeChT)

        ' オンラインユーザーの取得
        Dim sendPushUsers As VisitUtilityUsersDataTable = _
        utility.GetOnlineUsers(dealerCode, branchCode, stuffCodeList)
        utility = Nothing

        For Each row As VisitUtilityUsersRow In sendPushUsers.Rows
            ' 自分以外の場合Push送信する
            If Not String.Equals(row.ACCOUNT, staffCode) Then
                TransmissionForCall(row.ACCOUNT, smbRefreshFunction)
            End If
        Next
    End Sub
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    '2014/07/23 TMEJ 成澤　【開発】IT9711_タブレットSMB Job Dispatch機能開発 END
End Class
