'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3050701.aspx.vb
'─────────────────────────────────────
'機能： コンテンツメニュー設定
'補足： 
'作成： 2012/12/18 TMEJ 宇野
'更新： 
'─────────────────────────────────────

Imports System.Globalization
Imports System.Reflection.MethodBase
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.SC3050701
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.TCVSettingUtility
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic


''' <summary>
''' SC3050701 コンテンツメニュー設定プレゼンテーション層
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3050701
    Inherits BasePage

#Region " 定数 "

#Region " 機能ID "

    ''' <summary>
    ''' 機能ID:メインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPID_MAIN_MENU As String = "SC3010203"

    ''' <summary>
    ''' 機能ID:コンテンツメニュー設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPID_CONTENTS_MENU_SETTING As String = "SC3050701"

    ''' <summary>
    ''' 機能ID:セールスポイント設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPID_SALES_POINT_SETTING As String = "SC3050702"

    ''' <summary>
    ''' 機能ID:セールスポイント詳細設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPID_SALES_POINT_SETTING_DETAIL As String = "SC3050703"

    ''' <summary>
    ''' 機能ID:MOP/DOP設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPID_MOP_DOP_SETTING As String = "SC3050704"

    ''' <summary>
    ''' 機能ID:MOP/DOP詳細設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPID_MOP_DOP_SETTING_DETAIL As String = "SC3050705"

    ''' <summary>
    ''' 機能ID:自機能
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPID_CURRENT As String = APPID_CONTENTS_MENU_SETTING

#End Region

#Region " フッターメニューID "

    ''' <summary>
    ''' フッターメニューID:コンテンツメニュー設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_CONTENTS_MENU As Integer = 1301

    ''' <summary>
    ''' フッターメニューIDID:セールスポイント設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUBMENU_SALES_POINT As Integer = 1302

#End Region

