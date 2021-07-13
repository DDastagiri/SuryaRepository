'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ISC3190602BusinessLogic.vb
'─────────────────────────────────────
'機能： B/O部品入力 (ビジネス)
'補足： 
'作成： 2014/08/29 TMEJ M.Asano
'更新： 
'─────────────────────────────────────

''' <summary>
''' SC3190602
''' B/O部品入力画面のビジネスロジック用インターフェース
''' コミット行うメソッドを定義します。
''' </summary>
''' <remarks></remarks>
Public Interface ISC3190602BusinessLogic

    ''' <summary>
    ''' B/O情報の登録
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="boInfo">B/O情報</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Function RegisterBoInfo(ByVal dealerCode As String _
                          , ByVal branchCode As String _
                          , ByVal nowDate As Date _
                          , ByVal account As String _
                          , ByVal boInfo As Dictionary(Of String, Object)) As Integer

End Interface