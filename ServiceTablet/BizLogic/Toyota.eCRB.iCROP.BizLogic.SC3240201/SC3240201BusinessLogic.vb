'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240201BusinessLogic.vb
'─────────────────────────────────────
'機能： チップ詳細
'補足： 
'作成： 2013/07/31 TMEJ 岩城 タブレット版SMB機能開発(工程管理)
'更新： 2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新： 2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発
'更新： 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
'更新： 2014/08/19 TMEJ 明瀬 NoShowエリアのチップが消える不具合対応
'更新： 2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致
'更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'更新： 2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発
'更新： 2015/04/01 TMEJ 小澤 BTS-261対応 サービス名の表示制御の修正
'更新： 2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
'更新： 2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力
'更新： 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新： 2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新： 2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
'更新： 2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001
'更新： 2016/10/14 NSK 秋田谷 TR-SVT-TMT-20160824-003（チップ詳細の実績時間を変更できなくする）の対応
'更新： 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
'更新： 2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない
'更新： 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新： 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/07/19 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新： 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Reflection
Imports System.Text
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SMB.ChipDetail.DataAccess
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic.SMBCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess
'2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.BizLogic.IC3800703
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800703.IC3800703DataSet
'Imports Toyota.eCRB.DMSLinkage.Reserve.BizLogic.IC3800902
'Imports Toyota.eCRB.DMSLinkage.Reserve.DataAccess.IC3800902
'Imports Toyota.eCRB.DMSLinkage.ChangRepairOrder.BizLogic.IC3801503
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801015
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801015
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801016
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801016
'2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
'2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
Imports Toyota.eCRB.DMSLinkage.Reserve.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.DataAccess
'2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END
'2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
Imports Toyota.eCRB.DMSLinkage.StatusInfo.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSetTableAdapters

'2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

