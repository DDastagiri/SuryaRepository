Imports System
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Xml
Imports System.Security.Cryptography
Imports System.Text

Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Common.Login.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic


''' <summary>
''' ログイン画面のページクラスです。
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3010101
    Inherits System.Web.UI.Page

#Region "プロパティ"
    ''' <summary>
    ''' DB接続確認済み判断
    ''' </summary>
    ''' <value>成否</value>
    ''' <returns>DB接続結果</returns>
    ''' <remarks></remarks>
    Private Property CheckDb() As Boolean
        Get
            Return ViewState(CHECK_DB)
        End Get
        Set(value As Boolean)
            ViewState(CHECK_DB) = value
        End Set
    End Property
    Private Const CHECK_DB As Boolean = False

#End Region

#Region "PrivateConst"
    ''' <summary>
    ''' メッセージID管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MsgID
        none = -1
        id3 = 3     'ログインボタン
        id4 = 4     '再読込
        id21 = 21     'クルクル対応用のメッセージ
        id901 = 901 'アカウントを入力してください
        id902 = 902 'パスワードを入力してください。
        id903 = 903 'アカウントは6桁以上で入力してください。
        id904 = 904 'この端末でのログインは認められません。
        id905 = 905 '初回アクセスのためユーザID@販売店コードを入力してください。
        id906 = 906 '販売店コードが存在しません。入力情報を確認してください。
        id907 = 907 'Macアドレスの登録が完了しました。
        id908 = 908 '指定のマックアドレスは既に登録されています。入力情報を確認してください。
        id909 = 909 'ユーザーと端末の販売店コードの不整合により、この端末でのログインは認められません。
        id910 = 910 'ID名とパスワードが認識できませんでした。
        id911 = 911 'この時間帯にはシステムを使用できません。
        id912 = 912 '認証処理に失敗しました。システム管理者に問い合わせしてください。
        id913 = 913 'システムへのログインは認められていません。
        id914 = 914 'データベース接続に失敗しました。お手数ですが再読込ボタンをクリックしてください。
        id915 = 915 '認証処理に失敗しました。システム管理者に問い合わせしてください。(ポップアップメッセージ用)
        id916 = 916 '指定のユーザは既にログイン中です。
        id917 = 917 '認証処理に失敗しました。システム管理者に問い合わせしてください。(来店実績_ログイン更新失敗時)

    End Enum

    ''' <summary>
    ''' アカウント桁数最小値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ACCOUNT_CNT As Integer = 5

    ''' <summary>
    ''' マックアドレスが取得できない場合の値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UNDEFINED As String = "undefined"

    ''' <summary>
    ''' 共通基盤管理用トップページURLのセッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_TOPPAGE As String = "Toyota.eCRB.SystemFrameworks.Web.BasePage.TopPage"

    ''' <summary>
    ''' ログイン処理実行確認用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VIEWSTATE_LOGINSTATUS As String = "Toyota.eCRB.SystemFrameworks.Web.BasePage.LoginStatus"


    ''' <summary>
    ''' クルクル対応の待ち時
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_REFRESH_TIMER_TIME As String = "REFRESH_TIMER_TIME"

    ''' <summary>
    ''' マスターページ文言取得ID間設定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_MSTPG_DISPLAYID As String = "MASTERPAGEMAIN"                ''マスターページ文言取得ID間設定値
#End Region

#Region "ページイベント"
    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            'システム初期化
            FormsAuthentication.SignOut()

            'セッションのクリア
            Session.Abandon()

            'DB接続判断の初期化
            CheckDb = False

            'コントロール初期表示
            ControlInit()

        End If

        'マックアドレス取得確認/DB接続確認
        CheckConnection()

        '画面遷移
        'If (Not Session(SESSION_TOPPAGE) Is Nothing) Then
        If Not ViewState(VIEWSTATE_LOGINSTATUS) Is Nothing AndAlso ViewState(VIEWSTATE_LOGINSTATUS) Then
            RedirectPage()
        End If

    End Sub
