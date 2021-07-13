Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' aspx, asmx のサーバ処理時間を計測するためのHttpModule
    ''' </summary>
    Public Class PerformanceTraceHttpModule
        Implements IHttpModule

        ''' <summary>
        ''' HttpApplication
        ''' </summary>
        Private WithEvents hApp As HttpApplication

        ''' <summary>
        ''' HttpContext の item に開始時間を保持するためのキー名
        ''' </summary>
        ''' <remarks></remarks>
        Private ItemKey As String = "PerformanceTraceHttpModule.StartDatetime"


        ''' <summary>
        ''' モジュールを初期化し、要求を処理できるように準備します。
        ''' </summary>
        ''' <param name="app">HttpApplication</param>
        Sub Init(app As HttpApplication) Implements IHttpModule.Init
            hApp = app
        End Sub

        ''' <summary>
        ''' System.Web.IHttpModule を実装するモジュールで使用されるリソース (メモリを除く) を解放します。
        ''' </summary>
        Sub Dispose() Implements IHttpModule.Dispose
        End Sub


        ''' <summary>
        ''' リクエストを受信して最初に発生するイベントです。
        ''' </summary>
        ''' <param name="sender">イベントを発生させたクラスのインスタンス</param>
        ''' <param name="e">イベントデータ</param>
        ''' <remarks>
        ''' HttpContext.Items("PerformanceTraceHttpModule.StartDatetime")に開始時間を格納します。
        ''' </remarks>
        Private Sub context_BeginRequest(ByVal sender As Object, ByVal e As EventArgs) Handles hApp.BeginRequest
            Dim httpContext As HttpContext = (TryCast(sender, HttpApplication)).Context
            Dim pathEnds As String = httpContext.Request.Path
            If (pathEnds.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase)) OrElse _
               (pathEnds.EndsWith(".asmx", StringComparison.OrdinalIgnoreCase)) Then
                Dim startTime = DateTime.Now
                httpContext.Items(ItemKey) = startTime
                Dim msg As String
                msg = String.Format("BeginRequest: RawUrl={0} Method={1} RequestAt={2:yyyy/MM/dd_HH:mm:ss.fff}" _
                                    , httpContext.Request.Url.ToString() _
                                    , httpContext.Request.HttpMethod _
                                    , startTime)
                Logger.Perform(msg)
            End If
        End Sub

        ''' <summary>
        ''' クライアントのブラウザへデータを送信する直前に発生します。
        ''' </summary>
        ''' <param name="sender">イベントを発生させたクラスのインスタンス</param>
        ''' <param name="e">イベントデータ</param>
        ''' <remarks>
        ''' BeginRequest～EndRequestの処理時間を出力します。閾値を超えた処理はエラー情報を出力します。
        ''' </remarks>
        Private Sub context_EndRequest(ByVal sender As Object, ByVal e As EventArgs) Handles hApp.EndRequest
            Dim endTime As DateTime = DateTime.Now
            Dim httpContext = (TryCast(sender, HttpApplication)).Context
            Dim pathEnds As String = httpContext.Request.Path

            If (pathEnds.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase)) OrElse _
               (pathEnds.EndsWith(".asmx", StringComparison.OrdinalIgnoreCase)) Then
                Dim startTime = CType(httpContext.Items(ItemKey), DateTime)
                '' ログ出力 PerformanceTrace 用のログへ出力
                Dim msg As String
                msg = String.Format("EndRequest: RawUrl={0} Method={1} RequestAt={2:yyyy/MM/dd_HH:mm:ss.fff} Status={3} Milliseconds={4}" _
                                    , httpContext.Request.Url.ToString() _
                                    , httpContext.Request.HttpMethod _
                                    , startTime _
                                    , httpContext.Response.StatusCode _
                                    , (endTime - startTime).TotalMilliseconds)
                Logger.Perform(msg)

                '' 閾値を超えた処理はエラーログへ出力
                If (endTime - startTime).TotalMilliseconds > LoggerUtility.PerformErrorThresholdMilliSecond Then
                    Logger.PerformError(msg)
                End If
            End If
        End Sub
    End Class
End Namespace