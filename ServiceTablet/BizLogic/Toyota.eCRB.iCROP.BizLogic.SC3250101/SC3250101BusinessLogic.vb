'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250101BusinessLogic.vb
'─────────────────────────────────────
'機能： 商品訴求メイン（車両）ビジネスロジック
'補足： 
'作成： 2014/02/XX NEC 鈴木
'更新： 2014/03/xx NEC 上野
'更新： 2014/04/xx NEC 脇谷
'更新： 2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports Toyota.eCRB.iCROP.BizLogic.SC3250101.SC3250101WebServiceClassBusinessLogic
Imports Toyota.eCRB.iCROP.BizLogic.SC3250101.SC3250101WebServiceClassBusinessLogic_CreateXml
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.iCROP.DataAccess.SC3250101
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Data


Public Class SC3250101BusinessLogic

    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "PublicConst"

    ''' <summary>
    ''' 基幹コード区分
    ''' </summary>
    Public Enum DmsCodeType

        ''' <summary>
        ''' 区分なし
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' 販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        DealerCode = 1

        ''' <summary>
        ''' 店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        BranchCode = 2

        ''' <summary>
        ''' ストールID
        ''' </summary>
        ''' <remarks></remarks>
        StallId = 3

        ''' <summary>
        ''' 顧客分類
        ''' </summary>
        ''' <remarks></remarks>
        CustomerClass = 4

        ''' <summary>
        ''' 作業ステータス
        ''' </summary>
        ''' <remarks></remarks>
        WorkStatus = 5

        ''' <summary>
        ''' 中断理由区分
        ''' </summary>
        ''' <remarks></remarks>
        JobStopReasonType = 6

        ''' <summary>
        ''' チップステータス
        ''' </summary>
        ''' <remarks></remarks>
        ChipStatus = 7

        ''' <summary>
        ''' 希望連絡時間帯
        ''' </summary>
        ''' <remarks></remarks>
        ContactTimeZone = 8

        ''' <summary>
        ''' メーカー区分
        ''' </summary>
        ''' <remarks></remarks>
        MakerType = 9

    End Enum

    ''' <summary>
    ''' メーカータイプ
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum MakerType
        ''' <summary>1: トヨタ</summary>
        TOYOTA = 1
        ''' <summary>2:レクサス</summary>
        LEXUS
        ''' <summary>3:その他</summary>
        ELSE_MAKER
    End Enum

    ''' <summary>
    ''' タイミング
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DEF_TIMING
        ''' <summary>車両情報特定前</summary>
        Public Const UNKNOWN_VEHICLE As String = "0"
        ''' <summary>R/O発行前（顧客承認前）</summary>
        Public Const BEFORE_PUBLISH As String = "10"
        ''' <summary>R/O発行後（顧客承認後）</summary>
        Public Const AFTER_PUBLISH As String = "50"
        ''' <summary>追加作業起票後（PS見積もり後）</summary>
        Public Const AFTER_ADD_WK_MAKE As String = "35"
        ''' <summary>Close Job後</summary>
        Public Const COMPLETE As String = "85"
        ''' <summary>キャンセル</summary>
        Public Const CANCEL As String = "99"
    End Class

    ''' <summary>
    ''' 点検種類
    ''' </summary>
    ''' <remarks></remarks>
    Public Class InspectionType
        Public RESULT As String = String.Empty
        Public SUGGEST As String = String.Empty
        Public SUGGEST_DISP As String = String.Empty
    End Class



    ''' <summary>サービス戻り値(ResultID)：ServiceSuccess</summary>
    Private Const ServiceSuccess As String = "0"

    Public Const DATABASE_ERROR As Integer = -2

    Public Const WEBSERVICE_ERROR As Integer = -1

    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_START As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_END As String = "End"


#End Region

#Region "定数"

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

    ''' <summary>更新カウント：新規</summary>
    Private Const FIRST_TIME As String = "1"


    '2014/05/27 ポップアップによるROプレビュー（過去）表示　START　↓↓↓
    ''' <summary>
    ''' 入庫管理番号利用フラグ設定名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SettingNameSVCIN_Use As String = "SVCIN_NUM_USE_FLG"

    ''' <summary>
    ''' 入庫管理番号フォーマット設定名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SettingNameSVCIN_Format As String = "SVCIN_NUM_FORMAT"

    '2014/05/27 ポップアップによるROプレビュー（過去）表示　END　　↑↑↑

    ''' <summary>
    ''' デフォルトモデルコード（CAMRY）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_MODEL_CD As String = "CARY"

    ''' <summary>
    ''' SuggestInfo(0):INSPEC_ITEM_CD（点検項目コード）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const hdnINSPEC_ITEM_CD As Integer = 0

    ''' <summary>
    ''' SuggestInfo(1):SUGGEST_ICON（現在のSuggestアイコン番号）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const hdnSUGGEST_ICON As Integer = 1

    ''' <summary>
    ''' SuggestInfo(2):SUGGEST_STATUS（推奨フラグ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const hdnSUGGEST_STATUS As Integer = 2

    ''' <summary>
    ''' SuggestInfo(3):ChangeFlag（変更フラグ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const hdnChangeFlag As Integer = 3


#End Region

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
#Region "定数"

    ''' <summary>
    ''' VCL_KATASHIKIの初期値(半角スペース) 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_KATASHIKI_SPACE As String = " "

#End Region
#Region "Private変数"

    ''' <summary>
    ''' 型式使用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private useFlgKatashiki As Boolean

#End Region



