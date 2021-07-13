'-------------------------------------------------------------------------
'SC3180201BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：完成検査承認画面(ビジネスロジック)
'補足：
'作成：2014/02/25 AZ宮澤	初版作成
'更新：2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'─────────────────────────────────────

Option Explicit On
Imports System.IO
Imports System.Text
Imports System.Globalization
Imports System.Xml
Imports System.Xml.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Imports Toyota.eCRB.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.DataAccess.SC3180201.SC3180201DataSet
Imports Toyota.eCRB.iCROP.DataAccess.SC3180201.SC3180201DataSetTableAdapter.SC3180201TableAdapter
Imports Toyota.eCRB.iCROP.DataAccess.SC3180201.SC3180201DataSetTableAdapter

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode

Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess

Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.DMSLinkage.StatusInfo.Api.BizLogic
Imports Toyota.eCRB.iCROP.DataAccess.SC3180201

''' <summary>
''' チェックシートプレビュービジネスクラス
''' </summary>
''' <remarks></remarks>
Public Class SC3180201BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    Private Const StallUseStatusCompletion As String = "03"                     ' ストール利用ステータス(完了）

    Private Const AcceptanceTypeWalkin As String = "0"                          ' 受付区分（WalkIN）

    Private Const UnsetJobDtlId As Long = -1                                    ' 現在のJobDtlID（未設定時）

    Private Const EventkeyCommonProces As String = "100"                        ' イベントキー（共通処理）
    '2014/05/14 通知&PUSH処理追加　START　↓↓↓
    Private Const EventkeyApproveProces As String = "200"                       ' イベントキー（検査承認処理）
    '2014/05/14 通知&PUSH処理追加　END　　↑↑↑
    Private Const EventkeyLastApproveProces As String = "201"                   ' イベントキー（最終検査承認処理）
    Private Const EventkeyRejectProces As String = "300"                        ' イベントキー（検査否認処理）

    Private Const RoStatusWork As Long = 60                                     ' RO_STATUS（作業中）
    Private Const RoStatusCompExaminationRequest As Long = 65                   ' RO_STATUS（完成検査依頼中）
    Private Const RoStatusCompExaminationComplate As Long = 70                  ' RO_STATUS（完成検査完了）
    Private Const RoStatusDeliveryWait As Long = 80                             ' RO_STATUS（納車準備待ち）
    Private Const RoStatusDeliveryWork As Long = 85                             ' RO_STATUS（納車作業中）

    Private Const RoStatusProcDeliveryWait As Long = 1                          ' RO_STATUS（納車準備待ち以前）
    Private Const RoStatusProcAfterDeliveryWork As Long = 2                     ' RO_STATUS（納車作業中以降）

    Private Const DefaultItemCD As String = "                    "              ' ItemCD未設定値
    Private Const DefaultJobInspectId As String = "0"                           ' JobInstructID未設定値
    Private Const DefaultJobInspectSeq As Long = 0                              ' JobInstructSeq未設定値
    Private Const DefaultAlreadyReplace As Long = 0                             ' Replaced選択状態（未選択）
    Private Const DefaultAlreadyFix As Long = 0                                 ' Fixed選択状態（未選択）
    Private Const DefaultAlreadyClean As Long = 0                               ' Cleaned選択状態（未選択）
    Private Const DefaultAlreadySwap As Long = 0                                ' Swapped選択状態（未選択）
    Private Const DefaultBeforeText As Long = 0                                 ' Before入力内容（未入力値）
    Private Const DefaultAfterText As Long = 0                                  ' After入力内容（未入力値）

    Private Const LastChipFlag As String = "1"                                  ' 最終チップフラグ（最終チップ）
    Private Const DispatchUseFlg As String = "1"                                ' JobDispatch運用フラグ（使用）

    Private Const InspectionUpdateApprove As Long = 3                           '作業内容ステータス更新内容判定フラグ
    Private Const InspectionUpdateReject As Long = 4                            '作業内容ステータス更新内容判定フラグ

    Private Const AllDealerCode As String = "XXXXX"                             ' 全販売店を意味するワイルドカード販売店コード
    Private Const AllBranchCode As String = "XXX"                               ' 全店舗を意味するワイルドカード店舗コード

    Private Const DefaultAlreadyReplaceInt As Integer = 0                       ' Replaced選択状態（未選択）
    Private Const InspecResltCodeReplaceInt As Integer = 3                      ' 点検結果Replaced
    Private Const DefaultPreviousReplaceMile As Decimal = -1                    ' 交換走行距離初期値
    Private Const FormatDbDateTime As String = "1900/01/01"                     ' 前回部品交換情報.前回交換日時初期値(年月日)


    ''' <summary>点検項目表示順のソートキー</summary>
    Private Const SortKey_Inspec As String = "INSPEC_ITEM_DISP_SEQ, JOB_DTL_ID, DAIHYO_SEIBI DESC, JOB_INSTRUCT_ID, JOB_INSTRUCT_SEQ "

    ''' <summary>
    ''' 検査部位コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PartCdEngine As String = "01"
    Private Const PartCdInRoom As String = "02"
    Private Const PartCdLeft As String = "03"
    Private Const PartCdRight As String = "04"
    Private Const PartCdUnder As String = "05"
    Private Const PartCdTrunk As String = "06"
    Private Const PartCdNone As String = ""

    '共通関数の戻り値にて、継続する値
    '0:正常終了
    '-9000:ワーニング
    Private arySuccessList() As Long = {0, -9000}

#Region "通知用定数"

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
    ''' 自社客のリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MyPageLink As String = "<a id='SC3180201' Class='SC3180201' href='/Website/Pages/SC3180201.aspx' onclick='return ServiceLinkClick(event)'>"

    '2014/05/13 通知&PUSH処理追加　START　↓↓↓

    ''' <summary>
    ''' R/O番号のリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RoPreviewLink As String = "<a id='{0}0' Class='{0}' href='/Website/Pages/{0}.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' 車両番号のリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerVclLink As String = "<a id='{0}1' Class='{0}' href='/Website/Pages/{0}.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' 顧客名のリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerCstLink As String = "<a id='{0}2' Class='{0}' href='/Website/Pages/{0}.aspx' onclick='return ServiceLinkClick(event)'>"
    '2014/05/13 通知&PUSH処理追加　END　↑↑↑

    ''' <summary>
    ''' Aタグ終了文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EndLikTag As String = "</a>"

    ''' <summary>
    ''' 敬称利用区分("1"：後方)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeBack As String = "1"

    ''' <summary>
    ''' 敬称利用区分("2"：前方)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeFront As String = "2"

    ''' <summary>
    ''' イベントキーID
    ''' </summary>
    Private Enum EventKeyId

        ''' <summary>
        ''' 共通処理
        ''' </summary>
        CommonProces = 100

        ''' <summary>
        ''' 承認処理
        ''' </summary>
        AproveProces = 200

        ''' <summary>
        ''' 最終承認処理
        ''' </summary>
        LastAproveProces = 201

        ''' <summary>
        ''' 否認処理
        ''' </summary>
        RejectProces = 300

    End Enum

    ''' <summary>
    ''' メッセージID管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MsgID
        ''' <summary>通知用文言("50"：完成検査承認)</summary>
        id50 = 50
        ''' <summary>通知用文言("51"：精算準備)</summary>
        id51 = 51
        ''' <summary>通知用文言("52"：完成検査否認)</summary>
        id52 = 52
    End Enum

    ''' <summary>
    ''' 画面遷移セッションキー(DMS販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyDealerCode As String = "SessionKey.DealerCode,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(DMS店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyBranchCode As String = "SessionKey.BranchCode,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(アカウント)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyAccount As String = "SessionKey.LoginUserID,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(来店実績連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyVistSequence As String = "SessionKey.SAChipID,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(DMS予約ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyResrveId As String = "SessionKey.BASREZID,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyRepairorder As String = "SessionKey.R_O,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(RO作業連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeySequence As String = "SessionKey.SEQ_NO,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyJobDtlId As String = "SessionKey.JOB_DTL_ID,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyVin As String = "SessionKey.VIN,String,"
    ''' <summary>
    ''' 画面遷移セッションキー(編集モード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyViewMode As String = "SessionKey.ViewMode,String,"

    '2014/06/24　セッション情報作成処理変更　START　↓↓↓
    ''' <summary>
    ''' 通知履歴のSessionValue(DMS販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDearlerCode As String = "Session.Param1,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(DMS店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueBranchCode As String = "Session.Param2,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(アカウント)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueLoginUserID As String = "Session.Param3,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(来店実績連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSAChipID As String = "Session.Param4,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(DMS予約ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueBASREZID As String = "Session.Param5,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueR_O As String = "Session.Param6,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(RO作業連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSEQ_NO As String = "Session.Param7,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueVIN_NO As String = "Session.Param8,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueViewMode As String = "Session.Param9,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(フォーマット「0：プレビュー」固定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueFormat As String = "Session.Param10,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(入庫管理番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSvcInNum As String = "Session.Param11,String,"

    ''' <summary>
    '''  通知履歴のSessionValue(入庫販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSvcInDlrCd As String = "Session.Param12,String,"

    ''' <summary>
    '''  通知履歴のSessionValue(入庫店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueSvcInBrnCd As String = "Session.Param13,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(「5：R/O参照」固定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDisp_Num As String = "Session.DISP_NUM,String,"

    ''' <summary>
    ''' 通知履歴のSessionValue(顧客のDMSID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueDmsCstId As String = "SessionKey.DMS_CST_ID,String,"


    '2014/06/24　セッション情報作成処理変更　END　　↑↑↑


#End Region

    '2014/05/16 通知&PUSH処理追加　START　↓↓↓
#Region "遷移先画面取得Dictionary用定数"

    ''' <summary>
    ''' 文言DB番号キー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyWordNo As String = "WordNo"

    ''' <summary>
    ''' R/O番号URLキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyRoNoURL As String = "RoNoURL"

    ''' <summary>
    ''' 車両番号URLキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyRegNoURL As String = "RegNoURL"

    ''' <summary>
    ''' お客様名URLキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyCutomerURL As String = "CutomerURL"

    ''' <summary>
    ''' 商品名URLキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyMerchandiseURL As String = "MerchandiseURL"

    ''' <summary>
    ''' 画面リフレッシュキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicKeyPushMethod As String = "PushMethod"

    ''' <summary>
    ''' SMB画面リフレッシュメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSMB As String = "RefreshSMB()"

    ''' <summary>
    ''' FMメイン画面リフレッシュメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshFMMain As String = "MainRefresh()"
    '2014/11/26 起票SA個人へのPush通知追加　START ↓↓↓
    ''' <summary>
    ''' SAメイン画面リフレッシュメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSAMain As String = "MainRefresh()"
    '2014/11/26 起票SA個人へのPush通知追加　END   ↑↑↑
    ''' <summary>
    ''' ページID：SC3010501
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PageIdSC3010501 As String = "SC3010501"

    ''' <summary>
    ''' ページID：SC3010501　DispNum：13
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PageIdSC3010501_13 As String = "SC3010501-13"

    ''' <summary>
    ''' ページID：SC3080225
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PageIdSC3080225 As String = "SC3080225"

    ''' <summary>
    ''' 値無し
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransDicNone As String = ""

#End Region

#Region "PUSH処理用メッセージID"

    ''' <summary>
    ''' 正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As Integer = 0

    ''' <summary>
    ''' スタッフ情報が0件
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdAccountInfoIsNull As Integer = 1101

    ''' <summary>
    ''' 販売店コードに該当するマスタデータが存在しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdDealerInfoIsNull As Integer = 1102

    ''' <summary>
    ''' 店舗コードに該当するマスタデータが存在しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdBranchInfoIsNull As Integer = 1103

    ''' <summary>
    ''' Push送信に失敗
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdPushSendFailed As Integer = 6001

    ''' <summary>
    ''' システムエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSystemError As Integer = 9999

#End Region
    '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

    '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
    ''' <summary>
    ''' 型式使用フラグ取得用の[TB_M_SYSTEM_SETTING].[SETTING_NAME]値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SysParaNameKatashikiUseFlg As String = "USE_FLG_KATASHIKI"

    ''' <summary>
    ''' マスタ登録状態フラグ（登録なし）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ExistFlag As String = "0"

    ''' <summary>
    ''' システム設定不備エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorSystemSetting As String = "1101"
    '2019/12/02 NCN 吉川 TKM要件：型式対応 End
#End Region


#Region "メインロジック"

    ''' <summary>
    ''' ヘッダー情報取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <returns>ヘッダー情報</returns>
    ''' <remarks></remarks>
    Public Function GetHederInfo(ByVal dlrCD As String, _
                                  ByVal brnCD As String, _
                                  ByVal roNum As String) As SC3180201HederInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索処理
        Dim tableAdapter As New SC3180201TableAdapter
        Dim dtHeaderInfo As SC3180201HederInfoDataTable

        dtHeaderInfo = tableAdapter.GetDBHederInfo(dlrCD, brnCD, roNum)

        '2019/06/10 ジョブ名複数時対応 start
        If 1 < dtHeaderInfo.Count Then

            ' DBNullを除いたTrim後のジョブ名を取得
            Dim query = (From s In dtHeaderInfo.AsEnumerable _
                         Where Not s.IsSVC_CLASS_NAMENull() _
                         Select s.SVC_CLASS_NAME.Trim()).ToList()

            ' 重複削除（※大文字小文字違い、末尾スペース等は考慮しない）
            Dim names As IEnumerable(Of String) = query.Distinct()
            ' Null Or Empty削除
            names = (From s In names Where Not String.IsNullOrEmpty(s)).ToList()

            ' １件目を書き換える（※クライアント側で１件目のみを使用）
            dtHeaderInfo(0).SVC_CLASS_NAME = String.Join(" / ", names)

            ' クリア
            query = Nothing
            names = Nothing

        End If
        '2019/06/10 ジョブ名複数時対応 end

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return dtHeaderInfo

    End Function

    ''' <summary>
    ''' OperationCodeList(Inspection)情報取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <returns>OperationCode情報</returns>
    ''' <remarks></remarks>
    Public Function GetInspecCodeList(ByVal dlrCD As String, _
                                     ByVal brnCD As String, _
                                     ByVal roNum As String) As SC3180201InspecCodeListDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索処理
        Dim tableAdapter As New SC3180201TableAdapter
        Dim dtInspectCodeList As SC3180201InspecCodeListDataTable

        dtInspectCodeList = tableAdapter.GetDBInspecCodeList(dlrCD, brnCD, roNum)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return dtInspectCodeList

    End Function

    ''' <summary>
    ''' OperationCodeList(Maintenance)情報取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="specifyDlrCdFlgs">全販売店検索フラグセット</param>
    ''' <returns>OperationCode情報</returns>
    ''' <remarks></remarks>
    Public Function GetMainteCodeList(ByVal dlrCD As String, _
                                     ByVal brnCD As String, _
                                     ByVal roNum As String, _
                                     ByVal specifyDlrCdFlgs As Dictionary(Of String, Boolean)) As SC3180201MainteCodeListDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索処理
        Dim tableAdapter As New SC3180201TableAdapter
        Dim dtMainteCodeList As SC3180201MainteCodeListDataTable

        '2019/12/02 NCN吉川 TKM要件：型式対応 Start
        'dtMainteCodeList = tableAdapter.GetDBMainteCodeList(dlrCD, brnCD, roNum)
        dtMainteCodeList = tableAdapter.GetDBMainteCodeList(dlrCD, brnCD, roNum, specifyDlrCdFlgs)

        '型式使用で値が取得できていない場合
        If dtMainteCodeList.Rows.Count = 0 AndAlso specifyDlrCdFlgs("KATASHIKI_EXIST") = True Then
            'モデル使用で再取得
            specifyDlrCdFlgs("KATASHIKI_EXIST") = False
            dtMainteCodeList = tableAdapter.GetDBMainteCodeList(dlrCD, brnCD, roNum, specifyDlrCdFlgs)
        End If
        '2019/12/02 NCN吉川 TKM要件：型式対応 End
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return dtMainteCodeList

    End Function

    '2015/04/14 新販売店追加対応 start

    ''' <summary>
    ''' OperationItemsList(Inspection)情報取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="specifyDlrCdFlgs">全販売店検索フラグセット</param>
    ''' <param name="roStatus">ROStauts</param>
    ''' <returns>OperationItems情報</returns>
    ''' <remarks></remarks>
    Public Function GetAllInspecCode(ByVal dlrCD As String, _
                                     ByVal brnCD As String, _
                                     ByVal roNum As String, _
                                     ByVal specifyDlrCdFlgs As Dictionary(Of String, Boolean), _
                                     ByRef roStatus As String) As SC3180201InspectCodeDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180201TableAdapter
        Dim dtInspecCode As SC3180201InspectCodeDataTable
        Dim dtMainteCodeList As SC3180201MainteCodeListDataTable
        Dim strInspecItemCD As String = ""
        Dim intIdx As Integer = 0
        roStatus = ""

        '検索処理(チェック項目)
        'dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, PartCdNone)
        '2019/12/02 NCN吉川 TKM要件：型式対応 Start　
        'dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, specifyDlrCdFlgs)
        dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, specifyDlrCdFlgs)

        '自販売店で値が取得できていない場合　
        If dtInspecCode.Rows.Count = 0 AndAlso specifyDlrCdFlgs("COMB_DLR_AND_BRN_EXIST") = True Then
            specifyDlrCdFlgs("COMB_DLR_AND_BRN_EXIST") = False
            dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, specifyDlrCdFlgs)
        End If

        If dtInspecCode.Rows.Count = 0 AndAlso specifyDlrCdFlgs("KATASHIKI_EXIST") = True Then
            'モデルコードで検索する
            specifyDlrCdFlgs("KATASHIKI_EXIST") = False
        dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, specifyDlrCdFlgs)
        End If
        '2019/12/02 NCN吉川 TKM要件：型式対応 End
        '2015/04/14 新販売店追加対応 end

        ''ROステータス取得のためにJobDtlIDを取得
        Dim jobDtlId As String = ""
        If 0 < dtInspecCode.Count Then
            For idx = 0 To dtInspecCode.Count - 1
                If False = dtInspecCode(0).IsNull("JOB_DTL_ID") Then
                    jobDtlId = dtInspecCode(0).JOB_DTL_ID.ToString.Trim
                End If
                If "" <> jobDtlId Then
                    Exit For
                End If
            Next
        End If
        ''JobDtlIDを条件にROステータスを取得
        If "" <> jobDtlId Then
            Dim dtRoState As SC3180201RoStateDataTable
            dtRoState = GetDBRoState(jobDtlId)
            If 0 < dtRoState.Count Then
                If False = dtRoState(0).IsNull("RO_STATUS") Then
                    roStatus = dtRoState(0).RO_STATUS.ToString.Trim
                End If
            End If
        End If

        If "" = roStatus Then
            '2019/12/02 NCN吉川 TKM要件：型式対応 Start
            '検索処理(メンテナンス)
            'dtMainteCodeList = tableAdapter.GetDBMainteCodeList(dlrCD, brnCD, roNum, specifyDlrCdFlgs)
            dtMainteCodeList = tableAdapter.GetDBMainteCodeList(dlrCD, brnCD, roNum, specifyDlrCdFlgs)
            If dtMainteCodeList.Rows.Count = 0 AndAlso specifyDlrCdFlgs("KATASHIKI_EXIST") = True Then
                'モデル使用で再取得
                specifyDlrCdFlgs("KATASHIKI_EXIST") = False
                dtMainteCodeList = tableAdapter.GetDBMainteCodeList(dlrCD, brnCD, roNum, specifyDlrCdFlgs)
            End If
            '2019/12/02 NCN吉川 TKM要件：型式対応 End　
            ''ROステータス取得のためにJobDtlIDを取得
            jobDtlId = ""
            If 0 < dtMainteCodeList.Count Then
                For idx = 0 To dtMainteCodeList.Count - 1
                    If False = dtMainteCodeList(0).IsNull("JOB_DTL_ID") Then
                        jobDtlId = dtMainteCodeList(0).JOB_DTL_ID.ToString.Trim
                    End If
                    If "" <> jobDtlId Then
                        Exit For
                    End If
                Next
            End If
            ''JobDtlIDを条件にROステータスを取得
            If "" <> jobDtlId Then
                Dim dtRoState As SC3180201RoStateDataTable
                dtRoState = GetDBRoState(jobDtlId)
                If 0 < dtRoState.Count Then
                    If False = dtRoState(0).IsNull("RO_STATUS") Then
                        roStatus = dtRoState(0).RO_STATUS.ToString.Trim
                    End If
                End If
            End If
        End If

        ''2016/09/23 GetInspecCodeの処理を追加　Start
        '重複要素の削除
        If 0 < dtInspecCode.Count Then
            strInspecItemCD = dtInspecCode(intIdx).INSPEC_ITEM_CD.ToString.Trim()
            intIdx += 1
            Do While dtInspecCode.Count > intIdx
                If strInspecItemCD = dtInspecCode(intIdx).INSPEC_ITEM_CD.ToString.Trim() Then
                    dtInspecCode.RemoveSC3180201InspectCodeRow(dtInspecCode(intIdx))
                Else
                    strInspecItemCD = dtInspecCode(intIdx).INSPEC_ITEM_CD.ToString.Trim()
                    intIdx += 1
                End If
            Loop
        End If

        Dim rtn As SC3180201InspectCodeDataTable = New SC3180201InspectCodeDataTable()

        Using dv As New DataView(dtInspecCode)
            dv.Sort = SortKey_Inspec
            rtn.Merge(dv.ToTable())

            dtInspecCode.Clear()
            dtInspecCode.Dispose()
            dtInspecCode = Nothing
        End Using
        ''2016/09/23 GetInspecCodeの処理を追加　End

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return rtn

    End Function
    ''2015/04/14 新販売店追加対応 start

    ' ''' <summary>
    ' ''' OperationItemsList(Inspection)情報取得
    ' ''' </summary>
    ' ''' <param name="dlrCD">販売店コード</param>
    ' ''' <param name="brnCD">店舗コード</param>
    ' ''' <param name="roNum">RO番号</param>
    ' ''' <param name="specifyDlrCdFlgs">全販売店検索フラグセット</param>
    ' ''' <param name="partCD">部位コード</param>
    ' ''' <returns>OperationItems情報</returns>
    ' ''' <remarks></remarks>
    'Public Function GetInspecCode(ByVal dlrCD As String, _
    '                              ByVal brnCD As String, _
    '                              ByVal roNum As String, _
    '                              ByVal specifyDlrCdFlgs As DataTable, _
    '                              Optional ByVal partCD As String = "") As SC3180201InspectCodeDataTable

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim tableAdapter As New SC3180201TableAdapter
    '    Dim dtInspecCode As SC3180201InspectCodeDataTable
    '    Dim strInspecItemCD As String = ""
    '    Dim intIdx As Integer = 0

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "★Debug:GetDBInspectCode Before"))
    '    '検索処理
    '    'dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, partCD)
    '    dtInspecCode = tableAdapter.GetDBInspectCode(dlrCD, brnCD, roNum, specifyDlrCdFlgs, partCD)
    '    '2015/04/14 新販売店追加対応 end
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "★Debug:GetDBInspectCode After:Count={0}" _
    '                , dtInspecCode.Count))

    '    '重複要素の削除
    '    If 0 < dtInspecCode.Count Then
    '        strInspecItemCD = dtInspecCode(intIdx).INSPEC_ITEM_CD.ToString.Trim()
    '        intIdx += 1
    '        Do While dtInspecCode.Count > intIdx
    '            If strInspecItemCD = dtInspecCode(intIdx).INSPEC_ITEM_CD.ToString.Trim() Then
    '                dtInspecCode.RemoveSC3180201InspectCodeRow(dtInspecCode(intIdx))
    '            Else
    '                strInspecItemCD = dtInspecCode(intIdx).INSPEC_ITEM_CD.ToString.Trim()
    '                intIdx += 1
    '            End If
    '        Loop
    '    End If

    '    ''2014/08/14 ソート条件変更　Start
    '    Dim rtn As SC3180201InspectCodeDataTable = New SC3180201InspectCodeDataTable()

    '    Using dv As New DataView(dtInspecCode)
    '        dv.Sort = SortKey_Inspec
    '        rtn.Merge(dv.ToTable())

    '        dtInspecCode.Clear()
    '        dtInspecCode.Dispose()
    '        dtInspecCode = Nothing
    '    End Using
    '    ''2014/08/14 ソート条件変更　End

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '処理結果返却
    '    Return rtn

    'End Function

    ''' <summary>
    ''' RoStatusCheck情報判断
    ''' </summary>
    ''' <param name="roStatus">ROステータス</param>
    ''' <returns>OperationCode情報</returns>
    ''' <remarks></remarks>
    Public Function RoStatusCheck(ByVal roStatus As Long) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim lngResult As Long = 0

        If RoStatusDeliveryWait >= Integer.Parse(roStatus) Then
            '納車より前(80:納車準備待ちまで):編集可(変更なし)
            lngResult = RoStatusProcDeliveryWait
        Else
            '納車より前(85:納車作業中以降):編集不可
            lngResult = RoStatusProcAfterDeliveryWork
        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "lngResult=[{0}]" _
        '            , lngResult))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END lngResult=[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , lngResult.ToString))

        '判断結果返却
        Return lngResult

    End Function

    ''' <summary>
    ''' 完成検査結果データ登録(Reject)
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="jobDtlId">JobDtlID</param>
    ''' <param name="saChipId">来店者実績連番</param>
    ''' <param name="basrezId">DMS予約ID</param>
    ''' <param name="seqNo">RO_JOB_SEQ</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="viewMode">ViewMode</param>
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strAdviceContent">アドバイス</param>
    ''' <param name="dtInspecItem">検査項目</param>
    ''' <param name="dtMaintenance">メンテナンス項目</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <param name="rtnGlobalResult">戻り値用共通関数実行結果値</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function RejectLogic(ByVal dealerCD As String, _
                                    ByVal branchCD As String, _
                                    ByVal roNum As String, _
                                    ByVal jobDtlId As String, _
                                    ByVal saChipId As String, _
                                    ByVal basrezId As String, _
                                    ByVal seqNo As String, _
                                    ByVal vin As String, _
                                    ByVal viewMode As String, _
                                    ByRef decNowJobDtlID As Decimal, _
                                    ByVal decServiceID As Decimal, _
                                    ByRef decStallID As Decimal, _
                                    ByVal strAdviceContent As String, _
                                    ByVal dtInspecItem As SC3180201RegistInfoDataTable, _
                                    ByVal dtMaintenance As SC3180201RegistInfoDataTable, _
                                    ByVal strAccount As String, _
                                    ByVal strApplicationID As String, _
                                    ByRef rtnGlobalResult As Long) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        'Dim blnGlobalResult As Boolean = True
        Dim dicJobDtlID As New Dictionary(Of String, Long)()
        Dim strServiceINAdviceContent As String
        Dim dtfUpdate As Date = DateTimeFunc.Now(dealerCD)

        Dim objStaffContext As StaffContext = StaffContext.Current

        'TMT2販社 BTS310 新規登録時の例外処理追加 横展開修正 2015/04/06 start
        Try
            '2014/05/23 グローバル連携処理修正　START　↓↓↓
            '前ステータス取得 2014/05/08
            Dim prevStatus As String = JudgeChipStatus(decStallID)
            Dim prevJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable = Nothing
            prevJobStatus = JudgeJobStatus(jobDtlId)
            '2014/05/23 グローバル連携処理修正　　END　↑↑↑

            '画面データ登録
            blnResult = RegistDispData(dealerCD, _
                                       branchCD, _
                                       roNum, _
                                       strAdviceContent, _
                                       dtInspecItem, _
                                       dtMaintenance,
                                       strAccount, _
                                       dtfUpdate, _
                                       InspectionUpdateReject, _
                                       vin)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "Stall_use_id [{0}]: JOB_DTL_ID [{1}]" _
                                      , decStallID _
                                      , decNowJobDtlID))

            If True = blnResult Then
                strServiceINAdviceContent = strAdviceContent
                If String.Empty = strServiceINAdviceContent Then
                    strServiceINAdviceContent = " "
                End If

                blnResult = SetDBServiceINAdvice(decServiceID, _
                                                 strServiceINAdviceContent, _
                                                 strAccount, _
                                                 dtfUpdate)
            End If

            '共通関数実行結果値を初期化
            rtnGlobalResult = ActionResult.Success

            'SMBチップ更新処理
            If True = blnResult And UnsetJobDtlId <> decNowJobDtlID Then
                If True = GetStallUseStatus(decNowJobDtlID, dealerCD, branchCD) Then
                    rtnGlobalResult = Reject(decServiceID, _
                                             decStallID, _
                                             strApplicationID, _
                                             dtfUpdate)
                Else
                    '2014/05/23 グローバル連携処理修正　START　↓↓↓
                    rtnGlobalResult = SelfFinish(decNowJobDtlID, _
                                                 decServiceID, _
                                                 decStallID, _
                                                 strApplicationID, _
                                                 prevStatus, _
                                                 prevJobStatus)
                    'blnGlobalResult = SelfFinish(decNowJobDtlID, _
                    '                             decServiceID, _
                    '                             decStallID, _
                    '                             strApplicationID)
                    '2014/05/23 グローバル連携処理修正　　END　↑↑↑
                End If
            End If

            '2014/06/16 通知&PUSH処理を別ロジックに変更　START　↓↓↓
            '通知＆PUSH処理
            'If True = blnResult Then
            '    Try
            '        '2014/05/14 通知&PUSH処理追加　START　↓↓↓
            '        'NoticeProcessing(objStaffContext, _
            '        '                 saChipId, _
            '        '                 basrezId, _
            '        '                 roNum, _
            '        '                 seqNo, _
            '        '                 vin, _
            '        '                 viewMode, _
            '        '                 CStr(decNowJobDtlID), _
            '        '                 dtfUpdate, _
            '        '                 EventkeyCommonProces)

            '        NoticeProcessing(objStaffContext, _
            '                         saChipId, _
            '                         basrezId, _
            '                         roNum, _
            '                         seqNo, _
            '                         vin, _
            '                         viewMode, _
            '                         CStr(decNowJobDtlID), _
            '                         dtfUpdate, _
            '                         EventkeyRejectProces)
            '        '2014/05/14 通知&PUSH処理追加　END　　↑↑↑

            '    Catch ex As Exception
            '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '                    , "NoticeProcessing Exception:{0}" _
            '                    , ex.Message))
            '    End Try
            'End If
            '2014/06/16 通知&PUSH処理を別ロジックに変更　END　　↑↑↑

            '完成検査データ更新
            'If True = blnResult And UnsetJobDtlId <> decNowJobDtlID Then

            '全ての処理が正常終了している場合、完成検査データ更新
            ' 2015/5/1 強制納車対応  start
            If blnResult And _
               UnsetJobDtlId <> decNowJobDtlID And _
               arySuccessList.Contains(rtnGlobalResult) Then

                blnResult = InspectionUpdate(InspectionUpdateReject, _
                                             dealerCD, _
                                             decNowJobDtlID, _
                                             decServiceID, _
                                             "", _
                                             strAccount, _
                                             dtfUpdate)
            End If

            'エラーが発生した場合、ロールバックを実行して戻り値をFalse(エラー)に設定する
            If Not blnResult OrElse _
               Not arySuccessList.Contains(rtnGlobalResult) Then

                Me.Rollback = True
                blnResult = False

            End If
            ' 2015/5/1 強制納車対応  end
            ''If False = blnResult Then
            ''    Throw New ApplicationException
            ''End If

        Catch ex As Exception
            Logger.Error(String.Format(CultureInfo.CurrentCulture, ex.Message))
            Me.Rollback = True
            blnResult = False
        End Try
        'TMT2販社 BTS310 新規登録時の例外処理追加 横展開修正 2015/04/06 end

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    '2014/06/16 通知&PUSH処理を別ロジックに変更　START　↓↓↓
    ''' <summary>
    ''' 完成検査結果通知＆PUSH処理(Reject)
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="jobDtlId">JobDtlID</param>
    ''' <param name="saChipId">来店者実績連番</param>
    ''' <param name="basrezId">DMS予約ID</param>
    ''' <param name="seqNo">RO_JOB_SEQ</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="viewMode">ViewMode</param>
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strAdviceContent">アドバイス</param>
    ''' <param name="dtInspecItem">検査項目</param>
    ''' <param name="dtMaintenance">メンテナンス項目</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    Public Function NoticeAfterRejectLogic(ByVal dealerCD As String, _
                                    ByVal branchCD As String, _
                                    ByVal roNum As String, _
                                    ByVal jobDtlId As String, _
                                    ByVal saChipId As String, _
                                    ByVal basrezId As String, _
                                    ByVal seqNo As String, _
                                    ByVal vin As String, _
                                    ByVal viewMode As String, _
                                    ByRef decNowJobDtlID As Decimal, _
                                    ByVal decServiceID As Decimal, _
                                    ByRef decStallID As Decimal, _
                                    ByVal strAdviceContent As String, _
                                    ByVal dtInspecItem As SC3180201RegistInfoDataTable, _
                                    ByVal dtMaintenance As SC3180201RegistInfoDataTable, _
                                    ByVal strAccount As String, _
                                    ByVal strApplicationID As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        Dim dtfUpdate As Date = DateTimeFunc.Now(dealerCD)

        Dim objStaffContext As StaffContext = StaffContext.Current

        '通知＆PUSH処理
        If True = blnResult Then
            Try
                NoticeProcessing(objStaffContext, _
                                 saChipId, _
                                 basrezId, _
                                 roNum, _
                                 seqNo, _
                                 vin, _
                                 viewMode, _
                                 CStr(decNowJobDtlID), _
                                 dtfUpdate, _
                                 EventkeyRejectProces)

            Catch ex As Exception
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "NoticeProcessing Exception:{0}" _
                            , ex.Message))
            End Try
        End If

        '完成検査データ更新
        If True = blnResult And UnsetJobDtlId <> decNowJobDtlID Then
            blnResult = InspectionUpdate(InspectionUpdateReject, _
                                         dealerCD, _
                                         decNowJobDtlID, _
                                         decServiceID, _
                                         "", _
                                         strAccount, _
                                         dtfUpdate)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function
    '2014/06/16 通知&PUSH処理を別ロジックに変更　END　　↑↑↑

    ''' <summary>
    ''' 完成検査結果データ登録(Approve)
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="jobDtlId">JobDtlID</param>
    ''' <param name="saChipId">来店者実績連番</param>
    ''' <param name="basrezId">DMS予約ID</param>
    ''' <param name="seqNo">RO_JOB_SEQ</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="viewMode">ViewMode</param>
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strAdviceContent">アドバイス</param>
    ''' <param name="dtInspecItem">検査項目</param>
    ''' <param name="dtMaintenance">メンテナンス項目</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <param name="rtnGlobalResult">戻り値用共通関数実行結果値</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function ApproveLogic(ByVal dealerCD As String, _
                                     ByVal branchCD As String, _
                                     ByVal roNum As String, _
                                     ByVal jobDtlId As String, _
                                     ByVal saChipId As String, _
                                     ByVal basrezId As String, _
                                     ByVal seqNo As String, _
                                     ByVal vin As String, _
                                     ByVal viewMode As String, _
                                     ByRef decNowJobDtlID As Decimal, _
                                     ByVal decServiceID As Decimal, _
                                     ByRef decStallID As Decimal, _
                                     ByVal strAdviceContent As String, _
                                     ByVal dtInspecItem As SC3180201RegistInfoDataTable, _
                                     ByVal dtMaintenance As SC3180201RegistInfoDataTable, _
                                     ByVal strAccount As String, _
                                     ByVal strApplicationID As String, _
                                     ByRef rtnGlobalResult As Long, _
                                     ByRef blnLastChipFlg As Boolean) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        'Dim blnGlobalResult As Boolean = True
        Dim dicJobDtlID As New Dictionary(Of String, Long)()
        Dim strServiceINAdviceContent As String
        Dim dtfUpdate As Date = DateTimeFunc.Now(dealerCD)
        'Dim blnLastChipFlg As Boolean

        Dim objStaffContext As StaffContext = StaffContext.Current

        'TMT2販社 BTS310 新規登録時の例外処理追加 横展開修正 2015/04/06 start
        Try
            '2014/05/23 グローバル連携処理修正　START　↓↓↓
            '前ステータス取得 2014/05/08
            Dim prevStatus As String = JudgeChipStatus(decStallID)
            Dim prevJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable = Nothing
            prevJobStatus = JudgeJobStatus(jobDtlId)
            '2014/05/23 グローバル連携処理修正　　END　↑↑↑

            '画面データ登録
            blnResult = RegistDispData(dealerCD, _
                                       branchCD, _
                                       roNum, _
                                       strAdviceContent, _
                                       dtInspecItem, _
                                       dtMaintenance,
                                       strAccount, _
                                       dtfUpdate, _
                                       InspectionUpdateApprove, _
                                       vin)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "Stall_use_id [{0}]: JOB_DTL_ID [{1}]" _
                                      , decStallID _
                                      , decNowJobDtlID))

            blnLastChipFlg = LastChipCheck(dealerCD, _
                                           branchCD, _
                                           roNum)

            '完成検査データ更新
            If True = blnResult And UnsetJobDtlId <> decNowJobDtlID Then
                blnResult = InspectionUpdate(InspectionUpdateApprove, _
                                             dealerCD, _
                                             decNowJobDtlID, _
                                             decServiceID, _
                                             "", _
                                             strAccount, _
                                             dtfUpdate)
            End If

            If True = blnResult Then
                strServiceINAdviceContent = strAdviceContent
                If String.Empty = strServiceINAdviceContent Then
                    strServiceINAdviceContent = " "
                End If

                blnResult = SetDBServiceINAdvice(decServiceID, _
                                                 strServiceINAdviceContent, _
                                                 strAccount, _
                                                 dtfUpdate)
            End If

            '共通関数実行結果値を初期化
            rtnGlobalResult = ActionResult.Success

            'SMBチップ更新
            If True = blnResult And UnsetJobDtlId <> decNowJobDtlID Then
                If True = blnLastChipFlg Then
                    '検査合格関数
                    rtnGlobalResult = PassedInspection(dealerCD, _
                                                       decStallID, _
                                                       decServiceID, _
                                                       strApplicationID, _
                                                       dtfUpdate)

                End If
                ' 2015/5/1 強制納車対応  start
                If arySuccessList.Contains(rtnGlobalResult) Then
                    '自力基幹連携
                    '2014/05/23 グローバル連携処理修正　START　↓↓↓
                    rtnGlobalResult = SelfFinish(decNowJobDtlID, _
                                                 decServiceID, _
                                                 decStallID, _
                                                 strApplicationID, _
                                                 prevStatus, _
                                                 prevJobStatus)
                    'blnGlobalResult = SelfFinish(decNowJobDtlID, _
                    '                             decServiceID, _
                    '                             decStallID, _
                    '                             strApplicationID)
                    '2014/05/23 グローバル連携処理修正　　END　↑↑↑
                End If
            End If

            '2014/06/16 通知&PUSH処理を別ロジックに変更　START　↓↓↓
            '通知&PUSH処理
            '2014/05/14 通知&PUSH処理追加　START　↓↓↓
            'If True = blnResult And True = blnLastChipFlg Then
            '    Try
            '        NoticeProcessing(objStaffContext, _
            '                         saChipId, _
            '                         basrezId, _
            '                         roNum, _
            '                         seqNo, _
            '                         vin, _
            '                         viewMode, _
            '                         CStr(decNowJobDtlID), _
            '                         dtfUpdate, _
            '                         EventkeyLastApproveProces)
            '    Catch ex As Exception
            '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '                    , "NoticeProcessing Exception:{0}" _
            '                    , ex.Message))
            '    End Try
            'End If

            'If True = blnResult Then
            '    Try
            '        If True = blnLastChipFlg Then
            '            '最終チップの場合
            '            NoticeProcessing(objStaffContext, _
            '                             saChipId, _
            '                             basrezId, _
            '                             roNum, _
            '                             seqNo, _
            '                             vin, _
            '                             viewMode, _
            '                             CStr(decNowJobDtlID), _
            '                             dtfUpdate, _
            '                             EventkeyLastApproveProces)
            '        Else
            '            '最終チップ以外
            '            NoticeProcessing(objStaffContext, _
            '                             saChipId, _
            '                             basrezId, _
            '                             roNum, _
            '                             seqNo, _
            '                             vin, _
            '                             viewMode, _
            '                             CStr(decNowJobDtlID), _
            '                             dtfUpdate, _
            '                             EventkeyApproveProces)
            '        End If
            '    Catch ex As Exception
            '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '                    , "NoticeProcessing Exception:{0}" _
            '                    , ex.Message))
            '    End Try
            'End If
            '2014/05/14 通知&PUSH処理追加　END　　↑↑↑
            '2014/06/16 通知&PUSH処理を別ロジックに変更　END　　↑↑↑

            'If False = blnResult Then
            '    Throw New ApplicationException
            'End If

            'エラーが発生した場合、ロールバックを実行して戻り値をFalse(エラー)に設定する
            If Not blnResult OrElse _
               Not arySuccessList.Contains(rtnGlobalResult) Then

                Me.Rollback = True
                blnResult = False

            End If
            ' 2015/5/1 強制納車対応  end
        Catch ex As Exception
            Logger.Error(String.Format(CultureInfo.CurrentCulture, ex.Message))
            Me.Rollback = True
            blnResult = False
        End Try
        'TMT2販社 BTS310 新規登録時の例外処理追加 横展開修正 2015/04/06 end

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    '2014/06/16 通知&PUSH処理を別ロジックに変更　START　↓↓↓
    ''' <summary>
    ''' 完成検査結果通知＆PUSH処理(Approve)
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="jobDtlId">JobDtlID</param>
    ''' <param name="saChipId">来店者実績連番</param>
    ''' <param name="basrezId">DMS予約ID</param>
    ''' <param name="seqNo">RO_JOB_SEQ</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="viewMode">ViewMode</param>
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strAdviceContent">アドバイス</param>
    ''' <param name="dtInspecItem">検査項目</param>
    ''' <param name="dtMaintenance">メンテナンス項目</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    Public Function NoticeAfterApproveLogic(ByVal dealerCD As String, _
                                     ByVal branchCD As String, _
                                     ByVal roNum As String, _
                                     ByVal jobDtlId As String, _
                                     ByVal saChipId As String, _
                                     ByVal basrezId As String, _
                                     ByVal seqNo As String, _
                                     ByVal vin As String, _
                                     ByVal viewMode As String, _
                                     ByRef decNowJobDtlID As Decimal, _
                                     ByVal decServiceID As Decimal, _
                                     ByRef decStallID As Decimal, _
                                     ByVal strAdviceContent As String, _
                                     ByVal dtInspecItem As SC3180201RegistInfoDataTable, _
                                     ByVal dtMaintenance As SC3180201RegistInfoDataTable, _
                                     ByVal strAccount As String, _
                                     ByVal strApplicationID As String, _
                                     ByRef blnLastChipFlg As Boolean) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        Dim dtfUpdate As Date = DateTimeFunc.Now(dealerCD)
        'Dim blnLastChipFlg As Boolean

        Dim objStaffContext As StaffContext = StaffContext.Current

        'blnLastChipFlg = LastChipCheck(dealerCD, _
        '                               branchCD, _
        '                               roNum)

        '通知&PUSH処理
        If True = blnResult Then
            Try
                If True = blnLastChipFlg Then
                    '最終チップの場合
                    NoticeProcessing(objStaffContext, _
                                     saChipId, _
                                     basrezId, _
                                     roNum, _
                                     seqNo, _
                                     vin, _
                                     viewMode, _
                                     CStr(decNowJobDtlID), _
                                     dtfUpdate, _
                                     EventkeyLastApproveProces)
                Else
                    '最終チップ以外
                    NoticeProcessing(objStaffContext, _
                                     saChipId, _
                                     basrezId, _
                                     roNum, _
                                     seqNo, _
                                     vin, _
                                     viewMode, _
                                     CStr(decNowJobDtlID), _
                                     dtfUpdate, _
                                     EventkeyApproveProces)
                End If
            Catch ex As Exception
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "NoticeProcessing Exception:{0}" _
                            , ex.Message))
            End Try
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function
    '2014/06/16 通知&PUSH処理を別ロジックに変更　END　　↑↑↑

    ''' <summary>
    ''' 画面データ登録
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="strAdviceContent">アドバイス</param>
    ''' <param name="dtInspecItem">検査項目</param>
    ''' <param name="dtMaintenance">メンテナンス項目</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    Public Function RegistDispData(ByVal dealerCD As String, _
                                   ByVal branchCD As String, _
                                   ByVal roNum As String, _
                                   ByVal strAdviceContent As String, _
                                   ByVal dtInspecItem As SC3180201RegistInfoDataTable, _
                                   ByVal dtMaintenance As SC3180201RegistInfoDataTable, _
                                   ByVal strAccount As String, _
                                   ByVal dtfUpdate As Date, _
                                   ByVal intUpdateStatus As Integer, _
                                   ByVal vin As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        Dim intDataIndex As Integer = 0
        Dim intListIndex As Integer = 0
        Dim dicJobDtlID As New Dictionary(Of String, Long)()

        '登録済みJobDtlIDリストの初期化
        dicJobDtlID.Clear()


        '前回部品交換情報リスト取得
        Dim ta As New SC3180201TableAdapter
        Dim prePartsReplaceDt As SC3180201PreviousPartsReplaceDataTable = New SC3180201PreviousPartsReplaceDataTable()
        prePartsReplaceDt = ta.GetPreviousPartsReplace(vin)

        '部品交換情報差異比較用
        Dim editPartsReplaceArray As List(Of String) = New List(Of String)


        'InspectionItem
        For intDataIndex = 0 To dtInspecItem.Count - 1

            '完成検査結果データ登録(完成検査結果データ:TB_T_FINAL_INSPECTION_HEAD)
            Dim blnRegisted As Boolean = False
            If True = dicJobDtlID.ContainsKey(dtInspecItem(intDataIndex).JobDtlID) Then
                '存在している(登録対象としない)
                blnRegisted = True
            Else
                '存在していない(登録対象とする)
                dicJobDtlID.Add(dtInspecItem(intDataIndex).JobDtlID, dtInspecItem(intDataIndex).RowLockVer)
            End If

            If False = blnRegisted And True = blnResult Then
                '未登録・未更新
                blnResult = SetDBCmpChkReslut(dealerCD, _
                                              branchCD, _
                                              dtInspecItem(intDataIndex).JobDtlID, _
                                              roNum, _
                                              dtInspecItem(intDataIndex).AprovalStatus, _
                                              strAccount, _
                                              dtfUpdate, _
                                              dtInspecItem(intDataIndex).RowLockVer, _
                                              intUpdateStatus)
            End If

            '完成検査結果データ登録(完成検査結果データ:TB_T_FINAL_INSPECTION_HEAD)
            If True = blnResult Then
                'blnResult = SetDBCmpChkResultDetail(dealerCD, _
                '                                    branchCD, _
                '                                    dtInspecItem(intDataIndex).JobDtlID, _
                '                                    DefaultJobInspectId, _
                '                                    DefaultJobInspectSeq, _
                '                                    dtInspecItem(intDataIndex).ItemCD, _
                '                                    dtInspecItem(intDataIndex).ItemsCheck, _
                '                                    dtInspecItem(intDataIndex).ItemsSelect_Replaced, _
                '                                    dtInspecItem(intDataIndex).ItemsSelect_Fixed, _
                '                                    dtInspecItem(intDataIndex).ItemsSelect_Cleaned, _
                '                                    dtInspecItem(intDataIndex).ItemsSelect_Swapped, _
                '                                    dtInspecItem(intDataIndex).ItemsTextBefore, _
                '                                    dtInspecItem(intDataIndex).ItemsTextAfter, _
                '                                    strAccount, _
                '                                    dtfUpdate)
                blnResult = SetDBCmpChkResultDetail(dealerCD, _
                                                    branchCD, _
                                                    dtInspecItem(intDataIndex).JobDtlID, _
                                                    dtInspecItem(intDataIndex).JobInstructID, _
                                                    dtInspecItem(intDataIndex).JobInstructSeq, _
                                                    dtInspecItem(intDataIndex).ItemCD, _
                                                    dtInspecItem(intDataIndex).ItemsCheck, _
                                                    dtInspecItem(intDataIndex).ItemsSelect_Replaced, _
                                                    dtInspecItem(intDataIndex).ItemsSelect_Fixed, _
                                                    dtInspecItem(intDataIndex).ItemsSelect_Cleaned, _
                                                    dtInspecItem(intDataIndex).ItemsSelect_Swapped, _
                                                    dtInspecItem(intDataIndex).ItemsTextBefore, _
                                                    dtInspecItem(intDataIndex).ItemsTextAfter, _
                                                    strAccount, _
                                                    dtfUpdate)
            End If

            ' 2017/2/17 ライフサイクル対応 前回部品交換情報を登録 Start
            If dtInspecItem(intDataIndex).ItemsSelect_Replaced <> DefaultAlreadyReplaceInt _
                Or dtInspecItem(intDataIndex).ItemsCheck = InspecResltCodeReplaceInt Then

                ' 差異比較の配列に追加
                editPartsReplaceArray.Add(dtInspecItem(intDataIndex).ItemCD)

                If blnResult Then
                    blnResult = SetPreviousPartsReplace(vin,
                                            dtInspecItem(intDataIndex).ItemCD, _
                                            dealerCD, _
                                            branchCD, _
                                            roNum, _
                                            dtfUpdate, _
                                            strAccount, _
                                            prePartsReplaceDt, _
                                            intUpdateStatus)
                End If
            End If
            ' 2017/2/17 ライフサイクル対応 前回部品交換情報を登録 End

            If False = blnResult Then
                'エラー発生のため終了
                Exit For
            End If
        Next

        ' 2017/2/17 ライフサイクル対応 前回部品交換情報を削除・更新 Start
        If blnResult Then
            If dtInspecItem.Count > 0 Then
                blnResult = NotReplacePreviousParts(vin, roNum, editPartsReplaceArray, prePartsReplaceDt, strAccount, dtfUpdate)
            End If
        End If
        ' 2017/2/17 ライフサイクル対応 前回部品交換情報を削除・更新 End

        'Maintenance
        If True = blnResult Then
            For intDataIndex = 0 To dtMaintenance.Count - 1

                '完成検査結果データ登録(完成検査結果データ:TB_T_FINAL_INSPECTION_HEAD)
                Dim blnRegisted As Boolean = False
                If True = dicJobDtlID.ContainsKey(dtMaintenance(intDataIndex).JobDtlID) Then
                    '存在している(登録対象としない)
                    blnRegisted = True
                Else
                    '存在していない(登録対象とする)
                    dicJobDtlID.Add(dtMaintenance(intDataIndex).JobDtlID, dtMaintenance(intDataIndex).RowLockVer)
                End If

                If False = blnRegisted And True = blnResult Then
                    blnResult = SetDBCmpChkReslut(dealerCD, _
                                                  branchCD, _
                                                  dtMaintenance(intDataIndex).JobDtlID, _
                                                  roNum, _
                                                  dtMaintenance(intDataIndex).AprovalStatus, _
                                                  strAccount, _
                                                  dtfUpdate, _
                                                  dtMaintenance(intDataIndex).RowLockVer, _
                                                  intUpdateStatus)
                End If

                '完成検査結果データ登録(完成検査結果データ:TB_T_FINAL_INSPECTION_HEAD)
                If True = blnResult Then
                    blnResult = SetDBCmpChkResultDetail(dealerCD, _
                                                        branchCD, _
                                                        dtMaintenance(intDataIndex).JobDtlID, _
                                                        dtMaintenance(intDataIndex).JobInstructID, _
                                                        dtMaintenance(intDataIndex).JobInstructSeq, _
                                                        DefaultItemCD, _
                                                        dtMaintenance(intDataIndex).ItemsCheck, _
                                                        DefaultAlreadyReplace, _
                                                        DefaultAlreadyFix, _
                                                        DefaultAlreadyClean, _
                                                        DefaultAlreadySwap, _
                                                        DefaultBeforeText, _
                                                        DefaultAfterText, _
                                                        strAccount, _
                                                        dtfUpdate)

                End If

                If False = blnResult Then
                    'エラー発生のため終了
                    Exit For
                End If
            Next
        End If

        If True = blnResult Then
            For intListIndex = 0 To dicJobDtlID.Count - 1
                '行ロックバージョン更新処理
                If True = blnResult And -1 < dicJobDtlID.Values(intListIndex) Then
                    blnResult = SetDBInspectionLockUpt(dealerCD, _
                                                       branchCD, _
                                                       dicJobDtlID.Keys(intListIndex), _
                                                       dicJobDtlID.Values(intListIndex))
                End If

                If False = blnResult Then
                    'エラー発生のため終了
                    Exit For
                End If
            Next

            '2017/2/1 TR-SVT-TMT-20161209-002 アドバイスをRO単位で更新する Start
            '更新対象の合った場合のみ
            If True = blnResult And 0 < dicJobDtlID.Count Then
                'アドバイスを更新する
                blnResult = SetDBInspectionAdvice(dealerCD, _
                                                  branchCD, _
                                                  roNum, _
                                                  strAdviceContent, _
                                                  strAccount, _
                                                  dtfUpdate)
            End If
            '2017/2/1 TR-SVT-TMT-20161209-002 アドバイスをRO単位で更新する end

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END, Return:[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , blnResult))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 最終チップ判断
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <remarks>True:最終チップ/False:最終チップ以外</remarks>
    Private Function LastChipCheck(ByVal dealerCD As String, _
                                   ByVal branchCD As String, _
                                   ByVal roNum As String) As Boolean

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim lastChipFlg As String

        lastChipFlg = GetDBChkLastChip(dealerCD, _
                                       branchCD, _
                                       roNum)

        If LastChipFlag = lastChipFlg Then
            blnResult = True
        Else
            blnResult = False
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END, Return:[{2}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , blnResult))

        Return blnResult
    End Function

    ''' <summary>
    ''' SMBチップ更新 reject
    ''' </summary>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <param name="dtfUpdate">更新日付</param>
    ''' <remarks>True:成功/False:失敗</remarks>
    Private Function Reject(ByVal decServiceID As Decimal, _
                            ByVal decStallID As Decimal, _
                            ByVal strApplicationID As String, _
                            ByVal dtfUpdate As Date) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim commonUtility As New TabletSMBCommonClassBusinessLogic
        Dim rejectResult As Long = -1

        'サービス入庫 行ロックバージョン取得
        Dim lockVersion = GetServiceInLock(decServiceID)

        rejectResult = commonUtility.FailedInspection(decStallID, _
                                                      dtfUpdate, _
                                                      strApplicationID, _
                                                      lockVersion)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'If rejectResult = 0 Then
        '    Return True
        'Else
        '    Return False
        'End If

        Return rejectResult
    End Function

    ''' <summary>
    ''' 完成検査データ更新
    ''' </summary>
    ''' <param name="intUpdateStatus">更新区分</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="strSendUser">送信先ユーザー</param>
    ''' <param name="strAccount">登録アカウント</param>
    ''' <param name="dtfUpdate">更新日付</param>
    ''' <remarks>True:成功/False:失敗</remarks>
    Private Function InspectionUpdate(ByVal intUpdateStatus As Integer, _
                                      ByVal dealerCD As String, _
                                      ByVal decNowJobDtlID As Decimal, _
                                      ByVal decServiceID As Decimal, _
                                      ByVal strSendUser As String, _
                                      ByVal strAccount As String, _
                                      ByVal dtfUpdate As Date) As Boolean

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = False

        blnResult = SetDBInspection(decNowJobDtlID, _
                                    strAccount, _
                                    intUpdateStatus, _
                                    dtfUpdate, _
                                    decServiceID)
        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return blnResult
    End Function

    ''' <summary>
    ''' 検査合格関数
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="decStallID">ストール利用ID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    ''' <param name="dtfUpdate">更新日付</param>
    ''' <remarks>True:成功/False:失敗</remarks>
    Private Function PassedInspection(ByVal dealerCD As String, _
                                      ByVal decStallID As Decimal, _
                                      ByVal decServiceID As Decimal, _
                                      ByVal strApplicationID As String, _
                                      ByVal dtfUpdate As Date) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


        Dim commonUtility As New TabletSMBCommonClassBusinessLogic
        Dim finishResult As Long = -1

        'サービス入庫 行ロックバージョン取得
        Dim lockVersion = GetServiceInLock(decServiceID)

        finishResult = commonUtility.PassedInspection(decStallID, _
                                                      dtfUpdate, _
                                                      strApplicationID, _
                                                      lockVersion)

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'If finishResult = 0 Then
        '    Return True
        'Else
        '    Return False
        'End If

        Return finishResult

    End Function

    ''' <summary>
    ''' 登録処理(SetDBCmpChkResultUpt)
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="aprovalStatus">作業ステータス</param>
    ''' <param name="accountName">更新者</param>
    ''' <param name="updateTime">更新日</param>
    ''' <param name="lockVersion">LockVersion</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDBCmpChkReslut(ByVal dlrCD As String, _
                                          ByVal brnCD As String, _
                                          ByVal jobDtlId As Decimal, _
                                          ByVal roNum As String, _
                                          ByVal aprovalStatus As Integer, _
                                          ByVal accountName As String, _
                                          ByVal updateTime As Date, _
                                          ByVal lockVersion As Long, _
                                          ByVal updateFlg As Integer) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim tableAdapter As New SC3180201TableAdapter

        'ヘッダロック処理
        SelectInspectionHeadLock(jobDtlId)

        '登録処理
        blnResult = tableAdapter.SetDBCmpChkResultUpt(dlrCD, _
                                                      brnCD, _
                                                      jobDtlId, _
                                                      roNum, _
                                                      aprovalStatus, _
                                                      accountName, _
                                                      updateTime, _
                                                      lockVersion, _
                                                      updateFlg)
        blnResult = True    '暫定
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END, Return:[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , blnResult))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 登録処理(SetDBCmpChkResultDetailUpt)
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="jobInstructId">作業指示ID</param>
    ''' <param name="jobInstructSeq">作業指示枝番</param>
    ''' <param name="inspecItemCD">点検項目コード</param>
    ''' <param name="inspecRsltCD">点検結果</param>
    ''' <param name="alreadyReplace">選択状態(replace)</param>
    ''' <param name="alreadyFixed">選択状態(fixed)</param>
    ''' <param name="alreadyCelaning">選択状態(celaning)</param>
    ''' <param name="alreadySwapped">選択状態(swapped)</param>
    ''' <param name="beforeText">作業値入力(Before)</param>
    ''' <param name="afterText">作業値入力(After)</param>
    ''' <param name="accountName">更新者</param>
    ''' <param name="updateTime">更新日</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDBCmpChkResultDetail(ByVal dlrCD As String, _
                                                ByVal brnCD As String, _
                                                ByVal jobDtlId As Decimal, _
                                                ByVal jobInstructId As String, _
                                                ByVal jobInstructSeq As Long, _
                                                ByVal inspecItemCD As String, _
                                                ByVal inspecRsltCD As Long, _
                                                ByVal alreadyReplace As Long, _
                                                ByVal alreadyFixed As Long, _
                                                ByVal alreadyCelaning As Long, _
                                                ByVal alreadySwapped As Long, _
                                                ByVal beforeText As Decimal, _
                                                ByVal afterText As Decimal, _
                                                ByVal accountName As String, _
                                                ByVal updateTime As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim tableAdapter As New SC3180201TableAdapter

        'ヘッダロック処理
        SelectInspectionHeadLock(jobDtlId)

        '登録処理
        blnResult = tableAdapter.SetDBCmpChkResultDetailUpt(dlrCD, _
                                                            brnCD, _
                                                            jobDtlId, _
                                                            jobInstructId, _
                                                            jobInstructSeq, _
                                                            inspecItemCD, _
                                                            inspecRsltCD, _
                                                            alreadyReplace, _
                                                            alreadyFixed, _
                                                            alreadyCelaning, _
                                                            alreadySwapped, _
                                                            beforeText, _
                                                            afterText, _
                                                            accountName, _
                                                            updateTime)
        blnResult = True ' 暫定

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END, Return:[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , blnResult))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 登録処理(SetDBInspectionUpt)
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="accountName">更新者</param>
    ''' <param name="updateFlg">更新フラグ</param>
    ''' <param name="updateTime">更新日</param>
    ''' <param name="svcinId">サービスID</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDBInspection(ByVal jobDtlId As Decimal, _
                                        ByVal accountName As String, _
                                        ByVal updateFlg As Integer,
                                        ByVal updateTime As Date, _
                                        ByVal svcinId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim tableAdapter As New SC3180201TableAdapter

        'サービスインロック処理
        SelectSvcinLock(svcinId)

        Dim lockVersion As Long = Me.GetServiceInLock(svcinId)

        blnResult = tableAdapter.SetDBInspectionUpt(jobDtlId, _
                                                    accountName, _
                                                    updateFlg, _
                                                    updateTime)

        If blnResult = True Then
            blnResult = tableAdapter.SetServiceInLockUpt(svcinId)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' アドバイス登録処理(SetDBServiceINAdviceComment)
    ''' </summary>
    ''' <param name="svcinId">サービスID</param>
    ''' <param name="advicdContent">アドバイス</param>
    ''' <param name="accountName">更新者</param>
    ''' <param name="updateTime">更新日</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDBServiceINAdvice(ByVal svcinId As Decimal, _
                                             ByVal advicdContent As String, _
                                             ByVal accountName As String, _
                                             ByVal updateTime As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim tableAdapter As New SC3180201TableAdapter

        'サービスインロック処理
        SelectSvcinLock(svcinId)

        'サービスインロックバージョン取得
        Dim lockVersion As Long = Me.GetServiceInLock(svcinId)

        blnResult = tableAdapter.SetDBServiceINAdviceComment(svcinId, _
                                                                advicdContent, _
                                                                accountName, _
                                                                updateTime, _
                                                                lockVersion)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END, Return:[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , blnResult))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' ChkLastChip情報取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <returns>1:最終チップ　0:作業途中</returns>
    ''' <remarks></remarks>
    Public Function GetDBChkLastChip(ByVal dlrCD As String, _
                                     ByVal brnCD As String, _
                                     ByVal roNum As String) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索処理
        Dim tableAdapter As New SC3180201TableAdapter
        Dim dtChkLastChip As SC3180201ChkLastChipDataTable

        dtChkLastChip = tableAdapter.GetDBChkLastChip(dlrCD, brnCD, roNum)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return dtChkLastChip(0).count.ToString

    End Function

    ''' <summary>
    ''' 行ロック更新処理(SetInspectionHeadLockUpt)
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="lockVersion">ロックバージョン</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDBInspectionLockUpt(ByVal dlrCD As String, _
                                                 ByVal brnCD As String, _
                                                 ByVal jobDtlId As Decimal, _
                                                 ByVal lockVersion As Long) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim tableAdapter As New SC3180201TableAdapter

        '完成検査ヘッダロック処理
        SelectInspectionHeadLock(jobDtlId)

        blnResult = tableAdapter.SetInspectionHeadLockUpt(dlrCD, _
                                                          brnCD, _
                                                          jobDtlId, _
                                                          lockVersion)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END, Return:[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , blnResult))

        '処理結果返却
        Return blnResult

    End Function

    ''' <summary>
    ''' 行ロックバージョン取得(GetServiceInLock)
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <returns>True:成功/False:失敗</returns>
    ''' <remarks></remarks>
    Public Function GetServiceInLock(ByVal svcinId As Decimal) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180201TableAdapter

        'サービスイン 行ロックバージョンの取得
        Dim svcinLockVersion = tableAdapter.GetServiceLockVersion(svcinId)
        Dim lockVersion As Long

        lockVersion = Long.Parse(svcinLockVersion(0).ROW_LOCK_VERSION.ToString)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return lockVersion

    End Function

    ''' <summary>
    ''' ストール利用ステータス取得
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <param name="brnCD">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStallUseStatus(ByVal jobDtlId As Decimal, ByVal dlrCD As String, ByVal brnCD As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180201TableAdapter

        'ストール利用IDの取得
        Dim stallUse = tableAdapter.GteStallUse(jobDtlId, dlrCD, brnCD)
        Dim stallUseStatus As String = ""

        If Not IsNothing(stallUse(0).STALL_USE_STATUS.ToString) Then
            stallUseStatus = stallUse(0).STALL_USE_STATUS.ToString
        End If

        '取得したストールステータスを判定
        If StallUseStatusCompletion = stallUseStatus Then
            'ステータスが"03"ならばReject対象
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END True" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '処理結果返却
            Return True
        Else

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END False" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            '処理結果返却
            Return False
        End If


    End Function

#End Region

#Region "通知"

#Region "Publicメソッド"

    ''' <summary>
    ''' 通知処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Sub NoticeProcessing(ByVal inStaffInfo As StaffContext, _
                                ByVal inSaChip As String, _
                                ByVal inBaserzId As String, _
                                ByVal inRoNumber As String, _
                                ByVal inSeqNo As String, _
                                ByVal inVin As String, _
                                ByVal inViewMode As String, _
                                ByVal inJobDtlId As String, _
                                ByVal inPresentTime As DateTime, _
                                ByVal inEventKey As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ROUMBER[{2}] PRESENTTIME:{3} EVENTKEY:{4}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inRoNumber, inPresentTime, inEventKey))

        'SC3140103DataTableAdapterのインスタンス
        Dim da As New SC3180201TableAdapter

        '2016/11/08 (TR-SVT-TMT-20160512-001) TBL_SERVICE_VISIT_MANAGEMENTにデータが無い場合はSA通知を送らない
        'サービス来店者管理テーブル存在フラグ
        Dim isExistSvcVisitMng As Boolean = True

        '2014/05/14 通知&PUSH処理追加　START　↓↓↓
        '遷移先画面取得Directory作成
        Dim ScreenTransitionDictionary As Dictionary(Of String, Dictionary(Of String, String)) = Me.CreateScreenTransitionDictionary
        '2014/05/14 通知&PUSH処理追加　END　　↑↑↑

        '通知送信用情報取得
        Dim dtNoticeProcessingInfo As SC3180201NoticeProcessingInfoDataTable = _
            da.GetNoticeProcessingInfo(inRoNumber, _
                                       inStaffInfo.DlrCD, _
                                       inStaffInfo.BrnCD, _
                                       Decimal.Parse(inJobDtlId))

        '通知送信用情報取得チェック
        If 0 < dtNoticeProcessingInfo.Count Then
            '取得できた場合

            'Rowに変換
            Dim rowNoticeProcessingInfo As SC3180201NoticeProcessingInfoRow = _
                DirectCast(dtNoticeProcessingInfo.Rows(0), SC3180201NoticeProcessingInfoRow)

            '現在日時を設定
            rowNoticeProcessingInfo.PRESENTTIME = inPresentTime

            'イベント情報判定
            Select Case inEventKey

                Case CType(EventKeyId.CommonProces, String)
                    '共通処理

                    '共通の実行
                    Me.NoticeMainProcessing(rowNoticeProcessingInfo, _
                                            inStaffInfo, _
                                            inSaChip, _
                                            inBaserzId, _
                                            inRoNumber, _
                                            inSeqNo, _
                                            inVin, _
                                            inViewMode, _
                                            inJobDtlId, _
                                            EventKeyId.CommonProces, _
                                            ScreenTransitionDictionary)

                Case CType(EventKeyId.LastAproveProces, String)
                    '最終承認処理

                    '2014/05/14 通知&PUSH処理追加　START　↓↓↓
                    '承認の実行（依頼者へ通知）
                    Me.NoticeMainProcessing(rowNoticeProcessingInfo, _
                                            inStaffInfo, _
                                            inSaChip, _
                                            inBaserzId, _
                                            inRoNumber, _
                                            inSeqNo, _
                                            inVin, _
                                            inViewMode, _
                                            inJobDtlId, _
                                            EventKeyId.AproveProces, _
                                            ScreenTransitionDictionary)
                    '2014/05/14 通知&PUSH処理追加　END　　↑↑↑

                    '2016/11/08 (TR-SVT-TMT-20160512-001) TBL_SERVICE_VISIT_MANAGEMENTにデータが無い場合はSA通知を送らない
                    isExistSvcVisitMng = da.GetSvcVisitManagementExist(inRoNumber, _
                                                                       inStaffInfo.DlrCD, _
                                                                       inStaffInfo.BrnCD)

                    If isExistSvcVisitMng Then


                        '最終承認の実行（SAへ通知）
                        Me.NoticeMainProcessing(rowNoticeProcessingInfo, _
                                                inStaffInfo, _
                                                inSaChip, _
                                                inBaserzId, _
                                                inRoNumber, _
                                                inSeqNo, _
                                                inVin, _
                                                inViewMode, _
                                                inJobDtlId, _
                                                EventKeyId.LastAproveProces, _
                                                ScreenTransitionDictionary)

                    End If

                Case CType(EventKeyId.AproveProces, String)
                    '承認処理

                    '2014/05/14 通知&PUSH処理追加　START　↓↓↓
                    '承認の実行（依頼者へ通知）
                    Me.NoticeMainProcessing(rowNoticeProcessingInfo, _
                                            inStaffInfo, _
                                            inSaChip, _
                                            inBaserzId, _
                                            inRoNumber, _
                                            inSeqNo, _
                                            inVin, _
                                            inViewMode, _
                                            inJobDtlId, _
                                            EventKeyId.AproveProces, _
                                            ScreenTransitionDictionary)
                    '2014/05/14 通知&PUSH処理追加　END　　↑↑↑

                Case CType(EventKeyId.RejectProces, String)
                    '否認処理

                    '否認の実行（依頼者へ通知）
                    Me.NoticeMainProcessing(rowNoticeProcessingInfo, _
                                            inStaffInfo, _
                                            inSaChip, _
                                            inBaserzId, _
                                            inRoNumber, _
                                            inSeqNo, _
                                            inVin, _
                                            inViewMode, _
                                            inJobDtlId, _
                                            EventKeyId.RejectProces, _
                                            ScreenTransitionDictionary)

            End Select

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            'Push通知によるメイン画面リフレッシュ

            Dim operationCdList As New List(Of Decimal)

            'イベント特定キーが「201：最終承認処理」なら、「200：承認処理」としてDictionaryキーを作成する
            Dim ChangeEventKey As String
            If EventkeyLastApproveProces = inEventKey Then
                ChangeEventKey = EventkeyApproveProces
            Else
                ChangeEventKey = inEventKey
            End If

            '①ChTとCTにPUSH処理
            Dim TransDicKey = CreateTransDicKey(inStaffInfo.OpeCD, Operation.CT, ChangeEventKey)
            If ScreenTransitionDictionary.ContainsKey(TransDicKey) Then
                '作成したDictionaryキーが遷移先画面取得Dictionaryに存在する
                If Not String.IsNullOrWhiteSpace(ScreenTransitionDictionary(TransDicKey)(TransDicKeyPushMethod)) Then
                    '指定したキーにPushMethodが登録されている

                    '権限リストの作成
                    operationCdList.Clear()
                    'OperationCodeリストに権限"55"：CTを設定
                    operationCdList.Add(Operation.CT)
                    'OperationCodeリストに権限"62"：CHTを設定
                    operationCdList.Add(Operation.CHT)

                    'PUSH処理
                    Me.SendGateNotice(inStaffInfo.DlrCD _
                                      , inStaffInfo.BrnCD _
                                      , operationCdList _
                                      , ScreenTransitionDictionary(TransDicKey)(TransDicKeyPushMethod) _
                                      )
                End If
            End If

            '②FMにPUSH処理
            Dim SATransDicKey As String = CreateTransDicKey(inStaffInfo.OpeCD, Operation.FM, ChangeEventKey)

            If ScreenTransitionDictionary.ContainsKey(SATransDicKey) Then
                '作成したDictionaryキーが遷移先画面取得Dictionaryに存在する
                If Not String.IsNullOrWhiteSpace(ScreenTransitionDictionary(SATransDicKey)(TransDicKeyPushMethod)) Then
                    '指定したキーにPushMethodが登録されている

                    '権限リストの作成
                    operationCdList.Clear()
                    'OperationCodeリストに権限"58"：FAを設定
                    operationCdList.Add(Operation.FM)

                    'PUSH処理
                    Me.SendGateNotice(inStaffInfo.DlrCD _
                                      , inStaffInfo.BrnCD _
                                      , operationCdList _
                                      , ScreenTransitionDictionary(SATransDicKey)(TransDicKeyPushMethod) _
                                      )
                End If
            End If

            '2016/11/08 (TR-SVT-TMT-20160512-001) TBL_SERVICE_VISIT_MANAGEMENTにデータが無い場合はSA通知を送らない
            If isExistSvcVisitMng Then


                '2014/11/26 起票SA個人へのPush通知追加　 START   ↓↓↓
                '③SAにPUSH処理
                Dim strSATransDicKey As String = CreateTransDicKey(inStaffInfo.OpeCD, Operation.SA, inEventKey) '最終承認のみPush通知

                If ScreenTransitionDictionary.ContainsKey(strSATransDicKey) Then
                    '作成したDictionaryキーが遷移先画面取得Dictionaryに存在する
                    If Not String.IsNullOrWhiteSpace(ScreenTransitionDictionary(strSATransDicKey)(TransDicKeyPushMethod)) Then
                        '指定したキーにPushMethodが登録されている
                        '担当SAユーザーの取得用変数
                        Dim tableAdapter As New SC3180201TableAdapter
                        Dim user As SC3180201PicClientDataTable = Nothing

                        '担当SAユーザーの取得
                        user = tableAdapter.GetPicSaStf(inStaffInfo.DlrCD, inStaffInfo.BrnCD, inRoNumber, inJobDtlId)
                        Dim userdt As SC3180201PicClientRow = DirectCast(user.Rows(0), SC3180201PicClientRow)

                        Logger.Info("Push issue SA: " & userdt.ACCOUNT)

                        '担当SAへPUSH処理
                        Me.SendGatePush(userdt.ACCOUNT, ScreenTransitionDictionary(strSATransDicKey)(TransDicKeyPushMethod))
                    End If
                End If
                '2014/11/26 起票SA個人へのPush通知追加　 END     ↑↑↑

                '2014/05/13 通知&PUSH処理追加　END　　↑↑↑
            End If

        Else
            '取得失敗

            'エラーログ
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} GetNoticeProcessingInfo IS NOTHING" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 通知メイン処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <param name="inTransDic">遷移先画面取得Dictionary</param>
    ''' <remarks>2014/05/16　通知＆PUSH処理追加　引数「inTransDic」を追加</remarks>
    Private Sub NoticeMainProcessing(ByVal inRowNoticeProcessingInfo As SC3180201NoticeProcessingInfoRow, _
                                     ByVal inStaffInfo As StaffContext, _
                                     ByVal inSaChip As String, _
                                     ByVal inBaserzId As String, _
                                     ByVal inRoNumber As String, _
                                     ByVal inSeqNo As String, _
                                     ByVal inVin As String, _
                                     ByVal inViewMode As String, _
                                     ByVal inJobDtlId As String, _
                                     ByVal inEventKey As EventKeyId, _
                                     ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String)))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START EVENTKEY:{2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inEventKey))

        Dim account As List(Of XmlAccount) = New List(Of XmlAccount)()
        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '依頼者の権限コード
        Dim inClientCode As String = Nothing
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        Select Case inEventKey

            Case EventKeyId.CommonProces

                '送信先アカウント情報設定
                account = Me.CreateAccount(inStaffInfo, inEventKey)

            Case EventKeyId.LastAproveProces

                '送信先アカウント情報設定(個別)
                account = Me.CreateAccountParticular(inStaffInfo, inRowNoticeProcessingInfo.RO_NUM, inEventKey, inJobDtlId, inClientCode)

            Case EventKeyId.AproveProces

                '2014/05/14 通知&PUSH処理追加　START　↓↓↓
                '送信先アカウント情報設定(個別)
                account = Me.CreateAccountParticular(inStaffInfo, inRowNoticeProcessingInfo.RO_NUM, inEventKey, inJobDtlId, inClientCode)
                '2014/05/14 通知&PUSH処理追加　END　　↑↑↑

            Case EventKeyId.RejectProces

                '2014/05/14 通知&PUSH処理追加　START　↓↓↓
                '送信先アカウント情報設定(個別)
                account = Me.CreateAccountParticular(inStaffInfo, inRowNoticeProcessingInfo.RO_NUM, inEventKey, inJobDtlId, inClientCode)
                '2014/05/14 通知&PUSH処理追加　END　　↑↑↑

        End Select

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '遷移先画面取得Dictionaryのキーを作成
        Dim TransDicKey As String = CreateTransDicKey(CType(inStaffInfo.OpeCD, String), inClientCode, CType(inEventKey, String))

        Logger.Info(String.Format("ScreenTransitionDictionary, Key:[{0}], ContainsKey:[{1}]" _
                                  , TransDicKey _
                                  , inTransDic.ContainsKey(TransDicKey)))
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '「操作者権限、依頼者権限、イベント特定キー」が遷移先画面取得Dictionaryに登録されていたら通知処理を実行する
        If inTransDic.ContainsKey(TransDicKey) Then

            '通知履歴登録情報の設定
            Dim requestNotice As XmlRequestNotice = Me.CreateRequestNotice(inRowNoticeProcessingInfo, _
                                                                           inStaffInfo, _
                                                                           inSaChip, _
                                                                           inBaserzId, _
                                                                           inRoNumber, _
                                                                           inSeqNo, _
                                                                           inVin, _
                                                                           inViewMode, _
                                                                           inJobDtlId, _
                                                                           inEventKey, _
                                                                           inClientCode, _
                                                                           inTransDic)

            'Push情報作成処理の設定
            Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(inRowNoticeProcessingInfo _
                                                            , inEventKey _
                                                            , CType(inStaffInfo.OpeCD, String) _
                                                            , inClientCode _
                                                            , inTransDic _
                                                            )

            '設定したものを格納し、通知APIをコール
            Using noticeData As New XmlNoticeData

                '現在時間データの格納
                noticeData.TransmissionDate = inRowNoticeProcessingInfo.PRESENTTIME
                '送信ユーザーデータ格納
                noticeData.AccountList.AddRange(account.ToArray)
                '通知履歴用のデータ格納
                noticeData.RequestNotice = requestNotice
                'Pushデータ格納
                noticeData.PushInfo = pushInfo

                '通知処理実行
                Using ic3040801Biz As New IC3040801BusinessLogic

                    Logger.Info(GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
                    Logger.Info(LogNoticeData(noticeData) &
                                GetLogParam("noticeDisposalMode", CStr(NoticeDisposal.GeneralPurpose), True))

                    '通知処理実行
                    ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)

                End Using
            End Using

        End If
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "通知独自部分"

    ''' <summary>
    ''' 送信先アカウント情報作成処理
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <returns>送信先アカウント情報リスト</returns>
    ''' <remarks></remarks>
    Private Function CreateAccount(ByVal inStaffInfo As StaffContext, _
                                   ByVal inEventKey As EventKeyId) As List(Of XmlAccount)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START EVENTKEY:{2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inEventKey))

        '送信先アカウント情報リスト
        Dim accountList As New List(Of XmlAccount)

        'OperationCodeリスト
        Dim operationCodeList As New List(Of Long)

        'OperationCodeリストに権限"55"：CTを設定
        operationCodeList.Add(Operation.CT)

        'OperationCodeリストに権限"62"：CHTを設定
        operationCodeList.Add(Operation.CHT)

        'ユーザーステータス取得
        Using user As New IC3810601BusinessLogic

            'ユーザーステータス取得処理
            '各権限の全ユーザー情報取得
            Dim userdt As IC3810601DataSet.AcknowledgeStaffListDataTable = _
                user.GetAcknowledgeStaffList(inStaffInfo.DlrCD, _
                                             inStaffInfo.BrnCD, _
                                             operationCodeList)

            'オンラインユーザー分ループ
            For Each userRow As IC3810601DataSet.AcknowledgeStaffListRow In userdt

                '送信先アカウント情報 
                Using account As New XmlAccount

                    '受信先のアカウント設定
                    account.ToAccount = userRow.ACCOUNT

                    '受信先アカウントログ出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                              , "ACCOUNT [{0}] " _
                              , userRow.ACCOUNT))

                    '受信者名設定
                    account.ToAccountName = userRow.USERNAME

                    '受信者名ログ出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                              , "USERNAME [{0}] " _
                              , userRow.USERNAME))


                    '送信先アカウント情報リストに送信先アカウント情報を追加
                    accountList.Add(account)

                End Using

            Next

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return accountList


    End Function

    ''' <summary>
    ''' 送信先アカウント情報作成処理（個別）
    ''' </summary>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inRoNum">Ro番号</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <returns>送信先アカウント情報リスト</returns>
    ''' <remarks></remarks>
    Private Function CreateAccountParticular(ByVal inStaffInfo As StaffContext, _
                                             ByVal inRoNum As String, _
                                             ByVal inEventKey As EventKeyId, _
                                             ByVal inJobDtlId As String, _
                                             ByRef inAccountCD As String) As List(Of XmlAccount)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START EVENTKEY:{2}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , inEventKey))

        '送信先アカウント情報リスト
        Dim accountList As New List(Of XmlAccount)

        'ユーザーステータス取得
        Dim tableAdapter As New SC3180201TableAdapter

        '2014/05/14 通知&PUSH処理追加　START　↓↓↓
        Dim user As SC3180201PicClientDataTable = Nothing

        Select Case inEventKey
            Case EventkeyApproveProces, EventkeyRejectProces
                '承認処理、否認処理　→　依頼者に通知するため、依頼者情報を取得
                user = _
                    tableAdapter.GetPicClient(inStaffInfo.DlrCD, _
                                             inStaffInfo.BrnCD, _
                                             inRoNum, _
                                             inJobDtlId)
            Case EventkeyLastApproveProces
                '最終承認処理　→　SAに通知するため、SA情報を取得
                ' TODO: ★製造中：SAの項目テーブル不明のため、仮としてサービス入庫テーブルのSAスタッフコードを取得
                user = _
                    tableAdapter.GetPicSaStf(inStaffInfo.DlrCD, _
                                             inStaffInfo.BrnCD, _
                                             inRoNum, _
                                             inJobDtlId)
        End Select

        'Dim user As SC3180201PicSaStfDataTable = _
        '    tableAdapter.GetPicSaStf(inStaffInfo.DlrCD, _
        '                             inStaffInfo.BrnCD, _
        '                             inRoNum)
        '2014/05/14 通知&PUSH処理追加　END　　↑↑↑

        'ユーザーステータス取得処理
        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        Dim userdt As SC3180201PicClientRow =
            DirectCast(user.Rows(0), SC3180201PicClientRow)
        'Dim userdt As SC3180201PicSaStfRow =
        '    DirectCast(user.Rows(0), SC3180201PicSaStfRow)
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        '送信先アカウント情報 
        Using account As New XmlAccount

            '受信先のアカウント設定
            '2014/05/16 通知&PUSH処理追加　START　↓↓↓
            account.ToAccount = userdt.ACCOUNT
            'account.ToAccount = userdt.PIC_SA_STF_CD
            '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

            '受信先アカウントログ出力
            '2014/05/16 通知&PUSH処理追加　START　↓↓↓
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "ACCOUNT [{0}] " _
                      , userdt.ACCOUNT))
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '          , "ACCOUNT [{0}] " _
            '          , userdt.PIC_SA_STF_CD))
            '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

            '受信者名設定
            account.ToAccountName = userdt.USERNAME

            '受信者名ログ出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "USERNAME [{0}] " _
                      , userdt.USERNAME))

            '送信先アカウント情報リストに送信先アカウント情報を追加
            accountList.Add(account)

            '2014/05/16 通知&PUSH処理追加　START　↓↓↓
            '送信先アカウントの権限を取得
            inAccountCD = userdt.OPERATIONCODE
            '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return accountList

    End Function

    ''' <summary>
    ''' 通知履歴登録情報作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <param name="inClientCD">送信先（依頼者）権限</param>
    ''' <param name="inTransDic" >遷移先画面取得Dictionary</param>
    ''' <returns>通知履歴登録情報</returns>
    ''' <remarks>2014/05/16　通知＆PUSH処理追加　引数「inClientCD」「inTransDic」を追加</remarks>
    Private Function CreateRequestNotice(ByVal inRowNoticeProcessingInfo As SC3180201NoticeProcessingInfoRow, _
                                         ByVal inStaffInfo As StaffContext, _
                                         ByVal inSaChip As String, _
                                         ByVal inBaserzId As String, _
                                         ByVal inRoNumber As String, _
                                         ByVal inSeqNo As String, _
                                         ByVal inVin As String, _
                                         ByVal inViewMode As String, _
                                         ByVal inJobDtlId As String, _
                                         ByVal inEventKey As EventKeyId, _
                                         ByVal inClientCD As String, _
                                         ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String))) As XmlRequestNotice

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'XmlRequestNoticeのインスタンス
        Using requestNotice As New XmlRequestNotice

            '販売店コード設定
            requestNotice.DealerCode = inStaffInfo.DlrCD

            '店舗コード設定
            requestNotice.StoreCode = inStaffInfo.BrnCD

            'スタッフコード(送信元)設定
            requestNotice.FromAccount = inStaffInfo.Account

            'スタッフ名(送信元)設定
            requestNotice.FromAccountName = inStaffInfo.UserName

            ''顧客種別(リンク制御で使用)
            'Dim customerType As Integer = MessageType.MyCustomer

            '通知履歴にリンクをつけるか判定
            '顧客種別"1"：自社客　かつ　DMSISが存在する場合
            '通知履歴にリンクをつける

            Select Case inEventKey

                Case EventKeyId.CommonProces

                    requestNotice.Message = Space(1)

                Case Else


                    '通知履歴用メッセージ作成設定
                    requestNotice.Message = Me.CreateNoticeRequestMessage(inRowNoticeProcessingInfo _
                                                                          , inEventKey _
                                                                          , CType(inStaffInfo.OpeCD, String) _
                                                                          , inClientCD _
                                                                          , inTransDic)

            End Select

            'セッション設定値設定
            requestNotice.SessionValue = Me.CreateNoticeRequestSession(inRowNoticeProcessingInfo, _
                                                                       inStaffInfo, _
                                                                       inSaChip, _
                                                                       inBaserzId, _
                                                                       inRoNumber, _
                                                                       inSeqNo, _
                                                                       inVin, _
                                                                       inViewMode, _
                                                                       inJobDtlId, _
                                                                       inEventKey, _
                                                                       CType(inStaffInfo.OpeCD, String), _
                                                                       inClientCD, _
                                                                       inTransDic)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return requestNotice

        End Using

    End Function

    ''' <summary>
    ''' 通知履歴用メッセージ作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <param name="inOperationCode">送信元（操作者）権限コード</param>
    ''' <param name="inClientCode">送信先（依頼者）権限コード</param>
    ''' <param name="inTransDic">遷移先画面取得Dictionary</param>
    ''' <returns>通知履歴用メッセージ情報</returns>
    ''' <history>
    ''' </history>
    ''' <remarks>2014/05/16　通知＆PUSH処理追加　引数「inOperationCode」「inClientCode」「inTransDic」追加</remarks>
    Private Function CreateNoticeRequestMessage(ByVal inRowNoticeProcessingInfo As SC3180201NoticeProcessingInfoRow, _
                                                ByVal inEventKey As EventKeyId, _
                                                ByVal inOperationCode As String, _
                                                ByVal inClientCode As String, _
                                                ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String))) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メッセージ
        Dim workMessage As New StringBuilder

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '遷移先画面取得Dictionaryのキーを作成
        Dim TransDicKey As String = CreateTransDicKey(inOperationCode, inClientCode, CType(inEventKey, String))

        '作成したキーが遷移先画面取得Dictionaryしているか
        If Not inTransDic.ContainsKey(TransDicKey) Then
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END, Not Contain Key." _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return Nothing
        End If
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        'メッセージ組立処理

        '通知履歴にリンクをつける
        '自社客のAタグを設定
        'workMessage.Append(MyPageLink)

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '文言設定
        If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyWordNo)) Then
            workMessage.Append(WebWordUtility.GetWord(CType(inTransDic(TransDicKey)(TransDicKeyWordNo), Decimal)))
            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))
        Else
            workMessage.Append(Space(3))
        End If

        ''イベントごとに処置分岐
        'Select Case inEventKey
        '    Case EventKeyId.AproveProces
        '        '承認処理

        '        '文言：承認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id50))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(1))

        '    Case EventKeyId.LastAproveProces

        '        '文言：最終承認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id51))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(1))

        '    Case EventKeyId.RejectProces
        '        '否認処理

        '        '文言：否認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id52))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(1))

        'End Select
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        'メッセージ組立：RO番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.RO_NUM) Then

            Dim RO_LinkID As String = String.Empty

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyRoNoURL)) Then
                'R/OプレビューのAタグを設定
                '2014/06/24　セッション情報作成処理変更　START　↓↓↓
                'Dim RO_LinkID As String = String.Empty
                If inTransDic(TransDicKey)(TransDicKeyRoNoURL).Contains(PageIdSC3010501 & "-") Then
                    RO_LinkID = PageIdSC3010501
                Else
                    RO_LinkID = inTransDic(TransDicKey)(TransDicKeyRoNoURL)
                End If

                '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
                '顧客詳細画面（SC3080225）で顧客IDが無い時はリンクIDを空白にする
                If RO_LinkID = PageIdSC3080225 Then
                    If String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMS_CST_CD.Trim) Then
                        RO_LinkID = ""
                    End If
                End If

                'リンクIDがあればAタグを追加する
                If RO_LinkID <> "" Then
                    workMessage.Append(String.Format(RoPreviewLink, RO_LinkID))
                End If
                '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑

                'workMessage.Append(String.Format(RoPreviewLink, inTransDic(TransDicKey)(TransDicKeyRoNoURL)))
                '2014/06/24　セッション情報作成処理変更　END　　↑↑↑
            End If
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

            'RO番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.RO_NUM)

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
            'If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyRoNoURL)) Then
            If RO_LinkID <> "" Then
                'Aタグ終了を設定
                workMessage.Append(EndLikTag)
            End If
            '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))

        End If

        'メッセージ組立：REG番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.REG_NO) Then

            Dim Reg_LinkID As String = String.Empty

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyRegNoURL)) Then
                '車両情報のAタグを設定
                '2014/06/24　セッション情報作成処理変更　START　↓↓↓
                'Dim Reg_LinkID As String = String.Empty
                If inTransDic(TransDicKey)(TransDicKeyRegNoURL).Contains(PageIdSC3010501 & "-") Then
                    Reg_LinkID = PageIdSC3010501
                Else
                    Reg_LinkID = inTransDic(TransDicKey)(TransDicKeyRegNoURL)
                End If

                '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
                '顧客詳細画面（SC3080225）で顧客IDが無い時はリンクIDを空白にする
                If Reg_LinkID = PageIdSC3080225 Then
                    If String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMS_CST_CD.Trim) Then
                        Reg_LinkID = ""
                    End If
                End If

                'リンクIDがあればAタグを追加する
                If Reg_LinkID <> "" Then
                    workMessage.Append(String.Format(CustomerVclLink, Reg_LinkID))
                End If
                '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑

                'workMessage.Append(String.Format(CustomerCstLink, inTransDic(TransDicKey)(TransDicKeyRegNoURL)))
                '2014/06/24　セッション情報作成処理変更　END　　↑↑↑
            End If
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

            'REG番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.REG_NO)

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
            'If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyRegNoURL)) Then
            If Reg_LinkID <> "" Then
                'Aタグ終了を設定
                workMessage.Append(EndLikTag)
            End If
            '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))

        End If

        'メッセージ組立：お客様名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.CST_NAME) Then
            'お客様名がある場合

            Dim Cutomer_LinkID As String = String.Empty

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            '顧客詳細のAタグを設定
            If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyCutomerURL)) Then
                '2014/06/24　セッション情報作成処理変更　START　↓↓↓
                'Dim Cutomer_LinkID As String = String.Empty
                If inTransDic(TransDicKey)(TransDicKeyCutomerURL).Contains(PageIdSC3010501 & "-") Then
                    Cutomer_LinkID = PageIdSC3010501
                Else
                    Cutomer_LinkID = inTransDic(TransDicKey)(TransDicKeyCutomerURL)
                End If

                '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
                '顧客詳細画面（SC3080225）で顧客IDが無い時はリンクIDを空白にする
                If Cutomer_LinkID = PageIdSC3080225 Then
                    If String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMS_CST_CD.Trim) Then
                        Cutomer_LinkID = ""
                    End If
                End If

                'リンクIDがあればAタグを追加する
                If Cutomer_LinkID <> "" Then
                    workMessage.Append(String.Format(CustomerCstLink, Cutomer_LinkID))
                End If
                '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑

                'workMessage.Append(String.Format(CustomerVclLink, inTransDic(TransDicKey)(TransDicKeyCutomerURL)))
                '2014/06/24　セッション情報作成処理変更　END　　↑↑↑
            End If
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

            '敬称利用区分チェック
            If PositionTypeBack.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を後方につける

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

                'メッセージ間にスペースの設定
                'workMessage.Append(Space(1))

            ElseIf PositionTypeFront.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を前方につける

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

                'メッセージ間にスペースの設定
                'workMessage.Append(Space(1))

            Else
                '上記以外の場合

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

            End If

            '2014/05/13 通知&PUSH処理追加　START　↓↓↓
            '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
            'If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyCutomerURL)) Then
            If Cutomer_LinkID <> "" Then
                'Aタグ終了を設定
                workMessage.Append(EndLikTag)
            End If
            '2014/08/06 顧客IDが無い時はリンクを作成しない　END　　↑↑↑
            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))
            '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

        Else
            'お客様名がない場合

            '文言：お客様 設定
            workMessage.Append(Space(1))

        End If

        'メッセージ組立：商品名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.MERCHANDISENAME) Then

            '商品名を設定
            workMessage.Append(inRowNoticeProcessingInfo.MERCHANDISENAME)

            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))

        End If

        '通知履歴にリンクをつける
        'Aタグ終了を設定
        'workMessage.Append(EndLikTag)

        '戻り値設定
        Dim notifyMessage As String = workMessage.ToString().TrimEnd

        '送信メッセージログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "MESSAGE [{0}]" _
            , notifyMessage))


        '開放処理
        workMessage = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END MESSAGE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , notifyMessage))

        Return notifyMessage

    End Function

    ''' <summary>
    ''' 通知履歴用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateNoticeRequestSession(ByVal inRowNoticeProcessingInfo As SC3180201NoticeProcessingInfoRow, _
                                                ByVal inStaffInfo As StaffContext, _
                                                ByVal inSaChip As String, _
                                                ByVal inBaserzId As String, _
                                                ByVal inRoNumber As String, _
                                                ByVal inSeqNo As String, _
                                                ByVal inVin As String, _
                                                ByVal inViewMode As String, _
                                                ByVal inJobDtlId As String, _
                                                ByVal inEventKey As EventKeyId, _
                                                ByVal inOperationCode As String, _
                                                ByVal inClientCode As String, _
                                                ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String))) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim notifySession As String = String.Empty

        '2014/06/24　セッション情報作成処理変更　START　↓↓↓
        ''通知用セッション情報作成処理（顧客詳細用）
        'notifySession = CreateCustomerSession(inRowNoticeProcessingInfo, _
        '                                      inStaffInfo, _
        '                                      inSaChip, _
        '                                      inBaserzId, _
        '                                      inRoNumber, _
        '                                      inSeqNo, _
        '                                      inVin, _
        '                                      inViewMode, _
        '                                      inJobDtlId)

        '遷移先画面取得Dictionaryのキーを作成
        Dim TransDicKey As String = CreateTransDicKey(inOperationCode, inClientCode, CType(inEventKey, String))

        '作成したキーが遷移先画面取得Dictionaryしているか確認する
        If Not inTransDic.ContainsKey(TransDicKey) Then
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END, Not Contain Key." _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return Nothing
        End If
        'RO番号、車両番号、顧客名の遷移先ページIDを取得する
        Dim LinkPageIds(2) As String
        LinkPageIds(0) = inTransDic(TransDicKey)(TransDicKeyRoNoURL)
        LinkPageIds(1) = inTransDic(TransDicKey)(TransDicKeyRegNoURL)
        LinkPageIds(2) = inTransDic(TransDicKey)(TransDicKeyCutomerURL)

        Dim workNotifySession As New StringBuilder

        For Each LinkPageId As String In LinkPageIds
            If Not String.IsNullOrEmpty(LinkPageId) Then
                'ページIDがSC3010501か確認
                If LinkPageId.Contains(PageIdSC3010501 & "-") Then
                    'ページIDがSC3010501
                    'DispNumを取り出す
                    Dim DispNum As String = Replace(LinkPageId, PageIdSC3010501 & "-", "")
                    'DispNumにあわせてセッションキーを設定する
                    Select Case DispNum
                        Case "13"
                            '通知用セッション情報作成処理（ROプレビュー用）
                            workNotifySession.Append(CreateRoPreviewSession(inRowNoticeProcessingInfo, _
                                                                            inStaffInfo, _
                                                                            inSaChip, _
                                                                            inBaserzId, _
                                                                            inRoNumber, _
                                                                            inSeqNo, _
                                                                            inVin, _
                                                                            inViewMode, _
                                                                            inJobDtlId, _
                                                                            DispNum))
                        Case Else
                            'その他
                    End Select
                    'ElseIf LinkPageId = "None" Then
                    '    workNotifySession.Append("None")
                ElseIf LinkPageId.Contains(PageIdSC3080225) Then
                    '2014/07/16　セッション情報作成処理変更　START　↓↓↓
                    'ページIDがSC3080225（顧客詳細画面）
                    '通知用セッション情報作成処理
                    '2014/08/06 顧客IDが無い時はリンクを作成しない　START　↓↓↓
                    If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMS_CST_CD.Trim) Then
                        workNotifySession.Append(CreateCustomerSession(inRowNoticeProcessingInfo, _
                                                                       inStaffInfo, _
                                                                       inSaChip, _
                                                                       inBaserzId, _
                                                                       inRoNumber, _
                                                                       inSeqNo, _
                                                                       inVin, _
                                                                       inViewMode, _
                                                                       inJobDtlId))
                    End If
                    '2014/08/06 顧客IDが無い時はリンクを作成しない　END　↑↑↑
                Else
                    'ページIDがSC3010501/SC3080225以外
                    '通知用セッション情報作成処理
                    workNotifySession.Append(CreateOtherSession(inRowNoticeProcessingInfo, _
                                                                   inStaffInfo, _
                                                                   inSaChip, _
                                                                   inBaserzId, _
                                                                   inRoNumber, _
                                                                   inSeqNo, _
                                                                   inVin, _
                                                                   inViewMode, _
                                                                   inJobDtlId))

                End If
                '2014/07/16　セッション情報作成処理変更　END　　↑↑↑

            End If
            'タブで分ける
            workNotifySession.Append(vbTab)
        Next


        notifySession = workNotifySession.ToString
        '2014/06/24　セッション情報作成処理変更　END　　↑↑↑

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return notifySession
    End Function

    '2014/07/16　セッション情報作成処理変更　START　↓↓↓
    ''' <summary>
    ''' 通知用セッション情報作成メソッド（SC3080225　顧客詳細画面）
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateCustomerSession(ByVal inRowNoticeProcessingInfo As SC3180201NoticeProcessingInfoRow, _
                                           ByVal inStaffInfo As StaffContext, _
                                           ByVal inSaChip As String, _
                                           ByVal inBaserzId As String, _
                                           ByVal inRoNumber As String, _
                                           ByVal inSeqNo As String, _
                                           ByVal inVin As String, _
                                           ByVal inViewMode As String, _
                                           ByVal inJobDtlId As String) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder


        '基幹顧客IDのセッション設定
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMS_CST_CD.Trim) Then
            '基幹顧客IDがある場合は設定
            '基幹顧客IDのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueDmsCstId, inRowNoticeProcessingInfo.DMS_CST_CD.Trim)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueDmsCstId, "")

        End If

        'VINの設定
        If Not String.IsNullOrEmpty(inVin.Trim) Then
            'VINがある場合は設定
            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyVin, inVin.Trim)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionKeyVin, "")

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession.ToString

    End Function
    '2014/07/16　セッション情報作成処理変更　END　　↑↑↑

    '2014/06/24　セッション情報作成処理変更　START　↓↓↓
    ''' <summary>
    ''' ROプレビュー遷移の通知用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateRoPreviewSession(ByVal inRowNoticeProcessingInfo As SC3180201NoticeProcessingInfoRow, _
                                           ByVal inStaffInfo As StaffContext, _
                                           ByVal inSaChip As String, _
                                           ByVal inBaserzId As String, _
                                           ByVal inRoNumber As String, _
                                           ByVal inSeqNo As String, _
                                           ByVal inVin As String, _
                                           ByVal inViewMode As String, _
                                           ByVal inJobDtlId As String, _
                                           ByVal DispNum As String) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder

        Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = Me.GetDmsDealerData(inStaffInfo)

        'DMS情報のチェック
        If Not (IsNothing(dtDmsCodeMapDataTable)) Then
            '取得できた場合
            'DMS販売店コードのセッション設定
            Me.SetSessionValueWord(workSession, SessionValueDearlerCode, dtDmsCodeMapDataTable(0).CODE1)
            'DMS店舗コードのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueBranchCode, dtDmsCodeMapDataTable(0).CODE2)
        Else
            '取得できなかった場合
            'DMS販売店コードのセッション設定
            Me.SetSessionValueWord(workSession, SessionValueDearlerCode, inStaffInfo.DlrCD)
            'DMS店舗コードのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueBranchCode, inStaffInfo.BrnCD)
        End If

        'LoginUserIDのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueLoginUserID, inStaffInfo.Account)

        '来店管理番号のセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSAChipID, inSaChip)


        'BASREZIDの設定
        If Not String.IsNullOrEmpty(inBaserzId) Then
            'BASREZIDがある場合は設定
            'BASREZIDのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueBASREZID, inBaserzId)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueBASREZID, "")
        End If

        'R_Oの設定
        If Not String.IsNullOrEmpty(inRoNumber) Then
            'R_Oがある場合は設定

            'R_Oのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueR_O, inRoNumber)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueR_O, "")
        End If

        'SEQ_NOの設定
        If Not String.IsNullOrEmpty(SessionValueSEQ_NO) Then
            'SEQ_NOがある場合は設定

            'SEQ_NOのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueSEQ_NO, inSeqNo)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueSEQ_NO, "")
        End If

        'VIN_NOの設定
        If Not String.IsNullOrEmpty(inVin) Then
            'VIN_NOのセッション値作成
            Me.SetSessionValueWord(workSession, SessionValueVIN_NO, inVin)
        Else
            '値がない場合は空文字を設定
            Me.SetSessionValueWord(workSession, SessionValueVIN_NO, "")
        End If

        'ViewModeのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueViewMode, "0")

        'Formatのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueFormat, "0")

        '入庫管理番号のセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSvcInNum, "")

        '入庫販売店コードのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSvcInDlrCd, "")

        '入庫店舗コードのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueSvcInBrnCd, "")

        'DISP_NUMのセッション値作成
        Me.SetSessionValueWord(workSession, SessionValueDisp_Num, DispNum)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession.ToString

    End Function
    '2014/06/24　セッション情報作成処理変更　END　　↑↑↑

    '2014/07/16　セッション情報作成処理変更　START　↓↓↓
    ''' <summary>
    ''' 通知用セッション情報作成メソッド（その他）
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="inSaChip">   </param>
    ''' <param name="inBaserzId">   </param>
    ''' <param name="inRoNumber">Ro番号</param>
    ''' <param name="inSeqNo">   </param>
    ''' <param name="inVin">   </param>
    ''' <param name="inViewMode">   </param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateOtherSession(ByVal inRowNoticeProcessingInfo As SC3180201NoticeProcessingInfoRow, _
                                           ByVal inStaffInfo As StaffContext, _
                                           ByVal inSaChip As String, _
                                           ByVal inBaserzId As String, _
                                           ByVal inRoNumber As String, _
                                           ByVal inSeqNo As String, _
                                           ByVal inVin As String, _
                                           ByVal inViewMode As String, _
                                           ByVal inJobDtlId As String) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder

        'DEARLERCODEの設定
        If Not String.IsNullOrEmpty(inStaffInfo.DlrCD.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyDealerCode, inStaffInfo.DlrCD.Trim)

        End If

        'BRANCHCODEの設定
        If Not String.IsNullOrEmpty(inStaffInfo.BrnCD.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyBranchCode, inStaffInfo.BrnCD.Trim)

        End If

        'ACCOUNTの設定
        If Not String.IsNullOrEmpty(inStaffInfo.Account.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyAccount, inStaffInfo.Account.Trim)

        End If

        'RO_NUMの設定
        If Not String.IsNullOrEmpty(inRoNumber.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyRepairorder, inRoNumber.Trim)

        End If

        'SEQ_NOの設定
        If Not String.IsNullOrEmpty(inSeqNo.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeySequence, inSeqNo.Trim)

        End If

        'JOB_DTL_IDの設定
        If Not String.IsNullOrEmpty(inJobDtlId.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyJobDtlId, inJobDtlId.Trim)

        End If

        'VINの設定
        If Not String.IsNullOrEmpty(inVin.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyVin, inVin.Trim)

        End If

        'VIEWMODEの設定
        If Not String.IsNullOrEmpty(inViewMode.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyViewMode, inViewMode.Trim)

        End If

        'SACHIPの設定
        If Not String.IsNullOrEmpty(inSaChip.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyVistSequence, inSaChip.Trim)

        End If

        'BASERZIDの設定
        If Not String.IsNullOrEmpty(inBaserzId.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionKeyResrveId, inBaserzId.Trim)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession.ToString

    End Function
    '2014/06/24　セッション情報作成処理変更　END　　↑↑↑

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
                                         ByVal SessionValueData As String) As StringBuilder

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'カンマの設定
        If workSession.Length <> 0 Then
            'データがある場合

            '「,」を結合する
            workSession.Append(SessionValueKanma)

        End If

        'セッションキーを設定
        workSession.Append(SessionValueWord)

        'セッション値を設定
        workSession.Append(SessionValueData)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession

    End Function

    ''' <summary>
    ''' Push情報作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <param name="inOperationCode">送信元（操作者）権限コード</param>
    ''' <param name="inClientCode">送信先（依頼者）権限コード</param>
    ''' <param name="inTransDic">遷移先画面取得Dictionary</param>
    ''' <returns>Push情報</returns>
    ''' <remarks>2014/05/16　通知＆PUSH処理追加　引数「inOperationCode」「inClientCode」「inTransDic」追加</remarks>
    Private Function CreatePushInfo(ByVal inRowNoticeProcessingInfo As SC3180201NoticeProcessingInfoRow, _
                                    ByVal inEventKey As EventKeyId, _
                                    ByVal inOperationCode As String, _
                                    ByVal inClientCode As String, _
                                    ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String))) As XmlPushInfo

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

            Select Case inEventKey

                Case EventKeyId.CommonProces

                    'Push用メッセージ作成
                    pushInfo.DisplayContents = Space(3)

                Case EventKeyId.AproveProces, EventKeyId.LastAproveProces, EventKeyId.RejectProces

                    'Push用メッセージ作成
                    pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo, inEventKey, inOperationCode, inClientCode, inTransDic)

                    'Case EventKeyId.LastAproveProces

                    '    'Push用メッセージ作成
                    '    pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo, inEventKey)

                    'Case EventKeyId.RejectProces

                    '    'Push用メッセージ作成
                    '    pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo, inEventKey)

            End Select

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
    ''' Push用メッセージ作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <param name="inOperationCode">送信元（操作者）権限コード</param>
    ''' <param name="inClientCode">送信先（依頼者）権限コード</param>
    ''' <param name="inTransDic">遷移先画面取得Dictionary</param>
    ''' <returns>Puss用メッセージ文言</returns>
    ''' <history>
    ''' </history>
    ''' <remarks>2014/05/16　通知＆PUSH処理追加　引数「inOperationCode」「inClientCode」「inTransDic」追加</remarks>
    Private Function CreatePusuMessage(ByVal inRowNoticeProcessingInfo As SC3180201NoticeProcessingInfoRow, _
                                       ByVal inEventKey As EventKeyId, _
                                       ByVal inOperationCode As String, _
                                       ByVal inClientCode As String, _
                                       ByVal inTransDic As Dictionary(Of String, Dictionary(Of String, String))) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        '遷移先画面取得Dictionaryのキーを作成
        Dim TransDicKey = CreateTransDicKey(inOperationCode, inClientCode, CType(inEventKey, String))

        '作成したキーが遷移先画面取得Dictionaryしているか
        If Not inTransDic.ContainsKey(TransDicKey) Then
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END, Not Contain Key." _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return Nothing
        End If
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑

        'メッセージ
        Dim workMessage As New StringBuilder

        'メッセージ組立処理

        '2014/05/16 通知&PUSH処理追加　START　↓↓↓
        If Not String.IsNullOrWhiteSpace(inTransDic(TransDicKey)(TransDicKeyWordNo)) Then
            workMessage.Append(WebWordUtility.GetWord(CType(inTransDic(TransDicKey)(TransDicKeyWordNo), Decimal)))
            'メッセージ間にスペースの設定
            workMessage.Append(Space(1))
        Else
            workMessage.Append(Space(3))
        End If

        ''イベントごとに処置分岐
        'Select Case inEventKey
        '    Case EventKeyId.CommonProces
        '        '共通処理

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(3))


        '    Case EventKeyId.AproveProces
        '        '承認処理

        '        '文言：承認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id50))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(3))

        '    Case EventKeyId.LastAproveProces

        '        '文言：最終承認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id51))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(3))

        '    Case EventKeyId.RejectProces
        '        '否認処理

        '        '文言：否認 設定
        '        workMessage.Append(WebWordUtility.GetWord(MsgID.id52))

        '        'メッセージ間にスペースの設定
        '        workMessage.Append(Space(3))

        'End Select
        '2014/05/16 通知&PUSH処理追加　END　　↑↑↑


        'メッセージ組立：RO番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.RO_NUM) Then

            'RO番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.RO_NUM)

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If

        'メッセージ組立：REG番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.REG_NO) Then

            'REG番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.REG_NO)

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If


        'メッセージ組立：お客様名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.CST_NAME) Then
            'お客様名がある場合

            '敬称利用区分チェック
            If PositionTypeBack.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を後方につけつ

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

            ElseIf PositionTypeFront.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を前方につける

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

            Else
                '上記以外の場合

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.CST_NAME)

            End If

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        Else
            'お客様名がない場合
            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If

        'メッセージ組立：商品名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.MERCHANDISENAME) Then
            '商品名がある場合

            '商品名を設定
            workMessage.Append(inRowNoticeProcessingInfo.MERCHANDISENAME)

        End If


        '戻り値設定
        Dim notifyMessage As String = workMessage.ToString().TrimEnd


        '開放処理
        workMessage = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END MESSAGE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , notifyMessage))

        Return notifyMessage

    End Function

