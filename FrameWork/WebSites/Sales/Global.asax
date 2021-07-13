<%--
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Global.asax
─────────────────────────────────────
機能： Global.asax
補足： 
作成：
更新： 2013/01/17 TCS 神本 GTMC121228116(TO DO一覧のチップ底色表示異常)
'─────────────────────────────────────
 --%>

<%@ Application Inherits="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication" Language="vb" %>

<script RunAt="server">
    <%-- 2013/01/17 TCS 神本 GTMC121228116(TO DO一覧のチップ底色表示異常) START --%>
    Protected Sub Application_EndRequest(sender As Object, e As System.EventArgs)
        If HttpContext.Current.Request.HttpMethod.Equals("POST") Then
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        End If
    End Sub
    <%-- 2013/01/17 TCS 神本 GTMC121228116(TO DO一覧のチップ底色表示異常) END --%>
</script>
