'------------------------------------------------------------------------------
'SC3180204.aspx.vb
'------------------------------------------------------------------------------
'機能： 完成検査入力画面
'補足： 
'作成： 2014/02/14 AZ宮澤	初版作成
'更新： 2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System.Data
Imports System.Globalization
Imports Toyota.eCRB.iCROP.DataAccess.SC3180204.SC3180204DataSet
Imports Toyota.eCRB.ServerCheck.CheckResult.BizLogic.SC3180204
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic

Partial Class Pages_Default
    Inherits BasePage
    Implements IDisposable
   
#Region "定数"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationId As String = "SC3180204"

    ''' <summary>
    ''' メインメニュー(SA)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdMinaMenuSa As String = "SC3140103"
    ''' <summary>
    ''' 全体管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdAllManagment As String = "SC3220201"
    ''' <summary>
    ''' 工程管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdProcessControl As String = "SC3240101"
    ''' <summary>
    ''' メインメニュー(TC)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdMainMenuTc As String = "SC3150101"
    ''' <summary>
    ''' メインメニュー(FM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdMainMenuFm As String = "SC3230101"

    ''' <summary>
    ''' SAメイン画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdSaMain As String = "SC3140103"
    ''' <summary>
    ''' TCメイン画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdTcMain As String = "SC3150101"
    ''' <summary>
    ''' SMB画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdSMB As String = "SC3240101"        '工程管理
    ''' <summary>
    ''' FMメイン画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdFmMain As String = "SC3230101"
    ''' <summary>
    ''' 顧客詳細画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdCustomerDetails As String = "SC3080201"
    ''' <summary>
    ''' 商品訴求コンテンツ画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdGoodsSolication As String = "SC3250101"

    ''' <summary>
    ''' 予約管理画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdOrderControl As String = "SC3100303"
    ''' <summary>
    ''' 通知履歴画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdNoticeHistory As String = "SC3180204"
    ''' <summary>
    ''' 他システム連携画面画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdOtherLinkage As String = "SC3010501"

    ''' <summary>
    ''' セッションキー(表示番号14：R/O一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDataDispNumRoList As Long = 14
    ''' <summary>
    ''' セッションキー(表示番号22：追加作業一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDataDispNumAddList As Long = 22
    ''' <summary>
    ''' セッションキー(表示番号23：追加作業入力)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDataDispNumAddInput As Long = 23
    ''' <summary>
    ''' セッションキー(表示番号15：キャンペーン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDataDispNumCampaing As Long = 15

    ''' <summary>
    ''' セッションキー(表示番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionDispNum As String = "Session.DISP_NUM"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター1)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionParam01 As String = "Session.Param1"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionParam02 As String = "Session.Param2"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター3)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionParam03 As String = "Session.Param3"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター4)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionParam04 As String = "Session.Param4"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター5)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionParam05 As String = "Session.Param5"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター6)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionParam06 As String = "Session.Param6"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター7)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionParam07 As String = "Session.Param7"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionParam08 As String = "Session.Param8"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター9)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionParam09 As String = "Session.Param9"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター10)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionParam10 As String = "Session.Param10"

    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(DMS販売店コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyGoodsContentsDearlerCode As String = "DealerCode"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(DMS店舗コード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyGoodsContentsBranch As String = "BranchCode"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(アカウント)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyGoodsContentsAcount As String = "LoginUserID"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(来店実績連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyGoodsContentsSAChipId As String = "SAChipID"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(DMS予約ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyGoodsContentsBaserzId As String = "BASREZID"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(RO番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyGoodsContentsRo As String = "R_O"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(RO作業連番)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyGoodsContentsSeqNo As String = "SEQ_NO"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(VIN)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyGoodsContentsVinNo As String = "VIN_NO"
    ''' <summary>
    ''' 商品訴求コンテンツ画面遷移セッションキー(編集モード)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyGoodsContentsViewMode As String = "ViewMode"

    ''' <summary>
    ''' SessionKey(R_O)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionRO As String = "R_O"
    ''' <summary>
    ''' SessionKey(VINNO)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionVINNO As String = "VIN_NO"
    ''' <summary>
    ''' SessionKey(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionViewMode As String = "ViewMode"
    ''' <summary>
    ''' SessionKey(JOB_DTL_ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionJobDtlId As String = "JOB_DTL_ID"
    ''' <summary>
    ''' SessionKey(Referer)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionReferer As String = "Referer"
    ''' <summary>
    ''' SessionKey(Redraw)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionRedraw As String = "Redraw"

    ''' <summary>
    ''' セッション名("SAChipID")
    ''' </summary>
    Private Const SessionSAChipID As String = "SAChipID"
    ''' <summary>
    ''' セッション名("BASREZID")
    ''' </summary>
    Private Const SessionBASREZID As String = "BASREZID"
    ''' <summary>
    ''' セッション名("SEQ_NO")
    ''' </summary>
    Private Const SessionSeqNo As String = "SEQ_NO"

    ''' <summary>
    ''' フッターコード：メインメニュー(SMB)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterMainMenu As Integer = 100
    ''' <summary>
    ''' フッターコード：TCメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterTecnicianMain As Integer = 200
    ''' <summary>
    ''' フッターコード：FMメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterFormanMain As Integer = 300
    ''' <summary>
    ''' フッターコード：来店管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterVisitManament As Integer = 400
    ''' <summary>
    ''' フッターコード：R/Oボタン(R/O一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterRo As Integer = 500
    ''' <summary>
    ''' フッターコード：連絡先
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterTelDirector As Integer = 600
    ''' <summary>
    ''' フッターコード：顧客詳細
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterCustomer As Integer = 700
    ''' <summary>
    ''' フッターコード：商品訴求コンテンツ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterContents As Integer = 800
    ''' <summary>
    ''' フッターコード：キャンペーン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterCampaing As Integer = 900
    ''' <summary>
    ''' フッターコード：SMB
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterSmb As Integer = 1100
    ''' <summary>
    ''' フッターコード：追加作業ボタン(追加作業一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterAddList As Integer = 1200

    ''' <summary>
    ''' フッターイベントの置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterReplaceEvent As String = "FooterButtonClick({0});"

    ''' <summary>
    ''' 電話帳ボタンのイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterEventTel As String = "return schedule.appExecute.executeCont();"

    ''' <summary>
    ''' 追加作業起票画面セッション：追加作業連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AdditionalWorkSendValue As String = "New"

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

    ''' <summary>
    ''' メッセージID管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MsgID
        id22 = 22
        id23 = 23
        id34 = 34
        id35 = 35
        id36 = 36
        id37 = 37
        id54 = 54
        '【***完成検査_排他制御***】 start
        idExclusion = 58 'TODO:メッセージIDが確定したら変更
        '【***完成検査_排他制御***】 end

        '2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応　Start 
        id59 = 59
        '2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応  End

    End Enum

    Private Enum HiddenDataNo
        InspecItemRegistMode = 0
        InspecItemMode = 1
        InspecItemsCheck = 2
        InspecItemTextInputMode = 3
        JobDtlID = 4
        JobInstructID = 5
        JobInstructSeq = 6
        InspecItemCD = 7
        StallUseID = 8
        TRN_RowLockVer = 9
        BAK_InspecItemsCheck = 10
        BAK_BeforeText = 11
        BAK_AfterText = 12
        BAK_InspecItemsSelector = 13
        SVC_CD = 14
        '【***完成検査_排他制御***】 start
        Edit_Flg = 15
        ServiceInLockVer = 16
        '【***完成検査_排他制御***】 end
    End Enum
    '2014/12/10 [JobDispatch完成検査入力制御開発]対応　Start
    ''' <summary>
    ''' Screen linkage View Mode：Editモード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ViewModeEdit As String = "0"

    ''' <summary>
    ''' Screen linkage View Mode：ReadOnlyモード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ViewModeReadOnly As String = "1"

    ''' <summary>
    ''' チップ単位で全JOB開始ステータス
    ''' （※JavaScriptのBoolean型でTrue判定される為の値）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IsAllJobStartTrue As Long = 1

    ''' <summary>
    ''' チップ単位で開始していないJOBが存在するステータス
    ''' （※JavaScriptのBoolean型でFalse判定される為の値）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IsAllJobStartFalse As Long = 0
    '2014/12/10 [JobDispatch完成検査入力制御開発]対応　End

#Region "規定値"

    Private Const ErrorFlgError As String = "1"                                 ' エラーフラグ

    Private Const AcceptanceTypeWalkin As String = "0"                          ' 受付区分（WalkIN）
    Private Const VipFlgTrue As String = "1"                                    ' VIP_FLG状態（あり）

    Private Const ConvertDateYMD As Long = 3                                    ' 日付フォーマット形態(YMD)
    Private Const ConvertDateMD As Long = 11                                    ' 日付フォーマット形態(MD)
    Private Const ConvertDateHM As Long = 14                                    ' 日付フォーマット形態(HM)

    Private Const VehicleChartNoEngine As String = "1"                          ' 選択位置（エンジンルーム）

    Private Const StallUseStatusWork As String = "02"                           ' ストール利用ステータス(作業中）
    Private Const StallUseStatusCompletion As String = "03"                     ' ストール利用ステータス(完了）

    Private Const OracleExNumberTimeoutError As Long = 1013                     ' Oracle例外（タイムアウト）

    Private Const AprovalStatusAproveWorking As Long = 0                        ' 承認ステータス（作業中）
    Private Const AprovelStatusNotAprove As Long = 2                            ' 承認ステータス（承認）
    Private Const AprovalStatusWaitingRecognition As Long = 1                   ' 承認ステータス（承認依頼）
    Private Const AprovalStatusEtc As Long = 3                                  ' 承認ステータス（その他）

    Private Const FromTCMainPage As String = "1"                                 ' TCMainからの遷移

    Private Const InspecItemModeNow As String = "1"                             ' 表示モード（現在）
    Private Const InspecItemModeFuture As String = "0"                          ' 表示モード（未来）
    Private Const InspecItemModePast As String = "2"                            ' 表示モード（過去）

    Private Const RegistModeRegist As String = "1"                              ' 登録モード（登録する）
    Private Const RegistModeUnregist As String = "0"                            ' 登録モード（登録しない）

    Private Const RoStatusProcBeforeWork As Long = 1                            ' RO_STATUS（作業中前）
    Private Const RoStatusProcWorkToCompExaminationRequest As Long = 2          ' RO_STATUS（作業中～完成検査依頼中）
    Private Const RoStatusProcCompExaminationComplate As Long = 3               ' RO_STATUS（完成検査完了）
    Private Const RoStatusProcDeliveryWaitToDeliveryWork As Long = 4            ' RO_STATUS（納車準備待ち～納車作業中）
    Private Const RoStatusProcAfterDeliveried As Long = 5                       ' RO_STATUS（納車済み以降）

    Private Const DispTextPermView As String = "1"                              ' テキストボックス表示状態（表示する）
    Private Const TextInputModeUninput As String = "0"                          ' テキスト入力モード（入力させない）
    Private Const TextInputModeInput As String = "1"                            ' テキスト入力モード（入力する）

    Private Const SelectModeSelect As Long = 1                                  ' 選択状態
    Private Const SelectModeUnselect As Long = 0                                ' 非選択状態

    Private Const CheckModeUncheck As String = "0"                              ' 非選択状態
    Private Const CheckModeCheck As Long = 1                                    ' 選択状態
    Private Const CheckModeNoProblem As String = "1"                            ' 選択位置(NoProblem)
    Private Const CheckModeNeedInspection As String = "2"                       ' 選択位置(NeedInspection)
    Private Const CheckModeNeedReplace As String = "3"                          ' 選択位置(NeedReplace)
    Private Const CheckModeNeedFixing As String = "4"                           ' 選択位置(NeedFixing)
    Private Const CheckModeNeedCleaning As String = "5"                         ' 選択位置(NeedCleaning)
    Private Const CheckModeNeedSwapping As String = "6"                         ' 選択位置(NeedSwapping)
    Private Const CheckModeNoCheck As String = "7"                         ' 選択位置(NoCheck)
    Private Const CheckModeEnforcement As String = "1"                          ' 選択位置（項目1）
    Private Const CheckModeUncarriedOut As String = "2"                         ' 選択位置（項目2）

    Private Const NoProblemIdx As Long = 1                                      ' 表示位置(NoProblem)
    Private Const NeedInspectionIdx As Long = 2                                 ' 表示位置(NeedInspection)
    Private Const NeedReplaceIdx As Long = 3                                    ' 表示位置(NeedReplace)
    Private Const NeedFixingIdx As Long = 4                                     ' 表示位置(NeedFixing)
    Private Const NeedCleaningIdx As Long = 5                                   ' 表示位置(NeedCleaning)
    Private Const NeedSwappingIdx As Long = 6                                   ' 表示位置(NeedSwapping)

    Private Const AlreadyReplaceIdx As String = "1"                             ' 複数選択リスト（Replace）
    Private Const AlreadyFixIdx As String = "2"                                 ' 複数選択リスト（Fix）
    Private Const AlreadyCleanIdx As String = "3"                               ' 複数選択リスト（Clean）
    Private Const AlreadySwapIdx As String = "4"                                ' 複数選択リスト（Swap）

    Private Const InspectionNeedFlgRegister As String = "1"                     ' 検査必要フラグ（Register）
    Private Const SendOrRegisterFlgSend As String = "1"                         ' Send・Register切替フラグ（Send）

    Private Const UnsetRowLockVer As Long = -1                                  ' 行ロックバージョン未設定値

    Private Const InspecItemViewControlUnview As String = "0"                   ' 表示モード（表示しない）

    Private Const DefaultItemCD As String = "                    "              ' ItemCD未設定値
    Private Const DefaultJobInspectId As String = "0"                           ' JobInstructID未設定値
    Private Const DefaultJobInspectSeq As Long = 0                              ' JobInstructSeq未設定値
    Private Const DefaultAlreadyReplace As Long = 0                             ' Replaced選択状態（未選択）
    Private Const DefaultAlreadyFix As Long = 0                                 ' Fixed選択状態（未選択）
    Private Const DefaultAlreadyClean As Long = 0                               ' Cleaned選択状態（未選択）
    Private Const DefaultAlreadySwap As Long = 0                                ' Swapped選択状態（未選択）
    Private Const DefaultBeforeText As Decimal = -1                             ' Before入力内容（未入力値）
    Private Const DefaultAfterText As Decimal = -1                              ' After入力内容（未入力値）
    Private Const FormatRsltVal As String = "##0.##"                            ' Before、After共通の点検結果入力値のフォーマット(999.##値形式)

    Private Const PartIndexEngine As Long = 1                                   '部位インデックス(Engine)
    Private Const PartIndexInRoom As Long = 2                                   '部位インデックス(InRoom)
    Private Const PartIndexLeft As Long = 3                                     '部位インデックス(Left)
    Private Const PartIndexRight As Long = 4                                    '部位インデックス(Right)
    Private Const PartIndexUnder As Long = 5                                    '部位インデックス(Under)
    Private Const PartIndexTrunk As Long = 6                                    '部位インデックス(Trunk)
    Private Const PartIndexMaintenance As Long = 7                              '部位インデックス(Maintenance)
    '共通関数の戻り値にて、継続する値
    '0:正常終了
    '-9000:ワーニング
    Private arySuccessList As Long() = {0, -9000}
    '2019/06/27 TKM要件：型式対応 Start
    ''' <summary>
    ''' 日付型項目のデフォルト値（年）：1900
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DafaultDateYear As Integer = 1900
    '2019/06/27 TKM要件：納車実績未登録の場合表示しない End    
    
#End Region

#End Region

#Region "メンバ変数"

    Protected vin As String = String.Empty                'VIN
    Protected jobDtlId As String = String.Empty         'JOB_DTL_ID
    Protected viewMode As String = String.Empty          'ViewMode
    Protected amarkView As String = String.Empty                   'Aマーク

    'TKMローカル対応
    'Protected imarkView As String = String.Empty                   'Iマーク

    Protected roNum As String = String.Empty             'R/O番号
    Protected dealerCD As String = String.Empty        '販売店コード
    Protected branchCD As String = String.Empty        '店舗コード
    Protected account As String = String.Empty            '担当者
    Protected fromTCMainFlg As Boolean = False            '画面遷移元がTCMainかどうか(FMMainならTrue)
    Protected fromPreviewFlg As Boolean = False           '画面遷移元がチェックシートプレビューかどうか(SC3180202ならTrue)
    Protected roStatus As String = String.Empty                    'ROステータス
    Protected redraw As String = String.Empty                       'Redrawモード
    Protected nowStatus As String = String.Empty                   '現在のステータス

    Protected saChipID As String = String.Empty                     '来店者実績連番
    Protected basrezid As String = String.Empty                     'DMS予約ID
    Protected seqNo As String = String.Empty                       'RO_JOB_SEQ

    Protected isAllJobStart As Long = IsAllJobStartFalse '全Job開始判定フラグ(True：全Job開始)


    'VehicleChartボタン色
    Protected maintenanceBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected engineRoomBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected inroomBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected leftBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected rightBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected underBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected trunkBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    'VehicleChartボタン使用可不可
    Protected maintenanceBtnDisabled As String = "disabled"
    Protected engineRoomBtnDisabled As String = "disabled"
    Protected inroomBtnDisabled As String = "disabled"
    Protected leftBtnDisabled As String = "disabled"
    Protected rightBtnDisabled As String = "disabled"
    Protected underBtnDisabled As String = "disabled"
    Protected trunkBtnDisabled As String = "disabled"
    'TechnicalAdvice
    Protected technicianAdvice As String = String.Empty
    'Before/After表示文字列
    Protected beforeText As String = String.Empty
    Protected afterText As String = String.Empty
    '汎用
    Protected intPosIndex As String
    Protected intIndex As String
    Protected nowJobDtl As Decimal = -1
    Protected stallId As Decimal = -1
    Protected serviceID As Decimal = -1
    Protected rowLockVer As String
    Protected srvRowLockVer As Long = 0
    Protected roRowLockVer As Long = 0
    Protected trnRowLockVer As Long = 0
    Private mergeDataTable As New SC3180204InspectCodeMergeDataTable

    '【***完成検査_排他制御***】 start
    Protected serviceInLockVer As Long = 0
    Protected editFlag As String = "0"
    '【***完成検査_排他制御***】 end

    ' ''' <summary>
    ' ''' ビジネスロジック
    ' ''' </summary>
    ' ''' <remarks></remarks>
    Private businessLogic As New SC3180204BusinessLogic

    Protected saAccountId As String

    ''' <summary>マスタに販売店が登録されているか判定フラグ一覧</summary>
    Private specifyDlrCdFlgs As Dictionary(Of String, Boolean)

#End Region

#Region "初期処理"
    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ページの初期表示
        InitScreen()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' ページの初期表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitScreen(Optional ByVal inDtInspecItem As SC3180204RegistInfoDataTable = Nothing, _
                           Optional ByVal inDtMaintenance As SC3180204RegistInfoDataTable = Nothing, _
                           Optional ByVal inAdvice As String = "", _
                           Optional ByVal isExclusionFlag As Boolean = False)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnDataFind As Boolean = False

        ''ユーザ情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        ' 点検組み合わせマスタに指定販売店の点検項目が登録されているか判定する
        specifyDlrCdFlgs = businessLogic.GetDlrCdExistMst(roNum, dealerCD, branchCD)

        '現在のステータス
        nowStatus = ""

        'VehicleChart選択番号
        VehicleChartNo.Value = ""

        UserName.Value = staffInfo.UserName

        '検索処理(ヘッダ情報)
        Dim dtHeaderInfo As SC3180204HederInfoDataTable = businessLogic.GetHederInfo(dealerCD, branchCD, roNum, specifyDlrCdFlgs("TRANSACTION_EXIST"))

        '取得情報チェック
        If dtHeaderInfo IsNot Nothing AndAlso 0 < dtHeaderInfo.Count Then
            'データが取得できた場合
            If False = dtHeaderInfo(0).IsNull("REG_NUM") Then
                RegisterNoLabel.Text = dtHeaderInfo(0).REG_NUM
            End If
            AMark.Text = ""
            If False = dtHeaderInfo(0).IsNull("ACCEPTANCE_TYPE") Then
                If AcceptanceTypeWalkin = dtHeaderInfo(0).ACCEPTANCE_TYPE Then
                    AMark.Text = "A"
                Else
                    amarkView = " style=""visibility:hidden;"" "
                End If
            End If
            OrderNoLabel.Text = roNum
            If False = dtHeaderInfo(0).IsNull("USERNAME") Then
                BuyerNameLabel.Text = dtHeaderInfo(0).USERNAME
            End If

            If False = dtHeaderInfo(0).IsNull("CONTACT_PERSON_NAME") Then
                ContactPersonNameLabel.Text = dtHeaderInfo(0).CONTACT_PERSON_NAME
            End If
            If False = dtHeaderInfo(0).IsNull("CONTACT_PHONE") Then
                ContactPersonTelLabel.Text = dtHeaderInfo(0).CONTACT_PHONE
            End If
            If False = dtHeaderInfo(0).IsNull("MODEL_NAME") Then
                Series1Label.Text = dtHeaderInfo(0).MODEL_NAME
            End If

            'TKMローカル対応
            'IMark.Text = ""
            'If False = dtHeaderInfo(0).IsNull("IMP_VCL_FLG") Then
            '    If VipFlgTrue = dtHeaderInfo(0).IMP_VCL_FLG Then
            '        IMark.Text = "I"
            '    Else
            '        imarkView = " style=""visibility:hidden;"" "
            '    End If
            'End If

            VINLabel.Text = vin
            If False = dtHeaderInfo(0).IsNull("RSLT_DELI_DATETIME") AndAlso _
                DafaultDateYear < CDate(dtHeaderInfo(0).RSLT_DELI_DATETIME).Year Then   ' [サービス入庫].[実績納車日時]が未登録(1900年)の場合、値を出力しない
                DeliveryDate.Text = DateTimeFunc.FormatDate(ConvertDateYMD, CDate(dtHeaderInfo(0).RSLT_DELI_DATETIME))
            End If

            If False = dtHeaderInfo(0).IsNull("SVC_CLASS_NAME") Then
                Series2Label.Text = dtHeaderInfo(0).SVC_CLASS_NAME
            End If
            If False = dtHeaderInfo(0).IsNull("RSLT_SVCIN_DATETIME") Then
                ReceptionTimeLabel.Text = DateTimeFunc.FormatDate(ConvertDateMD, CDate(dtHeaderInfo(0).RSLT_SVCIN_DATETIME)) & " " & _
                    DateTimeFunc.FormatDate(ConvertDateHM, CDate(dtHeaderInfo(0).RSLT_SVCIN_DATETIME))
            End If
            If False = dtHeaderInfo(0).IsNull("SCHE_DELI_DATETIME") Then
                ScheDeliDate.Text = DateTimeFunc.FormatDate(ConvertDateMD, CDate(dtHeaderInfo(0).SCHE_DELI_DATETIME)) & " " & _
                    DateTimeFunc.FormatDate(ConvertDateHM, CDate(dtHeaderInfo(0).SCHE_DELI_DATETIME))
            End If
            If False = dtHeaderInfo(0).IsNull("SVCIN_ID") Then
                serviceID = Long.Parse(dtHeaderInfo(0).SVCIN_ID)
            End If
            rowLockVer = ""
            If False = dtHeaderInfo(0).IsNull("SRV_ROW_LOCK_VERSION") Then
                rowLockVer = dtHeaderInfo(0).SRV_ROW_LOCK_VERSION
                If rowLockVer <> "" Then
                    srvRowLockVer = Long.Parse(rowLockVer)
                End If
            End If

            saAccountId = ""
            If False = dtHeaderInfo(0).IsNull("ACCOUNT") Then
                saAccountId = dtHeaderInfo(0).ACCOUNT
            End If
        End If

        beforeText = WebWordUtility.GetWord(MsgID.id22)
        afterText = WebWordUtility.GetWord(MsgID.id23)

        ''2014/06/13 仕様変更対応 Start
        'If CheckDataCount() = False Then
        '    Return
        'End If
        ''2014/06/13 仕様変更対応 End

        'Register又はSendの切替用フラグ(空文字:Register/1:Send)
        SendOrRegister.Value = ""

        '2014/06/03 InspectionHeadのカウント取得 Start
        'Dim checkFlg As Boolean = businessLogic.SelectInspectionHeadCount(jobDtlId)
        'Dim checkFlg As Boolean = businessLogic.SelectInspectionHeadCount(dealerCD, branchCD, roNum)
        '2014/06/03 InspectionHeadのカウント取得 End

        'ROステータスを取得
        'ROステータス取得のためにInspectCodeを取得(部位指定なし)
        'Dim dtInspecCode As SC3180204InspectCodeDataTable
        'dtInspecCode = businessLogic.GetAllInspecCode(dealerCD, branchCD, roNum, nowStatus, roStatus, checkFlg)
        'businessLogic.GetAllInspecCode(dealerCD, branchCD, roNum, nowStatus, roStatus, checkFlg)
        '2015/04/14 新販売店追加対応 start
        '検索処理(OperationItems)

        'All
        Dim dtInspecCode As SC3180204InspectCodeDataTable
        '2019/12/02 NCN 吉川　TKM要件：型式対応 Start 
        'dtInspecCode = businessLogic.GetInspecCode(staffInfo, dealerCD, branchCD, roNum, specifyDlrCdFlgs, mergeDataTable, isExistActive)
        dtInspecCode = businessLogic.GetInspecCode(staffInfo, dealerCD, branchCD, roNum, specifyDlrCdFlgs, mergeDataTable)
        '2019/12/02 NCN 吉川　TKM要件：型式対応 End
        '【***完成検査_排他制御***】 start
        If Not IsNothing(inDtInspecItem) Then

            Dim tmp As SC3180204RegistInfoRow()

            For dtInsCounter As Integer = 0 To dtInspecCode.Count - 1
                tmp = Nothing
                tmp = CType(inDtInspecItem.Select(String.Format("ItemCD = '{0}' ", dtInspecCode(dtInsCounter).INSPEC_ITEM_CD.Replace("'", "''"))), SC3180204RegistInfoRow())
                If tmp.Count > 0 Then

                    '選択項目があれば選択状態にする
                    '選択可能なアイコン一覧を取得しtmp(0).ItemsCheckが存在するか確認の必要あり
                    If tmp(0).ItemsCheck > 0 Then
                        dtInspecCode(dtInsCounter).INSPEC_RSLT_CD = CStr(tmp(0).ItemsCheck)
                    End If

                    '入力欄があれば入力状態とする
                    '値入力欄が表示される項目であるかを確認する必要はない（マスタの変更は考慮しないため）

                    '入力していたBefAftを保持
                    If tmp(0).ItemsTextBefore > DefaultBeforeText Then
                        dtInspecCode(dtInsCounter).RSLT_VAL_BEFORE = CStr(tmp(0).ItemsTextBefore)
                    End If
                    If tmp(0).ItemsTextAfter > DefaultAfterText Then
                        dtInspecCode(dtInsCounter).RSLT_VAL_AFTER = CStr(tmp(0).ItemsTextAfter)
                    End If

                    'それぞれ選択可能であれば選択状態とする
                    '各項目がリストの対象になっているかを確認する必要はない（マスタの変更は考慮しないため）

                    dtInspecCode(dtInsCounter).OPERATION_RSLT_ALREADY_CLEAN = CStr(tmp(0).ItemsSelect_Cleaned)
                    dtInspecCode(dtInsCounter).OPERATION_RSLT_ALREADY_FIX = CStr(tmp(0).ItemsSelect_Fixed)
                    dtInspecCode(dtInsCounter).OPERATION_RSLT_ALREADY_REPLACE = CStr(tmp(0).ItemsSelect_Replaced)
                    dtInspecCode(dtInsCounter).OPERATION_RSLT_ALREADY_SWAP = CStr(tmp(0).ItemsSelect_Swapped)

                    '変更フラグの反映
                    dtInspecCode(dtInsCounter).EDIT_FLAG = CStr(tmp(0).EDIT_FLAG)

                End If
            Next
        End If
        '【***完成検査_排他制御***】 end

        'EngineRoom
        Dim dtInspecCode_Engine As SC3180204InspectCodeDataTable
        dtInspecCode_Engine = CType(dtInspecCode.Clone, SC3180204InspectCodeDataTable)
        Dim rowEngine As DataRow
        For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCdEngine))
            rowEngine = dtInspecCode_Engine.NewRow
            For n As Integer = 0 To rowSource.ItemArray.Length - 1
                rowEngine(n) = rowSource(n)
            Next
            dtInspecCode_Engine.Rows.Add(rowEngine)
        Next

        EditInspecCode(dtInspecCode_Engine)
        engineRoomBtnDisabled = "return false;"
        EngineRoomCheckCount.Value = dtInspecCode_Engine.Count.ToString
        If 0 < dtInspecCode_Engine.Count Then
            engineRoomBtnColor = "background:-webkit-gradient(linear, left top, left bottom, from(#FCBF05), to(#A17A03));"
            engineRoomBtnDisabled = ""
            EngineRoomLabel.Text = dtInspecCode_Engine(0).PART_NAME
        End If
        InspecItemsList_Engine.DataSource = dtInspecCode_Engine
        InspecItemsList_Engine.DataBind()

        'Inroom
        Dim dtInspecCode_Inroom As SC3180204InspectCodeDataTable
        dtInspecCode_Inroom = CType(dtInspecCode.Clone, SC3180204InspectCodeDataTable)
        Dim rowInRoom As DataRow
        For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCdInRoom))
            rowInRoom = dtInspecCode_Inroom.NewRow
            For n As Integer = 0 To rowSource.ItemArray.Length - 1
                rowInRoom(n) = rowSource(n)
            Next
            dtInspecCode_Inroom.Rows.Add(rowInRoom)
        Next

        EditInspecCode(dtInspecCode_Inroom)
        inroomBtnDisabled = "return false;"
        LeftCheckCount.Value = dtInspecCode_Inroom.Count.ToString
        If 0 < dtInspecCode_Inroom.Count Then
            inroomBtnColor = "background:-webkit-gradient(linear, left top, left bottom, from(#FCBF05), to(#A17A03));"
            inroomBtnDisabled = ""
            InroomLabel.Text = dtInspecCode_Inroom(0).PART_NAME
        End If
        InspecItemsList_Inroom.DataSource = dtInspecCode_Inroom
        InspecItemsList_Inroom.DataBind()

        'Left
        Dim dtInspecCode_Left As SC3180204InspectCodeDataTable
        dtInspecCode_Left = CType(dtInspecCode.Clone, SC3180204InspectCodeDataTable)
        Dim rowLeft As DataRow
        For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCdLeft))
            rowLeft = dtInspecCode_Left.NewRow
            For n As Integer = 0 To rowSource.ItemArray.Length - 1
                rowLeft(n) = rowSource(n)
            Next
            dtInspecCode_Left.Rows.Add(rowLeft)
        Next

        EditInspecCode(dtInspecCode_Left)
        leftBtnDisabled = "return false;"
        LeftCheckCount.Value = dtInspecCode_Left.Count.ToString
        If 0 < dtInspecCode_Left.Count Then
            leftBtnColor = "background:-webkit-gradient(linear, left top, left bottom, from(#FCBF05), to(#A17A03));"
            leftBtnDisabled = ""
            LeftLabel.Text = dtInspecCode_Left(0).PART_NAME
        End If
        InspecItemsList_Left.DataSource = dtInspecCode_Left
        InspecItemsList_Left.DataBind()

        'Right
        Dim dtInspecCode_Right As SC3180204InspectCodeDataTable
        dtInspecCode_Right = CType(dtInspecCode.Clone, SC3180204InspectCodeDataTable)
        Dim rowRight As DataRow
        For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCdRight))
            rowRight = dtInspecCode_Right.NewRow
            For n As Integer = 0 To rowSource.ItemArray.Length - 1
                rowRight(n) = rowSource(n)
            Next
            dtInspecCode_Right.Rows.Add(rowRight)
        Next

        EditInspecCode(dtInspecCode_Right)
        rightBtnDisabled = "return false;"
        RightCheckCount.Value = dtInspecCode_Right.Count.ToString
        If 0 < dtInspecCode_Right.Count Then
            rightBtnColor = "background:-webkit-gradient(linear, left top, left bottom, from(#FCBF05), to(#A17A03));"
            rightBtnDisabled = ""
            RightLabel.Text = dtInspecCode_Right(0).PART_NAME
        End If
        InspecItemsList_Right.DataSource = dtInspecCode_Right
        InspecItemsList_Right.DataBind()

        'Under
        Dim dtInspecCode_Under As SC3180204InspectCodeDataTable
        dtInspecCode_Under = CType(dtInspecCode.Clone, SC3180204InspectCodeDataTable)
        Dim rowUnder As DataRow
        For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCdUnder))
            rowUnder = dtInspecCode_Under.NewRow
            For n As Integer = 0 To rowSource.ItemArray.Length - 1
                rowUnder(n) = rowSource(n)
            Next
            dtInspecCode_Under.Rows.Add(rowUnder)
        Next

        EditInspecCode(dtInspecCode_Under)
        underBtnDisabled = "return false;"
        UnderCheckCount.Value = dtInspecCode_Under.Count.ToString
        If 0 < dtInspecCode_Under.Count Then
            underBtnColor = "background:-webkit-gradient(linear, left top, left bottom, from(#FCBF05), to(#A17A03));"
            underBtnDisabled = ""
            UnderLabel.Text = dtInspecCode_Under(0).PART_NAME
        End If
        InspecItemsList_Under.DataSource = dtInspecCode_Under
        InspecItemsList_Under.DataBind()

        'Trunk
        Dim dtInspecCode_Trunk As SC3180204InspectCodeDataTable
        dtInspecCode_Trunk = CType(dtInspecCode.Clone, SC3180204InspectCodeDataTable)
        Dim rowTrunk As DataRow
        For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCdTrunk))
            rowTrunk = dtInspecCode_Trunk.NewRow
            For n As Integer = 0 To rowSource.ItemArray.Length - 1
                rowTrunk(n) = rowSource(n)
            Next
            dtInspecCode_Trunk.Rows.Add(rowTrunk)
        Next

        EditInspecCode(dtInspecCode_Trunk)
        trunkBtnDisabled = "return false;"
        TrunkCheckCount.Value = dtInspecCode_Trunk.Count.ToString
        If 0 < dtInspecCode_Trunk.Count Then
            trunkBtnColor = "background:-webkit-gradient(linear, left top, left bottom, from(#FCBF05), to(#A17A03));"
            trunkBtnDisabled = ""
            TrunkLabel.Text = dtInspecCode_Trunk(0).PART_NAME
        End If
        InspecItemsList_Trunk.DataSource = dtInspecCode_Trunk
        InspecItemsList_Trunk.DataBind()

        'Maintenance
        Dim dtMainteCode As SC3180204MainteCodeListDataTable
        ''2014/06/06 引数変更（未使用を削除）　Start
        'dtMainteCode = businessLogic.GetMainteCodeList(staffInfo, nowStatus, dealerCD, branchCD, roNum)
        'dtMainteCode = businessLogic.GetMainteCodeList(staffInfo, dealerCD, branchCD, roNum)
        '2019/12/02 NCN 吉川　TKM要件：型式対応 Start
        'dtMainteCode = businessLogic.GetMainteCodeList(staffInfo, dealerCD, branchCD, roNum, specifyDlrCdFlgs, isExistActive)
        dtMainteCode = businessLogic.GetMainteCodeList(staffInfo, dealerCD, branchCD, roNum, specifyDlrCdFlgs)
        '2019/12/02 NCN 吉川　TKM要件：型式対応 End
        '2014/06/06 引数変更（未使用を削除）　End
        '2015/04/14 新販売店追加対応 end

        '【***完成検査_排他制御***】 start
        If Not IsNothing(inDtMaintenance) Then

            Dim Count As Integer = 0

            For Each tmp As SC3180204RegistInfoRow In inDtMaintenance


                '選択項目があれば選択状態にする
                dtMainteCode(Count).INSPEC_RSLT_CD = CStr(tmp.ItemsCheck)

                '変更フラグの反映
                If isExclusionFlag Then
                    dtMainteCode(Count).EDIT_FLAG = CStr(1)
                End If

                Count = Count + 1
            Next
        End If


        '【***完成検査_排他制御***】 end

        EditInspecCode(dtMainteCode)
        maintenanceBtnDisabled = "return false;"
        MaintenanceCheckCount.Value = dtMainteCode.Count.ToString
        If 0 < dtMainteCode.Count Then
            maintenanceBtnColor = "background:-webkit-gradient(linear, left top, left bottom, from(#66DA65), to(#228221));"
            maintenanceBtnDisabled = ""
        End If
        InspecItemsList_Maintenance.DataSource = dtMainteCode
        InspecItemsList_Maintenance.DataBind()

		'【***完成検査_排他制御***】 start
        '画面入力のアドバイスがあれば一時退避
        Dim tempAdvice As String = String.Empty
        If Not String.IsNullOrEmpty(inAdvice) Then
            tempAdvice = inAdvice
        End If
		'【***完成検査_排他制御***】 end
		
        'TechnicianAdvice
        technicianAdvice = ""
        '2014/09/09 複数チップが存在する場合、テクニシャンアドバイスが取得できない可能性が高い為、取得方法修正 Start
        If 0 < dtInspecCode_Engine.Count Then
            VehicleChartNo.Value = VehicleChartNoEngine
            blnDataFind = True
            'If False = dtInspecCode_Engine(0).IsNull("ADVICE_CONTENT") Then
            '    technicianAdvice = dtInspecCode_Engine(0).ADVICE_CONTENT.ToString.Trim()
            'End If
        ElseIf 0 < dtInspecCode_Inroom.Count Then
            blnDataFind = True
            'If False = dtInspecCode_Inroom(0).IsNull("ADVICE_CONTENT") Then
            '    technicianAdvice = dtInspecCode_Inroom(0).ADVICE_CONTENT.ToString.Trim()
            'End If
        ElseIf 0 < dtInspecCode_Left.Count Then
            blnDataFind = True
            'If False = dtInspecCode_Left(0).IsNull("ADVICE_CONTENT") Then
            '    technicianAdvice = dtInspecCode_Left(0).ADVICE_CONTENT.ToString.Trim()
            'End If
        ElseIf 0 < dtInspecCode_Right.Count Then
            blnDataFind = True
            'If False = dtInspecCode_Right(0).IsNull("ADVICE_CONTENT") Then
            '    technicianAdvice = dtInspecCode_Right(0).ADVICE_CONTENT.ToString.Trim()
            'End If
        ElseIf 0 < dtInspecCode_Under.Count Then
            blnDataFind = True
            'If False = dtInspecCode_Under(0).IsNull("ADVICE_CONTENT") Then
            '    technicianAdvice = dtInspecCode_Under(0).ADVICE_CONTENT.ToString.Trim()
            'End If
        ElseIf 0 < dtInspecCode_Trunk.Count Then
            blnDataFind = True
            'If False = dtInspecCode_Trunk(0).IsNull("ADVICE_CONTENT") Then
            '    technicianAdvice = dtInspecCode_Trunk(0).ADVICE_CONTENT.ToString.Trim()
            'End If
        ElseIf 0 < dtMainteCode.Count Then
            blnDataFind = True
            'If False = dtMainteCode(0).IsNull("ADVICE_CONTENT") Then
            '    technicianAdvice = dtMainteCode(0).ADVICE_CONTENT.ToString.Trim()
            'End If
        End If

		'【***完成検査_排他制御***】 start
        '元々入力があったアドバイスが（一時退避に）あればそれを使う 【***完成検査_排他制御***】対応
        If Not (String.Empty).Equals(tempAdvice) Then
            technicianAdvice = tempAdvice   '【***完成検査_排他制御***】対応
        Else
        'RO番号をキーに、登録されているテクニシャンアドバイスを取得する
        technicianAdvice = businessLogic.GetAdviceContent(dealerCD, branchCD, roNum, specifyDlrCdFlgs("TRANSACTION_EXIST"))
        '2014/09/09 複数チップが存在する場合、テクニシャンアドバイスが取得できない可能性が高い為、取得方法修正 End

        End If
        '【***完成検査_排他制御***】 end
        
        technicianAdvice = Server.HtmlEncode(technicianAdvice)

        Me.hdnErrorMsg.Value = ""
        If False = blnDataFind Then
            '読み込みエラーメッセージの取得
            ErrorMessage.Text = "Error"
            ErrorFlg.Value = ErrorFlgError
            ErrorMessage.Text = WebWordUtility.GetWord(MsgID.id35)
            Me.hdnErrorMsg.Value = WebWordUtility.GetWord(MsgID.id35)
        End If

        'チェックエラーメッセージの取得
        ItemCheckErrorMessage.Value = WebWordUtility.GetWord(MsgID.id34)

        '2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応　Start
        overText.Value = WebWordUtility.GetWord(MsgID.id59)
        '2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応　End

        '編集中メッセージの取得
        EditedMessage.Value = WebWordUtility.GetWord(MsgID.id36)

        'フッタボタンの初期化
        InitFooterButton(staffInfo)

        '2014/12/10 [JobDispatch完成検査入力制御開発]対応　Start
        '業務フッタ設定
        InitBusinessButton()
        '2014/12/10 [JobDispatch完成検査入力制御開発]対応　End

        '【***完成検査_排他制御***】 start
        '事前排他チェック用の行ロックバージョンを取得


        '作業内容の行ロックバージョン
        serviceInLockVer = businessLogic.GetServiceInLock(serviceID)

        '【***完成検査_排他制御***】 end

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
#End Region

#Region "ボタン処理(共通分)"
    ''' <summary>
    ''' フッターボタンの初期化
    ''' </summary>
    ''' <param name="inStaffInfo">ログインユーザー情報</param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub InitFooterButton(ByVal inStaffInfo As StaffContext)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メインメニューボタンの設定
        Dim mainMenuButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMainMenu)
        AddHandler mainMenuButton.Click, AddressOf MainMenuButton_Click
        mainMenuButton.OnClientClick = _
            String.Format(CultureInfo.CurrentCulture, _
                          FooterReplaceEvent, _
                          FooterMainMenu.ToString(CultureInfo.CurrentCulture))

        '権限チェック
        If inStaffInfo.OpeCD = Operation.SM Then
            'ServiceManager(SM)権限

            '顧客詳細ボタンの設定
            Dim customerDetailsButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterCustomer)
            If customerDetailsButton IsNot Nothing Then
                AddHandler customerDetailsButton.Click, AddressOf CustomerDetailsButton_Click
                customerDetailsButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterCustomer.ToString(CultureInfo.CurrentCulture))
            End If

            'R/O一覧ボタンの設定
            Dim roListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterRo)
            If roListButton IsNot Nothing Then
                AddHandler roListButton.Click, AddressOf ROListButton_Click
                roListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterRo.ToString(CultureInfo.CurrentCulture))
            End If

            '商品訴求コンテンツボタンの設定
            Dim goodsSolicitationContentsButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterContents)
            If goodsSolicitationContentsButton IsNot Nothing Then
                AddHandler goodsSolicitationContentsButton.Click, AddressOf GoodsSolicitationContentsButton_Click
                goodsSolicitationContentsButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterContents.ToString(CultureInfo.CurrentCulture))
            End If

            'キャンペーンボタンの設定
            Dim campainButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterCampaing)
            If campainButton IsNot Nothing Then
                AddHandler campainButton.Click, AddressOf CampainButton_Click
                campainButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterCampaing.ToString(CultureInfo.CurrentCulture))
            End If

            '予約管理ボタンの設定
            Dim orderControlButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterVisitManament)
            If orderControlButton IsNot Nothing Then
                AddHandler orderControlButton.Click, AddressOf OrderControlButton_Click
                orderControlButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterVisitManament.ToString(CultureInfo.CurrentCulture))
            End If

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterSmb)
            If smbButton IsNot Nothing Then
                AddHandler smbButton.Click, AddressOf SMBButton_Click
                smbButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterSmb.ToString(CultureInfo.CurrentCulture))
            End If

        ElseIf inStaffInfo.OpeCD = Operation.SA Then
            'ServiceAdvisor(SA)権限

            '顧客詳細ボタンの設定
            Dim customerDetailsButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterCustomer)
            If customerDetailsButton IsNot Nothing Then
                AddHandler customerDetailsButton.Click, AddressOf CustomerDetailsButton_Click
                customerDetailsButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterCustomer.ToString(CultureInfo.CurrentCulture))
            End If

            'R/O一覧ボタンの設定
            Dim roListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterRo)
            If roListButton IsNot Nothing Then
                AddHandler roListButton.Click, AddressOf ROListButton_Click
                roListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterRo.ToString(CultureInfo.CurrentCulture))
            End If

            '商品訴求コンテンツボタンの設定
            Dim goodsSolicitationContentsButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterContents)
            If goodsSolicitationContentsButton IsNot Nothing Then
                AddHandler goodsSolicitationContentsButton.Click, AddressOf GoodsSolicitationContentsButton_Click
                goodsSolicitationContentsButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterContents.ToString(CultureInfo.CurrentCulture))
            End If

            'キャンペーンボタンの設定
            Dim campainButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterCampaing)
            If campainButton IsNot Nothing Then
                AddHandler campainButton.Click, AddressOf CampainButton_Click
                campainButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterCampaing.ToString(CultureInfo.CurrentCulture))
            End If

            '予約管理ボタンの設定
            Dim orderControlButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterVisitManament)
            If orderControlButton IsNot Nothing Then
                AddHandler orderControlButton.Click, AddressOf OrderControlButton_Click
                orderControlButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterVisitManament.ToString(CultureInfo.CurrentCulture))
            End If

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterSmb)
            If smbButton IsNot Nothing Then
                AddHandler smbButton.Click, AddressOf SMBButton_Click
                smbButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterSmb.ToString(CultureInfo.CurrentCulture))
            End If

        ElseIf inStaffInfo.OpeCD = Operation.TEC Then
            'Tchnician(TC)権限
            'R/O一覧ボタンの設定
            Dim roListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterRo)
            If roListButton IsNot Nothing Then
                AddHandler roListButton.Click, AddressOf ROListButton_Click
                roListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterRo.ToString(CultureInfo.CurrentCulture))
            End If

            '追加作業一覧ボタンの設定
            Dim additionalWorkListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterAddList)
            If additionalWorkListButton IsNot Nothing Then
                AddHandler additionalWorkListButton.Click, AddressOf AdditionalWorkListButton_Click
                additionalWorkListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterAddList.ToString(CultureInfo.CurrentCulture))
            End If

        ElseIf inStaffInfo.OpeCD = Operation.CHT Then
            'ChiefTechnician(GS)権限

            'TCメインボタンの設定
            Dim tcMainButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterTecnicianMain)
            If tcMainButton IsNot Nothing Then
                AddHandler tcMainButton.Click, AddressOf TCMainButton_Click
                tcMainButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterTecnicianMain.ToString(CultureInfo.CurrentCulture))
            End If

            'FMメインボタンの設定
            Dim fmMainButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterFormanMain)
            If fmMainButton IsNot Nothing Then
                AddHandler fmMainButton.Click, AddressOf FMMainButton_Click
                fmMainButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterFormanMain.ToString(CultureInfo.CurrentCulture))
            End If

            'R/O一覧ボタンの設定
            Dim roListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterRo)
            If roListButton IsNot Nothing Then
                AddHandler roListButton.Click, AddressOf ROListButton_Click
                roListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterRo.ToString(CultureInfo.CurrentCulture))
            End If

            '追加作業一覧ボタンの設定
            Dim additionalWorkListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterAddList)
            If additionalWorkListButton IsNot Nothing Then
                AddHandler additionalWorkListButton.Click, AddressOf AdditionalWorkListButton_Click
                additionalWorkListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterAddList.ToString(CultureInfo.CurrentCulture))
            End If

        ElseIf inStaffInfo.OpeCD = Operation.FM Then
            'Foreman(FM)権限

            'SMBボタンの設定
            Dim smbButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterSmb)
            If smbButton IsNot Nothing Then
                AddHandler smbButton.Click, AddressOf SMBButton_Click
                smbButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterSmb.ToString(CultureInfo.CurrentCulture))
            End If

            'R/O一覧ボタンの設定
            Dim roListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterRo)
            If roListButton IsNot Nothing Then
                AddHandler roListButton.Click, AddressOf ROListButton_Click
                roListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterRo.ToString(CultureInfo.CurrentCulture))
            End If

            '追加作業一覧ボタンの設定
            Dim additionalWorkListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterAddList)
            If additionalWorkListButton IsNot Nothing Then
                AddHandler additionalWorkListButton.Click, AddressOf AdditionalWorkListButton_Click
                additionalWorkListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterAddList.ToString(CultureInfo.CurrentCulture))
            End If

        ElseIf inStaffInfo.OpeCD = Operation.CT Then
            'Controller(CT)権限

            'R/O一覧ボタンの設定
            Dim roListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterRo)
            If roListButton IsNot Nothing Then
                AddHandler roListButton.Click, AddressOf ROListButton_Click
                roListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterRo.ToString(CultureInfo.CurrentCulture))
            End If

            '追加作業一覧ボタンの設定
            Dim additionalWorkListButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterAddList)
            If additionalWorkListButton IsNot Nothing Then
                AddHandler additionalWorkListButton.Click, AddressOf AdditionalWorkListButton_Click
                additionalWorkListButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterAddList.ToString(CultureInfo.CurrentCulture))
            End If

        End If

        '電話帳ボタンの設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterTelDirector)

        telDirectoryButton.OnClientClick = FooterEventTel

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' メインメニューボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub MainMenuButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        '権限によって遷移先を変える
        If staffInfo.OpeCD = Operation.SA Then
            'メインメニュー(SA)に遷移する
            Me.RedirectNextScreen(ProgramIdMinaMenuSa)

        ElseIf staffInfo.OpeCD = Operation.SM Then
            '全体管理に遷移する
            Me.RedirectNextScreen(ProgramIdAllManagment)

        ElseIf staffInfo.OpeCD = Operation.CT OrElse staffInfo.OpeCD = Operation.CHT Then
            '工程管理に遷移する
            Me.RedirectNextScreen(ProgramIdProcessControl)

        ElseIf staffInfo.OpeCD = Operation.TEC Then
            'メインメニュー(TC)に遷移する
            Me.RedirectNextScreen(ProgramIdMainMenuTc)

        ElseIf staffInfo.OpeCD = Operation.FM Then
            'メインメニュー(FM)に遷移する
            Me.RedirectNextScreen(ProgramIdMainMenuFm)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 全体管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub AllManagmentButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionRO, roNum)
        Me.SetValue(ScreenPos.Next, SessionVINNO, vin)
        Me.SetValue(ScreenPos.Next, SessionViewMode, viewMode)
        Me.RedirectNextScreen(ProgramIdAllManagment)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' SAメインボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub SAMainButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionRO, roNum)
        Me.SetValue(ScreenPos.Next, SessionVINNO, vin)
        Me.SetValue(ScreenPos.Next, SessionViewMode, viewMode)
        Me.RedirectNextScreen(ProgramIdSaMain)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' TCメインボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub TCMainButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionRO, roNum)
        Me.SetValue(ScreenPos.Next, SessionVINNO, vin)
        Me.SetValue(ScreenPos.Next, SessionViewMode, viewMode)
        Me.RedirectNextScreen(ProgramIdTcMain)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' SMBボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub SMBButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionRO, roNum)
        Me.SetValue(ScreenPos.Next, SessionVINNO, vin)
        Me.SetValue(ScreenPos.Next, SessionViewMode, viewMode)
        Me.RedirectNextScreen(ProgramIdSMB)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' FMメインボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub FMMainButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionRO, roNum)
        Me.SetValue(ScreenPos.Next, SessionVINNO, vin)
        Me.SetValue(ScreenPos.Next, SessionViewMode, viewMode)
        Me.RedirectNextScreen(ProgramIdFmMain)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 顧客詳細ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub CustomerDetailsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionRO, roNum)
        Me.SetValue(ScreenPos.Next, SessionVINNO, vin)
        Me.SetValue(ScreenPos.Next, SessionViewMode, viewMode)
        Me.RedirectNextScreen(ProgramIdCustomerDetails)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' R/O一覧ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub ROListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Using biz As New SC3180204BusinessLogic


            'DMS情報取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

            'DMS情報のチェック
            If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                '取得できた場合
                '画面間パラメータを設定
                '表示番号
                Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNumRoList)

                'DMS販売店コード
                Me.SetValue(ScreenPos.Next, SessionParam01, dtDmsCodeMapDataTable(0).CODE1)

                'DMS店舗コード
                Me.SetValue(ScreenPos.Next, SessionParam02, dtDmsCodeMapDataTable(0).CODE2)

                'アカウント
                Me.SetValue(ScreenPos.Next, SessionParam03, dtDmsCodeMapDataTable(0).ACCOUNT)

                '来店実績連番
                Me.SetValue(ScreenPos.Next, SessionParam04, saChipID)

                'DMS予約ID
                Me.SetValue(ScreenPos.Next, SessionParam05, basrezid)

                'RO番号
                Me.SetValue(ScreenPos.Next, SessionParam06, roNum)

                'RO作業連番
                Me.SetValue(ScreenPos.Next, SessionParam07, seqNo)

                'VIN
                Me.SetValue(ScreenPos.Next, SessionParam08, vin)

                '編集モード
                Me.SetValue(ScreenPos.Next, SessionParam09, viewMode)

                '追加作業画面(枠)に遷移する
                Me.RedirectNextScreen(ProgramIdOtherLinkage)

            Else
                '取得できなかった場合
                'エラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} ERROR " _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 追加作業一覧ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub AdditionalWorkListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Using biz As New SC3180204BusinessLogic


            'DMS情報取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

            'DMS情報のチェック
            If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                '取得できた場合
                '画面間パラメータを設定
                '表示番号
                Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNumAddList)

                'DMS販売店コード
                Me.SetValue(ScreenPos.Next, SessionParam01, dtDmsCodeMapDataTable(0).CODE1)

                'DMS店舗コード
                Me.SetValue(ScreenPos.Next, SessionParam02, dtDmsCodeMapDataTable(0).CODE2)

                'アカウント
                Me.SetValue(ScreenPos.Next, SessionParam03, staffInfo.Account.Substring(0, staffInfo.Account.IndexOf("@")))

                '来店実績連番
                Me.SetValue(ScreenPos.Next, SessionParam04, saChipID)

                'DMS予約ID
                Me.SetValue(ScreenPos.Next, SessionParam05, basrezid)

                'RO番号
                Me.SetValue(ScreenPos.Next, SessionParam06, roNum)

                'RO作業連番
                Me.SetValue(ScreenPos.Next, SessionParam07, seqNo)

                'VIN
                Me.SetValue(ScreenPos.Next, SessionParam08, vin)

                '編集モード
                Me.SetValue(ScreenPos.Next, SessionParam09, viewMode)

                '追加作業画面(枠)に遷移する
                Me.RedirectNextScreen(ProgramIdOtherLinkage)

            Else
                '取得できなかった場合
                'エラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} ERROR " _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name))
            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 商品訴求コンテンツボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub GoodsSolicitationContentsButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        '画面間パラメータを設定
        'DMS販売店コード
        Me.SetValue(ScreenPos.Next, SessionKeyGoodsContentsDearlerCode, Space(1))

        'DMS店舗コード
        Me.SetValue(ScreenPos.Next, SessionKeyGoodsContentsBranch, Space(1))

        'アカウント
        Me.SetValue(ScreenPos.Next, SessionKeyGoodsContentsAcount, Space(1))

        '来店実績連番
        Me.SetValue(ScreenPos.Next, SessionKeyGoodsContentsSAChipId, saChipID)

        'DMS予約ID
        Me.SetValue(ScreenPos.Next, SessionKeyGoodsContentsBaserzId, basrezid)

        'RO番号
        Me.SetValue(ScreenPos.Next, SessionKeyGoodsContentsRo, roNum)

        'RO作業連番
        Me.SetValue(ScreenPos.Next, SessionKeyGoodsContentsSeqNo, seqNo)

        'VIN
        Me.SetValue(ScreenPos.Next, SessionKeyGoodsContentsVinNo, vin)

        '編集モード
        Me.SetValue(ScreenPos.Next, SessionKeyGoodsContentsViewMode, viewMode)

        '商品訴求コンテンツ画面に遷移する
        Me.RedirectNextScreen(ProgramIdGoodsSolication)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' キャンペーンボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub CampainButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Using biz As New SC3180204BusinessLogic


            'DMS情報取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

            'DMS情報のチェック
            If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                '取得できた場合
                '画面間パラメータを設定
                '表示番号
                Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNumCampaing)

                'DMS販売店コード
                Me.SetValue(ScreenPos.Next, SessionParam01, dtDmsCodeMapDataTable(0).CODE1)

                'DMS店舗コード
                Me.SetValue(ScreenPos.Next, SessionParam02, dtDmsCodeMapDataTable(0).CODE2)

                'アカウント
                Me.SetValue(ScreenPos.Next, SessionParam03, dtDmsCodeMapDataTable(0).ACCOUNT)

                '来店実績連番
                Me.SetValue(ScreenPos.Next, SessionParam04, saChipID)

                'DMS予約ID
                Me.SetValue(ScreenPos.Next, SessionParam05, basrezid)

                'RO番号
                Me.SetValue(ScreenPos.Next, SessionParam06, roNum)

                'RO作業連番
                Me.SetValue(ScreenPos.Next, SessionParam07, seqNo)

                'VIN
                Me.SetValue(ScreenPos.Next, SessionParam08, vin)

                '編集モード
                Me.SetValue(ScreenPos.Next, SessionParam09, viewMode)

                '追加作業画面(枠)に遷移する
                Me.RedirectNextScreen(ProgramIdOtherLinkage)

            Else
                '取得できなかった場合
                'エラー
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                         , "{0}.{1} ERROR " _
                                         , Me.GetType.ToString _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name))

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 予約管理ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub OrderControlButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionRO, roNum)
        Me.SetValue(ScreenPos.Next, SessionVINNO, vin)
        Me.SetValue(ScreenPos.Next, SessionViewMode, viewMode)
        Me.RedirectNextScreen(ProgramIdOrderControl)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "ボタン処理(個別分)"

    '2014/12/10 [JobDispatch完成検査入力制御開発]対応　Start
    ''' <summary>
    ''' チップ単位で全Jobが開始されているかチェックし、結果をメンバ変数に格納する
    ''' （※JavaScriptでその値を元に、[Send／Register]ボタンの[活性／非活性]制御を行う）
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitBusinessButton()

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        ' ReadOnlyモード、もしくはエラー発生時、処理を抜ける
        If (viewMode <> "" And _
            viewMode <> ViewModeEdit) _
           Or ErrorFlg.Value = ErrorFlgError Then

            Exit Sub
        End If

        'メンバ変数に、チップ単位の全Job開始状況を格納（[Send／Register]ボタン活性制御はJavaScript側で行う）
        Dim wk As Boolean = businessLogic.IsAllJobStartByChip(jobDtlId, specifyDlrCdFlgs("TRANSACTION_EXIST"))
        If wk Then
            '全Job開始時は [1] をセット(JavaScriptで [true] 判定になる為)
            isAllJobStart = IsAllJobStartTrue
        Else
            '開始していないJobが存在する場合時は [0] をセット(JavaScriptで [false] 判定になる為)
            isAllJobStart = IsAllJobStartFalse
        End If

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
    '2014/12/10 [JobDispatch完成検査入力制御開発]対応　End

    ''' <summary>
    ''' 追加作業ボタン押下(AdditionalJob)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub AdditionalJobButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonAdditionalJobWork.Click

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim intAprovalStatus As Integer = AprovalStatusAproveWorking        '作業ステータス
        Dim strAdviceContent As String = String.Empty                       'アドバイスコメント

        'パラメータの取得
        GetParam()

        '完成検査結果データの取得
        Dim dtInspecItem As SC3180204RegistInfoDataTable = New SC3180204RegistInfoDataTable
        Dim dtMaintenance As SC3180204RegistInfoDataTable = New SC3180204RegistInfoDataTable
        GetRegistInfo(intAprovalStatus, dtInspecItem, dtMaintenance)

        'アドバイスコメントの取得
        strAdviceContent = Server.HtmlDecode(Request.Form("TechnicianAdvice"))

        '送信先ユーザーの取得
        Dim strSendUser As String
        strSendUser = Request.Form("SendUser").ToString

        '【***完成検査_排他制御***】 start
        '事前排他チェックを実施
        If False = ExclusionChk(dtInspecItem, dtMaintenance, strAdviceContent) Then
            Exit Sub
        End If
        '【***完成検査_排他制御***】 end

        '完成検査結果データ登録
        blnResult = businessLogic.AdditionalJobLogic(dealerCD, _
                                                     branchCD, _
                                                     roNum, _
                                                     jobDtlId, _
                                                     nowJobDtl, _
                                                     serviceID, _
                                                     stallId, _
                                                     strAdviceContent, _
                                                     dtInspecItem, _
                                                     dtMaintenance, _
                                                     account, _
                                                     ApplicationId, _
                                                     strSendUser, _
                                                     vin)

        '画面遷移
        If True = blnResult Then

            ''Logger.Info(String.Format(CultureInfo.CurrentCulture _
            ''          , "SetValue {0}:[{1}] {2}:[{3}] {4}:[{5}] {6}:[{7}]" _
            ''          , SessionRO _
            ''          , roNum _
            ''          , SessionVINNO _
            ''          , vin _
            ''          , SessionJobDtlId _
            ''          , jobDtlId _
            ''          , SessionViewMode _
            ''          , viewMode))
            '
            ''追加作業入力画面に遷移する
            'Me.SetValue(ScreenPos.Next, SessionRO, roNum)
            'Me.SetValue(ScreenPos.Next, SessionVINNO, vin)
            'Me.SetValue(ScreenPos.Next, SessionJobDtlId, jobDtlId)
            'Me.SetValue(ScreenPos.Next, SessionViewMode, viewMode)
            'Me.SetValue(ScreenPos.Next, SessionSAChipID, saChipID)
            'Me.SetValue(ScreenPos.Next, SessionBASREZID, basrezid)
            'Me.SetValue(ScreenPos.Next, SessionSeqNo, seqNo)
            '
            ''Logger.Info(String.Format(CultureInfo.CurrentCulture _
            ''          , "{0}.{1} RedirectNextScreen [{2}]" _
            ''          , Me.GetType.ToString _
            ''          , System.Reflection.MethodBase.GetCurrentMethod.Name _
            ''          , "SC3170203"))
            '
            'Me.RedirectNextScreen("SC3170203")

            Dim staffInfo As StaffContext = StaffContext.Current

            Using biz As New SC3180204BusinessLogic

                'DMS情報取得
                Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

                'DMS情報のチェック
                If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                    '取得できた場合
                    '画面間パラメータを設定
                    '表示番号
                    Me.SetValue(ScreenPos.Next, SessionDispNum, SessionDataDispNumAddInput)

                    'DMS販売店コード
                    Me.SetValue(ScreenPos.Next, SessionParam01, dtDmsCodeMapDataTable(0).CODE1)

                    'DMS店舗コード
                    Me.SetValue(ScreenPos.Next, SessionParam02, dtDmsCodeMapDataTable(0).CODE2)

                    'アカウント
                    Me.SetValue(ScreenPos.Next, SessionParam03, dtDmsCodeMapDataTable(0).ACCOUNT)

                    '来店実績連番
                    Me.SetValue(ScreenPos.Next, SessionParam04, saChipID)

                    'DMS予約ID
                    Me.SetValue(ScreenPos.Next, SessionParam05, basrezid)

                    'RO番号
                    Me.SetValue(ScreenPos.Next, SessionParam06, roNum)

                    'RO作業連番
                    Me.SetValue(ScreenPos.Next, SessionParam07, AdditionalWorkSendValue)

                    'VIN
                    Me.SetValue(ScreenPos.Next, SessionParam08, vin)

                    '編集モード
                    Me.SetValue(ScreenPos.Next, SessionParam09, viewMode)

                    '作業内容ID
                    Me.SetValue(ScreenPos.Next, SessionParam10, jobDtlId)

                    '追加作業画面(枠)に遷移する
                    Me.RedirectNextScreen(ProgramIdOtherLinkage)

                Else
                    '取得できなかった場合
                    'エラー
                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                             , "{0}.{1} ERROR " _
                                             , Me.GetType.ToString _
                                             , System.Reflection.MethodBase.GetCurrentMethod.Name))

                End If

            End Using



        Else
            '処理失敗
            '書き込みエラーメッセージの取得
            ErrorMessage.Text = "Error"
            ErrorFlg.Value = ErrorFlgError
            ErrorMessage.Text = WebWordUtility.GetWord(MsgID.id37)
            'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 start
            hdnErrorMsg.Value = ErrorMessage.Text
            'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 end
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 入力内容一時登録ボタン押下(Save)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SaveButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonSaveWork.Click

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim intAprovalStatus As Integer = AprovalStatusAproveWorking        '作業ステータス
        Dim strAdviceContent As String = String.Empty                                 'アドバイスコメント

        'パラメータの取得
        GetParam()

        '完成検査結果データの取得
        Dim dtInspecItem As SC3180204RegistInfoDataTable = New SC3180204RegistInfoDataTable
        Dim dtMaintenance As SC3180204RegistInfoDataTable = New SC3180204RegistInfoDataTable
        GetRegistInfo(intAprovalStatus, dtInspecItem, dtMaintenance)

        'アドバイスコメントの取得
        strAdviceContent = Server.HtmlDecode(Request.Form("TechnicianAdvice"))

        '送信先ユーザーの取得
        Dim strSendUser As String
        strSendUser = Request.Form("SendUser").ToString


        '【***完成検査_排他制御***】 start
        '事前排他チェックを実施
        If False = ExclusionChk(dtInspecItem, dtMaintenance, strAdviceContent) Then
            Exit Sub
        End If
        '【***完成検査_排他制御***】 end

        '完成検査結果データ登録
        blnResult = businessLogic.SaveLogic(dealerCD, _
                                            branchCD, _
                                            roNum, _
                                            jobDtlId, _
                                            nowJobDtl, _
                                            serviceID, _
                                            stallId, _
                                            strAdviceContent, _
                                            dtInspecItem, _
                                            dtMaintenance, _
                                            account, _
                                            ApplicationId, _
                                            strSendUser, _
                                            vin)

        '画面遷移
        If True = blnResult Then
            'ページの初期表示
            InitScreen()

        Else
            '処理失敗
            '失敗ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} Error" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '書き込みエラーメッセージの取得
            ErrorMessage.Text = "Error"
            ErrorFlg.Value = ErrorFlgError
            ErrorMessage.Text = WebWordUtility.GetWord(MsgID.id37)
            'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 start
            hdnErrorMsg.Value = ErrorMessage.Text
            'TMT2販社 BTS260 更新エラー処理修正 2015/03/31 end
        End If




        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 完成検査入力完了ボタン押下(Register)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub RegisterButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonRegisterWork.Click

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim intAprovalStatus As Integer = AprovelStatusNotAprove         '作業ステータス
        Dim strAdviceContent As String
        Dim updateTime As Date = DateTimeFunc.Now(dealerCD)

        'パラメータの取得
        GetParam()

        '完成検査結果データの取得
        Dim dtInspecItem As SC3180204RegistInfoDataTable = New SC3180204RegistInfoDataTable
        Dim dtMaintenance As SC3180204RegistInfoDataTable = New SC3180204RegistInfoDataTable
        GetRegistInfo(intAprovalStatus, dtInspecItem, dtMaintenance)

        'ユーザ情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'アドバイスコメントの取得
        strAdviceContent = Server.HtmlDecode(Request.Form("TechnicianAdvice"))

        '送信先ユーザーの取得
        Dim strSendUser As String
        strSendUser = Request.Form("SendUser").ToString

        '共通関数実行結果戻り値格納用変数
        Dim rtnGlobalResult As Long = ActionResult.Success
        '2014/07/17　最終チップ判定を戻り値に追加
        Dim blnLastChipFlg As Boolean = False

        '【***完成検査_排他制御***】 start
        '事前排他チェックを実施
        If False = ExclusionChk(dtInspecItem, dtMaintenance, strAdviceContent) Then
            Exit Sub
        End If
        '【***完成検査_排他制御***】 end

        '完成検査結果データ登録
        blnResult = businessLogic.RegisterLogic(dealerCD, _
                                                branchCD, _
                                                roNum, _
                                                jobDtlId, _
                                                viewMode, _
                                                vin, _
                                                saChipID, _
                                                basrezid, _
                                                seqNo, _
                                                nowJobDtl, _
                                                serviceID, _
                                                stallId, _
                                                strAdviceContent,
                                                dtInspecItem, _
                                                dtMaintenance, _
                                                account, _
                                                ApplicationId, _
                                                fromPreviewFlg, _
                                                CInt(staffInfo.OpeCD), _
                                                saAccountId, _
                                                strSendUser,
                                                rtnGlobalResult, _
                                                blnLastChipFlg, _
                                                mergeDataTable)

        ''画面遷移
        'If True = blnResult Then

        'エラーが発生していない場合、通知&PUSH処理を実行し、画面遷移する
        ' 2015/5/1 強制納車対応 警告表示 start
        If blnResult And _
           arySuccessList.Contains(rtnGlobalResult) Then

            blnResult = businessLogic.NoticeAfterRegisterLogic(dealerCD, _
                                                    branchCD, _
                                                    roNum, _
                                                    jobDtlId, _
                                                    viewMode, _
                                                    vin, _
                                                    saChipID, _
                                                    basrezid, _
                                                    seqNo, _
                                                    nowJobDtl, _
                                                    serviceID, _
                                                    stallId, _
                                                    strAdviceContent,
                                                    dtInspecItem, _
                                                    dtMaintenance, _
                                                    account, _
                                                    ApplicationId, _
                                                    fromPreviewFlg, _
                                                    CInt(staffInfo.OpeCD), _
                                                    saAccountId, _
                                                    strSendUser, _
                                                    blnLastChipFlg)

            ' 2019/1/25 ISSUE_0167対応 ADD start
            If blnResult = False Then
                '書き込みエラーメッセージの取得
                ErrorMessage.Text = "Error"
                ErrorFlg.Value = ErrorFlgError
                ErrorMessage.Text = WebWordUtility.GetWord(MsgID.id37)
                Me.hdnErrorMsg.Value = ErrorMessage.Text

                Exit Sub
            End If
            ' 2019/1/25 ISSUE_0167対応 ADD end

            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '          , "{0}.{1} RedirectPrevScreen" _
            '          , Me.GetType.ToString _
            '          , System.Reflection.MethodBase.GetCurrentMethod.Name))

            '正常終了のみ自動遷移する
            If rtnGlobalResult = ActionResult.Success Then

                ''TCメインから来た場合と、チェックシートプレビューから来た場合で遷移先が異なる(元の画面に戻る)
                Me.RedirectPrevScreen()
            Else

                '基幹連携で警告が発生、メッセージを設定する
                hdnWarningMsg.Value = WebWordUtility.GetWord(MsgID.id54)
            End If
            ' 2015/5/1 強制納車対応 警告表示 end
        Else
            '処理失敗

            '失敗ログ
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} Error [rtnGlobalResult={2}]" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , rtnGlobalResult.ToString.ToString))

            '共通関数内でのエラーか、エラーの種類を判断する
            If arySuccessList.Contains(rtnGlobalResult) Then

                '書き込みエラーメッセージの取得
                ErrorMessage.Text = "Error"
                ErrorFlg.Value = ErrorFlgError
                ErrorMessage.Text = WebWordUtility.GetWord(MsgID.id37)
                Me.hdnErrorMsg.Value = ErrorMessage.Text

            Else

                '共通関数内エラー
                ErrorMessage.Text = "Error"
                ErrorFlg.Value = ErrorFlgError
                ErrorMessage.Text = "Chip could not be finished."    'チップ停止失敗メッセージ(今は固定値をセット)
                Me.hdnErrorMsg.Value = ErrorMessage.Text
            End If

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 完成検査入力完了ボタン押下(Send)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SendButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonSendWork.Click

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim intAprovalStatus As Integer = AprovalStatusWaitingRecognition         '作業ステータス
        Dim strAdviceContent As String
        Dim updateTime As Date = DateTimeFunc.Now(dealerCD)
        Dim strSendUser As String
        'ユーザ情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        '完成検査結果データの取得
        Dim dtInspecItem As SC3180204RegistInfoDataTable = New SC3180204RegistInfoDataTable
        Dim dtMaintenance As SC3180204RegistInfoDataTable = New SC3180204RegistInfoDataTable
        GetRegistInfo(intAprovalStatus, dtInspecItem, dtMaintenance)

        'アドバイスコメントの取得
        strAdviceContent = Server.HtmlDecode(Request.Form("TechnicianAdvice"))

        '送信先ユーザーの取得
        strSendUser = Request.Form("SendUser").ToString

        '共通関数実行結果戻り値格納用変数
        Dim rtnGlobalResult As Long = ActionResult.Success

        '【***完成検査_排他制御***】 start
        '事前排他チェックを実施
        If False = ExclusionChk(dtInspecItem, dtMaintenance, strAdviceContent) Then
            Exit Sub
        End If
        '【***完成検査_排他制御***】 end

        '完成検査結果データ登録
        blnResult = businessLogic.SendLogic(dealerCD, _
                                            branchCD, _
                                            roNum, _
                                            jobDtlId, _
                                            viewMode, _
                                            vin, _
                                            saChipID, _
                                            basrezid, _
                                            seqNo, _
                                            nowJobDtl, _
                                            serviceID, _
                                            stallId, _
                                            strAdviceContent, _
                                            dtInspecItem, _
                                            dtMaintenance, _
                                            account, _
                                            ApplicationId, _
                                            strSendUser, _
                                            CInt(staffInfo.OpeCD), _
                                            strSendUser,
                                            rtnGlobalResult, _
                                            mergeDataTable)

        ''画面遷移
        'If True = blnResult Then

        'エラーが発生していない場合、通知&PUSH処理を実行し、画面遷移する
        ' 2015/5/1 強制納車対応 警告表示 start
        If blnResult And _
           arySuccessList.Contains(rtnGlobalResult) Then

            blnResult = businessLogic.NoticeAfterSendLogic(dealerCD, _
                                    branchCD, _
                                    roNum, _
                                    jobDtlId, _
                                    viewMode, _
                                    vin, _
                                    saChipID, _
                                    basrezid, _
                                    seqNo, _
                                    nowJobDtl, _
                                    serviceID, _
                                    stallId, _
                                    strAdviceContent, _
                                    dtInspecItem, _
                                    dtMaintenance, _
                                    account, _
                                    ApplicationId, _
                                    strSendUser, _
                                    CInt(staffInfo.OpeCD), _
                                    strSendUser)

            ' 2019/1/25 ISSUE_0167対応 ADD start
            If blnResult = False Then
                '書き込みエラーメッセージの取得
                ErrorMessage.Text = "Error"
                ErrorFlg.Value = ErrorFlgError
                ErrorMessage.Text = WebWordUtility.GetWord(MsgID.id37)
                Me.hdnErrorMsg.Value = ErrorMessage.Text

                Exit Sub
            End If
            ' 2019/1/25 ISSUE_0167対応 ADD end

            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '          , "{0}.{1} RedirectPrevScreen " _
            '          , Me.GetType.ToString _
            '          , System.Reflection.MethodBase.GetCurrentMethod.Name))
            '正常終了のみ自動遷移する
            If rtnGlobalResult = ActionResult.Success Then

                ''FMメインから来る場合と、通知履歴から来た場合で遷移先が異なる(元の画面に戻る)
                Me.RedirectPrevScreen()
            Else

                '基幹連携で警告が発生、メッセージを設定する
                hdnWarningMsg.Value = WebWordUtility.GetWord(MsgID.id54)
            End If
            ' 2015/5/1 強制納車対応 警告表示 end
        Else
            '処理失敗

            '失敗ログ
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} Error [rtnGlobalResult={2}]" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , rtnGlobalResult.ToString.ToString))

            '共通関数内でのエラーか、エラーの種類を判断する
            If arySuccessList.Contains(rtnGlobalResult) Then

                '書き込みエラーメッセージの取得
                ErrorMessage.Text = "Error"
                ErrorFlg.Value = ErrorFlgError
                ErrorMessage.Text = WebWordUtility.GetWord(MsgID.id37)
                Me.hdnErrorMsg.Value = ErrorMessage.Text

            Else

                '共通関数内エラー
                ErrorMessage.Text = "Error"
                ErrorFlg.Value = ErrorFlgError
                ErrorMessage.Text = "Chip could not be finished."    'チップ停止失敗メッセージ(今は固定値をセット)
                Me.hdnErrorMsg.Value = ErrorMessage.Text
            End If

        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 入力データクリアボタン(Clear)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ClearButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonClearWork.Click

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'パラメータの取得
        GetParam()

        'ページの初期表示
        InitScreen()

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' スワイプ用ボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SwipeButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonSwipe.Click

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'パラメータの取得
        GetParam()

        '終了ログ
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '           , "{0}.{1} END" _
        '           , Me.GetType.ToString _
        '           , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '          , "SetValue {0}:[{1}] {2}:[{3}] {4}:[{5}] {6}:[{7}]" _
        '          , SessionRO _
        '          , roNum _
        '          , SessionVINNO _
        '          , vin _
        '          , SessionJobDtlId _
        '          , jobDtlId _
        '          , SessionViewMode _
        '          , viewMode))

        '画面遷移
        'TCメイン画面に遷移する
        Me.SetValue(ScreenPos.Next, SessionRO, roNum)
        Me.SetValue(ScreenPos.Next, SessionVINNO, vin)
        Me.SetValue(ScreenPos.Next, SessionJobDtlId, jobDtlId)
        Me.SetValue(ScreenPos.Next, SessionViewMode, viewMode)
        Me.SetValue(ScreenPos.Next, SessionSAChipID, saChipID)
        Me.SetValue(ScreenPos.Next, SessionBASREZID, basrezid)
        Me.SetValue(ScreenPos.Next, SessionSeqNo, seqNo)

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '          , "{0}.{1} RedirectNextScreen [{2}] " _
        '          , Me.GetType.ToString _
        '          , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '          , "SC3150101"))

        Me.RedirectNextScreen("SC3150101")
    End Sub
#End Region

#Region "その他"
    ''' <summary>
    ''' パラメータの取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetParam()

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ユーザ情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'Session情報取得
        If staffInfo IsNot Nothing Then
            '販売店コード
            dealerCD = staffInfo.DlrCD.Trim()
            '店舗コード
            branchCD = staffInfo.BrnCD.Trim()
            '担当者
            account = staffInfo.Account.Trim()
        End If

        'R/O番号(R_O)
        If Me.ContainsKey(ScreenPos.Current, SessionRO) Then
            roNum = CType(Me.GetValue(ScreenPos.Current, SessionRO, False), String).Trim()
        End If
        '車両識別番号(VIN)
        If Me.ContainsKey(ScreenPos.Current, SessionVINNO) Then
            vin = CType(Me.GetValue(ScreenPos.Current, SessionVINNO, False), String).Trim()
        End If
        '作業内容ID(JOB_DTL_ID)
        If Me.ContainsKey(ScreenPos.Current, SessionJobDtlId) Then
            jobDtlId = CType(Me.GetValue(ScreenPos.Current, SessionJobDtlId, False), String).Trim()
        End If
        'ビューモード(ViewMode)
        If Me.ContainsKey(ScreenPos.Current, SessionViewMode) Then
            viewMode = CType(Me.GetValue(ScreenPos.Current, SessionViewMode, False), String).Trim()
        End If
        'Redrawフラグ(Redraw)
        If Me.ContainsKey(ScreenPos.Current, SessionRedraw) Then
            redraw = CType(Me.GetValue(ScreenPos.Current, SessionRedraw, False), String).Trim()
        End If

        '来店者実績連番
        If Me.ContainsKey(ScreenPos.Current, SessionSAChipID) = True Then
            saChipID = DirectCast(GetValue(ScreenPos.Current, SessionSAChipID, False), String)
        End If
        'DMS予約ID
        If Me.ContainsKey(ScreenPos.Current, SessionBASREZID) = True Then
            basrezid = DirectCast(GetValue(ScreenPos.Current, SessionBASREZID, False), String)
        End If
        'RO_JOB_SEQ(親のRO_JOB_SEQ = 0)
        If Me.ContainsKey(ScreenPos.Current, SessionSeqNo) = True Then
            seqNo = DirectCast(GetValue(ScreenPos.Current, SessionSeqNo, False), String)
        End If

        'Redrawフラグ初期化
        Me.SetValue(ScreenPos.Next, SessionRedraw, "")

        'ビューモード(ViewMode):Saveによる再描画の考慮
        Dim nowReferer As String = Request.ServerVariables("HTTP_REFERER")

        Dim myReferer As String = String.Empty
        If Me.ContainsKey(ScreenPos.Current, SessionReferer) Then
            'Redirect.Refererあり(自画面からの遷移)
            myReferer = CType(Me.GetValue(ScreenPos.Current, SessionReferer, False), String).Trim()
            If 0 <= nowReferer.ToUpper.IndexOf(ApplicationId) Then
                '自画面からの遷移(Save)
            Else
                '他画面からの遷移(他画面遷移の場合は、基盤の遷移元画面ID取得メソッドを使用する)
                If nowReferer <> myReferer Then
                    Me.SetValue(ScreenPos.Current, SessionReferer, Me.GetPrevScreenIdLocal())
                End If
            End If
        Else
            'Redirect.Refererなし(自画面以外からの遷移)
            Me.SetValue(ScreenPos.Current, SessionReferer, Me.GetPrevScreenIdLocal())
        End If

        '判定のため遷移元取得
        FromTCMain.Value = ""
        fromTCMainFlg = False
        fromPreviewFlg = False
        myReferer = CType(Me.GetValue(ScreenPos.Current, SessionReferer, False), String).Trim()
        If 0 <= myReferer.ToUpper.IndexOf(ProgramIdTcMain) Then
            'TCメインからの遷移(自画面の再描画も考慮して)
            FromTCMain.Value = FromTCMainPage
            fromTCMainFlg = True
        ElseIf 0 <= myReferer.ToUpper.IndexOf("SC3180202") Then
            fromPreviewFlg = True
        Else

            '遷移元画面が上記以外(基盤より取得できなかった場合)、ログインユーザの権限で遷移元画面フラグを立てる
            If staffInfo.OpeCD = Operation.SA Or _
               staffInfo.OpeCD = Operation.SM Then

                fromPreviewFlg = True

            ElseIf staffInfo.OpeCD = Operation.TEC Or _
                   staffInfo.OpeCD = Operation.CHT Then

                FromTCMain.Value = FromTCMainPage
                fromTCMainFlg = True

            End If

        End If

        If jobDtlId = String.Empty Then
            jobDtlId = "0"
            fromPreviewFlg = True
        End If

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '          , "SESSION  RO_NUM [{0}]:VIN [{1}]: JOB_DTL_ID[{2}]: VIEW_MODE[{3}]: SAChipID[{4}]: BASREZID[{5}]: SEQ_NO[{6}]" _
        '          , roNum.ToString _
        '          , vin.ToString _
        '          , jobDtlId.ToString _
        '          , viewMode.ToString _
        '          , saChipID.ToString _
        '          , basrezid.ToString _
        '          , seqNo.ToString))

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "FromTCMain [{0}]" _
        '            , FromTCMain.Value.ToString))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 基盤より遷移元画面IDを取得(Nothingの場合は""を返す)
    ''' </summary>
    ''' <returns>遷移元画面 ／ ""(Empty)</returns>
    ''' <remarks></remarks>
    Private Function GetPrevScreenIdLocal() As String

        Dim rtn As String = ""

        Try
            Dim preScreenId As String = Me.GetPrevScreenId()

            If String.IsNullOrWhiteSpace(preScreenId) Then

                ' 基盤より遷移元画面ID取得失敗
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} [GetPrevScreenId returned Null.]" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Else
                rtn = preScreenId
            End If

        Catch ex As Exception

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} Exception happend. {2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , ex.ToString))

        End Try

        Return rtn

    End Function

    ''' <summary>
    ''' データテーブルの再編
    ''' </summary>
    ''' <param name="dtInspecCode">データテーブル</param>
    ''' <remarks></remarks>
    Private Sub EditInspecCode(ByRef dtInspecCode As SC3180204InspectCodeDataTable)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ユーザ情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '検査項目
        Dim strInspecItemMode As String = String.Empty
        Dim strInspecItemTextInputMode As String = String.Empty
        Dim strInspecItemViewStyleColor As String = String.Empty
        Dim strInspecItemInputStyle As String = String.Empty
        Dim strInspecItemInputStyle2 As String = String.Empty
        Dim strInspecItemRegistMode As String = String.Empty
        Dim strInspecItemViewControl As String = String.Empty
        '検査項目名
        Dim strInspecItemName As String = String.Empty
        Dim strNextInspecItemName As String = String.Empty
        Dim strInspecItemNameViewStyle As String = String.Empty
        'テキストボックス(Before,After)
        Dim blnInspecItemsText As Boolean = False
        Dim strInspecItemsTextViewStyle As String = String.Empty
        Dim strInspecItemsTextBefore As String = String.Empty
        Dim strInspecItemsTextAfter As String = String.Empty
        '択一チェック項目(Good,Inspect,Replace,Fix,Cleaning,Swap)
        Dim intInspecItemsStatusCount As Integer = 0
        Dim strInspecItemsStatusViewStyleGood As String = String.Empty
        Dim strInspecItemsStatusViewStyleInspect As String = String.Empty
        Dim strInspecItemsStatusViewStyleReplace As String = String.Empty
        Dim strInspecItemsStatusViewStyleFix As String = String.Empty
        Dim strInspecItemsStatusViewStyleCleaning As String = String.Empty
        Dim strInspecItemsStatusViewStyleSwap As String = String.Empty
        Dim strInspecItemsStatusViewPosGood As String = String.Empty
        Dim strInspecItemsStatusViewPosInspect As String = String.Empty
        Dim strInspecItemsStatusViewPosReplace As String = String.Empty
        Dim strInspecItemsStatusViewPosFix As String = String.Empty
        Dim strInspecItemsStatusViewPosCleaning As String = String.Empty
        Dim strInspecItemsStatusViewPosSwap As String = String.Empty
        Dim strInspecItemsStatusSelectGood As String = String.Empty
        Dim strInspecItemsStatusSelectInspect As String = String.Empty
        Dim strInspecItemsStatusSelectReplace As String = String.Empty
        Dim strInspecItemsStatusSelectFix As String = String.Empty
        Dim strInspecItemsStatusSelectCleaning As String = String.Empty
        Dim strInspecItemsStatusSelectSwap As String = String.Empty
        Dim strInspecItemsStatusColorGood As String = String.Empty
        Dim strInspecItemsStatusColorInspect As String = String.Empty
        Dim strInspecItemsStatusColorReplace As String = String.Empty
        Dim strInspecItemsStatusColorFix As String = String.Empty
        Dim strInspecItemsStatusColorCleaning As String = String.Empty
        Dim strInspecItemsStatusColorSwap As String = String.Empty
        Dim strInspecItemsCheck As String = String.Empty

        Dim strInspecItemsStatusViewStyle_No_Check As String = ""
        Dim strInspecItemsStatusViewPos_No_Check As String = ""
        Dim strInspecItemsStatusSelect_No_Check As String = ""
        Dim strInspecItemsStatusColor_No_Check As String = ""

        '複数選択リスト(Replaced,Fixed,Cleaned,Swapped)
        Dim strInspecItemsSelectViewStyleReplaced As String = String.Empty
        Dim strInspecItemsSelectViewStyleFixed As String = String.Empty
        Dim strInspecItemsSelectViewStyleCleaned As String = String.Empty
        Dim strInspecItemsSelectViewStyleSwapped As String = String.Empty

        'DB項目の追加
        ''検査項目
        With dtInspecCode.Columns
            .Add("InspecItemMode", Type.GetType("System.String"))
            .Add("InspecItemTextInputMode", Type.GetType("System.String"))
            .Add("InspecItemViewStyle_Color", Type.GetType("System.String"))
            .Add("InspecItemInputStyle", Type.GetType("System.String"))
            .Add("InspecItemInputStyle2", Type.GetType("System.String"))
            .Add("InspecItemRegistMode", Type.GetType("System.String"))
            .Add("InspecItemViewControl", Type.GetType("System.String"))
            ''検査項目名
            .Add("InspecItemNameViewStyle", Type.GetType("System.String"))
            ''テキストボックス(Before,After)
            .Add("InspecItemsTextViewStyle", Type.GetType("System.String"))
            .Add("InspecItemsTextBefore", Type.GetType("System.String"))
            .Add("InspecItemsTextAfter", Type.GetType("System.String"))
            .Add("HiddenAllData", Type.GetType("System.String"))
            ''択一チェック項目(Good,Inspect,Replace,Fix,Cleaning,Swap)
            .Add("InspecItemsStatusViewStyle_Good", Type.GetType("System.String"))
            .Add("InspecItemsStatusViewStyle_Inspect", Type.GetType("System.String"))
            .Add("InspecItemsStatusViewStyle_Replace", Type.GetType("System.String"))
            .Add("InspecItemsStatusViewStyle_Fix", Type.GetType("System.String"))
            .Add("InspecItemsStatusViewStyle_Cleaning", Type.GetType("System.String"))
            .Add("InspecItemsStatusViewStyle_Swap", Type.GetType("System.String"))
            .Add("InspecItemsStatusViewPos_Good", Type.GetType("System.String"))
            .Add("InspecItemsStatusViewPos_Inspect", Type.GetType("System.String"))
            .Add("InspecItemsStatusViewPos_Replace", Type.GetType("System.String"))
            .Add("InspecItemsStatusViewPos_Fix", Type.GetType("System.String"))
            .Add("InspecItemsStatusViewPos_Cleaning", Type.GetType("System.String"))
            .Add("InspecItemsStatusViewPos_Swap", Type.GetType("System.String"))
            .Add("InspecItemsStatusSelect_Good", Type.GetType("System.String"))
            .Add("InspecItemsStatusSelect_Inspect", Type.GetType("System.String"))
            .Add("InspecItemsStatusSelect_Replace", Type.GetType("System.String"))
            .Add("InspecItemsStatusSelect_Fix", Type.GetType("System.String"))
            .Add("InspecItemsStatusSelect_Cleaning", Type.GetType("System.String"))
            .Add("InspecItemsStatusSelect_Swap", Type.GetType("System.String"))
            .Add("InspecItemsStatusColor_Good", Type.GetType("System.String"))
            .Add("InspecItemsStatusColor_Inspect", Type.GetType("System.String"))
            .Add("InspecItemsStatusColor_Replace", Type.GetType("System.String"))
            .Add("InspecItemsStatusColor_Fix", Type.GetType("System.String"))
            .Add("InspecItemsStatusColor_Cleaning", Type.GetType("System.String"))
            .Add("InspecItemsStatusColor_Swap", Type.GetType("System.String"))
            .Add("InspecItemsCheck", Type.GetType("System.String"))

            dtInspecCode.Columns.Add("InspecItemsStatusViewStyle_No_Check", Type.GetType("System.String"))
            dtInspecCode.Columns.Add("InspecItemsStatusViewPos_No_Check", Type.GetType("System.String"))
            dtInspecCode.Columns.Add("InspecItemsStatusSelect_No_Check", Type.GetType("System.String"))
            dtInspecCode.Columns.Add("InspecItemsStatusColor_No_Check", Type.GetType("System.String"))

            ''複数選択リスト(Replaced,Fixed,Cleaned,Swapped)
            .Add("InspecItemsSelectViewStyle_Replaced", Type.GetType("System.String"))
            .Add("InspecItemsSelectViewStyle_Fixed", Type.GetType("System.String"))
            .Add("InspecItemsSelectViewStyle_Cleaned", Type.GetType("System.String"))
            .Add("InspecItemsSelectViewStyle_Swapped", Type.GetType("System.String"))

            '行ロックバージョン
            .Add("TrnRowLockVersion", Type.GetType("System.String"))

            .Add("color", Type.GetType("System.String"))

            '2014/06/27 項目追加　Start
            .Add("InspecItemsSelect_Options", Type.GetType("System.String"))
            '2014/06/27 項目追加　End

            '【***完成検査_排他制御***】 start
            .Add("ServiceInRowLockVersion", Type.GetType("System.String"))
            '【***完成検査_排他制御***】 end
        End With

        Dim strInspecItemCD As String = String.Empty
        Dim intIdx As Integer = 0

        Dim lngRoStatusProc As Long = 0

        '2014/06/06 入力制御不具合　Start
        'lngRoStatusProc = businessLogic.RoStatusCheck(Integer.Parse(roStatus))
        '2014/06/06 入力制御不具合　End

        '検査項目設定
        For intIdx = 0 To dtInspecCode.Count - 1
            '2014/06/06 入力制御不具合　Start
            lngRoStatusProc = businessLogic.RoStatusCheck(Integer.Parse(dtInspecCode(intIdx).RO_STATUS.ToString))
            '2014/06/06 入力制御不具合　End

            '検査項目
            'strInspecItemMode = InspecItemModeNow
            'strInspecItemInputStyle = ""
            'strInspecItemViewStyleColor = "background:transparent;"
            'strInspecItemRegistMode = RegistModeRegist
            'strInspecItemViewControl = ""

            'If StallUseStatusWork > dtInspecCode(intIdx).STALL_USE_STATUS.ToString.Trim Then
            '    '未来
            '    strInspecItemMode = InspecItemModeFuture
            '    strInspecItemInputStyle = "disabled"
            '    strInspecItemViewStyleColor = "background:darkgray;"
            '    strInspecItemRegistMode = RegistModeUnregist
            'ElseIf StallUseStatusWork < dtInspecCode(intIdx).STALL_USE_STATUS.ToString.Trim Then
            '    '過去
            '    strInspecItemMode = InspecItemModePast
            '    strInspecItemViewStyleColor = "background:lightgrey;"
            'End If
            Dim fontColor As String = String.Empty
            SetStatusColor(strInspecItemMode, strInspecItemViewStyleColor, strInspecItemInputStyle, strInspecItemRegistMode, strInspecItemViewControl, intIdx, dtInspecCode, fontColor)

            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                , "Stall_Use_Status [{0}] : strInspecItemMode [{1}]" _
            '                , dtInspecCode(intIdx).STALL_USE_STATUS.ToString.Trim _
            '                , strInspecItemMode))

            'ステータス状態による編集可/不可の決定
            ' ''Dim lngRoStatusProc As Long = 0
            If True = fromTCMainFlg Then
                ' ''lngRoStatusProc = businessLogic.RoStatusCheck(Integer.Parse(roStatus))
                If lngRoStatusProc = RoStatusProcBeforeWork Then
                    '整備・点検実績データなし:編集可(変更なし)
                ElseIf lngRoStatusProc = RoStatusProcWorkToCompExaminationRequest Then
                    '最終チップ承認前(60:作業中、65:完成検査依頼中):編集可(変更なし)
                ElseIf lngRoStatusProc = RoStatusProcCompExaminationComplate Then
                    '納車より前(70:完成検査完了まで):編集不可
                    strInspecItemInputStyle = "disabled"
                    strInspecItemRegistMode = RegistModeUnregist
                ElseIf lngRoStatusProc = RoStatusProcDeliveryWaitToDeliveryWork Then
                    '納車より前(80:納車準備待ち、85:納車作業中):編集不可
                    strInspecItemInputStyle = "disabled"
                    strInspecItemRegistMode = RegistModeUnregist
                Else
                    '納車より前(90:納車済み以降):編集不可
                    strInspecItemInputStyle = "disabled"
                    strInspecItemRegistMode = RegistModeUnregist
                End If
            Else
                ' TCMain以外(チェックシートプレビュー)からの遷移
                If staffInfo.OpeCD = Operation.FM OrElse staffInfo.OpeCD = Operation.CT OrElse staffInfo.OpeCD = Operation.CHT Then
                    'FM,CT,ChT
                    ' ''lngRoStatusProc = businessLogic.RoStatusCheck(Integer.Parse(roStatus))
                    If lngRoStatusProc = RoStatusProcBeforeWork Then
                        '整備・点検実績データなし:表示しない
                        strInspecItemInputStyle = "disabled"
                        strInspecItemRegistMode = RegistModeUnregist
                        strInspecItemViewControl = "display: none;"
                    ElseIf lngRoStatusProc = RoStatusProcWorkToCompExaminationRequest Then
                        '最終チップ承認前(60:作業中、65:完成検査依頼中):編集可(変更なし)
                    ElseIf lngRoStatusProc = RoStatusProcCompExaminationComplate Then
                        '納車より前(70:完成検査完了まで):編集可(変更なし)
                    ElseIf lngRoStatusProc = RoStatusProcDeliveryWaitToDeliveryWork Then
                        '納車より前(80:納車準備待ち、85:納車作業中):編集不可
                        strInspecItemInputStyle = "disabled"
                        strInspecItemRegistMode = RegistModeUnregist
                    Else
                        '納車より前(90:納車済み以降):編集不可
                        strInspecItemInputStyle = "disabled"
                        strInspecItemRegistMode = RegistModeUnregist
                    End If
                Else
                    'SA,SM
                    ' ''lngRoStatusProc = businessLogic.RoStatusCheck(Integer.Parse(roStatus))
                    If lngRoStatusProc = RoStatusProcBeforeWork Then
                        '整備・点検実績データなし:表示しない
                        strInspecItemInputStyle = "disabled"
                        strInspecItemRegistMode = RegistModeUnregist
                        strInspecItemViewControl = "display: none;"
                    ElseIf lngRoStatusProc = RoStatusProcWorkToCompExaminationRequest Then
                        '最終チップ承認前(60:作業中、65:完成検査依頼中):編集可(変更なし)
                    ElseIf lngRoStatusProc = RoStatusProcCompExaminationComplate Then
                        '納車より前(70:完成検査完了まで):編集可(変更なし)
                    ElseIf lngRoStatusProc = RoStatusProcDeliveryWaitToDeliveryWork Then
                        '納車より前(80:納車準備待ち、85:納車作業中):編集可(変更なし)
                    Else
                        '納車より前(90:納車済み以降):編集不可
                        strInspecItemInputStyle = "disabled"
                        strInspecItemRegistMode = RegistModeUnregist
                    End If
                End If
            End If
            strInspecItemInputStyle2 = ""

            If strInspecItemInputStyle = "disabled" Then

                strInspecItemInputStyle2 = "disabled"

            ElseIf Not dtInspecCode(intIdx).IsINSPEC_RSLT_CDNull AndAlso _
                   dtInspecCode(intIdx).INSPEC_RSLT_CD.ToString = "7" Then

                strInspecItemInputStyle2 = "disabled"

            End If

            With dtInspecCode(intIdx)
                .Item("InspecItemMode") = strInspecItemMode
                .Item("InspecItemInputStyle") = strInspecItemInputStyle
                .Item("InspecItemInputStyle2") = strInspecItemInputStyle2
                .Item("InspecItemViewStyle_Color") = strInspecItemViewStyleColor
                .Item("InspecItemRegistMode") = strInspecItemRegistMode
                .Item("InspecItemViewControl") = strInspecItemViewControl

                .Item("color") = fontColor
            End With

            '検査項目名
            If 0 = intIdx Then
                strInspecItemName = dtInspecCode(intIdx).INSPEC_ITEM_NAME.ToString.Trim
            Else
                strNextInspecItemName = dtInspecCode(intIdx).INSPEC_ITEM_NAME.ToString.Trim
                If True = strInspecItemName.Equals(strNextInspecItemName) Then
                    strInspecItemNameViewStyle = "display: none;"
                Else
                    strInspecItemNameViewStyle = ""
                    strInspecItemName = strNextInspecItemName
                End If
            End If
            dtInspecCode(intIdx).Item("InspecItemNameViewStyle") = strInspecItemNameViewStyle

            'テキストボックス(Before,After)
            strInspecItemTextInputMode = TextInputModeUninput
            strInspecItemsTextViewStyle = "display: none;"
            blnInspecItemsText = False
            If DispTextPermView = dtInspecCode(intIdx).DISP_TEXT_PERM.ToString.Trim Then
                strInspecItemTextInputMode = TextInputModeInput
                strInspecItemsTextViewStyle = ""
                blnInspecItemsText = True
            End If
            dtInspecCode(intIdx).Item("InspecItemsTextViewStyle") = strInspecItemsTextViewStyle

            strInspecItemsTextBefore = beforeText
            If False = dtInspecCode(intIdx).IsRSLT_VAL_BEFORENull Then
                Dim valBefore As Decimal = CType(dtInspecCode(intIdx).RSLT_VAL_BEFORE, Decimal)

                If DefaultBeforeText <> valBefore Then
                    strInspecItemsTextBefore = valBefore.ToString(FormatRsltVal)
                End If
            End If

            strInspecItemsTextAfter = afterText
            If False = dtInspecCode(intIdx).IsRSLT_VAL_AFTERNull Then
                Dim valAfter As Decimal = CType(dtInspecCode(intIdx).RSLT_VAL_AFTER, Decimal)

                If DefaultAfterText <> valAfter Then
                    strInspecItemsTextAfter = valAfter.ToString(FormatRsltVal)
                End If
            End If

            With dtInspecCode(intIdx)
                .Item("InspecItemTextInputMode") = strInspecItemTextInputMode
                .Item("InspecItemsTextBefore") = strInspecItemsTextBefore
                .Item("InspecItemsTextAfter") = strInspecItemsTextAfter
            End With

            '択一チェック項目(Good,Inspect,Replace,Fix,Cleaning,Swap)
            Dim cleanFlg As Boolean = False
            intInspecItemsStatusCount = 0
            strInspecItemsStatusViewStyleGood = "display: none;"
            strInspecItemsStatusViewPosGood = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NO_PROBLEM.ToString.Trim Then
                strInspecItemsStatusViewStyleGood = ""
                strInspecItemsStatusViewPosGood = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                intInspecItemsStatusCount += 1
            End If
            strInspecItemsStatusViewStyleInspect = "display: none;"
            strInspecItemsStatusViewPosInspect = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NEED_INSPEC.ToString.Trim Then
                strInspecItemsStatusViewStyleInspect = ""
                strInspecItemsStatusViewPosInspect = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                intInspecItemsStatusCount += 1
            End If
            strInspecItemsStatusViewStyleReplace = "display: none;"
            strInspecItemsStatusViewPosReplace = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NEED_REPLACE.ToString.Trim Then
                strInspecItemsStatusViewStyleReplace = ""
                strInspecItemsStatusViewPosReplace = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                intInspecItemsStatusCount += 1
            End If
            strInspecItemsStatusViewStyleFix = "display: none;"
            strInspecItemsStatusViewPosFix = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NEED_FIX.ToString.Trim Then
                strInspecItemsStatusViewStyleFix = ""
                strInspecItemsStatusViewPosFix = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                intInspecItemsStatusCount += 1
            End If
            strInspecItemsStatusViewStyleCleaning = "display: none;"
            strInspecItemsStatusViewPosCleaning = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NEED_CLEAN.ToString.Trim Then
                If False = blnInspecItemsText OrElse 4 > intInspecItemsStatusCount Then
                    strInspecItemsStatusViewStyleCleaning = ""
                    strInspecItemsStatusViewPosCleaning = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                    cleanFlg = True
                    intInspecItemsStatusCount += 1
                End If
            End If
            strInspecItemsStatusViewStyleSwap = "display: none;"
            strInspecItemsStatusViewPosSwap = ""
            If "1" = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NEED_SWAP.ToString.Trim Then
                If False = blnInspecItemsText OrElse 4 > intInspecItemsStatusCount Then
                    strInspecItemsStatusViewStyleSwap = ""
                    strInspecItemsStatusViewPosSwap = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                    intInspecItemsStatusCount += 1
                End If
            End If


            strInspecItemsStatusViewStyle_No_Check = "display: none;"
            strInspecItemsStatusViewPos_No_Check = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NO_ACTION.ToString.Trim Then
                If False = blnInspecItemsText And 6 > intInspecItemsStatusCount Or 4 > intInspecItemsStatusCount Then
                    strInspecItemsStatusViewStyle_No_Check = ""
                    strInspecItemsStatusViewPos_No_Check = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                    intInspecItemsStatusCount += 1
                End If
            End If

            With dtInspecCode(intIdx)
                .Item("InspecItemsStatusViewStyle_Good") = strInspecItemsStatusViewStyleGood
                .Item("InspecItemsStatusViewStyle_Inspect") = strInspecItemsStatusViewStyleInspect
                .Item("InspecItemsStatusViewStyle_Replace") = strInspecItemsStatusViewStyleReplace
                .Item("InspecItemsStatusViewStyle_Fix") = strInspecItemsStatusViewStyleFix
                .Item("InspecItemsStatusViewStyle_Cleaning") = strInspecItemsStatusViewStyleCleaning
                .Item("InspecItemsStatusViewStyle_Swap") = strInspecItemsStatusViewStyleSwap
                .Item("InspecItemsStatusViewPos_Good") = strInspecItemsStatusViewPosGood
                .Item("InspecItemsStatusViewPos_Inspect") = strInspecItemsStatusViewPosInspect
                .Item("InspecItemsStatusViewPos_Replace") = strInspecItemsStatusViewPosReplace
                .Item("InspecItemsStatusViewPos_Fix") = strInspecItemsStatusViewPosFix
                .Item("InspecItemsStatusViewPos_Cleaning") = strInspecItemsStatusViewPosCleaning
                .Item("InspecItemsStatusViewPos_Swap") = strInspecItemsStatusViewPosSwap
                .Item("InspecItemsStatusViewStyle_No_Check") = strInspecItemsStatusViewStyle_No_Check
                .Item("InspecItemsStatusViewPos_No_Check") = strInspecItemsStatusViewPos_No_Check
            End With

            strInspecItemsCheck = CheckModeUncheck
            strInspecItemsStatusSelectGood = ""
            strInspecItemsStatusSelectInspect = ""
            strInspecItemsStatusSelectReplace = ""
            strInspecItemsStatusSelectFix = ""
            strInspecItemsStatusSelectCleaning = ""
            strInspecItemsStatusSelectSwap = ""
            strInspecItemsStatusColorGood = "blue"
            strInspecItemsStatusColorInspect = "blue"
            strInspecItemsStatusColorReplace = "blue"
            strInspecItemsStatusColorFix = "blue"
            strInspecItemsStatusColorCleaning = "blue"
            strInspecItemsStatusColorSwap = "blue"

            strInspecItemsStatusSelect_No_Check = ""
            strInspecItemsStatusColor_No_Check = "blue"

            If False = dtInspecCode(intIdx).IsNull("INSPEC_RSLT_CD") Then
                strInspecItemsCheck = dtInspecCode(intIdx).INSPEC_RSLT_CD.ToString.Trim
                If CheckModeNoProblem = strInspecItemsCheck Then
                    strInspecItemsStatusSelectGood = "checked"
                    strInspecItemsStatusColorGood = "green"
                ElseIf CheckModeNeedInspection = strInspecItemsCheck Then
                    strInspecItemsStatusSelectInspect = "checked"
                    strInspecItemsStatusColorInspect = "green"
                ElseIf CheckModeNeedReplace = strInspecItemsCheck Then
                    strInspecItemsStatusSelectReplace = "checked"
                    strInspecItemsStatusColorReplace = "green"
                ElseIf CheckModeNeedFixing = strInspecItemsCheck Then
                    strInspecItemsStatusSelectFix = "checked"
                    strInspecItemsStatusColorFix = "green"
                ElseIf CheckModeNeedCleaning = strInspecItemsCheck Then
                    strInspecItemsStatusSelectCleaning = "checked"
                    strInspecItemsStatusColorCleaning = "green"
                ElseIf CheckModeNeedSwapping = strInspecItemsCheck Then
                    strInspecItemsStatusSelectSwap = "checked"
                    strInspecItemsStatusColorSwap = "green"
                ElseIf CheckModeNoCheck = strInspecItemsCheck Then
                    strInspecItemsStatusSelect_No_Check = "checked"
                    strInspecItemsStatusColor_No_Check = "green"
                Else
                    strInspecItemsCheck = CheckModeUncheck
                End If
            End If

            With dtInspecCode(intIdx)
                .Item("InspecItemsCheck") = strInspecItemsCheck
                .Item("InspecItemsStatusSelect_Good") = strInspecItemsStatusSelectGood
                .Item("InspecItemsStatusSelect_Inspect") = strInspecItemsStatusSelectInspect
                .Item("InspecItemsStatusSelect_Replace") = strInspecItemsStatusSelectReplace
                .Item("InspecItemsStatusSelect_Fix") = strInspecItemsStatusSelectFix
                .Item("InspecItemsStatusSelect_Cleaning") = strInspecItemsStatusSelectCleaning
                .Item("InspecItemsStatusSelect_Swap") = strInspecItemsStatusSelectSwap
                .Item("InspecItemsStatusColor_Good") = strInspecItemsStatusColorGood
                .Item("InspecItemsStatusColor_Inspect") = strInspecItemsStatusColorInspect
                .Item("InspecItemsStatusColor_Replace") = strInspecItemsStatusColorReplace
                .Item("InspecItemsStatusColor_Fix") = strInspecItemsStatusColorFix
                .Item("InspecItemsStatusColor_Cleaning") = strInspecItemsStatusColorCleaning
                .Item("InspecItemsStatusColor_Swap") = strInspecItemsStatusColorSwap


                .Item("InspecItemsStatusSelect_No_Check") = strInspecItemsStatusSelect_No_Check
                .Item("InspecItemsStatusColor_No_Check") = strInspecItemsStatusColor_No_Check
            End With

            '2014/06/27 削除　Start
            ''複数選択リスト(Replaced,Fixed,Cleaned,Swapped)
            'strInspecItemsSelectViewStyleReplaced = ""
            'If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_REPLACE") Then
            '    If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_REPLACE.ToString.Trim Then
            '        strInspecItemsSelectViewStyleReplaced = "selected"
            '    End If
            'End If
            'strInspecItemsSelectViewStyleFixed = ""
            'If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_FIX") Then
            '    If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_FIX.ToString.Trim Then
            '        strInspecItemsSelectViewStyleFixed = "selected"
            '    End If
            'End If
            'strInspecItemsSelectViewStyleCleaned = ""
            'If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_CLEAN") Then
            '    If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_CLEAN.ToString.Trim Then
            '        strInspecItemsSelectViewStyleCleaned = "selected"
            '    End If
            'End If
            'strInspecItemsSelectViewStyleSwapped = ""
            'If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_SWAP") Then
            '    If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_SWAP.ToString.Trim Then
            '        strInspecItemsSelectViewStyleSwapped = "selected"
            '    End If
            'End If

            'With dtInspecCode(intIdx)
            '    .Item("InspecItemsSelectViewStyle_Replaced") = strInspecItemsSelectViewStyleReplaced
            '    .Item("InspecItemsSelectViewStyle_Fixed") = strInspecItemsSelectViewStyleFixed
            '    .Item("InspecItemsSelectViewStyle_Cleaned") = strInspecItemsSelectViewStyleCleaned
            '    .Item("InspecItemsSelectViewStyle_Swapped") = strInspecItemsSelectViewStyleSwapped
            'End With
            '2014/06/27 削除　End

            '2014/06/27 項目追加　Start
            Dim InspecItemsSelectOptions As String = GetInspecItemsSelect_Options(dtInspecCode(intIdx))
            dtInspecCode(intIdx).Item("InspecItemsSelect_Options") = InspecItemsSelectOptions
            '2014/06/27 項目追加　End

            'Send又はRegisterの切替用フラグ(空文字:Register/1:Send)
            If InspecItemModeNow = strInspecItemMode OrElse jobDtlId = dtInspecCode(intIdx).JOB_DTL_ID.ToString Then
                If InspectionNeedFlgRegister = dtInspecCode(intIdx).INSPECTION_NEED_FLG.ToString.Trim Then
                    'Send又はRegisterの切替用フラグ(空文字:Register/1:Send)
                    SendOrRegister.Value = SendOrRegisterFlgSend
                End If
            End If

            ''Logger.Info(String.Format(CultureInfo.CurrentCulture _
            ''              , "InspecItemMode [{0}] : INSPECTION_STATUS [{1}] : INSPECTION_NEED_FLG [{2}] : SendOrRegister [{3}]" _
            ''              , strInspecItemMode _
            ''              , dtInspecCode(intIdx).INSPECTION_STATUS.ToString.Trim _
            ''              , dtInspecCode(intIdx).INSPECTION_NEED_FLG.ToString.Trim _
            ''              , SendOrRegister.Value))

            '行ロックバージョンの取得
            If False = dtInspecCode(intIdx).IsNull("TRN_ROW_LOCK_VERSION") Then
                dtInspecCode(intIdx).Item("TrnRowLockVersion") = dtInspecCode(intIdx).TRN_ROW_LOCK_VERSION.ToString.Trim
                'Logger.Info(String.Format(CultureInfo.CurrentCulture _
                '            , "TRN_Lock_version [{0}]" _
                '            , dtInspecCode(intIdx).TRN_ROW_LOCK_VERSION.ToString.Trim))
            Else
                dtInspecCode(intIdx).Item("TrnRowLockVersion") = UnsetRowLockVer
                'Logger.Info(String.Format(CultureInfo.CurrentCulture _
                '            , "TRN_Lock_version [-1]"))
            End If

            '2014/08/06 変更チェック処理修正　START　↓↓↓
            Dim SelectorValue As New StringBuilder

            If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_REPLACE") Then
                If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_REPLACE.ToString.Trim Then
                    SelectorValue.Append("1")
                End If
            End If

            If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_FIX") Then
                If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_FIX.ToString.Trim Then
                    If SelectorValue.ToString <> "" Then
                        SelectorValue.Append(",")
                    End If
                    SelectorValue.Append("2")
                End If
            End If

            If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_CLEAN") Then
                If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_CLEAN.ToString.Trim Then
                    If SelectorValue.ToString <> "" Then
                        SelectorValue.Append(",")
                    End If
                    SelectorValue.Append("3")
                End If
            End If

            If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_SWAP") Then
                If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_SWAP.ToString.Trim Then
                    If SelectorValue.ToString <> "" Then
                        SelectorValue.Append(",")
                    End If
                    SelectorValue.Append("4")
                End If
            End If
            '2014/08/06 変更チェック処理修正　END　　↑↑↑

            '【***完成検査_排他制御***】 start
            '変更フラグ追加
            dtInspecCode(intIdx).Item("EDIT_FLAG") = dtInspecCode(intIdx).Item("EDIT_FLAG").ToString

            '排他チェック用行ロックバージョン追加
            If False = dtInspecCode(intIdx).IsNull("EXCLUSION_ROW_LOCK_VERSION") Then
                dtInspecCode(intIdx).Item("ServiceInRowLockVersion") = dtInspecCode(intIdx).Item("EXCLUSION_ROW_LOCK_VERSION").ToString
            Else
                dtInspecCode(intIdx).Item("ServiceInRowLockVersion") = UnsetRowLockVer
            End If
　
         　   

            dtInspecCode(intIdx).Item("HiddenAllData") = dtInspecCode(intIdx).Item("InspecItemRegistMode").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("InspecItemMode").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("InspecItemsCheck").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("InspecItemTextInputMode").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("JOB_DTL_ID").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("JOB_INSTRUCT_ID").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("JOB_INSTRUCT_SEQ").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("INSPEC_ITEM_CD").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("STALL_USE_ID").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("TrnRowLockVersion").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("InspecItemsCheck").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("InspecItemsTextBefore").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("InspecItemsTextAfter").ToString & "|" & _
                                                        SelectorValue.ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("SVC_CD").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("EDIT_FLAG").ToString & "|" & _
                                                        dtInspecCode(intIdx).Item("EXCLUSION_ROW_LOCK_VERSION").ToString & "|"
		    '【***完成検査_排他制御***】 end 
		    		    
        Next intIdx

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#Region "2014/06/27 関数追加"

    ''' <summary>
    ''' 作業実績項目リストをソートして取得する
    ''' option タグ複数個を返却する
    ''' </summary>
    ''' <param name="dtInspecCodeRow">データ行</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetInspecItemsSelect_Options(ByVal dtInspecCodeRow As SC3180204InspectCodeRow) As String
        Dim Result As String = ""

        '作業実績項目をソートするためのDictionary　｛表示順,項目（HTML）｝
        '※キーは重複を避けるため表示順＊10＋｛1～4｝としている
        Dim dicOption As New Dictionary(Of Integer, String)

        '作業実績項目を表示しない
        Const NoDisplayOption As String = "0"

        '作業実績項目実施済み
        Const OptionSelectedString As String = " selected "

        Dim SelectedString As String = ""

        'ALREADY_REPLACE
        SelectedString = ""
        If dtInspecCodeRow.DISP_OPE_ITEM_ALREADY_REPLACE <> NoDisplayOption Then
            If dtInspecCodeRow.IsNull("OPERATION_RSLT_ALREADY_REPLACE") = False AndAlso dtInspecCodeRow.OPERATION_RSLT_ALREADY_REPLACE.ToString.Trim = SelectModeSelect.ToString Then
                SelectedString = OptionSelectedString
            End If
            dicOption.Add(CInt(dtInspecCodeRow.DISP_OPE_ITEM_ALREADY_REPLACE) * 10 + 1, "<option value=""1"" " & SelectedString & ">" & WebWordUtility.GetWord(ApplicationId, 24) & "</option>")
        End If

        'ALREADY_FIX
        SelectedString = ""
        If dtInspecCodeRow.DISP_OPE_ITEM_ALREADY_FIX <> NoDisplayOption Then
            If dtInspecCodeRow.IsNull("OPERATION_RSLT_ALREADY_FIX") = False AndAlso dtInspecCodeRow.OPERATION_RSLT_ALREADY_FIX.ToString.Trim = SelectModeSelect.ToString Then
                SelectedString = OptionSelectedString
            End If
            dicOption.Add(CInt(dtInspecCodeRow.DISP_OPE_ITEM_ALREADY_FIX) * 10 + 2, "<option value=""2"" " & SelectedString & ">" & WebWordUtility.GetWord(ApplicationId, 25) & "</option>")
        End If

        'ALREADY_CLEAN
        SelectedString = ""
        If dtInspecCodeRow.DISP_OPE_ITEM_ALREADY_CLEAN <> NoDisplayOption Then
            If dtInspecCodeRow.IsNull("OPERATION_RSLT_ALREADY_CLEAN") = False AndAlso dtInspecCodeRow.OPERATION_RSLT_ALREADY_CLEAN.ToString.Trim = SelectModeSelect.ToString Then
                SelectedString = OptionSelectedString
            End If
            dicOption.Add(CInt(dtInspecCodeRow.DISP_OPE_ITEM_ALREADY_CLEAN) * 10 + 3, "<option value=""3"" " & SelectedString & ">" & WebWordUtility.GetWord(ApplicationId, 26) & "</option>")
        End If

        'ALREADY_SWAP
        SelectedString = ""
        If dtInspecCodeRow.DISP_OPE_ITEM_ALREADY_SWAP <> NoDisplayOption Then
            If dtInspecCodeRow.IsNull("OPERATION_RSLT_ALREADY_SWAP") = False AndAlso dtInspecCodeRow.OPERATION_RSLT_ALREADY_SWAP.ToString.Trim = SelectModeSelect.ToString Then
                SelectedString = OptionSelectedString
            End If
            dicOption.Add(CInt(dtInspecCodeRow.DISP_OPE_ITEM_ALREADY_SWAP) * 10 + 4, "<option value=""4"" " & SelectedString & ">" & WebWordUtility.GetWord(ApplicationId, 27) & "</option>")
        End If

        'キーを昇順にソートして取得
        Dim SortedKeys As List(Of Integer) = dicOption.Keys.ToList
        SortedKeys.Sort()

        '選択肢リスト文字列を生成
        For i As Integer = 0 To SortedKeys.Count - 1
            '改行文字で結合して戻り値とする
            Result &= dicOption.Item(SortedKeys(i)) & vbCrLf
        Next

        Return Result
    End Function
#End Region

    ''' <summary>
    ''' データテーブルの再編(メンテナンス)
    ''' </summary>
    ''' <param name="dtMainteCode">データテーブル</param>
    ''' <remarks></remarks>
    Private Sub EditInspecCode(ByRef dtMainteCode As SC3180204MainteCodeListDataTable)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ユーザ情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'メンテ項目
        Dim strMainteMode As String = String.Empty
        Dim strMainteViewStyleColor As String = String.Empty
        Dim strMainteInputStyle As String = String.Empty
        Dim strMainteRegistMode As String = String.Empty
        Dim strMainteViewControl As String = String.Empty
        '択一チェック項目(未実施,実施)
        Dim strMainteSelectUncarriedOut As String = String.Empty
        Dim strMainteSelectEnforcement As String = String.Empty
        Dim strMainteCheck As String = String.Empty

        'DB項目の追加
        ''検査項目
        With dtMainteCode.Columns
            .Add("MainteMode", Type.GetType("System.String"))
            .Add("MainteViewStyle_Color", Type.GetType("System.String"))
            .Add("MainteInputStyle", Type.GetType("System.String"))
            .Add("MainteRegistMode", Type.GetType("System.String"))
            .Add("MainteViewControl", Type.GetType("System.String"))

            ''択一チェック項目(未実施,実施)
            .Add("MainteSelect_UncarriedOut", Type.GetType("System.String"))
            .Add("MainteSelect_Enforcement", Type.GetType("System.String"))
            .Add("MainteCheck", Type.GetType("System.String"))

            '行ロックバージョン
            .Add("TrnRowLockVersion", Type.GetType("System.String"))

            .Add("color", Type.GetType("System.String"))

            '【***完成検査_排他制御***】 start
            If dtMainteCode.Columns.Contains("EDIT_FLAG") = False Then
                .Add("EDIT_FLAG", Type.GetType("System.String"))
            End If
            If dtMainteCode.Columns.Contains("ServiceInRowLockVersion") = False Then
                .Add("ServiceInRowLockVersion", Type.GetType("System.String"))
            End If
            '【***完成検査_排他制御***】 end
        End With

        Dim strInspecItemCD As String = String.Empty
        Dim intIdx As Integer = 0

        'メンテ項目設定
        For intIdx = 0 To dtMainteCode.Count - 1
            'メンテ項目
            'strMainteMode = InspecItemModeNow
            'strMainteViewStyleColor = "background:transparent;"
            'strMainteInputStyle = ""
            'strMainteRegistMode = RegistModeRegist
            'strMainteViewControl = ""
            'If StallUseStatusWork > dtMainteCode(intIdx).STALL_USE_STATUS.ToString.Trim Then
            '    '未来
            '    strMainteMode = InspecItemModeFuture
            '    strMainteInputStyle = "disabled"
            '    strMainteViewStyleColor = "background:darkgray;"
            '    strMainteViewControl = InspecItemViewControlUnview
            '    strMainteRegistMode = RegistModeUnregist
            'ElseIf StallUseStatusWork < dtMainteCode(intIdx).STALL_USE_STATUS.ToString.Trim Then
            '    '過去
            '    strMainteMode = InspecItemModePast
            '    strMainteViewStyleColor = "background:lightgrey;"
            'End If
            Dim fontColor As String = String.Empty
            SetStatusColor(strMainteMode, strMainteViewStyleColor, strMainteInputStyle, strMainteRegistMode, strMainteViewControl, intIdx, dtMainteCode, fontColor)

            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '                , "Stall_Use_Status [{0}] : strInspecItemMode [{1}]" _
            '                , dtMainteCode(intIdx).STALL_USE_STATUS.ToString.Trim _
            '                , strMainteMode))

            'ステータス状態による編集可/不可の決定
            Dim lngRoStatusProc As Long = 0
            '2014/06/06 入力制御不具合　Start
            lngRoStatusProc = businessLogic.RoStatusCheck(Integer.Parse(dtMainteCode(intIdx).RO_STATUS.ToString))
            '2014/06/06 入力制御不具合　Start

            If True = fromTCMainFlg Then
                ' TCMainからの遷移
                ' TC,ChT
                'lngRoStatusProc = businessLogic.RoStatusCheck(Integer.Parse(roStatus))
                If lngRoStatusProc = RoStatusProcBeforeWork Then
                    '整備・点検実績データなし:編集可(変更なし)
                ElseIf lngRoStatusProc = RoStatusProcWorkToCompExaminationRequest Then
                    '最終チップ承認前(60:作業中、65:完成検査依頼中):編集可(変更なし)
                ElseIf lngRoStatusProc = RoStatusProcCompExaminationComplate Then
                    '納車より前(70:完成検査完了まで):編集不可
                    strMainteInputStyle = "disabled"
                    strMainteRegistMode = RegistModeUnregist
                ElseIf lngRoStatusProc = RoStatusProcDeliveryWaitToDeliveryWork Then
                    '納車より前(80:納車準備待ち、85:納車作業中):編集不可
                    strMainteInputStyle = "disabled"
                    strMainteRegistMode = RegistModeUnregist
                Else
                    '納車より前(90:納車済み以降):編集不可
                    strMainteInputStyle = "disabled"
                    strMainteRegistMode = RegistModeUnregist
                End If
            Else
                ' TCMain以外(チェックシートプレビュー)からの遷移
                If staffInfo.OpeCD = Operation.FM OrElse staffInfo.OpeCD = Operation.CT OrElse staffInfo.OpeCD = Operation.CHT Then
                    'lngRoStatusProc = businessLogic.RoStatusCheck(Integer.Parse(roStatus))
                    If lngRoStatusProc = RoStatusProcBeforeWork Then
                        '整備・点検実績データなし:表示しない
                        strMainteInputStyle = "disabled"
                        strMainteRegistMode = RegistModeUnregist
                        strMainteViewControl = "display: none;"
                    ElseIf lngRoStatusProc = RoStatusProcWorkToCompExaminationRequest Then
                        '最終チップ承認前(60:作業中、65:完成検査依頼中):編集可(変更なし)
                    ElseIf lngRoStatusProc = RoStatusProcCompExaminationComplate Then
                        '納車より前(70:完成検査完了まで):編集可(変更なし)
                    ElseIf lngRoStatusProc = RoStatusProcDeliveryWaitToDeliveryWork Then
                        '納車より前(80:納車準備待ち、85:納車作業中):編集不可
                        strMainteInputStyle = "disabled"
                        strMainteRegistMode = RegistModeUnregist
                    Else
                        '納車より前(90:納車済み以降):編集不可
                        strMainteInputStyle = "disabled"
                        strMainteRegistMode = RegistModeUnregist
                    End If
                Else
                    'SA,SM
                    'lngRoStatusProc = businessLogic.RoStatusCheck(Integer.Parse(roStatus))
                    If lngRoStatusProc = RoStatusProcBeforeWork Then
                        '整備・点検実績データなし:表示しない
                        strMainteInputStyle = "disabled"
                        strMainteRegistMode = RegistModeUnregist
                        strMainteViewControl = "display: none;"
                    ElseIf lngRoStatusProc = RoStatusProcWorkToCompExaminationRequest Then
                        '最終チップ承認前(60:作業中、65:完成検査依頼中):編集可(変更なし)
                    ElseIf lngRoStatusProc = RoStatusProcCompExaminationComplate Then
                        '納車より前(70:完成検査完了まで):編集可(変更なし)
                    ElseIf lngRoStatusProc = RoStatusProcDeliveryWaitToDeliveryWork Then
                        '納車より前(80:納車準備待ち、85:納車作業中):編集可(変更なし)
                    Else
                        '納車より前(90:納車済み以降):編集不可
                        strMainteInputStyle = "disabled"
                        strMainteRegistMode = RegistModeUnregist
                    End If
                End If
            End If

            With dtMainteCode(intIdx)
                .Item("MainteMode") = strMainteMode
                .Item("MainteViewStyle_Color") = strMainteViewStyleColor
                .Item("MainteInputStyle") = strMainteInputStyle
                .Item("MainteRegistMode") = strMainteRegistMode
                .Item("MainteViewControl") = strMainteViewControl

                .Item("color") = fontColor

            End With

            '択一チェック項目(未実施,実施)
            strMainteCheck = CheckModeUncheck
            strMainteSelectUncarriedOut = ""
            strMainteSelectEnforcement = ""
            If False = dtMainteCode(intIdx).IsNull("INSPEC_RSLT_CD") Then
                strMainteCheck = dtMainteCode(intIdx).INSPEC_RSLT_CD.ToString.Trim
                If CheckModeEnforcement = strMainteCheck Then
                    strMainteSelectUncarriedOut = "checked"
                    strMainteSelectEnforcement = ""
                ElseIf CheckModeUncarriedOut = strMainteCheck Then
                    strMainteSelectUncarriedOut = ""
                    strMainteSelectEnforcement = "checked"
                Else
                    strMainteCheck = CheckModeUncheck
                End If
            End If
            dtMainteCode(intIdx).Item("MainteCheck") = strMainteCheck
            dtMainteCode(intIdx).Item("MainteSelect_UncarriedOut") = strMainteSelectUncarriedOut
            dtMainteCode(intIdx).Item("MainteSelect_Enforcement") = strMainteSelectEnforcement

            'Send又はRegisterの切替用フラグ(空文字:Register/1:Send)
            If InspecItemModeNow = strMainteMode OrElse jobDtlId = dtMainteCode(intIdx).JOB_DTL_ID.ToString Then
                If InspectionNeedFlgRegister = dtMainteCode(intIdx).INSPECTION_NEED_FLG.ToString.Trim Then
                    'Send又はRegisterの切替用フラグ(空文字:Register/1:Send)
                    SendOrRegister.Value = SendOrRegisterFlgSend
                End If
            End If

            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '          , "InspecItemMode [{0}] : INSPECTION_STATUS [{1}] : INSPECTION_NEED_FLG [{2}] : SendOrRegister [{3}]" _
            '          , strMainteMode _
            '          , dtMainteCode(intIdx).INSPECTION_STATUS.ToString.Trim _
            '          , dtMainteCode(intIdx).INSPECTION_NEED_FLG.ToString.Trim _
            '          , SendOrRegister.Value))

            '行ロックバージョンの取得
            If False = dtMainteCode(intIdx).IsNull("TRN_ROW_LOCK_VERSION") Then
                dtMainteCode(intIdx).Item("TrnRowLockVersion") = dtMainteCode(intIdx).TRN_ROW_LOCK_VERSION.ToString.Trim
                'Logger.Info(String.Format(CultureInfo.CurrentCulture _
                '                        , "TRN_Lock_version [{0}]" _
                '                        , dtMainteCode(intIdx).TRN_ROW_LOCK_VERSION.ToString.Trim))
            Else
                dtMainteCode(intIdx).Item("TrnRowLockVersion") = UnsetRowLockVer
                'Logger.Info(String.Format(CultureInfo.CurrentCulture _
                '                        , "TRN_Lock_version [-1]"))
            End If

            '【***完成検査_排他制御***】 start
            ' 変更フラグ追加
            dtMainteCode(intIdx).Item("EDIT_FLAG") = dtMainteCode(intIdx).Item("EDIT_FLAG").ToString

            '排他チェック用行ロックバージョン追加
            If False = dtMainteCode(intIdx).IsNull("ServiceInRowLockVersion") Then
                dtMainteCode(intIdx).Item("ServiceInRowLockVersion") = dtMainteCode(intIdx).Item("EXCLUSION_ROW_LOCK_VERSION").ToString
            Else
                dtMainteCode(intIdx).Item("ServiceInRowLockVersion") = UnsetRowLockVer
            End If

            '【***完成検査_排他制御***】 end

        Next intIdx

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' データテーブルの再編
    ''' </summary>
    ''' <param name="intIndex">インデックス</param>
    ''' <remarks></remarks>
    
    '【***完成検査_排他制御***】 start
    Private Function GetInspecIconPosStyle(ByVal intIndex As Integer, ByVal cleanFlg As Boolean) As String
	'【***完成検査_排他制御***】 end

        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} START" _
        '            , Me.GetType.ToString _
        '            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim strResult As String = String.Empty

        If cleanFlg = True Then
            If NoProblemIdx = intIndex Then
                strResult = "left:0px;"
            ElseIf NeedInspectionIdx = intIndex Then
                strResult = "left:72px;"
            ElseIf NeedReplaceIdx = intIndex Then
                strResult = "left:124px;"
            ElseIf NeedFixingIdx = intIndex Then
                strResult = "left:176px;"
            ElseIf NeedCleaningIdx = intIndex Then
                strResult = "left:228px;"
            ElseIf NeedSwappingIdx = intIndex Then
                strResult = "left:280px;"
            End If
        Else
        If NoProblemIdx = intIndex Then
            strResult = "left:0px;"
        ElseIf NeedInspectionIdx = intIndex Then
            strResult = "left:48px;"
        ElseIf NeedReplaceIdx = intIndex Then
            strResult = "left:100px;"
        ElseIf NeedFixingIdx = intIndex Then
            strResult = "left:152px;"
        ElseIf NeedCleaningIdx = intIndex Then
            strResult = "left:204px;"
        ElseIf NeedSwappingIdx = intIndex Then
            strResult = "left:256px;"
        End If
        End If



        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '          , "{0}.{1} END" _
        '          , Me.GetType.ToString _
        '          , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return strResult

    End Function

    ''' <summary>
    ''' 完成検査結果データの取得
    ''' </summary>
    ''' <param name="intaprovalStatus">作業ステータス</param>
    ''' <param name="dtInspecItem">検査項目</param>
    ''' <param name="dtMaintenance">メンテナンス項目</param>
    ''' <remarks>True:成功/False:失敗</remarks>
    Private Function GetRegistInfo(ByVal intaprovalStatus As Integer, _
                                   ByRef dtInspecItem As SC3180204RegistInfoDataTable, _
                                   ByRef dtMaintenance As SC3180204RegistInfoDataTable) As Boolean

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True

        Dim strJobDtlID As String                   '作業内容ID
        Dim decJobDtlID As Decimal
        Dim strJobInstructID As String              '作業指示ID
        Dim strJobInstructSeq As String             '作業指示枝番
        Dim lngJobInstructSeq As Long
        Dim strInspecItemCD As String               '点検項目コード

        'Dim strAdviceContent As String = String.Empty         'アドバイス(一時保存用)

        Dim intPosIndex As Integer
        Dim intIndex As Integer
        'Dim intArrayIndex As Integer
        Dim strInspecItemMode As String
        Dim strInspecItemRegistMode As String
        Dim strInspecItemsCheck As String
        Dim lngInspecItemsCheck As Long
        Dim strInspecItemsTextBefore As String
        Dim decInspecItemsTextBefore As Decimal
        Dim strInspecItemsTextAfter As String
        Dim decInspecItemsTextAfter As Decimal
        Dim lngInspecItemsSelectReplaced As Long
        Dim lngInspecItemsSelectFixed As Long
        Dim lngInspecItemsSelectCleaned As Long
        Dim lngInspecItemsSelectSwapped As Long
        Dim updateTime As Date = DateTimeFunc.Now(dealerCD)
        Dim hiddendataId As String

        '【***完成検査_排他制御***】 start
        Dim strEditFlag As String = ""
        Dim strServiceInRowLockVersion As String = ""
        Dim lngServiceINRowLockVersion As Long
        '【***完成検査_排他制御***】 end

        ''TechnicianAdvice
        'strAdviceContent = Request.Form("TechnicianAdvice")

        'サービスIDの取得
        serviceID = Decimal.Parse(Request.Form("ServiceID"))

        For intPosIndex = PartIndexEngine To PartIndexTrunk
            intIndex = 1
            hiddendataId = "HiddenAllData" & intPosIndex.ToString & "_" & intIndex.ToString
            Do While Request.Form(hiddendataId) IsNot Nothing
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
              , "{0}.{1} hiddendata:{2}" _
              , Me.GetType.ToString _
              , System.Reflection.MethodBase.GetCurrentMethod.Name _
              , Request.Form(hiddendataId)))
                '登録モード
                Dim hiddenDataList As String() = Request.Form(hiddendataId).Split("|"c)

                strInspecItemMode = hiddenDataList(HiddenDataNo.InspecItemMode)
                strInspecItemRegistMode = hiddenDataList(HiddenDataNo.InspecItemRegistMode)

                If RegistModeRegist = strInspecItemRegistMode And _
                   (InspecItemModeNow = strInspecItemMode OrElse InspecItemModePast = strInspecItemMode) Then
                    '択一チェック項目(Good,Inspect,Replace,Fix,Cleaning,Swap)
                    strInspecItemsCheck = hiddenDataList(HiddenDataNo.InspecItemsCheck)
                    lngInspecItemsCheck = Long.Parse(strInspecItemsCheck)
                    'テキストボックス(Before,After)
                    strInspecItemsTextBefore = Request.Form("BeforeText" & intPosIndex.ToString & "_" & intIndex.ToString)
                    decInspecItemsTextBefore = DefaultBeforeText

                    If True = IsNumeric(strInspecItemsTextBefore) Then
                        decInspecItemsTextBefore = Decimal.Parse(strInspecItemsTextBefore)
                    End If
                    strInspecItemsTextAfter = Request.Form("AfterText" & intPosIndex.ToString & "_" & intIndex.ToString)
                    decInspecItemsTextAfter = DefaultAfterText
                    If True = IsNumeric(strInspecItemsTextAfter) Then
                        decInspecItemsTextAfter = Decimal.Parse(strInspecItemsTextAfter)
                    End If
                    '複数選択リスト(Replaced,Fixed,Cleaned,Swapped)
                    lngInspecItemsSelectReplaced = SelectModeUnselect
                    lngInspecItemsSelectFixed = SelectModeUnselect
                    lngInspecItemsSelectCleaned = SelectModeUnselect
                    lngInspecItemsSelectSwapped = SelectModeUnselect
                    Dim stArrayData() As String
                    stArrayData = Split(Request.Form("InspecItemsSelector" & intPosIndex.ToString & "_" & intIndex.ToString), ",")
                    For idx = 0 To UBound(stArrayData)
                        If AlreadyReplaceIdx = stArrayData(idx) Then
                            lngInspecItemsSelectReplaced = CheckModeCheck
                        ElseIf AlreadyFixIdx = stArrayData(idx) Then
                            lngInspecItemsSelectFixed = CheckModeCheck
                        ElseIf AlreadyCleanIdx = stArrayData(idx) Then
                            lngInspecItemsSelectCleaned = CheckModeCheck
                        ElseIf AlreadySwapIdx = stArrayData(idx) Then
                            lngInspecItemsSelectSwapped = CheckModeCheck
                        End If
                    Next idx

                    '作業内容ID
                    strJobDtlID = hiddenDataList(HiddenDataNo.JobDtlID)
                    decJobDtlID = Decimal.Parse(strJobDtlID)
                    If InspecItemModeNow = strInspecItemMode Then
                        'ストール利用ステータスが'02'の時、作業内容IDとストール利用IDを代入する
                        nowJobDtl = decJobDtlID
                        stallId = Decimal.Parse(hiddenDataList(HiddenDataNo.StallUseID))
                    End If
                    '作業指示ID
                    strJobInstructID = hiddenDataList(HiddenDataNo.JobInstructID)
                    '作業指示枝番
                    strJobInstructSeq = hiddenDataList(HiddenDataNo.JobInstructSeq)
                    lngJobInstructSeq = Long.Parse(strJobInstructSeq)
                    '点検項目コード
                    strInspecItemCD = hiddenDataList(HiddenDataNo.InspecItemCD)
                    '行ロックバージョンの取得
                    rowLockVer = ""
                    rowLockVer = hiddenDataList(HiddenDataNo.TRN_RowLockVer)
                    If rowLockVer <> "" Then
                        trnRowLockVer = Long.Parse(rowLockVer)
                    Else
                        trnRowLockVer = 0
                    End If

                    '【***完成検査_排他制御***】 start
                    '変更フラグの格納
                    strEditFlag = Request.Form("EditFlag" & intPosIndex.ToString & "_" & intIndex.ToString)

                    '排他チェック用行ロックバージョンの取得
                    strServiceInRowLockVersion = ""
                    strServiceInRowLockVersion = hiddenDataList(HiddenDataNo.ServiceInLockVer)
                    If strServiceInRowLockVersion <> "" Then
                        lngServiceINRowLockVersion = Long.Parse(strServiceInRowLockVersion)
                    Else
                        lngServiceINRowLockVersion = UnsetRowLockVer
                    End If
                    

                    '登録情報の格納
                    dtInspecItem.Rows.Add(decJobDtlID, _
                                        intaprovalStatus, _
                                        trnRowLockVer, _
                                        strJobInstructID, _
                                        lngJobInstructSeq, _
                                        strInspecItemCD, _
                                        lngInspecItemsCheck, _
                                        lngInspecItemsSelectReplaced, _
                                        lngInspecItemsSelectFixed, _
                                        lngInspecItemsSelectCleaned, _
                                        lngInspecItemsSelectSwapped, _
                                        decInspecItemsTextBefore, _
                                        decInspecItemsTextAfter, _
                                        hiddenDataList(HiddenDataNo.SVC_CD), _
                                        strEditFlag, _
                                        lngServiceINRowLockVersion) '【***完成検査_排他制御***】 変更フラグ＆排他チェック用行ロックバージョン追加
                    
                    '【***完成検査_排他制御***】 end
                    '2014/06/06 登録不具合のため引数変更　End

                End If

                intIndex += 1
                hiddendataId = "HiddenAllData" & intPosIndex.ToString & "_" & intIndex.ToString
            Loop

        Next

        'For intPosIndex = PartIndexEngine To PartIndexTrunk
        '    intIndex = 1
        '    strID = "InspecItemsCheck" & intPosIndex.ToString & "_" & intIndex.ToString
        '    Do While Request.Form(strID) IsNot Nothing
        '        '登録モード
        '        strInspecItemMode = Request.Form("InspecItemMode" & intPosIndex.ToString & "_" & intIndex.ToString)
        '        strInspecItemRegistMode = Request.Form("InspecItemRegistMode" & intPosIndex.ToString & "_" & intIndex.ToString)
        '        If RegistModeRegist = strInspecItemRegistMode And _
        '           (InspecItemModeNow = strInspecItemMode OrElse InspecItemModePast = strInspecItemMode) Then
        '            '択一チェック項目(Good,Inspect,Replace,Fix,Cleaning,Swap)
        '            strID = "InspecItemsCheck" & intPosIndex.ToString & "_" & intIndex.ToString
        '            strInspecItemsCheck = Request.Form(strID)
        '            lngInspecItemsCheck = Long.Parse(strInspecItemsCheck)
        '            'テキストボックス(Before,After)
        '            strInspecItemsTextBefore = Request.Form("BeforeText" & intPosIndex.ToString & "_" & intIndex.ToString)
        '            decInspecItemsTextBefore = DefaultBeforeText

        '            If True = IsNumeric(strInspecItemsTextBefore) Then
        '                decInspecItemsTextBefore = Decimal.Parse(strInspecItemsTextBefore)
        '            End If
        '            strInspecItemsTextAfter = Request.Form("AfterText" & intPosIndex.ToString & "_" & intIndex.ToString)
        '            decInspecItemsTextAfter = DefaultAfterText
        '            If True = IsNumeric(strInspecItemsTextAfter) Then
        '                decInspecItemsTextAfter = Decimal.Parse(strInspecItemsTextAfter)
        '            End If
        '            '複数選択リスト(Replaced,Fixed,Cleaned,Swapped)
        '            lngInspecItemsSelectReplaced = SelectModeUnselect
        '            lngInspecItemsSelectFixed = SelectModeUnselect
        '            lngInspecItemsSelectCleaned = SelectModeUnselect
        '            lngInspecItemsSelectSwapped = SelectModeUnselect
        '            Dim stArrayData() As String
        '            stArrayData = Split(Request.Form("InspecItemsSelector" & intPosIndex.ToString & "_" & intIndex.ToString), ",")
        '            For idx = 0 To UBound(stArrayData)
        '                If AlreadyReplaceIdx = stArrayData(idx) Then
        '                    lngInspecItemsSelectReplaced = CheckModeCheck
        '                ElseIf AlreadyFixIdx = stArrayData(idx) Then
        '                    lngInspecItemsSelectFixed = CheckModeCheck
        '                ElseIf AlreadyCleanIdx = stArrayData(idx) Then
        '                    lngInspecItemsSelectCleaned = CheckModeCheck
        '                ElseIf AlreadySwapIdx = stArrayData(idx) Then
        '                    lngInspecItemsSelectSwapped = CheckModeCheck
        '                End If
        '            Next idx

        '            '作業内容ID
        '            strJobDtlID = Request.Form("JobDtlID" & intPosIndex.ToString & "_" & intIndex.ToString)
        '            decJobDtlID = Decimal.Parse(strJobDtlID)
        '            If InspecItemModeNow = strInspecItemMode Then
        '                'ストール利用ステータスが'02'の時、作業内容IDとストール利用IDを代入する
        '                nowJobDtl = decJobDtlID
        '                stallId = Decimal.Parse(Request.Form("StallUseID" & intPosIndex.ToString & "_" & intIndex.ToString))
        '            End If
        '            '作業指示ID
        '            strJobInstructID = Request.Form("JobInstructID" & intPosIndex.ToString & "_" & intIndex.ToString)
        '            '作業指示枝番
        '            strJobInstructSeq = Request.Form("JobInstructSeq" & intPosIndex.ToString & "_" & intIndex.ToString)
        '            lngJobInstructSeq = Long.Parse(strJobInstructSeq)
        '            '点検項目コード
        '            strInspecItemCD = Request.Form("InspecItemCD" & intPosIndex.ToString & "_" & intIndex.ToString)
        '            '行ロックバージョンの取得
        '            rowLockVer = ""
        '            rowLockVer = Request.Form("TRN_RowLockVer" & intPosIndex.ToString & "_" & intIndex.ToString)
        '            If rowLockVer <> "" Then
        '                trnRowLockVer = Long.Parse(rowLockVer)
        '            Else
        '                trnRowLockVer = 0
        '            End If

        '            '登録情報の格納
        '            '2014/06/02 Edit svcCdを追加 Start
        '            'dtInspecItem.Rows.Add(decJobDtlID, _
        '            '                      intaprovalStatus, _
        '            '                      trnRowLockVer, _
        '            '                      DefaultJobInspectId, _
        '            '                      DefaultJobInspectSeq, _
        '            '                      strInspecItemCD, _
        '            '                      lngInspecItemsCheck, _
        '            '                      lngInspecItemsSelectReplaced, _
        '            '                      lngInspecItemsSelectFixed, _
        '            '                      lngInspecItemsSelectCleaned, _
        '            '                      lngInspecItemsSelectSwapped, _
        '            '                      decInspecItemsTextBefore, _
        '            '                      decInspecItemsTextAfter)
        '            '2014/06/02 Edit svcCdを追加 End

        '            '2014/06/06 登録不具合のため引数変更　Start
        '            'dtInspecItem.Rows.Add(decJobDtlID, _
        '            '                      intaprovalStatus, _
        '            '                      trnRowLockVer, _
        '            '                      DefaultJobInspectId, _
        '            '                      DefaultJobInspectSeq, _
        '            '                      strInspecItemCD, _
        '            '                      lngInspecItemsCheck, _
        '            '                      lngInspecItemsSelectReplaced, _
        '            '                      lngInspecItemsSelectFixed, _
        '            '                      lngInspecItemsSelectCleaned, _
        '            '                      lngInspecItemsSelectSwapped, _
        '            '                      decInspecItemsTextBefore, _
        '            '                      decInspecItemsTextAfter, _
        '            '                      Request.Form("SVC_CD" & intPosIndex.ToString & "_" & intIndex.ToString))
        '            dtInspecItem.Rows.Add(decJobDtlID, _
        '                                intaprovalStatus, _
        '                                trnRowLockVer, _
        '                                strJobInstructID, _
        '                                lngJobInstructSeq, _
        '                                strInspecItemCD, _
        '                                lngInspecItemsCheck, _
        '                                lngInspecItemsSelectReplaced, _
        '                                lngInspecItemsSelectFixed, _
        '                                lngInspecItemsSelectCleaned, _
        '                                lngInspecItemsSelectSwapped, _
        '                                decInspecItemsTextBefore, _
        '                                decInspecItemsTextAfter, _
        '                                Request.Form("SVC_CD" & intPosIndex.ToString & "_" & intIndex.ToString))
        '            '2014/06/06 登録不具合のため引数変更　End

        '        End If

        '        intIndex += 1
        '        strID = "InspecItemsCheck" & intPosIndex.ToString & "_" & intIndex.ToString
        '    Loop

        'Next

        Dim strID As String
        'Maintenance
        If True = blnResult Then
            intPosIndex = PartIndexMaintenance
            intIndex = 1
            strID = "MainteCheck" & intPosIndex.ToString & "_" & intIndex.ToString
            Do While Request.Form(strID) IsNot Nothing
                '登録モード
                strInspecItemMode = Request.Form("MainteMode" & intPosIndex.ToString & "_" & intIndex.ToString)
                strInspecItemRegistMode = Request.Form("MainteRegistMode" & intPosIndex.ToString & "_" & intIndex.ToString)
                If RegistModeRegist = strInspecItemRegistMode And _
                   (InspecItemModeNow = strInspecItemMode OrElse InspecItemModePast = strInspecItemMode) Then
                    '択一チェック項目(Good,Inspect,Replace,Fix,Cleaning,Swap)
                    strID = "Maintenance" & intPosIndex.ToString & "_" & intIndex.ToString
                    strInspecItemsCheck = Request.Form(strID)
                    If strInspecItemsCheck Is Nothing Then
                        strInspecItemsCheck = CheckModeUncheck
                    End If
                    lngInspecItemsCheck = Long.Parse(strInspecItemsCheck)

                    '作業内容ID
                    strJobDtlID = Request.Form("JobDtlID" & intPosIndex.ToString & "_" & intIndex.ToString)
                    decJobDtlID = Decimal.Parse(strJobDtlID)
                    If InspecItemModeNow = strInspecItemMode Then
                        'ストール利用ステータスが'02'の時、作業内容IDとストール利用IDを代入する
                        nowJobDtl = decJobDtlID
                        stallId = Decimal.Parse(Request.Form("StallUseID" & intPosIndex.ToString & "_" & intIndex.ToString))
                    End If
                    '作業指示ID
                    strJobInstructID = Request.Form("JobInstructID" & intPosIndex.ToString & "_" & intIndex.ToString)
                    '作業指示枝番
                    strJobInstructSeq = Request.Form("JobInstructSeq" & intPosIndex.ToString & "_" & intIndex.ToString)
                    lngJobInstructSeq = Long.Parse(strJobInstructSeq)
                    '行ロックバージョンの取得
                    rowLockVer = ""
                    rowLockVer = Request.Form("TRN_RowLockVer" & intPosIndex.ToString & "_" & intIndex.ToString)
                    If rowLockVer <> "" Then
                        trnRowLockVer = Long.Parse(rowLockVer)
                    Else
                        trnRowLockVer = 0
                    End If

                    '【***完成検査_排他制御***】 start
                    '変更フラグの格納
                    strEditFlag = Request.Form("EditFlag" & intPosIndex.ToString & "_" & intIndex.ToString)

                    '排他チェック用行ロックバージョンの取得
                    strServiceInRowLockVersion = ""
                    strServiceInRowLockVersion = Request.Form("ServiceInLockVer" & intPosIndex.ToString & "_" & intIndex.ToString)
                    If strServiceInRowLockVersion <> "" Then
                        lngServiceINRowLockVersion = Long.Parse(strServiceInRowLockVersion)
                    Else
                        lngServiceINRowLockVersion = UnsetRowLockVer
                    End If
                    

                    '登録情報の格納
                    dtMaintenance.Rows.Add(decJobDtlID, _
                                           intaprovalStatus, _
                                           trnRowLockVer, _
                                           strJobInstructID, _
                                           lngJobInstructSeq, _
                                           DefaultItemCD, _
                                           lngInspecItemsCheck, _
                                           DefaultAlreadyReplace, _
                                           DefaultAlreadyFix, _
                                           DefaultAlreadyClean, _
                                           DefaultAlreadySwap, _
                                           DefaultBeforeText, _
                                           DefaultAfterText, _
                                           serviceID, _
                                           strEditFlag, _
                                           lngServiceINRowLockVersion) '【***完成検査_排他制御***】 変更フラグ＆排他チェック用行ロックバージョン追加

					'【***完成検査_排他制御***】 end
                End If

                intIndex += 1
                strID = "MainteCheck" & intPosIndex.ToString & "_" & intIndex.ToString
            Loop
        End If

        '【***完成検査_排他制御***】 start
        '事前排他チェック用にHiddenパラメータに保持して置いた値を取得
        serviceInLockVer = Long.Parse(Request.Form("ServiceInLockVer"))
        '【***完成検査_排他制御***】 end

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return blnResult

    End Function

    ''' <summary>
    ''' 背景・文字色設定
    ''' </summary>
    ''' <param name="strMainteMode"></param>
    ''' <param name="strMainteViewStyleColor"></param>
    ''' <param name="strMainteInputStyle"></param>
    ''' <param name="strMainteRegistMode"></param>
    ''' <param name="strMainteViewControl"></param>
    ''' <param name="intIdx"></param>
    ''' <param name="dtMainteCode"></param>
    ''' <param name="fontColor"></param>
    ''' <remarks></remarks>
    Private Sub SetStatusColor(ByRef strMainteMode As String, _
                               ByRef strMainteViewStyleColor As String, _
                               ByRef strMainteInputStyle As String, _
                               ByRef strMainteRegistMode As String, _
                               ByRef strMainteViewControl As String, _
                               ByVal intIdx As Integer, _
                               ByVal dtMainteCode As SC3180204MainteCodeListDataTable, _
                               ByRef fontColor As String)

        strMainteMode = InspecItemModeNow
        strMainteViewStyleColor = "background:transparent;"
        strMainteInputStyle = ""
        strMainteRegistMode = RegistModeRegist
        strMainteViewControl = ""
        fontColor = ""

        Select Case dtMainteCode(intIdx).STALL_USE_STATUS.ToString.Trim
            Case Is < StallUseStatusWork ''00:着工指示待ち 01:作業開始待ち 
                strMainteMode = InspecItemModeFuture
                strMainteInputStyle = "disabled"
                strMainteViewStyleColor = "background:darkgray;"
                strMainteViewControl = InspecItemViewControlUnview
                strMainteRegistMode = RegistModeUnregist
                fontColor = ""

            Case Is = StallUseStatusWork '02:作業中
                'カレントジョブ 
                If dtMainteCode(intIdx).JOB_DTL_ID = jobDtlId Then
                    '文字色：通常
                    '背景色：白
                    strMainteMode = InspecItemModeNow
                    strMainteViewStyleColor = "background:transparent;"
                    fontColor = ""
                Else
                    '文字色：通常
                    '背景色：ダークグレイ
                    strMainteMode = InspecItemModeFuture
                    strMainteInputStyle = "disabled"
                    strMainteViewStyleColor = "background:darkgray;"
                    strMainteViewControl = InspecItemViewControlUnview
                    strMainteRegistMode = RegistModeUnregist
                    fontColor = ""
                End If

            Case Is = StallUseStatusCompletion '03:完了
                '背景色：ライトグレイ
                strMainteMode = InspecItemModePast
                strMainteViewStyleColor = "background:lightgrey;"
                fontColor = ""
                If dtMainteCode(intIdx).JOB_DTL_ID = jobDtlId Then
                    '文字色：オレンジ
                    fontColor = " color:#FF4500"
                End If

            Case Is > StallUseStatusCompletion ''04:作業指示の一部の作業が中断 05:中断 06:日跨ぎ終了 07:未来店客
                '文字色：通常
                '背景色：ダークグレイ
                'strMainteMode = InspecItemModePast
                strMainteMode = InspecItemModeFuture
                strMainteInputStyle = "disabled"
                strMainteViewStyleColor = "background:darkgray;"
                strMainteViewControl = InspecItemViewControlUnview
                strMainteRegistMode = RegistModeUnregist
                fontColor = ""

        End Select

    End Sub

    ''' <summary>
    ''' 背景・文字色設定
    ''' </summary>
    ''' <param name="strMainteMode"></param>
    ''' <param name="strMainteViewStyleColor"></param>
    ''' <param name="strMainteInputStyle"></param>
    ''' <param name="strMainteRegistMode"></param>
    ''' <param name="strMainteViewControl"></param>
    ''' <param name="intIdx"></param>
    ''' <param name="dtMainteCode"></param>
    ''' <param name="fontColor"></param>
    ''' <remarks></remarks>
    Private Sub SetStatusColor(ByRef strMainteMode As String, _
                               ByRef strMainteViewStyleColor As String, _
                               ByRef strMainteInputStyle As String, _
                               ByRef strMainteRegistMode As String, _
                               ByRef strMainteViewControl As String, _
                               ByVal intIdx As Integer, _
                               ByVal dtMainteCode As SC3180204InspectCodeDataTable, _
                               ByRef fontColor As String)

        strMainteMode = InspecItemModeNow
        strMainteViewStyleColor = "background:transparent;"
        strMainteInputStyle = ""
        strMainteRegistMode = RegistModeRegist
        strMainteViewControl = ""
        fontColor = ""

        Select Case dtMainteCode(intIdx).STALL_USE_STATUS.ToString.Trim
            Case Is < StallUseStatusWork ''00:着工指示待ち 01:作業開始待ち 
                strMainteMode = InspecItemModeFuture
                strMainteInputStyle = "disabled"
                strMainteViewStyleColor = "background:darkgray;"
                strMainteViewControl = InspecItemViewControlUnview
                strMainteRegistMode = RegistModeUnregist
                fontColor = ""

            Case Is = StallUseStatusWork '02:作業中
                'カレントジョブ 
                If dtMainteCode(intIdx).JOB_DTL_ID = jobDtlId Then
                    '文字色：通常
                    '背景色：白
                    strMainteMode = InspecItemModeNow
                    strMainteViewStyleColor = "background:transparent;"
                    fontColor = ""
                Else
                    '文字色：通常
                    '背景色：ダークグレイ
                    strMainteMode = InspecItemModeFuture
                    strMainteInputStyle = "disabled"
                    strMainteViewStyleColor = "background:darkgray;"
                    strMainteViewControl = InspecItemViewControlUnview
                    strMainteRegistMode = RegistModeUnregist
                    fontColor = ""
                End If

            Case Is = StallUseStatusCompletion '03:完了
                '背景色：ライトグレイ
                strMainteMode = InspecItemModePast
                strMainteViewStyleColor = "background:lightgrey;"
                fontColor = ""
                If dtMainteCode(intIdx).JOB_DTL_ID = jobDtlId Then
                    '文字色：オレンジ
                    fontColor = " color:#FF4500"
                End If

            Case Is > StallUseStatusCompletion ''04:作業指示の一部の作業が中断 05:中断 06:日跨ぎ終了 07:未来店客
                '文字色：通常
                '背景色：ダークグレイ
                'strMainteMode = InspecItemModePast
                strMainteMode = InspecItemModeFuture
                strMainteInputStyle = "disabled"
                strMainteViewStyleColor = "background:darkgray;"
                strMainteViewControl = InspecItemViewControlUnview
                strMainteRegistMode = RegistModeUnregist
                fontColor = ""

        End Select

    End Sub

