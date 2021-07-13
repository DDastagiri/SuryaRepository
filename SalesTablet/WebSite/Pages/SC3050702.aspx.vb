'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3050702.aspx.vb
'─────────────────────────────────────
'機能： セールスポイント設定
'補足： 
'作成： 2012/11/23 TMEJ 三和
'更新： 
'─────────────────────────────────────

Option Strict On

Imports System.Globalization
Imports Toyota.eCRB.Common
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.TCVSettingUtility
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.SC3050702
Imports System.Web.Script.Serialization

Partial Class Pages_SC3050702
    Inherits BasePage

#Region " セッションキー "

    ''' <summary>
    ''' TCV物理パスパラメータ名セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_TCV_PATH As String = "TcvPath"

    ''' <summary>
    ''' 履歴ファイル格納パスパラメータ名セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_TCV_SETTING_HISTORYFILE_PATH As String = "TcvSettingHistoryFilePath"

    ''' <summary>
    ''' 選択車種セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CAR_SERIES As String = "CarSeries"

    ''' <summary>
    ''' 選択外装/内装セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_EX_IN_DIVISION As String = "ExInDivision"

    ''' <summary>
    ''' 選択セールスポイントIDセッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SALES_POINT_ID As String = "SalesPointId"

#End Region

#Region " 定数 "

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM As String = "SC3050702"

    ''' <summary>
    ''' 画面ID:セールスポイント詳細設定(SC3050703)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_SALES_POINT_DETAIL As String = "SC3050703"

    ''' <summary>
    ''' TCV物理パスパラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_PATH As String = "TCV_PATH"

    ''' <summary>
    ''' 履歴ファイル格納パスパラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_SETTING_HISTORYFILE_PATH As String = "TCV_SETTING_HISTORYFILE_PATH"

    ''' <summary>
    ''' カーラインナップ情報ファイル項目[series]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CAR_LINEUP_JSON_SERIES As String = "series"

    ''' <summary>
    ''' カーラインナップ情報ファイル項目[name]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CAR_LINEUP_JSON_NAME As String = "name"

    ''' <summary>
    ''' メッセージID[7]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_7 As Integer = 7

    ''' <summary>
    ''' メッセージID[10]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_9 As Integer = 9

    ''' <summary>
    ''' メッセージID[10]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_10 As Integer = 10

    ''' <summary>
    ''' メッセージID[11]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_11 As Integer = 11

    ''' <summary>
    ''' メッセージID[901]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_901 As Integer = 901

    ''' <summary>
    ''' メッセージID[902]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_902 As Integer = 902

    ''' <summary>
    ''' タイプ エクステリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TYPE_EXTERIOR As String = "exterior"

    ''' <summary>
    ''' タイプ インテリア
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TYPE_INTERIOR As String = "interior"

    ''' <summary>
    ''' 表示順初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INIT_SORTNO As String = "0"

    ''' <summary>
    ''' 履歴ファイル操作区分[UPDATE]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SOUSA_KUBUN_UPDATE As String = "UPDATE"

    ''' <summary>
    ''' 日付フォーマット変換ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONV_ID_15 As Integer = 15

    ''' <summary>
    ''' 画面変更区分初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INIT_DISP_DVS_NO As String = "0"

    ''' <summary>
    ''' セールスポイント最大件数
    ''' </summary>
    ''' <remarks>縦マス数*横マス数*アングル数</remarks>
    Private Const MAX_COUNT As Integer = 999

    ''' <summary>
    ''' 一覧表示件数０件
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const RowCountNone As String = "0"

#End Region

