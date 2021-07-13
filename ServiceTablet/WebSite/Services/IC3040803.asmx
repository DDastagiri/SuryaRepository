<%@ WebService Language="VB" Class="IC3040803" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic

' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace := "http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _  
Public Class IC3040803
    Inherits System.Web.Services.WebService

#Region "定数"
    ' 予期せぬエラーコード
    Private Const ExceptionError As Integer = 9999
    
#End Region
    
    ''' <summary>
    ''' ウェブサービス経由でPush送信
    ''' </summary>
    ''' <param name="xsData">受信XML</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function PushForRefresh(ByVal xsData As String) As String
        Using biz As New IC3040803BusinessLogic
            Try
                Return biz.WebServicePush(xsData)
            Catch
                Return biz.CreateRexml(ExceptionError)
            End Try
        End Using
    End Function

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START
    
    ''' <summary>
    ''' WebService経由で着工指示の通知とPushを行う
    ''' </summary>
    ''' <param name="xsData">受信XML</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 本メソッドの作成目的：
    ''' 受付チップをストールに配置するという操作で性能問題が発生。
    ''' 時間のかかる通知とPushの処理をマルチスレッド化する対応を実施するため、
    ''' 本WebServiceのメソッド内で通知とPushの処理を実施し、
    ''' 本メソッドが呼ばれる部分をマルチスレッドで実施することで
    ''' 性能問題となった操作のレスポンス向上を狙う
    ''' ※WebServiceの呼び出しでなく、単にマルチスレッドで処理を実施してしまうと
    ''' 　アプリ基盤のログ出力メソッドやログイン情報取得で例外が発生してしまう    
    ''' </remarks>
    <WebMethod()> _
    Public Function NoticePushJobInstruct(ByVal xsData As String) As String
        
        Using biz As New IC3040803BusinessLogic
            
            Return biz.SendNoticePushJobInstruct(xsData)
        
        End Using
        
    End Function
    
    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END
    
End Class
