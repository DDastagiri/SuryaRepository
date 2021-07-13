Imports Oracle.DataAccess.Client
Imports System.Data.Common
Imports System.Globalization
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Core

    Public Enum DBQueryTarget
        ''' <summary>
        ''' 接続先：My.MySettings.ConnectionString
        ''' </summary>
        iCROP
        ''' <summary>
        ''' 接続先：My.MySettings.2ndConnectionString
        ''' </summary>
        DMS
    End Enum

    Public Class DBQuery
        Implements IDisposable

#Region "定数"
        Protected Const SqlTraceHeader As String = "SQLTrace"
        Protected Const SqlTraceDelimiter As String = " "
        Protected Const SqlTraceId As String = "ID:"
        Protected Const SqlTraceSql As String = "SQL:"
        Protected Const SqlTraceTime As String = "ResponseTime:"
        Protected Const SqlTraceTimeUnit As String = "s"
        Private Const DBDefaultTimeOut As Integer = 300
        Private Const DBDefaultSlowQuerySecond As Integer = 300
#End Region

#Region "変数"
        ''' <summary>
        ''' クエリID
        ''' </summary>
        ''' <remarks></remarks>
        Private _queryId As String
        ''' <summary>
        ''' コマンド
        ''' </summary>
        ''' <remarks></remarks>
        Private _command As New OracleCommand()

        Protected Property Command() As OracleCommand
            Get
                Return _command
            End Get
            Set(value As OracleCommand)
                _command = value
            End Set
        End Property

        ''' <summary>
        ''' SQL実行開始時間
        ''' </summary>
        ''' <remarks></remarks>
        Private _startTime As DateTime

        Protected Property StartTime() As DateTime
            Get
                Return _startTime
            End Get
            Set(value As DateTime)
                _startTime = value
            End Set
        End Property

        ''' <summary>
        ''' SQL実行終了時間
        ''' </summary>
        ''' <remarks></remarks>
        Private _endTime As Nullable(Of DateTime) = Nothing

        Protected Property EndTime() As Nullable(Of DateTime)
            Get
                Return _endTime
            End Get
            Set(value As Nullable(Of DateTime))
                _endTime = value
            End Set
        End Property

        ''' <summary>
        ''' SQL実行結果
        ''' </summary>
        ''' <remarks></remarks>
        Private _isSuccess As Boolean = False

        Protected Property IsSuccess() As Boolean
            Get
                Return _isSuccess
            End Get
            Set(value As Boolean)
                _isSuccess = value
            End Set
        End Property

        ''' <summary>
        ''' SQL実行時間
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _dbTimeOut As Nullable(Of Integer) = Nothing
        ''' <summary>
        ''' 実行時間エラー判定値
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _slowQuerySeconds As Nullable(Of Integer) = Nothing
        ''' <summary>
        ''' 接続先
        ''' </summary>
        ''' <remarks></remarks>
        Private _targetDB As DBQueryTarget = DBQueryTarget.iCROP

        Protected Property TargetDB() As DBQueryTarget
            Get
                Return _targetDB
            End Get
            Set(value As DBQueryTarget)
                _targetDB = value
            End Set
        End Property
#End Region