#End Region

#Region "コントロールイベント"
    ''' <summary>
    ''' ログインボタン押下処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub LogOnBtn02_Click(sender As Object, e As System.EventArgs) Handles logOnBtn02.Click

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '入力値検証
        Dim msg As MsgID = ValidateSC310101()
        If msg <> MsgID.none Then
            ErrorProcess(msg)
        Else
            '認証チェック
            Dim account As String = id.Text
            Dim authManager As New AuthenticationManager
            Dim res As LoginResult = authManager.Auth(account, password.Text, hdnMac.Value)

            If res = LoginResult.Success Then
                '認証成功
                RedirectTop(account)
                ViewState(VIEWSTATE_LOGINSTATUS) = True
            Else
                Dim errorNo As Integer = 0
                Select Case res
                    Case LoginResult.MachineCertificationError
                        errorNo = MsgID.id904
                    Case LoginResult.AccountFormatError
                        errorNo = MsgID.id905
                    Case LoginResult.NotExistDLRCDError
                        errorNo = MsgID.id906
                    Case LoginResult.GHDEditComplete
                        errorNo = MsgID.id907
                    Case LoginResult.GHDExistError
                        errorNo = MsgID.id908
                    Case LoginResult.MacAddressError
                        errorNo = MsgID.id909
                    Case LoginResult.LoginError
                        errorNo = MsgID.id910
                    Case LoginResult.LoginTimeError
                        errorNo = MsgID.id911
                    Case LoginResult.CreateSessionError
                        errorNo = MsgID.id915
                    Case LoginResult.DuplicateError
                        errorNo = MsgID.id916
                        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
                    Case LoginResult.GetLockError
                        errorNo = MsgID.id914
                        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
                End Select

                ErrorProcess(errorNo)
            End If
        End If

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
    End Sub

    ''' <summary>
    ''' 再読込ボタン押下処理(DB用)
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub BtnRefresh_Click(sender As Object, e As System.EventArgs) Handles btnRefresh.Click
        CheckDBConnection()
    End Sub

    ''' <summary>
    ''' 再表示ボタン(隠しボタン)押下時
    ''' </summary>
    ''' <param name="sender">ページオブジェクト</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    Protected Sub refreshButton_Click(sender As Object, e As System.EventArgs) Handles refreshButton.Click

        '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト START

        ''WebConfigよりﾛｸﾞｲﾝページのURLを取得
        'Dim url As String = SystemConfiguration.Current.Manager.ScreenUrl.GetSetting(String.Empty).GetValue("Login")

        Dim url As String = String.Empty

        '端末情報変数
        Dim userAgentType As String = String.Empty

        'Config変数
        Dim tecnitianConfig As ConfigurationManager = SystemConfiguration.Current.Manager

        'ConfigのUserAgentの設定値分をループ
        For Each userAgent As Item In tecnitianConfig.LoginManager.GetSetting("UserAgent").Item
            '設定値のデータ取得
            Dim userAgentRegEx As New Regex(userAgent.Value)

            'ログイン端末と設定値のデータチェック
            If (userAgentRegEx.IsMatch(HttpContext.Current.Request.UserAgent)) Then
                '一致する場合

                '設定値名を設定
                userAgentType = userAgent.Name

                'ループ終了
                Exit For

            End If

        Next

        'ログイン先情報とログイン端末のチェック
        If (String.Equals("iPod", userAgentType) OrElse String.Equals("iPhone", userAgentType)) Then
            'ログイン端末が「iPod」or「iPhone」の場合

            'GKのログインページを設定
            url = SystemConfiguration.Current.Manager.ScreenUrl.GetSetting(String.Empty).GetValue("GK_Login")

        Else
            '上記以外の場合

            'WEBのログインページを設定
            url = SystemConfiguration.Current.Manager.ScreenUrl.GetSetting(String.Empty).GetValue("WEB_Login")

        End If

        '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト END

        '自身のaspxを再読み込み
        Dim canonicalUrl As String = Me.ResolveUrl(url)

        '画面遷移
        Me.Response.Redirect(canonicalUrl)

    End Sub