''' <summary>
''' チップ詳細
''' ビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public Class SC3240201BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "SC3240201"

    ''' <summary>
    ''' サブチップボックス:受付ボタンID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBAREA_RECEPTION As String = "5"

    ''' <summary>
    ''' サブチップボックス:追加作業ボタンID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBAREA_ADDWORK As String = "15"

    ''' <summary>
    ''' サブチップボックス:NoShowボタンID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBAREA_NOSHOW As String = "20"

    ''' <summary>
    ''' サブチップボックス:中断ボタンID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBAREA_STOP As String = "21"

    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' サブチップボックス:完成検査ボタンID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBAREA_CONFIRMED_INSPECTION As String = "14"

    ''' <summary>
    ''' サブチップボックス:洗車開始待ちボタンID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBAREA_WAITING_WASH As String = "16"

    ''' <summary>
    ''' サブチップボックス:洗車中ボタンID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBAREA_WASHING As String = "17"

    ''' <summary>
    ''' サブチップボックス:納車待ちボタンID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBAREA_WAIT_DELIVERY As String = "18"

    ''' <summary>
    ''' チップエリア:ストール
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_AREA_TYPE_STALL As Integer = 1

    ''' <summary>
    ''' チップエリア:受付
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_AREA_TYPE_RECEPTION As Integer = 2

    ''' <summary>
    ''' チップエリア:追加作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_AREA_TYPE_ADDWORK As Integer = 3

    ''' <summary>
    ''' チップエリア:完成検査
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_AREA_TYPE_CONFIRMED_INSPECTION As Integer = 4

    ''' <summary>
    ''' チップエリア:洗車
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_AREA_TYPE_WASH As Integer = 5

    ''' <summary>
    ''' チップエリア:納車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_AREA_TYPE_WAIT_DELIVERY As Integer = 6

    ''' <summary>
    ''' チップエリア:中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_AREA_TYPE_STOP As Integer = 7

    ''' <summary>
    ''' チップエリア:NoShow
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_AREA_TYPE_NOSHOW As Integer = 8
    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 最小日付
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_MIN_VALUE As String = "1900/01/01 00:00:00"

    ''' <summary>
    ''' 来店無し
    ''' </summary>
    Private Const NOVISIT As String = "0"

    ''' <summary>
    ''' 来店有り
    ''' </summary>
    Private Const VISIT As String = "1"

    ''' <summary>
    ''' ストール利用ステータス"00":着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_USE_STATUS_00 As String = "00"

    ''' <summary>
    ''' ストール利用ステータス"01":作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_USE_STATUS_01 As String = "01"

    '2019/07/19 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ''' <summary>
    ''' ストール利用ステータス"02":作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_USE_STATUS_02 As String = "02"

    ''' <summary>
    ''' ストール利用ステータス"04":作業指示の一部の作業が中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_USE_STATUS_04 As String = "04"

    ''' <summary>
    ''' ストール利用ステータス"07":未来店客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_USE_STATUS_07 As String = "07"
    '2019/07/19 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

    ''' <summary>
    ''' 着工指示済みでない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INSTRUCT_0 As String = "0"

    ''' <summary>
    ''' 着工指示済みである
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INSTRUCT_1 As String = "1"

    ''' <summary>
    ''' 顧客車両区分(1：所有者)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OWNER As String = "1"

    ''' <summary>
    ''' 休憩取得フラグ（0：休憩を取得しない）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOT_USE_REST As String = "0"

    ''' <summary>
    ''' 休憩取得フラグ（1：休憩を取得する）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const USE_REST As String = "1"

    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' プッシュ送信用引数
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const PUSH_FUNTIONNAME As String = "CallPushEvent()"  'プッシュ送信で呼び出されるJSメソッド名

    ''' <summary>
    ''' プッシュ送信用引数(TC用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_FUNTIONNAME_TC As String = "CallPushEvent()"  'プッシュ送信で呼び出されるJSメソッド名

    ''' <summary>
    ''' プッシュ送信用引数(CT_ChT用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_FUNTIONNAME_CT_CHT As String = "RefreshSMB()"  'プッシュ送信で呼び出されるJSメソッド名

    ''' <summary>
    ''' プッシュ送信用引数(PS用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_FUNTIONNAME_PS As String = "MainRefresh()"  'プッシュ送信で呼び出されるJSメソッド名

    ''' <summary>
    ''' 敬称配置区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const POSITION_TYPE_FORWORD As String = "2"             '名称の前

    ''' <summary>
    ''' 日付フォーマット:yyyy/MM/dd
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FORMAT_DATE_YYYYMMDD As String = "yyyy/MM/dd"

    ''' <summary>
    ''' 日付フォーマット:yyyy/MM/dd HH:mm
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FORMAT_DATE_YYYYMMDDHHMM As String = "yyyy/MM/dd HH:mm"

    ''' <summary>
    ''' 日付フォーマット(システム設定値)
    ''' </summary>
    Private Const SYSDATEFORMAT = "DATE_FORMAT"
    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' WebService行ロックバージョンエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WEBSERVICE_ROWLOCKVERSION_ERROR As Long = 6014

    ''' <summary>
    ''' ハイフン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HYPHEN As String = "-"

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 全開始処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ALLSTART As String = "AllStart"

    ''' <summary>
    ''' 全終了処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ALLFINISH As String = "AllFinish"

    ''' <summary>
    ''' 全中断処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ALLSTOP As String = "AllStop"

    ''' <summary>
    ''' 単独中断処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SINGLESTOP As String = "SingleStop"

    ''' <summary>
    ''' 単独開始処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SINGLESTART As String = "SingleStart"

    ''' <summary>
    ''' 再開始処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESTART As String = "ReStart"

    ''' <summary>
    ''' 単独終了処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SINGLEFINISH As String = "SingleFinish"
    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 次のチップステータス(変わってない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEXTCHIPSTATUS_NOCHANGE As Integer = 0

    ''' <summary>
    ''' 次のチップステータス(中断に変える)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEXTCHIPSTATUS_CHANGETOSTOP As Integer = 1

    ''' <summary>
    ''' 次のチップステータス(終了に変える)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEXTCHIPSTATUS_CHANGETOFINISH As Integer = 2

    ''' <summary>開始後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushAfterStartSingleJob As Boolean = False

    ''' <summary>終了後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushAfterFinishSingleJob As Boolean = False

    ''' <summary>中断後、Push送信やる必要フラグ (True:Push送信 False:Push送信しない)</summary>
    Public Property NeedPushAfterStopSingleJob As Boolean = False

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>着工指示タイプ(すべてのJobが着工指示前)</summary>
    Public Property JobInsTypeAllJobBefore As Long = 0

    ''' <summary>着工指示タイプ(すべてのJobが着工指示済み)</summary>
    Public Property JobInsTypeAllJobAlready As Long = 1

    ''' <summary>着工指示タイプ(一部のJobが着工指示済み)</summary>
    Public Property JobInsTypePartJobAlready As Long = 2
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START 

    ''' <summary>
    ''' 仮置きフラグ"0":仮置きでない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOT_TEMP As String = "0"

    ''' <summary>
    ''' ストール利用ID:デフォルト値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STALL_USE_ID_ZERO As Long = 0

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    ''' <summary>
    ''' 販売店システム設定名:休憩取得自動判定フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RestAutoJudgeFlg = "REST_AUTO_JUDGE_FLG"
    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

#End Region

#Region "列挙体"

    ''' <summary>
    ''' DateTimeFunc.FormatDateメソッドで使用するフォーマット番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum Format As Integer

        ''' <summary>
        ''' yyyy/MM/dd HH:mm:ss
        ''' </summary>
        ''' <remarks></remarks>
        yyyyMMddHHmmss = 1

        ''' <summary>
        ''' yyyy/MM/dd HH:mm
        ''' </summary>
        ''' <remarks></remarks>
        yyyyMMddHHmm = 2

        ''' <summary>
        ''' yyyy/MM/dd
        ''' </summary>
        ''' <remarks></remarks>
        yyyyMMdd = 3

        ''' <summary>
        ''' MM:dd
        ''' </summary>
        ''' <remarks></remarks>
        MMdd = 11

        ''' <summary>
        ''' HH:mm
        ''' </summary>
        ''' <remarks></remarks>
        HHmm = 14

    End Enum
#End Region

#Region "プロパティ"
    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    Private Property instructStatusList As Dictionary(Of Decimal, String)
    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
    Public Property NewStallIdleId As Decimal
    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

#End Region

#Region "メンバ変数"

    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START

    ''' <summary>
    ''' ストールロックフラグ
    ''' </summary>
    ''' <remarks>
    ''' True ：ストールロックを実施
    ''' False：ストールロックを未実施
    ''' </remarks>
    Private IsLockStall As Boolean

    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

#End Region

#Region "Publicメソッド"

#Region "初期表示情報取得処理"

    ''' <summary>
    ''' 初期表示情報を取得する
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInitInfo(ByVal arg As CallBackArgumentClass) As SC3240201DataSet

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[DlrCD:{0}][StrCD:{1}][SvcInId:{2}][StallUseId:{3}][SrvAddSeq:{4}][SubAreaId:{5}][ShowDate:{6}][ROJobSeq:{7}][RONum:{8}]", _
                      arg.DlrCD, arg.StrCD, arg.SvcInId, arg.StallUseId, arg.SrvAddSeq, arg.SubAreaId, arg.ShowDate, arg.ROJobSeq, arg.RONum)

        '返却用データセット生成
        Using rtnDs As New SC3240201DataSet

            Dim ta As New SC3240201TableAdapter
            Dim smbCommonBizLogic As New SMBCommonClassBusinessLogic
            Dim smbCommonChipDetails As CommonUtility.SMBCommonClass.Api.BizLogic.ChipDetail = Nothing

            Try
                Dim chipBaseDt As SC3240201DataSet.SC3240201ChipBaseInfoDataTable
                Dim dispChipRow As SC3240201DataSet.SC3240201DispChipBaseInfoRow

                'チップ情報を取得
                If (SUBAREA_RECEPTION.Equals(arg.SubAreaId) Or SUBAREA_ADDWORK.Equals(arg.SubAreaId)) Then
                    '受付・追加作業エリアの場合

                    'サービス入庫IDに紐付くチップ情報を取得
                    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    'chipBaseDt = ta.GetChipBaseInfoBySvcInId(arg.SvcInId)
                    chipBaseDt = ta.GetChipBaseInfoBySvcInId(arg.SvcInId, arg.RONum)
                    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                Else

                    'ストール利用IDに紐付くチップ情報を取得
                    chipBaseDt = ta.GetChipBaseInfo(arg.DlrCD, arg.StrCD, arg.StallUseId)
                End If

                '他ユーザーによってチップが削除された等、異常発生
                If chipBaseDt.Rows.Count <= 0 Then
                    Return Nothing
                End If

                'チップ情報を画面表示用に編集し、返却用データ行に格納する
                dispChipRow = Me.SetDisplayChipData(rtnDs, chipBaseDt, arg)

                'サービス入庫IDに紐付く来店情報を取得
                Dim visitDt As SC3240201DataSet.SC3240201VisitInfoDataTable
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'Dim visitFlag As String
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                visitDt = ta.GetVisitInfo(arg.DlrCD, arg.StrCD, arg.SvcInId)

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                ''来店有無のフラグをセット
                'If visitDt.Rows.Count <= 0 Then
                '    '来店情報がない場合

                '    visitFlag = NOVISIT     '来店無し
                'Else

                '    visitFlag = VISIT       '来店有り
                'End If
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                Dim roJobSeq As Long = -1    'RO作業連番
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'Dim addSeq As Long = 0       '枝番
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                Dim stallUseStatus As String 'ストール利用ステータス
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'Dim stallUseId As Long       'ストール利用ID
                Dim stallUseId As Decimal       'ストール利用ID
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '枝番・RO作業連番・ストール利用ステータス、ストール利用IDをセット
                If (SUBAREA_RECEPTION.Equals(arg.SubAreaId) Or SUBAREA_ADDWORK.Equals(arg.SubAreaId)) Then
                    '受付・追加作業エリアの場合

                    'サブチップが保持しているRO作業連番が存在する場合
                    If Not IsNothing(arg.ROJobSeq) Then
                        roJobSeq = CLng(arg.ROJobSeq)
                    End If

                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    ''サブチップが保持している枝番が1以上の場合（＝追加作業の場合）
                    'If Not IsNothing(arg.SrvAddSeq) AndAlso CLng(arg.SrvAddSeq) >= 1 Then
                    '    addSeq = CLng(arg.SrvAddSeq)
                    'End If
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                    'ストール利用ステータスに、「00:着工指示待ち」をセット
                    stallUseStatus = STALL_USE_STATUS_00

                    'ストール利用IDに、-1 をセット
                    stallUseId = -1
                Else
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    'DBから取得したRO作業連番をセット
                    'roJobSeq = CLng(chipBaseDt.Rows(0)("RO_JOB_SEQ"))
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                    'DBから取得したストール利用ステータスをセット
                    stallUseStatus = ConvertDbNullToEmpty(chipBaseDt.Rows(0)("STALL_USE_STATUS"))

                    'チップのストール利用IDをセット
                    stallUseId = arg.StallUseId
                End If

                '共通クラスから必要な情報を取得
                'smbCommonChipDetails = smbCommonBizLogic.GetSmbChipDetail(arg.DlrCD, _
                '                                          arg.StrCD, _
                '                                          arg.SvcInId, _
                '                                          visitFlag, _
                '                                          ConvertDbMinDateToNull(chipBaseDt.Rows(0)("RESULT_STARTDATE")), _
                '                                          ConvertDbMinDateToNull(chipBaseDt.Rows(0)("RESULT_ENDDATE")), _
                '                                          CStr(chipBaseDt.Rows(0)("WASHFLAG")), _
                '                                          ConvertDbNullToEmpty(chipBaseDt.Rows(0)("RO_NUM")), _
                '                                          ConvertDbNullToEmpty(chipBaseDt.Rows(0)("INSPECTION_STATUS")), _
                '                                          stallUseStatus, _
                '                                          roJobSeq, _
                '                                          addSeq, _
                '                                          ConvertDbMinDateToNull(chipBaseDt.Rows(0)("RESULT_DELIDATE")), _
                '                                          stallUseId)
                '共通クラスから必要な情報を取得
                Using clsSmbChipDetailInputInfo As New SMBCommonClassDataSet.SmbChipDetailInputInfoDataTable
                    Dim dr As SMBCommonClassDataSet.SmbChipDetailInputInfoRow = clsSmbChipDetailInputInfo.NewSmbChipDetailInputInfoRow

                    SetCommonClassInputVal(arg, dr, chipBaseDt, visitDt, stallUseStatus, stallUseId)
                    smbCommonChipDetails = smbCommonBizLogic.GetSmbChipDetail(dr)
                End Using

                '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                '顧客車両区分を付与した顧客氏名を取得(オーバーロード用)
                Using GetCstNameAddCstVclType As New ServiceCommonClassBusinessLogic

                    '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
                    '同一顧客ID・車両IDの場合でも予約取得時の顧客車両区分が表示されるようにするため、サービス入庫．顧客車両区分を引数に追加
                    'Dim addCstName As String = GetCstNameAddCstVclType.GetCstNameWithCstVclType(arg.DlrCD, _
                    '                                                                            dispChipRow.CST_ID, _
                    '                                                                            dispChipRow.VCL_ID)
                    Dim addCstName As String = GetCstNameAddCstVclType.GetCstNameWithCstVclType(arg.DlrCD, _
                                                                                                dispChipRow.CST_ID, _
                                                                                                dispChipRow.VCL_ID, _
                                                                                                smbCommonChipDetails.CustomerVehicleType)
                    '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END

                    If String.IsNullOrEmpty(addCstName) Then
                        'エラーログ
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1} .Error GetCstNameWithCstVclType Is Empty DLR_CD = {2} CST_ID = {3} VCL_ID = {4}", _
                                  Me.GetType.ToString, _
                                  Reflection.MethodBase.GetCurrentMethod.Name, _
                                  arg.DlrCD, _
                                  dispChipRow.CST_ID, _
                                  dispChipRow.VCL_ID))

                    End If

                    '取得したチップ詳細情報・顧客情報を編集し、返却用データセット顧客氏名にローカル.顧客氏名を格納する
                    dispChipRow.CST_NAME = addCstName

                End Using

                '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
                ''IC3800703(顧客参照)
                'Dim iC3800703bl As IC3800703BusinessLogic = New IC3800703BusinessLogic
                'Dim customerDt As IC3800703SrvCustomerDataTable = Nothing

                ''「JDP調査対象客マーク」、「SSCマーク」の取得
                'customerDt = iC3800703bl.GetCustomerInfo(CStr(chipBaseDt.Rows(0)("REG_NUM")), _
                '                                         CStr(chipBaseDt.Rows(0)("VCL_VIN")), _
                '                                         arg.DlrCD)

                'OutPutIFLog(customerDt, "IC3800703BizLogic.GetCustomerInfo")

                ''上記で取得した情報を画面表示用に編集し、返却用データセットに格納する
                'Me.SetDispOtherData(rtnDs, customerDt, visitDt, smbCommonChipDetails, arg, dispChipRow)
                '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                'Me.SetDispOtherData(rtnDs, _
                '                    chipBaseDt.Item(0).JDP_FLG, _
                '                    visitDt, _
                '                    smbCommonChipDetails, _
                '                    arg, _
                '                    dispChipRow)
                Me.SetDispOtherData(rtnDs, _
                                    chipBaseDt.Item(0).JDP_FLG, _
                                    visitDt, _
                                    smbCommonChipDetails, _
                                    arg, _
                    dispChipRow, _
                    chipBaseDt.Item(0).SSC_MARK)
                '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

                'RO番号のチェック（RO発行前／RO発行後の判断）
                Dim roNum As String = CStr(chipBaseDt.Rows(0)("RO_NUM"))
                Dim hasRoNum As Boolean = False
                If Not (String.IsNullOrEmpty(roNum.Trim())) Then
                    hasRoNum = True   'RO番号有り（RO発行後）
                End If

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'RO番号がある場合、かつ、追加作業エリア以外の場合   
                'If (hasRoNum = True) Then
                If (hasRoNum = True) AndAlso Not (SUBAREA_ADDWORK.Equals(arg.SubAreaId)) Then

                    '整備情報／部品情報を画面表示用に編集し、返却用データセットに格納する
                    Me.SetMaintePartsData(rtnDs, ta, arg, roJobSeq)

                End If
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '受付・追加作業エリア以外の場合
                If Not (SUBAREA_RECEPTION.Equals(arg.SubAreaId) Or SUBAREA_ADDWORK.Equals(arg.SubAreaId)) Then

                    '整備種類／整備名称を画面表示用に編集し、返却用データセットに格納する
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    'Me.SetSvcMercData(rtnDs, ta, arg.DlrCD, arg.StrCD, CType(chipBaseDt.Rows(0)("SVC_CLASS_ID"), Long))
                    Me.SetSvcMercData(rtnDs, ta, arg.DlrCD, arg.StrCD, CType(chipBaseDt.Rows(0)("SVC_CLASS_ID"), Decimal))
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                End If

                '共通クラスのチップ情報がある場合
                If smbCommonChipDetails IsNot Nothing Then

                    '納車時刻変更履歴情報がある場合
                    If smbCommonChipDetails.DeliveryChgList IsNot Nothing AndAlso 0 < smbCommonChipDetails.DeliveryChgList.Count Then

                        '納車時刻変更履歴情報を画面表示用に編集し、返却用データセットに格納する
                        Me.SetDeliChangeData(rtnDs, smbCommonChipDetails, arg.ShowDate)

                    End If

                    '中断理由情報がある場合
                    If smbCommonChipDetails.StopReasonList IsNot Nothing AndAlso 0 < smbCommonChipDetails.StopReasonList.Count Then

                        '中断理由情報を画面表示用に編集し、返却用データセットに格納する
                        Me.SetInterruptionData(rtnDs, smbCommonChipDetails)

                    End If
                End If

                OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

                Return rtnDs

            Finally
                ta = Nothing
                If smbCommonBizLogic IsNot Nothing Then
                    smbCommonBizLogic.Dispose()
                    smbCommonBizLogic = Nothing
                End If
            End Try

        End Using

    End Function

    ''' <summary>
    ''' 商品情報取得
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns></returns>
    Public Function GetChangeMercInfo(ByVal arg As CallBackArgumentClass) As SC3240201DataSet.SC3240201MercListDataTable

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[DlrCD:{0}][StrCD:{1}][SvcClassId:{2}]", _
                      arg.DlrCD, arg.StrCD, arg.SvcClassId)

        Dim ta As New SC3240201TableAdapter

        Try
            Dim mercDt As SC3240201DataSet.SC3240201MercListDataTable = ta.GetMercList(arg.DlrCD, arg.StrCD, arg.SvcClassId)

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

            Return mercDt
        Finally
            ta = Nothing
        End Try

    End Function


    ''' <summary>
    '''   DataTableをJSON文字列に変換する
    ''' </summary>
    ''' <param name="dataTable">変換対象 DataSet</param>
    ''' <returns>JSON文字列</returns>
    Public Function ChipDetailDataTableToJson(ByVal dataTable As DataTable) As String

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim resultMain As New Dictionary(Of String, Object)
        Dim JSerializer As New JavaScriptSerializer

        If dataTable Is Nothing Then
            Return JSerializer.Serialize(resultMain)
        End If

        For Each dr As DataRow In dataTable.Rows
            Dim result As New Dictionary(Of String, Object)

            For Each dc As DataColumn In dataTable.Columns
                result.Add(dc.ColumnName, dr(dc).ToString)
            Next
            resultMain.Add("Key" + CType(resultMain.Count + 1, String), result)
        Next

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        Return JSerializer.Serialize(resultMain)

    End Function
#End Region

#Region "DB更新処理"

    ''' <summary>
    ''' データを更新する
    ''' （予約情報更新WebServiceを利用せずにDB更新）
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="updDate">更新日時</param>
    ''' <param name="startPlanTime">更新用の予定開始日時</param>
    ''' <param name="finishPlanTime">更新用の予定終了日時</param>
    ''' <param name="prmsEndTime">更新用の見込終了日時</param>
    ''' <param name="procTime">更新用の実績時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateData(ByVal arg As CallBackArgumentClass, _
                               ByVal updDate As Date, _
                               ByVal startPlanTime As Date, _
                               ByVal finishPlanTime As Date, _
                               ByVal prmsEndTime As Date, _
                               ByVal procTime As Long) As Long

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[updDate:{0}][startPlanTime:{1}][finishPlanTime:{2}][StallId:{3}][ChipDispStartDate:{4}][Account:{5}][SvcInId:{6}][RowLockVersion:{7}][DlrCD:{8}][StrCD:{9}][prmsEndTime:{10}][procTime:{11}]", _
                      updDate, startPlanTime, finishPlanTime, arg.StallId, arg.ChipDispStartDate, arg.Account, arg.SvcInId, arg.RowLockVersion, arg.DlrCD, arg.StrCD, prmsEndTime, procTime)

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START

        ''ストールロックフラグ
        'Dim isStallLock As Boolean = False

        'ストールロックフラグ(初期値はFalse(ストールロック未実施))
        IsLockStall = False

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

        Dim ta As New SC3240201TableAdapter
        Dim resultCD As Long = 0
        Dim prevChipStatus As String = String.Empty       '変更前のチップステータス
        Dim crntChipStatus As String = String.Empty       '変更後のチップステータス
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        instructStatusList = Nothing
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START
        'メソッドの戻り値
        Dim retValue As Long = 0
        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            Try

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                'このクラスのPushフラグを初期化
                Me.ResetPushFlg(clsTabletSMBCommonClass)
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START

                ''受付・追加作業・NoShow・中断エリア以外の場合
                'If Not (SUBAREA_RECEPTION.Equals(arg.SubAreaId) Or SUBAREA_ADDWORK.Equals(arg.SubAreaId) Or _
                '        SUBAREA_NOSHOW.Equals(arg.SubAreaId) Or SUBAREA_STOP.Equals(arg.SubAreaId)) Then

                '    'ストールロック
                '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                '    'resultCD = clsTabletSMBCommonClass.LockStall(CLng(arg.StallId), arg.ChipDispStartDate, arg.Account, updDate, MY_PROGRAMID)
                '    resultCD = clsTabletSMBCommonClass.LockStall(CType(arg.StallId, Decimal), arg.ChipDispStartDate, arg.Account, updDate, MY_PROGRAMID)
                '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '    If resultCD <> ActionResult.Success Then
                '        Me.Rollback = True
                '        Return resultCD
                '    End If
                '    isStallLock = True
                'End If

                'ストールロック処理
                Dim resultLockStall As Long = Me.LockStall(clsTabletSMBCommonClass, _
                                                           arg.SubAreaId, _
                                                           CType(arg.StallId, Decimal), _
                                                           arg.ChipDispStartDate, _
                                                           arg.Account, _
                                                           updDate)

                If resultLockStall <> ActionResult.Success Then
                    'ストールロック処理が失敗した場合

                    Me.Rollback = True
                    Return resultLockStall

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

                'サービス入庫をロックして、チェックする
                resultCD = clsTabletSMBCommonClass.LockServiceInTable(arg.SvcInId, arg.RowLockVersion, arg.Account, updDate, MY_PROGRAMID)
                If resultCD <> ActionResult.Success Then
                    Me.Rollback = True
                    Return resultCD
                End If

                '変更前の情報を取得する
                Dim dtServiceinBefore As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable
                dtServiceinBefore = clsTabletSMBCommonClass.GetChipChangeInfo(arg.SvcInId, arg.DlrCD, arg.StrCD)

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                'Jobが変わったフラグ(false：変わってない)
                Dim changeJobFlg As Boolean = Me.HasChangeJobNum(arg, dtServiceinBefore)
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                '●変更前のチップステータスを取得する
                Dim callFlag As Boolean
                callFlag = IsCallWebService(arg)
                If (callFlag) Then
                    '予約送信を行う場合

                    prevChipStatus = clsTabletSMBCommonClass.JudgeChipStatus(arg.StallUseId)
                End If

                'サービス入庫テーブルの更新
                resultCD = ta.UpdServiceIn(arg)
                If resultCD <= 0 Then
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} SC3240201TableAdapter.UpdServiceIn SvcInId={2}" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
                               , arg.SvcInId))
                    Return -1
                End If

                '受付・追加作業エリア以外の場合
                If Not (SUBAREA_RECEPTION.Equals(arg.SubAreaId) Or SUBAREA_ADDWORK.Equals(arg.SubAreaId)) Then

                    '作業内容テーブルの更新
                    resultCD = ta.UpdJobDtl(arg, updDate)
                    If resultCD <= 0 Then
                        Me.Rollback = True
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} SC3240201TableAdapter.UpdJobDtl JobDtlId={2}" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                   , arg.JobDtlId))
                        Return -1
                    End If

                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    ''ストール利用テーブルの更新
                    'resultCD = ta.UpdStallUse(arg, updDate, startPlanTime, finishPlanTime, prmsEndTime, procTime)

                    Using dealerEnvBiz As New ServiceCommonClassBusinessLogic
                        '休憩取得自動判定フラグ
                        Dim autoJudgeFlg = String.Empty
                        autoJudgeFlg = dealerEnvBiz.GetDlrSystemSettingValueBySettingName(RestAutoJudgeFlg)

                        'ストール利用テーブルの更新
                        resultCD = ta.UpdStallUse(arg, updDate, startPlanTime, finishPlanTime, prmsEndTime, procTime, autoJudgeFlg)
                    End Using
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                    If resultCD <= 0 Then
                        Me.Rollback = True
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1} SC3240201TableAdapter.UpdStallUse StallUseId={2}" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                   , arg.StallUseId))
                        Return -1
                    End If
                End If

                'その他の更新（RO作業連番、ストール利用ステータスの更新(リレーションチップ含む)）
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'If Not (String.IsNullOrEmpty(arg.RONum.Trim())) Then
                '    'RO番号がある場合

                '    Me.UpdateOtherData(arg, updDate, ta)
                'End If

                If Not (String.IsNullOrEmpty(arg.RONum.Trim())) AndAlso Not SUBAREA_ADDWORK.Equals(arg.SubAreaId) Then
                    'RO番号がある場合、かつ、追加作業以外の場合

                    instructStatusList = Me.UpdateOtherData(arg, updDate, ta)
                End If
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '変更後の情報を取得する
                Dim dtServiceinAfter As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable
                dtServiceinAfter = clsTabletSMBCommonClass.GetChipChangeInfo(arg.SvcInId, arg.DlrCD, arg.StrCD)

                '●変更後のチップステータスを取得する
                If (callFlag) Then
                    '予約送信を行う場合

                    crntChipStatus = clsTabletSMBCommonClass.JudgeChipStatus(arg.StallUseId)
                End If

                '履歴登録
                clsTabletSMBCommonClass.CreateChipOperationHistory(dtServiceinBefore, dtServiceinAfter, updDate, arg.Account, 0, MY_PROGRAMID)

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                ''着工指示・RO関連項目データの送信
                'If Not (String.IsNullOrEmpty(arg.RONum.Trim())) Then
                '    'RO番号がある場合のみ

                '着工指示の送信
                If Not (String.IsNullOrEmpty(arg.RONum.Trim())) AndAlso Not SUBAREA_ADDWORK.Equals(arg.SubAreaId) Then
                    'RO番号がある場合、かつ、追加作業以外の場合
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                    '着工指示データの送信
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    'resultCD = Me.SendInstruct(arg, clsTabletSMBCommonClass, ta)
                    resultCD = Me.SendInstruct(arg, ta, updDate)
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                    If resultCD <> ActionResult.Success Then
                        Me.Rollback = True
                        Return resultCD
                    End If

                    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
                    ''RO関連項目の送信
                    'resultCD = Me.SendROInfo(arg)
                    'If resultCD <> ActionResult.Success Then
                    '    Me.Rollback = True
                    '    Return resultCD
                    'End If
                    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END
                End If

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                '次のチップステータスを0で初期化(変更しない)
                Dim nextChipStatus As Integer = NEXTCHIPSTATUS_NOCHANGE

                'Job数が変わった場合
                If changeJobFlg = True Then

                    '次のチップステータスを取得
                    nextChipStatus = GetNextChipStatus(dtServiceinAfter)

                End If

                '2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
                If nextChipStatus = NEXTCHIPSTATUS_CHANGETOFINISH _
                    Or nextChipStatus = NEXTCHIPSTATUS_CHANGETOSTOP Then
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                               , "{0}.{1} SC3240201TableAdapter.UpdStallUse StallUseId={2}" _
                               , Me.GetType.ToString _
                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
                               , arg.StallUseId))
                    Return ActionResult.ChipFinishByJobUnInstructError
                End If

                '次のチップステータスが中断または終了の場合
                'If nextChipStatus = NEXTCHIPSTATUS_CHANGETOSTOP _
                'Or nextChipStatus = NEXTCHIPSTATUS_CHANGETOFINISH Then

                '    '休憩取得フラグ
                '    Dim restFlg As String

                '    If arg.RestFlg = -1 Then
                '        '休憩フラグが-1の場合、

                '        '0に変更(最構築と合わせる)
                '        restFlg = "0"

                '    Else
                '        '他の場合

                '        'そのままで設定
                '        restFlg = arg.RestFlg.ToString

                '    End If

                '    '終了関数を呼ぶ(行ロックバージョンが-1：ストールロックと行ロックバージョンチェックはいらない)
                '    Dim returnValue As Long = clsTabletSMBCommonClass.Finish(arg.StallUseId, _
                '                                                             updDate, _
                '                                                             restFlg, _
                '                                                             updDate, _
                '                                                             -1, _
                '                                                             MY_PROGRAMID)

                '    If returnValue <> ActionResult.Success Then
                '        'エラーの場合

                '        'ロールバック
                '        Me.Rollback = True

                '        Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                '                                   "{0}.{1} Finish failed. Error code is {2}", _
                '                                   Me.GetType.ToString, _
                '                                   System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                                   returnValue))

                '        'エラーコードを戻る
                '        Return returnValue

                '    Else

                '        '成功の場合、

                '        'pushフラグをリセット
                '        Me.ResetPushFlg(clsTabletSMBCommonClass)

                '    End If

                'End If
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
                '2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END

                '●予約送信を行う
                If (callFlag) Then

                    '予約送信を行う場合
                    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
                    'Dim retVal As Integer = clsTabletSMBCommonClass.SendReserveInfo(arg.SvcInId, arg.JobDtlId, arg.StallUseId, prevChipStatus, _
                    '                                                                crntChipStatus, arg.ResvStatus, MY_PROGRAMID, Nothing, True)
                    Dim retVal As Integer

                    Using biz3800903 As New IC3800903BusinessLogic
                        retVal = biz3800903.SendReserveInfo(arg.SvcInId, _
                                                            arg.JobDtlId, _
                                                            arg.StallUseId, _
                                                            prevChipStatus, _
                                                            crntChipStatus, _
                                                            arg.ResvStatus, _
                                                            MY_PROGRAMID, _
                                                            Nothing, _
                                                            True)
                    End Using
                    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

                    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                    'If retVal <> ActionResult.Success Then
                    '    Me.Rollback = True
                    '    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
                    '    'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '    '           , "{0}.{1} clsTabletSMBCommonClass.SendReserveInfo arg.SvcInId={2} arg.JobDtlId={3} arg.StallUseId={4} prevChipStatus={5} crntChipStatus={6} arg.ResvStatus={7} MY_PROGRAMID={8}" _
                    '    '           , Me.GetType.ToString _
                    '    '           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '    '           , arg.SvcInId, arg.JobDtlId, arg.StallUseId, prevChipStatus, crntChipStatus, arg.ResvStatus, MY_PROGRAMID))
                    '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '                                , "{0}.{1} biz3800903.SendReserveInfo arg.SvcInId={2} arg.JobDtlId={3} arg.StallUseId={4} prevChipStatus={5} crntChipStatus={6} arg.ResvStatus={7} MY_PROGRAMID={8}" _
                    '                                , Me.GetType.ToString _
                    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '                                , arg.SvcInId, arg.JobDtlId, arg.StallUseId, prevChipStatus, crntChipStatus, arg.ResvStatus, MY_PROGRAMID))
                    '    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END
                    '    Return ActionResult.DmsLinkageError
                    'End If

                    If retVal <> ActionResult.Success _
                    And retVal <> ActionResult.WarningOmitDmsError Then
                        '予約送信処理結果コードが下記の場合
                        '　　　0以外、かつ
                        '　-9000以外

                        Me.Rollback = True

                        '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

                        '戻り値
                        Dim returnValue As Long

                        If ActionResult.IC3800903ResultRangeLower <= retVal _
                        AndAlso retVal <= ActionResult.IC3800903ResultRangeUpper Then
                            '予約連携処理結果コードが8000以上かつ8999以下の場合
                            '予約連携処理結果コードを返却
                            returnValue = retVal

                        Else
                            '上記以外の場合
                            '「15：他システムとの連携エラー」を返却
                            returnValue = ActionResult.DmsLinkageError

                        End If

                        '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                        Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                                    , "{0}.{1} biz3800903.SendReserveInfo arg.SvcInId={2} arg.JobDtlId={3} arg.StallUseId={4} prevChipStatus={5} crntChipStatus={6} arg.ResvStatus={7} MY_PROGRAMID={8}" _
                                                    , Me.GetType.ToString _
                                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                    , arg.SvcInId, arg.JobDtlId, arg.StallUseId, prevChipStatus, crntChipStatus, arg.ResvStatus, MY_PROGRAMID))

                        '基幹連携エラーを返却
                        '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START
                        'Return ActionResult.DmsLinkageError
                        Return returnValue
                        '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                    End If

                    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    '●ステータス送信を行う
                    Dim dmsSendResult As Long
                    Using ic3802601blc As New IC3802601BusinessLogic
                        dmsSendResult = ic3802601blc.SendStatusInfo(arg.SvcInId, _
                                                                    arg.JobDtlId, _
                                                                    arg.StallUseId, _
                                                                    prevChipStatus, _
                                                                    crntChipStatus, _
                                                                    0)
                    End Using

                    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                    'If dmsSendResult <> ActionResult.Success Then
                    '    Me.Rollback = True
                    '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} ic3802601blc.SendStatusInfo FAILURE " _
                    '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                    '    Return ActionResult.DmsLinkageError
                    'End If

                    If dmsSendResult <> ActionResult.Success _
                    And dmsSendResult <> ActionResult.WarningOmitDmsError Then
                        'ステータス送信処理結果コードが下記の場合
                        '　　　0以外、かつ
                        '　-9000以外

                        Me.Rollback = True

                        Logger.Warn(String.Format(CultureInfo.CurrentCulture, _
                                                   "{0}.{1} ic3802601blc.SendStatusInfo FAILURE ", _
                                                   Me.GetType.ToString, _
                                                   MethodBase.GetCurrentMethod.Name))

                        '基幹連携エラーを返却
                        Return ActionResult.DmsLinkageError

                    End If

                    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                    '予約送信処理結果とステータス送信処理結果の値を比較し、
                    '小さい方の値を返却値とする
                    '※-9000と0が混同した場合、-9000を優先させるため
                    retValue = Math.Min(retVal, dmsSendResult)

                    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                End If

            Finally
                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START
                'If isStallLock Then
                '    'ストールロック解除
                '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                '    'clsTabletSMBCommonClass.LockStallReset(CLng(arg.StallId), arg.ChipDispStartDate, arg.Account, updDate, MY_PROGRAMID)
                '    clsTabletSMBCommonClass.LockStallReset(CType(arg.StallId, Decimal), arg.ChipDispStartDate, arg.Account, updDate, MY_PROGRAMID)
                '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                'End If

                If IsLockStall Then
                    'ストールロックを実施した場合

                    'ストールロック解除
                    clsTabletSMBCommonClass.LockStallReset(CType(arg.StallId, Decimal), _
                                                           arg.ChipDispStartDate, _
                                                           arg.Account, _
                                                           updDate, _
                                                           MY_PROGRAMID)
                End If
                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

                ta = Nothing
            End Try
        End Using

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START
        'Return 0
        Return retValue
        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    ''' <summary>
    ''' データを更新する
    ''' （予約情報更新WebServiceを利用してDB更新）
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="updDate">更新日時</param>
    ''' <param name="startPlanTime">更新用の予定開始日時</param>
    ''' <param name="finishPlanTime">更新用の予定終了日時</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateDataUsingWebService(ByVal arg As CallBackArgumentClass, _
                                              ByVal updDate As Date, _
                                              ByVal startPlanTime As Date, _
                                              ByVal finishPlanTime As Date) As Long

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[updDate:{0}][startPlanTime:{1}][finishPlanTime:{2}][StallId:{3}][ChipDispStartDate:{4}][Account:{5}][SvcInId:{6}][RowLockVersion:{7}][DlrCD:{8}][StrCD:{9}][SvcClassId:{10}][StallUseId:{11}]", _
                      updDate, startPlanTime, finishPlanTime, arg.StallId, arg.ChipDispStartDate, arg.Account, arg.SvcInId, arg.RowLockVersion, arg.DlrCD, arg.StrCD, arg.SvcClassId, arg.StallUseId)

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START

        ''ストールロックフラグ
        'Dim isStallLock As Boolean = False

        'ストールロックフラグ(初期値はFalse(ストールロック未実施))
        IsLockStall = False

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

        Dim ta As New SC3240201TableAdapter
        Dim resultCD As Long = 0
        Dim prevChipStatus As String = String.Empty       '変更前のチップステータス
        Dim crntChipStatus As String = String.Empty       '変更後のチップステータス
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        instructStatusList = Nothing
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START
        'メソッドの戻り値
        Dim retValue As Long = 0
        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START

                ''受付・追加作業・NoShow・中断エリア以外の場合
                'If Not (SUBAREA_RECEPTION.Equals(arg.SubAreaId) Or SUBAREA_ADDWORK.Equals(arg.SubAreaId) Or _
                '        SUBAREA_NOSHOW.Equals(arg.SubAreaId) Or SUBAREA_STOP.Equals(arg.SubAreaId)) Then

                '    'ストールロック
                '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                '    'resultCD = clsTabletSMBCommonClass.LockStall(CLng(arg.StallId), arg.ChipDispStartDate, arg.Account, updDate, MY_PROGRAMID)
                '    resultCD = clsTabletSMBCommonClass.LockStall(CType(arg.StallId, Decimal), arg.ChipDispStartDate, arg.Account, updDate, MY_PROGRAMID)
                '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                '    If resultCD <> ActionResult.Success Then
                '        Me.Rollback = True
                '        Return resultCD
                '    End If
                '    isStallLock = True
                'End If

                'ストールロック処理
                Dim resultLockStall As Long = Me.LockStall(clsTabletSMBCommonClass, _
                                                           arg.SubAreaId, _
                                                           CType(arg.StallId, Decimal), _
                                                           arg.ChipDispStartDate, _
                                                           arg.Account, _
                                                           updDate)

                If resultLockStall <> ActionResult.Success Then
                    'ストールロック処理が失敗した場合

                    Me.Rollback = True
                    Return resultLockStall

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

                '●予約送信用
                '変更前のチップステータスを取得する
                prevChipStatus = clsTabletSMBCommonClass.JudgeChipStatus(arg.StallUseId)

                '更新用のRO作業連番、ストール利用ステータスを取得
                Dim updateReserveInfo As SC3240201DataSet.SC3240201UpdateReserveInfoRow
                updateReserveInfo = Me.GetUpdateReserveInfo(arg)

                '整備種類DropDownListで選択されている値の、サービス分類IDを取得
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'Dim svcClassId As Long = CLng(arg.SvcClassId)
                Dim svcClassId As Decimal = CType(arg.SvcClassId, Decimal)
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                Dim svcClassCD As String = String.Empty
                If svcClassId > 0 Then
                    '整備種類が指定されている場合

                    'サービス分類IDを条件にサービス分類コードを取得
                    svcClassCD = ta.GetSvcClassCD(svcClassId)
                End If

                '◎予約情報更新WebServiceをCallする
                Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow
                drWebServiceResult = Me.CallUpdateReserve(arg, updDate, updateReserveInfo, startPlanTime, finishPlanTime, svcClassCD)

                If drWebServiceResult Is Nothing Then
                    'WebService処理失敗

                    Me.Rollback = True
                    Return -1

                ElseIf drWebServiceResult.RESULTCODE = WEBSERVICE_ROWLOCKVERSION_ERROR Then
                    'WebService行ロックバージョンエラー

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} WebService RowLockVersionError " _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name))

                    Me.Rollback = True
                    Return ActionResult.RowLockVersionError

                ElseIf drWebServiceResult.RESULTCODE <> ActionResult.Success Then
                    'その他、処理失敗

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} WebService Error OUT:RETURNCODE = {2}" _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name _
                                , drWebServiceResult.RESULTCODE))

                    Me.Rollback = True
                    Return -1
                End If

                'その他の更新（RO作業連番、ストール利用ステータスの更新(自チップ以外のリレーションチップ)）

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'If drWebServiceResult.ROW_LOCK_VERSION > 0 AndAlso Not (String.IsNullOrEmpty(arg.RONum.Trim())) Then

                '    '予約情報更新WebServiceが成功、且つ、RO番号がある場合
                '    'サービス入庫をロックして、チェックする
                '    resultCD = clsTabletSMBCommonClass.LockServiceInTable(arg.SvcInId, drWebServiceResult.ROW_LOCK_VERSION, arg.Account, updDate, MY_PROGRAMID)
                '    If resultCD <> ActionResult.Success Then
                '        Me.Rollback = True
                '        Return resultCD
                '    End If

                '    Me.UpdateOtherDataUsingWebService(arg, updDate, ta)
                'End If

                If drWebServiceResult.ROW_LOCK_VERSION > 0 AndAlso Not (String.IsNullOrEmpty(arg.RONum.Trim())) AndAlso Not SUBAREA_ADDWORK.Equals(arg.SubAreaId) Then
                    '予約情報更新WebServiceが成功、且つ、RO番号がある場合、且つ、追加作業以外の場合

                    'サービス入庫をロックして、チェックする
                    resultCD = clsTabletSMBCommonClass.LockServiceInTable(arg.SvcInId, drWebServiceResult.ROW_LOCK_VERSION, arg.Account, updDate, MY_PROGRAMID)
                    If resultCD <> ActionResult.Success Then
                        Me.Rollback = True
                        Return resultCD
                    End If

                    instructStatusList = Me.UpdateOtherDataUsingWebService(arg, updDate, ta)
                End If
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '●予約送信用
                '変更後のチップステータスを取得する
                crntChipStatus = clsTabletSMBCommonClass.JudgeChipStatus(arg.StallUseId)

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                ''着工指示・RO関連項目データの送信
                'If Not (String.IsNullOrEmpty(arg.RONum.Trim())) Then
                '    'RO番号がある場合のみ

                '着工指示の送信
                If Not (String.IsNullOrEmpty(arg.RONum.Trim())) AndAlso Not SUBAREA_ADDWORK.Equals(arg.SubAreaId) Then
                    'RO番号がある場合、かつ、追加作業以外の場合
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                    '着工指示データの送信
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    'resultCD = Me.SendInstruct(arg, clsTabletSMBCommonClass, ta)
                    resultCD = Me.SendInstruct(arg, ta, updDate)
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                    If resultCD <> ActionResult.Success Then
                        Me.Rollback = True
                        Return resultCD
                    End If

                    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
                    ''RO関連項目の送信
                    'resultCD = Me.SendROInfo(arg)
                    'If resultCD <> ActionResult.Success Then
                    '    Me.Rollback = True
                    '    Return resultCD
                    'End If
                    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END
                End If

                '●予約送信を行う
                '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
                'Dim retVal As Integer = clsTabletSMBCommonClass.SendReserveInfo(arg.SvcInId, arg.JobDtlId, arg.StallUseId, prevChipStatus, _
                '                                                                crntChipStatus, arg.ResvStatus, MY_PROGRAMID, Nothing, True)
                Dim retVal As Integer

                Using biz3800903 As New IC3800903BusinessLogic
                    retVal = biz3800903.SendReserveInfo(arg.SvcInId, _
                                                        arg.JobDtlId, _
                                                        arg.StallUseId, _
                                                        prevChipStatus, _
                                                        crntChipStatus, _
                                                        arg.ResvStatus, _
                                                        MY_PROGRAMID, _
                                                        Nothing, _
                                                        True)
                End Using
                '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If retVal <> ActionResult.Success Then
                '    Me.Rollback = True
                '    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
                '    'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '    '            , "{0}.{1} clsTabletSMBCommonClass.SendReserveInfo arg.SvcInId={2} arg.JobDtlId={3} arg.StallUseId={4} prevChipStatus={5} crntChipStatus={6} arg.ResvStatus={7} MY_PROGRAMID={8}" _
                '    '            , Me.GetType.ToString _
                '    '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '    '            , arg.SvcInId, arg.JobDtlId, arg.StallUseId, prevChipStatus, crntChipStatus, arg.ResvStatus, MY_PROGRAMID))
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                               , "{0}.{1} biz3800903.SendReserveInfo arg.SvcInId={2} arg.JobDtlId={3} arg.StallUseId={4} prevChipStatus={5} crntChipStatus={6} arg.ResvStatus={7} MY_PROGRAMID={8}" _
                '                               , Me.GetType.ToString _
                '                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                               , arg.SvcInId, arg.JobDtlId, arg.StallUseId, prevChipStatus, crntChipStatus, arg.ResvStatus, MY_PROGRAMID))
                '    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END
                '    Return ActionResult.DmsLinkageError
                'End If

                If retVal <> ActionResult.Success _
                And retVal <> ActionResult.WarningOmitDmsError Then
                    '予約送信処理結果コードが下記の場合
                    '　　　0以外、かつ
                    '　-9000以外

                    Me.Rollback = True

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

                    '戻り値
                    Dim returnValue As Long

                    If ActionResult.IC3800903ResultRangeLower <= retVal _
                    AndAlso retVal <= ActionResult.IC3800903ResultRangeUpper Then
                        '予約連携処理結果コードが8000以上かつ8999以下の場合
                        '予約連携処理結果コードを返却
                        returnValue = retVal

                    Else
                        '上記以外の場合
                        '「15：他システムとの連携エラー」を返却
                        returnValue = ActionResult.DmsLinkageError

                    End If

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                               , "{0}.{1} biz3800903.SendReserveInfo arg.SvcInId={2} arg.JobDtlId={3} arg.StallUseId={4} prevChipStatus={5} crntChipStatus={6} arg.ResvStatus={7} MY_PROGRAMID={8}" _
                                               , Me.GetType.ToString _
                                               , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                               , arg.SvcInId, arg.JobDtlId, arg.StallUseId, prevChipStatus, crntChipStatus, arg.ResvStatus, MY_PROGRAMID))

                    '基幹連携エラーを返却
                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START
                    'Return ActionResult.DmsLinkageError
                    Return returnValue
                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                '●ステータス送信を行う
                Dim dmsSendResult As Long
                Using ic3802601blc As New IC3802601BusinessLogic
                    dmsSendResult = ic3802601blc.SendStatusInfo(arg.SvcInId, _
                                                                arg.JobDtlId, _
                                                                arg.StallUseId, _
                                                                prevChipStatus, _
                                                                crntChipStatus, _
                                                                0)
                End Using

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If dmsSendResult <> ActionResult.Success Then
                '    Me.Rollback = True
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} ic3802601blc.SendStatusInfo FAILURE " _
                '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                '    Return ActionResult.DmsLinkageError
                'End If

                If dmsSendResult <> ActionResult.Success _
                And dmsSendResult <> ActionResult.WarningOmitDmsError Then
                    'ステータス送信処理結果コードが下記の場合
                    '　　　0以外、かつ
                    '　-9000以外

                    Me.Rollback = True

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture, _
                                               "{0}.{1} ic3802601blc.SendStatusInfo FAILURE ", _
                                               Me.GetType.ToString, _
                                               MethodBase.GetCurrentMethod.Name))

                    '基幹連携エラーを返却
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                '予約送信処理結果とステータス送信処理結果の値を比較し、
                '小さい方の値を返却値とする
                '※-9000と0が混同した場合、-9000を優先させるため
                retValue = Math.Min(retVal, dmsSendResult)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Finally
                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START

                'If isStallLock Then
                '    'ストールロック解除
                '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                '    'clsTabletSMBCommonClass.LockStallReset(CLng(arg.StallId), arg.ChipDispStartDate, arg.Account, updDate, MY_PROGRAMID)
                '    clsTabletSMBCommonClass.LockStallReset(CType(arg.StallId, Decimal), arg.ChipDispStartDate, arg.Account, updDate, MY_PROGRAMID)
                '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                'End If

                If IsLockStall Then
                    'ストールロック解除
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    'clsTabletSMBCommonClass.LockStallReset(CLng(arg.StallId), arg.ChipDispStartDate, arg.Account, updDate, MY_PROGRAMID)
                    clsTabletSMBCommonClass.LockStallReset(CType(arg.StallId, Decimal), arg.ChipDispStartDate, arg.Account, updDate, MY_PROGRAMID)
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

                ta = Nothing
            End Try
        End Using

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START
        'Return 0
        Return retValue
        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function
#End Region

#Region "DB更新処理後の処理"

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START

    ' ''' <summary>
    ' ''' Update操作をした後のチップ情報を取得する
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="strCD">店舗コード</param>
    ' ''' <param name="dtNow">更新日時</param>
    ' ''' <param name="svcInId">サービス入庫ID</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetStallChipInfoFromSvcInId(ByVal dlrCD As String, _
    '                                            ByVal strCD As String, _
    '                                            ByVal dtNow As Date, _
    '                                            ByVal svcInId As Decimal) As String

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
    '          "[dlrCD:{0}][strCD:{1}][dtNow:{2}][svcInId:{3}]", _
    '          dlrCD, strCD, dtNow, svcInId)


    '    Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = Nothing

    '    'サービス入庫IDをリストにセット
    '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    '    'Dim svcInIdList As New List(Of Long)
    '    Dim svcInIdList As New List(Of Decimal)
    '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    '    svcInIdList.Add(svcInId)

    '    Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
    '        dtChipInfo = clsTabletSMBCommonClass.GetStallChipBySvcinId(dlrCD, strCD, dtNow, svcInIdList)
    '    End Using

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    '    Return ChipDetailDataTableToJson(dtChipInfo)
    'End Function

    ''' <summary>
    ''' 各操作後、ストール上更新されたチップの情報を取得
    ''' </summary>
    ''' <param name="inDlrCode">販売店コード</param>
    ''' <param name="inBrnCode">店舗コード</param>
    ''' <param name="inShowDate">画面に表示されてる日時</param>
    ''' <param name="inLastRefreshTime">最新の更新日時</param>
    ''' <returns>最新のチップ情報</returns>
    ''' <remarks></remarks>
    Public Function GetStallChipAfterOperation(ByVal inDlrCode As String, _
                                               ByVal inBrnCode As String, _
                                               ByVal inShowDate As Date, _
                                               ByVal inLastRefreshTime As Date) As String

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, _
                      True, _
                      "[inDlrCode:{0}][inBrnCode:{1}][inShowDate:{2}][inLastRefreshTime:{3}]", _
                      inDlrCode, _
                      inBrnCode, _
                      inShowDate, _
                      inLastRefreshTime)


        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            '各操作後、ストール上更新されたチップの情報を取得
            Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
                clsTabletSMBCommonClass.GetStallChipAfterOperation(inDlrCode, _
                                                                   inBrnCode, _
                                                                   inShowDate, _
                                                                   inLastRefreshTime)

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

            Return ChipDetailDataTableToJson(dtChipInfo)

        End Using


    End Function

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END


#End Region

#Region "計算処理"
    ''' <summary>
    ''' 登録時、休憩・使用不可チップと重複し、Confirmメッセージにて休憩を取得するを選択した場合、
    ''' もしくは、Confirmメッセージなしの場合、更新用の各種日時を算出する
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <remarks></remarks>
    Public Function CalcWorkDateTime(ByVal arg As CallBackArgumentClass) As List(Of ChipDetailDateTimeClass)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[InputStallStartTime:{0}][InputStallEndTime:{1}][ProcWorkTime:{2}][PlanWorkTime:{3}][StallId:{4}][ChipDispStartDate:{5}][RestFlg:{6}][FinishPlanTime:{7}][prmsEndTime:{8}]", _
                      arg.InputStallStartTime, arg.InputStallEndTime, arg.ProcWorkTime, arg.PlanWorkTime, arg.StallId, arg.ChipDispStartDate, arg.RestFlg, arg.FinishPlanTime, arg.PrmsEndTime)

        Dim rtnDateTimeClass As List(Of ChipDetailDateTimeClass) = New List(Of ChipDetailDateTimeClass)
        Dim rtnDateTimeItem As ChipDetailDateTimeClass = New ChipDetailDateTimeClass

        '作業開始日時
        Dim workStartDateTime As Date

        '作業終了日時
        Dim workEndDateTime As Date

        '作業時間
        Dim workTime As Long

        '休憩取得フラグ
        Dim wkRestFlg As String

        '休憩を取得しない場合
        If arg.RestFlg = 0 Then
            wkRestFlg = NOT_USE_REST
        Else
            wkRestFlg = USE_REST
        End If

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            ''見込終了日時を算出（ストール利用ステータス=02:作業中の場合）
            'If arg.StallUseStatus.Equals("02") Then
            '見込終了日時を算出（ストール利用ステータスが 「02:作業中」または「04:作業指示の一部の作業が中断」の場合）
            If STALL_USE_STATUS_02.Equals(arg.StallUseStatus) Or STALL_USE_STATUS_04.Equals(arg.StallUseStatus) Then
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

                '予定作業時間をセット
                workTime = CLng(arg.PlanWorkTime)

                '見込終了日時
                Dim prmsWorkEndDateTime As Date

                '実績開始日時と作業予定時間から見込終了日時を算出
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                ''2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                ''prmsWorkEndDateTime = clsTabletSMBCommonClass.GetServiceEndDateTime(CLng(arg.StallId), _
                ''                                                                    arg.ChipDispStartDate, _
                ''                                                                    workTime, _
                ''                                                                    arg.InputStallStartTime, _
                ''                                                                    arg.InputStallEndTime, _
                ''                                                                    wkRestFlg)
                'prmsWorkEndDateTime = clsTabletSMBCommonClass.GetServiceEndDateTime(CType(arg.StallId, Decimal), _
                '                                                                    arg.ChipDispStartDate, _
                '                                                                    workTime, _
                '                                                                    arg.InputStallStartTime, _
                '                                                                    arg.InputStallEndTime, _
                '                                                                    wkRestFlg)
                ''2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                Dim serviceEndDateTimeData As ServiceEndDateTimeData = clsTabletSMBCommonClass.GetServiceEndDateTime(CType(arg.StallId, Decimal), _
                                                                                    arg.ChipDispStartDate, _
                                                                                    workTime, _
                                                                                    arg.InputStallStartTime, _
                                                                                    arg.InputStallEndTime, _
                                                                                    wkRestFlg)
                prmsWorkEndDateTime = serviceEndDateTimeData.ServiceEndDateTime
                arg.RestFlg = CInt(serviceEndDateTimeData.RestFlg)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                '●算出された見込終了日時
                rtnDateTimeItem.DetailprmsEndTime = prmsWorkEndDateTime
            Else
                '実績時間が0、且つ、作業終了実績日時が入っていない場合
                If (CLng(arg.ProcWorkTime) = 0) AndAlso (String.IsNullOrEmpty(arg.FinishProcessTime)) Then

                    '予定作業時間をセット
                    workTime = CLng(arg.PlanWorkTime)

                    '開始日時を算出
                    If arg.RestFlg = 1 Then
                        '休憩取得フラグが1(休憩を取得する)の場合

                        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                        'workStartDateTime = clsTabletSMBCommonClass.GetServiceStartDateTime(CLng(arg.StallId), _
                        '                                                                    arg.ChipDispStartDate, _
                        '                                                                    arg.InputStallStartTime, _
                        '                                                                    arg.InputStallEndTime, _
                        '                                                                    wkRestFlg)
                        workStartDateTime = clsTabletSMBCommonClass.GetServiceStartDateTime(CType(arg.StallId, Decimal), _
                                                                                            arg.ChipDispStartDate, _
                                                                                            arg.InputStallStartTime, _
                                                                                            arg.InputStallEndTime, _
                                                                                            wkRestFlg)
                        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                    Else

                        workStartDateTime = Date.Parse(arg.StartPlanTime, CultureInfo.InvariantCulture)

                    End If

                    '●算出された作業開始予定日時
                    rtnDateTimeItem.DetailStartPlanTime = workStartDateTime

                    '終了日時を算出
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    ''2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    ''workEndDateTime = clsTabletSMBCommonClass.GetServiceEndDateTime(CLng(arg.StallId), _
                    ''                                                                arg.ChipDispStartDate, _
                    ''                                                                workTime, _
                    ''                                                                arg.InputStallStartTime, _
                    ''                                                                arg.InputStallEndTime, _
                    ''                                                                wkRestFlg)
                    'workEndDateTime = clsTabletSMBCommonClass.GetServiceEndDateTime(CType(arg.StallId, Decimal), _
                    '                                                                arg.ChipDispStartDate, _
                    '                                                                workTime, _
                    '                                                                arg.InputStallStartTime, _
                    '                                                                arg.InputStallEndTime, _
                    '                                                                wkRestFlg)
                    ''2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                    Dim serviceEndDateTimeData As ServiceEndDateTimeData = clsTabletSMBCommonClass.GetServiceEndDateTime(CType(arg.StallId, Decimal), _
                                                                                    arg.ChipDispStartDate, _
                                                                                    workTime, _
                                                                                    arg.InputStallStartTime, _
                                                                                    arg.InputStallEndTime, _
                                                                                    wkRestFlg)
                    workEndDateTime = serviceEndDateTimeData.ServiceEndDateTime
                    arg.RestFlg = CInt(serviceEndDateTimeData.RestFlg)
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                    '●算出された作業終了予定日時
                    rtnDateTimeItem.DetailFinishPlanTime = workEndDateTime
                End If
            End If
        End Using

        '返却用のリストに追加
        rtnDateTimeClass.Add(rtnDateTimeItem)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        Return rtnDateTimeClass
    End Function

    ''' <summary>
    ''' 登録時、休憩・使用不可チップと重複し、Confirmメッセージにて休憩を取得するを選択した場合、
    ''' 更新用の実績時間を算出する
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <remarks></remarks>
    Public Function CalcResultWorkTime(ByVal arg As CallBackArgumentClass) As List(Of ChipDetailDateTimeClass)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[InputStallStartTime:{0}][InputStallEndTime:{1}][ProcWorkTime:{2}][PlanWorkTime:{3}][StallId:{4}][ChipDispStartDate:{5}][RestFlg:{6}][FinishPlanTime:{7}][prmsEndTime:{8}]", _
                      arg.InputStallStartTime, arg.InputStallEndTime, arg.ProcWorkTime, arg.PlanWorkTime, arg.StallId, arg.ChipDispStartDate, arg.RestFlg, arg.FinishPlanTime, arg.PrmsEndTime)

        Dim rtnDateTimeClass As List(Of ChipDetailDateTimeClass) = New List(Of ChipDetailDateTimeClass)
        Dim rtnDateTimeItem As ChipDetailDateTimeClass = New ChipDetailDateTimeClass

        '休憩取得フラグ
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        ''2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        ''Dim wkRestFlg As String = USE_REST
        'Dim wkRestFlg As String = NOT_USE_REST
        ''2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        Dim wkRestFlg = String.Empty

        Using biz As New TabletSMBCommonClassBusinessLogic

            If biz.IsRestAutoJudge() Then
                '休憩を自動判定する場合
                wkRestFlg = NOT_USE_REST
            Else
                '自動判定しない場合
                wkRestFlg = USE_REST
            End If

        End Using
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        Dim wkProcTime As Long = 0

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            '作業開始実績日時と作業終了実績日時から実績作業時間を算出
            'wkProcTime = clsTabletSMBCommonClass.GetServiceWorkTime(CLng(arg.StallId), _
            '                                                        CDate(arg.StartProcessTime), _
            '                                                        CDate(arg.FinishProcessTime), _
            '                                                        wkRestFlg, _
            '                                                        arg.InputStallStartTime, _
            '                                                        arg.InputStallEndTime)
            wkProcTime = clsTabletSMBCommonClass.GetServiceWorkTime(CType(arg.StallId, Decimal), _
                                                                    CDate(arg.StartProcessTime), _
                                                                    CDate(arg.FinishProcessTime), _
                                                                    wkRestFlg, _
                                                                    arg.InputStallStartTime, _
                                                                    arg.InputStallEndTime)
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        End Using

        '●算出された実績作業時間
        rtnDateTimeItem.DetailprocTime = wkProcTime

        '返却用のリストに追加
        rtnDateTimeClass.Add(rtnDateTimeItem)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        Return rtnDateTimeClass

    End Function
#End Region

#Region "Check処理"

    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
    ' ''' <summary>
    ' ''' 登録しようとしたチップが休憩・使用不可チップと重複しないかチェックする
    ' ''' </summary>
    ' ''' <param name="arg">引数クラスオブジェクト</param>
    ' ''' <returns>True:衝突しない/False:衝突する</returns>
    ' ''' <remarks></remarks>
    'Public Function CheckRestOrUnavailableChipCollision(ByVal arg As CallBackArgumentClass) As Boolean

    ''' <summary>
    ''' 登録しようとしたチップが休憩と重複しないかチェックする
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns>True:衝突しない/False:衝突する</returns>
    ''' <remarks></remarks>
    Public Function CheckRestCollision(ByVal arg As CallBackArgumentClass) As Boolean
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[SubAreaId:{0}][ProcWorkTime:{1}][PlanWorkTime:{2}][InputStallStartTime:{3}][InputStallEndTime:{4}][StallId:{5}][ChipDispStartDate:{6}]", _
                      arg.SubAreaId, arg.ProcWorkTime, arg.PlanWorkTime, arg.InputStallStartTime, arg.InputStallEndTime, arg.StallId, arg.ChipDispStartDate)

        Dim clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

        Try
            Dim rtnVal As Boolean = True

            '作業時間
            Dim workTime As Long

            'ストール開始日時、時間
            Dim dtStartDate As Date
            Dim stallStartTime As TimeSpan

            'ストール終了日時、時間
            Dim dtEndDate As Date
            Dim stallEndTime As TimeSpan

            'ストール利用ステータス=02:作業中の場合、予定時間をセット
            If arg.StallUseStatus.Equals("02") OrElse arg.StallUseStatus.Equals("04") Then

                workTime = CLng(arg.PlanWorkTime)

            ElseIf CLng(arg.ProcWorkTime) > 0 Then

                '実績時間が入っていれば、実績時間をセット
                workTime = CLng(arg.ProcWorkTime)

            ElseIf (CLng(arg.ProcWorkTime) = 0) AndAlso (Not String.IsNullOrEmpty(arg.FinishProcessTime)) Then

                '実績時間が0、且つ、作業終了実績日時が入っている場合は、チェックしない
                OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)
                Return rtnVal

            Else
                '作業開始実績日時が入ってなければ、予定時間をセット
                workTime = CLng(arg.PlanWorkTime)

            End If

            '現在開いている日付の営業開始時間と営業終了時間を取得する
            If arg.StallStartTime <> "" Then
                stallStartTime = New TimeSpan(CInt(arg.StallStartTime.Substring(0, 2)), CInt(arg.StallStartTime.Substring(3, 2)), 0)
            End If
            If arg.StallEndTime <> "" Then
                stallEndTime = New TimeSpan(CInt(arg.StallEndTime.Substring(0, 2)), CInt(arg.StallEndTime.Substring(3, 2)), 0)
            End If

            'arg.ShowDateがあれば、営業日時を計算する
            If CType(arg.ShowDate, Date) <> Date.MinValue Then
                dtStartDate = CType(arg.ShowDate, Date)
                dtStartDate = dtStartDate.AddHours(stallStartTime.Hours).AddMinutes(stallStartTime.Minutes)

                dtEndDate = CType(arg.ShowDate, Date)
                dtEndDate = dtEndDate.AddHours(stallEndTime.Hours).AddMinutes(stallEndTime.Minutes)
            End If

            '休憩エリア確認
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim hasRestTimeInServiceTime As Boolean = clsTabletSMBCommonClass.HasRestTimeInServiceTime(dtStartDate, _
            '                                                                                           dtEndDate,
            '                                                                                           CLng(arg.StallId), _
            '                                                                                           arg.ChipDispStartDate, _
            '                                                                                           workTime, _
            '                                                                                           True)
            Dim hasRestTimeInServiceTime As Boolean = clsTabletSMBCommonClass.HasRestTimeInServiceTime(dtStartDate, _
                                                                                                       dtEndDate,
                                                                                                       CType(arg.StallId, Decimal), _
                                                                                                       arg.ChipDispStartDate, _
                                                                                                       workTime, _
                                                                                                       True)
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            '休憩または使用不可エリアと重複する場合
            If hasRestTimeInServiceTime Then
                'False:衝突する
                rtnVal = False
            End If

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

            Return rtnVal

        Finally
            If clsTabletSMBCommonClass IsNot Nothing Then
                clsTabletSMBCommonClass.Dispose()
                clsTabletSMBCommonClass = Nothing
            End If
        End Try

    End Function

    '2019/07/19 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ''' <summary>
    ''' 更新しようとしたチップが使用不可チップと重複しないかチェックする
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="finishPlanTime">更新用の作業完了予定日時</param>
    ''' <returns>True:衝突しない/False:衝突する</returns>
    ''' <remarks></remarks>
    Public Function CheckUnavailableChipCollision(ByVal arg As CallBackArgumentClass, _
                                       ByVal finishPlanTime As Date) As Boolean
        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        If STALL_USE_STATUS_07.Equals(arg.StallUseStatus) Then
            'ストール利用ステータスが 「07:未来店客」の場合は、チェックしない
            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", False)
            Return False

        ElseIf STALL_USE_STATUS_02.Equals(arg.StallUseStatus) Or STALL_USE_STATUS_04.Equals(arg.StallUseStatus) Then
            'ストール利用ステータスが 「02:作業中」または「04:作業指示の一部の作業が中断」の場合は、チェックしない
            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", False)
            Return False

        ElseIf 0 < CLng(arg.ProcWorkTime) Then
            '実績作業時間が入ってる場合（＝実績開始～実績終了あり）は、チェックしない
            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", False)
            Return False

        ElseIf CLng(arg.ProcWorkTime) = 0 AndAlso Not String.IsNullOrEmpty(arg.FinishProcessTime) Then
            '実績時間が0、且つ、作業終了実績日時が入っている場合は、チェックしない
            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", False)
            Return False
        End If

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            'チップの使用不可衝突チェック
            Dim rtnVal As Boolean = clsTabletSMBCommonClass.CheckStallUnavailableOverlapPosition( _
                    CDate(arg.StartPlanTime), finishPlanTime, CDec(arg.StallId))

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
            Return rtnVal
        End Using

    End Function
    '2019/07/19 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

    ''' <summary>
    ''' 登録しようとしたチップが他のチップと衝突しないかチェックする
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="finishPlanTime">更新用の予定終了日時</param>
    ''' <param name="prmsEndTime">更新用の見込終了日時</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <returns>True:衝突しない/False:衝突する</returns>
    ''' <remarks></remarks>
    Public Function CheckChipCollision(ByVal arg As CallBackArgumentClass, _
                                       ByVal finishPlanTime As Date, _
                                       ByVal prmsEndTime As Date, _
                                       ByVal dtNow As Date) As Boolean

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[StallUseStatus:{0}][StartProcessTime:{1}][ProcWorkTime:{2}][FinishProcessTime:{3}][StartPlanTime:{4}][DlrCD:{5}][StrCD:{6}][StallUseId:{7}][StallId:{8}][finishPlanTime:{9}][prmsEndTime:{10}]", _
                      arg.StallUseStatus, arg.StartProcessTime, arg.ProcWorkTime, arg.FinishProcessTime, arg.StartPlanTime, arg.DlrCD, arg.StrCD, arg.StallUseId, arg.StallId, finishPlanTime, prmsEndTime)

        Dim clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

        Try
            Dim rtnVal As Boolean = True
            Dim dispStartDateTime As Date
            Dim dispEndDateTime As Date

            '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            ''ストール利用ステータス=02:作業中の場合
            'If arg.StallUseStatus.Equals("02") Then
            If STALL_USE_STATUS_07.Equals(arg.StallUseStatus) Then
                'ストール利用ステータスが 「07:未来店客」の場合は、チェックしない
                OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)
                Return rtnVal

            ElseIf STALL_USE_STATUS_02.Equals(arg.StallUseStatus) Or STALL_USE_STATUS_04.Equals(arg.StallUseStatus) Then
                'ストール利用ステータスが 「02:作業中」または「04:作業指示の一部の作業が中断」の場合
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

                '作業開始実績日時～見込終了日時の範囲で衝突チェック　※見込終了日時＝作業開始実績日時＋予定作業時間
                dispStartDateTime = CDate(arg.StartProcessTime)
                dispEndDateTime = prmsEndTime

            ElseIf CLng(arg.ProcWorkTime) > 0 Then
                '実績作業時間が入ってる場合（＝実績開始～実績終了あり）

                '作業開始実績日時～作業終了実績日時の範囲で衝突チェック
                dispStartDateTime = CDate(arg.StartProcessTime)
                dispEndDateTime = CDate(arg.FinishProcessTime)

            ElseIf (CLng(arg.ProcWorkTime) = 0) AndAlso (Not String.IsNullOrEmpty(arg.FinishProcessTime)) Then
                '実績時間が0、且つ、作業終了実績日時が入っている場合は、チェックしない
                OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)
                Return rtnVal

            Else
                '上記以外

                '作業開始予定日時～作業終了予定日時の範囲で衝突チェック
                dispStartDateTime = CDate(arg.StartPlanTime)
                dispEndDateTime = finishPlanTime

            End If

            'ストール利用．チップ重複配置チェック
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'If clsTabletSMBCommonClass.CheckChipOverlapPosition(arg.DlrCD, _
            '                                                    arg.StrCD, _
            '                                                    arg.StallUseId, _
            '                                                    CLng(arg.StallId), _
            '                                                    dispStartDateTime, _
            '                                                    dispEndDateTime,
            '                                                    dtNow) Then
            If clsTabletSMBCommonClass.CheckChipOverlapPosition(arg.DlrCD, _
                                                                arg.StrCD, _
                                                                arg.StallUseId, _
                                                                CType(arg.StallId, Decimal), _
                                                                dispStartDateTime, _
                                                                dispEndDateTime,
                                                                dtNow) Then
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                rtnVal = False
            End If

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnVal)

            Return rtnVal

        Finally
            If clsTabletSMBCommonClass IsNot Nothing Then
                clsTabletSMBCommonClass.Dispose()
                clsTabletSMBCommonClass = Nothing
            End If
        End Try

    End Function

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 1つのチップにR/Oと追加作業、または複数の追加作業を紐づけようとしたかどうかチェックする
    ' ''' </summary>
    ' ''' <param name="arg">引数クラスオブジェクト</param>
    ' ''' <returns>True:紐づけOK/False:紐づけNG</returns>
    ' ''' <remarks></remarks>
    'Public Function CheckNestError(ByVal arg As CallBackArgumentClass) As Boolean

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

    '    Dim rtnVal As Boolean = True

    '    Dim rezIdList As List(Of String) = arg.RezIdList                               'チップエリアに表示されている予約IDリスト
    '    Dim rezId_StallUseStatusList As List(Of String) = arg.RezIdStallUseStatusList  'チップエリアに表示されている予約のストール利用ステータスリスト
    '    Dim roJobSeq2List As List(Of String) = arg.ROJobSeq2List                       'チップエリアに表示されている予約のRO作業連番リスト
    '    Dim matchingRezIdList As List(Of String) = arg.MatchingRezIdList               '整備に紐付いた予約IDのリスト
    '    Dim roJobSeqList As List(Of String) = arg.ROJobSeqList                         '整備に紐付いたRO作業連番のリスト

    '    'チップエリアに表示されている予約IDリストの件数分Loop
    '    For i = 0 To rezIdList.Count - 1

    '        Dim isNestErr As Boolean = False            'エラーを判断するためのフラグ
    '        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    '        'Dim dispRezId As Long = CLng(rezIdList(i))  '表示されている予約ID
    '        Dim dispRezId As Decimal = CType(rezIdList(i), Decimal)  '表示されている予約ID
    '        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    '        Dim dispRezId_StallUseStatus As String = rezId_StallUseStatusList(i)  '表示されている予約のストール利用ステータス
    '        Dim dispRoJobSeq2 As String = roJobSeq2List(i)                        '表示されている予約のRO作業連番
    '        Dim searchKey As String = String.Empty      'チェック用のKey(RO作業連番)

    '        'ストール利用ステータスが03:完了 以外の場合
    '        If Not dispRezId_StallUseStatus.Equals("03") Then

    '            '整備に紐付いた予約IDリストの件数分Loop
    '            For j = 0 To matchingRezIdList.Count - 1

    '                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    '                'Dim checkRezId As Long = CLng(matchingRezIdList(j))  '整備に紐付いている予約ID
    '                Dim checkRezId As Decimal = CType(matchingRezIdList(j), Decimal)  '整備に紐付いている予約ID
    '                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '                '一致する予約IDがある場合
    '                If dispRezId = checkRezId Then

    '                    'チップ列を縦にLoopし、最初に見つかったチェックの場合
    '                    If String.IsNullOrEmpty(searchKey) Then
    '                        searchKey = roJobSeqList(j)   'RO作業連番をKeyとして保持
    '                    End If

    '                    '未チェック(-1)でなく、且つ、保持したKey(RO作業連番)と一致しない場合
    '                    If (checkRezId <> -1) AndAlso (Not searchKey.Equals(roJobSeqList(j))) Then

    '                        '紐づけNG
    '                        isNestErr = True
    '                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                    , "{0}.{1} RO Nest Error. checkRezId = {2}" _
    '                                    , Me.GetType.ToString _
    '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                    , checkRezId))

    '                        '内側(j)のループを中断する
    '                        Exit For
    '                    End If

    '                    '受付エリアの場合、画面上見えていない他チップの整備に対するチェック
    '                    '既に整備に紐付いているチップに対して、別のRO作業連番で紐付けようとした場合は「紐づけNG」とする
    '                    '受付エリアの場合
    '                    If (arg.SubAreaId.Equals(SUBAREA_RECEPTION)) Then

    '                        'チップエリアに表示されている予約のRO作業連番 ≠ チェックを付けた整備のRO作業連番 の場合
    '                        If Not dispRoJobSeq2.Equals("-1") AndAlso Not dispRoJobSeq2.Equals(roJobSeqList(j)) Then

    '                            '紐づけNG
    '                            isNestErr = True
    '                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                                        , "{0}.{1} RO Nest Error. CheckRezId = {2}" _
    '                                        , Me.GetType.ToString _
    '                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                        , checkRezId))

    '                            '内側(j)のループを中断する
    '                            Exit For
    '                        End If
    '                    End If
    '                End If
    '            Next

    '            '一つでも整備にチェックが入っている場合
    '            If isNestErr = True Then
    '                rtnVal = False
    '                Exit For
    '            End If
    '        End If
    '    Next

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

    '    Return rtnVal

    'End Function
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' リレーションを含めたチップの作業時間が、来店予定(実績がある場合は実績日時)から
    ''' 納車予定の間に収まっているかチェックする
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns>True:全て収まっている/False:収まっていないチップがある</returns>
    ''' <remarks></remarks>
    Public Function CheckChipOverSrvDateTime(ByVal arg As CallBackArgumentClass) As Boolean

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[SvcInId:{0}][VisitPlanTime:{1}][DeriveredPlanTime:{2}]", _
                      arg.SvcInId, arg.VisitPlanTime, arg.DeriveredPlanTime)

        Dim ta As New SC3240201TableAdapter

        Try
            Dim rtnVal As Boolean = False
            Dim rowCount As Integer = 0

            'リレーションを含めたチップの作業日時が来店予定(または実績)から納車予定の間に収まっていないチップの存在を確認する
            rowCount = ta.GetOverSrvDateTimeChipCount(arg.SvcInId, arg.VisitPlanTime, arg.DeriveredPlanTime, arg.DlrCD, arg.StrCD, arg.StallUseId)

            'カウントが0なら全て収まっているのでチェックOK
            If rowCount = 0 Then
                rtnVal = True
            End If

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rowCount)

            Return rtnVal

        Finally
            ta = Nothing
        End Try

    End Function

    '2016/10/14 NSK  秋田谷 TR-SVT-TMT-20160824-003（チップ詳細の実績時間を変更できなくする）の対応 START
    '''' <summary>
    '''' 実績日時を未来の日時で設定しようとしていないかチェックする
    '''' </summary>
    '''' <param name="arg">引数クラスオブジェクト</param>
    '''' <returns>True:チェックOK/False:チェックNG</returns>
    '''' <remarks></remarks>
    'Public Function CheckChipFutureDateTime(ByVal arg As CallBackArgumentClass) As Boolean
    '
    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
    '                  "[JobDtlId:{0}][StartProcessTime:{1}][StartProcessTime:{2}]", _
    '                  arg.JobDtlId, arg.StartProcessTime, arg.StartProcessTime)
    '
    '    Dim rtnVal As Boolean = False
    '    Dim compResult As Integer
    '
    '    '現在日時をセット
    '    Dim nowDate As Date
    '    nowDate = DateTimeFunc.Now(arg.DlrCD)
    '
    '    '作業開始実績日時に値があればチェック
    '    If Not String.IsNullOrEmpty(arg.StartProcessTime) Then
    '
    '        Dim procStartDateTime As Date
    '        procStartDateTime = CType(arg.StartProcessTime, Date)
    '
    '        compResult = Date.Compare(nowDate, procStartDateTime)
    '
    '        '現在日時 < 作業開始実績日時 の場合、チェックNG
    '        If compResult = -1 Then
    '
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} CheckChipFutureDateTime Error. JobDtlId = {2} StartProcessTime = {3}" _
    '                        , Me.GetType.ToString _
    '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                        , arg.JobDtlId, arg.StartProcessTime))
    '            Return rtnVal
    '        End If
    '    End If
    '
    '    '作業終了実績日時に値があればチェック
    '    If Not String.IsNullOrEmpty(arg.FinishProcessTime) Then
    '
    '        Dim procEndDateTime As Date
    '        procEndDateTime = CType(arg.FinishProcessTime, Date)
    '
    '        compResult = Date.Compare(nowDate, procEndDateTime)
    '
    '        '現在日時 < 作業終了実績日時 の場合、チェックNG
    '        If compResult = -1 Then
    '
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} CheckChipFutureDateTime Error. JobDtlId = {2} FinishProcessTime = {3}" _
    '                        , Me.GetType.ToString _
    '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                        , arg.JobDtlId, arg.FinishProcessTime))
    '            Return rtnVal
    '        End If
    '    End If
    '
    '    'チェックがOKの場合、Trueを返却する
    '    rtnVal = True
    '
    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
    '
    '    Return rtnVal
    '
    'End Function
    '
    '''' <summary>
    '''' 作業中チップの更新時、実績開始以降に、既に実績チップが
    '''' 存在していないかチェックする
    '''' </summary>
    '''' <param name="arg">引数クラスオブジェクト</param>
    '''' <returns>True:チェックOK/False:チェックNG</returns>
    '''' <remarks></remarks>
    'Public Function CheckProcChipCollision(ByVal arg As CallBackArgumentClass) As Boolean
    '
    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
    '                  "[SvcInId:{0}][VisitPlanTime:{1}][DeriveredPlanTime:{2}]", _
    '                  arg.SvcInId, arg.VisitPlanTime, arg.DeriveredPlanTime)
    '
    '    Dim ta As New SC3240201TableAdapter
    '
    '    Try
    '        Dim rtnVal As Boolean = False
    '        Dim rowCount As Integer = 0
    '
    '        'ストール利用ステータス=02:作業中の場合のみチェック
    '        If arg.StallUseStatus.Equals("02") Then
    '
    '            '実績開始以降で、既に存在する実績チップを確認
    '            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    '            'rowCount = ta.GetProcChipCollisionCount(arg.DlrCD, arg.StrCD, CType(arg.StallId, Long), arg.StartProcessTime)
    '            rowCount = ta.GetProcChipCollisionCount(arg.DlrCD, arg.StrCD, CType(arg.StallId, Decimal), arg.StartProcessTime)
    '            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    '
    '            'カウントが0なら該当チップが存在しないのでチェックOK
    '            If rowCount = 0 Then
    '                rtnVal = True
    '            End If
    '
    '        Else
    '            '作業中以外はチェックしない
    '
    '            rtnVal = True
    '        End If
    '
    '        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rowCount)
    '
    '        Return rtnVal
    '
    '    Finally
    '        ta = Nothing
    '    End Try
    '
    'End Function
    '2016/10/14 NSK  秋田谷 TR-SVT-TMT-20160824-003（チップ詳細の実績時間を変更できなくする）の対応 END

    ''' <summary>
    ''' 作業中のチップに紐付く整備の数が0にならないかどうかチェックする
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns>True:チェックOK/False:チェックNG</returns>
    ''' <remarks></remarks>
    Public Function CheckMaintenanceNumOfWorking(ByVal arg As CallBackArgumentClass) As Boolean

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim ta As New SC3240201TableAdapter

        Try
            Dim rtnVal As Boolean = True

            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim roJobSeq As Long = -1       '作業連番
            'Dim dt As SC3240201DataSet.SC3240201RezIdListDataTable

            ''受付エリアの場合
            'If (arg.SubAreaId.Equals(SUBAREA_RECEPTION)) Then

            '    'サブチップが保持している作業連番が存在する場合
            '    If Not IsNothing(arg.ROJobSeq) Then
            '        roJobSeq = CLng(arg.ROJobSeq)
            '    End If
            'End If

            ''サービス入庫IDに紐付く、作業開始済みの予約ID（作業内容ID）リストを取得
            ''　→(登録後、整備に紐付いていなければならない予約ID)リストを取得
            'dt = ta.GetStartedRezIdList(arg.SvcInId, roJobSeq, arg.DlrCD, arg.StrCD)

            Dim dt As SC3240201DataSet.SC3240201RezIdListDataTable

            'サービス入庫IDに紐付く、作業開始済みの予約ID（作業内容ID）リストを取得
            '　→(登録後、整備に紐付いていなければならない予約ID)リストを取得
            dt = ta.GetStartedRezIdList(arg.SvcInId, arg.DlrCD, arg.StrCD)
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            'リスト分ループ
            For Each row In dt
                '作業開始済みの予約IDが、整備に紐付いている予約IDリストに含まれていない場合、エラー
                If Not arg.MatchingRezIdList.Contains(row.REZID.ToString(CultureInfo.CurrentCulture)) Then
                    rtnVal = False
                    Exit For
                End If
            Next

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

            Return rtnVal

        Finally
            ta = Nothing
        End Try

    End Function

    ''' <summary>
    ''' 予約情報更新WebServiceをCallするかどうか判定する
    ''' （リレーションを含めた全チップについて、１つでも実績開始日時が入っていれば
    ''' 　WebServiceをCallしない）
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns>True:WebServiceをCallする/False:WebServiceをCallしない</returns>
    ''' <remarks></remarks>
    Public Function IsCallUpdateReserve(ByVal arg As CallBackArgumentClass) As Boolean

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[svcInId:{0}][dlrCD:{1}][strCD:{2}]", arg.SvcInId, arg.DlrCD, arg.StrCD)

        Dim ta As New SC3240201TableAdapter

        Try
            Dim rtnVal As Boolean = False
            Dim rowCount As Integer = 0
            Dim callFlag As Boolean

            'WebServiceを呼び出すかどうか事前チェック
            callFlag = IsCallWebService(arg)
            If (callFlag) Then

                'リレーションを含めた全チップについて、１つでも実績開始日時が入っていれば、カウントが1で返る
                '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'rowCount = ta.GetProcChipCount(arg.SvcInId, arg.DlrCD, arg.StrCD)
                rowCount = ta.GetProcChipCount(arg.SvcInId, arg.DlrCD, arg.StrCD, arg.StallUseId)
                '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                'カウントが0の場合、WebServiceをCallする
                If rowCount = 0 Then
                    rtnVal = True
                End If
            End If

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rowCount)

            Return rtnVal

        Finally
            ta = Nothing
        End Try

    End Function

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 中断Job存在判定
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>true：中断Jobが存在する、false：中断Jobが存在しない</returns>
    ''' <remarks></remarks>
    Public Function HasStopJob(ByVal inJobDtlId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inJobDtlId={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inJobDtlId))

        '返却値
        Dim retHasStopJob As Boolean

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            '中断Job存在判定(共通関数)
            retHasStopJob = clsTabletSMBCommonClass.HasStopJob(inJobDtlId)

        End Using

        '結果返却
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End retHasStopJob={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  retHasStopJob.ToString))
        Return retHasStopJob

    End Function
    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 作業中チップに紐づくJob追加、削除したことがあるかをチェック
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="dtServiceinBefore">変更前のサービス入庫情報</param>
    ''' <returns>true：Job追加、削除したことがある</returns>
    ''' <remarks></remarks>
    Public Function HasChangeJobNum(ByVal arg As CallBackArgumentClass, _
                                    ByVal dtServiceinBefore As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Jobが変わったフラグ(false：変わってない)
        Dim changeJobFlg As Boolean = False

        '作業中チップを探す
        Dim workingChipBefore As List(Of TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoRow) = _
            (From p In dtServiceinBefore _
             Where p.STALL_USE_STATUS = "04" Or p.STALL_USE_STATUS = "02" _
             Select p).ToList()

        '作業中チップがあれば、該当作業中チップにJobが変更するかを確認
        If workingChipBefore.Count = 1 Then

            '更新前、該当チップのJobを取得する
            Dim jobStatusTableBefore As TabletSmbCommonClassJobResultDataTable = Nothing

            Using tabletSMBCommonBiz As New TabletSMBCommonClassBusinessLogic

                '作業単位でステータスを取得する
                jobStatusTableBefore = tabletSMBCommonBiz.GetJobStatusByJob(workingChipBefore(0).JOB_DTL_ID)

            End Using

            '「Register」ボタンを押した後、該当作業中チップに紐づくJob数を取得
            Dim tiedUpJobNum As Integer = 0

            For i As Integer = 0 To arg.JobinstrucDtlIdList.Count - 1

                '今紐づくJob件数を取得
                If arg.JobinstrucDtlIdList(i).Equals(workingChipBefore(0).JOB_DTL_ID.ToString(CultureInfo.InvariantCulture)) _
                    And Not arg.MatchingRezIdList(i).Equals("-1") Then

                    tiedUpJobNum = tiedUpJobNum + 1

                End If

            Next

            '更新前の作業中チップについて
            '紐づくJob件数が変わった（外した、追加した）
            If jobStatusTableBefore.Count <> tiedUpJobNum Then

                'Job変更フラグにTrueを設定(変わった)
                changeJobFlg = True

            End If

        End If

        '結果返却
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End return={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  changeJobFlg.ToString))

        Return changeJobFlg

    End Function
    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

