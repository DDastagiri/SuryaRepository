'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3010204.aspx.vb
'─────────────────────────────────────
'機能： SCメイン(KPI)
'補足： 
'作成： 
'更新： 2014/02/19 TCS 高橋 受注後フォロー機能開発
'─────────────────────────────────────
Option Strict On
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Data
Imports System.Globalization
Imports Toyota.eCRB.Common.MainMenu.BizLogic.SC3010204
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010204

Partial Class Pages_SC3010204
    Inherits BasePage


    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>Postで呼ばれることを想定していないのでisPostBack判定は行いません</remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info("Page_Load Start")

        Dim stf As StaffContext = StaffContext.Current
        Dim biz As New SC3010204BusinessLogic
        Dim ds As DataSet = biz.SelectProcessKpi()
        '日付
        Dim dtDate As SC3010204DataSet.SalesKpiTgtDateDataTable _
            = CType(ds.Tables("SalesKpiTgtDateDataTable"), SC3010204DataSet.SalesKpiTgtDateDataTable)

        '項目名
        Dim dtSelectProcessKpiItem As SC3010204DataSet.SalesKpiItemDataTable _
            = CType(ds.Tables("SalesKpiItemDataTable"), SC3010204DataSet.SalesKpiItemDataTable)
        '日別
        Dim kpiSumDay As SC3010204DataSet.SalesKpiSummaryDataTable _
            = CType(ds.Tables("SalesKpiSummaryDataTable"), SC3010204DataSet.SalesKpiSummaryDataTable)
        '月間
        Dim kpiMonth As SC3010204DataSet.MonthlyKpiDataTable _
            = CType(ds.Tables("MonthlyKpiDataTable"), SC3010204DataSet.MonthlyKpiDataTable)

        '日付とカラム位置の紐付用Dictionary作成
        Dim colIdxDic As New Dictionary(Of Integer, Integer)
        Dim colIdx As Integer = 1
        For Each dr As SC3010204DataSet.SalesKpiTgtDateRow In dtDate.Rows
            colIdxDic.Add(dr.TGT_DATE.Day, colIdx)
            colIdx += 1
        Next

        'タイトル行に日付を表示
        Me.Row0Value1Name.Text = CType(dtDate.Rows(0), SC3010204DataSet.SalesKpiTgtDateRow).TGT_DATE.Day.ToString(CultureInfo.CurrentCulture)
        Me.Row0Value2Name.Text = CType(dtDate.Rows(1), SC3010204DataSet.SalesKpiTgtDateRow).TGT_DATE.Day.ToString(CultureInfo.CurrentCulture)
        Me.Row0Value3Name.Text = CType(dtDate.Rows(2), SC3010204DataSet.SalesKpiTgtDateRow).TGT_DATE.Day.ToString(CultureInfo.CurrentCulture)
        Me.Row0Value4Name.Text = CType(dtDate.Rows(3), SC3010204DataSet.SalesKpiTgtDateRow).TGT_DATE.Day.ToString(CultureInfo.CurrentCulture)

        '行毎のラベル配列作成
        Dim row1() As Controls.CustomLabel = {Me.Row1Title, Me.Row1Value1, Me.Row1Value2, Me.Row1Value3, Me.Row1Value4, Me.Row1Value5}
        Dim row2() As Controls.CustomLabel = {Me.Row2Title, Me.Row2Value1, Me.Row2Value2, Me.Row2Value3, Me.Row2Value4, Me.Row2Value5}
        Dim row3() As Controls.CustomLabel = {Me.Row3Title, Me.Row3Value1, Me.Row3Value2, Me.Row3Value3, Me.Row3Value4, Me.Row3Value5}
        Dim row4() As Controls.CustomLabel = {Me.Row4Title, Me.Row4Value1, Me.Row4Value2, Me.Row4Value3, Me.Row4Value4, Me.Row4Value5}
        Dim row5() As Controls.CustomLabel = {Me.Row5Title, Me.Row5Value1, Me.Row5Value2, Me.Row5Value3, Me.Row5Value4, Me.Row5Value5}
        Dim row6() As Controls.CustomLabel = {Me.Row6Title, Me.Row6Value1, Me.Row6Value2, Me.Row6Value3, Me.Row6Value4, Me.Row6Value5}
        Dim row7() As Controls.CustomLabel = {Me.Row7Title, Me.Row7Value1, Me.Row7Value2, Me.Row7Value3, Me.Row7Value4, Me.Row7Value5}
        Dim row8() As Controls.CustomLabel = {Me.Row8Title, Me.Row8Value1, Me.Row8Value2, Me.Row8Value3, Me.Row8Value4, Me.Row8Value5}

        Dim labelList As New List(Of Controls.CustomLabel())
        labelList.AddRange({row1, row2, row3, row4, row5, row6, row7, row8})
        Dim rowCnt As Integer = 0
        For Each dr As SC3010204DataSet.SalesKpiItemRow In dtSelectProcessKpiItem
            If labelList.Count <= rowCnt Then
                '表示は8行目以降に相当するデータがあっても表示しない
                Exit For
            End If

            If Not (dr.IsSALES_KPI_ITEMNull OrElse " ".Equals(dr.SALES_KPI_ITEM)) Then
                'KPI活用指標（名称）表示

                labelList(rowCnt)(0).Text = GetKpiItemName(dr)
            End If

            '日別の集計値を表示
            Dim daily() As DataRow = kpiSumDay.Select("SALES_KPI_ITEM_CD = '" & dr.SALES_KPI_ITEM_CD & "'", "TGT_DATE")
            For Each drDaily As SC3010204DataSet.SalesKpiSummaryRow In daily
                If colIdxDic.ContainsKey(drDaily.TGT_DATE.Day) Then
                    If String.IsNullOrEmpty(labelList(rowCnt)(colIdxDic(drDaily.TGT_DATE.Day)).Text) Then
                        labelList(rowCnt)(colIdxDic(drDaily.TGT_DATE.Day)).Text = drDaily.SUM_VAL.ToString(CultureInfo.CurrentCulture)
                        labelList(rowCnt)(colIdxDic(drDaily.TGT_DATE.Day)).CssClass &= " rightTx"
                    Else
                        Dim sumVal As Decimal = 0
                        Decimal.TryParse(labelList(rowCnt)(colIdxDic(drDaily.TGT_DATE.Day)).Text, sumVal)
                        labelList(rowCnt)(colIdxDic(drDaily.TGT_DATE.Day)).Text = (sumVal + drDaily.SUM_VAL).ToString(CultureInfo.CurrentCulture)
                    End If
                End If
            Next

            '月間の集計値を表示
            Dim monthly() = kpiMonth.Select("SALES_KPI_ITEM_CD = '" & dr.SALES_KPI_ITEM_CD & "'")
            If monthly IsNot Nothing AndAlso monthly.Length > 0 Then
                Dim drMonthly As SC3010204DataSet.MonthlyKpiRow = CType(monthly(0), SC3010204DataSet.MonthlyKpiRow)
                labelList(rowCnt)(labelList(rowCnt).Length - 1).Text = drMonthly.SUM_VAL.ToString(CultureInfo.CurrentCulture)
                labelList(rowCnt)(labelList(rowCnt).Length - 1).CssClass &= " rightTx"
            End If

            '対象行で数字が埋まっていないマスにハイフンを表示
            For Each lbl In labelList(rowCnt)
                If String.IsNullOrEmpty(lbl.Text) Then
                    lbl.Text = "-"
                    lbl.CssClass &= " centerTx"
                End If
            Next
            rowCnt += 1
        Next

        Logger.Info("Page_Load End")
    End Sub

    ''' <summary>
    ''' KPI項目名称を返す
    ''' </summary>
    ''' <param name="dr">活用指標KPI項目マスタ</param>
    ''' <returns>活用指標KPI項目名</returns>
    ''' <remarks></remarks>
    Private Function GetKpiItemName(dr As SC3010204DataSet.SalesKpiItemRow) As String

        Dim kpiItemName As String = String.Empty
        Dim replaceValue As String = String.Empty

        Select Case dr.SALES_KPI_ITEM_CD
            Case "007"
                'N分超過接客数

                Dim biz As New SC3010204BusinessLogic
                replaceValue = biz.GetWaitOverMin
                kpiItemName = String.Format(dr.SALES_KPI_ITEM, replaceValue)
            Case "008"
                'N日以上計画数

                Dim biz As New SC3010204BusinessLogic
                replaceValue = biz.GetPlanOverDays
                kpiItemName = String.Format(dr.SALES_KPI_ITEM, replaceValue)
            Case Else
                '上記以外
                kpiItemName = dr.SALES_KPI_ITEM
        End Select

        Return kpiItemName
    End Function
End Class
