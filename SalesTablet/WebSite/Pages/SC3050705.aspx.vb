'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3050705.aspx.vb
'─────────────────────────────────────
'機能： MOP/DOP詳細設定
'補足： 
'作成： 2012/11/30 TMEJ 玉置
'更新： 
'─────────────────────────────────────

Option Strict On

Imports System.Reflection.MethodBase
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core

Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Common
Imports System.Globalization

Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.TCVSettingUtility
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.SC3050705
Imports System.Web

Partial Class Pages_SC3050705
    Inherits BasePage

#Region " セッションキー "

    ''' <summary>
    ''' 車種IDセッションキー（遷移パラメータ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_CAR_ID As String = "CarId"

    ''' <summary>
    ''' 車種名セッションキー（遷移パラメータ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_CAR_NAME As String = "CarName"

    ''' <summary>
    ''' オプションIDセッションキー（遷移パラメータ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_OPTION_ID As String = "OptionId"

    ''' <summary>
    ''' オプション種別セッションキー（遷移パラメータ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_OPTION_KIND As String = "OptionKind"

    ''' <summary>
    ''' 追加フラグセッションキー（遷移パラメータ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SEARCH_KEY_PROCESS_ID As String = "ProcessId"

#End Region

#Region " 定数 "

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM As String = "SC3050705"

    ''' <summary>
    ''' TCV物理パスパラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_PATH As String = "TCV_PATH"

    ''' <summary>
    ''' TCV URLパラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_URL As String = "TCV_URL"

    ''' <summary>
    ''' 履歴ファイル格納パスパラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_SETTING_HISTORYFILE_PATH As String = "TCV_SETTING_HISTORYFILE_PATH"

    ''' <summary>
    ''' 画像ファイルサイズ上限パラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_IMAGE_MAX_FILE_SIZE As String = "TCV_OPTION_IMAGE_MAX_FILE_SIZE"

    ''' <summary>
    ''' 価格少数桁数パラメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TCV_DECIMAL_POINT_LENGTH As String = "TCV_DECIMAL_POINT_LENGTH"

    ''' <summary>
    ''' 画面ID:MOP/DOP設定(SC3050704)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATIONID_SALES_POINT_DETAIL As String = "SC3050704"

    ''' <summary>
    ''' 表示モード：すべての項目を設定する
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const DisplayModeInputAll As String = "1"

    ''' <summary>
    ''' 表示モード：リコメンド属性のみを設定する
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const DisplayModeRecommendOnly As String = "2"

    ''' <summary>
    ''' 表示モード：表示のみ
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const DisplayModeDisplayOnly As String = "3"

    ''' <summary>
    ''' オプション種別：メーカーオプション
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPTION_KIND_MAKER_OPTION As String = "1"

    ''' <summary>
    ''' オプション種別：ディーラーオプション
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPTION_KIND_DEALER_OPTION As String = "2"

    ''' <summary>
    ''' 処理区分（1：新規）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CD_INSERT As String = "1"

    ''' <summary>
    ''' 処理区分（2：更新）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CD_UPDATE As String = "2"

    ''' <summary>
    ''' 処理区分（3：削除）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROCESS_CD_DELETE As String = "3"

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
    ''' 拡張子
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXT_JPG_PNG As String = "jpg/png"

    ''' <summary>
    ''' ファイルサイズ単位
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FILE_SIZE_UNIT As String = "KB"

#End Region

#Region " 文言 "

    ''' <summary>
    ''' 文言:MOP詳細設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_TITLE_MOP_SETTING As Integer = 1

    ''' <summary>
    ''' 文言:DOP詳細設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_TITLE_DOP_SETTING As Integer = 2

    ''' <summary>
    ''' 文言:メッセージ
    ''' </summary>
    ''' <remarks>オプション名</remarks>
    Private Const WORD_OPTION_NAME As Integer = 3

    ''' <summary>
    ''' 文言:メッセージ
    ''' </summary>
    ''' <remarks>価格</remarks>
    Private Const WORD_PRICE As Integer = 4

    ''' <summary>
    ''' 文言:メッセージ
    ''' </summary>
    ''' <remarks>画像</remarks>
    Private Const WORD_IMAGE As Integer = 5

    ''' <summary>
    ''' 文言:メッセージ
    ''' </summary>
    ''' <remarks>グレード適合</remarks>
    Private Const WORD_GRADE As Integer = 7

    ''' <summary>
    ''' 文言:メッセージ
    ''' </summary>
    ''' <remarks>画面情報が破棄されますが、よろしいですか？</remarks>
    Private Const WORD_MSG_CONFIRM_DISCARD As Integer = 11

    ''' <summary>
    ''' 文言:メッセージ
    ''' </summary>
    ''' <remarks>{0}を削除します。よろしいですか？</remarks>
    Private Const WORD_MSG_CONFIRM_DELETE As Integer = 12

    ''' <summary>
    ''' 文言:メッセージ
    ''' </summary>
    ''' <remarks>オプション情報</remarks>
    Private Const WORD_MSG_OPTION_DATA As Integer = 13

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>画面に表示された情報が最新ではない可能性があります。画面に最新情報を表示します。</remarks>
    Private Const WORD_ERR_NOT_LATEST As Integer = 900

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>{0}を入力して下さい。</remarks>
    Private Const WORD_ERR_REQUIRED As Integer = 901

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>{0}の入力が不正です。</remarks>
    Private Const WORD_ERR_INVALID As Integer = 902

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>{0}に数値を入力して下さい。</remarks>
    Private Const WORD_ERR_NUMBER As Integer = 903

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>選択できる{0}は、{1}のみです。</remarks>
    Private Const WORD_ERR_IMAGE_FILE As Integer = 904

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>アップロード{0}のファイルサイズが上限({1})を超えています。</remarks>
    Private Const WORD_ERR_IMAGE_FILE_SIZE As Integer = 905

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>{0}を選択して下さい。</remarks>
    Private Const WORD_ERR_REQUISITE As Integer = 906

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>{0}は小数点第{1}位以内で入力して下さい。</remarks>
    Private Const WORD_ERR_DECIMAL As Integer = 907

#End Region

#Region " ページロード "
    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        ' ポストバック判定
        If Not Page.IsPostBack AndAlso Not Page.IsCallback Then

            '遷移パラメータの取得
            Dim carId As String = String.Empty
            Dim carName As String = String.Empty
            Dim optionId As String = String.Empty
            Dim optionKind As String = String.Empty
            Dim processId As String = String.Empty

            '車種ID
            If ContainsKey(ScreenPos.Current, SEARCH_KEY_CAR_ID) Then
                carId = DirectCast(Me.GetValue(ScreenPos.Current, SEARCH_KEY_CAR_ID, False), String)
            End If

            '車種名
            If ContainsKey(ScreenPos.Current, SEARCH_KEY_CAR_NAME) Then
                carName = DirectCast(Me.GetValue(ScreenPos.Current, SEARCH_KEY_CAR_NAME, False), String)
            End If

            'オプションID
            If ContainsKey(ScreenPos.Current, SEARCH_KEY_OPTION_ID) Then
                optionId = DirectCast(Me.GetValue(ScreenPos.Current, SEARCH_KEY_OPTION_ID, False), String)
            End If

            'オプション種別
            If ContainsKey(ScreenPos.Current, SEARCH_KEY_OPTION_KIND) Then
                optionKind = DirectCast(Me.GetValue(ScreenPos.Current, SEARCH_KEY_OPTION_KIND, False), String)
            End If

            '追加フラグ
            If ContainsKey(ScreenPos.Current, SEARCH_KEY_PROCESS_ID) Then
                processId = DirectCast(Me.GetValue(ScreenPos.Current, SEARCH_KEY_PROCESS_ID, False), String)
            End If

            '文言設定
            SetWord(optionKind)

            'ログイン情報取得
            Dim context As StaffContext = StaffContext.Current

            '車種名の設定
            Me.carName.Text = carName

            'オプション情報を取得
            GetOptionInfo(
                    optionId,
                    optionKind,
                    carId,
                    context.DlrCD
                )

            'ヘッダー初期設定
            InitializeHeader()

            'フッター初期設定
            InitializeFooter()

            '遷移情報の設定
            Me.HiddenCarId.Value = carId
            Me.HiddenOptionId.Value = optionId
            Me.HiddenOptionKind.Value = optionKind
            Me.HiddenProcessId.Value = processId

            Dim sysEnv As New SystemEnvSetting
            '画像アップロードサイズ
            Dim imageMaxFileSize As String = sysEnv.GetSystemEnvSetting(TCV_IMAGE_MAX_FILE_SIZE).PARAMVALUE
            '少数桁数
            Dim decimalPointLength As String = sysEnv.GetSystemEnvSetting(TCV_DECIMAL_POINT_LENGTH).PARAMVALUE
            'アップロードサイズの設定
            Me.HiddenImageMaxFileSizeField.Value = imageMaxFileSize
            '少数桁数の設定
            Me.HiddenDecimalPoint.Value = decimalPointLength
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

#Region " イベント "

    ''' <summary>
    ''' 保存ダミーボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub SaveButton_Click(sender As Object, e As System.EventArgs) Handles SaveButton.Click

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '遷移情報の取得
        Dim optionId As String = Me.HiddenOptionId.Value
        Dim optionKind As String = Me.HiddenOptionKind.Value
        Dim carId As String = Me.HiddenCarId.Value
        Dim processId As String = Me.HiddenProcessId.Value
        'ファイルオブジェクトの取得
        Dim fileOjbect As HttpPostedFile = Me.reference.PostedFile

        '入力チェック
        'オプション情報の格納
        Dim optionInfo As New OptionInfo
        If optionKind.Equals(OPTION_KIND_DEALER_OPTION) Then
            'オプションID
            optionInfo.OptionId = optionId
            'オプション名
            optionInfo.OptionName = Me.OptionName.Text
            '価格
            optionInfo.Price = Me.Price.Text
            'ファイル名
            optionInfo.ImageFileName = Me.HiddenFileName.Value
            'グレード適合
            Dim gradeList As New List(Of String)
            For i = 0 To Me.repeaterGradeInfo.Items.Count - 1
                Dim checkValue As String = String.Empty
                If DirectCast(Me.repeaterGradeInfo.Items(i).FindControl("Grade"), HtmlInputCheckBox).Checked Then
                    checkValue = GRADE_ON
                Else
                    checkValue = GRADE_OFF
                End If
                gradeList.Add(checkValue)
            Next

            optionInfo.SetGradeConformity(gradeList)
        End If
        '
        'リコメンド情報の格納
        Dim recommendInfoList As New RecommendInfoList
        '排他時間の取得
        recommendInfoList.TimeStamp = Me.HiddenTimeStamp.Value

        For i = 0 To Me.repeaterRecommendInfo.Items.Count - 1
            Dim recommendInfo As New RecommendInfo
            'リコメンド属性ID
            recommendInfo.RecommendId = DirectCast(Me.repeaterRecommendInfo.Items(i).FindControl("RecommendId"), HtmlInputHidden).Value
            'リコメンド属性
            recommendInfo.RecommendCheck = DirectCast(Me.repeaterRecommendInfo.Items(i).FindControl("Recommend"), HtmlInputCheckBox).Checked
            recommendInfoList.Root.Add(recommendInfo)
        Next

        'ログイン情報取得
        Dim context As StaffContext = StaffContext.Current

        '処理フラグ判定
        Dim processCd As String
        If String.IsNullOrEmpty(optionId) Then
            processCd = PROCESS_CD_INSERT
            Dim biz As SC3050705BusinessLogic = New SC3050705BusinessLogic()
            Dim sysEnv As New SystemEnvSetting
            Dim TcvPath As String = sysEnv.GetSystemEnvSetting(TCV_PATH).PARAMVALUE
            optionId = CStr(biz.GetMaxId(TcvPath, carId, context.DlrCD))
        Else
            processCd = PROCESS_CD_UPDATE
        End If

        '保存処理の実行
        Dim msgId As Integer = UpdateOptionInfo(
            optionInfo,
            recommendInfoList,
            optionId,
            optionKind,
            carId,
            context.DlrCD,
            context.Account,
            fileOjbect,
            processCd)

        'オプション情報を取得
        GetOptionInfo(
                optionId,
                optionKind,
                carId,
                context.DlrCD
            )

        '結果表示
        If msgId <> 0 Then
            '保存失敗
            ShowMessageBox(msgId)
            Me.refleshDvsField.Value = "1"
        End If

        If optionKind.Equals(OPTION_KIND_DEALER_OPTION) Then
            Me.HiddenOptionId.Value = optionId
            Me.delButtonLink.Visible = True
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))
    End Sub

    ''' <summary>
    ''' 削除ダミーボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub DeleteButton_Click(sender As Object, e As System.EventArgs) Handles DeleteButton.Click

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '遷移情報の取得
        Dim optionId As String = Me.HiddenOptionId.Value
        Dim optionKind As String = Me.HiddenOptionKind.Value
        Dim carId As String = Me.HiddenCarId.Value
        
        '排他時間の取得
        Dim recommendinfolist As New RecommendInfoList
        recommendInfoList.TimeStamp = Me.HiddenTimeStamp.Value

        'ログイン情報取得
        Dim context As StaffContext = StaffContext.Current

        '保存処理の実行
        Dim msgId As Integer = UpdateOptionInfo(
            Nothing,
            recommendinfolist,
            optionId,
            optionKind,
            carId,
            context.DlrCD,
            context.Account,
            Nothing,
            PROCESS_CD_DELETE)

        '結果表示
        If Not msgId = 0 Then
            '保存失敗
            ShowMessageBox(msgId)
            Me.refleshDvsField.Value = "1"
            Return
        End If

        '削除完了後、遷移元画面へ戻る
        Me.RedirectPrevScreen()

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>  
    ''' リフレッシュボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub RefleshButton_Click(sender As Object, e As System.EventArgs) Handles RefleshButton.Click

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'ログイン情報取得
        Dim context As StaffContext = StaffContext.Current

        'オプション情報を取得
        GetOptionInfo(
                Me.HiddenOptionId.Value,
                Me.HiddenOptionKind.Value,
                Me.HiddenCarId.Value,
                context.DlrCD
            )

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))
    End Sub

    ''' <summary>
    ''' 入力チェックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub ValidationButton_Click(sender As Object, e As System.EventArgs) Handles ValidationButton.Click

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '表示モード取得
        Dim displayMode As String = Me.HiddenDisplayMode.Value

        'オプション名が入力状態の場合
        If displayMode.Equals(DisplayModeInputAll) Then
            'オプション名の禁則文字チェック
            Dim optionName As String = Me.OptionName.Text
            If Not Validation.IsValidString(optionName) Then
                Me.ajaxErrorField.Value = "1"
                ShowMessageBox(WORD_ERR_INVALID, HttpUtility.HtmlEncode(WebWordUtility.GetWord(WORD_OPTION_NAME)))
            End If
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub
#End Region

