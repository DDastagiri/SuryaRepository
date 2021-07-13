Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
'Imports System.Web.Script.Serialization
Imports System.Globalization

Namespace Toyota.eCRB.SystemFrameworks.Web
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class JavaScriptUtility


        Private Const JAVASCRIPT_TYPE_START As String = "<script type=""text/javascript"">"
        Private Const JAVASCRIPT_TYPE_END As String = "</script>"

        Private Sub New()

        End Sub

        ''' <summary>
        ''' <see cref="System.Web.UI.Page"/>オブジェクトに起動時に呼ばれる関数を登録します。
        ''' </summary>
        ''' <param name="targetPage">ページオブジェクト</param>
        ''' <param name="functionName">起動時に呼び出されるJavascript関数名</param>
        ''' <param name="param">関数に渡す引数</param>
        ''' <remarks>
        ''' 同期ポストバック、非同期ポストバック共に使用可能です。<br/>
        ''' 引数paramの値は、エスケープ前の文字列を渡して下さい。
        ''' </remarks>
        Public Shared Sub RegisterStartupFunctionCallScript(ByVal targetPage As Page, ByVal functionName As String, ByVal key As String, ByVal ParamArray param As Object())

            Dim script As String = CreateFunctionCallScript(functionName, param)

            'スクリプト登録
            RegisterStartupScript(targetPage, script, key, True)

        End Sub

        ''' <summary>
        ''' アラートメッセージを起動スクリプトを登録します。
        ''' </summary>
        ''' <param name="targetPage">ページオブジェクト</param>
        ''' <param name="code">エラーコード</param>
        ''' <param name="detail">障害解析用文字列</param>
        ''' <param name="word">表示メッセージ</param>
        ''' <remarks>この関数は事前にjQueryが読み込まれているこをを前提に作成されています。</remarks>
        Friend Shared Sub RegisterAlertMessege(ByVal targetPage As Page, ByVal code As String, ByVal detail As String, ByVal word As String)
            Dim script As String = String.Format(CultureInfo.InvariantCulture, "$(function(){{ {0} }});", CreateFunctionCallScript("icropScript.ShowMessageBox", New String() {code, word, detail}))

            'スクリプト登録
            RegisterStartupScript(targetPage, script, String.Format(CultureInfo.InvariantCulture, "icropScript.ShowMessageBox.{0}", System.Guid.NewGuid()), True)
        End Sub

        ''' <summary>
        ''' <see cref="System.Web.UI.Page"/>オブジェクトに起動スクリプトを登録します。
        ''' </summary>
        ''' <param name="targetPage"></param>
        ''' <param name="script"></param>
        ''' <remarks></remarks>
        Public Shared Sub RegisterStartupScript(ByVal targetPage As Page, ByVal script As String, ByVal key As String, Optional ByVal wrapScriptTag As Boolean = False)

            Dim sb As New StringBuilder

            'scriptタグ開始
            If wrapScriptTag Then
                sb.Append(JAVASCRIPT_TYPE_START).Append(vbCrLf)
            End If
            'スクリプト
            sb.Append(script).Append(vbCrLf)
            'scriptタグ終了
            If wrapScriptTag Then
                sb.Append(JAVASCRIPT_TYPE_END).Append(vbCrLf)
            End If

            If CheckAsyncPostBack(targetPage) Then
                '非同期用
                ScriptManager.RegisterStartupScript(targetPage, targetPage.GetType, key, sb.ToString, False)
            Else
                '通常用
                targetPage.ClientScript.RegisterStartupScript(targetPage.GetType, key, sb.ToString)
            End If

        End Sub

        ''' <summary>
        ''' 現在のリクエストが非同期ポストバックかどうか判定した結果を取得します。
        ''' </summary>
        ''' <param name="targetPage"></param>
        ''' <returns>
        ''' True:非同期ポストバック<br/>
        ''' False:通常のポストバック又は、リダイレクトでの画面遷移
        ''' </returns>
        ''' <remarks></remarks>
        Private Shared Function CheckAsyncPostBack(ByVal targetPage As Page) As Boolean
            Dim sm As ScriptManager

            'スクリプトマネージャーを取得
            sm = ScriptManager.GetCurrent(targetPage)
            If Not (sm Is Nothing) Then
                Return sm.IsInAsyncPostBack
            End If
            sm = Nothing
            Return False

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="functionName"></param>
        ''' <param name="param"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function CreateFunctionCallScript(ByVal functionName As String, ByVal ParamArray param As Object()) As String

            Dim script As New StringBuilder

            script.Append(functionName).Append("(")
            For i As Integer = 0 To param.Length - 1
                '引数
                If i > 0 Then
                    script.Append(",")
                End If

                If TypeOf param(i) Is String Then
                    '文字列
                    script.Append("""").Append(HttpUtility.JavaScriptStringEncode(DirectCast(param(i), String))).Append("""")
                Else
                    '数値
                    script.Append(param(i).ToString)
                End If
            Next

            'セミコロン
            script.Append(");")

            Return script.ToString

        End Function




    End Class
End Namespace