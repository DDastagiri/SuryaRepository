'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Text
Imports System.Web
Imports System.Xml
Imports System.Globalization
Imports System.Web.Configuration
Imports System.Web.SessionState
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Oracle.DataAccess.Client

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' ログ出力のためのユーティリティ機能を提供するクラスです。
    ''' ログの出力設定は、外部ファイルとして定義されます。
    ''' </summary>
    ''' <remarks>
    ''' メンバにアクセスするために、静的クラスのインスタンスを宣言する必要はありません。
    ''' このクラスはアセンブリ外に公開します。このクラスは継承できません。
    ''' </remarks>
    Public NotInheritable Class LoggerUtility

#Region "定数"
        ''' <summary>
        ''' コンテキストに値を格納するときのキー（ログインID）を格納します。
        ''' </summary>
        Public Const ContextKeyLoginId As String = "LoggerUtility.loginId"

        ''' <summary>
        ''' コンテキストに値を格納するときのキー（現在の権限）を格納します。
        ''' </summary>
        Public Const ContextKeySelectedRole As String = "LoggerUtility.selectedRole"

        ''' <summary>
        ''' コンテキストに値を格納するときのキー（アクセスURL）を格納します。
        ''' </summary>
        Public Const ContextKeyAccessUrl As String = "LoggerUtility.accessUrl"

        ''' <summary>
        ''' コンテキストに値を格納するときのキー（アプリID）を格納します。
        ''' </summary>
        Public Const ContextKeyAplId As String = "LoggerUtility.aplId"

        ''' <summary>
        ''' ログのデリミタを格納します。
        ''' </summary>
        Friend Const LogDelimiter As String = " "
        ''' <summary>
        ''' ログのセッションIDのデフォルト文字列を格納します。
        ''' </summary>
        Private Const LogDefaultSessionID As String = "------------------------"
        ''' <summary>
        ''' ログのログインIDのデフォルト文字列を格納します。
        ''' </summary>
        Private Const LogDefaultLoginID As String = "------------"
        ''' <summary>
        ''' ログの権限の桁数を格納します。
        ''' </summary>
        Private Const DigitSelectedRole As Integer = 2
        ''' <summary>
        ''' ログの現在の権限のデフォルト文字列を格納します。
        ''' </summary>
        Private Const LogDefaultSelectedRole As String = "--"
        ''' <summary>
        ''' ログのラベル文字列の桁数を格納します。
        ''' </summary>
        Private Const DigitLabel As Integer = 3
        ''' <summary>
        ''' ログのラベル文字列のフォーマットを格納します。
        ''' </summary>
        Private Const LogLabelFormat As String = "000"
        ''' <summary>
        ''' ログのアプリIDの桁数を格納します。
        ''' </summary>
        Friend Const DigitApliID As Integer = 10
        ''' <summary>
        ''' ログのアプリIDのデフォルト文字列を格納します。
        ''' </summary>
        Friend Const LogDefaultAplID As String = "----------"
        ''' <summary>
        ''' ログのアクセスURLの桁数を格納します。
        ''' </summary>
        Friend Const DigitAccessUrl As Integer = 15
        ''' <summary>
        ''' エンコードのインスタンスを格納します。
        ''' 最大桁数をカウントするときに利用する。
        ''' </summary>
        Private Shared Encoder As Encoding = Encoding.GetEncoding("utf-8")

        ''' <summary>
        ''' configファイルに設定する販売店コードのClass要素名。
        ''' </summary>
        Private Const LoggerClass As String = "Logger"

        ''' <summary>
        ''' configファイルに設定する販売店コードのSetting要素名。
        ''' </summary>
        Private Const LoggerSetting As String = "EventLogInfo"

        ''' <summary>
        ''' configファイルに設定する販売店コードのItem要素名。
        ''' </summary>
        Private Const LoggerItem As String = "DealerCode"

        ''' <summary>
        ''' イベントログのソース項目の接頭語。
        ''' </summary>
        Private Const SourceNameHeader As String = "i-CROP"

        ''' <summary>
        ''' イベントログのソース項目の区切り文字。
        ''' </summary>
        Private Const SourceNameDelimiter As String = "_"

        ''' <summary>
        ''' 比較対象(文字列長0)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const Zero As Integer = 0

        Friend Const OutputEnableSetting As String = "true"

        'Friend Const SettingParameterTraceLogUser = "TraceLogUser"
        'Friend Const SettingParameterBaseDeirectory = "BaseDirectory"
        'Friend Const SettingParameterApplication = "Application"
        'Friend Const SettingParameterReceiveLogDirectory = "ReceiveLogDirectory"

        'Private Const SettingErrorLogggerName As String = "ErrorLogger"
        'Private Const SettingTraceLogggerName As String = "TraceLogger"
        'Private Const SettingReceiveLoggerName As String = "ReceiveLogger"
#End Region

#Region "変数"
        'Private Shared _baseDirectry As String = Nothing

        Private Shared _isEnableErrorLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableWarnLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableInfoLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableTraceLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableReceiveLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableSecurityLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableSqlPerformanceLog As Nullable(Of Boolean) = Nothing
        Private Shared _isEnablePerformLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnablePerformErrorLogSetting As Nullable(Of Boolean) = Nothing

        'Private Shared _isEnableTraceLogUsers As String = Nothing

        Private Shared _ErrorLogger As New TraceLogger("ErrorLog")
        Private Shared _WarnLogger As New TraceLogger("WarnLog")
        Private Shared _InfoLogger As New TraceLogger("InfoLog")
        Private Shared _TraceLogger As New TraceLogger("TraceLog")
        Private Shared _ReceiveLogger As New TraceLogger("ReceiveLog")
        Private Shared _PerformLogger As New TraceLogger("PerformLog")
        Private Shared _PerformErrorLogger As New TraceLogger("PerformErrorLog")
        Private Shared _PerformErrorThreshold As Nullable(Of Integer)

        Private Shared _SessionStateCookieName As String
        ''性能対応 Add Start
        'Private Shared _isEnableInfoErrorLogLevelSetting As Nullable(Of Boolean) = Nothing
        'Private Shared _isEnableInfoReceiveLogLevelSetting As Nullable(Of Boolean) = Nothing

        'Private Shared _isEnableWarnErrorLogLevelSetting As Nullable(Of Boolean) = Nothing

        'Private Shared _isEnableErrorLogLevelSetting As Nullable(Of Boolean) = Nothing

        'Private Shared _isEnableTraceLogLevelSetting As Nullable(Of Boolean) = Nothing
        ''性能対応 Add End
        'Private Shared _errorLoggerInstance As Log4Net.ILog
        'Private Shared _receiveLoggerInstance As Log4Net.ILog
        'Private Shared _traceLoggerInstance As Log4Net.ILog
        'Private Shared _eventLogSource As String = SourceNameHeader

#End Region

#Region "プロパティ"

#Region "ErrorLoggerInstance"
        Friend Shared ReadOnly Property ErrorLoggerInstance() As TraceLogger
            Get
                Return _ErrorLogger
            End Get
        End Property
#End Region

#Region "WarnLoggerInstance"
        Friend Shared ReadOnly Property WarnLoggerInstance() As TraceLogger
            Get
                Return _WarnLogger
            End Get
        End Property
#End Region

#Region "InfoLoggerInstance"
        Friend Shared ReadOnly Property InfoLoggerInstance() As TraceLogger
            Get
                Return _InfoLogger
            End Get
        End Property
#End Region

#Region "ReceiveLoggerInstance"
        Friend Shared ReadOnly Property ReceiveLoggerInstance() As TraceLogger
            Get
                Return _ReceiveLogger
            End Get
        End Property
#End Region

#Region "TraceLoggerInstance"
        Friend Shared ReadOnly Property TraceLoggerInstance() As TraceLogger
            Get
                Return _TraceLogger
            End Get
        End Property
#End Region

#Region "PerformanceTraceLoggerInstance"
        Friend Shared ReadOnly Property PerformLoggerInstance() As TraceLogger
            Get
                Return _PerformLogger
            End Get
        End Property
#End Region

#Region "PerformanceErrorTraceLoggerInstance"
        Friend Shared ReadOnly Property PerformErrorLoggerInstance() As TraceLogger
            Get
                Return _PerformErrorLogger
            End Get
        End Property
#End Region

#End Region


#Region "IsEnableErrorLogSetting"
        ''' <summary>
        ''' エラーログ出力設定
        ''' </summary>
        ''' <returns>Web.configに指定されているエラーログ出力設定</returns>
        Friend Shared ReadOnly Property IsEnableErrorLogSetting() As Boolean
            Get
                If _isEnableErrorLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableErrorLogSetting = False
                        Return _isEnableErrorLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableErrorLogSetting = False
                        Return _isEnableErrorLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableErrorLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableErrorLogSetting = False
                        Return _isEnableErrorLogSetting.Value
                    End If

                    _isEnableErrorLogSetting = True

                End If

                Return _isEnableErrorLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnableWarnLogSetting"
        ''' <summary>
        ''' 警告ログ出力設定
        ''' </summary>
        ''' <returns>Web.configに指定されている警告ログ出力設定</returns>
        Friend Shared ReadOnly Property IsEnableWarnLogSetting() As Boolean
            Get
                If _isEnableWarnLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableWarnLogSetting = False
                        Return _isEnableWarnLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableWarnLogSetting = False
                        Return _isEnableWarnLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableWarnLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableWarnLogSetting = False
                        Return _isEnableWarnLogSetting.Value
                    End If

                    _isEnableWarnLogSetting = True

                End If

                Return _isEnableWarnLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnableInfoLogSetting"
        ''' <summary>
        ''' 情報ログ出力設定
        ''' </summary>
        ''' <returns>Web.configに指定されている情報ログ出力設定</returns>
        Friend Shared ReadOnly Property IsEnableInfoLogSetting() As Boolean
            Get
                If _isEnableInfoLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableInfoLogSetting = False
                        Return _isEnableInfoLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableInfoLogSetting = False
                        Return _isEnableInfoLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableInfoLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableInfoLogSetting = False
                        Return _isEnableInfoLogSetting.Value
                    End If

                    _isEnableInfoLogSetting = True

                End If

                Return _isEnableInfoLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnableTraceLogSetting"
        ''' <summary>
        ''' トレースログ出力設定
        ''' </summary>
        ''' <returns>Web.configに指定されているエラーログ出力設定</returns>
        Friend Shared ReadOnly Property IsEnableTraceLogSetting() As Boolean
            Get
                If _isEnableTraceLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableTraceLogSetting = False
                        Return _isEnableTraceLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableTraceLogSetting = False
                        Return _isEnableTraceLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableTraceLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableTraceLogSetting = False
                        Return _isEnableTraceLogSetting.Value
                    End If

                    _isEnableTraceLogSetting = True
                End If

                Return _isEnableTraceLogSetting.Value

            End Get
        End Property
#End Region

#Region "IsEnableReceiveLogSetting"
        ''' <summary>
        ''' トレースログ出力設定
        ''' </summary>
        ''' <returns>Web.configに指定されているエラーログ出力設定</returns>
        Friend Shared ReadOnly Property IsEnableReceiveLogSetting() As Boolean
            Get
                If _isEnableReceiveLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableReceiveLogSetting = False
                        Return _isEnableReceiveLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableReceiveLogSetting = False
                        Return _isEnableReceiveLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableReceiveLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableReceiveLogSetting = False
                        Return _isEnableReceiveLogSetting.Value
                    End If

                    _isEnableReceiveLogSetting = True
                End If

                Return _isEnableReceiveLogSetting.Value

            End Get
        End Property
#End Region

#Region "IsEnablePerformLogSetting"
        ''' <summary>
        ''' サーバ処理時間 出力設定
        ''' </summary>
        ''' <returns>Web.configに指定されているエラーログ出力設定</returns>
        Public Shared ReadOnly Property IsEnablePerformLogSetting() As Boolean
            Get
                If _isEnablePerformLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnablePerformLogSetting = False
                        Return _isEnablePerformLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnablePerformLogSetting = False
                        Return _isEnablePerformLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnablePerformLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnablePerformLogSetting = False
                        Return _isEnablePerformLogSetting.Value
                    End If

                    _isEnablePerformLogSetting = True

                End If

                Return _isEnablePerformLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnablePerformErrorLogSetting"
        ''' <summary>
        ''' サーバ処理時間が閾値を超えた際に出力するエラー出力設定
        ''' </summary>
        ''' <returns>Web.configに指定されているエラーログ出力設定</returns>
        Public Shared ReadOnly Property IsEnablePerformErrorLogSetting() As Boolean
            Get
                If _isEnablePerformErrorLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnablePerformErrorLogSetting = False
                        Return _isEnablePerformErrorLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnablePerformErrorLogSetting = False
                        Return _isEnablePerformErrorLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnablePerformErrorLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnablePerformErrorLogSetting = False
                        Return _isEnablePerformErrorLogSetting.Value
                    End If

                    _isEnablePerformErrorLogSetting = True

                End If

                Return _isEnablePerformErrorLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnableSecurityLogSetting"
        ''' <summary>
        ''' エラーログ出力設定
        ''' </summary>
        ''' <returns>Web.configに指定されているエラーログ出力設定</returns>
        Public Shared ReadOnly Property IsEnableSecurityLogSetting() As Boolean
            Get
                If _isEnableSecurityLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableSecurityLogSetting = False
                        Return _isEnableSecurityLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableSecurityLogSetting = False
                        Return _isEnableSecurityLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableSecurityLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableSecurityLogSetting = False
                        Return _isEnableSecurityLogSetting.Value
                    End If

                    _isEnableSecurityLogSetting = True

                End If

                Return _isEnableSecurityLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnableSqlPerformanceLog"
        ''' <summary>
        ''' エラーログ出力設定
        ''' </summary>
        ''' <returns>Web.configに指定されているエラーログ出力設定</returns>
        Friend Shared ReadOnly Property IsEnableSqlPerformanceLog() As Boolean
            Get
                If _isEnableSqlPerformanceLog Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableSqlPerformanceLog = False
                        Return _isEnableSqlPerformanceLog.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableSqlPerformanceLog = False
                        Return _isEnableSqlPerformanceLog.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableSqlPerformanceLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableSqlPerformanceLog = False
                        Return _isEnableSqlPerformanceLog.Value
                    End If

                    _isEnableSqlPerformanceLog = True

                End If

                Return _isEnableSqlPerformanceLog.Value
            End Get
        End Property
#End Region

#Region "PerformErrorThresholdMilliSecond"
        Private Const DefaultPerformErrorThreshold As Integer = 5000
        ''' <summary>
        ''' サーバ処理時間超過閾値（ミリ秒）
        ''' </summary>
        ''' <returns>Web.configに指定されているエラーログ出力設定</returns>
        Public Shared ReadOnly Property PerformErrorThresholdMilliSecond() As Integer
            Get
                If _PerformErrorThreshold Is Nothing Then

                    Dim val As String = System.Configuration.ConfigurationManager.AppSettings.[Get]("PerformErrorThreshold")
                    If String.IsNullOrEmpty(val) Then
                        _PerformErrorThreshold = DefaultPerformErrorThreshold
                    Else
                        _PerformErrorThreshold = CType(ComponentModel.TypeDescriptor.GetConverter(GetType(Integer)).ConvertFrom(val), Integer)
                    End If
                End If
                Return _PerformErrorThreshold.Value
            End Get
        End Property
#End Region

#Region "SessionStateCookieName"
        ''' <summary>
        ''' SessionState CookieName 設定
        ''' </summary>
        ''' <returns>Web.configに指定されているエラーログ出力設定</returns>
        Public Shared ReadOnly Property SessionStateCookieName() As String
            Get
                If _SessionStateCookieName Is Nothing Then
                    _SessionStateCookieName = (New System.Web.Configuration.SessionStateSection()).CookieName
                End If
                Return _SessionStateCookieName
            End Get
        End Property
#End Region

#Region "New"
        ''' <summary>
        ''' コンストラクタです。インスタンスを生成させないようにするため、修飾子はPrivateです。
        ''' </summary>
        Private Sub New()


        End Sub