#Region " システム環境設定 "

    ''' <summary>
    ''' システム環境設定:TCV物理パス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENV_TCV_PATH As String = "TCV_PATH"

    ''' <summary>
    ''' システム環境設定:TCVURL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENV_TCV_URL As String = "TCV_URL"

    ''' <summary>
    ''' システム環境設定:履歴ファイル格納パス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENV_TCV_SETTING_HISTORYFILE_PATH As String = "TCV_SETTING_HISTORYFILE_PATH"

    ''' <summary>
    ''' システム環境設定:画像アップロードサイズ(最大サイズ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENV_TCV_IMAGE_MAX_FILE_SIZE As String = "TCV_MENU_IMAGE_MAX_FILE_SIZE"

#End Region

#Region " 文言 "

    ''' <summary>
    ''' 文言:コンテンツメニュー設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_TITLE As Integer = 1

    ''' <summary>
    ''' 文言:メニュー名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_MENU_NAME As Integer = 2

    ''' <summary>
    ''' 文言:アイコン画像
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_ICON_IMAGE As Integer = 3

    ''' <summary>
    ''' 文言:遷移先URL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_TRANSITION_URL As Integer = 4

    ''' <summary>
    ''' 文言:表示順
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_ORDER As Integer = 5

    ''' <summary>
    ''' 文言:削除
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_DELETE As Integer = 6

    ''' <summary>
    ''' 文言:削除(ボタン)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_DELETE_BUTTON As Integer = 7

    ''' <summary>
    ''' 文言:保存
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_SAVE As Integer = 8

    ''' <summary>
    ''' 文言:メッセージ
    ''' </summary>
    ''' <remarks>画面情報が破棄されますが、よろしいですか？</remarks>
    Private Const WORD_MSG_CONFIRM_DISCARD As Integer = 9

    ''' <summary>
    ''' 文言:メッセージ
    ''' </summary>
    ''' <remarks>{0}を削除します。よろしいですか？</remarks>
    Private Const WORD_MSG_CONFIRM_DELETE As Integer = 10

    ''' <summary>
    ''' 文言:行
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_ROW As Integer = 11

    ''' <summary>
    ''' 文言:画像
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_IMAGE As Integer = 12

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>画面に表示された情報が最新ではない可能性があります。画面に最新情報を表示します。</remarks>
    Private Const WORD_ERR_NOT_LATEST As Integer = 900

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>{0}は数値を入力して下さい。</remarks>
    Private Const WORD_ERR_INVALID_NUMERIC As Integer = 901

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>{0}の入力が不正です。</remarks>
    Private Const WORD_ERR_INVALID_VALUE As Integer = 902

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>アップロード{0}のファイルサイズが上限({1})を超えています。</remarks>
    Private Const WORD_ERR_OVER_FILE_SIZE As Integer = 903

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>選択できる{0}は、{1}のみです。</remarks>
    Private Const WORD_ERR_INVALID_SELECT As Integer = 904

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>{0}を入力して下さい。</remarks>
    Private Const WORD_ERR_REQUIRED As Integer = 905

#End Region

#Region " コード値 etc. "

    ''' <summary>
    ''' 車種選択リスト:値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CARLINEUP_VALUE As String = "series"

    ''' <summary>
    ''' 車種選択リスト:表示値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CARLINEUP_TEXT As String = "name"

    ''' <summary>
    ''' 画面状態:初期
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATE_INITIAL As String = "0"

    ''' <summary>
    ''' 画面状態:編集済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATE_EDITED As String = "1"

    ''' <summary>
    ''' 画面状態:不正な編集
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATE_INVALID As String = "2"

    ''' <summary>
    ''' 画面状態:最新でない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATE_NOT_LATEST As String = "3"

    ''' <summary>
    ''' 表示順:初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_DEFAULT As Integer = 0

    ''' <summary>
    ''' 表示順:指定なし(入力できる最大値より大きい値)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_UNSPECIFIED As Integer = 100

    ''' <summary>
    ''' ファイル種別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FILE_KIND As String = "jpg/png"

    ''' <summary>
    ''' ファイルサイズ単位
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FILE_SIZE_UNIT As String = "KB"

#End Region

#End Region

#Region " イベント "

    ''' <summary>
    ''' ページロードイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        If Not IsPostBack AndAlso Not IsCallback AndAlso Not Me.ScriptManager.IsInAsyncPostBack Then

            'カーラインナップ情報取得
            Dim carLineup As CarLineupCarSelectJson = GetCarLineup()

            'コンテンツメニュー情報取得
            Dim contentsMenuInfo As FooterListJson = GetContentsMenuInfo(carLineup.defaultCarSeries)

            'ヘッダー初期設定
            InitializeHeader()

            'フッター初期設定
            InitializeFooter()

            '文言設定
            SetWord()

            '車種選択リスト設定
            SetCarLineup(carLineup)

            'コンテンツメニュー情報設定
            SetContentsMenuInfo(contentsMenuInfo)

        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' ページロード後イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'フッター初期設定
        PostInitializeFooter()

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' 選択車種復元イベント
    ''' 選択車種変更時に編集内容を破棄しなかった場合に呼び出されます。
    ''' iOS6にてJavascriptでSelectを制御できないためAjaxで行います。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub RestoreButton_Click(sender As Object, e As System.EventArgs) Handles RestoreButton.Click

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '選択車種を復元
        Me.DropDownCarLineup.SelectedValue = Me.HiddenSeries.Value

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' 再描画イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub RefreshButton_Click(sender As Object, e As System.EventArgs) Handles RefreshButton.Click

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'コンテンツメニュー情報取得
        Dim contentsMenuInfo As FooterListJson = GetContentsMenuInfo(Me.DropDownCarLineup.SelectedValue)

        'コンテンツメニュー情報設定
        SetContentsMenuInfo(contentsMenuInfo)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' 入力チェックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>保存ボタン押下時にサーバサイドチェックを非同期で行うためのイベントです。</remarks>
    Protected Sub ValidationButton_Click(sender As Object, e As System.EventArgs) Handles ValidationButton.Click

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim rows As RepeaterItemCollection = Me.RepeaterList.Items

        Dim serializer As New JavaScriptSerializer(New SimpleTypeResolver)
        Dim contentsMenuInfo As FooterListJson = serializer.Deserialize(Of FooterListJson)(Me.HiddenFooterJson.Value)

        '一行ずつ処理
        For Each row As RepeaterItem In rows
            'メニュー名の禁則文字チェック
            Dim menuName As String = DirectCast(row.FindControl("SC3050701_Menu"), HtmlInputControl).Value
            If Not Validation.IsValidString(menuName) Then
                Me.HiddenState.Value = STATE_INVALID
                ShowMessageBox(WORD_ERR_INVALID_VALUE, HttpUtility.HtmlEncode(WebWordUtility.GetWord(WORD_MENU_NAME)))
                Exit For
            End If
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' 保存イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SaveButton_Click(sender As Object, e As System.EventArgs) Handles SaveButton.Click

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '更新情報を構築
        Dim updateInfo As FooterListJson = CreateUpdateList()

        '状態設定
        Me.HiddenState.Value = STATE_INITIAL

        '保存処理
        If UpdateContentsMenuInfo(updateInfo) = SC3050701BusinessLogic.ResultSucceed Then

            'コンテンツメニュー情報再取得
            Dim contentsMenuInfo As FooterListJson = GetContentsMenuInfo(Me.DropDownCarLineup.SelectedValue)

            'コンテンツメニュー情報再設定
            SetContentsMenuInfo(contentsMenuInfo)

        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' フッターボタンイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub FooterButton_Click(sender As Object, e As System.EventArgs) Handles FooterButton.Click

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '選択されたフッターの機能に遷移
        Dim appId As String = Me.HiddenAppId.Value
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("appId", appId, False))
        Me.RedirectNextScreen(appId)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

