Namespace Toyota.eCRB.SystemFrameworks.Web
    ''' <summary>
    ''' ログイン処理完了時に呼び出されるフック用メソッドを定義しているインターフェースです。
    ''' </summary>
    ''' <remarks>このインターフェースを実装するクラスは、引数なしのコンストラクタを備えている必要があります。</remarks>
    Public Interface ILoginHook

        ''' <summary>
        ''' ログイン処理完了時に呼び出される処理を実行します。
        ''' </summary>
        Sub HookAfterLogin()

    End Interface
End Namespace