#Region " ページロード "
    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("SC3050702(aspx) Page_Load", True))
        'ログ出力 End *****************************************************************************

        ' ポストバック判定
        If Not Page.IsPostBack Then

            '変更フラグ初期化
            Me.modifyDvsField.Value = INIT_DISP_DVS_NO

            'セールスポイント登録最大数をセット
            Me.maxCountField.Value = CStr(MAX_COUNT)

            'システム環境設定取得処理
            InitGetSystemEnvSetting()

            '画面文言取得
            GetWord()

            Dim tcvPath As String = _
                DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                SEARCH_KEY_TCV_PATH, False), String)

            '車種検索コンボボックスの設定
            InitSearchCarLineup(tcvPath)

            Dim exteriorInteriorDivision As String = String.Empty
            Dim carSeries As String = String.Empty

            '外装/内装セッションキーが存在しなければ初期値設定
            If ContainsKey(ScreenPos.Current, SESSION_KEY_EX_IN_DIVISION) Then
                '外装/内装パラメータ取得
                exteriorInteriorDivision = _
                    DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                    SESSION_KEY_EX_IN_DIVISION, False), String)
            Else
                exteriorInteriorDivision = TYPE_EXTERIOR
            End If

            '外装/内装を保持
            Me.exInField.Value = exteriorInteriorDivision

            '車種セッションキーが存在しなければ初期値設定
            If ContainsKey(ScreenPos.Current, SESSION_KEY_CAR_SERIES) Then
                '車種パラメータ取得
                carSeries = _
                    DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                    SESSION_KEY_CAR_SERIES, False), String)

                '車種パラメータを初期選択
                Me.DropDownList_Vehicle.SelectedValue = carSeries
            Else
                '車種が存在しなければ画面から取得
                carSeries = Me.DropDownList_Vehicle.SelectedValue
            End If

            '車種を保持
            Me.carSelectField.Value = carSeries

            'セールスポイント情報を取得
            GetSalesPointInfo(tcvPath, _
                                carSeries, _
                                exteriorInteriorDivision)

        End If

        'ヘッダー制御
        InitHeaderEvent()
        'フッター制御
        InitFooterEvent()

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("SC3050702(aspx) Page_Load", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " イベント "

#Region " 選択車種復元ダミーボタン押下処理 "

    ''' <summary>
    ''' 選択車種復元ダミーボタン押下処理
    ''' 選択車種変更時に編集内容を破棄しなかった場合に呼び出されます。
    ''' iOS6にてJavascriptでSelectを制御できないためAjaxで行います。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub RestoreButton_Click(sender As Object, e As System.EventArgs) Handles RestoreButton.Click

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("RestoreButton_Click", True))
        'ログ出力 End *****************************************************************************

        '選択車種を復元
        Me.DropDownList_Vehicle.SelectedValue = Me.carSelectField.Value

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("RestoreButton_Click", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " リフレッシュダミーボタン押下処理 "

    ''' <summary>
    ''' リフレッシュダミーボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub RefreshButton_Click(sender As Object, _
                                      e As System.EventArgs) Handles RefreshButton.Click

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("refreshButton_Click", True))
        'ログ出力 End *****************************************************************************

        '環境設定値を設定
        Dim tcvPath As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, SEARCH_KEY_TCV_PATH, False), String)

        '画面の車種選択値を設定
        Dim carSeries = Me.DropDownList_Vehicle.SelectedValue
        '画面の外装/内装選択値を設定
        Dim exteriorInteriorDivision As String = Me.exInField.Value

        'セールスポイント情報を取得
        GetSalesPointInfo(tcvPath, carSeries, exteriorInteriorDivision)

        '変更フラグ初期化
        Me.modifyDvsField.Value = INIT_DISP_DVS_NO

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("refreshButton_Click", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " 追加ダミーボタン押下処理 "

    ''' <summary>
    ''' 追加ダミーボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub AddButton_Click(sender As Object, e As System.EventArgs) Handles AddButton.Click

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("addButton_Click", True))
        'ログ出力 End *****************************************************************************

        '次画面遷移パラメータ
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CAR_SERIES, Me.DropDownList_Vehicle.SelectedValue)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_EX_IN_DIVISION, Me.exInField.Value)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_SALES_POINT_ID, String.Empty)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CAR_SERIES, Me.DropDownList_Vehicle.SelectedValue)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_EX_IN_DIVISION, Me.exInField.Value)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SALES_POINT_ID, String.Empty)

        'セールスポイント詳細画面へ遷移
        Me.RedirectNextScreen(APPLICATIONID_SALES_POINT_DETAIL)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("addButton_Click", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " 保存ダミーボタン押下処理 "

    ''' <summary>
    ''' 保存ダミーボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub SendButton_Click(sender As Object, _
                                   e As System.EventArgs) Handles SendButton.Click

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("sendButton_Click", True))
        'ログ出力 End *****************************************************************************

        '環境設定値を設定
        Dim tcvPath As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, SEARCH_KEY_TCV_PATH, False), String)

        '画面の車種選択値を設定
        Dim carSeries = Me.DropDownList_Vehicle.SelectedValue
        '画面の外装/内装選択値を設定
        Dim exteriorInteriorDivision As String = Me.exInField.Value

        'セールスポイント情報を更新
        Dim msgID As String = UpdateSalesPointInfo(tcvPath, _
                                                     carSeries, _
                                                     exteriorInteriorDivision)

        'エラーNo.が存在する場合、メッセージを表示して処理終了
        If Not String.IsNullOrEmpty(msgID) Then
            'ログ出力 Start *******************************************************************
            Logger.Warn(TcvSettingUtilityBusinessLogic.GetLogWarn( _
                        WebWordUtility.GetWord(CDec(msgID))))
            'ログ出力 End *********************************************************************

            Me.ShowMessageBox(CInt(msgID))
            Return
        End If

        Dim tcvSettingHistoryFilePath As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, _
                            SEARCH_KEY_TCV_SETTING_HISTORYFILE_PATH, False), String)

        'StaffContextからアカウントを取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim account As String = staffInfo.Account

        '現在日時取得
        Dim nowDate As Date = DateTimeFunc.Now
        Dim nowFormatDrate As String = DateTimeFunc.FormatDate(CONV_ID_15, nowDate)

        '履歴ファイル作成処理呼び出し
        CallCreateTcvArchiveFile(tcvSettingHistoryFilePath, _
                                 tcvPath, _
                                 carSeries, _
                                 nowFormatDrate, _
                                 account)

        '変更フラグ初期化
        Me.modifyDvsField.Value = INIT_DISP_DVS_NO

        'セールスポイント情報を取得
        msgID = GetSalesPointInfo(tcvPath, _
                                    carSeries, _
                                    exteriorInteriorDivision)

        'エラーNo.が存在する場合、メッセージを表示して処理終了
        If Not String.IsNullOrEmpty(msgID) Then
            'ログ出力 Start *******************************************************************
            Logger.Warn(TcvSettingUtilityBusinessLogic.GetLogWarn( _
                        WebWordUtility.GetWord(CDec(msgID))))
            'ログ出力 End *********************************************************************

            Me.ShowMessageBox(CInt(msgID))
            Return
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("sendButton_Click", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " 編集ダミーボタン押下処理 "

    ''' <summary>
    ''' 編集ダミーボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub EditButton_Click(sender As Object, e As System.EventArgs) Handles EditButton.Click

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("editButton_Click", True))
        'ログ出力 End *****************************************************************************

        '次画面遷移パラメータ
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CAR_SERIES, Me.DropDownList_Vehicle.SelectedValue)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_EX_IN_DIVISION, Me.exInField.Value)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_SALES_POINT_ID, Me.salesPointIdField.Value)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_CAR_SERIES, Me.DropDownList_Vehicle.SelectedValue)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_EX_IN_DIVISION, Me.exInField.Value)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SALES_POINT_ID, Me.salesPointIdField.Value)

        'セールスポイント詳細画面へ遷移
        Me.RedirectNextScreen(APPLICATIONID_SALES_POINT_DETAIL)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("editButton_Click", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " フレンダリング前最終イベント "

    ''' <summary>
    ''' フレンダリング前最終イベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        'TCV設定ボタンを表示
        If CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCVSetting) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCVSetting).Visible = True

        End If

    End Sub

