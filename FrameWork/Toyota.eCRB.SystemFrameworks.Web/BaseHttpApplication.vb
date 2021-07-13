Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Globalization
Imports System.Web
Imports System.Web.SessionState
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web.Security
Imports System.Diagnostics
Imports System.Security.Principal
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Reflection
Imports System.Web.Caching
Imports System.Web.Configuration
Imports System.Threading
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' e-CRB Framework用のASP.NETパイプライン処理を実装したクラスです。
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Public Class BaseHttpApplication
        Inherits System.Web.HttpApplication

        ''' <summary>
        ''' アプリケーション設定セクション
        ''' </summary>
        ''' <remarks></remarks>
        Private Const APP_NAMESPACE As String = "Toyota.eCRB.SystemFrameworks"

        ''' <summary>
        ''' 最大集約サーバー内販売店識別子
        ''' </summary>
        Private Const MAX_INDIVIDUAL_ID As Byte = 9

        ''' <summary>
        ''' 500エラー
        ''' </summary>
        Private Const WEB_500ERROR As String = "500"

        ''' <summary>
        ''' 500エラー
        ''' </summary>
        Private Const LOCAL_IPADDRESS As String = "127.0.0.1"

        ''' <summary>
        ''' 起動先クラスの実装インタフェース
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LOGIN_HOOK_INTERFACE As String = "ILoginHook"

        ''' <summary>
        ''' 初期化処理結果判定フラグ
        ''' </summary>
        Private Shared _startupResult As Boolean = False

        ''' <summary>
        ''' 初期化例外
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _startupException As Exception

        ''' <summary>
        ''' 初期化例外
        ''' </summary>
        Public Shared ReadOnly Property StartupException As Exception
            Get
                Return _startupException
            End Get
        End Property

        ''' <summary>
        ''' Webアプリケーション起動時の最初のイベントとして発生します。
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Logger.TraceOff = True
                _startupResult = False

                _SetSystemConfiguration()

                ''LoggerUtility.Setlog4netSettingPrameter(String.Empty, SystemConfiguration.Current.GetRuntimeSetting(SystemConfigurationType.ApplicationId), String.Empty)

                'フック取り込み
                _LoadHooks()

                ' アプリケーション共通で必要なデータを読み込み
                ApplicationDataLoader.Load()

                ''SQLResponseLog出力用スレッド生成
                'If SqlResponseLogger.IsDebug() Then
                '    Dim t As New Thread(AddressOf SqlResponseLogger.WaitThread)
                '    t.Start()
                'End If

                _startupResult = True
            Catch ex As Exception
                _startupException = ex
                Try
                    Logger.Error("WebApp initialization failed.", ex)
                Catch ex2 As Exception
                    '設定ファイルの内容に不備がある場合、ログ出力に失敗する可能性がある
                End Try
            Finally
                Logger.TraceOff = False
            End Try
        End Sub


        ''' <summary>
        ''' ASP.NET が要求に応答するときに、
        ''' 実行の HTTP パイプライン チェインの最初のイベントとして発生します。
        ''' </summary>
        ''' <param name="sender">イベントを発生させたクラスのインスタンス</param>
        ''' <param name="e">イベントデータ。イベント処理メソッドの作成ルールから、定義が必要な引数です。ログ稼動出力では利用していません</param>
        ''' <remarks>
        ''' </remarks>
        Private Sub BaseHttpApplication_BeginRequest( _
                                        ByVal sender As Object, _
                                        ByVal e As System.EventArgs) Handles Me.BeginRequest


            '現在のURLパスを取得し、ローカル変数pathEndsに格納する。
            Dim pathEnds As String = Me.Context.Request.Path

            If Not _CheckApplicationIsReady() Then
                'アプリケーションの初期化失敗（エラー画面にリダイレクト）
                Dim reqUrl As String = Request.AppRelativeCurrentExecutionFilePath
                If Not reqUrl.StartsWith("~/Error/", StringComparison.OrdinalIgnoreCase) AndAlso _
                    pathEnds.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) Then
                    Response.Redirect("~/Error/SC3010304.aspx")
                End If
                Return
            End If

            If (pathEnds.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase)) OrElse _
               (pathEnds.EndsWith(".asmx", StringComparison.OrdinalIgnoreCase)) OrElse _
               (pathEnds.EndsWith(".atsc", StringComparison.OrdinalIgnoreCase)) Then

                'コンテキストにアプリIDとアクセスURLを格納する。
                LoggerWebUtility.SetAccessUrlInfo(Me.Context)
            End If

        End Sub

        ''' <summary>
        ''' ASP.NET がイベント ハンドラ (ページ、XML Web サービスなど) の実行を開始する直前に発生します。
        ''' 稼動ログ出力機能では、このメソッドで稼動ログ(処理開始)の出力処理を行います。
        ''' </summary>
        ''' <param name="sender">イベントの発生元。</param>
        ''' <param name="e">イベントに固有のデータ。</param>
        ''' <remarks>このイベントハンドラでは、HTTPリクエスト情報のログの出力を行います。</remarks>
        Private Sub BaseHttpApplication_PreRequestHandlerExecute( _
                                        ByVal sender As Object, _
                                        ByVal e As System.EventArgs) Handles Me.PreRequestHandlerExecute

            '--障害ログ--
            '1.現在のURLパスを取得し、小文字に変換してローカル変数pathEndsに格納する。
            Dim pathEnds As String = Me.Context.Request.Path

            If Not _CheckApplicationIsReady() Then
                Return
            End If

            '2.pathEndsの末尾文字列判定を実施()
            '2.1.末尾文字列が「.aspx」もしくは「.asmx」もしくは「.atsc」の場合
            If (pathEnds.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase)) OrElse _
               (pathEnds.EndsWith(".asmx", StringComparison.OrdinalIgnoreCase)) OrElse _
               (pathEnds.EndsWith(".atsc", StringComparison.OrdinalIgnoreCase)) Then

                '2.1.1.ログインIDを格納するローカル変数を定義する。
                Dim loginId As String

                '2.1.2.現在の権限を格納するローカル変数を定義する。
                Dim selectedRole As String = Nothing

                '2.1.3.ユーザ情報がセッションに存在するか確認する。
                If System.Web.HttpContext.Current.Session IsNot Nothing AndAlso _
                    Me.Session(StaffContext.SESSION_KEY) IsNot Nothing Then

                    '2.1.3.1.ユーザ情報がセッションに存在する場合
                    '2.1.3.1.1.ユーザ情報を取得する。
                    Dim staff As StaffContext = StaffContext.Current

                    '2.1.3.1.2.ログインIDを取得し、ローカル変数loginIdに格納する。
                    loginId = staff.Account

                    ''2.1.3.1.3.現在の権限を取得し、Nothingで無い場合、Stringに型変換して
                    ''  ローカル変数selectedRole に格納する。
                    'If staff.Account IsNot Nothing Then
                    '    selectedRole = Format(staff.Account, "00")
                    'End If
                    selectedRole = CStr(staff.OpeCD)
                Else
                    '2.1.3.2.ユーザ情報がセッションに存在しない場合

                    Dim urlTokens As String() = Me.Context.Request.Url.AbsolutePath.Split(New Char() {"/"c})
                    If (2 <= urlTokens.Length AndAlso urlTokens(urlTokens.Length - 2).Equals("Pages")) Then
                        'Pages配下のページを要求した場合は、ログインページにリダイレクトさせる
                        Dim page As UI.Control = CType(HttpContext.Current.Handler, UI.Control)
                        Logger.Error("Session timeout occured. (redirected to login page)")
                        
                        '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト START

                        'Me.Context.Response.Redirect(page.ResolveClientUrl(CStr(SystemConfiguration.Current.Manager.ScreenUrl.GetSetting(String.Empty).GetValue("Login"))))

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
                            Me.Context.Response.Redirect(page.ResolveClientUrl(CStr(SystemConfiguration.Current.Manager.ScreenUrl.GetSetting(String.Empty).GetValue("GK_Login"))))

                        Else
                            '上記以外の場合

                            'WEBのログインページを設定
                            Me.Context.Response.Redirect(page.ResolveClientUrl(CStr(SystemConfiguration.Current.Manager.ScreenUrl.GetSetting(String.Empty).GetValue("WEB_Login"))))

                        End If
                        
                        '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト END

                        Return
                    Else
                        '2.1.3.2.1.Httpヘッダから、ログインIDを取得するAPIを呼び出す。
                        loginId = LoggerWebUtility.GetLoginIdFromHttpHeader(Me.Context)
                    End If
                End If

                '2.1.4.LoggerWebUtility.SetUserInfoメソッドを呼び出す。
                LoggerWebUtility.SetUserInfo(Me.Context, loginId, selectedRole)
            End If
            '2.2.その他の場合
            ' 処理なし
            '-- 障害ログ--
        End Sub

        ''' <summary>
        ''' 処理されない例外がスローされると発生します。 
        ''' </summary>
        ''' <param name="sender">イベントの発生元。</param>
        ''' <param name="e">イベントに固有のデータ。</param>
        ''' <remarks>
        ''' このイベントハンドラでは、例外情報をログに出力します。
        ''' </remarks>
        Private Sub BaseHttpApplication_Error( _
                                              ByVal sender As Object, _
                                              ByVal e As System.EventArgs) Handles Me.Error

            If Not _CheckApplicationIsReady() Then
                Return
            End If

            ' ハンドルされていないエラーが発生したときに実行するコードです
            Dim ex As Exception = Server.GetLastError
            If (TypeOf ex Is HttpUnhandledException) Then
                ex = ex.InnerException
            End If

            Dim guid As String = System.Guid.NewGuid().ToString

            Try
                Dim controlName As New Text.StringBuilder()
                controlName.Append(vbCrLf)
                For Each ctlName As String In Request.Form
                    If (ctlName IsNot Nothing) AndAlso (ctlName.IndexOf("__", StringComparison.OrdinalIgnoreCase) < 0) Then
                        With controlName
                            .Append(ctlName.Replace("ctl00$", "").Replace("ContentPlaceHolder1$", ""))
                            .Append(":")
                            .Append(Request.Params(ctlName).ToString())
                            .Append(";")
                        End With
                    End If
                Next

                If (TypeOf ex Is HttpException) Then
                    Dim httpEx As HttpException = CType(ex, HttpException)
                    If (httpEx.InnerException IsNot Nothing) Then
                        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} ([{1}] {2})", guid, httpEx.GetHttpCode(), httpEx.Message) & vbCrLf & controlName.ToString(), httpEx)
                    Else
                        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} ([{1}] {2})", guid, httpEx.GetHttpCode(), httpEx.Message) & vbCrLf & controlName.ToString(), httpEx.InnerException)
                    End If
                Else
                    Logger.Error(guid & vbCrLf & controlName.ToString(), ex)
                End If
                'If (TypeOf ex Is HttpException) Then
                '    Dim httpEx As HttpException = CType(ex, HttpException)
                '    If (httpEx.InnerException IsNot Nothing) Then
                '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} ([{1}] {2})", guid, httpEx.GetHttpCode(), httpEx.Message) & vbCrLf & controlName.ToString(), httpEx)
                '    Else
                '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} ([{1}] {2})", guid, httpEx.GetHttpCode(), httpEx.Message) & vbCrLf & controlName.ToString(), httpEx.InnerException)
                '    End If
                'Else
                '    Logger.Error(guid & vbCrLf & controlName.ToString(), ex)
                'End If

            Catch exp As Exception
                Logger.Error(guid, exp)
            End Try

            'HttpContext.Current.Items(APPLICATION_ERROR_ID) = guid

            _RedirectErrorPage(guid)
        End Sub

        ''' <summary>
        ''' ASP.NET イベント ハンドラー (ページ、XML Web サービスなど) の実行が完了すると発生します。
        ''' </summary>
        ''' <param name="sender">イベントの発生元。</param>
        ''' <param name="e">イベントに固有のデータ。</param>
        ''' <remarks></remarks>
        Private Sub BaseHttpApplication_PostRequestHandlerExecute(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PostRequestHandlerExecute

            '現在のURLパスを取得し、小文字に変換してローカル変数pathEndsに格納する。
            Dim pathEnds As String = Me.Context.Request.Path

            If Not _CheckApplicationIsReady() Then
                Return
            End If

            '--圧縮形式--
            'pathEndsの末尾文字列判定を実施
            '末尾文字列が「.aspx」の場合
            'かつ、Me.Response.ContentTypeがNothingでない場合
            'かつ、Me.Response.ContentTypeが"image/"で始まらない場合
            If pathEnds.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) _
                AndAlso Not String.IsNullOrEmpty(Me.Response.ContentType) _
                AndAlso Not Me.Response.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) Then
                'プラウザが受信可能なエンコード方式を取得する。
                Dim encoding As String = Me.Request.Headers.Get("Accept-Encoding")
                'Nothingでは無い場合
                If encoding IsNot Nothing Then
                    'gzip圧縮形式を含む場合
                    If encoding.Contains("gzip") Then
                        'gzip圧縮形式を設定する。
                        Me.Response.Filter = New Compression.GZipStream(Me.Response.Filter, Compression.CompressionMode.Compress)
                        'HTTPヘッダに圧縮形式を設定する。
                        Me.Response.AppendHeader("Content-Encoding", "gzip")
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' アプリケーションの終了時に呼び出されます。
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
            If Not _CheckApplicationIsReady() Then
                Return
            End If

            If SqlResponseLogger.IsDebug() Then
                SqlResponseLogger.Debug(1)
            End If
        End Sub


        Private Function _CheckApplicationIsReady() As Boolean
            Return _startupResult
        End Function

        Private Sub _SetSystemConfiguration()

            Dim sysConfig As SystemConfiguration = SystemConfiguration.Current
            Dim section As System.Xml.XmlElement = DirectCast(System.Web.Configuration.WebConfigurationManager.GetSection(APP_NAMESPACE), System.Xml.XmlElement)
            If section Is Nothing Then
                Throw New FileNotFoundException(APP_NAMESPACE & " section was not found.")
            End If

            sysConfig.Manager = New Toyota.eCRB.SystemFrameworks.Configuration.ConfigurationManager(section)
            sysConfig.SetRuntimeSetting(SystemConfigurationType.ApplicationType, CStr(ApplicationType.Web))
            sysConfig.SetRuntimeSetting(SystemConfigurationType.ApplicationId, String.Empty)
            _SetConnectionStringsItem(sysConfig, SystemConfigurationType.iCROPConnectionString, "My.MySettings.ConnectionString")
            _SetConnectionStringsItem(sysConfig, SystemConfigurationType.DMSConnectionString, "My.MySettings.2ndConnectionString")

        End Sub

        Private Sub _SetConnectionStringsItem(ByVal sysConfig As SystemConfiguration, ByVal type As SystemConfigurationType, ByVal name As String)
            Dim value As Object = System.Configuration.ConfigurationManager.ConnectionStrings.Item(name)
            If (value Is Nothing) Then
                Throw New FileNotFoundException(String.Format(CultureInfo.InvariantCulture, "[{0}] is not defined.", name))
            End If

            sysConfig.SetRuntimeSetting(type, value.ToString())
        End Sub

        Private Sub _RedirectErrorPage(ByVal guid As String)
            'Private Sub _RedirectErrorPage()

            'Dim query As String = "?aspxerrorpath=" & HttpUtility.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath)
            Dim query As New StringBuilder
            query.Append("?aspxerrorpath=" & HttpUtility.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath))
            query.Append("&apperrid=" & HttpUtility.UrlEncode(guid))

            'web.configからカスタムエラーページの情報を取得する
            Dim section As Object = WebConfigurationManager.GetSection("system.web/customErrors")
            If section Is Nothing Then
                Return
            End If

            Dim errorSection As CustomErrorsSection = DirectCast(section, CustomErrorsSection)
            If errorSection.Mode = CustomErrorsMode.Off Then
                Return
            ElseIf errorSection.Mode = CustomErrorsMode.RemoteOnly Then
                If HttpContext.Current.Request.UserHostAddress.Equals(LOCAL_IPADDRESS) Then
                    Return
                End If
            End If

            Dim redirectUrl As String
            '500エラー用のカスタムエラーページが設定されているか判定
            If errorSection.Errors.Item(WEB_500ERROR) IsNot Nothing Then
                '設定あり
                redirectUrl = errorSection.Errors.Item(WEB_500ERROR).Redirect
            Else
                '設定なし
                redirectUrl = errorSection.DefaultRedirect
            End If

            '変換したURLを返却
            'HttpContext.Current.Server.Transfer(redirectUrl & query)
            HttpContext.Current.Response.Redirect(redirectUrl & query.ToString)
        End Sub

        ''' <summary>
        ''' フック取り込み
        ''' </summary>
        Private Sub _LoadHooks()
            Dim config As SystemConfiguration = SystemConfiguration.Current
            Dim systemClass As ClassSection = config.Manager.System
            If (systemClass IsNot Nothing) Then
                Dim hooksSection As Setting = systemClass.GetSetting("Hooks")
                If (hooksSection IsNot Nothing) Then
                    For Each item In hooksSection.Item
                        Try
                            Dim asm As Assembly = Assembly.Load(item.Name)

                            Dim className As String = String.Empty
                            For Each type In asm.GetTypes
                                If (item.Value.Equals("Login")) Then
                                    If type.GetInterface(LOGIN_HOOK_INTERFACE) IsNot Nothing Then
                                        className = type.FullName
                                        Exit For
                                    End If
                                End If
                            Next type

                            If (Not String.IsNullOrEmpty(className)) Then
                                Dim hook As Object = asm.CreateInstance(className)
                                config.Hooks.Add(hook)
                                Logger.Info(String.Format(CultureInfo.InvariantCulture, "SystemFramework load {0} as {1} hook", className, item.Value))
                            Else
                                Dim message As String = String.Format(CultureInfo.InvariantCulture, "No {0} implementation found in {1}.", LOGIN_HOOK_INTERFACE, item.Name)
                                Logger.Error(message)
                            End If
                        Catch ex As Exception
                            Logger.Error(String.Format(CultureInfo.InvariantCulture, "SystemFramework couldn't load {0} hook ({1})", item.Value, item.Name), ex)
                        End Try
                    Next

                End If
            End If
        End Sub

    End Class
End Namespace