#Region "公開メソッド"

    ''' <summary>
    ''' 型式使用フラグの取得
    ''' </summary>
    ''' <param name="strRoNum">R/O番号</param>
    ''' <param name="strDlrCd">販売店コード</param>
    ''' <param name="strBrnCd">店舗コード</param>
    ''' <remarks></remarks>
    Public Sub New(Optional ByVal strRoNum As String = "", Optional ByVal strDlrCd As String = "", Optional ByVal strBrnCd As String = "")

        If String.IsNullOrEmpty(strRoNum) Or String.IsNullOrEmpty(strDlrCd) Or String.IsNullOrEmpty(strBrnCd) Then
            Return
        End If

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                        , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3250101DataSet
        Dim dt As DataTable = tableAdapter.GetDlrCdExistMst(strRoNum, strDlrCd, strBrnCd)
        Dim　katashiki_exist As Boolean = False
        If dt.Rows.Count > 0 Then
            katashiki_exist = "1".Equals(dt(0)("KATASHIKI_EXIST").ToString())
        End If

        If katashiki_exist = True Then
            SetUseFlgKatashiki(True)
        Else
            SetUseFlgKatashiki(False)
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END [Result=Return:{2}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , katashiki_exist.ToString))

    End Sub

    ''' <summary>
    ''' 型式使用フラグを設定する
    ''' </summary>
    ''' <param name="useFlgKatashiki">型式使用フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Sub SetUseFlgKatashiki(ByVal useFlgKatashiki As Boolean)
        Me.useFlgKatashiki = useFlgKatashiki
    End Sub

    ''' <summary>
    ''' 型式使用フラグを取得する
    ''' </summary>
    ''' <returns>型式使用フラグ設定値</returns>
    ''' <remarks>true：型式を条件に使用する ／ false：モデルを条件に使用する</remarks>
    Public Function GetUseFlgKatashiki() As Boolean
        Return Me.useFlgKatashiki
    End Function

    '2019/07/05　TKM要件:型式対応　END　↑↑↑

    ' 2販社 BTS310 横展開修正 追加・更新排他制御追加 2015/04/07 start
    ''' <summary>
    ''' 変更があった項目を一時ワーク（TB_W_REPAIR_SUGGESTION）に保存する
    ''' </summary>
    ''' <param name="tables">部位コード毎の点検項目入力結果セット</param>
    ''' <param name="staffInfo">ログインユーザー情報</param>
    ''' <param name="strSAChipID">SAChipID</param>
    ''' <returns>登録結果 1：正常終了　-1：失敗　99：登録なし</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function SetTB_W_REPAIR_SUGGESTION(ByRef tables As ArrayList, ByVal staffInfo As StaffContext, ByVal strSAChipID As String) As Integer
        Dim bInResult As Integer = 1 '戻り値宣言
        '例外処理
        Try
            '部位毎にループ
            For Each pair As KeyValuePair(Of String, ArrayList) In tables
                Dim result As Integer   '行単位の更新処理関数戻り値用
                '点検項目数ループ
                For j As Integer = 0 To pair.Value.Count - 1
                    '変更有のデータをDBに反映
                    Dim SuggestInfo() As String = DirectCast(pair.Value(j), String())
                    '変化があったら更新
                    If SuggestInfo(hdnChangeFlag) <> "0" Then
                        '点検項目毎の更新処理
                        result = Me.Set_TB_W_REPAIR_SUGGESTION_Process( _
                            staffInfo.DlrCD _
                            , staffInfo.BrnCD _
                            , staffInfo.Account _
                            , strSAChipID _
                            , pair.Key _
                            , SuggestInfo(hdnINSPEC_ITEM_CD) _
                            , SuggestInfo(hdnSUGGEST_ICON) _
                            )
                        '正常終了でも更新なしでもない場合はエラー、ロールバック指定して関数を抜ける
                        If result <> 1 And result <> 99 Then
                            Me.Rollback = True

                            Return result
                        End If
                    End If
                    '2014/06/02 レスポンス対策　END　　↑↑↑
                Next
            Next

        Catch ex As Exception
            '例外時ロールバック
            Me.Rollback = True
            bInResult = -1
        End Try

        Return bInResult
    End Function
    ' 2販社 BTS310 横展開修正 追加・更新排他制御追加 2015/04/07 end

    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　START　↓↓↓
    ''' <summary>
    ''' 商品訴求登録実績データへの登録・更新処理、及び商品訴求画面データWK削除処理
    ''' </summary>
    ''' <param name="DBSendData">データベース処理を行うデータリスト（Of ArrayList）</param>
    ''' <returns>登録結果 1：正常終了　-1：失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function RegisterAndDeleteWork(ByVal DBSendData As List(Of ArrayList)) As Integer

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1(Count):{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , DBSendData.Count.ToString))

        'Dim dsSC3250101 As New SC3250101DataSet
        'Dim dtSC3250101 As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
        'Dim dtSC3250101_2 As New SC3250101DataSet.TB_W_REPAIR_SUGGESTIONDataTable
        Dim ret As Integer

        Dim strDLR_CD As String = String.Empty
        Dim strBRN_CD As String = String.Empty
        Dim strSTF_CD As String = String.Empty
        Dim strSAChipID As String = String.Empty
        Dim strSVC_CD As String = String.Empty
        Dim strINSPEC_ITEM_CD As String = String.Empty
        Dim strSUGGEST_ICON As String = String.Empty
        ' 2販社 BTS310 横展開修正 追加・更新排他制御追加 2015/04/07 start
        Try
            'データを取り出す
            '2019/07/05　TKM要件:型式対応　START　↓↓
            Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
                '2019/07/05　TKM要件:型式対応　END　↑↑↑
                Dim dtSC3250101 As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
                Dim dtSC3250101_2 As New SC3250101DataSet.TB_W_REPAIR_SUGGESTIONDataTable
                For Each DBList As ArrayList In DBSendData

                    strDLR_CD = DBList(0).ToString
                    strBRN_CD = DBList(1).ToString
                    strSTF_CD = DBList(2).ToString
                    strSAChipID = DBList(3).ToString
                    strSVC_CD = DBList(4).ToString
                    strINSPEC_ITEM_CD = DBList(5).ToString
                    strSUGGEST_ICON = DBList(6).ToString

                    'Logger.Info(String.Format("★{0}:{1}:{2}:{3}:{4}:{5}:{6}", strDLR_CD, strBRN_CD, strSTF_CD, strRO_NUM, strINSPEC_TYPE, strINSPEC_ITEM_CD, strSUGGEST_ICON))

                    '対象データを商品訴求登録実績データより取得
                    dtSC3250101 = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_Select(strDLR_CD, strBRN_CD, strSTF_CD, strSAChipID, strSVC_CD, strINSPEC_ITEM_CD)

                    '対象データを登録
                    Select Case True
                        Case dtSC3250101.Rows.Count = 0
                            '対象データが対象データを商品訴求登録実績データに未登録である場合、新規登録
                            Logger.Info("TB_T_REPAIR_SUGGESTION_RSLT_Insert")
                            ret = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_Insert( _
                                                                      strDLR_CD _
                                                                    , strBRN_CD _
                                                                    , strSTF_CD _
                                                                    , strSAChipID _
                                                                    , strSVC_CD _
                                                                    , strINSPEC_ITEM_CD _
                                                                    , strSUGGEST_ICON _
                                                                    , strSTF_CD
                                                                    )
                            '新規登録に失敗していたらロールバック
                            If ret = 0 Then
                                Logger.Info("TB_T_REPAIR_SUGGESTION_RSLT_Insert_Failed")
                                Me.Rollback = True
                                Return -1
                            End If

                        Case 0 < dtSC3250101.Rows.Count
                            '対象データが対象データを商品訴求登録実績データに登録済である場合、更新
                            Logger.Info("TB_T_REPAIR_SUGGESTION_RSLT_Update")
                            ret = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_Update( _
                                                                      strDLR_CD _
                                                                    , strBRN_CD _
                                                                    , strSTF_CD _
                                                                    , strSAChipID _
                                                                    , strSVC_CD _
                                                                    , strINSPEC_ITEM_CD _
                                                                    , strSUGGEST_ICON _
                                                                    , strSTF_CD)
                            '更新に失敗していたらロールバック
                            If ret = 0 Then
                                Logger.Info("TB_T_REPAIR_SUGGESTION_RSLT_Update_Failed")
                                Me.Rollback = True
                                Return -1
                            End If

                        Case Else
                            Me.Rollback = True
                            Return -1

                    End Select

                    '対象データを商品訴求画面データWKより取得
                    dtSC3250101_2 = dsSC3250101.TB_W_REPAIR_SUGGESTION_Select(strDLR_CD, strBRN_CD, strSTF_CD, strSAChipID, strSVC_CD, strINSPEC_ITEM_CD)

                    'あれば削除処理を行う
                    If 0 < dtSC3250101_2.Rows.Count Then
                        '商品訴求画面データWK削除処理
                        Logger.Info("TB_W_REPAIR_SUGGESTION_Delete")
                        ret = dsSC3250101.TB_W_REPAIR_SUGGESTION_Delete(
                                                                  strDLR_CD _
                                                                , strBRN_CD _
                                                                , strSTF_CD _
                                                                , strSAChipID _
                                                                , strSVC_CD _
                                                                , strINSPEC_ITEM_CD _
                                                                    )

                        '削除に失敗していたらロールバック
                        If ret = 0 Then
                            Logger.Info("TB_W_REPAIR_SUGGESTION_Delete_Failed")
                            Me.Rollback = True
                            Return -1
                        End If
                    End If

                Next
            End Using

        Catch ex As Exception
            Me.Rollback = True
            Return -1
        End Try
        ' 2販社 BTS310 横展開修正 追加・更新排他制御追加 2015/04/07 end

        'すべての更新が完了したら1を返す

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ret))

        Return 1

    End Function
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　END　　↑↑↑

    ''' <summary>
    ''' モデルマスタ取得処理
    ''' </summary>
    ''' <param name="strVCL_VIN">VIN</param>
    ''' <returns></returns>
    ''' <remarks>2014/06/11　引数をR/O→VINに変更</remarks>
    Public Function GetModelInfo(ByVal strVCL_VIN As String) As SC3250101DataSet.TB_M_MODELDataTable
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strVCL_VIN))

        'Dim dsSC3250101 As New SC3250101DataSet
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dtSC3250101 As New SC3250101DataSet.TB_M_MODELDataTable
            dtSC3250101 = dsSC3250101.TB_M_MODEL_Select(strVCL_VIN)

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250101.Rows.Count.ToString))

            Return dtSC3250101

        End Using

    End Function

    ''' <summary>
    ''' TOYOTA車 or 他メーカー判定処理
    ''' </summary>
    ''' <param name="strMODEL_CD">モデルコード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsTOYOTA(ByVal strMODEL_CD As String) As Boolean

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strMODEL_CD))

        'Dim dsSC3250101 As New SC3250101DataSet
        'Dim dtSC3250101 As New SC3250101DataSet.TB_M_MAKERDataTable
        Dim ret As Boolean = False

        '指定されたモデルのメーカー取得
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dtSC3250101 As New SC3250101DataSet.TB_M_MAKERDataTable
            dtSC3250101 = dsSC3250101.TB_M_MAKER_Select(strMODEL_CD)

            If 0 < dtSC3250101.Rows.Count AndAlso dtSC3250101.Rows(0)("MAKER_TYPE").ToString = CStr(MakerType.TOYOTA) Then
                ret = True
            Else
                ret = False
            End If

        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} return:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ret.ToString))

        Return ret

    End Function

    ''' <summary>
    ''' サービス入庫取得
    ''' </summary>
    ''' <param name="strRO_NUM">R/O番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetServiceIn(ByVal strDLR_CD As String _
                                  , ByVal strBRN_CD As String _
                                  , ByVal strRO_NUM As String _
                                  ) As SC3250101DataSet.TB_T_SERVICEINDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3} P2:{4} P3:{5}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strRO_NUM))

        'Dim dsSC3250101 As New SC3250101DataSet
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dtSC3250101 As New SC3250101DataSet.TB_T_SERVICEINDataTable

            '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　START　↓↓↓
            dtSC3250101 = dsSC3250101.TB_T_SERVICEIN_Select(strDLR_CD, strBRN_CD, strRO_NUM)
            'dtSC3250101 = dsSC3250101.TB_T_SERVICEIN_Select(strRO_NUM)
            '2014/06/20 TB_T_SERVICEINテーブルのRO_NUM使用廃止　END　　↑↑↑

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250101.Rows.Count.ToString))

            Return dtSC3250101

        End Using

    End Function

    ''' <summary>
    ''' R/Oステータスを取得する
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strRO_NUM">RO番号</param>
    ''' <returns>R/Oステータス</returns>
    ''' <remarks></remarks>
    Public Function GetConvROStatus(ByVal strDLR_CD As String, ByVal strBRN_CD As String, ByVal strRO_NUM As String) As String

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3} P2:{4} P3:{5}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strRO_NUM))

        Dim title As New InspectionType
        'Dim dsSC3250101 As New SC3250101DataSet
        'Dim tblGetTiming As New SC3250101DataSet.GetTimingDataTable
        Dim timing As Integer

        '販売店コード、店舗コード、RO番号から指定したRO番号のステータスを取得する
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑

            Dim tblGetTiming As New SC3250101DataSet.GetTimingDataTable
            tblGetTiming = dsSC3250101.GetTiming_Select(strDLR_CD, strBRN_CD, strRO_NUM)

            If tblGetTiming IsNot Nothing And 0 < tblGetTiming.Rows.Count Then
                If Not String.IsNullOrWhiteSpace(tblGetTiming.Rows(0)("RO_STATUS").ToString) Then
                    timing = Integer.Parse(tblGetTiming.Rows(0)("RO_STATUS").ToString)
                Else
                    timing = Integer.Parse(DEF_TIMING.UNKNOWN_VEHICLE)
                End If
            Else
                'ROステータスが取得できなかった
                timing = Integer.Parse(DEF_TIMING.UNKNOWN_VEHICLE)
            End If

        End Using

        '2014/06/13 ROステータスによってアドバイス表示を変更　START　↓↓↓
        Dim RetStatus As String = String.Empty
        If timing = 0 Then
            '車両情報特定前
            RetStatus = DEF_TIMING.UNKNOWN_VEHICLE
        ElseIf 0 < timing And timing < 35 Then
            'R/O発行前（顧客承認前）
            RetStatus = DEF_TIMING.BEFORE_PUBLISH
        ElseIf 35 = timing Then
            '追加作業起票後（PS見積もり後）
            RetStatus = DEF_TIMING.AFTER_ADD_WK_MAKE
        ElseIf 35 < timing And timing < 50 Then
            'R/O発行前（顧客承認前）
            RetStatus = DEF_TIMING.BEFORE_PUBLISH
        ElseIf 50 <= timing And timing < 85 Then
            'R/O発行後（顧客承認後）
            RetStatus = DEF_TIMING.AFTER_PUBLISH
        ElseIf 85 <= timing And timing < 99 Then
            'Close Job後
            RetStatus = DEF_TIMING.COMPLETE
        Else
            'キャンセル
            RetStatus = DEF_TIMING.CANCEL
        End If
        '2014/06/13 ROステータスによってアドバイス表示を変更　END　　↑↑↑

        'If timing = "0" Then
        '    timing = "0"
        'ElseIf CInt(timing) < 70 Then
        '    'ROクローズ前
        '    timing = "10"
        'ElseIf 70 <= CInt(timing) And CInt(timing) < 99 Then
        '    'ROクローズ後
        '    timing = "70"
        'Else
        '    'キャンセル
        '    timing = "99"
        'End If

        'Select Case timing
        '    'R/O発行前、R/O発行後、追加作業起票後
        '    Case DEF_TIMING.BEFORE_PUBLISH _
        '        , DEF_TIMING.AFTER_PUBLISH _
        '        , DEF_TIMING.AFTER_ADD_WK_MAKE
        '        title.RESULT = "10K"
        '        title.SUGGEST = "20K"

        '        '整備完了後
        '    Case DEF_TIMING.COMPLETE
        '        title.RESULT = "20K"
        '        title.SUGGEST = "30K"

        '        'キャンセル
        '    Case DEF_TIMING.CANCEL
        '        title.RESULT = "10K"
        '        title.SUGGEST = "10K"

        'End Select

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:{3}(RO_STATUS:{4})" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , RetStatus _
                   , timing))

        Return RetStatus
    End Function

    ''' <summary>
    ''' 商品訴求部位マスタ取得処理
    ''' </summary>
    ''' <param name="strPART_NAME">商品訴求部位名称</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPartInfo(ByVal strPART_NAME As String) As SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strPART_NAME))

        'Dim dsSC3250101 As New SC3250101DataSet

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dtSC3250101 As New SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable
            dtSC3250101 = dsSC3250101.TB_M_REPAIR_SUGGESTION_PART_Select(strPART_NAME)

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250101.Rows.Count.ToString))

            Return dtSC3250101

        End Using

    End Function

    '2014/05/29 レスポンス対策　START　↓↓↓
    ''' <summary>
    ''' 商品訴求部位マスタ取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllPartInfo(ByVal listPART_NAME As List(Of String)) As SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1(Count):{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , listPART_NAME.Count))

        'Dim dsSC3250101 As New SC3250101DataSet

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dtSC3250101 As New SC3250101DataSet.TB_M_REPAIR_SUGGESTION_PARTDataTable
            dtSC3250101 = dsSC3250101.TB_M_REPAIR_SUGGESTION_ALL_PART_Select(listPART_NAME)

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250101.Rows.Count.ToString))

            Return dtSC3250101

        End Using

    End Function
    '2014/05/29 レスポンス対策　　END　↑↑↑

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)(SQL変更による引数追加)　START　↓↓↓
    '2014/05/29 レスポンス対策　START　↓↓↓
    '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 Start
    ''' <summary>
    ''' 点検項目名を取得する
    ''' </summary>
    ''' <param name="strModel_CD">モデルコード</param>
    ''' <param name="strKatashiki">型式</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">SAChipID</param>
    ''' <param name="strSVC_CD">サービスコード</param>
    ''' <param name="strDefaultModel_CD">デフォルトモデルコード</param>
    ''' <returns>点検項目名</returns>
    ''' <remarks></remarks>
    Public Function GetInspectionList( _
                                  ByVal strModel_CD As String _
                                , ByVal strKatashiki As String _
                                , ByVal strDLR_CD As String _
                                , ByVal strBRN_CD As String _
                                , ByVal strSTF_CD As String _
                                , ByVal strSAChipID As String _
                                , ByVal strSVC_CD As String _
                                , ByVal strDefaultModel_CD As String _
                          ) As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}  P1:{3} P2:{4} P3:{5} P4:{6} P5:{7} P6:{8} P7:{9} P8:{10}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strModel_CD _
                  , strKatashiki _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strSTF_CD _
                  , strSAChipID _
                  , strSVC_CD _
                  , strDefaultModel_CD))
        '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 End
        'Dim dsSC3250101 As New SC3250101DataSet

        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            Dim dtSC3250101 As New SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable
            '部位に関連する点検項目取得
            '2016/10/07　レスポンス改善対応　START　↓↓↓
            'dtSC3250101 = dsSC3250101.TB_M_INSPECTION_COMB_SelectList(strModel_CD _
            '                                                          , strGrade_CD _
            '                                                          , strDLR_CD _
            '                                                          , strBRN_CD _
            '                                                          , strSTF_CD _
            '                                                          , strSAChipID _
            '                                                          , strSVC_CD _
            '                                                          , strDefaultModel_CD _
            '                                                          , strDLR_CD _
            '                                                          , strBRN_CD)
            dtSC3250101 = GetInspectaionSelectList(strModel_CD _
                                                    , strKatashiki _
                                                    , strDLR_CD _
                                                    , strBRN_CD _
                                                    , strSTF_CD _
                                                    , strSAChipID _
                                                    , strSVC_CD _
                                                    , strDefaultModel_CD _
                                                    , strDLR_CD _
                                                    , strBRN_CD)

            '2014/06/17　指定したDLR_CD、BRN_CDで点検マスタが取得出来なかったときの処理追加　START　↓↓↓
            If DirectCast(dtSC3250101.Select("REQ_ITEM_DISP_SEQ IS NOT NULL"), SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILRow()).Count = 0 Then
                'dtSC3250101 = dsSC3250101.TB_M_INSPECTION_COMB_SelectList(strModel_CD _
                '                                                          , strGrade_CD _
                '                                                          , strDLR_CD _
                '                                                          , strBRN_CD _
                '                                                          , strSTF_CD _
                '                                                          , strSAChipID _
                '                                                          , strSVC_CD _
                '                                                          , strDefaultModel_CD _
                '                                                          , "XXXXX" _
                '                                                          , "XXX")
                dtSC3250101 = GetInspectaionSelectList(strModel_CD _
                                                        , strKatashiki _
                                                        , strDLR_CD _
                                                        , strBRN_CD _
                                                        , strSTF_CD _
                                                        , strSAChipID _
                                                        , strSVC_CD _
                                                        , strDefaultModel_CD _
                                                        , "XXXXX" _
                                                        , "XXX")
            End If
            '2014/06/17　指定したDLR_CD、BRN_CDで点検マスタが取得出来なかったときの処理追加　END　↑↑↑
            '2016/10/07　レスポンス改善対応　END　↑↑↑

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250101.Rows.Count.ToString))

            Return dtSC3250101

        End Using

    End Function
    '2014/05/29 レスポンス対策　　END　↑↑↑
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)(SQL変更による引数追加)　END　　↑↑↑

    '2019/07/05　TKM要件:型式対応　END　↑↑↑


    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 Start
    ''' <summary>
    ''' 点検項目名を取得する
    ''' </summary>
    ''' <param name="strModel_CD">モデルコード</param>
    ''' <param name="strKatashiki">型式</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">SAChipID</param>
    ''' <param name="strSVC_CD">サービスコード</param>
    ''' <param name="strDefaultModel_CD">デフォルトモデルコード</param>
    ''' <param name="strDLR_CD_M">販売店コード(商品訴求初期表示用アイテム取得時)</param>
    ''' <param name="strBRN_CD_M">店舗コード(商品訴求初期表示用アイテム取得時)</param>
    ''' <returns>商品訴求点検内容データセット</returns>
    ''' <remarks></remarks>
    Public Function GetInspectaionSelectList( _
                                            ByVal strModel_CD As String _
                                          , ByVal strKatashiki As String _
                                          , ByVal strDLR_CD As String _
                                          , ByVal strBRN_CD As String _
                                          , ByVal strSTF_CD As String _
                                          , ByVal strSAChipID As String _
                                          , ByVal strSVC_CD As String _
                                          , ByVal strDefaultModel_CD As String _
                                          , ByVal strDLR_CD_M As String _
                                          , ByVal strBRN_CD_M As String _
                          ) As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}  P1:{3} P2:{4} P3:{5} P4:{6} P5:{7} P6:{8} P7:{9} P8:{10}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strModel_CD _
                  , strKatashiki _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strSTF_CD _
                  , strSAChipID _
                  , strSVC_CD _
                  , strDefaultModel_CD _
                  ))
        '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 End
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            Dim dtSC3250101 As New SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable

            'MM1
            Dim dtSC3250101MM1 As New SC3250101DataSet.MM1DataTable
            dtSC3250101MM1 = dsSC3250101.TB_M_INSPECTION_COMB_MM1_Select(strModel_CD, strKatashiki, strDefaultModel_CD, strDLR_CD_M, strBRN_CD_M)
            'MSM1
            Dim dtSC3250101MSM1 As New SC3250101DataSet.MSM1DataTable
            dtSC3250101MSM1 = dsSC3250101.TB_M_INSPECTION_COMB_MSM1_Select(strDefaultModel_CD, strSVC_CD, strDLR_CD_M, strBRN_CD_M)
            'MSM2
            Dim dtSC3250101MSM2 As New SC3250101DataSet.MSM2DataTable
            dtSC3250101MSM2 = dsSC3250101.TB_M_INSPECTION_COMB_MSM2_Select(strModel_CD, strKatashiki, strSVC_CD, strDLR_CD_M, strBRN_CD_M)
            'MM3
            Dim dtSC3250101MM3 As New SC3250101DataSet.MM3DataTable
            dtSC3250101MM3 = dsSC3250101.TB_M_INSPECTION_COMB_MM3_Select(strDLR_CD, strBRN_CD, strSAChipID, strSVC_CD)
            'MM4
            Dim dtSC3250101MM4 As New SC3250101DataSet.MM4DataTable
            dtSC3250101MM4 = dsSC3250101.TB_M_INSPECTION_COMB_MM4_Select(strDLR_CD, strBRN_CD, strSTF_CD, strSAChipID, strSVC_CD)

            Dim SC3250101MM1NewRow As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILRow
            Dim SC3250101DataTable As New SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILDataTable

            For Each MM1Row As SC3250101DataSet.MM1Row In dtSC3250101MM1
                ' 値のセット
                SC3250101MM1NewRow = DirectCast(SC3250101DataTable.NewRow(), SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_FINAL_INSPECTION_DETAILRow)
                SC3250101MM1NewRow.INSPEC_ITEM_CD = MM1Row.INSPEC_ITEM_CD
                SC3250101MM1NewRow.REQ_PART_CD = MM1Row.REQ_PART_CD
                If Not MM1Row.IsREQ_ITEM_DISP_SEQNull Then
                    SC3250101MM1NewRow.REQ_ITEM_DISP_SEQ = MM1Row.REQ_ITEM_DISP_SEQ
                End If
                SC3250101MM1NewRow.INSPEC_ITEM_NAME = MM1Row.INSPEC_ITEM_NAME
                SC3250101MM1NewRow.SUB_INSPEC_ITEM_NAME = MM1Row.SUB_INSPEC_ITEM_NAME
                SC3250101MM1NewRow.DISP_INSPEC_ITEM_NEED_INSPEC = Integer.Parse(MM1Row.DISP_INSPEC_ITEM_NEED_INSPEC)
                SC3250101MM1NewRow.DISP_INSPEC_ITEM_NEED_REPLACE = Integer.Parse(MM1Row.DISP_INSPEC_ITEM_NEED_REPLACE)
                SC3250101MM1NewRow.DISP_INSPEC_ITEM_NEED_FIX = Integer.Parse(MM1Row.DISP_INSPEC_ITEM_NEED_FIX)
                SC3250101MM1NewRow.DISP_INSPEC_ITEM_NEED_CLEAN = Integer.Parse(MM1Row.DISP_INSPEC_ITEM_NEED_CLEAN)
                SC3250101MM1NewRow.DISP_INSPEC_ITEM_NEED_SWAP = Integer.Parse(MM1Row.DISP_INSPEC_ITEM_NEED_SWAP)
                ' 取得されるのは1件のみ
                For Each itemRow In dtSC3250101MSM1.Select(String.Format("INSPEC_ITEM_CD={0}", MM1Row.INSPEC_ITEM_CD))
                    SC3250101MM1NewRow.REQ_ITEM_CD_DEFAULT = itemRow("REQ_ITEM_CD_DEFAULT").ToString()
                    SC3250101MM1NewRow.SVC_CD_DEFAULT = itemRow("SVC_CD_DEFAULT").ToString()
                    SC3250101MM1NewRow.SUGGEST_FLAG_DEFAULT = itemRow("SUGGEST_FLAG_DEFAULT").ToString()
                    Exit For
                Next
                For Each itemRow In dtSC3250101MSM2.Select(String.Format("INSPEC_ITEM_CD={0}", MM1Row.INSPEC_ITEM_CD))
                    SC3250101MM1NewRow.REQ_ITEM_CD = itemRow("REQ_ITEM_CD").ToString()
                    SC3250101MM1NewRow.SVC_CD = itemRow("SVC_CD").ToString()
                    SC3250101MM1NewRow.SUGGEST_FLAG = itemRow("SUGGEST_FLAG").ToString()
                    Exit For
                Next
                For Each itemRow In dtSC3250101MM3.Select(String.Format("INSPEC_ITEM_CD={0}", MM1Row.INSPEC_ITEM_CD))
                    SC3250101MM1NewRow.R_SUGGEST_ICON = itemRow("R_SUGGEST_ICON").ToString()
                    SC3250101MM1NewRow.R_SVC_CD = itemRow("R_SVC_CD").ToString()
                    Exit For
                Next
                For Each itemRow In dtSC3250101MM4.Select(String.Format("INSPEC_ITEM_CD={0}", MM1Row.INSPEC_ITEM_CD))
                    SC3250101MM1NewRow.W_SUGGEST_ICON = itemRow("W_SUGGEST_ICON").ToString()
                    SC3250101MM1NewRow.W_SVC_CD = itemRow("W_SVC_CD").ToString()
                    Exit For
                Next

                SC3250101DataTable.Rows.Add(SC3250101MM1NewRow)
            Next

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250101.Rows.Count.ToString))

            Return SC3250101DataTable

        End Using

    End Function

    '2019/07/05　TKM要件:型式対応　END　↑↑↑

    ''' <summary>
    ''' Resutに表示する過去の実績データを完成検査結果詳細データから取得する
    ''' </summary>
    ''' <param name="strVIN_CD">VINコード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInspectionDetail( _
                            ByVal strVIN_CD As String _
                      ) As SC3250101DataSet.TB_T_FINAL_INSPECTION_DETAILDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strVIN_CD))

        'Dim dsSC3250101 As New SC3250101DataSet

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dtSC3250101 As New SC3250101DataSet.TB_T_FINAL_INSPECTION_DETAILDataTable
            '部位に関連する点検項目取得
            'dtSC3250101 = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_OF_PART_Select(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account, strRO_NUM, strINSPEC_TYPE)
            dtSC3250101 = dsSC3250101.TB_T_FINAL_INSPECTION_DETAIL_Select(strVIN_CD)

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250101.Rows.Count.ToString))

            Return dtSC3250101

        End Using

    End Function

    ''' <summary>
    ''' 過去の実績一覧（Resut一覧）を取得する
    ''' </summary>
    ''' <param name="strVIN_CD">VINコード</param>
    ''' <param name="specifyDlrCdFlgs">全販売店検索フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetResultList(ByVal strVIN_CD As String, _
                                  ByVal specifyDlrCdFlgs As Boolean _
                                  ) As SC3250101DataSet.ResultListDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}  P1:{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strVIN_CD))

        'Dim dsSC3250101 As New SC3250101DataSet

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dtSC3250101 As New SC3250101DataSet.ResultListDataTable
            '部位に関連する点検項目取得
            'dtSC3250101 = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_OF_PART_Select(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account, strRO_NUM, strINSPEC_TYPE)
            '2015/04/14 新販売店追加対応 start
            'dtSC3250101 = dsSC3250101.ResultList_Select(strVIN_CD)
            dtSC3250101 = dsSC3250101.ResultList_Select(strVIN_CD, specifyDlrCdFlgs)
            '2015/04/14 新販売店追加対応 end

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250101.Rows.Count.ToString))

            Return dtSC3250101

        End Using

    End Function

    ''' <summary>
    ''' 画面URL情報取得
    ''' </summary>
    ''' <param name="inDisplayNumber">表示番号</param>
    ''' <returns>URL情報</returns>
    ''' <remarks></remarks>
    Public Function GetDisplayUrl(ByVal inDisplayNumber As Long) As SC3250101DataSet.SC3250101DisplayRelationDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDisplayNumber.ToString))

        '画面URL情報取得
        'Dim dsSC3250101 As New SC3250101DataSet

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dt As SC3250101DataSet.SC3250101DisplayRelationDataTable
            dt = dsSC3250101.TB_M_DISP_RELATION_Select(inDisplayNumber)

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2}  Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dt.Rows.Count.ToString))

            Return dt

        End Using

    End Function

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    ''' <summary>
    ''' 次の点検種類を取得する
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strModel_CD">モデルコード</param>
    ''' <param name="strKatashiki">型式</param>
    ''' <param name="strSVC_CD">サービスコード</param>
    ''' <param name="strDefaultModel_CD">デフォルトモデルコード</param>
    ''' <returns>点検種類</returns>
    ''' <remarks></remarks>
    Public Function GetNextInspecType(ByVal strDLR_CD As String _
                                      , ByVal strBRN_CD As String _
                                      , ByVal strMODEL_CD As String _
                                      , ByVal strKatashiki As String _
                                      , ByVal strSVC_CD As String _
                                      , ByVal strDefaultMODEL_CD As String) As String

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7}, P6:{8}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strMODEL_CD _
                  , strKatashiki _
                  , strSVC_CD _
                  , strDefaultMODEL_CD))

        'Dim dsSC3250101 As New SC3250101DataSet
        'Dim dtSC3250101 As New SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable
        'Dim dsSC3250101_2 As New SC3250101DataSet
        'Dim dtSC3250101_2 As New SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable
        Dim ret As String = String.Empty

        '次の点検種類の番号を取得する
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            Dim dtSC3250101 As New SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable
            Dim dtSC3250101_2 As New SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable

            If strSVC_CD = "0" Then
                '初めての時は初回点検を表示する
                dtSC3250101_2 = dsSC3250101.TB_M_INSPECTION_ORDER_From_INSPEC_ORDER(strDLR_CD, strBRN_CD, strMODEL_CD, strKatashiki, "0")

                If dtSC3250101_2 IsNot Nothing And 0 < dtSC3250101_2.Rows.Count Then
                    ret = dtSC3250101_2.Rows(0)("SVC_CD").ToString
                End If

                If ret = String.Empty Then
                    '全店舗コードで再取得
                    dtSC3250101_2 = dsSC3250101.TB_M_INSPECTION_ORDER_From_INSPEC_ORDER(AllDealerCode, AllBranchCode, strMODEL_CD, strKatashiki, "0")

                    If dtSC3250101_2 IsNot Nothing And 0 < dtSC3250101_2.Rows.Count Then
                        ret = dtSC3250101_2.Rows(0)("SVC_CD").ToString
                    End If
                End If

                If ret = String.Empty Then
                    'デフォルトモデルコードで再取得
                    dtSC3250101_2 = dsSC3250101.TB_M_INSPECTION_ORDER_From_INSPEC_ORDER(strDLR_CD, strBRN_CD, strDefaultMODEL_CD, DEFAULT_KATASHIKI_SPACE, "0")

                    If dtSC3250101_2 IsNot Nothing And 0 < dtSC3250101_2.Rows.Count Then
                        ret = dtSC3250101_2.Rows(0)("SVC_CD").ToString
                    End If
                End If

                If ret = String.Empty Then
                    'デフォルトモデルコードの全店舗コードで再取得
                    dtSC3250101_2 = dsSC3250101.TB_M_INSPECTION_ORDER_From_INSPEC_ORDER(AllDealerCode, AllBranchCode, strDefaultMODEL_CD, DEFAULT_KATASHIKI_SPACE, "0")

                    If dtSC3250101_2 IsNot Nothing And 0 < dtSC3250101_2.Rows.Count Then
                        ret = dtSC3250101_2.Rows(0)("SVC_CD").ToString
                    End If
                End If

                If ret = String.Empty Then
                    ret = strSVC_CD
                End If

            Else
                '現在の点検種類が何番目の位置か取得する
                dtSC3250101 = dsSC3250101.TB_M_INSPECTION_ORDER_From_INSPEC_TYPE(strSVC_CD)
                If dtSC3250101 IsNot Nothing And 0 < dtSC3250101.Rows.Count Then
                    Dim InspecNo As String
                    If String.IsNullOrWhiteSpace(dtSC3250101.Rows(0)("INSPEC_ORDER").ToString) Then
                        InspecNo = "0"
                    Else
                        InspecNo = dtSC3250101.Rows(0)("INSPEC_ORDER").ToString
                    End If
                    '2014/05/29 レスポンス対策　START　↓↓↓
                    'dtSC3250101_2 = dsSC3250101_2.TB_M_INSPECTION_ORDER_From_INSPEC_ORDER(CStr(CInt(InspecNo)))
                    dtSC3250101_2 = dsSC3250101.TB_M_INSPECTION_ORDER_From_INSPEC_ORDER(strDLR_CD, strBRN_CD, strMODEL_CD, strKatashiki, InspecNo)
                    '2014/05/29 レスポンス対策　　END　↑↑↑

                    If dtSC3250101_2 IsNot Nothing And 0 < dtSC3250101_2.Rows.Count Then
                        ret = dtSC3250101_2.Rows(0)("SVC_CD").ToString
                    End If

                    If ret = String.Empty Then
                        '全店舗コードで再取得
                        dtSC3250101_2 = dsSC3250101.TB_M_INSPECTION_ORDER_From_INSPEC_ORDER(AllDealerCode, AllBranchCode, strMODEL_CD, strKatashiki, InspecNo)

                        If dtSC3250101_2 IsNot Nothing And 0 < dtSC3250101_2.Rows.Count Then
                            ret = dtSC3250101_2.Rows(0)("SVC_CD").ToString
                        End If
                    End If

                    If ret = String.Empty Then
                        'デフォルトモデルコードで再取得
                        dtSC3250101_2 = dsSC3250101.TB_M_INSPECTION_ORDER_From_INSPEC_ORDER(strDLR_CD, strBRN_CD, strDefaultMODEL_CD, DEFAULT_KATASHIKI_SPACE, InspecNo)

                        If dtSC3250101_2 IsNot Nothing And 0 < dtSC3250101_2.Rows.Count Then
                            ret = dtSC3250101_2.Rows(0)("SVC_CD").ToString
                        End If
                    End If

                    If ret = String.Empty Then
                        'デフォルトモデルコードの全店舗コードで再取得
                        dtSC3250101_2 = dsSC3250101.TB_M_INSPECTION_ORDER_From_INSPEC_ORDER(AllDealerCode, AllBranchCode, strDefaultMODEL_CD, DEFAULT_KATASHIKI_SPACE, InspecNo)

                        If dtSC3250101_2 IsNot Nothing And 0 < dtSC3250101_2.Rows.Count Then
                            ret = dtSC3250101_2.Rows(0)("SVC_CD").ToString
                        End If
                    End If

                    If ret = String.Empty Then
                        ret = strSVC_CD
                    End If
                Else
                    '現在の点検種類が何番目の位置か取得できなかった
                    ret = strSVC_CD
                End If

            End If
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ret))

        Return ret

    End Function

    '2019/07/05　TKM要件:型式対応　END　↑↑↑

    ''' <summary>
    ''' 入庫管理番号取得
    ''' </summary>
    ''' <returns>入庫管理番号</returns>
    ''' <remarks>入庫管理番号の書式変換を行う</remarks>
    Public Function GetSVCIN_NUM(ByVal strBRN_CD As String _
                                 , ByVal strRO_NUM As String _
                                 ) As String

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3} P2:{4}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strBRN_CD _
                  , strRO_NUM))

        '①「販売店システム設定」より、「入庫管理番号利用フラグ」を取得する。
        Dim SVCIN_FLG As String = Me.GetDlrSystemSettingValueBySettingName(SettingNameSVCIN_Use)

        '②「入庫管理番号利用フラグ」が０の場合、書式変換を行う
        Dim SVCIN_Num As String = String.Empty
        If Not String.IsNullOrWhiteSpace(SVCIN_FLG) Then
            If SVCIN_FLG = "0" Then
                Dim SVCIN_Format As String = Me.GetDlrSystemSettingValueBySettingName(SettingNameSVCIN_Format)
                If Not String.IsNullOrWhiteSpace(SVCIN_Format) Then
                    SVCIN_Num = Replace(Replace(SVCIN_Format, "[RO_NUM]", strRO_NUM), "[DMS_BRN_CD]", strBRN_CD)
                End If
            End If
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , SVCIN_Num))

        Return SVCIN_Num

    End Function

    '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　START　↓↓↓
    ''' <summary>
    ''' 販売店システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">販売店システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDlrSystemSettingValueBySettingName(ByVal settingName As String) As String

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , settingName))

        '戻り値
        Dim retValue As String = String.Empty

        'ログイン情報
        Dim userContext As StaffContext = StaffContext.Current

        '自分のテーブルアダプタークラスインスタンスを生成
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using ta As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑

            '販売店システム設定から取得
            Dim dt As SC3250101DataSet.SystemSettingDataTable _
                                    = ta.GetDlrSystemSettingValue(userContext.DlrCD, _
                                                                              userContext.BrnCD, _
                                                                              AllDealerCode, _
                                                                              AllBranchCode, _
                                                                              settingName)

            If 0 < dt.Count Then

                '設定値を取得
                retValue = dt.Item(0).SETTING_VAL

            End If

        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' i-CROP→DMSの値に変換された値を取得する
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="dmsCodeType">基幹コード区分</param>
    ''' <param name="icropCD1">iCROPコード1</param>
    ''' <param name="icropCD2">iCROPコード2</param>
    ''' <param name="icropCD3">iCROPコード3</param>
    ''' <param name="account">アカウント</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 基幹コード区分(1～7)によって、引数に設定する値が異なる
    ''' ※ここに全て記載すると非常に長くなるため、TB_M_DMS_CODE_MAPのテーブル定義書を参照して下さい
    ''' </remarks>
    Public Function GetIcropToDmsCode(ByVal dealerCD As String, _
                                      ByVal dmsCodeType As DmsCodeType, _
                                      ByVal icropCD1 As String, _
                                      ByVal icropCD2 As String, _
                                      ByVal icropCD3 As String, _
                                      Optional ByVal account As String = "") As SC3250101DataSet.DmsCodeMapDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7} P6:{8}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , dealerCD _
                  , CType(dmsCodeType, Integer) _
                  , icropCD1 _
                  , icropCD2 _
                  , icropCD3 _
                  , account))

        '戻り値
        Dim dt As SC3250101DataSet.DmsCodeMapDataTable

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using ta As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            dt = ta.GetIcropToDmsCode(AllDealerCode, _
                                      dealerCD, _
                                      dmsCodeType, _
                                      icropCD1, _
                                      icropCD2, _
                                      icropCD3)
        End Using

        If dt.Count <= 0 Then

            'データが取得できない場合
            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}.{1} WARN：No data found. ", _
                                       Me.GetType.ToString, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            dt.Rows.Add(dt.NewRow)
            dt(0).CODE1 = String.Empty
            dt(0).CODE2 = String.Empty
            dt(0).ACCOUNT = String.Empty

        End If

        'アカウント情報と取得項目のチェック
        If Not (String.IsNullOrEmpty(account)) AndAlso _
           (dmsCodeType = SC3250101BusinessLogic.DmsCodeType.DealerCode OrElse _
           dmsCodeType = SC3250101BusinessLogic.DmsCodeType.BranchCode OrElse _
           dmsCodeType = SC3250101BusinessLogic.DmsCodeType.StallId) Then
            'アカウントが存在する場合且つ、販売店・店舗・ストールの情報を取得する場合
            '変換したアカウントを格納
            dt(0).ACCOUNT = account.Split(CChar("@"))(0)
        Else
            dt(0).ACCOUNT = String.Empty
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return(Count):{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , dt.Count))

        Return dt

    End Function
    '2014/05/23 「ServiceCommonClassBusinessLogic」の使用廃止　　END　↑↑↑

#Region "未使用メソッド"

    '2014/09/05　未使用メソッドに変更　
    '***** ヘッダーのResult一覧ボックス作成時に定期点検かどうかチェックするフラグも
    '***** 一緒に取得するように変更したためコメント化

    ' ''' <summary>
    ' ''' 定期点検サービスかどうか確認
    ' ''' </summary>
    ' ''' <returns>True：定期点検</returns>
    ' ''' <remarks></remarks>
    'Public Function IsPeriodicInspection(ByVal strDLR_CD As String _
    '                                        , ByVal strMAINTE_KATASHIKI As String _
    '                                        , ByVal strMAINTE_CD As String) As Boolean

    '    '開始ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '              , "{0}.{1} {2} P1:{3} P2:{4} P3:{5}" _
    '              , Me.GetType.ToString _
    '              , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '              , LOG_START _
    '              , strDLR_CD _
    '              , strMAINTE_KATASHIKI _
    '              , strMAINTE_CD))

    '    'Dim dsSC3250101 As New SC3250101DataSet
    '    'Dim dtSC3250101 As New SC3250101DataSet.TB_M_SERVICEDataTable
    '    'Dim ChangeModelCD As String = String.Empty
    '    Dim CheckRet As Boolean

    '    Using dsSC3250101 As New SC3250101DataSet
    '        Dim dtSC3250101 As New SC3250101DataSet.TB_M_SERVICEDataTable

    '        '部位に関連する点検項目取得
    '        dtSC3250101 = dsSC3250101.TB_M_SERVICE_Select(strDLR_CD, strMAINTE_KATASHIKI, strMAINTE_CD)

    '        If 0 < dtSC3250101.Count Then
    '            CheckRet = True
    '        Else
    '            CheckRet = False
    '        End If
    '    End Using

    '    '終了ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '               , "{0}.{1} {2} Return:{3}" _
    '               , Me.GetType.ToString _
    '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '               , LOG_END _
    '               , CheckRet))

    '    Return CheckRet
    'End Function