#End Region

#Region "その他処理"

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 作業中チップに紐づくJob追加、削除したことがあるかをチェック
    ''' </summary>
    ''' <param name="dtServiceinAfter">変更後のサービス入庫情報</param>
    ''' <returns>true：Job追加、削除したことがある</returns>
    ''' <remarks></remarks>
    Public Function GetNextChipStatus(ByVal dtServiceinAfter As TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoDataTable) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.Start.", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name))

        'チップステータス(0：変更してない、1:中断に変更、2：終了に変更)
        Dim retChipStatus As Integer = 0

        '作業中チップを探す
        Dim workingChipBefore As List(Of TabletSMBCommonClassDataSet.TabletSmbCommonClassServiceinChangeInfoRow) = _
            (From p In dtServiceinAfter _
             Where p.STALL_USE_STATUS = "04" Or p.STALL_USE_STATUS = "02" _
             Select p).ToList()

        '作業中チップがあれば、該当作業中チップにJobが変更するかを確認
        If workingChipBefore.Count = 1 Then

            '更新後、該当チップに紐づくJobを取得する
            Dim jobStatusTableAfter As TabletSmbCommonClassJobResultDataTable = Nothing

            Using tabletSMBCommonBiz As New TabletSMBCommonClassBusinessLogic

                '作業単位でステータスを取得する
                jobStatusTableAfter = tabletSMBCommonBiz.GetJobStatusByJob(workingChipBefore(0).JOB_DTL_ID)

            End Using

            If jobStatusTableAfter.Count > 0 Then

                '未開始Jobがあるフラグ(false：なし)
                Dim notStartJobFlg As Boolean = False

                '作業中Jobがあるフラグ(false：なし)
                Dim workingJobFlg As Boolean = False

                '中断したJobがあるフラグ(false：なし)
                Dim stopedJobFlg As Boolean = False

                '今該当作業中チップの各Jobステータスを取得
                For Each jobStatusAfter As TabletSmbCommonClassJobResultRow In jobStatusTableAfter

                    '未開始Jobがあれば、
                    If jobStatusAfter.IsJOB_STATUSNull Then

                        '未開始Jobがあるフラグにtrueを設定
                        notStartJobFlg = True

                        '次のJobへ
                        Continue For

                    End If

                    '作業中Jobがあれば、
                    If TabletSMBCommonClassBusinessLogic.JobStatusWorking.Equals(jobStatusAfter.JOB_STATUS) Then

                        '未開始Jobがあるフラグにtrueを設定
                        workingJobFlg = True
                        '次のJobへ
                        Continue For

                    End If

                    '中断Jobがあれば、
                    If TabletSMBCommonClassBusinessLogic.JobStatusStop.Equals(jobStatusAfter.JOB_STATUS) Then

                        '中断Jobがあるフラグにtrueを設定
                        stopedJobFlg = True

                    End If

                Next

                '未開始Job且つ開始中Jobがないの場合、何もしない
                If notStartJobFlg = False _
                    And workingJobFlg = False Then

                    If stopedJobFlg = True Then
                        '中断Jobがあれば、

                        '中断チップにする
                        retChipStatus = NEXTCHIPSTATUS_CHANGETOSTOP

                    Else

                        '中断Jobがなければ、終了チップにする

                        retChipStatus = NEXTCHIPSTATUS_CHANGETOFINISH

                    End If

                End If

            End If

        End If

        '結果返却
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End return={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  retChipStatus.ToString(CultureInfo.InvariantCulture)))

        Return retChipStatus

    End Function

    ''' <summary>
    ''' タブレットコモンクラスのPushフラグがこのクラスに設定する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ResetPushFlg(ByVal bizTabletSmbCommon As TabletSMBCommonClassBusinessLogic)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        'タブレットコモンクラスのPushフラグがこのクラスに設定する
        Me.NeedPushAfterFinishSingleJob = bizTabletSmbCommon.NeedPushAfterFinishSingleJob
        Me.NeedPushAfterStartSingleJob = bizTabletSmbCommon.NeedPushAfterStartSingleJob
        Me.NeedPushAfterStopSingleJob = bizTabletSmbCommon.NeedPushAfterStopSingleJob

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End NeedPushAfterFinishSingleJob={1}, NeedPushAfterStartSingleJob={2}, NeedPushAfterStopSingleJob={3}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  Me.NeedPushAfterFinishSingleJob, _
                                  Me.NeedPushAfterStartSingleJob, _
                                  Me.NeedPushAfterStopSingleJob))

    End Sub
    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 基幹販売店、基幹店舗コードを取得する
    ''' </summary>
    ''' <param name="dealerCode">i-CROP販売店コード</param>
    ''' <param name="branchCode">i-CROP店舗コード</param>
    ''' <param name="account">i-CROPスタッフコード</param>
    ''' <returns>基幹販売店、基幹店舗コード</returns>
    ''' <remarks></remarks>
    Public Function GetDmsBlnCD(ByVal dealerCode As String, _
                                  ByVal branchCode As String, _
                                  ByVal account As String) As ServiceCommonClassDataSet.DmsCodeMapRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}.S ", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name))

        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

        Using smbCommonBiz As New ServiceCommonClassBusinessLogic

            '基幹販売店コード、店舗コードを取得
            dmsDlrBrnTable = smbCommonBiz.GetIcropToDmsCode(dealerCode, _
                                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                                            dealerCode, _
                                                            branchCode, _
                                                            String.Empty, _
                                                            account)
            If dmsDlrBrnTable.Count <= 0 Then
                'データが取得できない場合はエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.{1}.Error ErrCode: Failed to convert key dealer code.(No data found)", _
                                           Me.GetType.ToString, _
                                           MethodBase.GetCurrentMethod.Name))
                Return Nothing
            ElseIf 1 < dmsDlrBrnTable.Count Then
                'データが2件以上取得できた場合は一意に決定できないためエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.{1}.Error ErrCode:Failed to convert key dealer code.(Non-unique)", _
                                           Me.GetType.ToString, _
                                           MethodBase.GetCurrentMethod.Name))
                Return Nothing
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}.E ", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name))

        Return dmsDlrBrnTable.Item(0)

    End Function

    ''' <summary>
    ''' Push処理と通知処理
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="updDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <remarks></remarks>
    Public Function SendPushAndNoticeDisplay(ByVal arg As CallBackArgumentClass, _
                                  ByVal updDate As Date, _
                                  ByVal objStaffContext As StaffContext) As Long

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim returnCode As Long = 0
        Dim ta As New SC3240201TableAdapter

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                ' サブエリアリフレッシュフラグ
                Dim subAreaRefreshFlg As Boolean = CheckSubAreaRefresh(arg)
                ' Push完了フラグ
                Dim pushCompleteFlg As Boolean = False

                Dim operationCodeList As New List(Of Decimal)

                ' サブエリアリフレッシュフラグがTureの場合
                If subAreaRefreshFlg Then

                    '■ChTのPush処理 (全Cht送付:(自アカウントは除く))
                    '全ChTのスタッフコードリストを取得
                    operationCodeList.Add(Operation.CHT)
                    Dim StaffCodeListChT2 As List(Of String) = clsTabletSMBCommonClass.GetSendStaffCode(arg.DlrCD, arg.StrCD, operationCodeList)
                    operationCodeList.Remove(Operation.CHT)

                    '自アカウントと同じアカウントが存在する場合
                    '自アカウントを除く
                    If StaffCodeListChT2.Contains(objStaffContext.Account) Then
                        StaffCodeListChT2.Remove(objStaffContext.Account)
                    End If

                    '全ChTにPush送信(自身と関連チップを除く)
                    clsTabletSMBCommonClass.SendPushByStaffCodeList(StaffCodeListChT2, PUSH_FUNTIONNAME_CT_CHT)

                    '■CTのPush処理 (全CT送付:(自アカウントは除く))
                    '全CTのスタッフコードリストを取得
                    operationCodeList.Add(Operation.CT)
                    Dim StaffCodeListCT As List(Of String) = clsTabletSMBCommonClass.GetSendStaffCode(arg.DlrCD, arg.StrCD, operationCodeList)
                    operationCodeList.Remove(Operation.CT)

                    '自アカウントと同じアカウントが存在する場合
                    '自アカウントを除く
                    If StaffCodeListCT.Contains(objStaffContext.Account) Then
                        StaffCodeListCT.Remove(objStaffContext.Account)
                    End If

                    '全CTにPush送信(自身を除く)
                    clsTabletSMBCommonClass.SendPushByStaffCodeList(StaffCodeListCT, PUSH_FUNTIONNAME_CT_CHT)

                    ' Push完了フラグをTrueへ
                    pushCompleteFlg = True
                End If

                '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

                '着工指示／着工指示キャンセル
                If Not IsNothing(instructStatusList) AndAlso 0 < instructStatusList.Count Then

                    '着工指示の変更がある作業内容IDリストを元に、該当チップのストールIDリストを取得
                    Dim stallDt As SC3240201DataSet.SC3240201PushStallIdDataTable = _
                            ta.GetStallIdListByJobDtlId(arg.DlrCD, arg.StrCD, arg.SvcInId, instructStatusList)

                    '2017/11/20 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
                    'If stallDt.Count = 0 Then
                    '    Return returnCode
                    'End If

                    'ストールIDリストが取得できた場合
                    If 0 < stallDt.Count Then
                        '2017/11/20 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END

                        'PUSH送信用のストールIDリストを作成
                        Dim stallIdList As New List(Of Decimal)
                        '通知送信用のストールIDリストを作成
                        Dim stallIdListForNotice As New List(Of Decimal)

                        For i = 0 To stallDt.Rows.Count - 1

                            Dim selectStallDr As SC3240201DataSet.SC3240201PushStallIdRow = DirectCast(stallDt.Rows(i), SC3240201DataSet.SC3240201PushStallIdRow)
                            selectStallDr.INSTRUCT_STATUS = String.Empty

                            For j = 0 To instructStatusList.Count - 1
                                '着工指示の状態を設定
                                If instructStatusList.Keys(j).Equals(selectStallDr.JOB_DTL_ID) Then
                                    selectStallDr.INSTRUCT_STATUS = instructStatusList.Values(j)
                                    Exit For
                                End If
                            Next

                            'PUSH送信用のストールIDリストに追加
                            If Not stallIdList.Contains(selectStallDr.STALL_ID) Then
                                stallIdList.Add(selectStallDr.STALL_ID)
                            End If

                            '通知送信用のストールIDリストに追加
                            stallIdListForNotice.Add(selectStallDr.STALL_ID)

                            '■TC,ChTの通知API処理 (指定アカウント)
                            'TCのスタッフコードリストを取得
                            Dim StaffCodeListTC As List(Of String) = clsTabletSMBCommonClass.GetSendStaffCodeTC(arg.DlrCD, arg.StrCD, stallIdListForNotice)

                            'ChTのスタッフコードリストを取得
                            Dim StaffCodeListChT As List(Of String) = clsTabletSMBCommonClass.GetSendStaffCodeCht(arg.DlrCD, arg.StrCD, stallIdListForNotice)
                            Dim exceptStaffCode As String = ""
                            For Each staffCodeCht In StaffCodeListChT
                                If staffCodeCht.Equals(objStaffContext.Account) Then
                                    '自アカウントと同じアカウントが存在する場合
                                    exceptStaffCode = staffCodeCht
                                    Exit For
                                End If
                            Next
                            If Not String.IsNullOrEmpty(exceptStaffCode) Then
                                '自アカウントを除く
                                StaffCodeListChT.Remove(exceptStaffCode)
                            End If

                            '通知送信用のストールIDリストを次回送信用に更新
                            stallIdListForNotice.Remove(selectStallDr.STALL_ID)

                            Using clsTabletSMBCommonClassNoticeInfo As New TabletSmbCommonClassNoticeInfoDataTable
                                Dim dr As TabletSmbCommonClassNoticeInfoRow = clsTabletSMBCommonClassNoticeInfo.NewTabletSmbCommonClassNoticeInfoRow

                                '通知処理の共通項目に設定する文字列
                                returnCode = SetCommonContents(arg, dr, objStaffContext)
                                If returnCode < 0 Then
                                    Return returnCode
                                End If

                                dr.BASREZID = ConvertDbNullToEmpty(stallDt.Rows(i)("DMS_JOB_DTL_ID")).Trim()

                                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} [BASREZID:{2}]" _
                                                          , Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name, dr.BASREZID))

                                '通知処理の「表示内容」項目に設定する文字列を作成
                                dr.Message = SetDispContents(arg, _
                                                             ConvertDbNullToEmpty(stallDt.Rows(i)("INSTRUCT_STATUS")), _
                                                             dr.CST_NAME, _
                                                             ConvertDbMinDateToNull(stallDt.Rows(i)("SCHE_START_DATETIME")), _
                                                             ConvertDbMinDateToNull(stallDt.Rows(i)("SCHE_END_DATETIME")), _
                                                             ConvertDbNullToEmpty(stallDt.Rows(i)("SVC_CLASS_NAME")).Trim(), _
                                                             ConvertDbNullToEmpty(stallDt.Rows(i)("MERC_NAME")).Trim(), _
                                                             ConvertDbNullToEmpty(stallDt.Rows(i)("UPPER_DISP")).Trim(), _
                                                             ConvertDbNullToEmpty(stallDt.Rows(i)("LOWER_DISP")).Trim(), _
                                                             updDate)

                                'TC通知処理の実行
                                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} [Execute Notice To TC. SendCount:{2}]" _
                                                          , Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name, i + 1))
                                clsTabletSMBCommonClass.Notice(StaffCodeListTC, objStaffContext, dr)

                                'ChT通知処理の実行
                                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} [Execute Notice To ChT. SendCount:{2}]" _
                                                          , Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name, i + 1))
                                clsTabletSMBCommonClass.Notice(StaffCodeListChT, objStaffContext, dr)
                            End Using
                        Next

                        '■TCのPush処理 (指定アカウント)
                        'TCのスタッフコードリストを取得
                        Dim StaffCodeListTC2 As List(Of String) = clsTabletSMBCommonClass.GetSendStaffCodeTC(arg.DlrCD, arg.StrCD, stallIdList)
                        'TCにPush送信
                        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} [Execute Push To TC (Specify Staff).]", Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name))
                        clsTabletSMBCommonClass.SendPushByStaffCodeList(StaffCodeListTC2, PUSH_FUNTIONNAME_TC)

                        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                        ' Push完了フラグがFalseの場合
                        If Not pushCompleteFlg Then
                            Using serviceCommonbiz As New ServiceCommonClassBusinessLogic
                                ' 権限コードリスト生成(CT・ChT)
                                Dim stuffCodeList As New List(Of Decimal)
                                stuffCodeList.Add(Operation.CT)
                                stuffCodeList.Add(Operation.CHT)

                                ' ストールIDよりPush通知アカウントリスト取得
                                Dim staffInfoDataTable As ServiceCommonClassDataSet.StaffInfoDataTable
                                staffInfoDataTable = serviceCommonbiz.GetNoticeSendAccountListToStall(arg.DlrCD, arg.StrCD, stallIdList, stuffCodeList)

                                ' 自分以外の場合Push送信する
                                Dim pushUsersList As List(Of String) = New List(Of String)
                                For Each row As ServiceCommonClassDataSet.StaffInfoRow In staffInfoDataTable.Rows
                                    If Not String.Equals(row.ACCOUNT, objStaffContext.Account) Then
                                        pushUsersList.Add(row.ACCOUNT)
                                    End If
                                Next
                                clsTabletSMBCommonClass.SendPushByStaffCodeList(pushUsersList, PUSH_FUNTIONNAME_CT_CHT)
                            End Using
                        End If

                        ''■ChTのPush処理 (全Cht送付:(自アカウントは除く))
                        'Dim exceptStaffCode2 As String = ""

                        ''ChTのオペレーションコードをセット
                        'operationCodeList.Add(Operation.CHT)
                        ''全ChTのスタッフコードリストを取得
                        'Dim StaffCodeListChT2 As List(Of String) = clsTabletSMBCommonClass.GetSendStaffCode(arg.DlrCD, arg.StrCD, operationCodeList)
                        'operationCodeList.Remove(Operation.CHT)

                        'For Each staffCodeChT2 In StaffCodeListChT2
                        '    If staffCodeChT2.Equals(objStaffContext.Account) Then
                        '        '自アカウントと同じアカウントが存在する場合
                        '        exceptStaffCode2 = staffCodeChT2
                        '        Exit For
                        '    End If
                        'Next
                        'If Not String.IsNullOrEmpty(exceptStaffCode2) Then
                        '    '自アカウントを除く
                        '    StaffCodeListChT2.Remove(exceptStaffCode2)
                        'End If
                        ''全ChTにPush送信(自身と関連チップを除く)
                        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} [Execute Push To ChT (All Staff Except Own Account).]", Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name))
                        'clsTabletSMBCommonClass.SendPushByStaffCodeList(StaffCodeListChT2, PUSH_FUNTIONNAME_CT_CHT)


                        ''■CTのPush処理 (全CT送付:(自アカウントは除く))
                        ''CTのオペレーションコードをセット
                        'operationCodeList.Add(Operation.CT)
                        ''全CTのスタッフコードリストを取得
                        'Dim StaffCodeListCT As List(Of String) = clsTabletSMBCommonClass.GetSendStaffCode(arg.DlrCD, arg.StrCD, operationCodeList)
                        'operationCodeList.Remove(Operation.CT)

                        'exceptStaffCode2 = ""
                        'For Each staffCodeCT In StaffCodeListCT
                        '    If staffCodeCT.Equals(objStaffContext.Account) Then
                        '        '自アカウントと同じアカウントが存在する場合
                        '        exceptStaffCode2 = staffCodeCT
                        '        Exit For
                        '    End If
                        'Next
                        'If Not String.IsNullOrEmpty(exceptStaffCode2) Then
                        '    '自アカウントを除く
                        '    StaffCodeListCT.Remove(exceptStaffCode2)
                        'End If

                        ''全CTにPush送信(自身を除く)
                        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} [Execute Push To CT (All Staff Except Own Account).]", Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name))
                        'clsTabletSMBCommonClass.SendPushByStaffCodeList(StaffCodeListCT, PUSH_FUNTIONNAME_CT_CHT)
                        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

                        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
                        '■PSのPush処理
                        'PSのオペレーションコードをセット
                        'operationCodeList.Add(Operation.PS)
                        ''全PSのスタッフコードリストを取得
                        'Dim StaffCodeListPS As List(Of String) = clsTabletSMBCommonClass.GetSendStaffCode(arg.DlrCD, arg.StrCD, operationCodeList)
                        'operationCodeList.Remove(Operation.PS)
                        ''全PSにPush送信
                        'Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} [Execute Push To PS (All Staff).]", Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name))
                        'clsTabletSMBCommonClass.SendPushByStaffCodeList(StaffCodeListPS, PUSH_FUNTIONNAME_PS)
                    End If
                    '2017/11/20 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
                End If

                '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
                '整備の紐付け状態が変更された場合、Push送信する
                If Me.IsChangeInstruct(arg) Then

                    '■PSのPush処理
                    'PSのオペレーションコードをセット
                    operationCodeList.Add(Operation.PS)
                    '全PSのスタッフコードリストを取得
                    Dim StaffCodeListPS As List(Of String) = clsTabletSMBCommonClass.GetSendStaffCode(arg.DlrCD, arg.StrCD, operationCodeList)
                    operationCodeList.Remove(Operation.PS)
                    '全PSにPush送信
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} [Execute Push To PS (All Staff).]", Me.GetType.ToString, System.Reflection.MethodBase.GetCurrentMethod.Name))
                    clsTabletSMBCommonClass.SendPushByStaffCodeList(StaffCodeListPS, PUSH_FUNTIONNAME_PS)
                End If
                '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
            Finally
                ta = Nothing
            End Try
        End Using

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        Return returnCode
    End Function
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>
    ''' サブエリアのリフレッシュ有無判断
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckSubAreaRefresh(ByVal arg As CallBackArgumentClass) As Boolean

        ' ROシーケンスのリスト生成
        Dim roSeqList As New List(Of String)
        For i = 0 To arg.ROJobSeqList.Count - 1
            If Not roSeqList.Contains(arg.ROJobSeqList(i)) Then
                ' リストに含まれていない場合は追加
                roSeqList.Add(arg.ROJobSeqList(i))
            End If
        Next

        ' 変更前着工指示フラグ
        Dim beforeInstructFlg As Long = 0
        ' 変更後着工指示フラグ
        Dim afterInstructFlg As Long = 0
        ' ROシーケンスに紐づくJob数
        Dim jobCount As Long = 0
        ' ROシーケンスに紐づく着工指示済みのJob数(変更前)
        Dim beforeJobInstructCount As Long = 0
        ' ROシーケンスに紐づく着工指示済みのJob数(変更後)
        Dim afterJobInstructCount As Long = 0

        ' ROシーケンス単位でJobの着工指示を確認する
        For Each targetRoSeq As String In roSeqList
            ' 初期化
            beforeInstructFlg = 0
            afterInstructFlg = 0
            jobCount = 0
            beforeJobInstructCount = 0
            afterJobInstructCount = 0

            ' Job分ループ
            For i = 0 To arg.JobInstructIdList.Count - 1
                '処理対象のROシーケンスかどうか
                If targetRoSeq = arg.ROJobSeqList(i) Then
                    ' 変更前が着工指示済みか否か
                    If arg.BeforeMatchingRezIdList(i) <> "-1" Then
                        ' Job単位で着工指示済みならカウントアップ
                        beforeJobInstructCount = beforeJobInstructCount + 1
                    End If

                    ' 変更後が着工指示済みか否か
                    If arg.MatchingRezIdList(i) <> "-1" Then
                        ' Job単位で着工指示済みならカウントアップ
                        afterJobInstructCount = afterJobInstructCount + 1
                    End If
                    jobCount = jobCount + 1
                End If
            Next

            ' 変更前の状態確認
            If beforeJobInstructCount = 0 Then
                ' すべてのJobが着工指示前
                beforeInstructFlg = JobInsTypeAllJobBefore
            ElseIf beforeJobInstructCount = jobCount Then
                ' すべてのJobが着工指示済み
                beforeInstructFlg = JobInsTypeAllJobAlready
            Else
                ' 1部のJobが着工指示済み
                beforeInstructFlg = JobInsTypePartJobAlready
            End If

            ' 変更後のROシーケンス毎の状態確認
            If afterJobInstructCount = 0 Then
                ' すべてのJobが着工指示前
                afterInstructFlg = JobInsTypeAllJobBefore
            ElseIf afterJobInstructCount = jobCount Then
                ' すべてのJobが着工指示済み
                afterInstructFlg = JobInsTypeAllJobAlready
            Else
                ' 1部のJobが着工指示済み
                afterInstructFlg = JobInsTypePartJobAlready
            End If

            ' 以下のいづれかに該当する場合サブエリアのリフレッシュが必要
            ' ① 変更前(全てのJobが着工指示前) ⇒ 変更後(全てのJobが着工指示済)
            ' ② 変更前(一部のJobが着工指示済) ⇒ 変更後(全てのJobが着工指示済)
            ' ③ 変更前(全てのJobが着工指示済) ⇒ 変更後(一部のJobが着工指示済)
            ' ④ 変更前(全てのJobが着工指示済) ⇒ 変更後(全てのJobが着工指示前)
            If (beforeInstructFlg = JobInsTypeAllJobBefore AndAlso afterInstructFlg = JobInsTypeAllJobAlready) OrElse _
               (beforeInstructFlg = JobInsTypePartJobAlready AndAlso afterInstructFlg = JobInsTypeAllJobAlready) OrElse _
               (beforeInstructFlg = JobInsTypeAllJobAlready AndAlso afterInstructFlg = JobInsTypePartJobAlready) OrElse _
               (beforeInstructFlg = JobInsTypeAllJobAlready AndAlso afterInstructFlg = JobInsTypeAllJobBefore) Then
                Return True
            End If
        Next

        ' サブエリアのリフレッシュ不要
        Return False
    End Function
    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
    ''' <summary>
    ''' 整備の紐付けチェック
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsChangeInstruct(ByVal arg As CallBackArgumentClass) As Boolean

        Dim fixItemCodeList As List(Of String) = arg.FixItemCodeList          '整備コードリスト

        '整備コードリストが存在する場合
        If Not IsNothing(fixItemCodeList) AndAlso 0 < fixItemCodeList.Count Then

            '整備コードリストの件数分Loop
            For i = 0 To fixItemCodeList.Count - 1

                '着工指示が変更されたかチェック
                If Not arg.BeforeMatchingRezIdList(i).Equals(arg.MatchingRezIdList(i)) Then

                    '変更あり
                    Return True
                End If
            Next
        End If

        '変更なし
        Return False
    End Function
    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
#End Region

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
#Region "開始処理"
    ''' <summary>
    ''' 全開始処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltStartDateTime">実績開始日時</param>
    ''' <param name="restFlg">休憩取得フラグ(0：休憩を取得しない　1：休憩を取得する)</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="restartStopJobFlg">中断Job再開フラグ true：中断Jobを再開始する、false：中断Jobを再開始しない</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function JobAllStart(ByVal stallUseId As Decimal, _
                                ByVal rsltStartDateTime As Date, _
                                ByVal restFlg As String, _
                                ByVal dtNow As Date, _
                                ByVal rowLockVersion As Long, _
                                Optional ByVal restartStopJobFlg As Boolean = True) As Long
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. stallUseId={1} rsltStartDateTime={2} restFlg={3} dtNow={4} rowLockVersion={5} restartStopJobFlg={6}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  stallUseId, _
                                  rsltStartDateTime, _
                                  restFlg, _
                                  dtNow, _
                                  rowLockVersion, _
                                  restartStopJobFlg))

        '返却変数
        Dim result As Long

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                'このクラスPushフラグをリセット(初期化)
                ResetPushFlg(clsTabletSMBCommonClass)

                '全開始共通関数を呼び出す
                result = clsTabletSMBCommonClass.Start(stallUseId, _
                                                       rsltStartDateTime, _
                                                       restFlg, _
                                                       dtNow, _
                                                       rowLockVersion, _
                                                       MY_PROGRAMID, _
                                                       restartStopJobFlg, _
                                                       TabletSMBCommonClassBusinessLogic.CallerTypeDetailAllJobAction)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                ''共通関数から返却された処理結果コードは0以外の場合はRollBackする
                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Me.Rollback = True
                '    'エラーログ出力
                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                               "{0}.E Error:Failed to JobAllStart.　ResultCode={1}", _
                '                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                               result))
                '    Return result
                'Else

                '    'このクラスPushフラグをリセット
                '    ResetPushFlg(clsTabletSMBCommonClass)

                '    '処理成功
                '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                '                              "{0}.E ResultCode={1}", _
                '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                              result))
                '    '0(成功)を返す
                '    Return TabletSMBCommonClassBusinessLogic.ActionResult.Success
                'End If

                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success _
                And result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '共通関数から返却された処理結果コードが下記の場合
                    '　　　0以外、かつ
                    '　-9000以外

                    'ロールバック実行
                    Me.Rollback = True

                    'エラーログ出力
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.E Error:Failed to JobAllStart.　ResultCode={1}", _
                                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                               result))
                Else
                    '処理成功

                    'このクラスPushフラグをリセット
                    ResetPushFlg(clsTabletSMBCommonClass)

                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                              "{0}.E ResultCode={1}", _
                                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                              result))

                End If

                Return result

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'エラーログ出力
                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E DBTimeOutError.", _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                'DBタイムアウトのエラーコードを返す
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError
            End Try

        End Using

    End Function

    ''' <summary>
    ''' 単独開始、再開始処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltStartDateTime">実績開始日時</param>
    ''' <param name="restFlg">休憩取得フラグ(0：休憩を取得しない　1：休憩を取得する)</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示シーケンス</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function JobSingleStart(ByVal stallUseId As Decimal, _
                                   ByVal rsltStartDateTime As Date, _
                                   ByVal restFlg As String, _
                                   ByVal dtNow As Date, _
                                   ByVal rowLockVersion As Long, _
                                   ByVal inJobInstructId As String, _
                                   ByVal inJobInstructSeq As Long) As Long

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. stallUseId={1} rsltStartDateTime={2} restFlg={3} dtNow={4} rowLockVersion={5} inJobInstructId={6} inJobInstructSeq={7}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  stallUseId, _
                                  rsltStartDateTime, _
                                  restFlg, _
                                  dtNow, _
                                  rowLockVersion, _
                                  inJobInstructId, _
                                  inJobInstructSeq))

        '返却変数
        Dim result As Long

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                'このクラスPushフラグをリセット(初期化)
                ResetPushFlg(clsTabletSMBCommonClass)

                '単独開始共通関数を呼び出す
                result = clsTabletSMBCommonClass.StartSingleJob(stallUseId, _
                                                                rsltStartDateTime, _
                                                                restFlg, _
                                                                inJobInstructId, _
                                                                inJobInstructSeq, _
                                                                dtNow, _
                                                                rowLockVersion, _
                                                                MY_PROGRAMID)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                ''共通関数から返却された処理結果コードは0以外の場合はRollBackする
                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Me.Rollback = True
                '    'エラーログ出力
                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                               "{0}.E Error:Failed to JobSingleStart.　ResultCode={1}", _
                '                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                               result))

                'Else

                '    'このクラスPushフラグをリセット
                '    ResetPushFlg(clsTabletSMBCommonClass)

                '    '処理成功
                '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                '                              "{0}.E ResultCode={1}", _
                '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                              result))
                'End If

                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success _
                And result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '共通関数から返却された処理結果コードが下記の場合
                    '　　　0以外、かつ
                    '　-9000以外

                    'ロールバック実行
                    Me.Rollback = True

                    'エラーログ出力
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.E Error:Failed to JobSingleStart.　ResultCode={1}", _
                                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                               result))

                Else
                    '処理成功

                    'このクラスPushフラグをリセット
                    ResetPushFlg(clsTabletSMBCommonClass)

                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                              "{0}.E ResultCode={1}", _
                                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                              result))
                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '結果コードを返す
                Return result

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'エラーログ出力
                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E DBTimeOutError.", _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                'DBタイムアウトのエラーコードを返す
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError
            End Try

        End Using

    End Function


#End Region

#Region "終了処理"
    ''' <summary>
    ''' 作業全完了操作
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltEndDateTime">実績終了日時</param>
    ''' <param name="restFlg">休憩取得フラグ(0：休憩を取得しない　1：休憩を取得する)</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function JobAllFinish(ByVal stallUseId As Decimal, _
                              ByVal rsltEndDateTime As Date, _
                              ByVal restFlg As String, _
                              ByVal dtNow As Date, _
                              ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                    "{0}.S. stallUseId={1} rsltEndDateTime={2} restFlg={3} dtNow={4} rowLockVersion={5}", _
                                    System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                    stallUseId, _
                                    rsltEndDateTime, _
                                    restFlg, _
                                    dtNow, _
                                    rowLockVersion))

        '返却変数
        Dim result As Long

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                'このクラスPushフラグをリセット(初期化)
                ResetPushFlg(clsTabletSMBCommonClass)

                '全終了共通関数を呼び出す
                result = clsTabletSMBCommonClass.Finish(stallUseId, _
                                                        rsltEndDateTime, _
                                                        restFlg, _
                                                        dtNow, _
                                                        rowLockVersion, _
                                                        MY_PROGRAMID)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                ''共通関数から返却された処理結果コードは0以外の場合はRollBackする
                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Me.Rollback = True
                '    'エラーログ出力
                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                               "{0}.E Error:Failed to JobAllFinish.　ResultCode={1}", _
                '                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                               result))
                '    Return result
                'Else

                '    'このクラスPushフラグをリセット
                '    ResetPushFlg(clsTabletSMBCommonClass)

                '    '処理成功
                '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                '                              "{0}.E ResultCode={1}", _
                '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                              result))

                '    Return TabletSMBCommonClassBusinessLogic.ActionResult.Success
                'End If

                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success _
                And result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '共通関数から返却された処理結果コードが下記の場合
                    '　　　0以外、かつ
                    '　-9000以外

                    'ロールバック実行
                    Me.Rollback = True

                    'エラーログ出力
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.E Error:Failed to JobAllFinish.　ResultCode={1}", _
                                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                               result))

                Else
                    '処理成功

                    'このクラスPushフラグをリセット
                    ResetPushFlg(clsTabletSMBCommonClass)

                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                              "{0}.E ResultCode={1}", _
                                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                              result))

                End If

                Return result

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'エラーログ出力
                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E DBTimeOutError.", _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                'DBタイムアウトのエラーコードを返す
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError
            End Try

        End Using

    End Function

    ''' <summary>
    ''' 作業単独完了操作
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="jobInstructId">作業指示ID</param>
    ''' <param name="jobInstructSeq">作業指示シーケンス</param>
    ''' <param name="rsltEndDateTime">実績終了日時</param>
    ''' <param name="restFlg">休憩取得フラグ(0：休憩を取得しない　1：休憩を取得する)</param>
    ''' <param name="dtNow">更新日時</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function JobSingleFinish(ByVal stallUseId As Decimal, _
                              ByVal jobInstructId As String, _
                              ByVal jobInstructSeq As Long, _
                              ByVal rsltEndDateTime As Date, _
                              ByVal restFlg As String, _
                              ByVal dtNow As Date, _
                              ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                    "{0}.S. stallUseId={1} jobInstructId={2} jobInstructSeq={3} rsltEndDateTime={4} restFlg={5} dtNow={6} rowLockVersion={7}", _
                                    System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                    stallUseId, _
                                    jobInstructId, _
                                    jobInstructSeq, _
                                    rsltEndDateTime, _
                                    restFlg, _
                                    dtNow, _
                                    rowLockVersion))

        '返却変数
        Dim result As Long

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                'このクラスPushフラグをリセット(初期化)
                ResetPushFlg(clsTabletSMBCommonClass)

                '単独終了共通関数を呼び出す
                result = clsTabletSMBCommonClass.FinishSingleJob(stallUseId, _
                                                                 rsltEndDateTime, _
                                                                 restFlg, _
                                                                 jobInstructId, _
                                                                 jobInstructSeq, _
                                                                 dtNow, _
                                                                 rowLockVersion, _
                                                                 MY_PROGRAMID)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                ''共通関数から返却された処理結果コードは0以外の場合はRollBackする
                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Me.Rollback = True
                '    'エラーログ出力
                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                               "{0}.End. Error:Failed to JobSingleFinish. resultCode={1}", _
                '                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                               result))

                'Else

                '    'このクラスPushフラグをリセット(初期化)
                '    ResetPushFlg(clsTabletSMBCommonClass)

                '    '処理成功
                '    'ログ出力
                '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                '                              "{0}.E ResultCode={1}", _
                '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                              result))
                'End If

                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success _
                And result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '共通関数から返却された処理結果コードが下記の場合
                    '　　　0以外、かつ
                    '　-9000以外

                    'ロールバック実行
                    Me.Rollback = True

                    'エラーログ出力
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.End. Error:Failed to JobSingleFinish. resultCode={1}", _
                                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                               result))

                Else
                    '処理成功

                    'このクラスPushフラグをリセット(初期化)
                    ResetPushFlg(clsTabletSMBCommonClass)

                    'ログ出力
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                              "{0}.E ResultCode={1}", _
                                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                              result))
                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '結果コードを返す
                Return result

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'エラーログ出力
                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E DBTimeOutError.", _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                'DBタイムアウトのエラーコードを返す
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError
            End Try

        End Using

    End Function

