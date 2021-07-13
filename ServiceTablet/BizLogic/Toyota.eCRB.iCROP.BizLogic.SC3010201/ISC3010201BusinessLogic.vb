'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ISC3010201BusinessLogic.vb
'─────────────────────────────────────
'機能： メインメニューのビジネスロジック用インターフェース
'補足： 
'作成： 2012/01/23 TCS 相田
'─────────────────────────────────────

Imports Toyota.eCRB.Common.MainMenu.DataAccess
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010201DataSet

''' <summary>
'''メインメニューのビジネスロジック用インターフェース
''' </summary>
''' <remarks></remarks>
Public Interface ISC3010201BusinessLogic
    ''' <summary>
    ''' 連絡事項を取得する。
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function ReadMessageInfo() As SC3010201MessageDataTable
    ''' <summary>
    ''' RSS情報を取得する。
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function ReadRssInfo() As SC3010201RssDataTable
    ''' <summary>
    ''' 連絡事項を削除する。
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function DeleteMessageInfo(ByVal messageNo As Long) As Boolean
End Interface
