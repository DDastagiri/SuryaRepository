Imports System.Web.Security
Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Globalization

Namespace Toyota.eCRB.SystemFrameworks.Web

    Public Class SC3010301
        Inherits System.Web.UI.Page

        ''' <summary>
        ''' エラーページのLoadイベントを処理します。
        ''' </summary>
        ''' <param name="sender">イベントの発生元。</param>
        ''' <param name="e">イベントに固有のデータ。</param>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If (Me.IsPostBack) Then
                Return
            End If

            'スローされた最後の例外を削除
            Server.ClearError()

            'エラー情報設定
            Dim userId As String = GetUserID()
            Dim windowId As String = GetWindowID()
            Dim sessionId As String = GetSessionID()

            Dim masterContent As ContentPlaceHolder = CType(Master.FindControl("content"), ContentPlaceHolder)
            'If (HttpContext.Current.Items(BaseHttpApplication.APPLICATION_ERROR_ID) IsNot Nothing) Then
            '    CType(masterContent.FindControl("CellIdValue"), TableCell).Text = System.Web.HttpUtility.HtmlEncode(CStr(HttpContext.Current.Items(BaseHttpApplication.APPLICATION_ERROR_ID)))
            'End If
            CType(masterContent.FindControl("CellIdValue"), TableCell).Text = System.Web.HttpUtility.HtmlEncode(Me.Request.QueryString("apperrid"))
            CType(masterContent.FindControl("CellTimeValue"), TableCell).Text = System.Web.HttpUtility.HtmlEncode(Format(Now, "yyyy/MM/dd HH:mm:ss.fff"))
            CType(masterContent.FindControl("CellServerValue"), TableCell).Text = System.Web.HttpUtility.HtmlEncode(Server.MachineName)
            CType(masterContent.FindControl("CellUserIdValue"), TableCell).Text = System.Web.HttpUtility.HtmlEncode(userId)
            CType(masterContent.FindControl("CellScreenIdValue"), TableCell).Text = System.Web.HttpUtility.HtmlEncode(windowId)
            CType(masterContent.FindControl("CellSessionIdValue"), TableCell).Text = System.Web.HttpUtility.HtmlEncode(sessionId)

        End Sub

        ''' <summary>
        ''' ユーザIDを取得します。
        ''' </summary>
        ''' <returns>ユーザIDを表す文字列</returns>
        Private Shared Function GetUserID() As String

            Dim userId As String = String.Empty
            If (System.Web.HttpContext.Current.Session IsNot Nothing) Then
                Dim staffContextKey As String _
                    = "Toyota.eCRB.SystemFrameworks.AppService.StaffContext"
                Dim instance As StaffContext _
                    = DirectCast(System.Web.HttpContext.Current.Session(staffContextKey),  _
                        StaffContext)
                If (instance IsNot Nothing) Then
                    userId = StaffContext.Current.Account
                End If
            End If

            Return userId

        End Function


        ''' <summary>
        ''' セッションIDを取得します。
        ''' </summary>
        ''' <returns>セッションIDを表す文字列</returns>
        Private Shared Function GetSessionID() As String

            Dim sessionId As String = String.Empty

            If (System.Web.HttpContext.Current.Session IsNot Nothing) Then
                sessionId = System.Web.HttpContext.Current.Session.SessionID
            End If

            Return sessionId

        End Function

        ''' <summary>
        ''' 画面IDを取得します。
        ''' </summary>
        ''' <returns>画面ID</returns>
        ''' <remarks></remarks>
        Private Function GetWindowID() As String

            Dim windowId As String = String.Empty

            Dim errorpath = Me.Request.QueryString("aspxerrorpath")
            If Not String.IsNullOrEmpty(errorpath) Then
                Dim pathSeparatorIndex As Integer = errorpath.LastIndexOf("/", StringComparison.OrdinalIgnoreCase)
                If (pathSeparatorIndex <= 0) Then
                    errorpath = errorpath.Substring(pathSeparatorIndex + 1)
                End If

                Dim expansionIndex As Integer = errorpath.LastIndexOf(".", StringComparison.OrdinalIgnoreCase)
                If (0 <= expansionIndex) Then
                    errorpath = errorpath.Remove(expansionIndex)
                End If

                windowId = errorpath
            End If

            Return windowId

        End Function

        Protected Sub BackButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim redirectScript As String = String.Format(CultureInfo.InvariantCulture, "var w = window.parent || window; w.location.href = '{0}';", ResolveClientUrl(EnvironmentSetting.LoginUrl))
            ClientScript.RegisterStartupScript(Me.GetType(), "redirect", redirectScript, True)
        End Sub

    End Class

End Namespace
