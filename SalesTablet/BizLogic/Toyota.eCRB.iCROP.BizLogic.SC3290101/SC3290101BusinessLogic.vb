'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290101BusinessLogic.vb
'─────────────────────────────────────
'機能： 異常リストビジネスロジック
'補足： 
'作成： 2014/06/13 TMEJ y.gotoh
'更新： 
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess
Imports System.Text

Public Class SC3290101BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' 異常分類コード：担当スタッフ未振当て
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregClassCdFuriateStaffNot As String = "10"

    ''' <summary>
    ''' 異常分類コード：活動遅れ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregClassCdDelayActivity As String = "30"

    ''' <summary>
    ''' 異常項目コード：顧客担当
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregItemCdCustomerRepresentative As String = "01"

    ''' <summary>
    ''' 異常項目コード：活動担当
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregItemCdActivityResponsible As String = "02"

    ''' <summary>
    ''' 異常項目コード：受注前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregItemCdOrdersBefore As String = "01"

    ''' <summary>
    ''' 異常項目コード：受注後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregItemCdAfterReceiptOfOrder As String = "02"

    ''' <summary>
    ''' 異常項目コード：納車後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IrregItemCdDeliveredAfter As String = "03"

    ''' <summary>
    ''' 受注後活動区分：受注後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterOdrActTypeAfterReceiptOfOrder As String = "1"

    ''' <summary>
    ''' 受注後活動区分：納車後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AfterOdrActTypeDeliveredAfter As String = "2"
#End Region