#End Region

#Region "ログデータ加工処理"

    ''' <summary>
    ''' ログデータ（メソッド）
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="startEndFlag">True：「method start」を表示、False：「method end」を表示</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function GetLogMethod(ByVal methodName As String,
                                  ByVal startEndFlag As Boolean) As String
        Dim sb As New StringBuilder
        With sb
            .Append("[")
            .Append(methodName)
            .Append("]")
            If startEndFlag Then
                .Append(" method start")
            Else
                .Append(" method end")
            End If
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（引数）
    ''' </summary>
    ''' <param name="paramName">引数名</param>
    ''' <param name="paramData">引数値</param>
    ''' <param name="kanmaFlag">True：引数名の前に「,」を表示、False：特になし</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function GetLogParam(ByVal paramName As String,
                                 ByVal paramData As String,
                                 ByVal kanmaFlag As Boolean) As String
        Dim sb As New StringBuilder
        With sb
            If kanmaFlag Then
                .Append(",")
            End If
            .Append(paramName)
            .Append("=")
            .Append(paramData)
        End With
        Return sb.ToString
    End Function

#End Region

#Region "XmlNoticeDataログデータ加工処理"

    ''' <summary>
    ''' XmlNoticeDataログデータ加工処理
    ''' </summary>
    ''' <param name="xmlNoticeData">XmlNoticeDataクラス</param>
    ''' <returns>ログ情報</returns>
    ''' <remarks></remarks>
    Private Function LogNoticeData(ByVal xmlNoticeData As XmlNoticeData) As String

        Dim log As New StringBuilder

        With log
            '見やすくするために改行
            .AppendLine("")
            .AppendLine("000･･･")
            .AppendLine("<TransmissionDate>" & CStr(xmlNoticeData.TransmissionDate))
            .AppendLine("100･･･")
            For Each accountData In xmlNoticeData.AccountList
                .AppendLine("<ToAccount>" & accountData.ToAccount)
                .AppendLine("<ToClientID>" & accountData.ToClientId)
                .AppendLine("<ToAccountName>" & accountData.ToAccountName)
            Next
            .AppendLine("200･･･")
            .AppendLine("<DealerCode>" & xmlNoticeData.RequestNotice.DealerCode)
            .AppendLine("<StoreCode>" & xmlNoticeData.RequestNotice.StoreCode)
            .AppendLine("<RequestClass>" & xmlNoticeData.RequestNotice.RequestClass)
            .AppendLine("<Status>" & xmlNoticeData.RequestNotice.Status)
            .AppendLine("<RequestID>" & xmlNoticeData.RequestNotice.RequestId)
            .AppendLine("<RequestClassID>" & xmlNoticeData.RequestNotice.RequestClassId)
            .AppendLine("<FromAccount>" & xmlNoticeData.RequestNotice.FromAccount)
            .AppendLine("<FromClientID>" & xmlNoticeData.RequestNotice.FromClientId)
            .AppendLine("<FromAccountName>" & xmlNoticeData.RequestNotice.FromAccountName)
            .AppendLine("<CustomID>" & xmlNoticeData.RequestNotice.CustomId)
            .AppendLine("<CustomName>" & xmlNoticeData.RequestNotice.CustomName)
            .AppendLine("<CustomerClass>" & xmlNoticeData.RequestNotice.CustomerClass)
            .AppendLine("<CstKind>" & xmlNoticeData.RequestNotice.CustomerKind)
            .AppendLine("<Message>" & xmlNoticeData.RequestNotice.Message)
            .AppendLine("<SessionValue>" & xmlNoticeData.RequestNotice.SessionValue)
            .AppendLine("<SalesStaffCode>" & xmlNoticeData.RequestNotice.SalesStaffCode)
            .AppendLine("<VehicleSequenceNumber>" & xmlNoticeData.RequestNotice.VehicleSequenceNumber)
            .AppendLine("<FollowUpBoxStoreCode>" & xmlNoticeData.RequestNotice.FollowUpBoxStoreCode)
            .AppendLine("<FollowUpBoxNumber>" & xmlNoticeData.RequestNotice.FollowUpBoxNumber)
            ' $01 start step2開発
            .AppendLine("<CSPaperName>" & xmlNoticeData.RequestNotice.CSPaperName)
            ' $01 end   step2開発
            .AppendLine("300･･･")
            If Not IsNothing(xmlNoticeData.PushInfo) Then
                .AppendLine("<PushCategory>" & xmlNoticeData.PushInfo.PushCategory)
                .AppendLine("<PositionType>" & xmlNoticeData.PushInfo.PositionType)
                .AppendLine("<Time>" & xmlNoticeData.PushInfo.Time)
                .AppendLine("<DisplayType>" & xmlNoticeData.PushInfo.DisplayType)
                .AppendLine("<DisplayContents>" & xmlNoticeData.PushInfo.DisplayContents)
                .AppendLine("<Color>" & xmlNoticeData.PushInfo.Color)
                .AppendLine("<PopWidth>" & xmlNoticeData.PushInfo.PopWidth)
                .AppendLine("<PopHeight>" & xmlNoticeData.PushInfo.PopHeight)
                .AppendLine("<PopX>" & xmlNoticeData.PushInfo.PopX)
                .AppendLine("<PopY>" & xmlNoticeData.PushInfo.PopY)
                .AppendLine("<DisplayFunction>" & xmlNoticeData.PushInfo.DisplayFunction)
                .AppendLine("<ActionFunction>" & xmlNoticeData.PushInfo.ActionFunction)
            End If
        End With
        Return log.ToString
    End Function

