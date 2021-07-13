'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240501.ascx.vb
'─────────────────────────────────────
'機能： 新規予約作成
'補足： 
'作成： 2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'更新： 2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
'更新： 2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成
'更新： 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新： 2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新： 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
'更新： 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新： 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
'更新： 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core

Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Reflection
Imports Toyota.eCRB.SMB.ReservationManagement.BizLogic
Imports Toyota.eCRB.SMB.ReservationManagement.DataAccess
Imports Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSet
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.Common.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic

' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemSettingDlrDataSet
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess.IC3810701DataSet
' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSetTableAdapters
' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

''' <summary>
''' 新規予約作成
''' プレゼンテーションクラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3240501
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler

#Region "定数"

    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEWCHIP_PROGRAMID As String = "SC3240501"

    ''' <summary>
    ''' 顧客詳細画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_CUSTOMEROUT As String = "SC3080225"

    ''' <summary>
    ''' コールバック時に画面を作成する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_DISPCREATE_NEW As String = "CreateNewChip"

    ''' <summary>
    ''' 顧客情報リストの高さ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_LIST_HEIGHT As Long = 84

    ''' <summary>
    ''' コンボボックス初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const COMBO_INIT_VALUE As String = "0"

    ''' <summary>
    ''' コールバック時に整備名を取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_GETMERC As String = "GetMercList"

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

    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
    ''' <summary>
    ''' 検査必要フラグ（0：不要）
    ''' </summary>
    Private Const INSPECTION_NEED_FLG_NEEDLESS = "0"

    ''' <summary>
    ''' 検査必要フラグ（1：必要）
    ''' </summary>
    Private Const INSPECTION_NEED_FLG_NEED = "1"

    ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
    ''' <summary>
    ''' 洗車必要フラグ（0：不要）
    ''' </summary>
    Private Const CAR_WASH_NEED_FLG_NEEDLESS = "0"
    ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

    ''' <summary>
    ''' 洗車必要フラグ（1：必要）
    ''' </summary>
    Private Const CAR_WASH_NEED_FLG_NEED = "1"

    ''' <summary>
    ''' 予定入庫納車自動表示フラグ（デフォルト値（0：非表示））
    ''' </summary>
    Private Const SCHE_SVCIN_DELI_AUTO_DISP_FLG_DEFAULT = "0"

    ''' <summary>
    ''' 予定入庫納車自動表示フラグ（1：表示）
    ''' </summary>
    Private Const SCHE_SVCIN_DELI_AUTO_DISP_FLG_ENABLE = "1"

    ''' <summary>
    ''' 標準時間（デフォルト値（0分））
    ''' </summary>
    Private Const STD_TIME_DEFAULT = 0
    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

    ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
    ''' <summary>
    ''' サービス分類区分（EM）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SVC_CLASS_TYPE_EM As String = "1"

    ''' <summary>
    ''' サービス分類区分（PM）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SVC_CLASS_TYPE_PM As String = "2"
    ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

    ''' <summary>
    ''' 入庫日時・納車日時の必須フラグ(システム設定値)
    ''' </summary>
    Private Const SYS_DATETIME_MANDATORY_FLG = "SCHE_SVCIN_DELI_DATETIME_MANDATORY_FLG"

    ''' <summary>
    ''' サービス・商品項目必須区分(システム設定値)
    ''' </summary>
    Private Const SYS_MERC_MANDATORY_TYPE = "SVCIN_MERC_MANDATORY_TYPE"

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

        'DBTimeOut = 9

        ''' <summary>
        ''' 入力項目値チェックエラー
        ''' </summary>
        ''' <remarks></remarks>
        CheckError = 1

        ''' <summary>
        ''' 登録時のチップ衝突エラー
        ''' </summary>
        ''' <remarks></remarks>
        CollisionError = 2

        ''' <summary>
        ''' 商品データなし
        ''' </summary>
        ''' <remarks></remarks>
        MercDataNothing = 3

        ''' <summary>
        ''' 休憩・使用不可チップと重複
        ''' </summary>
        ''' <remarks></remarks>
        RestCollision = 4

        ''' <summary>
        ''' ストールロックエラー
        ''' </summary>
        ''' <remarks></remarks>
        StallRock = 5

        ''' <summary>
        ''' 基幹連携エラー
        ''' </summary>
        ''' <remarks></remarks>
        IFError = 6

        ''' <summary>
        ''' DBタイムアウトエラー
        ''' </summary>
        ''' <remarks></remarks>
        DBTimeOut = 7

        ''' <summary>
        ''' 失敗
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
    Private callBackNewChipResult As String

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
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private objStaffContext As StaffContext
#End Region


