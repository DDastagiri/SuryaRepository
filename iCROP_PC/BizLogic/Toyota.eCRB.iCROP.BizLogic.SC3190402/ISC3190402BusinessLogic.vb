'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ISC3190402BusinessLogic.vb
'─────────────────────────────────────
'機能： 部品庫モニター画面
'補足： 
'作成： 2014/09/09 TMEJ Y.Gotoh 部品庫B／O管理に向けた評価用アプリ作成
'更新： 2016/03/16 NSK A.Minagawa TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 $01
'─────────────────────────────────────
Imports Toyota.eCRB.PartsManagement.PSMonitor.DataAccess

''' <summary>
''' 部品庫モニター インタフェース
''' </summary>
''' <remarks></remarks>
Public Interface ISC3190402BusinessLogic

    '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
    ' ''' <summary>
    ' ''' かごの解放
    ' ''' </summary>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="branchCode">店舗コード</param>
    ' ''' <param name="nowDate">現在日付</param>
    ' ''' <param name="account">アカウント</param>
    'Sub ReleaseCage(ByVal dealerCode As String, _
    '                                    ByVal branchCode As String, _
    '                                    ByVal nowDate As Date, _
    '                                    ByVal account As String)

    ' ''' <summary>
    ' ''' 引き取り待ちデータ取得
    ' ''' </summary>
    ' ''' <param name="nowDate">現在日時</param>
    ' ''' <param name="chipAcquisitionMaxCount">MAX取得件数</param>
    ' ''' <returns>データテーブル</returns>
    'Function GetWaitingforTechnicianPickupListData(ByVal nowDate As Date, _
    '                                               ByVal chipAcquisitionMaxCount As Integer, _
    '                                               ByRef selectDataCount As Integer _
    '                                               ) As SC3190402DataSet.AREA04ResDataTable

    ' ''' <summary>
    ' ''' 出庫待ちデータ取得
    ' ''' </summary>
    ' ''' <param name="nowdate">現在日時</param>
    ' ''' <param name="chipAcquisitionMaxCount">MAX取得件数</param>
    ' ''' <returns>データテーブル</returns>
    ' ''' <remarks>ROステータス及びストール利用ステータスを条件にデータを取得する</remarks>
    'Function GetWaitingforPartsIssuingListData(ByVal nowDate As Date, _
    '                                           ByVal chipAcquisitionMaxCount As Integer, _
    '                                           ByRef selectDataCount As Integer _
    '                                           ) As SC3190402DataSet.AREA03DataTable

    ''' <summary>
    ''' かご情報更新
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="area03Data">出庫待ち表示対象データセット</param>
    ''' <param name="area04Data">引き取り待ち表示対象データセット</param>
    ''' <remarks></remarks>
    Sub UpdateCageInfo(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal nowDate As Date, _
                                        ByVal account As String, _
                                        ByRef area03Data As SC3190402DataSet.AREA03DataTable, _
                                        ByRef area04Data As SC3190402DataSet.AREA04ResDataTable)
    '$01 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

End Interface
