'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240101.aspx.vb
'─────────────────────────────────────
'機能： 工程管理画面
'補足： 
'作成： 2012/12/25 TMEJ 張 タブレット版SMB機能開発(工程管理)
'更新： 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新： 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発
'更新： 2014/06/18 TMEJ 丁 タブレットSMB テレマ走行距離機能開発
'更新： 2014/07/01 TMEJ 丁　 TMT_UAT対応
'更新： 2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応
'更新： 2014/07/17 TMEJ 張 文言エンコード対応
'更新： 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
'更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'更新： 2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応
'更新： 2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発
'更新： 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
'更新： 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
'更新： 2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理
'更新： 2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善）
'更新： 2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新： 2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応
'更新： 2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
'更新： 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする
'更新： 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
'更新： 2017/10/04 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新： 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新： 2020/01/07 NSK 坂本 TR-SVT-TMT-20190118-001 Stop start tagのXMLログが過去日19000101を表示している
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports Toyota.eCRB.SMB.ProcessManagement.BizLogic
Imports Toyota.eCRB.SMB.ProcessManagement.DataAccess
Imports System.Reflection.MethodBase
Imports Toyota.eCRB.CommonUtility.Common.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
'2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
Imports System.Reflection
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic

'2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

Partial Class Pages_SC3240101
    Inherits BasePage
    Implements ICallbackEventHandler
    Implements ICommonSessionControl

#Region "メンバー変数"
    ''' <summary>
    ''' 最大行数
    ''' </summary>
    ''' <remarks></remarks>
    Private m_nMaxRow As Integer
    ''' <summary>
    ''' ストール開始時間
    ''' </summary>
    ''' <remarks></remarks>
    Private m_tStallStartTime As TimeSpan
    ''' <summary>
    ''' ストール終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private m_tStallEndTime As TimeSpan
    ''' <summary>
    ''' interval時間
    ''' </summary>
    ''' <remarks></remarks>
    Private m_strInterValTime As Long = 5
    ''' <summary>
    ''' SC3240101BusinessLogic
    ''' </summary>
    ''' <remarks></remarks>
    Private businessLogic As New SC3240101BusinessLogic

    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private objStaffContext As StaffContext
    ''' <summary>
    ''' コールバックメソッドの呼び出し元に返却する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private callBackResult As String
    ''' <summary>
    ''' 画面自動リフレッシュのパラ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TabletSmbRefreshInterval As String = "TABLET_SMB_REFRESH_INTERVAL"
#End Region