#Region "イベント処理メソッド"
    ''' <summary>
    ''' 画面ロードの処理を実施
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current

        'コールバックスクリプトの生成
        ScriptManager.RegisterStartupScript(
            Me,
            Me.GetType(),
            "gCallbackSC3240501",
            String.Format(CultureInfo.InvariantCulture,
                          "gCallbackSC3240501.beginCallback = function () {{ {0}; }};",
                          Page.ClientScript.GetCallbackEventReference(Me, "gCallbackSC3240501.packedArgument", _
                                                                      "gCallbackSC3240501.endCallback", "", False)
                          ),
            True
        )

        If Not Page.IsPostBack Then
        End If

        '共通ヘッダーエリアに固定文言設定
        SetNewChipHeaderWord()

        OutputLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' コールバック用文字列を返却
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCallbackNewChipResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Return Me.callBackNewChipResult

    End Function

    ''' <summary>
    ''' コールバックイベント時のハンドリング
    ''' </summary>
    ''' <param name="eventArgument">クライアントから渡されるJSON形式のパラメータ</param>
    ''' <remarks></remarks>
    Public Sub RaiseCallbackNewChipEvent(eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

        OutputLog(MethodBase.GetCurrentMethod.Name, True, "[eventArgument:{0}]", eventArgument)

        'Dim serializer = New JavaScriptSerializer
        Dim serializer As New JavaScriptSerializer

        'コールバック返却用内部クラスのインスタンスを生成
        Dim result As New CallBackResultClass

        Try
            'コールバック引数用内部クラス
            Dim argument As New NewChipCallBackArgumentClass

            'JSON形式の引数を内部クラス型に変換して受け取る
            argument = serializer.Deserialize(Of NewChipCallBackArgumentClass)(eventArgument)

            If argument.Method.Equals(CBACK_DISPCREATE_NEW) Then
                '******************************
                '* 初期表示で画面の作成
                '******************************
                ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
                ' 商品情報取得のコールバック用引数
                Dim argumentForGetChangeMercInfo As New NewChipCallBackArgumentClass
                ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

                'コールバック呼び出し元に返却する文字列
                ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
                ' Dim resultString As String = Me.GetMyDisplayCreateData(argument)
                Dim resultString As String = Me.GetMyDisplayCreateData(argument, argumentForGetChangeMercInfo)
                ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

                'クライアントへの返却用クラスに値を設定
                result.Caller = CBACK_DISPCREATE_NEW
                result.Contents = resultString
                result.ResultCode = ResultCode.Success
                result.Message = String.Empty

                ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
                '******************************
                '* 商品名の取得
                '******************************
                '商品情報取得（商品コンボボックス内にセットする値を取得
                If (Not (String.IsNullOrEmpty(argumentForGetChangeMercInfo.DlrCD)) AndAlso _
                    Not (String.IsNullOrEmpty(argumentForGetChangeMercInfo.StrCD)) AndAlso _
                    Not (argumentForGetChangeMercInfo.SvcClassId = 0)) Then

                    Using bl As SC3240501BusinessLogic = New SC3240501BusinessLogic()
                        Dim changeMercDt As SC3240501DataSet.SC3240501MercListDataTable = bl.GetChangeMercInfo(argumentForGetChangeMercInfo)

                        If 0 < changeMercDt.Count Then
                            'データテーブルをJSON文字列に変換する
                            Dim changeMercDtJson As String
                            changeMercDtJson = bl.NewChipDataTableToJson(changeMercDt)

                            'クライアントへの返却用クラスに値を設定
                            'クライアントへの返却用として、JSON形式のデータをセットする
                            result.NewChipJson = HttpUtility.HtmlEncode(changeMercDtJson)
                        End If
                    End Using
                End If
                ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

            ElseIf argument.Method.Equals(CBACK_GETMERC) Then
                'OutputInfoLog(MethodBase.GetCurrentMethod.Name & Space(1) & CBACK_GETMERC, True)

                '******************************
                '* 商品名の取得
                '******************************

                '商品情報取得（商品コンボボックス内にセットする値を取得
                Using bl As SC3240501BusinessLogic = New SC3240501BusinessLogic()
                    Dim changeMercDt As SC3240501DataSet.SC3240501MercListDataTable = bl.GetChangeMercInfo(argument)

                    If changeMercDt.Count <= 0 Then
                        '商品マスタの情報が存在しない場合
                        result.ResultCode = ResultCode.MercDataNothing
                        'OutputInfoLog(MethodBase.GetCurrentMethod.Name & Space(1) & CBACK_GETMERC, False)
                    Else
                        'データテーブルをJSON文字列に変換する
                        Dim changeMercDtJson As String
                        changeMercDtJson = bl.NewChipDataTableToJson(changeMercDt)

                        'クライアントへの返却用クラスに値を設定
                        result.Caller = CBACK_GETMERC
                        result.Contents = String.Empty
                        result.ResultCode = ResultCode.Success
                        result.Message = String.Empty
                        'クライアントへの返却用として、JSON形式のデータをセットする
                        result.NewChipJson = HttpUtility.HtmlEncode(changeMercDtJson)

                        'OutputInfoLog(MethodBase.GetCurrentMethod.Name & Space(1) & CBACK_GETMERC, False)
                    End If
                End Using
            Else
                '******************************
                '* 登録ボタンクリック時
                '******************************
                'チップ新規作成の入力項目値でエラーがある場合、エラーを返す
                If argument.ValidateCode <> 0 Then

                    'クライアントへの返却用クラスに値を設定
                    result.Contents = String.Empty
                    result.ResultCode = ResultCode.CheckError
                    '904：予定日時の大小関係が不正です。 906：チップの配置時間が営業時間外です。
                    result.Message = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, argument.ValidateCode)

                    OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][ValidateCode:{1}]", result.ResultCode, argument.ValidateCode)

                Else
                    '更新用の時間をセットする(休憩・使用不可チップ考慮前)
                    Me.SetUpdatingTime(argument)

                    Dim dtNow As Date = DateTimeFunc.Now(objStaffContext.DlrCD)

                    'エラーがない場合は、登録前チェックを行う
                    Dim check As Integer = Me.CheckBeforeRegistNewChip(argument, dtNow)

                    '登録前チェックもエラーがなかった場合、新規登録処理を行う
                    If check = 0 Then

                        Dim retInsertedRezId As Long = -1
                        Dim newChipDataJson As String
                        Dim svcInID As Decimal

                        '新規登録（Insert）処理
                        Using bl As SC3240501BusinessLogic = New SC3240501BusinessLogic()
                            retInsertedRezId = bl.InsertDataUsingWebService(argument, objStaffContext, svcInID, Me.updStartPlanTime, Me.updFinishPlanTime, dtNow)
                            Select Case (retInsertedRezId)
                                Case ActionResult.Success
                                    '正常終了時
                                    'Update操作をした後のチップ情報を取得し、返却する（更新後の再描画のため）
                                    newChipDataJson = bl.GetStallChipInfoFromSvcInId(argument.DlrCD, argument.StrCD, dtNow, svcInID)

                                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                , "{0}.{1} GetNewChipDataFromServer NewChipData:{2}" _
                                                , Me.GetType.ToString _
                                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                , newChipDataJson))

                                    'クライアントへの返却用として、JSON形式のデータをセットする
                                    result.NewChipJson = newChipDataJson

                                    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
                                    'result.Contents = String.Empty

                                    'メイン画面で表示されている日
                                    Dim dtShowDate As Date = Date.Parse(argument.ShowDate, CultureInfo.InvariantCulture)

                                    result.Contents = bl.GetStallChipAfterOperation(argument.DlrCD, argument.StrCD, dtShowDate, argument.PreRefreshDateTime)
                                    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

                                    result.ResultCode = ResultCode.Success
                                    result.Message = String.Empty
                                Case ActionResult.LockStallError
                                    'ストールロックエラー発生時
                                    result.NewChipJson = String.Empty
                                    result.Contents = String.Empty
                                    result.ResultCode = ResultCode.StallRock
                                    result.Message = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 907)

                                    OutputErrLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][retInsertedRezId:{1}]", result.ResultCode, retInsertedRezId)

                                Case ActionResult.DBTimeOutError
                                    'DBタイムアウトエラー発生時
                                    result.NewChipJson = String.Empty
                                    result.Contents = String.Empty
                                    result.ResultCode = ResultCode.DBTimeOut
                                    result.Message = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 901)

                                    OutputErrLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][retInsertedRezId:{1}]", result.ResultCode, retInsertedRezId)
                                Case ActionResult.DmsLinkageError
                                    '基幹連携エラー発生時
                                    result.NewChipJson = String.Empty
                                    result.Contents = String.Empty
                                    result.ResultCode = ResultCode.IFError
                                    result.Message = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 909)

                                    OutputErrLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][retInsertedRezId:{1}]", result.ResultCode, retInsertedRezId)

                                    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                                Case ActionResult.WarningOmitDmsError
                                    'DMS除外エラーの警告発生時

                                    'Update操作をした後のチップ情報を取得し、返却する（更新後の再描画のため）
                                    newChipDataJson = _
                                        bl.GetStallChipInfoFromSvcInId(argument.DlrCD, _
                                                                       argument.StrCD, _
                                                                       dtNow, _
                                                                       svcInID)

                                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                                , "{0}.{1} GetNewChipDataFromServer NewChipData:{2}" _
                                                , Me.GetType.ToString _
                                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                                , newChipDataJson))

                                    'クライアントへの返却用として、JSON形式のデータをセットする
                                    result.NewChipJson = newChipDataJson

                                    'メイン画面で表示されている日
                                    Dim dtShowDate As Date = Date.Parse(argument.ShowDate, _
                                                                        CultureInfo.InvariantCulture)

                                    result.Contents = _
                                        bl.GetStallChipAfterOperation(argument.DlrCD, _
                                                                      argument.StrCD, _
                                                                      dtShowDate, _
                                                                      argument.PreRefreshDateTime)

                                    'DMSエラー除外の警告(-9000)をResultCodeに設定
                                    result.ResultCode = ResultCode.WarningOmitDmsError

                                    'DMSエラー除外の警告メッセージをMessageに設定
                                    result.Message = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 913)

                                    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                                    '2015/10/08 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 Start

                                Case ActionResult.IC3800903ResultRangeLower To ActionResult.IC3800903ResultRangeUpper
                                    '予約送信IFエラー(エラーコードが8000番台)
                                    result.NewChipJson = String.Empty
                                    result.Contents = String.Empty
                                    result.ResultCode = ResultCode.IFError
                                    result.Message = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, retInsertedRezId)

                                    '2015/10/08 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 End

                                Case Else
                                    'その他のエラー発生時
                                    result.NewChipJson = String.Empty
                                    result.Contents = String.Empty
                                    result.ResultCode = ResultCode.Failure
                                    result.Message = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 902)

                                    OutputErrLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][retInsertedRezId:{1}]", result.ResultCode, retInsertedRezId)
                            End Select
                        End Using

                    ElseIf check = 908 Then
                        '休憩・使用不可チップと配置時間が衝突
                        result.Contents = String.Empty
                        result.ResultCode = ResultCode.RestCollision

                        OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][check:{1}]", result.ResultCode, check)

                        '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                    ElseIf check = 916 Then
                        '使用不可チップと配置時間が衝突
                        result.Contents = String.Empty
                        result.ResultCode = ActionResult.ChipOverlapUnavailableError
                        '「ストールは使用不可。使用不可チップを移動するか、作業チップを別ストールに配置するようにしてください。」
                        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, check))

                        OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][check:{1}]", result.ResultCode, check)

                        '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

                    ElseIf check = 905 Then
                        '他チップと配置時間が衝突
                        result.Contents = String.Empty
                        result.ResultCode = ResultCode.CollisionError
                        '「他のチップと配置時間が重複します。」
                        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, check))

                        OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][check:{1}]", result.ResultCode, check)
                    Else
                        '登録前チェックエラー
                        result.Contents = String.Empty
                        result.ResultCode = ResultCode.CheckError
                        result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 902))

                        OutputWarnLog(MethodBase.GetCurrentMethod.Name, "[ResultCode:{0}][check:{1}]", result.ResultCode, check)
                    End If
                End If

            End If

            '処理結果をコールバック返却用文字列に設定
            Me.callBackNewChipResult = serializer.Serialize(result)

        Catch ex As Exception

            result.ResultCode = ResultCode.Failure
            result.Message = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 902)

            '処理結果をコールバック返却用文字列に設定
            Me.callBackNewChipResult = serializer.Serialize(result)

            'エラーログ出力
            OutputErrLog2(MethodBase.GetCurrentMethod.Name, ex, "[MessageID:{0}]", ResultCode.Failure)

        Finally
            serializer = Nothing
            result = Nothing
        End Try

        OutputLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 顧客検索画面のダミーボタンクリック処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' 顧客検索画面の虫眼鏡アイコン押下、もしくは検索条件入力エリアでEnterキー（Searchキー）、次のN件、前のN件を押下した際に
    ''' 隠しボタンである当ボタンがクライアント側でクリックされることでイベントが発生します。
    ''' </remarks>
    Protected Sub SearchCustomerButton_Click(sender As Object,
                                             e As System.EventArgs) Handles SearchCustomerDummyButton.Click
        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        Dim staffInfo As StaffContext = StaffContext.Current

        ' 自社客検索処理
        Me.GetCustomerInfomation(staffInfo.DlrCD, staffInfo.BrnCD)

        'Me.SearchDataUpdate.Update()

        'タイマークリア、くるくる非表示、スクロール初期化
        Dim scriptFunc As New StringBuilder
        scriptFunc.Append(" <script>")
        scriptFunc.Append("  $(function() { ")
        scriptFunc.Append("      commonClearTimer(); ")
        scriptFunc.Append("      $('#SearchDataLoading').css('display', 'none'); ")
        scriptFunc.Append("      $('.NextSearchingImage').css('display', 'none'); ")
        scriptFunc.Append("      $('.NextListSearching').css('display', 'none'); ")
        scriptFunc.Append("      $('.FrontSearchingImage').css('display', 'none'); ")
        scriptFunc.Append("      $('.FrontListSearching').css('display', 'none'); ")
        scriptFunc.Append("      $('.SearchDataBox').fingerScroll(); ")
        scriptFunc.Append("      $('.SearchDataBox .scroll-inner').css('transform', 'translate3d(0px, -' + $('#ScrollPositionHidden').val() + 'px, 0px)'); ")
        scriptFunc.Append("      $('#SearchListBox .Ellipsis').CustomLabel({ useEllipsis: true }); ")
        scriptFunc.Append("  });")
        scriptFunc.Append(" </script>")
        JavaScriptUtility.RegisterStartupScript(Me.Page, scriptFunc.ToString(), "after")

        Me.SearchDataUpdate.Update()

        OutputLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub
#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 画面を作成するために必要な情報を取得する
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
    ''' </history>
    Private Function GetMyDisplayCreateData(ByVal argument As NewChipCallBackArgumentClass, _
                                            ByRef argumentForGetChangeMercInfo As NewChipCallBackArgumentClass) As String

        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        'チップ新規作成・顧客検索画面の固定文言を設定
        Me.SetNewChipWord()

        '顧客検索画面の検索結果エリアを初期化
        Me.InitSearchResultArea()

        'チップ新規作成に初期表示用データを設定
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
        ' Me.SetNewChipDisplayData(argument)
        Me.SetNewChipDisplayData(argument, argumentForGetChangeMercInfo)
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

        '上記で作成した画面のHTMLを返却する
        Using sw As New System.IO.StringWriter(CultureInfo.InvariantCulture)

            Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
            Me.RenderControl(writer)

            'OutputLog(MethodBase.GetCurrentMethod.Name, False, "[GetStringBuilder:{0}]", sw.GetStringBuilder().ToString)
            OutputLog(MethodBase.GetCurrentMethod.Name, False)

            Return sw.GetStringBuilder().ToString
        End Using

    End Function

    ''' <summary>
    ''' チップ新規作成画面、顧客検索画面の共通ヘッダーエリアの固定文言を設定する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetNewChipHeaderWord()

        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        'ヘッダーラベル
        Me.NewChipHeaderLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 1))   '詳細
        Me.SearchHeaderLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 33))   '詳細 (予約客検索)

        'キャンセルボタン
        Me.NewChipCancelBtn.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 2))     'キャンセル
        Me.SearchCancelBtn.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 34))     'キャンセル (予約客検索)

        '登録ボタン
        Me.NewChipRegisterBtn.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 3))   '登録
        Me.SearchRegisterBtn.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 35))   '登録 (予約客検索)

        OutputLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 画面コントロールに固定文言を設定する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetNewChipWord()

        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        '**********************************
        '* チップ新規作成画面
        '**********************************
        'ステータス
        Me.NewChipChipStatusLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 4))           '新規予約

        '車両情報エリア
        Me.NewChipRegNoWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 5))            '登録No.
        Me.NewChipVinWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 6))              'VIN
        Me.NewChipVehicleWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 7))          '車種

        '顧客情報エリア
        Me.NewChipCstNameWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 8))          '顧客名
        Me.NewChipTitleWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 9))            '敬称
        Me.NewChipHomeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 10))            '固定番号
        Me.NewChipMobileWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 11))          '携帯番号
        Me.NewChipCstAddressWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 12))      '住所
        Me.NewChipSAWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 13))              '担当SA

        '整備種類エリア
        Me.NewChipMaintenanceTypeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 14)) '整備種類
        Me.NewChipMercWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 15))            '整備名

        '予定・実績時間エリア
        Me.NewChipVisitTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 16))       '来店予定
        Me.NewChipStartTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 17))       '開始予定
        Me.NewChipFinishTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 18))      '終了予定
        Me.NewChipDeliveredTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 19))   '納車予定

        'チェックエリア
        Me.NewChipReservationCheckWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 20))            '予約有無
        Me.NewChipReservationYesWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 21))              '予約
        Me.NewChipWalkInWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 22))                      '飛込み
        Me.NewChipWaitingCheckWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 23))                '待ち方
        Me.NewChipWaitingInsideWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 24))               '店内
        Me.NewChipWaitingOutsideWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 25))              '店外
        Me.NewChipCarWashCheckWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 26))                '洗車有無
        Me.NewChipCarWashYesWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 27))                  '有り
        Me.NewChipCarWashNoWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 28))                   '無し
        Me.NewChipCompleteExaminationCheckWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 29))    '検査有無
        Me.NewChipCompleteExaminationYesWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 30))      '有り
        Me.NewChipCompleteExaminationNoWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 31))       '無し

        'ご用命エリア
        Me.NewChipOrderWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 32))           'ご用命

        '登録時に休憩／使用不可チップと重複する場合の文言 (休憩を取得しますか？(取得する場合はOKを選択、取得しない場合はキャンセルを選択))
        Me.NewChipWordDuplicateRestOrUnavailableHidden.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 908))
        Me.NewChipCstBtnErrMsgHidden.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 910))   '新規顧客登録をしてください。

        '**********************************
        '* 顧客検索画面
        '**********************************
        Me.SelectRegNo.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 36))             '登録No.
        Me.SelectVin.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 37))               'VIN
        Me.SelectName.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 38))              '名前
        Me.SelectTelNo.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 39))             'TEL

        Me.SearchPlaceRegNo.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 40))        '登録No.で検索
        Me.SearchPlaceVin.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 41))          'VINで検索
        Me.SearchPlaceName.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 42))         '名前で検索
        Me.SearchPlacePhone.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 43))        'TELで検索
        Me.SearchBottomButton.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 48))     '顧客詳細

        Me.SearchErrMsg1Hidden.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 903))   '検索文字を入力してください。

        OutputLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 顧客検索画面の検索結果エリアを初期化する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitSearchResultArea()

        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        '前のn件を非表示にする
        Dim divFrontLink As HtmlContainerControl
        divFrontLink = CType(Me.FrontLink, HtmlContainerControl)
        divFrontLink.Style("display") = "none"

        '次のn件を非表示にする
        Dim divNextLink As HtmlContainerControl
        divNextLink = CType(Me.NextLink, HtmlContainerControl)
        divNextLink.Style("display") = "none"

        '「条件に一致する検索結果がありません」を表示する
        Dim divSearchList As HtmlContainerControl = CType(Me.NoSearchImage, HtmlContainerControl)
        divSearchList.InnerHtml = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 49)
        divSearchList.Style("display") = "block"

        OutputLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' チップ新規作成に初期表示用データを設定
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
    ''' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
    ''' </history>
    Private Sub SetNewChipDisplayData(ByVal argument As NewChipCallBackArgumentClass, _
                                      ByRef argumentForGetChangeMercInfo As NewChipCallBackArgumentClass)

        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        '作業開始予定日時
        Me.NewChipPlanStartTimeLabel.Text = DateTimeFunc.FormatDate(2, argument.DisplayStartDate)
        Me.NewChipPlanStartDateTimeSelector.Value = argument.DisplayStartDate

        '作業終了予定日時
        Me.NewChipPlanFinishTimeLabel.Text = DateTimeFunc.FormatDate(2, argument.DisplayEndDate)
        Me.NewChipPlanFinishDateTimeSelector.Value = argument.DisplayEndDate

        '来店予定日時
        Me.NewChipPlanVisitTimeLabel.Text = ""
        Me.NewChipPlanVisitDateTimeSelector.Value = Nothing

        '納車予定日時
        Me.NewChipPlanDeriveredTimeLabel.Text = ""
        Me.NewChipPlanDeriveredDateTimeSelector.Value = Nothing

        ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
        ' 検査必要フラグを必要にする。
        Me.NewChipCompleteExaminationFlgHidden.Value = INSPECTION_NEED_FLG_NEED

        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
        ' ' 洗車必要フラグを必要にする。
        'Me.NewChipCarWashFlgHidden.Value = CAR_WASH_NEED_FLG_NEED

        ' 洗車必要フラグを不要にする。
        Me.NewChipCarWashFlgHidden.Value = CAR_WASH_NEED_FLG_NEEDLESS
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
        ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

        ' ストールに紐づくサービス分類情報を取得する。
        Dim rowServiceClass As TabletSmbCommonClassServiceClassRow = Nothing
        Using bl As TabletSMBCommonClassBusinessLogic = New TabletSMBCommonClassBusinessLogic
            rowServiceClass = bl.GetSvcClassInfo(argument.StallId)
        End Using

        ' サービス分類情報を取得できた場合
        If (rowServiceClass IsNot Nothing) Then
            ' ストールに紐づく洗車必要フラグ設定
            Me.NewChipCarWashFlgHidden.Value = rowServiceClass.CARWASH_NEED_FLG

            ' 検査必要フラグ設定
            If (SVC_CLASS_TYPE_EM.Equals(rowServiceClass.SVC_CLASS_TYPE) _
                OrElse SVC_CLASS_TYPE_PM.Equals(rowServiceClass.SVC_CLASS_TYPE)) Then
                ' サービス分類情報．サービス分類区分が「"1"：ＥＭ、"2"：ＰＭ」の場合
                ' 検査必要フラグを'0'（検査不要）にする。
                Me.NewChipCompleteExaminationFlgHidden.Value = INSPECTION_NEED_FLG_NEEDLESS

            Else
                ' 上記以外の場合
                ' 検査必要フラグを'1'（検査必要）にする。
                Me.NewChipCompleteExaminationFlgHidden.Value = INSPECTION_NEED_FLG_NEED
            End If
        End If
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

        Dim SATable As IC3810601DataSet.AcknowledgeStaffListDataTable
        Using rtnDs As New SC3240501DataSet
            Using bl As SC3240501BusinessLogic = New SC3240501BusinessLogic()
                '整備種類情報をチップ情報表示用データテーブルに設定
                bl.SetSvcData(rtnDs, objStaffContext.DlrCD, objStaffContext.BrnCD)

                '敬称情報をチップ情報表示用データテーブルに設定
                bl.SetNameTitleData(rtnDs)

                '担当SAを取得
                SATable = bl.GetAcknowledgeStaffList(objStaffContext.DlrCD, objStaffContext.BrnCD)
            End Using
            '整備種類
            Me.NewChipMaintenanceTypeList.Items.Clear()
            Me.NewChipMaintenanceTypeList.DataSource = rtnDs.SC3240501SvcClassList
            Me.NewChipMaintenanceTypeList.DataTextField = "SVC_CLASS_NAME"
            Me.NewChipMaintenanceTypeList.DataValueField = "SVCID_TIME"
            Me.NewChipMaintenanceTypeList.DataBind()
            Me.NewChipMaintenanceTypeList.Items.Insert(0, String.Empty)
            Me.NewChipMaintenanceTypeList.Items(0).Value = COMBO_INIT_VALUE

            Me.NewChipMaintenanceTypeLabel.Text = String.Empty
            Me.NewChipMaintenanceTypeList.SelectedValue = 0

            ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
            If (rowServiceClass IsNot Nothing) Then
                For i As Integer = 0 To rtnDs.SC3240501SvcClassList.Count - 1

                    Dim selectSvcClassDr As SC3240501DataSet.SC3240501SvcClassListRow = _
                        DirectCast(rtnDs.SC3240501SvcClassList.Rows(i), SC3240501DataSet.SC3240501SvcClassListRow)

                    ' 整備種類の中にストールに紐づくサービス分類IDがある場合
                    If (rowServiceClass.SVC_CLASS_ID = CType(selectSvcClassDr.SVCID_TIME.Split(CChar(","))(0), Decimal)) Then
                        ' 整備種類のデフォルト値設定
                        Me.NewChipMaintenanceTypeLabel.Text = CType(selectSvcClassDr.SVC_CLASS_NAME, String)
                        Me.NewChipMaintenanceTypeList.SelectedValue = CType(selectSvcClassDr.SVCID_TIME, String)

                        '  商品情報取得のコールバック用引数設定
                        argumentForGetChangeMercInfo.DlrCD = rowServiceClass.DLR_CD
                        argumentForGetChangeMercInfo.StrCD = rowServiceClass.BRN_CD
                        argumentForGetChangeMercInfo.SvcClassId = rowServiceClass.SVC_CLASS_ID

                        Exit For
                    End If
                Next i
            End If
            ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

            '敬称
            Me.NewChipTitleList.Items.Clear()
            Me.NewChipTitleList.DataSource = rtnDs.SC3240501NameTitleList
            Me.NewChipTitleList.DataTextField = "NAMETITLE_NAME"
            Me.NewChipTitleList.DataValueField = "NAMETITLE_CD"
            Me.NewChipTitleList.DataBind()
            Me.NewChipTitleList.Items.Insert(0, String.Empty)
            Me.NewChipTitleList.Items(0).Value = COMBO_INIT_VALUE

            Me.NewChipTitleLabel.Text = String.Empty
            Me.NewChipTitleList.SelectedValue = 0

            '担当SA
            Me.NewChipSAList.Items.Clear()
            Me.NewChipSAList.DataSource = SATable
            Me.NewChipSAList.DataTextField = "USERNAME"
            Me.NewChipSAList.DataValueField = "ACCOUNT"
            Me.NewChipSAList.DataBind()
            Me.NewChipSAList.Items.Insert(0, String.Empty)
            Me.NewChipSAList.Items(0).Value = COMBO_INIT_VALUE

            Me.NewChipSALabel.Text = String.Empty
            Me.NewChipSAList.SelectedValue = 0
        End Using

        '整備名
        Me.NewChipMercLabel.Text = String.Empty
        Me.NewChipMercList.Enabled = False

        'ご用命
        Me.NewChipOrderTxt.Text = String.Empty

        '入庫日時・納車日時必須フラグ (1:必須)の設定
        Using smbCommonBiz As New ServiceCommonClassBusinessLogic

            '入庫日時・納車日時必須フラグの取得
            Dim mandatoryFlg As String = smbCommonBiz.GetSystemSettingValueBySettingName(SYS_DATETIME_MANDATORY_FLG)

            If String.IsNullOrEmpty(mandatoryFlg) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                              "{0}.Error ErrCode:Failed to get System SCHE_SVCIN_DELI_DATETIME_MANDATORY_FLG.", _
                              MethodBase.GetCurrentMethod.Name))
                'システム設定値から取得できない場合、固定値(1:必須)とする
                Me.NewChipMandatoryFlgHidden.Value = "1"
            Else
                Me.NewChipMandatoryFlgHidden.Value = mandatoryFlg
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
                Me.NewChipMercMandatoryTypeHidden.Value = "0"

            Else
                '取得できた場合

                '取得値を設定する
                Me.NewChipMercMandatoryTypeHidden.Value = mercMandatoryType

            End If

            '2015/06/15 TMEJ 河原  サービス入庫予約情報のバリデーション実行タイミング変更に向けた評価用アプリ作成 END

        End Using

        ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
        ' 予定入庫日時、予定納車日時算出
        CalculateScheSvcinDateTimeScheDeliDateTime()
        ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

        OutputLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 更新用の時間をセットする(休憩・使用不可チップ考慮前)
    ''' </summary>
    ''' <param name="arg"></param>
    ''' <remarks></remarks>
    Private Sub SetUpdatingTime(ByVal arg As NewChipCallBackArgumentClass)

        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        Me.updStartPlanTime = Nothing             '更新用の予定開始日時
        Me.updFinishPlanTime = Nothing            '更新用の予定終了日時

        '予定開始日時
        If Not String.IsNullOrEmpty(arg.StartPlanTime) Then
            Me.updStartPlanTime = CDate(arg.StartPlanTime)
        End If

        '予定終了日時
        If Not String.IsNullOrEmpty(arg.FinishPlanTime) Then
            Me.updFinishPlanTime = CDate(arg.FinishPlanTime)
        End If

        OutputLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 登録前チェックを行う
    ''' </summary>
    ''' <param name="arg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckBeforeRegistNewChip(ByVal arg As NewChipCallBackArgumentClass, _
                                                ByVal dtNow As Date) As Integer

        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        Dim rtnVal As Integer = 0
        Dim bizLogic As New SC3240501BusinessLogic

        Try
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            ''休憩取得フラグが-1(まだ未チェック)の場合
            'If arg.RestFlg = -1 Then
            '    '休憩・使用不可チップとの重複チェックを行い、衝突ありの場合
            '    If Not bizLogic.CheckRestOrUnavailableChipCollision(arg) Then
            '        rtnVal = 908        'confirmメッセージを出力するためのコードをセット

            '        OutputLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
            '        Return rtnVal
            '    End If
            'End If

            Dim wkRestFlg As String         '休憩取得フラグ

            Using biz As New TabletSMBCommonClassBusinessLogic

                '休憩取得フラグが-1(まだ未チェック)かつ休憩を自動判定しない場合
                If arg.RestFlg = -1 AndAlso Not biz.IsRestAutoJudge() Then
                    '休憩との重複チェックを行い、衝突ありの場合
                    If Not bizLogic.CheckRestOrUnavailableChipCollision(arg) Then
                        rtnVal = 908        'confirmメッセージを出力するためのコードをセット

                        OutputLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                        Return rtnVal
                    End If
                End If

                '休憩を自動判定しない場合
                If Not biz.IsRestAutoJudge() Then
                    '休憩を取得しない場合
                    If arg.RestFlg = 0 Then
                        wkRestFlg = NOT_USE_REST
                    Else
                        wkRestFlg = USE_REST
                    End If
                Else
                    wkRestFlg = USE_REST
                End If

            End Using
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

            '休憩取得フラグが0(休憩を取得しない)以外  の場合
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            'If (arg.RestFlg <> 0) Then
            If wkRestFlg <> NOT_USE_REST Then
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                '更新用の各種日時を算出する
                Dim workStartDateTime As Date   '作業開始日時
                Dim workEndDateTime As Date     '作業終了日時
                Dim workTime As Long            '作業時間
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'Dim wkRestFlg As String         '休憩取得フラグ

                ''休憩を取得しない場合
                'If arg.RestFlg = 0 Then
                '    wkRestFlg = NOT_USE_REST
                'Else
                '    wkRestFlg = USE_REST
                'End If
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

                    '予定作業時間をセット
                    workTime = CLng(arg.WorkTime)

                    ''開始日時を算出
                    If arg.RestFlg = 1 Then
                        '休憩取得フラグが1(休憩を取得する)の場合

                        workStartDateTime = clsTabletSMBCommonClass.GetServiceStartDateTime(CType(arg.StallId, Decimal), _
                                                                                            arg.NewChipDispStartDate, _
                                                                                            arg.InputStallStartTime, _
                                                                                            arg.InputStallEndTime, _
                                                                                            wkRestFlg)
                    Else
                        workStartDateTime = Date.Parse(arg.StartPlanTime, CultureInfo.InvariantCulture)

                    End If

                    '終了日時を算出
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                    'workEndDateTime = clsTabletSMBCommonClass.GetServiceEndDateTime(CType(arg.StallId, Decimal), _
                    '                                                                arg.NewChipDispStartDate, _
                    '                                                                workTime, _
                    '                                                                arg.InputStallStartTime, _
                    '                                                                arg.InputStallEndTime, _
                    '                                                                wkRestFlg)
                    Dim serviceEndDateTimeData As ServiceEndDateTimeData = clsTabletSMBCommonClass.GetServiceEndDateTime(CType(arg.StallId, Decimal), _
                                                                                    arg.NewChipDispStartDate, _
                                                                                    workTime, _
                                                                                    arg.InputStallStartTime, _
                                                                                    arg.InputStallEndTime, _
                                                                                    wkRestFlg)
                    workEndDateTime = serviceEndDateTimeData.ServiceEndDateTime
                    arg.RestFlg = CInt(serviceEndDateTimeData.RestFlg)
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                End Using

                '更新用の各種日時をセットする
                If Not IsNothing(workStartDateTime) AndAlso (workStartDateTime > Date.MinValue) Then
                    Me.updStartPlanTime = workStartDateTime
                End If

                If Not IsNothing(workEndDateTime) AndAlso (workEndDateTime > Date.MinValue) Then
                    Me.updFinishPlanTime = workEndDateTime
                End If
            End If

            'チップの配置時間衝突チェック
            If Not bizLogic.CheckChipCollision(arg, Me.updFinishPlanTime, dtNow) Then

                rtnVal = 905

                OutputLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                Return rtnVal

                '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            ElseIf bizLogic.CheckStallUnavailableOverlapPosition(arg, Me.updFinishPlanTime) Then

                'ストール使用不可と重複している旨の文言番号を格納
                rtnVal = 916

                OutputLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)
                Return rtnVal
                '2019/07/10 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
            End If

        Finally
            bizLogic = Nothing
        End Try

        OutputLog(MethodBase.GetCurrentMethod.Name, False, "[rtnVal:{0}]", rtnVal)

        Return rtnVal

    End Function


    ' ''' <summary>
    ' ''' コントロールに指定したCSSクラスを追加する
    ' ''' </summary>
    ' ''' <param name="element">コントロールオブジェクト</param>
    ' ''' <param name="cssClass">CSSクラス名</param>
    ' ''' <remarks></remarks>
    'Private Sub AddCssClass(ByVal element As HtmlGenericControl, ByVal cssClass As String)

    '    OutputLog(MethodBase.GetCurrentMethod.Name, True, "[element:{0}][cssClass:{1}]", element.ClientID, cssClass)

    '    If String.IsNullOrEmpty(element.Attributes("Class").Trim) Then
    '        element.Attributes("Class") = cssClass
    '    Else
    '        element.Attributes("Class") = element.Attributes("Class") & Space(1) & cssClass
    '    End If

    '    OutputLog(MethodBase.GetCurrentMethod.Name, False)

    'End Sub

    ''' <summary>
    ''' 顧客情報の検索処理
    ''' </summary>
    ''' <param name="dealerCode">顧客コード</param>
    ''' <param name="storeCode">販売店コード</param>
    ''' <remarks></remarks>
    Private Sub GetCustomerInfomation(ByVal dealerCode As String,
                                      ByVal storeCode As String)

        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        '検索条件をHiddenから取得
        Dim searchRegistrationNumber As String = Me.SearchRegistrationNumberHidden.Value
        Dim searchVin As String = Me.SearchVinHidden.Value
        Dim searchCustomerName As String = Me.SearchCustomerNameHidden.Value
        Dim searchPhoneNumber As String = Me.SearchPhoneNumberHidden.Value
        Dim searchStartRow As Long = CType(Me.SearchStartRowHidden.Value, Long)
        Dim searchEndRow As Long = CType(Me.SearchEndRowHidden.Value, Long)
        Dim searchSelectType As Long = CType(Me.SearchSelectTypeHidden.Value, Long)

        '前のN件
        Dim divFrontLink As HtmlContainerControl
        divFrontLink = CType(Me.FrontLink, HtmlContainerControl)

        '次のN件
        Dim divNextLink As HtmlContainerControl
        divNextLink = CType(Me.NextLink, HtmlContainerControl)

        '「条件に一致する検索結果がありません」
        Dim divSearchList As HtmlContainerControl = CType(Me.NoSearchImage, HtmlContainerControl)
        divSearchList.InnerHtml = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 49)

        '顧客検索処理を実行
        Dim result As SC3240501SearchResult
        Using bl As SC3240501BusinessLogic = New SC3240501BusinessLogic()
            result = bl.GetCustomerList(dealerCode, _
                                        storeCode, _
                                        searchRegistrationNumber, _
                                        searchVin, _
                                        searchCustomerName, _
                                        searchPhoneNumber, _
                                        searchStartRow, _
                                        searchEndRow, _
                                        searchSelectType)
        End Using

        '顧客情報が取得できた場合
        If result.DataTable IsNot Nothing AndAlso 0 < result.DataTable.Count Then

            'コントロールにバインドする
            Me.SearchRepeater.DataSource = result.DataTable.Select()
            Me.SearchRepeater.DataBind()

            Dim searchData As Control
            Dim row As SC3240501DataSet.SC3240501CustomerListRow

            Dim rowList As SC3240501DataSet.SC3240501CustomerListRow() = _
                DirectCast(Me.SearchRepeater.DataSource, SC3240501DataSet.SC3240501CustomerListRow())

            Dim CustomerChangeParameterDiv As HtmlContainerControl

            For i = 0 To SearchRepeater.Items.Count - 1

                searchData = SearchRepeater.Items(i)
                row = rowList(i)

                '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                '顧客車両区分を付与した顧客氏名を取得
                Using GetCstNameAddCstVclType As New ServiceCommonClassBusinessLogic

                    row.CST_NAME = GetCstNameAddCstVclType.GetCstNameWithCstVclType(row.CST_NAME, _
                                                                                    row.CST_VCL_TYPE, _
                                                                                    row.CST_TYPE)
                    If String.IsNullOrEmpty(row.CST_NAME) Then
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0},{1}.Error GetCstNameAddCstVclType Is Nothing CST_ID {2} VCL_ID {3}.", _
                                      Me.GetType.ToString, _
                                      MethodBase.GetCurrentMethod.Name, _
                                      row.CST_ID, _
                                      row.VCL_ID))

                    End If

                End Using

                '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                '顧客名称
                CType(searchData.FindControl("SearchCustomerName"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.CST_NAME)
                '車両登録No
                CType(searchData.FindControl("SearchRegistrationNumber"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.REG_NUM)
                'VIN
                CType(searchData.FindControl("SearchVinNo"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.VCL_VIN)
                '車種
                CType(searchData.FindControl("SearchModel"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.MODEL_NAME)
                '電話番号
                CType(searchData.FindControl("SearchPhone"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.CST_PHONE)
                '携帯電話番号
                CType(searchData.FindControl("SearchMobile"), HtmlContainerControl).InnerHtml = Me.SetNullToString(row.CST_MOBILE)

                '紐付け用データの設定
                CustomerChangeParameterDiv = DirectCast(searchData.FindControl("CustomerChangeParameter"), HtmlContainerControl)
                With CustomerChangeParameterDiv
                    .Attributes("CstIdParameter") = row.CST_ID                                          '顧客ID
                    .Attributes("VclIdParameter") = row.VCL_ID                                          '車両ID
                    .Attributes("VinParameter") = row.VCL_VIN                                           'VIN
                    .Attributes("CustomerAddressParameter") = row.CST_ADDRESS                           '顧客住所
                    .Attributes("NameTitleCodeParameter") = row.NAMETITLE_CD                            '敬称コード
                    .Attributes("NameTitleNameParameter") = row.NAMETITLE_NAME                          '敬称
                    .Attributes("CustomerVehicleTypeParameter") = row.CST_VCL_TYPE                      '顧客車両区分
                    '以下のパラメータは外部結合テーブルから取得するため、NULLの可能性あり
                    .Attributes("SaCodeParameter") = Me.SetNullToString(row.SVC_PIC_STF_CD)             'SAコード
                    .Attributes("SaNameParameter") = Me.SetNullToString(row.STF_NAME)                   '担当SA名
                    .Attributes("VehicleParameter") = Me.SetNullToString(row.MODEL_NAME)                '車種
                    .Attributes("DmsCstCodeParameter") = Me.SetNullToString(row.DMS_CST_CD.Trim())      '基幹顧客コード
                End With
            Next

            '次の表示件数，および前の表示件数の設定
            Dim customerCount As Long = result.ResultCustomerCount
            Dim resultStartRow As Long = result.ResultStartRow
            Dim resultEndRow As Long = result.ResultEndRow
            Dim standardCount As Long = result.StandardCount
            Me.SetOtherDisplay(resultStartRow, resultEndRow, customerCount, standardCount)
            divSearchList.Style("display") = "none"

            'スクロール設定
            Dim differenceRow As Long = 0
            If 0 < searchSelectType Then
                ' 前回終了位置と今回開始位置の差分を求める
                Dim beforeEndRow As Long = searchEndRow - 4
                differenceRow = beforeEndRow - resultStartRow + 1
            ElseIf searchSelectType < 0 Then
                ' 前回開始位置と今回開始位置の差分を求める
                differenceRow = searchStartRow - resultStartRow - 1
            End If
            If 1 < resultStartRow Then
                ' 今回開始位置が1行目以降の場合、前のN件表示分、行が加算される
                Me.ScrollPositionHidden.Value = _
                    ((differenceRow + 1) * SEARCH_LIST_HEIGHT).ToString(CultureInfo.CurrentCulture)
            Else
                Me.ScrollPositionHidden.Value = _
                    (differenceRow * SEARCH_LIST_HEIGHT).ToString(CultureInfo.CurrentCulture)
            End If

        Else
            '顧客情報が取得できなかった場合
            divSearchList.Style("display") = "block"
            divFrontLink.Style("display") = "none"
            divNextLink.Style("display") = "none"
            Me.ScrollPositionHidden.Value = "0"
        End If

        OutputLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 次回表示件数の設定（次のN件，前のN件）
    ''' </summary>
    ''' <param name="startRow">開始行番号</param>
    ''' <param name="endRow">終了行番号</param>
    ''' <param name="customerCount">顧客検索件数</param>
    ''' <param name="standardCount">標準取得件数</param>
    ''' <remarks></remarks>
    Private Sub SetOtherDisplay(ByVal startRow As Long, _
                                ByVal endRow As Long, _
                                ByVal customerCount As Long, _
                                ByVal standardCount As Long)

        OutputLog(MethodBase.GetCurrentMethod.Name, True, "[startRow:{0}][endRow:{1}][customerCount:{2}][standardCount:{3}]", startRow, endRow, customerCount, standardCount)

        If 0 < customerCount Then
            ' 前の件数検索表示の設定
            Dim displayFront As String
            Dim divFrontLink As HtmlContainerControl
            Dim divFrontList As HtmlContainerControl
            Dim divFrontListSearching As HtmlContainerControl
            divFrontLink = CType(Me.FrontLink, HtmlContainerControl)
            divFrontList = CType(Me.FrontList, HtmlContainerControl)
            divFrontListSearching = CType(Me.FrontListSearching, HtmlContainerControl)
            If 1 < startRow Then
                If startRow <= standardCount Then
                    displayFront = (startRow - 1).ToString(CultureInfo.CurrentCulture)
                Else
                    displayFront = standardCount.ToString(CultureInfo.CurrentCulture)
                End If

                '「前の{0}件を読み込む…」
                divFrontList.InnerHtml = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 44).Replace("{0}", CType(displayFront, String))

                '「前の{0}件を読み込み中…」
                divFrontListSearching.InnerHtml = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 45).Replace("{0}", CType(displayFront, String))

                divFrontLink.Style("display") = "block"
            Else
                divFrontLink.Style("display") = "none"
            End If

            ' 次の件数検索表示の設定
            Dim displayNext As String
            Dim divNextLink As HtmlContainerControl
            Dim divNextList As HtmlContainerControl
            Dim divNextListSearching As HtmlContainerControl
            divNextLink = CType(Me.NextLink, HtmlContainerControl)
            divNextList = CType(Me.NextList, HtmlContainerControl)
            divNextListSearching = CType(Me.NextListSearching, HtmlContainerControl)
            If endRow < customerCount Then
                Dim differenceEndRow As Long = customerCount - endRow
                If differenceEndRow < standardCount Then
                    displayNext = CType(differenceEndRow, String)
                Else
                    displayNext = standardCount.ToString(CultureInfo.CurrentCulture)
                End If

                '「次の{0}件を読み込む…」
                divNextList.InnerHtml = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 46).Replace("{0}", CType(displayNext, String))

                '「次の{0}件を読み込み中…」
                divNextListSearching.InnerHtml = WebWordUtility.GetWord(NEWCHIP_PROGRAMID, 47).Replace("{0}", CType(displayNext, String))

                divNextLink.Style("display") = "block"
            Else
                divNextLink.Style("display") = "none"
            End If
            ' 開始行，終了行を記憶する
            Me.SearchStartRowHidden.Value = startRow.ToString(CultureInfo.CurrentCulture)
            Me.SearchEndRowHidden.Value = endRow.ToString(CultureInfo.CurrentCulture)
        End If

        OutputLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
    ''' <summary>
    ''' 予定入庫日時、予定納車日時算出
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
    ''' </history>
    Private Sub CalculateScheSvcinDateTimeScheDeliDateTime()

        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        ' サービス標準LT取得
        Dim standardLTListDataTable As StandardLTListDataTable = Nothing
        Using bl As IC3810701BusinessLogic = New IC3810701BusinessLogic()
            standardLTListDataTable = bl.GetStandardLTList(objStaffContext.DlrCD, _
                                                           objStaffContext.BrnCD)
        End Using

        ' 取得有無（データ件数）判定
        If standardLTListDataTable.Rows.Count = 0 Then
            ' 取得できなかった（データ件数0件）場合
            ' Hidden項目に初期値を設定する。
            ' 予定入庫納車自動表示フラグに0（非表示）を設定する。
            Me.ScheSvcinDeliAutoDispFlg.Value = SCHE_SVCIN_DELI_AUTO_DISP_FLG_DEFAULT
            ' 各標準時間に0（分）を設定する。
            Me.StdAcceptanceTime.Value = STD_TIME_DEFAULT
            Me.StdInspectionTime.Value = STD_TIME_DEFAULT
            Me.StdDeliPreparationTime.Value = STD_TIME_DEFAULT
            Me.StdCarwashTime.Value = STD_TIME_DEFAULT
            Me.StdDeliTime.Value = STD_TIME_DEFAULT

        Else
            ' 取得できた（データ件数1件以上）場合
            ' Hidden項目にサービス標準LT取得データを設定する。
            Me.ScheSvcinDeliAutoDispFlg.Value = standardLTListDataTable(0).SCHE_SVCIN_DELI_AUTO_DISP_FLG
            Me.StdAcceptanceTime.Value = standardLTListDataTable(0).RECEPT_STANDARD_LT
            Me.StdInspectionTime.Value = standardLTListDataTable(0).STD_INSPECTION_TIME
            Me.StdDeliPreparationTime.Value = standardLTListDataTable(0).DELIVERYPRE_STANDARD_LT
            Me.StdCarwashTime.Value = standardLTListDataTable(0).WASHTIME
            Me.StdDeliTime.Value = standardLTListDataTable(0).DELIVERYWR_STANDARD_LT
        End If

        ' 予定入庫納車自動表示フラグが「1: 表示」の場合、自動計算を行う。
        If SCHE_SVCIN_DELI_AUTO_DISP_FLG_ENABLE.Equals(Me.ScheSvcinDeliAutoDispFlg.Value) Then

            ' 予定入庫日時算出
            ' 予定開始日時 － 標準受付時間（分）
            Dim scheSvcinDatetime As Date = Me.NewChipPlanStartDateTimeSelector.Value
            scheSvcinDatetime = scheSvcinDatetime.AddMinutes(-1 * Me.StdAcceptanceTime.Value)

            ' 来店予定日時設定
            Me.NewChipPlanVisitTimeLabel.Text = DateTimeFunc.FormatDate(2, scheSvcinDatetime)
            Me.NewChipPlanVisitDateTimeSelector.Value = scheSvcinDatetime

            ' 予定納車日時算出
            ' 予定終了日時取得
            Dim scheDeliDatetime As Date = Me.NewChipPlanFinishDateTimeSelector.Value

            ' 検査が必要な場合、標準検査時間（分）を加算する。
            If (INSPECTION_NEED_FLG_NEED.Equals(Me.NewChipCompleteExaminationFlgHidden.Value)) Then
                scheDeliDatetime = scheDeliDatetime.AddMinutes(Me.StdInspectionTime.Value)
            End If

            ' 標準納車準備時間（分）、標準洗車時間（分）のうち、長いほうを加算する。
            Dim addTime As Long = Me.StdDeliPreparationTime.Value
            Dim stdCarwashTime As Long = Me.StdCarwashTime.Value
            If (CAR_WASH_NEED_FLG_NEED.Equals(Me.NewChipCarWashFlgHidden.Value) _
                And (addTime < stdCarwashTime)) Then
                addTime = stdCarwashTime
            End If
            scheDeliDatetime = scheDeliDatetime.AddMinutes(addTime)

            ' 標準納車時間（分）を加算する。
            scheDeliDatetime = scheDeliDatetime.AddMinutes(Me.StdDeliTime.Value)

            ' 納車予定日時設定
            Me.NewChipPlanDeriveredTimeLabel.Text = DateTimeFunc.FormatDate(2, scheDeliDatetime)
            Me.NewChipPlanDeriveredDateTimeSelector.Value = scheDeliDatetime
        End If
    End Sub
    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

    ''' <summary>
    ''' NULLから文字列への変換
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns>変換値</returns>
    Private Function SetNullToString(ByVal str As String, Optional ByVal strNull As String = "") As String

        ' 空白チェック
        If String.IsNullOrWhiteSpace(str) Then
            Return strNull
        End If

        Return str

    End Function
#End Region

#Region "イベント処理メソッド"
    ''' <summary>
    ''' 顧客詳細画面へ遷移するためのダミーボタンClick処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub NewChipCustButtonDummy_Click(sender As Object, e As System.EventArgs) Handles NewChipCustButtonDummy.Click

        OutputLog(MethodBase.GetCurrentMethod.Name, True)

        '基幹顧客コード
        Dim dmsCstCd As String = Me.SearchDmsCstCodeChange.Value.Trim()
        If String.IsNullOrEmpty(dmsCstCd) Then
            dmsCstCd = ""
        End If

        'VIN
        Dim vin As String = Me.SearchVinChange.Value.Trim()
        If String.IsNullOrEmpty(vin) Then
            vin = ""
        End If

        '次画面遷移パラメータ設定
        Me.SetValue(ScreenPos.Next, "SessionKey.DMS_CST_ID", dmsCstCd)            '基幹顧客コード
        Me.SetValue(ScreenPos.Next, "SessionKey.VIN", vin)                        'VIN

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} [DMS_CST_ID:{2}][VIN:{3}]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dmsCstCd, vin))

        ' 顧客詳細画面に遷移
        CType(Me.Page, BasePage).RedirectNextScreen(APPLICATIONID_CUSTOMEROUT)

        OutputLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub
#End Region

#Region "ページクラス処理のバイパス処理"
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

#Region "コールバック用内部クラス"
    ''' <summary>
    ''' コールバック結果をクライアントに返すための内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackResultClass

        Private _caller As String
        Private _resultCode As Short
        Private _message As String
        Private _contents As String
        Private _newChipJson As String

        ''' <summary>
        ''' 呼び出し元メソッド(JavaScript側)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Caller() As String
            Get
                Return _caller
            End Get
            Set(ByVal value As String)
                _caller = value
            End Set
        End Property

        ''' <summary>
        ''' 処理結果コード
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ResultCode() As Short
            Get
                Return _resultCode
            End Get
            Set(ByVal value As Short)
                _resultCode = value
            End Set
        End Property

        ''' <summary>
        ''' メッセージ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                _message = HttpUtility.HtmlEncode(value)
                '_message = value
            End Set
        End Property

        ''' <summary>
        ''' HTMLコンテンツ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Contents() As String
            Get
                Return _contents
            End Get
            Set(ByVal value As String)
                _contents = HttpUtility.HtmlEncode(value)
            End Set
        End Property

        ''' <summary>
        ''' JSON形式のデータ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property NewChipJson() As String
            Get
                Return _newChipJson
            End Get
            Set(ByVal value As String)
                _newChipJson = HttpUtility.HtmlEncode(value)
            End Set
        End Property
    End Class
#End Region

#Region "ログ出力メソッド"

    ''' <summary>
    ''' 引数のないInfoレベルのログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="isStart">True:Startログ/False:Endログ</param>
    ''' <remarks></remarks>
    Private Sub OutputLog(ByVal method As String, ByVal isStart As Boolean)

        If isStart Then
            Logger.Info(NEWCHIP_PROGRAMID & ".ascx " & method & "_Start")
        Else
            Logger.Info(NEWCHIP_PROGRAMID & ".ascx " & method & "_End")
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
    Private Sub OutputLog(ByVal method As String, ByVal isStart As Boolean, ByVal argString As String, ParamArray args() As Object)

        Dim logString As String = String.Empty

        If isStart Then
            logString = NEWCHIP_PROGRAMID & ".ascx " & method & "_Start" & argString
            Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
        Else
            logString = NEWCHIP_PROGRAMID & ".ascx " & method & "_End" & argString
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

        logString = NEWCHIP_PROGRAMID & ".ascx " & method & argString
        Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))

    End Sub

    ''' <summary>
    ''' エラーログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="ex">例外オブジェクト</param>
    ''' <param name="argString">フォーマット用文字列</param>
    ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ''' <remarks></remarks>
    Private Sub OutputErrLog2(ByVal method As String, ByVal ex As Exception, ByVal argString As String, ParamArray args() As Object)

        Dim logString As String = String.Empty

        logString = NEWCHIP_PROGRAMID & ".ascx " & method & "_Error" & argString
        Logger.Error(String.Format(CultureInfo.InvariantCulture, logString, args), ex)

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

        logString = NEWCHIP_PROGRAMID & ".ascx " & method & "_Error" & argString
        Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))

    End Sub

#End Region

End Class
