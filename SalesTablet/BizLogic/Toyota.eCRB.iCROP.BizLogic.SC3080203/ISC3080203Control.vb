Public Interface ISC3080203Control

    ''' <summary>
    ''' 活動継続イベント
    ''' </summary>
    Event ContinueActivity As EventHandler

    ''' <summary>
    ''' 活動完了イベント
    ''' </summary>
    Event SuccessActivity As EventHandler

    ''' <summary>
    ''' 更新
    ''' </summary>
    ''' <remarks></remarks>
    Sub UpdateActivityResult()

    ''' <summary>
    ''' 活動結果登録ボタン押下時にコールされるメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Sub RegistActivity()

    ''' <summary>
    ''' 商談画面で活動が変更された際に呼び出されるメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Sub ChangeFollow()

End Interface