#End Region

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    '【追加要件１】．今回の点検以外の点検を選択できるようにする　START　↓↓↓
    ''' <summary>
    ''' Suggestリストを取得する
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strMODEL_CD">モデルコード</param>
    ''' <param name="strKatashiki">型式</param>
    ''' <param name="strSVC_CD">サービスコード</param>
    ''' <param name="strDefaultModel_CD">デフォルトモデルコード</param>
    ''' <returns>Suggestリスト</returns>
    ''' <remarks></remarks>
    Public Function GetSuggestList(ByVal strDLR_CD As String _
                                   , ByVal strBRN_CD As String _
                                   , ByVal strMODEL_CD As String _
                                   , ByVal strKatashiki As String _
                                   , ByVal strSVC_CD As String _
                                   , ByVal strDefaultMODEL_CD As String) As SC3250101DataSet.TB_M_INSPECTION_ORDER_ListDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7} P6:{8}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strMODEL_CD _
                  , strKatashiki _
                  , strSVC_CD _
                  , strDefaultMODEL_CD))

        'Dim dsSC3250101 As New SC3250101DataSet

        '全定期点検名を取得する
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            Dim dtSC3250101_SuggestList As New SC3250101DataSet.TB_M_INSPECTION_ORDER_ListDataTable
            '2019/12/02 NCN 吉川 TKM要件：型式対応 Start
            '①自店舗コード+型式で取得
            dtSC3250101_SuggestList = dsSC3250101.TB_M_INSPECTION_ORDER_ListSelect(strDLR_CD _
                                                                      , strBRN_CD _
                                                                      , strMODEL_CD _
                                                                      , strKatashiki _
                                                                      )
            If dtSC3250101_SuggestList.Count = 0 Then
                '②全店舗コード+型式で再取得
                dtSC3250101_SuggestList = dsSC3250101.TB_M_INSPECTION_ORDER_ListSelect(AllDealerCode _
                                                                                      , AllBranchCode _
                                                                                      , strMODEL_CD _
                                                                                      , strKatashiki _
                                                                                      )
            End If
            If Me.useFlgKatashiki Then
                If dtSC3250101_SuggestList.Count = 0 Then
                    dsSC3250101.SetUseFlgKatashiki(False)
                    SetUseFlgKatashiki(False)
                    '③自店舗+モデルで再取得
                    dtSC3250101_SuggestList = dsSC3250101.TB_M_INSPECTION_ORDER_ListSelect(strDLR_CD _
                                                                  , strBRN_CD _
                                                                  , strMODEL_CD _
                                                                  , strKatashiki _
                                                                  )
                End If
                If dtSC3250101_SuggestList.Count = 0 Then
                    '④全店舗+モデルで取得
                dtSC3250101_SuggestList = dsSC3250101.TB_M_INSPECTION_ORDER_ListSelect(AllDealerCode _
                                                                                      , AllBranchCode _
                                                                                      , strMODEL_CD _
                                                                  , strKatashiki _
                                                                                      )
            End If
            End If
            '2019/12/02 NCN 吉川 TKM要件：型式対応 End


            If dtSC3250101_SuggestList.Count = 0 Then
                '⑤デフォルトモデルコードで再取得
                dtSC3250101_SuggestList = dsSC3250101.TB_M_INSPECTION_ORDER_ListSelect(strDLR_CD _
                                                                                      , strBRN_CD _
                                                                                      , strDefaultMODEL_CD _
                                                                                      , DEFAULT_KATASHIKI_SPACE _
                                                                                      )
            End If

            If dtSC3250101_SuggestList.Count = 0 Then
                '⑥デフォルトモデルコードの全店舗コードで再取得
                dtSC3250101_SuggestList = dsSC3250101.TB_M_INSPECTION_ORDER_ListSelect(AllDealerCode _
                                                                                      , AllBranchCode _
                                                                                      , strDefaultMODEL_CD _
                                                                                      , DEFAULT_KATASHIKI_SPACE _
                                                                                      )
            End If

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250101_SuggestList.Count))

            Return dtSC3250101_SuggestList

        End Using

    End Function
    '2019/07/05　TKM要件:型式対応　END　↑↑↑

    ''' <summary>
    ''' 商品訴求データベースより、実績、一時WKに保存されているサービスコードを取得する
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <returns>サービスコード</returns>
    ''' <remarks></remarks>
    Public Function GetSuggestFromREPAIR_SUGGESTION(ByVal strDLR_CD As String _
                                                    , ByVal strBRN_CD As String _
                                                    , ByVal strSTF_CD As String _
                                                    , ByVal strSAChipID As String _
                                                    ) As String

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strSTF_CD _
                  , strSAChipID))

        'Dim dsSC3250101 As New SC3250101DataSet
        'Dim dtSC3250101R As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
        'Dim dtSC3250101W As New SC3250101DataSet.TB_W_REPAIR_SUGGESTIONDataTable
        Dim ret As String = Nothing

        '実績データから取得する
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dtSC3250101R As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
            Dim dtSC3250101W As New SC3250101DataSet.TB_W_REPAIR_SUGGESTIONDataTable

            dtSC3250101R = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_Select(strDLR_CD, strBRN_CD, strSTF_CD, strSAChipID)

            If dtSC3250101R IsNot Nothing AndAlso 0 < dtSC3250101R.Count Then
                'SVC_CDを取得する
                ret = dtSC3250101R(0).SVC_CD
            End If

            '一時WKから取得する（一時WKにあれば、こちらを優先）
            dtSC3250101W = dsSC3250101.TB_W_REPAIR_SUGGESTION_Select(strDLR_CD, strBRN_CD, strSTF_CD, strSAChipID)

            If dtSC3250101W IsNot Nothing AndAlso 0 < dtSC3250101W.Count Then
                'SVC_CDを取得する
                ret = dtSC3250101W(0).SVC_CD
            End If
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ret))

        Return ret

    End Function

    ''' <summary>
    ''' 一時ワーク、実績データベースの削除とWebサービス送信（DL送信）作業
    ''' </summary>
    ''' <returns>0：成功　-1：失敗</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function DeleteSuggestionResultProcess(ByVal strDLR_CD As String _
                                                  , ByVal strBRN_CD As String _
                                                  , ByVal strSTF_CD As String _
                                                  , ByVal strSAChipID As String _
                                                  , ByVal strSVC_CD As String _
                                                  , ByVal dtSC3250101R As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable _
                                                  , ByVal xmlWebService As ServiceItemsXmlDocumentClass) As Integer

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7} P6(Count):{8}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strSTF_CD _
                  , strSAChipID _
                  , strSVC_CD _
                  , dtSC3250101R.Rows.Count))

        Dim ProcResult As Integer = 0

        '①画面データ一時WKを削除する
        ProcResult = DeleteAllWorkData(strDLR_CD, strBRN_CD, strSTF_CD, strSAChipID, strSVC_CD)

        '②実績データを削除する
        If dtSC3250101R IsNot Nothing AndAlso 0 < dtSC3250101R.Count Then
            ProcResult = DeleteRepairSuggestionResult(dtSC3250101R)
        End If

        '③Webサービス送信する
        If xmlWebService IsNot Nothing And ProcResult <> DATABASE_ERROR Then
            Using BizSrv As New SC3250101WebServiceClassBusinessLogic
                Dim RetCode As String
                RetCode = BizSrv.CallGetServiceItemsWebService(xmlWebService)

                If RetCode <> ServiceSuccess Then
                    'Webサービスにてエラーが発生
                    ProcResult = WEBSERVICE_ERROR
                End If
            End Using
        End If

        'テスト用
        'ProcResult = 0

        'Webサービス送信処理に失敗していたらロールバック
        If ProcResult = DATABASE_ERROR Or ProcResult = WEBSERVICE_ERROR Then
            Me.Rollback = True
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ProcResult))

        Return ProcResult

    End Function

    ''' <summary>
    ''' 指定したサービスコードの実績データを取得する
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <param name="strSVC_CD">サービスコード</param>
    ''' <returns>実績データ（TB_T_REPAIR_SUGGESTION_RSLTDataTable）</returns>
    ''' <remarks></remarks>
    Public Function GetRepairSuggestionResult(ByVal strDLR_CD As String _
                                              , ByVal strBRN_CD As String _
                                              , ByVal strSTF_CD As String _
                                              , ByVal strSAChipID As String _
                                              , ByVal strSVC_CD As String _
                                              ) As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strSTF_CD _
                  , strSAChipID _
                  , strSVC_CD))

        'Dim dsSC3250101 As New SC3250101DataSet

        '実績データから取得する
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dtSC3250101R As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
            dtSC3250101R = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_Select(strDLR_CD, strBRN_CD, strSTF_CD, strSAChipID, strSVC_CD)

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250101R.Count))


            Return dtSC3250101R

        End Using

    End Function

    '【追加要件３】．デフォルトのカムリを変更する　START　↓↓↓
    'TODO: ★製造中：【■追加要件３．デフォルトのカムリを変更する】　DB特定後、すぐに対応できるようにしておく
    ''' <summary>
    ''' デフォルトのモデルコードを取得する
    ''' </summary>
    ''' <returns>デフォルトモデルコード</returns>
    ''' <remarks>モデルコードの取得に失敗したときはCARYを返す</remarks>
    Public Function GetDefaultModelCode(ByVal strDLR_CD As String) As String

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strDLR_CD))

        Dim DefaultModelCode As String = String.Empty

        Using dsSC3250101 As New SC3250101DataSet
            Dim dtSC3250101 As New SC3250101DataSet.TB_M_DEALERDataTable                           '←【要修正　DataTable　列追加】
            dtSC3250101 = dsSC3250101.TB_M_DEALER_Select(strDLR_CD)                                 '←【要修正　DataAccessメソッドSQL修正】

            If 0 < dtSC3250101.Count Then
                If Not String.IsNullOrWhiteSpace(dtSC3250101(0).DLR_CD) Then     '←【要修正　カラム名未定】
                    '取得成功
                    DefaultModelCode = dtSC3250101(0).DLR_CD                      '←【要修正　カラム名未定】
                Else
                    '取得結果が空白だった　→　CARY
                    DefaultModelCode = DEFAULT_MODEL_CD                                             '←【132行目に定数宣言あり】
                End If
            Else
                '取得失敗　→　CARY
                DefaultModelCode = DEFAULT_MODEL_CD
            End If
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , DefaultModelCode))

        Return DefaultModelCode
    End Function
    '【追加要件３】．デフォルトのカムリを変更する　END　　↑↑↑

    '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　START　↓↓↓
    ''' <summary>
    ''' R/O番号一覧を取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRoList(ByVal strVCL_VIN As String) As SC3250101DataSet.RO_NUM_ListDataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strVCL_VIN))
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dtSC3250101 As New SC3250101DataSet.RO_NUM_ListDataTable
            dtSC3250101 = dsSC3250101.GetRoListData(strVCL_VIN)

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} Return(Count):{3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250101.Rows.Count))

            Return dtSC3250101

        End Using

    End Function
    '2014/09/03　ResultリストをR/O単位でまとめる仕様に変更　END　　↑↑↑

    '2015/04/14 新販売店追加対応 start
    ''' <summary>
    ''' マスタに販売店が登録されているか判定する
    ''' </summary>
    ''' <param name="strDlrCd">販売店コード</param>
    ''' <returns>登録状態</returns>
    ''' <remarks>整備属性マスタに指定の販売店データが登録されているかをフラグで取得する</remarks>
    Public Function ChkDlrCdExistMst(ByVal strDlrCd As String) As Boolean

        Dim ret As Boolean = False
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            'マスタに販売店が登録されているか判定する
            ret = dsSC3250101.ChkDlrCdExistMst(strDlrCd)
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ret

    End Function

#End Region

#Region "非公開メソッド"

    ''' <summary>
    ''' 実績データベースの削除
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeleteRepairSuggestionResult(ByVal dtSC3250101R As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable) As Integer

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1(Count):{3}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , dtSC3250101R.Count))

        '実績データを削除する
        'Dim dsSC3250101 As New SC3250101DataSet
        Dim ret As Integer

        If dtSC3250101R IsNot Nothing AndAlso 0 < dtSC3250101R.Count Then
            '取得したデータを削除する
            '2019/07/05　TKM要件:型式対応　START　↓↓↓
            Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
                '2019/07/05　TKM要件:型式対応　END　↑↑↑
                For Each rows As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTRow In dtSC3250101R
                    '取り出したINSPEC_ITEM_CDを一時WKから削除する
                    ret = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_Delete(rows.DLR_CD _
                                                                    , rows.BRN_CD _
                                                                    , rows.RO_NUM _
                                                                    , rows.SVC_CD _
                                                                    , rows.INSPEC_ITEM_CD)

                    If ret = 0 Then
                        ret = DATABASE_ERROR
                        Exit For
                    End If
                Next
            End Using
        End If

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ret))

        Return ret

    End Function

    ''' <summary>
    ''' 商品訴求画面データWK　一括削除処理
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <param name="strSVC_CD">点検種類（未指定時は全点検種類）</param>
    ''' <returns>1：成功　-1：失敗</returns>
    ''' <remarks></remarks>
    Private Function DeleteAllWorkData(ByVal strDLR_CD As String _
                                     , ByVal strBRN_CD As String _
                                     , ByVal strSTF_CD As String _
                                     , ByVal strSAChipID As String _
                                     , Optional ByVal strSVC_CD As String = "" _
                                     ) As Integer

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strSTF_CD _
                  , strSAChipID _
                  , strSVC_CD))

        'Dim dsSC3250101 As New SC3250101DataSet
        'Dim dtSC3250101 As New SC3250101DataSet.TB_W_REPAIR_SUGGESTIONDataTable
        Dim ret As Integer

        '一時WKから取得する
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑
            Dim dtSC3250101 As New SC3250101DataSet.TB_W_REPAIR_SUGGESTIONDataTable
            dtSC3250101 = dsSC3250101.TB_W_REPAIR_SUGGESTION_Select(strDLR_CD _
                                                                    , strBRN_CD _
                                                                    , strSTF_CD _
                                                                    , strSAChipID _
                                                                    , strSVC_CD _
                                                                    )

            If dtSC3250101 IsNot Nothing AndAlso 0 < dtSC3250101.Count Then

                '一時WKから削除する
                For Each rows As SC3250101DataSet.TB_W_REPAIR_SUGGESTIONRow In dtSC3250101
                    '取り出したINSPEC_ITEM_CDを一時WKから削除する
                    ret = dsSC3250101.TB_W_REPAIR_SUGGESTION_Delete(rows.DLR_CD _
                                                                    , rows.BRN_CD _
                                                                    , rows.STF_CD _
                                                                    , rows.RO_NUM _
                                                                    , rows.SVC_CD _
                                                                    , rows.INSPEC_ITEM_CD _
                                                                    )

                    If ret = 0 Then
                        ret = DATABASE_ERROR
                        Exit For
                    End If
                Next
            End If
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ret))

        Return ret

    End Function

    '2販社 BTS310 横展開修正 追加・更新排他制御追加 2015/04/07
    'Public→Privateに変更
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　START　↓↓↓
    ''' <summary>
    ''' 商品訴求画面データWKへの登録・更新処理
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strSAChipID">来店実績連番</param>
    ''' <param name="strSVC_CD">点検種類</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <param name="strSUGGEST_ICON">Suggest（明細）アイコン</param>
    ''' <returns>登録結果 1：正常終了　-1：失敗　99：登録なし</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Private Function Set_TB_W_REPAIR_SUGGESTION_Process( _
                            ByVal strDLR_CD As String _
                          , ByVal strBRN_CD As String _
                          , ByVal strSTF_CD As String _
                          , ByVal strSAChipID As String _
                          , ByVal strSVC_CD As String _
                          , ByVal strINSPEC_ITEM_CD As String _
                          , ByVal strSUGGEST_ICON As String
                          ) As Integer

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} P1:{3} P2:{4} P3:{5}, P4:{6} P5:{7} P6:{8} P7:{9}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_START _
                   , strDLR_CD _
                   , strBRN_CD _
                   , strSTF_CD _
                   , strSAChipID _
                   , strSVC_CD _
                   , strINSPEC_ITEM_CD _
                   , strSUGGEST_ICON))

        Dim ret As Integer

        'Dim dsSC3250101 As New SC3250101DataSet
        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dsSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑

            Dim dtSC3250101 As New SC3250101DataSet.TB_W_REPAIR_SUGGESTIONDataTable

            '対象データを商品訴求画面データWKより取得
            dtSC3250101 = dsSC3250101.TB_W_REPAIR_SUGGESTION_Select(strDLR_CD, strBRN_CD, strSTF_CD, strSAChipID, strSVC_CD, strINSPEC_ITEM_CD)

            '対象データを登録
            Select Case True
                Case dtSC3250101.Rows.Count = 0
                    '対象データが対象データを商品訴求画面データWKに未登録である場合、新規登録
                    ret = dsSC3250101.TB_W_REPAIR_SUGGESTION_Insert( _
                                                              strDLR_CD _
                                                            , strBRN_CD _
                                                            , strSTF_CD _
                                                            , strSAChipID _
                                                            , strSVC_CD _
                                                            , strINSPEC_ITEM_CD _
                                                            , strSUGGEST_ICON _
                                                            , strSTF_CD
                                                            )

                    '新規登録に失敗していたらロールバック
                    If ret = 0 Then
                        ' BTS310 
                        'Me.Rollback = True
                        ret = -1
                    Else
                        ret = 1
                    End If

                Case 0 < dtSC3250101.Rows.Count
                    '対象データが対象データを商品訴求画面データWKに登録済である場合、更新
                    ret = dsSC3250101.TB_W_REPAIR_SUGGESTION_Update( _
                                                              strDLR_CD _
                                                            , strBRN_CD _
                                                            , strSTF_CD _
                                                            , strSAChipID _
                                                            , strSVC_CD _
                                                            , strINSPEC_ITEM_CD _
                                                            , strSUGGEST_ICON _
                                                            , strSTF_CD _
                                                            )

                    '新規登録に失敗していたらロールバック
                    If ret = 0 Then
                        '
                        'Me.Rollback = True
                        ret = -1
                    Else
                        ret = 1
                    End If

                Case Else
                    '更新作業なし
                    ret = 99

            End Select
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} Return:{3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , ret))

        Return ret

    End Function
    '2014/06/09 車両不明時も登録できるように変更(strRO_NUM → strSAChipID)　END　　↑↑↑

#End Region

    '2017/XX/XX ライフサイクル対応　↓
    ''' <summary>
    ''' RO Active存在チェック ActiveにROが存在する場合に真を帰す
    ''' </summary>
    ''' <param name="argDlrCd">販売店コード</param>
    ''' <param name="argBrnCd">店舗コード</param>
    ''' <param name="argRoNum">RO番号</param>
    ''' <returns>True:Activeに存在する False:Activeに存在しない</returns>
    ''' <remarks></remarks>
    Public Function ChkExistParamRoActive(ByVal argDlrCd As String, _
                                           ByVal argBrnCd As String, _
                                           ByVal argRoNum As String) As Boolean

        Dim isExistActive As Boolean = False

        'If String.IsNullOrEmpty(argRoNum) OrElse String.IsNullorWhiteSpace(argRoNum) Then
        '   isExistActive = True
        '   Return isExistActive
        'End If

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        Using dcSC3250101 As New SC3250101DataSet(Me.useFlgKatashiki)
            '2019/07/05　TKM要件:型式対応　END　↑↑↑

            isExistActive = dcSC3250101.ChkExistParamRoActive(argDlrCd, argBrnCd, argRoNum)

        End Using

        Return isExistActive

    End Function
    '2017/XX/XX ライフサイクル対応　↑

#Region "未使用メソッド"
#Region "未使用メソッド"

    ' ''' <summary>
    ' ''' 登録済データ確認処理
    ' ''' </summary>
    ' ''' <param name="strDLR_CD">販売店コード</param>
    ' ''' <param name="strBRN_CD">店舗コード</param>
    ' ''' <param name="strSTF_CD">スタッフコード</param>
    ' ''' <param name="strRO_NUM">RO番号</param>
    ' ''' <param name="strSVC_CD">点検種類</param>
    ' ''' <param name="dt">点検項目コードのリスト</param>
    ' ''' <remarks></remarks>
    'Public Sub SetIsRegisted( _
    '                              ByVal strDLR_CD As String _
    '                            , ByVal strBRN_CD As String _
    '                            , ByVal strSTF_CD As String _
    '                            , ByVal strRO_NUM As String _
    '                            , ByVal strSVC_CD As String _
    '                            , ByRef dt As DataTable
    '                            )
    '    '開始ログの記録
    '    Logger.Info(String.Format("SetIsRegisted_Start, strDLR_CD:[{0}], strBRN_CD:[{1}], strSTF_CD:[{2}], strRO_NUM[{3}], strSVC_CD:[{4}], dt_Count:[{5}]" _
    '                              , strDLR_CD _
    '                              , strBRN_CD _
    '                              , strSTF_CD _
    '                              , strRO_NUM _
    '                              , strSVC_CD _
    '                              , dt.Rows.Count.ToString))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
    '    Dim strINSPEC_ITEM_CD As String = String.Empty
    '    'Dim strSUGGEST_ICON As String = String.Empty

    '    For Each row As DataRow In dt.Rows
    '        strINSPEC_ITEM_CD = row("INSPEC_ITEM_CD").ToString
    '        dtSC3250101 = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_Select(strDLR_CD, strBRN_CD, strSTF_CD, strRO_NUM, strSVC_CD, strINSPEC_ITEM_CD)

    '        '未登録データは変更有として登録対象データにする
    '        row("ChangeFlag") = If(dtSC3250101.Rows.Count = 0, 1, 0)
    '    Next

    '    '終了ログの記録
    '    Logger.Info("SetIsRegisted_End")

    'End Sub

