''' <summary>
''' 見積書・契約書印刷のビジネスロジック用インターフェース
''' </summary>
''' <remarks></remarks>
Public Interface ISC3070204BusinessLogic

    ''' <summary>
    ''' 見積印刷日更新
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function UpdateEstimatePrintDate(ByVal estimateid As Long) As Boolean

    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
    ''' <summary>
    ''' 契約書印刷フラグ更新
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function UpdateContractPrintFlg(ByVal estimateid As Long, ByVal method As String) As Boolean
    '2013/03/08 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

    ''' <summary>
    ''' 契約情報更新(確定時)
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <param name="paymentKbn">支払方法区分</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function UpdateContractInfoByDecide(ByVal estimateid As Long, ByVal paymentKbn As String) As Boolean

    ''' <summary>
    ''' 契約情報更新(キャンセル時)
    ''' </summary>
    ''' <param name="estimateid">見積管理ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function UpdateContractInfoByCancel(ByVal estimateid As Long) As Boolean

End Interface