#End Region

#Region "中断処理"
    ''' <summary>
    '''   全中断処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltEndDateTime">実績終了日時</param>
    ''' <param name="stallWaitTime">中断時間</param>
    ''' <param name="stopMemo">中断メモ</param>
    ''' <param name="stopReasonType">中断原因区分</param>
    ''' <param name="restFlg">休憩取得フラグ(0：休憩を取得しない　1：休憩を取得する)</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    <EnableCommit()>
    Public Function JobAllStop(ByVal stallUseId As Decimal, _
                                     ByVal rsltEndDateTime As Date, _
                                     ByVal stallWaitTime As Long, _
                                     ByVal stopMemo As String, _
                                     ByVal stopReasonType As String, _
                                     ByVal restFlg As String, _
                                     ByVal updateDate As Date, _
                                     ByVal rowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. stallUseId={1} rsltEndDateTime={2} stallWaitTime={3} stopMemo={4} stopReasonType={5} restFlg={6} updateDate={7} rowLockVersion={8}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  stallUseId, _
                                  rsltEndDateTime, _
                                  stallWaitTime, _
                                  stopMemo, _
                                  stopReasonType, _
                                  restFlg, _
                                  updateDate, _
                                  rowLockVersion))

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                'このクラスPushフラグをリセット(初期化)
                ResetPushFlg(clsTabletSMBCommonClass)

                '全中断共通関数を呼び出す
                Dim result As Long = clsTabletSMBCommonClass.JobStop(stallUseId, _
                                                                     rsltEndDateTime, _
                                                                     stallWaitTime, _
                                                                     stopMemo, _
                                                                     stopReasonType, _
                                                                     restFlg, _
                                                                     updateDate, _
                                                                     rowLockVersion, _
                                                                     MY_PROGRAMID)

                '中断により生成された非稼動エリアチップID
                NewStallIdleId = clsTabletSMBCommonClass.NewStallIdleId

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                ''共通関数から返却された処理結果コードは0以外の場合はRollBackする
                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Me.Rollback = True
                '    'エラーログ出力
                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                               "{0}.End. Error:Failed to JobAllStop. resultCode={1}", _
                '                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                               result))

                'Else

                '    'このクラスPushフラグをリセット(初期化)
                '    ResetPushFlg(clsTabletSMBCommonClass)

                '    '処理成功
                '    'ログ出力
                '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                '          "{0}.E", _
                '          System.Reflection.MethodBase.GetCurrentMethod.Name))
                'End If

                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success _
                And result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '共通関数から返却された処理結果コードが下記の場合
                    '　　　0以外、かつ
                    '　-9000以外

                    'ロールバック実行
                    Me.Rollback = True

                    'エラーログ出力
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.End. Error:Failed to JobAllStop. resultCode={1}", _
                                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                               result))

                Else
                    '処理成功

                    'このクラスPushフラグをリセット(初期化)
                    ResetPushFlg(clsTabletSMBCommonClass)

                    'ログ出力
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                              "{0}.E", _
                                              System.Reflection.MethodBase.GetCurrentMethod.Name))
                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '結果コードを返す
                Return result

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'エラーログ出力
                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E DBTimeOutError.", _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                'DBタイムアウトのエラーコードを返す
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError
            End Try

        End Using


    End Function

    ''' <summary>
    '''   単独中断処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示SEQ</param>
    ''' <param name="rsltEndDateTime">実績終了日時</param>
    ''' <param name="stallWaitTime">中断時間</param>
    ''' <param name="stopMemo">中断メモ</param>
    ''' <param name="stopReasonType">中断原因区分</param>
    ''' <param name="restFlg">休憩取得フラグ(0：休憩を取得しない　1：休憩を取得する)</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    <EnableCommit()>
    Public Function JobSingleStop(ByVal stallUseId As Decimal, _
                                  ByVal inJobInstructId As String, _
                                  ByVal inJobInstructSeq As Long, _
                                  ByVal rsltEndDateTime As Date, _
                                  ByVal stallWaitTime As Long, _
                                  ByVal stopMemo As String, _
                                  ByVal stopReasonType As String, _
                                  ByVal restFlg As String, _
                                  ByVal updateDate As Date, _
                                  ByVal rowLockVersion As Long) As Long

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S.　stallUseId={1} inJobInstructId={2} inJobInstructSeq={3} rsltEndDateTime={4} stallWaitTime={5} stopMemo={6} stopReasonType={7} restFlg={8} updateDate={9} rowLockVersion={10}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  stallUseId, _
                                  inJobInstructId, _
                                  inJobInstructSeq, _
                                  rsltEndDateTime, _
                                  stallWaitTime, _
                                  stopMemo, _
                                  stopReasonType, _
                                  restFlg, _
                                  updateDate, _
                                  rowLockVersion))

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try

                'このクラスPushフラグをリセット(初期化)
                ResetPushFlg(clsTabletSMBCommonClass)

                '単独中断共通関数を呼び出す
                Dim result As Long = clsTabletSMBCommonClass.StopSingleJob(stallUseId, _
                                                                     rsltEndDateTime, _
                                                                     stallWaitTime, _
                                                                     stopMemo, _
                                                                     stopReasonType, _
                                                                     restFlg, _
                                                                     inJobInstructId, _
                                                                     inJobInstructSeq, _
                                                                     updateDate, _
                                                                     rowLockVersion, _
                                                                     MY_PROGRAMID)

                '中断により生成された非稼動エリアチップID
                NewStallIdleId = clsTabletSMBCommonClass.NewStallIdleId

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                ' ''共通関数から返却された処理結果コードは0以外の場合はRollBackする
                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Me.Rollback = True
                '    'エラーログ出力
                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                               "{0}.End. Error:Failed to JobSingleStop. resultCode={1}", _
                '                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                               result))

                'Else

                '    'このクラスPushフラグをリセット
                '    ResetPushFlg(clsTabletSMBCommonClass)

                '    '処理成功
                '    'ログ出力
                '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                '          "{0}.E", _
                '          System.Reflection.MethodBase.GetCurrentMethod.Name))

                'End If

                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success _
                And result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '共通関数から返却された処理結果コードが下記の場合
                    '　　　0以外、かつ
                    '　-9000以外

                    'ロールバック実行
                    Me.Rollback = True

                    'エラーログ出力
                    Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.End. Error:Failed to JobSingleStop. resultCode={1}", _
                                               System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                               result))

                Else
                    '処理成功

                    'このクラスPushフラグをリセット
                    ResetPushFlg(clsTabletSMBCommonClass)

                    'ログ出力
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.E", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name))

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '結果コードを返す
                Return result

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'エラーログ出力
                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E DBTimeOutError.", _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                'DBタイムアウトのエラーコードを返す
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError
            End Try

        End Using

    End Function
#End Region

#Region "ストール非稼働情報の取得"
    ''' <summary>
    ''' 非稼働情報を取得します
    ''' </summary>
    ''' <param name="stallIdList">ストールIDリスト</param>
    ''' <param name="dtShowDate">画面表示日付</param>
    ''' <param name="objStaffContext">ログイン情報</param>
    ''' <returns>ストール非稼働マスタ情報リスト</returns>
    ''' <remarks></remarks>
    Public Function GetAllIdleDateInfo(ByVal stallIdList As List(Of Decimal), ByVal dtShowDate As Date, _
                                 ByVal objStaffContext As StaffContext) As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallIdleInfoDataTable

        Dim dtIdleDate As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallIdleInfoDataTable

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            '営業開始と終了時間を取得する
            Dim dtBranchOperatingHours As TabletSmbCommonClassBranchOperatingHoursDataTable = _
                clsTabletSMBCommonClass.GetOneDayBrnOperatingHours(dtShowDate, _
                                              objStaffContext.DlrCD, _
                                              objStaffContext.BrnCD)

            'Nothingの場合、予期せぬエラーを出す
            If IsNothing(dtBranchOperatingHours) Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.End. ExceptionError:GetOneDayBrnOperatingHours" _
                                           , MethodBase.GetCurrentMethod.Name))
                Return Nothing

            End If

            '営業開始日時を設定する
            Dim stallStartTime As Date = dtBranchOperatingHours(0).SVC_JOB_START_TIME

            '営業終了日時を設定する
            Dim stallEndTime As Date = dtBranchOperatingHours(0).SVC_JOB_END_TIME
            '非稼働時間を取得
            dtIdleDate = clsTabletSMBCommonClass.GetAllIdleDateInfo(stallIdList, stallStartTime, stallEndTime)
        End Using

        Return dtIdleDate
    End Function
#End Region

    ''' <summary>
    ''' 作業中チップの履歴情報を取得(前回だけの履歴情報)
    ''' </summary>
    ''' <param name="stallUseIdList">ストール利用ID</param>
    ''' <returns>履歴情報</returns>
    ''' <remarks></remarks>
    Public Function GetWorkingChipHisInfo(ByVal stallUseIdList As List(Of Decimal)) As String
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            '作業中チップの履歴情報を取得し、Jsonデータに変換する
            Dim workingChipHis As String = _
                ChipDetailDataTableToJson(clsTabletSMBCommonClass.GetWorkingChipHis(stallUseIdList))

            'ログ出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E workingChipHis={1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      workingChipHis))
            'Jsonデータを返却
            Return workingChipHis

        End Using

    End Function

    ''' <summary>
    ''' 最初開始チップチェック
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <returns>True:最初開始チップ False:最初開始チップではない</returns>
    ''' <remarks></remarks>
    Public Function IsFirstStartChip(ByVal svcinId As Decimal) As Boolean

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                            "{0}.S.　svcinId={1}", _
                            System.Reflection.MethodBase.GetCurrentMethod.Name, _
                            svcinId))

        Dim staffContext As StaffContext = staffContext.Current
        '返却変数
        Dim firstStartChip As Boolean
        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            '共通関数でチェックを行う
            firstStartChip = clsTabletSMBCommonClass.IsFirstStartChip(staffContext.DlrCD, _
                                                                      staffContext.BrnCD, _
                                                                      svcinId)
        End Using

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                "{0}.E.　result={1}", _
                System.Reflection.MethodBase.GetCurrentMethod.Name, _
                firstStartChip))

        Return firstStartChip
    End Function

    ''' <summary>
    ''' 各操作の通知を出す
    ''' </summary>
    ''' <param name="argumentMethod">操作名</param>
    ''' <param name="argument">クライアントからもらった引数</param>
    ''' <param name="isFirstStartchipFlg">最初開始チップフラグ</param>
    ''' <remarks></remarks>
    Public Sub SendNotice(ByVal argumentMethod As String, _
                          ByVal argument As CallBackArgumentClass, _
                          ByVal isFirstStartchipFlg As Boolean)

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                            "{0}.S.　argumentMethod={1}  argument={2} isFirstStartchipFlg={3}", _
                            System.Reflection.MethodBase.GetCurrentMethod.Name, _
                            argumentMethod, _
                            argument, _
                            isFirstStartchipFlg))

        Dim objStaffContext As StaffContext = StaffContext.Current

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Select Case argumentMethod
                Case ALLSTART, SINGLESTART, RESTART

                    '開始通知を出す
                    If Me.NeedPushAfterStartSingleJob Then

                        clsTabletSMBCommonClass.SendNoticeByStart(objStaffContext, _
                                                                  argument.SvcInId, _
                                                                  CType(argument.StallId, Decimal), _
                                                                  isFirstStartchipFlg)
                    End If

                Case ALLFINISH, SINGLEFINISH

                    If Me.NeedPushAfterFinishSingleJob Then

                        '終了通知を出す
                        clsTabletSMBCommonClass.SendNoticeByFinish(objStaffContext, _
                                                                   argument.SvcInId, _
                                                                   CType(argument.StallId, Decimal), _
                                                                   MY_PROGRAMID)

                    ElseIf Me.NeedPushAfterStopSingleJob Then

                        '中断通知を出す
                        clsTabletSMBCommonClass.SendNoticeByJobStop(objStaffContext, _
                                                                    CType(argument.StallId, Decimal))

                    End If

                Case ALLSTOP, SINGLESTOP

                    If Me.NeedPushAfterStopSingleJob Then

                        '中断通知を出す
                        clsTabletSMBCommonClass.SendNoticeByJobStop(objStaffContext, _
                                                                    CType(argument.StallId, Decimal))

                    End If

            End Select

        End Using

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                "{0}.E.", _
                System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END
#End Region

#Region "Privateメソッド"

#Region "返却データ設定処理"

    ''' <summary>
    ''' チップ情報を画面表示用データ行に格納する
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="dt"></param>
    ''' <param name="arg"></param>
    ''' <remarks></remarks>
    Private Function SetDisplayChipData(ByVal ds As SC3240201DataSet, _
                                     ByVal dt As SC3240201DataSet.SC3240201ChipBaseInfoDataTable, _
                                     ByVal arg As CallBackArgumentClass) As SC3240201DataSet.SC3240201DispChipBaseInfoRow

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'DBから取得したチップ情報データテーブル
        Dim dr As SC3240201DataSet.SC3240201ChipBaseInfoRow = DirectCast(dt.Rows(0), SC3240201DataSet.SC3240201ChipBaseInfoRow)

        '表示用データテーブル・データ行
        Dim dispDt As SC3240201DataSet.SC3240201DispChipBaseInfoDataTable = ds.SC3240201DispChipBaseInfo
        Dim dispRow As SC3240201DataSet.SC3240201DispChipBaseInfoRow = dispDt.NewSC3240201DispChipBaseInfoRow()

        dispRow.DLR_CD = Me.ConvertDbNullToEmpty(dr.Item(dt.DLR_CDColumn))               '販売店コード
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'dispRow.SVCIN_ID = CLng(dr.Item(dt.SVCIN_IDColumn))                              'サービス入庫ID
        'dispRow.CST_ID = CLng(dr.Item(dt.CST_IDColumn))                                  '顧客ID
        'dispRow.VCL_ID = CLng(dr.Item(dt.VCL_IDColumn))                                  '車両ID
        dispRow.SVCIN_ID = CType(dr.Item(dt.SVCIN_IDColumn), Decimal)                    'サービス入庫ID
        dispRow.CST_ID = CType(dr.Item(dt.CST_IDColumn), Decimal)                        '顧客ID
        dispRow.VCL_ID = CType(dr.Item(dt.VCL_IDColumn), Decimal)                        '車両ID
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '車両情報
        dispRow.REG_NUM = Me.ConvertDbNullToEmpty(dr.Item(dt.REG_NUMColumn))             '車両登録No.
        dispRow.VCL_VIN = Me.ConvertDbNullToEmpty(dr.Item(dt.VCL_VINColumn))             'VIN
        dispRow.VCL_KATASHIKI = Me.ConvertDbNullToEmpty(dr.Item(dt.VCL_KATASHIKIColumn)) '車両型式
        dispRow.MODEL_NAME = Me.ConvertDbNullToEmpty(dr.Item(dt.MODEL_NAMEColumn))       '車種

        '顧客情報
        dispRow.CST_NAME = Me.ConvertDbNullToEmpty(dr.Item(dt.CST_NAMEColumn))           '顧客名
        dispRow.CST_MOBILE = Me.ConvertDbNullToEmpty(dr.Item(dt.CST_MOBILEColumn))       'Mobile
        dispRow.CST_PHONE = Me.ConvertDbNullToEmpty(dr.Item(dt.CST_PHONEColumn))         'Home
        dispRow.STF_NAME = Me.ConvertDbNullToEmpty(dr.Item(dt.STF_NAMEColumn))           '担当SA
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        dispRow.CST_ADDRESS = Me.ConvertDbNullToEmpty(dr.Item(dt.CST_ADDRESSColumn))     '顧客住所
        dispRow.FLEET_FLG = Me.ConvertDbNullToEmpty(dr.Item(dt.FLEET_FLGColumn))         '法人フラグ
        dispRow.CST_TYPE = Me.ConvertDbNullToEmpty(dr.Item(dt.CST_TYPEColumn))           '顧客種別
        dispRow.DMS_CST_CD = Me.ConvertDbNullOrWhiteSpaceToEmpty(dr.Item(dt.DMS_CST_CDColumn))       '顧客基幹顧客コード
        dispRow.NAMETITLE_NAME = Me.ConvertDbNullToEmpty(dr.Item(dt.NAMETITLE_NAMEColumn))   '敬称
        dispRow.POSITION_TYPE = Me.ConvertDbNullToEmpty(dr.Item(dt.POSITION_TYPEColumn))     '配置区分
        dispRow.DMS_JOB_DTL_ID = Me.ConvertDbNullToEmpty(dr.Item(dt.DMS_JOB_DTL_IDColumn))   '基幹作業内容ID
        dispRow.INVOICE_DATETIME = Me.ConvertDbMinDateToNull(dr.Item(dt.INVOICE_DATETIMEColumn))   '清算準備完了日時

        'ご用命
        dispRow.ORDERMEMO = Me.ConvertDbNullOrWhiteSpaceToEmpty(dr.Item(dt.JOB_DTL_MEMOColumn))

        'メモ
        If Not (String.IsNullOrWhiteSpace(dr.NEXT_SVCIN_INSPECTION_ADVICE)) Then
            If Not (String.IsNullOrWhiteSpace(dr.ADVICE)) Then
                dispRow.WORKMEMO = dr.NEXT_SVCIN_INSPECTION_ADVICE & vbCrLf & dr.ADVICE     '次回入庫点検アドバイス + 改行 + アドバイス
            Else
                dispRow.WORKMEMO = dr.NEXT_SVCIN_INSPECTION_ADVICE
            End If
        Else
            If Not (String.IsNullOrWhiteSpace(dr.ADVICE)) Then
                dispRow.WORKMEMO = dr.ADVICE
            Else
                dispRow.WORKMEMO = String.Empty
            End If
        End If
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'DateTimeSelector設定用日時
        '来店予定日時
        If Not dr.IsPLAN_VISITDATENull _
            AndAlso (dr.PLAN_VISITDATE > Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture)) Then
            dispRow.PLAN_VISITDATE = dr.PLAN_VISITDATE
        End If

        '納車予定日時
        If Not dr.IsPLAN_DELIDATENull _
            AndAlso (dr.PLAN_DELIDATE > Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture)) Then
            dispRow.PLAN_DELIDATE = dr.PLAN_DELIDATE
        End If

        '来店実績日時
        If Not dr.IsRESULT_VISITDATENull _
            AndAlso (dr.RESULT_VISITDATE > Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture)) Then
            dispRow.RESULT_VISITDATE = dr.RESULT_VISITDATE
        End If

        '納車実績日時
        If Not dr.IsRESULT_DELIDATENull _
            AndAlso (dr.RESULT_DELIDATE > Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture)) Then
            dispRow.RESULT_DELIDATE = dr.RESULT_DELIDATE
        End If

        'ラベルしかない来店実績、納車実績の属性保持用日時(yyyy/MM/dd HH:mm)
        dispRow.ATTR_RESULT_VISIT = Me.ConvertDbNullToEmptyDateString(dr.Item(dt.RESULT_VISITDATEColumn))
        dispRow.ATTR_RESULT_DELI = Me.ConvertDbNullToEmptyDateString(dr.Item(dt.RESULT_DELIDATEColumn))

        '画面表示用日時(MM/ddまたはHH:mm)                                                                                            
        dispRow.DISP_PLAN_VISIT = Me.GetDispDateTimeString(arg.ShowDate, dr.Item(dt.PLAN_VISITDATEColumn))          '来店予定
        dispRow.DISP_PLAN_DELI = Me.GetDispDateTimeString(arg.ShowDate, dr.Item(dt.PLAN_DELIDATEColumn))            '納車予定
        dispRow.DISP_RESULT_VISIT = Me.GetDispDateTimeString(arg.ShowDate, dr.Item(dt.RESULT_VISITDATEColumn))      '来店実績
        dispRow.DISP_RESULT_DELI = Me.GetDispDateTimeString(arg.ShowDate, dr.Item(dt.RESULT_DELIDATEColumn))        '納車実績

        dispRow.REZFLAG = dr.REZFLAG        '予約フラグ
        dispRow.WASHFLAG = dr.WASHFLAG      '洗車フラグ
        dispRow.WAITTYPE = dr.WAITTYPE      '待ち方フラグ
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        dispRow.INSPECTIONFLG = dr.INSPECTIONFLG  '完成検査フラグ
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        dispRow.SVC_STATUS = dr.SVC_STATUS               'サービス入庫.サービスステータス
        dispRow.STALL_USE_STATUS = Me.ConvertDbNullToEmpty(dr.Item(dt.STALL_USE_STATUSColumn))   'ストール利用.ストール利用ステータス

        dispRow.RESV_STATUS = dr.RESV_STATUS             'サービス入庫.予約ステータス
        dispRow.RO_NUM = dr.RO_NUM                       'RO番号
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'dispRow.RO_JOB_SEQ = Me.ConvertDbNullToMinusNum(dr.Item(dt.RO_JOB_SEQColumn))   'ストール利用.作業連番

        dispRow.VISIT_ID = Me.ConvertDbNullToMinusNum(dr.Item(dt.VISIT_IDColumn))                '訪問ID
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        dispRow.ROW_LOCK_VERSION = dr.ROW_LOCK_VERSION   '行ロックバージョン

        '*********************************************
        '* 受付・追加作業エリア以外の共通パラメータ
        '*********************************************
        If Not (SUBAREA_RECEPTION.Equals(arg.SubAreaId) Or SUBAREA_ADDWORK.Equals(arg.SubAreaId)) Then

            'DateTimeSelector設定用日時
            dispRow.PLAN_STARTDATE = dr.PLAN_STARTDATE          '作業開始予定日時
            dispRow.PLAN_ENDDATE = dr.PLAN_ENDDATE              '作業完了予定日時

            If Not dr.IsRESULT_STARTDATENull _
                AndAlso (dr.RESULT_STARTDATE > Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture)) Then '作業開始実績日時
                dispRow.RESULT_STARTDATE = dr.RESULT_STARTDATE
            End If

            If Not dr.IsRESULT_ENDDATENull _
                AndAlso (dr.RESULT_ENDDATE > Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture)) Then   '作業完了実績日時
                dispRow.RESULT_ENDDATE = dr.RESULT_ENDDATE
            End If

            '日時欄に表示用(MM/ddまたはHH:mm)
            dispRow.DISP_PLAN_START = Me.GetDispDateTimeString(arg.ShowDate, dr.Item(dt.PLAN_STARTDATEColumn))          '作業開始予定
            dispRow.DISP_PLAN_END = Me.GetDispDateTimeString(arg.ShowDate, dr.Item(dt.PLAN_ENDDATEColumn))              '作業終了予定
            dispRow.DISP_RESULT_START = Me.GetDispDateTimeString(arg.ShowDate, dr.Item(dt.RESULT_STARTDATEColumn))      '作業開始実績
            dispRow.DISP_RESULT_END = Me.GetDispDateTimeString(arg.ShowDate, dr.Item(dt.RESULT_ENDDATEColumn))          '作業終了実績

            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'dispRow.SVC_CLASS_ID = Me.ConvertDbNullToMinusNum(dr.Item(dt.SVC_CLASS_IDColumn))     '整備種類(サービス分類ID)
            dispRow.SVC_CLASS_ID = Me.ConvertDbNullToMinusNum_Decimal(dr.Item(dt.SVC_CLASS_IDColumn))                   '整備種類(サービス分類ID)
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            dispRow.SVCID_TIME = Me.ConvertDbNullToEmpty(dr.Item(dt.SVCID_TIMEColumn))            '整備種類(標準作業時間)
            dispRow.SVC_CLASS_NAME = Me.ConvertDbNullToEmpty(dr.Item(dt.SVC_CLASS_NAMEColumn))    '整備種類(サービス分類名称)

            dispRow.MERC_ID = Me.ConvertDbNullToMinusNum(dr.Item(dt.MERC_IDColumn))               '整備名(商品ID)
            dispRow.MERCID_TIME = Me.ConvertDbNullToEmpty(dr.Item(dt.MERCID_TIMEColumn))          '整備名(標準作業時間)
            dispRow.MERC_NAME = Me.ConvertDbNullToEmpty(dr.Item(dt.MERC_NAMEColumn))              '整備名(商品名称)
            dispRow.WORKTIME = CStr(dr.WORKTIME)     '作業時間
        End If

        '返却用データセットに編集データ行を追加
        'ds.SC3240201DispChipBaseInfo.AddSC3240201DispChipBaseInfoRow(dispRow)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        Return dispRow

    End Function

    '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 顧客情報、来店情報、SMBCommonClassから取得した情報を
    ' ''' 画面表示用データテーブルに格納する
    ' ''' </summary>
    ' ''' <param name="ds"></param>
    ' ''' <param name="customers"></param>
    ' ''' <param name="visit"></param>
    ' ''' <param name="smbCommonChipDetails"></param>
    ' ''' <param name="arg"></param>
    ' ''' <param name="dispRow"></param>
    ' ''' <remarks></remarks>
    'Private Sub SetDispOtherData(ByVal ds As SC3240201DataSet, _
    '                             ByVal customers As IC3800703SrvCustomerDataTable, _
    '                             ByVal visit As SC3240201DataSet.SC3240201VisitInfoDataTable, _
    '                             ByVal smbCommonChipDetails As CommonUtility.SMBCommonClass.Api.BizLogic.ChipDetail, _
    '                             ByVal arg As CallBackArgumentClass, _
    '                             ByVal dispRow As SC3240201DataSet.SC3240201DispChipBaseInfoRow)
    ' ''' <summary>
    ' ''' 顧客情報、来店情報、SMBCommonClassから取得した情報を
    ' ''' 画面表示用データテーブルに格納する
    ' ''' </summary>
    ' ''' <param name="ds"></param>
    ' ''' <param name="jdpFlg"></param>
    ' ''' <param name="visit"></param>
    ' ''' <param name="smbCommonChipDetails"></param>
    ' ''' <param name="arg"></param>
    ' ''' <param name="dispRow"></param>
    ' ''' <remarks></remarks>
    ' Private Sub SetDispOtherData(ByVal ds As SC3240201DataSet, _
    '                              ByVal jdpFlg As String, _
    '                              ByVal visit As SC3240201DataSet.SC3240201VisitInfoDataTable, _
    '                              ByVal smbCommonChipDetails As CommonUtility.SMBCommonClass.Api.BizLogic.ChipDetail, _
    '                              ByVal arg As CallBackArgumentClass, _
    '                              ByVal dispRow As SC3240201DataSet.SC3240201DispChipBaseInfoRow)
    ''' <summary>
    ''' 顧客情報、来店情報、SMBCommonClassから取得した情報を
    ''' 画面表示用データテーブルに格納する
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="jdpFlg"></param>
    ''' <param name="visit"></param>
    ''' <param name="smbCommonChipDetails"></param>
    ''' <param name="arg"></param>
    ''' <param name="dispRow"></param>
    ''' <param name="sscFlg">SSC対象フラグ</param>
    ''' <remarks></remarks>
    Private Sub SetDispOtherData(ByVal ds As SC3240201DataSet, _
                                 ByVal jdpFlg As String, _
                                 ByVal visit As SC3240201DataSet.SC3240201VisitInfoDataTable, _
                                 ByVal smbCommonChipDetails As CommonUtility.SMBCommonClass.Api.BizLogic.ChipDetail, _
                                 ByVal arg As CallBackArgumentClass, _
                                 ByVal dispRow As SC3240201DataSet.SC3240201DispChipBaseInfoRow, _
                                 ByVal sscFlg As String)
        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END
        '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '工程管理画面上で表示している日付
        Dim nowDate As Date = Date.Parse(arg.ShowDate, CultureInfo.InvariantCulture)

        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
        ''IC3800703(顧客参照)
        ''「JDP調査対象客マーク」、「SSCマーク」が取得できた場合
        'If customers IsNot Nothing AndAlso 0 < customers.Rows.Count Then
        '    Dim customersDr As IC3800703SrvCustomerFRow = DirectCast(customers.Rows(0), IC3800703SrvCustomerFRow)
        '    dispRow.IFLAG = Me.ConvertDbNullToStrZero(customersDr.Item(customers.JDPFLAGColumn))   '0:非表示／1:表示
        '    dispRow.SFLAG = Me.ConvertDbNullToStrZero(customersDr.Item(customers.SSCFLAGColumn))   '0:非表示／1:表示
        'Else
        '    dispRow.IFLAG = "0"   '非表示
        '    dispRow.SFLAG = "0"   '非表示
        'End If
        If Not String.IsNullOrWhiteSpace(jdpFlg) Then
            'JDPフラグ値が設定されている場合
            dispRow.IFLAG = jdpFlg      '0:非表示/1:表示
            dispRow.SFLAG = "0"         '非表示
        Else
            'JDPフラグ値が設定されていない場合
            dispRow.IFLAG = "0"         '非表示
            dispRow.SFLAG = "0"         '非表示
        End If
        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
        If Not String.IsNullOrWhiteSpace(sscFlg) Then
            'SSCフラグが設定されている場合
            dispRow.SFLAG = sscFlg
        Else
            'SSCフラグが設定されていない場合
            dispRow.SFLAG = "0"
        End If
        '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

        '来店情報
        '来店情報が取得できた場合
        If visit IsNot Nothing AndAlso 0 < visit.Rows.Count Then
            Dim visitDr As SC3240201DataSet.SC3240201VisitInfoRow = DirectCast(visit.Rows(0), SC3240201DataSet.SC3240201VisitInfoRow)
            dispRow.VISIT_VISITSEQ = Me.ConvertDbNullToMinusNum(visitDr.Item(visit.VISITSEQColumn))      '来店実績連番
            dispRow.VISIT_VCLREGNO = Me.ConvertDbNullToEmpty(visitDr.Item(visit.VCLREGNOColumn))         '車両登録番号
            dispRow.VISIT_VIN = Me.ConvertDbNullToEmpty(visitDr.Item(visit.VINColumn))                   'VIN
            dispRow.VISIT_TELNO = Me.ConvertDbNullToEmpty(visitDr.Item(visit.TELNOColumn))               '電話番号
            dispRow.VISIT_MOBILE = Me.ConvertDbNullToEmpty(visitDr.Item(visit.MOBILEColumn))             '携帯番号
            dispRow.VISIT_ASSIGNSTATUS = Me.ConvertDbNullToEmpty(visitDr.Item(visit.ASSIGNSTATUSColumn)) '振当ステータス
        Else
            dispRow.VISIT_VISITSEQ = -1
            dispRow.VISIT_VCLREGNO = String.Empty
            dispRow.VISIT_VIN = String.Empty
            dispRow.VISIT_TELNO = String.Empty
            dispRow.VISIT_MOBILE = String.Empty
            dispRow.VISIT_ASSIGNSTATUS = String.Empty
        End If

        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ''ステータス文言
        'dispRow.STATUSWORD = smbCommonChipDetails.Status

        ''ステータス表示エリアの納車予定時間
        'dispRow.DELIPLANTIME = SetDateTimeToStringDetail2(smbCommonChipDetails.DeliveryPlanDateTime, nowDate)

        ''納車予定時刻変更回数
        'If smbCommonChipDetails.DeliveryPlanDateUpdateCount = 0 Then
        '    dispRow.CHGCNTWORD = String.Empty
        '    dispRow.CHGCNT = 0
        'Else
        '    dispRow.CHGCNTWORD = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(MY_PROGRAMID, 5), _
        '                                   smbCommonChipDetails.DeliveryPlanDateUpdateCount)
        '    dispRow.CHGCNT = smbCommonChipDetails.DeliveryPlanDateUpdateCount
        'End If

        ''納車見込み日時
        'dispRow.DELIPROSPECTSTIME = SetDateTimeToStringDetail2(smbCommonChipDetails.DeliveryHopeDateTime, nowDate)

        ''ご用命
        'If String.IsNullOrEmpty(smbCommonChipDetails.OrderMemo) Then
        '    dispRow.ORDERMEMO = String.Empty
        'Else
        '    dispRow.ORDERMEMO = smbCommonChipDetails.OrderMemo
        'End If

        ''故障原因
        'If String.IsNullOrEmpty(smbCommonChipDetails.FailureCause) Then
        '    dispRow.FAILURECAUSE = String.Empty
        'Else
        '    dispRow.FAILURECAUSE = smbCommonChipDetails.FailureCause
        'End If

        ''診断結果
        'If String.IsNullOrEmpty(smbCommonChipDetails.DiagnosticResult) Then
        '    dispRow.DIAGNOSTICRESULT = String.Empty
        'Else
        '    dispRow.DIAGNOSTICRESULT = smbCommonChipDetails.DiagnosticResult
        'End If

        ''作業結果及びアドバイス
        'If String.IsNullOrEmpty(smbCommonChipDetails.WorkResultAdvice) Then
        '    dispRow.WORKRESULTADVICE = String.Empty
        'Else
        '    dispRow.WORKRESULTADVICE = smbCommonChipDetails.WorkResultAdvice
        'End If

        ''顧客区分
        'dispRow.CUSTTYPE = smbCommonChipDetails.CustomerType

        '共通クラスのチップ情報がある場合
        If smbCommonChipDetails IsNot Nothing Then
            'ステータス文言
            dispRow.STATUSWORD = smbCommonChipDetails.Status

            'ステータス表示エリアの納車予定時間
            dispRow.DELIPLANTIME = SetDateTimeToStringDetail2(smbCommonChipDetails.DeliveryPlanDateTime, nowDate)

            '納車予定時刻変更回数
            If smbCommonChipDetails.DeliveryPlanDateUpdateCount = 0 Then
                dispRow.CHGCNTWORD = String.Empty
                dispRow.CHGCNT = 0
            Else
                dispRow.CHGCNTWORD = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(MY_PROGRAMID, 5), _
                                               smbCommonChipDetails.DeliveryPlanDateUpdateCount)
                dispRow.CHGCNT = smbCommonChipDetails.DeliveryPlanDateUpdateCount
            End If

            '納車見込み日時
            dispRow.DELIPROSPECTSTIME = SetDateTimeToStringDetail2(smbCommonChipDetails.DeliveryHopeDateTime, nowDate)
        Else
            'ステータス文言
            dispRow.STATUSWORD = ""

            'ステータス表示エリアの納車予定時間
            dispRow.DELIPLANTIME = WebWordUtility.GetWord(MY_PROGRAMID, 10)

            '納車予定時刻変更回数
            dispRow.CHGCNTWORD = String.Empty
            dispRow.CHGCNT = 0

            '納車見込み日時
            dispRow.DELIPROSPECTSTIME = WebWordUtility.GetWord(MY_PROGRAMID, 10)
        End If
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '返却用データセットに編集データ行を追加
        ds.SC3240201DispChipBaseInfo.AddSC3240201DispChipBaseInfoRow(dispRow)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' APIから取得した親RO＋追加作業の整備情報を元に、整備情報表示用データテーブルに値を設定する
    ' ''' </summary>
    ' ''' <param name="ds">返却用データセット</param>
    ' ''' <param name="maintenanceDt">整備情報【親RO＋追加作業の情報】</param>
    ' ''' <param name="chipDt">リレーションチップ情報</param>
    ' ''' <param name="arg">引数クラスオブジェクト</param>
    ' ''' <param name="roJobSeq">RO作業連番</param>
    ' ''' <remarks></remarks>
    'Private Sub SetDispMaintenanceData(ByVal ds As SC3240201DataSet, _
    '                                   ByVal maintenanceDt As IC3801015DataSet.IC3801015ServiceInfoDataTable, _
    '                                   ByVal chipDt As SC3240201DataSet.SC3240201RelatedChipInfoDataTable, _
    '                                   ByVal arg As CallBackArgumentClass, _
    '                                   ByVal roJobSeq As Long)

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

    '    'チップ詳細(小)の整備内容エリア、チップ詳細(大)の整備内容エリアのリピーターに使用する
    '    Dim dispDt As SC3240201DataSet.SC3240201DispMaintenanceListDataTable = ds.SC3240201DispMaintenanceList

    '    'チップ詳細(大)のチップスクロールエリアのリピーターに使用する
    '    Dim dispDt2 As SC3240201DataSet.SC3240201DispMaintenanceList2DataTable = ds.SC3240201DispMaintenanceList2

    '    '画面のNo.項目に表示するインデックス
    '    Dim index As Integer = 0

    '    Dim selectChipDr As DataRow()
    '    Dim mainteStallUseState As String

    '    'エリアにより、取得データを絞り込む
    '    Dim selectDrList As DataRow()
    '    If arg.SubAreaId.Equals(SUBAREA_RECEPTION) Then
    '        '受付エリアの場合

    '        '顧客承認後、且つ、自チップの作業連番で絞り込む
    '        selectDrList = maintenanceDt.Select("customerConfirmFlag = '1' AND workSeq = " & roJobSeq)

    '    ElseIf arg.SubAreaId.Equals(SUBAREA_ADDWORK) Then
    '        '追加作業エリアの場合

    '        '顧客承認前、且つ、自チップの作業連番で絞り込む
    '        selectDrList = maintenanceDt.Select("customerConfirmFlag = '0' AND workSeq = " & roJobSeq)

    '    Else
    '        '上記以外の場合

    '        '顧客承認後で絞り込む
    '        selectDrList = maintenanceDt.Select("customerConfirmFlag = '1'")
    '    End If

    '    '整備情報の事前バインディング
    '    Dim srvName As DataColumn = maintenanceDt.srvNameColumn
    '    Dim srvTypeName As DataColumn = maintenanceDt.srvTypeNameColumn
    '    Dim srvCode As DataColumn = maintenanceDt.srvCodeColumn
    '    Dim srvSeq As DataColumn = maintenanceDt.srvSeqColumn
    '    Dim workByCode As DataColumn = maintenanceDt.workByCodeColumn
    '    Dim rezId As DataColumn = maintenanceDt.REZIDColumn
    '    Dim workSeq As DataColumn = maintenanceDt.workSeqColumn
    '    Dim addSeq As DataColumn = maintenanceDt.addSeqColumn

    '    'リレーションチップ情報の事前バインディング
    '    Dim stallUseStatus As DataColumn = chipDt.STALL_USE_STATUSColumn

    '    '整備情報【親RO＋追加作業の情報】分、Loop
    '    For Each dr In selectDrList

    '        Dim selectDr As IC3801015DataSet.IC3801015ServiceInfoRow = DirectCast(dr, IC3801015DataSet.IC3801015ServiceInfoRow)

    '        'No.インデックスをインクリメント
    '        index += 1

    '        '******************************
    '        '* チップ詳細(小)用
    '        '******************************
    '        Dim dispRow As SC3240201DataSet.SC3240201DispMaintenanceListRow = dispDt.NewSC3240201DispMaintenanceListRow

    '        dispRow.INDEX = index                                                         'No.
    '        dispRow.MAINTENAME = Me.ConvertDbNullToEmpty(selectDr(srvName))               '整備名称
    '        dispRow.MAINTETYPENAME = Me.ConvertDbNullToEmpty(selectDr(srvTypeName))       '整備区分名称
    '        dispRow.MAINTECODE = Me.ConvertDbNullToEmpty(selectDr(srvCode))               '整備コード
    '        dispRow.MAINTESEQ = Me.ConvertDbNullToEmpty(selectDr(srvSeq))                 '整備連番
    '        dispRow.WORKGROUP = Me.ConvertDbNullToEmpty(selectDr(workByCode))             '作業グループ
    '        dispRow.REZID = Me.ConvertRezId(selectDr(rezId))                              '予約ID
    '        dispRow.ROJOBSEQ = Me.ConvertDbNullToMinusNum(selectDr(workSeq))              '作業連番
    '        dispRow.SRVADDSEQ = Me.ConvertDbNullToMinusNum(selectDr(addSeq))              '枝番

    '        '【親ROの情報】は「R/O」
    '        If dispRow.SRVADDSEQ = 0 Then
    '            dispRow.SRVADDSEQCONTENTS = WebWordUtility.GetWord(MY_PROGRAMID, 54)
    '        Else
    '            '【追加作業の情報】は「追加N」　※Nは枝番
    '            dispRow.SRVADDSEQCONTENTS = String.Format(CultureInfo.InvariantCulture, _
    '                                                      WebWordUtility.GetWord(MY_PROGRAMID, 55), _
    '                                                      selectDr.addSeq.ToString(CultureInfo.CurrentCulture))
    '        End If

    '        'APIから受け取った作業内容IDをキーに、
    '        'リレーションチップ情報から該当レコードのストール利用ステータスを取得する
    '        selectChipDr = chipDt.Select("JOB_DTL_ID = " & Me.ConvertRezId(selectDr(rezId)))

    '        If selectChipDr IsNot Nothing AndAlso 0 < selectChipDr.Count Then
    '            mainteStallUseState = Me.ConvertDbNullToEmpty(selectChipDr(0)(stallUseStatus))
    '            dispRow.STALL_USE_STATUS = mainteStallUseState    'ストール利用ステータス
    '        Else
    '            dispRow.STALL_USE_STATUS = String.Empty           'ストール利用ステータス
    '        End If

    '        '返却用データセットに行を追加
    '        ds.SC3240201DispMaintenanceList.AddSC3240201DispMaintenanceListRow(dispRow)

    '        '******************************
    '        '* チップ詳細(大)用
    '        '******************************
    '        Dim dispRow2 As SC3240201DataSet.SC3240201DispMaintenanceList2Row = dispDt2.NewSC3240201DispMaintenanceList2Row

    '        dispRow2.INDEX = index
    '        dispRow2.MAINTESEQ = Me.ConvertDbNullToEmpty(selectDr(srvSeq))       '整備連番
    '        dispRow2.REZID = Me.ConvertRezId(selectDr(rezId))                    '予約ID

    '        '返却用データセットに行を追加
    '        ds.SC3240201DispMaintenanceList2.AddSC3240201DispMaintenanceList2Row(dispRow2)
    '    Next

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    'End Sub
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 作業指示の情報を元に、整備情報表示用データテーブルに値を設定する
    ''' </summary>
    ''' <param name="ds">返却用データセット</param>
    ''' <param name="jobInstructDt">作業指示情報</param>
    ''' <param name="chipDt">リレーションチップ情報</param>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="roJobSeq">RO作業連番</param>
    ''' <remarks></remarks>
    Private Sub SetDispjobInstructData(ByVal ds As SC3240201DataSet, _
                                       ByVal jobInstructDt As SC3240201DataSet.SC3240201JobInstructListDataTable, _
                                       ByVal chipDt As SC3240201DataSet.SC3240201RelatedChipInfoDataTable, _
                                       ByVal arg As CallBackArgumentClass, _
                                       ByVal roJobSeq As Long)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'チップ詳細(小)の整備内容エリア、チップ詳細(大)の整備内容エリアのリピーターに使用する
        Dim dispDt As SC3240201DataSet.SC3240201JobInstructListDataTable = ds.SC3240201JobInstructList

        'チップ詳細(大)のチップスクロールエリアのリピーターに使用する
        Dim dispDt2 As SC3240201DataSet.SC3240201JobInstructList2DataTable = ds.SC3240201JobInstructList2

        '画面のNo.項目に表示するインデックス
        Dim index As Integer = 0

        Dim selectChipDr As DataRow()
        Dim mainteStallUseState As String

        'エリアにより、取得データを絞り込む
        Dim selectDrList As DataRow()
        If SUBAREA_RECEPTION.Equals(arg.SubAreaId) Then
            '受付エリアの場合

            '自チップの作業内容ID + 作業連番で絞り込む
            selectDrList = jobInstructDt.Select("RO_SEQ = " & roJobSeq)
        ElseIf SUBAREA_ADDWORK.Equals(arg.SubAreaId) Then
            '追加作業エリアの場合

            '自チップの作業連番で絞り込む
            selectDrList = jobInstructDt.Select("RO_SEQ = " & roJobSeq)
        Else
            '上記以外の場合
            selectDrList = jobInstructDt.Select
        End If

        '整備情報の事前バインディング
        Dim jobNameClm As DataColumn = jobInstructDt.JOB_NAMEColumn                 '作業名
        Dim oprTypeNameClm As DataColumn = jobInstructDt.OPERATION_TYPE_NAMEColumn  'オペレーション区分名称
        Dim jobCdClm As DataColumn = jobInstructDt.JOB_CDColumn                     '作業コード
        Dim roJobSeqClm As DataColumn = jobInstructDt.RO_SEQColumn                  'RO連番
        Dim jobDtlIdClm As DataColumn = jobInstructDt.JOB_DTL_IDColumn              '作業内容ID
        Dim jobInstructIdClm As DataColumn = jobInstructDt.JOB_INSTRUCT_IDColumn    '作業指示枝番
        Dim jobInstructSeqClm As DataColumn = jobInstructDt.JOB_INSTRUCT_SEQColumn  '作業指示枝番
        Dim selectjobDtlIdClm As DataColumn = jobInstructDt.SELECT_JOB_DTL_IDColumn '作業内容ID(選択)
        Dim jobStatusClm As DataColumn = jobInstructDt.JOB_STATUSColumn             '作業ステータス

        'リレーションチップ情報の事前バインディング
        Dim stallUseStatus As DataColumn = chipDt.STALL_USE_STATUSColumn

        '整備情報【親RO＋追加作業の情報】分、Loop
        For Each dr In selectDrList

            Dim selectDr As SC3240201DataSet.SC3240201JobInstructListRow = DirectCast(dr, SC3240201DataSet.SC3240201JobInstructListRow)

            'No.インデックスをインクリメント
            index += 1

            '******************************
            '* チップ詳細(小)用
            '******************************
            Dim dispRow As SC3240201DataSet.SC3240201JobInstructListRow = dispDt.NewSC3240201JobInstructListRow

            dispRow.INDEX = index                                                               'No.
            dispRow.JOB_NAME = Me.ConvertDbNullToEmpty(selectDr(jobNameClm))                    '作業名
            dispRow.OPERATION_TYPE_NAME = Me.ConvertDbNullToEmpty(selectDr(oprTypeNameClm))     'オペレーション区分名称
            dispRow.JOB_CD = Me.ConvertDbNullToEmpty(selectDr(jobCdClm))                        '作業コード
            dispRow.RO_SEQ = Me.ConvertDbNullToMinusNum(selectDr(roJobSeqClm))                  'RO連番
            dispRow.JOB_DTL_ID = Me.ConvertRezId(selectDr(jobDtlIdClm))                         '作業内容ID
            dispRow.JOB_INSTRUCT_ID = Me.ConvertDbNullToEmpty(selectDr(jobInstructIdClm))       '作業指示ID
            dispRow.JOB_INSTRUCT_SEQ = Me.ConvertDbNullToMinusNum(selectDr(jobInstructSeqClm))  '作業指示枝番
            dispRow.SELECT_JOB_DTL_ID = Me.ConvertRezId(selectDr(selectjobDtlIdClm))            '作業内容ID(選択)
            dispRow.JOB_STATUS = Me.ConvertDbNullToEmpty(selectDr(jobStatusClm))                '作業ステータス

            '【親ROの情報】は「R/O」
            If dispRow.RO_SEQ = 0 Then
                dispRow.RO_SEQCONTENTS = WebWordUtility.GetWord(MY_PROGRAMID, 54)
            Else
                '【追加作業の情報】は「追加N」　※Nは枝番
                dispRow.RO_SEQCONTENTS = String.Format(CultureInfo.InvariantCulture, _
                                                          WebWordUtility.GetWord(MY_PROGRAMID, 55), _
                                                          selectDr.RO_SEQ.ToString(CultureInfo.CurrentCulture))
            End If

            'リレーションチップ情報から該当レコードのストール利用ステータスを取得する
            selectChipDr = chipDt.Select("JOB_DTL_ID = " & Me.ConvertRezId(selectDr(selectjobDtlIdClm)))

            If selectChipDr IsNot Nothing AndAlso 0 < selectChipDr.Count Then
                mainteStallUseState = Me.ConvertDbNullToEmpty(selectChipDr(0)(stallUseStatus))
                dispRow.STALL_USE_STATUS = mainteStallUseState    'ストール利用ステータス
            Else
                dispRow.STALL_USE_STATUS = String.Empty           'ストール利用ステータス
            End If

            '返却用データセットに行を追加
            ds.SC3240201JobInstructList.AddSC3240201JobInstructListRow(dispRow)

            '******************************
            '* チップ詳細(大)用
            '******************************
            Dim dispRow2 As SC3240201DataSet.SC3240201JobInstructList2Row = dispDt2.NewSC3240201JobInstructList2Row

            dispRow2.INDEX = index
            dispRow2.SELECT_JOB_DTL_ID = Me.ConvertRezId(selectDr(selectjobDtlIdClm))   '作業内容ID(選択)
            dispRow2.JOB_STATUS = Me.ConvertDbNullToEmpty(selectDr(jobStatusClm))       '作業ステータス

            '返却用データセットに行を追加
            ds.SC3240201JobInstructList2.AddSC3240201JobInstructList2Row(dispRow2)
        Next

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' DBから取得したチップ情報を元に、チップ情報表示用データテーブルに値を設定する
    ''' </summary>
    ''' <param name="ds">返却用データセット</param>
    ''' <param name="chipDt">リレーションチップ情報</param>
    ''' <param name="showDate">工程管理画面の表示日時</param>
    ''' <remarks></remarks>
    Private Sub SetDispChipData(ByVal ds As SC3240201DataSet, _
                                ByVal chipDt As SC3240201DataSet.SC3240201RelatedChipInfoDataTable, _
                                ByVal showDate As String)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'チップ詳細(小)の段組チップエリアに使用する
        Dim smallDispDt As SC3240201DataSet.SC3240201SmallDispChipListDataTable = ds.SC3240201SmallDispChipList

        'チップ詳細(大)のチップスクロールエリア(チェックを付ける部分)に使用する
        Dim largeDispDt2 As SC3240201DataSet.SC3240201LargeDispChipList2DataTable = ds.SC3240201LargeDispChipList2

        '整備行インデックス
        Dim rowIndex As Integer = 1

        'チップのインデックス
        Dim chipIndex As Integer = 0

        'チップエリアは1整備分だけデータを作成して残りはクライアント側でコピーする
        For Each row In chipDt

            chipIndex += 1

            '******************************
            '* チップ詳細(小)用
            '******************************
            Dim smallDispRow As SC3240201DataSet.SC3240201SmallDispChipListRow = smallDispDt.NewSC3240201SmallDispChipListRow

            smallDispRow.INDEX = rowIndex                                                               '行インデックス
            smallDispRow.REZID = row.JOB_DTL_ID                                                         '予約ID
            smallDispRow.CHIPINDEX = chipIndex                                                          'チップのインデックス
            smallDispRow.STALL_USE_STATUS = row.STALL_USE_STATUS                                        'ストール利用ステータス
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'smallDispRow.ROJOBSEQ2 = row.RO_JOB_SEQ                                                     'RO作業連番
            smallDispRow.INVISIBLE_INSTRUCT_FLG = row.INVISIBLE_INSTRUCT_FLG                            '着工指示フラグ
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            'チップ情報
            smallDispRow.CHIPINFO = row.STALL_NAME_SHORT & Space(1) & _
                        Me.GetDispDateTimeString(showDate, row.PLAN_STARTDATE) & _
                        WebWordUtility.GetWord(MY_PROGRAMID, 53)

            '行を追加
            ds.SC3240201SmallDispChipList.AddSC3240201SmallDispChipListRow(smallDispRow)

            '******************************
            '* チップ詳細(大)用
            '******************************
            Dim largeDispRow2 As SC3240201DataSet.SC3240201LargeDispChipList2Row = largeDispDt2.NewSC3240201LargeDispChipList2Row

            largeDispRow2.INDEX = rowIndex                              '行インデックス
            largeDispRow2.REZID = row.JOB_DTL_ID                        '予約ID
            largeDispRow2.CHIPINDEX = chipIndex                         'チップのインデックス
            largeDispRow2.STALL_USE_STATUS = row.STALL_USE_STATUS       'ストール利用ステータス
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'largeDispRow2.ROJOBSEQ2 = row.RO_JOB_SEQ                    'RO作業連番
            largeDispRow2.INVISIBLE_INSTRUCT_FLG = row.INVISIBLE_INSTRUCT_FLG   '着工指示フラグ
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            '行を追加
            ds.SC3240201LargeDispChipList2.AddSC3240201LargeDispChipList2Row(largeDispRow2)
        Next

        'チップ詳細(小)用には未選択の行を追加する
        Dim smallDispRowUnSelected As SC3240201DataSet.SC3240201SmallDispChipListRow = smallDispDt.NewSC3240201SmallDispChipListRow

        smallDispRowUnSelected.INDEX = rowIndex
        smallDispRowUnSelected.REZID = -1                                           '未選択の場合、予約IDは-1とする
        smallDispRowUnSelected.CHIPINFO = WebWordUtility.GetWord(MY_PROGRAMID, 42)  'チップ情報は固定文言の「未選択」
        smallDispRowUnSelected.CHIPINDEX = 0                                        '未選択のチップインデックスは0とする
        smallDispRowUnSelected.STALL_USE_STATUS = String.Empty                      '未選択のストール利用ステータスはEmptyとする
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'smallDispRowUnSelected.ROJOBSEQ2 = -1                                       '未選択の場合、RO作業連番は-1とする
        smallDispRowUnSelected.INVISIBLE_INSTRUCT_FLG = "0"                         '未選択の場合、着工指示フラグは0とする
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '行を追加
        ds.SC3240201SmallDispChipList.AddSC3240201SmallDispChipListRow(smallDispRowUnSelected)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 整備情報／部品情報を、チップ情報表示用データテーブルに設定する
    ''' </summary>
    ''' <param name="ds">返却用データセット</param>
    ''' <param name="ta">テーブルアダプタ</param>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="roJobSeq">RO作業連番</param>
    ''' <remarks></remarks>
    Private Sub SetMaintePartsData(ByVal ds As SC3240201DataSet, _
                                   ByVal ta As SC3240201TableAdapter, _
                                   ByVal arg As CallBackArgumentClass, _
                                   ByVal roJobSeq As Long)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'サービス入庫IDに紐付くリレーションチップ情報(自分自身を含む)を取得
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim chipDt As SC3240201DataSet.SC3240201RelatedChipInfoDataTable = ta.GetRelatedChipInfo(arg.SvcInId, arg.DlrCD, arg.StrCD)
        Dim chipDt As SC3240201DataSet.SC3240201RelatedChipInfoDataTable = ta.GetRelatedChipInfo(arg.SvcInId, arg.DlrCD, arg.StrCD, roJobSeq)
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '●整備情報の取得
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim mainteDt As IC3801015DataSet.IC3801015ServiceInfoDataTable
        'Dim iC3801015bl As New IC3801015BusinessLogic
        'mainteDt = iC3801015bl.GetServiceInfo(arg.DlrCD, arg.RONum)

        'OutPutIFLog(mainteDt, "IC3801015BusinessLogic.GetServiceInfo")

        ''整備情報【親RO＋追加作業の情報】が取得できた場合
        'If mainteDt IsNot Nothing AndAlso 0 < mainteDt.Rows.Count Then

        '    '整備情報【親RO＋追加作業の情報】を画面表示用に編集し、返却用データセットに格納する
        '    Me.SetDispMaintenanceData(ds, mainteDt, chipDt, arg, roJobSeq)

        'End If

        '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 START

        ''整備情報の取得 (作業指示テーブルより取得)
        'Dim jobInstructDt As SC3240201DataSet.SC3240201JobInstructListDataTable = ta.GetJobInstruct(arg.RONum, arg.JobDtlId)

        '整備情報の取得 (作業指示テーブルより取得)
        Dim jobInstructDt As SC3240201DataSet.SC3240201JobInstructListDataTable _
            = ta.GetJobInstruct(arg.RONum, _
                                arg.JobDtlId, _
                                arg.DlrCD, _
                                arg.StrCD)

        '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 END

        '整備情報が取得できた場合
        If jobInstructDt IsNot Nothing AndAlso 0 < jobInstructDt.Rows.Count Then

            '整備情報を画面表示用に編集し、返却用データセットに格納する
            Me.SetDispjobInstructData(ds, jobInstructDt, chipDt, arg, roJobSeq)

        End If
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '●部品情報の取得
        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim partsDt As IC3801016DataSet.IC3801016PartsInfoDataTable
        'Dim iC3801016bl As New IC3801016BusinessLogic
        'partsDt = iC3801016bl.GetPartsInfo(arg.DlrCD, arg.RONum)

        'OutPutIFLog(partsDt, "IC3801016BusinessLogic.GetPartsInfo")

        '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 START

        ''RO番号情報(RO番号とそれに紐づくRO作業連番)を取得する
        'Dim roNumInfo As SC3240201DataSet.SC3240201RONumInfoDataTable _
        '    = ta.GetRONumInfo(arg.RONum)

        'RO番号情報(RO番号とそれに紐づくRO作業連番)を取得する
        Dim roNumInfo As SC3240201DataSet.SC3240201RONumInfoDataTable _
            = ta.GetRONumInfo(arg.RONum, _
                              arg.DlrCD, _
                              arg.StrCD)

        '2014/09/12 TMEJ 明瀬 BTS-392 TopServのROとSMBのJOBが不一致 END

        'IC3802504(部品詳細取得)の戻り値用
        Dim partsDt As IC3802504DataSet.IC3802504PartsDetailDataTable

        Using iC3802504RoNumTable As New IC3802504DataSet.IC3802504RONumInfoDataTable

            '上記で取得したRO番号情報をIC3802504RONumInfoDataTableに詰める
            For Each row As SC3240201DataSet.SC3240201RONumInfoRow In roNumInfo

                Dim iC3802504RoNumRow As IC3802504DataSet.IC3802504RONumInfoRow _
                    = iC3802504RoNumTable.NewIC3802504RONumInfoRow

                With iC3802504RoNumRow
                    .R_O = row.R_O
                    .R_O_SEQNO = row.R_O_SEQNO.ToString(CultureInfo.CurrentCulture)
                End With

                iC3802504RoNumTable.AddIC3802504RONumInfoRow(iC3802504RoNumRow)

            Next

            Using ic3802504bl As New IC3802504BusinessLogic

                If Not IsNothing(iC3802504RoNumTable) AndAlso 0 < iC3802504RoNumTable.Count Then
                    'RO番号情報が存在する場合

                    '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 START
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                 "{0}.{1} ⑦SC3240201_チップ詳細表示[部品詳細情報取得] START", _
                                 Me.GetType.ToString, _
                                 System.Reflection.MethodBase.GetCurrentMethod.Name))
                    '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 END

                    'IC3802504を使用して部品情報の取得
                    partsDt = ic3802504bl.GetPartsDetailList(arg.DlrCD, _
                                                             arg.StrCD, _
                                                             iC3802504RoNumTable)

                    '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 START
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                 "{0}.{1} ⑦SC3240201_チップ詳細表示[部品詳細情報取得] END", _
                                 Me.GetType.ToString, _
                                 System.Reflection.MethodBase.GetCurrentMethod.Name))
                    '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 END

                    If Not IsNothing(partsDt) AndAlso 0 < partsDt.Rows.Count Then
                        '部品情報が取得できた or 部品情報の取得でエラーが発生した場合

                        '部品詳細情報の取得結果を取得
                        Dim getPartsDetailResult As Long = partsDt.Item(0).ResultCode

                        '結果コードをデータセット内のテーブルに設定
                        Me.SetLinkResult(ds, getPartsDetailResult)

                        '成功でない場合、部品テーブルはNothingにする
                        If IC3802504BusinessLogic.Result.Success <> getPartsDetailResult Then
                            partsDt = Nothing
                        End If

                    Else
                        '部品情報がなかった場合

                        '結果コードを0(成功)でデータセット内のテーブルに設定
                        Me.SetLinkResult(ds, 0)
                    End If

                Else
                    'RO番号情報が存在しない場合
                    partsDt = Nothing

                    '結果コードを0(成功)でデータセット内のテーブルに設定
                    Me.SetLinkResult(ds, 0)
                End If

            End Using

        End Using
        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        '部品情報【親RO＋追加作業の情報】が取得できた場合
        If partsDt IsNot Nothing AndAlso 0 < partsDt.Rows.Count Then

            '部品情報【親RO＋追加作業の情報】を画面表示用に編集し、返却用データセットに格納する
            Me.SetDispPartsData(ds, partsDt, arg, roJobSeq)

        End If

        '追加作業エリア以外は、チップ情報を取得する
        If arg.SubAreaId <> SUBAREA_ADDWORK Then

            'リレーションチップ情報が1件以上存在する場合
            If 0 < chipDt.Rows.Count Then

                'リレーションチップ情報を画面表示用に編集し、返却用データセットに格納する
                Me.SetDispChipData(ds, chipDt, arg.ShowDate)

            End If
        End If

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 整備種類／整備名称情報を、チップ情報表示用データテーブルに設定する
    ''' </summary>
    ''' <param name="ds">返却用データセット</param>
    ''' <param name="ta">テーブルアダプタ</param>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="strCD">店舗コード</param>
    ''' <param name="svcClassId">サービス分類ID</param>
    ''' <remarks></remarks>
    Private Sub SetSvcMercData(ByVal ds As SC3240201DataSet, _
                               ByVal ta As SC3240201TableAdapter, _
                               ByVal dlrCD As String, _
                               ByVal strCD As String, _
                               ByVal svcClassId As Decimal)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '整備種類
        Dim svcClassDt As SC3240201DataSet.SC3240201SvcClassListDataTable = ta.GetSvcClassList(dlrCD, strCD)

        '表示用データテーブル・データ行
        Dim dispSvcClassDt As SC3240201DataSet.SC3240201SvcClassListDataTable = ds.SC3240201SvcClassList

        '整備種類情報の事前バインディング
        Dim svcIdTime As DataColumn = svcClassDt.SVCID_TIMEColumn
        Dim svcClassName As DataColumn = svcClassDt.SVC_CLASS_NAMEColumn
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        Dim svcClassType As DataColumn = svcClassDt.SVC_CLASS_TYPEColumn
        Dim carWashNeedFlg As DataColumn = svcClassDt.CARWASH_NEED_FLGColumn
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        For i = 0 To svcClassDt.Rows.Count - 1

            Dim selectSvcClassDr As SC3240201DataSet.SC3240201SvcClassListRow = DirectCast(svcClassDt.Rows(i), SC3240201DataSet.SC3240201SvcClassListRow)

            Dim dispSvcClassRow As SC3240201DataSet.SC3240201SvcClassListRow = dispSvcClassDt.NewSC3240201SvcClassListRow

            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'dispSvcClassRow.SVCID_TIME = CType(selectSvcClassDr(svcIdTime), String)                   'サービス分類ID,標準作業時間
            'サービス分類ID,標準作業時間 + サービス分類区分 + 洗車必要フラグ
            dispSvcClassRow.SVCID_TIME = String.Format(CultureInfo.CurrentCulture _
                                                       , "{0},{1},{2}" _
                                                       , CType(selectSvcClassDr(svcIdTime), String) _
                                                       , Me.ConvertDbNullToEmpty(selectSvcClassDr(svcClassType)) _
                                                       , Me.ConvertDbNullToEmpty(selectSvcClassDr(carWashNeedFlg)))
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            dispSvcClassRow.SVC_CLASS_NAME = Me.ConvertDbNullToEmpty(selectSvcClassDr(svcClassName))  'サービス分類名称

            '行を追加
            ds.SC3240201SvcClassList.AddSC3240201SvcClassListRow(dispSvcClassRow)
        Next


        '整備名
        Dim mercDt As SC3240201DataSet.SC3240201MercListDataTable = ta.GetMercList(dlrCD, strCD, svcClassId)

        '整備名情報の事前バインディング
        Dim mercIdTime As DataColumn = mercDt.MERCID_TIMEColumn
        Dim mercName As DataColumn = mercDt.MERC_NAMEColumn

        '表示用データテーブル・データ行
        Dim dispMercDt As SC3240201DataSet.SC3240201MercListDataTable = ds.SC3240201MercList

        For i = 0 To mercDt.Rows.Count - 1

            Dim selectMercDr As SC3240201DataSet.SC3240201MercListRow = DirectCast(mercDt.Rows(i), SC3240201DataSet.SC3240201MercListRow)

            Dim dispMercRow As SC3240201DataSet.SC3240201MercListRow = dispMercDt.NewSC3240201MercListRow

            dispMercRow.MERCID_TIME = CType(selectMercDr(mercIdTime), String)        '商品ID,標準作業時間
            dispMercRow.MERC_NAME = Me.ConvertDbNullToEmpty(selectMercDr(mercName))  '商品名称

            '行を追加
            ds.SC3240201MercList.AddSC3240201MercListRow(dispMercRow)
        Next

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 納車時刻変更履歴情報を、チップ情報表示用データテーブルに設定する
    ''' </summary>
    ''' <param name="ds">返却用データセット</param>
    ''' <param name="smbCommonChipDetails">共通クラスチップ情報</param>
    ''' <param name="showDate">工程管理画面の表示日時</param>
    ''' <remarks></remarks>
    Private Sub SetDeliChangeData(ByVal ds As SC3240201DataSet, _
                                  ByVal smbCommonChipDetails As CommonUtility.SMBCommonClass.Api.BizLogic.ChipDetail, _
                                  ByVal showDate As String)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '表示用データテーブル・データ行
        Dim dtDeliChange As SC3240201DataSet.SC3240201DeliveryTimeChangeLogInfoDataTable = ds.SC3240201DeliveryTimeChangeLogInfo

        '工程管理画面上で表示している日付
        Dim nowDate As Date = Date.Parse(showDate, CultureInfo.InvariantCulture)

        For Each item As DeliveryChg In smbCommonChipDetails.DeliveryChgList

            '表示用データ行
            Dim rowDeliChange As SC3240201DataSet.SC3240201DeliveryTimeChangeLogInfoRow = dtDeliChange.NewSC3240201DeliveryTimeChangeLogInfoRow()

            '変更前納車予定時刻
            rowDeliChange.CHANGEFROMTIME = Me.SetDateTimeToStringDetail(item.OldDeliveryHopeDate, nowDate)
            '変更後納車予定時刻
            rowDeliChange.CHANGETOTIME = Me.SetDateTimeToStringDetail(item.NewDeliveryHopeDate, nowDate)
            '変更日時
            rowDeliChange.UPDATETIME = Me.SetDateTimeToStringDetail(item.ChangeDate, nowDate)
            '変更理由
            rowDeliChange.UPDATEPRETEXT = item.ChangeReason
            '変更前納車予定時刻と変更後納車予定時刻の間の右矢印
            rowDeliChange.RIGHTARROWLABEL = WebWordUtility.GetWord(MY_PROGRAMID, 8)

            ds.SC3240201DeliveryTimeChangeLogInfo.AddSC3240201DeliveryTimeChangeLogInfoRow(rowDeliChange)
        Next

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 中断理由情報を、チップ情報表示用データテーブルに設定する
    ''' </summary>
    ''' <param name="ds">返却用データセット</param>
    ''' <param name="smbCommonChipDetails">共通クラスチップ情報</param>
    ''' <remarks></remarks>
    Private Sub SetInterruptionData(ByVal ds As SC3240201DataSet, _
                                    ByVal smbCommonChipDetails As CommonUtility.SMBCommonClass.Api.BizLogic.ChipDetail)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '表示用データテーブル・データ行
        Dim dtInterruption As SC3240201DataSet.SC3240201InterruptionInfoDataTable = ds.SC3240201InterruptionInfo

        For Each item As StopReason In smbCommonChipDetails.StopReasonList

            '表示用データ行
            Dim rowInterruption As SC3240201DataSet.SC3240201InterruptionInfoRow = dtInterruption.NewSC3240201InterruptionInfoRow()

            '中断理由
            rowInterruption.INTERRUPTIONCAUSE = item.ResultStatus
            '中断注釈
            rowInterruption.INTERRUPTIONDETAILS = item.StopMemo

            ds.SC3240201InterruptionInfo.AddSC3240201InterruptionInfoRow(rowInterruption)
        Next

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' APIから取得した親RO＋追加作業の部品情報を元に、部品情報表示用データテーブルに値を設定する
    ' ''' </summary>
    ' ''' <param name="ds">返却用データセット</param>
    ' ''' <param name="partsDt">部品情報【親RO＋追加作業の情報】</param>
    ' ''' <param name="arg">引数クラスオブジェクト</param>
    ' ''' <param name="roJobSeq">RO作業連番</param>
    ' ''' <remarks></remarks>
    'Private Sub SetDispPartsData(ByVal ds As SC3240201DataSet, _
    '                             ByVal partsDt As IC3801016DataSet.IC3801016PartsInfoDataTable, _
    '                             ByVal arg As CallBackArgumentClass, _
    '                             ByVal roJobSeq As Long)
    ''' <summary>
    ''' APIから取得した親RO＋追加作業の部品情報を元に、部品情報表示用データテーブルに値を設定する
    ''' </summary>
    ''' <param name="ds">返却用データセット</param>
    ''' <param name="partsDt">部品情報【親RO＋追加作業の情報】</param>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="roJobSeq">RO作業連番</param>
    ''' <remarks></remarks>
    Private Sub SetDispPartsData(ByVal ds As SC3240201DataSet, _
                                 ByVal partsDt As IC3802504DataSet.IC3802504PartsDetailDataTable, _
                                 ByVal arg As CallBackArgumentClass, _
                                 ByVal roJobSeq As Long)
        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim dispDt As SC3240201DataSet.SC3240201DispPartsListDataTable = ds.SC3240201DispPartsList

        Dim index As Integer = 0

        'エリアにより、取得データを絞り込む
        Dim selectDrList As DataRow()
        If SUBAREA_RECEPTION.Equals(arg.SubAreaId) Then
            '受付エリアの場合

            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
            ''顧客承認後、且つ、自チップの作業連番で絞り込む
            'selectDrList = partsDt.Select("customerConfirmFlag = '1' AND workSeq = " & roJobSeq)
            '自チップの作業連番で絞り込む
            selectDrList = partsDt.Select("R_O_SEQNO = " & roJobSeq)
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        ElseIf SUBAREA_ADDWORK.Equals(arg.SubAreaId) Then
            '追加作業エリアの場合

            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
            ''顧客承認前、且つ、自チップの作業連番で絞り込む
            'selectDrList = partsDt.Select("customerConfirmFlag = '0' AND workSeq = " & roJobSeq)
            '自チップの作業連番で絞り込む
            selectDrList = partsDt.Select("R_O_SEQNO = " & roJobSeq)
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        Else
            '上記以外の場合

            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
            ''顧客承認後で絞り込む
            'selectDrList = partsDt.Select("customerConfirmFlag = '1'")
            '※無意味な絞込みだが、以下の処理でselectDrListを使用するために
            '　この絞込みを行う
            selectDrList = partsDt.Select("R_O = '" & arg.RONum & "'")
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END
        End If
        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        '部品情報の事前バインディング
        Dim partsName As DataColumn = partsDt.PartsNameColumn
        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim srvTypeName As DataColumn = partsDt.srvTypeNameColumn
        'Dim quantity As DataColumn = partsDt.quantityColumn
        'Dim unit As DataColumn = partsDt.unitColumn
        'Dim boFlg As DataColumn = partsDt.boFlgColumn
        'Dim partsRepareFlg As DataColumn = partsDt.partsRepareFlgColumn
        'Dim workSeq As DataColumn = partsDt.workSeqColumn
        'Dim addSeq As DataColumn = partsDt.addSeqColumn
        Dim partsType As DataColumn = partsDt.PartsTypeColumn
        Dim partsUnit As DataColumn = partsDt.PartsUnitColumn
        Dim partsAmount As DataColumn = partsDt.PartsAmountColumn
        Dim boDateTime As DataColumn = partsDt.BO_Scheduled_DateTimeColumn
        Dim partsStatus As DataColumn = partsDt.PartsStatusColumn
        Dim roSeqNo As DataColumn = partsDt.R_O_SEQNOColumn
        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        '部品情報【親RO＋追加作業の情報】分、Loop
        For Each dr In selectDrList

            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim selectDr As IC3801016DataSet.IC3801016PartsInfoRow = DirectCast(dr, IC3801016DataSet.IC3801016PartsInfoRow)
            Dim selectDr As IC3802504DataSet.IC3802504PartsDetailRow = DirectCast(dr, IC3802504DataSet.IC3802504PartsDetailRow)
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

            '画面表示用インデックスをインクリメント
            index += 1

            Dim dispRow As SC3240201DataSet.SC3240201DispPartsListRow = dispDt.NewSC3240201DispPartsListRow

            dispRow.INDEX = index                                               'No.
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
            'dispRow.PARTS = Me.ConvertDbNullToEmpty(selectDr(partsName))        '品名
            'dispRow.PARTSDIV = Me.ConvertDbNullToEmpty(selectDr(srvTypeName))   '整備区分名称
            'dispRow.AMOUNT = Me.ConvertDbNullToEmpty(selectDr(quantity))        '数量
            'dispRow.PARTSUNIT = Me.ConvertDbNullToEmpty(selectDr(unit))         '単位

            ''B/Oフラグが1なら「B/O」
            'Dim boFlag As String
            'boFlag = Me.ConvertDbNullToEmpty(selectDr(boFlg))
            'If "1".Equals(boFlag) Then
            '    dispRow.BOFLG = WebWordUtility.GetWord(MY_PROGRAMID, 62)
            'Else
            '    dispRow.BOFLG = String.Empty
            'End If

            ''出庫ステータス
            'Dim partsStatus As String
            'partsStatus = Me.ConvertDbNullToEmpty(selectDr(partsRepareFlg))
            'If "1".Equals(partsStatus) Then
            '    dispRow.PARTSPREPARE = WebWordUtility.GetWord(MY_PROGRAMID, 63)
            'Else
            '    dispRow.PARTSPREPARE = String.Empty
            'End If

            'dispRow.ROJOBSEQ = Me.ConvertDbNullToMinusNum(selectDr(workSeq))           '作業連番
            'dispRow.SRVADDSEQ = Me.ConvertDbNullToMinusNum(selectDr(addSeq))           '枝番

            dispRow.PARTS = selectDr(partsName).ToString()        '品名
            dispRow.PARTSDIV = selectDr(partsType).ToString()     '整備区分名称
            dispRow.AMOUNT = selectDr(partsAmount).ToString()     '数量
            dispRow.PARTSUNIT = selectDr(partsUnit).ToString()    '単位

            'B/Oスケジュール日時が入っていれば「B/O」
            If Not String.IsNullOrEmpty(selectDr(boDateTime).ToString()) Then
                dispRow.BOFLG = WebWordUtility.GetWord(MY_PROGRAMID, 62)
            Else
                dispRow.BOFLG = String.Empty
            End If

            '出庫ステータスが8なら「済み」※部品準備済みということ
            If "8".Equals(selectDr(partsStatus).ToString()) Then
                dispRow.PARTSPREPARE = WebWordUtility.GetWord(MY_PROGRAMID, 63)
            Else
                dispRow.PARTSPREPARE = String.Empty
            End If

            dispRow.SRVADDSEQ = CType(selectDr(roSeqNo), Long)          'RO作業連番
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

            '【親ROの情報】は「R/O」
            If dispRow.SRVADDSEQ = 0 Then
                dispRow.SRVADDSEQCONTENTS = WebWordUtility.GetWord(MY_PROGRAMID, 54)
            Else
                '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
                ''【追加作業の情報】は「追加N」　※Nは枝番
                'dispRow.SRVADDSEQCONTENTS = String.Format(CultureInfo.InvariantCulture, _
                '                                          WebWordUtility.GetWord(MY_PROGRAMID, 55), _
                '                                          selectDr.addSeq.ToString(CultureInfo.CurrentCulture))
                '【追加作業の情報】は「追加N」　※Nは枝番
                dispRow.SRVADDSEQCONTENTS = String.Format(CultureInfo.InvariantCulture, _
                                                          WebWordUtility.GetWord(MY_PROGRAMID, 55), _
                                                          selectDr.R_O_SEQNO.ToString(CultureInfo.CurrentCulture))
                '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END
            End If

            '行を追加
            ds.SC3240201DispPartsList.AddSC3240201DispPartsListRow(dispRow)
        Next

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 着工指示(整備と予約の紐付け)データの送信
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="ta">テーブルアダプタ</param>
    ''' <param name="updDate">更新日時</param>
    ''' <remarks></remarks>
    Private Function SendInstruct(ByVal arg As CallBackArgumentClass, _
                                  ByVal ta As SC3240201TableAdapter, _
                                  ByVal updDate As Date) As Long

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim returnCode As Long = 0
        Dim fixItemCodeList As List(Of String) = arg.FixItemCodeList          '整備コードリスト
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim fixItemSeqList As List(Of String) = arg.FixItemSeqList            '整備連番リスト
        'Dim srvAddSeqList As List(Of String) = arg.SrvAddSeqList              '枝番リスト
        Dim jobinstrucDtlIdList As List(Of String) = arg.JobinstrucDtlIdList  '整備に紐付いた作業内容IDのリスト　 ※チップ詳細画面を開いた直後の状態
        Dim jobInstructIdList As List(Of String) = arg.JobInstructIdList      '整備に紐付いた作業指示IDのリスト　 ※チップ詳細画面を開いた直後の状態
        Dim jobInstructSeqList As List(Of String) = arg.JobInstructSeqList    '整備に紐付いた作業指示枝番のリスト ※チップ詳細画面を開いた直後の状態
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        Dim matchingRezIdList As List(Of String) = arg.MatchingRezIdList               '整備に紐付いた予約IDのリスト(全て-1の場合は全て紐付かない)　※更新時の状態
        Dim before_MatchingRezIdList As List(Of String) = arg.BeforeMatchingRezIdList  '整備に紐付いた予約IDのリスト(全て-1の場合は全て紐付かない)　※チップ詳細画面を開いた直後の状態

        '整備コードリストが存在する場合
        If Not IsNothing(fixItemCodeList) AndAlso 0 < fixItemCodeList.Count Then

            Dim matchingRezId As Decimal
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim nextSrvAddSeq As String                  '次のレコードの枝番
            'Dim rtnVal As Long = -1
            'Dim remainFlg As Boolean = False             '着工指示送信残フラグ
            Dim structFlg As String                      '着工支持フラグ
            Dim instrucDtlId As Decimal                  '作業内容ID
            Dim InstructId As String                     '作業指示ID
            Dim InstructSeq As Long                      '作業指示枝番
            Dim before_matchingRezId As Decimal
            Dim resultCD As Long = 0
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim sendFlg As Boolean = False               '着工指示送信フラグ
            'Dim changedJobDtlIdList As New List(Of Decimal) '着工指示の変更がある作業内容IDリスト(TC Push送信用)

            ''枝番を初期化
            'nextSrvAddSeq = "-1"

            'Using dt As New IC3800902DataSet.IC3800902ServiceInfoDataTable
            '    Dim dr As IC3800902DataSet.IC3800902ServiceInfoRow

            '    '整備コードリストの件数分Loop
            '    For i = 0 To fixItemCodeList.Count - 1

            '        '次のレコードが存在する場合
            '        If i < fixItemCodeList.Count - 1 Then
            '            '次のレコードの枝番を取得
            '            nextSrvAddSeq = srvAddSeqList(i + 1)
            '        End If

            '        '変更のあったものだけ着工指示データを送る
            '        If Not before_MatchingRezIdList(i).Equals(matchingRezIdList(i)) Then

            '            dr = DirectCast(dt.NewRow(), IC3800902DataSet.IC3800902ServiceInfoRow)

            '            dr.srvCode = fixItemCodeList(i)                  '整備コード
            '            dr.srvSeq = CType(fixItemSeqList(i), Integer)    '整備連番

            '            '予約ID
            '            matchingRezId = CType(matchingRezIdList(i), Decimal)
            '            If matchingRezId <= 0 Then
            '                'dr.REZID = Nothing
            '                dr.SetREZIDNull()

            '                '着工指示の変更がある場合、作業内容IDをリストに追加
            '                If Not changedJobDtlIdList.Contains(CType(before_MatchingRezIdList(i), Decimal)) Then
            '                    changedJobDtlIdList.Add(CType(before_MatchingRezIdList(i), Decimal))
            '                End If
            '            Else
            '                dr.REZID = CType(matchingRezId, Integer)

            '                '着工指示の変更がある場合、作業内容IDをリストに追加
            '                If Not changedJobDtlIdList.Contains(matchingRezId) Then
            '                    changedJobDtlIdList.Add(matchingRezId)
            '                End If
            '            End If

            '            dt.Rows.Add(dr)

            '            remainFlg = True  '着工指示送信残あり
            '        End If

            '        '『現在レコードの枝番≠次レコードの枝番、且つ、着工指示送信残がある場合』、もしくは、
            '        '『最終レコード、且つ、着工指示送信残がある場合』、送信
            '        If ((Not (srvAddSeqList(i).Equals(nextSrvAddSeq))) AndAlso (remainFlg = True)) OrElse
            '             ((i = fixItemCodeList.Count - 1) AndAlso (remainFlg = True)) Then

            '            '着工指示（「RO番号＋枝番」毎にデータを送る）
            '            Dim iC3800902bl As New IC3800902BusinessLogic
            '            rtnVal = iC3800902bl.UpdateInstruct(arg.DlrCD, arg.RONum, CType(srvAddSeqList(i), Integer), dt)

            '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                        , "{0}.{1} [RONum:{2}][srvAddSeqList:{3}]" _
            '                        , Me.GetType.ToString _
            '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '                        , arg.RONum, srvAddSeqList(i)))
            '            OutPutIFLog(dt, "IC3800902BusinessLogic.UpdateInstruct")

            '            '更新失敗の場合
            '            If rtnVal = 1 Then
            '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '                            , "{0}.{1} biz.UpdateInstruct RONum={2} SrvAddSeq={3}" _
            '                            , Me.GetType.ToString _
            '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '                            , arg.RONum, srvAddSeqList(i)))
            '                Return ActionResult.DmsLinkageError
            '            Else
            '                sendFlg = True     '着工指示送信済み
            '            End If

            '            dt.Clear()

            '            remainFlg = False  '着工指示送信残なし
            '        End If
            '    Next
            'End Using

            ''TCにPUSH送信する(着工指示／着工指示キャンセル)
            'If sendFlg = True AndAlso _
            '    Not IsNothing(changedJobDtlIdList) AndAlso 0 < changedJobDtlIdList.Count Then

            '    'TCのオペレーションコードをセット
            '    Dim operationCodeList As New List(Of Decimal)
            '    operationCodeList.Add(Operation.TEC)

            '    '着工指示の変更がある作業内容IDリストを元に、該当チップのストールIDリストを取得
            '    Dim stallDt As SC3240201DataSet.SC3240201PushStallIdDataTable = _
            '        ta.GetStallIdListByJobDtlId(arg.DlrCD, arg.StrCD, arg.SvcInId, changedJobDtlIdList)

            '    'PUSH送信用のストールIDリストを作成
            '    Dim stallIdList As New List(Of Decimal)
            '    For i = 0 To stallDt.Rows.Count - 1

            '        Dim selectStallDr As SC3240201DataSet.SC3240201PushStallIdRow = DirectCast(stallDt.Rows(i), SC3240201DataSet.SC3240201PushStallIdRow)

            '        'ストールIDリストに追加
            '        stallIdList.Add(CType(selectStallDr.Item(stallDt.STALL_IDColumn), Decimal))
            '    Next

            '    'TCにPUSH送信する
            '    clsTabletSMBCommonClass.SendPushGetReady(arg.DlrCD, arg.StrCD, operationCodeList, PUSH_FUNTIONNAME, stallIdList)

            '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                , "{0}.{1} SendPush." _
            '                , Me.GetType.ToString _
            '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
            'End If

            '整備コードリストの件数分Loop
            For i = 0 To fixItemCodeList.Count - 1

                '変更のあったものだけ着工指示データを更新
                If Not before_MatchingRezIdList(i).Equals(matchingRezIdList(i)) Then

                    matchingRezId = CType(matchingRezIdList(i), Decimal)                '予約ID
                    before_matchingRezId = CType(before_MatchingRezIdList(i), Decimal)  '予約ID(前回)
                    instrucDtlId = CType(jobinstrucDtlIdList(i), Decimal)               '作業内容ID
                    InstructId = jobInstructIdList(i)                                     '作業指示ID
                    InstructSeq = CType(jobInstructSeqList(i), Long)                     '作業指示枝番

                    If matchingRezId < 0 Then
                        '着工支持選択状態 → 未選択に変更時

                        '作業指示テーブルを更新する (Update)
                        structFlg = "0"
                        resultCD = ta.UpdJobInstruct(updDate, arg.Account, structFlg, before_matchingRezId, InstructId, InstructSeq)
                        If resultCD <= 0 Then
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                       , "{0}.{1} SC3240201TableAdapter.UpdJobInstruct updDate={2} arg.Account={3} structFlg={4} before_matchingRezId={5} InstructId={6} InstructSeq={7}" _
                                       , Me.GetType.ToString _
                                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                       , updDate, arg.Account, structFlg, before_matchingRezId, InstructId, InstructSeq))
                            Return -1
                        End If
                    Else
                        If ((before_matchingRezId < 0) AndAlso (matchingRezId.Equals(instrucDtlId))) Then
                            '着工支持(未)選択状態 → 選択に変更、かつ、選択された予約が着工支持テーブルの作業内容IDと同じ場合

                            '作業指示テーブルを更新する (Update)
                            structFlg = "1"
                            resultCD = ta.UpdJobInstruct(updDate, arg.Account, structFlg, matchingRezId, InstructId, InstructSeq)
                            If resultCD <= 0 Then
                                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                           , "{0}.{1} SC3240201TableAdapter.UpdJobInstruct updDate={2} arg.Account={3} structFlg={4} matchingRezId={5} InstructId={6} InstructSeq={7}" _
                                           , Me.GetType.ToString _
                                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                           , updDate, arg.Account, structFlg, matchingRezId, InstructId, InstructSeq))
                                Return -1
                            End If
                        Else
                            '着工支持選択状態 → 違う予約に選択を変更した場合、または、
                            '着工支持(未)選択状態 → 選択に変更、かつ、選択された予約が着工支持テーブルの作業内容IDと違う場合

                            '作業指示テーブルを更新する (DeleteInsert)
                            '更新する作業指示テーブルを退避
                            Dim dtJobInstruct As SC3240201DataSet.SC3240201JobInstructBakupListDataTable = ta.GetJobInstructBackup(instrucDtlId, InstructId, InstructSeq)
                            If dtJobInstruct.Count <= 0 Then
                                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                           , "{0}.{1} SC3240201TableAdapter.GetJobInstructBackup instrucDtlId={2} InstructId={3} InstructSeq={4}" _
                                           , Me.GetType.ToString _
                                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                           , instrucDtlId, InstructId, InstructSeq))
                                Return -1
                            End If
                            '更新する作業指示テーブルを削除
                            resultCD = ta.DeleteJobInstruct(instrucDtlId, InstructId, InstructSeq)
                            If resultCD <= 0 Then
                                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                           , "{0}.{1} SC3240201TableAdapter.DeleteJobInstruct instrucDtlId={2} InstructId={3} InstructSeq={4}" _
                                           , Me.GetType.ToString _
                                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                           , instrucDtlId, InstructId, InstructSeq))
                                Return -1
                            End If
                            '更新する作業指示テーブルを挿入する
                            structFlg = "1"
                            Dim dtJobInstructRow As SC3240201DataSet.SC3240201JobInstructBakupListRow = DirectCast(dtJobInstruct.Rows(0), SC3240201DataSet.SC3240201JobInstructBakupListRow)
                            resultCD = ta.InsertJobInstruct(dtJobInstructRow, updDate, arg.Account, structFlg, matchingRezId)
                            If resultCD <> 1 Then
                                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                           , "{0}.{1} SC3240201TableAdapter.InsertJobInstruct updDate={2} arg.Account={3} structFlg={4} matchingRezId={5}" _
                                           , Me.GetType.ToString _
                                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                           , updDate, arg.Account, structFlg, matchingRezId))
                                Return -1
                            End If
                        End If
                    End If
                End If
            Next
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        End If

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        Return returnCode

    End Function

    ''' <summary>
    ''' 通知処理の共通項目に設定
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="dr">通知用CommonClass情報</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <remarks></remarks>
    Private Function SetCommonContents(ByVal arg As CallBackArgumentClass, _
                                  ByVal dr As TabletSmbCommonClassNoticeInfoRow, _
                                  ByVal objStaffContext As StaffContext) As Long

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      " [DlrCD:{0}][StrCD:{1}][PositionType:{2}][NameTitleName:{3}][CstName:{4}][VisitSeq:{5}][RONum:{6}][VisitVin:{7}][DmsCstCd:{8}][CstType:{9}]", _
                      arg.DlrCD, arg.StrCD, arg.PositionType, arg.NameTitleName, arg.CstName, arg.VisitSeq, arg.RONum, arg.VisitVin, arg.DmsCstCD, arg.CstType)

        Dim returnCode As Long = 0
        Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Nothing

        '基幹販売店コード、店舗コードを取得
        dmsDlrBrnRow = Me.GetDmsBlnCD(arg.DlrCD, arg.StrCD, objStaffContext.Account)
        If IsNothing(dmsDlrBrnRow) _
            OrElse dmsDlrBrnRow.IsCODE1Null _
            OrElse dmsDlrBrnRow.IsCODE2Null _
            OrElse dmsDlrBrnRow.IsACCOUNTNull Then
            returnCode = -1
            Return returnCode
        End If

        '顧客名に敬称を設定する
        Dim cstNameWithTitle As String
        If arg.PositionType.Equals(POSITION_TYPE_FORWORD) Then
            '名称の前
            cstNameWithTitle = arg.NameTitleName & arg.CstName
        Else
            '名称の後
            cstNameWithTitle = arg.CstName & arg.NameTitleName
        End If

        dr.DearlerCode = dmsDlrBrnRow.CODE1
        dr.BranchCode = dmsDlrBrnRow.CODE2
        dr.LoginUserID = dmsDlrBrnRow.ACCOUNT
        If Not arg.VisitSeq.Trim().Equals("-1") Then
            dr.SAChipID = arg.VisitSeq
        Else
            returnCode = -1
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} VisitSeq = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , arg.VisitSeq))
            Return returnCode
        End If
        dr.BASREZID = ""
        dr.R_O = arg.RONum
        dr.SEQ_NO = "0"
        dr.VIN = arg.VisitVin
        dr.DMS_CST_ID = arg.DmsCstCD
        dr.VCLREGNO = arg.RegNo
        dr.CST_NAME = cstNameWithTitle
        dr.Message = ""
        If String.IsNullOrEmpty(arg.DmsCstCD) Then
            dr.CUSTSEGMENT = "2"        '未取引客
        Else
            dr.CUSTSEGMENT = "1"        '自社客
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} [DearlerCode:{2}][BranchCode:{3}][LoginUserID:{4}][SAChipID:{5}][R_O:{6}][SEQ_NO:{7}][VIN:{8}][DMS_CST_ID:{9}][VCLREGNO:{10}][CST_NAME:{11}][CUSTSEGMENT:{12}]" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , dr.DearlerCode, dr.BranchCode, dr.LoginUserID, dr.SAChipID, dr.R_O, dr.SEQ_NO, dr.VIN, dr.DMS_CST_ID, dr.VCLREGNO, dr.CST_NAME, dr.CUSTSEGMENT))

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        Return returnCode

    End Function

    ''' <summary>
    ''' 通知処理用のDispContentsの設定
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="instruct_Status">着工指示状態</param>
    ''' <param name="cstName">顧客名称</param>
    ''' <param name="startPlanTime">更新用の作業開始予定日時</param>
    ''' <param name="finishPlanTime">更新用の作業完了予定日時</param>
    ''' <param name="svcClassName">サービス分類名称</param>
    ''' <param name="mercName">商品名称</param>
    ''' <param name="upperDisp">商品マーク上部表示文字列</param>
    ''' <param name="lowerDisp">商品マーク下部表示文字列</param>
    ''' <param name="updDate">更新日時</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/01 TMEJ 小澤 BTS-261対応 サービス名の表示制御の修正
    ''' </history>
    Private Function SetDispContents(ByVal arg As CallBackArgumentClass, _
                                  ByVal instruct_Status As String, _
                                  ByVal cstName As String, _
                                  ByVal startPlanTime As Date, _
                                  ByVal finishPlanTime As Date, _
                                  ByVal svcClassName As String, _
                                  ByVal mercName As String, _
                                  ByVal upperDisp As String, _
                                  ByVal lowerDisp As String, _
                                  ByVal updDate As Date) As String

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      " [instruct_Status:{0}][cstName:{1}][startPlanTime:{2}][finishPlanTime:{3}][svcClassName:{4}][mercName:{5}][upperDisp:{6}][lowerDisp:{7}][updDate:{8}]", _
                      instruct_Status, cstName, startPlanTime, finishPlanTime, svcClassName, mercName, upperDisp, lowerDisp, updDate)

        Dim svcName As String = String.Empty

        If Not String.IsNullOrEmpty(upperDisp) OrElse Not String.IsNullOrEmpty(lowerDisp) Then
            '2015/04/01 TMEJ 小澤 BTS-261対応 サービス名の表示制御の修正 START
            'If Not String.IsNullOrEmpty(upperDisp) AndAlso Not String.IsNullOrEmpty(lowerDisp) Then
            '2015/04/01 TMEJ 小澤 BTS-261対応 サービス名の表示制御の修正 END

            '商品マーク上部表示文字列と商品マーク下部表示文字列が存在する場合
            svcName = upperDisp & lowerDisp

        ElseIf Not String.IsNullOrEmpty(mercName) Then
            '商品名称
            svcName = mercName
        Else
            'サービス分類名称
            svcName = svcClassName
        End If

        Dim wordDispContents As String = ""
        If instruct_Status.Equals(INSTRUCT_1) Then
            '着工指示開始
            '[作用指示 R/O番号 車両登録番号 顧客名+敬称 作業開始予定日時+"～"+作業終了予定日時 整備内容] (例.作業指示 R/O No. 広A-12B34 渡辺 友介様 10:00～11:00 20K)
            wordDispContents = String.Format(CultureInfo.CurrentCulture _
                             , "{0} {1} {2} {3} {4} {5}" _
                             , WebWordUtility.GetWord(MY_PROGRAMID, 73) _
                             , arg.RONum _
                             , arg.RegNo _
                             , cstName _
                             , SetDateTimeToStringDetail2(startPlanTime, updDate) & WebWordUtility.GetWord(MY_PROGRAMID, 76) & SetDateTimeToStringDetail2(finishPlanTime, updDate) _
                             , svcName)
        Else
            '着工指示キャンセル
            '[作業計画変更 R/O番号 車両登録番号 顧客名+敬称 作業開始予定日時+"～"+作業終了予定日時 整備内容] (例.作業計画変更 R/O No. 広A-12B34 渡辺 友介様 10:00～11:00 20K)
            wordDispContents = String.Format(CultureInfo.CurrentCulture _
                             , "{0} {1} {2} {3} {4} {5}" _
                             , WebWordUtility.GetWord(MY_PROGRAMID, 74) _
                             , arg.RONum _
                             , arg.RegNo _
                             , cstName _
                             , SetDateTimeToStringDetail2(startPlanTime, updDate) & WebWordUtility.GetWord(MY_PROGRAMID, 76) & SetDateTimeToStringDetail2(finishPlanTime, updDate) _
                             , svcName)
        End If

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[message:{0}]", wordDispContents)

        Return wordDispContents

    End Function

    ''' <summary>
    ''' 共通クラスの入力項目の設定
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="dr">共通クラスの入力項目情報</param>
    ''' <param name="chipBaseDt">チップ情報</param>
    ''' <param name="stallUseStatus">チップ情報</param>
    ''' <param name="stallUseId">チップ情報</param>
    ''' <remarks></remarks>
    Private Sub SetCommonClassInputVal(ByVal arg As CallBackArgumentClass, _
                                  ByVal dr As SMBCommonClassDataSet.SmbChipDetailInputInfoRow, _
                                  ByVal chipBaseDt As SC3240201DataSet.SC3240201ChipBaseInfoDataTable, _
                                  ByVal visitDt As SC3240201DataSet.SC3240201VisitInfoDataTable, _
                                  ByVal stallUseStatus As String, _
                                  ByVal stallUseId As Decimal)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '販売店コード
        dr.DLR_CD = arg.DlrCD

        '店舗コード
        dr.BRN_CD = arg.StrCD

        'サービス入庫
        dr.SVCIN_ID = arg.SvcInId

        '作業内容ID
        dr.JOB_DTL_ID = arg.JobDtlId

        'ストール利用ID
        dr.STALL_USE_ID = stallUseId

        'ストール利用ステータス
        dr.STALL_USE_STATUS = stallUseStatus

        'チップエリア
        dr.CHIP_AREA_TYPE = GetChipAreaType(arg.SubAreaId)

        '来店実績番号
        If visitDt IsNot Nothing AndAlso 0 < visitDt.Rows.Count Then
            dr.VISIT_SEQ = Me.ConvertDbNullToMinusNum(visitDt.Rows(0)("VISITSEQ"))
        Else
            dr.VISIT_SEQ = -1
        End If

        'R/O番号(親RO:サービス入庫)
        dr.RO_NUM = ConvertDbNullOrWhiteSpaceToEmpty(chipBaseDt.Rows(0)("RO_NUM"))

        'サービス入庫ステータス
        dr.SVC_STATUS = ConvertDbNullOrWhiteSpaceToEmpty(chipBaseDt.Rows(0)("SVC_STATUS"))

        '洗車有無
        dr.CARWASH_NEED_FLG = ConvertDbNullOrWhiteSpaceToEmpty(chipBaseDt.Rows(0)("WASHFLAG"))

        '完成検査フラグ
        dr.INSPECTION_STATUS = ConvertDbNullOrWhiteSpaceToEmpty(chipBaseDt.Rows(0)("INSPECTION_STATUS"))

        '実績開始日時
        dr.RSLT_START_DATETIME = ConvertDbMinDateToNull(chipBaseDt.Rows(0)("RESULT_STARTDATE"))

        '実績終了日時
        dr.RSLT_END_DATETIME = ConvertDbMinDateToNull(chipBaseDt.Rows(0)("RESULT_ENDDATE"))

        'R/O有無(RO情報の有無)
        Dim roRelationId As String
        If visitDt IsNot Nothing AndAlso 0 < visitDt.Rows.Count Then
            roRelationId = ConvertDbNullOrWhiteSpaceToEmpty(visitDt.Rows(0)("RO_RELATION_ID"))
        Else
            roRelationId = ""
        End If
        If String.IsNullOrEmpty(roRelationId) Then
            dr.RO_TYPE = "0"    '無し
        Else
            dr.RO_TYPE = "1"    '有り
        End If

        '清算書印刷日時
        dr.INVOICE_PRINT_DATETIME = ConvertDbMinDateToNull(chipBaseDt.Rows(0)("INVOICE_DATETIME"))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} dr. = [DLR_CD:{2}][BRN_CD:{3}][SVCIN_ID:{4}][JOB_DTL_ID:{5}][STALL_USE_ID:{6}][STALL_USE_STATUS:{7}][CHIP_AREA_TYPE:{8}][VISIT_SEQ:{9}][RO_NUM:{10}][SVC_STATUS:{11}][CARWASH_NEED_FLG:{12}][INSPECTION_STATUS:{13}][RSLT_START_DATETIME:{14}][RSLT_END_DATETIME:{15}][RO_TYPE:{16}][INVOICE_PRINT_DATETIME:{17}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dr.DLR_CD, dr.BRN_CD, dr.SVCIN_ID, dr.JOB_DTL_ID, dr.STALL_USE_ID, dr.STALL_USE_STATUS, dr.CHIP_AREA_TYPE, dr.VISIT_SEQ, dr.RO_NUM, dr.SVC_STATUS, dr.CARWASH_NEED_FLG, dr.INSPECTION_STATUS, dr.RSLT_START_DATETIME, dr.RSLT_END_DATETIME, dr.RO_TYPE, dr.INVOICE_PRINT_DATETIME))

    End Sub
    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' RO関連項目の送信
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Function SendROInfo(ByVal arg As CallBackArgumentClass) As Long

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

    '    Dim returnCode As Long = 0

    '    'RO情報変更フラグ＝0:変更なしの場合
    '    If arg.ROInfoChangeFlg.Equals("0") Then
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                    , "{0}.{1} ROInfo does not send. ROInfoChangeFlg = {2}" _
    '                    , Me.GetType.ToString _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                    , arg.ROInfoChangeFlg))
    '        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)
    '        Return returnCode
    '    End If

    '    'RO番号
    '    Dim roNum As String = arg.RONum.Trim()

    '    '飛び込みフラグ(0:予約／1:飛び込み)
    '    Dim rezFlg As String

    '    If arg.RezFlg.Trim() = "0" Then
    '        rezFlg = "1" '変換
    '    Else
    '        rezFlg = "0" '変換
    '    End If

    '    '洗車フラグ(0:無し／1:有り)
    '    Dim carWashFlg As String = arg.CarWashFlg.Trim()

    '    '待ち方フラグ(0:店内／4:店外)
    '    Dim waitingFlg As String

    '    If arg.WaitingFlg.Trim() = "0" Then
    '        waitingFlg = "0" '変換
    '    Else
    '        waitingFlg = "1" '変換
    '    End If

    '    'ご用命
    '    Dim order As String = arg.Order.Trim()

    '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    '    ''故障原因
    '    'Dim failure As String = arg.Failure.Trim()

    '    ''診断結果
    '    'Dim result As String = arg.Result.Trim()

    '    ''アドバイス
    '    'Dim advice As String = arg.Advice.Trim()

    '    '故障原因
    '    Dim failure As String = ""

    '    '診断結果
    '    Dim result As String = ""

    '    'アドバイス
    '    Dim advice As String = ""

    '    'メモ
    '    Dim memo As String = arg.Memo.Trim()
    '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '    '自分自身の作業内容ID
    '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    '    'Dim jobDtlId As Long = arg.JobDtlId
    '    Dim jobDtlId As Decimal = arg.JobDtlId
    '    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '    '整備に紐付いた予約IDのリスト(-1の場合は紐付かない)　※チップ詳細画面を開いた直後の状態
    '    Dim before_MatchingRezIdList As List(Of String) = arg.BeforeMatchingRezIdList

    '    '整備に紐付いた作業連番のリスト
    '    Dim roJobSeqList As List(Of String) = arg.ROJobSeqList

    '    '作業連番のセット
    '    '　受付・追加作業エリアの場合
    '    Dim roJobSeq As String = "0"
    '    If (arg.SubAreaId.Equals(SUBAREA_RECEPTION) Or arg.SubAreaId.Equals(SUBAREA_ADDWORK)) Then

    '        'サブチップが保持している作業連番が存在する場合
    '        If Not IsNothing(arg.ROJobSeq) Then
    '            roJobSeq = arg.ROJobSeq
    '        End If

    '    Else
    '        '予約IDリストが存在する場合
    '        If 0 < before_MatchingRezIdList.Count Then

    '            '整備に紐付いた予約IDリストの件数分Loop
    '            For i = 0 To before_MatchingRezIdList.Count - 1

    '                '自分自身の作業内容ID＝整備に紐付いた予約ID　の場合
    '                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    '                'If jobDtlId = CLng(before_MatchingRezIdList(i)) Then
    '                If jobDtlId = CType(before_MatchingRezIdList(i), Decimal) Then
    '                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '                    '作業連番をセット
    '                    '  紐付いていない場合（見つからなかった場合）は、初期値"0"となる→親ROとする
    '                    roJobSeq = roJobSeqList(i)
    '                    Exit For
    '                End If
    '            Next
    '        End If
    '    End If


    '    Dim rtnVal As Long = -1

    '    'RO関連項目の送信
    '    Dim iC3801503bl As New IC3801503BusinessLogic
    '    rtnVal = iC3801503bl.UpdateROInfo(arg.DlrCD, roNum, CType(roJobSeq, Integer), rezFlg, carWashFlg, waitingFlg, order, failure, result, advice)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} [roNum:{2}][roJobSeq:{3}][rezFlg:{4}][carWashFlg:{5}][waitingFlg:{6}][order:{7}][failure:{8}][result:{9}][advice:{10}]" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , roNum, roJobSeq, rezFlg, carWashFlg, waitingFlg, order, failure, result, advice))

    '    '更新失敗の場合
    '    If rtnVal = 1 Then
    '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                   , "{0}.{1} IC3801503.UpdateROInfo - ROInfo does not update. RoNum={2} RoJobSeq={3}" _
    '                   , Me.GetType.ToString _
    '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                   , roNum, roJobSeq))
    '        Return ActionResult.DmsLinkageError
    '    End If

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    '    Return returnCode

    'End Function
    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' その他の更新（RO作業連番、ストール利用ステータスの更新(リレーションチップ含む)）
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="updDate">更新日時</param>
    ''' <param name="ta">テーブルアダプタ</param>
    ''' <remarks></remarks>
    Private Function UpdateOtherData(ByVal arg As CallBackArgumentClass, _
                                ByVal updDate As Date, _
                                ByVal ta As SC3240201TableAdapter) As Dictionary(Of Decimal, String)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rezIdList As List(Of String) = arg.RezIdList                               'チップエリアに表示されている予約IDリスト
        Dim rezId_StallUseStatusList As List(Of String) = arg.RezIdStallUseStatusList  'チップエリアに表示されている予約のストール利用ステータスリスト
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim roJobSeq2List As List(Of String) = arg.ROJobSeq2List                       'チップエリアに表示されている予約の着工支持フラグリスト
        'Dim matchingRezIdList As List(Of String) = arg.MatchingRezIdList               '整備に紐付いた予約IDのリスト
        'Dim roJobSeqList As List(Of String) = arg.ROJobSeqList                         '整備に紐付いた作業連番のリスト
        Dim matchingRezIdList As List(Of String) = arg.MatchingRezIdList              '整備に紐付いた予約IDのリスト
        Dim invisibleInstructFlgList As List(Of String) = arg.InvisibleInstructFlgList    'チップエリアに表示されている予約の着工支持フラグリスト
        Dim instructStatusList As New Dictionary(Of Decimal, String)                '各チップの着工指示状態
        Dim before_MatchingRezIdList As List(Of String) = arg.BeforeMatchingRezIdList  '整備に紐付いた予約IDのリスト(全て-1の場合は全て紐付かない)　※チップ詳細画面を開いた直後の状態
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'チップエリアに表示されている予約IDリストが存在する場合
        If Not IsNothing(rezIdList) AndAlso 0 < rezIdList.Count Then

            'チップエリアに表示されている予約IDリストの件数分Loop
            For i = 0 To rezIdList.Count - 1

                Dim resultCD As Integer
                Dim isChecked As Boolean = False            '表示されているチップにチェックが入っているかを判断するためのフラグ
                Dim dispRezId As Decimal = CType(rezIdList(i), Decimal)  '表示されている予約ID
                Dim dispRezId_StallUseStatus As String = rezId_StallUseStatusList(i)  '表示されている予約のストール利用ステータス
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'Dim dispRoJobSeq2 As String = roJobSeq2List(i)                        '表示されている予約のRO作業連番
                'Dim updStallUseStatusVal As String          '更新用のストール利用ステータス
                'Dim updRoJobSeqVal As Long = -1             '更新用の作業連番
                Dim invisibleInstructFlg As String = invisibleInstructFlgList(i)     '表示されている予約の作業指示フラグ
                Dim updStallUseStatusVal As String                               '更新用のストール利用ステータス
                Dim before_updStallUseStatusVal As String                        '更新用のストール利用ステータス(※チップ詳細画面を開いた直後の状態)
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '整備に紐付いた予約IDリストの件数分Loop
                For j = 0 To matchingRezIdList.Count - 1

                    Dim checkRezId As Decimal = CType(matchingRezIdList(j), Decimal)  '整備に紐付いている予約ID

                    '一致する予約IDがある場合
                    If dispRezId = checkRezId Then

                        isChecked = True   'チェックあり

                        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                        'updRoJobSeqVal = CLng(roJobSeqList(j))   '作業連番をセット
                        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                        '内側(j)のループを中断する
                        Exit For
                    End If
                Next

                '一つでも整備にチェックが入っている場合
                If isChecked = True Then

                    'ステータスに「01：作業開始待ち」をセット
                    updStallUseStatusVal = STALL_USE_STATUS_01
                Else

                    'ステータスに「00：着工指示待ち」をセット
                    updStallUseStatusVal = STALL_USE_STATUS_00
                End If

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                isChecked = False
                '整備に紐付いた予約IDリストの件数分Loop(※チップ詳細画面を開いた直後の状態)
                For j = 0 To before_MatchingRezIdList.Count - 1

                    Dim checkRezId As Decimal = CType(before_MatchingRezIdList(j), Decimal)  '整備に紐付いている予約ID

                    '一致する予約IDがある場合
                    If dispRezId = checkRezId Then

                        isChecked = True   'チェックあり
                        Exit For
                    End If
                Next

                '一つでも整備にチェックが入っている場合
                If isChecked = True Then
                    'ステータスに「01：作業開始待ち」をセット
                    before_updStallUseStatusVal = STALL_USE_STATUS_01
                Else
                    'ステータスに「00：着工指示待ち」をセット
                    before_updStallUseStatusVal = STALL_USE_STATUS_00
                End If
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END


                '受付エリアの場合、画面上見えていない他チップの整備も考慮する
                '受付エリアの場合
                If (SUBAREA_RECEPTION.Equals(arg.SubAreaId)) Then

                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    ''チップエリアに表示されている予約が、既に整備と紐付いている状態、且つ、
                    ''チップエリアに表示されている予約のRO作業連番 ≠ 自チップのRO作業連番 の場合（画面上、見えない所でチェックが入っている）
                    'If Not dispRoJobSeq2.Equals("-1") AndAlso Not dispRoJobSeq2.Equals(arg.ROJobSeq) Then

                    '    'ステータスに「01：作業開始待ち」をセット
                    '    updStallUseStatusVal = STALL_USE_STATUS_01

                    '    '作業連番をセット
                    '    updRoJobSeqVal = CLng(dispRoJobSeq2)
                    'End If

                    'チップエリアに表示されている予約(GetRelatedChipInfo()で取得した情報)の着工支持フラグが着工支持済みの場合（画面上、見えない所でチェックが入っている場合）
                    '(着工支持フラグは、作業指示テーブルの着工支持テーブルが「1」、かつ、RO作業連番が自チップのRO作業連番でない場合に「1」)
                    If invisibleInstructFlg.Equals("1") Then

                        'ステータスに「01：作業開始待ち」をセット
                        updStallUseStatusVal = STALL_USE_STATUS_01
                        before_updStallUseStatusVal = STALL_USE_STATUS_01
                    End If
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                End If

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                ''作業内容.RO作業連番を更新する
                'resultCD = ta.UpdROJobSeq(arg, updDate, dispRezId, updRoJobSeqVal)

                'If resultCD = 0 Then
                '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                '                , "{0}.{1} RoJobSeq does not update. dispRezId = {2}" _
                '                , Me.GetType.ToString _
                '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                , dispRezId))
                'End If
                If Not updStallUseStatusVal.Equals(before_updStallUseStatusVal) Then
                    '画面を開いた直後と整備の紐付状態が変化している場合
                    If updStallUseStatusVal.Equals(STALL_USE_STATUS_01) Then
                        'ステータスに「01：作業開始待ち」の場合、着工指示開始
                        instructStatusList.Add(dispRezId, INSTRUCT_1)
                    Else
                        'ステータスに「00：着工指示待ち」の場合、着工指示キャンセル
                        instructStatusList.Add(dispRezId, INSTRUCT_0)
                    End If
                End If
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                'ストール利用ステータスが下記以外の場合、ステータスを更新する
                '（02:作業中、03:完了、04:作業指示の一部の作業が中断、05:中断、06:日跨ぎ終了）
                If Not (dispRezId_StallUseStatus.Equals("02") Or dispRezId_StallUseStatus.Equals("03") Or _
                        dispRezId_StallUseStatus.Equals("04") Or dispRezId_StallUseStatus.Equals("05") Or _
                        dispRezId_StallUseStatus.Equals("06")) Then

                    'ストール利用.ストール利用ステータスを更新する
                    resultCD = ta.UpdStallUseStatus(arg, updDate, dispRezId, updStallUseStatusVal)

                    If resultCD = 0 Then
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} StallUseStatus does not update. dispRezId = {2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , dispRezId))
                    End If
                End If
            Next
        End If

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        Return instructStatusList
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    End Function

    ''' <summary>
    ''' その他の更新（RO作業連番、ストール利用ステータスの更新(自チップ以外のリレーションチップ)）
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="updDate">更新日時</param>
    ''' <param name="ta">テーブルアダプタ</param>
    ''' <remarks></remarks>
    Private Function UpdateOtherDataUsingWebService(ByVal arg As CallBackArgumentClass, _
                                               ByVal updDate As Date, _
                                               ByVal ta As SC3240201TableAdapter) As Dictionary(Of Decimal, String)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rezIdList As List(Of String) = arg.RezIdList                               'チップエリアに表示されている予約IDリスト
        Dim rezId_StallUseStatusList As List(Of String) = arg.RezIdStallUseStatusList  'チップエリアに表示されている予約のストール利用ステータスリスト
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim roJobSeq2List As List(Of String) = arg.ROJobSeq2List                       'チップエリアに表示されている予約のRO作業連番リスト
        'Dim matchingRezIdList As List(Of String) = arg.MatchingRezIdList               '整備に紐付いた予約IDのリスト
        'Dim roJobSeqList As List(Of String) = arg.ROJobSeqList                         '整備に紐付いた作業連番のリスト
        Dim invisibleInstructFlgList As List(Of String) = arg.InvisibleInstructFlgList    'チップエリアに表示されている予約の着工支持フラグリスト
        Dim matchingRezIdList As List(Of String) = arg.MatchingRezIdList               '整備に紐付いた予約IDのリスト
        Dim instructStatusList As New Dictionary(Of Decimal, String)                '各チップの着工指示状態
        Dim before_MatchingRezIdList As List(Of String) = arg.BeforeMatchingRezIdList  '整備に紐付いた予約IDのリスト(全て-1の場合は全て紐付かない)　※チップ詳細画面を開いた直後の状態
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'チップエリアに表示されている予約IDリストが存在する場合
        If Not IsNothing(rezIdList) AndAlso 0 < rezIdList.Count Then

            'チップエリアに表示されている予約IDリストの件数分Loop
            For i = 0 To rezIdList.Count - 1

                Dim resultCD As Integer
                Dim isChecked As Boolean = False            '表示されているチップにチェックが入っているかを判断するためのフラグ
                Dim dispRezId As Decimal = CType(rezIdList(i), Decimal)  '表示されている予約ID
                Dim dispRezId_StallUseStatus As String = rezId_StallUseStatusList(i)  '表示されている予約のストール利用ステータス
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'Dim dispRoJobSeq2 As String = roJobSeq2List(i)                        '表示されている予約のRO作業連番
                'Dim updStallUseStatusVal As String          '更新用のストール利用ステータス
                'Dim updRoJobSeqVal As Long = -1             '更新用の作業連番
                Dim invisibleInstructFlg As String = invisibleInstructFlgList(i)     '表示されている予約の作業指示フラグ
                Dim updStallUseStatusVal As String          '更新用のストール利用ステータス
                Dim before_updStallUseStatusVal As String   '更新用のストール利用ステータス(※チップ詳細画面を開いた直後の状態)
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                ''自チップ以外の場合
                'If dispRezId <> arg.JobDtlId Then
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '整備に紐付いた予約IDリストの件数分Loop
                For j = 0 To matchingRezIdList.Count - 1

                    Dim checkRezId As Decimal = CType(matchingRezIdList(j), Decimal)  '整備に紐付いている予約ID

                    '一致する予約IDがある場合
                    If dispRezId = checkRezId Then

                        isChecked = True   'チェックあり

                        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                        'updRoJobSeqVal = CLng(roJobSeqList(j))   '作業連番をセット
                        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                        '内側(j)のループを中断する
                        Exit For
                    End If
                Next

                '一つでも整備にチェックが入っている場合
                If isChecked = True Then

                    'ステータスに「01：作業開始待ち」をセット
                    updStallUseStatusVal = STALL_USE_STATUS_01
                Else

                    'ステータスに「00：着工指示待ち」をセット
                    updStallUseStatusVal = STALL_USE_STATUS_00
                End If

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                isChecked = False
                '整備に紐付いた予約IDリストの件数分Loop(※チップ詳細画面を開いた直後の状態)
                For j = 0 To before_MatchingRezIdList.Count - 1

                    Dim checkRezId As Decimal = CType(before_MatchingRezIdList(j), Decimal)  '整備に紐付いている予約ID

                    '一致する予約IDがある場合
                    If dispRezId = checkRezId Then

                        isChecked = True   'チェックあり
                        Exit For
                    End If
                Next

                '一つでも整備にチェックが入っている場合
                If isChecked = True Then
                    'ステータスに「01：作業開始待ち」をセット
                    before_updStallUseStatusVal = STALL_USE_STATUS_01
                Else
                    'ステータスに「00：着工指示待ち」をセット
                    before_updStallUseStatusVal = STALL_USE_STATUS_00
                End If
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '受付エリアの場合、画面上見えていない他チップの整備も考慮する
                '受付エリアの場合
                If (SUBAREA_RECEPTION.Equals(arg.SubAreaId)) Then

                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    ''チップエリアに表示されている予約が、既に整備と紐付いている状態、且つ、
                    ''チップエリアに表示されている予約のRO作業連番 ≠ 自チップのRO作業連番 の場合（画面上、見えない所でチェックが入っている）
                    'If Not dispRoJobSeq2.Equals("-1") AndAlso Not dispRoJobSeq2.Equals(arg.ROJobSeq) Then

                    '    'ステータスに「01：作業開始待ち」をセット
                    '    updStallUseStatusVal = STALL_USE_STATUS_01

                    '    '作業連番をセット
                    '    updRoJobSeqVal = CLng(dispRoJobSeq2)
                    'End If

                    'チップエリアに表示されている予約(GetRelatedChipInfo()で取得した情報)の着工支持フラグが着工支持済みの場合（画面上、見えない所でチェックが入っている場合）
                    '(着工支持フラグは、作業指示テーブルの着工支持テーブルが「1」、かつ、RO作業連番が自チップのRO作業連番でない場合に「1」)
                    If invisibleInstructFlg.Equals("1") Then
                        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                        'ステータスに「01：作業開始待ち」をセット
                        updStallUseStatusVal = STALL_USE_STATUS_01
                        before_updStallUseStatusVal = STALL_USE_STATUS_01
                    End If
                    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                End If

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                If Not updStallUseStatusVal.Equals(before_updStallUseStatusVal) Then
                    '画面を開いた直後と整備の紐付状態が変化している場合
                    If updStallUseStatusVal.Equals(STALL_USE_STATUS_01) Then
                        'ステータスに「01：作業開始待ち」の場合、着工指示開始
                        instructStatusList.Add(dispRezId, INSTRUCT_1)
                    Else
                        'ステータスに「00：着工指示待ち」の場合、着工指示キャンセル
                        instructStatusList.Add(dispRezId, INSTRUCT_0)
                    End If
                End If
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                ''作業内容.RO作業連番を更新する
                'resultCD = ta.UpdROJobSeq(arg, updDate, dispRezId, updRoJobSeqVal)

                'If resultCD = 0 Then
                '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                '                , "{0}.{1} RoJobSeq does not update. dispRezId = {2}" _
                '                , Me.GetType.ToString _
                '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                , dispRezId))
                'End If

                ''ストール利用ステータスが下記以外の場合、ステータスを更新する
                ''（02:作業中、03:完了、04:作業指示の一部の作業が中断、05:中断、06:日跨ぎ終了）
                'If Not (dispRezId_StallUseStatus.Equals("02") Or dispRezId_StallUseStatus.Equals("03") Or _
                '        dispRezId_StallUseStatus.Equals("04") Or dispRezId_StallUseStatus.Equals("05") Or _
                '        dispRezId_StallUseStatus.Equals("06")) Then

                '    'ストール利用.ストール利用ステータスを更新する
                '    resultCD = ta.UpdStallUseStatus(arg, updDate, dispRezId, updStallUseStatusVal)

                '    If resultCD = 0 Then
                '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                '                    , "{0}.{1} StallUseStatus does not update. dispRezId = {2}" _
                '                    , Me.GetType.ToString _
                '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '                    , dispRezId))
                '    End If
                'End If
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                '自チップ以外の場合
                If dispRezId <> arg.JobDtlId Then
                    'ストール利用ステータスが下記以外の場合、ステータスを更新する
                    '（02:作業中、03:完了、04:作業指示の一部の作業が中断、05:中断、06:日跨ぎ終了）
                    If Not (dispRezId_StallUseStatus.Equals("02") Or dispRezId_StallUseStatus.Equals("03") Or _
                            dispRezId_StallUseStatus.Equals("04") Or dispRezId_StallUseStatus.Equals("05") Or _
                            dispRezId_StallUseStatus.Equals("06")) Then

                        'ストール利用.ストール利用ステータスを更新する
                        resultCD = ta.UpdStallUseStatus(arg, updDate, dispRezId, updStallUseStatusVal)

                        If resultCD = 0 Then
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} StallUseStatus does not update. dispRezId = {2}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , dispRezId))
                        End If
                    End If
                End If
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'End If
                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            Next
        End If

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        Return instructStatusList
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    End Function

    ''' <summary>
    ''' 更新用のRO作業連番、ストール利用ステータスを取得する（自チップのみ）
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <remarks></remarks>
    Private Function GetUpdateReserveInfo(ByVal arg As CallBackArgumentClass) As SC3240201DataSet.SC3240201UpdateReserveInfoRow

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Using rtnDt As New SC3240201DataSet.SC3240201UpdateReserveInfoDataTable

            Dim rtnRow As SC3240201DataSet.SC3240201UpdateReserveInfoRow = _
                    DirectCast(rtnDt.NewRow(), SC3240201DataSet.SC3240201UpdateReserveInfoRow)

            Dim rezIdList As List(Of String) = arg.RezIdList                               'チップエリアに表示されている予約IDリスト
            Dim rezId_StallUseStatusList As List(Of String) = arg.RezIdStallUseStatusList  'チップエリアに表示されている予約のストール利用ステータスリスト
            Dim matchingRezIdList As List(Of String) = arg.MatchingRezIdList               '整備に紐付いた予約IDのリスト
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim roJobSeqList As List(Of String) = arg.ROJobSeqList                         '整備に紐付いた作業連番のリスト
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            Dim updStallUseStatusVal As String = String.Empty    '更新用のストール利用ステータス
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim updRoJobSeqVal As Long = -1                      '更新用の作業連番
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            'RO番号がある場合、且つ、チップエリアに表示されている予約IDリストが存在する場合
            'If Not (String.IsNullOrEmpty(arg.RONum.Trim())) And 0 < rezIdList.Count Then
            If Not (String.IsNullOrEmpty(arg.RONum.Trim())) AndAlso Not IsNothing(rezIdList) AndAlso 0 < rezIdList.Count Then

                'チップエリアに表示されている予約IDリストの件数分Loop
                For i = 0 To rezIdList.Count - 1

                    Dim isChecked As Boolean = False            '表示されているチップにチェックが入っているかを判断するためのフラグ
                    Dim dispRezId As Decimal = CType(rezIdList(i), Decimal)  '表示されている予約ID
                    Dim dispRezId_StallUseStatus As String = rezId_StallUseStatusList(i)  '表示されている予約のストール利用ステータス

                    '自チップの場合のみ
                    If dispRezId = arg.JobDtlId Then

                        '整備に紐付いた予約IDリストの件数分Loop
                        For j = 0 To matchingRezIdList.Count - 1

                            Dim checkRezId As Decimal = CType(matchingRezIdList(j), Decimal)  '整備に紐付いている予約ID

                            '一致する予約IDがある場合
                            If dispRezId = checkRezId Then

                                isChecked = True   'チェックあり

                                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                                'updRoJobSeqVal = CLng(roJobSeqList(j))   '作業連番をセット
                                '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                                '内側(j)のループを中断する
                                Exit For
                            End If
                        Next

                        '一つでも整備にチェックが入っている場合
                        If isChecked = True Then

                            'ステータスに「01：作業開始待ち」をセット
                            updStallUseStatusVal = STALL_USE_STATUS_01
                        Else

                            'ステータスに「00：着工指示待ち」をセット
                            updStallUseStatusVal = STALL_USE_STATUS_00
                        End If

                        '2014/08/19 TMEJ 明瀬 NoShowエリアのチップが消える不具合対応 START

                        ''ストール利用ステータスが下記の場合、ステータスを更新しない為、空をセットする
                        ''（02:作業中、03:完了、04:作業指示の一部の作業が中断、05:中断、06:日跨ぎ終了）
                        'If (dispRezId_StallUseStatus.Equals("02") Or dispRezId_StallUseStatus.Equals("03") Or _
                        '    dispRezId_StallUseStatus.Equals("04") Or dispRezId_StallUseStatus.Equals("05") Or _
                        '    dispRezId_StallUseStatus.Equals("06")) Then

                        '    updStallUseStatusVal = String.Empty   '更新しない為、空をセット
                        'End If

                        'ストール利用ステータスが下記の場合、ステータスを更新しない為、空をセットする
                        '（02:作業中、03:完了、04:作業指示の一部の作業が中断、05:中断、06:日跨ぎ終了、07:未来店客）
                        If (dispRezId_StallUseStatus.Equals("02") _
                        Or dispRezId_StallUseStatus.Equals("03") _
                        Or dispRezId_StallUseStatus.Equals("04") _
                        Or dispRezId_StallUseStatus.Equals("05") _
                        Or dispRezId_StallUseStatus.Equals("06") _
                        Or dispRezId_StallUseStatus.Equals("07")) Then

                            '更新しない為、空をセット
                            updStallUseStatusVal = String.Empty

                        End If

                        '2014/08/19 TMEJ 明瀬 NoShowエリアのチップが消える不具合対応 END

                        '外側(i)のループを中断する
                        Exit For
                    End If
                Next
            End If

            rtnRow.STALLUSESTATUS = updStallUseStatusVal     '更新用のストール利用ステータス
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'rtnRow.ROJOBSEQ = updRoJobSeqVal                 '更新用の作業連番
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

            Return rtnRow

        End Using

    End Function

    ''' <summary>
    ''' 予約情報更新WebServiceをCallする
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="updDate">更新日時</param>
    ''' <param name="updateReserveInfo">更新用のRO作業連番、ストール利用ステータス</param>
    ''' <param name="startPlanTime">更新用の作業開始予定日時</param>
    ''' <param name="finishPlanTime">更新用の作業完了予定日時</param>
    ''' <param name="svcClassCD">更新用のサービス分類コード</param>
    ''' <remarks></remarks>
    Private Function CallUpdateReserve(ByVal arg As CallBackArgumentClass, _
                                       ByVal updDate As Date, _
                                       ByVal updateReserveInfo As SC3240201DataSet.SC3240201UpdateReserveInfoRow, _
                                       ByVal startPlanTime As Date, _
                                       ByVal finishPlanTime As Date, _
                                       ByVal svcClassCD As String) As SMBCommonClassDataSet.WebServiceResultRow

        '開始ログの出力
        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rowWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow = Nothing

        Try
            'WebService送信用XMLクラス作成処理
            Dim sendXml As XmlDocumentClass = CreateXml(arg, updDate, updateReserveInfo, startPlanTime, finishPlanTime, svcClassCD)

            'WebService呼出処理
            Using common As New SMBCommonClassBusinessLogic
                rowWebServiceResult = common.CallReserveWebService(sendXml)
            End Using

            'WebServiceの結果確認
            If rowWebServiceResult Is Nothing Then
                'WebService処理失敗

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} WebServiceErr " _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

                Return rowWebServiceResult

            End If

            '終了ログの出力
            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

            Return rowWebServiceResult

        Catch ex As System.Net.WebException
            'WebServiceエラー

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ErrWebService = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex.Message))

            Return rowWebServiceResult

        End Try

    End Function

    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START

    ''' <summary>
    ''' データセット内のテーブルに基幹連携の結果コードを設定する
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="resultCode">結果コード</param>
    ''' <remarks></remarks>
    Private Sub SetLinkResult(ByVal ds As SC3240201DataSet, _
                              ByVal resultCode As Long)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[resultCode:{0}]", resultCode)

        Dim resultCodeTable As SC3240201DataSet.SC3240201WebServiceResultDataTable _
            = ds.SC3240201WebServiceResult

        resultCodeTable.AddSC3240201WebServiceResultRow(resultCode)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' チップエリア種類を取得する
    ''' </summary>
    ''' <param name="subAreaId">サブエリアID</param>
    ''' <remarks></remarks>
    Private Function GetChipAreaType(ByVal subAreaId As String) As Integer

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[subAreaId:{0}]", subAreaId)

        Dim ChipAreaType As Integer = 1
        Select Case subAreaId
            Case SUBAREA_RECEPTION
                '受付
                ChipAreaType = CHIP_AREA_TYPE_RECEPTION

            Case SUBAREA_ADDWORK
                '追加作業
                ChipAreaType = CHIP_AREA_TYPE_ADDWORK

            Case SUBAREA_CONFIRMED_INSPECTION
                '完成検査
                ChipAreaType = CHIP_AREA_TYPE_CONFIRMED_INSPECTION

            Case SUBAREA_WAITING_WASH, SUBAREA_WASHING
                '洗車
                ChipAreaType = CHIP_AREA_TYPE_WASH

            Case SUBAREA_WAIT_DELIVERY
                '納車待ち
                ChipAreaType = CHIP_AREA_TYPE_WAIT_DELIVERY

            Case SUBAREA_STOP
                '中断
                ChipAreaType = CHIP_AREA_TYPE_STOP

            Case SUBAREA_NOSHOW
                'NoShow
                ChipAreaType = CHIP_AREA_TYPE_NOSHOW

            Case Else
                'ストール
                ChipAreaType = CHIP_AREA_TYPE_STALL
        End Select

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[ChipAreaType:{0}]", ChipAreaType)

        Return ChipAreaType

    End Function
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

