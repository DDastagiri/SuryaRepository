'------------------------------------------------------------------------------
'SC3180201.aspx.vb
'------------------------------------------------------------------------------
'機能： 完成検査承認画面
'補足： 
'作成： 2014/02/14 AZ宮澤	初版作成
'更新： 2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'------------------------------------------------------------------------------
Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization

Imports Toyota.eCRB.iCROP.DataAccess.SC3180201.SC3180201DataSet
Imports Toyota.eCRB.iCROP.DataAccess.SC3180201.SC3180201DataSetTableAdapter.SC3180201TableAdapter
Imports Toyota.eCRB.ServerCheck.CheckResult.BizLogic.SC3180201

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Imports Toyota.eCRB.Technician.MainMenu
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic.ServiceCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess

Imports Toyota.eCRB.DMSLinkage.StatusInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.Reserve.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.DataAccess

Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode

Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet


Partial Class Pages_Default
    Inherits BasePage
    Implements IDisposable

#Region "定数"
    ''' セッションキー
    Public Const SessionKeyStallId As String = "SC3180201.StallId"  'ストールID

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationId As String = "SC3180201"

    ''' <summary>
    ''' メインメニュー(SA)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdMainMenuSA As String = "SC3140103"
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
    Private Const ProgramIdMainMenuTC As String = "SC3150101"
    ''' <summary>
    ''' メインメニュー(FM)画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdMainMenuFM As String = "SC3230101"

    ''' <summary>
    ''' FMメイン画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdFMMain As String = "SC3230101"
    ''' <summary>
    ''' SMB画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdSMB As String = "SC3240101"        '工程管理
    ''' <summary>
    ''' 連絡先画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdContact As String = "SC3040601"
    ''' <summary>
    ''' TCメイン画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramIdTCMain As String = "SC3150101"
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
    ''' セッションキー(表示番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyDispNum As String = "Session.DISP_NUM"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター1)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyLinkageParam1 As String = "Session.Param1"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyLinkageParam2 As String = "Session.Param2"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター3)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyLinkageParam3 As String = "Session.Param3"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター4)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyLinkageParam4 As String = "Session.Param4"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター5)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyLinkageParam5 As String = "Session.Param5"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター6)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyLinkageParam6 As String = "Session.Param6"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター7)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyLinkageParam7 As String = "Session.Param7"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyLinkageParam8 As String = "Session.Param8"
    ''' <summary>
    ''' 他システム連携画面遷移セッションキー(パラメーター9)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyLinkageParam9 As String = "Session.Param9"

    ''' <summary>
    ''' SessionKey(R_O)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyRo As String = "R_O"
    ''' <summary>
    ''' SessionKey(VINNO)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyVinNo As String = "VIN_NO"
    ''' <summary>
    ''' SessionKey(ViewMode)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyViewMode As String = "ViewMode"
    ''' <summary>
    ''' SessionKey(JOB_DTL_ID)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyJobDtlId As String = "JOB_DTL_ID"

    ''' <summary>
    ''' セッション名("SAChipID")
    ''' </summary>
    Private Const SessionKeySAChipId As String = "SAChipID"
    ''' <summary>
    ''' セッション名("BASREZID")
    ''' </summary>
    Private Const SessionKeyBasrezId As String = "BASREZID"
    ''' <summary>
    ''' セッション名("SEQ_NO")
    ''' </summary>
    Private Const SessionKeySeqNo As String = "SEQ_NO"

    ''' <summary>
    ''' フッターコード：メインメニュー(SMB)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterMainMenu As Integer = 100
    ''' <summary>
    ''' フッターコード：TCメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterTechnicianMain As Integer = 200
    ''' <summary>
    ''' フッターコード：FMメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterForemanMain As Integer = 300
    ''' <summary>
    ''' フッターコード：R/Oボタン(R/O一覧)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterRo As Integer = 500
    ''' <summary>
    ''' フッターコード：連絡先
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FooterTelDirectory As Integer = 600
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
    ''' 検査部位コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PartCDEngine As String = "01"
    Private Const PartCDInroom As String = "02"
    Private Const PartCDLeft As String = "03"
    Private Const PartCDRight As String = "04"
    Private Const PartCDUnder As String = "05"
    Private Const PartCDTrunk As String = "06"


#Region "定数(マジック№対応)"

    Private Const ErrorFlgError As String = "1"                                 ' エラーフラグ

    Private Const AcceptanceTypeWalkin As String = "0"                          ' 受付区分（WalkIN）
    Private Const VipFlgTrue As String = "1"                                    ' VIP_FLG状態（あり）

    Private Const ConvertDateYMD As Long = 3                                    ' 日付フォーマット形態(YMD)
    Private Const ConvertDateMD As Long = 11                                    ' 日付フォーマット形態(MD)
    Private Const ConvertDateHM As Long = 14                                    ' 日付フォーマット形態(HM)

    Private Const VehicleChartNoEngine As String = "1"                          ' 選択位置（エンジンルーム）

    Private Const OracleExNumberTimeoutError As Long = 1013                     ' Oracle例外（タイムアウト）

    Private Const AprovalStatusAproveWorking As Long = 0                        ' 承認ステータス（承認作業中）
    Private Const AprovelStatusNotApprove As Long = 2                           ' 承認ステータス（否承認）
    Private Const AprovalStatusWaitingRecognition As Long = 1                   ' 承認ステータス（承認済み）
    Private Const AprovalStatusEtc As Long = 3                                  ' 承認ステータス（その他）

    Private Const InspectionStatusCompExaminationUncomplate As String = "0"     ' 完成検査ステータス（完成検査未完了）
    Private Const InspectionStatusCompExaminationComplate As String = "2"       '完成検査ステータス（完成検査完了）

    Private Const InspecItemModeNow As String = "1"                             ' 表示モード（現在）
    Private Const InspecItemModeFuture As String = "0"                          ' 表示モード（未来）
    Private Const InspecItemModePast As String = "2"                            ' 表示モード（過去）

    Private Const RegistModeRegist As String = "1"                              ' 登録モード（登録する）
    Private Const RegistModeUnregist As String = "0"                            ' 登録モード（登録しない）

    Private Const RoStatusWork As Long = 60                                     ' RO_STATUS（作業中）
    Private Const RoStatusCompExaminationRequest As Long = 65                   ' RO_STATUS（完成検査依頼中）
    Private Const RoStatusCompExaminationComplate As Long = 70                  ' RO_STATUS（完成検査完了）
    Private Const RoStatusDeliveryWait As Long = 80                             ' RO_STATUS（納車準備待ち）

    Private Const RoStatusProcDeliveryWait As Long = 1                          ' RO_STATUS（納車準備待ち以前）

    Private Const DispTextPermView As String = "1"                              ' テキストボックス表示状態（表示する）
    Private Const TextInputModeUninput As String = "0"                          ' テキスト入力モード（入力させない）
    Private Const TextInputModeInput As String = "1"                            ' テキスト入力モード（入力する）

    'Private Const RsltBeforeTextNull As String = "0"                            ' Before入力状態（未入力）
    'Private Const RsltAfterTextNull As String = "0"                             ' After入力状態（未入力）

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

    Private Const UnsetRowLockVer As Long = -1                                  ' 行ロックバージョン未設定値
    Private Const DefaultRowLockVer As Long = 0                                 ' 行ロックバージョン初期値

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

    ''' <summary>
    ''' メッセージID管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MsgID
        id22 = 22
        id23 = 23
        id37 = 37
        id38 = 38
        id39 = 39
        id40 = 40
        id54 = 54
        '2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応　Start 
        id56 = 56
        '2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応  End
        id900 = 900 'チップ停止エラー(＝共通関数内エラー時の共通メッセージ)
        '【***完成検査_排他制御***】 start
        id58 = 58
        '【***完成検査_排他制御***】 end
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
    End Enum

    '共通関数の戻り値にて、継続する値
    '0:正常終了
    '-9000:ワーニング
    Private arySuccessList() As Long = {0, -9000}

    '2019/06/27 TKM要件：納車実績未登録の場合表示しない Start
    ''' <summary>
    ''' 日付型項目のデフォルト値（年）：1900
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DafaultDateYear As Integer = 1900

#End Region

#End Region