#Region "列挙型"
#Region "文言ID"
    ''' <summary>
    ''' クライアントに渡す文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordID
        ''' <summary>他%1名</summary>
        id001 = 2
        ''' <summary>(日)</summary>
        id002 = 3
        ''' <summary>(月)</summary>
        id003 = 4
        ''' <summary>(火)</summary>
        id004 = 5
        ''' <summary>(水)</summary>
        id005 = 6
        ''' <summary>(木)</summary>
        id006 = 7
        ''' <summary>(金)</summary>
        id007 = 8
        ''' <summary>(土)</summary>
        id008 = 9
        ''' <summary>/</summary>
        id009 = 10
        ''' <summary>Unavailable</summary>
        id010 = 12
        ''' <summary>Rest</summary>
        id011 = 13
        ''' <summary>分</summary>
        id012 = 27
        ''' <summary>該当処理が営業時間外のため、処理を実行できません。</summary>
        id013 = 903
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ' ''' <summary>ストールに作業者が設定されていません。作業者を設定し再度処理を行ってください。</summary>
        'id014 = 907
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        ''' <summary>営業開始時間(%1:%2)以降に配置してください。</summary>
        id015 = 911
        ''' <summary>営業終了時間(%1:%2)以内に配置してください。</summary>
        id016 = 912
        ''' <summary>選択したチップを削除しますか？</summary>
        id017 = 913
        ''' <summary>選択したチップをNo Showエリアに移動しますか？</summary>
        id018 = 916
        ''' <summary>他のチップと配置時間が重複します。</summary>
        id019 = 906
        ''' <summary>そのストールでは、既に他の作業が着工されています。作業を終了後に再度処理を行ってください。</summary>
        id020 = 905
        ''' <summary>既に関連作業が開始している。関連作業を終了後に再度処理を行ってください。</summary>
        id021 = 910
        ''' <summary>使用不可チップを他のチップの上に重複させることができません。</summary>
        id022 = 914
        ''' <summary>整備種類が選択されていないため、作業開始できません。</summary>
        id023 = 904
        ''' <summary>選択したテクニシャンが4名以下してください。</summary>
        id024 = 924

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''' <summary>中断中のJobがあります。中断中のJobを除いて作業中のJobを終了しますがよろしいですか？</summary>
        id025 = 930
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
        ''' <summary>納車予定日時に間に合わなくなりますが、配置しますか？</summary>
        id026 = 935
        '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

        '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
        ''' <summary>既に関連作業が開始している。関連作業を終了後に再度処理を行ってください(Stall:ストール名)。</summary>
        id027 = 937
        '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END

        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
        ''' <summary>選択したチップを受付エリアに移動しますか？</summary>
        id028 = 938
        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

    End Enum

    Private Enum ErrorType
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ' ''' <summary>リフレッシュしても、無用なエラー、操作を元に戻す</summary>
        'TypeReposit = 1
        ' ''' <summary>リフレッシュして、再操作できるエラー</summary>
        'TypeRefresh
        ' ''' <summary>予想外エラー</summary>
        'TypeUnexpected
        ''' <summary>エラーを出した後で、画面をリフレッシュ</summary>
        TypeRefresh = 1
        ''' <summary>エラーを出した後で、画面がそのままで表示される</summary>
        TypeShowErr = 2
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''' <summary>確認ボックスを出す</summary>
        TypeShowConfirmMsg = 3
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
    End Enum
#End Region

#End Region

#Region "定数"

    ''' <summary>
    ''' プログラムID：CTメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_CTMAIN As String = "SC3200101"

    ''' <summary>
    ''' プログラムID：FMメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_FMMAIN As String = "SC3230101"

    ''' <summary>
    ''' プログラムID：SAメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_SAMAIN As String = "SC3140103"

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' プログラムID：SMメイン
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const PGMID_SMMAIN As String = "SC3220101"
    ''' <summary>
    ''' プログラムID：SMメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_SMMAIN As String = "SC3220201"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' プログラムID：工程管理(SMB)
    ''' </summary>
    Private Const PGMID_SMB As String = "SC3240101"
    ''' <summary>
    ''' プログラムID：完成検査承認
    ''' </summary>
    Private Const PGMID_INSPEC As String = "SC3180201"

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' プログラムID：追加作業承認
    ' ''' </summary>
    'Private Const PGMID_ADD As String = "SC3170301"
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' プログラムID：TCメイン画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_TEC As String = "SC3150101"

    ''' <summary>
    ''' プログラムID：来店管理画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_VISIT_MANAGER As String = "SC3100303"

    ''' <summary>
    ''' プログラムID：顧客詳細画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_CUSTOMER_DETAIL As String = "SC3080201"
    ''' <summary>
    ''' プログラムID：商品訴求コンテンツ画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PGMID_GOOD_SOLICITATION_CONTENTS As String = "SC3250101"
    ''' <summary>
    ''' 現地にシステム連携用画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OTHER_LINKAGE_PAGE As String = "SC3010501"

    '2014/06/18 TMEJ 丁 タブレットSMB テレマ走行距離機能開発 START
    ''' <summary>
    ''' 走行距離履歴画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MILEAGE_HISTORY_PAGE As String = "SC3240601"
    '2014/06/18 TMEJ 丁 タブレットSMB テレマ走行距離機能開発 END

    ''' <summary>
    ''' 連絡先ボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_EVENT_TEL As String = "return schedule.appExecute.executeCont();"

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 追加作業遷移
    ''' </summary>
    Private Const ADDWORKCONFIRM_REDIRECT As String = "202"

    ''' <summary>
    ''' 完成検査遷移
    ''' </summary>
    Private Const COMPLETIONINSPECTION_REDIRECT As String = "402"

    '2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
    ''' <summary>
    ''' G-BOOKボタンオペレーションCODE
    ''' </summary>
    Private Const GBOOK_REDIRECT As String = "3100"
    '2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

    ' ''' <summary>
    ' ''' SessionValue(R/O No)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueORDERNO As String = "Redirect.ORDERNO"
    ' ''' <summary>
    ' ''' SessionValue(枝番)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueSRVADDSEQ As String = "Redirect.SRVADDSEQ"
    ' ''' <summary>
    ' ''' SessionValue(編集フラグ)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueEDIT As String = "Redirect.EDITFLG"
    ' ''' <summary>
    ' ''' SessionValue(予約ID)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const SessionValueREZID As String = "Redirect.REZID="

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' SessionKey(DearlerCode):ログインユーザーのDMS販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_DEARLER_CODE As String = "Session.Param1"
    ''' <summary>
    ''' SessionKey(BranchCode):ログインユーザーのDMS店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_BRANCH_CODE As String = "Session.Param2"
    ''' <summary>
    ''' SessionKey(LoginUserID):ログインユーザーのアカウント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_LOGIN_USER_ID As String = "Session.Param3"
    ''' <summary>
    ''' SessionKey(SAChipID):来店管理番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SA_CHIP_ID As String = "Session.Param4"
    ''' <summary>
    ''' SessionKey(BASREZID):DMS予約ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_BASREZID As String = "Session.Param5"
    ''' <summary>
    ''' SessionKey(R_O):RO番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_R_O As String = "Session.Param6"
    ''' <summary>
    ''' SessionKey(SEQ_NO):RO作業連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_SEQ_NO As String = "Session.Param7"
    ''' <summary>
    ''' SessionKey(VIN_NO):車両登録No.のVIN
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VIN_NO As String = "Session.Param8"
    ''' <summary>
    ''' SessionKey(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VIEW_MODE As String = "Session.Param9"
    ''' <summary>
    ''' SessionKey(Format)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_FORMAT As String = "Session.Param10"
    ''' <summary>
    ''' SessionKey(DISP_NUM)：画面番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_DISP_NUM As String = "Session.DISP_NUM"
    ''' <summary>
    ''' SessionKey(DealerCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_DEALERCODE As String = "DealerCode"
    ''' <summary>
    ''' SessionKey(BranchCode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_BRANCHCODE As String = "BranchCode"
    ''' <summary>
    ''' SessionKey(LoginUserID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_LOGINUSERID As String = "LoginUserID"
    ''' <summary>
    ''' SessionKey(SAChipID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_SACHIPID As String = "SAChipID"
    ''' <summary>
    ''' SessionKey(BASREZID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_BASREZID As String = "BASREZID"
    ''' <summary>
    ''' SessionKey(R_O)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_RO As String = "R_O"
    ''' <summary>
    ''' SessionKey(SEQ_NO)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_SEQ_NO As String = "SEQ_NO"
    ''' <summary>
    ''' SessionKey(VIN_NO)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_VIN_NO As String = "VIN_NO"
    ''' <summary>
    ''' SessionKey(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_VIEWMODE As String = "ViewMode"

    '2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 START
    ''' <summary>
    ''' SessionKey(JobDetailID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TONEC_JOBDTLID As String = "JOB_DTL_ID"
    '2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 END

    ''' <summary>
    ''' SessionKey：ストールID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_TEC_STALLID As String = "SC3150101.StallId"

    ''' <summary>
    ''' SessionValue(ViewMode)：編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_VIEWMODE_EDIT As String = "0"

    ''' <summary>
    ''' SessionValue(ViewMode)：プレビュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_VIEWMODE_PREVIEW As String = "1"

    ''' <summary>
    ''' SessionValue(画面番号)：RO一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_DISPNUM_ROPREVIEW As String = "14"

    ''' <summary>
    ''' SessionValue(画面番号)：キャンペーン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_DISPNUM_CAMPAIGN As String = "15"

    ''' <summary>
    ''' SessionValue(画面番号)：追加作業一覧
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_DISPNUM_ADDWORKLIST As String = "22"

    ''' <summary>
    ''' SessionValue(画面番号)：追加作業承認
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONVALUE_DISPNUM_ADDWORKCONFIRM As String = "24"

    ''' <summary>
    ''' １つスベース
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ONE_SPACE As String = " "

    '2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
    ''' <summary>
    ''' SessionKey(VCL_ID)：車両ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSIONKEY_VCL_ID As String = "Session.VCL_ID"
    '2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

    ''' <summary>
    ''' フッターイベントの置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FOOTER_REPLACE_EVENT As String = "FooterButtonClick({0});"

    ''' <summary>
    ''' 日付のフォーマット:yyyy/MM/dd
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FORMATDATE_YYYYMMDD As String = "yyyy/MM/dd"

    ''' <summary>
    ''' 日付のフォーマット:yyyy/MM/dd HH:mm:ss
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FORMATDATE_YYYYMMDDHHMMSS As String = "yyyy/MM/dd HH:mm:ss"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 確認ボックス表示タイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SHOW_CONFIRM_MSGBOX As Long = 101

    ''' <summary>
    ''' 中断されてるJob再開ステータス(まだ設定してない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESTARTJOB_NOTSET As Long = 0

    ''' <summary>
    ''' 中断されてるJob再開ステータス(再開する)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESTARTJOB_YES As Long = 1

    ''' <summary>
    ''' 中断されてるJob再開ステータス(再開しない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESTARTJOB_NO As Long = 2
    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
#End Region

#Region "初期表示"
    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
    ''' </history>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info(GetCurrentMethod.Name & " Start")
        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current

        'コールバックスクリプトの生成
        ScriptManager.RegisterStartupScript(
            Me,
            Me.GetType(),
            "gCallbackSC3240101",
            String.Format(CultureInfo.InvariantCulture,
                          "gCallbackSC3240101.beginCallback = function () {{ {0}; }};",
                          Page.ClientScript.GetCallbackEventReference(Me, "gCallbackSC3240101.packedArgument", _
                                                                      "gCallbackSC3240101.endCallback", "", True)
                          ), True)

        If Not Me.IsPostBack AndAlso Not Me.IsCallback Then
            'セッションバリューを設定
            Me.GetSessionValue()
            '日付の設定
            hidShowDate.Value = ""
            Me.hidDlrCD.Value = objStaffContext.DlrCD
            Me.hidBrnCD.Value = objStaffContext.BrnCD
            Me.hidAccount.Value = objStaffContext.Account
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'MM/ddとHH:mmのデータフォーマットを取得する
            Me.hidDateFormatMMdd.Value = DateTimeFunc.GetDateFormat(11)
            Me.hidDateFormatHHmm.Value = DateTimeFunc.GetDateFormat(14)
            Me.hidDateFormatYYYYMMddHHmm.Value = DateTimeFunc.GetDateFormat(2)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            Using svcCommonBiz As New ServiceCommonClassBusinessLogic
                Me.hidRestAutoJudgeFlg.Value = svcCommonBiz.GetDlrSystemSettingValueBySettingName("REST_AUTO_JUDGE_FLG")
            End Using
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
            '2017/10/04 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            'Me.hidStandardWashTime.Value = CType(businessLogic.GetStandardWashTime(objStaffContext), String)
            Dim serviceSettingDt As SC3240101DataSet.SC3240101ServiceSettingDataTable = businessLogic.GetServiceSettingTime(objStaffContext)
            If 0 < serviceSettingDt.Rows.Count Then
                '洗車標準時間
                Me.hidStandardWashTime.Value = CType(serviceSettingDt.Rows(0).Item("STD_CARWASH_TIME"), String)

                '検査標準時間
                Me.hidStandardInspectionTime.Value = CType(serviceSettingDt.Rows(0).Item("STD_INSPECTION_TIME"), String)

                ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                ' 標準納車準備時間
                Me.hidStandardDeliPreTime.Value = CType(serviceSettingDt.Rows(0).Item("STD_DELI_PREPARATION_TIME"), String)
                ' 標準納車時間
                Me.hidStandardDeliWrTime.Value = CType(serviceSettingDt.Rows(0).Item("STD_DELI_TIME"), String)
                ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

            Else
                Me.hidStandardWashTime.Value = "0"
                Me.hidStandardInspectionTime.Value = "0"

                ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                Me.hidStandardDeliPreTime.Value = "15"
                Me.hidStandardDeliWrTime.Value = "15"
                ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

            End If
            '2017/10/04 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

            ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
            'Dim deliPreTime As Long
            'Dim deliWorkTime As Long
            'Dim dt As SC3240101DataSet.SC3240101StandardTimeDataTable = businessLogic.GetStandardDeliveryTime(objStaffContext)
            'If dt.Rows.Count = 1 Then
            '    deliPreTime = CType(dt.Rows(0).Item("DELIVERYPRE_STANDARD_LT"), Long)
            '    deliWorkTime = CType(dt.Rows(0).Item("DELIVERYWR_STANDARD_LT"), Long)
            'Else
            '    deliPreTime = 15
            '    deliWorkTime = 15
            'End If
            'Me.hidStandardDeliPreTime.Value = CType(deliPreTime, String)
            'Me.hidStandardDeliWrTime.Value = CType(deliWorkTime, String)
            ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            Me.hidOpeCode.Value = CType(objStaffContext.OpeCD, String)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
            'スタッフストール表示区分を初期化
            businessLogic.InitStaffStallDispType(objStaffContext)
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

            '店舗稼動時間情報の取得
            GetBranchOperationgHours()

            '中断理由ウィンドウに中断メモのテンプレートをバインドする
            BindStopMemoTemplate()
            SetCalendarDay(0)
        ElseIf Not Me.IsCallback Then
            If Not String.IsNullOrEmpty(Request.Form.Item("__EVENTARGUMENT")) Then
                Dim serializer = New JavaScriptSerializer
                Dim argument As PostBackParamClass = New PostBackParamClass
                argument = serializer.Deserialize(Of PostBackParamClass)(Request.Form.Item("__EVENTARGUMENT"))
                If Not String.IsNullOrEmpty(argument.OperationCode) Then
                    '2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
                    'Me.Redirect(argument)
                    If GBOOK_REDIRECT.Equals(argument.OperationCode) Then
                        Me.MileageRedirect(argument)
                    Else
                        Me.Redirect(argument)
                    End If
                    '2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                ElseIf Not String.IsNullOrEmpty(argument.StallId) Then
                    'チーフテクニシャンがストールにタップして、タップされたストールIDでTC画面に遷移する
                    'タップされたストールIDを設定
                    Me.SetValue(ScreenPos.Next, SESSIONKEY_TEC_STALLID, CType(argument.StallId, Decimal))
                    'TC画面に遷移
                    Me.RedirectNextScreen(PGMID_TEC)
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                End If
            End If
        End If

        'フッター初期化
        Me.InitFooterEvent()

        'hiddenコントロールに開始時間と終了時間を設定する
        If hidStallStartTime.Value <> "" Then
            m_tStallStartTime = New TimeSpan(CType(hidStallStartTime.Value.Substring(0, 2), Integer), CType(hidStallStartTime.Value.Substring(3, 2), Integer), 0)
        End If
        If hidStallEndTime.Value <> "" Then
            m_tStallEndTime = New TimeSpan(CType(hidStallEndTime.Value.Substring(0, 2), Integer), CType(hidStallEndTime.Value.Substring(3, 2), Integer), 0)
        End If

        If Not Me.IsCallback AndAlso Not Me.Page.IsPostBackEventControlRegistered Then
            'サーバ時間を文字列として取得して、HiddenFieldに格納.（yyyy/MM/dd HH:mm:ss形式）
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'Me.hidServerTime.Value = DateTimeFunc.FormatDate(1, DateTimeFunc.Now(objStaffContext.DlrCD))
            Me.hidServerTime.Value = DateTimeFunc.Now(objStaffContext.DlrCD).ToString(FORMATDATE_YYYYMMDDHHMMSS, CultureInfo.CurrentCulture)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            'hiddenコントロールにclient端用の文言を設定する
            SendWordToClient()

            '時間、ストール名、テクニク名がrepeaterコントロールとバインドする
            Me.DataBindWithControl()
        End If
        Logger.Info(GetCurrentMethod.Name & " End")
    End Sub

    ''' <summary>
    ''' hiddenコントロールにclient端用の文言を設定する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SendWordToClient()
        Dim sbWord As StringBuilder = New StringBuilder
        sbWord.Append("{")
        '全てwordをループする
        For Each i As WordID In [Enum].GetValues(GetType(WordID))
            With sbWord
                .Append("""")
                .Append(Convert.ToInt32(i).ToString(CultureInfo.InvariantCulture))
                .Append(""":""")
                '2014/07/17 TMEJ 張 文言エンコード対応 START
                '.Append(WebWordUtility.GetWord(PGMID_SMB, CType(i, Decimal)))
                .Append(HttpUtility.HtmlEncode(WebWordUtility.GetWord(PGMID_SMB, CType(i, Decimal))))
                '2014/07/17 TMEJ 張 文言エンコード対応 END
                .Append(""",")
            End With
        Next i

        '最後の","を削除する
        sbWord.Remove(sbWord.Length - 1, 1)
        sbWord.Append("}")
        Me.hidMsgData.Value = sbWord.ToString()
    End Sub

    ''' <summary>
    ''' 検査画面からのセッションバリューを取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetSessionValue()
        Me.hidSessionValue.Value = ""

        Dim sbSession As New StringBuilder
        sbSession.Append("{""STALL_USE_ID"":""")
        'ストール利用ID
        If (MyBase.ContainsKey(ScreenPos.Current, "Session.STALL_USE_ID")) Then
            sbSession.Append(MyBase.GetValue(ScreenPos.Current, "Session.STALL_USE_ID", True).ToString())
            sbSession.Append(""",")
        Else
            Return
        End If
        '表示日付
        If (MyBase.ContainsKey(ScreenPos.Current, "Session.DATE")) Then
            sbSession.Append("""DATE"":""")
            sbSession.Append(MyBase.GetValue(ScreenPos.Current, "Session.DATE", True).ToString())
            sbSession.Append(""",")
        Else
            Return
        End If
        'ストール利用ステータス
        If (MyBase.ContainsKey(ScreenPos.Current, "Session.STALL_USE_STATUS")) Then
            sbSession.Append("""STALL_USE_STATUS"":""")
            sbSession.Append(MyBase.GetValue(ScreenPos.Current, "Session.STALL_USE_STATUS", True).ToString())
            sbSession.Append(""",")
        Else
            Return
        End If
        'サービスステータス
        If (MyBase.ContainsKey(ScreenPos.Current, "Session.SVC_STATUS")) Then
            sbSession.Append("""SVC_STATUS"":""")
            sbSession.Append(MyBase.GetValue(ScreenPos.Current, "Session.SVC_STATUS", True).ToString())
            sbSession.Append(""",")
        Else
            Return
        End If
        'サブエリアタイプ
        If (MyBase.ContainsKey(ScreenPos.Current, "Session.SUB_CHIP_TYPE")) Then
            sbSession.Append("""SUB_CHIP_TYPE"":""")
            sbSession.Append(MyBase.GetValue(ScreenPos.Current, "Session.SUB_CHIP_TYPE", True).ToString())
            sbSession.Append(""",")
        Else
            Return
        End If
        '検査ステータス
        If (MyBase.ContainsKey(ScreenPos.Current, "Session.INSPECTION_STATUS")) Then
            sbSession.Append("""INSPECTION_STATUS"":""")
            sbSession.Append(MyBase.GetValue(ScreenPos.Current, "Session.INSPECTION_STATUS", True).ToString())
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            'sbSession.Append("""")
            sbSession.Append(""",")
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
        Else
            Return
        End If
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        '仮置きフラグ
        If (MyBase.ContainsKey(ScreenPos.Current, "Session.TEMP_FLG")) Then
            sbSession.Append("""TEMP_FLG"":""")
            sbSession.Append(MyBase.GetValue(ScreenPos.Current, "Session.TEMP_FLG", True).ToString())
            sbSession.Append(""",")
        Else
            Return
        End If

        'RO番号
        If (MyBase.ContainsKey(ScreenPos.Current, "Session.RO_NUM")) Then
            sbSession.Append("""RO_NUM"":""")
            sbSession.Append(MyBase.GetValue(ScreenPos.Current, "Session.RO_NUM", True).ToString())
            sbSession.Append(""",")
        Else
            Return
        End If
        'RO連番
        If (MyBase.ContainsKey(ScreenPos.Current, "Session.RO_SEQ")) Then
            sbSession.Append("""RO_SEQ"":""")
            sbSession.Append(MyBase.GetValue(ScreenPos.Current, "Session.RO_SEQ", True).ToString())
            sbSession.Append(""",")
        Else
            Return
        End If
        'ROステータス
        If (MyBase.ContainsKey(ScreenPos.Current, "Session.RO_STATUS")) Then
            sbSession.Append("""RO_STATUS"":""")
            sbSession.Append(MyBase.GetValue(ScreenPos.Current, "Session.RO_STATUS", True).ToString())
            sbSession.Append("""")
        Else
            Return
        End If
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
        sbSession.Append("}")
        'hiddenコントロールに設定する
        Me.hidSessionValue.Value = HttpUtility.HtmlEncode(sbSession.ToString())
    End Sub
#End Region

#Region "チップ情報の取得処理"
    ''' <summary>
    ''' 当ページのストールチップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <param name="stallIdList">ストールIDリスト</param>
    ''' <param name="dtStallStartTime">営業開始時間</param>
    ''' <param name="dtStallEndTime">営業終了時間</param>
    ''' <param name="theTime">この日時後変更があったチップを取得</param>
    ''' <returns>ストールチップ情報テーブル</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理
    ''' </history>
    ''' 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    Private Function GetAllStallChipInfo(ByVal stallIdList As List(Of Decimal), _
                                         ByVal dtStallStartTime As Date, _
                                         ByVal dtStallEndTime As Date, _
                                         Optional ByVal theTime As Date = Nothing) As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable
        'Private Function GetAllStallChipInfo(ByVal stallIdList As List(Of Long), ByVal dtStallStartTime As Date, ByVal dtStallEndTime As Date) As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        Logger.Info(GetCurrentMethod.Name & " Start")

        '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 START

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} ③SC3240101_チップ情報取得処理 START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 END

        'チップ情報テーブル
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
        '                businessLogic.GetAllStallChip(objStaffContext.DlrCD, objStaffContext.BrnCD, stallIdList, _
        '                                    dtStallStartTime, dtStallEndTime)
        Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
                                businessLogic.GetAllStallChip(objStaffContext.DlrCD, objStaffContext.BrnCD, stallIdList, _
                                                    dtStallStartTime, dtStallEndTime, theTime)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 START

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} ③SC3240101_チップ情報取得処理 END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 END

        Logger.Info(GetCurrentMethod.Name & " End")
        Return dtChipInfo
    End Function


    ''' <summary>
    ''' StallChipInfoストール上チップ情報テーブルからサービス入庫IDリストを取得する
    ''' </summary>
    ''' <param name="dtChipInfo">StallChipInfoテーブル</param>
    ''' <returns>サービス入庫IDリスト</returns>
    ''' <remarks></remarks>
    Private Function GetSvcIdListFromStallChipTable(ByVal dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable) As List(Of Decimal)
        Return businessLogic.GetSvcIdListFromStallChipTable(dtChipInfo)
    End Function

    ''' <summary>
    ''' 当ページのリレーションチップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <param name="svcInIdList">サービス入庫IDのリスト</param>
    ''' <remarks></remarks>
    Private Function GetRelationChipInfo(ByVal svcInIdList As List(Of Decimal)) As String

        Logger.Info(GetCurrentMethod.Name & " Start")
        Dim dtNow As Date = DateTimeFunc.Now(objStaffContext.DlrCD)

        '関連チップ情報を取得
        Dim dtRelationChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassRelationChipInfoDataDataTable = _
                businessLogic.GetAllRelationChipInfo(svcInIdList)

        Logger.Info(GetCurrentMethod.Name & " End")
        Return HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtRelationChipInfo))
    End Function

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 当ページのリレーションチップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <param name="stallUseIdList">サービス入庫IDのリスト</param>
    ''' <remarks></remarks>
    Private Function GetWorkingChipHisInfo(ByVal stallUseIdList As List(Of Decimal)) As TabletSMBCommonClassDataSet.TabletSmbCommonClassChipHisDataTable

        Logger.Info(GetCurrentMethod.Name & " Start")

        '関連チップ情報を取得
        Dim dtChipHisInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassChipHisDataTable = _
                businessLogic.GetWorkingChipHisInfo(stallUseIdList)

        Logger.Info(GetCurrentMethod.Name & " End")
        Return dtChipHisInfo
    End Function

    ''' <summary>
    ''' チップ情報テーブルより、作業中チップの履歴を取得する
    ''' </summary>
    ''' <param name="dtChipInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetChipHis(ByVal dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable) As String
        Dim stallUseIdList As New List(Of Decimal)
        For Each drChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoRow In dtChipInfo
            stallUseIdList.Add(drChipInfo.STALL_USE_ID)
        Next

        Dim dtChipHisInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassChipHisDataTable = _
                        Me.GetWorkingChipHisInfo(stallUseIdList)
        '作業中チップの履歴情報
        Return HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtChipHisInfo))
    End Function
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応 START
    ''' <summary>
    ''' チップ情報テーブルより、作業中チップの履歴を取得し、JSON形式の文字列データを格納する処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallStartTime">稼働時間From</param>
    ''' <param name="stallEndTime">稼働時間To</param>
    ''' <returns>履歴情報</returns>
    ''' <remarks></remarks>
    Private Function GetChipHisFromStallTime(ByVal dealerCode As String, _
                                ByVal branchCode As String, _
                                ByVal stallStartTime As Date, _
                                ByVal stallEndTime As Date _
                                ) As String

        Logger.Info(GetCurrentMethod.Name & " Start")

        '作業中チップの履歴を取得
        Dim dtChipHisInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassChipHisDataTable = _
                        businessLogic.GetWorkingChipHisInfoFromStallTime(dealerCode, branchCode, stallStartTime, stallEndTime)

        Logger.Info(GetCurrentMethod.Name & " End")

        '作業中チップの履歴情報
        Return HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtChipHisInfo))
    End Function
    '2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応 END

    ''' <summary>
    ''' 当ページの仮仮チップ情報を取得する
    ''' </summary>
    ''' <param name="stallIdList">ストールIDリスト</param>
    ''' <param name="dtStallStartTime">営業開始時間</param>
    ''' <param name="dtStallEndTime">営業終了時間</param>
    ''' <returns>ストール仮仮チップ情報テーブル</returns>
    ''' <remarks></remarks>
    Private Function GetAllStallKariKariChipInfo(ByVal stallIdList As List(Of Decimal), ByVal dtStallStartTime As Date, ByVal dtStallEndTime As Date) As TabletSMBCommonClassDataSet.TabletSmbCommonClassKariKariChipInfoDataTable

        Logger.Info(GetCurrentMethod.Name & " Start")
        Dim dtNow As Date = DateTimeFunc.Now(objStaffContext.DlrCD)

        '仮仮チップ情報を取得する
        Dim dtKariKariChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassKariKariChipInfoDataTable = _
                businessLogic.GetAllStallKariKariChip(stallIdList, dtNow, dtStallStartTime, dtStallEndTime)

        Logger.Info(GetCurrentMethod.Name & " End")
        Return dtKariKariChipInfo
    End Function

#End Region

#Region "タイム、ストール名エリアにデータbind"
    ''' <summary>
    ''' '時間、ストール名、テクニク名がrepeaterコントロールとバインドする
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DataBindWithControl()

        Logger.Info(GetCurrentMethod.Name & " Start")

        '時間をrepeaterコントロールとbindする
        Me.stallTimeRepeater.DataSource = Me.CreateDataSourceStallTime()
        Me.stallTimeRepeater.DataBind()
        'ストール名とテクニク名をrepeaterコントロールとbindする
        Me.stallNameRepeater.DataSource = Me.CreateDataSourceStallName()
        Me.stallNameRepeater.DataBind()
        Logger.Info(GetCurrentMethod.Name & " End")
    End Sub

    ''' <summary>
    ''' 時間のdataviewを取得する
    ''' </summary>
    ''' <returns>時間のdataview</returns>
    ''' <remarks></remarks>
    Private Function CreateDataSourceStallTime() As SC3240101DataSet.SC3240101StringValueDataTable

        Logger.Info(GetCurrentMethod.Name & " Start")
        Using dtHour As New SC3240101DataSet.SC3240101StringValueDataTable
            Dim drHour As SC3240101DataSet.SC3240101StringValueRow
            Dim bStartTimeFlg As Boolean = False
            Dim bEndTimeFlg As Boolean = False
            Dim nEndHour As Long = m_tStallEndTime.Hours
            '分があれば、例えば、20:20分の場合、終了時間が21時まで表示される
            If m_tStallEndTime.Minutes > 0 Then
                nEndHour = nEndHour + 1
            End If
            'time行の時間を計算する
            For i As Integer = m_tStallStartTime.Hours To CType(nEndHour, Integer)
                drHour = CType(dtHour.NewRow, SC3240101DataSet.SC3240101StringValueRow)
                Dim sbString As New StringBuilder
                sbString.Append(DateTime.Parse("00:00", CultureInfo.InvariantCulture).AddHours(i).Hour.ToString(CultureInfo.InvariantCulture))
                sbString.Append(WebWordUtility.GetWord(PGMID_SMB, 11))
                sbString.Append("00")
                drHour.COL1 = sbString.ToString()
                '1行datarowをdatatableに追加する
                dtHour.Rows.Add(drHour)
            Next

            Logger.Info(GetCurrentMethod.Name & " End")
            'dataviewを戻す
            Return dtHour
        End Using
    End Function

    ''' <summary>
    ''' ストール名、テクニク名のdataviewを取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateDataSourceStallName() As SC3240101DataSet.SC3240101StallBindDataTable

        Logger.Info(GetCurrentMethod.Name & " Start")

        'ストールの情報をdbから取得する(ストールid、名前、短い名前(4桁以下))
        Dim dtStallInfo As SC3240101DataSet.SC3240101AllStallDataTable
        Dim dtShowDate As Date = Date.Parse(hidShowDate.Value, CultureInfo.InvariantCulture)
        dtStallInfo = businessLogic.GetAllStall(objStaffContext)

        Using dt As New SC3240101DataSet.SC3240101StallBindDataTable
            Dim dr As SC3240101DataSet.SC3240101StallBindRow

            '全てレコードをループする
            For i As Integer = 1 To dtStallInfo.Rows.Count
                dr = CType(dt.NewRow, SC3240101DataSet.SC3240101StallBindRow)
                'ストールid
                dr.STALLID = CType(dtStallInfo.Rows(i - 1)(0), Decimal)
                '短い名前(4桁以下)
                dr.STALLNAME = CType(dtStallInfo.Rows(i - 1)(2), String)
                'ストール番目
                dr.STALLNO = (i).ToString(CultureInfo.InvariantCulture)
                'datatableに追加
                dt.Rows.Add(dr)
            Next

            '最大行数を設定する
            m_nMaxRow = dtStallInfo.Rows.Count
            hidMaxRow.Value = CType(m_nMaxRow, String)
            Logger.Info(GetCurrentMethod.Name & " End")
            Return dt
        End Using
    End Function

    ''' <summary>
    ''' 営業時間を取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetBranchOperationgHours()

        Logger.Info(GetCurrentMethod.Name & " Start")

        Dim dtStartTime As Date
        Dim dtEndTime As Date
        Dim dt = businessLogic.GetBranchOperatingHours(objStaffContext)
        dtStartTime = CType(dt(0)(0), Date)
        dtEndTime = CType(dt(0)(1), Date)

        'ストール開始時間
        m_tStallStartTime = New TimeSpan(dtStartTime.Hour, dtStartTime.Minute, 0)
        'ストール終了時間
        m_tStallEndTime = New TimeSpan(dtEndTime.Hour, dtEndTime.Minute, 0)

        'interval時間
        m_strInterValTime = businessLogic.GetIntervalTime(objStaffContext)

        'hiddenコントロールに開始時間と終了時間を設定する
        hidStallStartTime.Value = m_tStallStartTime.ToString().Substring(0, 5)
        hidStallEndTime.Value = m_tStallEndTime.ToString().Substring(0, 5)
        hidIntervlTime.Value = CType(m_strInterValTime, String)

        '画面自動リフレッシュ時間単位を取得する
        Dim systemEnv As New SystemEnvSetting
        Dim strTabletSmbRefreshInterval As String = systemEnv.GetSystemEnvSetting(TabletSmbRefreshInterval).PARAMVALUE
        If String.IsNullOrEmpty(strTabletSmbRefreshInterval) Then
            hidTabletSmbRefreshInterval.Value = "180"
        Else
            hidTabletSmbRefreshInterval.Value = strTabletSmbRefreshInterval
        End If
        Logger.Info(GetCurrentMethod.Name & " End")
    End Sub

    ''' <summary>
    ''' 中断理由を取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindStopMemoTemplate()
        Logger.Info(GetCurrentMethod.Name & " Start")
        Dim dtStopMemo As SC3240101DataSet.SC3240101StringValueDataTable = businessLogic.GetStopMemoTemplate(objStaffContext.DlrCD, objStaffContext.BrnCD)
        Me.dpDetailStopMemo.Items.Clear()
        Me.dpDetailStopMemo.DataSource = dtStopMemo
        Me.dpDetailStopMemo.DataTextField = "COL1"
        Me.dpDetailStopMemo.DataBind()
        Me.dpDetailStopMemo.Items.Insert(0, WebWordUtility.GetWord(PGMID_SMB, 29))

        Me.lblDetailStopMemo.Text = WebWordUtility.GetWord(PGMID_SMB, 29)

        Logger.Info(GetCurrentMethod.Name & " End")
    End Sub
#End Region

#Region "カレンダー処理"

    ''' <summary>
    ''' 日付を画面に設定する
    ''' </summary>
    ''' <param name="nOffsetDays"></param>
    ''' <remarks></remarks>
    Protected Function SetCalendarDay(ByVal nOffsetDays As Integer) As Date
        Logger.Info(GetCurrentMethod.Name & " Start")

        Dim dtDay As Date
        If hidShowDate.Value = "" Then
            dtDay = Now
        Else
            dtDay = CType(hidShowDate.Value, Date)
            dtDay = DateAdd("d", nOffsetDays, dtDay)
        End If
        'yyyy/MM/ddの形式でhiddenコントロールに保存する
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'hidShowDate.Value = DateTimeFunc.FormatDate(21, dtDay)
        hidShowDate.Value = dtDay.ToString(FORMATDATE_YYYYMMDD, CultureInfo.InvariantCulture)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        Logger.Info(GetCurrentMethod.Name & " End")

        Return dtDay
    End Function
#End Region

#Region "フッター制御"

#Region "各ボタンタップイベント登録"

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        Logger.Info(GetCurrentMethod.Name & " Start")

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''メインメニュー
        'Dim footerMainMenuButton As CommonMasterFooterButton = _
        'CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)

        ''メインボタンのClientとサーバ両方のクリックイベントをbind
        'AddHandler footerMainMenuButton.Click, AddressOf footerMainMenuButton_Click
        'footerMainMenuButton.OnClientClick = "return MainButtonClick();"


        ''基盤にフッターボタンが追加されるまでコメント
        ''SMB
        'Dim footerSMBButton As CommonMasterFooterButton = _
        'CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
        'footerSMBButton.OnClientClick = "SMBButtonEvent();"

        ''SMBで不要なフッターボタンを非表示にする
        'Me.HiddenFooterButton()

        '各ボタンタップイベントを登録する

        'メインボタン
        Me.InitMainButtonEvent()

        'RO一覧ボタン
        Me.InitROListButtonEvent()

        '追加作業一覧ボタン
        Me.InitAddWorkListButtonEvent()

        '電話帳ボタン
        Me.InitContactButtonEvent()

        'SMBボタンボタン
        Me.InitSmbButtonEvent()

        '顧客詳細ボタン
        Me.InitCustomerDetailButtonEvent()

        '商品訴求コンテンツボタン
        Me.InitGoodsSolicitationContentsButtonEvent()

        'キャンペーンボタン
        Me.InitCampaignButtonEvent()

        '来店管理ボタン
        Me.InitVisitManagerButtonEvent()

        'TCメインボタン
        Me.InitTCMainButtonEvent()

        'FMメイン
        Me.InitFMMainButtonEvent()
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(GetCurrentMethod.Name & " End")

    End Sub

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' メインボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitMainButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SA、SM、FMの場合、メインメニュータップすると、対応権限のメイン画面に戻す
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA _
            OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.SM _
            OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.FM Then

            'メインメニュー
            Dim footerMainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)

            'メインボタンのClientとサーバ両方のクリックイベントをbind
            AddHandler footerMainMenuButton.Click, AddressOf footerMainMenuButton_Click
            footerMainMenuButton.OnClientClick = "return MainButtonClick();"

        Else

            'CT、CHTの場合、メインメニュータップすると、ボタンを切替える
            Dim footerMainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)

            'メインボタンのClientとサーバ両方のクリックイベントをbind
            footerMainMenuButton.OnClientClick = "return ChangeButtonEvent();"

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' RO一覧ボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitROListButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SA、SM、FM、CT、CHTの場合、RO一覧ボタンタップすると、RO一覧画面に遷移
        If objStaffContext.OpeCD = Operation.CT _
            OrElse objStaffContext.OpeCD = Operation.FM _
            OrElse objStaffContext.OpeCD = Operation.SA _
            OrElse objStaffContext.OpeCD = Operation.SM _
            OrElse objStaffContext.OpeCD = Operation.CHT Then

            'RO一覧ボタン
            Dim footerRoListButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RepairOrderList)

            'メインボタンのClientとサーバ両方のクリックイベントをbind
            AddHandler footerRoListButton.Click, AddressOf footerRoListMenuButton_Click
            footerRoListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                             FOOTER_REPLACE_EVENT, _
                                                             CType(FooterMenuCategory.RepairOrderList, String))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 追加作業一覧ボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitAddWorkListButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'CT、CHT、FMの場合、SMBボタンのイベントを登録する
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.CT _
            OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.CHT _
            OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.FM Then

            '追加作業一覧ボタン
            Dim footerAddWorkListButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.AddWorkList)
            'イベントをbindする
            AddHandler footerAddWorkListButton.Click, AddressOf footerAddWorkListMenuButton_Click
            footerAddWorkListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                                  FOOTER_REPLACE_EVENT, _
                                                                  CType(FooterMenuCategory.AddWorkList, String))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 連絡先ボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitContactButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '連絡先ボタンの設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Contact)
        If Not IsNothing(telDirectoryButton) Then
            telDirectoryButton.OnClientClick = FOOTER_EVENT_TEL
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' SMBボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitSmbButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SA、SM、FMの場合、SMBボタンのイベントを登録する
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA _
            OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.SM _
            OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.FM Then

            'SMB
            Dim footerSMBButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.SMB)
            footerSMBButton.OnClientClick = "return ChangeButtonEvent();"

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 顧客詳細ボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitCustomerDetailButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SA、SMの場合、顧客詳細ボタンのイベントを登録する
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA _
            OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.SM Then

            '顧客詳細ボタン
            Dim footerCustomerDetailButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.CustomerDetail)
            'イベントをbindする
            footerCustomerDetailButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                                     FOOTER_REPLACE_EVENT, _
                                                                     CType(FooterMenuCategory.CustomerDetail, String))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 商品訴求コンテンツボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitGoodsSolicitationContentsButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SA、SMの場合、商品訴求コンテンツボタンのイベントを登録する
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA _
            OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.SM Then

            '商品訴求コンテンツボタン
            Dim footerGoodsSolicitationContentsButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.GoodsSolicitationContents)
            'イベントをbindする
            AddHandler footerGoodsSolicitationContentsButton.Click, AddressOf footerGoodsSolicitationContentsMenuButton_Click
            footerGoodsSolicitationContentsButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                                                FOOTER_REPLACE_EVENT, _
                                                                                CType(FooterMenuCategory.GoodsSolicitationContents, String))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' キャンペーンボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitCampaignButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'SA、SMの場合、キャンペーンボタンタップすると、現地のキャンペーン画面に遷移
        If objStaffContext.OpeCD = Operation.SA _
            OrElse objStaffContext.OpeCD = Operation.SM Then

            'キャンペーンボタン
            Dim footerCampaignButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Campaign)

            'メインボタンのClientとサーバ両方のクリックイベントをbind
            AddHandler footerCampaignButton.Click, AddressOf footerCampaignMenuButton_Click
            footerCampaignButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                               FOOTER_REPLACE_EVENT, _
                                                               CType(FooterMenuCategory.Campaign, String))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 来店管理ボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitVisitManagerButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'CHTの場合、FMメインメニュータップすると、FMメイン画面に遷移する
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA _
           OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.SM Then

            'メインメニュー
            Dim footerVisitManagerMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ReserveManagement)

            'メインボタンのClientとサーバ両方のクリックイベントをbind
            AddHandler footerVisitManagerMenuButton.Click, AddressOf footerVisitManagerMenuButton_Click
            footerVisitManagerMenuButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                                     FOOTER_REPLACE_EVENT, _
                                                                     CType(FooterMenuCategory.ReserveManagement, String))

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' TCメインボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitTCMainButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'CHTの場合、TCメインメニュータップすると、処理なし
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.CHT Then

            'メインメニュー
            Dim footerTCMainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TechnicianMain)
            footerTCMainMenuButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                                 FOOTER_REPLACE_EVENT, _
                                                                 CType(FooterMenuCategory.TechnicianMain, String))
            '2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応 START
            footerTCMainMenuButton.Enabled = False
            '2016/11/24 NSK 竹中 サブエリアのTCメインフッターのDisable対応 END

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' FMメインボタンタップイベント初期化
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFMMainButtonEvent()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'CHTの場合、FMメインメニュータップすると、FMメイン画面に遷移する
        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.CHT Then

            'メインメニュー
            Dim footerFMMainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ForemanMain)

            'メインボタンのClientとサーバ両方のクリックイベントをbind
            AddHandler footerFMMainMenuButton.Click, AddressOf footerFMMainMenuButton_Click
            footerFMMainMenuButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, _
                                                                 FOOTER_REPLACE_EVENT, _
                                                                 CType(FooterMenuCategory.ForemanMain, String))
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
#End Region