#End Region

#Region "Privateメソッド"
    ''' <summary>
    ''' スクリプト実行
    ''' </summary>
    ''' <param name="scriptVal">スクリプトの中身</param>
    ''' <remarks></remarks>
    Private Sub SetScript(ByVal scriptVal As String)
        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType(), "error", scriptVal)

    End Sub

    ''' <summary>
    ''' コントロールセット
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ControlInit()
        'ログインボタンの文言セット
        logOnBtn02.Text = WebWordUtility.GetWord(MsgID.id3)

        '再読込ボタンの文言セット
        btnRefresh.Text = WebWordUtility.GetWord(MsgID.id4)

        ' クルクル対応用のメッセージ・設定値を取得
        Dim sysEnv As New SystemEnvSetting
        loginPG_RefreshTimerTime.Value = sysEnv.GetSystemEnvSetting(C_REFRESH_TIMER_TIME).PARAMVALUE
        loginPG_RefreshTimerMessage1.Value = WebWordUtility.GetWord(C_MSTPG_DISPLAYID, MsgID.id21)

    End Sub


    ''' <summary>
    ''' マックアドレス取得、DB接続確認
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CheckConnection()
        Dim macaddress As String = hdnMac.Value.Replace(UNDEFINED, String.Empty)

        If String.IsNullOrEmpty(macaddress) Then
            'マックアドレス取得スクリプトセット
            Dim sb As New StringBuilder

            sb.Append("<script>")
            sb.Append("  getMacaddress();")
            sb.Append("</script>")

            SetScript(sb.ToString())
        Else
            If macaddress.Equals(Convert.ToInt32(MsgID.id912).ToString(CultureInfo.InvariantCulture)) Then
                'マックアドレス取得失敗
                ErrorProcess(MsgID.id912)
            Else
                'マックアドレス取得後⇒DB接続未確認
                If Not CheckDb Then
                    CheckDBConnection()
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' MacAddressに対応した販売店コードの取得処理
    ''' </summary>
    ''' <remarks>実質はDB接続確認のための処理</remarks>
    Private Sub CheckDBConnection()
        Try
            'コントロール初期化
            ControlRefresh(True)

            'DB接続チェック
            SC3010101BusinessLogic.CheckDBConnection(hdnMac.Value)

            'DB接続成功
            CheckDb = True

        Catch ex As OracleExceptionEx
            ErrorProcess(MsgID.id914)
        End Try
    End Sub

    ''' <summary>
    ''' 入力値検証
    ''' </summary>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Private Function ValidateSC310101() As MsgID
        Dim rtn As MsgID = MsgID.none

        'アカウント桁数チェック
        If Validation.IsCorrectByte(id.Text, ACCOUNT_CNT) Then
            rtn = MsgID.id903
        End If

        Return rtn
    End Function

    ''' <summary>
    ''' 認証後の遷移処理
    ''' </summary>
    ''' <param name="account">入力アカウント</param>
    ''' <remarks></remarks>
    Private Sub RedirectTop(ByVal account As String)
        '遷移先画面の取得
        Dim path As String = String.Empty
        Dim staff As StaffContext = StaffContext.Current
        Dim config As ClassSection = SystemConfiguration.Current.Manager.TopPageUrl
        If config IsNot Nothing Then
            
            '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト START
            'Dim setting As Setting = config.GetSetting(String.Empty)
            'If (setting IsNot Nothing) Then
            '    path = DirectCast(setting.GetValue(staff.OpeCD), String)

            'End If

            '端末情報変数
            Dim userAgentType As String = String.Empty

            'Config変数
            Dim tecnitianConfig As ConfigurationManager = SystemConfiguration.Current.Manager

            'ConfigのUserAgentの設定値分をループ
            For Each userAgent As Item In tecnitianConfig.LoginManager.GetSetting("UserAgent").Item
                '設定値のデータ取得
                Dim userAgentRegEx As New Regex(userAgent.Value)

                'ログイン端末と設定値のデータチェック
                If (userAgentRegEx.IsMatch(HttpContext.Current.Request.UserAgent)) Then
                    '一致する場合

                    '設定値名を設定
                    userAgentType = userAgent.Name

                    'ループ終了
                    Exit For

                End If

            Next

            'TOP画面遷移用変数
            Dim setting As Setting = Nothing

            '端末情報チェック
            If String.Equals("iPad", userAgentType) Then
                'iPadの場合

                'iPadのTOP画面情報設定
                setting = config.GetSetting("iPad")

            ElseIf (String.Equals("iPod", userAgentType) OrElse String.Equals("iPhone", userAgentType)) Then
                '「iPod」「iPhone」の場合

                'iPodのTOP画面情報設定
                setting = config.GetSetting("iPod")

            End If

            'TOP画面遷移情報チェック
            If (setting IsNot Nothing) Then
                '存在する場合

                '権限のTOP画面IDを設定
                path = DirectCast(setting.GetValue(staff.OpeCD), String)

            End If
            
            '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト END

        End If

        '画面遷移処理
        If String.IsNullOrEmpty(path) Then
            '遷移先未設定
            ErrorProcess(MsgID.id913)
            
            '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト START

            'ログアウト状態に戻す
            staff.UpdatePresence("4", "0")
            
            '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト END

        Else
            'トップページURLを共通基盤管理用セッションに退避（遷移情報管理機能にて使用）
            Session(SESSION_TOPPAGE) = path

            login.Style("display") = "none"
            loading.Style("display") = "block"

            Dim sb As New StringBuilder
            '2012/07/06 KN 小澤 STEP2対応 START
            'sb.Append("<script>")
            'sb.Append("  movePage('" & account & "');")
            'sb.Append("</script>")
            Dim isServiceUser As Boolean = IsService(staff.OpeCD)
            sb.Append("<script>")
            sb.Append("  movePage('" & account & "','" & CStr(isServiceUser) & "');")
            sb.Append("</script>")
            '2012/07/06 KN 小澤 STEP2対応 END

            SetScript(sb.ToString())
            
            '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト START

            ViewState(VIEWSTATE_LOGINSTATUS) = True
            
            '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト END

        End If

    End Sub

    ''' <summary>
    ''' 画面遷移処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RedirectPage()
        '来店実績_ログイン処理
        Dim updateFlg As Boolean = True
        '2012/07/06 KN 小澤 STEP2対応 START
        'If VersionInformation.IsEqualOrLaterThan(1, 2, 0) Then
        '    'updateFlg = IIf(hdnUploadFlg.Value.Equals("0"), True, False)
        'End If
        Dim staff As StaffContext = StaffContext.Current
        If VersionInformation.IsEqualOrLaterThan(1, 2, 0) AndAlso Not (IsService(staff.OpeCD)) Then
            updateFlg = IIf(hdnUploadFlg.Value.Equals("0"), True, False)
        End If
        '2012/07/06 KN 小澤 STEP2対応 END

        If (updateFlg) Then
            '認証チケットを作成
            ''Dim account As String = id.Text
            Dim account As String = staff.Account
            FormsAuthentication.SetAuthCookie(account, False)

            SecurityLogger.Security("Login successful:" & StaffContext.Current.Account)

            '画面遷移
            Dim topPageUrl As String = Me.ResolveUrl("~/Pages/" & Session(SESSION_TOPPAGE) & ".aspx")
            Logger.Debug(String.Format("SC3010101.RedirectPage: {0}", topPageUrl))
            Response.Redirect(topPageUrl)

        Else
            ViewState(VIEWSTATE_LOGINSTATUS) = False
            ErrorProcess(MsgID.id917)
        End If
    End Sub

    ''' <summary>
    ''' 後処理（エラー時、警告など）
    ''' </summary>
    ''' <param name="msg">メッセージID</param>
    ''' <remarks></remarks>
    Private Sub ErrorProcess(ByVal msg As MsgID)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Select Case msg
            Case MsgID.id912
                '-----------------------------マックアドレス取得エラー
                'コントロール制御
                ControlRefresh(False)
                btnRefresh.Visible = False

                'メッセージセット
                clError.Text = WebWordUtility.GetWord(msg)

            Case MsgID.id914
                '-----------------------------DB接続エラー
                'コントロール制御
                ControlRefresh(False)
                btnRefresh.Visible = True

                'メッセージセット
                clError.Text = WebWordUtility.GetWord(msg)

            Case MsgID.id907, MsgID.id908
                '-----------------------------GHDユーザ関連
                'アカウントのクリア
                id.Text = String.Empty

                'メッセージ表示
                ShowMessageBox(msg)

            Case MsgID.id915
                '-----------------------------ポップアップ表示用
                'メッセージ表示
                ShowMessageBox(MsgID.id912)

            Case MsgID.id917
                '-----------------------------来店実績_ログイン更新用
                'メッセージ表示
                ShowMessageBox(MsgID.id912)

                'ローディングクリア
                login.Style("display") = "block"
                loading.Style("display") = "none"

            Case Else
                '-----------------------------その他
                'メッセージ表示
                ShowMessageBox(msg)

        End Select

        SecurityLogger.Security("Login Failed:" & Me.id.Text)

        'パスワードのクリア
        password.Text = String.Empty

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================
    End Sub

    ''' <summary>
    ''' 処理続行判断でのコントロール制御
    ''' </summary>
    ''' <param name="flg">処理状態(可能/初期化：true 不可：false)</param>
    ''' <remarks></remarks>
    Private Sub ControlRefresh(ByVal flg As Boolean)
        pnlError.Visible = IIf(flg = False, True, False)
        id.Enabled = IIf(flg = True, True, False)
        password.Enabled = IIf(flg = True, True, False)

    End Sub

#End Region

    ''' <summary>
    ''' バリデーション結果を通知するためのポップアップダイアログを表示したい時に使用します
    ''' </summary>
    ''' <param name="wordNo">表示メッセージ（文言No）</param>
    ''' <param name="wordParam">表示メッセージ（置換文字列）</param>
    ''' <remarks></remarks>
    Protected Sub ShowMessageBox(ByVal wordNo As Integer, ByVal ParamArray wordParam As String())

        Dim word As String = WebWordUtility.GetWord(wordNo)
        If wordParam IsNot Nothing AndAlso wordParam.Length > 0 Then
            word = String.Format(CultureInfo.InvariantCulture, word, wordParam)
        End If
        Dim alert As New StringBuilder
        alert.Append("<script type='text/javascript'>")
        alert.Append("  alert('" & HttpUtility.JavaScriptStringEncode(word) & "')")
        alert.Append("</script>")

        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType, "alert", alert.ToString)

    End Sub

    '2012/07/06 KN 小澤 STEP2対応 START
    ''' <summary>
    ''' サービスユーザーかを判断する
    ''' </summary>
    ''' <param name="operationCD">権限コード</param>
    ''' <returns>True：サービスユーザー、False：セールスユーザー</returns>
    ''' <remarks></remarks>
    Private Function IsService(ByVal operationCD As Operation) As Boolean
        Dim configStaffDivision As ClassSection = SystemConfiguration.Current.Manager.StaffDivision
        Dim settingStaffDivision As Setting = configStaffDivision.GetSetting(String.Empty)
        If "Service".Equals(DirectCast(settingStaffDivision.GetValue(operationCD), String)) Then
            Return True
        Else
            Return False
        End If
    End Function
    '2012/07/06 KN 小澤 STEP2対応 END

End Class