#Region " 内部メソッド "

#Region " オプション情報取得処理 "
    ''' <summary>
    ''' オプション情報取得処理
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="optionKind">オプション種別</param>
    ''' <param name="carId">車種ID</param>
    ''' <param name="dealerCd">販売店コード</param>
    ''' <remarks></remarks>
    Private Sub GetOptionInfo(ByVal optionId As String,
                              ByVal optionKind As String,
                              ByVal carId As String,
                              ByVal dealerCd As String)
        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim sysEnv As New SystemEnvSetting

        'TCV物理パス
        Dim TcvPath As String = sysEnv.GetSystemEnvSetting(TCV_PATH).PARAMVALUE
        'TCV URL
        Dim TcvUrl As String = sysEnv.GetSystemEnvSetting(TCV_URL).PARAMVALUE

        'オプション情報取得
        Dim biz As SC3050705BusinessLogic = New SC3050705BusinessLogic()
        Dim optionInfo As OptionInfo = Nothing
        optionInfo = biz.GetOptionInfo(
                optionId,
                optionKind,
                carId,
                dealerCd,
                TcvPath,
                TcvUrl
                )

        'リコメンド属性情報取得
        Dim recommendInfo As RecommendInfoList = Nothing
        recommendInfo = biz.GetRecommendInfo(
                optionId,
                optionKind,
                carId,
                dealerCd,
                TcvPath
                )

        'グレード情報の取得
        Dim gradeInfo As List(Of TcvWebGradeJson)
        gradeInfo = getSearchTcvWeb(TcvPath,
                                    carId)

        'オプション情報を画面に反映
        'メーカーオプションの場合
        If optionKind.Equals(OPTION_KIND_MAKER_OPTION) Then

            'オプション名
            Me.LabelOptionName.Text = optionInfo.OptionName
            '価格
            Me.LabelPrice.Text = optionInfo.Price
            '画像ファイル名
            Me.OptionImageLink.HRef = "#" & optionInfo.ImageFileName
            Me.OptionImageName.Text = optionInfo.ImageFileName
            Me.HiddenFileName.Value = optionInfo.ImageFileName
            Me.HiddenFilePath.Value = optionInfo.ImageFilePath

            '表示のみ
            'リコメンド情報の設定
            SetRecommendInfo(recommendInfo, True)
            'グレード情報の設定
            SetGradeInfo(gradeInfo, optionInfo.GradeConformity, True)

            Me.HiddenDisplayMode.Value = DisplayModeDisplayOnly
            Me.delButtonLink.Visible = False
            Me.saveButtonLink.Visible = False

        'ディーラーオプションの場合
        Else
            'オプション名
            Me.OptionName.Text = optionInfo.OptionName
            '価格
            Me.Price.Text = optionInfo.Price
            '画像ファイル名
            Me.OptionImageLink.HRef = "#" & optionInfo.ImageFileName
            Me.OptionImageName.Text = optionInfo.ImageFileName
            Me.HiddenFileName.Value = optionInfo.ImageFileName
            Me.HiddenFilePath.Value = optionInfo.ImageFilePath

            'リコメンド情報の設定
            SetRecommendInfo(recommendInfo, False)
            'グレード情報の設定
            SetGradeInfo(gradeInfo, optionInfo.GradeConformity, False)

            '表示モード
            Me.HiddenDisplayMode.Value = DisplayModeInputAll

            '新規の場合は削除ボタン非活性
            If String.IsNullOrEmpty(optionId) Then
                Me.delButtonLink.Visible = False
            End If
        End If

        '排他日時
        Me.HiddenTimeStamp.Value = recommendInfo.TimeStamp

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub
#End Region

#Region " オプション情報の更新処理 "
    ''' <summary>
    ''' オプション情報の更新処理
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="optionKind">オプション種別</param>
    ''' <param name="carId">車種ID</param>
    ''' <param name="dealerCd">販売店コード</param>
    ''' <returns>メッセージ</returns>
    ''' <remarks></remarks>
    Private Function UpdateOptionInfo(ByVal optionInfo As OptionInfo,
                                      ByVal recommendInfo As RecommendInfoList,
                                      ByVal optionId As String,
                                      ByVal optionKind As String,
                                      ByVal carId As String,
                                      ByVal dealerCd As String,
                                      ByVal account As String,
                                      ByVal uploadFile As HttpPostedFile,
                                      ByVal processCd As String) As Integer
        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim sysEnv As New SystemEnvSetting
        'TCV物理パス
        Dim TcvPath As String = sysEnv.GetSystemEnvSetting(TCV_PATH).PARAMVALUE
        '履歴ファイル格納パス
        Dim TcvSettingHistoryFilePath As String = sysEnv.GetSystemEnvSetting(TCV_SETTING_HISTORYFILE_PATH).PARAMVALUE
        Dim msgId As Integer = 0

        'オプション情報の更新
        Dim biz As SC3050705BusinessLogic = New SC3050705BusinessLogic()

        msgId = biz.UpdateOptionInfo(
                optionInfo,
                recommendInfo,
                optionId,
                optionKind,
                carId,
                dealerCd,
                account,
                TcvPath,
                TcvSettingHistoryFilePath,
                uploadFile,
                processCd
                )

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return msgId

    End Function

#End Region

#Region " DIST権限判定処理 "
    ''' <summary>
    ''' DIST権限かどうかを判定します。
    ''' </summary>
    ''' <returns>DIST権限はTrue、それ以外はFalseを返します。</returns>
    ''' <remarks></remarks>
    Private Function IsDist() As Boolean

        '権限取得
        Dim opeCd As Operation = StaffContext.Current.OpeCD

        'DIST権限かどうかを判定
        If opeCd = Operation.DM OrElse opeCd = Operation.DO Then
            Return True
        End If

        Return False

    End Function

