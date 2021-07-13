Imports System.Globalization
Imports System.Text
Imports System.Web
Imports System.Web.Configuration
Imports System.Xml
Imports System.Linq
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' ログ出力機能を提供するクラスです。
    ''' ログの出力設定は、外部ファイルとして定義されます。
    ''' </summary>
    ''' <remarks>
    ''' メンバにアクセスするために、静的クラスのインスタンスを宣言する必要はありません。
    ''' このクラスはアセンブリ外に公開します。
    ''' このクラスは継承できません。
    ''' </remarks>
    Public NotInheritable Class Logger

        Private Shared _lock As New Object()

        Private Shared _PerformErrorThreshold As Double

        ''' <summary>
        ''' トレースの実行状態を取得または設定します
        ''' </summary>
        ''' <remarks>
        ''' このフラグはスレッド単位で有効・無効が設定されます。
        ''' TraceOffの制御はtry-finallyで確実に元に戻すようにします。
        ''' TraceOffをtrueにしたままにするとそのスレッドで別の処理が
        ''' 実行された場合にログが出力されない現象が発生します
        ''' </remarks>
        Public Shared Property TraceOff As Boolean
            Get
                Return _traceOff
            End Get
            Set(ByVal value As Boolean)
                _traceOff = value
            End Set
        End Property

        <ThreadStatic()>
        Private Shared _traceOff As Boolean

        Private Const DefaultLogDateTimeFormat = "yyyy/MM/dd_HH:mm:ss.fff"

