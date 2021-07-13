Imports Toyota.eCRB.Estimate.Order.DataAccess

''' <summary>
''' 契約書印刷のビジネスロジック用インターフェース
''' </summary>
''' <remarks></remarks>
Public Interface ISC3070301BusinessLogic
   
    ''' <summary>
    ''' 契約書印刷フラグ更新
    ''' </summary>
    ''' <param name="tbl">セッションデータテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>更新結果</returns>
    ''' <remarks></remarks>
    Function UpdatePrintFlg(ByVal tbl As SC3070301DataSet.SessionDataTable,ByRef msgId As Integer) As Boolean

    ''' <summary>
    ''' 契約情報の更新（実行ボタン押下時）
    ''' </summary>
    ''' <param name="tbl">セッションデータテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <param name="constractDB">契約情報データテーブル</param>
    ''' <returns>更新結果</returns>
    ''' <remarks></remarks>
    Function UpdateConstractInfoSend(ByVal tbl As SC3070301DataSet.SessionDataTable,
                                            ByRef msgId As Integer,
                                            ByVal constractDB As SC3070301DataSet.ConstractInfoDataTable) As Boolean

    ''' <summary>
    ''' 契約情報の更新(キャンセルボタン押下時）
    ''' </summary>
    ''' <param name="tbl">セッションデータテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>更新結果</returns>
    ''' <remarks></remarks>
    Function UpdateConstractInfoCancel(ByVal tbl As SC3070301DataSet.SessionDataTable,ByRef msgId As Integer) As Boolean
End Interface