#Region "メンバ変数"

    Protected vin As String = String.Empty                'VIN
    Protected jobDtlId As String = String.Empty         'JOB_DTL_ID
    Protected viewMode As String = String.Empty          'ViewMode
    Protected aMarkView As String = ""                   'Aマーク    

    'TKMローカル対応
    'Protected iMarkView As String = ""                   'Iマーク

    Protected roNum As String = String.Empty             'R/O番号
    Protected dealerCD As String = String.Empty        '販売店コード
    Protected branchCD As String = String.Empty        '店舗コード
    Protected account As String = String.Empty            '担当者
    Protected fromFMMainFlg As Boolean = False            '画面遷移元がFMMainかどうか(FMMainならTrue)
    Protected roStatus As String = ""                    'ROステータス

    Protected saChipId As String = ""                     '来店者実績連番
    Protected basrezId As String = ""                     'DMS予約ID
    Protected seqNo As String = ""                       'RO_JOB_SEQ

    'VehicleChartボタン色
    Protected maintenanceBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected engineRoomBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected inRoomBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected leftBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected rightBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected underBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    Protected trunkBtnColor As String = "background:-webkit-gradient(linear, left top, left bottom, from(#B3B1B1), to(#797878));"
    'VehicleChartボタン使用可不可
    Protected maintenanceBtnDisabled As String = "disabled"
    Protected engineRoomBtnDisabled As String = "disabled"
    Protected inRoomBtnDisabled As String = "disabled"
    Protected leftBtnDisabled As String = "disabled"
    Protected rightBtnDisabled As String = "disabled"
    Protected underBtnDisabled As String = "disabled"
    Protected trunkBtnDisabled As String = "disabled"
    'TechnicalAdvice
    Protected technicianAdvice As String = ""
    'Before/After表示文字列
    Protected beforeText As String = ""
    Protected afterText As String = ""
    '汎用
    Protected intPosIndex As Integer
    Protected intIndex As Integer
    Protected nowJobDtl As Decimal = -1
    Protected stallId As Decimal = -1
    Protected svcinId As Decimal = -1
    Protected intRecCount As Integer = 0
    Protected rowLockversion As String
    Protected svcinRowLockVersion As Long = 0
    Protected roRowLockVersion As Long = 0
    Protected trnRowLockVersion As Long = 0
    Protected rejectStallId As Decimal = -1

    ' ''' <summary>
    ' ''' ビジネスロジック
    ' ''' </summary>
    ' ''' <remarks></remarks>
    Private businessLogic As New SC3180201BusinessLogic

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

        Dim blnDataFind As Boolean = False

        'ユーザ情報の取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        '2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応　Start
        overText.Value = WebWordUtility.GetWord(MsgID.id56)
        '2019/4/19 [PUAT4226 アドバイスコメント上限対応]対応　End

        '【***完成検査_排他制御***】 start
        'If Not IsPostBack Then
        '【***完成検査_排他制御***】 END

            'VehicleChart選択番号
            VehicleChartNo.Value = ""

            UserName.Value = staffInfo.UserName

            '検索処理(ヘッダ情報)
            Dim dtHeaderInfo As SC3180201HederInfoDataTable
            dtHeaderInfo = businessLogic.GetHederInfo(dealerCD, branchCD, roNum)

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
                        aMarkView = " style=""visibility:hidden;"" "
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
                '        iMarkView = " style=""visibility:hidden;"" "
                '    End If
                'End If

                VINLabel.Text = vin
                '2019/06/27 TKM要件：納車実績未登録の場合表示しない Start
                If False = dtHeaderInfo(0).IsNull("RSLT_DELI_DATETIME") AndAlso _
                    DafaultDateYear < CDate(dtHeaderInfo(0).RSLT_DELI_DATETIME).Year Then   ' [サービス入庫].[実績納車日時]が未登録(1900年)の場合、値を出力しない
                    DeliveryDate.Text = DateTimeFunc.FormatDate(ConvertDateYMD, CDate(dtHeaderInfo(0).RSLT_DELI_DATETIME))
                End If
                '2019/06/27 TKM要件：納車実績未登録の場合表示しない End

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
                    svcinId = Long.Parse(dtHeaderInfo(0).SVCIN_ID)
                End If

                rowLockversion = ""
                If False = dtHeaderInfo(0).IsNull("SRV_ROW_LOCK_VERSION") Then
                    rowLockversion = ""
                    rowLockversion = dtHeaderInfo(0).SRV_ROW_LOCK_VERSION
                    If rowLockversion <> "" Then
                        svcinRowLockVersion = Long.Parse(rowLockversion)
                    End If
                End If
                If False = dtHeaderInfo(0).IsNull("RO_ROW_LOCK_VERSION") Then
                    rowLockversion = ""
                    rowLockversion = dtHeaderInfo(0).RO_ROW_LOCK_VERSION
                    If rowLockversion <> "" Then
                        roRowLockVersion = Long.Parse(rowLockversion)
                    End If
                End If
            End If

            beforeText = WebWordUtility.GetWord(MsgID.id22)
            afterText = WebWordUtility.GetWord(MsgID.id23)

            '検索処理(OperationName)
            Dim dtInspecCodeList As SC3180201InspecCodeListDataTable
            dtInspecCodeList = businessLogic.GetInspecCodeList(dealerCD, branchCD, roNum)
            InspecCodeList.DataSource = dtInspecCodeList
            InspecCodeList.DataBind()

            '2015/04/14 新販売店追加対応 start
            'マスタに指定販売店が登録されているか判定する
            Dim specifyDlrCdFlgs As Dictionary(Of String, Boolean) = businessLogic.GetDlrCdExistMst(roNum, dealerCD, branchCD)

            'ROステータスを取得
            'InspectCodeを取得(部位指定なし)＋ROステータス取得
            Dim dtInspecCode As SC3180201InspectCodeDataTable

            'dtInspecCode = businessLogic.GetAllInspecCode(dealerCD, branchCD, roNum, roStatus)
            dtInspecCode = businessLogic.GetAllInspecCode(dealerCD, branchCD, roNum, specifyDlrCdFlgs, roStatus)
            '2019/12/02 NCN吉川 TKM要件：型式対応 End

            ''EngineRoom
            Dim dtInspecCode_Engine As SC3180201InspectCodeDataTable
            dtInspecCode_Engine = CType(dtInspecCode.Clone, SC3180201InspectCodeDataTable)
            Dim rowEngine As DataRow
            For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCDEngine))
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

            ''Inroom
            Dim dtInspecCode_Inroom As SC3180201InspectCodeDataTable
            dtInspecCode_Inroom = CType(dtInspecCode.Clone, SC3180201InspectCodeDataTable)
            Dim rowInRoom As DataRow
            For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCDInroom))
                rowInRoom = dtInspecCode_Inroom.NewRow
                For n As Integer = 0 To rowSource.ItemArray.Length - 1
                    rowInRoom(n) = rowSource(n)
                Next
                dtInspecCode_Inroom.Rows.Add(rowInRoom)
            Next
            EditInspecCode(dtInspecCode_Inroom)
            inRoomBtnDisabled = "return false;"
            LeftCheckCount.Value = dtInspecCode_Inroom.Count.ToString
            If 0 < dtInspecCode_Inroom.Count Then
                inRoomBtnColor = "background:-webkit-gradient(linear, left top, left bottom, from(#FCBF05), to(#A17A03));"
                inRoomBtnDisabled = ""
                InroomLabel.Text = dtInspecCode_Inroom(0).PART_NAME
            End If
            InspecItemsList_Inroom.DataSource = dtInspecCode_Inroom
            InspecItemsList_Inroom.DataBind()

            ''Left
            Dim dtInspecCode_Left As SC3180201InspectCodeDataTable
            dtInspecCode_Left = CType(dtInspecCode.Clone, SC3180201InspectCodeDataTable)
            Dim rowLeft As DataRow
            For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCDLeft))
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

            ''Right
            Dim dtInspecCode_Right As SC3180201InspectCodeDataTable
            dtInspecCode_Right = CType(dtInspecCode.Clone, SC3180201InspectCodeDataTable)
            Dim rowRight As DataRow
            For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCDRight))
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

            ''Under
            Dim dtInspecCode_Under As SC3180201InspectCodeDataTable
            dtInspecCode_Under = CType(dtInspecCode.Clone, SC3180201InspectCodeDataTable)
            Dim rowUnder As DataRow
            For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCDUnder))
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

            ''Trunk
            Dim dtInspecCode_Trunk As SC3180201InspectCodeDataTable
            dtInspecCode_Trunk = CType(dtInspecCode.Clone, SC3180201InspectCodeDataTable)
            Dim rowTrunk As DataRow
            For Each rowSource In dtInspecCode.Select(String.Format("PART_CD={0}", PartCDTrunk))
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

            ''Maintenance
            Dim dtMainteCode As SC3180201MainteCodeListDataTable
            'dtMainteCode = businessLogic.GetMainteCodeList(dealerCD, branchCD, roNum)
            dtMainteCode = businessLogic.GetMainteCodeList(dealerCD, branchCD, roNum, specifyDlrCdFlgs)
            EditInspecCode(dtMainteCode)
            maintenanceBtnDisabled = "return false;"
            MaintenanceCheckCount.Value = dtMainteCode.Count.ToString
            If 0 < dtMainteCode.Count Then
                maintenanceBtnColor = "background:-webkit-gradient(linear, left top, left bottom, from(#66DA65), to(#228221));"
                maintenanceBtnDisabled = ""
            End If
            InspecItemsList_Maintenance.DataSource = dtMainteCode
            InspecItemsList_Maintenance.DataBind()

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
        'End If
        '【***完成検査_排他制御***】 end

        'RO番号をキーに、登録されているテクニシャンアドバイスを取得する
        technicianAdvice = businessLogic.GetAdviceContent(dealerCD, branchCD, roNum)
        '2014/09/09 複数チップが存在する場合、テクニシャンアドバイスが取得できない可能性が高い為、取得方法修正 End

        technicianAdvice = Server.HtmlEncode(technicianAdvice)

        If False = blnDataFind Then
            '読み込みエラーメッセージの取得
            ErrorMessage.Text = "Error"
            ErrorFlg.Value = ErrorFlgError
            ErrorMessage.Text = WebWordUtility.GetWord(MsgID.id38)
            '2019/11/27 ROステータスが空のとき検査項目を設定しない対応　NCN吉川　start
            'エラーメッセージを取得する
            Me.hdnErrorMsg.Value = WebWordUtility.GetWord(MsgID.id38)
            '2019/11/27 ROステータスが空のとき検査項目を設定しない対応　NCN吉川　end
        End If

        'チェックエラーメッセージの取得
        ItemCheckErrorMessage.Value = WebWordUtility.GetWord(MsgID.id37)

        '編集中メッセージの取得
        EditedMessage.Value = WebWordUtility.GetWord(MsgID.id39)

        'フッタボタンの初期化
        InitFooterButton(staffInfo)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
#End Region