#End Region

#End Region

#Region " 内部メソッド "

#Region " カーラインナップ取得処理 "
    ''' <summary>
    ''' カーラインナップ情報取得処理
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <remarks></remarks>
    Private Sub InitSearchCarLineup(ByVal tcvPath As String)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchCarLineup", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        'ログ出力 End *****************************************************************************

        Dim carSelectList As CarLineupCarSelectListJson = Nothing
        carSelectList = TcvSettingUtilityBusinessLogic.GetCarLineup(tcvPath)

        Me.DropDownList_Vehicle.DataSource = carSelectList.carselect.carList
        Me.DropDownList_Vehicle.DataValueField = CAR_LINEUP_JSON_SERIES
        Me.DropDownList_Vehicle.DataTextField = CAR_LINEUP_JSON_NAME
        Me.DropDownList_Vehicle.DataBind()

        '初期値選択
        Me.DropDownList_Vehicle.SelectedValue = carSelectList.carselect.defaultCarSeries

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchCarLineup", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " セールスポイント情報取得処理 "
    ''' <summary>
    ''' セールスポイント情報取得処理
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carSeries">車種Series</param>
    ''' <param name="exInDvs">エクステリア/インテリア</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function GetSalesPointInfo(ByVal tcvPath As String, _
                                       ByVal carSeries As String, _
                                       ByVal exInDvs As String) As String

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchCarLineup", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("exInDvs", exInDvs, True))
        'ログ出力 End *****************************************************************************

        Dim msgID As String = String.Empty
        Dim salesPointList As SalesPointListJson = Nothing

        Dim bizLogic = New SC3050702BusinessLogic

        salesPointList = bizLogic.GetSalesPointInfo(tcvPath, _
                                                                  carSeries, _
                                                                  exInDvs)

        repeaterSalesPointInfo.DataSource = salesPointList.sales_point
        repeaterSalesPointInfo.DataBind()

        Me.HiddenRowCount.Value = CStr(salesPointList.sales_point.Count)

        'JSON形式に変換
        Dim jss As JavaScriptSerializer = _
            New JavaScriptSerializer(New Script.Serialization.SimpleTypeResolver)
        Dim jsonString As String = jss.Serialize(salesPointList)

        'JSONを保持
        Me.salesPointJsonField.Value = jsonString

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(msgID))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchCarLineup", False))
        'ログ出力 End *****************************************************************************

        Return msgID

    End Function

#End Region

#Region " システム環境設定取得処理 "
    ''' <summary>
    ''' システム環境設定取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitGetSystemEnvSetting()

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitGetSystemEnvSetting", True))
        'ログ出力 End *****************************************************************************

        Dim sysEnv As New SystemEnvSetting

        'TCV物理パス
        Dim TcvPath As String = _
            sysEnv.GetSystemEnvSetting(TCV_PATH).PARAMVALUE
        MyBase.SetValue(ScreenPos.Current, SEARCH_KEY_TCV_PATH, TcvPath)

        '履歴ファイル格納パス
        Dim TcvSettingHistoryFilePath As String = _
            sysEnv.GetSystemEnvSetting(TCV_SETTING_HISTORYFILE_PATH).PARAMVALUE
        MyBase.SetValue(ScreenPos.Current, _
                        SEARCH_KEY_TCV_SETTING_HISTORYFILE_PATH, TcvSettingHistoryFilePath)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitGetSystemEnvSetting", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " セールスポイント情報保存処理 "
    ''' <summary>
    ''' セールスポイント情報保存処理
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carSeries">車種Series</param>
    ''' <param name="exInDvs">エクステリア/インテリア</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function UpdateSalesPointInfo(ByVal tcvPath As String, _
                                          ByVal carSeries As String, _
                                          ByVal exInDvs As String) As String

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("UpdateSalesPointInfo", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("exInDvs", exInDvs, True))
        'ログ出力 End *****************************************************************************

        'JSON形式からデシリアライズ
        Dim jss As JavaScriptSerializer = _
            New JavaScriptSerializer(New Script.Serialization.SimpleTypeResolver)
        Dim salesPointList As SalesPointListJson = _
            jss.Deserialize(Of SalesPointListJson)(Me.salesPointJsonField.Value)

        Dim sortNo As String = String.Empty

        '入力された表示順を設定
        For i As Integer = 0 To repeaterSalesPointInfo.Items.Count - 1
            Dim serviceReception As Control = repeaterSalesPointInfo.Items(i)

            '表示順を取得
            sortNo = DirectCast(serviceReception.FindControl("sortNo"), HtmlInputControl).Value

            '未入力の場合、0を設定
            If String.IsNullOrEmpty(sortNo) Then
                sortNo = INIT_SORTNO
            End If

            'セールスポイントJSONクラスのソートNoに表示順を設定
            salesPointList.sales_point(i).sortNo = CInt(sortNo)

        Next

        Dim bizLogic = New SC3050702BusinessLogic

        Dim msgID As String = _
            bizLogic.UpdateSalesPointInfoSend(tcvPath, _
                                                            carSeries, _
                                                            exInDvs, _
                                                            salesPointList)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(msgID))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("UpdateSalesPointInfo", False))
        'ログ出力 End *****************************************************************************

        Return msgID

    End Function