#End Region

#Region " パブリック メソッド "

    ''' <summary>
    ''' フッターを再定義します。
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">フッターカテゴリ</param>
    ''' <returns>サブメニュー</returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        category = FooterMenuCategory.TCVSetting
        Dim subMenus As Integer() = {SUBMENU_CONTENTS_MENU, SUBMENU_SALES_POINT}

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return subMenus

    End Function

    ''' <summary>
    ''' コンテキストメニューを再定義します。
    ''' </summary>
    ''' <param name="commonMaster">マスタページ</param>
    ''' <returns>表示するメニュー</returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterContextMenu(ByVal commonMaster As CommonMasterPage) As Integer()

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim contextMenu As Integer() = {CommonMasterContextMenuBuiltinMenuID.LogoutItem}

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return contextMenu

    End Function

#End Region

#Region " プロテクト メソッド "

    ''' <summary>
    ''' 文字列にパラメータをバインドします。
    ''' </summary>
    ''' <param name="value">文字列</param>
    ''' <param name="parameters">パラメータ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function BindParameters(ByVal value As String, ByVal parameters As Object()) As String
        Return String.Format(CultureInfo.InvariantCulture, value, parameters)
    End Function

    ''' <summary>
    ''' 表示順を画面表示用の書式に変換します。
    ''' データバインド時にaspxより呼び出します。
    ''' </summary>
    ''' <param name="order">変換前の値</param>
    ''' <returns>変換後の値</returns>
    ''' <remarks></remarks>
    Protected Function ToOrderForDisplay(ByVal order As Integer) As String
        If order = ORDER_DEFAULT Then
            Return String.Empty
        End If
        Return order.ToString(CultureInfo.InvariantCulture)
    End Function

#End Region

