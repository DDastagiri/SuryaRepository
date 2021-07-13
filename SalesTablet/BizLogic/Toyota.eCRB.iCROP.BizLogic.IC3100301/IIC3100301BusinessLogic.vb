Option Strict On
Option Explicit On

''' <summary>
''' 来店実績更新インターフェースビジネスロジックのインターフェース
''' </summary>
''' <remarks></remarks>
Public Interface IIC3100301BusinessLogic

#Region "来店実績更新_ログイン"

    ''' <summary>
    ''' 来店実績更新_ログイン
    ''' </summary>
    ''' <param name="updateId">更新機能ID</param>
    ''' <param name="resultId">終了コード</param>
    ''' <return>更新件数</return>
    ''' <remarks>
    ''' </remarks>
    Function UpdateVisitLogin(ByVal updateId As String, ByRef resultId As Integer) As Integer

#End Region

End Interface