#Region "フッター表示、ライト制御"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                                                        ByRef category As FooterMenuCategory) As Integer()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'SMBボタンを活性にする
        'category = FooterMenuCategory.SMB
        If StaffContext.Current.OpeCD = Operation.CT _
            OrElse StaffContext.Current.OpeCD = Operation.CHT Then
            category = FooterMenuCategory.MainMenu
        Else
            category = FooterMenuCategory.SMB
        End If
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return New Integer() {}

    End Function

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' SMBで不要なフッターボタンを非表示にする
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Sub HiddenFooterButton()

    '    Logger.Info(GetCurrentMethod.Name & " Start")

    '    ''顧客
    '    Dim footerCustomerButton As CommonMasterFooterButton = _
    '    CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer)
    '    footerCustomerButton.Visible = False

    '    'R/O
    '    Dim footerROButton As CommonMasterFooterButton = _
    '    CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.RO)
    '    footerROButton.Visible = False

    '    '追加作業
    '    Dim footerAddOperationButton As CommonMasterFooterButton = _
    '    CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.AddOperation)
    '    footerAddOperationButton.Visible = False

    '    'スケジューラ
    '    Dim footerScheduleButton As CommonMasterFooterButton = _
    '    CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Schedule)
    '    footerScheduleButton.Visible = False

    '    '電話帳
    '    Dim footerTelDirectoryButton As CommonMasterFooterButton = _
    '    CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TelDirectory)
    '    footerTelDirectoryButton.Visible = False

    '    'ユーザ情報の取得
    '    objStaffContext = StaffContext.Current

    '    'CT権限、またはFM権限の場合のみ完成検査ボタンを非表示にする
    '    If objStaffContext.OpeCD = iCROP.BizLogic.Operation.CT _
    '    OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.FM Then
    '        ''完成検査
    '        'Dim footerExaminationButton As CommonMasterFooterButton = _
    '        'CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Examination)
    '        'footerExaminationButton.Visible = False
    '    End If

    '    Logger.Info(GetCurrentMethod.Name & " End")

    'End Sub