#End Region

    '2014/05/13 通知&PUSH処理追加　START　↓↓↓
#Region "Push送信（メイン画面リフレッシュ）"

    ''' <summary>
    ''' PUSH処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <returns>終了コード</returns>
    ''' <remarks>PUSH処理（メイン画面リフレッシュ）を送信する</remarks>
    <EnableCommit()>
    Public Function SendGateNotice(ByVal dealerCode As String _
                                   , ByVal storeCode As String _
                                   , ByVal operationCdList As List(Of Decimal) _
                                   , ByVal pushMethod As String) As Integer
        'SendGateNotice開始ログ出力
        Dim sendGateNoticeStartLogInfo As New StringBuilder
        sendGateNoticeStartLogInfo.Append("SendGateNotice_Start ")
        sendGateNoticeStartLogInfo.Append("dealerCode[" & dealerCode & "]")
        sendGateNoticeStartLogInfo.Append(",storeCode[" & storeCode & "]")
        sendGateNoticeStartLogInfo.Append(",pushMethod[" & pushMethod & "]")
        Logger.Info(sendGateNoticeStartLogInfo.ToString())

        '終了コード
        Dim resultId As Integer = MessageIdSuccess

        'Logger.Info("SendGateNotice_001 " & "MasterCheck_Start")

        ''マスターチェック
        'resultId = IsVaildMaster(dealerCode, storeCode)

        'If resultId <> MessageIdSuccess Then

        '    Logger.Info("SendGateNotice_002 " & "IsVaildMaster NG")

        '    'エラー出力
        '    Logger.Warn("ResultId : " & CStr(resultId))

        '    'チェックに引っかかっていたら返却
        '    Logger.Error("SendGateNotice_End Ret[" & CStr(resultId) & "]")
        '    Return resultId
        'End If

        'Logger.Info("SendGateNotice_003 " & "IsVaildMaster OK")

        'ユーザマスタから販売店コード、店舗コード、権限リストを条件にスタッフ情報を取得
        Dim users As Users = New Users
        'Dim operationCdList As New List(Of Decimal)

        ''OperationCodeリストに権限"55"：CTを設定
        'operationCdList.Add(Operation.CT)

        ''OperationCodeリストに権限"62"：CHTを設定
        'operationCdList.Add(Operation.CHT)

        'userData取得ログ出力
        Dim userDataStartLogInfo As New StringBuilder
        userDataStartLogInfo.Append("SendGateNotice_004 " & "Call_Start users.GetAllUser ")
        userDataStartLogInfo.Append("param1[" & dealerCode & "]")
        userDataStartLogInfo.Append(",param2[" & storeCode & "]")
        userDataStartLogInfo.Append(",param3[" & operationCdList.Item(0).ToString(CultureInfo.InvariantCulture()) & "]")
        Logger.Info(userDataStartLogInfo.ToString())

        '販売店コード、店舗コード、権限リストを元にスタッフ情報を取得
        Dim userData As UsersDataSet.USERSDataTable = users.GetAllUser(dealerCode, storeCode, operationCdList)

        'userData取得ログ出力
        Dim userDataEndLogInfo As New StringBuilder
        userDataEndLogInfo.Append("SendGateNotice_004 " & "Call_End users.GetAllUser ")
        userDataEndLogInfo.Append("Ret[" & userData.ToString & "]")
        Logger.Info(userDataEndLogInfo.ToString())

        'スタッフ情報チェック
        If userData.Count = 0 Then

            userDataStartLogInfo.Append("SendGateNotice_005 NotStaffInfo")

            'スタッフ情報が0件
            resultId = MessageIdAccountInfoIsNull

            'エラー出力
            Logger.Warn("ResultId : " & CStr(resultId))

            Logger.Error("SendGateNotice_End Ret[" & CStr(resultId) & "]")
            Return resultId
        End If

        'マスタチェック終了
        Logger.Info("SendGateNotice_006 MasterCheck_End")

        ''デバッグログ出力(来店日時取得開始)
        'Logger.Info("SendGateNotice_007 " & "Call_Start DateTimeFunc.Now Param[" & dealerCode & "]")

        ''日付管理機能から来店日時(現在日時)を販売店コードを元に取得
        'Dim visitTimeStamp As Date = DateTimeFunc.Now(dealerCode)

        ''デバッグログ出力(来店日時取得終了)
        'Logger.Info("SendGateNotice_008 " & "Call_End DateTimeFunc.Now Ret[" & visitTimeStamp & "]")

        'CTとCHTスタッフのアカウントを取得
        For Each target As UsersDataSet.USERSRow In userData

            'Push機能にて、スタッフ端末へ、ゲート通知送信命令を送信
            SendGatePush(target.ACCOUNT, pushMethod)
        Next

        '終了デバッグログ出力
        Dim sendGateNoticeEndLogInfo As New StringBuilder
        sendGateNoticeEndLogInfo.Append("SendGateNotice_End ")
        sendGateNoticeEndLogInfo.Append("Ret[" & CStr(resultId) & "]")
        Logger.Info(sendGateNoticeEndLogInfo.ToString())

        Return resultId
    End Function

#End Region

#Region "マスターデータチェック"

    ''' <summary>
    ''' マスターデータチェックメソッド
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="strCd">店舗コード</param>
    ''' <returns>チェック結果を終了コードで返却</returns>
    ''' <remarks></remarks>
    Private Function IsVaildMaster(ByVal dlrCd As String, ByVal strCd As String) As Integer

        'マスタチェック開始
        Dim startLogMaster As New StringBuilder
        startLogMaster.Append("IsVaildMaster_Start ")
        startLogMaster.Append("param1[" & dlrCd & "]")
        startLogMaster.Append(",param2[" & strCd & "]")
        Logger.Info(startLogMaster.ToString())

        '販売店コードの存在チェック
        Logger.Info("IsVaildMaster_001 Call_Start dealers.GetDealer Param[" & dlrCd & "]")
        Dim dealers As Dealer = New Dealer
        Dim dealerData As DealerDataSet.DEALERRow = dealers.GetDealer(dlrCd)

        '指定した販売店コードが取れなかった場合
        If dealerData Is Nothing Then

            Logger.Error("IsVaildMaster_002  dealerData Is Nothing")

            '終了ログ
            Logger.Error("IsVaildMaster_End Ret[" & MessageIdDealerInfoIsNull & "]")
            Return MessageIdDealerInfoIsNull
        End If
        Logger.Info("IsVaildMaster_001 Call_End dealers.GetDealer Ret[" & dealerData.ToString & "]")

        ''店舗コードの存在チェック
        'Dim storesLogMaster As New StringBuilder
        'storesLogMaster.Append("IsVaildMaster_003 Call_Start stores.GetBranch ")
        'storesLogMaster.Append("param1[" & dlrCd & "]")
        'storesLogMaster.Append(",param2[" & strCd & "]")
        'Logger.Info(storesLogMaster.ToString())
        'Dim stores As Branch = New Branch
        'Dim storesData As BranchDataSet.BRANCHRow = stores.GetBranch(dlrCd, strCd)

        ''指定した店舗コードが取れなかった場合
        'If storesData Is Nothing Then

        '    Logger.Error("IsVaildMaster_004  storesData Is Nothing")

        '    '終了ログ
        '    Logger.Error("IsVaildMaster_End Ret[" & MessageIdBranchInfoIsNull & "]")
        '    Return MessageIdBranchInfoIsNull
        'End If
        'Logger.Error("IsVaildMaster_003 Call_End stores.GetBranch Ret[" & storesData.ToString & "]")


        '終了ログ
        Logger.Info("IsVaildMaster_End Ret[" & MessageIdSuccess & "]")
        Return MessageIdSuccess
    End Function

#End Region

#Region "Push送信実行"

    ''' <summary>
    ''' PUSH処理（画面リフレッシュ）を送信
    ''' </summary>
    ''' <param name="accountCd">アカウント</param>
    ''' <remarks>push送信を行う</remarks>
    Private Sub SendGatePush(ByVal accountCd As String, ByVal pushMethod As String)

        'デバッグログ出力(PUSH開始)
        Dim sendGatePushStartLogInfo As New StringBuilder
        sendGatePushStartLogInfo.Append("SendGatePush_Start ")
        sendGatePushStartLogInfo.Append("param1[" & accountCd & "]")
        sendGatePushStartLogInfo.Append(", pushMethod[" & pushMethod & "]")
        Logger.Info(sendGatePushStartLogInfo.ToString())

        'POST送信する文字列を作成する
        Dim postMsg As New StringBuilder
        With postMsg
            .Append("cat=action")
            .Append("&type=main")
            .Append("&sub=js")
            .Append("&uid=" & accountCd)
            .Append("&time=0")
            '.Append("&js1=sc3090301pushRecv()")
            .Append(String.Format("&js1={0}", pushMethod))
        End With

        'Push送信を行う
        Dim util As New VisitUtility
        util.SendPush(postMsg.ToString)

        'デバッグログ出力(PUSH終了)
        Logger.Info("SendGatePush_End")

    End Sub