#Region "New"
        ''' <summary>
        ''' コンストラクタです。インスタンスを生成させないようにするため、修飾子はPrivateです。
        ''' </summary>
        Private Sub New()

        End Sub
#End Region

#Region "[Error]"
        Private Shared ErrorLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' エラーログを出力します。（ログレベル：ERROR）
        ''' </summary>
        ''' <param name="msg">メッセージ</param>
        ''' <param name="ex">エラーの原因となった例外（ある場合のみ指定する）</param>
        ''' <remarks>エラーログを出力します。（ログレベル：ERROR）</remarks>
        Public Shared Sub [Error](ByVal msg As String, Optional ByVal ex As Exception = Nothing)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            ''出力設定ではないので終了
            If Not LoggerUtility.IsEnableErrorLogSetting Then
                Return
            End If

            If ErrorLogDateTimeFormat Is Nothing Then
                Dim listener = LoggerUtility.ErrorLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    ErrorLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    ErrorLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If


            Dim log As New StringBuilder  ''ログ文字列
            log.Append(LoggerUtility.CreateWebHeader())

            log.Append(now.ToString(ErrorLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg & LoggerUtility.LogDelimiter)

            If ex Is Nothing Then
                LoggerUtility.ErrorLoggerInstance.TraceEvent(TraceEventType.Error, TraceCategory.AppError, log.ToString())
                ''instance.Error(log.ToString)
            Else
                If TypeOf ex Is OracleExceptionEx Then
                    Dim oraex As OracleExceptionEx = DirectCast(ex, OracleExceptionEx)
                    log.Append(vbCrLf)
                    log.Append("SQL:" & oraex.CommandText)
                    log.Append(LoggerUtility.CreateParameterString(oraex.Parameters))
                    log.Append(vbCrLf)
                End If
                log.Append(ex.ToString)
                LoggerUtility.ErrorLoggerInstance.TraceEvent(TraceEventType.Error, TraceCategory.AppError, log.ToString)
            End If
        End Sub
#End Region

#Region "Warn"
        Private Shared WarnLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' 警告ログを出力します。（ログレベル：WARN）
        ''' </summary>
        ''' <param name="msg">メッセージ</param>
        ''' <remarks>エラーログを出力します。（ログレベル：WARN）</remarks>
        Public Shared Sub Warn(ByVal msg As String)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            ''出力設定ではないので終了
            If Not LoggerUtility.IsEnableErrorLogSetting Then
                Return
            End If

            If WarnLogDateTimeFormat Is Nothing Then
                Dim listener = LoggerUtility.WarnLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    WarnLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    WarnLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If

            Dim log As New StringBuilder
            ''ヘッダー(セッションID、アカウント、権限、画面ID)まで作成
            log.Append(LoggerUtility.CreateWebHeader())

            log.Append(now.ToString(WarnLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg)

            LoggerUtility.WarnLoggerInstance.TraceEvent(TraceEventType.Warning, TraceCategory.AppWarning, log.ToString)

        End Sub
#End Region

#Region "Info"
        Private Shared ReceiveLogDateTimeFormat As String = Nothing
        Private Shared InfoLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' 情報ログを出力します。（ログレベル：INFO）
        ''' </summary>
        ''' <param name="msg">メッセージ</param>
        ''' <param name="receiveLog">受信ログに出力する場合のみTrue。</param>
        ''' <remarks>ログを出力します。（ログレベル：INFO）</remarks>
        Public Shared Sub Info(ByVal msg As String, Optional ByVal receiveLog As Boolean = False)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            '性能対応 Add Start
            If (receiveLog) Then
                ''出力設定ではないので終了
                If Not LoggerUtility.IsEnableReceiveLogSetting Then
                    Return
                End If
            Else
                ''出力設定ではないので終了
                If Not LoggerUtility.IsEnableInfoLogSetting Then
                    Return
                End If
            End If
            '性能対応 Add End

            Dim instance As TraceLogger
            If (receiveLog) Then
                instance = LoggerUtility.ReceiveLoggerInstance
                If ReceiveLogDateTimeFormat Is Nothing Then
                    Dim listener = LoggerUtility.ReceiveLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                    If listener Is Nothing Then
                        ReceiveLogDateTimeFormat = DefaultLogDateTimeFormat
                    Else
                        ReceiveLogDateTimeFormat = listener.LogDateTimeFormat
                    End If
                End If
            Else
                instance = LoggerUtility.InfoLoggerInstance
                If InfoLogDateTimeFormat Is Nothing Then
                    Dim listener = LoggerUtility.InfoLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                    If listener Is Nothing Then
                        InfoLogDateTimeFormat = DefaultLogDateTimeFormat
                    Else
                        InfoLogDateTimeFormat = listener.LogDateTimeFormat
                    End If
                End If
            End If

            Dim log As New StringBuilder
            ''ヘッダー(セッションID、アカウント、権限、画面ID)まで作成
            log.Append(LoggerUtility.CreateWebHeader())
            If (receiveLog) Then
                log.Append(now.ToString(ReceiveLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            Else
                log.Append(now.ToString(InfoLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            End If
            log.Append(msg)

            instance.TraceEvent(TraceEventType.Information, TraceCategory.AppInformation, log.ToString)

        End Sub
#End Region

#Region "Debug"
        Private Shared DebugLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' トレースログを出力します。（ログレベル：DEBUG）
        ''' </summary>
        ''' <param name="msg">メッセージ</param>
        ''' <remarks>トレースログを出力します。（ログレベル：DEBUG）</remarks>
        Public Overloads Shared Sub Debug(ByVal msg As String)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            ''出力設定ではないので終了
            If Not LoggerUtility.IsEnableTraceLogSetting Then
                Return
            End If

            If DebugLogDateTimeFormat Is Nothing Then
                Dim listener = LoggerUtility.TraceLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    DebugLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    DebugLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If

            Dim log As New StringBuilder

            ''ヘッダー(セッションID、アカウント、権限、画面ID)まで作成
            log.Append(LoggerUtility.CreateWebHeader())

            log.Append(now.ToString(DebugLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg)

            LoggerUtility.TraceLoggerInstance.TraceEvent(TraceEventType.Verbose, TraceCategory.AppDebug, log.ToString)

        End Sub
#End Region

#Region "Perform"
        Private Shared PerformLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' サーバー処理時間をログ出力します。（ログレベル：INFO）
        ''' </summary>
        ''' <param name="msg">メッセージ</param>
        ''' <remarks>ログを出力します。（ログレベル：INFO）</remarks>
        Public Shared Sub Perform(ByVal msg As String)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            ''出力設定ではないので終了
            If Not LoggerUtility.IsEnablePerformLogSetting Then
                Return
            End If

            If PerformLogDateTimeFormat Is Nothing Then
                Dim listener = LoggerUtility.PerformLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    PerformLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    PerformLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If


            Dim log As New StringBuilder
            ''ヘッダー(セッションID、アカウント、権限、画面ID)まで作成
            log.Append(LoggerUtility.CreateWebHeader())

            log.Append(now.ToString(PerformLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg)

            LoggerUtility.PerformLoggerInstance.TraceEvent(TraceEventType.Information, TraceCategory.AppInformation, log.ToString)
        End Sub
#End Region

#Region "PerformError"
        Private Shared PerformErrorLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' サーバー処理時間エラーをログ出力します。（ログレベル：ERROR）
        ''' </summary>
        ''' <param name="msg">メッセージ</param>
        ''' <remarks>ログを出力します。（ログレベル：INFO）</remarks>
        Public Shared Sub PerformError(ByVal msg As String)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            ''出力設定ではないので終了
            If Not LoggerUtility.IsEnablePerformErrorLogSetting Then
                Return
            End If

            If PerformErrorLogDateTimeFormat Is Nothing Then
                Dim listener = LoggerUtility.PerformErrorLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    PerformErrorLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    PerformErrorLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If

            Dim log As New StringBuilder
            ''ヘッダー(セッションID、アカウント、権限、画面ID)まで作成
            log.Append(LoggerUtility.CreateWebHeader())

            log.Append(now.ToString(PerformErrorLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg)

            LoggerUtility.PerformErrorLoggerInstance.TraceEvent(TraceEventType.Error, TraceCategory.ProcessOverThreshold, log.ToString)
        End Sub
#End Region


    End Class
End Namespace