#End Region

#Region " グレード情報取得処理 "
    ''' <summary>
    ''' グレード情報取得処理
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carSeries">車種シリーズ</param>
    ''' <remarks></remarks>
    Private Function getSearchTcvWeb(ByVal tcvPath As String, _
                                      ByVal carSeries As String) As List(Of TcvWebGradeJson)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim tcvWebList As TcvWebListJson = Nothing
        tcvWebList = TcvSettingUtilityBusinessLogic.GetTcvWeb(tcvPath, carSeries)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return tcvWebList.grade

    End Function
#End Region

#Region " 文言設定処理 "
    ''' <summary>
    ''' 文言を設定します。
    ''' </summary>
    ''' <remarks>動的に表示する文言のみを設定します。</remarks>
    Private Sub SetWord(ByVal OptionKind As String)


        Dim sysEnv As New SystemEnvSetting
        '画像アップロードサイズ
        Dim imageMaxFileSize As String = sysEnv.GetSystemEnvSetting(TCV_IMAGE_MAX_FILE_SIZE).PARAMVALUE
        '少数桁数
        Dim decimalPointLength As String = sysEnv.GetSystemEnvSetting(TCV_DECIMAL_POINT_LENGTH).PARAMVALUE
        '文言設定
        'タイトル設定
        If OptionKind.Equals(OPTION_KIND_MAKER_OPTION) Then
            Me.HiddenTitle.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(WORD_TITLE_MOP_SETTING))
        Else
            Me.HiddenTitle.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(WORD_TITLE_DOP_SETTING))
        End If
        'メッセージ設定
        Me.HiddenConfirmMessage.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(WORD_MSG_CONFIRM_DISCARD))
        Me.HiddenDeleteMessage.Value = HttpUtility.HtmlEncode(BindParameters(WebWordUtility.GetWord(WORD_MSG_CONFIRM_DELETE),
                                                                            {WebWordUtility.GetWord(WORD_MSG_OPTION_DATA)}))
        Me.HiddenImageDeleteMessage.Value = HttpUtility.HtmlEncode(BindParameters(WebWordUtility.GetWord(WORD_MSG_CONFIRM_DELETE),
                                                                                 {WebWordUtility.GetWord(WORD_IMAGE)}))
        Me.HiddenRequiredMessage.Value = HttpUtility.HtmlEncode(BindParameters(WebWordUtility.GetWord(WORD_ERR_REQUIRED),
                                                                           {WebWordUtility.GetWord(WORD_OPTION_NAME)}))
        Me.HiddenPriceMessage.Value = HttpUtility.HtmlEncode(BindParameters(WebWordUtility.GetWord(WORD_ERR_NUMBER),
                                                                           {WebWordUtility.GetWord(WORD_PRICE)}))
        Me.HiddenUploadMessage.Value = HttpUtility.HtmlEncode(BindParameters(WebWordUtility.GetWord(WORD_ERR_IMAGE_FILE),
                                                                            {WebWordUtility.GetWord(WORD_IMAGE),
                                                                             EXT_JPG_PNG}))
        Me.HiddenUploadFileSizeMessage.Value = HttpUtility.HtmlEncode(BindParameters(WebWordUtility.GetWord(WORD_ERR_IMAGE_FILE_SIZE),
                                                                                    {WebWordUtility.GetWord(WORD_IMAGE),
                                                                                    imageMaxFileSize & FILE_SIZE_UNIT}))
        Me.HiddenGreadMessage.Value = HttpUtility.HtmlEncode(BindParameters(WebWordUtility.GetWord(WORD_ERR_REQUISITE),
                                                                           {WebWordUtility.GetWord(WORD_GRADE)}))
        Me.HiddenDecimalMessage.Value = HttpUtility.HtmlEncode(BindParameters(WebWordUtility.GetWord(WORD_ERR_DECIMAL),
                                                                            {WebWordUtility.GetWord(WORD_PRICE),
                                                                            decimalPointLength}))
    End Sub

#End Region

#Region " リコメンド情報設定処理 "
    ''' <summary>
    ''' リコメンド情報設定処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetRecommendInfo(ByVal recommendListInfo As RecommendInfoList,
                                 ByVal isDisplayOnly As Boolean)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        If isDisplayOnly Then
            Dim recommendBindInfo As New RecommendInfoList
            For Each recommnedInfo As RecommendInfo In recommendListInfo.Root
                If recommnedInfo.RecommendCheck Then
                    recommendBindInfo.Root.Add(recommnedInfo)
                End If
            Next
            'リコメンド情報の設定
            Me.repeaterRecommendInfo.DataSource = recommendBindInfo.Root
            Me.repeaterRecommendInfo.DataBind()
        Else
            'リコメンド情報の設定
            Me.repeaterRecommendInfo.DataSource = recommendListInfo.Root
            Me.repeaterRecommendInfo.DataBind()

            For i = 0 To Me.repeaterRecommendInfo.Items.Count - 1
                DirectCast(Me.repeaterRecommendInfo.Items(i).FindControl("Recommend"), HtmlInputCheckBox).Checked = recommendListInfo.Root.Item(i).RecommendCheck
            Next
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

#End Region

#Region " グレード情報設定処理 "
    ''' <summary>
    ''' グレード情報設定処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetGradeInfo(ByVal gradeListInfo As List(Of TcvWebGradeJson),
                             ByVal gradeConformity As List(Of String),
                             ByVal isDisplayOnly As Boolean)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        If isDisplayOnly Then
            Dim gradeBindInfo As New List(Of TcvWebGradeJson)
            Dim i As Integer = 0
            For i = 0 To gradeConformity.Count - 1
                If gradeConformity(i).Equals(GRADE_ON) Then
                    gradeBindInfo.Add(gradeListInfo(i))
                End If
            Next
            'グレード情報の設定
            Me.repeaterGradeInfo.DataSource = gradeBindInfo
            Me.repeaterGradeInfo.DataBind()
        Else
            'グレード情報の設定
            Me.repeaterGradeInfo.DataSource = gradeListInfo
            Me.repeaterGradeInfo.DataBind()

            For i = 0 To Me.repeaterGradeInfo.Items.Count - 1
                If IsNothing(gradeConformity) Then
                    DirectCast(Me.repeaterGradeInfo.Items(i).FindControl("Grade"), HtmlInputCheckBox).Checked = False
                ElseIf gradeConformity(i).Equals(GRADE_ON) Then
                    DirectCast(Me.repeaterGradeInfo.Items(i).FindControl("Grade"), HtmlInputCheckBox).Checked = True
                End If
            Next
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

#End Region

#End Region

#Region " フッター制御・ヘッダー制御 "

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
    Private Const APPID_CURRENT As String = APPID_MOP_DOP_SETTING_DETAIL

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

    ''' <summary>
    ''' ヘッダーの初期制御設定を行います。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeHeader()

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'マスターページ取得
        Dim masterPage As CommonMasterPage = DirectCast(Me.Master, CommonMasterPage)

        '戻るボタンを活性
        masterPage.IsRewindButtonEnabled = True

        'カスタマーサーチを非活性
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

        'DIST権限かどうかを判定
        If IsDist() Then
            'メインメニューの制御
            If Not IsNothing(mainMenu) Then
                mainMenu.Visible = False
            End If

            'コンテンツメニュー設定の制御
            If Not IsNothing(contentsMenuSetting) Then
                contentsMenuSetting.Visible = True
                contentsMenuSetting.OnClientClick = String.Format(CultureInfo.InvariantCulture, "return onFooterHandler('{0}');", {APPID_CONTENTS_MENU_SETTING})
            End If

            'セールスポイント設定の制御
            If Not IsNothing(salesPointSetting) Then
                salesPointSetting.Visible = True
                salesPointSetting.OnClientClick = String.Format(CultureInfo.InvariantCulture, "return onFooterHandler('{0}');", {APPID_SALES_POINT_SETTING})
            End If
        Else
            'メインメニューの制御
            If Not IsNothing(mainMenu) Then
                mainMenu.Visible = True
                mainMenu.OnClientClick = String.Format(CultureInfo.InvariantCulture, "return onFooterHandler('{0}');", {APPID_MAIN_MENU})
            End If

            'コンテンツメニュー設定の制御
            If Not IsNothing(contentsMenuSetting) Then
                contentsMenuSetting.Visible = False
            End If

            'セールスポイント設定の制御
            If Not IsNothing(salesPointSetting) Then
                salesPointSetting.Visible = False
            End If
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
            tcvSetting.Enabled = False
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub
#End Region

#Region " 共通処理 "

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

#End Region

End Class