#End Region

#Region "遷移先画面取得Dictionary"

    ''' <summary>
    ''' 遷移先画面取得Dictionaryを作成する
    ''' </summary>
    ''' <remarks>遷移先画面取得Dictionaryを作成する</remarks>
    Private Function CreateScreenTransitionDictionary() _
                                              As Dictionary(Of String, Dictionary(Of String, String))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))


        Dim ScreenTransDic As Dictionary(Of String, Dictionary(Of String, String)) = New Dictionary(Of String, Dictionary(Of String, String))
        'TODO: ★製造中：文言DB番号はあらかじめソース上で指定されていた番号、遷移先URLはTabletSMBCommonClassBusinessLogicよりそのまま入れてあります。
        '送信元：送信先：操作
        ' FM： TC：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.TEC, EventkeyApproveProces), _
                           TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        ' FM：ChT：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.CHT, EventkeyApproveProces), _
                           TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        ' FM： SA：最終承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.SA, EventkeyLastApproveProces), _
                           TransDicValue(CType(MsgID.id51, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSAMain)) '2014/11/26 起票SA個人へのPush通知追加
        ' FM： FM：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.FM, EventkeyApproveProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        ' FM： CT：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.CT, EventkeyApproveProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))
        ' FM： TC：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.TEC, EventkeyRejectProces), _
                           TransDicValue(CType(MsgID.id52, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        ' FM：ChT：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.CHT, EventkeyRejectProces), _
                           TransDicValue(CType(MsgID.id52, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        ' FM： FM：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.FM, EventkeyRejectProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        ' FM： CT：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.FM, Operation.CT, EventkeyRejectProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        ' CT： TC：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.TEC, EventkeyApproveProces), _
                           TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        ' CT：ChT：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.CHT, EventkeyApproveProces), _
                           TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        ' CT： SA：最終承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.SA, EventkeyLastApproveProces), _
                           TransDicValue(CType(MsgID.id51, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSAMain)) '2014/11/26 起票SA個人へのPush通知追加
        ' CT： FM：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.FM, EventkeyApproveProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        ' CT： CT：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.CT, EventkeyApproveProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))
        ' CT： TC：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.TEC, EventkeyRejectProces), _
                           TransDicValue(CType(MsgID.id52, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        ' CT：ChT：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.CHT, EventkeyRejectProces), _
                           TransDicValue(CType(MsgID.id52, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        ' CT： FM：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.FM, EventkeyRejectProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        ' CT： CT：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CT, Operation.CT, EventkeyRejectProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        'ChT： TC：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.TEC, EventkeyApproveProces), _
                           TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        'ChT：ChT：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CHT, EventkeyApproveProces), _
                           TransDicValue(CType(MsgID.id50, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        'ChT： SA：最終承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.SA, EventkeyLastApproveProces), _
                           TransDicValue(CType(MsgID.id51, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSAMain)) '2014/11/26 起票SA個人へのPush通知追加
        'ChT： FM：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.FM, EventkeyApproveProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        'ChT： CT：承認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CT, EventkeyApproveProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))
        'ChT： TC：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.TEC, EventkeyRejectProces), _
                           TransDicValue(CType(MsgID.id52, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, TransDicNone))
        'ChT：ChT：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CHT, EventkeyRejectProces), _
                           TransDicValue(CType(MsgID.id52, String), PageIdSC3010501_13, PageIdSC3080225, PageIdSC3080225, RefreshSMB))
        'ChT： FM：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.FM, EventkeyRejectProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshFMMain))
        'ChT： CT：否認
        ScreenTransDic.Add(CreateTransDicKey(Operation.CHT, Operation.CT, EventkeyRejectProces), _
                           TransDicValue(TransDicNone, TransDicNone, TransDicNone, TransDicNone, RefreshSMB))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ScreenTransDic

    End Function

    ''' <summary>
    ''' 遷移先画面取得Dictionaryのキーを作成する
    ''' </summary>
    ''' <param name="OperationKey">送信元（操作者）権限コード</param>
    ''' <param name="ClientKey">送信先（依頼者）権限コード</param>
    ''' <param name="inEventKey">イベント特定キー情報</param>
    ''' <remarks>遷移先画面取得Dictionaryのキーを作成する</remarks>
    Private Function CreateTransDicKey(ByVal OperationKey As String _
                                     , ByVal ClientKey As String _
                                     , ByVal inEventKey As String _
                                     ) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START, OperationKey:[{2}], ClientKey:[{3}], inEventKey:[{4}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , OperationKey _
                    , ClientKey _
                    , inEventKey))


        Dim ChangeEventKey As String
        ''イベント特定キーが「201：最終承認処理」なら、「200：承認処理」としてDictionaryキーを作成する
        'If EventkeyLastApproveProces = inEventKey Then
        '    ChangeEventKey = EventkeyApproveProces
        'Else
        '    ChangeEventKey = inEventKey
        'End If
        ChangeEventKey = inEventKey

        Dim DicKey As String = String.Format("{0};{1};{2}", OperationKey, ClientKey, ChangeEventKey)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END, Return:[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , DicKey))

        Return DicKey

    End Function

    ''' <summary>
    ''' 遷移先画面取得Dictionaryに追加する
    ''' </summary>
    ''' <param name="WordNo">文言DB番号</param>
    ''' <param name="RoNoURL">R/Oリンク先ID</param>
    ''' <param name="RegNoURL">車両番号リンク先ID</param>
    ''' <param name="CutomerURL">お客様リンク先ID</param>
    ''' <param name="PushMethod">Push処理（画面更新）時のJavaScript関数名</param>
    ''' <remarks>遷移先画面取得Dictionaryに追加する</remarks>
    Private Function TransDicValue(ByVal WordNo As String _
                                 , ByVal RoNoURL As String _
                                 , ByVal RegNoURL As String _
                                 , ByVal CutomerURL As String _
                                 , ByVal PushMethod As String _
                                 ) As Dictionary(Of String, String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START, WordNo:[{2}], RoNoURL:[{3}], RegNoURL:[{4}], CutomerURL:[{5}], PushMethod:[{6}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , WordNo _
                    , RoNoURL _
                    , RegNoURL _
                    , CutomerURL _
                    , PushMethod))

        Dim DicValueInfo As Dictionary(Of String, String) = New Dictionary(Of String, String)
        DicValueInfo.Add(TransDicKeyWordNo, WordNo)
        DicValueInfo.Add(TransDicKeyRoNoURL, RoNoURL)
        DicValueInfo.Add(TransDicKeyRegNoURL, RegNoURL)
        DicValueInfo.Add(TransDicKeyCutomerURL, CutomerURL)
        DicValueInfo.Add(TransDicKeyPushMethod, PushMethod)


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END, Return_Count:[{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , DicValueInfo.Count))

        Return DicValueInfo

    End Function

#End Region
    '2014/05/13 通知&PUSH処理追加　END　　↑↑↑

#End Region

#Region "ROステータス取得処理"

    ''' <summary>
    ''' ROステータス取得
    ''' </summary>
    ''' <param name="dlrCD">販売店コード</param>
    ''' <returns>ヘッダー情報</returns>
    ''' <remarks></remarks>
    Public Function GetDBRoState(ByVal dlrCD As String) As SC3180201RoStateDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索処理
        Dim tableAdapter As New SC3180201TableAdapter
        Dim dtROStatusInfo As SC3180201RoStateDataTable

        dtROStatusInfo = tableAdapter.GetDBRoState(dlrCD)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return dtROStatusInfo

    End Function

#End Region

#Region "DMS情報取得"

    ''' <summary>
    ''' DMS情報取得
    ''' </summary>
    ''' <param name="inStaffInfo">sスタッフ情報</param>
    ''' <returns>DMS情報</returns>
    ''' <remarks></remarks>
    Public Function GetDmsDealerData(ByVal inStaffInfo As StaffContext) As DmsCodeMapDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using biz As New ServiceCommonClassBusinessLogic
            'DMS販売店データの取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = _
                biz.GetIcropToDmsCode(inStaffInfo.DlrCD,
                                      ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                      inStaffInfo.DlrCD, _
                                      inStaffInfo.BrnCD, _
                                      String.Empty, _
                                      inStaffInfo.Account)

            If dtDmsCodeMapDataTable.Count <= 0 Then
                'データが取得できない場合はエラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is nothing" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return Nothing
                '2015/04/14 新販売店追加対応 start
                '全販売店を含む複数取得の場合は1件目を参照する。エラーとしないようコメントアウト
                'ElseIf 1 < dtDmsCodeMapDataTable.Count Then
                '    'データが2件以上取得できた場合は一意に決定できないためエラー
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '                , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is sum data" _
                '                , Me.GetType.ToString _
                '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '    Return Nothing
                '2015/04/14 新販売店追加対応 end
            Else
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return dtDmsCodeMapDataTable
            End If

        End Using
    End Function

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

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "基幹連携独自更新"

#Region "実行部分"
    ''' <param name="decNowJobDtlID">現在ステータスのJobDtlID</param>
    ''' <param name="decServiceID">サービスID</param>
    ''' <param name="decStallId">ストール利用ID</param>
    ''' <param name="strApplicationID">アプリケーションID</param>
    Public Function SelfFinish(ByVal decNowJobDtlID As Decimal, _
                               ByVal decServiceID As Decimal, _
                               ByVal decStallId As Decimal, _
                               ByVal strApplicationID As String, _
                               ByVal prevStatus As String, _
                               ByVal prevJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable) As Long
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim finishResult As Long = -1

        '2014/05/23 グローバル連携処理修正　START　↓↓↓
        'finishResult = Finish_update(decStallId, decServiceID, decNowJobDtlID, strApplicationID)
        finishResult = Finish_update(decStallId, decServiceID, decNowJobDtlID, strApplicationID, prevStatus, prevJobStatus)
        '2014/05/23 グローバル連携処理修正　　END　↑↑↑

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'If finishResult = 0 Then
        '    Return True
        'Else
        '    Return False
        'End If

        Return finishResult
    End Function
#End Region

#Region "更新部分"

    ''' <summary>
    ''' G10とG13を実行
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="systemId">呼ぶ画面ID</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    ''' '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    Public Function Finish_update(ByVal stallUseId As Decimal, _
                                  ByVal svcinId As Decimal, _
                                  ByVal jobDtlId As Decimal, _
                                  ByVal systemId As String, _
                                  ByVal prevStatus As String, _
                                  ByVal prevJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable) As Long

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '***********************************************************************
        ' 1. いろいろな値を準備する
        '***********************************************************************

        'ステータス取得

        '2014/05/23 グローバル連携処理修正　START　↓↓↓
        'Dim prevStatus As String = JudgeChipStatus(stallUseId)
        Dim crntStatus As String = JudgeChipStatus(stallUseId)
        '2014/05/23 グローバル連携処理修正　　END　↑↑↑

        '***********************************************************************
        ' 2. 実行
        '***********************************************************************

        Dim dmsSendResult As Long

        '基幹側にステータス情報を送信(G13)
        Using ic3802601blc As New IC3802601BusinessLogic
            '2014/05/23 グローバル連携処理修正　START　↓↓↓
            dmsSendResult = ic3802601blc.SendStatusInfo(svcinId, _
                                                        jobDtlId, _
                                                        stallUseId, _
                                                        prevStatus, _
                                                        crntStatus, _
                                                        0, _
                                                        "")
            'Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcinId, _
            '                                                        jobDtlId, _
            '                                                        stallUseId, _
            '                                                        prevStatus, _
            '                                                        prevStatus, _
            '                                                        0)
            '2014/05/23 グローバル連携処理修正　　END　↑↑↑
            ' 2015/5/1 強制納車対応  start
            If Not arySuccessList.Contains(dmsSendResult) Then
                'Return ActionResult.DmsLinkageError
                Return dmsSendResult
            End If
            ' 2015/5/1 強制納車対応  end
        End Using

        '2014/05/23 グローバル連携処理修正　START　↓↓↓
        ''作業ステータスを取得する()
        'Dim prevJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable = Nothing
        'If IsUseJobDispatch() Then
        '    prevJobStatus = JudgeJobStatus(jobDtlId)
        'End If
        '2014/05/23 グローバル連携処理修正　　END　↑↑↑

        ''実績送信使用の場合
        'If IsUseJobDispatch() Then

        '作業ステータスを取得する
        Dim crntJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable = JudgeJobStatus(jobDtlId)

        '基幹側にJobDispatch実績情報を送信(G10)
        '2014/05/23 グローバル連携処理修正　START　↓↓↓
        Dim resultSendJobClock As Long = SendJobClockOnInfo(svcinId, _
                                                            jobDtlId, _
                                                            prevJobStatus, _
                                                            crntJobStatus)
        'Dim resultSendJobClock As Long = SendJobClockOnInfo(svcinId, _
        '                                                    jobDtlId, _
        '                                                    crntJobStatus, _
        '                                                    crntJobStatus)
        '2014/05/23 グローバル連携処理修正　　END　↑↑↑
        ' 2015/5/1 強制納車対応  start
        If Not arySuccessList.Contains(resultSendJobClock) Then
            'Return ActionResult.DmsLinkageError
            Return resultSendJobClock
        End If

        'End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If dmsSendResult <> ActionResult.Success Then
            'ワーニング返却
            Return dmsSendResult
        ElseIf resultSendJobClock <> ActionResult.Success Then
            'ワーニング返却
            Return resultSendJobClock
        Else
            ' 正常終了
            Return ActionResult.Success
        End If
        ' 2015/5/1 強制納車対応  end
    End Function

#End Region

#Region "JobDispatch送信"
    ''' <summary>
    ''' 基幹連携(JobDispatch実績情報送信処理)を行う(メイン)
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="prevJobStatus">更新前作業連携ステータス</param>
    ''' <param name="crntJobStatus">更新後作業連携ステータス</param>
    ''' <returns>ActionResult</returns>
    ''' <remarks></remarks>
    Public Function SendJobClockOnInfo(ByVal svcinId As Decimal, _
                                       ByVal jobDtlId As Decimal, _
                                       ByVal prevJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable, _
                                       ByVal crntJobStatus As IC3802701DataSet.IC3802701JobStatusDataTable) As Long

        Using IC3802701Biz As New IC3802701BusinessLogic
            Dim dmsSendResult As Long = IC3802701Biz.SendJobClockOnInfo(svcinId, _
                                                                        jobDtlId, _
                                                                        prevJobStatus, _
                                                                        crntJobStatus)
            ' 2015/5/1 強制納車対応  start
            If Not arySuccessList.Contains(dmsSendResult) Then
                Return ActionResult.DmsLinkageError
            Else
                Return dmsSendResult
            End If
            ' 2015/5/1 強制納車対応  end
        End Using

    End Function
#End Region

#Region "各ステータス"

#Region "作業ステータス"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobStatusWorking As String = "0"
    ''' <summary>
    ''' 完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobStatusFinish As String = "1"
    ''' <summary>
    ''' 中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobStatusStop As String = "2"

    ''' <summary>
    ''' 制御用作業ステータス：開始前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusBeforeWork As String = "101"
    ''' <summary>
    ''' 制御用作業ステータス：作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusWorking As String = "102"
    ''' <summary>
    ''' 制御用作業ステータス：完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusFinish As String = "103"
    ''' <summary>
    ''' 制御用作業ステータス：中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobLinkStatusStop As String = "104"
#End Region

#Region "サービスステータス"
    ''' <summary>
    ''' 未入庫
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNotCarin As String = "00"
    ''' <summary>
    ''' 未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNoShow As String = "01"
    ''' <summary>
    ''' キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusCanel As String = "02"
    ''' <summary>
    ''' 着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusWorkOrderWait As String = "03"
    ''' <summary>
    ''' 作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusStartwait As String = "04"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusStart As String = "05"
    ''' <summary>
    ''' 次の作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusNextStartWait As String = "06"
    ''' <summary>
    ''' 洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusCarWashWait As String = "07"
    ''' <summary>
    ''' 洗車中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusCarWashStart As String = "08"
    ''' <summary>
    ''' 検査待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusInspectionWait As String = "09"
    ''' <summary>
    ''' 検査中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusInspectionStart As String = "10"
    ''' <summary>
    ''' 預かり中（DropOff）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusDropOffCustomer As String = "11"
    ''' <summary>
    ''' 納車待ち（Waiting）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusWaitingCustomer As String = "12"
    ''' <summary>
    ''' 納車済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusDelivery As String = "13"
#End Region

#Region "チップステータス"
    ''' <summary>
    ''' 未入庫(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeNotCarIn As String = "1"

    ''' <summary>
    ''' 未入庫(本予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusConfirmedNotCarIn As String = "2"

    ''' <summary>
    ''' 作業開始待ち(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeWaitStart As String = "3"

    ''' <summary>
    ''' 作業開始待ち(本予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusConfirmedWaitStart As String = "4"

    ''' <summary>
    ''' 仮置き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTemp As String = "5"

    ''' <summary>
    ''' 未来店客(本予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusNoshow As String = "6"

    ''' <summary>
    ''' 飛び込み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusWalkin As String = "7"

    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusWorking As String = "8"

    ''' <summary>
    ''' 作業中断：部品欠品
    ''' </summary>
    Private Const ChipStatusStopForPartsStockout As String = "9"

    ''' <summary>
    ''' 作業中断：顧客連絡待ち
    ''' </summary>
    Private Const ChipStatusStopForWaitCustomer As String = "10"

    ''' <summary>
    ''' 作業中断：ストール待ち
    ''' </summary>
    Private Const ChipStatusStopForWaitStall As String = "11"

    ''' <summary>
    ''' 作業中断：その他
    ''' </summary>
    Private Const ChipStatusStopForOtherReason As String = "12"

    ''' <summary>
    ''' 作業中断：検査中断
    ''' </summary>
    Private Const ChipStatusStopForInspection As String = "13"

    ''' <summary>
    ''' 洗車待ち
    ''' </summary>
    Private Const ChipStatusWaitWash As String = "14"

    ''' <summary>
    ''' 洗車中
    ''' </summary>
    Private Const ChipStatusWashing As String = "15"

    ''' <summary>
    ''' 検査待ち
    ''' </summary>
    Private Const ChipStatusWaitInspection As String = "16"

    ''' <summary>
    ''' 検査中
    ''' </summary>
    Private Const ChipStatusInspecting As String = "17"

    ''' <summary>
    ''' 預かり中
    ''' </summary>
    Private Const ChipStatusKeeping As String = "18"

    ''' <summary>
    ''' 納車待ち
    ''' </summary>
    Private Const ChipStatusWaitDelivery As String = "19"

    ''' <summary>
    ''' 作業完了
    ''' </summary>
    Private Const ChipStatusJobFinish As String = "20"

    ''' <summary>
    ''' 日跨ぎ終了
    ''' </summary>
    Private Const ChipStatusDateCrossEnd As String = "21"

    ''' <summary>
    ''' 納車済み
    ''' </summary>
    Private Const ChipStatusDeliveryEnd As String = "22"

    ''' <summary>
    ''' 未来店客(仮予約)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ChipStatusTentativeNoShow As String = "24"

#End Region

#Region "予約ステータス"
    ''' <summary>
    ''' 仮予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResvStatusTentative As String = "0"

    ''' <summary>
    ''' 本予約
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResvStatusConfirmed As String = "1"
#End Region

#Region "ストール利用ステータス"
    ''' <summary>
    ''' 着工指示待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusWorkOrderWait As String = "00"
    ''' <summary>
    ''' 作業開始待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStartWait As String = "01"
    ''' <summary>
    ''' 作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStart As String = "02"
    ''' <summary>
    ''' 完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusFinish As String = "03"
    ''' <summary>
    ''' 作業計画の一部の作業が中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStartIncludeStopJob As String = "04"
    ''' <summary>
    ''' 中断
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusStop As String = "05"
    ''' <summary>
    ''' 日跨ぎ終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusMidfinish As String = "06"
    ''' <summary>
    ''' 未来店客（予定通りの入庫がないため一旦チップをNoShowエリアに移動している）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StalluseStatusNoshow As String = "07"
#End Region

#Region "中断理由区分"
    ''' <summary>
    ''' 部品欠品
    ''' </summary>
    Private Const StopReasonPartsStockOut As String = "01"
    ''' <summary>
    ''' お客様連絡待ち
    ''' </summary>
    Private Const StopReasonCustomerReportWaiting As String = "02"
    ''' <summary>
    ''' 検査不合格
    ''' </summary>
    Private Const StopReasonInspectionFailure As String = "03"
    ''' <summary>
    ''' その他
    ''' </summary>
    Private Const StopReasonOthers As String = "99"
#End Region

#Region "既定値"
    ''' <summary>
    ''' DB数値型の既定値（0）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultNumberValue As Long = 0
#End Region

#End Region

#Region "Jobステータス判定"

    ''' <summary>
    ''' 該チップに紐付く作業のステータスを取得する
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <returns>作業ステータステーブル</returns>
    ''' <remarks></remarks>
    Private Function JudgeJobStatus(ByVal jobDtlId As Decimal) As IC3802701DataSet.IC3802701JobStatusDataTable

        Dim jobStatusTable As TabletSMBCommonClassDataSet.TabletSmbCommonClassJobResultDataTable = Nothing

        Using ta As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            '作業単位でステータスを取得する
            jobStatusTable = ta.GetJobStatusByJob(jobDtlId)
        End Using

        '戻す用テーブル
        Using retJobStatusTable As New IC3802701DataSet.IC3802701JobStatusDataTable
            For Each jobStatusRow As TabletSMBCommonClassDataSet.TabletSmbCommonClassJobResultRow In jobStatusTable
                Dim retJobStatusRow As IC3802701DataSet.IC3802701JobStatusRow = retJobStatusTable.NewIC3802701JobStatusRow

                '値の設定
                retJobStatusRow.JOB_DTL_ID = jobStatusRow.JOB_DTL_ID
                retJobStatusRow.JOB_INSTRUCT_ID = jobStatusRow.JOB_INSTRUCT_ID
                retJobStatusRow.JOB_INSTRUCT_SEQ = jobStatusRow.JOB_INSTRUCT_SEQ

                '作業ステータスを設定する
                '作業前(実績テーブルに作業指示のレコードがないので、DBNULLだ)
                If jobStatusRow.IsJOB_STATUSNull Then
                    retJobStatusRow.JOB_STATUS = JobLinkStatusBeforeWork
                Else
                    'Linkテーブル用の作業ステータスに変更する
                    Select Case jobStatusRow.JOB_STATUS
                        Case JobStatusFinish
                            '終了
                            retJobStatusRow.JOB_STATUS = JobLinkStatusFinish
                        Case JobStatusWorking
                            '作業中
                            retJobStatusRow.JOB_STATUS = JobLinkStatusWorking
                        Case JobStatusStop
                            '中断
                            retJobStatusRow.JOB_STATUS = JobLinkStatusStop
                    End Select
                End If

                '一行追加
                retJobStatusTable.AddIC3802701JobStatusRow(retJobStatusRow)
            Next

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.END " _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            'ログ
            Me.OutPutIFLog(retJobStatusTable, "IC3802701JobStatusDataTable:")

            Return retJobStatusTable
        End Using

    End Function

#End Region

#Region "チップステータス判定"
    ''' <summary>
    ''' チップのステータス判定
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <returns>
    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    ''' 1：未入庫(仮予約)、2：未入庫(本予約)、3：作業開始待ち(仮予約)、
    ''' 4：作業開始待ち(本予約)、6：未来店客、7：飛び込み客、
    ''' 8：作業中、9：中断・部品欠品、10：中断・お客様連絡待ち、
    ''' 11：中断・ストール待機、12：中断・その他、13：中断・検査中断、
    ''' 14：洗車待ち、15：洗車中、16：検査待ち、17：検査中、18：預かり中、
    ''' 19：納車待ち、20：次の作業開始待ち、22：納車済み
    ''' </returns>
    ''' <remarks></remarks>
    Public Function JudgeChipStatus(ByVal stallUseId As Decimal) As String

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '戻り値
        Dim retValue As String = String.Empty

        'エラー発生フラグ
        Dim errorFlg As Boolean = False


        'チップエンティティ
        Dim chipEntityTable As New TabletSMBCommonClassDataSet.TabletSmbCommonClassChipEntityDataTable

        'TabletSMBCommonClassのテーブルアダプタークラスインスタンスを生成
        Using myTableAdapter As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter()
            'チップ情報取得
            chipEntityTable = myTableAdapter.GetChipEntity(stallUseId, 0)
            Me.OutPutIFLog(chipEntityTable, "ChipEntityTable:") 'ログ
        End Using

        'チップ情報が取得できない場合はエラー
        If chipEntityTable.Count <= 0 Then
            errorFlg = True

            Return String.Empty
        End If

        'データ行の抜き出し
        Dim chipEntityRow As TabletSMBCommonClassDataSet.TabletSmbCommonClassChipEntityRow _
            = DirectCast(chipEntityTable.Rows(0), TabletSMBCommonClassDataSet.TabletSmbCommonClassChipEntityRow)
        'サービスステータス
        Dim svcStatus As String = chipEntityRow.SVC_STATUS
        '予約ステータス
        Dim resvStatus As String = chipEntityRow.RESV_STATUS
        'ストール利用ステータス
        Dim stallUseStatus As String = chipEntityRow.STALL_USE_STATUS
        '中断理由区分
        Dim stopReasonType As String = chipEntityRow.STOP_REASON_TYPE
        '関連ストール非稼動ID
        Dim stallIdleId As Decimal = chipEntityRow.STALL_IDLE_ID
        '受付区分
        Dim acceptanceType As String = chipEntityRow.ACCEPTANCE_TYPE
        'ストールID
        Dim stallId As Decimal = chipEntityRow.STALL_ID
        'サービスステータスによって分岐
        Select Case svcStatus

            Case SvcStatusNotCarin
                'サービスステータス「00：未入庫」の場合
                retValue = Me.JudgeNotCarInStatus(resvStatus)

            Case SvcStatusNoShow
                'サービスステータス「01：未来店客」の場合
                retValue = Me.JudgeNoShowStatus(resvStatus, stallUseStatus)

            Case SvcStatusWorkOrderWait, SvcStatusStartwait, SvcStatusNextStartWait
                'サービスステータス「03：着工指示待ち」「04：作業開始待ち」「06：次の作業開始待ち」の場合
                retValue = Me.JudgeWaitStartStatus(resvStatus, stallUseStatus, stopReasonType, stallIdleId, acceptanceType, stallId)

            Case SvcStatusStart
                'サービスステータス「05：作業中」
                retValue = Me.JudgeStartStatus(stallUseStatus, stopReasonType, stallIdleId)

            Case SvcStatusCarWashWait
                'サービスステータス「07：洗車待ち」→チップステータス【14:洗車待ち】
                retValue = ChipStatusWaitWash

            Case SvcStatusCarWashStart
                'サービスステータス「08：洗車中」→チップステータス【15:洗車中】
                retValue = ChipStatusWashing

            Case SvcStatusInspectionWait
                'サービスステータス「09：検査待ち」→チップステータス【16:検査待ち】
                retValue = ChipStatusWaitInspection

            Case SvcStatusInspectionStart
                'サービスステータス「10：検査中」→チップステータス【17:検査中】
                retValue = ChipStatusInspecting

            Case SvcStatusDropOffCustomer
                'サービスステータス「11：預かり中」→チップステータス【18:預かり中】
                retValue = ChipStatusKeeping

            Case SvcStatusWaitingCustomer
                'サービスステータス「12：納車待ち」→チップステータス【19:納車待ち】
                retValue = ChipStatusWaitDelivery

            Case SvcStatusDelivery
                'サービスステータス「13：納車済み」→チップステータス【22:納車済み】
                retValue = ChipStatusDeliveryEnd

        End Select

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return retValue

    End Function
#End Region

#Region "チップステータス"

    ''' <summary>
    ''' サービスステータス「00:未入庫」の場合のチップステータスを判定
    ''' </summary>
    ''' <param name="resvStatus">予約ステータス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function JudgeNotCarInStatus(ByVal resvStatus As String) As String

        '戻り値
        Dim retValue As String = String.Empty

        If resvStatus.Equals(ResvStatusTentative) Then
            'チップステータス【1：未入庫(仮予約)】
            retValue = ChipStatusTentativeNotCarIn
        Else
            'チップステータス【2：未入庫(本予約)】
            retValue = ChipStatusConfirmedNotCarIn
        End If

        Return retValue

    End Function

    ''' <summary>
    ''' サービスステータス「01：未来店客」の場合のチップステータスを判定
    ''' </summary>
    ''' <param name="resvStatus">予約ステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function JudgeNoShowStatus(ByVal resvStatus As String, ByVal stallUseStatus As String) As String

        '戻り値
        Dim retValue As String = String.Empty

        If stallUseStatus.Equals(StalluseStatusNoshow) Then
            'ストール利用ステータス「07：未来店客」の場合
            'チップステータス【6：未来店客】
            retValue = ChipStatusNoshow

        ElseIf stallUseStatus.Equals(StalluseStatusWorkOrderWait) _
        OrElse stallUseStatus.Equals(StalluseStatusStartWait) Then
            'ストール利用ステータス「00：着工指示待ち」または「01：作業開始待ち」の場合

            If resvStatus.Equals(ResvStatusTentative) Then
                'チップステータス【1：未入庫(仮予約)】
                retValue = ChipStatusTentativeNotCarIn
            Else
                'チップステータス【2：未入庫(本予約)】
                retValue = ChipStatusConfirmedNotCarIn
            End If
        End If

        Return retValue

    End Function

    ''' <summary>
    ''' サービスステータス「03:着工指示待ち」「04:作業開始待ち」「06:次の作業開始待ち」の場合のチップステータスを判定
    ''' </summary>
    ''' <param name="resvStatus">予約ステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="stopReasonType">中断理由区分</param>
    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    ''' <param name="acceptanceType">受付区分</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function JudgeWaitStartStatus(ByVal resvStatus As String, ByVal stallUseStatus As String, _
                                          ByVal stopReasonType As String, ByVal stallIdleId As Decimal, _
                                          ByVal acceptanceType As String, ByVal stallId As Decimal) As String

        '戻り値
        Dim retValue As String = String.Empty

        'ストール利用ステータスで分岐
        Select Case stallUseStatus

            Case StalluseStatusFinish
                'ストール利用ステータス「03：完了」の場合
                'チップステータス【20：次の作業開始待ち】
                retValue = ChipStatusJobFinish

            Case StalluseStatusStop
                'ストール利用ステータス「05：中断」の場合
                retValue = Me.JudgeStopStatus(stopReasonType, stallIdleId)

            Case StalluseStatusWorkOrderWait, StalluseStatusStartWait
                'ストール利用ステータス「00：着工指示待ち」「01：作業開始待ち」の場合

                If acceptanceType.Equals(AcceptanceTypeWalkin) _
                AndAlso stallId = DefaultNumberValue Then
                    '受付区分が「1：Walk-in」、かつストールIDが未設定の場合
                    'チップステータス【7：Walk-in】
                    retValue = ChipStatusWalkin

                Else
                    If resvStatus.Equals(ResvStatusTentative) Then
                        'チップステータス【3：作業開始待ち(仮予約)】
                        retValue = ChipStatusTentativeWaitStart
                    Else
                        'チップステータス【4：作業開始待ち(本予約)】
                        retValue = ChipStatusConfirmedWaitStart
                    End If
                End If

        End Select

        Return retValue

    End Function

    ''' <summary>
    ''' ストール利用ステータス「05:中断」の場合のチップステータス判定
    ''' </summary>
    ''' <param name="stopReasonType">中断理由区分</param>
    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    ''' <returns>
    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    ''' 9：中断・部品欠品、10：中断・お客様連絡待ち、11：中断・ストール待機、
    ''' 12：中断・その他、13：中断・検査中断
    ''' </returns>
    ''' <remarks></remarks>
    Private Function JudgeStopStatus(ByVal stopReasonType As String, ByVal stallIdleId As Decimal) As String

        '戻り値
        Dim retValue As String = String.Empty

        If stallIdleId <> DefaultNumberValue Then
            '関連ストール非稼動IDが設定されている場合
            'チップステータス【11：作業中断(ストール待ち)】
            retValue = ChipStatusStopForWaitStall
        Else
            '中断理由区分で分岐
            Select Case stopReasonType
                Case StopReasonPartsStockOut
                    '中断理由区分「01：部品欠品」の場合
                    'チップステータス【09：作業中断(部品欠品)】
                    retValue = ChipStatusStopForPartsStockout

                Case StopReasonCustomerReportWaiting
                    '中断理由区分「02：お客様連絡待ち」の場合
                    'チップステータス【10：作業中断(お客様連絡待ち)】
                    retValue = ChipStatusStopForWaitCustomer

                Case StopReasonInspectionFailure
                    '中断理由区分「03：検査不合格」の場合
                    'チップステータス【13：作業中断(検査中断)】
                    retValue = ChipStatusStopForInspection

                Case StopReasonOthers
                    '中断理由区分が「99：その他」の場合
                    'チップステータス【12：作業中断(その他)】
                    retValue = ChipStatusStopForOtherReason

                Case Else
                    '中断理由区分が上記以外の場合
                    'チップステータス【12：作業中断(その他)】
                    retValue = ChipStatusStopForOtherReason
            End Select
        End If

        Return retValue

    End Function

    ''' <summary>
    ''' サービスステータス「05:作業中」の場合のチップステータス判定
    ''' </summary>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="stopReasonType">中断理由区分</param>
    ''' <param name="stallIdleId">関連ストール非稼動ID</param>
    ''' <returns>
    ''' チップのステータスを判定し、以下のいずれかのチップステータスを返却する。
    ''' 4：作業開始待ち（本予約）、8：作業中、9：中断・部品欠品、
    ''' 10：中断・お客様連絡待ち、11：中断・ストール待機、12：中断・その他、13：中断・検査中断、20：作業完了
    ''' </returns>
    ''' <remarks></remarks>
    Private Function JudgeStartStatus(ByVal stallUseStatus As String, ByVal stopReasonType As String, ByVal stallIdleId As Decimal) As String

        '戻り値
        Dim retValue As String = String.Empty

        'ストール利用ステータスで分岐
        Select Case stallUseStatus

            Case StalluseStatusWorkOrderWait, StalluseStatusStartWait
                'ストール利用ステータス「00：着工指示待ち」「01：作業開始待ち」の場合
                'チップステータス【4：作業開始待ち(本予約)】
                retValue = ChipStatusConfirmedWaitStart

            Case StalluseStatusStart, StalluseStatusStartIncludeStopJob
                'ストール利用ステータス「02：作業中」「04：作業指示の一部の作業が中断」の場合
                'チップステータス【8：作業中】
                retValue = ChipStatusWorking

            Case StalluseStatusStop
                'ストール利用ステータス「05：中断」の場合
                retValue = Me.JudgeStopStatus(stopReasonType, stallIdleId)

            Case StalluseStatusFinish
                'ストール利用ステータス「03：完了」の場合
                'チップステータス【20：次の作業開始待ち】
                retValue = ChipStatusJobFinish

        End Select

        Return retValue

    End Function
#End Region

#Region "作業実績送信使用フラグ"
    ''' <summary>
    ''' 作業実績送信使用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SysParaNameJobDispatchUseFlg As String = "JOBDISPATCH_USE_FLG"
#End Region

#Region "Job Dispatch運用判定"

    ''' <summary>
    ''' Job Dispatch運用を行うか否かを設定
    ''' </summary>
    ''' <returns>作業実績送信使用フラグ True:使用</returns>
    ''' <remarks></remarks>
    Private Function IsUseJobDispatch() As Boolean
        Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
            'Job Dispatch運用フラグ
            Dim jobDispatchUseFlg As String = serviceCommonBiz.GetDlrSystemSettingValueBySettingName(SysParaNameJobDispatchUseFlg)

            If String.IsNullOrEmpty(jobDispatchUseFlg) Then
                Return False
            Else
                '使用の場合、trueを戻す
                If jobDispatchUseFlg.Trim().Equals(DispatchUseFlg) Then
                    Return True
                Else

                    Return False
                End If
            End If

        End Using
    End Function

#End Region

#Region "OutPutIFLog"
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
#End Region

#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    '2014/09/09 複数チップが存在する場合、テクニシャンアドバイスが取得できない可能性が高い為、取得方法修正 Start
    ''' <summary>
    ''' RO番号をキーに、[完成検査結果データ]テーブルに登録された[アドバイス]を取得する
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <returns>[完成検査結果データ].[アドバイス]</returns>
    ''' <remarks></remarks>
    Public Function GetAdviceContent(ByVal dealerCD As String, _
                                     ByVal branchCD As String, _
                                     ByVal roNum As String) As String

        Dim tableAdapter As New SC3180201TableAdapter

        Return tableAdapter.GetAdviceContent(dealerCD, branchCD, roNum)

    End Function
    '2014/09/09 複数チップが存在する場合、テクニシャンアドバイスが取得できない可能性が高い為、取得方法修正 End

    '2020/02/13 NCN 小林 TKM要件：型式対応 Start
    ''' <summary>
    ''' マスタに販売店が登録されているか判定する
    ''' </summary>
    ''' <param name="strRoNum">R/O番号</param>
    ''' <param name="strDlrCd">販売店コード</param>
    ''' <param name="strBrnCd">店舗コード</param>
    ''' <returns>登録状態 TRANSACTION_EXIST : True or False , HISTORY_EXIST : True or False , MAINTE_CD_EXIST : True or False , KATASHIKI_EXIST : True or False, COMB_DLR_AND_BRN_EXIST : True or False</returns>
    ''' <remarks></remarks>
    Public Function GetDlrCdExistMst(ByVal strRoNum As String, ByVal strDlrCd As String, ByVal strBrnCd As String) As Dictionary(Of String, Boolean)

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180201TableAdapter
        Dim dt As New DataTable

        '点検組み合わせマスタ検索
        dt = tableAdapter.GetDlrCdExistMst(strRoNum, _
                                           strDlrCd, _
                                           strBrnCd)

        Dim dict As New Dictionary(Of String, Boolean)
        dict.Add("TRANSACTION_EXIST", False)
        dict.Add("HISTORY_EXIST", False)
        dict.Add("MAINTE_CD_EXIST", False)
        dict.Add("KATASHIKI_EXIST", False)
        dict.Add("COMB_DLR_AND_BRN_EXIST", False)

        If dt.Rows.Count > 0 Then
            If dt.Rows.Cast(Of DataRow).Any(Function(row) "1".Equals(row("COMB_DLR_AND_BRN_EXIST").ToString())) Then
                dict("COMB_DLR_AND_BRN_EXIST") = True
            End If
            If dt.Rows.Cast(Of DataRow).Any(Function(row) "1".Equals(row("TRANSACTION_EXIST").ToString())) Then
                dict("TRANSACTION_EXIST") = True
            End If
            If dt.Rows.Cast(Of DataRow).Any(Function(row) "1".Equals(row("HISTORY_EXIST").ToString())) Then
                dict("HISTORY_EXIST") = True
            End If
            If dt.Rows.Cast(Of DataRow).Any(Function(row) "1".Equals(row("MAINTE_CD_EXIST").ToString())) Then
                dict("MAINTE_CD_EXIST") = True
            End If
            If dt.Rows.Cast(Of DataRow).Any(Function(row) "1".Equals(row("KATASHIKI_EXIST").ToString())) Then
                dict("KATASHIKI_EXIST") = True
            End If
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END [Result=TRANSACTION_EXIST:{2}, HISTORY_EXIST:{3}, MAINTE_CD_EXIST:{4}, KATASHIKI_EXIST:{5}, COMB_DLR_AND_BRN_EXIST:{6}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , dict("TRANSACTION_EXIST").ToString _
                  , dict("HISTORY_EXIST").ToString _
                  , dict("MAINTE_CD_EXIST").ToString _
                  , dict("KATASHIKI_EXIST").ToString _
                  , dict("COMB_DLR_AND_BRN_EXIST").ToString))
        Return dict

    End Function
    '2020/02/13 NCN 小林 TKM要件：型式対応 End

    '2017/2/1 TR-SVT-TMT-20161209-002 アドバイスをRO単位で更新する Start
    ''' <summary>
    ''' 行ロックバージョン取得(GetInspectionHeadLock)
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <returns>行ロックバージョン</returns>
    ''' <remarks></remarks>
    Public Function GetInspectionHeadLock(ByVal jobDtlId As Decimal) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180201TableAdapter

        '完成検査結果データ 行ロックバージョンの取得
        Dim headLockVersion = tableAdapter.GetHeadLockVersion(jobDtlId)
        Dim lockVersion As Long

        If Not IsNothing(headLockVersion(0).ROW_LOCK_VERSION.ToString) Then
            lockVersion = Long.Parse(headLockVersion(0).ROW_LOCK_VERSION.ToString)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return lockVersion

    End Function


    ''' <summary>
    ''' 完成検査結果データ.アドバイスの更新
    ''' RO番号単位でアドバイスを更新する
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="adviceContent">アドバイスコメント</param>
    ''' <param name="accountName">更新アカウント</param>
    ''' <param name="updateTime">更新日付</param>
    ''' <returns>True：更新完了 ／ False：更新失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetDBInspectionAdvice(ByVal dealerCD As String, _
                                                 ByVal branchCD As String, _
                                                 ByVal roNum As String, _
                                                 ByVal adviceContent As String, _
                                                 ByVal accountName As String, _
                                                 ByVal updateTime As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim isUpdatedSuccess As Boolean
        Dim tableAdapter As New SC3180201TableAdapter

        'アドバイス更新対象リストの取得
        Dim AdviceJobData As SC3180201AdviceJobDataTable
        AdviceJobData = tableAdapter.SelectInspectionHeadList(dealerCD, branchCD, roNum)

        For intListIndex = 0 To AdviceJobData.Count - 1
            Dim jobDtlid As String = AdviceJobData(intListIndex).JOB_DTL_ID

            '完成検査結果データロック処理
            SelectInspectionHeadLock(jobDtlid)

            '完成検査結果データロックバージョンの取得
            Dim lockVersion As Long = Me.GetInspectionHeadLock(jobDtlid)

            '更新処理
            isUpdatedSuccess = tableAdapter.SetDBInspectionAdviceUpt(jobDtlid, _
                                                              adviceContent, _
                                                              accountName, _
                                                              updateTime, _
                                                              lockVersion)
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END [Result=Return:{2}, RO_NUM:{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , isUpdatedSuccess _
                  , roNum))

        '処理結果返却
        Return isUpdatedSuccess

    End Function
    '2017/2/1 TR-SVT-TMT-20161209-002 アドバイスをRO単位で更新する End

    '2017/2/20 ライフサイクル対応 走行距離を完成検査で登録する Start

    ''' <summary>
    ''' 前回部品交換情報登録処理
    ''' </summary>
    ''' <param name="vin">VIN</param>
    ''' <param name="inspecItemCd">点検項目コード</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="updateTime">日時</param>
    ''' <param name="accountName">アカウント名</param>
    ''' <param name="prePartsReplaceDt">前回部品交換情報(取得条件VIN)</param>
    ''' <param name="updateFlg">更新フラグ</param>
    ''' <returns>登録成功：True／失敗：False</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetPreviousPartsReplace(ByVal vin As String, _
                                            ByVal inspecItemCd As String, _
                                            ByVal dealerCD As String, _
                                            ByVal branchCD As String, _
                                            ByVal roNum As String, _
                                            ByVal updateTime As Date, _
                                            ByVal accountName As String, _
                                            ByVal prePartsReplaceDt As SC3180201PreviousPartsReplaceDataTable, _
                                            ByVal updateFlg As Integer) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))


        Dim isSuccessSet As Boolean = False
        Dim tableAdapter As New SC3180201TableAdapter

        ' VINを条件に取得された前回部品交換情報を点検項目コードで絞込
        Dim prePartsReplaceArray As Array = prePartsReplaceDt.Select(String.Format("INSPEC_ITEM_CD = {0}", inspecItemCd))

        '走行距離取得
        Dim regMile As Decimal = Me.GetReplaceMile(dealerCD, branchCD, roNum)

        If prePartsReplaceArray.Length > 0 Then

            Dim prePartsReplaceRow As SC3180201PreviousPartsReplaceRow = DirectCast(prePartsReplaceArray(0), SC3180201PreviousPartsReplaceRow)
            Dim replaceDate As Date
            If updateFlg = CType(InspectionUpdateApprove, Integer) Then
                replaceDate = updateTime
            Else
                replaceDate = Date.Parse(FormatDbDateTime, CultureInfo.CurrentCulture)
            End If

            '行ロック取得
            SelectPartsReplaceLock(vin, inspecItemCd)
            If roNum.Equals(prePartsReplaceRow.RO_NUM) Then
                '更新処理
                isSuccessSet = tableAdapter.SetPartsReplaceUpt(vin,
                                                               inspecItemCd,
                                                               replaceDate,
                                                               accountName,
                                                               updateTime,
                                                               CType(prePartsReplaceRow.ROW_LOCK_VERSION, Long))

            Else
                ' RO番号が前回部品交換情報と一致しない場合
                ' 完成検査承認で変更された部品
                '更新処理
                isSuccessSet = tableAdapter.SetPartsReplaceUpt(vin,
                                                               inspecItemCd,
                                                               roNum,
                                                               regMile,
                                                               replaceDate,
                                                               CType(prePartsReplaceRow.REPLACE_MILE, Decimal),
                                                               Date.Parse(prePartsReplaceRow.REPLACE_DATE, CultureInfo.CurrentCulture),
                                                               accountName,
                                                               updateTime,
                                                               CType(prePartsReplaceRow.ROW_LOCK_VERSION, Long))
            End If
        Else
            '登録処理
            isSuccessSet = tableAdapter.SetPartsReplaceIns(vin,
                                                           inspecItemCd,
                                                           roNum,
                                                           CType(regMile, Decimal),
                                                           accountName,
                                                           updateTime,
                                                           updateFlg)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END [Result=Return:{2}, VIN:{3}, INSPEC_ITEM_CD:{4}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , isSuccessSet _
                  , vin _
                  , inspecItemCd))

        '処理結果返却
        Return isSuccessSet

    End Function

#Region "走行距離の取得"
    ''' <summary>
    ''' 走行距離の取得
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <returns>走行距離(取得失敗時は-1)</returns>
    ''' <remarks></remarks>
    Public Function GetReplaceMile(ByVal dealerCD As String, _
                                    ByVal branchCD As String, _
                                    ByVal roNum As String) As Decimal

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Const INIT_REG_MILE As Decimal = -1

        Dim DmsDataTable As DmsCodeMapDataTable = Me.GetDmsDealerData(dealerCD, branchCD)

        Dim DMS_BRN_CD As String
        If 0 < DmsDataTable.Count Then
            If Not DmsDataTable(0).IsCODE2Null AndAlso Not String.IsNullOrWhiteSpace(DmsDataTable(0).CODE2) Then
                'DMS変換後の店舗コードを取得
                DMS_BRN_CD = DmsDataTable(0).CODE2
            Else
                'DMS変換失敗時は変換前の店舗コードを入れる
                DMS_BRN_CD = branchCD
            End If
        Else
            'DMS変換取得失敗時は変換前の店舗コードを入れる
            DMS_BRN_CD = branchCD
        End If

        '入庫管理番号作成
        Dim strSVCIN_NUM As String = GetSVCIN_NUM(DMS_BRN_CD, roNum)

        '入庫履歴より走行距離を取得
        Dim tableAdapter As New SC3180201TableAdapter
        Dim replaceMileDt As SC3180201PreviosReplacementMileageDataTable
        replaceMileDt = tableAdapter.GetPreviosReplacementMileage(dealerCD, strSVCIN_NUM)

        Dim regMile As Decimal
        If replaceMileDt.Count > 0 Then
            regMile = CType(replaceMileDt(0).REG_MILE, Decimal)
        Else
            regMile = INIT_REG_MILE
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END [Result=Return:{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , regMile))

        '処理結果返却
        Return regMile

    End Function

    ''' <summary>
    ''' DMS情報取得
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <returns>DMS情報</returns>
    ''' <remarks></remarks>
    Public Function GetDmsDealerData(ByVal dealerCD As String, ByVal branchCD As String) As DmsCodeMapDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using biz As New ServiceCommonClassBusinessLogic
            'DMS販売店データの取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = _
                biz.GetIcropToDmsCode(dealerCD,
                                      ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                      dealerCD, _
                                      branchCD, _
                                      String.Empty, _
                                      String.Empty)

            If dtDmsCodeMapDataTable.Count <= 0 Then
                'データが取得できない場合はエラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is nothing" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return Nothing
            Else
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return dtDmsCodeMapDataTable
            End If

        End Using
    End Function

    ''' <summary>
    ''' 入庫管理番号取得
    ''' </summary>
    ''' <returns>入庫管理番号</returns>
    ''' <remarks>入庫管理番号の書式変換を行う</remarks>
    Public Function GetSVCIN_NUM(ByVal strBRN_CD As String, ByVal strRO_NUM As String) As String

        '開始ログの記録
        Logger.Info(String.Format("GetSVCIN_NUM_START, strBRN_CD:[{0}]", strBRN_CD))

        '①「販売店システム設定」より、「入庫管理番号利用フラグ」を取得する。
        Dim SVCIN_FLG As String = Me.GetDlrSystemSettingValueBySettingName("SVCIN_NUM_USE_FLG")

        '②「入庫管理番号利用フラグ」が０の場合、書式変換を行う
        Dim SVCIN_Num As String = String.Empty
        If Not String.IsNullOrWhiteSpace(SVCIN_FLG) Then
            If SVCIN_FLG = "0" Then
                Dim SVCIN_Format As String = Me.GetDlrSystemSettingValueBySettingName("SVCIN_NUM_FORMAT")
                If Not String.IsNullOrWhiteSpace(SVCIN_Format) Then
                    SVCIN_Num = Replace(Replace(SVCIN_Format, "[RO_NUM]", strRO_NUM), "[DMS_BRN_CD]", strBRN_CD)
                End If
            End If
        End If

        '終了ログの記録
        Logger.Info(String.Format("GetSVCIN_NUM_END, Return:[{0}]", SVCIN_Num))

        Return SVCIN_Num

    End Function

    ''' <summary>
    ''' 販売店システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">販売店システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDlrSystemSettingValueBySettingName(ByVal settingName As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:settingName={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName))

        '戻り値
        Dim retValue As String = String.Empty

        'ログイン情報
        Dim userContext As StaffContext = StaffContext.Current

        '自分のテーブルアダプタークラスインスタンスを生成
        Dim ta As New SC3180201TableAdapter

        '販売店システム設定から取得
        Dim dt As SC3180201DataSet.SC3180201SystemSettingDataTable _
                                = ta.GetDlrSystemSettingValue(userContext.DlrCD, _
                                                                          userContext.BrnCD, _
                                                                          AllDealerCode, _
                                                                          AllBranchCode, _
                                                                          settingName)

        If 0 < dt.Count Then
            '設定値を取得
            retValue = dt.Item(0).SETTING_VAL
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S OUT:{1}={2}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName, _
                                  retValue))

        Return retValue

    End Function
#End Region


    ''' <summary>
    ''' 前回部品交換情報削除処理
    ''' </summary>
    ''' <param name="vin">VIN</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="editPartsReplaceArray">画面パラメータでReplace選択された点検項目コードリスト</param>
    ''' <param name="prePartsReplaceDt">前回部品交換情報(取得条件VIN)</param>
    ''' <param name="strAccount">行更新アカウント</param>
    ''' <param name="dtfUpdate">行更新日時</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function NotReplacePreviousParts(ByVal vin As String, _
                                            ByVal roNum As String, _
                                            ByVal editPartsReplaceArray As List(Of String), _
                                            ByVal prePartsReplaceDt As SC3180201PreviousPartsReplaceDataTable, _
                                            ByVal strAccount As String, _
                                            ByVal dtfUpdate As Date) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim isSuccessSet As Boolean = True
        Dim tableAdapter As New SC3180201TableAdapter
        ' 点検項目がReplace→Replace以外になった場合
        For Each prePartsReplaceRow As SC3180201PreviousPartsReplaceRow In prePartsReplaceDt

            If Not editPartsReplaceArray.Contains(prePartsReplaceRow.INSPEC_ITEM_CD) Then
                '更新前DBにあって、更新対象にない

                Dim preDateCmp As Integer = DateTime.Compare(Date.Parse(prePartsReplaceRow.PREVIOUS_REPLACE_DATE, CultureInfo.InvariantCulture), _
                                                                Date.Parse(FormatDbDateTime, CultureInfo.InvariantCulture))

                If prePartsReplaceRow.RO_NUM.Equals(roNum) AndAlso
                    preDateCmp = 0 AndAlso
                    prePartsReplaceRow.PREVIOUS_REPLACE_MILE = DefaultPreviousReplaceMile Then

                    '初回交換時(削除)
                    isSuccessSet = tableAdapter.DelPreviousPartsReplace(vin, prePartsReplaceRow.INSPEC_ITEM_CD)

                ElseIf prePartsReplaceRow.RO_NUM.Equals(roNum) Then

                    'ロック
                    SelectPartsReplaceLock(vin, prePartsReplaceRow.INSPEC_ITEM_CD)
                    '2回目以降交換時(更新)
                    isSuccessSet = tableAdapter.SetDelPartsReplaceUpt(vin, _
                                                                        prePartsReplaceRow.INSPEC_ITEM_CD, _
                                                                        CType(prePartsReplaceRow.PREVIOUS_REPLACE_MILE, Decimal), _
                                                                        Date.Parse(prePartsReplaceRow.PREVIOUS_REPLACE_DATE, CultureInfo.CurrentCulture), _
                                                                        strAccount, _
                                                                        dtfUpdate, _
                                                                        CType(prePartsReplaceRow.ROW_LOCK_VERSION, Long))
                End If
            End If
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END [Result=Return:{2}, VIN:{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , isSuccessSet _
                  , vin))

        Return isSuccessSet

    End Function

    '2017/2/20 ライフサイクル対応 走行距離を完成検査で登録する End

    '【***完成検査_排他制御***】 start
    ''' <summary>
    ''' 完成検査結果更新可能判定
    ''' </summary>
    ''' <param name="svcinRowLockVersion">行ロックバージョン</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="branchCD">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <remarks>True:行ロックバージョン更新可能/False:行ロックバージョン更新不可</remarks>
    Public Function CheckUpdateFinalInspection(ByVal svcinRowLockVersion As Long,
                                   ByVal dealerCD As String,
                                   ByVal branchCD As String,
                                   ByVal roNum As String) As Boolean

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim Flg As Boolean = True
        Dim tableAdapter As New SC3180201TableAdapter
        Dim dt As DataTable


        dt = tableAdapter.GetAndLockServiceinRow(roNum, dealerCD, branchCD)

        If svcinRowLockVersion = Long.Parse(dt.Rows(0).Item("ROW_LOCK_VERSION").ToString) Then

        Else
            Flg = False
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END, Return:[{2}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , Flg))

        Return Flg
    End Function
    '【***完成検査_排他制御***】 end

End Class