#End Region

#Region "各ボタンタップイベント"
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' メインボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' ログイン権限がFMならFMメインメニュー、
    ''' ログイン権限がSAならSAメインメニュー、
    ''' ログイン権限がSMならSMメインメニュー、
    ''' に遷移する
    ''' </remarks>
    Private Sub footerMainMenuButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(GetCurrentMethod.Name & " Start. OpeCD=" & objStaffContext.OpeCD)

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''遷移先のプログラムID(初期値はCTMainのIDをとりあえず入れておく)
        'Dim nextScreenId As String = PGMID_CTMAIN

        '遷移先のプログラムID
        Dim nextScreenId As String = ""
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        'ログイン権限によって遷移するメインメニューを分岐する
        Select Case objStaffContext.OpeCD
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'Case iCROP.BizLogic.Operation.CT
            'CTメインメニューに遷移決定
            'nextScreenId = PGMID_CTMAIN
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            Case iCROP.BizLogic.Operation.FM
                'FMメインメニューに遷移決定
                nextScreenId = PGMID_FMMAIN

            Case iCROP.BizLogic.Operation.SA
                'SAメインメニューに遷移決定
                nextScreenId = PGMID_SAMAIN

            Case iCROP.BizLogic.Operation.SM
                'SMメインメニューに遷移決定
                nextScreenId = PGMID_SMMAIN

        End Select

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''決定した遷移先に遷移
        'Me.RedirectNextScreen(nextScreenId)

        If objStaffContext.OpeCD = iCROP.BizLogic.Operation.SA _
            OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.SM _
            OrElse objStaffContext.OpeCD = iCROP.BizLogic.Operation.FM Then
            '決定した遷移先に遷移
            Me.RedirectNextScreen(nextScreenId)
        End If
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(GetCurrentMethod.Name & " End")

    End Sub

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

    ''' <summary>
    ''' ROリストボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub footerRoListMenuButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If objStaffContext.OpeCD = Operation.CT _
            OrElse objStaffContext.OpeCD = Operation.FM _
            OrElse objStaffContext.OpeCD = Operation.SA _
            OrElse objStaffContext.OpeCD = Operation.SM _
            OrElse objStaffContext.OpeCD = Operation.CHT Then

            '基幹販売店コード、店舗コードを取得
            Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsBlnCd(objStaffContext.DlrCD, _
                                                                                         objStaffContext.BrnCD, _
                                                                                         objStaffContext.Account)
            If IsNothing(dmsDlrBrnRow) _
                OrElse dmsDlrBrnRow.IsCODE1Null _
                OrElse dmsDlrBrnRow.IsCODE2Null Then
                Throw New ArgumentException("Error: Failed to convert key dealer code.")
                Return
            End If

            'セション値の設定
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, dmsDlrBrnRow.CODE1)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, dmsDlrBrnRow.CODE2)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, dmsDlrBrnRow.ACCOUNT)

            '来店実績連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, "")

            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, "")

            'RO番号
            Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, "")

            'RO作業連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, "")

            '車両登録NOのVIN
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, "")

            'RO作成フラグ
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_VIEWMODE_EDIT)

            '画面番号(RO一覧)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_DISPNUM_ROPREVIEW)

            '決定した遷移先に遷移
            Me.RedirectNextScreen(OTHER_LINKAGE_PAGE)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 追加作業一覧ボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub footerAddWorkListMenuButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If objStaffContext.OpeCD = Operation.CT _
            OrElse objStaffContext.OpeCD = Operation.FM _
            OrElse objStaffContext.OpeCD = Operation.CHT Then

            '基幹販売店コード、店舗コードを取得
            Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsBlnCd(objStaffContext.DlrCD, _
                                                                                            objStaffContext.BrnCD, _
                                                                                            objStaffContext.Account)
            If IsNothing(dmsDlrBrnRow) _
                OrElse dmsDlrBrnRow.IsCODE1Null _
                OrElse dmsDlrBrnRow.IsCODE2Null Then
                Throw New ArgumentException("Error: Failed to convert key dealer code.")
                Return
            End If

            'セション値の設定
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, dmsDlrBrnRow.CODE1)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, dmsDlrBrnRow.CODE2)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, dmsDlrBrnRow.ACCOUNT)

            '来店実績連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, "")

            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, "")

            'RO番号
            Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, "")

            'RO作業連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, "")

            '車両登録NOのVIN
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, "")

            'RO作成フラグ
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_VIEWMODE_EDIT)

            '画面番号(追加作業一覧)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_DISPNUM_ADDWORKLIST)

            '決定した遷移先に遷移
            Me.RedirectNextScreen(OTHER_LINKAGE_PAGE)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 商品訴求コンテンツボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub footerGoodsSolicitationContentsMenuButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If objStaffContext.OpeCD = Operation.SA _
            OrElse objStaffContext.OpeCD = Operation.SM Then

            'セション値の設定
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_DEALERCODE, ONE_SPACE)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_BRANCHCODE, ONE_SPACE)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_LOGINUSERID, ONE_SPACE)

            '来店実績連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_SACHIPID, "")

            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_BASREZID, "")

            'RO番号
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_RO, "")

            'RO作業連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_SEQ_NO, "")

            '車両登録NOのVIN
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_VIN_NO, "")

            'RO作成フラグ
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_VIEWMODE, SESSIONVALUE_VIEWMODE_PREVIEW)

            '商品訴求コンテンツ画面に遷移
            Me.RedirectNextScreen(PGMID_GOOD_SOLICITATION_CONTENTS)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' キャンペーンボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub footerCampaignMenuButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If objStaffContext.OpeCD = Operation.SA _
            OrElse objStaffContext.OpeCD = Operation.SM Then

            '基幹販売店コード、店舗コードを取得
            Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsBlnCd(objStaffContext.DlrCD, _
                                                                                         objStaffContext.BrnCD, _
                                                                                         objStaffContext.Account)
            If IsNothing(dmsDlrBrnRow) _
                OrElse dmsDlrBrnRow.IsCODE1Null _
                OrElse dmsDlrBrnRow.IsCODE2Null Then
                Throw New ArgumentException("Error: Failed to convert key dealer code.")
                Return
            End If

            'セション値の設定
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, dmsDlrBrnRow.CODE1)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, dmsDlrBrnRow.CODE2)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, dmsDlrBrnRow.ACCOUNT)

            '来店実績連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, "")

            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, "")

            'RO番号
            Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, "")

            'RO作業連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, "")

            '車両登録NOのVIN
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, "")

            'RO作成フラグ
            '2014/07/01 TMEJ 丁　 TMT_UAT対応 START
            'Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_VIEWMODE_EDIT)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_VIEWMODE_PREVIEW)
            '2014/07/01 TMEJ 丁　 TMT_UAT対応 END

            '画面番号(RO一覧)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_DISPNUM_CAMPAIGN)

            '決定した遷移先に遷移
            Me.RedirectNextScreen(OTHER_LINKAGE_PAGE)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 来店管理ボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub footerVisitManagerMenuButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If objStaffContext.OpeCD = Operation.SA _
            OrElse objStaffContext.OpeCD = Operation.SM Then
            '決定した遷移先に遷移
            Me.RedirectNextScreen(PGMID_VISIT_MANAGER)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' FMメインボタンタップイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub footerFMMainMenuButton_Click(ByVal sender As Object, ByVal e As CommonMasterFooterButtonClickEventArgs)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If objStaffContext.OpeCD = Operation.CHT Then
            '決定した遷移先に遷移
            Me.RedirectNextScreen(PGMID_FMMAIN)
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

