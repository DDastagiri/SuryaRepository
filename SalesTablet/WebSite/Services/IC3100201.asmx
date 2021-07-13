<%@ WebService Language="VB" Class="IC3100201" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Visit.NotDealCustomer.BizLogic

' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace := "http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _  
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class IC3100201
    Inherits System.Web.Services.WebService 
    
    ''' <summary>
    ''' 未対応来店客件数の取得処理
    ''' </summary>
    ''' <returns>未対応来店客件数</returns>
    ''' <remarks>
    ''' 取得に必要な販売店コード、店舗コード、スタッフコードは
    ''' セッションから取得するため引数として不要
    ''' </remarks>
    <WebMethod(EnableSession:=True)> _
    Public Function GetNotDealCount() As Long
        
        Dim notDealCount As Long = 0
        
        ' 未対応来店客件数を取得
        Dim bizClass As New IC3100201BusinessLogic
        notDealCount = bizClass.GetNotDealCount()
        
        Return notDealCount
    End Function

End Class