#End Region

#Region "FormatElement"
        ''' <summary>
        ''' 文字列を、指定した文字列長に整形する。不足文字列を半角スペースでパディング、
        ''' もしくは指定よりオーバーした文字列を削除する。
        ''' </summary>
        ''' <param name="value">整形対象の文字列。</param>
        ''' <param name="maxByte">文字列最大バイト数。</param>
        ''' <returns>整形した文字列。</returns>
        Friend Shared Function FormatElement(ByVal value As String, _
                                             ByVal maxByte As Integer) As String

            '1.引数valueの文字列のバイト数を引数maxLengthで引いた値を、ローカル変数spaceLengthに格納する。
            Dim spaceLength As Integer = maxByte - Encoder.GetByteCount(value)

            '2.1 で取得した値を判定する。
            If spaceLength = Zero Then
                '2.1.spaceLengthが0の場合
                ' 引数valueをそのまま返す。
                Return value

            ElseIf 0 < spaceLength Then
                '2.2.spaceLengthが0よりも大きい場合
                ' 引数valueの文字列のバイト数が引数maxByteになるまで右側を空白でパディングし、生成した文字列を返す。
                '2.2.1.ローカル変数returnStr を定義する。初期値は、New StringBuilder(value)とする。
                Dim sbLarge As New StringBuilder(value)

                '2.2.2.returnStrに、spaceLengthの数だけ空白文字を追加する。
                sbLarge.Append(String.Empty.PadRight(spaceLength))

                '2.2.3.作成した文字列を返す。
                Return sbLarge.ToString()

            Else
                '2.3.2.1、2.2 以外の場合
                ' 引数valueの文字列のバイト数が、引数maxByteの数になるように文字列を削除し、整形した文字列を返す。
                '2.3.1.ローカル変数returnStr を定義する。
                ' 初期値は、StringBuilder(LoggerUtility.GetOmittedString(value, maxByte))とする。
                Dim sbElse As New StringBuilder(LoggerUtility.GetOmittedString(value, maxByte))
                '（また、LoggingUtility.GetOmittedStringメソッドで返される値は、1バイト文字列と2バイト文字列の混合の
                ' 文字列の場合、引数maxLengthより1バイト不足する場合があるので、不足したバイトはスペース文字で埋める
                ' 処理を行う必要があるため下記処理を実施する。）

                '2.3.2.引数maxLength から、2.3.1 で生成したShift_JIS文字コードとしたときのバイト数を引き、
                ' ローカル変数lengthに格納する。
                Dim count As Integer = maxByte - Encoder.GetByteCount(sbElse.ToString())

                '2.3.3.countを判定する。
                If 0 < count Then
                    '2.3.3.1.countが0より大きい場合、
                    'returnStrに、countの数だけ空白文字を追加する。
                    sbElse.Append(String.Empty.PadRight(count))
                End If
                '2.3.3.2.その他
                '次の処理を実施。

                '2.3.4.作成した文字列を返す。
                Return sbElse.ToString()
            End If

        End Function
#End Region

#Region "FormatElementNotDelete"
        ''' <summary>
        ''' 文字列を、指定した文字列長に整形する。不足文字列を半角スペースでパディングする。
        ''' </summary>
        ''' <param name="value">整形対象の文字列。</param>
        ''' <param name="maxByte">文字列最大バイト数。</param>
        ''' <returns>整形した文字列。</returns>
        Friend Shared Function FormatElementNotDelete(ByVal value As String, _
                                             ByVal maxByte As Integer) As String

            '1.引数valueの文字列のバイト数を引数maxLengthで引いた値を、ローカル変数spaceLengthに格納する。
            Dim spaceLength As Integer = maxByte - Encoder.GetByteCount(value)

            If 0 < spaceLength Then
                '2.spaceLengthが0よりも大きい場合
                ' 引数valueの文字列のバイト数が引数maxByteになるまで右側を空白でパディングし、生成した文字列を返す。
                '2.1.ローカル変数returnStr を定義する。初期値は、New StringBuilder(value)とする。
                Dim returnStr As New StringBuilder(value)

                '2.2.returnStrに、spaceLengthの数だけ空白文字を追加する。
                returnStr.Append(String.Empty.PadRight(spaceLength))

                '2.3.作成した文字列を返す。
                Return returnStr.ToString()

            Else
                '2以外の場合

                ' 引数valueをそのまま返す。
                Return value
            End If

        End Function
#End Region

#Region "ConvertLabel"
        ' ''' <summary>
        ' ''' ラベル番号を３桁の文字列に整形する。
        ' ''' </summary>
        ' ''' <param name="labelNo">ラベル番号。</param>
        ' ''' <returns>整形した３桁の文字列。</returns>
        'Friend Shared Function ConvertLabel(ByVal labelNo As Integer) As String
        '    '1.labelNoを文字列に変換してローカル変数labelStrに格納する。
        '    Dim labelStr As String = labelNo.ToString(CultureInfo.InvariantCulture)

        '    '2.labelStrの文字列数をカウントし、ローカル変数labelLengthに格納する。
        '    Dim labelLength As Integer = labelStr.Length

        '    '3.labelLengthの数を比較する。
        '    If labelLength = DigitLabel Then
        '        '3.1.3（ログに出力するラベルの文字列数、定数:DIGIT_LABEL）と等しいとき
        '        '1で生成した文字列を返します。
        '        Return labelStr
        '    ElseIf labelLength < DigitLabel Then
        '        '3.2.3（ログに出力するラベルの文字列数、定数:DIGIT_LABEL）より小さいとき

        '        '文字列labelStrが3桁になるように左側を0で埋め、生成した文字列を返す。
        '        Return Format(labelNo, LogLabelFormat)
        '    Else
        '        '3.3.3.1、3.2以外の場合
        '        '文字列labelStrの右側3桁を取得し、取得した文字列を返す。
        '        Return labelStr.Substring(labelLength - DigitLabel, DigitLabel)
        '    End If
        'End Function