#Region "変換処理"

    ''' <summary>
    ''' 画面表示用に変換した日時を取得する
    ''' </summary>
    ''' <param name="showDate"></param>
    ''' <param name="objColumn"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' ・画面に表示している日時と同じ日時の場合はHH:mm
    ''' ・そうでない場合はMM/dd
    ''' </remarks>
    Private Function GetDispDateTimeString(ByVal showDate As String, ByVal objColumn As Object) As String

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[showDate:{0}]", showDate)

        Dim rtnVal As String = String.Empty

        If Not IsDBNull(objColumn) _
            AndAlso (CDate(objColumn) > Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture)) Then
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'If DateTimeFunc.FormatDate(Format.yyyyMMdd, CDate(objColumn)).Equals(showDate) Then
            If CDate(objColumn).ToString(FORMAT_DATE_YYYYMMDD, CultureInfo.InvariantCulture).Equals(showDate) Then
                '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                rtnVal = CutFrontZeroOfHours(DateTimeFunc.FormatDate(Format.HHmm, CDate(objColumn)))
            Else
                rtnVal = CutFrontZeroOfMonthDate(DateTimeFunc.FormatDate(Format.MMdd, CDate(objColumn)))
            End If

        End If

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

    ''' <summary>
    ''' 時間変換 (基準日と対象日時が別日：MM/dd hh:mm、基準日と対象日時が同日：hh:mm)
    ''' </summary>
    ''' <param name="time">対象日時</param>
    ''' <param name="inNowDate">比較する基準日</param>
    ''' <returns>変換値</returns>
    '''<remarks></remarks>
    Private Function SetDateTimeToStringDetail(ByVal time As DateTime, _
                                               ByVal inNowDate As Date) As String

        '開始ログ
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[time:{0}][inNowDate:{1}]", time, inNowDate)

        Dim strResult As String

        '日付チェック
        If time.Equals(DateTime.MinValue) Then
            Return String.Empty
        End If

        Try
            If Not inNowDate.Date = time.Date Then
                '(MM/dd hh:mm)
                '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'strResult = time.ToString("MM/dd HH:mm", CultureInfo.CurrentCulture)
                strResult = DateTimeFunc.FormatDate(Format.MMdd, time) + " " + DateTimeFunc.FormatDate(Format.HHmm, time)
                '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            Else
                '(hh:mm)
                strResult = DateTimeFunc.FormatDate(14, time)
            End If

        Catch ex As FormatException
            strResult = String.Empty
        End Try

        '終了ログ
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[strResult:{0}]", strResult)

        Return strResult

    End Function

    ''' <summary>
    ''' 時間変換２ (基準日と対象日時が別日：MM/dd、基準日と対象日時が同日：hh:mm)
    ''' </summary>
    ''' <param name="time">対象日時</param>
    ''' <param name="inNowDate">比較する基準日</param>
    ''' <returns>変換値</returns>
    '''<remarks></remarks>
    Private Function SetDateTimeToStringDetail2(ByVal time As DateTime, _
                                               ByVal inNowDate As Date) As String

        '開始ログ
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[time:{0}][inNowDate:{1}]", time, inNowDate)

        Dim strResult As String
        Dim strDefaultTime As String = WebWordUtility.GetWord(MY_PROGRAMID, 10)   '--:--

        ' 日付チェック
        If time.Equals(DateTime.MinValue) Then
            Return strDefaultTime   '--:--
        End If

        Try
            If Not inNowDate.Date = time.Date Then
                '(MM/dd)
                '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                'strResult = time.ToString("MM/dd", CultureInfo.CurrentCulture)
                strResult = DateTimeFunc.FormatDate(Format.MMdd, time)
                '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            Else
                '(hh:mm)
                strResult = DateTimeFunc.FormatDate(14, time)
            End If

        Catch ex As FormatException
            strResult = strDefaultTime   '--:--
        End Try

        '終了ログ
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[strResult:{0}]", strResult)

        Return strResult

    End Function

    ''' <summary>
    ''' DBNullチェックをした値を返却する(String)
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns>DBNull:""/Not DBNull:StringにCastして返却</returns>
    ''' <remarks></remarks>
    Private Function ConvertDbNullToEmpty(ByVal objColumn As Object) As String

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rtnVal As String = String.Empty

        If Not IsDBNull(objColumn) Then
            rtnVal = CStr(objColumn)
        End If

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    Private Function ConvertDbNullOrWhiteSpaceToEmpty(ByVal objColumn As Object) As String

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rtnVal As String = String.Empty

        If Not IsDBNull(objColumn) Then
            If String.IsNullOrWhiteSpace(CStr(objColumn)) Then
                Return rtnVal
            Else
                rtnVal = CStr(objColumn)
            End If
        End If

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function
    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ' ''' <summary>
    ' ''' DBNullチェックをした値を返却する(String)
    ' ''' </summary>
    ' ''' <param name="objColumn"></param>
    ' ''' <returns>DBNull:"0"/Not DBNull:StringにCastして返却</returns>
    ' ''' <remarks></remarks>
    'Private Function ConvertDbNullToStrZero(ByVal objColumn As Object) As String

    '    'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

    '    Dim rtnVal As String = "0"

    '    If Not IsDBNull(objColumn) Then
    '        rtnVal = CStr(objColumn)
    '    End If

    '    'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

    '    Return rtnVal

    'End Function

    ''' <summary>
    ''' DBNULLチェックをした値を返却する(String)
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns>DBNull:""/Not DBNull:yyyy/MM/dd HH:mmに変換した文字列を返却</returns>
    ''' <remarks></remarks>
    Private Function ConvertDbNullToEmptyDateString(ByVal objColumn As Object) As String

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rtnVal As String = String.Empty

        If Not IsDBNull(objColumn) _
            AndAlso (CDate(objColumn) > Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture)) Then
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'rtnVal = DateTimeFunc.FormatDate(Format.yyyyMMddHHmm, CDate(objColumn))

            rtnVal = CDate(objColumn).ToString(FORMAT_DATE_YYYYMMDDHHMM, CultureInfo.InvariantCulture)
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        End If

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

    ''' <summary>
    ''' 最小日付チェックをした値を返却する(Date)
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns>DBNull or DATE_MIN_VALUE:NULL / Not DBNull:パラメータを日付型に変換した値を返却</returns>
    ''' <remarks></remarks>
    Private Function ConvertDbMinDateToNull(ByVal objColumn As Object) As Date

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rtnVal As Date

        If Not IsDBNull(objColumn) _
            AndAlso (CDate(objColumn) > Date.Parse(DATE_MIN_VALUE, CultureInfo.InvariantCulture)) Then
            rtnVal = CDate(objColumn)
        Else
            rtnVal = Nothing
        End If

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

    ''' <summary>
    ''' DBNull、0以下チェックをした値を返却する(Decimal)　※予約ID変換用
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns>DBNull or 0以下:-1/Not DBNull:DecimalにCastして返却</returns>
    ''' <remarks></remarks>
    Private Function ConvertRezId(ByVal objColumn As Object) As Decimal

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim rtnVal As Long = -1
        '
        'If Not IsDBNull(objColumn) AndAlso CLng(objColumn) > 0 Then
        '    rtnVal = CLng(objColumn)
        'End If
        '
        Dim rtnVal As Decimal = -1

        If Not IsDBNull(objColumn) AndAlso CType(objColumn, Decimal) > 0 Then
            rtnVal = CType(objColumn, Decimal)
        End If
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

    ''' <summary>
    ''' DBNullチェックをした値を返却する(Long)
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns>DBNull:-1/Not DBNull:LongにCastして返却</returns>
    ''' <remarks></remarks>
    Private Function ConvertDbNullToMinusNum(ByVal objColumn As Object) As Long

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rtnVal As Long = -1

        If Not IsDBNull(objColumn) Then
            rtnVal = CLng(objColumn)
        End If

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' DBNullチェックをした値を返却する(Decimal)
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns>DBNull:-1/Not DBNull:DecimalにCastして返却</returns>
    ''' <remarks></remarks>
    Private Function ConvertDbNullToMinusNum_Decimal(ByVal objColumn As Object) As Decimal

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rtnVal As Decimal = -1

        If Not IsDBNull(objColumn) Then
            rtnVal = CType(objColumn, Decimal)
        End If

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function
    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' HH:mmのHHが一桁の場合、前ゼロをカットする
    ''' </summary>
    ''' <param name="time"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CutFrontZeroOfHours(ByVal time As String) As String

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[time:{0}]", time)

        Dim rtnVal As String = String.Empty

        If Left(time, 1).Equals("0") Then
            rtnVal = time.Substring(1, 1) & ":" & Right(time, 2)
        Else
            rtnVal = Left(time, 2) & ":" & Right(time, 2)
        End If

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

    ''' <summary>
    ''' MM/ddのMMとddが一桁の場合、前ゼロをカットする
    ''' </summary>
    ''' <param name="md"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CutFrontZeroOfMonthDate(ByVal md As String) As String

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[monthDate:{0}]", md)

        Dim rtnVal As String = String.Empty

        Dim strMonth As String = Left(md, 2)
        Dim strDate As String = Right(md, 2)

        If Left(strMonth, 1).Equals("0") Then
            rtnVal = strMonth.Substring(1, 1) & "/"
        Else
            rtnVal = strMonth & "/"
        End If

        If Left(strDate, 1).Equals("0") Then
            rtnVal += strDate.Substring(1, 1)
        Else
            rtnVal += strDate
        End If

        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

#End Region

#Region "WebService呼び出しの要否チェック"
    ''' <summary>
    ''' WebServiceを呼び出すかどうかチェックし、結果を返す
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns>True:"WebServiceを呼び出す/False:WebServiceを呼び出さない</returns>
    '''<remarks></remarks>
    Private Function IsCallWebService(ByVal arg As CallBackArgumentClass) As Boolean

        '開始ログ
        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[StallUseId:{0}][JobDtlId:{1}][SvcInId:{2}][CstId:{3}][VclId:{4}]", _
                      arg.StallUseId, arg.JobDtlId, arg.SvcInId, arg.CstId, arg.VclId)

        Dim rtnVal As Boolean = True

        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        '受付、または、追加作業エリアの場合
        If (SUBAREA_RECEPTION.Equals(arg.SubAreaId) Or SUBAREA_ADDWORK.Equals(arg.SubAreaId)) Then

            rtnVal = False    'WebServiceを呼び出さない
        End If
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'ストール利用ID = 0、もしくは作業内容ID = 0、もしくはサービス入庫ID = 0の場合
        If arg.StallUseId = 0 OrElse arg.JobDtlId = 0 OrElse arg.SvcInId = 0 Then

            rtnVal = False    'WebServiceを呼び出さない
        End If

        ' ''追加作業エリアの場合
        ''If arg.SubAreaId.Equals(SUBAREA_ADDWORK) Then

        ''    rtnVal = False    'WebServiceを呼び出さない
        ''End If

        '終了ログ
        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function