#Region "New"
        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="queryId">クエリID</param>
        ''' <remarks></remarks>
        Protected Sub New(ByVal queryId As String)
            Me._queryId = queryId
        End Sub
#End Region

#Region "AddParameterWithTypeValue"
        ''' <summary>
        ''' BindParameter設定(null可用に型を指定)
        ''' </summary>
        ''' <param name="name">パラメータ</param>
        ''' <param name="valueType">型</param>
        ''' <remarks></remarks>
        Public Sub AddParameterWithTypeValue(ByVal name As String, ByVal valueType As OracleDbType, ByVal value As Object)
            _command.Parameters.Add(name, valueType).Value = value
        End Sub
#End Region

#Region "SetParameterValue"
        ''' <summary>
        ''' BindParameter値設定メソッド
        ''' </summary>
        ''' <param name="name">パラメータ名</param>
        ''' <param name="value">値</param>
        ''' <remarks></remarks>
        Public Sub SetParameterValue(ByVal name As String, ByVal value As Object)
            _command.Parameters(name).Value = value
        End Sub
#End Region

#Region "プロパティ"

#Region "CommandText"
        ''' <summary>
        ''' プロパティ(SQL文)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CommandText As String
            Get
                Return _command.CommandText
            End Get
            Set(value As String)
                _command.CommandText = value
            End Set
        End Property
#End Region

#Region "Connection"
        ''' <summary>
        ''' プロパティ(Connection)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Property Connection As DbConnection
            Get
                Return _command.Connection
            End Get
            Set(value As DbConnection)
                _command.Connection = CType(value, OracleConnection)
            End Set
        End Property
#End Region

#Region "DbTimeOut"
        ''' <summary>
        ''' Timeout時間を管理
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Shared ReadOnly Property DBTimeOut As Integer
            Get
                If _dbTimeOut Is Nothing Then

                    _dbTimeOut = DBDefaultTimeOut

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.ConnectionManager
                    If config Is Nothing Then
                        Return _dbTimeOut.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        Return _dbTimeOut.Value
                    End If

                    Dim item As Nullable(Of Integer) = CType(setting.GetValue("SqlCommandTimeout"), Integer)

                    If item Is Nothing Then
                        Return _dbTimeOut.Value
                    End If

                    _dbTimeOut = item.Value
                End If

                Return _dbTimeOut.Value

            End Get
        End Property
#End Region

#Region "SlowQuerySeconds"
        ''' <summary>
        ''' 実行時間エラー判定値
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Shared ReadOnly Property SlowQuerySeconds As Integer
            Get
                If _slowQuerySeconds Is Nothing Then

                    _slowQuerySeconds = DBDefaultSlowQuerySecond

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.ConnectionManager
                    If config Is Nothing Then
                        Return _slowQuerySeconds.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        Return _slowQuerySeconds.Value
                    End If

                    Dim item As Nullable(Of Integer) = CType(setting.GetValue("SlowQuerySeconds"), Integer)

                    If item Is Nothing Then
                        Return _slowQuerySeconds.Value
                    End If

                    _slowQuerySeconds = item.Value
                End If

                Return _slowQuerySeconds.Value

            End Get
        End Property
#End Region

#End Region

#Region "WriteTraceLog"
        ''' <summary>
        ''' トレースログを出力します。
        ''' </summary>
        ''' <remarks>トレースログを出力します。</remarks>
        Protected Sub WriteTraceLog()

            '性能対応 Add Start
            '出力設定ではないので終了
            If Not LoggerUtility.IsEnableTraceLogSetting Then
                Return
            End If

            ''出力対象のレベルではないので終了
            'If Not LoggerUtility.IsEnableTraceLogLevelSetting Then
            '    Return
            'End If
            ''性能対応 Add End

            Logger.Debug(CreateSqlString())

        End Sub
#End Region

#Region "WriteSlowQueryLog"
        ''' <summary>
        ''' 実行時間が遅いSQLを出力します。
        ''' </summary>
        ''' <remarks>エラーログを出力します。</remarks>
        Protected Sub WriteSlowQueryLog()

            If SlowQuerySeconds() < GetExcuteTime().TotalSeconds Then

                Dim log As New StringBuilder
                log.Append(CreateSqlString)
                log.Append(SqlTraceDelimiter)
                log.Append("SlowSql")
                Logger.Error(log.ToString())
            End If

        End Sub
#End Region

#Region "CreateSqlString"
        ''' <summary>
        ''' ログ出力フォーマットを作成します。
        ''' </summary>
        ''' <returns>ログ出力フォーマット</returns>
        ''' <remarks>
        ''' ログ出力フォーマットを作成します。
        ''' ヘッダー、実行時間、ID、SQLを出力します。
        ''' </remarks>
        Protected Function CreateSqlString() As String

            Dim sqlText As New StringBuilder
            sqlText.Append(SqlTraceHeader & SqlTraceDelimiter)
            Dim time As String = Nothing
            If Not _isSuccess Then
                time = "error"
            Else
                time = GetExcuteTime().TotalSeconds.ToString(CultureInfo.InvariantCulture) & SqlTraceTimeUnit
            End If
            sqlText.Append(SqlTraceTime & time & SqlTraceDelimiter)
            sqlText.Append(SqlTraceId & _queryId & SqlTraceDelimiter)
            sqlText.Append(SqlTraceSql & _command.CommandText & LoggerUtility.CreateParameterString(_command.Parameters))

            Return sqlText.ToString

        End Function
#End Region

#Region "CreateParameterString"
        ''' <summary>
        ''' クエリのパラメータを文字列にします。
        ''' </summary>
        ''' <returns>パラメーターの文字列</returns>
        ''' <remarks>クエリのパラメータを文字列にします。</remarks>
        Protected Function CreateParameterString() As String

            Dim param As New StringBuilder
            For i As Integer = 0 To _command.Parameters.Count - 1
                param.Append(" [" & _command.Parameters.Item(i).ToString() & "]" & _command.Parameters.Item(i).Value.ToString())
            Next

            Return param.ToString

        End Function
#End Region

#Region "GetExcuteTime"
        ''' <summary>
        ''' SQL実行時間の計算します。
        ''' </summary>
        ''' <returns>実行時間</returns>
        ''' <remarks>SQL実行時間の計算します。</remarks>
        Protected ReadOnly Property GetExcuteTime() As TimeSpan
            Get
                Return _endTime.Value - _startTime
            End Get
        End Property
#End Region

#Region "WriteSQLTimeLog"
        ''' <summary>
        ''' SQL実行時間を集計します。
        ''' </summary>
        ''' <remarks>SQL実行時間を集計します。</remarks>
        Protected Sub WriteSqlTimeLog()

            If Not _isSuccess Then
                Return
            End If

            'レスポンスログがデバッグモードON、かつSQLエラーでない場合は、SQLレスポンスログの集計対象
            If Not SqlResponseLogger.IsDebug Then
                Return
            End If

            If Not LoggerUtility.IsEnableSqlPerformanceLog Then
                Return
            End If

            Dim time As String = Format(_endTime, "yyyy/MM/dd HH:00")
            SqlResponseLogger.Debug(time, _queryId, GetExcuteTime().TotalMilliseconds, _command.CommandText)
        End Sub
#End Region

        Public Overloads Sub Dispose() Implements IDisposable.Dispose

            Dispose(True)
            GC.SuppressFinalize(Me)

        End Sub

        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

            If disposing Then
                _command.Dispose()
            End If

        End Sub

    End Class
End Namespace