#End Region

#Region "未使用メソッド"

    ' ''' <summary>
    ' ''' 商品訴求登録実績データに既に登録されているか確認する
    ' ''' </summary>
    ' ''' <param name="strDLR_CD">販売店コード</param>
    ' ''' <param name="strBRN_CD">店舗コード</param>
    ' ''' <param name="strSTF_CD">スタッフコード</param>
    ' ''' <param name="strRO_NUM">RO番号</param>
    ' ''' <param name="strSVC_CD">点検種類</param>
    ' ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ' ''' <param name="strSUGGEST_ICON">Suggest（明細）アイコン</param>
    ' ''' <returns>True：登録済み　False：未登録</returns>
    ' ''' <remarks></remarks>
    'Public Function CanGetFromTB_T_REPAIR_SUGGESTION_RSLT( _
    '                        ByVal strDLR_CD As String _
    '                      , ByVal strBRN_CD As String _
    '                      , ByVal strSTF_CD As String _
    '                      , ByVal strRO_NUM As String _
    '                      , ByVal strSVC_CD As String _
    '                      , ByVal strINSPEC_ITEM_CD As String _
    '                      , ByVal strSUGGEST_ICON As String
    '                      ) As Boolean
    '    '開始ログの記録
    '    Logger.Info(String.Format("CanGetFromTB_T_REPAIR_SUGGESTION_RSLT_Start, strDLR_CD:[{0}], strBRN_CD:[{1}], strSTF_CD:[{2}], strRO_NUM:[{3}], strSVC_CD:[{4}], strINSPEC_ITEM_CD:[{5}], strSUGGEST_ICON:[{6}]" _
    '                              , strDLR_CD _
    '                              , strBRN_CD _
    '                              , strSTF_CD _
    '                              , strRO_NUM _
    '                              , strSVC_CD _
    '                              , strINSPEC_ITEM_CD _
    '                              , strSUGGEST_ICON))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
    '    Dim RetExsist As Boolean

    '    '対象データを商品訴求登録実績データより取得
    '    dtSC3250101 = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_Select(strDLR_CD, strBRN_CD, strSTF_CD, strRO_NUM, strSVC_CD, strINSPEC_ITEM_CD)
    '    '対象データを登録
    '    Select Case True
    '        Case dtSC3250101.Rows.Count = 0
    '            '登録なし
    '            RetExsist = False
    '            'Return False

    '        Case 0 < dtSC3250101.Rows.Count
    '            '登録済み
    '            If strSUGGEST_ICON = dtSC3250101.Rows(0)("SUGGEST_ICON").ToString Then
    '                '同じだったら戻る
    '                RetExsist = True
    '                'Return True
    '            Else
    '                '登録済みでもSUGGESTの内容が異なる
    '                RetExsist = False
    '                'Return False
    '            End If

    '        Case Else
    '            '更新作業なし
    '            RetExsist = True
    '            'Return True
    '    End Select

    '    '終了ログの記録
    '    Logger.Info(String.Format("CanGetFromTB_T_REPAIR_SUGGESTION_RSLT_End, Return:[{0}]", RetExsist.ToString))

    '    Return RetExsist

    'End Function

