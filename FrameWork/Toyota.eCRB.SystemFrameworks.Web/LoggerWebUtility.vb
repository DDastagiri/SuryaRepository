'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Text
Imports System.Web
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' オンラインのみで利用する、ログ出力のためのユーティリティ機能を提供するクラスです。
    ''' ログの出力設定は、外部ファイルとして定義されます。
    ''' </summary>
    ''' <remarks>
    ''' メンバにアクセスするために、静的クラスのインスタンスを宣言する必要はありません。
    ''' このクラスはアセンブリ外に公開します。
    ''' このクラスは継承できません。
    ''' </remarks>
    Friend NotInheritable Class LoggerWebUtility

        ''' <summary>
        ''' コンストラクタです。インスタンスを生成させないようにするため、修飾子はPrivateです。
        ''' </summary>
        Private Sub New()

        End Sub

        ' ''' <summary>
        ' ''' Friend Shared Sub SetWebConfig()
        ' ''' </summary>
        'Friend Shared Sub SetWebConfig()

        '    '1.稼動ログのファイル名と、イベントログのソース項目文字列を設定する。		
        '    LoggerUtility.UpdateLogConfiguration()

        'End Sub

        ''' <summary>
        ''' ログ出力に必要なアクセスURLとアプリID情報を取得し、コンテキストに情報を格納する。
        ''' </summary>
        ''' <param name="context">コンテキスト。</param>
        Friend Shared Sub SetAccessUrlInfo(ByVal context As HttpContext)
            '1.現在のURLを取得し、ローカル変数aplIdに格納する。
            Dim aplId As String = context.Request.AppRelativeCurrentExecutionFilePath

            '2.URLからアクセスURL（"/"区切りの一番最後の文字列）を取得し、ローカル変数aplIdに格納する。
            aplId = aplId.Substring(aplId.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1)

            '3.アクセスURLをコンテキストに格納する。キーは、定数LoggerUtility.CONTEXT_KEY_ACCESSURLを利用する。
            context.Items(LoggerUtility.ContextKeyAccessUrl) = aplId

            '4.アクセスURLからアプリIDを取得し、ローカル変数aplIdに格納する。
            aplId = aplId.Remove(aplId.LastIndexOf(".", StringComparison.OrdinalIgnoreCase))

            '5.アプリIDをコンテキストに格納する。キーは、定数LoggerUtility.CONTEXT_KEY_APLIDを利用する。
            context.Items(LoggerUtility.ContextKeyAplId) = aplId
        End Sub

        ''' <summary>
        ''' ログ出力に必要なログインID、ユーザ権限をコンテキストに格納する。
        ''' </summary>
        ''' <param name="context">コンテキスト。</param>
        ''' <param name="loginId">ログインID</param>
        ''' <param name="selectedRole">現在のユーザ権限</param>
        Friend Shared Sub SetUserInfo(ByVal context As HttpContext, _
                                      ByVal loginId As String, _
                                      ByVal selectedRole As String)
            '1.ログインIDをコンテキストに格納する。キーは、定数LoggerUtility.CONTEXT_KEY_LOGINIDを利用する。
            context.Items(LoggerUtility.ContextKeyLoginId) = loginId

            '2.現在の権限をコンテキストに格納する。キーは、定数LoggerUtility.CONTEXT_KEY_SELECTEDROLEを利用する。
            context.Items(LoggerUtility.ContextKeySelectedRole) = selectedRole

        End Sub

        ''' <summary>
        ''' ASP.NET認証情報よりログインIDを取得する。
        ''' </summary>
        ''' <param name="context">コンテキスト。</param>
        ''' <returns>ログインID文字列</returns>
        Friend Shared Function GetLoginIdFromHttpHeader(ByVal context As HttpContext) As String
            '1.ASP.NET認証情報からログインIDを取得します。
            '2.取得した文字列を返します。
            Return context.User.Identity.Name

        End Function

        ' ''' <summary>
        ' ''' Friend Shared Function GetSessionInfo() As String
        ' ''' </summary>
        ' ''' <returns>セッション情報</returns>
        'Friend Shared Function GetSessionInfo() As String
        '    '(後日検討のため、現在はReturn "" だけ実装しておくこと）
        '    Return ""
        'End Function

    End Class

End Namespace