#End Region

#Region " 履歴ファイル作成処理 "

    ''' <summary>
    ''' 履歴ファイル作成処理
    ''' </summary>
    ''' <param name="tcvSettingHistoryFilePath">履歴ファイル格納パス</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carSeries">車種シリーズ</param>
    ''' <param name="timeStamp">タイムスタンプ</param>
    ''' <param name="account">アカウント</param>
    ''' <remarks></remarks>
    Private Sub CallCreateTcvArchiveFile(ByVal tcvSettingHistoryFilePath As String, _
                                         ByVal tcvPath As String, _
                                         ByVal carSeries As String, _
                                         ByVal timeStamp As String, _
                                         ByVal account As String)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("CallCreateTcvArchiveFile", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("timeStamp", timeStamp, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("account", account, True))
        'ログ出力 End *****************************************************************************

        Dim salesPointJsonPath As String = TcvSettingConstants.SalesPointJsonPath

        Dim repFileRoot As New ReplicationFileRoot

        Dim repFileInfo As New ReplicationFileInfo

        repFileInfo.FileAccess = SOUSA_KUBUN_UPDATE
        repFileInfo.FilePath = salesPointJsonPath.Replace(JsonUtilCommon.ReplaceFileString, carSeries)

        repFileRoot.Root.Add(repFileInfo)

        TcvSettingUtilityBusinessLogic.CreateRepFile(tcvSettingHistoryFilePath, timeStamp, account, repFileRoot)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("CallCreateTcvArchiveFile", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " 画面文言取得処理 "

    ''' <summary>
    ''' 画面文言取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetWord()
        'ログ出力 Start ***************************************************************************
        Logger.Debug("GetWord Start")
        'ログ出力 End *****************************************************************************

        Me.modifyMessageField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MESSAGE_ID_9))
        Me.sortNoMessageField.Value = HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_901, {WebWordUtility.GetWord(MESSAGE_ID_7)}))
        Me.maxCountMessageField.Value = HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_902, {CStr(MAX_COUNT)}))
        Me.NoRequestMsg.Text = HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_10, {WebWordUtility.GetWord(MESSAGE_ID_11)}))

        'ログ出力 Start ***************************************************************************
        Logger.Debug("GetWord End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' 置換文字置き換え処理
    ''' </summary>
    ''' <param name="wordNo">表示メッセージ（文言No）</param>
    ''' <param name="wordParam">表示メッセージ（置換文字列）</param>
    ''' <remarks></remarks>
    Private Function ReplaceMessage(ByVal wordNo As Integer, ByVal ParamArray wordParam As String()) As String

        'ログ出力 Start ***************************************************************************
        Logger.Debug("ReplaceMessage Start")
        'ログ出力 End *****************************************************************************

        Dim word As String = WebWordUtility.GetWord(wordNo)
        If wordParam IsNot Nothing AndAlso wordParam.Length > 0 Then
            word = String.Format(CultureInfo.InvariantCulture, word, wordParam)
        End If

        Return word

        'ログ出力 Start ***************************************************************************
        Logger.Debug("ReplaceMessage End")
        'ログ出力 End *****************************************************************************

    End Function

#End Region

#End Region

#Region " フッター制御・ヘッダー制御 "

    'メニューのＩＤを定義
    Private Const MAIN_MENU As Integer = 100
    Private Const TCV_SETTING As Integer = 1300
    Private Const SUBMENU_CONTENTS_MENU As Integer = 1301
    Private Const SUBMENU_SALES_POINT As Integer = 1302

    ''' <summary>
    ''' フッター作成
    ''' </summary>
    ''' <param name="commonMaster"></param>
    ''' <param name="category"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function DeclareCommonMasterFooter(ByVal commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("DeclareCommonMasterFooter", True))
        'ログ出力 End *****************************************************************************

        category = FooterMenuCategory.TCVSetting

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("DeclareCommonMasterFooter", False))
        'ログ出力 End *****************************************************************************

        Return {SUBMENU_CONTENTS_MENU, SUBMENU_SALES_POINT}

    End Function

    ''' <summary>
    ''' コンテキストメニュー作成
    ''' </summary>
    ''' <param name="commonMaster">マスタページ</param>
    ''' <returns>表示内容</returns>
    ''' <remarks>コンテキストメニューの作成</remarks>
    Public Overrides Function DeclareCommonMasterContextMenu(ByVal commonMaster As CommonMasterPage) As Integer()
        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("DeclareCommonMasterContextMenu", True))
        'ログ出力 End *****************************************************************************

        Return New Integer() {CommonMasterContextMenuBuiltinMenuID.LogoutItem}

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("DeclareCommonMasterContextMenu", False))
        'ログ出力 End *****************************************************************************

    End Function

    ''' <summary>
    ''' ヘッダーボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitHeaderEvent()

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitHeaderEvent", True))
        'ログ出力 End *****************************************************************************

        '戻るボタンを活性()
        CType(Master, CommonMasterPage).IsRewindButtonEnabled = True

        '戻る・進む・ログアウト
        For Each buttonId In {HeaderButton.Rewind, HeaderButton.Forward, HeaderButton.Logout}
            '活動破棄チェックのクライアントサイドスクリプトを埋め込む
            CType(Me.Master, CommonMasterPage).GetHeaderButton(buttonId).OnClientClick = "return onChangeDisplayCheck();"
        Next

        'カスタマーサーチを非活性
        CType(Master, CommonMasterPage).SearchBox.Enabled = False

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitHeaderEvent", False))
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitFooterEvent()

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitFooterEvent", True))
        'ログ出力 End *****************************************************************************

        'ボタン非表示
        If CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCVSetting) IsNot Nothing Then
            'クライアントスクリプト埋め込み
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCVSetting).OnClientClick = "return onChangeDisplayCheck();"

            'TCV設定
            AddHandler CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCVSetting).Click, _
                Sub()
                    'オプション設定遷移
                    Me.RedirectNextScreen("SC3050704")
                End Sub

        End If
        If CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_CONTENTS_MENU) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_CONTENTS_MENU).Visible = True
            'クライアントスクリプト埋め込み
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_CONTENTS_MENU).OnClientClick = "return onChangeDisplayCheck();"

            'コンテンツメニュー
            AddHandler CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_CONTENTS_MENU).Click, _
                Sub()
                    'コンテンツメニュー遷移
                    Me.RedirectNextScreen("SC3050701")
                End Sub

        End If
        If CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_SALES_POINT) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_SALES_POINT).Visible = True
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_SALES_POINT).Enabled = False
            'クライアントスクリプト埋め込み
            CType(Me.Master, CommonMasterPage).GetFooterButton(SUBMENU_SALES_POINT).OnClientClick = "return false;"
        End If
        If CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu).Visible = False
            'クライアントスクリプト埋め込み
            CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).OnClientClick = "return onChangeDisplayCheck();"

            'メニュー
            AddHandler CType(Me.Master, CommonMasterPage).GetFooterButton(MAIN_MENU).Click, _
                Sub()
                    'メニューに遷移
                    Me.RedirectNextScreen("SC3010203")
                End Sub

        End If
        If CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer).Visible = False
        End If
        If CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ShowRoomStatus) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.ShowRoomStatus).Visible = False
        End If
        If CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV) IsNot Nothing Then
            CType(Me.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.TCV).Visible = False
        End If


        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitFooterEvent", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

End Class