#End Region

    ''2014/06/13 仕様変更対応 Start
    ' ''' <summary>
    ' ''' データ件数チェック
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function CheckDataCount() As Boolean
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            , "{0}.{1} START" _
    '            , Me.GetType.ToString _
    '            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '
    '    Dim result As Boolean = businessLogic.CheckDataCount(dealerCD, branchCD, roNum)
    '
    '    If result = False Then
    '        '読み込みエラーメッセージの取得
    '        ErrorMessage.Text = "Error"
    '        ErrorFlg.Value = ErrorFlgError
    '        ErrorMessage.Text = WebWordUtility.GetWord(MsgID.id35)
    '    End If
    '
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} END" _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '
    '    Return result
    '
    'End Function
    ''2014/06/13 仕様変更対応 End

    ' 2015/5/1 強制納車対応 警告表示後の前画面遷移リクエスト start
    ''' <summary>
    ''' 警告表示時の前画面遷移処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    
    '【***完成検査_排他制御***】 start
    Protected Sub HiddenButtonWarning_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonWarning.Click
　　'【***完成検査_排他制御***】 end	
        'FMメインから来る場合と、通知履歴から来た場合で遷移先が異なる(元の画面に戻る)
        Me.RedirectPrevScreen()
    End Sub
    ' 2015/5/1 強制納車対応 警告表示後の前画面遷移リクエスト end

    '【***完成検査_排他制御***】 start
    ''' <summary>
    ''' 事前排他チェック
    ''' </summary>
    ''' <param name="updateType">更新タイプ</param>
    ''' <remarks></remarks>
    Private Function ExclusionChk(ByRef dtInspecItem As SC3180204RegistInfoDataTable, _
                                  ByRef dtMaintenance As SC3180204RegistInfoDataTable, _
                                  ByRef strAdviceContent As String, _
                                  Optional ByVal updateType As Integer = 0) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
           , "{0}.{1} START" _
           , Me.GetType.ToString _
           , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True
        Dim isExclusionFlag As Boolean = False
        isExclusionFlag = businessLogic.CheckUpdateFinalInspection( _
                                 serviceInLockVer, _
                                 dealerCD, _
                                 branchCD, _
                                 roNum)

        If isExclusionFlag = False Then

            'ShowMessageBox(MsgID.idExclusion)

            '画面で変更のあったデータを抽出
            Dim editDtInspecItem As New SC3180204RegistInfoDataTable()
            Dim editDtMaintenance As New SC3180204RegistInfoDataTable()
            ExtractionEditingData(dtInspecItem, dtMaintenance, editDtInspecItem, editDtMaintenance)

            'ページ初期表示
            InitScreen(editDtInspecItem, editDtMaintenance, strAdviceContent, isExclusionFlag)

            blnResult = False
            editFlag = "1"
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
            ShowMessageBox(MsgID.idExclusion)
            
        Else
            blnResult = True
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
       , "{0}.{1} END" _
       , Me.GetType.ToString _
       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return blnResult
    End Function

    ''' <summary>
    ''' 変更フラグの立っている項目のみを抽出して返却
    ''' </summary>
    ''' <param name="inDtInspecItem">点検項目データ</param>
    ''' <param name="inDtMaintenance">メンテナンスデータ</param>
    ''' <remarks></remarks>
    Private Sub ExtractionEditingData(ByVal inDtInspecItem As SC3180204RegistInfoDataTable, _
                                      ByVal inDtMaintenance As SC3180204RegistInfoDataTable, _
                                      ByRef editDtInspecItem As SC3180204RegistInfoDataTable, _
                                      ByRef editDtMaintenance As SC3180204RegistInfoDataTable)

        Dim tmpDtInspecItem As New SC3180204RegistInfoDataTable()
        Dim tmpDtMaintenance As New SC3180204RegistInfoDataTable()

        '点検項目抽出
        If Not IsNothing(inDtInspecItem) Then
            For Each tmp As SC3180204RegistInfoRow In inDtInspecItem
                '変更フラグが立っているか確認する
                If tmp.IsEDIT_FLAGNull = False AndAlso CStr(tmp.EDIT_FLAG) = "1" Then
                    '「変更あり」として一時退避
                    tmpDtInspecItem.AddSC3180204RegistInfoRow(
                        tmp.JobDtlID,
                        tmp.AprovalStatus,
                        tmp.RowLockVer,
                        tmp.JobInstructID,
                        tmp.JobInstructSeq,
                        tmp.ItemCD,
                        tmp.ItemsCheck,
                        tmp.ItemsSelect_Replaced,
                        tmp.ItemsSelect_Fixed,
                        tmp.ItemsSelect_Cleaned,
                        tmp.ItemsSelect_Swapped,
                        tmp.ItemsTextBefore,
                        tmp.ItemsTextAfter,
                        tmp.SVC_CD,
                        tmp.EDIT_FLAG,
                        tmp.EXCLUSION_ROW_LOCK_VERSION)
                End If
            Next
        End If

        'メンテナンス抽出
        If Not IsNothing(inDtMaintenance) Then
            For Each tmp As SC3180204RegistInfoRow In inDtMaintenance
                '変更フラグが立っているか確認する
                If tmp.IsEDIT_FLAGNull = False AndAlso CStr(tmp.EDIT_FLAG) = "1" Then

                    '「変更あり」として一時退避
                    tmpDtMaintenance.AddSC3180204RegistInfoRow(
                        tmp.JobDtlID,
                        tmp.AprovalStatus,
                        tmp.RowLockVer,
                        tmp.JobInstructID,
                        tmp.JobInstructSeq,
                        tmp.ItemCD,
                        tmp.ItemsCheck,
                        tmp.ItemsSelect_Replaced,
                        tmp.ItemsSelect_Fixed,
                        tmp.ItemsSelect_Cleaned,
                        tmp.ItemsSelect_Swapped,
                        tmp.ItemsTextBefore,
                        tmp.ItemsTextAfter,
                        tmp.SVC_CD,
                        tmp.EDIT_FLAG,
                        tmp.EXCLUSION_ROW_LOCK_VERSION)
                End If
            Next
        End If

        '退避した入力データを格納
        editDtInspecItem = CType(tmpDtInspecItem.Copy, SC3180204RegistInfoDataTable)
        editDtMaintenance = CType(tmpDtMaintenance.Copy, SC3180204RegistInfoDataTable)

    End Sub
    '【***完成検査_排他制御***】 end

End Class
