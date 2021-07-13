'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3130501BusinessLogic.vb
'─────────────────────────────────────
'機能： 受付待ち画面(受付データ参照)
'補足： 
'作成：            SKFC 久代 【A. STEP1】
'更新： 2013/03/27 SKFC 久代 【A. STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic
Imports System.Globalization
Imports Toyota.eCRB.iCROP.DataAccess.SC3130501

Public Class SC3130501BusinessLogic
    Inherits BaseBusinessComponent



#Region "処理"

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' 受付データ取得
    ''' </summary>
    ''' <returns>受付データ(JSON形式)</returns>
    ''' <remarks></remarks>
    Public Function GetDisplayData() As String
        Dim context As StaffContext = StaffContext.Current
        ' 呼出中データ取得
        Dim displayData As SC3130501DataSet.SC3130501DisplayDataDataTable
        displayData = SC3130501DataSetTableAdapters.SC3130501TableAdapter.GetCalleeList(context.DlrCD, context.BrnCD)
        ' 呼出待ち人数取得
        Dim waitNumberData As SC3130501DataSet.SC3130501WaitNumberDataTable
        waitNumberData = SC3130501DataSetTableAdapters.SC3130501TableAdapter.GetWaitNumber(context.DlrCD, context.BrnCD)

        ' 問合せ結果をJSONデータに落し込む
        Dim resultJson As String
        resultJson = "{"

        ' 呼出待ち人数
        resultJson &= " ""waitNumber"": " & waitNumberData.Item(0).WAITNUMBER & " ,"

        ' 今回の呼出データ(最終データ)
        resultJson &= " ""stackCallee"":"
        If displayData.Count > 0 Then
            resultJson &= " { ""number"":""" & displayData.Last.CALLNO & """, "
            resultJson &= """place"":""" & displayData.Last.CALLPLACE & """, "
            resultJson &= """saName"":""" & displayData.Last.STF_NAME & """ },"
        Else
            resultJson &= " { ""number"":"""", ""place"":"""", ""saName"":"""" },"
        End If

        ' 呼出済データ(最終データを除く呼出データ)
        resultJson &= " ""afterCallee"":["

        For i As Integer = 0 To (displayData.Count - 2)
            resultJson &= " { ""number"":""" & displayData.Item(i).CALLNO & """, "
            resultJson &= """place"":""" & displayData.Item(i).CALLPLACE & """, "
            resultJson &= """saName"":""" & displayData.Item(i).STF_NAME & """ }"
            If i < (displayData.Count - 2) Then
                resultJson &= ","
            End If
        Next

        resultJson &= "]}"

        Return (resultJson)
    End Function

    ''' <summary>
    ''' 日付フォーマット取得
    ''' </summary>
    ''' <param name="cntcd">国コード</param>
    ''' <returns>日付フォーマット配列</returns>
    ''' <remarks></remarks>
    Public Function GetDateFormat(ByVal cntcd As String) As String()
        ' 日付フォーマットデータ取得
        Dim dateFormatDataSet As SC3130501DataSet.SC3130501DateFormatDataTable
        dateFormatDataSet = SC3130501DataSetTableAdapters.SC3130501TableAdapter.GetDateFormat(cntcd)

        Dim dateFormatList(dateFormatDataSet.Count - 1) As String
        ' 初期化
        For i As Integer = 0 To (dateFormatDataSet.Count - 1)
            dateFormatList(i) = New String("")
        Next

        ' CONVID - 1をインデックスとしてフォーマットデータをセットする
        For i As Integer = 0 To (dateFormatDataSet.Count - 1)
            dateFormatList(dateFormatDataSet.Item(i).CONVID - 1) = dateFormatDataSet.Item(i).FORMAT
        Next

        Return dateFormatList
    End Function

#End Region


End Class