#End Region

#Region "XML送信用クラス作成"

    ''' <summary>
    ''' XML作成(メイン)
    ''' </summary>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="updDate">更新日時</param>
    ''' <param name="updateReserveInfo">更新用のRO作業連番、ストール利用ステータス</param>
    ''' <param name="startPlanTime">更新用の作業開始予定日時</param>
    ''' <param name="finishPlanTime">更新用の作業完了予定日時</param>
    ''' <param name="svcClassCD">更新用のサービス分類コード</param>
    ''' <returns>XML送信用クラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXml(ByVal arg As CallBackArgumentClass, _
                               ByVal updDate As Date, _
                               ByVal updateReserveInfo As SC3240201DataSet.SC3240201UpdateReserveInfoRow, _
                               ByVal startPlanTime As Date, _
                               ByVal finishPlanTime As Date, _
                               ByVal svcClassCD As String) As XmlDocumentClass

        '開始ログの出力
        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'XMLクラスのインスタンス化
        Dim sendXml As New XmlDocumentClass

        'XMLのHeadTagの作成処理
        sendXml = CreateHeadTag(sendXml, updDate)

        'XMLのDetailTagの作成処理
        sendXml = CreateDetailTag(sendXml, arg, updateReserveInfo, startPlanTime, finishPlanTime, svcClassCD)

        '終了ログの出力
        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        Return sendXml

    End Function

    ''' <summary>
    ''' XML作成(HeadTag)
    ''' </summary>
    ''' <param name="sendXml">予約情報更新引数</param>
    ''' <param name="updDate">更新日時</param>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateHeadTag(ByVal sendXml As XmlDocumentClass, _
                                   ByVal updDate As Date) As XmlDocumentClass

        ''開始ログの出力
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '送信日時
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'sendXml.Head.TransmissionDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", updDate)

        Using smbCommonBiz As New ServiceCommonClassBusinessLogic
            Dim dateFormat As String = smbCommonBiz.GetSystemSettingValueBySettingName(SYSDATEFORMAT)
            If String.IsNullOrEmpty(dateFormat) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                              "{0}.Error ErrCode:Failed to get System Date Format.", _
                              MethodBase.GetCurrentMethod.Name))
                'システム設定値から取得できない場合、固定値とする
                sendXml.Head.TransmissionDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", updDate)
            Else
                'システム設定値から変換したDateFormatで設定
                sendXml.Head.TransmissionDate = updDate.ToString(dateFormat, CultureInfo.InvariantCulture)
            End If
        End Using
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        ''終了ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:RETURNCODE = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , sendXml))

        Return sendXml

    End Function

    ''' <summary>
    ''' XML作成(DetailTag)
    ''' </summary>
    ''' <param name="sendXml">予約情報更新引数</param>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="updateReserveInfo">更新用のRO作業連番、ストール利用ステータス</param>
    ''' <param name="startPlanTime">更新用の作業開始予定日時</param>
    ''' <param name="finishPlanTime">更新用の作業完了予定日時</param>
    ''' <param name="svcClassCD">更新用のサービス分類コード</param>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateDetailTag(ByVal sendXml As XmlDocumentClass, _
                                     ByVal arg As CallBackArgumentClass, _
                                     ByVal updateReserveInfo As SC3240201DataSet.SC3240201UpdateReserveInfoRow, _
                                     ByVal startPlanTime As Date, _
                                     ByVal finishPlanTime As Date, _
                                     ByVal svcClassCD As String) As XmlDocumentClass

        ''開始ログの出力
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'XMLのCommonTagの作成処理
        sendXml = CreateCommonTag(sendXml, arg)

        'XMLのReserveInformationTagの作成処理
        sendXml = CreateReserveInformationTag(sendXml, arg, updateReserveInfo, startPlanTime, finishPlanTime, svcClassCD)

        ''終了ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:RETURNCODE = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , sendXml))

        Return sendXml

    End Function

    ''' <summary>
    ''' XML作成(CommonTag)
    ''' </summary>
    ''' <param name="sendXml">予約情報更新引数</param>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateCommonTag(ByVal sendXml As XmlDocumentClass, _
                                     ByVal arg As CallBackArgumentClass) As XmlDocumentClass

        ''開始ログの出力
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '販売店コード
        sendXml.Detail.Common.DealerCode = arg.DlrCD

        '店舗コード
        sendXml.Detail.Common.BranchCode = arg.StrCD

        'スタッフコード
        sendXml.Detail.Common.StaffCode = Nothing

        '基幹顧客コード
        sendXml.Detail.Common.CustomerCode = Nothing

        '注文番号
        sendXml.Detail.Common.SalesBookingNumber = Nothing

        'VIN
        sendXml.Detail.Common.Vin = Nothing

        ''終了ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:RETURNCODE = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , sendXml))

        Return sendXml

    End Function

    ''' <summary>
    ''' XML作成(ReserveInformationTag)
    ''' </summary>
    ''' <param name="sendXml">予約情報更新引数</param>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="updateReserveInfo">更新用のRO作業連番、ストール利用ステータス</param>
    ''' <param name="startPlanTime">更新用の作業開始予定日時</param>
    ''' <param name="finishPlanTime">更新用の作業完了予定日時</param>
    ''' <param name="svcClassCD">更新用のサービス分類コード</param>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateReserveInformationTag(ByVal sendXml As XmlDocumentClass, _
                                                 ByVal arg As CallBackArgumentClass, _
                                                 ByVal updateReserveInfo As SC3240201DataSet.SC3240201UpdateReserveInfoRow, _
                                                 ByVal startPlanTime As Date, _
                                                 ByVal finishPlanTime As Date, _
                                                 ByVal svcClassCD As String) As XmlDocumentClass

        ''開始ログの出力
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'XMLのReserve_CustomerInformationTagの作成処理
        sendXml = CreateReserveCustomerInformationTag(sendXml, arg)

        'XMLのReserve_VehicleInformationTagの作成処理
        sendXml = CreateReserveVehicleInformationTag(sendXml, arg)

        'XMLのReserve_ServiceInformationTagの作成処理
        sendXml = CreateReserveServiceInformationTag(sendXml, arg, startPlanTime, finishPlanTime, svcClassCD)

        'Seq
        sendXml.Detail.ReserveInformation.SeqNo = Nothing

        'REZID
        sendXml.Detail.ReserveInformation.ReserveId = CType(arg.JobDtlId, String)

        'BASREZID
        sendXml.Detail.ReserveInformation.BasReserveId = Nothing

        'PREZID
        sendXml.Detail.ReserveInformation.PReserveId = Nothing

        'STATUS(Walk-inフラグ=「1:飛び込み」の場合、STATUSは「1:本予約」にしないとエラーになる)
        If arg.RezFlg.Equals("1") Then
            'Walk-inフラグ=「1:飛び込み」の場合

            sendXml.Detail.ReserveInformation.Status = "1"
        Else

            sendXml.Detail.ReserveInformation.Status = Nothing
        End If

        'WALKIN
        sendXml.Detail.ReserveInformation.WalkIn = arg.RezFlg

        'SMSFLG
        sendXml.Detail.ReserveInformation.SmsFlg = Nothing

        'CANCELFLG
        sendXml.Detail.ReserveInformation.CancelFlg = Nothing

        'NOSHOWFLG
        sendXml.Detail.ReserveInformation.NoShowFlg = Nothing

        'WORKORDERFLG
        If String.IsNullOrEmpty(updateReserveInfo.STALLUSESTATUS) Then
            '更新用のストール利用ステータスが空の場合

            sendXml.Detail.ReserveInformation.WorkOrderFlg = Nothing

        ElseIf updateReserveInfo.STALLUSESTATUS.Equals(STALL_USE_STATUS_00) Then
            '更新用のストール利用ステータスが「00：着工指示待ち」の場合

            sendXml.Detail.ReserveInformation.WorkOrderFlg = INSTRUCT_0

        ElseIf updateReserveInfo.STALLUSESTATUS.Equals(STALL_USE_STATUS_01) Then
            '更新用のストール利用ステータスが「01：作業開始待ち」の場合

            sendXml.Detail.ReserveInformation.WorkOrderFlg = INSTRUCT_1
        End If

        'ACCOUNT_PLAN        
        sendXml.Detail.ReserveInformation.AcountPlan = Nothing

        'MEMO
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        If String.IsNullOrWhiteSpace(arg.Order) Then
            sendXml.Detail.ReserveInformation.Memo = Nothing
        Else
            sendXml.Detail.ReserveInformation.Memo = arg.Order
        End If
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'UPDATEACCOUNT
        sendXml.Detail.ReserveInformation.UpdateAccount = String.Empty

        'R_O
        sendXml.Detail.ReserveInformation.OerderNo = Nothing

        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ''RO_JOB_SEQ
        'sendXml.Detail.ReserveInformation.OerderJobSeq = CType(updateReserveInfo.ROJOBSEQ, String)
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'ROW_LOCK_VERSION
        sendXml.Detail.ReserveInformation.RowLockVersion = CType(arg.RowLockVersion, String)


        ''終了ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:RETURNCODE = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , sendXml))

        Return sendXml

    End Function

    ''' <summary>
    ''' XML作成(Reserve_CustomerInformationTag)
    ''' </summary>
    ''' <param name="sendXml">予約情報更新引数</param>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateReserveCustomerInformationTag(ByVal sendXml As XmlDocumentClass, _
                                                         ByVal arg As CallBackArgumentClass) As XmlDocumentClass

        ''開始ログの出力
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        '顧客ID
        Dim cstId As String = arg.CstId.ToString(CultureInfo.InvariantCulture)
        If Not String.IsNullOrWhiteSpace(cstId) Then
            sendXml.Detail.ReserveInformation.ReserveCustomerInformation.CstId = cstId
        End If
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'CUSTCD
        sendXml.Detail.ReserveInformation.ReserveCustomerInformation.CustCode = Nothing

        'CUSTOMERNAME
        sendXml.Detail.ReserveInformation.ReserveCustomerInformation.CustomerName = arg.CstName

        'CUSTOMERCLASS(1：所有者固定)
        sendXml.Detail.ReserveInformation.ReserveCustomerInformation.CustomerClass = OWNER

        'TELNO
        If String.IsNullOrEmpty(arg.Home.Trim()) Then

            sendXml.Detail.ReserveInformation.ReserveCustomerInformation.TelNo = Nothing
        Else

            sendXml.Detail.ReserveInformation.ReserveCustomerInformation.TelNo = arg.Home
        End If

        'MOBILE
        If String.IsNullOrEmpty(arg.Mobile.Trim()) Then

            sendXml.Detail.ReserveInformation.ReserveCustomerInformation.Mobile = Nothing
        Else

            sendXml.Detail.ReserveInformation.ReserveCustomerInformation.Mobile = arg.Mobile
        End If

        'EMAIL
        sendXml.Detail.ReserveInformation.ReserveCustomerInformation.Email = Nothing

        'ZIPCODE
        sendXml.Detail.ReserveInformation.ReserveCustomerInformation.ZipCode = Nothing

        'ADDRESS
        sendXml.Detail.ReserveInformation.ReserveCustomerInformation.Address = Nothing

        ''終了ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:RETURNCODE = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , sendXml))

        Return sendXml

    End Function

    ''' <summary>
    ''' XML作成(Reserve_VehicleInformationTag)
    ''' </summary>
    ''' <param name="sendXml">予約情報更新引数</param>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateReserveVehicleInformationTag(ByVal sendXml As XmlDocumentClass, _
                                                        ByVal arg As CallBackArgumentClass) As XmlDocumentClass

        ''開始ログの出力
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        '車両ID
        Dim vclId As String = arg.VclId.ToString(CultureInfo.InvariantCulture)
        If Not String.IsNullOrWhiteSpace(vclId) Then
            sendXml.Detail.ReserveInformation.ReserveVehicleInformation.VclId = vclId
        End If
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'VCLREGNO
        If String.IsNullOrEmpty(arg.RegNo.Trim()) Then

            sendXml.Detail.ReserveInformation.ReserveVehicleInformation.VehicleNo = Nothing
        Else

            sendXml.Detail.ReserveInformation.ReserveVehicleInformation.VehicleNo = arg.RegNo
        End If

        'VIN
        If String.IsNullOrEmpty(arg.Vin.Trim()) Then

            sendXml.Detail.ReserveInformation.ReserveVehicleInformation.Vin = Nothing
        Else

            sendXml.Detail.ReserveInformation.ReserveVehicleInformation.Vin = arg.Vin
        End If

        'MAKERCD
        sendXml.Detail.ReserveInformation.ReserveVehicleInformation.MakerCode = Nothing

        'SERIESCD
        sendXml.Detail.ReserveInformation.ReserveVehicleInformation.SeriesCode = Nothing

        'SERIESNM
        If String.IsNullOrEmpty(arg.Vehicle.Trim()) Then

            sendXml.Detail.ReserveInformation.ReserveVehicleInformation.SeriesName = HYPHEN
        Else

            sendXml.Detail.ReserveInformation.ReserveVehicleInformation.SeriesName = arg.Vehicle
        End If

        'BASETYPE
        sendXml.Detail.ReserveInformation.ReserveVehicleInformation.BaseType = Nothing

        'MILEAGE
        sendXml.Detail.ReserveInformation.ReserveVehicleInformation.Mileage = Nothing

        ''終了ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:RETURNCODE = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , sendXml))

        Return sendXml

    End Function

    ''' <summary>
    ''' XML作成(Reserve_ServiceInformationTag)
    ''' </summary>
    ''' <param name="sendXml">予約情報更新引数</param>
    ''' <param name="arg">引数クラスオブジェクト</param>
    ''' <param name="startPlanTime">更新用の作業開始予定日時</param>
    ''' <param name="finishPlanTime">更新用の作業完了予定日時</param>
    ''' <param name="svcClassCD">更新用のサービス分類コード</param>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateReserveServiceInformationTag(ByVal sendXml As XmlDocumentClass, _
                                                        ByVal arg As CallBackArgumentClass, _
                                                        ByVal startPlanTime As Date, _
                                                        ByVal finishPlanTime As Date, _
                                                        ByVal svcClassCD As String) As XmlDocumentClass

        ''開始ログの出力
        'OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '受付エリアの場合
        If (SUBAREA_RECEPTION.Equals(arg.SubAreaId)) Then

            'STALLID
            sendXml.Detail.ReserveInformation.ReserveServiceInformation.StallId = arg.SubStallId

            'STARTTIME
            sendXml.Detail.ReserveInformation.ReserveServiceInformation.StartTime = arg.SubStartPlanTime

            'ENDTIME
            sendXml.Detail.ReserveInformation.ReserveServiceInformation.EndTime = arg.SubFinishPlanTime

            'WORKTIME
            sendXml.Detail.ReserveInformation.ReserveServiceInformation.WorkTime = arg.SubPlanWorkTime

            'BREAKFLG
            If ("0").Equals(arg.SubRestFlg) Then
                sendXml.Detail.ReserveInformation.ReserveServiceInformation.BreakFlg = "1"
            Else
                sendXml.Detail.ReserveInformation.ReserveServiceInformation.BreakFlg = "0"
            End If

        Else
            'STALLID
            sendXml.Detail.ReserveInformation.ReserveServiceInformation.StallId = arg.StallId

            'STARTTIME
            sendXml.Detail.ReserveInformation.ReserveServiceInformation.StartTime = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", startPlanTime)

            'ENDTIME
            sendXml.Detail.ReserveInformation.ReserveServiceInformation.EndTime = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", finishPlanTime)

            'WORKTIME
            sendXml.Detail.ReserveInformation.ReserveServiceInformation.WorkTime = arg.PlanWorkTime

            'BREAKFLG
            If arg.RestFlg = 0 Then
                sendXml.Detail.ReserveInformation.ReserveServiceInformation.BreakFlg = "1"
            Else
                sendXml.Detail.ReserveInformation.ReserveServiceInformation.BreakFlg = "0"
            End If
        End If

        'WASHFLG
        sendXml.Detail.ReserveInformation.ReserveServiceInformation.WashFlg = arg.CarWashFlg

        'INSPECTIONFLG
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'sendXml.Detail.ReserveInformation.ReserveServiceInformation.InspectionFlg = Nothing
        sendXml.Detail.ReserveInformation.ReserveServiceInformation.InspectionFlg = arg.CompleteExaminationFlg
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'MERCHANDISECD
        sendXml.Detail.ReserveInformation.ReserveServiceInformation.MerchandiseCode = CType(arg.MercId, String)

        'MNTNCD
        sendXml.Detail.ReserveInformation.ReserveServiceInformation.MntnCode = Nothing

        'SERVICECODE
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'If String.IsNullOrEmpty(svcClassCD) Then

        '    '受付エリアの場合
        '    If (arg.SubAreaId.Equals(SUBAREA_RECEPTION)) Then
        '        sendXml.Detail.ReserveInformation.ReserveServiceInformation.ServiceCode = Nothing
        '    Else
        '        sendXml.Detail.ReserveInformation.ReserveServiceInformation.ServiceCode = "0"
        '    End If
        'Else

        '    sendXml.Detail.ReserveInformation.ReserveServiceInformation.ServiceCode = svcClassCD
        'End If
        If Not String.IsNullOrEmpty(svcClassCD) Then
            sendXml.Detail.ReserveInformation.ReserveServiceInformation.ServiceCode = svcClassCD
        Else
            sendXml.Detail.ReserveInformation.ReserveServiceInformation.ServiceCode = "0"
        End If
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'REZ_RECEPTION
        sendXml.Detail.ReserveInformation.ReserveServiceInformation.ReserveReception = arg.WaitingFlg

        'REZ_PICK_DATE       
        If String.IsNullOrEmpty(arg.VisitPlanTime) OrElse arg.VisitPlanTime.Equals(DATE_MIN_VALUE) Then

            sendXml.Detail.ReserveInformation.ReserveServiceInformation.ReservePickDate = DATE_MIN_VALUE
        Else

            sendXml.Detail.ReserveInformation.ReserveServiceInformation.ReservePickDate = arg.VisitPlanTime
        End If

        'REZ_PICK_LOC
        sendXml.Detail.ReserveInformation.ReserveServiceInformation.ReservePickLoc = Nothing

        'REZ_PICK_TIME
        sendXml.Detail.ReserveInformation.ReserveServiceInformation.ReservePickTime = Nothing

        'REZ_DELI_DATE
        If String.IsNullOrEmpty(arg.DeriveredPlanTime) OrElse arg.DeriveredPlanTime.Equals(DATE_MIN_VALUE) Then

            sendXml.Detail.ReserveInformation.ReserveServiceInformation.ReserveDeliDate = DATE_MIN_VALUE
        Else

            sendXml.Detail.ReserveInformation.ReserveServiceInformation.ReserveDeliDate = arg.DeriveredPlanTime
        End If

        'REZ_DELI_LOC
        sendXml.Detail.ReserveInformation.ReserveServiceInformation.ReserveDeliLoc = Nothing

        'REZ_DELI_TIME
        sendXml.Detail.ReserveInformation.ReserveServiceInformation.ReserveDeliTime = Nothing

        ''終了ログの出力
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:RETURNCODE = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , sendXml))

        Return sendXml

    End Function

#End Region

    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START

#Region "事前処理"

    ''' <summary>
    ''' ストールロック処理
    ''' </summary>
    ''' <param name="inClsTabletSMBCommonClass">TabletSMB用共通関数のビジネスロジッククラスインスタンス</param>
    ''' <param name="inSubAreaId">サブチップボックスID</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inChipDisplayStartDate">チップ表示日時</param>
    ''' <param name="inUpdateAccount">更新アカウント</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <returns>TabletSMB用共通関数.LockStallメソッドの戻り値</returns>
    ''' <remarks>
    ''' TabletSMB用共通関数.LockStallメソッドを呼び出してストールロック処理を実施する
    ''' </remarks>
    Private Function LockStall(ByVal inClsTabletSMBCommonClass As TabletSMBCommonClassBusinessLogic, _
                               ByVal inSubAreaId As String, _
                               ByVal inStallId As Decimal, _
                               ByVal inChipDisplayStartDate As Date, _
                               ByVal inUpdateAccount As String, _
                               ByVal inUpdateDate As Date) As Long

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim resultCode As Long = ActionResult.Success

        '受付・追加作業・NoShow・中断エリア以外の場合
        If Not (SUBAREA_RECEPTION.Equals(inSubAreaId) _
         OrElse SUBAREA_ADDWORK.Equals(inSubAreaId) _
         OrElse SUBAREA_NOSHOW.Equals(inSubAreaId) _
         OrElse SUBAREA_STOP.Equals(inSubAreaId)) Then

            'ストールロック
            resultCode = inClsTabletSMBCommonClass.LockStall(inStallId, _
                                                             inChipDisplayStartDate, _
                                                             inUpdateAccount, _
                                                             inUpdateDate, _
                                                             MY_PROGRAMID)

            If resultCode = ActionResult.Success Then
                'ストールロック処理が成功した場合

                'ストールロックフラグをTrue(ストールロック実施)に設定する
                IsLockStall = True

            Else
                'ストールロック処理が失敗した場合

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1}_Error resultCode={2}" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , resultCode))
            End If

        End If

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        Return resultCode

    End Function