#End Region

#Region "GetKeyElementInfo"
        ''' <summary>
        ''' ログ出力文字列を取得する。セッションID、ログインID、ユーザ権限をコンテキストから取得し、文字列に整形する。
        ''' </summary>
        ''' <param name="context">コンテキスト。</param>
        ''' <returns>整形した文字列。</returns>
        Friend Shared Function GetKeyElementInfo(ByVal context As HttpContext) As String
            '1.ログ出力文字列（セッションID、ログインID、ユーザ権限）を取得する。
            '1.1. セッションを取得し、ローカル変数sessionに格納する。
            Dim session As HttpSessionState = context.Session

            '1.2. ローカル編集sesssionId を定義する。
            Dim sessionId As String

            '1.3.セッションの有無を判定する。
            If Not IsNothing(session) Then
                '1.3.1.セッションが存在する場合
                'ローカル変数sessionIdにセッションIDを格納する。
                sessionId = session.SessionID
            Else
                If String.IsNullOrEmpty(SessionStateCookieName) Then
                    '1.3.2.セッションが存在しない場合
                    'ローカル変数sessionIdに、"------------------------" を格納する。（ハイフン- を24つ）
                    sessionId = LogDefaultSessionID
                Else
                    'SessionStateのCookieName でクッキーを検索して、SessionIdの取得を試みる
                    Dim ses = context.Request.Cookies.Get(SessionStateCookieName)
                    If ses Is Nothing Then
                        sessionId = LogDefaultSessionID
                    Else
                        sessionId = ses.Value
                    End If
                End If
            End If

            '1.4.コンテキストから、定数CONTEXT_KEY_LOGINID をキーとして値を取得し、Stringにキャストし、
            'ローカル変数loginIdに格納する。
            Dim loginId As String = DirectCast(context.Items(ContextKeyLoginId), String)

            '1.5.ローカル変数loginIdがNothingであるか判定する。
            If String.IsNullOrEmpty(loginId) Then
                '1.5.1.Nothingの場合
                'ローカル変数loginIdに、"------------" を格納する。（ハイフン- を12つ）
                loginId = LogDefaultLoginID
            End If
            '1.5.2.Nothingでない場合
            '処理なし。


            '1.6.コンテキストから、定数CONTEXT_KEY_SELECTEDROLE をキーとして値を取得し、Stringにキャストし、
            'ローカル変数selectedRoleに格納する。
            Dim selectedRole As String = DirectCast(context.Items(ContextKeySelectedRole), String)

            '1.7.ローカル変数selectedRoleがNothingであるか判定する。
            If IsNothing(selectedRole) Then
                '1.7.1.Nothingの場合
                'ローカル変数selectedRoleに、"--" を格納する。（ハイフン- を2つ）
                selectedRole = LogDefaultSelectedRole
            Else
                '1.7.2.Nothingでない場合
                '現在の権限をローカル変数selectedRoleに格納する。
                '現在の権限が1桁の場合、右側に空白文字を1つ追加する。
                selectedRole = selectedRole.PadRight(DigitSelectedRole)
            End If

            '2.ログ出力用メッセージの生成を行う。
            '2.1.ローカル変数returnInfoを定義し、StringBuilderオブジェクトを生成し、格納する。
            Dim returnInfo As New StringBuilder

            '2.2.ローカル変数returnInfoに、セッションID文字列と、区切り文字" "（LoggerUtility.LOG_DELIM）を追加する。
            returnInfo.Append(sessionId).Append(LoggerUtility.LogDelimiter)

            '2.3.ローカル変数returnInfoに、ログインID文字列と、区切り文字" "（LoggerUtility.LOG_DELIM）を追加する。
            returnInfo.Append(loginId).Append(LoggerUtility.LogDelimiter)

            '2.4.ローカル変数returnInfoに、現在のユーザ権限文字列と、区切り文字" "（LoggerUtility.LOG_DELIM）を追加する。
            'ユーザ権限文字列を追加する際、
            returnInfo.Append(selectedRole).Append(LoggerUtility.LogDelimiter)

            '3.生成した文字列を返す。
            Return returnInfo.ToString()

        End Function
#End Region

#Region "GetOmittedString"
        ''' <summary>
        ''' 指定バイト数（UTF-8にてカウント）分の文字数を返す。余分な文字列は、削除される。
        ''' </summary>
        ''' <param name="value">対象文字列。</param>
        ''' <param name="maxByte">文字列最大バイト数。</param>
        ''' <returns>整形した文字列。</returns>
        Friend Shared Function GetOmittedString(ByVal value As String, _
                                                ByVal maxByte As Integer) As String

            '文字列最大バイト数の半分のバイトサイズを取得する。
            Dim halfByteSize As Integer = maxByte \ 2

            '引数の文字列数を格納する
            Dim valueLength As Integer = value.Length

            '引数の文字列数と、文字列の最大バイト数の半分のバイト数との比較
            If halfByteSize < valueLength Then
                '返却する文字列の文字数を格納する。
                Dim resultValueLength As Integer = halfByteSize

                '返却する文字列のバイト数を格納する。
                Dim resultByteSize As Integer = _
                             Encoder.GetByteCount(value.Substring(0, halfByteSize))

                '文字列最大バイト数の半分の値以降の引数の文字列をChar配列で取得する。
                Dim tailHalfByteChar As Char() = _
                         value.ToCharArray(halfByteSize, valueLength - halfByteSize)

                'Char配列数分、処理を繰り返す。文字を1ずつ取り出し、バイト数を判定する。
                For Each cs As Char In tailHalfByteChar
                    resultByteSize += Encoder.GetByteCount(cs)
                    If maxByte < resultByteSize Then
                        Return value.Substring(0, resultValueLength)
                    Else
                        resultValueLength += 1
                    End If
                Next cs
            End If
            '指定バイト数未満の場合、値をそのまま返す
            Return value
        End Function
#End Region

#Region "EventLogSourceName"
        ' ''' <summary>
        ' ''' イベントログのソース項目に表示する文字列を返す。
        ' ''' </summary>
        'Public Shared ReadOnly Property EventLogSourceName() As String
        '    Get
        '        '1.定数SOURCE_NAME_HEADERと、クラス変数_eventLogSourceの比較。
        '        '1.1.定数SOURCE_NAME_HEADERと、クラス変数_eventLogSourceの文字列が等しい場合。
        '        If SourceNameHeader.Equals(_eventLogSource) Then

        '            '1.1.1.ローカル変数dealerCode（属性:String、
        '            '      初期値: GetDealerCode()の戻り値)を定義します。
        '            Dim dealerCode As String = GetDealerCode()

        '            '1.1.2.ローカル変数dealerCodeのNothing判定。
        '            If dealerCode IsNot Nothing Then
        '                '1.1.2.1.ローカル変数dealerCodeがNothingでない場合、
        '                '        クラス変数_eventLogSourceに、下記で生成した文字列を格納する。
        '                '        _eventLogSource = 定数SOURCE_NAME_HEADER & _
        '                '                           定数SOURCE_NAME_DELIMITER & _
        '                '                           ローカル変数dealerCode
        '                _eventLogSource = SourceNameHeader & _
        '                                    SourceNameDelimiter & _
        '                                    dealerCode
        '            End If

        '        End If
        '        '1.2.上記以外の場合。
        '        '処理なし。

        '        '2.クラス変数_eventLogSourceの値を返す。
        '        Return _eventLogSource
        '    End Get
        'End Property
