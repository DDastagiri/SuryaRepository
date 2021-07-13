'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240201.ascx.vb
'─────────────────────────────────────
'機能： チップ詳細
'補足： 小と大の共通クラス
'作成： 2013/07/31 TMEJ 岩城 タブレット版SMB機能開発(工程管理)
'更新： 2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/01/13 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発
'更新： 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
'更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'更新： 2014/09/27 TMEJ 張「RO番号がない場合、 R/O参照ボタンを押すと、現地画面エラーが出る」対応
'更新： 2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）
'更新： 2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発
'更新： 2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
'更新： 2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力
'更新： 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成
'更新： 2015/06/26 TMEJ 明瀬 （トライ店システム評価）部品情報取得エラーメッセージ仕様の見直し
'更新： 2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新： 2016/10/14 NSK  秋田谷 TR-SVT-TMT-20160824-003（チップ詳細の実績時間を変更できなくする）の対応
'更新： 2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
'更新： 2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新： 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Reflection
Imports Toyota.eCRB.SMB.ChipDetail.BizLogic
Imports Toyota.eCRB.SMB.ChipDetail.DataAccess
Imports System.Reflection.MethodBase
Imports Toyota.eCRB.CommonUtility.Common.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
'2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
'2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END
'2014/01/13 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.BizLogic
'2014/01/13 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END
'2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
'2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

