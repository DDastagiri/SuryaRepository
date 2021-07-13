Imports Toyota.eCRB.CustomerInfo.Details.DataAccess

'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ISC3080202BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客詳細(商談情報)
'補足： 
'作成： 2011/11/24 TCS 小野
'─────────────────────────────────────


''' <summary>
''' 顧客詳細（顧客情報）のビジネスロジック用インターフェース
''' </summary>
''' <remarks></remarks>
Public Interface ISC3080202BusinessLogic

    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
    ''' <summary>
    ''' 商談条件登録
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function UpdateSalesCondition(ByVal datatableFrom As SC3080202DataSet.SC3080202UpdateSalesConditionFromDataTable, ByVal datatableFromLocal As SC3080202DataSet.SC3080202GetSalesLocalDataTable, ByRef msgId As Integer) As SC3080202DataSet.SC3080202GetSeqnoToDataTable
    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

    ''' <summary>
    ''' 希望車種登録
    ''' </summary>
    ''' <param name="datatablefrom">引数DataTable</param>
    ''' <param name="msgId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function UpdateSelectedSeries(ByVal datatablefrom As SC3080202DataSet.SC3080202UpdateSelectedSeriesFromDataTable,
                                         ByRef msgId As Integer) As SC3080202DataSet.SC3080202GetSeqnoToDataTable
    ''' <summary>
    ''' 商談メモ登録
    ''' </summary>
    ''' <param name="datatablefrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function UpdateSalesMemo(ByVal datatablefrom As SC3080202DataSet.SC3080202UpdateSalesMemoFromDataTable) As SC3080202DataSet.SC3080202GetSeqnoToDataTable

    ''' <summary>
    ''' 台数登録
    ''' </summary>
    ''' <param name="datatablefrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function UpdateSelectedVclCount(ByVal datatablefrom As SC3080202DataSet.SC3080202UpdateSelectedVclCountFromDataTable) As Boolean

    ''' <summary>
    ''' 競合車種登録
    ''' </summary>
    ''' <param name="datatablefrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function UpdateSelectedCompe(ByVal datatablefrom As SC3080202DataSet.SC3080202UpdateSelectedCompeFromDataTable) As SC3080202DataSet.SC3080202GetSeqnoToDataTable

End Interface