#End Region

#Region "画面遷移"
    ''' <summary>
    ''' 完成検査承認、追加作業承認画面の遷移
    ''' </summary>
    ''' <param name="inArgument">クライアントからの引数</param>
    ''' <remarks></remarks>
    Private Sub Redirect(ByVal inArgument As PostBackParamClass)
        Logger.Info(GetCurrentMethod.Name & " Start")
        '基幹販売店コード、店舗コードを取得
        Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsBlnCd(objStaffContext.DlrCD, _
                                                                                     objStaffContext.BrnCD, _
                                                                                     objStaffContext.Account)
        If IsNothing(dmsDlrBrnRow) _
            OrElse dmsDlrBrnRow.IsCODE1Null _
            OrElse dmsDlrBrnRow.IsCODE2Null Then
            Throw New ArgumentException("Error: Failed to convert key dealer code.")
            Return
        End If

        'セション値の設定
        If inArgument.OperationCode = COMPLETIONINSPECTION_REDIRECT Then
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_DEALERCODE, ONE_SPACE)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_BRANCHCODE, ONE_SPACE)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_LOGINUSERID, ONE_SPACE)

            '来店実績連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_SACHIPID, inArgument.VisitId)

            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_BASREZID, inArgument.DmsJobDtlId)

            'RO番号
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_RO, inArgument.OrderNo)

            'RO作業連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_SEQ_NO, "0")

            '車両登録NOのVIN
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_VIN_NO, inArgument.Vin)

            'RO作成フラグ
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_VIEWMODE, SESSIONVALUE_VIEWMODE_EDIT)

            '2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 START
            '予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_TONEC_JOBDTLID, inArgument.JobDtlId)
            '2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 END

            '遷移処理
            Me.RedirectNextScreen(PGMID_INSPEC)

        ElseIf inArgument.OperationCode = ADDWORKCONFIRM_REDIRECT Then
            'DMS用販売店コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DEARLER_CODE, dmsDlrBrnRow.CODE1)

            'DMS用店舗コード
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BRANCH_CODE, dmsDlrBrnRow.CODE2)

            'ログインユーザアカウント
            Me.SetValue(ScreenPos.Next, SESSIONKEY_LOGIN_USER_ID, dmsDlrBrnRow.ACCOUNT)

            '来店実績連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SA_CHIP_ID, inArgument.VisitId)

            'DMS予約ID
            Me.SetValue(ScreenPos.Next, SESSIONKEY_BASREZID, inArgument.DmsJobDtlId)

            'RO番号
            Me.SetValue(ScreenPos.Next, SESSIONKEY_R_O, inArgument.OrderNo)

            'RO作業連番
            Me.SetValue(ScreenPos.Next, SESSIONKEY_SEQ_NO, inArgument.RoJobSeq)

            '車両登録NOのVIN
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIN_NO, inArgument.Vin)

            'VIEW_MODE
            Me.SetValue(ScreenPos.Next, SESSIONKEY_VIEW_MODE, SESSIONVALUE_VIEWMODE_EDIT)

            '画面番号(追加作業承認)
            Me.SetValue(ScreenPos.Next, SESSIONKEY_DISP_NUM, SESSIONVALUE_DISPNUM_ADDWORKCONFIRM)

            '他システム連携画面（SC3010501）に遷移
            Me.RedirectNextScreen(OTHER_LINKAGE_PAGE)
        End If

        Logger.Info(GetCurrentMethod.Name & " End")
    End Sub

    ' '2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
    ''' <summary>
    ''' 走行距離履歴画面へ遷移
    ''' </summary>
    ''' <param name="inArgument">クライアントからの引数</param>
    ''' <remarks></remarks>
    Private Sub MileageRedirect(ByVal inArgument As PostBackParamClass)
        Logger.Info(GetCurrentMethod.Name & " Start")
        'セション値の設定

        '車両ID
        Me.SetValue(ScreenPos.Next, SESSIONKEY_VCL_ID, inArgument.VclId)

        '走行距離履歴画面画面（SC3240601）に遷移
        Me.RedirectNextScreen(MILEAGE_HISTORY_PAGE)

        Logger.Info(GetCurrentMethod.Name & " End")
    End Sub
    '2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END

