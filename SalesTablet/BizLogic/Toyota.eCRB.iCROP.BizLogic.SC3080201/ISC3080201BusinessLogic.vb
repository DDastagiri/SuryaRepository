'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ISC3080201BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客詳細共通処理
'補足： 
'作成： 2012/03/01 TCS 天野 【SALES_2】
'─────────────────────────────────────

Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
''' <summary>
''' 顧客詳細（顧客情報）のビジネスロジック用インターフェース
''' </summary>
''' <remarks></remarks>
Public Interface ISC3080201BusinessLogic

    ''' <summary>
    ''' 顧客職業登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客職業を登録する処理</remarks>
    Function InsertCstOccupation(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstOccupationDataTable) As Boolean

    ''' <summary>
    ''' 顧客家族構成登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客家族構成を登録する処理</remarks>
    Function InsertCstFamily(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstFamilyDataTable) As Boolean

    ''' <summary>
    ''' 顧客趣味登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客趣味を登録する処理</remarks>
    Function InsertCstHobby(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstHobbyDataTable) As Boolean

    ''' <summary>
    ''' 希望コンタクト方法登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>希望コンタクト方法を登録する処理</remarks>
    Function InsertCstContactInfo(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstContactInfoDataTable) As Boolean

    ''' <summary>
    ''' 希望連絡時間登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>希望連絡時間を登録する処理</remarks>
    Function InsertCstContactTime(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstContactInfoDataTable) As Boolean

    ''' <summary>
    ''' 希望連絡曜日登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>希望連絡曜日を登録する処理</remarks>
    Function InsertCstContactWeekOfDay(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstContactInfoDataTable) As Boolean

    ''' <summary>
    ''' 顔写真登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>希望連絡情報を登録する処理</remarks>
    Function InsertImageFile(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertImageFileDataTable) As Boolean

    ''' <summary>
    ''' 商談・一時対応・営業活動開始処理
    ''' </summary>
    ''' <param name="dtParam">データテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>商談・一時対応・営業活動開始処理</remarks>
    Function StartVisitSales(ByVal dtParam As SC3080201DataSet.SC3080201SalesStartDataTable,
                                   ByRef msgId As Integer) As Boolean


    ''' <summary>
    ''' 商談・一時対応・営業活動終了処理
    ''' </summary>
    ''' <param name="dtParam">データテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>商談・一時対応・営業活動終了処理</remarks>
    Function EndVisitSales(ByVal dtParam As SC3080201DataSet.SC3080201SalesStartDataTable, ByRef msgId As Integer) As Boolean


End Interface
