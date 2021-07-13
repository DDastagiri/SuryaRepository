'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3050703.aspx.vb
'─────────────────────────────────────
'機能： セールスポイント詳細設定
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
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.SC3050703
Imports System.Web.Script.Serialization
Imports System.IO

Partial Class Pages_SC3050703
    Inherits BasePage

#Region " セッションキー "

    ''' <summary>
    ''' TCV物理パスパラメータ名セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_TCV_PATH As String = "TcvPath"

    ''' <summary>
    ''' TCV_URLパスパラメータ名セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_TCV_URL As String = "TcvUrl"

    ''' <summary>
    ''' 履歴ファイル格納パスパラメータ名セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_TCV_SETTING_HISTORYFILE_PATH As String = "TcvSettingHistoryFilePath"

    ''' <summary>
    ''' 一覧説明省略桁数パラメータ名セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_SALESPOINT_CONTENTS_OMIT_LENGTH As String = "SalesPointContentsOmitLength"

    ''' <summary>
    ''' 概要画像アップロードサイズ(最大サイズ)パラメータ名セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_OVERVIEW_IMAGE_MAX_FILE_SIZE As String = "TcvOverViewImageMaxFileSize"

    ''' <summary>
    ''' 詳細画像アップロードサイズ(最大サイズ)パラメータ名セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_POPUP_IMAGE_MAX_FILE_SIZE As String = "TcvPopUpImageMaxFileSize"

    ''' <summary>
    ''' 詳細拡大画像アップロードサイズ(最大サイズ)パラメータ名セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_FULLSCREEN_POPUP_IMAGE_MAX_FILE_SIZE As String = "TcvFullPopUpImageMaxFileSize"

    ''' <summary>
    ''' 動画アップロードサイズ(最大サイズ)パラメータ名セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_POPUP_MOVIE_MAX_FILE_SIZE As String = "TcvMovieMaxFileSize"


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
    Private Const C_SYSTEM As String = "SC3050703"

    ''' <summary>
    ''' 画面ID:セールスポイント設定(SC3050702)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_SALES_POINT_RESULT As String = "SC3050702"

    ''' <summary>
    ''' TCV物理パスパラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_PATH As String = "TCV_PATH"

    ''' <summary>
    ''' TCV_URLパスパラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_URL As String = "TCV_URL"

    ''' <summary>
    ''' 履歴ファイル格納パスパラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_SETTING_HISTORYFILE_PATH As String = "TCV_SETTING_HISTORYFILE_PATH"

    ''' <summary>
    ''' 一覧説明省略桁数パラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SALESPOINT_CONTENTS_OMIT_LENGTH As String = "TCV_SALESPOINT_CONTENTS_OMIT_LENGTH"

    ''' <summary>
    ''' 概要画像アップロードサイズ(最大サイズ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_SALESPOINT_OVERVIEW_IMAGE_MAX_FILE_SIZE As String = "TCV_SALESPOINT_OVERVIEW_IMAGE_MAX_FILE_SIZE"

    ''' <summary>
    ''' 詳細画像アップロードサイズ(最大サイズ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_SALESPOINT_POPUP_IMAGE_MAX_FILE_SIZE As String = "TCV_SALESPOINT_POPUP_IMAGE_MAX_FILE_SIZE"

    ''' <summary>
    ''' 詳細拡大画像アップロードサイズ(最大サイズ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_SALESPOINT_FULLSCREEN_POPUP_IMAGE_MAX_FILE_SIZE As String = "TCV_SALESPOINT_FULLSCREEN-POPUP_IMAGE_MAX_FILE_SIZE"

    ''' <summary>
    ''' 動画アップロードサイズ(最大サイズ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_SALESPOINT_POPUP_MOVIE_MAX_FILE_SIZE As String = "TCV_SALESPOINT_POPUP_MOVIE_MAX_FILE_SIZE"


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
    ''' メッセージID[903]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_903 As Integer = 903

    ''' <summary>
    ''' メッセージID[904]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_904 As Integer = 904

    ''' <summary>
    ''' メッセージID[905]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_905 As Integer = 905

    ''' <summary>
    ''' メッセージID[906]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_906 As Integer = 906

    ''' <summary>
    ''' メッセージID[1]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_1 As Integer = 1

    ''' <summary>
    ''' メッセージID[2]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_2 As Integer = 2

    ''' <summary>
    ''' メッセージID[3]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_3 As Integer = 3

    ''' <summary>
    ''' メッセージID[4]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_4 As Integer = 4

    ''' <summary>
    ''' メッセージID[5]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_5 As Integer = 5

    ''' <summary>
    ''' メッセージID[6]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_6 As Integer = 6

    ''' <summary>
    ''' メッセージID[7]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_7 As Integer = 7

    ''' <summary>
    ''' メッセージID[8]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_8 As Integer = 8

    ''' <summary>
    ''' メッセージID[9]
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
    ''' メッセージID[12]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_12 As Integer = 12

    ''' <summary>
    ''' メッセージID[13]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_13 As Integer = 13

    ''' <summary>
    ''' メッセージID[14]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_14 As Integer = 14

    ''' <summary>
    ''' メッセージID[15]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_15 As Integer = 15

    ''' <summary>
    ''' メッセージID[16]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_16 As Integer = 16

    ''' <summary>
    ''' メッセージID[17]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_17 As Integer = 17

    ''' <summary>
    ''' メッセージID[18]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_18 As Integer = 18

    ''' <summary>
    ''' メッセージID[19]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_19 As Integer = 19

    ''' <summary>
    ''' メッセージID[20]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_ID_20 As Integer = 20

    ''' <summary>
    ''' 日付フォーマット変換ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONV_ID_15 As Integer = 15

    ''' <summary>
    ''' 外装ポイントテーブル列数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EX_TABLE_COLS As Integer = 13

    ''' <summary>
    ''' 内装ポイントテーブル横幅
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IN_TABLE_COLS As Integer = 18

    ''' <summary>
    ''' 外装ポイントテーブル行数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EX_TABLE_ROWS As Integer = 7

    ''' <summary>
    ''' 内装ポイントテーブル縦幅
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IN_TABLE_ROWS As Integer = 8

    ''' <summary>
    ''' ポイントテーブルグリッドサイズ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TABLE_GRID_SIZE As Integer = 49

    ''' <summary>
    ''' 内装セル幅
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IN_CELL_SIZE As Integer = 44

    ''' <summary>
    ''' 内装セル幅
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IN_CELL_WIDTH As Integer = 48

    ''' <summary>
    ''' 内装セル高さ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IN_CELL_HEIGHT As Integer = 44

    ''' <summary>
    ''' ポイントテーブルボーダーサイズ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TABLE_BORDER_WIDTH As Integer = 2

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
    ''' 画面変更区分初期値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INIT_DISP_DVS_NO As String = "0"

    ''' <summary>
    ''' タイプ[image]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VIEW_TYPE_IMAGE As String = "image"

    ''' <summary>
    ''' タイプ[movie]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VIEW_TYPE_MOVIE As String = "movie"

    ''' <summary>
    ''' タイプ[text]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VIEW_TYPE_TEXT As String = "text"

    ''' <summary>
    ''' Only追加文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TYPE_ONLY As String = "-only"

    ''' <summary>
    ''' ファイル名連結文字列[_salespoint_]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FILE_STRING_SALES_POINT As String = "_salespoint_"

    ''' <summary>
    ''' ファイル名連結文字列[/]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FILE_STRING_1 As String = "/"

    ''' <summary>
    ''' ファイル名連結文字列[..]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FILE_STRING_2 As String = ".."

    ''' <summary>
    ''' ファイル名連結文字列[..]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FILE_STRING_S As String = "_s"

    ''' <summary>
    ''' ファイル名連結文字列[..]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FILE_STRING_L As String = "_l"

    ''' <summary>
    ''' 文字列省略
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OMIT_STRING As String = "..."

    ''' <summary>
    ''' グレード適合ON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GRADE_ON As String = "1"

    ''' <summary>
    ''' グレード適合OFF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GRADE_OFF As String = "0"

    ''' <summary>
    ''' 削除区分ON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEL_DVS_ON As String = "1"

    ''' <summary>
    ''' 削除区分OFF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEL_DVS_OFF As String = "0"

    ''' <summary>
    ''' エラー区分ON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERROR_DVS_ON As String = "1"

    ''' <summary>
    ''' エラー区分OFF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERROR_DVS_OFF As String = "0"

    ''' <summary>
    ''' 置き換えタグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TAG_BR As String = "<br>"

    ''' <summary>
    ''' 置き換えタグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TAG_BR_BIG As String = "<BR>"

    ''' <summary>
    ''' 文字連結[メッセージ]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MESSAGE_BUILD As String = ":"

    ''' <summary>
    ''' 文字連結[拡張子]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_BUILD As String = "."

    ''' <summary>
    ''' 拡張子[jpg]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_JPG As String = "jpg"

    ''' <summary>
    ''' 拡張子[jpeg]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_JPEG As String = "jpeg"

    ''' <summary>
    ''' 拡張子[png]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_PNG As String = "png"

    ''' <summary>
    ''' 拡張子[mp4]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_MP4 As String = "mp4"

    ''' <summary>
    ''' 拡張子[mov]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_MOV As String = "mov"

    ''' <summary>
    ''' 拡張子[JPG]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_JPG_BIG As String = "JPG"

    ''' <summary>
    ''' 拡張子[JPEG]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_JPEG_BIG As String = "JPEG"

    ''' <summary>
    ''' 拡張子[PNG]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_PNG_BIG As String = "PNG"

    ''' <summary>
    ''' 拡張子[MP4]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_MP4_BIG As String = "MP4"

    ''' <summary>
    ''' 拡張子[MOV]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_MOV_BIG As String = "MOV"

    ''' <summary>
    ''' セルID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CELL_ID As String = "td"

    ''' <summary>
    ''' セルID文字埋め
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CELL_ID_STRING As String = "0"

    ''' <summary>
    ''' ファイルサイズ単位
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FILE_SIZE_UNIT As String = "KB"

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
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("SC3050703(aspx) Page_Load", True))
        'ログ出力 End *****************************************************************************

        ' ポストバック判定
        If Not Page.IsPostBack Then
            '変更フラグ初期化
            Me.modifyDvsField.Value = INIT_DISP_DVS_NO

            'システム環境設定取得処理
            InitGetSystemEnvSetting()

            '画面文言取得
            GetWord()

            '遷移元パラメータの取得
            Dim carSeries As String = _
                DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                SESSION_KEY_CAR_SERIES, False), String)
            Dim exInDvs As String = _
                DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                SESSION_KEY_EX_IN_DIVISION, False), String)
            Dim salesPointId As String = _
                DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                SESSION_KEY_SALES_POINT_ID, False), String)

            'セールスポイントセットテーブルの生成
            CreatePointTable(exInDvs)

            'パラメータを保持
            Me.carSelectField.Value = carSeries
            Me.exInField.Value = exInDvs
            Me.salesPointIdField.Value = salesPointId

            '新規の場合、削除ボタンを非活性
            If String.IsNullOrEmpty(Me.salesPointIdField.Value) Then
                Me.DelButtonLink.Visible = False
            End If

            Dim tcvPath As String = _
                DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                SEARCH_KEY_TCV_PATH, False), String)
            Dim tcvUrl As String = _
                DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                SEARCH_KEY_TCV_URL, False), String)

            'カーラインナップ情報の取得
            InitSearchCarLineup(tcvPath, carSeries)

            'グレード情報の取得
            InitSearchTcvWeb(tcvPath, carSeries)

            'セールスポイント情報を取得
            GetSalesPointInfo(tcvPath, _
                                tcvUrl, _
                                carSeries, _
                                exInDvs)

            '外装/内装判定
            If TYPE_EXTERIOR.Equals(exInDvs) Then
                '外装情報取得
                InitSearchExteriorImageInfo(tcvPath, _
                                            tcvUrl, _
                                            carSeries)

            Else
                '内装情報取得
                InitSearchInteriorImageInfo(tcvPath, _
                                            tcvUrl, _
                                            carSeries)

            End If

        End If

        'ヘッダー制御
        InitHeaderEvent()
        'フッター制御
        InitFooterEvent()

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("SC3050703(aspx) Page_Load", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " イベント "

#Region " フレンダリング前最終処理 "

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

#Region " 保存ダミーボタン押下処理 "

    ''' <summary>
    ''' 保存ダミーボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub SendButton_Click(sender As Object, e As System.EventArgs) Handles SendButton.Click

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("SendButton_Click", True))
        'ログ出力 End *****************************************************************************

        '環境設定値を設定
        Dim tcvPath As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, SEARCH_KEY_TCV_PATH, False), String)
        Dim SalesPointContentsOmitLength As Integer = CInt(MyBase.GetValue(ScreenPos.Current, _
                            SEARCH_KEY_SALESPOINT_CONTENTS_OMIT_LENGTH, False))

        '画面の車種を設定
        Dim carSeries = Me.carSelectField.Value
        '画面の外装/内装を設定
        Dim exteriorInteriorDivision As String = Me.exInField.Value
        '画面のセールスポイントIDを設定
        Dim targetSalesPointID As String = Me.targetID.Value

        'セールスポイント情報を更新
        Dim msgID As String = UpdateSalesPointInfo(tcvPath, _
                                                     carSeries, _
                                                     exteriorInteriorDivision, _
                                                     targetSalesPointID, _
                                                     SalesPointContentsOmitLength)

        Dim tcvUrl As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, _
                            SEARCH_KEY_TCV_URL, False), String)
        Dim exInDvs As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, _
                            SESSION_KEY_EX_IN_DIVISION, False), String)

        'セールスポイントセットテーブルの生成
        CreatePointTable(exInDvs)

        'エラーNo.が存在する場合、メッセージを表示して処理終了
        If Not String.IsNullOrEmpty(msgID) Then
            'ログ出力 Start *******************************************************************
            Logger.Warn(TcvSettingUtilityBusinessLogic.GetLogWarn( _
                        WebWordUtility.GetWord(CDec(msgID))))
            'ログ出力 End *********************************************************************

            Me.ShowMessageBox(CInt(msgID))
            Me.refleshDvsField.Value = ERROR_DVS_ON

            Return
        End If

        'セールスポイントIDを再セット
        MyBase.SetValue(ScreenPos.Current, SESSION_KEY_SALES_POINT_ID, targetSalesPointID)
        Me.salesPointIdField.Value = targetSalesPointID

        '変更フラグ初期化
        Me.modifyDvsField.Value = INIT_DISP_DVS_NO

        'グレード情報の取得
        InitSearchTcvWeb(tcvPath, carSeries)

        'セールスポイント情報を取得
        GetSalesPointInfo(tcvPath, _
                            tcvUrl, _
                            carSeries, _
                            exInDvs)

        '外装/内装判定
        If TYPE_EXTERIOR.Equals(exInDvs) Then
            '外装情報取得
            InitSearchExteriorImageInfo(tcvPath, _
                                        tcvUrl, _
                                        carSeries)

        Else
            '内装情報取得
            InitSearchInteriorImageInfo(tcvPath, _
                                        tcvUrl, _
                                        carSeries)

        End If

        '登録後は削除ボタンを活性化
        Me.DelButtonLink.Visible = True


        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("SendButton_Click", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " 削除ダミーボタン押下処理 "

    ''' <summary>
    ''' 削除ダミーボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub DeleteButton_Click(sender As Object, e As System.EventArgs) Handles DeleteButton.Click

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("DeleteButton_Click", True))
        'ログ出力 End *****************************************************************************

        '環境設定値を設定
        Dim tcvPath As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, SEARCH_KEY_TCV_PATH, False), String)
        Dim tcvUrl As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, SEARCH_KEY_TCV_URL, False), String)

        '画面の車種を設定
        Dim carSeries = Me.carSelectField.Value
        '画面の外装/内装を設定
        Dim exteriorInteriorDivision As String = Me.exInField.Value
        '画面のセールスポイントIDを設定
        Dim targetSalesPointID As String = Me.targetID.Value

        'セールスポイント情報を更新
        Dim msgID As String = DeleteSalesPointInfo(tcvPath, _
                                                     carSeries, _
                                                     exteriorInteriorDivision, _
                                                     targetSalesPointID)

        Dim exInDvs As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, _
                            SESSION_KEY_EX_IN_DIVISION, False), String)

        'セールスポイントセットテーブルの生成
        CreatePointTable(exInDvs)

        'エラーNo.が存在する場合、メッセージを表示して処理終了
        If Not String.IsNullOrEmpty(msgID) Then
            'ログ出力 Start *******************************************************************
            Logger.Warn(TcvSettingUtilityBusinessLogic.GetLogWarn( _
                        WebWordUtility.GetWord(CDec(msgID))))
            'ログ出力 End *********************************************************************

            Me.ShowMessageBox(CInt(msgID))
            Me.refleshDvsField.Value = ERROR_DVS_ON
            Return
        End If

        '削除完了後、遷移元画面へ戻る
        Me.RedirectPrevScreen()

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("DeleteButton_Click", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " 入力チェックボタン押下処理 "

    ''' <summary>
    ''' 入力チェックボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub CheckButton_Click(sender As Object, e As System.EventArgs) Handles CheckButton.Click

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("CheckButton_Click", True))
        'ログ出力 End *****************************************************************************

        '入力チェック
        If ValidateSC3050703() Then
            Me.ajaxErrorField.Value = ERROR_DVS_OFF
        Else
            Me.ajaxErrorField.Value = ERROR_DVS_ON
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("CheckButton_Click", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " リフレッシュボタン押下処理 "

    ''' <summary>
    ''' リフレッシュボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub RefleshButton_Click(sender As Object, e As System.EventArgs) Handles RefleshButton.Click

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("RefleshButton_Click", True))
        'ログ出力 End *****************************************************************************

        '環境設定値を設定
        Dim tcvPath As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, SEARCH_KEY_TCV_PATH, False), String)
        Dim SalesPointContentsOmitLength As Integer = CInt(MyBase.GetValue(ScreenPos.Current, _
                            SEARCH_KEY_SALESPOINT_CONTENTS_OMIT_LENGTH, False))

        '画面の車種を設定
        Dim carSeries = Me.carSelectField.Value
        '画面の外装/内装を設定
        Dim exteriorInteriorDivision As String = Me.exInField.Value
        '画面のセールスポイントIDを設定
        Dim targetSalesPointID As String = Me.targetID.Value

        Dim tcvUrl As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, _
                            SEARCH_KEY_TCV_URL, False), String)
        Dim exInDvs As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, _
                            SESSION_KEY_EX_IN_DIVISION, False), String)

        'セールスポイントセットテーブルの生成
        CreatePointTable(exInDvs)

        '変更フラグ初期化
        Me.modifyDvsField.Value = INIT_DISP_DVS_NO

        'グレード情報の取得
        InitSearchTcvWeb(tcvPath, carSeries)

        'セールスポイント情報を取得
        GetSalesPointInfo(tcvPath, _
                            tcvUrl, _
                            carSeries, _
                            exInDvs)

        '外装/内装判定
        If TYPE_EXTERIOR.Equals(exInDvs) Then
            '外装情報取得
            InitSearchExteriorImageInfo(tcvPath, _
                                        tcvUrl, _
                                        carSeries)

        Else
            '内装情報取得
            InitSearchInteriorImageInfo(tcvPath, _
                                        tcvUrl, _
                                        carSeries)

        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("RefleshButton_Click", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#End Region

#Region " 内部メソッド "

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

        'TCV_URLパス
        Dim TcvUrl As String = _
            sysEnv.GetSystemEnvSetting(TCV_URL).PARAMVALUE
        MyBase.SetValue(ScreenPos.Current, SEARCH_KEY_TCV_URL, TcvUrl)

        '履歴ファイル格納パス
        Dim TcvSettingHistoryFilePath As String = _
            sysEnv.GetSystemEnvSetting(TCV_SETTING_HISTORYFILE_PATH).PARAMVALUE
        MyBase.SetValue(ScreenPos.Current, _
                        SEARCH_KEY_TCV_SETTING_HISTORYFILE_PATH, TcvSettingHistoryFilePath)

        '一覧説明省略桁数
        Dim SalesPointContentsOmitLength As String = _
            sysEnv.GetSystemEnvSetting(SALESPOINT_CONTENTS_OMIT_LENGTH).PARAMVALUE
        MyBase.SetValue(ScreenPos.Current, _
                        SEARCH_KEY_SALESPOINT_CONTENTS_OMIT_LENGTH, SalesPointContentsOmitLength)

        '概要画像アップロードサイズ(最大サイズ)
        Dim SalesPointOverViewImageFileSize As String = _
            sysEnv.GetSystemEnvSetting(TCV_SALESPOINT_OVERVIEW_IMAGE_MAX_FILE_SIZE).PARAMVALUE
        MyBase.SetValue(ScreenPos.Current, _
                        SEARCH_KEY_OVERVIEW_IMAGE_MAX_FILE_SIZE, SalesPointOverViewImageFileSize)
        Me.overViewImageMaxFileSizeField.Value = SalesPointOverViewImageFileSize

        '詳細画像アップロードサイズ(最大サイズ)
        Dim SalesPointPopUpImageFileSize As String = _
            sysEnv.GetSystemEnvSetting(TCV_SALESPOINT_POPUP_IMAGE_MAX_FILE_SIZE).PARAMVALUE
        MyBase.SetValue(ScreenPos.Current, _
                        SEARCH_KEY_POPUP_IMAGE_MAX_FILE_SIZE, SalesPointPopUpImageFileSize)
        Me.popUpImageMaxFileSizeField.Value = SalesPointPopUpImageFileSize

        '詳細拡大画像アップロードサイズ(最大サイズ)
        Dim SalesPointFullPopUpImageFileSize As String = _
            sysEnv.GetSystemEnvSetting(TCV_SALESPOINT_FULLSCREEN_POPUP_IMAGE_MAX_FILE_SIZE).PARAMVALUE
        MyBase.SetValue(ScreenPos.Current, _
                        SEARCH_KEY_FULLSCREEN_POPUP_IMAGE_MAX_FILE_SIZE, SalesPointFullPopUpImageFileSize)
        Me.fullPopUpImageMaxFileSizeField.Value = SalesPointFullPopUpImageFileSize

        '動画アップロードサイズ(最大サイズ)
        Dim SalesPointMovieFileSize As String = _
            sysEnv.GetSystemEnvSetting(TCV_SALESPOINT_POPUP_MOVIE_MAX_FILE_SIZE).PARAMVALUE
        MyBase.SetValue(ScreenPos.Current, _
                        SEARCH_KEY_POPUP_MOVIE_MAX_FILE_SIZE, SalesPointMovieFileSize)
        Me.movieMaxFileSizeField.Value = SalesPointMovieFileSize

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitGetSystemEnvSetting", False))
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

        'ポイント座標必須エラーメッセージ
        Me.pointMessageField.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MESSAGE_ID_902))

        'セールスポイント必須エラーメッセージ
        Me.salesPointMessageField.Value = HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_904, {WebWordUtility.GetWord(MESSAGE_ID_3)}))

        'グレード必須エラーメッセージ
        Me.greadMessageField.Value = HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_903, {WebWordUtility.GetWord(MESSAGE_ID_2)}))

        '概要ファイル拡張子エラーメッセージ
        Me.summaryMessageField.Value = _
            HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_905, _
                                                  {WebWordUtility.GetWord(MESSAGE_ID_6), _
                                                  WebWordUtility.GetWord(MESSAGE_ID_7), _
                                                  EXT_JPG & FILE_STRING_1 & EXT_PNG}))

        '詳細ファイル拡張子エラーメッセージ
        Me.detailMessageField.Value = _
            HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_905, _
                                                  {WebWordUtility.GetWord(MESSAGE_ID_9), _
                                                  WebWordUtility.GetWord(MESSAGE_ID_10), _
                                                  EXT_JPG & FILE_STRING_1 & EXT_PNG & _
                                                  FILE_STRING_1 & _
                                                  EXT_MP4 & FILE_STRING_1 & EXT_MOV}))

        '詳細(拡大)ファイル拡張子エラーメッセージ
        Me.detailPopupMessageField.Value = _
            HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_905, _
                                                  {WebWordUtility.GetWord(MESSAGE_ID_12), _
                                                  WebWordUtility.GetWord(MESSAGE_ID_13), _
                                                  EXT_JPG & FILE_STRING_1 & EXT_PNG}))

        Dim overViewImageMaxFileSize As String = DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                        SEARCH_KEY_OVERVIEW_IMAGE_MAX_FILE_SIZE, False), String)

        Dim popUpImageMaxFileSize As String = DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                        SEARCH_KEY_POPUP_IMAGE_MAX_FILE_SIZE, False), String)

        Dim fullPopUpImageMaxFileSize As String = DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                        SEARCH_KEY_FULLSCREEN_POPUP_IMAGE_MAX_FILE_SIZE, False), String)

        Dim movieMaxFileSize As String = DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                        SEARCH_KEY_POPUP_MOVIE_MAX_FILE_SIZE, False), String)

        '概要ファイルサイズエラーメッセージ
        Me.summaryFileSizeMessageField.Value = _
            HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_906, _
                                                  {WebWordUtility.GetWord(MESSAGE_ID_6), _
                                                  WebWordUtility.GetWord(MESSAGE_ID_7), _
                                                  overViewImageMaxFileSize & FILE_SIZE_UNIT}))

        '詳細ファイルサイズエラーメッセージ
        Me.detailFileSizeImageMessageField.Value = _
            HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_906, _
                                                  {WebWordUtility.GetWord(MESSAGE_ID_9), _
                                                  WebWordUtility.GetWord(MESSAGE_ID_7), _
                                                  popUpImageMaxFileSize & FILE_SIZE_UNIT}))
        Me.detailFileSizeMovieMessageField.Value = _
            HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_906, _
                                                  {WebWordUtility.GetWord(MESSAGE_ID_9), _
                                                  WebWordUtility.GetWord(MESSAGE_ID_20), _
                                                  movieMaxFileSize & FILE_SIZE_UNIT}))

        '詳細(拡大)ファイルサイズエラーメッセージ
        Me.detailPopupFileSizeMessageField.Value = _
            HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_906, _
                                                  {WebWordUtility.GetWord(MESSAGE_ID_12), _
                                                  WebWordUtility.GetWord(MESSAGE_ID_13), _
                                                  fullPopUpImageMaxFileSize & FILE_SIZE_UNIT}))

        '概要ファイル削除確認メッセージ
        Me.summaryAlertField.Value = _
            HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_18, _
                                                  {WebWordUtility.GetWord(MESSAGE_ID_6) & _
                                                  MESSAGE_BUILD & " " & _
                                                  WebWordUtility.GetWord(MESSAGE_ID_7)}))

        '詳細ファイル削除確認メッセージ
        Me.detailAlertField.Value = _
            HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_18, _
                                                  {WebWordUtility.GetWord(MESSAGE_ID_9) & _
                                                  MESSAGE_BUILD & " " & _
                                                  WebWordUtility.GetWord(MESSAGE_ID_10)}))

        '詳細(拡大画像)ファイル削除確認メッセージ
        Me.detailPopupAlertField.Value = _
            HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_18, _
                                                  {WebWordUtility.GetWord(MESSAGE_ID_12) & _
                                                  MESSAGE_BUILD & " " & _
                                                  WebWordUtility.GetWord(MESSAGE_ID_13)}))


        'セールスポイント情報削除確認メッセージ
        Me.deleteAlertField.Value = _
            HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_18, _
                                                  {WebWordUtility.GetWord(MESSAGE_ID_19)}))
        '画面変更確認メッセージ
        Me.modifyMessageField.Value = HttpUtility.HtmlEncode(ReplaceMessage(MESSAGE_ID_17))

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

#Region " カーラインナップ取得処理 "
    ''' <summary>
    ''' カーラインナップ情報取得処理
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carSeries">車種シリーズ</param>
    ''' <remarks></remarks>
    Private Sub InitSearchCarLineup(ByVal tcvPath As String, _
                                    ByVal carSeries As String)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchCarLineup", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        'ログ出力 End *****************************************************************************

        Dim carSelectList As CarLineupCarSelectListJson = Nothing
        carSelectList = TcvSettingUtilityBusinessLogic.GetCarLineup(tcvPath)

        '車種シリーズより車種名を取得
        For Each carSelectListData As CarLineupCarListJson In carSelectList.carselect.carList
            '対象車種の車種名を設定
            If carSeries.Equals(carSelectListData.series) Then
                Me.carName.Text = carSelectListData.name

            End If
        Next

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchCarLineup", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " グレード情報取得処理 "
    ''' <summary>
    ''' グレード情報取得処理
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carSeries">車種シリーズ</param>
    ''' <remarks></remarks>
    Private Sub InitSearchTcvWeb(ByVal tcvPath As String, _
                                    ByVal carSeries As String)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchTcvWeb", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        'ログ出力 End *****************************************************************************

        Dim tcvWebList As TcvWebListJson = Nothing
        tcvWebList = TcvSettingUtilityBusinessLogic.GetTcvWeb(tcvPath, carSeries)

        Me.repeaterGradeInfo.DataSource = tcvWebList.grade
        Me.repeaterGradeInfo.DataBind()

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchTcvWeb", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " 外装画像サムネイル情報取得処理 "
    ''' <summary>
    ''' 外装画像サムネイル情報取得処理
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="TcvUrl">TCV_URL</param>
    ''' <param name="carSeries">車種シリーズ</param>
    ''' <remarks></remarks>
    Private Sub InitSearchExteriorImageInfo(ByVal tcvPath As String, _
                                            ByVal tcvUrl As String, _
                                            ByVal carSeries As String)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchExteriorImageInfo",
                                                                True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvUrl", tcvUrl, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        'ログ出力 End *****************************************************************************

        Dim bizLogic = New SC3050703BusinessLogic

        Dim thumbnailInfoList As ThumbnailInfoList = Nothing
        thumbnailInfoList = bizLogic.GetExteriorImageInfo(tcvPath, _
                                                                    tcvUrl, _
                                                                    carSeries)

        Me.repeaterthumbnailInfo.DataSource = thumbnailInfoList.ThumbnailInfo
        Me.repeaterthumbnailInfo.DataBind()

        'デフォルトアングルを取得
        For Each thumbnailInfoListData As ThumbnailInfo In thumbnailInfoList.ThumbnailInfo
            '新規の場合のみ設定
            If String.IsNullOrEmpty(Me.salesPointIdField.Value) Then
                Me.angleField.Value = thumbnailInfoListData.Id
                Me.defaultGridPathField.Value = thumbnailInfoListData.GridPath
                Exit For
            Else
                '編集の場合、デフォルトアングルからグリッドパスを設定
                If Me.angleField.Value.Equals(thumbnailInfoListData.Id) Then
                    Me.defaultGridPathField.Value = thumbnailInfoListData.GridPath
                End If
            End If
        Next

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchExteriorImageInfo", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " 内装画像サムネイル情報取得処理 "
    ''' <summary>
    ''' 内装画像サムネイル情報取得処理
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="TcvUrl">TCV_URL</param>
    ''' <param name="carSeries">車種シリーズ</param>
    ''' <remarks></remarks>
    Private Sub InitSearchInteriorImageInfo(ByVal tcvPath As String, _
                                            ByVal tcvUrl As String, _
                                            ByVal carSeries As String)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchInteriorImageInfo",
                                                                True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvUrl", tcvUrl, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        'ログ出力 End *****************************************************************************

        Dim bizLogic = New SC3050703BusinessLogic

        Dim thumbnailInfoList As ThumbnailInfoList = Nothing
        thumbnailInfoList = bizLogic.GetInteriorImageInfo(tcvPath, _
                                                                    tcvUrl, _
                                                                    carSeries)

        Me.repeaterthumbnailInfo.DataSource = thumbnailInfoList.ThumbnailInfo
        Me.repeaterthumbnailInfo.DataBind()

        'デフォルトアングルを取得
        For Each thumbnailInfoListData As ThumbnailInfo In thumbnailInfoList.ThumbnailInfo
            '新規の場合のみ設定
            If String.IsNullOrEmpty(Me.salesPointIdField.Value) Then
                Me.angleField.Value = thumbnailInfoListData.Id
                Me.defaultGridPathField.Value = thumbnailInfoListData.GridPath
                Exit For
            Else
                '編集の場合、デフォルトアングルからグリッドパスを設定
                If Me.angleField.Value.Equals(thumbnailInfoListData.Id) Then
                    Me.defaultGridPathField.Value = thumbnailInfoListData.GridPath
                End If
            End If
        Next

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchInteriorImageInfo", _
                                                                False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#Region " セールスポイント情報取得処理 "
    ''' <summary>
    ''' セールスポイント情報取得処理
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="tcvUrl">TCV_URL</param>
    ''' <param name="carSeries">車種Series</param>
    ''' <param name="exInDvs">エクステリア/インテリア</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function GetSalesPointInfo(ByVal tcvPath As String, _
                                       ByVal tcvUrl As String, _
                                       ByVal carSeries As String, _
                                       ByVal exInDvs As String) As String

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("InitSearchCarLineup", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvUrl", TCV_URL, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("exInDvs", exInDvs, True))
        'ログ出力 End *****************************************************************************

        Dim bizLogic = New SC3050703BusinessLogic

        Dim msgID As String = String.Empty
        Dim salesPointList As SalesPointListJson = Nothing
        salesPointList = bizLogic.GetSalesPointInfo(tcvPath, _
                                                    carSeries, _
                                                    exInDvs, _
                                                    Me.salesPointIdField.Value, _
                                                    "")

        '新規/編集判定
        If String.IsNullOrEmpty(Me.salesPointIdField.Value) Then
            '新規の場合
            'セールスポイントID取得
            Me.targetID.Value = salesPointList.TargetId

            'セールスポイント番号取得
            Me.salesPointNoField.Value = salesPointList.TargetNo
        Else
            '編集の場合
            '対象セールスポイントの情報を取得
            For Each salesPointListData As SalesPointJson In salesPointList.sales_point
                '修正の場合のみ設定
                If salesPointListData.id.Equals(Me.salesPointIdField.Value) And _
                    Not String.IsNullOrEmpty(Me.salesPointIdField.Value) Then
                    If TYPE_EXTERIOR.Equals(Me.exInField.Value) Then
                        '外装の場合、アングル
                        Me.angleField.Value = salesPointListData.angle.Item(0)
                    Else
                        '外装の場合、内装ID
                        Me.angleField.Value = salesPointListData.interiorid.Item(0)
                    End If

                    'セールスポイントIDを取得
                    Me.targetID.Value = Me.salesPointIdField.Value
                    'セールスポイント番号取得
                    Me.salesPointNoField.Value = CStr(salesPointListData.SortNo)
                    'TOP座標取得
                    Me.topPointField.Value = salesPointListData.top.Item(0)
                    'LEFT座標取得
                    Me.leftPointField.Value = salesPointListData.left.Item(0)
                    'セールスポイント
                    Me.salesPointTxt.Value = salesPointListData.popuptitle
                    '説明
                    Me.contentsTxt.Value = _
                        salesPointListData.popupcontents.Replace(TAG_BR, vbCrLf).Replace(TAG_BR_BIG, vbCrLf)
                    '概要ファイル名
                    Me.overViewLink.HRef = "#" & salesPointListData.OverviewFile
                    Me.overViewFile.Text = salesPointListData.OverviewFile
                    Me.overViewFileNameField.Value = salesPointListData.OverviewFile
                    '詳細ファイル名
                    Me.popUpLink.HRef = "#" & salesPointListData.PopupFile
                    Me.popUpFile.Text = salesPointListData.PopupFile
                    Me.popUpFileNameField.Value = salesPointListData.PopupFile
                    '詳細(拡大画像)ファイル名
                    If (System.IO.Path.GetExtension(salesPointListData.FullscreenPopupFile).Equals(EXT_BUILD & EXT_MP4) _
                        Or System.IO.Path.GetExtension(salesPointListData.FullscreenPopupFile).Equals(EXT_BUILD & EXT_MP4_BIG) _
                        Or System.IO.Path.GetExtension(salesPointListData.FullscreenPopupFile).Equals(EXT_BUILD & EXT_MOV) _
                        Or System.IO.Path.GetExtension(salesPointListData.FullscreenPopupFile).Equals(EXT_BUILD & EXT_MOV_BIG)) Then
                    Else
                        '動画ファイル以外の場合、ファイル名をセットする
                        Me.fullPopUpLink.HRef = "#" & salesPointListData.FullscreenPopupFile
                        Me.fullPopUpFile.Text = salesPointListData.FullscreenPopupFile
                        Me.fullPopUpFileNameField.Value = salesPointListData.FullscreenPopupFile
                    End If

                    '概要ファイルパス
                    Dim overViewFilePath As New StringBuilder
                    overViewFilePath.Append(tcvUrl)
                    overViewFilePath.Append(carSeries)
                    overViewFilePath.Append(FILE_STRING_1)
                    overViewFilePath.Append(TcvSettingConstants.SalespointIntroductionPath)
                    overViewFilePath.Append(salesPointListData.overviewimg)
                    overViewFilePath.Replace(JsonUtilCommon.ReplaceFileString, carSeries)
                    Me.overViewFilePathField.Value = overViewFilePath.ToString
                    '詳細ファイルパス
                    Dim popUpFilePath As New StringBuilder
                    popUpFilePath.Append(tcvUrl)
                    popUpFilePath.Append(carSeries)
                    popUpFilePath.Append(salesPointListData.popupsrc.Replace(FILE_STRING_2, ""))
                    popUpFilePath.Replace(JsonUtilCommon.ReplaceFileString, carSeries)
                    Me.popUpFilePathField.Value = popUpFilePath.ToString
                    '詳細(拡大画像)ファイルパス
                    Dim fullPopUpFilePath As New StringBuilder
                    fullPopUpFilePath.Append(tcvUrl)
                    fullPopUpFilePath.Append(carSeries)
                    fullPopUpFilePath.Append(salesPointListData.fullscreenpopupsrc)
                    fullPopUpFilePath.Replace(JsonUtilCommon.ReplaceFileString, carSeries)
                    Me.fullPopUpFilePathField.Value = fullPopUpFilePath.ToString

                End If
            Next
        End If

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

#Region " セールスポイント情報保存処理 "
    ''' <summary>
    ''' セールスポイント情報保存処理
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carSeries">車種Series</param>
    ''' <param name="exInDvs">エクステリア/インテリア</param>
    ''' <param name="targetSalesPointID">対象セールスポイントID</param>
    ''' <param name="SalesPointContentsOmitLength">一覧説明省略桁数</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function UpdateSalesPointInfo(ByVal tcvPath As String, _
                                          ByVal carSeries As String, _
                                          ByVal exInDvs As String, _
                                          ByVal targetSalesPointID As String, _
                                          ByVal SalesPointContentsOmitLength As Integer) As String

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("UpdateSalesPointInfo", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("exInDvs", exInDvs, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("targetSalesPointID", targetSalesPointID, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("SalesPointContents1OmitLength", CStr(SalesPointContentsOmitLength), True))
        'ログ出力 End *****************************************************************************

        'JSON形式からデシリアライズ
        Dim jss As JavaScriptSerializer = _
            New JavaScriptSerializer(New Script.Serialization.SimpleTypeResolver)
        Dim salesPointList As SalesPointListJson = _
            jss.Deserialize(Of SalesPointListJson)(Me.salesPointJsonField.Value)

        '新規/編集の判定
        If String.IsNullOrEmpty(Me.salesPointIdField.Value) Then
            '新規の場合
            Dim salesPointInfoData As New SalesPointJson

            '新規情報を設定
            salesPointInfoData = UpdateSalesPointInfoNew(tcvPath, _
                                                         carSeries, _
                                                         exInDvs, _
                                                         targetSalesPointID, _
                                                         SalesPointContentsOmitLength)
            '編集した情報を追加
            salesPointList.sales_point.Add(salesPointInfoData)

        Else
            '編集情報を設定
            salesPointList = UpdateSalesPointInfoEdit(tcvPath, _
                                                         carSeries, _
                                                         exInDvs, _
                                                         targetSalesPointID, _
                                                         SalesPointContentsOmitLength, _
                                                         salesPointList)

        End If

        Dim bizLogic = New SC3050703BusinessLogic

        Dim msgID As String = _
            bizLogic.UpdateSalesPointInfoSend(tcvPath, _
                                              carSeries, _
                                              exInDvs, _
                                              targetSalesPointID, _
                                              salesPointList,
                                              Me.summaryFile.PostedFile, _
                                              Me.detailFile.PostedFile, _
                                              Me.detailPopupFile.PostedFile)

        If String.IsNullOrEmpty(msgID) Then
            Dim tcvSettingHistoryFilePath As String = _
                DirectCast(MyBase.GetValue(ScreenPos.Current, _
                                SEARCH_KEY_TCV_SETTING_HISTORYFILE_PATH, False), String)

            'StaffContextからアカウントを取得
            Dim staffInfo As StaffContext = StaffContext.Current
            Dim account As String = staffInfo.Account

            '現在日時取得
            Dim nowDate As Date = DateTimeFunc.Now
            Dim nowFormatDrate As String = DateTimeFunc.FormatDate(CONV_ID_15, nowDate)

            Dim summaryUploadFile As String = String.Empty
            Dim detailUploadFile As String = String.Empty
            Dim detailPopupUploadFile As String = String.Empty

            If Me.summaryFile.HasFile Then
                summaryUploadFile = Me.summaryFile.PostedFile.FileName
            End If

            If Me.detailFile.HasFile Then
                detailUploadFile = Me.detailFile.PostedFile.FileName
            End If

            If Me.detailPopupFile.HasFile Then
                detailPopupUploadFile = Me.detailPopupFile.FileName
            End If

            '履歴ファイル作成処理呼び出し
            bizLogic.CallCreateTcvArchiveFile(carSeries, _
                                        tcvPath, _
                                        tcvSettingHistoryFilePath, _
                                        targetSalesPointID, _
                                        salesPointList, _
                                        nowFormatDrate, _
                                        account, _
                                        DEL_DVS_OFF, _
                                        summaryUploadFile, _
                                        detailUploadFile, _
                                        detailPopupUploadFile, _
                                        Me.overViewFileNameField.Value, _
                                        Me.overViewFileNameField.Value, _
                                        Me.fullPopUpFileNameField.Value)

        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(msgID))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("UpdateSalesPointInfo", False))
        'ログ出力 End *****************************************************************************

        Return msgID

    End Function

    ''' <summary>
    ''' セールスポイント情報新規データ設定
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carSeries">車種Series</param>
    ''' <param name="exInDvs">エクステリア/インテリア</param>
    ''' <param name="targetSalesPointID">対象セールスポイントID</param>
    ''' <param name="SalesPointContentsOmitLength">一覧説明省略桁数</param>
    ''' <returns>セールスポイント情報新規データ</returns>
    ''' <remarks></remarks>
    Private Function UpdateSalesPointInfoNew(ByVal tcvPath As String, _
                                          ByVal carSeries As String, _
                                          ByVal exInDvs As String, _
                                          ByVal targetSalesPointID As String, _
                                          ByVal SalesPointContentsOmitLength As Integer) As SalesPointJson

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("UpdateSalesPointInfoNew", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("exInDvs", exInDvs, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("targetSalesPointID", targetSalesPointID, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("SalesPointContents1OmitLength", CStr(SalesPointContentsOmitLength), True))
        'ログ出力 End *****************************************************************************

        '新規の場合
        Dim salesPointInfoData As New SalesPointJson

        'ソートNo
        salesPointInfoData.SortNo = CInt(Me.salesPointNoField.Value)
        'No
        salesPointInfoData.No = Me.salesPointNoField.Value
        'ID
        salesPointInfoData.id = Me.targetID.Value
        '外装/内装
        salesPointInfoData.type = Me.exInField.Value
        'タイプ
        If Me.summaryFile.HasFile Or Me.detailFile.HasFile Or Me.detailPopupFile.HasFile Then
            If Me.detailFile.HasFile _
                And (System.IO.Path.GetExtension(Me.detailFile.PostedFile.FileName).Equals(EXT_BUILD & EXT_MP4) _
                Or System.IO.Path.GetExtension(Me.detailFile.PostedFile.FileName).Equals(EXT_BUILD & EXT_MP4_BIG) _
                Or System.IO.Path.GetExtension(Me.detailFile.PostedFile.FileName).Equals(EXT_BUILD & EXT_MOV) _
                Or System.IO.Path.GetExtension(Me.detailFile.PostedFile.FileName).Equals(EXT_BUILD & EXT_MOV_BIG)) Then
                salesPointInfoData.viewtype = VIEW_TYPE_MOVIE
            Else
                salesPointInfoData.viewtype = VIEW_TYPE_IMAGE
            End If

        Else
            salesPointInfoData.viewtype = VIEW_TYPE_TEXT
        End If

        '外装アングル
        If Me.exInField.Value = TYPE_EXTERIOR Then
            Dim angleList As New List(Of String)
            angleList.Add(Me.angleField.Value)
            salesPointInfoData.angle = angleList
        Else
            Dim angleList As New List(Of String)
            angleList.Add(String.Empty)
            salesPointInfoData.angle = angleList
        End If

        '内装画面ID
        If Me.exInField.Value = TYPE_INTERIOR Then
            Dim interioridList As New List(Of String)
            interioridList.Add(Me.angleField.Value)
            salesPointInfoData.interiorid = interioridList
        Else
            Dim interioridList As New List(Of String)
            interioridList.Add(String.Empty)
            salesPointInfoData.interiorid = interioridList
        End If

        'グレード適合
        Dim gradeList As New List(Of String)
        For i As Integer = 0 To repeaterGradeInfo.Items.Count - 1
            Dim serviceReception As Control = repeaterGradeInfo.Items(i)

            'グレードを取得
            Dim gradeCheck As HtmlInputCheckBox = DirectCast(serviceReception.FindControl("grade"), HtmlInputCheckBox)

            If gradeCheck.Checked Then
                gradeList.Add(GRADE_ON)
            Else
                gradeList.Add(GRADE_OFF)
            End If

        Next
        salesPointInfoData.grd = gradeList

        'タイトル
        salesPointInfoData.title = Me.salesPointTxt.Value

        '説明文
        Dim contents As String = Me.contentsTxt.Value.Replace(vbCrLf, String.Empty)

        If contents.Length > SalesPointContentsOmitLength Then
            salesPointInfoData.contents = Left(contents, SalesPointContentsOmitLength) & OMIT_STRING
        Else
            salesPointInfoData.contents = contents
        End If

        '指示ポイント(トップ)
        Dim topList As New List(Of String)
        topList.Add(Me.topPointField.Value)
        salesPointInfoData.top = topList

        '指示ポイント(レフト)
        Dim leftList As New List(Of String)
        leftList.Add(Me.leftPointField.Value)
        salesPointInfoData.left = leftList

        'オーバーレイ(タイトル)
        salesPointInfoData.overviewtitle = Me.salesPointTxt.Value

        'オーバーレイ(説明文)
        salesPointInfoData.overviewcontents = contents

        'オーバーレイ(トップ)
        Dim topOverList As New List(Of String)
        topOverList.Add(Me.topOverPointField.Value)
        salesPointInfoData.overviewtop = topOverList

        'オーバーレイ(レフト)
        Dim leftOverList As New List(Of String)
        leftOverList.Add(Me.leftOverPointField.Value)
        salesPointInfoData.overviewleft = leftOverList

        'オーバーレイ(画像)
        If Me.summaryFile.HasFile Then
            Dim summaryFilePath As New StringBuilder
            summaryFilePath.Append(TcvSettingConstants.SalespointOverviewPath)
            summaryFilePath.Append(carSeries)
            summaryFilePath.Append(FILE_STRING_SALES_POINT)
            summaryFilePath.Append(targetSalesPointID)
            summaryFilePath.Append(FILE_STRING_S)
            summaryFilePath.Append(System.IO.Path.GetExtension(Me.summaryFile.PostedFile.FileName))
            salesPointInfoData.overviewimg = summaryFilePath.ToString
        Else
            salesPointInfoData.overviewimg = String.Empty
        End If

        'ポップアップ(タイプ)
        If String.IsNullOrEmpty(Me.contentsTxt.Value.Trim) And salesPointInfoData.viewtype.Equals(VIEW_TYPE_IMAGE) Then
            salesPointInfoData.popuptype = VIEW_TYPE_IMAGE & TYPE_ONLY
        ElseIf Not String.IsNullOrEmpty(Me.contentsTxt.Value.Trim) And salesPointInfoData.viewtype.Equals(VIEW_TYPE_IMAGE) Then
            salesPointInfoData.popuptype = VIEW_TYPE_IMAGE
        ElseIf String.IsNullOrEmpty(Me.contentsTxt.Value.Trim) And salesPointInfoData.viewtype.Equals(VIEW_TYPE_MOVIE) Then
            salesPointInfoData.popuptype = VIEW_TYPE_MOVIE & TYPE_ONLY
        ElseIf Not String.IsNullOrEmpty(Me.contentsTxt.Value.Trim) And salesPointInfoData.viewtype.Equals(VIEW_TYPE_MOVIE) Then
            salesPointInfoData.popuptype = VIEW_TYPE_MOVIE
        ElseIf salesPointInfoData.type = VIEW_TYPE_TEXT Then
            salesPointInfoData.popuptype = VIEW_TYPE_TEXT
        End If

        'ポップアップ(タイトル)
        salesPointInfoData.popuptitle = Me.salesPointTxt.Value

        'ポップアップ(説明文)
        salesPointInfoData.popupcontents = Me.contentsTxt.Value.Replace(vbCrLf, TAG_BR)

        'ポップアップ(画像)
        If Me.detailFile.HasFile Then
            Dim detailFilePath As New StringBuilder
            detailFilePath.Append(TcvSettingConstants.SalespointPopupPath)
            detailFilePath.Append(carSeries)
            detailFilePath.Append(FILE_STRING_SALES_POINT)
            detailFilePath.Append(targetSalesPointID)
            detailFilePath.Append(System.IO.Path.GetExtension(Me.detailFile.PostedFile.FileName))
            salesPointInfoData.popupsrc = detailFilePath.ToString
        Else
            salesPointInfoData.popupsrc = String.Empty
        End If

        'フルスクリーンポップアップ(画像)
        If Me.detailPopupFile.HasFile Then
            Dim detailPopupFilePath As New StringBuilder
            detailPopupFilePath.Append(TcvSettingConstants.SalespointFullscreenPath)
            detailPopupFilePath.Append(carSeries)
            detailPopupFilePath.Append(FILE_STRING_SALES_POINT)
            detailPopupFilePath.Append(targetSalesPointID)
            detailPopupFilePath.Append(FILE_STRING_L)
            detailPopupFilePath.Append(System.IO.Path.GetExtension(Me.detailPopupFile.PostedFile.FileName))
            salesPointInfoData.fullscreenpopupsrc = detailPopupFilePath.ToString
        Else
            salesPointInfoData.fullscreenpopupsrc = String.Empty
        End If

        'セールスポイント有効フラグ
        salesPointInfoData.introductionVisible = True

        'オーバーレイファイル名
        salesPointInfoData.OverviewFile = String.Empty

        'ポップアップファイル名
        salesPointInfoData.PopupFile = String.Empty

        'フルスクリーンポップアップファイル名
        salesPointInfoData.FullscreenPopupFile = String.Empty


        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("UpdateSalesPointInfoNew", False))
        'ログ出力 End *****************************************************************************

        Return salesPointInfoData

    End Function

    ''' <summary>
    ''' セールスポイント情報編集データ設定
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carSeries">車種Series</param>
    ''' <param name="exInDvs">エクステリア/インテリア</param>
    ''' <param name="targetSalesPointID">対象セールスポイントID</param>
    ''' <param name="SalesPointContentsOmitLength">一覧説明省略桁数</param>
    ''' <returns>セールスポイント情報編集データ</returns>
    ''' <remarks></remarks>
    Private Function UpdateSalesPointInfoEdit(ByVal tcvPath As String, _
                                          ByVal carSeries As String, _
                                          ByVal exInDvs As String, _
                                          ByVal targetSalesPointID As String, _
                                          ByVal SalesPointContentsOmitLength As Integer, _
                                          ByVal SalesPointList As SalesPointListJson) As SalesPointListJson

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("UpdateSalesPointInfo", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("exInDvs", exInDvs, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("targetSalesPointID", targetSalesPointID, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("SalesPointContents1OmitLength", CStr(SalesPointContentsOmitLength), True))
        'ログ出力 End *****************************************************************************

        '編集の場合
        For Each salesPointData As SalesPointJson In SalesPointList.sales_point

            If Not targetSalesPointID.Equals(salesPointData.id) Then
                '対象セールスポイントIDに該当するIDでない場合は処理しない。次のデータへ
                Continue For
            End If

            'タイプ
            If Me.summaryFile.HasFile Or Me.detailFile.HasFile Or Me.detailPopupFile.HasFile _
                Or Not String.IsNullOrEmpty(Me.overViewFileNameField.Value) _
                Or Not String.IsNullOrEmpty(Me.popUpFileNameField.Value) _
                Or Not String.IsNullOrEmpty(Me.fullPopUpFileNameField.Value) Then

                If Me.detailFile.HasFile _
                    And (System.IO.Path.GetExtension(Me.detailFile.PostedFile.FileName).Equals(EXT_BUILD & EXT_MP4) _
                    Or System.IO.Path.GetExtension(Me.detailFile.PostedFile.FileName).Equals(EXT_BUILD & EXT_MP4_BIG) _
                    Or System.IO.Path.GetExtension(Me.detailFile.PostedFile.FileName).Equals(EXT_BUILD & EXT_MOV) _
                    Or System.IO.Path.GetExtension(Me.detailFile.PostedFile.FileName).Equals(EXT_BUILD & EXT_MOV_BIG)) Then
                    salesPointData.viewtype = VIEW_TYPE_MOVIE
                Else
                    If Not String.IsNullOrEmpty(Me.overViewFileNameField.Value) _
                        And (System.IO.Path.GetExtension(Me.overViewFileNameField.Value).Equals(EXT_BUILD & EXT_MP4) _
                        Or System.IO.Path.GetExtension(Me.overViewFileNameField.Value).Equals(EXT_BUILD & EXT_MP4_BIG) _
                        Or System.IO.Path.GetExtension(Me.overViewFileNameField.Value).Equals(EXT_BUILD & EXT_MOV) _
                        Or System.IO.Path.GetExtension(Me.overViewFileNameField.Value).Equals(EXT_BUILD & EXT_MOV_BIG)) Then
                        salesPointData.viewtype = VIEW_TYPE_MOVIE
                    Else
                        salesPointData.viewtype = VIEW_TYPE_IMAGE
                    End If

                End If

            Else
                salesPointData.viewtype = VIEW_TYPE_TEXT
            End If

            '外装アングル
            If Me.exInField.Value = TYPE_EXTERIOR Then
                Dim angleList As New List(Of String)
                angleList.Add(Me.angleField.Value)
                salesPointData.angle = angleList
            Else
                Dim angleList As New List(Of String)
                angleList.Add(String.Empty)
                salesPointData.angle = angleList
            End If

            '内装画面ID
            If Me.exInField.Value = TYPE_INTERIOR Then
                Dim interioridList As New List(Of String)
                interioridList.Add(Me.angleField.Value)
                salesPointData.interiorid = interioridList
            Else
                Dim interioridList As New List(Of String)
                interioridList.Add(String.Empty)
                salesPointData.interiorid = interioridList
            End If

            'グレード適合
            Dim gradeList As New List(Of String)
            For i As Integer = 0 To repeaterGradeInfo.Items.Count - 1
                Dim serviceReception As Control = repeaterGradeInfo.Items(i)

                'グレードを取得
                Dim gradeCheck As HtmlInputCheckBox = DirectCast(serviceReception.FindControl("grade"), HtmlInputCheckBox)

                If gradeCheck.Checked Then
                    gradeList.Add(GRADE_ON)
                Else
                    gradeList.Add(GRADE_OFF)
                End If

            Next
            salesPointData.grd = gradeList

            'タイトル
            salesPointData.title = Me.salesPointTxt.Value

            '説明文
            Dim contents As String = Me.contentsTxt.Value.Replace(vbCrLf, String.Empty)

            If contents.Length > SalesPointContentsOmitLength Then
                salesPointData.contents = Left(contents, SalesPointContentsOmitLength) & OMIT_STRING
            Else
                salesPointData.contents = contents
            End If

            '指示ポイント(トップ)
            Dim topList As New List(Of String)
            topList.Add(Me.topPointField.Value)
            salesPointData.top = topList

            '指示ポイント(レフト)
            Dim leftList As New List(Of String)
            leftList.Add(Me.leftPointField.Value)
            salesPointData.left = leftList

            'オーバーレイ(タイトル)
            salesPointData.overviewtitle = Me.salesPointTxt.Value

            'オーバーレイ(説明文)
            salesPointData.overviewcontents = contents

            'オーバーレイ(トップ)
            Dim topOverList As New List(Of String)
            topOverList.Add(Me.topOverPointField.Value)
            salesPointData.overviewtop = topOverList

            'オーバーレイ(レフト)
            Dim leftOverList As New List(Of String)
            leftOverList.Add(Me.leftOverPointField.Value)
            salesPointData.overviewleft = leftOverList

            'オーバーレイ(画像)
            If Me.summaryFile.HasFile Then
                Dim summaryFilePath As New StringBuilder
                summaryFilePath.Append(TcvSettingConstants.SalespointOverviewPath)
                summaryFilePath.Append(carSeries)
                summaryFilePath.Append(FILE_STRING_SALES_POINT)
                summaryFilePath.Append(targetSalesPointID)
                summaryFilePath.Append(FILE_STRING_S)
                summaryFilePath.Append(System.IO.Path.GetExtension(Me.summaryFile.PostedFile.FileName))
                salesPointData.overviewimg = summaryFilePath.ToString

            ElseIf Not Me.summaryFile.HasFile And Not String.IsNullOrEmpty(Me.overViewFileNameField.Value) Then
                Dim summaryFilePath As New StringBuilder
                summaryFilePath.Append(TcvSettingConstants.SalespointOverviewPath)
                summaryFilePath.Append(salesPointData.OverviewFile)
                salesPointData.overviewimg = summaryFilePath.ToString

            ElseIf String.IsNullOrEmpty(Me.overViewFileNameField.Value) Then
                salesPointData.overviewimg = String.Empty

            End If

            'ポップアップ(タイプ)
            If String.IsNullOrEmpty(Me.contentsTxt.Value.Trim) And salesPointData.viewtype.Equals(VIEW_TYPE_IMAGE) Then
                salesPointData.popuptype = VIEW_TYPE_IMAGE & TYPE_ONLY
            ElseIf Not String.IsNullOrEmpty(Me.contentsTxt.Value.Trim) And salesPointData.viewtype.Equals(VIEW_TYPE_IMAGE) Then
                salesPointData.popuptype = VIEW_TYPE_IMAGE
            ElseIf String.IsNullOrEmpty(Me.contentsTxt.Value.Trim) And salesPointData.viewtype.Equals(VIEW_TYPE_MOVIE) Then
                salesPointData.popuptype = VIEW_TYPE_MOVIE & TYPE_ONLY
            ElseIf Not String.IsNullOrEmpty(Me.contentsTxt.Value.Trim) And salesPointData.viewtype.Equals(VIEW_TYPE_MOVIE) Then
                salesPointData.popuptype = VIEW_TYPE_MOVIE
            ElseIf salesPointData.viewtype = VIEW_TYPE_TEXT Then
                salesPointData.popuptype = VIEW_TYPE_TEXT
            End If

            'オーバーレイ(タイトル)
            salesPointData.popuptitle = Me.salesPointTxt.Value

            'オーバーレイ(説明文)
            salesPointData.popupcontents = Me.contentsTxt.Value.Replace(vbCrLf, TAG_BR)

            'ポップアップ(画像)
            If Me.detailFile.HasFile Then
                Dim detailFilePath As New StringBuilder
                detailFilePath.Append(TcvSettingConstants.SalespointPopupPath)
                detailFilePath.Append(carSeries)
                detailFilePath.Append(FILE_STRING_SALES_POINT)
                detailFilePath.Append(targetSalesPointID)
                detailFilePath.Append(System.IO.Path.GetExtension(Me.detailFile.PostedFile.FileName))
                salesPointData.popupsrc = detailFilePath.ToString
            Else
                If String.IsNullOrEmpty(Me.popUpFileNameField.Value) Then
                    salesPointData.popupsrc = String.Empty
                End If
            End If

            'フルスクリーンポップアップ(画像)
            If Me.detailPopupFile.HasFile Then
                Dim detailPopupFilePath As New StringBuilder
                detailPopupFilePath.Append(TcvSettingConstants.SalespointFullscreenPath)
                detailPopupFilePath.Append(carSeries)
                detailPopupFilePath.Append(FILE_STRING_SALES_POINT)
                detailPopupFilePath.Append(targetSalesPointID)
                detailPopupFilePath.Append(FILE_STRING_L)
                detailPopupFilePath.Append(System.IO.Path.GetExtension(Me.detailPopupFile.PostedFile.FileName))
                salesPointData.fullscreenpopupsrc = detailPopupFilePath.ToString
            Else
                If String.IsNullOrEmpty(Me.fullPopUpFileNameField.Value) Then
                    salesPointData.fullscreenpopupsrc = String.Empty
                End If
            End If

            'IDは一意なので、処理が完了したら処理を終わる
            Exit For

        Next


        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("UpdateSalesPointInfo", False))
        'ログ出力 End *****************************************************************************

        Return SalesPointList

    End Function

#End Region

#Region " セールスポイント情報削除処理 "
    ''' <summary>
    ''' セールスポイント情報削除処理
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carSeries">車種Series</param>
    ''' <param name="exInDvs">エクステリア/インテリア</param>
    ''' <param name="targetSalesPointID">削除対象セールスポイントID</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function DeleteSalesPointInfo(ByVal tcvPath As String, _
                                          ByVal carSeries As String, _
                                          ByVal exInDvs As String, _
                                          ByVal targetSalesPointID As String) As String

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("UpdateSalesPointInfo", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("exInDvs", exInDvs, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("targetSalesPointID", targetSalesPointID, True))
        'ログ出力 End *****************************************************************************

        'JSON形式からデシリアライズ
        Dim jss As JavaScriptSerializer = _
            New JavaScriptSerializer(New Script.Serialization.SimpleTypeResolver)
        Dim salesPointList As SalesPointListJson = _
            jss.Deserialize(Of SalesPointListJson)(Me.salesPointJsonField.Value)

        Dim repSalesPointList As SalesPointListJson = _
            jss.Deserialize(Of SalesPointListJson)(Me.salesPointJsonField.Value)

        Dim bizLogic = New SC3050703BusinessLogic

        Dim msgID As String = _
            bizLogic.DeleteSalesPointInfoSend(tcvPath, _
                                                carSeries, _
                                                exInDvs, _
                                                targetSalesPointID, _
                                                salesPointList)

        Dim tcvSettingHistoryFilePath As String = _
            DirectCast(MyBase.GetValue(ScreenPos.Current, _
                            SEARCH_KEY_TCV_SETTING_HISTORYFILE_PATH, False), String)

        If String.IsNullOrEmpty(msgID) Then
            'StaffContextからアカウントを取得
            Dim staffInfo As StaffContext = StaffContext.Current
            Dim account As String = staffInfo.Account

            '現在日時取得
            Dim nowDate As Date = DateTimeFunc.Now
            Dim nowFormatDrate As String = DateTimeFunc.FormatDate(CONV_ID_15, nowDate)

            Dim summaryUploadFile As String = String.Empty
            Dim detailUploadFile As String = String.Empty
            Dim detailPopupUploadFile As String = String.Empty

            If Me.summaryFile.HasFile Then
                summaryUploadFile = Me.summaryFile.PostedFile.FileName
            End If

            If Me.detailFile.HasFile Then
                detailUploadFile = Me.detailFile.PostedFile.FileName
            End If

            If Me.detailPopupFile.HasFile Then
                detailPopupUploadFile = Me.detailPopupFile.FileName
            End If

            '履歴ファイル作成処理呼び出し
            bizLogic.CallCreateTcvArchiveFile(carSeries, _
                                        tcvPath, _
                                        tcvSettingHistoryFilePath, _
                                        targetSalesPointID, _
                                        repSalesPointList, _
                                        nowFormatDrate, _
                                        account, _
                                        DEL_DVS_ON, _
                                        summaryUploadFile, _
                                        detailUploadFile, _
                                        detailPopupUploadFile, _
                                        Me.overViewFileNameField.Value, _
                                        Me.overViewFileNameField.Value, _
                                        Me.fullPopUpFileNameField.Value)
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(msgID))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("UpdateSalesPointInfo", False))
        'ログ出力 End *****************************************************************************

        Return msgID

    End Function

#End Region

#Region " セールスポイントテーブル動的生成 "

    ''' <summary>
    ''' セールスポイントテーブル動的生成
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreatePointTable(ByVal exInDvs As String)

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("CreatePointTable", True))
        'ログ出力 End *****************************************************************************

        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim intCells As Integer = 0 ' セルの数
        Dim intRows As Integer = 0 ' 行の数

        'テーブルサイズから構成数を算出
        If TYPE_EXTERIOR.Equals(exInDvs) Then
            intCells = EX_TABLE_COLS
            intRows = EX_TABLE_ROWS

            ''補正値分の背景画像位置を調整
            'Me.frame.Style.Add("background-position", "-20pt -20pt")
            Me.setPointArea.Style.Add("width", "693px")
            Me.imageFrame.Style.Add("width", "665px")
            Me.imageFrame.Style.Add("padding", "12px")

        Else

            intCells = IN_TABLE_COLS
            intRows = IN_TABLE_ROWS
            Me.setPointArea.Style.Add("width", "936px")
            Me.imageFrame.Style.Add("width", "902px")
            Me.imageFrame.Style.Add("height", "375px")
            Me.imageFrame.Style.Add("padding", "10px 15px 11px") '[上][左右][下]


            '内装の場合は画像を縮小表示
            Me.imageFrame.Style.Add("background-size", "cover")

            ''補正値分の背景画像位置を調整
            'Me.frame.Style.Add("background-position", "-20pt -20pt")

        End If

        Me.frame.Style.Add("background-color", "transparent")

        'ポイントテーブルを生成
        For i = 0 To intRows - 1
            '行の作成
            Dim tbRow As New TableRow
            For j = 0 To intCells - 1
                ' セルの作成
                Dim tbCell As New TableCell

                'グリッドサイズ指定
                'tbCell.Width = TABLE_GRID_SIZE
                'tbCell.Height = TABLE_GRID_SIZE
                If TYPE_EXTERIOR.Equals(exInDvs) Then
                    tbCell.Width = TABLE_GRID_SIZE
                    tbCell.Height = TABLE_GRID_SIZE
                Else
                    tbCell.Width = IN_CELL_WIDTH
                    tbCell.Height = IN_CELL_HEIGHT
                End If

                'グリッドボーダーサイズ指定
                tbCell.BorderWidth = TABLE_BORDER_WIDTH

                'セルのIDを指定
                tbCell.ID = _
                    CELL_ID & CStr(j + 1).PadLeft(2, CChar(CELL_ID_STRING)) & _
                    CStr(i + 1).PadLeft(2, CChar(CELL_ID_STRING))

                'セルを追加
                tbRow.Cells.Add(tbCell)

            Next

            '行を追加
            Me.frame.Rows.Add(tbRow)

        Next


        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("CreatePointTable", False))
        'ログ出力 End *****************************************************************************

    End Sub

#End Region

#End Region

#Region " 入力チェック "

    ''' <summary>
    ''' 入力値検証
    ''' </summary>
    ''' <returns>正常:True/異常:False</returns>
    ''' <remarks></remarks>
    Private Function ValidateSC3050703() As Boolean

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("ValidateSC3050703", True))
        'ログ出力 End *****************************************************************************

        '-------------------------------------------------------------------禁則文字チェック
        Dim sortNo As String = String.Empty

        'セールスポイント名禁則文字チェック
        If Not Validation.IsValidString(Me.salesPointTxt.Value) Then
            Me.ShowMessageBox(MESSAGE_ID_901, {WebWordUtility.GetWord(MESSAGE_ID_3)})
            Return False
        End If

        '説明禁則文字チェック
        If Not Validation.IsValidString(Me.contentsTxt.Value) Then
            Me.ShowMessageBox(MESSAGE_ID_901, {WebWordUtility.GetWord(MESSAGE_ID_4)})
            Return False
        End If

        'ログ出力 Start ***************************************************************************
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("ValidateSC3050703", False))
        'ログ出力 End *****************************************************************************

        Return True

    End Function

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
