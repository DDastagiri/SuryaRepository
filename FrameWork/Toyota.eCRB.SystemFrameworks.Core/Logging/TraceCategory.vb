Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' トレースカテゴリ
    ''' </summary>
    Public Enum TraceCategory
        ''' <summary>既定</summary>
        None = 0
        ''' <summary>処理開始</summary>
        AppStart = 10
        ''' <summary>処理完了</summary>
        AppSuccess = 11
        ''' <summary>エラー</summary>
        AppError = 12
        ''' <summary>エラー</summary>
        AppWarning = 13
        ''' <summary>情報</summary>
        AppInformation = 14
        ''' <summary>デバッグ情報</summary>
        AppDebug = 15
        ''' <summary>デバッグ情報</summary>
        AppProcessingSummary = 16
        ''' <summary>SQL一般</summary>
        SqlGeneral = 100
        ''' <summary>SQL単項目取得</summary>
        SqlScalar = 101
        ''' <summary>SQL検索</summary>
        SqlFill = 102
        ''' <summary>SQL行更新</summary>
        SqlUpdateRow = 103
        ''' <summary>SQLコマンド実行</summary>
        SqlCommand = 104
        ''' <summary>SQLトランザクション開始</summary>
        SqlBeginTransaction = 105
        ''' <summary>SQLコミット</summary>
        SqlCommit = 106
        ''' <summary>SQLロールバック</summary>
        Rollback = 107
        ''' <summary>SQL接続オープン</summary>
        SqlOpen = 108
        ''' <summary>SQL接続クローズ</summary>
        SqlClose = 109
        ''' <summary>SQL接続Dispose</summary>
        SqlDispose = 110
        ''' <summary>SQL接続Dispose</summary>
        SqlStopWatch = 111
        ''' <summary>Web一般</summary>
        WebGeneral = 200
        ''' <summary>HTTPリクエスト</summary>
        WebHttpRequestStart = 201
        ''' <summary>HTTPレスポンス</summary>
        WebHttpResponseEnd = 202
        ''' <summary>画面遷移</summary>
        WebRedirect = 203
        ''' <summary>処理時間の閾値超え時</summary>
        ProcessOverThreshold = 205

        ''' <summary>アプリケーションデバッグ</summary>
        AppTrace = 300

        ''' <summary>ユーザ利用可能開始No</summary>
        UserBase = 1000
    End Enum
End Namespace
