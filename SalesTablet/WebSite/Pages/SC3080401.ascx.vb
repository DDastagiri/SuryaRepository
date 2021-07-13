'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080401.ascx.vb
'─────────────────────────────────────
'機能： ヘルプ依頼画面
'補足： 
'作成： 2012/01/30 TCS 鈴木(健)
'更新： 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更
'                               （仕変によりSALES_STEP2以降、BMを依頼先から除外）
'更新： 2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正
'                               ※Sales Step1B ユーザーテスト 問題管理No.0074
'更新： 2012/03/13 TCS 鈴木(健) 【SALES_2】アカウント/アカウント名にカンマが含まれると正しいデータが登録できない不具合修正
'                               ※Sales Step1B 号口課題 問題管理No.73の横展
'更新： 2012/04/05 TCS 鈴木(健) 【SALES_2】顧客詳細画面の初期表示時にスクリプトエラーが発生する不具合修正
'更新： 2012/04/12 TCS 鈴木(健) HTMLエンコード対応
'更新： 2013/01/24 TCS 藤井    【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'
'─────────────────────────────────────

Imports System.Globalization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CustomerInfo.Help.BizLogic
Imports Toyota.eCRB.CustomerInfo.Help.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic.Common

''' <summary>
''' ヘルプ依頼画面
''' プレゼンテーション層クラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3080401
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler

#Region "定数"
    ''' <summary>
    ''' 在籍状態（大分類）：スタンバイ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryStandby As String = "1"

    ''' <summary>
    ''' 在籍状態（大分類）：商談中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryNegotiation As String = "2"

    ''' <summary>
    ''' 在籍状態（大分類）：退席中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryLeaving As String = "3"

    ''' <summary>
    ''' 在籍状態（大分類）：オフライン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategoryOffline As String = "4"

    ''' <summary>
    ''' 在籍状態（小分類）：0
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceDetailOff As String = "0"

    ''' <summary>
    ''' 在籍状態（小分類）：1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceDetailOn As String = "1"

    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
    ''' <summary>
    ''' 在籍状態（小分類）：2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceDetail_Two As String = "2"
    '小分類追加
    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End

    ''' <summary>
    ''' 画面間インターフェイスのキー値：顧客種別（1：自社客 / 2：未取引客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyCstKind As String = "SearchKey.CSTKIND"

    ''' <summary>
    ''' 画面間インターフェイスのキー値：顧客分類（1：所有者 / 2：使用者 / 3：その他）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyCustomerClass As String = "SearchKey.CUSTOMERCLASS"

    ''' <summary>
    ''' 画面間インターフェイスのキー値：活動先顧客コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyCrCustID As String = "SearchKey.CRCUSTID"

    ''' <summary>
    ''' 画面間インターフェイスのキー値：Follow-up Box店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyFllwupBoxStrCd As String = "SearchKey.FLLWUPBOX_STRCD"

    ''' <summary>
    ''' 画面間インターフェイスのキー値：Follow-up Box内連番
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyFllwupBoxSeqNo As String = "SearchKey.FOLLOW_UP_BOX"

    ''' <summary>
    ''' 画面間インターフェイスのキー値：顧客名称 + 敬称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyName As String = "SearchKey.NAME"

    ''' <summary>
    ''' 画面間インターフェイスのキー値：顧客担当セールススタッフコード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeySalesStaffCode As String = "SearchKey.SALESSTAFFCD"
#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' パラメータ情報データテーブル
    ''' </summary>
    ''' <remarks></remarks>
    Private dtParam As SC3080401DataSet.SC3080401ParameterDataTable

    ''' <summary>
    ''' コールバックメソッドの呼び出し元に返却する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private CallBackResult As String

    ''' <summary>
    ''' ヘルプ依頼中か否か（True：依頼中 / False：未依頼）
    ''' </summary>
    ''' <remarks></remarks>
    Private IsRequesting As Boolean = False

    ''' <summary>
    ''' 所属店舗のマネージャーが存在するか否か（True：存在する / False：存在しない）
    ''' </summary>
    ''' <remarks></remarks>
    Private IsManagerExists As Boolean = False

    ''' <summary>
    ''' 所属店舗のマネージャーで対応可能なマネージャーが存在するか否か（True：存在する / False：存在しない）
    ''' </summary>
    ''' <remarks></remarks>
    Private IsSendAccountExists As Boolean = False

    ''' <summary>
    ''' ヘルプ内容マスタが存在するか否か（True：存在する / False：存在しない）
    ''' </summary>
    ''' <remarks></remarks>
    Private IsHelpMstExists As Boolean = False
#End Region

#Region "プロパティ"
    ''' <summary>
    ''' ポップアップ表示のトリガーとなるボタンのIDを取得または設定します。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TriggerClientId() As String
        Get
            Return Me.HelpRequestPopOverForm.Attributes("data-TriggerClientID")
        End Get
        Set(ByVal value As String)
            Me.HelpRequestPopOverForm.Attributes("data-TriggerClientID") = value
        End Set
    End Property
#End Region

#Region "イベント"
    '' 2012/04/05 TCS 鈴木(健) 【SALES_2】顧客詳細画面の初期表示時にスクリプトエラーが発生する不具合修正 START
    ' ''' <summary>
    ' ''' ページロード時の処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    '    ' ======================== ログ出力 開始 ========================
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture,
    '                              "[{0}]_[{1}]_Start, sender:[{2}], e:[{3}], IsPostBack:[{4}]",
    '                              SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, sender.ToString, e.ToString, Me.IsPostBack.ToString))
    '    ' ======================== ログ出力 終了 ========================

    '    ' コールバックスクリプトの生成
    '    ScriptManager.RegisterStartupScript(
    '        Me,
    '        Me.GetType(),
    '        "CallbackSC3080401",
    '        String.Format(CultureInfo.InvariantCulture,
    '                      "CallbackSC3080401.beginCallback = function () {{ {0}; }};",
    '                      Page.ClientScript.GetCallbackEventReference(Me, "CallbackSC3080401.packedArgument", "CallbackSC3080401.endCallback", "", False)
    '                      ),
    '        True
    '    )

    '    ' 文言の設定
    '    Me.WordNo0001HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 1)
    '    Me.WordNo0002HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 2)
    '    Me.WordNo0003HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 3)
    '    Me.WordNo0004HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 4)
    '    Me.WordNo0005HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 5)
    '    Me.WordNo0006HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 6)
    '    Me.WordNo0007HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 7)
    '    Me.WordNo0008HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 8)
    '    Me.WordNo0009HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 9)
    '    Me.WordNo9001HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 9001)

    '    ' ======================== ログ出力 開始 ========================
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture,
    '                              "[{0}]_[{1}]_End",
    '                              SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
    '    ' ======================== ログ出力 終了 ========================

    'End Sub

    ''' <summary>
    ''' Control オブジェクトの読み込み後、表示を開始する前に発生するイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/12 TCS 鈴木(健) HTMLエンコード対応
    ''' </History>
    Protected Sub SC3080401Control_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, sender:[{2}], e:[{3}], IsPostBack:[{4}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, sender.ToString, e.ToString, Me.IsPostBack.ToString))
        ' ======================== ログ出力 終了 ========================

        ' コールバックスクリプトの生成
        ScriptManager.RegisterStartupScript(
            Me,
            Me.GetType(),
            "CallbackSC3080401",
            String.Format(CultureInfo.InvariantCulture,
                          "CallbackSC3080401.beginCallback = function () {{ {0}; }};",
                          Page.ClientScript.GetCallbackEventReference(Me, "CallbackSC3080401.packedArgument", "CallbackSC3080401.endCallback", "", False)
                          ),
            True
        )

        ' 文言の設定
        ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
        'Me.WordNo0001HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 1)
        'Me.WordNo0002HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 2)
        'Me.WordNo0003HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 3)
        'Me.WordNo0004HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 4)
        'Me.WordNo0005HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 5)
        'Me.WordNo0006HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 6)
        'Me.WordNo0007HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 7)
        'Me.WordNo0008HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 8)
        'Me.WordNo0009HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 9)
        'Me.WordNo9001HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 9001)
        ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub
    '' 2012/04/05 TCS 鈴木(健) 【SALES_2】顧客詳細画面の初期表示時にスクリプトエラーが発生する不具合修正 END

    ''' <summary>
    ''' 依頼先一覧リピータの行バインド時のイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SendAccountRepeater_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles SendAccountRepeater.ItemDataBound

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, sender:[{2}], e:[{3}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, sender.ToString, e.ToString))
        ' ======================== ログ出力 終了 ========================

        If e.Item.ItemType = ListItemType.Item _
         OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            ' 依頼先一覧リピータコントロールの取得
            Dim SendAccountRow As HtmlGenericControl = DirectCast(e.Item.FindControl("SendAccountRow"), HtmlGenericControl)

            ' 依頼先一覧リピータコントロールのCSSクラス初期化
            SendAccountRow.Attributes("Class") = ""

            ' オンライン状態アイコン欄のDIVコントロール取得
            Dim onlineStatusIconArea As HtmlGenericControl = DirectCast(e.Item.FindControl("OnlineStatusIconArea"), HtmlGenericControl)

            ' オンライン状態アイコン欄に在籍状態オンオフアイコンを設定
            Me.AddCssClass(onlineStatusIconArea, "ncv51OnOffIcn")

            ' 依頼先情報データテーブル行の取得
            Dim row As SC3080401DataSet.SC3080401GetSendAccountRow = DirectCast(e.Item.DataItem.row, SC3080401DataSet.SC3080401GetSendAccountRow)

            ' 在籍状態（大分類）
            Select Case row.PRESENCECATEGORY
                Case PresenceCategoryStandby, PresenceCategoryNegotiation, PresenceCategoryLeaving
                    ' スタンバイ、商談中、退席中はオンライン
                    Me.AddCssClass(SendAccountRow, "Online")
                    Me.AddCssClass(onlineStatusIconArea, "helpRequestNcv51OnIcn")
                Case Else
                    ' 上記以外はオフライン
                    Me.AddCssClass(SendAccountRow, "Offline")
                    Me.AddCssClass(onlineStatusIconArea, "helpRequestNcv51OffIcn")
            End Select

            ' 選択中の依頼先にチェックマーク付与
            If row.ACCOUNT.Equals(Me.SelectedSendAccount.Value) Then
                Me.AddCssClass(SendAccountRow, "Check")
            End If

            ' リストの最終行CSSを設定
            Dim view As Data.DataView = DirectCast(e.Item.DataItem.DataView, Data.DataView)
            If view.Count - 1 = e.Item.ItemIndex Then
                Me.AddCssClass(SendAccountRow, "ListEnd")
            End If
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End, ItemIndex:{2}",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, e.Item.ItemIndex.ToString(CultureInfo.InvariantCulture)))
        ' ======================== ログ出力 終了 ========================

    End Sub

    ''' <summary>
    ''' ヘルプ内容一覧リピータの行バインド時のイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub HelpMstRepeater_ItemDataBound(sender As Object, e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles HelpMstRepeater.ItemDataBound

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, sender:[{2}], e:[{3}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, sender.ToString, e.ToString))
        ' ======================== ログ出力 終了 ========================

        If e.Item.ItemType = ListItemType.Item _
         OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            ' ヘルプ内容一覧リピータコントロールの取得
            Dim HelpMstRow As HtmlGenericControl = DirectCast(e.Item.FindControl("HelpMstRow"), HtmlGenericControl)

            ' ヘルプ内容一覧リピータコントロールのCSSクラス初期化
            HelpMstRow.Attributes("Class") = ""

            ' 選択中のヘルプ内容にチェックマーク付与
            Dim row As SC3080401DataSet.SC3080401GetHelpMstRow = DirectCast(e.Item.DataItem.row, SC3080401DataSet.SC3080401GetHelpMstRow)
            If row.ID.ToString(CultureInfo.InvariantCulture).Equals(Me.SelectedHelpid.Value) Then
                Me.AddCssClass(HelpMstRow, "Check")
            End If

            ' リストの最終行CSSを設定
            Dim view As Data.DataView = DirectCast(e.Item.DataItem.DataView, Data.DataView)
            If view.Count - 1 = e.Item.ItemIndex Then
                Me.AddCssClass(HelpMstRow, "ListEnd")
            End If
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End, ItemIndex:{2}",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, e.Item.ItemIndex.ToString(CultureInfo.InvariantCulture)))
        ' ======================== ログ出力 終了 ========================

    End Sub
#End Region

#Region "Publicメソッド"
    ''' <summary>
    ''' コールバック用文字列を返却します。
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_StartEnd, Return String:[{2}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, CallBackResult))
        ' ======================== ログ出力 終了 ========================

        Return Me.CallBackResult

    End Function

    ''' <summary>
    ''' コールバックイベント時のハンドリング
    ''' </summary>
    ''' <param name="eventArgument"></param>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/03/13 TCS 鈴木(健) 【SALES_2】アカウント/アカウント名にカンマが含まれると正しいデータが登録できない不具合修正
    '''  2012/04/12 TCS 鈴木(健) HTMLエンコード対応
    ''' </History>
    Public Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

        Try
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}.ascx {1}_Start, eventArgument:[{2}]",
                                      SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, eventArgument))
            ' ======================== ログ出力 終了 ========================

            ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
            ' 文言の設定
            Me.WordNo0001HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 1)
            Me.WordNo0002HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 2)
            Me.WordNo0003HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 3)
            Me.WordNo0004HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 4)
            Me.WordNo0005HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 5)
            Me.WordNo0006HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 6)
            Me.WordNo0007HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 7)
            Me.WordNo0008HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 8)
            Me.WordNo0009HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 9)
            Me.WordNo9001HiddenField.Value = WebWordUtility.GetWord(SC3080401TableAdapter.FunctionId, 9001)
            ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END

            ' 変数の宣言
            Dim resultString As String = String.Empty           ' コールバック呼び出し元に返却する文字列
            Dim SendAccount As String = String.Empty            ' 依頼先アカウント
            Dim SendAccountName As String = String.Empty        ' 依頼先アカウント名
            Dim helpid As String = String.Empty                 ' ヘルプID
            Dim helpNo As String = String.Empty                 ' ヘルプNo
            Dim noticeReqId As String = String.Empty            ' 通知依頼ID

            ' セッション情報の設定
            Me.SetSessionValue()

            ' イベントパラメータを配列に格納
            Dim tokens As String() = eventArgument.Split(New Char() {","c})

            ' 呼び出しメソッド名の取得
            Dim method As String = tokens(0)

            ' 呼び出しメソッドの判定
            Select Case method
                Case "CreateHelpRequestWindow"
                    ' 初期表示時

                    ' ヘルプ依頼画面の作成
                    resultString = Me.CreateHelpRequestWindow()

                Case "RequestButton_Click"
                    ' 依頼ボタン押下時

                    ' パラメータの取得
                    ' 2012/03/13 TCS 鈴木(健) 【SALES_2】アカウント/アカウント名にカンマが含まれると正しいデータが登録できない不具合修正 START
                    'SendAccount = tokens(1)             ' 依頼先アカウント
                    'SendAccountName = tokens(2)         ' 依頼先アカウント名
                    SendAccount = HttpUtility.UrlDecode(tokens(1))             ' 依頼先アカウント
                    SendAccountName = HttpUtility.UrlDecode(tokens(2))         ' 依頼先アカウント名
                    ' 2012/03/13 TCS 鈴木(健) 【SALES_2】アカウント/アカウント名にカンマが含まれると正しいデータが登録できない不具合修正 END
                    helpid = tokens(3)                  ' ヘルプID

                    ' ヘルプ依頼の登録処理
                    resultString = Me.RequestButtonClick(SendAccount, SendAccountName, helpid)

                Case "CancelButton_Click"
                    ' キャンセルボタン押下時

                    ' パラメータの取得
                    ' 2012/03/13 TCS 鈴木(健) 【SALES_2】アカウント/アカウント名にカンマが含まれると正しいデータが登録できない不具合修正 START
                    'SendAccount = tokens(1)             ' 依頼先アカウント
                    'SendAccountName = tokens(2)         ' 依頼先アカウント名
                    SendAccount = HttpUtility.UrlDecode(tokens(1))             ' 依頼先アカウント
                    SendAccountName = HttpUtility.UrlDecode(tokens(2))         ' 依頼先アカウント名
                    ' 2012/03/13 TCS 鈴木(健) 【SALES_2】アカウント/アカウント名にカンマが含まれると正しいデータが登録できない不具合修正 END
                    helpid = tokens(3)                  ' ヘルプID
                    helpNo = tokens(4)                  ' ヘルプNo
                    noticeReqId = tokens(5)             ' 通知依頼ID

                    ' ヘルプ依頼のキャンセル処理
                    resultString = Me.CancelButtonClick(SendAccount, SendAccountName, helpid, Long.Parse(helpNo, CultureInfo.InvariantCulture), Long.Parse(noticeReqId, CultureInfo.InvariantCulture))
            End Select

            ' 処理結果をコールバック返却用文字列に設定
            Me.CallBackResult = HttpUtility.HtmlEncode(resultString)

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}.ascx {1}_End, Result String:[{2}]",
                                      SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, Me.CallBackResult))
            ' ======================== ログ出力 終了 ========================
        Catch ex As Exception
            ' エラーメッセージの設定
            Me.CallBackResult = String.Format(CultureInfo.InvariantCulture,
                                              "System Error Occured. Error Code : {0}, Message : {1}",
                                              SC3080401BusinessLogic.MessageIdSys, ex.Message)

            ' ======================== ログ出力 開始 ========================
            Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                       "ProgramID:[{0}], MessageID:[{1}]",
                                       SC3080401TableAdapter.FunctionId, SC3080401BusinessLogic.MessageIdSys),
                                       ex)
            ' ======================== ログ出力 開始 ========================
        End Try

    End Sub
#End Region

#Region "Privateメソッド"
    ''' <summary>
    ''' セッション情報をパラメータデータテーブルに設定します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetSessionValue()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' パラメータ情報データテーブルにセッション情報をセット
        Me.dtParam = New SC3080401DataSet.SC3080401ParameterDataTable

        ' パラメータ情報データテーブル行の生成
        Dim drParam As SC3080401DataSet.SC3080401ParameterRow = Me.dtParam.NewSC3080401ParameterRow

        ' セッション情報の設定
        With drParam
            ' スタッフコンテキストから取得
            .DLRCD = StaffContext.Current.DlrCD
            .STRCD = StaffContext.Current.BrnCD
            .FLLWUPBOX_DLRCD = StaffContext.Current.DlrCD
            .FROMACCOUNT = StaffContext.Current.Account
            .FROMACCOUNTNAME = StaffContext.Current.UserName
            .LEADERFLG = StaffContext.Current.TeamLeader

            ' セッション情報から取得
            .CSTKIND = DirectCast(Me.GetValue(ScreenPos.Current, SessionKeyCstKind, False), String)
            .CUSTOMERCLASS = DirectCast(Me.GetValue(ScreenPos.Current, SessionKeyCustomerClass, False), String)
            .CRCUSTID = DirectCast(Me.GetValue(ScreenPos.Current, SessionKeyCrCustID, False), String)
            .FLLWUPBOX_STRCD = DirectCast(Me.GetValue(ScreenPos.Current, SessionKeyFllwupBoxStrCd, False), String)
            .FLLWUPBOX_SEQNO = Convert.ToInt64(DirectCast(Me.GetValue(ScreenPos.Current, SessionKeyFllwupBoxSeqNo, False), String), CultureInfo.InvariantCulture)
            .CUSTOMNAME = DirectCast(Me.GetValue(ScreenPos.Current, SessionKeyName, False), String)
            .SALESSTAFFCODE = DirectCast(Me.GetValue(ScreenPos.Current, SessionKeySalesStaffCode, False), String)
        End With

        ' パラメータ情報データテーブルの行追加
        Me.dtParam.AddSC3080401ParameterRow(drParam)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

    ''' <summary>
    ''' ヘルプ依頼画面を作成します。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/12 TCS 鈴木(健) HTMLエンコード対応
    ''' </History>
    Private Function CreateHelpRequestWindow() As String

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' ヘルプ依頼画面初期化
        Me.InitializeHelpRequest()

        ' ヘルプ依頼画面のデータセット生成
        Using ds As SC3080401DataSet = New SC3080401DataSet

            ' ヘルプ依頼画面ビジネスロジック
            Dim bizLogic As SC3080401BusinessLogic

            Try
                ' パラメータ情報をデータセットに追加
                ds.SC3080401Parameter.Merge(Me.dtParam)

                ' ビジネスロジックのインスタンス生成
                bizLogic = New SC3080401BusinessLogic

                ' 初期データの取得
                bizLogic.GetInitialData(ds)

                ' ヘルプ依頼中フラグの取得
                Me.IsRequesting = (ds.SC3080401GetHelpInfo.Count > 0)

                If Me.IsRequesting Then
                    ' ヘルプ依頼中の場合

                    ' ヘルプ依頼情報データテーブル行
                    Dim row As SC3080401DataSet.SC3080401GetHelpInfoRow = ds.SC3080401GetHelpInfo.Item(0)

                    ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
                    '' ラベルの設定
                    Me.RequestDate.Text = HttpUtility.HtmlEncode(DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, row.CREATEDATE, StaffContext.Current.DlrCD))      ' 依頼日時
                    Me.SelectedSendAccountName_Display.Text = HttpUtility.HtmlEncode(row.TOACCOUNTNAME)                                                                     ' 依頼先アカウント
                    Me.SelectedHelpName_Display.Text = HttpUtility.HtmlEncode(row.MSG_DLR)                                                                                  ' ヘルプ内容

                    ' Hidden値の設定
                    Me.HelpNo.Value = row.HELPNO.ToString(CultureInfo.InvariantCulture)                 ' ヘルプNo
                    ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END
                    Me.IsUnderHelpRequest.Value = Me.IsRequesting.ToString                              ' 依頼中フラグ（依頼中）
                    Me.SelectedSendAccount.Value = row.TOACCOUNT                                        ' 依頼先アカウント
                    Me.SelectedSendAccountName.Value = row.TOACCOUNTNAME                                ' 依頼先アカウント名
                    Me.SelectedHelpid.Value = row.ID                                                    ' ヘルプID
                    Me.SelectedHelpName.Value = row.MSG_DLR                                             ' ヘルプ内容
                    Me.NoticeReqId.Value = row.NOTICEREQID.ToString(CultureInfo.InvariantCulture)       ' 通知依頼ID

                    ' 各コントロールの制御
                    Me.SetPageControls()
                Else
                    ' ヘルプ依頼中でない場合

                    ' Hidden値の設定
                    Me.IsUnderHelpRequest.Value = Me.IsRequesting.ToString          ' 依頼中フラグ（未依頼）

                    ' 依頼先情報データテーブル
                    Dim sendAccountList As SC3080401DataSet.SC3080401GetSendAccountDataTable = ds.SC3080401GetSendAccount

                    ' 依頼先マネージャ存在フラグの取得
                    Me.IsManagerExists = (sendAccountList.Count > 0)

                    If Me.IsManagerExists Then
                        ' 依頼先が存在する場合

                        ' 依頼先情報の初期選択状態の設定
                        Me.SetAccountListSelectedItem(sendAccountList)

                        ' ヘルプ内容の初期選択状態の設定
                        Dim helpMstList As SC3080401DataSet.SC3080401GetHelpMstDataTable = ds.SC3080401GetHelpMst
                        Me.IsHelpMstExists = (helpMstList.Count > 0)
                        Me.SetHelpMstListSelectedItem(helpMstList)

                        ' 依頼先情報一覧リピータのデータ連結
                        Me.SendAccountRepeater.DataSource = sendAccountList
                        Me.SendAccountRepeater.DataBind()

                        ' ヘルプ内容一覧リピータのデータ連結
                        Me.HelpMstRepeater.DataSource = helpMstList
                        Me.HelpMstRepeater.DataBind()

                        ' 各コントロールの制御
                        Me.SetPageControls()
                    Else
                        ' 各コントロールの制御
                        Me.SetPageControls()
                    End If
                End If

                ' 上記で作成したヘルプ依頼画面のHTMLを返却する
                Using sw As New System.IO.StringWriter(CultureInfo.InvariantCulture)

                    Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
                    Me.RenderControl(writer)

                    ' ======================== ログ出力 開始 ========================
                    Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                              "{0}.ascx {1}_End, Return String[{2}]",
                                              SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, sw.GetStringBuilder().ToString))
                    ' ======================== ログ出力 終了 ========================

                    Return sw.GetStringBuilder().ToString
                End Using
            Finally
                bizLogic = Nothing
            End Try
        End Using

    End Function

    ''' <summary>
    ''' 依頼ボタン押下時の処理
    ''' </summary>
    ''' <param name="SendAccount">依頼先アカウント</param>
    ''' <param name="SendAccountName">依頼先アカウント名</param>
    ''' <param name="helpID">ヘルプID</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function RequestButtonClick(ByVal SendAccount As String, ByVal SendAccountName As String, ByVal helpID As String) As String

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, SendAccount:[{2}], SendAccountName:[{3}], helpID:[{4}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, SendAccount, SendAccountName, helpID))
        ' ======================== ログ出力 終了 ========================

        ' ヘルプ依頼画面ビジネスロジック
        Dim bizLogic As SC3080401BusinessLogic

        Try
            ' パラメータ情報データテーブル行
            Dim drParam As SC3080401DataSet.SC3080401ParameterRow = Me.dtParam.Item(0)

            ' パラメータ情報の設定
            With drParam
                ' 画面での選択内容から取得
                .ID = helpID
                .TOACCOUNT = SendAccount
                .TOACCOUNTNAME = SendAccountName
            End With

            ' ビジネスロジックのインスタンスを生成
            bizLogic = New SC3080401BusinessLogic

            ' ヘルプ依頼の登録
            Dim result As String = bizLogic.RegistHelpRequest(drParam).ToString(CultureInfo.InvariantCulture)

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}.ascx {1}_End, result:[{2}]",
                                      SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, result))
            ' ======================== ログ出力 終了 ========================

            ' 結果の返却
            Return result

        Finally
            bizLogic = Nothing
        End Try

    End Function

    ''' <summary>
    ''' キャンセルボタン押下時の処理
    ''' </summary>
    ''' <param name="SendAccount">依頼先アカウント</param>
    ''' <param name="SendAccountName">依頼先アカウント名</param>
    ''' <param name="helpID">ID</param>
    ''' <param name="helpNo">ヘルプNo</param>
    ''' <param name="noticeReqID">通知依頼ID</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function CancelButtonClick(ByVal SendAccount As String, ByVal SendAccountName As String, ByVal helpID As String, ByVal helpNo As Long, ByVal noticeReqID As Long) As String

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, SendAccount:[{2}], SendAccountName:[{3}], helpID:[{4}], helpNo:[{5}], noticeReqID:[{6}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, SendAccount, SendAccountName, helpID, helpNo, noticeReqID))
        ' ======================== ログ出力 終了 ========================

        ' ヘルプ依頼画面ビジネスロジック
        Dim bizLogic As SC3080401BusinessLogic

        Try
            ' パラメータ情報データテーブル行
            Dim drParam As SC3080401DataSet.SC3080401ParameterRow = Me.dtParam.Item(0)

            ' パラメータ情報の設定
            With drParam
                ' 画面での選択内容から取得
                .ID = helpID
                .TOACCOUNT = SendAccount
                .TOACCOUNTNAME = SendAccountName
                .HELPNO = helpNo
                .NOTICEREQID = noticeReqID
            End With

            ' ビジネスロジックのインスタンスを生成
            bizLogic = New SC3080401BusinessLogic

            ' ヘルプ依頼のキャンセル
            Dim result As String = bizLogic.CancelHelpRequest(drParam).ToString(CultureInfo.InvariantCulture)

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}.ascx {1}_End, result:[{2}]",
                                      SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, result))
            ' ======================== ログ出力 終了 ========================

            ' 結果の返却
            Return result

        Finally
            bizLogic = Nothing
        End Try

    End Function

    ''' <summary>
    ''' ヘルプ依頼画面を初期化します。
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/12 TCS 鈴木(健) HTMLエンコード対応
    ''' </History>
    Private Sub InitializeHelpRequest()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' 依頼先（選択済み）の初期化
        Me.SelectedSendAccountName_Display.Text = String.Empty
        Me.SelectedSendAccountName.Value = String.Empty
        Me.SelectedSendAccount.Value = String.Empty
        ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
        'Me.SelectedSendAccountOnlineStatus.Value = String.Empty
        ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END

        ' ヘルプ内容（選択済み）の初期化
        Me.SelectedHelpName_Display.Text = String.Empty
        Me.SelectedHelpName.Value = String.Empty
        Me.SelectedHelpid.Value = String.Empty

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

    ''' <summary>
    ''' コントロールに指定したCSSクラスを追加します。
    ''' </summary>
    ''' <param name="element">コントロールオブジェクト</param>
    ''' <param name="cssClass">CSSクラス名</param>
    ''' <remarks></remarks>
    Private Sub AddCssClass(ByVal element As HtmlGenericControl, ByVal cssClass As String)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, element:[{2}, cssClass:[{3}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, element.ToString, cssClass))
        ' ======================== ログ出力 終了 ========================

        If String.IsNullOrEmpty(element.Attributes("Class").Trim) Then
            element.Attributes("Class") = cssClass
        Else
            element.Attributes("Class") = element.Attributes("Class") & " " & cssClass
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

    ''' <summary>
    ''' 依頼先情報の初期選択状態を設定します。
    ''' </summary>
    ''' <param name="dt">依頼先情報データテーブル</param>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更
    '''                          （STEP2からの仕変によりBM不要化）
    '''  2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正
    '''  2012/04/12 TCS 鈴木(健) HTMLエンコード対応
    ''' </History>
    Private Sub SetAccountListSelectedItem(dt As SC3080401DataSet.SC3080401GetSendAccountDataTable)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, IsNothing(dt):[{2}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dt)))
        ' ======================== ログ出力 終了 ========================

        ' 依頼先情報が存在する場合
        If dt.Count > 0 Then

            ' 検索条件：在籍状態（大分類）がオフライン以外
            Dim filterExp As String = String.Format(CultureInfo.InvariantCulture, "PRESENCECATEGORY <> '{0}'", PresenceCategoryOffline)

            ' 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更 START
            '' 並び替え条件：権限がSCM⇒BM順、ユーザDISPLAY.表示順の昇順
            'Const sortExp As String = "OPERATIONCODE DESC, SORTNO"
            ' 並び替え条件：ユーザDISPLAY.表示順の昇順
            ' 2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正 START
            'Const sortExp As String = "SORTNO"
            Const sortExp As String = "ONLINESTATUS, SORTNO"
            ' 2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正 END
            ' 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更 END

            ' 依頼先情報データテーブル内を上記条件で検索
            Dim drc() As System.Data.DataRow = dt.Select(filterExp, sortExp)

            ' オフライン状態以外の依頼先が存在する場合
            If drc.Count > 0 Then

                ' 優先順位の最も高い依頼先情報を取得
                Dim dr As SC3080401DataSet.SC3080401GetSendAccountRow = DirectCast(drc(0), SC3080401DataSet.SC3080401GetSendAccountRow)

                ' ラベルの設定
                ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
                Me.SelectedSendAccountName_Display.Text = HttpUtility.HtmlEncode(dr.USERNAME)                   ' 依頼先アカウント
                ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END

                ' Hidden値の設定
                Me.SelectedSendAccount.Value = dr.ACCOUNT                               ' 依頼先アカウント
                Me.SelectedSendAccountName.Value = dr.USERNAME                          ' 依頼先アカウント名

                ' 対応可能マネージャー存在フラグの設定
                Me.IsSendAccountExists = True
            End If
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End, IsSendAccountExists:[{2}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, Me.IsSendAccountExists))
        ' ======================== ログ出力 終了 ========================

    End Sub

    ''' <summary>
    ''' ヘルプ内容の初期選択状態を設定します。
    ''' </summary>
    ''' <param name="dt">ヘルプマスタデータテーブル</param>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/04/12 TCS 鈴木(健) HTMLエンコード対応
    ''' </History>
    Private Sub SetHelpMstListSelectedItem(dt As SC3080401DataSet.SC3080401GetHelpMstDataTable)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, IsNothing(dt):[{2}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dt)))
        ' ======================== ログ出力 終了 ========================

        ' ヘルプ内容が存在する場合、先頭を初期値として選択状態にする
        If dt.Count > 0 Then
            ' ラベルの設定
            ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 START
            Me.SelectedHelpName_Display.Text = HttpUtility.HtmlEncode(dt(0).MSG_DLR)                                ' ヘルプ内容
            ' 2012/04/12 TCS 鈴木(健) HTMLエンコード対応 END

            ' Hidden値の設定
            Me.SelectedHelpid.Value = dt(0).ID.ToString(CultureInfo.InvariantCulture)       ' ヘルプID
            Me.SelectedHelpName.Value = dt(0).MSG_DLR                                       ' ヘルプ内容
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

    ''' <summary>
    ''' ページ内のコントロールを制御します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetPageControls()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        If Me.IsRequesting Then
            ' ヘルプ依頼中の場合

            ' 選択済み依頼先欄に依頼中のCSSクラスを追加
            Me.AddCssClass(Me.SelectedSendAccountNameArea, "helpRequestPopUpAuditButtonUnderRequest")

            ' 選択済みヘルプ内容欄に依頼中のCSSクラスを追加
            Me.AddCssClass(Me.SelectedHelpNameArea, "helpRequestPopUpAuditButtonUnderRequest")

            ' ヘルプ依頼メイン（依頼先不在時）を非表示
            Me.NoSendAccountArea.Visible = False

            ' キャンセルボタンの制御
            Me.IsCancelButtonEnabled.Value = Boolean.TrueString

            ' 依頼ボタンの非表示
            Me.RequestButton.Visible = False
        Else
            ' ヘルプ依頼中でない場合

            If Me.IsManagerExists Then
                ' 所属マネージャーが存在する場合

                ' ヘルプ依頼中エリアの非表示
                Me.UnderHelpRequestArea.Visible = False

                ' キャンセルボタンの非表示
                Me.CancelButton.Visible = False

                ' ヘルプ依頼メイン（依頼先不在時）を非表示
                Me.NoSendAccountArea.Visible = False

                ' 依頼ボタンの制御
                Me.SetRequestButtonControl()
            Else
                ' 所属マネージャーが存在しない場合

                ' ヘルプ依頼メインを非表示
                Me.UnderHelpRequestArea.Visible = False
                Me.SelectedSendAccountArea.Visible = False
                Me.SelectedHelpMstArea.Visible = False
                Me.RequestButton.Visible = False
                Me.CancelButton.Visible = False

                ' ヘルプ依頼メイン（依頼先不在時）を表示
                Me.NoSendAccountArea.Visible = True
            End If
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

    ''' <summary>
    ''' 依頼ボタンの活性状態を制御します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetRequestButtonControl()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' 依頼ボタンの活性状態
        Dim isRequestButtonEnabled As Boolean = False

        ' 在籍状態の取得
        Dim presenceCategory As String = StaffContext.Current.PresenceCategory      ' 在籍状態（大分類）
        Dim presenceDetail As String = StaffContext.Current.PresenceDetail          ' 在籍状態（小分類）

        ' 在籍状態が、スタンバイ（営業活動中）または商談中または納車作業中の場合
        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        If (presenceCategory.Equals(PresenceCategoryStandby) And presenceDetail.Equals(PresenceDetailOn)) Or
            (presenceCategory.Equals(PresenceCategoryNegotiation) And (presenceDetail.Equals(PresenceDetailOff) Or
                                                                       presenceDetail.Equals(PresenceDetail_Two))) Then
            '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End

            '対応可能なマネージャーおよびヘルプ内容マスタが存在する場合のみ活性
            If Me.IsSendAccountExists And Me.IsHelpMstExists Then
                isRequestButtonEnabled = True
            End If
        End If

        ' 依頼ボタンイメージのCSS初期化
        Me.RequestButton.Attributes("Class") = ""

        If isRequestButtonEnabled Then
            ' 活性の場合
            Me.AddCssClass(Me.RequestButton, "helpRequestPopUpUnderRequestButton")
        Else
            ' 非活性の場合
            Me.AddCssClass(Me.RequestButton, "helpRequestPopUpUnderRequestButtonDisabled")
        End If

        ' Hidden値の設定
        Me.IsRequestButtonEnabled.Value = isRequestButtonEnabled.ToString

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_End",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

    End Sub

#Region "ページクラス処理のバイパス処理"
    ''' <summary>
    ''' セッション情報から値を取得します。
    ''' </summary>
    ''' <param name="pos"></param>
    ''' <param name="key"></param>
    ''' <param name="removeFlg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetValue(pos As ScreenPos, key As String, removeFlg As Boolean) As Object

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_StartEnd, pos:[{2}], key:[{3}], removeFlg:[{4}], Return Object:[{5}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, pos.ToString, key, removeFlg.ToString,
                                  GetPageInterface().GetValueCommonBypass(pos, key, removeFlg).ToString))
        ' ======================== ログ出力 終了 ========================

        Return GetPageInterface().GetValueCommonBypass(pos, key, removeFlg)

    End Function

    ''' <summary>
    ''' 顧客詳細画面のインターフェイスを取得します。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPageInterface() As ICommonSessionControl

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_StartEnd, Return String:[{2}]",
                                  SC3080401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, CType(Me.Page, ICommonSessionControl).ToString))
        ' ======================== ログ出力 終了 ========================

        Return CType(Me.Page, ICommonSessionControl)

    End Function
#End Region

#End Region

End Class