#End Region

    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

#Region "ログ出力メソッド"

    ''' <summary>
    ''' 引数のないInfoレベルのログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="isStart">True:Startログ/False:Endログ</param>
    ''' <remarks></remarks>
    Private Sub OutputInfoLog(ByVal method As String, ByVal isStart As Boolean)

        If isStart Then
            Logger.Info(MY_PROGRAMID & ".ascx " & method & "_Start")
        Else
            Logger.Info(MY_PROGRAMID & ".ascx " & method & "_End")
        End If

    End Sub

    ''' <summary>
    ''' 引数のあるInfoレベルのログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="isStart">True:Startログ/False:Endログ</param>
    ''' <param name="argString">フォーマット用文字列</param>
    ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ''' <remarks></remarks>
    Private Sub OutputInfoLog(ByVal method As String, ByVal isStart As Boolean, ByVal argString As String, ByVal ParamArray args() As Object)

        Dim logString As String = String.Empty

        If isStart Then
            logString = MY_PROGRAMID & ".ascx " & method & "_Start" & argString
            Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
        Else
            logString = MY_PROGRAMID & ".ascx " & method & "_End" & argString
            Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
        End If

    End Sub

    ' ''' <summary>
    ' ''' エラーログを出力する
    ' ''' </summary>
    ' ''' <param name="method">メソッド名</param>
    ' ''' <param name="ex">例外オブジェクト</param>
    ' ''' <param name="argString">フォーマット用文字列</param>
    ' ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ' ''' <remarks></remarks>
    'Private Sub OutputErrLog(ByVal method As String, ByVal ex As Exception, ByVal argString As String, ParamArray args() As Object)

    '    Dim logString As String = String.Empty

    '    logString = MY_PROGRAMID & ".ascx " & method & "_Error" & argString
    '    Logger.Error(String.Format(CultureInfo.InvariantCulture, logString, args), ex)

    'End Sub

    ' ''' <summary>
    ' ''' ログ出力(IF戻り値用)
    ' ''' </summary>
    ' ''' <param name="dt">戻り値(DataTable)</param>
    ' ''' <param name="ifName">使用IF名</param>
    ' ''' <remarks></remarks>
    'Private Sub OutPutIFLog(ByVal dt As DataTable, ByVal ifName As String)

    '    If dt Is Nothing Then
    '        Return
    '    End If

    '    Logger.Info(MY_PROGRAMID & ".ascx " & ifName + " Result START " + " OutPutCount: " + (dt.Rows.Count).ToString(CultureInfo.InvariantCulture))

    '    Dim log As New Text.StringBuilder

    '    For j = 0 To dt.Rows.Count - 1

    '        log = New Text.StringBuilder()
    '        Dim dr As DataRow = dt.Rows(j)

    '        log.Append("RowNum: " + (j + 1).ToString(CultureInfo.InvariantCulture) + " -- ")

    '        For i = 0 To dt.Columns.Count - 1
    '            log.Append(dt.Columns(i).Caption)
    '            If IsDBNull(dr(i)) Then
    '                log.Append(" IS NULL")
    '            Else
    '                log.Append(" = ")
    '                log.Append(dr(i).ToString)
    '            End If

    '            If i <= dt.Columns.Count - 2 Then
    '                log.Append(", ")
    '            End If
    '        Next

    '        Logger.Info(log.ToString)
    '    Next

    '    Logger.Info(MY_PROGRAMID & ".ascx " & ifName + " Result END ")

    'End Sub