#End Region

#Region "GetDealerCode"
        ' ''' <summary>
        ' ''' Web.Config、exe.config ファイルに設定されている、販売店コードを取得する。
        ' ''' 販売店コードが取得できない場合はNothingを返す。
        ' ''' </summary>
        ' ''' <returns>販売店コード</returns>
        'Private Shared Function GetDealerCode() As String
        '    '1.ローカル変数config（属性:Toyota.eCRB.SystemFrameworks.Configuration.ClassSection、
        '    '  初期値:ConfigurationManager.GetClassSection(引数: 定数LOGGER_CLASS))を定義します。
        '    Dim config As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
        '        SystemConfiguration.Current.Manager.GetClassSection(LoggerClass)

        '    '2.ローカル変数config の判定。
        '    If config Is Nothing Then
        '        '2.1.ローカル変数configが、Nothingの場合。
        '        '    Nothingを返す。
        '        Return Nothing
        '    End If
        '    '2.2.上記以外の場合。
        '    '    処理なし。

        '    '3.ローカル変数setting（属性:Toyota.eCRB.SystemFrameworks.Configuration.Setting、
        '    '  初期値:ローカル変数config.GetSetting(引数: 定数LOGGER_SETTING))を定義します。
        '    Dim setting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = config.GetSetting(LoggerSetting)

        '    '4.ローカル変数setting の判定。
        '    If setting Is Nothing Then
        '        '4.1.ローカル変数settingが、Nothingの場合。
        '        '    Nothing を返す。
        '        Return Nothing
        '    End If
        '    '4.2.上記以外の場合。
        '    '    処理なし。

        '    '5.ローカル変数dealerCode（属性:Object、
        '    '  初期値:ローカル変数setting.GetValue(引数: 定数LOGGER_ITEM))を定義します。
        '    Dim dealerCode As Object = setting.GetValue(LoggerItem)

        '    '6.ローカル変数dealerCode の判定。
        '    If dealerCode Is Nothing Then
        '        '6.1.ローカル変数dealerCodeが、Nothingの場合。
        '        '    Nothing を返す。
        '        Return Nothing
        '    End If
        '    '6.2.上記以外の場合。
        '    '    処理なし。

        '    '7.ローカル変数dealerCodeをStringにDirectCastし、値を返す。
        '    Return DirectCast(dealerCode, String)
        'End Function