#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
#Region "DMS販売店コード、店舗コードの取得する"

    ''' <summary>
    ''' 基幹販売店、基幹店舗コードを取得する
    ''' </summary>
    ''' <param name="dealerCode">i-CROP販売店コード</param>
    ''' <param name="branchCode">i-CROP店舗コード</param>
    ''' <returns>中断情報テーブル</returns>
    ''' <remarks></remarks>
    Private Function GetDmsBlnCd(ByVal dealerCode As String, _
                                 ByVal branchCode As String, _
                                 ByVal account As String) As ServiceCommonClassDataSet.DmsCodeMapRow

        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

        Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
            '基幹販売店コード、店舗コードを取得
            dmsDlrBrnTable = serviceCommonBiz.GetIcropToDmsCode(dealerCode, _
                                                                ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                                                dealerCode, _
                                                                branchCode, _
                                                                String.Empty, _
                                                                account)
            If dmsDlrBrnTable.Count <= 0 Then
                'データが取得できない場合はエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode: Failed to convert key dealer code.(No data found)", _
                                           MethodBase.GetCurrentMethod.Name))
                Return Nothing
            ElseIf 1 < dmsDlrBrnTable.Count Then
                'データが2件以上取得できた場合は一意に決定できないためエラー
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error ErrCode:Failed to convert key dealer code.(Non-unique)", _
                                           MethodBase.GetCurrentMethod.Name))
                Return Nothing
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E ", _
                                  MethodBase.GetCurrentMethod.Name))

        Return dmsDlrBrnTable.Item(0)

    End Function

#End Region
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#Region " セッション取得・設定バイパス処理 "

    Public Function ContainsKeyBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) As Boolean Implements ICommonSessionControl.ContainsKeyBypass
        Return Me.ContainsKey(pos, key)
    End Function

    Public Sub SetValueCommonBypass(ByVal pos As ScreenPos, ByVal key As String, ByVal value As Object) Implements ICommonSessionControl.SetValueCommonBypass
        Me.SetValue(pos, key, value)
    End Sub

    Public Function GetValueCommonBypass(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object Implements ICommonSessionControl.GetValueCommonBypass
        Return Me.GetValue(pos, key, removeFlg)
    End Function

#End Region

#Region "Call Back"

#Region "コールバック用内部クラス"
    ''' <summary>
    ''' コールバック用引数の内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackArgumentClass
        Public Property Method As String
        Public Property StallUseId As Decimal
        Public Property JobDtlId As Decimal
        Public Property SvcInId As Decimal
        Public Property SvcInIdLst As String
        Public Property StallId As Decimal
        Public Property StallIdleId As Decimal
        Public Property ShowDate As Date
        Public Property DisplayStartDate As Date
        Public Property DisplayEndDate As Date
        Public Property RsltServiceInDate As Date
        Public Property RsltStartDateTime As Date
        Public Property RsltEndDateTime As Date
        Public Property RowLockVersion As Long
        Public Property RestFlg As String
        Public Property ScheWorkTime As Long
        Public Property StopReasonType As String
        Public Property StopTime As Long
        Public Property StopMemo As String
        Public Property InspectionNeedFlg As String
        Public Property PickDeliType As String
        Public Property ScheSvcinDateTime As Date
        Public Property ScheDeliDateTime As Date
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        Public Property AddTechnicianAccount As String
        Public Property DeleteTechnicianAccount As String
        Public Property AddStaffRowLockVersion As String
        Public Property DeleteStaffRowLockVersion As String
        Public Property PreRefreshDateTime As Date    '前回リフレッシュ時間 
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        Public Property ReStartJobFlg As Long
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
    End Class

    ''' <summary>
    ''' コールバック結果をクライアントに返すための内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackResultClass
        Public Property MessageId As Long
        Public Property Message As String
        Public Property Contents As String
        Public Property ChipInfo As String
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        Public Property WorkingChipHisInfo As String
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        Public Property KariKariChipInfo As String
        Public Property RelationChipInfo As String
        Public Property TechnicianInfo As String
        Public Property StallIdleInfo As String
        Public Property NewStallIdleId As String
        Public Property Method As String
        Public Property Caller As String
        Public Property ResultCode As Long
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        Public Property StallUseId As Decimal
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
    End Class
#End Region

    ''' <summary>
    ''' コールバックイベント時のハンドリング
    ''' </summary>
    ''' <param name="eventArgument">クライアントから渡されるJSON形式のパラメータ</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' 2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理
    ''' </history>
    Public Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent
        Logger.Info(GetCurrentMethod.Name & " Start")

        Dim serializer = New JavaScriptSerializer
        'コールバック返却用内部クラスのインスタンスを生成
        Dim result As New CallBackResultClass

        'コールバック引数用内部クラスのインスタンスを生成し、JSON形式の引数を内部クラス型に変換して受け取る
        Dim argument As New CallBackArgumentClass
        Dim rtValue As Long = 0
        Dim dtStartDate As Date
        Dim dtEndDate As Date
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        Dim isFirstStartchipFlg As Boolean = False
        Dim dtNow As Date = DateTimeFunc.Now(objStaffContext.DlrCD)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
        Dim chipInstructFlg As Boolean = False
        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

        Try
            'clientからのパラを取得
            argument = serializer.Deserialize(Of CallBackArgumentClass)(eventArgument)
            'argument.ShowDateがあれば、営業時間を計算する
            If argument.ShowDate <> Date.MinValue Then
                dtStartDate = argument.ShowDate
                dtStartDate = dtStartDate.AddHours(m_tStallStartTime.Hours).AddMinutes(m_tStallStartTime.Minutes)

                dtEndDate = argument.ShowDate
                dtEndDate = dtEndDate.AddHours(m_tStallEndTime.Hours).AddMinutes(m_tStallEndTime.Minutes)
            End If
            'データテーブル(チップ)
            Dim strChipJson As String = ""

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim dtNow As Date = DateTimeFunc.Now(objStaffContext.DlrCD)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            Dim restFlg = String.Empty

            Using biz As New TabletSMBCommonClassBusinessLogic

                If biz.IsRestAutoJudge() Then
                    '休憩を自動判定する場合
                    restFlg = RestTimeGetFlgGetRest
                Else
                    '自動判定しない場合
                    restFlg = argument.RestFlg
                End If

            End Using
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            '呼ばられる関数
            Select Case argument.Method
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                'Case "ShowMainArea", "ReShowMainArea"
                Case "ShowMainArea", "ReShowMainArea", "ReShowMainAreaFromTheTime"
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

                    ''ストールIDリストを取得
                    ''ストールを取得
                    'Dim dtStallIdInfo As SC3240101DataSet.SC3240101AllStallDataTable = _
                    '    businessLogic.GetAllStall(objStaffContext)

                    'Dim stallIdList As New List(Of Decimal)

                    'For Each drStallInfo As SC3240101DataSet.SC3240101AllStallRow In dtStallIdInfo
                    '    stallIdList.Add(drStallInfo.STALLID)
                    'Next

                    ''当画面の日付のチップ、関連チップの情報を取得
                    ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    ''Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
                    ''                Me.GetAllStallChipInfo(stallIdList, dtStartDate, dtEndDate)
                    'Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable
                    ''前回リフレッシュ時間があれば、前回のリフレッシュ時間から変更があるチップ情報を取得する
                    'If argument.Method.Equals("ReShowMainAreaFromTheTime") Then
                    '    dtChipInfo = Me.GetAllStallChipInfo(stallIdList, dtStartDate, dtEndDate, argument.PreRefreshDateTime)
                    'Else
                    '    dtChipInfo = Me.GetAllStallChipInfo(stallIdList, dtStartDate, dtEndDate)
                    'End If
                    ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                    ''当ページのチップのサービス入庫IDリストを取得
                    'Dim svcinIdList As List(Of Decimal) = Me.GetSvcIdListFromStallChipTable(dtChipInfo)

                    ''仮仮チップ情報を取得
                    'Dim dtKariKariChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassKariKariChipInfoDataTable = _
                    '                Me.GetAllStallKariKariChipInfo(stallIdList, dtStartDate, dtEndDate)

                    ''jsonストリングでクライアント側に転送する
                    ''ストール上のチップ情報
                    'result.ChipInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtChipInfo))
                    ''関連チップ情報
                    'result.RelationChipInfo = Me.GetRelationChipInfo(svcinIdList)
                    ''仮仮チップ情報
                    'result.KariKariChipInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtKariKariChipInfo))
                    ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    ''作業中チップの履歴情報
                    'result.WorkingChipHisInfo = Me.GetChipHis(dtChipInfo)

                    ''テクニシャン情報
                    'Dim dtStallInfo As SC3240101DataSet.SC3240101StallStaffDataTable = _
                    '     businessLogic.GetAllStallStaff(objStaffContext)

                    ' ''テクニシャン情報
                    ''Dim dtStallInfo As SC3240101DataSet.SC3240101StallStaffDataTable = _
                    ''     businessLogic.GetAllStallStaff(DateTimeFunc.FormatDate(9, dtStartDate))
                    ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                    'result.TechnicianInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtStallInfo))

                    ''ストール非稼働時間情報
                    'Dim dtStallIdleInfo As DataTable = businessLogic.GetAllIdleDateInfo(stallIdList, dtStartDate, dtEndDate)
                    'result.StallIdleInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtStallIdleInfo))

                    '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 START

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} ②SC3240101_初期表示、定期リフレッシュ、自動リフレッシュ処理 START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 END

                    'メインページ表示処理実施
                    result = Me.ExecuteMethodShowMainArea(result, argument, dtStartDate, dtEndDate)

                    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    '部品ステータス情報取得結果コードを設定する
                    'rtValue = 0
                    rtValue = Me.CheckErrorCode()
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                    '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 START

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} ②SC3240101_初期表示、定期リフレッシュ、自動リフレッシュ処理 END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

                    '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 END

                    '2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善） START
                    'Case "GetPLanLaterTime"
                Case "GetPLanLaterTime", "GetPLanLaterTimeForFirstDisplay"
                    '2015/10/02 TM 小牟禮 サブエリアの情報取得処理を2回から1回に変更（性能改善） END
                    '遅れ見込み計算したチップ情報を取得
                    Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = _
                            businessLogic.GetPLanLaterTime(objStaffContext.DlrCD, objStaffContext.BrnCD, dtNow, argument.SvcInIdLst)
                    result.Contents = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtChipInfo))
                    rtValue = 0
                Case "ClickBtnStopJob"
                    '中断ボタンを押す
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    ''rtValue = businessLogic.StallChipJobStop(argument.SvcInId, argument.StallUseId, argument.StallId, argument.RsltStartDateTime, argument.RsltEndDateTime, argument.StopTime, argument.StopMemo, argument.StopReasonType, argument.RestFlg, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion)
                    'rtValue = businessLogic.StallChipJobStop(argument.StallUseId, argument.RsltEndDateTime, argument.StopTime, argument.StopMemo, argument.StopReasonType, argument.RestFlg, dtNow, argument.RowLockVersion)
                    rtValue = businessLogic.StallChipJobStop(argument.StallUseId, argument.RsltEndDateTime, argument.StopTime, argument.StopMemo, argument.StopReasonType, restFlg, dtNow, argument.RowLockVersion)
                    ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                    '中断して、生成された非稼動エリアチップID
                    result.NewStallIdleId = CType(businessLogic.NewStallIdleId, String)

                    '中断したチップがある行の非稼働チップを全部取得する
                    Dim stallIdList As New List(Of Decimal)
                    stallIdList.Add(argument.StallId)
                    'ストール非稼働時間情報
                    Dim dtStallIdleInfo As DataTable = businessLogic.GetAllIdleDateInfo(stallIdList, dtStartDate, dtEndDate)
                    result.StallIdleInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtStallIdleInfo))

                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                Case "ClickBtnStart"

                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

                    '中断Job再開フラグ(再開で初期化)
                    Dim reStartJobFlg As Boolean = True

                    '返信用のストール利用を設定する
                    result.StallUseId = argument.StallUseId

                    'ReStartJobFlg(0：設定してない　1：中断Job再開確認ボックスにOKボタンを押す 2：キャンセルボタンを押す)
                    'ReStartJobFlgが0の場合、中断Job含むチェックまだしてなかった
                    If RESTARTJOB_NOTSET = argument.ReStartJobFlg Then

                        '中断Job含むかどうかをチェックする
                        If businessLogic.HasStopJob(argument.JobDtlId) Then

                            '確認メッセージボックスを出すタイプに設定
                            rtValue = SHOW_CONFIRM_MSGBOX

                            '確認ボックスのメッセージ内容を設定
                            result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(PGMID_SMB, 931))
                            Exit Select

                        End If

                    ElseIf RESTARTJOB_NO = argument.ReStartJobFlg Then
                        '再開しないの場合

                        '中断Job再開フラグにFalseを設定する
                        reStartJobFlg = False

                    End If
                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                    '最初開始チップフラグ
                    isFirstStartchipFlg = Me.IsFirstStartChip(argument.SvcInId)

                    '作業開始ボタンを押す
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                    ''rtValue = businessLogic.StallChipStart(argument.StallUseId, argument.RsltStartDateTime, argument.RestFlg, dtNow, argument.RowLockVersion)
                    'rtValue = businessLogic.StallChipStart(argument.StallUseId, _
                    '                                       argument.RsltStartDateTime, _
                    '                                       argument.RestFlg, _
                    '                                       dtNow, _
                    '                                       argument.RowLockVersion, _
                    '                                       reStartJobFlg)
                    ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                    rtValue = businessLogic.StallChipStart(argument.StallUseId, _
                                                           argument.RsltStartDateTime, _
                                                           restFlg, _
                                                           dtNow, _
                                                           argument.RowLockVersion, _
                                                           reStartJobFlg)
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
                    ''開始したチップの履歴情報を取得する
                    'Dim stallUseIdList As New List(Of Decimal)
                    'stallUseIdList.Add(argument.StallUseId)
                    'Dim dtChipHisInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassChipHisDataTable = _
                    '                Me.GetWorkingChipHisInfo(stallUseIdList)
                    'result.WorkingChipHisInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtChipHisInfo))
                    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

                Case "GetTechnicians"
                    Dim dtStallInfo As SC3240101DataSet.SC3240101StallStaffDataTable = businessLogic.GetAllTechnicianByBrn(objStaffContext)
                    '該販売店の全てテクニシャンを取得する
                    result.TechnicianInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtStallInfo))
                Case "SetStallTechnicians"
                    rtValue = businessLogic.UpdateStallStaff(argument.AddTechnicianAccount, argument.DeleteTechnicianAccount, argument.AddStaffRowLockVersion, argument.DeleteStaffRowLockVersion, argument.StallId, dtNow, objStaffContext)
                    If rtValue = 0 Then
                        '該販売店の全てテクニシャンを取得する
                        Dim dtStallInfo As SC3240101DataSet.SC3240101StallStaffDataTable = businessLogic.GetAllTechnicianByBrn(objStaffContext)
                        result.TechnicianInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtStallInfo))
                    End If
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                Case "HasBeforeStartJob"
                    '未開始Jobあるかどうか
                    Dim bHasBeforeStartJob As Boolean = businessLogic.HasBeforeStartJob(argument.JobDtlId)

                    If bHasBeforeStartJob Then
                        '未開始Jobある場合、戻り値が1 
                        result.ResultCode = 1
                    Else
                        '未開始Jobない場合、戻り値が0
                        result.ResultCode = 0
                    End If

                    'イベント名前は引数の名前
                    result.Method = argument.Method

                    '処理結果をコールバック返却用文字列に設定
                    Me.callBackResult = serializer.Serialize(result)

                    Logger.Info(GetCurrentMethod.Name & " End")

                    Return
                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                Case "GetCanRestChange"
                    '営業開始終了はdtStartDate, dtEndDateに持ってる
                    Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
                        '休憩変更が可能かどうか
                        Dim bCanRestChangeFlg = clsTabletSMBCommonClass.CanRestChange(argument.StallUseId, dtStartDate, dtEndDate)

                        If bCanRestChangeFlg Then
                            '休憩変更可能な場合、戻り値が1
                            result.ResultCode = 1
                        Else
                            '休憩変更不可な場合、戻り値が0
                            result.ResultCode = 0
                        End If
                    End Using

                    'イベント名前は引数の名前
                    result.Method = argument.Method

                    '処理結果をコールバック返却用文字列に設定
                    Me.callBackResult = serializer.Serialize(result)

                    Logger.Info(GetCurrentMethod.Name & " End")

                    Return

                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                Case Else
                    '他の操作
                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                    'rtValue = Me.ExecuteMethod(argument, dtStartDate, dtEndDate, dtNow)
                    rtValue = Me.ExecuteMethod(argument, dtStartDate, dtEndDate, dtNow, chipInstructFlg)
                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
            End Select

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'result.ResultCode = rtValue
            'result.Method = argument.Method

            '成功の場合、各操作の通知を出す
            If rtValue = 0 OrElse rtValue = ActionResult.WarningOmitDmsError Then
                '「0：成功」「-9000：DMS除外エラーの警告」の場合

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                'If rtValue = 0 Then
                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
                'Me.SendNotice(argument, isFirstStartchipFlg)
                '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

                '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
                '必要な情報を取得する
                'result = Me.GetInfoAfterOperation(argument, dtNow, dtStartDate, dtEndDate, rtValue, result)
                result = Me.GetInfoAfterOperation(argument, _
                                                  dtNow, _
                                                  dtStartDate, _
                                                  dtEndDate, _
                                                  rtValue, _
                                                  result)
                '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

                '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
                If argument.Method = "CreateRelationChip" Then
                    'Relation Copyの場合

                    '新規されたチップのストール利用IDを取得する
                    argument.StallUseId = _
                        businessLogic.GetMaxStallUseIdGroupByServiceId(argument.SvcInId, _
                                                                       objStaffContext.DlrCD, _
                                                                       objStaffContext.BrnCD)

                End If


                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                '通知を出す
                'Me.SendNotice(argument, isFirstStartchipFlg)
                '通知を出す
                Me.SendNotice(argument, isFirstStartchipFlg, chipInstructFlg)
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
            ElseIf SHOW_CONFIRM_MSGBOX = rtValue Then

                'ResultCodeがSuccess
                result.ResultCode = 0

                'イベント名前は引数の名前
                result.Method = argument.Method

                'エラータイプ取得
                result.MessageId = GetResultType(rtValue)
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

            Else
                'update操作をした後のチップ情報を取得する
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                'If Not argument.Method.Equals("ShowMainArea") _
                '   And Not argument.Method.Equals("ReShowMainArea") _
                '   And Not argument.Method.Equals("GetPLanLaterTime") _
                '   And Not argument.Method.Equals("UpdateStallUnavailable") _
                '   And Not argument.Method.Equals("ClickBtnDeleteStallUnavailable") Then
                ''サービス入庫IDを取得する
                'Dim svcinIdList As New List(Of Decimal)
                'svcinIdList.Add(argument.SvcInId)
                ''サービス入庫IDの全てチップの情報を取得する
                'Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
                '    businessLogic.GetStallChipBySvcinId(dtNow, objStaffContext, svcinIdList)
                'result.Contents = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtChipInfo))

                ''リレーションコピーの場合、または移動する場合、リレーションチップ情報を取得する
                ''移動:リレショーンチップ日跨ぎ移動した後、矢印の表示ため
                'If argument.Method.Equals("CreateRelationChip") _
                '    OrElse argument.Method.Equals("DisplayTimeAndStallId") Then
                '    'リレーションチップ情報をclientに渡す
                '    result.RelationChipInfo = Me.GetRelationChipInfo(svcinIdList)
                'End If
                'ElseIf argument.Method.Equals("UpdateStallUnavailable") Then
                '    '使用不可チップ移動、リサイズした後で、最新の情報を取得する
                '    Dim stallIdList As New List(Of Decimal)
                '    stallIdList.Add(argument.StallId)
                '    Dim dtStallIdleInfo As DataTable = businessLogic.GetAllIdleDateInfo(stallIdList, dtStartDate, dtEndDate)
                '    result.StallIdleInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtStallIdleInfo))
                'End If

                '必要な情報を取得する
                result = Me.GetInfoAfterOperation(argument, dtNow, dtStartDate, dtEndDate, rtValue, result)

                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                'エラー後の操作タイプを取得する(リフレッシュか、元に戻すか、エラーページに遷移するか)
                result.MessageId = GetResultType(rtValue)
                'エラーで警告文言を取得
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                'result.Message = HttpUtility.HtmlEncode(GetErrorMessage(rtValue))
                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
                'result.Message = HttpUtility.HtmlEncode(GetErrorMessage(argument, rtValue))
                result.Message = GetErrorMessage(argument, rtValue)
                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            End If

            '処理結果をコールバック返却用文字列に設定
            Me.callBackResult = serializer.Serialize(result)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        Catch ex As OracleExceptionEx When ex.Number = 1013
            '必要な情報を取得する
            result = Me.GetInfoAfterOperation(argument, dtNow, dtStartDate, dtEndDate, rtValue, result)

            '予期せぬエラー
            result.ResultCode = ActionResult.DBTimeOutError
            result.MessageId = ErrorType.TypeRefresh
            '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
            'result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(PGMID_SMB, 901))
            result.Message = WebWordUtility.GetWord(PGMID_SMB, 901)
            '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END
            '処理結果をコールバック返却用文字列に設定
            Me.callBackResult = serializer.Serialize(result)
            Logger.Error(GetCurrentMethod.Name & " Error:", ex)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        Catch ex As Exception

            '必要な情報を取得する
            result = Me.GetInfoAfterOperation(argument, dtNow, dtStartDate, dtEndDate, rtValue, result)

            '予期せぬエラー
            result.ResultCode = ActionResult.ExceptionError
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'result.MessageId = ErrorType.TypeUnexpected
            result.MessageId = ErrorType.TypeRefresh
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
            'result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(PGMID_SMB, 922))
            result.Message = WebWordUtility.GetWord(PGMID_SMB, 922)
            '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END
            '処理結果をコールバック返却用文字列に設定
            Me.callBackResult = serializer.Serialize(result)
            Logger.Error(GetCurrentMethod.Name & " Error:", ex)
        End Try
        Logger.Info(GetCurrentMethod.Name & " End")

    End Sub

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

    ''' <summary>
    ''' メインページ表示処理
    ''' </summary>
    ''' <param name="result">返却用内部クラスのインスタンス</param>
    ''' <param name="argument">クライアントからもらった引数</param>
    ''' <param name="dtStartDate">営業開始時間</param>
    ''' <param name="dtEndDate">営業終了時間</param>
    ''' <returns>処理結果情報</returns>
    ''' <remarks></remarks>
    Private Function ExecuteMethodShowMainArea(ByVal result As CallBackResultClass, _
                                               ByVal argument As CallBackArgumentClass, _
                                               ByVal dtStartDate As Date, _
                                               ByVal dtEndDate As Date) As CallBackResultClass
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ストールIDリストを取得
        'ストールを取得
        Dim dtStallIdInfo As SC3240101DataSet.SC3240101AllStallDataTable = _
            businessLogic.GetAllStall(objStaffContext)

        Dim stallIdList As New List(Of Decimal)

        For Each drStallInfo As SC3240101DataSet.SC3240101AllStallRow In dtStallIdInfo
            stallIdList.Add(drStallInfo.STALLID)
        Next

        '当画面の日付のチップ、関連チップの情報を取得
        Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable

        '前回リフレッシュ時間があれば、前回のリフレッシュ時間から変更があるチップ情報を取得する
        If argument.Method.Equals("ReShowMainAreaFromTheTime") Then
            dtChipInfo = Me.GetAllStallChipInfo(stallIdList, dtStartDate, dtEndDate, argument.PreRefreshDateTime)
        Else
            dtChipInfo = Me.GetAllStallChipInfo(stallIdList, dtStartDate, dtEndDate)
        End If

        '当ページのチップのサービス入庫IDリストを取得
        Dim svcinIdList As List(Of Decimal) = Me.GetSvcIdListFromStallChipTable(dtChipInfo)

        '仮仮チップ情報を取得
        Dim dtKariKariChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassKariKariChipInfoDataTable = _
                        Me.GetAllStallKariKariChipInfo(stallIdList, dtStartDate, dtEndDate)

        'jsonストリングでクライアント側に転送する
        'ストール上のチップ情報
        result.ChipInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtChipInfo))
        '関連チップ情報
        result.RelationChipInfo = Me.GetRelationChipInfo(svcinIdList)
        '仮仮チップ情報
        result.KariKariChipInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtKariKariChipInfo))
        '作業中チップの履歴情報
        '2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応 START
        'result.WorkingChipHisInfo = Me.GetChipHis(dtChipInfo)
        result.WorkingChipHisInfo = Me.GetChipHisFromStallTime(objStaffContext.DlrCD, objStaffContext.BrnCD, dtStartDate, dtEndDate)
        '2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応 END

        'テクニシャン情報
        Dim dtStallInfo As SC3240101DataSet.SC3240101StallStaffDataTable = _
             businessLogic.GetAllStallStaff(objStaffContext)

        ''テクニシャン情報
        result.TechnicianInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtStallInfo))

        'ストール非稼働時間情報
        Dim dtStallIdleInfo As DataTable = businessLogic.GetAllIdleDateInfo(stallIdList, dtStartDate, dtEndDate)
        result.StallIdleInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtStallIdleInfo))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return result
    End Function

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
    ' ''' <summary>
    ' ''' コールバックイベントを実行する
    ' ''' </summary>
    ' ''' <param name="argument">クライアントからもらった引数</param>
    ' ''' <param name="dtStartDate">営業開始日時</param>
    ' ''' <param name="dtEndDate">営業終了日時</param>
    ' ''' <param name="dtNow">現在日時</param>
    ' ''' <returns>実行結果</returns>
    ' ''' <remarks></remarks>
    'Private Function ExecuteMethod(ByVal argument As CallBackArgumentClass, _
    '                               ByVal dtStartDate As Date, _
    '                               ByVal dtEndDate As Date, _
    '                               ByVal dtNow As Date) As Long
    ''' <summary>
    ''' コールバックイベントを実行する
    ''' </summary>
    ''' <param name="argument">クライアントからもらった引数</param>
    ''' <param name="dtStartDate">営業開始日時</param>
    ''' <param name="dtEndDate">営業終了日時</param>
    ''' <param name="dtNow">現在日時</param>
    ''' <param name="chipInstructFlg">着工指示フラグ</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    Private Function ExecuteMethod(ByVal argument As CallBackArgumentClass, _
                                   ByVal dtStartDate As Date, _
                                   ByVal dtEndDate As Date, _
                                   ByVal dtNow As Date, _
                                   ByRef chipInstructFlg As Boolean) As Long
        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

        Dim rtValue As Long = 0

        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        Dim restFlg = String.Empty

        Using biz As New TabletSMBCommonClassBusinessLogic

            If biz.IsRestAutoJudge() Then
                '休憩を自動判定する場合
                restFlg = RestTimeGetFlgGetRest
            Else
                '自動判定しない場合
                restFlg = argument.RestFlg
            End If

        End Using
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        Select Case argument.Method
            Case "DisplayTimeAndStallId"
                'チップの開始時間、終了時間、ストールidを設定する
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'rtValue = businessLogic.StallChipMoveResize(argument.StallUseId, argument.StallId, argument.DisplayStartDate, argument.ScheWorkTime, argument.RestFlg, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion)
                rtValue = businessLogic.StallChipMoveResize(argument.StallUseId, argument.StallId, argument.DisplayStartDate, argument.ScheWorkTime, restFlg, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
            Case "ClickRezConFirmed"
                '予約確定ボタンを押す
                rtValue = businessLogic.StallChipConfirmRez(argument.SvcInId, argument.StallUseId, dtNow, objStaffContext, argument.RowLockVersion)
            Case "ClickCancelRezConFirmed"
                '予約確定取り消しボタンを押す
                rtValue = businessLogic.StallChipCancelConfirmRez(argument.SvcInId, argument.StallUseId, dtNow, objStaffContext, argument.RowLockVersion)
            Case "ClickBtnCarIn"
                '入庫ボタンを押す
                rtValue = businessLogic.StallChipCarIn(argument.SvcInId, argument.StallUseId, argument.RsltServiceInDate, dtNow, objStaffContext.Account, argument.RowLockVersion)
            Case "ClickBtnCancelCarIn"
                '入庫取消ボタンを押す
                rtValue = businessLogic.StallChipCancelCarIn(argument.SvcInId, argument.StallUseId, dtNow, objStaffContext.Account, argument.RowLockVersion)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                'Case "ClickBtnStart"
                '    '作業開始ボタンを押す
                '    rtValue = businessLogic.StallChipStart(argument.SvcInId, argument.StallUseId, argument.StallId, argument.RsltStartDateTime, argument.RestFlg, dtNow, dtStartDate, dtEndDate, objStaffContext, argument.RowLockVersion)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            Case "ClickBtnFinish"
                '作業終了ボタンを押す
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                ''rtValue = businessLogic.StallChipFinish(argument.SvcInId, argument.StallUseId, argument.StallId, argument.RsltStartDateTime, argument.RsltEndDateTime, argument.RestFlg, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion)
                'rtValue = businessLogic.StallChipFinish(argument.StallUseId, argument.RsltEndDateTime, argument.RestFlg, dtNow, argument.RowLockVersion)
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                rtValue = businessLogic.StallChipFinish(argument.StallUseId, argument.RsltEndDateTime, restFlg, dtNow, argument.RowLockVersion)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
            Case "ClickMidFinish"
                '日跨ぎ終了ボタンを押す
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'rtValue = businessLogic.StallChipMidFinish(argument.SvcInId, argument.StallUseId, argument.StallId, argument.RsltStartDateTime, argument.RsltEndDateTime, argument.RestFlg, dtNow, objStaffContext, dtStartDate, dtEndDate, argument.RowLockVersion)
                rtValue = businessLogic.StallChipMidFinish(argument.SvcInId, argument.StallUseId, argument.StallId, argument.RsltStartDateTime, argument.RsltEndDateTime, restFlg, dtNow, objStaffContext, dtStartDate, dtEndDate, argument.RowLockVersion)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
            Case "ClickBtnDeleteStallChip"
                '削除ボタンを押す(普通のチップが選択中)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                'rtValue = businessLogic.DeleteStallChip(argument.StallUseId, objStaffContext, argument.RowLockVersion)
                rtValue = businessLogic.DeleteStallChip(argument.StallUseId, argument.RowLockVersion)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            Case "ClickBtnDeleteStallUnavailable"
                '削除ボタンを押す(使用不可エリアが選択中)
                rtValue = businessLogic.DeleteStallUnavailable(argument.StallIdleId, dtNow, objStaffContext.Account, argument.RowLockVersion)
            Case "ClickBtnNoshow"
                'NOSHOWボタンを押す
                rtValue = businessLogic.StallChipNoShow(argument.StallUseId, dtNow, objStaffContext, argument.RowLockVersion)
            Case "UpdateStallUnavailable"
                '非稼働エリア移動、リサイズ処理
                rtValue = businessLogic.StallUnavailableChipMoveResize(argument.StallIdleId, argument.StallId, argument.DisplayStartDate, argument.ScheWorkTime, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion)
            Case "CreateRelationChip"
                'リレーションコピー
                'rtValue = businessLogic.RelationCopy(argument.StallUseId, argument.JobDtlId, argument.SvcInId, argument.StallId, argument.DisplayStartDate, argument.ScheWorkTime, argument.RestFlg, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion, argument.InspectionNeedFlg, argument.PickDeliType, argument.ScheDeliDateTime, argument.ScheSvcinDateTime)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                rtValue = businessLogic.RelationCopy(argument.StallUseId, argument.JobDtlId, argument.SvcInId, argument.StallId, argument.DisplayStartDate, argument.ScheWorkTime, restFlg, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion, argument.InspectionNeedFlg, argument.PickDeliType, argument.ScheDeliDateTime, argument.ScheSvcinDateTime)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            Case "UndoMainStallChip"
                'メインストールのチップのundo処理
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                'rtValue = businessLogic.UndoWorkingChip(argument.SvcInId, argument.StallUseId, dtNow, objStaffContext, argument.RowLockVersion)
                rtValue = businessLogic.UndoWorkingChip(argument.SvcInId, argument.StallUseId, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion)
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
            Case "ClickBtnToReception"
                '計画取り消しボタンを押す'
                rtValue = businessLogic.StallChipToReception(argument.SvcInId, argument.JobDtlId, argument.StallUseId, dtNow, objStaffContext.Account, PGMID_SMB, argument.RowLockVersion, chipInstructFlg)
                '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            Case "ClickBtnNoRest"
                '休憩なしボタンを押す
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                'rtValue = businessLogic.RestChange(argument.StallUseId, argument.StallId, argument.DisplayStartDate, argument.ScheWorkTime, RestTimeGetFlgNoGetRest, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion)
                rtValue = businessLogic.RestChange(argument.StallUseId, RestTimeGetFlgNoGetRest, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion)
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
            Case "ClickBtnRest"
                '休憩ありボタンを押す
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                'rtValue = businessLogic.RestChange(argument.StallUseId, argument.StallId, argument.DisplayStartDate, argument.ScheWorkTime, RestTimeGetFlgGetRest, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion)
                rtValue = businessLogic.RestChange(argument.StallUseId, RestTimeGetFlgGetRest, dtStartDate, dtEndDate, dtNow, objStaffContext, argument.RowLockVersion)
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        End Select

        Return rtValue
    End Function

    ''' <summary>
    ''' 操作結果により、エラータイプを取得
    ''' </summary>
    ''' <param name="inValue">操作結果</param>
    ''' <returns>エラータイプ</returns>
    ''' <remarks></remarks>
    Private Function GetResultType(ByVal inValue As Long) As Long
        Logger.Info(GetCurrentMethod.Name & " Start")

        'エラータイプ
        Dim rtValue As Long

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '予期せぬエラー
        'If inValue = ActionResult.NotStartDayError _
        '    Or inValue = ActionResult.CheckError _
        '    Or inValue = ActionResult.GetChipEntityError _
        '    Or inValue = ActionResult.ExceptionError Then
        '    rtValue = ErrorType.TypeUnexpected
        If inValue = ActionResult.IC3802503ResultDmsError _
            Or inValue = ActionResult.IC3802503ResultOtherError _
            Or inValue = ActionResult.IC3802503ResultTimeOutError Then

            rtValue = ErrorType.TypeShowErr
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ElseIf SHOW_CONFIRM_MSGBOX = inValue Then

            '確認ボックスを出すタイプに設定
            rtValue = ErrorType.TypeShowConfirmMsg

            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        Else

            'ほかのサーバ側のエラーは全部リフレッシュ
            rtValue = ErrorType.TypeRefresh

        End If

        Logger.Info(GetCurrentMethod.Name & " End")
        Return rtValue
    End Function

    ''' <summary>
    ''' 操作結果により、エラー文言を取得
    ''' </summary>
    ''' <param name="inValue">操作結果</param>
    ''' <returns>エラー文言</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Private Function GetErrorMessage(ByVal argument As CallBackArgumentClass, ByVal inValue As Long) As String
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'Private Function GetErrorMessage(ByVal inValue As Long) As String
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        Logger.Info(GetCurrentMethod.Name & " Start")

        Dim rtMessage As String = ""
        'エラーコードにより、エラータイプを分類する
        Select Case inValue
            Case ActionResult.NotSetroNoError
                'RO紐付いてないエラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 918)
            Case ActionResult.OutOfWorkingTimeError
                '営業時間を超えるエラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 903)
            Case ActionResult.NotSetJobSvcClassIdError
                '処理対象の作業内容.表示サービス分類コードが未設定値エラー()
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 904)
            Case ActionResult.HasWorkingChipInOneStallError
                '同一のストールに既に作業中のステータスが存在するエラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 905)
            Case ActionResult.OverlapError
                '重複エラー(普通チップ)
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 906)
            Case ActionResult.OverlapUnavailableError
                '重複エラー(使用不可チップ)
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 914)
            Case ActionResult.NoTechnicianError
                'テクニシャンないエラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 907)
            Case ActionResult.RowLockVersionError
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                'rtMessage = WebWordUtility.GetWord(PGMID_SMB, 908)
                If argument.Method.Equals("SetStallTechnicians") Then
                    rtMessage = WebWordUtility.GetWord(PGMID_SMB, 925)
                Else
                    '最新のデータではないエラー
                    rtMessage = WebWordUtility.GetWord(PGMID_SMB, 908)
                End If
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            Case ActionResult.LockStallError
                'ストールロックエラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 909)
            Case ActionResult.DBTimeOutError
                'DBと接続する時、TimeOutエラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 901)
            Case ActionResult.DmsLinkageError
                '基幹連携エラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 920)
            Case ActionResult.InspectionStatusFinishError
                '検査依頼中、終了不可エラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 919)
            Case ActionResult.InspectionStatusStopError
                '検査依頼中、中断不可エラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 923)
            Case ActionResult.ParentroNotStartedError
                '親R/Oが作業開始されていないエラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 921)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            Case ActionResult.OverMaxTechnicianNumsError
                '1つストールにテクニシャンが4名以上(最大人数)を超えるエラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 924)
            Case ActionResult.IC3802503ResultTimeOutError
                '部品ステータス情報取得タイムアウトエラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 926)
            Case ActionResult.IC3802503ResultDmsError
                '部品ステータス情報取得時、他システム側でエラー発生エラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 927)
            Case ActionResult.IC3802503ResultOtherError
                '部品ステータス情報取得時、エラーが発生
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 928)
            Case ActionResult.NoJobResultDataError
                '着工指示した作業終了時、実績データが持ってないエラーが発生
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 929)
            Case ActionResult.HasStartedRelationChipError
                'リレーションチップにもう開始してるエラーが発生
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 910)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START
                Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
                    Dim stallName As String = String.Empty
                    '作業中の関連チップのあるストール名称を取得する

                    stallName = serviceCommonBiz.GetStallNameWithRelationChip(CStr(argument.StallUseId), argument.StallId)

                    If Not String.IsNullOrEmpty(stallName) Then
                        rtMessage = String.Format(CultureInfo.CurrentCulture, _
                                           WebWordUtility.GetWord(PGMID_SMB, 937), _
                                           stallName)
                    End If
                End Using
                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END
                '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 START
            Case ActionResult.UnablePlanChipInWashingError
                '洗車中、チップ変更不可エラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 932)

            Case ActionResult.UnablePlanChipInInspectingError
                '検査中、チップ変更不可エラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 933)

            Case ActionResult.UnablePlanChipAfterDeliveriedError
                '納車済、チップ変更不可エラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 934)

                '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 END

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            Case ActionResult.WarningOmitDmsError
                'DMS除外エラーの警告
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 936)

                '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            Case ActionResult.ChipOverlapUnavailableError
                'ストール使用不可と重複する配置である場合のエラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 939)
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

                '2015/10/08 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 Start

            Case ActionResult.IC3800903ResultRangeLower To ActionResult.IC3800903ResultRangeUpper
                '予約送信IFエラー(エラーコードが8000番台)
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, inValue)

                '2015/10/08 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 End

            Case Else
                '予期せぬエラー
                rtMessage = WebWordUtility.GetWord(PGMID_SMB, 922)
        End Select
        Logger.Info(GetCurrentMethod.Name & " End")
        Return rtMessage
    End Function

    ''' <summary>
    ''' コールバック用文字列を返却
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult
        Return Me.callBackResult
    End Function

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 部品ステータスなど操作した時、エラーがあるかどうかをチェックする
    ''' </summary>
    ''' <returns>ActionResult</returns>
    ''' <remarks></remarks>
    Private Function CheckErrorCode() As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim retCode As Long = ActionResult.Success

        '部品ステータス情報取得結果コードがエラーの場合、エラーコードを戻す
        If businessLogic.IC3802503ResultValue <> ActionResult.Success Then
            retCode = businessLogic.IC3802503ResultValue
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E return={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  retCode))
        Return retCode
    End Function

    ''' <summary>
    ''' 最初開始チップフラグ
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <returns>True:最初開始チップ</returns>
    ''' <remarks></remarks>
    Private Function IsFirstStartChip(ByVal svcinId As Decimal) As Boolean

        Logger.Info(GetCurrentMethod.Name & " Start")

        Dim firstStartChip As Boolean
        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            firstStartChip = clsTabletSMBCommonClass.IsFirstStartChip(objStaffContext.DlrCD, _
                                                                      objStaffContext.BrnCD, _
                                                                      svcinId)
        End Using

        Logger.Info(GetCurrentMethod.Name & " End return " & firstStartChip)

        Return firstStartChip
    End Function

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
    ' ''' <summary>
    ' ''' 各操作の通知を出す
    ' ''' </summary>
    ' ''' <param name="argument">クライアントからもらった引数</param>
    ' ''' <param name="isFirstStartchipFlg">最初開始チップフラグ</param>
    ' ''' <remarks></remarks>
    'Private Sub SendNotice(ByVal argument As CallBackArgumentClass, _
    '                       ByVal isFirstStartchipFlg As Boolean)
    ''' <summary>
    ''' 各操作の通知を出す
    ''' </summary>
    ''' <param name="argument">クライアントからもらった引数</param>
    ''' <param name="isFirstStartchipFlg">最初開始チップフラグ</param>
    ''' <param name="chipInstructFlg">着工指示フラグ</param>
    ''' <remarks></remarks>
    Private Sub SendNotice(ByVal argument As CallBackArgumentClass, _
                           ByVal isFirstStartchipFlg As Boolean, _
                           ByVal chipInstructFlg As Boolean)
        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

        Logger.Info(GetCurrentMethod.Name & " Start")

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Select Case argument.Method
                Case "ClickBtnStart"
                    '開始通知を出す
                    clsTabletSMBCommonClass.SendNoticeByStart(objStaffContext, argument.SvcInId, argument.StallId, isFirstStartchipFlg)
                Case "ClickBtnFinish"

                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                    ''終了通知を出す
                    'clsTabletSMBCommonClass.SendNoticeByFinish(objStaffContext, argument.SvcInId, argument.StallId, PGMID_SMB)

                    If businessLogic.NeedPushAfterFinishJob Then

                        '終了通知を出す
                        clsTabletSMBCommonClass.SendNoticeByFinish(objStaffContext, argument.SvcInId, argument.StallId, PGMID_SMB)

                    ElseIf businessLogic.NeedPushAfterStopJob Then

                        '中断通知を出す
                        clsTabletSMBCommonClass.SendNoticeByJobStop(objStaffContext, argument.StallId)

                    End If
                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                Case "ClickMidFinish"
                    '日跨ぎ終了通知を出す
                    clsTabletSMBCommonClass.SendNoticeByMidFinish(objStaffContext, argument.StallId)
                Case "ClickBtnDeleteStallChip"
                    'ストールチップの削除通知を出す
                    If Not IsNothing(businessLogic.TabletSmbCommonCancelInstructedChipInfo) Then
                        clsTabletSMBCommonClass.TabletSmbCommonCancelInstructedChipInfo = _
                            CType(businessLogic.TabletSmbCommonCancelInstructedChipInfo, TabletSMBCommonClassDataSet.TabletSmbCommonClassCanceledJobInfoDataTable)
                    End If
                    clsTabletSMBCommonClass.SendNoticeByDeleteStallChip(argument.StallUseId, objStaffContext)
                Case "ClickBtnNoshow"
                    'Noshow通知を出す
                    clsTabletSMBCommonClass.SendNoticeByNoShow(objStaffContext)
                Case "ClickBtnStopJob"

                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                    ''中断通知を出す
                    'clsTabletSMBCommonClass.SendNoticeByJobStop(objStaffContext, argument.StallId)

                    '中断Pushが立てる場合
                    If businessLogic.NeedPushAfterStopJob Then

                        '中断通知を出す
                        clsTabletSMBCommonClass.SendNoticeByJobStop(objStaffContext, argument.StallId)

                    End If
                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                Case "UndoMainStallChip"
                    'Undo通知を出す
                    clsTabletSMBCommonClass.SendNoticeByUndoWorkingChip(objStaffContext, argument.StallId)

                    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
                Case "DisplayTimeAndStallId", "CreateRelationChip"

                    If 0 < argument.StallUseId Then
                        'ストール利用IDがあれば

                        '納車遅れとなる配置を行った場合、通知の処理
                        businessLogic.SendNoticeWhenSetChipToBeLated(argument.StallUseId, _
                                                                     DateTimeFunc.Now(objStaffContext.DlrCD), _
                                                                     objStaffContext)

                    End If
                    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                Case "ClickBtnToReception"
                    '計画取り消し通知を出す
                    clsTabletSMBCommonClass.SendNoticeByToReception(argument.StallUseId, argument.StallId, argument.JobDtlId, objStaffContext, chipInstructFlg)
                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

            End Select
        End Using

        Logger.Info(GetCurrentMethod.Name & " End")

    End Sub

    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
    ' ''' <summary>
    ' ''' 各操作後、必要が情報を取得する
    ' ''' </summary>
    ' ''' <param name="argument">クライアントからもらった引数</param>
    ' ''' <param name="updateDate">更新日時</param>
    ' ''' <param name="stallStartTime">営業開始日時</param>
    ' ''' <param name="stallEndTime">営業終了日時</param>
    ' ''' <param name="rtValue">実行結果</param>
    ' ''' <param name="result">返却用内部クラスのインスタンス</param>
    ' ''' <returns>返却用内部クラスのインスタンス</returns>
    ' ''' <remarks></remarks>
    'Private Function GetInfoAfterOperation(ByVal argument As CallBackArgumentClass, _
    '                                       ByVal updateDate As Date, _
    '                                       ByVal stallStartTime As Date, _
    '                                       ByVal stallEndTime As Date, _
    '                                       ByVal rtValue As Long, _
    '                                       ByVal result As CallBackResultClass) As CallBackResultClass

    ''' <summary>
    ''' 各操作後、必要が情報を取得する
    ''' </summary>
    ''' <param name="argument">クライアントからもらった引数</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="stallStartTime">営業開始日時</param>
    ''' <param name="stallEndTime">営業終了日時</param>
    ''' <param name="rtValue">実行結果</param>
    ''' <param name="result">返却用内部クラスのインスタンス</param>
    ''' <param name="dtChipInfo">チップ情報</param>
    ''' <returns>返却用内部クラスのインスタンス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Private Function GetInfoAfterOperation(ByVal argument As CallBackArgumentClass, _
                                           ByVal updateDate As Date, _
                                           ByVal stallStartTime As Date, _
                                           ByVal stallEndTime As Date, _
                                           ByVal rtValue As Long, _
                                           ByVal result As CallBackResultClass, _
                                           Optional ByVal dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = Nothing) As CallBackResultClass

        '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_Start. updateDate={1}, stallStartTime={2}, stallEndTime={3}, rtValue={4}" _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name, updateDate, stallStartTime, stallEndTime, rtValue))

        result.ResultCode = rtValue
        result.Method = argument.Method

        If argument.SvcInId <> 0 Then

            'サービス入庫IDを取得する
            Dim svcinIdList As New List(Of Decimal)
            '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
            'svcinIdList.Add(argument.SvcInId)
            ''サービス入庫IDの全てチップの情報を取得する
            'Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
            '    businessLogic.GetStallChipBySvcinId(updateDate, objStaffContext, svcinIdList)

            '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
            '各操作後、ストール上更新されたチップの情報を取得
            'Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
            '    businessLogic.GetStallChipAfterOperation(objStaffContext.DlrCD, _
            '                                             objStaffContext.BrnCD, _
            '                                             stallStartTime, _
            '                                             argument.PreRefreshDateTime)

            dtChipInfo = _
                businessLogic.GetStallChipAfterOperation(objStaffContext.DlrCD, _
                                                         objStaffContext.BrnCD, _
                                                         stallStartTime, _
                                                         argument.PreRefreshDateTime)
            '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END


            '全チップのサービス入庫IDを取得
            For Each drChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoRow In dtChipInfo

                If Not svcinIdList.Contains(drChipInfo.SVCIN_ID) Then

                    svcinIdList.Add(drChipInfo.SVCIN_ID)

                End If

            Next

            '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END
            result.Contents = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtChipInfo))

            '作業中チップの履歴情報を取得する
            result.WorkingChipHisInfo = Me.GetChipHis(dtChipInfo)
            'リレーションチップ情報をclientに渡す
            result.RelationChipInfo = Me.GetRelationChipInfo(svcinIdList)
            '2020/01/07 NSK 坂本 TR-SVT-TMT-20190118-001 Stop start tagのXMLログが過去日19000101を表示している START
            'If argument.Method.Equals("UndoMainStallChip") Then
            '    result.StallUseId = argument.StallUseId
            'End If

            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'If argument.Method.Equals("UndoMainStallChip") Then
            '    'UNDOの場合は操作チップのIDを保持する
            '    result.StallUseId = argument.StallUseId
            'End If
            If argument.Method.Equals("DisplayTimeAndStallId") Or argument.Method.Equals("UndoMainStallChip") _
                Or argument.Method.Equals("ClickBtnStart") _
                Or argument.Method.Equals("ClickBtnNoRest") Or argument.Method.Equals("ClickBtnRest") Then
                'チップ移動、UNDO、作業開始、休憩なし、休憩ありの場合は操作チップのIDを保持する
                result.StallUseId = argument.StallUseId
            End If
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            '2020/01/07 NSK 坂本 TR-SVT-TMT-20190118-001 Stop start tagのXMLログが過去日19000101を表示している END

        ElseIf argument.Method.Equals("UpdateStallUnavailable") Then
            '使用不可チップ移動、リサイズした後で、最新の情報を取得する
            Dim stallIdList As New List(Of Decimal)
            stallIdList.Add(argument.StallId)
            Dim dtStallIdleInfo As DataTable = businessLogic.GetAllIdleDateInfo(stallIdList, stallStartTime, stallEndTime)
            result.StallIdleInfo = HttpUtility.HtmlEncode(businessLogic.DataTableToJson(dtStallIdleInfo))
        End If

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
        '処理結果チェック
        If rtValue = ActionResult.WarningOmitDmsError Then
            '「-9000：DMS除外エラーの警告」の場合
            '「-9000：DMS除外エラーの警告」のエラー内容を設定
            result.MessageId = ErrorType.TypeShowErr
            result.Message = HttpUtility.HtmlEncode(GetErrorMessage(argument, rtValue))

        End If

        '2015/05/14 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Logger.Info(GetCurrentMethod.Name & " End")
        Return result

    End Function
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#End Region

#Region "POSTBACK"
    ''' <summary>
    ''' ポストバック用引数の内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class PostBackParamClass
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        'RO番号
        Public Property OrderNo As String
        'RO枝番
        Public Property RoJobSeq As String
        'Public Property EditFlg As String = "0"
        'Public Property Rezid As Long
        '基幹予約ID
        Public Property DmsJobDtlId As String
        '訪問ID
        Public Property VisitId As String
        'VIN
        Public Property Vin As String
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 START
        '予約ID
        Public Property JobDtlId As String
        '2014/04/22 TMEJ 丁 【IT9669】サービスタブレットDMS連携作業追加機能開発 END


        Public Property OperationCode As String
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        Public Property StallId As String
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 START
        Public Property VclId As String
        '2014/06/18 TMEJ 丁 タブレット版SMB テレマ走行距離機能開発 END
    End Class
#End Region

End Class
