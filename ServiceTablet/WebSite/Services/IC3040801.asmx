<%@ WebService Language="VB" Class="IC3040801" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic.IC3040801BusinessLogic

' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri2.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class IC3040801
    Inherits System.Web.Services.WebService
    
    ''' <summary>
    ''' 通知DB(固有)
    ''' </summary>
    ''' <param name="getXmlString">取得したXML</param>
    ''' <returns>応答インターフェイス　スタッフ作業指示</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function NoticePeculiarAPI(ByVal getXmlString As String) As Response
        
        Dim returnXML As New Response()

        Using bizClass As New IC3040801BusinessLogic
        
            returnXML = bizClass.Notice(getXmlString, NoticeDisposal.Peculiar)

        End Using

        Return returnXML
    End Function
    
    ''' <summary>
    ''' 通知DB(汎用)
    ''' </summary>
    ''' <param name="getXmlString">取得したXML</param>
    ''' <returns>応答インターフェイス　スタッフ作業指示</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function NoticeGeneralPurposeAPI(ByVal getXmlString As String) As Response
        
        Dim returnXML As New Response()

        Using bizClass As New IC3040801BusinessLogic
        
            returnXML = bizClass.Notice(getXmlString, NoticeDisposal.GeneralPurpose)

        End Using

        Return returnXML
    End Function
End Class