#End Region

#Region "SetLog4netSettingPrameter"
        Public Shared Sub Setlog4netSettingPrameter(ByVal user As String, ByVal app As String, ByVal receiver As String)
            Exit Sub
            '    If String.IsNullOrEmpty(user) Then
            '        log4net.ThreadContext.Properties(SettingParameterTraceLogUser) = String.Empty
            '    Else
            '        log4net.ThreadContext.Properties(SettingParameterTraceLogUser) = "." & user
            '    End If

            '    log4net.ThreadContext.Properties(SettingParameterBaseDeirectory) = LoggerUtility.BaseDeirectory & "\"

            '    If Not String.IsNullOrEmpty(app) Then
            '        app = app & "\"
            '    End If
            '    log4net.ThreadContext.Properties(SettingParameterApplication) = app

            '    If (String.IsNullOrEmpty(receiver)) Then
            '        log4net.ThreadContext.Properties(SettingParameterReceiveLogDirectory) = String.Empty
            '    Else
            '        log4net.ThreadContext.Properties(SettingParameterReceiveLogDirectory) = receiver & "\"
            '    End If

            '    log4net.Config.XmlConfigurator.Configure()

        End Sub
#End Region

#Region "CreateWebHeader"
        Public Shared Function CreateWebHeader() As String

            If Not CStr(ApplicationType.Web).Equals(SystemConfiguration.Current.GetRuntimeSetting(SystemConfigurationType.ApplicationType)) Then
                Return String.Empty
            End If

            Dim context As HttpContext = HttpContext.Current
            Dim log As New StringBuilder

            Dim aplId As String = Nothing
            If Not IsNothing(context) Then
                ''ログインID、セッションID、ユーザ情報を追加
                log.Append(GetKeyElementInfo(context))

                ''アプリIDの取得
                aplId = DirectCast(context.Items(ContextKeyAplId), String)
            End If

            If String.IsNullOrEmpty(aplId) Then
                ''"--------"（8文字のハイフン）をaplIdに格納する。
                aplId = LogDefaultAplID
            Else
                ''文字列を指定桁数で整形する。
                aplId = FormatElement(aplId, DigitApliID)
            End If

            log.Append(aplId)
            log.Append(LogDelimiter)

            Dim accessUrl As String = Nothing
            If Not IsNothing(context) Then
                accessUrl = DirectCast(context.Items(LoggerUtility.ContextKeyAccessUrl), String)
            End If
            If Not IsNothing(accessUrl) Then
                accessUrl = LoggerUtility.GetOmittedString(accessUrl, LoggerUtility.DigitAccessUrl)
                log.Append(accessUrl).Append(LoggerUtility.LogDelimiter)
            End If

            Return log.ToString

        End Function
#End Region

#Region "CreateParameterString"
        ''' <summary>
        ''' クエリのパラメータを文字列にします。
        ''' </summary>
        ''' <returns>パラメーターの文字列</returns>
        ''' <remarks>クエリのパラメータを文字列にします。</remarks>
        Public Shared Function CreateParameterString(ByVal param As OracleParameterCollection) As String

            If param Is Nothing Then
                Return String.Empty
            End If

            Dim paramStr As New StringBuilder
            For i As Integer = 0 To param.Count - 1
                paramStr.Append(" [" & param.Item(i).ToString() & "]" & param.Item(i).Value.ToString())
            Next

            Return paramStr.ToString

        End Function
#End Region

    End Class
End Namespace