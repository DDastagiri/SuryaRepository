'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290103BusinessLogic.vb
'─────────────────────────────────────
'機能： 異常詳細画面ビジネスロジック
'補足： 
'作成： 2014/06/12 TMEJ y.gotoh
'更新： 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Text
Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess

Public Class SC3290103BusinessLogic
    Inherits BaseBusinessComponent

    ''' <summary>
    ''' 異常詳細一覧の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="irregClassCode">異常分類コード</param>
    ''' <param name="irregItemCode">異常分類コード</param>
    ''' <param name="todayDate">本日日付</param>
    ''' <returns>異常情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetIrregularDetailList(ByVal dealerCode As String, ByVal branchCode As String, _
                                           ByVal irregClassCode As String, ByVal irregItemCode As String, _
                                           ByVal todayDate As Date) As SC3290103DataSet.IrregularDetailInfoDataTable

        Dim startLog As New StringBuilder
        With startLog
            .Append("GetIrregularDetailList_Start ")
            .Append("dealerCode[" & dealerCode & "]")
            .Append(",branchCode[" & branchCode & "]")
            .Append(",irregClassCd[" & irregClassCode & "]")
            .Append(",irregItemCd[" & irregItemCode & "]")
        End With
        Logger.Info(startLog.ToString)


        Dim dt As SC3290103DataSet.IrregularDetailInfoDataTable

        Using da As New SC3290103DataSetTableAdapters.SC3290103TableAdapter

            '異常詳細情報取得
            dt = da.GetIrregularDetailList(dealerCode, branchCode, irregClassCode, irregItemCode, todayDate)

        End Using

        '結果返却
        Dim endLog As New StringBuilder
        With endLog
            .Append("GetIrregularDetailList_End Ret:[Count:")
            .Append(dt.Rows.Count)
            .Append("] ")
        End With
        Logger.Info(endLog.ToString)

        Return dt

    End Function

    ''' <summary>
    ''' 異常項目名表示名称取得
    ''' </summary>
    ''' <param name="irregClassCode">異常分類コード</param>
    ''' <param name="irregItemCode">異常分類コード</param>
    ''' <returns>異常項目名表示名称</returns>
    ''' <remarks></remarks>
    Public Function GetIrregularItemDisplayName(ByVal irregClassCode As String, _
                                                ByVal irregItemCode As String) As String

        Dim startLog As New StringBuilder
        With startLog
            .Append("GetIrregularItemDisplayName_Start ")
            .Append(",irregClassCd[" & irregClassCode & "]")
            .Append(",irregItemCd[" & irregItemCode & "]")
        End With
        Logger.Info(startLog.ToString)


        Dim irregularItemDisplayName As String

        Using da As New SC3290103DataSetTableAdapters.SC3290103TableAdapter

            '異常項目名表示名称取得
            irregularItemDisplayName = da.GetIrregularItemDisplayName(irregClassCode, irregItemCode)

        End Using

        '結果返却
        Dim endLog As New StringBuilder
        With endLog
            .Append("GetIrregularItemDisplayName_End Ret:[")
            .Append(irregularItemDisplayName)
            .Append("] ")
        End With
        Logger.Info(endLog.ToString)

        Return irregularItemDisplayName

    End Function

End Class