#Region " プライベート メソッド "

    ''' <summary>
    ''' カーラインナップ情報を取得します。
    ''' </summary>
    ''' <returns>カーラインナップ情報</returns>
    ''' <remarks></remarks>
    Private Function GetCarLineup() As CarLineupCarSelectJson

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'システム環境設定取得
        Dim sysEnvSetting As New SystemEnvSetting
        Dim tcvPath As String = sysEnvSetting.GetSystemEnvSetting(ENV_TCV_PATH).PARAMVALUE

        'カーラインナップ情報取得
        Dim carLineup As CarLineupCarSelectJson = TcvSettingUtilityBusinessLogic.GetCarLineup(tcvPath).carselect

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(carLineup.defaultCarSeries))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("carList", carLineup.carList.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        'カーラインナップ情報返却
        Return carLineup

    End Function

    ''' <summary>
    ''' カーラインナップ情報を設定します。
    ''' </summary>
    ''' <param name="carLineup">カーラインナップ情報</param>
    ''' <remarks></remarks>
    Private Sub SetCarLineup(ByVal carLineup As CarLineupCarSelectJson)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '車種選択リストにバインド
        Me.DropDownCarLineup.DataSource = carLineup.carList
        Me.DropDownCarLineup.DataValueField = CARLINEUP_VALUE
        Me.DropDownCarLineup.DataTextField = CARLINEUP_TEXT
        Me.DropDownCarLineup.DataBind()

        '選択値を設定
        Me.DropDownCarLineup.SelectedValue = carLineup.defaultCarSeries

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' コンテンツメニュー情報を取得します。
    ''' </summary>
    ''' <param name="carId">車両ID</param>
    ''' <returns>コンテンツメニュー情報</returns>
    ''' <remarks></remarks>
    Private Function GetContentsMenuInfo(ByVal carId As String) As FooterListJson

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, False))

        'システム環境設定取得
        Dim sysEnvSetting As New SystemEnvSetting
        Dim tcvPath As String = sysEnvSetting.GetSystemEnvSetting(ENV_TCV_PATH).PARAMVALUE
        Dim tcvURL As String = sysEnvSetting.GetSystemEnvSetting(ENV_TCV_URL).PARAMVALUE

        'コンテンツメニュー情報取得
        Dim bizLogic As New SC3050701BusinessLogic
        Dim contentsMenuInfo As FooterListJson = bizLogic.GetContentsMenuInfo(tcvPath, tcvURL, carId)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("footerMap", contentsMenuInfo.footerMap.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        'コンテンツメニュー情報返却
        Return contentsMenuInfo

    End Function

    ''' <summary>
    ''' コンテンツメニュー情報を設定します。
    ''' </summary>
    ''' <param name="contentsMenuInfo">コンテンツメニュー情報</param>
    ''' <remarks></remarks>
    Private Sub SetContentsMenuInfo(ByVal contentsMenuInfo As FooterListJson)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'コンテンツメニュー一覧にバインド
        Me.RepeaterList.DataSource = contentsMenuInfo.footerMap
        Me.RepeaterList.DataBind()

        'JSONを保持
        Dim sirealizer As New JavaScriptSerializer
        Me.HiddenFooterJson.Value = sirealizer.Serialize(contentsMenuInfo)

        '更新日時を保持
        Me.HiddenTimeStamp.Value = contentsMenuInfo.TimeStamp

        'Me.HiddenState.Value = STATE_INITIAL

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' 画面情報をもとに更新情報を構築します。
    ''' </summary>
    ''' <returns>更新情報</returns>
    ''' <remarks></remarks>
    Private Function CreateUpdateList() As FooterListJson

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '画面の一覧情報取得
        Dim rows As RepeaterItemCollection = Me.RepeaterList.Items

        Dim serializer As New JavaScriptSerializer(New SimpleTypeResolver)
        Dim contentsMenuInfo As FooterListJson = serializer.Deserialize(Of FooterListJson)(Me.HiddenFooterJson.Value)

        '一行ずつ処理
        Dim rowIndex As Integer = 0
        For Each row As RepeaterItem In rows

            Dim contentsMenu As FooterJson = contentsMenuInfo.footerMap(rowIndex)
            contentsMenu.id = DirectCast(row.FindControl("SC3050701_ID"), HtmlInputHidden).Value
            contentsMenu.name = DirectCast(row.FindControl("SC3050701_Menu"), HtmlInputControl).Value
            contentsMenu.url = DirectCast(row.FindControl("SC3050701_Url"), HtmlInputControl).Value

            Dim uploadFile As FileUpload = DirectCast(row.FindControl("SC3050701_File"), FileUpload)
            'アップロードするファイルの有無を判定
            If uploadFile.HasFile Then
                contentsMenu.PostedFile = uploadFile.PostedFile
                contentsMenu.IconNameNew = uploadFile.FileName
            Else
                contentsMenu.PostedFile = Nothing
                contentsMenu.IconNameNew = DirectCast(row.FindControl("SC3050701_IconNameNew"), HtmlInputControl).Value
            End If

            Dim order As Integer
            If Not Integer.TryParse(DirectCast(row.FindControl("SC3050701_Order"), HtmlInputControl).Value, order) Then
                order = ORDER_UNSPECIFIED
            End If
            contentsMenu.Order = order

            rowIndex += 1
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '更新情報を返却
        Return contentsMenuInfo

    End Function

    ''' <summary>
    ''' コンテンツメニュー情報を更新します。
    ''' </summary>
    ''' <param name="contentsMenuInfo">コンテンツメニュー情報</param>
    ''' <returns>正常時は0、異常時はエラーメッセージIDを返します。</returns>
    ''' <remarks></remarks>
    Private Function UpdateContentsMenuInfo(ByVal contentsMenuInfo As FooterListJson) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("footerMap", contentsMenuInfo.footerMap.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("TimeStamp", contentsMenuInfo.TimeStamp, False))

        'システム環境設定取得
        Dim sysEnvSetting As New SystemEnvSetting
        Dim tcvPath As String = sysEnvSetting.GetSystemEnvSetting(ENV_TCV_PATH).PARAMVALUE
        Dim updateListPath As String = sysEnvSetting.GetSystemEnvSetting(ENV_TCV_SETTING_HISTORYFILE_PATH).PARAMVALUE

        'コンテンツメニュー情報取得
        Dim bizLogic As New SC3050701BusinessLogic
        Dim resultId As Integer = bizLogic.UpdateContentsMenuInfo(
                                    contentsMenuInfo,
                                    tcvPath,
                                    Me.DropDownCarLineup.SelectedValue,
                                    StaffContext.Current.Account,
                                    updateListPath
                                )


        '結果表示
        If resultId <> SC3050701BusinessLogic.ResultSucceed Then
            '保存失敗
            Me.HiddenState.Value = STATE_NOT_LATEST
            ShowMessageBox(resultId)
            Logger.Warn("Exclusion error.")
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(resultId.ToString(CultureInfo.InvariantCulture)))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '処理結果を返却
        Return resultId

    End Function

    ''' <summary>
    ''' 文言を設定します。
    ''' </summary>
    ''' <remarks>動的に表示する文言のみを設定します。</remarks>
    Private Sub SetWord()

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '文言設定
        Dim sysEnvSetting As New SystemEnvSetting
        Dim maxFileSize As String = sysEnvSetting.GetSystemEnvSetting(ENV_TCV_IMAGE_MAX_FILE_SIZE).PARAMVALUE
        Dim row As String = WebWordUtility.GetWord(WORD_ROW)
        Dim image As String = WebWordUtility.GetWord(WORD_IMAGE)
        Dim menu As String = WebWordUtility.GetWord(WORD_MENU_NAME)
        Dim url As String = WebWordUtility.GetWord(WORD_TRANSITION_URL)
        Dim order As String = WebWordUtility.GetWord(WORD_ORDER)
        Dim confirmDelete As String = WebWordUtility.GetWord(WORD_MSG_CONFIRM_DELETE)
        Dim errRequired As String = WebWordUtility.GetWord(WORD_ERR_REQUIRED)
        Dim errInvalid As String = WebWordUtility.GetWord(WORD_ERR_INVALID_VALUE)
        Dim errFileSize As String = WebWordUtility.GetWord(WORD_ERR_OVER_FILE_SIZE)
        Dim errFileKind As String = WebWordUtility.GetWord(WORD_ERR_INVALID_SELECT)
        Dim errNumeric As String = WebWordUtility.GetWord(WORD_ERR_INVALID_NUMERIC)

        Me.HiddenMaxFileSize.Value = maxFileSize
        Me.HiddenMsgConfirmDiscard.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(WORD_MSG_CONFIRM_DISCARD))
        Me.HiddenMsgConfirmDelete.Value = HttpUtility.HtmlEncode(BindParameters(confirmDelete, {row}))
        Me.HiddenErrRequiredMenu.Value = HttpUtility.HtmlEncode(BindParameters(errRequired, {menu}))
        Me.HiddenErrInvalidMenu.Value = HttpUtility.HtmlEncode(BindParameters(errInvalid, {menu}))
        Me.HiddenErrFileSize.Value = HttpUtility.HtmlEncode(BindParameters(errFileSize, {image, maxFileSize & FILE_SIZE_UNIT}))
        Me.HiddenErrFileKind.Value = HttpUtility.HtmlEncode(BindParameters(errFileKind, {image, FILE_KIND}))
        Me.HiddenErrRequiredURL.Value = HttpUtility.HtmlEncode(BindParameters(errRequired, {url}))
        Me.HiddenErrInvalidURL.Value = HttpUtility.HtmlEncode(BindParameters(errInvalid, {url}))
        Me.HiddenErrNumericOrder.Value = HttpUtility.HtmlEncode(BindParameters(errNumeric, {order}))

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' ヘッダーの初期制御設定を行います。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeHeader()

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'マスターページ取得
        Dim masterPage As CommonMasterPage = DirectCast(Me.Master, CommonMasterPage)

        '戻るボタン活性
        masterPage.IsRewindButtonEnabled = True

        'カスタマーサーチ非活性
        masterPage.SearchBox.Enabled = False

        'ヘッダーにイベントを関連付け
        For Each buttonId In {HeaderButton.Rewind, HeaderButton.Forward, HeaderButton.Logout}
            masterPage.GetHeaderButton(buttonId).OnClientClick = "return onHeaderHandler();"
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' フッターの初期制御設定を行います。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeFooter()

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'マスターページ取得
        Dim masterPage As CommonMasterPage = DirectCast(Me.Master, CommonMasterPage)

        '制御するフッターを取得
        Dim mainMenu As CommonMasterFooterButton = masterPage.GetFooterButton(FooterMenuCategory.MainMenu)
        Dim contentsMenuSetting As CommonMasterFooterButton = masterPage.GetFooterButton(SUBMENU_CONTENTS_MENU)
        Dim salesPointSetting As CommonMasterFooterButton = masterPage.GetFooterButton(SUBMENU_SALES_POINT)

        'メインメニューの制御
        If Not IsNothing(mainMenu) Then
            mainMenu.Visible = False
        End If

        'コンテンツメニュー設定の制御
        If Not IsNothing(contentsMenuSetting) Then
            contentsMenuSetting.Visible = True
            contentsMenuSetting.Enabled = False
        End If

        'セールスポイント設定の制御
        If Not IsNothing(salesPointSetting) Then
            salesPointSetting.Visible = True
            salesPointSetting.OnClientClick = BindParameters("return onFooterHandler('{0}');", {APPID_SALES_POINT_SETTING})
        End If

        '表示しないフッターを定義
        Dim hideCategories As FooterMenuCategory() = {
            FooterMenuCategory.AddOperation,
            FooterMenuCategory.Customer,
            FooterMenuCategory.Examination,
            FooterMenuCategory.Explanation,
            FooterMenuCategory.None,
            FooterMenuCategory.Parts,
            FooterMenuCategory.RO,
            FooterMenuCategory.Schedule,
            FooterMenuCategory.ShowRoomStatus,
            FooterMenuCategory.SMB,
            FooterMenuCategory.TCV,
            FooterMenuCategory.TelDirectory
        }

        '表示しないフッターを設定
        For Each hideCategory As FooterMenuCategory In hideCategories
            Dim button As CommonMasterFooterButton = masterPage.GetFooterButton(hideCategory)
            If Not IsNothing(button) Then
                button.Visible = False
            End If
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' フッターの初期制御設定を行います。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PostInitializeFooter()

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'マスターページ取得
        Dim masterPage As CommonMasterPage = DirectCast(Me.Master, CommonMasterPage)

        'TCV設定ボタンの制御
        Dim tcvSetting As CommonMasterFooterButton = masterPage.GetFooterButton(FooterMenuCategory.TCVSetting)
        If Not IsNothing(tcvSetting) Then
            tcvSetting.Visible = True
            tcvSetting.OnClientClick = BindParameters("return onFooterHandler('{0}');", {APPID_MOP_DOP_SETTING})
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

#End Region

End Class