#Region "公開メソッド"
    ''' <summary>
    ''' 異常情報一覧の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="todayDate">本日日付</param>
    ''' <returns>更新日時</returns>
    ''' <remarks></remarks>
    Public Function GetIrregularList(ByVal dealerCode As String, ByVal branchCode As String, _
                                     ByVal todayDate As Date) As SC3290101DataSet.IrregularInfoDataTable

        Dim startLog As New StringBuilder
        With startLog
            .Append("GetIrregularList_Start ")
            .Append("dealerCode[" & dealerCode & "]")
            .Append(",branchCode[" & branchCode & "]")
        End With
        Logger.Info(startLog.ToString)

        Dim sortDt As SC3290101DataSet.IrregularInfoDataTable = Nothing

        Using da As New SC3290101DataSetTableAdapters.SC3290101TableAdapter

            '異常スタッフ数、異常件数が共に0のものを除いた異常情報格納用
            Using afterDeleteDt As SC3290101DataSet.IrregularInfoDataTable = New SC3290101DataSet.IrregularInfoDataTable()

                '表示対象異常項目一覧
                Using irregularInfoDt As SC3290101DataSet.IrregularInfoDataTable = da.GetDisplayIrregularList(dealerCode, branchCode)

                    For Each row As SC3290101DataSet.IrregularInfoRow In irregularInfoDt.Rows

                        Dim irregClassCode As String = row.IRREG_CLASS_CD
                        Dim irregItemCode As String = row.IRREG_ITEM_CD

                        '担当スタッフ未振当て、活動遅れの情報を設定
                        If IrregClassCdFuriateStaffNot.Equals(irregClassCode) Then

                            '担当スタッフ未振当ての場合
                            afterDeleteDt.ImportRow(row)

                        ElseIf IrregClassCdDelayActivity.Equals(irregClassCode) _
                            AndAlso IrregItemCdOrdersBefore.Equals(irregItemCode) Then

                            '受注前の活動遅れの場合

                            '受注前活動遅れの件数を設定
                            SetBeforeOrderDelayActivityCount(dealerCode, branchCode, da, row, todayDate)

                        ElseIf IrregClassCdDelayActivity.Equals(irregClassCode) _
                            AndAlso IrregItemCdAfterReceiptOfOrder.Equals(irregItemCode) Then

                            '受注後の活動遅れの場合

                            '受注後活動遅れの件数を設定
                            SetAfterOrderDelayActivity(dealerCode, branchCode, da, row, AfterOdrActTypeAfterReceiptOfOrder, todayDate)

                        ElseIf IrregClassCdDelayActivity.Equals(irregClassCode) _
                            AndAlso IrregItemCdDeliveredAfter.Equals(irregItemCode) Then

                            '納車後の活動遅れの場合

                            '受注後活動遅れの件数を設定
                            SetAfterOrderDelayActivity(dealerCode, branchCode, da, row, AfterOdrActTypeDeliveredAfter, todayDate)

                        End If

                    Next

                    Dim goalUnachievedDt As SC3290101DataSet.IrregularInfoDataTable

                    '目標未達情報取得
                    goalUnachievedDt = da.GetGoalUnachieved(dealerCode, branchCode, todayDate)

                    If 0 < goalUnachievedDt.Rows.Count Then
                        irregularInfoDt.Merge(goalUnachievedDt)
                    End If

                    Dim planningAbnormalDt As SC3290101DataSet.IrregularInfoDataTable

                    '計画異常情報取得
                    planningAbnormalDt = da.GetPlanningAbnormal(dealerCode, branchCode)

                    If 0 < planningAbnormalDt.Rows.Count Then
                        irregularInfoDt.Merge(planningAbnormalDt)
                    End If


                    '異常情報一覧から異常スタッフ数・異常件数が共に0のレコードを削除
                    '担当スタッフ未振当ての情報は後から非同期で取得するため、行を削除しない
                    For Each row As SC3290101DataSet.IrregularInfoRow In irregularInfoDt.Rows

                        Dim irregStaffCount As Integer = row.IRREG_STAFF_COUNT
                        Dim irregCount As Integer = row.IRREG_COUNT

                        If Not irregStaffCount = 0 OrElse Not irregCount = 0 Then
                            afterDeleteDt.ImportRow(row)
                        End If
                    Next


                    'ソートを行う
                    sortDt = SortDataTable(afterDeleteDt)

                End Using
            End Using
        End Using

        '結果返却
        Dim endLog As New StringBuilder
        With endLog
            .Append("GetIrregularList_End Ret:[")
            .Append("Count:")
            .Append(sortDt.Rows.Count)
            .Append("] ")
        End With
        Logger.Info(endLog.ToString)

        Return sortDt

    End Function

    ''' <summary>
    ''' 更新日時の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <returns>更新日時</returns>
    ''' <remarks></remarks>
    Public Function GetUpdatetime(ByVal dealerCode As String, ByVal branchCode As String) As Date

        Dim startLog As New StringBuilder
        With startLog
            .Append("GetUpdatetime_Start ")
            .Append("dealerCode[" & dealerCode & "]")
            .Append(",branchCode[" & branchCode & "]")
        End With
        Logger.Info(startLog.ToString)


        Dim updateTime As Date

        Using da As New SC3290101DataSetTableAdapters.SC3290101TableAdapter

            '異常詳細情報取得
            updateTime = da.GetUpdatetime(dealerCode, branchCode)

        End Using

        '結果返却
        Dim endLog As New StringBuilder
        With endLog
            .Append("GetUpdatetime_End Ret:[")
            .Append(updateTime)
            .Append("] ")
        End With
        Logger.Info(endLog.ToString)

        Return updateTime

    End Function

#End Region

#Region "非公開メソッド"

    ''' <summary>
    ''' 受注前活動遅れの件数をセットする
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="da">テーブルアダプタ</param>
    ''' <param name="row">表示対象異常情報</param>
    ''' <remarks></remarks>
    Private Sub SetBeforeOrderDelayActivityCount(ByVal dealerCode As String, ByVal branchCode As String, _
                                                 ByVal da As SC3290101DataSetTableAdapters.SC3290101TableAdapter, _
                                                 ByRef row As SC3290101DataSet.IrregularInfoRow, _
                                                 ByVal todayDate As Date)

        Dim beforeOrderDelayActivityDt As SC3290101DataSet.ActivityDelayInfoDataTable

        '受注前活動遅れ情報の取得
        beforeOrderDelayActivityDt = da.GetBeforeOrderDelayActivity(dealerCode, branchCode, todayDate)

        If 0 < beforeOrderDelayActivityDt.Rows.Count Then
            Dim beforeOrderDelayActivityRow As SC3290101DataSet.ActivityDelayInfoRow
            beforeOrderDelayActivityRow = CType(beforeOrderDelayActivityDt.Rows(0), SC3290101DataSet.ActivityDelayInfoRow)
            row.IRREG_STAFF_COUNT = beforeOrderDelayActivityRow.IRREG_STAFF_COUNT
            row.IRREG_COUNT = beforeOrderDelayActivityRow.IRREG_COUNT
        End If
    End Sub

    ''' <summary>
    ''' 受注後活動遅れの件数をセットする
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="da">テーブルアダプタ</param>
    ''' <param name="row">表示対象異常情報</param>
    ''' <param name="afterOdrActType">受注後活動区分(1：受注後、2：納車後)</param>
    ''' <remarks></remarks>
    Private Sub SetAfterOrderDelayActivity(ByVal dealerCode As String, ByVal branchCode As String, _
                                           ByVal da As SC3290101DataSetTableAdapters.SC3290101TableAdapter, _
                                           ByRef row As SC3290101DataSet.IrregularInfoRow, _
                                           ByVal afterOdrActType As String, _
                                           ByVal todayDate As Date)

        Dim afterOrderDelayActivityDt As SC3290101DataSet.ActivityDelayInfoDataTable

        '受注後活動遅れ情報の取得
        afterOrderDelayActivityDt = da.GetAfterOrderDelayActivity(dealerCode, branchCode, _
                                                                  todayDate, _
                                                                  afterOdrActType)
        If 0 < afterOrderDelayActivityDt.Rows.Count Then
            Dim afterOrderDelayActivityRow As SC3290101DataSet.ActivityDelayInfoRow
            afterOrderDelayActivityRow = CType(afterOrderDelayActivityDt.Rows(0), SC3290101DataSet.ActivityDelayInfoRow)
            row.IRREG_STAFF_COUNT = afterOrderDelayActivityRow.IRREG_STAFF_COUNT
            row.IRREG_COUNT = afterOrderDelayActivityRow.IRREG_COUNT
        End If

    End Sub

    ''' <summary>
    ''' データテーブルのソートを行う
    ''' </summary>
    ''' <param name="irregularInfo">ソート対象のデータテーブル</param>
    ''' <returns>ソート後のデータテーブル</returns>
    ''' <remarks></remarks>
    Private Function SortDataTable(ByVal irregularInfo As SC3290101DataSet.IrregularInfoDataTable) As SC3290101DataSet.IrregularInfoDataTable

        Using sortDt As SC3290101DataSet.IrregularInfoDataTable = New SC3290101DataSet.IrregularInfoDataTable()

            '表示対象異常項目の一覧を並び順の昇順にてソート
            Using dv As DataView = New DataView(irregularInfo)

                dv.Sort = "SORT_ORDER"

                For Each drv As DataRowView In dv
                    sortDt.ImportRow(drv.Row)
                Next
            End Using

            Return sortDt
        End Using
    End Function
#End Region

End Class
