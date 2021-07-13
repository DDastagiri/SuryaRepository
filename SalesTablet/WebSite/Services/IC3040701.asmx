<%@ WebService Language="VB" Class="IC3040701" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Toyota.eCRB.SystemFrameworks.Core

Imports Toyota.eCRB.iCROP.BizLogic.IC3040701

' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace := "http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _  
Public Class IC3040701
    Inherits System.Web.Services.WebService 
    
    ''' <summary>
    ''' 置換済みテンプレート取得処理
    ''' </summary>
    ''' <param name="xmlString">Request XML</param>
    ''' <returns>置換済みテンプレートを含むResponse XML</returns>
    <WebMethod()> _
    Public Function GetReplacedTemplate(ByVal xmlString As String) As IC3040701BusinessLogic.Response
        Logger.Info("IC3040701 GetReplacedTemplateAPI Process Start")

        Using bizLogic As New IC3040701BusinessLogic
            Dim response As IC3040701BusinessLogic.Response = bizLogic.GetReplacedTemplate(xmlString)
            Logger.Info("IC3040701 GetReplacedTemplateAPI Process End")
            Return response
        End Using
    End Function

End Class