#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 商品訴求登録実績データへの登録・更新処理
    ' ''' </summary>
    ' ''' <param name="strDLR_CD">販売店コード</param>
    ' ''' <param name="strBRN_CD">店舗コード</param>
    ' ''' <param name="strSTF_CD">スタッフコード</param>
    ' ''' <param name="strRO_NUM">RO番号</param>
    ' ''' <param name="strSVC_CD">点検種類</param>
    ' ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ' ''' <param name="strSUGGEST_ICON">Suggest（明細）アイコン</param>
    ' ''' <returns>登録結果 1：正常終了　-1：失敗　99：登録なし</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function Register( _
    '                        ByVal strDLR_CD As String _
    '                      , ByVal strBRN_CD As String _
    '                      , ByVal strSTF_CD As String _
    '                      , ByVal strRO_NUM As String _
    '                      , ByVal strSVC_CD As String _
    '                      , ByVal strINSPEC_ITEM_CD As String _
    '                      , ByVal strSUGGEST_ICON As String
    '                      ) As Integer

    '    '開始ログの記録
    '    Logger.Info(String.Format("Register_Start, strDLR_CD:[{0}], strBRN_CD:[{1}], strSTF_CD:[{2}], strRO_NUM:[{3}], strSVC_CD:[{4}], strINSPEC_ITEM_CD:[{5}], strSUGGEST_ICON:[{6}]" _
    '                , strDLR_CD _
    '                , strBRN_CD _
    '                , strSTF_CD _
    '                , strRO_NUM _
    '                , strSVC_CD _
    '                , strINSPEC_ITEM_CD _
    '                , strSUGGEST_ICON _
    '                ))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
    '    Dim ret As Integer

    '    '対象データを商品訴求登録実績データより取得
    '    dtSC3250101 = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_Select(strDLR_CD, strBRN_CD, strSTF_CD, strRO_NUM, strSVC_CD, strINSPEC_ITEM_CD)

    '    '対象データを登録
    '    Select Case True
    '        Case dtSC3250101.Rows.Count = 0
    '            '対象データが対象データを商品訴求登録実績データに未登録である場合、新規登録
    '            ret = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_Insert( _
    '                                                      strDLR_CD _
    '                                                    , strBRN_CD _
    '                                                    , strSTF_CD _
    '                                                    , strRO_NUM _
    '                                                    , strSVC_CD _
    '                                                    , strINSPEC_ITEM_CD _
    '                                                    , strSUGGEST_ICON _
    '                                                    , FIRST_TIME
    '                                                    )

    '            '新規登録に失敗していたらロールバック
    '            If ret = 0 Then
    '                Me.Rollback = True
    '                ret = -1
    '            Else
    '                ret = 1
    '            End If

    '        Case 0 < dtSC3250101.Rows.Count
    '            '登録済みの場合、SUGGEST_ICONが同じかどうか確認して、違っていたら更新する
    '            If strSUGGEST_ICON = dtSC3250101.Rows(0)("SUGGEST_ICON").ToString Then
    '                '同じだったら戻る
    '                ret = 99
    '            End If

    '            '対象データが対象データを商品訴求登録実績データに登録済である場合、更新
    '            ret = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_Update( _
    '                                                      strDLR_CD _
    '                                                    , strBRN_CD _
    '                                                    , strSTF_CD _
    '                                                    , strRO_NUM _
    '                                                    , strSVC_CD _
    '                                                    , strINSPEC_ITEM_CD _
    '                                                    , strSUGGEST_ICON _
    '                                                    )

    '            '新規登録に失敗していたらロールバック
    '            If ret = 0 Then
    '                Me.Rollback = True
    '                ret = -1
    '            Else
    '                ret = 1
    '            End If

    '        Case Else
    '            '更新作業なし
    '            ret = 99

    '    End Select

    '    '終了ログの記録
    '    Logger.Info(String.Format("Register_End, Return:[{0}]", ret))

    '    Return ret

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 商品訴求画面データWK削除処理
    ' ''' </summary>
    ' ''' <param name="strDLR_CD">販売店コード</param>
    ' ''' <param name="strBRN_CD">店舗コード</param>
    ' ''' <param name="strSTF_CD">スタッフコード</param>
    ' ''' <param name="strRO_NUM">RO番号</param>
    ' ''' <param name="strSVC_CD">点検種類</param>
    ' ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ' ''' <returns>結果 1：正常終了　-1：失敗</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function DeleteWork( _
    '                        ByVal strDLR_CD As String _
    '                      , ByVal strBRN_CD As String _
    '                      , ByVal strSTF_CD As String _
    '                      , ByVal strRO_NUM As String _
    '                      , ByVal strSVC_CD As String _
    '                      , ByVal strINSPEC_ITEM_CD As String
    '                      ) As Integer

    '    '開始ログの記録
    '    Logger.Info(String.Format("DeleteWork_Start, strDLR_CD:[{0}], strBRN_CD:[{1}], strSTF_CD:[{2}], strRO_NUM:[{3}], strSVC_CD:[{4}], strINSPEC_ITEM_CD:[{5}]" _
    '                , strDLR_CD _
    '                , strBRN_CD _
    '                , strSTF_CD _
    '                , strRO_NUM _
    '                , strSVC_CD _
    '                , strINSPEC_ITEM_CD _
    '                ))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLTDataTable
    '    Dim ret As Integer

    '    'ワークテーブルから削除
    '    ret = dsSC3250101.TB_W_REPAIR_SUGGESTION_Delete(
    '                                              strDLR_CD _
    '                                            , strBRN_CD _
    '                                            , strSTF_CD _
    '                                            , strRO_NUM _
    '                                            , strSVC_CD _
    '                                            , strINSPEC_ITEM_CD _
    '                                                )

    '    '新規登録に失敗していたらロールバック
    '    If ret = 0 Then
    '        Me.Rollback = True
    '        ret = -1
    '    Else
    '        ret = 1
    '    End If

    '    '終了ログの記録
    '    Logger.Info(String.Format("DeleteWork_End, Return:[{0}]", ret))

    '    Return ret

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 車輌マスタ取得処理
    ' ''' </summary>
    ' ''' <param name="strVCL_VIN">モデルコード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetVehicleInfo(ByVal strVCL_VIN As String) As SC3250101DataSet.TB_M_VEHICLEDataTable
    '    '開始ログの記録
    '    Logger.Info(String.Format("GetVehicleInfo_START, strVCL_VIN:[{0}]", strVCL_VIN))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_M_VEHICLEDataTable

    '    dtSC3250101 = dsSC3250101.TB_M_VEHICLE_Select(strVCL_VIN)

    '    '開始ログの記録
    '    Logger.Info(String.Format("GetVehicleInfo_End, Return_Count:[{0}]", dtSC3250101.Rows.Count.ToString))

    '    Return dtSC3250101

    'End Function
#End Region

#Region "未使用メソッド"

    ' ''' <summary>
    ' ''' グレードマスタ取得処理
    ' ''' </summary>
    ' ''' <param name="strMODEL_CD">モデルコード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetGradeInfo(ByVal strMODEL_CD As String) As SC3250101DataSet.TB_M_GRADEDataTable
    '    '開始ログの記録
    '    Logger.Info(String.Format("GetGradeInfo_Start, strMODEL_CD:[{0}]", strMODEL_CD))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_M_GRADEDataTable

    '    dtSC3250101 = dsSC3250101.TB_M_GRADE_Select(strMODEL_CD)

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetGradeInfo_End, Return_Count:[{0}]", dtSC3250101.Rows.Count.ToString))

    '    Return dtSC3250101

    'End Function

#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 登録済データ取得処理
    ' ''' </summary>
    ' ''' <param name="strMODEL_CD">モデルコード</param>
    ' ''' <returns></returns>
    ' ''' <remarks>未使用です</remarks>
    'Public Function GetRegistData(ByVal strMODEL_CD As String) As SC3250101DataSet.RESULT_DATADataTable
    '    '開始ログの記録
    '    Logger.Info(String.Format("GetRegistData_START, strMODEL_CD:[{0}]", strMODEL_CD))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.RESULT_DATADataTable

    '    dtSC3250101 = dsSC3250101.RESULT_DATA_Select(strMODEL_CD)

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetRegistData_End, Return_Count:[{0}]", dtSC3250101.Rows.Count.ToString))

    '    Return dtSC3250101
    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 次点検種類取得処理
    ' ''' </summary>
    ' ''' <param name="strMODEL_CD">モデルコード</param>
    ' ''' <param name="strGRADE_CD">グレードコード</param>
    ' ''' <param name="staffInfo">点検種類</param>
    ' ''' <returns></returns>
    ' ''' <remarks>未使用です</remarks>
    'Public Function GetNextSuggest( _
    '                                ByVal strMODEL_CD As String _
    '                              , ByVal strGRADE_CD As String _
    '                              , ByVal staffInfo As StaffContext
    '                              ) As String
    '    '開始ログの記録
    '    Logger.Info(String.Format("GetNextSuggest_START, strMODEL_CD:[{0}], strGRADE_CD:[{1}], staffInfo.DlrCD:[{2}], staffInfo.BrnCD:[{3}]" _
    '                              , strMODEL_CD _
    '                              , strGRADE_CD _
    '                              , staffInfo.DlrCD _
    '                              , staffInfo.BrnCD _
    '                              ))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101_01 As New SC3250101DataSet.TB_M_INSPECTION_ORDERDataTable
    '    Dim dtSC3250101_02 As New SC3250101DataSet.TB_M_INSPECTION_COMBDataTable
    '    Dim NextInspecType As String = String.Empty

    '    '点検順番マスタ取得
    '    dtSC3250101_01 = dsSC3250101.TB_M_INSPECTION_ORDER_Select()
    '    '直近で更新された点検組み合わせマスタのデータを取得
    '    dtSC3250101_02 = dsSC3250101.TB_M_INSPECTION_COMB_LATEST_Select(strMODEL_CD, strGRADE_CD, staffInfo.DlrCD, staffInfo.BrnCD)

    '    If 0 < dtSC3250101_02.Rows.Count Then
    '        '最終点検実績の次の点検順序の点検種類を表示する
    '        'カレントの点検順序が最後である場合は空白表示する為、最終レコードの一つ前までループ
    '        For i As Integer = 0 To dtSC3250101_01.Rows.Count - 2
    '            If dtSC3250101_02.Rows(0)("SVC_CD").ToString = dtSC3250101_01.Rows(i)("SVC_CD").ToString Then
    '                NextInspecType = dtSC3250101_01.Rows(i + 1)("SVC_CD").ToString
    '                Exit For
    '            End If
    '        Next
    '    End If

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetNextSuggest_End, Return:[{0}]", NextInspecType))

    '    Return NextInspecType

    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 商品訴求登録実績テーブルよりデータ取得
    ' ''' </summary>
    ' ''' <param name="strDLR_CD">販売店コード</param>
    ' ''' <param name="strBRN_CD">店舗コード</param>
    ' ''' <param name="strSTF_CD">アカウント</param>
    ' ''' <param name="strRO_NUM">RO番号</param>
    ' ''' <param name="strSVC_CD">点検種別</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetRepairSuggestionRsltOfPart( _
    '                            ByVal strDLR_CD As String _
    '                            , ByVal strBRN_CD As String _
    '                            , ByVal strSTF_CD As String _
    '                            , ByVal strRO_NUM As String _
    '                            , ByVal strSVC_CD As String _
    '                      ) As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable

    '    '開始ログの記録
    '    Logger.Info(String.Format("GetRepairSuggestionRsltOfPart_Start, strDLR_CD:[{0}], strBRN_CD:[{1}], strSTF_CD:[{2}], strRO_NUM:[{3}], strSVC_CD:[{4}]" _
    '                              , strDLR_CD _
    '                              , strBRN_CD _
    '                              , strSTF_CD _
    '                              , strRO_NUM _
    '                              , strSVC_CD _
    '                              ))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable

    '    '部位に関連する点検項目取得
    '    dtSC3250101 = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_OF_PART_Select(strDLR_CD, strBRN_CD, strSTF_CD, strRO_NUM, strSVC_CD)

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetRepairSuggestionRsltOfPart_End, Return_Count:[{0}]", dtSC3250101.Rows.Count))

    '    Return dtSC3250101
    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 商品訴求画面WORKテーブルよりデータ取得
    ' ''' </summary>
    ' ''' <param name="strBRN_CD">販売店コード</param>
    ' ''' <param name="strDLR_CD">店舗コード</param>
    ' ''' <param name="strSTF_CD">アカウント</param>
    ' ''' <param name="strRO_NUM">RO番号</param>
    ' ''' <param name="strSVC_CD">点検種別</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetWorkRepairSuggestionRsltOfPart( _
    '                              ByVal strDLR_CD As String _
    '                            , ByVal strBRN_CD As String _
    '                            , ByVal strSTF_CD As String _
    '                            , ByVal strRO_NUM As String _
    '                            , ByVal strSVC_CD As String _
    '                      ) As SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable
    '    '開始ログの記録
    '    Logger.Info(String.Format("GetWorkRepairSuggestionRsltOfPart_Start, strDLR_CD:[{0}], strBRN_CD:[{1}], strSTF_CD:[{2}], strRO_NUM:[{3}], strSVC_CD:[{4}]" _
    '                              , strDLR_CD _
    '                              , strBRN_CD _
    '                              , strSTF_CD _
    '                              , strRO_NUM _
    '                              , strSVC_CD _
    '                              ))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_T_REPAIR_SUGGESTION_RSLT_OF_PARTDataTable

    '    '部位に関連する点検項目取得
    '    dtSC3250101 = dsSC3250101.TB_W_REPAIR_SUGGESTION_Select_List(strDLR_CD, strBRN_CD, strSTF_CD, strRO_NUM, strSVC_CD)

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetWorkRepairSuggestionRsltOfPart_End, Return_Count:[{0}]", dtSC3250101.Rows.Count))

    '    Return dtSC3250101
    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' すべての点検項目を取得する
    ' ''' </summary>
    ' ''' <param name="strModel_CD">モデルコード</param>
    ' ''' <param name="strGrade_CD">グレードコード</param>
    ' ''' <param name="strSVC_CD">点検種類</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetAllInspectionList( _
    '                            ByVal strModel_CD As String _
    '                            , ByVal strGrade_CD As String _
    '                            , ByVal strSVC_CD As String _
    '                      ) As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable
    '    '開始ログの記録
    '    Logger.Info(String.Format("GetAllInspectionList_Start, strModel_CD:[{0}], strGrade_CD:[{1}], strSVC_CD:[{2}]" _
    '                              , strModel_CD _
    '                              , strGrade_CD _
    '                              , strSVC_CD _
    '                              ))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable

    '    '部位に関連する点検項目取得
    '    'dtSC3250101 = dsSC3250101.TB_T_REPAIR_SUGGESTION_RSLT_OF_PART_Select(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account, strRO_NUM, strSVC_CD)
    '    dtSC3250101 = dsSC3250101.TB_M_INSPECTION_COMB_Select(strModel_CD, strGrade_CD, strSVC_CD)

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetAllInspectionList_End, Return_Count:[{0}]", dtSC3250101.Rows.Count))

    '    Return dtSC3250101
    'End Function
#End Region

#Region "未使用メソッド"
    'Public Function GetInspectionList( _
    '                              ByVal strModel_CD As String _
    '                            , ByVal strGrade_CD As String _
    '                      ) As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable
    '    '開始ログの記録
    '    Logger.Info(String.Format("GetInspectionList_Start, strModel_CD:[{0}], strGrade_CD:[{1}]", strModel_CD, strGrade_CD))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable

    '    '部位に関連する点検項目取得
    '    dtSC3250101 = dsSC3250101.TB_M_INSPECTION_COMB_SelectList(strModel_CD, strGrade_CD)

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetInspectionList_End, Return_Count:[{0}]", dtSC3250101.Rows.Count.ToString))

    '    Return dtSC3250101
    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' 点検組み合わせマスタの商品訴求初期表示用アイテムを取得する
    ' ''' </summary>
    ' ''' <param name="strModel_CD">モデルコード</param>
    ' ''' <param name="strGrade_CD">グレードコード</param>
    ' ''' <param name="strSVC_CD">点検種類</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetSuggestDefaultList( _
    '                            ByVal strModel_CD As String _
    '                            , ByVal strGrade_CD As String _
    '                            , ByVal strSVC_CD As String _
    '                      ) As SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable

    '    '開始ログの記録
    '    Logger.Info(String.Format("GetSuggestDefaultList_Start, strModel_CD:[{0}], strGrade_CD:[{1}], strSVC_CD:[{2}]" _
    '                              , strModel_CD _
    '                              , strGrade_CD _
    '                              , strSVC_CD _
    '                              ))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_M_INSPECTION_COMB_TB_M_INSPECTION_DETAILDataTable

    '    '部位に関連する点検項目取得
    '    dtSC3250101 = dsSC3250101.TB_M_INSPECTION_COMB_DefaultList(strModel_CD, strGrade_CD, strSVC_CD)

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetSuggestDefaultList_End, Return_Count:[{0}]", dtSC3250101.Rows.Count.ToString))

    '    Return dtSC3250101
    'End Function
#End Region

#Region "未使用メソッド"
    ' ''' <summary>
    ' ''' オペレーション変換マスタによる変換後のモデルコードを取得
    ' ''' </summary>
    ' ''' <param name="strMODEL_CD"></param>
    ' ''' <returns>変換後のモデルコード</returns>
    ' ''' <remarks></remarks>
    'Public Function GetChangeModelCode(ByVal strMODEL_CD As String) As String

    '    '開始ログの記録
    '    Logger.Info(String.Format("GetChangeModelCode_START, strMODEL_CD:[{0}]", strMODEL_CD))

    '    Dim dsSC3250101 As New SC3250101DataSet
    '    Dim dtSC3250101 As New SC3250101DataSet.TB_M_OPERATION_CHANGEDataTable
    '    Dim ChangeModelCD As String = String.Empty

    '    '部位に関連する点検項目取得
    '    dtSC3250101 = dsSC3250101.TB_M_OPERATION_CHANGE_Select(strMODEL_CD)

    '    For Each row As SC3250101DataSet.TB_M_OPERATION_CHANGERow In dtSC3250101
    '        If Not String.IsNullOrWhiteSpace(row("MODEL_CD").ToString) Then
    '            ChangeModelCD = row("MODEL_CD").ToString
    '            Exit For
    '        End If
    '    Next

    '    '終了ログの記録
    '    Logger.Info(String.Format("GetChangeModelCode_END, Return:[{0}]", ChangeModelCD))

    '    Return ChangeModelCD
    'End Function
#End Region



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

    '【***完成検査_排他制御***】 start
    ''' <summary>
    ''' 商品訴求登録実績更新可能判定
    ''' </summary>
    ''' <param name="rowLockvs ">販売店コード</param>
    ''' <param name="DealerCode ">販売店コード</param>
    ''' <param name="BranchCode">店舗コード</param>
    ''' <param name="RoNum">RO番号</param>
    ''' <returns>True:更新なし/false:更新あり</returns>
    Public Function CheckUpdateRepairSuggestion(
                                           ByVal rowLockvs As Long,
                                           ByVal DealerCode As String,
                                           ByVal BranchCode As String,
                                           ByVal RoNum As String) As Boolean

        Dim result As Boolean = True
        Dim dcSC3250101 As New SC3250101DataSet

        If rowLockvs = GetServiceinRowLockVertion(DealerCode, BranchCode, RoNum) Then

            result = True

        Else
            result = False

        End If

        Return result

    End Function

    ''' <summary>
    ''' サービス入庫行ロックバージョン取得処理
    ''' </summary>
    ''' <param name="DealerCode ">販売店コード</param>
    ''' <param name="BranchCode">店舗コード</param>
    ''' <param name="RoNum">RO番号</param>
    ''' <returns>行ロックバージョン取得</returns>
    ''' <remarks></remarks>
    Public Function GetServiceinRowLockVertion(
                                           ByVal DealerCode As String,
                                           ByVal BranchCode As String,
                                           ByVal RoNum As String) As Long

        Dim nowRockversion As Long
        Dim dcSC3250101 As New SC3250101DataSet


        nowRockversion = dcSC3250101.GetServiceinRowLockVertion(DealerCode, BranchCode, RoNum)


        Return nowRockversion

    End Function
    '【***完成検査_排他制御***】 end

End Class
