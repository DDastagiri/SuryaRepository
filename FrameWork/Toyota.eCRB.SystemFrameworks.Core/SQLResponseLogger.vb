'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Text
Imports System.Web
Imports System.Globalization
Imports System.Threading
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace Toyota.eCRB.SystemFrameworks.Core

    Public NotInheritable Class SqlResponseLogger

        ' ''' <summary>
        ' ''' SQL集計ログ
        ' ''' Dictionary(KEY:集計時刻 VALUE(KEY:SQLクエリID VALUE:(KEY:種別 VALUE:詳細)))
        ' ''' </summary>
        'Private Shared _sqlTotalLog As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, Object)))

        ''定数
        'Private Const DICTNAME_COUNT As String = "Counter"
        'Private Const DICTNAME_MINTIME As String = "Mintime"
        'Private Const DICTNAME_MAXTIME As String = "Maxtime"
        'Private Const DICTNAME_TOTALTIME As String = "Totaltime"
        'Private Const DICTNAME_QUERYDETAIL As String = "QueryDetail"

        'Private Const DIGIT_TIME As Integer = 16
        'Private Const DIGIT_SQLID As Integer = 20
        'Private Const DIGIT_COUNT As Integer = 5
        'Private Const DIGIT_MINTIME As Integer = 7
        'Private Const DIGIT_MAXTIME As Integer = 7
        'Private Const DIGIT_AVGTIME As Integer = 7

        ' ''' <summary>
        ' ''' Dictionaryのキー項目
        ' ''' </summary>
        'Private Shared _time As String

        ' ''' <summary>
        ' ''' コードテーブル取得時の排他用に使用するオブジェクト
        ' ''' </summary>
        'Private Shared _lockGetCodeTables As Object = New Object()

        ' ''' <summary>
        ' ''' log4netのLoggerインスタンスを格納します。
        ' ''' </summary>
        'Private Shared _storeLogger As log4net.ILog

        ' ''' <summary>
        ' ''' log4netのLoggerインスタンスを取得します。
        ' ''' </summary>
        'Private Shared ReadOnly Property GetLoggerInstance() As log4net.ILog
        '    Get
        '        LoggerUtility.Setlog4netSettingPrameter(String.Empty, SystemConfiguration.Current.GetRuntimeSetting(SystemConfigurationType.ApplicationId), String.Empty)
        '        '1.クラス変数_storeLoggerのNothing判定()
        '        If IsNothing(_storeLogger) Then
        '            '1.1.Nothingの場合
        '            '1.1.1.log4netのロガーインスタンスを取得し、クラス変数_storeLoggerに格納する。
        '            _storeLogger = log4net.LogManager.GetLogger( _
        '                            System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
        '        End If
        '        '1.2.Nothing以外の場合
        '        '処理なし。

        '        '2._storeLoggerを返す。
        '        Return _storeLogger

        '    End Get
        'End Property

        ''性能対応 Add Start
        'Private Shared _isDebug As Nullable(Of Boolean) = Nothing
        ''性能対応 Add End

        ''' <summary>
        ''' デバッグログを出力する設定になっているか、判定結果を返します。
        ''' デバッグログを出力する設定になっている場合はTrueを返します。
        ''' </summary>
        ''' <returns>デバッグログを出力する設定になっているか、判定結果</returns>
        Public Shared ReadOnly Property IsDebug() As Boolean
            Get
                Return False
                ' ''性能対応 Mod Start
                'If _isDebug Is Nothing Then
                '    _isDebug = GetLoggerInstance.IsDebugEnabled
                'End If
                'Return _isDebug.Value
                ' ''性能対応 Mod Start
            End Get
        End Property

        ''' <summary>
        ''' コンストラクタです。インスタンスを生成させないようにするため、修飾子はPrivateです。
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' 集計ログの生成・加算
        ''' </summary>
        ''' <param name="time">ログ時間</param>
        ''' <param name="queryId">クエリID</param>
        ''' <param name="responseTime">実行時間</param>
        ''' <param name="queryString">SQL</param>
        Public Shared Sub Debug(ByVal time As String, ByVal queryId As String, responseTime As Double, ByVal queryString As String)

            Exit Sub

            ''キー項目
            '_time = time

            ''排他処理を行う
            'SyncLock _lockGetCodeTables

            '    '1.ログの初期化
            '    If _sqlTotalLog Is Nothing Then
            '        _sqlTotalLog = New Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, Object)))
            '    End If

            '    '2.ログデータ生成
            '    'キー存在チェックし、あれば加算・なければ生成
            '    If _sqlTotalLog.ContainsKey(_time) Then
            '        If _sqlTotalLog(_time).ContainsKey(queryId) Then
            '            '加算
            '            '実行回数
            '            _sqlTotalLog.Item(_time).Item(queryId).Item(DICTNAME_COUNT) = (CInt(_sqlTotalLog.Item(_time).Item(queryId).Item(DICTNAME_COUNT)) + 1).ToString(CultureInfo.InvariantCulture)
            '            '最小時間
            '            If responseTime < CType(_sqlTotalLog.Item(time).Item(queryId).Item(DICTNAME_MINTIME), Double) Then
            '                _sqlTotalLog.Item(_time).Item(queryId).Item(DICTNAME_MINTIME) = responseTime
            '            End If
            '            '最大時間
            '            If responseTime > CType(_sqlTotalLog.Item(_time).Item(queryId).Item(DICTNAME_MAXTIME), Double) Then
            '                _sqlTotalLog.Item(_time).Item(queryId).Item(DICTNAME_MAXTIME) = responseTime
            '            End If
            '            'TOTAL時間
            '            _sqlTotalLog.Item(_time).Item(queryId).Item(DICTNAME_TOTALTIME) = CType(_sqlTotalLog.Item(_time).Item(queryId).Item(DICTNAME_TOTALTIME), Double) + responseTime
            '        Else
            '            '追加
            '            _sqlTotalLog.Item(_time).Add(queryId, New Dictionary(Of String, Object))
            '            '実行回数
            '            _sqlTotalLog.Item(_time).Item(queryId).Add(DICTNAME_COUNT, 1)
            '            '最小時間
            '            _sqlTotalLog.Item(_time).Item(queryId).Add(DICTNAME_MINTIME, responseTime)
            '            '最大時間
            '            _sqlTotalLog.Item(_time).Item(queryId).Add(DICTNAME_MAXTIME, responseTime)
            '            'TOTAL時間
            '            _sqlTotalLog.Item(_time).Item(queryId).Add(DICTNAME_TOTALTIME, responseTime)
            '            'クエリ詳細
            '            _sqlTotalLog.Item(_time).Item(queryId).Add(DICTNAME_QUERYDETAIL, queryString)
            '        End If
            '    Else
            '        '追加
            '        _sqlTotalLog.Add(time, New Dictionary(Of String, Dictionary(Of String, Object)))
            '        _sqlTotalLog.Item(_time).Add(queryId, New Dictionary(Of String, Object))
            '        '実行回数
            '        _sqlTotalLog.Item(_time).Item(queryId).Add(DICTNAME_COUNT, 1)
            '        '最小時間
            '        _sqlTotalLog.Item(_time).Item(queryId).Add(DICTNAME_MINTIME, responseTime)
            '        '最大時間
            '        _sqlTotalLog.Item(_time).Item(queryId).Add(DICTNAME_MAXTIME, responseTime)
            '        'TOTAL時間
            '        _sqlTotalLog.Item(_time).Item(queryId).Add(DICTNAME_TOTALTIME, responseTime)
            '        'クエリ詳細
            '        _sqlTotalLog.Item(_time).Item(queryId).Add(DICTNAME_QUERYDETAIL, queryString)
            '    End If
            'End SyncLock

        End Sub

        ''' <summary>
        ''' ログ出力処理
        ''' </summary>
        ''' <param name="appFlg">正常起動フラグ(1:正常,0:アプリ終了)</param>
        Public Shared Sub Debug(ByVal appFlg As Integer)

            Exit Sub

            'If appFlg = 1 Then
            '    '正常時
            '    ThreadTrue()
            'Else
            '    'アプリケーション終了時
            '    Dim t As New Thread(AddressOf ThreadFalse)
            '    t.Start()
            'End If
        End Sub

        ''' <summary>
        ''' 正常起動時のログ出力処理
        ''' </summary>
        Public Shared Sub ThreadTrue()
            Exit Sub

            ''1.変数定義
            'Dim logSb As New StringBuilder()
            'Dim writeValue As Dictionary(Of String, Dictionary(Of String, Object)) = Nothing
            'Dim writeKey As String = ""

            ''2.対象のデータを取得する
            ''排他処理を行う
            'SyncLock _lockGetCodeTables
            '    If _sqlTotalLog.ContainsKey(_time) Then
            '        writeValue = _sqlTotalLog.Item(_time)
            '        writeKey = _time
            '    End If
            'End SyncLock

            ''3.ログ出力
            'If writeValue IsNot Nothing Then
            '    For Each s2 As KeyValuePair(Of String, Dictionary(Of String, Object)) In writeValue
            '        logSb.Append(ControlChars.Tab)
            '        logSb.Append(LoggerUtility.FormatElement(writeKey, DIGIT_TIME))
            '        logSb.Append(ControlChars.Tab)
            '        logSb.Append(LoggerUtility.FormatElementNotDelete(s2.Key, DIGIT_SQLID))
            '        logSb.Append(ControlChars.Tab)
            '        logSb.Append(LoggerUtility.FormatElementNotDelete(s2.Value.Item(DICTNAME_COUNT).ToString(), DIGIT_COUNT))
            '        logSb.Append(ControlChars.Tab)
            '        logSb.Append(LoggerUtility.FormatElementNotDelete(CInt((CType(s2.Value.Item(DICTNAME_TOTALTIME), Double)) / (CType(s2.Value.Item(DICTNAME_COUNT), Integer))).ToString(CultureInfo.InvariantCulture), DIGIT_AVGTIME))
            '        logSb.Append(ControlChars.Tab)
            '        logSb.Append(LoggerUtility.FormatElementNotDelete(CInt(s2.Value.Item(DICTNAME_MINTIME)).ToString(CultureInfo.InvariantCulture), DIGIT_MINTIME))
            '        logSb.Append(ControlChars.Tab)
            '        logSb.Append(LoggerUtility.FormatElementNotDelete(CInt(s2.Value.Item(DICTNAME_MAXTIME)).ToString(CultureInfo.InvariantCulture), DIGIT_MAXTIME))
            '        logSb.Append(ControlChars.Tab)
            '        logSb.Append(s2.Value.Item(DICTNAME_QUERYDETAIL).ToString())

            '        GetLoggerInstance().Debug(logSb.ToString())
            '        logSb.Clear()
            '    Next s2
            'End If

            ''4.オブジェクトから出力済データを削除する
            ''排他処理を行う
            'SyncLock _lockGetCodeTables
            '    If writeValue IsNot Nothing Then
            '        _sqlTotalLog.Remove(writeKey)
            '    End If
            'End SyncLock
        End Sub

        ''' <summary>
        ''' アプリ終了時のログ出力処理
        ''' </summary>
        Public Shared Sub ThreadFalse()
            Exit Sub

            ''1.変数定義
            'Dim logSb As New StringBuilder()
            'Dim writeValue As Dictionary(Of String, Dictionary(Of String, Object)) = Nothing
            'Dim writeKey As String = ""

            ''2.ログ出力
            'If _sqlTotalLog IsNot Nothing Then
            '    For Each s1 As KeyValuePair(Of String, Dictionary(Of String, Dictionary(Of String, Object))) In _sqlTotalLog
            '        writeValue = s1.Value
            '        writeKey = s1.Key
            '        If writeValue IsNot Nothing Then
            '            For Each s2 As KeyValuePair(Of String, Dictionary(Of String, Object)) In writeValue
            '                logSb.Append(ControlChars.Tab)
            '                logSb.Append(LoggerUtility.FormatElement(writeKey, DIGIT_TIME))
            '                logSb.Append(ControlChars.Tab)
            '                logSb.Append(LoggerUtility.FormatElementNotDelete(s2.Key, DIGIT_SQLID))
            '                logSb.Append(ControlChars.Tab)
            '                logSb.Append(LoggerUtility.FormatElementNotDelete(s2.Value.Item(DICTNAME_COUNT).ToString(), DIGIT_COUNT))
            '                logSb.Append(ControlChars.Tab)
            '                logSb.Append(LoggerUtility.FormatElementNotDelete(CInt((CType(s2.Value.Item(DICTNAME_TOTALTIME), Double)) / (CType(s2.Value.Item(DICTNAME_COUNT), Integer))).ToString(CultureInfo.InvariantCulture), DIGIT_AVGTIME))
            '                logSb.Append(ControlChars.Tab)
            '                logSb.Append(LoggerUtility.FormatElementNotDelete(CInt(s2.Value.Item(DICTNAME_MINTIME)).ToString(CultureInfo.InvariantCulture), DIGIT_MINTIME))
            '                logSb.Append(ControlChars.Tab)
            '                logSb.Append(LoggerUtility.FormatElementNotDelete(CInt(s2.Value.Item(DICTNAME_MAXTIME)).ToString(CultureInfo.InvariantCulture), DIGIT_MAXTIME))
            '                logSb.Append(ControlChars.Tab)
            '                logSb.Append(s2.Value.Item(DICTNAME_QUERYDETAIL).ToString())

            '                GetLoggerInstance().Debug(logSb.ToString())
            '                logSb.Clear()
            '            Next s2
            '        End If
            '    Next s1
            'End If
        End Sub

        ''' <summary>
        ''' レスポンスログ出力用スレッド処理
        ''' </summary>
        Public Shared Sub WaitThread()
            Exit Sub

            ''1.XX時01分に処理を起動
            ''現在時刻取得
            'Dim nowDate As DateTime = Now()
            ''現在からXX時01分までの時間を計算(ミリ秒)
            'Dim hourSpan As Integer = 60000 * (60 - nowDate.Minute) + 1000 * (59 - nowDate.Second) + (1000 - nowDate.Millisecond)
            ''スレッド待機
            'Thread.Sleep(hourSpan)
            ''キー項目の引渡し
            '_time = Format(Now().Add(New TimeSpan(-1, 0, 0)), "yyyy/MM/dd HH:00")
            ''レスポンスログ出力処理実行
            'Debug(1)
            'While True
            '    '2.再計算。XX時01分に処理を起動するように処理をLOOP
            '    nowDate = Now()
            '    hourSpan = 60000 * (60 - nowDate.Minute) + 1000 * (59 - nowDate.Second) + (1000 - nowDate.Millisecond)
            '    Thread.Sleep(hourSpan)
            '    _time = Format(Now().Add(New TimeSpan(-1, 0, 0)), "yyyy/MM/dd HH:00")
            '    Debug(1)
            'End While
        End Sub

    End Class
End Namespace