''' <summary>
''' チップ詳細
''' プレゼンテーションクラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3240201
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler

#Region "定数"

    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "SC3240201"

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' R/O参照画面のプログラムID
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const APPLICATIONID_ORDEROUT As String = "SC3160208"

    ' ''' <summary>
    ' ''' 顧客詳細画面のプログラムID
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const APPLICATIONID_CUSTOMEROUT As String = "SC3080208"

    ''' <summary>
    ''' R/O参照画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_ORDEROUT As String = "SC3010501"

    ''' <summary>
    ''' 顧客詳細画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_CUSTOMEROUT As String = "SC3080225"

    ''' <summary>
    ''' 入庫日時・納車日時の必須フラグ(システム設定値)
    ''' </summary>
    Private Const SYS_DATETIME_MANDATORY_FLG = "SCHE_SVCIN_DELI_DATETIME_MANDATORY_FLG"
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' コールバック時に画面を作成する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_DISPCREATE As String = "CreateChipDetailSL"

    ''' <summary>
    ''' コールバック時に整備名を取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_GETMERC As String = "CreateChipDetailMercSL"

    ''' <summary>
    ''' コールバック時に登録処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_REGISTER As String = "Register"

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' コールバック時に全開始処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_ALLSTART As String = "AllStart"

    ''' <summary>
    ''' コールバック時に全終了処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_ALLFINISH As String = "AllFinish"

    ''' <summary>
    ''' コールバック時に全中断処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_ALLSTOP As String = "AllStop"

    ''' <summary>
    ''' コールバック時に単独中断処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_SINGLESTOP As String = "SingleStop"

    ''' <summary>
    ''' コールバック時に単独開始処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_SINGLESTART As String = "SingleStart"

    ''' <summary>
    ''' コールバック時に単独終了処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_SINGLEFINISH As String = "SingleFinish"

    ''' <summary>
    ''' コールバック時に再開始処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_RESTART As String = "ReStart"

    ''' <summary>
    ''' 中断されてるJob再開ステータス(まだ設定してない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESTARTJOB_NOTSET As String = "0"

    ''' <summary>
    ''' 中断されてるJob再開ステータス(再開する)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESTARTJOB_YES As String = "1"

    ''' <summary>
    ''' 中断されてるJob再開ステータス(再開しない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RESTARTJOB_NO As String = "2"

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発  END

    ''' <summary>
    ''' 日時表示ラベルにyyyy/MM/dd HH:mm形式の文字列を保持しておく属性名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ATTR_DATETIME As String = "datetime"

    ''' <summary>
    ''' DateTimeSelectorに初期表示時の値を保持しておく属性名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ATTR_DATE As String = "date"

    ''' <summary>
    ''' サブチップボックス:受付ボックスID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBAREA_RECEPTION As String = "5"

    ''' <summary>
    ''' サブチップボックス:追加作業ボックスID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBAREA_ADDWORK As String = "15"

    ''' <summary>
    ''' コンボボックス初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const COMBO_INIT_VALUE As String = "0"

    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
    ''' <summary>
    ''' サービスステータス:納車済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcStatusDelivery As String = "13"
    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

    '2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 START

    ''' <summary>
    ''' サービス・商品項目必須区分(システム設定値)
    ''' </summary>
    Private Const SYS_MERC_MANDATORY_TYPE = "SVCIN_MERC_MANDATORY_TYPE"

    ''' <summary>
    ''' 属性名(商品選択可能フラグ)
    ''' </summary>
    Private Const ATT_MERC_ITEM = "MERCITEM"

    '2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

#End Region

#Region "列挙体"

    ''' <summary>
    ''' 列挙体 コールバック結果コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ResultCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        ''' <remarks></remarks>
        Success = 0

        ''' <summary>
        ''' 入力項目値チェックエラー
        ''' </summary>
        ''' <remarks></remarks>
        CheckError = 1

        ''' <summary>
        ''' 登録時の排他エラー
        ''' </summary>
        ''' <remarks></remarks>
        ExclusionError = 2

        ''' <summary>
        ''' 登録時の排他エラー
        ''' </summary>
        ''' <remarks></remarks>
        CollisionError = 3

        ''' <summary>
        ''' チップが他ユーザーに削除されていたエラー
        ''' </summary>
        ''' <remarks></remarks>
        OtherDeleteError = 4

        ''' <summary>
        ''' 商品データなし
        ''' </summary>
        ''' <remarks></remarks>
        MercDataNothing = 5

        ''' <summary>
        ''' DBタイムアウトエラー
        ''' </summary>
        ''' <remarks></remarks>
        DBTimeOut = 6

        ''' <summary>
        ''' ストールロックエラー
        ''' </summary>
        ''' <remarks></remarks>
        StallRock = 7

        ''' <summary>
        ''' 休憩・使用不可チップと重複
        ''' </summary>
        ''' <remarks></remarks>
        RestCollision = 8

        ''' <summary>
        ''' 1つのチップにR/Oと追加作業、または複数の追加作業を紐づけようとした
        ''' </summary>
        ''' <remarks></remarks>
        NestError = 9

        ''' <summary>
        ''' 基幹連携エラー
        ''' </summary>
        ''' <remarks></remarks>
        IFError = 10

        '2014/01/13 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>
        ''' 基幹連携のタイムアウトエラー
        ''' </summary>
        ''' <remarks></remarks>
        LinkTimeOutError = 11

        ''' <summary>
        ''' 基幹連携の基幹側エラー
        ''' </summary>
        ''' <remarks></remarks>
        LinkDmsError = 12

        ''' <summary>
        ''' 基幹連携のその他エラー
        ''' </summary>
        ''' <remarks></remarks>
        LinkOtherError = 13
        '2014/01/13 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
        ''' <summary>
        ''' 再開確認
        ''' </summary>
        ''' <remarks></remarks>
        ReStart = 14

        ''' <summary>
        ''' 休憩と再開確認
        ''' </summary>
        ''' <remarks></remarks>
        RestCollisionAndReStart = 15

        ''' <summary>
        ''' 既に開始チップがあった
        ''' </summary>
        ''' <remarks></remarks>
        AlreadyStart = 16

        ''' <summary>
        ''' 休憩・使用不可チップと配置時間が衝突
        ''' </summary>
        ''' <remarks></remarks>
        OverlapChip = 17

        ''' <summary>
        '''Jobの開始、終了時に、R/Oが未発行の場合
        ''' </summary>
        ''' <remarks></remarks>
        NotSetRO = 18

        ''' <summary>
        '''Jobの開始、終了、中断時に、ストールに作業者が設定されていない場合
        ''' </summary>
        ''' <remarks></remarks>
        NoTechnician = 19

        ''' <summary>
        '''Jobの開始時に、現在時間が営業終了時間を超えている場合
        ''' </summary>
        ''' <remarks></remarks>
        OutOfWorkingTime = 20

        ''' <summary>
        '''Jobの開始時に、チップの整備種類が未選択の場合
        ''' </summary>
        ''' <remarks></remarks>
        NotSetJobSvcClassId = 21

        ''' <summary>
        '''Jobの開始時に、親R/OのJobが1つも作業開始していない場合
        ''' </summary>
        ''' <remarks></remarks>
        ParentroNotStarted = 22

        ''' <summary>
        '''チップの終了時に、検査依頼中、終了不可エラー場合
        ''' </summary>
        ''' <remarks></remarks>
        InspectionStatusFinish = 23

        ''' <summary>
        '''チップの中断時に、検査依頼中、中断不可エラー場合
        ''' </summary>
        ''' <remarks></remarks>
        InspectionStatusStop = 24

        ''' <summary>
        '''Jobの開始時に、該当チップの関連チップが作業中であった場合
        ''' </summary>
        ''' <remarks></remarks>
        StartedRelationChip = 25

        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

        '2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
        ''' <summary>
        '''Jobの紐付き解除による作業終了であった場合
        ''' </summary>
        ''' <remarks></remarks>
        ChipFinishByJobUnInstruct = 26
        '2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END

        '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        ''' <summary>
        ''' ストール使用不可と重複する配置である場合のエラー (Consts.vbのChipOverlapUnavailableErrorと同じ値)
        ''' </summary>
        ''' <remarks></remarks>
        ChipOverlapUnavailableError = 34
        '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        ''' <summary>
        ''' 異常終了
        ''' </summary>
        ''' <remarks></remarks>
        Failure = 9999

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

        ''' <summary>
        ''' DMS除外エラーの警告
        ''' </summary>
        ''' <remarks></remarks>
        WarningOmitDmsError = -9000

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

    End Enum
#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' コールバックメソッドの呼び出し元に返却する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private callBackResult As String

    ''' <summary>
    ''' 予定開始日時
    ''' </summary>
    ''' <remarks></remarks>
    Private updStartPlanTime As Date

    ''' <summary>
    ''' 予定終了日時
    ''' </summary>
    ''' <remarks></remarks>
    Private updFinishPlanTime As Date

    ''' <summary>
    ''' 見込終了日時
    ''' </summary>
    ''' <remarks></remarks>
    Private updPrmsEndTime As Date

    ''' <summary>
    ''' 実績時間
    ''' </summary>
    ''' <remarks></remarks>
    Private updProcTime As Long

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private objStaffContext As StaffContext
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

#Region "ページクラス処理のバイパス処理"

    ''Private Sub RedirectNextScreen(ByVal pageID As String)
    ''    GetPageInterface().RedirectNextScreenBypass(pageID)
    ''End Sub

    ' ''' <summary>
    ' ''' セッション情報から値を取得します。
    ' ''' </summary>
    ' ''' <param name="pos"></param>
    ' ''' <param name="key"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function ContainsKey(ByVal pos As ScreenPos, ByVal key As String) As Object

    '    Return GetPageInterface().ContainsKeyBypass(pos, key)

    'End Function

    ' ''' <summary>
    ' ''' セッション情報から値を取得します。
    ' ''' </summary>
    ' ''' <param name="pos"></param>
    ' ''' <param name="key"></param>
    ' ''' <param name="removeFlg"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function GetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object

    '    Return GetPageInterface().GetValueCommonBypass(pos, key, removeFlg)

    'End Function

    ''' <summary>
    ''' セッション情報に値を設定します。
    ''' </summary>
    ''' <param name="pos"></param>
    ''' <param name="key"></param>
    ''' <param name="val"></param>
    ''' <remarks></remarks>
    Private Sub SetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal val As Object)

        GetPageInterface().SetValueCommonBypass(pos, key, val)

    End Sub

    ''' <summary>
    ''' 共通のインターフェイスを取得します。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPageInterface() As ICommonSessionControl

        Return CType(Me.Page, ICommonSessionControl)

    End Function
#End Region

#Region "イベント処理メソッド"

    ''' <summary>
    ''' 画面ロードの処理を実施
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'コールバックスクリプトの生成
        ScriptManager.RegisterStartupScript(
            Me,
            Me.GetType(),
            "gCallbackSC3240201",
            String.Format(CultureInfo.InvariantCulture,
                          "gCallbackSC3240201.beginCallback = function () {{ {0}; }};",
                          Page.ClientScript.GetCallbackEventReference(Me, "gCallbackSC3240201.packedArgument", _
                                                                      "gCallbackSC3240201.endCallback", "", True)
                          ),
            True
        )

        If Not Page.IsPostBack Then
        End If

        '共通ヘッダーエリアに固定文言設定
        SetDetailHeaderWord()

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub


    ''' <summary>
    ''' 顧客詳細画面へ遷移するためのダミーボタンClick処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub DetailCustButtonDummy_Click(sender As Object, e As System.EventArgs) Handles DetailCustButtonDummy.Click

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ''優先順位により設定値を置換　※Me.SetReplaceString(優先１, 優先２)

        ''車両登録No
        'Dim registerNo As String = Me.SetReplaceString(Me.Visit_VclRegNoHidden.Value, Me.ChipDetail_VclRegNoHidden.Value)

        ''VIN
        'Dim vin As String = Me.SetReplaceString(Me.Visit_VINHidden.Value, Me.ChipDetail_VinHidden.Value)

        ''モデルコード
        'Dim modelCode As String = Me.ChipDetail_KatashikiHidden.Value.Trim()
        'If String.IsNullOrEmpty(modelCode) Then
        '    modelCode = String.Empty
        'End If

        ''電話番号
        'Dim tel1 As String = Me.SetReplaceString(Me.Visit_TelNoHidden.Value, Me.ChipDetail_TelNoHidden.Value)

        ''携帯番号
        'Dim tel2 As String = Me.SetReplaceString(Me.Visit_MobileNoHidden.Value, Me.ChipDetail_MobileNoHidden.Value)

        ''来店者ID
        'Dim visitSeq As String = Me.Visit_VisitSeqHidden.Value.Trim()
        'If String.IsNullOrEmpty(visitSeq) Or visitSeq.Equals("-1") Then
        '    visitSeq = String.Empty
        'End If

        ''販売店コード
        'Dim dlrCode As String = Me.ChipDetail_DlrCodeHidden.Value.Trim()
        'If String.IsNullOrEmpty(dlrCode) Then
        '    dlrCode = String.Empty
        'End If

        ''予約ID(サービス入庫ID)
        'Dim serviceInID As String = Me.ChipDetail_ServiceInIDHidden.Value.Trim()
        'If String.IsNullOrEmpty(serviceInID) Then
        '    serviceInID = String.Empty
        'End If

        ''事前準備チップフラグ（1：事前準備チップ／0：事前準備チップ以外）
        'Dim prepareChipFlag As String = "1"
        'Dim assignStatus As String = Me.Visit_AssignStatusHidden.Value.Trim()
        ''「振当ステータス＝２：SA振当済み」　の場合、
        'If assignStatus.Equals("2") Then
        '    '0：事前準備チップ以外
        '    prepareChipFlag = "0"
        'End If

        ''次画面遷移パラメータ設定
        'Me.SetValue(ScreenPos.Next, "Redirect.REGISTERNO", registerNo)            '車両登録No
        'Me.SetValue(ScreenPos.Next, "Redirect.VINNO", vin)                        'VIN
        'Me.SetValue(ScreenPos.Next, "Redirect.MODELCODE", modelCode)              'モデルコード
        'Me.SetValue(ScreenPos.Next, "Redirect.TEL1", tel1)                        '電話番号
        'Me.SetValue(ScreenPos.Next, "Redirect.TEL2", tel2)                        '携帯番号
        'Me.SetValue(ScreenPos.Next, "Redirect.VISITSEQ", visitSeq)                '来店者ID
        'Me.SetValue(ScreenPos.Next, "Redirect.CRDEALERCODE", dlrCode)             'DLRコード
        'Me.SetValue(ScreenPos.Next, "Redirect.FLAG", "1")                         '固定フラグ
        'Me.SetValue(ScreenPos.Next, "Redirect.REZID", serviceInID)                '予約ID
        'Me.SetValue(ScreenPos.Next, "Redirect.PREPARECHIPFLAG", prepareChipFlag)  '事前準備フラグ

        '基幹顧客コード
        Dim dmsCstCd As String = Me.DmsCstCdHidden.Value.Trim()
        If String.IsNullOrEmpty(dmsCstCd) Then
            dmsCstCd = ""
        End If

        '優先順位により設定値を置換　※Me.SetReplaceString(優先１, 優先２)
        'VIN
        Dim vin As String = Me.SetReplaceString(Me.Visit_VINHidden.Value, Me.ChipDetail_VinHidden.Value)

        '次画面遷移パラメータ設定
        Me.SetValue(ScreenPos.Next, "SessionKey.DMS_CST_ID", dmsCstCd)            '基幹顧客コード
        Me.SetValue(ScreenPos.Next, "SessionKey.VIN", vin)                        'VIN

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} [DMS_CST_ID:{2}][VIN:{3}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dmsCstCd, vin))
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        ' 顧客詳細画面に遷移
        'Me.RedirectNextScreen(APPLICATIONID_CUSTOMEROUT)
        CType(Me.Page, BasePage).RedirectNextScreen(APPLICATIONID_CUSTOMEROUT)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' R/O参照画面へ遷移するためのダミーボタンClick処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub DetailROButtonDummy_Click(sender As Object, e As System.EventArgs) Handles DetailROButtonDummy.Click

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ''RO番号
        'Dim roNum As String = Me.ChipDetailOrderNoHidden.Value.Trim()

        '' 次画面遷移パラメータ設定
        'Me.SetValue(ScreenPos.Next, "OrderNo", roNum)   'R/O No

        '基幹販売店コード、店舗コードを取得
        Dim bizLogic As New SC3240201BusinessLogic
        Dim dmsDlrBrnRow As ServiceCommonClassDataSet.DmsCodeMapRow = Nothing

        Try
            dmsDlrBrnRow = bizLogic.GetDmsBlnCD(objStaffContext.DlrCD, objStaffContext.BrnCD, objStaffContext.Account)
        Finally
            bizLogic = Nothing
        End Try

        If IsNothing(dmsDlrBrnRow) Then
            dmsDlrBrnRow.CODE1 = ""
            dmsDlrBrnRow.CODE2 = ""
            dmsDlrBrnRow.ACCOUNT = ""
        Else
            If dmsDlrBrnRow.IsCODE1Null Then
                dmsDlrBrnRow.CODE1 = ""
            End If
            If dmsDlrBrnRow.IsCODE2Null Then
                dmsDlrBrnRow.CODE2 = ""
            End If
            If dmsDlrBrnRow.IsACCOUNTNull Then
                dmsDlrBrnRow.ACCOUNT = ""
            End If
        End If


        '基幹作業内容ID
        Dim dmsJobDtlId As String = Me.DmsJobDtlIdHidden.Value.Trim()
        If String.IsNullOrEmpty(dmsJobDtlId) Then
            dmsJobDtlId = ""
        End If

        '来店者ID
        Dim visitSeq As String = Me.Visit_VisitSeqHidden.Value.Trim()
        If String.IsNullOrEmpty(visitSeq) Or visitSeq.Equals("-1") Then
            visitSeq = ""
        End If

        'RO番号
        Dim orderNo As String = Me.ChipDetailOrderNoHidden.Value
        If String.IsNullOrEmpty(orderNo) Then
            orderNo = ""
        End If

        'VIN
        Dim vin As String = Me.SetReplaceString(Me.Visit_VINHidden.Value, Me.ChipDetail_VinHidden.Value)

        ' 次画面遷移パラメータ設定
        Me.SetValue(ScreenPos.Next, "Session.Param1", dmsDlrBrnRow.CODE1)       'ログインユーザーのDMS販売店コード
        Me.SetValue(ScreenPos.Next, "Session.Param2", dmsDlrBrnRow.CODE2)       'ログインユーザーのDMS店舗コード
        Me.SetValue(ScreenPos.Next, "Session.Param3", dmsDlrBrnRow.ACCOUNT)     'ログインユーザアカウント
        Me.SetValue(ScreenPos.Next, "Session.Param4", visitSeq)                 '来店実績連番
        Me.SetValue(ScreenPos.Next, "Session.Param5", dmsJobDtlId)              'DMS予約ID
        Me.SetValue(ScreenPos.Next, "Session.Param6", orderNo)                  'RO番号
        Me.SetValue(ScreenPos.Next, "Session.Param7", "0")                      'RO作業連番
        Me.SetValue(ScreenPos.Next, "Session.Param8", vin)                      '車両登録NOのVIN
        Me.SetValue(ScreenPos.Next, "Session.Param9", "0")                      'RO作成フラグ [0:編集]
        Me.SetValue(ScreenPos.Next, "Session.Param10", "0")                     'フォーマット [0：プレビュー]
        Me.SetValue(ScreenPos.Next, "Session.Param11", "")                      '入庫管理番号（タイでは R/O番号＋＠＋DMS店舗コード）
        Me.SetValue(ScreenPos.Next, "Session.Param12", "")                      '入庫時のディーラーコード
        Me.SetValue(ScreenPos.Next, "Session.DISP_NUM", "13")                   '画面番号(RO一覧) [1:R:O一覧]

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} [Param1:{2}][Param2:{3}][Param3:{4}][Param4:{5}][Param5:{6}][Param6:{7}][Param7:{8}][Param8:{9}][Param9:{10}][Param10:{11}][DISP_NUM:{12}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dmsDlrBrnRow.CODE1, dmsDlrBrnRow.CODE2, objStaffContext.Account, visitSeq, dmsJobDtlId, orderNo, "0", vin, "0", "0", "13"))
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        ' R/O参照画面に遷移
        'Me.RedirectNextScreen(APPLICATIONID_ORDEROUT)
        CType(Me.Page, BasePage).RedirectNextScreen(APPLICATIONID_ORDEROUT)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' コールバック用文字列を返却
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Return Me.callBackResult

    End Function

    ''' <summary>
    ''' コールバックイベント時のハンドリング
    ''' </summary>
    ''' <param name="eventArgument">クライアントから渡されるJSON形式のパラメータ</param>
    ''' <remarks></remarks>
    Public Sub RaiseCallbackEvent(eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[eventArgument:{0}]", eventArgument)

        Dim serializer As New JavaScriptSerializer

        'コールバック返却用内部クラスのインスタンスを生成
        Dim result As New CallBackResultClass

        Dim bizLogic As New SC3240201BusinessLogic

        Try
            'コールバック引数用内部クラス
            Dim argument As CallBackArgumentClass

            'JSON形式の引数を内部クラス型に変換して受け取る
            argument = serializer.Deserialize(Of CallBackArgumentClass)(eventArgument)

            If argument.Method.Equals(CBACK_DISPCREATE) Then

                OutputInfoLog(MethodBase.GetCurrentMethod.Name & Space(1) & CBACK_DISPCREATE, True)

                '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 START
                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                             "{0}.{1} ⑤SC3240201_チップ詳細表示 START", _
                             Me.GetType.ToString, _
                             System.Reflection.MethodBase.GetCurrentMethod.Name))
                '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 END

                '******************************
                '* 初期表示で画面の作成
                '******************************
                'コールバック呼び出し元に返却する文字列
                Dim resultString As String = Me.GetMyDisplayCreateData(argument)

                If IsNothing(resultString) Then
                    '他ユーザーによってチップが削除された場合
                    result.Contents = String.Empty
                    result.ResultCode = ResultCode.OtherDeleteError
                    result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 913))

                    OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][MsgNumber:{1}]", result.ResultCode, 913)

                Else
                    'クライアントへの返却用クラスに値を設定
                    result.Caller = CBACK_DISPCREATE
                    result.Contents = HttpUtility.HtmlEncode(resultString)
                    result.ResultCode = ResultCode.Success
                    result.Message = String.Empty

                    OutputInfoLog(MethodBase.GetCurrentMethod.Name & Space(1) & CBACK_DISPCREATE, False)
                End If

                '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 START
                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                             "{0}.{1} ⑤SC3240201_チップ詳細表示 END", _
                             Me.GetType.ToString, _
                             System.Reflection.MethodBase.GetCurrentMethod.Name))
                '2015/06/26 TMEJ 明瀬 TMTレスポンス調査のためのログ出力 END

            ElseIf argument.Method.Equals(CBACK_GETMERC) Then

                OutputInfoLog(MethodBase.GetCurrentMethod.Name & Space(1) & CBACK_GETMERC, True)

                '******************************
                '* 商品名の取得
                '******************************

                '商品情報取得（商品コンボボックス内にセットする値を取得）
                Dim changeMercDt As SC3240201DataSet.SC3240201MercListDataTable = bizLogic.GetChangeMercInfo(argument)

                If changeMercDt.Count <= 0 Then
                    '商品マスタの情報が存在しない場合
                    result.ResultCode = ResultCode.MercDataNothing

                    OutputInfoLog(MethodBase.GetCurrentMethod.Name & Space(1) & CBACK_GETMERC, False)
                Else
                    'データテーブルをJSON文字列に変換する
                    Dim changeMercDtJson As String
                    changeMercDtJson = bizLogic.ChipDetailDataTableToJson(changeMercDt)

                    'クライアントへの返却用クラスに値を設定
                    result.Caller = CBACK_GETMERC
                    result.Contents = String.Empty
                    result.ResultCode = ResultCode.Success
                    result.Message = String.Empty
                    'クライアントへの返却用として、JSON形式のデータをセットする
                    result.MercJson = HttpUtility.HtmlEncode(changeMercDtJson)

                    OutputInfoLog(MethodBase.GetCurrentMethod.Name & Space(1) & CBACK_GETMERC, False)
                End If

                '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
                'Else
            ElseIf argument.Method.Equals(CBACK_REGISTER) Then
                '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

                OutputInfoLog(MethodBase.GetCurrentMethod.Name & Space(1) & CBACK_REGISTER, True)

                '******************************
                '* 登録ボタンクリック時
                '******************************
                Dim updDate As Date = Nothing

                If argument.ValidateCode <> 0 Then
                    'クライアント側エラーチェックにより、エラーとなった場合
                    result.Contents = String.Empty
                    result.ResultCode = ResultCode.CheckError
                    result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, argument.ValidateCode))

                    OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][ValidateCode:{1}]", result.ResultCode, argument.ValidateCode)
                Else
                    '更新用の時間をセットする(休憩・使用不可チップ考慮前)
                    Me.SetUpdatingTime(argument)

                    '更新日時は全てこの値を使用する
                    updDate = DateTimeFunc.Now(argument.DlrCD)

                    Dim check As Integer
                    Dim resultCD As Long

                    '登録前チェックを行う（受付・追加作業エリア以外）
                    check = Me.CheckBeforeRegist(argument, updDate)

                    '登録前チェックを行う（すべてのチップ）
                    If check = 0 Then
                        check = Me.CheckBeforeRegistAll(argument)
                    End If

                    If check = 0 Then

                        '予約情報更新WebServiceをCallするかどうか判定する
                        Dim callFlag As Boolean
                        callFlag = bizLogic.IsCallUpdateReserve(argument)
                        If (callFlag) Then

                            '予約情報更新WebServiceを利用してDB更新(CalDAV連携あり)
                            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                            'resultCD = bizLogic.UpdateDataUsingWebService(argument, updDate, Me.updStartPlanTime, Me.updFinishPlanTime)
                            resultCD = bizLogic.UpdateDataUsingWebService(argument, updDate, Me.updStartPlanTime, Me.updFinishPlanTime)
                            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                        Else

                            '予約情報更新WebServiceを利用せずにDB更新
                            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                            'resultCD = bizLogic.UpdateData(argument, updDate, Me.updStartPlanTime, Me.updFinishPlanTime, Me.updPrmsEndTime, Me.updProcTime)
                            resultCD = bizLogic.UpdateData(argument, updDate, Me.updStartPlanTime, Me.updFinishPlanTime, Me.updPrmsEndTime, Me.updProcTime)
                            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                        End If

                        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START

                        'Select Case (resultCD)
                        '    Case ActionResult.Success
                        '        '正常終了時
                        '        result.ResultCode = ResultCode.Success
                        '        result.Message = String.Empty

                        '        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                        '        '終了のPush送信をやる(中断、終了Push送信可能)
                        '        bizLogic.SendNotice(CBACK_ALLFINISH, argument, False)
                        '        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                        '        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                        '        'Push処理と通知処理
                        '        Dim returnCode As Long = bizLogic.SendPushAndNoticeDisplay(argument, updDate, objStaffContext)
                        '        If returnCode < 0 Then
                        '            OutputErrLog(MethodBase.GetCurrentMethod.Name, " SendPushAndNotice Error.")
                        '        End If
                        '        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                        '        '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
                        '        ''Update操作をした後のチップ情報を取得し、返却する（更新後の再描画のため）
                        '        'Dim updatedChipInfo As String
                        '        'updatedChipInfo = Me.GetUpdatedChipInfo(argument, updDate, bizLogic)
                        '        'result.Contents = HttpUtility.HtmlEncode(updatedChipInfo)
                        '        '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

                        '        Dim dtShowDate As Date = Date.Parse(argument.ShowDate, CultureInfo.InvariantCulture)              'メイン画面で表示されている日
                        '        Dim dispChipStartDate As Date = Date.Parse(argument.DispStartTime, CultureInfo.InvariantCulture)  '更新したチップの表示開始日時
                        '        Dim dispChipEndDate As Date = Date.Parse(argument.DispEndTime, CultureInfo.InvariantCulture)      '更新したチップの表示終了日時

                        '        '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
                        '        'Update操作をした後のチップ情報を取得し、返却する（更新後の再描画のため）
                        '        Dim updatedChipInfo As String
                        '        updatedChipInfo = Me.GetUpdatedChipInfo(argument, dtShowDate, bizLogic)
                        '        result.Contents = HttpUtility.HtmlEncode(updatedChipInfo)
                        '        '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

                        '        '更新したチップの表示開始日時　＜＝　メイン画面で表示されている日　且つ、
                        '        'メイン画面で表示されている日　＜＝　更新したチップの表示終了日時　の場合
                        '        If (dispChipStartDate.Date.CompareTo(dtShowDate.Date) <= 0 AndAlso _
                        '            dtShowDate.Date.CompareTo(dispChipEndDate.Date) <= 0) Then

                        '            'メイン画面からチップは消えない
                        '            result.DelDispChip = 0
                        '        Else

                        '            'メイン画面からチップが消える
                        '            result.DelDispChip = 1
                        '        End If
                        '        result.StallUseId = argument.StallUseId

                        '    Case ActionResult.NoDataFound, ActionResult.RowLockVersionError
                        '        'チップの排他エラー発生時
                        '        result.Contents = String.Empty
                        '        result.ResultCode = ResultCode.ExclusionError
                        '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 914))

                        '        OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][resultCD:{1}]", result.ResultCode, resultCD)

                        '    Case ActionResult.LockStallError
                        '        'ストールロックエラー発生時
                        '        result.Contents = String.Empty
                        '        result.ResultCode = ResultCode.StallRock
                        '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 915))

                        '        OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][resultCD:{1}]", result.ResultCode, resultCD)

                        '    Case ActionResult.DBTimeOutError
                        '        'DBタイムアウトエラー発生時
                        '        result.Contents = String.Empty
                        '        result.ResultCode = ResultCode.DBTimeOut
                        '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 901))

                        '        OutputErrLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][resultCD:{1}]", result.ResultCode, resultCD)

                        '    Case ActionResult.DmsLinkageError
                        '        '基幹連携エラー発生時
                        '        result.Contents = String.Empty
                        '        result.ResultCode = ResultCode.IFError
                        '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 918))

                        '        OutputErrLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][resultCD:{1}]", result.ResultCode, resultCD)

                        '        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

                        '    Case ActionResult.NoTechnicianError

                        '        'チップを終了、中断時に、ストールに作業者が設定されていない場合
                        '        result.Contents = String.Empty
                        '        result.ResultCode = ResultCode.Failure
                        '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 927))

                        '    Case ActionResult.InspectionStatusFinishError

                        '        'チップを終了時に、検査依頼中、終了不可エラー場合
                        '        result.Contents = String.Empty
                        '        result.ResultCode = ResultCode.Failure
                        '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 937))

                        '    Case ActionResult.InspectionStatusStopError
                        '        'チップを中断時に、検査依頼中、中断不可エラー場合
                        '        result.Contents = String.Empty
                        '        result.ResultCode = ResultCode.Failure
                        '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 938))
                        '        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                        '        '2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 START
                        '    Case ActionResult.ChipFinishByJobUnInstructError
                        '        'Jobの紐付き解除によるチップ終了エラー
                        '        result.Contents = String.Empty
                        '        result.ResultCode = ResultCode.ChipFinishByJobUnInstruct
                        '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 940))
                        '        '2014/12/08 TMEJ 丁 DMS連携版サービスタブレット JobDispatch完成検査入力制御開発 END

                        '        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                        '    Case ActionResult.WarningOmitDmsError
                        '        'DMS除外エラーの警告が発生した場合

                        '        '結果コードに-9000(DMS除外エラーの警告)を設定
                        '        result.ResultCode = ResultCode.WarningOmitDmsError

                        '        '終了のPushを実施(中断、終了Push送信可能)
                        '        bizLogic.SendNotice(CBACK_ALLFINISH, argument, False)

                        '        '着工指示のPush処理と通知処理
                        '        Dim returnCode As Long = _
                        '            bizLogic.SendPushAndNoticeDisplay(argument, _
                        '                                              updDate, _
                        '                                              objStaffContext)

                        '        If returnCode < 0 Then
                        '            OutputErrLog(MethodBase.GetCurrentMethod.Name, _
                        '                         " SendPushAndNotice Error.")
                        '        End If

                        '        '工程管理画面で表示されている日
                        '        Dim dtShowDate As Date = Date.Parse(argument.ShowDate, _
                        '                                            CultureInfo.InvariantCulture)

                        '        '更新したチップの表示開始日時
                        '        Dim dispChipStartDate As Date = Date.Parse(argument.DispStartTime, _
                        '                                                   CultureInfo.InvariantCulture)

                        '        '更新したチップの表示終了日時
                        '        Dim dispChipEndDate As Date = Date.Parse(argument.DispEndTime, _
                        '                                                 CultureInfo.InvariantCulture)

                        '        'Update操作をした後のチップ情報を取得し、返却する
                        '        '※更新後の再描画のため
                        '        Dim updatedChipInfo As String = Me.GetUpdatedChipInfo(argument, _
                        '                                                              dtShowDate, _
                        '                                                              bizLogic)

                        '        result.Contents = HttpUtility.HtmlEncode(updatedChipInfo)

                        '        '更新したチップの表示開始日時　＜＝　メイン画面で表示されている日　且つ、
                        '        'メイン画面で表示されている日　＜＝　更新したチップの表示終了日時　の場合
                        '        If dispChipStartDate.Date.CompareTo(dtShowDate.Date) <= 0 _
                        '        AndAlso dtShowDate.Date.CompareTo(dispChipEndDate.Date) <= 0 Then

                        '            'メイン画面からチップは消えない
                        '            result.DelDispChip = 0
                        '        Else

                        '            'メイン画面からチップが消える
                        '            result.DelDispChip = 1
                        '        End If

                        '        result.StallUseId = argument.StallUseId

                        '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 941))

                        '        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                        '    Case Else
                        '        'その他のエラー発生時
                        '        result.Contents = String.Empty
                        '        result.ResultCode = ResultCode.Failure
                        '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 902))

                        '        OutputErrLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][resultCD:{1}]", result.ResultCode, resultCD)
                        'End Select

                        '登録後の処理を実施
                        Me.TreatmentAfterRegister(bizLogic, _
                                                  argument, _
                                                  result, _
                                                  resultCD, _
                                                  updDate)


                        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

                    ElseIf check = 916 Then
                        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                        Using biz As New TabletSMBCommonClassBusinessLogic
                            '休憩を自動判定しない場合
                            If Not biz.IsRestAutoJudge Then
                                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                                '休憩・使用不可チップと配置時間が衝突
                                result.Contents = String.Empty
                                result.ResultCode = ResultCode.RestCollision
                                result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, check))

                                OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][check:{1}]", result.ResultCode, check)

                                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                            End If
                        End Using
                        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                        'ElseIf check = 917 Then
                        '    '1つのチップにR/Oと追加作業、または複数の追加作業を紐づけようとした
                        '    result.Contents = String.Empty
                        '    result.ResultCode = ResultCode.NestError
                        '    result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, check))

                        '    OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][check:{1}]", result.ResultCode, check)
                        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                        '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                    ElseIf check = 948 Then
                        '使用不可チップと配置時間が衝突
                        result.Contents = String.Empty
                        result.ResultCode = ResultCode.ChipOverlapUnavailableError
                        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, check))

                        OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][check:{1}]", result.ResultCode, check)
                        '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

                    ElseIf check = 910 Then
                        '他チップと配置時間が衝突
                        result.Contents = String.Empty
                        result.ResultCode = ResultCode.CollisionError
                        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, check))

                        OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][check:{1}]", result.ResultCode, check)
                    Else
                        '登録前チェックエラー
                        result.Contents = String.Empty
                        result.ResultCode = ResultCode.CheckError
                        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, check))

                        OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][check:{1}]", result.ResultCode, check)
                    End If
                End If

                OutputInfoLog(MethodBase.GetCurrentMethod.Name & Space(1) & CBACK_REGISTER, False)

                '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
            Else
                result = Me.JobOperation(argument)
                '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

            End If

            '処理結果をコールバック返却用文字列に設定
            Me.callBackResult = serializer.Serialize(result)

        Catch ex As Exception

            result.ResultCode = ResultCode.Failure
            result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 902))

            '処理結果をコールバック返却用文字列に設定
            Me.callBackResult = serializer.Serialize(result)

            'エラーログ出力
            OutputErrExLog(MethodBase.GetCurrentMethod.Name, ex, "[MessageID:{0}]", ResultCode.Failure)

        Finally
            serializer = Nothing
            result = Nothing
            bizLogic = Nothing
        End Try

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 親子関係にあるDataviewの子を取得する(Repeaterの入れ子に必要)
    ''' </summary>
    ''' <param name="item"></param>
    ''' <param name="relName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChildView(item As Object, relName As String) As DataView

        Return CType(item, DataRowView).CreateChildView(relName)

    End Function

#End Region

#Region "Privateメソッド"

    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START

    ''' <summary>
    ''' 登録後処理
    ''' </summary>
    ''' <param name="inSC3240201biz">SC3240201BizLogicクラスインスタンス</param>
    ''' <param name="inArgumentClass">コールバック引数クラス</param>
    ''' <param name="inResultClass">コールバック結果クラス</param>
    ''' <param name="inRegisterResultCode">登録操作結果コード</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <remarks>
    ''' 登録操作後に実施する処理
    ''' 　・コールバック結果クラスに結果を設定するメソッドを呼び出す
    ''' 　・通知処理メソッドを呼び出す
    ''' </remarks>
    Private Sub TreatmentAfterRegister(ByVal inSC3240201biz As SC3240201BusinessLogic, _
                                       ByVal inArgumentClass As CallBackArgumentClass, _
                                       ByVal inResultClass As CallBackResultClass, _
                                       ByVal inRegisterResultCode As Long, _
                                       ByVal inUpdateDate As Date)

        Me.OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'クライアントに返却するCallBackResultClassに結果を設定する
        Me.SetRegisterResultToCallbackResultClass(inSC3240201biz, _
                                                  inArgumentClass, _
                                                  inResultClass, _
                                                  inRegisterResultCode, _
                                                  inUpdateDate)

        If inRegisterResultCode = ActionResult.Success _
        OrElse inRegisterResultCode = ActionResult.WarningOmitDmsError Then
            'Job操作の結果コードが下記の場合
            '　    0:成功
            '　-9000:DMS除外エラーの警告

            '終了のPushを実施(中断、終了Push送信可能)
            inSC3240201biz.SendNotice(CBACK_ALLFINISH, _
                                      inArgumentClass, _
                                      False)

            '着工指示のPush処理と通知処理
            Dim resultSendPushAndNoticeDisplay As Long = _
                inSC3240201biz.SendPushAndNoticeDisplay(inArgumentClass, _
                                                        inUpdateDate, _
                                                        objStaffContext)

            If resultSendPushAndNoticeDisplay < 0 Then

                OutputErrLog(MethodBase.GetCurrentMethod.Name, _
                             " SendPushAndNotice Error.")

            End If

        End If

        Me.OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' コールバック結果クラスに登録操作の結果を設定する
    ''' </summary>
    ''' <param name="inSC3240201biz">SC3240201BizLogicクラスインスタンス</param>
    ''' <param name="inArgumentClass">コールバック引数クラス</param>
    ''' <param name="inResultClass">コールバック結果クラス</param>
    ''' <param name="inRegisterResultCode">登録操作結果コード</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <remarks>
    ''' 登録操作後の結果コードにより、
    ''' クライアントに返却するコールバック結果クラスのプロパティに設定値を分岐する
    ''' </remarks>
    Private Sub SetRegisterResultToCallbackResultClass(ByVal inSC3240201biz As SC3240201BusinessLogic, _
                                                       ByVal inArgumentClass As CallBackArgumentClass, _
                                                       ByVal inResultClass As CallBackResultClass, _
                                                       ByVal inRegisterResultCode As Long, _
                                                       ByVal inUpdateDate As Date)

        Me.OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Select Case (inRegisterResultCode)
            Case ActionResult.Success
                '0:正常終了時

                '工程管理で表示されている日
                Dim dtShowDate As Date = Date.Parse(inArgumentClass.ShowDate, _
                                                    CultureInfo.InvariantCulture)

                '更新したチップの表示開始日時
                Dim dispChipStartDate As Date = Date.Parse(inArgumentClass.DispStartTime, _
                                                           CultureInfo.InvariantCulture)

                '更新したチップの表示終了日時
                Dim dispChipEndDate As Date = Date.Parse(inArgumentClass.DispEndTime, _
                                                         CultureInfo.InvariantCulture)

                '更新処理後のチップ情報を取得する(更新後にストールチップを再描画するため)
                Dim updatedChipInfo As String = _
                    Me.GetUpdatedChipInfo(inArgumentClass, _
                                          dtShowDate, _
                                          inSC3240201biz)


                inResultClass.ResultCode = ResultCode.Success
                inResultClass.Message = String.Empty
                inResultClass.Contents = HttpUtility.HtmlEncode(updatedChipInfo)
                inResultClass.StallUseId = inArgumentClass.StallUseId

                '更新したチップの表示開始日時　＜＝　メイン画面で表示されている日　且つ、
                'メイン画面で表示されている日　＜＝　更新したチップの表示終了日時　の場合
                If dispChipStartDate.Date.CompareTo(dtShowDate.Date) <= 0 _
                AndAlso dtShowDate.Date.CompareTo(dispChipEndDate.Date) <= 0 Then

                    'メイン画面からチップは消えない
                    inResultClass.DelDispChip = 0
                Else

                    'メイン画面からチップが消える
                    inResultClass.DelDispChip = 1
                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            Case ActionResult.WarningOmitDmsError
                '-9000:DMS除外エラーの警告が発生した場合

                '工程管理で表示されている日
                Dim dtShowDate As Date = Date.Parse(inArgumentClass.ShowDate, _
                                                    CultureInfo.InvariantCulture)

                '更新したチップの表示開始日時
                Dim dispChipStartDate As Date = Date.Parse(inArgumentClass.DispStartTime, _
                                                           CultureInfo.InvariantCulture)

                '更新したチップの表示終了日時
                Dim dispChipEndDate As Date = Date.Parse(inArgumentClass.DispEndTime, _
                                                         CultureInfo.InvariantCulture)

                '更新処理後のチップ情報を取得する(更新後にストールチップを再描画するため)
                Dim updatedChipInfo As String = _
                    Me.GetUpdatedChipInfo(inArgumentClass, _
                                          dtShowDate, _
                                          inSC3240201biz)

                inResultClass.ResultCode = ResultCode.WarningOmitDmsError
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 941))
                inResultClass.Contents = HttpUtility.HtmlEncode(updatedChipInfo)
                inResultClass.StallUseId = inArgumentClass.StallUseId

                '更新したチップの表示開始日時　＜＝　メイン画面で表示されている日　且つ、
                'メイン画面で表示されている日　＜＝　更新したチップの表示終了日時　の場合
                If dispChipStartDate.Date.CompareTo(dtShowDate.Date) <= 0 _
                AndAlso dtShowDate.Date.CompareTo(dispChipEndDate.Date) <= 0 Then

                    'メイン画面からチップは消えない
                    inResultClass.DelDispChip = 0
                Else

                    'メイン画面からチップが消える
                    inResultClass.DelDispChip = 1
                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Case Else
                '上記以外:エラーが発生した場合

                'コールバック結果クラスにエラー結果を設定する
                Me.SetErrorResultToCallbackResultClass(inResultClass, _
                                                       inRegisterResultCode, _
                                                       inArgumentClass.Method)

        End Select

        Me.OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' コールバック結果クラスにエラー結果を設定する
    ''' </summary>
    ''' <param name="inResultClass">コールバック結果クラス</param>
    ''' <param name="inErrorCode">結果コード</param>
    ''' <param name="inMethodName">メソッド名</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <remarks>
    ''' <history>
    ''' 2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
    ''' </history>
    ''' エラー結果コードにより、
    ''' クライアントに返却するコールバック結果クラスのプロパティに設定値を分岐する
    ''' </remarks>
    Private Sub SetErrorResultToCallbackResultClass(ByVal inResultClass As CallBackResultClass, _
                                                    ByVal inErrorCode As Long, _
                                                    ByVal inMethodName As String, _
                                                    Optional ByVal inStallId As String = "")

        Me.OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Select Case (inErrorCode)

            Case ActionResult.NoDataFound
                '1:データが見つからないエラー発生時

                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.OtherDeleteError
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 913))

            Case ActionResult.NotSetroNoError
                '4:Jobの開始、終了時に、R/Oが未発行の場合

                inResultClass.Caller = inMethodName
                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.NotSetRO
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 926))

            Case ActionResult.OutOfWorkingTimeError
                '5:Jobの開始時に、現在時間が営業終了時間を超えている場合

                inResultClass.Caller = inMethodName
                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.OutOfWorkingTime
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 928))

            Case ActionResult.NotSetJobSvcClassIdError
                '7:Jobの開始時に、チップの整備種類が未選択の場合

                inResultClass.Caller = inMethodName
                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.NotSetJobSvcClassId
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 930))

            Case ActionResult.HasWorkingChipInOneStallError
                '8:Jobの開始時に、該当チップがあるストールで他のチップが作業中であった場合

                inResultClass.Caller = inMethodName
                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.AlreadyStart
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 931))

            Case ActionResult.OverlapUnavailableError
                '10:休憩・使用不可チップと配置時間が衝突

                inResultClass.Caller = inMethodName
                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.OverlapChip
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 934))

            Case ActionResult.RowLockVersionError
                '12:チップの排他エラー発生時

                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.ExclusionError
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 914))

            Case ActionResult.LockStallError
                '13:ストールロックエラー発生時

                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.StallRock
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 915))

            Case ActionResult.DBTimeOutError
                '14:DBタイムアウトエラー発生時

                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.DBTimeOut
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 901))

            Case ActionResult.DmsLinkageError
                '15:基幹連携エラー発生時

                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.IFError
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 918))

            Case ActionResult.InspectionStatusFinishError
                '17:チップの終了時に、検査依頼中、終了不可エラー場合

                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.InspectionStatusFinish
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 937))

            Case ActionResult.InspectionStatusStopError
                '18:チップの中断時に、検査依頼中、中断不可エラー場合

                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.InspectionStatusStop
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 938))

            Case ActionResult.ParentroNotStartedError
                '19:Jobの開始時に、親R/OのJobが1つも作業開始していない場合

                inResultClass.Caller = inMethodName
                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.ParentroNotStarted
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 933))

            Case ActionResult.NoTechnicianError
                '20:Jobの開始、終了、中断時に、ストールに作業者が設定されていない場合

                'チップを終了、中断時に、ストールに作業者が設定されていない場合
                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.NoTechnician
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 927))

            Case ActionResult.HasStartedRelationChipError
                '28:Jobの開始時に、該当チップの関連チップが作業中であった場合

                inResultClass.Caller = inMethodName
                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.StartedRelationChip
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 932))

                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START

                Using serviceCommonBiz As New ServiceCommonClassBusinessLogic
                    Dim stallName As String = String.Empty
                    '作業中の関連チップのあるストール名称を取得する
                    If Not String.IsNullOrEmpty(inStallId) Then
                        stallName = serviceCommonBiz.GetStallNameWithRelationChip(CStr(inResultClass.StallUseId), CDec(inStallId))
                    End If

                    If Not String.IsNullOrEmpty(stallName) Then
                        inResultClass.Message = String.Format(CultureInfo.CurrentCulture, _
                                           WebWordUtility.GetWord(MY_PROGRAMID, 947), _
                                           stallName)
                    End If

                End Using

                '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END

            Case ActionResult.ChipFinishByJobUnInstructError
                '33:Jobの紐付き解除によるチップ終了エラー

                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.ChipFinishByJobUnInstruct
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 940))

                '2015/10/08 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 Start

            Case ActionResult.IC3800903ResultRangeLower To ActionResult.IC3800903ResultRangeUpper
                '8000番台:予約送信IFエラー
                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.IFError
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, inErrorCode))

                '2015/10/08 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 End

            Case Else
                'その他のエラー発生時
                inResultClass.Contents = String.Empty
                inResultClass.ResultCode = ResultCode.Failure
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 902))

                OutputErrLog(MethodBase.GetCurrentMethod.Name, _
                             "[ResultCode:{0}][inErrorCode:{1}]", _
                             inResultClass.ResultCode, _
                             inErrorCode)

        End Select

        Me.OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

    ''' <summary>
    ''' 画面を作成するために必要な情報を取得する
    ''' </summary>
    ''' <param name="argument"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetMyDisplayCreateData(ByVal argument As CallBackArgumentClass) As String

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim bizLogic As New SC3240201BusinessLogic

        Try
            'チップ詳細(小)(大)の固定文言を設定(小と大で共通の文言はクライアント側で小からコピーする)
            Me.SetDetailSWord()
            Me.SetDetailLWord()

            '初期表示用データセットを取得
            Dim InitDs As SC3240201DataSet = bizLogic.GetInitInfo(argument)

            '他ユーザーによってチップが削除された等、異常発生
            If IsNothing(InitDs) Then
                Return Nothing
            End If

            'チップ詳細(小)(大)にそれぞれ表示用データを設定
            Me.SetDetailSAndCommonDisplayData(InitDs, argument)
            Me.SetDetailLDisplayData(InitDs, argument)

            '非表示エリアの設定
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            Me.SetHideArea(InitDs, argument)
            '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            '上記で作成した画面のHTMLを返却する
            Using sw As New System.IO.StringWriter(CultureInfo.InvariantCulture)

                Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
                Me.RenderControl(writer)

                'OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[GetStringBuilder:{0}]", sw.GetStringBuilder().ToString)
                OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

                Return sw.GetStringBuilder().ToString
            End Using

        Finally
            bizLogic = Nothing
        End Try

    End Function

    ''' <summary>
    ''' チップ詳細画面の共通ヘッダーエリアの固定文言を設定する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDetailHeaderWord()

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'チップ詳細ヘッダー
        Me.DetailSHeaderLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 1))

        'キャンセルボタン
        Me.DetailCancelBtn.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 2))

        '登録ボタン
        Me.DetailRegisterBtn.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 3))

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' チップ詳細(小)画面に固定文言を設定する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDetailSWord()

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'チップ詳細(小)のチップ選択欄で未選択を選択した場合、１行表示に使用する文言
        Me.WordChipUnselectedHidden.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 43))

        'オレンジ枠内のチップステータス
        Me.DetailSDeriveredPlanWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 4))
        Me.DetailSDeriveredProspectWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 9))
        Me.DetailSTriangleLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 6))
        Me.DetailSFixUpArrow.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 7))

        '顧客車両エリア
        Me.DetailSRegNoWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 11))
        Me.DetailSVinWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 12))
        Me.DetailSVehicleWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 13))
        Me.DetailSCstNameWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 17))
        Me.DetailSMobileWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 18))
        Me.DetailSHomeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 19))
        Me.DetailSSAWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 20))

        'アイコン
        Me.DetailSIcnD.InnerText = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 14))
        '2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        'Me.DetailSIcnI.InnerText = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 15))
        Me.DetailSIcnP.InnerText = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 10001))
        Me.DetailSIcnL.InnerText = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 10002))
        '2018/06/20 NSK 可児　TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
        Me.DetailSIcnS.InnerText = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 16))

        '時間エリア
        Me.DetailSVisitTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 21))
        Me.DetailSStartTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 22))
        Me.DetailSFinishTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 23))
        Me.DetailSDeliveredTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 24))
        Me.DetailSPlanTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 25))
        Me.DetailSProcessTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 26))

        '整備種類エリア
        Me.DetailSMaintenanceTypeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 27))
        Me.DetailSMercWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 28))
        Me.DetailSWorkTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 29))

        '整備内容エリア
        Me.DetailSMaintenanceNoWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 39))
        Me.DetailSMaintenanceWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 40))
        Me.DetailSStallWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 41))
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        Me.DetailSMaintenanceNoCstApproveLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 75))    '承認待ち
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '作業時間に付加する単位の文言
        Me.WordWorkTimeUnitHidden.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 64))

        '登録時に休憩／使用不可チップと重複する場合の文言
        Me.WordDuplicateRestOrUnavailableHidden.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 916))

        'パーツエリア
        Me.DetailSPartsNoWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 44))
        Me.DetailSPartsWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 45))
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        Me.DetailSTablePartsNoCstApproveLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 75))    '承認待ち
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'ご用命エリア
        Me.DetailSOrderWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 46))

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ''故障原因エリア
        'Me.DetailSFailureWord1Label.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 47))

        ''診断結果エリア
        'Me.DetailSResultWord1Label.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 48))

        ''アドバイスエリア
        'Me.DetailSAdviceWord1Label.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 49))

        'メモ
        Me.DetailSMemoWord1Label.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 68))
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'チェックエリア
        Me.DetailSReservationCheckWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 30))
        Me.DetailSCarWashCheckWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 31))
        Me.DetailSWaitingCheckWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 32))
        Me.DetailSReservationYesWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 33))
        Me.DetailSWalkInWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 34))
        Me.DetailSCarWashYesWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 35))
        Me.DetailSCarWashNoWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 36))
        Me.DetailSWaitingInsideWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 37))
        Me.DetailSWaitingOutsideWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 38))

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        Me.DetailSCompleteExaminationCheckWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 65))     '検査有無
        Me.DetailSCompleteExaminationYesWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 66))       '有り   
        Me.DetailSCompleteExaminationNoWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 67))        '無し

        Me.DetailCstBtnErrMsgHidden.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 920))   '新規顧客登録をしてください。
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
        Me.JobStartDtlErrMsgHidden.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 929))   '本日の予約ではないため、作業開始できません。本日に移動してから再度操作してください。
        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' チップ詳細(大)画面に固定文言を設定する(チップ詳細(小)と同じ文言はクライアント側でコピーする)
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDetailLWord()

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        '顧客情報エリア
        Me.DetailLCstAddressWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 69))   '住所
        Me.DetailLIndividualOrCorporationWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 70))   '個人・法人
        Me.DetailLIndividualWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 71))    '個人
        Me.DetailLCorporationWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 72))   '法人
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '整備内容エリア
        Me.DetailLMaintenanceNoWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 39))
        Me.DetailLMaintenanceItemsWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 50))
        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
        'Me.DetailLMaintenanceDivisionWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 51))
        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'Me.DetailLMaintenanceWorkGWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 52))
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        Me.DetailLChipWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 41))

        'パーツエリア
        Me.DetailLPartsNoWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 44))
        Me.DetailLPartsItemsWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 56))
        Me.DetailLPartsDivisionWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 57))
        Me.DetailLPartsQuantityWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 58))
        Me.DetailLPartsUnitWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 59))
        Me.DetailLPartsBOWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 60))
        Me.DetailLPartsStatusWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 61))

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' チップ詳細(小)(+ チップ詳細(大)と共通)の画面にパラメータを設定する
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="arg"></param>
    ''' <remarks></remarks>
    Private Sub SetDetailSAndCommonDisplayData(ByVal ds As SC3240201DataSet, ByVal arg As CallBackArgumentClass)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'Dim drSrvRez As SC3240201DataSet.SC3240201DispChipBaseInfoRow = ds.SC3240201DispChipBaseInfo.Rows(0)
        Dim drSrvRez As SC3240201DataSet.SC3240201DispChipBaseInfoRow = DirectCast(ds.SC3240201DispChipBaseInfo.Rows(0), SC3240201DataSet.SC3240201DispChipBaseInfoRow)

        '******************************
        '* 各エリア共通
        '******************************
        'チップステータスエリア
        Me.DetailSChipStatusLabel.Text = HttpUtility.HtmlEncode(drSrvRez.STATUSWORD)                      'ステータス
        Me.DetailSDeriveredPlanTimeLabel.Text = HttpUtility.HtmlEncode(drSrvRez.DELIPLANTIME)             '納車予定時間
        Me.DetailSChangeNumberLabel.Text = HttpUtility.HtmlEncode(drSrvRez.CHGCNTWORD)                    '納車予定変更回数(文言付き)
        Me.DeliveryPlanUpdateCountHidden.Value = HttpUtility.HtmlEncode(drSrvRez.CHGCNT)                  '納車予定変更回数(回数のみ)
        Me.DetailSDeriveredProspectTimeLabel.Text = HttpUtility.HtmlEncode(drSrvRez.DELIPROSPECTSTIME)    '納車見込み時間

        '中断理由
        If ds.SC3240201InterruptionInfo.Count > 0 Then
            Me.DetailSInterruptionCauseRepeater.DataSource = ds.SC3240201InterruptionInfo
            Me.DetailSInterruptionCauseRepeater.DataBind()
        Else
            Me.DetailSInterruptionCauseRepeater.DataSource = Nothing
            Me.DetailSInterruptionCauseRepeater.DataBind()
        End If

        'チップ詳細(小)納車時刻変更履歴エリア
        Me.DetailSChangeTimeRepeater.DataSource = ds.SC3240201DeliveryTimeChangeLogInfo
        Me.DetailSChangeTimeRepeater.DataBind()

        '顧客・車両情報エリア
        Me.DetailSRegNoLabel.Text = HttpUtility.HtmlEncode(drSrvRez.REG_NUM)      '車両登録No.
        Me.DetailSVinLabel.Text = HttpUtility.HtmlEncode(drSrvRez.VCL_VIN)        'VIN
        Me.DetailSVehicleLabel.Text = HttpUtility.HtmlEncode(drSrvRez.MODEL_NAME) '車種
        Me.DetailSCstNameLabel.Text = HttpUtility.HtmlEncode(drSrvRez.CST_NAME)   '顧客名
        Me.DetailSMobileLabel.Text = HttpUtility.HtmlEncode(drSrvRez.CST_MOBILE)  'Mobile
        Me.DetailSHomeLabel.Text = HttpUtility.HtmlEncode(drSrvRez.CST_PHONE)     'Home
        Me.DetailSSALabel.Text = HttpUtility.HtmlEncode(drSrvRez.STF_NAME)        '担当SA
        Me.JDPMarkFlgHidden.Value = drSrvRez.IFLAG                                'iマーク表示判断用
        Me.SSCMarkFlgHidden.Value = drSrvRez.SFLAG                                'Sマーク表示判断用

        '時間エリア
        '来店予定日時
        If drSrvRez.IsPLAN_VISITDATENull Then
            Me.DetailSPlanVisitDateTimeSelector.Value = Nothing
            Me.DetailSPlanVisitLabel.Text = String.Empty
        Else
            Me.DetailSPlanVisitDateTimeSelector.Value = drSrvRez.PLAN_VISITDATE
            Me.DetailSPlanVisitLabel.Text = HttpUtility.HtmlEncode(drSrvRez.DISP_PLAN_VISIT)
        End If

        '納車予定日時
        If drSrvRez.IsPLAN_DELIDATENull Then
            Me.DetailSPlanDeriveredDateTimeSelector.Value = Nothing
            Me.DetailSPlanDeriveredLabel.Text = String.Empty
        Else
            Me.DetailSPlanDeriveredDateTimeSelector.Value = drSrvRez.PLAN_DELIDATE
            Me.DetailSPlanDeriveredLabel.Text = HttpUtility.HtmlEncode(drSrvRez.DISP_PLAN_DELI)
        End If

        'ラベルしかない来店実績、納車実績は属性に日時を持つ
        '来店実績日時
        Me.DetailSProcessVisitTimeLabel.Attributes(ATTR_DATETIME) = HttpUtility.HtmlEncode(drSrvRez.ATTR_RESULT_VISIT)
        Me.DetailSProcessVisitTimeLabel.Text = HttpUtility.HtmlEncode(drSrvRez.DISP_RESULT_VISIT)

        '納車実績日時
        Me.DetailSProcessDeriveredTimeLabel.Attributes(ATTR_DATETIME) = HttpUtility.HtmlEncode(drSrvRez.ATTR_RESULT_DELI)
        Me.DetailSProcessDeriveredTimeLabel.Text = HttpUtility.HtmlEncode(drSrvRez.DISP_RESULT_DELI)

        'ご用命(テキストボックス)
        '※テキストボックスでの文字列表示はエンコードが自動で行われるため、
        '　手動エンコードは不要
        Me.DetailSOrderTxt.Text = drSrvRez.ORDERMEMO

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ''故障原因
        'Me.DetailSFailureTxt.Text = HttpUtility.HtmlEncode(drSrvRez.FAILURECAUSE)

        ''診断結果
        'Me.DetailSResultTxt.Text = HttpUtility.HtmlEncode(drSrvRez.DIAGNOSTICRESULT)

        ''アドバイス
        'Me.DetailSAdviceTxt.Text = HttpUtility.HtmlEncode(drSrvRez.WORKRESULTADVICE)

        'メモ(テキストボックス)
        Me.DetailSMemoTxt.Text = drSrvRez.WORKMEMO
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'チェックエリア
        Me.RezFlgHidden.Value = drSrvRez.REZFLAG          '預マーク表示・予約有無判断用
        Me.CarWashFlgHidden.Value = drSrvRez.WASHFLAG     '洗車有無判断用
        Me.WaitingFlgHidden.Value = drSrvRez.WAITTYPE     '待ち方判断用
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        Me.CompleteExaminationFlgHidden.Value = drSrvRez.INSPECTIONFLG     '完成検査有無判断用
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '*********************************************
        '* 受付・追加作業エリア以外の共通
        '*********************************************
        If Not (arg.SubAreaId.Equals(SUBAREA_RECEPTION) Or arg.SubAreaId.Equals(SUBAREA_ADDWORK)) Then

            '作業開始予定日時
            Me.DetailSPlanStartDateTimeSelector.Value = drSrvRez.PLAN_STARTDATE
            Me.DetailSPlanStartLabel.Text = HttpUtility.HtmlEncode(drSrvRez.DISP_PLAN_START)

            '作業完了予定日時
            Me.DetailSPlanFinishDateTimeSelector.Value = drSrvRez.PLAN_ENDDATE
            Me.DetailSPlanFinishLabel.Text = HttpUtility.HtmlEncode(drSrvRez.DISP_PLAN_END)

            '作業開始実績日時
            If drSrvRez.IsRESULT_STARTDATENull Then
                Me.DetailSProcessStartDateTimeSelector.Value = Nothing
                Me.DetailSProcessStartDateTimeSelector.Attributes(ATTR_DATE) = String.Empty
                Me.DetailSProcessStartLabel.Text = String.Empty
            Else
                Me.DetailSProcessStartDateTimeSelector.Value = drSrvRez.RESULT_STARTDATE
                '初期表示時の年月日を属性で保持する
                Me.DetailSProcessStartDateTimeSelector.Attributes(ATTR_DATE) = HttpUtility.HtmlEncode(Me.DetailSProcessStartDateTimeSelector.Value)
                Me.DetailSProcessStartLabel.Text = HttpUtility.HtmlEncode(drSrvRez.DISP_RESULT_START)
            End If

            '作業完了実績日時
            If drSrvRez.IsRESULT_ENDDATENull Then
                Me.DetailSProcessFinishDateTimeSelector.Value = Nothing
                Me.DetailSProcessFinishDateTimeSelector.Attributes(ATTR_DATE) = String.Empty
                Me.DetailSProcessFinishLabel.Text = String.Empty
            Else
                Me.DetailSProcessFinishDateTimeSelector.Value = drSrvRez.RESULT_ENDDATE
                '初期表示時の年月日を属性で保持する
                Me.DetailSProcessFinishDateTimeSelector.Attributes(ATTR_DATE) = HttpUtility.HtmlEncode(Me.DetailSProcessFinishDateTimeSelector.Value)
                Me.DetailSProcessFinishLabel.Text = HttpUtility.HtmlEncode(drSrvRez.DISP_RESULT_END)
            End If

            '整備種類
            Me.DetailSMaintenanceTypeList.Items.Clear()
            Me.DetailSMaintenanceTypeList.DataSource = ds.SC3240201SvcClassList
            Me.DetailSMaintenanceTypeList.DataTextField = "SVC_CLASS_NAME"
            Me.DetailSMaintenanceTypeList.DataValueField = "SVCID_TIME"
            Me.DetailSMaintenanceTypeList.DataBind()
            Me.DetailSMaintenanceTypeList.Items.Insert(0, String.Empty)
            Me.DetailSMaintenanceTypeList.Items(0).Value = COMBO_INIT_VALUE

            Me.DetailSMaintenanceTypeLabel.Text = HttpUtility.HtmlEncode(drSrvRez.SVC_CLASS_NAME)
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'Me.DetailSMaintenanceTypeList.SelectedValue = CType(drSrvRez.SVCID_TIME, String)
            If (String.IsNullOrEmpty(drSrvRez.SVCID_TIME)) Then
                Me.DetailSMaintenanceTypeList.SelectedValue = "0"
            Else
                For i = 0 To ds.SC3240201SvcClassList.Rows.Count - 1

                    Dim selectSvcClassDr As SC3240201DataSet.SC3240201SvcClassListRow = DirectCast(ds.SC3240201SvcClassList.Rows(i), SC3240201DataSet.SC3240201SvcClassListRow)

                    If (drSrvRez.SVCID_TIME.Split(CChar(","))(0).Equals(selectSvcClassDr.SVCID_TIME.Split(CChar(","))(0))) And _
                       (drSrvRez.SVCID_TIME.Split(CChar(","))(1).Equals(selectSvcClassDr.SVCID_TIME.Split(CChar(","))(1))) Then
                        Me.DetailSMaintenanceTypeList.SelectedValue = CType(selectSvcClassDr.SVCID_TIME, String)

                        Exit For
                    End If
                Next
            End If
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            '2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 START

            '商品選択可能フラグ(0：選択不可、1：選択可能)
            Dim mercItemCanSelectedFlg As String = "1"

            '2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

            '整備名
            If ds.SC3240201MercList.Count <= 0 Then

                '2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 START

                '商品選択不可かのチェック
                '整備種類が選択されているか判定
                If Me.DetailSMaintenanceTypeList.SelectedValue <> "0" Then
                    '整備種類が選択されているかつ整備名が存在しない場合

                    '商品選択不可の為、フラグをOFF
                    mercItemCanSelectedFlg = "0"

                End If

                '2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

                Me.DetailSMercList.Enabled = False
            Else
                Me.DetailSMercList.Items.Clear()
                Me.DetailSMercList.DataSource = ds.SC3240201MercList
                Me.DetailSMercList.DataTextField = "MERC_NAME"
                Me.DetailSMercList.DataValueField = "MERCID_TIME"
                Me.DetailSMercList.DataBind()
                Me.DetailSMercList.Items.Insert(0, String.Empty)
                Me.DetailSMercList.Items(0).Value = COMBO_INIT_VALUE

                Me.DetailSMercLabel.Text = HttpUtility.HtmlEncode(drSrvRez.MERC_NAME)
                Me.DetailSMercList.SelectedValue = CType(drSrvRez.MERCID_TIME, String)

            End If

            '2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 START

            '商品選択可能フラグをHTMLにAttributes
            Me.DetailSMercList.Attributes(ATT_MERC_ITEM) = HttpUtility.HtmlEncode(mercItemCanSelectedFlg)

            '2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

            '作業時間
            Me.DetailSWorkTimeTxt.Text = drSrvRez.WORKTIME
            Me.DetailSWorkTimeLabel.Text = HttpUtility.HtmlEncode(drSrvRez.WORKTIME) & HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 64))
        Else
            '作業開始予定日時
            Me.DetailSPlanStartDateTimeSelector.Value = Nothing
            Me.DetailSPlanStartLabel.Text = String.Empty

            '作業完了予定日時
            Me.DetailSPlanFinishDateTimeSelector.Value = Nothing
            Me.DetailSPlanFinishLabel.Text = String.Empty

            '作業開始実績日時
            Me.DetailSProcessStartDateTimeSelector.Value = Nothing
            Me.DetailSProcessStartLabel.Text = String.Empty

            '作業完了実績日時
            Me.DetailSProcessFinishDateTimeSelector.Value = Nothing
            Me.DetailSProcessFinishLabel.Text = String.Empty

            '整備種類
            Me.DetailSMaintenanceTypeList.Items.Clear()
            Me.DetailSMaintenanceTypeLabel.Text = String.Empty

            '整備名
            Me.DetailSMercList.Items.Clear()
            Me.DetailSMercLabel.Text = String.Empty

            '作業時間
            Me.DetailSWorkTimeTxt.Text = String.Empty
            Me.DetailSWorkTimeLabel.Text = String.Empty
        End If

        '編集可能項目判断用
        Me.ChipDetailSvcStatusHidden.Value = drSrvRez.SVC_STATUS              'サービス入庫.サービスステータス
        Me.ChipDetailStallUseStatusHidden.Value = drSrvRez.STALL_USE_STATUS   'ストール利用.ストール利用ステータス
        Me.ChipDetailResvStatusHidden.Value = drSrvRez.RESV_STATUS            'サービス入庫.予約ステータス

        'RO番号
        Me.ChipDetailOrderNoHidden.Value = HttpUtility.HtmlEncode(drSrvRez.RO_NUM)

        '作業内容ID
        Me.MyJobDtlIdHidden.Value = CType(arg.JobDtlId, String)

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ''作業連番
        'Me.ChipDetailRoJobSeqHidden.Value = HttpUtility.HtmlEncode(drSrvRez.RO_JOB_SEQ)

        ''受付エリア以外は、顧客詳細ボタン非活性かどうかの制御を行う
        'If Not (arg.SubAreaId.Equals(SUBAREA_RECEPTION)) Then
        '
        '    '顧客区分が「1:自社客」以外は顧客詳細ボタン非活性
        '    If Not "1".Equals(drSrvRez.CUSTTYPE) Then
        '        Me.DetailSCustDetailBtn.Enabled = False
        '        Me.DetailLCustDetailBtn.Enabled = False
        '    End If
        '
        'End If

        ''RO番号がない場合はR/O参照ボタン非活性
        'If (String.IsNullOrEmpty(drSrvRez.RO_NUM.Trim())) Then
        '    Me.DetailSRORefBtn.Enabled = False
        '    Me.DetailLRORefBtn.Enabled = False
        'End If

        '顧客種別
        Me.CstTypeHidden.Value = HttpUtility.HtmlEncode(drSrvRez.CST_TYPE)

        '顧客基幹顧客コード 
        Me.DmsCstCdHidden.Value = HttpUtility.HtmlEncode(drSrvRez.DMS_CST_CD)

        '敬称
        Me.NameTitleNameHidden.Value = HttpUtility.HtmlEncode(drSrvRez.NAMETITLE_NAME)

        '配置区分
        Me.PositionTypeHidden.Value = HttpUtility.HtmlEncode(drSrvRez.POSITION_TYPE)

        '基幹作業内容ID
        Me.DmsJobDtlIdHidden.Value = HttpUtility.HtmlEncode(drSrvRez.DMS_JOB_DTL_ID)

        '清算準備完了日時
        Me.InvoiceDateTimeHidden.Value = HttpUtility.HtmlEncode(drSrvRez.INVOICE_DATETIME)

        '2014/09/27 TMEJ 張「RO番号がない場合、 R/O参照ボタンを押すと、現地画面エラーが出る」対応 START 
        ''訪問IDが無い場合はR/O参照ボタン非活性
        'If (drSrvRez.VISIT_ID.Equals(-1)) Then
        '    Me.DetailSRORefBtn.Enabled = False
        '    Me.DetailLRORefBtn.Enabled = False
        'End If

        'RO番号がない場合はR/O参照ボタン非活性
        If (String.IsNullOrEmpty(drSrvRez.RO_NUM.Trim())) Then
            Me.DetailSRORefBtn.Enabled = False
            Me.DetailLRORefBtn.Enabled = False
        End If
        '2014/09/27 TMEJ 張「RO番号がない場合、 R/O参照ボタンを押すと、現地画面エラーが出る」対応 END

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'チップ詳細(小)整備内容エリア
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim dispMainteDt As SC3240201DataSet.SC3240201DispMaintenanceListDataTable = ds.SC3240201DispMaintenanceList
        'Dim smallChipDt As SC3240201DataSet.SC3240201SmallDispChipListDataTable = ds.SC3240201SmallDispChipList

        ''チップ詳細に表示する整備が存在する場合
        'If 0 < dispMainteDt.Rows.Count Then
        '    'リピーターの入れ子処理のためにリレーションを設定する
        '    dispMainteDt.ChildRelations.Add("MaintenanceRelation", dispMainteDt.INDEXColumn, smallChipDt.INDEXColumn)

        '    'リピータのデータ連結(親テーブルで連結)
        '    Me.DetailSMaintenanceRepeater.DataSource = dispMainteDt
        '    Me.DetailSMaintenanceRepeater.DataBind()
        'End If
        Dim dispjobInstructDt As SC3240201DataSet.SC3240201JobInstructListDataTable = ds.SC3240201JobInstructList
        Dim smallChipDt As SC3240201DataSet.SC3240201SmallDispChipListDataTable = ds.SC3240201SmallDispChipList

        'チップ詳細に表示する整備が存在する場合
        If 0 < dispjobInstructDt.Rows.Count Then
            'リピーターの入れ子処理のためにリレーションを設定する
            dispjobInstructDt.ChildRelations.Add("MaintenanceRelation", dispjobInstructDt.INDEXColumn, smallChipDt.INDEXColumn)

            'リピータのデータ連結(親テーブルで連結)
            Me.DetailSMaintenanceRepeater.DataSource = dispjobInstructDt
            Me.DetailSMaintenanceRepeater.DataBind()
        End If
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'チップ詳細(小)部品エリア
        Me.DetailSPartsRepeater.DataSource = ds.SC3240201DispPartsList
        Me.DetailSPartsRepeater.DataBind()

        'その他のHidden項目
        Me.Visit_VclRegNoHidden.Value = HttpUtility.HtmlEncode(drSrvRez.VISIT_VCLREGNO)           '【サービス来店者管理】車両登録No
        Me.Visit_VINHidden.Value = HttpUtility.HtmlEncode(drSrvRez.VISIT_VIN)                     '【サービス来店者管理】VIN
        Me.Visit_TelNoHidden.Value = HttpUtility.HtmlEncode(drSrvRez.VISIT_TELNO)                 '【サービス来店者管理】電話番号
        Me.Visit_MobileNoHidden.Value = HttpUtility.HtmlEncode(drSrvRez.VISIT_MOBILE)             '【サービス来店者管理】携帯番号
        Me.Visit_VisitSeqHidden.Value = HttpUtility.HtmlEncode(drSrvRez.VISIT_VISITSEQ)           '【サービス来店者管理】来店者実績連番
        Me.Visit_AssignStatusHidden.Value = HttpUtility.HtmlEncode(drSrvRez.VISIT_ASSIGNSTATUS)   '【サービス来店者管理】振当ステータス
        Me.ChipDetail_VclRegNoHidden.Value = HttpUtility.HtmlEncode(drSrvRez.REG_NUM)             '車両登録No
        Me.ChipDetail_VinHidden.Value = HttpUtility.HtmlEncode(drSrvRez.VCL_VIN)                  'VIN
        Me.ChipDetail_KatashikiHidden.Value = HttpUtility.HtmlEncode(drSrvRez.VCL_KATASHIKI)      '車両型式
        Me.ChipDetail_TelNoHidden.Value = HttpUtility.HtmlEncode(drSrvRez.CST_PHONE)              '電話番号
        Me.ChipDetail_MobileNoHidden.Value = HttpUtility.HtmlEncode(drSrvRez.CST_MOBILE)          '携帯番号
        Me.ChipDetail_DlrCodeHidden.Value = HttpUtility.HtmlEncode(drSrvRez.DLR_CD)               '販売店コード
        Me.ChipDetail_ServiceInIDHidden.Value = HttpUtility.HtmlEncode(drSrvRez.SVCIN_ID)         'サービス入庫ID

        '2014/01/13 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
        '部品詳細情報取得APIの結果コードを取得
        Dim wsResultCode As Long = 0
        If Not (String.IsNullOrEmpty(drSrvRez.RO_NUM.Trim())) AndAlso Not (arg.SubAreaId.Equals(SUBAREA_ADDWORK)) _
           AndAlso Not (IsNothing(ds.SC3240201WebServiceResult)) AndAlso (0 < ds.SC3240201WebServiceResult.Count) Then
            'RO番号有り、且つ、追加作業でない場合
            wsResultCode = ds.SC3240201WebServiceResult.Item(0).ResultCode
        End If
        Me.PartsDtlErrMsgHidden.Value = Me.GetWsErrorMessage(wsResultCode)
        '2014/01/13 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' チップ詳細(大)の画面にパラメータを設定する(小と共通のデータはクライアント側からコピーする)
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <remarks></remarks>
    Private Sub SetDetailLDisplayData(ByVal ds As SC3240201DataSet, ByVal arg As CallBackArgumentClass)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim drSrvRez As SC3240201DataSet.SC3240201DispChipBaseInfoRow = DirectCast(ds.SC3240201DispChipBaseInfo.Rows(0), SC3240201DataSet.SC3240201DispChipBaseInfoRow)

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        '顧客情報エリア
        Me.DetailLCstAddressLabel.Text = drSrvRez.CST_ADDRESS                           '顧客住所
        Me.FleetFlgHidden.Value = drSrvRez.FLEET_FLG                                    '個人・法人判断用
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '中断理由
        If ds.SC3240201InterruptionInfo.Count > 0 Then
            Me.DetailLInterruptionCauseRepeater.DataSource = ds.SC3240201InterruptionInfo
            Me.DetailLInterruptionCauseRepeater.DataBind()
        Else
            Me.DetailLInterruptionCauseRepeater.DataSource = Nothing
            Me.DetailLInterruptionCauseRepeater.DataBind()
        End If

        'チップ詳細(大)納車時刻変更履歴エリア
        Me.DetailLChangeTimeRepeater.DataSource = ds.SC3240201DeliveryTimeChangeLogInfo
        Me.DetailLChangeTimeRepeater.DataBind()

        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ''チップ詳細(大)整備内容エリア
        'Me.DetailLMaintenanceRepeater.DataSource = ds.SC3240201DispMaintenanceList
        'Me.DetailLMaintenanceRepeater.DataBind()

        ''チップ詳細(大)スクロールチップエリア(チェック部分)
        'Dim dispMainteDt2 As SC3240201DataSet.SC3240201DispMaintenanceList2DataTable = ds.SC3240201DispMaintenanceList2
        'Dim childrenDt2 As SC3240201DataSet.SC3240201LargeDispChipList2DataTable = ds.SC3240201LargeDispChipList2

        ''チップ詳細に表示する整備が存在する場合
        'If 0 < dispMainteDt2.Rows.Count Then
        '    'リピーターの入れ子処理のためにリレーションを設定する
        '    dispMainteDt2.ChildRelations.Add("CheckRelation", dispMainteDt2.INDEXColumn, childrenDt2.INDEXColumn)

        '    Me.DetailLMaintenanceRepeater2.DataSource = dispMainteDt2
        '    Me.DetailLMaintenanceRepeater2.DataBind()
        'End If
        'チップ詳細(大)整備内容エリア
        Me.DetailLMaintenanceRepeater.DataSource = ds.SC3240201JobInstructList
        Me.DetailLMaintenanceRepeater.DataBind()

        'チップ詳細(大)スクロールチップエリア(チェック部分)
        Dim dispjobInstructDt2 As SC3240201DataSet.SC3240201JobInstructList2DataTable = ds.SC3240201JobInstructList2
        Dim childrenDt2 As SC3240201DataSet.SC3240201LargeDispChipList2DataTable = ds.SC3240201LargeDispChipList2

        'チップ詳細に表示する整備が存在する場合
        If 0 < dispjobInstructDt2.Rows.Count Then
            'リピーターの入れ子処理のためにリレーションを設定する
            dispjobInstructDt2.ChildRelations.Add("CheckRelation", dispjobInstructDt2.INDEXColumn, childrenDt2.INDEXColumn)

            Me.DetailLMaintenanceRepeater2.DataSource = dispjobInstructDt2
            Me.DetailLMaintenanceRepeater2.DataBind()
        End If
        '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        'チップ詳細(大)部品エリア
        Me.DetailLPartsRepeater.DataSource = ds.SC3240201DispPartsList
        Me.DetailLPartsRepeater.DataBind()

        '*********************************************
        '* 受付・追加作業エリア以外
        '*********************************************
        If Not (arg.SubAreaId.Equals(SUBAREA_RECEPTION) Or arg.SubAreaId.Equals(SUBAREA_ADDWORK)) Then
            '整備種類
            Me.DetailLMaintenanceTypeList.Items.Clear()
            Me.DetailLMaintenanceTypeList.DataSource = ds.SC3240201SvcClassList
            Me.DetailLMaintenanceTypeList.DataTextField = "SVC_CLASS_NAME"
            Me.DetailLMaintenanceTypeList.DataValueField = "SVCID_TIME"
            Me.DetailLMaintenanceTypeList.DataBind()
            Me.DetailLMaintenanceTypeList.Items.Insert(0, String.Empty)
            Me.DetailLMaintenanceTypeList.Items(0).Value = COMBO_INIT_VALUE

            Me.DetailLMaintenanceTypeLabel.Text = HttpUtility.HtmlEncode(drSrvRez.SVC_CLASS_NAME)
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'Me.DetailLMaintenanceTypeList.SelectedValue = CType(drSrvRez.SVCID_TIME, String)
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            If (String.IsNullOrEmpty(drSrvRez.SVCID_TIME)) Then
                Me.DetailLMaintenanceTypeList.SelectedValue = "0"
            Else
                For i = 0 To ds.SC3240201SvcClassList.Rows.Count - 1

                    Dim selectSvcClassDr As SC3240201DataSet.SC3240201SvcClassListRow = DirectCast(ds.SC3240201SvcClassList.Rows(i), SC3240201DataSet.SC3240201SvcClassListRow)

                    If (drSrvRez.SVCID_TIME.Split(CChar(","))(0).Equals(selectSvcClassDr.SVCID_TIME.Split(CChar(","))(0))) And _
                       (drSrvRez.SVCID_TIME.Split(CChar(","))(1).Equals(selectSvcClassDr.SVCID_TIME.Split(CChar(","))(1))) Then
                        Me.DetailLMaintenanceTypeList.SelectedValue = CType(selectSvcClassDr.SVCID_TIME, String)
                        Exit For
                    End If
                Next
            End If

            '整備名
            If ds.SC3240201MercList.Count <= 0 Then
                Me.DetailLMercList.Enabled = False
            Else
                Me.DetailLMercList.Items.Clear()
                Me.DetailLMercList.DataSource = ds.SC3240201MercList
                Me.DetailLMercList.DataTextField = "MERC_NAME"
                Me.DetailLMercList.DataValueField = "MERCID_TIME"
                Me.DetailLMercList.DataBind()
                Me.DetailLMercList.Items.Insert(0, String.Empty)
                Me.DetailLMercList.Items(0).Value = COMBO_INIT_VALUE

                Me.DetailLMercLabel.Text = HttpUtility.HtmlEncode(drSrvRez.MERC_NAME)
                '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                Me.DetailLMercList.SelectedValue = CType(drSrvRez.MERCID_TIME, String)
                'Me.DetailLMercList.SelectedValue = CType(drSrvRez.MERCID_TIME, String)
                '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
            End If
        End If

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 登録前チェックを行う（受付・追加作業エリア以外）
    ''' </summary>
    ''' <param name="arg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckBeforeRegist(ByVal arg As CallBackArgumentClass, _
                                       ByVal dtNow As Date) As Integer

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rtnVal As Integer = 0
        Dim bizLogic As New SC3240201BusinessLogic

        Try
            '受付・追加作業エリア以外の場合
            If Not (arg.SubAreaId.Equals(SUBAREA_RECEPTION) Or arg.SubAreaId.Equals(SUBAREA_ADDWORK)) Then

                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                ''休憩取得フラグが-1(まだ未チェック)の場合
                'If arg.RestFlg = -1 Then
                '    '休憩・使用不可チップとの重複チェックを行い、衝突ありの場合
                '    If Not bizLogic.CheckRestOrUnavailableChipCollision(arg) Then
                '        rtnVal = 916        'confirmメッセージを出力するためのコードをセット

                '        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                '        Return rtnVal
                '    End If
                'End If

                Using biz As New TabletSMBCommonClassBusinessLogic
                    '休憩を自動判定しないかつ休憩取得フラグが-1(まだ未チェック)の場合
                    If Not biz.IsRestAutoJudge AndAlso arg.RestFlg = -1 Then
                        '休憩との重複チェックを行い、衝突ありの場合
                        If Not bizLogic.CheckRestCollision(arg) Then
                            rtnVal = 916        'confirmメッセージを出力するためのコードをセット

                            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                            Return rtnVal
                        End If
                    End If

                    '休憩を自動判定する場合
                    If biz.IsRestAutoJudge() Then
                        '休憩取得フラグを「1:取得する」で処理する
                        arg.RestFlg = 1
                    End If
                End Using
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証END

                '休憩取得フラグが0(休憩を取得しない)以外　の場合
                If (arg.RestFlg <> 0) Then

                    '更新用の各種日時を算出する
                    Dim dataList As List(Of SC3240201BusinessLogic.ChipDetailDateTimeClass) = bizLogic.CalcWorkDateTime(arg)

                    '更新用の各種日時をセットする
                    If Not IsNothing(dataList) AndAlso dataList.Count > 0 Then

                        If Not IsNothing(dataList.Item(0).DetailStartPlanTime) AndAlso (dataList.Item(0).DetailStartPlanTime > Date.MinValue) Then
                            Me.updStartPlanTime = dataList.Item(0).DetailStartPlanTime
                        End If

                        If Not IsNothing(dataList.Item(0).DetailFinishPlanTime) AndAlso (dataList.Item(0).DetailFinishPlanTime > Date.MinValue) Then
                            Me.updFinishPlanTime = dataList.Item(0).DetailFinishPlanTime
                        End If

                        If Not IsNothing(dataList.Item(0).DetailprmsEndTime) AndAlso (dataList.Item(0).DetailprmsEndTime > Date.MinValue) Then
                            Me.updPrmsEndTime = dataList.Item(0).DetailprmsEndTime
                        End If
                    End If

                    '休憩取得フラグが1(休憩を取得する)、且つ、実績作業時間が入ってる場合（＝実績開始～実績終了あり）
                    If (arg.RestFlg = 1) AndAlso CLng(arg.ProcWorkTime) > 0 Then
                        '更新用の実績時間を算出する
                        Dim dataList2 As List(Of SC3240201BusinessLogic.ChipDetailDateTimeClass) = bizLogic.CalcResultWorkTime(arg)

                        '更新用の実績時間をセットする
                        If Not IsNothing(dataList2) AndAlso dataList2.Count > 0 AndAlso _
                           Not IsNothing(dataList2.Item(0).DetailprocTime) Then
                            Me.updProcTime = dataList2.Item(0).DetailprocTime
                        End If
                    End If
                End If

                'チップの配置時間衝突チェック
                If Not bizLogic.CheckChipCollision(arg, Me.updFinishPlanTime, Me.updPrmsEndTime, dtNow) Then

                    rtnVal = 910

                    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                    Return rtnVal
                End If

                '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                If bizLogic.CheckUnavailableChipCollision(arg, Me.updFinishPlanTime) Then

                    'ストール使用不可と重複している旨の文言番号を格納
                    rtnVal = 948

                    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                    Return rtnVal
                End If
                '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END


                ' ''リレーションを含めたチップの表示日時が来店予定(または実績)から納車予定の間に収まっているかチェック
                ''If Not bizLogic.CheckChipOverSrvDateTime(arg) Then

                ''    rtnVal = 904

                ''    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                ''    Return rtnVal
                ''End If

                '2016/10/14 NSK  秋田谷 問連TR-SVT-TMT-20160824-003（チップ詳細の実績時間を変更できなくする）の対応 START
                ''実績日時を未来の日時で設定しようとしていないかチェック
                'If Not bizLogic.CheckChipFutureDateTime(arg) Then
                '
                '    rtnVal = 919
                '
                '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                '    Return rtnVal
                'End If
                '
                ''作業中チップの更新時、実績開始以降に、既に実績チップが存在していないかチェック
                'If Not bizLogic.CheckProcChipCollision(arg) Then
                '
                '    rtnVal = 910
                '
                '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                '    Return rtnVal
                'End If
                '2016/10/14 NSK  秋田谷 問連TR-SVT-TMT-20160824-003（チップ詳細の実績時間を変更できなくする）の対応 END
            End If

        Finally
            bizLogic = Nothing
        End Try

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

    ''' <summary>
    ''' 登録前チェックを行う（すべてのチップ）
    ''' </summary>
    ''' <param name="arg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckBeforeRegistAll(ByVal arg As CallBackArgumentClass) As Integer

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rtnVal As Integer = 0
        Dim bizLogic As New SC3240201BusinessLogic

        Try
            '1つのチップにR/Oと追加作業、または複数の追加作業を紐づけることが出来ないため、チェックを行う
            If Not (String.IsNullOrEmpty(arg.RONum.Trim())) Then
                'RO番号がある場合

                If Not IsNothing(arg.RezIdList) AndAlso 0 < arg.RezIdList.Count Then
                    'チップエリアに表示されている予約IDリストが存在する場合

                    '作業中チップに紐付く整備があるかチェック
                    If Not bizLogic.CheckMaintenanceNumOfWorking(arg) Then
                        rtnVal = 912

                        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                        Return rtnVal
                    End If

                    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    '1つのチップにR/Oと追加作業、または複数の追加作業を紐づけようとしたかどうかチェック
                    'If Not bizLogic.CheckNestError(arg) Then
                    '    rtnVal = 917

                    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                    '    Return rtnVal
                    'End If
                    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
                End If
            End If
        Finally
            bizLogic = Nothing
        End Try

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

    ''' <summary>
    ''' 更新用の時間をセットする(休憩・使用不可チップ考慮前)
    ''' </summary>
    ''' <param name="arg"></param>
    ''' <remarks></remarks>
    Private Sub SetUpdatingTime(ByVal arg As CallBackArgumentClass)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim startPlanTime As Date = Nothing      '予定開始日時
        Dim finishPlanTime As Date = Nothing     '予定終了日時
        Dim prmsEndTime As Date = Nothing        '見込終了日時
        Dim procTime As Long = 0                 '実績時間
        Me.updStartPlanTime = Nothing            '更新用の予定開始日時
        Me.updFinishPlanTime = Nothing           '更新用の予定終了日時
        Me.updPrmsEndTime = Nothing              '更新用の見込終了日時
        Me.updProcTime = 0                       '更新用の実績時間

        '予定開始日時
        If Not String.IsNullOrEmpty(arg.StartPlanTime) Then
            startPlanTime = CDate(arg.StartPlanTime)
            Me.updStartPlanTime = startPlanTime
        End If

        '予定終了日時
        If Not String.IsNullOrEmpty(arg.FinishPlanTime) Then
            finishPlanTime = CDate(arg.FinishPlanTime)
            Me.updFinishPlanTime = finishPlanTime
        End If

        '見込終了日時
        If Not String.IsNullOrEmpty(arg.PrmsEndTime) Then
            prmsEndTime = CDate(arg.PrmsEndTime)
            Me.updPrmsEndTime = prmsEndTime
        End If

        '実績時間
        procTime = CLng(arg.ProcWorkTime)
        Me.updProcTime = procTime

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' Update操作をした後のチップ情報を取得し、返却する（更新後の再描画のため）
    ''' </summary>
    ''' <param name="arg"></param>
    ''' <param name="showDate"></param>
    ''' <param name="biz"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUpdatedChipInfo(ByVal arg As CallBackArgumentClass, _
                                        ByVal showDate As Date, _
                                        ByVal biz As SC3240201BusinessLogic) As String

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rtnVal As String

        '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
        'Update操作をした後のチップ情報を取得し、返却する（更新後の再描画のため）
        'rtnVal = biz.GetStallChipInfoFromSvcInId(arg.DlrCD, arg.StrCD, updDate, arg.SvcInId)

        rtnVal = biz.GetStallChipAfterOperation(arg.DlrCD, _
                                                arg.StrCD, _
                                                showDate, _
                                                arg.PreRefreshDateTime)
        '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function

    ''' <summary>
    ''' 非表示にするエリアを設定する
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <remarks></remarks>
    Private Sub SetHideArea(ByVal ds As SC3240201DataSet, ByVal arg As CallBackArgumentClass)

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        Dim roNum As String = CStr(ds.SC3240201DispChipBaseInfo.Rows(0)("RO_NUM"))

        'RO番号無し（RO発行前）の場合
        If (String.IsNullOrEmpty(roNum.Trim())) Then
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            '顧客承認待ちのタグを非表示にする
            Me.detailSMaintenanceNoCstApproveLi.Visible = False
            Me.detailLMaintenanceNoCstApproveLi.Visible = False
            Me.detailLMaintenanceNoCstApproveLi2.Visible = False
            Me.detailSTablePartsNoCstApproveLi.Visible = False
            Me.detailLTablePartsNoCstApproveLi.Visible = False
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

            '「整備エリア」非表示
            Me.detailSTableChipUl.Style.Add(HtmlTextWriterStyle.Display, "none")
            Me.detailLClearDiv.Style.Add(HtmlTextWriterStyle.Display, "none")

            '「部品エリア」非表示
            Me.detailSTablePartsUl.Style.Add(HtmlTextWriterStyle.Display, "none")
            Me.detailLTablePartsUl.Style.Add(HtmlTextWriterStyle.Display, "none")

            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            ''「ご用命エリア」非表示
            'Me.DetailSOrderUl.Style.Add(HtmlTextWriterStyle.Display, "none")
            'Me.DetailLOrderUl.Style.Add(HtmlTextWriterStyle.Display, "none")

            ''「故障原因」エリア非表示
            'Me.DetailSFailureUl.Style.Add(HtmlTextWriterStyle.Display, "none")
            'Me.DetailLFailureUl.Style.Add(HtmlTextWriterStyle.Display, "none")

            ''「診断結果」エリア非表示
            'Me.DetailSResultUl.Style.Add(HtmlTextWriterStyle.Display, "none")
            'Me.DetailLResultUl.Style.Add(HtmlTextWriterStyle.Display, "none")

            ''「アドバイス」エリア非表示
            'Me.DetailSAdviceUl.Style.Add(HtmlTextWriterStyle.Display, "none")
            'Me.DetailLAdviceUl.Style.Add(HtmlTextWriterStyle.Display, "none")
            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        Else

            'RO発行後であっても、整備情報がない場合（※運用上は必ず存在する）

            '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
            'If ds.SC3240201DispMaintenanceList.Rows.Count = 0 Then

            '追加作業エリア以外の場合
            If Not (arg.SubAreaId.Equals(SUBAREA_ADDWORK)) Then
                '顧客承認待ちのタグを非表示にする
                Me.detailSMaintenanceNoCstApproveLi.Visible = False
                Me.detailLMaintenanceNoCstApproveLi.Visible = False
                Me.detailLMaintenanceNoCstApproveLi2.Visible = False
                Me.detailSTablePartsNoCstApproveLi.Visible = False
                Me.detailLTablePartsNoCstApproveLi.Visible = False
                '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                If ds.SC3240201JobInstructList.Rows.Count = 0 Then

                    '「整備エリア」非表示
                    Me.detailSTableChipUl.Style.Add(HtmlTextWriterStyle.Display, "none")
                    Me.detailLClearDiv.Style.Add(HtmlTextWriterStyle.Display, "none")
                End If

                'RO発行後であっても、部品情報がない場合
                If ds.SC3240201DispPartsList.Rows.Count = 0 Then

                    '「部品エリア」非表示
                    Me.detailSTablePartsUl.Style.Add(HtmlTextWriterStyle.Display, "none")
                    Me.detailLTablePartsUl.Style.Add(HtmlTextWriterStyle.Display, "none")
                End If
            End If
        End If

        '入庫日時・納車日時必須フラグ (1:必須)の設定
        Using smbCommonBiz As New ServiceCommonClassBusinessLogic
            Dim mandatoryFlg As String = smbCommonBiz.GetSystemSettingValueBySettingName(SYS_DATETIME_MANDATORY_FLG)
            If String.IsNullOrEmpty(mandatoryFlg) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                              "{0}.Error ErrCode:Failed to get System SCHE_SVCIN_DELI_DATETIME_MANDATORY_FLG.", _
                              MethodBase.GetCurrentMethod.Name))
                'システム設定値から取得できない場合、固定値(1:必須)とする
                Me.MandatoryFlgHidden.Value = "1"
            Else
                Me.MandatoryFlgHidden.Value = mandatoryFlg
            End If


            '2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 START
            '新規予約作成時にサービスまたは商品を必須項目にするかフラグで管理

            'サービス・商品項目必須区分の取得(デフォルトは0)
            Dim mercMandatoryType As String = smbCommonBiz.GetSystemSettingValueBySettingName(SYS_MERC_MANDATORY_TYPE)

            '取得結果確認
            If String.IsNullOrEmpty(mercMandatoryType) Then
                'システム設定値から取得できない場合

                'エラーログの出力
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                              "{0}.Error ErrCode:Failed to get System SVCIN_MERC_MANDATORY_TYPE.", _
                              MethodBase.GetCurrentMethod.Name))

                '固定値(0:サービス分類・商品を必須としない)とする
                Me.MercMandatoryTypeHidden.Value = "0"

            Else
                '取得できた場合

                '取得値を設定する
                Me.MercMandatoryTypeHidden.Value = mercMandatoryType

            End If

            '2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END


        End Using

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ' ''' <summary>
    ' ''' コントロールに指定したCSSクラスを追加する
    ' ''' </summary>
    ' ''' <param name="element">コントロールオブジェクト</param>
    ' ''' <param name="cssClass">CSSクラス名</param>
    ' ''' <remarks></remarks>
    'Private Sub AddCssClass(ByVal element As HtmlGenericControl, ByVal cssClass As String)

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[element:{0}][cssClass:{1}]", element.ClientID, cssClass)

    '    If String.IsNullOrEmpty(element.Attributes("Class").Trim) Then
    '        element.Attributes("Class") = cssClass
    '    Else
    '        element.Attributes("Class") = element.Attributes("Class") & Space(1) & cssClass
    '    End If

    '    OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    'End Sub

    ''' <summary>
    ''' データ置換
    ''' </summary>
    ''' <param name="valBefore">データ元</param>
    ''' <param name="valAfter">データ先</param>
    ''' <returns>置換データ</returns>
    ''' <remarks></remarks>
    Private Function SetReplaceString(ByVal valBefore As String, ByVal valAfter As String) As String

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[valBefore:{0}][valAfter:{1}]", valBefore, valAfter)

        'データ元の値が存在する場合
        If Not String.IsNullOrEmpty(valBefore.Trim()) Then
            'データ元を返却する
            Return valBefore.Trim()
        End If

        'データ先の値が存在しない場合
        If String.IsNullOrEmpty(valAfter.Trim()) Then
            'データ元を返却する
            If String.IsNullOrEmpty(valBefore.Trim()) Then
                Return String.Empty
            Else
                Return valBefore.Trim()
            End If
        End If

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

        'データ先を返却する（データ元：値なし、データ先：値あり　のケース）
        Return valAfter.Trim()

    End Function

    '2014/01/13 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 部品詳細取得APIのエラーメッセージを取得する
    ''' </summary>
    ''' <param name="wsResultCode">部品詳細取得APIの結果コード</param>
    ''' <returns>エラーメッセージ</returns>
    ''' <remarks>エラー無しの場合はEmptyを返却</remarks>
    Private Function GetWsErrorMessage(ByVal wsResultCode As Long) As String

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[wsResultCode:{0}]", wsResultCode)

        Dim errorMessage As String

        Select Case (wsResultCode)

            Case IC3802504BusinessLogic.Result.TimeOutError
                'タイムアウトエラー
                errorMessage = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 921))

            Case IC3802504BusinessLogic.Result.DmsError
                'DMS側のエラー

                '2015/06/26 TMEJ 明瀬 （トライ店システム評価）部品情報取得エラーメッセージ仕様の見直し START

                'errorMessage = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 922))
                errorMessage = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 946))

                '2015/06/26 TMEJ 明瀬 （トライ店システム評価）部品情報取得エラーメッセージ仕様の見直し END

            Case IC3802504BusinessLogic.Result.OtherError
                'その他のエラー
                errorMessage = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 923))

                '2015/06/26 TMEJ 明瀬 （トライ店システム評価）部品情報取得エラーメッセージ仕様の見直し START

            Case IC3802504BusinessLogic.Result.XmlParseError
                'XMLの解析エラー
                errorMessage = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 944))

            Case IC3802504BusinessLogic.Result.XmlMandatoryItemsError
                'XMLタグの必須エラー
                errorMessage = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 945))

                '2015/06/26 TMEJ 明瀬 （トライ店システム評価）部品情報取得エラーメッセージ仕様の見直し END

            Case Else
                'エラー無し
                errorMessage = String.Empty

        End Select

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[errorMessage:{0}]", errorMessage)

        Return errorMessage

    End Function
    '2014/01/13 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START

    ''' <summary>
    ''' Job操作後処理
    ''' </summary>
    ''' <param name="inSC3240201biz">SC3240201BizLogicクラスインスタンス</param>
    ''' <param name="inArgumentClass">コールバック引数クラス</param>
    ''' <param name="inResultClass">コールバック結果クラス</param>
    ''' <param name="inJobOperationResultCode">Job操作結果コード</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inIsFirstStartChipFlg">最初開始チップフラグ</param>
    ''' <remarks>
    ''' Job操作後に実施する処理
    ''' 　・コールバック結果クラスに結果を設定するメソッドを呼び出す
    ''' 　・通知処理メソッドを呼び出す
    ''' </remarks>
    Private Sub TreatmentAfterJobOperation(ByVal inSC3240201biz As SC3240201BusinessLogic, _
                                           ByVal inArgumentClass As CallBackArgumentClass, _
                                           ByVal inResultClass As CallBackResultClass, _
                                           ByVal inJobOperationResultCode As Long, _
                                           ByVal inUpdateDate As Date, _
                                           ByVal inIsFirstStartChipFlg As Boolean)

        Me.OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'クライアントに返却するCallBackResultClassにJob操作の結果を設定する
        Me.SetJobOperationResultToCallbackResultClass(inSC3240201biz, _
                                                      inArgumentClass, _
                                                      inResultClass, _
                                                      inJobOperationResultCode, _
                                                      inUpdateDate)

        If inJobOperationResultCode = ActionResult.Success _
        OrElse inJobOperationResultCode = ActionResult.WarningOmitDmsError Then
            'Job操作の結果コードが下記の場合
            '　    0:成功
            '　-9000:DMS除外エラーの警告

            '通知送信を行う
            inSC3240201biz.SendNotice(inArgumentClass.Method, _
                                      inArgumentClass, _
                                      inIsFirstStartChipFlg)

        End If

        Me.OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' コールバック結果クラスに結果を設定する
    ''' </summary>
    ''' <param name="inSC3240201biz">SC3240201BizLogicクラスインスタンス</param>
    ''' <param name="inArgumentClass">コールバック引数クラス</param>
    ''' <param name="inResultClass">コールバック結果クラス</param>
    ''' <param name="inJobOperationResultCode">Job操作結果コード</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <remarks>
    ''' Job操作後の結果コードにより、
    ''' クライアントに返却するコールバック結果クラスのプロパティに設定値を分岐する
    ''' </remarks>
    Private Sub SetJobOperationResultToCallbackResultClass(ByVal inSC3240201biz As SC3240201BusinessLogic, _
                                                           ByVal inArgumentClass As CallBackArgumentClass, _
                                                           ByVal inResultClass As CallBackResultClass, _
                                                           ByVal inJobOperationResultCode As Long, _
                                                           ByVal inUpdateDate As Date)

        Me.OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'Job操作の結果コードで処理を分岐
        Select Case inJobOperationResultCode

            Case ActionResult.Success
                '0:処理成功

                'チップ詳細の最新情報を取得
                Dim resultString As String = Me.GetMyDisplayCreateData(inArgumentClass)

                'Job操作後のストールチップ情報を取得(ストールチップ再描画のため)
                Dim updatedChipInfo As String = _
                    Me.GetUpdatedChipInfo(inArgumentClass, _
                                          inUpdateDate, _
                                          inSC3240201biz)

                inResultClass.Contents = HttpUtility.HtmlEncode(resultString)
                inResultClass.Caller = inArgumentClass.Method
                inResultClass.ResultCode = ResultCode.Success
                inResultClass.StallChipInfo = HttpUtility.HtmlEncode(updatedChipInfo)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            Case ActionResult.WarningOmitDmsError
                '-9000:DMS除外エラーの警告が発生した場合

                'チップ詳細の最新情報を取得
                Dim resultString As String = Me.GetMyDisplayCreateData(inArgumentClass)

                'Job操作後のストールチップ情報を取得(ストールチップ再描画のため)
                Dim updatedChipInfo As String = _
                    Me.GetUpdatedChipInfo(inArgumentClass, _
                                          inUpdateDate, _
                                          inSC3240201biz)

                inResultClass.Contents = HttpUtility.HtmlEncode(resultString)
                inResultClass.Caller = inArgumentClass.Method
                inResultClass.ResultCode = ResultCode.WarningOmitDmsError
                inResultClass.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 941))
                inResultClass.StallChipInfo = HttpUtility.HtmlEncode(updatedChipInfo)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Case Else
                '上記以外:エラーが発生した場合

                inResultClass.StallUseId = inArgumentClass.StallUseId

                Me.SetErrorResultToCallbackResultClass(inResultClass, _
                                                       inJobOperationResultCode, _
                                                       inArgumentClass.Method, _
                                                       inArgumentClass.StallId)

        End Select

        Me.OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START

    ''' <summary>
    ''' Job操作
    ''' </summary>
    ''' <param name="argument">クライアントから渡されるJSON形式のパラメータ</param>
    ''' <returns>JSON形式の処理結果</returns>
    ''' <history>2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析)</history>
    Private Function JobOperation(ByVal argument As CallBackArgumentClass) As CallBackResultClass

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                            "{0}.S. Method={1}", _
                            System.Reflection.MethodBase.GetCurrentMethod.Name, _
                            argument.Method))

        '処理結果コード
        Dim reCode As Long
        '処理日時
        Dim updDate As Date = DateTimeFunc.Now(argument.DlrCD)

        '最初開始チップフラグ
        Dim isFirstStartchipFlg As Boolean = False

        'コールバック返却用内部クラスのインスタンスを生成
        Dim result As New CallBackResultClass

        Dim check As Integer

        Dim sc3240201blz As New SC3240201BusinessLogic

        Try
            Select Case (argument.Method)
                Case CBACK_ALLSTART, CBACK_SINGLESTART
                    '開始処理(再開始処理を含まない)
                    '最初開始チップフラグ
                    isFirstStartchipFlg = sc3240201blz.IsFirstStartChip(argument.SvcInId)

                    '開始処理
                    If IsNothing(argument.RestartJobFlg) Then
                        '再開フラグが未設定する時、チェックを行う
                        check = Me.CheckBeforeOperation(argument, sc3240201blz)
                        If check <> 0 Then
                            '休憩・使用不可チップと配置時間が衝突と中断Job再開
                            result.Caller = argument.Method
                            result.Contents = String.Empty
                            result.ResultCode = check
                            result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 936))
                            'ログ出力
                            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                "{0}.E. Method={1} ResultCode={2}", _
                                                System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                                argument.Method, _
                                                check))
                            Return result
                        Else
                            reCode = Me.CallStart(argument, True, sc3240201blz)
                        End If
                    ElseIf RESTARTJOB_NO.Equals(argument.RestartJobFlg) Then
                        '再開フラグがfalse
                        reCode = Me.CallStart(argument, False, sc3240201blz)
                    Else
                        '再開フラグがtrue
                        reCode = Me.CallStart(argument, True, sc3240201blz)
                    End If

                    If reCode = ActionResult.Success Then
                        '開始したチップの履歴情報を取得する
                        Dim stallUseIdList As New List(Of Decimal)
                        stallUseIdList.Add(argument.StallUseId)

                        Dim chipHisInfo As String = _
                                        Me.GetWorkingChipHisInfo(stallUseIdList, sc3240201blz)

                        result.WorkingChipHisInfo = HttpUtility.HtmlEncode(chipHisInfo)
                    End If

                Case CBACK_RESTART
                    '再開始
                    '処理前チェックを行う
                    If IsNothing(argument.RestartJobFlg) Then
                        check = Me.CheckBeforeOperation(argument, sc3240201blz)

                        If check <> 0 Then
                            '休憩・使用不可チップと配置時間が衝突
                            result.Caller = argument.Method
                            result.Contents = String.Empty
                            result.ResultCode = check
                            result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 936))
                            'ログ出力
                            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                "{0}.E. Method={1} ResultCode={2}", _
                                                System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                                argument.Method, _
                                                check))
                            Return result
                        End If
                    End If
                    reCode = Me.CallStart(argument, True, sc3240201blz)
                    If reCode = ActionResult.Success Then
                        '開始したチップの履歴情報を取得する
                        Dim stallUseIdList As New List(Of Decimal)
                        stallUseIdList.Add(argument.StallUseId)

                        Dim chipHisInfo As String = _
                                        Me.GetWorkingChipHisInfo(stallUseIdList, sc3240201blz)

                        result.WorkingChipHisInfo = HttpUtility.HtmlEncode(chipHisInfo)
                    End If

                Case CBACK_SINGLEFINISH, CBACK_ALLFINISH
                    '終了処理
                    If IsNothing(argument.FinishStopJobFlg) Then
                        '中断Job終了フラグが未設定する時、チェックを行う
                        check = Me.CheckBeforeOperation(argument, sc3240201blz)
                        If check <> 0 Then
                            '休憩・使用不可チップと配置時間が衝突
                            result.Caller = argument.Method
                            result.Contents = String.Empty
                            result.ResultCode = check
                            result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 935))
                            'ログ出力
                            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                "{0}.E. Method={1} ResultCode={2}", _
                                                System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                                argument.Method, _
                                                check))
                            Return result
                        End If
                    End If
                    '終了処理
                    reCode = Me.CallFinish(argument, sc3240201blz)

                Case CBACK_ALLSTOP, CBACK_SINGLESTOP
                    '中断処理
                    If argument.ChipStopFlg = "1" Then
                        '処理前チェック
                        check = Me.CheckBeforeOperation(argument, sc3240201blz)
                        If check <> 0 Then
                            '休憩・使用不可チップと配置時間が衝突
                            result.Caller = argument.Method
                            result.Contents = String.Empty
                            result.ResultCode = check
                            result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 935))
                            'ログ出力
                            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                "{0}.E. Method={1} ResultCode={2}", _
                                                System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                                argument.Method, _
                                                check))
                            Return result
                        End If
                    End If

                    reCode = Me.CallStop(argument, sc3240201blz)

                    If reCode = ActionResult.Success Then
                        '中断したチップがある行の非稼働チップを全部取得する
                        Dim stallIdList As New List(Of Decimal)
                        stallIdList.Add(CType(argument.StallId, Decimal))
                        '中断により生成された非稼動エリアチップID
                        result.NewStallIdleId = CType(sc3240201blz.NewStallIdleId, String)
                        Dim dtShowDate As Date = Date.Parse(argument.ShowDate, CultureInfo.InvariantCulture)              'メイン画面で表示されている日
                        'ストール非稼働時間情報
                        Dim dtStallIdleInfo As TabletSmbCommonClassStallIdleInfoDataTable = sc3240201blz.GetAllIdleDateInfo(stallIdList, _
                                                                                                                            dtShowDate, _
                                                                                                                            objStaffContext)
                        If Not IsNothing(dtStallIdleInfo) Then
                            result.StallIdleInfo = HttpUtility.HtmlEncode(sc3240201blz.ChipDetailDataTableToJson(dtStallIdleInfo))
                        End If
                    End If

            End Select

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) START
            'Select Case reCode
            '    Case ActionResult.Success
            '        'チップ詳細の最新情報を取得
            '        Dim resultString As String = Me.GetMyDisplayCreateData(argument)

            '        result.Contents = HttpUtility.HtmlEncode(resultString)
            '        result.Caller = argument.Method
            '        result.ResultCode = ResultCode.Success
            '        'Update操作をした後のチップ情報を取得し、返却する（更新後の再描画のため）
            '        Dim updatedChipInfo As String
            '        updatedChipInfo = Me.GetUpdatedChipInfo(argument, updDate, sc3240201blz)

            '        result.StallChipInfo = HttpUtility.HtmlEncode(updatedChipInfo)
            '        'Push
            '        sc3240201blz.SendNotice(argument.Method, argument, isFirstStartchipFlg)
            '    Case ActionResult.NoDataFound, ActionResult.RowLockVersionError
            '        'チップの排他エラー発生時
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.ExclusionError
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 914))
            '    Case ActionResult.LockStallError
            '        'ストールロックエラー発生時
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.StallRock
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 915))
            '    Case ActionResult.DBTimeOutError
            '        'DBタイムアウトエラー発生時
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.DBTimeOut
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 901))
            '    Case ActionResult.DmsLinkageError
            '        '基幹連携エラー発生時
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.IFError
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 918))
            '    Case ActionResult.OverlapUnavailableError
            '        '休憩・使用不可チップと配置時間が衝突
            '        result.Caller = argument.Method
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.OverlapChip
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 934))
            '    Case ActionResult.NotSetroNoError
            '        'Jobの開始、終了時に、R/Oが未発行の場合
            '        result.Caller = argument.Method
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.NotSetRO
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 926))
            '    Case ActionResult.NoTechnicianError
            '        'Jobの開始、終了、中断時に、ストールに作業者が設定されていない場合
            '        result.Caller = argument.Method
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.NoTechnician
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 927))
            '    Case ActionResult.OutOfWorkingTimeError
            '        'Jobの開始時に、現在時間が営業終了時間を超えている場合
            '        result.Caller = argument.Method
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.OutOfWorkingTime
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 928))
            '    Case ActionResult.NotSetJobSvcClassIdError
            '        'Jobの開始時に、チップの整備種類が未選択の場合
            '        result.Caller = argument.Method
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.NotSetJobSvcClassId
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 930))
            '    Case ActionResult.HasWorkingChipInOneStallError
            '        'Jobの開始時に、該当チップがあるストールで他のチップが作業中であった場合
            '        result.Caller = argument.Method
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.AlreadyStart
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 931))
            '    Case ActionResult.HasStartedRelationChipError
            '        'Jobの開始時に、該当チップの関連チップが作業中であった場合
            '        result.Caller = argument.Method
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.StartedRelationChip
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 932))
            '    Case ActionResult.ParentroNotStartedError
            '        'Jobの開始時に、親R/OのJobが1つも作業開始していない場合
            '        result.Caller = argument.Method
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.ParentroNotStarted
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 933))
            '    Case ActionResult.InspectionStatusFinishError
            '        'チップの終了時に、検査依頼中、終了不可エラー場合
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.InspectionStatusFinish
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 937))
            '    Case ActionResult.InspectionStatusStopError
            '        'チップの中断時に、検査依頼中、中断不可エラー場合
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.InspectionStatusStop
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 938))

            '        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            '    Case ActionResult.WarningOmitDmsError
            '        'DMS除外エラーの警告が発生した場合

            '        'チップ詳細の最新情報を取得
            '        Dim resultString As String = Me.GetMyDisplayCreateData(argument)

            '        result.Contents = HttpUtility.HtmlEncode(resultString)
            '        result.Caller = argument.Method
            '        result.ResultCode = ResultCode.WarningOmitDmsError
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 941))

            '        'Update操作をした後のチップ情報を取得し、返却する（更新後の再描画のため）
            '        Dim updatedChipInfo As String
            '        updatedChipInfo = Me.GetUpdatedChipInfo(argument, updDate, sc3240201blz)

            '        result.StallChipInfo = HttpUtility.HtmlEncode(updatedChipInfo)

            '        'Push
            '        sc3240201blz.SendNotice(argument.Method, argument, isFirstStartchipFlg)

            '        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            '    Case Else
            '        'その他のエラー発生時
            '        result.Contents = String.Empty
            '        result.ResultCode = ResultCode.Failure
            '        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, 902))
            'End Select

            'Job操作後の処理を行うメソッドを呼び出す
            Me.TreatmentAfterJobOperation(sc3240201blz, _
                                          argument, _
                                          result, _
                                          reCode, _
                                          updDate, _
                                          isFirstStartchipFlg)

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発(コード分析) END

            'ログ出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                    "{0}.E.　Method={1} resultCode={2}", _
                    System.Reflection.MethodBase.GetCurrentMethod.Name, _
                    argument.Method, _
                    result))

            Return result

        Finally
            sc3240201blz = Nothing
            result = Nothing
        End Try

    End Function

    ''' <summary>
    ''' 開始処理を呼ぶ
    ''' </summary>
    ''' <param name="argument">クライアントから渡された引数</param>
    ''' <param name="inReStartJobFlg">再開始フラグ</param>
    ''' <param name="bizLogic">ビジネスクラス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CallStart(ByVal argument As CallBackArgumentClass, _
                               ByVal inReStartJobFlg As Boolean, _
                               ByVal bizLogic As SC3240201BusinessLogic) As Long
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                            "{0}.S.　argument={1} inReStartJobFlg={2}", _
                            System.Reflection.MethodBase.GetCurrentMethod.Name, _
                            argument, _
                            inReStartJobFlg))
        '返却結果
        Dim result As Long
        '現在日時
        Dim dtNow As Date = DateTimeFunc.Now(objStaffContext.DlrCD)
        '実績日時をDate方に変換する
        Dim rsltStartDateTime As Date = Date.Parse(argument.StartProcessTime, _
                                         CultureInfo.InvariantCulture)

        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        Dim restFlg = String.Empty

        Using biz As New TabletSMBCommonClassBusinessLogic

            If biz.IsRestAutoJudge() Then
                '休憩を自動判定する場合
                restFlg = RestTimeGetFlgGetRest
            Else
                '自動判定しない場合
                restFlg = CType(argument.RestFlg, String)
            End If

        End Using
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        Try
            If CBACK_ALLSTART.Equals(argument.Method) Then
                'クライアントから渡された引数(Method)により、処理を分別する
                '全開始処理実行
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'result = bizLogic.JobAllStart(argument.StallUseId, _
                '                           rsltStartDateTime, _
                '                           CType(argument.RestFlg, String), _
                '                           dtNow, _
                '                           argument.RowLockVersion, _
                '                           inReStartJobFlg)
                result = bizLogic.JobAllStart(argument.StallUseId, _
                                           rsltStartDateTime, _
                                           restFlg, _
                                           dtNow, _
                                           argument.RowLockVersion, _
                                           inReStartJobFlg)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
            Else
                '全開始以外(SingleStart、Restart)は単独開始処理を実行
                '単独開始処理実行
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'result = bizLogic.JobSingleStart(argument.StallUseId, _
                '                           rsltStartDateTime, _
                '                           CType(argument.RestFlg, String), _
                '                           dtNow, _
                '                           argument.RowLockVersion, _
                '                           argument.JobInstructId, _
                '                           CType(argument.JobInstructSeq, Long))
                result = bizLogic.JobSingleStart(argument.StallUseId, _
                                           rsltStartDateTime, _
                                           restFlg, _
                                           dtNow, _
                                           argument.RowLockVersion, _
                                           argument.JobInstructId, _
                                           CType(argument.JobInstructSeq, Long))
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            End If

            'ログ出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                    "{0}.E.　resultCode={1}", _
                    System.Reflection.MethodBase.GetCurrentMethod.Name, _
                    result))

            '処理コードを返す
            Return result
        Finally
            bizLogic = Nothing
        End Try

    End Function

    ''' <summary>
    ''' 終了処理を呼ぶ
    ''' </summary>
    ''' <param name="argument">クライアント渡された引数</param>
    ''' <param name="bizLogic">ビジネスクラス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CallFinish(ByVal argument As CallBackArgumentClass, _
                                ByVal bizLogic As SC3240201BusinessLogic) As Long
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                            "{0}.S.", _
                            System.Reflection.MethodBase.GetCurrentMethod.Name))
        '返却結果
        Dim result As Long
        '現在日
        Dim dtNow As Date = DateTimeFunc.Now(objStaffContext.DlrCD)
        '実績日時をDate方に変換する
        Dim rsltEndDateTime As Date = Date.Parse(argument.FinishProcessTime, _
                                         CultureInfo.InvariantCulture)

        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        Dim restFlg = String.Empty

        Using biz As New TabletSMBCommonClassBusinessLogic

            If biz.IsRestAutoJudge() Then
                '休憩を自動判定する場合
                restFlg = RestTimeGetFlgGetRest
            Else
                '自動判定しない場合
                restFlg = CType(argument.RestFlg, String)
            End If

        End Using
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        Try
            If CBACK_ALLFINISH.Equals(argument.Method) Then
                'クライアントから渡された引数(Method)により、処理を分別する
                '全終了処理実行
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'result = bizLogic.JobAllFinish(argument.StallUseId, _
                '                            rsltEndDateTime, _
                '                            CType(argument.RestFlg, String), _
                '                            dtNow, _
                '                            argument.RowLockVersion)
                result = bizLogic.JobAllFinish(argument.StallUseId, _
                                            rsltEndDateTime, _
                                            restFlg, _
                                            dtNow, _
                                            argument.RowLockVersion)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            Else
                '単独終了処理実行
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'result = bizLogic.JobSingleFinish(argument.StallUseId, _
                '                                  argument.JobInstructId, _
                '                                  CType(argument.JobInstructSeq, Long), _
                '                                  rsltEndDateTime, _
                '                                  CType(argument.RestFlg, String), _
                '                                  dtNow, _
                '                                  argument.RowLockVersion)
                result = bizLogic.JobSingleFinish(argument.StallUseId, _
                                                  argument.JobInstructId, _
                                                  CType(argument.JobInstructSeq, Long), _
                                                  rsltEndDateTime, _
                                                  restFlg, _
                                                  dtNow, _
                                                  argument.RowLockVersion)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            End If

            'ログ出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                    "{0}.E.　resultCode={1}", _
                    System.Reflection.MethodBase.GetCurrentMethod.Name, _
                    result))

            '処理コードを返す
            Return result

        Finally
            bizLogic = Nothing
        End Try

    End Function

    ''' <summary>
    ''' 中断処理を呼ぶ
    ''' </summary>
    ''' <param name="argument">クライアント渡された引数</param>
    ''' <param name="bizLogic">ビジネスクラス</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CallStop(ByVal argument As CallBackArgumentClass, _
                              ByVal bizLogic As SC3240201BusinessLogic) As Long
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                            "{0}.S.", _
                            System.Reflection.MethodBase.GetCurrentMethod.Name))
        '返却結果
        Dim result As Long
        '現在日
        Dim dtNow As Date = DateTimeFunc.Now(objStaffContext.DlrCD)
        '実績日時をDate方に変換する
        Dim rsltEndDateTime As Date = Date.Parse(argument.FinishProcessTime, _
                                         CultureInfo.InvariantCulture)

        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        Dim restFlg = String.Empty

        Using biz As New TabletSMBCommonClassBusinessLogic

            If biz.IsRestAutoJudge() Then
                '休憩を自動判定する場合
                restFlg = RestTimeGetFlgGetRest
            Else
                '自動判定しない場合
                restFlg = CType(argument.RestFlg, String)
            End If

        End Using
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        Try
            If CBACK_ALLSTOP.Equals(argument.Method) Then
                'クライアントから渡された引数(Method)により、処理を分別する
                '全中断処理実行
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'result = bizLogic.JobAllStop(argument.StallUseId, _
                '                    rsltEndDateTime, _
                '                    argument.StallWaitTime, _
                '                    argument.StopMemo, _
                '                    argument.StopReasonType, _
                '                    argument.RestFlg.ToString, _
                '                    dtNow, _
                '                    argument.RowLockVersion)
                result = bizLogic.JobAllStop(argument.StallUseId, _
                                    rsltEndDateTime, _
                                    argument.StallWaitTime, _
                                    argument.StopMemo, _
                                    argument.StopReasonType, _
                                    restFlg, _
                                    dtNow, _
                                    argument.RowLockVersion)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
            Else
                '単独中断実行
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'result = bizLogic.JobSingleStop(argument.StallUseId, _
                '                argument.JobInstructId, _
                '                CType(argument.JobInstructSeq, Long), _
                '                rsltEndDateTime, _
                '                argument.StallWaitTime, _
                '                argument.StopMemo, _
                '                argument.StopReasonType, _
                '                argument.RestFlg.ToString, _
                '                dtNow, _
                '                argument.RowLockVersion)
                result = bizLogic.JobSingleStop(argument.StallUseId, _
                                argument.JobInstructId, _
                                CType(argument.JobInstructSeq, Long), _
                                rsltEndDateTime, _
                                argument.StallWaitTime, _
                                argument.StopMemo, _
                                argument.StopReasonType, _
                                restFlg, _
                                dtNow, _
                                argument.RowLockVersion)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
            End If

            'ログ出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                    "{0}.E.　resultCode={1}", _
                    System.Reflection.MethodBase.GetCurrentMethod.Name, _
                    result))

            '処理コードを返す
            Return result

        Finally
            bizLogic = Nothing
        End Try

    End Function


    ''' <summary>
    ''' Job操作前チェックを行う(中断Job存在チェック、'休憩・使用不可チップとの重複チェック)
    ''' </summary>
    ''' <param name="arg">クライアントから渡された引数</param>
    ''' <param name="bizLogic">ビジネスクラス</param>
    ''' <returns>結果コード(8:休憩・使用不可チップとの重複  14:中断Job存在する  
    ''' 15:中断Job存在するかつ休憩・使用不可チップとの重複) </returns>
    ''' <remarks></remarks>
    Private Function CheckBeforeOperation(ByVal arg As CallBackArgumentClass, _
                                          ByVal bizLogic As SC3240201BusinessLogic) As Integer

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                            "{0}.S.", _
                            System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim rtnVal As Integer = 0
        Dim blRestFlg As Boolean = True
        Dim blStopJobFlg As Boolean = False

        Try
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            Using biz As New TabletSMBCommonClassBusinessLogic
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END


                'チップ状態が変化した場合のみ、チェックする
                If arg.ChipStartFlg = "1" OrElse arg.ChipFinishFlg = "1" OrElse arg.ChipStopFlg = "1" Then
                    '休憩・使用不可チップとの重複チェックを行い、衝突ありの場合
                    'True:衝突しない/False:衝突する
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                    'blRestFlg = bizLogic.CheckRestOrUnavailableChipCollision(arg)
                    blRestFlg = bizLogic.CheckRestCollision(arg)
                    '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                End If

                'ReStartJobFlg(0：設定してない　1：中断Job再開確認ボックスにOKボタンを押す 2：キャンセルボタンを押す)
                'ReStartJobFlgが0の場合、中断Job含むチェックまだしてなかった
                '単独開始の場合はチェックしない
                If CBACK_ALLSTART.Equals(arg.Method) OrElse CBACK_ALLFINISH.Equals(arg.Method) Then
                    ' true：中断Jobが存在する、false：中断Jobが存在しない
                    blStopJobFlg = bizLogic.HasStopJob(arg.JobDtlId)
                End If

                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'If blRestFlg Then

                '    If blStopJobFlg Then
                '        '再開確認ボップアップのみ表示する
                '        rtnVal = ResultCode.ReStart
                '    Else
                '        '衝突なし、中断作業ない
                '        rtnVal = 0
                '    End If

                'Else

                '    If blStopJobFlg Then
                '        '休憩ボップアップと再開ボップアップ両方も表示する
                '        rtnVal = ResultCode.RestCollisionAndReStart
                '    Else
                '        '休憩ボップアップのみ表示する
                '        rtnVal = ResultCode.RestCollision
                '    End If

                'End If

                '休憩を自動判定しない場合
                If Not biz.IsRestAutoJudge() Then
                    '休憩に衝突しない場合
                    If blRestFlg Then
                        '中断Jobを含む場合
                        If blStopJobFlg Then
                            '再開確認ボップアップのみ表示する
                            rtnVal = ResultCode.ReStart
                        Else
                            '衝突なし、中断作業ない
                            rtnVal = 0
                        End If

                    Else
                        '中断Jobを含む場合
                        If blStopJobFlg Then
                            '休憩ボップアップと再開ボップアップ両方も表示する
                            rtnVal = ResultCode.RestCollisionAndReStart
                        Else
                            '休憩ボップアップのみ表示する
                            rtnVal = ResultCode.RestCollision
                        End If

                    End If

                Else
                    '中断Jobを含む場合
                    If blStopJobFlg Then
                        '再開確認ボップアップのみ表示する
                        rtnVal = ResultCode.ReStart
                    Else
                        '衝突なし、中断作業ない
                        rtnVal = 0
                    End If
                End If
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            End Using
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        Finally
            bizLogic = Nothing
        End Try

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                "{0}.E.　resultCode={1}", _
                System.Reflection.MethodBase.GetCurrentMethod.Name, _
                rtnVal))

        Return rtnVal

    End Function


    ''' <summary>
    ''' 作業中チップの履歴情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <param name="stallUseIdList">サービス入庫IDのリスト</param>
    ''' <param name="businessLogic">ビジネスクラス</param>
    ''' <remarks></remarks>
    Private Function GetWorkingChipHisInfo(ByVal stallUseIdList As List(Of Decimal), _
                                           ByVal businessLogic As SC3240201BusinessLogic) As String

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                            "{0}.S.", _
                            System.Reflection.MethodBase.GetCurrentMethod.Name))

        Try
            '作業中チップの履歴情報を取得
            Dim dtChipHisInfo As String = _
                    businessLogic.GetWorkingChipHisInfo(stallUseIdList)

            'ログ出力
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                    "{0}.E. ", _
                    System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dtChipHisInfo
        Finally
            businessLogic = Nothing
        End Try
    End Function

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

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
    Private Sub OutputInfoLog(ByVal method As String, ByVal isStart As Boolean, ByVal argString As String, ParamArray args() As Object)

        Dim logString As String = String.Empty

        If isStart Then
            logString = MY_PROGRAMID & ".ascx " & method & "_Start" & argString
            Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
        Else
            logString = MY_PROGRAMID & ".ascx " & method & "_End" & argString
            Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
        End If

    End Sub

    ''' <summary>
    ''' 引数のあるInfoレベルのログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="argString">フォーマット用文字列</param>
    ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ''' <remarks></remarks>
    Private Sub OutputWarnLog(ByVal method As String, ByVal argString As String, ParamArray args() As Object)

        Dim logString As String = String.Empty

        logString = MY_PROGRAMID & ".ascx " & method & argString
        Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))

    End Sub

    ''' <summary>
    ''' エラーログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="argString">フォーマット用文字列</param>
    ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ''' <remarks></remarks>
    Private Sub OutputErrLog(ByVal method As String, ByVal argString As String, ParamArray args() As Object)

        Dim logString As String = String.Empty

        logString = MY_PROGRAMID & ".ascx " & method & "_Error" & argString
        Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))

    End Sub

    ''' <summary>
    ''' エラーログを出力する ※例外オブジェクトあり
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="ex">例外オブジェクト</param>
    ''' <param name="argString">フォーマット用文字列</param>
    ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ''' <remarks></remarks>
    Private Sub OutputErrExLog(ByVal method As String, ByVal ex As Exception, ByVal argString As String, ParamArray args() As Object)

        Dim logString As String = String.Empty

        logString = MY_PROGRAMID & ".ascx " & method & "_Error" & argString
        Logger.Error(String.Format(CultureInfo.InvariantCulture, logString, args), ex)

    End Sub
#End Region

#End Region

#Region "コールバック用内部クラス"

    ''' <summary>
    ''' コールバック結果をクライアントに返すための内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackResultClass

        Public Property Caller As String
        Public Property ResultCode As Integer
        Public Property Message As String
        Public Property Contents As String
        Public Property MercJson As String
        Public Property DelDispChip As Short
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        'Public Property StallUseId As Long
        Public Property StallUseId As Decimal
        '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
        Public Property StallChipInfo As String
        Public Property NewStallIdleId As String
        Public Property WorkingChipHisInfo As String
        Public Property StallIdleInfo As String
        '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

    End Class

#End Region

End Class
