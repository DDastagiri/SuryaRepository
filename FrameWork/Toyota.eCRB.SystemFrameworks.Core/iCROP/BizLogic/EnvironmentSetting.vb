Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports System.Text.RegularExpressions
Imports System.Web

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' 環境設定読み取りクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class EnvironmentSetting

#Region "変数"
        ''' <summary>
        ''' 国コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _countryCode As String = String.Empty

        ''' <summary>
        ''' ログインURL
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _loginUrl As String = String.Empty

        ''' <summary>
        ''' 履歴管理上限件数
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _maxHistoryCount As Nullable(Of Integer) = Nothing

        ''' <summary>
        ''' 履歴管理Sessionサイズ上限
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _maxHistorySize As Nullable(Of Integer) = Nothing

#End Region

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>プライベートコンストラクタです。</remarks>
        Private Sub New()

        End Sub


#Region "Public Shared ReadOnly Property CountryCode As String"
        ''' <summary>
        ''' 国コードを取得します。
        ''' </summary>
        ''' <returns>Web.configに指定されている国コード</returns>
        Public Shared ReadOnly Property CountryCode As String
            Get
                If String.IsNullOrEmpty(_countryCode) Then
                    'configから読み込まれていない場合は、初回読み込み
                    Dim config As ClassSection = SystemConfiguration.Current.Manager.EnvironmentSetting
                    Dim setting As Setting = config.GetSetting(String.Empty)
                    _countryCode = DirectCast(setting.GetValue("CountryCode"), String)
                    config = Nothing
                    setting = Nothing
                End If
                Return _countryCode
            End Get
        End Property
#End Region

#Region "Friend Shared ReadOnly Property LoginUrl As String"
        ''' <summary>
        ''' Web.configよりログインURLを取得します。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property LoginUrl As String
            Get
                '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト START

                'If String.IsNullOrEmpty(_loginUrl) Then
                '    Dim config As ClassSection = SystemConfiguration.Current.Manager.ScreenUrl
                '    If config IsNot Nothing Then
                '        Dim setting As Setting = config.GetSetting(String.Empty)
                '        If (setting IsNot Nothing) Then
                '            _loginUrl = DirectCast(setting.GetValue("Login"), String)

                '        End If
                '    End If
                'End If

                'WebconfigのScreenUrlタグ設定
                Dim config As ClassSection = SystemConfiguration.Current.Manager.ScreenUrl

                'ScreenUrlタグのチェック
                If config IsNot Nothing Then
                    'ScreenUrlタグが存在する場合

                    'ScreenUrlタグのSettingタグ設定
                    Dim setting As Setting = config.GetSetting(String.Empty)

                    'Settingタグ設定のチェック
                    If (setting IsNot Nothing) Then
                        '存在する場合

                        '端末情報変数
                        Dim userAgentType As String = String.Empty

                        '端末情報Config変数
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
                            _loginUrl = DirectCast(setting.GetValue("GK_Login"), String)

                        Else
                            '上記以外の場合

                            'WEBのログインページを設定
                            _loginUrl = DirectCast(setting.GetValue("WEB_Login"), String)

                        End If


                    End If
                End If

                '2014/09/03 TMEJ 小澤 IT9745_NextSTEPサービス サービス業務向け評価用アプリのシステムテスト END

                Return _loginUrl
            End Get
        End Property
#End Region

#Region "Friend Shared ReadOnly Property MaxHistoryCount As Integer"
        ''' <summary>
        ''' Web.configに定義された履歴管理上限件数を取得します。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property MaxHistoryCount As Integer
            Get
                If _maxHistoryCount Is Nothing Then
                    Dim config As ClassSection = SystemConfiguration.Current.Manager.SessionManager
                    If config IsNot Nothing Then
                        Dim setting As Setting = config.GetSetting("UrlHistory")
                        If (setting IsNot Nothing) Then
                            _maxHistoryCount = DirectCast(setting.GetValue("MaxHistoryCount"), Integer)
                        End If
                    End If

                    If _maxHistoryCount Is Nothing Then
                        _maxHistoryCount = 0
                    End If
                End If
                Return _maxHistoryCount.Value
            End Get
        End Property
#End Region

#Region "Friend Shared ReadOnly Property MaxHistorySize As Integer"
        ''' <summary>
        ''' Web.configに定義された履歴管理Sessionサイズ上限を取得します。
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property MaxHistorySize As Integer
            Get
                If _maxHistorySize Is Nothing Then
                    Dim config As ClassSection = SystemConfiguration.Current.Manager.SessionManager
                    If config IsNot Nothing Then
                        Dim setting As Setting = config.GetSetting("UrlHistory")
                        If (setting IsNot Nothing) Then
                            _maxHistorySize = DirectCast(setting.GetValue("MaxHistorySizeKB"), Integer)
                        End If
                    End If

                    If _maxHistorySize Is Nothing Then
                        _maxHistorySize = 0
                    End If
                End If
                Return _maxHistorySize.Value
            End Get
        End Property
#End Region

    End Class
End Namespace