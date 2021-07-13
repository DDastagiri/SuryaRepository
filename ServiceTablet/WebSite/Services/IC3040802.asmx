<%@ WebService Language="VB" Class="IC3040802" %>

Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic

' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class IC3040802
    Inherits System.Web.Services.WebService
    
    ''' <summary>
    ''' 未読件数取得処理
    ''' </summary>
    ''' <returns>未読件数</returns>
    ''' <remarks></remarks>
    <WebMethod(EnableSession:=True)> _
    Public Function GetUnreadNotice() As Long
        Dim unreadCount As Long = 0
        Using bizClass As New IC3040802BusinessLogic
            unreadCount = bizClass.GetUnreadNotice()
        End Using
        Return unreadCount
    End Function
End Class