#End Region

#End Region

#Region "チップ詳細の日付・時間項目データクラス"
    ''' <summary>
    ''' チップ詳細の日付・時間項目データクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChipDetailDateTimeClass

        Private _detailStartPlanTime As Date
        Private _detailFinishPlanTime As Date
        Private _detailprmsEndTime As Date
        Private _detailprocTime As Long

        ''' <summary>
        ''' 予定開始日時
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DetailStartPlanTime As Date
            Get
                Return _detailStartPlanTime
            End Get
            Set(ByVal value As Date)
                _detailStartPlanTime = value
            End Set
        End Property

        ''' <summary>
        ''' 予定終了日時
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DetailFinishPlanTime As Date
            Get
                Return _detailFinishPlanTime
            End Get
            Set(ByVal value As Date)
                _detailFinishPlanTime = value
            End Set
        End Property

        ''' <summary>
        ''' 見込終了日時
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DetailprmsEndTime As Date
            Get
                Return _detailprmsEndTime
            End Get
            Set(ByVal value As Date)
                _detailprmsEndTime = value
            End Set
        End Property

        ''' <summary>
        ''' 実績時間
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DetailprocTime As Long
            Get
                Return _detailprocTime
            End Get
            Set(ByVal value As Long)
                _detailprocTime = value
            End Set
        End Property
    End Class
#End Region

End Class