#Region "ボタン処理(共通分)"

    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, _
                                                        ByRef category As FooterMenuCategory) As Integer()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return New Integer() {}

    End Function

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
        If inStaffInfo.OpeCD = Operation.FM Then
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

        ElseIf inStaffInfo.OpeCD = Operation.CHT Then
            'ChiefTechnician(GS)権限

            'TCメインボタンの設定
            Dim tcMainButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterTechnicianMain)
            If tcMainButton IsNot Nothing Then
                AddHandler tcMainButton.Click, AddressOf TCMainButton_Click
                tcMainButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterTechnicianMain.ToString(CultureInfo.CurrentCulture))
            End If

            'FMメインボタンの設定
            Dim fmMainButton As CommonMasterFooterButton = CType(Me.Master, CommonMasterPage).GetFooterButton(FooterForemanMain)
            If fmMainButton IsNot Nothing Then
                AddHandler fmMainButton.Click, AddressOf FMMainButton_Click
                fmMainButton.OnClientClick = String.Format(CultureInfo.CurrentCulture, FooterReplaceEvent, FooterForemanMain.ToString(CultureInfo.CurrentCulture))
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

        End If

        '電話帳ボタンの設定
        Dim telDirectoryButton As CommonMasterFooterButton = _
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterTelDirectory)
        AddHandler telDirectoryButton.Click, AddressOf AllManagmentButton_Click

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
            Me.RedirectNextScreen(ProgramIdMainMenuSA)

        ElseIf staffInfo.OpeCD = Operation.SM Then
            '全体管理に遷移する
            Me.RedirectNextScreen(ProgramIdAllManagment)

        ElseIf staffInfo.OpeCD = Operation.CT OrElse staffInfo.OpeCD = Operation.CHT Then
            '工程管理に遷移する
            Me.RedirectNextScreen(ProgramIdProcessControl)

        ElseIf staffInfo.OpeCD = Operation.TEC Then
            'メインメニュー(TC)に遷移する
            Me.RedirectNextScreen(ProgramIdMainMenuTC)

        ElseIf staffInfo.OpeCD = Operation.FM Then
            'メインメニュー(FM)に遷移する
            Me.RedirectNextScreen(ProgramIdMainMenuFM)

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

        Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionKeyRo, roNum)
        Me.SetValue(ScreenPos.Next, SessionKeyVinNo, vin)
        Me.SetValue(ScreenPos.Next, SessionKeyViewMode, viewMode)
        Me.RedirectNextScreen(ProgramIdAllManagment)

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

        Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionKeyRo, roNum)
        Me.SetValue(ScreenPos.Next, SessionKeyVinNo, vin)
        Me.SetValue(ScreenPos.Next, SessionKeyViewMode, viewMode)
        Me.RedirectNextScreen(ProgramIdSMB)

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

        Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionKeyRo, roNum)
        Me.SetValue(ScreenPos.Next, SessionKeyVinNo, vin)
        Me.SetValue(ScreenPos.Next, SessionKeyViewMode, viewMode)

        '権限によって遷移先を変える
        If staffInfo.OpeCD = Operation.FM Then
            'Foreman(FM)権限
        ElseIf staffInfo.OpeCD = Operation.CT Then
            'Controller(CT)権限
        ElseIf staffInfo.OpeCD = Operation.CHT Then
            'ChiefTechnician(GS)権限
            Me.RedirectNextScreen(ProgramIdTCMain)

        End If

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

        Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionKeyRo, roNum)
        Me.SetValue(ScreenPos.Next, SessionKeyVinNo, vin)
        Me.SetValue(ScreenPos.Next, SessionKeyViewMode, viewMode)

        '権限によって遷移先を変える
        If staffInfo.OpeCD = Operation.FM Then
            'Foreman(FM)権限
            Me.RedirectNextScreen(ProgramIdFMMain)

        ElseIf staffInfo.OpeCD = Operation.CT Then
            'Controller(CT)権限

        ElseIf staffInfo.OpeCD = Operation.CHT Then
            'ChiefTechnician(GS)権限
            Me.RedirectNextScreen(ProgramIdFMMain)

        End If

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

        Using biz As New SC3180201BusinessLogic

            'DMS情報取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

            'DMS情報のチェック
            If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                '取得できた場合
                '画面間パラメータを設定
                '表示番号
                Me.SetValue(ScreenPos.Next, SessionKeyDispNum, SessionDataDispNumRoList)

                'DMS販売店コード
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam1, dtDmsCodeMapDataTable(0).CODE1)

                'DMS店舗コード
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam2, dtDmsCodeMapDataTable(0).CODE2)

                'アカウント
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam3, dtDmsCodeMapDataTable(0).ACCOUNT)

                '来店実績連番
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam4, saChipId)

                'DMS予約ID
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam5, basrezId)

                'RO番号
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam6, roNum)

                'RO作業連番
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam7, seqNo)

                'VIN
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam8, vin)

                '編集モード
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam9, viewMode)

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

        Using biz As New SC3180201BusinessLogic

            'DMS情報取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = biz.GetDmsDealerData(staffInfo)

            'DMS情報のチェック
            If Not (IsNothing(dtDmsCodeMapDataTable)) Then
                '取得できた場合
                '画面間パラメータを設定
                '表示番号
                Me.SetValue(ScreenPos.Next, SessionKeyDispNum, SessionDataDispNumAddList)

                'DMS販売店コード
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam1, dtDmsCodeMapDataTable(0).CODE1)

                'DMS店舗コード
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam2, dtDmsCodeMapDataTable(0).CODE2)

                'アカウント
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam3, staffInfo.Account.Substring(0, staffInfo.Account.IndexOf("@")))

                '来店実績連番
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam4, saChipId)

                'DMS予約ID
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam5, basrezId)

                'RO番号
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam6, roNum)

                'RO作業連番
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam7, seqNo)

                'VIN
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam8, vin)

                '編集モード
                Me.SetValue(ScreenPos.Next, SessionKeyLinkageParam9, viewMode)

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
    ''' 連絡先ボタンを押した時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub ContactButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        'パラメータの取得
        GetParam()

        Me.SetValue(ScreenPos.Next, SessionKeyRo, roNum)
        Me.SetValue(ScreenPos.Next, SessionKeyVinNo, vin)
        Me.SetValue(ScreenPos.Next, SessionKeyViewMode, viewMode)

        '権限によって遷移先を変える
        If staffInfo.OpeCD = Operation.FM Then
            'Foreman(FM)権限
            Me.RedirectNextScreen(ProgramIdContact)

        ElseIf staffInfo.OpeCD = Operation.CT Then
            'Controller(CT)権限
            Me.RedirectNextScreen(ProgramIdContact)

        ElseIf staffInfo.OpeCD = Operation.CHT Then
            'ChiefTechnician(GS)権限
            Me.RedirectNextScreen(ProgramIdContact)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "ボタン処理(個別分)"

    ''' <summary>
    ''' Rejectボタン_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonReject_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonRejectWork.Click

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim globalResult As Boolean = True
        Dim intAprovalStatus As Integer = AprovalStatusEtc         '作業ステータス
        Dim strAdviceContent As String
        Dim updateTime As Date = DateTimeFunc.Now(dealerCD)
        '【***完成検査_排他制御***】 start
        Dim rockVer As Long = Long.Parse(Request.Form("SRV_RowLockVer"))
        Dim blnLockResult As Boolean = False
        '【***完成検査_排他制御***】 end

        'パラメータの取得
        GetParam()

        '【***完成検査_排他制御***】 start
        blnLockResult = businessLogic.CheckUpdateFinalInspection(rockVer, dealerCD, branchCD, roNum)

        If blnLockResult = True Then

        '完成検査結果データの取得
        Dim dtInspecItem As SC3180201RegistInfoDataTable = New SC3180201RegistInfoDataTable
        Dim dtMaintenance As SC3180201RegistInfoDataTable = New SC3180201RegistInfoDataTable
        GetRegistInfo(intAprovalStatus, dtInspecItem, dtMaintenance)

        'アドバイスコメントの取得
        strAdviceContent = Server.HtmlDecode(Request.Form("TechnicianAdvice"))

        '共通関数実行結果戻り値格納用変数
        Dim rtnGlobalResult As Long = ActionResult.Success

        '完成検査結果データ登録
        blnResult = businessLogic.RejectLogic(dealerCD, _
                                              branchCD, _
                                              roNum, _
                                              jobDtlId, _
                                              saChipId, _
                                              basrezId, _
                                              seqNo, _
                                              vin, _
                                              viewMode, _
                                              nowJobDtl, _
                                              svcinId, _
                                              stallId, _
                                              strAdviceContent, _
                                              dtInspecItem, _
                                              dtMaintenance, _
                                              account, _
                                              ApplicationId,
                                              rtnGlobalResult)


        '画面遷移
        If True = blnResult Then
            '処理成功
            '2014/06/16 通知&PUSH処理を別ロジックに変更　START　↓↓↓
            blnResult = businessLogic.NoticeAfterRejectLogic(dealerCD, _
                                                             branchCD, _
                                                             roNum, _
                                                             jobDtlId, _
                                                             saChipId, _
                                                             basrezId, _
                                                             seqNo, _
                                                             vin, _
                                                             viewMode, _
                                                             nowJobDtl, _
                                                             svcinId, _
                                                             stallId, _
                                                             strAdviceContent, _
                                                             dtInspecItem, _
                                                             dtMaintenance, _
                                                             account, _
                                                             ApplicationId)
            '2014/06/16 通知&PUSH処理を別ロジックに変更　END　　↑↑↑

            ''FMメインから来た場合と、通知履歴から来た場合で遷移先が異なる
            ''FMメイン画面ID:SC3230101:PROGRAM_ID_FM_MAIN
            ''通知履歴画面ID:SC3040801:PROGRAM_ID_NOTICE_HISTORY
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} RedirectPrevScreen " _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name))
            ' 2015/5/1 強制納車対応 警告表示 start
            If rtnGlobalResult = ActionResult.Success Then

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
                       , rtnGlobalResult.ToString))

            '共通関数内でのエラーか、エラーの種類を判断する
            If arySuccessList.Contains(rtnGlobalResult) Then

                '書き込みエラーメッセージの取得
                ErrorMessage.Text = "Error"
                ErrorFlg.Value = ErrorFlgError
                ErrorMessage.Text = WebWordUtility.GetWord(MsgID.id40)
                Me.hdnErrorMsg.Value = ErrorMessage.Text

            Else

                '共通関数内エラー
                ErrorMessage.Text = "Error"
                ErrorFlg.Value = ErrorFlgError
                ErrorMessage.Text = "Chip could not be finished."    'チップ停止失敗メッセージ(今は固定値をセット)
                Me.hdnErrorMsg.Value = ErrorMessage.Text
            End If

        End If
        Else
            'エラーメッセージの文言を取得
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
            ShowMessageBox(MsgID.id58)

        End If
        '【***完成検査_排他制御***】 end

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' Approveボタン_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ButtonApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonApproveWork.Click

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean
        Dim globalResult As Boolean = True
        Dim intAprovalStatus As Integer = AprovelStatusNotApprove         '作業ステータス
        Dim strAdviceContent As String
        Dim updateTime As Date = DateTimeFunc.Now(dealerCD)
        '【***完成検査_排他制御***】 start
        Dim rockVer As Long = Long.Parse(Request.Form("SRV_RowLockVer"))
        '【***完成検査_排他制御***】 end
        'パラメータの取得
		GetParam()

        ''【***完成検査_排他制御***】 start
        If businessLogic.CheckUpdateFinalInspection(rockVer, dealerCD, branchCD, roNum) = True Then
            ''【***完成検査_排他制御***】 END

        '完成検査結果データの取得
        Dim dtInspecItem As SC3180201RegistInfoDataTable = New SC3180201RegistInfoDataTable
        Dim dtMaintenance As SC3180201RegistInfoDataTable = New SC3180201RegistInfoDataTable
        GetRegistInfo(intAprovalStatus, dtInspecItem, dtMaintenance)

        'アドバイスコメントの取得
        strAdviceContent = Server.HtmlDecode(Request.Form("TechnicianAdvice"))

        '共通関数実行結果戻り値格納用変数
        Dim rtnGlobalResult As Long = ActionResult.Success
        '2014/07/16　最終チップ判定を戻り値に追加
        Dim blnLastChipFlg As Boolean = False

        '完成検査結果データ登録
        blnResult = businessLogic.ApproveLogic(dealerCD, _
                                               branchCD, _
                                               roNum, _
                                               jobDtlId, _
                                               saChipId, _
                                               basrezId, _
                                               seqNo, _
                                               vin, _
                                               viewMode, _
                                               nowJobDtl, _
                                               svcinId, _
                                               stallId, _
                                               strAdviceContent, _
                                               dtInspecItem, _
                                               dtMaintenance, _
                                               account, _
                                               ApplicationId,
                                               rtnGlobalResult, _
                                               blnLastChipFlg)


        ''画面遷移
        'If True = blnResult Then

        'エラーが発生していない場合、通知&PUSH処理を実行し、画面遷移する
        If blnResult And _
           arySuccessList.Contains(rtnGlobalResult) Then

            '2014/06/16 通知&PUSH処理を別ロジックに変更　START　↓↓↓
            '通知＆PUSH処理
            blnResult = businessLogic.NoticeAfterApproveLogic(dealerCD, _
                                                              branchCD, _
                                                              roNum, _
                                                              jobDtlId, _
                                                              saChipId, _
                                                              basrezId, _
                                                              seqNo, _
                                                              vin, _
                                                              viewMode, _
                                                              nowJobDtl, _
                                                              svcinId, _
                                                              stallId, _
                                                              strAdviceContent, _
                                                              dtInspecItem, _
                                                              dtMaintenance, _
                                                              account, _
                                                              ApplicationId, _
                                                              blnLastChipFlg)
            '2014/06/16 通知&PUSH処理を別ロジックに変更　END　　↑↑↑

            ''FMメインから来た場合と、通知履歴から来た場合で遷移先が異なる
            ''FMメイン画面ID:SC3230101:PROGRAM_ID_FM_MAIN
            ''通知履歴画面ID:SC3040801:PROGRAM_ID_NOTICE_HISTORY
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} RedirectPrevScreen " _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name))

            ' 2015/5/1 強制納車対応 警告表示 start
            If rtnGlobalResult = ActionResult.Success Then

                Me.RedirectPrevScreen()
            Else

                '基幹連携で警告が発生、メッセージを設定する
                hdnWarningMsg.Value = WebWordUtility.GetWord(MsgID.id54)
            End If
        Else
            '処理失敗

            '失敗ログ
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} Error [rtnGlobalResult={2}]" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , rtnGlobalResult.ToString))

            '共通関数内でのエラーか、エラーの種類を判断する
            If arySuccessList.Contains(rtnGlobalResult) Then

                '書き込みエラーメッセージの取得
                ErrorMessage.Text = "Error"
                ErrorFlg.Value = ErrorFlgError
                ErrorMessage.Text = WebWordUtility.GetWord(MsgID.id40)
                Me.hdnErrorMsg.Value = ErrorMessage.Text

            Else

                '共通関数内エラー
                ErrorMessage.Text = "Error"
                ErrorFlg.Value = ErrorFlgError
                ErrorMessage.Text = "Chip could not be finished."    'チップ停止失敗メッセージ(今は固定値をセット)
                Me.hdnErrorMsg.Value = ErrorMessage.Text
            End If

        End If

        Else
            'エラーメッセージの文言を取得
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "PageLoad", "initDisplay();", True)
            ShowMessageBox(MsgID.id58)

        End If
        ''【***完成検査_排他制御***】 end

        '' 2015/5/1 強制納車対応 警告表示 end
        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
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
        If Me.ContainsKey(ScreenPos.Current, SessionKeyRo) Then
            roNum = CType(Me.GetValue(ScreenPos.Current, SessionKeyRo, False), String).Trim()
        End If
        'RO_NUM = Request(SESSION_KEY_RO)
        '車両識別番号(VIN)
        If Me.ContainsKey(ScreenPos.Current, SessionKeyVinNo) Then
            vin = CType(Me.GetValue(ScreenPos.Current, SessionKeyVinNo, False), String).Trim()
        End If
        'VIN = Request(SESSION_KEY_VINNO)
        '作業内容ID(JOB_DTL_ID)
        If Me.ContainsKey(ScreenPos.Current, SessionKeyJobDtlId) Then
            jobDtlId = CType(Me.GetValue(ScreenPos.Current, SessionKeyJobDtlId, False), String).Trim()
        End If
        'ビューモード(ViewMode)
        If Me.ContainsKey(ScreenPos.Current, SessionKeyViewMode) Then
            viewMode = CType(Me.GetValue(ScreenPos.Current, SessionKeyViewMode, False), String).Trim()
        End If
        'VIEW_MODE = Request(SESSION_KEY_VIEWMODE)

        '来店者実績連番
        If Me.ContainsKey(ScreenPos.Current, SessionKeySAChipId) = True Then
            saChipId = DirectCast(GetValue(ScreenPos.Current, SessionKeySAChipId, False), String)
        End If
        'DMS予約ID
        If Me.ContainsKey(ScreenPos.Current, SessionKeyBasrezId) = True Then
            basrezId = DirectCast(GetValue(ScreenPos.Current, SessionKeyBasrezId, False), String)
        End If
        'RO_JOB_SEQ(親のRO_JOB_SEQ = 0)
        If Me.ContainsKey(ScreenPos.Current, SessionKeySeqNo) = True Then
            seqNo = DirectCast(GetValue(ScreenPos.Current, SessionKeySeqNo, False), String)
        End If

        '判定のため遷移元取得
        FromFMMain.Value = ""
        fromFMMainFlg = False
        'Dim nowReferer As String = Request.ServerVariables("HTTP_REFERER")
        'If 0 <= nowReferer.ToUpper.IndexOf(ProgramIdFMMain) Then
        Dim nowReferer As String = Me.GetPrevScreenId()
        If Not String.IsNullOrEmpty(nowReferer) And _
           0 <= nowReferer.ToUpper.IndexOf(ProgramIdFMMain) Then

            'FMメインからの遷移(自画面の再描画も考慮して)
            FromFMMain.Value = "1"
            fromFMMainFlg = True
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "SESSION  RO_NUM [{0}]:VIN [{1}]: JOB_DTL_ID[{2}]: VIEW_MODE[{3}]: SAChipID[{4}]: BASREZID[{5}]: SEQ_NO[{6}]" _
                  , roNum.ToString _
                  , vin.ToString _
                  , jobDtlId.ToString _
                  , viewMode.ToString _
                  , saChipId.ToString _
                  , basrezId.ToString _
                  , seqNo.ToString))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "FromFMMain [{0}]" _
                    , FromFMMain.Value.ToString))


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' データテーブルの再編
    ''' </summary>
    ''' <param name="dtInspecCode">データテーブル</param>
    ''' <remarks></remarks>
    Private Sub EditInspecCode(ByRef dtInspecCode As SC3180201InspectCodeDataTable)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検査項目
        Dim strInspecItemMode As String = ""
        Dim strInspecItemTextInputMode As String = ""
        Dim strInspecItemViewStyle_Color As String = ""
        Dim strInspecItemInputStyle As String = ""
        Dim strInspecItemInputStyle2 As String = ""
        Dim strInspecItemRegistMode As String = ""
        '検査項目名
        Dim strInspecItemName As String = ""
        Dim strNextInspecItemName As String = ""
        Dim strInspecItemNameViewStyle As String = ""
        'テキストボックス(Before,After)
        Dim blnInspecItemsText As Boolean = False
        Dim strInspecItemsTextViewStyle As String = ""
        Dim strInspecItemsTextBefore As String = ""
        Dim strInspecItemsTextAfter As String = ""
        '択一チェック項目(Good,Inspect,Replace,Fix,Cleaning,Swap)
        Dim intInspecItemsStatusCount As Integer = 0
        Dim strInspecItemsStatusViewStyle_Good As String = ""
        Dim strInspecItemsStatusViewStyle_Inspect As String = ""
        Dim strInspecItemsStatusViewStyle_Replace As String = ""
        Dim strInspecItemsStatusViewStyle_Fix As String = ""
        Dim strInspecItemsStatusViewStyle_Cleaning As String = ""
        Dim strInspecItemsStatusViewStyle_Swap As String = ""
        Dim strInspecItemsStatusViewPos_Good As String = ""
        Dim strInspecItemsStatusViewPos_Inspect As String = ""
        Dim strInspecItemsStatusViewPos_Replace As String = ""
        Dim strInspecItemsStatusViewPos_Fix As String = ""
        Dim strInspecItemsStatusViewPos_Cleaning As String = ""
        Dim strInspecItemsStatusViewPos_Swap As String = ""
        Dim strInspecItemsStatusSelect_Good As String = ""
        Dim strInspecItemsStatusSelect_Inspect As String = ""
        Dim strInspecItemsStatusSelect_Replace As String = ""
        Dim strInspecItemsStatusSelect_Fix As String = ""
        Dim strInspecItemsStatusSelect_Cleaning As String = ""
        Dim strInspecItemsStatusSelect_Swap As String = ""
        Dim strInspecItemsStatusColor_Good As String = ""
        Dim strInspecItemsStatusColor_Inspect As String = ""
        Dim strInspecItemsStatusColor_Replace As String = ""
        Dim strInspecItemsStatusColor_Fix As String = ""
        Dim strInspecItemsStatusColor_Cleaning As String = ""
        Dim strInspecItemsStatusColor_Swap As String = ""
        Dim strInspecItemsCheck As String = ""

        Dim strInspecItemsStatusViewStyle_No_Check As String = ""
        Dim strInspecItemsStatusViewPos_No_Check As String = ""
        Dim strInspecItemsStatusSelect_No_Check As String = ""
        Dim strInspecItemsStatusColor_No_Check As String = ""

        '複数選択リスト(Replaced,Fixed,Cleaned,Swapped)
        Dim strInspecItemsSelectViewStyle_Replaced As String = ""
        Dim strInspecItemsSelectViewStyle_Fixed As String = ""
        Dim strInspecItemsSelectViewStyle_Cleaned As String = ""
        Dim strInspecItemsSelectViewStyle_Swapped As String = ""

        'DB項目の追加
        ''検査項目
        dtInspecCode.Columns.Add("InspecItemMode", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemTextInputMode", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemViewStyle_Color", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemInputStyle", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemInputStyle2", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemRegistMode", Type.GetType("System.String"))
        ''検査項目名
        dtInspecCode.Columns.Add("InspecItemNameViewStyle", Type.GetType("System.String"))
        ''テキストボックス(Before,After)
        dtInspecCode.Columns.Add("InspecItemsTextViewStyle", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsTextBefore", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsTextAfter", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("HiddenAllData", Type.GetType("System.String"))
        ''択一チェック項目(Good,Inspect,Replace,Fix,Cleaning,Swap)
        dtInspecCode.Columns.Add("InspecItemsStatusViewStyle_Good", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewStyle_Inspect", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewStyle_Replace", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewStyle_Fix", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewStyle_Cleaning", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewStyle_Swap", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewPos_Good", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewPos_Inspect", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewPos_Replace", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewPos_Fix", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewPos_Cleaning", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewPos_Swap", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusSelect_Good", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusSelect_Inspect", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusSelect_Replace", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusSelect_Fix", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusSelect_Cleaning", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusSelect_Swap", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusColor_Good", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusColor_Inspect", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusColor_Replace", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusColor_Fix", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusColor_Cleaning", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusColor_Swap", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsCheck", Type.GetType("System.String"))

        dtInspecCode.Columns.Add("InspecItemsStatusViewStyle_No_Check", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusViewPos_No_Check", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusSelect_No_Check", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsStatusColor_No_Check", Type.GetType("System.String"))

        ''複数選択リスト(Replaced,Fixed,Cleaned,Swapped)
        dtInspecCode.Columns.Add("InspecItemsSelectViewStyle_Replaced", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsSelectViewStyle_Fixed", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsSelectViewStyle_Cleaned", Type.GetType("System.String"))
        dtInspecCode.Columns.Add("InspecItemsSelectViewStyle_Swapped", Type.GetType("System.String"))

        '行ロックバージョン
        dtInspecCode.Columns.Add("TrnRowLockVersion", Type.GetType("System.String"))


        '2014/06/27 項目追加　Start
        dtInspecCode.Columns.Add("InspecItemsSelect_Options", Type.GetType("System.String"))
        '2014/06/27 項目追加　End

        Dim strInspecItemCD As String = ""
        Dim intIdx As Integer = 0


        'ステータス状態セット
        Dim lngRoStatusProc As Long = 0

        '2019/11/27 ROステータスが空のとき検査項目を設定しない対応　NCN吉川　start
        If String.IsNullOrEmpty(roStatus) Then

            Return
        End If
        '2019/11/27 ROステータスが空のとき検査項目を設定しない対応　NCN吉川　end

        lngRoStatusProc = businessLogic.RoStatusCheck(Long.Parse(roStatus))
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} dtInspecCode.Count:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dtInspecCode.Count))

        '検査項目設定
        For intIdx = 0 To dtInspecCode.Count - 1
            '検査項目
            strInspecItemMode = InspecItemModeNow
            strInspecItemInputStyle = ""
            strInspecItemInputStyle2 = ""
            strInspecItemViewStyle_Color = "background:transparent;"
            strInspecItemRegistMode = "1"

            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '            , "★{0}.{1} INSPECTION_STATUSt:{2}" _
            '            , Me.GetType.ToString _
            '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '            , dtInspecCode(intIdx).INSPECTION_STATUS.ToString.Trim))

            If InspectionStatusCompExaminationUncomplate = dtInspecCode(intIdx).INSPECTION_STATUS.ToString.Trim Then
                '未来
                strInspecItemMode = InspecItemModeFuture
                strInspecItemInputStyle = "disabled"
                strInspecItemViewStyle_Color = "background:darkgray;"
                strInspecItemRegistMode = RegistModeUnregist
            ElseIf InspectionStatusCompExaminationComplate = dtInspecCode(intIdx).INSPECTION_STATUS.ToString.Trim Then
                '過去
                strInspecItemMode = InspecItemModePast
                strInspecItemViewStyle_Color = "background:lightgrey;"
            End If

            'ステータス状態による編集可/不可の決定
            If True = fromFMMainFlg Then
                ' FMMainからの遷移
                If lngRoStatusProc = RoStatusProcDeliveryWait Then
                    '納車より前(80:納車準備待ちまで):編集可(変更なし)
                Else
                    '納車より前(85:納車作業中以降):編集不可
                    strInspecItemInputStyle = "disabled"
                    strInspecItemRegistMode = RegistModeUnregist
                End If
            Else
                ' FMMain以外(通知履歴)からの遷移
                If lngRoStatusProc = RoStatusProcDeliveryWait Then
                    '納車より前(80:納車準備待ちまで):編集可(変更なし)
                Else
                    '納車より前(85:納車作業中以降):編集不可
                    strInspecItemInputStyle = "disabled"
                    strInspecItemRegistMode = RegistModeUnregist
                End If
            End If

            If strInspecItemInputStyle = "disabled" Then

                strInspecItemInputStyle2 = "disabled"

            ElseIf Not dtInspecCode(intIdx).IsINSPEC_RSLT_CDNull AndAlso _
                   dtInspecCode(intIdx).INSPEC_RSLT_CD.ToString = "7" Then

                strInspecItemInputStyle2 = "disabled"

            End If


            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '            , "★{0}.{1} InspecItemMode:{2}" _
            '            , Me.GetType.ToString _
            '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '            , strInspecItemMode))
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '            , "★{0}.{1} InspecItemInputStyle:{2}" _
            '            , Me.GetType.ToString _
            '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '            , strInspecItemInputStyle))
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '            , "★{0}.{1} InspecItemViewStyle_Color:{2}" _
            '            , Me.GetType.ToString _
            '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '            , strInspecItemViewStyle_Color))
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '            , "★{0}.{1} strInspecItemRegistMode:{2}" _
            '            , Me.GetType.ToString _
            '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '            , strInspecItemRegistMode))

            dtInspecCode(intIdx).Item("InspecItemMode") = strInspecItemMode
            dtInspecCode(intIdx).Item("InspecItemInputStyle") = strInspecItemInputStyle
            dtInspecCode(intIdx).Item("InspecItemInputStyle2") = strInspecItemInputStyle2
            dtInspecCode(intIdx).Item("InspecItemViewStyle_Color") = strInspecItemViewStyle_Color
            dtInspecCode(intIdx).Item("InspecItemRegistMode") = strInspecItemRegistMode

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


            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '             , "★{0}.{1} strInspecItemTextInputMode:{2}" _
            '             , Me.GetType.ToString _
            '             , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '             , strInspecItemTextInputMode))
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '             , "★{0}.{1} strInspecItemsTextBefore:{2}" _
            '             , Me.GetType.ToString _
            '             , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '             , strInspecItemsTextBefore))
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '             , "★{0}.{1} strInspecItemsTextAfter:{2}" _
            '             , Me.GetType.ToString _
            '             , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '             , strInspecItemsTextAfter))


            dtInspecCode(intIdx).Item("InspecItemTextInputMode") = strInspecItemTextInputMode
            dtInspecCode(intIdx).Item("InspecItemsTextBefore") = strInspecItemsTextBefore
            dtInspecCode(intIdx).Item("InspecItemsTextAfter") = strInspecItemsTextAfter

            '択一チェック項目(Good,Inspect,Replace,Fix,Cleaning,Swap)
            Dim cleanFlg As Boolean = False
            intInspecItemsStatusCount = 0
            strInspecItemsStatusViewStyle_Good = "display: none;"
            strInspecItemsStatusViewPos_Good = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NO_PROBLEM.ToString.Trim Then
                strInspecItemsStatusViewStyle_Good = ""
                strInspecItemsStatusViewPos_Good = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                intInspecItemsStatusCount += 1
            End If
            strInspecItemsStatusViewStyle_Inspect = "display: none;"
            strInspecItemsStatusViewPos_Inspect = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NEED_INSPEC.ToString.Trim Then
                strInspecItemsStatusViewStyle_Inspect = ""
                strInspecItemsStatusViewPos_Inspect = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                intInspecItemsStatusCount += 1
            End If
            strInspecItemsStatusViewStyle_Replace = "display: none;"
            strInspecItemsStatusViewPos_Replace = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NEED_REPLACE.ToString.Trim Then
                strInspecItemsStatusViewStyle_Replace = ""
                strInspecItemsStatusViewPos_Replace = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                intInspecItemsStatusCount += 1
            End If
            strInspecItemsStatusViewStyle_Fix = "display: none;"
            strInspecItemsStatusViewPos_Fix = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NEED_FIX.ToString.Trim Then
                strInspecItemsStatusViewStyle_Fix = ""
                strInspecItemsStatusViewPos_Fix = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                intInspecItemsStatusCount += 1
            End If
            strInspecItemsStatusViewStyle_Cleaning = "display: none;"
            strInspecItemsStatusViewPos_Cleaning = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NEED_CLEAN.ToString.Trim Then
                If False = blnInspecItemsText Or 4 > intInspecItemsStatusCount Then
                    strInspecItemsStatusViewStyle_Cleaning = ""
                    strInspecItemsStatusViewPos_Cleaning = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
                    cleanFlg = True
                    intInspecItemsStatusCount += 1
                End If
            End If
            strInspecItemsStatusViewStyle_Swap = "display: none;"
            strInspecItemsStatusViewPos_Swap = ""
            If SelectModeSelect.ToString = dtInspecCode(intIdx).DISP_INSPEC_ITEM_NEED_SWAP.ToString.Trim Then
                If False = blnInspecItemsText Or 4 > intInspecItemsStatusCount Then
                    strInspecItemsStatusViewStyle_Swap = ""
                    strInspecItemsStatusViewPos_Swap = GetInspecIconPosStyle(intInspecItemsStatusCount + 1, cleanFlg)
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
            dtInspecCode(intIdx).Item("InspecItemsStatusViewStyle_Good") = strInspecItemsStatusViewStyle_Good
            dtInspecCode(intIdx).Item("InspecItemsStatusViewStyle_Inspect") = strInspecItemsStatusViewStyle_Inspect
            dtInspecCode(intIdx).Item("InspecItemsStatusViewStyle_Replace") = strInspecItemsStatusViewStyle_Replace
            dtInspecCode(intIdx).Item("InspecItemsStatusViewStyle_Fix") = strInspecItemsStatusViewStyle_Fix
            dtInspecCode(intIdx).Item("InspecItemsStatusViewStyle_Cleaning") = strInspecItemsStatusViewStyle_Cleaning
            dtInspecCode(intIdx).Item("InspecItemsStatusViewStyle_Swap") = strInspecItemsStatusViewStyle_Swap
            dtInspecCode(intIdx).Item("InspecItemsStatusViewPos_Good") = strInspecItemsStatusViewPos_Good
            dtInspecCode(intIdx).Item("InspecItemsStatusViewPos_Inspect") = strInspecItemsStatusViewPos_Inspect
            dtInspecCode(intIdx).Item("InspecItemsStatusViewPos_Replace") = strInspecItemsStatusViewPos_Replace
            dtInspecCode(intIdx).Item("InspecItemsStatusViewPos_Fix") = strInspecItemsStatusViewPos_Fix
            dtInspecCode(intIdx).Item("InspecItemsStatusViewPos_Cleaning") = strInspecItemsStatusViewPos_Cleaning
            dtInspecCode(intIdx).Item("InspecItemsStatusViewPos_Swap") = strInspecItemsStatusViewPos_Swap
            dtInspecCode(intIdx).Item("InspecItemsStatusViewStyle_No_Check") = strInspecItemsStatusViewStyle_No_Check
            dtInspecCode(intIdx).Item("InspecItemsStatusViewPos_No_Check") = strInspecItemsStatusViewPos_No_Check

            strInspecItemsCheck = CheckModeUncheck
            strInspecItemsStatusSelect_Good = ""
            strInspecItemsStatusSelect_Inspect = ""
            strInspecItemsStatusSelect_Replace = ""
            strInspecItemsStatusSelect_Fix = ""
            strInspecItemsStatusSelect_Cleaning = ""
            strInspecItemsStatusSelect_Swap = ""
            strInspecItemsStatusColor_Good = "blue"
            strInspecItemsStatusColor_Inspect = "blue"
            strInspecItemsStatusColor_Replace = "blue"
            strInspecItemsStatusColor_Fix = "blue"
            strInspecItemsStatusColor_Cleaning = "blue"
            strInspecItemsStatusColor_Swap = "blue"

            strInspecItemsStatusSelect_No_Check = ""
            strInspecItemsStatusColor_No_Check = "blue"

            If False = dtInspecCode(intIdx).IsNull("INSPEC_RSLT_CD") Then
                strInspecItemsCheck = dtInspecCode(intIdx).INSPEC_RSLT_CD.ToString.Trim
                If CheckModeNoProblem = strInspecItemsCheck Then
                    strInspecItemsStatusSelect_Good = "checked"
                    strInspecItemsStatusColor_Good = "green"
                ElseIf CheckModeNeedInspection = strInspecItemsCheck Then
                    strInspecItemsStatusSelect_Inspect = "checked"
                    strInspecItemsStatusColor_Inspect = "green"
                ElseIf CheckModeNeedReplace = strInspecItemsCheck Then
                    strInspecItemsStatusSelect_Replace = "checked"
                    strInspecItemsStatusColor_Replace = "green"
                ElseIf CheckModeNeedFixing = strInspecItemsCheck Then
                    strInspecItemsStatusSelect_Fix = "checked"
                    strInspecItemsStatusColor_Fix = "green"
                ElseIf CheckModeNeedCleaning = strInspecItemsCheck Then
                    strInspecItemsStatusSelect_Cleaning = "checked"
                    strInspecItemsStatusColor_Cleaning = "green"
                ElseIf CheckModeNeedSwapping = strInspecItemsCheck Then
                    strInspecItemsStatusSelect_Swap = "checked"
                    strInspecItemsStatusColor_Swap = "green"
                ElseIf CheckModeNoCheck = strInspecItemsCheck Then
                    strInspecItemsStatusSelect_No_Check = "checked"
                    strInspecItemsStatusColor_No_Check = "green"
                Else
                    strInspecItemsCheck = CheckModeUncheck
                End If
            End If
            dtInspecCode(intIdx).Item("InspecItemsCheck") = strInspecItemsCheck
            dtInspecCode(intIdx).Item("InspecItemsStatusSelect_Good") = strInspecItemsStatusSelect_Good
            dtInspecCode(intIdx).Item("InspecItemsStatusSelect_Inspect") = strInspecItemsStatusSelect_Inspect
            dtInspecCode(intIdx).Item("InspecItemsStatusSelect_Replace") = strInspecItemsStatusSelect_Replace
            dtInspecCode(intIdx).Item("InspecItemsStatusSelect_Fix") = strInspecItemsStatusSelect_Fix
            dtInspecCode(intIdx).Item("InspecItemsStatusSelect_Cleaning") = strInspecItemsStatusSelect_Cleaning
            dtInspecCode(intIdx).Item("InspecItemsStatusSelect_Swap") = strInspecItemsStatusSelect_Swap
            dtInspecCode(intIdx).Item("InspecItemsStatusColor_Good") = strInspecItemsStatusColor_Good
            dtInspecCode(intIdx).Item("InspecItemsStatusColor_Inspect") = strInspecItemsStatusColor_Inspect
            dtInspecCode(intIdx).Item("InspecItemsStatusColor_Replace") = strInspecItemsStatusColor_Replace
            dtInspecCode(intIdx).Item("InspecItemsStatusColor_Fix") = strInspecItemsStatusColor_Fix
            dtInspecCode(intIdx).Item("InspecItemsStatusColor_Cleaning") = strInspecItemsStatusColor_Cleaning
            dtInspecCode(intIdx).Item("InspecItemsStatusColor_Swap") = strInspecItemsStatusColor_Swap

            dtInspecCode(intIdx).Item("InspecItemsStatusSelect_No_Check") = strInspecItemsStatusSelect_No_Check
            dtInspecCode(intIdx).Item("InspecItemsStatusColor_No_Check") = strInspecItemsStatusColor_No_Check
            '2014/06/27 削除　Start
            ''複数選択リスト(Replaced,Fixed,Cleaned,Swapped)
            'strInspecItemsSelectViewStyle_Replaced = ""
            'If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_REPLACE") Then
            '    If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_REPLACE.ToString.Trim Then
            '        strInspecItemsSelectViewStyle_Replaced = "selected"
            '    End If
            'End If
            'strInspecItemsSelectViewStyle_Fixed = ""
            'If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_FIX") Then
            '    If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_FIX.ToString.Trim Then
            '        strInspecItemsSelectViewStyle_Fixed = "selected"
            '    End If
            'End If
            'strInspecItemsSelectViewStyle_Cleaned = ""
            'If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_CLEAN") Then
            '    If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_CLEAN.ToString.Trim Then
            '        strInspecItemsSelectViewStyle_Cleaned = "selected"
            '    End If
            'End If
            'strInspecItemsSelectViewStyle_Swapped = ""
            'If False = dtInspecCode(intIdx).IsNull("OPERATION_RSLT_ALREADY_SWAP") Then
            '    If SelectModeSelect.ToString = dtInspecCode(intIdx).OPERATION_RSLT_ALREADY_SWAP.ToString.Trim Then
            '        strInspecItemsSelectViewStyle_Swapped = "selected"
            '    End If
            'End If

            'dtInspecCode(intIdx).Item("InspecItemsSelectViewStyle_Replaced") = strInspecItemsSelectViewStyle_Replaced
            'dtInspecCode(intIdx).Item("InspecItemsSelectViewStyle_Fixed") = strInspecItemsSelectViewStyle_Fixed
            'dtInspecCode(intIdx).Item("InspecItemsSelectViewStyle_Cleaned") = strInspecItemsSelectViewStyle_Cleaned
            'dtInspecCode(intIdx).Item("InspecItemsSelectViewStyle_Swapped") = strInspecItemsSelectViewStyle_Swapped
            '2014/06/27 削除　End

            '2014/06/27 項目追加　Start
            Dim InspecItemsSelectOptions As String = GetInspecItemsSelect_Options(dtInspecCode(intIdx))
            dtInspecCode(intIdx).Item("InspecItemsSelect_Options") = InspecItemsSelectOptions
            '2014/06/27 項目追加　End

            '行ロックバージョンの取得
            If False = dtInspecCode(intIdx).IsNull("TRN_ROW_LOCK_VERSION") Then
                dtInspecCode(intIdx).Item("TrnRowLockVersion") = dtInspecCode(intIdx).TRN_ROW_LOCK_VERSION.ToString.Trim
            Else
                dtInspecCode(intIdx).Item("TrnRowLockVersion") = UnsetRowLockVer
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
                                                        SelectorValue.ToString

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
    Private Function GetInspecItemsSelect_Options(ByVal dtInspecCodeRow As SC3180201InspectCodeRow) As String
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
    Private Sub EditInspecCode(ByRef dtMainteCode As SC3180201MainteCodeListDataTable)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メンテ項目
        Dim strMainteMode As String = ""
        Dim strMainteViewStyle_Color As String = ""
        Dim strMainteInputStyle As String = ""
        Dim strMainteRegistMode As String = ""
        '択一チェック項目(未実施,実施)
        Dim strMainteSelect_UncarriedOut As String = ""
        Dim strMainteSelect_Enforcement As String = ""
        Dim strMainteCheck As String = ""

        'DB項目の追加
        ''検査項目
        dtMainteCode.Columns.Add("MainteMode", Type.GetType("System.String"))
        dtMainteCode.Columns.Add("MainteViewStyle_Color", Type.GetType("System.String"))
        dtMainteCode.Columns.Add("MainteInputStyle", Type.GetType("System.String"))
        dtMainteCode.Columns.Add("MainteRegistMode", Type.GetType("System.String"))
        ''択一チェック項目(未実施,実施)
        dtMainteCode.Columns.Add("MainteSelect_UncarriedOut", Type.GetType("System.String"))
        dtMainteCode.Columns.Add("MainteSelect_Enforcement", Type.GetType("System.String"))
        dtMainteCode.Columns.Add("MainteCheck", Type.GetType("System.String"))

        '行ロックバージョン
        dtMainteCode.Columns.Add("TrnRowLockVersion", Type.GetType("System.String"))

        Dim strInspecItemCD As String = ""
        Dim intIdx As Integer = 0
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} dtMainteCode.Count:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dtMainteCode.Count))


        'メンテ項目設定
        For intIdx = 0 To dtMainteCode.Count - 1
            'メンテ項目
            strMainteMode = InspecItemModeNow
            strMainteViewStyle_Color = "background:transparent;"
            strMainteInputStyle = ""
            strMainteRegistMode = RegistModeRegist

            If InspectionStatusCompExaminationUncomplate = dtMainteCode(intIdx).INSPECTION_STATUS.ToString.Trim Then
                '未来
                strMainteMode = InspecItemModeFuture
                strMainteInputStyle = "disabled"
                strMainteViewStyle_Color = "background:darkgray;"
                strMainteRegistMode = RegistModeUnregist
            ElseIf InspectionStatusCompExaminationComplate = dtMainteCode(intIdx).INSPECTION_STATUS.ToString.Trim Then
                '過去
                strMainteMode = InspecItemModePast
                strMainteViewStyle_Color = "background:lightgrey;"
            End If

            'ステータス状態による編集可/不可の決定
            If True = fromFMMainFlg Then
                ' FMMainからの遷移
                If RoStatusDeliveryWait >= Integer.Parse(roStatus) Then
                    '納車より前(80:納車準備待ちまで):編集可(変更なし)
                Else
                    '納車より前(85:納車作業中以降):編集不可
                    strMainteInputStyle = "disabled"
                    strMainteRegistMode = RegistModeUnregist
                End If
            Else
                ' FMMain以外(通知履歴)からの遷移
                If RoStatusDeliveryWait >= Integer.Parse(roStatus) Then
                    '納車より前(80:納車準備待ちまで):編集可(変更なし)
                Else
                    '納車より前(85:納車作業中以降):編集不可
                    strMainteInputStyle = "disabled"
                    strMainteRegistMode = RegistModeUnregist
                End If
            End If

            dtMainteCode(intIdx).Item("MainteMode") = strMainteMode
            dtMainteCode(intIdx).Item("MainteViewStyle_Color") = strMainteViewStyle_Color
            dtMainteCode(intIdx).Item("MainteInputStyle") = strMainteInputStyle
            dtMainteCode(intIdx).Item("MainteRegistMode") = strMainteRegistMode

            '択一チェック項目(未実施,実施)
            strMainteCheck = CheckModeUncheck
            strMainteSelect_UncarriedOut = ""
            strMainteSelect_Enforcement = ""
            If False = dtMainteCode(intIdx).IsNull("INSPEC_RSLT_CD") Then
                strMainteCheck = dtMainteCode(intIdx).INSPEC_RSLT_CD.ToString.Trim
                If CheckModeEnforcement = strMainteCheck Then
                    strMainteSelect_UncarriedOut = "checked"
                    strMainteSelect_Enforcement = ""
                ElseIf CheckModeUncarriedOut = strMainteCheck Then
                    strMainteSelect_UncarriedOut = ""
                    strMainteSelect_Enforcement = "checked"
                Else
                    strMainteCheck = CheckModeUncheck
                End If
            End If
            dtMainteCode(intIdx).Item("MainteCheck") = strMainteCheck
            dtMainteCode(intIdx).Item("MainteSelect_UncarriedOut") = strMainteSelect_UncarriedOut
            dtMainteCode(intIdx).Item("MainteSelect_Enforcement") = strMainteSelect_Enforcement

            '行ロックバージョンの取得
            If False = dtMainteCode(intIdx).IsNull("TRN_ROW_LOCK_VERSION") Then
                dtMainteCode(intIdx).Item("TrnRowLockVersion") = dtMainteCode(intIdx).TRN_ROW_LOCK_VERSION.ToString.Trim
            Else
                dtMainteCode(intIdx).Item("TrnRowLockVersion") = UnsetRowLockVer
            End If

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
    ''' <param name="cleanFlg">クリーンアイコン有無</param>
    ''' <remarks></remarks>
    Private Function GetInspecIconPosStyle(ByVal intIndex As Integer, ByVal cleanFlg As Boolean) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim strResult As String = ""

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
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

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
                                   ByRef dtInspecItem As SC3180201RegistInfoDataTable, _
                                   ByRef dtMaintenance As SC3180201RegistInfoDataTable) As Boolean

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim blnResult As Boolean = True

        Dim strJobDtlID As String                   '作業内容ID
        Dim decJobDtlId As Decimal
        Dim strJobInstructID As String              '作業指示ID
        Dim strJobInstructSeq As String             '作業指示枝番
        Dim lngJobInstructSeq As Long
        Dim strInspecItemCD As String               '点検項目コード
        Dim hiddendataId As String
        Dim intPosIndex As Integer
        Dim intIndex As Integer
        Dim strInspecItemMode As String
        Dim strInspecItemRegistMode As String
        Dim strInspecItemsCheck As String
        Dim lngInspecItemsCheck As Long
        Dim strInspecItemsTextBefore As String
        Dim decInspecItemsTextBefore As Decimal
        Dim strInspecItemsTextAfter As String
        Dim decInspecItemsTextAfter As Decimal
        Dim lngInspecItemsSelect_Replaced As Long
        Dim lngInspecItemsSelect_Fixed As Long
        Dim lngInspecItemsSelect_Cleaned As Long
        Dim lngInspecItemsSelect_Swapped As Long
        Dim updateTime As Date = DateTimeFunc.Now(dealerCD)

        'サービスIDの取得
        svcinId = Decimal.Parse(Request.Form("ServiceID"))
        roRowLockVersion = Long.Parse(Request.Form("RO_RowLockVer"))

        For intPosIndex = PartIndexEngine To PartIndexMaintenance
            If Request.Form("StallUseID" & intPosIndex & "_1") IsNot Nothing Then
                rejectStallId = Decimal.Parse(Request.Form("StallUseID" & intPosIndex & "_1"))
            End If
        Next


        'InspecItems
        For intPosIndex = PartIndexEngine To PartIndexTrunk
            intIndex = 1
            hiddendataId = "HiddenAllData" & intPosIndex.ToString & "_" & intIndex.ToString
            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} ID:{2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , hiddendataId))
            Do While Request.Form(hiddendataId) IsNot Nothing
                '登録モード
                Dim hiddenDataList As String() = Request.Form(hiddendataId).Split("|"c)

                strInspecItemMode = hiddenDataList(HiddenDataNo.InspecItemMode)
                strInspecItemRegistMode = hiddenDataList(HiddenDataNo.InspecItemRegistMode)
                If RegistModeRegist = strInspecItemRegistMode And (InspecItemModeNow = strInspecItemMode Or InspecItemModePast = strInspecItemMode) Then
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
                    lngInspecItemsSelect_Replaced = 0
                    lngInspecItemsSelect_Fixed = 0
                    lngInspecItemsSelect_Cleaned = 0
                    lngInspecItemsSelect_Swapped = 0
                    Dim stArrayData() As String
                    stArrayData = Split(Request.Form("InspecItemsSelector" & intPosIndex.ToString & "_" & intIndex.ToString), ",")
                    For idx = 0 To UBound(stArrayData)
                        If AlreadyReplaceIdx = stArrayData(idx) Then
                            lngInspecItemsSelect_Replaced = CheckModeCheck
                        ElseIf AlreadyFixIdx = stArrayData(idx) Then
                            lngInspecItemsSelect_Fixed = CheckModeCheck
                        ElseIf AlreadyCleanIdx = stArrayData(idx) Then
                            lngInspecItemsSelect_Cleaned = CheckModeCheck
                        ElseIf AlreadySwapIdx = stArrayData(idx) Then
                            lngInspecItemsSelect_Swapped = CheckModeCheck
                        End If
                    Next idx

                    '作業内容ID
                    strJobDtlID = hiddenDataList(HiddenDataNo.JobDtlID)
                    decJobDtlId = Decimal.Parse(strJobDtlID)
                    'SESSIONから作業内容IDががあるか確認
                    If jobDtlId <> String.Empty Then
                        '完成検査承認が "1" の時 かつ、SESSIONの作業内容IDと一致する場合　True
                        If InspecItemModeNow = strInspecItemMode And Decimal.Parse(jobDtlId) = decJobDtlId Then
                            nowJobDtl = decJobDtlId
                            stallId = Decimal.Parse(hiddenDataList(HiddenDataNo.StallUseID))
                        End If
                    End If
                    '作業指示ID
                    strJobInstructID = hiddenDataList(HiddenDataNo.JobInstructID)
                    '作業指示枝番
                    strJobInstructSeq = hiddenDataList(HiddenDataNo.JobInstructSeq)
                    lngJobInstructSeq = Long.Parse(strJobInstructSeq)
                    '点検項目コード
                    strInspecItemCD = hiddenDataList(HiddenDataNo.InspecItemCD)
                    '行ロックバージョンの取得
                    rowLockversion = ""
                    rowLockversion = hiddenDataList(HiddenDataNo.TRN_RowLockVer)
                    If rowLockversion <> "" Then
                        trnRowLockVersion = Long.Parse(rowLockversion)
                    Else
                        trnRowLockVersion = DefaultRowLockVer
                    End If

                    '登録情報の格納
                    '2014.6.28 Edit Start JOB_INSTRUCT_ID/JOB_INSTRUCT_SEQの値設定が固定値「0」になっているため
                    'データから取れた値を適切にセット
                    dtInspecItem.Rows.Add(decJobDtlId, _
                                          intaprovalStatus, _
                                          trnRowLockVersion, _
                                          strJobInstructID, _
                                          strJobInstructSeq, _
                                          strInspecItemCD, _
                                          lngInspecItemsCheck, _
                                          lngInspecItemsSelect_Replaced, _
                                          lngInspecItemsSelect_Fixed, _
                                          lngInspecItemsSelect_Cleaned, _
                                          lngInspecItemsSelect_Swapped, _
                                          decInspecItemsTextBefore, _
                                          decInspecItemsTextAfter)
                    '2014.6.28 Edit End JOB_INSTRUCT_ID/JOB_INSTRUCT_SEQの値設定が固定値「0」になっているため

                End If

                intIndex += 1
                hiddendataId = "HiddenAllData" & intPosIndex.ToString & "_" & intIndex.ToString
            Loop
            'strID = "InspecItemsCheck" & intPosIndex.ToString & "_" & intIndex.ToString
            ''開始ログ
            'Logger.Info(String.Format(CultureInfo.CurrentCulture _
            '          , "{0}.{1} ID:{2}" _
            '          , Me.GetType.ToString _
            '          , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '          , strID))
            'Do While Request.Form(strID) IsNot Nothing
            '    '登録モード
            '    strInspecItemMode = Request.Form("InspecItemMode" & intPosIndex.ToString & "_" & intIndex.ToString)
            '    strInspecItemRegistMode = Request.Form("InspecItemRegistMode" & intPosIndex.ToString & "_" & intIndex.ToString)
            '    If RegistModeRegist = strInspecItemRegistMode And (InspecItemModeNow = strInspecItemMode Or InspecItemModePast = strInspecItemMode) Then
            '        '択一チェック項目(Good,Inspect,Replace,Fix,Cleaning,Swap)
            '        strID = "InspecItemsCheck" & intPosIndex.ToString & "_" & intIndex.ToString
            '        strInspecItemsCheck = Request.Form(strID)
            '        lngInspecItemsCheck = Long.Parse(strInspecItemsCheck)
            '        'テキストボックス(Before,After)
            '        strInspecItemsTextBefore = Request.Form("BeforeText" & intPosIndex.ToString & "_" & intIndex.ToString)
            '        decInspecItemsTextBefore = DefaultBeforeText
            '        If True = IsNumeric(strInspecItemsTextBefore) Then
            '            decInspecItemsTextBefore = Decimal.Parse(strInspecItemsTextBefore)
            '        End If
            '        strInspecItemsTextAfter = Request.Form("AfterText" & intPosIndex.ToString & "_" & intIndex.ToString)
            '        decInspecItemsTextAfter = DefaultAfterText
            '        If True = IsNumeric(strInspecItemsTextAfter) Then
            '            decInspecItemsTextAfter = Decimal.Parse(strInspecItemsTextAfter)
            '        End If
            '        '複数選択リスト(Replaced,Fixed,Cleaned,Swapped)
            '        lngInspecItemsSelect_Replaced = 0
            '        lngInspecItemsSelect_Fixed = 0
            '        lngInspecItemsSelect_Cleaned = 0
            '        lngInspecItemsSelect_Swapped = 0
            '        Dim stArrayData() As String
            '        stArrayData = Split(Request.Form("InspecItemsSelector" & intPosIndex.ToString & "_" & intIndex.ToString), ",")
            '        For idx = 0 To UBound(stArrayData)
            '            If AlreadyReplaceIdx = stArrayData(idx) Then
            '                lngInspecItemsSelect_Replaced = CheckModeCheck
            '            ElseIf AlreadyFixIdx = stArrayData(idx) Then
            '                lngInspecItemsSelect_Fixed = CheckModeCheck
            '            ElseIf AlreadyCleanIdx = stArrayData(idx) Then
            '                lngInspecItemsSelect_Cleaned = CheckModeCheck
            '            ElseIf AlreadySwapIdx = stArrayData(idx) Then
            '                lngInspecItemsSelect_Swapped = CheckModeCheck
            '            End If
            '        Next idx

            '        '作業内容ID
            '        strJobDtlID = Request.Form("JobDtlID" & intPosIndex.ToString & "_" & intIndex.ToString)
            '        decJobDtlId = Decimal.Parse(strJobDtlID)
            '        'SESSIONから作業内容IDががあるか確認
            '        If jobDtlId <> String.Empty Then
            '            '完成検査承認が "1" の時 かつ、SESSIONの作業内容IDと一致する場合　True
            '            If InspecItemModeNow = strInspecItemMode And Decimal.Parse(jobDtlId) = decJobDtlId Then
            '                nowJobDtl = decJobDtlId
            '                stallId = Decimal.Parse(Request.Form("StallUseID" & intPosIndex.ToString & "_" & intIndex.ToString))
            '            End If
            '        End If
            '        '作業指示ID
            '        strJobInstructID = Request.Form("JobInstructID" & intPosIndex.ToString & "_" & intIndex.ToString)
            '        '作業指示枝番
            '        strJobInstructSeq = Request.Form("JobInstructSeq" & intPosIndex.ToString & "_" & intIndex.ToString)
            '        lngJobInstructSeq = Long.Parse(strJobInstructSeq)
            '        '点検項目コード
            '        strInspecItemCD = Request.Form("InspecItemCD" & intPosIndex.ToString & "_" & intIndex.ToString)
            '        '行ロックバージョンの取得
            '        rowLockversion = ""
            '        rowLockversion = Request.Form("TRN_RowLockVer" & intPosIndex.ToString & "_" & intIndex.ToString)
            '        If rowLockversion <> "" Then
            '            trnRowLockVersion = Long.Parse(rowLockversion)
            '        Else
            '            trnRowLockVersion = DefaultRowLockVer
            '        End If

            '        '登録情報の格納
            '        '2014.6.28 Edit Start JOB_INSTRUCT_ID/JOB_INSTRUCT_SEQの値設定が固定値「0」になっているため
            '        'データから取れた値を適切にセット
            '        'dtInspecItem.Rows.Add(decJobDtlId, _
            '        '                      intaprovalStatus, _
            '        '                      trnRowLockVersion, _
            '        '                      DefaultJobInspectId, _
            '        '                      DefaultJobInspectSeq, _
            '        '                      strInspecItemCD, _
            '        '                      lngInspecItemsCheck, _
            '        '                      lngInspecItemsSelect_Replaced, _
            '        '                      lngInspecItemsSelect_Fixed, _
            '        '                      lngInspecItemsSelect_Cleaned, _
            '        '                      lngInspecItemsSelect_Swapped, _
            '        '                      decInspecItemsTextBefore, _
            '        '                      decInspecItemsTextAfter)

            '        dtInspecItem.Rows.Add(decJobDtlId, _
            '                              intaprovalStatus, _
            '                              trnRowLockVersion, _
            '                              strJobInstructID, _
            '                              strJobInstructSeq, _
            '                              strInspecItemCD, _
            '                              lngInspecItemsCheck, _
            '                              lngInspecItemsSelect_Replaced, _
            '                              lngInspecItemsSelect_Fixed, _
            '                              lngInspecItemsSelect_Cleaned, _
            '                              lngInspecItemsSelect_Swapped, _
            '                              decInspecItemsTextBefore, _
            '                              decInspecItemsTextAfter)
            '        '2014.6.28 Edit End JOB_INSTRUCT_ID/JOB_INSTRUCT_SEQの値設定が固定値「0」になっているため

            '    End If

            '    intIndex += 1
            '    strID = "InspecItemsCheck" & intPosIndex.ToString & "_" & intIndex.ToString
            'Loop

        Next

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
                If RegistModeRegist = strInspecItemRegistMode And (InspecItemModeNow = strInspecItemMode Or InspecItemModePast = strInspecItemMode) Then
                    '択一チェック項目(Good,Inspect,Replace,Fix,Cleaning,Swap)
                    strID = "Maintenance" & intPosIndex.ToString & "_" & intIndex.ToString
                    strInspecItemsCheck = Request.Form(strID)
                    If strInspecItemsCheck Is Nothing Then
                        strInspecItemsCheck = CheckModeUncheck
                    End If
                    lngInspecItemsCheck = Long.Parse(strInspecItemsCheck)

                    '作業内容ID
                    strJobDtlID = Request.Form("JobDtlID" & intPosIndex.ToString & "_" & intIndex.ToString)
                    decJobDtlId = Decimal.Parse(strJobDtlID)
                    'SESSIONから作業内容IDががあるか確認
                    If jobDtlId <> String.Empty Then
                        '完成検査承認が "1" の時 かつ、SESSIONの作業内容IDと一致する場合　True
                        If InspecItemModeNow = strInspecItemMode And Decimal.Parse(jobDtlId) = decJobDtlId Then
                            nowJobDtl = decJobDtlId
                            stallId = Decimal.Parse(Request.Form("StallUseID" & intPosIndex.ToString & "_" & intIndex.ToString))
                        End If
                    End If
                    '作業指示ID
                    strJobInstructID = Request.Form("JobInstructID" & intPosIndex.ToString & "_" & intIndex.ToString)
                    '作業指示枝番
                    strJobInstructSeq = Request.Form("JobInstructSeq" & intPosIndex.ToString & "_" & intIndex.ToString)
                    lngJobInstructSeq = Long.Parse(strJobInstructSeq)
                    '行ロックバージョンの取得
                    rowLockversion = ""
                    rowLockversion = Request.Form("TRN_RowLockVer" & intPosIndex.ToString & "_" & intIndex.ToString)
                    If rowLockversion <> "" Then
                        trnRowLockVersion = Long.Parse(rowLockversion)
                    End If

                    '登録情報の格納
                    dtMaintenance.Rows.Add(decJobDtlId, _
                                           intaprovalStatus, _
                                           trnRowLockVersion, _
                                           strJobInstructID, _
                                           lngJobInstructSeq, _
                                           DefaultItemCD, _
                                           lngInspecItemsCheck, _
                                           DefaultAlreadyReplace, _
                                           DefaultAlreadyFix, _
                                           DefaultAlreadyClean, _
                                           DefaultAlreadySwap, _
                                           DefaultBeforeText, _
                                           DefaultAfterText)
                End If

                intIndex += 1
                strID = "MainteCheck" & intPosIndex.ToString & "_" & intIndex.ToString
            Loop
        End If


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} END" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return blnResult

    End Function

    ' 2015/5/1 強制納車対応 警告表示後の前画面遷移リクエスト start
    ''' <summary>
    ''' 警告表示時の前画面遷移処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HiddenButtonWarning_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HiddenButtonWarning.Click
        'FMメインから来る場合と、通知履歴から来た場合で遷移先が異なる(元の画面に戻る)
        Me.RedirectPrevScreen()
    End Sub
    ' 2015/5/1 強制納車対応 警告表示後の前画面遷移リクエスト end

#End Region

End Class
