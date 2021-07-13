''' <summary>
''' 未対応来店客のビジネスロジックインターフェースです。
''' </summary>
''' <remarks></remarks>
Public Interface ISC3100201BusinessLogic

    ''' <summary>
    ''' 来店客の対応
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <param name="visitStatus">来店実績ステータス</param>
    ''' <param name="isUpdateDealAccount">対応担当アカウントの更新有無</param>
    ''' <param name="dealAccount">対応担当アカウント</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="updateDate">取得更新日時</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Function UpdateVisitCustomer( _
            ByVal visitSeq As Long, ByVal visitStatus As String, _
            ByVal isUpdateDealAccount As Boolean, ByVal dealAccount As String, _
            ByVal updateAccount As String, ByVal dealerCode As String, ByVal storeCode As String, _
            ByVal updateDate As String) As Integer

End Interface
